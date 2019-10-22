using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetRegionOutage : XtraForm
    {
        string UserName = "";
        string UserID = "";
        bool isEdit = false;
        RegionOutageBusiness regionOutageBusiness;
        //参照： Sys.Safety.Client.Define.Sensor CuAnalog
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        SetAnalysisModelPointRecordBusiness setAnalysisModelPointRecordBusiness;

        string largedataAnalysisConfigId = "";
        /// <summary>
        /// 模型ID
        /// </summary>
        /// <param name="id"></param>
        public SetRegionOutage(string id)
        {
            InitializeComponent();

            largedataAnalysisConfigId = id;
            isEdit = !string.IsNullOrEmpty(id);
            if (isEdit)
                this.Text = "修改区域断电配置";

            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
            if (!string.IsNullOrEmpty(_ClientItem.UserName))
            {
                UserName = _ClientItem.UserName;
            }
            if (!string.IsNullOrEmpty(_ClientItem.UserID))
            {
                UserID = _ClientItem.UserID;
            }
        }
        /// <summary>
        /// 控制
        /// </summary>
        private List<CrossControlItem> CrossControlList = new List<CrossControlItem>();
        /// <summary>
        /// 解除控制
        /// </summary>
        private List<DeControlItem> DeControlItemList = new List<DeControlItem>();

        /// <summary>
        /// 数据源DT
        /// </summary>
        private DataTable CrossControlDT = new DataTable();

        List<Jc_DefInfo> pointDefineList = new List<Jc_DefInfo>();
        /// <summary>
        /// 初始化窗体
        /// </summary>
        public void LoadForm()
        {
            regionOutageBusiness = new RegionOutageBusiness();
            //参照： Sys.Safety.Client.Define.Sensor CuAnalog
            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
            setAnalysisModelPointRecordBusiness = new SetAnalysisModelPointRecordBusiness();

            gridLookUpEdit.Properties.ImmediatePopup = true;
            gridLookUpEdit.Properties.TextEditStyle = TextEditStyles.Standard;//允许输入
            gridLookUpEdit.Properties.NullText = "请输入模型名称";//清空默认值
            if (isEdit)
            {
                gridLookUpEdit.Properties.DataSource = largedataAnalysisConfigBusiness.LoadAnalysisTemplate();
                this.gridLookUpEdit.EditValue = largedataAnalysisConfigId;
                //this.gridLookUpEdit.Properties.ReadOnly = true;
                this.gridLookUpEdit.Enabled = false;
            }
            else
            {
                gridLookUpEdit.Properties.DataSource = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigWithoutRegionOutage();
            }

            pointDefineList = PointDefineBusiness.QueryPointByDevpropertIDCache(3);


            if (pointDefineList != null && pointDefineList.Count > 0)
                repositoryItemLookUpEdit1.DataSource = (from p in pointDefineList orderby p.Point ascending select new { PointName = p.Point, Point = p.Point, PointId = p.PointID, Wz = p.Wz, DevName = p.DevName }).ToList();
        }



        /// <summary>
        /// 数据改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (null == e.Value)
            {
                return;
            }
            if (e.Column.FieldName != "PointId")
            {
                return;
            }
        }
        /// <summary>
        /// 选择值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = e.RowHandle.ToString();
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.CdvCrossControlTrue.FocusedRowHandle >= 0)
            {
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CdvCrossControlTrue.DeleteRow(CdvCrossControlTrue.FocusedRowHandle);
                }
            }
        }
        /// <summary>
        /// 验证重复定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CdvCrossControl_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (e.RowHandle == -2147483647 || e.RowHandle >= 0)
            {
                string validatePoint = CdvCrossControlTrue.GetRowCellValue(e.RowHandle, "PointId").ToString();
                if (!PointDefineBusiness.ControlPointLegal(validatePoint))
                {
                    XtraMessageBox.Show("定义成甲烷风电闭锁的控制口，不能再定义在数据分析的控制口中！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Valid = false;
                    return;
                }
            }
            for (int i = 0; i < CdvCrossControlTrue.RowCount; i++)
            {
                if (e.RowHandle != i || e.RowHandle == -2147483647)
                {
                    if (CdvCrossControlTrue.GetRowCellValue(i, "PointId").ToString() == CdvCrossControlTrue.GetRowCellValue(e.RowHandle, "PointId").ToString())
                    {
                        XtraMessageBox.Show("存在重复定义！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Valid = false;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 修改控制点时验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemLookUpEdit1_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (null != e.OldValue)
            {
                string responseMessage = regionOutageBusiness.NoReleaseControlForAnalysysModelAndPoint(this.gridLookUpEdit.EditValue.ToString(), e.OldValue.ToString());
                if (!string.IsNullOrEmpty(responseMessage))
                {
                    XtraMessageBox.Show(responseMessage, "提示", MessageBoxButtons.OK);
                    e.Cancel = true;
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (isEdit && this.gridLookUpEdit.EditValue != null && this.CdvCrossControlTrue.FocusedRowHandle >= 0)
                {
                    string pointId = this.CdvCrossControlTrue.GetRowCellValue(this.CdvCrossControlTrue.FocusedRowHandle, "PointId").ToString();
                    string responseMessage = regionOutageBusiness.NoReleaseControlForAnalysysModelAndPoint(this.gridLookUpEdit.EditValue.ToString(), pointId);
                    if (!string.IsNullOrEmpty(responseMessage))
                    {
                        XtraMessageBox.Show(responseMessage, "提示", MessageBoxButtons.OK);
                        return;
                    }
                }
                CdvCrossControlTrue.DeleteRow(CdvCrossControlTrue.FocusedRowHandle);
            }
        }
        /// <summary>
        /// 保存报警设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = CdgControl.DataSource as DataTable;
                DataTable dtRemove = gridControlFalse.DataSource as DataTable;

                if (CheckData(dt, dtRemove) == false)
                    return;

                RegionOutageBusinessModel newRegionOutageBusinessModel = new RegionOutageBusinessModel();
                List<JC_RegionOutageConfigInfo> regionOutageConfigInfoList = new List<JC_RegionOutageConfigInfo>();

                string daID = this.gridLookUpEdit.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JC_RegionOutageConfigInfo regionOutageConfigInfo = new JC_RegionOutageConfigInfo();
                    //regionOutageConfigInfo.Id = Guid.NewGuid().ToString();
                    regionOutageConfigInfo.Id = IdHelper.CreateLongId().ToString();
                    regionOutageConfigInfo.AnalysisModelId = daID;
                    regionOutageConfigInfo.PointId = dt.Rows[i]["PointId"].ToString();
                    regionOutageConfigInfo.Point = dt.Rows[i]["Point"].ToString();
                    regionOutageConfigInfo.ControlStatus = 1;
                    regionOutageConfigInfo.CreatorId = UserID;
                    regionOutageConfigInfo.CreatorName = UserName;
                    regionOutageConfigInfo.IsRemoveControl = checkEditRemoveControl.Checked ? 1 : 0;
                    regionOutageConfigInfoList.Add(regionOutageConfigInfo);
                }

                //解除控制
                DataTable dtDeControlItem = gridControlFalse.DataSource as DataTable;
                List<DeControlItem> dtFalsh = new List<DeControlItem>();
                if (dtDeControlItem == null && dtDeControlItem.Rows.Count == 0)
                {
                    dtFalsh = new List<DeControlItem>();
                }
                else
                {
                    dtFalsh = ToListDeControlItem(dtDeControlItem);
                }

                if (dtFalsh != null && dtFalsh.Count > 0)
                {
                    for (int i = 0; i < dtFalsh.Count; i++)
                    {
                        JC_RegionOutageConfigInfo regionOutageConfigInfo = new JC_RegionOutageConfigInfo();
                        //regionOutageConfigInfo.Id = Guid.NewGuid().ToString();
                        regionOutageConfigInfo.Id = IdHelper.CreateLongId().ToString();
                        regionOutageConfigInfo.AnalysisModelId = daID;
                        regionOutageConfigInfo.PointId = dtFalsh[i].PointId;
                        regionOutageConfigInfo.Point = dtFalsh[i].Point;
                        regionOutageConfigInfo.RemoveModelId = dtFalsh[i].RemoveModelId;
                        regionOutageConfigInfo.ControlStatus = 0;
                        regionOutageConfigInfo.IsRemoveControl = 0;
                        regionOutageConfigInfo.CreatorId = UserID;
                        regionOutageConfigInfo.CreatorName = UserName;
                        regionOutageConfigInfoList.Add(regionOutageConfigInfo);
                    }
                }

                newRegionOutageBusinessModel.RegionOutageConfigInfoList = regionOutageConfigInfoList;
                newRegionOutageBusinessModel.AnalysisModelId = daID;
                string error = regionOutageBusiness.AddRegionOutageConfig(newRegionOutageBusinessModel);

                if (error == "100")
                {
                    XtraMessageBox.Show("保存成功", "消息");
                    OperateLogHelper.InsertOperateLog(16, "区域断电配置-更新【" + this.gridLookUpEdit.Text + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(newRegionOutageBusinessModel)), "区域断电配置-更新");

                    this.Close();
                }
                else
                {
                    XtraMessageBox.Show(error, "消息");
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 检测页面数据
        /// </summary>
        /// <param name="dt"></param>
        private bool CheckData(DataTable dt, DataTable dtRemove)
        {
            if (this.gridLookUpEdit.EditValue == null)
            {
                XtraMessageBox.Show("请选择分析模型", "消息");
                return false;
            }
            if (dt != null && dt.Rows.Count > 0 && dtRemove != null && dtRemove.Rows.Count > 0)
            {
                XtraMessageBox.Show("不能同时设置控制测点和解控测点.", "消息");
                return false;
            }

            int checkDataRowCount = 0;
            if (dt != null)
                checkDataRowCount += dt.Rows.Count;
            if (dtRemove != null)
                checkDataRowCount += dtRemove.Rows.Count;

            if (checkDataRowCount == 0)
            {
                XtraMessageBox.Show("请添加控制测点或解控测点", "消息");
                return false;
            }

            return true;
        }

        private void simpleBtnQuery_Click(object sender, EventArgs e)
        {

        }

        private void gridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string daID = this.gridLookUpEdit.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
                string xm = this.gridLookUpEdit.Text.Trim();

                gridControlLargedataData.DataSource = largedataAnalysisConfigBusiness.GetLargedataAnalysisConfigDetailById(daID);

                List<JC_RegionOutageConfigInfo> regionOutageConfigInfoList = regionOutageBusiness.GetRegionOutage(daID);

                CrossControlList = new List<CrossControlItem>();
                DeControlItemList = new List<DeControlItem>();
                bool isRemoveControl = false;
                if (regionOutageConfigInfoList != null && regionOutageConfigInfoList.Count > 0)
                {
                    foreach (var item in regionOutageConfigInfoList)
                    {
                        if (item.ControlStatus == 1)
                        {
                            isRemoveControl = item.IsRemoveControl == 1 ? true : false;
                            CrossControlItem crossControlItem = new CrossControlItem();
                            crossControlItem.PointId = item.PointId;
                            crossControlItem.Point = item.Point;
                            if (item.ControlStatus == 1)
                                crossControlItem.ControlType = "控制";
                            else
                                crossControlItem.ControlType = "解除控制";
                            crossControlItem.DelInfBtnStr = "删除";
                            CrossControlList.Add(crossControlItem);
                        }
                        else
                        {
                            DeControlItem DeControlItem = new DeControlItem();
                            DeControlItem.PointId = item.PointId;
                            DeControlItem.Point = item.Point;
                            DeControlItem.RemoveModelId = item.RemoveModelId;
                            DeControlItem.RemoveModelName = item.RemoveModelName;
                            DeControlItem.DelInfBtnStrFalse = "删除";
                            DeControlItemList.Add(DeControlItem);
                        }
                    }
                }
                checkEditRemoveControl.Checked = isEdit ? isRemoveControl : true;
                CdgControl.DataSource = ToDataTable(CrossControlList);
                gridControlFalse.DataSource = ToDataTable(DeControlItemList);

                if (DeControlItemList.Count > 0)
                {
                    checkEditRemoveControl.Enabled = false;
                }

                List<JC_LargedataAnalysisConfigInfo> JC_LargedataAnalysisConfigInfoList = largedataAnalysisConfigBusiness.GetLargeDataAnalysisConfigWithRegionOutage(string.Empty);
                if (!string.IsNullOrEmpty(daID) && JC_LargedataAnalysisConfigInfoList != null && JC_LargedataAnalysisConfigInfoList.Count > 0)
                {
                    JC_LargedataAnalysisConfigInfo selectedAnalysisModel = JC_LargedataAnalysisConfigInfoList.FirstOrDefault(q => q.Id == daID);
                    if (null != selectedAnalysisModel)
                        JC_LargedataAnalysisConfigInfoList.Remove(selectedAnalysisModel);
                }
                lookUpEditAnalysisTemplate.Properties.DataSource = JC_LargedataAnalysisConfigInfoList;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }



        /// <summary>
        /// 添加接触控制的测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string analysisModelId = this.lookUpEditAnalysisTemplate.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
                string pointId = this.lookUpEditPoint.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
                string point = this.lookUpEditPoint.Text;
                string RemoveModelName = this.lookUpEditAnalysisTemplate.Text.Trim();
                DataTable dt = gridControlFalse.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    DeControlItemList = new List<DeControlItem>();
                }
                else
                {
                    DeControlItemList = ToListDeControlItem(dt);
                }

                bool bAddState = true;
                if (DeControlItemList != null && DeControlItemList.Count > 0)
                {

                    foreach (var item in DeControlItemList)
                    {
                        if (item.PointId == pointId && item.RemoveModelId == analysisModelId)
                        {
                            bAddState = false;
                            XtraMessageBox.Show("请勿选择重复数据！", "消息");
                            break;
                        }
                    }
                }
                if (bAddState)
                {
                    DeControlItem deControlItem = new DeControlItem();
                    deControlItem.PointId = pointId;
                    deControlItem.Point = point;
                    deControlItem.RemoveModelName = RemoveModelName;
                    deControlItem.RemoveModelId = analysisModelId;
                    deControlItem.DelInfBtnStrFalse = "删除";
                    DeControlItemList.Add(deControlItem);

                    checkEditRemoveControl.Checked = false;
                    //checkEditRemoveControl.Properties.ReadOnly = true;
                    checkEditRemoveControl.Enabled = false;
                }
                gridControlFalse.DataSource = ToDataTable(DeControlItemList);
            }
            catch
            {
            }

        }

        private void lookUpEditAnalysisTemplate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.lookUpEditAnalysisTemplate.EditValue == null)
                    return;
                string daID = this.lookUpEditAnalysisTemplate.EditValue.ToString(); //是ookUpEdit.Properties.ValueMember的值
                if (gridLookUpEdit.EditValue == null)
                {
                    return;
                }
                if (daID.ToLower() == gridLookUpEdit.EditValue.ToString().ToLower())
                {
                    XtraMessageBox.Show("解除模型不能选择当前分析模型.\n要解除当前分析模型的控制请勾选【不满足条件时是否解除控制】", "提示消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lookUpEditAnalysisTemplate.EditValue = lookUpEditAnalysisTemplate.OldEditValue;
                    return;
                }

                List<JC_RegionOutageConfigInfo> JC_RegionOutageConfigInfoList =
                    regionOutageBusiness.GetRegionOutage(daID);

                List<Jc_DefInfo> pointList = ObjectConverter.DeepCopy(pointDefineList);
                List<Jc_DefInfo> closeControlList = new List<Jc_DefInfo>();
                if (JC_RegionOutageConfigInfoList != null && JC_RegionOutageConfigInfoList.Count > 0)
                {
                    foreach (var item in JC_RegionOutageConfigInfoList)
                    {
                        if (item.ControlStatus == 0)
                            continue;//解除控制的跳过，只加载被选模型配置为控制的测点。
                        Jc_DefInfo addItem = pointList.FirstOrDefault(p => p.PointID == item.PointId);
                        if (null != addItem)
                        {
                            closeControlList.Add(addItem);
                        }
                    }
                }
                //if(closeControlList.Count > 0)
                lookUpEditPoint.Properties.DataSource = (from p in closeControlList orderby p.Point ascending select new { PointName = p.Point, PointId = p.PointID, Point = p.Point, Wz = p.Wz, DevName = p.DevName }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("加载解控测点出错！ 错误消息:{0}", ex.StackTrace));
            }

        }

        private void repositoryItemButtonEdit3_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CdvCrossControlFalse.DeleteRow(CdvCrossControlFalse.FocusedRowHandle);

                if (CdvCrossControlFalse.DataRowCount > 0)
                {
                    //checkEditRemoveControl.Properties.ReadOnly = true;
                    checkEditRemoveControl.Enabled = false;
                }
                else
                {
                    //checkEditRemoveControl.Properties.ReadOnly = false;
                    checkEditRemoveControl.Enabled = true;
                }
            }
        }


        /// <summary>
        /// 将值对象列表转换为DataTable
        /// 如果vos为空,则返回空
        /// </summary>
        /// <param name="voList"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> vos)
        {
            Type voType = typeof(T);
            //构造数据表
            DataTable dt = new DataTable(voType.Name);
            PropertyInfo[] properties = voType.GetProperties();
            IDictionary<string, PropertyInfo> voProperties = new Dictionary<string, PropertyInfo>();
            //构造数据列
            foreach (PropertyInfo property in properties)
            {
                DataColumn col = new DataColumn(property.Name);
                col.DataType = property.PropertyType;
                col.Caption = property.Name;
                dt.Columns.Add(col);
                voProperties.Add(property.Name, property);
            }
            if (vos == null || vos.Count == 0)
            {
                return dt;
            }
            //读取记录数据
            foreach (object obj in vos)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pro in voProperties.Values)
                {
                    dr[pro.Name] = pro.GetValue(obj, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 将DT转换为List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<DeControlItem> ToListDeControlItem(DataTable dt)
        {
            List<DeControlItem> ret = new List<DeControlItem>();
            if (null != dt)
            {
                DeControlItem temp;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp = new DeControlItem();
                    temp.PointId = dt.Rows[i]["PointId"].ToString();
                    temp.Point = dt.Rows[i]["Point"].ToString();
                    temp.RemoveModelName = dt.Rows[i]["RemoveModelName"].ToString();
                    temp.RemoveModelId = dt.Rows[i]["RemoveModelId"].ToString();
                    temp.DelInfBtnStrFalse = "删除";
                    ret.Add(temp);
                }
            }
            return ret;
        }

        private void SetRegionOutage_Load(object sender, EventArgs e)
        {
            try
            {
                if (!GetIsDesignMode())
                {
                    LoadForm();
                }
            }
            catch
            {

            }
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
        /// 设置行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void CdvCrossControlFalse_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {

        }
    }
}
