using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Model;
using Sys.Safety.Reports.PubClass;
using CellValueChangedEventArgs = DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs;

namespace Sys.Safety.Reports
{
    public partial class frmSchemaDesign : XtraForm
    {
        private string _strListName = "";
        private IDictionary<string, int> aliasNumDic = new Dictionary<string, int>();
        private DataTable curNodeDt; //当前树节点对应数据


        private GridHitInfo downHitInfo; //鼠标左键按下去时在GridView中的坐标
        private DataTable metadataDt;
        private DataTable metadataFieldDt;
        private readonly ListExModel Model = new ListExModel();
        private readonly IList<string> refCalcColList = new List<string>();
        private readonly IList<string> refContextContextColList = new List<string>();
        private int[] rows; //拖拽的所有行
        private int startRow; // 拖拽的第一
        private readonly IDictionary<string, DataTable> tableAliasDataDic = new Dictionary<string, DataTable>();
        private readonly DataTable treeDt = new DataTable();
        private GridHitInfo upHitInfo; //鼠标左键弹起来时在GridView中的坐标

        public frmSchemaDesign()
        {
            BlnOk = false;
            BlnListEnter = false;
            ListDisplayExList = null;
            LmdDt = null;
            ListDataExVo = null;
            MetadataId = 0;
            CurListDataId = 0;
            ListDataID = 0;
            ListID = 0;
            InitializeComponent();
        }

        /// <summary>
        ///     列表ID
        /// </summary>
        public int ListID { get; set; }

        /// <summary>
        ///     列表数据ID
        /// </summary>
        public int ListDataID { get; set; }

        /// <summary>
        ///     列表正在使用的方案ID
        /// </summary>
        public int CurListDataId { get; set; }

        /// <summary>
        ///     主实体ID
        /// </summary>
        public int MetadataId { get; set; }

        public ListdataexInfo ListDataExVo { get; set; }

        /// <summary>
        ///     选择列以及关联列数据
        /// </summary>
        public DataTable LmdDt { get; set; }

        public IList<ListdisplayexInfo> ListDisplayExList { get; set; }


        /// <summary>
        ///     列表进入
        /// </summary>
        public bool BlnListEnter { get; set; }

        public bool BlnOk { get; set; }


        public string StrListName
        {
            get { return _strListName; }
            set { _strListName = value; }
        }

        private void frmSchemaDesign_Load(object sender, EventArgs e)
        {
            try
            {
                //初始化树
                InitTree();

                //获取元数据描述
                GetMetaDataDesc();

                FillLookup();
                SetShowStyle();

                if (ListDataID <= 0)
                    CreateNewSchema();

                LoadSchema(true);
                SetControlVisible();
                MessageShowUtil.ShowStaticInfo("载入成功", barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     创建新方案
        /// </summary>
        private void CreateNewSchema()
        {
            ListDataExVo = new ListdataexInfo();
            ListDataExVo.InfoState = InfoState.AddNew;
        }

        /// <summary>
        ///     载入方案
        /// </summary>
        private void LoadSchema(bool blnFormLoad)
        {
            lookUpSchema.EditValueChanged -= lookUpSchema_EditValueChanged;
            lookUpSchema.EditValue = ListDataID;
            lookUpSchema.EditValueChanged += lookUpSchema_EditValueChanged;

            //获取表别名计数列表
            if (ListDataID > 0)
                aliasNumDic = Model.GetAliasNumDic(ListDataID);

            LoadTreeData();

            //获取选择记录
            if (blnFormLoad)
            {
                if (LmdDt == null)
                    LmdDt = Model.GetListMetaData(ListDataID, ListDataExVo == null ? 0 : ListDataExVo.UserID);

                if (ListDataID > 0)
                {
                    if (ListDataExVo == null)
                        ListDataExVo = Model.GetVO("listdatainfo", ListDataID) as ListdataexInfo;

                    ListDataExVo.InfoState = InfoState.Modified;
                }
            }
            else
            {
                if (ListDataID > 0)
                {
                    ListDataExVo = Model.GetVO("listdatainfo", ListDataID) as ListdataexInfo;
                    ListDataExVo.InfoState = InfoState.Modified;
                }

                LmdDt = Model.GetListMetaData(ListDataID, ListDataExVo.UserID);
            }

            SetSelectedColSrcFieldChs();
            SetRefCalcCol();
            SetRefContextContextCol();
            gcColSelected.DataSource = LmdDt;
            gcAdvanceSet.DataSource = LmdDt;
            gridControlCondition.DataSource = LmdDt;
            LmdDt.DefaultView.RowFilter = "isnull(blnSysProcess,0)=0";

            ShowGroupTitle();
            chkSmlSum.Checked = ListDataExVo.BlnSmlSum;
            lookUpSmlSumType.EditValue = ListDataExVo.LngSmlSumType;

            treeList1.SetFocusedNode(treeList1.Nodes.FirstNode);
            treeList1_Click(null, null);
        }

        /// <summary>
        ///     初始化树控件
        /// </summary>
        private void InitTree()
        {
            AddTreeDataColumn();
            SetTreeListProperty();
            treeList1.DataSource = treeDt;
            treeDt.DefaultView.RowFilter =
                "isnull(blnPK,0)<>1 or strFieldName='ReceiptTypeID' or strFieldName='OrderTypeID'";
        }

        /// <summary>
        ///     获取元数据描述
        /// </summary>
        private void GetMetaDataDesc()
        {
            metadataDt = ClientCacheModel.GetServerMetaData();
            metadataFieldDt = ClientCacheModel.GetServerMetaDataFields();
        }

        /// <summary>
        ///     添加元数据关联列数据源列
        /// </summary>
        private void AddTreeDataColumn()
        {
            treeDt.Columns.Add(new DataColumn("strId", Type.GetType("System.String"))); //节点唯一标识列
            treeDt.Columns.Add(new DataColumn("metaDataId", Type.GetType("System.Int32"))); //元数据ID lngRelativeFieldID
            treeDt.Columns.Add(new DataColumn("parentId", Type.GetType("System.Int32"))); //元数据父ID
            treeDt.Columns.Add(new DataColumn("strName", Type.GetType("System.String"))); //元数据名称
            treeDt.Columns.Add(new DataColumn("metaDataFieldId", Type.GetType("System.Int32"))); //元数据字段ID
            treeDt.Columns.Add(new DataColumn("lngParentFieldId", Type.GetType("System.Int32"))); //元数据父字段ID
            treeDt.Columns.Add(new DataColumn("lngRelativeFieldID", Type.GetType("System.Int32"))); //元数据关联字段ID
            treeDt.Columns.Add(new DataColumn("strTableAlias", Type.GetType("System.String"))); //表别名
            treeDt.Columns.Add(new DataColumn("strParentTableAlias", Type.GetType("System.String"))); //父表别名
            treeDt.Columns.Add(new DataColumn("lngAliasCount", Type.GetType("System.Int32"))); //别名计数lngAliasCount
            treeDt.Columns.Add(new DataColumn("lngNextAliasCount", Type.GetType("System.Int32"))); //下级节点别名计数
            treeDt.Columns.Add(new DataColumn("strFullPath", Type.GetType("System.String"))); //字段全路径描述
            treeDt.Columns.Add(new DataColumn("strParentFullPath", Type.GetType("System.String"))); //父级全路径描述
            treeDt.Columns.Add(new DataColumn("strNextFullPath", Type.GetType("System.String"))); //下级节点全路径描述

            treeDt.Columns.Add(new DataColumn("strFieldName", Type.GetType("System.String"))); //字段名
            treeDt.Columns.Add(new DataColumn("strFieldChName", Type.GetType("System.String"))); //字段中文名
            treeDt.Columns.Add(new DataColumn("strFieldType", Type.GetType("System.String"))); //字段类型
            treeDt.Columns.Add(new DataColumn("strFkCode", Type.GetType("System.String"))); //引用参照编码
            treeDt.Columns.Add(new DataColumn("lngSourceType", Type.GetType("System.Int32"))); //字段关联档案来源类型
            treeDt.Columns.Add(new DataColumn("lngRelativeID", Type.GetType("System.Int32"))); //关联元数据ID
            treeDt.Columns.Add(new DataColumn("blnPK", Type.GetType("System.Boolean"))); //是否为PK字段
            treeDt.Columns.Add(new DataColumn("lngDataRightType", Type.GetType("System.Int32"))); //数据权限类型   
            treeDt.Columns.Add(new DataColumn("strSummaryDisplayFormat", Type.GetType("System.String"))); //显示格式
        }

        /// <summary>
        ///     设置树控件属性
        /// </summary>
        private void SetTreeListProperty()
        {
            treeList1.KeyFieldName = "strTableAlias";
            treeList1.ParentFieldName = "strParentTableAlias";
            treeList1.OptionsBehavior.AllowIndeterminateCheckState = true;
            treeList1.OptionsView.ShowCheckBoxes = true;
            treeList1.OptionsView.ShowHorzLines = false;
            treeList1.OptionsView.ShowVertLines = false;
        }

        /// <summary>
        ///     添加树控件数据源列
        /// </summary>
        private void AddRelationDataColumn()
        {
            LmdDt.Columns.Add(new DataColumn("strFieldName", Type.GetType("System.String"))); //字段名
            LmdDt.Columns.Add(new DataColumn("strFieldChName", Type.GetType("System.String"))); //字段中文名
            LmdDt.Columns.Add(new DataColumn("lngRelativeID", Type.GetType("System.Int32"))); //关联元数据ID
        }

        /// <summary>
        ///     刷新方案数据
        /// </summary>
        private void RefreshSchemaDataSource()
        {
            LmdDt = Model.GetListMetaData(ListDataID, ListDataExVo == null ? 0 : ListDataExVo.UserID);
            SetSelectedColSrcFieldChs();
            SetRefCalcCol();
            SetRefContextContextCol();
            gcColSelected.DataSource = LmdDt;
            gcAdvanceSet.DataSource = LmdDt;
            gridControlCondition.DataSource = LmdDt;
            LmdDt.DefaultView.RowFilter = "isnull(blnSysProcess,0)=0";
        }

        private void SetSelectedColSrcFieldChs()
        {
            if (LmdDt == null) return;

            for (var i = 0; i < LmdDt.Rows.Count; i++)
                if (TypeUtil.ToBool(LmdDt.Rows[i]["isCalcField"]))
                    LmdDt.Rows[i]["strSrcFieldNameCHS"] = TypeUtil.ToString(LmdDt.Rows[i]["strListDisplayFieldNameCHS"]);
                else
                    LmdDt.Rows[i]["strSrcFieldNameCHS"] =
                        GetMetaDataFieldNameCHS(TypeUtil.ToInt(LmdDt.Rows[i]["MetaDataFieldID"]));
        }

        /// <summary>
        ///     设置引用列
        /// </summary>
        private void SetRefCalcCol()
        {
            if (LmdDt == null) return;

            refCalcColList.Clear();
            var strRefColList = string.Empty;
            for (var i = 0; i < LmdDt.Rows.Count; i++)
                if (TypeUtil.ToBool(LmdDt.Rows[i]["isCalcField"]))
                {
                    //加载引用列
                    strRefColList = TypeUtil.ToString(LmdDt.Rows[i]["strRefColList"]);
                    if (strRefColList == string.Empty) continue;

                    var strs = strRefColList.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var str in strs)
                    {
                        if (refCalcColList.Contains(str))
                            continue;

                        refCalcColList.Add(str);
                    }
                }
        }

        /// <summary>
        ///     设置上下文条件引用列
        /// </summary>
        private void SetRefContextContextCol()
        {
            if (ListDataExVo == null) return;

            refContextContextColList.Clear();
            if ((TypeUtil.ToString(ListDataExVo.StrConTextCondition) == string.Empty) ||
                (ListDataExVo.StrConTextCondition == null))
                return;

            var strConTextCondition = GetConTextConditionRefColString(ListDataExVo.StrConTextCondition);
            var strs = strConTextCondition.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in strs)
            {
                if (refContextContextColList.Contains(str))
                    continue;

                refContextContextColList.Add(str);
            }
        }

        private void FillLookup()
        {
            SetOrderMethodData();
            SetSummaryTypeData();
            SetDisplayFormat();
            SetApplyTypeData();
            SetFKTypeData();
            SetLngKeyGroupData();
            SetLngProivtTypeData();
            LoadShcemaList();
        }

        private void SetOrderMethodData()
        {
            var orderMethodDt = new DataTable();
            orderMethodDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            orderMethodDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            var dr = orderMethodDt.NewRow();
            dr["ID"] = 0;
            dr["Name"] = "";
            orderMethodDt.Rows.Add(dr);

            dr = orderMethodDt.NewRow();
            dr["ID"] = 1;
            dr["Name"] = "升序";
            orderMethodDt.Rows.Add(dr);

            dr = orderMethodDt.NewRow();
            dr["ID"] = 2;
            dr["Name"] = "降序";
            orderMethodDt.Rows.Add(dr);

            LookUpEditOrderMethod.DataSource = orderMethodDt;
        }

        private void SetSummaryTypeData()
        {
            var summaryTypeDt = new DataTable();
            summaryTypeDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            summaryTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<int, string> _dic = new Dictionary<int, string>();
            _dic.Add(0, "");
            _dic.Add(1, "汇总");
            _dic.Add(2, "最小值");
            _dic.Add(3, "最大值");
            _dic.Add(4, "计数");
            _dic.Add(5, "平均");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = summaryTypeDt.NewRow();
                dr["ID"] = key;
                dr["Name"] = _dic[key];
                summaryTypeDt.Rows.Add(dr);
            }

            LookUpEditSummaryType.DataSource = summaryTypeDt;
            lookUpSmlSumType.Properties.DataSource = summaryTypeDt;
        }

        private void SetDisplayFormat()
        {
            var displayFormatDt = new DataTable();
            displayFormatDt.Columns.Add("Name", Type.GetType("System.String"));

            IList<string> _list = new List<string>();
            _list.Add("");
            _list.Add("字符");
            _list.Add("数量");
            _list.Add("货币");
            _list.Add("重量");
            _list.Add("日期");
            _list.Add("时间");
            _list.Add("日期时间");

            DataRow dr = null;
            foreach (var str in _list)
            {
                dr = displayFormatDt.NewRow();
                dr["Name"] = str;
                displayFormatDt.Rows.Add(dr);
            }

            repositoryItemLookUpDisplayFormat.DataSource = displayFormatDt;
        }


