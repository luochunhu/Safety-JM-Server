using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_Department")]
    public partial class R_DeptModel
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [Key]
        public string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 父节点部门编号
        /// </summary>
        public string Parentid
        {
            get;
            set;
        }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Dept
        {
            get;
            set;
        }
        /// <summary>
        /// 当前部门编号
        /// </summary>
        public string Location
        {
            get;
            set;
        }
        /// <summary>
        /// 部门排序编号
        /// </summary>
        public int Order_id
        {
            get;
            set;
        }
        /// <summary>
        /// 部门负责人职务编号
        /// </summary>
        public string Zu
        {
            get;
            set;
        }
        /// <summary>
        /// 部门负责人的Yid
        /// </summary>
        public string Manager
        {
            get;
            set;
        }
        /// <summary>
        /// 部门备注信息
        /// </summary>
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 编制人数
        /// </summary>
        public int Pcount
        {
            get;
            set;
        }
        /// <summary>
        /// 部门编制限制
        /// </summary>
        public int Isforbid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
    }
}

