using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.DataContract;
using System.Data;
using System.Web.Http;
using Basic.Framework.Common;

namespace Sys.Safety.WebApi
{
    public class RealMessageController : Basic.Framework.Web.WebApi.BasicApiController, IRealMessageService
    {
        private IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();

        /// <summary>
        /// 获取报警数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getalarmdata")]
        public BasicResponse<List<Jc_BInfo>> GetAlarmData()
        {
            return realMessageService.GetAlarmData();
        }

        /// <summary>
        /// 获取所有测点信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getallpointinfo")]
        public BasicResponse<string> GetAllPointinformation()
        {
            var res = realMessageService.GetAllPointinformation();
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };           
        }

        /// <summary>
        /// 获取所有绑定电源箱分站
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getbinddianyuanfenzhan")]
        public BasicResponse<DataTable> GetBindDianYuanFenzhan()
        {
            return realMessageService.GetBindDianYuanFenzhan();
        }

        /// <summary>
        /// /获取所有绑定电源箱的交换机
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getbinddianyuanmac")]
        public BasicResponse<DataTable> GetBindDianYuanMac()
        {
            return realMessageService.GetBindDianYuanMac();
        }

        /// <summary>
        /// 根据时间获取测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getbxpoint")]
        public BasicResponse<DataTable> GetBXPoint(GetbxpointRequest realMessageRequest)
        {
            return realMessageService.GetBXPoint(realMessageRequest);
        }

        /// <summary>
        /// 获取自定编排测点号
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getcustompagepoint")]
        public BasicResponse<DataTable> GetCustomPagePoint(GetCustomPagePointRequest realMessageRequest)
        {
            return realMessageService.GetCustomPagePoint(realMessageRequest);
        }

        /// <summary>
        /// 获取定义改变时间 判断定义是否改变
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getdefinechangeflg")]
        public BasicResponse<string> GetDefineChangeFlg()
        {
            return realMessageService.GetDefineChangeFlg();
        }

        /// <summary>
        /// 获取分站交叉控制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getfzjxcontrol")]
        public BasicResponse<DataTable> GetFZJXControl(GetFZJXControlRequest realMessageRequest)
        {
            return realMessageService.GetFZJXControl(realMessageRequest);
        }

        /// <summary>
        /// 获取分站测点号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getfzpoint")]
        public BasicResponse<DataTable> GetFZPoint()
        {
            return realMessageService.GetFZPoint();
        }

        /// <summary>
        /// 获取控制量测点号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getkzpoint")]
        public BasicResponse<DataTable> GetKZPoint()
        {
            return realMessageService.GetKZPoint();
        }

        /// <summary>
        /// 根据测点号获取历史维保记录
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/gethistorybypointid")]
        public BasicResponse<DataTable> GetMaintenanceHistoryByPointId(GetMaintenanceHistoryByPointIdRequst realMessageRequest)
        {
            return realMessageService.GetMaintenanceHistoryByPointId(realMessageRequest);
        }

        /// <summary>
        /// 根据测点获取结构体
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getpoint")]
        public BasicResponse<Jc_DefInfo> GetPoint(GetPointRequest realMessageRequest)
        {
            return realMessageService.GetPoint(realMessageRequest);
        }

        /// <summary>
        /// 获取显示配置改变时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getrealcfgchangeflg")]
        public BasicResponse<string> GetRealCfgChangeFlg()
        {
            return realMessageService.GetRealCfgChangeFlg();
        }

        /// <summary>
        /// 获取所有实时数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getrealdata")]
        public BasicResponse<List<RealDataDataInfo>> GetRealData(GetRealDataRequest realMessageRequest)
        {
            return realMessageService.GetRealData(realMessageRequest);
        }

        /// <summary>
        /// 获取网络模块数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getrealmac")]
        public BasicResponse<DataTable> GetRealMac()
        {
            return realMessageService.GetRealMac();
        }

