using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Reports.Model;
using Sys.Safety.Reports.PubClass;
using Sys.Safety.ClientFramework.CBFCommon;

namespace Sys.Safety.Reports
{
    public partial class frmReportDesign : XtraForm
    {
        public bool blnInit; //是否为初化报表，如果是，将重新根据方案里的配置进行初始化模板
        public bool blnOK;
        public DataTable dtLmd = null;
        public DataTable dtReportDataSource = null;
        public ListdataexInfo listDataExDTO = null;
        private readonly ListExModel Model = new ListExModel();


        private readonly XtraReportTemple rp = new XtraReportTemple();
        public string StrFileName = "";


        public frmReportDesign()
        {
            InitializeComponent();
            commandBarItem31.Visibility = BarItemVisibility.Never;
        }


        private void frmReportDesign_Load(object sender, EventArgs e)
        {
            try
            {
                var strPath = StrFileName;
                if (!File.Exists(strPath) || blnInit)
                {
//如果不存在或者点击的初始化，那么就创建一个模板文件
                    var frm = new frmList(listDataExDTO.ListID);
                    frm.strReportFileName = listDataExDTO.StrListDataName + listDataExDTO.ListDataID;
                    frm.listDisplayExList = Model.GetListDisplayExData(listDataExDTO.ListDataID);
                    frm.SetPivotFormat();
                }
                if (File.Exists(strPath))
                {
//存在后直接加载模板
                    rp.LoadLayout(strPath);
                    var dt = dtReportDataSource.Copy();

                    #region // 2015-02-05 由于需要要求选择绑定列的时候需要显示为中文，所以根据字段名得到其字段中文名

                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        if (dt.Columns[i].ColumnName == "colNumber") continue;
                        var rows =
                            dtLmd.Select("MetaDataFieldName='" + dt.Columns[i].ColumnName +
                                         "' and blnSysProcess=0  and blnShow=1");
                        if ((rows != null) && (rows.Length > 0))
                            dt.Columns[i].ColumnName = rows[0]["strListDisplayFieldNameCHS"].ToString();
                        else
                        {
                            dt.Columns.RemoveAt(i);
                            i--;
                        }
                    }
                    //将Detail区域的数据源绑定中文,然后在保存的时候再转为英文
                    var xrTable = rp.FindControl("TableReportDetail", true) as XRTable;
                    var xrRow = xrTable.Rows[0];
                    foreach (XRTableCell cell in xrRow.Cells)
                    {
                        if ((cell.DataBindings == null) || (cell.DataBindings.Count == 0)) continue;
                        var strTag = cell.Tag.ToString();
                        var strBindingFileName = cell.DataBindings["Text"].DataMember;
                        var rows = dtLmd.Select("MetaDataFieldName='" + strBindingFileName + "'"); //通过英文名去找中文名
                        if ((rows != null) && (rows.Length > 0))
                        {
                            var strColumCHSName = rows[0]["strListDisplayFieldNameCHS"].ToString();
                            var strdatasource = TypeUtil.ToString(cell.DataBindings["Text"].DataSource);
                            var strdatamerber = TypeUtil.ToString(cell.DataBindings["Text"].DataMember);
                            var strformat = cell.DataBindings["Text"].FormatString;
                            cell.DataBindings.Clear();
                            cell.DataBindings.Add("Text", strdatasource, strColumCHSName, strformat);
                        }
                    }

                    #endregion

                    rp.DataSource = dt;
                    rp.Name = "报表模板";
                    var fs = MdiChildren;
                    foreach (var f in fs)
                        f.Close();
                    reportDesigner1.OpenReport(rp);
                }
                else
                {
                    MessageBox.Show("未找到报表模板文件,请先打开一次报表！");
                }
                SetVisible();

                ribbonPage2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void SetVisible()
        {
            commandBarItem31.Visibility = BarItemVisibility.Never; //新建报表
            commandBarItem34.Visibility = BarItemVisibility.Never; //保存报表
            scriptsCommandBarItem1.Visibility = BarItemVisibility.Never; //脚本
            //commandBarItem33.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//全部保存
            panelContainer3.Visibility = DockVisibility.Hidden; //分组排序
            xrDesignRibbonPageGroup7.Visible = false;

            commandBarItem33.Caption = "保存";
            tlbSaveReport.Caption = "保存到本地";
            tlbSaveReport.Visibility = BarItemVisibility.Never;

            PermissionManager.HavePermission("InitReportDesign", tlbInitReport);
            PermissionManager.HavePermission("SaveReportDesign", commandBarItem33);
        }

        private void commandBarItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveReportDesign"))
                {
                    return;
                }
                commandBarItem37.Command = ReportCommand.None;
                var fs = MdiChildren;
                foreach (var f in fs)
                    foreach (Control c in f.Controls)
                    {
                        //目前测试两种方法存盘都能够在load事件里正常加载
                        var p = (XRDesignPanel) c;
                        p.FileName = StrFileName;
                        p.SaveReport(p.FileName);


                        //XRTable tablehead = (XRTable)p.Report.FindControl("TablePageHeader", true);
                        //if (tablehead.Rows.Count > 0)
                        //{
                        //    XRTableRow row = tablehead.Rows[0];
                        //    foreach (XRTableCell cell in row.Cells)
                        //    {
                        //        string strcelltag = cell.Tag.ToString();// tag属性存的FileName
                        //        DataRow[] rows = dtLmd.Select("MetaDataFieldName='" + strcelltag + "'");
                        //        foreach (DataRow r in rows)
                        //        {
                        //            r["LngDisplayWidth"] = cell.WidthF;
                        //        }

                        //    }
                        //}

                        //XtraReport report = p.Report;
                        //report.SaveLayout("E:\\ywlayount.xml");
                        //MessageBox.Show("保存报表成功");
                    }
                blnOK = true;
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }


