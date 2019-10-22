using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Alarm;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataAccess;
using Sys.Safety.Model;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract.Cache;

namespace Sys.Safety.Services
{
    public class AlarmService : IAlarmService
    {
        private IAlarmRepository _Repository;
        private ISettingRepository _SettingRepository;

        private IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
        private IRunLogCacheService runLogCacheService = ServiceFactory.Create<IRunLogCacheService>();
        private IAllSystemPointDefineService pointDefineCacheService = ServiceFactory.Create<IAllSystemPointDefineService>();
        private IDeviceClassCacheService deviceClassCacheService = ServiceFactory.Create<IDeviceClassCacheService>();
        private IDevicePropertyCacheService devicePropertyCacheService = ServiceFactory.Create<IDevicePropertyCacheService>();

        public AlarmService(IAlarmRepository _Repository, ISettingRepository _SettingRepository)
        {
            this._Repository = _Repository;
            this._SettingRepository = _SettingRepository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RealMessageService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }

        public BasicResponse<List<ShowDataInfo>> GetReleaseAlarmRecords(GetReleaseAlarmRecordsRequest alarmRequest)
        {
            var response = new BasicResponse<List<ShowDataInfo>>();
            var runLogCacheGetAllRequest = new RunLogCacheGetAllRequest();
            var runLogCacheGetAllResponse = runLogCacheService.GetAllRunLogCache(runLogCacheGetAllRequest);
            //var pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
            var pointDefineCacheGetAllResponse = pointDefineCacheService.GetAllPointDefineCache();//从多系统缓存中查找  20171213
            if (runLogCacheGetAllResponse.Data != null && runLogCacheGetAllResponse.Data.Count > 0)
            {
                //跨天0点的记录不查询
                var listR = runLogCacheGetAllResponse.Data.Where(a => a.Counter > alarmRequest.Id && a.Timer > DateTime.Parse(DateTime.Now.ToShortDateString() + " 00:00:00")).ToList();
                var listData = new List<ShowDataInfo>();
                try
                {
                    foreach (var item in listR)
                    {
                        if (item.Point == "000000")
                        {
                            continue;
                        }
                        ShowDataInfo sd = new ShowDataInfo();
                        var defInfo = pointDefineCacheGetAllResponse.Data.Find(x => x.Point == item.Point && x.Wzid == item.Wzid && x.Devid == item.Devid);
                        if (defInfo == null)
                        {
                            continue;
                        }
                        sd.ID = long.Parse(item.ID);
                        sd.Counter = item.Counter;
                        sd.Fzh = item.Fzh;
                        sd.Kh = item.Kh;
                        sd.Devid = item.Devid;
                        sd.Wzid = item.Wzid;
                        sd.Property = defInfo.DevPropertyID;
                        sd.Class = defInfo.DevClassID;
                        sd.Type = item.Type;
                        sd.Point = item.Point;
                        sd.Devname = defInfo.DevName;
                        sd.Wz = defInfo.Wz;
                        sd.Ssz = item.Val;
                        sd.Unit = defInfo.Unit;
                        sd.State = item.State;
                        sd.Timer = item.Timer;
                        sd.Flag = 0;
                        listData.Add(sd);
                    }
                    response.Data = listData;
                }
                catch (Exception ex)
                {
                    response.Code = -100;
                    response.Message = ex.Message;
                    this.ThrowException("GetReleaseAlarmRecords-发生异常", ex); ;
                }
            }

            return response;
        }

