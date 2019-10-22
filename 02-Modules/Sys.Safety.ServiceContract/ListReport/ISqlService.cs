using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract.ListReport
{
    public interface ISqlService
    {
        BasicResponse<int> ExecuteNonQueryBySql(SqlRequest req);

        BasicResponse<DataTable> QueryTableBySql(SqlRequest req);

    }
}
