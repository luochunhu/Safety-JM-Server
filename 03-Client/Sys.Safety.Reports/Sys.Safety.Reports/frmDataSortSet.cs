using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.Custom;
using Sys.Safety.Reports.Controls;
using Sys.Safety.Reports.Model;
using Sys.Safety.Reports.ServiceRequest;
using Exception = System.Exception;

namespace Sys.Safety.Reports
{
    public partial class frmDataSortSet : XtraForm
    {
        // 20180124
        /// <summary>复制的测点
        /// 
        /// </summary>
        private static List<string> _copyPoint;

        // 20180117
        private readonly DataTable _waitintPoint = new DataTable();

        private string _strDisplayValue = "";
        private string _strKeyValue = "";
        public bool blnOK;
        private GridHitInfo downHitInfo; //鼠标左键按下去时在GridView中的坐标
        private readonly DataTable dt = new DataTable();
        public DataTable dtLmd = null; //列表元数据
        public DataTable dtMetaDataFiles; //元数据字段
        public int ListDataID = 0;
        private Hashtable lookInfo;
        public int MetadataID = 0; //主元数据ID
        private readonly ListExModel Model = new ListExModel();
        private int[] rows; //拖拽的所有行
        private int startRow; // 拖拽的第一行
        public string strCondition = "";
        public string strConditionCHS = "";
        public string strFileName = ""; //方案里面点击进行编排的字段名
        public string strFKCode = "";
        public string strListDisplayName = "";
        private GridHitInfo upHitInfo; //鼠标左键弹起来时在GridView中的坐标
        private List<ListdatalayountInfo> ListdatalayountInfo;      //编排时间列表数据源

        /// <summary>需要移动项的索引
        /// 
        /// </summary>
        private string _selectMoveIndex;

        public frmDataSortSet()
        {
            InitializeComponent();
            dtMetaDataFiles = ClientCacheModel.GetServerMetaDataFields();
            gridView1.OptionsSelection.MultiSelect = true; //确保能够多选
            //dateEdit1.EditValueChanged -= dateEdit1_EditValueChanged;
            //dateEdit1.DateTime = DateTime.Now;
            //dateEdit1.EditValueChanged += dateEdit1_EditValueChanged;
        }

        private void frmDataSortSet_Load(object sender, EventArgs e)
        {
            CreateDataTable();

            // 20170824
            //BingdingGrid(true);
            BindingArrangeTimeList();

            Text = strListDisplayName + Text;

            // 20180117
            // 20180116
            //bool blnNewData = false;
            //var lookInfo = LookUpUtil.GetlookInfo("AllPointActivity", ref blnNewData);
            //var dt = (lookInfo["dataSource"] as DataTable).Copy();
            //comboBoxPoint.DataSource = dt;
            //comboBoxPoint.SelectedIndex = -1;
            lookUpEditSubstation.Properties.NullText = "未选择";

            _waitintPoint.Columns.Add("PointID");
            _waitintPoint.Columns.Add("Point");
            _waitintPoint.Columns.Add("Wz");
            _waitintPoint.Columns.Add("DevName");

            //dateEditNewArrange.DateTime = DateTime.Now;
            NewArrangeName.Text = DateTime.Now.ToString("yyyy-MM-dd");

            //comboBoxProperty.SelectedIndex = 0;
            lookUpEditFiltrate.Properties.NullText = "未选择";
            var data = new List<Kvp>()
            {
                new Kvp {Id = "1", Text = "按性质"},
                new Kvp {Id = "2", Text = "按分站"},
                new Kvp {Id = "3", Text = "按种类"}
            };
            lookUpEditFiltrate.Properties.DataSource = data;
        }

        /// <summary>
        ///     根据FK动态创建GridView的列和DataTable的列
        /// </summary>
        private void CreateDataTable()
        {
            //读取参照表的配置，并进行解析
            var blnNewData = false;

            //lookInfo = LookUpUtil.GetlookInfo(strFKCode, ref blnNewData);
            lookInfo = LookUpUtil.GetlookInfo("AllPointActivity", ref blnNewData);

            var strColumns = lookInfo["StrColumns"] as string;
            strColumns = strColumns.Trim();
            var fieldName = strColumns.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            //var width = 0;
            GridColumn col = null;
            //var strValueMember = lookInfo["StrValueMember"] as string;

            for (var i = 0; i < fieldName.Length; i++)
            {
                var subFieldName = fieldName[i].Split(',');
                //width = 0;
                //if (subFieldName.Length >= 3)
                //    width = TypeUtil.ToInt(subFieldName[2]);
                col = new GridColumn();
                col.FieldName = subFieldName[0];
                col.Caption = subFieldName[1];
                col.Width = TypeUtil.ToInt(subFieldName[2]);

                // 20180117
                // 20170825
                col.Visible = col.Width > 0 ? true : false;
                //col.Visible = subFieldName[0] == "point" ? true : false;

                gridView1.Columns.Add(col);
                dt.Columns.Add(new DataColumn(subFieldName[0], Type.GetType("System.String")));
            }
        }

