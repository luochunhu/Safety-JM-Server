using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class AreaInfo : Basic.Framework.Web.BasicInfo
    {
        protected List<R_ArearestrictedpersonInfo> _RestrictedpersonInfoList;
        /// <summary>
        /// 禁止、限制进入人员Yid列表
        /// </summary>
        public List<R_ArearestrictedpersonInfo> RestrictedpersonInfoList
        {
            get { return _RestrictedpersonInfoList; }
            set
            {
                _RestrictedpersonInfoList = value;
            }
        }

        protected List<AreaRuleInfo> _AreaRuleInfoList;
        /// <summary>
        /// 区域内传感器定义限制信息
        /// </summary>
        public List<AreaRuleInfo> AreaRuleInfoList
        {
            get { return _AreaRuleInfoList; }
            set
            {
                _AreaRuleInfoList = value;
            }
        }
    }
}


