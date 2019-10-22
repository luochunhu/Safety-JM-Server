using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Graphic
{
    public partial class AnalysisModelPointEdit : XtraForm
    {
        private string EditPoint = "";
        private string EditPointUnitName = "";
        private string EditPointBindType = "";
        private string EditPointZoomLevel = "";
        private string EditPointanimationState = "";
        private string EidtPointTurnToPage = "";
        private string EditPointId = "";

        public AnalysisModelPointEdit(string PointName, string UnitName, string BindType, string ZoomLevel, string animationState, string TurnToPage, string PointId)
        {
            InitializeComponent();
            EditPoint = PointName;
            EditPointUnitName = UnitName;
            EditPointBindType = BindType;
            EditPointZoomLevel = ZoomLevel;
            EditPointanimationState = animationState;
            EidtPointTurnToPage = TurnToPage;
            EditPointId = PointId;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择一个设备进行绑定！");
                    return;
                }

                string Point = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Name").ToString();
                string Wz = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Name").ToString();
                string DevName = "分析模型";

                string TurnToPage = "";
                if (!string.IsNullOrEmpty(comboBoxEdit1.Text))
                {
                    TurnToPage = comboBoxEdit1.Text;
                }

                int minZoomLevel = 0, maxZoomLevel = 0;
                if (int.TryParse(this.spinEdit6.Value.ToString(), out minZoomLevel))
                {
                    if (minZoomLevel < 1)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("最小缩放级别必须定义在1级以上！");
                        return;
                    }
                }

                if (int.TryParse(this.spinEdit7.Value.ToString(), out maxZoomLevel))
                {

                    if (maxZoomLevel > 22)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("最大缩放级别必须小于22级！");
                        return;
                    }
                    if (maxZoomLevel <= minZoomLevel)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("最大缩放级别必须大于最小缩放级别！");
                        return;
                    }
                }
                string zoomLevel = minZoomLevel + "$" + maxZoomLevel;
                Program.main.EditPoint(Point, Wz, DevName, zoomLevel, "", "", "", TurnToPage, EditPointId,"0");
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("VideoPointDefEdit_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AnalysisModelPointEdit_Load(object sender, EventArgs e)
        {
            try
            {

                string type = "0";//加载类型（0：所有测点，1：分站，2：所有传感器，3：交换机，4：开关量，5：模拟量,6：控制量,7：开关量和控制量）      
                if (!string.IsNullOrWhiteSpace(EditPointZoomLevel))
                {
                    string[] zoomLevelArr = EditPointZoomLevel.Split('$');
                    this.spinEdit6.Value = int.Parse(zoomLevelArr[0]);
                    this.spinEdit7.Value = int.Parse(zoomLevelArr[1]);
                }


                if (Program.main.GraphOpt.getGraphicNowType() == 3)//SVG组态图形，不判断缩放等级
                {
                    this.spinEdit6.Value = 0;
                    this.spinEdit7.Value = 0;
                }

                if (EditPointBindType == "2")//SVG图元
                {
                    this.spinEdit6.Enabled = true;
                    this.spinEdit7.Enabled = true;
                }

                //加载分析模型
                List<JC_LargedataAnalysisConfigInfo> vdefinfos = Program.main.GraphOpt.LoadAllAnalysisConfigInfo();
                gridControl1.DataSource = vdefinfos;

                //加载双击页面跳转
                IList<GraphicsbaseinfInfo> GraphicsbaseinfDTOs = Program.main.GraphOpt.getAllGraphicDto();
                foreach (GraphicsbaseinfInfo GraphicsbaseinfDTO_ in GraphicsbaseinfDTOs)
                {
                    comboBoxEdit1.Properties.Items.Add(GraphicsbaseinfDTO_.GraphName);
                }
                if (!string.IsNullOrEmpty(EidtPointTurnToPage))
                {
                    comboBoxEdit1.Text = EidtPointTurnToPage;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDefEdit_PointEdit_Load" + ex.Message + ex.StackTrace);
            }
        }
    }
}