        /// <summary>
        /// 初始化编排时间列表
        /// </summary>
        private void BindingArrangeTimeList()
        {
            var ListdatalayountInfo = Model.GetListDataLayountData(ListDataID).ToList();
            //ListdatalayountInfo = lisDataLay.OrderByDescending(a => a.StrDate).ToList();

            listBoxControlArrangeTime.ValueMember = "ListDataLayoutID";
            listBoxControlArrangeTime.DisplayMember = "StrDate";
            listBoxControlArrangeTime.DataSource = ListdatalayountInfo;

            if (ListdatalayountInfo.Count == 0)
            {
                dt.Clear();
            }
        }

        /// <summary>
        /// 绑定测点列表
        /// </summary>
        /// <param name="listDataLayId"></param>
        /// <param name="time"></param>
        private void BindingPointGrid(long listDataLayId, string time)
        {
            IList<ListdatalayountInfo> lists = Model.GetListDataLayoutDataA(ListDataID, time);
            dt.Clear();

            if (lists.Count > 0)
            {
                strCondition = lists[0].StrCondition;
                strConditionCHS = lists[0].StrConditionCHS;
            }

            if (strCondition.Length > 0)
            {
                var strIDs = strCondition.Substring(5).Split(',');
                for (var i = 0; i < strIDs.Length; i++)
                {
                    var strKey = strIDs[i].Replace("'", "");
                    var drNew = dt.NewRow();
                    drNew["point"] = strKey;
                    dt.Rows.Add(drNew);
                }
            }

            //bool blnNewData = false;
            //Hashtable lookInfo = LookUpUtil.GetlookInfo("AllPointActivity", ref blnNewData);
            //var dtAllPointActivity = lookInfo["dataSource"] as DataTable;
            //foreach (DataRow item in dt.Rows)
            //{
            //    var dr = dtAllPointActivity.Select("point='" + item["point"] + "'");
            //    if (dr.Length == 0)
            //    {
            //        item["strDevName"] = "（测点已删除）";
            //        item["strWzName"] = "（测点已删除）";
            //    }
            //    else
            //    {
            //        item["strDevName"] = dr[0]["strDevName"];
            //        item["strWzName"] = dr[0]["strWzName"];
            //    }
            //}
            SelectedPointPropertyAdd(dt);

            gridControl1.DataSource = dt;
        }

        /// <summary>已选测点属性增加
        /// 
        /// </summary>
        /// <param name="selectedPoint"></param>
        private void SelectedPointPropertyAdd(DataTable selectedPoint)
        {
            bool blnNewData = false;
            Hashtable lookInfo = LookUpUtil.GetlookInfo("AllPointActivity", ref blnNewData);
            var dtAllPointActivity = lookInfo["dataSource"] as DataTable;
            foreach (DataRow item in selectedPoint.Rows)
            {
                var dr = dtAllPointActivity.Select("point='" + item["point"] + "'");
                if (dr.Length == 0)
                {
                    item["strDevName"] = "（测点已删除）";
                    item["strWzName"] = "（测点已删除）";
                }
                else
                {
                    item["strDevName"] = dr[0]["strDevName"];
                    item["strWzName"] = dr[0]["strWzName"];
                }
            }
        }

        /// <summary>
        ///     根据元数据配置的参照编码,弹出参照进行选择
        /// </summary>
        /// <para
        ///     m name="sender"></param>
        ///     <param name="e"></param>
        private void tlbSelectDataSource_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                // 20170919
                if (listBoxControlArrangeTime.SelectedItem == null)
                {
                    MessageShowUtil.ShowInfo("请先增加编排。");
                    return;
                }

                var strSelectdValue = GetGridIDs();
                var multiDlgRef = new frmGenericRef();

                // 20170828
                //multiDlgRef.StrFkCode = strFKCode;
                //multiDlgRef.StrFkCode = "AllPointActivity";
                multiDlgRef.StrFkCode = "AllPointMultisystem";

