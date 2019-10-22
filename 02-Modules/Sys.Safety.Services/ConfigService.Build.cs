using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Config;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Basic.Framework.Logging;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Basic.Framework.Service;
using Basic.Framework.Configuration;
using Sys.Safety.DataAccess;

namespace Sys.Safety.Services
{
    public partial class ConfigService : IConfigService
    {
        private IConfigRepository _Repository;
        private IPointDefineService _PointDefineService;
        private IPersonPointDefineService _PersonPointDefineService;
        private INetworkModuleService _NetworkModuleService;
        private IConfigCacheService _ConfigCacheService;

        /// <summary>
        /// 定义委托
        /// </summary>
        /// <param name="objFrm"></param>
        public delegate void SeverCloseEvent();
        /// <summary>
        /// 定义退出服务端事件
        /// </summary>
        public static SeverCloseEvent severCloseEvent;
        public ConfigService(IConfigRepository _Repository, IPointDefineService _PointDefineService, IPersonPointDefineService _PersonPointDefineService,
            INetworkModuleService _NetworkModuleService, IConfigCacheService _ConfigCacheService)
        {
            this._Repository = _Repository;
            this._PointDefineService = _PointDefineService;
            this._PersonPointDefineService = _PersonPointDefineService;
            this._NetworkModuleService = _NetworkModuleService;
            this._ConfigCacheService = _ConfigCacheService;
        }
        public BasicResponse<ConfigInfo> AddConfig(ConfigAddRequest configrequest)
        {
            var _config = ObjectConverter.Copy<ConfigInfo, ConfigModel>(configrequest.ConfigInfo);
            var resultconfig = _Repository.AddConfig(_config);
            var configresponse = new BasicResponse<ConfigInfo>();
            configresponse.Data = ObjectConverter.Copy<ConfigModel, ConfigInfo>(resultconfig);
            return configresponse;
        }
        public BasicResponse<ConfigInfo> UpdateConfig(ConfigUpdateRequest configrequest)
        {
            var _config = ObjectConverter.Copy<ConfigInfo, ConfigModel>(configrequest.ConfigInfo);
            _Repository.UpdateConfig(_config);
            var configresponse = new BasicResponse<ConfigInfo>();
            configresponse.Data = ObjectConverter.Copy<ConfigModel, ConfigInfo>(_config);
            return configresponse;
        }
        public BasicResponse DeleteConfig(ConfigDeleteRequest configrequest)
        {
            _Repository.DeleteConfig(configrequest.Id);
            var configresponse = new BasicResponse();
            return configresponse;
        }
        public BasicResponse<List<ConfigInfo>> GetConfigList(ConfigGetListRequest configrequest)
        {
            var configresponse = new BasicResponse<List<ConfigInfo>>();
            configrequest.PagerInfo.PageIndex = configrequest.PagerInfo.PageIndex - 1;
            if (configrequest.PagerInfo.PageIndex < 0)
            {
                configrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var configModelLists = _Repository.GetConfigList(configrequest.PagerInfo.PageIndex, configrequest.PagerInfo.PageSize, out rowcount);
            var configInfoLists = new List<ConfigInfo>();
            foreach (var item in configModelLists)
            {
                var ConfigInfo = ObjectConverter.Copy<ConfigModel, ConfigInfo>(item);
                configInfoLists.Add(ConfigInfo);
            }
            configresponse.Data = configInfoLists;
            return configresponse;
        }
        public BasicResponse<List<ConfigInfo>> GetConfigList()
        {
            var configresponse = new BasicResponse<List<ConfigInfo>>();
            var configModelLists = _Repository.GetConfigList();
            var configInfoLists = new List<ConfigInfo>();
            foreach (var item in configModelLists)
            {
                var ConfigInfo = ObjectConverter.Copy<ConfigModel, ConfigInfo>(item);
                configInfoLists.Add(ConfigInfo);
            }
            configresponse.Data = configInfoLists;
            return configresponse;
        }
        public BasicResponse<ConfigInfo> GetConfigById(ConfigGetRequest configrequest)
        {
            var result = _Repository.GetConfigById(configrequest.Id);
            var configInfo = ObjectConverter.Copy<ConfigModel, ConfigInfo>(result);
            var configresponse = new BasicResponse<ConfigInfo>();
            configresponse.Data = configInfo;
            return configresponse;
        }
        /// <summary>
        /// 根据名称获取配置信息
        /// </summary>
        /// <param name="configrequest"></param>
        /// <returns></returns>
        public BasicResponse<ConfigInfo> GetConfigByName(ConfigGetByNameRequest configrequest)
        {
            var result = _Repository.GetConfigByName(configrequest.Name);
            var configInfo = ObjectConverter.Copy<ConfigModel, ConfigInfo>(result);
            var configresponse = new BasicResponse<ConfigInfo>();
            configresponse.Data = configInfo;
            return configresponse;
        }
        /// <summary>
        /// 保存巡检
        /// </summary>
        /// <returns></returns>
        public BasicResponse SaveInspection()
        {
            DateTime SaveTime = DateTime.Now;
            BasicResponse Result = new BasicResponse();

            //测点定义保存巡检(安全监控)
            _PointDefineService.PointDefineSaveData();

            //测点定义保存巡检(人员定位)  20171123
            _PersonPointDefineService.PointDefineSaveData();

            //网络模块管理保存巡检
            _NetworkModuleService.NetworkModuleSaveData();

            ////保存定义更新时间
            //if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_DefUpdateTime"))
            //{
            //    Basic.Framework.Data.PlatRuntime.Items["_DefUpdateTime"] = SaveTime;
            //}
            //else
            //{
            //    Basic.Framework.Data.PlatRuntime.Items.Add("_DefUpdateTime", SaveTime);
            //}

            ////保存数据库
            //ConfigInfo tempConfig = new ConfigInfo();
            //tempConfig.Name = "defdatetime";
            //tempConfig.Text = SaveTime.ToString("yyyy-MM-dd HH:mm:ss");
            //tempConfig.Upflag = "0";
            //ConfigCacheGetByKeyRequest configCacheRequest = new ConfigCacheGetByKeyRequest();
            //configCacheRequest.Name = "defdatetime";
            //var result = _ConfigCacheService.GetConfigCacheByKey(configCacheRequest);
            //ConfigInfo tempConfigCache = result.Data;
            //if (tempConfig != null)
            //{
            //    tempConfig.ID = tempConfigCache.ID;
            //    tempConfig.InfoState = InfoState.Modified;
            //    //更新数据库
            //    var request = ObjectConverter.Copy<ConfigInfo, ConfigModel>(tempConfig);
            //    _Repository.UpdateConfig(request);
            //    //更新缓存
            //    ConfigCacheUpdateRequest UpdateConfigCacheRequest = new ConfigCacheUpdateRequest();
            //    UpdateConfigCacheRequest.ConfigInfo = tempConfig;
            //    _ConfigCacheService.UpdateConfigCahce(UpdateConfigCacheRequest);
            //}
            var req = new SaveInspectionInRequest
            {
                SaveTime = SaveTime
            };
            SaveInspectionIn(req);

            return Result;
        }

        public BasicResponse SaveInspectionIn(SaveInspectionInRequest saveInspectionInRequest)
        {
            var saveTime = saveInspectionInRequest.SaveTime;
            //保存定义更新时间
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("_DefUpdateTime"))
            {
                Basic.Framework.Data.PlatRuntime.Items["_DefUpdateTime"] = saveTime;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add("_DefUpdateTime", saveTime);
            }

            //保存数据库
            ConfigInfo tempConfig = new ConfigInfo();
            tempConfig.Name = "defdatetime";
            tempConfig.Text = saveTime.ToString("yyyy-MM-dd HH:mm:ss");
            tempConfig.Upflag = "0";
            ConfigCacheGetByKeyRequest configCacheRequest = new ConfigCacheGetByKeyRequest();
            configCacheRequest.Name = "defdatetime";
            var result = _ConfigCacheService.GetConfigCacheByKey(configCacheRequest);
            ConfigInfo tempConfigCache = result.Data;
            if (tempConfig != null)
            {
                tempConfig.ID = tempConfigCache.ID;
                tempConfig.InfoState = InfoState.Modified;
                //更新数据库
                var request = ObjectConverter.Copy<ConfigInfo, ConfigModel>(tempConfig);
                _Repository.UpdateConfig(request);
                //更新缓存
                ConfigCacheUpdateRequest UpdateConfigCacheRequest = new ConfigCacheUpdateRequest();
                UpdateConfigCacheRequest.ConfigInfo = tempConfig;
                _ConfigCacheService.UpdateConfigCahce(UpdateConfigCacheRequest);
            }

            return new BasicResponse();
        }

