using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("VO_DeviceDefInfo")]
    public partial class V_DefModel
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
        /// 区域Id
        /// </summary>
        public string AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string Devname
        {
            get;
            set;
        }
        /// <summary>
        /// 厂商 0-海康，1-大华
        /// </summary>
        public int Vendor
        {
            get;
            set;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 端口
        /// </summary>
        public string Port
        {
            get;
            set;
        }
        /// <summary>
        /// 通道号
        /// </summary>
        public int Channel
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆名
        /// </summary>
        public string Username
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By5
        {
            get;
            set;
        }
    }
}