                multiDlgRef.BlnSelectStr = true;
                multiDlgRef.BlnMulti = true;
                multiDlgRef.StrSelectValue = strSelectdValue;
                multiDlgRef.BlnSort = true;
                multiDlgRef.ShowDialog();
                if (multiDlgRef.BlnOk)
                {
                    var strValueMember = lookInfo["StrValueMember"] as string;
                    var strSelectDisplay = multiDlgRef.StrSelectDisplay;
                    var strSelectValue = multiDlgRef.StrSelectValue;
                    var dicSelectValue = multiDlgRef.StrSelectIDs;
                    if (strSelectValue.Length == 0)
                        dt.Rows.Clear();

                    foreach (var id in dicSelectValue.Keys)
                    {
                        //看返回回来的数据在gridview里面是否已存在,若不存在才新增行                    
                        var rows = dt.Select(strValueMember + "='" + id + "'");
                        if ((rows == null) || (rows.Length == 0))
                        {
                            var dtsource = lookInfo["dataSource"] as DataTable;
                            var rowsdatasource = dtsource.Select(strValueMember + "='" + id + "'");
                            if (rowsdatasource.Length > 0)
                            {
                                var row = dt.NewRow();
                                foreach (GridColumn column in gridView1.Columns)
                                    row[column.FieldName] = rowsdatasource[0][column.FieldName];
                                dt.Rows.Add(row);
                            }
                        }
                    }

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        //看当前gridview的数据在返回回来的数据中是否已不存在，不存在就要删除，用于取消选择这种情况
                        var id = dt.Rows[i][strValueMember];
                        if (!strSelectValue.Contains("'" + id + "'"))
                        {
                            dt.Rows.RemoveAt(i);
                            i--;
                        }
                    }

                    _strKeyValue = multiDlgRef.StrSelectValue;
                    _strDisplayValue = multiDlgRef.StrSelectDisplay;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private string GetGridIDs()
        {
            var strValueMember = lookInfo["StrValueMember"] as string;
            var ids = "";
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            for (var i = 0; i < gridView1.RowCount; i++)
                ids += "'" + TypeUtil.ToString(gridView1.GetRowCellValue(i, strValueMember)) + "',";
            if (ids.Length > 0)
                ids = ids.Substring(0, ids.Length - 1);
            return ids;
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            var rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && (rowHandle > 0))
                e.Info.DisplayText = rowHandle.ToString();
        }

        /// <summary>
        ///     返回并回写固定条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbOK_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 20170919
            if (listBoxControlArrangeTime.SelectedItem == null)
            {
                MessageShowUtil.ShowInfo("请先增加编排。");
                return;
            }

            var blnNewData = false;
            var lookInfo = LookUpUtil.GetlookInfo(strFKCode, ref blnNewData);
            var strValueMember = lookInfo["StrValueMember"] as string;
            var strDisplayFileName = lookInfo["StrDsiplayMember"] as string;

            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();
            if (gridView1.RowCount != 0)
            {
                strCondition = "等于&&$";
                strConditionCHS = "等于&&$";
                for (var i = 0; i < gridView1.RowCount; i++)
                {
                    strCondition += "'" + TypeUtil.ToString(gridView1.GetRowCellValue(i, strValueMember)) + "',";
                    strConditionCHS += "'" + TypeUtil.ToString(gridView1.GetRowCellValue(i, strDisplayFileName)) + "',";
                }

                strCondition = strCondition.Substring(0, strCondition.Length - 1);
                strConditionCHS = strConditionCHS.Substring(0, strConditionCHS.Length - 1);
            }
            else
            {
                strCondition = "";
                strConditionCHS = "";
            }
            try
            {
                if (strCondition == "")
                {
                    MessageShowUtil.ShowInfo("请至少输入一条记录!");
                    return;
                }

                // 20170829
                //var strDate = dateEdit1.DateTime.ToString("yyyyMMdd");
                var strDate = (listBoxControlArrangeTime.SelectedItem as ListdatalayountInfo).StrDate;

                ListdatalayountInfo listdatalayoutdto = null;
                IList<ListdatalayountInfo> lists = Model.GetListDataLayoutDataA(ListDataID, strDate);
                if (lists.Count > 0)
                {
                    listdatalayoutdto = lists[0];
                    listdatalayoutdto.InfoState = InfoState.Modified;
                }
                else
                {
                    listdatalayoutdto = new ListdatalayountInfo();
                    listdatalayoutdto.InfoState = InfoState.AddNew;

                }

                listdatalayoutdto.StrCondition = strCondition;
                listdatalayoutdto.StrConditionCHS = strConditionCHS;
                listdatalayoutdto.ListDataID = ListDataID;
                listdatalayoutdto.StrDate = strDate;
                listdatalayoutdto.StrFileName = strFileName;
                var strfilename = strFileName;
                var index = strfilename.LastIndexOf("_");
                if (index > -1)
                    strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");
                var strwhere = BulidConditionUtil.GetRefCondition(strfilename, strCondition, "varchar");
                listdatalayoutdto.StrConTextCondition = strwhere;
                if (strCondition == "")
                    listdatalayoutdto.InfoState = InfoState.Delete;
                Model.SaveVO(listdatalayoutdto);


                blnOK = true;
                MessageShowUtil.ShowInfo("保存成功!");
                //Close();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            downHitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y)); //鼠标左键按下去时在GridView中的坐标
        }

