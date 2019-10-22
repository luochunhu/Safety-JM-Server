using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Enums.Constant;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
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

        /// <summary>获取触发数据类型
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetTriggerDataState()
        {
            //var triggerVlaue = new List<int>()
            //{
            //    8,10,12,14,16,18,20,22,23,25,26,27
            //};
            var triggerVlaue = LinkageConstant.TriggerDataStateVlaue;

            var allState = GetEnum(4);
            var ret = allState.Where(a => triggerVlaue.Contains(a.LngEnumValue)).ToList();
            return ret;
        }
    }
}
