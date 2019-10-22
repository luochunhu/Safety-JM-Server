using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.UserRoleAuthorize
{
    /// <summary>
    /// 常用的Key 常量类定义
    /// </summary>
    public class KeyConst
    {
        /// <summary>
        /// 存上SessionState或者CallContext里的Key名称
        /// value类型 
        /// </summary>
        public const string ContextKey = "ApplicationContext";

        /// <summary>
        /// 客户端缓存 客户信息KEY名称
        /// value类型 ClientItem 对象
        /// </summary>
        public const string ClientItemKey = "_ClientItem";

        /// <summary>
        /// 权限缓存Key名称
        /// value类型 Dictionary<string, RightItem>
        /// </summary>
        public const string RightKey = "_Rights";

        /// <summary>
        /// 用户登录名KEY名称
        /// value类型 string
        /// </summary>
        public const string LoginUserKey = "_LoginUserCode";

        /// <summary>
        /// 用户登录密码 Key名称
        /// value类型 string
        /// </summary>
        public const string LoginPasswordKey = "_LoginPassword";

        /// <summary>
        /// 用户会话ID Key名称
        /// value类型 string 的Guid
        /// </summary>
        public const string SessionIdKey = "_SessionId";

        /// <summary>
        /// 登录时间 Key名称
        /// value类型 DateTime
        /// </summary>
        public const string LoginTimeKey = "_LoginTime";

        /// <summary>
        /// 最后一次操作时间 Key名称
        /// value类型 DateTime
        /// </summary>
        public const string LastUpdateTimeKey = "_LastUpdateTime";

        /// <summary>
        /// 是否登录 Key名称
        /// value类型 bool
        /// </summary>
        public const string IsLoginKey = "_IsLogin";

        /// <summary>
        /// 用户DTO对象 Key名称
        /// value类型 UserDTO
        /// </summary>
        public const string UserDtoKey = "_UserDto";

        /// <summary>
        /// 客户端菜单类型 Key名称        
        /// </summary>
        public const string UserMenuTypeKey = "_MenuType";

        /// <summary>
        /// 动态参数key
        /// value类型 Dictionary<string, string>
        /// </summary>
        public const string DynamicParameterKey = "_DynamicParameter";

        /// <summary>
        /// 显示配置改变时间
        /// </summary>
        public const string RealListCfgUpdateTimeKey = "_RealListCfgUpdateTime";

        /// <summary>
        /// 定义改变时间 判断定义是否改变
        /// </summary>
        public const string DefUpdateTimeKey = "_DefUpdateTime";

        /// <summary>
        /// 应急联动配置改变时间, 判断应急联动配置是否改变
        /// </summary>
        public const string EmergencyLinkageChangedKey = "_EmergencyLinkageChangedKey";

        /// <summary>
        /// 大数据分析模型改变时间, 判断大数据分析模型是否改变
        /// </summary>
        public const string AnalysisModelChangedKey = "_AnalysisModelChangedKey";

        /// <summary>
        /// 报警通知配置改变时间, 判断报警通知配置是否改变
        /// </summary>
        public const string AlarmNotificationChangedKey = "_AlarmNotificationChangedKey";

        /// <summary>
        /// 区域断电配置改变时间, 判断区域断电配置是否改变
        /// </summary>
        public const string RegionOutageChangedKey = "_RegionOutageChangedKey";
    }
}
