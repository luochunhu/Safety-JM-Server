using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sys.Safety.Processing.DataToDb
{
    public abstract class DataToDbManager<T> where T : BasicInfo
    {
        /// <summary>
        /// 操作列表
        /// </summary>
        protected List<T> InsertItemList = new List<T>();

        /// <summary>
        /// 是否启动线程
        /// </summary>
        protected bool IsRun = false;

        /// <summary>
        /// 取数条数
        /// </summary>
        protected int SendSqlCount = 500;

        /// <summary>
        /// 入库线程
        /// </summary>
        protected Thread InsertToDbThread;

        /// <summary>
        /// 补录线程
        /// </summary>
        protected Thread InsertLocalToDbThread;

        /// <summary>
        /// 失败数据生成文件路径
        /// </summary>
        protected string FilePath;

        /// <summary>
        /// 失败数据生成文件名
        /// </summary>
        protected string FileName;

        /// <summary>
        /// 本地缓存多少小时的历史数据
        /// </summary>
        protected int LocalFileHourCount = 24;

        /// <summary>
        /// 入库多线程单例锁
        /// </summary>
        protected static readonly object obj = new object();

        protected static volatile DataToDbManager<T> _instance;

        protected long _totalRecord = 0;

        /// <summary>
        /// 入库操作
        /// </summary>
        protected void DoTransfer()
        {
            while (IsRun || InsertItemList.Any())
            {
                try
                {
                    List<T> dolist = new List<T>();
                    lock (this)
                    {
                        if (InsertItemList.Count > 0 && InsertItemList.Count <= SendSqlCount)
                        {
                            dolist.AddRange(InsertItemList);
                            InsertItemList.Clear();
                        }
                        else if (InsertItemList.Count > SendSqlCount)
                        {
                            dolist.AddRange(InsertItemList.GetRange(0, SendSqlCount));
                            InsertItemList.RemoveRange(0, SendSqlCount);
                        }
                    }

                    IEnumerable<IGrouping<InfoState, T>> group = dolist.GroupBy(p => p.InfoState);
                    List<T> listAddNew = new List<T>();
                    List<T> listModified = new List<T>();
                    foreach (IGrouping<InfoState, T> info in group)
                    {
                        if (info.Key == InfoState.AddNew)
                        {
                            listAddNew = info.ToList<T>();
                        }
                        if (info.Key == InfoState.Modified)
                        {
                            listModified = info.ToList<T>();
                        }
                    }

                    //添加或者修改都清除列表数据
                    //如果数据库连接失败，则写入本地文件;如果数据库连接成功但插入失败，记录异常日志
                    if (listAddNew.Count > 0)
                    {
                        AddItemsToDb(listAddNew);
                        listAddNew.Clear();
                    }
                    if (listModified.Count > 0)
                    {
                        UpdateItemsToDb(listModified);
                        listModified.Clear();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("数据处理入库失败:" + typeof(T).Name.ToString() + "\r\n" + ex.Message);
                }
                Thread.Sleep(200);
            }

            string typename = typeof(T).Name;
            LogHelper.Info(typename + "入库线程已退出！");
        }

        /// <summary>
        /// 补录操作
        /// </summary>
        protected void CollectionData()
        {
            while (IsRun)
            {
                try
                {
                    if (Directory.Exists(FilePath) == false)
                    {
                        return;
                    }
                    DirectoryInfo dirinfo = new DirectoryInfo(FilePath);
                    FileInfo[] files = dirinfo.GetFiles();
                    Array.Sort<FileInfo>(files, new FIleLastTimeComparer());
                    foreach (FileInfo file in files)
                    {
                        int index = file.Name.LastIndexOf('_');
                        string fileTimestr = file.Name.Substring(index + 1, 14);
                        DateTime fileTime;

                        if (DateTime.TryParseExact(fileTimestr, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out fileTime))
                        {
                            TimeSpan timeSpan = DateTime.Now - fileTime;
                            //如果文件保存时间大于24小时，则删除文件
                            if (timeSpan.TotalHours > LocalFileHourCount)
                            {
                                File.Delete(FilePath + file);
                                continue;
                            }
                            string myStr = string.Empty;
                            using (FileStream fsRead = new FileStream(file.FullName, FileMode.Open))
                            {
                                int fsLen = (int)fsRead.Length;
                                byte[] heByte = new byte[fsLen];
                                int r = fsRead.Read(heByte, 0, heByte.Length);
                                myStr = Encoding.UTF8.GetString(heByte);
                            }
                            //处理文件
                            if (!string.IsNullOrEmpty(myStr))
                            {
                                try
                                {
                                    List<T> listTemp = JSONHelper.ParseJSONString<List<T>>(myStr);
                                    if (listTemp != null && listTemp.Any())
                                    {
                                        bool issuccess = InsertLocalToDb(listTemp);
                                        //如果补录成功，则删除此文件
                                        if (issuccess)
                                            File.Delete(FilePath + file);
                                    }
                                    else
                                    {
                                        //如果数据为空，则删除此文件
                                        File.Delete(FilePath + file);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //如果文件解析失败，则删除此文件
                                    LogHelper.Error("文件解析失败：" + file.Name + ex.Message);
                                    File.Delete(FilePath + file);
                                }
                            }
                        }
                        else
                        {
                            File.Delete(FilePath + file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("入库失败数据补录失败：" + FilePath + "\r\n" + ex.Message);
                }
                Thread.Sleep(2000);
            }

            string typename = typeof(T).Name;
            LogHelper.Info(typename + "补录线程已退出！");
        }

        #region 外部接口

        /// <summary>
        /// 添加数据至列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public void AddItem(T item)
        {
            if (item != null && IsRun)
            {
                lock (this)
                {
                    _totalRecord++;
                    InsertItemList.Add(item);
                }
            }
        }

        /// <summary>
        /// 批量添加数据至列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public void AddItems(List<T> items)
        {
            if (items != null && items.Any() && IsRun)
            {
                lock (this)
                {
                    _totalRecord = _totalRecord + items.Count;
                    InsertItemList.AddRange(items);
                }
            }
        }
        /// <summary>
        /// 获取队列积压数
        /// </summary>
        public int GetInsertItemListCount()
        {
            return InsertItemList.Count;
        }

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public void Start()
        {
            if (IsRun)
            {
                return;
            }
            IsRun = true;

            if (InsertToDbThread == null || InsertToDbThread.ThreadState != ThreadState.Running)
            {
                InsertToDbThread = new Thread(DoTransfer);
                InsertToDbThread.Start();
            }
            if (InsertLocalToDbThread == null || InsertLocalToDbThread.ThreadState != ThreadState.Running)
            {
                InsertLocalToDbThread = new Thread(CollectionData);
                InsertLocalToDbThread.Start();
            }
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public void Stop()
        {
            string typename = typeof(T).Name;
            LogHelper.Info(typename + "入库、补录线程正在退出！");

            if (!IsRun)
            {
                return;
            }
            IsRun = false;

            while (InsertItemList.Any())
            {
                Thread.Sleep(100);
            }

            LogHelper.Info(typename + "入库、补录线程已退出！");
        }

        #endregion

        #region 需要子类重写的方法
        /// <summary>
        /// 添加集合至数据库
        /// </summary>
        protected abstract bool AddItemsToDb(List<T> addItems);

        /// <summary>
        /// 修改集合至数据库
        /// </summary>
        protected abstract bool UpdateItemsToDb(List<T> updateItems);

        /// <summary>
        ///添加本地缓存文件数据至数据库
        /// </summary>
        /// <param name="addLocalItems"></param>
        /// <returns></returns>
        protected abstract bool AddLocalDataToDb(List<T> addLocalItems);

        /// <summary>
        /// 更新本地缓存文件至数据库
        /// </summary>
        /// <param name="updateLocalItems"></param>
        /// <returns></returns>
        protected abstract bool UpdateLocalDataToDb(List<T> updateLocalItems);

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="nowtime">时间</param>
        /// <returns></returns>
        protected abstract string GetFileName(DateTime nowtime);

        #endregion

        /// <summary>
        /// 添加本地数据至数据库
        /// </summary>
        /// <param name="localItems"></param>
        /// <returns></returns>
        protected bool InsertLocalToDb(List<T> localItems)
        {
            bool issuccessful = false;
            //如果数据是AddNew，则执行添加操作;如果是Modify，则执行修改操作
            if (localItems[0].InfoState == InfoState.AddNew)
                issuccessful = AddLocalDataToDb(localItems);
            else if (localItems[0].InfoState == InfoState.Modified)
                issuccessful = UpdateLocalDataToDb(localItems);
            return issuccessful;
        }

        /// <summary>
        /// 添加失败数据至本地
        /// </summary>
        /// <param name="data"></param>
        protected void AddDataToLocal(List<T> data)
        {
            try
            {
                if (Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(FilePath);
                }
                //根据时间命名文件
                DateTime TempTime = DateTime.Now;
                string jsonString = JSONHelper.ToJSONString(data);
                byte[] myByte = Encoding.UTF8.GetBytes(jsonString);
                using (FileStream fsWrite = new FileStream(GetFileName(TempTime), FileMode.Create))
                {
                    fsWrite.Write(myByte, 0, myByte.Length);
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error("入库失败数据缓存至本地失败：" + FilePath + "\r\n" + ex.Message);
            }
        }

        /// <summary>
        /// 字符数组生成DataCloumn数组
        /// </summary>
        /// <param name="columnsName"></param>
        /// <returns></returns>
        protected DataColumn[] BuildDataColumn(string[] columnsName)
        {
            DataColumn[] dataColumns = new DataColumn[columnsName.Count()];
            for (int i = 0; i < columnsName.Count(); i++)
            {
                dataColumns[i] = new DataColumn();
                dataColumns[i].ColumnName = columnsName[i];
            }
            return dataColumns;
        }

        /// <summary>
        /// 文件排序
        /// </summary>
        protected class FIleLastTimeComparer : IComparer<FileInfo>
        {
            public int Compare(FileInfo x, FileInfo y)
            {
                return x.LastWriteTime.CompareTo(y.LastWriteTime);//递增
            }
        }

        /// <summary>
        /// 获取总量
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            return _totalRecord;
        }
    }
}
