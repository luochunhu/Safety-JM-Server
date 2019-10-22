using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System.Data;
using DevExpress.XtraGrid.Columns;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports.Controls
{
    /// <summary>
    /// 通能参照类

    /// </summary>
    public class LookUpUtil
    {

        private static ListExModel listexModel = new ListExModel();

        /// <summary>
        /// 创建窗体上用的通能参照
        /// 1.获取参照的数据信息

        /// 2.创建参照的各列

        /// 3.设置参照的值字段

        /// 4.设置参照的显示字段

        /// 5.处理数据源(如有必要,要在数据源的第一行添加一行空数据,以便取消选择)
        /// 6.设置参照的数据源
        /// </summary>
        /// <param name="lookUpCode">参照编码</param>
        /// <param name="lookUp">参照对象</param>
        /// <param name="firstRowIsNull">是否插入一行空数据</param>
        public static void CreateLookUp(string lookUpCode, LookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }
            int width = 0;
            string strColumns = lookInfo["StrColumns"] as string;
            strColumns = strColumns.Trim();
            string[] fieldName = strColumns.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            int filedCount = fieldName.Length;
            int totalWidth = 0;
            for (int i = 0; i < filedCount; i++)
            {
                string[] subFieldName = fieldName[i].Split(',');
                width = 0;
                if (subFieldName.Length >= 3)
                {
                    width = TypeUtil.ToInt(subFieldName[2]);
                }
                if (width > 0)
                {
                    totalWidth += width;
                    lookUp.Properties.Columns.AddRange(new LookUpColumnInfo[] {
                                                       new LookUpColumnInfo(subFieldName[0] ,subFieldName[1], width, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None) });
                }
            }

            lookUp.Properties.ValueMember = lookInfo["StrValueMember"] as string;
            lookUp.Properties.DisplayMember = lookInfo["StrDsiplayMember"] as string;
            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.Properties.ValueMember] = 0;
                dr[lookUp.Properties.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
            if (totalWidth > 600)
                lookUp.Properties.PopupFormWidth = 600;
            else
                lookUp.Properties.PopupFormWidth = totalWidth;
            lookUp.Properties.DataSource = dt;
            if (TypeUtil.ToBool(lookInfo["BlnDefaultSelection"]))
            {
                if (dt != null && firstRowIsNull)
                    lookUp.EditValue = dt.Rows[1][lookUp.Properties.ValueMember];
                else if (dt != null)
                    lookUp.EditValue = dt.Rows[0][lookUp.Properties.ValueMember];
            }
        }

        /// <summary>
        /// 创建Grid通能参照
        /// 1.获取参照的数据信息

        /// 2.创建参照的各列

        /// 3.设置参照的值字段

        /// 4.设置参照的显示字段

        /// 5.处理数据源(如有必要,要在数据源的第一行添加一行空数据,以便取消选择)
        /// 6.设置参照的数据源
        /// </summary>
        /// <param name="lookUpCode">参照编码</param>
        /// <param name="lookUp">参照对象</param>
        /// <param name="firstRowIsNull">是否插入一行空数据</param>
        public static void CreateLookUp(string lookUpCode, RepositoryItemLookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }
            int width = 0;
            string strColumns = lookInfo["StrColumns"] as string;
            strColumns = strColumns.Trim();
            string[] fieldName = strColumns.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            int filedCount = fieldName.Length;
            int totalWidth = 0;
            for (int i = 0; i < filedCount; i++)
            {
                string[] subFieldName = fieldName[i].Split(',');
                width = 0;
                if (subFieldName.Length >= 3)
                {
                    width = TypeUtil.ToInt(subFieldName[2]);
                }
                if (width > 0)
                {
                    totalWidth += width;
                    lookUp.Columns.AddRange(new LookUpColumnInfo[] {
                                           new LookUpColumnInfo(subFieldName[0] ,subFieldName[1], width, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None) });
                }
            }

            lookUp.ValueMember = lookInfo["StrValueMember"] as string;
            lookUp.DisplayMember = lookInfo["StrDsiplayMember"] as string;
            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.ValueMember] = 0;
                dr[lookUp.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
            if (totalWidth > 600)
                lookUp.PopupFormWidth = 600;
            else
                lookUp.PopupFormWidth = totalWidth;
            lookUp.DataSource = dt;
        }

        /// <summary>
        /// 创建窗体上用的通能GridLookUp参照
        /// 1.获取参照的数据信息

        /// 2.创建参照的各列

        /// 3.设置参照的值字段

        /// 4.设置参照的显示字段

        /// 5.处理数据源(如有必要,要在数据源的第一行添加一行空数据,以便取消选择)
        /// 6.设置参照的数据源
        /// </summary>
        /// <param name="lookUpCode">参照编码</param>
        /// <param name="lookUp">参照对象</param>
        /// <param name="firstRowIsNull">是否插入一行空数据</param>
        /// <param name="isNeedFilter">是否设置过滤 blnEnable 为 False的列数据</param>
        public static void CreateGridLookUp(string lookUpCode, GridLookUpEdit lookUp, bool firstRowIsNull, bool isNeedFilter)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }
            int width = 0;
            string strColumns = lookInfo["StrColumns"] as string;
            strColumns = strColumns.Trim();
            string[] fieldName = strColumns.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            int filedCount = fieldName.Length;
            int totalWidth = 0;
            int visibleIndex = 0;
            GridColumn gc;
            for (int i = 0; i < filedCount; i++)
            {
                string[] subFieldName = fieldName[i].Split(',');
                width = 0;
                if (subFieldName.Length >= 3)
                {
                    width = TypeUtil.ToInt(subFieldName[2]);
                }

                gc = new GridColumn();
                gc.Name = "col" + subFieldName[0];
                gc.FieldName = subFieldName[0];
                gc.Caption = subFieldName[1];

                if (width > 0)
                {
                    gc.Visible = true;
                    gc.VisibleIndex = visibleIndex++;
                    gc.Width = width;

                    totalWidth += gc.Width;
                }
                else
                {
                    gc.VisibleIndex = -1;
                    gc.Visible = false;
                }

                lookUp.Properties.View.Columns.Add(gc);
            }
            lookUp.Properties.ValueMember = lookInfo["StrValueMember"] as string;
            lookUp.Properties.DisplayMember = lookInfo["StrDsiplayMember"] as string;
            DataTable dt = lookInfo["dataSource"] as DataTable;
            if (totalWidth > 600)
            {
                lookUp.Properties.PopupFormWidth = 600;
            }

            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.Properties.ValueMember] = 0;
                dr[lookUp.Properties.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
            lookUp.Properties.DataSource = dt;
            if (isNeedFilter)
            {
                gc = new GridColumn();
                gc.Name = "colblnEnable";
                gc.FieldName = "blnEnable";
                gc.Caption = "colblnEnable";
                lookUp.Properties.View.Columns.Add(gc);
                lookUp.Properties.View.Columns["blnEnable"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo(true);
                lookUp.Properties.View.OptionsBehavior.Editable = true;
                lookUp.Properties.View.OptionsSelection.EnableAppearanceFocusedCell = false;
                lookUp.Properties.View.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
                lookUp.Properties.ImmediatePopup = true;
               
            }
            if (TypeUtil.ToBool(lookInfo["BlnDefaultSelection"]))
            {
                if (dt != null && firstRowIsNull)
                    lookUp.EditValue = dt.Rows[1][lookUp.Properties.ValueMember];
                else if (dt != null)
                    lookUp.EditValue = dt.Rows[0][lookUp.Properties.ValueMember];
            }

            SetGridLookUpColumnFormat(lookUp.Properties.View);

            if (lookUp.Properties.Buttons.Count > 0)
            {
                lookUp.Properties.Buttons[0].Visible = true;//显示出第一个按钮

            }
        }

        /// <summary>
        /// 创建Grid通能GridLookUp参照
        /// 1.获取参照的数据信息

        /// 2.创建参照的各列

        /// 3.设置参照的值字段

        /// 4.设置参照的显示字段

        /// 5.处理数据源(如有必要,要在数据源的第一行添加一行空数据,以便取消选择)
        /// 6.设置参照的数据源
        /// </summary>
        /// <param name="lookUpCode">参照编码</param>
        /// <param name="lookUp">参照对象</param>
        /// <param name="firstRowIsNull">是否插入一行空数据</param>
        /// <param name="isNeedFilter">是否设置过滤 blnEnable 为 False的列数据</param>
        public static void CreateGridLookUp(string lookUpCode, RepositoryItemGridLookUpEdit lookUp, bool firstRowIsNull, bool isNeedFilter)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }
            int width = 0;
            string strColumns = lookInfo["StrColumns"] as string;
            strColumns = strColumns.Trim();
            string[] fieldName = strColumns.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            int filedCount = fieldName.Length;
            int totalWidth = 0;
            int visibleIndex = 0;
            GridColumn gc;
            for (int i = 0; i < filedCount; i++)
            {
                string[] subFieldName = fieldName[i].Split(',');
                width = 0;
                if (subFieldName.Length >= 3)
                {
                    width = TypeUtil.ToInt(subFieldName[2]);
                }

                gc = new GridColumn();
                gc.Name = "col" + subFieldName[0];
                gc.FieldName = subFieldName[0];
                gc.Caption = subFieldName[1];

                if (width > 0)
                {
                    gc.Visible = true;
                    gc.VisibleIndex = visibleIndex++;
                    gc.Width = width;

                    totalWidth += gc.Width;
                }
                else
                {
                    gc.VisibleIndex = -1;
                    gc.Visible = false;
                }

                lookUp.View.Columns.Add(gc);
            }

            lookUp.ValueMember = lookInfo["StrValueMember"] as string;
            lookUp.DisplayMember = lookInfo["StrDsiplayMember"] as string;
            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.ValueMember] = 0;
                dr[lookUp.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
            lookUp.PopupFormWidth = totalWidth;
            if (totalWidth > 600)
            {
                lookUp.PopupFormWidth = 600;
            }
            lookUp.DataSource = dt;
            if (isNeedFilter)
            {
                gc = new GridColumn();
                gc.Name = "colblnEnable";
                gc.FieldName = "blnEnable";
                gc.Caption = "colblnEnable";
                lookUp.View.Columns.Add(gc);
                lookUp.View.Columns["blnEnable"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo(true);
                lookUp.View.OptionsBehavior.Editable = true;
                lookUp.View.OptionsSelection.EnableAppearanceFocusedCell = false;
                lookUp.View.OptionsView.ShowFilterPanel = false;
                lookUp.ImmediatePopup = true;
            }

            SetGridLookUpColumnFormat(lookUp.View);

            if (lookUp.Buttons.Count > 0)
            {
                lookUp.Buttons[0].Visible = true;//显示出第一个按钮

            }
        }

        private static void SetGridLookUpColumnFormat(DevExpress.XtraGrid.Views.Grid.GridView gridView)
        {
            if (gridView.DataSource == null)
            {
                return;
            }

            int strWei = TypeUtil.ToInt(GetParameterValue("CoinDecBit"));
            int columnCount = gridView.Columns.Count;
            string strType = string.Empty;
            for (int i = 0; i < columnCount; i++)
            {
                strType = gridView.Columns[i].ColumnType.Name.ToLower();
                //根据列的数据类型来设置格式

                if (strType == "int32")
                {
                    gridView.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView.Columns[i].DisplayFormat.FormatString = "N0";
                    gridView.Columns[i].SummaryItem.DisplayFormat = "{0:#}";
                }
                else if (gridView.Columns[i].ColumnType.Name.ToLower() == "decimal")
                {
                    gridView.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    gridView.Columns[i].DisplayFormat.FormatString = "n" + strWei;
                    gridView.Columns[i].SummaryItem.DisplayFormat = "{0:n" + strWei + "}";
                }
                else if (gridView.Columns[i].ColumnType.Name.ToLower() == "datetime")
                {
                    gridView.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                    gridView.Columns[i].DisplayFormat.FormatString = "yyyy-MM-dd";
                }
            }
        }

        public static string GetParameterValue(string strKey)
        {
            string strValue = string.Empty;
            //System.Collections.IDictionary content = ClientCache.GetClientParameters();
            //if (content != null && content.Contains(strKey))
            //{
            //    strValue = TypeUtil.ToString(content[strKey]);
            //}

            return strValue;
        }

        /// <summary>
        /// 刷新 Lookup 数据源

        /// </summary>
        /// <param name="lookUpCode"></param>
        /// <param name="lookUp"></param>
        /// <param name="firstRowIsNull"></param>
        public static void RefreshLookUpDataSource(string lookUpCode, LookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }

            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.Properties.ValueMember] = 0;
                dr[lookUp.Properties.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }

            lookUp.Properties.DataSource = dt;
        }

        /// <summary>
        /// 刷新 RepositoryItemLookup 数据源

        /// </summary>
        /// <param name="lookUpCode"></param>
        /// <param name="lookUp"></param>
        /// <param name="firstRowIsNull"></param>
        public static void RefreshLookUpDataSource(string lookUpCode, RepositoryItemLookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }

            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.ValueMember] = 0;
                dr[lookUp.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }

            lookUp.DataSource = dt;
        }

        /// <summary>
        /// 刷新 GridLookup 数据源

        /// </summary>
        /// <param name="lookUpCode"></param>
        /// <param name="lookUp"></param>
        /// <param name="firstRowIsNull"></param>
        public static void RefreshGridLookUpDataSource(string lookUpCode, GridLookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }

            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            lookUp.Properties.DataSource = dt;

            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.Properties.ValueMember] = 0;
                dr[lookUp.Properties.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
        }

        /// <summary>
        /// 刷新 RepositoryItemGridLookup 数据源

        /// </summary>
        /// <param name="lookUpCode"></param>
        /// <param name="lookUp"></param>
        /// <param name="firstRowIsNull"></param>
        public static void RefreshGridLookUpDataSource(string lookUpCode, RepositoryItemGridLookUpEdit lookUp, bool firstRowIsNull)
        {
            bool blnNewData = false;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return;
            }

            DataTable dt = lookInfo["dataSource"] as DataTable;
            dt = dt.Copy();
            lookUp.DataSource = dt;

            if (dt != null && firstRowIsNull)
            {
                DataRow dr = dt.NewRow();
                dr[lookUp.ValueMember] = 0;
                dr[lookUp.DisplayMember] = "空";
                if (dt.Columns.Contains("blnEnable"))
                {
                    dr["blnEnable"] = true;
                }
                dt.Rows.InsertAt(dr, 0);
            }
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="lookUpCode"></param>
        /// <returns></returns>
        public static DataTable GetCacheData(string lookUpCode, ref bool blnNewData)
        {
            DataTable dt;
            Hashtable lookInfo = GetlookInfo(lookUpCode, ref blnNewData);
            if (lookInfo == null)
            {
                return null;
            }
            else
            {
                dt = lookInfo["dataSource"] as DataTable;
            }
            return dt;
        }

        public static Hashtable GetlookInfo(string lookUpCode, ref bool blnNewData)
        {


            DataTable dtfklib = listexModel.GetDataTable("select * from BFT_FKLib where strFKCode='" + lookUpCode + "'");
            if (dtfklib == null || dtfklib.Rows.Count == 0)
            {
                throw new Exception("未找到参照编码为"+lookUpCode+"的记录，请查看参照表配置！");
            }
            string strsql = RequestUtil.ProcessDynamicParameters(TypeUtil.ToString(dtfklib.Rows[0]["strSQL"]));
            DataTable dataSource = listexModel.GetDataTable(strsql);

            Hashtable lookInfo = new Hashtable();
            lookInfo.Add("StrDsiplayMember", TypeUtil.ToString(dtfklib.Rows[0]["StrDsiplayMember"]).Trim());
            lookInfo.Add("StrValueMember", TypeUtil.ToString(dtfklib.Rows[0]["StrValueMember"]).Trim());
            lookInfo.Add("StrColumns", TypeUtil.ToString(dtfklib.Rows[0]["StrColumns"]).Trim());
            lookInfo.Add("BlnDefaultSelection", TypeUtil.ToString(dtfklib.Rows[0]["BlnDefaultSelection"]).Trim());
            lookInfo.Add("StrEntityList", TypeUtil.ToString(dtfklib.Rows[0]["StrEntityList"]).Trim());
            lookInfo.Add("StrFKName", TypeUtil.ToString(dtfklib.Rows[0]["StrFKName"]).Trim());
            lookInfo.Add("StrTreeFkCode", TypeUtil.ToString(dtfklib.Rows[0]["StrTreeFkCode"]).Trim());
            lookInfo.Add("StrTreeFilterField", TypeUtil.ToString(dtfklib.Rows[0]["StrTreeFilterField"]).Trim());
            lookInfo.Add("StrGridFilterField", TypeUtil.ToString(dtfklib.Rows[0]["StrGridFilterField"]).Trim());
            lookInfo.Add("StrParentField", TypeUtil.ToString(dtfklib.Rows[0]["StrParentField"]).Trim());
            lookInfo.Add("StrCommandParameter", TypeUtil.ToString(dtfklib.Rows[0]["StrCommandParameter"]).Trim());
            lookInfo.Add("dataSource", dataSource);
            return lookInfo;
        }
    }
}
