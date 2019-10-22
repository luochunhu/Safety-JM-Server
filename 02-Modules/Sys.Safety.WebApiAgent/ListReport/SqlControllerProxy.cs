using Sys.Safety.ServiceContract.ListReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.WebApiAgent.ListReport
{
    public class SqlControllerProxy : BaseProxy, ISqlService
    {
        public Basic.Framework.Web.BasicResponse<int> ExecuteNonQueryBySql(SqlRequest req)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Sql/ExecuteNonQueryBySql?token=" + Token, JSONHelper.ToJSONString(req));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<string> QueryTableBySqlString(SqlRequest req)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Sql/QueryTableStringBySql?token=" + Token, JSONHelper.ToJSONString(req));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            return res;
        }

        public Basic.Framework.Web.BasicResponse<DataTable> QueryTableBySql(SqlRequest req)
        {
            var res = QueryTableBySqlString(req);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }
    }
}
