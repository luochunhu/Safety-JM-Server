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

namespace Sys.Safety.Client.Graphic
{
    public partial class PointEdit : DevExpress.XtraEditors.XtraForm
    {
        private string EditPoint = "";
        private string EditPointWidth = "";
        private string EditPointHeight = "";
        private string EditPointTurnToPage = "";
        private string EditTransformDeg = "";

        public PointEdit(string PointName, string TurnToPage, string transformDeg)
        {
            InitializeComponent();
            EditPoint = PointName;
            EditPointTurnToPage = TurnToPage;
            EditTransformDeg = transformDeg;
        }
        public PointEdit(string PointName, string Width, string Height, string TurnToPage, string transformDeg)
        {
            InitializeComponent();
            EditPoint = PointName;
            EditPointWidth = Width;
            EditPointHeight = Height;
            EditPointTurnToPage = TurnToPage;
            EditTransformDeg = transformDeg;
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
                string Point = textPointName.Text;
                string PointWz = "";
                string PointDevName = "";
                string ZoomLevel = "1$22";
                string Width = spinEdit3.Value.ToString();
                string Height = spinEdit4.Value.ToString();
                string TurnToPage = "";
                if (!string.IsNullOrEmpty(comboBoxEdit1.Text))
                {
                    TurnToPage = comboBoxEdit1.Text;
                }

                Program.main.EditPoint(Point, PointWz, PointDevName, ZoomLevel, "0", Width, Height, TurnToPage, "", spinEdit1.Value.ToString());

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("PointEdit_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PointEdit_Load(object sender, EventArgs e)
        {
            this.textPointName.Text = EditPoint;
            this.spinEdit3.Text = EditPointWidth;
            this.spinEdit4.Text = EditPointHeight;
            //加载旋转角度  20171226
            this.spinEdit1.Text = EditTransformDeg;
            //加载双击页面跳转 
            IList<GraphicsbaseinfInfo> GraphicsbaseinfDTOs = Program.main.GraphOpt.getAllGraphicDto();
            foreach (GraphicsbaseinfInfo GraphicsbaseinfDTO_ in GraphicsbaseinfDTOs)
            {
                comboBoxEdit1.Properties.Items.Add(GraphicsbaseinfDTO_.GraphName);
            }
            if (!string.IsNullOrEmpty(EditPointTurnToPage))
            {
                comboBoxEdit1.Text = EditPointTurnToPage;
            }
        }
    }
}
