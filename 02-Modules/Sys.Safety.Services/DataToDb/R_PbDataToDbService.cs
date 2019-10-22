using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Processing.DataToDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.DataToDb
{
    public class R_PbDataToDbService : InsertToDbService<R_PbInfo>
    {
        public override Basic.Framework.Web.BasicResponse AddItem(Sys.Safety.Request.DataToDb.DataToDbAddRequest<R_PbInfo> dataToDbRequest)
        {
            R_PbDataToDb.Instance.AddItem(dataToDbRequest.Item);
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse AddItems(Sys.Safety.Request.DataToDb.DataToDbBatchAddRequest<R_PbInfo> dataToDbRequest)
        {
            R_PbDataToDb.Instance.AddItems(dataToDbRequest.Items);
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse Start(Sys.Safety.Request.DataToDb.DataToDbStartRequest dataToDbRequest)
        {
            R_PbDataToDb.Instance.Start();
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse Stop(Sys.Safety.Request.DataToDb.DataToDbStopRequest dataToDbRequest)
        {
            R_PbDataToDb.Instance.Stop();
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse<int> GetQueueBacklog()
        {
            BasicResponse<int> result = new BasicResponse<int>();
            result.Data = R_PbDataToDb.Instance.GetInsertItemListCount();
            return result;
        }

        public override Basic.Framework.Web.BasicResponse<long> GetTotalCount()
        {
            BasicResponse<long> result = new BasicResponse<long>();
            result.Data = R_PbDataToDb.Instance.GetTotalCount();
            return result;
        }
    }
}
