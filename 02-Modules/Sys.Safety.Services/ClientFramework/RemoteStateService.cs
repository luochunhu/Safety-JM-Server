using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 获取远程机状态方法  20170523
    /// </summary>
    public partial class RemoteStateService : IRemoteStateService
    {
        /// <summary>
        /// 网关通讯状态标识
        /// </summary>
        private bool GatewayState = false;
        /// <summary>
        /// 远程机状态
        /// </summary>
        private bool RemoteState;
        /// <summary>
        /// 最后接收数据时间
        /// </summary>
        private DateTime LastReciveTime;

        /// <summary>
        /// 获取网关状态
        /// </summary>
        /// <returns></returns>
        public BasicResponse<bool> GetGatewayState()
        {
            BasicResponse<bool> Result = new BasicResponse<bool>();
            bool rvalue = false;
            try
            {
                rvalue = GatewayState;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                rvalue = false;
            }
            Result.Data = rvalue;
            return Result;
        }
        /// <summary>
        /// 设置网关的状态
        /// </summary>
        /// <param name="State"></param>
        public BasicResponse SetGatewayState(RemoteStateRequest staterequest)
        {
            BasicResponse Result = new BasicResponse();
            GatewayState = staterequest.State;
            return Result;
        }
        /// <summary>
        /// 获取远程机状态
        /// </summary>
        /// <returns></returns>
        public BasicResponse<bool> GetRemoteState()
        {
            BasicResponse<bool> Result = new BasicResponse<bool>();
            bool rvalue = false;
            try
            {
                rvalue = RemoteState;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                rvalue = false;
            }

            Result.Data = rvalue;
            return Result;
        }
        /// <summary>
        /// 设置远程机的状态
        /// </summary>
        /// <param name="State"></param>
        public BasicResponse SetRemoteState(RemoteStateRequest staterequest)
        {
            BasicResponse Result = new BasicResponse();
            RemoteState = staterequest.State;
            return Result;
        }
        /// <summary>
        /// 获取最后接收数据时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DateTime> GetLastReciveTime()
        {
            BasicResponse<DateTime> Result = new BasicResponse<DateTime>();           
            Result.Data = LastReciveTime;
            return Result;
        }
        /// <summary>
        /// 设置最后接收数据时间
        /// </summary>
        /// <param name="staterequest"></param>
        /// <returns></returns>
        public BasicResponse SetLastReciveTime(RemoteStateRequest staterequest)
        {
            BasicResponse Result = new BasicResponse();
            LastReciveTime = staterequest.LastReviceTime;
            return Result;
        }
        //----------------------------------------------------------------------系统巡检周期-----------------------------------
        /// <summary>
        /// 系统巡检周期（单位：毫秒）
        /// </summary>
        private  long inspectionTime = 0;


        public BasicResponse UpdateInspectionTime(UpdateInspectionTimeRequest request)
        {
            BasicResponse Result = new BasicResponse();
            inspectionTime = request.InspectionTime;
            return Result;
        }


        public BasicResponse<long> GetInspectionTime(GetInspectionTimeRequest request)
        {
            BasicResponse<long> Result = new BasicResponse<long>();
            Result.Data = inspectionTime;
            return Result;
        }
    }
}
