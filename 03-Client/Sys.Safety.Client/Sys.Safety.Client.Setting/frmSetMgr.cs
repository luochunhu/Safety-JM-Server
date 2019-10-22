using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Alarm;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Sys.Safety.ClientFramework.CBFCommon;



namespace Sys.Safety.Client.Setting
{
    public partial class frmSetMgr : DevExpress.XtraEditors.XtraForm
    {
        private IList<string> delDetailItems = new List<string>();
        SettingModel Model = new SettingModel();
        public frmSetMgr()
        {
            InitializeComponent();
        }

        private void frmSetMgr_Load(object sender, EventArgs e)
        {
            FillGrid();
            //lookup加载数据源 来自 枚举表
            //ClientAlarmServer ser = new ClientAlarmServer();
            this.repositoryItemLookUpEdit1.DataSource = ClientAlarmServer.GetListEnumAlarmType();
            this.repositoryItemLookUpEdit1.DisplayMember = "strEnumDisplay";
            this.repositoryItemLookUpEdit1.ValueMember = "StrType";
        }

        private void FillGrid()
        {
            IList<SettingInfo> list = Model.GetSettingList();
            DataTable dt = ObjectConverter.ToDataTable(list);
            if (dt == null || dt.Rows.Count < 1) { return; }
            DataRow[] dra = dt.Select("StrKey='SoundLightPortName'");
            if (dra == null || dra.Length < 1) { }
            else
            {
                dra[0]["StrValue"] = ClientAlarmConfigCache.soundLightPortName;
            }
            dra = dt.Select("StrKey='SoundLightBaudRate'");
            if (dra == null || dra.Length < 1) { }
            else
            {
                dra[0]["StrValue"] = ClientAlarmConfigCache.soundLightBaudRate.ToString();
            }
            this.gridControl1.DataSource = dt;
        }

        private void tlbSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.gridView1.CloseEditor();
                this.gridView1.UpdateCurrentRow();
                if (!CheckValid()) return;
                SettingInfo dto = null;
                //通讯/数采相关的配置 按照数采提供的结构 把变化的配置项 按字典推送过去
                Dictionary<string, SettingInfo> dicDataCollector = new Dictionary<string, SettingInfo>();
                //班次配置列表
                List<ClassInfo> listClassDTO = new List<ClassInfo>();

                for (int i = 0; i < this.gridView1.RowCount; i++)
                {
                    dto = new SettingInfo();
                    string settingId = gridView1.GetRowCellValue(i, "ID").ToString();
                    int state = string.IsNullOrWhiteSpace(settingId) ? 1 : 2;
                    dto.ID = settingId;
                    dto.StrType = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrType"));
                    dto.StrKey = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrKey"));
                    dto.StrKeyCHs = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrKeyCHs"));
                    dto.StrValue = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrValue"));
                    dto.StrDesc = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrDesc"));
                    dto.Creator = RequestUtil.GetParameterValue("userId");
                    dto.LastUpdateDate = DateTime.Now.ToString();
                    Model.SaveSetting(dto, state);

                    if (dto.StrKey == "SoundLightPortName")//声光报警配置 只存本地不存数据库
                    {
                        ClientAlarmConfigCache.soundLightPortName = dto.StrValue;
                    }
                    if (dto.StrKey == "SoundLightBaudRate")//声光报警配置 只存本地不存数据库
                    {
                        int iBaudRate = 9600;
                        bool bIsOk = int.TryParse(dto.StrValue, out iBaudRate);
                        ClientAlarmConfigCache.soundLightBaudRate = iBaudRate;
                    }

                    //班次配置除了要存setting表外，还需要存班次表
                    if (dto.StrType == "8")//班次配置
                    {
                        var dtoClass = new ClassInfo();
                        if (state == 2)
                        {
                            dtoClass = new Sys.Safety.Client.Setting.Model.ClassModel().GetClassDtoByKey(dto.StrKey);
                            state = dtoClass == null ? 1 : 2;
                            dtoClass.StrCode = dto.StrKey;
                            dtoClass.StrName = dto.StrKeyCHs;
                            dtoClass.DatStart = string.IsNullOrEmpty(dto.StrValue) ? "" : dto.StrValue.Split('|')[0];
                            dtoClass.DatEnd = string.IsNullOrEmpty(dto.StrValue) ? "" : dto.StrValue.Split('|')[1];
                            new Sys.Safety.Client.Setting.Model.ClassModel().SaveClass(dtoClass, state);
                        }

                    }
                }
                //声光报警串口的设置保存到本地配置文件
                ClientAlarmConfig.SaveSoundLighPortSetting();

