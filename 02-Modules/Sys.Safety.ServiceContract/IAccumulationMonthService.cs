using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    /// <summary>
    /// 累积量月表数据
    /// </summary>
    public interface IAccumulationMonthService
    {
        BasicResponse<Jc_Ll_MInfo> ExistsAccumulationMonthInfo(AccumulationMonthExistsRequest accumulationMonthGetRequest);
    }
}
