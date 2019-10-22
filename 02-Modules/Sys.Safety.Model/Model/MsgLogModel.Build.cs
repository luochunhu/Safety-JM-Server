using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("MS_MsgLog")]
    public partial class MsgLogModel
    {
        	    /// <summary>
        /// 唯一编码
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 短信接收人姓名
        /// </summary>
                public string UserName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 短信接收号码
        /// </summary>
                public string Phone
        {
           get;
           set;
        }
        	    /// <summary>
        /// 短信内容
        /// </summary>
                public string Message
        {
           get;
           set;
        }
        	    /// <summary>
        /// 发送时间
        /// </summary>
                public DateTime SendTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用1
        /// </summary>
                public string B1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备用2
        /// </summary>
                public string B2
        {
           get;
           set;
        }
            }
}

