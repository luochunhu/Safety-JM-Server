using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.ClientFramework.CBFCommon;
using System.Linq;
using Sys.Safety.Enums;



namespace Sys.Safety.Client.Display
{
    public class OprFuction
    {
        /// <summary>
        /// 将内存实时显示配置存储到本地
        /// </summary>
        /// <returns></returns>
        public static bool SaveRealDataDisplayConfig()
        {
            #region 存储基础配置
            StaticClass.RealDataDisplayCnfgDoc = new XmlConfig(StaticClass.RealDataCnfg);
            try
            {
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.GvBackColor", StaticClass.realdataconfig.BaseCfg.GvBackColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.TableHadeBackColor", StaticClass.realdataconfig.BaseCfg.TableHadeBackColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.Showgrid", StaticClass.realdataconfig.BaseCfg.Showgrid ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.TableHadeHigh", StaticClass.realdataconfig.BaseCfg.TableHadeHigh.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.DataRowHigh", StaticClass.realdataconfig.BaseCfg.DataRowHigh.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.DoubleRowColor", StaticClass.realdataconfig.BaseCfg.DoubleRowColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.SingleRowColor", StaticClass.realdataconfig.BaseCfg.SingleRowColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.GridColor", StaticClass.realdataconfig.BaseCfg.GridColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.PageChangeInterval", StaticClass.realdataconfig.BaseCfg.PageChangeInterval.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.SelectColor", StaticClass.realdataconfig.BaseCfg.SelectColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.SplitColor", StaticClass.realdataconfig.BaseCfg.SplitColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.SplitWidth", StaticClass.realdataconfig.BaseCfg.SplitWidth.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.Colorchange", StaticClass.realdataconfig.BaseCfg.Colorchange);
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.Showju", StaticClass.realdataconfig.BaseCfg.Showju ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.BJfontsize", StaticClass.realdataconfig.BaseCfg.Bjfontsize.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("存储基础配置SaveRealDataDisplayConfig.BaseCfg", ex);
            }
            #endregion

            #region 存储字体配置
            try
            {
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataFontName", StaticClass.realdataconfig.FontCfg.DataFontName);
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.TableHadeFontSize", StaticClass.realdataconfig.FontCfg.TableHadeFontSize.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.TableHadeFontColor", StaticClass.realdataconfig.FontCfg.TableHadeFontColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.TableHadeFontName", StaticClass.realdataconfig.FontCfg.TableHadeFontName);
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.IsBold", StaticClass.realdataconfig.FontCfg.IsBold ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.IsHaveUnderLine", StaticClass.realdataconfig.FontCfg.IsHaveUnderLine ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.IsItalic", StaticClass.realdataconfig.FontCfg.IsItalic ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataFontColor", StaticClass.realdataconfig.FontCfg.DataFontColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataFontSize", StaticClass.realdataconfig.FontCfg.DataFontSize.ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataIsBold", StaticClass.realdataconfig.FontCfg.DataIsBold ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataIsHaveUnderLine", StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("FontCfg.DataIsItalic", StaticClass.realdataconfig.FontCfg.DataIsItalic ? "1" : "0");
            }
            catch (Exception ex)
            {
                LogHelper.Error("存储字体配置SaveRealDataDisplayConfig.FontCfg", ex);
            }
            #endregion

            #region 存储各种状态颜色配置
            try
            {
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.DefaultColor", StaticClass.realdataconfig.StateCorCfg.DefaultColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.InterruptionColor", StaticClass.realdataconfig.StateCorCfg.InterruptionColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.KAlarmColor", StaticClass.realdataconfig.StateCorCfg.KAlarmColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.KBlackOutColor", StaticClass.realdataconfig.StateCorCfg.KBlackOutColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.LowAlarmColor", StaticClass.realdataconfig.StateCorCfg.LowAlarmColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.LowBlackOutColor", StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.LowPreAlarmColor", StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.UpAlarmColor", StaticClass.realdataconfig.StateCorCfg.UpAlarmColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.UpBlackOutColor", StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.UpPreAlarmColor", StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.OverRangeColor", StaticClass.realdataconfig.StateCorCfg.OverRangeColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.DcColor", StaticClass.realdataconfig.StateCorCfg.DcColor.ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("StateCorCfg.EffectColor", StaticClass.realdataconfig.StateCorCfg.EffectColor.ToArgb().ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("存储各种状态颜色配置SaveRealDataDisplayConfig.StateCorCfg", ex);
            }
            #endregion

            #region 存储显示数据列配置
            try
            {
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("DataClnCfg.ShowUnit", StaticClass.realdataconfig.DataClnCfg.ShowUnit ? "1" : "0");
                for (int i = 0; i < StaticClass.realdataconfig.DataClnCfg.ColumnsMsg.Length; i++)
                {
                    StaticClass.RealDataDisplayCnfgDoc.SavaConfig("DataClnCfg.ColumnsMsg[" + i + "].ColumnName", StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName);
                    StaticClass.RealDataDisplayCnfgDoc.SavaConfig("DataClnCfg.ColumnsMsg[" + i + "].ColumnType", StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType.ToString());
                    StaticClass.RealDataDisplayCnfgDoc.SavaConfig("DataClnCfg.ColumnsMsg[" + i + "].ColumnWidth", StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth.ToString());
                    StaticClass.RealDataDisplayCnfgDoc.SavaConfig("DataClnCfg.ColumnsMsg[" + i + "].IsLocked", StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].IsLocked ? "1" : "0");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("存储显示数据列配置SaveRealDataDisplayConfig.ColumnsMsg", ex);
            }
            #endregion

            #region 存储特殊类型显示配置
            try
            {
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.Selectnet", StaticClass.realdataconfig.SplPointCfg.Selectnet ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.SelectS", StaticClass.realdataconfig.SplPointCfg.SelectS ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.SelectM", StaticClass.realdataconfig.SplPointCfg.SelectM ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.SelectK", StaticClass.realdataconfig.SplPointCfg.SelectK ? "1" : "0");
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.SelectC", StaticClass.realdataconfig.SplPointCfg.SelectC ? "1" : "0");

                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.CColor[0]", StaticClass.realdataconfig.SplPointCfg.CColor[0].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.CColor[1]", StaticClass.realdataconfig.SplPointCfg.CColor[1].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.KColor[0]", StaticClass.realdataconfig.SplPointCfg.KColor[0].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.KColor[1]", StaticClass.realdataconfig.SplPointCfg.KColor[1].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.MColor[0]", StaticClass.realdataconfig.SplPointCfg.MColor[0].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.MColor[1]", StaticClass.realdataconfig.SplPointCfg.MColor[1].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.NetColor[0]", StaticClass.realdataconfig.SplPointCfg.NetColor[0].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.NetColor[1]", StaticClass.realdataconfig.SplPointCfg.NetColor[1].ToArgb().ToString());

                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.Scolor[0]", StaticClass.realdataconfig.SplPointCfg.Scolor[0].ToArgb().ToString());
                StaticClass.RealDataDisplayCnfgDoc.SavaConfig("SplPointCfg.Scolor[1]", StaticClass.realdataconfig.SplPointCfg.Scolor[1].ToArgb().ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("存储特殊类型显示配置SaveRealDataDisplayConfig.SplPointCfg", ex);
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 从本地读取实时显示配置到内存
        /// </summary>
        /// <returns></returns>
        public static bool ReadRealDataDisplayConfig()
        {
            string temp = "";
            int data = 0;
            #region 读取基础配置到内存
            try
            {
                StaticClass.RealDataDisplayCnfgDoc = new XmlConfig(StaticClass.RealDataCnfg);
                #region 显示列表背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.GvBackColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.GvBackColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.GvBackColor = Color.FromArgb(data);
                }
                #endregion

                #region 表头背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.TableHadeBackColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeBackColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeBackColor = Color.FromArgb(data);
                }
                #endregion
                try
                {
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Colorchange");
                    StaticClass.realdataconfig.BaseCfg.Colorchange = temp;

                    StaticClass.realdataconfig.BaseCfg.Showju = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Showju") == "1" ? true : false;
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }

                #region 是否显示网格
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Showgrid");
                StaticClass.realdataconfig.BaseCfg.Showgrid = temp == "1" ? true : false;
                #endregion

                #region 表头行高
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.TableHadeHigh");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = data;
                }
                #endregion

                #region 数据行高
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.DataRowHigh");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = data;
                }
                #endregion

                #region 偶数行颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.DoubleRowColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.DoubleRowColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.DoubleRowColor = Color.FromArgb(data);
                }
                #endregion

