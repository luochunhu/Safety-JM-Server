using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.ServiceContract
{
    public interface IGasContentService
    {
        BasicResponse<List<GasContentAlarmInfo>> GetAllGasContentAlarmCache();
    }
}
