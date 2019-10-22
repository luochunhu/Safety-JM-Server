using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 枚举扩展方法：获取枚举的key value对组，T只能为枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<KeyValuePair<int, string>> GetEnumKeyPairValue<T>(this object obj)
        {
            return EnumHelper.GetEnumKeyPairValue<T>();
        }

        /// <summary>
        /// 枚举扩展方法：根据枚举值获取枚举描述 enumValue为实际枚举的值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this object enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }
            return EnumHelper.GetEnumDescription(enumValue);
        }

        //枚举类型转换
        //eg. int val=(int)UserType.Teacher
        //eg. UserType userType=(UserType)1;
    }
}
