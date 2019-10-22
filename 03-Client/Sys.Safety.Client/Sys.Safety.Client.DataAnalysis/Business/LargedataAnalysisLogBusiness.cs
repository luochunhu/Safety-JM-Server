using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Largedataanalysislog;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class LargedataAnalysisLogBusiness
    {
        ILargedataAnalysisLogService largedataAnalysisLogService = ServiceFactory.Create<ILargedataAnalysisLogService>();

        public List<JC_LargedataAnalysisLogInfo> AnalysisLogQueryByModelIdAndTime(string analysisModelId, string analysisDate)
        {
            LargedataAnalysisLogGetListByModelIdAndTimeRequest largedataAnalysisLogGetListByModelIdAndTimeRequest = new LargedataAnalysisLogGetListByModelIdAndTimeRequest();
            if (!string.IsNullOrEmpty(analysisModelId))
                largedataAnalysisLogGetListByModelIdAndTimeRequest.AnalysisModelId = analysisModelId;
            DateTime dtResult;
            if (!string.IsNullOrEmpty(analysisDate) && DateTime.TryParse(analysisDate, out dtResult))
            {
                largedataAnalysisLogGetListByModelIdAndTimeRequest.AnalysisDate = Convert.ToDateTime(analysisDate);
            }
            else
            {
                //默认 DateTime.Now
                largedataAnalysisLogGetListByModelIdAndTimeRequest.AnalysisDate = DateTime.Now;
            }

            BasicResponse<List<JC_LargedataAnalysisLogInfo>> analysisLogListResponse = largedataAnalysisLogService.GetLargedataAnalysisLogListByModelIdAndTime(largedataAnalysisLogGetListByModelIdAndTimeRequest);

            if (analysisLogListResponse.Data != null)
                return analysisLogListResponse.Data;
            return new List<JC_LargedataAnalysisLogInfo>();
        }
    }
}
