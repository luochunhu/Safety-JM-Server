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
    public partial class UCTextTwo : DevExpress.XtraEditors.XtraUserControl
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
                    this.txtValue1.EditValueChanged -= new EventHandler(this.txtValue1_EditValueChanged);

                    string[] strs = _strKeyValue.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 1) this.txtValue1.Text = strs[1];
                }
                catch
                { }
                finally
                {
                    this.txtValue1.EditValueChanged += new EventHandler(this.txtValue1_EditValueChanged);
                }
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem2.Text; }
            set { this.layoutControlItem2.Text = value; }
        }

        public UCTextTwo()
        {
        }

        public UCTextTwo(string name)
        {           
            InitializeComponent();
            layoutControlItem2.Text = name;         
        }

        /// <summary>
        /// 保存条件到TAG里

        /// </summary>
        private void SetTag()
        {
            try
            {
                string str = string.Empty;
                string str1 = this.txtValue1.Text.Trim();
                if (str1 != string.Empty)
                {
                    str = "包含&&$" + str1;
                }

                _strKeyValue = str;

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

        private void txtValue1_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }
    }
}
