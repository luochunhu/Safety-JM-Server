using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class AlarmProperty
    {
        private decimal m_nAlarmType;
        /// <summary>
        /// 报警类型
        /// </summary>
        public decimal NAlarmType
        {
            get { return m_nAlarmType; }
            set { m_nAlarmType = value; }
        }

        private DateTime m_dttHighAlarmTime;
        /// <summary>
        /// 最近一次上限报警时间
        /// </summary>
        public DateTime DttHighAlarmTime
        {
            get { return m_dttHighAlarmTime; }
            set { m_dttHighAlarmTime = value; }
        }

        private DateTime m_dttLowAlarmTime;
        /// <summary>
        /// 最近一次下限报警时间
        /// </summary>
        public DateTime DttLowAlarmTime
        {
            get { return m_dttLowAlarmTime; }
            set { m_dttLowAlarmTime = value; }
        }

        private DateTime m_dttHighPowerOffTime;
        /// <summary>
        /// 最近一次上限断电时间
        /// </summary>
        public DateTime DttHighPowerOffTime
        {
            get { return m_dttHighPowerOffTime; }
            set { m_dttHighPowerOffTime = value; }
        }
        private DateTime m_dttLowPowerOffTime;
        /// <summary>
        /// 最近一次下限断电时间
        /// </summary>
        public DateTime DttLowPowerOffTime
        {
            get { return m_dttLowPowerOffTime; }
            set { m_dttLowPowerOffTime = value; }
        }
        private decimal m_nAlarmMaxVal;
        /// <summary>
        /// 报警最大值
        /// </summary>
        public decimal NAlarmMaxVal
        {
            get { return m_nAlarmMaxVal; }
            set { m_nAlarmMaxVal = value; }
        }

        private decimal m_nAlarmMinVal;
        /// <summary>
        /// 报警最小值
        /// </summary>
        public decimal NAlarmMinVal
        {
            get { return m_nAlarmMinVal; }
            set { m_nAlarmMinVal = value; }
        }

        private DateTime m_dttAlarmMaxValTime;
        /// <summary>
        /// 报警最大值时间
        /// </summary>
        public DateTime DttAlarmMaxValTime
        {
            get { return m_dttAlarmMaxValTime; }
            set { m_dttAlarmMaxValTime = value; }
        }

        private DateTime m_dttAlarmMinValTime;
        /// <summary>
        /// 报警最小值时间
        /// </summary>
        public DateTime DttAlarmMinValTime
        {
            get { return m_dttAlarmMinValTime; }
            set { m_dttAlarmMinValTime = value; }
        }

        private decimal m_nPowerOffMaxVal;
        /// <summary>
        /// 断电最大值
        /// </summary>
        public decimal NPowerOffMaxVal
        {
            get { return m_nPowerOffMaxVal; }
            set { m_nPowerOffMaxVal = value; }
        }

        private decimal m_nPowerOffMinVal;
        /// <summary>
        /// 断电最小值
        /// </summary>
        public decimal NPowerOffMinVal
        {
            get { return m_nPowerOffMinVal; }
            set { m_nPowerOffMinVal = value; }
        }

        private DateTime m_dttPowerOffMaxValTime;
        /// <summary>
        /// 断电最大值时间
        /// </summary>
        public DateTime DttPowerOffMaxValTime
        {
            get { return m_dttPowerOffMaxValTime; }
            set { m_dttPowerOffMaxValTime = value; }
        }

        private DateTime m_dttPowerOffMinValTime;
        /// <summary>
        /// 断电最小值时间
        /// </summary>
        public DateTime DttPowerOffMinValTime
        {
            get { return m_dttPowerOffMinValTime; }
            set { m_dttPowerOffMinValTime = value; }
        }

        private decimal m_nAlarmAllVal;
        /// <summary>
        /// 报警期间累计值
        /// </summary>
        public decimal NAlarmAllVal
        {
            get { return m_nAlarmAllVal; }
            set { m_nAlarmAllVal = value; }
        }

        private decimal m_nPowerOffAllVal;
        /// <summary>
        /// 断电期间累计值
        /// </summary>
        public decimal NPowerOffAllVal
        {
            get { return m_nPowerOffAllVal; }
            set { m_nPowerOffAllVal = value; }
        }

        private decimal m_nAlarmAllCount;
        /// <summary>
        /// 报警期间累计次数
        /// </summary>
        public decimal NAlarmAllCount
        {
            get { return m_nAlarmAllCount; }
            set { m_nAlarmAllCount = value; }
        }

        private decimal m_nPowerOffAllCount;
        /// <summary>
        /// 断电期间累计次数
        /// </summary>
        public decimal NPowerOffAllCount
        {
            get { return m_nPowerOffAllCount; }
            set { m_nPowerOffAllCount = value; }
        }

        private decimal m_nAlarmNum;
        /// <summary>
        /// 当天累计报警次数
        /// </summary>
        public decimal NAlarmNum
        {
            get { return m_nAlarmNum; }
            set { m_nAlarmNum = value; }
        }

        private TimeSpan m_tsAlarmTimeSpan;
        /// <summary>
        /// 累计报警时间
        /// </summary>
        public TimeSpan TsAlarmTimeSpan
        {
            get { return m_tsAlarmTimeSpan; }
            set { m_tsAlarmTimeSpan = value; }
        }

        private decimal m_nPowerFailureNum;
        /// <summary>
        ///当天累计断电次数
        /// </summary>
        public decimal NPowerFailureNum
        {
            get { return m_nPowerFailureNum; }
            set { m_nPowerFailureNum = value; }
        }

        private TimeSpan m_tsPowerFailureTimeSpan;
        /// <summary>
        /// 累计断电时间
        /// </summary>
        public TimeSpan TsPowerFailureTimeSpan
        {
            get { return m_tsPowerFailureTimeSpan; }
            set { m_tsPowerFailureTimeSpan = value; }
        }

        private decimal m_nPowerFeedNum;
        /// <summary>
        ///当天累计馈电异常次数
        /// </summary>
        public decimal NPowerFeedNum
        {
            get { return m_nPowerFeedNum; }
            set { m_nPowerFeedNum = value; }
        }

        private TimeSpan m_tsPowerFeedTimeSpan;
        /// <summary>
        /// 累计馈电异常时间
        /// </summary>
        public TimeSpan TsPowerFeedTimeSpan
        {
            get { return m_tsPowerFeedTimeSpan; }
            set { m_tsPowerFeedTimeSpan = value; }
        }

        private long alarmid;
        /// <summary>
        /// 报警记录id
        /// </summary>
        public long AlarmID
        {
            get { return alarmid; }
            set { alarmid = value; }
        }

        private long dttalarmid;
        /// <summary>
        /// 断电记录id
        /// </summary>
        public long DttAlarmID
        {
            get { return dttalarmid; }
            set { dttalarmid = value; }
        }

        private decimal m_nbjMaxVal;
        /// <summary>
        /// 标校最大值
        /// </summary>
        public decimal NBJMaxVal
        {
            get { return m_nbjMaxVal; }
            set { m_nbjMaxVal = value; }
        }

        private decimal m_nbjMinVal;
        /// <summary>
        /// 标校最小值
        /// </summary>
        public decimal NBJMinVal
        {
            get { return m_nbjMinVal; }
            set { m_nbjMinVal = value; }
        }

        private DateTime m_dttbjMaxValTime;
        /// <summary>
        /// 标校最大值时间
        /// </summary>
        public DateTime DttBJMaxValTime
        {
            get { return m_dttbjMaxValTime; }
            set { m_dttbjMaxValTime = value; }
        }

        private DateTime m_dttbjMinValTime;
        /// <summary>
        /// 标校最小值时间
        /// </summary>
        public DateTime DttBJMinValTime
        {
            get { return m_dttbjMinValTime; }
            set { m_dttbjMinValTime = value; }
        }

        private decimal m_nbjNum;
        /// <summary>
        /// 当前标校数据次数
        /// </summary>
        public decimal NBJNum
        {
            get { return m_nbjNum; }
            set { m_nbjNum = value; }
        }

        private TimeSpan m_tsbjTimeSpan;
        /// <summary>
        /// 累计标校时间
        /// </summary>
        public TimeSpan TsBJTimeSpan
        {
            get { return m_tsbjTimeSpan; }
            set { m_tsbjTimeSpan = value; }
        }
    }
}
