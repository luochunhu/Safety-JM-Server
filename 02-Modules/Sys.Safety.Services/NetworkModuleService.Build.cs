using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.NetworkModule;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Data;
using Sys.Safety.Processing.Rpc;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.DataCollection.Common.Rpc;

namespace Sys.Safety.Services
{
    /// <summary>
    /// 网络模块管理
    /// </summary>
    public partial class NetworkModuleService : INetworkModuleService
    {
        private INetworkModuleRepository _Repository;
        private INetworkModuleCacheService _NetworkModuleCacheService;

        public NetworkModuleService(INetworkModuleRepository _Repository, INetworkModuleCacheService _NetworkModuleCacheService)
        {
            this._Repository = _Repository;
            this._NetworkModuleCacheService = _NetworkModuleCacheService;
        }
        /// <summary>
        /// 添加网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse AddNetworkModule(NetworkModuleAddRequest NetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_MacInfo item = NetworkModuleRequest.NetworkModuleInfo;

            //重复判断
            NetworkModuleCacheGetByKeyRequest networkModuleCacheRequest = new NetworkModuleCacheGetByKeyRequest();
            networkModuleCacheRequest.Mac = item.MAC;
            var result = _NetworkModuleCacheService.GetNetworkModuleCacheByKey(networkModuleCacheRequest);
            Jc_MacInfo OldItem = result.Data;
            if (result.Data != null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前添加的网络模块已存在！";
                return Result;
            }

            //重置内存状态标记，因为搜索的时候，只保存了内存   20170410
            //if (OldItem != null)
            //{
            //    if (OldItem.IsMemoryData)//如果内存中存在数据，则重置添加、修改标记  20170415
            //    {
            //        if (OldItem.InfoState == InfoState.AddNew)
            //        {
            //            if (item.InfoState == InfoState.Modified)
            //            {
            //                item.InfoState = InfoState.AddNew;
            //            }
            //        }
            //        if (OldItem.InfoState == InfoState.Delete)
            //        {
            //            if (item.InfoState == InfoState.AddNew)
            //            {
            //                item.InfoState = InfoState.Modified;
            //            }
            //        }
            //        item.IsMemoryData = false;//置完标记后，将内存数据变成非内存数据标记  20170415
            //    }
            //} 
            item.IsMemoryData = false;//置完标记后，将内存数据变成非内存数据标记  20170415

            //向网关同步数据
            List<Jc_MacInfo> SendItemList = new List<Jc_MacInfo>();
            SendItemList.Add(item);
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            //保存数据库
            var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
            //保存数据库不存连接号  20170713                   
            _jc_Mac.NetID = 0;
            var resultjc_Mac = _Repository.AddNetworkModule(_jc_Mac);

            //更新缓存
            NetworkModuleCacheAddRequest AddNetworkModuleCacheRequest = new NetworkModuleCacheAddRequest();
            AddNetworkModuleCacheRequest.NetworkModuleInfo = item;
            _NetworkModuleCacheService.AddNetworkModuleCache(AddNetworkModuleCacheRequest);

            return Result;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse AddNetworkModules(NetworkModulesRequest NetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_MacInfo> items = NetworkModuleRequest.NetworkModulesInfo;
            List<Jc_MacInfo> NetworkModuleCaches = new List<Jc_MacInfo>();
            Jc_MacInfo olditem = null;

            NetworkModuleCacheGetAllRequest NetworkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var result = _NetworkModuleCacheService.GetAllNetworkModuleCache(NetworkModuleCacheRequest);
            NetworkModuleCaches = result.Data;

            foreach (Jc_MacInfo item in items)
            {
                olditem = NetworkModuleCaches.Find(a => a.MAC == item.MAC);
                //增加重复判断
                if (olditem != null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前添加列表中的数据在数据库中已经存在！";
                    return Result;
                }
            }

            //向网关同步数据
            List<Jc_MacInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }

            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_MacInfo item in items)
                {
                    item.IsMemoryData = false;
                    //保存数据库
                    var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
                    //保存数据库不存连接号  20170713                   
                    _jc_Mac.NetID = 0;
                    var resultjc_Def = _Repository.AddNetworkModule(_jc_Mac);
                }


