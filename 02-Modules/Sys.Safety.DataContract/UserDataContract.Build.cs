using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class UserInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 登录用户名(编码)
        /// </summary>
        public string UserCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 真实用户姓名
        /// </summary>
        public string UserName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
           get;
           set;
        }
         	    /// <summary>
        /// 所属单位编码
        /// </summary>
        public string DeptCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 登陆次数
        /// </summary>
        public int LoginCount
        {
           get;
           set;
        }
         	    /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 登陆时间
        /// </summary>
        public DateTime LoginTime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LastLoginTime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否使用
        /// </summary>
        public int UserFlag
        {
           get;
           set;
        }
         	    /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone
        {
           get;
           set;
        }
         	    /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Remark1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Remark2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Remark3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Remark4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Remark5
        {
           get;
           set;
        }
        /// <summary>
        /// 报警推送使用，用于判断是否选择了推送人员
        /// </summary>
        public bool IsCheck
        {
            get;
            set;
        }
            }
}


