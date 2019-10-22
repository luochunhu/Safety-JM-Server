using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_Menu")]
    public partial class MenuModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [Key]
        public string MenuID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单编码
        /// </summary>
                public string MenuCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单名称
        /// </summary>
                public string MenuName
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
        /// 父菜单编号
        /// </summary>
                public string MenuParent
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单标志位
        /// </summary>
                public int MenuMemo
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单使用标志
        /// </summary>
                public int MenuFlag
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
        /// 菜单排序标志
        /// </summary>
                public string MenuSort
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单是否在菜单树中进行加载
        /// </summary>
                public int MenuStatus
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
        /// 菜单小图标
        /// </summary>
                public string MenuSmallIcon
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单大图标
        /// </summary>
                public string MenuLargeIcon
        {
           get;
           set;
        }
        	    /// <summary>
        /// 窗口打开次数
        /// </summary>
                public int LoadByIframe
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统桌面标记
        /// </summary>
                public int IsSystemDesktop
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
        /// 权限编号
        /// </summary>
                public string RightCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 请求库编码
        /// </summary>
                public string RequestCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 菜单简称(已使用)
        /// </summary>
                public string Remark1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注2
        /// </summary>
                public string Remark2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注3
        /// </summary>
                public string Rmark3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注4
        /// </summary>
                public string Remark4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注5
        /// </summary>
                public string Remark5
        {
           get;
           set;
        }
            }
}

