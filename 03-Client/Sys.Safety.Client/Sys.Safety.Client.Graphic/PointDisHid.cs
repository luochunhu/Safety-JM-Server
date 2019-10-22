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

namespace Sys.Safety.Client.Graphic
{
    public partial class PointDisHid : DevExpress.XtraEditors.XtraForm
    {
        public PointDisHid()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //int[] selerowindex = this.gridView1.GetSelectedRows();
                //string Point = "";
                //for (int i = 0; i < selerowindex.Length; i++)
                //{
                //    Point += gridView1.GetRowCellValue(selerowindex[i], "Point").ToString() + "|";
                //}
                //if (Point.Contains("|"))
                //{
                //    Point = Point.Substring(0, Point.Length - 1);
                //}
                //Program.main.PointDisplay(Point);

                int[] selerowindex = this.gridView1.GetSelectedRows();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    bool isSel = false;
                    for (int j = 0; j < selerowindex.Length; j++)
                    {
                        if (selerowindex[j] == i)
                        {
                            isSel = true;
                        }
                    }
                    string PointName = gridView1.GetRowCellValue(i, "Point").ToString();
                    if (isSel)
                    {
                        Program.main.PointDisplay(PointName);
                    }
                    else
                    {
                        Program.main.PointHidden(PointName);
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisHid_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selerowindex = this.gridView1.GetSelectedRows();
                string Point = "";
                for (int i = 0; i < selerowindex.Length; i++)
                {
                    Point += gridView1.GetRowCellValue(selerowindex[i], "Point").ToString() + "|";
                }
                if (Point.Contains("|"))
                {
                    Point = Point.Substring(0, Point.Length - 1);
                }
                Program.main.PointHidden(Point);

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisHid_simpleButton3_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void LayerDisHid_Load(object sender, EventArgs e)
        {
            RefGrid("0");
        }

        public void RefGrid(string type)
        {
            try
            {
                DataTable dtpoint = Program.main.GraphOpt.getAllGraphPoint(type);
                DataTable dt = new DataTable();
                dt.Columns.Add("Point", typeof(string));
                dt.Columns.Add("Wz", typeof(string));
                dt.Columns.Add("DevName", typeof(string));
                foreach (DataRow drpoint in dtpoint.Rows)
                {
                    object[] obj = new object[dt.Columns.Count];
                    if (drpoint["point"].ToString().Contains("."))//交换机
                    {
                        obj[0] = drpoint["point"].ToString();
                        obj[1] = drpoint["wz1"].ToString();
                        obj[2] = "交换机";
                    }
                    else
                    {
                        obj[0] = drpoint["point"].ToString();
                        obj[1] = drpoint["wz"].ToString();
                        obj[2] = drpoint["name"].ToString();
                    }
                    dt.Rows.Add(obj);
                }
                gridControl1.DataSource = dt;
                List<string> PointDis = Program.main.GetAllPointDis();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    string TempPoint = gridView1.GetRowCellValue(i, "Point").ToString();
                    string TempDis = PointDis.Find(a => a.Contains(TempPoint));
                    if (!string.IsNullOrEmpty(TempDis))
                    {
                        string DisState = TempDis.Split(',')[1];
                        if (DisState.ToLower() == "true")//表示显示状态
                        {
                            gridView1.SelectRow(i);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisHid_RefGrid" + ex.Message + ex.StackTrace);
            }
        }

        private void cBoxPointType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string type = "0";
                if (cBoxPointType.SelectedItem.ToString() == "所有测点")
                {
                    type = "0";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "交换机")
                {
                    type = "3";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "分站")
                {
                    type = "1";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "所有传感器")
                {
                    type = "2";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "开关量")
                {
                    type = "4";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "模拟量")
                {
                    type = "5";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "控制量")
                {
                    type = "6";
                }
                else if (cBoxPointType.SelectedItem.ToString() == "开关量、控制量")
                {
                    type = "7";
                }
                RefGrid(type);
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointDisHid_cBoxPointType_SelectedIndexChanged" + ex.Message + ex.StackTrace);
            }
        }
    }
}
