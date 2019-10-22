using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public class LargeDataAnalysisLastChangedService : ILargeDataAnalysisLastChangedService
    {
        public BasicResponse<string> GetAlarmNotificationLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.AlarmNotificationChangedKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.AlarmNotificationChangedKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取报警通知配置改变时间出错", ex);
            }
            return response;
        }

        public BasicResponse<string> GetAnalysisModelLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.AnalysisModelChangedKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.AnalysisModelChangedKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取分析模型改变时间出错", ex);
            }
            return response;
        }

        public BasicResponse<string> GetEmergencyLinkageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.EmergencyLinkageChangedKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.EmergencyLinkageChangedKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取应急联动配置改变时间出错", ex);
            }
            return response;
        }

        public BasicResponse<string> GetPointDefineLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.DefUpdateTimeKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.DefUpdateTimeKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取定义改变时间出错", ex);
            }
            return response;
        }

        public BasicResponse<string> GetRegionOutageLastChangedTime(LargeDataAnalysisLastChangedRequest lastChangedRequest)
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.RegionOutageChangedKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.RegionOutageChangedKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取区域断电配置改变时间出错", ex);
            }
            return response;
        }
    }
}
