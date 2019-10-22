using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Basic.Framework.Web;

namespace Sys.Safety.DataContract
{
    public partial class R_AreaAlarmInfo : BasicInfo
    {        
         	    /// <summary>
        /// 主键ID
        /// </summary>
        public string Id
        {
           get;
           set;
        }
         	    /// <summary>
        /// 区域Id 
        /// </summary>
        public string AreaId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据状态，类型编码，详见附件1：类型编码说明（关联数据状态）
        /// </summary>
        public string Type
        {
           get;
           set;
        }
         	    /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Stime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Etime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 处理人
        /// </summary>
        public string AgentUserId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 处理措施
        /// </summary>
        public string Cs
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用1
        /// </summary>
        public string Bz1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用2
        /// </summary>
        public string Bz2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用3
        /// </summary>
        public string Bz3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用4
        /// </summary>
        public string Bz4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用5
        /// </summary>
        public string Bz5
        {
           get;
           set;
        }
         	    /// <summary>
        /// 上传标志0-未传1-已传
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
            }
}


