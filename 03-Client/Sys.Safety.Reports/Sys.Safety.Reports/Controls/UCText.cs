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
    public partial class UCText : DevExpress.XtraEditors.XtraUserControl
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
                    this.cboCondition.EditValueChanged -= new EventHandler(this.cboCondition_EditValueChanged);
                    this.txtValue1.EditValueChanged -= new EventHandler(this.txtValue1_EditValueChanged);
                    this.txtValue2.EditValueChanged -= new EventHandler(this.txtValue2_EditValueChanged);

                    if (_strKeyValue.Contains("&&$"))
                    {
                        string[] strs = _strKeyValue.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                        this.cboCondition.Text = strs[0];
                        if (strs.Length > 1) this.txtValue1.Text = strs[1];
                        if (strs.Length > 2) this.txtValue2.Text = strs[2];
                    }
                    else
                    {
                        if (this._strKeyValue == "所有")
                        {
                            this._strKeyValue = "";
                        }
                        this.cboCondition.Text = _strKeyValue;
                    }

                    this.SetUIStyle();
                }
                catch
                { }
                finally
                {
                    this.cboCondition.EditValueChanged += new System.EventHandler(this.cboCondition_EditValueChanged);
                    this.txtValue1.EditValueChanged += new EventHandler(this.txtValue1_EditValueChanged);
                    this.txtValue2.EditValueChanged += new EventHandler(this.txtValue2_EditValueChanged);
                }
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCText()
        {
        }

        public UCText(string name)
        {           
            InitializeComponent();
            layoutControlItem1.Text = name;         
        }


        private void UCText_Load(object sender, EventArgs e)
        {
            SetTag();
        }

        /// <summary>
        /// 保存条件到TAG里

        /// </summary>
        private void SetTag()
        {
            try
            {
                string str;
                string str1 = this.txtValue1.Text;
                string str2 = this.txtValue2.Text;
                string strCboText = cboCondition.Text.Trim();
           
               if (strCboText == "介于")
                {
                    str = strCboText + "&&$" + str1 + "&&$" + str2;
                }
                else if (strCboText != "" && strCboText != "所有" && strCboText != "空值")
                {
                    str = strCboText + "&&$" + str1;
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

        private void cboCondition_EditValueChanged(object sender, EventArgs e)
        {
            SetUIStyle();
            SetTag();
        }


        private void txtValue1_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }

        private void txtValue2_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }
       

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        private void SetUIStyle()
        {
            string strCboText = this.cboCondition.Text.Trim();
            this.txtValue1.EditValueChanged  -= new EventHandler(this.txtValue1_EditValueChanged);
            this.txtValue2.EditValueChanged  -= new EventHandler(this.txtValue2_EditValueChanged);

            if (strCboText == "" || strCboText == "所有" || strCboText == "空值")
            {           
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else if (strCboText == "介于")
            {      
                this.layoutControlItem2.Text  = "从";               
                this.layoutControlItem3.Text = "到";
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {       
                this.layoutControlItem2.Text = " ";             
                this.layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            this.txtValue1.EditValueChanged += new EventHandler(this.txtValue1_EditValueChanged);
            this.txtValue2.EditValueChanged += new EventHandler(this.txtValue2_EditValueChanged);
        }

    }
}
