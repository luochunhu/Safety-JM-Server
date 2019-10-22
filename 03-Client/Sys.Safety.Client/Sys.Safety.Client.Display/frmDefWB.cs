using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.User;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.Request.Jc_Defwb;
using Basic.Framework.Service;
using Sys.Safety.Request.PointDefine;

namespace Sys.Safety.Client.Display
{
    public partial class frmDefWB : DevExpress.XtraEditors.XtraForm
    {
        //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
        IUserService userService = ServiceFactory.Create<IUserService>();
        IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();
        IDeviceKoriyasuService jc_DefwbService = ServiceFactory.Create<IDeviceKoriyasuService>();
        IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>(); 
        public frmDefWB()
        {
            InitializeComponent();
        }

        public frmDefWB(long _PointID)
        {
            InitializeComponent();
            ClientItem _ClientItem = Basic.Framework.Common.JSONHelper.ParseJSONString<ClientItem>(Basic.Framework.Data.PlatRuntime.Items["_ClientItem"].ToString());
            string strUserCode = _ClientItem != null ? _ClientItem.UserName : "";
            var request = new UserGetByCodeRequest() { Code = strUserCode };
            var response = userService.GetUserByCode(request);
            if (response.Data != null)
            {
                this.txtUserName.Text = response.Data.UserName;
                this.txtUserName.Tag = response.Data.UserID;
            }

            PointDefineGetByPointIDRequest PointDefineRequest = new PointDefineGetByPointIDRequest();
            PointDefineRequest.PointID = _PointID.ToString();
            List<Jc_DefInfo> DefList = _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest).Data;
            Jc_DefInfo dto = null;
            if (DefList.Count > 0)
            {
                dto = DefList[0];
            }
            if (dto != null)
            {
                this.txtDefName.Text = dto.Point;
                this.txtDefName.Tag = dto.PointID;
            }
        }

        public frmDefWB(Dictionary<string, string> param)
        {
            InitializeComponent();
            //ClientItem _ClientItem = Basic.Framework.Common.JSONHelper.ParseJSONString<ClientItem>(Basic.Framework.Data.PlatRuntime.Items["_ClientItem"].ToString());
            ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items["_ClientItem"] as ClientItem;
            string strUserCode = _ClientItem != null ? _ClientItem.UserName : "";
            var request = new UserGetByCodeRequest() { Code = strUserCode };
            var response = userService.GetUserByCode(request);
            if (response.Data != null)
            {
                this.txtUserName.Text = response.Data.UserName;
                this.txtUserName.Tag = response.Data.UserID;
            }

            PointDefineGetByPointIDRequest PointDefineRequest = new PointDefineGetByPointIDRequest();
            PointDefineRequest.PointID = param["PointID"].ToString();
            List<Jc_DefInfo> DefList = _PointDefineService.GetPointDefineCacheByPointID(PointDefineRequest).Data;
            Jc_DefInfo dto = null;
            if (DefList.Count > 0)
            {
                dto = DefList[0];
            }           
            if (dto != null)
            {
                this.txtDefName.Text = dto.Point;
                this.txtDefName.Tag = dto.PointID;
            }
        }

        private void frmDefWB_Load(object sender, EventArgs e)
        {

            try
            {
                LoadData();
                this.Text = "设备维保记录";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("加载数据失败,原因为" + ex.ToString());
            }
        }

        private void LoadData()
        {
            var request = new GetMaintenanceHistoryByPointIdRequst() { PointId = Convert.ToInt64(this.txtDefName.Tag) };
            var response = realMessageService.GetMaintenanceHistoryByPointId(request);
            if (response.Data != null)
            {
                this.gridControl1.DataSource = response.Data;
            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            int rowHandle = e.RowHandle + 1;
            if (e.Info.IsRowIndicator && rowHandle > 0)
            {
                e.Info.DisplayText = rowHandle.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtRemark.Text.Trim() == "")
                {
                    MessageBox.Show("请录入维保说明！", "错误");
                    return;
                }
                var request = new DeviceKoriyasuAddRequest();
                var defwbInfo = new Jc_DefwbInfo();
                defwbInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                defwbInfo.Pointid = Convert.ToInt64(this.txtDefName.Tag).ToString();
                defwbInfo.User = Convert.ToInt64(txtUserName.Tag).ToString();
                defwbInfo.Remerk = this.txtRemark.Text;
                defwbInfo.Timer = DateTime.Now;
                request.Jc_DefwbInfo = defwbInfo;
                jc_DefwbService.AddDeviceKoriyasu(request);

                LoadData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("增加维保记录失败！原因为" + ex.ToString());
            }

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string strFileType = ".xls";
            Export(1, strFileType);
        }

        /// <summary>
        /// 导出通用方法
        /// </summary>
        /// <param name="LngTypeID">文件类型ID，用于默认选择类型</param>
        /// <param name="strFileType">文件类型名称，用于给文件名赋后缀名</param>
        private void Export(int LngTypeID, string strFileType)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls|PDF文件(*.pdf)|*.pdf|TXT文件(*.txt)|*.txt|CSV文件(*.csv)|*.csv|HTML文件(*.html)|*.html";
                saveFileDialog.Title = "保存文件";
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.FileName = "设备" + txtDefName.Text + "维保历史记录";
                saveFileDialog.FilterIndex = LngTypeID;
                string strShowStyle = "gridview";


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (strShowStyle == "chart")
                    {   //图表导出

                    }
                    else
                    {
                        gridView1.OptionsPrint.PrintHeader = true;//设置后列表的列名会打印出来
                        gridView1.OptionsPrint.PrintFooter = true;//设置后列表有footer（如小计）
                        //gridView.OptionsPrint.UsePrintStyles = false;//设置后为保留列表颜色样式
                        //列表方式
                        if (LngTypeID == 1)
                        {
                            gridView1.ExportToXls(saveFileDialog.FileName);
                        }
                        if (LngTypeID == 2)
                        {
                            gridView1.ExportToPdf(saveFileDialog.FileName);
                        }
                        if (LngTypeID == 3)
                        {
                            gridView1.ExportToText(saveFileDialog.FileName);
                        }
                        if (LngTypeID == 4)
                        {
                            gridView1.ExportToCsv(saveFileDialog.FileName);
                        }
                        if (LngTypeID == 5)
                        {
                            gridView1.ExportToHtml(saveFileDialog.FileName);
                        }


                    }

                    if (DialogResult.Yes == MessageBox.Show("是否立即打开此文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        System.Diagnostics.Process.Start(saveFileDialog.FileName);

                    }

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



        }


    }
}