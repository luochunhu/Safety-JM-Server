using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Common
{
    /// <summary>
    /// 表达式格式处理操作类
    /// </summary>
    public class ExpressionResolution
    {
        /// <summary>
        /// 格式化表达式
        /// </summary>
        /// <param name="expressionData">表达式，如：(N1<参数1→因子或N2<参数2→因子)与参数2→因子<N3</param>
        /// <returns>返回表达式格式化的集合</returns>
        public static List<string> ExpressionToList(string expressionData)
        {
            List<string> listExpression = new List<string>();
            int start = 0;
            if (string.IsNullOrWhiteSpace(expressionData))
            {
                return listExpression;
            }
            for (int i = 0; i < expressionData.Length; i++)
            {
                if (expressionData[i] == '('
                    || expressionData[i] == ')'
                    || expressionData[i] == '或'
                    || expressionData[i] == '与')
                {
                    if (start == 0 && i == 0)
                    {
                        listExpression.Add(expressionData.Substring(start, i + 1));
                        start = i + 1;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(expressionData.Substring(start, i - start)))
                        {
                            listExpression.Add(expressionData.Substring(start, i - start));
                        }
                        listExpression.Add(expressionData[i].ToString());
                        start = i + 1;
                    }

                }
                else
                {
                    if (i == expressionData.Length - 1)
                    {
                        listExpression.Add(expressionData.Substring(start, i + 1 - start));
                        start = i;
                    }
                }
            }

            return listExpression;
        }

        /// <summary>
        /// 表达式集合转换成string
        /// </summary>
        /// <param name="expressionList">返回表达式格式化的集合</param>
        /// <returns>表达式，如：(N1<参数1→因子或N2<参数2→因子)与参数2→因子<N3</returns>
        public static string ListToExpression(List<string> expressionList)
        {
            StringBuilder sbExpression = new StringBuilder();
            if (expressionList == null || expressionList.Count == 0)
            {
                return "";
            }

            foreach (var item in expressionList)
            {
                sbExpression.Append(item);
            }

            return sbExpression.ToString();
        }

        /// <summary>
        /// 返回listbox每行的操作序号集合
        /// </summary>
        /// <param name="operationRecordList">操作记录list</param>
        /// <param name="listBox">listBox记录集合</param>
        /// <returns></returns>
        public static List<List<int>> ListToLsbExpression(List<string> operationRecordList, List<string> listBox)
        {
            List<List<int>> listBoxoperationRecordlist = new List<List<int>>();
            int recordInt = 0;

            if (listBox == null || listBox.Count == 0)
            {
                return listBoxoperationRecordlist;
            }

            foreach (var item in listBox)
            {
                List<int> listInt = new List<int>();
                StringBuilder itemRecord = new StringBuilder();
                for (int i = recordInt; i < operationRecordList.Count; i++)
                {
                    itemRecord.Append(operationRecordList[i]);
                    listInt.Add(i);
                    if (itemRecord.ToString() == item)
                    {
                        recordInt = i + 1;
                        listBoxoperationRecordlist.Add(listInt);
                        break;
                    }
                }
            }
            return listBoxoperationRecordlist;
        }

        /// <summary>
        /// 模拟C#表达式值
        /// </summary>
        /// <param name="expressionList">表达式解析列表(操作表达式集合)</param>
        /// <param name="paramterList">参数列表(操作表达式集合)</param>
        /// <returns></returns>
        public static string SimulationExpreesion(List<string> expressionList, List<string> paramterList)
        {
            StringBuilder sbReturn = new StringBuilder();
            for (int i = 0; i < expressionList.Count; i++)
            {
                if (paramterList.Contains(expressionList[i].ToString()))
                // == "设备1"
                //|| expressionList[i].ToString() == "设备2"
                //   || expressionList[i].ToString() == "设备3"
                //   || expressionList[i].ToString() == "设备4"
                //   || expressionList[i].ToString() == "设备5")
                {
                    sbReturn.Append("5");
                }
                else if (expressionList[i].ToString().Contains("->"))
                {

                }
                else
                {
                    switch (expressionList[i].ToString())
                    {
                        case "或":
                            sbReturn.Append("||");
                            break;
                        case "与":
                            sbReturn.Append("&&");
                            break;
                        case "=":
                            sbReturn.Append("==");
                            break;
                        case "≤":
                            sbReturn.Append("<=");
                            break;
                        case "≥":
                            sbReturn.Append(">=");
                            break;
                        case "≠":
                            sbReturn.Append("!=");
                            break;
                        case "÷":
                            sbReturn.Append("/");
                            break;
                        case "×":
                            sbReturn.Append("*");
                            break;
                        case "0态":
                            sbReturn.Append("0");
                            break;
                        case "1态":
                            sbReturn.Append("1");
                            break;
                        case "2态":
                            sbReturn.Append("2");
                            break;
                        default:
                            sbReturn.Append(expressionList[i].ToString());
                            break;
                    }
                }
            }

            return sbReturn.ToString();
        }
        /// <summary>
        /// 模拟Lua表达式值
        /// </summary>
        /// <param name="expressionList">表达式解析列表(操作表达式集合)</param>
        /// <returns></returns>
        public static string SimulationLuaExpreesion(List<string> expressionList)
        {
            StringBuilder sbReturn = new StringBuilder();
            for (int i = 0; i < expressionList.Count; i++)
            {
                switch (expressionList[i].ToString())
                {
                    case "或":
                        sbReturn.Append(" or ");
                        break;
                    case "与":
                        sbReturn.Append(" and ");
                        break;
                    case "=":
                        sbReturn.Append(" == ");
                        break;
                    case "<":
                        if (expressionList[i].ToString().Contains("->"))
                        {
                            sbReturn.Append(expressionList[i].ToString());
                        }
                        else
                        {
                            sbReturn.Append(" < ");
                        }

                        break;
                    case "≤":
                        sbReturn.Append(" <= ");
                        break;
                    case ">":
                        sbReturn.Append(" > ");
                        break;
                    case "≥":
                        sbReturn.Append(" >= ");
                        break;
                    case "≠":
                        sbReturn.Append(" ~= ");
                        break;
                    case "+":
                        sbReturn.Append(" + ");
                        break;
                    case "-":
                        if (expressionList[i].ToString().Contains("->"))
                        {
                            sbReturn.Append(expressionList[i].ToString());
                        }
                        else
                        {
                            sbReturn.Append(" - ");
                        }
                        break;
                    case "×":
                        sbReturn.Append(" * ");
                        break;
                    case "÷":
                        sbReturn.Append(" / ");
                        break;
                    case "0态":
                        sbReturn.Append(" 0 ");
                        break;
                    case "1态":
                        sbReturn.Append(" 1 ");
                        break;
                    case "2态":
                        sbReturn.Append(" 2 ");
                        break;
                    default:
                        sbReturn.Append(expressionList[i].ToString());
                        break;
                }
            }

            return sbReturn.ToString();
        }

        /// <summary>
        /// 根据模拟表达式值来验证表达式的正确性
        /// </summary>
        ///  <param name="expressionList">表达式解析列表(操作表达式集合)</param>
        ///   <param name="paramterList">参数列表(操作表达式集合)</param>
        /// <returns></returns>
        public static bool CheckExpreesion(List<string> expressionList, List<string> paramterList)
        {
            try
            {
                //模拟表达式值
                string simulationExpreesion = SimulationExpreesion(expressionList, paramterList);

                CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

                // 2.ICodeComplier
                ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

                // 3.CompilerParameters
                CompilerParameters objCompilerParameters = new CompilerParameters();
                objCompilerParameters.ReferencedAssemblies.Add("System.dll");
                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;



                // 4.CompilerResults
                CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, GenerateCode(simulationExpreesion));

                if (cr.Errors.HasErrors)
                {
                    return false;
                }
                else
                {
                    // 通过反射，调用HelloWorld的实例
                    Assembly objAssembly = cr.CompiledAssembly;
                    object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.ValidateExpression");
                    MethodInfo objMI = objHelloWorld.GetType().GetMethod("DoValidate");

                    objMI.Invoke(objHelloWorld, null);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// check 表达式字符串
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GenerateCode(string expression)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System;");
            sb.Append(Environment.NewLine);
            sb.Append("namespace DynamicCodeGenerate");
            sb.Append(Environment.NewLine);
            sb.Append("{");
            sb.Append(Environment.NewLine);
            sb.Append("      public class ValidateExpression");
            sb.Append(Environment.NewLine);
            sb.Append("      {");
            sb.Append(Environment.NewLine);
            sb.Append("          public object DoValidate()");
            sb.Append(Environment.NewLine);
            sb.Append("          {");
            sb.Append(Environment.NewLine);
            sb.Append(string.Format("               return {0};", expression));
            sb.Append(Environment.NewLine);
            sb.Append("          }");
            sb.Append(Environment.NewLine);
            sb.Append("      }");
            sb.Append(Environment.NewLine);
            sb.Append("}");

            string code = sb.ToString();

            return code;
        }

        /// <summary>
        /// 通过表达式解析参数因子列表
        /// </summary>
        /// <param name="expression">表达式字符串</param>
        /// <param name="paramterList">参数列表</param>
        /// <param name="factorList">因子列表</param>
        /// <returns></returns>
        public static List<ParamterFactor> GetParamterFactorForExpression(string expression, List<string> paramterList, List<string> factorList)
        {
            List<ParamterFactor> paramterFactorList = new List<ParamterFactor>();
            if (expression.IndexOf("->") < 0)
            {
                return paramterFactorList;
            }
            string[] str = expression.Split(new[] { "->" }, StringSplitOptions.None);
            ParamterFactor paramterFactor = new ParamterFactor();
            for (int i = 0; i < str.Length; i++)
            {

                if (i == 0)
                {
                    for (int j = 0; j < paramterList.Count; j++)
                    {
                        if (str[i].ToString().Contains(paramterList[j]))
                        {
                            paramterFactor.ParamterName = paramterList[j];
                            break;
                        }
                    }

                }
                else
                {
                    for (int k = 0; k < factorList.Count; k++)
                    {
                        if (str[i].ToString().Contains(factorList[k]))
                        {
                            paramterFactor.FactorName = factorList[k];
                            paramterFactorList.Add(paramterFactor);
                            paramterFactor = new ParamterFactor();
                            if (i != str.Length - 1)
                            {
                                for (int j = 0; j < paramterList.Count; j++)
                                {
                                    if (str[i].ToString().Contains(paramterList[j]))
                                    {
                                        paramterFactor.ParamterName = paramterList[j];
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            return paramterFactorList;
        }

        /// <summary>
        /// 获取表达式中最后一个因子的名称
        /// </summary>
        /// <param name="operationRecordList">操作记录list</param>
        /// <returns>因子名称 ，如：开关量实时值</returns>
        public static string GetEndFactorNameByOperationRecordList(List<string> operationRecordList)
        {
            string strEndFactorName = "";
            for (int i = operationRecordList.Count - 1; i >= 0; i--)
            {
                if (operationRecordList[i].Contains("->"))
                {
                    strEndFactorName = operationRecordList[i].Replace("->", "");
                    break;
                }
            }

            return strEndFactorName;
        }

        /// <summary>
        /// 验证表达式中只包含 参数 或者 参数->因子
        /// </summary>
        /// <param name="operationRecordList">操作记录list</param>
        /// <param name="signList">运算符</param>
        /// <returns>true  or  false</returns>
        public static bool GetEndFactorNameByOperationRecordList(List<string> operationRecordList, List<string> paramterList, List<string> signList)
        {
            bool reEndFactorName = false;
            if (operationRecordList.Count <= 2)
            {
                reEndFactorName = true;
            }
            else
            {
                for (int i = 0; i < operationRecordList.Count; i++)
                {
                    if (operationRecordList[i].Contains("->"))
                    {
                        if (i + 1 >= operationRecordList.Count)
                        {//最后一个操作是因子
                            if (i >= 3)
                                if (!signList.Contains(operationRecordList[i - 2].ToString().Trim()))
                                {
                                    reEndFactorName = true;
                                    break;
                                }

                        }
                        else
                        {
                            try
                            {
                                if (operationRecordList[i + 1].ToString().Trim() == ")" ||
                                   operationRecordList[i + 1].ToString().Trim() == "或"
                                   || operationRecordList[i + 1].ToString().Trim() == "与")
                                {//判断前运算符
                                    if (!signList.Contains(operationRecordList[i - 2].ToString().Trim()))
                                    {
                                        reEndFactorName = true;
                                        break;
                                    }
                                }
                                if (operationRecordList[i -2].ToString().Trim() == "(" ||
                                    operationRecordList[i - 2].ToString().Trim() == "或"
                                    || operationRecordList[i - 2].ToString().Trim() == "与")
                                {//判断后运算符
                                    if (!signList.Contains(operationRecordList[i + 1].ToString().Trim()))
                                    {
                                        reEndFactorName = true;
                                        break;
                                    }
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                    else
                    {
                        if (paramterList.Contains(operationRecordList[i].ToString().Trim()))
                        {//参数与因子配对
                            if (i + 1 >= operationRecordList.Count)
                            {//最后一个是参数
                                reEndFactorName = true;
                                break;
                            }
                            else
                            {
                                if (!operationRecordList[i + 1].Contains("->"))
                                {
                                    reEndFactorName = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return reEndFactorName;
        }
    }
    /// <summary>
    /// 参数因子
    /// </summary>
    public class ParamterFactor
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamterName { get; set; }
        /// <summary>
        /// 因子名称
        /// </summary>
        public string FactorName { get; set; }
    }
}
