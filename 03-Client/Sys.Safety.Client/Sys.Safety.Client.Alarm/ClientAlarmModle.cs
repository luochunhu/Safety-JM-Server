using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Alarm;
using Sys.Safety.Request.Setting;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.Enums;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract.UserRoleAuthorize;

namespace Sys.Safety.Client.Alarm
{
    public class ClientAlarmModle
    {
        private static IAlarmService alarmService = ServiceFactory.Create<IAlarmService>();
        private static ISettingService settingService = ServiceFactory.Create<ISettingService>();
        private static IEnumcodeService enumcodeService = ServiceFactory.Create<IEnumcodeService>();
        private static IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();
        public static List<ShowDataInfo> GetReleaseAlarmRecords(long id)
        {
            List<ShowDataInfo> list = new List<ShowDataInfo>();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var request = new GetReleaseAlarmRecordsRequest() { Id = id };
                    var response = alarmService.GetReleaseAlarmRecords(request);
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmModle-GetReleaseAlarmRecords-发生异常 " + ex.Message);
            }
            return list;
        }

        public static long GetMaxID()
        {
            long maxID = 0;
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = alarmService.GetMaxId();
                    maxID = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmModle-GetMaxID-发生异常 " + ex.Message);
            }
            return maxID;
        }
        public static DateTime GetMaxTimeFromJCR()
        {
            DateTime dtm = new DateTime();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = alarmService.GetMaxTimeFromJCR();
                    dtm = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmModle-GetMaxID-发生异常 " + ex.Message);
            }
            return dtm;
        }
        public static List<EnumcodeInfo> GetListEnumProperty()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = alarmService.GetListEnumPropert();
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public static List<EnumcodeInfo> GetListEnumClass()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = alarmService.GetListEnumClass();
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public static List<Jc_DefInfo> GetListDef()
        {
            List<Jc_DefInfo> list = new List<Jc_DefInfo>();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = alarmService.GetListDef();
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public static bool SaveConfigToDatabase(SettingInfo dto, string s)
        {
            bool flg = false;
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var request = new SaveConfigToDatabaseRequest();
                    request.SettingInfo = dto;
                    request.Config = s;
                    var response = alarmService.SaveConfigToDatabase(request);
                    flg = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return flg;
        }

        public static SettingInfo CheckAlarmConfigIsOnServer()
        {
            SettingInfo dto = new SettingInfo();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var request = new GetSettingByKeyRequest() { StrKey = "AlarmShowConfig" };
                    var response = settingService.GetSettingByKey(request);
                    if (response.Data != null)
                    {
                        dto = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return dto;
        }

        public static string GetDBType()
        {
            string str = "";
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    str = Basic.Framework.Configuration.Global.DatabaseType.GetEnumDescription();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return str;
        }

        public static string GetDevDefineChangeDatetime()
        {
            string str = "";
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var response = realMessageService.GetDefineChangeFlg();
                    str = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return str;
        }

        public static Dictionary<int, EnumcodeInfo> QueryDevClassByDevpropertID(int DevPropertyID)
        {
            Dictionary<int, EnumcodeInfo> list = new Dictionary<int, EnumcodeInfo>();
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {

                    var request = new QueryDevClassByDevpropertRequest() { DevPropertId = DevPropertyID };
                    var response = alarmService.QueryDevClassByDevpropertId(request);
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public static IList<Jc_DefInfo> QueryPointByDevClassIDCache(int DevClassID)
        {
            IList<Jc_DefInfo> list = null;
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var request = new QueryPointByDevClassRequest() { DevClassId = DevClassID };
                    var response = alarmService.QueryPointByDevClassIDCache(request);
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        public static IList<Jc_DefInfo> QueryPointByDevpropertIDCache(int DevPropertID)
        {
            IList<Jc_DefInfo> list = null;
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var requst = new QueryPointByDevpropertRequest() { DevPropertId = DevPropertID };
                    var response = alarmService.QueryPointByDevpropertIDCache(requst);
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;

        }

        public static IList<EnumcodeInfo> GetListEnumState()
        {
            IList<EnumcodeInfo> list = null;
            try
            {
                if (ClientAlarmConfig.getsererconnectstate())
                {
                    var request = new EnumcodeGetByEnumTypeIDRequest() { EnumTypeId = "4" };
                    var response = enumcodeService.GetEnumcodeByEnumTypeID(request);
                    if (response.Data != null)
                    {
                        list = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;

        }
    }
}
