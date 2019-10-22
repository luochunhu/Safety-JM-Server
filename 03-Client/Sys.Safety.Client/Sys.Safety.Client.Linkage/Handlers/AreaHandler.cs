using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Area;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class AreaHandler
    {
        private static readonly IAreaService DeviceDefineService = ServiceFactory.Create<IAreaService>();

        /// <summary>
        /// 所有活动区域
        /// </summary>
        /// <returns></returns>
        public static List<AreaInfo> GetAllArea()
        {
            var req = new AreaGetListRequest();
            var res = DeviceDefineService.GetAllAreaList(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = res.Data.Where(a => a.Activity == "1").OrderBy(a => a.Areaname).ToList();
            return ret;
        }
    }
}
