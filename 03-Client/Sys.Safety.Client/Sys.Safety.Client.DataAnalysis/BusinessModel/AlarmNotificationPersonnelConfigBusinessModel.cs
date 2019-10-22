using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
    public class AlarmNotificationPersonnelConfigBusinessModel
    { /// <summary>
        ///报警配置列表
        /// </summary>
        public List<JC_AlarmNotificationPersonnelConfigInfo> AlarmNotificationPersonnelConfigInfoList { get; set; }
        /// <summary>
        ///报警配置信息
        /// </summary>
        public JC_AlarmNotificationPersonnelConfigInfo AlarmNotificationPersonnelConfigInfo { get; set; }
        /// <summary>
        /// 报警推送人员信息
        /// </summary>
        public List<JC_AlarmNotificationPersonnelInfo> AlarmNotificationPersonnelInfoList { get; set; }
        /// <summary>
        /// 分页
        /// </summary>
        public PagerInfo pagerInfo { get; set; }
        public AlarmNotificationPersonnelConfigBusinessModel() {
            AlarmNotificationPersonnelConfigInfo = new JC_AlarmNotificationPersonnelConfigInfo();
            AlarmNotificationPersonnelInfoList = new List<JC_AlarmNotificationPersonnelInfo>();
            AlarmNotificationPersonnelConfigInfoList = new List<JC_AlarmNotificationPersonnelConfigInfo>();
        }
    }
}
