using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Alarm;
using System.Threading;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.CBFCommon;
using DevExpress.XtraGrid.Views.Grid;
using Sys.Safety.Client.Chart;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using Sys.Safety.DataContract.UserRoleAuthorize;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Sys.Safety.ClientFramework.View.LogOn;
using Sys.Safety.Client.Define;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract;


namespace Sys.Safety.Client.Display
{
    public partial class RealDisplayForm : XtraUserControl
    {
        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        public RealDisplayForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 自定义编排测点表
        /// </summary>
        private DataTable CustomDt = new DataTable();

        /// <summary>
        /// 显示表
        /// </summary>
        public DataTable ShowDt = new DataTable();

        /// <summary>
        /// 展示类型 1-设备 2-类别 3-区域 4-状态 5-自定义
        /// </summary>
        private int ShowType = 0;

        /// <summary>
        /// 自定义显示页面 或 分站号
        /// </summary>
        private int CustomPage = 0;

        /// <summary>
        /// 排序方式  1-测点 2-安装位置 3-类型
        /// </summary>
        private int SortType = 0;

        /// <summary>
        /// 排序顺序 true-升序 false-降序
        /// </summary>
        private bool SortDirection = false;

        /// <summary>
        /// 列名数组
        /// </summary>
        private string[] ColumnNames = null;

        /// <summary>
        /// 显示组数
        /// </summary>
        private int ColumGroupCount = 0;

        /// <summary>
        /// 填充方向 false-横向 true-竖向
        /// </summary>
        private bool FillDirection = false;

        /// <summary>
        /// 开始行索引
        /// </summary>
        private int StartRow = 0;

        /// <summary>
        /// 结束行索引
        /// </summary>
        private int CrutRow = 0;

        /// <summary>
        /// 总行数
        /// </summary>
        private int ShowDtRows = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        private int PageCount = 1;

        /// <summary>
        /// 当前页
        /// </summary>
        private int CrutPage = 1;

        /// <summary>
        /// 是否显示point列
        /// </summary>
        private bool IsShowPoint = false;

        /// <summary>
        /// 选择的信息 如 区域
        /// </summary>
        private List<string> areamsg;

        private int ztpage = 0;

        /// <summary>
        /// 显示showdt操作对象锁  20170705
        /// </summary>
        public readonly object objShowDt = new object();

        ///// <summary>
        ///// 测点值和测点名对象
        ///// </summary>
        //RealMeasurePoint _selected;//20140906添加

