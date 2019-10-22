using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class EquipmentTypeHandler
    {
        private static readonly IDeviceDefineService DeviceDefineService = ServiceFactory.Create<IDeviceDefineService>();

        public static List<Jc_DevInfo> GetAllEquipmentType()
        {
            var res = DeviceDefineService.GetAllDeviceDefineCache();
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        public static List<Jc_DevInfo> GetEquipmentTypeByPropertyIds(List<int> properytIds)
        {
            var allEquipmentType = GetAllEquipmentType();
            var res = allEquipmentType.Where(a => properytIds.Contains(a.Type)).OrderBy(a=>a.Name).ToList();
            return res;
        }
    }
}
