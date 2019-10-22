using Basic.Framework.Logging;
using Sys.Safety.Interface;
using Sys.Safety.Processing.Rpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Processing.DataProcessing
{
    public delegate void OnLoadDriverSuccessEventHandler();
    public class DriverHandle
    {
        public event OnLoadDriverSuccessEventHandler OnLoadDriverSuccessEvent;
        /// <summary>
        /// 设备驱动字典
        /// 键：驱动编号
        /// 值：驱动对象
        /// </summary>
        public Dictionary<decimal, DriverItem> DriverItems = new Dictionary<decimal, DriverItem>();

        /// <summary>
        /// 本地驱动加载
        /// </summary>
        /// <returns></returns>
        public bool LoadLocalDrivers()
        {
            //日志
            string strLog = "";
            //驱动类名
            string fileName = "";
            //驱动程序集名称
            string assemblyName = "";
            //驱动文件所在路径
            //string path = Application.StartupPath + "\\Driver\\";
            string path = "";
            path = AppDomain.CurrentDomain.BaseDirectory + @"Driver\";
            //if (System.Environment.CurrentDirectory + "\\" == AppDomain.CurrentDomain.BaseDirectory)//Windows应用程序则相等
            //{
            //    path = AppDomain.CurrentDomain.BaseDirectory + @"\Driver\";
            //}
            //else
            //{
            //    path = AppDomain.CurrentDomain.BaseDirectory + @"Bin\Driver\";
            //}
            //加载是否成功
            bool flag = true;
            //驱动对象结构体 
            DriverItem drvObj;
            //驱动文件夹
            DirectoryInfo dir = new DirectoryInfo(path);
            //驱动程序集
            Assembly assembly;
            Type dllTypes;
            if (!System.IO.Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            #region 加载所有驱动
            try
            {
                if (dir.GetFiles("Sys.*.Driver.dll").Count() < 1)
                {
                    strLog = path + "没有驱动文件,程序将无法正常运行.............................................................";
                    LogHelper.Error(strLog);
                    return false;
                }
                foreach (System.IO.FileInfo file in dir.GetFiles("Sys.*.Driver.dll"))
                {
                    assembly = null;
                    drvObj = new DriverItem();
                    try
                    {
                        //加载程序集
                        assemblyName = file.Name;
                        if (assemblyName == "Sys.*.Driver.dll")
                        {
                            continue;
                        }
                        assembly = Assembly.LoadFrom(path + assemblyName);
                    }
                    catch
                    {
                        assembly = null;
                    }
                    if (assembly != null)
                    {
                        //驱动命名空间必须为DriverNameSpace  
                        //fileName = "Basic.*.Drivers." + file.Name.Substring(15).Replace(file.Extension, "");
                        fileName = assemblyName.Replace(file.Extension, "") + "." + assemblyName.Split('.')[1];
                        //获取程序集中的指定类型-即驱动对象类型
                        dllTypes = assembly.GetType(fileName);

                        if (dllTypes != null)
                        {
                            //根据程序集中的类型实例化对象
                            drvObj.DLLObj = Activator.CreateInstance(dllTypes) as IDriver;

                            //当为第一次加载或是该驱动对象在内存中不存在时，就添加到驱动内存中
                            if (DriverItems.Count == 0 ||
                                !DriverItems.ContainsKey(DriverTransferInterface.Get_Drv_ID(drvObj.DLLObj)))
                            {
                                #region 驱动存在
                                drvObj.NDriverID = DriverTransferInterface.Get_Drv_ID(drvObj.DLLObj);
                                drvObj.StrDriverName = DriverTransferInterface.Get_Drv_StrDriverName(drvObj.DLLObj);
                                drvObj.StrDriverSource = DriverTransferInterface.Get_Drv_StrDriverSource(drvObj.DLLObj);
                                drvObj.StrDriverVersion = DriverTransferInterface.Get_Drv_StrDriverVersion(drvObj.DLLObj);
                                drvObj.DttDriverVersionTime = DriverTransferInterface.Get_Drv_DttDriverVersionTime(drvObj.DLLObj);
                                DriverItems.Add(drvObj.NDriverID, drvObj);
                                strLog = "加载驱动【" + drvObj.StrDriverSource + " " + drvObj.DttDriverVersionTime.ToString("yyyyMMdd") + "】成功 √";
                                LogHelper.Info(strLog);
                                //GlobalCnfgInfo.AddLogs(strLog);
                                #endregion
                            }
                            else if (DriverItems.ContainsKey(DriverTransferInterface.Get_Drv_ID(drvObj.DLLObj)))
                            {
                                #region 驱动存在则需比较驱动时间
                                DriverItem drvObjTemp = DriverItems[DriverTransferInterface.Get_Drv_ID(drvObj.DLLObj)];
                                if (drvObjTemp.DttDriverVersionTime < drvObj.DttDriverVersionTime)
                                {
                                    drvObj.NDriverID = DriverTransferInterface.Get_Drv_ID(drvObj.DLLObj);
                                    drvObj.StrDriverName = DriverTransferInterface.Get_Drv_StrDriverName(drvObj.DLLObj);
                                    drvObj.StrDriverSource = DriverTransferInterface.Get_Drv_StrDriverSource(drvObj.DLLObj);
                                    drvObj.StrDriverVersion = DriverTransferInterface.Get_Drv_StrDriverVersion(drvObj.DLLObj);
                                    drvObj.DttDriverVersionTime = DriverTransferInterface.Get_Drv_DttDriverVersionTime(drvObj.DLLObj);
                                    DriverItems[drvObj.NDriverID] = drvObj;
                                    strLog = "加载驱动【" + drvObj.StrDriverSource + " " + drvObj.DttDriverVersionTime.ToString("yyyyMMdd") + "】更新成功 √";
                                    LogHelper.Info(strLog);
                                    //GlobalCnfgInfo.AddLogs(strLog);
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            strLog = "加载驱动【" + fileName + "】失败 ×";
                            LogHelper.Info(strLog);
                            //GlobalCnfgInfo.AddLogs(strLog);
                            flag = false;
                        }
                    }
                }

                OnLoadDriverSuccessEvent();
            }
            catch (Exception ex)
            {
                strLog = ex.Message + "加载程序集出错（" + assemblyName + "）";
                LogHelper.Error(strLog);
                flag = false;
            }
            #endregion
            return flag;
        }
    }
}
