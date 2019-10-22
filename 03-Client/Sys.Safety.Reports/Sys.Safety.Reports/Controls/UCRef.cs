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


namespace Sys.Safety.Reports.Controls
{
    public partial class UCRef : DevExpress.XtraEditors.XtraUserControl
    {      
        private string fkCode = string.Empty;//参照编码      
        private string _strKeyValue = "";//条件
        private string _strDisplayValue = "";

        /// <summary>
        /// 测点过滤面板
        /// </summary>
        public PanelControl PointFilter { get; set; }

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
                //if (_strDisplayValue != string.Empty && _strDisplayValue.Length > 5)
                //{
                //    _strDisplayValue = _strDisplayValue.Substring(5);
                //}

                this.buttonEdit.Text = _strDisplayValue;

                SetConditionToTag();
            }
        }


        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCRef()
        {
        }

        /// <summary>
        /// 构造方法

        /// </summary>
        /// <param name="textName">参照标题</param>
        public UCRef(string textName)
        {
            InitializeComponent();
            this.layoutControlItem1.Text = textName;
            this.buttonEdit.Properties.ReadOnly = true;
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

        private void buttonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index != 0)
            {
                return;
            }
            frmGenericRef multiDlgRef = new frmGenericRef();
            multiDlgRef.StrFkCode = this.fkCode;
            multiDlgRef.BlnSelectStr = true;
            multiDlgRef.BlnMulti = true;
            multiDlgRef.StrSelectValue = this._strKeyValue;
            multiDlgRef.PointFilter = PointFilter;
            multiDlgRef.ShowDialog();
            if (multiDlgRef.BlnOk)
            {
                this.buttonEdit.EditValue = multiDlgRef.StrSelectDisplay;
                this._strKeyValue = multiDlgRef.StrSelectValue;
                this._strDisplayValue = multiDlgRef.StrSelectDisplay;
                SetConditionToTag();
            }
        }
    }
}
