using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Controls;
using Sys.Safety.Reports.Model;
using Sys.Safety.ClientFramework.View.Menu;

namespace Sys.Safety.Reports
{
    public partial class frmListEdit : XtraForm
    {
        public delegate void RefreshDir(int dirId);

        private DataTable cmdDt;
        private readonly IList<int> delCmdList = new List<int>();
        private DataTable listDirDt;
        private DataTable listDirTreeDt;
        private ListexInfo listExVo;
        private readonly int listId;
        private DataTable metadataDt;
        private readonly ListExModel Model = new ListExModel();
        private WaitDialogForm wdf;

        public frmListEdit(int _listId)
        {
            DirID = 0;
            InitializeComponent();

            listId = _listId;
        }

        /// <summary>
        ///     目录ID
        /// </summary>
        public int DirID { set; get; }

        public event RefreshDir RefreshListDir;

        private void frmListEdit_Load(object sender, EventArgs e)
        {
            try
            {
                FillLookUp();

                metadataDt = ClientCacheModel.GetServerMetaData();

                if (listId > 0)
                {
                    listExVo = Model.GetListEx(listId);
                    listExVo.InfoState = InfoState.Modified;
                }

                if (listExVo == null)
                {
                    CreateNewVO();
                    listExVo.DirID = DirID;
                }

                LoadData();

                SetUIState();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void frmListEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void FillLookUp()
        {
            listDirDt = Model.GetListDir();
            listDirTreeDt = listDirDt.Clone();
            GetListTreeDir(0, string.Empty);
            lookUpDir.Properties.DataSource = listDirTreeDt;
            SetIconData();


            LookUpUtil.CreateGridLookUp("AllRight", repositoryItemGridLookUpRight, false, true);
            //DataTable roleRightDt = roleRightModel.GetTotalRoleRight();
            //this.repositoryItemGridLookUpRight.DataSource = roleRightDt;
        }

        /// <summary>
        ///     获取列表目录
        /// </summary>
        /// <returns>DataTable</returns>
        public void GetListTreeDir(int dirId, string strPre)
        {
            var strSpace = "   ";
            var drs = listDirDt.Select("DirID=" + dirId);
            DataRow descDr = null;
            foreach (var dr in drs)
            {
                descDr = listDirTreeDt.NewRow();
                foreach (DataColumn dc in listDirTreeDt.Columns)
                    descDr[dc.ColumnName] = dr[dc.ColumnName];
                descDr["strListName"] = strPre + TypeUtil.ToString(dr["strListName"]);

                listDirTreeDt.Rows.Add(descDr);
                GetListTreeDir(TypeUtil.ToInt(dr["ListID"]), strPre + strSpace);
            }
        }

        /// <summary>
        ///     设置图标选择数据源
        /// </summary>
        public void SetIconData()
        {
            var iconDt = new DataTable();
            iconDt.Columns.Add(new DataColumn("Code", Type.GetType("System.String")));
            iconDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            IDictionary<string, string> _dic = new Dictionary<string, string>();
            _dic.Add("", "");
            _dic.Add("add", "新增");
            _dic.Add("edit", "修改");
            _dic.Add("delete", "删除");

            DataRow dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = iconDt.NewRow();
                dr["Code"] = key;
                dr["Name"] = _dic[key];
                iconDt.Rows.Add(dr);
            }

            LookUpEditIconName.DataSource = iconDt;


            var webIconDt = new DataTable();
            webIconDt.Columns.Add(new DataColumn("Code", Type.GetType("System.String")));
            webIconDt.Columns.Add(new DataColumn("Name", Type.GetType("System.String")));

            _dic.Clear();
            _dic.Add("", "");
            _dic.Add("icon-add", "新增");
            _dic.Add("icon-edit", "修改");
            _dic.Add("icon-delete", "删除");

            dr = null;
            foreach (var key in _dic.Keys)
            {
                dr = webIconDt.NewRow();
                dr["Code"] = key;
                dr["Name"] = _dic[key];
                webIconDt.Rows.Add(dr);
            }

            repLookUpstrWebListIconName.DataSource = webIconDt;
        }

        private void CreateNewVO()
        {
            listExVo = new ListexInfo();
            listExVo.InfoState = InfoState.AddNew;
            listExVo.BlnList = true;
            listExVo.BlnEnable = true;
        }

        private void Clear()
        {
            lookUpDir.EditValue = null;
            buttonEditMainMetaData.Text = "";
            txtListCode.Text = "";
            txtListName.Text = "";
            txtstrListGroupCode.Text = "";
            txtListDesc.Text = "";
            chkEnable.Checked = true;
            chkPivot.Checked = false;
            chkChart.Checked = false;
        }

        private void LoadData()
        {
            lookUpDir.EditValue = listExVo.DirID;
            buttonEditMainMetaData.Text = GetMetaDataName(listExVo.MainMetaDataID);
            txtListCode.Text = listExVo.StrListCode;
            txtListName.Text = listExVo.StrListName;
            txtstrListGroupCode.Text = listExVo.StrListGroupCode;
            btnListRight.Text = GetRightName(listExVo.StrRightCode);
            txtListDesc.Text = listExVo.StrListDescription;
            chkEnable.Checked = listExVo.BlnEnable;
            chkPivot.Checked = listExVo.BlnPivot;
            chkChart.Checked = listExVo.BlnChart;

            //载入命令数据
            cmdDt = Model.GetListCommandExData(listExVo.ListID);
            gridControlCmd.DataSource = cmdDt;
        }

        private string GetMetaDataName(int id)
        {
            if (id == 0) return "";

            var drs = metadataDt.Select("ID=" + id);
            if ((drs != null) && (drs.Length > 0))
                return TypeUtil.ToString(drs[0]["strName"]);

            return "";
        }

        //private string GetEntityName()
        //{
        //    ListCommandEx lc = Model.GetListCommandEx(this.listExVo.ListID);
        //    if (lc != null)
        //    {
        //        return lc.Parameters.Substring(7);
        //    }

        //    return "";
        //}

        private void SetUIState()
        {
            var blnLock = listExVo.InfoState != InfoState.AddNew;

            buttonEditMainMetaData.Properties.ReadOnly = blnLock;
            buttonEditMainMetaData.Properties.Buttons[0].Enabled = !blnLock;
        }

        /// <summary>
        ///     校验数据
        /// </summary>
        private void CheckData()
        {
            if (TypeUtil.ToInt(lookUpDir.EditValue) <= 0)
                throw new Exception("请选择目录");
            if (listExVo.MainMetaDataID <= 0)
                throw new Exception("请选择主业务对象");
            if (txtListCode.Text == string.Empty)
                throw new Exception("请录入列表编码");
            if (txtListName.Text == string.Empty)
                throw new Exception("请录入列表名称");

            if (!TypeUtil.IsValidCode(txtListCode.Text.Trim()))
                throw new Exception("列表编码只能为字母，数字和下划线！");

            DataRow dr = null;
            gridViewCmd.CloseEditor();
            gridViewCmd.UpdateCurrentRow();
            var dblCount = 0;
            for (var i = 0; i < cmdDt.Rows.Count; i++)
            {
                dr = cmdDt.Rows[i];
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                if (TypeUtil.ToBool(dr["blnDblClick"]))
                    dblCount++;
            }

            if (dblCount > 1)
                throw new Exception("支持双击事件不能超过一个");
        }

        private void GetListData()
        {
            listExVo.DirID = TypeUtil.ToInt(lookUpDir.EditValue);
            //this.listExVo.MainMetaDataID

            listExVo.StrListCode = txtListCode.Text.Trim();
            listExVo.StrListName = txtListName.Text;
            listExVo.StrListGroupCode = txtstrListGroupCode.Text.Trim();
            listExVo.StrListDescription = txtListDesc.Text;
            listExVo.BlnEnable = chkEnable.Checked;
            listExVo.BlnPivot = chkPivot.Checked;
            listExVo.BlnChart = chkChart.Checked;
        }

        /// <summary>
        ///     组织ListCommandEx
        /// </summary>
        /// <returns>IList</returns>
        private IList<ListcommandexInfo> GetListCommandExData()
        {
            IList<ListcommandexInfo> list = new List<ListcommandexInfo>();
            ListcommandexInfo item = null;
            DataRow dr = null;
            for (var i = 0; i < cmdDt.Rows.Count; i++)
            {
                dr = cmdDt.Rows[i];
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                item = new ListcommandexInfo();
                item.ListCommandID = TypeUtil.ToInt(dr["ListCommandID"]);
                if (item.ListCommandID <= 0)
                    item.InfoState = InfoState.AddNew;
                else
                    item.InfoState = InfoState.Modified;


                item.ListID = listExVo.ListID;
                item.StrRequestCode = "";
                item.RequestId = TypeUtil.ToString(dr["requestId"]);
                item.StrRequestCode = TypeUtil.ToString(dr["strRequestCode"]);
                item.Parameters = TypeUtil.ToString(dr["parameters"]);
                item.StrWebParameters = TypeUtil.ToString(dr["strWebParameters"]);
                item.StrListCommandCode = TypeUtil.ToString(dr["strListCommandCode"]);
                item.StrListCommandName = TypeUtil.ToString(dr["strListCommandName"]);
                item.StrListCommandTip = TypeUtil.ToString(dr["strListCommandTip"]);
                item.StrListIconName = TypeUtil.ToString(dr["strListIconName"]);
                item.StrWebListIconName = TypeUtil.ToString(dr["strWebListIconName"]);
                item.BlnVisible = true;
                item.BlnDblClick = TypeUtil.ToBool(dr["blnDblClick"]);
                item.LngRowIndex = TypeUtil.ToInt(dr["lngRowIndex"]);
                item.StrCustomer = TypeUtil.ToString(dr["strCustomer"]);
                list.Add(item);
            }

            if (listExVo.InfoState != InfoState.AddNew)
                foreach (var id in delCmdList)
                {
                    item = new ListcommandexInfo();
                    item.ListCommandID = id;
                    item.StrRequestCode = "";
                    item.InfoState = InfoState.Delete;
                    list.Add(item);
                }

            return list;
        }

        private void tlbAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CreateNewVO();

                LoadData();

                SetUIState();
                delCmdList.Clear();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     保存
        /// </summary>
        private void tlbSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CheckData(); // 校验数据
                GetListData(); //获取list数据
                var cmdList = GetListCommandExData(); //组织ListCommandEx

                listExVo = Model.SaveList(listExVo, cmdList, false, false);
                listExVo.InfoState = InfoState.Modified;

                SetUIState();
                delCmdList.Clear();

                LoadData();

                if (RefreshListDir != null)
                    RefreshListDir(listExVo.DirID);

                MessageShowUtil.ShowInfo("保存成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     另存
        /// </summary>
        private void tlbSaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            var oldState = listExVo.InfoState;
            try
            {
                var blnSaveAsSchema = false;
                var dialogResult = MessageBox.Show("确定要另存列表,另存后将会生成一张新的列表", "提示", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                    blnSaveAsSchema = true;
                else
                    return;

                listExVo.InfoState = InfoState.AddNew;
                CheckData(); // 校验数据
                GetListData(); //获取list数据
                var cmdList = GetListCommandExData(); //组织ListCommandEx               
                listExVo = Model.SaveList(listExVo, cmdList, true, blnSaveAsSchema);
                listExVo.InfoState = InfoState.Modified;

                SetUIState();
                delCmdList.Clear();

                LoadData();

                if (RefreshListDir != null)
                    RefreshListDir(listExVo.DirID);

                MessageShowUtil.ShowInfo("另存成功");
            }
            catch (Exception ex)
            {
                listExVo.InfoState = oldState;
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     删除列表
        /// </summary>
        private void tlbDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (listExVo.ListID <= 0)
                {
                    MessageShowUtil.ShowInfo("未保存列表，不能删除");
                    return;
                }

                if (listExVo.BlnPredefine)
                {
                    MessageShowUtil.ShowInfo("预制列表不能删除");
                    return;
                }


                if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前列表?"))
                    return;

                Model.DeleteList(listExVo.ListID);

                if (RefreshListDir != null)
                    RefreshListDir(listExVo.DirID);

                CreateNewVO();
                LoadData();
                SetUIState();
                delCmdList.Clear();

                MessageShowUtil.ShowInfo("删除成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     生成导航
        /// </summary>
        private void tlbBulidMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (listExVo.ListID <= 0)
                {
                    MessageShowUtil.ShowInfo("未保存列表，不能生成导航菜单");
                    return;
                }


                var strsql = "select * from  BFT_Request where MenuURL='frmList' and MenuParams='ListID=" +
                             listExVo.ListID + "'";
                var dt = Model.GetDataTable(strsql);
                if ((dt == null) || (dt.Rows.Count == 0))
                {
                    MessageShowUtil.ShowInfo("未找到列表请求库,不能添加菜单");
                    return;
                }
                var strRequestCode = TypeUtil.ToString(dt.Rows[0]["RequestCode"]);

                var frm = new frmAddMenu(strRequestCode);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     方案
        /// </summary>
        private void tlbSchema_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (listExVo.ListID <= 0)
                {
                    MessageShowUtil.ShowInfo("未保存列表，不能设置方案");
                    return;
                }

                var frm = new frmSchemaDesign();
                frm.ListID = listExVo.ListID;
                frm.MetadataId = listExVo.MainMetaDataID;
                frm.StrListName = listExVo.StrListName;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void buttonEditMainMetaData_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                var frm = new frmMetaDataSelect();
                frm.ShowDialog();
                if (0 == frm.MetadataId)
                    return;

                listExVo.MainMetaDataID = frm.MetadataId;
                buttonEditMainMetaData.Text = GetMetaDataName(frm.MetadataId);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     列表权限
        /// </summary>
        private void btnListRight_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //try
            //{
            //    frmRightSelect frm = new frmRightSelect();
            //    frm.ShowDialog();
            //    if (!frm.BlnOk)
            //    {
            //        return;
            //    }

            //    this.listExVo.StrRightCode = frm.StrRightCode;
            //    this.btnListRight.Text = GetRightName(frm.StrRightCode);
            //}
            //catch (Exception ex)
            //{
            //    ShowInfo(ex.Message);
            //}
        }


        private void repositoryItemGridLookUpRight_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index != 1)
                return;

            try
            {
                var strRightCode =
                    TypeUtil.ToString(gridViewCmd.GetRowCellValue(gridViewCmd.FocusedRowHandle, "parameters"));
                gridViewCmd.CellValueChanged -= gridViewCmd_CellValueChanged;
                var multiDlgRef = new frmGenericRef();
                multiDlgRef.StrFkCode = "AllRight";
                multiDlgRef.BlnMulti = false;
                multiDlgRef.BlnSelectStr = true;
                multiDlgRef.StrSelectValue = strRightCode;
                multiDlgRef.ShowDialog();
                if (multiDlgRef.BlnOk)
                    gridViewCmd.SetRowCellValue(gridViewCmd.FocusedRowHandle, "parameters", multiDlgRef.StrSelectDisplay);

                gridViewCmd.CellValueChanged += gridViewCmd_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     获取权限名称
        /// </summary>
        /// <param name="strRightCode"></param>
        /// <returns></returns>
        private string GetRightName(string strRightCode)
        {
            var strRightName = "";
            //Right r = Model.GetVO<Webtek.DRP.BL.VO.Right>("from Right  r where r.StrRightCode='" + strRightCode + "'");
            //if (r != null)
            //{
            //    strRightName = r.StrRightName;
            //}

            return strRightName;
        }


        private void buttonEditParameters_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                gridViewCmd.CellValueChanged -= gridViewCmd_CellValueChanged;
                var rowHandle = gridViewCmd.FocusedRowHandle;
                var strquestCode = TypeUtil.ToString(gridViewCmd.GetRowCellValue(rowHandle, "strRequestCode"));
                var multiDlgRef = new frmGenericRef();
                multiDlgRef.StrFkCode = "AllRequest";
                multiDlgRef.BlnSelectStr = true;
                multiDlgRef.BlnMulti = false;
                //multiDlgRef.StrSelectValue = this._strKeyValue;
                multiDlgRef.ShowDialog();
                if (multiDlgRef.BlnOk)
                    gridViewCmd.SetRowCellValue(rowHandle, "strRequestCode", multiDlgRef.StrSelectDisplay);
                gridViewCmd.CellValueChanged += gridViewCmd_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void buttonEditDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                var rowHandle = gridViewCmd.FocusedRowHandle;
                var id = TypeUtil.ToInt(gridViewCmd.GetRowCellValue(rowHandle, "ListCommandID"));
                if (id > 0)
                    delCmdList.Add(id);

                gridViewCmd.DeleteRow(rowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }


        /// <summary>
        ///     打开等待对话框
        /// </summary>
        private void OpenWaitDialog(string caption)
        {
            wdf = new WaitDialogForm(caption + "...", "请等待...");
        }

        /// <summary>
        ///     关闭等待对话框
        /// </summary>
        private void CloseWaitDialog()
        {
            if (wdf != null)
                wdf.Close();
        }

        private void gridViewCmd_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            gridViewCmd.CellValueChanged -= gridViewCmd_CellValueChanged;

            var row = e.RowHandle;
            if (e.Column.Name == "gcstrListCommandName") //事件名称
            {
                gridViewCmd.SetRowCellValue(row, "lngRowIndex", GetMaxRowIndex(gridViewCmd, "lngRowIndex", 1));
                if (e.Value.Equals("新增"))
                {
                    gridViewCmd.SetRowCellValue(row, "strListCommandCode", "add");
                    gridViewCmd.SetRowCellValue(row, "strListCommandTip", "新增一条记录"); //提示
                    gridViewCmd.SetRowCellValue(row, "strListIconName", "add"); //图标
                    gridViewCmd.SetRowCellValue(row, "strWebListIconName", "icon-add"); //Web图标
                    gridViewCmd.SetRowCellValue(row, "requestId", "add");
                }
                else if (e.Value.Equals("修改"))
                {
                    gridViewCmd.SetRowCellValue(row, "strListCommandCode", "update");
                    gridViewCmd.SetRowCellValue(row, "strListCommandTip", "修改一条记录"); //提示
                    gridViewCmd.SetRowCellValue(row, "strListIconName", "edit"); //图标
                    gridViewCmd.SetRowCellValue(row, "strWebListIconName", "icon-edit"); //Web图标
                    gridViewCmd.SetRowCellValue(row, "blnDblClick", 1);
                    gridViewCmd.SetRowCellValue(row, "requestId", "modfiy");
                }
                else if (e.Value.Equals("删除"))
                {
                    gridViewCmd.SetRowCellValue(row, "strListCommandCode", "del");
                    gridViewCmd.SetRowCellValue(row, "strListCommandTip", "删除一条记录"); //提示
                    gridViewCmd.SetRowCellValue(row, "strListIconName", "delete"); //图标
                    gridViewCmd.SetRowCellValue(row, "strWebListIconName", "icon-delete"); //Web图标
                    gridViewCmd.SetRowCellValue(row, "requestId", "delete");
                }
            }

            gridViewCmd.CellValueChanged += gridViewCmd_CellValueChanged;
        }

        public int GetMaxRowIndex(GridView grid, string columnName, int lng)
        {
            var Result = 0;
            if (grid.RowCount == 0)
                Result = 1;
            var dt = new DataTable();
            dt.Columns.Add("RowIndex", Type.GetType("System.Int32"));
            for (var i = 0; i < grid.RowCount; i++)
            {
                var dr = dt.NewRow();
                dr["RowIndex"] = grid.GetRowCellValue(i, columnName);
                dt.Rows.Add(dr);
            }
            var dv = dt.DefaultView;
            dv.Sort = "RowIndex desc";
            dt = dv.ToTable();
            if (dt.Rows.Count > 0)
                if (TypeUtil.ToInt(dt.Rows[0][0]) < 0)
                    Result = TypeUtil.ToInt(dt.Rows[0][0]) + 2;
                else
                    Result = TypeUtil.ToInt(dt.Rows[0][0]) + 1*lng;
            return Result;
        }

        private void buttonEditMainMetaData_EditValueChanged(object sender, EventArgs e)
        {
            txtListName.Text = buttonEditMainMetaData.Text + "列表";
        }

        private void chkPivot_CheckedChanged(object sender, EventArgs e)
        {
        }


        private void repbtnSelectIcon_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                if (e.Button.Index == 0)
                {
                    var strPath = AppDomain.CurrentDomain.BaseDirectory + "Image\\ListReportImage";
                    if (!Directory.Exists(strPath))
                        Directory.CreateDirectory(strPath);
                    var openFileDialog = new OpenFileDialog();
                    openFileDialog.FileName = "";
                    openFileDialog.Filter = "png文件(*.png)|*.png|ico文件(*.ico)|*.ico";
                    openFileDialog.FilterIndex = 0;
                    openFileDialog.Title = "请选择图标";
                    openFileDialog.InitialDirectory = strPath;
                    openFileDialog.DefaultExt = ".png";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var strFilName = openFileDialog.SafeFileName;
                        gridViewCmd.SetRowCellValue(gridViewCmd.FocusedRowHandle, "strListIconName", strFilName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowInfo(ex.Message);
            }
        }
    }
}