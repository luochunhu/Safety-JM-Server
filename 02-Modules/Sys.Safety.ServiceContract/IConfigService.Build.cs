using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Config;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IConfigService
    {
        BasicResponse<ConfigInfo> AddConfig(ConfigAddRequest configrequest);
        BasicResponse<ConfigInfo> UpdateConfig(ConfigUpdateRequest configrequest);
        BasicResponse DeleteConfig(ConfigDeleteRequest configrequest);
        BasicResponse<List<ConfigInfo>> GetConfigList(ConfigGetListRequest configrequest);
        BasicResponse<List<ConfigInfo>> GetConfigList();
        BasicResponse<ConfigInfo> GetConfigById(ConfigGetRequest configrequest);
        /// <summary>
        /// 根据名称获取配置信息
        /// </summary>
        /// <param name="configrequest"></param>
        /// <returns></returns>
        BasicResponse<ConfigInfo> GetConfigByName(ConfigGetByNameRequest configrequest);
        /// <summary>
        /// 保存巡检
        /// </summary>
        /// <returns></returns>
        BasicResponse SaveInspection();

        BasicResponse SaveInspectionIn(SaveInspectionInRequest saveInspectionInRequest);

        /// <summary>
        /// 获取平台运情况
        /// </summary>
        /// <returns></returns>      
        BasicResponse<RunningInfo> GetRunningInfo();

        /// <summary>
        /// 获取服务器磁盘情况
        /// </summary>
        /// <param name="diskName">磁盘名称</param>
        /// <returns></returns>   
        BasicResponse<HardDiskInfo> GetDiskInfo(ConfigGetDiskInfoRequest request);

        /// <summary>
        /// 获取服务器进程信息
        /// </summary>
        /// <param name="processName">进程名称</param>
        /// <returns></returns>       
        BasicResponse<PorcessInfo> GetProcessInfo(ConfigGetProcessInfoRequest request);

        /// <summary>
        /// 获取数据状态
        /// </summary>
        /// <returns></returns>
        BasicResponse<bool> GetDbState();

        /// <summary>
        /// 获取数据库磁盘信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<HardDiskInfo> GetDatabaseDiskInfo();
        /// <summary>
        /// 退出服务端
        /// </summary>
        /// <returns></returns>
        BasicResponse ExitServer();
    }
}

