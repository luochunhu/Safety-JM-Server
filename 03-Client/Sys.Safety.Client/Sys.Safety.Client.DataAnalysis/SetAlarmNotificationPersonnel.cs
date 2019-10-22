using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.Client.DataAnalysis.Common;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetAlarmNotificationPersonnel : XtraForm
    {
        string UserName = "";
        string UserID = "";
        //新增模板  add   修改模板  edit
        string dataType = "add";
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        AlarmNotificationPersonnelBusiness alarmNotificationPersonnelBusiness;
        string analysisModelId = string.Empty;
        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="AnalysisModelId"></param>
        public SetAlarmNotificationPersonnel(string AnalysisModelId = null)
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(AnalysisModelId))
            {
                this.Text = "新增报警推送";
                dataType = "add";
            }
            else
            {
                this.Text = "修改报警推送";
                analysisModelId = AnalysisModelId;
                dataType = "edit";
            }
        }
        private void SetAlarmNotificationPersonnel_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GetIsDesignMode())
                {
                    DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开设置报警通知窗体...", "请等待...");
                    //在这里加载窗体
                    LoadForm();
                    wdf.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.StackTrace);
            }
        }

        /// <summary>
        ///  初始化窗体
        /// </summary>
        public void LoadForm()
        {
            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
            alarmNotificationPersonnelBusiness = new AlarmNotificationPersonnelBusiness();

            //初始化报警类型
            DataTable dt = GetAlarmShow();
            checkedCBEAlarm.Properties.DisplayMember = "show";
            checkedCBEAlarm.Properties.ValueMember = "alarmShow";
            checkedCBEAlarm.Properties.SeparatorChar = ','; //逗号 隔开   存储的 值是 编号(ID)如 2，3，4
            checkedCBEAlarm.Properties.DataSource = dt;
            checkedCBEAlarm.RefreshEditValue();

            try
            {
                ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                if (!string.IsNullOrEmpty(_ClientItem.UserName))
                {
                    UserName = _ClientItem.UserName;
                }
                if (!string.IsNullOrEmpty(_ClientItem.UserID))
                {
                    UserID = _ClientItem.UserID;
                }
                AlarmNotificationPersonnelConfigBusinessModel model = alarmNotificationPersonnelBusiness.GetAlarmNotificationPersonnelByanalysisModelId(analysisModelId);
                gridControlModule.DataSource = model.AlarmNotificationPersonnelInfoList;
                if (dataType == "edit")
                {
                    lookUpAnalysisModels.Properties.DataSource = largedataAnalysisConfigBusiness.LoadAnalysisTemplate();
                    lookUpAnalysisModels.EditValue = analysisModelId;
                    lookUpAnalysisModels.SelectedText = analysisModelId;
                    lookUpAnalysisModels.Properties.ReadOnly = true;

                    //设置选中
                    for (int i = 0; i < gridViewModule.RowCount; i++)
                    {
                        var alarmNotificationPersonnelInfo = gridViewModule.GetRow(i) as Sys.Safety.DataContract.JC_AlarmNotificationPersonnelInfo;
                        if (alarmNotificationPersonnelInfo != null && alarmNotificationPersonnelInfo.IsCheck)
                        {
                            gridViewModule.SelectRow(i);
                        }
                    }

                    //2、设置默认值：
                    checkedCBEAlarm.EditValue = model.AlarmNotificationPersonnelConfigInfo.AlarmType;
                    checkedCBEAlarm.RefreshEditValue();
                    colorPickEdit.Color = Color.FromArgb(int.Parse(model.AlarmNotificationPersonnelConfigInfo.AlarmColor));
                }
                else
                {
                    lookUpAnalysisModels.Properties.DataSource = largedataAnalysisConfigBusiness.GetAnalysisModelWithoutAlarmConfig();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("获取报警推送人员出错, 错误消息{0}", ex.Message));
            }
        }
        /// <summary>
        /// 初始化报警类型
        /// </summary>
        /// <returns></returns>
        private static DataTable GetAlarmShow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("show", typeof(string));
            dt.Columns.Add("alarmShow", typeof(string));
            //dt.Rows.Add("不处理", "0");
            dt.Rows.Add("语音播报", "1");
            dt.Rows.Add("声光报警", "2");
            dt.Rows.Add("图文弹窗", "3");
            return dt;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            //1.数据验证
            string strError = ValidateData();
            if (strError != "100")
            {
                XtraMessageBox.Show(strError, "消息");
                return;
            }
            AlarmNotificationPersonnelConfigBusinessModel alarmNotificationPersonnelConfigBusinessModel = new AlarmNotificationPersonnelConfigBusinessModel();

            var addParms = new List<JC_AlarmNotificationPersonnelConfigInfo>();
            if (dataType == "add")
            {
                int[] selectedModels = searchLookUpEdit1View.GetSelectedRows();
                for (int i = 0; i < selectedModels.Length; i++)
                {
                    string analysisModelId = searchLookUpEdit1View.GetRowCellValue(selectedModels[i], "Id").ToString();

                    var addAlarmConfig = new JC_AlarmNotificationPersonnelConfigInfo();
                    addAlarmConfig.AlarmColor = colorPickEdit.Color.ToArgb().ToString();
                    addAlarmConfig.AlarmType = checkedCBEAlarm.EditValue.ToString();
                    addAlarmConfig.AnalysisModelId = analysisModelId;
                    addAlarmConfig.CreatorId = UserID;
                    addAlarmConfig.CreatorName = UserName;
                    addParms.Add(addAlarmConfig);
                }
                alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelConfigInfoList = addParms;
                if (alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelInfoList == null)
                    alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelInfoList = new List<JC_AlarmNotificationPersonnelInfo>();
                int[] selectedRows = this.gridViewModule.GetSelectedRows();

                for (int j = 0; j < selectedRows.Length; j++)
                {
                    string personId = this.gridViewModule.GetRowCellValue(selectedRows[j], "UserID").ToString();
                    JC_AlarmNotificationPersonnelInfo alarmNotificationPersonnelInfo = new JC_AlarmNotificationPersonnelInfo();
                    alarmNotificationPersonnelInfo.PersonId = personId;
                    alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelInfoList.Add(alarmNotificationPersonnelInfo);
                }
            }
            else
            {
                var updateAlarmConfig = alarmNotificationPersonnelBusiness.GetAlarmNotificationPersonnelByanalysisModelId(this.lookUpAnalysisModels.EditValue.ToString()).AlarmNotificationPersonnelConfigInfo;
                updateAlarmConfig.AlarmColor = colorPickEdit.Color.ToArgb().ToString();
                updateAlarmConfig.AlarmType = checkedCBEAlarm.EditValue.ToString();
                updateAlarmConfig.AnalysisModelId = this.lookUpAnalysisModels.EditValue.ToString();
                updateAlarmConfig.CreatorId = UserID;
                updateAlarmConfig.CreatorName = UserName;
                List<JC_AlarmNotificationPersonnelInfo> alarmNotificationPersonnelInfoList = new List<JC_AlarmNotificationPersonnelInfo>();
                int[] selectedRows = this.gridViewModule.GetSelectedRows();

                for (int j = 0; j < selectedRows.Length; j++)
                {
                    string personId = this.gridViewModule.GetRowCellValue(selectedRows[j], "UserID").ToString();
                    JC_AlarmNotificationPersonnelInfo alarmNotificationPersonnelInfo = new JC_AlarmNotificationPersonnelInfo();
                    //alarmNotificationPersonnelInfo.Id = Guid.NewGuid().ToString();
                    alarmNotificationPersonnelInfo.Id = IdHelper.CreateLongId().ToString();
                    alarmNotificationPersonnelInfo.AlarmConfigId = updateAlarmConfig.Id;
                    alarmNotificationPersonnelInfo.PersonId = personId;

                    alarmNotificationPersonnelInfoList.Add(alarmNotificationPersonnelInfo);
                }
                alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelConfigInfo = updateAlarmConfig;
                alarmNotificationPersonnelConfigBusinessModel.AlarmNotificationPersonnelInfoList = alarmNotificationPersonnelInfoList;
            }
            string reError = alarmNotificationPersonnelBusiness.AddAlarmNotificationPersonnelConfig(alarmNotificationPersonnelConfigBusinessModel, dataType);
            if (reError == "100")
            {
                XtraMessageBox.Show("保存成功", "消息");

                if (dataType == "add")
                {
                    OperateLogHelper.InsertOperateLog(16, "报警推送-新增【" + lookUpAnalysisModels.Text + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(alarmNotificationPersonnelConfigBusinessModel)), "报警推送-新增");
                }
                else
                {
                    OperateLogHelper.InsertOperateLog(16, "报警推送-修改【" + lookUpAnalysisModels.Text + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(alarmNotificationPersonnelConfigBusinessModel)), "报警推送-修改");
                }
                this.Close();
            }
            else
            {
                XtraMessageBox.Show(reError, "消息");
            }
        }
        /// <summary>
        /// 100 返回成功
        /// </summary>
        /// <returns></returns>
        public string ValidateData()
        {
            if(dataType == "add" && searchLookUpEdit1View.GetSelectedRows().Length == 0)
            {
                return "请选择大数据分析模型名称!";
            }

            if (string.IsNullOrWhiteSpace(colorPickEdit.Color.ToArgb().ToString()))
            {
                return "请选择报警颜色";
            }

            List<JC_AlarmNotificationPersonnelInfo> dataGrid = gridControlModule.DataSource as List<JC_AlarmNotificationPersonnelInfo>;
            if (dataGrid == null || dataGrid.Count <= 0)
            {
                return "请选择推送用户!";
            }

            int[] selectedRows = this.gridViewModule.GetSelectedRows();
            if (selectedRows == null || selectedRows.Length == 0)
            {
                return "请选择推送用户!";
            }

            return "100";
        }
        /// <summary>  
        /// 获取当前是否处于设计器模式  
        /// </summary>  
        /// <remarks>  
        /// 在程序初始化时获取一次比较准确，若需要时获取可能由于布局嵌套导致获取不正确，如GridControl-GridView组合。  
        /// </remarks>  
        /// <returns>是否为设计器模式</returns>  
        private bool GetIsDesignMode()
        {
            return (this.GetService(typeof(System.ComponentModel.Design.IDesignerHost)) != null
                || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
        }
        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewModule_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void searchLookUpEdit1View_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            lookUpAnalysisModels.RefreshEditValue();
        }

        private void searchLookUpEdit1_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            int[] rowsIndex = searchLookUpEdit1View.GetSelectedRows();
            foreach (var index in rowsIndex)
            {
                string modelName = searchLookUpEdit1View.GetRowCellValue(index, "Name").ToString();
                sb.Append(modelName).Append(",");
            }
            if(!string.IsNullOrEmpty(sb.ToString()))
                e.DisplayText = sb.ToString().TrimEnd(',');
        }

        private void lookUpAnalysisModels_QueryCloseUp(object sender, CancelEventArgs e)
        {
            lookUpAnalysisModels.RefreshEditValue();
        }
    }
}
