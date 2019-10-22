using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public class EnumHelper
    {
        /// <summary>
        /// 获取枚举的key value对组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<int, string>> GetEnumKeyPairValue<T>()
        {


            Type type = typeof(T);
            List<KeyValuePair<int, string>> val = new List<KeyValuePair<int, string>>();

            var fields = type.GetFields();
            if (fields != null && fields.Count() > 0)
            {
                for (var i = 1; i < fields.Count(); i++)
                {
                    var value = (int)fields[i].GetRawConstantValue();
                    var attr = fields[i].GetCustomAttributes(false).FirstOrDefault();
                    var desc = string.Empty;
                    if (attr != null)
                    {
                        DescriptionAttribute descAttr = attr as DescriptionAttribute;
                        desc = descAttr != null ? descAttr.Description : string.Empty;
                    }
                    val.Add(new KeyValuePair<int, string>(value, desc));
                }
            }
            return val;
        }

        /// <summary>
        /// 根据枚举值获取枚举描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object enumValue)
        {
            Type type = enumValue.GetType();
            var fields = type.GetFields();
            var desc = string.Empty;
            if (fields != null && fields.Count() > 0)
            {
                for (var i = 1; i < fields.Count(); i++)
                {
                    var value = (int)fields[i].GetRawConstantValue();
                    if (value == (int)enumValue)
                    {
                        var attr = fields[i].GetCustomAttributes(false).FirstOrDefault(a => a.GetType() == typeof(DescriptionAttribute));

                        if (attr != null)
                        {
                            DescriptionAttribute descAttr = attr as DescriptionAttribute;
                            desc = descAttr != null ? descAttr.Description : string.Empty;
                            break;
                        }
                    }
                }
            }
            return desc;
        }
    }
}
