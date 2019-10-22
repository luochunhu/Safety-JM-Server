using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.User;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ClientFramework.UserRoleAuthorize
{
    /// <summary>
    /// 客户端权限管理
    /// </summary>
    public class ClientPermission
    {
        static IUserService _UserService = ServiceFactory.Create<IUserService>();
        static IRightService _RightService = ServiceFactory.Create<IRightService>();
        static List<RightInfo> rightList = null;
        /// <summary>
        /// 最后一次更新的用户权限信息
        /// </summary>
        public static string userRightString = "";
        /// <summary>
        /// 新增权限点到缓存
        /// </summary>
        /// <param name="item"></param>
        public static void AddRight(RightItem item)
        {
            Dictionary<string, RightItem> rightsCache = null;
            rightsCache = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, RightItem>>(Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString());
            if (rightsCache == null)
            {
                rightsCache = new Dictionary<string, RightItem>();
            }
            if (rightsCache.ContainsKey(item.RightCode))
            {
                rightsCache[item.RightCode] = item;
            }
            else
            {
                rightsCache.Add(item.RightCode, item);
            }
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.RightKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey] = rightsCache;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(KeyConst.RightKey, rightsCache);
            }
        }

        /// <summary>
        /// 批量新增权限点到缓存
        /// </summary>
        /// <param name="items"></param>
        public static void AddRights(IList<RightItem> items)
        {
            Dictionary<string, RightItem> rightsCache = null;
            rightsCache = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, RightItem>>(Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString());
            if (rightsCache == null)
            {
                rightsCache = new Dictionary<string, RightItem>();
            }
            foreach (RightItem item in items)
            {
                if (rightsCache.ContainsKey(item.RightCode))
                {
                    rightsCache[item.RightCode] = item;
                }
                else
                {
                    rightsCache.Add(item.RightCode, item);
                }
            }

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.RightKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey] = rightsCache;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(KeyConst.RightKey, rightsCache);
            }
        }

        /// <summary>
        /// 移除权限点
        /// </summary>
        /// <param name="rightCode">权限点编码</param>
        public static void RemoveRight(string rightCode)
        {
            if (string.IsNullOrEmpty(rightCode))
            {
                return;
            }

            Dictionary<string, RightItem> rightsCache = null;
            rightsCache = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, RightItem>>(Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString());
            if (rightsCache == null)
            {
                LogHelper.Debug("ClientPermission类RemoveRight方法，权限缓存对象 list = ClientContext.Current.GetContext(KeyConst.ClientRightKey) as Dictionary<string, RightItem>; list对象为空。");
                return;
            }

            if (rightsCache.ContainsKey(rightCode))
            {
                rightsCache.Remove(rightCode);
            }

            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.RightKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey] = rightsCache;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(KeyConst.RightKey, rightsCache);
            }
        }

        /// <summary>
        /// 清除所有权限点
        /// </summary>
        public static void RemoveAll()
        {
            Basic.Framework.Data.PlatRuntime.Items.Remove(KeyConst.RightKey);
        }


        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="rightCode">权限点编码</param>
        /// <returns>True 为有权限；False为没有权限</returns>
        public static bool Authorize(string rightCode)
        {
            userRightString = Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString();
            if (string.IsNullOrEmpty(rightCode))
            {
                return false;
            }

            bool result = false;

            Dictionary<string, RightItem> rightsCache = null;
            rightsCache = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, RightItem>>(Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString());
            if (rightsCache == null)
            {
                LogHelper.Debug("ClientPermission类Authorize方法，权限缓存对象 rightsCache = ClientContext.Current.GetContext(KeyConst.RightKey) as Dictionary<string, RightItem>; list对象为空。");
                return false;
            }

            if (rightsCache.ContainsKey(rightCode))
            {
                result = true;
            }
            else//如果没有权限，则判断当前权限是否在权限表中存在，如果未定义权限，则默认为启用  20180111
            {
                if (rightList == null)
                {
                    rightList = _RightService.GetRightList().Data;//性能优化，不用每次请求
                }
                if (rightList.FindAll(a => a.RightCode == rightCode).Count < 1)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 判断用户是否登录
        /// </summary>
        private static bool IsLogin
        {
            get
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.IsLoginKey))
                {
                    return TypeConvert.ToBool(Basic.Framework.Data.PlatRuntime.Items[KeyConst.IsLoginKey]);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 初始化客户端登录信息（登录成功后调用）
        /// </summary>
        /// <param name="loginContext">登录上下文</param>
        public static void InitLogin(Dictionary<string, object> loginContext)
        {
            //if (IsLogin)
            //{
            //    return;
            //}

            if (loginContext == null)
            {
                return;
            }

            foreach (string key in loginContext.Keys)
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(key))
                {
                    Basic.Framework.Data.PlatRuntime.Items[key] = loginContext[key];
                }
                else
                {
                    Basic.Framework.Data.PlatRuntime.Items.Add(key, loginContext[key]);
                }
            }
            ClientItem _ClientItem = new ClientItem();
            if (loginContext.ContainsKey(KeyConst.SessionIdKey))
            {
                _ClientItem.SessionId = loginContext[KeyConst.SessionIdKey].ToString();
            }
            if (loginContext.ContainsKey(KeyConst.LoginUserKey))
            {
                _ClientItem.UserName = loginContext[KeyConst.LoginUserKey].ToString();
            }
            if (loginContext.ContainsKey(KeyConst.LoginTimeKey))
            {
                _ClientItem.LoginTime = TypeConvert.ToDateTime(loginContext[KeyConst.LoginTimeKey].ToString());
            }
            _ClientItem.LastOptTime = DateTime.Now;

            UserGetByCodeRequest userrequest = new UserGetByCodeRequest();
            userrequest.Code = _ClientItem.UserName;
            var result = _UserService.GetUserByCode(userrequest);
            if (result.Data != null)
            {
                _ClientItem.UserID = result.Data.UserID;
            }

            //保存到运行对象缓存中
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] = _ClientItem;
            }
            else
            {
                Basic.Framework.Data.PlatRuntime.Items.Add(KeyConst.ClientItemKey, _ClientItem);
            }

        }



    }
}
