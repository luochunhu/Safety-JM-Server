using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuDerail : CuBase
    {
        string _arrPoint = "";
        public CuDerail()
        {
            InitializeComponent();
        }

        public CuDerail(string arrPoint, int devID, uint SourceNum)
            : base(arrPoint, devID, SourceNum)
        {
            _arrPoint = arrPoint;
            InitializeComponent();
        }

        #region =============================加载信息===============================
        /// <summary>
        /// 加载测点的默认信息函数
        /// </summary>
        public override void LoadPretermitInf()
        {
            //逻辑报警
            CcmbLogicAlarmCon.Properties.Items.Add("");
            CcmbLogicAlarmCon.Properties.Items.Add("与");
            CcmbLogicAlarmCon.Properties.Items.Add("或");
            //注释非  20170714
            //CcmbLogicAlarmCon.Properties.Items.Add("非");
            IList<Jc_DefInfo> temp = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, 2); //得到默认的开关量关联测点
            CcmbLogicAlarmPoint.Properties.Items.Add(" ");
            if (temp.Count > 0)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (_arrPoint != temp[i].Point)
                    {
                        CcmbLogicAlarmPoint.Properties.Items.Add(temp[i].Point + "." + temp[i].Wz); //加入默认的测点
                    }
                }
            }
            //报警控制
            CckZeroAlarm.Checked = false;
            CckOneAlarm.Checked = false;
            CckTwoAlarm.Checked = false;

            IList<Jc_DevInfo> DerailType = Model.DEVServiceModel.QueryDevByDevpropertIDCache(2); ;
            if (DerailType.Count > 0)
            {
                for (int i = 0; i < DerailType.Count; i++)
                {
                    if (!CcmbZeroContent.Properties.Items.Contains(DerailType[i].Xs1))
                    {
                        CcmbZeroContent.Properties.Items.Add(DerailType[i].Xs1);//0态显示内容
                    }
                    if (!CcmbOneContent.Properties.Items.Contains(DerailType[i].Xs2))
                    {
                        CcmbOneContent.Properties.Items.Add(DerailType[i].Xs2);//1态显示内容
                    }
                    if (!CcmbTwoContent.Properties.Items.Contains(DerailType[i].Xs3))
                    {
                        CcmbTwoContent.Properties.Items.Add(DerailType[i].Xs3);//2态显示内容
                    }
                }
            }

            //显示颜色

            this.CpZeroColour.Color = Color.Yellow;
            this.CpOneColour.Color = Color.Red;
            this.CpTwoColour.Color = Color.Green;

            //控制Enable
            IList<Jc_DefInfo> tempLocalControl = Model.DEFServiceModel.QueryPointByInfs((int)_SourceNum, 3); //获得所有滴Local控制量
            if (tempLocalControl != null)
            {
                for (int i = 0; i < tempLocalControl.Count; i++)
                {
                    cckLocalControlZero.Properties.Items.Add(tempLocalControl[i].Point);
                    cckLocalControlOne.Properties.Items.Add(tempLocalControl[i].Point);
                    cckLocalControlTwo.Properties.Items.Add(tempLocalControl[i].Point);

                    if (!Model.RelateUpdate.ControlPointLegal(tempLocalControl[i])) //xuzp20151126
                    {
                        cckLocalControlZero.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                        cckLocalControlOne.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                        cckLocalControlTwo.Properties.Items[tempLocalControl[i].Point].Enabled = false;
                    }
                }
            }
            List<string> StationWindBreakControlPoint = Model.RelateUpdate.GetStationWindBreakControlPoint();//获取所有甲烷风电闭锁控制口  20170923
            IList<Jc_DefInfo> tempControl = Model.DEFServiceModel.QueryPointByDevpropertIDCache(3); //获得所有的控制量
            if (null != tempControl) //xuzp20151126
            {
                if (tempControl.Count > 0)
                {
                    try
                    {
                        tempControl = tempControl.OrderBy(item => item.Fzh).ThenBy(item => item.Kh).ToList();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                    List<CrossControlItem> tempControlList = new List<CrossControlItem>();
                    for (int i = 0; i < tempControl.Count; i++)
                    {
                        if (tempControl[i].Fzh != base._SourceNum)
                        {
                            //if (!Model.RelateUpdate.ControlPointLegal(tempControl[i])) //xuzp20151126
                            //{
                            //    continue;
                            //}
                            //不每次从服务端查询，先获取分站对应的风电闭锁控制口，再判断当前是否包含在风电闭锁控制中  20170721
                            if (StationWindBreakControlPoint.Contains(tempControl[i].Point))
                            {
                                continue;
                            }
                            CrossControlItem tempItem = new CrossControlItem();
                            tempItem.ArrPoint = tempControl[i].Point;
                            tempItem.PointName = tempControl[i].Point + ":" + tempControl[i].Wz;
                            tempControlList.Add(tempItem);
                        }
                    }
                    repositoryItemLookUpEdit2.DataSource = tempControlList;
                }
            }
            repositoryItemComboBox2.Items.Add("0态控制");
            repositoryItemComboBox2.Items.Add("1态控制");
            repositoryItemComboBox2.Items.Add("2态控制");

        }
        /// <summary>
        /// 加载测点信息的函数
        /// </summary>
        public override void LoadInf(string arrPoint, int _devID)
        {
            if (string.IsNullOrEmpty(arrPoint))
            {
                return;
            }
            Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
            if (null == temp)
            {
                return;
            }

            LoadPointInfo(temp);

            //#region ==================扩展属性==================
            //if (temp.K4 > 0)
            //{
            //    CcmbLogicAlarmCon.SelectedIndex = temp.K4;//报警逻辑
            //}
            //if (temp.K5 > 0)
            //{
            //    Jc_DefInfo LogicPoint = Model.DEFServiceModel.QueryPointByCodeCache(temp.Fzh.ToString().PadLeft(3, '0') + "D" + temp.K5.ToString().PadLeft(2, '0') + temp.K6.ToString());
            //    if (null != LogicPoint)
            //    {
            //        CcmbLogicAlarmPoint.SelectedItem = LogicPoint.Point + "." + LogicPoint.Wz;
            //    }
            //}
            //CtxbDesc.Text = temp.Remark;//描述

            //#endregion

            //#region ==================报警控制==================

            //Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);

            //if ((temp.K8 & 0x01) == 1) //0态度是否报警
            //{
            //    CckZeroAlarm.Checked = true;
            //}
            //else
            //{
            //    CckZeroAlarm.Checked = false;
            //}

            //if ((temp.K8 & 0x02) == 2)  //1态度是否报警
            //{
            //    CckOneAlarm.Checked = true;
            //}
            //else
            //{
            //    CckOneAlarm.Checked = false;
            //}

            //if ((temp.K8 & 0x04) == 4) //2态度是否报警
            //{
            //    CckTwoAlarm.Checked = true;
            //}
            //else
            //{
            //    CckTwoAlarm.Checked = false;
            //}
            ////颜色显示
            //try
            //{
            //    this.CpZeroColour.Color = Color.FromArgb(int.Parse(temp.Bz9.ToString()));
            //    this.CpOneColour.Color = Color.FromArgb(int.Parse(temp.Bz10.ToString()));
            //    this.CpTwoColour.Color = Color.FromArgb(int.Parse(temp.Bz11.ToString()));
            //}
            //catch
            //{ }
            ////显示内容
            //this.CcmbZeroContent.Text = temp.Bz6;
            //this.CcmbOneContent.Text = temp.Bz7;
            //this.CcmbTwoContent.Text = temp.Bz8;
            ////各态控制口
            //cckLocalControlZero.Text = SetLocalControlText(temp.K1);
            //cckLocalControlOne.Text = SetLocalControlText(temp.K2);
            //cckLocalControlTwo.Text = SetLocalControlText(temp.K3);

            //#endregion

            //#region ==================交叉控制==================
            //getCrossInf(arrPoint);
            //CrossControlDT = ToDataTable(CrossControlList);
            //CdgControl.DataSource = CrossControlDT;
            //CdgControl.RefreshDataSource();
            //#endregion


        }
        /// <summary>
        /// 交叉控制数据源
        /// </summary>
        private List<CrossControlItem> CrossControlList = new List<CrossControlItem>();
        /// <summary>
        /// 数据源DT
        /// </summary>
        private DataTable CrossControlDT = new DataTable();
        /// <summary>
        /// 设置交叉控制信息
        /// </summary>
        /// <param name="arrPoint"></param>
        /// <returns></returns>
        private void getCrossInf(string arrPoint)
        {
            try
            {
                Jc_DefInfo temp = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
                if (temp == null)
                {
                    return;
                }
                CrossControlList.Clear();
                string[] ControlPoints;
                Jc_DefInfo tempPoint;
                CrossControlItem tempCrossItem;
                if (!string.IsNullOrEmpty(temp.Jckz1))
                {
                    ControlPoints = temp.Jckz1.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "0态控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(temp.Jckz2))
                {
                    ControlPoints = temp.Jckz2.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "1态控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(temp.Jckz3))
                {
                    ControlPoints = temp.Jckz3.Split('|');
                    if (ControlPoints.Length > 0)
                    {
                        for (int i = 0; i < ControlPoints.Length; i++)
                        {
                            tempCrossItem = new CrossControlItem();
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ControlPoints[i]);
                            if (null != tempPoint)
                            {
                                tempCrossItem.ControlType = "2态控制";
                                tempCrossItem.ArrPoint = tempPoint.Point;
                                tempCrossItem.PointName = tempPoint.Point + ":" + tempPoint.Wz;

                                if (tempPoint.K1 > 0 & tempPoint.K2 > 0)
                                {
                                    tempCrossItem.FeedBackPointName = tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString() + "." + Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString()).Wz;
                                }
                                CrossControlList.Add(tempCrossItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获得交叉控制信息【getCrossInf】", ex);
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
        public static List<CrossControlItem> ToList(DataTable dt)
        {
            List<CrossControlItem> ret = new List<CrossControlItem>();
            if (null != dt)
            {
                CrossControlItem temp;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    temp = new CrossControlItem();
                    temp.ArrPoint = dt.Rows[i]["ArrPoint"].ToString();
                    temp.ControlType = dt.Rows[i]["ControlType"].ToString();
                    temp.FeedBackPointName = dt.Rows[i]["FeedBackPointName"].ToString();
                    temp.DelInfBtnStr = "删除";
                    ret.Add(temp);
                }
            }
            return ret;
        }
        #endregion

        #region =============================确认信息===============================


        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DefInfo point)
        {
            if (!SensorInfoverify())
            {
                return false;
            }

            Jc_DevInfo temp = new Jc_DevInfo();
            point.Bz9 = CpZeroColour.Color.ToArgb().ToString(); //0态显示颜色
            point.Bz10 = CpOneColour.Color.ToArgb().ToString();//1态度显示颜色
            point.Bz11 = CpTwoColour.Color.ToArgb().ToString();//2态显示颜色

            point.K8 = Convert.ToByte(CckZeroAlarm.Checked ? 1 : 0);//0态是否报警
            point.K8 |= Convert.ToByte((CckOneAlarm.Checked ? 1 : 0) << 1);//1态是否报警
            point.K8 |= Convert.ToByte((CckTwoAlarm.Checked ? 1 : 0) << 2);//2态度是否报警

            point.Bz6 = CcmbZeroContent.Text;//0态显示内容
            point.Bz7 = CcmbOneContent.Text;//1态显示内容
            point.Bz8 = CcmbTwoContent.Text;//2态显示内容

            point.K1 = ConfirmCheckBoxValue(cckLocalControlZero.Text);//0态控制信息
            point.K2 = ConfirmCheckBoxValue(cckLocalControlOne.Text);//1态控制信息
            point.K3 = ConfirmCheckBoxValue(cckLocalControlTwo.Text);//2态控制信息
            //先赋默认值，如果未设置，则表示清除  20170629
            point.K4 = 0;
            point.K5 = 0;
            point.K6 = 0;
            if (!string.IsNullOrEmpty(CcmbLogicAlarmCon.Text.Trim()))
            {
                point.K4 = Convert.ToByte(CcmbLogicAlarmCon.SelectedIndex);
                point.K5 = Convert.ToByte(CcmbLogicAlarmPoint.Text.Substring(0, CcmbLogicAlarmPoint.Text.IndexOf('.')).Substring(4, 2));              
                if (CcmbLogicAlarmPoint.Text.Substring(0, CcmbLogicAlarmPoint.Text.IndexOf('.')).Length > 6)
                {
                    point.K6 = Convert.ToByte(CcmbLogicAlarmPoint.Text.Substring(0, CcmbLogicAlarmPoint.Text.IndexOf('.')).Substring(6));
                }

            }
            #region ====================交叉控制====================
            point.Jckz1 = "";
            point.Jckz2 = "";
            point.Jckz3 = "";
            List<CrossControlItem> listTemp = new List<CrossControlItem>();
            listTemp = ToList(CrossControlDT); //将数据源DT转换成List
            if (listTemp != null)
            {
                if (listTemp.Count > 0)
                {
                    Jc_DefInfo PointTemp;
                    for (int i = 0; i < listTemp.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(listTemp[i].ArrPoint))
                        {
                            PointTemp = Model.DEFServiceModel.QueryPointByCodeCache(listTemp[i].ArrPoint);
                            if (!string.IsNullOrEmpty(PointTemp.Point))
                            {
                                if (listTemp[i].ControlType == "0态控制")
                                {
                                    point.Jckz1 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else if (listTemp[i].ControlType == "1态控制")
                                {
                                    point.Jckz2 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else if (listTemp[i].ControlType == "2态控制")
                                {
                                    point.Jckz3 += PointTemp.Fzh.ToString().PadLeft(3, '0') + "C" + PointTemp.Kh.ToString().PadLeft(2, '0')
                                        + PointTemp.Dzh.ToString() + "|";
                                }
                                else
                                {
                                    XtraMessageBox.Show("交叉控制中存在控制类型未选择的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("交叉控制中存在受控对象未选择的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                    //处理完成后去掉最后一根“|”
                    if (point.Jckz1 != null)
                    {
                        if (point.Jckz1.Length > 0)
                        {
                            point.Jckz1 = point.Jckz1.Substring(0, point.Jckz1.Length - 1);
                        }
                    }
                    if (point.Jckz2 != null)
                    {
                        if (point.Jckz2.Length > 0)
                        {
                            point.Jckz2 = point.Jckz2.Substring(0, point.Jckz2.Length - 1);
                        }
                    }
                    if (point.Jckz3 != null)
                    {
                        if (point.Jckz3.Length > 0)
                        {
                            point.Jckz3 = point.Jckz3.Substring(0, point.Jckz3.Length - 1);
                        }
                    }
                }
            }
            #endregion

            point.Remark = CtxbDesc.Text;

            return true;
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        public override bool SensorInfoverify()
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(CcmbLogicAlarmCon.Text.Trim()))
            {
                if (string.IsNullOrEmpty(CcmbLogicAlarmPoint.Text))
                {
                    XtraMessageBox.Show("请选择关联测点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
            }
            if (string.IsNullOrEmpty(CcmbZeroContent.Text))
            {
                XtraMessageBox.Show("请选择0态显示内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbOneContent.Text))
            {
                XtraMessageBox.Show("请选择1态显示内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbTwoContent.Text))
            {
                XtraMessageBox.Show("请选择2态显示内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }

            ret = true;
            return ret;
        }

        #endregion

        #region  =============================触发事件===============================
        /// <summary>
        /// 设备类型变化产生的影响
        /// </summary>
        public override void DevTypeChanngeEvent(long DevID, Jc_DefInfo Point)
        {
            try
            {
                if (Point != null)
                {
                    CcmbLogicAlarmCon.Text = "";
                    CcmbLogicAlarmPoint.Text = "";
                    LoadPointInfo(Point);
                }
                else
                {//取jc_dev的定义信息
                    CcmbLogicAlarmCon.Text = "";
                    CcmbLogicAlarmPoint.Text = "";
                    CtxbDesc.Text = "";

                    //各态控制口
                    cckLocalControlZero.Text = "";
                    cckLocalControlOne.Text = "";
                    cckLocalControlTwo.Text = "";

                    #region ==================交叉控制==================
                    CrossControlList.Clear();
                    CrossControlDT = ToDataTable(CrossControlList);
                    CdgControl.DataSource = CrossControlDT;
                    CdgControl.RefreshDataSource();
                    #endregion


                    Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());
                    if (null == temp)
                    {
                        return;
                    }

                    #region ==================报警属性==================
                    if (temp.Pl1 == 1)
                    {
                        CckZeroAlarm.Checked = true;
                    }
                    else
                    {
                        CckZeroAlarm.Checked = false;
                    }
                    if (temp.Pl2 == 1)
                    {
                        CckOneAlarm.Checked = true;
                    }
                    else
                    {
                        CckOneAlarm.Checked = false;
                    }
                    if (temp.Pl3 == 1)
                    {
                        CckTwoAlarm.Checked = true;
                    }
                    else
                    {
                        CckTwoAlarm.Checked = false;
                    }

                    //显示内容
                    this.CcmbZeroContent.Text = temp.Xs1;
                    this.CcmbOneContent.Text = temp.Xs2;
                    this.CcmbTwoContent.Text = temp.Xs3;
                    //颜色显示
                    this.CpZeroColour.Color = Color.FromArgb(temp.Color1);
                    this.CpOneColour.Color = Color.FromArgb(temp.Color2);
                    this.CpTwoColour.Color = Color.FromArgb(temp.Color3);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

        }

        private void LoadPointInfo(Jc_DefInfo Point)
        {
            #region ==================扩展属性==================
            if (Point.K4 > 0)
            {
                CcmbLogicAlarmCon.SelectedIndex = Point.K4;//报警逻辑
            }
            if (Point.K5 > 0)
            {
                Jc_DefInfo LogicPoint = Model.DEFServiceModel.QueryPointByCodeCache(Point.Fzh.ToString().PadLeft(3, '0') + "D" + Point.K5.ToString().PadLeft(2, '0') + Point.K6.ToString());
                if (null != LogicPoint)
                {
                    CcmbLogicAlarmPoint.SelectedItem = LogicPoint.Point + "." + LogicPoint.Wz;
                }
            }
            CtxbDesc.Text = Point.Remark;//描述
            #endregion

            ////各态控制口
            //cckLocalControlZero.Text = SetLocalControlText(Point.K1);
            //cckLocalControlOne.Text = SetLocalControlText(Point.K2);
            //cckLocalControlTwo.Text = SetLocalControlText(Point.K3);
            #region ==================报警控制==================

            Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(Point.Devid);

            if ((Point.K8 & 0x01) == 1) //0态度是否报警
            {
                CckZeroAlarm.Checked = true;
            }
            else
            {
                CckZeroAlarm.Checked = false;
            }

            if ((Point.K8 & 0x02) == 2)  //1态度是否报警
            {
                CckOneAlarm.Checked = true;
            }
            else
            {
                CckOneAlarm.Checked = false;
            }

            if ((Point.K8 & 0x04) == 4) //2态度是否报警
            {
                CckTwoAlarm.Checked = true;
            }
            else
            {
                CckTwoAlarm.Checked = false;
            }
            //颜色显示
            try
            {
                this.CpZeroColour.Color = Color.FromArgb(int.Parse(Point.Bz9.ToString()));
                this.CpOneColour.Color = Color.FromArgb(int.Parse(Point.Bz10.ToString()));
                this.CpTwoColour.Color = Color.FromArgb(int.Parse(Point.Bz11.ToString()));
            }
            catch
            { }
            //显示内容
            this.CcmbZeroContent.Text = Point.Bz6;
            this.CcmbOneContent.Text = Point.Bz7;
            this.CcmbTwoContent.Text = Point.Bz8;
            //各态控制口
            cckLocalControlZero.Text = SetLocalControlText(Point.K1);
            cckLocalControlOne.Text = SetLocalControlText(Point.K2);
            cckLocalControlTwo.Text = SetLocalControlText(Point.K3);

            #endregion

            #region ==================交叉控制==================
            getCrossInf(Point.Point);
            CrossControlDT = ToDataTable(CrossControlList);
            CdgControl.DataSource = CrossControlDT;
            CdgControl.RefreshDataSource();
            #endregion
        }

        private void CcmbZeroColour_Click(object sender, EventArgs e)
        {

        }

        private void CcmbOneColour_Click(object sender, EventArgs e)
        {

        }

        private void CcmbTwoColour_Click(object sender, EventArgs e)
        {

        }

        private void CckZeroAlarm_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CdvCrossControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.CdvCrossControl.FocusedRowHandle >= 0)
            {
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CdvCrossControl.DeleteRow(CdvCrossControl.FocusedRowHandle);
                }
            }
        }

        private void CdvCrossControl_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (null == e.Value)
            {
                return;
            }
            if (e.Column.FieldName != "ArrPoint")
            {
                return;
            }
            Jc_DefInfo tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(e.Value.ToString());
            //e.Value.ToString().Substring(0, CdvCrossControl.FocusedValue.ToString().IndexOf('.'))
            if (tempPoint == null)
            {
                return;
            }
            if (tempPoint.K1 > 0 && tempPoint.K2 > 0)
            {
                Jc_DefInfo tempFeedBackPoint = Model.DEFServiceModel.QueryPointByCodeCache(tempPoint.K1.ToString().PadLeft(3, '0') + "D" + tempPoint.K2.ToString().PadLeft(2, '0') + tempPoint.K4.ToString());
                if (tempFeedBackPoint == null)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(tempFeedBackPoint.Point))
                {
                    CdvCrossControl.SetRowCellValue(e.RowHandle, "FeedBackPointName", tempFeedBackPoint.Point + "." + tempFeedBackPoint.Wz); //自动生成反馈测点
                }
            }


        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "删除确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CdvCrossControl.DeleteRow(CdvCrossControl.FocusedRowHandle);
            }
        }
        private void CdvCrossControl_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = e.RowHandle.ToString();
            }
        }

        private void CdvCrossControl_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            for (int i = 0; i < CdvCrossControl.RowCount; i++)
            {
                if (e.RowHandle != i || e.RowHandle == -2147483647)
                {
                    if (CdvCrossControl.GetRowCellValue(i, "ArrPoint").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "ArrPoint").ToString() && CdvCrossControl.GetRowCellValue(i, "ControlType").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "ControlType").ToString() && CdvCrossControl.GetRowCellValue(i, "FeedBackPointName").ToString() == CdvCrossControl.GetRowCellValue(e.RowHandle, "FeedBackPointName").ToString())
                    {
                        XtraMessageBox.Show("存在重复定义！", "消息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Valid = false;
                        return;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// 设置本地控制信息
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private string SetLocalControlText(int K)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {

                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";

                        }
                        else
                        {
                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1" + ",";
                        }
                    }
                }
                if (temp.Contains(","))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            return temp;
        }
        /// <summary>
        /// 获取控制信息
        /// </summary>
        /// <param name="LocalControlText"></param>
        /// <returns></returns>
        private int ConfirmCheckBoxValue(string LocalControlText)
        {
            int temp = 0;
            if (string.IsNullOrEmpty(LocalControlText))
            {
                return temp;
            }
            string[] tempArray = LocalControlText.Split(',');
            if (tempArray.Length <= 0)
            {
                return temp;
            }
            int Channel = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                Channel = 0;
                if (tempArray[i].Trim().Length >= 6)
                {
                    if (tempArray[i].Trim().Length == 6)
                    {
                        Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2));
                    }
                    else
                    {
                        if (tempArray[i].Trim()[6] == '0')
                        {
                            Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2));
                        }
                        else
                        {
                            Channel = Convert.ToInt32(tempArray[i].Trim().Substring(4, 2)) + 8;
                        }
                    }

                }
                if (Channel > 0)
                {
                    temp += 1 << (Channel - 1);
                }
            }
            return temp;
        }
    }
}
