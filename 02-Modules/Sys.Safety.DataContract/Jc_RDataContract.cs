using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class Jc_RInfo
    {
        protected long _Counter;
        /// <summary>
        /// 自增计数器
        /// </summary>
        public virtual long Counter
        {
            get { return _Counter; }
            set
            {
                _Counter = value;
            }
        }
    }
}
