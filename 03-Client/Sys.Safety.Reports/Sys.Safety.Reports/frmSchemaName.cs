using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;

namespace Sys.Safety.Reports
{
    public partial class frmSchemaName : XtraForm
    {
        private string _schemaName = string.Empty;

        public frmSchemaName()
        {
            LngUseRight = 0;
            ExistSchemaNameList = null;
            InitializeComponent();
        }

        /// <summary>
        ///     存在的方案名列表
        /// </summary>
        public IList<string> ExistSchemaNameList { get; set; }

        /// <summary>
        ///     方案名
        /// </summary>
        public string SchemaName
        {
            get { return _schemaName; }
            set { _schemaName = value; }
        }

        /// <summary>
        ///     使用权限 0公开 1 私有
        /// </summary>
        public int LngUseRight { get; private set; }

        private void frmSchemaName_Load(object sender, EventArgs e)
        {
            txtSchemaName.Text = _schemaName;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var str = txtSchemaName.Text.Trim();
            if (str == string.Empty)
            {
                MessageShowUtil.ShowInfo("方案名不能为空，请重新输入!");
                return;
            }
            if (ExistSchemaNameList.Contains(str) && (str != _schemaName))
            {
                MessageShowUtil.ShowInfo("此方案名已存在，请重新输入!");
                return;
            }

            LngUseRight = TypeUtil.ToInt(radioGroupUseRight.EditValue);
            if (LngUseRight == 0)
            {
                //保存公有方案 需判断权限

                //if (!PermissionManager.HavePermission("AddListPublicSchema"))
                //{
                //    MessageBox.Show("无新增公有方案权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
            }

            _schemaName = str;
            Close();
        }

        private void blnCancel_Click(object sender, EventArgs e)
        {
            _schemaName = "";
            Close();
        }
    }
}