        /// <summary>
        ///     保存报表事件(保存到本地)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSaveReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            var a = ribbonPage2.Collection;
            var b = a.Category;
            //b.Tag

            commandBarItem37_ItemClick(null, null);
        }


        /// <summary>
        ///     保存报表事件(保存到服务器)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandBarItem33_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!PermissionManager.HavePermission("SaveReportDesign"))
                {
                    return;
                }

                var fs = MdiChildren;

                //先保存到本地，然后再把本地文件保存到数据库
                foreach (var f in fs)
                    foreach (Control c in f.Controls)
                    {
                        //目前测试两种方法存盘都能够在load事件里正常加载
                        var p = (XRDesignPanel) c;
                        p.FileName = StrFileName;


                        p.SaveReport(p.FileName);

                        #region 2015-02-05 保存后将绑定的中文字段名转换为英文名，这样才能在报表中正常显示

                        var str = File.ReadAllText(p.FileName);
                        var xrTable = p.Report.FindControl("TableReportDetail", true) as XRTable;
                        var xrRow = xrTable.Rows[0];
                        foreach (XRTableCell cell in xrRow.Cells)
                            if ((cell.DataBindings != null) && (cell.DataBindings.Count > 0))
                            {
                                var strBingName = cell.DataBindings[0].DataMember;
                                var rows =
                                    dtLmd.Select("strListDisplayFieldNameCHS='" + strBingName +
                                                 "' and blnSysProcess=0 and blnShow=1");
                                if ((rows != null) && (rows.Length > 0))
                                {
                                    var strFileName = rows[0]["MetaDataFieldName"].ToString();
                                    str = str.Replace(",\"" + strBingName + "\"", ",\"" + strFileName + "\"");
                                    str = str.Replace(", \"" + strBingName + "\"", ", \"" + strFileName + "\"");
                                }
                            }

                        foreach (Band band in rp.Bands)
                            foreach (XRControl control in band.Controls)
                            {
                                var strname = control.Name;
                                if ((control.DataBindings != null) && (control.DataBindings.Count > 0))
                                {
                                    var strBingName = control.DataBindings[0].DataMember;
                                    var rows =
                                        dtLmd.Select("strListDisplayFieldNameCHS='" + strBingName +
                                                     "' and blnSysProcess=0 and blnShow=1");
                                    if ((rows != null) && (rows.Length > 0))
                                    {
                                        var strFileName = rows[0]["MetaDataFieldName"].ToString();
                                        str = str.Replace(",\"" + strBingName + "\"", ",\"" + strFileName + "\"");
                                        str = str.Replace(", \"" + strBingName + "\"", ", \"" + strFileName + "\"");
                                    }
                                }
                            }
                        File.WriteAllText(p.FileName, str);

                        #endregion
                    }


