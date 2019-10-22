using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.Request.SysEmergencyLinkage
{
    public partial class SysEmergencyLinkageAddRequest : Basic.Framework.Web.BasicRequest
    {
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }      
    }

	public partial class SysEmergencyLinkageUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }      
    }

	public partial class SysEmergencyLinkageDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SysEmergencyLinkageGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class SysEmergencyLinkageGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class AddEmergencylinkageconfigMasterInfoPassiveInfoRequest : BasicRequest
    {
        /// <summary>
        /// 应急联动配置信息
        /// </summary>
        public SysEmergencyLinkageInfo SysEmergencyLinkageInfo { get; set; }

        /// <summary>
        /// 应急联动主控区域信息
        /// </summary>
        public List<EmergencyLinkageMasterAreaAssInfo> EmergencyLinkageMasterAreaAssInfo { get; set; }

        /// <summary>
        /// 应急联动主控设备类型信息
        /// </summary>
        public List<EmergencyLinkageMasterDevTypeAssInfo> EmergencyLinkageMasterDevTypeAssInfo { get; set; }

        /// <summary>
        /// 应急联动主控测点信息
        /// </summary>
        public List<EmergencyLinkageMasterPointAssInfo> EmergencyLinkageMasterPointAssInfo { get; set; }

        /// <summary>
        /// 应急联动主控触发数据状态信息
        /// </summary>
        public List<EmergencyLinkageMasterTriDataStateAssInfo> EmergencyLinkageMasterTriDataStateAssInfo { get; set; }

        /// <summary>
        /// 应急联动被控区域信息
        /// </summary>
        public List<EmergencyLinkagePassiveAreaAssInfo> EmergencyLinkagePassiveAreaAssInfo { get; set; }

        /// <summary>
        /// 应急联动被控人员信息
        /// </summary>
        public List<EmergencyLinkagePassivePersonAssInfo> EmergencyLinkagePassivePersonAssInfo { get; set; }

        /// <summary>
        /// 应急联动被控测点信息
        /// </summary>
        public List<EmergencyLinkagePassivePointAssInfo> EmergencyLinkagePassivePointAssInfo { get; set; }
    }

    public partial class GetSysEmergencyLinkageListAndStatisticsResponse
    {
        /// <summary>应急联动ID
        ///     
        /// </summary>
        public string Id { get; set; }

        /// <summary>应急联动名称
        ///     
        /// </summary>
        public string Name { get; set; }

        /// <summary>联动类型（1：普通联动，2:大数据分析联动）
        ///     
        /// </summary>
        public int Type { get; set; }

        /// <summary>联动类型名称
        /// 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>持续时长
        /// 
        /// </summary>
        public int Duration { get; set; }

        /// <summary>主控测点关联id
        ///     
        /// </summary>
        public string MasterPointAssId { get; set; }

        /// <summary>主控测点数量
        ///     
        /// </summary>
        public string MasterPointNum { get; set; }

        /// <summary>主控设备类型关联id
        ///     
        /// </summary>
        public string MasterDevTypeAssId { get; set; }

        /// <summary>主控设备类型数量
        ///     
        /// </summary>
        public string MasterDevTypeNum { get; set; }

        /// <summary>主控区域关联id
        ///     
        /// </summary>
        public string MasterAreaAssId { get; set; }

        /// <summary>主控区域数量
        ///     
        /// </summary>
        public string MasterAreaNum { get; set; }

        /// <summary>主控触发数据状态关联id
        ///     
        /// </summary>
        public string MasterTriDataStateAssId { get; set; }

        /// <summary>主控触发数据状态数量
        ///     
        /// </summary>
        public string MasterTriDataStateNum { get; set; }

        /// <summary>主控模型名称
        ///     
        /// </summary>
        public string MasterModelName { get; set; }

        /// <summary>被控测点关联id
        ///     
        /// </summary>
        public string PassivePointAssId { get; set; }

        /// <summary>被控测点数量
        ///     
        /// </summary>
        public string PassivePointNum { get; set; }

        /// <summary>被控区域关联id
        ///     
        /// </summary>
        public string PassiveAreaAssId { get; set; }

        /// <summary>被控区域数量
        ///     
        /// </summary>
        public string PassiveAreaNum { get; set; }

        /// <summary>被控人员关联id
        ///     
        /// </summary>
        public string PassivePersonAssId { get; set; }

        /// <summary>被控人员数量
        ///     
        /// </summary>
        public string PassivePersonNum { get; set; }

        /// <summary>编辑人
        ///     
        /// </summary>
        public string EditPerson { get; set; }

        /// <summary>编辑时间
        ///     
        /// </summary>
        public string EditTime { get; set; }

        /// <summary>删除人
        /// 
        /// </summary>
        public string DeletePerson { get; set; }

        /// <summary>删除时间
        /// 
        /// </summary>
        public string DeleteTime { get; set; }
    }

    public class IdTextCheck
    {
        public bool Check { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public string AreaId { get; set; }

        public string SysId { get; set; }

        public string Point { get; set; }
    }

    public class UpdateRealTimeStateRequest
    {
        public string LinkageId { get; set; }

        public string State { get; set; }
    }
}
