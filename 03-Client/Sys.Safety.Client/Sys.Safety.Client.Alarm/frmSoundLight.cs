using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmSoundLight : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 上次输出初始化串口配置异常日志的时间
        /// </summary>
        private DateTime dtmInitLastLog = DateTime.Now;

        /// <summary>
        /// 上次输出语音播放异常日志的时间
        /// </summary>
        private DateTime dtmShowLastLog = DateTime.Now;

        /// <summary>
        /// 是否是第一次输出日志
        /// </summary>
        private bool bIsFirstLog = true;

        public frmSoundLight()
        {
            InitializeComponent();
        }

        private void frmSoundLight_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.timerLight.Enabled = true;

            InitSerialPort();//端口打开的情况下无法再对端口进行赋值
        }

        private void InitSerialPort()
        {
            try
            {
                //从配置中读取 如果本地配置文件没有则从数据库中读取
                if (this.serialPort.IsOpen) { this.serialPort.Close(); }
                this.serialPort.PortName = ClientAlarmConfigCache.soundLightPortName;//端口打开时无法设置“PortName”
                this.serialPort.BaudRate = ClientAlarmConfigCache.soundLightBaudRate;
                this.serialPort.DataBits = 8;
                this.serialPort.Parity = System.IO.Ports.Parity.Space;
                this.serialPort.StopBits = System.IO.Ports.StopBits.One;
                this.serialPort.RtsEnable = true;

                this.serialPort.DtrEnable = false;
            }
            catch (Exception ex)
            {
                if (bIsFirstLog)
                {
                    bIsFirstLog = false;
                    LogHelper.Error("声光报警串口配置发生异常 " + ex.Message);
                }
                else if (dtmInitLastLog.AddHours(1) < DateTime.Now)
                {
                    dtmInitLastLog = DateTime.Now;
                    LogHelper.Error("声光报警串口配置发生异常 " + ex.Message);
                }
            }

            //当存在多个声光报警信息时，如何控制每条信息的展示
            if (!this.serialPort.IsOpen)
            {
                try
                {
                    this.serialPort.Open();//电脑没有串口 USB转串口
                }
                catch (Exception ex)
                {
                    //异常处理、日志
                    if (dtmShowLastLog.AddHours(1) < DateTime.Now)
                    {
                        dtmShowLastLog = DateTime.Now;
                        LogHelper.Error("打开声光报警串口连接失败！", ex);
                    }
                    return;
                }
            }
        }

        private void ShowSoundLight()
        {
            if (ClientAlarmConfigCache.showDataLight == null || ClientAlarmConfigCache.showDataLight.Count < 1) { return; }
           
            try
            {
                if (this.serialPort.IsOpen)
                {
                    this.serialPort.DtrEnable = true;
                    Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                if (dtmShowLastLog.AddHours(1) < DateTime.Now)
                {
                    dtmShowLastLog = DateTime.Now;
                    LogHelper.Error("启用Dtr信号失败！", ex);
                }
                return;
            }
            //每报一条就删除一天，或者 只要有就一直报，直到没有或者用户手动触发停止
            //ClientAlarmConfigCache.showDataLight.RemoveAt(0);
            //this.serialPort.DtrEnable = false;Z
        }

        private void timerLight_Tick(object sender, EventArgs e)
        {          
            ShowSoundLight();
        }

        public void CancelAlarm()
        {
            try
            {
                this.serialPort.DtrEnable = false;
                Thread.Sleep(2);
                if (ClientAlarmConfigCache.showDataLight != null)
                {
                    ClientAlarmConfigCache.showDataLight.Clear();
                }
            }
            catch (Exception ex)
            {
                //写日志或抛出异常等
                LogHelper.Error("关闭声光报警串口失败！", ex);
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, "关闭声光报警串口失败！");
            }
        }
    }
}