                commandBarItem33.Command = ReportCommand.None;
                var b = GetBinaryData(StrFileName);
                if (b.Length > 00)
                {
                    var Model = new ListTempleModel();
                    ListtempleInfo dto = Model.GetListTempleDTOByID(listDataExDTO.ListDataID);
                    if ((dto != null) && (dto.ListTempleID > 0))
                    {
                        if (DialogResult.No ==
                            MessageBox.Show("保存到服务器会覆盖服务器记录，确认要覆盖？", "提示", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question))
                            return;
                        dto.InfoState = InfoState.Modified;
                    }
                    else
                    {
                        dto = new ListtempleInfo();
                        dto.InfoState = InfoState.AddNew;
                    }
                    dto.BloImage = b;
                    dto.ListDataID = listDataExDTO.ListDataID;
                    dto.LngFileSize = b.Length;
                    dto.StrFileName = GetRealFileName(StrFileName);
                    Model.SaveListTemple(dto);
                    blnOK = true;

                    //保存操作记录
                    var listDataId = listDataExDTO.ListDataID;
                    var dtName = Model.GetNameFromListDataExListEx(listDataId);
                    string listName = dtName.Rows[0]["strListName"].ToString();
                    string listCode = dtName.Rows[0]["strListCode"].ToString();
                    string listDataName = dtName.Rows[0]["strListDataName"].ToString();
                    string log = "保存报表模板。报表名称【" + listName + "】，报表编码【" + listCode + "】，方案名称【" + listDataName + "】";
                    OperateLogHelper.InsertOperateLog(3, log, "");

                    MessageBox.Show("保存成功");

                    
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private byte[] GetBinaryData(string filePath)
        {
            var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
            var data = new byte[fs.Length];
            fs.Read(data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return data;
        }

        /// <summary>
        ///     返回文件名
        /// </summary>
        /// <param name="strPach"></param>
        /// <returns></returns>
        private string GetRealFileName(string strPach)
        {
            return strPach.Substring(strPach.LastIndexOf("\\") + 1, strPach.Length - (strPach.LastIndexOf("\\") + 1));
        }

        private void tlbInitReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!PermissionManager.HavePermission("InitReportDesign"))
            {
                return;
            }

            if (DialogResult.No ==
                MessageBox.Show("确定要初始化吗？初始化后之前保存的模板将会被覆盖！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                return;
            try
            {
                File.Delete(StrFileName);
                blnInit = true;
                frmReportDesign_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbExportCurTemple_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "报表文件(*.repx)|*.repx";
                saveFileDialog.Title = "保存报表";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = listDataExDTO.StrListDataName + listDataExDTO.ListDataID;
                saveFileDialog.FilterIndex = 0;


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var strFilepach = saveFileDialog.FileName;
                    var fileData = GetBinaryData(StrFileName);
                    var buffer = fileData.GetUpperBound(0) + 1;
                    var fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                    fs.Write(fileData, 0, buffer);
                    fs.Close();
                    MessageShowUtil.ShowMsg("成功导出1个报表文件。");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowMsg(ex.Message);
            }
        }

        private void tlbExportAllTemple_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var strReportDir = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple";
                if (Directory.Exists(strReportDir))
                {
                    var str = Directory.GetFiles(strReportDir);
                    var fold = new FolderBrowserDialog();
                    if (fold.ShowDialog() == DialogResult.OK)
                    {
                        foreach (var strsourcefile in str)
                        {
                            var strFileName = GetRealFileName(strsourcefile);
                            File.Copy(strsourcefile, fold.SelectedPath + "\\" + strFileName, true);
                        }
                        MessageShowUtil.ShowMsg("成功导出" + str.Length + "个报表文件。");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbImportTemple_ItemClick(object sender, ItemClickEventArgs e)
        {
            ImportRepotTemple(false);
        }

        private void tlbImportAllTemple_ItemClick(object sender, ItemClickEventArgs e)
        {
            ImportRepotTemple(true);
        }

        private void ImportRepotTemple(bool blnMuliSelect)
        {
            try
            {
                var strReportDir = AppDomain.CurrentDomain.BaseDirectory + "Config\\ReportTemple";
                var openFileDialog = new OpenFileDialog();
                openFileDialog.FileName = GetRealFileName(StrFileName);
                openFileDialog.Filter = "打开文件(*.repx)|*.repx";
                openFileDialog.Title = "请选择要导入的报表文件";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var strfile in openFileDialog.FileNames)
                        File.Copy(strfile, strReportDir + "\\" + GetRealFileName(strfile), true);
                    MessageShowUtil.ShowInfo("成功导入" + openFileDialog.FileNames.Length + "个报表文件件。");
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }
    }
}