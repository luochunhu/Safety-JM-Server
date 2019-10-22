using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.RealMessage;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class RealMessageControllerProxy : BaseProxy, IRealMessageService
    {
        public BasicResponse<List<Jc_BInfo>> GetAlarmData()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getalarmdata?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_BInfo>>>(responsestr);
        }

        public BasicResponse<DataTable> GetAllPointinformation()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getallpointinfo?token=" + Token, string.Empty);            
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };            
        }

        public BasicResponse<DataTable> GetBindDianYuanFenzhan()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getbinddianyuanfenzhan?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetBindDianYuanMac()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getbinddianyuanmac?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetBXPoint(GetbxpointRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getbxpoint?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetCustomPagePoint(GetCustomPagePointRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getcustompagepoint?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<string> GetDefineChangeFlg()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getdefinechangeflg?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<DataTable> GetFZJXControl(GetFZJXControlRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getfzjxcontrol?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetFZPoint()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getfzpoint?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetKZPoint()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getkzpoint?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetMaintenanceHistoryByPointId(GetMaintenanceHistoryByPointIdRequst realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/gethistorybypointid?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<Jc_DefInfo> GetPoint()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getpoint?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responsestr);
        }

        public BasicResponse<Jc_DefInfo> GetPoint(GetPointRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getpoint?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_DefInfo>>(responsestr);
        }

        public BasicResponse<string> GetRealCfgChangeFlg()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getrealcfgchangeflg?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<List<RealDataDataInfo>> GetRealData(GetRealDataRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getrealdata?token=" + Token, JSONHelper.ToJSONString(realMessageRequest), 10);
            return JSONHelper.ParseJSONString<BasicResponse<List<RealDataDataInfo>>>(responsestr);
        }

        public BasicResponse<DataTable> GetRealMac()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getrealmac?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetRunLogs()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getrunlogs?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetRunLogs(GetRunLogsRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/gethistorybypointid?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DateTime> GetTimeNow()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/gettimenow?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DateTime>>(responsestr);
        }

        public BasicResponse<DataTable> GetZKPoint(GetZKPointRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getzkpoint?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<DataTable> GetZKPointex()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/getzkpointex?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<string> ReadConfig(ReadConfigRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/readconfig?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responsestr);
        }

        public BasicResponse<DataTable> RemoteGetShowTb(RemoteGetShowTbRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/remotegetshowtb?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responsestr);
        }

        public BasicResponse<bool> RemoteUpdateStrtOrStop(RemoteUpdateStrtOrStopRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/remoteupdatestrtorstop?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }

        public BasicResponse RemoteUpgradeCommand(RemoteUpgradeCommandRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/remoteupgradecommand?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<bool> SaveConfig(SaveConfigRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/saveconfig?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }

        public BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/savecustompagepoints?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }

        public BasicResponse<bool> SavePoint(SavePointRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/savepoint?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responsestr);
        }

        public BasicResponse SetRealCfgChange()
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/SetRealCfgChange?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse UpdateAlarmStep(UpdateAlarmStepRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/updatealarmstep?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<List<Jc_RInfo>> GetRunRecordListByCounter(GetRunRecordListByCounterRequest realMessageRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/realmessage/GetRunRecordListByCounter?token=" + Token, JSONHelper.ToJSONString(realMessageRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_RInfo>>>(responsestr);
        }
    }
}
