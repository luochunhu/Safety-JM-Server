using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.Request.Config;
using DevExpress.Utils;

namespace Sys.Safety.Client.Setting
{
    public partial class frmAutoCheck : DevExpress.XtraEditors.XtraForm
    {
        private IConfigService configService = ServiceFactory.Create<IConfigService>();
        public frmAutoCheck()
        {
            InitializeComponent();
        }
        private void frmAutoCheck_Load(object sender, EventArgs e)
        {

            // SetDefault();
            //btnRefresh_Click(sender, e);
            this.timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;

            btnRefresh_Click(sender, e);
        }
        private void SetDefault()
        {
            this.lblMasterServerState.Text = "未知";
            this.lblMasterServerState.ForeColor = labelControl1.ForeColor;

            this.lblLastReceiveTime.Text = "未知";
            this.lblLastReceiveTime.ForeColor = labelControl1.ForeColor;

            //this.lblSlaveServerState.Visible = false;
            this.lblSlaveServerState.Text = "未知";
            this.lblSlaveServerState.ForeColor = labelControl1.ForeColor;

            this.lblHAState.Text = "未知";
            this.lblHAState.ForeColor = labelControl1.ForeColor;

            this.lblMasterDataCollectorState.Text = "未知";
            this.lblMasterDataCollectorState.ForeColor = labelControl1.ForeColor;

            this.lblDbState.Text = "未知";
            this.lblDbState.ForeColor = labelControl1.ForeColor;

            //this.lblSlaveDataCollectorState.Text = "未知";
            //this.lblSlaveDataCollectorState.ForeColor = labelControl1.ForeColor;

            this.lblDbSize.Text = "未知";
            this.lblDbSize.ForeColor = labelControl1.ForeColor;

            this.lblClientCPU.Text = "未知";
            this.lblClientCPU.ForeColor = labelControl1.ForeColor;

            this.lblClientMemory.Text = "未知";
            this.lblClientMemory.ForeColor = labelControl1.ForeColor;

            this.lblSeverCPU.Text = "未知";
            this.lblSeverCPU.ForeColor = labelControl1.ForeColor;

            this.lblServerMemory.Text = "未知";
            this.lblServerMemory.ForeColor = labelControl1.ForeColor;

            this.lblDatabaseCPU.Text = "未知";
            this.lblDatabaseCPU.ForeColor = labelControl1.ForeColor;

            this.lblDatabaseMemory.Text = "未知";
            this.lblDatabaseMemory.ForeColor = labelControl1.ForeColor;

            this.lblDiskC.Text = "未知";
            this.lblDiskC.ForeColor = labelControl1.ForeColor;

            this.lblDiskD.Text = "未知";
            this.lblDiskD.ForeColor = labelControl1.ForeColor;

            this.lblDiskE.Text = "未知";
            this.lblDiskE.ForeColor = labelControl1.ForeColor;

            this.lblDiskF.Text = "未知";
            this.lblDiskF.ForeColor = labelControl1.ForeColor;

            this.lblGatherCPU.Text = "未知";
            this.lblGatherCPU.ForeColor = labelControl1.ForeColor;

            this.lblGatherMemory.Text = "未知";
            this.lblGatherMemory.ForeColor = labelControl1.ForeColor;

            this.lblResult.Text = "";
            this.lblResult.ForeColor = labelControl1.ForeColor;


            this.backUpWorkState.Text = "未知";
            this.backUpWorkState.ForeColor = labelControl1.ForeColor;
        }

