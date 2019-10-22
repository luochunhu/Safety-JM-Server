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

namespace Sys.Safety.Client.Define.Station
{
    public partial class CFWLogicBreakCfg : XtraForm
    {
        ///// <summary>
        ///// 当前的分站号
        ///// </summary>
        //public int Fz;
        ///// <summary>
        ///// 是否为测点类型发生改变
        ///// </summary>
        //public bool LxChange;

        ///// <summary>
        ///// 存储开关量各态显示 string[]数长度为3 分别存储0 1 2 态 by tanxingyan 2014-12-11
        ///// </summary>
        //Dictionary<string, string[]> kglxs = new Dictionary<string, string[]>();
        //int xdevid = 0; //by tanxingyan 2014-12-11
        //string[] str;  // by tanxingyan 2014-12-11
        //string xpoint;// by tanxingyan 2014-12-11
        //string[] Kgl_array = null;
        ////控制口(本控和交叉控) by huangxxUP 2013-12-28
        //byte mkkzk = 0;
        //byte MasCheck_ws = 0;
        public CFWLogicBreakCfg()
        {
            InitializeComponent();
        }

        private void CbtnOK_Click(object sender, EventArgs e)
        {

        }

        private void CbtnCancle_Click(object sender, EventArgs e)
        {

        }

        //private void Ljkz_Load(object sender, EventArgs e)
        //{
        //    if (LxChange)
        //    {
        //        masBtn_OK_Click(sender, e);
        //    }
        //    lab_fz.Text = Fz.ToString();
        //    rb_one.Checked = true;

        //    cmb_bs_zfjkt_tj.Text = "与";
        //    cmb_bs_bfjkt_tj.Text = "与";
        //    cmb_bs_zbfjkt_tj.Text = "与";
        //    cmb_js_zfjkt_tj.Text = "或";
        //    cmb_js_bfjkt_tj.Text = "或";
        //    cmb_js_zbfjkt_tj.Text = "或";

        //    #region ---信息初始化---
        //    try
        //    {
        //        List<string> Mnl = new List<string>();  //保存模拟量测点
        //        List<string> Kgl = new List<string>();  //保存开关量测点
        //        List<string> MnKg = new List<string>(); //保存所有测点

        //        rb_one.Checked = true;
        //        int fzh = Fz - 1;
        //        int kzk = -1;
        //        lab_fz.Text = Fz.ToString();
        //        ComBoxClear();

