using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.Custom
{
    public class GasContentAlarmInfo
    {
        /// <summary>主键
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>瓦斯含量分析配置id
        /// 
        /// </summary>
        public string GasContentAnalyzeConfigId { get; set; }

        /// <summary>瓦斯含量
        /// 
        /// </summary>
        public string GasContent { get; set; }

        /// <summary>报警测点
        /// 
        /// </summary>
        public string Point { get; set; }

        /// <summary>安装位置
        /// 
        /// </summary>
        public string Location { get; set; }
    }
}
