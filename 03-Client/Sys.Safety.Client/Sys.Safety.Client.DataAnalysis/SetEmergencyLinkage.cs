using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Basic.Framework.Common;
using Sys.Safety.Client.DataAnalysis.Business;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.Client.DataAnalysis.Common;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Client.Graphic;
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
    public partial class SetEmergencyLinkage : XtraForm
    {
        string UserName = "";
        string UserID = "";
        LargedataAnalysisConfigBusiness largedataAnalysisConfigBusiness;
        EmergencyLinkageBusiness emergencyLinkageBusiness;
        public SetEmergencyLinkage()
        {
            InitializeComponent();
        }
        /// <summary>
        ///  初始化窗体
        /// </summary>
        public void LoadForm()
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

            largedataAnalysisConfigBusiness = new LargedataAnalysisConfigBusiness();
            emergencyLinkageBusiness = new EmergencyLinkageBusiness();
            //初始化模型名称

            gridLookUpEdit.Properties.ImmediatePopup = true;
            gridLookUpEdit.Properties.TextEditStyle = TextEditStyles.Standard;//允许输入
            gridLookUpEdit.Properties.NullText = "请输入模型名称";//清空默认值
            gridLookUpEdit.Properties.DataSource = largedataAnalysisConfigBusiness.LoadAnalysisTemplate();
        }

        private void simpleBtnSave_Click(object sender, EventArgs e)
        {

        }

        private void SetEmergencyLinkage_Load(object sender, EventArgs e)
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
        /// 添加应急联动的窗体
        /// </summary>
        /// <param name="jsonStr">jsonStr</param>
        private void AddForm(string jsonStr = "")
        {
            try
            {
                GraphDrawing form;
                if (string.IsNullOrWhiteSpace(jsonStr))
                {
                    form = new GraphDrawing();
                }
                else
                {
                    form = new GraphDrawing(jsonStr);
                }
                form.FormBorderStyle = FormBorderStyle.None;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.TopLevel = false;
                form.ControlBox = false;
                form.WindowState = FormWindowState.Normal;
                form.Visible = true;
                form.Dock = DockStyle.Fill;
                if (groupControlForm.Controls.Count > 0)
                {
                    GraphDrawing itemForm = groupControlForm.Controls[0] as GraphDrawing;
                    itemForm.Close();
                    itemForm.Dispose();
                }
                groupControlForm.Controls.Add(form);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
            }
        }

        private void gridLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (this.gridLookUpEdit.EditValue == null)
            {
                XtraMessageBox.Show("请选择大数据分析模型", "消息");
                return;
            }


            //初始化配置窗体
            string daID = this.gridLookUpEdit.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
            string xm = this.gridLookUpEdit.Text.Trim();
            if (string.IsNullOrEmpty(daID))
            {
                XtraMessageBox.Show("请选择大数据分析模型", "消息");
            }
            else
            {
                try
                {
                    JC_EmergencyLinkageConfigInfo emergencyLinkageConfigInfo = emergencyLinkageBusiness.GetEmergencylinkageconfig(daID);
                    if (emergencyLinkageConfigInfo == null)
                    {
                        AddForm();
                    }
                    else
                    {
                        AddForm(emergencyLinkageConfigInfo.Coordinate);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Info(string.Format("加载应急联动图形数据出错, 错误消息:{0}", ex.Message));
                    XtraMessageBox.Show("加载应急联动图形数据出错, 错误消息:\n" + ex.Message, "加载数据出错", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void groupControlForm_Leave(object sender, EventArgs e)
        {

        }

        private void SetEmergencyLinkage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (groupControlForm.Controls.Count > 0)
            {
                GraphDrawing itemForm = groupControlForm.Controls[0] as GraphDrawing;
                itemForm.Close();
                itemForm.Dispose();
            }
        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridLookUpEdit.EditValue == null)
            {
                XtraMessageBox.Show("请选择大数据分析模型", "消息");
                return;
            }
            if (groupControlForm.Controls.Count == 0)
            {
                XtraMessageBox.Show("请选择大数据分析模型", "消息");
                return;
            }

            string daID = this.gridLookUpEdit.EditValue.ToString();　//是ookUpEdit.Properties.ValueMember的值
            EmergencyLinkageConfigBusinessModel emergencyLinkageConfigBusinessModel = new EmergencyLinkageConfigBusinessModel();
            emergencyLinkageConfigBusinessModel.AnalysisModelId = daID;
            JC_EmergencyLinkageConfigInfo emergencyLinkageConfigInfo = new JC_EmergencyLinkageConfigInfo();

            if (groupControlForm.Controls.Count > 0)
            {
                //保存联动
                var graph = new GraphicOperations();
                string result = graph.DoSaveDrawing(GraphDrawing.Mapobj);
        
                if (string.IsNullOrWhiteSpace(result))
                {
                    XtraMessageBox.Show("请配置应急联动范围", "消息");
                    return;
                }
                else
                {
                    emergencyLinkageConfigInfo.CreatorId = UserID;
                    emergencyLinkageConfigInfo.CreatorName = UserName;
                    emergencyLinkageConfigInfo.Id = Guid.NewGuid().ToString();
                    emergencyLinkageConfigInfo.AnalysisModelId = daID;
                    emergencyLinkageConfigInfo.Coordinate = result;
                }
            }
            emergencyLinkageConfigBusinessModel.EmergencyLinkageConfigInfo = emergencyLinkageConfigInfo;
            //保存配置信息
            string error = emergencyLinkageBusiness.AddEmergencyLinkageConfig(emergencyLinkageConfigBusinessModel);
            if (error == "100")
            {
                XtraMessageBox.Show("保存成功", "消息");
                OperateLogHelper.InsertOperateLog(16, "应急联动-更新【" + this.gridLookUpEdit.Text + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(emergencyLinkageConfigBusinessModel)), "应急联动-更新");
            }
            else
            {
                XtraMessageBox.Show(error, "消息");
            }

        }
        /// <summary>
        /// 绘制圆形联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGraphYuanXing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var graph = new GraphicOperations();
            string result = graph.DoOtherDrawingGraphics(GraphDrawing.Mapobj);
            if (!string.IsNullOrWhiteSpace(result))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result, "提示");
                return;
            }
            graph.DoDrawingCircle(GraphDrawing.Mapobj);
        }
        /// <summary>
        /// 绘制多边形联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGraphDuoBianXing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var graph = new GraphicOperations();
            string result = graph.DoOtherDrawingGraphics(GraphDrawing.Mapobj);
            if (!string.IsNullOrWhiteSpace(result))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(result, "提示");
                return;
            }
            graph.DoDrawingPolygon(GraphDrawing.Mapobj);
        }

        /// <summary>
        /// 清除联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var graph = new GraphicOperations();
            graph.DoClearDrawing(GraphDrawing.Mapobj);
            GraphDrawing gr = new GraphDrawing();
            gr.Jsonstr = "";
        }
    }
}
