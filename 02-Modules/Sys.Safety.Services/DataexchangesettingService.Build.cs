using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Dataexchangesetting;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class DataexchangesettingService:IDataexchangesettingService
    {
		private IDataexchangesettingRepository _Repository;

		public DataexchangesettingService(IDataexchangesettingRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<DataexchangesettingInfo> AddDataexchangesetting(DataexchangesettingAddRequest dataexchangesettingrequest)
        {
            var _dataexchangesetting = ObjectConverter.Copy<DataexchangesettingInfo, DataexchangesettingModel>(dataexchangesettingrequest.DataexchangesettingInfo);
            var resultdataexchangesetting = _Repository.AddDataexchangesetting(_dataexchangesetting);
            var dataexchangesettingresponse = new BasicResponse<DataexchangesettingInfo>();
            dataexchangesettingresponse.Data = ObjectConverter.Copy<DataexchangesettingModel, DataexchangesettingInfo>(resultdataexchangesetting);
            return dataexchangesettingresponse;
        }
				public BasicResponse<DataexchangesettingInfo> UpdateDataexchangesetting(DataexchangesettingUpdateRequest dataexchangesettingrequest)
        {
            var _dataexchangesetting = ObjectConverter.Copy<DataexchangesettingInfo, DataexchangesettingModel>(dataexchangesettingrequest.DataexchangesettingInfo);
            _Repository.UpdateDataexchangesetting(_dataexchangesetting);
            var dataexchangesettingresponse = new BasicResponse<DataexchangesettingInfo>();
            dataexchangesettingresponse.Data = ObjectConverter.Copy<DataexchangesettingModel, DataexchangesettingInfo>(_dataexchangesetting);  
            return dataexchangesettingresponse;
        }
				public BasicResponse DeleteDataexchangesetting(DataexchangesettingDeleteRequest dataexchangesettingrequest)
        {
            _Repository.DeleteDataexchangesetting(dataexchangesettingrequest.Id);
            var dataexchangesettingresponse = new BasicResponse();            
            return dataexchangesettingresponse;
        }
				public BasicResponse<List<DataexchangesettingInfo>> GetDataexchangesettingList(DataexchangesettingGetListRequest dataexchangesettingrequest)
        {
            var dataexchangesettingresponse = new BasicResponse<List<DataexchangesettingInfo>>();
            dataexchangesettingrequest.PagerInfo.PageIndex = dataexchangesettingrequest.PagerInfo.PageIndex - 1;
            if (dataexchangesettingrequest.PagerInfo.PageIndex < 0)
            {
                dataexchangesettingrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var dataexchangesettingModelLists = _Repository.GetDataexchangesettingList(dataexchangesettingrequest.PagerInfo.PageIndex, dataexchangesettingrequest.PagerInfo.PageSize, out rowcount);
            var dataexchangesettingInfoLists = new List<DataexchangesettingInfo>();
            foreach (var item in dataexchangesettingModelLists)
            {
                var DataexchangesettingInfo = ObjectConverter.Copy<DataexchangesettingModel, DataexchangesettingInfo>(item);
                dataexchangesettingInfoLists.Add(DataexchangesettingInfo);
            }
            dataexchangesettingresponse.Data = dataexchangesettingInfoLists;
            return dataexchangesettingresponse;
        }
				public BasicResponse<DataexchangesettingInfo> GetDataexchangesettingById(DataexchangesettingGetRequest dataexchangesettingrequest)
        {
            var result = _Repository.GetDataexchangesettingById(dataexchangesettingrequest.Id);
            var dataexchangesettingInfo = ObjectConverter.Copy<DataexchangesettingModel, DataexchangesettingInfo>(result);
            var dataexchangesettingresponse = new BasicResponse<DataexchangesettingInfo>();
            dataexchangesettingresponse.Data = dataexchangesettingInfo;
            return dataexchangesettingresponse;
        }
	}
}


