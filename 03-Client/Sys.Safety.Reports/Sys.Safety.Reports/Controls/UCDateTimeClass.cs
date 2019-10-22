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
    public partial class UCDateTimeClass : DevExpress.XtraEditors.XtraUserControl
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
                    //this.datFrom.EditValueChanged -= new EventHandler(this.datFrom_EditValueChanged);
                    //this.datTo.EditValueChanged -= new EventHandler(this.datTo_EditValueChanged);
                    string[] strs = _strKeyValue.Split(new string[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length > 1 && strs[1].Substring(0, 4) != "1900")
                    {                       
                        this.datFrom.Text = strs[1];
                    }
                    if (strs.Length > 2 && strs[2].Substring(0, 4) != "9999")
                    {                      
                        this.datTo.Text = strs[2];
                    }

                }
                catch
                {
 
                }
                finally
                {
                    //this.datFrom.EditValueChanged += new EventHandler(this.datFrom_EditValueChanged);
                    //this.datTo.EditValueChanged += new EventHandler(this.datTo_EditValueChanged);
                }
            }
        }

        public string ControlTile
        {
            get { return this.layoutControlItem1.Text; }
            set { this.layoutControlItem1.Text = value; }
        }

        public UCDateTimeClass()
        {
        }

        public UCDateTimeClass(string name)
        {
            InitializeComponent();

            this.layoutControlItem1.Text = name;
            LookUpUtil.CreateGridLookUp("AllClass", gridLookUpClass, false, false);
            checkEdit2.Checked = true;
        }

        private void datFrom_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }

        private void datTo_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }

        private void gridLookUpClass_EditValueChanged(object sender, EventArgs e)
        {
            SetTag();
        }

        /// <summary>
        /// 保存条件到TAG里//操作符&&$NumRangle1&&$NumRangle2
        /// </summary>
        /// \\\\\
        private void SetTag()
        {
            try
            {
                string str;
                string datFrom = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
                string datTo = this.datTo.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";

                if (this.datFrom.EditValue == null)
                {
                    datFrom = "1900-01-01 00:00:00";
                }
                if (this.datTo.EditValue == null)
                {
                    datTo = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
                }

                //2014-12-12
                if (checkEdit1.Checked)
                {//如果是按班次查询,则只能查询一天的数据
                    string strstartTime = "00:00:00";
                    string strendTime = "23:59:00";
                    if (this.gridLookUpViewClass.FocusedRowHandle > -1)
                    {
                        strstartTime = TypeUtil.ToString(gridLookUpViewClass.GetRowCellValue(gridLookUpViewClass.FocusedRowHandle, "datStart"));
                        strendTime = TypeUtil.ToString(gridLookUpViewClass.GetRowCellValue(gridLookUpViewClass.FocusedRowHandle, "datEnd"));
                    }
                    datFrom = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " " + strstartTime;
                    datTo = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " " + strendTime;
                    if (TypeUtil.ToInt(strendTime.Substring(0, 2)) < TypeUtil.ToInt(strstartTime.Substring(0, 2)))
                    {
                        datTo = this.datFrom.DateTime.AddDays(1).ToString("yyyy-MM-dd") + " " + strendTime;
                    }
                }





                str = "介于&&$" + datFrom + "&&$" + datTo;

                _strKeyValue = str;

                if (((Hashtable)this.Tag).ContainsKey("_strKeyValue"))
                {
                    ((Hashtable)this.Tag)["_strKeyValue"] = this._strKeyValue;
                }
                else
                {
                    ((Hashtable)this.Tag).Add("_strKeyValue", this._strKeyValue);
                }

                if (((Hashtable)this.Tag).ContainsKey("_strValueChs"))
                {
                    ((Hashtable)this.Tag)["_strValueChs"] = datFrom + "至" + datTo;
                }
                else
                {
                    ((Hashtable)this.Tag).Add("_strValueChs", datFrom + "至" + datTo);
                }
                SetDateString(datFrom, datTo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDateString(string datFrom, string datTo)
        {
            List<string> li = DateTimeUtil.SetDateString(datFrom, datTo);
            if (((Hashtable)this.Tag).ContainsKey("_date"))
            {
                ((Hashtable)this.Tag)["_date"] = li;
            }
            else
            {
                ((Hashtable)this.Tag).Add("_date", li);
            }
        }

        private void datFrom_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string datFrom = TypeUtil.ToDateTime(e.NewValue).ToString("yyyy-MM-dd") + " 00:00:00"; ;
            string datTo = this.datTo.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";
            string str = datFrom.Substring(0, 4);
            if (str == "0001" || str == "1900")
            {
                return;
            }
            if (this.datTo.EditValue != null && datFrom.CompareTo(datTo) > 0)
            {
                MessageBox.Show("前面的日期不能大于后面的日期！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void datTo_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string datFrom = this.datFrom.DateTime.ToString("yyyy-MM-dd") + " 00:00:00"; ;
            string datTo = TypeUtil.ToDateTime(e.NewValue).ToString("yyyy-MM-dd") + " 23:59:59";
            string str = datFrom.Substring(0, 4);
            if (str == "0001" || str == "1900")
            {
                return;
            }
            if (this.datFrom.EditValue != null && datFrom.CompareTo(datTo) > 0)
            {
                MessageBox.Show("前面的日期不能大于后面的日期！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }



        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked)
            {
                checkEdit2.Checked = false;
                //layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                if (datFrom.EditValue == null)
                {
                    datFrom.DateTime = DateTime.Now;
                }
                int width = this.layoutControl1.Width - layoutControlItem3.Width - layoutControlItem5.Width;
                this.layoutControlItem1.Width = this.layoutControlItem4.Width = width / 2;
                gridLookUpClass_EditValueChanged(null, null);
            }
            else
            {
                datFrom_EditValueChanged(null, null);
                datTo_EditValueChanged(null, null);
            }
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkEdit2.Checked)
            {
                checkEdit1.Checked = false;
                //layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                int width = this.layoutControl1.Width - layoutControlItem3.Width - layoutControlItem5.Width;
                this.layoutControlItem1.Width = width / 2 + 17; ;
                this.layoutControlItem2.Width = width / 2 - 17;

            }
        }


    }
}
