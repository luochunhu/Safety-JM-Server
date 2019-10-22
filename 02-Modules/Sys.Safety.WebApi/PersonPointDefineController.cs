using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Service;
using System.Web.Http;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.WebApi
{
    public class PersonPointDefineController : Basic.Framework.Web.WebApi.BasicApiController, IPersonPointDefineService
    {
        static PersonPointDefineController()
        {

        }
        IPersonPointDefineService _PointDefineService = ServiceFactory.Create<IPersonPointDefineService>();   
        /// <summary>
        ///  添加测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/Add")]
        public BasicResponse AddPointDefine(PointDefineAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefine(PointDefineRequest);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/AddPointDefines")]
        public BasicResponse AddPointDefines(PointDefinesAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefines(PointDefineRequest);
        }
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/Update")]
        public BasicResponse UpdatePointDefine(PointDefineUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefine(PointDefineRequest);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/UpdatePointDefines")]
        public BasicResponse UpdatePointDefines(PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefines(PointDefineRequest);
        }
        /// <summary>
        /// 批量更新缓存
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/UpdatePointDefinesCache")]
        public BasicResponse UpdatePointDefinesCache(PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefinesCache(PointDefineRequest);
        }
       /// <summary>
        /// 同时添加、更新定义及网络模拟绑定信息
       /// </summary>
       /// <param name="PointDefineAddNetworkModuleRequest"></param>
       /// <returns></returns>
        [HttpPost]
        [Route("v1/PersonPointDefine/AddUpdatePointDefineAndNetworkModuleCache")]
        public BasicResponse AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest)
        {
            return _PointDefineService.AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleRequest);
        }
        [HttpPost]
        [Route("v1/PersonPointDefine/Delete")]
        public BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest)
        {
            return _PointDefineService.DeletePointDefine(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/BatchUpdatePointDefineInfo")]
        public BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request)
        {
            return _PointDefineService.BatchUpdatePointDefineInfo(request);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/GetAllPointDefineCache")]
        public BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache()
        {
            return _PointDefineService.GetAllPointDefineCache();
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/PointDefineSaveData")]
        public BasicResponse PointDefineSaveData()
        {
            return _PointDefineService.PointDefineSaveData();
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/GetPointDefineCacheByDevpropertID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByDevpropertID(PointDefineGetByDevpropertIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/GetPointDefineCacheByAreaID")]
        public BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaID(PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/GetPointDefineCacheByPointID")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/OldPlsPointSync")]
        public BasicResponse OldPlsPointSync(OldPlsPointSyncRequest request)
        {
            return _PointDefineService.OldPlsPointSync(request);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/PlsPointSync")]
        public BasicResponse PlsPointSync(PlsPointSyncRequest request)
        {
            return _PointDefineService.PlsPointSync(request);
        }

        [HttpPost]
        [Route("v1/PersonPointDefine/GetPointDefineCacheByPoint")]
        public BasicResponse<Jc_DefInfo> GetPointDefineCacheByPoint(PointDefineGetByPointRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPoint(PointDefineRequest);
        }
    }
}