        /// <summary>
        /// 根据条件生成布局
        /// </summary>
        /// <param name="type">布局方式</param>
        /// <param name="n">自定义布局方式 当type=5 时 有效</param>
        /// <param name="msg" >选择的信息</parm>
        public void Gv_init(int type, int n, List<string> lbmsg)
        {
            int m = 0;//自增序号 用于建列名
            int index = 0;//索引
            int SplitWidth = 0;//分隔线宽
            int Rows = 0;//显示行数
            string str = "";
            DevExpress.XtraGrid.Columns.GridColumn column;
            try
            {

                #region 基础信息赋值
                if (type > 0 && type < 6)
                {
                    ShowType = type;
                    CustomPage = n;
                    areamsg = lbmsg;
                }
                else
                {
                    type = ShowType;
                    n = CustomPage;
                }
                SplitWidth = StaticClass.realdataconfig.BaseCfg.SplitWidth;
                #endregion

                OprFuction.setfont(StaticClass.realdataconfig.BaseCfg.Bjfontsize);

                #region 布局
                switch (ShowType)
                {
                    case 1:
                        #region 设备编排初始化布局
                        ColumnNames = StaticClass.arrangeconfig.TypeConfig.IsColumnsMsg.Split('|');
                        ColumGroupCount = StaticClass.arrangeconfig.TypeConfig.ShowColumnCount;
                        Rows = StaticClass.arrangeconfig.TypeConfig.ShowRowCount;
                        SortType = StaticClass.arrangeconfig.TypeConfig.PageSortType;
                        FillDirection = !StaticClass.arrangeconfig.TypeConfig.IsDataFillType;
                        SortDirection = StaticClass.arrangeconfig.TypeConfig.IsUpIndex;
                        #endregion
                        break;
                    case 2:
                        #region 类别编排初始化布局
                        ColumnNames = StaticClass.arrangeconfig.NetConfig.IsColumnsMsg.Split('|');
                        ColumGroupCount = StaticClass.arrangeconfig.NetConfig.ShowColumnCount;
                        Rows = StaticClass.arrangeconfig.NetConfig.ShowRowCount;
                        SortType = StaticClass.arrangeconfig.NetConfig.PageSortType;
                        FillDirection = !StaticClass.arrangeconfig.NetConfig.IsDataFillType;
                        SortDirection = StaticClass.arrangeconfig.NetConfig.IsUpIndex;
                        #endregion
                        break;
                    case 4:
                        #region 状态编排初始化布局
                        ColumnNames = StaticClass.arrangeconfig.StateConfig.IsColumnsMsg.Split('|');
                        ColumGroupCount = StaticClass.arrangeconfig.StateConfig.ShowColumnCount;
                        Rows = StaticClass.arrangeconfig.StateConfig.ShowRowCount;
                        SortType = StaticClass.arrangeconfig.StateConfig.PageSortType;
                        FillDirection = !StaticClass.arrangeconfig.StateConfig.IsDataFillType;
                        SortDirection = StaticClass.arrangeconfig.StateConfig.IsUpIndex;
                        #endregion
                        break;
                    case 5:
                        #region 自定义编排初始化布局
                        if (n > 0 && n < 16)
                        {
                            CustomDt = Model.RealInterfaceFuction.GetCustomPagePoint(n);
                            ColumnNames = StaticClass.arrangeconfig.CustomCofig[n - 1].IsColumnsMsg.Split('|');
                            ColumGroupCount = StaticClass.arrangeconfig.CustomCofig[n - 1].ShowColumnCount;
                            Rows = StaticClass.arrangeconfig.CustomCofig[n - 1].ShowRowCount;
                            SortType = StaticClass.arrangeconfig.CustomCofig[n - 1].PageSortType;
                            FillDirection = !StaticClass.arrangeconfig.CustomCofig[n - 1].IsDataFillType;
                            SortDirection = StaticClass.arrangeconfig.CustomCofig[n - 1].IsUpIndex;
                        }
                        #endregion
                        break;
                }
                #region 创建显示界面
                gview.Columns.Clear();
                chk_wg.Checked = StaticClass.realdataconfig.BaseCfg.Showgrid;
                if (ColumnNames.Length > 0)
                {
                    IsShowPoint = IsShowPointColumn(ColumnNames);
                    if (IsShowPoint)//有point列
                    {
                        #region 生成列===
                        for (int i = 0; i < ColumGroupCount; i++)
                        {
                            for (int j = 0; j < ColumnNames.Length; j++)
                            {
                                column = new DevExpress.XtraGrid.Columns.GridColumn();
                                if (ColumnNames[j] == "1")
                                {
                                    column.FieldName = "point" + (i + 1);
                                }
                                else
                                {
                                    column.FieldName = (i + 1).ToString() + "name" + j;
                                }
                                index = int.Parse(ColumnNames[j]) - 1;
                                column.OptionsFilter.AllowFilter = false;
                                column.OptionsFilter.AllowAutoFilter = false;
                                column.AppearanceHeader.BackColor = Color.Red;
                                column.Caption = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnName;
                                //column.MaxWidth =
                                column.Width = column.MinWidth = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnWidth;
                                column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                column.OptionsColumn.AllowSize = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].IsLocked ? false : true;
                                column.AppearanceHeader.Options.UseBackColor = true;
                                column.AppearanceHeader.Options.UseTextOptions = true;
                                //column.AppearanceHeader.Options.UseForeColor = true;
                                //column.AppearanceHeader.Options.UseFont = true;
                                column.AppearanceHeader.Options.UseBorderColor = true;

                                column.AppearanceCell.Options.UseBackColor = true;
                                column.AppearanceCell.Options.UseTextOptions = true;
                                //column.AppearanceCell.Options.UseForeColor = true;
                                //column.AppearanceCell.Options.UseFont = true;
                                column.AppearanceCell.Options.UseBorderColor = true;
                                if (!column.OptionsColumn.AllowSize)
                                {
                                    column.MaxWidth = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnWidth;
                                }
                                str = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnType;
                                if (str == "右对齐")
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                }
                                else if (str == "居中")
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                }
                                else
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                }
                                column.Tag = index;
                                column.Visible = true;
                                gview.Columns.Add(column);
                            }
                            if (i < ColumGroupCount - 1)
                            {
                                column = new DevExpress.XtraGrid.Columns.GridColumn();
                                column.FieldName = "s" + i;
                                column.Caption = " ";
                                column.MinWidth = 0;
                                column.Width = column.MaxWidth = SplitWidth;
                                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                column.OptionsColumn.AllowSize = false;
                                column.Tag = -1;
                                column.Visible = true;
                                gview.Columns.Add(column);
                                column.OptionsFilter.AllowFilter = false;
                                column.OptionsFilter.AllowAutoFilter = false;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 生成列===
                        for (int i = 0; i < ColumGroupCount; i++)
                        {
                            column = new DevExpress.XtraGrid.Columns.GridColumn();
                            column.FieldName = "point" + (i + 1);
                            column.Tag = 0;
                            column.Visible = false;
                            gview.Columns.Add(column);
                            for (int j = 0; j < ColumnNames.Length; j++)
                            {
                                column = new DevExpress.XtraGrid.Columns.GridColumn();
                                column.FieldName = (i + 1).ToString() + "name" + j;
                                index = int.Parse(ColumnNames[j]) - 1;
                                column.OptionsFilter.AllowFilter = false;
                                column.OptionsFilter.AllowAutoFilter = false;
                                column.AppearanceHeader.BackColor = Color.Red;
                                column.Caption = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnName;
                                //column.MaxWidth = 
                                column.Width = column.MinWidth = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnWidth;
                                column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                column.OptionsColumn.AllowSize = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].IsLocked ? false : true;
                                str = StaticClass.realdataconfig.DataClnCfg.ColumnsMsg[index].ColumnType;
                                if (str == "右对齐")
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                                }
                                else if (str == "居中")
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                }
                                else
                                {
                                    column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                    column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                }
                                column.Tag = index;
                                column.Visible = true;
                                gview.Columns.Add(column);
                            }
                            if (i < ColumGroupCount - 1)
                            {
                                column = new DevExpress.XtraGrid.Columns.GridColumn();
                                column.FieldName = "s" + i;
                                column.Caption = " ";
                                column.OptionsFilter.AllowFilter = false;
                                column.OptionsFilter.AllowAutoFilter = false;
                                column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                column.Width = column.MinWidth = SplitWidth;
                                column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                                column.OptionsColumn.AllowSize = false;
                                column.Tag = -1;
                                column.Visible = true;
                                gview.Columns.Add(column);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                #region 创建显示数据源
                ShowGvTbInit();
                #endregion

                #region 生成行
                ShowGvTb.Rows.Clear();
                for (int i = 0; i < Rows; i++)
                {
                    ShowGvTb.Rows.Add("");
                }
                gview.RowHeight = StaticClass.realdataconfig.BaseCfg.DataRowHigh;
                #endregion

                #region 列头设置
                try
                {
                    gview.ColumnPanelRowHeight = StaticClass.realdataconfig.BaseCfg.TableHadeHigh;
                    gview.Appearance.HeaderPanel.BackColor = StaticClass.realdataconfig.BaseCfg.TableHadeBackColor;
                    gview.Appearance.HeaderPanel.ForeColor = StaticClass.realdataconfig.FontCfg.TableHadeFontColor;
                    gview.Appearance.HeaderPanel.Font = GetFont(StaticClass.realdataconfig.FontCfg.TableHadeFontName,
                        StaticClass.realdataconfig.FontCfg.TableHadeFontSize,
                        StaticClass.realdataconfig.FontCfg.IsBold,
                        StaticClass.realdataconfig.FontCfg.IsItalic,
                        StaticClass.realdataconfig.FontCfg.IsHaveUnderLine);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion

                #region 数据行设置
                try
                {
                    gview.Appearance.Row.BackColor = StaticClass.realdataconfig.BaseCfg.GvBackColor;
                    gview.Appearance.Row.Font = GetFont(StaticClass.realdataconfig.FontCfg.DataFontName,
                        StaticClass.realdataconfig.FontCfg.DataFontSize,
                        StaticClass.realdataconfig.FontCfg.DataIsBold,
                        StaticClass.realdataconfig.FontCfg.DataIsItalic,
                        StaticClass.realdataconfig.FontCfg.DataIsHaveUnderLine);
                    //gview.OptionsView.EnableAppearanceEvenRow = StaticClass.realdataconfig.BaseCfg.Showju;
                    //gview.OptionsView.EnableAppearanceOddRow = StaticClass.realdataconfig.BaseCfg.Showju;
                    gview.Appearance.Row.BackColor = StaticClass.realdataconfig.BaseCfg.SingleRowColor;
                    gview.Appearance.Row.BackColor2 = StaticClass.realdataconfig.BaseCfg.TableHadeBackColor;
                    gview.Appearance.Row.ForeColor = StaticClass.realdataconfig.FontCfg.DataFontColor;
                    gview.Appearance.EvenRow.BackColor = StaticClass.realdataconfig.BaseCfg.SingleRowColor;
                    gview.Appearance.EvenRow.BackColor2 = StaticClass.realdataconfig.BaseCfg.TableHadeBackColor;
                    gview.Appearance.OddRow.BackColor = StaticClass.realdataconfig.BaseCfg.DoubleRowColor;
                    gview.Appearance.OddRow.BackColor2 = StaticClass.realdataconfig.BaseCfg.GvBackColor;
                    gview.Appearance.FocusedCell.BackColor = StaticClass.realdataconfig.BaseCfg.SelectColor;
                    gview.Appearance.VertLine.BackColor = gview.Appearance.HorzLine.BackColor = StaticClass.realdataconfig.BaseCfg.GridColor;

                    if (StaticClass.realdataconfig.BaseCfg.Colorchange == "0")
                    {
                        gview.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                        gview.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                        gview.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
                    }
                    else if (StaticClass.realdataconfig.BaseCfg.Colorchange == "1")
                    {
                        gview.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                        gview.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                        gview.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                    }
                    else if (StaticClass.realdataconfig.BaseCfg.Colorchange == "2")
                    {
                        gview.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                        gview.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                        gview.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                    }
                    else if (StaticClass.realdataconfig.BaseCfg.Colorchange == "3")
                    {
                        gview.Appearance.Row.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                        gview.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                        gview.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
                    }

                    ckc_ju.Checked = StaticClass.realdataconfig.BaseCfg.Showju;
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion

                #region 分隔线设置
                try
                {
                    for (int i = 0; i < gview.Columns.Count; i++)
                    {
                        if (gview.Columns[i].FieldName.Contains("s"))
                        {
                            gview.Columns[i].AppearanceCell.BackColor = StaticClass.realdataconfig.BaseCfg.SplitColor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }

                #endregion
                #endregion

                gridC.DataSource = ShowGvTb;

                #region 获取测点
                SelectPoints();
                #endregion


                if (ShowType != 5)
                {
                    #region 排序
                    SortPoint();
                    #endregion
                }
                Fyshow();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据条件生成布局
        /// </summary>
        /// <param name="type">布局方式</param>
        /// <param name="n">自定义布局方式 当type=5 时 有效</param>
        /// <param name="msg" >选择的信息</parm>
        public void Gv_initex(int n, List<string> lbmsg)
        {
            try
            {
                ztpage = CrutPage;
                SelectPoints();
                SortPoint();
                if (ztpage <= PageCount)
                {
                    CrutPage = ztpage;
                }
                else
                {
                    CrutPage = 1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 分页显示
        /// </summary>
        private void Fyshow()
        {
            #region 分页设置
            if (PageCount == 1)
            {
                //toolS.Enabled = false;
                CrutPage = 1;
            }
            else
            {
                //toolS..Enabled = true;
                tbtn_sy.Appearance.ForeColor = Color.Black;
                toolS.Appearance.ForeColor = Color.Black;
            }
            tlab_crupage.Caption = CrutPage + "/{" + PageCount + "}";
            #endregion

            #region 计算开始结束行
            SetStartEndRowIndex(CrutPage);
            #endregion

            #region 填充测点
            ViewClear();
            FillData();
            #endregion
        }

        /// <summary>
        /// 表初始化
        /// </summary>
        private void ShowTableInit()
        {
            lock (StaticClass.real_s.objShowDt)
            {
                ShowDt.Columns.Clear();
                #region 初始化表
                DataColumn clm = new DataColumn();
                clm.ColumnName = "point";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "fzh";
                clm.DataType = typeof(int);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "tdh";
                clm.DataType = typeof(int);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "dzh";
                clm.DataType = typeof(int);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "lb";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "wz";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "dw";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "ssz";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "zt";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sbzt";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "bj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "lx";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "zl";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "cjsj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sxbj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sxdd";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "xxbj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "xxdd";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sxyj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sxfd";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "xxyj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "xxfd";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "time";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "0t";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "1t";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "2t";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "0tcolor";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "1tcolor";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "2tcolor";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "dldj";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "sszcolor";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                clm = new DataColumn();
                clm.ColumnName = "statecolor";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                //新增加设备性质ID,用于排序  20170616
                clm = new DataColumn();
                clm.ColumnName = "lxtype";
                clm.DataType = typeof(int);
                ShowDt.Columns.Add(clm);
                //馈电状态  20170725
                clm = new DataColumn();
                clm.ColumnName = "NCtrlSate";
                clm.DataType = typeof(int);
                ShowDt.Columns.Add(clm);

                clm = new DataColumn();
                clm.ColumnName = "GradingAlarmLevel";
                clm.DataType = typeof(string);
                ShowDt.Columns.Add(clm);
                #endregion
            }
        }

        ///<summary>
        ///通过状态获取测点信息 如果CustomPage为0 表示获取所有测点
        ///</summary>
        private void GetPointFromZT(List<string> lbmsg)
        {
            DataRow[] rows = null;
            if (lbmsg == null || lbmsg.Count < 1)
            {
                return;
            }
            if (lbmsg[0] == "所有状态")
            {
                lock (StaticClass.allPointDtLockObj)
                {
                    rows = StaticClass.AllPointDt.Select();
                    AddToShowDt(rows);
                }
            }
            else
            {
                foreach (string zt in lbmsg)
                {
                    switch (zt)
                    {
                        case "正常":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select("zt='3' or zt='4' or zt='21' or (bj='0' and (zt='26' or zt='27'))");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "模拟量预警":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='8' or zt='14'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "模拟量报警":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='10' or zt='16'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "模拟量断电":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='12' or zt='18'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "开关量报警":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select("bj='1' and lx='开关量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "设备异常":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='20' or zt='22'or zt='4' or zt='23' or zt='25'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "休眠":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" sbzt='6'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "标校":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" sbzt='24'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "检修":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" sbzt='7'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "开关量断电":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select("bj='1' and lx='开关量' and (k1>0 or k2>0 or k3>0) ");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "通讯故障":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='0' or zt='1'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "开关量0态":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='25' and lx='开关量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "开关量1态":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='26' and lx='开关量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "开关量2态":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='27' and lx='开关量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "控制量0态":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='43' and lx='控制量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                        case "控制量1态":
                            #region
                            try
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    rows = StaticClass.AllPointDt.Select(" zt='44' and lx='控制量'");
                                    if (rows.Length > 0)
                                    {
                                        AddToShowDt(rows);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                            #endregion
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 通过类别获取测点信息 
        /// </summary>
        private void GetPointFromLB(List<string> lbmsg)
        {
            DataRow[] rows = null;
            try
            {
                if (lbmsg != null && lbmsg.Count > 0)
                {
                    if (lbmsg[0] == "所有种类")
                    {
                        lock (StaticClass.allPointDtLockObj)
                        {
                            rows = StaticClass.AllPointDt.Select();
                            AddToShowDt(rows);
                        }
                    }
                    else
                    {
                        foreach (string msg in lbmsg)
                        {
                            lock (StaticClass.allPointDtLockObj)
                            {
                                rows = StaticClass.AllPointDt.Select("lb='" + msg + "'");
                                AddToShowDt(rows);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 通过区域获取测点信息 
        /// </summary>
        private void GetPointFromFZ(List<string> lbmsg)
        {
            DataRow[] rows = null;
            try
            {
                if (lbmsg != null && lbmsg.Count > 0)
                {
                    if (lbmsg[0] == "所有设备")
                    {
                        lock (StaticClass.allPointDtLockObj)
                        {
                            // 20170322
                            if (StaticClass.AllPointDt != null)
                            {
                                if (StaticClass.AllPointDt.Rows.Count > 0)
                                {
                                    rows = StaticClass.AllPointDt.Select();
                                }
                            }
                            AddToShowDt(rows);
                        }
                    }
                    else
                    {
                        foreach (string msg in lbmsg)
                        {
                            lock (StaticClass.allPointDtLockObj)
                            {
                                // 20170322
                                if (StaticClass.AllPointDt != null)
                                {
                                    if (StaticClass.AllPointDt.Rows.Count > 0)
                                    {
                                        rows = StaticClass.AllPointDt.Select("fzh='" + msg + "'");
                                    }
                                }
                                AddToShowDt(rows);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取自定义编排测点
        /// </summary>
        /// <param name="lbmsg"></param>
        private void GetPointFromCustom(DataTable lbmsg)
        {
            DataRow[] rows = null;
            try
            {
                if (lbmsg != null && lbmsg.Rows.Count > 0)
                {
                    foreach (DataRow point in lbmsg.Rows)
                    {
                        lock (StaticClass.allPointDtLockObj)
                        {
                            rows = StaticClass.AllPointDt.Select("point ='" + point["point"].ToString() + "'");
                            if (rows.Length > 0)
                            {
                                AddToShowDt(rows);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 模糊查找测点
        /// </summary>
        private void GetPointMohu(string msg)
        {
            DataRow[] rows = null;
            try
            {
                //ShowDt.Clear();
                //rows = StaticClass.AllPointDt.Select("wz like '%"+msg+"%'");
                //if (rows.Length > 0)
                //{
                //    AddToShowDt(rows);
                //}
                rows = ShowDt.Select("wz like '%" + msg + "%'");
                lock (StaticClass.real_s.objShowDt)
                {
                    if (rows.Length > 0)
                    {
                        ShowDt = rows.CopyToDataTable();
                    }
                    else
                    {
                        ShowDt.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 根据需要查询测点 并将其添加到表中
        /// </summary>
        /// <param name="type" >条件</param>
        private void GetPointToShowdt(int type)
        {
            try
            {
                switch (type)
                {
                    case 1://分站
                        #region 分站
                        GetPointFromFZ(areamsg);
                        #endregion
                        break;
                    case 2://类别
                        #region 类别
                        GetPointFromLB(areamsg);
                        #endregion
                        break;
                    case 4://状态
                        GetPointFromZT(areamsg);
                        break;
                    case 5://自定义
                        GetPointFromCustom(CustomDt);
                        break;
                }
                if (StaticClass.fuzzyserch)
                {
                    GetPointMohu(StaticClass.fuzzyserchtext);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 将信息添加到表
        /// </summary>
        /// <param name="item"></param>
        //private void AddToShowDt(DataRow[] items)
        //{
        //    DataRow row = null;
        //    string state = "";
        //    string color = "";
        //    DataRow[] rows = null;
        //    string point = "";
        //    try
        //    {
        //        if (ShowType == 5)
        //        {
        //            #region 将信息加入showdt
        //            foreach (DataRow item in items)
        //            {
        //                try
        //                {
        //                    lock (StaticClass.real_s.objShowDt)
        //                    {
        //                        row = ShowDt.NewRow();
        //                        row["fzh"] = item["fzh"];
        //                        row["tdh"] = item["tdh"];
        //                        row["dzh"] = item["dzh"];
        //                        row["point"] = item["point"];
        //                        row["lb"] = item["lb"];
        //                        row["wz"] = item["wz"];
        //                        row["ssz"] = item["ssz"];
        //                        row["zt"] = item["zt"];
        //                        row["sbzt"] = item["sbzt"];
        //                        row["bj"] = item["bj"];
        //                        row["lx"] = item["lx"];
        //                        row["lxtype"] = item["lxtype"];//增加设备性质ID  20170616
        //                        row["zl"] = item["zl"];
        //                        row["dw"] = item["dw"];
        //                        row["sxbj"] = item["sxbj"];
        //                        row["sxdd"] = item["sxdd"];
        //                        row["xxbj"] = item["xxbj"];
        //                        row["xxdd"] = item["xxdd"];
        //                        row["sxyj"] = item["sxyj"];
        //                        row["sxfd"] = item["sxfd"];
        //                        row["xxyj"] = item["xxyj"];
        //                        row["xxfd"] = item["xxfd"];
        //                        row["0t"] = item["0t"];
        //                        row["1t"] = item["1t"];
        //                        row["2t"] = item["2t"];
        //                        row["0tcolor"] = item["0tcolor"];
        //                        row["1tcolor"] = item["1tcolor"];
        //                        row["2tcolor"] = item["2tcolor"];
        //                        row["dldj"] = item["dldj"];
        //                        state = item["zt"].ToString();
        //                        if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
        //                        {
        //                            row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["0tcolor"].ToString(), item["bj"].ToString());
        //                        }
        //                        else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
        //                        {
        //                            row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["1tcolor"].ToString(), item["bj"].ToString());
        //                        }
        //                        else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
        //                        {
        //                            row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["2tcolor"].ToString(), item["bj"].ToString());
        //                        }
        //                        else
        //                        {
        //                            row["statecolor"] = row["sszcolor"] = GetShowColor(state, "0", item["bj"].ToString());
        //                        }
        //                        ShowDt.Rows.Add(row);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Basic.Framework.Logging.LogHelper.Error(ex);
        //                }
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region 将信息加入showdt
        //            if (items != null)//增加判断  20170401
        //            {
        //                foreach (DataRow item in items)
        //                {
        //                    try
        //                    {
        //                        //优化代码，每次到ShowDt中去查找，存在性能问题  20170720
        //                        //point = item["point"].ToString();
        //                        //if (!string.IsNullOrEmpty(point))
        //                        //{
        //                        //    rows = ShowDt.Select("point='" + point + "'");
        //                        //}
        //                        //if (rows == null)
        //                        //{
        //                        //    continue;
        //                        //}
        //                        //if (rows.Length < 1)
        //                        //{
        //                            lock (StaticClass.real_s.objShowDt)
        //                            {
        //                                row = ShowDt.NewRow();
        //                                row["fzh"] = item["fzh"];
        //                                row["tdh"] = item["tdh"];
        //                                row["dzh"] = item["dzh"];
        //                                row["point"] = item["point"];
        //                                row["lb"] = item["lb"];
        //                                row["wz"] = item["wz"];
        //                                row["ssz"] = item["ssz"];
        //                                row["zt"] = item["zt"];
        //                                row["sbzt"] = item["sbzt"];
        //                                row["bj"] = item["bj"];
        //                                row["lx"] = item["lx"];
        //                                row["lxtype"] = item["lxtype"];//增加设备性质ID  20170616
        //                                row["zl"] = item["zl"];
        //                                row["dw"] = item["dw"];
        //                                row["sxbj"] = item["sxbj"];
        //                                row["sxdd"] = item["sxdd"];
        //                                row["xxbj"] = item["xxbj"];
        //                                row["xxdd"] = item["xxdd"];
        //                                row["sxyj"] = item["sxyj"];
        //                                row["sxfd"] = item["sxfd"];
        //                                row["xxyj"] = item["xxyj"];
        //                                row["xxfd"] = item["xxfd"];
        //                                row["0t"] = item["0t"];
        //                                row["1t"] = item["1t"];
        //                                row["2t"] = item["2t"];
        //                                row["0tcolor"] = item["0tcolor"];
        //                                row["1tcolor"] = item["1tcolor"];
        //                                row["2tcolor"] = item["2tcolor"];
        //                                row["dldj"] = item["dldj"];
        //                                state = item["zt"].ToString();
        //                                if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
        //                                {
        //                                    row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["0tcolor"].ToString(), item["bj"].ToString());
        //                                }
        //                                else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
        //                                {
        //                                    row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["1tcolor"].ToString(), item["bj"].ToString());
        //                                }
        //                                else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
        //                                {
        //                                    row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["2tcolor"].ToString(), item["bj"].ToString());
        //                                }
        //                                else
        //                                {
        //                                    row["statecolor"] = row["sszcolor"] = GetShowColor(state, "0", item["bj"].ToString());
        //                                }
        //                                ShowDt.Rows.Add(row);
        //                            }
        //                        //}
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Basic.Framework.Logging.LogHelper.Error(ex);
        //                    }
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(ex.Message, ex);
        //    }
        //}
        /// <summary>
        /// 优化添加到显示的代码，精简并解决性能问题  20170720
        /// </summary>
        /// <param name="items"></param>
        private void AddToShowDt(DataRow[] items)
        {
            DataRow row = null;
            string state = "";
            string color = "";
            DataRow[] rows = null;
            string point = "";
            try
            {
                if (!StaticClass.ServerConet)
                {
                    if ((DateTime.Now - StaticClass.ServerConnetInrTime).TotalSeconds > 30)
                    {
                        if (items != null)//增加判断  20170401
                        {
                            foreach (DataRow item in items)
                            {
                                try
                                {
                                    lock (StaticClass.real_s.objShowDt)
                                    {
                                        row = ShowDt.NewRow();
                                        row["fzh"] = item["fzh"];
                                        row["tdh"] = item["tdh"];
                                        row["dzh"] = item["dzh"];
                                        row["point"] = item["point"];
                                        row["lb"] = item["lb"];
                                        row["wz"] = item["wz"];
                                        row["ssz"] = "";
                                        row["zt"] = "119";
                                        row["sbzt"] = "119";
                                        row["bj"] = "1";
                                        row["lx"] = item["lx"];
                                        row["lxtype"] = item["lxtype"];//增加设备性质ID  20170616
                                        row["zl"] = item["zl"];
                                        row["dw"] = item["dw"];
                                        row["sxbj"] = item["sxbj"];
                                        row["sxdd"] = item["sxdd"];
                                        row["xxbj"] = item["xxbj"];
                                        row["xxdd"] = item["xxdd"];
                                        row["sxyj"] = item["sxyj"];
                                        row["sxfd"] = item["sxfd"];
                                        row["xxyj"] = item["xxyj"];
                                        row["xxfd"] = item["xxfd"];
                                        row["0t"] = item["0t"];
                                        row["1t"] = item["1t"];
                                        row["2t"] = item["2t"];
                                        row["0tcolor"] = item["0tcolor"];
                                        row["1tcolor"] = item["1tcolor"];
                                        row["2tcolor"] = item["2tcolor"];
                                        row["dldj"] = item["dldj"];
                                        row["GradingAlarmLevel"] = item["GradingAlarmLevel"];

                                        row["statecolor"] = row["sszcolor"] = GetShowColor(StaticClass.itemStateToClient.EqpState2.ToString(), "0", "1");
                                        ShowDt.Rows.Add(row);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Basic.Framework.Logging.LogHelper.Error(ex);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (items != null)//增加判断  20170401
                    {
                        foreach (DataRow item in items)
                        {
                            try
                            {
                                lock (StaticClass.real_s.objShowDt)
                                {
                                    row = ShowDt.NewRow();
                                    row["fzh"] = item["fzh"];
                                    row["tdh"] = item["tdh"];
                                    row["dzh"] = item["dzh"];
                                    row["point"] = item["point"];
                                    row["lb"] = item["lb"];
                                    row["wz"] = item["wz"];
                                    row["ssz"] = item["ssz"];
                                    row["zt"] = item["zt"];
                                    row["sbzt"] = item["sbzt"];
                                    row["bj"] = item["bj"];
                                    row["lx"] = item["lx"];
                                    row["lxtype"] = item["lxtype"];//增加设备性质ID  20170616
                                    row["zl"] = item["zl"];
                                    row["dw"] = item["dw"];
                                    row["sxbj"] = item["sxbj"];
                                    row["sxdd"] = item["sxdd"];
                                    row["xxbj"] = item["xxbj"];
                                    row["xxdd"] = item["xxdd"];
                                    row["sxyj"] = item["sxyj"];
                                    row["sxfd"] = item["sxfd"];
                                    row["xxyj"] = item["xxyj"];
                                    row["xxfd"] = item["xxfd"];
                                    row["0t"] = item["0t"];
                                    row["1t"] = item["1t"];
                                    row["2t"] = item["2t"];
                                    row["0tcolor"] = item["0tcolor"];
                                    row["1tcolor"] = item["1tcolor"];
                                    row["2tcolor"] = item["2tcolor"];
                                    row["dldj"] = item["dldj"];
                                    row["GradingAlarmLevel"] = item["GradingAlarmLevel"];
                                    //todo:20190119
                                    if (row["ssz"].ToString() == "断线")
                                    {
                                        row["GradingAlarmLevel"] = "-";
                                    }
                                    state = item["zt"].ToString();
                                    if (state == StaticClass.itemStateToClient.EqpState43.ToString() || state == StaticClass.itemStateToClient.EqpState24.ToString())
                                    {
                                        row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["0tcolor"].ToString(), item["bj"].ToString());
                                    }
                                    else if (state == StaticClass.itemStateToClient.EqpState44.ToString() || state == StaticClass.itemStateToClient.EqpState25.ToString())
                                    {
                                        row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["1tcolor"].ToString(), item["bj"].ToString());
                                    }
                                    else if (state == StaticClass.itemStateToClient.EqpState26.ToString())
                                    {
                                        row["statecolor"] = row["sszcolor"] = GetShowColor(state, item["2tcolor"].ToString(), item["bj"].ToString());
                                    }
                                    else
                                    {
                                        row["statecolor"] = row["sszcolor"] = GetShowColor(state, "0", item["bj"].ToString());
                                    }
                                    ShowDt.Rows.Add(row);
                                }
                            }
                            catch (Exception ex)
                            {
                                Basic.Framework.Logging.LogHelper.Error(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
        }
        private string GetShowColor(string state, string color, string alarmflag)
        {
            bool alarm = alarmflag == "1" ? true : false;
            Color forcolor;
            if (state == "28" || state == "29" || state == "30")
            {
                forcolor = OprFuction.StateToColor(state, color, alarm);
            }
            else
            {
                forcolor = OprFuction.StateToColor(state, "0", alarm);
            }

            return forcolor.ToArgb().ToString();
        }
        /// <summary>
        /// 测点选择
        /// </summary>
        private void SelectPoints()
        {
            lock (StaticClass.real_s.objShowDt)
            {
                ShowDt.Rows.Clear();
            }
            #region 筛选测点及赋初值
            try
            {
                GetPointToShowdt(ShowType);
                ShowDtRows = ShowDt.Rows.Count;

                if (ShowDtRows > 0)
                {
                    if (ShowDtRows % (ColumGroupCount * ShowGvTb.Rows.Count) > 0)
                    {
                        PageCount = ShowDtRows / (ColumGroupCount * ShowGvTb.Rows.Count) + 1;
                    }
                    else
                    {
                        PageCount = ShowDtRows / (ColumGroupCount * ShowGvTb.Rows.Count);
                    }
                }
                else
                {
                    PageCount = 1;
                }
                if (CrutPage > PageCount)
                {
                    CrutPage = 1;
                }
                barStaticItem2.Caption = "共" + ShowDtRows + "条记录";
            }
            catch (Exception ex)
            {
                LogHelper.Error("筛选测点及赋初值", ex);
            }
            #endregion
        }

        /// <summary>
        /// 计算开始行结束行
        /// </summary>
        /// <param name="n" >页数</param>
        private void SetStartEndRowIndex(int n)
        {
            int allrow = ColumGroupCount * ShowGvTb.Rows.Count;
            if (ShowDtRows > 0)
            {
                if (ShowType == 5)//自定义编排
                {
                    StartRow = 0;
                    CrutRow = ShowDtRows;
                }
                else
                {
                    if (n < 1)
                    {
                        n = 1;
                    }
                    if (n > PageCount)
                    {
                        n = PageCount;
                    }
                    StartRow = (n - 1) * allrow;
                    CrutRow = n * allrow;
                    if (CrutRow > ShowDtRows)
                    {
                        CrutRow = ShowDtRows;
                    }
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private void SortPoint()
        {
            lock (StaticClass.real_s.objShowDt)
            {
                #region 根据条件排序
                if (ShowType != 5)
                {
                    if (SortType == 3)
                    {
                        if (SortDirection)
                        {
                            ShowDt.DefaultView.Sort = "wz ASC";
                        }
                        else
                        {
                            ShowDt.DefaultView.Sort = "wz DESC";
                        }
                        ShowDt = ShowDt.DefaultView.ToTable();
                    }
                    else if (SortType == 2)
                    {
                        if (SortDirection)
                        {
                            ShowDt.DefaultView.Sort = "lxtype,point ASC";
                        }
                        else
                        {
                            ShowDt.DefaultView.Sort = "lxtype,point DESC";
                        }
                        ShowDt = ShowDt.DefaultView.ToTable();
                    }
                    else
                    {
                        if (SortDirection)
                        {
                            //ShowDt.DefaultView.Sort = "fzh,lxtype,point ASC";
                            //升序重新排列 按基础通道，智能通道,控制通道，累计通道顺序进行排序  20170712
                            ShowDt = SortShowDt();
                        }
                        else
                        {
                            ShowDt.DefaultView.Sort = "fzh,lxtype,point DESC";
                            ShowDt = ShowDt.DefaultView.ToTable();
                        }
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 重新排列 按基础通道，智能通道,控制通道，累计通道顺序进行排序  20170712
        /// </summary>
        /// <returns></returns>
        private DataTable SortShowDt()
        {
            DataTable SortShowDt = ShowDt.Clone();
            DataRow[] SortFz = null;
            lock (StaticClass.allPointDtLockObj)
            {
                SortFz = StaticClass.AllPointDt.Select("lxtype='0'", "fzh ASC");
            }
            if (SortFz == null)
            {
                SortShowDt = ShowDt;
                return SortShowDt;
            }
            for (int i = 0; i < SortFz.Length; i++)
            {
                //添加分站
                DataRow[] Station = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "' and lxtype='0'");
                if (Station.Length > 0)
                {
                    SortShowDt.Rows.Add(Station[0].ItemArray);
                }
                //加载基础通道
                DataRow[] BaseChanelInStation = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "'  and (lxtype='1' or lxtype='2' or (lxtype='3' and dzh>0))", "fzh,tdh,dzh ASC");
                foreach (DataRow temprow in BaseChanelInStation)
                {
                    SortShowDt.Rows.Add(temprow.ItemArray);
                }
                ////加载智能通道
                //DataRow[] SmartChanelInStation = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "' and tdh>=17 and tdh<=24 ", "fzh,tdh,dzh ASC");
                //foreach (DataRow temprow in SmartChanelInStation)
                //{
                //    SortShowDt.Rows.Add(temprow.ItemArray);
                //}
                //加载本地控制通道
                DataRow[] ControlChanelInStation = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "' and (lxtype='3' and dzh=0)", "fzh,tdh,dzh ASC");
                foreach (DataRow temprow in ControlChanelInStation)
                {
                    SortShowDt.Rows.Add(temprow.ItemArray);
                }
                //加载累计通道
                DataRow[] TiredChanelInStation = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "' and (lxtype='4')", "fzh,tdh,dzh ASC");
                foreach (DataRow temprow in TiredChanelInStation)
                {
                    SortShowDt.Rows.Add(temprow.ItemArray);
                }
                //加载人员通道  20171123
                DataRow[] PersonChanelInStation = ShowDt.Select("fzh='" + SortFz[i]["fzh"].ToString() + "' and (lxtype='7')", "fzh,tdh,dzh ASC");
                foreach (DataRow temprow in PersonChanelInStation)
                {
                    SortShowDt.Rows.Add(temprow.ItemArray);
                }
            }
            return SortShowDt;
        }
        /// <summary>
        /// 用于换页显示时 清空表中内容
        /// </summary>
        private void ViewClear()
        {
            for (int i = 0; i < ShowGvTb.Rows.Count; i++)
            {
                for (int j = 0; j < ShowGvTb.Columns.Count; j++)
                {
                    ShowGvTb.Rows[i][j] = "";
                }
            }
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private void FillData()
        {
            int clmindex = 0;
            string points = "";//测点号
            bool returnflg = false;
            int rows = StartRow;
            int r, c;
            DataRow[] drows;
            #region 根据条件填充
            if (ShowType != 5)
            {
                if (ColumGroupCount > 0)
                {
                    if (ShowDtRows > 0)
                    {
                        if (FillDirection)
                        {
                            #region 竖向填充 填充方式 先填充第一组中每行 再填充第二组中每行 以此类推
                            if (IsShowPoint)
                            {
                                #region 显示point列
                                try
                                {
                                    clmindex = 0;
                                    for (int i = 0; i < ColumGroupCount; i++)//按组循环
                                    {
                                        for (int j = 0; j < ShowGvTb.Rows.Count; j++)
                                        {
                                            if (i != 0)
                                            {
                                                clmindex = i * (ColumnNames.Length) + i;//取列开始下标
                                            }
                                            else
                                            {
                                                clmindex = 0;
                                            }
                                            points = GetPoint(rows);
                                            for (int m = 0; m < ColumnNames.Length; m++)
                                            {
                                                ShowGvTb.Rows[j][clmindex] = GetMsgFromShowTable(rows, ColumnNames[m]);
                                                clmindex += 1;
                                            }
                                            rows += 1;
                                            if (rows >= CrutRow)
                                            {
                                                returnflg = true;
                                                break;
                                            }
                                        }
                                        if (returnflg)
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("数据填充竖向", ex);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 不显示point列
                                try
                                {
                                    clmindex = 0;
                                    for (int i = 0; i < ColumGroupCount; i++)//按组循环
                                    {
                                        for (int j = 0; j < ShowGvTb.Rows.Count; j++)
                                        {
                                            if (i != 0)
                                            {
                                                clmindex = i * (ColumnNames.Length + 1) + i;//取列开始下标
                                            }
                                            else
                                            {
                                                clmindex = 0;
                                            }
                                            ShowGvTb.Rows[j][clmindex] = GetMsgFromShowTable(rows, "1");
                                            clmindex += 1;
                                            for (int m = 0; m < ColumnNames.Length; m++)
                                            {
                                                ShowGvTb.Rows[j][clmindex] = GetMsgFromShowTable(rows, ColumnNames[m]);
                                                clmindex += 1;
                                            }
                                            rows += 1;
                                            if (rows >= CrutRow)
                                            {
                                                returnflg = true;
                                                break;
                                            }
                                        }
                                        if (returnflg)
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("数据填充竖向", ex);
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            #region 横向填充 填充方式 先填充第一行中每组 在填充第二行中每组 以此类推
                            if (IsShowPoint)
                            {
                                #region 显示point
                                try
                                {
                                    for (int i = 0; i < ShowGvTb.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < ColumGroupCount; j++)
                                        {
                                            if (j != 0)
                                            {
                                                clmindex = j * (ColumnNames.Length) + j;//取列开始下标
                                            }
                                            else
                                            {
                                                clmindex = 0;
                                            }
                                            for (int m = 0; m < ColumnNames.Length; m++)
                                            {
                                                ShowGvTb.Rows[i][clmindex] = GetMsgFromShowTable(rows, ColumnNames[m]);
                                                clmindex += 1;
                                            }
                                            rows += 1;
                                            if (rows >= CrutRow)
                                            {
                                                returnflg = true;
                                                break;
                                            }
                                        }
                                        if (returnflg)
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex.Message, ex);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 不显示point
                                try
                                {
                                    for (int i = 0; i < ShowGvTb.Rows.Count; i++)
                                    {
                                        for (int j = 0; j < ColumGroupCount; j++)
                                        {
                                            if (j != 0)
                                            {
                                                clmindex = j * (ColumnNames.Length + 1) + j;//取列开始下标
                                            }
                                            else
                                            {
                                                clmindex = 0;
                                            }
                                            ShowGvTb.Rows[i][clmindex] = GetMsgFromShowTable(rows, "1");
                                            clmindex += 1;
                                            for (int m = 0; m < ColumnNames.Length; m++)
                                            {
                                                ShowGvTb.Rows[i][clmindex] = GetMsgFromShowTable(rows, ColumnNames[m]);
                                                clmindex += 1;
                                            }
                                            rows += 1;
                                            if (rows >= CrutRow)
                                            {
                                                returnflg = true;
                                                break;
                                            }
                                        }
                                        if (returnflg)
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error(ex.Message, ex);
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
            }
            else
            {
                #region 自定义页面数据加载
                try
                {
                    if (CustomDt != null && CustomDt.Rows.Count > 0)
                    {
                        if (IsShowPoint)
                        {
                            #region 显示point
                            for (int i = 0; i < CustomDt.Rows.Count; i++)
                            {
                                points = CustomDt.Rows[i]["point"].ToString();
                                r = int.Parse(CustomDt.Rows[i]["rowid"].ToString());
                                c = int.Parse(CustomDt.Rows[i]["px"].ToString());
                                if (c != 0)
                                {
                                    clmindex = c * (ColumnNames.Length) + c;//取列开始下标
                                }
                                else
                                {
                                    clmindex = 0;
                                }
                                drows = ShowDt.Select("point='" + points + "'");
                                if (drows.Length > 0)
                                {
                                    for (int m = 0; m < ColumnNames.Length; m++)
                                    {
                                        if (ShowGvTb.Rows.Count > r)
                                        {
                                            ShowGvTb.Rows[r][clmindex] = GetMsgFromShowTable(drows[0], ColumnNames[m]);
                                            clmindex += 1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 显示point
                            for (int i = 0; i < CustomDt.Rows.Count; i++)
                            {
                                points = CustomDt.Rows[i]["point"].ToString();
                                r = int.Parse(CustomDt.Rows[i]["rowid"].ToString());
                                c = int.Parse(CustomDt.Rows[i]["px"].ToString());
                                if (c != 0)
                                {
                                    /*clmindex = c * (ColumnNames.Length + 1);*/
                                    //取列开始下标

                                    clmindex = c * (ColumnNames.Length + 2) + 1;
                                }
                                else
                                {
                                    clmindex = 1;
                                }
                                drows = ShowDt.Select("point='" + points + "'");
                                if (drows.Length > 0)
                                {
                                    for (int m = 0; m < ColumnNames.Length; m++)
                                    {
                                        if (ShowGvTb.Rows.Count > r)
                                        {
                                            ShowGvTb.Rows[r][clmindex] = GetMsgFromShowTable(drows[0], ColumnNames[m]);
                                            clmindex += 1;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                }
                #endregion
            }
            #endregion
        }

        /// <summary>
        /// 获取测点号
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private string GetPoint(int rows)
        {
            string point = "";
            try
            {
                if (rows < ShowDt.Rows.Count)
                {
                    point = ShowDt.Rows[rows]["point"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            return point;
        }

        /// <summary>
        /// 获取填充信息
        /// </summary>
        /// <param name="rows">行</param>
        /// <param name="type">列信息</param>
        /// <returns></returns>
        private string GetMsgFromShowTable(int rows, string type)
        {
            string value = "";
            string temp = "";
            decimal mvale = 0;
            try
            {
                if (ShowDt.Rows.Count - 1 < rows)//增加判断  20170505
                {
                    return value;
                }
                switch (type)
                {
                    case "1"://标签名
                        value = ShowDt.Rows[rows]["point"].ToString();
                        break;
                    case "2"://位置列
                        value = ShowDt.Rows[rows]["wz"].ToString();
                        break;
                    case "3"://实时值
                        temp = ShowDt.Rows[rows]["zt"].ToString();
                        if (temp == "1")
                        {
                            value = "";
                        }
                        else
                        {
                            if (StaticClass.realdataconfig.DataClnCfg.ShowUnit)
                            {
                                value = ShowDt.Rows[rows]["ssz"].ToString();
                                if (!string.IsNullOrEmpty(value) && decimal.TryParse(value, out mvale))
                                {
                                    value += " " + ShowDt.Rows[rows]["dw"].ToString();
                                }
                            }
                            else
                            {
                                value = ShowDt.Rows[rows]["ssz"].ToString();
                            }
                        }
                        break;
                    case "4"://状态
                        //value = ShowDt.Rows[rows]["zt"].ToString();
                        //value = OprFuction.StateChange(value);

                        value = ShowDt.Rows[rows]["zt"].ToString();
                        int tempState = 0;
                        int.TryParse(value, out tempState);
                        value = OprFuction.StateChange(value);

                        #region 开关量、控制量状态处理，显示报警或者正常
                        if (value != "未知" && value != "与服务器连接中断")
                        {
                            if (ShowDt.Rows[rows]["lx"].ToString() == "开关量" && (ShowDt.Rows[rows]["zt"].ToString() == "25"
                                || ShowDt.Rows[rows]["zt"].ToString() == "26"
                                || ShowDt.Rows[rows]["zt"].ToString() == "27"))
                            {
                                value = "正常";
                                DataRow[] dr = ShowDt.Select("point='" + ShowDt.Rows[rows]["point"].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    //int k8 = 0;
                                    //int.TryParse(dr[0]["k8"].ToString(), out k8);
                                    //if ((k8 & 0x01) == 0x01 && tempState == 25)
                                    //{//0态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x02) == 0x02 && tempState == 26)
                                    //{//1态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x04) == 0x04 && tempState == 27)
                                    //{//2态报警
                                    //    value = "报警";
                                    //}
                                    int alarm = 0;
                                    int.TryParse(dr[0]["bj"].ToString(), out alarm);
                                    if (alarm > 0)
                                    {
                                        value = "报警";
                                    }
                                    else
                                    {
                                        value = "正常";
                                    }
                                }
                            }
                            if (ShowDt.Rows[rows]["lx"].ToString() == "控制量" && ShowDt.Rows[rows]["zt"].ToString().Contains("态"))
                            {
                                value = "正常";
                                DataRow[] dr = ShowDt.Select("point='" + ShowDt.Rows[rows]["point"].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    //int k8 = 0;
                                    //int.TryParse(dr[0]["k8"].ToString(), out k8);
                                    //if ((k8 & 0x01) == 0x01 && tempState == 43)
                                    //{//0态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x02) == 0x02 && tempState == 44)
                                    //{//1态报警
                                    //    value = "报警";
                                    //}
                                    int alarm = 0;
                                    int.TryParse(dr[0]["bj"].ToString(), out alarm);
                                    if (alarm > 0)
                                    {
                                        value = "报警";
                                    }
                                    else
                                    {
                                        value = "正常";
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case "5"://设备状态
                        value = ShowDt.Rows[rows]["sbzt"].ToString();
                        value = OprFuction.StateChange(value);
                        break;
                    case "6"://类型
                        value = ShowDt.Rows[rows]["lx"].ToString();
                        break;
                    case "7"://上限预警
                        value = ShowDt.Rows[rows]["sxyj"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["sxyj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "8"://上限报警
                        value = ShowDt.Rows[rows]["sxbj"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["sxbj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "9"://上限断电
                        value = ShowDt.Rows[rows]["sxdd"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["sxdd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "10"://上限复电
                        value = ShowDt.Rows[rows]["sxfd"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["sxfd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "11"://下限预警
                        value = ShowDt.Rows[rows]["xxyj"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["xxyj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "12"://下限报警
                        value = ShowDt.Rows[rows]["xxbj"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["xxbj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "13"://下限断电
                        value = ShowDt.Rows[rows]["xxdd"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["xxdd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "14"://下限复电
                        value = ShowDt.Rows[rows]["xxfd"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (ShowDt.Rows[rows]["xxfd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "15"://最近报警 种类
                        value = ShowDt.Rows[rows]["zl"].ToString();
                        break;
                    case "16"://最近断电 类别
                        value = ShowDt.Rows[rows]["lb"].ToString();
                        break;
                    case "17"://电量等级
                        value = ShowDt.Rows[rows]["dldj"].ToString();
                        if (ShowDt.Rows[rows]["lx"].ToString() != "分站")
                        {
                            if (value == "0" || value == "")
                            {
                                value = "无";
                            }
                            else
                            {
                                value += "V";
                            }
                        }
                        break;
                    case "18"://分级报警等级
                        if (ShowDt.Rows[rows]["lx"].ToString() == "模拟量")
                        {
                            if (ShowDt.Rows[rows]["zt"].ToString() == "20" || ShowDt.Rows[rows]["zt"].ToString() == "46")
                            {
                                value = "-";
                            }
                            else
                            {
                                if (ShowDt.Rows[rows]["GradingAlarmLevel"].ToString() == "0")
                                {
                                    value = "-";
                                }
                                else
                                {
                                    value = ShowDt.Rows[rows]["GradingAlarmLevel"].ToString() + "级";
                                }
                            }
                        }
                        else {
                            value = "-";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            return value;
        }

        /// <summary>
        /// 获取填充信息
        /// </summary>
        /// <param name="rows">行</param>
        /// <param name="type">列信息</param>
        /// <returns></returns>
        private string GetMsgFromShowTable(DataRow rows, string type)
        {
            string value = "";
            string temp = "";
            decimal mvale = 0;
            try
            {
                switch (type)
                {
                    case "1"://标签名
                        value = rows["point"].ToString();
                        break;
                    case "2"://位置列
                        value = rows["wz"].ToString();
                        break;
                    case "3"://实时值
                        temp = rows["zt"].ToString();
                        if (temp == "1")
                        {
                            value = "";
                        }
                        else
                        {
                            if (StaticClass.realdataconfig.DataClnCfg.ShowUnit)
                            {
                                value = rows["ssz"].ToString();
                                if (!string.IsNullOrEmpty(value) && decimal.TryParse(value, out mvale))
                                {
                                    value += " " + rows["dw"].ToString();
                                }
                            }
                            else
                            {
                                value = rows["ssz"].ToString();
                            }
                        }
                        break;
                    case "4"://状态
                        value = rows["zt"].ToString();
                        int tempState = 0;
                        int.TryParse(value, out tempState);
                        value = OprFuction.StateChange(value);

                        #region 开关量、控制量状态处理，显示报警或者正常
                        if (value != "未知" && value != "与服务器连接中断")
                        {
                            if (rows["lx"].ToString() == "开关量" && (rows["zt"].ToString() == "25"
                                || rows["zt"].ToString() == "26"
                                || rows["zt"].ToString() == "27"))
                            {
                                value = "正常";
                                DataRow[] dr = ShowDt.Select("point='" + rows["point"].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    //int k8 = 0;
                                    //int.TryParse(dr[0]["k8"].ToString(), out k8);
                                    //if ((k8 & 0x01) == 0x01 && tempState == 25)
                                    //{//0态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x02) == 0x02 && tempState == 26)
                                    //{//1态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x04) == 0x04 && tempState == 27)
                                    //{//2态报警
                                    //    value = "报警";
                                    //}
                                    int alarm = 0;
                                    int.TryParse(dr[0]["bj"].ToString(), out alarm);
                                    if (alarm > 0)
                                    {
                                        value = "报警";
                                    }
                                    else
                                    {
                                        value = "正常";
                                    }
                                }
                            }
                            if (rows["lx"].ToString() == "控制量" && rows["zt"].ToString().Contains("态"))
                            {
                                value = "正常";
                                DataRow[] dr = ShowDt.Select("point='" + rows["point"].ToString() + "'");
                                if (dr.Length > 0)
                                {
                                    //int k8 = 0;
                                    //int.TryParse(dr[0]["k8"].ToString(), out k8);
                                    //if ((k8 & 0x01) == 0x01 && tempState == 43)
                                    //{//0态报警
                                    //    value = "报警";
                                    //}
                                    //if ((k8 & 0x02) == 0x02 && tempState == 44)
                                    //{//1态报警
                                    //    value = "报警";
                                    //}
                                    int alarm = 0;
                                    int.TryParse(dr[0]["bj"].ToString(), out alarm);
                                    if (alarm > 0)
                                    {
                                        value = "报警";
                                    }
                                    else
                                    {
                                        value = "正常";
                                    }
                                }
                            }
                        }
                        #endregion
                        break;
                    case "5"://设备状态
                        value = rows["sbzt"].ToString();
                        value = OprFuction.StateChange(value);
                        break;
                    case "6"://类型
                        value = rows["lx"].ToString();
                        break;
                    case "7"://上限预警
                        value = rows["sxyj"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["sxyj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "8"://上限报警
                        value = rows["sxbj"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["sxbj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "9"://上限断电
                        value = rows["sxdd"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["sxdd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "10"://上限复电
                        value = rows["sxfd"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["sxfd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "11"://下限预警
                        value = rows["xxyj"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["xxyj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "12"://下限报警
                        value = rows["xxbj"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["xxbj"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "13"://下限断电
                        value = rows["xxdd"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["xxdd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "14"://下限复电
                        value = rows["xxfd"].ToString();
                        if (rows["lx"].ToString() == "分站")
                        {
                            value = "-";
                        }
                        else if (rows["xxfd"].ToString() == "0")
                        {
                            value = "-";
                        }
                        break;
                    case "15"://最近报警 种类
                        value = rows["zl"].ToString();
                        break;
                    case "16"://最近断电 类别
                        value = rows["lb"].ToString();
                        break;
                    case "17"://电量等级
                        value = rows["dldj"].ToString();
                        if (rows["lx"].ToString() != "分站")
                        {
                            if (value == "0" || value == "")
                            {
                                value = "无";
                            }
                            else
                            {
                                value += "V";
                            }
                        }
                        break;
                    case "18"://分级报警等级
                        if (rows["lx"].ToString() == "模拟量")
                        {
                            if (rows["zt"].ToString() == "20" || rows["zt"].ToString() == "46")
                            {
                                value = "-";
                            }
                            else
                            {
                                if (rows["GradingAlarmLevel"].ToString() == "0")
                                {
                                    value = "-";
                                }
                                else
                                {
                                    value = rows["GradingAlarmLevel"].ToString() + "级";
                                }
                            }
                        }
                        else {
                            value = "-";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
            }
            return value;
        }

        /// <summary>
        /// 将表中数据填充的显示界面
        /// </summary>
        private void RefreshRealData()
        {
            int clmindex = 0;
            string value = "", temp = "";
            bool returnflg = false;
            bool flg = false;
            Color forcolor = new Color();
            int rows = StartRow;
            #region 将表中数据填充的显示界面
            if (ColumGroupCount > 0)
            {
                if (ShowDtRows > 0)
                {
                    if (FillDirection)
                    {
                        #region 竖向刷新
                        clmindex = 0;
                        for (int i = 0; i < ColumGroupCount; i++)//按组循环
                        {
                            for (int j = 0; j < ShowGvTb.Rows.Count; j++)
                            {
                                if (i != 0)
                                {
                                    clmindex = i * (ColumnNames.Length) + 1;//取列开始下标
                                }
                                for (int m = 0; m < ColumnNames.Length; m++)
                                {
                                    switch (ColumnNames[m])
                                    {
                                        case "3"://实时值
                                            try
                                            {
                                                flg = true;
                                                value = ShowDt.Rows[rows]["ssz"].ToString();
                                                temp = ShowDt.Rows[rows]["zt"].ToString();
                                                #region 实时值 及颜色
                                                if (temp == "0") //正常
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                                                }
                                                else if (temp == "2")//上限报警
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.UpAlarmColor;
                                                }
                                                else if (temp == "1")//上限预警
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor;
                                                }
                                                else if (temp == "3")//上限断电
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor;
                                                }
                                                else if (temp == "4")//下限预警
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor;
                                                }
                                                else if (temp == "5")//下限报警
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.LowAlarmColor;
                                                }
                                                else if (temp == "6")//下限断电
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor;
                                                }
                                                else if (temp == "7")//开关量报警
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.KAlarmColor;
                                                }
                                                else if (temp == "8")//开关量断电
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.KBlackOutColor;
                                                }
                                                else if (temp == "10")//超量程
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.OverRangeColor;
                                                }
                                                if (value == "9")//通讯中断
                                                {
                                                    forcolor = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
                                                }
                                                #endregion
                                                ShowGvTb.Rows[j][clmindex] = value;
                                            }
                                            catch (Exception ex)
                                            {
                                                Basic.Framework.Logging.LogHelper.Error(ex);
                                            }
                                            break;
                                        case "4"://状态
                                            try
                                            {
                                                value = ShowDt.Rows[rows]["zt"].ToString();
                                                value = "正常";
                                                #region ////
                                                if (value == "0")
                                                {
                                                    value = "正常";
                                                }
                                                else if (value == "1" || value == "4")
                                                {
                                                    value = "预警";
                                                }
                                                else if (value == "2" || value == "5" || value == "7")
                                                {
                                                    value = "报警";
                                                }
                                                else if (value == "3" || value == "6" || value == "8")
                                                {
                                                    value = "断电";
                                                }
                                                else if (value == "9")
                                                {
                                                    value = "通讯中断";
                                                }
                                                else if (value == "10")
                                                {
                                                    value = "超量程";
                                                }
                                                #endregion
                                            }
                                            catch (Exception ex)
                                            {
                                                Basic.Framework.Logging.LogHelper.Error(ex);
                                            }
                                            break;
                                        case "6"://采集时间
                                            try
                                            {
                                                value = ShowDt.Rows[rows]["cjsj"].ToString();
                                            }
                                            catch (Exception ex)
                                            {
                                                Basic.Framework.Logging.LogHelper.Error(ex);
                                            }
                                            break;
                                        case "11"://最近报警
                                            value = ShowDt.Rows[rows]["zjbj"].ToString();
                                            break;
                                        case "12"://最近断电
                                            value = ShowDt.Rows[rows]["zjdd"].ToString();
                                            break;
                                        case "13"://报警最大值
                                            value = ShowDt.Rows[rows]["bjzdz"].ToString();
                                            break;
                                        case "14"://报警最小值
                                            value = ShowDt.Rows[rows]["bjzxz"].ToString();
                                            break;
                                        case "15"://预警开始时间
                                            value = ShowDt.Rows[rows]["yjkssj"].ToString();
                                            break;
                                        case "16"://变化率
                                            value = ShowDt.Rows[rows]["bhl"].ToString();
                                            break;
                                    }
                                    clmindex += 1;
                                }
                                rows += 1;
                                if (rows >= CrutRow)
                                {
                                    returnflg = true;
                                    break;
                                }
                            }
                            if (returnflg)
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region 横向刷新
                        for (int i = 0; i < ShowGvTb.Rows.Count; i++)
                        {
                            for (int j = 0; j < ColumGroupCount; j++)
                            {
                                if (i != 0)
                                {
                                    clmindex = i * (ColumnNames.Length) + 1;//取列开始下标
                                    if (!IsShowPoint)
                                    {
                                        clmindex = i * (ColumnNames.Length + 1) + 1;
                                    }
                                }
                                if (!IsShowPoint)
                                {
                                    clmindex++;
                                }
                                for (int m = 0; m < ColumnNames.Length; m++)
                                {
                                    switch (ColumnNames[m])
                                    {
                                        case "3"://实时值
                                            flg = true;
                                            value = ShowDt.Rows[rows]["ssz"].ToString();
                                            temp = ShowDt.Rows[rows]["zt"].ToString();
                                            #region 实时值 及颜色
                                            if (temp == "0") //正常
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                                            }
                                            else if (temp == "2")//上限报警
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.UpAlarmColor;
                                            }
                                            else if (temp == "1")//上限预警
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.UpPreAlarmColor;
                                            }
                                            else if (temp == "3")//上限断电
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.UpBlackOutColor;
                                            }
                                            else if (temp == "4")//下限预警
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.LowPreAlarmColor;
                                            }
                                            else if (temp == "5")//下限报警
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.LowAlarmColor;
                                            }
                                            else if (temp == "6")//下限断电
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.LowBlackOutColor;
                                            }
                                            else if (temp == "7")//开关量报警
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.KAlarmColor;
                                            }
                                            else if (temp == "8")//开关量断电
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.KBlackOutColor;
                                            }
                                            else if (temp == "10")//超量程
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.OverRangeColor;
                                            }
                                            if (value == "9")//通讯中断
                                            {
                                                forcolor = StaticClass.realdataconfig.StateCorCfg.InterruptionColor;
                                            }
                                            #endregion
                                            ShowGvTb.Rows[j][clmindex] = value;
                                            break;
                                        case "4"://状态
                                            value = ShowDt.Rows[rows]["zt"].ToString();
                                            value = "正常";
                                            #region ////
                                            if (value == "0")
                                            {
                                                value = "正常";
                                            }
                                            else if (value == "1" || value == "4")
                                            {
                                                value = "预警";
                                            }
                                            else if (value == "2" || value == "5" || value == "7")
                                            {
                                                value = "报警";
                                            }
                                            else if (value == "3" || value == "6" || value == "8")
                                            {
                                                value = "断电";
                                            }
                                            else if (value == "9")
                                            {
                                                value = "通讯中断";
                                            }
                                            else if (value == "10")
                                            {
                                                value = "超量程";
                                            }
                                            #endregion
                                            break;
                                        case "6"://采集时间
                                            value = ShowDt.Rows[rows]["cjsj"].ToString();
                                            break;
                                        case "11"://最近报警
                                            value = ShowDt.Rows[rows]["zjbj"].ToString();
                                            break;
                                        case "12"://最近断电
                                            value = ShowDt.Rows[rows]["zjdd"].ToString();
                                            break;
                                        case "13"://报警最大值
                                            value = ShowDt.Rows[rows]["bjzdz"].ToString();
                                            break;
                                        case "14"://报警最小值
                                            value = ShowDt.Rows[rows]["bjzxz"].ToString();
                                            break;
                                        case "15"://预警开始时间
                                            value = ShowDt.Rows[rows]["yjkssj"].ToString();
                                            break;
                                        case "16"://变化率
                                            value = ShowDt.Rows[rows]["bhl"].ToString();
                                            break;
                                    }
                                    clmindex += 1;
                                }
                                rows += 1;
                                if (rows >= CrutRow)
                                {
                                    returnflg = true;
                                    break;
                                }
                            }
                            if (returnflg)
                            {
                                break;
                            }
                        }
                        #endregion
                    }
                }
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
                LogHelper.Error(ex.Message, ex);
            }
            return newfont;
        }

        private void RealDisplayForm_Load(object sender, EventArgs e)
        {
            gv_init();

            ShowTableInit();
            StaticClass.RefreshTime = Model.RealInterfaceFuction.GetServerNowTime();
            List<string> msg = new List<string>();
            msg.Add("所有设备");
            //Gv_init(1, 0, msg);
            //timer1.Enabled = true;
            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.Start();

            //打开报警背景窗体
            StaticClass.Initalarm();
        }
        private Thread freshthread;
        private void fthread()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    if (ClientPermission.userRightString != Basic.Framework.Data.PlatRuntime.Items[KeyConst.RightKey].ToString())//判断，如果当前登录用户与最后一次登录用户权限发生变化则刷新右键  20180111
                    {
                        MethodInvoker In = new MethodInvoker(refreshRightMenu);
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                        //refreshRightMenu();
                    }

                    //System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                    //stopWatch.Restart();
                    OprFuction.definechange();
                    //stopWatch.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug("获取定义改变时间:" + stopWatch.ElapsedMilliseconds);
                    //stopWatch.Restart();
                    OprFuction.RealDataFresh();
                    //stopWatch.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug("实时显示获取时间:" + stopWatch.ElapsedMilliseconds);
                    //stopWatch.Restart();
                    if (ShowType == 4)
                    {
                        Gv_initex(CustomPage, areamsg);
                        MethodInvoker In = new MethodInvoker(Fyshow);
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                    }
                    else
                    {
                        MethodInvoker In = new MethodInvoker(FillData);
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(In);
                        }
                    }
                    //stopWatch.Stop();
                    //Basic.Framework.Logging.LogHelper.Debug("绑定显示到界面时间:" + stopWatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 根据权限刷新右键菜单  20180111
        /// </summary>
        private void refreshRightMenu()
        {
            //根据权限控制右键菜单中的功能显示/隐藏  20180111            
            bool blnHaveRight = ClientPermission.Authorize("QueryMnl_McLine");
            if (!blnHaveRight)
            {
                barButtonItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("QueryMnl_FiveMiniteLine");
            if (!blnHaveRight)
            {
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("QueryMnl_SSZChart");
            if (!blnHaveRight)
            {
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            //blnHaveRight = ClientPermission.Authorize("SBRunLogReport");
            //if (!blnHaveRight)
            //{
            //    barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //}
            //else
            //{
            //    barButtonItem1.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //}
            blnHaveRight = ClientPermission.Authorize("QueryMnl_LineWithScreen");
            if (!blnHaveRight)
            {
                barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("QueryKgl_StateLine");
            if (!blnHaveRight)
            {
                barButtonItem7.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem7.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("KGLStateDayReport");
            if (!blnHaveRight)
            {
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem9.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("MCRunLogReport");
            if (!blnHaveRight)
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("MLLDayReport");
            if (!blnHaveRight)
            {
                barButtonItem14.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem14.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("QueryMnlAndKgl_LineWithScreen");
            if (!blnHaveRight)
            {
                barButtonItem15.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem15.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            blnHaveRight = ClientPermission.Authorize("QueryKgl_StateBar");
            if (!blnHaveRight)
            {
                barButtonItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                barButtonItem17.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            #region 网络断开重联判断
            //StaticClass.TAG_ReConnectTimeout += 5;
            //if (StaticClass.TAG_ReConnectTimeout >= StaticClass.ReConnectTimeout)
            //{
            //    OprFuction.LoadAllMsg();
            //    StaticClass.TAG_ReConnectTimeout = 0;
            //}
            #endregion
            //OprFuction.definechange();
            //OprFuction.RealDataFresh();
            if (ShowType == 4)
            {
                Gv_initex(CustomPage, areamsg);
            }
            else
            {
                FillData();
            }
            timer1.Enabled = true;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtn_sy_Click(object sender, EventArgs e)
        {
            if (CrutPage != 1)
            {
                tbtn_sy.Enabled = false;
                CrutPage = 1;
                Fyshow();
                tbtn_sy.Enabled = true;
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtn_my_Click(object sender, EventArgs e)
        {
            if (CrutPage != PageCount)
            {
                btn_my.Enabled = false;
                CrutPage = PageCount;
                Fyshow();
                btn_my.Enabled = true;
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtn_syy_Click(object sender, EventArgs e)
        {
            if (CrutPage > 1)
            {
                tbtn_syy.Enabled = false;
                CrutPage--;
                Fyshow();
                tbtn_syy.Enabled = true;
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtn_xyy_Click(object sender, EventArgs e)
        {
            if (CrutPage < PageCount)
            {
                tbtn_xyy.Enabled = false;
                CrutPage++;
                Fyshow();
                tbtn_xyy.Enabled = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //RealMeasurePoint point = null;
            //if (gv_realdatashow.SelectedCells != null)
            //{
            //    point = StaticClass.PointsList.Find(delegate(RealMeasurePoint obj) 
            //    { 
            //        return obj.PointMValue.ArrPoint == gv_realdatashow.SelectedCells[0].Tag.ToString(); 
            //    });
            //    if (point != null)
            //    {
            //        //MsgShow showfrom = new MsgShow(point);
            //        //showfrom.Show();
            //    }
            //}
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (PageCount > 1)
            {
                if (CrutPage < PageCount)
                {
                    CrutPage++;
                }
                else
                {
                    CrutPage = 1;
                }
                Fyshow();
            }
            timer2.Enabled = true;
        }

        /// <summary>
        /// 显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStrip_showset_Click(object sender, EventArgs e)
        {
            RealDatadisplaySetForm form = new RealDatadisplaySetForm();
            form.mydel = new RealDatadisplaySetForm.mydelegate(StaticClass.real_s.Gv_init);
            form.mydeltree = new RealDatadisplaySetForm.treedelegate(StaticClass._type_s.refresh);
            form.Show();
        }


        private void tbtn_sy_ForeColorChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 显示界面数据源
        /// </summary>
        private DataTable ShowGvTb = new DataTable();

        /// <summary>
        /// 数据源表初始化
        /// </summary>
        private void ShowGvTbInit()
        {
            #region 创建显示数据源
            DataColumn col;
            ShowGvTb.Columns.Clear();
            if (ColumnNames.Length > 0)
            {
                if (IsShowPoint)
                {
                    #region 生成列===
                    for (int i = 0; i < ColumGroupCount; i++)
                    {
                        for (int j = 0; j < ColumnNames.Length; j++)
                        {
                            col = new DataColumn();
                            col.DataType = typeof(string);
                            if (ColumnNames[j] == "1")
                            {
                                col.ColumnName = "point" + (i + 1);
                            }
                            else
                            {
                                col.ColumnName = (i + 1).ToString() + "name" + j;
                            }
                            ShowGvTb.Columns.Add(col);
                        }
                        if (i < ColumGroupCount - 1)
                        {
                            col = new DataColumn();
                            col.DataType = typeof(string);
                            col.ColumnName = "s" + i;
                            ShowGvTb.Columns.Add(col);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 生成列===
                    for (int i = 0; i < ColumGroupCount; i++)
                    {
                        col = new DataColumn();
                        col.DataType = typeof(string);
                        col.ColumnName = "point" + (i + 1);
                        ShowGvTb.Columns.Add(col);
                        for (int j = 0; j < ColumnNames.Length; j++)
                        {
                            col = new DataColumn();
                            col.DataType = typeof(string);
                            col.ColumnName = (i + 1).ToString() + "name" + j;
                            ShowGvTb.Columns.Add(col);
                        }
                        if (i < ColumGroupCount - 1)
                        {
                            col = new DataColumn();
                            col.DataType = typeof(string);
                            col.ColumnName = "s" + i;
                            ShowGvTb.Columns.Add(col);
                        }
                    }
                    #endregion
                }
            }
            #endregion
        }

        /// <summary>
        /// 判断要显示的是否有point列
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool IsShowPointColumn(string[] col)
        {
            bool flg = false;
            if (col != null && col.Length > 0)
            {
                for (int i = 0; i < col.Length; i++)
                {
                    if (col[i] == "1")
                    {
                        flg = true;
                        break;
                    }
                }
            }
            return flg;
        }

        private void gv_init()
        {
            gview.OptionsBehavior.Editable = false;//单元格编辑
            gview.OptionsCustomization.AllowColumnMoving = false;//拖动列头
            //gview.OptionsCustomization.AllowColumnResizing = false;//修改列宽
            gview.OptionsCustomization.AllowRowSizing = false;//修改行高
            gview.OptionsCustomization.AllowSort = false;//排序
            //gview.OptionsFilter.AllowFilterEditor = false;//使用过滤编辑器
            gview.OptionsMenu.EnableColumnMenu = false;//列头菜单
            gview.OptionsMenu.EnableFooterMenu = false;//页脚菜单
            gview.OptionsMenu.EnableGroupPanelMenu = true;//分组面板上的菜单
            gview.OptionsSelection.MultiSelect = false;//多选行
            gview.OptionsView.ShowHorzLines = true;//水平网格线
            gview.OptionsView.ShowVertLines = true;//垂直网格线
        }

        private void tbtn_sy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PageCount > 1)
            {
                if (CrutPage != 1)
                {
                    CrutPage = 1;
                    Fyshow();
                }
            }
        }

        private void chk_wg_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gview.OptionsView.ShowHorzLines = chk_wg.Checked;//水平网格线
            gview.OptionsView.ShowVertLines = chk_wg.Checked;//垂直网格线
            StaticClass.realdataconfig.BaseCfg.Showgrid = chk_wg.Checked;
            StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.Showgrid", StaticClass.realdataconfig.BaseCfg.Showgrid ? "1" : "0");
        }

        private Color GetRowCellColor(string point)
        {
            DataRow[] rows;
            Color cellColor = new Color();
            rows = ShowDt.Select("point='" + point + "'");
            if (rows.Length > 0)
            {
                if (rows[0]["zt"].ToString() == "46")
                {
                    cellColor = Color.Red;
                }
                else if (rows[0]["sbzt"].ToString() == "24")
                {
                    cellColor = OprFuction.StateToColor("24", "0", false);
                }
                else
                {
                    int tempInt = 0;
                    int.TryParse(rows[0]["sszcolor"].ToString(), out tempInt);
                    cellColor = Color.FromArgb(tempInt);
                }
            }
            return cellColor;
        }
        private void gview_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string point = "", columnname = "";

            try
            {
                if (e.Column.Tag.ToString() == "2")
                {
                    if (e.Column.FieldName.Contains("point"))
                    {
                        columnname = e.Column.FieldName;
                    }
                    else
                    {
                        columnname = "point" + e.Column.FieldName.Substring(0, 1);
                    }
                    object obj = gview.GetRowCellValue(e.RowHandle, gview.Columns[columnname]);
                    if (obj != null)
                    {
                        point = obj.ToString();
                        e.Appearance.ForeColor = GetRowCellColor(point);
                    }
                }

                if (e.Column.Tag.ToString() == "3")
                {
                    columnname = "point" + e.Column.FieldName.Substring(0, 1);
                    object obj = gview.GetRowCellValue(e.RowHandle, gview.Columns[columnname]);
                    if (obj != null)
                    {
                        point = obj.ToString();
                        e.Appearance.ForeColor = GetRowCellColor(point);
                    }
                }
                if (e.Column.Tag.ToString() == "4")
                {
                    columnname = "point" + e.Column.FieldName.Substring(0, 1);
                    object obj = gview.GetRowCellValue(e.RowHandle, gview.Columns[columnname]);
                    if (obj != null)
                    {
                        point = obj.ToString();
                        e.Appearance.ForeColor = GetRowCellColor(point);
                    }
                }
                if (e.Column.Tag.ToString() == "17")//分级报警背景颜色显示
                {

                    object obj = gview.GetRowCellValue(e.RowHandle, gview.Columns[e.Column.FieldName]);

                    if (obj != null)
                    {
                        int level = 0;
                        int.TryParse(obj.ToString().Replace("级", ""), out level);
                        switch (level)
                        {
                            case 1:
                                e.Appearance.BackColor = Color.Red;
                                break;
                            case 2:
                                e.Appearance.BackColor = Color.Orange;
                                break;
                            case 3:
                                e.Appearance.BackColor = Color.Yellow;
                                break;
                            case 4:
                                e.Appearance.BackColor = Color.Blue;
                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void gridC_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 获取选择测点
        /// </summary>
        /// <returns></returns>
        private string GetSelectPoint()
        {
            int row = 0;
            string point = "";
            try
            {
                row = gview.FocusedRowHandle;
                if (row > -1 && gview.FocusedColumn != null)
                {
                    if (gview.FocusedColumn.FieldName.Contains("point"))
                    {
                        point = gview.GetFocusedRowCellValue(gview.FocusedColumn).ToString();
                    }
                    else if (gview.FocusedColumn.FieldName.Contains("name"))
                    {
                        point = gview.GetFocusedRowCellValue("point" + gview.FocusedColumn.FieldName.Substring(0, 1)).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
            return point;
        }

        /// <summary>
        /// 判断测点是否为某种类型
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lx"></param>
        /// <returns></returns>
        private bool PointLx(string point, string lx)
        {
            bool flg = false;
            try
            {
                DataRow[] rows = null;
                if (!string.IsNullOrEmpty(point))
                {
                    lock (StaticClass.allPointDtLockObj)
                    {
                        rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                        if (rows.Length > 0)
                        {
                            if (rows[0]["lx"].ToString() == lx)
                            {
                                flg = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
            return flg;
        }

        private void gview_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string point = "";
                if (doublesplite) return;
                point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (gview.GetFocusedRowCellValue(gview.FocusedColumn).ToString() == "类型有误")
                    {//如果当前选择的单元格为类型有误
                        SensorTypeErrorDisplay sensorTypeErrorDisplay = new SensorTypeErrorDisplay(point);
                        sensorTypeErrorDisplay.ShowDialog();
                    }
                    else
                    {
                        if (PointLx(point, "控制量"))
                        {
                            KzRealForm kzshow = new KzRealForm(point);
                            kzshow.ShowDialog();
                        }
                        else if (PointLx(point, "分站"))
                        {
                            FzShowForm fzshow = new FzShowForm(point);
                            fzshow.ShowDialog();
                        }
                        else if (PointLx(point, "模拟量"))
                        {
                            AnalogRealForm analogshow = new AnalogRealForm(point);
                            analogshow.ShowDialog();
                        }
                        else if (PointLx(point, "开关量"))
                        {
                            SwitchRealForm switchshow = new SwitchRealForm(point);
                            switchshow.ShowDialog();
                        }
                        else if (PointLx(point, "识别器"))
                        {
                            RecognizerRealForm recognizershow = new RecognizerRealForm(point);
                            recognizershow.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs(ex.Message, ex);
            }
        }

        private void ckc_lx_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ckc_lx.Checked)
            {
                timer2.Enabled = true;
                timer2.Interval = StaticClass.realdataconfig.BaseCfg.PageChangeInterval * 1000;
            }
            else
            {
                //ckc_lx .Glyph =System .Drawing .Bitmap
                timer2.Enabled = false;
            }
        }

        private void tbtn_syy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PageCount > 1)
            {
                if (CrutPage > 1)
                {
                    CrutPage--;
                    Fyshow();
                }
            }
        }

        private void tbtn_xyy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PageCount > 1)
            {
                if (CrutPage < PageCount)
                {
                    CrutPage++;
                    Fyshow();
                }
            }
        }

        private void btn_my_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (PageCount > 1)
            {
                if (CrutPage < PageCount)
                {
                    CrutPage = PageCount;
                    Fyshow();
                }
            }
        }

        private void gview_Click(object sender, EventArgs e)
        {
        }

        private void gview_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        /// <summary>
        /// 历史运行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (point.Length <= 6)//如果是应急联动人员定位或者广播系统的设备，则不调用运行记录  20180309
                {
                    OprFuction.MessageBoxShow(0, "请选择监控系统设备");
                    return;
                }
                if (!string.IsNullOrEmpty(point))
                {
                    point1.Add("SourceIsList", "true");
                    point1.Add("Key_viewsbrunlogreport1_point", "等于&&$" + point);
                    point1.Add("Display_viewsbrunlogreport1_point", "等于&&$" + point);
                }
                point1.Add("ListID", "27");
                //RequestUtil.ExcuteCommand("Requestsbrunlogreport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("历史运行记录", ex);
            }
        }

        /// <summary>
        /// 实时运行记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string point = GetSelectPoint();
                frmRunLog log = new frmRunLog(point);
                log.ShowDialog();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("实时运行记录窗体异常", ex);
            }
        }

        private string GetPointID(string point)
        {
            string str = point;
            DataRow[] rows;
            lock (StaticClass.allPointDtLockObj)
            {
                rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                if (rows.Length > 0)
                {
                    str = rows[0]["pointid"].ToString();
                }
            }
            return str;
        }

        /// <summary>
        /// 实时曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_SSZChart", point1, false);
                        Mnl_SSZChart sszChart = new Mnl_SSZChart(point1);
                        sszChart.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_SSZChart", null, false);
                    Mnl_SSZChart sszChart = new Mnl_SSZChart();
                    sszChart.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量实时曲线", ex);
            }
        }
        /// <summary>
        /// 实时故障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmFaultInfo log = new frmFaultInfo();
                log.ShowDialog();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("实时故障窗体异常", ex);
            }
        }
        /// <summary>
        /// 网络模块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                NetForm net = new NetForm();
                net.ShowDialog();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("网络模块窗体异常", ex);
            }
        }
        /// <summary>
        /// 历史曲线5分钟
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_FiveMiniteLine", point1, false);
                        Mnl_FiveMiniteLine fiveMiniteLine = new Mnl_FiveMiniteLine(point1);
                        fiveMiniteLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_FiveMiniteLine", null, false);
                    Mnl_FiveMiniteLine fiveMiniteLine = new Mnl_FiveMiniteLine();
                    fiveMiniteLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量历史曲线5分钟", ex);
            }
        }
        /// <summary>
        /// 多点同屏曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_LineWithScreen", point1, false);
                        Mnl_LineWithScreen lineWithScreen = new Mnl_LineWithScreen(point1);
                        lineWithScreen.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_LineWithScreen", null, false);
                    Mnl_LineWithScreen lineWithScreen = new Mnl_LineWithScreen();
                    lineWithScreen.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量多点曲线", ex);
            }
        }

        /// <summary>
        /// 状态图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestKgl_StateLine", point1, false);
                        Kgl_StateLine kgl_StateLine = new Kgl_StateLine(point1);
                        kgl_StateLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestKgl_StateLine", null, false);
                    Kgl_StateLine kgl_StateLine = new Kgl_StateLine(point1);
                    kgl_StateLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量状态图", ex);
            }
        }

        /// <summary>
        /// 密采记录查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point1.Add("SourceIsList", "true");
                        point1.Add("Key_viewmcrunlogreport1_point", "等于&&$" + point);
                        point1.Add("Display_viewmcrunlogreport1_point", "等于&&$" + point);
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                        return;
                    }
                }
                point1.Add("ListID", "28");
                //RequestUtil.ExcuteCommand("RequestMCRungLogReport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();

            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量密采记录", ex);
            }
        }

        /// <summary>
        /// 开关量状态变动记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();

                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        //point = GetPointID(point);

                        point1.Add("SourceIsList", "true");
                        point1.Add("Key_ViewJC_KGStateMonth1_point", "等于&&$" + point);
                        point1.Add("Display_ViewJC_KGStateMonth1_point", "等于&&$" + point);
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                        return;
                    }
                }
                point1.Add("ListID", "17");
                //RequestUtil.ExcuteCommand("RequestKGLStateRBReport", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量状态变动记录", ex);
            }

        }

        /// <summary>
        /// 模拟量调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OprFuction.MessageBoxShow(0, "未实现");
        }

        /// <summary>
        /// 显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RealDatadisplaySetForm set = new RealDatadisplaySetForm();
                set.ShowDialog();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("实时显示设置窗体异常", ex);
            }
        }

        /// <summary>
        /// 参数设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //try
            //{
            //    RequestUtil.ExcuteCommand("RequestPointMrg", null, false);
            //}
            //catch (Exception ex)
            //{
            //    OprFuction.SaveErrorLogs("参数设置", ex);
            //}

            try
            {
                int resultIsMaster = MasterManagement.IsMaster();//等于0，表示正常
                if (resultIsMaster == 1)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("当前非主控电脑,请确认本机是否为主控并检查本机网络是否正常！");
                    return;
                }
                if (resultIsMaster == 2)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("连接服务器失败,请检查网络是否正常！");
                    return;
                }
                if (resultIsMaster == 3)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("获取当前主机是否为主控主机失败，详细见日志！");
                    return;
                }

                string point = GetSelectPoint();
                string fzh = point.Substring(0, 3);
                string kh = point.Substring(4, 2);

                Jc_DefInfo def = pointDefineService.GetPointDefineCacheByPoint(new Request.PointDefine.PointDefineGetByPointRequest()
                {
                    Point = point
                }).Data;
                frmLogOn loginForm = new frmLogOn(false);
                loginForm.ShowDialog();
                if (LoginManager.isLoginSuccess)//登录成功
                {
                    CFPointMrgFrame defineform = new CFPointMrgFrame(def.Fzh, def.Kh, def.Dzh, (int)def.DevPropertyID, int.Parse(def.Devid));
                    defineform.Show();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        /// <summary>
        /// 模拟量日班报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Dictionary<string, string> point1 = new Dictionary<string, string>();
                //point1.Add("SourceIsList", "true");
                //point1.Add("Key_viewMLLDayReport1_point", "等于&&$" );
                //point1.Add("Display_viewMLLDayReport1_point", "等于&&$");
                point1.Add("ListID", "9");
                //RequestUtil.ExcuteCommand("Requestddd", point1, false);
                Sys.Safety.Reports.frmList listReport = new Sys.Safety.Reports.frmList(point1);
                listReport.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量日班报表", ex);
            }

        }

        /// <summary>
        /// 模块同屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //RequestUtil.ExcuteCommand("RequestMnlAndKgl_LineWithScreen", null, false);
                MnlAndKgl_LineWithScreen kgl_StateLine = new MnlAndKgl_LineWithScreen();
                kgl_StateLine.Show();
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("调用模开同屏曲线异常", ex);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Gv_init(ShowType, CustomPage, areamsg);
        }

        /// <summary>
        /// 开关量柱状图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "开关量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestKgl_StateBar", point1, false);
                        Kgl_StateBar kgl_StateBar = new Kgl_StateBar(point1);
                        kgl_StateBar.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择开关量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestKgl_StateBar", null, false);
                    Kgl_StateBar kgl_StateBar = new Kgl_StateBar();
                    kgl_StateBar.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("开关量柱状图", ex);
            }
        }

        /// <summary>
        /// 模拟量历史曲线密采
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_McLine", point1, false);
                        Mnl_McLine mnl_McLine = new Mnl_McLine(point1);
                        mnl_McLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_McLine", null, false);
                    Mnl_McLine mnl_McLine = new Mnl_McLine();
                    mnl_McLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量历史曲线密采", ex);
            }
        }

        /// <summary>
        /// 取消当前报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //关闭图文、声光、语音报警、关闭右下角报警提示列表窗口
            StaticClass.CancelAlarm();
        }

        private void gview_CustomDrawEmptyForeground(object sender, DevExpress.XtraGrid.Views.Base.CustomDrawEventArgs e)
        {
        }

        private void ckc_ju_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gview.OptionsView.EnableAppearanceEvenRow = ckc_ju.Checked;
            gview.OptionsView.EnableAppearanceOddRow = ckc_ju.Checked;
            StaticClass.realdataconfig.BaseCfg.Showju = ckc_ju.Checked;
            StaticClass.RealDataDisplayCnfgDoc.SavaConfig("BaseCfg.Showju", StaticClass.realdataconfig.BaseCfg.Showju ? "1" : "0");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem1.Checked)
            {
                bar_mh.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                StaticClass.fuzzyserch = true;
            }
            else
            {
                bar_mh.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                StaticClass.fuzzyserch = false;
            }
            Gv_init(ShowType, CustomPage, areamsg);
        }

        private void bar_mh_EditValueChanged(object sender, EventArgs e)
        {
            StaticClass.fuzzyserchtext = bar_mh.EditValue.ToString();
            Gv_init(ShowType, CustomPage, areamsg);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gview.OptionsView.ShowIndicator = barCheckItem2.Checked;
        }

        private void gview_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        /// <summary>
        /// 已定义控制关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                KzDefineMsg df = new KzDefineMsg();
                df.ShowDialog();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> point1 = new Dictionary<string, string>();
            try
            {
                string point = GetSelectPoint();
                if (!string.IsNullOrEmpty(point))
                {
                    if (PointLx(point, "模拟量"))
                    {
                        point = GetPointID(point);
                        point1.Add("PointID", point);
                        //RequestUtil.ExcuteCommand("RequestMnl_DayZdzLine", point1, false);
                        Mnl_DayZdzLine mnl_DayZdzLine = new Mnl_DayZdzLine(point1);
                        mnl_DayZdzLine.Show();
                    }
                    else
                    {
                        OprFuction.MessageBoxShow(0, "请选择模拟量测点");
                    }
                }
                else
                {
                    //RequestUtil.ExcuteCommand("RequestMnl_DayZdzLine", null, false);
                    Mnl_DayZdzLine mnl_DayZdzLine = new Mnl_DayZdzLine();
                    mnl_DayZdzLine.Show();
                }
            }
            catch (Exception ex)
            {
                OprFuction.SaveErrorLogs("模拟量小时曲线", ex);
            }
        }

        private void gview_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    DataRow dr = gview.GetDataRow(e.RowHandle);
                    if (dr == null)
                        return;
                    List<string> stationFieldIndex = new List<string>();
                    foreach (DataColumn col in dr.Table.Columns)
                    {
                        if (col.ColumnName.Contains("point"))
                        {
                            if (dr[col.ColumnName].ToString().Length > 5)
                            {
                                if (dr[col.ColumnName].ToString().Substring(3, 1) == "0")
                                {
                                    stationFieldIndex.Add(col.ColumnName.Substring(col.ColumnName.Length - 1, 1));
                                }
                            }
                        }
                    }
                    //进行判断，只将分站部分的信息，字体加粗  20170923
                    if (stationFieldIndex.Contains(e.Column.FieldName.Substring(0, 1))
                        || stationFieldIndex.Contains(e.Column.FieldName.Substring(e.Column.FieldName.Length - 1, 1)) && e.Column.FieldName.Contains("point"))
                    {
                        Font font = new System.Drawing.Font("宋体", e.Appearance.Font.Size, FontStyle.Bold);
                        e.Appearance.Font = font;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("gview_RowStyle" + ex.Message + ex.StackTrace);
            }
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StaticClass.OpenAlarm();
        }

        private void gview_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {

        }
        private bool doublesplite = false;//表示当前双击的是否是列头，如果是列头不进行弹窗显示。

        private void gview_MouseDown(object sender, MouseEventArgs e)
        {
            //鼠标左键点击
            if (e.Button == MouseButtons.Left)
            {
                GridHitInfo info = gview.CalcHitInfo(e.X, e.Y);
                //在列标题栏内且列标题name是"colName"
                if (info.InColumnPanel)
                {
                    doublesplite = true;
                }
                else
                {
                    doublesplite = false;
                }

            }
        }
        /// <summary>
        /// 模拟图显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Dictionary<string, string> formParams = new Dictionary<string, string>();
            bool isSystemDesktop = false;
            RequestUtil.ExcuteCommand("requestgisedit12", formParams, isSystemDesktop);
        }
    }
}

