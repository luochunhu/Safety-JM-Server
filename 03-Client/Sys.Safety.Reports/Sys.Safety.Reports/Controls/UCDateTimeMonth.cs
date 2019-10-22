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
    public partial class UCDateTimeMonth : DevExpress.XtraEditors.XtraUserControl
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
                        DateTime dtFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        this.datFrom.DateTime = dtFrom;
                    }
                    if (strs.Length > 2 && strs[2].Substring(0, 4) != "9999")
                    {
                        DateTime d1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        DateTime dtTo = d1.AddMonths(1).AddDays(-1);
                        this.datTo.DateTime = dtTo;
                    }
                }
                catch
                { }
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

        public UCDateTimeMonth()
        {
        }

        public UCDateTimeMonth(string name)
        {
            InitializeComponent();

            this.layoutControlItem1.Text = name;

            this.datFrom.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.YearView;
            this.datFrom.Properties.EditMask = "y";
            this.datTo.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.YearView;
            this.datTo.Properties.EditMask = "y";

            this.datFrom.Properties.DisplayFormat.FormatString = "y";
            this.datFrom.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.datFrom.Properties.EditFormat.FormatString = "y";
            this.datFrom.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;

            this.datTo.Properties.DisplayFormat.FormatString = "y";
            this.datTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.datTo.Properties.EditFormat.FormatString = "y";
            this.datTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
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

                DateTime d1 = new DateTime(this.datTo.DateTime.Year, this.datTo.DateTime.Month, 1);
                DateTime dtTo = d1.AddMonths(1).AddDays(-1);
                string datTo = dtTo.ToString("yyyy-MM-dd") + " 23:59:59";

                if (this.datFrom.EditValue == null)
                {
                    datFrom = "1900-01-01 00:00:00";
                }
                if (this.datTo.EditValue == null)
                {
                    datTo = "9999-12-31 23:59:59";
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
    }
}
