using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Setting;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Services
{
    public partial class SettingService : ISettingService
    {
        private ISettingRepository _Repository;
        private ISettingCacheService _SettingCacheService;

        public SettingService(ISettingRepository _Repository,ISettingCacheService _SettingCacheService)
        {
            this._Repository = _Repository;
            this._SettingCacheService = _SettingCacheService;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("SettingService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 保存配置信息
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        public BasicResponse<SettingInfo> SaveSetting(SettingAddRequest settingrequest)
        {
            BasicResponse<SettingInfo> Result = new BasicResponse<SettingInfo>();

            SettingInfo dto = null;

            dto = settingrequest.SettingInfo;

            if (dto == null)
            {
                //throw new Exception("Setting对象为空，请检查是否已赋值！");
                ThrowException("SaveSetting", new Exception("Setting对象为空，请检查是否已赋值！"));
            }
            if (dto.StrKey == "")
            {
                //throw new Exception("SettingDTO对象未定义Key值，请先定义Key后保存！");
                ThrowException("SaveSetting", new Exception("SettingDTO对象未定义Key值，请先定义Key后保存！"));
            }
            if (dto.StrValue.Trim() == "")
            {
                //throw new Exception("SettingDTO对象未定义值，请先输入后保存！");
                ThrowException("SaveSetting", new Exception("SettingDTO对象未定义值，请先输入后保存！"));
            }
            if (dto.InfoState == InfoState.NoChange)
            {
                //throw new Exception("DTO对象未设置状态，请先设置！");
                ThrowException("SaveSetting", new Exception("DTO对象未设置状态，请先设置！"));
            }
            try
            {
                long ID = long.Parse(dto.ID);
                if (dto.InfoState == InfoState.AddNew)
                {
                    ID = IdHelper.CreateLongId();
                    dto.ID = ID.ToString();
                    Result = AddSetting(settingrequest);
                }
                else
                {
                    SettingUpdateRequest updaterequest = new SettingUpdateRequest();
                    updaterequest.SettingInfo = settingrequest.SettingInfo;
                    Result = UpdateSetting(updaterequest);
                }
                //重新加载缓存 
                SettingCacheLoadRequest settingCacheRequest=new SettingCacheLoadRequest();
                _SettingCacheService.LoadSettingCache(settingCacheRequest);
            }
            catch (System.Exception ex)
            {
                ThrowException("SaveSetting", ex);
            }
            return Result;
        }
        public BasicResponse<SettingInfo> AddSetting(SettingAddRequest settingrequest)
        {
            var _setting = ObjectConverter.Copy<SettingInfo, SettingModel>(settingrequest.SettingInfo);
            var resultsetting = _Repository.AddSetting(_setting);
            var settingresponse = new BasicResponse<SettingInfo>();
            settingresponse.Data = ObjectConverter.Copy<SettingModel, SettingInfo>(resultsetting);
            return settingresponse;
        }
        public BasicResponse<SettingInfo> UpdateSetting(SettingUpdateRequest settingrequest)
        {
            var _setting = ObjectConverter.Copy<SettingInfo, SettingModel>(settingrequest.SettingInfo);
            _Repository.UpdateSetting(_setting);
            var settingresponse = new BasicResponse<SettingInfo>();
            settingresponse.Data = ObjectConverter.Copy<SettingModel, SettingInfo>(_setting);
            return settingresponse;
        }
        public BasicResponse DeleteSetting(SettingDeleteRequest settingrequest)
        {
            _Repository.DeleteSetting(settingrequest.Id);
            var settingresponse = new BasicResponse();
            return settingresponse;
        }
        public BasicResponse<List<SettingInfo>> GetSettingList(SettingGetListRequest settingrequest)
        {
            var settingresponse = new BasicResponse<List<SettingInfo>>();
            settingrequest.PagerInfo.PageIndex = settingrequest.PagerInfo.PageIndex - 1;
            if (settingrequest.PagerInfo.PageIndex < 0)
            {
                settingrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var settingModelLists = _Repository.GetSettingList(settingrequest.PagerInfo.PageIndex, settingrequest.PagerInfo.PageSize, out rowcount);
            var settingInfoLists = new List<SettingInfo>();
            foreach (var item in settingModelLists)
            {
                var SettingInfo = ObjectConverter.Copy<SettingModel, SettingInfo>(item);
                settingInfoLists.Add(SettingInfo);
            }
            settingresponse.Data = settingInfoLists;
            return settingresponse;
        }
        public BasicResponse<List<SettingInfo>> GetSettingList()
        {
            var settingresponse = new BasicResponse<List<SettingInfo>>();
            var settingModelLists = _Repository.GetSettingList();
            var settingInfoLists = new List<SettingInfo>();
            foreach (var item in settingModelLists)
            {
                var SettingInfo = ObjectConverter.Copy<SettingModel, SettingInfo>(item);
                settingInfoLists.Add(SettingInfo);
            }
            settingresponse.Data = settingInfoLists;
            return settingresponse;
        }
        public BasicResponse<SettingInfo> GetSettingById(SettingGetRequest settingrequest)
        {
            var result = _Repository.GetSettingById(settingrequest.Id);
            var settingInfo = ObjectConverter.Copy<SettingModel, SettingInfo>(result);
            var settingresponse = new BasicResponse<SettingInfo>();
            settingresponse.Data = settingInfo;
            return settingresponse;
        }

        /// <summary>
        /// 根据strKey获取settinginfo
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        public BasicResponse<SettingInfo> GetSettingByKey(GetSettingByKeyRequest settingrequest)
        {
            var response = new BasicResponse<SettingInfo>();
            if (string.IsNullOrWhiteSpace(settingrequest.StrKey))
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                var settingModel = _Repository.GetSettingByKey(settingrequest.StrKey);
                var settingInfo = ObjectConverter.Copy<SettingModel, SettingInfo>(settingModel);
                response.Data = settingInfo;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                ThrowException(" 根据strKey获取settinginfo", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据条件保存配置
        /// </summary>
        /// <param name="settingrequest"></param>
        /// <returns></returns>
        public BasicResponse SaveSettingForCondition(SaveSettingForConditionRequest settingrequest)
        {
            var response = new BasicResponse();
            if (!settingrequest.State.HasValue || settingrequest.SettingInfo == null)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                if (settingrequest.State == 1)
                {
                    settingrequest.SettingInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    var settingModel = ObjectConverter.Copy<SettingInfo, SettingModel>(settingrequest.SettingInfo);
                    _Repository.AddSetting(settingModel);
                }
                else if (settingrequest.State == 2)
                {
                    var settingModel = ObjectConverter.Copy<SettingInfo, SettingModel>(settingrequest.SettingInfo);
                    _Repository.Update(settingModel);
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("SaveSetting-发生异常", ex);
            }

            return response;
        }

        /// <summary>
        /// 根据创建者获取GetSettingList
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<SettingInfo>> GetSettingListForCreator()
        {
            var response = new BasicResponse<List<SettingInfo>>();
            try
            {
                var settingModelLists = _Repository.GetSettingListForCreator();
                var settingInfoLists = ObjectConverter.CopyList<SettingModel, SettingInfo>(settingModelLists);
                response.Data = settingInfoLists.ToList();
            }
            catch (Exception ex)
            {

                response.Code = -100;
                response.Message = ex.Message;
                ThrowException(" 根据创建者获取GetSettingList", ex);
            }
            return response;
        }

        public BasicResponse<SettingInfo> GetSettingCacheByKey(GetSettingCacheByKeyRequest request)
        {
            var req = new SettingCacheGetByKeyRequest
            {
                StrKey = request.Key
            };
            var res = _SettingCacheService.GetSettingCacheByKey(req);
            var ret = new BasicResponse<SettingInfo>
            {
                Data = res.Data
            };
            return ret;
        }
    }
}


