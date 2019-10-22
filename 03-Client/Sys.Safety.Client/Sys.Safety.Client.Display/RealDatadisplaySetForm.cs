using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Data;
using DevExpress.XtraBars.Ribbon;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Display
{


    public partial class RealDatadisplaySetForm : XtraForm
    {
        #region ==参数==
        /// <summary>
        /// 列数
        /// </summary>
        private int Cloumncount = 13;
        /// <summary>
        /// 当前显示页
        /// </summary>
        private int CruPage = 11;

        /// <summary>
        /// 当前的tabpage
        /// </summary>
        private int Crutabpage = 0;

        /// <summary>
        /// 记录单元格默认值
        /// </summary>
        private string CellValue = "";

        /// <summary>
        /// 记录当前要存储的页 用于固定类型编排
        /// </summary>
        private int Savepage = 0;

        public delegate void mydelegate(int type, int n, List<string> msg);

        public delegate void treedelegate();

        /// <summary>
        /// 委托调用 更改实时显示界面布局
        /// </summary>
        public mydelegate mydel;

        /// <summary>
        /// 委托调用 刷新导航
        /// </summary>
        public treedelegate mydeltree;

        /// <summary>
        /// 当前选定的自定义页面
        /// </summary>
        private int CustomPage = 0;

        /// <summary>
        /// 是否加载完成
        /// </summary>
        private bool LoadTrue = false;

        /// <summary>
        /// 线程
        /// </summary>
        private Thread th;
        #endregion
        public RealDatadisplaySetForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void Loads()
        {
            #region 从本地读取配置信息到内存
            OprFuction.ReadRealDataDisplayConfig();
            OprFuction.ReadDefalutDataConfig();
            OprFuction.ReadCustomConfig();
            #endregion

            LoadTrue = true;
        }

        private void RealDatadisplaySetForm_Load(object sender, EventArgs e)
        {
            this.mydel = new RealDatadisplaySetForm.mydelegate(StaticClass.real_s.Gv_init);
            this.mydeltree = new RealDatadisplaySetForm.treedelegate(StaticClass._type_s.refresh);
            #region 初始化treelist
            for (int i = 0; i < treeList.Nodes.Count; i++)
            {
                treeList.Nodes[i].Tag = i + 1;
                for (int j = 0; j < treeList.Nodes[i].Nodes.Count; j++)
                {
                    treeList.Nodes[i].Nodes[j].Tag = (i + 1) * 10 + j + 1;
                }
            }
            treeList.ExpandAll();
            treeList.FocusedNode = treeList.Nodes[0].Nodes[0];
            CruPage = 11;
            pageshow(true);
            #endregion
            //th = new Thread(new ThreadStart(Loads));
            //th.Start();
            LoadTrue = true;
        }


        #region gv_defalutn
        /// <summary>
        /// 组设置数据源
        /// </summary>
        private DataTable table_defalut;

        /// <summary>
        /// 给gv_defalutn 赋初值
        /// </summary>
        private void SetDefalutMsg()
        {
            #region gv_defalutn赋初值
            try
            {
                DataColumnMsg msg;
                table_defalut = new DataTable();
                table_defalut.Columns.Add("lm", typeof(string));
                table_defalut.Columns.Add("qyxs", typeof(bool));
                table_defalut.Columns.Add("sy", typeof(int));
                if (StaticClass.realdataconfig.DataClnCfg.ColumnsMsg.Length > 0)
                {
                    for (int i = 0; i < StaticClass.realdataconfig.DataClnCfg.ColumnsMsg.Length; i++)
                    {
                        msg = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i];
                        table_defalut.Rows.Add(msg.ColumnName, false, i + 1);
                    }
                }
                gridC_defalut.DataSource = table_defalut;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion
        }
        #endregion

        #region 公共


        /// <summary>
        /// 根据选择不同的菜单调用显示不同的显示页面
        /// </summary>
        /// <param name="flg" >是否刷新</param>
        private void pageshow(bool flg)
        {
            if (flg)
            {
                panel1.Visible = true;
                lb_mr.Text = "默认:当一页显示不完时将会自动分页";
                switch (CruPage)
                {
                    case 11:
                        Savepage = 0;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_realcofign);
                        alarmp.Visible = false;
                        fontp.Visible = false;
                        tablecolumnp.Visible = false;
                        tp_realcofign.Text = "基础配置-[表格属性]";
                        break;
                    case 12:
                        Savepage = 0;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_realcofign);
                        alarmp.Visible = false;
                        fontp.Visible = true;
                        tablecolumnp.Visible = false;
                        tp_realcofign.Text = "基础配置-[字体颜色]";
                        break;
                    case 13:
                        Savepage = 0;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_realcofign);
                        alarmp.Visible = false;
                        fontp.Visible = true;
                        tablecolumnp.Visible = true;
                        tp_realcofign.Text = "基础配置-[列头信息]";
                        break;
                    case 14:
                        Savepage = 0;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_realcofign);
                        alarmp.Visible = true;
                        fontp.Visible = true;
                        tablecolumnp.Visible = true;
                        tp_realcofign.Text = "基础配置-[状态颜色]";
                        break;
                    case 21:
                        Savepage = 7;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_defaltn);
                        tp_defaltn.Text = "导航配置-[设备]";
                        ReadDealutConfig();
                        break;
                    case 22:
                        Savepage = 8;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_defaltn);
                        tp_defaltn.Text = "导航配置-[种类]";
                        ReadDealutConfig();
                        break;
                    case 9:
                        Savepage = 9;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_defaltn);
                        tp_defaltn.Text = "导航配置-[区域]";
                        ReadDealutConfig();
                        break;
                    case 23:
                        Savepage = 10;
                        tbCrln.TabPages.Clear();
                        tbCrln.TabPages.Add(tp_defaltn);
                        tp_defaltn.Text = "导航配置-[状态]";
                        ReadDealutConfig();
                        break;
                    case 10:
                        Savepage = 0;
                        break;
                    case 31:
                        Savepage = 12;
                        tbCrln.TabPages.Clear();
                        panel1.Visible = false;
                        tbCrln.TabPages.Add(tp_defaltn);
                        tp_defaltn.Text = "自定义编排-[" + treeList.FocusedNode.GetDisplayText(0) + "]";
                        ReadDealutConfig();
                        txt_zdy.Text = treeList.FocusedNode.GetDisplayText(0);
                        CustomPage = int.Parse(treeList.FocusedNode.Tag.ToString()) - 30;
                        break;
                }
            }
        }


        /// <summary>
        /// 取消 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool CheckPoint()
        {
            bool revalue = true;
            for (int i = 0; i < table_defalut.Rows.Count; i++)
            {
                if (table_defalut.Rows[i][2].ToString() == "1")
                {
                    if (!(bool)table_defalut.Rows[i][1])
                    {
                        revalue = false;
                        MessageBox.Show("标签名必须选择！！");
                    }
                }
            }
            return revalue;
        }
        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_effect_Click(object sender, EventArgs e)
        {
            if (!CheckPoint())
            {
                return;
            }
            btn_effectn.Enabled = false;
            SaveBaseConfig();
            SaveFontConfig();
            SaveColumnConfig();
            SaveAlarmConfig();
            SaveDealutConfig();
            btn_effectn.Enabled = true;
            //if (mydel != null)
            //{
            //    mydel(0, CustomPage, null);
            //}
            try
            {
                if (mydeltree != null)
                {
                    mydeltree();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
            btn_saveall.Enabled = true;
        }

        /// <summary>
        /// 存储所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveall_Click(object sender, EventArgs e)
        {
            if (!CheckPoint())
            {
                return;
            }
            btn_Cancel.Enabled = false;
            btn_saveall.Text = "存储中..";
            btn_saveall.Enabled = false;
            OprFuction.SaveRealDataDisplayConfig();
            OprFuction.SaveDefalutDataConfig();
            OprFuction.SaveCustomConfig();
            OprFuction.SaveRealConfigToDB();
            btn_saveall.Text = "保 存";
            btn_Cancel.Enabled = true;
        }

        #endregion

        #region  基础设置
        /// <summary>
        /// 预览数据源
        /// </summary>
        private DataTable showdt;

        /// <summary>
        /// 预览数据源初始化
        /// </summary>
        private void showdtinit()
        {
            showdt = new DataTable();
            showdt.Columns.Add("point1", typeof(string));
            showdt.Columns.Add("wz1", typeof(string));
            showdt.Columns.Add("ssz1", typeof(string));
            showdt.Columns.Add("s1", typeof(string));
            showdt.Columns.Add("point2", typeof(string));
            showdt.Columns.Add("wz2", typeof(string));
            showdt.Columns.Add("ssz2", typeof(string));
        }

        private void gv_showinit()
        {
            gv_shown.Columns.Clear();
            DevExpress.XtraGrid.Columns.GridColumn col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "测点号";
            col.FieldName = "point1";
            col.Width = 80;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "位置";
            col.FieldName = "wz1";
            col.Width = 140;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "实时值";
            col.FieldName = "ssz1";
            col.Width = 80;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "";
            col.FieldName = "s1";
            col.Width = 2;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "测点号";
            col.FieldName = "point2";
            col.Width = 80;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "位置";
            col.FieldName = "wz2";
            col.Width = 140;
            col.Visible = true;
            gv_shown.Columns.Add(col);
            col = new DevExpress.XtraGrid.Columns.GridColumn();
            col.Caption = "实时值";
            col.FieldName = "ssz2";
            col.Width = 80;
            col.Visible = true;
            gv_shown.Columns.Add(col);
        }

        /// <summary>
        /// 存储信息到内存
        /// </summary>
        private void SaveBaseConfig()
        {
            try
            {
                #region 颜色存储
                StaticClass.realdataconfig.BaseCfg.TableHadeBackColor = pic1.Color;
                StaticClass.realdataconfig.BaseCfg.SingleRowColor = pic2.Color;
                StaticClass.realdataconfig.BaseCfg.GridColor = pic3.Color;
                StaticClass.realdataconfig.BaseCfg.SelectColor = pic4.Color;
                StaticClass.realdataconfig.BaseCfg.DoubleRowColor = pic5.Color;
                StaticClass.realdataconfig.BaseCfg.SplitColor = pic6.Color;
                StaticClass.realdataconfig.BaseCfg.GvBackColor = pic7.Color;
                #endregion

                #region 其它存储
                StaticClass.realdataconfig.BaseCfg.Colorchange = getstyle();
                StaticClass.realdataconfig.BaseCfg.Showju = cmb_ju.Checked;
                int n = 0;
                #region 数据行高
                if (int.TryParse(cmb_DataRowHigh.Text, out n))
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = n;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.DataRowHigh = 20;
                }
                #endregion

                #region 表头行高
                if (int.TryParse(cmb_TableHadehigh.Text, out n))
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = n;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.TableHadeHigh = 20;
                }
                #endregion

                #region  分隔线宽
                if (int.TryParse(cmb_SplitLineWidth.Text, out n))
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = n;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.SplitWidth = 20;
                }
                #endregion

                #region 自动切换间隔时间
                if (int.TryParse(cmb_Interval.Text, out n))
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = n;
                }
                else
                {
                    StaticClass.realdataconfig.BaseCfg.PageChangeInterval = 20;
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 从内存读取信息到界面
        /// </summary>
        private void ReadBaseConfig()
        {
            #region 颜色存储
            try
            {

                pic1.Color = StaticClass.realdataconfig.BaseCfg.TableHadeBackColor;
                pic2.Color = StaticClass.realdataconfig.BaseCfg.SingleRowColor;
                pic3.Color = StaticClass.realdataconfig.BaseCfg.GridColor;
                pic4.Color = StaticClass.realdataconfig.BaseCfg.SelectColor;
                pic5.Color = StaticClass.realdataconfig.BaseCfg.DoubleRowColor;
                pic6.Color = StaticClass.realdataconfig.BaseCfg.SplitColor;
                pic7.Color = StaticClass.realdataconfig.BaseCfg.GvBackColor;
                setbut(StaticClass.realdataconfig.BaseCfg.Colorchange);
                cmb_ju.Checked = StaticClass.realdataconfig.BaseCfg.Showju;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion

            #region 其它存储
            int n = 0;
            try
            {
                #region 数据行高
                n = StaticClass.realdataconfig.BaseCfg.DataRowHigh;
                if (n >= 10 && n <= 100)
                {
                    cmb_DataRowHigh.Text = n.ToString();
                }
                else
                {
                    cmb_DataRowHigh.Text = "20";
                }
                #endregion

                #region 表头行高
                n = StaticClass.realdataconfig.BaseCfg.TableHadeHigh;
                if (n >= 10 && n <= 100)
                {
                    cmb_TableHadehigh.Text = n.ToString();
                }
                else
                {
                    cmb_TableHadehigh.Text = "20";
                }
                #endregion

                #region  分隔线宽
                n = StaticClass.realdataconfig.BaseCfg.SplitWidth;
                if (n >= 1 && n <= 100)
                {
                    cmb_SplitLineWidth.Text = n.ToString();
                }
                else
                {
                    cmb_SplitLineWidth.Text = "2";
                }
                #endregion

                #region 自动切换间隔时间
                n = StaticClass.realdataconfig.BaseCfg.PageChangeInterval;
                try
                {
                    cmb_Interval.Text = n.ToString();
                }
                catch
                {
                    cmb_Interval.Text = "5";
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            #endregion
        }

        /// <summary>
        /// 基础信息设置后预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_baseview_Click(object sender, EventArgs e)
        {
            try
            {

                #region 初始化表

                #region 初始化列
                try
                {
                    showdtinit();
                    gv_showinit();
                    gridC_show.DataSource = showdt;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion

                #region 赋初值
                try
                {
                    showdt.Rows.Add("00100000", "T1工作面[KJF86N(16)]", "交流正常", "", "00200000", "T2工作面[KJF86N(16)]", "通讯中断");
                    showdt.Rows.Add("001A0100", "T1工作面[瓦斯1]", "0.8%", "", "003A0200", "T2工作面[瓦斯1]", "1.9%");
                    showdt.Rows.Add("001D0101", "T1工作面[瓦斯2]", "1.0%", "", "003D0201", "T2工作面[瓦斯2]", "负漂");
                    showdt.Rows.Add("001D0102", "T1工作面[泵]", "断线", "", "003A0202", "T2工作面[温度]", "-50℃");
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion

                #endregion

                #region 赋基础值
                try
                {
                    int n = 0;
                    n = int.Parse(cmb_TableHadehigh.Text);
                    gv_shown.ColumnPanelRowHeight = n;
                    n = int.Parse(cmb_DataRowHigh.Text);
                    gv_shown.RowHeight = n;
                    n = int.Parse(cmb_SplitLineWidth.Text);
                    gv_shown.Columns[3].Width = n;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                try
                {
                    gv_shown.OptionsView.EnableAppearanceEvenRow = cmb_ju.Checked;
                    gv_shown.OptionsView.EnableAppearanceOddRow = cmb_ju.Checked;
                    gv_shown.Appearance.Row.BackColor = pic2.Color;
                    gv_shown.Appearance.Row.BackColor2 = pic1.Color;
                    gv_shown.Appearance.OddRow.BackColor = pic2.Color;
                    gv_shown.Appearance.EvenRow.BackColor = pic5.Color;
                    gv_shown.Appearance.OddRow.BackColor2 = pic1.Color;
                    gv_shown.Appearance.EvenRow.BackColor2 = pic7.Color;
                    gv_shown.Appearance.FocusedCell.BackColor = pic4.Color;
                    gv_shown.Appearance.VertLine.BackColor = gv_shown.Appearance.HorzLine.BackColor = pic3.Color;
                    gv_shown.Columns[3].AppearanceCell.BackColor = pic6.Color;
                    //gv_show.EnableHeadersVisualStyles = false;
                    gv_shown.Appearance.HeaderPanel.BackColor = pic1.BackColor;
                    //gv_shown.Appearance .ColumnHeadersDefaultCellStyle.BackColor = pic1.BackColor;
                    //gv_shown.Appearance .bac.BackgroundColor = pic7.BackColor;
                    //if (cmb_colorchange .SelectedIndex  == 0)
                    //{
                    //    gv_shown.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                    //    gv_shown.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                    //    gv_shown.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                    //}
                    //else if (cmb_colorchange.SelectedIndex == 1)
                    //{
                    //    gv_shown.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    //    gv_shown.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    //    gv_shown.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    //}
                    //else if (cmb_colorchange.SelectedIndex == 2)
                    //{
                    //    gv_shown.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    //    gv_shown.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    //    gv_shown.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    //}
                    //else if (cmb_colorchange.SelectedIndex == 3)
                    //{
                    //    gv_shown.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                    //    gv_shown.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                    //    gv_shown.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                    //}

                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
        #endregion

        #region 字体配置

        /// <summary>
        /// 存储字体配置
        /// </summary>
        private void SaveFontConfig()
        {
            try
            {
                #region 颜色存储
                StaticClass.realdataconfig.FontCfg.TableHadeFontColor = pic_tablefontcor.Color;
                StaticClass.realdataconfig.FontCfg.DataFontColor = pic_datafontcor.Color;
                #endregion

                #region 其它存储
                int n = 0;
                #region 表头字体大小
                if (int.TryParse(cmb_tablefontsize.Text, out n))
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = n;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.TableHadeFontSize = 10;
                }
                #endregion

                try
                {
                    StaticClass.realdataconfig.BaseCfg.Bjfontsize = float.Parse(bjfont.Text);
                }
                catch
                {
                    StaticClass.realdataconfig.BaseCfg.Bjfontsize = 8;
                }

                #region 数据字体大小
                if (int.TryParse(cmb_datafontsize.Text, out n))
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = n;
                }
                else
                {
                    StaticClass.realdataconfig.FontCfg.DataFontSize = 10;
                }
                #endregion

                #region  字体名称
                StaticClass.realdataconfig.FontCfg.TableHadeFontName = cmb_tablefontname.Text;
                StaticClass.realdataconfig.FontCfg.DataFontName = cmb_datafontname.Text;
                #endregion

                #region 其它
                StaticClass.realdataconfig.FontCfg.IsBold = ckb_tablefontbold.Checked;
                StaticClass.realdataconfig.FontCfg.IsHaveUnderLine = ckb_tablefontxhx.Checked;
                StaticClass.realdataconfig.FontCfg.IsItalic = ckb_tablefontxt.Checked;
                StaticClass.realdataconfig.FontCfg.DataIsBold = ckb_datafontbold.Checked;
                StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine = ckb_datafontxhx.Checked;
                StaticClass.realdataconfig.FontCfg.DataIsItalic = ckb_datafontxt.Checked;
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 读取字体配置
        /// </summary>
        private void ReadFontConfig()
        {
            try
            {
                #region 颜色存储
                pic_tablefontcor.Color = StaticClass.realdataconfig.FontCfg.TableHadeFontColor;
                pic_datafontcor.Color = StaticClass.realdataconfig.FontCfg.DataFontColor;
                #endregion

                #region 其它存储
                int n = 0;
                #region 表头字体大小
                n = StaticClass.realdataconfig.FontCfg.TableHadeFontSize;
                if (n >= 5 && n <= 50)
                {
                    cmb_tablefontsize.Text = n.ToString();
                }
                else
                {
                    cmb_tablefontsize.Text = "10";
                }
                #endregion

                bjfont.Text = StaticClass.realdataconfig.BaseCfg.Bjfontsize.ToString();

                #region 数据字体大小
                n = StaticClass.realdataconfig.FontCfg.DataFontSize;
                if (n >= 5 && n <= 50)
                {
                    cmb_datafontsize.Text = n.ToString();
                }
                else
                {
                    cmb_datafontsize.Text = "10";
                }
                #endregion

                #region  字体名称
                try
                {
                    cmb_tablefontname.Text = StaticClass.realdataconfig.FontCfg.TableHadeFontName;
                }
                catch
                {
                    cmb_tablefontname.SelectedIndex = 0;
                }
                try
                {
                    cmb_datafontname.Text = StaticClass.realdataconfig.FontCfg.DataFontName;
                }
                catch
                {
                    cmb_datafontname.SelectedIndex = 0;
                }
                #endregion

                #region 其它
                ckb_tablefontbold.Checked = StaticClass.realdataconfig.FontCfg.IsBold;
                ckb_tablefontxhx.Checked = StaticClass.realdataconfig.FontCfg.IsHaveUnderLine;
                ckb_tablefontxt.Checked = StaticClass.realdataconfig.FontCfg.IsItalic;
                ckb_datafontbold.Checked = StaticClass.realdataconfig.FontCfg.DataIsBold;
                ckb_datafontxhx.Checked = StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine;
                ckb_datafontxt.Checked = StaticClass.realdataconfig.FontCfg.DataIsItalic;
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 字体配置预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_fontview_Click(object sender, EventArgs e)
        {
            btn_baseview_Click(null, null);

            #region 设置字体信息
            try
            {

                gv_shown.Appearance.HeaderPanel.ForeColor = pic_tablefontcor.Color;
                gv_shown.Appearance.HeaderPanel.Font = GetFont(cmb_tablefontname.Text, int.Parse(cmb_tablefontsize.Text), ckb_tablefontbold.Checked, ckb_tablefontxt.Checked, ckb_tablefontxhx.Checked);
                gv_shown.Appearance.Row.Font = GetFont(cmb_datafontname.Text, int.Parse(cmb_datafontsize.Text), ckb_datafontbold.Checked, ckb_datafontxt.Checked, ckb_datafontxhx.Checked);
                gv_shown.Appearance.Row.ForeColor = pic_datafontcor.Color;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            #endregion
        }

        /// <summary>
        /// 生成字体
        /// </summary>
        /// <param name="name">字体名称</param>
        /// <param name="siz">字体大小</param>
        /// <param name="bold">是否加粗</param>
        /// <param name="italic">是否斜体</param>
        /// <param name="underline">是否下划线</param>
        /// <returns></returns>
        private Font GetFont(string name, int siz, bool bold, bool italic, bool underline)
        {
            Font newfont = new Font(name, siz);
            int n = 0;
            try
            {
                #region 模式判断
                if (bold && !italic && !underline)
                {
                    n = 1;
                }
                else if (!bold && italic && !underline)
                {
                    n = 2;
                }
                else if (!bold && !italic && underline)
                {
                    n = 3;
                }
                else if (bold && italic && !underline)
                {
                    n = 4;
                }
                else if (bold && !italic && underline)
                {
                    n = 5;
                }
                else if (!bold && italic && underline)
                {
                    n = 6;
                }
                else if (bold && italic && underline)
                {
                    n = 7;
                }
                else
                {
                    n = 8;
                }
                #endregion

                #region 生成
                switch (n)
                {
                    case 1:
                        newfont = new Font(name, siz, FontStyle.Bold);
                        break;
                    case 2:
                        newfont = new Font(name, siz, FontStyle.Italic);
                        break;
                    case 3:
                        newfont = new Font(name, siz, FontStyle.Underline);
                        break;
                    case 4:
                        newfont = new Font(name, siz, FontStyle.Bold | FontStyle.Italic);
                        break;
                    case 5:
                        newfont = new Font(name, siz, FontStyle.Bold | FontStyle.Underline);
                        break;
                    case 6:
                        newfont = new Font(name, siz, FontStyle.Italic | FontStyle.Underline);
                        break;
                    case 7:
                        newfont = new Font(name, siz, FontStyle.Bold | FontStyle.Italic | FontStyle.Underline);
                        break;
                    case 8:
                        newfont = new Font(name, siz);
                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return newfont;
        }
        #endregion

        #region 列配置

        /// <summary>
        /// 固定列数据源
        /// </summary>
        private DataTable gdltb;

        /// <summary>
        /// 初始化列
        /// </summary>
        private void gdltbinit()
        {
            gdltb = new DataTable();
            gdltb.Columns.Add("lm", typeof(string));
            gdltb.Columns.Add("zdylm", typeof(string));
            gdltb.Columns.Add("lk", typeof(double));
            gdltb.Columns.Add("dqfs", typeof(string));
            gdltb.Columns.Add("sdlk", typeof(bool));
            for (int i = 0; i < StaticClass.showcol.Length; i++)
            {
                gdltb.Rows.Add(StaticClass.showcol[i], StaticClass.showcol[i], 100, "左对齐", false);
            }
            gridC_Col.DataSource = gdltb;
        }

        /// <summary>
        /// 单元格开始修改事件 记录修改前的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_columns_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if ((e.Column.FieldName == "lk" && e.RowHandle > -1) || (e.Column.FieldName == "zdylm" && e.RowHandle > -1))
                {
                    CellValue = gv_columns.GetRowCellValue(e.RowHandle, e.Column).ToString();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 单元格结束修改事件 检测修改后的值是否符合要求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gv_columns_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            object strvalue = null;
            int intvalue = 0;
            try
            {
                if (e.Column.FieldName == "zdylm" && e.RowHandle > -1)
                {
                    #region 列名判断
                    strvalue = gv_columns.GetRowCellValue(e.RowHandle, e.Column);
                    if (strvalue != null)
                    {
                        if (strvalue.ToString() != "")
                        {
                            if (strvalue.ToString().Length > 20)
                            {
                                gv_columns.SetRowCellValue(e.RowHandle, e.Column, CellValue);
                                throw new Exception("设置列名长度太长，有效长度20个汉字以内，请重新设置！");
                            }
                        }
                        else
                        {
                            gv_columns.SetRowCellValue(e.RowHandle, e.Column, CellValue);
                            throw new Exception("列名不能设置空值，请重新设置！");
                        }
                    }
                    else
                    {
                        gv_columns.SetRowCellValue(e.RowHandle, e.Column, CellValue);
                        throw new Exception("列名不能设置空值，请重新设置！");
                    }
                    #endregion
                }
                else if (e.Column.FieldName == "lk" && e.RowHandle > -1)
                {
                    #region 列宽判断
                    try
                    {
                        strvalue = gv_columns.GetRowCellValue(e.RowHandle, e.Column);
                        intvalue = int.Parse(strvalue.ToString());
                    }
                    catch
                    {
                        gv_columns.SetRowCellValue(e.RowHandle, e.Column, CellValue);
                        throw new Exception("列宽设置错误，有效范围1-9999，请重新设置！");
                    }
                    if (intvalue < 1 || intvalue > 9999)
                    {
                        gv_columns.SetRowCellValue(e.RowHandle, e.Column, CellValue);
                        throw new Exception("列宽设置错误，有效范围1-9999，请重新设置！");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 存储列信息
        /// </summary>
        private void SaveColumnConfig()
        {
            try
            {
                #region 存储列设置信息到内存
                StaticClass.realdataconfig.DataClnCfg.ShowUnit = ckb_showunit.Checked;
                DataColumnMsg clmmsg = new DataColumnMsg();
                for (int i = 0; i < gdltb.Rows.Count; i++)
                {
                    clmmsg = new DataColumnMsg();
                    clmmsg.ColumnName = gdltb.Rows[i][1].ToString();
                    clmmsg.ColumnWidth = int.Parse(gdltb.Rows[i][2].ToString());
                    clmmsg.ColumnType = gdltb.Rows[i][3].ToString();
                    clmmsg.IsLocked = (bool)gdltb.Rows[i][4];
                    StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i] = clmmsg;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 读取列设置信息
        /// </summary>
        private void ReadColumnConfig()
        {
            try
            {
                ckb_showunit.Checked = StaticClass.realdataconfig.DataClnCfg.ShowUnit;
                for (int i = 0; i < gdltb.Rows.Count; i++)
                {
                    gdltb.Rows[i][1] = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnName;
                    gdltb.Rows[i][2] = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnWidth;
                    gdltb.Rows[i][3] = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].ColumnType;
                    gdltb.Rows[i][4] = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[i].IsLocked;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 列设置预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clomnview_Click(object sender, EventArgs e)
        {
            btn_fontview_Click(null, null);

            #region 列设置预览
            string str = "";
            gv_shown.Columns[0].Caption = gv_shown.Columns[4].Caption = gdltb.Rows[0][1].ToString();
            gv_shown.Columns[1].Caption = gv_shown.Columns[5].Caption = gdltb.Rows[1][1].ToString();
            gv_shown.Columns[2].Caption = gv_shown.Columns[6].Caption = gdltb.Rows[2][1].ToString();

            gv_shown.Columns[0].Width = gv_shown.Columns[4].Width = int.Parse(gdltb.Rows[0][2].ToString());
            gv_shown.Columns[1].Width = gv_shown.Columns[5].Width = int.Parse(gdltb.Rows[0][2].ToString());
            gv_shown.Columns[2].Width = gv_shown.Columns[6].Width = int.Parse(gdltb.Rows[0][2].ToString());
            str = gdltb.Rows[0][3].ToString();
            if (str == "右对齐")
            {
                gv_shown.Columns[4].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gv_shown.Columns[4].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else if (str == "居中")
            {
                gv_shown.Columns[4].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                gv_shown.Columns[4].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
            else
            {
                gv_shown.Columns[4].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gv_shown.Columns[4].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            str = gdltb.Rows[1][3].ToString();
            if (str == "右对齐")
            {
                gv_shown.Columns[5].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gv_shown.Columns[5].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else if (str == "居中")
            {
                gv_shown.Columns[5].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ;
                gv_shown.Columns[5].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
            else
            {
                gv_shown.Columns[5].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gv_shown.Columns[5].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            str = gdltb.Rows[2][3].ToString();
            if (str == "右对齐")
            {
                gv_shown.Columns[6].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gv_shown.Columns[6].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            }
            else if (str == "居中")
            {
                gv_shown.Columns[6].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                ;
                gv_shown.Columns[6].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            }
            else
            {
                gv_shown.Columns[6].AppearanceCell.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                gv_shown.Columns[6].AppearanceHeader.TextOptions.HAlignment = gv_shown.Columns[2].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            }

            #endregion
        }
        #endregion

        #region 状态颜色设置

        /// <summary>
        /// 存储报警颜色显示配置到内存
        /// </summary>
        private void SaveAlarmConfig()
        {
            try
            {
                #region 存储状态颜色配置
                StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor = pic_upprealarm.Color;
                StaticClass.realdataconfig.StateCorCfg.UpAlarmColor = pic_upalarm.Color;
                StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor = pic_upbrackdown.Color;
                StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor = pic_lowpredown.Color;
                StaticClass.realdataconfig.StateCorCfg.LowAlarmColor = pic_lowalarm.Color;
                StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor = pic_lowbrackdown.Color;
                StaticClass.realdataconfig.StateCorCfg.KAlarmColor = pic_kalarm.Color;
                //StaticClass.realdataconfig.StateCorCfg.KBlackOutColor = pic_kbrackdown.Color;
                StaticClass.realdataconfig.StateCorCfg.DefaultColor = pic_daluft.Color;
                StaticClass.realdataconfig.StateCorCfg.InterruptionColor = pic_interrupt.Color;
                StaticClass.realdataconfig.StateCorCfg.OverRangeColor = pic_clc.Color;
                StaticClass.realdataconfig.StateCorCfg.DcColor = pic_dc.Color;
                StaticClass.realdataconfig.StateCorCfg.EffectColor = colorEffect.Color;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 读取报警颜色显示配置到界面
        /// </summary>
        private void ReadAlarmConfig()
        {
            try
            {
                #region 读取状态颜色配置
                pic_upprealarm.Color = StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor;
                pic_upalarm.Color = StaticClass.realdataconfig.StateCorCfg.UpAlarmColor;
                pic_upbrackdown.Color = StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor;
                pic_lowpredown.Color = StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor;
                pic_lowalarm.Color = StaticClass.realdataconfig.StateCorCfg.LowAlarmColor;
                pic_lowbrackdown.Color = StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor;
                pic_kalarm.Color = StaticClass.realdataconfig.StateCorCfg.KAlarmColor;
                //pic_kbrackdown.BackColor = StaticClass.realdataconfig.StateCorCfg.KBlackOutColor;
                pic_daluft.Color = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                pic_interrupt.Color = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
                pic_clc.Color = StaticClass.realdataconfig.StateCorCfg.OverRangeColor;
                pic_dc.Color = StaticClass.realdataconfig.StateCorCfg.DcColor;
                colorEffect.Color = StaticClass.realdataconfig.StateCorCfg.EffectColor;
                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
        private void gv_shown_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            if (e.Column.FieldName.Contains("ssz"))
            {
                if (e.CellValue.ToString().Equals("1.9%"))
                {
                    e.Appearance.ForeColor = pic_upbrackdown.Color;
                }
                else if (e.CellValue.ToString().Equals("0.8%"))
                {
                    e.Appearance.ForeColor = pic_upalarm.Color;
                }
                else if (e.CellValue.ToString().Equals("1.0%"))
                {
                    e.Appearance.ForeColor = pic_upprealarm.Color;
                }
                else if (e.CellValue.ToString().Equals("断线"))
                {
                    e.Appearance.ForeColor = pic_kalarm.Color;
                }
                else if (e.CellValue.ToString().Equals("通讯中断"))
                {
                    e.Appearance.ForeColor = pic_interrupt.Color;
                }
                else if (e.CellValue.ToString().Equals("-50℃"))
                {
                    e.Appearance.ForeColor = pic_lowbrackdown.Color;
                }
                else if (e.CellValue.ToString().Equals("负漂"))
                {
                    e.Appearance.ForeColor = pic_clc.Color;
                }
                else
                {
                    e.Appearance.ForeColor = pic_daluft.Color;
                }
            }

        }
        /// <summary>
        /// 状态预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_alarmshown_Click(object sender, EventArgs e)
        {
            btn_clomnview_Click(null, null);
            #region 状态赋值预览
            try
            {
                gv_shown.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gv_shown_CustomDrawCell);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            #endregion
        }
        #endregion

        #region 固定编排及自定义编排

        /// <summary>
        /// 移至顶层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_upmoveall_Click(object sender, EventArgs e)
        {
            int n = 0;
            string str1 = "", str2 = "";
            bool clm = false;
            try
            {
                #region 移至顶层
                if (table_defalut.Rows.Count > 1)
                {
                    if (gv_defalutn.FocusedRowHandle > 0)
                    {
                        n = gv_defalutn.FocusedRowHandle;
                    }
                    if (n > 0)
                    {
                        str1 = table_defalut.Rows[n][0].ToString();
                        clm = (bool)table_defalut.Rows[n][1];
                        str2 = table_defalut.Rows[n][2].ToString();

                        for (int i = n; i > 0; i--)
                        {
                            table_defalut.Rows[i][0] = table_defalut.Rows[i - 1][0];
                            table_defalut.Rows[i][1] = (bool)table_defalut.Rows[i - 1][1];
                            table_defalut.Rows[i][2] = table_defalut.Rows[i - 1][2];
                        }

                        table_defalut.Rows[0][0] = str1;
                        table_defalut.Rows[0][1] = clm;
                        table_defalut.Rows[0][2] = str2;
                        gv_defalutn.FocusedRowHandle = 0;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 移至底层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_downmoveall_Click(object sender, EventArgs e)
        {
            int n = 0;
            string str1 = "", str2 = "";
            bool clm = false;
            try
            {
                #region 移至底层
                if (table_defalut.Rows.Count > 1)
                {
                    if (gv_defalutn.FocusedRowHandle >= 0)
                    {
                        n = gv_defalutn.FocusedRowHandle;
                    }
                    if (n < table_defalut.Rows.Count - 1)
                    {
                        str1 = table_defalut.Rows[n][0].ToString();
                        clm = (bool)table_defalut.Rows[n][1];
                        str2 = table_defalut.Rows[n][2].ToString();

                        for (int i = n; i < table_defalut.Rows.Count - 1; i++)
                        {
                            table_defalut.Rows[i][0] = table_defalut.Rows[i + 1][0];
                            table_defalut.Rows[i][1] = table_defalut.Rows[i + 1][1];
                            table_defalut.Rows[i][2] = table_defalut.Rows[i + 1][2];
                        }

                        table_defalut.Rows[table_defalut.Rows.Count - 1][0] = str1;
                        table_defalut.Rows[table_defalut.Rows.Count - 1][1] = clm;
                        table_defalut.Rows[table_defalut.Rows.Count - 1][2] = str2;
                        gv_defalutn.FocusedRowHandle = table_defalut.Rows.Count - 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_upmove_Click(object sender, EventArgs e)
        {
            int n = 0;
            string str1 = "", str2 = "";
            bool clm = false;
            try
            {
                #region 上移
                if (table_defalut.Rows.Count > 1)
                {
                    if (gv_defalutn.FocusedRowHandle >= 0)
                    {
                        n = gv_defalutn.FocusedRowHandle;
                    }
                    if (n > 0)
                    {
                        str1 = table_defalut.Rows[n][0].ToString();
                        clm = (bool)table_defalut.Rows[n][1];
                        str2 = table_defalut.Rows[n][2].ToString();

                        table_defalut.Rows[n][0] = table_defalut.Rows[n - 1][0];
                        table_defalut.Rows[n][1] = table_defalut.Rows[n - 1][1];
                        table_defalut.Rows[n][2] = table_defalut.Rows[n - 1][2];

                        table_defalut.Rows[n - 1][0] = str1;
                        table_defalut.Rows[n - 1][1] = clm;
                        table_defalut.Rows[n - 1][2] = str2;
                        gv_defalutn.FocusedRowHandle = n - 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_downmove_Click(object sender, EventArgs e)
        {
            int n = 0;
            string str1 = "", str2 = "";
            bool clm = false;
            try
            {
                #region 下移
                if (table_defalut.Rows.Count > 1)
                {
                    if (gv_defalutn.FocusedRowHandle >= 0)
                    {
                        n = gv_defalutn.FocusedRowHandle;
                    }
                    if (n < table_defalut.Rows.Count - 1)
                    {
                        str1 = table_defalut.Rows[n][0].ToString();
                        clm = (bool)table_defalut.Rows[n][1];
                        str2 = table_defalut.Rows[n][2].ToString();

                        table_defalut.Rows[n][0] = table_defalut.Rows[n + 1][0];
                        table_defalut.Rows[n][1] = (bool)table_defalut.Rows[n + 1][1];
                        table_defalut.Rows[n][2] = table_defalut.Rows[n + 1][2];

                        table_defalut.Rows[n + 1][0] = str1;
                        table_defalut.Rows[n + 1][1] = clm;
                        table_defalut.Rows[n + 1][2] = str2;
                        gv_defalutn.FocusedRowHandle = n + 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 获取显示信息顺序
        /// </summary>
        /// <returns></returns> 
        private colummsg[] GetColumnSetMsg()
        {
            colummsg[] col = new colummsg[StaticClass.showcol.Length];
            colummsg cl = new colummsg();
            try
            {
                #region 显示顺序
                for (int i = 0; i < table_defalut.Rows.Count; i++)
                {
                    cl = new colummsg();
                    cl.Index = int.Parse(table_defalut.Rows[i][2].ToString());
                    cl.Isuse = (bool)table_defalut.Rows[i][1];
                    col[i] = cl;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return col;
        }

        /// <summary>
        /// 获取组显示信息顺序
        /// </summary>
        /// <returns></returns>
        private string GetColumnMsg()
        {
            string str = "";
            try
            {
                #region 组显示信息
                for (int i = 0; i < table_defalut.Rows.Count; i++)
                {
                    if ((bool)table_defalut.Rows[i][1])
                    {
                        str += string.Format("{0}|", table_defalut.Rows[i][2].ToString());
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            return str;
        }

        /// <summary>
        /// 固定显示编排及自定义编排设置存储到内存
        /// </summary>
        private void SaveDealutConfig()
        {
            try
            {
                int m = 0;
                string str = "";
                switch (Savepage)
                {
                    case 7:
                        #region 存储测点类型编排
                        StaticClass.arrangeconfig.TypeConfig.IsColumnsMsg = GetColumnMsg();
                        StaticClass.arrangeconfig.TypeConfig.Column = GetColumnSetMsg();
                        StaticClass.arrangeconfig.TypeConfig.IsDataFillType = (bool)radioGroup_tc.EditValue;
                        StaticClass.arrangeconfig.TypeConfig.IsUpIndex = (bool)radioGroup_px.EditValue;
                        StaticClass.arrangeconfig.TypeConfig.PageSortType = cmb_sorttype.SelectedIndex + 1;
                        StaticClass.arrangeconfig.TypeConfig.ShowColumnCount = int.Parse(cmb_rowcount.Text);
                        StaticClass.arrangeconfig.TypeConfig.ShowRowCount = int.Parse(cmb_pagecount.Text);
                        #endregion
                        break;
                    case 8:
                        #region 存储网络模块编排
                        StaticClass.arrangeconfig.NetConfig.IsColumnsMsg = GetColumnMsg();
                        StaticClass.arrangeconfig.NetConfig.Column = GetColumnSetMsg();
                        StaticClass.arrangeconfig.NetConfig.IsDataFillType = (bool)radioGroup_tc.EditValue;
                        StaticClass.arrangeconfig.NetConfig.IsUpIndex = (bool)radioGroup_px.EditValue;
                        StaticClass.arrangeconfig.NetConfig.PageSortType = cmb_sorttype.SelectedIndex + 1;
                        StaticClass.arrangeconfig.NetConfig.ShowColumnCount = int.Parse(cmb_rowcount.Text);
                        StaticClass.arrangeconfig.NetConfig.ShowRowCount = int.Parse(cmb_pagecount.Text);
                        #endregion
                        break;
                    case 10:
                        #region 存储状态编排
                        StaticClass.arrangeconfig.StateConfig.IsColumnsMsg = GetColumnMsg();
                        StaticClass.arrangeconfig.StateConfig.Column = GetColumnSetMsg();
                        StaticClass.arrangeconfig.StateConfig.IsDataFillType = (bool)radioGroup_tc.EditValue;
                        StaticClass.arrangeconfig.StateConfig.IsUpIndex = (bool)radioGroup_px.EditValue;
                        StaticClass.arrangeconfig.StateConfig.PageSortType = cmb_sorttype.SelectedIndex + 1;
                        StaticClass.arrangeconfig.StateConfig.ShowColumnCount = int.Parse(cmb_rowcount.Text);
                        StaticClass.arrangeconfig.StateConfig.ShowRowCount = int.Parse(cmb_pagecount.Text);
                        #endregion
                        break;
                    case 12:
                        #region 存储自定义编排
                        m = 0;
                        if (treeList.FocusedNode != null)
                        {
                            str = treeList.FocusedNode.Tag.ToString();
                            m = int.Parse(str) - 30;
                        }
                        if (m > 0)
                        {
                            PageSetConfig page = new PageSetConfig();
                            page.PageName = txt_zdy.Text.Trim();
                            page.IsColumnsMsg = GetColumnMsg();
                            page.Column = GetColumnSetMsg();
                            page.Page = m;
                            if (StaticClass.arrangeconfig.CustomCofig[m - 1] != null)
                            {
                                page.ShowColumnCount = StaticClass.arrangeconfig.CustomCofig[m - 1].ShowColumnCount;
                                page.ShowRowCount = StaticClass.arrangeconfig.CustomCofig[m - 1].ShowRowCount;
                            }
                            else
                            {
                                page.ShowColumnCount = 2;
                                page.ShowRowCount = 30;
                            }
                            StaticClass.arrangeconfig.CustomCofig[m - 1] = page;
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 固定显示编排及自定义编排设置读取到界面
        /// </summary>
        private void ReadDealutConfig()
        {
            try
            {
                int n = 0, m = 0, k = 0;
                string str = "";
                switch (Savepage)
                {
                    case 7:
                        #region 读取固定类型编排设置信息
                        if (StaticClass.arrangeconfig.TypeConfig.Column[0] != null && StaticClass.arrangeconfig.TypeConfig.Column[0].Index != 0)
                        {
                            #region 加载基本信息
                            radioGroup_tc.EditValue = StaticClass.arrangeconfig.TypeConfig.IsDataFillType;
                            radioGroup_px.EditValue = StaticClass.arrangeconfig.TypeConfig.IsUpIndex;
                            n = StaticClass.arrangeconfig.TypeConfig.ShowColumnCount;
                            //if (n > 0 && n < 10)
                            //{
                            cmb_rowcount.Text = n.ToString();//加载时不限制  20170923
                            //}
                            //else
                            //{
                            //    cmb_rowcount.Text = "2";
                            //}
                            n = StaticClass.arrangeconfig.TypeConfig.ShowRowCount;
                            //if (n > 4 && n < 100)
                            //{
                            cmb_pagecount.Text = n.ToString();//可以配置超过100  20170923
                            //}
                            //else
                            //{
                            //    cmb_pagecount.Text = "5";
                            //}
                            try
                            {
                                cmb_sorttype.SelectedIndex = StaticClass.arrangeconfig.TypeConfig.PageSortType - 1;
                            }
                            catch
                            {
                                cmb_sorttype.SelectedIndex = 0;
                            }
                            #endregion

                            #region 加载显示列信息
                            table_defalut.Rows.Clear();
                            for (int i = 0; i < StaticClass.arrangeconfig.TypeConfig.Column.Length; i++)
                            {
                                m = StaticClass.arrangeconfig.TypeConfig.Column[i].Index;
                                table_defalut.Rows.Add(StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[m - 1].ColumnName, StaticClass.arrangeconfig.TypeConfig.Column[i].Isuse, m);
                            }
                            #endregion
                        }
                        else
                        {
                            SetDefalutMsg();
                        }
                        #endregion
                        break;
                    case 8:
                        #region 读取网络编排设置信息
                        if (StaticClass.arrangeconfig.NetConfig.Column[0] != null && StaticClass.arrangeconfig.NetConfig.Column[0].Index != 0)
                        {
                            #region 加载基本信息
                            radioGroup_tc.EditValue = StaticClass.arrangeconfig.NetConfig.IsDataFillType;
                            radioGroup_px.EditValue = StaticClass.arrangeconfig.NetConfig.IsUpIndex;
                            n = StaticClass.arrangeconfig.NetConfig.ShowColumnCount;
                            //if (n > 0 && n < 10)
                            //{
                            cmb_rowcount.Text = n.ToString();//加载时不限制  20170923
                            //}
                            //else
                            //{
                            //    cmb_rowcount.Text = "2";
                            //}
                            n = StaticClass.arrangeconfig.NetConfig.ShowRowCount;
                            //if (n > 4 && n < 100)
                            //{
                            cmb_pagecount.Text = n.ToString();//可以配置超过100  20170923
                            //}
                            //else
                            //{
                            //    cmb_pagecount.Text = "5";
                            //}
                            try
                            {
                                cmb_sorttype.SelectedIndex = StaticClass.arrangeconfig.NetConfig.PageSortType - 1;
                            }
                            catch
                            {
                                cmb_sorttype.SelectedIndex = 0;
                            }
                            #endregion

                            #region 加载显示列信息

                            table_defalut.Rows.Clear();
                            for (int i = 0; i < StaticClass.arrangeconfig.NetConfig.Column.Length; i++)
                            {
                                m = StaticClass.arrangeconfig.NetConfig.Column[i].Index;
                                table_defalut.Rows.Add(StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[m - 1].ColumnName, StaticClass.arrangeconfig.NetConfig.Column[i].Isuse, m);
                            }

                            #endregion
                        }
                        else
                        {
                            SetDefalutMsg();
                        }
                        #endregion
                        break;
                    case 9:
                        #region 读取区域编排设置信息
                        if (StaticClass.arrangeconfig.AreaConfig.Column[0] != null && StaticClass.arrangeconfig.AreaConfig.Column[0].Index != 0)
                        {
                            #region 加载基本信息
                            radioGroup_tc.EditValue = StaticClass.arrangeconfig.AreaConfig.IsDataFillType;
                            radioGroup_px.EditValue = StaticClass.arrangeconfig.AreaConfig.IsUpIndex;
                            n = StaticClass.arrangeconfig.AreaConfig.ShowColumnCount;
                            //if (n > 0 && n < 10)
                            //{
                            cmb_rowcount.Text = n.ToString();//加载时不限制  20170923
                            //}
                            //else
                            //{
                            //    cmb_rowcount.Text = "2";
                            //}
                            n = StaticClass.arrangeconfig.AreaConfig.ShowRowCount;
                            //if (n > 4 && n < 100)
                            //{
                            cmb_pagecount.Text = n.ToString();//可以配置超过100  20170923
                            //}
                            //else
                            //{
                            //    cmb_pagecount.Text = "5";
                            //}
                            try
                            {
                                cmb_sorttype.SelectedIndex = StaticClass.arrangeconfig.AreaConfig.PageSortType - 1;
                            }
                            catch
                            {
                                cmb_sorttype.SelectedIndex = 0;
                            }
                            #endregion

                            #region 加载显示列信息
                            table_defalut.Rows.Clear();
                            for (int i = 0; i < StaticClass.arrangeconfig.AreaConfig.Column.Length; i++)
                            {
                                m = StaticClass.arrangeconfig.AreaConfig.Column[0].Index;
                                table_defalut.Rows.Add(StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[m - 1].ColumnName, StaticClass.arrangeconfig.AreaConfig.Column[0].Isuse, m);
                            }
                            #endregion
                        }
                        else
                        {
                            SetDefalutMsg();
                        }
                        #endregion
                        break;
                    case 10:
                        #region 读取状态编排设置信息
                        if (StaticClass.arrangeconfig.StateConfig.Column[0] != null && StaticClass.arrangeconfig.StateConfig.Column[0].Index != 0)
                        {
                            #region 加载基本信息
                            radioGroup_tc.EditValue = StaticClass.arrangeconfig.StateConfig.IsDataFillType;
                            radioGroup_px.EditValue = StaticClass.arrangeconfig.StateConfig.IsUpIndex;
                            n = StaticClass.arrangeconfig.StateConfig.ShowColumnCount;
                            //if (n > 0 && n < 10)
                            //{
                            cmb_rowcount.Text = n.ToString();//加载时不限制  20170923
                            //}
                            //else
                            //{
                            //    cmb_rowcount.Text = "2";
                            //}
                            n = StaticClass.arrangeconfig.StateConfig.ShowRowCount;
                            //if (n > 4 && n < 100)
                            //{
                            cmb_pagecount.Text = n.ToString();//可以配置超过100  20170923
                            //}
                            //else
                            //{
                            //    cmb_pagecount.Text = "5";
                            //}
                            try
                            {
                                cmb_sorttype.SelectedIndex = StaticClass.arrangeconfig.StateConfig.PageSortType - 1;
                            }
                            catch
                            {
                                cmb_sorttype.SelectedIndex = 0;
                            }
                            #endregion

                            #region 加载显示列信息

                            table_defalut.Rows.Clear();
                            for (int i = 0; i < StaticClass.arrangeconfig.StateConfig.Column.Length; i++)
                            {
                                m = StaticClass.arrangeconfig.StateConfig.Column[i].Index;
                                table_defalut.Rows.Add(StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[m - 1].ColumnName, StaticClass.arrangeconfig.StateConfig.Column[i].Isuse, m);
                            }
                            #endregion
                        }
                        else
                        {
                            SetDefalutMsg();
                        }
                        #endregion
                        break;
                    case 12:
                        #region 读取自定义编排
                        if (treeList.FocusedNode != null)
                        {
                            str = treeList.FocusedNode.Tag.ToString();
                            n = int.Parse(str) - 30;
                        }
                        if (StaticClass.arrangeconfig.CustomCofig[n - 1] != null && StaticClass.arrangeconfig.CustomCofig[n - 1].Page > 0)
                        {
                            #region 加载显示列信息

                            table_defalut.Rows.Clear();
                            for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig[n - 1].Column.Length; i++)
                            {
                                m = StaticClass.arrangeconfig.CustomCofig[n - 1].Column[i].Index;
                                table_defalut.Rows.Add(StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[m - 1].ColumnName, StaticClass.arrangeconfig.CustomCofig[n - 1].Column[i].Isuse, m);
                            }

                            #endregion
                        }
                        else
                        {
                            SetDefalutMsg();
                        }
                        #endregion
                        break;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 修改树节点名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txb_name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                treeList.FocusedNode.SetValue(0, txt_zdy.Text);
                tp_defaltn.Text = "自定义编排-[" + txt_zdy.Text + "]";
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 测点选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_choice_Click(object sender, EventArgs e)
        {
            PageSetConfig p;
            bool flg = true;
            try
            {
                #region 判断是否有更改权限
                #endregion
                if (flg)
                {
                    p = new PageSetConfig();
                    if (StaticClass.arrangeconfig.CustomCofig[CustomPage - 1] != null)
                    {
                        p.ShowRowCount = StaticClass.arrangeconfig.CustomCofig[CustomPage - 1].ShowRowCount;
                        p.ShowColumnCount = StaticClass.arrangeconfig.CustomCofig[CustomPage - 1].ShowColumnCount;
                    }
                    else
                    {
                        p.ShowColumnCount = 2;
                        p.ShowRowCount = 30;
                    }
                    p.PageName = txt_zdy.Text.Trim();
                    p.IsColumnsMsg = GetColumnMsg();
                    p.Column = GetColumnSetMsg();
                    p.Page = CustomPage;
                    SetPointsShowForm ps = new SetPointsShowForm(p);
                    ps.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (LoadTrue)
            {
                timer1.Enabled = false;
                treeList.ExpandAll();
                pageshow(true);

                #region 初始化gv_columns
                try
                {
                    gdltbinit();
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }

                #endregion

                #region 初始化gv_defalut
                for (int i = 5; i < 100; i++)
                {
                    cmb_pagecount.Properties.Items.Add(i.ToString());
                }
                cmb_pagecount.Text = "50";
                cmb_rowcount.Text = "2";
                cmb_sorttype.SelectedIndex = 0;

                #region 固定行、列
                SetDefalutMsg();
                #endregion

                #endregion

                #region 初始化base
                cmb_DataRowHigh.Text = "20";
                cmb_Interval.Text = "5";
                cmb_TableHadehigh.Text = "20";
                cmb_SplitLineWidth.Text = "2";
                #endregion

                #region 初始化字体
                try
                {
                    InstalledFontCollection MyFont = new InstalledFontCollection();
                    FontFamily[] MyFontFamilies = MyFont.Families;
                    for (int i = 0; i < MyFontFamilies.Length; i++)
                    {
                        cmb_tablefontname.Properties.Items.Add(MyFontFamilies[i].Name);
                        cmb_datafontname.Properties.Items.Add(MyFontFamilies[i].Name);
                    }
                    if (cmb_tablefontname.Properties.Items.Count > 0)
                    {
                        cmb_tablefontname.SelectedIndex = 0;
                    }
                    if (cmb_datafontname.Properties.Items.Count > 0)
                    {
                        cmb_datafontname.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                #endregion

                #region 读取基础配置到界面
                ReadBaseConfig();
                #endregion

                #region 读取字体配置到界面
                ReadFontConfig();
                #endregion

                #region 读取列设置到界面
                ReadColumnConfig();
                #endregion

                #region 读取状态配置到界面
                ReadAlarmConfig();
                #endregion

                #region 给自定义树节点赋名称
                try
                {
                    for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
                    {
                        if (StaticClass.arrangeconfig.CustomCofig[i] != null)
                        {
                            treeList.Nodes[2].Nodes[i].SetValue(0, StaticClass.arrangeconfig.CustomCofig[i].PageName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                #endregion
            }
        }

        /// <summary>
        /// 从数据库中获取配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_readsyetemconfig_Click(object sender, EventArgs e)
        {
            string text = btn_readsyetemconfig.Text;
            btn_readsyetemconfig.Text = "正在加载....";
            btn_readsyetemconfig.Enabled = false;
            try
            {
                if (OprFuction.ReadRealConfigFromDB())
                {
                    #region 从本地读取配置信息到内存
                    OprFuction.ReadRealDataDisplayConfig();
                    OprFuction.ReadDefalutDataConfig();
                    OprFuction.ReadCustomConfig();
                    #endregion

                    #region 读取基础配置到界面
                    ReadBaseConfig();
                    #endregion

                    #region 读取字体配置到界面
                    ReadFontConfig();
                    #endregion

                    #region 读取列设置到界面
                    ReadColumnConfig();
                    #endregion

                    #region 读取状态配置到界面
                    ReadAlarmConfig();
                    #endregion

                    #region 给自定义树节点赋名称
                    try
                    {
                        for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
                        {
                            if (StaticClass.arrangeconfig.CustomCofig[i] != null)
                            {
                                treeList.Nodes[2].Nodes[i].SetValue(0, StaticClass.arrangeconfig.CustomCofig[i].PageName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(ex);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            btn_readsyetemconfig.Text = text;
            btn_readsyetemconfig.Enabled = true;
        }

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int n = 0;
            try
            {
                #region
                n = int.Parse(e.Node.Tag.ToString());
                if (n == 1)
                {
                    treeList.FocusedNode = treeList.Nodes[0].Nodes[0];
                }
                else if (n == 2)
                {
                    treeList.FocusedNode = treeList.Nodes[0].Nodes[0];
                }
                else if (n == 3)
                {
                    treeList.FocusedNode = treeList.Nodes[2].Nodes[0];
                }
                else if (n > 30)
                {
                    CruPage = 31;
                    pageshow(true);
                }
                else
                {
                    if (n != CruPage)
                    {
                        CruPage = n;
                        pageshow(true);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void panelControl2_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 读取本地默认配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string text = simpleButton1.Text;
            simpleButton1.Text = "正在加载....";
            simpleButton1.Enabled = false;
            try
            {
                if (OprFuction.ReadRealConfigFromDB())
                {
                    #region 从本地读取配置信息到内存
                    OprFuction.ReadRealDataDisplayConfig(Application.StartupPath + "\\Config\\DisPlayDefaultCnfg\\RealDataCnfg.xml");
                    OprFuction.ReadDefalutDataConfig(Application.StartupPath + "\\Config\\DisPlayDefaultCnfg\\RealDataDefalutCnfg.xml");
                    OprFuction.ReadCustomConfig(Application.StartupPath + "\\Config\\DisPlayDefaultCnfg\\RealDataCustomCnfg.xml");
                    #endregion

                    #region 读取基础配置到界面
                    ReadBaseConfig();
                    #endregion

                    #region 读取字体配置到界面
                    ReadFontConfig();
                    #endregion

                    #region 读取列设置到界面
                    ReadColumnConfig();
                    #endregion

                    #region 读取状态配置到界面
                    ReadAlarmConfig();
                    #endregion

                    #region 给自定义树节点赋名称
                    try
                    {
                        for (int i = 0; i < StaticClass.arrangeconfig.CustomCofig.Length; i++)
                        {
                            if (StaticClass.arrangeconfig.CustomCofig[i] != null)
                            {
                                treeList.Nodes[2].Nodes[i].SetValue(0, StaticClass.arrangeconfig.CustomCofig[i].PageName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Basic.Framework.Logging.LogHelper.Error(ex);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            simpleButton1.Text = text;
            simpleButton1.Enabled = true;
        }

        private void sButton1_Click(object sender, EventArgs e)
        {
            setbut("0");
        }

        private string getstyle()
        {
            string str = "0";
            if (sButton2.ForeColor == Color.Red)
            {
                str = "1";
            }
            else if (sButton3.ForeColor == Color.Red)
            {
                str = "2";
            }
            else if (sButton4.ForeColor == Color.Red)
            {
                str = "3";
            }
            return str;
        }

        private void setbut(string n)
        {
            if (n == "0")
            {
                sButton1.ForeColor = Color.Red;
                sButton1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
                sButton2.ForeColor = Color.White;
                sButton2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton3.ForeColor = Color.White;
                sButton3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton4.ForeColor = Color.White;
                sButton4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            }
            else if (n == "1")
            {
                sButton2.ForeColor = Color.Red;
                sButton2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
                sButton1.ForeColor = Color.White;
                sButton1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton3.ForeColor = Color.White;
                sButton3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton4.ForeColor = Color.White;
                sButton4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            }
            else if (n == "2")
            {
                sButton3.ForeColor = Color.Red;
                sButton3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
                sButton2.ForeColor = Color.White;
                sButton2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton1.ForeColor = Color.White;
                sButton1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton4.ForeColor = Color.White;
                sButton4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            }
            else if (n == "3")
            {
                sButton4.ForeColor = Color.Red;
                sButton4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
                sButton2.ForeColor = Color.White;
                sButton2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton3.ForeColor = Color.White;
                sButton3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
                sButton1.ForeColor = Color.White;
                sButton1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            }
        }

        private void sButton2_Click(object sender, EventArgs e)
        {
            setbut("1");
        }

        private void sButton3_Click(object sender, EventArgs e)
        {
            setbut("2");
        }

        private void sButton4_Click(object sender, EventArgs e)
        {
            setbut("3");
        }




    }
}
