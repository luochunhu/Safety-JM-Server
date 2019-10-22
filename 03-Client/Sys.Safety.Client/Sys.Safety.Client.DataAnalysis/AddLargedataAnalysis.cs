using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using Basic.Framework.Common;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.Client.DataAnalysis.Common;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Enums.Enums;
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
    public partial class AddLargedataAnalysis : DevExpress.XtraEditors.XtraForm
    {
        AnalysisTemplateConfigBusiness analysisTemplateConfigBusiness = new AnalysisTemplateConfigBusiness();
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
        SetAnalysisModelPointRecordBusiness setAnalysisModelPointRecordBusiness = new SetAnalysisModelPointRecordBusiness();
        public JC_LargedataAnalysisConfigInfo returnModel = null;
        string UserName = "";
        string UserID = "";
        private bool isEidt = true;
        private string largedataAnalysisId = string.Empty;
        /// <summary>
        /// 大数据分析模型ID
        /// </summary>
        /// <param name="largedataAnalysisId"></param>
        public AddLargedataAnalysis(string largedataAnalysisId = null)
        {
            InitializeComponent();
            this.largedataAnalysisId = largedataAnalysisId;
        }

        private void AddLargedataAnalysis_Load(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开大数据分析模型编辑窗体...", "请等待...");
            // 初始化窗体
            LoadForm(largedataAnalysisId);
            wdf.Close();
        }
        /// <summary>
        ///  初始化窗体
        /// </summary>
        private void LoadForm(string largedataAnalysisId)
        {
            if (!string.IsNullOrEmpty(largedataAnalysisId))
            {
                this.largedataAnalysisId = largedataAnalysisId;
                this.Text = "修改大数据分析模型";
            }
            else
            {
                this.Text = "新增大数据分析模型";
            }

            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            if (!string.IsNullOrEmpty(_ClientItem.UserName))
            {
                UserName = _ClientItem.UserName;
            }
            if (!string.IsNullOrEmpty(_ClientItem.UserID))
            {
                UserID = _ClientItem.UserID;
            }
            isEidt = !string.IsNullOrEmpty(largedataAnalysisId);

            initControl();
            if (isEidt)
                editLoadForm();
        }

        public void ReBindGrid(string id, string points, string pontIds)
        {
            if (dataSource.Count > 0)
            {
                JC_SetAnalysisModelPointRecordInfo setPointRecrod = dataSource.FirstOrDefault(q => q.Id == id);
                if (setPointRecrod != null)
                {
                    setPointRecrod.PointId = pontIds;
                    setPointRecrod.Point = points;
                    setPointRecrod.PointActivity = "1";
                }
                foreach (var item in dataSource)
                {
                    if (item.ParameterId == setPointRecrod.ParameterId)
                    {
                        item.PointId = setPointRecrod.PointId;
                        item.Point = setPointRecrod.Point;
                    }
                }
                dataSource = dataSource.OrderBy(q => q.ExpressionId).ThenBy(q => q.ParameterName).ToList();
                setViewList(dataSource);
                //gridControlData.DataSource = dataSource;
                gridControlData.DataSource = viewList;
                gridView1.Columns["ExpressionId"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                gridView1.BestFitColumns();
                gridControlData.RefreshDataSource();
            }
        }

        private void initControl()
        {
            txtAnalysisInterval.Text = "30";
            lookUpEditName.Properties.DataSource = analysisTemplateConfigBusiness.GetAnalysisTemplateList().AnalysisTemplateInfoList;
            var devTypeDataSource = (from dev in largedataAnalysisConfigBusiness.GetAllDeviceClassCache()
                                     select new { DevTypeId = dev.LngEnumValue, DevTypeName = dev.StrEnumDisplay }).ToList();
            devTypeDataSource.Insert(0, new { DevTypeId = -1, DevTypeName = "选择设备类型" });
            repositoryItemLookUpEdit1.DataSource = devTypeDataSource;
        }

        private void RepositoryItemLookUpEdit1_EditValueChanged(object sender, System.EventArgs e)
        {
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            if (null != lookUpEdit && lookUpEdit.EditValue != null)
            {
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DevTypeId", lookUpEdit.EditValue);
                gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "DevTypeName", lookUpEdit.Text);
                string parameterId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ParameterId").ToString();
                SetDataSourceForDevType(parameterId, lookUpEdit.EditValue, lookUpEdit.Text);
                gridView1.RefreshData();
            }
        }

        private void GridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            var devicePropertyId = gridView1.GetRowCellValue(e.RowHandle, "DevicePropertyId").ToString();
            if (e.Column.FieldName.Equals("DevTypeId") && !string.IsNullOrEmpty(devicePropertyId))
            {
                RepositoryItemLookUpEdit lookupEdit = new RepositoryItemLookUpEdit();
                var devTypeDataSource = (from dev in largedataAnalysisConfigBusiness.GetAllDeviceClassCache()
                                         where dev.LngEnumValue3 == devicePropertyId
                                         select new { DevTypeId = dev.LngEnumValue, DevTypeName = dev.StrEnumDisplay, ValueType = dev.LngEnumValue3 }).ToList();
                devTypeDataSource.Insert(0, new { DevTypeId = -1, DevTypeName = "选择设备类型", ValueType = "-1" });
                lookupEdit.DataSource = devTypeDataSource;
                lookupEdit.DisplayMember = "DevTypeName";
                lookupEdit.ValueMember = "DevTypeId";
                lookupEdit.Columns.Clear();
                lookupEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] { new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DevTypeName", "设备类型") });
                //lookupEdit.NullText = string.Empty;
                //lookupEdit.ShowHeader = false;
                e.RepositoryItem = lookupEdit;
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "DevTypeId")
            {
                gridView1.SetRowCellValue(e.RowHandle, "DevTypeName", repositoryItemLookUpEdit1.GetDisplayText(e.Value));
                string parameterId = gridView1.GetRowCellValue(e.RowHandle, "ParameterId").ToString();
                SetDataSourceForDevType(parameterId, e.Value, repositoryItemLookUpEdit1.GetDisplayText(e.Value));
            }
        }

        private void SetDataSourceForDevType(string parameterId, object devTypeId, string devTypeName)
        {
            var setViewList = viewList.Where(q => q.ParameterId == parameterId);
            foreach (var item in setViewList)
            {
                item.DevTypeId = int.Parse(devTypeId.ToString());
                item.DevTypeName = devTypeName;
            }
            var list = dataSource.Where(q => q.ParameterId == parameterId);
            foreach (var item in list)
            {
                item.DevTypeId = int.Parse(devTypeId.ToString());
                item.DevTypeName = devTypeName;
            }
        }



        JC_LargedataAnalysisConfigInfo editModel;
        private void editLoadForm()
        {
            editModel = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigById(largedataAnalysisId);

            txtTempleteName.Text = editModel.Name;
            lookUpEditName.EditValue = editModel.TempleteId;
            lookUpEditName.Properties.ReadOnly = true;
            txtAnalysisInterval.EditValue = editModel.AnalysisInterval;
            txtTrueDescription.EditValue = editModel.TrueDescription;
            txtFalseDescription.EditValue = editModel.FalseDescription;

            dataSource = editModel.AnalysisModelPointRecordInfoList;
            dataSource = dataSource.OrderBy(q => q.ExpressionId).ThenBy(q => q.ParameterName).ToList();
            setViewList(dataSource);
            //gridControlData.DataSource = dataSource;
            gridControlData.DataSource = viewList;
            gridView1.Columns["ExpressionId"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            gridView1.BestFitColumns();
        }
        /// <summary>
        /// 保存大数据分析模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            if (dataSource.Count > 0)
            {


                //1.数据验证
                string strError = ValidateData();
                if (strError != "100")
                {
                    XtraMessageBox.Show(strError, "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                LargedataAnalysisConfigBusinessModel largedataAnalysisConfigBusinessModel = new LargedataAnalysisConfigBusinessModel();
                //2.获取界面数据
                largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo = CreateModel();

                //3.提交数据
                if (isEidt)
                {
                    string responseMessage = largedataAnalysisConfigBusiness.EditLargedataAnalysisConfig(largedataAnalysisConfigBusinessModel);
                    if (string.IsNullOrEmpty(responseMessage))
                    {
                        XtraMessageBox.Show("保存成功", "保存消息框", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OperateLogHelper.InsertOperateLog(16, "大数据分析模型-修改【" + largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo.Name + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo)), "大数据分析模型-修改");
                        returnModel = largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo; //将添加成功的对象返回主窗体
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show(responseMessage, "保存消息框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    string responseMessage = largedataAnalysisConfigBusiness.AddLargedataAnalysisConfig(largedataAnalysisConfigBusinessModel);
                    if (string.IsNullOrEmpty(responseMessage))
                    {
                        XtraMessageBox.Show("保存成功", "保存消息框", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OperateLogHelper.InsertOperateLog(16, "大数据分析模型-新增【" + largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo.Name + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo)), "大数据分析模型-新增");
                        returnModel = largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo; //将添加成功的对象返回主窗体
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        //当前窗口变为编辑模式
                        //this.largedataAnalysisId = largedataAnalysisConfigBusinessModel.LargedataAnalysisConfigInfo.Id;

                    }
                    else
                    {
                        XtraMessageBox.Show(responseMessage, "保存消息框", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("请选择分析模板，并配置表达式测点。", "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 创建模型数据
        /// </summary>
        private JC_LargedataAnalysisConfigInfo CreateModel()
        {
            if (isEidt)
            {
                editModel.Name = txtTempleteName.Text.Trim();
                editModel.TempleteId = this.lookUpEditName.EditValue.ToString();
                editModel.TrueDescription = txtTrueDescription.Text.Trim();
                editModel.FalseDescription = txtFalseDescription.Text.Trim();
                editModel.AnalysisInterval = int.Parse(txtAnalysisInterval.Text.Trim());

                //从视图列表viewList获取数据源。
                foreach (var item in viewList)
                {
                    var expressionConfigList = editModel.AnalysisModelPointRecordInfoList.Where(q => q.ExpressionId == item.ExpressionId && q.ParameterId == item.ParameterId);
                    foreach (var expressionConfig in expressionConfigList)
                    {
                        expressionConfig.DevTypeId = item.DevTypeId;
                        expressionConfig.DevTypeName = string.Empty;
                        if (item.DevTypeId > -1)
                            expressionConfig.DevTypeName = item.DevTypeName;
                        expressionConfig.Point = string.IsNullOrEmpty(item.Point) ? string.Empty : item.Point;
                        expressionConfig.PointId = string.IsNullOrEmpty(item.PointId) ? string.Empty : item.PointId;
                        expressionConfig.PointActivity = item.PointActivity;
                    }
                }

                return editModel;
            }

            //Add Model
            JC_LargedataAnalysisConfigInfo largedataAnalysisConfigInfo = new JC_LargedataAnalysisConfigInfo();
            //largedataAnalysisConfigInfo.Id = Guid.NewGuid().ToString();
            largedataAnalysisConfigInfo.Id = IdHelper.CreateLongId().ToString();

            string templeteId = this.lookUpEditName.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
            largedataAnalysisConfigInfo.TempleteId = templeteId;
            largedataAnalysisConfigInfo.IsDeleted = DeleteState.No;
            largedataAnalysisConfigInfo.IsEnabled = EnableState.Yes;

            largedataAnalysisConfigInfo.Name = txtTempleteName.Text.Trim();
            largedataAnalysisConfigInfo.TrueDescription = txtTrueDescription.Text.Trim();
            largedataAnalysisConfigInfo.FalseDescription = txtFalseDescription.Text.Trim();
            largedataAnalysisConfigInfo.AnalysisInterval = int.Parse(txtAnalysisInterval.Text.Trim());
            largedataAnalysisConfigInfo.AnalysisResult = 0;
            largedataAnalysisConfigInfo.CreatorId = UserID;
            largedataAnalysisConfigInfo.CreatorName = UserName;
            //从视图列表viewList获取数据源。
            foreach (var item in viewList)
            {
                var expressionConfigList = dataSource.Where(q => q.ExpressionId == item.ExpressionId && q.ParameterId == item.ParameterId);
                foreach (var expressionConfig in expressionConfigList)
                {
                    expressionConfig.DevTypeId = item.DevTypeId;
                    expressionConfig.DevTypeName = string.Empty;
                    if (item.DevTypeId > -1)
                        expressionConfig.DevTypeName = item.DevTypeName;
                    expressionConfig.Point = string.IsNullOrEmpty(item.Point) ? string.Empty : item.Point;
                    expressionConfig.PointId = string.IsNullOrEmpty(item.PointId) ? string.Empty : item.PointId;
                    expressionConfig.PointActivity = item.PointActivity;
                }
            }
            largedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList = dataSource;

            return largedataAnalysisConfigInfo;
        }
        /// <summary>
        /// 100 返回成功
        /// </summary>
        /// <returns></returns>
        public string ValidateData()
        {
            if (string.IsNullOrWhiteSpace(txtTempleteName.Text.Trim()))
            {
                return "大数据分析模型名称不能为空";
            }

            if (txtTempleteName.Text.Trim().Length > 50)
            {
                return "大数据分析模型名称最长长度不能超过50个字符";
            }

            if (string.IsNullOrWhiteSpace(this.lookUpEditName.EditValue.ToString()))
            {
                return "请选择分析模板";
            }

            try
            {
                int analysisInterval = int.Parse(txtAnalysisInterval.Text.Trim());

                if (analysisInterval < 5 || analysisInterval > 3600)
                    return "分析周期大于等于5秒,小于等于3600秒";
            }
            catch
            {
                return "分析周期输入有误";
            }


            if (string.IsNullOrWhiteSpace(txtTrueDescription.Text.Trim()))
            {
                return "满足条件输出状态不能为空";
            }

            if (string.IsNullOrWhiteSpace(txtFalseDescription.Text.Trim()))
            {
                return "不满足条件输出状态不能为空";
            }

            List<JC_SetAnalysisModelPointRecordInfo> dtGrid = gridControlData.DataSource as List<JC_SetAnalysisModelPointRecordInfo>;
            if (dtGrid == null || dtGrid.Count <= 0)
            {
                return "请选择分析模板";
            }

            if (viewList.Any(q => string.IsNullOrEmpty(q.PointId)) && viewList.Any(q => q.DevTypeId == -1))
            {
                return "请为参数设置测点或指定设备类型";
            }
            if (viewList.Any(q => !string.IsNullOrEmpty(q.PointId)) && viewList.Any(q => q.DevTypeId >= 0))
            {
                return "设置测点或绑定类型只能选择一种方式";
            }
            if ((viewList.Any(q => !string.IsNullOrEmpty(q.PointId)) && viewList.Count(q => !string.IsNullOrEmpty(q.PointId)) != viewList.Count)
                || (viewList.Any(q => q.DevTypeId > -1) && viewList.Count(q => q.DevTypeId > -1) != viewList.Count))
            {
                return "参数设置不完整.";
            }

            IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> expressionGroup = viewList.GroupBy(p => p.ExpressionId);
            foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expressionParameterList in expressionGroup)
            {
                if (expressionParameterList.GroupBy(p => p.DevTypeId).Count() > 1)
                {
                    return string.Format("表达式【{0}】只能设置一种设备类型.", expressionParameterList.FirstOrDefault().Expresstion);
                }
            }


            return "100";
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private List<JC_SetAnalysisModelPointRecordInfo> dataSource = new List<JC_SetAnalysisModelPointRecordInfo>();

        /// <summary>
        /// 页面视图数据
        /// </summary>
        private List<JC_SetAnalysisModelPointRecordInfo> viewList = new List<JC_SetAnalysisModelPointRecordInfo>();

        private void setViewList(List<JC_SetAnalysisModelPointRecordInfo> source)
        {
            viewList.Clear();
            IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> expressionGroup = source.GroupBy(p => p.ExpressionId);
            foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> expressionParameterList in expressionGroup)
            {
                IEnumerable<IGrouping<string, JC_SetAnalysisModelPointRecordInfo>> parameterGroup = expressionParameterList.GroupBy(p => p.ParameterId);
                foreach (IGrouping<string, JC_SetAnalysisModelPointRecordInfo> parameterList in parameterGroup)
                {
                    JC_SetAnalysisModelPointRecordInfo vi = parameterList.FirstOrDefault();
                    if (null != vi)
                        viewList.Add(ObjectConverter.DeepCopy(vi));
                }
            }
        }

        private void lookUpEditName_EditValueChanged(object sender, EventArgs e)
        {
            if (!isEidt)
            {
                string templeteId = this.lookUpEditName.EditValue.ToString(); //是ookUpEdit.Properties.ValueMember的值
                if (string.IsNullOrEmpty(templeteId))
                    return;
                dataSource = setAnalysisModelPointRecordBusiness.GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(templeteId);
                dataSource = dataSource.OrderBy(q => q.ExpressionId).ThenBy(q => q.ParameterName).ToList();
                //gridControlData.DataSource = dataSource;
                setViewList(dataSource);
                gridControlData.DataSource = viewList;
                gridView1.Columns["ExpressionId"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                gridView1.RefreshData();
            }
        }
        /// <summary>
        /// 设置测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit4_Click(object sender, System.EventArgs e)
        {
            int[] selectedRows = this.gridView1.GetSelectedRows();
            if (selectedRows.Length > 0)
            {
                string id = this.gridView1.GetRowCellValue(selectedRows[0], "Id").ToString();
                int devicePropertyId;

                int.TryParse(this.gridView1.GetRowCellValue(selectedRows[0], "DevicePropertyId").ToString(), out devicePropertyId);
                if (devicePropertyId > 0)
                {
                    bool isSingle = true;
                    if (!isEidt)
                    {
                        bool isMultipleParameter = dataSource.GroupBy(g => g.ParameterId).ToList().Count > 1;
                        isSingle = isMultipleParameter;
                    }
                    SetPoint setPoint = new SetPoint(this, id, devicePropertyId, isSingle);
                    setPoint.ShowDialog();
                }
            }
        }

        private void gridView1_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            if (e.Column.FieldName == "ParameterName" || e.Column.FieldName == "Point" || e.Column.FieldName == "DelInfBtnStr")
            {
                e.Merge = false;
                e.Handled = true;
            }
            else
            {
                if (e.Column.FieldName == "ExpresstionText")
                {
                    //ExpressionId
                    DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
                    string expressionId1 = view.GetRowCellValue(e.RowHandle1, view.Columns["ExpressionId"]).ToString();
                    string expressionId2 = view.GetRowCellValue(e.RowHandle2, view.Columns["ExpressionId"]).ToString();
                    e.Merge = expressionId1 == expressionId2;
                    e.Handled = true;
                }
                else
                {
                    e.Merge = true;
                    e.Handled = false;
                }
            }

        }

        private void txtAnalysisInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8 || (e.KeyChar >= (char)48 && e.KeyChar <= (char)57))
            {
                //退格或数字
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtAnalysisInterval_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int number = 0;
            if (!int.TryParse(txtAnalysisInterval.Text.Trim(), out number))
            {
                XtraMessageBox.Show("请输入数字", "消息");
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            var currentView = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            Rectangle r = e.Bounds;
            if (currentView != null)
            {
                if (e.Column.FieldName == "Point")
                {
                    object pointActivity = currentView.GetRowCellValue(e.RowHandle, "PointActivity");
                    if (null != pointActivity && pointActivity.ToString() == "0")
                    {
                        e.Appearance.ForeColor = Color.Red;
                        e.DisplayText = string.Format("{0}(已删除)", e.DisplayText);
                        e.Appearance.DrawString(e.Cache, e.DisplayText, r);
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtAnalysisInterval_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
