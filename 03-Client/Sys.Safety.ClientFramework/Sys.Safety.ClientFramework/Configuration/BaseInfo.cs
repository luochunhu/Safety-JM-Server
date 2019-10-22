using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.ClientFramework.Configuration
{
    public class BaseInfo
    {
        /// <summary>
        /// 系统图标文件名-（全路径）
        /// </summary>
        public static string FormIcon = string.Empty;
        /// <summary>
        /// 当前软件ID
        /// </summary>
        public static string SoftName = string.Empty;
        /// <summary>
        /// 软件的名称
        /// </summary>
        public static string SoftFullName = string.Empty;
        /// <summary>
        /// 当前版本
        /// </summary>
        public static string Version = "V1.0";
        /// <summary>
        ///  当前客户公司名称
        /// </summary>
        public static string CustomerCompanyName = string.Empty;
        /// <summary>
        /// 是否自动登录
        /// </summary>
        public static string AutoLogoIn = "0";
        /// <summary>
        /// 自动登录用户
        /// </summary>
        public static string AutoLogoUser = "";
        /// <summary>
        /// 自动登录密码
        /// </summary>
        public static string AutoLogoPass = "";
        /// <summary>
        /// 当前菜单类型（0：实用菜单，1：AQ菜单） 
        /// </summary>
        public static string MenuType = "1";
        /// <summary>
        /// 是否采用GIS图形定义界面（0：不采用，1：采用）
        /// </summary>
        public static string GraphicDefine = "1";

        /// <summary>
        /// 当前窗体样式
        /// </summary>
        public static string WindowStyleNow = "蓝色风格(默认)";
    }
}
