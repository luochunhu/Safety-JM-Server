using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RoledatarightInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据权限类型【枚举类型（0：测点权限，可扩展）】
        /// </summary>
        public int DataRightType
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据权限ID测点关联：JC_DEF表中的PointID其它关联：【可业务系统自定义】
        /// </summary>
        public string DataID
        {
           get;
           set;
        }
            }
}


