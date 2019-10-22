using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Dept;

namespace Sys.Safety.Services
{
    public partial class R_DeptService : IR_DeptService
    {
        private IR_DeptRepository _Repository;
        private IR_PersoninfRepository _personRepository;
        private IEnumcodeRepository _enumcodeRepository;

        public R_DeptService(IR_DeptRepository _Repository, IR_PersoninfRepository personRepository, IEnumcodeRepository enumcodeRepository)
        {
            this._Repository = _Repository;
            this._personRepository = personRepository;
            this._enumcodeRepository = enumcodeRepository;
        }
        public BasicResponse<R_DeptInfo> AddDept(R_DeptAddRequest deptRequest)
        {
            var _dept = ObjectConverter.Copy<R_DeptInfo, R_DeptModel>(deptRequest.DeptInfo);
            var resultdept = _Repository.AddDept(_dept);
            var deptresponse = new BasicResponse<R_DeptInfo>();
            deptresponse.Data = ObjectConverter.Copy<R_DeptModel, R_DeptInfo>(resultdept);
            return deptresponse;
        }
        public BasicResponse<R_DeptInfo> UpdateDept(R_DeptUpdateRequest deptRequest)
        {
            var _dept = ObjectConverter.Copy<R_DeptInfo, R_DeptModel>(deptRequest.DeptInfo);
            _Repository.UpdateDept(_dept);
            var deptresponse = new BasicResponse<R_DeptInfo>();
            deptresponse.Data = ObjectConverter.Copy<R_DeptModel, R_DeptInfo>(_dept);
            return deptresponse;
        }
        public BasicResponse DeleteDept(R_DeptDeleteRequest deptRequest)
        {
            _Repository.DeleteDept(deptRequest.Id);
            var deptresponse = new BasicResponse();
            return deptresponse;
        }
        public BasicResponse<List<R_DeptInfo>> GetDeptList(R_DeptGetListRequest deptRequest)
        {
            var deptresponse = new BasicResponse<List<R_DeptInfo>>();
            deptRequest.PagerInfo.PageIndex = deptRequest.PagerInfo.PageIndex - 1;
            if (deptRequest.PagerInfo.PageIndex < 0)
            {
                deptRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var deptModelLists = _Repository.GetDeptList(deptRequest.PagerInfo.PageIndex, deptRequest.PagerInfo.PageSize, out rowcount);
            var deptInfoLists = new List<R_DeptInfo>();
            foreach (var item in deptModelLists)
            {
                var DeptInfo = ObjectConverter.Copy<R_DeptModel, R_DeptInfo>(item);
                deptInfoLists.Add(DeptInfo);
            }
            deptresponse.Data = deptInfoLists;
            return deptresponse;
        }
        public BasicResponse<R_DeptInfo> GetDeptById(R_DeptGetRequest deptRequest)
        {
            var result = _Repository.GetDeptById(deptRequest.Id);
            var deptInfo = ObjectConverter.Copy<R_DeptModel, R_DeptInfo>(result);
            var deptresponse = new BasicResponse<R_DeptInfo>();
            deptresponse.Data = deptInfo;
            return deptresponse;
        }

        public BasicResponse<List<R_DeptInfo>> GetAllDept(BasicRequest deptRequest) 
        {
            var persons = _personRepository.Datas.ToList();

            var result = _Repository.Datas.ToList();
            var deptinfolist = ObjectConverter.CopyList<R_DeptModel, R_DeptInfo>(result).ToList();

            var titles=_enumcodeRepository.Datas.Where(t => t.EnumTypeID == "20" );

            deptinfolist.ForEach(o =>
            {
                var manager = persons.FirstOrDefault(p => p.Id == o.Manager);
                o.ManagerName = manager == null ? string.Empty : manager.Name;
                var title=titles.FirstOrDefault(t=>t.LngEnumValue.ToString()==o.Zu);
                o.ManagerTitle = title == null ? string.Empty : title.StrEnumDisplay;
            });

            var deptresponse = new BasicResponse<List<R_DeptInfo>>();
            deptresponse.Data = deptinfolist.ToList();
            return deptresponse;
        }
    }
}


