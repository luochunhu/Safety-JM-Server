using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Sys.Safety.Interface;

namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 通讯驱动对象
    /// </summary>
    [Serializable, DataContract]
    public class DriverItem
    {
        /// <summary>
        /// 驱动描述(如:大分站驱动)
        /// </summary>
        private string m_strDriverName;
        /// <summary>
        /// 驱动描述(如:大分站驱动)
        /// </summary>
        public string StrDriverName
        {
            get { return m_strDriverName; }
            set { m_strDriverName = value; }
        }
        /// <summary>
        /// 驱动源文件
        /// 驱动文件的名称,路径为Driver文件夹下
        /// 如:xxx.dll
        /// </summary>
        private string m_strDriverSource;
        /// <summary>
        /// 驱动源文件
        /// 驱动文件的名称,路径为Driver文件夹下
        /// 如:xxx.dll
        /// </summary>
        public string StrDriverSource
        {
            get { return m_strDriverSource; }
            set { m_strDriverSource = value; }
        }
        /// <summary>
        /// 驱动版本号
        /// </summary>
        private string m_strDriverVersion;
        /// <summary>
        /// 驱动版本号
        /// </summary>
        public string StrDriverVersion
        {
            get { return m_strDriverVersion; }
            set { m_strDriverVersion = value; }
        }
        /// <summary>
        /// 驱动索引ID
        /// </summary>
        private decimal m_nDriverID;
        /// <summary>
        /// 驱动索引ID
        /// </summary>
        public decimal NDriverID
        {
            get { return m_nDriverID; }
            set { m_nDriverID = value; }
        }
        /// <summary>
        /// 驱动更新时间
        /// </summary>
        private DateTime m_dttDriverVersionTime;
        /// <summary>
        /// 驱动更新时间
        /// </summary>
        public DateTime DttDriverVersionTime
        {
            get { return m_dttDriverVersionTime; }
            set { m_dttDriverVersionTime = value; }
        }
        /// <summary>
        /// 驱动所属分组
        /// </summary>
        private string m_strDriverGroup;
        /// <summary>
        /// 驱动所属分组
        /// </summary>
        public string StrDriverGroup
        {
            get { return m_strDriverGroup; }
            set { m_strDriverGroup = value; }
        }
        /// <summary>
        /// 驱动支持的模拟量通道个数
        /// </summary>
        private decimal m_nAnalogNum;
        /// <summary>
        /// 驱动支持的模拟量通道个数
        /// </summary>
        public decimal NAnalogNum
        {
            get { return m_nAnalogNum; }
            set { m_nAnalogNum = value; }
        }
        /// <summary>
        /// 驱动支持的开关量通道个数
        /// </summary>
        private decimal m_nDerailNum;
        /// <summary>
        /// 驱动支持的开关量通道个数
        /// </summary>
        public decimal NDerailNum
        {
            get { return m_nDerailNum; }
            set { m_nDerailNum = value; }
        }
        /// <summary>
        /// 驱动支持的控制量通道个数
        /// </summary>
        private decimal m_nControlNum;
        /// <summary>
        /// 驱动支持的控制量通道个数
        /// </summary>
        public decimal NControlNum
        {
            get { return m_nControlNum; }
            set { m_nControlNum = value; }
        }
        /// <summary>
        /// 驱动支持的控制量起始号
        /// </summary>
        private decimal m_nControlNumStart;
        /// <summary>
        /// 驱动支持的控制量起始号
        /// </summary>
        public decimal NControlNumStart
        {
            get { return m_nControlNumStart; }
            set { m_nControlNumStart = value; }
        }
        /// <summary>
        /// 驱动支持的累积量通道个数
        /// </summary>
        private decimal m_AccumulationNum;
        /// <summary>
        /// 驱动支持的累积量通道个数
        /// </summary>
        public decimal AccumulationNum
        {
            get { return m_AccumulationNum; }
            set { m_AccumulationNum = value; }
        }
        /// <summary>
        /// 驱动支持的人员个数
        /// </summary>
        private decimal m_RecognitionNum;
        /// <summary>
        /// 驱动支持的人员个数
        /// </summary>
        public decimal RecognitionNum
        {
            get { return m_RecognitionNum; }
            set { m_RecognitionNum = value; }
        }
        /// <summary>
        /// 通讯取数命令长度
        /// </summary>
        private uint m_nDriverCommFLen;
        /// <summary>
        /// 通讯取数命令长度
        /// </summary>
        public uint NDriverCommFLen
        {
            get { return m_nDriverCommFLen; }
            set { m_nDriverCommFLen = value; }
        }
        /// <summary>
        /// 反射驱动后的实例对象
        /// </summary>
        private IDriver m_DLLObj;
        /// <summary>
        /// 反射驱动后的实例对象
        /// </summary>
        public IDriver DLLObj
        {
            get { return m_DLLObj; }
            set { m_DLLObj = value; }
        }
        /// <summary>
        /// 驱动协议类型
        /// </summary>
        private byte m_comtype;
        /// <summary>
        /// 驱动协议类型
        /// </summary>
        public byte Comtype
        {
            get { return m_comtype; }
            set { m_comtype = value; }
        }
        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <returns></returns>
        public DriverItem Clone()
        {
            DriverItem CloneDriverItem = Basic.Framework.Common.JSONHelper.ParseJSONString<DriverItem>(Basic.Framework.Common.JSONHelper.ToJSONString(this));
            //MemoryStream stream = new MemoryStream();
            //BinaryFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(stream, this);
            //stream.Position = 0;
            return CloneDriverItem;
        }
    }
}
