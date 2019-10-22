using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Enumtype;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class EnumtypeService : IEnumtypeService
    {
        private IEnumtypeRepository _Repository;

        public EnumtypeService(IEnumtypeRepository _Repository)
        {
            this._Repository = _Repository;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("EnumTypeService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 保存枚举类型
        /// </summary>
        /// <param name="enumtyperequest"></param>
        /// <returns></returns>
        public BasicResponse<EnumtypeInfo> SaveEnumType(EnumtypeAddRequest enumtyperequest)
        {
            BasicResponse<EnumtypeInfo> enumdto = new BasicResponse<EnumtypeInfo>();
            EnumtypeInfo dto = enumtyperequest.EnumtypeInfo;
            if (dto == null)
            {
                //throw new Exception("枚举类型对象为空，请检查是否已赋值！");
                ThrowException("SaveEnumType", new Exception("枚举类型对象为空，请检查是否已赋值！"));
            }
            if (dto.StrCode.Trim() == "")
            {
                // throw new Exception("枚举类型编码不能为空！");
                ThrowException("SaveEnumType", new Exception("枚举类型编码不能为空！"));
            }
            if (dto.StrName.Trim() == "")
            {
                //throw new Exception("枚举类型名称不能为空！");
                ThrowException("SaveEnumType", new Exception("枚举类型名称不能为空！"));
            }
            if (dto.InfoState == InfoState.NoChange)
            {
                //throw new Exception("DTO对象未设置状态，请先设置！");
                ThrowException("SaveEnumType", new Exception("DTO对象未设置状态，请先设置！"));
            }
            try
            {
                string ID = "0";
                if (dto.InfoState == InfoState.AddNew)
                {
                    dto.ID = "0";
                    dto.EnumTypeID = "0";
                    
                    AddEnumtype(enumtyperequest);

                    var result = _Repository.GetEnumtypeByStrCode(enumtyperequest.EnumtypeInfo.StrCode);
                    var enumtypeInfo = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(result);
                    ID = enumtypeInfo.ID;
                    //将自增ID赋值给枚举类型ID
                    EnumtypeUpdateRequest updaterequest = new EnumtypeUpdateRequest();
                    updaterequest.EnumtypeInfo = enumtypeInfo;
                    updaterequest.EnumtypeInfo.EnumTypeID = ID;
                    enumdto = UpdateEnumtype(updaterequest);
                }
                else if (dto.InfoState == InfoState.Modified)
                {
                    ID = dto.ID;
                    EnumtypeUpdateRequest updaterequest = new EnumtypeUpdateRequest();
                    updaterequest.EnumtypeInfo = dto;
                    enumdto = UpdateEnumtype(updaterequest);
                }
                else if (dto.InfoState == InfoState.Delete)
                {
                    _Repository.DeleteEnumtype(dto.ID);
                }
            }
            catch (Exception ex)
            {
                ThrowException("SaveEnumType", ex);
            }
            return enumdto;
        }
       
        public BasicResponse<EnumtypeInfo> AddEnumtype(EnumtypeAddRequest enumtyperequest)
        {
            var _enumtype = ObjectConverter.Copy<EnumtypeInfo, EnumtypeModel>(enumtyperequest.EnumtypeInfo);
            var resultenumtype = _Repository.AddEnumtype(_enumtype);
            var enumtyperesponse = new BasicResponse<EnumtypeInfo>();
            enumtyperesponse.Data = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(resultenumtype);
            return enumtyperesponse;
        }
        public BasicResponse<EnumtypeInfo> UpdateEnumtype(EnumtypeUpdateRequest enumtyperequest)
        {
            var _enumtype = ObjectConverter.Copy<EnumtypeInfo, EnumtypeModel>(enumtyperequest.EnumtypeInfo);
            _Repository.UpdateEnumtype(_enumtype);
            var enumtyperesponse = new BasicResponse<EnumtypeInfo>();
            enumtyperesponse.Data = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(_enumtype);
            return enumtyperesponse;
        }
        public BasicResponse DeleteEnumtype(EnumtypeDeleteRequest enumtyperequest)
        {
            _Repository.DeleteEnumtype(enumtyperequest.Id);
            var enumtyperesponse = new BasicResponse();
            return enumtyperesponse;
        }
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList(EnumtypeGetListRequest enumtyperequest)
        {
            var enumtyperesponse = new BasicResponse<List<EnumtypeInfo>>();
            enumtyperequest.PagerInfo.PageIndex = enumtyperequest.PagerInfo.PageIndex - 1;
            if (enumtyperequest.PagerInfo.PageIndex < 0)
            {
                enumtyperequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var enumtypeModelLists = _Repository.GetEnumtypeList(enumtyperequest.PagerInfo.PageIndex, enumtyperequest.PagerInfo.PageSize, out rowcount);
            var enumtypeInfoLists = new List<EnumtypeInfo>();
            foreach (var item in enumtypeModelLists)
            {
                var EnumtypeInfo = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(item);
                enumtypeInfoLists.Add(EnumtypeInfo);
            }
            enumtyperesponse.Data = enumtypeInfoLists;
            return enumtyperesponse;
        }
        public BasicResponse<List<EnumtypeInfo>> GetEnumtypeList()
        {
            var enumtyperesponse = new BasicResponse<List<EnumtypeInfo>>();            
            var enumtypeModelLists = _Repository.GetEnumtypeList();
            var enumtypeInfoLists = new List<EnumtypeInfo>();
            foreach (var item in enumtypeModelLists)
            {
                var EnumtypeInfo = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(item);
                enumtypeInfoLists.Add(EnumtypeInfo);
            }
            enumtyperesponse.Data = enumtypeInfoLists;
            return enumtyperesponse;
        }
        public BasicResponse<EnumtypeInfo> GetEnumtypeById(EnumtypeGetRequest enumtyperequest)
        {
            var result = _Repository.GetEnumtypeById(enumtyperequest.Id);
            var enumtypeInfo = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(result);
            var enumtyperesponse = new BasicResponse<EnumtypeInfo>();
            enumtyperesponse.Data = enumtypeInfo;
            return enumtyperesponse;
        }
        public BasicResponse<EnumtypeInfo> GetEnumtypeByStrCode(EnumtypeGetByStrCodeRequest enumtyperequest)
        {
            var result = _Repository.GetEnumtypeByStrCode(enumtyperequest.StrCode);
            var enumtypeInfo = ObjectConverter.Copy<EnumtypeModel, EnumtypeInfo>(result);
            var enumtyperesponse = new BasicResponse<EnumtypeInfo>();
            enumtyperesponse.Data = enumtypeInfo;
            return enumtyperesponse;
        }
    }
}


