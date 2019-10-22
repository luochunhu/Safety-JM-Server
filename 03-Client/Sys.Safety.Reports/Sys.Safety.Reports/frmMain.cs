using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Model;
using Sys.Safety.Reports.PubClass;

namespace Sys.Safety.Reports
{
    public partial class frmMain : XtraForm
    {
        private readonly ListExModel Model = new ListExModel();

        public frmMain()
        {
            InitializeComponent();

            #region 将报表用到的视图用存在磁盘，以免费丢失，仅在调试的时候使用

            try
            {
//                string strsql = "select * from DataExchangeSetting";
//                DataTable dt = Model.GetDataTable(strsql);
//                List<string> listsType = new List<string>();
//                listsType.Add("INSERT");
//                listsType.Add("UPDATE");
//                listsType.Add("DELETE");

//                foreach (DataRow row in dt.Rows)
//                {
//                    string strTableName = Convert.ToString(row["Code"]);

//                    foreach (string strType in listsType)
//                    {
//                        string strcreatesql = string.Format(@"DROP TRIGGER IF EXISTS trigger_{0}_{1}; 
//                        CREATE TRIGGER trigger_{0}_{1} 
//                        AFTER {1} ON {0} 
//                        FOR EACH ROW 
//                        BEGIN                  
//                                update DataExchangeSetting  set datLastExportTime=now() where Code='{0}';
//                        END", strTableName,strType);
//                            Model.ExecuteSQL(strcreatesql);
//                    }
//                }


                //string strsql = "select * from information_schema.VIEWS where TABLE_SCHEMA= 'masx'";
                //DataTable dt = Model.GetDataTable(strsql);
                //string path = @"d:\viewsql.txt";
                //File.WriteAllText(path, DateTime.Now.ToString() + "\r\n");
                //foreach (DataRow row in dt.Rows)
                //{
                //    string strViewName = TypeUtil.ToString(row["TABLE_NAME"]);
                //    string sql = TypeUtil.ToString(row["VIEW_DEFINITION"]);
                //    string strText = "视图名" + strViewName + "\r\n" + sql + "\r\n\r\n";
                //    File.AppendAllText(path, strText);

                //}
            }
            catch
            {
            }
            //string strsql = "select * from jc_def";
            //DataTable dt = Model.GetDataTable(strsql);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    long ID = Convert.ToInt64(dt.Rows[i]["ID"]);
            //    strsql = "update jc_def set bz13='MAS" + DateTime.Now.ToString("yyyyMMdd") + i.ToString() + "' where ID=" + ID;
            //    Model.ExecuteSQL(strsql);
            //}

            #endregion
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //DateTime datetime = DateTime.Now.AddDays(-17);
                //for (int i = 0; i < 300; i++)
                //{
                //    datetime = datetime.AddDays(1);
                //    if (datetime.Month > 8) break;
                //    string strdate = datetime.ToString("yyyyMMdd");
                //    string TableName = "AS_LyTal" + strdate;
                //    string strsql = "CREATE TABLE " + TableName + " (	[id] [int] NOT NULL,	[kbh] [smallint] NULL,	[gcbh] [smallint] NULL,	[systype] [smallint] NULL,	[point] [nvarchar](30) NULL,	[fzh] [smallint] NULL,	[kh] [smallint] NULL,	[wz] [nvarchar](50) NULL,	[jsid] [nvarchar](50) NULL,	[sim] [nvarchar](20) NULL,	[type] [tinyint] NULL,	[value] [smallint] NULL,	[rtime] [datetime] NULL,	[Remark] [nvarchar](50) NULL,	[by1] [ntext] NULL,	[by2] [ntext] NULL,	[by3] [ntext] NULL,	[upflag] [nvarchar](1) NULL) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
                //    Model.ExecuteSQL(strsql);
                //    barStatic.Caption = i.ToString();
                //}
                LoadForm();
                SetControlVisible();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        /// <summary>
        ///     载入界面
        /// </summary>
        private void LoadForm()
        {
            treeListManager.DataSource = Model.GetListDir();
            treeListManager.ExpandAll();
            //treeListManager.SetFocusedNode(treeListManager.Nodes.FirstNode);
            RefreshGrid();

            treeListManager.Focus();
        }

        private void SetControlVisible()
        {
            PermissionManager.HavePermission("AddReportDirectory", tlbCreatDir);
            PermissionManager.HavePermission("DeleteReportDirectory", tlbDeleteDir);
            PermissionManager.HavePermission("ReNameReportDirectory", tlbRenameDir);
            PermissionManager.HavePermission("AddReport", tlbListAdd);
            PermissionManager.HavePermission("EditReport", tlbListEdit);
            PermissionManager.HavePermission("DeleteReport", tlbListDelete);
            PermissionManager.HavePermission("ImportExportReport", barSubImportExport);
        }

        /// <summary>
        ///     刷新Grid,并得到数的节点的子节点下的所有列表和目录
        /// </summary>
        private void RefreshGrid()
        {
            var name = "";
            try
            {
                name = treeListManager.FocusedNode.GetDisplayText(treeListColumnName);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowInfo("没有找到相关列表");
                return;
            }
            layoutControlGroupName.Text = "‘" + name + "’的列表";

            //保持焦点位置不变
            var ListID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));
            gridControl1.DataSource = Model.GetChildListDir(ListID);
            barStatic.Caption = "共有" + gridView1.RowCount + "张列表";
        }


