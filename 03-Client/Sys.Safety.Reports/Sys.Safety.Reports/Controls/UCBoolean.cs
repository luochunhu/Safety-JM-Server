using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;

namespace Sys.Safety.Reports.Controls
{        
    public partial class UCBoolean : DevExpress.XtraEditors.XtraUserControl
    {
        private string _strKeyValue = "";

        /// <summary>
        /// 设置条件的键值

        /// </summary>
        public string StrKeyValue
        {
            set
            {
                _strKeyValue = value;
                if (this._strKeyValue == "所有")
                {
                    this._strKeyValue = "";
                }
                this.cboBool.Text = _strKeyValue;
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCBoolean()
        {
            InitializeComponent();
        }

        public UCBoolean(string textName)
        {
            InitializeComponent();
            this.layoutControlItem1.Text = textName;
        }

        private void UCCombo_Load(object sender, EventArgs e)
        {
            SetTag();
        }

        /// <summary>
        /// 保存条件到TAG里

        /// </summary>
        private void SetTag()
        {
            this._strKeyValue = this.cboBool.Text.Trim();
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

        private void cboBool_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTag();
        }
    }
}
