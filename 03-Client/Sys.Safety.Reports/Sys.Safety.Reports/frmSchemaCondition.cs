using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Reports.Controls;

namespace Sys.Safety.Reports
{
    public partial class frmSchemaCondition : XtraForm
    {
        private string _curFieldNameCHS = "";
        private string _fkCode = string.Empty; //参照编码      
        private int _lngNum;
        private string _strCondition = ""; //条件
        private string _strConditionCHS = "";
        private string _strFieldType = "";


        public frmSchemaCondition()
        {
            BlnOk = false;
            InitializeComponent();
        }

        private void frmSchemaCondition_Load(object sender, EventArgs e)
        {
            try
            {
                if (_strCondition != string.Empty)
                {
                    string[] strConditions = {};
                    string[] strConditionChs = {};
                    if (_strCondition.Contains("&&$$"))
                        strConditions = _strCondition.Split(new[] {"&&$$"}, StringSplitOptions.RemoveEmptyEntries);
                    if (_strConditionCHS.Contains("&&$$"))
                        strConditionChs = _strConditionCHS.Split(new[] {"&&$$"}, StringSplitOptions.RemoveEmptyEntries);

                    if (strConditions.Length > 0)
                        if (strConditionChs.Length > 0)
                            for (var i = 0; i < strConditions.Length; i++)
                                CreateConditionControl(strConditions[i], strConditionChs[i]);
                        else
                            for (var i = 0; i < strConditions.Length; i++)
                                CreateConditionControl(strConditions[i], string.Empty);
                    else
                        CreateConditionControl(_strCondition, _strConditionCHS);
                }
                else
                {
                    CreateConditionControl(string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     添加条件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CreateConditionControl(string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     删除条件
        /// </summary>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_lngNum < 1)
                {
                    MessageShowUtil.ShowInfo("没有可删除的条件");
                    return;
                }

                _lngNum--;
                Controls.RemoveAt(Controls.Count - 1);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     组织条件数据
        /// </summary>
        /// <remarks>
        ///     条件格式：
        ///     操作符&&$NumRangle1&&$NumRangle2
        ///     Condition1&&$$Condition21&&$$Condition3
        /// </remarks>
        private void btnOk_Click(object sender, EventArgs e)
        {
            //组织条件
            var strKey = "";
            var strDisplay = "";
            var conditon = "";
            var conditonchs = "";
            for (var i = 0; i < Controls.Count; i++)
            {
                strKey = "";
                strDisplay = "";
                if ((Controls[i].Name != "btnAdd") && (Controls[i].Name != "btnDel")
                    && (Controls[i].Name != "btnOk") && (Controls[i].Name != "btnCancel"))
                {
                    var hash = (Hashtable) Controls[i].Tag;
                    if (Controls[i] is UCRef)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) == string.Empty)
                            continue;
                        strKey += "等于&&$" + TypeUtil.ToString(hash["_strKeyValue"]);
                        strDisplay += "等于&&$" + TypeUtil.ToString(hash["_strDisplayValue"]);
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strKey = TypeUtil.ToString(hash["_strKeyValue"]);
                    }
                }
                if (strKey != string.Empty)
                {
                    if (conditon != string.Empty)
                        conditon += "&&$$" + strKey;
                    else
                        conditon = strKey;

                    if (strDisplay != string.Empty)
                        if (conditonchs != string.Empty)
                            conditonchs += "&&$$" + strDisplay;
                        else
                            conditonchs = strDisplay;
                }
            }

            _strCondition = conditon;
            _strConditionCHS = conditonchs;
            if (_strConditionCHS == string.Empty)
                _strConditionCHS = conditon;

            BlnOk = true;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     设置提示信息
        /// </summary>
        /// <param name="strInfo"></param>
        private void SetInfo(string strInfo)
        {
            MessageBox.Show(strInfo, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        ///     获取条件标题
        /// </summary>
        /// <returns>string</returns>
        private string GetConditionTitle()
        {
            var titleName = _curFieldNameCHS;
            if (_lngNum > 1)
                titleName = "或".PadLeft(2*_curFieldNameCHS.Length, ' ');

            return titleName;
        }

        private void CreateConditionControl(string strValPara, string strDisplayPara)
        {
            if (_lngNum >= 2)
            {
                SetInfo("最多能添加2个条件");
                return;
            }

            try
            {
                _lngNum++;

                if (_fkCode != string.Empty)
                {
                    CreateRefControl(strValPara, strDisplayPara);
                    return;
                }

                var strFieldType = _strFieldType.ToLower();
                if ((strFieldType == "varchar") || (strFieldType == "nvarchar") || (strFieldType == "nchar") ||
                    (strFieldType == "char"))
                    CreateTextControl(strValPara, strDisplayPara);
                else if ((strFieldType == "money") || (strFieldType == "decimal") || (strFieldType == "float")
                         || (strFieldType == "int") || (strFieldType == "smallint") || (strFieldType == "bigint") ||
                         (strFieldType == "tinyint") || strFieldType == "real")
                    CreateNumberControl(strValPara, strDisplayPara);
                else if (strFieldType == "bit")
                    CreateBoolControl(strValPara, strDisplayPara);
                else if ((strFieldType == "smalldatetime") || (strFieldType == "datetime"))
                    CreateDateTimeControl(strValPara, strDisplayPara);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     创建参照型控件
        /// </summary>
        private void CreateRefControl(string strValPara, string strDisplayPara)
        {
            try
            {
                var titleName = GetConditionTitle();
                var control = new UCRef(titleName);
                var hash = new Hashtable();
                hash.Add("_strKeyValue", strValPara);
                hash.Add("_strDisplayValue", strDisplayPara);
                control.Tag = hash;
                control.FkCode = _fkCode;
                control.Height += 5;
                control.Location = new Point(5, CountHight());
                Controls.Add(control);

                control.StrKeyValue = strValPara;
                control.StrDisplayValue = strDisplayPara;
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建参照控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建布尔型控件
        /// </summary>
        private void CreateBoolControl(string strValPara, string strDisplayPara)
        {
            try
            {
                var titleName = GetConditionTitle();
                var control = new UCBoolean(titleName);
                var hash = new Hashtable();
                hash.Add("_strKeyValue", strValPara);
                control.Tag = hash;
                control.StrKeyValue = strValPara;
                control.Height += 5;
                control.Location = new Point(5, CountHight());
                Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建Bool型控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建整型控件
        /// </summary>
        private void CreateNumberControl(string strValPara, string strDisplayPara)
        {
            try
            {
                var titleName = GetConditionTitle();
                var control = new UCNumber(titleName);
                var hash = new Hashtable();
                hash.Add("_strKeyValue", strValPara);
                control.HeaderValue = strValPara;
                control.Tag = hash;
                control.Height += 5;
                control.Location = new Point(5, CountHight());
                Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建整型控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建日期型控件
        /// </summary>
        private void CreateDateTimeControl(string strValPara, string strDisplayPara)
        {
            try
            {
                var titleName = GetConditionTitle();
                var control = new UCDateTimeOne(titleName);
                var hash = new Hashtable();
                hash.Add("_strKeyValue", strValPara);
                control.HeaderValue = strValPara;
                control.Tag = hash;
                control.Height += 5;
                control.Location = new Point(5, CountHight());
                Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建日期型控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建字符型控件
        /// </summary>
        private void CreateTextControl(string strValPara, string strDisplayPara)
        {
            try
            {
                var titleName = GetConditionTitle();
                var control = new UCText(titleName);
                var hash = new Hashtable();
                hash.Add("_strKeyValue", strValPara);
                control.HeaderValue = strValPara;
                control.Tag = hash;
                control.Height += 5;
                control.Location = new Point(5, CountHight());
                Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建字符串型控件出错！\n" + e.Message);
            }
        }

        private int CountHight()
        {
            var hightCounter = 0;
            for (var i = 0; i < Controls.Count; i++)
                if ((Controls[i].Name != "btnAdd") && (Controls[i].Name != "btnDel")
                    && (Controls[i].Name != "btnOk") && (Controls[i].Name != "btnCancel"))
                    hightCounter += Controls[i].Height;
            return hightCounter + 5;
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
        ///     设置参照编码
        /// </summary>
        public string FkCode
        {
            set { _fkCode = value; }
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
        ///     所选条件显示数据
        /// </summary>
        public string StrConditionCHS
        {
            get { return _strConditionCHS; }
            set { _strConditionCHS = value; }
        }

        /// <summary>
        ///     是否确定
        /// </summary>
        public bool BlnOk { get; set; }

        #endregion
    }
}