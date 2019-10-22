using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class FiveMinData
    {
        /// <summary>
        /// 5分钟最大值
        /// </summary>
        public decimal m_nMaxVal;
        /// <summary>
        /// 5分钟最大值时间
        /// </summary>
        public DateTime m_nMaxValTime;
        /// <summary>
        /// 5分钟最小值
        /// </summary>
        public decimal m_nMinVal;
        /// <summary>
        /// 5分钟最小值时间
        /// </summary>
        public DateTime m_nMinValTime;
        /// <summary>
        /// 5分钟累计值
        /// </summary>
        public decimal m_nAllVal;
        /// <summary>
        /// 5分钟累计次数
        /// </summary>
        public ushort m_nAllCount;

        /// <summary>
        /// 最大值时的数据状态
        /// </summary>
        public int m_maxValueDataState;

        /// <summary>
        /// 最大值时的设备状态
        /// </summary>
        public int m_maxValueSBState;
    }
}