        private void SetApplyTypeData()
        {
            var summaryTypeDt = new DataTable();
            summaryTypeDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            summaryTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<int, string> _dic = new Dictionary<int, string>();
            _dic.Add(0, "");
            _dic.Add(1, "全部");
            _dic.Add(2, "列表");
            _dic.Add(3, "报表");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = summaryTypeDt.NewRow();
                dr["ID"] = key;
                dr["Name"] = _dic[key];
                summaryTypeDt.Rows.Add(dr);
            }

            LookUpEditApplyType.DataSource = summaryTypeDt;
        }

        private void SetFKTypeData()
        {
            var summaryTypeDt = new DataTable();
            summaryTypeDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            summaryTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<int, string> _dic = new Dictionary<int, string>();
            _dic.Add(0, "");
            _dic.Add(1, "弹出");
            _dic.Add(2, "下拉");

            _dic.Add(11, "日期");
            _dic.Add(12, "日期+时间");
            _dic.Add(13, "班次");
            _dic.Add(14, "月");
            _dic.Add(15, "季");
            _dic.Add(16, "年");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = summaryTypeDt.NewRow();
                dr["ID"] = key;
                dr["Name"] = _dic[key];
                summaryTypeDt.Rows.Add(dr);
            }

            repLookUpFKType.DataSource = summaryTypeDt;
        }

        private void SetLngKeyGroupData()
        {
            var summaryTypeDt = new DataTable();
            summaryTypeDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            summaryTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<int, string> _dic = new Dictionary<int, string>();
            _dic.Add(0, "");
            //_dic.Add(1, "计数");
            _dic.Add(2, "汇总");
            _dic.Add(3, "平均");
            //_dic.Add(4, "最大");
            //_dic.Add(5, "最小");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = summaryTypeDt.NewRow();
                dr["ID"] = key;
                dr["Name"] = _dic[key];
                summaryTypeDt.Rows.Add(dr);
            }

            repGridLookUplLngKeyGroup.DataSource = summaryTypeDt;
        }

        private void SetLngProivtTypeData()
        {
            var summaryTypeDt = new DataTable();
            summaryTypeDt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            summaryTypeDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<int, string> _dic = new Dictionary<int, string>();
            _dic.Add(0, "");
            _dic.Add(1, "列区 ");
            _dic.Add(2, "行区");
            _dic.Add(3, "数据区");
            _dic.Add(4, "列区扩展");
            //_dic.Add(4, "最大");
            //_dic.Add(5, "最小");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = summaryTypeDt.NewRow();
                dr["ID"] = key;
                dr["Name"] = _dic[key];
                summaryTypeDt.Rows.Add(dr);
            }

            repGridLookUpLngProivtType.DataSource = summaryTypeDt;
        }

        private void LoadShcemaList()
        {
            var dt = Model.GetSchemaList(ListID);
            lookUpSchema.Properties.DataSource = dt;
            if ((ListDataID <= 0) && (dt != null) && (dt.Rows.Count > 0)) //默认第一个方案
                ListDataID = TypeUtil.ToInt(dt.Rows[0]["ListDataID"]);
        }

        /// <summary>
        ///     设置显示样式(条件格式)
        /// </summary>
        private void SetShowStyle()
        {
            if (!BlnListEnter)
            {
                tlbOk.Visibility = BarItemVisibility.Never;
                tlbCancel.Visibility = BarItemVisibility.Never;
            }

            var cn = new StyleFormatCondition(FormatConditionEnum.NotEqual, gridViewCondition.Columns["strCondition"],
                null, string.Empty);
            cn.Appearance.ForeColor = Color.Blue; //设置列的字体色
            cn.ApplyToRow = true;
            gridViewCondition.FormatConditions.Add(cn);

            var treelistSFC
                = new DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition(FormatConditionEnum.Greater,
                    treeList1.Columns["lngRelativeID"], null, 0);
            treelistSFC.Appearance.Font = new Font("Tahoma", 9, FontStyle.Bold);
            ; //设置列的字体           
            treelistSFC.ApplyToRow = true;
            treeList1.FormatConditions.Add(treelistSFC);


            //看有没有链接设置
            var cn1 = new StyleFormatCondition(FormatConditionEnum.NotEqual, gvAdvanceSet.Columns["strParaColName"],
                null, string.Empty);
            cn1.Appearance.ForeColor = Color.Blue; //设置列的字体色     
            cn1.ApplyToRow = true;
            gvAdvanceSet.FormatConditions.Add(cn1);


            //暂时控制
            //if (PermissionManager.HavePermission("QrerySchemaSQL"))
            //{
            //    this.layoutGroupDevelop.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //}

            //if (PermissionManager.HavePermission("EditContextCondition"))
            //{
            //    this.layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //}
            //else
            //{
            //    this.layoutControlItem20.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}
        }

        /// <summary>
        ///     显示标题
        /// </summary>
        private void ShowGroupTitle()
        {
            if (LmdDt != null)
                layoutGroupSelected.Text = "所选栏目(" + gvColSelected.RowCount + ")";
        }


        /// <summary>
        ///     显示信息
        /// </summary>
        /// <param name="caption">信息串</param>
        private void ShowMsg(string caption, bool isMsg)
        {
            MessageShowUtil.ShowStaticInfo(caption, barStaticItemMsg);
            if (isMsg)
                MessageShowUtil.ShowInfo(caption);
        }

        /// <summary>
        ///     获取别名数
        /// </summary>
        /// <param name="strKey">别名键</param>
        /// <returns>int</returns>
        private int GetAliasNum(string strKey)
        {
            var num = 0;
            if (aliasNumDic.ContainsKey(strKey))
                num = aliasNumDic[strKey];
            else
                for (var i = 1; i < 1000; i++)
                    if (!aliasNumDic.Values.Contains(i))
                    {
                        num = i;
                        aliasNumDic.Add(strKey, num);
                        break;
                    }

            return num;
        }

        /// <summary>
        ///     方案改变
        /// </summary>
        private void lookUpSchema_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var id = TypeUtil.ToInt(lookUpSchema.EditValue);
                if (id > 0)
                {
                    //载入方案

                    ClearSchema();

                    ListDataID = id;

                    LoadSchema(false);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     选择实体（新增列表才有效）
        /// </summary>
        private void tlbSelectMaster_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var frm = new frmMetaDataSelect();
                frm.ShowDialog();
                if (0 == frm.MetadataId)
                    return;

                MetadataId = frm.MetadataId;

                ClearList();

                LoadTreeData(); // 载入树控件数据
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     清空列表数据
        /// </summary>
        private void ClearList()
        {
            ListID = 0;
            ListDataID = 0;
            treeDt.Rows.Clear();
            if (curNodeDt != null) curNodeDt.Rows.Clear(); //当前树节点对应数据
            LmdDt.Rows.Clear();
            LmdDt.Rows.Clear(); //选择列
            aliasNumDic.Clear();
            tableAliasDataDic.Clear();
        }

        /// <summary>
        ///     清空方案数据
        /// </summary>
        private void ClearSchema()
        {
            ListDataID = 0;
            treeDt.Rows.Clear();
            if (curNodeDt != null) curNodeDt.Rows.Clear(); //当前树节点对应数据
            if (LmdDt != null) LmdDt.Rows.Clear();
            if (LmdDt != null) LmdDt.Rows.Clear(); //选择列
            aliasNumDic.Clear();
            tableAliasDataDic.Clear();
            refCalcColList.Clear(); //清空引用列
        }

        /// <summary>
        ///     载入树控件数据
        /// </summary>
        private void LoadTreeData()
        {
            try
            {
                //组织当前实体    
                var drs = metadataDt.Select("ID=" + MetadataId);
                var dr = treeDt.NewRow();
                dr["strId"] = "_0";
                dr["metaDataId"] = 0;
                dr["parentId"] = 0;
                dr["lngRelativeID"] = MetadataId;
                dr["strName"] = drs[0]["strName"];

                dr["lngParentFieldId"] = 0;
                dr["metaDataFieldId"] = 0;
                dr["lngRelativeFieldID"] = 0;

                dr["strParentFullPath"] = "";
                dr["strFullPath"] = "";
                dr["strNextFullPath"] = "0";

                var strKey = "0";
                var aliasNum = GetAliasNum(strKey);
                var tableName = TypeUtil.ToString(drs[0]["strTableName"]);

                var strTableAlias = tableName.Replace('[', 'B').Replace(']', 'E') + aliasNum;
                dr["strTableAlias"] = strTableAlias;
                dr["strParentTableAlias"] = "";
                dr["lngAliasCount"] = 0;
                dr["lngNextAliasCount"] = aliasNum;

                treeDt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     组织树控件数据（关联实体数据展开）
        /// </summary>
        /// <param name="parentMetaDataId">父节点元数据ID</param>
        /// <param name="lngParentRelativeID">父节点关联元数据ID</param>
        /// <param name="lngParentFieldId">父节点元数据字段ID</param>
        /// <param name="topFullPath">父节点字段全路径描述</param>
        /// <param name="topNextFullPath">父节点下级当前节点全路径描述</param>
        /// <param name="strParentTableAlias">父表别名</param>
        /// <param name="lngSourceType">有关联:1,无关联:2</param>
        /// <param name="lngParentAliasCount">父节点下级节点别名数</param>
        private void AddTreeData(int parentMetaDataId, int lngParentRelativeID, int lngParentFieldId, string topFullPath,
            string topNextFullPath,
            string strParentTableAlias, int lngTopNextAliasCount, DataRow fieldDr)
        {
            try
            {
                var lngSourceType = TypeUtil.ToInt(fieldDr["lngSourceType"]); //数据来源类型
                var curMetaDataId = TypeUtil.ToInt(fieldDr["MetaDataId"]); //当前元数据ID
                var metaDataFieldId = TypeUtil.ToInt(fieldDr["ID"]);
                var lngRelativeFieldID = TypeUtil.ToInt(fieldDr["lngRelativeFieldID"]);
                var strFieldName = TypeUtil.ToString(fieldDr["strFieldName"]);
                var strFieldChName = TypeUtil.ToString(fieldDr["strFieldChName"]);
                var strFieldType = TypeUtil.ToString(fieldDr["strFieldType"]); //字段类型
                var strFkCode = TypeUtil.ToString(fieldDr["strFkCode"]); //引用参照编码
                var lngRelativeID = TypeUtil.ToInt(fieldDr["lngRelativeID"]); //关联元数据ID
                var blnPK = TypeUtil.ToBool(fieldDr["blnPK"]); //是否为PK字段
                var lngDataRightType = TypeUtil.ToInt(fieldDr["lngDataRightType"]); //数据权限类型  
                var strSummaryDisplayFormat = TypeUtil.ToString(fieldDr["strDisplayFormat"]); //显示格式

                var dr = treeDt.NewRow();
                var strTableAlias = "";
                if (lngSourceType > 0)
                {
                    //有关联                   
                    //组织当前实体    
                    var drs = metadataDt.Select("ID=" + lngRelativeID);
                    if (drs.Length == 1)
                    {
                        dr["metaDataId"] = curMetaDataId;
                        dr["parentId"] = parentMetaDataId;
                        dr["strName"] = drs[0]["strName"];
                        if (strFieldChName != string.Empty)
                            dr["strName"] = strFieldChName;

                        dr["lngParentFieldId"] = lngParentFieldId;
                        dr["metaDataFieldId"] = metaDataFieldId;
                        dr["lngRelativeFieldID"] = lngRelativeFieldID;

                        var strKey = topNextFullPath + "_" + metaDataFieldId;

                        dr["strParentFullPath"] = topFullPath;
                        dr["strFullPath"] = topNextFullPath;
                        dr["strNextFullPath"] = strKey;
                        var aliasNum = GetAliasNum(strKey);
                        var tableName = TypeUtil.ToString(drs[0]["strTableName"]);

                        strTableAlias = tableName.Replace('[', 'B').Replace(']', 'E') + aliasNum;
                        dr["strTableAlias"] = strTableAlias;
                        dr["strParentTableAlias"] = strParentTableAlias;
                        dr["lngAliasCount"] = lngTopNextAliasCount;
                        dr["lngNextAliasCount"] = aliasNum;

                        dr["strId"] = strKey;
                    }
                    else
                    {
                        throw new Exception("元数据配置错误，请查看字段名[" + strFieldChName + "]的关联关系！");
                    }
                }
                else
                {
                    //无关联

                    dr["metaDataId"] = curMetaDataId;
                    dr["parentId"] = parentMetaDataId;
                    dr["strName"] = strFieldChName;

                    dr["lngParentFieldId"] = lngParentFieldId;
                    dr["metaDataFieldId"] = metaDataFieldId;
                    dr["lngRelativeFieldID"] = lngRelativeFieldID;

                    dr["strParentFullPath"] = topFullPath;
                    dr["strFullPath"] = topNextFullPath;
                    dr["strNextFullPath"] = "";

                    strTableAlias = strParentTableAlias + metaDataFieldId;
                    dr["strTableAlias"] = "";
                    dr["strParentTableAlias"] = strParentTableAlias;
                    dr["lngAliasCount"] = lngTopNextAliasCount;

                    dr["strId"] = topNextFullPath + "_" + metaDataFieldId;
                }

                dr["strFieldName"] = strFieldName;
                dr["strFieldChName"] = strFieldChName;
                dr["strFieldType"] = strFieldType;
                dr["strFkCode"] = strFkCode;
                dr["lngSourceType"] = lngSourceType;
                dr["lngRelativeID"] = lngRelativeID;
                dr["blnPK"] = blnPK;
                dr["lngDataRightType"] = lngDataRightType;
                dr["strSummaryDisplayFormat"] = strSummaryDisplayFormat;

                treeDt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void treeList1_Click(object sender, EventArgs e)
        {
            try
            {
                var curNode = treeList1.FocusedNode;
                var treeListField = new TreeListField();
                treeListField.metaDataId = TypeUtil.ToInt(curNode.GetValue("metaDataId"));
                treeListField.parentId = TypeUtil.ToInt(curNode.GetValue("parentId"));
                treeListField.lngRelativeID = TypeUtil.ToInt(curNode.GetValue("lngRelativeID"));
                treeListField.metaDataFieldId = TypeUtil.ToInt(curNode.GetValue("metaDataFieldId"));
                treeListField.strName = TypeUtil.ToString(curNode.GetValue("strName"));
                treeListField.lngAliasCount = TypeUtil.ToInt(curNode.GetValue("lngAliasCount"));
                treeListField.lngNextAliasCount = TypeUtil.ToInt(curNode.GetValue("lngNextAliasCount"));
                treeListField.strTableAlias = TypeUtil.ToString(curNode.GetValue("strTableAlias"));
                treeListField.strFullPath = TypeUtil.ToString(curNode.GetValue("strFullPath"));
                treeListField.strParentFullPath = TypeUtil.ToString(curNode.GetValue("strParentFullPath"));
                treeListField.strNextFullPath = TypeUtil.ToString(curNode.GetValue("strNextFullPath"));
                if ((treeListField.strTableAlias == "") || (treeListField.lngRelativeID == 0))
                    return;
                //展开下一级
                if (!tableAliasDataDic.ContainsKey(treeListField.strTableAlias))
                {
                    //未展开

                    var fieldDrs = metadataFieldDt.Select("blnHidden<>1 and MetaDataID=" + treeListField.lngRelativeID);
                    foreach (var fieldDr in fieldDrs)
                    {
                        //父字段ID（取来源字段ID改为取父节点字段ID）
                        var lngParentFieldId = 0;
                        if (curNode.ParentNode != null)
                            lngParentFieldId = TypeUtil.ToInt(curNode.ParentNode.GetValue("metaDataFieldId"));

                        AddTreeData(treeListField.metaDataId, treeListField.lngRelativeID, treeListField.metaDataFieldId,
                            treeListField.strFullPath, treeListField.strNextFullPath,
                            treeListField.strTableAlias, treeListField.lngNextAliasCount, fieldDr);
                    }
                }

                //显示并缓存当前节点栏目数据
                DataTable dt;
                if (tableAliasDataDic.ContainsKey(treeListField.strTableAlias))
                {
                    dt = tableAliasDataDic[treeListField.strTableAlias];
                }
                else
                {
                    var strFilter = "MetaDataID=" + treeListField.lngRelativeID;

                    //过滤掉无字段权限的数据blnFieldRight
                    //To

                    // 20170605
                    IDictionary<string, string> colNameAliasDic = new Dictionary<string, string>();
                    colNameAliasDic.Add("ID", "MetaDataFieldID"); //字段ID
                    colNameAliasDic.Add("MetaDataID", "MetaDataID"); //元数据ID
                    //colNameAliasDic.Add("strFieldName", "strFieldName"); //字段名
                    //colNameAliasDic.Add("strFieldType", "strFieldType"); //字段类型
                    //colNameAliasDic.Add("strFkCode", "strFkCode"); //引用参照编码
                    //colNameAliasDic.Add("strFieldChName", "strFieldChName"); //字段中文名
                    //colNameAliasDic.Add("lngSourceType", "lngSourceType"); //字段关联档案来源类型
                    //colNameAliasDic.Add("lngRelativeID", "lngRelativeID"); //关联元数据ID
                    //colNameAliasDic.Add("lngRelativeFieldID", "lngRelativeFieldID"); //关联元数据字段ID
                    //colNameAliasDic.Add("blnPK", "blnPK"); //是否为PK字段
                    //colNameAliasDic.Add("lngDataRightType", "lngDataRightType"); //数据权限类型         
                    colNameAliasDic.Add("StrFieldName", "strFieldName"); //字段名
                    colNameAliasDic.Add("StrFieldType", "strFieldType"); //字段类型
                    colNameAliasDic.Add("StrFkCode", "strFkCode"); //引用参照编码
                    colNameAliasDic.Add("StrFieldChName", "strFieldChName"); //字段中文名
                    colNameAliasDic.Add("LngSourceType", "lngSourceType"); //字段关联档案来源类型
                    colNameAliasDic.Add("LngRelativeID", "lngRelativeID"); //关联元数据ID
                    colNameAliasDic.Add("LngRelativeFieldID", "lngRelativeFieldID"); //关联元数据字段ID
                    colNameAliasDic.Add("BlnPK", "blnPK"); //是否为PK字段
                    colNameAliasDic.Add("LngDataRightType", "lngDataRightType"); //数据权限类型              

                    dt = GetPartialData(metadataFieldDt, strFilter, colNameAliasDic);

                    var dc = new DataColumn("blnSelect", Type.GetType("System.Boolean"));
                    dc.DefaultValue = false;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("lngParentID", Type.GetType("System.Int32")); //父元数据ID
                    dc.DefaultValue = treeListField.metaDataId;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("lngParentFieldID", Type.GetType("System.Int32")); //父元数据字段ID
                    dc.DefaultValue = treeListField.metaDataFieldId;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("lngAliasCount", Type.GetType("System.Int32")); //表计数
                    dc.DefaultValue = treeListField.lngNextAliasCount;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("strTableAlias", Type.GetType("System.String")); //表别名
                    dc.DefaultValue = treeListField.strTableAlias;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("strFullPath", Type.GetType("System.String"));
                    dc.DefaultValue = treeListField.strNextFullPath;
                    dt.Columns.Add(dc);

                    dc = new DataColumn("strParentFullPath", Type.GetType("System.String"));
                    dc.DefaultValue = treeListField.strFullPath;
                    dt.Columns.Add(dc);

                    tableAliasDataDic.Add(treeListField.strTableAlias, dt);

                    //恢复勾选记录
                    strFilter = "strFullPath='" + treeListField.strNextFullPath + "'";
                    strFilter += " and isnull(blnSysProcess,0)=0";
                    var selectedDr = LmdDt.Select(strFilter);
                    DataRow dr;
                    for (var i = 0; i < selectedDr.Length; i++)
                    {
                        dr = selectedDr[i];
                        if (!curNode.HasChildren)
                            break;
                        TreeListNode childNode = null;
                        for (var j = 0; j < curNode.Nodes.Count; j++)
                        {
                            childNode = curNode.Nodes[j];
                            if (TypeUtil.ToInt(dr["MetaDataFieldID"]) ==
                                TypeUtil.ToInt(childNode.GetValue("metaDataFieldId")))
                                childNode.CheckState = CheckState.Checked;
                        }
                    }

                    //隐藏PK字段
                    //dt.DefaultView.RowFilter = "isnull(blnPK,0)<>1 or strFieldName='ReceiptTypeID' or strFieldName='OrderTypeID'";
                }

                curNodeDt = dt; //记录当前节点记录数据

                //this.gcCurTable.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     获取数据（根据过滤条件）
        /// </summary>
        /// <param name="dt">需过滤表</param>
        /// <param name="strFilter">过滤条件</param>
        /// <param name="colNameAliasDic">返回列与别名</param>
        /// <returns>DataTable</returns>
        private DataTable GetPartialData(DataTable dt, string strFilter, IDictionary<string, string> colNameAliasDic)
        {
            DataTable returnDt = null;
            try
            {
                dt.DefaultView.RowFilter = strFilter;
                returnDt = dt.DefaultView.ToTable();

                var colName = "";
                var dcc = returnDt.Columns;
                for (var i = 0; i < dcc.Count; i++)
                {
                    colName = dcc[i].ColumnName;
                    if (!colNameAliasDic.ContainsKey(colName))
                    {
                        i--;
                        dcc.Remove(colName);
                    }
                    else
                    {
                        dcc[i].ColumnName = colNameAliasDic[colName];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }

            return returnDt;
        }

        private void treeList1_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            try
            {
                if (e.Node.Level <= 0)
                {
                    ShowMsg("根节点不能选择！", true);
                    e.CanCheck = false;
                    return;
                }

                e.State = e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            try
            {
                if (e.Node.Level <= 0)
                    return;

                //SetCheckedChildNodes(e.Node, e.Node.CheckState);
                //SetCheckedParentNodes(e.Node, e.Node.CheckState);

                var isSelect = e.Node.CheckState == CheckState.Checked;
                if (!isSelect)
                    if (e.Node.ParentNode != null)
                    {
                        var str = TypeUtil.ToString(e.Node.ParentNode.GetValue("strTableAlias")) + "_" +
                                  TypeUtil.ToString(e.Node.GetValue("strFieldName"));
                        if (refCalcColList.Contains(str)) //计算列引用，不能取消选择
                        {
                            e.Node.CheckState = CheckState.Checked;
                            ShowMsg("当前字段被部分计算列使用，不能取消选择！", true);
                            return;
                        }

                        if (refContextContextColList.Contains(str)) //上下文条件引用，不能取消选择
                        {
                            e.Node.CheckState = CheckState.Checked;
                            ShowMsg("当前字段被上下文条件使用，不能取消选择！", true);
                            return;
                        }
                    }

                SynchronousSelectCol(e.Node, isSelect);

                if (isSelect)
                {
                    if ((e.Node.ParentNode != null) && (e.Node.ParentNode.Level > 0))
                        AddRelativeCol(e.Node.ParentNode);
                }
                else
                {
                    if ((e.Node.ParentNode != null) && (e.Node.ParentNode.Level > 0))
                        CancelRelativeCol(e.Node.ParentNode);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (var i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                var b = false;
                CheckState state;
                for (var i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }


        private void chkSmlSum_CheckedChanged(object sender, EventArgs e)
        {
            var isVisble = chkSmlSum.Checked;
            gcblnSummary.Visible = isVisble;
            if (isVisble)
                gcblnSummary.VisibleIndex = 6;
        }

        private void checkEditSelect_CheckedChanged(object sender, EventArgs e)
        {
            gvAdvanceSet.CloseEditor();
            gvAdvanceSet.UpdateCurrentRow();
        }

        private void CheckEditFreCondition_CheckedChanged(object sender, EventArgs e)
        {
            gvColSelected.CloseEditor();
            gvColSelected.UpdateCurrentRow();
        }

        /// <summary>
        ///     高级设置
        /// </summary>
        private void gvAdvanceSet_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                gvAdvanceSet.CellValueChanged -= gvAdvanceSet_CellValueChanged;

                if ("lngOrderMethod" == e.Column.FieldName)
                {
                    //选择列发生改变

                    var isSelect = TypeUtil.ToBool(gvAdvanceSet.GetRowCellValue(e.RowHandle, "lngOrderMethod"));
                    if (isSelect)
                    {
                        if (TypeUtil.ToInt(gvAdvanceSet.GetRowCellValue(e.RowHandle, "lngOrder")) <= 0)
                        {
                            var selectCount = GetSelectFieldRecordCount(gvAdvanceSet, "lngOrderMethod"); //选择记录数
                            gvAdvanceSet.SetRowCellValue(e.RowHandle, "lngOrder", selectCount);
                        }
                    }
                    else
                    {
                        ReSetSelectFieldIndex(gvAdvanceSet, "lngOrderMethod", "lngOrder",
                            TypeUtil.ToInt(gvAdvanceSet.GetRowCellValue(e.RowHandle, "lngOrder")));
                        gvAdvanceSet.SetRowCellValue(e.RowHandle, "lngOrder", 0);
                    }
                }
                else if ("blnSummary" == e.Column.FieldName)
                {
                    var blnSummary = TypeUtil.ToBool(gvAdvanceSet.GetRowCellValue(e.RowHandle, "blnSummary"));
                    if (blnSummary)
                    {
                        var strFieldType = TypeUtil.ToString(gvAdvanceSet.GetRowCellValue(e.RowHandle, "strFieldType"));
                        if ((strFieldType != "money") && (strFieldType != "decimal") && (strFieldType != "float")
                            && (strFieldType != "int") && (strFieldType != "smallint") && (strFieldType != "bigint") &&
                            (strFieldType != "tinyint"))
                        {
                            gvAdvanceSet.SetRowCellValue(e.RowHandle, "blnSummary", false);
                            ShowMsg("数值型栏目才能小计！", true);
                        }
                    }
                }

                gvAdvanceSet.CellValueChanged += gvAdvanceSet_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void gvAdvanceSet_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //int rowHandle = e.FocusedRowHandle;
            //string strFieldType = TypeUtil.ToString(this.gvAdvanceSet.GetRowCellValue(rowHandle, "strFieldType"));
            //if (strFieldType != "money" && strFieldType != "decimal" && strFieldType != "float"
            //                && strFieldType != "int" && strFieldType != "smallint" && strFieldType != "bigint")
            //{
            //    this.LookUpEditSummaryType.View.ActiveFilterString = "Name<>'汇总' and Name<>'平均'";
            //}
            //else
            //{
            //    this.LookUpEditSummaryType.View.ActiveFilterString = "";
            //}
        }

        private void gvColSelected_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                var rowHandle = e.FocusedRowHandle;
                var MetaDataFieldID = TypeUtil.ToInt(gvColSelected.GetRowCellValue(rowHandle, "MetaDataFieldID"));
                var rows = metadataFieldDt.Select("ID=" + MetaDataFieldID + " and len(strFkCode)>0");
                var rowsa = metadataFieldDt.Select("ID=" + MetaDataFieldID + " and strFieldType like  '%datetime%'");


                if ((rows != null) && (rows.Length > 0))
                    return;
                if ((rowsa != null) && (rowsa.Length > 0))
                    return;

                gcFKType.OptionsColumn.ReadOnly = false;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     所选栏目
        /// </summary>
        private void gvColSelected_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                gvColSelected.CellValueChanged -= gvColSelected_CellValueChanged;
                if ("blnFreCondition" == e.Column.FieldName)
                {
                    //是否为常用条件

                    var isFreCondition = TypeUtil.ToBool(gvColSelected.GetRowCellValue(e.RowHandle, "blnFreCondition"));
                    if (isFreCondition)
                    {
                        if (TypeUtil.ToBool(gvColSelected.GetRowCellValue(e.RowHandle, "isCalcField")))
                        {
                            //计算列不能设置为常用条件

                            gvColSelected.SetRowCellValue(e.RowHandle, "blnFreCondition", false);
                            ShowMsg("计算列不能设置为常用条件！", true);
                        }
                        else
                        {
                            var selectCount = GetSelectFieldRecordCount(gvColSelected, "blnFreCondition"); //选择记录数
                            if (selectCount > 30)
                            {
                                gvColSelected.SetRowCellValue(e.RowHandle, "blnFreCondition", false);
                                ShowMsg("常用条件不能多于30个！", true);
                            }
                            else
                            {
                                gvColSelected.SetRowCellValue(e.RowHandle, "lngFreConIndex", selectCount);
                            }
                        }
                    }
                    else
                    {
                        ReSetSelectFieldIndex(gvColSelected, "blnFreCondition", "lngFreConIndex",
                            TypeUtil.ToInt(gvColSelected.GetRowCellValue(e.RowHandle, "lngFreConIndex")));
                        gvColSelected.SetRowCellValue(e.RowHandle, "lngFreConIndex", 0);
                    }
                }

                gvColSelected.CellValueChanged += gvColSelected_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private int GetSelectFieldRecordCount(GridView gv, string selectFieldName)
        {
            var selectCount = 0; //选择记录数
            var rowCount = gv.RowCount;
            for (var i = 0; i < rowCount; i++)
                if (TypeUtil.ToBool(gv.GetRowCellValue(i, selectFieldName)))
                    selectCount++;

            return selectCount;
        }

        private void ReSetSelectFieldIndex(GridView gv, string selectFieldName, string indexFieldName, int curIndex)
        {
            var rowCount = gv.RowCount;
            var index = 0;
            for (var i = 0; i < rowCount; i++)
            {
                index = TypeUtil.ToInt(gv.GetRowCellValue(i, indexFieldName));
                if (TypeUtil.ToBool(gv.GetRowCellValue(i, selectFieldName)) && (index > curIndex))
                    gv.SetRowCellValue(i, indexFieldName, index - 1);
            }
        }

        /// <summary>
        ///     添加关联列
        /// </summary>
        /// <param name="curNode">当前树节点</param>
        private void AddRelativeCol(TreeListNode curNode)
        {
            try
            {
                var treeListField = new TreeListField();
                treeListField.metaDataId = TypeUtil.ToInt(curNode.GetValue("metaDataId"));
                treeListField.parentId = TypeUtil.ToInt(curNode.GetValue("parentId"));
                treeListField.metaDataFieldId = TypeUtil.ToInt(curNode.GetValue("metaDataFieldId"));
                treeListField.lngParentFieldId = TypeUtil.ToInt(curNode.GetValue("lngParentFieldId"));
                treeListField.lngRelativeFieldID = TypeUtil.ToInt(curNode.GetValue("lngRelativeFieldID"));
                treeListField.strName = TypeUtil.ToString(curNode.GetValue("strName"));
                treeListField.lngAliasCount = TypeUtil.ToInt(curNode.GetValue("lngAliasCount"));
                treeListField.strTableAlias = TypeUtil.ToString(curNode.GetValue("strTableAlias"));
                treeListField.strFullPath = TypeUtil.ToString(curNode.GetValue("strFullPath"));
                treeListField.strParentFullPath = TypeUtil.ToString(curNode.GetValue("strParentFullPath"));

                treeListField.strTableAlias = TypeUtil.ToString(curNode.GetValue("strTableAlias")); //表别名 
                treeListField.strFieldName = TypeUtil.ToString(curNode.GetValue("strFieldName"));
                treeListField.strFieldChName = TypeUtil.ToString(curNode.GetValue("strFieldChName"));
                treeListField.strFieldType = TypeUtil.ToString(curNode.GetValue("strFieldType"));
                treeListField.strFkCode = TypeUtil.ToString(curNode.GetValue("strFkCode"));
                treeListField.lngSourceType = TypeUtil.ToInt(curNode.GetValue("lngSourceType"));
                treeListField.lngRelativeID = TypeUtil.ToInt(curNode.GetValue("lngRelativeID"));
                treeListField.blnPK = TypeUtil.ToBool(curNode.GetValue("blnPK"));
                treeListField.lngDataRightType = TypeUtil.ToInt(curNode.GetValue("lngDataRightType"));
                treeListField.strSummaryDisplayFormat = TypeUtil.ToString(curNode.GetValue("strSummaryDisplayFormat"));

                if (curNode.ParentNode != null)
                    treeListField.strTableAlias = TypeUtil.ToString(curNode.ParentNode.GetValue("strTableAlias"));

                var strFilter = "strFullPath='" + treeListField.strFullPath + "'";
                strFilter += " and isnull(MetaDataFieldID,0)=" + treeListField.metaDataFieldId;
                if (LmdDt.Select(strFilter).Length <= 0)
                {
                    //创建关联                

                    //添加关联列字段
                    var dr = LmdDt.NewRow();
                    dr["ListDataID"] = ListDataID;
                    dr["MetaDataFieldID"] = treeListField.metaDataFieldId;
                    dr["MetaDataID"] = treeListField.metaDataId; //元数据ID
                    dr["strFieldType"] = treeListField.strFieldType; //字段类型
                    dr["strFkCode"] = treeListField.strFkCode; //  引用参照编码
                    dr["strListDisplayFieldNameCHS"] = treeListField.strFieldChName; //字段中文名
                    dr["lngSourceType"] = treeListField.lngSourceType; //字段关联档案来源类型
                    dr["lngRelativeFieldID"] = treeListField.lngRelativeFieldID; //关联元数据字段ID

                    dr["lngParentID"] = treeListField.parentId; //父元数据ID
                    dr["lngParentFieldID"] = treeListField.lngParentFieldId; //父元数据字段ID
                    dr["lngAliasCount"] = treeListField.lngAliasCount; //表计数


                    dr["MetaDataFieldName"] = treeListField.strTableAlias + "_" + treeListField.strFieldName;
                    dr["strTableAlias"] = treeListField.strTableAlias; //表别名 
                    dr["strFullPath"] = treeListField.strFullPath;
                    dr["strParentFullPath"] = treeListField.strParentFullPath;
                    dr["strSrcFieldNameCHS"] = GetMetaDataFieldNameCHS(treeListField.metaDataFieldId) + "."
                                               + TypeUtil.ToString(dr["strListDisplayFieldNameCHS"]);

                    dr["strSummaryDisplayFormat"] = treeListField.strSummaryDisplayFormat;

                    dr["blnSysProcess"] = true;
                    dr["lngKeyFieldType"] = 1;
                    dr["blnShow"] = false;

                    LmdDt.Rows.Add(dr);

                    //处理父节点关联栏目
                    if ((curNode.ParentNode != null) && (curNode.ParentNode.Level > 0))
                        AddRelativeCol(curNode.ParentNode);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     删除关联列
        /// </summary>
        /// <param name="curNode">当前树节点</param>
        private void CancelRelativeCol(TreeListNode curNode)
        {
            try
            {
                var strFullPath = TypeUtil.ToString(curNode.GetValue("strFullPath"));
                var strNextFullPath = TypeUtil.ToString(curNode.GetValue("strNextFullPath"));
                var metaDataFieldId = TypeUtil.ToInt(curNode.GetValue("metaDataFieldId"));

                //判断当前节点是否有关联栏目记录
                var strFilter = "blnSysProcess=0 and strFullPath like '" + strNextFullPath + "%'";
                var relatoinDrs = LmdDt.Select(strFilter);

                if (relatoinDrs.Length <= 0)
                {
                    //当前节点数据无选择栏目 且无关联选择栏目 方可取消栏目节点

                    strFilter = "strFullPath='" + strFullPath + "'";
                    strFilter += " and isnull(MetaDataFieldID,0)=" + metaDataFieldId;
                    relatoinDrs = LmdDt.Select(strFilter);
                    if (relatoinDrs.Length > 0)
                        if (TypeUtil.ToBool(relatoinDrs[0]["blnSysProcess"])) //如果不是栏目字段
                            LmdDt.Rows.Remove(relatoinDrs[0]);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     同步选择栏目数据
        /// </summary>
        /// <param name="curNode">当前节点</param>
        /// <param name="isSelect">是否选择</param>
        private void SynchronousSelectCol(TreeListNode curNode, bool isSelect)
        {
            try
            {
                var treeListField = new TreeListField();
                treeListField.metaDataId = TypeUtil.ToInt(curNode.GetValue("metaDataId"));
                treeListField.parentId = TypeUtil.ToInt(curNode.GetValue("parentId"));
                treeListField.metaDataFieldId = TypeUtil.ToInt(curNode.GetValue("metaDataFieldId"));
                treeListField.lngParentFieldId = TypeUtil.ToInt(curNode.GetValue("lngParentFieldId"));
                treeListField.lngRelativeFieldID = TypeUtil.ToInt(curNode.GetValue("lngRelativeFieldID"));
                treeListField.strName = TypeUtil.ToString(curNode.GetValue("strName"));
                treeListField.lngAliasCount = TypeUtil.ToInt(curNode.GetValue("lngAliasCount"));
                treeListField.strTableAlias = TypeUtil.ToString(curNode.GetValue("strTableAlias"));
                treeListField.strFullPath = TypeUtil.ToString(curNode.GetValue("strFullPath"));
                treeListField.strParentFullPath = TypeUtil.ToString(curNode.GetValue("strParentFullPath"));

                treeListField.strTableAlias = TypeUtil.ToString(curNode.GetValue("strTableAlias")); //表别名 
                treeListField.strFieldName = TypeUtil.ToString(curNode.GetValue("strFieldName"));
                treeListField.strFieldChName = TypeUtil.ToString(curNode.GetValue("strFieldChName"));
                treeListField.strFieldType = TypeUtil.ToString(curNode.GetValue("strFieldType"));
                treeListField.strFkCode = TypeUtil.ToString(curNode.GetValue("strFkCode"));
                treeListField.lngSourceType = TypeUtil.ToInt(curNode.GetValue("lngSourceType"));
                treeListField.lngRelativeID = TypeUtil.ToInt(curNode.GetValue("lngRelativeID"));
                treeListField.blnPK = TypeUtil.ToBool(curNode.GetValue("blnPK"));
                treeListField.lngDataRightType = TypeUtil.ToInt(curNode.GetValue("lngDataRightType"));
                treeListField.strSummaryDisplayFormat = TypeUtil.ToString(curNode.GetValue("strSummaryDisplayFormat"));

                if (curNode.ParentNode != null)
                    treeListField.strTableAlias = TypeUtil.ToString(curNode.ParentNode.GetValue("strTableAlias"));
                var strFilter = "strFullPath='" + treeListField.strFullPath + "'";
                strFilter += " and isnull(MetaDataFieldID,0)=" + treeListField.metaDataFieldId;
                var drs = LmdDt.Select(strFilter);

                if (isSelect)
                {
                    //添加当前行
                    if ((drs != null) && (drs.Length > 0))
                    {
                        drs[0]["blnSysProcess"] = false;
                        drs[0]["blnShow"] = true;
                        drs[0]["strFkCode"] = treeListField.strFkCode;
                        drs[0]["lngSourceType"] = treeListField.lngSourceType;

                        var copyDr = LmdDt.NewRow();
                        foreach (DataColumn dc in LmdDt.Columns)
                            copyDr[dc.ColumnName] = drs[0][dc.ColumnName];
                        LmdDt.Rows.Remove(drs[0]);
                        LmdDt.Rows.Add(copyDr);

                        ShowGroupTitle();
                        return;
                    }

                    //添加关联列字段
                    var dr = LmdDt.NewRow();
                    dr["ListDataID"] = ListDataID;
                    dr["MetaDataFieldID"] = treeListField.metaDataFieldId;
                    dr["MetaDataID"] = treeListField.metaDataId; //元数据ID
                    dr["strFieldType"] = treeListField.strFieldType; //字段类型
                    dr["strFkCode"] = treeListField.strFkCode; //  引用参照编码
                    dr["strListDisplayFieldNameCHS"] = treeListField.strFieldChName; //字段中文名
                    dr["lngSourceType"] = treeListField.lngSourceType; //字段关联档案来源类型     
                    dr["lngParentID"] = treeListField.parentId; //父元数据ID
                    dr["lngRelativeFieldID"] = treeListField.lngRelativeFieldID; //关联元数据字段ID
                    dr["lngParentFieldID"] = treeListField.lngParentFieldId; //父元数据字段ID
                    dr["lngAliasCount"] = treeListField.lngAliasCount; //表计数
                    dr["MetaDataFieldName"] = treeListField.strTableAlias + "_" + treeListField.strFieldName;
                    dr["strTableAlias"] = treeListField.strTableAlias; //表别名 
                    dr["strFullPath"] = treeListField.strFullPath;
                    dr["strParentFullPath"] = treeListField.strParentFullPath;
                    dr["strSrcFieldNameCHS"] = GetMetaDataFieldNameCHS(treeListField.lngParentFieldId) + "."
                                               + treeListField.strFieldChName;

                    dr["strSummaryDisplayFormat"] = treeListField.strSummaryDisplayFormat;

                    dr["blnSysProcess"] = false;
                    dr["blnShow"] = true;

                    dr["strCondition"] = string.Empty;
                    dr["strConditionCHS"] = string.Empty;
                    dr["strParaColName"] = string.Empty;
                    LmdDt.Rows.Add(dr);
                }
                else
                {
                    //删除当前行

                    if ((drs != null) && (drs.Length > 0))
                        if (TypeUtil.ToInt(drs[0]["lngKeyFieldType"]) <= 0) //如果是关键字段，不能删除
                        {
                            LmdDt.Rows.Remove(drs[0]);
                        }
                        else
                        {
                            drs[0]["blnSysProcess"] = true;
                            drs[0]["blnShow"] = false;
                            drs[0]["blnFreCondition"] = false;
                            drs[0]["lngSourceType"] = treeListField.lngSourceType;
                        }
                }

                ShowGroupTitle();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private string GetMetaDataFieldNameCHS(int fieldId)
        {
            var strFieldNameChs = "";
            if (fieldId == 0)
            {
                strFieldNameChs = TypeUtil.ToString(treeList1.Nodes.FirstNode.GetValue("strName"));
            }
            else
            {
                var drs = metadataFieldDt.Select("ID=" + fieldId);
                if ((drs != null) && (drs.Length > 0))
                {
                    strFieldNameChs = TypeUtil.ToString(drs[0]["strFieldChName"]);

                    var lmdDrs = LmdDt.Select("MetaDataFieldID=" + fieldId);
                    if ((lmdDrs != null) && (lmdDrs.Length > 0))
                    {
                        var lngParentFieldID = TypeUtil.ToInt(lmdDrs[0]["lngParentFieldID"]);
                        strFieldNameChs = GetMetaDataFieldNameCHS(lngParentFieldID) + "." + strFieldNameChs;
                    }
                }
            }

            return strFieldNameChs;
        }

        /// <summary>
        ///     更新元数据缓存
        /// </summary>
        private void btnUpdateMetaDataCache_Click(object sender, EventArgs e)
        {
            try
            {
                ClientCacheModel.UpdateMetaDataCache();
                GetMetaDataDesc();
                MessageShowUtil.ShowStaticInfo("更新成功！", barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     生成SQL
        /// </summary>
        private void btnBuildSql_Click(object sender, EventArgs e)
        {
            memoEditSQL.Text = "";

            try
            {
                var dt = GetListMetaDataTable();
                var strListSQL = Model.GetSQL(dt);
                if ((TypeUtil.ToString(ListDataExVo.StrConTextCondition) != string.Empty) &&
                    (ListDataExVo.StrConTextCondition != null))
                {
                    var strs = ListDataExVo.StrConTextCondition.Split(new[] {"&&&$$$"},
                        StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 2)
                        strListSQL = strListSQL.Replace("where 1=1",
                            "where 1=1 " + " and " + strs[1].Trim().Replace('_', '.'));
                }

                strListSQL = RequestUtil.ProcessDynamicParameters(strListSQL);
                memoEditSQL.Text = strListSQL;

                MessageShowUtil.ShowStaticInfo("生成成功！", barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     校验SQL
        /// </summary>
        private void btnTestSql_Click(object sender, EventArgs e)
        {
            try
            {
                if (memoEditSQL.Text == string.Empty)
                {
                    MessageShowUtil.ShowInfo("校验SQL不能为空！");
                    return;
                }

                //为提高效率，预览数据只显示100
                var strsql = memoEditSQL.Text;


                if (Model.GetDbType() == "mysql")
                    strsql = strsql + " LIMIT 0,10";
                if (Model.GetDbType() == "sqlserver")
                    strsql = "select * from (" + strsql.Insert(6, " top 100 ") + ") as A";
                var dt = Model.TestSql(strsql);
                gridControlPreView.DataSource = null;
                gridViewPreView.Columns.Clear();
                gridControlPreView.DataSource = dt;
                gridViewPreView.BestFitColumns();


                MessageShowUtil.ShowStaticInfo("校验成功，返回记录数为" + dt.Rows.Count, barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     移到首行
        /// </summary>
        private void btnMoveFirst_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColSelected.IsFirstRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是首行！");
                    return;
                }

                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                var copyDr = LmdDt.NewRow();
                foreach (DataColumn dc in LmdDt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];
                LmdDt.Rows.Remove(focuseDr);
                LmdDt.Rows.InsertAt(copyDr, 0);
                gvColSelected.FocusedRowHandle = 0;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     上移
        /// </summary>
        private void btnMovePrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColSelected.IsFirstRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是首行！");
                    return;
                }

                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                var copyDr = LmdDt.NewRow();
                foreach (DataColumn dc in LmdDt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];

                var curRowIndex = LmdDt.Rows.IndexOf(focuseDr); //处理隐藏行记录             
                for (var insertRow = curRowIndex - 1; insertRow >= 0; insertRow--)
                    if (!TypeUtil.ToBool(LmdDt.Rows[insertRow]["blnSysProcess"]))
                    {
                        LmdDt.Rows.Remove(focuseDr);
                        LmdDt.Rows.InsertAt(copyDr, insertRow);
                        break;
                    }
                gvColSelected.FocusedRowHandle = focusedRowHandle - 1;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     下移
        /// </summary>
        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColSelected.IsLastRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是末行！");
                    return;
                }

                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                var copyDr = LmdDt.NewRow();
                foreach (DataColumn dc in LmdDt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];

                var curRowIndex = LmdDt.Rows.IndexOf(focuseDr); //处理隐藏行记录               
                for (var insertRow = curRowIndex + 1; insertRow < LmdDt.Rows.Count; insertRow++)
                    if (!TypeUtil.ToBool(LmdDt.Rows[insertRow]["blnSysProcess"]))
                    {
                        LmdDt.Rows.Remove(focuseDr);
                        LmdDt.Rows.InsertAt(copyDr, insertRow);
                        break;
                    }
                gvColSelected.FocusedRowHandle = focusedRowHandle + 1;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     移到末行
        /// </summary>
        private void btnMoveLast_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColSelected.IsLastRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是末行！");
                    return;
                }

                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                var copyDr = LmdDt.NewRow();
                foreach (DataColumn dc in LmdDt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];
                LmdDt.Rows.Remove(focuseDr);
                LmdDt.Rows.Add(copyDr);
                gvColSelected.FocusedRowHandle = gvColSelected.RowCount - 1;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     删除行
        /// </summary>
        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                var rowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();

                if (focuseDr == null)
                {
                    MessageShowUtil.ShowInfo("请选择行记录！");
                    return;
                }

                CheckDeleteRow(focuseDr);

                var isCalcField = TypeUtil.ToBool(focuseDr["isCalcField"]);
                if (isCalcField)
                {
                    ShowMsg("无编辑计算列的权限！", true);
                    return;
                }

                if (DialogResult.No ==
                    MessageBox.Show("确认要删除当前列吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    return;

                if (isCalcField)
                {
                    LmdDt.Rows.Remove(focuseDr); //计算列，直接删除
                    ShowGroupTitle();
                    SetRefCalcCol();
                    return;
                }

                //查找记录对应树节点并取消节点选择状态、对应关联列（关联列可以保存时取消）
                //如果当前列是关联列，不能删除，设置状态即可。
                var strFullPath = TypeUtil.ToString(focuseDr["strFullPath"]);
                var metaDataFieldId = TypeUtil.ToInt(focuseDr["metaDataFieldId"]);
                var strId = strFullPath + "_" + metaDataFieldId;
                var curNode = treeList1.FindNodeByFieldValue("strId", strId);
                if (curNode != null)
                    if (curNode.CheckState == CheckState.Checked)
                        curNode.CheckState = CheckState.Unchecked;

                var strFilter = "blnSysProcess=0 and strFullPath like '" + strId + "%'";
                var relatoinDrs = LmdDt.Select(strFilter);
                if ((relatoinDrs != null) && (relatoinDrs.Length > 0))
                {
                    focuseDr["blnSysProcess"] = true;
                    focuseDr["blnShow"] = false;
                }
                else
                {
                    LmdDt.Rows.Remove(focuseDr); //末级节点，直接删除
                }
            }
            catch (Exception ex)
            {
                ShowMsg(ex.Message, true);
            }
        }

        /// <summary>
        ///     删除行校验
        /// </summary>
        /// <param name="dr"></param>
        private void CheckDeleteRow(DataRow dr)
        {
            var str = TypeUtil.ToString(dr["MetaDataFieldName"]);
            if (refCalcColList.Contains(str)) //计算列引用，不能删除
                throw new Exception("当前字段被部分计算列使用，不能删除！");
            if (refContextContextColList.Contains(str)) //上下文条件引用，不能删除
                throw new Exception("当前字段被上下文条件使用，不能删除！");
        }

        /// <summary>
        ///     添加计算列
        /// </summary>
        private void btnAddCalcCol_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SetReportCaclColumn"))
                    return;
                var frm = new frmSchemaAddCalc();
                frm.SelectedDt = LmdDt.Copy();
                frm.ShowDialog();
                if (frm.BlnOk)
                {
                    var dr = LmdDt.NewRow();

                    dr["ListDataID"] = ListDataID;
                    dr["strFieldType"] = frm.Lmd.StrFieldType;
                    dr["MetaDataFieldName"] = frm.Lmd.MetaDataFieldName;
                    dr["strSrcFieldNameCHS"] = frm.StrFieldDispaly;
                    dr["strListDisplayFieldNameCHS"] = frm.StrFieldDispaly;

                    dr["blnSysProcess"] = false;
                    dr["blnShow"] = true;

                    dr["isCalcField"] = true;
                    dr["strFormula"] = frm.Lmd.StrFormula;
                    dr["strRefColList"] = frm.Lmd.StrRefColList;

                    dr["strCondition"] = string.Empty;
                    dr["strConditionCHS"] = string.Empty;
                    dr["strParaColName"] = string.Empty;
                    LmdDt.Rows.Add(dr);

                    SetRefCalcCol();
                    ShowGroupTitle();

                    btnBuildSql_Click(null, null);
                    btnTestSql_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     编辑计算列
        /// </summary>
        private void btnEditCalcCol_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SetReportCaclColumn"))
                    return;
                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                if (focuseDr == null)
                {
                    ShowMsg("请选择要编辑的计算列！", true);
                    return;
                }
                if (!TypeUtil.ToBool(focuseDr["isCalcField"]))
                {
                    ShowMsg("选择的不是计算列！", true);
                    return;
                }

                var frm = new frmSchemaAddCalc();
                frm.SelectedDt = LmdDt.Copy();
                frm.BlnEdit = true;
                frm.Lmd.StrFieldType = TypeUtil.ToString(focuseDr["strFieldType"]);
                frm.Lmd.MetaDataFieldName = TypeUtil.ToString(focuseDr["MetaDataFieldName"]);
                frm.StrFieldDispaly = TypeUtil.ToString(focuseDr["strListDisplayFieldNameCHS"]);
                frm.Lmd.StrFormula = TypeUtil.ToString(focuseDr["strFormula"]);
                frm.Lmd.StrRefColList = TypeUtil.ToString(focuseDr["strRefColList"]);
                frm.ShowDialog();
                if (frm.BlnOk)
                {
                    focuseDr["ListDataID"] = ListDataID;
                    focuseDr["strFieldType"] = frm.Lmd.StrFieldType;
                    focuseDr["MetaDataFieldName"] = frm.Lmd.MetaDataFieldName;
                    focuseDr["strSrcFieldNameCHS"] = frm.StrFieldDispaly;
                    focuseDr["strListDisplayFieldNameCHS"] = frm.StrFieldDispaly;

                    focuseDr["blnSysProcess"] = false;
                    focuseDr["blnShow"] = true;

                    focuseDr["isCalcField"] = true;
                    focuseDr["strFormula"] = frm.Lmd.StrFormula;
                    focuseDr["strRefColList"] = frm.Lmd.StrRefColList;

                    focuseDr["strCondition"] = string.Empty;
                    focuseDr["strConditionCHS"] = string.Empty;

                    SetRefCalcCol();
                    btnBuildSql_Click(null, null);
                    btnTestSql_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        /// <summary>
        ///     删除计算列
        /// </summary>
        private void btnDelCalcCol_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SetReportCaclColumn"))
                    return;
                var focusedRowHandle = gvColSelected.FocusedRowHandle;
                var focuseDr = gvColSelected.GetFocusedDataRow();
                if (focuseDr == null)
                {
                    ShowMsg("请选择要删除的计算列！", true);
                    return;
                }
                if (!TypeUtil.ToBool(focuseDr["isCalcField"]))
                {
                    ShowMsg("选择的不是计算列！", true);
                    return;
                }

                if (DialogResult.No ==
                    MessageBox.Show("确认要删除当前列吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                    return;

                LmdDt.Rows.Remove(focuseDr);
                ShowGroupTitle();
                SetRefCalcCol();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        /// <summary>
        ///     上下文条件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContextConditionSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SetReportContenxt"))
                {
                    return;
                }
                var frm = new frmConTextCondition();
                frm.SelectedDt = LmdDt.Copy();
                var strConTextCondition = ListDataExVo.StrConTextCondition == null
                    ? ""
                    : ListDataExVo.StrConTextCondition;
                frm.Lmd.StrFormula = GetConTextConditionString(strConTextCondition);
                frm.ShowDialog();
                if (frm.BlnOk)
                {
                    if (frm.Lmd.StrFormula == string.Empty)
                        ListDataExVo.StrConTextCondition = "";
                    else
                        ListDataExVo.StrConTextCondition = frm.Lmd.StrRefColList + "&&&$$$" + frm.Lmd.StrFormula;

                    SetRefContextContextCol();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     获取上下条件串
        /// </summary>
        /// <returns></returns>
        private string GetConTextConditionString(string str)
        {
            if (str == string.Empty)
                return str;

            var strs = str.Split(new[] {"&&&$$$"}, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length != 2)
                return string.Empty;

            return strs[1].Trim();
        }

        /// <summary>
        ///     获取上下条件串引用列串
        /// </summary>
        /// <returns></returns>
        private string GetConTextConditionRefColString(string str)
        {
            if (str == string.Empty)
                return str;

            var strs = str.Split(new[] {"&&&$$$"}, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length != 2)
                return string.Empty;

            return strs[0].Trim();
        }

        /// <summary>
        ///     获取列表元数据数据
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetListMetaDataTable()
        {
            DataTable dt = null;
            DataRow dr = null;

            dt = LmdDt;
            var strFilter = "";

            //添加每个表的PK字段
            DataRow[] childDrs = null;
            var strFullPath = string.Empty;
            foreach (var key in tableAliasDataDic.Keys)
            {
                strFilter = "strTableAlias='" + key + "'";
                childDrs = dt.Select(strFilter);
                if (childDrs.Length <= 0)
                    continue;

                strFullPath = TypeUtil.ToString(childDrs[0]["strFullPath"]);
                strFilter = "blnSysProcess=0 and strFullPath like'" + strFullPath + "%'";
                if (dt.Select(strFilter).Length <= 0)
                {
                    //不存在此表、以及子表栏目字段

                    //删除冗余字段
                    strFilter = "strFullPath like'" + strFullPath + "%'";
                    childDrs = dt.Select(strFilter);
                    for (var i = 0; i < childDrs.Length; i++)
                        dt.Rows.Remove(childDrs[i]);

                    continue;
                }

                var keyDt = tableAliasDataDic[key];
                var keyDrs = keyDt.Select("blnPK=1");
                foreach (var keyDr in keyDrs)
                {
                    strFilter = "isnull(MetaDataFieldID,0)=" + TypeUtil.ToInt(keyDr["MetaDataFieldID"]);
                    strFilter += " and isnull(strFullPath,'')='" + TypeUtil.ToString(keyDr["strFullPath"]) + "'";

                    var drs = dt.Select(strFilter);
                    if (drs.Length <= 0)
                    {
                        //添加数据

                        dr = dt.NewRow();
                        foreach (DataColumn dc in keyDt.Columns)
                            if ("strFieldChName" == dc.ColumnName)
                                dr["strListDisplayFieldNameCHS"] = keyDr[dc.ColumnName];
                            else if (("strFieldName" != dc.ColumnName) && ("lngDataRightType" != dc.ColumnName)
                                        && ("blnPK" != dc.ColumnName) && ("lngRelativeID" != dc.ColumnName) &&
                                        ("blnSelect" != dc.ColumnName))
                                dr[dc.ColumnName] = keyDr[dc.ColumnName];

                        dr["MetaDataFieldName"] = key + "_" + TypeUtil.ToString(keyDr["strFieldName"]);

                        dr["blnSysProcess"] = true;
                        dr["blnShow"] = false;
                        dr["lngKeyFieldType"] = 2;
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        drs[0]["lngKeyFieldType"] = 2;
                    }
                }
            }

            //添加每个表的数据权限字段
            foreach (var key in tableAliasDataDic.Keys)
            {
                strFilter = "strTableAlias='" + key + "'";
                childDrs = dt.Select(strFilter);
                if (childDrs.Length <= 0)
                    continue;

                var keyDt = tableAliasDataDic[key];
                var keyDrs = keyDt.Select("isnull(lngDataRightType,0)>0");
                foreach (var keyDr in keyDrs)
                {
                    strFilter = "isnull(MetaDataFieldID,0)=" + TypeUtil.ToInt(keyDr["MetaDataFieldID"]);
                    strFilter += " and isnull(strFullPath,'')='" + TypeUtil.ToString(keyDr["strFullPath"]) + "'";

                    var drs = dt.Select(strFilter);
                    if (drs.Length <= 0)
                    {
                        //添加数据

                        dr = dt.NewRow();
                        foreach (DataColumn dc in keyDt.Columns)
                            if ("strFieldChName" == dc.ColumnName)
                                dr["strListDisplayFieldNameCHS"] = keyDr[dc.ColumnName];
                            else if (("strFieldName" != dc.ColumnName) && ("lngDataRightType" != dc.ColumnName)
                                        && ("blnPK" != dc.ColumnName) && ("lngRelativeID" != dc.ColumnName) &&
                                        ("blnSelect" != dc.ColumnName))
                                dr[dc.ColumnName] = keyDr[dc.ColumnName];

                        dr["MetaDataFieldName"] = key + "_" + TypeUtil.ToString(keyDr["strFieldName"]);

                        dr["blnSysProcess"] = true;
                        dr["blnShow"] = false;

                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        private void gvAdvanceSet_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gvColSelected_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridViewPreView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        private void gridViewCondition_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        /// <summary>
        ///     条件设置
        /// </summary>
        private void ButtonEditCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (0 != e.Button.Index)
                return;

            var rowHandle = gridViewCondition.FocusedRowHandle;
            var curDr = gridViewCondition.GetDataRow(rowHandle);
            if (curDr == null)
                return;

            try
            {
                if (!PermissionManager.HavePermission("SetReportFixCon"))
                {
                    return;
                }
                var frm = new frmSchemaCondition();
                frm.CurFieldNameCHS = TypeUtil.ToString(curDr["strListDisplayFieldNameCHS"]);
                frm.StrFieldType = TypeUtil.ToString(curDr["strFieldType"]);
                frm.FkCode = TypeUtil.ToString(curDr["strFkCode"]);
                frm.StrCondition = TypeUtil.ToString(curDr["strCondition"]);
                frm.StrConditionCHS = TypeUtil.ToString(curDr["strConditionCHS"]);
                frm.ShowDialog();

                if (frm.BlnOk)
                {
                    curDr["strCondition"] = frm.StrCondition;
                    curDr["strConditionCHS"] = frm.StrConditionCHS;
                    curDr["strConditionShow"] = frm.StrConditionCHS.Replace("&&$$", ";").Replace("&&$", ",");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     超链接设置
        /// </summary>
        private void ButtonEditHyperlink_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (0 != e.Button.Index)
                return;

            var rowHandle = gvAdvanceSet.FocusedRowHandle;
            var curDr = gvAdvanceSet.GetDataRow(rowHandle);
            if (curDr == null)
                return;

            try
            {
                //if (TypeUtil.ToBool(curDr["isCalcField"]))
                //{
                //    this.ShowMsg("计算列不能进行超链接设置！", true);
                //    return;
                //}

                var strTableAlias = TypeUtil.ToString(curDr["strTableAlias"]);
                var dt = GetListMetaDataTable().Copy();

                var frm = new frmListParameterSet();
                frm.DataSource = dt;
                frm.StrTableAlias = strTableAlias;
                frm.Model = Model;
                frm.StrFieldNameList = TypeUtil.ToString(curDr["strParaColName"]);
                frm.StrHyperlink = TypeUtil.ToString(curDr["strHyperlink"]);
                // frm.StrWebHyperlink = TypeUtil.ToString(curDr["strWebHyperlink"]);
                frm.LngHyperLinkType = TypeUtil.ToInt(curDr["lngHyperLinkType"]);
                frm.BlnLink = true;
                frm.ShowDialog();

                if (frm.BlnOk)
                {
                    curDr["strParaColName"] = frm.StrFieldNameList;
                    curDr["strHyperlink"] = frm.StrHyperlink;
                    // curDr["strWebHyperlink"] = frm.StrWebHyperlink;
                    curDr["lngHyperLinkType"] = frm.LngHyperLinkType;
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     条件格式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSFCSetting_Click(object sender, EventArgs e)
        {
            if (!PermissionManager.HavePermission("SetReportStyle"))
            {
                return;
            }

            var rowHandle = gridViewCondition.FocusedRowHandle;
            var curDr = gridViewCondition.GetDataRow(rowHandle);
            if (curDr == null)
            {
                ShowMsg("请选择栏目！", true);
                return;
            }

            if (TypeUtil.ToString(curDr["strFieldType"]) == "bit")
            {
                ShowMsg("逻辑型栏目不能设置！", true);
                return;
            }

            try
            {
                if (!PermissionManager.HavePermission("SetReportStyle"))
                    return;
                var frm = new frmListExSFC();
                frm.CurFieldNameCHS = TypeUtil.ToString(curDr["strListDisplayFieldNameCHS"]);
                frm.StrFieldType = TypeUtil.ToString(curDr["strFieldType"]);
                frm.StrCondition = TypeUtil.ToString(curDr["strConditionFormat"]);
                frm.ShowDialog();
                if (frm.BlnOk)
                {
                    curDr["strConditionFormat"] = frm.StrCondition;
                    curDr["strConditionFormatShow"] = frm.StrCondition.Replace("&&$$", ";").Replace("&&$", ",");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void frmSchemaDesign_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        /// <summary>
        ///     鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvColSelected_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                downHitInfo = gvColSelected.CalcHitInfo(new Point(e.X, e.Y)); //鼠标左键按下去时在GridView中的坐标
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvColSelected_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left) return; //不是左键则无效
                if ((downHitInfo == null) || (downHitInfo.RowHandle < 0)) return; //判断鼠标的位置是否有效

                var rowsselect = gvColSelected.GetSelectedRows(); //获取所选行的index


                var list = new ArrayList();
                foreach (var i in rowsselect)
                {
                    var drs = LmdDt.Select("ID=" + TypeUtil.ToInt(gvColSelected.GetRowCellValue(i, "ID")));
                    var index = LmdDt.Rows.IndexOf(drs[0]); //获取拖拽的目标行index，由于有隐藏行，所以要去找数据源的索引
                    list.Add(index);
                }

                rows = new int[list.Count];
                for (var i = 0; i < list.Count; i++)
                    rows[i] = TypeUtil.ToInt(list[i]);


                startRow = rows.Length == 0 ? -1 : rows[0];
                var dt = LmdDt.Clone();

                foreach (var r in rows) // 根据所选行的index进行取值，去除所选的行数据，可能是选取的多行
                {
                    var dataSourcerows = r;
                    dt.ImportRow(LmdDt.Rows[dataSourcerows]); //保存所选取的行数据
                }
                gcColSelected.DoDragDrop(dt, DragDropEffects.Move); //开始拖放操作，将拖拽的数据存储起来
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     拖拽过程事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcColSelected_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }


        /// <summary>
        ///     拖拽完成后事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcColSelected_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var gridviewPoint = PointToScreen(gcColSelected.Location); //获取鼠标在屏幕上的位置。
                upHitInfo = gvColSelected.CalcHitInfo(new Point(e.X - gridviewPoint.X, e.Y - gridviewPoint.Y));
                    //鼠标左键弹起来时在GridView中的坐标（屏幕位置减去 gridView 开始位置）
                if ((upHitInfo == null) || (upHitInfo.RowHandle < 0))
                    if (upHitInfo.RowHandle == -2147483648)
                        upHitInfo.RowHandle = gvColSelected.RowCount - 1;
                    else
                        return;
                else
                    upHitInfo.RowHandle -= 1;

                var drs = LmdDt.Select("ID=" + TypeUtil.ToInt(gvColSelected.GetRowCellValue(upHitInfo.RowHandle, "ID")));
                var endRow = LmdDt.Rows.IndexOf(drs[0]); //获取拖拽的目标行index


                var dt = e.Data.GetData(typeof(DataTable)) as DataTable;
                    //获取要移动的数据，从拖拽保存的地方：（gridControl1.DoDragDrop(dt, DragDropEffects.Move); ）


                if ((dt != null) && (dt.Rows.Count > 0)) //拖拽的数据为空
                {
                    int a;
                    var xs = LmdDt.Rows[endRow]; //获取拖拽的目标行，准备进行移植
                    if (!rows.Contains(endRow)) //如果多选的话，确保不能拖拽到这几个里
                    {
                        gvColSelected.ClearSelection(); //从GirdView中删除所拖拽的数据
                        var moveValue = 0;
                        foreach (var i in rows)
                        {
                            LmdDt.Rows.Remove(LmdDt.Rows[i - moveValue]); //从GirdView的数据源中删除所拖拽的数据
                            moveValue++;
                        }

                        if (startRow > endRow)
                            a = LmdDt.Rows.IndexOf(xs); //若果往上托，则加在鼠标到达行的上面
                        else
                            a = LmdDt.Rows.IndexOf(xs) + 1; //如果往下拖，则加在鼠标到达行的下面
                        var j = 0;
                        DataRow drTemp;
                        foreach (DataRow dr in dt.Rows)
                        {
                            drTemp = LmdDt.NewRow();
                            foreach (DataColumn dc in dr.Table.Columns)
                                drTemp[dc.ColumnName] = dr[dc.ColumnName];
                            LmdDt.Rows.InsertAt(drTemp, a + j); //将拖拽的数据再次添加进来


                            for (var i = 0; i < gvColSelected.RowCount; i++)
                            {
//拖动的数据源目标ID和gridviewf进行比较,如果相等则设置gridview行选择
                                var id = TypeUtil.ToInt(gvColSelected.GetRowCellValue(i, "ID"));
                                var iddrag = TypeUtil.ToInt(drTemp["ID"]);
                                if (id == iddrag)
                                    gvColSelected.SelectRow(i);
                            }


                            // gvColSelected.SelectRow(a + j);
                            j++;
                        }
                    }


                    //gcColSelected.DataSource = _lmdDt; //重新绑定
                    //gvColSelected.RefreshData();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void gvColSelected_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnDeleteRow_Click(null, null);
        }


        /// <summary>
        ///     模糊显示设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBluerConditionSet_Click(object sender, EventArgs e)
        {
            if (!PermissionManager.HavePermission("SetReportBlurr"))
            {
                return;
            }
            var rowHandle = gridViewCondition.FocusedRowHandle;
            var curDr = gridViewCondition.GetDataRow(rowHandle);
            if (curDr == null)
            {
                ShowMsg("请选择栏目！", true);
                return;
            }

            if (TypeUtil.ToString(curDr["strFieldType"]) == "bit")
            {
                ShowMsg("逻辑型栏目不能设置！", true);
                return;
            }

            var frm = new frmBluerCondition();
            frm.StrCondition = TypeUtil.ToString(curDr["strBluerCondition"]);
            frm.SelectedDt = LmdDt.Copy();
            frm.ShowDialog();
            if (frm.BlnOk)
                curDr["StrBluerCondition"] = frm.StrCondition;
        }


        /// <summary>
        ///     右键菜单添加计算列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddCalcCol_Click(object sender, EventArgs e)
        {
            btnAddCalcCol_Click(null, null);
        }

        /// <summary>
        ///     右键编辑计算列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbEditCalcCol_Click(object sender, EventArgs e)
        {
            btnEditCalcCol_Click(null, null);
        }


        /// <summary>
        ///     右键删除计算列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDelCalcCol_Click(object sender, EventArgs e)
        {
            btnDelCalcCol_Click(null, null);
        }


        /// <summary>
        ///     右键条件格式设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSFCSetting_Click(object sender, EventArgs e)
        {
            btnSFCSetting_Click(null, null);
        }

        /// <summary>
        ///     右键上下文条件设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbContextConditionSet_Click(object sender, EventArgs e)
        {
            btnContextConditionSet_Click(null, null);
        }

        private void tlbBluerConditionSet_Click(object sender, EventArgs e)
        {
            btnBluerConditionSet_Click(null, null);
        }


        /// <summary>
        ///     栏目顺序编排
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbColumnSort_Click(object sender, EventArgs e)
        {
            var rowHandle = gridViewCondition.FocusedRowHandle;
            var curDr = gridViewCondition.GetDataRow(rowHandle);
            var strFileName = TypeUtil.ToString(curDr["MetaDataFieldName"]);
            var strListDisplayName = TypeUtil.ToString(curDr["strListDisplayFieldNameCHS"]);
            var rows =
                metadataFieldDt.Select("ID='" + TypeUtil.ToInt(curDr["MetaDataFieldID"]) + "' and blnDesignSort=1");
            if ((rows == null) || (rows.Length == 0))
            {
                MessageShowUtil.ShowMsg("元数据未配置此列可编排,请先在元数据定义！");
                return;
            }

            if (TypeUtil.ToString(rows[0]["strFkCode"]) == "")
            {
                MessageShowUtil.ShowMsg("元数据未配置参照编码，请先在元数据配置！");
                return;
            }
            var strFKCode = TypeUtil.ToString(rows[0]["strFkCode"]);
            var frm = new frmDataSortSet();
            frm.MetadataID = MetadataId;
            frm.dtLmd = LmdDt;
            frm.strFileName = strFileName;
            frm.strListDisplayName = strListDisplayName;
            frm.strFKCode = strFKCode;
            //frm.strCondition = TypeUtil.ToString(curDr["strCondition"]);
            //frm.strConditionCHS = TypeUtil.ToString(curDr["strConditionCHS"]);
            frm.ListDataID = ListDataID;

            frm.ShowDialog();
            if (frm.blnOK)
            {
                //curDr["strCondition"] = frm.strCondition;
                //curDr["strConditionCHS"] = frm.strConditionCHS;
                //curDr["strConditionShow"] = frm.strConditionCHS.Replace("&&$$", ";").Replace("&&$", ",");
            }
        }

        private void tlbPivotSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("OpenReportDesign"))
                {
                    return;
                }

                if ((ListDataExVo == null) || (ListDataExVo.ListDataID == 0))
                {
                    ShowMsg("未保存方案，请先保存后再操作！", true);
                    return;
                }

                var strFileName = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple\\" +
                                  ListDataExVo.StrListDataName + ListDataExVo.ListDataID + ".repx";
                //现改在报表设计器里进行判断，需要验证 
                //if (!File.Exists(strFileName))
                //{//如果不存在，则先创建报表文件
                //    frmList frm = new frmList();
                //    frm.strReportFileName = _listDataExVo.StrListDataName + _listDataExVo.ListDataID.ToString();
                //    frm.listDisplayExList = Model.GetListDisplayExData(_listDataExVo.ListDataID);                  
                //    frm.SetPivotFormat();
                //}              

                var frmDesign = new frmReportDesign();
                frmDesign.StrFileName = strFileName;
                var strsql = ListDataExVo.StrListSQL.Insert(6, " top 10 ");
                if (Model.GetDbType() == "mysql")
                    strsql = ListDataExVo.StrListSQL + " LIMIT 1,10";
                var dt = Model.GetDataTable(strsql);
                dt = TypeUtil.AddNumberToDataTable(dt);
                frmDesign.dtReportDataSource = dt;
                frmDesign.dtLmd = LmdDt;
                frmDesign.listDataExDTO = ListDataExVo;
                frmDesign.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        /// <summary>
        ///     初始化方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbInitScheme_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("InitReportColumn"))
                {
                    return;
                }

                UpdateGridCurrentRow();

                foreach (DataRow row in LmdDt.Rows)
                {
                    row["strConditionShow"] = "";
                    row["blnSummary"] = false;
                    row["lngSummaryType"] = 0;
                    row["strSummaryDisplayFormat"] = "";
                    row["lngHyperLinkType"] = 0;
                    row["strHyperlink"] = "";
                    row["strParaColName"] = "";
                    row["strConditionFormat"] = "";
                    row["strConditionFormatShow"] = "";
                    row["strBluerCondition"] = "";
                    row["blnMerge"] = false;
                    row["lngApplyType"] = 1;
                    row["lngOrder"] = 0;
                    row["lngOrderMethod"] = 0;
                    row["strCondition"] = "";
                    row["strConditionCHS"] = "";
                    row["lngFKType"] = 0;
                    row["blnMainMerge"] = false;
                    row["blnKeyWord"] = false;
                    row["lngKeyGroup"] = 0;
                }
                var rows = LmdDt.Select("isCalcField=1");
                foreach (var row in rows)
                    LmdDt.Rows.Remove(row);
                MessageShowUtil.ShowInfo("初始化成功！");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetControlVisible()
        {
            PermissionManager.HavePermission("SaveAsReportScheme", tlbSaveAs);
            PermissionManager.HavePermission("SaveReportScheme", tlbSave);
            PermissionManager.HavePermission("DeleteReportScheme", tlbDelete);
            PermissionManager.HavePermission("SetReportFixCon", ButtonEditCondition);
            PermissionManager.HavePermission("SetReportCaclColumn", btnAddCalcCol);
            PermissionManager.HavePermission("SetReportCaclColumn", btnEditCalcCol);
            PermissionManager.HavePermission("SetReportCaclColumn", btnDelCalcCol);
            PermissionManager.HavePermission("SetReportStyle", btnSFCSetting);
            PermissionManager.HavePermission("SetReportBlurr", btnBluerConditionSet);
            PermissionManager.HavePermission("SetReportContenxt", btnContextConditionSet);
            PermissionManager.HavePermission("OpenReportDesign", tlbPivotSetting);
            PermissionManager.HavePermission("InitReportColumn", tlbInitScheme);
            Text = StrListName + Text;
        }

        #region 工具栏事件

        /// <summary>
        ///     获取存在的方案名列表
        /// </summary>
        /// <returns>IList</returns>
        private IList<string> GetExistSchemaNameList()
        {
            IList<string> existNameList = new List<string>();
            var dt = (DataTable) lookUpSchema.Properties.DataSource;
            if ((dt != null) && (dt.Rows.Count > 0))
                for (var i = 0; i < dt.Rows.Count; i++)
                    existNameList.Add(TypeUtil.ToString(dt.Rows[i]["strListDataName"]));

            return existNameList;
        }

        /// <summary>
        ///     保存方案
        /// </summary>
        private void tlbSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveReportScheme"))
                {
                    return;
                }


                var schemaName = lookUpSchema.Text;
                if (schemaName == string.Empty)
                {
                    var existNameList = GetExistSchemaNameList();
                    var frm = new frmSchemaName();
                    frm.ExistSchemaNameList = existNameList;
                    frm.ShowDialog();

                    if (frm.SchemaName == string.Empty)
                        return;

                    schemaName = frm.SchemaName;
                }

                UpdateGridCurrentRow(); //更新Grid当前行

                //2015-02-03 如果是日表元数据,则不能设置汇总字段
                var dtMetadata = ClientCacheModel.GetServerMetaData(MetadataId);
                DataRow[] rows = null;
                //DataRow[] rows = dtMetadata.Select("blnDay=1");//判断主元数据是不是日表
                //if (rows != null && rows.Length != 0)
                //{
                //    rows = this._lmdDt.Select("lngSummaryType>1");
                //    if (rows != null && rows.Length > 0)
                //    {
                //        MessageShowUtil.ShowInfo("日表不能设置汇总字段！");
                //        return;
                //    }
                //}

                var rows2 = LmdDt.Select("blnMainMerge=1");
                var rows3 = LmdDt.Select("strConditionFormat<>''");
                if (rows2.Length > 0 && rows3.Length > 0)
                {
                    MessageShowUtil.ShowInfo("不能同时设置主合并和条件格式！");
                    return;
                }

                rows = LmdDt.Select("blnKeyWord=1");
                if (rows.Length > 1)
                {
                    MessageShowUtil.ShowInfo("关键字有且只能设置一个！");
                    return;
                }
                rows = LmdDt.Select("blnKeyWord=1  and  lngKeyGroup>0");
                if (rows.Length > 0)
                {
                    MessageShowUtil.ShowInfo("关键字不能设置汇总！");
                    return;
                }

                rows = LmdDt.Select("lngProivtType>0");
                if ((rows.Length > 0) && (rows.Length < 3))
                {
                    MessageShowUtil.ShowInfo("交叉表类型必须且只有一个行区、列区、数据区！");
                    return;
                }


                //组织数据
                GetListMetaDataTable(); //获取ListMetaData数据表
                OrgListDataExData(schemaName, 0, LmdDt); //组织ListDataEx                
                var lmdList = OrgListMetaDataData(LmdDt); //组织ListMetaData
                var ldList = OrgListDisplayExData(ListDataExVo.UserID > 0 ? 1 : 0, LmdDt); //组织ListDisplayEx

                //2015-02-13  判断栏目名称是否有重复
                var viewsource = LmdDt.Copy().DefaultView;
                viewsource.RowFilter = "blnSysProcess=0 and blnShow=1";
                var dtsource = viewsource.ToTable(true, "strListDisplayFieldNameCHS");
                if (viewsource.Count != dtsource.Rows.Count)
                {
                    MessageShowUtil.ShowInfo("栏目名称有重复,请修改后再保存！");
                    return;
                }

                //保存数据
                // 20170605
                // 20170624
                //var par = new ListexInfo()
                //{
                //    ListCommandExDTOList = new List<ListcommandexInfo>(),
                //    ListDataExDTOList = new List<ListdataexInfo>(),
                //    LastUpdateDate = "",
                //    Programer = "",
                //    ProgramerNotes = "",
                //    StrDescription = "",
                //    StrGuidCode = "",
                //    StrListCode = "",
                //    StrListDescription = "",
                //    StrListGroupCode = "",
                //    StrListName = "",
                //    StrRightCode = ""
                //};
                //ListDataExVo.ListDataLayoutDTOList = new List<ListdatalayountInfo>();
                //ListDataExVo.ListDisplayExDTOList = new List<ListdisplayexInfo>();
                //ListDataExVo.ListMetaDataDTOList = new List<ListmetadataInfo>();
                //ListDataExVo.ListTempleDTOList = new List<ListtempleInfo>();
                //ListDataExVo.StrConTextCondition = "";
                //ListDataExVo.StrDefaultShowStyle = "";
                
                //ListDataExVo = Model.SaveList(null, new List<ListcommandexInfo>(), ListDataExVo, lmdList,
                //    ldList, LmdDt, ListID <= 0 ? 0 : 1);
                ListDataExVo = Model.SaveList(null, null, ListDataExVo, lmdList,
                    ldList, LmdDt, ListID <= 0 ? 0 : 1);
                ListDataExVo.InfoState = InfoState.Modified;

                ListDataID = ListDataExVo.ListDataID;

                LoadShcemaList(); //刷新数据源
                lookUpSchema.EditValueChanged -= lookUpSchema_EditValueChanged;
                lookUpSchema.EditValue = ListDataID;
                lookUpSchema.EditValueChanged += lookUpSchema_EditValueChanged;

                RefreshSchemaDataSource();

                MessageShowUtil.ShowStaticInfo("保存成功！", barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     另存方案
        /// </summary>
        private void tlbSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveAsReportScheme"))
                {
                    return;
                }
                if ((ListDataExVo == null) || (ListDataExVo.InfoState == InfoState.AddNew))
                {
                    ShowMsg("当前方案未保存，不能另存！", true);
                    return;
                }

                UpdateGridCurrentRow(); //更新Grid当前行

                var existNameList = GetExistSchemaNameList();
                var frm = new frmSchemaName();
                frm.ExistSchemaNameList = existNameList;
                frm.ShowDialog();

                if (frm.SchemaName == string.Empty)
                    return;

                ListDataExVo.InfoState = InfoState.AddNew;

                //组织数据
                GetListMetaDataTable(); //获取ListMetaData数据表
                OrgListDataExData(frm.SchemaName, frm.LngUseRight, LmdDt); //组织ListDataEx               
                var lmdList = OrgListMetaDataData(LmdDt); //组织ListMetaData
                var ldList = OrgListDisplayExData(frm.LngUseRight, LmdDt); //组织ListDisplayEx

                // 20170605
                // 20170624
                //var par = new ListexInfo()
                //{
                //    ListCommandExDTOList = new List<ListcommandexInfo>(),
                //    ListDataExDTOList = new List<ListdataexInfo>(),
                //    LastUpdateDate = "",
                //    Programer = "",
                //    ProgramerNotes = "",
                //    StrDescription = "",
                //    StrGuidCode = "",
                //    StrListCode = "",
                //    StrListDescription = "",
                //    StrListGroupCode = "",
                //    StrListName = "",
                //    StrRightCode = ""
                //};
                //ListDataExVo.ListDataLayoutDTOList = new List<ListdatalayountInfo>();
                //ListDataExVo.ListDisplayExDTOList = new List<ListdisplayexInfo>();
                //ListDataExVo.ListMetaDataDTOList = new List<ListmetadataInfo>();
                //ListDataExVo.ListTempleDTOList = new List<ListtempleInfo>();
                //ListDataExVo.StrConTextCondition = "";
                //ListDataExVo.StrDefaultShowStyle = "";
                //ListDataExVo = Model.SaveList(par, new List<ListcommandexInfo>(), ListDataExVo, lmdList,
                //    ldList, LmdDt, 2);
                ListDataExVo = Model.SaveList(null, null, ListDataExVo, lmdList, ldList, LmdDt, 2);
                ListDataExVo.InfoState = InfoState.Modified;
                ;

                ListDataID = ListDataExVo.ListDataID;

                var dt = (DataTable) lookUpSchema.Properties.DataSource;
                //刷新数据源
                var dr = dt.NewRow();
                dr["ListDataID"] = ListDataID;
                dr["strListDataName"] = frm.SchemaName;
                dt.Rows.Add(dr);
                lookUpSchema.Properties.DataSource = dt;

                //设置为当前方案
                lookUpSchema.EditValueChanged -= lookUpSchema_EditValueChanged;
                lookUpSchema.EditValue = ListDataID;
                lookUpSchema.EditValueChanged += lookUpSchema_EditValueChanged;

                RefreshSchemaDataSource();

                MessageShowUtil.ShowStaticInfo("另存成功！", barStaticItemMsg);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     更新Grid当前行
        /// </summary>
        private void UpdateGridCurrentRow()
        {
            gvAdvanceSet.CloseEditor();
            gvAdvanceSet.UpdateCurrentRow();

            gvColSelected.CloseEditor();
            gvColSelected.UpdateCurrentRow();
        }

        /// <summary>
        ///     删除方案
        /// </summary>
        private void tlbDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("DeleteReportScheme"))
            {
                return;
            }

            if (ListDataID == 0)
            {
                ShowMsg("当前方案未保存，不能删除！", true);
                return;
            }
            if (CurListDataId == ListDataExVo.ListDataID)
            {
                ShowMsg("列表正在使用的方案不能删除！", true);
                return;
            }
            //if (!PermissionManager.HavePermission("DeleteSchema"))
            //{
            //    this.ShowMsg("没有删除方案的权限，不能删除！", true);
            //    return;
            //}
            //if (this._listDataExVo.BlnDefault && !PermissionManager.HavePermission("DeleteDefaultSchema"))
            //{
            //    this.ShowMsg("没有删除默认方案的权限，不能删除！", true);
            //    return;
            //}
            //if (this._listDataExVo.BlnPredefine && !PermissionManager.HavePermission("DeletePredefineSchema"))
            //{
            //    this.ShowMsg("没有删除预置方案的权限，不能删除", true);
            //    return;
            //}

            if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前方案？"))
                return;

            try
            {
                Model.DeleteSchema(ListDataExVo.ListID, ListDataExVo.ListDataID);

                ListDataID = 0;

                ClearSchema();

                LoadShcemaList(); //刷新数据源

                if (ListDataID <= 0)
                    CreateNewSchema();

                LoadSchema(false);

                MessageShowUtil.ShowInfo("删除成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     设置为默认方案
        /// </summary>
        private void tlbSetDefault_ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((ListDataExVo == null) || (ListDataExVo.InfoState == InfoState.AddNew))
            {
                MessageShowUtil.ShowInfo("当前方案未保存，请先保存！");
                return;
            }

            try
            {
                if (ListDataExVo.BlnDefault)
                {
                    MessageShowUtil.ShowInfo("设置成功！");
                    return;
                }

                ListDataExVo.BlnDefault = true;
                Model.SetDefaultSchema(ListDataExVo);
                LoadShcemaList(); //刷新数据源
                MessageShowUtil.ShowInfo("设置成功！");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     方案重命名
        /// </summary>
        private void tlbRename_ItemClick(object sender, ItemClickEventArgs e)
        {
            if ((ListDataExVo == null) || (ListDataExVo.InfoState == InfoState.AddNew))
            {
                MessageShowUtil.ShowInfo("当前方案未保存，请先保存！");
                return;
            }

            try
            {
                var existNameList = GetExistSchemaNameList();
                var frm = new frmSchemaName();
                frm.SchemaName = lookUpSchema.Text.Trim();
                frm.ExistSchemaNameList = existNameList;
                frm.ShowDialog();

                if (frm.SchemaName == string.Empty)
                    return;

                ListDataExVo.StrListDataName = frm.SchemaName;
                Model.SaveListDataEx(ListDataExVo);
                LoadShcemaList(); //刷新数据源
                MessageShowUtil.ShowInfo("方案重命名成功！");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     组织ListDataEx
        /// </summary>
        /// <param name="schemaName">方案名</param>
        /// <param name="lngUseRight">方案使用权限</param>
        private void OrgListDataExData(string schemaName, int lngUseRight, DataTable lmdDt)
        {
            ListDataExVo.ListDataID = ListDataID;
            ListDataExVo.ListID = ListID;
            ListDataExVo.StrListSQL = "11";
            ListDataExVo.StrListSrcSQL = "";
            ListDataExVo.StrListDataName = schemaName;
            ListDataExVo.BlnSmlSum = chkSmlSum.Checked;
            ListDataExVo.LngSmlSumType = TypeUtil.ToInt(lookUpSmlSumType.EditValue);
            if (TypeUtil.ToString(ListDataExVo.StrListDefaultField) == string.Empty)
            {
                DataTable dt = null;
                if (lmdDt != null)
                {
                    dt = lmdDt.Copy();
                    dt.DefaultView.RowFilter = " isnull(lngKeyFieldType,0)=2";
                }
                var frm = new frmListParameterSet();
                frm.DataSource = dt;
                frm.ShowDialog();
                ListDataExVo.StrListDefaultField = frm.StrFieldNameList;

                if (ListDataExVo.StrListDefaultField == string.Empty)
                    throw new Exception("请输入方案默认参数！");
            }

            if (ListDataExVo.InfoState == InfoState.AddNew)
            {
                ListDataExVo.StrPivotSetting = "";
                ListDataExVo.StrChartSetting = "";

                ListDataExVo.LngRowIndex = 1;
                if (lookUpSchema.EditValue == null)
                    ListDataExVo.BlnDefault = true;
                else
                    ListDataExVo.BlnDefault = false;
                ListDataExVo.BlnPredefine = false;
                ListDataExVo.UserID = 0;
                if (lngUseRight == 1)
                    ListDataExVo.UserID = TypeUtil.ToInt(RequestUtil.GetParameterValue("userId"));
            }
            else
            {
                if (ListDataExVo.BlnPredefine)
                    throw new Exception("没有编辑预制方案的权限，不能保存！");
            }
        }

        /// <summary>
        ///     组织ListMetaData
        /// </summary>
        /// <returns>IList</returns>
        private IList<ListmetadataInfo> OrgListMetaDataData(DataTable lmdDt)
        {
            IList<ListmetadataInfo> list = new List<ListmetadataInfo>();
            ListmetadataInfo item = null;
            DataRow dr;
            var strTableAlias = "";
            for (var i = 0; i < lmdDt.Rows.Count; i++)
            {
                dr = lmdDt.Rows[i];

                strTableAlias = TypeUtil.ToString(dr["strTableAlias"]);

                item = new ListmetadataInfo();
                item.InfoState = InfoState.AddNew;
                item.ID = 0;
                item.ListDataID = ListDataID;
                item.MetaDataID = TypeUtil.ToInt(dr["MetaDataID"]);
                item.MetaDataFieldID = TypeUtil.ToInt(dr["MetaDataFieldID"]);
                item.MetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                item.LngParentFieldID = TypeUtil.ToInt(dr["lngParentFieldID"]);
                item.LngRelativeFieldID = TypeUtil.ToInt(dr["lngRelativeFieldID"]);
                item.StrTableAlias = strTableAlias;
                item.StrFullPath = TypeUtil.ToString(dr["strFullPath"]);
                item.StrParentFullPath = TypeUtil.ToString(dr["strParentFullPath"]);
                item.LngAliasCount = TypeUtil.ToInt(dr["lngAliasCount"]);
                item.LngSourceType = TypeUtil.ToInt(dr["lngSourceType"]);
                item.LngParentID = TypeUtil.ToInt(dr["lngParentID"]);
                item.StrFieldType = TypeUtil.ToString(dr["strFieldType"]);
                item.StrFkCode = TypeUtil.ToString(dr["strFkCode"]);
                item.IsCalcField = TypeUtil.ToBool(dr["isCalcField"]);
                item.StrFormula = TypeUtil.ToString(dr["strFormula"]);
                item.StrRefColList = TypeUtil.ToString(dr["strRefColList"]);
                item.LngOrder = TypeUtil.ToInt(dr["lngOrder"]);
                item.LngOrderMethod = TypeUtil.ToInt(dr["lngOrderMethod"]);
                item.StrCondition = TypeUtil.ToString(dr["strCondition"]);
                item.StrConditionCHS = TypeUtil.ToString(dr["strConditionCHS"]);
                item.LngKeyFieldType = TypeUtil.ToInt(dr["lngKeyFieldType"]);

                item.BlnFreCondition = TypeUtil.ToBool(dr["blnFreCondition"]);
                item.LngFreConIndex = TypeUtil.ToInt(dr["lngFreConIndex"]);
                item.StrFreCondition = TypeUtil.ToString(dr["strFreCondition"]);
                item.StrFreConditionCHS = TypeUtil.ToString(dr["strFreConditionCHS"]);
                item.BlnReceivePara = TypeUtil.ToBool(dr["blnReceivePara"]);

                item.BlnSysProcess = TypeUtil.ToBool(dr["blnSysProcess"]);
                item.BlnShow = TypeUtil.ToBool(dr["blnShow"]);
                item.BlnPrintFreCondition = TypeUtil.ToBool(dr["blnPrintFreCondition"]);
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        ///     组织ListDisplayEx
        /// </summary>
        /// <returns>IList</returns>
        private IList<ListdisplayexInfo> OrgListDisplayExData(int lngUseRight, DataTable lmdDt) //部分数据需界面录入（测试版本暂未开发）
        {
            IList<ListdisplayexInfo> list = new List<ListdisplayexInfo>();
            ListdisplayexInfo item = null;
            var lngRowIndex = 0; //显示顺序
            DataRow dr;
            for (var i = 0; i < lmdDt.Rows.Count; i++)
            {
                dr = lmdDt.Rows[i];

                item = new ListdisplayexInfo();
                item.InfoState = InfoState.AddNew;

                item.ListDisplayID = 0;
                item.UserID = TypeUtil.ToInt(RequestUtil.GetParameterValue("userId"));
                item.ListDataID = ListDataID;
                item.StrListDisplayFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                item.StrListDisplayFieldNameCHS = TypeUtil.ToString(dr["strListDisplayFieldNameCHS"]);
                item.BlnSummary = TypeUtil.ToBool(dr["blnSummary"]);
                item.LngListDisplayFieldFormat = 0;
                item.LngDisplayWidth = TypeUtil.ToInt(dr["lngDisplayWidth"]);
                if (TypeUtil.ToBool(dr["blnShow"]))
                    item.LngRowIndex = ++lngRowIndex;
                else
                    item.LngRowIndex = -1;
                item.LngSummaryType = TypeUtil.ToInt(dr["lngSummaryType"]);
                item.StrSummaryDisplayFormat = TypeUtil.ToString(dr["strSummaryDisplayFormat"]);
                item.LngHyperLinkType = TypeUtil.ToInt(dr["lngHyperLinkType"]);
                item.StrHyperlink = TypeUtil.ToString(dr["strHyperlink"]);
                item.StrParaColName = TypeUtil.ToString(dr["strParaColName"]);
                item.StrConditionFormat = TypeUtil.ToString(dr["strConditionFormat"]);
                item.IsCalcField = TypeUtil.ToBool(dr["isCalcField"]);
                item.StrBluerCondition = TypeUtil.ToString(dr["strBluerCondition"]);
                item.BlnMerge = TypeUtil.ToBool(dr["blnMerge"]);
                item.LngApplyType = TypeUtil.ToInt(dr["lngApplyType"]);
                item.LngFKType = TypeUtil.ToInt(dr["lngFKType"]);
                item.BlnMainMerge = TypeUtil.ToBool(dr["blnMainMerge"]);
                item.BlnKeyWord = TypeUtil.ToBool(dr["blnKeyWord"]);
                item.LngKeyGroup = TypeUtil.ToInt(dr["lngKeyGroup"]);
                item.BlnConstant = TypeUtil.ToBool(dr["blnConstant"]);
                //item.LngProivtType = TypeUtil.ToInt(dr["lngProivtType"]);
                item.LngProivtType = TypeUtil.ToBool(dr["lngProivtType"]);
                list.Add(item);
            }

            return list;
        }


        /// <summary>
        ///     确定
        /// </summary>
        private void tlbOk_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (BlnListEnter)
                {
                    UpdateGridCurrentRow();
                    var lmdDt = GetListMetaDataTable(); //获取ListMetaData数据表
                    ListDataExVo.BlnSmlSum = chkSmlSum.Checked;
                    ListDataExVo.LngSmlSumType = TypeUtil.ToInt(lookUpSmlSumType.EditValue);
                    var strListSQL = Model.GetSQL(lmdDt);
                    if ((ListDataExVo.StrConTextCondition != null) &&
                        (TypeUtil.ToString(ListDataExVo.StrConTextCondition) != string.Empty))
                    {
                        var strs = ListDataExVo.StrConTextCondition.Split(new[] {"&&&$$$"},
                            StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length == 2)
                            strListSQL = strListSQL.Replace("where 1=1",
                                "where 1=1 " + " and " + strs[1].Trim().Replace('_', '.'));
                    }
                    ListDataExVo.StrListSQL = strListSQL;
                    ListDisplayExList = OrgListDisplayExData(ListDataExVo.UserID > 0 ? 1 : 0, lmdDt);
                        //组织ListDisplayEx                    
                }

                BlnOk = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     取消
        /// </summary>
        private void tlbCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            BlnOk = false;
            Close();
        }

        #endregion

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
                try
            {
                var rows = metadataFieldDt.Select("MetaDataID=" + MetadataId + " and blnDesignSort=1");
                if ((rows == null) || (rows.Length == 0))
                {
                    MessageShowUtil.ShowMsg("元数据里没有配置可编排的栏目，不能进行编排");
                    return;
                }
                var strFKCode = TypeUtil.ToString(rows[0]["strFkCode"]);
                var frm = new frmDataSortSet();
                frm.MetadataID = MetadataId;
                frm.dtLmd = LmdDt;
                var strFileName =
                    LmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["MetaDataFieldName"].ToString();
                frm.strFileName = strFileName;
                var strListDisplayName =
                    LmdDt.Select("MetaDataFieldID=" + TypeUtil.ToInt(rows[0]["ID"]))[0]["strListDisplayFieldNameCHS"]
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

    /// <summary>
    ///     树列表字段结构
    /// </summary>
    internal struct TreeListField
    {
        public int metaDataId;
        public int parentId;
        public int lngRelativeID;
        public int metaDataFieldId;
        public int lngParentFieldId;
        public int lngRelativeFieldID;
        public string strName;
        public int lngAliasCount;
        public int lngNextAliasCount;
        public string strTableAlias;
        public string strFullPath;
        public string strParentFullPath;
        public string strNextFullPath;
        public string strFieldName;
        public string strFieldChName;
        public string strFieldType;
        public string strFkCode;
        public int lngSourceType;
        public bool blnPK;
        public int lngDataRightType;
        public string strSummaryDisplayFormat;
    }
}