                //添加到缓存
                NetworkModuleCacheBatchAddRequest BatchAddNetworkModuleCacheRequest = new NetworkModuleCacheBatchAddRequest();
                BatchAddNetworkModuleCacheRequest.NetworkModuleInfos = items;
                _NetworkModuleCacheService.BacthAddNetworkModuleCache(BatchAddNetworkModuleCacheRequest);

            });

            return Result;
        }
        /// <summary>
        /// 更新网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateNetworkModule(NetworkModuleUpdateRequest NetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            Jc_MacInfo item = NetworkModuleRequest.NetworkModuleInfo;

            //重复判断
            NetworkModuleCacheGetByKeyRequest networkModuleCacheRequest = new NetworkModuleCacheGetByKeyRequest();
            networkModuleCacheRequest.Mac = item.MAC;
            var result = _NetworkModuleCacheService.GetNetworkModuleCacheByKey(networkModuleCacheRequest);
            Jc_MacInfo OldItem = result.Data;
            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前更新的网络模块不存在！";
                return Result;
            }

            //重置内存状态标记，因为搜索的时候，只保存了内存   20170410
            if (OldItem != null)
            {
                if (OldItem.IsMemoryData)//如果内存中存在数据，则重置添加、修改标记  20170415
                {
                    if (OldItem.InfoState == InfoState.AddNew)
                    {
                        if (item.InfoState == InfoState.Modified)
                        {
                            item.InfoState = InfoState.AddNew;
                        }
                    }
                    if (OldItem.InfoState == InfoState.Delete)
                    {
                        if (item.InfoState == InfoState.AddNew)
                        {
                            item.InfoState = InfoState.Modified;
                        }
                    }
                    item.IsMemoryData = false;//置完标记后，将内存数据变成非内存数据标记  20170415
                }
            }

            //item.IsMemoryData = false;//置完标记后，将内存数据变成非内存数据标记  20170415

            if (item.InfoState == InfoState.AddNew)
            {
                //向网关同步数据
                List<Jc_MacInfo> SendItemList = new List<Jc_MacInfo>();
                SendItemList.Add(item);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    Result.Code = 1;
                    Result.Message = "向网关同步数据失败！";
                    return Result;
                }

                //保存数据库
                var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
                _Repository.AddNetworkModule(_jc_Mac);
            }
            else
            {
                //向网关同步数据
                List<Jc_MacInfo> SendItemList = new List<Jc_MacInfo>();
                SendItemList.Add(item);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    Result.Code = 1;
                    Result.Message = "向网关同步数据失败！";
                    return Result;
                }

                //保存数据库
                var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
                _Repository.UpdateNetworkModule(_jc_Mac);
            }

            //更新缓存
            UpdateNetworkModuleCacheByProperty(item);

            return Result;
        }
        /// <summary>
        /// 更新网络模块定义信息（只更新定义相关字段）
        /// </summary>
        /// <param name="item"></param>
        private void UpdateNetworkModuleCacheByProperty(Jc_MacInfo item)
        {
            Dictionary<string, object> paramater = new Dictionary<string, object>();
            paramater.Add("IP", item.IP);
            paramater.Add("Wzid", item.Wzid);
            paramater.Add("Wz", item.Wz);
            paramater.Add("Istmcs", item.Istmcs);
            paramater.Add("Type", item.Type);
            paramater.Add("Bz1", item.Bz1);
            paramater.Add("Bz2", item.Bz2);
            paramater.Add("Bz3", item.Bz3);
            paramater.Add("Bz4", item.Bz4);
            paramater.Add("Bz5", item.Bz5);
            paramater.Add("Bz6", item.Bz6);
            paramater.Add("Upflag", item.Upflag);

            paramater.Add("IsMemoryData", item.IsMemoryData);//是否内存数据标记

            //修改标记
            paramater.Add("InfoState", item.InfoState);

            NetworkModuleCacheUpdatePropertiesRequest networkModuleCacheUpdatePropertiesRequest = new NetworkModuleCacheUpdatePropertiesRequest();
            networkModuleCacheUpdatePropertiesRequest.Mac = item.MAC;
            networkModuleCacheUpdatePropertiesRequest.UpdateItems = paramater;
            _NetworkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateNetworkModules(NetworkModulesRequest NetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();
            List<Jc_MacInfo> items = NetworkModuleRequest.NetworkModulesInfo;
            List<Jc_MacInfo> NetworkModuleCaches = new List<Jc_MacInfo>();
            Jc_MacInfo olditem = null;

            NetworkModuleCacheGetAllRequest NetworkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var result = _NetworkModuleCacheService.GetAllNetworkModuleCache(NetworkModuleCacheRequest);
            NetworkModuleCaches = result.Data;

            foreach (Jc_MacInfo item in items)
            {
                olditem = NetworkModuleCaches.Find(a => a.MAC == item.MAC);
                //增加重复判断
                if (olditem == null)
                { //缓存中存在此测点
                    Result.Code = 1;
                    Result.Message = "当前更新列表中的数据在数据库中不存在！";
                    return Result;
                }
                else
                {
                    if (olditem.IsMemoryData)//如果内存中存在数据，则重置添加、修改标记  20170415
                    {
                        if (olditem.InfoState == InfoState.AddNew)
                        {
                            if (item.InfoState == InfoState.Modified)
                            {
                                item.InfoState = InfoState.AddNew;
                            }
                        }
                        if (olditem.InfoState == InfoState.Delete)
                        {
                            if (item.InfoState == InfoState.AddNew)
                            {
                                item.InfoState = InfoState.Modified;
                            }
                        }
                        item.IsMemoryData = false;//置完标记后，将内存数据变成非内存数据标记  20170415
                    }
                }
            }
            //向网关同步数据
            List<Jc_MacInfo> SendItemList = items;
            var resultSync = SynchronousDataToGateway(SendItemList);
            if (!resultSync)
            {
                Result.Code = 1;
                Result.Message = "向网关同步数据失败！";
                return Result;
            }
            TransactionsManager.BeginTransaction(() =>
            {
                foreach (Jc_MacInfo item in items)
                {
                    if (item.InfoState == InfoState.AddNew)
                    {
                        //保存数据库
                        var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
                        _Repository.AddNetworkModule(_jc_Mac);
                    }
                    else
                    {
                        //保存数据库
                        var _jc_Mac = ObjectConverter.Copy<Jc_MacInfo, Jc_MacModel>(item);
                        _Repository.UpdateNetworkModule(_jc_Mac);
                    }
                    //更新到缓存
                    UpdateNetworkModuleCacheByProperty(item);
                }
            });

            return Result;
        }
        /// <summary>
        /// 删除网络模块
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteNetworkModule(NetworkModuleDeleteByMacRequest NetworkModuleRequest)
        {
            BasicResponse Result = new BasicResponse();

            //重复判断
            NetworkModuleCacheGetByKeyRequest networkModuleCacheRequest = new NetworkModuleCacheGetByKeyRequest();
            networkModuleCacheRequest.Mac = NetworkModuleRequest.Mac;
            var result = _NetworkModuleCacheService.GetNetworkModuleCacheByKey(networkModuleCacheRequest);
            Jc_MacInfo OldItem = result.Data;
            if (result.Data == null)
            {
                //缓存中存在此测点                
                Result.Code = 1;
                Result.Message = "当前删除的网络模块不存在！";
                return Result;
            }

            OldItem.InfoState = InfoState.Delete;

            //保存数据库 
            if (!OldItem.IsMemoryData)
            {
                //向网关同步数据                
                List<Jc_MacInfo> SendItemList = new List<Jc_MacInfo>();
                SendItemList.Add(OldItem);
                var resultSync = SynchronousDataToGateway(SendItemList);
                if (!resultSync)
                {
                    Result.Code = 1;
                    Result.Message = "向网关同步数据失败！";
                    return Result;
                }

                _Repository.DeleteNetworkModule(OldItem.ID);
            }

            //更新缓存
            NetworkModuleCacheDeleteRequest DeleteNetworkModuleCacheRequest = new NetworkModuleCacheDeleteRequest();
            DeleteNetworkModuleCacheRequest.NetworkModuleInfo = OldItem;
            _NetworkModuleCacheService.DeleteNetworkModuleCache(DeleteNetworkModuleCacheRequest);

            return Result;
        }
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList(NetworkModuleGetListRequest NetworkModuleRequest)
        {
            var jc_Macresponse = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleRequest.PagerInfo.PageIndex = NetworkModuleRequest.PagerInfo.PageIndex - 1;
            if (NetworkModuleRequest.PagerInfo.PageIndex < 0)
            {
                NetworkModuleRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_MacModelLists = _Repository.GetNetworkModuleList(NetworkModuleRequest.PagerInfo.PageIndex, NetworkModuleRequest.PagerInfo.PageSize, out rowcount);
            var jc_MacInfoLists = new List<Jc_MacInfo>();
            foreach (var item in jc_MacModelLists)
            {
                var Jc_MacInfo = ObjectConverter.Copy<Jc_MacModel, Jc_MacInfo>(item);
                jc_MacInfoLists.Add(Jc_MacInfo);
            }
            jc_Macresponse.Data = jc_MacInfoLists;
            return jc_Macresponse;
        }
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleList()
        {
            var jc_Macresponse = new BasicResponse<List<Jc_MacInfo>>();
            var jc_MacModelLists = _Repository.GetNetworkModuleList();
            var jc_MacInfoLists = new List<Jc_MacInfo>();
            foreach (var item in jc_MacModelLists)
            {
                var Jc_MacInfo = ObjectConverter.Copy<Jc_MacModel, Jc_MacInfo>(item);
                jc_MacInfoLists.Add(Jc_MacInfo);
            }
            jc_Macresponse.Data = jc_MacInfoLists;
            return jc_Macresponse;
        }
        /// <summary>
        /// 获取所有交换机的安装位置
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<string>> GetSwitchsPosition()
        {
            BasicResponse<List<string>> Result = new BasicResponse<List<string>>();
            List<string> SwitchsPosition = new List<string>();

            NetworkModuleCacheGetAllRequest networkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var jc_MacModelLists = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheRequest);
            foreach (Jc_MacInfo item in jc_MacModelLists.Data)
            {
                if (!SwitchsPosition.Contains(item.Wz) && long.Parse(item.Wzid) >= 0)
                {
                    SwitchsPosition.Add(item.Wz);
                }
            }
            Result.Data = SwitchsPosition;
            return Result;
        }
        public BasicResponse<Jc_MacInfo> GetNetworkModuleById(NetworkModuleGetRequest NetworkModuleRequest)
        {
            var result = _Repository.GetNetworkModuleById(NetworkModuleRequest.Id);
            var jc_MacInfo = ObjectConverter.Copy<Jc_MacModel, Jc_MacInfo>(result);
            var jc_Macresponse = new BasicResponse<Jc_MacInfo>();
            jc_Macresponse.Data = jc_MacInfo;
            return jc_Macresponse;
        }
        /// <summary>
        /// 获取所有网格模块缓存
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetAllNetworkModuleCache()
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleCacheGetAllRequest networkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var result = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 获取所有交换机列表
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetAllSwitchsCache()
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest = new NetworkModuleCacheGetByConditonRequest();
            networkModuleCacheRequest.Predicate=a=>a.Upflag=="1";
            var result = _NetworkModuleCacheService.GetNetworkModuleCache(networkModuleCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 根据安装位置获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByWz(NetworkModuleGetByWzRequest NetworkModuleRequest)
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest = new NetworkModuleCacheGetByConditonRequest();
            networkModuleCacheRequest.Predicate = a => a.Wz == NetworkModuleRequest.Wz && a.Type == 0;
            var result = _NetworkModuleCacheService.GetNetworkModuleCache(networkModuleCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过交换机的mac获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheBySwitchesMac(NetworkModuleGetBySwitchesMacRequest NetworkModuleRequest)
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest = new NetworkModuleCacheGetByConditonRequest();
            networkModuleCacheRequest.Predicate = a => a.Bz2 == NetworkModuleRequest.SwitchesMac && a.Type == 0;
            var result = _NetworkModuleCacheService.GetNetworkModuleCache(networkModuleCacheRequest);
            Result.Data = result.Data;
            return Result;
        }
        /// <summary>
        /// 通过Mac获取网络模块缓存
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> GetNetworkModuleCacheByMac(NetworkModuleGetByMacRequest NetworkModuleRequest)
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();
            NetworkModuleCacheGetByConditonRequest networkModuleCacheRequest = new NetworkModuleCacheGetByConditonRequest();
            networkModuleCacheRequest.Predicate = a => a.MAC == NetworkModuleRequest.Mac && a.Type == 0;
            var result = _NetworkModuleCacheService.GetNetworkModuleCache(networkModuleCacheRequest);
            Result.Data = result.Data;
            return Result;
        }

        /// <summary>
        /// 搜索网络模块，并更新缓存信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_MacInfo>> SearchALLNetworkModuleAndAddCache(SearchNetworkModuleRequest request)
        {
            BasicResponse<List<Jc_MacInfo>> Result = new BasicResponse<List<Jc_MacInfo>>();

            //查找现有网络模块
            NetworkModuleCacheGetAllRequest networkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var resultAllNetwork = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheRequest);
            List<Jc_MacInfo> ALLNetworkModule = resultAllNetwork.Data;

            if (request.StationFind != 2)//=2表示全部搜索
            {
                #region 搜索交换机/分站
                Sys.DataCollection.Common.Protocols.SearchNetworkDeviceRequest searchNetworkDeviceRequest = new SearchNetworkDeviceRequest();
                MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SearchNetworkDeviceRequest);
                masProtocol.Protocol = searchNetworkDeviceRequest;
                masProtocol.StationFind = request.StationFind;

                //调用RPC发送搜索网络模块命令，并接收回传的参数
                var result = RpcService.Send<SearchNetworkDeviceResponse>(masProtocol, RequestType.DeviceUdpRequest);//todo RequestType类型增加3

                List<Sys.DataCollection.Common.Protocols.NetworkDeviceItem> NetworkDeviceItems = null;
                if (result != null)
                {
                    NetworkDeviceItems = result.NetworkDeviceItems;
                }
                if (NetworkDeviceItems == null)
                {
                    Result.Code = 1;
                    Result.Message = "未搜索到设备！";
                    return Result;
                }
                //NetworkDeviceItems = result.NetworkDeviceItems;
                foreach (Sys.DataCollection.Common.Protocols.NetworkDeviceItem Network in NetworkDeviceItems)
                {
                    if (ALLNetworkModule.FindAll(a => a.MAC == Network.Mac).Count == 0)//缓存不存在此对象，则添加到缓存中
                    {
                        Jc_MacInfo NetworkModule = new Jc_MacInfo();
                        NetworkModule.ID = IdHelper.CreateLongId().ToString();
                        NetworkModule.MAC = Network.Mac;//网络模块MAC
                        NetworkModule.IP = Network.Ip;//网络模块IP
                        NetworkModule.Type = 0;
                        NetworkModule.Istmcs = 0;
                        NetworkModule.Wzid = "-1";// 20170331
                        NetworkModule.Wz = null;
                        NetworkModule.Bz2 = Network.SwitchMac;//网络模块所属交换机MAC
                        NetworkModule.Bz3 = Network.AddressNumber.ToString();//网络模块在交换机中的地址编码

                        NetworkModule.SubMask = Network.SubMask;
                        NetworkModule.GatewayIp = Network.GatewayIp;

                        if (Network.DeviceType == 3)//表示分站带网络模块
                        {
                            NetworkModule.Bz5 = "1";
                            NetworkModule.Upflag = "0";
                            NetworkModule.BindStatinNumber = Network.StationAddress;
                        }
                        else
                        {
                            NetworkModule.Bz5 = "6";//默认支持6个分站
                            NetworkModule.Upflag = "1";
                        }
                        NetworkModule.IsMemoryData = true;//搜索的时候把内存数据标记置为true   20170415
                        NetworkModule.InfoState = InfoState.AddNew;
                        //调用更新到缓存中
                        NetworkModuleCacheAddRequest AddNetworkModuleCacheRequest = new NetworkModuleCacheAddRequest();
                        AddNetworkModuleCacheRequest.NetworkModuleInfo = NetworkModule;
                        _NetworkModuleCacheService.AddNetworkModuleCache(AddNetworkModuleCacheRequest);
                    }
                    else//第一次搜索的时候，可能会获取不到IP，增加处理  20170627
                    {
                        Jc_MacInfo NetworkModule = ALLNetworkModule.Find(a => a.MAC == Network.Mac);
                        NetworkModule.MAC = Network.Mac;//网络模块MAC
                        NetworkModule.IP = Network.Ip;//网络模块IP                   
                        NetworkModule.Bz2 = Network.SwitchMac;//网络模块所属交换机MAC
                        NetworkModule.Bz3 = Network.AddressNumber.ToString();//网络模块在交换机中的地址编码

                        NetworkModule.SubMask = Network.SubMask;
                        NetworkModule.GatewayIp = Network.GatewayIp;

                        if (Network.DeviceType == 3)//表示分站带网络模块
                        {
                            NetworkModule.Bz5 = "1";
                            NetworkModule.Upflag = "0";
                            NetworkModule.BindStatinNumber = Network.StationAddress;
                        }
                        else
                        {
                            NetworkModule.Bz5 = "6";//默认支持6个分站
                            NetworkModule.Upflag = "1";
                        }

                        //调用更新到缓存中
                        NetworkModuleCacheUpdateRequest updateNetworkModuleCacheRequest = new NetworkModuleCacheUpdateRequest();
                        updateNetworkModuleCacheRequest.NetworkModuleInfo = NetworkModule;
                        _NetworkModuleCacheService.UpdateNetworkModuleCahce(updateNetworkModuleCacheRequest);
                    }
                }
                #endregion
            }
            else
            {
                #region 搜索交换机+分站
                Sys.DataCollection.Common.Protocols.SearchNetworkDeviceRequest searchNetworkDeviceRequest = new SearchNetworkDeviceRequest();
                MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SearchNetworkDeviceRequest);
                masProtocol.Protocol = searchNetworkDeviceRequest;
                masProtocol.StationFind = 0;
                //调用RPC发送搜索网络模块命令，并接收回传的参数
                var result = RpcService.Send<SearchNetworkDeviceResponse>(masProtocol, RequestType.DeviceUdpRequest);//todo RequestType类型增加3
                List<NetworkDeviceItem> NetworkDeviceItems = new List<NetworkDeviceItem>();
                if (result != null)
                {
                    NetworkDeviceItems = result.NetworkDeviceItems;
                }

                Sys.DataCollection.Common.Protocols.SearchNetworkDeviceRequest searchNetworkDeviceRequest1 = new SearchNetworkDeviceRequest();
                MasProtocol masProtocol1 = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SearchNetworkDeviceRequest);
                masProtocol1.Protocol = searchNetworkDeviceRequest1;
                masProtocol1.StationFind = 1;
                //调用RPC发送搜索网络模块命令，并接收回传的参数
                var result1 = RpcService.Send<SearchNetworkDeviceResponse>(masProtocol, RequestType.DeviceUdpRequest);//todo RequestType类型增加3
                if (result1 != null)
                {
                    NetworkDeviceItems.AddRange(result1.NetworkDeviceItems);
                }

                if (NetworkDeviceItems.Count < 1)
                {
                    Result.Code = 1;
                    Result.Message = "未搜索到设备！";
                    return Result;
                }
                //NetworkDeviceItems = result.NetworkDeviceItems;
                foreach (Sys.DataCollection.Common.Protocols.NetworkDeviceItem Network in NetworkDeviceItems)
                {
                    if (ALLNetworkModule.FindAll(a => a.MAC == Network.Mac).Count == 0)//缓存不存在此对象，则添加到缓存中
                    {
                        Jc_MacInfo NetworkModule = new Jc_MacInfo();
                        NetworkModule.ID = IdHelper.CreateLongId().ToString();
                        NetworkModule.MAC = Network.Mac;//网络模块MAC
                        NetworkModule.IP = Network.Ip;//网络模块IP
                        NetworkModule.Type = 0;
                        NetworkModule.Istmcs = 0;
                        NetworkModule.Wzid = "-1";// 20170331
                        NetworkModule.Wz = null;
                        NetworkModule.Bz2 = Network.SwitchMac;//网络模块所属交换机MAC
                        NetworkModule.Bz3 = Network.AddressNumber.ToString();//网络模块在交换机中的地址编码

                        NetworkModule.SubMask = Network.SubMask;
                        NetworkModule.GatewayIp = Network.GatewayIp;

                        if (Network.DeviceType == 3)//表示分站带网络模块
                        {
                            NetworkModule.Bz5 = "1";
                            NetworkModule.Upflag = "0";
                            NetworkModule.BindStatinNumber = Network.StationAddress;
                        }
                        else
                        {
                            NetworkModule.Bz5 = "6";//默认支持6个分站
                            NetworkModule.Upflag = "1";
                        }
                        NetworkModule.IsMemoryData = true;//搜索的时候把内存数据标记置为true   20170415
                        NetworkModule.InfoState = InfoState.AddNew;
                        //调用更新到缓存中
                        NetworkModuleCacheAddRequest AddNetworkModuleCacheRequest = new NetworkModuleCacheAddRequest();
                        AddNetworkModuleCacheRequest.NetworkModuleInfo = NetworkModule;
                        _NetworkModuleCacheService.AddNetworkModuleCache(AddNetworkModuleCacheRequest);
                    }
                    else//第一次搜索的时候，可能会获取不到IP，增加处理  20170627
                    {
                        Jc_MacInfo NetworkModule = ALLNetworkModule.Find(a => a.MAC == Network.Mac);
                        NetworkModule.MAC = Network.Mac;//网络模块MAC
                        NetworkModule.IP = Network.Ip;//网络模块IP                   
                        NetworkModule.Bz2 = Network.SwitchMac;//网络模块所属交换机MAC
                        NetworkModule.Bz3 = Network.AddressNumber.ToString();//网络模块在交换机中的地址编码

                        NetworkModule.SubMask = Network.SubMask;
                        NetworkModule.GatewayIp = Network.GatewayIp;

                        if (Network.DeviceType == 3)//表示分站带网络模块
                        {
                            NetworkModule.Bz5 = "1";
                            NetworkModule.Upflag = "0";
                            NetworkModule.BindStatinNumber = Network.StationAddress;
                        }
                        else
                        {
                            NetworkModule.Bz5 = "6";//默认支持6个分站
                            NetworkModule.Upflag = "1";
                        }

                        //调用更新到缓存中
                        NetworkModuleCacheUpdateRequest updateNetworkModuleCacheRequest = new NetworkModuleCacheUpdateRequest();
                        updateNetworkModuleCacheRequest.NetworkModuleInfo = NetworkModule;
                        _NetworkModuleCacheService.UpdateNetworkModuleCahce(updateNetworkModuleCacheRequest);
                    }
                }
                #endregion
            }

            //查找所有缓存包括后面增加的内容           
            NetworkModuleCacheGetAllRequest networkModuleNewCacheRequest = new NetworkModuleCacheGetAllRequest();
            var resultAllNetworkNew = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleNewCacheRequest);
            Result.Data = resultAllNetworkNew.Data;

            return Result;
        }
        /// <summary>
        /// 设置网络模块参数---基础参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse SetNetworkModuletParameters(NetworkModuletParametersSetRequest networkModuleCacheRequest)
        {
            BasicResponse Result = new BasicResponse();
            SetNetworkDeviceParamRequest setNetworkDeviceRequest = new SetNetworkDeviceParamRequest();
            setNetworkDeviceRequest.Mac = networkModuleCacheRequest.MAC;
            setNetworkDeviceRequest.NetworkDeviceParam = networkModuleCacheRequest.Parameters;
            setNetworkDeviceRequest.StationFind = networkModuleCacheRequest.StationFind;
            //调用RPC组件下发数据           
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SetNetworkDeviceParamRequest);
            masProtocol.Protocol = setNetworkDeviceRequest;

            //调用RPC发送搜索网络模块命令，并接收回传的参数
            var result = RpcService.Send<SetNetworkDeviceParamResponse>(masProtocol, RequestType.DeviceUdpRequest);

            if (result.ExeRtn != 1)
            {
                Result.Code = 1;
                Result.Message = "设置网络模块参数失败！";
            }
            return Result;
        }
        /// <summary>
        /// 设置网络模块参数---串口参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse SetNetworkModuletParametersComm(NetworkModuletCommParametersSetRequest networkModuleCacheRequest)
        {
            BasicResponse Result = new BasicResponse();
            SetNetworkDeviceParamCommRequest setNetworkDeviceRequest = new SetNetworkDeviceParamCommRequest();
            setNetworkDeviceRequest.Mac = networkModuleCacheRequest.MAC;
            setNetworkDeviceRequest.CommPort = networkModuleCacheRequest.CommPort;
            setNetworkDeviceRequest.NetworkDeviceParam = networkModuleCacheRequest.Parameters;
            //调用RPC组件下发数据           
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SetNetworkDeviceParamCommRequest);
            masProtocol.Protocol = setNetworkDeviceRequest;

            //调用RPC发送搜索网络模块命令，并接收回传的参数--todo,SetNetworkDeviceParamResponse没有改
            var result = RpcService.Send<SetNetworkDeviceParamCommResponse>(masProtocol, RequestType.DeviceUdpRequest);

            if (result.ExeRtn != 1)
            {
                Result.Code = 1;
                Result.Message = "设置网络模块串口参数失败！";
            }
            return Result;
        }
        /// <summary>
        /// 获取网络模块参数
        /// </summary>
        /// <param name="networkModuleCacheRequest"></param>
        /// <returns></returns>
        public BasicResponse<NetDeviceSettingInfo> GetNetworkModuletParameters(NetworkModuletParametersGetRequest networkModuleCacheRequest)
        {
            BasicResponse<NetDeviceSettingInfo> Result = new BasicResponse<NetDeviceSettingInfo>();

            QuerytNetworkDeviceParamRequest searchAssignNetDeviceRequest = new QuerytNetworkDeviceParamRequest();
            searchAssignNetDeviceRequest.Mac = networkModuleCacheRequest.Mac;
            //searchAssignNetDeviceRequest.WaitTime = networkModuleCacheRequest.WaitTime;
            //todo 调用RPC组件获取数据
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QuerytNetworkDeviceParamRequest);
            masProtocol.Protocol = searchAssignNetDeviceRequest;

            //调用RPC发送搜索网络模块命令，并接收回传的参数
            var result = RpcService.Send<QuerytNetworkDeviceParamResponse>(masProtocol, RequestType.DeviceUdpRequest);
            if (result == null)
            {
                Result.Code = 1;
                Result.Message = "获取网络模块参数失败！";
                return Result;
            }
            Result.Data = result.NetworkDeviceParam;

            return Result;
        }

        /// <summary>
        /// 网络模块保存巡检
        /// </summary>
        /// <returns></returns>
        public BasicResponse NetworkModuleSaveData()
        {
            BasicResponse Result = new BasicResponse();

            NetworkModuleCacheGetAllRequest networkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var resultAllNetwork = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheRequest);
            List<Jc_MacInfo> ALLNetworkModule = resultAllNetwork.Data;
            foreach (Jc_MacInfo NetworkModule in ALLNetworkModule)
            {
                if (NetworkModule.IsMemoryData)
                {
                    if (NetworkModule.Type == 0 && NetworkModule.Wzid == "-1" && !NetworkModuleIsDefinePoint(NetworkModule.Bz1))//处理未定义网络模块，只有绑定了分站的网络模块才保留，否则从缓存中清除 20170331
                    {
                        NetworkModuleCacheDeleteRequest DeleteNetworkModuleCacheRequest = new NetworkModuleCacheDeleteRequest();
                        DeleteNetworkModuleCacheRequest.NetworkModuleInfo = NetworkModule;
                        _NetworkModuleCacheService.DeleteNetworkModuleCache(DeleteNetworkModuleCacheRequest);
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// 判断网络模块是否绑定分站
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool NetworkModuleIsDefinePoint(string s)
        {
            //20170323 modified by  当s参数为空串时，之前会返回TRUE，导致未绑定的MAC删除不掉
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            bool flg = false;
            string[] fzh;
            try
            {
                fzh = s.Split('|');
                for (int i = 0; i < fzh.Length; i++)
                {
                    if (fzh[i] != "0")
                    {
                        flg = true;
                        break;
                    }
                }
            }
            catch
            { }
            return flg;
        }
        /// <summary>
        /// 向网关同步数据
        /// </summary>
        /// <param name="SendItemList"></param>
        /// <returns></returns>
        private bool SynchronousDataToGateway(List<Jc_MacInfo> SendItemList)
        {

            UpdateCacheDataRequest UpdateCache = new UpdateCacheDataRequest();
            List<NetworkDeviceInfo> UpdateCacheDataList = new List<NetworkDeviceInfo>();

            UpdateCacheDataList = ObjectConverter.CopyList<Jc_MacInfo, NetworkDeviceInfo>(SendItemList).ToList();
            foreach (NetworkDeviceInfo deviceInfo in UpdateCacheDataList)
            {
                deviceInfo.UniqueKey = deviceInfo.MAC;
            }
            UpdateCache.NetworkDeviceList = UpdateCacheDataList;
            //调用RPC发送
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.UpdateCacheDataRequest);
            masProtocol.Protocol = UpdateCache;
            var result = RpcService.Send<GatewayRpcResponse>(masProtocol, RequestType.BusinessRequest);
            if (result == null)
            {
                return false;
            }
            return result.IsSuccess;
            //return true;
        }

        public BasicResponse<List<string>> GetAllPowerBoxAddress(GetAllPowerBoxAddressByMacRequest request)
        {
            var req = new NetworkModuleCacheGetByConditonRequest
            {
                Predicate = a => a.MAC == request.Mac
            };
            var res = _NetworkModuleCacheService.GetNetworkModuleCache(req);
            var ret = new BasicResponse<List<string>>()
            {
                Data = new List<string>()
            };
            if (res.Data.Count != 0)
            {
                if (res.Data[0].BatteryItems != null)
                {
                    var retData = new List<string>();
                    foreach (var item in res.Data[0].BatteryItems)
                    {
                        retData.Add(item.BatteryAddress);
                    }
                    ret.Data = retData;
                }
            }
            return ret;
        }

        public BasicResponse<BatteryItem> GetSwitchBatteryInfo(GetSwitchBatteryInfoRequest request)
        {
            var req = new NetworkModuleCacheGetByConditonRequest
            {
                Predicate = a => a.MAC.ToString() == request.Mac
            };
            var res = _NetworkModuleCacheService.GetNetworkModuleCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var batteryItems = res.Data[0].BatteryItems;
            var ret = new BasicResponse<BatteryItem>();
            if (batteryItems != null)
            {
                var battery = from m in res.Data[0].BatteryItems
                              where m.BatteryAddress == request.Address
                              select m;
                ret.Data = battery.FirstOrDefault();
            }
            return ret;
        }

        public BasicResponse<GetSwitchAllPowerBoxInfoResponse> GetSwitchAllPowerBoxInfo(GetSwitchAllPowerBoxInfoRequest request)
        {
            var req = new NetworkModuleCacheGetByConditonRequest
            {
                Predicate = a => a.MAC.ToString() == request.Mac
            };
            var res = _NetworkModuleCacheService.GetNetworkModuleCache(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var model = res.Data.FirstOrDefault();
            if (model == null)
            {
                var ret = new BasicResponse<GetSwitchAllPowerBoxInfoResponse>
                {
                    Data = new GetSwitchAllPowerBoxInfoResponse()
                    {
                        PowerBoxInfo = new List<BatteryItem>(),
                        PowerDateTime = new DateTime()
                    }
                };
                return ret;
            }
            else
            {
                var ret = new BasicResponse<GetSwitchAllPowerBoxInfoResponse>
                {
                    Data = new GetSwitchAllPowerBoxInfoResponse()
                    {
                        PowerBoxInfo = model.BatteryItems,
                        PowerDateTime = model.PowerDateTime
                    }
                };
                return ret;
            }
        }
        /// <summary>
        /// 下发所有网络模块时间同步命令接口
        /// </summary>
        /// <returns></returns>
        public BasicResponse SetNetworkModuleSyncTime()
        {
            BasicResponse result = new BasicResponse();
            NetworkModuleCacheGetAllRequest networkModuleCacheRequest = new NetworkModuleCacheGetAllRequest();
            var resultGet = _NetworkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheRequest);
            List<Jc_MacInfo> macItems = resultGet.Data;
            if (macItems != null && macItems.Any())
            {
                foreach (Jc_MacInfo item in macItems)
                {
                    item.TimeSynchronization = true;
                    item.TimeSynchronizationcount = 3;//默认发3次网络模块时间同步命令
                    //更新指定字段到缓存中
                    Dictionary<string, object> paramater = new Dictionary<string, object>();
                    paramater.Add("TimeSynchronization", item.TimeSynchronization);
                    paramater.Add("TimeSynchronizationcount", item.TimeSynchronizationcount);
                    NetworkModuleCacheUpdatePropertiesRequest networkModuleCacheUpdatePropertiesRequest = new NetworkModuleCacheUpdatePropertiesRequest();
                    networkModuleCacheUpdatePropertiesRequest.Mac = item.MAC;
                    networkModuleCacheUpdatePropertiesRequest.UpdateItems = paramater;
                    _NetworkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
                }
            }
            return result;
        }


        public BasicResponse TestAlarm(TestAlarmRequest testAlarmRequest)
        {
            return _NetworkModuleCacheService.TestAlarm(testAlarmRequest);
        }
    }
}