        /// <summary>
        /// 获取平台运情况
        /// </summary>
        /// <returns></returns>
        public BasicResponse<RunningInfo> GetRunningInfo()
        {
            RunningInfo runInfo = new RunningInfo();

            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.Keys.Contains("CustomerInfo"))
                {
                    runInfo.CustomerInfo = Basic.Framework.Data.PlatRuntime.Items["CustomerInfo"].ToString();
                }
                if (Basic.Framework.Data.PlatRuntime.Items.Keys.Contains("AuthorizationExpires"))
                {
                    runInfo.AuthorizationExpires = (bool)Basic.Framework.Data.PlatRuntime.Items["AuthorizationExpires"];
                }
                //IPant service = ServiceFactory.CreateService<IPant>();
                //1.获取服务端到数据采的状态
                //2.获取数采到服务端的状态，数据采的数据最后接收时间
                //runInfo.LastReceiveTime = service.GetLastReceiveTime();
                //service.CheckServerState(); 
                var remoteStateService = ServiceFactory.Create<IRemoteStateService>();
                runInfo.LastReceiveTime = remoteStateService.GetLastReciveTime().Data;
                if (remoteStateService.GetRemoteState().Data)
                {
                    runInfo.SlaveServerState = 1;
                }
                else
                {
                    runInfo.SlaveServerState = 0;
                }


