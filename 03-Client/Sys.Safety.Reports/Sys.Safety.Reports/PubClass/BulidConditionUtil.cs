using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Safety.Reports
{
    public class BulidConditionUtil
    {
        /// <summary>
        /// 获取条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldType">字段类型</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetConditionString(string fieldName, string fieldType, string strCondition)
        {
            string strReturn = "";

            try
            {
                string strFieldType = fieldType.ToLower();
                if (strFieldType == "varchar" || strFieldType == "nvarchar" || strFieldType == "nchar" || strFieldType == "char")
                {
                    strReturn = GetStringCondition(fieldName, strCondition);
                }
                else if (strFieldType == "money" || strFieldType == "decimal" || strFieldType == "float"
                    || strFieldType == "int" || strFieldType == "smallint" || strFieldType == "bigint")
                {
                    strReturn = GetNumberCondition(fieldName, strCondition);
                }
                else if (strFieldType == "bit")
                {
                    strReturn = GetBooleanCondition(fieldName, strCondition);
                }
                else if (strFieldType == "smalldatetime" || strFieldType == "datetime")
                {
                    strReturn = GetDateTimeCondition(fieldName, strCondition);
                }
                else
                {
                    strReturn = GetStringCondition(fieldName, strCondition);
                }


                if (strReturn != string.Empty)
                {
                    strReturn = " (" + strReturn + " )";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        /// <summary>
        /// 获取参照条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetRefCondition(string fieldName, string strCondition, string fieldType)
        {
            string strReturn = "";
            try
            {
                string str = string.Empty;
                string[] strConditions = { };
                strConditions = strCondition.Split(new string[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);

                if (strConditions.Length > 0)
                {
                    string str1 = string.Empty;
                    for (int i = 0; i < strConditions.Length; i++)
                    {
                        if (!fieldType.Contains("int"))//如果不是int类型，说明没有建立外键关系，这个时候就是字符串，in查询的时候不需要去年引号
                            str1 = strConditions[i].Substring(5);
                        else
                            str1 = strConditions[i].Substring(5).Replace("'", "");
                        if (i == 0)
                        {
                            str = str1;
                        }
                        else
                        {
                            str += "," + str1;
                        }
                    }
                }

                strReturn = fieldName + " in (" + str + ")";


                if (strReturn != string.Empty)
                {
                    strReturn = " (" + strReturn + " )";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        /// <summary>
        /// 获取Boolean条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetBooleanCondition(string fieldName, string strCondition)
        {
            string strReturn = "";
            try
            {
                string[] strConditions = { };
                strConditions = strCondition.Split(new string[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);

                bool blnY = false;//存在是
                bool blnN = false;//存在否
                if (strConditions.Length > 0)
                {
                    string str1 = string.Empty;
                    for (int i = 0; i < strConditions.Length; i++)
                    {
                        str1 = strConditions[i];
                        if (str1 == "是")
                        {
                            blnY = true;
                        }
                        else if (str1 == "否")
                        {
                            blnN = true;
                        }
                        else
                        {
                            blnY = true;
                            blnN = true;
                        }
                    }
                }

                if (blnY && blnN)
                {
                    strReturn = "";
                }
                else if (blnY)
                {
                    strReturn = fieldName + "=1";
                }
                else if (blnN)
                {
                    strReturn = fieldName + "=0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        /// <summary>
        /// 获取数字条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetNumberCondition(string fieldName, string strCondition)
        {
            string strReturn = "";
            try
            {
                string[] strConditions = { };
                strConditions = strCondition.Split(new string[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);

                string strTemp = string.Empty;
                if (strConditions.Length > 0)
                {
                    string str = string.Empty;
                    string str0 = string.Empty;
                    string str1 = string.Empty;
                    string str2 = string.Empty;
                    for (int i = 0; i < strConditions.Length; i++)
                    {
                        str = strConditions[i];
                        if (str.Contains("&&$"))
                        {
                            string[] strs = str.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                            str0 = strs[0];
                            if (strs.Length > 1) str1 = strs[1];
                            if (strs.Length > 2) str2 = strs[2];
                        }
                        else
                        {
                            str0 = str;
                        }

                        strTemp = string.Empty;
                        switch (str0)
                        {
                            case "空值":
                                strTemp = " is null";
                                break;
                            case "等于":
                                strTemp = "={0}";
                                break;
                            case "大于":
                                strTemp = ">{0}";
                                break;
                            case "小于":
                                strTemp = "<{0}";
                                break;
                            case "不等于":
                                strTemp = "<>{0}";
                                break;
                            case "大于等于":
                                strTemp = ">={0}";
                                break;
                            case "小于等于":
                                strTemp = "<={0}";
                                break;
                            case "介于":
                                strTemp = " between {0} and {1}";
                                break;
                            default:
                                strTemp = "";
                                break;
                        }

                        string strFieldName = fieldName;
                        strFieldName = string.Format("isnull({0},0)", fieldName);
                        if (strTemp != string.Empty)
                        {
                            if (strReturn == string.Empty)
                            {
                                strReturn = strFieldName + string.Format(strTemp, str1, str2);
                            }
                            else
                            {
                                strReturn += " or " + strFieldName + string.Format(strTemp, str1, str2);
                            }
                        }
                    }

                    // 20170704
                    if (!string.IsNullOrEmpty(strReturn.Trim()))
                    {
                        strReturn = "(" + strReturn + ") and (" + fieldName + " REGEXP '(^[0-9]+.[0-9]+$)|(^[0-9]$)') =1";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        /// <summary>
        /// 获取时间条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetDateTimeCondition(string fieldName, string strCondition)
        {
            string strReturn = "";
            try
            {
                string[] strConditions = { };
                strConditions = strCondition.Split(new string[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);
                string strTemp = string.Empty;
                if (strConditions.Length > 0)
                {
                    string str = string.Empty;
                    string str0 = string.Empty;
                    string str1 = string.Empty;
                    string str2 = string.Empty;
                    bool _blnDynamicPara = false;
                    for (int i = 0; i < strConditions.Length; i++)
                    {
                        str = strConditions[i];
                        if (str.Contains("&&$"))
                        {
                            string[] strs = str.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                            str0 = strs[0];
                            if (strs.Length > 1) str1 = strs[1];
                            if (strs.Length > 2) str2 = strs[2];
                        }
                        else
                        {
                            str0 = str;
                        }
                        //2014-12-11 取消日期强制转换
                        //if (str1 != string.Empty && str1.Length > 10)
                        //{
                            //str1 = str1.Remove(10);
                        //}
                        //if (str2 != string.Empty && str2.Length > 10)
                        //{
                            //str2 = str2.Remove(10);
                        //}
                        strTemp = string.Empty;
                        _blnDynamicPara = false;
                        switch (str0)
                        {
                            case "空值":
                                strTemp = " is null";
                                break;
                            case "等于":
                                strTemp = "='{0}'";
                                break;
                            case "大于":
                                strTemp = ">'{0}'";
                                break;
                            case "小于":
                                strTemp = "<'{0}'";
                                break;
                            case "不等于":
                                strTemp = "<>'{0}'";
                                break;
                            case "大于等于":
                                strTemp = ">='{0}'";
                                break;
                            case "小于等于":
                                strTemp = "<='{0}'";
                                break;
                            case "介于":
                                strTemp = " between '{0}' and '{1}'";
                                break;
                            case "开始于":
                                strTemp = " like '{0}%'";
                                break;
                            case "包含":
                                strTemp = " like '%{0}%'";
                                break;
                            case "今天":
                                strTemp = "  ${Today} ";
                                _blnDynamicPara = true;
                                break;
                            case "本周":
                                strTemp = "  ${ThisWeek} ";
                                _blnDynamicPara = true;
                                break;
                            case "本周至今日":
                                strTemp = "  ${ThisWeekToToday} ";
                                _blnDynamicPara = true;
                                break;
                            case "本月":
                                strTemp = "  ${ThisMonth} ";
                                _blnDynamicPara = true;
                                break;
                            case "本月至今日":
                                strTemp = "  ${ThisMonthToToday} ";
                                _blnDynamicPara = true;
                                break;
                            case "本季度":
                                strTemp = "  ${ThisSeason} ";
                                _blnDynamicPara = true;
                                break;
                            case "本季度至今日":
                                strTemp = "  ${ThisSeasonToToday} ";
                                _blnDynamicPara = true;
                                break;
                            case "本年":
                                strTemp = "  ${ThisYear} ";
                                _blnDynamicPara = true;
                                break;
                            case "本年至今日":
                                strTemp = "  ${ThisYearToToday} ";
                                _blnDynamicPara = true;
                                break;
                            case "上周":
                                strTemp = "  ${LastWeek} ";
                                _blnDynamicPara = true;
                                break;
                            case "上月":
                                strTemp = "  ${LastMonth} ";
                                _blnDynamicPara = true;
                                break;
                            case "上季度":
                                strTemp = "  ${LastSeason} ";
                                _blnDynamicPara = true;
                                break;
                            case "上年":
                                strTemp = "  ${LastYear} ";
                                _blnDynamicPara = true;
                                break;
                            case "最近一周":
                                strTemp = "  ${LastestWeek} ";
                                _blnDynamicPara = true;
                                break;
                            case "最近一月":
                                strTemp = "  ${LastestMonth} ";
                                _blnDynamicPara = true;
                                break;
                            case "最近一季度":
                                strTemp = "  ${LastestSeason} ";
                                _blnDynamicPara = true;
                                break;
                            case "最近一年":
                                strTemp = "  ${LastestYear} ";
                                _blnDynamicPara = true;
                                break;
                            default:
                                strTemp = "";
                                break;
                        }

                        string strFieldName = fieldName;
                        if (!_blnDynamicPara)
                        {
                            strTemp = string.Format(strTemp, str1, str2);
                            //2014-12-11 取消日期强制转换
                            //strFieldName = string.Format("convert(varchar(10),isnull({0},'1900-01-01'),120)", fieldName);

                        }
                        else
                        {
                            strFieldName = string.Format("isnull({0},'1900-01-01 00:00:00')", fieldName);
                        }
                        if (strTemp != string.Empty)
                        {
                            if (strReturn == string.Empty)
                            {
                                strReturn = strFieldName + strTemp;
                            }
                            else
                            {
                                strReturn += " or " + strFieldName + strTemp;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }

        /// <summary>
        /// 获取字符串条件串
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="strCondition">条件串</param>
        /// <returns>string</returns>
        public static string GetStringCondition(string fieldName, string strCondition)
        {
            string strReturn = "";
            try
            {
                string[] strConditions = { };
                strConditions = strCondition.Split(new string[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);

                string strTemp = string.Empty;
                if (strConditions.Length > 0)
                {
                    string str = string.Empty;
                    string str0 = string.Empty;
                    string str1 = string.Empty;
                    string str2 = string.Empty;
                    for (int i = 0; i < strConditions.Length; i++)
                    {
                        str = strConditions[i];
                        if (str.Contains("&&$"))
                        {
                            string[] strs = str.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                            str0 = strs[0];
                            if (strs.Length > 1) str1 = strs[1];
                            if (strs.Length > 2) str2 = strs[2];
                        }
                        else
                        {
                            str0 = str;
                        }

                        strTemp = string.Empty;
                        switch (str0)
                        {
                            case "空值":
                                strTemp = " is null";
                                break;
                            case "等于":
                                strTemp = "='{0}'";
                                break;
                            case "大于":
                                strTemp = ">'{0}'";
                                break;
                            case "小于":
                                strTemp = "<'{0}'";
                                break;
                            case "不等于":
                                strTemp = "<>'{0}'";
                                break;
                            case "大于等于":
                                strTemp = ">='{0}'";
                                break;
                            case "小于等于":
                                strTemp = "<='{0}'";
                                break;
                            case "介于":
                                strTemp = " between '{0}' and '{1}'";
                                break;
                            case "开始于":
                                strTemp = " like '{0}%'";
                                break;
                            case "包含":
                                strTemp = " like '%{0}%'";
                                break;
                            case "不包含":
                                strTemp = " not like '%{0}%'";
                                break;
                            default:
                                strTemp = "";
                                break;
                        }

                        string strFieldName = fieldName;
                        strFieldName = string.Format("isnull({0},'')", fieldName);
                        if (strTemp != string.Empty)
                        {
                            if (strReturn == string.Empty)
                            {
                                strReturn = strFieldName + string.Format(strTemp, str1, str2);
                            }
                            else
                            {
                                strReturn += " or " + strFieldName + string.Format(strTemp, str1, str2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return strReturn;
        }
    }
}
