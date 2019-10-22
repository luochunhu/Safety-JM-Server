using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Sysinf;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class SysinfService:ISysinfService
    {
		private ISysinfRepository _Repository;

		public SysinfService(ISysinfRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<SysinfInfo> AddSysinf(SysinfAddRequest sysinfrequest)
        {
            var _sysinf = ObjectConverter.Copy<SysinfInfo, SysinfModel>(sysinfrequest.SysinfInfo);
            var resultsysinf = _Repository.AddSysinf(_sysinf);
            var sysinfresponse = new BasicResponse<SysinfInfo>();
            sysinfresponse.Data = ObjectConverter.Copy<SysinfModel, SysinfInfo>(resultsysinf);
            return sysinfresponse;
        }
				public BasicResponse<SysinfInfo> UpdateSysinf(SysinfUpdateRequest sysinfrequest)
        {
            var _sysinf = ObjectConverter.Copy<SysinfInfo, SysinfModel>(sysinfrequest.SysinfInfo);
            _Repository.UpdateSysinf(_sysinf);
            var sysinfresponse = new BasicResponse<SysinfInfo>();
            sysinfresponse.Data = ObjectConverter.Copy<SysinfModel, SysinfInfo>(_sysinf);  
            return sysinfresponse;
        }
				public BasicResponse DeleteSysinf(SysinfDeleteRequest sysinfrequest)
        {
            _Repository.DeleteSysinf(sysinfrequest.Id);
            var sysinfresponse = new BasicResponse();            
            return sysinfresponse;
        }
				public BasicResponse<List<SysinfInfo>> GetSysinfList(SysinfGetListRequest sysinfrequest)
        {
            var sysinfresponse = new BasicResponse<List<SysinfInfo>>();
            sysinfrequest.PagerInfo.PageIndex = sysinfrequest.PagerInfo.PageIndex - 1;
            if (sysinfrequest.PagerInfo.PageIndex < 0)
            {
                sysinfrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var sysinfModelLists = _Repository.GetSysinfList(sysinfrequest.PagerInfo.PageIndex, sysinfrequest.PagerInfo.PageSize, out rowcount);
            var sysinfInfoLists = new List<SysinfInfo>();
            foreach (var item in sysinfModelLists)
            {
                var SysinfInfo = ObjectConverter.Copy<SysinfModel, SysinfInfo>(item);
                sysinfInfoLists.Add(SysinfInfo);
            }
            sysinfresponse.Data = sysinfInfoLists;
            return sysinfresponse;
        }
				public BasicResponse<SysinfInfo> GetSysinfById(SysinfGetRequest sysinfrequest)
        {
            var result = _Repository.GetSysinfById(sysinfrequest.Id);
            var sysinfInfo = ObjectConverter.Copy<SysinfModel, SysinfInfo>(result);
            var sysinfresponse = new BasicResponse<SysinfInfo>();
            sysinfresponse.Data = sysinfInfo;
            return sysinfresponse;
        }
	}
}


