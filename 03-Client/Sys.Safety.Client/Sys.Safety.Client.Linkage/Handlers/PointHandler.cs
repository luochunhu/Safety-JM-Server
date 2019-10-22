using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class PointHandler
    {
        private static readonly IPointDefineService PointDefineService = ServiceFactory.Create<IPointDefineService>();

        private static readonly IPersonPointDefineService PersonPointDefineService = ServiceFactory.Create<IPersonPointDefineService>();

        private static IV_DefService _videoDefineService = ServiceFactory.Create<IV_DefService>();


        /// <summary>获取监控系统测点
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetMonitoringSystemAllPoint()
        {
            var res = PointDefineService.GetAllPointDefineCache();
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据性质获取监控系统测点
        /// 
        /// </summary>
        /// <param name="propertyIds"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetMonitoringSystemPointByPropertyIds(List<int> propertyIds)
        {
            var allPoint = GetMonitoringSystemAllPoint();
            var point = allPoint.Where(a => propertyIds.Contains(a.DevPropertyID)).OrderBy(a => a.Point).ToList();
            return point;
        }
    }
}
