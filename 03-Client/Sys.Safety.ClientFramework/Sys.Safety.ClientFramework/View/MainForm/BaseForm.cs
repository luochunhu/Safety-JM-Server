using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.ClientFramework.Configuration;

namespace Sys.Safety.ClientFramework.View.MainForm
{

    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取系统图标
        /// </summary>
        public virtual void GetIcon()
        {
            if (System.IO.File.Exists(BaseInfo.FormIcon))
            {
                this.Icon = new Icon(BaseInfo.FormIcon);
            }
        }
    }
}