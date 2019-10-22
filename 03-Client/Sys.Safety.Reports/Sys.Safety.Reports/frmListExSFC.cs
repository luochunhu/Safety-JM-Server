using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports
{
    public partial class frmListExSFC : XtraForm
    {
        private string _curFieldNameCHS = "";
        private string _strCondition = ""; //条件;
        private string _strFieldType = "";
        private DataTable gridDt;
        private readonly ListExModel Model = new ListExModel();
        private SFCDataTypeEnum sfcType;

        public frmListExSFC()
        {
            BlnOk = false;
            InitializeComponent();
        }

        private void frmListExSFC_Load(object sender, EventArgs e)
        {
            try
            {
                SetFormStyle();
                InitGrid();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetFormStyle()
        {
            var strFieldType = _strFieldType.ToLower();
            if ((strFieldType == "money") || (strFieldType == "decimal") || (strFieldType == "float")
                || (strFieldType == "int") || (strFieldType == "smallint") || (strFieldType == "bigint"))
            {
                layoutControlItem11.Visibility = LayoutVisibility.Never;
                layoutControlItem12.Visibility = LayoutVisibility.Never;
                layoutControlItem13.Visibility = LayoutVisibility.Never;
                layoutControlItem31.Visibility = LayoutVisibility.Never;
                layoutControlItem32.Visibility = LayoutVisibility.Never;
                layoutControlItem33.Visibility = LayoutVisibility.Never;

                sfcType = SFCDataTypeEnum.NumberDataType;

                SetDecimal(spinEdit1);
                SetDecimal(spinEdit2);
            }
            else if ((strFieldType == "smalldatetime") || (strFieldType == "datetime"))
            {
                layoutControlItem11.Visibility = LayoutVisibility.Never;
                layoutControlItem12.Visibility = LayoutVisibility.Never;
                layoutControlItem13.Visibility = LayoutVisibility.Never;
                layoutControlItem21.Visibility = LayoutVisibility.Never;
                layoutControlItem22.Visibility = LayoutVisibility.Never;
                layoutControlItem23.Visibility = LayoutVisibility.Never;

                sfcType = SFCDataTypeEnum.DateTimeDataType;
            }
            else
                //if (strFieldType == "varchar" || strFieldType == "nvarchar" || strFieldType == "nchar" || strFieldType == "char")
            {
                layoutControlItem21.Visibility = LayoutVisibility.Never;
                layoutControlItem22.Visibility = LayoutVisibility.Never;
                layoutControlItem23.Visibility = LayoutVisibility.Never;
                layoutControlItem31.Visibility = LayoutVisibility.Never;
                layoutControlItem32.Visibility = LayoutVisibility.Never;
                layoutControlItem33.Visibility = LayoutVisibility.Never;

                sfcType = SFCDataTypeEnum.StringDataType;
            }
        }

        /// <summary>
        ///     设置小数位数
        /// </summary>
        public void SetDecimal(SpinEdit spinEdit)
        {
            var strWei = TypeUtil.ToInt(RequestUtil.GetParameterValue("CoinDecBit"));
            spinEdit.Properties.DisplayFormat.FormatType = FormatType.Numeric;
            spinEdit.Properties.DisplayFormat.FormatString = "N" + strWei;
            spinEdit.Properties.EditFormat.FormatType = FormatType.Numeric;
            spinEdit.Properties.EditFormat.FormatString = "N" + strWei;
            spinEdit.Properties.Mask.MaskType = MaskType.Numeric;
            spinEdit.Properties.Mask.EditMask = "N" + strWei;
        }

        private void InitGrid()
        {
            // 20170906
            gridDt = new DataTable();
            gridDt.Columns.Add("strOper", Type.GetType("System.String"));
            gridDt.Columns.Add("Value1", Type.GetType("System.String"));
            gridDt.Columns.Add("Value2", Type.GetType("System.String"));
            gridDt.Columns.Add("ApplyRow", Type.GetType("System.Boolean"));
            gridDt.Columns.Add("ColorName", Type.GetType("System.Object"));
            gridDt.Columns.Add("FontColorName", Type.GetType("System.Object"));
            gridDt.Columns.Add("Bold", Type.GetType("System.Boolean"));
            gridDt.Columns.Add("Italic", Type.GetType("System.Boolean"));
            gridDt.Columns.Add("Underline", Type.GetType("System.Boolean"));
            gridDt.Columns.Add("Strikeout", Type.GetType("System.Boolean"));
            gridControl1.DataSource = gridDt;
        }

        private void LoadData()
        {
            if (_strCondition != string.Empty)
            {
                var strTemp = _strCondition.Remove(_strCondition.Length - 1);
                var strConditions = strTemp.Split(new[] {"&&$$"}, StringSplitOptions.RemoveEmptyEntries);

                if (strConditions.Length > 0)
                {
                    DataRow dr = null;
                    var str = string.Empty;
                    var color = Color.White; //背景默认颜色
                    var fontcolor = Color.Black; //字体默认颜色
                    for (var i = 0; i < strConditions.Length; i++)
                    {
                        str = strConditions[i];
                        if (str.Contains("&&$"))
                        {
                            // 20170906
                            //var strs = str.Split(new[] {"&&$"}, StringSplitOptions.RemoveEmptyEntries);
                            var strs = str.Split(new[] { "&&$" }, StringSplitOptions.None);
                            dr = gridDt.NewRow();
                            dr["strOper"] = strs[0];
                            dr["ApplyRow"] = TypeUtil.ToBool(strs[1]);

                            // 20170801
                            //if (sfcType == SFCDataTypeEnum.DateTimeDataType)
                            //{
                            //    //日期类型需特殊处理

                            //    if ((strs.Length > 4) && (strs[2].Substring(0, 4) != "1900"))
                            //        dr["Value1"] = strs[2];
                            //    if ((strs.Length > 5) && (strs[3].Substring(0, 4) != "9999"))
                            //        dr["Value2"] = strs[3];
                            //}
                            //else
                            //{
                            //    if (strs.Length > 4)
                            //        dr["Value1"] = strs[2];
                            //    if (strs.Length > 5)
                            //        dr["Value2"] = strs[3];
                            //}
                            dr["Value1"] = strs[2];
                            if (strs[3] == "##*")
                            {
                                dr["Value2"] = "";
                            }
                            else
                            {
                                dr["Value2"] = strs[3];                                
                            }

                            //背景颜色加载2014-11-18
                            //var ss = strs.Length > 5 ? strs[4].Split(';') : strs[3].Split(';');
                            var ss = strs[4].Split(';');
                            Model.GetColorByString(ss, ref color);
                            dr["ColorName"] = color;

                            //字体颜色设置
                            //var fontss = strs.Length > 5 ? strs[5].Split(';') : strs[4].Split(';');
                            var fontss = strs[5].Split(';');
                            Model.GetColorByString(fontss, ref fontcolor);
                            dr["FontColorName"] = fontcolor;

                            dr["Bold"] = TypeUtil.ToBool(strs[6]);      //粗体

                            //斜体
                            if (strs.Length >= 8)
                            {
                                dr["Italic"] = Convert.ToBoolean(strs[7]);
                            }
                            else
                            {
                                dr["Italic"] = false;
                            }

                            if (strs.Length >= 9)
                            {
                                dr["Underline"] = Convert.ToBoolean(strs[8]);
                            }
                            else
                            {
                                dr["Underline"] = false;
                            }

                            if (strs.Length >= 10)
                            {
                                dr["Strikeout"] = Convert.ToBoolean(strs[9]);
                            }
                            else
                            {
                                dr["Strikeout"] = false;
                            }
                            
                            gridDt.Rows.Add(dr);
                        }
                    }
                }
            }
        }

        private void tlbAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 20171226
                //if (gridDt.Rows.Count >= 2)
                //{
                //    ShowMsg("一个栏目不能超过2个条件设置！", true);
                //    return;
                //}

                var strOper = string.Empty;
                var strValue1 = string.Empty;
                var strValue2 = string.Empty;
                if (sfcType == SFCDataTypeEnum.StringDataType)
                {
                    strOper = TypeUtil.ToString(comboString.EditValue);
                    strValue1 = this.strValue1.Text.Trim();
                    strValue2 = this.strValue2.Text.Trim();
                }
                else if (sfcType == SFCDataTypeEnum.NumberDataType)
                {
                    strOper = TypeUtil.ToString(comboNumber.EditValue);
                    strValue1 = TypeUtil.ToString(spinEdit1.EditValue);
                    strValue2 = TypeUtil.ToString(spinEdit2.EditValue);
                }
                else
                {
                    strOper = TypeUtil.ToString(comboDateTime.EditValue);
                    if (dateEdit1.EditValue != null)
                        strValue1 = TypeUtil.ToString(dateEdit1.EditValue);
                    if (dateEdit2.EditValue != null)
                        strValue2 = TypeUtil.ToString(dateEdit2.EditValue);
                }

                if (strOper == string.Empty)
                {
                    ShowMsg("操作符不能为空！", true);
                    return;
                }

                var dr = gridDt.NewRow();
                dr["strOper"] = strOper;
                dr["Value1"] = strValue1;

                if (string.IsNullOrEmpty(strValue1))
                {
                    ShowMsg("值1不能为空！", true);
                    return;
                }
                if (strOper == "介于" || strOper == "不介于")
                {
                    if (string.IsNullOrEmpty(strValue2))
                    {
                        ShowMsg("值2不能为空！", true);
                        return;
                    }
                    dr["Value2"] = strValue2;
                }
                else
                {
                    dr["Value2"] = "";                    
                }

                dr["ApplyRow"] = chkApplyRow.Checked;
                dr["ColorName"] = Color.Red;
                dr["FontColorName"] = Color.Black;
                dr["Bold"] = checkEditBold.Checked;
                dr["Italic"] = checkEditItalic.Checked;
                dr["Underline"] = checkEditUnderline.Checked;
                dr["Strikeout"] = checkEditStrikeout.Checked;

                gridDt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rowHandle = gridView1.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MessageShowUtil.ShowInfo("请选择要删除的行！");
                    return;
                }
                if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前行吗？"))
                    return;

                gridView1.DeleteRow(rowHandle);

                gridView1_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbOK_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                gridView1.CloseEditor();
                gridView1.UpdateCurrentRow();
                var strReturn = string.Empty;
                var str = string.Empty;
                string strOper;
                string strApplyRow;
                string strValue1;
                string strValue2;
                string strColorName;
                for (var i = 0; i < gridDt.Rows.Count; i++)
                {
                    strOper = TypeUtil.ToString(gridDt.Rows[i]["strOper"]);
                    strApplyRow = TypeUtil.ToString(gridDt.Rows[i]["ApplyRow"]);
                    strValue1 = TypeUtil.ToString(gridDt.Rows[i]["Value1"]);
                    strValue2 = TypeUtil.ToString(gridDt.Rows[i]["Value2"]);

                    // 20170801
                    //if (sfcType == SFCDataTypeEnum.DateTimeDataType)
                    //{
                    //    //日期类型需特殊处理

                    //    if (string.IsNullOrEmpty(strValue1))
                    //        strValue1 = "1900-01-01";
                    //    if (string.IsNullOrEmpty(strValue2))
                    //        strValue2 = "9999-12-31";
                    //}

                    //if ((strOper == "介于") || (strOper == "不介于"))
                    //    str = strOper + "&&$" + strApplyRow + "&&$" + strValue1 + "&&$" + strValue2;
                    //else
                    //    str = strOper + "&&$" + strApplyRow + "&&$" + strValue1;
                    if ((strOper == "介于") || (strOper == "不介于"))
                        str = strOper + "&&$" + strApplyRow + "&&$" + strValue1 + "&&$" + strValue2;
                    else
                        str = strOper + "&&$" + strApplyRow + "&&$" + strValue1 + "&&$##*";

                    if (TypeUtil.ToString(gridView1.GetRowCellValue(i, "ColorName")) == "")
                    {
                        ShowMsg("第" + (i + 1) + "行未选择背景颜色，请选择", true);
                        return;
                    }
                    
                    //得到背景颜色
                    var c = (Color) gridView1.GetRowCellValue(i, "ColorName");
                    if (c != null)
                    {
                        var strColorType = ""; //颜色类型1代表自定义,2代表网页,3代表系统（就是点击颜色对话框出来后有三个标签选择颜色）
                        if ((c.IsEmpty == false) && (c.IsKnownColor == false) && (c.IsNamedColor == false) &&
                            (c.IsSystemColor == false))
                            strColorType = "1";
                        if ((c.IsEmpty == false) && c.IsKnownColor && c.IsNamedColor && (c.IsSystemColor == false))
                            strColorType = "2";
                        if ((c.IsEmpty == false) && c.IsKnownColor && c.IsNamedColor && c.IsSystemColor)
                            strColorType = "3";
                        str += "&&$" + strColorType + ";" + c.A + ";" + c.B + ";" + c.G + ";" + c.R + ";" + c.Name;
                    }

                    //得到字体颜色
                    c = (Color) gridView1.GetRowCellValue(i, "FontColorName");
                    if (c != null)
                    {
                        var strColorType = ""; //颜色类型1代表自定义,2代表网页,3代表系统（就是点击颜色对话框出来后有三个标签选择颜色）
                        if ((c.IsEmpty == false) && (c.IsKnownColor == false) && (c.IsNamedColor == false) &&
                            (c.IsSystemColor == false))
                            strColorType = "1";
                        if ((c.IsEmpty == false) && c.IsKnownColor && c.IsNamedColor && (c.IsSystemColor == false))
                            strColorType = "2";
                        if ((c.IsEmpty == false) && c.IsKnownColor && c.IsNamedColor && c.IsSystemColor)
                            strColorType = "3";
                        str += "&&$" + strColorType + ";" + c.A + ";" + c.B + ";" + c.G + ";" + c.R + ";" + c.Name;
                    }
                    else
                        str += "&&$" + "NOColor";

                    //设置粗体
                    string sBold = TypeUtil.ToString(gridDt.Rows[i]["Bold"]);
                    str += "&&$" + sBold;

                    //设置斜体
                    string sItalic = TypeUtil.ToString(gridDt.Rows[i]["Italic"]);
                    str += "&&$" + sItalic;

                    //设置下划线
                    string sUnderline = TypeUtil.ToString(gridDt.Rows[i]["Underline"]);
                    str += "&&$" + sUnderline;

                    //设置删除线
                    string sStrikeout = TypeUtil.ToString(gridDt.Rows[i]["Strikeout"]);
                    str += "&&$" + sStrikeout;

                    strReturn += str + "&&$$";                    
                }

                if (strReturn != string.Empty)
                {
                    strReturn = strReturn.Remove(strReturn.Length - 4);

                    //添加字段数据类型
                    if (sfcType == SFCDataTypeEnum.StringDataType)
                        strReturn += "1";
                    else if (sfcType == SFCDataTypeEnum.NumberDataType)
                        strReturn += "2";
                    else if (sfcType == SFCDataTypeEnum.DateTimeDataType)
                        strReturn += "3";
                }

                _strCondition = strReturn;

                BlnOk = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            BlnOk = false;
            Close();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            // 20170727
            //try
            //{
            //    UnRegistereEvent();

            //    var focusedRowHandle = gridView1.FocusedRowHandle;
            //    var strOper = TypeUtil.ToString(gridView1.GetRowCellValue(focusedRowHandle, "strOper"));
            //    if (sfcType == SFCDataTypeEnum.StringDataType)
            //    {
            //        comboString.EditValue = strOper;
            //        strValue1.Text = TypeUtil.ToString(gridView1.GetRowCellValue(focusedRowHandle, "Value1"));
            //        strValue2.Text = TypeUtil.ToString(gridView1.GetRowCellValue(focusedRowHandle, "Value2"));

            //        SetComboStringStyle(strOper);
            //    }
            //    else if (sfcType == SFCDataTypeEnum.NumberDataType)
            //    {
            //        comboNumber.EditValue = strOper;
            //        spinEdit1.EditValue = TypeUtil.ToDecimal(gridView1.GetRowCellValue(focusedRowHandle, "Value1"));
            //        spinEdit2.EditValue = TypeUtil.ToDecimal(gridView1.GetRowCellValue(focusedRowHandle, "Value2"));

            //        SetComboNumberStyle(strOper);
            //    }
            //    else
            //    {
            //        comboDateTime.EditValue = strOper;
            //        var strValue1 = TypeUtil.ToString(gridView1.GetRowCellValue(focusedRowHandle, "Value1"));
            //        var strValue2 = TypeUtil.ToString(gridView1.GetRowCellValue(focusedRowHandle, "Value2"));
            //        if (strValue1 != string.Empty)
            //            dateEdit1.EditValue = TypeUtil.ToDateTime(strValue1);
            //        if (strValue2 != string.Empty)
            //            dateEdit2.EditValue = TypeUtil.ToDateTime(strValue2);

            //        SetComboDateTimeStyle(strOper);
            //    }

            //    chkApplyRow.Checked = TypeUtil.ToBool(gridView1.GetRowCellValue(focusedRowHandle, "ApplyRow"));

            //    RegistereEvent();
            //}
            //catch (Exception ex)
            //{
            //    MessageShowUtil.ShowErrow(ex);
            //}
        }


        /// <summary>
        ///     显示信息
        /// </summary>
        /// <param name="caption">信息串</param>
        private void ShowMsg(string caption)
        {
            staticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + caption;
        }

        /// <summary>
        ///     显示信息
        /// </summary>
        /// <param name="caption">信息串</param>
        private void ShowMsg(string caption, bool isMsg)
        {
            staticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + caption;
            if (isMsg) MessageBox.Show(caption, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void RegistereEvent()
        {
            if (sfcType == SFCDataTypeEnum.StringDataType)
            {
                comboString.EditValueChanged += comboString_EditValueChanged;
                strValue1.EditValueChanged += Value1_EditValueChanged;
                strValue2.EditValueChanged += Value2_EditValueChanged;
            }
            else if (sfcType == SFCDataTypeEnum.NumberDataType)
            {
                comboNumber.EditValueChanged += comboNumber_EditValueChanged;
                spinEdit1.EditValueChanged += spinValue1_EditValueChanged;
                spinEdit2.EditValueChanged += spinEdit2_EditValueChanged;
            }
            else
            {
                comboDateTime.EditValueChanged += comboDateTime_EditValueChanged;
                dateEdit1.EditValueChanged += dateEdit1_EditValueChanged;
                dateEdit2.EditValueChanged += dateEdit2_EditValueChanged;
            }
        }

        private void UnRegistereEvent()
        {
            if (sfcType == SFCDataTypeEnum.StringDataType)
            {
                comboString.EditValueChanged -= comboString_EditValueChanged;
                strValue1.EditValueChanged -= Value1_EditValueChanged;
                strValue2.EditValueChanged -= Value2_EditValueChanged;
            }
            else if (sfcType == SFCDataTypeEnum.NumberDataType)
            {
                comboNumber.EditValueChanged -= comboNumber_EditValueChanged;
                spinEdit1.EditValueChanged -= spinValue1_EditValueChanged;
                spinEdit2.EditValueChanged -= spinEdit2_EditValueChanged;
            }
            else
            {
                comboDateTime.EditValueChanged -= comboDateTime_EditValueChanged;
                dateEdit1.EditValueChanged -= dateEdit1_EditValueChanged;
                dateEdit2.EditValueChanged -= dateEdit2_EditValueChanged;
            }
        }

        private void comboString_EditValueChanged(object sender, EventArgs e)
        {
            var strOper = comboString.Text.Trim();
            SetComboStringStyle(strOper);

            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "strOper", strOper);
        }

        private void SetComboStringStyle(string strOper)
        {
            if ((strOper == "等于") || (strOper == "大于") || (strOper == "小于") ||
                (strOper == "不等于") || (strOper == "大于等于") || (strOper == "小于等于"))
            {
                layoutControlItem12.Text = " ";
                layoutControlItem12.Visibility = LayoutVisibility.Always;
                layoutControlItem13.Visibility = LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem12.Text = "从";
                layoutControlItem13.Text = "到";
                layoutControlItem12.Visibility = LayoutVisibility.Always;
                layoutControlItem13.Visibility = LayoutVisibility.Always;
            }
        }

        private void Value1_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "Value1", strValue1.Text.Trim());
        }

        private void Value2_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "Value2", strValue2.Text.Trim());
        }

        private void comboNumber_EditValueChanged(object sender, EventArgs e)
        {
            
            var strOper = comboNumber.Text.Trim();
            SetComboNumberStyle(strOper);

            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "strOper", strOper);
        }

        private void SetComboNumberStyle(string strOper)
        {
            if ((strOper == "等于") || (strOper == "大于") || (strOper == "小于") ||
                (strOper == "不等于") || (strOper == "大于等于") || (strOper == "小于等于"))
            {
                layoutControlItem22.Text = " ";
                layoutControlItem22.Visibility = LayoutVisibility.Always;                
                layoutControlItem23.Visibility = LayoutVisibility.Never;
                spinEdit1.Value = 0;
                spinEdit2.Value = 0;
            }
            else
            {
                layoutControlItem22.Text = "从";
                layoutControlItem23.Text = "到";
                layoutControlItem22.Visibility = LayoutVisibility.Always;
                layoutControlItem23.Visibility = LayoutVisibility.Always;
                spinEdit1.Value = 0;
                spinEdit2.Value = 0;
            }
        }

        private void spinValue1_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "Value1", spinEdit1.Text.Trim());
        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "Value2", spinEdit2.Text.Trim());
        }

        private void comboDateTime_EditValueChanged(object sender, EventArgs e)
        {
            var strOper = comboDateTime.Text.Trim();
            SetComboDateTimeStyle(strOper);

            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "strOper", strOper);
        }

        private void SetComboDateTimeStyle(string strOper)
        {
            if ((strOper == "等于") || (strOper == "大于") || (strOper == "小于") ||
                (strOper == "不等于") || (strOper == "大于等于") || (strOper == "小于等于"))
            {
                layoutControlItem32.Text = " ";
                layoutControlItem32.Visibility = LayoutVisibility.Always;
                layoutControlItem33.Visibility = LayoutVisibility.Never;
            }
            else
            {
                layoutControlItem32.Text = "从";
                layoutControlItem33.Text = "到";
                layoutControlItem32.Visibility = LayoutVisibility.Always;
                layoutControlItem33.Visibility = LayoutVisibility.Always;
            }
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    if (dateEdit1.EditValue != null)
            //        gridView1.SetRowCellValue(rowHandle, "Value1",
            //            TypeUtil.ToDateTime(dateEdit1.EditValue).ToString("yyyy-MM-dd"));
            //    else
            //        gridView1.SetRowCellValue(rowHandle, "Value1", "");
        }

        private void dateEdit2_EditValueChanged(object sender, EventArgs e)
        {
            // 20170727
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    if (dateEdit2.EditValue != null)
            //        gridView1.SetRowCellValue(rowHandle, "Value2",
            //            TypeUtil.ToDateTime(dateEdit2.EditValue).ToString("yyyy-MM-dd"));
            //    else
            //        gridView1.SetRowCellValue(rowHandle, "Value2", "");
        }

        private void chkApplyRow_CheckedChanged(object sender, EventArgs e)
        {
            //var rowHandle = gridView1.FocusedRowHandle;
            //if (rowHandle >= 0)
            //    gridView1.SetRowCellValue(rowHandle, "ApplyRow", chkApplyRow.Checked);
        }

        #region 属性设置

        /// <summary>
        ///     设置条件的字段中文名
        /// </summary>
        public string CurFieldNameCHS
        {
            set { _curFieldNameCHS = value; }
        }

        /// <summary>
        ///     字段类型
        /// </summary>
        public string StrFieldType
        {
            set { _strFieldType = value; }
        }

        /// <summary>
        ///     所选条件
        /// </summary>
        public string StrCondition
        {
            get { return _strCondition; }
            set { _strCondition = value; }
        }

        /// <summary>
        ///     是否确定
        /// </summary>
        public bool BlnOk { get; set; }

        #endregion
    }

    /// <summary>
    ///     条件格式数据类型枚举
    /// </summary>
    public enum SFCDataTypeEnum
    {
        /// <summary>
        ///     字符型
        /// </summary>
        StringDataType = 1,

        /// <summary>
        ///     数字型
        /// </summary>
        NumberDataType = 2,

        /// <summary>
        ///     日期型
        /// </summary>
        DateTimeDataType = 3
    }
}