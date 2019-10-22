using DevExpress.XtraBars.Ribbon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.Client.Display.Model;
using Basic.Framework.Logging;
using Sys.Safety.Enums;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Display
{
    public partial class RealHandControl : XtraForm
    {
        /// <summary>
        ///GRID分站数据源
        /// </summary>
        public static List<ControlInfItem> GridControlSource = new List<ControlInfItem>();

        /// 列表显示名称
        /// </summary>
        public string[] colname = new string[] { "主控测点", "被控测点", "控制类型" };

        /// <summary>
        /// 表列头名称
        /// </summary>
        public string[] tcolname = new string[] { "Zkpoint", "Bkpoint", "Type" };

        public int[] colwith = new int[] { 350, 350, 505 };

        private Thread freshthread;

        public RealHandControl()
        {
            InitializeComponent();
        }

        private void RealHandControl_Load(object sender, EventArgs e)
        {
            var tempControl = RealInterfaceFuction.QueryJCSDKZsCache();
            EvaluateControleDataSource(tempControl);
            GridColumn col;
            for (int i = 0; i < colname.Length; i++)
            {
                col = new GridColumn();
                col.Caption = colname[i];
                col.FieldName = tcolname[i];
                col.Width = colwith[i];
                col.Visible = true;
                col.OptionsFilter.AllowFilter = false;
                col.OptionsFilter.AllowAutoFilter = false;
                col.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                gridView1.Columns.Add(col);
            }
            if (GridControlSource != null)
            {
                gridControl1.DataSource = GridControlSource;
                gridControl1.RefreshDataSource();
            }

            freshthread = new Thread(new ThreadStart(fthread));
            freshthread.Start();
        }

        private void fthread()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    var tempControl = RealInterfaceFuction.QueryJCSDKZsCache();
                    MethodInvoker In = new MethodInvoker(()=> EvaluateControleDataSource(tempControl));
                    this.BeginInvoke(In);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        private void EvaluateControleDataSource(IList<Jc_JcsdkzInfo> tempControl)
        {
            try
            {
                int count = 0;
                if (null != tempControl)
                {
                    GridControlSource.Clear();
                    ControlInfItem ControlInf;
                    tempControl = tempControl.OrderBy(item => item.Bkpoint).ThenBy(item => item.Type).ToList();
                    foreach (var item in tempControl)
                    {
                        ControlInf = new ControlInfItem();
                        ControlInf.Bkpoint = item.Bkpoint;
                        ControlInf.Zkpoint = item.ZkPoint;
                        ControlInf.Type = EnumHelper.GetEnumDescription((ControlType)item.Type);
                        GridControlSource.Add(ControlInf);
                        count++;
                    }                   
                    StaticClass.yccount[6] = count;
                }
                gridControl1.RefreshDataSource();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
    }


    /// <summary>
    /// 分站对象
    /// </summary>
    [Serializable]
    public class ControlInfItem
    {

        /// <summary>
        ///被控测点
        /// </summary>
        public string Bkpoint { get; set; }

        /// <summary>
        /// 主控测点
        /// </summary>
        public string Zkpoint { get; set; }

        /// <summary>
        /// 控制类型
        /// </summary>
        public string Type { get; set; }
    }
}
