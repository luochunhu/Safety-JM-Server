using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.Client.Define.Model;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;

namespace Sys.Safety.Client.Define.CommCfg
{
    public partial class CFCOMCfg : XtraForm
    {
        public CFCOMCfg()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 传入的COM口
        /// </summary>
        private string _COMCode;

        public CFCOMCfg(string Code)
        {
            _COMCode = Code;
            InitializeComponent();
        }
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        private bool Stationverify()
        {
            bool ret = false;

            if (string.IsNullOrEmpty(CcmbBaudRate.Text))
            {
                XtraMessageBox.Show("请设置波特率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbCommPAL.Text))
            {
                XtraMessageBox.Show("请设置通讯制式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbDatabit.Text))
            {
                XtraMessageBox.Show("请设置数据位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbCheckbit.Text))
            {
                XtraMessageBox.Show("请设置校验位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbStopbit.Text))
            {
                XtraMessageBox.Show("请设置停止位", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }

            ret = true;
            return ret;
        }
        /// <summary>
        ///加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFCOMCfg_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPretermitInf();
                LoadBasicInf();
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex); ;
            }

        }
        /// <summary>
        /// 加载默认信息
        /// </summary>
        private void LoadPretermitInf()
        {

            //加载波特率
            CcmbBaudRate.Properties.Items.Add(2400);
            CcmbBaudRate.Properties.Items.Add(4800);
            CcmbBaudRate.Properties.Items.Add(9600);
            CcmbBaudRate.Properties.Items.Add(14400);
            CcmbBaudRate.Properties.Items.Add(19200);
            CcmbBaudRate.Properties.Items.Add(38400);
            CcmbBaudRate.Properties.Items.Add(56000);
            CcmbBaudRate.Properties.Items.Add(56700);
            CcmbBaudRate.Properties.Items.Add(115200);
            CcmbBaudRate.Properties.Items.Add(128000);


            //加载串口号
            for (int i = 0; i < 100; i++)
            {
                CcmbSerialPortNum.Properties.Items.Add("COM" + (i + 1).ToString());
            }

            //加载通讯制式
            CcmbCommPAL.Properties.Items.Add("RS485");
            CcmbCommPAL.Properties.Items.Add("DPSK");

            //加载数据位
            CcmbDatabit.Properties.Items.Add("7位");
            CcmbDatabit.Properties.Items.Add("8位");

            //加载校验位
            CcmbCheckbit.Properties.Items.Add("无校验");
            CcmbCheckbit.Properties.Items.Add("奇校验");
            CcmbCheckbit.Properties.Items.Add("偶校验");

            //加载停止位
            CcmbStopbit.Properties.Items.Add("1位");
            CcmbStopbit.Properties.Items.Add("1位半");
            CcmbStopbit.Properties.Items.Add("2位");
        }
        /// <summary>
        /// 加载传入信息
        /// </summary>
        private void LoadBasicInf()
        {
            if (string.IsNullOrEmpty(_COMCode))
            {
                return;
            }
            Jc_MacInfo COM = Model.MACServiceModel.QueryMACByCode(_COMCode);
            if (null == COM)
            {
                return;
            }
            CcmbSerialPortNum.Text = COM.MAC;
            CcmbCommPAL.Text = COM.Bz2;
            CcmbBaudRate.Text = COM.Bz1;
            CcmbDatabit.Text = COM.Bz3;
            CcmbCheckbit.Text = COM.Bz4;
            CcmbStopbit.Text = COM.Bz5;
        }
        /// <summary>
        /// 取消保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {

                Jc_MacInfo temp = new Jc_MacInfo();
                Jc_MacInfo _COM;
                temp.MAC = this.CcmbSerialPortNum.Text;
                temp.Type = 1;
                temp.Bz1 = CcmbBaudRate.Text;
                temp.Bz2 = CcmbCommPAL.Text;
                temp.Bz3 = CcmbDatabit.Text;
                temp.Bz4 = CcmbCheckbit.Text;
                temp.Bz5 = CcmbStopbit.Text;
                if (string.IsNullOrEmpty(_COMCode))
                {
                    temp.ID = IdHelper.CreateLongId().ToString();
                    temp.InfoState = InfoState.AddNew;
                    try
                    {
                        Model.MACServiceModel.AddMACCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OperateLogHelper.InsertOperateLog(11, CONFIGServiceModel.AddMacLogs(temp), "");// 20170111
                }
                else
                {
                    _COM = Model.MACServiceModel.QueryMACByCode(_COMCode);
                    if (_COM != null)
                    {
                        if (_COM != temp)
                        {
                            temp.ID = _COM.ID;
                            temp.InfoState = InfoState.Modified;
                            try
                            {
                                Model.MACServiceModel.UpdateMACCache(temp);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            OperateLogHelper.InsertOperateLog(11, CONFIGServiceModel.UpdateMacLogs(_COM, temp), "");// 20170111
                        }
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                Jc_MacInfo temp = new Jc_MacInfo();
                Jc_MacInfo _COM;
                temp.MAC = this.CcmbSerialPortNum.Text;
                temp.Type = 1;
                temp.Bz1 = CcmbBaudRate.Text;
                temp.Bz2 = CcmbCommPAL.Text;
                temp.Bz3 = CcmbDatabit.Text;
                temp.Bz4 = CcmbCheckbit.Text;
                temp.Bz5 = CcmbStopbit.Text;
                if (string.IsNullOrEmpty(_COMCode))
                {
                    temp.ID = IdHelper.CreateLongId().ToString();
                    temp.InfoState = InfoState.AddNew;
                    try
                    {
                        Model.MACServiceModel.AddMACCache(temp);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    OperateLogHelper.InsertOperateLog(11, CONFIGServiceModel.AddMacLogs(temp), "");// 20170111
                }
                else
                {
                    _COM = Model.MACServiceModel.QueryMACByCode(_COMCode);
                    if (_COM != null)
                    {
                        if (_COM != temp)
                        {
                            temp.ID = _COM.ID;
                            temp.InfoState = InfoState.Modified;
                            try
                            {
                                Model.MACServiceModel.UpdateMACCache(temp);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            OperateLogHelper.InsertOperateLog(11, CONFIGServiceModel.UpdateMacLogs(_COM, temp), "");// 20170111
                        }
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}
