using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ClientFramework.Configuration;
using System.Windows.Forms;
using System.IO;

namespace Sys.Safety.ClientFramework.View.MainForm
{
    public class BaseLoad
    {
        /// <summary>
        /// 窗体加载
        /// </summary>
        public void FormOnLoad()
        {
            //系统图标文件
            BaseInfo.FormIcon = Path.Combine(Application.StartupPath, "WorkImage/Image/Form.ico");
        }
    }
}
