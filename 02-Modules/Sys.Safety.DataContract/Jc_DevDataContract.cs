using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class Jc_DevInfo
    {
        protected string _DevProperty;

        protected string _DevClass;

        protected string _DevModel;

        /// <summary>
        /// 设备性质名称
        /// </summary>
        public string DevProperty
        {
            get { return _DevProperty; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevProperty", value, value.ToString());
                _DevProperty = value;
            }
        }

        /// <summary>
        /// 设备种类名称
        /// </summary>
        public string DevClass
        {
            get { return _DevClass; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevClass", value, value.ToString());
                _DevClass = value;
            }
        }

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DevModel
        {
            get { return _DevModel; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevModel", value, value.ToString());
                _DevModel = value;
            }
        }

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
            Jc_DevInfo temp = obj as Jc_DevInfo;
            //新增系统ID判断  20171123
            if (temp.Sysid != this.Sysid)
            {
                return false;
            }
            if (temp.Devid != this.Devid)
            {
                return false;
            }
            if (temp.Type != this.Type)
            {
                return false;
            }
            if (temp.Name != this.Name)
            {
                return false;
            }           
            if (temp.LC != this.LC)
            {
                return false;
            }
            if (temp.LC2 != this.LC2)
            {
                return false;
            }
            if (temp.Pl1 != this.Pl1)
            {
                return false;
            }
            if (temp.Pl2 != this.Pl2)
            {
                return false;
            }
            if (temp.Pl3 != this.Pl3)
            {
                return false;
            }
            if (temp.Pl4 != this.Pl4)
            {
                return false;
            }
            if (temp.Xzxs != this.Xzxs)
            {
                return false;
            }
            if (temp.Z1 != this.Z1)
            {
                return false;
            }
            if (temp.Z2 != this.Z2)
            {
                return false;
            }
            if (temp.Z3 != this.Z3)
            {
                return false;
            }
            if (temp.Z4 != this.Z4)
            {
                return false;
            }
            if (temp.Z5 != this.Z5)
            {
                return false;
            }
            if (temp.Z6 != this.Z6)
            {
                return false;
            }
            if (temp.Z7 != this.Z7)
            {
                return false;
            }
            if (temp.Z8 != this.Z8)
            {
                return false;
            }
            if (temp.Color1 != this.Color1)
            {
                return false;
            }
            if (temp.Color2 != this.Color2)
            {
                return false;
            }
            if (temp.Color3 != this.Color3)
            {
                return false;
            }
            if (temp.Xs1 != this.Xs1)
            {
                return false;
            }
            if (temp.Xs2 != this.Xs2)
            {
                return false;
            }
            if (temp.Xs3 != this.Xs3)
            {
                return false;
            }
            if (temp.Bz1 != this.Bz1)
            {
                return false;
            }
            if (temp.Bz2 != this.Bz2)
            {
                return false;
            }
            if (temp.Bz3 != this.Bz3)
            {
                return false;
            }
            if (temp.Bz4 != this.Bz4)
            {
                return false;
            }
            if (temp.Bz5 != this.Bz5)
            {
                return false;
            }
            if (temp.Bz6 != this.Bz6)
            {
                return false;
            }
            if (temp.Bz7 != this.Bz7)
            {
                return false;
            }
            if (temp.Bz8 != this.Bz8)
            {
                return false;
            }
            if (temp.Bz9 != this.Bz9)
            {
                return false;
            }
            if (temp.Bz10 != this.Bz10)
            {
                return false;
            }
            if (temp.Bz11 != this.Bz11)
            {
                return false;
            }
            if (temp.Bz12 != this.Bz12)
            {
                return false;
            }
            if (temp.Bz13 != this.Bz13)
            {
                return false;
            }
            if (temp.Bz14 != this.Bz14)
            {
                return false;
            }
            if (temp.Bz15 != this.Bz15)
            {
                return false;
            }
            if (temp.Remark != this.Remark)
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
        public static bool operator ==(Jc_DevInfo m1, Jc_DevInfo m2)
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
        public static bool operator !=(Jc_DevInfo m1, Jc_DevInfo m2)
        {
            return !(m1 == m2);
        }
        #endregion
    }
}
