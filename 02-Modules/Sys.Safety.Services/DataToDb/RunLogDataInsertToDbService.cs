using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using System.Collections.Generic;
using System;
using Basic.Framework.Configuration;
using System.Linq;
using Basic.Framework.Logging;
using Sys.Safety.Processing.DataToDb;
using Basic.Framework.Web;

namespace Sys.Safety.Services.DataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-26
    /// 描述:运行记录入库服务
    /// 修改记录
    /// 2017-05-26
    /// </summary>
    public class RunLogDataInsertToDbService : InsertToDbService<Jc_RInfo>
    {
        public override Basic.Framework.Web.BasicResponse AddItem(Sys.Safety.Request.DataToDb.DataToDbAddRequest<Jc_RInfo> dataToDbRequest)
        {
            RunLogDataInsertToDb.Instance.AddItem(dataToDbRequest.Item);
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse AddItems(Sys.Safety.Request.DataToDb.DataToDbBatchAddRequest<Jc_RInfo> dataToDbRequest)
        {
            RunLogDataInsertToDb.Instance.AddItems(dataToDbRequest.Items);
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse Start(Sys.Safety.Request.DataToDb.DataToDbStartRequest dataToDbRequest)
        {
            RunLogDataInsertToDb.Instance.Start();
            return new BasicResponse();
        }

        public override Basic.Framework.Web.BasicResponse Stop(Sys.Safety.Request.DataToDb.DataToDbStopRequest dataToDbRequest)
        {
            RunLogDataInsertToDb.Instance.Stop();
            return new BasicResponse();
        }
        /// <summary>
        /// 获取队列积压数量  20170717
        /// </summary>
        /// <returns></returns>
        public override BasicResponse<int> GetQueueBacklog()
        {
            BasicResponse<int> result = new BasicResponse<int>();
            result.Data = RunLogDataInsertToDb.Instance.GetInsertItemListCount();
            return result;
        }
        /// <summary>
        /// 获取总处理量
        /// </summary>
        /// <returns></returns>
        public override BasicResponse<long> GetTotalCount()
        {
            BasicResponse<long> result = new BasicResponse<long>();
            result.Data = RunLogDataInsertToDb.Instance.GetTotalCount();
            return result;
        }
    }
}
