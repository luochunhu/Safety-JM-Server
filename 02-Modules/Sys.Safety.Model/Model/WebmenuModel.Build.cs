using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_WEBMenu")]
    public partial class WebmenuModel
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        [Key]
        public string ModuleID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块编号
        /// </summary>
                public string ModuleCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块名称
        /// </summary>
                public string ModuleName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块链接地址
        /// </summary>
                public string ModuleUrl
        {
           get;
           set;
        }
        	    /// <summary>
        /// 父模块编号
        /// </summary>
                public string ModuleParent
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块标志位(-1:代表为一级模块)
        /// </summary>
                public string ModuleMemo
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块使用标志(0:禁用,1:启用)
        /// </summary>
                public string ModuleFlag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块状态标志，是否可见(1：显示，0：隐藏)
        /// </summary>
                public string ModuleStatus
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块排序标志
        /// </summary>
                public int ModuleSort
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块所属系统
        /// </summary>
                public int Sysflag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块小图标名称(菜单图标)
        /// </summary>
                public string ModuleIcon
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块大图标名称
        /// </summary>
                public string ModuleImg
        {
           get;
           set;
        }
        	    /// <summary>
        /// 模块是否通过iframe加载到主框架中
        /// </summary>
                public string LoadByIframe
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否将该模块设置为系统桌面，如果是，则加载该系统时，自动加载该模块
        /// </summary>
                public string IsSystemDesktop
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据标记
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用1
        /// </summary>
                public string By1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用2
        /// </summary>
                public string By2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用3
        /// </summary>
                public string By3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用4
        /// </summary>
                public string By4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用5
        /// </summary>
                public string By5
        {
           get;
           set;
        }
            }
}

