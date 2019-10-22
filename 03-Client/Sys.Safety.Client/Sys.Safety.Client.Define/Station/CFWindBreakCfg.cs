using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using Sys.Safety.Client.Define.Model;
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Define.Station
{
    //<summary>
    //huangxxUP 2013-12-28
    //检查人：
    //</summary>
    public partial class CFWindBreakCfg : XtraForm
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CFWindBreakCfg(Jc_DefInfo _tempStation, CFStation _tempStationForm, string _WindBreakBytes, string _WindBreakCondition)
        {
            tempStation = _tempStation;
            tempStationForm = _tempStationForm;

            if (!_WindBreakCondition.Contains("&"))//包含&表示是新风电闭锁的配置
            {
                WindBreakCondition = _WindBreakCondition;
                WindBreakBytes = _WindBreakBytes;
            }
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();

        }

        #region ==========================变量管理 ==========================
        //<summary>
        //当前的分站窗体
        //</summary>
        private CFStation tempStationForm;
        /// <summary>
        /// 风电闭锁字节
        /// </summary>
        private string WindBreakBytes;

        /// <summary>
        /// 风电闭锁条件
        /// </summary>
        private string WindBreakCondition;
        /// <summary>
        /// 分站对象
        /// </summary>
        private Jc_DefInfo tempStation = null;
        /// <summary>
        /// 分站对象设备
        /// </summary>
        private Jc_DevInfo tempStationDev = null;
        //<summary>
        //是否为测点类型发生改变
        //</summary>
        public bool LxChange;
        /// <summary>
        ///模拟量数组
        /// </summary>
        private string[] Mnl_array = null;
        /// <summary>
        /// 开关量数组
        /// </summary>
        private string[] Kgl_array = null;
        /// <summary>
        ///模开量数组
        /// </summary>
        private string[] MnKg_array = null;
        //<summary>
        //存储开关量各态显示 string[]数长度为3 分别存储0 1 2 态 by tanxingyan 2014-12-11
        //</summary>
        Dictionary<string, string[]> kglxs = new Dictionary<string, string[]>();
        /// <summary>
        /// 控制口(本控和交叉控) by huangxxUP 2013-12-28
        /// </summary>
        byte mkkzk = 0;
        /// <summary>
        /// 瓦斯控制口 by huangxxUP 2013-12-28
        /// </summary>
        byte MasCheck_ws = 0;
        /// <summary>
        /// 风电控制口 by huangxxUP 2013-12-28
        /// </summary>
        byte MasCheck_fd = 0;
        #endregion

        #region  ==========================事件相关 ==========================
        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CFWindBreakCfg_Load(object sender, EventArgs e)
        {
            if (null == tempStation)
            {
                XtraMessageBox.Show("分站未定义");
                return;
            }
            tempStationDev = Model.DEVServiceModel.QueryDevByDevIDCache(tempStation.Devid);
            //临时设备
            Jc_DevInfo tempDev = null;
            //临时测点
            Jc_DefInfo TempPoint = null;

            #region 开关量逻辑条件初始化 by  huangxxUP 2015-02-03
            fd_logic_1.Text = "或";
            fd_logic_2.Text = "或";
            #endregion


            //加载时调用手动控制赋值到内存 by huangxxUP 2013-12-28
            //Swap.sqlc.GetSdkzzt();

            fd_logic_4.SelectedIndex = 0;



            #region ---加载界面模拟量/开关量信息---
            try
            {
                #region ---加载控制默认信息---
                DefaultControlValueLoad();
                #endregion

                List<string> Mnl = new List<string>();  //保存模拟量测点
                List<string> Kgl = new List<string>();  //保存开关量测点
                List<string> MnKg = new List<string>(); //保存所有测点

                rb_one.Checked = true;
                lab_fz.Text = tempStation.Fzh.ToString() + "." + tempStation.Wz;
                ComBoxClear();


                IList<Jc_DefInfo> TempAnalog = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 1);
                if (TempAnalog != null)
                {
                    if (TempAnalog.Count > 0)
                    {
                        foreach (var item in TempAnalog)
                        {
                            if (item.Dzh < 1)
                            { //不支持多参数  20170428
                                //Mnl.Add(item.Point);
                                //MnKg.Add(item.Point);
                                //2017.12.20 by
                                if (item.DevClassID == 1)
                                {
                                    Mnl.Add(item.Point + "." + item.Wz + "【" + item.DevName + "】");
                                }
                                MnKg.Add(item.Point + "." + item.Wz + "【" + item.DevName + "】");
                            }
                        }
                    }
                }

                IList<Jc_DefInfo> TempDerail = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 2);

                string[] DerailStateView;  // by tanxingyan 2014-12-11
                if (TempDerail != null)
                {
                    if (TempDerail.Count > 0)
                    {
                        foreach (var item in TempDerail)
                        {
                            if (item.Dzh < 1)
                            { //不支持多参数  20170428
                                //Kgl.Add(item.Point);
                                //MnKg.Add(item.Point);
                                //2017.12.20 by
                                Kgl.Add(item.Point + "." + item.Wz + "【" + item.DevName + "】");
                                MnKg.Add(item.Point + "." + item.Wz + "【" + item.DevName + "】");
                            }

                            #region 加载开关量及0 1 2态显示  by tanxingyan 2014-12-11
                            tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(item.Devid);
                            if (tempDev != null)
                            {
                                if (tempDev != null)
                                {
                                    DerailStateView = new string[3];
                                    DerailStateView[0] = tempDev.Xs1;
                                    DerailStateView[1] = tempDev.Xs2;
                                    DerailStateView[2] = tempDev.Xs3;
                                    kglxs.Add(item.Point, DerailStateView);
                                }
                            }
                            #endregion
                        }
                    }
                }
                PointLoad(Mnl, ref Mnl_array);
                PointLoad(Kgl, ref Kgl_array);
                PointLoad(MnKg, ref MnKg_array);

                ComBoxInit();


                Mnl.Clear();
                Kgl.Clear();
                MnKg.Clear();
            }
            catch (Exception err)
            {
                LogHelper.Error(err.Message);
            }
            #endregion

            #region ---风电闭锁加载---

            try
            {
                long temp = -1;
                long value = 0;
                int index = 0;
                long devid = -1;
                int t1, t2, kt1, kt1b, kt3, kt3b, ftfl, ftflb;
                int ftfl_cmd = 0;
                int ftflb_cmd = 0;
                string fjlx = "";  // xuzpUP 20150116

                if (!string.IsNullOrEmpty(WindBreakBytes))
                {
                    string[] fdbs = WindBreakBytes.Split(',');
                    if (fdbs.Length > 0)
                    {
                        #region ---读取风机类型,单/双风机  // xuzpUP 20150116---
                        fjlx = fdbs[index++];
                        if (fjlx == "1")
                        {
                            rb_one.Checked = true;
                        }
                        else if (fjlx == "2")
                        {
                            rb_two.Checked = true;
                        }
                        else
                        {
                            rb_three.Checked = true;
                        }
                        #endregion

                        UInt16 ckws = Convert.ToByte(fdbs[index++]);
                        ckws |= (UInt16)(Convert.ToByte(fdbs[fdbs.Length - 1]) << 8);

                        #region ---读取风电闭锁开关量解锁逻辑条件---

                        if ((Convert.ToInt32(fdbs[index]) & 0x01) == 0x01)
                        {
                            fd_logic_1.Text = "与";
                        }
                        else
                        {
                            fd_logic_1.Text = "或";
                        }

                        if (((Convert.ToInt32(fdbs[index]) >> 2) & 0x01) == 0x01)
                        {
                            fd_logic_2.Text = "与";
                        }
                        else
                        {
                            fd_logic_2.Text = "或";
                        }
                        index += 1;

                        #endregion

                        #region ---读取风电闭锁相关口号信息---
                        IList<Jc_DefInfo> tempPointItems = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 2);
                        t1 = Convert.ToByte(fdbs[index++]) & 0x1F;
                        if (t1 > 0)
                        {
                            //cmb_t1.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + t1.ToString().PadLeft(2, '0') + "0";              //口1,取低5位口号
                            //多参数支持  20170428
                            for (int i = 0; i < cmb_t1.Items.Count; i++)
                            {
                                if (cmb_t1.Items[i].ToString().Contains(tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + t1.ToString().PadLeft(2, '0')))
                                {
                                    cmb_t1.SelectedIndex = i;
                                }
                            }
                        }

                        t2 = Convert.ToByte(fdbs[index++]) & 0x1F;
                        if (t2 > 0)
                        {
                            //cmb_t2.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + t2.ToString().PadLeft(2, '0') + "0";              //口2,取低5位口号
                            //多参数支持  20170428
                            for (int i = 0; i < cmb_t2.Items.Count; i++)
                            {
                                if (cmb_t2.Items[i].ToString().Contains(tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + t2.ToString().PadLeft(2, '0')))
                                {
                                    cmb_t2.SelectedIndex = i;
                                }
                            }
                        }

                        ftfl = Convert.ToByte(fdbs[index]) & 0x1F;
                        if (ftfl > 0)
                        {
                            ws_ftfl_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + ftfl.ToString().PadLeft(2, '0') + "0";    //口3,取低5位口号

                            fd_ft_js.Text = ws_ftfl_bs.Text;
                            ftfl_cmd = (Convert.ToByte(fdbs[index++]) >> 5) & 0xff;                                         //口3的条件
                            cb_ftfl_mk.Checked = true;
                        }
                        else
                        {
                            index += 1;
                        }

                        ftflb = Convert.ToByte(fdbs[index]) & 0x1F;
                        if (ftflb > 0)
                        {
                            ws_ftflb_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "A" + ftflb.ToString().PadLeft(2, '0') + "0";    //口4,取低5位口号

                            fd_ftb_js.Text = ws_ftflb_bs.Text;
                            ftflb_cmd = (Convert.ToByte(fdbs[index++]) >> 5) & 0xff;                                        //口4的条件
                            cb_ftfl_mk.Checked = true;
                        }
                        else
                        {
                            index += 1;
                        }

                        if (fdbs[index] != "0")
                        {
                            kt1 = Convert.ToByte(fdbs[index]) & 0x1F;
                            //ws_kt1_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1.ToString().PadLeft(2, '0') + "0";      //口5,取低5位口号
                            ws_kt1_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1, 0);  //2018.2.6 by
                            if (string.IsNullOrEmpty(ws_kt1_bs.Text))
                            {
                                //ws_kt1_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1.ToString().PadLeft(2, '0') + "1";      //口5,取低5位口号
                                ws_kt1_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1, 1);  //2018.2.6 by
                            }
                            if (string.IsNullOrEmpty(ws_kt1_bs.Text))
                            {
                                //ws_kt1_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1.ToString().PadLeft(2, '0') + "2";      //口5,取低5位口号
                                ws_kt1_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1, 2);  //2018.2.6 by
                            }

                            temp = (Convert.ToInt32(fdbs[index++]) & 0x60) >> 5;

                            #region by tanxingyan 2014-12-11
                            xtb_ws_kt1_bs.SelectedIndex = Convert.ToInt32(temp.ToString());
                            xtb_fd_kt1.SelectedIndex = Convert.ToInt32(temp.ToString());
                            #endregion
                        }
                        else
                        {
                            index += 1;
                        }
                        if (fdbs[index] != "0")
                        {
                            kt1b = Convert.ToByte(fdbs[index]) & 0x1F;
                            //ws_kt1b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1b.ToString().PadLeft(2, '0') + "0";     //口6,取低5位口号
                            ws_kt1b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1b, 0);  //2018.2.6 by
                            if (string.IsNullOrEmpty(ws_kt1b_bs.Text))
                            {
                                //ws_kt1b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1b.ToString().PadLeft(2, '0') + "1";     //口6,取低5位口号
                                ws_kt1b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1b, 1);  //2018.2.6 by
                            }
                            if (string.IsNullOrEmpty(ws_kt1b_bs.Text))
                            {
                                //ws_kt1b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt1b.ToString().PadLeft(2, '0') + "2";     //口6,取低5位口号
                                ws_kt1b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt1b, 2);  //2018.2.6 by
                            }
                            temp = (Convert.ToInt32(fdbs[index++]) & 0x60) >> 5;

                            #region by tanxingyan 2014-12-11
                            xtb_ws_kt1b_bs.SelectedIndex = Convert.ToInt32(temp.ToString());
                            xtb_fd_kt1b.SelectedIndex = Convert.ToInt32(temp.ToString());
                            #endregion
                        }
                        else
                        {
                            index += 1;
                        }
                        if (fdbs[index] != "0")
                        {
                            kt3 = Convert.ToByte(fdbs[index]) & 0x1F;
                            //ws_kt3_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3.ToString().PadLeft(2, '0') + "0";     //口7,取低5位口号
                            ws_kt3_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3, 0);  //2018.2.6 by
                           
                            if (string.IsNullOrEmpty(ws_kt3_bs.Text))
                            {
                                //ws_kt3_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3.ToString().PadLeft(2, '0') + "1";     //口7,取低5位口号
                                ws_kt3_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3, 1);  //2018.2.6 by
                            }
                            if (string.IsNullOrEmpty(ws_kt3_bs.Text))
                            {
                                //ws_kt3_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3.ToString().PadLeft(2, '0') + "2";     //口7,取低5位口号
                                ws_kt3_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3, 2);  //2018.2.6 by
                            }
                            temp = (Convert.ToInt32(fdbs[index++]) & 0x60) >> 5;

                            #region by tanxingyan 2014-12-11
                            xtb_ws_kt3_bs.SelectedIndex = Convert.ToInt32(temp.ToString());
                            xtb_fd_kt3.SelectedIndex = Convert.ToInt32(temp.ToString());
                            #endregion
                        }
                        else
                        {
                            index += 1;
                        }
                        if (fdbs[index] != "0")
                        {
                            kt3b = Convert.ToByte(fdbs[index]) & 0x1F;
                            //ws_kt3b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3b.ToString().PadLeft(2, '0') + "0";     //口8,取低5位口号
                            ws_kt3b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3b, 0);  //2018.2.6 by
                            
                            if (string.IsNullOrEmpty(ws_kt3b_bs.Text))
                            {
                                //ws_kt3b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3b.ToString().PadLeft(2, '0') + "1";     //口8,取低5位口号
                                ws_kt3b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3b, 1);  //2018.2.6 by
                            }
                            if (string.IsNullOrEmpty(ws_kt3b_bs.Text))
                            {
                                //ws_kt3b_bs.Text = tempStation.Fzh.ToString().PadLeft(3, '0') + "D" + kt3b.ToString().PadLeft(2, '0') + "2";     //口8,取低5位口号
                                ws_kt3b_bs.Text = GetCmbItemStr(tempPointItems, tempStation.Fzh, kt3b, 2);  //2018.2.6 by
                            }
                            temp = (Convert.ToInt32(fdbs[index++]) & 0x60) >> 5;

                            #region by tanxingyan 2014-12-11
                            xtb_ws_kt3b_bs.SelectedIndex = Convert.ToInt32(temp.ToString());
                            xtb_fd_kt3b.SelectedIndex = Convert.ToInt32(temp.ToString());
                            #endregion
                        }
                        else
                        {
                            index += 1;
                        }

                        #endregion

                        #region ---读取模拟量闭锁值---
                        index += 4;
                        #endregion

                        #region ---读取风筒风量口号信息(闭锁值)---


                        #region 从数据库读取风筒风量的值 by huangxxUP 2013-12-28
                        string ftflss = string.Empty;
                        if (!string.IsNullOrEmpty(WindBreakCondition))//bz9存储相关测点值信息
                        {
                            ftflss = WindBreakCondition;
                        }
                        #endregion
                        if (ftfl > 0)
                        {
                            temp = Convert.ToInt32(fdbs[index++]);
                            if (((temp >> 7) & 1) == 1)
                            {
                                ws_ftfl_logic.Text = "与";
                            }
                            else
                            {
                                ws_ftfl_logic.Text = "或";
                            }

                            value = ((temp & 0x7F) << 8) + Convert.ToInt32(fdbs[index++]);
                            //从Bz9中取出来判断  20170627
                            string[] tempArray = WindBreakCondition.Split('|')[1].Split(',');
                            string tempPoint = "";
                            if (tempArray.Length == 8)
                            {
                                tempPoint = tempArray[6].Split(':')[0];
                            }
                            if (tempArray.Length == 6)
                            {
                                tempPoint = tempArray[4].Split(':')[0];
                            }
                            TempPoint = Model.DEFServiceModel.QueryPointByCodeCache(tempPoint);
                            if (TempPoint != null)
                            {
                                temp = long.Parse(TempPoint.Devid);
                            }
                            //增加对风筒风量测点的判断 by huangxxUP 2013-12-28
                            if (temp > 0)
                            {
                                tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.ToString());
                                if (tempDev != null)
                                {
                                    if (tempDev.Type == 1)
                                    {
                                        if (tempDev.Pl2 != 2000)
                                        {
                                            if (ftflss.Split('|').Length > 1)
                                            {
                                                ws_ftfl_value.Text = ftflss.Split('|')[0].ToString().Split(',')[0].ToString();
                                            }

                                        }
                                        else
                                        {
                                            if (ftflss.Split('|').Length > 1)
                                            {
                                                ws_ftfl_value.Text = ftflss.Split('|')[0].ToString().Split(',')[0].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ws_ftfl_value.Text = value.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (ftflss.Split('|').Length > 1)
                                {
                                    ws_ftfl_value.Text = ftflss.Split('|')[0].ToString().Split(',')[0].ToString();
                                }
                            }

                            switch (ftfl_cmd)
                            {
                                case 1: ws_ftfl_tj.Text = "="; break;
                                case 2: ws_ftfl_tj.Text = ">"; break;
                                case 3: ws_ftfl_tj.Text = ">="; break;
                                case 4: ws_ftfl_tj.Text = "<"; break;
                                case 5: ws_ftfl_tj.Text = "<="; break;
                            }
                        }
                        else
                        {
                            index += 2;
                        }

                        if (ftflb > 0)
                        {
                            temp = Convert.ToInt32(fdbs[index++]);
                            value = ((temp & 0x7F) << 8) + Convert.ToInt32(fdbs[index++]);
                            //TempPoint = Model.DEFServiceModel.QueryPointByChannelInfs(tempStation.Fzh, ftflb, 0, 0);
                            //从Bz9中取出来判断  20170627
                            string[] tempArray = WindBreakCondition.Split('|')[1].Split(',');
                            string tempPoint = "";
                            if (tempArray.Length == 8)
                            {
                                tempPoint = tempArray[7].Split(':')[0];
                            }
                            if (tempArray.Length == 6)
                            {
                                tempPoint = tempArray[5].Split(':')[0];
                            }
                            TempPoint = Model.DEFServiceModel.QueryPointByCodeCache(tempPoint);
                            if (TempPoint != null)
                            {
                                temp = long.Parse(TempPoint.Devid);
                            }
                            //增加对风筒风量测点的判断 by huangxxUP 2013-12-28
                            if (temp > 0)
                            {
                                tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(temp.ToString());
                                if (tempDev != null)
                                {
                                    if (tempDev.Type == 1)
                                    {
                                        if (tempDev.Pl2 != 2000)
                                        {
                                            if (ftflss.Split('|').Length > 1)
                                            {
                                                ws_ftflb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[1].ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (ftflss.Split('|').Length > 1)
                                            {
                                                ws_ftflb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[1].ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ws_ftflb_value.Text = value.ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (ftflss.Split('|').Length > 1)
                                {
                                    ws_ftflb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[1].ToString();
                                }
                            }

                            switch (ftflb_cmd)
                            {
                                case 1: ws_ftflb_tj.Text = "="; break;
                                case 2: ws_ftflb_tj.Text = ">"; break;
                                case 3: ws_ftflb_tj.Text = ">="; break;
                                case 4: ws_ftflb_tj.Text = "<"; break;
                                case 5: ws_ftflb_tj.Text = "<="; break;
                            }
                        }
                        else
                        {
                            index += 2;
                        }

                        #endregion

                        //模拟量解锁条件 2字节,
                        //第1字节值固定,T1,T2都是小于,不需要加载
                        index += 1;

                        #region ---读取风电闭锁模/开量解锁条件---
                        try
                        {
                            temp = Convert.ToInt32(fdbs[index++]);
                            switch (temp >> 4)                          //模拟量解锁条件第2字节,高4位对应条件
                            {
                                case 0: fd_ft_tj.Text = ""; break;
                                case 1: fd_ft_tj.Text = "="; break;
                                case 2: fd_ft_tj.Text = ">"; break;
                                case 3: fd_ft_tj.Text = ">="; break;
                                case 4: fd_ft_tj.Text = "<"; break;
                                case 5: fd_ft_tj.Text = "<="; break;
                            }

                            switch (temp & 0x0f)                        //模拟量解锁条件第2字节,低位对应条件
                            {
                                case 0: fd_ftb_tj.Text = ""; break;
                                case 1: fd_ftb_tj.Text = "="; break;
                                case 2: fd_ftb_tj.Text = ">"; break;
                                case 3: fd_ftb_tj.Text = ">="; break;
                                case 4: fd_ftb_tj.Text = "<"; break;
                                case 5: fd_ftb_tj.Text = "<="; break;
                            }
                        }
                        catch (Exception err)
                        {
                            XtraMessageBox.Show("读取风电闭锁模/开量解锁条件读取异常-" + err.Message + " " + err.Source);
                        }
                        #endregion

                        #region ---读取解锁值3---

                        //前4字节表示T1,T2,都是1.5固定,不加载
                        index += 4;

                        if (ftfl > 0)
                        {
                            temp = Convert.ToInt32(fdbs[index++]);
                            if ((temp >> 7 & 0x01) == 0x01)                 //高字节最高位表示与或条件
                            {
                                fd_logic_3.Text = "与";
                            }
                            else
                            {
                                fd_logic_3.Text = "或";
                            }

                            temp = ((temp & 0x7F) << 8) + Convert.ToInt32(fdbs[index++]);       //值3,去掉最高位

                            TempPoint = Model.DEFServiceModel.QueryPointByChannelInfs(tempStation.Fzh, ftfl, 0, 0);
                            if (TempPoint != null)
                            {
                                devid = long.Parse(TempPoint.Devid);
                            }

                            //新增解锁风筒风量测点号判断 by huangxxUP 2013-12-28
                            if (devid > 0)
                            {
                                tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(TempPoint.Devid);
                                if (tempDev != null)
                                {
                                    switch (tempDev.Type)
                                    {
                                        case 1:     //模拟量根据频率关系取值转化

                                            if (tempDev.Pl2 != 2000)
                                            {
                                                if (ftflss.Split('|').Length > 1)
                                                {
                                                    fd_ft_value.Text = ftflss.Split('|')[0].ToString().Split(',')[2].ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (ftflss.Split('|').Length > 1)
                                                {
                                                    fd_ft_value.Text = ftflss.Split('|')[0].ToString().Split(',')[2].ToString();
                                                }
                                            }

                                            break;
                                        case 2:     //开关量,直接取低字节
                                            fd_ft_value.Text = temp.ToString();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (ftflss.Split('|').Length > 1)
                                {
                                    fd_ft_value.Text = ftflss.Split('|')[0].ToString().Split(',')[2].ToString();
                                }
                            }
                        }
                        else
                        {
                            index += 2;
                        }

                        #endregion

                        #region ---读取解锁值4---

                        if (ftflb > 0)
                        {
                            temp = Convert.ToInt32(fdbs[index++]);                              //值4高字节固定为与,

                            if ((temp >> 7 & 0x01) == 0x01)                 //高字节最高位表示与或条件
                            {
                                fd_logic_4.Text = "与";
                            }
                            else
                            {
                                fd_logic_4.Text = "或";
                            }

                            temp = ((temp & 0x7F) << 8) + Convert.ToInt32(fdbs[index++]);                //值4

                            TempPoint = Model.DEFServiceModel.QueryPointByChannelInfs(tempStation.Fzh, ftflb, 0, 0);
                            if (TempPoint != null)
                            {
                                devid = long.Parse(TempPoint.Devid);
                            }
                            //新增解锁风筒风量测点号判断 by huangxxUP 2013-12-28
                            if (devid > 0)
                            {
                                tempDev = Model.DEVServiceModel.QueryDevByDevIDCache(TempPoint.Devid);
                                if (tempDev != null)
                                {
                                    switch (tempDev.Type)
                                    {
                                        case 1:     //模拟量根据频率关系取值转化

                                            if (tempDev.Pl2 != 2000)
                                            {
                                                if (ftflss.Split('|').Length > 1)
                                                {
                                                    fd_ftb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[3].ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (ftflss.Split('|').Length > 1)
                                                {
                                                    fd_ftb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[3].ToString();
                                                }
                                            }

                                            break;
                                        case 2:     //开关量,直接取低字节
                                            fd_ftb_value.Text = temp.ToString();
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (ftflss.Split('|').Length > 1)
                                {
                                    fd_ftb_value.Text = ftflss.Split('|')[0].ToString().Split(',')[3].ToString();
                                }
                            }
                        }
                        else
                        {
                            index += 2;
                        }

                        #endregion

                        #region ---读取风电闭锁开关量解锁值---

                        temp = Convert.ToInt32(fdbs[index++]);

                        #region by tanxingyan 2014-12-11
                        xtb_fd_kt1_js.SelectedIndex = Convert.ToInt32(temp & 0x03);          //第1,2位对应第7字节
                        xtb_fd_kt1b_js.SelectedIndex = Convert.ToInt32((temp >> 2) & 0x03);  //第3,4位对应第8字节
                        if (xtb_fd_kt3_js.Items.Count > 0)
                        {
                            xtb_fd_kt3_js.SelectedIndex = Convert.ToInt32((temp >> 4) & 0x03);   //第5,6位对应第9字节
                            xtb_fd_kt3b_js.SelectedIndex = Convert.ToInt32((temp >> 6) & 0x03);  //第7,8位对应第10字节
                        }
                        #endregion
                        #endregion

                        UInt16 ckwind = Convert.ToByte(fdbs[index++]);
                        ckwind |= (UInt16)(Convert.ToByte(fdbs[index]) << 8);
                        //修改加载方式  20170620
                        string bindPointWindBreak = "";
                        string bindPointWindBreakCH4 = "";
                        for (int i = 0; i < cCmbControlWindBreak.Properties.Items.Count; i++)
                        {
                            bindPointWindBreak += cCmbControlWindBreak.Properties.Items[i].ToString() + ",";
                        }
                        for (int i = 0; i < cCmbControlWindBreakCH4.Properties.Items.Count; i++)
                        {
                            bindPointWindBreakCH4 += cCmbControlWindBreakCH4.Properties.Items[i].ToString() + ",";
                        }
                        if (tempStationDev != null)
                        {
                            if (tempStationDev.LC2 == 13) //通用智能分站处理方式  20170428
                            {
                                cCmbControlWindBreak.Text = SetControlText(ckwind, bindPointWindBreak); //风电闭锁控制口
                                cCmbControlWindBreakCH4.Text = SetControlText(ckws, bindPointWindBreakCH4); //甲烷风电闭锁控制口
                            }
                            else
                            {
                                //其他处理方式
                                cCmbControlWindBreak.Text = SetControlText(ckwind, bindPointWindBreak); //风电闭锁控制口
                                cCmbControlWindBreakCH4.Text = SetControlText(ckws, bindPointWindBreakCH4); //甲烷风电闭锁控制口
                            }
                        }
                        else
                        {
                            //其他处理方式
                            cCmbControlWindBreak.Text = SetControlText(ckwind, bindPointWindBreak); //风电闭锁控制口
                            cCmbControlWindBreakCH4.Text = SetControlText(ckws, bindPointWindBreakCH4); //甲烷风电闭锁控制口
                        }
                    }
                    else
                    {
                        fd_logic_1.SelectedIndex = 1;
                        fd_logic_2.SelectedIndex = 1;
                        fd_logic_3.SelectedIndex = 1;
                        ws_ftfl_logic.SelectedIndex = 1;
                    }
                }
                else
                {
                    //xuzp 20150319
                    fd_logic_1.SelectedIndex = 1;
                    fd_logic_2.SelectedIndex = 1;
                    fd_logic_3.SelectedIndex = 1;
                    ws_ftfl_logic.SelectedIndex = 1;
                }

                //加载时 获取本控和交叉控 by huangxxUP 2013-12-28
                //GetKzk();
                //瓦斯控制口 by huangxxUP 2013-12-28
                //MasCheck_ws = ck_ws_kzk.CheckValue;
                //风电控制口 by huangxxUP 2013-12-28
                //MasCheck_fd = ck_fdbs_kzk.CheckValue;


            }
            catch (Exception err)
            {
                LogHelper.Error("风电闭锁加载失败!" + err.Message);
            }
            #endregion
            if (LxChange)
            {
                CbtnOK_Click(sender, e);
            }
        }

        private string GetCmbItemStr( IList<Jc_DefInfo> defInfos,int fzh, int kh, int dzh)
        {
            Jc_DefInfo def = defInfos.FirstOrDefault(a => a.Fzh == fzh && a.Kh == kh && a.Dzh == dzh);
            string itemstr = def.Point + "." + def.Wz + "【" + def.DevName + "】";
            return itemstr;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int fzh = tempStation.Fzh - 1;
                int dy_byte = 0;
                StringBuilder sb = new StringBuilder();
                int mn1_hz = 0;
                int mn2_hz = 0;
                int value = -1;
                Jc_DefInfo tempPoint = null;
                Jc_DevInfo tempDEV = null;
                string tempstring;
                int kh_t1 = 0, kh_t2 = 0, kh_kt1 = 0, kh_kt1b = 0, kh_kt3 = 0, kh_kt3b = 0, kh_ftfl = 0, kh_ftflb = 0;

                if (!valueCheck())
                {
                    return;
                }

                #region ---单双风机标志 //xuzpUP 20150116 处理双风机两个开停的问题---
                //双单风机标志
                if (rb_one.Checked)
                {
                    sb.Append("1,");
                }
                else if (rb_two.Checked)
                {
                    sb.Append("2,");
                }
                else
                {
                    sb.Append("3,");
                }
                #endregion

                #region ---口号取值---
                try
                {
                    kh_ftfl = 0; kh_ftflb = 0;
                    tempstring = cmb_t1.Text.Split('.')[0];
                    kh_t1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_t1 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = cmb_t2.Text.Split('.')[0];
                    kh_t2 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_t2 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = ws_kt1_bs.Text.Split('.')[0];
                    kh_kt1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_kt1 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = ws_kt1b_bs.Text.Split('.')[0];
                    kh_kt1b = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_kt1b = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    if (rb_one.Checked == false && rb_three.Checked == false) //xuzpUP 20150116 处理双风机两个开停的问题
                    {
                        tempstring = ws_kt3_bs.Text.Split('.')[0];
                        kh_kt3 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                        if (tempstring.Length > 6)
                        {
                            kh_kt3 = Convert.ToInt16(tempstring.Substring(4, 2));
                        }

                        tempstring = ws_kt3b_bs.Text.Split('.')[0];
                        kh_kt3b = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                        if (tempstring.Length > 6)
                        {
                            kh_kt3b = Convert.ToInt16(tempstring.Substring(4, 2));
                        }
                    }

                    if (cb_ftfl_mk.Checked)
                    {
                        tempstring = ws_ftfl_bs.Text.Split('.')[0];
                        if (tempstring != "")
                        {
                            kh_ftfl = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                            if (tempstring.Length > 6)
                            {
                                kh_ftfl = Convert.ToInt16(tempstring.Substring(4, 2));
                            }
                        }
                        tempstring = ws_ftflb_bs.Text.Split('.')[0];
                        if (tempstring != "")
                        {
                            kh_ftflb = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                            if (tempstring.Length > 6)
                            {
                                kh_ftflb = Convert.ToInt16(tempstring.Substring(4, 2));
                            }
                        }
                    }
                    else
                    {
                        kh_ftfl = 0;
                        kh_ftflb = 0;
                    }
                }
                catch { XtraMessageBox.Show("口号取值错误!"); return; }
                #endregion
                sb.Append(Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) & 0x00FF).ToString() + ",");      //甲烷闭锁控制口 1字节

                #region ---风电闭锁开关量逻辑解锁条件---
                dy_byte = 0;
                dy_byte = dy_byte | ((fd_logic_1.Text == "与") ? 0x01 : 0);             //7,8字节之间关系,根据选择保存
                //8,9字节之间关系,固定为0,或
                dy_byte = dy_byte | ((fd_logic_2.Text == "与") ? (1 << 2) : (0 << 2));  //7,8字节之间关系,根据选择保存
                sb.Append(dy_byte.ToString() + ",");        //风筒风量标记
                #endregion

                #region ---口号1,2---

                dy_byte = 0;
                dy_byte = ((dy_byte | 0x60) | kh_t1);       //>= 01100000
                sb.Append(dy_byte.ToString() + ",");        //口号1

                dy_byte = 0;
                dy_byte = ((dy_byte | 0x60) | kh_t2);       //>= 01100000
                sb.Append(dy_byte.ToString() + ",");        //口号2;

                #endregion

                #region ---口号3,4 风筒风量---

                dy_byte = 0;
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftfl);
                        switch (ws_ftfl_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                sb.Append(dy_byte.ToString() + ",");        //口号3;

                dy_byte = 0;
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftflb);
                        switch (ws_ftflb_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                sb.Append(dy_byte.ToString() + ",");        //口号4;

                #endregion

                #region ---口号5,6风机开停---

                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                sb.Append(dy_byte.ToString() + ",");    //口号5;

                dy_byte = 0;
                if (rb_one.Checked || rb_three.Checked) //单风机,kt3,kt3b口号为0
                {
                    dy_byte = dy_byte | 0x00;               //最高位置0(或)--kt3未定义时,表示Kt1b与kt3之间的关系为或
                }
                else
                {
                    dy_byte = dy_byte | 0x80;               //最高位置1(与)--kt3已定义时,表示Kt1b与kt3之间的关系为与
                }
                dy_byte = dy_byte | kh_kt1b;            //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1b_bs.Value) << 5));  //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1b_bs.SelectedIndex) << 5));  //5,6位为值 by tanxingyan 2014-12-11
                sb.Append(dy_byte.ToString() + ",");    //口号6;

                #endregion

                #region ---口号7,8风机开停---

                if (rb_one.Checked || rb_three.Checked)
                {
                    //单风机,只有2个开关量,7,8为0
                    dy_byte = 0;
                    sb.Append(dy_byte.ToString() + ",");        //口号7;
                    sb.Append(dy_byte.ToString() + ",");        //口号8;
                }
                else
                {
                    //双风机,4个开关量
                    dy_byte = 0;
                    dy_byte = dy_byte | 0x80;                   //最高位置1,8
                    dy_byte = dy_byte | kh_kt3;                 //低5位口号,0-4
                    //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt3_bs.Value) << 5));   //5,6位为值
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt3_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11

                    sb.Append(dy_byte.ToString() + ",");        //口号7;

                    dy_byte = 0;
                    dy_byte = dy_byte | 0x80;                   //最高位置1,8
                    dy_byte = dy_byte | kh_kt3b;                //低5位口号,0-4
                    //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt3b_bs.Value) << 5));   //5,6位为值
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt3b_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                    sb.Append(dy_byte.ToString() + ",");                    //口号8;
                }

                #endregion

                #region ---瓦斯闭锁值1,值2---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_t1 > 0)
                {
                    //tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text);
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]); //2017.12.20 by
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                        }
                    }
                    sb.Append(((mn1_hz >> 8) & 0xff).ToString() + ",");         //口1的值,值1高字节,最高位为0表示或
                    sb.Append(((mn1_hz) & 0xff).ToString() + ",");              //值1低字节
                }

                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15)
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                        }
                    }

                    sb.Append((((mn2_hz >> 8) & 0xff) | 0x80).ToString() + ",");        //值1高字节:最高位为1表示与(后跟的是开关量字节)
                    sb.Append(((mn2_hz) & 0xff).ToString() + ",");                      //值1低字节
                }

                #endregion

                #region ---风电闭锁值3---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftfl_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(ws_ftfl_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)              //取频率值
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) * Convert.ToDouble(ws_ftfl_value.Text) / tempDEV.LC));
                                    }
                                    else
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftfl_value.Text));
                                    }
                                }
                            }

                            sb.Append(((mn1_hz >> 8) | ((ws_ftfl_logic.Text == "与") ? 0x80 : 0)).ToString() + ",");    //模拟量值3;频率高8位,最高位0是或,1是与
                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                              //模拟量值3;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftfl_value.Text);
                            sb.Append(((ws_ftfl_logic.Text == "与") ? 0x80 : 0).ToString() + ",");                      //开关量值3;最高位0是或,1是与
                            sb.Append(value.ToString() + ",");
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---风电闭锁值4---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        if (ws_ftflb_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftflb_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn2_hz = (int)(Convert.ToDouble(ws_ftflb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                    else
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                }
                            }

                            sb.Append((mn2_hz >> 8).ToString() + ",");                  //模拟量值4;频率高8位,最高位0是或,1是与
                            sb.Append(((mn2_hz) & 0xff).ToString() + ",");              //模拟量值4;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftflb_value.Text);
                            sb.Append("0,");                                            //开关量值4;最高位0是或,1是与
                            sb.Append(value.ToString() + ",");                          //开关量值4
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---模拟量解锁条件---

                dy_byte = 0;
                sb.Append((Convert.ToString(68) + ","));  //0100 0100(//模拟量解锁逻辑条件,1,2号口直接是小于)

                dy_byte = 0;
                if (kh_ftfl > 0 || kh_ftflb > 0)
                {
                    if (fd_ft_js.Text != "")                //模拟量解锁逻辑条件3,4可表示模/开两种
                    {
                        dy_byte = calculateValue(fd_ft_tj.Text) << 4;
                    }

                    if (fd_ftb_js.Text != "")
                    {
                        dy_byte |= calculateValue(fd_ftb_tj.Text);
                    }
                }
                sb.Append(dy_byte.ToString() + ",");

                #endregion

                #region ---解锁值1,值2---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_t1 > 0)
                {//判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_js.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_js.Text));
                            }
                        }
                    }

                    sb.Append(((mn1_hz >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    sb.Append(((mn1_hz) & 0xff).ToString() + ",");              //值1低;
                }

                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_js.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_js.Text));
                            }
                        }
                    }

                    sb.Append(((mn2_hz >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    sb.Append(((mn2_hz) & 0xff).ToString() + ",");              //值1低字节
                }

                #endregion

                #region ---解锁值,3值4---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftfl > 0)
                {
                    if (fd_ft_js.Text != "")
                    {
                        if (fd_ft_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ft_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ft_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ft_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ft_value.Text));
                                }
                            }

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                sb.Append(((mn1_hz >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;频率高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                sb.Append((mn1_hz >> 8).ToString() + ",");                                               //解锁值3;频率高8位,最高位0是或,1是与
                            }

                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                               //解锁值3;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ft_value.Text);

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                sb.Append(((value >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                sb.Append((value >> 8).ToString() + ",");                                               //解锁值3;高8位,最高位0是或,1是与
                            }

                            sb.Append(value.ToString() + ",");                                                          //解锁值3:低8位
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                if (kh_ftflb > 0)
                {
                    if (fd_ftb_js.Text != "")
                    {
                        if (fd_ftb_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ftb_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ftb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                            }

                            sb.Append(((mn1_hz >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值4;频率高8位,最高位为0或
                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                           //解锁值4;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ftb_value.Text);
                            sb.Append(((value >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");     //解锁值4;高8位,最高位为0或
                            sb.Append(value.ToString() + ",");                                                       //解锁值4:低8位
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---开关量解锁值---

                dy_byte = 0;
                #region by tanxingyan 2014-12-11
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt1_js.SelectedIndex)));            //口5解锁值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt1b_js.SelectedIndex) << 2));      //口6解锁值
                if (xtb_fd_kt3_js.SelectedIndex < 0)
                {
                    if (panel_fd_js.Visible == false) //备开停没有配置的情况下 发送 A xuzp20150529
                    {
                        dy_byte = (dy_byte | (0x2 << 4));       //口7解锁值
                    }
                    else
                    {
                        dy_byte = (dy_byte | (0 << 4));       //口7解锁值
                    }
                }
                else
                {
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt3_js.SelectedIndex) << 4));       //口7解锁值
                }
                if (xtb_fd_kt3b_js.SelectedIndex < 0)
                {
                    if (panel_fd_js.Visible == false) //备开停没有配置的情况下 发送 A xuzp20150529
                    {
                        dy_byte = (dy_byte | (0x2 << 6));      //口8解锁值
                    }
                    else
                    {
                        dy_byte = (dy_byte | (0 << 6));      //口8解锁值
                    }
                }
                else
                {
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt3b_js.SelectedIndex) << 6));      //口8解锁值
                }
                #endregion
                sb.Append(dy_byte.ToString() + ",");                               //开关量口解锁条件

                #endregion

                #region ---风电闭锁控制口---

                dy_byte = Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreak.Text) & 0x00FF);
                sb.Append(dy_byte.ToString() + ",");                                    //风电闭锁控制口

                dy_byte = Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreak.Text) >> 8));
                //增加故障闭锁标记  20170627
                if ((tempStation.Bz3 & 0x4) == 0x4)
                {
                    dy_byte |= 0x01;//置故障闭锁标记
                }
                sb.Append(dy_byte.ToString() + ",");                                    //风电闭锁控制口


                #endregion

                #region ---甲烷风电闭锁控制2---
                sb.Append(Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) >> 8)).ToString());
                #endregion

                #region 单独存储风筒风量值与模开点号
                string ftflsb = string.Empty;
                if (ws_ftfl_value.Visible)
                {
                    string ftfl_bs = ws_ftfl_value.Text.ToString();
                    string ftflb_bs = ws_ftflb_value.Text.ToString();
                    string ftfl_js = fd_ft_value.Text.ToString();
                    string ftflb_js = fd_ftb_value.Text.ToString();
                    ftflsb = ftfl_bs + "," + ftflb_bs + "," + ftfl_js + "," + ftflb_js + "|";
                }
                //储存固定口
                //ftflsb += cmb_t1.Text.ToString() + ":0," + cmb_t2.Text.ToString() + ":0," + ws_kt1_bs.Text.ToString() + ":0," + ws_kt1b_bs.Text.ToString() + ":0,";
                ftflsb += cmb_t1.Text.ToString().Split('.')[0] + ":0,"
                    + cmb_t2.Text.ToString().Split('.')[0] + ":0,"
                    + ws_kt1_bs.Text.ToString().Split('.')[0] + ":0,"
                    + ws_kt1b_bs.Text.ToString().Split('.')[0] + ":0,";
                //双风机
                if (ws_kt3_bs.Visible)
                {
                    ftflsb += ws_kt3_bs.Text.ToString().Split('.')[0] + ":0," + ws_kt3b_bs.Text.ToString().Split('.')[0] + ":0,";
                }
                //风筒风量
                if (ws_ftfl_bs.Enabled)
                {
                    ftflsb += ws_ftfl_bs.Text.ToString().Split('.')[0] + ":0," + ws_ftflb_bs.Text.ToString().Split('.')[0] + ":0,";
                }
                ftflsb = ftflsb.TrimEnd(',');

                #endregion

                #region 新协议（36个字节）
                string WindAtresiaByteString = "";
                #region//甲烷风电闭锁控制口
                WindAtresiaByteString += Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) >> 8)).ToString() + ",";
                WindAtresiaByteString += Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) & 0x00FF).ToString() + ",";
                #endregion
                #region//风电闭锁解锁条件
                dy_byte = 0;
                dy_byte = dy_byte | (xtb_fd_kt1_js.SelectedIndex);             //0,1字节之间关系,根据选择保存
                //8,9字节之间关系,固定为0,或
                dy_byte = dy_byte | (xtb_fd_kt1b_js.SelectedIndex << 2);  //2,3字节之间关系,根据选择保存
                dy_byte = dy_byte | ((fd_logic_1.Text == "与" ? 0x01 : 0x00) << 4);
                WindAtresiaByteString += dy_byte.ToString() + ",";
                #endregion
                #region//主控通道T1口号及判断条件
                int T1ChannelNumber = 0;
                T1ChannelNumber = (byte)(3 << 5) + kh_t1;//闭锁条件默认是 011 >=
                WindAtresiaByteString += T1ChannelNumber + ",";
                #endregion
                #region//主控通道T2口号及判断条件
                int T2ChannelNumber = 0;
                T2ChannelNumber = (byte)(3 << 5) + kh_t2;//闭锁条件默认是 011 >=
                WindAtresiaByteString += T2ChannelNumber + ",";
                #endregion
                #region//风筒风量1及判断条件
                dy_byte = 0;
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftfl);
                        switch (ws_ftfl_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                WindAtresiaByteString += (dy_byte.ToString() + ",");        //口号3;
                #endregion
                #region//风筒风量2及判断条件
                dy_byte = 0;
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftflb);
                        switch (ws_ftflb_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                WindAtresiaByteString += (dy_byte.ToString() + ",");        //口号4;
                #endregion
                #region//1#配电柜 主开停地址号及判断条件、取值
                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                WindAtresiaByteString += (dy_byte.ToString() + ",");    //口号5;
                #endregion
                #region//1#配电柜 副开停地址号
                byte BackKT1PointHighBit = 0;
                byte BackKT1PointMiddleBit = 0;
                byte BackKT1PointLowBit = 0;
                string[] BackKT1Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                if (kh_kt3 > 0)
                {
                    BackKT1Point[24 - kh_kt3] = "1";
                }
                BackKT1PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 16);
                BackKT1PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 8);
                BackKT1PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2));
                WindAtresiaByteString += BackKT1PointHighBit + ",";
                WindAtresiaByteString += BackKT1PointMiddleBit + ",";
                WindAtresiaByteString += BackKT1PointLowBit + ",";
                #endregion
                #region//2#配电柜 主开停地址号及判断条件、取值
                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1b;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1b_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                WindAtresiaByteString += (dy_byte.ToString() + ",");    //口号5;
                #endregion
                #region//2#配电柜 副开停地址号
                byte BackKT2PointHighBit = 0;
                byte BackKT2PointMiddleBit = 0;
                byte BackKT2PointLowBit = 0;
                string[] BackKT2Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                if (kh_kt3b > 0)
                {
                    BackKT1Point[24 - kh_kt3b] = "1";
                }
                BackKT2PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 16);
                BackKT2PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 8);
                BackKT2PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2));
                WindAtresiaByteString += BackKT2PointHighBit + ",";
                WindAtresiaByteString += BackKT2PointMiddleBit + ",";
                WindAtresiaByteString += BackKT2PointLowBit + ",";
                #endregion
                #region//T1甲烷闭锁值及条件
                if (kh_t1 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);

                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                        }
                    }
                    WindAtresiaByteString += (((mn1_hz >> 8) & 0xff).ToString() + ",");         //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");              //值1低字节
                }
                else
                {
                    WindAtresiaByteString += ((((0 >> 8) & 0xff)).ToString() + ",");          //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");               //值1低字节
                }
                #endregion
                #region//T2甲烷闭锁值及条件
                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15)
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += ((((mn2_hz >> 8) & 0xff) | 0x80).ToString() + ",");        //值1高字节:最高位为1表示与(后跟的是开关量字节)
                    WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");                      //值1低字节
                }
                else
                {
                    WindAtresiaByteString += ((((0 >> 8) & 0xff) | 0x80).ToString() + ",");          //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");               //值1低字节
                }
                #endregion
                #region//风筒风量1闭锁值及条件
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftfl_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(ws_ftfl_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)              //取频率值
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) * Convert.ToDouble(ws_ftfl_value.Text) / tempDEV.LC));
                                    }
                                    else
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftfl_value.Text));
                                    }
                                }
                            }

                            WindAtresiaByteString += (((mn1_hz >> 8) | ((ws_ftfl_logic.Text == "与") ? 0x80 : 0)).ToString() + ",");    //模拟量值3;频率高8位,最高位0是或,1是与
                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                              //模拟量值3;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftfl_value.Text);
                            WindAtresiaByteString += (((ws_ftfl_logic.Text == "与") ? 0x80 : 0).ToString() + ",");                      //开关量值3;最高位0是或,1是与
                            WindAtresiaByteString += (value.ToString() + ",");
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风筒风量2闭锁值及条件
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        if (ws_ftflb_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftflb_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn2_hz = (int)(Convert.ToDouble(ws_ftflb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                    else
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                }
                            }

                            WindAtresiaByteString += ((mn2_hz >> 8).ToString() + ",");                  //模拟量值4;频率高8位,最高位0是或,1是与
                            WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");              //模拟量值4;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftflb_value.Text);
                            WindAtresiaByteString += ("0,");                                            //开关量值4;最高位0是或,1是与
                            WindAtresiaByteString += (value.ToString() + ",");                          //开关量值4
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//T1 T2解锁条件
                dy_byte = 0;
                WindAtresiaByteString += ((Convert.ToString(68) + ","));  //0100 0100(//模拟量解锁逻辑条件,1,2号口直接是小于)                
                #endregion
                #region//风筒风量1 风筒风量2解锁条件
                dy_byte = 0;
                if (kh_ftfl > 0 || kh_ftflb > 0)
                {
                    if (fd_ft_js.Text != "")                //模拟量解锁逻辑条件3,4可表示模/开两种
                    {
                        dy_byte = calculateValue(fd_ft_tj.Text) << 4;
                    }

                    if (fd_ftb_js.Text != "")
                    {
                        dy_byte |= calculateValue(fd_ftb_tj.Text);
                    }
                }
                WindAtresiaByteString += (dy_byte.ToString() + ",");
                #endregion
                #region//T1甲烷解锁值
                if (kh_t1 > 0)
                {//判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_js.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_js.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += (((mn1_hz >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");              //值1低;
                }
                else
                {
                    WindAtresiaByteString += (((0 >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");              //值1低;
                }
                #endregion
                #region//T2甲烷解锁值
                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_js.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_js.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += (((mn2_hz >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");              //值1低字节
                }
                else
                {
                    WindAtresiaByteString += (((0 >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");              //值1低字节
                }
                #endregion
                #region//风筒风量1解锁值
                if (kh_ftfl > 0)
                {
                    if (fd_ft_js.Text != "")
                    {
                        if (fd_ft_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ft_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ft_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ft_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ft_value.Text));
                                }
                            }

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                WindAtresiaByteString += (((mn1_hz >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;频率高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                WindAtresiaByteString += ((mn1_hz >> 8).ToString() + ",");                                               //解锁值3;频率高8位,最高位0是或,1是与
                            }

                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                               //解锁值3;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ft_value.Text);

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                WindAtresiaByteString += (((value >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                WindAtresiaByteString += ((value >> 8).ToString() + ",");                                               //解锁值3;高8位,最高位0是或,1是与
                            }

                            WindAtresiaByteString += (value.ToString() + ",");                                                          //解锁值3:低8位
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风筒风量2解锁值
                if (kh_ftflb > 0)
                {
                    if (fd_ftb_js.Text != "")
                    {
                        if (fd_ftb_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ftb_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ftb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                            }

                            WindAtresiaByteString += (((mn1_hz >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值4;频率高8位,最高位为0或
                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                           //解锁值4;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ftb_value.Text);
                            WindAtresiaByteString += (((value >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");     //解锁值4;高8位,最高位为0或
                            WindAtresiaByteString += (value.ToString() + ",");                                                       //解锁值4:低8位
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风电闭锁控制口
                dy_byte = Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreak.Text) >> 8));
                WindAtresiaByteString += (dy_byte.ToString() + ",");                                    //风电闭锁控制口(高在前)

                dy_byte = Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreak.Text) & 0x00FF);
                WindAtresiaByteString += (dy_byte.ToString() + ",");                                    //风电闭锁控制口（低在后）
                #endregion
                #region//T1 T2 风筒风量1 风筒风量2 多参数地址号
                byte T1AddressNumber = 0;
                if (!string.IsNullOrEmpty(cmb_t1.Text))
                {
                    T1AddressNumber = byte.Parse(cmb_t1.Text.Trim().Substring(6, 1));
                }
                byte T2AddressNumber = 0;
                if (!string.IsNullOrEmpty(cmb_t2.Text))
                {
                    T2AddressNumber = byte.Parse(cmb_t2.Text.Trim().Substring(6, 1));
                }
                int AdderssNumberByte = (T2AddressNumber << 2) + T1AddressNumber;
                byte FTFL1AddressNumber = 0;
                if (!string.IsNullOrEmpty(ws_ftfl_bs.Text))
                {
                    FTFL1AddressNumber = byte.Parse(ws_ftfl_bs.Text.Trim().Substring(6, 1));
                }
                byte FTFL2AddressNumber = 0;
                if (!string.IsNullOrEmpty(ws_ftflb_bs.Text))
                {
                    FTFL2AddressNumber = byte.Parse(ws_ftflb_bs.Text.Trim().Substring(6, 1));
                }
                int FTFLAdderssNumberByte = (FTFL2AddressNumber << 2) + FTFL1AddressNumber;
                WindAtresiaByteString += (FTFLAdderssNumberByte << 4) + AdderssNumberByte;
                #endregion

                #endregion

                #region ---数据库存储---
                if (sb.Length > 0)
                {
                    try
                    {
                        if (tempStation.Bz10 != sb.ToString() || tempStation.Bz9 != ftflsb)
                        {
                            tempStationForm.CtxbControlBytes.Text = sb.ToString();
                            tempStationForm.CtxbControlBytesNew.Text = WindAtresiaByteString;//新风电闭锁的配置  20171018
                            tempStationForm.CtxbControlConditon.Text = ftflsb;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
                #endregion

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbtnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmb_t_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int _fzh = -1;
                int _kh = -1;
                int _dzh = 0;
                Jc_DefInfo TempPoint = null;
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;
                if (_combox.Text != "")
                {
                    _fzh = Convert.ToInt32(_combox.Text.Substring(0, 3));

                    if (_combox.Text.Length >= 6)
                    {
                        _kh = Convert.ToInt32(_combox.Text.Substring(4, 2));
                    }
                    if (_combox.Text.Length > 6)
                    {
                        //_dzh = Convert.ToInt32(_combox.Text.Substring(6));
                        _dzh = Convert.ToInt32(_combox.Text.Substring(6, 1));
                    }

                    TempPoint = Model.DEFServiceModel.QueryPointByChannelInfs(_fzh, _kh, _dzh, 0);
                }

                switch (_combox.Tag.ToString())
                {
                    case "T1":
                        ws_t1_bs.Items.Clear();
                        ws_t1_js.Items.Clear();
                        CcmbPowerOFFControlT1.Text = "";
                        if (_combox.SelectedIndex != -1)
                        {
                            ws_t1_bs.Items.Add(_combox.Text);
                            ws_t1_js.Items.Add(_combox.Text);
                            ws_t1_bs.SelectedIndex = 0;
                            ws_t1_js.SelectedIndex = 0;
                            if (TempPoint != null)
                            {
                                string bindPointCcmbPowerOFFControlT1 = "";
                                for (int i = 0; i < CcmbPowerOFFControlT1.Properties.Items.Count; i++)
                                {
                                    bindPointCcmbPowerOFFControlT1 += CcmbPowerOFFControlT1.Properties.Items[i].ToString() + ",";
                                }
                                CcmbPowerOFFControlT1.Text = SetLocalControlText(TempPoint.K2, bindPointCcmbPowerOFFControlT1); //T1断电控制口
                            }
                        }

                        break;
                    case "T2":
                        ws_t2_bs.Items.Clear();
                        ws_t2_js.Items.Clear();
                        CcmbPowerOFFControlT2.Text = "";
                        if (_combox.SelectedIndex != -1)
                        {
                            ws_t2_bs.Items.Add(_combox.Text);
                            ws_t2_js.Items.Add(_combox.Text);
                            ws_t2_bs.SelectedIndex = 0;
                            ws_t2_js.SelectedIndex = 0;
                            if (TempPoint != null)
                            {
                                string bindPointCcmbPowerOFFControlT2 = "";
                                for (int i = 0; i < CcmbPowerOFFControlT2.Properties.Items.Count; i++)
                                {
                                    bindPointCcmbPowerOFFControlT2 += CcmbPowerOFFControlT2.Properties.Items[i].ToString() + ",";
                                }
                                CcmbPowerOFFControlT2.Text = SetLocalControlText(TempPoint.K2, bindPointCcmbPowerOFFControlT2); //T2断电控制口
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_ftfl_bs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;
                if (ws_ftfl_bs.Text != "")
                {
                    fd_ft_js.Text = ws_ftfl_bs.Text;
                    ws_ftfl_tj.Enabled = true;
                    ws_ftfl_value.Enabled = true;

                    if (_combox.Text.IndexOf("A") != -1)
                    {
                        //模拟量
                        ws_ftfl_tj.Items.Clear();
                        //ws_ftfl_tj.Items.Add("=");
                        ws_ftfl_tj.Items.Add(">");
                        ws_ftfl_tj.Items.Add(">=");
                        ws_ftfl_tj.Items.Add("<");
                        ws_ftfl_tj.Items.Add("<=");
                    }
                    else
                    {
                        //开关量
                        ws_ftfl_tj.Items.Clear();
                        ws_ftfl_tj.Items.Add("=");
                        ws_ftfl_tj.Text = "=";
                    }
                }
                else
                {
                    ws_ftfl_tj.Enabled = false;
                    ws_ftfl_value.Enabled = false;
                    ws_ftfl_value.Text = "";
                }

                if (ws_ftfl_bs.Text != "" && ws_ftflb_bs.Text != "")
                {
                    ws_ftfl_logic.Enabled = true;
                }
                else
                {
                    ws_ftfl_logic.Enabled = false;
                }
            }
            catch (Exception err) { XtraMessageBox.Show(err.Message); }
        }

        private void ws_ftflb_bs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;

                if (ws_ftflb_bs.Text != "")
                {
                    fd_ftb_js.Text = ws_ftflb_bs.Text;
                    ws_ftflb_tj.Enabled = true;
                    ws_ftflb_value.Enabled = true;

                    if (_combox.Text.IndexOf("A") != -1)
                    {
                        //模拟量
                        ws_ftflb_tj.Items.Clear();
                        //ws_ftflb_tj.Items.Add("=");
                        ws_ftflb_tj.Items.Add(">");
                        ws_ftflb_tj.Items.Add(">=");
                        ws_ftflb_tj.Items.Add("<");
                        ws_ftflb_tj.Items.Add("<=");
                    }
                    else
                    {
                        //开关量
                        ws_ftflb_tj.Items.Clear();
                        ws_ftflb_tj.Items.Add("=");
                        ws_ftflb_tj.Text = "=";
                    }
                }
                else
                {
                    ws_ftflb_tj.Enabled = false;
                    ws_ftflb_value.Enabled = false;
                    ws_ftflb_value.Text = "";
                }

                if (ws_ftfl_bs.Text != "" && ws_ftflb_bs.Text != "")
                {
                    ws_ftfl_logic.Enabled = true;
                }
                else
                {
                    ws_ftfl_logic.Enabled = false;
                }
            }
            catch (Exception err) { XtraMessageBox.Show(err.Message); }
        }

        private void rb_one_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FJ_One();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void rb_two_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FJ_Two();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void cb_ftfl_mk_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cb_ftfl_mk.Checked)
                {
                    ws_ftfl_bs.Enabled = true;
                    ws_ftflb_bs.Enabled = true;
                    ws_ftfl_logic.Enabled = true;
                    fd_logic_3.Enabled = true;
                }
                else
                {
                    ws_ftfl_bs.Text = "";
                    ws_ftfl_bs.Enabled = false;
                    ws_ftflb_bs.Text = "";
                    ws_ftflb_bs.Enabled = false;
                    ws_ftfl_logic.Text = "";
                    ws_ftfl_logic.Enabled = false;
                    fd_logic_3.Text = "";
                    fd_logic_3.Enabled = false;
                    fd_ft_js.Text = "";
                    fd_ftb_js.Text = "";
                    fd_ft_tj.Text = "";
                    fd_ft_tj.Enabled = false;
                    fd_ftb_tj.Text = "";
                    fd_ftb_tj.Enabled = false;
                    fd_ft_value.Text = "";
                    fd_ft_value.Enabled = false;
                    fd_ftb_value.Text = "";
                    fd_ftb_value.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void xtb_ws_kt1_bs_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                xtb_fd_kt1.SelectedIndex = xtb_ws_kt1_bs.SelectedIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void xtb_ws_kt1b_bs_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                xtb_fd_kt1b.SelectedIndex = xtb_ws_kt1b_bs.SelectedIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }

        private void xtb_ws_kt3_bs_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                xtb_fd_kt3.SelectedIndex = xtb_ws_kt3_bs.SelectedIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void xtb_ws_kt3b_bs_SelectedItemChanged(object sender, EventArgs e)
        {
            try
            {
                xtb_fd_kt3b.SelectedIndex = xtb_ws_kt3b_bs.SelectedIndex;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }

        }
        /// <summary>
        /// 开停选择关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ws_kt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;

                //2018.2.6 by 增加设备名称显示  后修改
                string point = "";
                point = _combox.Text.Split('.')[0];
                switch (_combox.Tag.ToString())
                {
                    case "1":
                        fd_kt1.Text = _combox.Text;
                        fd_kt1_js.Text = _combox.Text;

                        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                        if (kglxs.ContainsKey(point))
                        {
                            xtb_ws_kt1_bs.Text = "";
                            xtb_ws_kt1_bs.Items.Clear();
                            xtb_ws_kt1_bs.Items.Add(kglxs[point][0]);
                            xtb_ws_kt1_bs.Items.Add(kglxs[point][1]);
                            xtb_ws_kt1_bs.Items.Add(kglxs[point][2]);

                            xtb_fd_kt1.Text = "";
                            xtb_fd_kt1.Items.Clear();
                            xtb_fd_kt1.Items.Add(kglxs[point][0]);
                            xtb_fd_kt1.Items.Add(kglxs[point][1]);
                            xtb_fd_kt1.Items.Add(kglxs[point][2]);

                            xtb_fd_kt1_js.Text = "";
                            xtb_fd_kt1_js.Items.Clear();
                            xtb_fd_kt1_js.Items.Add(kglxs[point][0]);
                            xtb_fd_kt1_js.Items.Add(kglxs[point][1]);
                            xtb_fd_kt1_js.Items.Add(kglxs[point][2]);
                            xtb_ws_kt1_bs.SelectedIndex = 1;
                            xtb_fd_kt1.SelectedIndex = 1;
                            xtb_fd_kt1_js.SelectedIndex = 2;
                        }
                        #endregion
                        break;
                    case "2":
                        fd_kt1b.Text = _combox.Text;
                        fd_kt1b_js.Text = _combox.Text;
                        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                        if (kglxs.ContainsKey(point))
                        {
                            xtb_ws_kt1b_bs.Text = "";
                            xtb_ws_kt1b_bs.Items.Clear();
                            xtb_ws_kt1b_bs.Items.Add(kglxs[point][0]);
                            xtb_ws_kt1b_bs.Items.Add(kglxs[point][1]);
                            xtb_ws_kt1b_bs.Items.Add(kglxs[point][2]);

                            xtb_fd_kt1b.Text = "";
                            xtb_fd_kt1b.Items.Clear();
                            xtb_fd_kt1b.Items.Add(kglxs[point][0]);
                            xtb_fd_kt1b.Items.Add(kglxs[point][1]);
                            xtb_fd_kt1b.Items.Add(kglxs[point][2]);

                            xtb_fd_kt1b_js.Text = "";
                            xtb_fd_kt1b_js.Items.Clear();
                            xtb_fd_kt1b_js.Items.Add(kglxs[point][0]);
                            xtb_fd_kt1b_js.Items.Add(kglxs[point][1]);
                            xtb_fd_kt1b_js.Items.Add(kglxs[point][2]);
                            xtb_ws_kt1b_bs.SelectedIndex = 1;
                            xtb_fd_kt1b.SelectedIndex = 1;
                            xtb_fd_kt1b_js.SelectedIndex = 2;
                        }
                        #endregion
                        break;
                    case "3":
                        fd_kt3.Text = _combox.Text;
                        fd_kt3_js.Text = _combox.Text;
                        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                        if (kglxs.ContainsKey(point))
                        {
                            xtb_ws_kt3_bs.Text = "";
                            xtb_ws_kt3_bs.Items.Clear();
                            xtb_ws_kt3_bs.Items.Add(kglxs[point][0]);
                            xtb_ws_kt3_bs.Items.Add(kglxs[point][1]);
                            xtb_ws_kt3_bs.Items.Add(kglxs[point][2]);

                            xtb_fd_kt3.Text = "";
                            xtb_fd_kt3.Items.Clear();
                            xtb_fd_kt3.Items.Add(kglxs[point][0]);
                            xtb_fd_kt3.Items.Add(kglxs[point][1]);
                            xtb_fd_kt3.Items.Add(kglxs[point][2]);

                            xtb_fd_kt3_js.Text = "";
                            xtb_fd_kt3_js.Items.Clear();
                            xtb_fd_kt3_js.Items.Add(kglxs[point][0]);
                            xtb_fd_kt3_js.Items.Add(kglxs[point][1]);
                            xtb_fd_kt3_js.Items.Add(kglxs[point][2]);
                            xtb_ws_kt3_bs.SelectedIndex = 1;
                            xtb_fd_kt3.SelectedIndex = 1;
                            xtb_fd_kt3_js.SelectedIndex = 2;
                        }
                        #endregion
                        break;
                    case "4":
                        fd_kt3b.Text = _combox.Text;
                        fd_kt3b_js.Text = _combox.Text;
                        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                        if (kglxs.ContainsKey(point))
                        {
                            xtb_ws_kt3b_bs.Text = "";
                            xtb_ws_kt3b_bs.Items.Clear();
                            xtb_ws_kt3b_bs.Items.Add(kglxs[point][0]);
                            xtb_ws_kt3b_bs.Items.Add(kglxs[point][1]);
                            xtb_ws_kt3b_bs.Items.Add(kglxs[point][2]);

                            xtb_fd_kt3b.Text = "";
                            xtb_fd_kt3b.Items.Clear();
                            xtb_fd_kt3b.Items.Add(kglxs[point][0]);
                            xtb_fd_kt3b.Items.Add(kglxs[point][1]);
                            xtb_fd_kt3b.Items.Add(kglxs[point][2]);

                            xtb_fd_kt3b_js.Text = "";
                            xtb_fd_kt3b_js.Items.Clear();
                            xtb_fd_kt3b_js.Items.Add(kglxs[point][0]);
                            xtb_fd_kt3b_js.Items.Add(kglxs[point][1]);
                            xtb_fd_kt3b_js.Items.Add(kglxs[point][2]);
                            xtb_ws_kt3b_bs.SelectedIndex = 1;
                            xtb_fd_kt3b.SelectedIndex = 1;
                            xtb_fd_kt3b_js.SelectedIndex = 2;

                        }
                        #endregion
                        break;
                }
                //switch (_combox.Tag.ToString())
                //{
                //    case "1":
                //        fd_kt1.Text = _combox.Text;
                //        fd_kt1_js.Text = _combox.Text;

                //        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                //        if (kglxs.ContainsKey(_combox.Text))
                //        {
                //            xtb_ws_kt1_bs.Text = "";
                //            xtb_ws_kt1_bs.Items.Clear();
                //            xtb_ws_kt1_bs.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_ws_kt1_bs.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_ws_kt1_bs.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt1.Text = "";
                //            xtb_fd_kt1.Items.Clear();
                //            xtb_fd_kt1.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt1.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt1.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt1_js.Text = "";
                //            xtb_fd_kt1_js.Items.Clear();
                //            xtb_fd_kt1_js.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt1_js.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt1_js.Items.Add(kglxs[_combox.Text][2]);
                //            xtb_ws_kt1_bs.SelectedIndex = 1;
                //            xtb_fd_kt1.SelectedIndex = 1;
                //            xtb_fd_kt1_js.SelectedIndex = 2;
                //        }
                //        #endregion
                //        break;
                //    case "2":
                //        fd_kt1b.Text = _combox.Text;
                //        fd_kt1b_js.Text = _combox.Text;
                //        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                //        if (kglxs.ContainsKey(_combox.Text))
                //        {
                //            xtb_ws_kt1b_bs.Text = "";
                //            xtb_ws_kt1b_bs.Items.Clear();
                //            xtb_ws_kt1b_bs.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_ws_kt1b_bs.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_ws_kt1b_bs.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt1b.Text = "";
                //            xtb_fd_kt1b.Items.Clear();
                //            xtb_fd_kt1b.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt1b.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt1b.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt1b_js.Text = "";
                //            xtb_fd_kt1b_js.Items.Clear();
                //            xtb_fd_kt1b_js.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt1b_js.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt1b_js.Items.Add(kglxs[_combox.Text][2]);
                //            xtb_ws_kt1b_bs.SelectedIndex = 1;
                //            xtb_fd_kt1b.SelectedIndex = 1;
                //            xtb_fd_kt1b_js.SelectedIndex = 2;
                //        }
                //        #endregion
                //        break;
                //    case "3":
                //        fd_kt3.Text = _combox.Text;
                //        fd_kt3_js.Text = _combox.Text;
                //        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                //        if (kglxs.ContainsKey(_combox.Text))
                //        {
                //            xtb_ws_kt3_bs.Text = "";
                //            xtb_ws_kt3_bs.Items.Clear();
                //            xtb_ws_kt3_bs.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_ws_kt3_bs.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_ws_kt3_bs.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt3.Text = "";
                //            xtb_fd_kt3.Items.Clear();
                //            xtb_fd_kt3.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt3.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt3.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt3_js.Text = "";
                //            xtb_fd_kt3_js.Items.Clear();
                //            xtb_fd_kt3_js.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt3_js.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt3_js.Items.Add(kglxs[_combox.Text][2]);
                //            xtb_ws_kt3_bs.SelectedIndex = 1;
                //            xtb_fd_kt3.SelectedIndex = 1;
                //            xtb_fd_kt3_js.SelectedIndex = 2;
                //        }
                //        #endregion
                //        break;
                //    case "4":
                //        fd_kt3b.Text = _combox.Text;
                //        fd_kt3b_js.Text = _combox.Text;
                //        #region 加载0 1 2态显示 by tanxingyan 2014-12-11
                //        //if (kglxs.ContainsKey(_combox.Text))
                //        if (kglxs.ContainsKey(_combox.Text.Split('.')[0])) //2018.2.6 by
                //        {
                //            xtb_ws_kt3b_bs.Text = "";
                //            xtb_ws_kt3b_bs.Items.Clear();
                //            xtb_ws_kt3b_bs.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_ws_kt3b_bs.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_ws_kt3b_bs.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt3b.Text = "";
                //            xtb_fd_kt3b.Items.Clear();
                //            xtb_fd_kt3b.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt3b.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt3b.Items.Add(kglxs[_combox.Text][2]);

                //            xtb_fd_kt3b_js.Text = "";
                //            xtb_fd_kt3b_js.Items.Clear();
                //            xtb_fd_kt3b_js.Items.Add(kglxs[_combox.Text][0]);
                //            xtb_fd_kt3b_js.Items.Add(kglxs[_combox.Text][1]);
                //            xtb_fd_kt3b_js.Items.Add(kglxs[_combox.Text][2]);
                //            xtb_ws_kt3b_bs.SelectedIndex = 1;
                //            xtb_fd_kt3b.SelectedIndex = 1;
                //            xtb_fd_kt3b_js.SelectedIndex = 2;

                //        }
                //        #endregion
                //        break;
                //}
            }
            catch (Exception err) { XtraMessageBox.Show(err.Message); }
        }

        private void fd_ft_js_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;
                if (fd_ft_js.Text != "")
                {
                    fd_ft_tj.Enabled = true;
                    fd_ft_value.Enabled = true;

                    if (_combox.Text.IndexOf("A") != -1)
                    {
                        //模拟量
                        fd_ft_tj.Items.Clear();
                        fd_ft_tj.Items.Add(">");
                        fd_ft_tj.Items.Add(">=");
                        fd_ft_tj.Items.Add("<");
                        fd_ft_tj.Items.Add("<=");
                    }
                    else
                    {
                        //开关量
                        fd_ft_tj.Items.Clear();
                        fd_ft_tj.Items.Add("=");
                        fd_ft_tj.Text = "=";
                    }
                }
                else
                {
                    fd_ft_tj.Enabled = false;
                    fd_ft_value.Enabled = false;
                }
            }
            catch (Exception err) { XtraMessageBox.Show(err.Message + err.Source); }
        }

        private void fd_ftb_js_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox _combox = (System.Windows.Forms.ComboBox)sender;
                if (fd_ftb_js.Text != "")
                {
                    fd_ftb_tj.Enabled = true;
                    fd_ftb_value.Enabled = true;

                    if (_combox.Text.IndexOf("A") != -1)
                    {
                        //模拟量
                        fd_ftb_tj.Items.Clear();
                        //fd_ftb_tj.Items.Add("=");
                        fd_ftb_tj.Items.Add(">");
                        fd_ftb_tj.Items.Add(">=");
                        fd_ftb_tj.Items.Add("<");
                        fd_ftb_tj.Items.Add("<=");
                    }
                    else
                    {
                        //开关量
                        fd_ftb_tj.Items.Clear();
                        fd_ftb_tj.Items.Add("=");
                        fd_ftb_tj.Text = "=";
                    }
                }
                else
                {
                    fd_ftb_tj.Enabled = false;
                    fd_ftb_value.Enabled = false;
                }
            }
            catch (Exception err) { XtraMessageBox.Show(err.Message + err.Source); }
        }

        private void cmb_t1_DropDown(object sender, EventArgs e)
        {
            try
            {
                cmb_t1.Items.Clear();
                cmb_t1.Items.AddRange(Mnl_array);
                if (cmb_t2.SelectedIndex > 0)
                {
                    cmb_t1.Items.Remove(cmb_t2.Text.ToString());

                }
                if (ws_ftfl_bs.Enabled)
                {
                    if (ws_ftfl_bs.SelectedIndex > 0)
                    {
                        cmb_t1.Items.Remove(ws_ftfl_bs.Text.ToString());
                    }
                    if (ws_ftflb_bs.SelectedIndex > 0)
                    {
                        cmb_t1.Items.Remove(ws_ftflb_bs.Text.ToString());
                    }
                }
                cmb_t1.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void cmb_t2_DropDown(object sender, EventArgs e)
        {
            try
            {
                cmb_t2.Items.Clear();
                cmb_t2.Items.AddRange(Mnl_array);
                if (cmb_t1.SelectedIndex > 0)
                {
                    cmb_t2.Items.Remove(cmb_t1.Text.ToString());

                }
                if (ws_ftfl_bs.Enabled)
                {
                    if (ws_ftfl_bs.SelectedIndex > 0)
                    {
                        cmb_t2.Items.Remove(ws_ftfl_bs.Text.ToString());
                    }
                    if (ws_ftflb_bs.SelectedIndex > 0)
                    {
                        cmb_t2.Items.Remove(ws_ftflb_bs.Text.ToString());
                    }
                }
                cmb_t2.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_kt1_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_kt1_bs.Items.Clear();
                ws_kt1_bs.Items.AddRange(Kgl_array);
                if (ws_kt1b_bs.SelectedIndex > 0)
                {
                    ws_kt1_bs.Items.Remove(ws_kt1b_bs.Text.ToString());
                }

                if (ws_kt3_bs.SelectedIndex > 0)
                {
                    ws_kt1_bs.Items.Remove(ws_kt3_bs.Text.ToString());
                }

                if (ws_kt3b_bs.SelectedIndex > 0)
                {
                    ws_kt1_bs.Items.Remove(ws_kt3b_bs.Text.ToString());
                }
                ws_kt1_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_kt1b_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_kt1b_bs.Items.Clear();
                ws_kt1b_bs.Items.AddRange(Kgl_array);
                if (ws_kt1_bs.SelectedIndex > 0)
                {
                    ws_kt1b_bs.Items.Remove(ws_kt1_bs.Text.ToString());
                }

                if (ws_kt3_bs.SelectedIndex > 0)
                {
                    ws_kt1b_bs.Items.Remove(ws_kt3_bs.Text.ToString());
                }

                if (ws_kt3b_bs.SelectedIndex > 0)
                {
                    ws_kt1b_bs.Items.Remove(ws_kt3b_bs.Text.ToString());
                }
                ws_kt1b_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_kt3_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_kt3_bs.Items.Clear();
                ws_kt3_bs.Items.AddRange(Kgl_array);
                if (ws_kt1b_bs.SelectedIndex > 0)
                {
                    ws_kt3_bs.Items.Remove(ws_kt1b_bs.Text.ToString());
                }
                if (ws_kt1_bs.SelectedIndex > 0)
                {
                    ws_kt3_bs.Items.Remove(ws_kt1_bs.Text.ToString());
                }
                if (ws_kt3b_bs.SelectedIndex > 0)
                {
                    ws_kt3_bs.Items.Remove(ws_kt3b_bs.Text.ToString());
                }
                ws_kt3_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_kt3b_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_kt3b_bs.Items.Clear();
                ws_kt3b_bs.Items.AddRange(Kgl_array);
                if (ws_kt1b_bs.SelectedIndex > 0)
                {
                    ws_kt3b_bs.Items.Remove(ws_kt1b_bs.Text.ToString());
                }
                if (ws_kt3_bs.SelectedIndex > 0)
                {
                    ws_kt3b_bs.Items.Remove(ws_kt3_bs.Text.ToString());
                }
                if (ws_kt1_bs.SelectedIndex > 0)
                {
                    ws_kt3b_bs.Items.Remove(ws_kt1_bs.Text.ToString());
                }
                ws_kt3b_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_ftfl_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_ftfl_bs.Items.Clear();
                ws_ftfl_bs.Items.AddRange(Mnl_array);
                if (ws_ftflb_bs.SelectedIndex > 0)
                {
                    ws_ftfl_bs.Items.Remove(ws_ftflb_bs.Text.ToString());
                }
                if (cmb_t1.SelectedIndex > 0)
                {
                    ws_ftfl_bs.Items.Remove(cmb_t1.Text.ToString());
                }
                if (cmb_t2.SelectedIndex > 0)
                {
                    ws_ftfl_bs.Items.Remove(cmb_t2.Text.ToString());
                }
                ws_ftfl_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void ws_ftflb_bs_DropDown(object sender, EventArgs e)
        {
            try
            {
                ws_ftflb_bs.Items.Clear();
                ws_ftflb_bs.Items.AddRange(Mnl_array);
                if (ws_ftfl_bs.SelectedIndex > 0)
                {
                    ws_ftflb_bs.Items.Remove(ws_ftfl_bs.Text.ToString());
                }
                if (cmb_t1.SelectedIndex > 0)
                {
                    ws_ftflb_bs.Items.Remove(cmb_t1.Text.ToString());
                }
                if (cmb_t2.SelectedIndex > 0)
                {
                    ws_ftflb_bs.Items.Remove(cmb_t2.Text.ToString());
                }
                ws_ftflb_bs.Sorted = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        #endregion

        #region  ==========================业务函数 ==========================
        /// <summary>
        /// 设置对应checkBox的勾选状态 by huangxxUP 2013-12-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangEnableA(object sender, EventArgs e)
        {
            //try
            //{
            //    CheckBox cb = sender as CheckBox;

            //    if (null == cb) { return; }
            //    //选择任意一个的checkBox让另一个checkBox对应项变为不启用 
            //    //check.ChangeCheckEnable(cb.Name, cb.CheckState == CheckState.Checked ? true : false);

            //    if (cb.Checked)
            //    {
            //        byte checkedKzk = oneKzk(cb.Name);
            //        //如果即将勾选的控制口已经设置了本地控制或者交叉控制，则给出提示
            //        if ((checkedKzk & mkkzk) > 0)
            //        {
            //            ck_ws_kzk.CheckValue = MasCheck_ws;
            //            MessageBox.Show("该控制口已经设置本地控制或交叉控制或手动控制！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            //        }
            //        else
            //        {
            //            MasCheck_ws = ck_ws_kzk.CheckValue;
            //        }

            //        //选择任意一个的checkBox让另一个checkBox对应项变为不选择
            //        if (cb.CheckState == CheckState.Checked)
            //        {
            //            ck_fdbs_kzk.ChangeCheckState(cb.Name, CheckState.Unchecked);
            //        }
            //    }
            //    else
            //    {
            //        MasCheck_ws = ck_ws_kzk.CheckValue;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MFuncClass.SysClass.WriteLog(ex.Message, "错误Fdbs_ChangEnableA");
            //}

        }
        /// <summary>
        /// 设置对应checkBox的勾选状态 by huangxxUP 2013-12-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangEnableB(object sender, EventArgs e)
        {
            //try
            //{
            //    CheckBox cb = sender as CheckBox;

            //    if (null == cb) { return; }
            //    //选择任意一个的checkBox让另一个checkBox对应项变为不启用 
            //    //check.ChangeCheckEnable(cb.Name, cb.CheckState == CheckState.Checked ? true : false);

            //    //if (cb.Checked)
            //    //{
            //    //    byte checkedKzk = oneKzk(cb.Name);
            //    //    //如果即将勾选的控制口已经设置了本地控制或者交叉控制，则给出提示
            //    //    if ((checkedKzk & mkkzk) > 0)
            //    //    {
            //    //        ck_fdbs_kzk.CheckValue = MasCheck_fd;
            //    //        MessageBox.Show("该控制口已经设置本地控制或者交叉控制！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            //    //    }
            //    //    else
            //    //    {
            //    //        MasCheck_fd = ck_fdbs_kzk.CheckValue;
            //    //    }

            //    //选择任意一个的checkBox让另一个checkBox对应项变为不选择
            //    if (cb.CheckState == CheckState.Checked)
            //    {
            //        ck_ws_kzk.ChangeCheckState(cb.Name, CheckState.Unchecked);
            //    }
            //    //}
            //    //else
            //    //{
            //    //    MasCheck_fd = ck_fdbs_kzk.CheckValue;
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    MFuncClass.SysClass.WriteLog(ex.Message, "错误Fdbs_ChangEnableB");
            //}
        }
        /// <summary>
        /// 获取控制口(交叉控制和本地控制和手动控制) by huangxxUP 2013-12-28
        /// </summary>
        private void GetKzk()
        {
            #region
            ////获取本分站下所有口号的本地控制口和其他分站的交叉控制口
            ////获取本分站下所有的模开量
            //string mkl = string.Empty;
            ////除本分站的交叉控制测点号
            //string jckzPoints = string.Empty;
            //DataTable dt = Swap.sqlc.GetDefTable();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["fzh"].ToString() == Fz.ToString())
            //    {
            //        if (dt.Rows[i]["point"].ToString().IndexOf("A") != -1 || dt.Rows[i]["point"].ToString().IndexOf("D") != -1)
            //        {
            //            mkl += dt.Rows[i]["point"].ToString() + ",";
            //        }
            //    }
            //}
            //mkl = mkl.TrimEnd(',');
            //if (!string.IsNullOrEmpty(mkl))
            //{
            //    //根据模开量找对应的本地控制口
            //    for (int i = 0; i < mkl.Split(',').Length; i++)
            //    {
            //        dt = Swap.sqlc.GetDEF(mkl.Split(',')[i].ToString());
            //        if (mkl.Split(',')[i].ToString().IndexOf("A") != -1)
            //        {
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k1"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k2"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k3"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k4"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k5"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k6"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k7"].ToString());
            //        }
            //        else
            //        {
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k1"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k2"].ToString());
            //            mkkzk |= Convert.ToByte(dt.Rows[0]["k3"].ToString());
            //        }
            //        //如果1-8号控制口都已勾选，则跳出循环
            //        if (mkkzk == 255)
            //        {
            //            return;
            //        }
            //    }
            //}
            //mkl = string.Empty;
            ////如果控制口小于255则继续判断交叉控制口和手动控制
            //if (mkkzk < 255)
            //{
            //    dt = Swap.sqlc.GetDefTable();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        if (dt.Rows[i]["point"].ToString().IndexOf("A") != -1)
            //        {
            //            if (dt.Rows[i]["fzh"].ToString() != Fz.ToString())
            //            {
            //                mkl += dt.Rows[i]["point"].ToString() + ",";
            //            }
            //        }
            //        else if (dt.Rows[i]["point"].ToString().IndexOf("D") != -1)
            //        {
            //            mkl += dt.Rows[i]["point"].ToString() + ",";
            //        }

            //    }
            //    mkl = mkl.TrimEnd(',');
            //    if (!string.IsNullOrEmpty(mkl))
            //    {
            //        //根据模开量找对应的交叉控制口
            //        for (int i = 0; i < mkl.Split(',').Length; i++)
            //        {
            //            dt = Swap.sqlc.GetDEF(mkl.Split(',')[i].ToString());

            //            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz1"].ToString()))
            //            {
            //                if (dt.Rows[0]["jckz1"].ToString().Contains("|"))
            //                {
            //                    for (int j = 0; j < dt.Rows[0]["jckz1"].ToString().Split('|').Length; j++)
            //                    {
            //                        if (!string.IsNullOrEmpty(dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString()))
            //                        {
            //                            if ((dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString()) == -1)
            //                            {
            //                                jckzPoints += dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString() + "|";
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if ((dt.Rows[0]["jckz1"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz1"].ToString()) == -1)
            //                    {
            //                        jckzPoints += dt.Rows[0]["jckz1"].ToString() + "|";
            //                    }
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz2"].ToString()))
            //            {
            //                if (dt.Rows[0]["jckz2"].ToString().Contains("|"))
            //                {
            //                    for (int j = 0; j < dt.Rows[0]["jckz2"].ToString().Split('|').Length; j++)
            //                    {
            //                        if (!string.IsNullOrEmpty(dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString()))
            //                        {
            //                            if ((dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString()) == -1)
            //                            {
            //                                jckzPoints += dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString() + "|";
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if ((dt.Rows[0]["jckz2"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz2"].ToString()) == -1)
            //                    {
            //                        jckzPoints += dt.Rows[0]["jckz2"].ToString() + "|";
            //                    }
            //                }
            //            }
            //            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz3"].ToString()))
            //            {
            //                if (dt.Rows[0]["jckz3"].ToString().Contains("|"))
            //                {
            //                    for (int j = 0; j < dt.Rows[0]["jckz3"].ToString().Split('|').Length; j++)
            //                    {
            //                        if (!string.IsNullOrEmpty(dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString()))
            //                        {
            //                            if ((dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString()) == -1)
            //                            {
            //                                jckzPoints += dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString() + "|";
            //                            }
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    if ((dt.Rows[0]["jckz3"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz3"].ToString()) == -1)
            //                    {
            //                        jckzPoints += dt.Rows[0]["jckz3"].ToString() + "|";
            //                    }
            //                }
            //            }

            //        }
            //        jckzPoints = jckzPoints.TrimEnd('|');
            //        if (!string.IsNullOrEmpty(jckzPoints))
            //        {
            //            for (int i = 0; i < jckzPoints.Split('|').Length; i++)
            //            {
            //                switch (Convert.ToInt32(jckzPoints.Split('|')[i].ToString().Substring(4, 2)))
            //                {
            //                    case 1:
            //                        mkkzk |= 0x01;
            //                        break;
            //                    case 2:
            //                        mkkzk |= 0x02;
            //                        break;
            //                    case 3:
            //                        mkkzk |= 0x04;
            //                        break;
            //                    case 4:
            //                        mkkzk |= 0x08;
            //                        break;
            //                    case 5:
            //                        mkkzk |= 0x10;
            //                        break;
            //                    case 6:
            //                        mkkzk |= 0x20;
            //                        break;
            //                    case 7:
            //                        mkkzk |= 0x40;
            //                        break;
            //                    case 8:
            //                        mkkzk |= 0x80;
            //                        break;
            //                    default:
            //                        break;
            //                }
            //                //如果1-8号控制口都已勾选，则跳出循环
            //                if (mkkzk == 255)
            //                {
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    //获取本分站的手动控制
            //    mkkzk |= Swap.fz[Fz - 1].kzzt;
            //}
            #endregion
        }
        /// <summary>
        /// 下拉列表清空
        /// </summary>
        private void ComBoxClear()
        {
            try
            {
                cmb_t1.Items.Clear(); cmb_t2.Items.Clear();
                ws_kt1_bs.Items.Clear(); ws_kt1b_bs.Items.Clear(); ws_kt3_bs.Items.Clear(); ws_kt3b_bs.Items.Clear();
                ws_t1_bs.Items.Clear(); ws_t2_bs.Items.Clear(); ws_t1_js.Items.Clear(); ws_t2_js.Items.Clear();
                fd_kt1.Items.Clear(); fd_kt1b.Items.Clear(); fd_kt3.Items.Clear(); fd_kt3b.Items.Clear();
                ws_ftfl_bs.Items.Clear(); ws_ftflb_bs.Items.Clear();
                ws_ftfl_tj.Items.Clear(); ws_ftflb_tj.Items.Clear();
                ws_ftfl_value.Text = ""; ws_ftflb_value.Text = "";
            }
            catch { }
        }
        /// <summary>
        /// 下拉列表初始化
        /// </summary>
        private void ComBoxInit()
        {
            try
            {
                cmb_t1.Items.AddRange(Mnl_array); cmb_t2.Items.AddRange(Mnl_array);
                CcmbPowerOFFControlT1.Text = "";
                CcmbPowerOFFControlT2.Text = "";

                ws_kt1_bs.Items.AddRange(Kgl_array); ws_kt1b_bs.Items.AddRange(Kgl_array); ws_kt3_bs.Items.AddRange(Kgl_array); ws_kt3b_bs.Items.AddRange(Kgl_array);

                fd_kt1.Items.AddRange(Kgl_array); fd_kt3.Items.AddRange(Kgl_array);
                fd_kt1b.Items.AddRange(Kgl_array); fd_kt3b.Items.AddRange(Kgl_array);

                fd_kt1_js.Items.AddRange(Kgl_array); fd_kt1b_js.Items.AddRange(Kgl_array);
                fd_kt3_js.Items.AddRange(Kgl_array); fd_kt3b_js.Items.AddRange(Kgl_array);
                //风筒风量屏蔽开关量 2014-1-2 huangxx
                ws_ftfl_bs.Items.AddRange(Mnl_array); ws_ftflb_bs.Items.AddRange(Mnl_array);
                fd_ft_js.Items.AddRange(MnKg_array); fd_ftb_js.Items.AddRange(MnKg_array);
            }
            catch { }
        }
        private string[] PointLoad(List<string> _list, ref string[] _value)
        {
            try
            {
                _value = new string[_list.Count + 1];
                _value[0] = "";
                for (int i = 0; i < _list.Count; i++)
                {
                    _value[i + 1] = _list[i];
                }
            }
            catch { }
            return _value;
        }
        /// <summary>
        /// 输入校验
        /// </summary>
        /// <returns></returns>
        private bool CheckInput()
        {
            bool value = false;

            if (cmb_t1.Text == "" || cmb_t2.Text == "")
            {
                value = false;
                XtraMessageBox.Show("甲烷选择不完整!");
            }
            else
            {

            }

            return value;
        }
        /// <summary>
        /// 单风机逻辑判断
        /// </summary>
        private void FJ_One()
        {
            if (rb_one.Checked)
            {
                ws_kt3_bs.Text = "";
                ws_kt3_bs.Visible = false; lb_kt3_bs.Visible = false; lb_kt3_bs_d.Visible = false;
                xtb_ws_kt3_bs.Visible = false; //by tanxingyan 2014-12-11
                ws_kt3b_bs.Text = "";
                ws_kt3b_bs.Visible = false; lb_kt3b_bs.Visible = false; lb_kt3b_bs_d.Visible = false;
                xtb_ws_kt3b_bs.Visible = false; //by tanxingyan 2014-12-11
                fd_kt3.Text = "";
                fd_kt3.Visible = false; lb_fd_kt3.Visible = false; lb_fd_kt3_d.Visible = false;
                xtb_fd_kt3.Visible = false; //by tanxingyan 2014-12-11
                fd_kt3b.Text = "";
                fd_kt3b.Visible = false; lb_fd_kt3b.Visible = false; lb_fd_kt3b_d.Visible = false;
                xtb_fd_kt3b.Visible = false; //by tanxingyan 2014-12-11

                fd_kt3_js.Text = "";
                fd_kt3b_js.Text = "";
                panel_fd_js.Visible = false;

                // xuzpUP 20150319
                if (label78.Text != "主风机开停(备):")
                {
                    label78.Text = "主风机开停(备):";
                    label78.Location = new Point(label78.Location.X, label78.Location.Y);
                    label33.Text = "主风机开停(备):";
                    label33.Location = new Point(label33.Location.X, label33.Location.Y);
                    groupBox11.Text = "主风机开停(备):";
                }
            }
        }
        /// <summary>
        /// 单风机逻辑判断
        /// </summary>
        private void FJ_Two()
        {
            if (rb_two.Checked)
            {
                ws_kt3_bs.Text = "";
                ws_kt3_bs.Visible = true; lb_kt3_bs.Visible = true; lb_kt3_bs_d.Visible = true;
                xtb_ws_kt3_bs.Visible = true;//by tanxingyan 2014-12-11
                ws_kt3b_bs.Text = "";
                ws_kt3b_bs.Visible = true; lb_kt3b_bs.Visible = true; lb_kt3b_bs_d.Visible = true;
                xtb_ws_kt3b_bs.Visible = true;//by tanxingyan 2014-12-11

                fd_kt3.Text = "";
                fd_kt3.Visible = true; lb_fd_kt3.Visible = true; lb_fd_kt3_d.Visible = true;
                xtb_fd_kt3.Visible = true;//by tanxingyan 2014-12-11
                fd_kt3b.Text = "";
                fd_kt3b.Visible = true; lb_fd_kt3b.Visible = true; lb_fd_kt3b_d.Visible = true;
                xtb_fd_kt3b.Visible = true;//by tanxingyan 2014-12-11

                fd_kt3_js.Text = "";
                fd_kt3b_js.Text = "";
                panel_fd_js.Visible = true;

                // xuzpUP 20150319
                if (label78.Text != "主风机开停(备):")
                {
                    label78.Text = "主风机开停(备):";
                    label78.Location = new Point(label78.Location.X, label78.Location.Y);
                    label33.Text = "主风机开停(备):";
                    label33.Location = new Point(label33.Location.X, label33.Location.Y);
                    groupBox11.Text = "主风机开停(备):";
                }
            }
        }
        /// <summary>
        /// 双风机双开停显示处理函数 xuzpUP 20150319
        /// </summary>
        private void FJ_Two_TwoKT()
        {
            if (rb_three.Checked)
            {
                ws_kt3_bs.Text = "";
                ws_kt3_bs.Visible = false; lb_kt3_bs.Visible = false; lb_kt3_bs_d.Visible = false; xtb_ws_kt3_bs.Visible = false;
                ws_kt3b_bs.Text = "";
                ws_kt3b_bs.Visible = false; lb_kt3b_bs.Visible = false; lb_kt3b_bs_d.Visible = false; xtb_ws_kt3b_bs.Visible = false;

                fd_kt3.Text = "";
                fd_kt3.Visible = false; lb_fd_kt3.Visible = false; lb_fd_kt3_d.Visible = false; xtb_fd_kt3.Visible = false;
                fd_kt3b.Text = "";
                fd_kt3b.Visible = false; lb_fd_kt3b.Visible = false; lb_fd_kt3b_d.Visible = false; xtb_fd_kt3b.Visible = false;

                fd_kt3_js.Text = "";
                fd_kt3b_js.Text = "";
                panel_fd_js.Visible = false;

                if (label78.Text != "副风机开停")
                {
                    label78.Text = "副风机开停:";
                    label78.Location = new Point(label78.Location.X, label78.Location.Y);
                    label33.Text = "副风机开停:";
                    label33.Location = new Point(label33.Location.X, label33.Location.Y);
                    groupBox11.Text = "副风机开停:";
                }

            }
        }
        /// <summary>
        /// 根据传入的数据运算符,返回对应的值
        /// </summary>
        /// <param name="express">运算符</param>
        private int calculateValue(string express)
        {
            int value = -1;
            switch (express)
            {
                case "=": value = 1; break;
                case ">": value = 2; break;
                case ">=": value = 3; break;
                case "<": value = 4; break;
                case "<=": value = 5; break;
            }

            return value;
        }
        /// <summary>
        /// 输入值完整及正确校验
        /// </summary>
        private bool valueCheck()
        {
            bool value = true;
            double mnlvalue = 0;
            try
            {
                #region 判断风筒风量值 huangxxUP 2013-12-28
                if (cb_ftfl_mk.Checked)
                {
                    //if (!SqlClass.NumValidata(ws_ftfl_value.Text.Trim().ToString(), 1) || !SqlClass.NumValidata(ws_ftflb_value.Text.Trim().ToString(), 1) || !SqlClass.NumValidata(fd_ft_value.Text.Trim().ToString(), 1) || !SqlClass.NumValidata(fd_ftb_value.Text.Trim().ToString(), 1))
                    //{
                    //    value = false;
                    //    XtraMessageBox.Show("风筒风量值输入有误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}
                }
                #endregion

                #region 新增对甲烷风电闭锁断电区域的判断 by huangxxUP 2013-12-28
                if (string.IsNullOrEmpty(cCmbControlWindBreakCH4.Text))
                {
                    value = false;
                    XtraMessageBox.Show("甲烷风电闭锁区域未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                if (!string.IsNullOrEmpty(cCmbControlWindBreakCH4.Text))
                {
                    if (!string.IsNullOrEmpty(cCmbControlWindBreak.Text))
                    {
                        if (cCmbControlWindBreakCH4.Text.Contains(cCmbControlWindBreak.Text) || cCmbControlWindBreak.Text.Contains(cCmbControlWindBreakCH4.Text))
                        {
                            value = false;
                            XtraMessageBox.Show("甲烷风电闭锁区域 和 风电闭锁区域 不能相同!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
                #endregion
                if (cmb_t1.SelectedIndex <= 0 || cmb_t2.SelectedIndex <= 0)
                {
                    value = false;
                    XtraMessageBox.Show("瓦斯信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

                if (ws_kt1_bs.SelectedIndex <= 0 || ws_kt1b_bs.SelectedIndex <= 0)
                {
                    value = false;
                    XtraMessageBox.Show("开停信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }


                if (fd_logic_1.Text == "")
                {
                    value = false;
                    XtraMessageBox.Show("开停信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

                if (rb_one.Checked || rb_three.Checked) //xuzp 20150324
                {

                }
                else
                {
                    if (ws_kt3_bs.SelectedIndex <= 0 || ws_kt3b_bs.SelectedIndex <= 0)
                    {
                        value = false;
                        XtraMessageBox.Show("开停信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }

                    if (fd_logic_2.Text == "")
                    {
                        value = false;
                        XtraMessageBox.Show("开停信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }

                if (cb_ftfl_mk.Checked)
                {
                    if (ws_ftfl_bs.SelectedIndex <= 0 ||
                        ws_ftflb_bs.SelectedIndex <= 0 ||
                        ws_ftfl_tj.SelectedIndex == -1 ||
                        ws_ftflb_tj.SelectedIndex == -1 ||
                        ws_ftfl_logic.SelectedIndex == -1 ||
                        ws_ftfl_value.Text == "" ||
                        ws_ftflb_value.Text == "" ||
                        fd_ft_tj.SelectedIndex == -1 ||
                        fd_ftb_tj.SelectedIndex == -1 ||
                        fd_ft_value.Text == "" ||
                        fd_ftb_value.Text == "" ||
                        fd_logic_3.SelectedIndex == -1
                        )
                    {
                        value = false;
                        XtraMessageBox.Show("风筒风量信息不完整!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }

                    if (ws_ftfl_bs.Text == cmb_t1.Text || ws_ftfl_bs.Text == cmb_t2.Text)
                    {
                        value = false;
                        XtraMessageBox.Show("风筒风量不能为T1，T2对应口!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }

                    if (ws_ftflb_bs.Text == cmb_t1.Text || ws_ftflb_bs.Text == cmb_t2.Text)
                    {
                        value = false;
                        XtraMessageBox.Show("风筒风量不能为T1，T2对应口!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }

                    if (ws_ftfl_bs.SelectedIndex != -1)
                    {
                        try
                        {
                            mnlvalue = Convert.ToDouble(ws_ftfl_value.Text);
                        }
                        catch
                        {
                            value = false;
                        }

                        if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                        {
                            if (mnlvalue < 0)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            if (mnlvalue < 0 || mnlvalue > 2)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }

                    if (ws_ftflb_bs.SelectedIndex != -1)
                    {
                        try
                        {
                            mnlvalue = Convert.ToDouble(ws_ftflb_value.Text);
                        }
                        catch
                        {
                            value = false;
                        }

                        if (ws_ftflb_bs.Text.IndexOf("A") != -1)
                        {
                            if (mnlvalue < 0)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            if (mnlvalue < 0 || mnlvalue > 2)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }

                    if (fd_ft_js.SelectedIndex != -1)
                    {
                        try
                        {
                            mnlvalue = Convert.ToDouble(fd_ft_value.Text);
                        }
                        catch
                        {
                            value = false;
                        }

                        if (fd_ft_js.Text.IndexOf("A") != -1)
                        {
                            if (mnlvalue < 0)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            if (mnlvalue < 0 || mnlvalue > 2)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }

                    if (fd_ftb_js.SelectedIndex != -1)
                    {
                        try
                        {
                            mnlvalue = Convert.ToDouble(fd_ftb_value.Text);
                        }
                        catch
                        {
                            value = false;
                        }

                        if (fd_ftb_js.Text.IndexOf("A") != -1)
                        {
                            if (mnlvalue < 0)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            if (mnlvalue < 0 || mnlvalue > 2)
                            {
                                value = false;
                                XtraMessageBox.Show("风筒风量取值错误!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }

                    if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                    {
                        if (ws_ftflb_bs.Text.IndexOf("A") == -1)
                        {
                            value = false;
                            XtraMessageBox.Show("风筒风量类型必须相同!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }

                    if (ws_ftfl_bs.Text.IndexOf("D") != -1)
                    {
                        if (ws_ftflb_bs.Text.IndexOf("D") == -1)
                        {
                            value = false;
                            XtraMessageBox.Show("风筒风量类型必须相同!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }

                    if (ws_ftfl_bs.Text != "")
                    {
                        if (ws_ftfl_value.Text == fd_ft_value.Text)
                        {
                            value = false;
                            XtraMessageBox.Show("风筒风量类解锁与闭锁值不能相同!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
            catch
            {
            }

            return value;
        }
        /// <summary>
        /// 根据MasCheck单选框返回单点控制口号(byte) by huangxxUP 2013-12-28
        /// </summary>
        /// <param name="kzkName"></param>
        /// <returns></returns>
        private byte oneKzk(string kzkName)
        {
            byte checkedKzk = 0;
            switch (kzkName)
            {
                case "ck1":
                    checkedKzk = 1;
                    break;
                case "ck2":
                    checkedKzk = 2;
                    break;
                case "ck3":
                    checkedKzk = 4;
                    break;
                case "ck4":
                    checkedKzk = 8;
                    break;
                case "ck5":
                    checkedKzk = 16;
                    break;
                case "ck6":
                    checkedKzk = 32;
                    break;
                case "ck7":
                    checkedKzk = 64;
                    break;
                case "ck8":
                    checkedKzk = 128;
                    break;
                default:
                    break;
            }
            return checkedKzk;
        }
        /// <summary>
        /// 双风机（双开停）切换按钮 xuzpUP 20150319
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rb_three_CheckedChanged(object sender, EventArgs e)
        {
            FJ_Two_TwoKT();
        }
        /// <summary>
        /// 设置控制显示值 KJ306_F
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        //private string SetControlTextForKJ306_F(UInt16 K, string PointList)
        //{
        //    string temp = "";
        //    if (K > 0)
        //    {
        //        for (int i = 0; i < 16; i++)
        //        {
        //            if (((K >> i) & 0x1) == 0x1)
        //            {
        //                //if (i < 2)
        //                //{
        //                string[] pointArray = PointList.Split(',');
        //                foreach (string point in pointArray)
        //                {
        //                    if (point.IndexOf(tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0')) == 0)
        //                    {
        //                        temp += tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";
        //                    }
        //                }

        //                //}
        //                //else
        //                //{
        //                //    temp += tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "1" + ",";
        //                //}
        //            }
        //        }
        //        if (temp.Contains(","))
        //        {
        //            temp = temp.Substring(0, temp.Length - 1);
        //        }
        //    }
        //    return temp;
        //}

        /// <summary>
        /// 设置控制显示值 KJF86N(16)
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private string SetControlText(UInt16 K, string PointList)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        //temp += tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";
                        string[] pointArray = PointList.Split(',');
                        foreach (string point in pointArray)
                        {
                            if (point.IndexOf(tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0')) == 0)
                            {
                                temp += point + ",";
                            }
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
        /// 通过控制文本得到控制字
        /// </summary>
        /// <returns></returns>
        private UInt16 GetControlValueForKJ306_F(string ControlText)
        {
            UInt16 temp = 0;
            if (string.IsNullOrEmpty(ControlText))
            {
                return temp;
            }
            string[] tempArray = ControlText.Split(',');
            if (tempArray.Length <= 0)
            {
                return temp;
            }
            UInt16 Channel = 0;
            for (int i = 0; i < tempArray.Length; i++)
            {
                Channel = 0;
                if (tempArray[i].Trim().Length >= 6)
                {
                    Channel = Convert.ToUInt16(tempArray[i].Trim().Substring(4, 2));
                }
                if (Channel > 0)
                {
                    temp |= (UInt16)(1 << (Channel - 1));
                }
            }
            return temp;
        }
        /// <summary>
        /// 加载默认控制值
        /// </summary>
        private void DefaultControlValueLoad()
        {
            IList<Jc_DefInfo> TempControl = Model.DEFServiceModel.QueryPointByInfs(tempStation.Fzh, 3);
            IList<Jc_JcsdkzInfo> TempJCJCSDKZDTO;
            if (TempControl == null)
            {
                return;
            }
            if (TempControl.Count <= 0)
            {
                return;
            }
            cCmbControlWindBreakCH4.Properties.Items.Clear();
            cCmbControlWindBreak.Properties.Items.Clear();
            foreach (var item in TempControl)
            {
                cCmbControlWindBreakCH4.Properties.Items.Add(item.Point);
                cCmbControlWindBreak.Properties.Items.Add(item.Point);
                if (item.Kh == 1 || item.Kh == 2) //地址号为1-2的智能断电器不能作为控制口
                {
                    if (item.Dzh > 0)
                    {
                        cCmbControlWindBreakCH4.Properties.Items[item.Point].Enabled = false;
                        cCmbControlWindBreak.Properties.Items[item.Point].Enabled = false;
                    }
                }
                if (!Model.RelateUpdate.CheckControlEnable(item))
                {
                    cCmbControlWindBreakCH4.Properties.Items[item.Point].Enabled = false;
                    //cCmbControlWindBreak.Properties.Items[item.Point].Enabled = false;//风电闭锁不限制  20171018
                }
                TempJCJCSDKZDTO = Model.JCSDKZServiceModel.QueryJCSDKZbyInf(item.Point);
                if (null != TempJCJCSDKZDTO)
                {
                    if (TempJCJCSDKZDTO.Count > 0)
                    {
                        cCmbControlWindBreakCH4.Properties.Items[item.Point].Enabled = false;
                        //cCmbControlWindBreak.Properties.Items[item.Point].Enabled = false;//风电闭锁不限制  20171018
                    }
                }
            }
        }
        /// <summary>
        /// 设置本地控制信息
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private string SetLocalControlText(int K, string PointList)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        //if (i < 8)
                        //{

                        //    temp += tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";

                        //}
                        //else
                        //{
                        //    temp += tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1" + ",";
                        //}
                        string[] pointArray = PointList.Split(',');
                        foreach (string point in pointArray)
                        {
                            if (point.IndexOf(tempStation.Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0')) == 0)
                            {
                                temp += point + ",";
                            }
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
        #endregion

        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int fzh = tempStation.Fzh - 1;
                int dy_byte = 0;
                StringBuilder sb = new StringBuilder();
                int mn1_hz = 0;
                int mn2_hz = 0;
                int value = -1;
                Jc_DefInfo tempPoint = null;
                Jc_DevInfo tempDEV = null;
                string tempstring;
                int kh_t1 = 0, kh_t2 = 0, kh_kt1 = 0, kh_kt1b = 0, kh_kt3 = 0, kh_kt3b = 0, kh_ftfl = 0, kh_ftflb = 0;

                if (!valueCheck())
                {
                    return;
                }

                #region ---单双风机标志 //xuzpUP 20150116 处理双风机两个开停的问题---
                //双单风机标志
                if (rb_one.Checked)
                {
                    sb.Append("1,");
                }
                else if (rb_two.Checked)
                {
                    sb.Append("2,");
                }
                else
                {
                    sb.Append("3,");
                }
                #endregion

                #region ---口号取值---
                try
                {
                    kh_ftfl = 0; kh_ftflb = 0;
                    tempstring = cmb_t1.Text.Split('.')[0];
                    kh_t1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_t1 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = cmb_t2.Text.Split('.')[0];
                    kh_t2 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_t2 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = ws_kt1_bs.Text.Split('.')[0];
                    kh_kt1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_kt1 = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    tempstring = ws_kt1b_bs.Text.Split('.')[0];
                    kh_kt1b = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                    if (tempstring.Length > 6)
                    {
                        kh_kt1b = Convert.ToInt16(tempstring.Substring(4, 2));
                    }

                    if (rb_one.Checked == false && rb_three.Checked == false) //xuzpUP 20150116 处理双风机两个开停的问题
                    {
                        tempstring = ws_kt3_bs.Text.Split('.')[0];
                        kh_kt3 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                        if (tempstring.Length > 6)
                        {
                            kh_kt3 = Convert.ToInt16(tempstring.Substring(4, 2));
                        }

                        tempstring = ws_kt3b_bs.Text.Split('.')[0];
                        kh_kt3b = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                        if (tempstring.Length > 6)
                        {
                            kh_kt3b = Convert.ToInt16(tempstring.Substring(4, 2));
                        }
                    }

                    if (cb_ftfl_mk.Checked)
                    {
                        tempstring = ws_ftfl_bs.Text.Split('.')[0];
                        if (tempstring != "")
                        {
                            kh_ftfl = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                            if (tempstring.Length > 6)
                            {
                                kh_ftfl = Convert.ToInt16(tempstring.Substring(4, 2));
                            }
                        }
                        tempstring = ws_ftflb_bs.Text.Split('.')[0];
                        if (tempstring != "")
                        {
                            kh_ftflb = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
                            if (tempstring.Length > 6)
                            {
                                kh_ftflb = Convert.ToInt16(tempstring.Substring(4, 2));
                            }
                        }
                    }
                    else
                    {
                        kh_ftfl = 0;
                        kh_ftflb = 0;
                    }
                }
                catch { XtraMessageBox.Show("口号取值错误!"); return; }
                #endregion
                sb.Append(Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) & 0x00FF).ToString() + ",");      //甲烷闭锁控制口 1字节

                #region ---风电闭锁开关量逻辑解锁条件---
                dy_byte = 0;
                dy_byte = dy_byte | ((fd_logic_1.Text == "与") ? 0x01 : 0);             //7,8字节之间关系,根据选择保存
                //8,9字节之间关系,固定为0,或
                dy_byte = dy_byte | ((fd_logic_2.Text == "与") ? (1 << 2) : (0 << 2));  //7,8字节之间关系,根据选择保存
                sb.Append(dy_byte.ToString() + ",");        //风筒风量标记
                #endregion

                #region ---口号1,2---

                dy_byte = 0;
                dy_byte = ((dy_byte | 0x60) | kh_t1);       //>= 01100000
                sb.Append(dy_byte.ToString() + ",");        //口号1

                dy_byte = 0;
                dy_byte = ((dy_byte | 0x60) | kh_t2);       //>= 01100000
                sb.Append(dy_byte.ToString() + ",");        //口号2;

                #endregion

                #region ---口号3,4 风筒风量---

                dy_byte = 0;
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftfl);
                        switch (ws_ftfl_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                sb.Append(dy_byte.ToString() + ",");        //口号3;

                dy_byte = 0;
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftflb);
                        switch (ws_ftflb_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                sb.Append(dy_byte.ToString() + ",");        //口号4;

                #endregion

                #region ---口号5,6风机开停---

                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                sb.Append(dy_byte.ToString() + ",");    //口号5;

                dy_byte = 0;
                if (rb_one.Checked || rb_three.Checked) //单风机,kt3,kt3b口号为0
                {
                    dy_byte = dy_byte | 0x00;               //最高位置0(或)--kt3未定义时,表示Kt1b与kt3之间的关系为或
                }
                else
                {
                    dy_byte = dy_byte | 0x80;               //最高位置1(与)--kt3已定义时,表示Kt1b与kt3之间的关系为与
                }
                dy_byte = dy_byte | kh_kt1b;            //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1b_bs.Value) << 5));  //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1b_bs.SelectedIndex) << 5));  //5,6位为值 by tanxingyan 2014-12-11
                sb.Append(dy_byte.ToString() + ",");    //口号6;

                #endregion

                #region ---口号7,8风机开停---

                if (rb_one.Checked || rb_three.Checked)
                {
                    //单风机,只有2个开关量,7,8为0
                    dy_byte = 0;
                    sb.Append(dy_byte.ToString() + ",");        //口号7;
                    sb.Append(dy_byte.ToString() + ",");        //口号8;
                }
                else
                {
                    //双风机,4个开关量
                    dy_byte = 0;
                    dy_byte = dy_byte | 0x80;                   //最高位置1,8
                    dy_byte = dy_byte | kh_kt3;                 //低5位口号,0-4
                    //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt3_bs.Value) << 5));   //5,6位为值
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt3_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11

                    sb.Append(dy_byte.ToString() + ",");        //口号7;

                    dy_byte = 0;
                    dy_byte = dy_byte | 0x80;                   //最高位置1,8
                    dy_byte = dy_byte | kh_kt3b;                //低5位口号,0-4
                    //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt3b_bs.Value) << 5));   //5,6位为值
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt3b_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                    sb.Append(dy_byte.ToString() + ",");                    //口号8;
                }

                #endregion

                #region ---瓦斯闭锁值1,值2---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_t1 > 0)
                {
                    //tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text);
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]); //2017.12.20 by
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                        }
                    }
                    sb.Append(((mn1_hz >> 8) & 0xff).ToString() + ",");         //口1的值,值1高字节,最高位为0表示或
                    sb.Append(((mn1_hz) & 0xff).ToString() + ",");              //值1低字节
                }

                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15)
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                        }
                    }

                    sb.Append((((mn2_hz >> 8) & 0xff) | 0x80).ToString() + ",");        //值1高字节:最高位为1表示与(后跟的是开关量字节)
                    sb.Append(((mn2_hz) & 0xff).ToString() + ",");                      //值1低字节
                }

                #endregion

                #region ---风电闭锁值3---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftfl_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(ws_ftfl_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)              //取频率值
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) * Convert.ToDouble(ws_ftfl_value.Text) / tempDEV.LC));
                                    }
                                    else
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftfl_value.Text));
                                    }
                                }
                            }

                            sb.Append(((mn1_hz >> 8) | ((ws_ftfl_logic.Text == "与") ? 0x80 : 0)).ToString() + ",");    //模拟量值3;频率高8位,最高位0是或,1是与
                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                              //模拟量值3;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftfl_value.Text);
                            sb.Append(((ws_ftfl_logic.Text == "与") ? 0x80 : 0).ToString() + ",");                      //开关量值3;最高位0是或,1是与
                            sb.Append(value.ToString() + ",");
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---风电闭锁值4---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        if (ws_ftflb_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftflb_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn2_hz = (int)(Convert.ToDouble(ws_ftflb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                    else
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                }
                            }

                            sb.Append((mn2_hz >> 8).ToString() + ",");                  //模拟量值4;频率高8位,最高位0是或,1是与
                            sb.Append(((mn2_hz) & 0xff).ToString() + ",");              //模拟量值4;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftflb_value.Text);
                            sb.Append("0,");                                            //开关量值4;最高位0是或,1是与
                            sb.Append(value.ToString() + ",");                          //开关量值4
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---模拟量解锁条件---

                dy_byte = 0;
                sb.Append((Convert.ToString(68) + ","));  //0100 0100(//模拟量解锁逻辑条件,1,2号口直接是小于)

                dy_byte = 0;
                if (kh_ftfl > 0 || kh_ftflb > 0)
                {
                    if (fd_ft_js.Text != "")                //模拟量解锁逻辑条件3,4可表示模/开两种
                    {
                        dy_byte = calculateValue(fd_ft_tj.Text) << 4;
                    }

                    if (fd_ftb_js.Text != "")
                    {
                        dy_byte |= calculateValue(fd_ftb_tj.Text);
                    }
                }
                sb.Append(dy_byte.ToString() + ",");

                #endregion

                #region ---解锁值1,值2---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_t1 > 0)
                {//判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_js.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_js.Text));
                            }
                        }
                    }

                    sb.Append(((mn1_hz >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    sb.Append(((mn1_hz) & 0xff).ToString() + ",");              //值1低;
                }

                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_js.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_js.Text));
                            }
                        }
                    }

                    sb.Append(((mn2_hz >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    sb.Append(((mn2_hz) & 0xff).ToString() + ",");              //值1低字节
                }

                #endregion

                #region ---解锁值,3值4---//新增判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                if (kh_ftfl > 0)
                {
                    if (fd_ft_js.Text != "")
                    {
                        if (fd_ft_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ft_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ft_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ft_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ft_value.Text));
                                }
                            }

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                sb.Append(((mn1_hz >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;频率高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                sb.Append((mn1_hz >> 8).ToString() + ",");                                               //解锁值3;频率高8位,最高位0是或,1是与
                            }

                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                               //解锁值3;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ft_value.Text);

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                sb.Append(((value >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                sb.Append((value >> 8).ToString() + ",");                                               //解锁值3;高8位,最高位0是或,1是与
                            }

                            sb.Append(value.ToString() + ",");                                                          //解锁值3:低8位
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                if (kh_ftflb > 0)
                {
                    if (fd_ftb_js.Text != "")
                    {
                        if (fd_ftb_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ftb_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ftb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                            }

                            sb.Append(((mn1_hz >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值4;频率高8位,最高位为0或
                            sb.Append(((mn1_hz) & 0xff).ToString() + ",");                                           //解锁值4;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ftb_value.Text);
                            sb.Append(((value >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");     //解锁值4;高8位,最高位为0或
                            sb.Append(value.ToString() + ",");                                                       //解锁值4:低8位
                        }
                    }
                    else
                    {
                        sb.Append("0,0,");
                    }
                }
                else
                {
                    sb.Append("0,0,");
                }

                #endregion

                #region ---开关量解锁值---

                dy_byte = 0;
                #region by tanxingyan 2014-12-11
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt1_js.SelectedIndex)));            //口5解锁值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt1b_js.SelectedIndex) << 2));      //口6解锁值
                if (xtb_fd_kt3_js.SelectedIndex < 0)
                {
                    if (panel_fd_js.Visible == false) //备开停没有配置的情况下 发送 A xuzp20150529
                    {
                        dy_byte = (dy_byte | (0x2 << 4));       //口7解锁值
                    }
                    else
                    {
                        dy_byte = (dy_byte | (0 << 4));       //口7解锁值
                    }
                }
                else
                {
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt3_js.SelectedIndex) << 4));       //口7解锁值
                }
                if (xtb_fd_kt3b_js.SelectedIndex < 0)
                {
                    if (panel_fd_js.Visible == false) //备开停没有配置的情况下 发送 A xuzp20150529
                    {
                        dy_byte = (dy_byte | (0x2 << 6));      //口8解锁值
                    }
                    else
                    {
                        dy_byte = (dy_byte | (0 << 6));      //口8解锁值
                    }
                }
                else
                {
                    dy_byte = (dy_byte | (Convert.ToInt32(xtb_fd_kt3b_js.SelectedIndex) << 6));      //口8解锁值
                }
                #endregion
                sb.Append(dy_byte.ToString() + ",");                               //开关量口解锁条件

                #endregion

                #region ---风电闭锁控制口---

                dy_byte = Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreak.Text) & 0x00FF);
                sb.Append(dy_byte.ToString() + ",");                                    //风电闭锁控制口

                dy_byte = Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreak.Text) >> 8));
                //增加故障闭锁标记  20170627
                if ((tempStation.Bz3 & 0x4) == 0x4)
                {
                    dy_byte |= 0x01;//置故障闭锁标记
                }
                sb.Append(dy_byte.ToString() + ",");                                    //风电闭锁控制口


                #endregion

                #region ---甲烷风电闭锁控制2---
                sb.Append(Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) >> 8)).ToString());
                #endregion

                #region 单独存储风筒风量值与模开点号
                string ftflsb = string.Empty;
                if (ws_ftfl_value.Visible)
                {
                    string ftfl_bs = ws_ftfl_value.Text.ToString();
                    string ftflb_bs = ws_ftflb_value.Text.ToString();
                    string ftfl_js = fd_ft_value.Text.ToString();
                    string ftflb_js = fd_ftb_value.Text.ToString();
                    ftflsb = ftfl_bs + "," + ftflb_bs + "," + ftfl_js + "," + ftflb_js + "|";
                }
                //储存固定口
                //ftflsb += cmb_t1.Text.ToString() + ":0," + cmb_t2.Text.ToString() + ":0," + ws_kt1_bs.Text.ToString() + ":0," + ws_kt1b_bs.Text.ToString() + ":0,";
                ftflsb += cmb_t1.Text.ToString().Split('.')[0] + ":0,"
                    + cmb_t2.Text.ToString().Split('.')[0] + ":0,"
                    + ws_kt1_bs.Text.ToString().Split('.')[0] + ":0,"
                    + ws_kt1b_bs.Text.ToString().Split('.')[0] + ":0,";
                //双风机
                if (ws_kt3_bs.Visible)
                {
                    ftflsb += ws_kt3_bs.Text.ToString().Split('.')[0] + ":0," + ws_kt3b_bs.Text.ToString().Split('.')[0] + ":0,";
                }
                //风筒风量
                if (ws_ftfl_bs.Enabled)
                {
                    ftflsb += ws_ftfl_bs.Text.ToString().Split('.')[0] + ":0," + ws_ftflb_bs.Text.ToString().Split('.')[0] + ":0,";
                }
                ftflsb = ftflsb.TrimEnd(',');

                #endregion

                #region 新协议（36个字节）
                string WindAtresiaByteString = "";
                #region//甲烷风电闭锁控制口
                WindAtresiaByteString += Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) >> 8)).ToString() + ",";
                WindAtresiaByteString += Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreakCH4.Text) & 0x00FF).ToString() + ",";
                #endregion
                #region//风电闭锁解锁条件
                dy_byte = 0;
                dy_byte = dy_byte | (xtb_fd_kt1_js.SelectedIndex);             //0,1字节之间关系,根据选择保存
                //8,9字节之间关系,固定为0,或
                dy_byte = dy_byte | (xtb_fd_kt1b_js.SelectedIndex << 2);  //2,3字节之间关系,根据选择保存
                dy_byte = dy_byte | ((fd_logic_1.Text == "与" ? 0x01 : 0x00) << 4);
                WindAtresiaByteString += dy_byte.ToString() + ",";
                #endregion
                #region//主控通道T1口号及判断条件
                int T1ChannelNumber = 0;
                T1ChannelNumber = (byte)(3 << 5) + kh_t1;//闭锁条件默认是 011 >=
                WindAtresiaByteString += T1ChannelNumber + ",";
                #endregion
                #region//主控通道T2口号及判断条件
                int T2ChannelNumber = 0;
                T2ChannelNumber = (byte)(3 << 5) + kh_t2;//闭锁条件默认是 011 >=
                WindAtresiaByteString += T2ChannelNumber + ",";
                #endregion
                #region//风筒风量1及判断条件
                dy_byte = 0;
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftfl);
                        switch (ws_ftfl_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                WindAtresiaByteString += (dy_byte.ToString() + ",");        //口号3;
                #endregion
                #region//风筒风量2及判断条件
                dy_byte = 0;
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        dy_byte = (dy_byte | kh_ftflb);
                        switch (ws_ftflb_tj.Text)
                        {
                            case "=": dy_byte = dy_byte | 0x20; break;
                            case ">": dy_byte = dy_byte | 0x40; break;
                            case ">=": dy_byte = dy_byte | 0x60; break;
                            case "<": dy_byte = dy_byte | 0x80; break;
                            case "<=": dy_byte = dy_byte | 0xA0; break;
                        }
                    }
                }

                WindAtresiaByteString += (dy_byte.ToString() + ",");        //口号4;
                #endregion
                #region//1#配电柜 主开停地址号及判断条件、取值
                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                WindAtresiaByteString += (dy_byte.ToString() + ",");    //口号5;
                #endregion
                #region//1#配电柜 副开停地址号
                byte BackKT1PointHighBit = 0;
                byte BackKT1PointMiddleBit = 0;
                byte BackKT1PointLowBit = 0;
                string[] BackKT1Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                if (kh_kt3 > 0)
                {
                    BackKT1Point[24 - kh_kt3] = "1";
                }
                BackKT1PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 16);
                BackKT1PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2) >> 8);
                BackKT1PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT1Point), 2));
                WindAtresiaByteString += BackKT1PointHighBit + ",";
                WindAtresiaByteString += BackKT1PointMiddleBit + ",";
                WindAtresiaByteString += BackKT1PointLowBit + ",";
                #endregion
                #region//2#配电柜 主开停地址号及判断条件、取值
                dy_byte = 0;
                dy_byte = dy_byte | 0x80;               //最高位置1(与)--表示Kt1与Kt1b之间的关系为与   
                dy_byte = dy_byte | kh_kt1b;             //低5位口号,0-4
                //dy_byte = (dy_byte | (Convert.ToInt32(tb_ws_kt1_bs.Value) << 5));   //5,6位为值
                dy_byte = (dy_byte | (Convert.ToInt32(xtb_ws_kt1b_bs.SelectedIndex) << 5));   //5,6位为值 by tanxingyan 2014-12-11
                WindAtresiaByteString += (dy_byte.ToString() + ",");    //口号5;
                #endregion
                #region//2#配电柜 副开停地址号
                byte BackKT2PointHighBit = 0;
                byte BackKT2PointMiddleBit = 0;
                byte BackKT2PointLowBit = 0;
                string[] BackKT2Point = new string[24] { "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0" };
                if (kh_kt3b > 0)
                {
                    BackKT1Point[24 - kh_kt3b] = "1";
                }
                BackKT2PointHighBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 16);
                BackKT2PointMiddleBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2) >> 8);
                BackKT2PointLowBit = (byte)(Convert.ToInt32(string.Join("", BackKT2Point), 2));
                WindAtresiaByteString += BackKT2PointHighBit + ",";
                WindAtresiaByteString += BackKT2PointMiddleBit + ",";
                WindAtresiaByteString += BackKT2PointLowBit + ",";
                #endregion
                #region//T1甲烷闭锁值及条件
                if (kh_t1 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);

                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 12 || tempStationDev.LC2 == 13 || tempStationDev.LC2 == 15)
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_bs.Text));
                            }
                        }
                    }
                    WindAtresiaByteString += (((mn1_hz >> 8) & 0xff).ToString() + ",");         //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");              //值1低字节
                }
                else
                {
                    WindAtresiaByteString += ((((0 >> 8) & 0xff)).ToString() + ",");          //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");               //值1低字节
                }
                #endregion
                #region//T2甲烷闭锁值及条件
                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if (tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15)
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_bs.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_bs.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += ((((mn2_hz >> 8) & 0xff) | 0x80).ToString() + ",");        //值1高字节:最高位为1表示与(后跟的是开关量字节)
                    WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");                      //值1低字节
                }
                else
                {
                    WindAtresiaByteString += ((((0 >> 8) & 0xff) | 0x80).ToString() + ",");          //口1的值,值1高字节,最高位为0表示或
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");               //值1低字节
                }
                #endregion
                #region//风筒风量1闭锁值及条件
                if (kh_ftfl > 0)
                {
                    if (ws_ftfl_bs.Text != "")
                    {
                        if (ws_ftfl_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftfl_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(ws_ftfl_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)              //取频率值
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) * Convert.ToDouble(ws_ftfl_value.Text) / tempDEV.LC));
                                    }
                                    else
                                    {
                                        mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftfl_value.Text));
                                    }
                                }
                            }

                            WindAtresiaByteString += (((mn1_hz >> 8) | ((ws_ftfl_logic.Text == "与") ? 0x80 : 0)).ToString() + ",");    //模拟量值3;频率高8位,最高位0是或,1是与
                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                              //模拟量值3;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftfl_value.Text);
                            WindAtresiaByteString += (((ws_ftfl_logic.Text == "与") ? 0x80 : 0).ToString() + ",");                      //开关量值3;最高位0是或,1是与
                            WindAtresiaByteString += (value.ToString() + ",");
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风筒风量2闭锁值及条件
                if (kh_ftflb > 0)
                {
                    if (ws_ftflb_bs.Text != "")
                    {
                        if (ws_ftflb_bs.Text.IndexOf("A") != -1)
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(ws_ftflb_bs.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn2_hz = (int)(Convert.ToDouble(ws_ftflb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV != null)
                                {
                                    if (tempDEV.Pl2 != 2000)
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                    else
                                    {
                                        mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(ws_ftflb_value.Text));
                                    }
                                }
                            }

                            WindAtresiaByteString += ((mn2_hz >> 8).ToString() + ",");                  //模拟量值4;频率高8位,最高位0是或,1是与
                            WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");              //模拟量值4;频率低8位
                        }
                        else
                        {
                            value = Convert.ToInt32(ws_ftflb_value.Text);
                            WindAtresiaByteString += ("0,");                                            //开关量值4;最高位0是或,1是与
                            WindAtresiaByteString += (value.ToString() + ",");                          //开关量值4
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//T1 T2解锁条件
                dy_byte = 0;
                WindAtresiaByteString += ((Convert.ToString(68) + ","));  //0100 0100(//模拟量解锁逻辑条件,1,2号口直接是小于)                
                #endregion
                #region//风筒风量1 风筒风量2解锁条件
                dy_byte = 0;
                if (kh_ftfl > 0 || kh_ftflb > 0)
                {
                    if (fd_ft_js.Text != "")                //模拟量解锁逻辑条件3,4可表示模/开两种
                    {
                        dy_byte = calculateValue(fd_ft_tj.Text) << 4;
                    }

                    if (fd_ftb_js.Text != "")
                    {
                        dy_byte |= calculateValue(fd_ftb_tj.Text);
                    }
                }
                WindAtresiaByteString += (dy_byte.ToString() + ",");
                #endregion
                #region//T1甲烷解锁值
                if (kh_t1 > 0)
                {//判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630

                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t1.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn1_hz = (int)(Convert.ToDouble(tb_t1_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t1_js.Text));
                            }
                            else
                            {
                                mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t1_js.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += (((mn1_hz >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");              //值1低;
                }
                else
                {
                    WindAtresiaByteString += (((0 >> 8) | 0x80).ToString() + ",");         //值1高,最高位为1表示值1,值2之间为和关系.
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");              //值1低;
                }
                #endregion
                #region//T2甲烷解锁值
                if (kh_t2 > 0)
                {
                    tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(cmb_t2.Text.Split('.')[0]);
                    if (tempPoint != null)
                    {
                        tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                    }
                    //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                    if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                    {
                        mn2_hz = (int)(Convert.ToDouble(tb_t2_js.Text) * 100);
                    }
                    else
                    {
                        if (tempDEV != null)
                        {
                            if (tempDEV.Pl2 != 2000)
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(tb_t2_js.Text));
                            }
                            else
                            {
                                mn2_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(tb_t2_js.Text));
                            }
                        }
                    }

                    WindAtresiaByteString += (((mn2_hz >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    WindAtresiaByteString += (((mn2_hz) & 0xff).ToString() + ",");              //值1低字节
                }
                else
                {
                    WindAtresiaByteString += (((0 >> 8) | 0x80).ToString() + ",");         //值1高字节:最高位为1未启用
                    WindAtresiaByteString += (((0) & 0xff).ToString() + ",");              //值1低字节
                }
                #endregion
                #region//风筒风量1解锁值
                if (kh_ftfl > 0)
                {
                    if (fd_ft_js.Text != "")
                    {
                        if (fd_ft_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ft_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ft_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ft_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ft_value.Text));
                                }
                            }

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                WindAtresiaByteString += (((mn1_hz >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;频率高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                WindAtresiaByteString += ((mn1_hz >> 8).ToString() + ",");                                               //解锁值3;频率高8位,最高位0是或,1是与
                            }

                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                               //解锁值3;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ft_value.Text);

                            if (fd_ftb_js.Text != "")   //当有备风筒风量时,关系取下拉列表
                            {
                                WindAtresiaByteString += (((value >> 8) | ((fd_logic_3.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值3;高8位,最高位0是或,1是与
                            }
                            else                        //当无备风筒风量时,关系取或
                            {
                                WindAtresiaByteString += ((value >> 8).ToString() + ",");                                               //解锁值3;高8位,最高位0是或,1是与
                            }

                            WindAtresiaByteString += (value.ToString() + ",");                                                          //解锁值3:低8位
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风筒风量2解锁值
                if (kh_ftflb > 0)
                {
                    if (fd_ftb_js.Text != "")
                    {
                        if (fd_ftb_js.Text.IndexOf("A") != -1)   //模拟量
                        {
                            tempPoint = Model.DEFServiceModel.QueryPointByCodeCache(fd_ftb_js.Text.Split('.')[0]);
                            if (tempPoint != null)
                            {
                                tempDEV = Model.DEVServiceModel.QueryDevByDevIDCache(tempPoint.Devid);
                            }
                            //判断是否为智能分站，如果是智能分站，则将值放大100倍进行传输并转换成整数  20140630
                            if ((tempStationDev.LC2 == 13 || tempStationDev.LC2 == 12 || tempStationDev.LC2 == 15))
                            {
                                mn1_hz = (int)(Convert.ToDouble(fd_ftb_value.Text) * 100);
                            }
                            else
                            {
                                if (tempDEV.Pl2 != 2000)              //取频率值
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(tempDEV.Pl2 - tempDEV.Pl1) / tempDEV.LC) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                                else
                                {
                                    mn1_hz = Convert.ToInt32(200 + ((double)(1000 - tempDEV.Pl1) / tempDEV.LC2) * Convert.ToDouble(fd_ftb_value.Text));
                                }
                            }

                            WindAtresiaByteString += (((mn1_hz >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");    //解锁值4;频率高8位,最高位为0或
                            WindAtresiaByteString += (((mn1_hz) & 0xff).ToString() + ",");                                           //解锁值4;频率低8位
                        }
                        else  //开关量
                        {
                            value = Convert.ToInt32(fd_ftb_value.Text);
                            WindAtresiaByteString += (((value >> 8) | ((fd_logic_4.Text == "与") ? 0x80 : 0)).ToString() + ",");     //解锁值4;高8位,最高位为0或
                            WindAtresiaByteString += (value.ToString() + ",");                                                       //解锁值4:低8位
                        }
                    }
                    else
                    {
                        WindAtresiaByteString += ("0,0,");
                    }
                }
                else
                {
                    WindAtresiaByteString += ("0,0,");
                }
                #endregion
                #region//风电闭锁控制口
                dy_byte = Convert.ToByte((GetControlValueForKJ306_F(cCmbControlWindBreak.Text) >> 8));
                WindAtresiaByteString += (dy_byte.ToString() + ",");                                    //风电闭锁控制口(高在前)

                dy_byte = Convert.ToByte(GetControlValueForKJ306_F(cCmbControlWindBreak.Text) & 0x00FF);
                WindAtresiaByteString += (dy_byte.ToString() + ",");                                    //风电闭锁控制口（低在后）
                #endregion
                #region//T1 T2 风筒风量1 风筒风量2 多参数地址号
                byte T1AddressNumber = 0;
                if (!string.IsNullOrEmpty(cmb_t1.Text))
                {
                    T1AddressNumber = byte.Parse(cmb_t1.Text.Trim().Substring(6, 1));
                }
                byte T2AddressNumber = 0;
                if (!string.IsNullOrEmpty(cmb_t2.Text))
                {
                    T2AddressNumber = byte.Parse(cmb_t2.Text.Trim().Substring(6, 1));
                }
                int AdderssNumberByte = (T2AddressNumber << 2) + T1AddressNumber;
                byte FTFL1AddressNumber = 0;
                if (!string.IsNullOrEmpty(ws_ftfl_bs.Text))
                {
                    FTFL1AddressNumber = byte.Parse(ws_ftfl_bs.Text.Trim().Substring(6, 1));
                }
                byte FTFL2AddressNumber = 0;
                if (!string.IsNullOrEmpty(ws_ftflb_bs.Text))
                {
                    FTFL2AddressNumber = byte.Parse(ws_ftflb_bs.Text.Trim().Substring(6, 1));
                }
                int FTFLAdderssNumberByte = (FTFL2AddressNumber << 2) + FTFL1AddressNumber;
                WindAtresiaByteString += (FTFLAdderssNumberByte << 4) + AdderssNumberByte;
                #endregion

                #endregion

                #region ---数据库存储---
                if (sb.Length > 0)
                {
                    try
                    {
                        if (tempStation.Bz10 != sb.ToString() || tempStation.Bz9 != ftflsb)
                        {
                            tempStationForm.CtxbControlBytes.Text = sb.ToString();
                            tempStationForm.CtxbControlBytesNew.Text = WindAtresiaByteString;//新风电闭锁的配置  20171018
                            tempStationForm.CtxbControlConditon.Text = ftflsb;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex);
                    }
                }
                #endregion

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}

