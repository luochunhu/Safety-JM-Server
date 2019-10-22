using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class PersonHandler
    {
        private static readonly IR_PersoninfService PersoninfService = ServiceFactory.Create<IR_PersoninfService>();

        public static List<R_PersoninfInfo> GetAllPerson()
        {
            var req = new BasicRequest();
            var res = PersoninfService.GetAllPersonInfoCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var ret = res.Data.OrderBy(a => a.Name).ToList();
            return ret;
        }
    }
}
