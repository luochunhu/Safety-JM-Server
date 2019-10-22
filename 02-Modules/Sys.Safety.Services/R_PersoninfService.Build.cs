using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Personinf;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class R_PersoninfService : IR_PersoninfService
    {
        private IR_PersoninfRepository _Repository;
        private IRPersoninfCacheService _RPersoninfCacheService;
        private IEnumcodeRepository _enumcodeRepository;
        private IR_DeptRepository _deptRepository;

        public R_PersoninfService(IR_PersoninfRepository _Repository, IRPersoninfCacheService _RPersoninfCacheService, IEnumcodeRepository enumcodeRepository, IR_DeptRepository deptRepository)
        {
            this._Repository = _Repository;
            this._RPersoninfCacheService = _RPersoninfCacheService;
            this._enumcodeRepository = enumcodeRepository;
            this._deptRepository = deptRepository;
        }
        public BasicResponse<R_PersoninfInfo> AddPersoninf(R_PersoninfAddRequest personinfRequest)
        {
            var _personinf = ObjectConverter.Copy<R_PersoninfInfo, R_PersoninfModel>(personinfRequest.PersoninfInfo);
            var resultpersoninf = _Repository.AddPersoninf(_personinf);
            var personinfresponse = new BasicResponse<R_PersoninfInfo>();

            var addpersoninfo = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(resultpersoninf);
            addpersoninfo.deptName = personinfRequest.PersoninfInfo.deptName;
            addpersoninfo.zwDesc = personinfRequest.PersoninfInfo.zwDesc;
            addpersoninfo.gzDesc = personinfRequest.PersoninfInfo.gzDesc;
            personinfresponse.Data = addpersoninfo;
            //更新缓存
            RPersoninfCacheAddRequest addpersonrequest = new RPersoninfCacheAddRequest();
            addpersonrequest.RPersoninfInfo = addpersoninfo;
            _RPersoninfCacheService.AddRPersoninfCache(addpersonrequest);

            return personinfresponse;
        }
        public BasicResponse<R_PersoninfInfo> UpdatePersoninf(R_PersoninfUpdateRequest personinfRequest)
        {
            var _personinf = ObjectConverter.Copy<R_PersoninfInfo, R_PersoninfModel>(personinfRequest.PersoninfInfo);
            _Repository.UpdatePersoninf(_personinf);
            var personinfresponse = new BasicResponse<R_PersoninfInfo>();

            var updatepersoninfo = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(_personinf);
            updatepersoninfo.deptName = personinfRequest.PersoninfInfo.deptName;
            updatepersoninfo.zwDesc = personinfRequest.PersoninfInfo.zwDesc;
            updatepersoninfo.gzDesc = personinfRequest.PersoninfInfo.gzDesc;
            personinfresponse.Data = updatepersoninfo;
            //更新缓存
            RPersoninfCacheUpdateRequest updatepersonrequest = new RPersoninfCacheUpdateRequest();
            updatepersonrequest.RPersoninfInfo = updatepersoninfo;
            _RPersoninfCacheService.UpdateRPersoninfCache(updatepersonrequest);

            personinfresponse.Data = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(_personinf);
            return personinfresponse;
        }
        public BasicResponse DeletePersoninf(R_PersoninfDeleteRequest personinfRequest)
        {
            var deletemodel = _Repository.Datas.FirstOrDefault(o => o.Id == personinfRequest.Id);
            R_PersoninfInfo deleteinfo = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(deletemodel);

            _Repository.DeletePersoninf(personinfRequest.Id);
            //更新缓存
            RPersoninfCacheDeleteRequest deletepersonrequest = new RPersoninfCacheDeleteRequest();
            deletepersonrequest.RPersoninfInfo = deleteinfo;
            _RPersoninfCacheService.DeleteRPersoninfCache(deletepersonrequest);

            var personinfresponse = new BasicResponse();
            return personinfresponse;
        }
        public BasicResponse<List<R_PersoninfInfo>> GetPersoninfList(R_PersoninfGetListRequest personinfRequest)
        {
            var personinfresponse = new BasicResponse<List<R_PersoninfInfo>>();
            personinfRequest.PagerInfo.PageIndex = personinfRequest.PagerInfo.PageIndex - 1;
            if (personinfRequest.PagerInfo.PageIndex < 0)
            {
                personinfRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var personinfModelLists = _Repository.GetPersoninfList(personinfRequest.PagerInfo.PageIndex, personinfRequest.PagerInfo.PageSize, out rowcount);
            var personinfInfoLists = new List<R_PersoninfInfo>();
            foreach (var item in personinfModelLists)
            {
                var PersoninfInfo = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(item);
                personinfInfoLists.Add(PersoninfInfo);
            }
            personinfresponse.Data = personinfInfoLists;
            return personinfresponse;
        }
        public BasicResponse<R_PersoninfInfo> GetPersoninfById(R_PersoninfGetRequest personinfRequest)
        {
            var result = _Repository.GetPersoninfById(personinfRequest.Id);
            var personinfInfo = ObjectConverter.Copy<R_PersoninfModel, R_PersoninfInfo>(result);
            var personinfresponse = new BasicResponse<R_PersoninfInfo>();
            personinfresponse.Data = personinfInfo;
            return personinfresponse;
        }


        public BasicResponse<List<R_PersoninfInfo>> GetAllPersonInfo(BasicRequest personinfRequest)
        {
            //
            var encodelist = _enumcodeRepository.Datas.ToList();
            var deptlist = _deptRepository.Datas.ToList();

            var result = _Repository.Datas.ToList();
            var personinfInfoList = ObjectConverter.CopyList<R_PersoninfModel, R_PersoninfInfo>(result);
            var personinfresponse = new BasicResponse<List<R_PersoninfInfo>>();
            personinfresponse.Data = personinfInfoList.Select(o =>
            {
                //性别
                o.Gender = ConvertGender(o.A22);
                //部门
                var dept = deptlist.FirstOrDefault(d => d.ID == o.Bm);
                o.deptName = dept == null ? string.Empty : dept.Dept;
                //职务
                var title = encodelist.FirstOrDefault(t => t.EnumTypeID == "20" && t.LngEnumValue.ToString() == o.Zw);
                o.zwDesc = title == null ? string.Empty : title.StrEnumDisplay;
                //工种
                var worktype = encodelist.FirstOrDefault(t => t.EnumTypeID == "25" && t.LngEnumValue.ToString() == o.Gz);
                o.gzDesc = worktype == null ? string.Empty : worktype.StrEnumDisplay;

                return o;
            }).ToList();
            return personinfresponse;
        }
        /// <summary>
        /// 获取所有人员缓存信息
        /// </summary>
        /// <param name="personinfRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<R_PersoninfInfo>> GetAllPersonInfoCache(BasicRequest personinfRequest)
        {
            return _RPersoninfCacheService.GetAllRPersoninfCache(new RPersoninfCacheGetAllRequest());
        }
        /// <summary>
        /// 获取所有已定义人员
        /// </summary>
        /// <param name="personinfRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<R_PersoninfInfo>> GetAllDefinedPersonInfoCache(BasicRequest personinfRequest)
        {
            RPersoninfCacheGetByConditionRequest RPersoninfCacheRequest = new RPersoninfCacheGetByConditionRequest();
            RPersoninfCacheRequest.Predicate = a => !string.IsNullOrEmpty(a.Name);
            return _RPersoninfCacheService.GetRPersoninfCache(RPersoninfCacheRequest);
        }
        private string ConvertGender(string value)
        {
            if (value == "0")
                return "男";
            else if (value == "1")
                return "女";
            return string.Empty;
        }

        private R_PersoninfInfo InitPersonExtendProperty(R_PersoninfInfo personinfo)
        {
            var encodelist = _enumcodeRepository.Datas.ToList();
            var deptlist = _deptRepository.Datas.ToList();
            //性别
            personinfo.Gender = ConvertGender(personinfo.A22);
            //部门
            var dept = deptlist.FirstOrDefault(d => d.ID == personinfo.Bm);
            personinfo.deptName = dept == null ? string.Empty : dept.Dept;
            //职务
            var title = encodelist.FirstOrDefault(t => t.EnumTypeID == "20" && t.LngEnumValue.ToString() == personinfo.Zw);
            personinfo.zwDesc = title == null ? string.Empty : title.StrEnumDisplay;
            //工种
            var worktype = encodelist.FirstOrDefault(t => t.EnumTypeID == "25" && t.LngEnumValue.ToString() == personinfo.Gz);
            personinfo.gzDesc = worktype == null ? string.Empty : worktype.StrEnumDisplay;

            return personinfo;
        }

        public BasicResponse<R_PersoninfInfo> GetPersoninfCache(R_PersoninfGetRequest personinfRequest)
        {
            return _RPersoninfCacheService.GetByKeyRPersoninfCache(new RPersoninfCacheGetByKeyRequest() { Id = personinfRequest.Id });
        }
        public BasicResponse<List<R_PersoninfInfo>> GetPersoninfCacheByBh(R_PersoninfGetByBhRequest personinfRequest)
        {
            RPersoninfCacheGetByConditionRequest RPersoninfCacheRequest = new RPersoninfCacheGetByConditionRequest();
            RPersoninfCacheRequest.Predicate = a => a.Bh == personinfRequest.Bh;
            return _RPersoninfCacheService.GetRPersoninfCache(RPersoninfCacheRequest);
        }
    }
}


