using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Fklib;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class FklibService:IFklibService
    {
		private IFklibRepository _Repository;

		public FklibService(IFklibRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<FklibInfo> AddFklib(FklibAddRequest fklibrequest)
        {
            var _fklib = ObjectConverter.Copy<FklibInfo, FklibModel>(fklibrequest.FklibInfo);
            var resultfklib = _Repository.AddFklib(_fklib);
            var fklibresponse = new BasicResponse<FklibInfo>();
            fklibresponse.Data = ObjectConverter.Copy<FklibModel, FklibInfo>(resultfklib);
            return fklibresponse;
        }
				public BasicResponse<FklibInfo> UpdateFklib(FklibUpdateRequest fklibrequest)
        {
            var _fklib = ObjectConverter.Copy<FklibInfo, FklibModel>(fklibrequest.FklibInfo);
            _Repository.UpdateFklib(_fklib);
            var fklibresponse = new BasicResponse<FklibInfo>();
            fklibresponse.Data = ObjectConverter.Copy<FklibModel, FklibInfo>(_fklib);  
            return fklibresponse;
        }
				public BasicResponse DeleteFklib(FklibDeleteRequest fklibrequest)
        {
            _Repository.DeleteFklib(fklibrequest.Id);
            var fklibresponse = new BasicResponse();            
            return fklibresponse;
        }
				public BasicResponse<List<FklibInfo>> GetFklibList(FklibGetListRequest fklibrequest)
        {
            var fklibresponse = new BasicResponse<List<FklibInfo>>();
            fklibrequest.PagerInfo.PageIndex = fklibrequest.PagerInfo.PageIndex - 1;
            if (fklibrequest.PagerInfo.PageIndex < 0)
            {
                fklibrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var fklibModelLists = _Repository.GetFklibList(fklibrequest.PagerInfo.PageIndex, fklibrequest.PagerInfo.PageSize, out rowcount);
            var fklibInfoLists = new List<FklibInfo>();
            foreach (var item in fklibModelLists)
            {
                var FklibInfo = ObjectConverter.Copy<FklibModel, FklibInfo>(item);
                fklibInfoLists.Add(FklibInfo);
            }
            fklibresponse.Data = fklibInfoLists;
            return fklibresponse;
        }
				public BasicResponse<FklibInfo> GetFklibById(FklibGetRequest fklibrequest)
        {
            var result = _Repository.GetFklibById(fklibrequest.Id);
            var fklibInfo = ObjectConverter.Copy<FklibModel, FklibInfo>(result);
            var fklibresponse = new BasicResponse<FklibInfo>();
            fklibresponse.Data = fklibInfo;
            return fklibresponse;
        }
	}
}


