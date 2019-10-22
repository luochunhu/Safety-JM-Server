using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Logging;
using Sys.Safety.Enums.Enums;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class AddAnalysisTemplate : XtraForm
    {
        AnalysisTemplateConfigBusiness analysisTemplateConfigBusiness = new AnalysisTemplateConfigBusiness();
        AnalysisTemplateBusinessModel analysisTemplateBusinessModel = new AnalysisTemplateBusinessModel();
       public JC_AnalysisTemplateInfo returnModel = null;
        string UserName = "";
        string UserID = "";
        string templateId = string.Empty;
        DataTable dtGrid = new DataTable();
        /// <summary>
        /// 参数列表
        /// </summary>
        List<string> paramterList = new List<string>();

        /// <summary>
        /// 因子列表
        /// </summary>
        List<string> factorList = new List<string>();

        /// <summary>
        /// 运算符列表
        /// </summary>
        List<string> signList = new List<string>();

        /// <summary>
        ///符号列表( ) 或 与
        /// </summary>
        List<string> operatorList = new List<string>();

        //新增模板  add   修改模板  edit
       public  string dataType = "add";
        //新增表达式  add   修改表达式  edit
        string operationExpresssionType = "add";
        //编辑表达式ID
        string operationExpresssionID = "";

        /// <summary>
        /// 新建表达式
        /// </summary>
        StringBuilder strNewExpresssion = new StringBuilder();
        /// <summary>
        /// 修改表达式
        /// </summary>
        StringBuilder strOldExpresssion = new StringBuilder();
        //列表每行的操作记录标记
        List<List<int>> listBoxoperationRecordlist = new List<List<int>>();


        /// <summary>
        /// 新建表达式项
        /// </summary>
        StringBuilder strNewExpresssionItem = new StringBuilder();

        /// <summary>
        /// 表达式项列表
        /// </summary>
        List<string> strNewExpresssionItemList = new List<string>();

        /// <summary>
        /// 记录操作痕迹:参数->因子   运算符  （ ） 或 与
        /// </summary>
        List<string> operationRecordList = new List<string>();


        //修改方式
        ModifyWay modifyWay = ModifyWay.None;

        /// <summary>
        /// 编辑模板，新增：templateId = null 
        /// </summary>
        /// <param name="templateId"></param>
        public AddAnalysisTemplate(string templateId = null)
        {
            InitializeComponent();
            this.templateId = templateId;
        }

        private void AddAnalysisTemplate_Load(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开分析模板编辑窗口...", "请等待...");

            //初始化窗体
            LoadForm(templateId);
            wdf.Close();
        }
        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void LoadForm(string templateId)
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
            if (string.IsNullOrWhiteSpace(templateId))
            {
                dataType = "add";
                this.Text = "新增逻辑分析模板";
            }
            else
            {
                dataType = "edit";
                this.Text = "修改逻辑分析模板";
                textEditName.Properties.ReadOnly = true;
            }

            //显示控件
            ShowDataControl("");

            //绑定默认数据源
            dtGrid = CreateDataTable();

            //初始化运算符列表
            LoadSignList();

            //初始化符号列表
            LoadOperatorList();

            //初始化选中状态
            LoadSelectState();

            //初始化因子和参数列表
            analysisTemplateBusinessModel = analysisTemplateConfigBusiness.LoadAnalysisTemplate(templateId);

            //初始化参数列表
            LoadParamterList(analysisTemplateBusinessModel.ParameterInfoList);

            //初始化因子列表
            LoadFactionList(analysisTemplateBusinessModel.FactorInfoList);

            //编辑时，初始化界面数据
            if (analysisTemplateBusinessModel.AnalysisTemplateInfo != null
                && !string.IsNullOrWhiteSpace(analysisTemplateBusinessModel.AnalysisTemplateInfo.Id))
            {
                textEditName.Text = analysisTemplateBusinessModel.AnalysisTemplateInfo.Name;
                if (analysisTemplateBusinessModel.AnalysisExpressionInfoList != null && analysisTemplateBusinessModel.AnalysisExpressionInfoList.Count > 0)
                    for (int i = 0; i < analysisTemplateBusinessModel.AnalysisExpressionInfoList.Count; i++)
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Id"] = analysisTemplateBusinessModel.AnalysisExpressionInfoList[i].Id;
                        dr["ExpresstionText"] = analysisTemplateBusinessModel.AnalysisExpressionInfoList[i].ExpresstionText;
                        dr["ContinueTime"] = analysisTemplateBusinessModel.AnalysisExpressionInfoList[i].ContinueTime;
                        dr["MaxContinueTime"] = analysisTemplateBusinessModel.AnalysisExpressionInfoList[i].MaxContinueTime;
                        dr["ExpresstionOperationRecord"] = analysisTemplateBusinessModel.AnalysisExpressionInfoList[i].ExpresstionOperationRecord;
                        dtGrid.Rows.Add(dr);
                    }
            }

            gridControlData.DataSource = dtGrid;
        }
        /// <summary>
        /// 初始化选中状态
        /// </summary>
        private void LoadSelectState()
        {
            EditSeleteParameterBtnState("");
            EditSeleteFactorBtnState("");
            EditSelectSignBtnState("");
        }

        /// <summary>
        /// 添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewData_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && rowHandle > 0)
            {
                e.Info.DisplayText = rowHandle.ToString();
            }

        }





        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckGridDataRowCount())
            {
                XtraMessageBox.Show("一个模板最多只能设置20个表达式", "消息");
                return;
            }
            //1.数据验证
            string strError = ValidateData();
            if (strError != "100")
            {
                XtraMessageBox.Show(strError, "消息");
                return;
            }
            //2.组装数据
            //1.模型
            JC_AnalysisTemplateInfo analysisTemplateInfo = new JC_AnalysisTemplateInfo();
            if (dataType == "add")
            {
                //analysisTemplateInfo.Id = Guid.NewGuid().ToString();
                analysisTemplateInfo.Id = IdHelper.CreateLongId().ToString();
                analysisTemplateInfo.IsDeleted = DeleteState.No;
                analysisTemplateInfo.Name = textEditName.Text.Trim();
                analysisTemplateInfo.CreatorId = UserID;
                analysisTemplateInfo.CreatorName = UserName;
            }
            else
            {
                analysisTemplateInfo = analysisTemplateBusinessModel.AnalysisTemplateInfo;
            }


            dtGrid = gridControlData.DataSource as DataTable;

            //2.表达式
            //AnalysisExpressionInfoList
            List<JC_AnalyticalExpressionInfo> analysisExpressionInfoList = new List<JC_AnalyticalExpressionInfo>();
            for (int i = 0; i < dtGrid.Rows.Count; i++)
            {
                JC_AnalyticalExpressionInfo analyticalExpressionInfo = new JC_AnalyticalExpressionInfo();
                analyticalExpressionInfo.Id = dtGrid.Rows[i]["Id"].ToString().Trim();

                if (!string.IsNullOrWhiteSpace(dtGrid.Rows[i]["ContinueTime"].ToString().Trim()))
                {
                    analyticalExpressionInfo.ContinueTime = int.Parse(dtGrid.Rows[i]["ContinueTime"].ToString().Trim());
                }
                else
                {
                    analyticalExpressionInfo.ContinueTime = 0;
                }

                if (!string.IsNullOrWhiteSpace(dtGrid.Rows[i]["MaxContinueTime"].ToString().Trim()))
                {
                    analyticalExpressionInfo.MaxContinueTime = int.Parse(dtGrid.Rows[i]["MaxContinueTime"].ToString().Trim());
                }
                else
                {
                    analyticalExpressionInfo.ContinueTime = 0;
                }

                analyticalExpressionInfo.CreatorId = UserID;
                analyticalExpressionInfo.CreatorName = UserName;
                analyticalExpressionInfo.ExpresstionText = dtGrid.Rows[i]["ExpresstionText"].ToString().Trim();
                analyticalExpressionInfo.ExpresstionOperationRecord = dtGrid.Rows[i]["ExpresstionOperationRecord"].ToString().Trim();
                analysisExpressionInfoList.Add(analyticalExpressionInfo);
            }
            //3.分析模板配置表
            List<JC_AnalysisTemplateConfigInfo> analysisTemplateConfigInfoList = new List<JC_AnalysisTemplateConfigInfo>();
            for (int i = 0; i < analysisExpressionInfoList.Count; i++)
            {
                JC_AnalysisTemplateConfigInfo analysisTemplateConfigInfo = new JC_AnalysisTemplateConfigInfo();
                //analysisTemplateConfigInfo.Id = Guid.NewGuid().ToString();
                analysisTemplateConfigInfo.Id = IdHelper.CreateLongId().ToString();
                analysisTemplateConfigInfo.ExpressionId = analysisExpressionInfoList[i].Id;
                analysisTemplateConfigInfo.TempleteId = analysisTemplateInfo.Id;

                analysisTemplateConfigInfoList.Add(analysisTemplateConfigInfo);
            }

            //4.表达式配置表
            List<JC_ExpressionConfigInfo> expressionConfigInfoList = new List<JC_ExpressionConfigInfo>();

            for (int i = 0; i < analysisExpressionInfoList.Count; i++)
            {
                List<ParamterFactor> paramterFactor = ExpressionResolution.GetParamterFactorForExpression(analysisExpressionInfoList[i].ExpresstionText
                    , paramterList, factorList);
                string expressionData = ExpressionResolution.SimulationLuaExpreesion(
              JSONHelper.ParseJSONString<List<string>>(analysisExpressionInfoList[i].ExpresstionOperationRecord));
                if (paramterFactor != null && paramterFactor.Count > 0)
                {
                    string paramterName = "";
                    string factorName = "";
                    for (int j = 0; j < paramterFactor.Count; j++)
                    {
                        JC_ExpressionConfigInfo expressionConfigInfo = new JC_ExpressionConfigInfo();
                        //expressionConfigInfo.Id = Guid.NewGuid().ToString();
                        expressionConfigInfo.Id = IdHelper.CreateLongId().ToString();

                        paramterName = paramterFactor[j].ParamterName;
                        factorName = paramterFactor[j].FactorName;
                        if (j == 0)
                        {
                            analysisExpressionInfoList[i].Expresstion = expressionData.Replace(paramterName + "->" + factorName, expressionConfigInfo.Id);
                        }
                        else
                        {
                            analysisExpressionInfoList[i].Expresstion = analysisExpressionInfoList[i].Expresstion.Replace(paramterName + "->" + factorName, expressionConfigInfo.Id);

                        }

                        expressionConfigInfo.ParameterId = analysisTemplateBusinessModel.ParameterInfoList.FirstOrDefault(t => t.Name == paramterName).Id;
                        expressionConfigInfo.FactorId = analysisTemplateBusinessModel.FactorInfoList.FirstOrDefault(t => t.Name == factorName).Id;
                        expressionConfigInfo.ExpressionId = analysisExpressionInfoList[i].Id;

                        expressionConfigInfoList.Add(expressionConfigInfo);


                    }
                }
            }

            AnalysisTemplateBusinessModel businessModel = new AnalysisTemplateBusinessModel();
            businessModel.AnalysisTemplateInfo = analysisTemplateInfo;
            businessModel.AnalysisExpressionInfoList = analysisExpressionInfoList;
            businessModel.ExpressionConfigInfoList = expressionConfigInfoList;
            businessModel.JC_AnalysisTemplateConfigInfoList = analysisTemplateConfigInfoList;
            string reError = analysisTemplateConfigBusiness.AddAnalysisTemplateConfig(businessModel, dataType);

            EditSeleteParameterBtnState();
            if (reError == "100")
            {
                XtraMessageBox.Show("保存成功", "消息");
                if (dataType == "add")
                {
                    OperateLogHelper.InsertOperateLog(16, "大数据分析模版-新增【" + analysisTemplateInfo.Name + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(businessModel)), "大数据分析模版-新增");
                }
                else
                {
                    OperateLogHelper.InsertOperateLog(16, "大数据分析模版-修改【" + analysisTemplateInfo.Name + "】," + string.Format("内容:{0}", JSONHelper.ToJSONString(businessModel)), "大数据分析模版-修改");
                }
                
                returnModel = analysisTemplateInfo; //将添加成功的对象返回主窗体
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                XtraMessageBox.Show(reError, "消息");
            }
        }


        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsbExpression_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //BaseListBoxControl listBox = sender as BaseListBoxControl;
            //List<string> listViewData = listBox.DataSource as List<string>;

            //RemoveSelectItems(listViewData);

        }



        /// <summary>
        /// 单击修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsbExpression_MouseClick(object sender, MouseEventArgs e)
        {
            modifyWay = ModifyWay.None;
            //try
            //{
            //    if (e.Clicks == 1)
            //    {//修改
            //        BaseListBoxControl listBox = sender as BaseListBoxControl;
            //        List<string> listViewData = listBox.DataSource as List<string>;

            //        var selectValue = lsbExpression.SelectedValue.ToString().Trim();

            //        if (selectValue != "("
            //           && selectValue != ")"
            //              && selectValue != "与"
            //              && selectValue != "或")
            //        {
            //            //List<int> dataListIndex = listBoxoperationRecordlist[lsbExpression.SelectedIndex];
            //            //List<string> dataListString = new List<string>();
            //            //foreach (var item in dataListIndex)
            //            //{
            //            //    dataListString.Add(operationRecordList[item]);
            //            //}
            //            //LoadPanlBtnData(dataListString);
            //        }
            //    }

            //}
            //catch
            //{

            //}

        }


        /// <summary>
        /// 因子按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtFactor_Click(object sender, EventArgs e)
        {

            SimpleButton sbt = sender as SimpleButton;

            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块

                if (selectSimpleButton.Text.Trim().Contains("->"))
                {
                    if((selectSimpleButton.Text.Trim() == "->开关量实时值")
                        || (selectSimpleButton.Text.Trim() != "->开关量实时值" && sbt.Text.Trim() == "开关量实时值"))
                    {
                        //开关量不能改， 模拟量可以改成除开关量之外的其它如：5分钟最大， 上线报警值
                        return;
                    }
                    //显示值是输入还是选择
                    ShowDataControl(sbt.Text.Trim());
                    int record = -1;
                    foreach (var item in panelControlEdit.Controls)
                    {
                        record++;
                        SimpleButton simpleButton = item as SimpleButton;
                        if (simpleButton == selectSimpleButton)
                        {
                            simpleButton.Text = "->" + sbt.Text;
                            selectSimpleButton.Text = "->" + sbt.Text;
                            btnList[record].Text = "->" + sbt.Text;
                            break;
                        }
                    }
                    //刷新控件
                    RefreshPanelControlEdit();

                    EditSeleteFactorBtnState(sbt.Text);
                }

                return;
            }


            //显示值是输入还是选择
            ShowDataControl(sbt.Text.Trim());
            //if (dataType == "add")
            //{
            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString().Trim();

                if (operationEndItem == "=")
                {//最后一次是等号
                    if (operationRecordList.Count - 2 >= 0)
                    {
                        if (operationRecordList[operationRecordList.Count - 2].ToString().Contains("->"))
                        {//最后一次是因子
                            operationRecordList[operationRecordList.Count - 2] = "->" + sbt.Text;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                }
                if (paramterList.Contains(operationEndItem))
                {//最后一次选的参数 

                    //检查是否已经选择了开关量实时值
                    if (checkHaveKGLSSZ(operationEndItem))
                    {
                        if (sbt.Text.Trim() != "开关量实时值")
                        {
                            XtraMessageBox.Show("当前参数已设置为开关量实时值，如果要选择模拟量，请选择其他参数。", "消息");
                            return;
                        }
                    }

                    operationRecordList.Add("->" + sbt.Text);
                }
                else if (operationEndItem.Contains("->"))
                {//最后一次是因子
                    operationRecordList[operationRecordList.Count - 1] = "->" + sbt.Text;
                }
                else
                {
                    if (operationRecordList.Count - 3 >= 0)
                    {
                        if (operationRecordList[operationRecordList.Count - 2] == "=")
                        {
                            if (sbt.Text.Trim() != operationRecordList[operationRecordList.Count - 3])
                                if (operationRecordList[operationRecordList.Count - 3] != "->开关量实时值" &&
                                   sbt.Text.Trim() == "开关量实时值")
                                {
                                    operationRecordList[operationRecordList.Count - 3] = "->开关量实时值";
                                    operationRecordList[operationRecordList.Count - 1] = "0态";
                                }
                                else if (operationRecordList[operationRecordList.Count - 3] == "->开关量实时值" &&
                                   sbt.Text.Trim() != "开关量实时值")
                                {
                                    operationRecordList[operationRecordList.Count - 3] = "->" + sbt.Text;
                                    operationRecordList[operationRecordList.Count - 1] = "0";
                                }
                        }
                    }
                }

            }
            else
            {
                return;
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();

            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();
            List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);

            EditSeleteFactorBtnState(sbt.Text);
        }
        /// <summary>
        /// 检查是否已经选择了开关量实时值
        /// </summary>
        /// <returns></returns>
        private bool checkHaveKGLSSZ(string dataEnd)
        {
            //因子
            var recordItem = "";
            for (int i = 1; i < operationRecordList.Count; i++)
            {
                if (operationRecordList[i] == "->开关量实时值")
                {
                    recordItem = operationRecordList[i - 1];
                    break;
                }
            }
            if (!string.IsNullOrWhiteSpace(recordItem))
            {
                if (dataEnd == recordItem)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 参数按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sbtnParameter_Click(object sender, EventArgs e)
        {
            SimpleButton sbt = sender as SimpleButton;

            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块

                if (paramterList.Contains(selectSimpleButton.Text.Trim()))
                {
                    int record = -1;
                    foreach (var item in panelControlEdit.Controls)
                    {
                        record++;
                        SimpleButton simpleButton = item as SimpleButton;
                        if (simpleButton == selectSimpleButton)
                        {
                            simpleButton.Text = sbt.Text;
                            selectSimpleButton.Text = sbt.Text;
                            btnList[record].Text = sbt.Text;
                            break;
                        }
                    }

                    //刷新控件
                    RefreshPanelControlEdit();
                    EditSeleteParameterBtnState(sbt.Text);
                }

                return;
            }


            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString();
                if (paramterList.Contains(operationEndItem))
                {//最后一次选的参数 
                    operationRecordList[operationRecordList.Count - 1] = sbt.Text;
                }
                else if (operationEndItem.Contains("->"))
                {//最后一次是因子
                    operationRecordList[operationRecordList.Count - 2] = sbt.Text;
                }
                else
                {

                    try
                    {//最后一次是数字 
                        double checkData = double.Parse(operationEndItem.Trim());
                    }
                    catch
                    {
                        operationRecordList.Add(sbt.Text);
                    }

                }
            }
            else
            {
                operationRecordList.Add(sbt.Text);
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();

            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();
            List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);

            EditSeleteParameterBtnState(sbt.Text);
        }
        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshPanelControlEdit()
        {
            //panelControlEdit.Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (panelControlEdit.Controls.Count > 0)
            {
                if (panelControlEdit.Controls[0] != null)
                    panelControlEdit.Controls[0].Dispose();
            }
            int left = 0;
            foreach (var item in btnList)
            {
                item.Width = TextRenderer.MeasureText(item.Text, item.Font).Width;
                item.Left = left;
                panelControlEdit.Controls.Add(item);
                left += item.Width;
            }
            panelControlEdit.Refresh();
        }
        /// <summary>
        /// 操作运算符按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOperator(object sender, EventArgs e)
        {
            SimpleButton sbt = sender as SimpleButton;

            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块

                if (signList.Contains(selectSimpleButton.Text.Trim()))
                {
                    int record = -1;
                    foreach (var item in panelControlEdit.Controls)
                    {
                        record++;
                        SimpleButton simpleButton = item as SimpleButton;
                        if (simpleButton == selectSimpleButton)
                        {
                            simpleButton.Text = sbt.Text;
                            selectSimpleButton.Text = sbt.Text;
                            btnList[record].Text = sbt.Text;
                            break;
                        }
                    }
                    //刷新控件
                    RefreshPanelControlEdit();
                    EditSelectSignBtnState(sbt.Text);
                }

                return;
            }


            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString();
                if (paramterList.Contains(operationEndItem))
                {//最后一次选的参数 
                    //operationRecordList[operationRecordList.Count - 1] = sbt.Text;
                }
                else if (operationEndItem.Contains("->"))
                {//最后一次是因子
                    if (operationEndItem == "->开关量实时值")
                    {
                        if (sbt.Text.Trim() == "=")
                        {
                            operationRecordList.Add(sbt.Text);
                        }
                    }
                    else
                    {
                        operationRecordList.Add(sbt.Text);
                    }
                }
                else if (signList.Contains(operationEndItem))
                {//最后一次是运算符
                    operationRecordList[operationRecordList.Count - 1] = sbt.Text;
                }
                else if (operationEndItem == ")")
                {//最后一次是符号 (  )  或  与
                    operationRecordList.Add(sbt.Text);
                }
                else
                {

                    try
                    {//最后一次是数字 
                        double checkData = double.Parse(operationEndItem.Trim());
                        operationRecordList.Add(sbt.Text);
                    }
                    catch
                    {

                    }

                }
            }
            else
            {
                operationRecordList.Add(sbt.Text);
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();

            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();

            List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);

            EditSelectSignBtnState(sbt.Text);
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块
                XtraMessageBox.Show("您还有未确认的修改记录，请点击修改按钮确认修改！", "消息");
                return;
            }

            if (XtraMessageBox.Show("是否确认删除已编辑表达式内容?删除后不能恢复表达式内容。", "确定是否删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                //清空操作记录
                ClearData();

                ShowDataControl("");
                LoadSelectState();

                //显示值是输入还是选择
                ShowDataControl(ExpressionResolution.GetEndFactorNameByOperationRecordList(operationRecordList));
            }
            else { }


        }
        /// <summary>
        /// 退格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块
                // XtraMessageBox.Show("您还有未确认的修改记录，请点击Update按钮确认修改！", "消息");
                return;
            }
            if (operationRecordList.Count > 0)
            {
                operationRecordList.RemoveAt(operationRecordList.Count - 1);
                //组合表达式
                strNewExpresssion = new StringBuilder();

                strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
                memoEditContent.Text = strNewExpresssion.ToString();
                List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
                lsbExpression.DataSource = listData;
                listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);
                LoadSelectState();
                //显示值是输入还是选择
                ShowDataControl(ExpressionResolution.GetEndFactorNameByOperationRecordList(operationRecordList));
            }
        }

        /// <summary>
        /// 检查单个表达式设备类型是否一致
        /// </summary>
        /// <returns></returns>
        private bool ValidateExpressionTheSameDevType(string expressionText)
        {
            List<string> deviceTypeList = null;
            for (int i = 1; i <= 5; i++)
            {
                deviceTypeList = new List<string>();
                System.Text.RegularExpressions.Regex findDeviceType = new System.Text.RegularExpressions.Regex(string.Format("设备{0}->(?<device>[a-zA-Z0-9_\\u4e00-\\u9fa5]+)", i.ToString()));
                var deviceTypeMatch = findDeviceType.Match(expressionText);
                while (deviceTypeMatch.Success)
                {
                    deviceTypeList.Add(deviceTypeMatch.Groups["device"].Value);
                    deviceTypeMatch = deviceTypeMatch.NextMatch();
                }
                if (deviceTypeList.Exists(q => q == "开关量实时值") && deviceTypeList.Where(q => q == "开关量实时值").ToList().Count != deviceTypeList.Count)
                {
                    //既有开关量又有模拟量检查不通过
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查多个表达式设备类型是否一致
        /// </summary>
        /// <returns></returns>
        private bool ValidateMultipleExpressionTheSameDevType(List<string> expressionValidateList)
        {
            StringBuilder validateDevType = new StringBuilder();
            foreach (var expression in expressionValidateList)
            {
                validateDevType.Append(string.Format("{0}|", expression));
            }
            if(!string.IsNullOrEmpty(validateDevType.ToString()))
            {
                return ValidateExpressionTheSameDevType(validateDevType.ToString().TrimEnd('|'));
            }
            return false;
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveFactor_Click(object sender, EventArgs e)
        {
            if (CheckGridDataRowCount())
            {
                XtraMessageBox.Show("一个模板最多只能设置20个表达式", "消息");
                return;
            }
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块
                XtraMessageBox.Show("您还有未确认的修改记录! 请点击修改按钮确认修改, 再点击保存表达式.", "保存提示");
                return;
            }
            if (ExpressionResolution.GetEndFactorNameByOperationRecordList(operationRecordList, paramterList, signList))
            {
                XtraMessageBox.Show("表达式格式错误，请检查下您输入的表达式。", "提示消息");
                return;
            }
            if (!ExpressionResolution.CheckExpreesion(operationRecordList, paramterList))
            {
                XtraMessageBox.Show("表达式格式错误，请检查下您输入的表达式。", "提示消息");
                return;
            }
            if(!ValidateExpressionTheSameDevType(memoEditContent.Text.Trim()))
            {
                XtraMessageBox.Show("设备类型不一致! 请检查表达式.", "提示消息");
                return;
            }
            else
            {
                if (operationExpresssionType == "add")
                {
                    DataRow dr = dtGrid.NewRow();
                    //dr["Id"] = Guid.NewGuid();
                    dr["Id"] = IdHelper.CreateLongId().ToString();
                    dr["ExpresstionText"] = memoEditContent.Text.Trim();
                    dr["ContinueTime"] = 0;
                    dr["MaxContinueTime"] = 0;
                    dr["ExpresstionOperationRecord"] = JSONHelper.ToJSONString(operationRecordList);

                    dtGrid.Rows.Add(dr);
                }
                else
                {
                    for (int i = 0; i < dtGrid.Rows.Count; i++)
                    {
                        if (dtGrid.Rows[i]["Id"].ToString() == operationExpresssionID)
                        {
                            dtGrid.Rows[i]["ExpresstionText"] = memoEditContent.Text.Trim();
                            dtGrid.Rows[i]["ExpresstionOperationRecord"] = JSONHelper.ToJSONString(operationRecordList);
                            break;
                        }
                    }

                }
                //清空操作记录
                ClearData();

                operationExpresssionType = "add";

                ShowDataControl("");



                LoadSelectState();

            }
        }


        /// <summary>
        /// 符号 (  )  或  与
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFH_Click(object sender, EventArgs e)
        {

            SimpleButton sbt = sender as SimpleButton;
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块

                if (selectSimpleButton.Text.Trim() == "或"
                    || selectSimpleButton.Text.Trim() == "与")
                {
                    int record = -1;
                    foreach (var item in panelControlEdit.Controls)
                    {
                        record++;
                        SimpleButton simpleButton = item as SimpleButton;
                        if (simpleButton == selectSimpleButton)
                        {
                            simpleButton.Text = sbt.Text;
                            selectSimpleButton.Text = sbt.Text;
                            btnList[record].Text = sbt.Text;
                            break;
                        }
                    }

                }

                return;
            }

            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString();
                if (sbt.Text == "或" || sbt.Text == "与")
                {
                    #region 或 与
                    if (operationEndItem == "=")
                    {//最后一次是等号
                        return;

                    }
                    if (paramterList.Contains(operationEndItem))
                    {//最后一次选的参数 
                        return;
                    }
                    else if (operationEndItem.Contains("->"))
                    {//最后一次是因子
                        operationRecordList.Add(sbt.Text);
                    }
                    else if (signList.Contains(operationEndItem))
                    {//最后一次是运算符
                        return;
                    }

                    else if (operationEndItem == ")")
                    {//最后一次是符号 (  )  或  与
                        operationRecordList.Add(sbt.Text);
                    }
                    else if (operationEndItem == "或" || operationEndItem == "与")
                    {//最后一次是符号 或  与
                        operationRecordList[operationRecordList.Count - 1] = sbt.Text;
                    }
                    else
                    {
                        try
                        {
                            float checkdata = float.Parse(operationEndItem);

                            operationRecordList.Add(sbt.Text);
                        }
                        catch
                        {
                            operationRecordList.Add(sbt.Text);
                        }
                    }

                    #endregion
                }
                else if (sbt.Text == "(")
                {
                    if (paramterList.Contains(operationEndItem))
                    {//最后一次选的参数 
                        return;
                    }
                    else if (operationEndItem.Contains("->"))
                    {//最后一次是因子
                        return;
                    }
                    else
                    {
                        operationRecordList.Add(sbt.Text);
                    }

                }
                else if (sbt.Text == ")")
                {
                    if (operationEndItem == "=")
                    {//最后一次是等号
                        return;

                    }
                    if (paramterList.Contains(operationEndItem))
                    {//最后一次选的参数 
                        return;
                    }
                    else if (signList.Contains(operationEndItem))
                    {//最后一次是运算符
                        return;
                    }
                    else if (operationEndItem == "或" || operationEndItem == "与")
                    {//最后一次是符号 或  与
                        return;
                    }
                    else if (operationEndItem.Contains("->"))
                    {//最后一次是因子
                        if (operationEndItem != "->开关量实时值")
                        {
                            operationRecordList.Add(sbt.Text);
                        }
                    }
                    else
                    {
                        operationRecordList.Add(sbt.Text);
                    }
                }
            }
            else
            {
                operationRecordList.Add(sbt.Text);
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();

            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();

            List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);

            // EditSelectSignBtnState(sbt.Text);

        }

        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="listControl"></param>
        public void RemoveSelectItems(List<string> listViewData)
        {
            if (listViewData == null || listViewData.Count == 0)
            {
                return;
            }
            var list = listViewData;
            var selectItems = lsbExpression.SelectedItems;

            int selectIndex = lsbExpression.SelectedIndex;
            var selectValue = lsbExpression.SelectedValue.ToString().Trim();


            var listDelete = new List<int>();
            if (selectValue == "或" || selectValue == "与")
            {
                XtraMessageBox.Show("不能直接删除或/与", "消息");
                return;
            }

            //记录删除列表
            listDelete = RecordDeleteList(selectValue, selectIndex, list);

            //删除操作记录
            var deleteoperationRecordIndexList = new List<int>();

            List<string> listBind = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                if (!listDelete.Contains(i))
                {//记录删除后的数据
                    listBind.Add(list[i].ToString());
                }
                else
                {
                    foreach (var item in listBoxoperationRecordlist[i])
                    {
                        deleteoperationRecordIndexList.Add(item);
                    }
                }
            }

            var operationRecordListBind = new List<string>();
            for (int i = 0; i < operationRecordList.Count; i++)
            {
                if (!deleteoperationRecordIndexList.Contains(i))
                {//记录删除后的数据
                    operationRecordListBind.Add(operationRecordList[i].ToString());

                }
            }


            //清楚括号
            list = listBind;
            operationRecordList = operationRecordListBind;
            listDelete = new List<int>();
            listBind = new List<string>();
            deleteoperationRecordIndexList = new List<int>();

            for (int i = 1; i < list.Count; i++)
            {//统计前后括号，如(参数->因子)
                if (i + 1 < list.Count && i - 1 >= 0)
                { //前后括号
                    if (list[i - 1].ToString().Trim() == "(" && list[i + 1].ToString().Trim() == ")")
                    {
                        listDelete.Add(i + 1);
                        listDelete.Add(i - 1);
                    }
                }
            }


            for (int i = 0; i < list.Count; i++)
            {
                if (!listDelete.Contains(i))
                {
                    listBind.Add(list[i].ToString());
                }
                else
                {
                    foreach (var item in listBoxoperationRecordlist[i])
                    {
                        deleteoperationRecordIndexList.Add(item);
                    }
                }
            }
            operationRecordListBind = new List<string>();
            for (int i = 0; i < operationRecordList.Count; i++)
            {
                if (!deleteoperationRecordIndexList.Contains(i))
                {//记录删除后的数据
                    operationRecordListBind.Add(operationRecordList[i].ToString());

                }
            }

            //lsbExpression.DataSource = null;
            lsbExpression.DataSource = listBind;
            operationRecordList = operationRecordListBind;
            strNewExpresssion = new StringBuilder();
            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listBind);

        }
        /// <summary>
        /// 获取删除的index列表
        /// </summary>
        /// <param name="selectValue">选择的值</param>
        /// <param name="selectIndex">选择的索引</param>
        /// <param name="list">listbox的列表</param>
        /// <returns></returns>
        public List<int> RecordDeleteList(string selectValue, int selectIndex, List<string> list)
        {
            var listDelete = new List<int>();
            if (selectValue == "(")
            {//当点击前括号时

                if (XtraMessageBox.Show("如果删除括号，将把括号里面的内容全部删除?", "确定是否删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //delete
                    for (int i = selectIndex; i < list.Count; i++)
                    {
                        listDelete.Add(i);
                        if (list[i].ToString().Trim() == ")")
                        {
                            break;
                        }
                    }

                    //自动删除后面的或 /与
                    if (listDelete[listDelete.Count - 1] + 1 < list.Count)
                    {
                        if (list[listDelete[listDelete.Count - 1] + 1] == "或" || list[listDelete[listDelete.Count - 1] + 1] == "与")
                        {
                            listDelete.Add(listDelete[listDelete.Count - 1] + 1);
                        }
                    }
                }
                else
                {
                    return listDelete;
                }
            }
            else if (selectValue == ")")
            {//当点击后括号时
                if (MessageBox.Show("如果删除括号，将把括号里面的内容全部删除?", "确定是否删除", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //delete
                    for (int i = selectIndex; i >= 0; i--)
                    {
                        listDelete.Add(i);
                        if (list[i].ToString().Trim() == "(")
                        {
                            break;
                        }
                    }

                    //自动删除后面的或 /与
                    if (selectIndex + 1 < list.Count)
                    {
                        if (list[selectIndex + 1] == "或" || list[selectIndex + 1] == "与")
                        {
                            listDelete.Add(selectIndex + 1);
                        }
                    }
                }
                else
                {
                    return listDelete;
                }
            }
            else
            {//判断表达式前后的符号 
                if (list.Count >= 2)
                {
                    if (selectIndex + 1 < list.Count)
                    {//后
                        if (list[selectIndex + 1].ToString().Trim() == "或" || list[selectIndex + 1].ToString().Trim() == "与")
                        {
                            listDelete.Add(selectIndex);
                            listDelete.Add(selectIndex + 1);
                        }
                    }

                    if (selectIndex - 1 >= 0)
                    {//前
                        if (list[selectIndex - 1].ToString().Trim() == "或" || list[selectIndex - 1].ToString().Trim() == "与")
                        {
                            listDelete.Add(selectIndex);
                            listDelete.Add(selectIndex - 1);
                        }
                    }


                }
                else
                {
                    listDelete.Add(0);
                }
            }

            return listDelete;

        }

        /// <summary>
        /// 选中某一行的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewData_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            operationRecordList = JSONHelper.ParseJSONString<List<string>>(gridViewData.GetDataRow(e.RowHandle)["ExpresstionOperationRecord"].ToString());
            var ExpresstionText = gridViewData.GetDataRow(e.RowHandle)["ExpresstionText"].ToString();
            operationExpresssionID = gridViewData.GetDataRow(e.RowHandle)["Id"].ToString();

            //表达式最后一个因子是开关量实时值时
            //显示值是输入还是选择
            int first = 0;
            bool firstHave = false;
            for (int i = operationRecordList.Count - 1; i > 0; i--)
            {
                if (operationRecordList[i].Contains("->"))
                {
                    first++;
                    if (first == 1 && operationRecordList[i] == "->开关量实时值")
                    {
                        firstHave = true;
                        ShowDataControl("开关量实时值");
                        comboBoxEditData.Text = operationRecordList[i + 2];

                        break;
                    }
                }
            }
            //值的输入框
            if (!firstHave)
                ShowDataControl("");


            //记录历史表达式
            strNewExpresssion = new StringBuilder();
            strNewExpresssion.Append(ExpresstionText);


            //表达式显示控件的内容
            memoEditContent.Text = strNewExpresssion.ToString();

            //表达式内容


            //列表内容
            var listData = ExpressionResolution.ExpressionToList(memoEditContent.Text);
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);
            operationExpresssionType = "edit";
        }

        /// <summary>
        /// 编辑值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtData_0_EditValueChanged(object sender, EventArgs e)
        {
            TextEdit sbt = sender as TextEdit;
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块
                float tryCheckDate;
                if (float.TryParse(selectSimpleButton.Text.Trim(), out tryCheckDate)
                    || selectSimpleButton.Text.Trim() == "0态"
                    || selectSimpleButton.Text.Trim() == "1态"
                    || selectSimpleButton.Text.Trim() == "2态")
                {
                    int record = -1;
                    foreach (var item in panelControlEdit.Controls)
                    {
                        record++;
                        SimpleButton simpleButton = item as SimpleButton;
                        if (simpleButton == selectSimpleButton)
                        {
                            simpleButton.Text = sbt.Text.ToString().Trim();
                            selectSimpleButton.Text = sbt.Text.ToString().Trim();
                            btnList[record].Text = sbt.Text.ToString().Trim();
                            break;
                        }
                    }
                }
                return;
            }

            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString();


                if (paramterList.Contains(operationEndItem))
                {//最后一次选的参数 

                }
                else if (operationEndItem.Contains("->"))
                {//最后一次是因子

                }
                else if (operationEndItem == ")")
                {//最后一次是 ）

                }
                else
                {
                    try
                    {
                        float checkdata = float.Parse(operationEndItem);
                        if (!string.IsNullOrWhiteSpace(sbt.Text))
                            operationRecordList[operationRecordList.Count - 1] = sbt.Text;
                    }
                    catch
                    {
                        if (!string.IsNullOrWhiteSpace(sbt.Text))
                            operationRecordList.Add(sbt.Text);
                    }
                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sbt.Text.Trim()))
                {
                    operationRecordList.Add(sbt.Text);
                }
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();
            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();

            ;
            var listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxEditData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEditData.SelectedIndex == -1 ||
               comboBoxEditData.SelectedIndex == 0)
                return;
            ComboBoxEdit sbt = sender as ComboBoxEdit;
            if (modifyWay == ModifyWay.Edit)
            {//编辑表达式中某一块
                int record = -1;
                foreach (var item in panelControlEdit.Controls)
                {
                    record++;
                    SimpleButton simpleButton = item as SimpleButton;
                    if (simpleButton == selectSimpleButton)
                    {
                        if (record - 2 >= 0 && btnList[record - 2].Text == "->开关量实时值" && btnList[record - 1].Text == "=")
                        {
                            simpleButton.Text = sbt.SelectedText.ToString().Trim();
                            selectSimpleButton.Text = sbt.SelectedText.ToString().Trim();
                            btnList[record].Text = sbt.SelectedText.ToString().Trim();
                        }
                        break;
                    }
                }
                return;
            }

            //if (dataType == "add")
            //{
            if (operationRecordList.Count > 0)
            {
                //读取最后一次操作
                string operationEndItem = operationRecordList[operationRecordList.Count - 1].ToString();


                if (paramterList.Contains(operationEndItem))
                {//最后一次选的参数 

                }
                else if (operationEndItem.ToString().Contains("->"))
                {//最后一次是因子
                    //try
                    //{
                    //    float checkdata = float.Parse(operationEndItem);

                    //    operationRecordList[operationRecordList.Count - 1] = sbt.SelectedIndex.ToString();
                    //}
                    //catch
                    //{
                    //    operationRecordList.Add(sbt.SelectedIndex.ToString());
                    //}
                }
                else if (operationEndItem == "(" || operationEndItem == ")" || operationEndItem == "或" || operationEndItem == "与")
                {//最后一次是 ）或 与

                }
                else if (operationEndItem == "=")
                {//最后一次是 =

                    if (operationRecordList[operationRecordList.Count - 2].ToString() == "->开关量实时值")
                    {
                        operationRecordList.Add(sbt.SelectedText.ToString().Trim());
                    }
                    //try
                    //{
                    //    float checkdata = float.Parse(operationEndItem);
                    //    if (operationRecordList[operationRecordList.Count - 2].ToString() == "=")
                    //        operationRecordList[operationRecordList.Count - 1] = sbt.SelectedIndex.ToString();
                    //}
                    //catch
                    //{
                    //    //operationRecordList.Add(sbt.SelectedIndex.ToString());
                    //}
                }
                else if (operationRecordList[operationRecordList.Count - 2] == "=")
                {//倒数第二次操作是等号
                    if (operationEndItem == "0态" || operationEndItem == "1态" || operationEndItem == "2态")
                    {
                        if (string.IsNullOrWhiteSpace(sbt.SelectedText.ToString().Trim()))
                        {
                            operationRecordList[operationRecordList.Count - 1] = sbt.Text.ToString().Trim();
                        }
                        else
                        {
                            operationRecordList[operationRecordList.Count - 1] = sbt.SelectedText.ToString().Trim();
                        }
                    }
                }

            }
            else
            {
                // operationRecordList.Add(sbt.SelectedIndex.ToString());
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();
            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();

            var listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);

            //}
            //else if (dataType == "edit")
            //{

            //}
        }

        #region 数据操作
        /// <summary>
        /// 创建绑定数据源
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("ExpresstionText");
            dt.Columns.Add("ContinueTime");
            dt.Columns.Add("MaxContinueTime");
            dt.Columns.Add("ExpresstionOperationRecord");
            return dt;
        }
        /// <summary>
        /// 100 返回成功
        /// </summary>
        /// <returns></returns>
        public string ValidateData()
        {

            if (string.IsNullOrWhiteSpace(textEditName.Text.Trim()))
            {
                return "逻辑分析模板名称不能为空";
            }

            if (textEditName.Text.Trim().Length > 50)
            {
                return "逻辑分析模板名称最长长度不能超过50个字符";
            }

            DataTable dtGrid = gridControlData.DataSource as DataTable;
            if (dtGrid == null || dtGrid.Rows.Count <= 0)
            {
                return "请为分析模板添加分析表达式";
            }else
            {
                //设备类型验证
                List<string> expressionValidateList = new List<string>();
                foreach (DataRow dr in dtGrid.Rows)
                {
                    expressionValidateList.Add(dr["ExpresstionText"].ToString());
                }
                if(!ValidateMultipleExpressionTheSameDevType(expressionValidateList))
                {
                    return "设备类型不一致! 请检查表达式.";
                }
            }

            return "100";
        }
        /// <summary>
        /// 检查表达式项列表
        /// </summary>
        /// <returns></returns>
        public bool checkExpression()
        {
            //表达式解析后的列表
            List<string> expressionItemList = new List<string>();

            //解析表达式
            expressionItemList = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());

            //1.括号成对

            //2.首末不能为或 /与

            //3.参数->因子


            return false;
        }
        /// <summary>
        /// dataTet=开关量实时值 显示选择0 1 2 态
        /// 等于其他时，输入值
        /// </summary>
        /// <param name="dataText"></param>
        private void ShowDataControl(string dataText)
        {
            if (dataText == "开关量实时值")
            {
                comboBoxEditData.Visible = true;
                comboBoxEditData.SelectedIndex = 0;
                txtData_0.Visible = false;
            }
            else
            {
                comboBoxEditData.Visible = false;
                txtData_0.Visible = true;
                txtData_0.Text = "";
            }
        }

        /// <summary>
        /// 清空显示数据
        /// </summary>
        private void ClearData()
        {
            //清空操作记录
            operationRecordList.Clear();

            //清空表达式显示控件的内容
            memoEditContent.Text = "";

            //清空列表内容
            lsbExpression.DataSource = new List<string>();
            listBoxoperationRecordlist = new List<List<int>>();
            //清空表达式内容
            strNewExpresssion = new StringBuilder();
        }
        #endregion


        #region 编辑表达式中某一块区域

        SimpleButton selectSimpleButton = new SimpleButton() { Text = "" };
        List<SimpleButton> btnList = new List<SimpleButton>();
        /// <summary>
        /// 记录修改表达式局部的操作list
        /// </summary>
        private List<int> recordOperationEditList = new List<int>();
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

            foreach (var item in listData)
            {
                SimpleButton createBtn = CreateBtn(item);

                btnList.Add(createBtn);
            }

            //刷新控件
            RefreshPanelControlEdit();

        }
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="text">显示文本值</param>
        /// <param name="left">左边的位置</param>
        /// <returns></returns>
        private SimpleButton CreateBtn(string text)
        {
            SimpleButton simpleButton = new SimpleButton();
            simpleButton.Text = text;
            simpleButton.Appearance.BackColor = System.Drawing.Color.Gainsboro;

            simpleButton.Appearance.Options.UseBackColor = true;
            simpleButton.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            simpleButton.Cursor = System.Windows.Forms.Cursors.Hand;
            //simpleButton.ContextMenuStrip = RightMenuBtn;
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
            //if (e.Button == MouseButtons.Right)
            //{
            //    //if (string.IsNullOrWhiteSpace(selectSimpleButton.Text))
            //    //{
            //    //    return;
            //    //}
            //    //else
            //    //{

            //    //    SimpleButton simpleButtonitem = new SimpleButton();

            //    //    int record = -1;
            //    //    foreach (var item in panelControlEdit.Controls)
            //    //    {
            //    //        record++;
            //    //        SimpleButton simpleButton = item as SimpleButton;
            //    //        if (simpleButton == selectSimpleButton)
            //    //        {
            //    //            simpleButtonitem = simpleButton;
            //    //            panelControlEdit.Controls.Remove(simpleButton);
            //    //            btnList.Remove(simpleButton);
            //    //            break;
            //    //        }
            //    //    }
            //    //    for (int i = record; i < btnList.Count; i++)
            //    //    {
            //    //        btnList[i].Left -= simpleButtonitem.Width;
            //    //    }

            //    //    selectSimpleButton = new SimpleButton() { Text = "" };
            //    //}
            //}
            //else if (e.Button == MouseButtons.Left)
            //{
            SimpleButton simpleButton = sender as SimpleButton;
            decimal tryParseM = 0.00M;
            if (simpleButton.Text.Trim().Contains("->") || decimal.TryParse(simpleButton.Text, out tryParseM))
            {//显示输入值或者选择0态 1态 2态
                ShowDataControl(simpleButton.Text.Trim().Replace("->", ""));
            }
            else
            {
                //,2017-06-27. 从后往前找，找到当前选择按钮前的第一个因子按钮 开始.
                int childCount = panelControlEdit.Controls.Count;
                int selectedButtonIndex = -1;
                for (int i = childCount - 1; i >= 0; i--)
                {
                    SimpleButton simpleButtonitem = panelControlEdit.Controls[i] as SimpleButton;
                    //找到当前选择的按钮.
                    if (simpleButtonitem == simpleButton)
                    {
                        selectedButtonIndex = i;
                    }
                    else
                    {
                        if (simpleButtonitem.Text.Trim().Contains("->") && selectedButtonIndex != -1)
                        {
                            //显示输入值或者选择0态 1态 2态
                            ShowDataControl(simpleButtonitem.Text.Trim().Replace("->", ""));
                            selectedButtonIndex = -1;
                        }
                    }
                }
                //,2017-06-27. 从后往前找，找到当前选择按钮前的第一个因子按钮 结束.
            }

            selectSimpleButton = simpleButton;
            foreach (var item in panelControlEdit.Controls)
            {
                SimpleButton simpleButtonitem = item as SimpleButton;
                if (simpleButtonitem != selectSimpleButton)
                {
                    simpleButtonitem.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                }
                else
                {
                    simpleButtonitem.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                }

            }

            panelControlEdit.Refresh();
            //}
        }

        #endregion

        #region 初始化数据
        /// <summary>
        /// 初始化符号
        /// </summary>
        private void LoadOperatorList()
        {
            operatorList.Add("(");
            operatorList.Add(")");
            operatorList.Add("与");
            operatorList.Add("或");
        }
        /// <summary>
        /// 初始化运算符
        /// </summary>
        private void LoadSignList()
        {
            signList.Add("-");
            signList.Add("+");
            signList.Add("×");
            signList.Add("÷");
            signList.Add("≠");
            signList.Add(">");
            signList.Add("≥");
            signList.Add("<");
            signList.Add("≤");
            signList.Add("=");
        }

        /// <summary>
        /// 初始化因子
        /// </summary>
        private void LoadFactionList(List<JC_FactorInfo> factorInfoList)
        {
            factorList = new List<string>();
            foreach (var item in factorInfoList)
            {
                factorList.Add(item.Name);
            }
            //factorList.Add("模拟量实时值");
            //factorList.Add("日最大值");
            //factorList.Add("日平均值");
            //factorList.Add("月平均值");
            //factorList.Add("开关量实时值");
            //factorList.Add("报警值");
            //factorList.Add("断电值");
            //factorList.Add("复电值");
            //factorList.Add("五分钟最大值");
            //factorList.Add("预警值");
            //factorList.Add("五分钟平均值");
            //factorList.Add("值");
        }
        /// <summary>
        /// 初始化设备
        /// </summary>
        private void LoadParamterList(List<JC_ParameterInfo> parameterInfoList)
        {
            paramterList = new List<string>();
            foreach (var item in parameterInfoList)
            {
                paramterList.Add(item.Name);
            }
            //paramterList.Add("设备1");
            //paramterList.Add("设备2");
            //paramterList.Add("设备3");
            //paramterList.Add("设备4");
            //paramterList.Add("设备5");
        }

        #endregion

        #region 选中状态控制
        /// <summary>
        /// 编辑参数的时候植入选中状态  模拟量实时值
        ///  当parameterName 为空时，取消选中状态
        /// </summary>
        public void EditSeleteParameterBtnState(string parameterName = "")
        {

            switch (parameterName)
            {
                case "设备1":
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "设备2":
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "设备3":
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "设备4":
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "设备5":
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    break;
                default: //取消选中
                    this.sbtnParameterOne.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterTwo.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterThree.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFour.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnParameterFive.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
            }
        }

        /// <summary>
        /// 编辑因子的时候植入选中状态  
        ///  当factorName 为空时，取消选中状态
        /// </summary>
        public void EditSeleteFactorBtnState(string factorName)
        {

            switch (factorName)
            {
                case "模拟量实时值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "开关量实时值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "日最大值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "日平均值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "月平均值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "上限报警值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "下限报警值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "上限预警值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "下限预警值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "上限复电值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "下限复电值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "五分钟最大值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "五分钟平均值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "上线断电值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "下线断电值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "周平均值":
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    break;
                default: //取消选中
                    this.sbtnFactorMNLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorKGLSSZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorRPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorYPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownBJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownYJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownFDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZZDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactor5FZPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorUpDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.sbtnFactorDownDDZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnWeekPJZ.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
            }
        }
        /// <summary>
        /// 编辑运算符的时候植入选中状态
        /// 当signName 为空时，取消选中状态
        /// </summary>
        public void EditSelectSignBtnState(string signName)
        {

            switch (signName)
            {
                case "-":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;

                    break;
                case "+":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;

                    break;
                case "×":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "÷":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "≠":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case ">":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "≥":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;

                case "<":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "≤":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
                case "=":
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
                    break;

                default: //取消选中
                    this.btnMinusSign.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnPlus.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnMultiply.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnDivide.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnNotEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreater.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnGreaterEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLess.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnLessEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    this.btnEqual.Appearance.BackColor = System.Drawing.Color.Gainsboro;
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 修改表达式局部逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateListItem_Click(object sender, EventArgs e)
        {
            if (modifyWay != ModifyWay.Edit)
            {//编辑表达式中某一块
                XtraMessageBox.Show("无更新记录", "消息");
                return;
            }
            //1.验证表达式
            List<string> dataBtnList = new List<string>();
            foreach (var item in btnList)
            {
                dataBtnList.Add(item.Text.Trim());
            }

            //2.组装数据 
            operationRecordList = UpdateListItemData(dataBtnList);
            //3.填充表达式到列表


            if (dataBtnList.Count > 0)
            {
                if (!ExpressionResolution.CheckExpreesion(operationRecordList, paramterList))
                {
                    XtraMessageBox.Show("表达式格式错误，请检查下您输入的表达式。", "提示消息");
                    return;
                }
            }

            //组合表达式
            strNewExpresssion = new StringBuilder();

            strNewExpresssion.Append(ExpressionResolution.ListToExpression(operationRecordList));
            memoEditContent.Text = strNewExpresssion.ToString();
            List<string> listData = ExpressionResolution.ExpressionToList(strNewExpresssion.ToString());
            lsbExpression.DataSource = listData;
            listBoxoperationRecordlist = ExpressionResolution.ListToLsbExpression(operationRecordList, listData);


            //4.清除数据

            recordOperationEditList = new List<int>();
            selectSimpleButton = new SimpleButton() { Text = "" };
            btnList = new List<SimpleButton>();
            //panelControlEdit.Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (panelControlEdit.Controls.Count > 0)
            {
                if (panelControlEdit.Controls[0] != null)
                    panelControlEdit.Controls[0].Dispose();
            }
            modifyWay = ModifyWay.None;
            LoadSelectState();

            //显示值是输入还是选择
            ShowDataControl(ExpressionResolution.GetEndFactorNameByOperationRecordList(operationRecordList));
        }
        /// <summary>
        /// 组装修改后的数据
        /// </summary>
        /// <param name="dataBtnList"></param>
        /// <returns></returns>
        private List<string> UpdateListItemData(List<string> dataBtnList)
        {
            List<string> newOperationRecordList = new List<string>();
            if (dataBtnList.Count > 0)
            {
                //start
                for (int i = 0; i < recordOperationEditList[0]; i++)
                {
                    newOperationRecordList.Add(operationRecordList[i]);
                }

                //eidt  content
                foreach (var item in btnList)
                {
                    newOperationRecordList.Add(item.Text.Trim());
                }
                //end
                for (int i = recordOperationEditList[recordOperationEditList.Count - 1] + 1; i < operationRecordList.Count; i++)
                {
                    newOperationRecordList.Add(operationRecordList[i]);
                }

            }
            else
            {
                for (int i = 0; i < operationRecordList.Count; i++)
                {
                    if (!recordOperationEditList.Contains(i))
                    {
                        newOperationRecordList.Add(operationRecordList[i]);
                    }
                }

            }

            return newOperationRecordList;
        }
        /// <summary>
        /// 左侧插入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddLeft_Click(object sender, EventArgs e)
        {
            modifyWay = ModifyWay.Left;
        }
        /// <summary>
        /// 右侧插入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddRight_Click(object sender, EventArgs e)
        {
            modifyWay = ModifyWay.Right;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemEdit_Click(object sender, EventArgs e)
        {
            modifyWay = ModifyWay.Edit;
            try
            {
                //修改
                BaseListBoxControl listBox = lsbExpression as BaseListBoxControl;
                List<string> listViewData = listBox.DataSource as List<string>;

                var selectValue = lsbExpression.SelectedValue.ToString().Trim();

                if (selectValue != "("
                   && selectValue != ")"
                      && selectValue != "与"
                      && selectValue != "或")
                {
                    List<int> dataListIndex = listBoxoperationRecordlist[lsbExpression.SelectedIndex];
                    List<string> dataListString = new List<string>();
                    foreach (var item in dataListIndex)
                    {
                        dataListString.Add(operationRecordList[item]);
                    }
                    //记录修改表达式局部的操作list
                    recordOperationEditList = dataListIndex;

                    LoadPanlBtnData(dataListString);
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            modifyWay = ModifyWay.Delete;
            BaseListBoxControl listBox = lsbExpression as BaseListBoxControl;
            List<string> listViewData = listBox.DataSource as List<string>;

            RemoveSelectItems(listViewData);
            modifyWay = ModifyWay.None;
        }

        private void gridViewData_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {

            if (gridViewData.FocusedColumn.Name == "gc_ContinueTime" || gridViewData.FocusedColumn.Name == "gc_MaxContinueTime")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(e.Value.ToString().Trim(), @"^[0-9]\d*$"))
                {
                    e.ErrorText = "只能输入大于等于0的正整数!";
                    e.Valid = false;
                    return;
                }
                else
                {
                    int inputContinueTime = 0;
                    if (int.TryParse(e.Value.ToString().Trim(), out inputContinueTime))
                    {
                        if (inputContinueTime > 3600)
                        {
                            e.ErrorText = "持续时间最长为3600秒!";
                            e.Valid = false;
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 删除表达式项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmBtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectSimpleButton.Text))
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
                    if (simpleButton == selectSimpleButton)
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

                selectSimpleButton = new SimpleButton() { Text = "" };
            }
        }
        /// <summary>
        /// 验证数字或者小数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtData_0_KeyPress(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (txtData_0.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(txtData_0.Text, out oldf);
                    b2 = float.TryParse(txtData_0.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = false;
                        }
                    }
                }
            }
            else
            {
                float f;
                bool b2 = false;
                b2 = float.TryParse(txtData_0.Text + e.KeyChar.ToString(), out f);
                if (b2 == true)
                {
                    if (f > 5000)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewData_MouseUp(object sender, MouseEventArgs e)
        {
            GridHitInfo HitInfo = gridViewData.CalcHitInfo(e.Location);//获取鼠标点击的位置
            if (HitInfo.InRowCell && HitInfo.Column != null)
            {
                if (HitInfo.Column.Caption.Trim() == "删除")
                {
                    if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        gridViewData.DeleteRow(gridViewData.FocusedRowHandle);
                    }
                }
            }
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemStick_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckGridDataRowCount())
                {
                    XtraMessageBox.Show("一个模板最多只能设置20个表达式", "消息");
                    return;
                }

                MemoryStream stream = Clipboard.GetAudioStream() as MemoryStream;
                BinaryFormatter formatter = new BinaryFormatter();
                List<GridData> gridDataList = formatter.Deserialize(stream) as List<GridData>;

                if (gridDataList != null && gridDataList.Count > 0)
                {
                    foreach (var item in gridDataList)
                    {
                        DataRow dr = dtGrid.NewRow();
                        dr["Id"] = Guid.NewGuid();
                        dr["ExpresstionText"] = item.ExpresstionText;
                        dr["ContinueTime"] = item.ContinueTime;
                        dr["MaxContinueTime"] = item.MaxContinueTime;
                        dr["ExpresstionOperationRecord"] = item.ExpresstionOperationRecord;
                        dtGrid.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("只能粘贴表达式", "消息");
            }

        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = gridViewData.FocusedRowHandle;
                if (gridViewData.GetRowCellValue(rowHandle, "Id") == null)
                {
                    XtraMessageBox.Show("无数据", "消息");
                    return;
                }

                List<GridData> gridDataList = new List<GridData>();


                GridData gridData = new GridData();
                gridData.Id = Guid.NewGuid().ToString();
                if (!string.IsNullOrWhiteSpace(gridViewData.GetRowCellValue(rowHandle, "ContinueTime").ToString()))
                    gridData.ContinueTime = int.Parse(gridViewData.GetRowCellValue(rowHandle, "ContinueTime").ToString());
                else
                    gridData.ContinueTime = 0;
                gridData.ExpresstionOperationRecord = gridViewData.GetRowCellValue(rowHandle, "ExpresstionOperationRecord").ToString();
                gridData.ExpresstionText = gridViewData.GetRowCellValue(rowHandle, "ExpresstionText").ToString();
                gridDataList.Add(gridData);



                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, gridDataList);
                Clipboard.SetData(DataFormats.Serializable, stream);
                Clipboard.SetAudio(stream);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                XtraMessageBox.Show("复制失败", "消息");
            }
        }

        /// <summary>
        /// 一个模型最多只能添加20个表达式
        /// </summary>
        /// <returns>true 超过20个表达式  false 少于20个表达式</returns>
        public bool CheckGridDataRowCount()
        {
            DataTable dtGrid = gridControlData.DataSource as DataTable;
            if (dtGrid == null || dtGrid.Rows.Count <= 20)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
    /// <summary>
    /// 用于复杂粘贴表达式
    /// </summary>
    [Serializable()]
    public class GridData
    {

        public string Id { get; set; }
        public string ExpresstionText { get; set; }
        public int ContinueTime { get; set; }

        public int MaxContinueTime { get; set; }
        public string ExpresstionOperationRecord { get; set; }

    }

}
