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
    public class AccumulationDayService : IAccumulationDayService
    {
        private readonly IAccumulationDayRepository accumulationRepository;

        public AccumulationDayService()
        {
            accumulationRepository = ServiceFactory.Create<IAccumulationDayRepository>();
        }

        public BasicResponse<Jc_Ll_DInfo> ExistsAccumulationDayInfo(AccumulationDayExistsRequest accumulationDayGetRequest)
        {
            string pointId = accumulationDayGetRequest.PointId;
            DateTime timer = accumulationDayGetRequest.Timer;

            DataTable dayTable = accumulationRepository.QueryTable("global_GetAccumulationDataByPointIdAndTime", new object[] { "CF_Day", pointId, timer });
            BasicResponse<Jc_Ll_DInfo> response = new BasicResponse<Jc_Ll_DInfo>();
            if (dayTable.Rows.Count > 0)
            {
                var dayList = accumulationRepository.ToEntityFromTable<Jc_Ll_DInfo>(dayTable);
                response.Data = dayList.FirstOrDefault();
            }

            return response;
        }
    }
}
