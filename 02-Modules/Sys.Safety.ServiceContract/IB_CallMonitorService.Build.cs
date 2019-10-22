using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.R_Call;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_CallMonitorService
    {
        /// <summary>
        /// 监听呼叫（只调用广播服务器操作，不记录历史数据）
        /// </summary>
        /// <param name="callRequest"></param>
        /// <returns></returns>
        BasicResponse monitorCall(B_CallMonitorRequest callRequest);
    }
}

