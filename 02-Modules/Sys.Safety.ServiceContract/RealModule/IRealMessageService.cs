using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.RealMessage;

namespace Sys.Safety.ServiceContract
{
    public interface IRealMessageService
    {
        /// <summary>
        /// 远程升级命令
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse RemoteUpgradeCommand(RemoteUpgradeCommandRequest realMessageRequest);

        /// <summary>
        /// 开始升级过程、或结束升级过程
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> RemoteUpdateStrtOrStop(RemoteUpdateStrtOrStopRequest realMessageRequest);

        /// <summary>
        /// TODO:目前没有弄懂这个方法的含义
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> RemoteGetShowTb(RemoteGetShowTbRequest realMessageRequest);

        /// <summary>
        /// 获取自定编排测点号
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetCustomPagePoint(GetCustomPagePointRequest realMessageRequest);

        /// <summary>
        /// 读取配置信息到config表中
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<string> ReadConfig(ReadConfigRequest realMessageRequest);

        /// <summary>
        /// 获取所有测点信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetAllPointinformation();

        /// <summary>
        /// 获取所有绑定电源箱的分站
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetBindDianYuanFenzhan();

        /// <summary>
        ///获取所有绑定电源箱的交换机
        ///</summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetBindDianYuanMac();
        /// <summary>
        /// 获取所有实时数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<RealDataDataInfo>> GetRealData(GetRealDataRequest request);

        /// <summary>
        /// 获取运行记录
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetRunLogs(GetRunLogsRequest realMessageRequest);

        /// <summary>
        /// 获取网络模块数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetRealMac();

        /// <summary>
        /// 获取报警数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_BInfo>> GetAlarmData();
        /// <summary>
        /// 修改报警措施
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateAlarmStep(UpdateAlarmStepRequest realMessageRequest);

        /// <summary>
        /// 根据时间获取测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetBXPoint(GetbxpointRequest realMessageRequest);

        /// <summary>
        /// 保存测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> SavePoint(SavePointRequest realMessageRequest);

        /// <summary>
        /// 获取控制量测点号
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetKZPoint();

        /// <summary>
        /// 获取分站测点号
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetFZPoint();

        /// <summary>
        /// 根据测点获取结构体
        /// </summary>
        /// <returns></returns>
        BasicResponse<Jc_DefInfo> GetPoint(GetPointRequest realMessageRequest);

        /// <summary>
        /// 根据测点号获取主控点
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetZKPoint(GetZKPointRequest realMessageRequest);

        /// <summary>
        /// 获取主控点
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetZKPointex();

        /// <summary>
        /// 获取分站交叉控制
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetFZJXControl(GetFZJXControlRequest realMessageRequest);

        /// <summary>
        /// 获取服务端当前时间
        /// </summary>
        /// <returns></returns>
        BasicResponse<DateTime> GetTimeNow();

        /// <summary>
        /// 存储配置信息到config表
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> SaveConfig(SaveConfigRequest realMessageRequest);

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest realMessageRequest);

        /// <summary>
        /// 根据测点号获取历史维保记录
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMaintenanceHistoryByPointId(GetMaintenanceHistoryByPointIdRequst realMessageRequest);

        /// <summary>
        /// 获取定义改变时间 判断定义是否改变
        /// </summary>
        /// <returns></returns>
        BasicResponse<string> GetDefineChangeFlg();

        /// <summary>
        /// 获取显示配置改变时间
        /// </summary>
        /// <returns></returns>
        BasicResponse<string> GetRealCfgChangeFlg();

        /// <summary>
        /// 设置显示配置改变时间
        /// </summary>
        /// <returns></returns>
        BasicResponse SetRealCfgChange();
        /// <summary>
        /// 根据Counter获取运行记录信息
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_RInfo>> GetRunRecordListByCounter(GetRunRecordListByCounterRequest realMessageRequest);
    }
}
