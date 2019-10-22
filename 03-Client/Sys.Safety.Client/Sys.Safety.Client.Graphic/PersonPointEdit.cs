using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Graphic
{
    public partial class PersonPointDefEdit : DevExpress.XtraEditors.XtraForm
    {
        private string EditPoint = "";
        private string EditPointUnitName = "";
        private string EditPointBindType = "";
        private string EditPointZoomLevel = "";
        private string EditPointanimationState = "";
        private string EditPointWidth = "";
        private string EditPointHeight = "";
        private string EidtPointTurnToPage = "";
        private string EditPointId = "";
        DataTable dt = new DataTable();

        private IPersonPointDefineService rdefService = ServiceFactory.Create<IPersonPointDefineService>();


        public PersonPointDefEdit(string PointName, string UnitName, string BindType, string ZoomLevel, string animationState, string TurnToPage, string PointId)
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

        /// <summary>
        /// 测点绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择一个设备进行绑定！");
                    return;
                }

                string Point = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString();
                string Wz = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Wz").ToString();
                string DevName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DevName").ToString();
                string animationState = spinEdit2.Value.ToString();
                string Width = spinEdit3.Value.ToString();
                string Height = spinEdit4.Value.ToString();
                string TurnToPage = "";
                if (!string.IsNullOrEmpty(comboBoxEdit1.Text))
                {
                    TurnToPage = comboBoxEdit1.Text;
                }

                //if (EditPointBindType == "2")//SVG图元
                //{
                //    if (Program.main.GraphOpt.getGraphicNowType() != 3)//SVG组态图形，不判断缩放等级
                //    {
                //        if (int.TryParse(spinEdit1.Value.ToString(), out zoomlevel))
                //        {
                //            if (zoomlevel <= 5)
                //            {
                //                DevExpress.XtraEditors.XtraMessageBox.Show("SVG图元必须定义缩放级别在5级以上！");
                //                return;
                //            }
                //        }
                //    }

                //}

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
                Program.main.EditPoint(Point, Wz, DevName, zoomLevel, animationState, Width, Height, TurnToPage, EditPointId, "0");
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDefEdit_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PointEdit_Load(object sender, EventArgs e)
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

                if (EditPointBindType == "2" || EditPointBindType == "1")//SVG图元、拓扑图元
                {
                    spinEdit3.Enabled = true;
                    spinEdit4.Enabled = true;
                }
                else
                {
                    spinEdit3.Enabled = false;
                    spinEdit4.Enabled = false;
                }

                if (EditPointBindType == "2")//SVG图元
                {
                    this.spinEdit6.Enabled = true;
                    this.spinEdit7.Enabled = true;
                }
                //else
                //{
                //    this.spinEdit6.Enabled = false;
                //    this.spinEdit7.Enabled = false;
                //}
                if (EditPointBindType == "2" || EditPointBindType == "3")//SVG图元,gif图元
                {
                    spinEdit2.Enabled = true;
                }
                else
                {
                    spinEdit2.Enabled = false;
                }


                //加载测点信息，包括交换机测点
                //dt = Program.main.GraphOpt.LoadAllpointDef(type);

                var definfos = rdefService.GetAllPointDefineCache().Data;
                gridControl1.DataSource = definfos;
                //设置选择状态
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (EditPoint.Contains(gridView1.GetDataRow(i).ItemArray[0].ToString()))
                    {
                        gridView1.FocusedRowHandle = i;
                    }
                }

                //设置绑定动画的状态
                spinEdit2.Value = decimal.Parse(EditPointanimationState);

                if (!string.IsNullOrEmpty(EditPointWidth) && !string.IsNullOrEmpty(EditPointHeight))
                {
                    spinEdit3.Value = decimal.Parse(EditPointWidth);
                    spinEdit4.Value = decimal.Parse(EditPointHeight);
                }
                else
                {
                    spinEdit3.Value = 0;
                    spinEdit4.Value = 0;
                }
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
  
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                labelControl3.Text = "";
                if (dt.Rows.Count > 0 && gridView1.FocusedRowHandle < dt.Rows.Count)
                {
                    DataRow[] dr = dt.Select("point='" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString() + "'");
                    if (dr.Length > 0)
                    {
                        if (dr[0]["type"].ToString() == "2" || dr[0]["type"].ToString() == "3")
                        {
                            if (dr[0]["type"].ToString() == "2")
                            {
                                spinEdit2.Value = 2;
                            }
                            if (dr[0]["type"].ToString() == "3")
                            {
                                spinEdit2.Value = 0;
                            }

                            switch (spinEdit2.Value.ToString())
                            {
                                case "0":
                                    labelControl3.Text = "(值:" + dr[0]["Xs1"].ToString() + ")";
                                    break;
                                case "1":
                                    labelControl3.Text = "(值:" + dr[0]["Xs2"].ToString() + ")";
                                    break;
                                case "2":
                                    labelControl3.Text = "(值:" + dr[0]["Xs3"].ToString() + ")";
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDefEdit_gridView1_FocusedRowChanged" + ex.Message + ex.StackTrace);
            }
        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                labelControl3.Text = "";
                if (dt.Rows.Count > 0 && gridView1.FocusedRowHandle < dt.Rows.Count)
                {
                    DataRow[] dr = dt.Select("point='" + gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString() + "'");
                    if (dr.Length > 0)
                    {
                        if (dr[0]["type"].ToString() == "2")
                        {
                            switch (spinEdit2.Value.ToString())
                            {
                                case "0":
                                    labelControl3.Text = "(值:" + dr[0]["Xs1"].ToString() + ")";
                                    break;
                                case "1":
                                    labelControl3.Text = "(值:" + dr[0]["Xs2"].ToString() + ")";
                                    break;
                                case "2":
                                    labelControl3.Text = "(值:" + dr[0]["Xs3"].ToString() + ")";
                                    break;
                            }
                        }
                        else if (dr[0]["type"].ToString() == "3")
                        {
                            switch (spinEdit2.Value.ToString())
                            {
                                case "0":
                                    labelControl3.Text = "(值:" + dr[0]["Xs1"].ToString() + ")";
                                    break;
                                case "1":
                                    labelControl3.Text = "(值:" + dr[0]["Xs2"].ToString() + ")";
                                    break;
                                case "2":
                                    labelControl3.Text = "(值:断线)";
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDefEdit_spinEdit2_EditValueChanged" + ex.Message + ex.StackTrace);
            }
        }
    }
}
