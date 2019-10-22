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
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Sys.Safety.Enums;
using Sys.Safety.Request.PersonCache;
using Basic.Framework.Web;

namespace Sys.Safety.Client.Display
{
    public partial class RecognizerRealForm : XtraForm
    {

        public RecognizerRealForm(string str)
        {
            obj.analogpoint = str;
            InitializeComponent();
        }

        public RecognizerRealForm()
        {
            InitializeComponent();
        }

        private showRecognizer obj = new showRecognizer();

        private Thread freshthread;
        private bool _isRun = false;

        IPersonPointDefineService pointDefineService = ServiceFactory.Create<IPersonPointDefineService>();
        IR_PrealService r_PrealService = ServiceFactory.Create<IR_PrealService>();
        IR_PersoninfService r_PersoninfService = ServiceFactory.Create<IR_PersoninfService>();

        /// <summary>
        /// 初始显示表
        /// </summary>
        private void inigrid()
        {
            GridColumn col;
            for (int i = 0; i < obj.localControlColname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = obj.localControlColname[i];
                col.FieldName = obj.localControlTColname[i];
                col.Width = obj.localControlColwith[i];
                col.Tag = i;
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mainGridView.Columns.Add(col);
            }
            mainGrid.DataSource = obj.rPrealShowdt;
        }

