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
    public partial class PersonRealForm : XtraForm
    {

        public PersonRealForm()
        {
            InitializeComponent();
        }

        private showPersonReal obj = new showPersonReal();

        private Thread freshthread;
        private bool _isRun = false;

        IR_KqbcService r_KqbcService = ServiceFactory.Create<IR_KqbcService>();
        IR_PrealService r_PrealService = ServiceFactory.Create<IR_PrealService>();
        IR_PersoninfService r_PersoninfService = ServiceFactory.Create<IR_PersoninfService>();
        IPersonPointDefineService personPointDefineService = ServiceFactory.Create<IPersonPointDefineService>();

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

                labelControl1.Text = "";
                labelControl2.Text = "";

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


        private void getpoint()
        {

        }


        private void getmsg(R_KqbcInfo DefaultKqbc, List<R_PrealInfo> rPrealInfoList, List<R_PersoninfInfo> rPersoninfInfoList, List<Jc_DefInfo> pointdefList)
        {
            try
            {
                //计算今日下井人数
                obj.TodayNumberWells = rPrealInfoList.FindAll(a => a.Ontime >= GetFirstBcTime()).Count.ToString();
                //计算井下总人数
                obj.UndergroundNumber = rPrealInfoList.FindAll(a => a.Flag != "1").Count.ToString();


                obj.rPrealShowdt.Clear();
                //人员实时值
                List<R_PrealInfo> pointPrealList = rPrealInfoList.FindAll(a => a.Flag != "1");
                foreach (R_PrealInfo temppreal in pointPrealList)
                {
                    R_PersoninfInfo temppersoninf = rPersoninfInfoList.Find(a => a.Yid == temppreal.Yid);
                    object[] prealObj = new object[obj.rPrealShowdt.Columns.Count];
                    prealObj[0] = temppreal.Bh;
                    if (temppersoninf != null)
                    {
                        prealObj[1] = temppersoninf.Gh;
                    }
                    else
                    {
                        prealObj[1] = "";
                    }
                    if (temppersoninf != null)
                    {
                        prealObj[2] = temppersoninf.Name;
                    }
                    else
                    {
                        prealObj[2] = "";
                    }
                    if (temppersoninf != null)
                    {
                        prealObj[3] = temppersoninf.deptName;
                    }
                    else
                    {
                        prealObj[3] = "";
                    }
                    if (temppersoninf != null)
                    {
                        prealObj[4] = temppersoninf.zwDesc;
                    }
                    else
                    {
                        prealObj[4] = "";
                    }
                    if (temppersoninf != null)
                    {
                        prealObj[5] = temppersoninf.gzDesc;
                    }
                    else
                    {
                        prealObj[5] = "";
                    }
                    Jc_DefInfo tempdef = pointdefList.Find(a => a.PointID == temppreal.Pointid);
                    if (tempdef != null)
                    {
                        prealObj[6] = tempdef.Wz;
                    }
                    else
                    {
                        prealObj[6] = "";
                    }
                    prealObj[7] = temppreal.Rtime;
                    tempdef = pointdefList.Find(a => a.PointID == temppreal.Uppointid);
                    if (tempdef != null)
                    {
                        prealObj[8] = tempdef.Wz;
                    }
                    else
                    {
                        prealObj[8] = "";
                    }
                    tempdef = pointdefList.Find(a => a.PointID == temppreal.Onpointid);
                    if (tempdef != null)
                    {
                        prealObj[9] = tempdef.Wz;
                    }
                    else
                    {
                        prealObj[9] = "";
                    }
                    prealObj[10] = temppreal.Ontime;
                    int tempRjsc = 0;
                    int.TryParse(temppreal.Rjsc, out tempRjsc);
                    prealObj[11] = tempRjsc / 60 + "小时" + tempRjsc % 60 + "分钟";
                    prealObj[12] = temppreal.BjtypeDesc;
                    obj.rPrealShowdt.Rows.Add(prealObj);
                }



                realshow();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private DateTime GetFirstBcTime()
        {
            DateTime firstBcTime = new DateTime();
            R_KqbcInfo defaultKqbc = r_KqbcService.GetDefaultKqbcCache(new RKqbcCacheGetByConditionRequest()).Data;
            if (defaultKqbc != null)
            {
                firstBcTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + defaultKqbc.B1stime + ":00");
            }
            return firstBcTime;
        }

        private void realshow()
        {
            try
            {
                int x = -1, y = -1, count = 0, toprowindex = 0;
                labelControl1.Text = "今日下井人数:" + obj.TodayNumberWells + "人";
                labelControl2.Text = "当前井下人数:" + obj.UndergroundNumber + "人";


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

            R_KqbcInfo DefaultKqbc = r_KqbcService.GetDefaultKqbcCache(new RKqbcCacheGetByConditionRequest()).Data;

            List<R_PrealInfo> prealInfoList = r_PrealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
            List<R_PersoninfInfo> personinfInfoList = r_PersoninfService.GetAllPersonInfoCache(new BasicRequest()).Data;
            List<Jc_DefInfo> pointdefList = personPointDefineService.GetAllPointDefineCache().Data;
            getmsg(DefaultKqbc, prealInfoList, personinfInfoList, pointdefList);
            realshow();
        }

        private void fthread()
        {
            while (_isRun)
            {
                try
                {
                    R_KqbcInfo DefaultKqbc = r_KqbcService.GetDefaultKqbcCache(new RKqbcCacheGetByConditionRequest()).Data;
                    List<R_PrealInfo> prealInfoList = r_PrealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
                    List<R_PersoninfInfo> personinfInfoList = r_PersoninfService.GetAllPersonInfoCache(new BasicRequest()).Data;
                    List<Jc_DefInfo> pointdefList = personPointDefineService.GetAllPointDefineCache().Data;

                    MethodInvoker In = new MethodInvoker(() => getmsg(DefaultKqbc, prealInfoList, personinfInfoList, pointdefList));
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

    public class showPersonReal
    {
        public showPersonReal()
        {
            inidt();
        }
        /// <summary>
        /// 今日下井人数
        /// </summary>
        public string TodayNumberWells = "";
        /// <summary>
        /// 井下总人数
        /// </summary>
        public string UndergroundNumber = "";

        /// <summary>
        /// 人员实时信息显示表
        /// </summary>
        public DataTable rPrealShowdt = new DataTable();
        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] localControlColname = new string[] { "卡号", "工号", "人员姓名", "部门", "职务", "工种", "当前位置", "当前时间", "来源位置", "入井位置", "入井时间", "入井时长", "报警类型" };
        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] localControlTColname = new string[] { "bh", "gh", "name", "dept", "zw", "gz", "wz", "rtime", "uppoint", "onpoint", "ontime", "rjsc", "bjtype" };
        public int[] localControlColwith = new int[] { 50, 50, 80, 80, 80, 80, 150, 120, 150, 150, 120, 80, 100 };
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
            TodayNumberWells = "";
            UndergroundNumber = "";


            rPrealShowdt.Rows.Clear();
        }
    }
}
