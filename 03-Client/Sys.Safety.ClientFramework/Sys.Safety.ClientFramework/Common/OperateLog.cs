using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;
using System.Collections;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Operatelog;
using Basic.Framework.Common;
using System.IO;
using Basic.Framework.Logging;
using Sys.Safety.DataContract.UserRoleAuthorize;

namespace Sys.Safety.ClientFramework.CBFCommon
{
    /// <summary>
    /// 系统操作日志帮助类
    /// </summary>
    public class OperateLogHelper
    {
        static IOperatelogService operatelogService = ServiceFactory.Create<IOperatelogService>();
        /// <summary>
        /// 是否运行
        /// </summary>
        static bool _isRun = false;
        /// <summary>
        /// 缓存对象
        /// </summary>
        static List<OperatelogInfo> _OperateLogCache = new List<OperatelogInfo>();
        /// <summary>
        /// 写失败的操作日志
        /// </summary>
        static List<OperatelogInfo> _FailOperateLogCache = new List<OperatelogInfo>();
        /// <summary>
        /// 对象锁
        /// </summary>
        static object _objLocker = new object();
        /// <summary>
        /// 数据库状态
        /// </summary>
        static bool DBState = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        static OperateLogHelper()
        {
            if (!_isRun)
            {
                Start();
            }
        }
        /// <summary>
        /// 启动插入线程
        /// </summary>
        private static void Start()
        {
            if (_isRun)
            {
                return;
            }

            _isRun = true;

            Thread t = new Thread(new ParameterizedThreadStart(InsertOperateLogToDB));
            t.IsBackground = true;
            t.Start();
            Thread t1 = new Thread(new ParameterizedThreadStart(CacheOperateLogToDB));
            t.IsBackground = true;
            t1.Start();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            _isRun = false;
        }
        /// <summary>
        /// 增加操作日志
        /// </summary>
        /// <param name="dto">操作日志对象</param>
        public static void InsertOperateLog(List<OperatelogInfo> dtos)
        {
            try
            {
                if (dtos.Count < 1)
                {
                    return;
                }
                OperatelogAddListRequest operatelogrequest = new OperatelogAddListRequest();
                operatelogrequest.OperatelogInfo = dtos;
                operatelogService.AddOperatelogs(operatelogrequest);

            }
            catch (Exception ex)
            {
                LogHelper.Error("OperateLogHelper-InsertOperateLog" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 增加操作日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="OperationContent">日志内容</param>
        /// <param name="Remark">备注信息</param>
        /// <returns></returns>
        public static void InsertOperateLog(int type, string OperationContent, string Remark)
        {
            try
            {
                OperatelogInfo opdto = new OperatelogInfo();
                opdto.OperateLogID = IdHelper.CreateLongId().ToString();
                //opdto.UserName = ClientContext.Current.ClientItem.UserName;
                ClientItem clientItem = new ClientItem();
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                {
                    clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                }               
                opdto.UserName = clientItem.UserName;
                opdto.LoginIP = Basic.Framework.Common.HardwareHelper.GetIPAddress();
                opdto.Type = type;
                opdto.OperationContent = OperationContent;
                opdto.CreateTime = DateTime.Now;
                opdto.Remark = Remark;
                //InsertOperateLog(opdto);
                lock (_objLocker)
                {
                    _OperateLogCache.Add(opdto);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OperateLogHelper-InsertOperateLog" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 异步写操作日志
        /// </summary>
        /// <param name="obj"></param>
        private static void InsertOperateLogToDB(object obj)
        {
            List<OperatelogInfo> dataItem = new List<OperatelogInfo>();

            while (_isRun)
            {
                lock (_objLocker)
                {
                    if (_OperateLogCache.Count > 0)
                    {
                        dataItem.AddRange(_OperateLogCache);
                        _OperateLogCache.Clear();
                    }
                }
                if (dataItem == null || dataItem.Count <= 0)
                {
                    //没有数据则休眠
                    Thread.Sleep(1000);
                    continue;
                }
                else
                {
                    try
                    {
                        InsertOperateLog(dataItem);
                        DBState = true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("InsertOperateLogToDB" + ex.Message + ex.StackTrace);
                        //异常处理，当插入异常时，对数据进行缓存 
                        _FailOperateLogCache.AddRange(dataItem);
                        int CacheToFileCount = int.Parse(ConfigurationManager.AppSettings["LocalCachePath"].ToString());
                        if (_FailOperateLogCache.Count > CacheToFileCount)//当失败的内存缓存数量达到一定数量时，进行文件缓存
                        {
                            String DtoStr = JSONHelper.ToJSONString(_FailOperateLogCache);
                            string FilePath = ConfigurationManager.AppSettings["LocalCachePath"].ToString();
                            string FileName = "OperateLog";
                            //Basic.Framework.Utils.TxtReadWrite.TxtReadWrite.Write(FilePath, DtoStr, FileName, true);
                            string FileFullName = FilePath + "\\" + FileName + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")
                  + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".txt";

                            byte[] myByte = System.Text.Encoding.UTF8.GetBytes(DtoStr);
                            using (FileStream fsWrite = new FileStream(FileName, FileMode.Append))
                            {
                                fsWrite.Write(myByte, 0, myByte.Length);
                            };
                            _FailOperateLogCache.Clear();
                        }
                    }
                    finally
                    {
                        if (dataItem.Count > 0)
                        {
                            dataItem.Clear();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 缓存入库
        /// </summary>
        /// <param name="obj"></param>
        private static void CacheOperateLogToDB(object obj)
        {
            List<OperatelogInfo> dataItem = null;

            while (_isRun)
            {
                if (DBState)
                {
                    try
                    {
                        string FilePath = ConfigurationManager.AppSettings["LocalCachePath"].ToString();
                        //List<string> OperateLogCacheFiles = Basic.Framework.Utils.TxtReadWrite.TxtReadWrite.GetPathTxt(FilePath, "OperateLog");
                        if (Directory.Exists(FilePath) == false)//如果不存在就创建file文件夹
                        {
                            return;//如果目录不存在，直接退出
                        }
                        DirectoryInfo dirinfo = new DirectoryInfo(FilePath);
                        FileInfo[] OperateLogCacheFiles = dirinfo.GetFiles();
                        foreach (FileInfo TempFile in OperateLogCacheFiles)
                        {
                            string myStr = string.Empty;
                            using (FileStream fsRead = new FileStream(TempFile.FullName, FileMode.Open))
                            {
                                int fsLen = (int)fsRead.Length;
                                byte[] heByte = new byte[fsLen];
                                int r = fsRead.Read(heByte, 0, heByte.Length);
                                myStr = System.Text.Encoding.UTF8.GetString(heByte);
                            }
                            if (!string.IsNullOrEmpty(myStr))
                            {
                                dataItem = JSONHelper.ParseJSONString<List<OperatelogInfo>>(myStr);
                            }
                            try
                            {
                                InsertOperateLog(dataItem);
                                //写入成功后删除文件
                                File.Delete(TempFile.FullName);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error("CacheOperateLogToDB" + ex.Message + ex.StackTrace);
                            }
                            finally
                            {
                                if (dataItem.Count > 0)
                                {
                                    dataItem.Clear();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("CacheOperateLogToDB" + ex.Message + ex.StackTrace);
                    }
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 清除数据库中指定日期的操作日志数据
        /// </summary>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        public static void ClearOperateLog(DateTime stime, DateTime etime)
        {
            try
            {
                string strsql = "";
                if (stime == null)
                {
                    OperatelogDeleteByStimeEtimeRequest operatelogrequest = new OperatelogDeleteByStimeEtimeRequest();
                    operatelogrequest.Etime = etime;
                    operatelogService.DeleteOperatelogByEtime(operatelogrequest);
                }
                else
                {
                    OperatelogDeleteByStimeEtimeRequest operatelogrequest = new OperatelogDeleteByStimeEtimeRequest();
                    operatelogrequest.Etime = etime;
                    operatelogrequest.Stime = stime;
                    operatelogService.DeleteOperatelogByStimeEtime(operatelogrequest);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("OperateLogHelper-ClearOperateLog" + ex.Message + ex.StackTrace);
            }
        }
    }
}
