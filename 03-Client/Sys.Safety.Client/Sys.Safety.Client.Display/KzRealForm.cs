using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using System.Threading;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Display
{
    public partial class KzRealForm : XtraForm
    {

        public KzRealForm(string str)
        {
            obj.kzpoint = str;
            InitializeComponent();
        }

        public KzRealForm()
        {
            InitializeComponent();
        }

        private showkz obj = new showkz();

        private Thread freshthread;
        private bool _isRun = false;

        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();

        /// <summary>
        /// 初始显示表
        /// </summary>
        private void inigrid()
        {
            GridColumn col;
            for (int i = 0; i < obj.colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = obj.colname[i];
                col.FieldName = obj.tcolname[i];
                col.Width = obj.colwith[i];
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = obj.showdt;
        }

        private void KzRealForm_Load(object sender, EventArgs e)
        {
            try
            {
                inigrid();

                lb_wz.Text = "";
                lb_type.Text = "";
                lb_state.Text = "";
                lb_kddh.Text = "";
                lb_kddkdzt.Text = "";
                lb_kdwz.Text = "";
                lb_kddzt.Text = "";
                getpoint();

                _isRun = true;
                freshthread = new Thread(new ThreadStart(fthread));
                freshthread.IsBackground = true;
                freshthread.Start();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

        }

        /// <summary>
        /// 获取点号
        /// </summary>
        private void getpoint()
        {
            DataTable dt = new DataTable();
            cmb_adr.Properties.Items.Clear();
            dt = Model.RealInterfaceFuction.Getkzpoint();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmb_adr.Properties.Items.Add(dt.Rows[i]["point"].ToString());
                }
            }
            if (obj.kzpoint != "")
            {
                for (int i = 0; i < cmb_adr.Properties.Items.Count; i++)
                {
                    if (obj.kzpoint == cmb_adr.Properties.Items[i].ToString())
                    {
                        cmb_adr.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 根据测点获取数据
        /// </summary>
        private void getmsg(List<Jc_DefInfo> dtos, DataTable dt)
        {
            try
            {               
                string kdpoint = "";
                DataRow[] row;
                int i = 0, j = 0;
                if (obj.kzpoint != "")
                {
                    Jc_DefInfo dto = dtos.Find(a => a.Point == obj.kzpoint);
                    if (dto != null)
                    {
                        obj.kzwz = dto.Wz;
                        obj.kztype = dto.DevName;
                        obj.kzssz = dto.Ssz;
                        obj.kzsszcolor = Color.Blue;
                        lock (StaticClass.allPointDtLockObj)
                        {
                            row = StaticClass.AllPointDt.Select("point='" + obj.kzpoint + "'");
                            if (row.Length > 0)
                            {
                                int tempInt = 0;
                                int.TryParse(row[0]["sszcolor"].ToString(), out tempInt);
                                obj.kzsszcolor = Color.FromArgb(tempInt);
                            }
                            if (dto.DataState == StaticClass.itemStateToClient.EqpState43)
                            {
                                i = 0;
                            }
                            else if (dto.DataState == StaticClass.itemStateToClient.EqpState44)
                            {
                                i = 1;
                            }
                            #region 馈电点数据
                            if (dto.K1 > 0 && dto.K2 > 0)
                            {
                                lock (StaticClass.allPointDtLockObj)
                                {
                                    row = StaticClass.AllPointDt.Select("fzh='" + dto.K1 + "' and tdh='" + dto.K2 + "' and lx='开关量'");
                                    if (row.Length > 0)
                                    {
                                        kdpoint = row[0]["point"].ToString();
                                        dto = dtos.Find(a => a.Point == kdpoint);
                                        if (dto != null)
                                        {
                                            obj.kdpoint = dto.Point;
                                            obj.kdssz = dto.Ssz;
                                            obj.kdwz = dto.Wz;
                                            if (dto.DataState == StaticClass.itemStateToClient.EqpState25)
                                            {
                                                j = 1;
                                            }
                                            else if (dto.DataState == StaticClass.itemStateToClient.EqpState26)
                                            {
                                                j = 2;
                                            }
                                            else
                                            {
                                                j = 0;
                                            }
                                            obj.kdstate = getkdstate(i, j);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            obj.showdt = dt;
                        }
                        else if (dt != null && dt.Columns.Count > 0)
                        {
                            obj.showdt.Rows.Clear();
                        }
                    }
                }

                realshow();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private string getkdstate(int i, int j)
        {
            string msg = "未知";
            if (i == 1)
            {
                if (j == 2)
                {
                    msg = "断电失败";
                }
                else if (j == 1)
                {
                    msg = "断电成功";
                }
            }
            else
            {
                if (j == 2)
                {
                    msg = "复电成功";
                }
                else if (j == 1)
                {
                    msg = "复电失败";
                }
            }
            return msg;
        }


        private void realshow()
        {
            try
            {
                int x = -1, y = -1, count = 0, toprowindex = 0;
                lb_wz.Text = obj.kzwz;
                lb_type.Text = obj.kztype;
                lb_state.Text = obj.kzssz;
                lb_state.ForeColor = obj.kzsszcolor;
                lb_kddh.Text = obj.kdpoint;
                lb_kddkdzt.Text = obj.kdstate;
                if (obj.kdstate == "断电失败" || obj.kdstate == "复电失败")
                {
                    lb_kddkdzt.ForeColor = Color.Red;
                }
                else
                {
                    lb_kddkdzt.ForeColor = Color.Blue;
                }
                lb_kdwz.Text = obj.kdwz;
                lb_kddzt.Text = obj.kdssz;
                if (mainGridView.FocusedColumn != null)
                {
                    x = mainGridView.FocusedColumn.ColumnHandle;
                    y = mainGridView.FocusedRowHandle;
                }
                count = mainGridView.RowCount;
                mainGrid.DataSource = obj.showdt;

                if (obj.showdt.Rows.Count == count)
                {
                    mainGridView.FocusedColumn.ColumnHandle = x;
                    mainGridView.FocusedRowHandle = y;
                    if (x > -1 && y > -1)
                    {
                        mainGridView.FocusedColumn.ColumnHandle = x;
                        mainGridView.FocusedRowHandle = y;
                    }
                    mainGridView.TopRowIndex = toprowindex;
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmb_adr_SelectedIndexChanged(object sender, EventArgs e)
        {
            obj.showdt.Clear();
            obj.kzpoint = cmb_adr.Text;
            List<Jc_DefInfo> dtos = pointDefineService.GetAllPointDefineCache().Data;
            DataTable dt = Model.RealInterfaceFuction.Getzkpoint(obj.kzpoint);
            getmsg(dtos, dt);
            realshow();
        }

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    timer1.Enabled = false;
        //    try
        //    {
        //        obj.clear();
        //        getmsg();
        //        realshow();
        //    }
        //    catch (Exception ex)
        //    {
        //        Basic.Framework.Logging.LogHelper.Error(ex);
        //    }
        //    timer1.Enabled = true;
        //}
        private void fthread()
        {
            while (_isRun)
            {
                try
                {

                    List<Jc_DefInfo> dtos = pointDefineService.GetAllPointDefineCache().Data;
                    DataTable dt = Model.RealInterfaceFuction.Getzkpoint(obj.kzpoint);                    

                    MethodInvoker In = new MethodInvoker(() => getmsg(dtos, dt));
                    this.BeginInvoke(In);
                    //MethodInvoker In1 = new MethodInvoker(() => realshow());
                    //this.BeginInvoke(In1);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        private void mainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            string point = "";
            DataRow[] rows;
            try
            {
                if (e.Column.Tag.ToString() == "3")
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                if (e.Column.Tag.ToString() == "4")
                {
                    if (mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns[0]) != null)
                    {
                        point = mainGridView.GetRowCellValue(e.RowHandle, mainGridView.Columns[0]).ToString();
                        lock (StaticClass.allPointDtLockObj)
                        {
                            rows = StaticClass.AllPointDt.Select("point='" + point + "'");
                            if (rows.Length > 0)
                            {
                                int tempInt = 0;
                                int.TryParse(rows[0]["sszcolor"].ToString(), out tempInt);
                                if (string.IsNullOrEmpty(rows[0]["sszcolor"].ToString()))
                                {
                                    e.Appearance.ForeColor = StaticClass.realdataconfig.StateCorCfg.DefaultColor;
                                }
                                else
                                {
                                    e.Appearance.ForeColor = Color.FromArgb(tempInt);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void mainGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = Convert.ToString(e.RowHandle + 1);
            }
        }

        private void KzRealForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isRun = false;
        }
    }

    public class showkz
    {
        public showkz()
        {
            inidt();
        }
        /// <summary>
        /// 控制点号
        /// </summary>
        public string kzpoint = "";

        /// <summary>
        /// 控制位置
        /// </summary>
        public string kzwz = "";

        /// <summary>
        /// 控制类型
        /// </summary>
        public string kztype = "";

        /// <summary>
        /// 控制实时值
        /// </summary>
        public string kzssz = "";

        /// <summary>
        /// 控制实时值颜色
        /// </summary>
        public Color kzsszcolor = Color.Blue;


        /// <summary>
        /// 馈电点
        /// </summary>
        public string kdpoint = "";

        /// <summary>
        /// 馈电点位置
        /// </summary>
        public string kdwz = "";

        /// <summary>
        /// 馈电点实时值
        /// </summary>
        public string kdssz = "";

        /// <summary>
        /// 馈电点状态
        /// </summary>
        public string kdstate = "";

        /// <summary>
        /// 主控点显示表
        /// </summary>
        public DataTable showdt = new DataTable();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "测点编号", "安装位置", "设备类型", "控制类型", "实时值" };

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] { "point", "wz", "type", "kzlx", "ssz" };

        public int[] colwith = new int[] { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };

        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void inidt()
        {
            DataColumn col;
            showdt = new DataTable();
            for (int i = 0; i < tcolname.Length; i++)
            {
                col = new DataColumn(tcolname[i]);
                showdt.Columns.Add(col);
            }
        }

        public void clear()
        {
            kzwz = "";
            kztype = "";
            kzssz = "";
            kdpoint = "";
            kdwz = "";
            kdssz = "";
            kdstate = "";
            showdt.Rows.Clear();
        }
    }
}
