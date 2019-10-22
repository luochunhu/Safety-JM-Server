using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.BusinessModel
{
    /// <summary>
    /// 控制测点
    /// </summary>
    public class CrossControlItem
    {
        /// <summary>
        /// 测点Id
        /// </summary>
        public string PointId { get; set; }
        /// <summary>
        /// 控制类型
        /// </summary>
        public string ControlType { get; set; }
        /// <summary>
        /// 删除按钮
        /// </summary>
        public string DelInfBtnStr { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CrossControlItem()
        {
            PointId = string.Empty;
            ControlType = string.Empty;
            DelInfBtnStr = "删除";
        }
    }
    /// <summary>
    /// 解除控制测点
    /// </summary>
    public class  DeControlItem
    {
        /// <summary>
        /// 模型ID
        /// </summary>
        public string RemoveModelId { get; set; }
        /// <summary>
        /// 模型名称
        /// </summary>
        public string RemoveModelName { get; set; }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointId { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 删除按钮
        /// </summary>
        public string DelInfBtnStrFalse { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DeControlItem()
        {
            PointId = string.Empty;
            RemoveModelId = string.Empty;
            RemoveModelName = string.Empty;
            DelInfBtnStrFalse = "删除";
        }
    }
}
