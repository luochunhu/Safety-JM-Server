using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Basic.Framework.Logging;
using System.Windows.Forms;

namespace Sys.Safety.ClientFramework.CBFCommon
{
    /// <summary>
    /// 调用系统帮助文档
    /// </summary>
    public class SysHelp
    {
        /// <summary>
        /// 调用系统帮助文档
        /// </summary>
        /// <param name="filePath">文件路径及参数（如C:\help.chm::菜单栏.html#cdlcssz）</param>
        public static void ShowSysHelp(Dictionary<string, string> param)
        {
            try
            {
                string filePath = "";
                if (param != null && param.Count > 0)
                {
                    filePath = param["filePath"].ToString();
                }
                Process.Start("hh.exe", Application.StartupPath + "\\" + filePath);
            }
            catch (Exception ex)
            {
                LogHelper.Error("SysHelp_ShowSysHelp" + ex.Message + ex.StackTrace);
            }
        }
    }
}
