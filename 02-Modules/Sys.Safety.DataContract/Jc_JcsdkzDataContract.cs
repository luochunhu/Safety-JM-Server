using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class Jc_JcsdkzInfo
    {
        #region 重写Equals 和 操作符号
        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Jc_JcsdkzInfo temp = (Jc_JcsdkzInfo)obj;
            if (temp.ZkPoint != this.ZkPoint)
            {
                return false;
            }
            if (temp.Bkpoint != this.Bkpoint)
            {
                return false;
            }
            if (temp.Type != this.Type)
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
        public static bool operator ==(Jc_JcsdkzInfo m1, Jc_JcsdkzInfo m2)
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
        public static bool operator !=(Jc_JcsdkzInfo m1, Jc_JcsdkzInfo m2)
        {
            return !(m1 == m2);

        }
        #endregion
        ///<summary>
        /// 深度拷贝
        /// </summary>
        /// <returns></returns>
        public Jc_JcsdkzInfo Clone()
        {
            Jc_JcsdkzInfo CloneJcsdkz = Basic.Framework.Common.JSONHelper.ParseJSONString<Jc_JcsdkzInfo>(Basic.Framework.Common.JSONHelper.ToJSONString(this));
            //MemoryStream stream = new MemoryStream();
            //BinaryFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(stream, this);
            //stream.Position = 0;
            return CloneJcsdkz;
        }
    }
}