                //备机服务器状态
                //if (Basic.CBF.Common.Service.ComPantService.RemoteState)
                //{
                //    runInfo.SlaveServerState = 1;
                //}
                //else
                //{
                //    runInfo.SlaveServerState = 0;
                //}

                //主网关状态
                if (remoteStateService.GetGatewayState().Data)
                {
                    runInfo.MasterDataCollectorState = 1;
                }
                else
                {
                    runInfo.MasterDataCollectorState = 0;
                }

                //备网关状态
                //if (Basic.CBF.Common.Service.ComPantService.StandbyFrontendState)
                //{
                //    runInfo.SlaveDataCollectorState = 1;
                //}
                //else
                //{
                //    runInfo.SlaveDataCollectorState = 0;
                //}

                //(数采到服务端的状态，暂时写死（只要客户端能访问到服务端，即认为数采到服务端也是正常的）)
                runInfo.MasterServerState = 1;
            }
            catch (Exception ex)
            {
                runInfo.MasterServerState = 0;
                LogHelper.Error(ex);
            }

            //3.获取数据库连接状态，获取数据库占用磁盘空间
            GetDbInfo(runInfo);
            //4.获取是否启用双机热备
            GetHAInfo(runInfo);

            return new BasicResponse<RunningInfo>()
            {
                Data = runInfo
            };
            //return runInfo;
        }

        /// <summary>
        /// 获取数据状态
        /// </summary>
        /// <returns></returns>
        public BasicResponse<bool> GetDbState()
        {
            BasicResponse<bool> response = new BasicResponse<bool>();

            RunningInfo runningInfo = new RunningInfo();
            GetDbInfo(runningInfo);
            if (runningInfo.DbState == 1)
            {
                response.Data = true;
            }
            else
            {
                response.Data = false;
            }
            return response;
        }

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        /// <param name="runInfo"></param>
        private void GetDbInfo(RunningInfo runInfo)
        {
            var repository = _Repository as ConfigRepository;

            string dbName = _Repository.DatebaseName;
            string sql = @"SELECT
	                        concat(
		                        round(
			                        sum(data_length / 1024 / 1024),
			                        2
		                        ),
		                        ''
	                        ) AS data_length_MB,
	                        concat(
		                        round(
			                        sum(index_length / 1024 / 1024),
			                        2
		                        ),
		                        ''
	                        ) AS index_length_MB
                        FROM
	                        information_schema.TABLES
                        WHERE
	                        table_schema = '{0}'";

            sql = string.Format(sql, dbName);

            try
            {
                System.Data.DataTable dt = repository.QueryTableBySql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    runInfo.DbState = 1;
                    runInfo.DbSize = TypeConvert.ToDecimal(dt.Rows[0]["data_length_MB"]);
                }
            }
            catch (Exception ex)
            {
                runInfo.DbState = 0;

                LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 获取双机热备信息
        /// </summary>
        /// <param name="runInfo"></param>
        private void GetHAInfo(RunningInfo runInfo)
        {
            //20170315 适应新监控系统架构   

            //获取双机热备目录
            try
            {
                string haConfigPath = ConfigurationManager.FileConfiguration.GetString("HAPath", @"HA\BackConfig.ini");
                if (!haConfigPath.StartsWith(@"\"))
                {
                    haConfigPath = @"\" + haConfigPath;
                }

                var dr = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                if (dr != null && dr.Parent != null)
                {
                    //获取双机热备的完整目录
                    haConfigPath = dr.Parent.FullName + haConfigPath;
                }

                if (!File.Exists(haConfigPath))
                {
                    runInfo.IsUseHA = false;
                    return;
                }

                string haFlag = "0"; // 
                //从ini配置文件里去读
                haFlag = IniFileHelper.IniReadValue("Backupdb", "isbackup", haConfigPath);

                if (string.IsNullOrEmpty(haFlag) || haFlag != "1")
                {
                    runInfo.IsUseHA = false;
                }
                else
                {
                    runInfo.IsUseHA = true;
                }

                //获取当前是主机还是备机  20171109
                string ismasterorbackup = "0"; // 
                //从ini配置文件里去读
                ismasterorbackup = IniFileHelper.IniReadValue("Backupdb", "BackupZbj", haConfigPath);

                if (string.IsNullOrEmpty(ismasterorbackup) || ismasterorbackup == "1")
                {
                    runInfo.IsMasterOrBackup = 1;
                }
                else
                {
                    runInfo.IsMasterOrBackup = 2;
                }

                //获取双机热备工作状态  20180123
                //从ini配置文件里去读
                string backUpWorkState = IniFileHelper.IniReadValue("Backupdb", "BackUpWorkState", haConfigPath);
                if (string.IsNullOrEmpty(backUpWorkState))
                {
                    runInfo.BackUpWorkState = -1;//未知状态
                }
                else
                {
                    runInfo.BackUpWorkState = int.Parse(backUpWorkState);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                runInfo.IsUseHA = false;
                return;
            }

        }


        /// <summary>
        /// 获取服务器磁盘情况
        /// </summary>
        /// <param name="DiskName">磁盘名称</param>
        /// <returns></returns>       
        public BasicResponse<HardDiskInfo> GetDiskInfo(ConfigGetDiskInfoRequest request)
        {
            // return HardwareUtils.GetDiskInfo(diskName);
            return new BasicResponse<HardDiskInfo>()
            {
                Data = HardwareUtils.GetDiskInfo(request.DiskName)
            };
        }

        /// <summary>
        /// 获取数据库磁盘信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<HardDiskInfo> GetDatabaseDiskInfo()
        {
            BasicResponse<HardDiskInfo> response = new BasicResponse<HardDiskInfo>();
            string databaseDiskName = GetDatabaseDiskName();
            if (string.IsNullOrEmpty(databaseDiskName))
            {
                response.Code = BasicResponseCodes.CommonFailure;
                response.Message = "未能正确解析数据库所在磁盘名称";
            }
            else
            {
                response.Data = HardwareUtils.GetDiskInfo(databaseDiskName);
            }
            return response;
        }


        /// <summary>
        /// 获取数据库所在磁盘
        /// </summary>
        /// <returns></returns>
        private string GetDatabaseDiskName()
        {
            string databaseDiskName = "";

            var repository = _Repository as ConfigRepository;
            string sql = "show global variables like '%datadir%';   ";

            try
            {
                System.Data.DataTable dt = repository.QueryTableBySql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    databaseDiskName = dt.Rows[0]["Value"].ToString().Trim();
                    // C:\ProgramData\MySQL\MySQL Server 5.6\Data\
                    databaseDiskName = databaseDiskName.Substring(0, 1);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return databaseDiskName;
        }

        /// <summary>
        /// 获取服务器进程信息
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns></returns>       
        public BasicResponse<PorcessInfo> GetProcessInfo(ConfigGetProcessInfoRequest request)
        {
            return new BasicResponse<PorcessInfo>()
            {
                Data = HardwareUtils.GetProcessInfo(request.ProcessName)
            };

        }

        /// <summary>
        /// 退出服务端
        /// </summary>
        /// <returns></returns>
        public BasicResponse ExitServer()
        {
            BasicResponse result = new BasicResponse();
            if (null != severCloseEvent)
            {
                severCloseEvent();
            }
            return result;
        }

    }

    public class HardwareUtils
    {
        ///  <summary> 
        /// 获取指定驱动器的空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            long totalSize = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize / (1024 * 1024 * 1024);
                }
            }
            return totalSize;
        }

        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                }
            }
            return freeSpace;
        }



        public static HardDiskInfo GetDiskInfo(string diskName)
        {
            HardDiskInfo entity = null;

            try
            {
                string diskFullName = diskName + ":\\";
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    if (drive.Name.ToLower() == diskFullName.ToLower())
                    {
                        entity = new HardDiskInfo();
                        entity.DiskName = diskName;
                        entity.TotalSize = drive.TotalSize / (1024 * 1024 * 1024);
                        entity.TotalFreeSize = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                        entity.TotalUsageSize = entity.TotalSize - entity.TotalFreeSize;
                        entity.TotalUsageRate = Convert.ToInt32(Math.Round((decimal)entity.TotalUsageSize / (decimal)entity.TotalSize, 2) * 100);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            return entity;
        }

        public static PorcessInfo GetProcessInfo(string processName)
        {
            PorcessInfo result = null;

            var processList = Process.GetProcessesByName(processName);
            if (processList.Length <= 0)
            {
                return result;
            }

            result = new PorcessInfo();
            Process cur = processList[0];

            PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
            PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

            const int KB_DIV = 1024;
            const int MB_DIV = 1024 * 1024;
            const int GB_DIV = 1024 * 1024 * 1024;

            // Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024, curtime.NextValue() / Environment.ProcessorCount);
            result.ProcessName = processName;
            result.MemoryUsageSize = Math.Round((decimal)curpcp.NextValue() / MB_DIV, 2);
            result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);

            //added by  20170318 有时获取CPU为0，这里增加5次循环，尽量让CPU使用率不为0
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                if (result.CpuUsageRate <= 0)
                {
                    result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);
                }
                else
                {
                    break;
                }
            }
            return result;
        }
    }

    public class IniFileHelper
    {
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        public static string IniReadValue(string Section, string Key, string filepath)
        {
            StringBuilder retVal = new StringBuilder(0x800);
            int num = GetPrivateProfileString(Section, Key, "", retVal, 0x800, filepath);
            return retVal.ToString();
        }

        public static void IniWriteValue(string Section, string Key, string Value, string filepath)
        {
            WritePrivateProfileString(Section, Key, Value, filepath);
        }
    }

}


