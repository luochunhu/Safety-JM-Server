using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
   public class EmergencyLinkageConfigBusinessModel
   {      
       /// <summary>
       /// 应急联动
       /// </summary>
       public List<JC_EmergencyLinkageConfigInfo> EmergencyLinkageConfigInfoList { get; set; }
       /// <summary>
       /// 应急联动
       /// </summary>
       public JC_EmergencyLinkageConfigInfo EmergencyLinkageConfigInfo { get; set; }
       /// <summary>
       /// 模型ID
       /// </summary>
       public string AnalysisModelId { get; set; }
    }
}
