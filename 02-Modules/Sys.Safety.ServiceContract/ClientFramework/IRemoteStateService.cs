using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.RemoteState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract
{
    /// <summary>
    /// 获取远程机状态方法  20170523
    /// </summary>
    public interface IRemoteStateService
    {
        /// <summary>
        /// 获取网关状态
        /// </summary>
        /// <returns></returns>
        BasicResponse<bool> GetGatewayState();
        /// <summary>
        /// 设置网关的状态
        /// </summary>        
        BasicResponse SetGatewayState(RemoteStateRequest staterequest);
        /// <summary>
        /// 获取远程机状态
        /// </summary>
        /// <returns></returns>
        BasicResponse<bool> GetRemoteState();
        /// <summary>
        /// 设置远程机的状态
        /// </summary>        
        BasicResponse SetRemoteState(RemoteStateRequest staterequest);
        /// <summary>
        /// 获取最后接收数据时间
        /// </summary>
        /// <returns></returns>
        BasicResponse<DateTime> GetLastReciveTime();
         /// <summary>
        /// 设置最后接收数据时间
        /// </summary>
        /// <param name="staterequest"></param>
        /// <returns></returns>
        BasicResponse SetLastReciveTime(RemoteStateRequest staterequest);

        BasicResponse UpdateInspectionTime(UpdateInspectionTimeRequest request);


        BasicResponse<long> GetInspectionTime(GetInspectionTimeRequest request);
    }
}
