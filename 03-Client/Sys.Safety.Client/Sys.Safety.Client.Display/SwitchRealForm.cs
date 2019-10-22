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

namespace Sys.Safety.Client.Display
{
    public partial class SwitchRealForm : XtraForm
    {

        public SwitchRealForm(string str)
        {
            obj.switchpoint = str;
            InitializeComponent();
        }

        public SwitchRealForm()
        {
            InitializeComponent();
        }

        private showSwitch obj = new showSwitch();

        private Thread freshthread;
        private bool _isRun = false;

        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();     

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
            mainGrid.DataSource = obj.localControlShowdt;
        }

        private void KzRealForm_Load(object sender, EventArgs e)
        {
            try
            {
                inigrid();

                lb_wz.Text = "";
                lb_type.Text = "";
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
            pointDefineRequest.DevpropertID = 2;
            dt = Basic.Framework.Common.ObjectConverter.ToDataTable<Jc_DefInfo>(pointDefineService.GetPointDefineCacheByDevpropertID(pointDefineRequest).Data);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmb_adr.Properties.Items.Add(dt.Rows[i]["point"].ToString());
                }
            }
            if (obj.switchpoint != "")
            {
                for (int i = 0; i < cmb_adr.Properties.Items.Count; i++)
                {
                    if (obj.switchpoint == cmb_adr.Properties.Items[i].ToString())
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
        private void getmsg(List<Jc_DefInfo> pointDefineList, List<Jc_BInfo> alarmInfoList)
        {
            try
            {
                Jc_DefInfo point = pointDefineList.Find(a => a.Point == obj.switchpoint);
                if (point != null)
                {
                    obj.switchwz = point.Wz;
                    obj.switchtype = point.DevName;
                    obj.switchstate = EnumHelper.GetEnumDescription((DeviceDataState)point.DataState);
                    obj.switchssz = point.Ssz;
                    if (point.Alarm > 0)
                    {
                        obj.switchsszcolor = Color.Red;
                    }
                    else
                    {
                        obj.switchsszcolor = Color.Green;
                    }
                    //查找当前测点控制关联信息
                    if (point.K1 > 0 || point.K2 > 0 || point.K3 > 0 
                        || point.Jckz1.Trim().Length > 0 || point.Jckz2.Trim().Length > 0 || point.Jckz3.Trim().Length > 0)
                    {
                        obj.localControlShowdt.Clear();
                        //模拟量实时值需要显示单位
                        List<Jc_BInfo> analogalrmList = alarmInfoList.FindAll(a => a.Kdid != null && a.PointID == point.PointID);
                        Dictionary<string, string> LinkagePoint = GetControlPoint(point);
                        foreach (var link in LinkagePoint)
                        {
                            Jc_DefInfo temppoint = pointDefineList.FirstOrDefault(tp => tp.Point == link.Value);
                            if (temppoint != null)
                            {
                                object[] controlObj = new object[obj.localControlShowdt.Columns.Count];
                                controlObj[0] = temppoint.Point;
                                controlObj[1] = temppoint.Wz;
                                controlObj[2] = temppoint.DevName;
                                controlObj[3] = link.Key.Substring(0, link.Key.IndexOf("-"));
                                controlObj[4] = temppoint.Ssz;
                                //控制状态                                
                                if ((point.Bz4 & 0x02) == 0x02)
                                {
                                    controlObj[4] = "休眠";
                                }
                                //馈电状态获取
                                controlObj[5] = "正常";
                                if (temppoint.NCtrlSate == 32)
                                {//断电失败

                                    controlObj[5] = "断电失败";
                                }
                                else if (temppoint.NCtrlSate == 2)
                                {//断电成功
                                    controlObj[5] = "正常";
                                }
                                else if (temppoint.NCtrlSate == 0)
                                {//复电成功
                                    controlObj[5] = "正常";
                                }
                                else if (temppoint.NCtrlSate == 30)
                                {//复电失败
                                    controlObj[5] = "复电失败";
                                }
                                obj.localControlShowdt.Rows.Add(controlObj);
                            }
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
                lb_wz.Text = obj.switchwz;
                lb_type.Text = obj.switchtype;
                lb_value.Text = obj.switchssz;
                lb_value.ForeColor = obj.switchsszcolor;

                if (mainGridView.FocusedColumn != null)
                {
                    x = mainGridView.FocusedColumn.ColumnHandle;
                    y = mainGridView.FocusedRowHandle;
                }
                count = mainGridView.RowCount;
                mainGrid.DataSource = obj.localControlShowdt;

                if (obj.localControlShowdt.Rows.Count == count)
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
            obj.clear();
            obj.switchpoint = cmb_adr.Text;
            List<Jc_DefInfo> pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
            List<Jc_BInfo> alarmInfoList = Model.RealInterfaceFuction.GetBjData();
            getmsg(pointDefineList, alarmInfoList);
            realshow();
        }
        private void fthread()
        {
            while (_isRun)
            {
                try
                {

                    List<Jc_DefInfo> pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
                    List<Jc_BInfo> alarmInfoList =  Model.RealInterfaceFuction.GetBjData();

                    MethodInvoker In = new MethodInvoker(() => getmsg(pointDefineList, alarmInfoList));
                    this.BeginInvoke(In);

                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// 获取模拟量开关量的所有控制口关联信息
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetControlPoint(Jc_DefInfo point)
        {
            Dictionary<string, string> LinkagePoint = new Dictionary<string, string>();
            //查找所有关联控制口的状态                            
            List<string> tempK = GetLocalControlPoint(point.Fzh, point.K1);//上限报警控制口，0态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Values.Contains("0态控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("0态控制-" + K, K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K2);//上限断电控制口，1态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Values.Contains("1态控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("1态控制-" + K, K);
                }
            }
            tempK = GetLocalControlPoint(point.Fzh, point.K3);//下限报警控制口，2态控制口
            foreach (string K in tempK)
            {
                if (!LinkagePoint.Values.Contains("2态控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("2态控制-" + K, K);
                }
            }


            //获取交叉控制信息
            string[] TempJckz = point.Jckz1.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Values.Contains("0态交叉控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("0态交叉控制-" + K, K);
                }
            }
            TempJckz = point.Jckz2.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Values.Contains("1态交叉控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("1态交叉控制-" + K, K);
                }
            }
            TempJckz = point.Jckz3.Split('|');
            foreach (string K in TempJckz)
            {
                if (!LinkagePoint.Values.Contains("2态交叉控制-" + K) && K.Length > 0)
                {
                    LinkagePoint.Add("2态交叉控制-" + K, K);
                }
            }
            return LinkagePoint;
        }
        /// <summary>
        /// 获取本地控制口测点号列表信息
        /// </summary>
        /// <param name="K"></param>
        /// <returns></returns>
        private List<string> GetLocalControlPoint(int Fzh, int K)
        {
            List<string> temp = new List<string>();
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {
                            temp.Add(Fzh.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0");
                        }
                        else
                        {
                            temp.Add(Fzh.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1");
                        }
                    }
                }
            }
            return temp;
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

    public class showSwitch
    {
        public showSwitch()
        {
            inidt();
        }
        /// <summary>
        /// 模拟量点号
        /// </summary>
        public string switchpoint = "";

        /// <summary>
        /// 模拟量位置
        /// </summary>
        public string switchwz = "";

        /// <summary>
        /// 模拟量类型
        /// </summary>
        public string switchtype = "";

        /// <summary>
        /// 模拟量状态
        /// </summary>
        public string switchstate = "";

        /// <summary>
        /// 模拟量实时值
        /// </summary>
        public string switchssz = "";

        /// <summary>
        /// 模拟量实时值颜色
        /// </summary>
        public Color switchsszcolor = Color.Blue;


        /// <summary>
        /// 本地控制显示表
        /// </summary>
        public DataTable localControlShowdt = new DataTable();

        /// <summary>
        /// 列表显示名称
        /// </summary>
        public string[] localControlColname = new string[] { "测点编号", "安装位置", "设备类型", "控制类型", "控制状态", "馈电状态" };

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] localControlTColname = new string[] { "point", "wz", "type", "kzlx", "controlstate", "feedstate" };

        public int[] localControlColwith = new int[] { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };


        /// <summary>
        /// 初始化数据源
        /// </summary>
        private void inidt()
        {
            DataColumn col;
            localControlShowdt = new DataTable();
            for (int i = 0; i < localControlTColname.Length; i++)
            {
                col = new DataColumn(localControlTColname[i]);
                localControlShowdt.Columns.Add(col);
            }
        }
        public void clear()
        {
            switchwz = "";
            switchtype = "";
            switchstate = "";
            switchssz = "";

            localControlShowdt.Rows.Clear();
        }
    }
}
