using DevExpress.XtraEditors;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Control.PersonInfo
{
    public partial class PersonCallManage : XtraForm
    {
        DataTable dt = new DataTable();
        public PersonCallManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 解除呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            try
            {
                int selectedHandle;
                selectedHandle = this.gridView1.GetSelectedRows()[0];
                if (selectedHandle >= 0)
                {
                    string Id = this.gridView1.GetRowCellValue(selectedHandle, "Id").ToString();
                    if (string.IsNullOrEmpty(Id))
                    {
                        XtraMessageBox.Show("选择的记录ID不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    Sys.Safety.Client.Control.Model.R_CallModel.R_CallModelInstance.RemoveR_CallInfo(Id);

                    XtraMessageBox.Show("呼叫解除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("请选择一条记录进行操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                RefreshGrid();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 添加呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var personCall = new PersonCall();
                var res = personCall.ShowDialog();
                

                RefreshGrid();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        private void IniTable()
        {
            dt.Columns.Clear();
            dt.Columns.Add("Id");
            dt.Columns.Add("MasterId");
            dt.Columns.Add("Type");
            dt.Columns.Add("CallType");
            dt.Columns.Add("CallPersonDefType");
            dt.Columns.Add("BhContent");
            dt.Columns.Add("PointList");
            dt.Columns.Add("CallTime");
            gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonCallManage_Load(object sender, EventArgs e)
        {
            try
            {
                IniTable();

                RefreshGrid();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshGrid()
        {
            try
            {
                dt.Rows.Clear();
                //加载所有人员呼叫信息
                List<R_CallInfo> R_CallList = Sys.Safety.Client.Control.Model.R_CallModel.R_CallModelInstance.GetAllRCallCache();
                foreach (R_CallInfo temp in R_CallList)
                {
                    object[] obj = new object[dt.Columns.Count];
                    obj[0] = temp.Id;
                    obj[1] = temp.MasterId;
                    switch (temp.Type)
                    {
                        case 0:
                            obj[2] = "人员呼叫";
                            break;
                        case 1:
                            obj[2] = "设备呼叫";
                            break;
                    }
                    switch (temp.CallType)
                    {
                        case 0:
                            obj[3] = "一般呼叫";
                            break;
                        case 1:
                            obj[3] = "紧急呼叫";
                            break;
                        case 2:
                            obj[3] = "解除呼叫";
                            break;
                    }
                    switch (temp.CallPersonDefType)
                    {
                        case 0:
                            obj[4] = "所有人员呼叫";
                            break;
                        case 1:
                            obj[4] = "呼叫指定卡号段";
                            break;
                        case 2:
                            obj[4] = "呼叫指定人员";
                            break;
                        case 3:
                            obj[4] = "呼叫所有设备";
                            break;
                        case 4:
                            obj[4] = "呼叫指定设备";
                            break;
                    }
                    obj[5] = temp.BhContent;
                    obj[6] = temp.PointList;
                    obj[7] = temp.CallTime.ToString("yyyy-MM-dd HH:mm:ss");
                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshGrid();
        }

    }
}
