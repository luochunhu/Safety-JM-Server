using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.Cache;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IBroadCastPointDefineService
    {
        /// <summary>
        /// 添加测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefine(PointDefineAddRequest PointDefineRequest);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse AddPointDefines(PointDefinesAddRequest PointDefineRequest);
        /// <summary>
        /// 更新测点
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefine(PointDefineUpdateRequest PointDefineRequest);
        ///<summary>
        /// 批量更新
        /// </summary>
        /// <param name="jc_Defrequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefines(PointDefinesUpdateRequest PointDefineRequest);
        /// <summary>
        /// 批量更新测点定义缓存
        /// </summary>
        /// <param name="PointDefineRequest"></param>
        /// <returns></returns>
        BasicResponse UpdatePointDefinesCache(PointDefinesUpdateRequest PointDefineRequest);
        /// <summary>
        /// 批量更新属性
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse BatchUpdatePointDefineInfo(DefineCacheBatchUpdatePropertiesRequest request);        
        BasicResponse DeletePointDefine(PointDefineDeleteRequest PointDefineRequest);
        /// <summary>
        /// 获取所有定义缓存数据
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetAllPointDefineCache();
        /// <summary>
        /// 保存巡检
        /// </summary>
        /// <returns></returns>
        BasicResponse PointDefineSaveData();        

        BasicResponse<List<Jc_DefInfo>> GetPointDefineCacheByAreaID(PointDefineGetByAreaIDRequest PointDefineRequest);

        BasicResponse<Jc_DefInfo> GetPointDefineCacheByPointID(PointDefineGetByPointIDRequest PointDefineRequest);

        BasicResponse<bool> SynchronousPoint(SynchronousPointRequest PointDefineRequest);

        BasicResponse BroadcastSysPointSync(BroadcastSysPointSyncRequest request);
    } 
}

