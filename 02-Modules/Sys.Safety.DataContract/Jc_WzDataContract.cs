using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class Jc_WzInfo
    {
        #region 重写Equals 和 操作符号
        /// <summary>
        /// 重写Equals方法 实时值 状态 不做判断 NetID state / ID 不做判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Jc_WzInfo temp = (Jc_WzInfo)obj;
            if (temp.Wz != this.Wz)
            {
                return false;
            }
            if (temp.WzID != this.WzID)
            {
                return false;
            }
            if (temp.Upflag != this.Upflag)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 重写==运算符 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator ==(Jc_WzInfo m1, Jc_WzInfo m2)
        {
            if (null == (m1 as object)) { return null == (m2 as object); }
            return m1.Equals(m2);
        }
        /// <summary>
        /// 重写!=运算符
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator !=(Jc_WzInfo m1, Jc_WzInfo m2)
        {
            return !(m1 == m2);
        }
        #endregion
    }
}
