using DevExpress.XtraEditors;
using Sys.Safety.Client.DataAnalysis.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetAnalysisModelPoint : XtraForm
    {

        public SetAnalysisModelPoint()
        {
            InitializeComponent();

            DataTable dt = new DataTable();

            dt.Columns.Add("data");


            DataRow dr = dt.NewRow();
            dr[0] = true;
            dt.Rows.Add(dr);
             DataRow dr1 = dt.NewRow();
            dr1[0] = false;
            dt.Rows.Add(dr1);
            gridControl1.DataSource = dt;
        }

        private void LoadForm()
        {
            //_LinkLabel.Width = TextRenderer.MeasureText(_LinkLabel.Text, _LinkLabel.Font).Width;
            //panelControlEdit.Controls.Add(new TextBox());
        }
        string selectBtnText = "";
        List<SimpleButton> btnList = new List<SimpleButton>();

        /// <summary>
        /// 表达式拆分控件
        /// </summary>
        /// <param name="listData">操作步骤</param>
        private void LoadPanlBtnData(List<string> listData)
        {
            btnList = new List<SimpleButton>();
            if (listData == null || listData.Count == 0)
            {
                return;
            }
            int left = 0;
            foreach (var item in listData)
            {
                SimpleButton createBtn = CreateBtn(item, left);

                btnList.Add(createBtn);

                left += createBtn.Width;
            }

            //panelControlEdit.Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (panelControlEdit.Controls.Count > 0)
            {
                if (panelControlEdit.Controls[0] != null)
                    panelControlEdit.Controls[0].Dispose();
            }
            foreach (var item in btnList)
            {
                panelControlEdit.Controls.Add(item);
            }

            panelControlEdit.Refresh();

        }
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="text">显示文本值</param>
        /// <param name="left">左边的位置</param>
        /// <returns></returns>
        private SimpleButton CreateBtn(string text, int left)
        {
            SimpleButton simpleButton = new SimpleButton();
            simpleButton.Text = text;
            simpleButton.Left = left;
            simpleButton.Appearance.BackColor = System.Drawing.Color.Gainsboro;

            simpleButton.Appearance.Options.UseBackColor = true;
            simpleButton.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            simpleButton.Cursor = System.Windows.Forms.Cursors.Hand;

            simpleButton.Width = TextRenderer.MeasureText(simpleButton.Text, simpleButton.Font).Width;
            simpleButton.MouseDown += simpleButton_MouseDown;
            return simpleButton;
        }
        /// <summary>
        /// 鼠标左键修改，右键删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
            else if (e.Button == MouseButtons.Left)
            {

            }
        }



        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {


        }

        private void btnAdd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MessageBox.Show("右键");
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (btnList.Count == 0)
                {
                    btnList.Add(CreateBtn(Guid.NewGuid().ToString(), 0));
                }
                else
                {
                    int left = 0;
                    foreach (var item in btnList)
                    {
                        left += item.Width;
                    }
                    btnList.Add(CreateBtn(Guid.NewGuid().ToString().ToString(), left));
                }

                //panelControlEdit.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (panelControlEdit.Controls.Count > 0)
                {
                    if (panelControlEdit.Controls[0] != null)
                        panelControlEdit.Controls[0].Dispose();
                }
                foreach (var item in btnList)
                {
                    panelControlEdit.Controls.Add(item);
                }

                panelControlEdit.Refresh();
            }
        }

        

        private void MenuItemAddLeft_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemAddRight_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectBtnText))
            {
                return;
            }
            else
            {

                SimpleButton simpleButtonitem = new SimpleButton();

                int record = -1;
                foreach (var item in panelControlEdit.Controls)
                {
                    record++;
                    SimpleButton simpleButton = item as SimpleButton;
                    if (simpleButton.Text.Trim() == selectBtnText)
                    {
                        simpleButtonitem = simpleButton;
                        panelControlEdit.Controls.Remove(simpleButton);
                        btnList.Remove(simpleButton);
                        break;
                    }
                }
                for (int i = record; i < btnList.Count; i++)
                {
                    btnList[i].Left -= simpleButtonitem.Width;
                }

                selectBtnText = "";
            }
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectBtnText))
            {
                return;
            }
            else
            {

                SimpleButton simpleButtonitem = new SimpleButton();

                int record = -1;
                foreach (var item in panelControlEdit.Controls)
                {
                    record++;
                    SimpleButton simpleButton = item as SimpleButton;
                    if (simpleButton.Text.Trim() == selectBtnText)
                    {
                        simpleButtonitem = simpleButton;
                        panelControlEdit.Controls.Remove(simpleButton);
                        btnList.Remove(simpleButton);
                        break;
                    }
                }
                for (int i = record; i < btnList.Count; i++)
                {
                    btnList[i].Left -= simpleButtonitem.Width;
                }



                selectBtnText = "";
            }
        }

        private void MenuItemEdit_Click(object sender, EventArgs e)
        {
            if (btnList.Count == 0)
            {
                btnList.Add(CreateBtn(Guid.NewGuid().ToString(), 0));
            }
            else
            {
                int left = 0;
                foreach (var item in btnList)
                {
                    left += item.Width;
                }
                btnList.Add(CreateBtn(Guid.NewGuid().ToString().ToString(), left));
            }

            //panelControlEdit.Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (panelControlEdit.Controls.Count > 0)
            {
                if (panelControlEdit.Controls[0] != null)
                    panelControlEdit.Controls[0].Dispose();
            }
            foreach (var item in btnList)
            {
                panelControlEdit.Controls.Add(item);
            }

            panelControlEdit.Refresh();
        }

        private void lsbExpression_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                
                selectBtnText = lsbExpression.SelectedValue.ToString();
                foreach (var item in panelControlEdit.Controls)
                {
                    SimpleButton simpleButtonitem = item as SimpleButton;
                    if (simpleButtonitem.Text.Trim() != selectBtnText)
                    {
                        simpleButtonitem.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    }
                    else
                    {
                        simpleButtonitem.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    }

                }

                panelControlEdit.Refresh();
            }
        }
    }
}