        /// <summary>
        ///获取最大Id
        /// </summary>
        /// <returns></returns>
        public BasicResponse<long> GetMaxId()
        {
            var response = new BasicResponse<long>();
            try
            {
                var runLogCacheGetAllRequest = new RunLogCacheGetAllRequest();
                var runLogCacheGetAllResponse = runLogCacheService.GetAllRunLogCache(runLogCacheGetAllRequest);
                if (runLogCacheGetAllResponse.Data != null && runLogCacheGetAllResponse.Data.Count > 0)
                {
                    var log = runLogCacheGetAllResponse.Data.OrderBy(x => x.Counter).LastOrDefault();
                    if (log != null)
                    {
                        response.Data = log.Counter;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetMaxID-获取报警记录的当前最大ID号-发生异常", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取最大时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DateTime> GetMaxTimeFromJCR()
        {
            var response = new BasicResponse<DateTime>();
            DateTime time = DateTime.Now;
            try
            {
                var runLogCacheGetAllRequest = new RunLogCacheGetAllRequest();
                var runLogCacheGetAllResponse = runLogCacheService.GetAllRunLogCache(runLogCacheGetAllRequest);
                if (runLogCacheGetAllResponse.Data != null && runLogCacheGetAllResponse.Data.Count > 0)
                {
                    var listR = runLogCacheGetAllResponse.Data.OrderBy(a => a.Timer).LastOrDefault();
                    if (listR != null)
                    {
                        time = listR.Timer;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetNowMaxTimeFromJCR-获取报警记录的当前最大时间-发生异常", ex);

            }
            response.Data = time;

            return response;
        }

        /// <summary>
        /// 获取所有设备性质
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetListEnumPropert()
        {
            var response = new BasicResponse<List<EnumcodeInfo>>();
            try
            {
                var devicePropertyCacheGetAllRequest = new DevicePropertyCacheGetAllRequest();
                var devicePropertyCacheGetAllResponse = devicePropertyCacheService.GetAllDevicePropertyCache(devicePropertyCacheGetAllRequest);
                response.Data = devicePropertyCacheGetAllResponse.Data;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetListEnumPropert-发生异常", ex); ;
            }

            return response;
        }

        /// <summary>
        /// 从服务端内存结构中获取所有设备种类
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<EnumcodeInfo>> GetListEnumClass()
        {
            var response = new BasicResponse<List<EnumcodeInfo>>();
            try
            {
                var deviceClassCacheGetAllRequest = new DeviceClassCacheGetAllRequest();
                var deviceClassCacheGetAllResponse = deviceClassCacheService.GetAllDeviceClassCache(deviceClassCacheGetAllRequest);
                response.Data = deviceClassCacheGetAllResponse.Data;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetNowMaxTimeFromJCR-获取报警记录的当前最大时间-发生异常", ex);
            }

            return response;
        }

        /// <summary>
        /// 从服务端内存获取设备定义列表
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> GetListDef()
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            try
            {
                //var pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
                var pointDefineCacheGetAllResponse = pointDefineCacheService.GetAllPointDefineCache();//从多系统缓存中查找  20171213
                response.Data = pointDefineCacheGetAllResponse.Data;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("GetListDef-从服务端内存获取设备定义列表-发生异常", ex);
            }

            return response;
        }

        /// <summary>
        ///保存配置文件到数据库
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> SaveConfigToDatabase(SaveConfigToDatabaseRequest alarmRequest)
        {
            var response = new BasicResponse<bool>();
            try
            {
                if (alarmRequest.SettingInfo == null)
                {
                    var settingModel = new SettingModel();
                    settingModel.StrType = "报警配置";
                    settingModel.StrKey = "AlarmShowConfig";
                    settingModel.StrKeyCHs = "报警设置";
                    settingModel.StrValue = alarmRequest.Config;
                    settingModel.LastUpdateDate = DateTime.Now.ToString();
                    _SettingRepository.AddSetting(settingModel);
                }
                else
                {
                    var settingModel = _SettingRepository.GetSettingById(alarmRequest.SettingInfo.ID);
                    settingModel.StrValue = alarmRequest.Config;
                    settingModel.LastUpdateDate = DateTime.Now.ToString();
                    _SettingRepository.UpdateSetting(settingModel);
                }
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("SaveConfigToDatabase-保存配置文件到数据库-发生异常 ", ex);
            }

            return response;
        }

        /// <summary>
        /// 通过设备性质查找设备种类
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<Dictionary<int, EnumcodeInfo>> QueryDevClassByDevpropertId(QueryDevClassByDevpropertRequest alarmRequest)
        {
            var response = new BasicResponse<Dictionary<int, EnumcodeInfo>>();
            try
            {
                var result = new Dictionary<int, EnumcodeInfo>();
                var deviceClassCacheGetAllRequest = new DeviceClassCacheGetAllRequest();
                var deviceClassCacheGetAllResponse = deviceClassCacheService.GetAllDeviceClassCache(deviceClassCacheGetAllRequest);
                if (deviceClassCacheGetAllResponse.Data != null && deviceClassCacheGetAllResponse.Data.Count > 0)
                {
                    foreach (var item in deviceClassCacheGetAllResponse.Data)
                    {

                        if (alarmRequest.DevPropertId.ToString() == item.LngEnumValue3)
                        {
                            result.Add(item.LngEnumValue, item);
                        }
                    }
                }
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("QueryDevClassByDevpropertID-通过设备性质查找设备种类-发生异常", ex);
            }

            return response;
        }

        /// <summary>
        /// 通过设备种类查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevClassIDCache(QueryPointByDevClassRequest alarmRequest)
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevClassID == alarmRequest.DevClassId;
            var result = pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            response.Data = result.Data;
            return response;
        }

        /// <summary>
        /// 通过设备性质查找测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> QueryPointByDevpropertIDCache(QueryPointByDevpropertRequest alarmRequest)
        {
            var response = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevPropertyID == alarmRequest.DevPropertId;
            var result = pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            response.Data = result.Data;
            return response;
        }

        /// <summary>
        /// 根据EnumTypeId获取Enumcode
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetEnumcodeByEnumTypeId(GetEnumcodeByEnumTypeIdRequest alarmRequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(alarmRequest.EnumTypeId))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var datatable = _Repository.GetEnumcodeByEnumTypeId(alarmRequest.EnumTypeId);
                response.Data = datatable;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据EnumTypeId获取Enumcode", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取已定义设备中所有的性质
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetDataDefProperty()
        {
            var response = new BasicResponse<DataTable>();
            try
            {
                response.Data = _Repository.GetDataDefProperty();
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取已定义设备中所有的性质", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据性质获取种类
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetClassByProperty(GetClassByPropertyRequest alarmRequest)
        {
            var response = new BasicResponse<DataTable>();
            try
            {
                response.Data = _Repository.GetClassByProperty(alarmRequest.Type);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据性质获取种类", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据种类获取测点
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetPointByClass(GetPointByClassRequest alarmRequest)
        {
            var response = new BasicResponse<DataTable>();
            try
            {
                response.Data = _Repository.GetPointByClass(alarmRequest.sClass);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据种类获取测点", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据设备性质来获取设备性质对应的状态类型
        /// </summary>
        /// <param name="alarmRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAlarmTypeDataByProperty(GetAlarmTypeDataByPropertyRequest alarmRequest)
        {
            var response = new BasicResponse<DataTable>();
            if (string.IsNullOrWhiteSpace(alarmRequest.Code) || string.IsNullOrWhiteSpace(alarmRequest.Name))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                response.Data = _Repository.GetAlarmTypeDataByProperty(alarmRequest.Code, alarmRequest.Name);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据设备性质来获取设备性质对应的状态类型", ex);
            }

            return response;
        }
        /// <summary>
        /// 获取指定时间范围内的标校记录
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetCalibrationRecord(GetCalibrationRecordRequest alarmRequest)
        {
            var response = new BasicResponse<DataTable>();
            if (alarmRequest.StartTime > alarmRequest.EndTime)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                response.Data = _Repository.GetCalibrationRecord(alarmRequest.StartTime, alarmRequest.EndTime);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据设备性质来获取设备性质对应的状态类型", ex);
            }
            return response;
        }



        public BasicResponse<int> UpdateCalibrationRecord(UpdateCalibrationRecordRequest request)
        {
            var response = new BasicResponse<int>();

            response.Data = _Repository.UpdateCalibrationRecord(request.ID, request.csStr);

            return response;
        }


        public BasicResponse InsertCalibrationRecord(InsertCalibrationRecordRequest request)
        {
            Basic.Framework.Data.RepositoryBase<UserModel> _Repository =
            ServiceFactory.Create<IUserRepository>() as Basic.Framework.Data.RepositoryBase<UserModel>;
            Jc_BxexInfo info = request.jc_bx;
            _Repository.ExecuteNonQuery("global_Calibration_InsertIntoJcbxex1", info.ID,
                        info.Point, info.PointID, info.Stime,
                       info.Etime, (info.Etime - info.Stime).TotalSeconds, info.Zdz,
                       info.Zxz, info.Pjz, info.Zdztime,
                        info.Zxztime, info.Bxzt, info.Cs);

            return new BasicResponse();
        }

    }
}