        private void gridView1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left) return; //不是左键则无效
                if ((downHitInfo == null) || (downHitInfo.RowHandle < 0)) return; //判断鼠标的位置是否有效

                rows = gridView1.GetSelectedRows(); //获取所选行的index
                startRow = rows.Length == 0 ? -1 : rows[0];
                var dt = this.dt.Clone();

                foreach (var r in rows) // 根据所选行的index进行取值，去除所选的行数据，可能是选取的多行
                {
                    var dataSourcerows = gridView1.GetDataSourceRowIndex(r); //获取行数据
                    dt.ImportRow(this.dt.Rows[dataSourcerows]); //保存所选取的行数据
                }
                gridControl1.DoDragDrop(dt, DragDropEffects.Move); //开始拖放操作，将拖拽的数据存储起来
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowMsg(ex.Message);
            }
        }

        //拖拽过程事件
        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        //拖拽完成后事件
        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            var gridviewPoint = PointToScreen(gridControl1.Location); //获取鼠标在屏幕上的位置。
            upHitInfo = gridView1.CalcHitInfo(new Point(e.X - gridviewPoint.X, e.Y - gridviewPoint.Y));
            //鼠标左键弹起来时在GridView中的坐标（屏幕位置减去 gridView 开始位置）
            if ((upHitInfo == null) || (upHitInfo.RowHandle < 0))
                if (upHitInfo.RowHandle == -2147483648)
                    upHitInfo.RowHandle = gridView1.RowCount - 1;
                else
                    return;
            else
                upHitInfo.RowHandle -= 1;

            var endRow = gridView1.GetDataSourceRowIndex(gridView1.GetDataSourceRowIndex(upHitInfo.RowHandle));
            //获取拖拽的目标行index

            var dt = e.Data.GetData(typeof(DataTable)) as DataTable;
            //获取要移动的数据，从拖拽保存的地方：（gridControl1.DoDragDrop(dt, DragDropEffects.Move); ）

            if ((dt != null) && (dt.Rows.Count > 0)) //拖拽的数据为空
            {
                int a;
                var xs = this.dt.Rows[endRow]; //获取拖拽的目标行，准备进行移植
                if (!rows.Contains(endRow)) //如果多选的话，确保不能拖拽到这几个里
                {
                    gridView1.ClearSelection(); //从GirdView中删除所拖拽的数据
                    var moveValue = 0;
                    foreach (var i in rows)
                    {
                        this.dt.Rows.Remove(this.dt.Rows[i - moveValue]); //从GirdView的数据源中删除所拖拽的数据
                        moveValue++;
                    }

                    if (startRow > endRow)
                        a = this.dt.Rows.IndexOf(xs); //若果往上托，则加在鼠标到达行的上面
                    else
                        a = this.dt.Rows.IndexOf(xs) + 1; //如果往下拖，则加在鼠标到达行的下面
                    var j = 0;
                    DataRow drTemp;
                    foreach (DataRow dr in dt.Rows)
                    {
                        drTemp = this.dt.NewRow();
                        foreach (DataColumn dc in dr.Table.Columns)
                            drTemp[dc.ColumnName] = dr[dc.ColumnName];
                        this.dt.Rows.InsertAt(drTemp, a + j); //将拖拽的数据再次添加进来
                        gridView1.SelectRow(a + j);
                        j++;
                    }
                    gridView1.FocusedRowHandle = a;
                }
                gridControl1.DataSource = this.dt; //重新绑定
                gridView1.RefreshData();
            }
        }

        /// <summary>
        ///     控制新增行可以编辑，其他行不能编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gridView1.FocusedRowHandle >= 0)
                e.Cancel = true;
        }

        private void tlbCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     校验在新增行输入的数据在基础档案中是否存在
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            try
            {
                var strValueMember = lookInfo["StrValueMember"] as string;
                var strValue = TypeUtil.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, strValueMember));

                var rowgridivew = dt.Select(strValueMember + "='" + strValue + "'");
                if (rowgridivew.Length > 0)
                {
                    e.ErrorText = "已录入过此数据，不能重复录入！";
                    e.Valid = false;
                }
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        ///     录入的主键发生变化后，给其他列赋值,如录入测点后自动带出设备类型和安装位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            gridView1.CellValueChanged -= gridView1_CellValueChanged;
            var strValueMember = lookInfo["StrValueMember"] as string;
            var strValue = TypeUtil.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, strValueMember));

            if (TypeUtil.ToString(strValue).Length == 0)
            {
                //2016-10-20 ，现支持测点编排的时候以ID做为strvaluemeber，而录入的时候又是录入的是strdisplaymenber，所以通过StrDsiplayMember来得到其他值
                strValueMember = lookInfo["StrDsiplayMember"] as string;
                strValue = TypeUtil.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, strValueMember));
            }


            var dtsource = lookInfo["dataSource"] as DataTable;
            var rows = dtsource.Select(strValueMember + "='" + strValue + "'");
            if (rows.Length > 0)
                foreach (GridColumn column in gridView1.Columns)
                    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, column, rows[0][column.FieldName]);
            else
            {
                MessageShowUtil.ShowInfo("录入的数据在系统中不存在，请重新录入！");
            }
            gridView1.CellValueChanged += gridView1_CellValueChanged;
        }

        private void repBtnDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void bltDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                gridView1.CloseEditor();
                gridView1.UpdateCurrentRow();
                var rowhandel = gridView1.FocusedRowHandle;
                gridView1.DeleteRow(rowhandel);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowMsg(ex.ToString());
            }
        }

        private void btnMoveFirst_Click(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (gridView1.IsFirstRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是首行！");
                    return;
                }

                var focusedRowHandle = gridView1.FocusedRowHandle;
                var focuseDr = gridView1.GetFocusedDataRow();
                var copyDr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];
                dt.Rows.Remove(focuseDr);
                dt.Rows.InsertAt(copyDr, 0);
                gridView1.FocusedRowHandle = 0;
                gridView1.SelectRow(gridView1.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void btnMovePrev_Click(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (gridView1.IsFirstRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是首行！");
                    return;
                }

                var focusedRowHandle = gridView1.FocusedRowHandle;
                var focuseDr = gridView1.GetFocusedDataRow();
                var copyDr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];

                var curRowIndex = dt.Rows.IndexOf(focuseDr); //处理隐藏行记录             
                for (var insertRow = curRowIndex - 1; insertRow >= 0; insertRow--)
                {
                    dt.Rows.Remove(focuseDr);
                    dt.Rows.InsertAt(copyDr, insertRow);
                    break;
                }
                gridView1.FocusedRowHandle = focusedRowHandle - 1;
                gridView1.SelectRow(gridView1.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void btnMoveNext_Click(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (gridView1.IsLastRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是末行！");
                    return;
                }

                var focusedRowHandle = gridView1.FocusedRowHandle;
                var focuseDr = gridView1.GetFocusedDataRow();
                var copyDr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];

                var curRowIndex = dt.Rows.IndexOf(focuseDr); //处理隐藏行记录               
                for (var insertRow = curRowIndex + 1; insertRow < dt.Rows.Count; insertRow++)
                {
                    dt.Rows.Remove(focuseDr);
                    dt.Rows.InsertAt(copyDr, insertRow);
                    break;
                }
                gridView1.FocusedRowHandle = focusedRowHandle + 1;
                gridView1.SelectRow(gridView1.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void btnMoveLast_Click(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (gridView1.IsLastRow)
                {
                    MessageShowUtil.ShowInfo("当前行已是末行！");
                    return;
                }

                var focusedRowHandle = gridView1.FocusedRowHandle;
                var focuseDr = gridView1.GetFocusedDataRow();
                var copyDr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns)
                    copyDr[dc.ColumnName] = focuseDr[dc.ColumnName];
                dt.Rows.Remove(focuseDr);
                dt.Rows.Add(copyDr);
                gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                gridView1.SelectRow(gridView1.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        /// <summary>
        /// 编排时间选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControlArrangeTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (listBoxControlArrangeTime.SelectedItem == null)
                {
                    return;
                }
                string sTime = (listBoxControlArrangeTime.SelectedItem as ListdatalayountInfo).StrDate;
                //DateTime dtTime = new DateTime(Convert.ToInt32(sTime.Substring(0, 4)), Convert.ToInt32(sTime.Substring(4, 2)), Convert.ToInt32(sTime.Substring(6, 2)));
                BindingPointGrid(ListDataID, sTime);
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowErrow(exception);
            }

        }

        /// <summary>
        /// 新增编排
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addArrange_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                //var frmDataSortSetEditWin = new frmDataSortSetEdit(ListDataID, true, "");
                //frmDataSortSetEditWin.ShowDialog();
                //if (frmDataSortSetEditWin.IsOk)
                //{
                //    string selPoint = frmDataSortSetEditWin.SelPoint;
                //    strCondition = "等于&&$" + selPoint;
                //    strConditionCHS = "等于&&$" + selPoint;

                //    var strDate = frmDataSortSetEditWin.SelTime;
                //    ListdatalayountInfo listdatalayoutdto = new ListdatalayountInfo();
                //    listdatalayoutdto.InfoState = InfoState.AddNew;
                //    listdatalayoutdto.StrCondition = strCondition;
                //    listdatalayoutdto.StrConditionCHS = strConditionCHS;
                //    listdatalayoutdto.ListDataID = ListDataID;
                //    listdatalayoutdto.StrDate = strDate.ToString("yyyyMMdd");
                //    listdatalayoutdto.StrFileName = strFileName;
                //    var strfilename = strFileName;
                //    var index = strfilename.LastIndexOf("_");
                //    if (index > -1)
                //        strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");
                //    var strwhere = BulidConditionUtil.GetRefCondition(strfilename, strCondition, "varchar");
                //    listdatalayoutdto.StrConTextCondition = strwhere;
                //    Model.SaveVO(listdatalayoutdto);
                //    BindingArrangeTimeList();       //刷新列表
                //    MessageShowUtil.ShowInfo("测点编排成功!");
                //}

                //IList<ListdatalayountInfo> lists = new ListExModel().GetListDataLayoutDataA(Convert.ToInt32(ListDataID), dateEditNewArrange.DateTime.ToString("yyyyMMdd"));
                IList<ListdatalayountInfo> lists = new ListExModel().GetListDataLayoutDataA(Convert.ToInt32(ListDataID), NewArrangeName.Text);

                if (lists.Count > 0)
                {
                    MessageShowUtil.ShowInfo("该编排名称已存在。");
                    return;
                }

                StringBuilder sbPoint = new StringBuilder();
                var selIndex = gridViewSubstationPoint.GetSelectedRows();
                foreach (var item in selIndex)
                {
                    sbPoint.Append("'" + _waitintPoint.Rows[item]["Point"] + "',");
                }

                if (sbPoint.Length == 0)
                {
                    MessageShowUtil.ShowInfo("请先选择测点。");
                    return;
                }

                var res = XtraMessageBox.Show("是否新增编排？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    return;
                }

                string selPoint = sbPoint.ToString().Substring(0, sbPoint.Length - 1);
                strCondition = "等于&&$" + selPoint;
                strConditionCHS = "等于&&$" + selPoint;

                //var strDate = dateEditNewArrange.DateTime;
                ListdatalayountInfo listdatalayoutdto = new ListdatalayountInfo();
                listdatalayoutdto.InfoState = InfoState.AddNew;
                listdatalayoutdto.StrCondition = strCondition;
                listdatalayoutdto.StrConditionCHS = strConditionCHS;
                listdatalayoutdto.ListDataID = ListDataID;
                //listdatalayoutdto.StrDate = strDate.ToString("yyyyMMdd");
                listdatalayoutdto.StrDate = NewArrangeName.Text;
                listdatalayoutdto.StrFileName = strFileName;
                var strfilename = strFileName;
                var index = strfilename.LastIndexOf("_");
                if (index > -1)
                    strfilename = "" + strfilename.Remove(index, 1).Insert(index, ".");
                var strwhere = BulidConditionUtil.GetRefCondition(strfilename, strCondition, "varchar");
                listdatalayoutdto.StrConTextCondition = strwhere;
                Model.SaveVO(listdatalayoutdto);
                BindingArrangeTimeList();       //刷新列表
                MessageShowUtil.ShowInfo("新增测点编排成功!");
            }
            catch (Exception exc)
            {
                MessageShowUtil.ShowErrow(exc);
            }
        }

        private void deleteArrange_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                if (listBoxControlArrangeTime.SelectedItem == null)
                {
                    MessageShowUtil.ShowInfo("请选择编排时间。");
                    return;
                }

                var res = XtraMessageBox.Show("是否删除编排？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res != DialogResult.Yes)
                {
                    return;
                }

                string sTime = (listBoxControlArrangeTime.SelectedItem as ListdatalayountInfo).StrDate;
                Model.DeleteListDataLay(sTime, ListDataID);
                BindingArrangeTimeList();       //刷新列表
                MessageShowUtil.ShowInfo("删除成功!");
            }
            catch (Exception exc)
            {
                MessageShowUtil.ShowErrow(exc);
            }
        }

        private void MoveSelect_Click(object sender, EventArgs e)
        {
            try
            {
                var focusedRow = gridView1.GetFocusedDataRow();
                _selectMoveIndex = dt.Rows.IndexOf(focusedRow).ToString();

            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowErrow(exception);
            }
        }

        private void MoveToHere_Click(object sender, EventArgs e)
        {
            try
            {
                var iSelIndex = Convert.ToInt32(_selectMoveIndex);
                if (string.IsNullOrEmpty(_selectMoveIndex) || iSelIndex + 1 > dt.Rows.Count)
                {
                    MessageShowUtil.ShowInfo("请先选择需要移动的项！");
                    return;
                }

                var focusedRow = gridView1.GetFocusedDataRow();
                var moveToIndex = dt.Rows.IndexOf(focusedRow);

                if (moveToIndex == iSelIndex)
                {
                    return;
                }

                var selPoint = dt.Rows[iSelIndex]["point"].ToString();
                var selPointDev = dt.Rows[iSelIndex]["strDevName"].ToString();
                var selPointWz = dt.Rows[iSelIndex]["strWzName"].ToString();
                dt.Rows.RemoveAt(iSelIndex);
                var newRow = dt.NewRow();
                newRow["point"] = selPoint;
                newRow["strDevName"] = selPointDev;
                newRow["strWzName"] = selPointWz;
                dt.Rows.InsertAt(newRow, moveToIndex);

                if (iSelIndex > moveToIndex)
                {
                    gridView1.UnselectRow(moveToIndex + 1);
                }

                if (iSelIndex < moveToIndex)
                {
                    gridView1.UnselectRow(moveToIndex - 1);
                }
                gridView1.FocusedRowHandle = moveToIndex;
                gridView1.SelectRow(gridView1.FocusedRowHandle);

                // 20180112
                _selectMoveIndex = null;
            }
            catch (Exception exc)
            {
                MessageShowUtil.ShowErrow(exc);
            }
        }

        private void lookUpEditSubstation_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var selSubstationNum = lookUpEditSubstation.EditValue;
                var type = lookUpEditFiltrate.Text;

                List<Jc_DefInfo> lisPoint;
                if (type == "按性质")
                {
                    lisPoint = PointRequest.GetPointByEquipmentPropertyId(selSubstationNum.ToString());
                }
                else if (type == "按种类")
                {
                    lisPoint = PointRequest.GetPointByEquipmentCategoryId(selSubstationNum.ToString());
                }
                else
                {
                    lisPoint = PointRequest.GetPointBySubstationNum(selSubstationNum.ToString());
                }

                _waitintPoint.Clear();
                foreach (var item in lisPoint)
                {
                    var newRow = _waitintPoint.NewRow();
                    newRow["PointID"] = item.PointID;
                    newRow["Point"] = item.Point;
                    newRow["Wz"] = item.Wz;
                    newRow["DevName"] = item.DevName;
                    _waitintPoint.Rows.Add(newRow);
                }
                gridControlSubstationPoint.DataSource = _waitintPoint;

            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowErrow(exception);
            }
        }

        // 20180117
        private void PointInsert_Click(object sender, EventArgs e)
        {
            InsertPoint(1);
        }

        private void AllPointInsert_Click(object sender, EventArgs e)
        {
            InsertPoint(2);
        }

        private void InsertPoint(int type)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                // 20170919
                if (listBoxControlArrangeTime.SelectedItem == null)
                {
                    MessageShowUtil.ShowInfo("请先增加编排。");
                    return;
                }

                // 20180116
                //检查是否已存在该测点
                //var row3 = dt.Select("point='" + textEditPoint.Text + "'");
                //var row3 = dt.Select("point='" + comboBoxPoint.Text + "'");
                bool ifExist = false;
                List<DataRow> selRows = new List<DataRow>();
                if (type == 1)
                {
                    var selRowIndex = gridViewSubstationPoint.GetSelectedRows();
                    if (selRowIndex == null || selRowIndex.Length == 0)
                    {
                        MessageShowUtil.ShowInfo("请选择需要添加的项！");
                        return;
                    }

                    foreach (var item in selRowIndex)
                    {
                        var row = _waitintPoint.Rows[item];
                        selRows.Add(row);

                        var rows = dt.Select("point='" + row["Point"] + "'");
                        if (rows.Length != 0)
                        {
                            ifExist = true;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (DataRow item in _waitintPoint.Rows)
                    {
                        selRows.Add(item);
                        var rows = dt.Select("point='" + item["Point"] + "'");
                        if (rows.Length != 0)
                        {
                            ifExist = true;
                            break;
                        }
                    }
                }

                if (ifExist)
                {
                    MessageShowUtil.ShowInfo("请勿添加相同测点。");
                    return;
                }

                var selectedRow = gridView1.GetFocusedDataRow();
                int selectedRowIndex = 0;
                if (selectedRow != null)
                {
                    selectedRowIndex = dt.Rows.IndexOf(selectedRow);
                }

                for (int i = selRows.Count - 1; i >= 0; i--)
                {
                    var row = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                        row[j] = selRows[i][j];

                    dt.Rows.InsertAt(row, selectedRowIndex);
                    gridView1.UnselectRow(gridView1.FocusedRowHandle);
                    gridView1.FocusedRowHandle -= 1;
                    gridView1.SelectRow(gridView1.FocusedRowHandle);
                }
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowErrow(exception);
            }
        }

        private void PointDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 20180112
                _selectMoveIndex = null;

                gridView1.CloseEditor();
                gridView1.UpdateCurrentRow();
                //var rowhandel = gridView1.FocusedRowHandle;
                //gridView1.DeleteRow(rowhandel);

                var selIndex = gridView1.GetSelectedRows();
                for (int i = selIndex.Length - 1; i >= 0; i--)
                {
                    dt.Rows.RemoveAt(selIndex[i]);
                }
                gridView1.RefreshData();
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowMsg(ex.ToString());
            }
        }

        private void AllPointDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _selectMoveIndex = null;

                dt.Clear();
                gridView1.RefreshData();
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowMsg(exception.ToString());
            }
        }

        private void lookUpEditFiltrate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //var type = comboBoxProperty.Text;
                var type = lookUpEditFiltrate.Text;
                if (type == "按性质")
                {
                    layoutControlItemSubstation.Text = "性质：";

                    lookUpEditSubstation.EditValueChanged -= lookUpEditSubstation_EditValueChanged;
                    lookUpEditSubstation.Properties.Columns.Clear();
                    lookUpEditSubstation.Properties.Columns.AddRange(new[]
                    {
                        new LookUpColumnInfo("LngEnumValue", "LngEnumValue", 50, DevExpress.Utils.FormatType.None, "",
                            false, DevExpress.Utils.HorzAlignment.Default),
                        new LookUpColumnInfo("StrEnumDisplay", 100, "性质")
                    });
                    lookUpEditSubstation.Properties.ValueMember = "LngEnumValue";
                    lookUpEditSubstation.Properties.DisplayMember = "StrEnumDisplay";
                    lookUpEditSubstation.EditValueChanged += lookUpEditSubstation_EditValueChanged;

                    var equPro = EnumRequest.GetEquipmentProperty();
                    lookUpEditSubstation.Properties.DataSource = equPro;
                }
                else if(type == "按种类")
                {
                    layoutControlItemSubstation.Text = "种类：";

                    lookUpEditSubstation.EditValueChanged -= lookUpEditSubstation_EditValueChanged;
                    lookUpEditSubstation.Properties.Columns.Clear();
                    lookUpEditSubstation.Properties.Columns.AddRange(new[]
                    {
                        new LookUpColumnInfo("LngEnumValue", "LngEnumValue", 50, DevExpress.Utils.FormatType.None, "",
                            false, DevExpress.Utils.HorzAlignment.Default),
                        new LookUpColumnInfo("StrEnumDisplay", 100, "种类")
                    });
                    lookUpEditSubstation.Properties.ValueMember = "LngEnumValue";
                    lookUpEditSubstation.Properties.DisplayMember = "StrEnumDisplay";
                    lookUpEditSubstation.EditValueChanged += lookUpEditSubstation_EditValueChanged;

                    var equCat = EnumRequest.GetEquipmentCategory();
                    lookUpEditSubstation.Properties.DataSource = equCat;
                }
                else
                {
                    layoutControlItemSubstation.Text = "分站：";

                    lookUpEditSubstation.EditValueChanged -= lookUpEditSubstation_EditValueChanged;
                    lookUpEditSubstation.Properties.Columns.Clear();
                    lookUpEditSubstation.Properties.Columns.AddRange(new[]
                    {
                        new LookUpColumnInfo("Fzh", 50, "分站号"),
                        new LookUpColumnInfo("Wz", 100, "安装位置")
                    });
                    lookUpEditSubstation.Properties.ValueMember = "Fzh";
                    lookUpEditSubstation.Properties.DisplayMember = "Wz";
                    lookUpEditSubstation.EditValueChanged += lookUpEditSubstation_EditValueChanged;

                    var substation = PointRequest.GetAllSubstationPoint();
                    lookUpEditSubstation.Properties.DataSource = substation;

                }
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowMsg(exception.ToString());
            }
        }

        private void gridControlSubstationPoint_DoubleClick(object sender, EventArgs e)
        {
            InsertPoint(1);
        }

        private void simpleButtonCopyAll_Click(object sender, EventArgs e)
        {
            try
            {
                _copyPoint = new List<string>();
                foreach (DataRow item in dt.Rows)
                {
                    _copyPoint.Add(item["point"].ToString());
                }
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowMsg(exception.ToString());
            }
        }

        private void simpleButtonPasteAll_Click(object sender, EventArgs e)
        {
            try
            {
                _selectMoveIndex = null;

                // 20170919
                if (listBoxControlArrangeTime.SelectedItem == null)
                {
                    MessageShowUtil.ShowInfo("请先增加编排。");
                    return;
                }

                if (_copyPoint == null)
                {
                    return;
                }

                dt.Rows.Clear();
                foreach (var item in _copyPoint)
                {
                    var newRow = dt.NewRow();
                    newRow["point"] = item;
                    dt.Rows.Add(newRow);
                }

                SelectedPointPropertyAdd(dt);
            }
            catch (Exception exception)
            {
                MessageShowUtil.ShowMsg(exception.ToString());
            }
        }
    }
}