        /// <summary>
        /// 获取运行记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getrunlogs")]
        public BasicResponse<DataTable> GetRunLogs(GetRunLogsRequest realMessageRequest)
        {
            return realMessageService.GetRunLogs(realMessageRequest);
        }

        /// <summary>
        /// 获取服务端当前时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/gettimenow")]
        public BasicResponse<DateTime> GetTimeNow()
        {
            return realMessageService.GetTimeNow();
        }

        /// <summary>
        /// 根据测点号获取主控点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getzkpoint")]
        public BasicResponse<DataTable> GetZKPoint(GetZKPointRequest realMessageRequest)
        {
            return realMessageService.GetZKPoint(realMessageRequest);
        }

        /// <summary>
        /// 获取主控点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/getzkpointex")]
        public BasicResponse<DataTable> GetZKPointex()
        {
            return realMessageService.GetZKPointex();
        }

        /// <summary>
        /// 读取配置信息到config表中
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/readconfig")]
        public BasicResponse<string> ReadConfig(ReadConfigRequest realMessageRequest)
        {
            return realMessageService.ReadConfig(realMessageRequest);
        }

        /// <summary>
        /// TODO:目前没有弄懂这个方法的含义
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/remotegetshowtb")]
        public BasicResponse<DataTable> RemoteGetShowTb(RemoteGetShowTbRequest realMessageRequest)
        {
            return realMessageService.RemoteGetShowTb(realMessageRequest);
        }

        /// <summary>
        /// 开始升级过程、或结束升级过程
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/remoteupdatestrtorstop")]
        public BasicResponse<bool> RemoteUpdateStrtOrStop(RemoteUpdateStrtOrStopRequest realMessageRequest)
        {
            return realMessageService.RemoteUpdateStrtOrStop(realMessageRequest);
        }

        /// <summary>
        /// 远程升级命令
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/remoteupgradecommand")]
        public BasicResponse RemoteUpgradeCommand(RemoteUpgradeCommandRequest realMessageRequest)
        {
            return realMessageService.RemoteUpgradeCommand(realMessageRequest);
        }

        /// <summary>
        /// 存储配置信息到config表
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/saveconfig")]
        public BasicResponse<bool> SaveConfig(SaveConfigRequest realMessageRequest)
        {
            var response = new BasicResponse<bool>();
            try
            {
                response= realMessageService.SaveConfig(realMessageRequest);
            }
            catch (Exception ex)
            {

                throw;
            }


            return response;
        }

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/savecustompagepoints")]
        public BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest realMessageRequest)
        {
            var response = new BasicResponse<bool>();
            try
            {
                response= realMessageService.SaveCustomPagePoints(realMessageRequest);
            }
            catch (Exception ex)
            {

                throw;
            }

            return response;
        }

        /// <summary>
        /// 保存测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/savepoint")]
        public BasicResponse<bool> SavePoint(SavePointRequest realMessageRequest)
        {
            return realMessageService.SavePoint(realMessageRequest);
        }

        /// <summary>
        /// 设置显示配置改变时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/SetRealCfgChange")]
        public BasicResponse SetRealCfgChange()
        {
            return realMessageService.SetRealCfgChange(); ;
        }

        /// <summary>
        /// 修改报警措施
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/updatealarmstep")]
        public BasicResponse UpdateAlarmStep(UpdateAlarmStepRequest realMessageRequest)
        {
            return realMessageService.UpdateAlarmStep(realMessageRequest);
        }

        /// <summary>
        /// 修改报警措施
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/realmessage/GetRunRecordListByCounter")]
        public BasicResponse<List<Jc_RInfo>> GetRunRecordListByCounter(GetRunRecordListByCounterRequest realMessageRequest)
        {
            return realMessageService.GetRunRecordListByCounter(realMessageRequest);
        }


        BasicResponse<DataTable> IRealMessageService.GetAllPointinformation()
        {
            throw new NotImplementedException();
        }
    }
}
