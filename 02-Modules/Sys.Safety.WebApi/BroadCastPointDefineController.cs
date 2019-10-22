using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class BroadCastPointDefineController : Basic.Framework.Web.WebApi.BasicApiController, IBroadCastPointDefineService
    {
        IBroadCastPointDefineService _PointDefineService = ServiceFactory.Create<IBroadCastPointDefineService>();

        [HttpPost]
        [Route("v1/BroadCastPointDefine/AddPointDefine")]
        public Basic.Framework.Web.BasicResponse AddPointDefine(Sys.Safety.Request.PointDefine.PointDefineAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefine(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/AddPointDefines")]
        public Basic.Framework.Web.BasicResponse AddPointDefines(Sys.Safety.Request.PointDefine.PointDefinesAddRequest PointDefineRequest)
        {
            return _PointDefineService.AddPointDefines(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/UpdatePointDefine")]
        public Basic.Framework.Web.BasicResponse UpdatePointDefine(Sys.Safety.Request.PointDefine.PointDefineUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefine(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/UpdatePointDefines")]
        public Basic.Framework.Web.BasicResponse UpdatePointDefines(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefines(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/UpdatePointDefinesCache")]
        public Basic.Framework.Web.BasicResponse UpdatePointDefinesCache(Sys.Safety.Request.PointDefine.PointDefinesUpdateRequest PointDefineRequest)
        {
            return _PointDefineService.UpdatePointDefinesCache(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/BatchUpdatePointDefineInfo")]
        public Basic.Framework.Web.BasicResponse BatchUpdatePointDefineInfo(Sys.Safety.Request.Cache.DefineCacheBatchUpdatePropertiesRequest request)
        {
            return _PointDefineService.BatchUpdatePointDefineInfo(request);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/DeletePointDefine")]
        public Basic.Framework.Web.BasicResponse DeletePointDefine(Sys.Safety.Request.PointDefine.PointDefineDeleteRequest PointDefineRequest)
        {
            return _PointDefineService.DeletePointDefine(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/GetAllPointDefineCache")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefInfo>> GetAllPointDefineCache()
        {
            return _PointDefineService.GetAllPointDefineCache();
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/PointDefineSaveData")]
        public Basic.Framework.Web.BasicResponse PointDefineSaveData()
        {
            return _PointDefineService.PointDefineSaveData();
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/GetPointDefineCacheByAreaID")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefInfo>> GetPointDefineCacheByAreaID(Sys.Safety.Request.PointDefine.PointDefineGetByAreaIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByAreaID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/GetPointDefineCacheByPointID")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_DefInfo> GetPointDefineCacheByPointID(Sys.Safety.Request.PointDefine.PointDefineGetByPointIDRequest PointDefineRequest)
        {
            return _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest);
        }

        [HttpPost]
        [Route("v1/BroadCastPointDefine/SynchronousPoint")]
        public Basic.Framework.Web.BasicResponse<bool> SynchronousPoint(Sys.Safety.Request.PointDefine.SynchronousPointRequest PointDefineRequest)
        {
            return _PointDefineService.SynchronousPoint(PointDefineRequest);
        }
       

        [HttpPost]
        [Route("v1/BroadCastPointDefine/BroadcastSysPointSync")]
        public Basic.Framework.Web.BasicResponse BroadcastSysPointSync(Sys.Safety.Request.PointDefine.BroadcastSysPointSyncRequest request)
        {
            return _PointDefineService.BroadcastSysPointSync(request);
        }
    }
}
