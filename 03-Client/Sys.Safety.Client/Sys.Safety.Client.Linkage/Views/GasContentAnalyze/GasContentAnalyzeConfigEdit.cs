using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Client.Linkage.Handlers;

namespace Sys.Safety.Client.Linkage.Views.GasContentAnalyze
{
    public partial class GasContentAnalyzeConfigEdit : DevExpress.XtraEditors.XtraForm
    {
        public GasContentAnalyzeConfigEdit()
        {
            InitializeComponent();
        }

        private void GasContentAnalyzeConfigEdit_Load(object sender, EventArgs e)
        {
            try
            {
                lookUpEditPoint.Properties.NullText = "未选择";
                lookUpEditPoint.Properties.ValueMember = "Id";
                lookUpEditPoint.Properties.DisplayMember = "Text";
                var pointPropertyIds = new List<int>{
                    1
                };
                var points = PointHandler.GetMonitoringSystemPointByPropertyIds(pointPropertyIds);
                var lisPoint = new List<PointInfo>();
                foreach (var item in points)
                {
                    var newItem = new PointInfo
                    {
                        Id = item.PointID,
                        Text = item.Point,
                        Location = item.Wz,
                        Type = item.DevName
                    };
                    lisPoint.Add(newItem);
                }
                lookUpEditPoint.Properties.DataSource = lisPoint;

            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItemAffirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (lookUpEditPoint.EditValue == null ||
                    string.IsNullOrEmpty(textEditHeight.Text) ||
                    string.IsNullOrEmpty(textEditWidth.Text)||
                    string.IsNullOrEmpty(textEditThickness.Text)||
                    string.IsNullOrEmpty(textEditSpeed.Text)||
                    string.IsNullOrEmpty(textEditLength.Text)||
                    string.IsNullOrEmpty(textEditAcreage.Text)||
                    string.IsNullOrEmpty(textEditPercent.Text)||
                    string.IsNullOrEmpty(textEditWind.Text)||
                    string.IsNullOrEmpty(textEditComparevalue.Text))
                {
                    XtraMessageBox.Show("请录入完整信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var info = new GascontentanalyzeconfigInfo
                {
                    Pointid = lookUpEditPoint.EditValue.ToString(),
                    Height = textEditHeight.Text,
                    Width = textEditWidth.Text,
                    Thickness = textEditThickness.Text,
                    Speed = textEditSpeed.Text,
                    Length = textEditLength.Text,
                    Acreage = textEditAcreage.Text,
                    Percent = textEditPercent.Text,
                    Wind = textEditWind.Text,
                    Comparevalue = textEditComparevalue.Text
                };
                var res = GasContentAnalyzeRequest.AddGasContentAnalyzeConfig(info);
                if (res)
                {
                    XtraMessageBox.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.Yes;
                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show("操作失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void barButtonItemCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}