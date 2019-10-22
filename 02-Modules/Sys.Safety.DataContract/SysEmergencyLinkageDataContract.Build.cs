using System;

namespace Sys.Safety.DataContract
{
    public partial class SysEmergencyLinkageInfo
    {
        /// <summary>
        ///     应急联动ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     应急联动名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     联动类型（1：普通联动，2:大数据分析联动）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 持续时间（秒）
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     主控测点关联ID（联动类型为1时有效）
        /// </summary>
        public string MasterPointAssId { get; set; }

        /// <summary>
        ///     主控设备类型关联ID（联动类型为1时有效）
        /// </summary>
        public string MasterDevTypeAssId { get; set; }

        /// <summary>
        ///     主控区域关联ID（联动类型为1时有效）
        /// </summary>
        public string MasterAreaAssId { get; set; }

        /// <summary>
        ///     主控触发数据状态关联ID（联动类型为1时有效）
        /// </summary>
        public string MasterTriDataStateAssId { get; set; }

        /// <summary>
        ///     主控模型ID（联动类型为2时有效）
        /// </summary>
        public string MasterModelId { get; set; }

        /// <summary>
        ///     被控测点关联ID
        /// </summary>
        public string PassivePointAssId { get; set; }

        /// <summary>
        ///     被控区域关联ID
        /// </summary>
        public string PassiveAreaAssId { get; set; }

        /// <summary>
        ///     被控人员关联ID（人员定位系统有效）
        /// </summary>
        public string PassivePersonAssId { get; set; }

        /// <summary>
        ///     编辑人
        /// </summary>
        public string EditPerson { get; set; }

        /// <summary>
        ///     编辑时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>删除人
        /// 
        /// </summary>
        public string DeletePerson { get; set; }

        /// <summary>删除时间
        /// 
        /// </summary>
        public DateTime DeleteTime { get; set; }

        /// <summary>是否活动测点（1：活动测点；0：非活动测点）
        /// 
        /// </summary>
        public int Activity { get; set; }

        /// <summary>
        /// </summary>
        public string Bz1 { get; set; }

        /// <summary>
        /// </summary>
        public string Bz2 { get; set; }

        /// <summary>
        /// </summary>
        public string Bz3 { get; set; }
    }
}