using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class JC_AlarmNotificationPersonnelConfigInfo
    {
        private List<JC_AlarmNotificationPersonnelInfo> jc_AlarmNotificationPersonnelInfoList = null;
        /// <summary>
        /// 报警通知人员列表.
        /// </summary>
        public List<JC_AlarmNotificationPersonnelInfo> JC_AlarmNotificationPersonnelInfoList
        {
            get
            {
                return jc_AlarmNotificationPersonnelInfoList == null ? new List<JC_AlarmNotificationPersonnelInfo>() : jc_AlarmNotificationPersonnelInfoList;
            }
            set
            {
                jc_AlarmNotificationPersonnelInfoList = value;
            }
        }
    }
}
