using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Phj;
using Sys.Safety.Request.R_Call;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Control.Model
{
    public class R_CallModel
    {
        static IR_CallService _R_CallService;
        static IR_PhjService _R_PhjService;
        static object obj = new object();
        private R_CallModel()
        {
            _R_CallService = ServiceFactory.Create<IR_CallService>();
            _R_PhjService = ServiceFactory.Create<IR_PhjService>();
        }
        private static volatile R_CallModel _R_CallModelInstance;
        /// <summary>
        /// 配置缓存单例
        /// </summary>
        public static R_CallModel R_CallModelInstance
        {
            get
            {
                if (_R_CallModelInstance == null)
                {
                    lock (obj)
                    {
                        if (_R_CallModelInstance == null)
                        {
                            _R_CallModelInstance = new R_CallModel();
                        }
                    }
                }
                return _R_CallModelInstance;
            }
        }
        /// <summary>
        /// 添加呼叫
        /// </summary>
        /// <param name="tempCallInfo"></param>
        public void AddR_CallInfo(R_CallInfo tempCallInfo)
        {
            //赋值操作用户，客户端IP地址
            UpdateCreateUser(tempCallInfo);

            R_CallAddRequest callRequest = new R_CallAddRequest();
            callRequest.CallInfo = tempCallInfo;
            _R_CallService.AddCall(callRequest);
        }
        private void UpdateCreateUser(R_CallInfo tempCallInfo)
        {            
            ClientItem clientItem = new ClientItem();
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            }
            tempCallInfo.CreateUserName = clientItem.UserName;
            tempCallInfo.CreateClientIP = Basic.Framework.Common.HardwareHelper.GetIPAddress();
        }
        /// <summary>
        /// 获取所有呼叫缓存
        /// </summary>
        /// <returns></returns>
        public List<R_CallInfo> GetAllRCallCache()
        {
            return _R_CallService.GetAllRCallCache(new RCallCacheGetAllRequest()).Data;
        }
        /// <summary>
        /// 解除呼叫
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveR_CallInfo(string Id)
        {
            R_CallInfo R_Call = GetAllRCallCache().Find(a => a.Id == Id);
            if (R_Call != null)
            {
                Dictionary<string, Dictionary<string, object>> updateItems = new Dictionary<string, Dictionary<string, object>>();
                Dictionary<string, object> updateItem = new Dictionary<string, object>();

                R_Call.CallType = 2;
                updateItem.Add("CallType", R_Call.CallType);
                R_Call.SendCount = 3;
                updateItem.Add("SendCount", R_Call.SendCount);
                //赋值操作用户，客户端IP地址
                UpdateCreateUser(R_Call);
                updateItem.Add("CreateUserName", R_Call.CreateUserName);
                updateItem.Add("CreateClientIP", R_Call.CreateClientIP);

                updateItems.Add(R_Call.Id,updateItem);

                R_CallUpdateProperitesRequest request = new R_CallUpdateProperitesRequest();
                request.updateItems = updateItems;
                _R_CallService.BachUpdateAlarmInfoProperties(request);                
            }
        }
    }
}
