using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.ClientFramework.Model;
using Sys.Safety.ClientFramework.View.Message;
using Sys.Safety.DataContract;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;

namespace Sys.Safety.ClientFramework.View.Setting
{
    public partial class frmSetting : DevExpress.XtraEditors.XtraForm
    {
        private IList<long> delDetailItems = new List<long>();
        SettingModel Model = new SettingModel();

        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void FillGrid()
        {
            DataTable dt = ObjectConverter.ToDataTable<SettingInfo>(Model.GetSettingList());
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
                for (int i = 0; i < this.gridView1.RowCount; i++)
                {
                    dto = new SettingInfo();
                    long ID = 0;
                    try
                    {
                        ID = Convert.ToInt64(gridView1.GetRowCellValue(i, "ID"));
                    }
                    catch
                    {

                    }
                    if (ID == 0)
                    {
                        dto.InfoState = InfoState.AddNew;
                    }
                    else
                    {
                        dto.InfoState = InfoState.Modified;
                    }
                    dto.ID = ID.ToString();
                    dto.StrType = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrType"));
                    dto.StrKey = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrKey"));
                    dto.StrKeyCHs = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrKeyCHs"));
                    dto.StrValue = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrValue"));
                    dto.StrDesc = TypeConvert.ToString(gridView1.GetRowCellValue(i, "StrDesc"));
                    dto.Creator = RequestUtil.GetParameterValue("userId");
                    dto.LastUpdateDate = DateTime.Now.ToString();
                    Model.SaveSetting(dto);
                }
                foreach (long delID in delDetailItems)
                {
                    dto = new SettingInfo();
                    dto.ID = delID.ToString();
                    dto.InfoState = InfoState.Delete;
                    Model.DeleteSetting(dto);
                }
                this.delDetailItems.Clear();
                FillGrid();
                DevMessageBox.Show(DevMessageBox.MessageType.Information, "保存成功");
            }
            catch (System.Exception ex)
            {
                DevMessageBox.Show(DevMessageBox.MessageType.Stop, "保存失败,错误原因为" + ex.Message);
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
                    DevMessageBox.Show(DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行strKey列为空，不能保存");
                    return false;
                }
                string StrValue = TypeConvert.ToString(gridView1.GetRowCellValue(i - 1, "StrValue"));
                if (StrValue.Trim() == "")
                {
                    DevMessageBox.Show(DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行值列为空，不能保存");
                    return false;
                }
                if (lists.Contains(StrKey))
                {
                    DevMessageBox.Show(DevMessageBox.MessageType.Warning, "第" + i.ToString() + "行值列" + StrKey + "已和前面行重复，请重新输入！");
                    return false;
                }
                else
                {
                    lists.Add(StrKey);
                }
            }
            return true;
        }


        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)//如果Grid有数据显示
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();//因为数据表里面的数据是从0开始，为了方便，所以加1
            }
        }

        private void repBtnDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int rowHandle = gridView1.FocusedRowHandle;
                if (rowHandle < 0)
                    return;
                DialogResult dr = DevExpress.XtraEditors.XtraMessageBox.Show("是否删除当前数据？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    long Id = 0;
                    try
                    {
                        Id = Convert.ToInt64(gridView1.GetRowCellValue(rowHandle, "ID"));
                    }
                    catch
                    {

                    }                    
                    if (Id > 0)
                        this.delDetailItems.Add(Id);
                    gridView1.DeleteSelectedRows();

                }
            }
            catch (System.Exception ex)
            {
                DevMessageBox.Show(Message.DevMessageBox.MessageType.Stop, ex.Message);
            }
        }

        private void tlbClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


    }
}