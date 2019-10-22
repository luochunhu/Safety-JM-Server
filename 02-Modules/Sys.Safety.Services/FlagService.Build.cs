using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Flag;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class FlagService:IFlagService
    {
		private IFlagRepository _Repository;

		public FlagService(IFlagRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<FlagInfo> AddFlag(FlagAddRequest flagrequest)
        {
            var _flag = ObjectConverter.Copy<FlagInfo, FlagModel>(flagrequest.FlagInfo);
            var resultflag = _Repository.AddFlag(_flag);
            var flagresponse = new BasicResponse<FlagInfo>();
            flagresponse.Data = ObjectConverter.Copy<FlagModel, FlagInfo>(resultflag);
            return flagresponse;
        }
				public BasicResponse<FlagInfo> UpdateFlag(FlagUpdateRequest flagrequest)
        {
            var _flag = ObjectConverter.Copy<FlagInfo, FlagModel>(flagrequest.FlagInfo);
            _Repository.UpdateFlag(_flag);
            var flagresponse = new BasicResponse<FlagInfo>();
            flagresponse.Data = ObjectConverter.Copy<FlagModel, FlagInfo>(_flag);  
            return flagresponse;
        }
				public BasicResponse DeleteFlag(FlagDeleteRequest flagrequest)
        {
            _Repository.DeleteFlag(flagrequest.Id);
            var flagresponse = new BasicResponse();            
            return flagresponse;
        }
				public BasicResponse<List<FlagInfo>> GetFlagList(FlagGetListRequest flagrequest)
        {
            var flagresponse = new BasicResponse<List<FlagInfo>>();
            flagrequest.PagerInfo.PageIndex = flagrequest.PagerInfo.PageIndex - 1;
            if (flagrequest.PagerInfo.PageIndex < 0)
            {
                flagrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var flagModelLists = _Repository.GetFlagList(flagrequest.PagerInfo.PageIndex, flagrequest.PagerInfo.PageSize, out rowcount);
            var flagInfoLists = new List<FlagInfo>();
            foreach (var item in flagModelLists)
            {
                var FlagInfo = ObjectConverter.Copy<FlagModel, FlagInfo>(item);
                flagInfoLists.Add(FlagInfo);
            }
            flagresponse.Data = flagInfoLists;
            return flagresponse;
        }
				public BasicResponse<FlagInfo> GetFlagById(FlagGetRequest flagrequest)
        {
            var result = _Repository.GetFlagById(flagrequest.Id);
            var flagInfo = ObjectConverter.Copy<FlagModel, FlagInfo>(result);
            var flagresponse = new BasicResponse<FlagInfo>();
            flagresponse.Data = flagInfo;
            return flagresponse;
        }
	}
}


