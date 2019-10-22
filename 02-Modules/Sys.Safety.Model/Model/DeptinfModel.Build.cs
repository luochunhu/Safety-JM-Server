using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_DeptInf")]
    public partial class DeptinfModel
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [Key]
        public string DeptID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位（部门）编码
        /// </summary>
                public string DeptCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位（部门）名称
        /// </summary>
                public string DeptName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位（部门）负责人用户名
        /// </summary>
                public string DeptLegal
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位地址
        /// </summary>
                public string Address
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位联系信息
        /// </summary>
                public string ContactInf
        {
           get;
           set;
        }
        	    /// <summary>
        /// 单位总人数
        /// </summary>
                public int WorkerCount
        {
           get;
           set;
        }
        	    /// <summary>
        /// 建设时间
        /// </summary>
                public DateTime DesignTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 年产量
        /// </summary>
                public string DesignPro
        {
           get;
           set;
        }
        	    /// <summary>
        /// 设计服务年限
        /// </summary>
                public string DesignNum
        {
           get;
           set;
        }
        	    /// <summary>
        /// 开采储量数据
        /// </summary>
                public string Reser
        {
           get;
           set;
        }
        	    /// <summary>
        /// 所属上级单位编码
        /// </summary>
                public string LDeptCode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 部门类型编码（0集团公司；1分局；2煤矿；3部门;4区队;5班组）
        /// </summary>
                public string DeptType
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

