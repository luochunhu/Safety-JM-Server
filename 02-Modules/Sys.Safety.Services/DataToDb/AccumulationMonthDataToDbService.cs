using Basic.Framework.Common;
using Basic.Framework.Configuration;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request.DataToDb;
using Basic.Framework.Logging;
using Sys.Safety.Processing.DataToDb;

namespace Sys.Safety.Services.DataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-06-02
    /// 描述:累积量月数据入库服务
    /// 修改记录
    /// 2017-06-02
    /// </summary>
    public class AccumulationMonthDataToDbService : InsertToDbService<Jc_Ll_MInfo>
    {
        public override BasicResponse AddItem(DataToDbAddRequest<Jc_Ll_MInfo> dataToDbRequest)
        {
            AccumulationMonthDataToDb.Instance.AddItem(dataToDbRequest.Item);
            return new BasicResponse();
        }

        public override BasicResponse AddItems(DataToDbBatchAddRequest<Jc_Ll_MInfo> dataToDbRequest)
        {
            AccumulationMonthDataToDb.Instance.AddItems(dataToDbRequest.Items);
            return new BasicResponse();
        }

        public override BasicResponse Start(DataToDbStartRequest dataToDbRequest)
        {
            AccumulationMonthDataToDb.Instance.Start();
            return new BasicResponse();
        }

        public override BasicResponse Stop(DataToDbStopRequest dataToDbRequest)
        {
            AccumulationMonthDataToDb.Instance.Stop();
            return new BasicResponse();
        }
        /// <summary>
        /// 获取队列积压数量  20170717
        /// </summary>
        /// <returns></returns>
        public override BasicResponse<int> GetQueueBacklog()
        {
            BasicResponse<int> result = new BasicResponse<int>();
            result.Data = AccumulationMonthDataToDb.Instance.GetInsertItemListCount();
            return result;
        }

        /// <summary>
        /// 获取总处理量
        /// </summary>
        /// <returns></returns>
        public override BasicResponse<long> GetTotalCount()
        {
            BasicResponse<long> result = new BasicResponse<long>();
            result.Data = AccumulationMonthDataToDb.Instance.GetTotalCount();
            return result;
        }
    }
}
