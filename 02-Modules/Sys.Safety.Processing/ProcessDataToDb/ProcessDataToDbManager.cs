using Basic.Framework.Common;
using Basic.Framework.JobSchedule;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sys.Safety.Processing.ProcessDataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-21
    /// 描述:入库线程基类
    /// 修改记录
    /// 2017-06-21
    /// </summary>
    public abstract class ProcessDataToDbManager<T> : BasicTask where T : BasicInfo
    {
        public ProcessDataToDbManager(string taskname, int interval)
            : base(taskname, interval)
        {
             
        }

        /// <summary>
        /// 操作列表
        /// </summary>
        protected List<T> InsertItemList = new List<T>();

        /// <summary>
        /// 取数条数
        /// </summary>
        protected int SendSqlCount = 100;

        /// <summary>
        /// 入库多线程单例锁
        /// </summary>
        protected static readonly object obj = new object();

        /// <summary>
        /// 失败数据生成文件路径
        /// </summary>
        protected string FilePath;

        /// <summary>
        /// 失败数据生成文件名
        /// </summary>
        protected string FileName;

        protected override void DoWork()
        {
            //如果存在数据，则一直处理，数据处理完成之后才可休眠
            while (InsertItemList.Any())
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
            base.DoWork();
        }

        public override void Stop()
        {
            //等待数据处理完成才可停止
            while (InsertItemList.Any()) 
            {
                Thread.Sleep(200);
            }

            //IsRunning = false;
        }

        public void AddItem(T item)
        {
            if (item != null && IsRunning)
            {
                lock (this)
                {
                    InsertItemList.Add(item);
                }
            }
        }

        public void AddItems(List<T> items)
        {
            if (items != null && items.Any() && IsRunning)
            {
                lock (this)
                {
                    InsertItemList.AddRange(items);
                }
            }
        }

        /// <summary>
        /// 添加集合至数据库
        /// </summary>
        protected abstract void AddItemsToDb(List<T> addItems);

        /// <summary>
        /// 更新集合至数据库
        /// </summary>
        protected abstract void UpdateItemsToDb(List<T> updateItems);

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="nowtime">时间</param>
        /// <returns></returns>
        protected abstract string GetFileName(DateTime nowtime);

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
    }
}
