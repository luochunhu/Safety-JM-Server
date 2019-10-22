using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ClientFramework.View.UserControl;
using DevExpress.XtraBars;
using Sys.Safety.ClientFramework.View.UserControl.Message;
using DevExpress.Utils;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.View.UserControl.WaitForm;

namespace Sys.Safety.Reports
{
    public class MessageShowUtil
    {

        private static WaitDialogForm wdf = null;
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="caption">信息串</param>
        /// <param name="isMsg">是否弹出提示框</param>
        /// <param name="staticMsg">显示信息状态栏</param>
        public static void ShowMsg(string caption, bool isMsg, DevExpress.XtraBars.BarStaticItem staticMsg)
        {
            caption = caption.Replace("!", "！");
            staticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + caption;
            if (isMsg)
            {
                MessageBox.Show(MessageBox.MessageType.Information,caption);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="caption">信息串</param>
        /// <param name="isMsg">是否弹出提示框</param>
        /// <param name="staticMsg">显示信息状态栏</param>
        public static void ShowMsg(string caption)
        {
            MessageBox.Show(MessageBox.MessageType.Information,caption);
        }


        public static void ShowErrow(Exception ex)
        {
            MessageBox.Show(MessageBox.MessageType.Stop, "操作失败,原因为" + ex.Message);
            LogHelper.Error(ex.ToString());
        }

        public static void ShowInfo(string strInfo)
        {
            if (TypeUtil.ToString(strInfo) == "")
            {
                MessageBox.Show(MessageBox.MessageType.Information, "操作成功");
            }
            else
            {
                MessageBox.Show(MessageBox.MessageType.Information, strInfo);
            }
          
        }


        public static System.Windows.Forms.DialogResult ReturnDialogResult(string strInfo)
        {
            return MessageBox.Show(MessageBox.MessageType.Confirm,strInfo);
        }


        public static void ShowStaticInfo(string strInfo, BarStaticItem barStaticItemMsg)
        {
            strInfo = strInfo.Replace("!", "！");
            if (barStaticItemMsg != null)
            {
                barStaticItemMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + strInfo;
            }
        }


        /// <summary>
        /// 打开等待对话框


        /// </summary>
        public static void OpenWaitDialog(string caption)
        {
            wdf = new WaitDialogForm(caption + "...", "请等待...");
           
        }

        /// <summary>
        /// 关闭等待对话框
        /// </summary>
        public static void CloseWaitDialog()
        {
            if (wdf != null)
            {
                wdf.Close();
            }

          
        }
    }
}

