using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 客户端对象
    /// </summary>   
    public class ClientItem 
    {
        public const string HeaderLocalName = "ClientItem";
        public const string HeaderNamespace = "";

        /// <summary>
        /// 客户端的SessionId
        /// </summary>      
        public string SessionId { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>      
        public string UserName { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>       
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>      
        public string LoginIP { get; set; }

        /// <summary>
        /// 登录MAC地址
        /// </summary>       
        public string LoginMac { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>       
        public DateTime LastOptTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>      
        public string UserID { get; set; }
    }
}
