using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    /// <summary>
    /// 模拟量日报表返回数据
    /// </summary>
    public class MnlDayReportDataContract
    {
        /// <summary>
        /// 值的ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointID { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Wz { get; set; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public string DevName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Xs1 { get; set; }
        /// <summary>
        /// 报警门限
        /// </summary>
        public string Z2 { get; set; }
        /// <summary>
        /// 断电门限
        /// </summary>
        public string Z3 { get; set; }
        /// <summary>
        /// 复电门限
        /// </summary>
        public string Z4 { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public string Zdz { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public string Zxz { get; set; }
        /// <summary>
        /// 平均值
        /// </summary>
        public string Pjz { get; set; }
        /// <summary>
        /// 最大值时间
        /// </summary>
        public string Zdzs { get; set; }
        /// <summary>
        /// 报警次数
        /// </summary>
        public string BjCount { get; set; }
        /// <summary>
        /// 累计报警时间
        /// </summary>
        public string BjTime { get; set; }
        /// <summary>
        /// 断电次数
        /// </summary>
        public string DdCount { get; set; }
        /// <summary>
        /// 累计断电时间
        /// </summary>
        public string DdTime { get; set; }
        /// <summary>
        /// 馈电异常次数
        /// </summary>
        public string KdYcCount { get; set; }
        /// <summary>
        /// 累计馈电异常时间
        /// </summary>
        public string KdYcTime { get; set; }
        /// <summary>
        /// 故障次数
        /// </summary>
        public string GzCount { get; set; }
        /// <summary>
        /// 累计故障时间
        /// </summary>
        public string GzTime { get; set; }
    }
}
