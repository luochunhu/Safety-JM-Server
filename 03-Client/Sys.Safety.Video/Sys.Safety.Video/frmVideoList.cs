using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Def;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Video
{
    public partial class frmVideoList : XtraForm
    {
        private readonly IV_DefService _vdefService;

        public frmVideoList()
        {
            InitializeComponent();
            _vdefService = ServiceFactory.Create<IV_DefService>();
        }

        /// <summary>
        /// 添加视频测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVideoAdd addform = new frmVideoAdd();
            addform.ShowDialog();
            RefreshVideoList();
        }

        /// <summary>
        /// 编辑视频测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var videoId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
            if (videoId != null)
            {
                frmVideoAdd addform = new frmVideoAdd(videoId.ToString());
                addform.ShowDialog();
                RefreshVideoList();
            }
        }

        /// <summary>
        /// 删除视频测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确认删除该设备?", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    var videoId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Id");
                    if (videoId != null)
                    {
                        DefDeleteRequest defdeleteRequest = new DefDeleteRequest();
                        defdeleteRequest.Id = videoId.ToString();
                        var defdeleteResponse = _vdefService.DeleteDef(defdeleteRequest);
                        if (defdeleteResponse.IsSuccess)
                        {
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 操作成功");
                            RefreshVideoList();
                        }
                        else
                        {
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 视频测点删除失败");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("视频测点删除失败：" + ex.Message);
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 视频测点删除失败");
            }
        }

        /// <summary>
        /// 加载全部视频测点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshVideoList();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void RefreshVideoList() 
        {
            try
            {
                List<V_DefInfo> deflist = new List<V_DefInfo>();

                var vdefresponse = _vdefService.GetAllDef(new DefGetAllRequest());
                if (vdefresponse.IsSuccess)
                {
                    deflist = vdefresponse.Data;
                }
                this.videoGrid.DataSource = deflist;
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 数据准备就绪...");
            }
            catch (Exception ex)
            {
                LogHelper.Info("视频测点信息加载失败：" + ex.Message);
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 数据加载失败");
            }
        }
    }
}
