using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_SysInf")]
    public partial class SysinfModel
    {
        	    /// <summary>
        /// ID
        /// </summary>
                [Key]   
        public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统简称
        /// </summary>
                public string Sysname
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统全称
        /// </summary>
                public string SysfullName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统标志/编码(与Mas_Modules中的ModuleForSys关联)
        /// </summary>
                public int SysID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否使用(0:禁用,1:启用)
        /// </summary>
                public string Useflag
        {
           get;
           set;
        }
        	    /// <summary>
        /// 是否默认系统(1:默认系统,0:非默认系统)
        /// </summary>
                public string IsDefault
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据标记
        /// </summary>
                public string Upfalg
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统图标名称
        /// </summary>
                public string SystemIcon
        {
           get;
           set;
        }
        	    /// <summary>
        /// 系统图片名称
        /// </summary>
                public string SystemImage
        {
           get;
           set;
        }
        	    /// <summary>
        /// 排序ID
        /// </summary>
                public int SystemSortId
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

