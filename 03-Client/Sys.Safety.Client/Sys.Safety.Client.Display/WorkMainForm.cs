using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using System.Windows.Forms;
using System;

namespace Sys.Safety.Client.Display
{
    public partial class WorkMainForm : XtraForm
    {
        public WorkMainForm()
        {
            if (!StaticClass.haveinit)
            {
                StaticClass.haveinit = true;
            }
            else
            {
                return;
            }

            InitializeComponent();
        }

        /// <summary>
        /// 获取实时显示的page容器
        /// </summary>
        /// <returns></returns>
        public PanelControl GetRealDiaplayTabPage()
        {
            return this.mworkPControl;
        }

        /// <summary>
        /// 获取导航窗口的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetNagDockPanel()
        {
            return this.dataNagDkPanel;
        }

        /// <summary>
        /// 获取预警控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetEwarlDockPanel()
        {
            return this.anaEWarDkPanel;
        }

        /// <summary>
        /// 获取模拟量报警控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetWarGridDockPanel()
        {
            return this.anaWCPowDkPanel;
        }

        /// <summary>
        /// 获取开关量报警控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetSWWarDockPanel()
        {
            return this.switchWCPowDkPanel;
        }

        /// <summary>
        /// 获取馈电异常控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetFBExeDockPanel()
        {
            return this.fpowExeDkPanel;
        }

        /// <summary>
        /// 获取实时控制控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetRealControlDockPanel()
        {
            return this.realControlDkPanel;
        }

        /// <summary>
        /// 获取开关量状态变动控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetSWChangeDockPanel()
        {
            return this.switchStateDkPanel;
        }

        /// <summary>
        /// 获取手动控制控件的容器
        /// </summary>
        /// <returns></returns>
        public DevExpress.XtraBars.Docking.DockPanel GetHandControlPanel()
        {
            return this.handControlDockPanel;
        }

        private void WorkMainForm_Load(object sender, System.EventArgs e)
        {
            StaticClass.iniform();
            StaticClass.real_s.Show();
            GetRealDiaplayTabPage().Controls.Add(StaticClass.real_s);

            StaticClass._type_s.Show();
            GetNagDockPanel().Controls.Add(StaticClass._type_s);

            StaticClass.YJForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.YJForm.TopLevel = false;
            StaticClass.YJForm.Show();
            GetEwarlDockPanel().Controls.Add(StaticClass.YJForm);

            StaticClass.MNLBJForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.MNLBJForm.TopLevel = false;
            StaticClass.MNLBJForm.Show();
            GetWarGridDockPanel().Controls.Add(StaticClass.MNLBJForm);

            StaticClass.KGLBJForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.KGLBJForm.TopLevel = false;
            StaticClass.KGLBJForm.Show();
            GetSWWarDockPanel().Controls.Add(StaticClass.KGLBJForm);

            StaticClass.KDYCForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.KDYCForm.TopLevel = false;
            StaticClass.KDYCForm.Show();
            GetFBExeDockPanel().Controls.Add(StaticClass.KDYCForm);

            StaticClass.KZForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.KZForm.TopLevel = false;
            StaticClass.KZForm.Show();
            GetRealControlDockPanel().Controls.Add(StaticClass.KZForm);

            StaticClass.KGLBDForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.KGLBDForm.TopLevel = false;
            StaticClass.KGLBDForm.Show();
            GetSWChangeDockPanel().Controls.Add(StaticClass.KGLBDForm);

            StaticClass.RealHandForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.RealHandForm.TopLevel = false;
            StaticClass.RealHandForm.Show();
            GetHandControlPanel().Controls.Add(StaticClass.RealHandForm);

            //添加运行记录页面
            StaticClass.realRunRecordControl.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.realRunRecordControl.TopLevel = false;
            StaticClass.realRunRecordControl.Show();
            runRecordPanel.Controls.Add(StaticClass.realRunRecordControl);

            //添加自动挂接页面
            StaticClass.automaticArtControl.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.automaticArtControl.TopLevel = false;
            StaticClass.automaticArtControl.Show();
            automaticArtPanel.Controls.Add(StaticClass.automaticArtControl);

            StaticClass.MNLDDForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StaticClass.MNLDDForm.TopLevel = false;
            StaticClass.MNLDDForm.Show();
            anaDD_Container.Controls.Add(StaticClass.MNLDDForm);


            timer1.Enabled = true;
            StaticClass.updatefromtext = new StaticClass.mydel1(settext);
            StaticClass.dell = new StaticClass.mydell1(showform);
        }

