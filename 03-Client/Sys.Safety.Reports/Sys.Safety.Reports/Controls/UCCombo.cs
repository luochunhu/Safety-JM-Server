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
    public partial class UCCombo : DevExpress.XtraEditors.XtraUserControl
    {
        private string _headerValue = "";
        /// <summary>
        /// 设置条件的初始值

        /// </summary>
        public string HeaderValue
        {
            set
            {
                _headerValue = value;
                this.cboBool.Text = _headerValue;
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCCombo()
        {
        }
       
        public UCCombo(string name)
        {           
            InitializeComponent();
            this.layoutControlItem1.Text = name;
        }

        /// <summary>
        /// 保存条件到TAG里

        /// </summary>
        private void SetConditionToTag()
        {
            string strCondition = "";
            string strOperator = this.cboBool.Text.Trim();
            string CHS = strOperator;
            switch (strOperator)
            {                
                case "是":
                    strCondition = "=1";
                    break;
                case "否":
                    strCondition = "=0";
                    break;               
                default:
                    strCondition = "";
                    CHS = "所有";
                    break;
            }

            bool useFlag = true;
            if (strOperator == "所有" || strOperator == "" || strOperator == null)
            {
                useFlag = false;
            }
           
            if (((Hashtable)this.Tag).ContainsKey("Condition"))
            {
                ((Hashtable)this.Tag).Remove("Condition");
            }
            if (((Hashtable)this.Tag).ContainsKey("UseFlag"))
            {
                ((Hashtable)this.Tag).Remove("UseFlag");
            }
            if (((Hashtable)this.Tag).ContainsKey("HeaderValue"))
            {
                ((Hashtable)this.Tag).Remove("HeaderValue");
            }
            if (((Hashtable)this.Tag).ContainsKey("CHSValue"))
            {
                ((Hashtable)this.Tag).Remove("CHSValue");
            }

            ((Hashtable)this.Tag).Add("Condition", strCondition);
            ((Hashtable)this.Tag).Add("UseFlag", useFlag);
            ((Hashtable)this.Tag).Add("HeaderValue", strOperator);
            ((Hashtable)this.Tag).Add("CHSValue", CHS);
        }

        private void cboBool_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetConditionToTag();
        }

        private void UCCombo_Load(object sender, EventArgs e)
        {
            SetConditionToTag();
        }

    }
}
