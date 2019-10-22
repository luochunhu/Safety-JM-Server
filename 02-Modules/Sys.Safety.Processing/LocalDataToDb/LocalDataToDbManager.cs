using Basic.Framework.Common;
using Basic.Framework.JobSchedule;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Sys.Safety.Processing.LocalDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:数据处理入库失败数据补录线程基类
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public abstract class LocalDataToDbManager<T> : BasicTask where T : BasicInfo
    {
        public LocalDataToDbManager(string taskName, int interval)
            : base(taskName, interval)
        {

        }

        protected static readonly object obj = new object();

        /// <summary>
        /// 失败数据生成文件路径
        /// </summary>
        protected string FilePath;

        /// <summary>
        /// 本地缓存多少小时的历史数据
        /// </summary>
        protected int LocalFileHourCount = 24;

        protected override void DoWork()
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
            base.DoWork();
        }

        /// <summary>
        /// 添加本地数据至数据库
        /// </summary>
        /// <param name="localItems"></param>
        /// <returns></returns>
        protected bool InsertLocalToDb(List<T> localItems)
        {
            try
            {
                //如果数据是AddNew，则执行添加操作;如果是Modify，则执行修改操作
                if (localItems[0].InfoState == InfoState.AddNew)
                    return AddLocalDataToDb(localItems);
                else if (localItems[0].InfoState == InfoState.Modified)
                    return UpdateLocalDataToDb(localItems);
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("补录数据失败：" + "\r\n" + ex.Message);
                return false;
            }
        }

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
    }
}