        //        DataTable dt = Swap.sqlc.GetDEF(string.Format("{0:000}000", Fz));
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0]["devID"].ToString() != "")
        //            {
        //                kzk = Convert.ToByte(dt.Rows[0]["devID"].ToString());
        //            }
        //        }

        //        if (kzk == -1)
        //        {
        //            MessageBox.Show("分站未定义");
        //            return;
        //        }

        //        dt = Swap.sqlc.GetDefTable();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (dt.Rows[i]["fzh"].ToString() == Fz.ToString())
        //            {
        //                if (dt.Rows[i]["point"].ToString().IndexOf("A") != -1)
        //                {
        //                    Mnl.Add(dt.Rows[i]["point"].ToString());
        //                    MnKg.Add(dt.Rows[i]["point"].ToString());
        //                }
        //                else if (dt.Rows[i]["point"].ToString().IndexOf("D") != -1)
        //                {
        //                    Kgl.Add(dt.Rows[i]["point"].ToString());
        //                    MnKg.Add(dt.Rows[i]["point"].ToString());

        //                    #region 加载开关量及0 1 2态显示  by tanxingyan 2014-12-11
        //                    str = new string[3];
        //                    xpoint = dt.Rows[i]["point"].ToString();
        //                    xdevid = int.Parse(dt.Rows[i]["devid"].ToString());
        //                    str[0] = Swap.dev[xdevid - 1].xs0;
        //                    str[1] = Swap.dev[xdevid - 1].xs1;
        //                    str[2] = Swap.dev[xdevid - 1].xs2;
        //                    kglxs.Add(xpoint, str);
        //                    #endregion
        //                }
        //            }
        //        }

        //        PointLoad(Kgl, ref Kgl_array);

        //        ComBoxInit();

        //        Mnl.Clear();
        //        Kgl.Clear();
        //        MnKg.Clear();
        //    }
        //    catch (Exception err)
        //    {
        //        MessageBox.Show(err.Message);
        //    }
        //    #endregion

        //    #region 逻辑控制初始化
        //    try
        //    {
        //        int index = 0;

        //        if (Swap._ljkz[Fz - 1].Length > 0)
        //        {
        //            string[] ljkz = Swap._ljkz[Fz - 1].Split(',');
        //            if (ljkz.Length > 0)
        //            {
        //                #region ---读取风机类型,单/双风机---
        //                if (ljkz[index++] == "1")
        //                {
        //                    rb_one.Checked = true;
        //                }
        //                else
        //                {
        //                    rb_two.Checked = true;
        //                }
        //                #endregion

        //                #region ---读取逻辑控制开关量解锁逻辑条件---
        //                index++;
        //                if ((Convert.ToInt32(ljkz[index]) & 0x01) == 0x01)
        //                {
        //                    cmb_js_zfjkt_tj.Text = "与";
        //                }
        //                else
        //                {
        //                    cmb_js_zfjkt_tj.Text = "或";
        //                }
        //                if (rb_two.Checked)
        //                {
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x02) == 0x02)
        //                    {
        //                        cmb_js_zbfjkt_tj.Text = "与";
        //                    }
        //                    else
        //                    {
        //                        cmb_js_zbfjkt_tj.Text = "或";
        //                    }
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x04) == 0x04)
        //                    {
        //                        cmb_js_bfjkt_tj.Text = "与";
        //                    }
        //                    else
        //                    {
        //                        cmb_js_bfjkt_tj.Text = "或";
        //                    }
        //                }
        //                #endregion

        //                #region ---开关量闭锁信息---
        //                index = index + 5;
        //                #region KT1相关信息
        //                #region 条件
        //                if ((Convert.ToInt32(ljkz[index]) & 0x80) == 0x80)
        //                {
        //                    cmb_bs_zfjkt_tj.Text = "与";
        //                }
        //                else
        //                {
        //                    cmb_bs_zfjkt_tj.Text = "或";
        //                }
        //                #endregion
        //                #region 通道号
        //                cmb_bs_zfjkt_z.Text = Fz.ToString().PadLeft(3, '0') + "D" + (Convert.ToByte(ljkz[index]) & 0x1F).ToString().PadLeft(2, '0');
        //                #endregion
        //                #region 值
        //                if ((Convert.ToInt32(ljkz[index]) & 0x40) == 0x40)
        //                {
        //                    dud_bs_zfjkt_z_value.SelectedIndex = 2;
        //                }
        //                else if ((Convert.ToInt32(ljkz[index]) & 0x20) == 0x20)
        //                {
        //                    dud_bs_zfjkt_z_value.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    dud_bs_zfjkt_z_value.SelectedIndex = 0;
        //                }
        //                #endregion
        //                #endregion
        //                index++;
        //                #region KT2相关信息
        //                #region 条件
        //                if (rb_two.Checked)
        //                {
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x80) == 0x80)
        //                    {
        //                        cmb_bs_zbfjkt_tj.Text = "与";
        //                    }
        //                    else
        //                    {
        //                        cmb_bs_zbfjkt_tj.Text = "或";
        //                    }
        //                }
        //                #endregion
        //                #region 通道号
        //                cmb_bs_zfjkt_b.Text = Fz.ToString().PadLeft(3, '0') + "D" + (Convert.ToByte(ljkz[index]) & 0x1F).ToString().PadLeft(2, '0');
        //                #endregion
        //                #region 值
        //                if ((Convert.ToInt32(ljkz[index]) & 0x40) == 0x40)
        //                {
        //                    dud_bs_zfjkt_b_value.SelectedIndex = 2;
        //                }
        //                else if ((Convert.ToInt32(ljkz[index]) & 0x20) == 0x20)
        //                {
        //                    dud_bs_zfjkt_b_value.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    dud_bs_zfjkt_b_value.SelectedIndex = 0;
        //                }
        //                #endregion
        //                #endregion
        //                index++;
        //                if (rb_two.Checked)
        //                {
        //                    #region KT3相关信息
        //                    #region 条件
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x80) == 0x80)
        //                    {
        //                        cmb_bs_bfjkt_tj.Text = "与";
        //                    }
        //                    else
        //                    {
        //                        cmb_bs_bfjkt_tj.Text = "或";
        //                    }
        //                    #endregion
        //                    #region 通道号
        //                    cmb_bs_bfjkt_z.Text = Fz.ToString().PadLeft(3, '0') + "D" + (Convert.ToByte(ljkz[index]) & 0x1F).ToString().PadLeft(2, '0');
        //                    #endregion
        //                    #region 值
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x40) == 0x40)
        //                    {
        //                        dud_bs_bfjkt_z_value.SelectedIndex = 2;
        //                    }
        //                    else if ((Convert.ToInt32(ljkz[index]) & 0x20) == 0x20)
        //                    {
        //                        dud_bs_bfjkt_z_value.SelectedIndex = 1;
        //                    }
        //                    else
        //                    {
        //                        dud_bs_bfjkt_z_value.SelectedIndex = 0;
        //                    }
        //                    #endregion
        //                    #endregion
        //                }
        //                index++;
        //                if (rb_two.Checked)
        //                {
        //                    #region KT4相关信息
        //                    #region 通道号
        //                    cmb_bs_bfjkt_b.Text = Fz.ToString().PadLeft(3, '0') + "D" + (Convert.ToByte(ljkz[index]) & 0x1F).ToString().PadLeft(2, '0');
        //                    #endregion
        //                    #region 值
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x40) == 0x40)
        //                    {
        //                        dud_bs_bfjkt_b_value.SelectedIndex = 2;
        //                    }
        //                    else if ((Convert.ToInt32(ljkz[index]) & 0x20) == 0x20)
        //                    {
        //                        dud_bs_bfjkt_b_value.SelectedIndex = 1;
        //                    }
        //                    else
        //                    {
        //                        dud_bs_bfjkt_b_value.SelectedIndex = 0;
        //                    }
        //                    #endregion
        //                    #endregion
        //                }
        //                #endregion

        //                #region ---开关量解锁值---
        //                index += 19;
        //                #region KT1解锁值
        //                if ((Convert.ToInt32(ljkz[index]) & 0x02) == 0x02)
        //                {
        //                    dud_js_zfjkt_z_value.SelectedIndex = 2;
        //                }
        //                else if ((Convert.ToInt32(ljkz[index]) & 0x01) == 0x01)
        //                {
        //                    dud_js_zfjkt_z_value.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    dud_js_zfjkt_z_value.SelectedIndex = 0;
        //                }
        //                #endregion
        //                #region KT2解锁值
        //                if ((Convert.ToInt32(ljkz[index]) & 0x08) == 0x08)
        //                {
        //                    dud_js_zfjkt_b_value.SelectedIndex = 2;
        //                }
        //                else if ((Convert.ToInt32(ljkz[index]) & 0x04) == 0x04)
        //                {
        //                    dud_js_zfjkt_b_value.SelectedIndex = 1;
        //                }
        //                else
        //                {
        //                    dud_js_zfjkt_b_value.SelectedIndex = 0;
        //                }
        //                #endregion
        //                if (rb_two.Checked)
        //                {
        //                    #region KT3解锁值
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x20) == 0x20)
        //                    {
        //                        dud_js_bfjkt_z_value.SelectedIndex = 2;
        //                    }
        //                    else if ((Convert.ToInt32(ljkz[index]) & 0x10) == 0x10)
        //                    {
        //                        dud_js_bfjkt_z_value.SelectedIndex = 1;
        //                    }
        //                    else
        //                    {
        //                        dud_js_bfjkt_z_value.SelectedIndex = 0;
        //                    }
        //                    #endregion
        //                    #region KT4解锁值
        //                    if ((Convert.ToInt32(ljkz[index]) & 0x80) == 0x80)
        //                    {
        //                        dud_js_bfjkt_b_value.SelectedIndex = 2;
        //                    }
        //                    else if ((Convert.ToInt32(ljkz[index]) & 0x40) == 0x40)
        //                    {
        //                        dud_js_bfjkt_b_value.SelectedIndex = 1;
        //                    }
        //                    else
        //                    {
        //                        dud_js_bfjkt_b_value.SelectedIndex = 0;
        //                    }
        //                    #endregion
        //                }
        //                #endregion

        //                #region
        //                index++;
        //                ck_kzk.CheckValue = Convert.ToByte(ljkz[index]);
        //                #endregion
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("逻辑控制加载失败!", "提示");
        //        return;
        //    }
        //    #endregion

        //    //加载时 获取本控和交叉控 by huangxxUP 2013-12-28
        //    GetKzk();
        //    //瓦斯控制口 by huangxxUP 2013-12-28
        //    MasCheck_ws = ck_kzk.CheckValue;
        //}

        //private void masBtn_OK_Click(object sender, EventArgs e)
        //{
        //    int fzh = Fz - 1;
        //    StringBuilder sb = new StringBuilder();
        //    int dy_byte = 0;
        //    string tempstring;
        //    int ktz1 = 0, ktz2 = 0, ktb1 = 0, ktb2 = 0;

        //    if (!Check())
        //    {
        //        return;
        //    }

        //    #region ---口号取值---
        //    try
        //    {
        //        tempstring = cmb_bs_zfjkt_z.Text;
        //        ktz1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));

        //        tempstring = cmb_bs_zfjkt_b.Text;
        //        ktz2 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));

        //        if (rb_two.Checked)
        //        {
        //            tempstring = cmb_bs_bfjkt_z.Text;
        //            ktb1 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));

        //            tempstring = cmb_bs_bfjkt_b.Text;
        //            ktb2 = Convert.ToInt16(tempstring.Substring(tempstring.Length - 2));
        //        }
        //    }
        //    catch { MessageBox.Show("口号取值错误!"); return; }
        //    #endregion

        //    #region ---根据协议解析数据---

        //    #region ---单双风机标志---
        //    sb.Append(((rb_one.Checked) ? "1" : "2") + ",");      //双单风机标志
        //    #endregion
        //    #region ---甲烷闭锁控制口---
        //    sb.Append("0,");
        //    #endregion
        //    #region ---风电闭锁开关量逻辑解锁条件 1字节---
        //    dy_byte = 0;
        //    dy_byte = dy_byte | ((cmb_js_zfjkt_tj.Text == "与") ? 0x01 : 0);             //7,8字节之间关系 1:与 0:或
        //    if (rb_two.Checked)
        //    {
        //        dy_byte = dy_byte | ((cmb_js_zbfjkt_tj.Text == "与") ? (1 << 1) : (0 << 1));  //8,9字节之间关系 1:与 0:或
        //        dy_byte = dy_byte | ((cmb_js_bfjkt_tj.Text == "与") ? (1 << 2) : (0 << 2));  //9,10字节之间关系 1:与 0:或
        //    }
        //    sb.Append(dy_byte.ToString() + ",");
        //    #endregion
        //    #region ---主通道口号---
        //    #region ---前4个为模拟量 默认0---
        //    sb.Append("0,0,0,0,");
        //    #endregion
        //    #region ---后4个为开关量---
        //    #region ---第5字节开关量---
        //    dy_byte = 0;
        //    dy_byte = dy_byte | ((cmb_bs_zfjkt_tj.Text == "与") ? 0x80 : 0);  //最高位(第7位)
        //    //dy_byte = dy_byte | dud_bs_zfjkt_z_value.SelectedIndex;
        //    if (dud_bs_zfjkt_z_value.SelectedIndex == 1)  //第6,5位 表示值:00,01,10分别对应0,1,2态
        //    {
        //        dy_byte = dy_byte | (1 << 5);
        //    }
        //    else if (dud_bs_zfjkt_z_value.SelectedIndex == 2)
        //    {
        //        dy_byte = dy_byte | (1 << 6);
        //    }
        //    dy_byte = dy_byte | ktz1;  //4,3,2,1,0通道号
        //    sb.Append(dy_byte.ToString() + ",");
        //    #endregion
        //    #region ---第6字节开关量---
        //    dy_byte = 0;
        //    if (rb_two.Checked)
        //    {
        //        dy_byte = dy_byte | ((cmb_bs_zbfjkt_tj.Text == "与") ? 0x80 : 0);  //最高位(第7位)
        //    }
        //    //dy_byte = dy_byte | dud_bs_zfjkt_b_value.SelectedIndex;
        //    if (dud_bs_zfjkt_b_value.SelectedIndex == 1)  //第6,5位 表示值:00,01,10分别对应0,1,2态
        //    {
        //        dy_byte = dy_byte | (1 << 5);
        //    }
        //    else if (dud_bs_zfjkt_b_value.SelectedIndex == 2)
        //    {
        //        dy_byte = dy_byte | (1 << 6);
        //    }
        //    dy_byte = dy_byte | ktz2;  //4,3,2,1,0通道号
        //    sb.Append(dy_byte.ToString() + ",");
        //    #endregion
        //    if (rb_two.Checked)
        //    {
        //        #region ---第7字节开关量---
        //        dy_byte = 0;
        //        dy_byte = dy_byte | ((cmb_bs_bfjkt_tj.Text == "与") ? 0x80 : 0);  //最高位(第7位)
        //        //dy_byte = dy_byte | dud_bs_bfjkt_z_value.SelectedIndex;
        //        if (dud_bs_bfjkt_z_value.SelectedIndex == 1)  //第6,5位 表示值:00,01,10分别对应0,1,2态
        //        {
        //            dy_byte = dy_byte | (1 << 5);
        //        }
        //        else if (dud_bs_bfjkt_z_value.SelectedIndex == 2)
        //        {
        //            dy_byte = dy_byte | (1 << 6);
        //        }
        //        dy_byte = dy_byte | ktb1;  //4,3,2,1,0通道号
        //        sb.Append(dy_byte.ToString() + ",");
        //        #endregion
        //        #region ---第8字节开关量---
        //        dy_byte = 0;
        //        //dy_byte = dy_byte | ((cmb_bs_bfjkt_tj.Text == "与") ? 0x80 : 0);  //最高位(第7位)
        //        //dy_byte = dy_byte | dud_bs_bfjkt_b_value.SelectedIndex;
        //        if (dud_bs_bfjkt_b_value.SelectedIndex == 1)  //第6,5位 表示值:00,01,10分别对应0,1,2态
        //        {
        //            dy_byte = dy_byte | (1 << 5);
        //        }
        //        else if (dud_bs_bfjkt_b_value.SelectedIndex == 2)
        //        {
        //            dy_byte = dy_byte | (1 << 6);
        //        }
        //        dy_byte = dy_byte | ktb2;  //4,3,2,1,0通道号
        //        sb.Append(dy_byte.ToString() + ",");
        //        #endregion
        //    }
        //    else
        //    {
        //        sb.Append("0,0,");
        //    }
        //    #endregion
        //    #endregion
        //    #region ---闭锁值---
        //    sb.Append("0,0,0,0,0,0,0,0,"); //8位,不启用
        //    #endregion
        //    #region ---模拟量解锁条件---
        //    sb.Append("0,0,");  //2字节 不启用
        //    #endregion
        //    #region ---解锁值---
        //    sb.Append("0,0,0,0,0,0,0,0,"); //8位,不启用
        //    #endregion
        //    #region ---开关量解锁值---
        //    dy_byte = 0;
        //    #region KT1解锁状态值
        //    if (dud_js_zfjkt_z_value.SelectedIndex == 1)
        //    {
        //        dy_byte = dy_byte | 1;
        //    }
        //    else if (dud_js_zfjkt_z_value.SelectedIndex == 2)
        //    {
        //        dy_byte = dy_byte | (1 << 1);
        //    }
        //    #endregion
        //    #region KT2解锁状态值
        //    if (dud_js_zfjkt_b_value.SelectedIndex == 1)
        //    {
        //        dy_byte = dy_byte | (1 << 2);
        //    }
        //    else if (dud_js_zfjkt_b_value.SelectedIndex == 2)
        //    {
        //        dy_byte = dy_byte | (1 << 3);
        //    }
        //    #endregion
        //    if (rb_two.Checked)
        //    {
        //        #region KT3解锁状态值
        //        if (dud_js_bfjkt_z_value.SelectedIndex == 1)
        //        {
        //            dy_byte = dy_byte | (1 << 4);
        //        }
        //        else if (dud_js_bfjkt_z_value.SelectedIndex == 2)
        //        {
        //            dy_byte = dy_byte | (1 << 5);
        //        }
        //        #endregion
        //        #region KT4解锁状态值
        //        if (dud_js_bfjkt_b_value.SelectedIndex == 1)
        //        {
        //            dy_byte = dy_byte | (1 << 6);
        //        }
        //        else if (dud_js_bfjkt_b_value.SelectedIndex == 2)
        //        {
        //            dy_byte = dy_byte | (1 << 7);
        //        }
        //        #endregion
        //    }
        //    sb.Append(dy_byte + ",");
        //    #endregion
        //    #region ---闭锁控制口---
        //    dy_byte = Convert.ToByte(ck_kzk.CheckValue);
        //    sb.Append(dy_byte.ToString() + ",");
        //    #endregion
        //    #region ---第31,32位备用---
        //    sb.Append("0,0");
        //    #endregion

        //    #endregion

        //    #region ---数据库存储---
        //    if (sb.Length > 0)
        //    {
        //        #region 主数据库保存
        //        try
        //        {
        //            SqlConnection sql = new SqlConnection(Swap.ConnectionString);
        //            SqlCommand cmd = new SqlCommand("alter table config alter column text text", sql);
        //            sql.Open();
        //            cmd.ExecuteNonQuery();
        //            cmd = new SqlCommand("delete from config where name='ljkz_" + Fz.ToString() + "'", sql);
        //            cmd.ExecuteNonQuery();

        //            cmd = new SqlCommand("insert into config (name,text,flag) values ('ljkz_" + Fz.ToString() + "','" + sb.ToString() + "','0')", sql);
        //            cmd.ExecuteNonQuery();
        //            sql.Close();

        //            if (Swap._ljkz[fzh] != sb.ToString())
        //            {
        //                Swap._ljkzInit[fzh] = true;
        //                Swap._3fInit[fzh] = false;//逻辑控制生效时,需将风电闭锁初始化置为false
        //            }
        //            Swap._ljkz[fzh] = sb.ToString();

        //            Swap.sqlc.AddSql(string.Format("Insert into Log{0:yyyyMMdd} (username,type,remark,timer) " +
        //                            "VALUES ('{1}','{2}','{3}','{4}')",
        //                            DateTime.Now,
        //                            Swap.CurrUser,
        //                            0,
        //                            "修改开关量逻辑控制,分站:" + Convert.ToInt32(fzh + 1) + ",值:" + sb.ToString(),
        //                            DateTime.Now));
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("主数据库连接失败，逻辑控制保存失败！", "提示");
        //            return;
        //        }
        //        #endregion
        //        if (Swap.IsBackupDB)
        //        {
        //            #region 备数据库保存
        //            try
        //            {
        //                SqlConnection sql = new SqlConnection(Swap.BackupConnString);
        //                SqlCommand cmd = new SqlCommand("alter table config alter column text  text", sql);
        //                sql.Open();
        //                cmd.ExecuteNonQuery();
        //                cmd = new SqlCommand("delete from config where name='ljkz_" + Fz.ToString() + "'", sql);
        //                cmd.ExecuteNonQuery();

        //                cmd = new SqlCommand("insert into config (name,text,flag) values ('ljkz_" + Fz.ToString() + "','" + sb.ToString() + "','0')", sql);
        //                cmd.ExecuteNonQuery();
        //                sql.Close();

        //                Swap.sqlc.AddSql(string.Format("Insert into Log{0:yyyyMMdd} (username,type,remark,timer) " +
        //                                "VALUES ('{1}','{2}','{3}','{4}')",
        //                                DateTime.Now,
        //                                Swap.CurrUser,
        //                                0,
        //                                "修改开关量逻辑控制,分站:" + Convert.ToInt32(fzh + 1) + ",值:" + sb.ToString(),
        //                                DateTime.Now));
        //            }
        //            catch (Exception)
        //            {
        //                MessageBox.Show("主数据库逻辑控制保存成功，备用数据库逻辑控制保存失败！", "提示");
        //                return;
        //            }
        //            #endregion
        //        }
        //    }
        //    #endregion
        //    MessageBox.Show("保存成功!","提示");
        //}

        //private void masBtn_EXIT_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void rb_one_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rb_one.Checked)
        //    {
        //        panl_bs_more.Visible = false;
        //        panl_js_more.Visible = false;
        //    }
        //    else
        //    {
        //        panl_bs_more.Visible = true;
        //        panl_js_more.Visible = true;
        //    }
        //}

        ///// <summary>
        ///// 设置对应checkBox的勾选状态 by huangxxUP 2013-12-28
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ChangEnableA(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CheckBox cb = sender as CheckBox;

        //        if (null == cb) { return; }
        //        //选择任意一个的checkBox让另一个checkBox对应项变为不启用 
        //        //check.ChangeCheckEnable(cb.Name, cb.CheckState == CheckState.Checked ? true : false);

        //        if (cb.Checked)
        //        {
        //            byte checkedKzk = oneKzk(cb.Name);
        //            //如果即将勾选的控制口已经设置了本地控制或者交叉控制，则给出提示
        //            if ((checkedKzk & mkkzk) > 0)
        //            {
        //                ck_kzk.CheckValue = MasCheck_ws;
        //                MessageBox.Show("该控制口已经设置本地控制或交叉控制或手动控制！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        //            }
        //            else
        //            {
        //                MasCheck_ws = ck_kzk.CheckValue;
        //            }
        //        }
        //        else
        //        {
        //            MasCheck_ws = ck_kzk.CheckValue;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MFuncClass.SysClass.WriteLog(ex.Message, "错误Fdbs_ChangEnableA");
        //    }

        //}

        //private string[] PointLoad(List<string> _list, ref string[] _value)
        //{
        //    try
        //    {
        //        _value = new string[_list.Count + 1];
        //        _value[0] = "";
        //        for (int i = 0; i < _list.Count; i++)
        //        {
        //            _value[i + 1] = _list[i];
        //        }
        //    }
        //    catch { }
        //    return _value;
        //}

        ///// <summary>
        ///// 下拉列表清空
        ///// </summary>
        //private void ComBoxClear()
        //{
        //    try
        //    {
        //        cmb_bs_bfjkt_b.Items.Clear();
        //        cmb_bs_bfjkt_z.Items.Clear();
        //        cmb_bs_zfjkt_b.Items.Clear();
        //        cmb_bs_zfjkt_z.Items.Clear();
        //        cmb_js_bfjkt_b.Items.Clear();
        //        cmb_js_bfjkt_z.Items.Clear();
        //        cmb_js_zfjkt_b.Items.Clear();
        //        cmb_js_zfjkt_z.Items.Clear();
        //        dud_bs_bfjkt_b_value.Items.Clear();
        //        dud_bs_bfjkt_z_value.Items.Clear();
        //        dud_bs_zfjkt_b_value.Items.Clear();
        //        dud_bs_zfjkt_z_value.Items.Clear();
        //        dud_js_bfjkt_b_value.Items.Clear();
        //        dud_js_bfjkt_z_value.Items.Clear();
        //        dud_js_zfjkt_b_value.Items.Clear();
        //        dud_js_zfjkt_z_value.Items.Clear();
        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// 下拉列表初始化
        ///// </summary>
        //private void ComBoxInit()
        //{
        //    try
        //    {
        //        cmb_bs_zfjkt_z.Items.AddRange(Kgl_array);
        //        cmb_bs_zfjkt_b.Items.AddRange(Kgl_array);
        //        cmb_bs_bfjkt_z.Items.AddRange(Kgl_array);
        //        cmb_bs_bfjkt_b.Items.AddRange(Kgl_array);

        //        cmb_js_zfjkt_z.Items.AddRange(Kgl_array);
        //        cmb_js_zfjkt_b.Items.AddRange(Kgl_array);
        //        cmb_js_bfjkt_z.Items.AddRange(Kgl_array);
        //        cmb_js_bfjkt_b.Items.AddRange(Kgl_array);
        //    }
        //    catch { }
        //}

        ///// <summary>
        ///// 根据MasCheck单选框返回单点控制口号(byte) by huangxxUP 2013-12-28
        ///// </summary>
        ///// <param name="kzkName"></param>
        ///// <returns></returns>
        //private byte oneKzk(string kzkName)
        //{
        //    byte checkedKzk = 0;
        //    switch (kzkName)
        //    {
        //        case "ck1":
        //            checkedKzk = 1;
        //            break;
        //        case "ck2":
        //            checkedKzk = 2;
        //            break;
        //        case "ck3":
        //            checkedKzk = 4;
        //            break;
        //        case "ck4":
        //            checkedKzk = 8;
        //            break;
        //        case "ck5":
        //            checkedKzk = 16;
        //            break;
        //        case "ck6":
        //            checkedKzk = 32;
        //            break;
        //        case "ck7":
        //            checkedKzk = 64;
        //            break;
        //        case "ck8":
        //            checkedKzk = 128;
        //            break;
        //        default:
        //            break;
        //    }
        //    return checkedKzk;
        //}

        ///// <summary>
        ///// 获取控制口(交叉控制和本地控制和手动控制) by huangxxUP 2013-12-28
        ///// </summary>
        //private void GetKzk()
        //{
        //    #region
        //    //获取本分站下所有口号的本地控制口和其他分站的交叉控制口
        //    //获取本分站下所有的模开量
        //    string mkl = string.Empty;
        //    //除本分站的交叉控制测点号
        //    string jckzPoints = string.Empty;
        //    DataTable dt = Swap.sqlc.GetDefTable();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        if (dt.Rows[i]["fzh"].ToString() == Fz.ToString())
        //        {
        //            if (dt.Rows[i]["point"].ToString().IndexOf("A") != -1 || dt.Rows[i]["point"].ToString().IndexOf("D") != -1)
        //            {
        //                mkl += dt.Rows[i]["point"].ToString() + ",";
        //            }
        //        }
        //    }
        //    mkl = mkl.TrimEnd(',');
        //    if (!string.IsNullOrEmpty(mkl))
        //    {
        //        //根据模开量找对应的本地控制口
        //        for (int i = 0; i < mkl.Split(',').Length; i++)
        //        {
        //            dt = Swap.sqlc.GetDEF(mkl.Split(',')[i].ToString());
        //            if (mkl.Split(',')[i].ToString().IndexOf("A") != -1)
        //            {
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k1"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k2"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k3"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k4"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k5"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k6"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k7"].ToString());
        //            }
        //            else
        //            {
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k1"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k2"].ToString());
        //                mkkzk |= Convert.ToByte(dt.Rows[0]["k3"].ToString());
        //            }
        //            //如果1-8号控制口都已勾选，则跳出循环
        //            if (mkkzk == 255)
        //            {
        //                return;
        //            }
        //        }
        //    }
        //    mkl = string.Empty;
        //    //如果控制口小于255则继续判断交叉控制口和手动控制
        //    if (mkkzk < 255)
        //    {
        //        dt = Swap.sqlc.GetDefTable();
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (dt.Rows[i]["point"].ToString().IndexOf("A") != -1)
        //            {
        //                if (dt.Rows[i]["fzh"].ToString() != Fz.ToString())
        //                {
        //                    mkl += dt.Rows[i]["point"].ToString() + ",";
        //                }
        //            }
        //            else if (dt.Rows[i]["point"].ToString().IndexOf("D") != -1)
        //            {
        //                mkl += dt.Rows[i]["point"].ToString() + ",";
        //            }

        //        }
        //        mkl = mkl.TrimEnd(',');
        //        if (!string.IsNullOrEmpty(mkl))
        //        {
        //            //根据模开量找对应的交叉控制口
        //            for (int i = 0; i < mkl.Split(',').Length; i++)
        //            {
        //                dt = Swap.sqlc.GetDEF(mkl.Split(',')[i].ToString());

        //                if (!string.IsNullOrEmpty(dt.Rows[0]["jckz1"].ToString()))
        //                {
        //                    if (dt.Rows[0]["jckz1"].ToString().Contains("|"))
        //                    {
        //                        for (int j = 0; j < dt.Rows[0]["jckz1"].ToString().Split('|').Length; j++)
        //                        {
        //                            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString()))
        //                            {
        //                                if ((dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString()) == -1)
        //                                {
        //                                    jckzPoints += dt.Rows[0]["jckz1"].ToString().Split('|')[j].ToString() + "|";
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if ((dt.Rows[0]["jckz1"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz1"].ToString()) == -1)
        //                        {
        //                            jckzPoints += dt.Rows[0]["jckz1"].ToString() + "|";
        //                        }
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(dt.Rows[0]["jckz2"].ToString()))
        //                {
        //                    if (dt.Rows[0]["jckz2"].ToString().Contains("|"))
        //                    {
        //                        for (int j = 0; j < dt.Rows[0]["jckz2"].ToString().Split('|').Length; j++)
        //                        {
        //                            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString()))
        //                            {
        //                                if ((dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString()) == -1)
        //                                {
        //                                    jckzPoints += dt.Rows[0]["jckz2"].ToString().Split('|')[j].ToString() + "|";
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if ((dt.Rows[0]["jckz2"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz2"].ToString()) == -1)
        //                        {
        //                            jckzPoints += dt.Rows[0]["jckz2"].ToString() + "|";
        //                        }
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(dt.Rows[0]["jckz3"].ToString()))
        //                {
        //                    if (dt.Rows[0]["jckz3"].ToString().Contains("|"))
        //                    {
        //                        for (int j = 0; j < dt.Rows[0]["jckz3"].ToString().Split('|').Length; j++)
        //                        {
        //                            if (!string.IsNullOrEmpty(dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString()))
        //                            {
        //                                if ((dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString()) == -1)
        //                                {
        //                                    jckzPoints += dt.Rows[0]["jckz3"].ToString().Split('|')[j].ToString() + "|";
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if ((dt.Rows[0]["jckz3"].ToString().Substring(0, 3) == Fz.ToString().PadLeft(3, '0')) && jckzPoints.IndexOf(dt.Rows[0]["jckz3"].ToString()) == -1)
        //                        {
        //                            jckzPoints += dt.Rows[0]["jckz3"].ToString() + "|";
        //                        }
        //                    }
        //                }

        //            }
        //            jckzPoints = jckzPoints.TrimEnd('|');
        //            if (!string.IsNullOrEmpty(jckzPoints))
        //            {
        //                for (int i = 0; i < jckzPoints.Split('|').Length; i++)
        //                {
        //                    switch (Convert.ToInt32(jckzPoints.Split('|')[i].ToString().Substring(4, 2)))
        //                    {
        //                        case 1:
        //                            mkkzk |= 0x01;
        //                            break;
        //                        case 2:
        //                            mkkzk |= 0x02;
        //                            break;
        //                        case 3:
        //                            mkkzk |= 0x04;
        //                            break;
        //                        case 4:
        //                            mkkzk |= 0x08;
        //                            break;
        //                        case 5:
        //                            mkkzk |= 0x10;
        //                            break;
        //                        case 6:
        //                            mkkzk |= 0x20;
        //                            break;
        //                        case 7:
        //                            mkkzk |= 0x40;
        //                            break;
        //                        case 8:
        //                            mkkzk |= 0x80;
        //                            break;
        //                        default:
        //                            break;
        //                    }
        //                    //如果1-8号控制口都已勾选，则跳出循环
        //                    if (mkkzk == 255)
        //                    {
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //        //获取本分站的手动控制
        //        mkkzk |= Swap.fz[Fz - 1].kzzt;
        //    }
        //    #endregion
        //}

        //private bool Check()
        //{
        //    bool value = true;
        //    #region 闭锁区域判断
        //    if (ck_kzk.CheckValue == 0)
        //    {
        //        value = false;
        //        MessageBox.Show("逻辑控制闭锁区域未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //        return value;
        //    }
        //    #endregion
        //    #region 完整性判断
        //    #region 单风机判断
        //    if (cmb_bs_zfjkt_z.SelectedIndex <= 0 || cmb_bs_zfjkt_b.SelectedIndex <= 0)
        //    {
        //        value = false;
        //        MessageBox.Show("主风机闭锁测点号未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //        return value;
        //    }
        //    if (dud_bs_zfjkt_z_value.SelectedIndex < 0 || dud_bs_zfjkt_b_value.SelectedIndex < 0)
        //    {
        //        value = false;
        //        MessageBox.Show("主风机闭锁值未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //        return value;
        //    }
        //    if (dud_js_zfjkt_z_value.SelectedIndex < 0 || dud_js_zfjkt_b_value.SelectedIndex < 0)
        //    {
        //        value = false;
        //        MessageBox.Show("主风机解锁值未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //        return value;
        //    }
        //    #endregion
        //    if (rb_two.Checked)
        //    {
        //        #region 双风机判断
        //        if (cmb_bs_bfjkt_z.SelectedIndex <= 0 || cmb_bs_bfjkt_b.SelectedIndex <= 0)
        //        {
        //            value = false;
        //            MessageBox.Show("副风机闭锁测点号未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //            return value;
        //        }
        //        if (dud_bs_bfjkt_z_value.SelectedIndex < 0 || dud_bs_bfjkt_b_value.SelectedIndex < 0)
        //        {
        //            value = false;
        //            MessageBox.Show("副风机闭锁值未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //            return value;
        //        }
        //        if (dud_js_bfjkt_z_value.SelectedIndex < 0 || dud_js_bfjkt_b_value.SelectedIndex < 0)
        //        {
        //            value = false;
        //            MessageBox.Show("副风机解锁值未选择!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        //            return value;
        //        }
        //        #endregion
        //    }
        //    #endregion
        //    return value;
        //}

        //#region ---开停选择关联---

        //private void cmb_jsbskt_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComboBox _combox = (ComboBox)sender;
        //        switch (_combox.Tag.ToString())
        //        {
        //            case "1":
        //                cmb_js_zfjkt_z.Text = _combox.Text;

        //                #region 加载0 1 2态显示 by tanxingyan 2014-12-11
        //                if (kglxs.ContainsKey(_combox.Text))
        //                {
        //                    dud_bs_zfjkt_z_value.Text = "";
        //                    dud_bs_zfjkt_z_value.Items.Clear();
        //                    dud_bs_zfjkt_z_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_bs_zfjkt_z_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_bs_zfjkt_z_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_js_zfjkt_z_value.Text = "";
        //                    dud_js_zfjkt_z_value.Items.Clear();
        //                    dud_js_zfjkt_z_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_js_zfjkt_z_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_js_zfjkt_z_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_bs_zfjkt_z_value.SelectedIndex = 1;
        //                    dud_js_zfjkt_z_value.SelectedIndex = 2;
        //                }
        //                #endregion
        //                break;
        //            case "2":
        //                cmb_js_zfjkt_b.Text = _combox.Text;

        //                #region 加载0 1 2态显示 by tanxingyan 2014-12-11
        //                if (kglxs.ContainsKey(_combox.Text))
        //                {
        //                    dud_bs_zfjkt_b_value.Text = "";
        //                    dud_bs_zfjkt_b_value.Items.Clear();
        //                    dud_bs_zfjkt_b_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_bs_zfjkt_b_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_bs_zfjkt_b_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_js_zfjkt_b_value.Text = "";
        //                    dud_js_zfjkt_b_value.Items.Clear();
        //                    dud_js_zfjkt_b_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_js_zfjkt_b_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_js_zfjkt_b_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_bs_zfjkt_b_value.SelectedIndex = 1;
        //                    dud_js_zfjkt_b_value.SelectedIndex = 2;
        //                }
        //                #endregion
        //                break;
        //            case "3":
        //                cmb_js_bfjkt_z.Text = _combox.Text;

        //                #region 加载0 1 2态显示 by tanxingyan 2014-12-11
        //                if (kglxs.ContainsKey(_combox.Text))
        //                {
        //                    dud_bs_bfjkt_z_value.Text = "";
        //                    dud_bs_bfjkt_z_value.Items.Clear();
        //                    dud_bs_bfjkt_z_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_bs_bfjkt_z_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_bs_bfjkt_z_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_js_bfjkt_z_value.Text = "";
        //                    dud_js_bfjkt_z_value.Items.Clear();
        //                    dud_js_bfjkt_z_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_js_bfjkt_z_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_js_bfjkt_z_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_bs_bfjkt_z_value.SelectedIndex = 1;
        //                    dud_js_bfjkt_z_value.SelectedIndex = 2;
        //                }
        //                #endregion
        //                break;
        //            case "4":
        //                cmb_js_bfjkt_b.Text = _combox.Text;

        //                #region 加载0 1 2态显示 by tanxingyan 2014-12-11
        //                if (kglxs.ContainsKey(_combox.Text))
        //                {
        //                    dud_bs_bfjkt_b_value.Text = "";
        //                    dud_bs_bfjkt_b_value.Items.Clear();
        //                    dud_bs_bfjkt_b_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_bs_bfjkt_b_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_bs_bfjkt_b_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_js_bfjkt_b_value.Text = "";
        //                    dud_js_bfjkt_b_value.Items.Clear();
        //                    dud_js_bfjkt_b_value.Items.Add(kglxs[_combox.Text][0]);
        //                    dud_js_bfjkt_b_value.Items.Add(kglxs[_combox.Text][1]);
        //                    dud_js_bfjkt_b_value.Items.Add(kglxs[_combox.Text][2]);

        //                    dud_bs_bfjkt_b_value.SelectedIndex = 1;
        //                    dud_js_bfjkt_b_value.SelectedIndex = 2;
        //                }
        //                #endregion
        //                break;
        //        }
        //    }
        //    catch (Exception err) { MessageBox.Show(err.Message); }
        //}

        //#endregion

        //#region 互斥
        //private void cmb_bs_zfjkt_z_DropDown(object sender, EventArgs e)
        //{
        //    cmb_bs_zfjkt_z.Items.Clear();
        //    cmb_bs_zfjkt_z.Items.AddRange(Kgl_array);
        //    if (cmb_bs_zfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_z.Items.Remove(cmb_bs_zfjkt_b.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_z.Items.Remove(cmb_bs_bfjkt_z.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_z.Items.Remove(cmb_bs_bfjkt_b.Text.ToString());
        //    }
        //    cmb_bs_zfjkt_z.Sorted = true;
        //}
        //private void cmb_bs_zfjkt_b_DropDown(object sender, EventArgs e)
        //{
        //    cmb_bs_zfjkt_b.Items.Clear();
        //    cmb_bs_zfjkt_b.Items.AddRange(Kgl_array);
        //    if (cmb_bs_zfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_b.Items.Remove(cmb_bs_zfjkt_z.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_b.Items.Remove(cmb_bs_bfjkt_z.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_zfjkt_b.Items.Remove(cmb_bs_bfjkt_b.Text.ToString());
        //    }
        //    cmb_bs_zfjkt_b.Sorted = true;
        //}
        //private void cmb_bs_bfjkt_z_DropDown(object sender, EventArgs e)
        //{
        //    cmb_bs_bfjkt_z.Items.Clear();
        //    cmb_bs_bfjkt_z.Items.AddRange(Kgl_array);
        //    if (cmb_bs_zfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_z.Items.Remove(cmb_bs_zfjkt_z.Text.ToString());
        //    }

        //    if (cmb_bs_zfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_z.Items.Remove(cmb_bs_zfjkt_b.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_z.Items.Remove(cmb_bs_bfjkt_b.Text.ToString());
        //    }
        //    cmb_bs_bfjkt_z.Sorted = true;
        //}
        //private void cmb_bs_bfjkt_b_DropDown(object sender, EventArgs e)
        //{
        //    cmb_bs_bfjkt_b.Items.Clear();
        //    cmb_bs_bfjkt_b.Items.AddRange(Kgl_array);
        //    if (cmb_bs_zfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_b.Items.Remove(cmb_bs_zfjkt_z.Text.ToString());
        //    }

        //    if (cmb_bs_zfjkt_b.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_b.Items.Remove(cmb_bs_zfjkt_b.Text.ToString());
        //    }

        //    if (cmb_bs_bfjkt_z.SelectedIndex > 0)
        //    {
        //        cmb_bs_bfjkt_b.Items.Remove(cmb_bs_bfjkt_z.Text.ToString());
        //    }
        //    cmb_bs_bfjkt_b.Sorted = true;
        //}
        //#endregion

        ///// <summary>
        ///// 主数据库连接状态检测  2013-1-31 鲁攀 连数据库失败不能修改定义信息。
        ///// </summary>
        ///// <returns></returns>
        //private bool DBCheck()
        //{
        //    bool Db_state = false;
        //    try
        //    {
        //        SqlConnection sql_db = new SqlConnection(Swap.ConnectionString);
        //        sql_db.Open();
        //        sql_db.Close();
        //        Db_state = true;
        //    }
        //    catch { }

        //    return Db_state;
        //}

        ///// <summary>
        ///// 备数据库连接状态检测 连数据库失败不能修改定义信息。
        ///// </summary>
        ///// <returns></returns>
        //private bool DBBCheck()
        //{
        //    bool Db_state = false;
        //    try
        //    {
        //        SqlConnection sql_db = new SqlConnection(Swap.BackupConnString);
        //        sql_db.Open();
        //        sql_db.Close();
        //        Db_state = true;
        //    }
        //    catch { }

        //    return Db_state;
        //}
    }
}

