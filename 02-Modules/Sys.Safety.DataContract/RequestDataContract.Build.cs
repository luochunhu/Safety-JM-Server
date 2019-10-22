using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RequestInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 请求编号
        /// </summary>
        public string RequestID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 请求编码
        /// </summary>
        public string RequestCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 请求名称
        /// </summary>
        public string RequestName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单连接地址
        /// </summary>
        public string MenuURL
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单所属程序文件
        /// </summary>
        public string MenuFile
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单所属文件命名空间
        /// </summary>
        public string MenuNamespace
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单窗体参数
        /// </summary>
        public string MenuParams
        {
           get;
           set;
        }
         	    /// <summary>
        /// 模态标记
        /// </summary>
        public int ShowType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 窗体打开次数
        /// </summary>
        public int LoadByIframe
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单响应类型
        /// </summary>
        public int MenuForSys
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单方法名称（已使用）
        /// </summary>
        public string BZ1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string BZ2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string BZ3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string BZ4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public string BZ5
        {
           get;
           set;
        }
            }
}


