using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;

namespace Sys.Safety.Reports
{
    public partial class frmSchemaAddCalc : XtraForm
    {
        private ListmetadataInfo _lmd = new ListmetadataInfo();
        private string _strFieldDispaly = "";

        public frmSchemaAddCalc()
        {
            BlnOk = false;
            SelectedDt = null;
            BlnEdit = false;
            InitializeComponent();
        }

        /// <summary>
        ///     编辑状态
        /// </summary>
        public bool BlnEdit { get; set; }

        /// <summary>
        ///     选择栏目数据源
        /// </summary>
        public DataTable SelectedDt { get; set; }

        public ListmetadataInfo Lmd
        {
            get { return _lmd; }
            set { _lmd = value; }
        }

        public string StrFieldDispaly
        {
            get { return _strFieldDispaly; }
            set { _strFieldDispaly = value; }
        }

        public bool BlnOk { get; set; }

        private void frmSchemaAddCalc_Load(object sender, EventArgs e)
        {
            try
            {
                SetFieldTypeData();

                lookUpFieldType.EditValue = _lmd.StrFieldType;
                txtFieldName.Text = _lmd.MetaDataFieldName;
                txtDisplayName.Text = _strFieldDispaly;
                memoEditFormula.Text = _lmd.StrFormula;
                listBoxControl1.ValueMember = "MetaDataFieldName";
                listBoxControl1.DisplayMember = "strListDisplayFieldNameCHS";
                SelectedDt.DefaultView.RowFilter = "isnull(blnSysProcess,0)=0 and isnull(isCalcField,0)=0";
                listBoxControl1.DataSource = SelectedDt;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetFieldTypeData()
        {
            var fieldTypeDt = new DataTable();
            fieldTypeDt.Columns.Add(new DataColumn("Code", Type.GetType("System.String")));
            fieldTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<string, string> _dic = new Dictionary<string, string>();
            _dic.Add("varchar", "字符串");
            _dic.Add("decimal", "数值");
            _dic.Add("datetime", "日期时间");
            _dic.Add("bit", "逻辑值");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = fieldTypeDt.NewRow();
                dr["Code"] = key;
                dr["Name"] = _dic[key];
                fieldTypeDt.Rows.Add(dr);
            }

            lookUpFieldType.Properties.DataSource = fieldTypeDt;
        }

        private void listBoxControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var str = TypeUtil.ToString(listBoxControl1.SelectedValue);
                memoEditFormula.Text += str;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     清空
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            memoEditFormula.Text = "";
        }

        /// <summary>
        ///     确定
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                _lmd.StrFieldType = TypeUtil.ToString(lookUpFieldType.EditValue);
                if (_lmd.StrFieldType == string.Empty)
                {
                    MessageShowUtil.ShowInfo("栏目类型不能为空，请重新输入");
                    lookUpFieldType.Focus();
                    return;
                }

                var str = txtFieldName.Text.Trim();
                if (str == string.Empty)
                {
                    MessageShowUtil.ShowInfo("栏目别名不能为空，请重新输入");
                    txtFieldName.Focus();
                    return;
                }
                if (!BlnEdit || (str != _lmd.MetaDataFieldName))
                    if (SelectedDt.Select("MetaDataFieldName='" + str + "'").Length > 0)
                    {
                        MessageShowUtil.ShowInfo("栏目别名已经存在，请重新输入");
                        txtFieldName.Focus();
                        return;
                    }
                _lmd.MetaDataFieldName = str; //别名

                _strFieldDispaly = txtDisplayName.Text.Trim(); //显示名
                if (_strFieldDispaly == string.Empty)
                {
                    MessageShowUtil.ShowInfo("显示名称不能为空，请重新输入");
                    txtDisplayName.Focus();
                    return;
                }

                _lmd.StrFormula = memoEditFormula.Text.Trim(); //计算公式
                _lmd.StrRefColList = ""; //引用栏目列表
                if (_lmd.StrFormula == string.Empty)
                {
                    MessageShowUtil.ShowInfo("表达式不能为空，请重新输入");
                    memoEditFormula.Focus();
                    return;
                }

                var rowCount = SelectedDt.Rows.Count;
                for (var i = 0; i < rowCount; i++)
                {
                    str = TypeUtil.ToString(SelectedDt.Rows[i]["MetaDataFieldName"]);
                    if (_lmd.StrFormula.Contains(str))
                        _lmd.StrRefColList = _lmd.StrRefColList + str + ",";
                }
                if (_lmd.StrRefColList != string.Empty)
                    _lmd.StrRefColList = _lmd.StrRefColList.Remove(_lmd.StrRefColList.Length - 1);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }


            BlnOk = true;
            Close();
        }

        /// <summary>
        ///     取消
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            BlnOk = false;
            Close();
        }
    }
}