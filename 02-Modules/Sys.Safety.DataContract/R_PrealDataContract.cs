using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class R_PrealInfo
    {        
        /// <summary>
        /// 报警类型描述
        /// </summary>
        public string BjtypeDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 标识卡欠压标识（1：欠压，0：正常）
        /// </summary>
        public string PowerUnderVoltageFlag
        {
            get;
            set;
        }

        /// <summary>
        /// 工号。Yid-R_personinf.gh
        /// </summary>
        public string JobNumber { get; set; }

        /// <summary>
        /// 姓名。Yid-R_personinf.name
        /// </summary>
        public string PersonName { get; set; }

        /// <summary>
        /// 部门。Yid-R_personinf.bm-R_Dept.location-dept
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 职务。Yid-R_personinf.zw-R_Pdef.id-name
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 工种。Yid-R_personinf.gz-R_Pdef.id-name
        /// </summary>
        public string TypeOfWork { get; set; }

        /// <summary>
        /// 当前位置。point-PT_Def.point-point(wz)
        /// </summary>
        public string CurrentPosition { get; set; }

        /// <summary>
        /// 来源位置。uppoint-PT_Def.point-point(wz)
        /// </summary>
        public string UpPosition { get; set; }

        /// <summary>
        /// 入井位置。onpoint-PT_Def.point-point(wz)
        /// </summary>
        public string OnPosition { get; set; }
    }
}


