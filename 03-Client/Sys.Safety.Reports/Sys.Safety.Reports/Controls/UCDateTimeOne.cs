using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;


namespace Sys.Safety.Reports.Controls
{
    public partial class UCDateTimeOne : DevExpress.XtraEditors.XtraUserControl
    {
        private string _strKeyValue = "";

        /// <summary>
        /// 设置条件的初始值

        /// </summary>
        public string HeaderValue
        {
            set
            {
                _strKeyValue = value;
                try
                {
                    this.cboDate.EditValueChanged -= new EventHandler(this.cboDate_EditValueChanged);
                    this.datFrom.EditValueChanged -= new EventHandler(this.datFrom_EditValueChanged);
                    this.datTo.EditValueChanged -= new EventHandler(this.datTo_EditValueChanged);

                    if (_strKeyValue.Contains("&&$"))
                    {
                        string[] strs = _strKeyValue.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                        this.cboDate.Text = strs[0];
                        if (strs.Length > 1 && strs[1].Substring(0, 4) != "1900")
                        {
                            this.datFrom.Text = strs[1].Substring(0, 10);
                        }
                        if (strs.Length > 2 && strs[2].Substring(0, 4) != "9999")
                        {
                            this.datTo.Text = strs[2].Substring(0, 10);
                        }
                    }
                    else
                    {
                        if (this._strKeyValue == "所有")
                        {
                            this._strKeyValue = "";
                        }
                        this.cboDate.Text = _strKeyValue;
                    }

                    this.SetUIStyle();     
                }
                catch
                { }
                finally
                {
                    this.cboDate.EditValueChanged += new System.EventHandler(this.cboDate_EditValueChanged);
                    this.datFrom.EditValueChanged += new EventHandler(this.datFrom_EditValueChanged);
                    this.datTo.EditValueChanged += new EventHandler(this.datTo_EditValueChanged);
                }     
            }
        }


        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCDateTimeOne()
        {
        }

        public UCDateTimeOne(string name)
        {           
            InitializeComponent();

            this.layoutControlItem3.Text = name;
        }

        private void UCDateTimeOne_Load(object sender, EventArgs e)
        {
            SetTag();
        }

        private void cboDate_EditValueChanged(object sender, EventArgs e)
        {
            this.SetUIStyle();
            SetTag();
        }

        /// <summary>
        /// 设置显示样式
        /// </summary>
        private void SetUIStyle()
        {
            this.datFrom.EditValueChanged -= new EventHandler(this.datFrom_EditValueChanged);
            this.datTo.EditValueChanged -= new EventHandler(this.datTo_EditValueChanged);

            string strCboText = cboDate.Text.Trim();
            if (strCboText == "等于" || strCboText == "大于" || strCboText == "小于" ||
                 strCboText == "不等于" || strCboText == "大于等于" || strCboText == "小于等于")
            {
                this.layoutControlItem1.Text = " ";
                this.layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else if (strCboText == "介于")
            {
                this.layoutControlItem1.Text = "从";
                this.layoutControlItem2.Text = "到";
                this.layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                this.layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            this.datFrom.EditValueChanged += new EventHandler(this.datFrom_EditValueChanged);
            this.datTo.EditValueChanged += new EventHandler(this.datTo_EditValueChanged);
        }

        private void datFrom_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }

        private void datTo_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        } 

        /// <summary>
        /// 保存条件到TAG里//操作符&&$NumRangle1&&$NumRangle2
        /// </summary>
        private void SetTag()
        {
            try
            {
                string str;
                string datFrom = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                string datTo = this.datTo.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                string strCboText = cboDate.Text.Trim();
                if (this.datFrom.EditValue == null)
                {
                    datFrom = "1900-01-01 00:00:00";
                }
                if (this.datTo.EditValue == null)
                {
                    datTo = "9999-12-31 23:59:59";
                }
                if (strCboText == "等于" || strCboText == "大于" || strCboText == "小于" ||
                       strCboText == "不等于" || strCboText == "大于等于" || strCboText == "小于等于")
                {
                    str = strCboText + "&&$" + datFrom;
                }
                else if (strCboText == "介于")
                {
                    str = strCboText + "&&$" + datFrom + "&&$" + datTo;
                }
                else
                {
                    str = strCboText;
                }

                _strKeyValue = str;
                if (this._strKeyValue == string.Empty)
                {
                    this._strKeyValue = "所有";
                }

                if (((Hashtable)this.Tag).ContainsKey("_strKeyValue"))
                {
                    ((Hashtable)this.Tag)["_strKeyValue"] = this._strKeyValue;
                }
                else
                {
                    ((Hashtable)this.Tag).Add("_strKeyValue", this._strKeyValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
