using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Deletecheck;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class DeletecheckService:IDeletecheckService
    {
		private IDeletecheckRepository _Repository;

		public DeletecheckService(IDeletecheckRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<DeletecheckInfo> AddDeletecheck(DeletecheckAddRequest deletecheckrequest)
        {
            var _deletecheck = ObjectConverter.Copy<DeletecheckInfo, DeletecheckModel>(deletecheckrequest.DeletecheckInfo);
            var resultdeletecheck = _Repository.AddDeletecheck(_deletecheck);
            var deletecheckresponse = new BasicResponse<DeletecheckInfo>();
            deletecheckresponse.Data = ObjectConverter.Copy<DeletecheckModel, DeletecheckInfo>(resultdeletecheck);
            return deletecheckresponse;
        }
				public BasicResponse<DeletecheckInfo> UpdateDeletecheck(DeletecheckUpdateRequest deletecheckrequest)
        {
            var _deletecheck = ObjectConverter.Copy<DeletecheckInfo, DeletecheckModel>(deletecheckrequest.DeletecheckInfo);
            _Repository.UpdateDeletecheck(_deletecheck);
            var deletecheckresponse = new BasicResponse<DeletecheckInfo>();
            deletecheckresponse.Data = ObjectConverter.Copy<DeletecheckModel, DeletecheckInfo>(_deletecheck);  
            return deletecheckresponse;
        }
				public BasicResponse DeleteDeletecheck(DeletecheckDeleteRequest deletecheckrequest)
        {
            _Repository.DeleteDeletecheck(deletecheckrequest.Id);
            var deletecheckresponse = new BasicResponse();            
            return deletecheckresponse;
        }
				public BasicResponse<List<DeletecheckInfo>> GetDeletecheckList(DeletecheckGetListRequest deletecheckrequest)
        {
            var deletecheckresponse = new BasicResponse<List<DeletecheckInfo>>();
            deletecheckrequest.PagerInfo.PageIndex = deletecheckrequest.PagerInfo.PageIndex - 1;
            if (deletecheckrequest.PagerInfo.PageIndex < 0)
            {
                deletecheckrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var deletecheckModelLists = _Repository.GetDeletecheckList(deletecheckrequest.PagerInfo.PageIndex, deletecheckrequest.PagerInfo.PageSize, out rowcount);
            var deletecheckInfoLists = new List<DeletecheckInfo>();
            foreach (var item in deletecheckModelLists)
            {
                var DeletecheckInfo = ObjectConverter.Copy<DeletecheckModel, DeletecheckInfo>(item);
                deletecheckInfoLists.Add(DeletecheckInfo);
            }
            deletecheckresponse.Data = deletecheckInfoLists;
            return deletecheckresponse;
        }
				public BasicResponse<DeletecheckInfo> GetDeletecheckById(DeletecheckGetRequest deletecheckrequest)
        {
            var result = _Repository.GetDeletecheckById(deletecheckrequest.Id);
            var deletecheckInfo = ObjectConverter.Copy<DeletecheckModel, DeletecheckInfo>(result);
            var deletecheckresponse = new BasicResponse<DeletecheckInfo>();
            deletecheckresponse.Data = deletecheckInfo;
            return deletecheckresponse;
        }
	}
}


