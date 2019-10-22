using DevExpress.XtraEditors;
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
    public partial class BigDataAnalysisForm : DevExpress.XtraEditors.XtraForm
    {
        public BigDataAnalysisForm()
        {
            InitializeComponent();
            StaticClass.SystemOut = false;
        }

        private void xtraTabControlChildFrom_Selected(object sender, DevExpress.XtraTab.TabPageEventArgs e)
        {

            try
            {
                switch (e.PageIndex)
                {
                    case 0:
                        //if (tabAnalysisTemplateManage.Controls.Count > 0)
                        //{
                        //    AnalysisTemplateManage formAnalysisTemplateManage = tabAnalysisTemplateManage.Controls[0] as AnalysisTemplateManage;
                        //    formAnalysisTemplateManage.LoadForm();
                        //}

                        break;
                    case 1:

                        //if (TabLargedataAnalysisManage.Controls.Count > 0)
                        //{
                        //    LargedataAnalysisManage formLargedataAnalysisManage = TabLargedataAnalysisManage.Controls[0] as LargedataAnalysisManage;
                        //    formLargedataAnalysisManage.LoadForm();
                        //}

                        break;
                    case 2:

                        //if (TabSetAlarmNotificationPersonnelMange.Controls.Count > 0)
                        //{
                        //    SetAlarmNotificationPersonnelManage formSetAlarmNotificationPersonnelManage = TabSetAlarmNotificationPersonnelMange.Controls[0] as SetAlarmNotificationPersonnelManage;
                        //    //formSetAlarmNotificationPersonnel.LoadForm();
                        //}
                        break;
                    case 3:
                        //if (TabSetRegionOutageManage.Controls.Count > 0)
                        //{
                        //    SetRegionOutage formSetRegionOutage = TabSetRegionOutageManage.Controls[0] as SetRegionOutage;
                        //    formSetRegionOutage.LoadForm();
                        //}
                        break;
                    case 4:
                        //if (TabSetEmergencyLinkage.Controls.Count > 0)
                        //{
                        //    SetEmergencyLinkage formSetEmergencyLinkage = TabSetEmergencyLinkage.Controls[0] as SetEmergencyLinkage;
                        //    formSetEmergencyLinkage.LoadForm();
                        //}
                        break;
                    case 5:
                        //if (TabCurvilinearQuery.Controls.Count > 0)
                        //{
                        //    CurvilinearQuery formCurvilinearQuery = TabCurvilinearQuery.Controls[0] as CurvilinearQuery;
                        //    formCurvilinearQuery.InitializeControls();
                        //}
                        break;
                    case 6:

                        break;
                    case 7:
                        //if (TabViewEmergencyLinkage.Controls.Count > 0)
                        //{
                        //    SetEmergencyLinkageList formSetEmergencyLinkageList = TabViewEmergencyLinkage.Controls[0] as SetEmergencyLinkageList;
                        //    formSetEmergencyLinkageList.LoadForm();
                        //}
                        break;

                    default:
                        break;
                }
            }
            catch
            {

            }

        }

        private void BigDataAnalysisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseForm();
            StaticClass.SystemOut = true;
        }

        private void BigDataAnalysisForm_Load(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开大数据分析窗体...", "请等待...");
            LoadForm();
            wdf.Close();
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void CloseForm()
        {
            if (tabAnalysisTemplateManage.Controls.Count > 0)
            {
                XtraForm itemForm = tabAnalysisTemplateManage.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabLargedataAnalysisManage.Controls.Count > 0)
            {
                XtraForm itemForm = TabLargedataAnalysisManage.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabSetAlarmNotificationPersonnelMange.Controls.Count > 0)
            {
                XtraForm itemForm = TabSetAlarmNotificationPersonnelMange.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabSetRegionOutageManage.Controls.Count > 0)
            {
                XtraForm itemForm = TabSetRegionOutageManage.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabSetEmergencyLinkage.Controls.Count > 0)
            {
                XtraForm itemForm = TabSetEmergencyLinkage.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }

            if (TabCurvilinearQuery.Controls.Count > 0)
            {
                XtraForm itemForm = TabCurvilinearQuery.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabAnalysisResultsInRealtime.Controls.Count > 0)
            {
                XtraForm itemForm = TabAnalysisResultsInRealtime.Controls[0] as XtraForm;
                itemForm.Close();
                itemForm.Dispose();
            }
            if (TabViewEmergencyLinkage.Controls.Count > 0)
            {
                XtraForm itemForm = TabViewEmergencyLinkage.Controls[0] as XtraForm;
                if(itemForm != null)
                {
                    itemForm.Close();
                    itemForm.Dispose();
                }
            }

        }
        /// <summary>
        /// 加载窗体
        /// </summary>
        private void LoadForm()
        {
            try
            {
                AnalysisTemplateManage formAnalysisTemplateManage = new AnalysisTemplateManage();

                formAnalysisTemplateManage.FormBorderStyle = FormBorderStyle.None;
                formAnalysisTemplateManage.TopLevel = false;
                formAnalysisTemplateManage.Visible = true;
                formAnalysisTemplateManage.Dock = DockStyle.Fill;
                tabAnalysisTemplateManage.Controls.Add(formAnalysisTemplateManage);


                LargedataAnalysisManage formLargedataAnalysisManage = new LargedataAnalysisManage();

                formLargedataAnalysisManage.FormBorderStyle = FormBorderStyle.None;
                formLargedataAnalysisManage.TopLevel = false;
                formLargedataAnalysisManage.Visible = true;
                formLargedataAnalysisManage.Dock = DockStyle.Fill;
                TabLargedataAnalysisManage.Controls.Add(formLargedataAnalysisManage);


                SetAlarmNotificationPersonnelManage formSetAlarmNotificationPersonnelManage = new SetAlarmNotificationPersonnelManage();
                formSetAlarmNotificationPersonnelManage.FormBorderStyle = FormBorderStyle.None;
                formSetAlarmNotificationPersonnelManage.TopLevel = false;
                formSetAlarmNotificationPersonnelManage.Visible = true;
                formSetAlarmNotificationPersonnelManage.Dock = DockStyle.Fill;
                TabSetAlarmNotificationPersonnelMange.Controls.Add(formSetAlarmNotificationPersonnelManage);

                SetRegionOutageManage formSetRegionOutageManage = new SetRegionOutageManage();
                formSetRegionOutageManage.FormBorderStyle = FormBorderStyle.None;
                formSetRegionOutageManage.TopLevel = false;
                formSetRegionOutageManage.Visible = true;
                formSetRegionOutageManage.Dock = DockStyle.Fill;
                TabSetRegionOutageManage.Controls.Add(formSetRegionOutageManage);

                SetEmergencyLinkage formSetEmergencyLinkage = new SetEmergencyLinkage();
                formSetEmergencyLinkage.FormBorderStyle = FormBorderStyle.None;
                formSetEmergencyLinkage.TopLevel = false;
                formSetEmergencyLinkage.Visible = true;
                formSetEmergencyLinkage.Dock = DockStyle.Fill;
                TabSetEmergencyLinkage.Controls.Add(formSetEmergencyLinkage);

                CurvilinearQuery formCurvilinearQuery = new CurvilinearQuery();
                formCurvilinearQuery.FormBorderStyle = FormBorderStyle.None;
                formCurvilinearQuery.TopLevel = false;
                formCurvilinearQuery.Visible = true;
                formCurvilinearQuery.Dock = DockStyle.Fill;
                TabCurvilinearQuery.Controls.Add(formCurvilinearQuery);

                AnalysisResultsInRealtime formAnalysisResultsInRealtime = new AnalysisResultsInRealtime();
                formAnalysisResultsInRealtime.FormBorderStyle = FormBorderStyle.None;
                formAnalysisResultsInRealtime.TopLevel = false;
                formAnalysisResultsInRealtime.Visible = true;
                formAnalysisResultsInRealtime.Dock = DockStyle.Fill;
                TabAnalysisResultsInRealtime.Controls.Add(formAnalysisResultsInRealtime);

                SetEmergencyLinkageList formSetEmergencyLinkageList = new SetEmergencyLinkageList();
                formSetEmergencyLinkageList.FormBorderStyle = FormBorderStyle.None;
                formSetEmergencyLinkageList.TopLevel = false;
                formSetEmergencyLinkageList.Visible = true;
                formSetEmergencyLinkageList.Dock = DockStyle.Fill;
                TabViewEmergencyLinkage.Controls.Add(formSetEmergencyLinkageList);

            }
            catch
            {

            }
        }
    }
}
