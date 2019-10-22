using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Collections;
using Panel = System.Web.UI.WebControls.Panel;


namespace Sys.Safety.Reports.Controls
{
    public partial class UCGridLookUp : DevExpress.XtraEditors.XtraUserControl
    {      
        private string fkCode = string.Empty;//参照编码      
        private string _strKeyValue = "";//条件
        private string _strDisplayValue = "";

        /// <summary>
        /// 设置参照编码
        /// </summary>
        public string FkCode
        {
            set
            {
                fkCode = value;
            }
        }

        /// <summary>
        /// 所选条件值
        /// </summary>
        public string StrKeyValue
        {
            get
            {
                return _strKeyValue;
            }
            set
            {
                _strKeyValue = value;

                if (_strKeyValue != string.Empty && _strKeyValue.Length > 5)
                {
                    _strKeyValue = _strKeyValue.Substring(5);
                }

                gridLookUpEdit.EditValueChanged -= new EventHandler(gridLookUpEdit_EditValueChanged);
                this.gridLookUpEdit.EditValue = _strKeyValue;
                gridLookUpEdit.EditValueChanged += new EventHandler(gridLookUpEdit_EditValueChanged);
                SetConditionToTag();
            }
        }

        /// <summary>
        /// 所选条件显示数据

        /// </summary>
        public string StrDisplayValue
        {
            get
            {
                return _strDisplayValue;
            }
            set
            {
                _strDisplayValue = value;
                if (_strDisplayValue != string.Empty && _strDisplayValue.Length > 5)
                {
                    _strDisplayValue = _strDisplayValue.Substring(5);
                }              

                SetConditionToTag();
            }
        }


        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCGridLookUp()
        {
        }

        /// <summary>
        /// 构造方法

        /// </summary>
        /// <param name="textName">参照标题</param>
        public UCGridLookUp(string textName,string strFKCode)
        {
            InitializeComponent();
            this.layoutControlItem1.Text = textName;
            this.fkCode = strFKCode;
            LookUpUtil.CreateGridLookUp(this.fkCode, gridLookUpEdit, true, true);
        }

        /// <summary>
        /// 保存条件到TAG里


        /// </summary>
        private void SetConditionToTag()
        {
            if (((Hashtable)this.Tag).ContainsKey("_strKeyValue"))
            {
                ((Hashtable)this.Tag)["_strKeyValue"] = this._strKeyValue;
            }
            else
            {
                ((Hashtable)this.Tag).Add("_strKeyValue", this._strKeyValue);
            }

            if (((Hashtable)this.Tag).ContainsKey("_strDisplayValue"))
            {
                ((Hashtable)this.Tag)["_strDisplayValue"] = this._strDisplayValue;
            }
            else
            {
                ((Hashtable)this.Tag).Add("_strDisplayValue", this._strDisplayValue);
            }
        }       

       

        private void gridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (TypeUtil.ToString(gridLookUpEdit.EditValue) != "空" && TypeUtil.ToString(gridLookUpEdit.EditValue) != "")
            {
                this._strKeyValue = "'" + Convert.ToString(gridLookUpEdit.EditValue) + "'";
                this._strDisplayValue = Convert.ToString(gridLookUpEdit.Text);               
            }
            else
            {
                this._strKeyValue = "";
                this._strDisplayValue = "";
            }
            SetConditionToTag();
        }
    }
}
