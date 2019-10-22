using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports
{
    public class RequestUtil
    {

        private static IDictionary<string, object> dicCache = new Dictionary<string, object>();
        public static string GetParameterValue(string strKey)
        {
            if (strKey.ToLower() == "userid")
                return "10000001";
            //IDictionary<string,string> dic = ClientContext.Current.GetContext("CustomerSetting") as Dictionary<string,string>;
            IDictionary<string, string> dic =
                Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, string>>(Basic.Framework.Data.PlatRuntime.Items["CustomerSetting"].ToString());

            if (dic == null) return "";
            if(dic.ContainsKey(strKey))
                return dic[strKey];
            return "";
         
        }

      
      

        public static string ProcessDynamicParameters(string strsql)
        {
         
            string s = ProcessDynamicParameters(strsql, null); 
            return s;
        }

        /// <summary>
        /// 处理动态参数
        /// </summary>
        /// <param name="paramString">参数串,动态参数以${}区别</param>
        /// <param name="dynamicParameters">参考的动态串</param>
        /// <returns></returns>
        public static string ProcessDynamicParameters(string paramString, IDictionary dynamicParameters)
        {
            string result;
            result = paramString;
            if (string.IsNullOrEmpty(paramString))
            {
                return result;
            }
            IDictionary context = GetDynamicParametersContext(dynamicParameters);
            IEnumerator ie = context.Keys.GetEnumerator();
            while (ie.MoveNext())
            {
                string key = ie.Current.ToString();
                string value = TypeUtil.ToString(context[key]);
                result = result.Replace("${" + key + "}", value);
            }
            return result;
        }
        /// <summary>
        /// 获取动态参数上下文
        /// </summary>
        /// <param name="dynamicParameters"></param>
        /// <returns></returns>
        public static IDictionary GetDynamicParametersContext(IDictionary dynamicParameters)
        {
            IDictionary result = dynamicParameters;
            if (result == null)
            {
                result = new Hashtable();
            }
            IDictionary context = ClientCacheModel.GetClientParameters();
            if (context != null && context.Count > 0)
            {
                IEnumerator contextIe = context.Keys.GetEnumerator();
                while (contextIe.MoveNext())
                {
                    string key = contextIe.Current.ToString();
                    if (result.Contains(key))
                    {
                        result[key] = TypeUtil.ToString(context[key]);
                    }
                    else
                    {
                        result.Add(key, TypeUtil.ToString(context[key]));
                    }
                }
            }
            return result;
        }
    }
}
