using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Sys.Safety.Client.Alarm;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Alarm;
using Sys.Safety.Request.Enumcode;
using Basic.Framework.Service;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Client.Alarm
{
    public class ClientAlarmServer
    {
        private static IAlarmService alarmService = ServiceFactory.Create<IAlarmService>();
        private static IEnumcodeService enumcodeService = ServiceFactory.Create<IEnumcodeService>();
        private static IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        private static IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();

        /// <summary>
        /// 未标校传感器集合
        /// </summary>
        public static List<SensorCalibrationInfo> sensorCalibrationInfoList = new List<SensorCalibrationInfo>();
        /// <summary>
        /// 超期服役传感器集合
        /// </summary>
        public static List<OvertermServiceInfo> sensorOvertermServiceInfoList = new List<OvertermServiceInfo>();
        public static List<EnumcodeInfo> GetListEnum()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                EnumcodeInfo dto = new EnumcodeInfo();
                list = ClientAlarmModle.GetListEnumProperty();
                list.AddRange(ClientAlarmModle.GetListEnumClass());
                return list;
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListEnum-发生异常 " + ex.Message);
            }
            return list;
        }

        public static List<EnumcodeInfo> GetListEnumProperty()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                list = ClientAlarmModle.GetListEnumProperty();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListEnumProperty-发生异常 " + ex.Message);
            }
            return list;
        }

        public static List<EnumcodeInfo> GetListEnumClass()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                list = ClientAlarmModle.GetListEnumClass();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListEnumClass-发生异常 " + ex.Message);
            }
            return list;
        }

        public static DataTable GetListEnumAlarmType()
        {
            List<EnumcodeInfo> list = new List<EnumcodeInfo>();
            string sql = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                var request = new GetEnumcodeByEnumTypeIdRequest() { EnumTypeId = "6" };
                var response = alarmService.GetEnumcodeByEnumTypeId(request);
                if (response.Data != null)
                {
                    dt = response.Data;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListEnumAlarmType-发生异常 " + ex.Message);
            }
            return dt;
        }

        public static List<Jc_DefInfo> GetListDef()
        {
            List<Jc_DefInfo> list = new List<Jc_DefInfo>();
            try
            {
                return ClientAlarmModle.GetListDef();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListDef-发生异常 " + ex.Message);
            }
            return list;
        }

        public static bool SaveConfigToDatabase(SettingInfo dto, string s)
        {
            bool b = false;
            try
            {
                b = ClientAlarmModle.SaveConfigToDatabase(dto, s);
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-SaveConfigToDatabase-发生异常 " + ex.Message);
            }
            return b;
        }

        public static SettingInfo CheckAlarmConfigIsOnServer()
        {
            SettingInfo dto = new SettingInfo();
            try
            {
                dto = ClientAlarmModle.CheckAlarmConfigIsOnServer();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-CheckAlarmConfigIsOnServer-发生异常 " + ex.Message);
            }
            return dto;
        }
        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataDefProperty()
        {
            DataTable dt = new DataTable();
            try
            {
                var response = alarmService.GetDataDefProperty();
                if (response.Data != null)
                {
                    dt = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-CheckAlarmConfigIsOnServer-发生异常 " + ex.Message);
            }
            return dt;
        }
        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <param name="sProperty">性质</param>
        /// <returns></returns>
        public static DataTable GetClassByProperty(string sProperty)
        {
            DataTable dt = new DataTable();
            try
            {
                var requst = new GetClassByPropertyRequest() { Type = sProperty };
                var response = alarmService.GetClassByProperty(requst);
                if (response.Data != null)
                {
                    dt = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetClassByProperty-发生异常 " + ex.Message);
            }
            return dt;
        }
        public static DataTable GetDevByClass(int Classid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("txt");
            dt.Columns.Add("val");
            try
            {
                DeviceDefineGetByDevClassIDRequest DeviceDefineRequest = new DeviceDefineGetByDevClassIDRequest();
                DeviceDefineRequest.DevClassID = Classid;
                var result = deviceDefineService.GetDeviceDefineCacheByDevClassID(DeviceDefineRequest).Data;
                if (result.Count > 0)
                {
                    foreach (Jc_DevInfo tempdev in result)
                    {
                        object[] obj = new object[dt.Columns.Count];
                        obj[0] = tempdev.Name;
                        obj[1] = tempdev.Devid;
                        dt.Rows.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetClassByProperty-发生异常 " + ex.Message);
            }
            return dt;
        }
        public static DataTable GetDevByProperty(int PropertyID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("txt");
            dt.Columns.Add("val");
            try
            {
                DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest = new DeviceDefineGetByDevpropertIDRequest();
                DeviceDefineRequest.DevpropertID = PropertyID;
                var result = deviceDefineService.GetDeviceDefineCacheByDevpropertID(DeviceDefineRequest).Data;
                if (result.Count > 0)
                {
                    foreach (Jc_DevInfo tempdev in result)
                    {
                        object[] obj = new object[dt.Columns.Count];
                        obj[0] = tempdev.Name;
                        obj[1] = tempdev.Devid;
                        dt.Rows.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetClassByProperty-发生异常 " + ex.Message);
            }
            return dt;
        }
        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="sClass">种类</param>
        /// <returns></returns>
        public static DataTable GetPointByClass(string sClass)
        {
            DataTable dt = new DataTable();
            try
            {
                var request = new GetPointByClassRequest() { sClass = sClass };
                var response = alarmService.GetPointByClass(request);
                if (response.Data != null)
                {
                    dt = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetPointByClass-发生异常 " + ex.Message);
            }
            return dt;
        }
        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static DataTable GetAlarmTypeDataByProperty(string sCode, string sName)
        {
            DataTable dt = new DataTable();
            string sql = string.Empty;
            try
            {
                var resquest = new GetAlarmTypeDataByPropertyRequest();
                resquest.Code = sCode;
                resquest.Name = sName;
                var response = alarmService.GetAlarmTypeDataByProperty(resquest);
                if (response.Data != null)
                {
                    dt = response.Data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetAlarmTypeDataByProperty-发生异常 " + ex.Message);
            }
            return dt;
        }

        public static string GetDevDefineChangeDatetime()
        {
            return ClientAlarmModle.GetDevDefineChangeDatetime();
        }

        public static Dictionary<int, EnumcodeInfo> QueryDevClassByDevpropertID(int DevClassPropertyID)
        {
            return ClientAlarmModle.QueryDevClassByDevpropertID(DevClassPropertyID);
        }

        public static DataTable GetDevClassByDevpropertyID(int DevClassPropertyID)
        {
            Dictionary<int, EnumcodeInfo> dic = new Dictionary<int, EnumcodeInfo>();
            dic = ClientAlarmModle.QueryDevClassByDevpropertID(DevClassPropertyID);
            DataTable dt = new DataTable();
            dt.Columns.Add("txt", typeof(string));
            dt.Columns.Add("val", typeof(string));
            if (dic == null || dic.Count < 1) { return null; }
            foreach (var item in dic)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Value.StrEnumDisplay;
                dr[1] = item.Key;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable GetPointByDevClassID(int DevClassID)
        {
            IList<Jc_DefInfo> list = new List<Jc_DefInfo>();
            list = ClientAlarmModle.QueryPointByDevClassIDCache(DevClassID);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("txt", typeof(string));
            dt.Columns.Add("val", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Point + ":" + item.Wz;
                dr[1] = item.Point;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable GetPointByDevID(int DevID)
        {
            List<Jc_DefInfo> list = new List<Jc_DefInfo>();
            PointDefineGetByDevIDRequest PointDefineRequest = new PointDefineGetByDevIDRequest();
            PointDefineRequest.DevID = DevID.ToString();
            list = pointDefineService.GetPointDefineCacheByDevID(PointDefineRequest).Data;
            if (list == null || list.Count < 1)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("txt", typeof(string));
            dt.Columns.Add("val", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Point + ":" + item.Wz;
                dr[1] = item.Point;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable GetPointByDevPropertyID(int DevPropertyID)
        {
            IList<Jc_DefInfo> list = new List<Jc_DefInfo>();
            list = ClientAlarmModle.QueryPointByDevpropertIDCache(DevPropertyID);
            if (list == null || list.Count < 1)
            {
                return null;
            }
            DataTable dt = new DataTable();
            dt.Columns.Add("txt", typeof(string));
            dt.Columns.Add("val", typeof(string));
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.Point + ":" + item.Wz;
                dr[1] = item.Point;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static IList<EnumcodeInfo> GetListEnumState()
        {
            IList<EnumcodeInfo> list = new List<EnumcodeInfo>();
            try
            {
                list = ClientAlarmModle.GetListEnumState();
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClientAlarmServer-GetListEnumState-发生异常 " + ex.Message);
            }
            return list;
        }

        public static DataTable GetCalibrationRecord(DateTime startTime, DateTime endTime)
        {
            DataTable rvalue = new DataTable();
            GetCalibrationRecordRequest alarmRequest = new GetCalibrationRecordRequest();
            alarmRequest.StartTime = startTime;
            alarmRequest.EndTime = endTime;
            rvalue = alarmService.GetCalibrationRecord(alarmRequest).Data;
      
            return rvalue;
        }

        public static int UpdateCalibrationRecord(string id,string csStr)
        {
            UpdateCalibrationRecordRequest request = new UpdateCalibrationRecordRequest();
            request.ID = id;
            request.csStr = csStr;
            var result = alarmService.UpdateCalibrationRecord(request);
            if (result.IsSuccess && result.Data != null)
            {
                return result.Data;
            }
            return 0;
        }

        public static void InsertCalibrationRecord(Jc_BxexInfo bxInfo)
        {
            InsertCalibrationRecordRequest request = new InsertCalibrationRecordRequest();
            request.jc_bx = bxInfo;
            alarmService.InsertCalibrationRecord(request);
        }


    }
}
