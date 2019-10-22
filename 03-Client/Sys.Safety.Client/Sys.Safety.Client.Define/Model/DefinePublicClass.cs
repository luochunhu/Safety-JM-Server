using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.Define.Model
{
    public class DefinePublicClass
    {
        /// <summary>
        /// 验证特殊符号
        /// </summary>
        /// <returns></returns>
        public static bool ValidationSpecialSymbols(string text)
        {
            if (text.Contains(",") || text.Contains("\'") || text.Contains("\"")
                    || text.Contains("[") || text.Contains(".") || text.Contains("]")
                    || text.Contains(" ") || text.Contains("|") || text.Contains(";")
                    || text.Contains("+") || text.Contains("-") || text.Contains("~")
                    || text.Contains("‖") || text.Contains("@") || text.Contains(":")
                    || text.Contains("<") || text.Contains(">") || text.Contains("’"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
