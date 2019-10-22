using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Bxex;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class CalibrationStatisticsService:ICalibrationStatisticsService
    {
		private ICalibrationStatisticsRepository _Repository;

		public CalibrationStatisticsService(ICalibrationStatisticsRepository _Repository)
		{
			this._Repository = _Repository; 
		}
				public BasicResponse<Jc_BxexInfo> AddCalibrationStatistics(Jc_BxexAddRequest jc_Bxexrequest)
        {
            var _jc_Bxex = ObjectConverter.Copy<Jc_BxexInfo, Jc_BxexModel>(jc_Bxexrequest.Jc_BxexInfo);
            var resultjc_Bxex = _Repository.AddCalibrationStatistics(_jc_Bxex);
            var jc_Bxexresponse = new BasicResponse<Jc_BxexInfo>();
            jc_Bxexresponse.Data = ObjectConverter.Copy<Jc_BxexModel, Jc_BxexInfo>(resultjc_Bxex);
            return jc_Bxexresponse;
        }
				public BasicResponse<Jc_BxexInfo> UpdateCalibrationStatistics(Jc_BxexUpdateRequest jc_Bxexrequest)
        {
            var _jc_Bxex = ObjectConverter.Copy<Jc_BxexInfo, Jc_BxexModel>(jc_Bxexrequest.Jc_BxexInfo);
            _Repository.UpdateCalibrationStatistics(_jc_Bxex);
            var jc_Bxexresponse = new BasicResponse<Jc_BxexInfo>();
            jc_Bxexresponse.Data = ObjectConverter.Copy<Jc_BxexModel, Jc_BxexInfo>(_jc_Bxex);  
            return jc_Bxexresponse;
        }
				public BasicResponse DeleteCalibrationStatistics(Jc_BxexDeleteRequest jc_Bxexrequest)
        {
            _Repository.DeleteCalibrationStatistics(jc_Bxexrequest.Id);
            var jc_Bxexresponse = new BasicResponse();            
            return jc_Bxexresponse;
        }
				public BasicResponse<List<Jc_BxexInfo>> GetCalibrationStatisticsList(Jc_BxexGetListRequest jc_Bxexrequest)
        {
            var jc_Bxexresponse = new BasicResponse<List<Jc_BxexInfo>>();
            jc_Bxexrequest.PagerInfo.PageIndex = jc_Bxexrequest.PagerInfo.PageIndex - 1;
            if (jc_Bxexrequest.PagerInfo.PageIndex < 0)
            {
                jc_Bxexrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_BxexModelLists = _Repository.GetCalibrationStatisticsList(jc_Bxexrequest.PagerInfo.PageIndex, jc_Bxexrequest.PagerInfo.PageSize, out rowcount);
            var jc_BxexInfoLists = new List<Jc_BxexInfo>();
            foreach (var item in jc_BxexModelLists)
            {
                var Jc_BxexInfo = ObjectConverter.Copy<Jc_BxexModel, Jc_BxexInfo>(item);
                jc_BxexInfoLists.Add(Jc_BxexInfo);
            }
            jc_Bxexresponse.Data = jc_BxexInfoLists;
            return jc_Bxexresponse;
        }
				public BasicResponse<Jc_BxexInfo> GetCalibrationStatisticsById(Jc_BxexGetRequest jc_Bxexrequest)
        {
            var result = _Repository.GetCalibrationStatisticsById(jc_Bxexrequest.Id);
            var jc_BxexInfo = ObjectConverter.Copy<Jc_BxexModel, Jc_BxexInfo>(result);
            var jc_Bxexresponse = new BasicResponse<Jc_BxexInfo>();
            jc_Bxexresponse.Data = jc_BxexInfo;
            return jc_Bxexresponse;
        }
	}
}


