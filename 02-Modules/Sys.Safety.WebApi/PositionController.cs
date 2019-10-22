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

namespace Sys.Safety.WebApi
{
    public class PositionController : Basic.Framework.Web.WebApi.BasicApiController,IPositionService
    {
        static PositionController()
        {

        }
        IPositionService _PositionService = ServiceFactory.Create<IPositionService>();
        /// <summary>
        ///添加安装位置
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/Add")]
       public BasicResponse AddPosition(PositionAddRequest PositionRequest)
        {
            return _PositionService.AddPosition(PositionRequest);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/AddPositions")]
        public BasicResponse AddPositions(PositionsRequest PositionRequest)
        {
            return _PositionService.AddPositions(PositionRequest);
        }
        /// <summary>
        /// 安装位置更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/Update")]
        public BasicResponse UpdatePosition(PositionUpdateRequest PositionRequest)
        {
            return _PositionService.UpdatePosition(PositionRequest);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="PositionRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/UpdatePositions")]
        public BasicResponse UpdatePositions(PositionsRequest PositionRequest)
        {
            return _PositionService.UpdatePositions(PositionRequest);
        }
        [HttpPost]
        [Route("v1/Position/Delete")]
        public BasicResponse DeletePosition(PositionDeleteRequest PositionRequest)
        {
            return _PositionService.DeletePosition(PositionRequest);
        }

        [HttpPost]
        [Route("v1/Position/Get")]
        public BasicResponse<Jc_WzInfo> GetPositionById(PositionGetRequest PositionRequest)
        {
            return _PositionService.GetPositionById(PositionRequest);
            //4696858824537223757
        }
        [HttpPost]
        [Route("v1/Position/GetPageList")]
        public BasicResponse<List<Jc_WzInfo>> GetPositionList(PositionGetListRequest PositionRequest)
        {
            return _PositionService.GetPositionList(PositionRequest);
        }
        [HttpPost]
        [Route("v1/Position/GetList")]
        public BasicResponse<List<Jc_WzInfo>> GetPositionList()
        {
            return _PositionService.GetPositionList();
        }
        /// <summary>
        /// 获取所有安装位置缓存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/GetAllPositionCache")]
        public BasicResponse<List<Jc_WzInfo>> GetAllPositionCache()
        {
            return _PositionService.GetAllPositionCache();
        }
        
        /// <summary>
        /// 获取缓存中当前最大的位置ID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Position/GetMaxPositionId")]
        public BasicResponse<long> GetMaxPositionId()
        {
            return _PositionService.GetMaxPositionId();
        }

        [HttpPost]
        [Route("v1/Position/AddPositionBySql")]
        public BasicResponse<string> AddPositionBySql(AddPositionBySqlRequest request)
        {
            return _PositionService.AddPositionBySql(request);
        }

        [HttpPost]
        [Route("v1/Position/GetPositionCacheByWzID")]
        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWzID(PositionGetByWzIDRequest PositionRequest)
        {
            return _PositionService.GetPositionCacheByWzID(PositionRequest);
        }
        [HttpPost]
        [Route("v1/Position/GetPositionCacheByWz")]
        public BasicResponse<List<Jc_WzInfo>> GetPositionCacheByWz(PositionGetByWzRequest PositionRequest)
        {
            return _PositionService.GetPositionCacheByWz(PositionRequest);
        }
    }
}
