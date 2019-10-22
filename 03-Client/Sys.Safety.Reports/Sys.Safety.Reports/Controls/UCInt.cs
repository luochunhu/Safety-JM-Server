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
    public partial class UCInt : DevExpress.XtraEditors.XtraUserControl
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
                try
                {
                    string[] headerValue = _headerValue.Split('$');
                    this.cboCondition.Text = headerValue[0].ToString();
                    this.txtValue1.Text = headerValue[1].ToString();
                    this.txtValue2.Text = headerValue[2].ToString();
                }
                catch
                { }
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCInt()
        {
        }

        public UCInt(string name)
        {           
            InitializeComponent();
            layoutControlItem1.Text = name;
        }

        private void cboCondition_EditValueChanged(object sender, EventArgs e)
        {
            string strOperator = cboCondition.Text.Trim ();
            SetControlVisible(strOperator,true);
            SetConditionToTag();
        }

        /// <summary>
        /// 保存条件到TAG里

        /// </summary>
        private void SetConditionToTag()
        {
            string strCondition = "";
            string strOperator = cboCondition.Text.Trim();
            string CHS = "";
            switch (strOperator)
            {
                case "空值":
                    strCondition = "is null";
                    CHS = "{0}";
                    break;
                case "等于":
                    strCondition = "={0}";
                    CHS = "{0}";
                    break;
                case "大于":
                    strCondition = ">{0}";
                    CHS = "{0}";
                    break;
                case "小于":
                    strCondition = "<{0}";
                    CHS = "{0}";
                    break;
                case "不等于":
                    strCondition = "<>{0}";
                    CHS = "{0}";
                    break;
                case "大于等于":
                    strCondition = ">={0}";
                    CHS = "{0}";
                    break;
                case "小于等于":
                    strCondition = "<={0}";
                    CHS = "{0}";
                    break;
                case "介于":
                    strCondition = " between {0} and {1}";
                    CHS = "{0}和{1}";
                    break;               
                default :
                    strCondition = "";
                    break;
            } 
            string strValue1 = this.txtValue1.EditValue.ToString().Trim ();
            string strValue2 = this.txtValue2.EditValue.ToString().Trim();
            strCondition =string.Format (strCondition ,strValue1 ,strValue2 );
            CHS = string.Format(strOperator + CHS, strValue1, strValue2);

            string strHeaderValue = strOperator + "$" + strValue1 + "$" + strValue2; 
            bool useFlag = true;
            if (strOperator == "任意" || strValue1 == "" || strValue1 == null)
            {
                useFlag = false;
            }
            if (strOperator == "介于" && strValue2 == "" || strValue2 == null)
            {
                strCondition = string.Format(strCondition, strValue1, strValue1); 
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
            ((Hashtable)this.Tag).Add("HeaderValue", strHeaderValue);
            ((Hashtable)this.Tag).Add("CHSValue", CHS);
        }

        /// <summary>
        /// 设置控件是否可用
        /// </summary>
        /// <param name="strOperator"></param>
        /// <param name="isClear"></param>
        private void SetControlVisible(string strOperator, bool isClear)
        {
            this.txtValue1.EditValueChanged  -= new EventHandler(this.txtValue1_EditValueChanged);
            this.txtValue2.EditValueChanged -= new EventHandler(this.txtValue2_EditValueChanged);
            if (isClear)
            {
                this.txtValue1.Text = "";
                this.txtValue2.Text = "";
            }
            if (strOperator == "任意" || strOperator == "空值")
            {
                this.txtValue1.Enabled   = false;
                this.txtValue2.Enabled = false;              
                this.layoutControlItem2.Text = " ";
                this.layoutControlItem3.Text = " ";               
            }
            if (strOperator == "介于")
            {
                this.txtValue1.Enabled = true;
                this.txtValue2.Enabled = true;                
                this.layoutControlItem2.Text  = "从";               
                this.layoutControlItem3.Text = "到";              
            }
            else
            {
                this.txtValue1.Enabled = true;
                this.txtValue2.Enabled = false;               
                this.layoutControlItem2.Text = " ";             
                this.layoutControlItem3.Text = " ";               
            }
            this.txtValue1.EditValueChanged += new EventHandler(this.txtValue1_EditValueChanged);
            this.txtValue2.EditValueChanged += new EventHandler(this.txtValue2_EditValueChanged);
        }

        private void UCInt_Load(object sender, EventArgs e)
        {
            string strOperator = cboCondition.Text.Trim();
            SetControlVisible(strOperator,false);
        }

        private void txtValue1_EditValueChanged(object sender, EventArgs e)
        {
            SetConditionToTag();
        }

        private void txtValue2_EditValueChanged(object sender, EventArgs e)
        {
            SetConditionToTag();
        }      
       
    }
}
