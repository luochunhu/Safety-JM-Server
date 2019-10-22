using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.ServiceContract.Control
{
    public interface IControlService
    {
        BasicResponse<DataTable> GetDyxFz();

        BasicResponse<DataTable> GetDyxMac();
    }
}
