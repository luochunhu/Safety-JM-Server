using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Deptinf;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class DeptinfService:IDeptinfService
    {
		private IDeptinfRepository _Repository;

		public DeptinfService(IDeptinfRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<DeptinfInfo> AddDeptinf(DeptinfAddRequest deptinfrequest)
        {
            var _deptinf = ObjectConverter.Copy<DeptinfInfo, DeptinfModel>(deptinfrequest.DeptinfInfo);
            var resultdeptinf = _Repository.AddDeptinf(_deptinf);
            var deptinfresponse = new BasicResponse<DeptinfInfo>();
            deptinfresponse.Data = ObjectConverter.Copy<DeptinfModel, DeptinfInfo>(resultdeptinf);
            return deptinfresponse;
        }
				public BasicResponse<DeptinfInfo> UpdateDeptinf(DeptinfUpdateRequest deptinfrequest)
        {
            var _deptinf = ObjectConverter.Copy<DeptinfInfo, DeptinfModel>(deptinfrequest.DeptinfInfo);
            _Repository.UpdateDeptinf(_deptinf);
            var deptinfresponse = new BasicResponse<DeptinfInfo>();
            deptinfresponse.Data = ObjectConverter.Copy<DeptinfModel, DeptinfInfo>(_deptinf);  
            return deptinfresponse;
        }
				public BasicResponse DeleteDeptinf(DeptinfDeleteRequest deptinfrequest)
        {
            _Repository.DeleteDeptinf(deptinfrequest.Id);
            var deptinfresponse = new BasicResponse();            
            return deptinfresponse;
        }
				public BasicResponse<List<DeptinfInfo>> GetDeptinfList(DeptinfGetListRequest deptinfrequest)
        {
            var deptinfresponse = new BasicResponse<List<DeptinfInfo>>();
            deptinfrequest.PagerInfo.PageIndex = deptinfrequest.PagerInfo.PageIndex - 1;
            if (deptinfrequest.PagerInfo.PageIndex < 0)
            {
                deptinfrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var deptinfModelLists = _Repository.GetDeptinfList(deptinfrequest.PagerInfo.PageIndex, deptinfrequest.PagerInfo.PageSize, out rowcount);
            var deptinfInfoLists = new List<DeptinfInfo>();
            foreach (var item in deptinfModelLists)
            {
                var DeptinfInfo = ObjectConverter.Copy<DeptinfModel, DeptinfInfo>(item);
                deptinfInfoLists.Add(DeptinfInfo);
            }
            deptinfresponse.Data = deptinfInfoLists;
            return deptinfresponse;
        }
				public BasicResponse<DeptinfInfo> GetDeptinfById(DeptinfGetRequest deptinfrequest)
        {
            var result = _Repository.GetDeptinfById(deptinfrequest.Id);
            var deptinfInfo = ObjectConverter.Copy<DeptinfModel, DeptinfInfo>(result);
            var deptinfresponse = new BasicResponse<DeptinfInfo>();
            deptinfresponse.Data = deptinfInfo;
            return deptinfresponse;
        }
	}
}