        private void KzRealForm_Load(object sender, EventArgs e)
        {
            try
            {
                inigrid();

                lb_wz.Text = "";
                lb_type.Text = "";
                lb_state.Text = "";
                lb_value.Text = "";

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
            PointDefineGetByDevpropertIDRequest pointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            pointDefineRequest.DevpropertID = 7;
            dt = Basic.Framework.Common.ObjectConverter.ToDataTable<Jc_DefInfo>(pointDefineService.GetPointDefineCacheByDevpropertID(pointDefineRequest).Data);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmb_adr.Properties.Items.Add(dt.Rows[i]["point"].ToString());
                }
            }
            if (obj.analogpoint != "")
            {
                for (int i = 0; i < cmb_adr.Properties.Items.Count; i++)
                {
                    if (obj.analogpoint == cmb_adr.Properties.Items[i].ToString())
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
        private void getmsg(List<Jc_DefInfo> pointDefineList, List<R_PrealInfo> rPrealInfoList, List<R_PersoninfInfo> rPersoninfInfoList)
        {
            try
            {
                Jc_DefInfo point = pointDefineList.Find(a => a.Point == obj.analogpoint);
                if (point != null)
                {
                    obj.analogwz = point.Wz;
                    //obj.analogtype = point.RecognizerTypeDesc;
                    obj.analogtype = point.DevName;
                    obj.analogstate = EnumHelper.GetEnumDescription((DeviceDataState)point.DataState);
                    //断线、上溢、负漂、未知等情况不显示单位
                    obj.analogssz = point.K1 + "人";//显示识别器人数
                    if (point.Alarm > 0)
                    {
                        obj.analogsszcolor = Color.Red;
                    }
                    else
                    {
                        obj.analogsszcolor = Color.Green;
                    }
                    //查找当前测点控制关联信息
                    if (point.K1 > 0)
                    {
                        obj.rPrealShowdt.Clear();
                        //人员实时值
                        //List<R_PrealInfo> pointPrealList = rPrealInfoList.FindAll(a => a.Pointid == point.PointID && a.Flag != "1").OrderBy(a => a.Bh).ToList();
                        List<R_PrealInfo> pointPrealList = rPrealInfoList.FindAll(a => a.CurrentPosition.Contains(point.Point) && a.Flag != "1").OrderBy(a => a.Bh).ToList();
                        foreach (R_PrealInfo temppreal in pointPrealList)
                        {
                            R_PersoninfInfo temppersoninf = rPersoninfInfoList.Find(a => a.Yid == temppreal.Yid);
                            object[] prealObj = new object[obj.rPrealShowdt.Columns.Count];
                            prealObj[0] = temppreal.Bh;
                            //if (temppersoninf != null)
                            //{
                            //prealObj[1] = temppreal.Gh;
                            prealObj[1] = temppreal.JobNumber;
                            //}
                            //else
                            //{
                            //    prealObj[1] = "";
                            //}
                            //if (temppersoninf != null)
                            //{
                            //prealObj[2] = temppersoninf.Name;
                            prealObj[2] = temppreal.PersonName;
                            //}
                            //else
                            //{
                            //    prealObj[2] = "";
                            //}
                            //if (temppersoninf != null)
                            //{
                            //prealObj[3] = temppersoninf.deptName;
                            prealObj[3] = temppreal.Department;
                            //}
                            //else
                            //{
                            //    prealObj[3] = "";
                            //}
                            //if (temppersoninf != null)
                            //{
                            //prealObj[4] = temppersoninf.zwDesc;
                            prealObj[4] = temppreal.Duty;
                            //}
                            //else
                            //{
                            //    prealObj[4] = "";
                            //}
                            //if (temppersoninf != null)
                            //{
                            //prealObj[5] = temppersoninf.gzDesc;
                            prealObj[5] = temppreal.TypeOfWork;
                            //}
                            //else
                            //{
                            //    prealObj[5] = "";
                            //}

                            //Jc_DefInfo tempdef = pointDefineList.Find(a => a.PointID == temppreal.Pointid);
                            //if (tempdef != null)
                            //{
                            //prealObj[6] = tempdef.Wz;
                            prealObj[6] = temppreal.CurrentPosition;
                            //}
                            //else
                            //{
                            //    prealObj[6] = "";
                            //}
                            prealObj[7] = temppreal.Rtime;
                            //tempdef = pointDefineList.Find(a => a.PointID == temppreal.Uppointid);
                            //if (tempdef != null)
                            //{
                            //prealObj[8] = tempdef.Wz;
                            prealObj[8] = temppreal.UpPosition;
                            //}
                            //else
                            //{
                            //    prealObj[8] = "";
                            //}
                            //tempdef = pointDefineList.Find(a => a.PointID == temppreal.Onpointid);
                            //if (tempdef != null)
                            //{
                            //prealObj[9] = tempdef.Wz;
                            prealObj[9] = temppreal.OnPosition;
                            //}
                            //else
                            //{
                            //    prealObj[9] = "";
                            //}
                            prealObj[10] = temppreal.Ontime;
                            int tempRjsc = 0;
                            int.TryParse(temppreal.Rjsc, out tempRjsc);
                            prealObj[11] = tempRjsc / 60 + "小时" + tempRjsc % 60 + "分钟";
                            //prealObj[12] = temppreal.BjtypeDesc;
                            obj.rPrealShowdt.Rows.Add(prealObj);
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

        private void realshow()
        {
            try
            {
                int x = -1, y = -1, count = 0, toprowindex = 0;
                lb_wz.Text = obj.analogwz;
                lb_type.Text = obj.analogtype;
                lb_state.Text = obj.analogstate;
                lb_value.Text = obj.analogssz;
                lb_value.ForeColor = obj.analogsszcolor;

                if (mainGridView.FocusedColumn != null)
                {
                    x = mainGridView.FocusedColumn.ColumnHandle;
                    y = mainGridView.FocusedRowHandle;
                }
                count = mainGridView.RowCount;
                mainGrid.DataSource = obj.rPrealShowdt;

                if (obj.rPrealShowdt.Rows.Count == count)
                {
                    if (mainGridView.FocusedColumn != null)
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
            obj.clear();
            obj.analogpoint = cmb_adr.Text;
            List<Jc_DefInfo> pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
            List<R_PrealInfo> prealInfoList = r_PrealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
            List<R_PersoninfInfo> personinfInfoList = r_PersoninfService.GetAllPersonInfoCache(new BasicRequest()).Data;
            getmsg(pointDefineList, prealInfoList, personinfInfoList);
            realshow();
        }

        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    List<Jc_DefInfo> pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
                    List<R_PrealInfo> prealInfoList = r_PrealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
                    List<R_PersoninfInfo> personinfInfoList = r_PersoninfService.GetAllPersonInfoCache(new BasicRequest()).Data;

                    MethodInvoker In = new MethodInvoker(() => getmsg(pointDefineList, prealInfoList, personinfInfoList));
                    this.BeginInvoke(In);

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
                                e.Appearance.ForeColor = Color.FromArgb(int.Parse(rows[0]["sszcolor"].ToString()));
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

    public class showRecognizer
    {
        public showRecognizer()
        {
            inidt();
        }
        /// <summary>
        /// 识别器点号
        /// </summary>
        public string analogpoint = "";
        /// <summary>
        /// 识别器位置
        /// </summary>
        public string analogwz = "";
        /// <summary>
        /// 识别器类型
        /// </summary>
        public string analogtype = "";
        /// <summary>
        /// 识别器状态
        /// </summary>
        public string analogstate = "";
        /// <summary>
        /// 识别器实时值（实时人数）
        /// </summary>
        public string analogssz = "";
        /// <summary>
        /// 人员实时值颜色
        /// </summary>
        public Color analogsszcolor = Color.Blue;
        /// <summary>
        /// 人员实时信息显示表
        /// </summary>
        public DataTable rPrealShowdt = new DataTable();
        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] localControlColname = new string[] { "卡号", "工号", "人员姓名", "部门", "职务", "工种", "当前位置", "当前时间", "来源位置", "入井位置", "入井时间", "入井时长" };//, "报警类型" 
        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] localControlTColname = new string[] { "bh", "gh", "name", "dept", "zw", "gz", "wz", "rtime", "uppoint", "onpoint", "ontime", "rjsc" };//, "bjtype"
        public int[] localControlColwith = new int[] { 50, 50, 80, 80, 80, 80, 150, 120, 150, 150, 120, 80 };//, 100 
        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void inidt()
        {
            DataColumn col;
            rPrealShowdt = new DataTable();
            for (int i = 0; i < localControlTColname.Length; i++)
            {
                col = new DataColumn(localControlTColname[i]);
                rPrealShowdt.Columns.Add(col);
            }
        }
        public void clear()
        {
            analogwz = "";
            analogtype = "";
            analogstate = "";
            analogssz = "";

            rPrealShowdt.Rows.Clear();
        }
    }
}
