using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public enum PresonTrajectoryFlag
    {
        /*
         * 0-正常采集，
         * 1-补传采集，
         * 2-人工编辑；
         * 3-正常入井，
         * 4-补传入井，
         * 5-人工编辑入井；
         * 6-正常出井，
         * 7-补传出井，
         * 8-人工编辑出井。
         */

        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        NomalData = 0,
        /// <summary>
        /// 补传采集
        /// </summary>
        [Description("补传采集")]
        DelayData = 1,
        /// <summary>
        /// 人工编辑
        /// </summary>
        [Description("人工编辑")]
        UserEditData = 2,
        /// <summary>
        /// 正常入井
        /// </summary>
        [Description("正常入井")]
        NomalInWellData = 3,
        /// <summary>
        /// 补传入井
        /// </summary>
        [Description("补传入井")]
        DelayInWellData = 4,
        /// <summary>
        /// 补传入井
        /// </summary>
        [Description("人工编辑入井")]
        UserInWellData = 5,
        /// <summary>
        /// 正常出井
        /// </summary>
        [Description("正常出井")]
        NomalOutWellData = 6,
        /// <summary>
        /// 补传出井
        /// </summary>
        [Description("补传出井")]
        DelayOutWellData = 7,
        /// <summary>
        /// 人工编辑出井
        /// </summary>
        [Description("人工编辑出井")]
        UserOutWellData = 8
    }
}
