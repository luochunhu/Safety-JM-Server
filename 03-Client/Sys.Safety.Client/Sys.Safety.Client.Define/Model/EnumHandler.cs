using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Define.Model
{
    public class EnumHandler
    {
        private static readonly IEnumcodeService EnumcodeService = ServiceFactory.Create<IEnumcodeService>();

        /// <summary>
        /// 获枚举
        /// </summary>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetEnum(long enumTypeId)
        {
            var req = new EnumcodeGetByEnumTypeIDRequest
            {
                EnumTypeId = enumTypeId.ToString()
            };
            var res = EnumcodeService.GetEnumcodeByEnumTypeID(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var data = res.Data.OrderBy(a => a.LngRowIndex);
            return data.ToList();
        }
    }
}
