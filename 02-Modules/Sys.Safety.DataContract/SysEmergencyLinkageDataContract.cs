using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class SysEmergencyLinkageInfo
    {
        /// <summary>
        /// 主控区域列表
        /// </summary>
        public List<EmergencyLinkageMasterAreaAssInfo> MasterAreas { get; set; }

        /// <summary>
        /// 主控设备类型列表
        /// </summary>
        public List<EmergencyLinkageMasterDevTypeAssInfo> MasterDevTypes { get; set; }

        /// <summary>
        /// 主控测点
        /// </summary>
        public List<EmergencyLinkageMasterPointAssInfo> MasterPoint { get; set; }

        /// <summary>
        /// 主控数据状态
        /// </summary>
        public List<EmergencyLinkageMasterTriDataStateAssInfo> MasterTriDataStates { get; set; }

        /// <summary>
        /// 被控区域
        /// </summary>
        public List<EmergencyLinkagePassiveAreaAssInfo> PassiveAreas { get; set; }

        /// <summary>
        /// 被控人员
        /// </summary>
        public List<EmergencyLinkagePassivePersonAssInfo> PassivePersons { get; set; }

        /// <summary>
        /// 被控测点
        /// </summary>
        public List<EmergencyLinkagePassivePointAssInfo> PassivePoints { get; set; }

        /// <summary>
        /// 应急联动配置状态（0-不成立；1-成立）
        /// </summary>
        public int EmergencyLinkageState { get; set; }

        /// <summary>
        /// 是否强制结束
        /// </summary>
        public bool IsForceEnd { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 延时时间
        /// </summary>
        public int DelayTime { get; set; }
    }
}
