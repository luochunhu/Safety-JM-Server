using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public class LinkageHandler
    {
        private static readonly ISysEmergencyLinkageService SysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();

        /// <summary>新增应急联动
        /// 
        /// </summary>
        public static void AddEmergencyLinkage(AddEmergencylinkageconfigMasterInfoPassiveInfoRequest request)
        {
            var res = SysEmergencyLinkageService.AddEmergencylinkageconfigMasterInfoPassiveInfo(request);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>获取应急联动定义及统计信息
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<GetSysEmergencyLinkageListAndStatisticsResponse> GetSysEmergencyLinkageListAndStatistics(string name)
        {
            var req = new StringRequest()
            {
                Str = name
            };
            var res = SysEmergencyLinkageService.GetSysEmergencyLinkageListAndStatistics(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
        
        /// <summary>根据关联Id获取主控测点列表
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetMasterPointInfoByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetMasterPointInfoByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取主控区域列表
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<AreaInfo> GetMasterAreaInfoByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetMasterAreaInfoByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取主控设备类型列表
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<Jc_DevInfo> GetMasterEquTypeInfoByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetMasterEquTypeInfoByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取触发数据状态列表
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetMasterTriDataStateByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetMasterTriDataStateByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>获取应急联动所有被控测点
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<IdTextCheck> GetLinkagePassivePoint()
        {
            var res = SysEmergencyLinkageService.GetAllPassivePointInfo();
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取被控人员列表
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<R_PersoninfInfo> GetPassivePersonByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetPassivePersonByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取被控测点列表
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<IdTextCheck> GetPassivePointInfoByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetPassivePointInfoByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>根据关联Id获取被控区域列表
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<AreaInfo> GetPassiveAreaInfoByAssId(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.GetPassiveAreaInfoByAssId(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        public static void SoftDeleteSysEmergencyLinkage(string id)
        {
            var req = new LongIdRequest()
            {
                Id = Convert.ToInt64(id)
            };
            var res = SysEmergencyLinkageService.SoftDeleteSysEmergencyLinkageById(req);
            if (res.Code != 100)
            {
                throw new Exception(res.Message);
            }
        }
    }
}
