using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ILargedataAnalysisLogRepository : IRepository<JC_LargedataanalysislogModel>
    {
        JC_LargedataanalysislogModel AddJC_Largedataanalysislog(JC_LargedataanalysislogModel jC_LargedataanalysislogModel);
        void UpdateJC_Largedataanalysislog(JC_LargedataanalysislogModel jC_LargedataanalysislogModel);
        void DeleteJC_Largedataanalysislog(string id);
        IList<JC_LargedataanalysislogModel> GetJC_LargedataanalysislogList(int pageIndex, int pageSize, out int rowCount);
        JC_LargedataanalysislogModel GetJC_LargedataanalysislogById(string id);

        IList<JC_LargedataanalysislogModel> GetLargedataAnalysisLogListByAnalysisModelId(string analysisModelId);

        JC_LargedataanalysislogModel GetLargedataAnalysisLogLatestByAnalysisModelId(string analysisModelId);
        IList<JC_LargedataanalysislogModel> GetLargedataAnalysisLogListByModelIdAndTime(string analysisModelId, DateTime analysisDate);
    }
}
