using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Dataright;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class DatarightService:IDatarightService
    {
		private IDatarightRepository _Repository;

		public DatarightService(IDatarightRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<DatarightInfo> AddDataright(DatarightAddRequest datarightrequest)
        {
            var _dataright = ObjectConverter.Copy<DatarightInfo, DatarightModel>(datarightrequest.DatarightInfo);
            var resultdataright = _Repository.AddDataright(_dataright);
            var datarightresponse = new BasicResponse<DatarightInfo>();
            datarightresponse.Data = ObjectConverter.Copy<DatarightModel, DatarightInfo>(resultdataright);
            return datarightresponse;
        }
				public BasicResponse<DatarightInfo> UpdateDataright(DatarightUpdateRequest datarightrequest)
        {
            var _dataright = ObjectConverter.Copy<DatarightInfo, DatarightModel>(datarightrequest.DatarightInfo);
            _Repository.UpdateDataright(_dataright);
            var datarightresponse = new BasicResponse<DatarightInfo>();
            datarightresponse.Data = ObjectConverter.Copy<DatarightModel, DatarightInfo>(_dataright);  
            return datarightresponse;
        }
				public BasicResponse DeleteDataright(DatarightDeleteRequest datarightrequest)
        {
            _Repository.DeleteDataright(datarightrequest.Id);
            var datarightresponse = new BasicResponse();            
            return datarightresponse;
        }
				public BasicResponse<List<DatarightInfo>> GetDatarightList(DatarightGetListRequest datarightrequest)
        {
            var datarightresponse = new BasicResponse<List<DatarightInfo>>();
            datarightrequest.PagerInfo.PageIndex = datarightrequest.PagerInfo.PageIndex - 1;
            if (datarightrequest.PagerInfo.PageIndex < 0)
            {
                datarightrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var datarightModelLists = _Repository.GetDatarightList(datarightrequest.PagerInfo.PageIndex, datarightrequest.PagerInfo.PageSize, out rowcount);
            var datarightInfoLists = new List<DatarightInfo>();
            foreach (var item in datarightModelLists)
            {
                var DatarightInfo = ObjectConverter.Copy<DatarightModel, DatarightInfo>(item);
                datarightInfoLists.Add(DatarightInfo);
            }
            datarightresponse.Data = datarightInfoLists;
            return datarightresponse;
        }
				public BasicResponse<DatarightInfo> GetDatarightById(DatarightGetRequest datarightrequest)
        {
            var result = _Repository.GetDatarightById(datarightrequest.Id);
            var datarightInfo = ObjectConverter.Copy<DatarightModel, DatarightInfo>(result);
            var datarightresponse = new BasicResponse<DatarightInfo>();
            datarightresponse.Data = datarightInfo;
            return datarightresponse;
        }
	}
}


