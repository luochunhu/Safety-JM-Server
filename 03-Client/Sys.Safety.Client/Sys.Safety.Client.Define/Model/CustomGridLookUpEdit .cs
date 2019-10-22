using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.ComponentModel;
using System.Data;

namespace Sys.Safety.Client.Define.Model
{
    public class CustomGridLookUpEdit :  GridLookUpEdit 
    {
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit fProperties;
        private DevExpress.XtraGrid.Views.Grid.GridView fPropertiesView;
        /// <summary>
        /// 是否禁止新增内容
        /// </summary>
        [Browsable(true), Description("是否禁止新增内容")]
        public bool DisableAddNew { get; set; }

      
        public CustomGridLookUpEdit()
            : base()
        {
            //初始化一些状态
            this.Properties.PopupFilterMode = PopupFilterMode.Contains;//包含即可
            this.Properties.ImmediatePopup = true;//是否马上弹出窗体
            this.Properties.ValidateOnEnterKey = true;//回车确认
            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;//文本框可输入
            this.Properties.NullText = "";
            this.Properties.NullValuePrompt = "";
           
            this.ProcessNewValue += new DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(CustomGridLookUpEdit_ProcessNewValue);
        }

        /// <summary>
        /// 实现在列表没有记录的时候，可以录入一个不存在的记录，类似ComoboEidt功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomGridLookUpEdit_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (!DisableAddNew && !this.DesignMode)
            {   
                string displayName = this.Properties.DisplayMember;
                string valueName = this.Properties.ValueMember;
                string display = e.DisplayValue.ToString();

                DataTable dtTemp = this.Properties.DataSource as DataTable;
                if (dtTemp != null)
                {
                    DataRow[] selectedRows = dtTemp.Select(string.Format("{0}='{1}'", displayName, display.Replace("'", "‘")));
                    if (selectedRows == null || selectedRows.Length == 0)
                    {
                        DataRow row = dtTemp.NewRow();
                        row[displayName] = display;
                        row[valueName] = display;
                        dtTemp.Rows.Add(row);
                        dtTemp.AcceptChanges();
                    }
                }

                e.Handled = true;
            }
        }

        private void InitializeComponent()
        {
            this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.fPropertiesView = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fPropertiesView)).BeginInit();
            this.SuspendLayout();
            // 
            // fProperties
            // 
            this.fProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.fProperties.Name = "fProperties";
            this.fProperties.View = this.fPropertiesView;
            // 
            // fPropertiesView
            // 
            this.fPropertiesView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.fPropertiesView.Name = "fPropertiesView";
            this.fPropertiesView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.fPropertiesView.OptionsView.ShowGroupPanel = false;
            ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fPropertiesView)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
