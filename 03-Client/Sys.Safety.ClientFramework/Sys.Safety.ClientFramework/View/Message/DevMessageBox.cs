using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sys.Safety.ClientFramework.View.Message
{
    public class DevMessageBox
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public enum MessageType
        {
            None=0x0,
            /// <summary>
            /// 显示提示信息
            /// </summary>
            Information = 0x01,
            /// <summary>
            /// 确定对话框 YesNo
            /// </summary>
            Confirm = 0x02,
            /// <summary>
            /// 警告对话框
            /// </summary>
            Warning = 0x03,
            /// <summary>
            /// 风险等级
            /// </summary>
            Stop = 0x04,
            /// <summary>
            /// 询问对话框 OKCancel
            /// </summary>
            Question = 0x05,
            /// <summary>
            /// 等待对话框
            /// </summary>
            Hand=0x06

        }
        public static DialogResult Show(MessageType msgType, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return DevExpress.XtraEditors.XtraMessageBox.Show("消息对话框未指定消息内容！", "警告信息",
                                                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (msgType == MessageType.None)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else if (msgType == MessageType.Information)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (msgType == MessageType.Question)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                else if (msgType == MessageType.Confirm)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
                else if (msgType == MessageType.Stop)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else if (msgType == MessageType.Warning)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (msgType == MessageType.Hand)
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "等待", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } 
        }
    }
}
