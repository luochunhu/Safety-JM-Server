using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class FactorValueInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// 当数据获取出现异常时，返回"未知"
        /// </summary>
        private string _Value="未知";

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
