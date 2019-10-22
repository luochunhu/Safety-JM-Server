using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;


namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CuControl : CuBase
    {
        /// <summary>
        /// 当前添加或者编辑的测点
        /// </summary>
        private Jc_DefInfo EditPoint = null;
        public CuControl()
        {
            InitializeComponent();
        }
        public CuControl(string arrPoint, int devID, uint SourceNum)
            : base(arrPoint, devID, SourceNum)
        {           
            InitializeComponent();
        }

        #region =============================加载信息===============================
        /// <summary>
        /// 加载测点的默认信息函数
        /// </summary>
        public override void LoadPretermitInf()
        {
            IList<Jc_DefInfo> temp = Model.DEFServiceModel.QueryPointByDevpropertIDCache(2); //得到所有的开关量关联测点
            CCmbFeedBackPoint.Properties.Items.Add(" ");
            if (temp.Count > 0)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (base._SourceNum > 0)
                    {
                        if (temp[i].Fzh != _SourceNum)
                        {
                            continue;
                        }
                    }
                    CCmbFeedBackPoint.Properties.Items.Add(temp[i].Point + "." + temp[i].Wz); //加入所有的开关量关联测点
                }
            }
            //报警
            CckZeroAlarm.Checked = false;
            CckOneAlarm.Checked = false;

            IList<Jc_DevInfo> ControlType = Model.DEVServiceModel.QueryDevByDevpropertIDCache(3);
            if (ControlType != null)
            {
                for (int i = 0; i < ControlType.Count; i++)
                {
                    if (!CcmbZeroContent.Properties.Items.Contains(ControlType[i].Xs1))
                    {
                        CcmbZeroContent.Properties.Items.Add(ControlType[i].Xs1);
                    }
                    if (!CcmbOneContent.Properties.Items.Contains(ControlType[i].Xs2))
                    {
                        CcmbOneContent.Properties.Items.Add(ControlType[i].Xs2);
                    }
                }
            }

            //显示颜色

            this.CpZeroColour.Color = Color.Green;
            this.CpOneColour.Color = Color.Red;

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
            Jc_DevInfo tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.Devid);
            if (null == tempDev)
            {
                return;
            }
            LoadPointInfo(temp);
            //#region ==================扩展属性==================
            //if (temp.K1 > 0 && temp.K2 > 0)
            //{
            //    Jc_DefInfo FeedBackePoint = Model.DEFServiceModel.QueryPointByCodeCache(temp.K1.ToString().PadLeft(3, '0') + "D" + temp.K2.ToString().PadLeft(2, '0') + temp.K4.ToString());

            //    if (null != FeedBackePoint)
            //    {
            //        CCmbFeedBackPoint.SelectedItem = FeedBackePoint.Point + "." + FeedBackePoint.Wz;
            //    }
            //}

            //CtxbDesc.Text = temp.Remark;//描述

            //#endregion

            //#region ==================报警==================
            //if ((temp .K8 &0x01) == 1) //0态度是否报警
            //{
            //    CckZeroAlarm.Checked = true;
            //}
            //else
            //{
            //    CckZeroAlarm.Checked = false;
            //}

            //if ((temp .K8 &0x02) == 2)  //1态度是否报警
            //{
            //    CckOneAlarm.Checked = true;
            //}
            //else
            //{
            //    CckOneAlarm.Checked = false;
            //}
            ////颜色显示
            //try 
            //{
            //this.CpZeroColour.Color = Color.FromArgb(int.Parse (temp .Bz9.ToString ()));
            //this.CpOneColour.Color = Color.FromArgb(int.Parse (temp .Bz10.ToString ()));
            //}
            //catch 
            //{}

            ////显示内容
            //this.CcmbZeroContent.Text = temp .Bz6;
            //this.CcmbOneContent.Text = temp .Bz7 ;
            //#endregion

        }
        #endregion

        #region =============================确认信息===============================


        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DefInfo point)
        {
            EditPoint = point;

            if (!SensorInfoverify())
            {
                return false;
            }

            Jc_DevInfo temp = new Jc_DevInfo();
            point.Bz9 = CpZeroColour.Color.ToArgb().ToString(); //0态显示颜色
            point.Bz10 = CpOneColour.Color.ToArgb().ToString();//1态度显示颜色

            point.K8 = Convert.ToByte(CckZeroAlarm.Checked ? 1 : 0);//0态是否报警
            point.K8 |= Convert.ToByte((CckOneAlarm.Checked ? 1 : 0) << 1);//1态是否报警

            point.Bz6 = CcmbZeroContent.Text;//0态显示内容
            point.Bz7 = CcmbOneContent.Text;//1态显示内容
            if (!string.IsNullOrEmpty(CCmbFeedBackPoint.Text.Trim()))
            {
                point.K1 = Convert.ToInt32(CCmbFeedBackPoint.Text.Substring(0, 3)); //馈电分站号
                point.K2 = Convert.ToInt32(CCmbFeedBackPoint.Text.Substring(4, 2)); //馈电通道号
                if (CCmbFeedBackPoint.Text.Substring(0, CCmbFeedBackPoint.Text.IndexOf(".")).Length > 6)
                {
                    point.K4 = Convert.ToInt32(CCmbFeedBackPoint.Text.Substring(0, CCmbFeedBackPoint.Text.IndexOf(".")).Substring(6)); //馈电地址号
                }
            }
            else
            {
                point.K1 = 0;
                point.K2 = 0;
                point.K4 = 0;
            }
            point.Remark = CtxbDesc.Text;
            return true;
        }

        /// <summary>
        /// 有效性验证
        /// </summary>
        public override bool SensorInfoverify()
        {
            bool ret = false;

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
            if (EditPoint != null)
            {
                if (EditPoint.DevPropertyID == 3 && EditPoint.Devid == "45")//判断控制状态，须绑定馈电传感器  20171219
                {
                    if (string.IsNullOrEmpty(CCmbFeedBackPoint.Text.Trim()))
                    {
                        //if (XtraMessageBox.Show("当前控制量未绑定馈电传感器，确定要继续吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //{
                        //    return true;
                        //}
                        //else
                        //{
                        //    return false;
                        //}

                        XtraMessageBox.Show("当前控制量未绑定馈电传感器!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return ret;
                    }
                }
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
            //if (Point!=null)
            //{
            //    CCmbFeedBackPoint.Text = "";
            //    if (Point.K1 > 0 && Point.K2 > 0)
            //    {
            //        Jc_DefInfo FeedBackePoint = Model.DEFServiceModel.QueryPointByCodeCache(Point.K1.ToString().PadLeft(3, '0') + "D" + Point.K2.ToString().PadLeft(2, '0') + Point.K4.ToString());

            //        if (null != FeedBackePoint)
            //        {
            //            CCmbFeedBackPoint.SelectedItem = FeedBackePoint.Point + "." + FeedBackePoint.Wz;
            //        }
            //    }
            //    CtxbDesc.Text = Point.Remark;//描述
            //}
            //else
            //{
            //    CCmbFeedBackPoint.Text = "";
            //    CtxbDesc.Text = "";//描述
            //}
            //Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());

            //if (null == temp)
            //{
            //    return;
            //}
            //#region ==================报警属性==================
            //if (temp.Pl1==1)
            //{
            //    CckZeroAlarm.Checked = true;
            //}
            //else
            //{
            //    CckZeroAlarm.Checked = false;
            //}
            //if (temp.Pl2==1)
            //{
            //    CckOneAlarm.Checked = true;
            //}
            //else
            //{
            //    CckOneAlarm.Checked = false;
            //}


            ////显示内容
            //this.CcmbZeroContent.Text = temp.Xs1;
            //this.CcmbOneContent.Text = temp.Xs2;
            ////颜色显示
            //this.CpZeroColour.Color = Color.FromArgb(temp.Color1);
            //this.CpOneColour.Color = Color.FromArgb(temp.Color2);
            //#endregion
            try
            {
                if (Point != null)
                {
                    LoadPointInfo(Point);
                }
                else
                {//取jc_dev的定义信息
                    CCmbFeedBackPoint.SelectedItem = "";
                    CtxbDesc.Text = "";

                    Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID.ToString());
                    if (null == temp)
                    {
                        return;
                    }
                    #region ==================报警==================
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
                    //颜色显示

                    this.CpZeroColour.Color = Color.FromArgb(int.Parse(temp.Color1.ToString()));
                    this.CpOneColour.Color = Color.FromArgb(int.Parse(temp.Color2.ToString()));


                    //显示内容
                    this.CcmbZeroContent.Text = temp.Xs1;
                    this.CcmbOneContent.Text = temp.Xs2;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void LoadPointInfo(Jc_DefInfo temp)
        {

            #region ==================扩展属性==================

            if (temp.K1 > 0 && temp.K2 > 0)
            {
                Jc_DefInfo FeedBackePoint = Model.DEFServiceModel.QueryPointByCodeCache(temp.K1.ToString().PadLeft(3, '0') + "D" + temp.K2.ToString().PadLeft(2, '0') + temp.K4.ToString());

                if (null != FeedBackePoint)
                {
                    CCmbFeedBackPoint.SelectedItem = FeedBackePoint.Point + "." + FeedBackePoint.Wz;
                }
            }

            CtxbDesc.Text = temp.Remark;//描述

            #endregion

            #region ==================报警==================
            if ((temp.K8 & 0x01) == 1) //0态度是否报警
            {
                CckZeroAlarm.Checked = true;
            }
            else
            {
                CckZeroAlarm.Checked = false;
            }

            if ((temp.K8 & 0x02) == 2)  //1态度是否报警
            {
                CckOneAlarm.Checked = true;
            }
            else
            {
                CckOneAlarm.Checked = false;
            }
            //颜色显示
            try
            {
                this.CpZeroColour.Color = Color.FromArgb(int.Parse(temp.Bz9.ToString()));
                this.CpOneColour.Color = Color.FromArgb(int.Parse(temp.Bz10.ToString()));
            }
            catch
            { }

            //显示内容
            this.CcmbZeroContent.Text = temp.Bz6;
            this.CcmbOneContent.Text = temp.Bz7;
            #endregion
        }
        #endregion
    }
}