        private void tlbCreatDir_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("AddReportDirectory"))
                {
                    return;
                }
                //if (treeListManager.FocusedNode == null)
                //{
                //    MessageShowUtil.ShowMsg("请选择父级目录");
                //    return;
                //}

                var parentDirDt = Model.GetListDir();
                var frm = new frmListDirName();
                frm.ParentDirDt = parentDirDt;
                if (treeListManager.FocusedNode != null)
                    frm.ParentDirID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));
                else
                    frm.ParentDirID = 0;
                frm.ShowDialog();

                if (frm.DirName != string.Empty)
                {
                    var listexdto = new ListexInfo();
                    listexdto.InfoState = InfoState.AddNew;
                    listexdto.BlnList = false;
                    listexdto.BlnEnable = true;
                    listexdto.StrListCode = frm.DirName;
                    listexdto.StrListName = frm.DirName;
                    listexdto.DirID = frm.ParentDirID;
                    Model.SaveVO(listexdto);

                    LoadForm();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }


            //以下为测试代码
            //IBFT_ListDataExService servicel = ServiceFactory.CreateService<IBFT_ListDataExService>();
            //BFT_ListDataExDTO a = servicel.GetById(4);


            //CustomerinfoDTO dto = service.GetById(3);
            //MessageBox.Show(dto.Name);

            //IBFT_ListExService service1= ServiceFactory.CreateService<IBFT_ListExService>();
            //BFT_ListExDTO dto1 = service1.GetById(2);

            //IList<BFT_ListExDTO> all = service1.GetAll();

            //BFT_ListExDTO ab = service1.GetByHQL<BFT_ListExDTO>("from BFT_ListExDTO where Listid=2");
        }

        /// <summary>
        ///     单击目录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListManager_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var dirId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DirID"));
                var listId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ListID"));
                var blnList = TypeUtil.ToBool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "blnList"));

                if (blnList)
                {
                    var strListCode =
                        TypeUtil.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "strListCode"));
                    var strRightCode =
                        TypeUtil.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "strRightCode"));


                    var frm = new frmList(listId);
                    if ((ParentForm != null) && ParentForm.IsMdiContainer)
                        frm.MdiParent = ParentForm;

                    frm.Show();


                    //string paramters = "Entity=" + strListCode + "&ListID=" + listId;
                    //RequestUtil.ExcuteCommand("ListEx", paramters, strRightCode);
                }
                else
                {
                    treeListManager.SetFocusedNode(treeListManager.FindNodeByFieldValue("ListID", listId));
                    RefreshGrid();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetTitle(object sender, ListTitleEventArgs args)
        {
            MessageBox.Show(args.StrTitle);
        }

        private void treeListManager_GetSelectImage(object sender, GetSelectImageEventArgs e)
        {
            if (e.FocusedNode)
                e.NodeImageIndex = 2;
            else
                e.NodeImageIndex = 1;
        }

        private void tlbLastDir_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var node = treeListManager.FocusedNode;
                if (node.Level == 0) return;
                treeListManager.SetFocusedNode(node.ParentNode);
                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbRefreshDir_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadForm();
        }

        /// <summary>
        ///     删除目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteDir_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("DeleteReportDirectory"))
                {
                    return;
                }
                if (treeListManager.FocusedNode == null)
                {
                    MessageShowUtil.ShowInfo("请选择目录");
                    return;
                }

                if (TypeUtil.ToBool(treeListManager.FocusedNode.GetValue("blnPredefine")))
                {
                    MessageShowUtil.ShowInfo("预制目录不能删除");
                    return;
                }

                var listId = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));
                if (Model.IsExistChildListDir(listId))
                {
                    MessageShowUtil.ShowInfo("存在子级目录或者列表,不能删除");
                    return;
                }

                if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前目录?"))
                    return;

                var listex = new ListexInfo();
                listex.InfoState = InfoState.Delete;
                listex.ListID = listId;
                Model.SaveVO(listex);
                LoadForm();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbRenameDir_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    DateTime datfrom = TypeUtil.ToDateTime("2015-01-01");
            //    DateTime datto = TypeUtil.ToDateTime("2015-02-04");
            //    TimeSpan span = datto - datfrom;
            //    for (int i = 0; i < 100; i++)
            //    {
            //        if (datfrom <= datto)
            //        {
            //            string date = datfrom.ToString("yyyyMMdd");
            //            string datea = datfrom.ToString("yyyy-MM-dd");
            //            string strsql = @"if not exists(select * from syscolumns where id=object_id('JC_M" + date + "') and name='Timer') alter   table   JC_M" + date + "   add    Timer  datetime ";
            //            strsql += @"if not exists(select * from syscolumns where id=object_id('JC_M" + date + "') and name='Point')   alter   table   JC_M" + date + "   add    Point  varchar(50) ";
            //            strsql += @"if not exists(select * from syscolumns where id=object_id('JC_B" + date + "') and name='isalarm')   alter   table   JC_B" + date + "   add    isalarm  bit ";
            //            strsql += @"if not exists(select * from syscolumns where id=object_id('JC_B" + date + "') and name='kdid')   alter   table   JC_B" + date + "   add    kdid  varchar(50) ";

            //            Model.GetDataTable(strsql);

            //            strsql = " update dbo.JC_M" + date + " set Timer = '" + datea + " '+SUBSTRING(RIGHT(CAST(POWER(10, 4) AS varchar) + CONVERT(nvarchar, sj), 4), 0, 3)  + ':' + SUBSTRING(RIGHT(CAST(POWER(10, 4) AS varchar) + CONVERT(nvarchar, sj), 4), 3, 5) + ':00' \r\n";
            //            strsql += " update JC_M" + date + " set Point = RIGHT(CAST(POWER(10, 3) AS varchar) + CAST(fzh as varchar), 3)+'A'+RIGHT(CAST(POWER(10, 2) AS varchar) + CAST(kh as varchar), 2) ";
            //            Model.GetDataTable(strsql);

            //            datfrom = datfrom.AddDays(1);
            //        }
            //    }


            //}
            //catch (System.Exception ex)
            //{

            //}


            try
            {
                if (!PermissionManager.HavePermission("ReNameReportDirectory"))
                {
                    return;
                }
                if (treeListManager.FocusedNode == null)
                {
                    MessageShowUtil.ShowMsg("请选择目录");
                    return;
                }
                var frm = new frmListDirName();
                frm.DirName = TypeUtil.ToString(treeListManager.FocusedNode.GetValue("strListName"));
                frm.ShowDialog();
                if (frm.DirName != string.Empty)
                {
                    var listexdto = new ListexInfo();
                    listexdto.InfoState = InfoState.Modified;
                    listexdto.BlnList = false;
                    listexdto.BlnEnable = true;
                    listexdto.StrListCode = TypeUtil.ToString(treeListManager.FocusedNode.GetValue("strListCode"));
                    listexdto.StrListName = frm.DirName;
                    listexdto.BlnPredefine = TypeUtil.ToBool(treeListManager.FocusedNode.GetValue("blnPredefine"));
                    listexdto.ListID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));
                    listexdto.DirID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("DirID"));
                    Model.SaveVO(listexdto);
                    LoadForm();
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        //void RequestUtil_OnRereshReport(object sender, Win.View.MainForm.OnRereshReportEventArgs e)
        //{
        //    //this.RefreshListData();

        //}
        private void tlbListAdd_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("AddReport"))
                {
                    return;
                }
                var listId = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));

                var frm = new frmListEdit(0);
                frm.DirID = listId;
                frm.RefreshListDir += frm_RefreshListDir;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbListEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("EditReport"))
                {
                    return;
                }
                var dirId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DirID"));
                var listId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ListID"));
                var blnList = TypeUtil.ToBool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "blnList"));

                if (blnList)
                {
                    var frm = new frmListEdit(listId);
                    frm.DirID = dirId;
                    frm.RefreshListDir += frm_RefreshListDir;
                    frm.ShowDialog();
                }
                else
                {
                    MessageShowUtil.ShowInfo("请选择列表");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbListDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("DeleteReport"))
                {
                    return;
                }
                var dirId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "DirID"));
                var listId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ListID"));
                var blnList = TypeUtil.ToBool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "blnList"));

                if (blnList)
                {
                    if (TypeUtil.ToBool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "blnPredefine")))
                    {
                        MessageShowUtil.ShowInfo("预制列表不能删除");
                        return;
                    }

                    if (DialogResult.No == MessageShowUtil.ReturnDialogResult("确认要删除当前列表?"))
                        return;

                    Model.DeleteList(listId);

                    frm_RefreshListDir(dirId);

                    MessageShowUtil.ShowInfo("删除成功");
                }
                else
                {
                    MessageShowUtil.ShowInfo("请选择列表");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void barSubImportExport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                PermissionManager.HavePermission("ImportExportReport");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        private void frm_RefreshListDir(int dirId)
        {
            var ListID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID"));
            if (dirId == ListID)
                RefreshGrid();
        }

        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void treeListManager_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            RefreshGrid();
            Cursor = Cursors.Default;
        }

        private void treeListManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Right))
                gridView1.Focus();
            else if (e.KeyCode.Equals(Keys.Enter))
                treeListManager.FocusedNode.ExpandAll();
        }


        /// <summary>
        ///     导出元数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbExportMetaData_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var dataset = new DataSet("datasetMetaData");
                var dtmetadata = Model.GetDataTable("select * from BFT_MetaData");
                dtmetadata.TableName = "BFT_MetaData";
                var dtmetadatafiles = Model.GetDataTable("select * from BFT_MetaDataFields");
                dtmetadatafiles.TableName = "BFT_MetaDataFields";
                dataset.Tables.Add(dtmetadata);
                dataset.Tables.Add(dtmetadatafiles);

                var strpath = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportUpdate";
                if (!Directory.Exists(strpath))
                    Directory.CreateDirectory(strpath);
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "metadata";
                saveFileDialog.Filter = "xml文件(*.xml)|*.xml";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.Title = "保存文件";
                saveFileDialog.InitialDirectory = strpath;
                saveFileDialog.DefaultExt = ".xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageShowUtil.OpenWaitDialog("正在导出");
                    TypeUtil.SerializeSys(dataset, saveFileDialog.FileName);
                    //TypeUtil.SerializeSys(dataset, strpath + "\\" + GetRealFileName(saveFileDialog.FileName));
                    MessageShowUtil.ShowStaticInfo("导出元数据成功", barStatic);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                MessageShowUtil.CloseWaitDialog();
            }
        }

        /// <summary>
        ///     导入元数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbImportMetaData_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var strpath = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportUpdate";
                var openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "masmetadata.xml";
                openFileDialog.Filter = "xml文件(*.xml)|*.xml";
                openFileDialog.FilterIndex = 0;
                openFileDialog.Title = "请选择文件";
                openFileDialog.InitialDirectory = strpath;
                openFileDialog.DefaultExt = ".xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageShowUtil.OpenWaitDialog("正在导入");
                    var strFileName = openFileDialog.FileName;
                    var dataSet = new DataSet("datasetMetaData");
                    dataSet = TypeUtil.DeSerializeSys(dataSet, strFileName) as DataSet;
                    Model.ImportMetadata(dataSet);
                    MessageShowUtil.ShowStaticInfo("导入元数据成功", barStatic);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                MessageShowUtil.CloseWaitDialog();
            }
        }


        /// <summary>
        ///     导出报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbExportReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var listId = TypeUtil.ToInt(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "ListID"));
                var blnList = TypeUtil.ToBool(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "blnList"));
                if (!blnList)
                {
                    MessageShowUtil.ShowInfo("请选择一张具体报表进行导出！");
                    return;
                }

                ListexInfo listex = Model.OrgListExByExport(listId);
                var strpath = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportUpdate";
                if (!Directory.Exists(strpath))
                    Directory.CreateDirectory(strpath);
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = listex.StrListCode;
                saveFileDialog.Filter = "xml文件(*.xml)|*.xml";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.Title = "保存文件";
                saveFileDialog.InitialDirectory = strpath;
                saveFileDialog.DefaultExt = ".xml";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageShowUtil.OpenWaitDialog("正在导出");
                    TypeUtil.SerializeSys(listex, saveFileDialog.FileName);
                    //TypeUtil.SerializeSys(listex, strpath + "\\" + this.GetRealFileName(saveFileDialog.FileName));
                    MessageShowUtil.ShowStaticInfo("导出报表成功", barStatic);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                MessageShowUtil.CloseWaitDialog();
            }
        }


        /// <summary>
        ///     导入报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbImportReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var strpath = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportUpdate";
                var openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = "";
                openFileDialog.Filter = "xml文件(*.xml)|*.xml";
                openFileDialog.FilterIndex = 0;
                openFileDialog.Title = "请选择文件";
                openFileDialog.InitialDirectory = strpath;
                openFileDialog.DefaultExt = ".xml";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageShowUtil.OpenWaitDialog("正在导入");
                    var strFileName = openFileDialog.FileName;
                    var listEx = new ListexInfo();
                    listEx = TypeUtil.DeSerializeSys(listEx, strFileName) as ListexInfo;
                    var DirID = TypeUtil.ToInt(treeListManager.FocusedNode.GetValue("ListID")); //要导入的目录
                    listEx.DirID = DirID;
                    Model.ImportReport(listEx);
                    frm_RefreshListDir(DirID);
                    MessageShowUtil.ShowStaticInfo("导入报表成功", barStatic);
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
            finally
            {
                MessageShowUtil.CloseWaitDialog();
            }
        }


        private string GetRealFileName(string strPach)
        {
            return strPach.Substring(strPach.LastIndexOf("\\") + 1, strPach.Length - (strPach.LastIndexOf("\\") + 1));
        }
    }
}