                //执行删除操作
                foreach (string delID in delDetailItems)
                {
                    Model.DeleteSetting(delID);
                }
                this.delDetailItems.Clear();
                FillGrid();

                //IDictionary<string, string> dic = ClientContext.Current.GetContext("CustomerSetting") as Dictionary<string, string>;
                //for (int i = 0; i < gridView1.RowCount; i++)
                //{
                //    string strKey = Convert.ToString(gridView1.GetRowCellValue(i, "StrKey"));
                //    string strValue = Convert.ToString(gridView1.GetRowCellValue(i, "StrValue"));
                //    if (dic.Keys.Contains(strKey))
                //        dic[strKey] = strValue;
                //}
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Information, "保存成功！");
            }
            catch (System.Exception ex)
            {
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, "保存失败！错误原因为" + ex.Message);
            }
        }

        private bool CheckValid()
        {
            IList<string> lists = new List<string>();
            for (int i = 1; i <= this.gridView1.RowCount; i++)
            {
                string StrKey = TypeConvert.ToString(gridView1.GetRowCellValue(i - 1, "StrKey"));
                if (StrKey.Trim() == "")
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行strKey列为空，不能保存");
                    return false;
                }
                string StrValue = TypeConvert.ToString(gridView1.GetRowCellValue(i - 1, "StrValue"));
                if (StrValue.Trim() == "")
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行值列为空，不能保存");
                    return false;
                }
                if (lists.Contains(StrKey))
                {
                    Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行值列" + StrKey + "已和前面行重复，请重新输入！");
                    return false;
                }
                else
                {
                    lists.Add(StrKey);
                }
            }
            return true;
        }

        private void tlbClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)//如果Grid有数据显示
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();//因为数据表里面的数据是从0开始，为了方便，所以加1
            }
        }

        private void repBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = gridView1.FocusedRowHandle;
                if (rowHandle < 0)
                    return;
                //DialogResult dr = MessageBox.Show("是否删除当前数据？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                DialogResult dr = Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Confirm, "是否删除当前数据？");
                if (dr == DialogResult.Yes)
                {
                    string Id = "";
                    try
                    {
                        Id = gridView1.GetRowCellValue(rowHandle, "ID").ToString();
                    }
                    catch
                    {

                    }
                    if (!string.IsNullOrWhiteSpace(Id))
                    {
                        if (gridView1.GetRowCellValue(rowHandle, "StrType").ToString() == "8")//班次 删除班次配置的同时还要删除班次表
                        {
                            new Sys.Safety.Client.Setting.Model.ClassModel().DeleteClassBySql(gridView1.GetRowCellValue(rowHandle, "StrKey").ToString());
                        }
                        this.delDetailItems.Add(Id);
                    }
                    gridView1.DeleteSelectedRows();
                }
            }
            catch (System.Exception ex)
            {
                Sys.Safety.ClientFramework.View.Message.DevMessageBox.Show(Sys.Safety.ClientFramework.View.Message.DevMessageBox.MessageType.Warning, ex.Message);
            }
        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView gv = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gv.GetRowCellValue(gv.FocusedRowHandle, "ID") == null) { return; }
            if (gv.GetRowCellValue(gv.FocusedRowHandle, "StrDesc").ToString().StartsWith("--"))
            {
                e.Cancel = true;
            }
        }
    }
}