using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.Client.Define.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class MultiSelect : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 父窗体
        /// </summary>
        private CFCMSensorType _CFCMSensorType;
        /// <summary>
        /// 选择的多参数个数
        /// </summary>
        private int _SelMult;
        /// <summary>
        /// 选择的多参数列表，用“|”分隔
        /// </summary>
        private string _SelMultChanel;
        /// <summary>
        /// 设备性质ID
        /// </summary>
        private string _DevPtyID;
        public MultiSelect(CFCMSensorType CFCMSensorType, int SelMult, string SelMultChanel, string DevPtyID)
        {
            InitializeComponent();
            _CFCMSensorType = CFCMSensorType;
            _SelMult = SelMult;
            _SelMultChanel = SelMultChanel;
            _DevPtyID = DevPtyID;
        }

        private void MultiSelect_Load(object sender, EventArgs e)
        {
            try
            {
                if (_DevPtyID != "1" && _DevPtyID != "2" && _DevPtyID != "3")
                {
                    XtraMessageBox.Show("只有模拟量、开关量、控制量可以设置多参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                //加载选择的所有设备类型,副通道只能绑定模拟量和开关量(绑定控制量智能分站不支持)  20170623
                List<Jc_DevInfo> DevList = DEVServiceModel.QueryDevsCache().ToList();
                DevList = DevList.FindAll(a => a.Type == 1 || a.Type == 2);


                foreach (Jc_DevInfo Dev in DevList)
                {
                    listBoxControl1.Items.Add(Dev.Devid + "." + Dev.Name);
                }
                //加载已经选择的设备类型
                string[] TempSelChanel = _SelMultChanel.Split('|');
                foreach (string Chanel in TempSelChanel)
                {
                    if (!string.IsNullOrEmpty(Chanel))
                    {
                        Jc_DevInfo TempDev = DevList.Find(a => a.Devid == Chanel);
                        if (TempDev != null)
                        {
                            listBoxControl2.Items.Add(TempDev.Devid + "." + TempDev.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //数据验证
                if (listBoxControl2.Items.Count != _SelMult - 1)
                { //选择副通道数量不等于多参数个数-1则提示
                    XtraMessageBox.Show("选择的副通道个数与多参数个数不匹配，副通道个数等于多参数个数减1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string TempSelMultChanel = "";
                for (int i = 0; i < listBoxControl2.Items.Count; i++)
                {
                    TempSelMultChanel += listBoxControl2.Items[i].ToString().Substring(0, listBoxControl2.Items[i].ToString().IndexOf('.')) + "|";
                }
                if (TempSelMultChanel.Contains("|"))
                {
                    TempSelMultChanel = TempSelMultChanel.Substring(0, TempSelMultChanel.Length - 1);
                }
                _CFCMSensorType.textEdit1.Text = TempSelMultChanel;

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = listBoxControl1.SelectedItems.Count - 1; i >= 0; i--)
                {
                    string DevId = listBoxControl1.SelectedItems[i].ToString().Substring(0, listBoxControl1.SelectedItems[i].ToString().IndexOf('.'));
                    string DevName = listBoxControl1.SelectedItems[i].ToString().Substring(listBoxControl1.SelectedItems[i].ToString().IndexOf('.') + 1);
                    listBoxControl2.Items.Add(DevId + "." + DevName);
                    //listBoxControl1.Items.Remove(listBoxControl1.SelectedItems[i]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = listBoxControl2.SelectedItems.Count - 1; i >= 0; i--)
                {
                    string DevId = listBoxControl2.SelectedItems[i].ToString().Substring(0, listBoxControl2.SelectedItems[i].ToString().IndexOf('.'));
                    string DevName = listBoxControl2.SelectedItems[i].ToString().Substring(listBoxControl2.SelectedItems[i].ToString().IndexOf('.') + 1);
                    //listBoxControl1.Items.Add(DevId + "." + DevName);
                    listBoxControl2.Items.Remove(listBoxControl2.SelectedItems[i]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}
