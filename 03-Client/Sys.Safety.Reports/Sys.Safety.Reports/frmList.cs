using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Design;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Controls;
using Sys.Safety.Reports.Model;
using Sys.Safety.Reports.PubClass;
using BorderSide = DevExpress.XtraPrinting.BorderSide;
using Sys.Safety.DataContract.Custom;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Font = System.Drawing.Font;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.Reports
{
    public partial class frmList : XtraForm
    {
        private static readonly ResourceManager res = new ResourceManager(typeof(frmList));
        //2015-10-26   用于存储每个查询条件引用的表名
        private IDictionary<string, string> _dicConditionOldTable = new Dictionary<string, string>();
        private FreQryCondition _freQryCondition;
        private List<string> _listdate; //日表日期串
        private DataTable _lmdDt; //当前方案选择列
        private string _strFreQryCondition = string.Empty;
        private string _strFreQryConditionByChs = "";
        private readonly string _strReceiveParaCondition = string.Empty;
        private string _strSortCondtion = string.Empty; //2016-10-18 ,存储测点编排
        private bool blnFirstOpenLoadData;
        private bool blnListFreCondition;
        private bool blnRefreshed;
        private int CurrentPageNumber;
        private readonly IDictionary<string, string> DicBluerCondition = new Dictionary<string, string>();
        private IDictionary<string, string> DicReceivePara = new Dictionary<string, string>();
        private BarButtonItem DoubleButtonItem;
        private DataTable dt;
        private DateTime executeBeginTime = DateTime.MinValue; //用于记录各种操作执行开始时间
        private string extendEntity = "";
        private readonly IList<RepositoryItemHyperLinkEdit> hypers = new List<RepositoryItemHyperLinkEdit>();
        private IDictionary<int, bool> IsHaveFieldRightDic;
        private ListdataexInfo listDataExVo;
        public IList<ListdisplayexInfo> listDisplayExList;
        private ListexInfo listExvo;
        private int listId;

        private IList<ListdatalayountInfo> listlayount;
        private ListtempleInfo listTemplevo;
        private DataTable metadataFieldDt;
        private readonly ListExModel Model = new ListExModel();
        private int PerPageRecord;
        private string realEntity = "";
        private ResourceManager rm = new ResourceManager(typeof(frmList));
        private string strListStyle = "";

        private string strOldStrListSql = "";
        private string strRight = "";
        private string strSortFileName = "";
        private string strSortText = "";
        private int TotalPage;
        private int TotalRecord;
        private int userId;
        private string voType = null;
        private WaitDialogForm wdf;
        private XAppearances xapp;

        IConfigService _configService = ServiceFactory.Create<IConfigService>();

        /// <summary>
        ///     列表的构造方法
        /// </summary>
        public frmList()
        {
            ListDataID = 0;
            InitializeComponent();
        }

        public frmList(int listid)
        {
            ListDataID = 0;
            InitializeComponent();
            listId = listid;
        }

        public frmList(int listid, IDictionary<string, string> dicReceivePara)
        {
            ListDataID = 0;
            InitializeComponent();
            listId = listid;
            DicReceivePara = dicReceivePara;
        }

        public frmList(Dictionary<string, string> dicMeun)
        {
            ListDataID = 0;
            InitializeComponent();
            listId = TypeUtil.ToInt(dicMeun["ListID"]);
            if (dicMeun.Count >= 4) //2015-08-07   如果传递的是四个参数，则认为是从实时界面进来
                DicReceivePara = dicMeun;
        }

        public void Reload(Dictionary<string, string> dicMeun)
        {
            ListDataID = 0;
            listId = TypeUtil.ToInt(dicMeun["ListID"]);
            if (dicMeun.Count >= 4)
                DicReceivePara = dicMeun;

            //object sender1 = null;
            //var e1 = new EventArgs();
            //frmList_Load(sender1, e1);
        }

        /// <summary>
        ///     设置或获取列表的ListDataID号
        /// </summary>
        public int ListDataID { get; set; }

        private void frmList_Load(object sender, EventArgs e)
        {
            try
            {
                //授权检测
                RunningInfo runinfo = _configService.GetRunningInfo().Data;
                if (runinfo.AuthorizationExpires)
                {
                    XtraMessageBox.Show("检测到您的授权已到期，请及时联系管理员重新授权！");
                    //return;
                }


                OpenWaitDialog("列表数据正在加载中");

                //设置窗体高度和宽度
                Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
                Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
                Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
                Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);

                //限制左侧宽度  20180405
                layoutControlItem1.Width = 300;
                layoutControlItem2.Width = 300;
                layoutItemGrid.Height = Height - 110;

                SetExecuteBeginTime();

                GetSysSetting(); //获取系统设置      
                GetListEx(); //获取列表头


                SetTitleInfo(); //设置标题信息 
                InitListConfig(); //获取配置
                GetListDataEx(); //获取列表方案数据
                GetListDisplay(); //获取列表显示数据
                SetListFormat(); //根据方案设置列表格式   
                ReceiveParaCondition(); //获取接收参数
                SetControlVisible();
                if (blnFirstOpenLoadData)
                    RefreshListData();
                buttonContent.TabIndex = 0; //打开列表后焦点自动到查询框

                gridView.LayoutChanged();

                //动态加载测点过滤layout
                var rows = metadataFieldDt.Select("MetaDataID=" + listExvo.MainMetaDataID + " and blnDesignSort=1");
                if (rows.Length == 0)
                {
                    layoutControlItem1.Visibility = LayoutVisibility.Never;
                }
                else
                {
                    //初始化测点去重复下拉
                    DataTable dtRemoveDuplication = new DataTable();
                    dtRemoveDuplication.Columns.Add("Num");
                    dtRemoveDuplication.Columns.Add("QcfItem");

                    var dr = dtRemoveDuplication.NewRow();
                    dr["Num"] = "1";
                    dr["QcfItem"] = "全部";
                    dtRemoveDuplication.Rows.Add(dr);

                    var dr2 = dtRemoveDuplication.NewRow();
                    dr2["Num"] = "2";
                    dr2["QcfItem"] = "去重复（删除前）";
                    dtRemoveDuplication.Rows.Add(dr2);

                    var dr3 = dtRemoveDuplication.NewRow();
                    dr3["Num"] = "3";
                    dr3["QcfItem"] = "去重复（删除后）";
                    dtRemoveDuplication.Rows.Add(dr3);

                    lookUpEditRemoveDuplication.Properties.DataSource = dtRemoveDuplication;
                    lookUpEditRemoveDuplication.EditValue = "1";

                    InitializeArrangeTime();

                    //初始化编排活动
                    var bindData = new List<Kvp>
                    {
                        new Kvp {Id = "1", Text = "活动测点"},
                        new Kvp {Id = "2", Text = "存储测点"}
                    };
                    lookUpEditArrangeActivity.Properties.DataSource = bindData;
                    lookUpEditArrangeActivity.EditValue = "1";

                    layoutControlItem1.Visibility = LayoutVisibility.Always;
                }

                frmListEx_SizeChanged(this, null);

                //前一天后一天显示控制
                var controls = panelFreQry.Controls;
                bool existQueryTime = false;
                foreach (Control item in controls)
                {
                    // 20180408
                    var type = item.GetType();
                    var typeName = type.Name;
                    if (typeName.Contains("DateTime") && !typeName.Contains("Year") && !typeName.Contains("Month"))
                    {
                        existQueryTime = true;
                        break;
                    }

                    //var hash = item.Tag as Hashtable;
                    //if (hash == null)
                    //{
                    //    continue;
                    //}

                    //if (!hash.ContainsKey("_fieldName"))
                    //{
                    //    continue;
                    //}

                    //var fieldName = hash["_fieldName"].ToString();
                    //var fieldNameSplit = fieldName.Split('_');
                    //var split2 = fieldNameSplit[fieldNameSplit.Length - 1].ToLower();
                    //if (split2 != "datsearch")
                    //{
                    //    continue;
                    //}

                    //existQueryTime = true;
                    //break;
                }

                if (existQueryTime)
                {
                    BeforeDay.Visibility = BarItemVisibility.Always;
                    AfterDay.Visibility = BarItemVisibility.Always;
                    simpleButton1.Visible = true;
                    simpleButton2.Visible = true;
                }
                else
                {
                    BeforeDay.Visibility = BarItemVisibility.Never;
                    AfterDay.Visibility = BarItemVisibility.Never;
                    simpleButton1.Visible = false;
                    simpleButton2.Visible = false;
                }



                MessageShowUtil.ShowStaticInfo("加载成功！加载时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                CloseWaitDialog();
            }
        }

        /// <summary>
        /// //初始化编排时间
        /// </summary>
        private void InitializeArrangeTime()
        {
            listlayount = Model.GetListDataLayountData(ListDataID);
            //listlayount = listlayount.OrderByDescending(a => a.StrDate).ToList();
            lookUpEditArrangeTime.Properties.DataSource = listlayount;
            lookUpEditArrangeTime.Properties.DisplayMember = "StrDate";  // 指定显示字段
            lookUpEditArrangeTime.Properties.ValueMember = "ListDataLayoutID";    // 指定值字段
            if (listlayount.Count != 0)
            {
                lookUpEditArrangeTime.EditValue = listlayount[0].ListDataLayoutID;
            }
        }

        /// <summary>
        ///     设置执行开始时间
        /// </summary>
        protected void SetExecuteBeginTime()
        {
            executeBeginTime = DateTime.Now;
        }

        /// <summary>
        ///     获取执行时间字符串
        /// </summary>
        /// <returns></returns>
        protected string GetExecuteTimeString()
        {
            var strTime = string.Empty;
            var ts = DateTime.Now - executeBeginTime;
            if (ts.Minutes > 0)
                strTime += ts.Minutes + "分";
            if (ts.Seconds > 0)
                strTime += ts.Seconds + "秒";
            if (ts.Milliseconds > 0)
                strTime += ts.Milliseconds + "毫秒";

            strTime += "。";

            return strTime;
        }

        /// <summary>
        ///     获取接收参数
        /// </summary>
        private void ReceiveParaCondition()
        {
            if (_lmdDt == null)
                _lmdDt = Model.GetListMetaData(ListDataID, listDataExVo.UserID);
            //if (!TypeUtil.ToBool(RequestUtil.GetParameterValue("SourceIsList")))
            //{
            //    return;
            //}
            if ((DicReceivePara == null) || !DicReceivePara.ContainsKey("SourceIsList") ||
                !TypeUtil.ToBool(DicReceivePara["SourceIsList"]))
                return;
            if (_lmdDt != null)
            {
                var count = _lmdDt.Rows.Count;
                var strFieldName = "";
                var strValue = "";
                var strDisplay = "";
                DataRow curDr = null;
                for (var i = 0; i < count; i++)
                {
                    curDr = _lmdDt.Rows[i];
                    if (!TypeUtil.ToBool(curDr["blnReceivePara"]))
                        continue;


                    strFieldName = TypeUtil.ToString(curDr["MetaDataFieldName"]);

                    strValue = DicReceivePara.ContainsKey("Key_" + strFieldName)
                        ? TypeUtil.ToString(DicReceivePara["Key_" + strFieldName])
                        : "";
                    strDisplay = DicReceivePara.ContainsKey("Display_" + strFieldName)
                        ? TypeUtil.ToString(DicReceivePara["Display_" + strFieldName])
                        : "";
                    if ((strValue != string.Empty) && (strValue != null))
                    {
                        //2017-01-24 ，如果是从超链接进来，则默认要加载列表数据
                        blnFirstOpenLoadData = true;
                        //strReceivePara += strValue;
                        //strReceivePara += " and " + strFieldName.Replace("_", ".") + "='" + strValue + "'";
                        if (TypeUtil.ToBool(curDr["blnFreCondition"]))
                        {
                            //常用条件

                            curDr["strFreCondition"] = strValue;
                            curDr["strFreConditionCHS"] = strDisplay;
                            if (TypeUtil.ToString(curDr["strFkCode"]).Length > 0)
                            {
                                //2017-01-24   如果列表跳列表的时候是参照类型的，需要将常用条件根据ID把汉字显示出来
                                var blnNewData = false;
                                var lookup = LookUpUtil.GetlookInfo(TypeUtil.ToString(curDr["strFkCode"]),
                                    ref blnNewData);
                                var dtsource = lookup["dataSource"] as DataTable;
                                var strValueMember = TypeUtil.ToString(lookup["StrValueMember"]);
                                var strValueID = strValue.Substring(strValue.IndexOf("$") + 1);
                                var rowslookup = dtsource.Select(strValueMember + "='" + strValueID + "'");
                                if (rowslookup.Length > 0)
                                    curDr["strFreConditionCHS"] = rowslookup[0][lookup["StrDsiplayMember"].ToString()];
                            }
                        }
                        else
                        {
                            curDr["strCondition"] = strValue;
                            curDr["strConditionCHS"] = strDisplay;
                        }
                    }
                }

                var strListSQL = Model.GetSQL(_lmdDt);
                listDataExVo.StrListSQL = strListSQL;
                _freQryCondition.CreateControl(_lmdDt);
                //2015-10-27   如果是通过列表跳列表方式进来后，加载条件后需要将条件的元数据清空，否则点击方案进行保存会把这个条件也保存
                var rows = _lmdDt.Select("len(strFreCondition)>0 or len(strConditionCHS)>0");
                foreach (var row in rows)
                    row["strFreCondition"] = row["strFreConditionCHS"] = "";
                SetFrmQryConditionSize();
                if (_freQryCondition != null)
                    _strFreQryCondition = _freQryCondition.GetFreQryCondition();
            }

            //_strReceiveParaCondition = strReceivePara;
        }

        private void SetControlVisible()
        {
            PermissionManager.HavePermission("OpenReportScheme", tlbSchema);
            PermissionManager.HavePermission("SaveReportFreQryCon", tlbSaveFreQryCon);
            PermissionManager.HavePermission("SaveReportColumnWidth", tlbSaveWidth);
            //PermissionManager.HavePermission("ExportReportData", tlbExportExcel);
            //PermissionManager.HavePermission("ExportReportData", tlbExeclPDF);
            //PermissionManager.HavePermission("ExportReportData", txtExportTXT);
            //PermissionManager.HavePermission("ExportReportData", txtExportCSV);
            //PermissionManager.HavePermission("ExportReportData", txtExportHTML);
            //PermissionManager.HavePermission("PrintReport", tlbPrint);
            PermissionManager.HavePermission("OpenReportDesign", tlbPivotSetting);

            // 20170629
            //var RowsblnKeyWord = _lmdDt.Select("blnKeyWord=1");
            //if ((RowsblnKeyWord == null) || (RowsblnKeyWord.Length == 0))
            //    chkblnKeyWord.Visibility = BarItemVisibility.Never;
        }

        /// <summary>
        ///     获取系统设置
        /// </summary>
        private void GetSysSetting()
        {
            userId = TypeUtil.ToInt(RequestUtil.GetParameterValue("userId"));

            var listRowCount = TypeUtil.ToInt(RequestUtil.GetParameterValue("ListRowCount"));
            blnFirstOpenLoadData = TypeUtil.ToBool(RequestUtil.GetParameterValue("blnFirstOpenLoadData"));

            // 20170623
            //blnListFreCondition = true; // TypeUtil.ToBool(RequestUtil.GetParameterValue("blnListFreCondition"));
            blnListFreCondition = TypeUtil.ToBool(RequestUtil.GetParameterValue("blnListFreCondition"));


            listRowCount = listRowCount > 0 ? listRowCount : 500;
            if (listRowCount > 0)
            {
                //tlbSetPerPageNumber.EditValueChanged -= tlbSetPerPageNumber_EditValueChanged;
                //tlbSetPerPageNumber.EditValue = listRowCount;
                //tlbSetPerPageNumber.EditValueChanged += tlbSetPerPageNumber_EditValueChanged;

                comboBoxEdit1.SelectedValueChanged -= tlbSetPerPageNumber_EditValueChanged;
                comboBoxEdit1.Text = listRowCount.ToString();
                comboBoxEdit1.EditValueChanged += tlbSetPerPageNumber_EditValueChanged;
            }
            if (blnFirstOpenLoadData)
                blnRefreshed = true;

            //2014-11-05 判断操作员是否可以点击方案按钮，需要权限方法出来后再调用此方法
            //if (!PermissionManager.HavePermission("EditSchema"))
            //{
            //    this.tlbSchema.Visibility = BarItemVisibility.Never;
            //}
        }

        /// <summary>
        ///     获取列表实体
        /// </summary>
        private void GetListEx()
        {
            listExvo = Model.GetListEx(listId);
            if ((listExvo == null) || (listExvo.ListID <= 0))
                throw new Exception("请求列表未找到!");


            metadataFieldDt = ClientCacheModel.GetServerMetaDataFields();
        }

        /// <summary>
        ///     设置标题信息
        /// </summary>
        private void SetTitleInfo()
        {
            lblTile.Text = listExvo.StrListName + "       ";
            if (Parent == null)
                Text = listExvo.StrListName;

            //if (this.Parent != null)
            //{//处理主控标题问题,由于现在的主控用的tablepage方式，所以需要赋值
            //    Control c = this.Parent;
            //    string str = c.GetType().Name;
            //    if (str.ToLower() == "xtratabpage")
            //        c.Text = this.Text;
            //}
        }

        /// <summary>
        ///     初始化相关列表
        ///     列表的方案
        ///     设置列表显示样式参照
        ///     列表的风格
        ///     列表的工具栏
        /// </summary>
        private void InitListConfig()
        {
            InitGroupListToolbar();
            InitListSchema();
            SetListShowStyleData();
            InitListToolbarConfig();
        }

        /// <summary>
        ///     初始化相关列表
        /// </summary>
        private void InitGroupListToolbar()
        {
            if (string.IsNullOrEmpty(listExvo.StrListGroupCode))
            {
                tlbGroupList.Visibility = BarItemVisibility.Never;
                return;
            }

            var strArr = listExvo.StrListGroupCode.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var strSql = "";
            foreach (var str in strArr)
                if (strSql == "")
                    strSql =
                        string.Format(
                            @"select ListID,strListCode,strListName,strListGroupCode,strRightCode from BFT_ListEx where blnList=1 and  ' ' + strListGroupCode + ' ' like '% {0} %' and ListID<>" +
                            listExvo.ListID, str);
                else
                    strSql +=
                        string.Format(
                            @" union select ListID,strListCode,strListName,strListGroupCode,strRightCode from BFT_ListEx where blnList=1 and  ' ' + strListGroupCode + ' ' like '% {0} %' and ListID<>" +
                            listExvo.ListID, str);

            var groupListDt = Model.GetDataTable(strSql);
            BarButtonItem tlbItem;
            var tlbId = 101;
            foreach (DataRow dr in groupListDt.Rows)
            {
                tlbItem = new BarButtonItem();
                tlbItem.Caption = TypeUtil.ToString(dr["strListName"]);
                tlbItem.Id = tlbId++;
                tlbItem.Name = "tlbLinkList" + TypeUtil.ToString(dr["strListCode"]);
                tlbItem.Tag = dr;
                tlbItem.ItemClick += tlbItem_ItemClick;
                tlbGroupList.AddItem(tlbItem);
            }

            if (groupListDt.Rows.Count <= 0)
                tlbGroupList.Visibility = BarItemVisibility.Never;
        }

        /// <summary>
        ///     关联列表打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var tlbItem = e.Item as BarButtonItem;
                var dr = tlbItem.Tag as DataRow;
                var strParameters = "Entity=" + TypeUtil.ToString(dr["strListCode"]).Trim() + "&ListID=" +
                                    TypeUtil.ToInt(dr["ListID"]);

                var frm = new frmList(TypeUtil.ToInt(TypeUtil.ToInt(dr["ListID"])));
                frm.MdiParent = MdiParent;
                frm.Show();
                //需要实现
                //RequestUtil.ExcuteCommand("ListEx", strParameters, null, TypeUtil.ToString(dr["strRightCode"]).Trim());
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     初始化列表方案数据
        /// </summary>
        private void InitListSchema()
        {
            var dt = Model.GetSchemaList(listExvo.ListID);
            lookupSchema.Properties.DataSource = dt;
            if (dt.Rows.Count > 0) //默认第一个方案
            {
                ListDataID = TypeUtil.ToInt(dt.Rows[0]["ListDataID"]);

                lookupSchema.EditValueChanged -= lookupSchema_EditValueChanged;
                lookupSchema.EditValue = ListDataID;
                lookupSchema.EditValueChanged += lookupSchema_EditValueChanged;
            }
        }

        /// <summary>
        ///     设置列表显示样式参照
        /// </summary>
        public void SetListShowStyleData()
        {
            try
            {
                var listShowStyleDt = new DataTable();
                listShowStyleDt.Columns.Add(new DataColumn("Code", Type.GetType("System.String")));
                listShowStyleDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

                var dr = listShowStyleDt.NewRow();
                dr["Code"] = "list";
                dr["Name"] = "列表";
                listShowStyleDt.Rows.Add(dr);

                if (listExvo.BlnPivot)
                {
                    dr = listShowStyleDt.NewRow();
                    dr["Code"] = "pivot";
                    dr["Name"] = "报表";
                    listShowStyleDt.Rows.Add(dr);
                }
                if (listExvo.BlnChart)
                {
                    dr = listShowStyleDt.NewRow();
                    dr["Code"] = "chart";
                    dr["Name"] = "图表";
                    listShowStyleDt.Rows.Add(dr);
                }

                lookupShowStyle.Properties.DataSource = listShowStyleDt;

                lookupShowStyle.EditValueChanged -= lookupShowStyle_EditValueChanged;
                lookupShowStyle.EditValue = "list"; //默认为列表


                lookupShowStyle.EditValueChanged += lookupShowStyle_EditValueChanged;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     列表的显示风格
        /// </summary>
        private void InitGridStyleConfig()
        {
            if (gridControl.DataSource == null)
                return;
            xapp = new XAppearances(AppDomain.CurrentDomain.BaseDirectory + "Config\\" + "listStyle.xml");
            BarCheckItem item;
            foreach (var o in xapp.FormatNames)
            {
                item = new BarCheckItem(barManager, false);
                item.Caption = o.ToString();
                item.Name = o.ToString();
                item.ItemClick += item_ItemClick;
                menuListStyle.AddItems(new BarItem[] { item });
            }
            var strListStyle = RequestUtil.GetParameterValue("ListStyle");
            SetListStyle(strListStyle);
        }

        /// <summary>
        ///     初始化列表的工具栏
        /// </summary>
        private void InitListToolbarConfig()
        {
            tlbPivotSetting.Visibility = BarItemVisibility.Never;
            tlbChartSetting.Visibility = BarItemVisibility.Never;


            IList<ListcommandexInfo> listCommands = Model.GetListCommandExDTOs(listExvo.ListID);
            var bar = barManager.Bars["ToolBar"];
            if (listCommands != null)
            {
                //增加判断显示ToolBar  20180407
                if (listCommands.Count > 0)
                {
                    ToolBar.Visible = true;
                }
                foreach (var listCommand in listCommands)
                {
                    var button = new BarButtonItem();
                    button.Caption = listCommand.StrListCommandName;
                    button.Hint = listCommand.StrListCommandTip;
                    button.Tag = listCommand.StrRequestCode + "&" + listCommand.RequestId; //需要确定后再开发
                    if (listCommand.BlnDblClick)
                        DoubleButtonItem = button;
                    if (TypeUtil.ToString(listCommand.StrListIconName) != "")
                    {
                        button.Glyph = res.GetBitmap(listCommand.StrListIconName);
                        if (button.Glyph == null)
                        {
                            var strPath = AppDomain.CurrentDomain.BaseDirectory + "Image\\ListReportImage\\" +
                                          listCommand.StrListIconName;
                            if (File.Exists(strPath))
                                button.Glyph = Image.FromFile(strPath);
                        }
                    }

                    if (realEntity == string.Empty)
                    {
                        //string strPara = (string)commandItem.Tag;
                        //if (strPara != string.Empty)
                        //{
                        //    string[] strs = strPara.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        //    for (int i = 0; i < strs.Length; i++)
                        //    {
                        //        if (strs[i].Contains("RealEntity"))
                        //        {
                        //            this.realEntity = strs[i].Substring(11);
                        //            break;
                        //        }
                        //    }
                        //}
                    }

                    button.PaintStyle = BarItemPaintStyle.CaptionGlyph;
                    button.ItemClick += button_ItemClick;

                    bar.AddItem(button);
                    barManager.Items.Add(button);
                    if (listCommand.Parameters != "")
                        PermissionManager.HavePermission(listCommand.Parameters, button);
                }

                foreach (var listCommand in listCommands)
                {
                    if (extendEntity != string.Empty)
                        continue;

                    var strPara = listCommand.Parameters;
                    if (strPara != string.Empty)
                    {
                        var strs = strPara.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < strs.Length; i++)
                            if (strs[i].Contains("Entity"))
                            {
                                extendEntity = strs[i].Substring(7);
                                break;
                            }
                    }
                }
            }

            if (realEntity == string.Empty)
                realEntity = extendEntity;
        }

        private void SetGridColumnFormat(GridColumn gridColumn, string strDisplayFormat, string strCoinDecimalNum,
            string strWeightDecimalNum)
        {
            if (strDisplayFormat == "数量")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.Numeric;
                gridColumn.DisplayFormat.FormatString = "n0";
                gridColumn.SummaryItem.DisplayFormat = "{0:n0}";
            }
            else if (strDisplayFormat == "货币")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.Numeric;
                gridColumn.DisplayFormat.FormatString = "n" + strCoinDecimalNum;
                gridColumn.SummaryItem.DisplayFormat = "{0:n" + strCoinDecimalNum + "}";
            }
            else if (strDisplayFormat == "重量")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.Numeric;
                gridColumn.DisplayFormat.FormatString = "n" + strWeightDecimalNum;
                gridColumn.SummaryItem.DisplayFormat = "{0:n" + strWeightDecimalNum + "}";
            }
            else if (strDisplayFormat == "日期")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.DateTime;
                gridColumn.DisplayFormat.FormatString = "yyyy-MM-dd";
            }
            else if (strDisplayFormat == "时间")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.DateTime;
                gridColumn.DisplayFormat.FormatString = "HH:mm:ss";
            }
            else if (strDisplayFormat == "日期时间")
            {
                gridColumn.DisplayFormat.FormatType = FormatType.DateTime;
                gridColumn.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            }
            if (gridColumn.SummaryItem.SummaryType == SummaryItemType.Count)
                gridColumn.SummaryItem.DisplayFormat = "{0:n0}";
        }

        private void SetGridColumnFormat()
        {
            //if (this.gridControl.DataSource == null)
            //{
            //    return;
            //}

            if (_lmdDt == null)
                return;


            foreach (GridGroupSummaryItem groupSumItem in gridView.GroupSummary)
                if (groupSumItem.DisplayFormat == string.Empty)
                    groupSumItem.DisplayFormat = "{0:" +
                                                 gridView.Columns[groupSumItem.FieldName].DisplayFormat.FormatString +
                                                 "}";
        }

        /// <summary>
        ///     条件格式设置
        /// </summary>
        private void SetGridColumnConditionFormat(GridColumn gridColumn, string strConditionFormat)
        {
            var sfcType = (SFCDataTypeEnum)TypeUtil.ToInt(strConditionFormat.Substring(strConditionFormat.Length - 1));
            var strTemp = strConditionFormat.Remove(strConditionFormat.Length - 1);
            var strConditions = strTemp.Split(new[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);
            if (strConditions.Length > 0)
            {
                var str = string.Empty;
                var strOper = string.Empty;
                var strValue1 = string.Empty;
                var strValue2 = string.Empty;
                var blnApplyRow = false;
                StyleFormatCondition gridSFC = null;
                var color = Color.White;
                var fontColor = Color.Black;
                bool bBold = false;
                bool bItalic = false;
                bool bUnderline = false;
                bool bStrikeout = false;

                // 20170801
                for (var i = 0; i < strConditions.Length; i++)
                {
                    str = strConditions[i];
                    if (str.Contains("&&$"))
                    {
                        // 20170906
                        //var strs = str.Split(new[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                        var strs = str.Split(new[] { "&&$" }, StringSplitOptions.None);
                        strOper = strs[0];
                        blnApplyRow = TypeUtil.ToBool(strs[1]);
                        strValue1 = "";
                        strValue2 = "";

                        //if (strs.Length > 3)
                        //    strValue1 = strs[2];
                        //if (strs.Length > 4)
                        //    strValue2 = strs[3];
                        strValue1 = strs[2];
                        if (!(strs[3] == "##*"))
                        {
                            strValue2 = strs[3];
                        }
                        else
                        {
                            strValue2 = "";
                        }

                        var ss = strs[4].Split(';');
                        Model.GetColorByString(ss, ref color);

                        var fontss = strs[5].Split(';');
                        Model.GetColorByString(fontss, ref fontColor);

                        bBold = TypeUtil.ToBool(strs[6]);

                        if (strs.Length >= 8)
                        {
                            bItalic = Convert.ToBoolean(strs[7]);
                        }
                        if (strs.Length >= 9)
                        {
                            bUnderline = Convert.ToBoolean(strs[8]);
                        }
                        if (strs.Length >= 10)
                        {
                            bStrikeout = Convert.ToBoolean(strs[9]);
                        }
                    }

                    gridSFC = new StyleFormatCondition();
                    //设置粗体/斜体/下划线/删除线
                    if (bBold || bItalic || bUnderline || bStrikeout)
                    {
                        gridSFC.Appearance.Options.UseFont = true;
                        FontStyle fs = FontStyle.Regular;
                        if (bBold)
                        {
                            fs |= FontStyle.Bold;
                        }
                        if (bItalic)
                        {
                            fs |= FontStyle.Italic;
                        }
                        if (bUnderline)
                        {
                            fs |= FontStyle.Underline;
                        }
                        if (bStrikeout)
                        {
                            fs |= FontStyle.Strikeout;
                        }
                        gridSFC.Appearance.Font = new Font(gridSFC.Appearance.Font, fs);
                    }

                    gridSFC.Appearance.Options.UseBackColor = true; //背景色
                    gridSFC.Appearance.BackColor = color;
                    gridSFC.Appearance.Options.UseForeColor = true; //字体颜色
                    gridSFC.Appearance.ForeColor = fontColor;

                    gridSFC.Column = gridColumn;
                    gridSFC.ApplyToRow = blnApplyRow;
                    gridSFC.Condition = FormatConditionEnum.Expression;

                    var strCaclOper = GetCaclOper(strOper, gridColumn.FieldName);
                    gridSFC.Expression = string.Format(gridColumn.FieldName + strCaclOper, strValue1, strValue2);
                    gridView.FormatConditions.Add(gridSFC);
                }
            }
        }


        /// <summary>
        ///     模糊条件格式设置
        /// </summary>
        private void SetGridColumnBluerFormat(string strFileName, string strBluerCondition)
        {
            var strConditions = strBluerCondition.Split(new[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);
            if (strConditions.Length > 0)
                foreach (var s in strConditions)
                {
                    var ss = s.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length > 0)
                        DicBluerCondition.Add(strFileName, strBluerCondition);
                }
        }


        /// <summary>
        ///     得到编排串
        /// </summary>
        /// <param name="strListDisplayFieldName"></param>
        private void SetGridColumnSort(string strListDisplayFieldName)
        {
            //查找列表元数据里有没有设置了编排顺序(现在就是用的固定条件)
            if (_lmdDt == null) return;
            var rows =
                _lmdDt.Select("MetaDataID=" + listExvo.MainMetaDataID + " and MetaDataFieldName='" +
                              strListDisplayFieldName + "' and len(strCondition)>0");
            if (rows.Length > 0)
            {
                var MetaDataFileID = TypeUtil.ToInt(rows[0]["MetaDataFieldID"]);
                var rowm =
                    metadataFieldDt.Select("MetaDataID=" + listExvo.MainMetaDataID + " and ID=" + MetaDataFileID +
                                           " and blnDesignSort=1");
                if (rowm.Length > 0)
                {
                    //判断元数据里此列有没有设置为可编排
                    strSortText = TypeUtil.ToString(rows[0]["strCondition"]);
                    strSortText = strSortText.Substring(5);
                    strSortFileName = strListDisplayFieldName;
                }
            }
        }

        /// <summary>
        ///     获取列表数据实体
        /// </summary>
        private void GetListDataEx()
        {
            //获取列表方案数据
            listDataExVo = Model.GetListDataExData(ListDataID);
            strOldStrListSql = listDataExVo.StrListSQL;
            if ((listDataExVo == null) || (listDataExVo.ListDataID <= 0))
                throw new Exception("当前列表没有方案，请进行方案设置!");

            listTemplevo = Model.GetListTemple(listDataExVo.ListDataID);

            if ((listDataExVo != null) && (listDataExVo.StrDefaultShowStyle != string.Empty) &&
                (listDataExVo.StrDefaultShowStyle != null))
            {
                lookupShowStyle.EditValueChanged -= lookupShowStyle_EditValueChanged;
                lookupShowStyle.EditValue = listDataExVo.StrDefaultShowStyle; //默认为列表
                lookupShowStyle.EditValueChanged += lookupShowStyle_EditValueChanged;
            }

            InitFreQryCondition(); //初始化查询条件


            if (blnListFreCondition ||
                ((DicReceivePara != null) && DicReceivePara.ContainsKey("SourceIsList") &&
                 TypeUtil.ToBool(DicReceivePara["SourceIsList"])))
                chkItemFreQryCondition.Checked = true;
        }

        /// <summary>
        ///     获取列表显示数据
        /// </summary>
        private void GetListDisplay()
        {
            listDisplayExList = Model.GetListDisplayExData(listDataExVo.ListDataID);

            //是否控制字段权限字典
            if (IsHaveFieldRightDic == null)
                IsHaveFieldRightDic = new Dictionary<int, bool>();
            else
                IsHaveFieldRightDic.Clear();

            var fieldRightDt = Model.GetMetaDataFieldsByListDataID(listDataExVo.ListDataID);
            var fieldId = 0;
            for (var i = 0; i < fieldRightDt.Rows.Count; i++)
            {
                fieldId = TypeUtil.ToInt(fieldRightDt.Rows[i]["ID"]);
                if (IsHaveFieldRightDic.ContainsKey(fieldId))
                    continue;

                IsHaveFieldRightDic.Add(fieldId, TypeUtil.ToBool(fieldRightDt.Rows[i]["blnFieldRight"]));
            }
        }

        /// <summary>
        ///     是否有字段权限
        /// </summary>
        /// <returns>true/false</returns>
        private bool IsHaveFieldRight(bool isCalcField, string strFieldFullName)
        {
            var blnExist = true;
            if (IsHaveFieldRightDic == null)
                return true;

            var fieldId = 0;
            var drs = _lmdDt.Select("MetaDataFieldName='" + strFieldFullName + "'");
            if ((drs != null) && (drs.Length > 0))
            {
                fieldId = TypeUtil.ToInt(drs[0]["MetaDataFieldID"]);
                if (isCalcField)
                {
                    //计算列特殊处理
                    var strRefColList = TypeUtil.ToString(drs[0]["StrRefColList"]);
                    var strColumns = strRefColList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var strColumn in strColumns)
                    {
                        drs = _lmdDt.Select("MetaDataFieldName='" + strColumn + "'");
                        if ((drs != null) || (drs.Length > 0))
                        {
                            fieldId = TypeUtil.ToInt(drs[0]["MetaDataFieldID"]);
                            if ((fieldId == 0) ||
                                (IsHaveFieldRightDic.ContainsKey(fieldId) && IsHaveFieldRightDic[fieldId] &&
                                 !ClientCacheModel.IsHaveFieldRigth(fieldId)))
                            {
                                blnExist = false;
                                break;
                            }
                        }
                    }
                    return blnExist;
                }
            }

            //if (fieldId == 0 || (ClientCacheData.IsFieldRightById(fieldId) && !ClientCacheData.IsHaveFieldRigth(fieldId)))
            if ((fieldId == 0) ||
                (IsHaveFieldRightDic.ContainsKey(fieldId) && IsHaveFieldRightDic[fieldId] &&
                 !ClientCacheModel.IsHaveFieldRigth(fieldId)))
                blnExist = false;

            return blnExist;
        }


        /// <summary>
        ///     设置列表格式
        /// </summary>
        private void SetListFormat()
        {
            var strShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);
            if (strShowStyle == "pivot")
            {
                //报表              

                SetPivotFormat();
                tlbPivotSetting.Visibility = BarItemVisibility.Always;
                tlbChartSetting.Visibility = BarItemVisibility.Never;

                layoutItemGrid.Visibility = LayoutVisibility.Never;
                layoutItemReport.Visibility = LayoutVisibility.Always;
                layoutItemChart.Visibility = LayoutVisibility.Never;
                //tlbSetPerPageNumber.Visibility = BarItemVisibility.Never;
                comboBoxEdit1.Visible = false;
                labelControl3.Visible = false;
            }
            else if (strShowStyle == "chart")
            {
                //图表
                if (listDataExVo.StrChartSetting == string.Empty)
                    MessageShowUtil.ShowInfo("图表尚未设置，请初始化设置");

                SetChartFormat();
                tlbPivotSetting.Visibility = BarItemVisibility.Never;
                tlbChartSetting.Visibility = BarItemVisibility.Always;

                layoutItemGrid.Visibility = LayoutVisibility.Never;
                layoutItemReport.Visibility = LayoutVisibility.Never;
                layoutItemChart.Visibility = LayoutVisibility.Always;
            }
            else
            {
                //列表方式
                SetGridFormat();
                tlbPivotSetting.Visibility = BarItemVisibility.Never;
                tlbChartSetting.Visibility = BarItemVisibility.Never;
                layoutItemGrid.Visibility = LayoutVisibility.Always;
                layoutItemReport.Visibility = LayoutVisibility.Never;
                layoutItemChart.Visibility = LayoutVisibility.Never;
                //tlbSetPerPageNumber.Visibility = BarItemVisibility.Always;
                comboBoxEdit1.Visible = true;
                labelControl3.Visible = true;
            }
        }

        /// <summary>
        ///     设置Grid格式
        /// </summary>
        private void SetGridFormat()
        {
            gridView.OptionsView.ShowGroupPanel = false;
            gridView.Columns.Clear();
            hypers.Clear();
            gridView.GroupSummary.Clear();
            gridView.FormatConditions.Clear();
            DicBluerCondition.Clear();
            strSortText = "";
            GridColumn col = null;

            var fieldName = "";
            var strCoinDecimalNum = RequestUtil.GetParameterValue("CoinDecBit");
            var strWeightDecimalNum = RequestUtil.GetParameterValue("WeightBit");
            var summaryItem = new GridGroupSummaryItemCollection(gridView);

            foreach (var listDisplay in listDisplayExList)
            {
                #region 设置列表的显示信息

                fieldName = listDisplay.StrListDisplayFieldName;

                //判断字段权限
                //if (!IsHaveFieldRight(listDisplay.IsCalcField, fieldName))
                //    continue;
                col = new GridColumn();
                col.FieldName = fieldName;
                col.Caption = listDisplay.StrListDisplayFieldNameCHS;
                col.Width = listDisplay.LngDisplayWidth;
                col.OptionsColumn.ReadOnly = true;
                col.OptionsColumn.ShowInCustomizationForm = listDisplay.LngRowIndex > -1;
                if (listDisplay.LngHyperLinkType > 0)
                {
                    var hyperlink = new RepositoryItemHyperLinkEdit();
                    hyperlink.Name = "hyperlink";
                    hyperlink.SingleClick = true;
                    hyperlink.OpenLink += hyperlink_OpenLink;
                    col.Tag = listDisplay;
                    col.ColumnEdit = hyperlink;
                    col.OptionsColumn.ReadOnly = false;
                    hypers.Add(hyperlink);
                }
                else
                {
                    col.OptionsColumn.AllowEdit = false;
                }

                if (listDisplay.BlnMerge)
                {
                    if (!gridView.OptionsView.AllowCellMerge)
                        gridView.OptionsView.AllowCellMerge = true;
                    col.OptionsColumn.AllowMerge = DefaultBoolean.True;
                }
                else
                {
                    col.OptionsColumn.AllowMerge = DefaultBoolean.False;
                }

                //设置单元格可换行显示
                if (col.ColumnEdit == null)
                {
                    var rime = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
                    col.ColumnEdit = rime;
                }

                gridView.Columns.Add(col);

                gridView.Columns[fieldName].Visible = listDisplay.LngRowIndex > -1;

                #endregion

                if (listDataExVo.BlnSmlSum && listDisplay.BlnSummary)
                {
                    if (listDataExVo.LngSmlSumType <= 0)
                    {
                        gridView.Columns[fieldName].SummaryItem.SummaryType = SummaryItemType.Sum;
                        gridView.Columns[fieldName].SummaryItem.DisplayFormat = "{0:n2}";
                    }
                    else
                    {
                        gridView.Columns[fieldName].SummaryItem.SummaryType =
                            (SummaryItemType)(listDataExVo.LngSmlSumType - 1);
                        gridView.Columns[fieldName].SummaryItem.DisplayFormat = "{0:n2}";
                    }

                    summaryItem.Add(gridView.Columns[fieldName].SummaryItem.SummaryType, fieldName,
                        gridView.Columns[fieldName], gridView.Columns[fieldName].SummaryItem.DisplayFormat);
                    gridView.GroupSummary.Assign(summaryItem);
                    gridView.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Count, col.FieldName, null,
                        "(记录: 总行数 {0:n0})"));
                }


                //设置列格式
                if (!string.IsNullOrEmpty(listDisplay.StrSummaryDisplayFormat))
                    SetGridColumnFormat(gridView.Columns[fieldName],
                        TypeUtil.ToString(listDisplay.StrSummaryDisplayFormat), strCoinDecimalNum, strWeightDecimalNum);

                //条件格式设置
                if (!string.IsNullOrEmpty(listDisplay.StrConditionFormat) && (listDisplay.LngApplyType != 3))
                    SetGridColumnConditionFormat(gridView.Columns[fieldName], listDisplay.StrConditionFormat);

                //模糊查询设置
                if (!string.IsNullOrEmpty(listDisplay.StrBluerCondition))
                    SetGridColumnBluerFormat(fieldName, listDisplay.StrBluerCondition);

                //设置编码顺序
                SetGridColumnSort(listDisplay.StrListDisplayFieldName);
            }

            if (listDataExVo.BlnSmlSum)
                gridView.OptionsView.ShowFooter = true;

            SetGridColumnFormat();
        }

        /// <summary>
        ///     初始化列表数据
        /// </summary>
        private void InitListData()
        {
            try
            {
                //PerPageRecord = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
                PerPageRecord = TypeUtil.ToInt(comboBoxEdit1.Text);

                // 20171227
                //if ((PerPageRecord == 0) || (PerPageRecord > 10000)) PerPageRecord = 10000;
                if ((PerPageRecord == 0) || (PerPageRecord > 50000)) PerPageRecord = 50000;

                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                TotalRecord = Model.GetTotalRecord(strListSql);


                TotalPage = TotalRecord / PerPageRecord;
                if (TotalRecord % PerPageRecord > 0) TotalPage++;
                CurrentPageNumber = 1;

                if (TotalPage <= 1)
                    dt = Model.GetDataTable(strListSql);
                else
                    dt = Model.GetPageData(strListSql, 1, PerPageRecord);
                BandingData();
                SetListPageInfo();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private string GetSortWhere()
        {
            //var strSortWhere = (listlayount != null) && (listlayount.Count > 0)
            //    ? " and " + listlayount[0].StrConTextCondition
            //    : "";
            string strSortWhere = "";
            if (listlayount != null && listlayount.Count > 0)
            {
                var listDataLay = listlayount.FirstOrDefault(a => a.ListDataLayoutID.ToString() == lookUpEditArrangeTime.EditValue.ToString());
                if (listDataLay != null) strSortWhere = " and " + listDataLay.StrConTextCondition;
            }
            return strSortWhere;
        }

        /// <summary>
        ///     刷新列表中的数据
        /// </summary>
        private void RefreshListData()
        {
            try
            {
                OpenWaitDialog("正在刷新数据");

                SetExecuteBeginTime();

                if (_freQryCondition != null)
                {
                    _strFreQryCondition = _freQryCondition.GetFreQryCondition();
                    _strFreQryConditionByChs = _freQryCondition.GetFreQryCondtionByChs();
                    _listdate = _freQryCondition.GetDayDate();
                    _dicConditionOldTable = _freQryCondition.GetConditionOldTable();
                }

                //如果没有选择查询日期,则提示其选择查询日期
                var dtMetadata = ClientCacheModel.GetServerMetaData(listExvo.MainMetaDataID);
                var rows = dtMetadata.Select("blnDay=1");
                //判断主元数是不是日表，如果是日表，现在默认认为这里面不管有多少个日期字段，反正只存了当天的数据，所以暂时不用判断是哪个元数据字段做为日表日期条件字段
                if ((rows != null) && (rows.Length > 0))
                    if ((_strFreQryCondition.Contains("1900-") || _strFreQryCondition.Contains("9999-")) &&
                        _strFreQryCondition.ToLower().Contains("datsearch"))
                    {
                        MessageShowUtil.ShowMsg("请选择查询时间！", true, barStaticItemMsg);
                        return;
                    }
                if ((_listdate != null) && (_listdate.Count > 92))
                {
                    //2016-10-24  ,增加if判断，因为抽放报表月表，年表不是日表，但是又是虚拟的日表,所以不给_listdate赋值
                    var dtMetadataA = ClientCacheModel.GetServerMetaData(listExvo.MainMetaDataID);
                    var rowsA = dtMetadataA.Select("blnDay=1");
                    if ((rowsA == null) || (rowsA.Length == 0)) return;
                    var strDayType = TypeUtil.ToString(rowsA[0]["strDayType"]);
                    if (strDayType != "")
                    {
                        MessageShowUtil.ShowMsg("最多只能查询三个月数据！", true, barStaticItemMsg);
                        return;
                    }
                }

                if (listExvo.ListID == 68 || listExvo.ListID == 34 || listExvo.ListID == 19)//开关量断电、馈电异常时间限制
                {
                    if (_listdate.Count > 1)
                    {
                        DateTime stime, etime;
                        DateTime.TryParse(_listdate[0].ToString().Substring(0, 4) + "-" + _listdate[0].ToString().Substring(4, 2) + "-" + _listdate[0].ToString().Substring(6, 2), out stime);
                        DateTime.TryParse(_listdate[_listdate.Count - 1].ToString().Substring(0, 4) + "-" + _listdate[_listdate.Count - 1].ToString().Substring(4, 2) + "-" + _listdate[_listdate.Count - 1].ToString().Substring(6, 2), out etime);
                        TimeSpan ts = etime - stime;
                        if (ts.TotalDays > 7)
                        {
                            XtraMessageBox.Show("最多只能查询7天的数据！");
                            return;
                        }
                    }
                }

                SetDayTableSql();

                // 20180306
                // 20180227
                // 20170830
                if ((_listdate != null) && (_listdate.Count > 0))
                {
                    //此if用于测点编排，因为测点编排是按天建立的

                    if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
                    {
                        var strdate =
                            _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                        _strFreQryCondition = _strFreQryCondition.Replace(strdate, " <> '1900-1-1'");
                    }
                }

                if (radioButtonArrangePoint.Checked)
                {
                    _strSortCondtion = GetSortWhere();
                }
                else
                {
                    _strSortCondtion = "";
                }

                //2015-10-26   将查询条件不放在sql语句的最后，而是放到关联表的前面,以提前查询效率(密采记录这张报表在这里体现得比较大)

                #region

                var StrListSQL = strOldStrListSql;

                foreach (var str in _dicConditionOldTable.Keys)
                {
                    rows = dtMetadata.Select("strTableName='" + str + "'");
                    if (rows.Length > 0) //如果输入的查询条件是主元数据的情况才把条件放在关联表之前，因为不是主元数据可能有多个，如一个表可能关联多个枚举，如果不控制，会把关联的枚举表全部加上条件
                        StrListSQL = StrListSQL.Replace("from " + str,
                            "from " + str + " where " + _dicConditionOldTable[str]);
                }
                if (StrListSQL.Length > 0)
                    listDataExVo.StrListSQL = StrListSQL;

                #endregion

                if (PerPageRecord == 0)
                {
                    InitListData();
                    MessageShowUtil.ShowStaticInfo("加载数据执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
                    return;
                }

                //计算总行数 
                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                TotalRecord = Model.GetTotalRecord(strListSql);

                //计算总页数
                TotalPage = TotalRecord / PerPageRecord;
                if (TotalRecord % PerPageRecord > 0) TotalPage++;

                var rowHandle = gridView.FocusedRowHandle; //得到当前所选择的行
                tlbGoToPage_ItemClick(null, null); //执行刷新
                var totalRow = gridView.RowCount; //得到当前页的总行数              

                if (totalRow > 0)
                    if (rowHandle <= totalRow)
                        gridView.FocusedRowHandle = rowHandle;
                    else
                        gridView.FocusedRowHandle = totalRow - 1;
                if ((CurrentPageNumber > 0) && (totalRow == 0))
                    tlbGoToPage_ItemClick(null, null); //执行刷新

                MessageShowUtil.ShowStaticInfo("加载数据执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                CloseWaitDialog();
            }
        }

        /// <summary>
        ///     初始化常用查询条件
        /// </summary>
        private void InitFreQryCondition()
        {
            var strFreWhere = ""; //常用查询条件
            if (_lmdDt == null)
                _lmdDt = Model.GetListMetaData(ListDataID, listDataExVo.UserID);
            if (_lmdDt != null)
            {
                var dt = _lmdDt.Copy();
                dt.DefaultView.RowFilter = "blnFreCondition=1 and isnull(strFreCondition,'')<>''";
                dt.DefaultView.Sort = "lngFreConIndex asc";
                dt = dt.DefaultView.ToTable();
                var count = dt.Rows.Count;
                var strFieldName = "";
                var strFieldType = "";
                var strFkCode = "";
                var strFreCondition = "";
                DataRow curDr = null;
                for (var i = 0; i < count; i++)
                {
                    curDr = dt.Rows[i];
                    strFieldName = TypeUtil.ToString(curDr["MetaDataFieldName"]);
                    strFieldType = TypeUtil.ToString(curDr["strFieldType"]);
                    strFkCode = TypeUtil.ToString(curDr["strFkCode"]);
                    strFreCondition = TypeUtil.ToString(curDr["strFreCondition"]);

                    var str = "";
                    if (strFreCondition != string.Empty)
                    {
                        var strfilename = strFieldName;
                        var index = strfilename.LastIndexOf("_");
                        if (index > -1)
                            strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");

                        if (strFkCode != string.Empty)
                            str = BulidConditionUtil.GetRefCondition(strfilename, strFreCondition, strFieldType);
                        else
                            str = BulidConditionUtil.GetConditionString(strfilename, strFieldType, strFreCondition);

                        //由于项目中表名有下画线，所以要取最后一个下画线  2014-11-26 
                        //str = BulidConditionUtil.GetConditionString(strFieldName.Replace("_", "."), strFieldType, strFreCondition);

                        if (str != string.Empty)
                            strFreWhere += " and " + str;
                    }
                }
            }

            _strFreQryCondition = strFreWhere;
        }

        /// <summary>
        ///     设置列表的行号
        /// </summary>
        private void gridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        /// <summary>
        ///     绑定数据
        /// </summary>
        private void BandingData()
        {
            try
            {
                if (dt != null)
                {
                    //修改日期为“1900-01-01”为Null


                    var rowCount = dt.Rows.Count;

                    if (listExvo.StrListCode == "MLLKDDay")
                    {
                        // dt = TypeUtil.DeleteRepeateRow(dt);
                    }

                    // 模糊显示，如将金额>1000的显示为*
                    foreach (var s in DicBluerCondition.Keys)
                    {
                        var ss = DicBluerCondition[s].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        DataRow[] rows = null;
                        try
                        {
                            rows = dt.Select(ss[1]);
                            foreach (var row in rows)
                                try
                                {
                                    row[s] = ss[0];
                                }
                                catch
                                {
                                    //2017-04-07  ,解决抽放报表合计行显示时间问题
                                    row[s] = DBNull.Value;
                                }
                        }
                        catch (Exception ex)
                        {
                            MessageShowUtil.ShowInfo("模糊条件设置有误，请在方案里面查看!原因为" + ex.Message);
                            break;
                        }
                    }

                    // 20170830
                    //根据编排的测点从新排序，从BFT_ListDataLayount表读取测点信息
                    if (radioButtonArrangePoint.Checked)
                    {
                        if ((listlayount != null) && (listlayount.Count > 0))
                        {
                            var listDataLay = listlayount.FirstOrDefault(a => a.StrDate == lookUpEditArrangeTime.Text);
                            if (listDataLay != null)
                            {
                                strSortText = TypeUtil.ToString(listDataLay.StrCondition);
                                strSortText = strSortText.Substring(5);
                                strSortFileName = listDataLay.StrFileName;
                                var t = dt.Clone();
                                t.Clear();
                                var str = strSortText.Split(',');
                                foreach (var s in str)
                                {
                                    var rowss = dt.Select(strSortFileName + "=" + s + "");
                                    foreach (var row in rowss)
                                        t.Rows.Add(row.ItemArray);
                                }
                                dt = t;
                            }
                        }
                    }


                    #region //忽悠安装位置变化处理,如果勾选了忽略安装位置变化才进入以下代码

                    if (chkblnKeyWord.Checked)
                    {
                        var RowsblnKeyWord = _lmdDt.Select("blnKeyWord=1");
                        var RowsGroupBy = _lmdDt.Select("lngKeyGroup>0");
                        var d = dt.Clone();
                        if (RowsblnKeyWord.Length > 0)
                        {
                            IDictionary<object, object> dics = new Dictionary<object, object>();
                            foreach (DataRow row in dt.Rows)
                            {
                                //循环数据源的行数，将相同的KeyWord(如Point字段)存在key里
                                var strFileNameJia = "";
                                var strFileNames = "";
                                foreach (var rowkey in RowsblnKeyWord)
                                {
                                    var strFileName = TypeUtil.ToString(rowkey["MetaDataFieldName"]);
                                    var strvalue = TypeUtil.ToString(row[strFileName]);
                                    strFileNames += strFileName;
                                    strFileNameJia += strvalue;
                                }
                                if (!dics.ContainsKey(strFileNameJia))
                                    dics.Add(strFileNameJia, strFileNames);
                            }

                            foreach (var o in dics.Keys)
                            {
                                //根据相同的keyword，得到这个keyword的行集合进行计算
                                var rowselect = dt.Select(dics[o] + "='" + o + "'");
                                if (rowselect.Length > 1)
                                {
                                    //如果根据keyword筛选出来有两条以上记录,表示做了安装位置或者设备类型的变化，才需要做计算
                                    for (var i = 1; i < rowselect.Length; i++)
                                        foreach (var rowkeygroup in RowsGroupBy)
                                        {
                                            //得到第一行的值，计算的时候把计算结果写到第一行去
                                            var strValueOneRow =
                                                rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()];
                                            //得到合并后汇总类型(1计数,2汇总,3平均,4最大,5最小)
                                            var lngkeygrouptype = TypeUtil.ToInt(rowkeygroup["lngKeyGroup"]);
                                            //得到当时行的值
                                            var strValue = rowselect[i][rowkeygroup["MetaDataFieldName"].ToString()];
                                            var strFileType = TypeUtil.ToString(rowkeygroup["strFieldType"]);
                                            if (lngkeygrouptype == 2)
                                                if (strFileType.Contains("char"))
                                                {
                                                    //如果是字符串类型，则直接转化为日期进行判断
                                                    var time1 = TypeUtil.ToDateTime(strValueOneRow);
                                                    var time2 = TypeUtil.ToDateTime(strValue);
                                                    var span1 = new TimeSpan(time1.Hour, time1.Minute, time1.Second);
                                                    var span2 = new TimeSpan(time2.Hour, time2.Minute, time2.Second);
                                                    var sum = span1 + span2;
                                                    rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()] =
                                                        sum.ToString();
                                                }
                                                else
                                                {
                                                    rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()] =
                                                        TypeUtil.ToDecimal(strValueOneRow) +
                                                        TypeUtil.ToDecimal(strValue);
                                                }
                                            if (lngkeygrouptype == 3)
                                            {
                                                rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()] =
                                                    TypeUtil.ToDecimal(strValueOneRow) + TypeUtil.ToDecimal(strValue);
                                                if (i == rowselect.Length - 1)
                                                    rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()] =
                                                        Math.Round(
                                                            TypeUtil.ToDecimal(
                                                                rowselect[0][rowkeygroup["MetaDataFieldName"].ToString()
                                                                ]) / rowselect.Length, 2, MidpointRounding.AwayFromZero);
                                            }
                                        }
                                    d.Rows.Add(rowselect[0].ItemArray);
                                }
                                else
                                {
                                    foreach (var row in rowselect)
                                        d.Rows.Add(row.ItemArray);
                                }
                            }
                            dt = d;
                        }
                    }

                    #endregion
                }

                BandingDataToControl();
                if (dt != null)
                    //barStaticItemRowCount.Caption = "共计" + TotalRecord + "条记录。";
                    labelControl2.Text = "共" + TotalRecord + "条记录";
            }
            catch (OutOfMemoryException)
            {
                throw new Exception("显示数据量过大,内存溢出,请减少每页显示行数");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     绑定数据
        /// </summary>
        private void BandingDataToControl()
        {
            try
            {
                if (dt != null)
                    dt = TypeUtil.AddNumberToDataTable(dt); //在数据源里添加一列序号列，用于报表显示的时候有序号列
                var strShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);
                if (strShowStyle == "pivot")
                {
                    //报表                     
                    //DataSet dataset = new DataSet("ywdataset");
                    //dataset.Tables.Clear();
                    //dataset.Tables.Add(dt);   
                    if (dt == null) return;
                    dt.TableName = "dtMaster";
                    ReportDataBinding();

                    //加载条件汉字
                    var lblConditon = rp.FindControl("lblConditon", true) as XRLabel;
                    if (lblConditon != null)
                        lblConditon.Text = _strFreQryConditionByChs;

                    //加载备注
                    var xrrich = rp.FindControl("txtDesc", true) as XRRichText;
                    if (xrrich != null)
                    {
                        //xrrich.Text = listDataExVo.StrListSrcSQL;
                        DateTime dtRemark = GetRemarkTime();
                        var listDataRemarkInfo = Model.GetListDataRemarkByTimeListDataId(dtRemark, listDataExVo.ListDataID);

                        if (listDataRemarkInfo == null)
                        {
                            xrrich.Text = "";
                        }
                        else
                        {
                            xrrich.Text = listDataRemarkInfo.Remark;
                        }
                    }

                    documentViewer1.DocumentSource = rp;
                    documentViewer1.PrintingSystem = rp.PrintingSystem;

                    rp.CreateDocument();
                }
                else if (strShowStyle == "chart")
                {
                    //图表

                    chartControl.DataSource = dt;
                }
                else
                {
                    //列表方式
                    SwitchingValueStateChangeDataAmend();
                    gridControl.DataSource = null;
                    gridControl.DataSource = dt;

                    InitGridStyleConfig();
                }
                if (dt != null)
                    //barStaticItemRowCount.Caption = "共计" + TotalRecord + "条记录。";
                    labelControl2.Text = "共" + TotalRecord + "条记录";
            }
            catch (OutOfMemoryException)
            {
                throw new Exception("显示数据量过大,内存溢出,请减少每页显示行数");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void hyperlink_OpenLink(object sender, OpenLinkEventArgs e)
        {
            try
            {
                if (
                    string.IsNullOrEmpty(
                        TypeUtil.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, gridView.FocusedColumn))))
                    return;
                ListdisplayexInfo listDisplayEx = null;

                if (gridView.FocusedColumn.Tag != null)
                    listDisplayEx = (ListdisplayexInfo)gridView.FocusedColumn.Tag;
                if ((listDisplayEx == null) || (listDisplayEx.LngHyperLinkType <= 0))
                    return;


                var rowHandle = gridView.FocusedRowHandle;
                var lngHyperLinkType = listDisplayEx.LngHyperLinkType;
                var strRequest = listDisplayEx.StrHyperlink;
                var strParaColName = listDisplayEx.StrParaColName;

                var fieldName = "";
                var fieldValue = "";
                string[] strParaArr;
                if (lngHyperLinkType == 1)
                {
                    //卡片
                    strParaArr = strParaColName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var genericEntityName = GetGenericEntityName(rowHandle);
                    if (genericEntityName != string.Empty)
                        strRequest = strRequest.Replace("GenericEntity", genericEntityName);
                    foreach (var strPara in strParaArr)
                    {
                        fieldName = strPara.Remove(0, strPara.IndexOf('_') + 1);
                        fieldValue = TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, gridView.Columns[strPara]));
                        strRequest = strRequest.Replace("${" + fieldName + "}", fieldValue);
                    }

                    //卡片
                    var cmd = strRequest.Split(';');
                    if (cmd.Length == 2)
                        strRequest = cmd[1].Replace(",", "");
                }
                else if (lngHyperLinkType == 2)
                {
                    //列表
                    strParaArr = strParaColName.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                    var strProvide = string.Empty;
                    var strReceive = string.Empty;
                    var lngProvideType = 0;
                    IDictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("SourceIsList", "true");
                    var strKey = "";
                    var strDisplay = "";
                    foreach (var strPara in strParaArr)
                    {
                        strReceive = strPara.Remove(strPara.LastIndexOf('='));
                        strProvide = strPara.Substring(strPara.LastIndexOf('=') + 1);
                        if ((strProvide.Length > 2) && ('$' == strProvide[strProvide.Length - 2]))
                        {
                            //兼容历史数据

                            lngProvideType = TypeUtil.ToInt(strProvide.Substring(strProvide.Length - 1));
                            strProvide = strProvide.Remove(strProvide.Length - 2);
                        }
                        if (1 == lngProvideType)
                        {
                            //常用条件

                            GetFreQryConditionByField(strProvide, ref strKey, ref strDisplay);
                        }
                        else
                        {
                            //默认行数据作为条件
                            fieldValue =
                                TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, gridView.Columns[strProvide]));
                            strKey = "等于&&$" + fieldValue;
                            var drs = _lmdDt.Select("MetaDataFieldName='" + strProvide + "'");
                            if ((drs.Length > 0) && (TypeUtil.ToString(drs[0]["strFkCode"]) != string.Empty))
                                strDisplay = "等于&&$(" + fieldValue + ")";
                            else
                                strDisplay = strKey;
                        }

                        if (strKey != string.Empty)
                        {
                            if (!paraDic.ContainsKey("Key_" + strReceive))
                                paraDic.Add("Key_" + strReceive, strKey);
                            if (!paraDic.ContainsKey("Display_" + strReceive))
                                paraDic.Add("Display_" + strReceive, strDisplay);
                        }
                    }

                    var cmd = strRequest.Split(';');
                    if (cmd.Length == 2)
                    {
                        strRequest = cmd[1].Replace(",", "");
                        var paras = strRequest.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < paras.Length; i++)
                        {
                            var keyValues = paras[i].Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (keyValues.Length == 2)
                            {
                                var key = keyValues[0];
                                var value = keyValues[1];
                                if (!paraDic.ContainsKey(key))
                                    paraDic.Add(key, value);
                            }
                        }


                        var frm = new frmList(TypeUtil.ToInt(paraDic["ListID"]), paraDic);
                        frm.MdiParent = MdiParent;
                        frm.Show();

                        //RequestUtil.ExcuteCommand(cmd[0], paraDic);
                    }
                }
                else if (lngHyperLinkType == 3)
                {
                    //单据

                    strParaArr = strParaColName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var strPara in strParaArr)
                        strRequest = TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, gridView.Columns[strPara]));
                    // RequestUtil.ExcuteCommand("Card", strRequest);
                }
                else
                {
                    //其它 如：网址

                    strParaArr = strParaColName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var strPara in strParaArr)
                    {
                        fieldName = strPara.Remove(0, strPara.IndexOf('_') + 1);
                        fieldValue = TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, gridView.Columns[strPara]));
                        strRequest = strRequest.Replace("${" + fieldName + "}", fieldValue);
                    }
                    Process.Start(strRequest);
                }
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private string GetGenericEntityName(int rowHandle)
        {
            var str = string.Empty;
            if ((gridView.Columns["GenericEntity"] != null) && (rowHandle >= 0))
                str = TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, "GenericEntity"));
            return str;
        }

        /// <summary>
        ///     工具栏按钮的事件
        /// </summary>
        private void button_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                gridView.CloseEditor();
                var rowHandle = gridView.FocusedRowHandle;
                var dic = new Dictionary<string, string>();
                var strbuttonTag = e.Item.Tag.ToString();
                var str = strbuttonTag.Split('&');

                var strRequestCode = str[0];
                if (strRequestCode == "")
                {
                    MessageShowUtil.ShowInfo("未配置请求库编码,请先配置！");
                    return;
                }

                var dtRequest = ClientCacheModel.GetServerRequest();
                var row = dtRequest.Select("RequestCode='" + strRequestCode + "'");
                if (row.Length == 0)
                {
                    MessageShowUtil.ShowInfo("未找到请求库编码,请确认是配置是否正确");
                    return;
                }
                var strRequest = TypeUtil.ToString(row[0]["MenuParams"]);
                if ((strRequest == "") && (str[1] != "add"))
                {
                    MessageShowUtil.ShowInfo("请求库表未配置MenuParams字段,请先配置！");
                    return;
                }
                var fieldName = "";
                var fieldValue = "";
                var strParaArr = listDataExVo.StrListDefaultField.Split(new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (var strPara in strParaArr)
                {
                    fieldName = strPara.Remove(0, strPara.LastIndexOf('_') + 1);
                    fieldValue = TypeUtil.ToString(gridView.GetRowCellValue(rowHandle, gridView.Columns[strPara]));
                    strRequest = strRequest.Replace("${" + fieldName + "}", fieldValue);
                }
                var strpar = strRequest.Split('&');
                foreach (var s in strpar)
                {
                    if (s == "") continue;
                    var sp = s.Split('=');
                    dic.Add(sp[0], sp[1]);
                }

                if ((str[1].ToLower() == "add") || (str[1].ToLower() == "modfiy") || str[1].ToLower() == "delete")
                {
                    //如果是新增或者修改，打开窗体 
                    if ((str[1].ToLower() == "modfiy") && (rowHandle < 0))
                    {
                        MessageShowUtil.ShowInfo("请选择待修改的记录！");
                        return;
                    }

                    if ((str[1].ToLower() == "delete") && (rowHandle < 0))
                    {
                        MessageShowUtil.ShowInfo("请选择待删除的记录！");
                        return;
                    }

                    Sys.Safety.ClientFramework.CBFCommon.RequestUtil.OnRefreshReportEvent += RequestUtil_OnRefreshReportEvent;
                    Sys.Safety.ClientFramework.CBFCommon.RequestUtil.ExcuteCommand(strRequestCode, dic, false);
                    RefreshListData();
                }
                else
                {
                    //if (str[1].ToLower() == "delete")
                    //    DeleteListRows(row[0]);
                    //else
                    //    ButtonClieckExtend();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void RequestUtil_OnRefreshReportEvent(object sender, object e)
        {
            RefreshListData();
            Sys.Safety.ClientFramework.CBFCommon.RequestUtil.OnRefreshReportEvent -= RequestUtil_OnRefreshReportEvent;
        }

        public virtual void ButtonClieckExtend()
        {
            throw new Exception("该方法未实现,请在子类中实现！");
        }

        private void DeleteListRows(DataRow row)
        {
            var strRowIDs = string.Empty;
            try
            {
                var strAssembly = TypeUtil.ToString(row["MenuFile"]);
                if (strAssembly == "")
                {
                    MessageShowUtil.ShowInfo("请求库表未配置MenuFilep字段,请先配置！");
                    return;
                }
                var strClassName = TypeUtil.ToString(row["MenuNamespace"]);
                if (strClassName == "")
                {
                    MessageShowUtil.ShowInfo("请求库表未配置MenuNamespace字段,请先配置！");
                    return;
                }

                var strKeyFileName = TypeUtil.ToString(row["MenuParams"]);
                strKeyFileName = strKeyFileName.Substring(0, strKeyFileName.IndexOf("="));

                strRowIDs = GetSelectListRows(strKeyFileName);
                if (strRowIDs == string.Empty)
                {
                    MessageShowUtil.ShowInfo("请选择待删除的记录！");
                    return;
                }

                var result = MessageShowUtil.ReturnDialogResult("确定要删除所选记录？");
                if (result == DialogResult.No)
                    return;

                var dtoTypeName = strClassName + "," + strAssembly;
                var dtoType = Type.GetType(dtoTypeName);
                Model.DeteteRows(strRowIDs, dtoType);
                RefreshListData();
                MessageShowUtil.ShowInfo("操作成功");
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex.InnerException);
            }
        }

        private string GetEntityIDFieldName()
        {
            var strEntityIDField = ""; //实体主键参数
            var strParaArr = listDataExVo.StrListDefaultField.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var strPara in strParaArr)
            {
                var fieldName = strPara.Remove(0, strPara.LastIndexOf('_') + 1);
                if (fieldName.ToLower() == realEntity.ToLower() + "id")
                {
                    strEntityIDField = strPara;
                    break;
                }
            }

            return strEntityIDField;
        }

        private string GetSelectListRows(string strFieldName)
        {
            var strColFieldName = "";
            var fieldName = "";
            var colCount = gridView.Columns.Count;
            for (var i = 0; i < colCount; i++)
            {
                fieldName = gridView.Columns[i].FieldName;
                fieldName = fieldName.Remove(0, fieldName.LastIndexOf('_') + 1);
                if (fieldName.ToLower() == strFieldName.ToLower())
                {
                    strColFieldName = gridView.Columns[i].FieldName;
                    break;
                }
            }

            if (strColFieldName == "")
            {
                MessageShowUtil.ShowInfo("不存在[" + strFieldName + "]字段!");
                return string.Empty;
                ;
            }


            var strRowIDs = string.Empty;
            var ArraySelectRows = gridView.GetSelectedRows();
            for (var i = 0; i < ArraySelectRows.Length; i++)
                strRowIDs = strRowIDs + TypeUtil.ToString(gridView.GetRowCellValue(ArraySelectRows[i], strColFieldName)) +
                            ",";
            if (strRowIDs != "")
                strRowIDs = strRowIDs.Remove(strRowIDs.Length - 1);

            return strRowIDs;
        }

        /// <summary>
        ///     列表双击
        /// </summary>
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (DoubleButtonItem == null)
            {
                if (gridView.FocusedRowHandle < 0)
                {
                    return;
                }
                //取得选定行信息，判断并弹出详细窗口  20170731
                string nodeName = gridView.GetRowCellValue(gridView.FocusedRowHandle, gridView.FocusedColumn).ToString();
                if (nodeName.Length > 20)//超过30个字符，弹出详细窗口
                {
                    CellDetail cellDetail = new CellDetail(nodeName);
                    cellDetail.ShowDialog();
                }
                return;
            }
            var args = new ItemClickEventArgs(DoubleButtonItem, DoubleButtonItem.Links[0]);
            button_ItemClick(null, args);
        }

        /// <summary>
        ///     刷新数据
        /// </summary>
        private void tlbRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                RefreshListData();
                blnRefreshed = true;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private string GetSqlByDeleteNull(string strsql)
        {
            var sql = strsql.Replace("\r\n", " ");
            var blnFor = true;
            while (blnFor)
            {
                sql = sql.Replace("  ", " ");
                if (!sql.Contains("  "))
                    blnFor = false;
            }
            return sql;
        }

        /// <summary>
        ///     根据元数据表(BFT_MetaData)中的字段strDayTableName来解析具体的表
        /// </summary>
        /// <param name="dtTable"></param>
        /// <returns></returns>
        public DataTable GetDayTable(DataTable dtTable)
        {
            var dtTableCopy = dtTable.Clone();
            foreach (DataRow row in dtTable.Rows)
            {
                //根据列表用的元数据，然后去看MeataData表中strDayTableName字段，主要是用于strDayTableName也是视图的情况
                var strViewName = TypeUtil.ToString(row["strTableName"]);
                var strViewSrcTableName = TypeUtil.ToString(row["strDayTableName"]).ToLower();
                if (strViewSrcTableName.Contains(";"))
                {
                    var strs = strViewSrcTableName.Split(';');
                    foreach (var ss in strs)
                    {
                        if (ss == "") continue;
                        var s = ss.Split(':');

                        // 20180309
                        //foreach (var stable in s[1].Split(','))
                        //{
                        //    var r = dtTableCopy.NewRow();
                        //    r["strName"] = "";
                        //    r["strDayTableName"] = stable;
                        //    r["strTableName"] = s[0];
                        //    dtTableCopy.Rows.Add(r);
                        //}
                        var r = dtTableCopy.NewRow();
                        r["strName"] = "";
                        r["strDayTableName"] = s[1];
                        r["strTableName"] = s[0];
                        dtTableCopy.Rows.Add(r);
                    }
                }
            }

            return dtTableCopy;
        }

        /// <summary>
        /// 修改view_mdef视图、需按时间过滤的视图、日表视图
        /// </summary>
        private void SetDayTableSql()
        {
            string sAllUpdateSql = "";
            var dtMetadata = ClientCacheModel.GetServerMetaData(listExvo.MainMetaDataID);

            #region 修改view_mdef视图

            string sDate2 = "";
            if (radioButtonActivityPoint.Checked || radioButtonArrangePoint.Checked && lookUpEditArrangeActivity.EditValue == "1")
            {
                sDate2 = "between '9999-12-31 23:59:59' and '9999-12-31 23:59:59'";
            }
            else if (radioButtonStorePoint.Checked || radioButtonArrangePoint.Checked && lookUpEditArrangeActivity.EditValue == "2")
            {
                if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)     //有查询时间
                {
                    sDate2 = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                    sDate2 = sDate2.Split(new string[] { "and" }, StringSplitOptions.None)[0] + "and" + " '9999-12-31 23:59:59'";
                }
                else        //没有查询时间
                {
                    sDate2 = "between '1900-01-01 00:00:00' and '9999-12-31 23:59:59'";
                }
            }

            var defTableName = "KJ_DeviceDefInfo";
            var strKeyFieldPropName = dtMetadata.Rows[0]["strKeyFieldPropName"];
            if (strKeyFieldPropName != null && strKeyFieldPropName.ToString() != "")
            {
                int outValue;
                var suc = int.TryParse(strKeyFieldPropName.ToString(), out outValue);
                if (!suc)
                {
                    defTableName = strKeyFieldPropName.ToString();
                }
            }

            string sAllPointView =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM `" + defTableName + "` WHERE (( `" + defTableName + "`.`DeleteTime` = '1900-01-01 00:00:00' ) OR ( `" + defTableName + "`.`DeleteTime` " +
                sDate2 + " ))";
            string sRemoveDuplicationBefore =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM " + defTableName + " AS c WHERE c.pointid IN ( SELECT ( SELECT pointid FROM " + defTableName + " AS b WHERE b.point = temp.point AND activity = 0 ORDER BY b.DeleteTime DESC LIMIT 1 ) AS point FROM viewjc_mdefsubquerybef AS temp ) UNION ALL SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM " + defTableName + " WHERE activity = 1 AND point NOT IN ( SELECT point FROM viewjc_mdefsubquerybef )";
            string sRemoveDuplicationAfter =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM " + defTableName + " AS c WHERE c.pointid IN ( SELECT ( SELECT pointid FROM " + defTableName + " AS b WHERE b.point = temp.point AND activity = 0 ORDER BY b.DeleteTime DESC LIMIT 1 ) AS point FROM viewjc_mdefsubqueryaft AS temp ) UNION ALL SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM " + defTableName + " WHERE activity = 1";
            string sDefSubSqlBef =
                "SELECT DISTINCT point FROM " + defTableName + " AS a WHERE a.activity = 0 AND a.`DeleteTime` " + sDate2;
            string sDefSubSqlAft =
                "SELECT DISTINCT point FROM " + defTableName + " AS a WHERE a.activity = 0 AND point NOT IN ( SELECT point FROM " + defTableName + " WHERE activity = 1 ) AND a.`DeleteTime` " + sDate2;

            string sDefMainUseSql = "";
            string sDefSubUseSqlBef = "";
            string sDefSubUseSqlAft = "";
            if (radioButtonStorePoint.Checked)
            {
                if (lookUpEditRemoveDuplication.EditValue == "2")
                {
                    sDefMainUseSql = sRemoveDuplicationBefore;
                    sDefSubUseSqlBef = sDefSubSqlBef;
                }
                else if (lookUpEditRemoveDuplication.EditValue == "3")
                {
                    sDefMainUseSql = sRemoveDuplicationAfter;
                    sDefSubUseSqlAft = sDefSubSqlAft;
                }
                else
                {
                    sDefMainUseSql = sAllPointView;
                }
            }
            else
            {
                sDefMainUseSql = sAllPointView;
            }

            if (Model.GetDbType() == "sqlserver")
            {
                sDefMainUseSql = "go\r\n alter view viewjc_mdef \r\n as\r\n " + sDefMainUseSql;
                if (!string.IsNullOrEmpty(sDefSubUseSqlBef))
                {
                    sDefSubUseSqlBef = "go\r\n alter view viewjc_mdefsubquerybef \r\n as\r\n " + sDefSubUseSqlBef;
                }
                if (!string.IsNullOrEmpty(sDefSubUseSqlAft))
                {
                    sDefSubUseSqlAft = "go\r\n alter view viewjc_mdefsubqueryaft \r\n as\r\n " + sDefSubUseSqlAft;
                }
            }
            if (Model.GetDbType() == "mysql")
            {
                sDefMainUseSql = "alter view viewjc_mdef \r\n as\r\n " + sDefMainUseSql;
                if (!string.IsNullOrEmpty(sDefSubUseSqlBef))
                {
                    sDefSubUseSqlBef = ";alter view viewjc_mdefsubquerybef \r\n as\r\n " + sDefSubUseSqlBef;
                }
                if (!string.IsNullOrEmpty(sDefSubUseSqlAft))
                {
                    sDefSubUseSqlAft = ";alter view viewjc_mdefsubqueryaft \r\n as\r\n " + sDefSubUseSqlAft;
                }
            }

            sAllUpdateSql += sDefMainUseSql + sDefSubUseSqlBef + sDefSubUseSqlAft;

            #endregion

            #region 修改日表视图
            var strWhereBySumReport = "";

            if (_listdate != null)      //如果没有选择日期,那么将不重新组织sql
            {
                var rows = dtMetadata.Select("blnDay=1");
                //判断主元数是不是日表，如果是日表，现在默认认为这里面不管有多少个日期字段，反正只存了当天的数据，所以暂时不用判断是哪个元数据字段做为日表日期条件字段
                if (rows != null && rows.Length != 0)
                {
                    var strDayType = TypeUtil.ToString(rows[0]["strDayType"]);
                    if (strDayType.ToLower() == "yyyymm")
                    {
                        //如果是月表,要按照月表的格式来组织sql
                        var _listdatecopy = new List<string>();
                        foreach (var strdate in _listdate)
                        {
                            var s = strdate.Substring(0, 6);
                            if (!_listdatecopy.Contains(s))
                                _listdatecopy.Add(s);
                        }
                        _listdate = _listdatecopy;
                    }

                    var strupdatesql = "";
                    if (TypeUtil.ToString(rows[0]["strSrcType"]) == "V")
                    {
                        //如果是日表，并且建立的是视图,则需要动态修改视图的sql//                
                        var dtTable = GetDayTable(dtMetadata);

                        // 20170916
                        if (dtTable.Rows.Count != 0)
                        {
                            var strDayTable = "";
                            foreach (DataRow row in dtTable.Rows)
                                strDayTable += "'" + row["strTableName"] + "',";
                            strDayTable = strDayTable.Substring(0, strDayTable.Length - 1);
                            DataTable dt = null;
                            //得到视图的创建sql 
                            if (Model.GetDbType() == "sqlserver") //需要想办法支持sql同时查出多个视图的脚本
                                dt =
                                    Model.GetDataTable(
                                        "select name as TABLE_NAME,text as Text from sys.views  left join syscomments on sys.views.object_id=syscomments.id where name in(" +
                                        strDayTable + ")");
                            if (Model.GetDbType() == "mysql")
                                dt =
                                    Model.GetDataTable(
                                        "SELECT TABLE_NAME,VIEW_DEFINITION as Text FROM information_schema.VIEWS where TABLE_NAME in(" +
                                        strDayTable + ") and TABLE_SCHEMA='" + Model.GetDBName() + "'");

                            foreach (DataRow row in dtTable.Rows)
                            {
                                var strViewName = row["strTableName"].ToString();
                                var strViewSrcTableName = row["strDayTableName"].ToString();
                                var rowscript = dt.Select("TABLE_NAME='" + strViewName + "'");
                                var strsql = "";
                                for (var i = 0; i < rowscript.Length; i++)
                                {
                                    //dt里面存的是创建视图脚本，循环得到脚本

                                    var strvalue = TypeUtil.ToString(rowscript[i]["Text"]);
                                    if (Model.GetDbType() == "sqlserver")
                                        strvalue = strvalue.Substring(strvalue.ToLower().IndexOf("as") + 2);

                                    strsql += strvalue.ToLower();
                                    if (strsql.ToLower().Contains("union "))
                                    {
                                        strsql = strsql.Substring(0, strsql.IndexOf("union"));
                                        break;
                                    }
                                }

                                if (Model.GetDbType() == "sqlserver")
                                    strupdatesql += "go\r\n alter view " + strViewName + " \r\n as\r\n ";
                                if (Model.GetDbType() == "mysql")
                                    strupdatesql += ";alter view " + strViewName + " \r\n as\r\n ";
                                var k = 0;

                                if (strDayType == "")
                                    //2016-10-21 ，由于抽放报表日，月，年没有分表，但是jc_ll_dmonthmax视图需要做时间where，所以需要统一修改jc_ll_dmonth的时间，相当于是虚拟日表
                                    strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
                                else
                                    foreach (var s in _listdate)
                                    {
                                        var strSrcTableNames = strViewSrcTableName.Split(',');

                                        // 20180312
                                        //判断是否存在表
                                        //if ((_listdate.Count > 1) && !Model.blnExistsTable(strViewSrcTableName + s))
                                        //    //如果日期段大于1天，且选择的日期在数据库中不存在，且直接跳过此表
                                        //    continue;
                                        var lisTables = new List<string>();
                                        foreach (var item in strSrcTableNames)
                                        {
                                            lisTables.Add(item + s);
                                        }

                                        if ((_listdate.Count > 1) && !Model.IfExistTables(lisTables))
                                            continue;

                                        foreach (var strSrcTableName in strSrcTableNames)
                                        {
                                            var strname = strsql.Substring(
                                                strsql.IndexOf(strSrcTableName) + strSrcTableName.Length, strDayType.Length);
                                            if (TypeUtil.ToInt(strname) > 0)
                                            {
                                                var strDataBaseTable = strSrcTableName + s;
                                                if ((_listdate.Count == 1) && !Model.blnExistsTable(strDataBaseTable))
                                                {
                                                    gridControl.DataSource = null;
                                                    throw new Exception("无满足条件数据！");
                                                }
                                                strsql = strsql.Replace(strSrcTableName + strname, strSrcTableName + s + "");
                                            }
                                            else
                                            {
                                                strsql = strsql.Replace(strSrcTableName, strSrcTableName + s + "");
                                            }
                                        }

                                        strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
                                    }

                                if (strupdatesql.Contains("union all"))
                                {
                                    strupdatesql = strupdatesql.Substring(0, strupdatesql.Length - 12);
                                    if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
                                    {
                                        //得到查询日期的日期字符串
                                        //var strdate = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                                        var strdate = _freQryCondition.GetFreQryCondition().Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                                        var strsqldate = "";
                                        var strsqlCreateView = strupdatesql.ToLower().Substring(0, strupdatesql.ToLower().IndexOf("as") + 2) + "\r\n";
                                        var strselectsql = strupdatesql.ToLower().Substring(strupdatesql.ToLower().IndexOf("as") + 2);
                                        strselectsql = GetSqlByDeleteNull(strselectsql);

                                        var strSqlArray = TypeUtil.GetSubStrCountInStr(strselectsql, "between", 0);
                                        foreach (var startindex in strSqlArray)
                                        {
                                            strsqldate = strselectsql.Substring(startindex, 55);
                                            strupdatesql = strsqlCreateView + strselectsql.Replace(strsqldate, strdate);
                                        }
                                    }

                                    #region

                                    ////2015-09-08  处理设备状态选择后影响累计次数等累计问题

                                    //if ((_strFreQryCondition.ToLower().IndexOf("state in") > 0) ||
                                    //    (strupdatesql.Replace("`", "").ToLower().IndexOf(".state") > 0) ||
                                    //    (strupdatesql.Replace("`", "").ToLower().IndexOf(", state") > 0))
                                    //{
                                    //    //此if处理设备状态字段,现在设备状态要做group by的where条件,如在b表报警type为10，但是如果state字段有21，24两种，那么如果用户只选择21的state，那么报警次数也只能只有21的数据,同时报警次数也会减少
                                    //    strupdatesql = strupdatesql.Replace("`", "");
                                    //    var strUIDevStatus = " state <> 12345"; //旭果用户没有选择任凭设备状态,那么赋一个默认值，用于好组织sql
                                    //    var endindex = 0;
                                    //    if (_strFreQryCondition.ToLower().Contains("state in"))
                                    //    {
                                    //        //将常用条件中设备状态条件提取出来
                                    //        var strUIstaus =
                                    //            _strFreQryCondition.ToLower()
                                    //                .Substring(_strFreQryCondition.ToLower().IndexOf("state in"));
                                    //        endindex = strUIstaus.IndexOf(")");
                                    //        strUIDevStatus = strUIstaus.Substring(0, endindex + 1);
                                    //    }

                                    //    var strselectsql = strupdatesql.ToLower()
                                    //        .Substring(strupdatesql.ToLower().IndexOf("as") + 2);
                                    //    strselectsql = GetSqlByDeleteNull(strselectsql);
                                    //    if (strupdatesql.Contains("state in") || strupdatesql.Contains("state =") ||
                                    //        strupdatesql.Contains("state <> 12345"))
                                    //    {
                                    //        var strsqlstaus = "";
                                    //        if (strupdatesql.Contains("state in"))
                                    //        {
                                    //            //得到视图里面的设备状态条件
                                    //            strsqlstaus =
                                    //                strselectsql.ToLower().Substring(strselectsql.ToLower().IndexOf("state in"));
                                    //            endindex = strsqlstaus.IndexOf(")");
                                    //        }
                                    //        if (strupdatesql.Contains("state ="))
                                    //        {
                                    //            //当用户只选择一个设备状态的时候，存视图的时候mysql自动会把in改为=，所以要处理这个，我反正是醉了
                                    //            strsqlstaus =
                                    //                strselectsql.ToLower().Substring(strselectsql.ToLower().IndexOf("state ="));
                                    //            endindex = strsqlstaus.IndexOf(")") - 1;
                                    //        }
                                    //        if (strupdatesql.Contains("state <> 12345"))
                                    //        {
                                    //            //当用户一直没有选择条件的时候
                                    //            strsqlstaus =
                                    //                strselectsql.ToLower()
                                    //                    .Substring(strselectsql.ToLower().IndexOf("state <> 12345"));
                                    //            endindex = strsqlstaus.IndexOf(")") - 1;
                                    //        }
                                    //        var strsqlDevstatus = strsqlstaus.Substring(0, endindex + 1);

                                    //        var strsqlCreateView =
                                    //            strupdatesql.ToLower().Substring(0, strupdatesql.ToLower().IndexOf("as") + 2) +
                                    //            "\r\n";
                                    //        strupdatesql = strsqlCreateView + strselectsql.Replace(strsqlDevstatus, strUIDevStatus);
                                    //        //将视力中设备状态条件用界面上常用条件设备状态替换

                                    //        if ((listExvo.StrDescription == "汇总表") && (strWhereBySumReport == ""))
                                    //            strWhereBySumReport = _strFreQryCondition.Replace(strUIDevStatus, "state in(12345)");
                                    //    }
                                    //}
                                    //else
                                    //    ExecMoreSql(strupdatesql);

                                    #endregion
                                }
                                else
                                {
                                    throw new Exception("无满足条件数据！");
                                }
                            }
                        }

                        sAllUpdateSql += strupdatesql;
                    }
                }
            }

            #endregion

            if (sAllUpdateSql.Length > 0)
                ExecMoreSql(sAllUpdateSql);
            if (strWhereBySumReport != "")
                _strFreQryCondition = strWhereBySumReport;
        }

        /// <summary>
        ///     执行多条sql语句
        /// </summary>
        private void ExecMoreSql(string strupdatesql)
        {
            var strsqls = strupdatesql.Replace("go", "∷").Split('∷');
            foreach (var strsql in strsqls)
                if (Convert.ToString(strsql).Length > 10)
                    Model.ExecuteSQL(strsql);
        }

        private void tlbPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("PrintReport"))
                {
                    return;
                }

                var strShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);
                if (strShowStyle == "pivot")
                    PrintPivot();
                else if (strShowStyle == "chart")
                    PrintChart();
                else
                    lookupShowStyle.EditValue = "pivot";
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     打印Grid数据
        /// </summary>
        private void PrintGrid()
        {
            gridView.OptionsPrint.PrintHeader = true;
            gridView.OptionsPrint.PrintFooter = true;

            gridView.PrintInitialize += gridView_PrintInitialize;
            gridView.ShowRibbonPrintPreview();
            gridView.PrintInitialize -= gridView_PrintInitialize;
        }

        private void gridView_PrintInitialize(object sender, PrintInitializeEventArgs e)
        {
            //如果打印的时候加了表头，那么导出的时候也会有
            //PageHeaderFooter phf = e.Link.PageHeaderFooter as PageHeaderFooter;
            //phf.Header.Content.Clear();
            //phf.Header.Content.AddRange(new string[] { "", this.lblTile.Text, "" });
            //phf.Header.LineAlignment = BrickAlignment.Center;
        }

        /// <summary>
        ///     打印分析表数据
        /// </summary>
        private void PrintPivot()
        {
            rp.PrintDialog();
            //rp.ShowRibbonPreview();
        }

        /// <summary>
        ///     打印图表数据
        /// </summary>
        private void PrintChart()
        {
            chartControl.ShowPrintPreview();
        }

        private void tlbDesign_Popup(object sender, EventArgs e)
        {
            try
            {
                if (xapp == null) return;
                tlbDesign.Reset();
                BarCheckItem item;
                foreach (var o in xapp.FormatNames)
                {
                    item = new BarCheckItem(barManager, false);
                    item.Caption = o.ToString();
                    item.Name = o.ToString();
                    item.ItemClick += item_ItemClick;
                    menuListStyle.AddItems(new BarItem[] { item });
                }
                SetListStyle(strListStyle);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetListStyle(string strStyleName)
        {
            try
            {
                if (string.IsNullOrEmpty(strStyleName))
                    strStyleName = "绿夏";
                var itemCount = menuListStyle.ItemLinks.Count;
                for (var i = 0; i < itemCount; i++)
                    if (menuListStyle.ItemLinks[i].Item.Caption == strStyleName)
                    {
                        ((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked = true;
                        xapp.LoadScheme(strStyleName, gridView);
                        strListStyle = strStyleName;
                        break;
                    }
                for (var i = 0; i < itemCount; i++)
                    if (((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked &&
                        (menuListStyle.ItemLinks[i].Item.Caption != strStyleName))
                    {
                        ((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked = false;
                        break;
                    }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void item_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (((BarCheckItem)e.Item).Checked)
                {
                    xapp.LoadScheme(e.Item.Caption, gridView);
                    strListStyle = e.Item.Caption;
                    var itemCount = menuListStyle.ItemLinks.Count;
                    for (var i = 0; i < itemCount; i++)
                        if (((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked &&
                            (menuListStyle.ItemLinks[i].Item.Caption != e.Item.Caption))
                        {
                            ((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked = false;
                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     设置全选
        /// </summary>
        private void tlbMultiSelection_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            var rowCount = gridView.RowCount;
            var rowChecked = false;
            rowChecked = tlbMultiSelection.Checked;
            for (var i = 0; i < rowCount; i++)
                gridView.SetRowCellValue(i, "MultiSelect", rowChecked);
        }

        /// <summary>
        ///     当用户设置每页所显示的行数改变时，执行刷新
        /// </summary>
        private void tlbSetPerPageNumber_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                wdf = new WaitDialogForm("正在切换中", "请等待....");
                SetExecuteBeginTime();
                if (blnRefreshed)
                {
                    //PerPageRecord = TypeUtil.ToInt(tlbSetPerPageNumber.EditValue);
                    PerPageRecord = TypeUtil.ToInt(comboBoxEdit1.Text);

                    InitListData();
                }

                MessageShowUtil.ShowStaticInfo("加载数据执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                wdf.Close();
            }
        }

        /// <summary>
        ///     点第一页按钮
        /// </summary>
        private void tlbGoToFirstPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetExecuteBeginTime();
                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(0);
                    return;
                }


                if (PerPageRecord == 0 || TotalPage == 0)
                    return;

                CurrentPageNumber = 1;

                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                dt = Model.GetPageData(strListSql, 1, PerPageRecord);


                BandingData();

                SetListPageInfo();

                MessageShowUtil.ShowStaticInfo("加载第一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     点上一页按钮
        /// </summary>
        private void tlbGoToPreviousPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetExecuteBeginTime();
                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(documentViewer1.SelectedPageIndex - 1);
                    return;
                }


                if (PerPageRecord == 0)
                    return;

                var StartRecord = (CurrentPageNumber - 1) * PerPageRecord;
                if (StartRecord > PerPageRecord - 1)
                {
                    CurrentPageNumber--;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);


                    BandingData();

                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到第一页");
                }


                MessageShowUtil.ShowStaticInfo("加载上一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     点下一页按钮
        /// </summary>
        private void tlbGoToNextPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(documentViewer1.SelectedPageIndex + 1);
                    return;
                }

                if (PerPageRecord == 0)
                    return;

                var StartRecord = CurrentPageNumber * PerPageRecord;
                if (StartRecord < TotalRecord)
                {
                    CurrentPageNumber++;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);

                    BandingData();
                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到最后一页");
                }

                MessageShowUtil.ShowStaticInfo("加载下一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     点最后一页按钮
        /// </summary>
        private void tlbGoToLastPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(rp.Pages.Count - 1);
                    return;
                }

                if (PerPageRecord == 0 || TotalPage == 0)
                    return;

                var StartRecord = (TotalPage - 1) * PerPageRecord;
                if (StartRecord < TotalRecord)
                {
                    CurrentPageNumber = TotalPage;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);

                    BandingData();

                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到最后一页");
                }

                MessageShowUtil.ShowStaticInfo("加载最后一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     点GOTO按钮
        /// </summary>
        /// <param name="sender"></param>
        private void tlbGoToPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                // 20170622
                //if (tlbSetCurrentPage.EditValue == null)
                //{
                //    return;
                //}
                if (textEdit1.EditValue == null)
                {
                    return;
                }


                //var currPage = tlbSetCurrentPage.EditValue.ToString().Split('/');
                var currPage = textEdit1.EditValue.ToString().Split('/');
                CurrentPageNumber = TypeUtil.ToInt(currPage[0]);
                if (CurrentPageNumber > TotalPage)
                    CurrentPageNumber = TotalPage;
                else if ((0 == CurrentPageNumber) && (TotalPage > 0))
                    CurrentPageNumber = 1;

                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                if (TotalPage <= 1)
                    dt = Model.GetDataTable(strListSql);
                else
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);
                BandingData();
                SetListPageInfo();
                if (lookupShowStyle.EditValue == "pivot")
                    SetReportPage(TypeUtil.ToInt(currPage[0]) - 1);
                MessageShowUtil.ShowStaticInfo("加载执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     设置显示，当页数改变时
        /// </summary>
        private void SetListPageInfo()
        {
            try
            {
                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(0);
                    return;
                }
                if (PerPageRecord == 0)
                {
                    TotalPage = 1;
                }
                else
                {
                    TotalPage = TotalRecord / PerPageRecord;
                    if (TotalRecord % PerPageRecord > 0)
                        TotalPage = TotalPage + 1;
                }
                CurrentPageText.Mask.EditMask = @"([1-9]|[1-9]\d+)/" + TotalPage;
                //tlbSetCurrentPage.EditValue = CurrentPageNumber + "/" + TotalPage;
                textEdit1.Text = CurrentPageNumber + "/" + TotalPage;
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     当用户改变页数时
        /// </summary>
        private void tlbSetCurrentPage_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //var currPageNumber = tlbSetCurrentPage.EditValue.ToString().Split('/');
                var currPageNumber = textEdit1.Text.ToString().Split('/');
                if (lookupShowStyle.EditValue == "pivot")
                {
                    if (Convert.ToInt32(currPageNumber[0]) > rp.Pages.Count)
                        //tlbSetCurrentPage.EditValue = rp.Pages.Count + "/" + rp.Pages.Count;
                        textEdit1.Text = rp.Pages.Count + "/" + rp.Pages.Count;
                    return;
                }

                if (Convert.ToInt32(currPageNumber[0]) > TotalPage)
                    //tlbSetCurrentPage.EditValue = TotalPage + "/" + TotalPage;
                    textEdit1.Text = TotalPage + "/" + TotalPage;
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     报表翻页
        /// </summary>
        private void SetReportPage(int TypeID)
        {
            if (dt == null) return;
            if (TypeID == -1)
            {
                MessageShowUtil.ShowInfo("已到第一页");
                return;
            }
            if (TypeID == rp.Pages.Count)
            {
                MessageShowUtil.ShowInfo("已到最后一页");
                return;
            }

            documentViewer1.SelectedPageIndex = TypeID;


            CurrentPageText.Mask.EditMask = @"([1-9]|[1-9]\d+)/" + rp.Pages.Count;
            //tlbSetCurrentPage.EditValue = documentViewer1.SelectedPageIndex + 1 + "/" + rp.Pages.Count;

            textEdit1.Text = documentViewer1.SelectedPageIndex + 1 + "/" + rp.Pages.Count;
        }

        /// <summary>
        ///     方案
        /// </summary>
        private void tlbSchema_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("OpenReportScheme"))
                {
                    return;
                }
                var frm = new frmSchemaDesign();
                frm.ListID = listId;
                frm.ListDataID = ListDataID;
                frm.CurListDataId = ListDataID;
                frm.MetadataId = listExvo.MainMetaDataID;
                frm.LmdDt = _lmdDt;
                frm.BlnListEnter = true;
                frm.StrListName = listExvo.StrListName;
                frm.ShowDialog();

                if (frm.BlnOk)
                {
                    //切换方案

                    var blnNew = false;
                    if (ListDataID != frm.ListDataID)
                    {
                        blnNew = true;
                        ListDataID = frm.ListDataID;
                    }
                    listDataExVo = frm.ListDataExVo;
                    _lmdDt = frm.LmdDt;
                    if (frm.ListDisplayExList != null)
                        listDisplayExList = frm.ListDisplayExList;


                    if ((listDataExVo != null) && (listDataExVo.StrDefaultShowStyle != string.Empty))
                        listTemplevo = Model.GetListTemple(listDataExVo.ListDataID);

                    SwitchSchema(blnNew);
                }

                lookupSchema.Properties.DataSource = Model.GetSchemaList(listExvo.ListID);
                lookupSchema.EditValueChanged -= lookupSchema_EditValueChanged;
                lookupSchema.EditValue = ListDataID;
                lookupSchema.EditValueChanged += lookupSchema_EditValueChanged;
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     方案选择
        /// </summary>
        private void lookupSchema_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var id = TypeUtil.ToInt(lookupSchema.EditValue);
                if (id > 0)
                {
                    //载入方案

                    ListDataID = id;
                    _lmdDt = null;
                    GetListDataEx(); //获取列表方案数据
                    GetListDisplay(); //获取列表显示数据                   
                    SwitchSchema(true);
                }

                InitializeArrangeTime();
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     切换方案
        /// </summary>
        /// <param name="blnNew">是否为新方案</param>
        private void SwitchSchema(bool blnNew)
        {
            try
            {
                OpenWaitDialog("正在切换列表数据");

                SetExecuteBeginTime();

                SetListFormat(); //根据方案设置列表格式  
                // if (!blnNew || blnFirstOpenLoadData)               
                //{


                // 20170628
                //if (blnRefreshed)
                //{
                //    InitListData();
                //}
                ////}
                //else
                //{
                //    //ClearOldSchemaData

                //    if ((dt != null) && blnNew)
                //        dt.Rows.Clear();

                //    blnRefreshed = false;
                //}

                if (chkItemFreQryCondition.Checked && (_freQryCondition != null))
                {
                    _freQryCondition.CreateControl(_lmdDt);

                    SetFrmQryConditionSize();
                }

                RefreshListData();
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                CloseWaitDialog();
            }

            MessageShowUtil.ShowStaticInfo("列表切换的时间为" + GetExecuteTimeString(), barStaticItemMsg);
        }

        /// <summary>
        ///     列表样式切换
        /// </summary>
        private void lookupShowStyle_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                wdf = new WaitDialogForm("正在切换中...", "请等待");
                var strShowStyle = string.Empty;
                if (lookupShowStyle.EditValue != null)
                {
                    SetListFormat();
                    BandingDataToControl();
                    SetListPageInfo();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                wdf.Close();
            }
        }

        /// <summary>
        ///     报表设置
        /// </summary>
        private void tlbPivotSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("OpenReportDesign"))
                {
                    return;
                }

                if (listDataExVo == null)
                {
                    MessageShowUtil.ShowInfo("请选择列表方案");
                    return;
                }

                // 20170622
                if (dt == null)
                {
                    MessageShowUtil.ShowInfo("请先查询数据。");
                    return;
                }

                if (TypeUtil.ToString(lookupShowStyle.EditValue) == "pivot")
                {
                    var strFileName = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple\\" +
                                      listDataExVo.StrListDataName + ListDataID + ".repx";
                    var frm = new frmReportDesign();
                    frm.StrFileName = strFileName;
                    frm.dtReportDataSource = dt;
                    frm.dtLmd = _lmdDt;
                    frm.listDataExDTO = listDataExVo;
                    frm.ShowDialog();
                    if (frm.blnOK)
                    {
                        //如果保存了报表模板，那么就要重新加载模板
                        listTemplevo = Model.GetListTemple(listDataExVo.ListDataID);
                        lookupShowStyle_EditValueChanged(null, null);
                    }
                }

                //需要实现
                //frmListExPivotConfig frm = new frmListExPivotConfig();
                //frm.ListDataExVo = this.listDataExVo;
                //if (this._lmdDt != null)
                //{
                //    frm.LmdDt = this._lmdDt.Copy();
                //}
                //frm.ShowDialog();

                //SetPivotFormat();
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     图表设置
        /// </summary>
        private void tlbChartSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            int a;
            try
            {
                if (listDataExVo == null)
                    MessageShowUtil.ShowInfo("请选择列表方案");

                //需要实现
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        #region 图表设置

        public void SetChartFormat()
        {
            //需要实现
        }

        #endregion

        /// <summary>
        ///     支持复制
        /// </summary>
        private void chkItemAllowCopy_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //Dictionary<string, string> point1 = new Dictionary<string, string>();
            //try
            //{
            //    string point = "001A010";
            //    if (!string.IsNullOrEmpty(point))
            //    {
            //        point1.Add("SourceIsList", "true");
            //        point1.Add("Key_viewsbrunlogreport1_point", "等于&&$" + point);
            //        point1.Add("Display_viewsbrunlogreport1_point", "等于&&$" + point);
            //    }
            //    point1.Add("ListID", "27");
            //    frmList frm = new frmList(point1);
            //    frm.ShowDialog();
            //}
            //catch (Exception ex)
            //{

            //}


            var blnAllowCopy = chkItemAllowCopy.Checked;

            var colCount = gridView.Columns.Count;
            for (var i = 0; i < colCount; i++)
            {
                if (gridView.Columns[i].Tag != null)
                    continue;

                gridView.Columns[i].OptionsColumn.AllowEdit = blnAllowCopy;
            }
        }

        /// <summary>
        ///     常用查询条件
        /// </summary>
        private void chkItemFreQryCondition_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkItemFreQryCondition.Checked)
            {
                layoutControlGroupFreQry.Visibility = LayoutVisibility.Always;

                if (_freQryCondition == null)
                {
                    _freQryCondition = new FreQryCondition(panelFreQry);
                    _freQryCondition.PointFilter = panelControlPointFilter;
                    _freQryCondition.CalcControlWidth(2);//20190119
                    _freQryCondition.CreateControl(_lmdDt);
                }

                SetFrmQryConditionSize();
            }
            else
            {
                layoutControlGroupFreQry.Visibility = LayoutVisibility.Never;
            }
        }

        /// <summary>
        ///     保存常用查询条件
        /// </summary>
        private void tlbSaveFreQryCon_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveReportFreQryCon"))
                {
                    return;
                }

                var strFieldName = "";
                var strKey = "";
                var strDisplay = "";
                var controlCount = panelFreQry.Controls.Count;
                IList<ListmetadataInfo> lmdList = new List<ListmetadataInfo>();
                ListmetadataInfo lmd = null;
                DataRow[] drs = null;
                DataRow dr = null;
                for (var i = 0; i < controlCount; i++)
                {
                    strKey = "";
                    strDisplay = "";
                    strFieldName = "";
                    var hash = (Hashtable)panelFreQry.Controls[i].Tag;
                    if (hash.ContainsKey("_fieldName"))
                        strFieldName = TypeUtil.ToString(hash["_fieldName"]);
                    if (strFieldName == string.Empty)
                        continue;

                    if (panelFreQry.Controls[i] is UCRef)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) != string.Empty)
                        {
                            strKey += "等于&&$" + TypeUtil.ToString(hash["_strKeyValue"]);
                            strDisplay += "等于&&$" + TypeUtil.ToString(hash["_strDisplayValue"]);
                        }
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strKey = TypeUtil.ToString(hash["_strKeyValue"]);
                    }


                    drs = _lmdDt.Select("MetaDataFieldName='" + strFieldName + "'");
                    if (drs.Length != 1)
                        continue;

                    dr = drs[0];
                    lmd = new ListmetadataInfo();
                    lmd.InfoState = InfoState.Modified;
                    lmd.ID = TypeUtil.ToInt(dr["ID"]);
                    lmd.ListDataID = ListDataID;
                    lmd.MetaDataID = TypeUtil.ToInt(dr["MetaDataID"]);
                    lmd.MetaDataFieldID = TypeUtil.ToInt(dr["MetaDataFieldID"]);
                    lmd.MetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                    lmd.LngParentFieldID = TypeUtil.ToInt(dr["lngParentFieldID"]);
                    lmd.LngRelativeFieldID = TypeUtil.ToInt(dr["lngRelativeFieldID"]);
                    lmd.StrTableAlias = TypeUtil.ToString(dr["strTableAlias"]);
                    lmd.StrFullPath = TypeUtil.ToString(dr["strFullPath"]);
                    lmd.StrParentFullPath = TypeUtil.ToString(dr["strParentFullPath"]);
                    lmd.LngAliasCount = TypeUtil.ToInt(dr["lngAliasCount"]);
                    lmd.LngSourceType = TypeUtil.ToInt(dr["lngSourceType"]);
                    lmd.LngParentID = TypeUtil.ToInt(dr["lngParentID"]);
                    lmd.StrFieldType = TypeUtil.ToString(dr["strFieldType"]);
                    lmd.StrFkCode = TypeUtil.ToString(dr["strFkCode"]);
                    lmd.IsCalcField = TypeUtil.ToBool(dr["isCalcField"]);
                    lmd.StrFormula = TypeUtil.ToString(dr["strFormula"]);
                    lmd.StrRefColList = TypeUtil.ToString(dr["strRefColList"]);
                    lmd.LngOrder = TypeUtil.ToInt(dr["lngOrder"]);
                    lmd.LngOrderMethod = TypeUtil.ToInt(dr["lngOrderMethod"]);
                    lmd.StrCondition = TypeUtil.ToString(dr["strCondition"]);
                    lmd.StrConditionCHS = TypeUtil.ToString(dr["strConditionCHS"]);
                    lmd.LngKeyFieldType = TypeUtil.ToInt(dr["lngKeyFieldType"]);

                    lmd.BlnFreCondition = TypeUtil.ToBool(dr["blnFreCondition"]);
                    lmd.LngFreConIndex = TypeUtil.ToInt(dr["lngFreConIndex"]);
                    lmd.StrFreCondition = strKey;
                    lmd.StrFreConditionCHS = strDisplay;

                    lmd.BlnSysProcess = TypeUtil.ToBool(dr["blnSysProcess"]);
                    lmd.BlnShow = TypeUtil.ToBool(dr["blnShow"]);

                    lmdList.Add(lmd);
                }

                if (lmdList.Count > 0)
                    Model.SaveListMetaData(lmdList);

                MessageShowUtil.ShowInfo("保存列表常用条件成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbSaveWidth_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveReportColumnWidth"))
                {
                    return;
                }

                gridView.CloseEditor();
                gridView.UpdateColumnsCustomization();

                IList<ListdisplayexInfo> ldedto = Model.GetListDisplayExData(ListDataID);
                var lngRowIndex = 0;
                foreach (GridColumn column in gridView.VisibleColumns)
                    foreach (var dto in ldedto)
                        if (column.FieldName == dto.StrListDisplayFieldName)
                        {
                            dto.InfoState = InfoState.Modified;
                            if (column.VisibleIndex >= 0)
                            {
                                dto.LngRowIndex = lngRowIndex + 1;
                                lngRowIndex += 1;
                            }
                            else
                            {
                                dto.LngRowIndex = -1;
                            }
                            dto.LngDisplayWidth = column.Width;
                        }
                Model.SaveListDisplayEx(ldedto);


                IList<ListmetadataInfo> lmddto = Model.GetListMetaDataData(ListDataID);

                foreach (GridColumn column in gridView.Columns)
                    foreach (var dto in lmddto)
                        if (column.FieldName == dto.MetaDataFieldName)
                        {
                            dto.InfoState = InfoState.Modified;
                            if (column.VisibleIndex >= 0)
                                dto.BlnShow = true;
                            else
                                dto.BlnShow = false;
                        }
                Model.SaveListMetaData(lmddto);
                _lmdDt = Model.GetListMetaData(ListDataID, listDataExVo.UserID);
                MessageShowUtil.ShowInfo("保存列表栏目宽度成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     获取常用查询条件根据字段名
        /// </summary>
        private string GetFreQryConditionByField(string strParaFieldName)
        {
            var strConn = "";
            try
            {
                var strFieldType = "";
                var strFieldName = "";
                var strFkCode = "";
                var strKey = "";
                var strDisplay = "";
                var controlCount = panelFreQry.Controls.Count;
                for (var i = 0; i < controlCount; i++)
                {
                    strFieldName = "";
                    var hash = (Hashtable)panelFreQry.Controls[i].Tag;
                    if (hash.ContainsKey("_fieldName"))
                        strFieldName = TypeUtil.ToString(hash["_fieldName"]);
                    if (strFieldName != strParaFieldName)
                        continue;

                    if (hash.ContainsKey("_fieldType"))
                        strFieldType = TypeUtil.ToString(hash["_fieldType"]);
                    if (strFieldType == string.Empty)
                        break;

                    if (panelFreQry.Controls[i] is UCRef)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) != string.Empty)
                        {
                            strFkCode = TypeUtil.ToString(hash["_fkCode"]);
                            strKey += "等于&&$" + TypeUtil.ToString(hash["_strKeyValue"]);
                            strDisplay += "等于&&$" + TypeUtil.ToString(hash["_strDisplayValue"]);
                        }
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strKey = TypeUtil.ToString(hash["_strKeyValue"]);
                    }

                    var str = "";
                    if (strKey != string.Empty)
                    {
                        if (strFkCode != string.Empty)
                            str = BulidConditionUtil.GetRefCondition(strFieldName.Replace("_", "."), strKey,
                                strFieldType);
                        else
                            str = BulidConditionUtil.GetConditionString(strFieldName.Replace("_", "."), strFieldType,
                                strKey);
                        if (str != string.Empty)
                            strConn += " and " + str;
                    }

                    break;
                }
            }
            catch (Exception)
            {
            }

            return strConn;
        }

        /// <summary>
        ///     获取常用查询条件根据字段名
        /// </summary>
        private void GetFreQryConditionByField(string strParaFieldName, ref string strKey, ref string strDisplay)
        {
            try
            {
                var strFieldName = "";
                var controlCount = panelFreQry.Controls.Count;
                for (var i = 0; i < controlCount; i++)
                {
                    strFieldName = "";
                    var hash = (Hashtable)panelFreQry.Controls[i].Tag;
                    if (hash.ContainsKey("_fieldName"))
                        strFieldName = TypeUtil.ToString(hash["_fieldName"]);
                    if (strFieldName != strParaFieldName)
                        continue;

                    if (panelFreQry.Controls[i] is UCRef)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) != string.Empty)
                        {
                            strKey += "等于&&$" + TypeUtil.ToString(hash["_strKeyValue"]);
                            strDisplay += "等于&&$" + TypeUtil.ToString(hash["_strDisplayValue"]);
                        }
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strKey = TypeUtil.ToString(hash["_strKeyValue"]);
                        strDisplay = strKey;
                    }

                    break;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///     设置为默认显示样式
        /// </summary>
        private void tlbSetDefaultStyle_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (lookupShowStyle.EditValue != null)
                {
                    listDataExVo.StrDefaultShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);
                    listDataExVo.InfoState = InfoState.Modified;
                    Model.SaveListDataEx(listDataExVo);
                }
                MessageShowUtil.ShowInfo("设置为默认显示样式成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private string GetListStyle()
        {
            var itemCount = menuListStyle.ItemLinks.Count;
            for (var i = 0; i < itemCount; i++)
                if (((BarCheckItem)menuListStyle.ItemLinks[i].Item).Checked)
                    return menuListStyle.ItemLinks[i].Item.Caption;
            return "";
        }

        private void frmListEx_SizeChanged(object sender, EventArgs e)
        {
            var intX = Width - lookupSchema.Width - 5;
            var intY = lookupSchema.Location.Y;
            lookupSchema.Location = new Point(intX, intY);
            intX = intX - lookupShowStyle.Width - 5;
            lookupShowStyle.Location = new Point(intX, intY);
            intX = intX - buttonContent.Width - 5;
            buttonContent.Location = new Point(intX, intY);

            if (chkItemFreQryCondition.Checked && (_freQryCondition != null))
            {
                _freQryCondition.SetControlLocation();
                SetFrmQryConditionSize();
            }

            layoutControlItem1.Height = 35;
        }

        private void SetFrmQryConditionSize()
        {
            if (panelFreQry.Controls.Count > 3)
            {
                var height = 75; //2015-12-22 之前是85(如需调整,需要同时搜索2015-12-22,代码一共有三处需要修改
                var rows = panelFreQry.Controls.Count / 3;
                //var rows = panelFreQry.Controls.Count;
                if (panelFreQry.Controls.Count % 3 != 0)
                    rows++;

                height += 30 * (rows - 1); //2015-12-22 之前是37 * (rows - 1)
                layoutControlGroupFreQry.Size = new Size(layoutControlGroupFreQry.Size.Width, height);
            }
            else
            {
                layoutControlGroupFreQry.Size = new Size(layoutControlGroupFreQry.Size.Width, 120); //2015-02-06 之前是75
            }
            layoutControlItem4.Height = 40;
            layoutControlItem2.Width = 235;
            if (!ToolBar.Visible)
            {
                layoutItemGrid.Height = this.Height - 40 - layoutControlGroupFreQry.Height - 65;
            }
            else
            {
                layoutItemGrid.Height = this.Height - 70 - layoutControlGroupFreQry.Height - 65;
            }
            //layoutControlItemPointFilter.Width = 150;            
        }

        private void frmListEx_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (SocketException)
            {
            }
            catch (Exception ex)
            {
            }
        }

        private void tlbAdvancedSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView.ShowFilterEditor(gridView.FocusedColumn);
        }

        private void tlbSimpleSearch_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            gridView.OptionsView.ShowAutoFilterRow = tlbSimpleSearch.Checked;
        }

        private void gridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            gridView.Appearance.FocusedCell.BackColor = Color.White;
        }

        private void tlbFind_ItemClick()
        {
            try
            {
                var strContext = buttonContent.Text.Trim();
                if (string.IsNullOrEmpty(strContext))
                {
                    gridView.ActiveFilterString = "";
                    return;
                }

                var str = strContext.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var strFilterCondition = " ";
                var strFilter = "";
                foreach (var s in str)
                {
                    strFilterCondition = " like '%" + s + "%'";
                    foreach (GridColumn column in gridView.VisibleColumns)
                        if ((column.ColumnType.Name.ToLower() != "boolean") &&
                            (column.ColumnType.Name.ToLower() != "int"))
                            strFilter += " or [" + column.FieldName + "]" + strFilterCondition;
                }

                if (strFilter.Length > 2)
                    strFilter = strFilter.Substring(4);
                gridView.ActiveFilterString = strFilter;
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     返回字符串在字符串中出现的次数
        /// </summary>
        /// <param name="Char">要检测出现的字符</param>
        /// <param name="String">要检测的字符串</param>
        /// <returns>int</returns>
        public int GetCharInStringCount(string Char, string str1)
        {
            var str = str1.Replace(Char, "");
            return (str1.Length - str.Length) / Char.Length;
        }

        private void buttonContent_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            buttonContent.Text = "";
            gridView.ActiveFilterString = "";
        }

        private void buttonContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tlbFind_ItemClick();

                //过滤自动选择数据行
                if (tlbFilterAutoSelect.Checked)
                {
                    var rowCount = gridView.RowCount;
                    if (rowCount != 1)
                    {
                    }
                    for (var i = 0; i < rowCount; i++)
                        gridView.SetRowCellValue(i, "MultiSelect", true);

                    buttonContent.SelectAll();
                }
            }
        }

        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //AppearanceDefault appError = new AppearanceDefault(Color.White, Color.LightCoral, Color.Empty, Color.Red, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
            //AppearanceHelper.Apply(e.Appearance, appError);
        }

        private void tlbCancelCondtion_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView.FormatConditions.Clear();
        }

        private void buttonContent_EditValueChanged(object sender, EventArgs e)
        {
            //过滤自动选择数据行，不过滤，回车再过滤。 （解决问题：扫描枪录入每个字符都过滤太慢）
            if (tlbFilterAutoSelect.Checked)
                return;

            tlbFind_ItemClick();
        }

        /// <summary>
        ///     打开等待对话框
        /// </summary>
        private void OpenWaitDialog(string caption)
        {
            CloseWaitDialog();
            wdf = new WaitDialogForm(caption + "...", "请等待...");
            Cursor = Cursors.WaitCursor;
        }

        /// <summary>
        ///     关闭等待对话框
        /// </summary>
        private void CloseWaitDialog()
        {
            if (wdf != null)
                wdf.Close();

            Cursor = Cursors.Default;
        }

        private void ShowRunTimeMessage(DateTime datstart)
        {
            var span = DateTime.Now - datstart;
            var seconds = span.Seconds + span.Milliseconds / 1000.0M;
            MessageBox.Show("此按钮执行了 " + decimal.Round(seconds, 1) + " 秒时间!", "提示", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void tlbExportExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".xls";
            Export(1, strFileType);
        }

        private void tlbExeclPDF_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".pdf";
            Export(2, strFileType);
        }

        private void txtExportTXT_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".txt";
            Export(3, strFileType);
        }

        private void txtExportCSV_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".csv";
            Export(4, strFileType);
        }

        private void txtExportHTML_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".html";
            Export(5, strFileType);
        }

        /// <summary>
        ///     导出通用方法
        /// </summary>
        /// <param name="LngTypeID">文件类型ID，用于默认选择类型</param>
        /// <param name="strFileType">文件类型名称，用于给文件名赋后缀名</param>
        private void Export(int LngTypeID, string strFileType)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter =
                    "Excel文件(*.xls)|*.xls|PDF文件(*.pdf)|*.pdf|TXT文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|HTML文件(*.html)|*.html";
                saveFileDialog.Title = "保存文件";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = Text;
                saveFileDialog.FilterIndex = LngTypeID;
                var strShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (strShowStyle == "pivot")
                    {
                        //报表导出
                        if (LngTypeID == 1)
                            rp.ExportToXls(saveFileDialog.FileName);
                        if (LngTypeID == 2)
                            rp.ExportToPdf(saveFileDialog.FileName);
                        if (LngTypeID == 3)
                            rp.ExportToText(saveFileDialog.FileName);
                        if (LngTypeID == 4)
                            rp.ExportToCsv(saveFileDialog.FileName);
                        if (LngTypeID == 5)
                            rp.ExportToHtml(saveFileDialog.FileName);
                    }
                    else if (strShowStyle == "chart")
                    {
                        //图表导出
                    }
                    else
                    {
                        gridView.OptionsPrint.PrintHeader = true; //设置后列表的列名会打印出来
                        gridView.OptionsPrint.PrintFooter = true; //设置后列表有footer（如小计）
                        //gridView.OptionsPrint.UsePrintStyles = false;//设置后为保留列表颜色样式

                        //列表方式
                        if (LngTypeID == 1)
                            gridView.ExportToXls(saveFileDialog.FileName, false);
                        if (LngTypeID == 2)
                            gridView.ExportToPdf(saveFileDialog.FileName);
                        if (LngTypeID == 3)
                            gridView.ExportToText(saveFileDialog.FileName);
                        if (LngTypeID == 4)
                            gridView.ExportToCsv(saveFileDialog.FileName);
                        if (LngTypeID == 5)
                            gridView.ExportToHtml(saveFileDialog.FileName);
                    }

                    if (DialogResult.Yes ==
                        MessageBox.Show("是否立即打开此文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void barSubItemExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("ExportReportData"))
                    return;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            var frm = new frmSet();
            frm.ShowDialog();
        }

        private void gridView_CellMerge(object sender, CellMergeEventArgs e)
        {
            // 20170918
            ////处理合并问题。gridview合并事件，指定一列为合并依据列
            //var strFileName = e.Column.FieldName;
            //var rowsmain = _lmdDt.Select("blnMerge=1 and blnMainMerge=1");      //主合并
            //if (rowsmain.Length == 0) return;
            //var rowsmainA = _lmdDt.Select("blnMerge=1 and blnMainMerge=1 and MetaDataFieldName='" + strFileName + "'");

            //if ((rowsmainA == null) || (rowsmainA.Length == 0))     //当前合并不是主合并
            //{
            //    //如果合并的不是主合并字段，则需要处理其他合并字段，根据主合并字段进行合并
            //    var strValue = "";
            //    var strValuePrev = "";
            //    var strValueFileName = "";
            //    var strValueFileNamePrev = "";
            //    foreach (var row in rowsmain)
            //    {
            //        var strMainMergeFileName = TypeUtil.ToString(row["MetaDataFieldName"]);
            //        strValue += gridView.GetRowCellValue(e.RowHandle1, strMainMergeFileName) + "&";
            //        strValuePrev += gridView.GetRowCellValue(e.RowHandle2, strMainMergeFileName) + "&";
            //        strValueFileName += gridView.GetRowCellValue(e.RowHandle1, strFileName) + "&";
            //        strValueFileNamePrev += gridView.GetRowCellValue(e.RowHandle2, strFileName) + "&";
            //    }
            //    if ((strValue == strValuePrev) && (strValueFileName == strValueFileNamePrev))
            //    {
            //        //如果发现相邻两行主合并字段值相等并且不是主合并的值也相等，则才合并，否则取消合并
            //        e.Merge = true;
            //        e.Handled = true;
            //    }
            //    else
            //    {
            //        e.Merge = false;
            //        e.Handled = true;
            //    }
            //}

            ////如果原本就要合并则不处理
            var strFileName = e.Column.FieldName;       //当前列字段
            string strValueFileName = gridView.GetRowCellValue(e.RowHandle1, strFileName).ToString();
            string strValueFileNamePrev = gridView.GetRowCellValue(e.RowHandle2, strFileName).ToString();
            if (strValueFileName != strValueFileNamePrev)
            {
                return;
            }

            var rowsmain = _lmdDt.Select("blnMerge=1 and blnMainMerge=1");      //主合并
            bool bSplit = false;
            for (int i = 0; i < rowsmain.Length; i++)
            {
                var strMainMergeFileName = TypeUtil.ToString(rowsmain[i]["MetaDataFieldName"]);     //主合并字段
                string strValue = gridView.GetRowCellValue(e.RowHandle1, strMainMergeFileName).ToString();
                string strValuePrev = gridView.GetRowCellValue(e.RowHandle2, strMainMergeFileName).ToString();
                if (strValue != strValuePrev)
                {
                    bSplit = true;
                    break;
                }
            }
            if (bSplit)
            {
                e.Merge = false;
                e.Handled = true;
            }
        }

        /// <summary>
        ///     备注录入(录入后在对应的区域显示)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDescInput_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (lookupShowStyle.EditValue != null)
                {
                    // 20170821
                    if (dt == null)
                    {
                        MessageShowUtil.ShowMsg("请先查询数据。");
                        return;
                    }

                    DateTime dtRemark = GetRemarkTime();
                    var listDataRemarkInfo = Model.GetListDataRemarkByTimeListDataId(dtRemark, listDataExVo.ListDataID);

                    var frm = new frmInputDesc();
                    //frm.StrDesc = listDataExVo.StrListSrcSQL;

                    if (listDataRemarkInfo == null)
                    {
                        frm.StrDesc = "";
                    }
                    else
                    {
                        frm.StrDesc = listDataRemarkInfo.Remark;
                    }

                    frm.ShowDialog();
                    if (frm.BlnOK)
                    {
                        //listDataExVo.StrListSrcSQL = frm.StrDesc;
                        //listDataExVo.InfoState = InfoState.Modified;
                        //Model.SaveListDataEx(listDataExVo);
                        if (listDataRemarkInfo == null)
                        {
                            Model.AddListdataremark(listDataExVo.ListDataID, dtRemark, frm.StrDesc);
                        }
                        else
                        {
                            Model.UpdateListdataremarkByTimeListDataId(listDataExVo.ListDataID, dtRemark, frm.StrDesc);
                        }

                        listTemplevo = Model.GetListTemple(listDataExVo.ListDataID);
                        lookupShowStyle_EditValueChanged(null, null);
                        //MessageShowUtil.ShowInfo("备注保存成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     忽略安装位置勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkblnKeyWord_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (chkblnKeyWord.Checked)
            {
                var RowsblnKeyWord = _lmdDt.Select("blnKeyWord=1");
                if ((RowsblnKeyWord == null) || (RowsblnKeyWord.Length == 0))
                    MessageShowUtil.ShowMsg("方案里未设置关键字，此设置将无效");
            }
        }

        private void tlbColumnSortSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var rows = metadataFieldDt.Select("MetaDataID=" + listExvo.MainMetaDataID + " and blnDesignSort=1");
                if ((rows == null) || (rows.Length == 0))
                {
                    MessageShowUtil.ShowMsg("元数据里没有配置可编排的栏目，不能进行编排");
                    return;
                }
                var strFKCode = TypeUtil.ToString(rows[0]["strFkCode"]);
                var frm = new frmDataSortSet();
                frm.MetadataID = listExvo.MainMetaDataID;
                frm.dtLmd = _lmdDt;
                var strFileName =
                    _lmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["MetaDataFieldName"].ToString();
                frm.strFileName = strFileName;
                var strListDisplayName =
                    _lmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["strListDisplayFieldNameCHS"]
                        .ToString();
                frm.strListDisplayName = strListDisplayName;
                frm.strFKCode = strFKCode;
                frm.ListDataID = ListDataID;
                frm.ShowDialog();
                if (frm.blnOK)
                {
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void GetDistinctSql(ref string strListSql)
        {
            if (listExvo.StrListCode == "MLLKDDay")
                strListSql = strListSql.Insert(6, " distinct ");
        }

        #region 报表设置

        private XtraReportTemple rp;
        private int ReportWidth;
        public string strReportFileName = ""; //报表模板文件名称，这个主要用于从方案界面进入编辑报表模板用，不用方案调用此变量为"",为空的时候需要赋值


        public void SetPivotFormat()
        {
            rp = new XtraReportTemple();
            if (strReportFileName == "") //等于空说明是从列表里面点击，如果不等于空说明是从报表设计器里面点击
                strReportFileName = listDataExVo.StrListDataName + ListDataID;
            var strReportTemple = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple\\" + strReportFileName +
                                  ".repx";
            var strReportDir = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple";
            if (!Directory.Exists(strReportDir))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple");
            if (File.Exists(strReportTemple) && (0 == 1)) //2014-12-08由于现在模板全部从数据库里读取，所以暂时让这个IF语句一直不执行
            {
                //如果存在，直接加载模板，没有打开后另存
                rp.LoadLayout(strReportTemple);
                rp.Bands["Detail"].FormattingRules.Clear();
                //2014-12-01加载模板后设置条件格式格式,因为如果不加载，方案里面的设置就无效，而去读取报表模板文件
                foreach (var dto in listDisplayExList)
                {
                    //设置条件格式
                    if (!string.IsNullOrEmpty(dto.StrConditionFormat) && (dto.LngApplyType != 2))
                        SetReportColumnConditionFormat(dto.StrListDisplayFieldName, dto.StrConditionFormat);

                    //模糊查询设置
                    if (!string.IsNullOrEmpty(dto.StrBluerCondition))
                        SetGridColumnBluerFormat(dto.StrListDisplayFieldName, dto.StrBluerCondition);

                    //设置编码顺序
                    SetGridColumnSort(dto.StrListDisplayFieldName);
                }
            }
            //如果数据库里有存了模板文件，那么就去数据库里下载下来存在本地
            if ((listTemplevo != null) && (listTemplevo.ListTempleID > 0))
            {
                var fileData = listTemplevo.BloImage;
                var buffer = fileData.GetUpperBound(0) + 1;
                var fs = new FileStream(strReportTemple, FileMode.Create, FileAccess.Write);
                fs.Write(fileData, 0, buffer);
                fs.Close();

                rp.LoadLayout(strReportTemple);
                rp.Bands["Detail"].FormattingRules.Clear();
                DicBluerCondition.Clear();
                foreach (var dto in listDisplayExList)
                {
                    //加载条件格式
                    if (!string.IsNullOrEmpty(dto.StrConditionFormat) && (dto.LngApplyType != 2))
                        SetReportColumnConditionFormat(dto.StrListDisplayFieldName, dto.StrConditionFormat);

                    //模糊查询设置
                    if (!string.IsNullOrEmpty(dto.StrBluerCondition))
                        SetGridColumnBluerFormat(dto.StrListDisplayFieldName, dto.StrBluerCondition);
                    //设置编码顺序
                    SetGridColumnSort(dto.StrListDisplayFieldName);


                    //设置合并

                    var xrtable = rp.FindControl("TableReportDetail", true) as XRTable;
                    foreach (XRTableRow row in xrtable.Rows)
                        foreach (XRTableCell cell in row.Cells)
                        {
                            if (cell.DataBindings["Text"] == null) continue;
                            if (cell.DataBindings["Text"].DataMember == dto.StrListDisplayFieldName)
                                if (dto.BlnMerge)
                                    cell.ProcessDuplicates = ValueSuppressType.MergeByValue;
                                else
                                    cell.ProcessDuplicates = ValueSuppressType.Leave;
                        }
                }
            }
            else
            {
                //根据ListDisplay表得到报表的宽度
                ReportWidth = 0;
                foreach (var dto in listDisplayExList)
                {
                    if (dto.LngRowIndex < 0) continue;
                    ReportWidth += dto.LngDisplayWidth;
                }

                CreateReportHead();
                CreatePageHeader();
                CreateReportDetail();
                rp.PaperKind = PaperKind.Custom;
                rp.PageWidth = ReportWidth + rp.Margins.Left + rp.Margins.Right;
                rp.SaveLayout(strReportTemple);
            }
        }

        private void CreateReportHead()
        {
            var lable = (XRLabel)rp.FindControl("lblTitle", true); //这种方式是先在界面上把控件拖放起
            if (listExvo == null)
                listExvo = Model.GetListEx(listId);
            lable.Text = listExvo.StrListName;
            lable.LocationFloat = new PointFloat((ReportWidth - lable.WidthF) / 2F, lable.LocationF.Y);

            //这种方式是动态创建控件，目前不建立这么处理
            //XRLabel lable = new XRLabel();
            //lable.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //lable.Name = "lblTitle";
            //lable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            //lable.SizeF = new System.Drawing.SizeF(ReportWidth, 35.20833F);
            //lable.StylePriority.UseFont = false;
            //lable.StylePriority.UseTextAlignment = false;
            //lable.Text = this.Text;
            //lable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //rp.Bands["ReportHeader"].Controls.Add(lable);
        }


        private void CreatePageHeader()
        {
            //创建查询条件面板
            //rp.Bands["PageHeader"].Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (rp.Bands["PageHeader"].Controls.Count > 0)
            {
                if (rp.Bands["PageHeader"].Controls[0] != null)
                    rp.Bands["PageHeader"].Controls[0].Dispose();
            }
            var lblConditon = new XRLabel();
            lblConditon.Text = "";
            lblConditon.Name = "lblConditon";
            lblConditon.Size = new Size(ReportWidth, 40);
            lblConditon.TextAlignment = TextAlignment.MiddleLeft;
            lblConditon.Font = new Font("宋体", 12.5F, FontStyle.Regular, GraphicsUnit.Point, 134);
            rp.Bands["PageHeader"].Controls.Add(lblConditon);


            //创建标题
            var xrtable = new XRTable();
            xrtable.Name = "TablePageHeader";
            xrtable.SizeF = new SizeF(ReportWidth, 40F);
            xrtable.Borders = BorderSide.All;
            xrtable.LocationF = new PointF(lblConditon.LocationF.X, lblConditon.LocationF.Y + lblConditon.SizeF.Height);

            var row = new XRTableRow();
            xrtable.Rows.Add(row);

            XRTableCell xrcell = null;

            xrcell = new XRTableCell();
            xrcell.Weight = 50 / 250D;
            xrcell.Font = new Font("宋体", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            xrcell.TextAlignment = TextAlignment.MiddleCenter;
            xrcell.Text = "序号";
            row.Cells.Add(xrcell);


            foreach (var dto in listDisplayExList)
            {
                var lngRowIndex = dto.LngRowIndex;
                if (lngRowIndex < 0) continue;
                var strListDisplayFieldNameCHS = dto.StrListDisplayFieldNameCHS;
                ;
                var lngwidth = dto.LngDisplayWidth;
                xrcell = new XRTableCell();
                xrcell.Weight = lngwidth / 250D;
                xrcell.Font = new Font("宋体", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
                xrcell.TextAlignment = TextAlignment.MiddleCenter;
                xrcell.Text = strListDisplayFieldNameCHS;
                xrcell.Tag = dto.StrListDisplayFieldName;
                row.Cells.Add(xrcell);
            }

            //rp.Bands["PageHeader"].Controls.Clear();
            rp.Bands["PageHeader"].Controls.Add(xrtable);
        }

        private void CreateReportDetail()
        {
            ///动态创建一个表格
            var xrtable = new XRTable();
            xrtable.Name = "TableReportDetail";
            xrtable.SizeF = new SizeF(ReportWidth, 25F);
            xrtable.Borders = BorderSide.All;
            xrtable.Borders = BorderSide.Left | BorderSide.Bottom | BorderSide.Right;


            //动态创建一行
            var row = new XRTableRow();
            xrtable.Rows.Add(row);
            XRTableCell xrcell = null;

            //添加一个序号列
            xrcell = new XRTableCell();
            xrcell.Weight = 50 / 250D;
            xrcell.Font = new Font("宋体", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            xrcell.TextAlignment = TextAlignment.MiddleCenter;
            xrcell.DataBindings.Add("Text", dt, "colNumber");
            row.Cells.Add(xrcell);

            ///根据数据库Display表来自动创建列
            foreach (var dto in listDisplayExList)
            {
                var lngRowIndex = dto.LngRowIndex;
                if (lngRowIndex < 0) continue;
                var strFileName = dto.StrListDisplayFieldName;
                var lngwidth = dto.LngDisplayWidth;
                xrcell = new XRTableCell();
                xrcell.Weight = lngwidth / 250D; //Dev此版本是根据权重来控制显示大小，不能用Width属性   
                xrcell.Font = new Font("宋体", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
                xrcell.TextAlignment = TextAlignment.MiddleCenter;

                if (dto.StrSummaryDisplayFormat == "")
                    xrcell.DataBindings.Add("Text", dt, strFileName);
                if (dto.StrSummaryDisplayFormat == "货币")
                {
                    xrcell.DataBindings.Add("Text", dt, strFileName, "{0:c2}");
                    xrcell.TextAlignment = TextAlignment.MiddleRight;
                }
                if (dto.StrSummaryDisplayFormat == "重量")
                {
                    xrcell.DataBindings.Add("Text", dt, strFileName, "{0:n2}");
                    xrcell.TextAlignment = TextAlignment.MiddleRight;
                }
                if (dto.StrSummaryDisplayFormat == "日期")
                {
                    xrcell.DataBindings.Add("Text", dt, strFileName, "{0:yyyy-MM-dd}");
                    xrcell.TextAlignment = TextAlignment.MiddleRight;
                }
                if (dto.StrSummaryDisplayFormat == "时间")
                {
                    xrcell.DataBindings.Add("Text", dt, strFileName, "{0:HH:mm:ss}");
                    xrcell.TextAlignment = TextAlignment.MiddleRight;
                }
                if (dto.StrSummaryDisplayFormat == "日期时间")
                {
                    xrcell.DataBindings.Add("Text", dt, strFileName, "{0:yyyy-MM-dd HH:mm:ss}");
                    xrcell.TextAlignment = TextAlignment.MiddleRight;
                }
                xrcell.Tag = dto.StrListDisplayFieldName;
                xrcell.ProcessDuplicates = dto.BlnMerge ? ValueSuppressType.MergeByValue : ValueSuppressType.Leave;
                row.Cells.Add(xrcell);
                //条件格式设置
                if (!string.IsNullOrEmpty(dto.StrConditionFormat) && (dto.LngApplyType != 2))
                    SetReportColumnConditionFormat(strFileName, dto.StrConditionFormat);
                //模糊查询设置
                if (!string.IsNullOrEmpty(dto.StrBluerCondition))
                    SetGridColumnBluerFormat(dto.StrListDisplayFieldName, dto.StrBluerCondition);

                //设置编码顺序
                SetGridColumnSort(dto.StrListDisplayFieldName);
            }

            //rp.Bands["Detail"].Controls.Clear();
            //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
            while (rp.Bands["Detail"].Controls.Count > 0)
            {
                if (rp.Bands["Detail"].Controls[0] != null)
                    rp.Bands["Detail"].Controls[0].Dispose();
            }
            rp.Bands["Detail"].Controls.Add(xrtable);
        }


        private void ReportDataBinding()
        {
            // 20170918
            //#region 2015-02-04   解决报表合并问题
            //var rowsmain = _lmdDt.Select("blnMerge=1 and blnMainMerge=1");
            //var rowsdetail = _lmdDt.Select("blnMerge=1 and blnMainMerge=0");
            //if (rowsdetail.Length > 0)
            //{
            //    var strMerge = " ";
            //    var strMainMergeValue = "";
            //    var strMainMergeValuePrev = "";
            //    for (var i = 0; i < dt.Rows.Count; i++)
            //    {
            //        if (i == 0) continue;
            //        strMainMergeValue = "";
            //        strMainMergeValuePrev = "";
            //        foreach (var row in rowsmain)
            //        {
            //            var strMainMergeFileName = TypeUtil.ToString(row["MetaDataFieldName"]);
            //            strMainMergeValue += TypeUtil.ToString(dt.Rows[i][strMainMergeFileName]) + "&";
            //            strMainMergeValuePrev += TypeUtil.ToString(dt.Rows[i - 1][strMainMergeFileName]) + "&";
            //        }
            //        foreach (var row in rowsdetail)
            //        {
            //            var strMergeFileName = row["MetaDataFieldName"].ToString();
            //            var Value = TypeUtil.ToString(dt.Rows[i][strMergeFileName]);
            //            var ValuePrev = TypeUtil.ToString(dt.Rows[i - 1][strMergeFileName]);
            //            if (Value == ValuePrev)
            //                if (strMainMergeValue != strMainMergeValuePrev)
            //                {
            //                    dt.Rows[i][strMergeFileName] = dt.Rows[i][strMergeFileName] + strMerge;
            //                    if (strMerge == " ")
            //                        strMerge = "  ";
            //                    else
            //                        strMerge = " ";
            //                }
            //        }
            //    }
            //}
            //#endregion

            var rpDt = dt.Clone();

            for (int i = 0; i < rpDt.Columns.Count; i++)
            {
                rpDt.Columns[i].DataType = typeof(string);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = rpDt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string col = dt.Columns[j].ToString();
                    row[col] = dt.Rows[i][col].ToString();
                }
                rpDt.Rows.Add(row);
            }

            var rowsMainMerge = _lmdDt.Select("blnMerge=1 and blnMainMerge=1");     //主合并

            if (rowsMainMerge.Length > 0)        //有主合并列才处理
            {
                //string sSplitSign = " ";        //分割标志
                bool bIfAddBlankLast = false;       //上一次是否添加了空格
                for (int i = 0; i < rpDt.Rows.Count - 1; i++)
                {
                    //判断该行与下一行的主合并列是否有分隔线
                    bool bSsplit = false;       //是否需要分割
                    for (int j = 0; j < rowsMainMerge.Length; j++)
                    {
                        string sZhbzd = rowsMainMerge[j]["MetaDataFieldName"].ToString();
                        string sFirstRow = rpDt.Rows[i][sZhbzd].ToString();
                        string sSecondRow = rpDt.Rows[i + 1][sZhbzd].ToString();
                        if (sFirstRow != sSecondRow)
                        {
                            bSsplit = true;
                            break;
                        }
                    }

                    //整行需要分割线
                    if (bSsplit)
                    {
                        //当前行之后的行加空格
                        for (int j = i; j < rpDt.Rows.Count - 1; j++)
                        {
                            for (int k = 0; k < rpDt.Columns.Count; k++)
                            {
                                string sCol = rpDt.Columns[k].ToString();
                                if (bIfAddBlankLast)
                                {
                                    string val = rpDt.Rows[j + 1][sCol].ToString();
                                    rpDt.Rows[j + 1][sCol] = val.Substring(0, val.Length - 1);
                                }
                                else
                                {
                                    rpDt.Rows[j + 1][sCol] += " ";
                                }
                            }
                        }
                        bIfAddBlankLast = !bIfAddBlankLast;
                    }
                }
            }

            rp.DataSource = rpDt;
        }

        private void SetReportColumnConditionFormat(string strFileName, string strConditionFormat)
        {
            var sfcType = (SFCDataTypeEnum)TypeUtil.ToInt(strConditionFormat.Substring(strConditionFormat.Length - 1));
            var strTemp = strConditionFormat.Remove(strConditionFormat.Length - 1);
            var strConditions = strTemp.Split(new[] { "&&$$" }, StringSplitOptions.RemoveEmptyEntries);
            if (strConditions.Length > 0)
            {
                var str = string.Empty;
                var strOper = string.Empty;
                var strValue1 = string.Empty;
                var strValue2 = string.Empty;
                var blnApplyRow = false;
                FormattingRule rule = null;
                var color = Color.White;
                var fontColor = Color.Black;

                for (var i = 0; i < strConditions.Length; i++)
                {
                    str = strConditions[i];
                    if (str.Contains("&&$"))
                    {
                        var strs = str.Split(new[] { "&&$" }, StringSplitOptions.RemoveEmptyEntries);
                        strOper = strs[0];
                        blnApplyRow = TypeUtil.ToBool(strs[1]);
                        strValue1 = "";
                        strValue2 = "";
                        if (strs.Length > 3)
                            strValue1 = strs[2];
                        if (strs.Length > 4)
                            strValue2 = strs[3];

                        var ss = strs.Length > 5 ? strs[4].Split(';') : strs[3].Split(';');
                        Model.GetColorByString(ss, ref color);

                        var fontss = strs.Length > 5 ? strs[5].Split(';') : strs[4].Split(';');
                        Model.GetColorByString(fontss, ref fontColor);
                    }

                    var strCaclOper = GetCaclOper(strOper, strFileName);
                    rule = new FormattingRule();
                    rp.FormattingRuleSheet.Add(rule);
                    rule.DataSource = rp.DataSource;
                    rule.Formatting.BackColor = color;
                    rule.Formatting.ForeColor = fontColor;
                    rule.Condition = string.Format(strFileName + strCaclOper, strValue1, strValue2);
                    rp.Bands["Detail"].FormattingRules.Add(rule);
                }
            }
        }


        /// <summary>
        ///     根据设置的条件格式类型组织条件表达式串
        /// </summary>
        /// <param name="strOper">条件类型</param>
        /// <param name="strFileName">列名（主要是用于介于两者之间的情况）</param>
        /// <returns></returns>
        private string GetCaclOper(string strOper, string strFileName)
        {
            var strCaclOper = "";
            switch (strOper)
            {
                case "等于":
                    strCaclOper = " = '{0}'";
                    break;
                case "不等于":
                    strCaclOper = " <> '{0}'";
                    break;
                case "大于":
                    strCaclOper = " > {0}";
                    break;
                case "大于等于":
                    strCaclOper = " >= {0}";
                    break;
                case "小于":
                    strCaclOper = " < {0}";
                    break;
                case "小于等于":
                    strCaclOper = " <= {0}";
                    break;
                case "介于":
                    strCaclOper = " >= {0}  and  " + strFileName + " <= {1}";
                    break;
                case "不介于":
                    strCaclOper = " <= {0}  and  " + strFileName + " >= {1}";
                    break;
                default:
                    strCaclOper = "";
                    break;
            }
            return strCaclOper;
        }


        public string GetFieldCaption(string str)
        {
            var a = str.LastIndexOf('(');
            if (a > 0)
                str = str.Remove(a);
            return str;
        }

        #endregion

        /// <summary>
        /// 获取备注时间
        /// </summary>
        /// <returns></returns>
        private DateTime GetRemarkTime()
        {
            DateTime dtRemark;
            if (_strFreQryConditionByChs == "" || listId != 9)
            {
                dtRemark = new DateTime(1900, 01, 01);
            }
            else
            {
                string sQueryTime =
                    _strFreQryConditionByChs.Substring(5, _strFreQryConditionByChs.Length - 5).Split('至')[0];
                dtRemark = Convert.ToDateTime(sQueryTime);
            }
            return dtRemark;
        }

        private void radioButtonArrangePoint_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonArrangePoint.Checked)
            {
                lookUpEditArrangeTime.Enabled = true;
                lookUpEditArrangeActivity.Enabled = true;
            }
            else
            {
                lookUpEditArrangeTime.Enabled = false;
                lookUpEditArrangeActivity.Enabled = false;
            }
        }

        private void lookUpEditArrangeTime_EditValueChanged(object sender, EventArgs e)
        {
            var listDataLay = listlayount.FirstOrDefault(a => a.ListDataLayoutID.ToString() == lookUpEditArrangeTime.EditValue.ToString());
            if (listDataLay != null)
            {
                string point = listDataLay.StrConTextCondition.Split(new string[] { ".point in (" }, StringSplitOptions.None)[1];
                string point2 = point.Substring(0, point.Length - 3);
                labelArrangePoint.Text = point2;
            }
        }

        private void radioButtonStorePoint_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStorePoint.Checked)
            {
                lookUpEditRemoveDuplication.Enabled = true;
            }
            else
            {
                lookUpEditRemoveDuplication.Enabled = false;
            }
        }

        private void BeforeDay_ItemClick(object sender, ItemClickEventArgs e)
        {
            NearQuery(-1);
            tlbRefresh_ItemClick(this, null);
        }

        private void AfterDay_ItemClick(object sender, ItemClickEventArgs e)
        {
            NearQuery(1);
            tlbRefresh_ItemClick(this, null);
        }

        /// <summary>
        /// 临近时间查询
        /// </summary>
        /// <param name="day"></param>
        private void NearQuery(int day)
        {
            try
            {
                var controls = panelFreQry.Controls;
                foreach (Control item in controls)
                {
                    //var hash = item.Tag as Hashtable;
                    //if (hash == null)
                    //{
                    //    continue;
                    //}

                    //if (!hash.ContainsKey("_fieldName"))
                    //{
                    //    continue;
                    //}

                    //var fieldName = hash["_fieldName"].ToString();
                    //var fieldNameSplit = fieldName.Split('_');
                    //var split2 = fieldNameSplit[fieldNameSplit.Length - 1].ToLower();
                    //if (split2 != "datsearch")
                    //{
                    //    continue;
                    //}

                    // 20180408
                    var type = item.GetType();
                    var typeNameCustom = type.Name;
                    if (typeNameCustom.Contains("DateTime") && !typeNameCustom.Contains("Year") && !typeNameCustom.Contains("Month"))
                    {
                        var subcontrols = item.Controls[0].Controls;
                        List<DateEdit> allDe = new List<DateEdit>();
                        foreach (Control item2 in subcontrols)
                        {
                            var typeName = item2.GetType();

                            if (typeName.Name == "DateEdit")
                            {
                                var de = item2 as DateEdit;
                                if (de != null && de.DateTime != new DateTime(1, 1, 1))
                                {
                                    //de.DateTime = de.DateTime.AddDays(day);
                                    allDe.Add(de);
                                }
                            }
                        }
                        var allDeOrder = allDe.OrderBy(a => a.DateTime).ToList();
                        if (day < 0)
                        {
                            for (int i = 0; i < allDeOrder.Count; i++)
                            {
                                allDeOrder[i].DateTime = allDeOrder[i].DateTime.AddDays(day);
                            }
                        }
                        else
                        {
                            for (int i = allDeOrder.Count - 1; i >= 0; i--)
                            {
                                allDeOrder[i].DateTime = allDeOrder[i].DateTime.AddDays(day);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowErrow(e);
            }
        }

        private void barButtonItemExportExcelAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel文件(*.xls)|*.xls",
                    Title = "保存文件",
                    RestoreDirectory = true,
                    FileName = Text
                };

                var res = saveFileDialog.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                OpenWaitDialog("正在生成文件");

                SetDayTableSql();
                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                var dtExport = Model.GetDataTable(strListSql);      //导出的行
                var displayExListNeed = listDisplayExList.Where(a => a.LngRowIndex != -1).OrderBy(a => a.LngRowIndex).ToList();      //导出的列

                if (/*_listdate == null ||*/ dtExport == null || dtExport.Rows.Count == 0 || displayExListNeed.Count == 0)
                {
                    MessageShowUtil.ShowMsg("无数据导出！");
                    return;
                }

                HSSFWorkbook hssfworkbook = new HSSFWorkbook();
                Sheet sheet1 = hssfworkbook.CreateSheet("sheet1");      //建立Sheet1

                //标题
                Row rowTitle = sheet1.CreateRow(0);
                rowTitle.Height = 500;
                var cellTitle = rowTitle.CreateCell(0);
                cellTitle.SetCellValue(Text);

                var fontTitle = hssfworkbook.CreateFont();
                fontTitle.FontName = "宋体";      //字体
                fontTitle.FontHeightInPoints = 20;      //字号
                //font1.Color = HSSFColor.RED.index;        //颜色
                fontTitle.Boldweight = (short)FontBoldWeight.BOLD;      //粗体
                //font1.IsItalic = true;        //斜体
                //font1.Underline = (byte)FontUnderlineType.DOUBLE;     //添加双下划线
                CellStyle styleTitle = hssfworkbook.CreateCellStyle();
                styleTitle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;        //居中
                //styleTitle.VerticalAlignment = VerticalAlignment.CENTER;        //垂直居中 
                //styleTitle.WrapText = true;     //自动换行
                styleTitle.SetFont(fontTitle);
                cellTitle.CellStyle = styleTitle;

                var columnCount = displayExListNeed.Count;
                sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, columnCount - 1));     //CellRangeAddress(起始行,终止行,起始列,终止列);

                //列头
                var fontColumnHead = hssfworkbook.CreateFont();
                fontColumnHead.FontName = "宋体";      //字体
                fontColumnHead.FontHeightInPoints = 10;      //字号
                //font1.Color = HSSFColor.RED.index;        //颜色
                fontColumnHead.Boldweight = (short)FontBoldWeight.BOLD;     //粗体
                //font1.IsItalic = true;        //斜体
                //font1.Underline = (byte)FontUnderlineType.DOUBLE;     //添加双下划线
                CellStyle styleColumnHead = hssfworkbook.CreateCellStyle();
                styleColumnHead.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;        //居中
                //styleTitle.VerticalAlignment = VerticalAlignment.CENTER;        //垂直居中 
                //styleTitle.WrapText = true;     //自动换行
                styleColumnHead.SetFont(fontColumnHead);

                Row rowColumnHead = sheet1.CreateRow(1);
                for (int i = 0; i < displayExListNeed.Count; i++)
                {
                    sheet1.SetColumnWidth(i, displayExListNeed[i].LngDisplayWidth * 40);

                    var cellColumnHead = rowColumnHead.CreateCell(i);
                    var columnHeadText = displayExListNeed[i].StrListDisplayFieldNameCHS;
                    cellColumnHead.SetCellValue(columnHeadText);
                    cellColumnHead.CellStyle = styleColumnHead;
                }

                //列表内容
                var fontDetail = hssfworkbook.CreateFont();
                fontDetail.FontName = "宋体";      //字体
                fontDetail.FontHeightInPoints = 10;      //字号
                //font1.Color = HSSFColor.RED.index;        //颜色
                //fontColumnHead.Boldweight = 700;     //粗体
                //font1.IsItalic = true;        //斜体
                //font1.Underline = (byte)FontUnderlineType.DOUBLE;     //添加双下划线
                CellStyle styleDetail = hssfworkbook.CreateCellStyle();
                styleDetail.SetFont(fontDetail);
                styleDetail.WrapText = true;     //自动换行
                styleDetail.VerticalAlignment = VerticalAlignment.CENTER;        //垂直居中 

                for (int i = 0; i < dtExport.Rows.Count; i++)
                {
                    Row rowDetail = sheet1.CreateRow(i + 2);
                    for (int j = 0; j < displayExListNeed.Count; j++)
                    {
                        var cellDetail = rowDetail.CreateCell(j);
                        var detailFieldName = displayExListNeed[j].StrListDisplayFieldName;
                        var detailText = dtExport.Rows[i][detailFieldName].ToString();
                        cellDetail.SetCellValue(detailText);
                        cellDetail.CellStyle = styleDetail;
                    }
                }

                FileStream file = new FileStream(saveFileDialog.FileName, FileMode.Create);
                hssfworkbook.Write(file);
                file.Close();
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowErrow(exception);
            }
            finally
            {
                CloseWaitDialog();
            }
        }

        /// <summary>
        /// 开关量状态变动数据修正（重新计算次数和持续时间）
        /// </summary>
        private void SwitchingValueStateChangeDataAmend()
        {
            try
            {
                var listCode = listExvo.StrListCode;
                if (listCode != "KGLStateRBReport")
                {
                    return;
                }

                var ifMergeRecords = CbfSettingRequest.IfMergeRecords();
                if (!ifMergeRecords)
                {
                    return;
                }

                var dtHandled = dt.Copy();
                dtHandled.DefaultView.Sort = "ViewJC_KGStateMonth1_point asc,ViewJC_KGStateMonth1_stime asc";
                dtHandled = dtHandled.DefaultView.ToTable();

                //删未变化状态
                string currentPoint = null;
                string currentState = null;
                for (int i = dtHandled.Rows.Count - 1; i >= 0; i--)
                {
                    var point = dtHandled.Rows[i]["ViewJC_KGStateMonth1_point"].ToString();
                    var state = dtHandled.Rows[i]["ViewJC_KGStateMonth1_state"].ToString();

                    if (point == currentPoint && state == currentState)
                    {
                        var rightEndTime = dtHandled.Rows[i + 1]["ViewJC_KGStateMonth1_etime"];
                        dtHandled.Rows[i]["ViewJC_KGStateMonth1_etime"] = rightEndTime;
                        var startTime = dtHandled.Rows[i]["ViewJC_KGStateMonth1_stime"];
                        var duration = Convert.ToDateTime(rightEndTime) - Convert.ToDateTime(startTime);
                        dtHandled.Rows[i]["ViewJC_KGStateMonth1_duration"] = duration.ToString();
                        dtHandled.Rows.RemoveAt(i + 1);
                    }

                    currentPoint = point;
                    currentState = state;
                }

                //重算总次数、总时间
                var dtDis = dtHandled.DefaultView.ToTable(true, "ViewJC_KGStateMonth1_point", "ViewJC_KGStateMonth1_state");

                foreach (DataRow item in dtDis.Rows)
                {
                    var drs = dtHandled.Select("ViewJC_KGStateMonth1_point='" + item["ViewJC_KGStateMonth1_point"] + "' and ViewJC_KGStateMonth1_state='" + item["ViewJC_KGStateMonth1_state"] + "'");
                    var count = drs.Length;

                    TimeSpan sumTime = new TimeSpan(0);
                    foreach (var item2 in drs)
                    {
                        sumTime += TimeSpan.Parse(item2["ViewJC_KGStateMonth1_duration"].ToString());
                    }

                    foreach (DataRow item3 in dtHandled.Rows)
                    {
                        if (item3["ViewJC_KGStateMonth1_point"].ToString() == item["ViewJC_KGStateMonth1_point"].ToString() && item3["ViewJC_KGStateMonth1_state"].ToString() == item["ViewJC_KGStateMonth1_state"].ToString())
                        {
                            item3["ViewJC_KGStateMonth1_sumtime"] = sumTime.ToString();
                            item3["ViewJC_KGStateMonth1_sumcount"] = count.ToString();
                        }
                    }
                }

                //重新排序
                var orderInfo = _lmdDt.Select("lngOrder<>0");
                var orderInfoSort = orderInfo.OrderBy(a => Convert.ToInt32(a["lngOrder"])).ToList();
                var orderText = "";
                foreach (var item in orderInfoSort)
                {
                    var lngOrderMethod = item["lngOrderMethod"].ToString();
                    var order = lngOrderMethod == "1" ? "asc" : "desc";
                    orderText += item["MetaDataFieldName"] + " " + order + ",";
                }
                if (!string.IsNullOrEmpty(orderText))
                {
                    orderText = orderText.Substring(0, orderText.Length - 1);
                }
                dtHandled.DefaultView.Sort = orderText;

                dt = dtHandled.DefaultView.ToTable();
            }
            catch (Exception e)
            {
                dt = null;
                MessageShowUtil.ShowMsg("报表配置有误，请与管理员联系！");
                LogHelper.Error(e.ToString());
            }
        }

        private void tlbRefresh1_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshListData();
                blnRefreshed = true;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            NearQuery(-1);
            tlbRefresh_ItemClick(this, null);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            NearQuery(1);
            tlbRefresh_ItemClick(this, null);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (!PermissionManager.HavePermission("ExportReportData"))
            {
                return;
            }

            var strFileType = ".xls";
            Export(1, strFileType);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            if (simpleButton4.Text == "预览")
            {
                simpleButton4.Text = "取消";
                lookupShowStyle.EditValue = "pivot";
            }
            else
            {
                simpleButton4.Text = "预览";
                lookupShowStyle.EditValue = "list";
            }

            //限制左侧宽度  20180405
            layoutControlItem1.Width = 300;
            layoutControlItem2.Width = 300;

        }

        private void tlbRefresh1_Click_1(object sender, EventArgs e)
        {

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            try
            {
                SetExecuteBeginTime();
                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(0);
                    return;
                }


                if (PerPageRecord == 0 || TotalPage == 0)
                    return;

                CurrentPageNumber = 1;

                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                dt = Model.GetPageData(strListSql, 1, PerPageRecord);


                BandingData();

                SetListPageInfo();

                MessageShowUtil.ShowStaticInfo("加载第一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                SetExecuteBeginTime();
                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(documentViewer1.SelectedPageIndex - 1);
                    return;
                }


                if (PerPageRecord == 0)
                    return;

                var StartRecord = (CurrentPageNumber - 1) * PerPageRecord;
                if (StartRecord > PerPageRecord - 1)
                {
                    CurrentPageNumber--;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);


                    BandingData();

                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到第一页");
                }


                MessageShowUtil.ShowStaticInfo("加载上一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                // 20170622
                //if (tlbSetCurrentPage.EditValue == null)
                if (textEdit1.Text == null)
                {
                    return;
                }

                //var currPage = tlbSetCurrentPage.EditValue.ToString().Split('/');
                var currPage = textEdit1.Text.ToString().Split('/');

                CurrentPageNumber = TypeUtil.ToInt(currPage[0]);
                if (CurrentPageNumber > TotalPage)
                    CurrentPageNumber = TotalPage;
                else if ((0 == CurrentPageNumber) && (TotalPage > 0))
                    CurrentPageNumber = 1;

                var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                    "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                GetDistinctSql(ref strListSql);
                if (TotalPage <= 1)
                    dt = Model.GetDataTable(strListSql);
                else
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);
                BandingData();
                SetListPageInfo();
                if (lookupShowStyle.EditValue == "pivot")
                    SetReportPage(TypeUtil.ToInt(currPage[0]) - 1);
                MessageShowUtil.ShowStaticInfo("加载执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(documentViewer1.SelectedPageIndex + 1);
                    return;
                }

                if (PerPageRecord == 0)
                    return;

                var StartRecord = CurrentPageNumber * PerPageRecord;
                if (StartRecord < TotalRecord)
                {
                    CurrentPageNumber++;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);

                    BandingData();
                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到最后一页");
                }

                MessageShowUtil.ShowStaticInfo("加载下一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            try
            {
                SetExecuteBeginTime();

                if (lookupShowStyle.EditValue == "pivot")
                {
                    SetReportPage(rp.Pages.Count - 1);
                    return;
                }

                if (PerPageRecord == 0 || TotalPage == 0)
                    return;

                var StartRecord = (TotalPage - 1) * PerPageRecord;
                if (StartRecord < TotalRecord)
                {
                    CurrentPageNumber = TotalPage;

                    var strListSql = listDataExVo.StrListSQL.Replace("where 1=1",
                        "where 1=1 " + _strFreQryCondition + _strReceiveParaCondition + _strSortCondtion);
                    GetDistinctSql(ref strListSql);
                    dt = Model.GetPageData(strListSql, CurrentPageNumber, PerPageRecord);

                    BandingData();

                    SetListPageInfo();
                }
                else
                {
                    MessageShowUtil.ShowInfo("已到最后一页");
                }

                MessageShowUtil.ShowStaticInfo("加载最后一页的执行时间为" + GetExecuteTimeString(), barStaticItemMsg);
            }

            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            var strShowStyle = TypeUtil.ToString(lookupShowStyle.EditValue);
            if (strShowStyle == "pivot")
            {
                PrintPivot();
            }
            else
            {
                MessageShowUtil.ShowInfo("请先进行预览，再进行打印操作！");
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            try
            {
                if (lookupShowStyle.EditValue != null)
                {
                    // 20170821
                    if (dt == null)
                    {
                        MessageShowUtil.ShowMsg("请先查询数据。");
                        return;
                    }

                    DateTime dtRemark = GetRemarkTime();
                    var listDataRemarkInfo = Model.GetListDataRemarkByTimeListDataId(dtRemark, listDataExVo.ListDataID);

                    var frm = new frmInputDesc();
                    //frm.StrDesc = listDataExVo.StrListSrcSQL;

                    if (listDataRemarkInfo == null)
                    {
                        frm.StrDesc = "";
                    }
                    else
                    {
                        frm.StrDesc = listDataRemarkInfo.Remark;
                    }

                    frm.ShowDialog();
                    if (frm.BlnOK)
                    {
                        //listDataExVo.StrListSrcSQL = frm.StrDesc;
                        //listDataExVo.InfoState = InfoState.Modified;
                        //Model.SaveListDataEx(listDataExVo);
                        if (listDataRemarkInfo == null)
                        {
                            Model.AddListdataremark(listDataExVo.ListDataID, dtRemark, frm.StrDesc);
                        }
                        else
                        {
                            Model.UpdateListdataremarkByTimeListDataId(listDataExVo.ListDataID, dtRemark, frm.StrDesc);
                        }

                        listTemplevo = Model.GetListTemple(listDataExVo.ListDataID);
                        lookupShowStyle_EditValueChanged(null, null);
                        //MessageShowUtil.ShowInfo("备注保存成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            try
            {
                var rows = metadataFieldDt.Select("MetaDataID=" + listExvo.MainMetaDataID + " and blnDesignSort=1");
                if ((rows == null) || (rows.Length == 0))
                {
                    MessageShowUtil.ShowMsg("元数据里没有配置可编排的栏目，不能进行编排");
                    return;
                }
                var strFKCode = TypeUtil.ToString(rows[0]["strFkCode"]);
                var frm = new frmDataSortSet();
                frm.MetadataID = listExvo.MainMetaDataID;
                frm.dtLmd = _lmdDt;
                var strFileName =
                    _lmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["MetaDataFieldName"].ToString();
                frm.strFileName = strFileName;
                var strListDisplayName =
                    _lmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["strListDisplayFieldNameCHS"]
                        .ToString();
                frm.strListDisplayName = strListDisplayName;
                frm.strFKCode = strFKCode;
                frm.ListDataID = ListDataID;
                frm.ShowDialog();
                if (frm.blnOK)
                {
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }
    }

    public class FreQryCondition
    {
        private readonly Panel _panel;
        private readonly int controlHeight = 20;
        private int controlWidth;
        private readonly IDictionary<string, string> dicConditionOldTable = new Dictionary<string, string>();
        private readonly int intXSpacing = 5;
        private int intYSpacing = 15;
        private List<string> listsdate;
        private int panelWidth;

        public PanelControl PointFilter { get; set; }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="panel">条件面板</param>
        public FreQryCondition(Panel panel)
        {
            _panel = panel;

            if (_panel != null)
                panelWidth = _panel.Width;
        }

        /// <summary>
        ///     查询面板
        /// </summary>
        public Panel QryPanel
        {
            get { return _panel; }
        }

        public void CalcControlWidth(int cols)
        {
            controlWidth = (panelWidth - (cols + 1) * intXSpacing) / cols;
            // controlWidth = panelWidth - 5;
        }

        public void CreateControl(DataTable lmdDt)
        {
            if (lmdDt == null) return;
            if (_panel.Controls.Count > 0)
            {
                //_panel.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (_panel.Controls.Count > 0)
                {
                    if (_panel.Controls[0] != null)
                        _panel.Controls[0].Dispose();
                }
            }
            var dt = lmdDt.Copy();
            dt.DefaultView.RowFilter = "blnFreCondition=1";
            dt.DefaultView.Sort = "lngFreConIndex asc";
            dt = dt.DefaultView.ToTable();
            var count = dt.Rows.Count;
            int row;
            int col;
            var strFieldName = "";
            var strCurFieldNameCHS = "";
            var strFieldType = "";
            var strFkCode = "";
            var strValPara = "";
            var strDisplayPara = "";
            var blnPrintFreCondition = false;
            var lngFKType = 0;
            DataRow curDr = null;
            for (var i = 0; i < count; i++)
            {
                row = (i + 1) / 2;
                col = (i + 1) % 2;//20190119   /3
                //row = i;
                //col = 1;

                if (col == 0)
                    col = 2;//20190119   /3
                else
                    row = row + 1;

                curDr = dt.Rows[i];
                strFieldName = TypeUtil.ToString(curDr["MetaDataFieldName"]);
                strCurFieldNameCHS = TypeUtil.ToString(curDr["strListDisplayFieldNameCHS"]);
                strFieldType = TypeUtil.ToString(curDr["strFieldType"]);
                strFkCode = TypeUtil.ToString(curDr["strFkCode"]);
                strValPara = TypeUtil.ToString(curDr["strFreCondition"]);
                strDisplayPara = TypeUtil.ToString(curDr["strFreConditionCHS"]);
                blnPrintFreCondition = TypeUtil.ToBool(curDr["blnPrintFreCondition"]);
                lngFKType = TypeUtil.ToInt(curDr["lngFKType"]);
                if (strFkCode != string.Empty)
                {
                    CreateRefControl(strCurFieldNameCHS, strFieldName, row, col, strValPara, strDisplayPara, strFkCode,
                        blnPrintFreCondition, strFieldType, lngFKType);
                    continue;
                }

                strFieldType = strFieldType.ToLower();
                if ((strFieldType == "varchar") || (strFieldType == "nvarchar") || (strFieldType == "nchar") ||
                    (strFieldType == "char") || (strFieldType == "ntext"))
                    CreateTextControl(strCurFieldNameCHS, strFieldName, row, col, strValPara, blnPrintFreCondition);
                else if ((strFieldType == "money") || (strFieldType == "decimal") || (strFieldType == "float") ||
                         (strFieldType == "double")
                         || (strFieldType == "int") || (strFieldType == "smallint") || (strFieldType == "bigint") ||
                         (strFieldType == "tinyint"))
                    CreateNumberControl(strCurFieldNameCHS, strFieldName, row, col, strValPara, blnPrintFreCondition);
                else if (strFieldType == "bit")
                    CreateBooleanControl(strCurFieldNameCHS, strFieldName, row, col, strValPara, blnPrintFreCondition);
                else if ((strFieldType == "smalldatetime") || (strFieldType == "datetime"))
                    CreateDateTimeControl(strCurFieldNameCHS, strFieldName, row, col, strValPara, blnPrintFreCondition,
                        lngFKType);
            }
        }

        /// <summary>
        ///     设置控件坐标
        /// </summary>
        public void SetControlLocation()
        {
            int row;
            int col;
            panelWidth = _panel.Width;
            CalcControlWidth(2);//20190119
            for (var i = 0; i < _panel.Controls.Count; i++)
            {
                row = (i + 1) / 2;
                col = (i + 1) % 2;//20190119
                //row = i;
                //col = 1;
                if (col == 0)
                    col = 2;//20190119
                else
                    row = row + 1;
                _panel.Controls[i].Width = controlWidth;
                _panel.Controls[i].Location = GetLocation(row, col);
            }
        }

        /// <summary>
        ///     获取常用查询条件
        /// </summary>
        public string GetFreQryCondition()
        {
            dicConditionOldTable.Clear();
            var strFreWhere = ""; //常用查询条件
            var strFieldName = "";
            var strFieldType = "";
            var strFreCondition = "";
            var strFkCode = "";
            var controlCount = _panel.Controls.Count;
            if (controlCount > 0)
                for (var i = 0; i < controlCount; i++)
                {
                    strFieldName = "";
                    strFieldType = "";
                    strFreCondition = "";
                    strFkCode = "";
                    var hash = (Hashtable)_panel.Controls[i].Tag;
                    if (hash.ContainsKey("_fieldName"))
                        strFieldName = TypeUtil.ToString(hash["_fieldName"]);
                    if (strFieldName == string.Empty)
                        continue;
                    if (hash.ContainsKey("_fieldType"))
                        strFieldType = TypeUtil.ToString(hash["_fieldType"]);
                    if (strFieldType == string.Empty)
                        continue;

                    if (_panel.Controls[i] is UCRef || _panel.Controls[i] is UCGridLookUp)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) == string.Empty)
                            continue;

                        strFkCode = TypeUtil.ToString(hash["_fkCode"]);
                        if (!TypeUtil.ToString(hash["_fieldType"]).Contains("int"))
                            strFreCondition += "等于&&$" + "'" +
                                               TypeUtil.ToString(hash["_strDisplayValue"]).Replace(",", "','") + "'";
                        else
                            strFreCondition += "等于&&$" + TypeUtil.ToString(hash["_strKeyValue"]);
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strFreCondition = TypeUtil.ToString(hash["_strKeyValue"]);
                    }

                    var str = "";
                    if (strFreCondition != string.Empty)
                    {
                        var strfilename = strFieldName;
                        var index = strfilename.LastIndexOf("_");
                        if (index > -1)
                            strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");


                        if (strFkCode != string.Empty)
                            str = BulidConditionUtil.GetRefCondition(strfilename, strFreCondition, strFieldType);
                        else
                            str = BulidConditionUtil.GetConditionString(strfilename, strFieldType, strFreCondition);
                        if (str != string.Empty)
                        {
                            strFreWhere += " and " + str;

                            if (!str.ToLower().Contains("datsearch") && !str.Contains("state in"))
                            {
                                var strTableName = strfilename.Substring(0, strfilename.IndexOf(".") - 1);

                                // 20170705
                                //var strContion = str.Substring(0, str.IndexOf(".") - 1) +
                                //                 str.Substring(str.IndexOf("."));
                                int repIndex = strfilename.IndexOf(".");
                                string repStr = strfilename.Substring(0, repIndex);
                                var strContion = str.Replace(repStr, strTableName);

                                if (dicConditionOldTable.ContainsKey(strTableName))
                                    dicConditionOldTable[strTableName] += " and " + strContion;
                                else
                                    dicConditionOldTable.Add(strTableName, strContion);
                            }
                        }
                    }

                    if ((strFieldType == "smalldatetime") || (strFieldType == "datetime"))
                        listsdate = hash["_date"] as List<string>;
                }

            return strFreWhere;
        }

        /// <summary>
        ///     得到查询条件的中文串，用于打印的时候显示查询条件
        /// </summary>
        /// <returns></returns>
        public string GetFreQryCondtionByChs()
        {
            var strFreWhere = ""; //常用查询条件
            var strFieldName = "";
            var strFieldType = "";
            var strFreCondition = "";
            var strFkCode = "";
            var strFieldNameByChs = "";
            var controlCount = _panel.Controls.Count;
            var blnPrintFreCondition = false;

            if (controlCount > 0)
                for (var i = 0; i < controlCount; i++)
                {
                    strFieldName = "";
                    strFieldType = "";
                    strFreCondition = "";
                    strFkCode = "";
                    var hash = (Hashtable)_panel.Controls[i].Tag;
                    if (hash.ContainsKey("_fieldName"))
                        strFieldName = TypeUtil.ToString(hash["_fieldName"]);
                    if (strFieldName == string.Empty)
                        continue;
                    if (hash.ContainsKey("_fieldType"))
                        strFieldType = TypeUtil.ToString(hash["_fieldType"]);
                    if (strFieldType == string.Empty)
                        continue;
                    if (hash.ContainsKey("_strTitleName"))
                        strFieldNameByChs = TypeUtil.ToString(hash["_strTitleName"]);
                    if (strFieldNameByChs == string.Empty)
                        continue;

                    if (hash.ContainsKey("_blnPrintFreCondition"))
                        blnPrintFreCondition = TypeUtil.ToBool(hash["_blnPrintFreCondition"]);
                    if (!blnPrintFreCondition)
                        continue;


                    if (_panel.Controls[i] is UCRef)
                    {
                        if (TypeUtil.ToString(hash["_strKeyValue"]) == string.Empty)
                            continue;

                        strFkCode = TypeUtil.ToString(hash["_fkCode"]);
                        strFreCondition += "等于&&$" + TypeUtil.ToString(hash["_strDisplayValue"]);
                    }
                    else //UCDateTimeOne || UCText || UCNumber
                    {
                        strFreCondition = TypeUtil.ToString(hash["_strKeyValue"]);
                    }

                    var str = "";
                    if (strFreCondition != string.Empty)
                    {
                        var strfilename = strFieldName;
                        var index = strfilename.LastIndexOf("_");
                        if (index > -1)
                            strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");


                        if (strFkCode != string.Empty)
                            str = BulidConditionUtil.GetRefCondition(strfilename, strFreCondition, strFieldType);
                        else
                            str = BulidConditionUtil.GetConditionString(strfilename, strFieldType, strFreCondition);
                        if (str != string.Empty)
                            if (strFieldType == "datetime")
                            {
                                var strChsValue = TypeUtil.ToString(hash["_strValueChs"]);
                                strFreWhere += strFieldNameByChs + ":" + strChsValue + "  ";
                            }
                            else
                                strFreWhere += strFieldNameByChs + ":" + strFreCondition.Replace("&&$", "") + "     ";
                    }
                }

            return strFreWhere;
        }

        /// <summary>
        ///     得到日表日期段的日期集合
        /// </summary>
        /// <returns></returns>
        public List<string> GetDayDate()
        {
            return listsdate;
        }

        public IDictionary<string, string> GetConditionOldTable()
        {
            return dicConditionOldTable;
        }

        /// <summary>
        ///     创建参照型控件
        /// </summary>
        private void CreateRefControl(string titleName, string fieldName, int row, int col, string strValPara,
            string strDisplayPara, string fkCode, bool blnPrintFreCondition, string strFieldType, int RefType)
        {
            try
            {
                if (RefType <= 1)
                {
                    var control = new UCRef(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", strFieldType);
                    hash.Add("_fkCode", fkCode);
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strDisplayValue", strDisplayPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    control.Tag = hash;
                    control.FkCode = fkCode;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);
                    control.StrKeyValue = strValPara;
                    control.StrDisplayValue = strDisplayPara;
                    if (fkCode.Contains("AllPoint"))
                    {
                        control.PointFilter = PointFilter;
                    }
                }
                if (RefType == 2)
                {
                    //如果为2表示是下拉
                    var control = new UCGridLookUp(titleName, fkCode);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", strFieldType);
                    hash.Add("_fkCode", fkCode);
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strDisplayValue", strDisplayPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    control.Tag = hash;
                    control.FkCode = fkCode;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);
                    control.StrKeyValue = strValPara;
                    control.StrDisplayValue = strDisplayPara;
                }
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建参照控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建数值型控件
        /// </summary>
        private void CreateNumberControl(string titleName, string fieldName, int row, int col, string strValPara,
            bool blnPrintFreCondition)
        {
            try
            {
                var control = new UCNumber(titleName);
                var hash = new Hashtable();
                hash.Add("_fieldName", fieldName);
                hash.Add("_fieldType", "decimal");
                hash.Add("_strKeyValue", strValPara);
                hash.Add("_strTitleName", titleName);
                hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                control.HeaderValue = strValPara;
                control.Tag = hash;
                control.Width = controlWidth;
                control.Location = GetLocation(row, col);
                _panel.Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建参照控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建日期型控件
        /// </summary>
        private void CreateDateTimeControl(string titleName, string fieldName, int row, int col, string strValPara,
            bool blnPrintFreCondition, int RefType)
        {
            try
            {
                if (RefType <= 11)
                {
                    var control = new UCDateTimeTwo(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", "datetime");
                    strValPara = " " + "&&$" + DateTime.Now.ToShortDateString() + "&&$" +
                                 DateTime.Now.ToShortDateString();
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    hash.Add("_date", null);
                    hash.Add("_strValueChs", ""); //查询日期中文
                    control.Tag = hash;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);

                    control.HeaderValue = strValPara;
                }
                if (RefType == 12)
                {
                    //年月日 时分格式
                    var control = new UCDateTimeShiFen(titleName);
                    //UCDateTimeClass control = new UCDateTimeClass(titleName);
                    //UCDateTimeTwo control = new UCDateTimeTwo(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", "datetime");
                    strValPara = " " + "&&$" + DateTime.Now.ToShortDateString() + " 00:00" + "&&$" +
                                 DateTime.Now.ToShortDateString() + " 23:59";
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    hash.Add("_date", null);
                    hash.Add("_strValueChs", ""); //查询日期中文
                    control.Tag = hash;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);

                    control.HeaderValue = strValPara;
                }
                if (RefType == 13)
                {
                    //班次格式
                    var control = new UCDateTimeClass(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", "datetime");
                    strValPara = " " + "&&$" + DateTime.Now.ToShortDateString();
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    hash.Add("_date", null);
                    hash.Add("_strValueChs", ""); //查询日期中文
                    control.Tag = hash;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);

                    control.HeaderValue = strValPara;
                }
                if (RefType == 14)
                {
                    //月格式
                    var control = new UCDateTimeMonth(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", "datetime");
                    strValPara = " " + "&&$" + DateTime.Now.ToShortDateString() + "&&$" +
                                 DateTime.Now.ToShortDateString();
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    hash.Add("_date", null);
                    hash.Add("_strValueChs", ""); //查询日期中文
                    control.Tag = hash;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);

                    control.HeaderValue = strValPara;
                }
                if (RefType == 15)
                {
                    //季格式
                    //UCDateTimeYear control = new UCDateTimeYear(titleName);
                    //Hashtable hash = new Hashtable();
                    //hash.Add("_fieldName", fieldName);
                    //hash.Add("_fieldType", "datetime");
                    //strValPara = " " + "&&$" + DateTime.Now.ToShortDateString() + "&&$" + DateTime.Now.ToShortDateString();
                    //hash.Add("_strKeyValue", strValPara);
                    //hash.Add("_strTitleName", titleName);
                    //hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    //hash.Add("_date", null);
                    //hash.Add("_strValueChs", "");//查询日期中文
                    //control.Tag = hash;
                    //control.Width = controlWidth;
                    //control.Location = GetLocation(row, col);
                    //this._panel.Controls.Add(control);

                    //control.HeaderValue = strValPara;
                }
                if (RefType == 16)
                {
                    //年格式
                    var control = new UCDateTimeYear(titleName);
                    var hash = new Hashtable();
                    hash.Add("_fieldName", fieldName);
                    hash.Add("_fieldType", "datetime");
                    strValPara = " " + "&&$" + DateTime.Now.ToShortDateString() + "&&$" +
                                 DateTime.Now.ToShortDateString();
                    hash.Add("_strKeyValue", strValPara);
                    hash.Add("_strTitleName", titleName);
                    hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                    hash.Add("_date", null);
                    hash.Add("_strValueChs", ""); //查询日期中文
                    control.Tag = hash;
                    control.Width = controlWidth;
                    control.Location = GetLocation(row, col);
                    _panel.Controls.Add(control);

                    control.HeaderValue = strValPara;
                }
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建参照控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建逻辑型控件
        /// </summary>
        private void CreateBooleanControl(string titleName, string fieldName, int row, int col, string strValPara,
            bool blnPrintFreCondition)
        {
            try
            {
                var control = new UCBoolean(titleName);
                var hash = new Hashtable();
                hash.Add("_fieldName", fieldName);
                hash.Add("_fieldType", "bit");
                hash.Add("_strKeyValue", strValPara);
                hash.Add("_strTitleName", titleName);
                hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                control.Tag = hash;
                control.Width = controlWidth;
                control.StrKeyValue = strValPara;
                control.Location = GetLocation(row, col);
                _panel.Controls.Add(control);
            }
            catch (Exception e)
            {
                MessageBox.Show("创建逻辑型控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     创建字符型控件
        /// </summary>
        private void CreateTextControl(string titleName, string fieldName, int row, int col, string strValPara,
            bool blnPrintFreCondition)
        {
            try
            {
                var control = new UCTextTwo(titleName);
                var hash = new Hashtable();
                hash.Add("_fieldName", fieldName);
                hash.Add("_fieldType", "varchar");
                hash.Add("_strKeyValue", strValPara);
                hash.Add("_strTitleName", titleName);
                hash.Add("_blnPrintFreCondition", blnPrintFreCondition);
                control.Tag = hash;
                control.Width = controlWidth;
                control.Location = GetLocation(row, col);
                _panel.Controls.Add(control);

                control.HeaderValue = strValPara;
            }
            catch (Exception e)
            {
                MessageShowUtil.ShowInfo("创建参照控件出错！\n" + e.Message);
            }
        }

        /// <summary>
        ///     获取控件坐标
        /// </summary>
        /// <param name="row">行数</param>
        /// <param name="col">列数</param>
        /// <returns>Point</returns>
        private Point GetLocation(int row, int col)
        {
            int intX = 5, intY = 5;
            intYSpacing = 10; //2015-12-22  ，默认值是15，现改为10，以减少上下两行的距离
            intX = intX + (col - 1) * (controlWidth + intXSpacing);
            intY = intY + (row - 1) * (controlHeight + intYSpacing);
            return new Point(intX, intY);
        }
    }

    public class ListTitleEventArgs
    {
        /// <summary>
        ///     列表标题
        /// </summary>
        public string StrTitle = "";
    }
}