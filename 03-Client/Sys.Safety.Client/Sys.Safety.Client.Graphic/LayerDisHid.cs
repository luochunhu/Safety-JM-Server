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
    public partial class LayerDisHid : DevExpress.XtraEditors.XtraForm
    {
        public LayerDisHid()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selerowindex = this.gridView1.GetSelectedRows();
                for (int i = 0; i < selerowindex.Length; i++)
                {
                    string LayerName = gridView1.GetRowCellValue(selerowindex[i], "LayerName").ToString();
                    Program.main.LayerDisplay(LayerName);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("LayerDisHid_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selerowindex = this.gridView1.GetSelectedRows();
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    bool isSel=false;
                    for (int j = 0; j < selerowindex.Length; j++)
                    {
                        if (selerowindex[j] == i) {
                            isSel = true;
                        }
                    }
                    string LayerName = gridView1.GetRowCellValue(i, "LayerName").ToString();
                    if (isSel)
                    {                        
                        Program.main.LayerDisplay(LayerName);                       
                    }
                    else {
                        Program.main.LayerHidden(LayerName);
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("LayerDisHid_simpleButton3_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void LayerDisHid_Load(object sender, EventArgs e)
        {
            try
            {
                List<string> Layers = Program.main.LoadLayers();
                DataTable dt = new DataTable();
                dt.Columns.Add("LayerName", typeof(string));
                dt.Columns.Add("IsDisplay", typeof(string));
                foreach (string Layer in Layers)
                {
                    string[] layerArr = Layer.Split(',');
                    if (layerArr.Length == 2)
                    {
                        object[] obj = new object[dt.Columns.Count];
                        obj[0] = layerArr[0];
                        obj[1] = layerArr[1].ToLower();
                        dt.Rows.Add(obj);
                    }
                }
                gridControl1.DataSource = dt;
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (gridView1.GetRowCellValue(i, "IsDisplay").ToString().ToLower() == "true")//表示为隐藏状态
                    {

                    }
                    else
                    {//显示状态
                        gridView1.SelectRow(i);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("LayerDisHid_LayerDisHid_Load" + ex.Message + ex.StackTrace);
            }
        }
    }
}
