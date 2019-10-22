using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ButtonPanel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Control.Model;
using Sys.Safety.Client.Control.Properties;
using Sys.Safety.Client.Define.Model;
using DEFServiceModel = Sys.Safety.Client.Control.Model.DEFServiceModel;
using GridClumnsMrg = Sys.Safety.Client.Control.Model.GridClumnsMrg;
using GridSource = Sys.Safety.Client.Control.Model.GridSource;
using JCSDKZServiceModel = Sys.Safety.Client.Control.Model.JCSDKZServiceModel;

namespace Sys.Safety.Client.Control
{
    public partial class CFHandCtrl : XtraForm
    {
        /// <summary>
        ///     传入的分站号
        /// </summary>
        private readonly string _Station = "";

        public CFHandCtrl()
        {
            InitializeComponent();
        }

        public CFHandCtrl(string Station)
        {
            _Station = Station;
            InitializeComponent();
        }

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFHandCtrl_Load(object sender, EventArgs e)
        {
            try
            {
                LoadInf();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CgleStaionAdress_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                windowsUIButtonPanel1.Buttons.Clear();
                windowsUIButtonPanel2.Buttons.Clear();
                if (CgleStaionAdress.EditValue == null)
                    return;
                if (CgleStaionAdress.EditValue.ToString().Length < 3)
                    return;
                var tempButtons1 = GetControlButtons(CgleStaionAdress.EditValue.ToString());
                var tempButtons2 = GetControlButtons(CgleStaionAdress.EditValue.ToString(), true);
                if (tempButtons1 != null)
                {
                    CtimeCotrolReal.Enabled = false;
                    windowsUIButtonPanel1.Buttons.AddRange(tempButtons1);
                    windowsUIButtonPanel2.Buttons.AddRange(tempButtons2);
                    SetControlCurrent(CgleStaionAdress.EditValue.ToString());
                    CtimeCotrolReal.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void LoadInf()
        {
            List<Jc_DefInfo> temp = DEFServiceModel.QueryPointByDevpropertIDCache(0).ToList();
            try
            {
                temp = temp.OrderBy(item => item.Fzh).ToList();
                temp.ForEach(a =>
                {
                    a.Wz = a.Point + ":" + a.Wz + "[" + a.DevName + "]";//增加测点号，安装位置，设备类型三个值显示  20170716
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            if (null == temp)
                return;
            var dtTemp = ListToDataTable(temp);
            CgleStaionAdress.Properties.View.BestFitColumns();
            CgleStaionAdress.Properties.DisplayMember = "Wz";
            CgleStaionAdress.Properties.ValueMember = "Point";
            if (null != dtTemp)
                CgleStaionAdress.Properties.DataSource = dtTemp;
            if (!string.IsNullOrEmpty(_Station))
                CgleStaionAdress.EditValue = _Station;

            AddColumnInf();
            IList<Jc_JcsdkzInfo> tempControl = JCSDKZServiceModel.QueryJCSDKZsCache();
            EvaluateControleDataSource(tempControl);
            if (null != GridSource.GridControlSource)
            {
                CGridControl.DataSource = GridSource.GridControlSource;
                CGridControl.RefreshDataSource();
            }

            barStaticItem2.Caption = "就绪";
        }

        /// <summary>
        ///     将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        private static DataTable ListToDataTable<T>(IList<T> entitys)
        {
            //检查实体集合不能为空
            if ((entitys == null) || (entitys.Count < 1))
                throw new Exception("需转换的集合为空");
            //取出第一个实体的所有Propertie
            var entityType = entitys[0].GetType();
            var entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            var dt = new DataTable();
            for (var i = 0; i < entityProperties.Length; i++)
                dt.Columns.Add(entityProperties[i].Name);
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                    throw new Exception("要转换的集合元素类型不一致");
                var entityValues = new object[entityProperties.Length];
                for (var i = 0; i < entityProperties.Length; i++)
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        ///     得到控制量
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        private IBaseButton[] GetControlButtons(string Station, bool ifEnable = false)
        {
            // 20170629
            IBaseButton[] temp = null;
            if (string.IsNullOrEmpty(Station))
                return temp;
            var fzh = 0;
            if (Station.Length > 3)
                fzh = Convert.ToInt32(Station.Substring(0, 3));
            if (fzh <= 0)
                return temp;
            IList<Jc_DefInfo> Control = DEFServiceModel.QueryPointByInfs(fzh, 3);
            if (null == Control)
                return temp;
            try
            {
                Control = Control.OrderBy(item => item.Kh).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

            temp = new IBaseButton[Control.Count];
            for (var i = 0; i < Control.Count; i++)
            {
                var tempButton = new WindowsUIButton();
                tempButton.Caption = Control[i].Point;
                tempButton.ToolTip = Control[i].Wz;
                tempButton.Tag = Control[i].Bz14; //xuzp20160524
                tempButton.Style = ButtonStyle.CheckButton;
                tempButton.Image = Resources.apply_32x32;
                tempButton.Checked = true; //表示当前都没有控制
                if (ifEnable)
                    tempButton.Enabled = false;
                temp[i] = tempButton;
            }
            return temp;
        }

        /// <summary>
        ///     刷新控制实施值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtimeCotrolReal_Tick(object sender, EventArgs e)
        {
            try
            {
                if (null == CgleStaionAdress.EditValue)
                    return;
                if (string.IsNullOrEmpty(CgleStaionAdress.EditValue.ToString()))
                    return;
                var fzh = 0;
                if (CgleStaionAdress.EditValue.ToString().Length > 3)
                    fzh = Convert.ToInt32(CgleStaionAdress.EditValue.ToString().Substring(0, 3));
                if (fzh <= 0)
                    return;
                IList<Jc_DefInfo> Control = DEFServiceModel.QueryPointByInfs(fzh, 3);
                if (null == Control)
                    return;
                for (var i = 0; i < Control.Count; i++)
                    if (windowsUIButtonPanel1 != null)
                        if (windowsUIButtonPanel1.Buttons != null)
                            for (var j = 0; j < windowsUIButtonPanel1.Buttons.Count; j++)
                                if (windowsUIButtonPanel1.Buttons[j].Properties.Caption == Control[i].Point)
                                    if (Control[i].DataState == 44)
                                        windowsUIButtonPanel1.Buttons[j].Properties.Checked = false; //断开
                                    else
                                        windowsUIButtonPanel1.Buttons[j].Properties.Checked = true; //接通
            }
            catch (Exception ex)
            {
                CtimeCotrolReal.Enabled = false;//出现异常，则停止刷新界面，否则会造成界面卡死  20170701
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        ///     刷新控制实施值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetControlCurrent(string Station)
        {
            try
            {
                if (string.IsNullOrEmpty(Station))
                    return;
                var fzh = 0;
                if (Station.Length > 3)
                    fzh = Convert.ToInt32(Station.Substring(0, 3));
                if (fzh <= 0)
                    return;
                // 20170629
                //已设置成风机口的控制口不允许控制
                var lisDef = DEFServiceModel.QueryPointByFzh(fzh.ToString());
                foreach (IBaseButton item in windowsUIButtonPanel2.Buttons)
                {
                    var def = lisDef.FirstOrDefault(a => a.Point == item.Properties.Caption);
                    if (def == null) continue;
                    var ifUse = RelateUpdate.ControlPointLegal(def);
                    if (ifUse)//如果未定义成甲烷风电闭锁口，则允许进行控制  20170923
                    {
                        item.Properties.Enabled = true;
                    }
                    else//如果是甲烷风电闭锁口，则不允许进行控制  20170923
                    {
                        item.Properties.Enabled = false;
                    }
                }
                IList<Jc_JcsdkzInfo> temp = JCSDKZServiceModel.QueryJCSDKZbyFzhOnlyHCtrlCache(fzh);
                if (null == temp)
                    return;
                for (var i = 0; i < temp.Count; i++)
                    for (var j = 0; j < windowsUIButtonPanel2.Buttons.Count; j++)
                    {
                        if (windowsUIButtonPanel2.Buttons[j].Properties.Caption == temp[i].Bkpoint)
                        {
                            if (temp[i].Type == 0)
                            {
                                windowsUIButtonPanel2.Buttons[j].Properties.Checked = false; //表示当前有控制
                            }
                        }
                    }


            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void CFHandCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Model.JCSDKZServiceModel.SaveData();
                CtimeCotrolReal.Enabled = false;
                CtimeCotrolReal.Dispose();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        ///     保存数据
        /// </summary>
        private void SaveControl(string Station, int type)
        {


            if (string.IsNullOrEmpty(Station))
                return;
            var fzh = 0;
            if (Station.Length > 0)
                fzh = Convert.ToInt32(Station.Substring(0, 3));
            if (fzh <= 0)
                return;
            if (windowsUIButtonPanel2.Buttons.Count <= 0)
                return;
            IList<Jc_JcsdkzInfo> temp;
            Jc_JcsdkzInfo tempControl;

            Jc_DefInfo tempStation;
            tempStation = DEFServiceModel.QueryPointByCodeCache(Station);
            if (null == tempStation)
                return;

            var DelJckzList = new List<Jc_JcsdkzInfo>();
            var AddJckzList = new List<Jc_JcsdkzInfo>();

            for (var i = 0; i < windowsUIButtonPanel2.Buttons.Count; i++)
            {
                temp = new List<Jc_JcsdkzInfo>();

                if (tempStation.Devid == "228")
                {
                    if (null != windowsUIButtonPanel2.Buttons[i].Properties.Tag)
                        temp = JCSDKZServiceModel.QueryJCSDKZbyInf(6666,
                            "3015|" + windowsUIButtonPanel2.Buttons[i].Properties.Tag + "=FF");
                }
                else
                {                   
                    temp = JCSDKZServiceModel.QueryJCSDKZbyInf(type, windowsUIButtonPanel2.Buttons[i].Properties.Caption);
                }

                if (null == temp)
                    continue;

                if (temp.Count > 0) //原来有控制
                {
                    if (windowsUIButtonPanel2.Buttons[i].Properties.Checked) //现在没有控制
                        foreach (var item in temp)
                        {
                            DelJckzList.Add(item); //添加到交叉控制删除列表中   20170323
                            //Basic.CBF.Common.Util.OperateLogHelper.InsertOperateLog(4, "取消控制：主控【" + item.ZkPoint + "】-【" + item.Bkpoint + "】", "");
                            OperateLogHelper.InsertOperateLog(4, "取消控制：主控【" + item.ZkPoint + "】-【" + item.Bkpoint + "】",
                                "");
                        }
                }
                else //原来没有控制
                {
                    if (!windowsUIButtonPanel2.Buttons[i].Properties.Checked) //现在有控制
                    {
                        tempControl = new Jc_JcsdkzInfo();
                        //tempControl.ID = Basic.Framework.Utils.Date.DateTimeUtil.GetDateTimeNowToInt64();//ID
                        tempControl.ID = IdHelper.CreateLongId().ToString(); //ID
                        if (type == 13)
                        {
                            if (windowsUIButtonPanel1.Buttons[i].Properties.Checked)
                            {
                                continue;//只有在设备被控制时，才能进行强制解除控制操作
                            }
                            tempControl.Type = (short)ControlType.RemoveLocalControl; //强制解控
                        }
                        else
                        {
                            tempControl.Type = (short)ControlType.LocalControl; //手动控制
                        }
                        tempControl.ZkPoint = "0000000"; //主控测点
                        tempControl.Bkpoint = windowsUIButtonPanel2.Buttons[i].Properties.Caption; //被控测点
                        tempControl.InfoState = InfoState.AddNew;
                        //Model.JCSDKZServiceModel.AddJC_JCSDKZCache(tempControl);

                        AddJckzList.Add(tempControl); //添加到交叉控制添加列表中   20170323

                        OperateLogHelper.InsertOperateLog(4,
                            "添加控制：主控【" + tempControl.ZkPoint + "】-【" + tempControl.Bkpoint + "】", "");
                    }
                }
            }
            //在最后统一调用服务端进行处理  20170323
            // 20170623
            try
            {
                if (DelJckzList.Count > 0)
                    JCSDKZServiceModel.DoDoJCSDKZ(DelJckzList);
                if (AddJckzList.Count > 0)
                    JCSDKZServiceModel.AddJC_JCSDKZsCache(AddJckzList);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //JCSDKZServiceModel.SaveData(); //执行控制
            barStaticItem2.Caption = "执行成功";
        }

        /// <summary>
        ///     执行控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbarButtonItemExeCute_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (null == CgleStaionAdress.EditValue)
                    XtraMessageBox.Show("请选择分站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SaveControl(CgleStaionAdress.EditValue.ToString(), 0);

                IList<Jc_JcsdkzInfo> tempControl = JCSDKZServiceModel.QueryJCSDKZsCache();
                EvaluateControleDataSource(tempControl);
                if (null != GridSource.GridControlSource)
                    CGridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        /// <summary>
        ///     退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbarButtonItemExist_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.IsRowIndicator && (e.RowHandle >= 0))
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void AddColumnInf()
        {
            try
            {
                //添加gridView栏目
                var tempColomn = new List<GridColumn>();
                gridView.Columns.Clear();
                tempColomn = GridClumnsMrg.ControlGridColumn();
                if (null != tempColomn)
                    for (var i = 0; i < tempColomn.Count; i++)
                        gridView.Columns.Add(tempColomn[i]);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void EvaluateControleDataSource(IList<Jc_JcsdkzInfo> tempControl)
        {
            try
            {
                if (null != tempControl)
                {
                    GridSource.GridControlSource.Clear();
                    ControlInfItem ControlInf;
                    tempControl = tempControl.OrderBy(item => item.Bkpoint).ThenBy(item => item.Type).ToList();
                    foreach (var item in tempControl)
                    {
                        ControlInf = new ControlInfItem();
                        ControlInf.Bkpoint = item.Bkpoint;
                        ControlInf.Zkpoint = item.ZkPoint;
                        //ControlInf.Type = item.Type.ToString();
                        ControlInf.Type = EnumHelper.GetEnumDescription((ControlType)item.Type);
                        GridSource.GridControlSource.Add(ControlInf);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (null == CgleStaionAdress.EditValue)
                    XtraMessageBox.Show("请选择分站", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SaveControl(CgleStaionAdress.EditValue.ToString(), 13);

                IList<Jc_JcsdkzInfo> tempControl = JCSDKZServiceModel.QueryJCSDKZsCache();
                EvaluateControleDataSource(tempControl);
                if (null != GridSource.GridControlSource)
                    CGridControl.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }           
        }
    }
}