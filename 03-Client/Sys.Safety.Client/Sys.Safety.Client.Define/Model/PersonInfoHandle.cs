using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Web;

namespace Sys.Safety.Client.Define.Model
{
    public static class PersonInfoHandle
    {
        private static readonly IR_PersoninfService PersoninfService = ServiceFactory.Create<IR_PersoninfService>();
        
        /// <summary>
        /// 获取所有人员
        /// </summary>
        /// <returns></returns>
        public static List<R_PersoninfInfo> GetAllRPersoninfCache()
        {
            var req = new BasicRequest();
            var res = PersoninfService.GetAllDefinedPersonInfoCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
    }
}
