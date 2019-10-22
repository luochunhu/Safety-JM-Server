using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Enumcode;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;
using System.Data;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Services
{
    public partial class EnumcodeService : IEnumcodeService
    {
        private IEnumcodeRepository _Repository;

        IDevicePropertyCacheService _DevicePropertyCacheService = ServiceFactory.Create<IDevicePropertyCacheService>();

        IDeviceClassCacheService _DeviceClassCacheService = ServiceFactory.Create<IDeviceClassCacheService>();

        IDeviceTypeCacheService _DeviceTypeCacheService = ServiceFactory.Create<IDeviceTypeCacheService>();

        IDeviceClassCacheService _deviceClassCacheService = ServiceFactory.Create<IDeviceClassCacheService>();

        public EnumcodeService(IEnumcodeRepository _Repository)
        {
            this._Repository = _Repository;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("EnumCodeService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 保存枚举
        /// </summary>
        /// <param name="enumcoderequest"></param>
        /// <returns></returns>
        public BasicResponse<EnumcodeInfo> SaveEnumCode(EnumcodeAddRequest enumcoderequest)
        {
            EnumcodeInfo dto = null;

            BasicResponse<EnumcodeInfo> Result = new BasicResponse<EnumcodeInfo>();

            dto = enumcoderequest.EnumcodeInfo;

            if (dto == null)
            {
                //throw new Exception("枚举类型对象为空，请检查是否已赋值！");
                ThrowException("SaveEnumCode", new Exception("枚举类型对象为空，请检查是否已赋值！"));
            }
            if (dto.LngEnumValue == null)
            {
                //throw new Exception("枚举DTO中LngEnumValue属性未赋值！");
                ThrowException("SaveEnumCode", new Exception("枚举DTO中LngEnumValue属性未赋值！"));
            }
            if (dto.StrEnumDisplay.Trim() == "")
            {
                //throw new Exception("枚举DTO中strEnumDisplay属性未赋值！");
                ThrowException("SaveEnumCode", new Exception("枚举DTO中strEnumDisplay属性未赋值！"));
            }
            if (dto.InfoState == InfoState.NoChange)
            {
                //throw new Exception("DTO对象未设置状态，请先设置！");
                ThrowException("SaveEnumCode", new Exception("DTO对象未设置状态，请先设置！"));
            }
            if (dto.EnumTypeID == "0")
            {
                //throw new Exception("枚举类型必须设置值，属性名EnumTypeID！");
                ThrowException("SaveEnumCode", new Exception("枚举类型必须设置值，属性名EnumTypeID！"));
            }
            try
            {
                int ID = 0;
                if (dto.InfoState == InfoState.AddNew)
                {
                    Result = AddEnumcode(enumcoderequest);
                }
                else
                {
                    EnumcodeUpdateRequest updaterequest = new EnumcodeUpdateRequest();
                    updaterequest.EnumcodeInfo = enumcoderequest.EnumcodeInfo;
                    Result = UpdateEnumcode(updaterequest);
                }
            }
            catch (System.Exception ex)
            {
                ThrowException("SaveEnumCode", ex);
            }


            return Result;
        }        

        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>
        public BasicResponse UpdateCache()
        {
            BasicResponse Result = new BasicResponse();          
            DataTable dt = _Repository.GetAllEnumcode();
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey("CustomerEnumCode"))
            {
                Basic.Framework.Data.PlatRuntime.Items["CustomerEnumCode"] = dt;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add("CustomerEnumCode", dt);//添加到服务端进程运行对象缓存中  20170523
            }
            //重新加载设备性质、设备种类、设备型号的缓存  20170711
            //加载设备性质           
            _DevicePropertyCacheService.LoadDevicePropertyCache(new DevicePropertyCacheLoadRequest());
            //加载设备种类           
            _DeviceClassCacheService.LoadDeviceClassCache(new DeviceClassCacheLoadRequest());
            //加载设备型号            
            _DeviceTypeCacheService.LoadDeviceTypeCache(new DeviceTypeCacheLoadRequest());

            return Result;
        }
        
        public BasicResponse<EnumcodeInfo> AddEnumcode(EnumcodeAddRequest enumcoderequest)
        {
            var _enumcode = ObjectConverter.Copy<EnumcodeInfo, EnumcodeModel>(enumcoderequest.EnumcodeInfo);
            var resultenumcode = _Repository.AddEnumcode(_enumcode);
            var enumcoderesponse = new BasicResponse<EnumcodeInfo>();
            enumcoderesponse.Data = ObjectConverter.Copy<EnumcodeModel, EnumcodeInfo>(resultenumcode);
            return enumcoderesponse;
        }
        public BasicResponse<EnumcodeInfo> UpdateEnumcode(EnumcodeUpdateRequest enumcoderequest)
        {
            var _enumcode = ObjectConverter.Copy<EnumcodeInfo, EnumcodeModel>(enumcoderequest.EnumcodeInfo);
            _Repository.UpdateEnumcode(_enumcode);
            var enumcoderesponse = new BasicResponse<EnumcodeInfo>();
            enumcoderesponse.Data = ObjectConverter.Copy<EnumcodeModel, EnumcodeInfo>(_enumcode);
            return enumcoderesponse;
        }
        public BasicResponse DeleteEnumcode(EnumcodeDeleteRequest enumcoderequest)
        {
            _Repository.DeleteEnumcode(enumcoderequest.Id);
            var enumcoderesponse = new BasicResponse();
            return enumcoderesponse;
        }
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList(EnumcodeGetListRequest enumcoderequest)
        {
            var enumcoderesponse = new BasicResponse<List<EnumcodeInfo>>();
            enumcoderequest.PagerInfo.PageIndex = enumcoderequest.PagerInfo.PageIndex - 1;
            if (enumcoderequest.PagerInfo.PageIndex < 0)
            {
                enumcoderequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var enumcodeModelLists = _Repository.GetEnumcodeList(enumcoderequest.PagerInfo.PageIndex, enumcoderequest.PagerInfo.PageSize, out rowcount);
            var enumcodeInfoLists = new List<EnumcodeInfo>();
            foreach (var item in enumcodeModelLists)
            {
                var EnumcodeInfo = ObjectConverter.Copy<EnumcodeModel, EnumcodeInfo>(item);
                enumcodeInfoLists.Add(EnumcodeInfo);
            }
            enumcoderesponse.Data = enumcodeInfoLists;
            return enumcoderesponse;
        }
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeList()
        {
            var enumcoderesponse = new BasicResponse<List<EnumcodeInfo>>();            
            var enumcodeModelLists = _Repository.GetEnumcodeList();
            var enumcodeInfoLists = new List<EnumcodeInfo>();
            foreach (var item in enumcodeModelLists)
            {
                var EnumcodeInfo = ObjectConverter.Copy<EnumcodeModel, EnumcodeInfo>(item);
                enumcodeInfoLists.Add(EnumcodeInfo);
            }
            enumcoderesponse.Data = enumcodeInfoLists;
            return enumcoderesponse;
        }
        public BasicResponse<EnumcodeInfo> GetEnumcodeById(EnumcodeGetRequest enumcoderequest)
        {
            var result = _Repository.GetEnumcodeById(enumcoderequest.Id);
            var enumcodeInfo = ObjectConverter.Copy<EnumcodeModel, EnumcodeInfo>(result);
            var enumcoderesponse = new BasicResponse<EnumcodeInfo>();
            enumcoderesponse.Data = enumcodeInfo;
            return enumcoderesponse;
        }
        public BasicResponse<List<EnumcodeInfo>> GetEnumcodeByEnumTypeID(EnumcodeGetByEnumTypeIDRequest enumcoderequest)
        {
            var result = _Repository.GetEnumcodeByEnumTypeID(enumcoderequest.EnumTypeId);
            var enumcodeInfo = ObjectConverter.CopyList<EnumcodeModel, EnumcodeInfo>(result).ToList();
            var enumcoderesponse = new BasicResponse<List<EnumcodeInfo>>();
            enumcoderesponse.Data = enumcodeInfo;
            return enumcoderesponse;
        }

        public BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache()
        {
            var req = new DevicePropertyCacheGetAllRequest();
            return _DevicePropertyCacheService.GetAllDevicePropertyCache(req);
        }

        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache()
        {
            var req = new DeviceClassCacheGetAllRequest();
            return _deviceClassCacheService.GetAllDeviceClassCache(req);
        }
        public BasicResponse<List<EnumcodeInfo>> GetAllDeviceModelCache()
        {
            var req = new DeviceTypeCacheGetAllRequest();
            return _DeviceTypeCacheService.GetAllDeviceTypeCache(req);
        }
    }
}


