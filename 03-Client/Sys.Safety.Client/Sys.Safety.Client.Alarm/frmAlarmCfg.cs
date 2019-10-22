using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Common;
using Sys.Safety.DataContract;
using Sys.Safety.ClientFramework.UserRoleAuthorize;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmAlarmCfg : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 标记，用于标注当前是性质、种类还是测点
        /// 1-性质 2-种类 3-测点 0-没有数据，不进行保存
        /// </summary>
        private int iFlag = 0;
        /// <summary>
        /// 当前修改是否已经保存，当前修改没有保存则需要提示是否进行保存
        /// </summary>
        private bool bIsChangeType = false;
        private bool bIsChangeAlarm = false;
        public frmAlarmCfg()
        {
            InitializeComponent();
        }

        private void frmAlarmCfg_Load(object sender, EventArgs e)
        {
            try
            {
                //设备性质、种类
                LookupLoadData();
                //SmartReader.Read("测试语音", 0);//记得添加smartread6.dll文件 需放置在与主程序同级目录下才能被有效使用
                //注册委托事件，便于观察报警记录获取的情况
                //ClientAlarmMain.GetInstance().OnAlarmShow += new ClientAlarmMain.AlarmShowEventHandle(ClientAlarmMain_OnAlarmShow);
            }
            catch (Exception ex)
            {
                LogHelper.Error("读声音报警出错，Read（） 错误原因： ", ex);
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, ex.Message);
            }
        }

        private void LookupLoadData()
        {
            try
            {
                this.repositoryItemCheckedComboBoxEdit1.DataSource = GetAlarmShow();
                this.lookUpEditDefProperty.Properties.DataSource = ClientAlarmServer.GetListEnumProperty();

                ClientAlarmConfig.LoadConfigToCache();

                this.chk_IsUseAlarmCfg.Checked = ClientAlarmConfigCache.IsUseAlarmConfig;
                this.chkIsUsePopupAlarm.Checked = ClientAlarmConfigCache.IsUsePopupAlarm;

                this.lookUpEditDefProperty.ItemIndex = 0;
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载设备性质发生异常 " + ex.Message);
            }
        }

        private static DataTable GetAlarmShow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("show", typeof(string));
            dt.Columns.Add("alarmShow", typeof(string));
            //dt.Rows.Add("不处理", "0");
            dt.Rows.Add("语音播报", "1");
            dt.Rows.Add("声光报警", "2");
            dt.Rows.Add("图文弹窗", "3");
            dt.Rows.Add("蜂鸣器报警", "4");
            return dt;
        }
        /// <summary>
        /// 刷新加载Grid的数据源
        /// </summary>
        /// <param name="sProperty">性质代码</param>
        /// <param name="col">（性质）显示名称</param>
        /// <returns></returns>
        private static IList<ClientAlarmItems> GetDataGrid(string sProperty, string col)
        {
            IList<ClientAlarmItems> list = new List<ClientAlarmItems>();
            try
            {
                DataTable dt = ClientAlarmServer.GetAlarmTypeDataByProperty(sProperty, col);
                list= ObjectConverter.Copy<ClientAlarmItems>(dt);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return list;
        }

        private void dropDownButton_save_Click(object sender, EventArgs e)
        {
            this.popupMenu_save.ShowPopup(Control.MousePosition);
        }
        /// <summary>
        /// 取消/退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 保存配置到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnItem_saveLocal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //保存到本地文件
            SaveToLocal();
        }
        /// <summary>
        /// 保存当前报警配置到本地
        /// </summary>
        /// <returns></returns>
        private bool SaveToLocal()
        {
            bool b = false;
            try
            {
                if (InvalidGridDataBeforeSave())
                {
                    if (ClientAlarmConfig.SaveConfig(iFlag, gridCtrl.DataSource as List<ClientAlarmItems>))
                    {
                        ClientAlarmConfigCache.IsUsePopupAlarm = this.chkIsUsePopupAlarm.Checked;
                        ClientAlarmConfigCache.IsUseAlarmConfig = this.chk_IsUseAlarmCfg.Checked;
                        ClientAlarmConfig.SaveOtherAlarmSwitch();

                        ClientAlarmConfig.LoadConfigToCache();
                        Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存报警设置成功");
                        b = true;
                    }
                    else
                    {
                        Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存报警设置失败");
                    }
                    //性质、种类、测点发生改变时
                    bIsChangeType = false;
                    bIsChangeAlarm = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return b;
        }
        /// <summary>
        /// 保存之前对grid的数据进行检查，检查通过方可进行保存
        /// </summary>
        /// <returns></returns>
        private bool InvalidGridDataBeforeSave()
        {
            bool b = true;
            try
            {
                List<ClientAlarmItems> list = this.gridCtrl.DataSource as List<ClientAlarmItems>;
                object oType = lookUpEditDefProperty.EditValue;
                int iType = oType == null ? -1 : int.Parse(oType.ToString());
                if (iFlag == 0 || list == null || list.Count < 1)
                {
                    b = false;
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "当前没有可保存的数据！");
                }
                if (!chk_IsUseAlarmCfg.Checked)
                {
                    if (Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Confirm
                        , "当前未启用报警配置，所有报警配置处于无效状态，产生相应报警信息，系统也不会做出报警提示！确定要保存吗？") == DialogResult.No)
                    {
                        b = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return b;
        }

        /// <summary>
        /// 保存配置到服务器，先保存到本地，再保存到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnItem_saveServer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //增加权限验证  20171011
                bool blnHaveRight = ClientPermission.Authorize("SaveAlarmConfigInServer");
                if (!blnHaveRight)
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "当前用户无操作权限！");
                    return;
                }

                if (!InvalidGridDataBeforeSave())
                {
                    return;
                }
                //先保存报警配置到本地文件
                if (!ClientAlarmConfig.SaveConfig(iFlag, gridCtrl.DataSource as List<ClientAlarmItems>))
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存报警设置到服务器失败");
                    return;
                }
                ClientAlarmConfigCache.IsUseAlarmConfig = this.chk_IsUseAlarmCfg.Checked;
                ClientAlarmConfigCache.IsUsePopupAlarm = this.chkIsUsePopupAlarm.Checked;
                ClientAlarmConfig.SaveOtherAlarmSwitch();

                ClientAlarmConfig.LoadConfigToCache();

                //再保存到服务器数据库
                bool b = ClientAlarmConfig.SaveConfigToServer();
                //性质、种类、测点发生改变时
                bIsChangeType = false;
                bIsChangeAlarm = false;
                if (b)
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存报警设置到服务器成功");
                }
                else
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存报警设置到服务器失败");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 从服务端下载配置应用到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnItem_getFromServer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //判断服务器是否存在报警配置
                SettingInfo dto;
                dto = ClientAlarmServer.CheckAlarmConfigIsOnServer();
                if (dto == null || string.IsNullOrEmpty(dto.StrValue))
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "服务器上未发现配置文件，操作已取消。");
                    return;
                }
                //从服务器下载配置到本地
                if (!ClientAlarmConfig.DownloadConfigFromServer())
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "服务器上未发现配置文件，操作已取消。");
                    return;
                }
                //从本地配置加载到缓存
                ClientAlarmConfig.LoadConfigToCache();
                gridCtrl.DataSource = null;
                lookUpEditDefProperty.EditValue = null;
                lookUpEditDefCalss.EditValue = null;
                textEditDefPoint.EditValue = null;
                bIsChangeType = false;
                bIsChangeAlarm = false;
                LookupLoadData();
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "从服务端下载配置应用到本地成功");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 绘序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 性质切换时提醒用户是否对之前的操作进行保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditDefProperty_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (this.lookUpEditDefProperty.EditValue == null || this.lookUpEditDefProperty.Text == "") { return; }
                if (bIsChangeType && bIsChangeAlarm)
                {
                    if (Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Confirm, "是否保存刚才的配置？") == DialogResult.No) { return; }
                    SaveToLocal();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void lookUpEditDefCalss_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!this.lookUpEditDefCalss.Focused) { return; }
                if (this.lookUpEditDefCalss.EditValue == null || this.lookUpEditDefCalss.Text == "" || this.lookUpEditDefCalss.EditValue.ToString() == "0") { return; }
                if (bIsChangeType && bIsChangeAlarm)
                {
                    if (Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Confirm, "是否保存刚才的配置？") == DialogResult.No) { return; }
                    SaveToLocal();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void textEditDefPoint_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!this.textEditDefPoint.Focused) { return; }
                if (this.textEditDefPoint.EditValue == null || this.textEditDefPoint.Text == "" || this.textEditDefPoint.EditValue.ToString() == "0") { return; }
                if (bIsChangeType && bIsChangeAlarm)
                {
                    if (Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Confirm, "是否保存刚才的配置？") == DialogResult.No) { return; }
                    SaveToLocal();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void lookUpEditDefProperty_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //根据性质选择联动种类、测点数据源
                object oProperty = lookUpEditDefProperty.EditValue;
                if (oProperty == null || string.IsNullOrEmpty(oProperty.ToString()))
                {
                    lookUpEditDefCalss.EditValue = null;
                    lookUpEditDefCalss.Properties.DataSource = null;
                    textEditDefPoint.EditValue = null;
                    textEditDefPoint.Properties.DataSource = null;
                    devlookUpEdit.EditValue = null;
                    devlookUpEdit.Properties.DataSource = null;
                    gridCtrl.DataSource = null;
                    return;
                }
                //性质、种类、测点发生改变时
                bIsChangeType = true;
                //DataTable dt = new ClientAlarmServer().GetClassByProperty(oProperty.ToString());
                this.lookUpEditDefCalss.EditValue = null;
                this.lookUpEditDefCalss.Properties.DataSource = ClientAlarmServer.GetDevClassByDevpropertyID(int.Parse(oProperty.ToString()));
                //联动设备类型
                this.devlookUpEdit.EditValue = null;
                this.devlookUpEdit.Properties.DataSource = ClientAlarmServer.GetDevByProperty(int.Parse(oProperty.ToString()));
                //联动测点数据源，即根据性质找测点
                this.textEditDefPoint.EditValue = null;
                this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevPropertyID(int.Parse(oProperty.ToString()));
                
                iFlag = 1;
                //根据选择的性质，展示该性质的报警类型，设置该性质的各报警类型对应的报警方式
                IList<ClientAlarmItems> list = Basic.Framework.Common.JSONHelper.ParseJSONString < List < ClientAlarmItems >>
                    (Basic.Framework.Common.JSONHelper.ToJSONString(ClientAlarmConfigCache.listProperty.FindAll(a => a.code == oProperty.ToString())));
                if (list == null || list.Count < 1)
                {
                    this.gridCtrl.DataSource = GetDataGrid(oProperty.ToString(), lookUpEditDefProperty.Text);
                }
                else
                {
                    this.gridCtrl.DataSource = list;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void lookUpEditDefCalss_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //根据种类的选择联动测点数据源
                object oClass = this.lookUpEditDefCalss.EditValue;
                if (lookUpEditDefCalss.EditValue != null) { iFlag = 2; }
                else { return; }
                //性质、种类、测点发生改变时
                bIsChangeType = true;
                //再根据种类，于Grid中展示该种类所有的测点
                //从数据库加载所有的该种类的测点，然后结合该性质的报警类型，作为Grid的数据源
                string name = "";
                string code = "";
                List<ClientAlarmItems> listAll = new List<ClientAlarmItems>(); ;
                IList<ClientAlarmItems> Ilist = null;
                switch (iFlag)
                {                 
                    case 2://种类
                        name = lookUpEditDefCalss.Text;
                        code = lookUpEditDefCalss.EditValue.ToString();
                        listAll = DeepClone(ClientAlarmConfigCache.listClass) as List<ClientAlarmItems>;//先找当前种类的报警配置
                        Ilist = listAll.FindAll(a => a.code == code);
                        if (Ilist == null || Ilist.Count < 1)
                        {
                            Ilist = gridCtrl.DataSource as List<ClientAlarmItems>;
                            if (Ilist == null || Ilist.Count < 1) { return; }
                            for (int i = 0; i < Ilist.Count; i++)
                            {
                                Ilist[i].code = code;
                                Ilist[i].name = name;
                                Ilist[i].alarmShow = "";
                            }
                        }
                        //this.textEditDefPoint.EditValue = null;
                        //this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevClassID(int.Parse(code));
                        //加载设备类型
                        this.devlookUpEdit.EditValue = null;
                        this.devlookUpEdit.Properties.DataSource = ClientAlarmServer.GetDevByClass(int.Parse(code));
                        break;
                    //case 1:
                    //    name = lookUpEditDefProperty.Text;
                    //    code = lookUpEditDefProperty.EditValue.ToString();
                    //    listAll = DeepClone(ClientAlarmConfigCache.listProperty) as List<ClientAlarmItems>;
                    //    Ilist = listAll.FindAll(a => a.code == code);
                    //    if (Ilist == null || Ilist.Count < 1)
                    //    {
                    //        Ilist = GetDataGrid(code, name);//最后从枚举数据中找
                    //    }
                    //    this.textEditDefPoint.EditValue = null;
                    //    this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevPropertyID(int.Parse(code));
                    //    break;
                }
                this.gridCtrl.DataSource = Ilist;
                this.gridCtrl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void textEditDefPoint_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                object oPoint = this.textEditDefPoint.EditValue;
                //if (lookUpEditDefProperty.EditValue == null) { iFlag = 0; }
                //else { iFlag = 1; }               
                //if (lookUpEditDefCalss.EditValue != null) { iFlag = 2; }
                //if (devlookUpEdit.EditValue != null) { iFlag = 4; }
                if (textEditDefPoint.EditValue != null) { iFlag = 3; }
                else { return; }
                //性质、种类、测点发生改变时
                bIsChangeType = true;
                string name = string.Empty;
                string code = string.Empty;
                List<ClientAlarmItems> listAll = new List<ClientAlarmItems>();
                IList<ClientAlarmItems> Ilist = null;
                switch (iFlag)
                {
                    case 3:
                        name = textEditDefPoint.Text;
                        code = textEditDefPoint.EditValue.ToString();
                        listAll = DeepClone(ClientAlarmConfigCache.listPoint) as List<ClientAlarmItems>;
                        Ilist = listAll.FindAll(a => a.code == code);
                        if (Ilist == null || Ilist.Count < 1)
                        {
                            Ilist = gridCtrl.DataSource as List<ClientAlarmItems>;
                            if (Ilist == null || Ilist.Count < 1) { return; }
                            for (int i = 0; i < Ilist.Count; i++)
                            {
                                Ilist[i].code = code;
                                Ilist[i].name = name;
                                Ilist[i].alarmShow = "";
                            }
                        }
                        break;
                    //case 2:
                    //    name = lookUpEditDefCalss.Text;
                    //    code = lookUpEditDefCalss.EditValue.ToString();
                    //    listAll = DeepClone(ClientAlarmConfigCache.listClass) as List<ClientAlarmItems>;
                    //    Ilist = listAll.FindAll(a => a.code == code);
                    //    if (Ilist == null || Ilist.Count < 1)
                    //    {
                    //        Ilist = gridCtrl.DataSource as List<ClientAlarmItems>;
                    //        if (Ilist == null || Ilist.Count < 1) { return; }
                    //        for (int i = 0; i < Ilist.Count; i++)
                    //        {
                    //            Ilist[i].code = code;
                    //            Ilist[i].name = name;
                    //            Ilist[i].alarmShow = "";
                    //        }
                    //    }
                    //    this.textEditDefPoint.EditValue = null;
                    //    this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevClassID(int.Parse(code));
                    //    break;
                    //case 1:
                    //    name = lookUpEditDefProperty.Text;
                    //    code = lookUpEditDefProperty.EditValue.ToString();
                    //    listAll = DeepClone(ClientAlarmConfigCache.listProperty) as List<ClientAlarmItems>;
                    //    Ilist = listAll.FindAll(a => a.code == code);
                    //    if (Ilist == null || Ilist.Count < 1)
                    //    {
                    //        Ilist = GetDataGrid(code, name);//最后从枚举数据中找
                    //    }
                    //    this.textEditDefPoint.EditValue = null;
                    //    this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevPropertyID(int.Parse(code));
                    //    break;
                }
                this.gridCtrl.DataSource = Ilist;
                this.gridCtrl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void repositoryItemCheckedComboBoxEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                bIsChangeAlarm = true;
                CheckedComboBoxEdit comboBox = sender as CheckedComboBoxEdit;
                if (comboBox == null) { return; }
                if (comboBox.EditValue == null || comboBox.EditValue.ToString() == "")
                {
                    comboBox.EditValue = "无设置";
                    //comboBox.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void chk_autoLoadCfgFromSvr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //性质、种类、测点发生改变时
                //bIsSaved = false;
                if (chk_autoLoadCfgFromSvr.Checked)
                {
                    ClientAlarmConfigCache.IsAutoLoadAlarmConfigFromServer = true;
                }
                else
                {
                    ClientAlarmConfigCache.IsAutoLoadAlarmConfigFromServer = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        public object DeepClone(object ldc)
        {
            object clonedObj = null;
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                Formatter.Serialize(stream, ldc);
                stream.Position = 0;
                clonedObj = Formatter.Deserialize(stream);
                stream.Close();

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return clonedObj;
        }

        private void chk_IsUseAlarmCfg_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void devlookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //根据种类的选择联动测点数据源
                object oClass = this.devlookUpEdit.EditValue;
                if (devlookUpEdit.EditValue != null) { iFlag = 4; }
                else { return; }
                //else if (lookUpEditDefCalss.EditValue != null) { iFlag = 2; }
                //else { iFlag = 1; }
                //性质、种类、测点发生改变时
                bIsChangeType = true;
                //再根据种类，于Grid中展示该种类所有的测点
                //从数据库加载所有的该种类的测点，然后结合该性质的报警类型，作为Grid的数据源
                string name = "";
                string code = "";
                List<ClientAlarmItems> listAll = new List<ClientAlarmItems>(); ;
                IList<ClientAlarmItems> Ilist = null;
                switch (iFlag)
                {
                    //case 1://性质                   
                    //    name = lookUpEditDefProperty.Text;
                    //    code = lookUpEditDefProperty.EditValue.ToString();
                    //    listAll = DeepClone(ClientAlarmConfigCache.listProperty) as List<ClientAlarmItems>;
                    //    Ilist = listAll.FindAll(a => a.code == code);
                    //    if (Ilist == null || Ilist.Count < 1)
                    //    {
                    //        Ilist = GetDataGrid(code, name);//最后从枚举数据中找
                    //    }
                    //    this.textEditDefPoint.EditValue = null;
                    //    this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevPropertyID(int.Parse(code));                       
                    //    break;
                    //case 2://种类
                    //    name = lookUpEditDefCalss.Text;
                    //    code = lookUpEditDefCalss.EditValue.ToString();
                    //    listAll = DeepClone(ClientAlarmConfigCache.listClass) as List<ClientAlarmItems>;//先找当前种类的报警配置
                    //    Ilist = listAll.FindAll(a => a.code == code);
                    //    if (Ilist == null || Ilist.Count < 1)
                    //    {
                    //        Ilist = gridCtrl.DataSource as List<ClientAlarmItems>;
                    //        if (Ilist == null || Ilist.Count < 1) { return; }
                    //        for (int i = 0; i < Ilist.Count; i++)
                    //        {
                    //            Ilist[i].code = code;
                    //            Ilist[i].name = name;
                    //            Ilist[i].alarmShow = "";
                    //        }
                    //    }
                    //    this.textEditDefPoint.EditValue = null;
                    //    this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevClassID(int.Parse(code));                        
                    //    break;
                    case 4:
                        name = devlookUpEdit.Text;
                        code = devlookUpEdit.EditValue.ToString();
                        listAll = DeepClone(ClientAlarmConfigCache.listDev) as List<ClientAlarmItems>;
                        Ilist = listAll.FindAll(a => a.code == code);
                        if (Ilist == null || Ilist.Count < 1)
                        {
                            Ilist = gridCtrl.DataSource as List<ClientAlarmItems>;
                            if (Ilist == null || Ilist.Count < 1) { return; }
                            for (int i = 0; i < Ilist.Count; i++)
                            {
                                Ilist[i].code = code;
                                Ilist[i].name = name;
                                Ilist[i].alarmShow = "";
                            }
                        }
                        this.textEditDefPoint.EditValue = null;
                        this.textEditDefPoint.Properties.DataSource = ClientAlarmServer.GetPointByDevID(int.Parse(code));
                        break;
                }
                this.gridCtrl.DataSource = Ilist;
                this.gridCtrl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void devlookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!this.devlookUpEdit.Focused) { return; }
                if (this.devlookUpEdit.EditValue == null || this.devlookUpEdit.Text == "" || this.devlookUpEdit.EditValue.ToString() == "0") { return; }
                if (bIsChangeType && bIsChangeAlarm)
                {
                    if (Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.MessageType.Confirm, "是否保存刚才的配置？") == DialogResult.No) { return; }
                    SaveToLocal();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }
}