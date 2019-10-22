using Sys.Safety.DataContract.CommunicateExtend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 人员定位扩展属性  20171122
    /// </summary>
    public partial class Jc_DefInfo
    {
        /// <summary>
        /// 识别器类型名称
        /// </summary>
        protected string _RecognizerTypeDesc;
        /// <summary>
        /// 安装位置
        /// </summary>
        public string RecognizerTypeDesc
        {
            get { return _RecognizerTypeDesc; }
            set
            {
                _RecognizerTypeDesc = value;
            }
        }
        protected List<R_RestrictedpersonInfo> _RestrictedpersonInfoList;
        /// <summary>
        /// 禁止、限制进入人员Yid列表
        /// </summary>
        public List<R_RestrictedpersonInfo> RestrictedpersonInfoList
        {
            get { return _RestrictedpersonInfoList; }
            set
            {
                _RestrictedpersonInfoList = value;
            }
        }
    }
}
