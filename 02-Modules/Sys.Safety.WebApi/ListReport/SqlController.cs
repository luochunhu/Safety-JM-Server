using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.ListReport;

namespace Sys.Safety.WebApi.ListReport
{
    public class SqlController : Basic.Framework.Web.WebApi.BasicApiController, ISqlService
    {
        ISqlService _sqlService = ServiceFactory.Create<ISqlService>();

        [HttpPost]
        [Route("v1/Sql/ExecuteNonQueryBySql")]
        public Basic.Framework.Web.BasicResponse<int> ExecuteNonQueryBySql(SqlRequest req)
        {
            return _sqlService.ExecuteNonQueryBySql(req);

        }

        [HttpPost]
        [Route("v1/Sql/QueryTableStringBySql")]
        public Basic.Framework.Web.BasicResponse<string> QueryTableStringBySql(SqlRequest req)
        {
            var res = _sqlService.QueryTableBySql(req);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            }; 
        }

        public Basic.Framework.Web.BasicResponse<DataTable> QueryTableBySql(SqlRequest req)
        {
            return null;
        }
    }
}
