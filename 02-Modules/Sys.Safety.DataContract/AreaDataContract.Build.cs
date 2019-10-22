using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class AreaInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string Areaid
        {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Areaname
        {
            get;
            set;
        }
        /// <summary>
        /// 区域描述
        /// </summary>
        public string Areadescribe
        {
            get;
            set;
        }
        /// <summary>
        /// 区域边界(存储区域边界的坐标信息)
        /// </summary>
        public string AreaBound
        {
            get;
            set;
        }

        private string _loc = "";

        /// <summary>
        /// 编码
        /// </summary>
        public string Loc
        {
            get { return _loc;}
            set { _loc = value; }
        }

        private string _parentloc = "";

        /// <summary>
        /// 父编码
        /// </summary>
        public string Parentloc
        {
            get { return _loc; }
            set { _loc = value; }
        }

        /// <summary>
        /// 管理人员信息【备用】
        /// </summary>
        public string Managerinfo
        {
            get;
            set;
        }
        /// <summary>
        /// 是否删除（0：非活动，1：活动）
        /// </summary>
        public string Activity
        {
            get;
            set;
        }
        /// <summary>
        /// 创建或者更新时间
        /// </summary>
        public DateTime CreateUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 备注1
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 备注2
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 备注3
        /// </summary>
        public string Bz3
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


