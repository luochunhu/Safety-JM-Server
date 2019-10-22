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
    public class AccumulationMonthService : IAccumulationMonthService
    {
        private readonly IAccumulationMonthRepository accumulationRepository;

        public AccumulationMonthService()
        {
            accumulationRepository = ServiceFactory.Create<IAccumulationMonthRepository>();
        }

        public BasicResponse<Jc_Ll_MInfo> ExistsAccumulationMonthInfo(AccumulationMonthExistsRequest accumulationMonthGetRequest)
        {
            string pointId = accumulationMonthGetRequest.PointId;
            DateTime timer = accumulationMonthGetRequest.Timer;

            DataTable monthTable = accumulationRepository.QueryTable("global_GetAccumulationDataByPointIdAndTime", new object[] { "CF_Month", pointId, timer });
            BasicResponse<Jc_Ll_MInfo> response = new BasicResponse<Jc_Ll_MInfo>();
            if (monthTable.Rows.Count > 0)
            {
                var monthList = accumulationRepository.ToEntityFromTable<Jc_Ll_MInfo>(monthTable);
                response.Data = monthList.FirstOrDefault();
            }

            return response;
        }
    }
}
