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
    /// 累计量日表操作接口
    /// </summary>
    public interface IAccumulationDayService
    {
        BasicResponse<Jc_Ll_DInfo> ExistsAccumulationDayInfo(AccumulationDayExistsRequest accumulationDayGetRequest);
    }
}
