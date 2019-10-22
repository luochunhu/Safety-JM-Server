using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ListcommandexInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 列表按钮命令ID
        /// </summary>
        public int ListCommandID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 所属列表ID
        /// </summary>
        public int ListID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 请求参数
        /// </summary>
        public string Parameters
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限编码
        /// </summary>
        public string StrRequestCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 列表命令编码
        /// </summary>
        public string StrListCommandCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 列表命令名显示的汉字标题
        /// </summary>
        public string StrListCommandName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 列表命令提示
        /// </summary>
        public string StrListCommandTip
        {
           get;
           set;
        }
         	    /// <summary>
        /// 采用的图标索引
        /// </summary>
        public string StrListIconName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否显示
        /// </summary>
        public bool BlnVisible
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否作为默认列表双击时的相应事件
        /// </summary>
        public bool BlnDblClick
        {
           get;
           set;
        }
         	    /// <summary>
        /// 显示顺序
        /// </summary>
        public int LngRowIndex
        {
           get;
           set;
        }
         	    /// <summary>
        /// 客户标识
        /// </summary>
        public string StrCustomer
        {
           get;
           set;
        }
         	    /// <summary>
        /// Web请求参数
        /// </summary>
        public string StrWebParameters
        {
           get;
           set;
        }
         	    /// <summary>
        /// Web图标
        /// </summary>
        public string StrWebListIconName
        {
           get;
           set;
        }
            }
}