        private void switchWCPowDkPanel_Click(object sender, System.EventArgs e)
        {
        }

        private void anaEWarDkPanel_Click(object sender, System.EventArgs e)
        {

        }

        private void anaWCPowDkPanel_Click(object sender, System.EventArgs e)
        {

        }

        public void settext(int n)
        {
            try
            {
                switch (n)
                {
                    case 0:
                        anaEWarDkPanel.Text = "模拟量预警";
                        StaticClass.yccount[0] = 0;
                        break;
                    case 1:
                        anaWCPowDkPanel.Text = "模拟量报警";
                        StaticClass.yccount[1] = 0;
                        break;
                    case 2:
                        switchWCPowDkPanel.Text = "开关量报警";
                        StaticClass.yccount[2] = 0;
                        break;
                    case 3:
                        fpowExeDkPanel.Text = "馈电异常";
                        StaticClass.yccount[3] = 0;
                        break;
                    case 4:
                        realControlDkPanel.Text = "实时控制";
                        StaticClass.yccount[4] = 0;
                        break;
                    case 5:
                        switchStateDkPanel.Text = "开关量状态变动";
                        StaticClass.yccount[5] = 0;
                        break;
                    case 6:
                        handControlDockPanel.Text = "中心站控制";
                        StaticClass.yccount[6] = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }


        private void showyccount()
        {
            try
            {
                if (StaticClass.yccount[0] > 0)//新增预警数
                {
                    anaEWarDkPanel.Text = "模拟量预警" + "{" + StaticClass.yccount[0] + "}";
                }
                else
                {
                    anaEWarDkPanel.Text = "模拟量预警";
                }
                if (StaticClass.yccount[1] > 0)//新增模拟量报警数
                {
                    anaWCPowDkPanel.Text = "模拟量报警" + "{" + StaticClass.yccount[1] + "}";
                }
                else
                {
                    anaWCPowDkPanel.Text = "模拟量报警";
                }
                if (StaticClass.yccount[2] > 0)//新增开关量报警数
                {
                    switchWCPowDkPanel.Text = "开关量报警" + "{" + StaticClass.yccount[2] + "}";
                }
                else
                {
                    switchWCPowDkPanel.Text = "开关量报警";
                }
                if (StaticClass.yccount[3] > 0)//新增馈电异常数
                {
                    fpowExeDkPanel.Text = "馈电异常" + "{" + StaticClass.yccount[3] + "}";
                }
                else
                {
                    fpowExeDkPanel.Text = "馈电异常";
                }
                if (StaticClass.yccount[4] > 0)//新增控制数
                {
                    realControlDkPanel.Text = "实时控制" + "{" + StaticClass.yccount[4] + "}";
                }
                else
                {
                    realControlDkPanel.Text = "实时控制";
                }
                if (StaticClass.yccount[5] > 0)//新增开关量变动数
                {
                    switchStateDkPanel.Text = "开关量状态变动" + "{" + StaticClass.yccount[5] + "}";
                }
                else
                {
                    switchStateDkPanel.Text = "开关量状态变动";
                }
                if (StaticClass.yccount[6] > 0)//手动控制数
                {
                    handControlDockPanel.Text = "中心站控制" + "{" + StaticClass.yccount[6] + "}";
                }
                else
                {
                    handControlDockPanel.Text = "中心站控制";
                }
                if (StaticClass.yccount[7] > 0)//自动挂接
                {
                    automaticArtPanel.Text = "自动挂接" + "{" + StaticClass.yccount[7] + "}";
                }
                else
                {
                    automaticArtPanel.Text = "自动挂接";
                }
                if (StaticClass.yccount[8] > 0)//自动挂接
                {
                    anaDDPanel.Text = "模拟量断电" + "{" + StaticClass.yccount[8] + "}";
                }
                else
                {
                    anaDDPanel.Text = "模拟量断电";
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void showform(string n)
        {
            switch (n)
            {
                case "1":
                    anaEWarDkPanel.Show();//新增预警数
                    break;
                case "2":
                    anaWCPowDkPanel.Show();//新增模拟量报警数
                    break;
                case "3":
                    switchWCPowDkPanel.Show();//新增开关量报警数
                    break;
                case "4":
                    fpowExeDkPanel.Show();//新增馈电异常数
                    break;
                case "5":
                    realControlDkPanel.Show();//新增控制数
                    break;
                case "6":
                    switchStateDkPanel.Show();//新增开关量变动数
                    break;
                case "7":
                    handControlDockPanel.Show();//新增手动中心站控制数
                    break;
                case "8":
                    automaticArtPanel.Show();//新增自动挂接
                    break;
            }
        }

        private void fpowExeDkPanel_Click(object sender, System.EventArgs e)
        {
        }

        private void realControlDkPanel_Click(object sender, System.EventArgs e)
        {
        }

        private void switchStateDkPanel_Click(object sender, System.EventArgs e)
        {
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            timer1.Enabled = false;
            showyccount();
            timer1.Enabled = true;
        }

        private void WorkMainForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            StaticClass.SystemOut = true;
            StaticClass.CloseAlarmForm();
        }

        private void panelContainer1_Click(object sender, System.EventArgs e)
        {
        }

        private void WorkMainForm_Resize(object sender, System.EventArgs e)
        {
            //窗体最小化时  
            if (this.WindowState == FormWindowState.Minimized)
            {

            }

            //窗体恢复正常时,重新改变一下窗体的大小 解决在win2008下面的bug  20170403  
            if (this.WindowState == FormWindowState.Normal)
            {
                int width = dataNagDkPanel.Size.Width;
                int height = dataNagDkPanel.Size.Height;                
                //MessageBox.Show(width.ToString());

                dataNagDkPanel.Size = new System.Drawing.Size(width - 1, height);
                dataNagDkPanel.Refresh();
                dataNag_Container.Size = new System.Drawing.Size(width - 1, height);
                dataNag_Container.Refresh();
                if (StaticClass._type_s != null)
                {
                    StaticClass._type_s.Size = new System.Drawing.Size(StaticClass._type_s.Width - 1, StaticClass._type_s.Height);
                    StaticClass._type_s.Refresh();
                    StaticClass._type_s.treeList_fz.Size = new System.Drawing.Size(StaticClass._type_s.treeList_fz.Width - 1, StaticClass._type_s.treeList_fz.Height);
                    StaticClass._type_s.treeList_fz.Refresh();
                }
                dataNagDkPanel.Size = new System.Drawing.Size(width, height);
                dataNagDkPanel.Refresh();
                dataNag_Container.Size = new System.Drawing.Size(width, height);
                dataNag_Container.Refresh();
                if (StaticClass._type_s != null)
                {
                    StaticClass._type_s.Size = new System.Drawing.Size(StaticClass._type_s.Width + 1, StaticClass._type_s.Height);
                    StaticClass._type_s.Refresh();
                    StaticClass._type_s.treeList_fz.Size = new System.Drawing.Size(StaticClass._type_s.treeList_fz.Width + 1, StaticClass._type_s.treeList_fz.Height);
                    StaticClass._type_s.treeList_fz.Refresh();
                }
                
                //dataNagDkPanel.Size = new System.Drawing.Size(width + 1, height);
                //dataNagDkPanel.Refresh();
                //dataNagDkPanel.Size = new System.Drawing.Size(width, height);
                //dataNagDkPanel.Refresh();
            }
        }
    }
}
