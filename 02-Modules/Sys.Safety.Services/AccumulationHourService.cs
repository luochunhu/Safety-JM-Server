using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    public class AccumulationHourService : IAccumulationHourService
    {
        private readonly IAccumulationHourRepository accumulationRepository;

        public AccumulationHourService()
        {
            accumulationRepository = ServiceFactory.Create<IAccumulationHourRepository>();
        }

        public Basic.Framework.Web.BasicResponse<DataContract.Jc_Ll_HInfo> ExistsAccumulationHourInfo(Sys.Safety.Request.AccumulationHourExistsRequest accumulationHourGetRequest)
        {
            string pointId = accumulationHourGetRequest.PointId;
            DateTime timer = accumulationHourGetRequest.Timer;

            DataTable hourTable = accumulationRepository.QueryTable("global_GetAccumulationDataByPointIdAndTime", new object[] { "CF_Hour" + timer.ToString("yyyyMM"), pointId, timer });

            BasicResponse<Jc_Ll_HInfo> response = new BasicResponse<Jc_Ll_HInfo>();

            if (hourTable.Rows.Count > 0)
            {
                var hourList = accumulationRepository.ToEntityFromTable<Jc_Ll_HInfo>(hourTable);
                response.Data = hourList.FirstOrDefault();
            }

            return response;
        }
    }
}
