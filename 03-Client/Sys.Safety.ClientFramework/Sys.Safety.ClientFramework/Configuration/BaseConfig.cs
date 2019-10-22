using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.ClientFramework.Configuration
{
    /// <summary>
    /// 连接配置
    /// </summary>
    public class BaseConfig
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public const string COMPANYNAME = "CompanyName"; 
        /// <summary>
        /// 软件名称
        /// </summary>
        public const string SOFTNAME = "SoftName";
        /// <summary>
        /// 软件全名
        /// </summary>
        public const string SOFTFULLNAME = "SoftFullName"; 
        /// <summary>
        /// 软件版本
        /// </summary>
        public const string VERSION = "Version";
        /// <summary>
        /// 是否自动登录
        /// </summary>
        public const string AUTOLOGOIN = "AutoLogoIn";
        /// <summary>
        /// 自动登录用户名
        /// </summary>
        public const string AUTOLOGOUSER = "AutoLogoUser";
        /// <summary>
        /// 自动登录密码
        /// </summary>
        public const string AUTOLOGOPASS = "AutoLogoPass";

        /// <summary>
        /// 当前菜单类型
        /// </summary>
        public const string MENUTYPE = "MenuType";
        /// <summary>
        /// 当前菜单类型
        /// </summary>
        public const string GRAPHICDEFINE = "GraphicDefine";


        #region 构造方法
        /// <summary>
        /// 构造方法
        /// </summary>
        public BaseConfig()
        {
        }
        #endregion

        #region 读取配置信息
        public static void GetSetting()
        {
            ConfigHelper.GetConfig();
        }
        #endregion
    }
}
