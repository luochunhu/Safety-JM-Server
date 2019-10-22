using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Reports.ServiceRequest
{
    public static class EnumRequest
    {
        private static readonly IEnumcodeService EnumcodeService = ServiceFactory.Create<IEnumcodeService>();

        /// <summary>
        /// 获取设备性质
        /// </summary>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetEquipmentProperty()
        {
            var res = EnumcodeService.GetAllDevicePropertyCache();
            return res.Data;
        }

        /// <summary>
        /// 获取设备种类
        /// </summary>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetEquipmentCategory()
        {
            var res = EnumcodeService.GetAllDeviceClassCache();
            return res.Data;
        }
    }
}