                #region 奇数行颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SingleRowColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SingleRowColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SingleRowColor = Color.FromArgb(data);
                }
                #endregion

                #region 网格颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.GridColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.GridColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.GridColor = Color.FromArgb(data);
                }
                #endregion

                #region 自动翻页间隔时间
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.PageChangeInterval");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = data;
                }
                #endregion

                #region 选择后颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SelectColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SelectColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SelectColor = Color.FromArgb(data);
                }
                #endregion

                #region 分隔线颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SplitColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SplitColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SplitColor = Color.FromArgb(data);
                }
                #endregion

                #region 分割线宽度
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SplitWidth");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = data;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取基础配置到内存ReadRealDataDisplayConfig.BaseCfg", ex);
            }
            #endregion

            #region 读取字体配置到内存
            try
            {
                #region 表头字体大小
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontSize");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = data;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = data;
                }
                #endregion

                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.BJfontsize");
                if (!string.IsNullOrEmpty(temp))
                {
                    StaticClass.realdataconfig.BaseCfg.Bjfontsize = float.Parse(temp);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.Bjfontsize = 8;
                }

                #region 表头字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontColor = Color.FromArgb(data);
                }
                #endregion

                #region 表头字体名称
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontName");
                if (!string.IsNullOrEmpty(temp))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontName = temp;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontName = "宋体";
                }
                #endregion

                #region 表头字体是否加粗
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsBold");
                StaticClass.realdataconfig.FontCfg.IsBold = temp == "1" ? true : false;
                #endregion

                #region 表头字体是否有下划线
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsHaveUnderLine");
                StaticClass.realdataconfig.FontCfg.IsHaveUnderLine = temp == "1" ? true : false;
                #endregion

                #region 表头字体是否为斜体
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsItalic");
                StaticClass.realdataconfig.FontCfg.IsItalic = temp == "1" ? true : false;
                #endregion

                #region 数据行字体名称
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontName");
                if (!string.IsNullOrEmpty(temp))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontName = temp;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontName = "宋体";
                }
                #endregion

                #region 数据行字体大小
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontSize");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = data;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = data;
                }
                #endregion

                #region 数据行字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontColor = Color.FromArgb(data);
                }
                #endregion

                #region 数据行字体是否加粗
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsBold");
                StaticClass.realdataconfig.FontCfg.DataIsBold = temp == "1" ? true : false;
                #endregion

                #region 数据行字体是否有下划线
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsHaveUnderLine");
                StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine = temp == "1" ? true : false;
                #endregion

                #region 数据行字体是否为斜体
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsItalic");
                StaticClass.realdataconfig.FontCfg.DataIsItalic = temp == "1" ? true : false;
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error("读取字体配置到内存ReadRealDataDisplayConfig.FontCfg", ex);
            }
            #endregion

            #region 读取各种状态颜色配置到内存
            try
            {

                #region 正常态颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.DefaultColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.DefaultColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.DefaultColor = Color.FromArgb(data);
                }
                #endregion

                #region 中断颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.InterruptionColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.InterruptionColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.InterruptionColor = Color.FromArgb(data);
                }
                #endregion

                #region 开关量报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.KAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.KAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.KAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 开关量断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.KBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.KBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.KBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限预警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowPreAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限预警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpPreAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 超量程颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.OverRangeColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.OverRangeColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.OverRangeColor = Color.FromArgb(data);
                }
                #endregion

                #region 直流颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.DcColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.DcColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.DcColor = Color.FromArgb(data);
                }
                #endregion
                #region 标效颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.EffectColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.EffectColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.EffectColor = Color.FromArgb(data);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取各种状态颜色配置到内存ReadRealDataDisplayConfig.StateCorCfg", ex);
            }
            #endregion

            #region 读取显示数据列配置到内存
            try
            {
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ShowUnit");
                StaticClass.realdataconfig.DataClnCfg.ShowUnit = temp == "1" ? true : false;
                DataColumnMsg clm = new DataColumnMsg();
                for (int i = 0; i < StaticClass.realdataconfig.DataClnCfg.ColumnsMsg.Length; i++)
                {
                    #region 列名称
                    clm = new DataColumnMsg();
                    StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i] = clm;
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnName");
                    if (!string.IsNullOrEmpty(temp))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName = temp;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName = temp;
                    }
                    #endregion

                    #region 对齐方式
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnType");
                    if (!string.IsNullOrEmpty(temp))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType = temp;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType = "左对齐";
                    }
                    #endregion

                    #region 显示宽度
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnWidth");
                    if (int.TryParse(temp, out data))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth = data;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth = 50;
                    }
                    #endregion

                    #region 是否锁定列宽
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].IsLocked");
                    StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].IsLocked = temp == "1" ? true : false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取显示数据列配置到内存ReadRealDataDisplayConfig.ColumnsMsg", ex);
            }
            #endregion

            #region 读取特殊类型显示配置到内存
            try
            {

                #region 控制量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.CColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 控制量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.CColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 开关量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.KColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 开关量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.KColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 模拟量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.MColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 模拟量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.MColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 网络模块显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.NetColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 网络模块显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.NetColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 分站显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Scolor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 分站显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Scolor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 是否启用
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Selectnet");
                StaticClass.realdataconfig.SplPointCfg.Selectnet = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectS");
                StaticClass.realdataconfig.SplPointCfg.SelectS = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectM");
                StaticClass.realdataconfig.SplPointCfg.SelectM = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectK");
                StaticClass.realdataconfig.SplPointCfg.SelectK = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectC");
                StaticClass.realdataconfig.SplPointCfg.SelectC = temp == "1" ? true : false;
                #endregion


            }
            catch (Exception ex)
            {
                LogHelper.Error("读取特殊类型显示配置到内存ReadRealDataDisplayConfig.SplPointCfg", ex);
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 将内存固定编排信息存储到本地
        /// </summary>
        public static void SaveDefalutDataConfig()
        {
            #region 存储类型编排
            StaticClass.RealDataDisplayDefalutCnfgDoc = new XmlConfig(StaticClass.RealDataDefalutCnfg);
            try
            {
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.IsColumnsMsg", StaticClass.arrangeconfig.TypeConfig.IsColumnsMsg);
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.IsDataFillType", StaticClass.arrangeconfig.TypeConfig.IsDataFillType ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.IsUpIndex", StaticClass.arrangeconfig.TypeConfig.IsUpIndex ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.PageSortType", StaticClass.arrangeconfig.TypeConfig.PageSortType.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.ShowColumnCount", StaticClass.arrangeconfig.TypeConfig.ShowColumnCount.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.ShowRowCount", StaticClass.arrangeconfig.TypeConfig.ShowRowCount.ToString());

                for (int i = 0; i < StaticClass.arrangeconfig.TypeConfig.Column.Length; i++)
                {
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.Column[" + i + "].Index", StaticClass.arrangeconfig.TypeConfig.Column[i].Index.ToString());
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("TypeConfig.Column[" + i + "].Isuse", StaticClass.arrangeconfig.TypeConfig.Column[i].Isuse ? "1" : "0");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 存储网络模块编排
            try
            {
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.IsColumnsMsg", StaticClass.arrangeconfig.NetConfig.IsColumnsMsg);
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.IsDataFillType", StaticClass.arrangeconfig.NetConfig.IsDataFillType ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.IsUpIndex", StaticClass.arrangeconfig.NetConfig.IsUpIndex ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.PageSortType", StaticClass.arrangeconfig.NetConfig.PageSortType.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.ShowColumnCount", StaticClass.arrangeconfig.NetConfig.ShowColumnCount.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.ShowRowCount", StaticClass.arrangeconfig.NetConfig.ShowRowCount.ToString());

                for (int i = 0; i < StaticClass.arrangeconfig.NetConfig.Column.Length; i++)
                {
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.Column[" + i + "].Index", StaticClass.arrangeconfig.NetConfig.Column[i].Index.ToString());
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("NetConfig.Column[" + i + "].Isuse", StaticClass.arrangeconfig.NetConfig.Column[i].Isuse ? "1" : "0");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 存储区域编排
            try
            {
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.IsColumnsMsg", StaticClass.arrangeconfig.AreaConfig.IsColumnsMsg);
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.IsDataFillType", StaticClass.arrangeconfig.AreaConfig.IsDataFillType ? "1" : "0");
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.IsUpIndex", StaticClass.arrangeconfig.AreaConfig.IsUpIndex ? "1" : "0");
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.PageSortType", StaticClass.arrangeconfig.AreaConfig.PageSortType.ToString());
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.ShowColumnCount", StaticClass.arrangeconfig.AreaConfig.ShowColumnCount.ToString());
                //StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.ShowRowCount", StaticClass.arrangeconfig.AreaConfig.ShowRowCount.ToString());

                //for (int i = 0; i < StaticClass.arrangeconfig.AreaConfig.Column.Length; i++)
                //{
                //    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.Column[" + i + "].Index", StaticClass.arrangeconfig.AreaConfig.Column[i].Index.ToString());
                //    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("AreaConfig.Column[" + i + "].Isuse", StaticClass.arrangeconfig.AreaConfig.Column[i].Isuse ? "1" : "0");
                //}

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 存储状态编排
            try
            {
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.IsColumnsMsg", StaticClass.arrangeconfig.StateConfig.IsColumnsMsg);
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.IsDataFillType", StaticClass.arrangeconfig.StateConfig.IsDataFillType ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.IsUpIndex", StaticClass.arrangeconfig.StateConfig.IsUpIndex ? "1" : "0");
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.PageSortType", StaticClass.arrangeconfig.StateConfig.PageSortType.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.ShowColumnCount", StaticClass.arrangeconfig.StateConfig.ShowColumnCount.ToString());
                StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.ShowRowCount", StaticClass.arrangeconfig.StateConfig.ShowRowCount.ToString());

                for (int i = 0; i < StaticClass.arrangeconfig.StateConfig.Column.Length; i++)
                {
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.Column[" + i + "].Index", StaticClass.arrangeconfig.StateConfig.Column[i].Index.ToString());
                    StaticClass.RealDataDisplayDefalutCnfgDoc.SavaConfig("StateConfig.Column[" + i + "].Isuse", StaticClass.arrangeconfig.StateConfig.Column[i].Isuse ? "1" : "0");
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion
        }

        /// <summary>
        /// 从本地读取固定编排配置信息到内存
        /// </summary>
        public static void ReadDefalutDataConfig()
        {
            string temp = "";
            int n = 0;
            #region 读取类型编排配置
            try
            {
                StaticClass.RealDataDisplayDefalutCnfgDoc = new XmlConfig(StaticClass.RealDataDefalutCnfg);
                #region 读取基础配置
                StaticClass.arrangeconfig.TypeConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.TypeConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.TypeConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.TypeConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.TypeConfig.Column[i] = cm;
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取网络模块编排
            try
            {
                #region 读取基础配置
                StaticClass.arrangeconfig.NetConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.NetConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.NetConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.NetConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.NetConfig.Column[i] = cm;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取区域编排
            try
            {
                #region 读取基础配置
                //StaticClass.arrangeconfig.AreaConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsColumnsMsg");
                //StaticClass.arrangeconfig.AreaConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsDataFillType") == "1" ? true : false;
                //StaticClass.arrangeconfig.AreaConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsUpIndex") == "1" ? true : false;
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.PageSortType");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.PageSortType = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.PageSortType = 1;
                //}
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.ShowColumnCount");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowColumnCount = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowColumnCount = 2;
                //}
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.ShowRowCount");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowRowCount = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowRowCount = 20;
                //}
                #endregion

                #region 读取列信息配置
                //colummsg cm = new colummsg();
                //for (int i = 0; i < StaticClass.arrangeconfig.AreaConfig.Column.Length; i++)
                //{
                //    cm = new colummsg();
                //    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.Column[" + i + "].Index");
                //    if (int.TryParse(temp, out n))
                //    {
                //        cm.Index = n;
                //    }
                //    else
                //    {
                //        cm.Index = 0;
                //    }
                //    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                //    StaticClass.arrangeconfig.AreaConfig.Column[i] = cm;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取状态编排
            try
            {
                #region 读取基础配置
                StaticClass.arrangeconfig.StateConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.StateConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.StateConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.StateConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.StateConfig.Column[i] = cm;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion
        }

        /// <summary>
        /// 从本地读取实时显示配置到内存
        /// </summary>
        /// <returns></returns>
        public static bool ReadRealDataDisplayConfig(string path)
        {
            string temp = "";
            int data = 0;
            #region 读取基础配置到内存
            try
            {
                StaticClass.RealDataDisplayCnfgDoc = new XmlConfig(path);
                #region 显示列表背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.GvBackColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.GvBackColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.GvBackColor = Color.FromArgb(data);
                }
                #endregion

                #region 表头背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.TableHadeBackColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeBackColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeBackColor = Color.FromArgb(data);
                }
                #endregion
                try
                {
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Colorchange");
                    StaticClass.realdataconfig.BaseCfg.Colorchange = temp;

                    StaticClass.realdataconfig.BaseCfg.Showju = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Showju") == "1" ? true : false;
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }

                #region 是否显示网格
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.Showgrid");
                StaticClass.realdataconfig.BaseCfg.Showgrid = temp == "1" ? true : false;
                #endregion

                #region 表头行高
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.TableHadeHigh");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = data;
                }
                #endregion

                #region 数据行高
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.DataRowHigh");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = data;
                }
                #endregion

                #region 偶数行颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.DoubleRowColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.DoubleRowColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.DoubleRowColor = Color.FromArgb(data);
                }
                #endregion

                #region 奇数行颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SingleRowColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SingleRowColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SingleRowColor = Color.FromArgb(data);
                }
                #endregion

                #region 网格颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.GridColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.GridColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.GridColor = Color.FromArgb(data);
                }
                #endregion

                #region 自动翻页间隔时间
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.PageChangeInterval");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = data;
                }
                #endregion

                #region 选择后颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SelectColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SelectColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SelectColor = Color.FromArgb(data);
                }
                #endregion

                #region 分隔线颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SplitColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SplitColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SplitColor = Color.FromArgb(data);
                }
                #endregion

                #region 分割线宽度
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("BaseCfg.SplitWidth");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = data;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = data;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取基础配置到内存ReadRealDataDisplayConfig.BaseCfg", ex);
            }
            #endregion

            #region 读取字体配置到内存
            try
            {
                #region 表头字体大小
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontSize");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = data;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = data;
                }
                #endregion

                #region 表头字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontColor = Color.FromArgb(data);
                }
                #endregion

                #region 表头字体名称
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.TableHadeFontName");
                if (!string.IsNullOrEmpty(temp))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontName = temp;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontName = "宋体";
                }
                #endregion

                #region 表头字体是否加粗
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsBold");
                StaticClass.realdataconfig.FontCfg.IsBold = temp == "1" ? true : false;
                #endregion

                #region 表头字体是否有下划线
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsHaveUnderLine");
                StaticClass.realdataconfig.FontCfg.IsHaveUnderLine = temp == "1" ? true : false;
                #endregion

                #region 表头字体是否为斜体
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.IsItalic");
                StaticClass.realdataconfig.FontCfg.IsItalic = temp == "1" ? true : false;
                #endregion

                #region 数据行字体名称
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontName");
                if (!string.IsNullOrEmpty(temp))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontName = temp;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontName = "宋体";
                }
                #endregion

                #region 数据行字体大小
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontSize");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = data;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = data;
                }
                #endregion

                #region 数据行字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataFontColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontColor = Color.FromArgb(data);
                }
                #endregion

                #region 数据行字体是否加粗
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsBold");
                StaticClass.realdataconfig.FontCfg.DataIsBold = temp == "1" ? true : false;
                #endregion

                #region 数据行字体是否有下划线
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsHaveUnderLine");
                StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine = temp == "1" ? true : false;
                #endregion

                #region 数据行字体是否为斜体
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("FontCfg.DataIsItalic");
                StaticClass.realdataconfig.FontCfg.DataIsItalic = temp == "1" ? true : false;
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error("读取字体配置到内存ReadRealDataDisplayConfig.FontCfg", ex);
            }
            #endregion

            #region 读取各种状态颜色配置到内存
            try
            {

                #region 正常态颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.DefaultColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.DefaultColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.DefaultColor = Color.FromArgb(data);
                }
                #endregion

                #region 中断颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.InterruptionColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.InterruptionColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.InterruptionColor = Color.FromArgb(data);
                }
                #endregion

                #region 开关量报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.KAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.KAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.KAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 开关量断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.KBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.KBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.KBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 下限预警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.LowPreAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限报警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限断电颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpBlackOutColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor = Color.FromArgb(data);
                }
                #endregion

                #region 上限预警颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.UpPreAlarmColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor = Color.FromArgb(data);
                }
                #endregion

                #region 超量程颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("StateCorCfg.OverRangeColor");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.StateCorCfg.OverRangeColor = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.StateCorCfg.OverRangeColor = Color.FromArgb(data);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取各种状态颜色配置到内存ReadRealDataDisplayConfig.StateCorCfg", ex);
            }
            #endregion

            #region 读取显示数据列配置到内存
            try
            {
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ShowUnit");
                StaticClass.realdataconfig.DataClnCfg.ShowUnit = temp == "1" ? true : false;
                DataColumnMsg clm = new DataColumnMsg();
                for (int i = 0; i < StaticClass.realdataconfig.DataClnCfg.ColumnsMsg.Length; i++)
                {
                    #region 列名称
                    clm = new DataColumnMsg();
                    StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i] = clm;
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnName");
                    if (!string.IsNullOrEmpty(temp))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName = temp;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName = temp;
                    }
                    #endregion

                    #region 对齐方式
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnType");
                    if (!string.IsNullOrEmpty(temp))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType = temp;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType = "左对齐";
                    }
                    #endregion

                    #region 显示宽度
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].ColumnWidth");
                    if (int.TryParse(temp, out data))
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth = data;
                    }
                    else
                    {
                        StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth = 50;
                    }
                    #endregion

                    #region 是否锁定列宽
                    temp = "";
                    temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("DataClnCfg.ColumnsMsg[" + i + "].IsLocked");
                    StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].IsLocked = temp == "1" ? true : false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("读取显示数据列配置到内存ReadRealDataDisplayConfig.ColumnsMsg", ex);
            }
            #endregion

            #region 读取特殊类型显示配置到内存
            try
            {

                #region 控制量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.CColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 控制量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.CColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.CColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 开关量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.KColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 开关量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.KColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.KColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 模拟量显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.MColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 模拟量显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.MColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.MColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 网络模块显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.NetColor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 网络模块显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.NetColor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.NetColor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 分站显示字体颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Scolor[0]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[0] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[0] = Color.FromArgb(data);
                }
                #endregion

                #region 分站显示背景颜色
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Scolor[1]");
                if (int.TryParse(temp, out data))
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[1] = Color.FromArgb(data);
                }
                else
                {
                    StaticClass.realdataconfig.SplPointCfg.Scolor[1] = Color.FromArgb(data);
                }
                #endregion

                #region 是否启用
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.Selectnet");
                StaticClass.realdataconfig.SplPointCfg.Selectnet = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectS");
                StaticClass.realdataconfig.SplPointCfg.SelectS = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectM");
                StaticClass.realdataconfig.SplPointCfg.SelectM = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectK");
                StaticClass.realdataconfig.SplPointCfg.SelectK = temp == "1" ? true : false;
                temp = "";
                temp = StaticClass.RealDataDisplayCnfgDoc.GetValue("SplPointCfg.SelectC");
                StaticClass.realdataconfig.SplPointCfg.SelectC = temp == "1" ? true : false;
                #endregion


            }
            catch (Exception ex)
            {
                LogHelper.Error("读取特殊类型显示配置到内存ReadRealDataDisplayConfig.SplPointCfg", ex);
            }
            #endregion
            return true;
        }


        /// <summary>
        /// 从本地读取固定编排配置信息到内存
        /// </summary>
        public static void ReadDefalutDataConfig(string path)
        {
            string temp = "";
            int n = 0;
            #region 读取类型编排配置
            try
            {
                StaticClass.RealDataDisplayDefalutCnfgDoc = new XmlConfig(path);
                #region 读取基础配置
                StaticClass.arrangeconfig.TypeConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.TypeConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.TypeConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.TypeConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.TypeConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("TypeConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.TypeConfig.Column[i] = cm;
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取网络模块编排
            try
            {
                #region 读取基础配置
                StaticClass.arrangeconfig.NetConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.NetConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.NetConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.NetConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.NetConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.NetConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("NetConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.NetConfig.Column[i] = cm;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取区域编排
            try
            {
                #region 读取基础配置
                //StaticClass.arrangeconfig.AreaConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsColumnsMsg");
                //StaticClass.arrangeconfig.AreaConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsDataFillType") == "1" ? true : false;
                //StaticClass.arrangeconfig.AreaConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.IsUpIndex") == "1" ? true : false;
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.PageSortType");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.PageSortType = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.PageSortType = 1;
                //}
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.ShowColumnCount");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowColumnCount = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowColumnCount = 2;
                //}
                //temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.ShowRowCount");
                //if (int.TryParse(temp, out n))
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowRowCount = n;
                //}
                //else
                //{
                //    StaticClass.arrangeconfig.AreaConfig.ShowRowCount = 20;
                //}
                #endregion

                #region 读取列信息配置
                //colummsg cm = new colummsg();
                //for (int i = 0; i < StaticClass.arrangeconfig.AreaConfig.Column.Length; i++)
                //{
                //    cm = new colummsg();
                //    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.Column[" + i + "].Index");
                //    if (int.TryParse(temp, out n))
                //    {
                //        cm.Index = n;
                //    }
                //    else
                //    {
                //        cm.Index = 0;
                //    }
                //    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("AreaConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                //    StaticClass.arrangeconfig.AreaConfig.Column[i] = cm;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 读取状态编排
            try
            {
                #region 读取基础配置
                StaticClass.arrangeconfig.StateConfig.IsColumnsMsg = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsColumnsMsg");
                StaticClass.arrangeconfig.StateConfig.IsDataFillType = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsDataFillType") == "1" ? true : false;
                StaticClass.arrangeconfig.StateConfig.IsUpIndex = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.IsUpIndex") == "1" ? true : false;
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.PageSortType");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.PageSortType = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.PageSortType = 1;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.ShowColumnCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.ShowColumnCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.ShowColumnCount = 2;
                }
                temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.ShowRowCount");
                if (int.TryParse(temp, out n))
                {
                    StaticClass.arrangeconfig.StateConfig.ShowRowCount = n;
                }
                else
                {
                    StaticClass.arrangeconfig.StateConfig.ShowRowCount = 20;
                }
                #endregion

                #region 读取列信息配置
                colummsg cm = new colummsg();
                for (int i = 0; i < StaticClass.arrangeconfig.StateConfig.Column.Length; i++)
                {
                    cm = new colummsg();
                    temp = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.Column[" + i + "].Index");
                    if (int.TryParse(temp, out n))
                    {
                        cm.Index = n;
                    }
                    else
                    {
                        cm.Index = 0;
                    }
                    cm.Isuse = StaticClass.RealDataDisplayDefalutCnfgDoc.GetValue("StateConfig.Column[" + i + "].Isuse") == "1" ? true : false;
                    StaticClass.arrangeconfig.StateConfig.Column[i] = cm;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion
        }

        /// <summary>
        /// 存储自定义编排到本地配置
        /// </summary>
        public static void SaveCustomConfig()
        {
            #region 存储自定义配置
            StaticClass.RealDataDisplayCustomCnfgDoc = new XmlConfig(StaticClass.RealDataCustomCnfg);
            for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
            {
                try
                {
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].Page", StaticClass.arrangeconfig.CustomCofig[i].Page.ToString());
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].PageName", StaticClass.arrangeconfig.CustomCofig[i].PageName);
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].IsDataFillType", StaticClass.arrangeconfig.CustomCofig[i].IsDataFillType ? "1" : "0");
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].IsColumnsMsg", StaticClass.arrangeconfig.CustomCofig[i].IsColumnsMsg);
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].ShowColumnCount", StaticClass.arrangeconfig.CustomCofig[i].ShowColumnCount.ToString());
                    StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].ShowRowCount", StaticClass.arrangeconfig.CustomCofig[i].ShowRowCount.ToString());
                    for (int j = 0; j < StaticClass.arrangeconfig.CustomCofig[i].Column.Length; j++)
                    {
                        try
                        {
                            StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].Column[" + j + "].Index", StaticClass.arrangeconfig.CustomCofig[i].Column[j].Index.ToString());
                            StaticClass.RealDataDisplayCustomCnfgDoc.SavaConfig("CustomCofig[" + i + "].Column[" + j + "].Isuse", StaticClass.arrangeconfig.CustomCofig[i].Column[j].Isuse ? "1" : "0");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
            #endregion
        }


        public static void setfont(float n)
        {
            try
            {
                if (StaticClass.YJForm != null)
                {
                    StaticClass.YJForm.setfontsize(n);
                }


                if (StaticClass.KGLBJForm != null)
                {
                    StaticClass.KGLBJForm.setfontsize(n);
                }

                if (StaticClass.KDYCForm != null)
                {
                    StaticClass.KDYCForm.setfontsize(n);
                }

                if (StaticClass.KGLBDForm != null)
                {
                    StaticClass.KGLBDForm.setfontsize(n);
                }

                if (StaticClass.KZForm != null)
                {
                    StaticClass.KZForm.setfontsize(n);
                }

                if (StaticClass.MNLBJForm != null)
                {
                    StaticClass.MNLBJForm.setfontsize(n);
                }

                if (StaticClass.MNLDDForm != null)
                {
                    StaticClass.MNLDDForm.setfontsize(n);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 读取自定义编排到内存
        /// </summary>
        public static void ReadCustomConfig()
        {
            #region 读取自定义配置
            PageSetConfig page = new PageSetConfig();
            colummsg clm = new colummsg();
            string temp = "";
            int n = 0;
            StaticClass.RealDataDisplayCustomCnfgDoc = new XmlConfig(StaticClass.RealDataCustomCnfg);
            for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
            {
                try
                {
                    page = new PageSetConfig();
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Page");
                    if (int.TryParse(temp, out n))
                    {
                        page.Page = n;
                    }
                    else
                    {
                        page.Page = 0;
                    }
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].ShowColumnCount");
                    if (int.TryParse(temp, out n))
                    {
                        page.ShowColumnCount = n;
                    }
                    else
                    {
                        page.ShowColumnCount = 0;
                    }
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].ShowRowCount");
                    if (int.TryParse(temp, out n))
                    {
                        page.ShowRowCount = n;
                    }
                    else
                    {
                        page.ShowRowCount = 0;
                    }
                    page.PageName = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].PageName");
                    page.IsDataFillType = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].IsDataFillType") == "1" ? true : false;
                    page.IsColumnsMsg = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].IsColumnsMsg");

                    for (int j = 0; j < page.Column.Length; j++)
                    {
                        try
                        {
                            clm = new colummsg();
                            temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Column[" + j + "].Index");
                            if (int.TryParse(temp, out n))
                            {
                                clm.Index = n;
                            }
                            else
                            {
                                clm.Index = 0;
                            }
                            clm.Isuse = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Column[" + j + "].Isuse") == "1" ? true : false;
                            page.Column[j] = clm;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                    StaticClass.arrangeconfig.CustomCofig[i] = page;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
            #endregion
        }

        /// <summary>
        /// 读取自定义编排到内存
        /// </summary>
        public static void ReadCustomConfig(string path)
        {
            #region 读取自定义配置
            PageSetConfig page = new PageSetConfig();
            colummsg clm = new colummsg();
            string temp = "";
            int n = 0;
            StaticClass.RealDataDisplayCustomCnfgDoc = new XmlConfig(path);
            for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
            {
                try
                {
                    page = new PageSetConfig();
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Page");
                    if (int.TryParse(temp, out n))
                    {
                        page.Page = n;
                    }
                    else
                    {
                        page.Page = 0;
                    }
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].ShowColumnCount");
                    if (int.TryParse(temp, out n))
                    {
                        page.ShowColumnCount = n;
                    }
                    else
                    {
                        page.ShowColumnCount = 0;
                    }
                    temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].ShowRowCount");
                    if (int.TryParse(temp, out n))
                    {
                        page.ShowRowCount = n;
                    }
                    else
                    {
                        page.ShowRowCount = 0;
                    }
                    page.PageName = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].PageName");
                    page.IsDataFillType = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].IsDataFillType") == "1" ? true : false;
                    page.IsColumnsMsg = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].IsColumnsMsg");

                    for (int j = 0; j < page.Column.Length; j++)
                    {
                        try
                        {
                            clm = new colummsg();
                            temp = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Column[" + j + "].Index");
                            if (int.TryParse(temp, out n))
                            {
                                clm.Index = n;
                            }
                            else
                            {
                                clm.Index = 0;
                            }
                            clm.Isuse = StaticClass.RealDataDisplayCustomCnfgDoc.GetValue("CustomCofig[" + i + "].Column[" + j + "].Isuse") == "1" ? true : false;
                            page.Column[j] = clm;
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                    StaticClass.arrangeconfig.CustomCofig[i] = page;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取类别
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAlllb(string fzh)
        {
            DataTable dt = new DataTable();
            DataRow row = null;
            #region 类别
            DataColumn column = new DataColumn("lx", typeof(string));
            dt.Columns.Add(column);
            column = new DataColumn("zl", typeof(string));
            dt.Columns.Add(column);
            column = new DataColumn("lb", typeof(string));
            dt.Columns.Add(column);
            column = new DataColumn("lxtype", typeof(int));
            dt.Columns.Add(column);
            // 20170322
            lock (StaticClass.allPointDtLockObj)
            {
                if (StaticClass.AllPointDt == null || StaticClass.AllPointDt.Rows.Count < 1)
                {
                    dt = Model.RealInterfaceFuction.GetAllPoint();
                    return dt;
                }
                if (fzh == "")
                {
                    for (int i = 0; i < StaticClass.AllPointDt.Rows.Count; i++)
                    {
                        try
                        {
                            row = StaticClass.AllPointDt.Rows[i];
                            dt.Rows.Add(row["lx"].ToString(), row["zl"].ToString(), row["lb"].ToString(), row["lxtype"]);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < StaticClass.AllPointDt.Rows.Count; i++)
                    {
                        try
                        {
                            row = StaticClass.AllPointDt.Rows[i];
                            if (row["fzh"].ToString() == fzh)
                            {
                                dt.Rows.Add(row["lx"].ToString(), row["zl"].ToString(), row["lb"].ToString(), row["lxtype"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex.Message, ex);
                        }
                    }
                }
            }
            DataView view = new DataView(dt);
            dt = view.ToTable(true, "lx", "lxtype", "zl", "lb");
            #endregion
            return dt;
        }


        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void SaveErrorLogs(string msg, Exception ex)
        {
            LogHelper.Error(msg, ex);
        }

        /// <summary>
        /// 输出错误日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void SaveErrorLogs(object msg)
        {
            LogHelper.Error(msg);
        }

        /// <summary>
        /// 显示自定义messagebox
        /// </summary>
        /// <param name="type">弹出对话框样式 0-显示提示信息 1-确定对话框 2-等待对话框 3-询问对话框 4-警告对话框 5-风险等级</param>
        /// <param name="msg">显示内容</param>
        public static System.Windows.Forms.DialogResult MessageBoxShow(int type, string msg)
        {
            Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType etype;
            switch (type)
            {
                case 0:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Information;
                    break;
                case 1:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Confirm;
                    break;
                case 2:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Hand;
                    break;
                case 3:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Question;
                    break;
                case 4:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Warning;
                    break;
                case 5:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Stop;
                    break;
                default:
                    etype = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.None;
                    break;
            }
            return Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(etype, msg);
        }

        /// <summary>
        /// 加载系统名称及系统logo
        /// </summary>
        /// <param name="dt"></param>
        public static void ConfigLoad(DataTable dt)
        {
            DataRow[] rows = null;
            string temp = "";
            byte[] bys = new byte[1];
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    rows = dt.Select("configname='CustomerName'");
                    if (rows.Length > 0)
                    {
                        SystemName = rows[0]["configvalue"].ToString();
                    }
                    rows = dt.Select("configname='CustomerLogo'");
                    if (rows.Length > 0)
                    {
                        temp = rows[0]["configvalue"].ToString(); ;
                        bys = Convert.FromBase64String(temp);//把字符串读到字节数组中
                        MemoryStream ms = new MemoryStream(bys);
                        SystemLogo = System.Drawing.Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SystemName = "";

        /// <summary>
        /// 系统logo
        /// </summary>
        public static Image SystemLogo = null;

        /// <summary>
        /// 保存配置文件到数据库
        /// </summary>
        /// <returns></returns>
        public static bool SaveRealConfigToDB()
        {
            Stream filestream;
            byte[] by;
            int len = 0;
            bool flg = false;
            List<ConfigInfo> list = new List<ConfigInfo>();
            ConfigInfo con;
            string ConfigName, ConfigValue;
            try
            {
                if (File.Exists(StaticClass.RealDataCnfg))
                {
                    ConfigName = "RealDataCnfg";
                    using (filestream = File.Open(StaticClass.RealDataCnfg, FileMode.Open))
                    {
                        len = (int)filestream.Length;
                        by = new byte[len];
                        filestream.Read(by, 0, len);
                        ConfigValue = Convert.ToBase64String(by);
                        con = new ConfigInfo();
                        con.Name = ConfigName;
                        con.Text = ConfigValue;
                        list.Add(con);
                    }
                }

                if (File.Exists(StaticClass.RealDataDefalutCnfg))
                {
                    ConfigName = "RealDataDefalutCnfg";
                    using (filestream = File.Open(StaticClass.RealDataDefalutCnfg, FileMode.Open))
                    {
                        len = (int)filestream.Length;
                        by = new byte[len];
                        filestream.Read(by, 0, len);
                        ConfigValue = Convert.ToBase64String(by);
                        con = new ConfigInfo();
                        con.Name = ConfigName;
                        con.Text = ConfigValue;

                        list.Add(con);
                    }
                }

                if (File.Exists(StaticClass.RealDataCustomCnfg))
                {
                    ConfigName = "RealDataCustomCnfg";
                    using (filestream = File.Open(StaticClass.RealDataCustomCnfg, FileMode.Open))
                    {
                        len = (int)filestream.Length;
                        by = new byte[len];
                        filestream.Read(by, 0, len);
                        ConfigValue = Convert.ToBase64String(by);
                        con = new ConfigInfo();
                        con.Name = ConfigName;
                        con.Text = ConfigValue;
                        list.Add(con);
                    }
                }
                //if (OprFuction .GetClientType() && Model .RealInterfaceFuction.SaveConfig(list))
                //{
                if (Model.RealInterfaceFuction.SaveConfig(list))
                {
                    flg = true;
                    Model.RealInterfaceFuction.SetRealCfgChange();
                }
                else
                {
                    OprFuction.MessageBoxShow(0, "存数据库失败");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return flg;
        }

        /// <summary>
        /// 从数据库中读取配置信息
        /// </summary>
        /// <returns></returns>
        public static bool ReadRealConfigFromDB()
        {
            string str = "";
            bool flg = true;
            Stream filestream;
            byte[] by;
            try
            {
                #region 读取实时配置信息
                try
                {
                    str = Model.RealInterfaceFuction.ReadConfig("RealDataCnfg");
                    if (!string.IsNullOrEmpty(str))
                    {
                        by = Convert.FromBase64String(str);
                        using (filestream = File.Open(StaticClass.RealDataCnfg, FileMode.Create, FileAccess.ReadWrite))
                        {
                            filestream.Write(by, 0, by.Length);
                            filestream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                try
                {
                    str = Model.RealInterfaceFuction.ReadConfig("RealDataDefalutCnfg");
                    if (!string.IsNullOrEmpty(str))
                    {
                        by = Convert.FromBase64String(str);
                        using (filestream = File.Open(StaticClass.RealDataDefalutCnfg, FileMode.Create, FileAccess.ReadWrite))
                        {
                            filestream.Write(by, 0, by.Length);
                            filestream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                try
                {
                    str = Model.RealInterfaceFuction.ReadConfig("RealDataCustomCnfg");
                    if (!string.IsNullOrEmpty(str))
                    {
                        by = Convert.FromBase64String(str);
                        using (filestream = File.Open(StaticClass.RealDataCustomCnfg, FileMode.Create, FileAccess.ReadWrite))
                        {
                            filestream.Write(by, 0, by.Length);
                            filestream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                #endregion
            }
            catch
            {
                flg = false;
            }
            return flg;
        }

        /// <summary>
        /// 是否为初始时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsInitTime(DateTime time)
        {
            bool flg = false;
            if (OprFuction.TimeToString(time) == StaticClass.InitTime)
            {
                flg = true;
            }
            return flg;
        }

        /// <summary>
        /// 拷贝一个
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Jc_BInfo NewDTO(Jc_BInfo obj)
        {
            Jc_BInfo dto = new Jc_BInfo();
            dto.ID = obj.ID;
            dto.Kh = obj.Kh;
            dto.Kzk = obj.Kzk;
            dto.Kdid = obj.Kdid;
            dto.Pjz = obj.Pjz;
            dto.Point = obj.Point;
            dto.Ssz = obj.Ssz;
            dto.Stime = obj.Stime;
            dto.Etime = obj.Etime;
            dto.Dzh = obj.Dzh;
            dto.Upflag = obj.Upflag;
            dto.Type = obj.Type;
            dto.Zdz = obj.Zdz;
            dto.Zdzs = obj.Zdzs;
            dto.Wzid = obj.Wzid;
            dto.Devid = obj.Devid;
            dto.InfoState = obj.InfoState;
            dto.Cs = obj.Cs;
            dto.Fzh = obj.Fzh;
            dto.State = obj.State;
            dto.Isalarm = obj.Isalarm;//2017.7.14 by

            dto.Bz1 = obj.Bz1;//新增备用字段  20170817
            dto.Bz2 = obj.Bz2;//新增备用字段  20170817
            dto.Bz3 = obj.Bz3;//新增备用字段  20170817
            dto.Bz4 = obj.Bz4;//新增备用字段  20170817
            dto.Bz5 = obj.Bz5;//新增备用字段  20170817
            return dto;
        }

        /// <summary>
        /// 判断开关量是否报警 或断电
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool KGLisAlarm(Jc_BInfo obj)
        {
            bool flg = false;
            try
            {
                if (obj.Isalarm > 0)
                {
                    flg = true;
                }
            }
            catch (Exception ex)
            {
                SaveErrorLogs("开关量当前状态是否报警判断", ex);
            }
            return flg;
        }

        /// <summary>
        /// 是否为开关量
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsKGL(Jc_BInfo obj)
        {
            bool flg = false;
            try
            {
                lock (StaticClass.allPointDtLockObj)
                {
                    DataRow[] row = StaticClass.AllPointDt.Select("point='" + obj.Point + "'");
                    if (row != null)
                    {
                        if (row.Length > 0 && row[0].Table.Columns.Contains("lx") && row[0]["lx"].ToString() == "开关量")
                        {
                            flg = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return flg;
        }

        /// <summary>
        /// 将时间转换为标准格式字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string TimeToString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 是否为主操作客户端
        /// </summary>
        /// <returns></returns>
        public static bool GetClientType()
        {
            bool flg = false;
            try
            {
                flg = (MasterManagement.IsMaster() == 0);//等于0表示正常
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return flg;
        }

        /// <summary>
        /// 显示颜色
        /// </summary>
        /// <param name="state"></param>
        /// <param name="color"></param>
        /// <param name="alarmflag"></param>
        /// <returns></returns>
        public static string GetShowColor(string state, string color, string alarmflag)
        {
            bool alarm = alarmflag == "0" ? false : true;
            Color forcolor;
            if (state == StaticClass.itemStateToClient.EqpState43.ToString() ||
                state == StaticClass.itemStateToClient.EqpState24.ToString() ||
                state == StaticClass.itemStateToClient.EqpState25.ToString() ||
                state == StaticClass.itemStateToClient.EqpState26.ToString() ||
                state == StaticClass.itemStateToClient.EqpState44.ToString())
            {
                forcolor = StateToColor(state, color, alarm);
            }
            else
            {
                forcolor = StateToColor(state, "0", alarm);
            }

            return forcolor.ToArgb().ToString();
        }


        public static void definechange()
        {
            #region 定义改变重新加载
            try
            {
                if (Model.RealInterfaceFuction.GetDefineChangeFlg() || StaticClass.AllPointDt == null || StaticClass.AllPointDt.Columns.Count == 0)//如果AllPointDt为空也要重新加载一下  20171025
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        StaticClass.AllPointDt = Model.RealInterfaceFuction.GetAllPoint();
                    }
                    #region 更新窗体
                    if (StaticClass._type_s != null)
                    {
                        StaticClass._type_s.BeginInvoke(StaticClass._type_dele);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("定义改变重新加载", ex);
            }
            #endregion

            #region 实时配置改变重新加载 +是否主操作客户端判断
            try
            {
                if (Model.RealInterfaceFuction.GetRealCfgChangeFlg() && StaticClass.ReadConfigFromDataBase)
                {
                    OprFuction.ReadRealConfigFromDB();
                    OprFuction.ReadCustomConfig();
                    OprFuction.ReadDefalutDataConfig();
                    OprFuction.ReadRealDataDisplayConfig();
                    #region 更新窗体
                    if (StaticClass._type_s != null)
                    {
                        StaticClass._type_s.BeginInvoke(StaticClass._type_dele);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("实时配置改变重新加载", ex);
            }
            #endregion
        }

        /// <summary>
        /// 同时刷新总表和显示表
        /// </summary>
        public static void RealDataFresh()
        {
            DataTable dt;
            DataRow[] rows, rows1;
            string point;
            string state = "";
            try
            {
                //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                //stopWatch.Restart();
                dt = Model.RealInterfaceFuction.GetRealData();
                //stopWatch.Stop();
                //Basic.Framework.Logging.LogHelper.Debug("读取所有实时数据--:" + stopWatch.ElapsedMilliseconds);
                Thread.Sleep(50);

                //stopWatch.Restart();
                if (dt != null && dt.Rows.Count > 0)
                {

                    try
                    {
                        //stopWatch.Restart();
                        #region 刷新显示表
                        if (StaticClass.real_s != null && StaticClass.real_s.ShowDt != null && StaticClass.real_s.ShowDt.Rows.Count > 0)
                        {
                            lock (StaticClass.real_s.objShowDt)
                            {
                                for (int i = 0; i < StaticClass.real_s.ShowDt.Rows.Count; i++)
                                {
                                    point = StaticClass.real_s.ShowDt.Rows[i]["point"].ToString();
                                    rows = dt.Select("point='" + point + "'");
                                    if (rows.Length > 0)
                                    {
                                        StaticClass.real_s.ShowDt.Rows[i]["ssz"] = rows[0]["ssz"];
                                        StaticClass.real_s.ShowDt.Rows[i]["zt"] = rows[0]["zt"];
                                        StaticClass.real_s.ShowDt.Rows[i]["sbzt"] = rows[0]["sbzt"];
                                        StaticClass.real_s.ShowDt.Rows[i]["bj"] = rows[0]["bj"];
                                        StaticClass.real_s.ShowDt.Rows[i]["dldj"] = rows[0]["dldj"];
                                        StaticClass.real_s.ShowDt.Rows[i]["time"] = rows[0]["time"];
                                        StaticClass.real_s.ShowDt.Rows[i]["GradingAlarmLevel"] = rows[0]["GradingAlarmLevel"];
                                        state = rows[0]["zt"].ToString();
                                        if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
                                        {
                                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.real_s.ShowDt.Rows[i]["0tcolor"].ToString(), rows[0]["bj"].ToString());
                                        }
                                        else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
                                        {
                                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.real_s.ShowDt.Rows[i]["1tcolor"].ToString(), rows[0]["bj"].ToString());
                                        }
                                        else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
                                        {
                                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.real_s.ShowDt.Rows[i]["2tcolor"].ToString(), rows[0]["bj"].ToString());
                                        }
                                        else
                                        {
                                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(state, "0", rows[0]["bj"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        //stopWatch.Stop();
                        //Basic.Framework.Logging.LogHelper.Debug("刷新显示表实时数据--:" + stopWatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        OprFuction.SaveErrorLogs("刷新显示表", ex);
                    }
                    Thread.Sleep(50);
                    try
                    {
                        //stopWatch.Restart();
                        #region 刷新总表
                        //stopWatch.Restart();
                        lock (StaticClass.allPointDtLockObj)
                        {
                            //stopWatch.Stop();
                            //Basic.Framework.Logging.LogHelper.Debug("刷新总表获取锁时间--:" + stopWatch.ElapsedMilliseconds);
                            if (StaticClass.AllPointDt != null && StaticClass.AllPointDt.Rows.Count > 0)
                            {
                                if (dt != null)// 20170322
                                {
                                    //stopWatch.Restart();                                    
                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{
                                    //    point = dt.Rows[i]["point"].ToString();
                                    //    rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                                    //    if (rows.Length > 0)
                                    //    {
                                    //        rows[0]["ssz"] = dt.Rows[i]["ssz"];
                                    //        rows[0]["zt"] = dt.Rows[i]["zt"];
                                    //        rows[0]["sbzt"] = dt.Rows[i]["sbzt"];
                                    //        rows[0]["bj"] = dt.Rows[i]["bj"];
                                    //        rows[0]["dldj"] = dt.Rows[i]["dldj"];
                                    //        rows[0]["time"] = dt.Rows[i]["time"];
                                    //        state = rows[0]["zt"].ToString();
                                    //        if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
                                    //        {
                                    //            rows[0]["statecolor"] = rows[0]["sszcolor"] = GetShowColor(state, rows[0]["0tcolor"].ToString(), dt.Rows[i]["bj"].ToString());
                                    //        }
                                    //        else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
                                    //        {
                                    //            rows[0]["statecolor"] = rows[0]["sszcolor"] = GetShowColor(state, rows[0]["1tcolor"].ToString(), dt.Rows[i]["bj"].ToString());
                                    //        }
                                    //        else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
                                    //        {
                                    //            rows[0]["statecolor"] = rows[0]["sszcolor"] = GetShowColor(state, rows[0]["2tcolor"].ToString(), dt.Rows[i]["bj"].ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            rows[0]["statecolor"] = rows[0]["sszcolor"] = GetShowColor(state, "0", dt.Rows[i]["bj"].ToString());
                                    //        }
                                    //    }
                                    //}
                                    //解决性能问题  20170717
                                    for (int i = 0; i < StaticClass.AllPointDt.Rows.Count; i++)
                                    {
                                        point = StaticClass.AllPointDt.Rows[i]["point"].ToString();
                                        rows = dt.Select("point='" + point + "'");
                                        if (rows.Length > 0)
                                        {
                                            StaticClass.AllPointDt.Rows[i]["ssz"] = rows[0]["ssz"];
                                            StaticClass.AllPointDt.Rows[i]["voltage"] = rows[0]["voltage"];//电压赋值
                                            StaticClass.AllPointDt.Rows[i]["zt"] = rows[0]["zt"];
                                            StaticClass.AllPointDt.Rows[i]["NCtrlSate"] = rows[0]["NCtrlSate"];//获取控制量馈电状态  20170725
                                            StaticClass.AllPointDt.Rows[i]["sbzt"] = rows[0]["sbzt"];
                                            StaticClass.AllPointDt.Rows[i]["bj"] = rows[0]["bj"];
                                            StaticClass.AllPointDt.Rows[i]["dldj"] = rows[0]["dldj"];
                                            StaticClass.AllPointDt.Rows[i]["time"] = rows[0]["time"];
                                            StaticClass.AllPointDt.Rows[i]["StationDyType"] = rows[0]["StationDyType"];//电源箱通讯状态
                                            state = StaticClass.AllPointDt.Rows[i]["zt"].ToString();
                                            if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
                                            {
                                                StaticClass.AllPointDt.Rows[i]["statecolor"] = StaticClass.AllPointDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.AllPointDt.Rows[i]["0tcolor"].ToString(), rows[0]["bj"].ToString());
                                            }
                                            else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
                                            {
                                                StaticClass.AllPointDt.Rows[i]["statecolor"] = StaticClass.AllPointDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.AllPointDt.Rows[i]["1tcolor"].ToString(), rows[0]["bj"].ToString());
                                            }
                                            else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
                                            {
                                                StaticClass.AllPointDt.Rows[i]["statecolor"] = StaticClass.AllPointDt.Rows[i]["sszcolor"] = GetShowColor(state, StaticClass.AllPointDt.Rows[i]["2tcolor"].ToString(), rows[0]["bj"].ToString());
                                            }
                                            else
                                            {
                                                StaticClass.AllPointDt.Rows[i]["statecolor"] = StaticClass.AllPointDt.Rows[i]["sszcolor"] = GetShowColor(state, "0", rows[0]["bj"].ToString());
                                            }
                                        }
                                    }
                                    //stopWatch.Stop();
                                    //Basic.Framework.Logging.LogHelper.Debug("刷新总表循环赋值时间--:" + stopWatch.ElapsedMilliseconds);
                                }
                            }
                        }
                        #endregion
                        //stopWatch.Stop();
                        //Basic.Framework.Logging.LogHelper.Debug("刷新总表实时数据--:" + stopWatch.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        OprFuction.SaveErrorLogs("刷新总表", ex);
                    }
                    //stopWatch.Restart();
                    #region 分站休眠处理
                    try
                    {
                        lock (StaticClass.allPointDtLockObj)
                        {
                            if (StaticClass.AllPointDt != null && StaticClass.AllPointDt.Rows.Count > 0)
                            {
                                rows = StaticClass.AllPointDt.Select("lx='分站' and sbzt='" + StaticClass.itemStateToClient.EqpState33 + "'");
                                if (rows.Length > 0)
                                {
                                    foreach (DataRow r in rows)
                                    {
                                        rows1 = StaticClass.AllPointDt.Select("fzh='" + r["fzh"].ToString() + "'");
                                        if (rows1.Length > 0)
                                        {
                                            foreach (DataRow r1 in rows1)
                                            {
                                                r1["ssz"] = "休眠";
                                                r1["zt"] = StaticClass.itemStateToClient.EqpState33;
                                                r1["sbzt"] = StaticClass.itemStateToClient.EqpState33;
                                                r1["bj"] = "0";
                                                r1["statecolor"] = r1["sszcolor"] = GetShowColor(StaticClass.itemStateToClient.EqpState33.ToString(), "0", "0");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(ex);
                    }
                    #endregion

                    #region 分站休眠处理
                    try
                    {
                        if (StaticClass.real_s != null && StaticClass.real_s.ShowDt != null && StaticClass.real_s.ShowDt.Rows.Count > 0)
                        {
                            lock (StaticClass.allPointDtLockObj)
                            {
                                rows = StaticClass.AllPointDt.Select("lx='分站' and sbzt='" + StaticClass.itemStateToClient.EqpState33 + "'");
                                if (rows.Length > 0)
                                {
                                    lock (StaticClass.real_s.objShowDt)
                                    {
                                        foreach (DataRow r in rows)
                                        {
                                            rows1 = StaticClass.real_s.ShowDt.Select("fzh='" + r["fzh"].ToString() + "'");
                                            if (rows1.Length > 0)
                                            {

                                                foreach (DataRow r1 in rows1)
                                                {
                                                    r1["ssz"] = "休眠";
                                                    r1["zt"] = StaticClass.itemStateToClient.EqpState33;
                                                    r1["sbzt"] = StaticClass.itemStateToClient.EqpState33;
                                                    r1["bj"] = "0";
                                                    r1["statecolor"] = r1["sszcolor"] = GetShowColor(StaticClass.itemStateToClient.EqpState33.ToString(), "0", "0");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(ex);
                    }
                    #endregion
                    //stopWatch.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug("分站休眠处理--:" + stopWatch.ElapsedMilliseconds);
                }
                else
                {
                    if (!StaticClass.ServerConet)
                    {
                        if ((DateTime.Now - StaticClass.ServerConnetInrTime).TotalSeconds > 15)
                        {
                            Model.RealInterfaceFuction.lastRefreshRealDataTime = new DateTime(1900, 01, 01);//通讯中断后,重新设置获取数据的时候,以便下次通讯正常后,重新获取所有数据  20170729
                            gz();
                        }
                    }
                }
                //stopWatch.Stop();
                //Basic.Framework.Logging.LogHelper.Debug("刷新内存显示表--:" + stopWatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        public static void gz()
        {
            try
            {
                #region 刷新显示表
                if (StaticClass.real_s != null && StaticClass.real_s.ShowDt != null && StaticClass.real_s.ShowDt.Rows.Count > 0)
                {
                    lock (StaticClass.real_s.objShowDt)
                    {
                        for (int i = 0; i < StaticClass.real_s.ShowDt.Rows.Count; i++)
                        {

                            StaticClass.real_s.ShowDt.Rows[i]["ssz"] = "";
                            StaticClass.real_s.ShowDt.Rows[i]["zt"] = "119";
                            StaticClass.real_s.ShowDt.Rows[i]["sbzt"] = "119";
                            StaticClass.real_s.ShowDt.Rows[i]["bj"] = "1";
                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(StaticClass.itemStateToClient.EqpState2.ToString(), "0", "1");
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新显示表", ex);
            }

            try
            {
                #region 刷新总表
                if (StaticClass.AllPointDt != null && StaticClass.AllPointDt.Rows.Count > 0)
                {
                    lock (StaticClass.real_s.objShowDt)
                    {
                        for (int i = 0; i < StaticClass.AllPointDt.Rows.Count; i++)
                        {

                            StaticClass.AllPointDt.Rows[i]["ssz"] = "";
                            StaticClass.AllPointDt.Rows[i]["zt"] = "119";
                            StaticClass.AllPointDt.Rows[i]["sbzt"] = "119";
                            StaticClass.real_s.ShowDt.Rows[i]["bj"] = "1";
                            StaticClass.real_s.ShowDt.Rows[i]["statecolor"] = StaticClass.real_s.ShowDt.Rows[i]["sszcolor"] = GetShowColor(StaticClass.itemStateToClient.EqpState2.ToString(), "0", "1");
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("刷新总表", ex);
            }
        }

        /// <summary>
        /// 设置连接中断
        /// </summary>
        public static void SetServerConct()
        {
            if (StaticClass.ServerConet)
            {
                StaticClass.ServerConet = false;
                StaticClass.ServerConnetInrTime = DateTime.Now;
                Alarm.ClientAlarmConfig.setserverconnectstate(false);
            }
        }

        /// <summary>
        /// 获取报警数据线程
        /// </summary>
        public static void GetbjTh()
        {
            for (; ; )
            {
                try
                {
                    if (StaticClass.SystemOut)
                    {
                        break;
                    }
                    else
                    {
                        GetBJData();
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 将状态转换成文字输出 待统一  20171128
        /// </summary>
        /// <param name="msg">数字状态</param>
        /// <returns></returns>
        public static string StateChange(string ms)
        {
            string msg = "未知";
            //修改从枚举中去加载对应的状态文本  20180131
            int msInt = 0;
            bool isInt = int.TryParse(ms, out msInt);
            if (isInt)
            {
                msg = EnumHelper.GetEnumDescription((DeviceDataState)msInt);
            }
            //switch (ms)
            //{
            //    case "0":
            //        msg = "通讯中断";
            //        break;
            //    case "1":
            //        msg = "通讯误码";
            //        break;
            //    case "2":
            //        msg = "初始化中";
            //        break;
            //    case "3":
            //        msg = "交流正常";
            //        break;
            //    case "4":
            //        msg = "直流正常";
            //        break;
            //    case "5":
            //        msg = "红外遥控";
            //        break;
            //    case "6":
            //        msg = "设备休眠";
            //        break;
            //    case "7":
            //        msg = "设备检修";
            //        break;
            //    case "8":
            //        msg = "上限预警";
            //        break;
            //    case "9":
            //        msg = "上限预警解除";
            //        break;
            //    case "10":
            //        msg = "上限报警";
            //        break;
            //    case "11":
            //        msg = "上限报警解除";
            //        break;
            //    case "12":
            //        msg = "上限断电";
            //        break;
            //    case "13":
            //        msg = "上限断电解除";
            //        break;
            //    case "14":
            //        msg = "下限预警";
            //        break;
            //    case "15":
            //        msg = "下预预警解除";
            //        break;
            //    case "16":
            //        msg = "下限报警";
            //        break;
            //    case "17":
            //        msg = "下限报警解除";
            //        break;
            //    case "18":
            //        msg = "下限断电";
            //        break;
            //    case "19":
            //        msg = "下断断电解除";
            //        break;
            //    case "20":
            //        msg = "断线";
            //        break;
            //    case "21":
            //        msg = "正常";
            //        break;
            //    case "22":
            //        msg = "上溢";
            //        break;
            //    case "23":
            //        msg = "负漂";
            //        break;
            //    case "24":
            //        msg = "设备标校";
            //        break;
            //    case "25":
            //        msg = "0态";
            //        break;
            //    case "26":
            //        msg = "1态";
            //        break;
            //    case "27":
            //        msg = "2态";
            //        break;
            //    case "28":
            //        msg = "开机";
            //        break;
            //    case "29":
            //        msg = "复电成功";
            //        break;
            //    case "30":
            //        msg = "复电失败";
            //        break;
            //    case "31":
            //        msg = "断电成功";
            //        break;
            //    case "32":
            //        msg = "断电失败";
            //        break;
            //    case "33":
            //        msg = "头子断线";
            //        break;
            //    case "34":
            //        msg = "类型有误";
            //        break;
            //    case "35":
            //        msg = "系统退出";
            //        break;
            //    case "36":
            //        msg = "系统启动";
            //        break;
            //    case "37":
            //        msg = "非法退出";
            //        break;
            //    case "38":
            //        msg = "过滤数据";
            //        break;
            //    case "39":
            //        msg = "热备日志";
            //        break;
            //    case "40":
            //        msg = "满足条件";
            //        break;
            //    case "41":
            //        msg = "不满足条件";
            //        break;
            //    case "42":
            //        msg = "线性突变";
            //        break;
            //    case "43":
            //        msg = "控制量0态";
            //        break;
            //    case "44":
            //        msg = "控制量1态";
            //        break;
            //    case "45":
            //        msg = "控制量断线";
            //        break;
            //    case "46":
            //        msg = "未知";
            //        break;
            //    case "119":
            //        msg = "服务器中断";
            //        break;
            //}
            return msg;
        }

        /// <summary>
        /// 将状态转换成颜色
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Color StateToColor(string ms, string color, bool flg)
        {
            Color cor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
            int icolor = 0;
            Color iico;
            switch (ms)
            {
                case "0":
                case "1":
                case "119":
                case "45":
                    cor = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
                    break;
                case "4":
                    cor = StaticClass.realdataconfig.StateCorCfg.DcColor;
                    break;
                case "24":
                    cor = StaticClass.realdataconfig.StateCorCfg.EffectColor;
                    break;
                case "8":
                    cor = StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor;
                    break;
                case "10":
                    cor = StaticClass.realdataconfig.StateCorCfg.UpAlarmColor;
                    break;
                case "12":
                    cor = StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor;
                    break;
                case "20":
                    cor = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
                    break;
                case "22":
                case "23":
                    cor = StaticClass.realdataconfig.StateCorCfg.OverRangeColor;
                    break;
                case "14":
                    cor = StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor;
                    break;
                case "16":
                    cor = StaticClass.realdataconfig.StateCorCfg.LowAlarmColor;
                    break;
                case "18":
                    cor = StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor;
                    break;
                case "25":
                case "26":
                case "27":
                case "43":
                case "44":
                    int.TryParse(color, out icolor);
                    if (flg)
                    {
                        iico = StaticClass.realdataconfig.StateCorCfg.KAlarmColor;
                    }
                    else
                    {
                        if (icolor != 0)
                        {
                            iico = Color.FromArgb(icolor);
                        }
                        else
                        {
                            iico = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                        }
                    }
                    cor = iico;
                    break;
                //cor = StaticClass.realdataconfig.StateCorCfg.KAlarmColor;
                //break;
            }
            return cor;
        }

        /// <summary>
        /// 获取报警信息
        /// </summary>
        public static void GetBJData()
        {
            Dictionary<long, Jc_BInfo> bj = null;
            List<Jc_BInfo> AlarmList = Model.RealInterfaceFuction.GetBjData();
            if (AlarmList != null && AlarmList.Count > 0)
                bj = AlarmList.ToDictionary(key => long.Parse(key.ID));
            if (bj != null)
            {
                lock (StaticClass.bjobj)
                {
                    StaticClass.jcbdata = bj;
                }
            }
            else
            {
                lock (StaticClass.bjobj)
                {
                    StaticClass.jcbdata.Clear();
                }
            }
        }


        public static void ShowFromText(int n)
        {
            if (StaticClass.updatefromtext != null)
            {
                if (StaticClass.yccount[n] > 0)
                {
                    StaticClass.updatefromtext(n);
                }
            }
        }

    }
}
