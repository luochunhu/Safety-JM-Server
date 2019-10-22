using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Graphic
{
    public partial class GraphicsOpen : DevExpress.XtraEditors.XtraForm
    {
        public GraphicsOpen()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string GraphFileName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "GraphName").ToString();
                if (GraphFileName.Length > 0)
                {
                    string distFileName = Application.StartupPath + "\\mx\\dwg\\" + GraphFileName;
                    //加载图形
                    Program.main.LoadMap(GraphFileName);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsOpen_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GraphicsOpen_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("GraphName", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("bz3", typeof(string));
                dt.Columns.Add("bz4", typeof(string));
                dt.Columns.Add("Timer", typeof(string));
                IList<GraphicsbaseinfInfo> GraphicsbaseinfDTOs = Program.main.GraphOpt.getAllGraphicDto();
                foreach (GraphicsbaseinfInfo GraphicsbaseinfDTO_ in GraphicsbaseinfDTOs)
                {
                    object[] obj = new object[dt.Columns.Count];
                    obj[0] = GraphicsbaseinfDTO_.GraphName;
                    if (GraphicsbaseinfDTO_.Type == 0)
                    {
                        obj[1] = "动态图";
                        if (!string.IsNullOrWhiteSpace(GraphicsbaseinfDTO_.Bz3))
                        {
                            if (GraphicsbaseinfDTO_.Bz3 == "0")
                            {
                                obj[3] = "用户自定义图形";
                            }
                            else if (GraphicsbaseinfDTO_.Bz3 == "1")
                            {
                                obj[3] = "通风系统默认图形";
                            }
                        }
                    }
                    else if (GraphicsbaseinfDTO_.Type == 1)
                    {
                        obj[1] = "拓扑图";
                        if (!string.IsNullOrWhiteSpace(GraphicsbaseinfDTO_.Bz3))
                        {
                            if (GraphicsbaseinfDTO_.Bz3 == "0")
                            {
                                obj[3] = "用户自定义图形";
                            }
                            else if (GraphicsbaseinfDTO_.Bz3 == "2")
                            {
                                obj[3] = "拓扑定义默认图形";
                            }
                        }
                    }
                    else if (GraphicsbaseinfDTO_.Type == 2)
                    {
                        obj[1] = "GIS地图";
                    }
                    else if (GraphicsbaseinfDTO_.Type == 3)
                    {
                        obj[1] = "SVG组态图";
                    }                 
                    if (!string.IsNullOrWhiteSpace(GraphicsbaseinfDTO_.Bz4))
                    {
                        if (GraphicsbaseinfDTO_.Bz4 == "0")
                        {
                            obj[2] = "否";
                        }
                        else if (GraphicsbaseinfDTO_.Bz4 == "1")
                        {
                            obj[2] = "是";
                        }
                    }
                    obj[4] = GraphicsbaseinfDTO_.Timer;
                    dt.Rows.Add(obj);
                }
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsOpen_GraphicsOpen_Load" + ex.Message + ex.StackTrace);
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                int rowHandle = e.RowHandle + 1;
                if (e.Info.IsRowIndicator && rowHandle > 0)
                {
                    e.Info.DisplayText = rowHandle.ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsOpen_gridView1_CustomDrawRowIndicator" + ex.Message + ex.StackTrace);
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.RowHandle > 0)
                {
                    string GraphFileName = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "GraphName").ToString();
                    if (GraphFileName.Length > 0)
                    {
                        string distFileName = Application.StartupPath + "\\mx\\dwg\\" + GraphFileName;
                        //加载图形
                        Program.main.LoadMap(GraphFileName);
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {

                LogHelper.Error("GraphicsOpen_gridView1_DoubleClick" + ex.Message + ex.StackTrace); 
            }
        }
    }
}
