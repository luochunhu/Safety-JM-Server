using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class R_CallInfo
    {
        /// <summary>
        /// 发送次数(默认发3次)
        /// </summary>
        protected int _SendCount = 3;
        /// <summary>
        /// 安装位置
        /// </summary>
        public int SendCount
        {
            get { return _SendCount; }
            set
            {
                _SendCount = value;
            }
        }
        /// <summary>
        /// 操作人
        /// </summary>
        protected string _CreateUserName = "";
        /// <summary>
        /// 安装位置
        /// </summary>
        public string CreateUserName
        {
            get { return _CreateUserName; }
            set
            {
                _CreateUserName = value;
            }
        }
        /// <summary>
        /// 操作客户端IP
        /// </summary>
        protected string _CreateClientIP = "";
        /// <summary>
        /// 操作客户端IP
        /// </summary>
        public string CreateClientIP
        {
            get { return _CreateClientIP; }
            set
            {
                _CreateClientIP = value;
            }
        }
    }
}


