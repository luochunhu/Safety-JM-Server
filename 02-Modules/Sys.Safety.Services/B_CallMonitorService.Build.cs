using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 监听呼叫
    /// </summary>
    public partial class B_CallMonitorService : IB_CallMonitorService
    {               
        /// <summary>
        /// 监听呼叫（只调用广播服务器操作，不记录历史数据）
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        public BasicResponse monitorCall(B_CallMonitorRequest callRequest)
        {
            var callresponse = new BasicResponse();

            return callresponse;
        }
    }
}