        bool _isCheck = false;
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_isCheck)
            {
                return;
            }
            _isCheck = true;
            WaitDialogForm wdf = null;
            try
            {
                this.btnRefresh.Enabled = false;
                wdf = new WaitDialogForm("正在自检测中...", "请等待...");

                SetDefault();

                AutoCheck();

                if (wdf != null)
                {
                    wdf.Close();
                }
            }
            catch (Exception ex)
            {
                if (wdf != null)
                {
                    wdf.Close();
                }

                LogHelper.Error(ex);
                MessageBox.Show("检测模块发生错误，详细请查看日志！");
            }
            finally
            {
                this.btnRefresh.Enabled = true;
                _isCheck = false;
            }
        }

        private void AutoCheck()
        {
            this.lblResult.Text = "";

            CheckServerInfo();
            CheckProcessInfo();//检测进程信息
            CheckHardDiskInfo();//检测磁盘信息

            if (this.lblResult.Text == "")
            {
                lblResult.Text = "安全监控系统各模块运行良好，请放心使用！";
                lblResult.ForeColor = Color.Green;
            }
        }

        private void CheckServerInfo()
        {
            RunningInfo runInfo = null;
            try
            {
                //从服务端获取运行情况             
                runInfo = configService.GetRunningInfo().Data;
            }
            catch (Exception ex)
            {
                lblMasterServerState.Text = "连接失败";
                lblMasterServerState.ForeColor = Color.Red;

                this.lblResult.Text += "* 请检查服务端程序是否启动，或者服务端IP地址及端口号配置是否正确；请确认客户端连接服务端的IP及端口号与服务端配置一致；\n";
                this.lblResult.ForeColor = Color.Red;

                LogHelper.Error(ex);
                return;
            }

            //服务端状态检测
            if (runInfo.MasterServerState == 1)
            {
                lblMasterServerState.Text = "正常";
                lblMasterServerState.ForeColor = Color.Green;

                //数采状态检测
                if (runInfo.MasterDataCollectorState == 0)
                {
                    lblMasterDataCollectorState.Text = "连接失败";
                    lblMasterDataCollectorState.ForeColor = Color.Red;

                    this.lblResult.Text += "* 请检查网关程序是否启动，或者网络端口连接是否正常；\n";
                    this.lblResult.ForeColor = Color.Red;
                }
                else
                {
                    lblMasterDataCollectorState.Text = "正常";
                    lblMasterDataCollectorState.ForeColor = Color.Green;
                }
            }

            //双机热备检测
            if (runInfo.IsUseHA)
            {
                lblHAState.Text = "启用";
                lblHAState.ForeColor = Color.Green;

                //if (runInfo.SlaveServerState == 0)
                //{
                //    lblSlaveServerState.Text = "连接失败";
                //    lblSlaveServerState.ForeColor = Color.Red;

                //    //this.lblResult.Text += "* 请检查备机服务器程序是否启动，或者网络端口连接是否正常；\n";
                //    //this.lblResult.ForeColor = Color.Red;
                //}
                //else
                //{
                //    lblSlaveServerState.Text = "正常";
                //    lblSlaveServerState.ForeColor = Color.Green;
                //}

                if (runInfo.SlaveDataCollectorState == 0)
                {
                    //lblSlaveDataCollectorState.Text = "连接失败";
                    //lblSlaveDataCollectorState.ForeColor = Color.Red;

                    //this.lblResult.Text += "* 请检查备机网关程序是否启动，或者网络端口连接是否正常；\n";
                    //this.lblResult.ForeColor = Color.Red;
                }
                else
                {
                    //lblSlaveDataCollectorState.Text = "正常";
                    //lblSlaveDataCollectorState.ForeColor = Color.Green;
                }
            }
            else
            {
                lblHAState.Text = "未启用";
                lblHAState.ForeColor = Color.Blue;

                lblSlaveServerState.Text = "未启用";
                lblSlaveServerState.ForeColor = Color.Blue;

                //lblSlaveDataCollectorState.Text = "未启用";
                //lblSlaveDataCollectorState.ForeColor = Color.Blue;
            }

            if (runInfo.LastReceiveTime == DateTime.MinValue)
            {
                lblLastReceiveTime.ForeColor = Color.Red;
            }
            else
            {
                lblLastReceiveTime.Text = runInfo.LastReceiveTime.ToString("yyyy-MM-dd HH:mm:ss");
                if ((DateTime.Now - runInfo.LastReceiveTime).TotalMinutes > 5)
                {
                    //如果最后接收数据的时间晚于当前5分钟，则以红色提醒
                    lblLastReceiveTime.ForeColor = Color.Red;

                    this.lblResult.Text += "* 服务端程序最后接收数据时间晚于当前时间超过5分钟及以上，请检查服务端接收数据是否正常；\n";
                    this.lblResult.ForeColor = Color.Red;
                }
                else
                {
                    lblLastReceiveTime.ForeColor = Color.Green;
                }
            }

            //增加获取双机热备工作状态   20180123
            switch (runInfo.BackUpWorkState)
            {
                case -1:
                    backUpWorkState.Text = "未知";
                    lblSlaveServerState.Text = "未知";
                    lblSlaveServerState.ForeColor = Color.Black;
                    backUpWorkState.ForeColor = Color.Black;
                    break;
                case 0:
                    backUpWorkState.Text = "网络恢复";
                    lblSlaveServerState.Text = "";
                    lblSlaveServerState.ForeColor = Color.Green;
                    backUpWorkState.ForeColor = Color.Green;
                    break;
                case 1:
                    backUpWorkState.Text = "网络中断";
                    lblSlaveServerState.Text = "网络中断";
                    lblSlaveServerState.ForeColor = Color.Red;
                    backUpWorkState.ForeColor = Color.Red;
                    break;
                case 2:
                    backUpWorkState.Text = "连接中断";
                    lblSlaveServerState.Text = "连接中断";
                      lblSlaveServerState.ForeColor = Color.Red;
                    backUpWorkState.ForeColor = Color.Red;
                    break;
                case 3:
                    backUpWorkState.Text = "切换中";
                    lblSlaveServerState.Text = "待机中";
                      lblSlaveServerState.ForeColor = Color.Green;
                      backUpWorkState.ForeColor = Color.Green;
                    break;
                case 4:
                    backUpWorkState.Text = "正常";
                    lblSlaveServerState.Text = "待机中";
                     lblSlaveServerState.ForeColor = Color.Green;
                      backUpWorkState.ForeColor = Color.Green;
                    break;
                case 5:
                    backUpWorkState.Text = "主备机程序退出";
                    lblSlaveServerState.Text = "待机中";
                     lblSlaveServerState.ForeColor = Color.Green;
                      backUpWorkState.ForeColor = Color.Green;
                    break;
                case 6:
                    backUpWorkState.Text = "正常";
                    lblSlaveServerState.Text = "运行中";
                     lblSlaveServerState.ForeColor = Color.Green;
                      backUpWorkState.ForeColor = Color.Green;
                    break;
                case 7:
                    backUpWorkState.Text = "主备机同时运行";
                    lblSlaveServerState.Text = "运行中";
                     lblSlaveServerState.ForeColor = Color.Red;
                     backUpWorkState.ForeColor = Color.Red;
                    break;
                case 8:
                    backUpWorkState.Text = "网卡异常";
                    lblSlaveServerState.Text = "网络中断";
                    lblSlaveServerState.ForeColor = Color.Red;
                     backUpWorkState.ForeColor = Color.Red;
                    break;
            }



            //数据库检测
            if (runInfo.DbState == 0)
            {
                this.lblDbState.Text = "连接失败";
                this.lblDbState.ForeColor = Color.Red;
            }
            else
            {
                this.lblDbState.Text = "正常";
                this.lblDbState.ForeColor = Color.Green;
            }

            if (runInfo.DbSize > 0)
            {
                this.lblDbSize.Text = string.Format("{0}M", runInfo.DbSize);
                if (runInfo.DbSize < 2000)
                {
                    this.lblDbSize.ForeColor = Color.Green;
                }
                else
                {
                    this.lblDbSize.ForeColor = Color.Red;
                }
            }

        }

        /// <summary>
        /// 检测磁盘信息
        /// </summary>
        private void CheckHardDiskInfo()
        {
            HandleDiskInfo("C", lblDiskC);
            HandleDiskInfo("D", lblDiskD);
            HandleDiskInfo("E", lblDiskE);
            HandleDiskInfo("F", lblDiskF);
        }

        //处理磁盘信息显示 
        private void HandleDiskInfo(string diskName, LabelControl labelControl)
        {
            HardDiskInfo diskInfo = null;

            try
            {
                ConfigGetDiskInfoRequest request = new ConfigGetDiskInfoRequest()
                {
                    DiskName = diskName
                };
                //从服务端获取磁盘情况               
                diskInfo = configService.GetDiskInfo(request).Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            if (diskInfo == null)
            {
                labelControl.ForeColor = Color.Blue;
                labelControl.Text = "未检测到相关信息";
                return;
            }

            //labelControl.Text = string.Format("{0}GB ({1}%)", diskInfo.TotalFreeSize , diskInfo.TotalUsageRate); 
            labelControl.Text = string.Format("{0}GB 可用，共{1}GB (使用率{2}%)", diskInfo.TotalFreeSize, diskInfo.TotalSize, diskInfo.TotalUsageRate);
            if (diskInfo.TotalUsageRate >= 90)
            {
                labelControl.ForeColor = Color.Red;

                lblResult.Text += diskName + "* 盘剩余磁盘空间不足10%，请手工清理磁盘空间。\n";
                lblResult.ForeColor = Color.Red;
            }
            else
            {
                labelControl.ForeColor = Color.Green;
            }
        }

        /// <summary>
        /// 检测进程信息
        /// </summary>
        private void CheckProcessInfo()
        {
            HandleProcessInfo("Sys.Safety.Client.WindowHost", lblClientCPU, lblClientMemory, false);

            if (!HandleProcessInfo("Sys.Safety.Server.ServiceHost", lblSeverCPU, lblServerMemory))
            {
                //服务端程序，有可能不是以服务的形式部署，所以需要做特殊处理
                HandleProcessInfo("Sys.Safety.Server.ConsoleHost", lblSeverCPU, lblServerMemory);
            }

            if (!HandleProcessInfo("Sys.DataCollection.ServiceHost", lblGatherCPU, lblGatherMemory))
            {
                HandleProcessInfo("Sys.DataCollection.ConsoleHost", lblGatherCPU, lblGatherMemory);
            }

            HandleProcessInfo("mysqld", lblDatabaseCPU, lblDatabaseMemory);
        }
        //处理进程信息显示 
        private bool HandleProcessInfo(string processName, LabelControl labelCpuControl, LabelControl lableMemoryControl, bool isFromServer = true)
        {
            PorcessInfo processInfo = null;

            if (isFromServer) //从服务端获取
            {
                try
                {
                    ConfigGetProcessInfoRequest request = new ConfigGetProcessInfoRequest()
                    {
                        ProcessName = processName
                    };
                    //从服务端获取进程情况                   
                    processInfo = configService.GetProcessInfo(request).Data;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
            }
            else //从本地获取
            {
                processInfo = HardwareUtils.GetProcessInfo(processName);
            }

            if (processInfo == null)
            {
                labelCpuControl.ForeColor = Color.Blue;
                labelCpuControl.Text = "未检测到进程信息";

                lableMemoryControl.ForeColor = Color.Blue;
                lableMemoryControl.Text = "未检测到进程信息";
                return false;
            }

            labelCpuControl.Text = string.Format("{0}%", processInfo.CpuUsageRate);
            if (processInfo.CpuUsageRate >= 30)
            {
                labelCpuControl.ForeColor = Color.Red;
            }
            else
            {
                labelCpuControl.ForeColor = Color.Green;
            }

            lableMemoryControl.Text = string.Format("{0}M", processInfo.MemoryUsageSize);
            if (processInfo.MemoryUsageSize >= 1024)
            {
                lableMemoryControl.ForeColor = Color.Red;
            }
            else
            {
                lableMemoryControl.ForeColor = Color.Green;
            }

            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }

}