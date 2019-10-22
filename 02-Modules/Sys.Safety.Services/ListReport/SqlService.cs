using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Model;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.ListReport;

namespace Sys.Safety.Services.ListReport
{
    public class SqlService: ISqlService
    {
        private readonly RepositoryBase<ListexModel> _listexRepository;

        public SqlService(IListexRepository listexRepository)
        {
            _listexRepository = listexRepository as RepositoryBase<ListexModel>;
        }

        public BasicResponse<int> ExecuteNonQueryBySql(SqlRequest req)
        {
            var ret =_listexRepository.ExecuteNonQueryBySql(req.Sql);
            return new BasicResponse<int>()
            {
                Data = ret
            };
        }

        public BasicResponse<DataTable> QueryTableBySql(SqlRequest req)
        {
            var dt = _listexRepository.QueryTableBySql(req.Sql);
            var ret = new BasicResponse<DataTable>()
            {
                Data = dt
            };
            return ret;
        }
    }
}
