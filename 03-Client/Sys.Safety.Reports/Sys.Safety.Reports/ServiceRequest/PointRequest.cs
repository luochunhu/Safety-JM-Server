using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Reports.ServiceRequest
{
    public static class PointRequest
    {
        private static readonly IPointDefineService PointDefineService = ServiceFactory.Create<IPointDefineService>();

        public static List<Jc_DefInfo> GetAllSubstationPoint()
        {
            var req = new PointDefineGetByDevpropertIDRequest
            {
                DevpropertID = 0
            };
            var res = PointDefineService.GetPointDefineCacheByDevpropertID(req);
            return res.Data;
        }

        public static List<Jc_DefInfo> GetPointBySubstationNum(string substationNum)
        {
            var req = new PointDefineGetByStationIDRequest
            {
                StationID = Convert.ToInt32(substationNum)
            };
            var res = PointDefineService.GetPointDefineCacheByStationID(req);
            return res.Data;
        }

        public static List<Jc_DefInfo> GetPointByEquipmentPropertyId(string propertyId)
        {
            var req = new PointDefineGetByDevpropertIDRequest
            {
                DevpropertID = Convert.ToInt32(propertyId)
            };
            var res = PointDefineService.GetPointDefineCacheByDevpropertID(req);
            return res.Data;
        }

        public static List<Jc_DefInfo> GetPointByEquipmentCategoryId(string categoryId)
        {
            var req = new PointDefineGetByDevClassIDRequest
            {
                DevClassID = Convert.ToInt32(categoryId)
            };
            var res = PointDefineService.GetPointDefineCacheByDevClassID(req);
            return res.Data;
        }

    }
}
