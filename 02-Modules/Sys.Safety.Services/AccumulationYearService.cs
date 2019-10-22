using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class AccumulationYearService : IAccumulationYearService
    {
        private readonly IAccumulationYearRepository accumulationRepository;
        public AccumulationYearService()
        {
            accumulationRepository = ServiceFactory.Create<IAccumulationYearRepository>();
        }

        public BasicResponse<Jc_Ll_YInfo> ExistsAccumulationYearInfo(AccumulationYearExistsRequest accumulationYearGetRequest)
        {
            string pointId = accumulationYearGetRequest.PointId;
            DateTime timer = accumulationYearGetRequest.Timer;

            DataTable yearTable = accumulationRepository.QueryTable("global_GetAccumulationDataByPointIdAndTime", new object[] { "CF_Year", pointId, timer });
            BasicResponse<Jc_Ll_YInfo> response = new BasicResponse<Jc_Ll_YInfo>();
            if (yearTable.Rows.Count > 0)
            {
                var yearList = accumulationRepository.ToEntityFromTable<Jc_Ll_YInfo>(yearTable);
                response.Data = yearList.FirstOrDefault();
            }

            return response;
        }
    }
}
