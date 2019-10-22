using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Common
{
    /// <summary>
    /// 修改表达式局部逻辑的方式
    /// </summary>
    [Description("修改方式")]
    public enum ModifyWay
    {
        /// <summary>
        /// 点击取值或取消操作
        /// </summary>
        [Description("点击取值或取消操作")]
        None = 0,
        /// <summary>
        /// 左侧插入
        /// </summary>
        [Description("左侧插入")]
        Left = 1,
        /// <summary>
        /// 右侧插入
        /// </summary>
        [Description("右侧插入")]
        Right = 2,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Edit = 3,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 4
    }
}
