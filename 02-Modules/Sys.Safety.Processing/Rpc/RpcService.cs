using Basic.Framework.Logging;
using Basic.Framework.Rpc;
using Sys.DataCollection.Common.Protocols;
using Sys.DataCollection.Common.Rpc;
using Sys.Safety.Processing.QueryCacheData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.Rpc
{
    public delegate void DeviceMessageArrivedEventHandler(MasProtocol masProtocol);
    public class RpcService
    {
        static IRpcClient _client;
        static IRpcServer _server;

        static string _remoteIp = "127.0.0.1";//远程RPC服务器IP
        static int _remotePort = 10003;//远程RPC服务器端口号

        static string _localIp = "127.0.0.1";//自己做为RPC服务器的IP
        static int _localPort = 10002;//自己做为RPC服务器的端口号

        static RpcModel _rpcModel = RpcModel.WebApiModel;

        public static event DeviceMessageArrivedEventHandler OnDeviceMessageArrived;

        /// <summary>
        /// 获取默认RPC通讯方式
        /// </summary>
        /// <returns></returns>
        //private static RpcModel GetRpcModel()
        //{
        //    return RpcModel.gRPCModel;
        //}

        /// <summary>
        /// 收到RPC消息处理
        /// </summary>
        /// <param name="rpcRequest"></param>
        /// <returns></returns>
        private static RpcResponse OnRpcMessageArrived(RpcRequest rpcRequest)
        {
            if (rpcRequest.RequestType == (int)RequestType.DeviceRequest)
            {
                //设备类请求，直接交给驱动处理
                return HandleDeviceRequest(rpcRequest);
            }
            else if (rpcRequest.RequestType == (int)RequestType.BusinessRequest)
            {
                //业务类请求，网关直接处理 如心跳、获取业务数据或者交互处理 等
                return HandleBusinessRequest(rpcRequest);
            }

            return null;
        }

        /// <summary>
        /// 设备类请求处理
        /// </summary>
        /// <param name="rpcRequest"></param>
        /// <returns></returns>
        private static RpcResponse HandleDeviceRequest(RpcRequest rpcRequest)
        {
            var masProtocol = rpcRequest.ToRequest<GatewayRpcRequest>().MasProtocol;
            //todo 
            if (OnDeviceMessageArrived != null)
            {
                OnDeviceMessageArrived(masProtocol);
            }

            return RpcResponse.Response<GatewayRpcResponse>(new GatewayRpcResponse());
        }

        /// <summary>
        /// 业务类请求处理
        /// </summary>
        /// <param name="rpcRequest"></param>
        /// <returns></returns>
        private static RpcResponse HandleBusinessRequest(RpcRequest rpcRequest)
        {
            var masProtocol = rpcRequest.ToRequest<GatewayRpcRequest>().MasProtocol;
            //todo 处理业务
            if (masProtocol.ProtocolType == ProtocolType.QueryCacheDataRequest) //网关请求获取缓存信息
            {               
                QueryCacheDataResponse queryCacheDataResponse = QueryCacheDataToGateway.QueryServiceCacheDataToGateway();
                return RpcResponse.Response<QueryCacheDataResponse>(queryCacheDataResponse);
            }

            return RpcResponse.Response<GatewayRpcResponse>(new GatewayRpcResponse());
        }


        /// <summary>
        /// 启动RPC服务器
        /// </summary>
        /// <returns></returns>
        public static bool StartRpcServer(string remoteIp, int remotePort, string localIp, int localPort)
        {
            _remoteIp = remoteIp;
            _remotePort = remotePort;
            _localIp = localIp;
            _localPort = localPort;

            System.Reflection.Assembly.Load("Sys.Safety.WebApi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            int rpcModel = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetInt("RpcModel", 1);
            if (rpcModel == 1)
            {
                _rpcModel = RpcModel.WebApiModel;
            }
            else if (rpcModel == 2)
            {
                _rpcModel = RpcModel.gRPCModel;
            }

            _client = RpcFactory.CreateRpcClient(_rpcModel, _remoteIp, _remotePort);
            _server = RpcFactory.CreateRpcServer(_rpcModel);
            _server.RegistCallback(OnRpcMessageArrived);

            _server.Start(_localIp, _localPort);
            return true;
        }

        /// <summary>
        /// 停止RPC服务器
        /// </summary>
        /// <returns></returns>
        public static bool StopRpcServer()
        {
            if (_server != null)
            {
                _server.Stop();
            }
            LogHelper.Info("停止数据处理模块【停止RPC服务处理完成】！");
            return true;
        }

        /// <summary>
        /// 发送数据到远程服务器
        /// </summary>
        /// <typeparam name="TResult">返回的结果类型</typeparam>
        /// <param name="masProtocol">待发送的协议</param>
        /// <param name="requestType">请求的类型</param>
        /// <returns>调用结果</returns>
        public static TResult Send<TResult>(MasProtocol masProtocol, RequestType requestType)
        {
            GatewayRpcRequest request = new GatewayRpcRequest(requestType);
            request.MasProtocol = masProtocol;
            var response = _client.Send<GatewayRpcRequest, TResult>(request);
            if (response.IsSuccess)
            {
                //todo
            }
            return response.Data;
        }


    }
}
