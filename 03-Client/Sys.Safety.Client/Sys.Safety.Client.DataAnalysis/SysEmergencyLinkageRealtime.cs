using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    /// <summary>
    /// 应急联动主控详细信息
    /// </summary>
    public partial class SysEmergencyLinkageRealtime : XtraForm
    {
        private ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();

        ILargeDataAnalysisCacheClientService largedataAnalysisConfigCacheService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        private List<DefInfoExtend> realInfos = new List<DefInfoExtend>();

        private List<AnalysisConfigExtend> analysisConfig = new List<AnalysisConfigExtend>();

        private SysEmergencyLinkageInfo _sysEmergencyLinkageInfo;

        private string _emergencyLinkageId;

        private bool isrun = true;

        public SysEmergencyLinkageRealtime(string emergencyLinkageId)
        {
            InitializeComponent();

            _emergencyLinkageId = emergencyLinkageId;

            _sysEmergencyLinkageInfo = sysEmergencyLinkageService.GetSysEmergencyLinkageById(new Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest { Id = _emergencyLinkageId }).Data;

            InitGridControl();
            GetGridControlDataSource();
            if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 1)
            {
                this.gridControl1.DataSource = realInfos;
            }
            else if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 2)
            {
                this.gridControl1.DataSource = analysisConfig;
            }
            gridControl1.Refresh();

            //刷新实时值线程
            Thread refreshThread = new Thread(new ThreadStart(RfreshRealVaule));
            refreshThread.IsBackground = true;
            refreshThread.Start();
        }

        private void RfreshRealVaule()
        {
            while (isrun)
            {
                GetGridControlDataSource();
            }
        }

        private void GetGridControlDataSource()
        {
            _sysEmergencyLinkageInfo = sysEmergencyLinkageService.GetSysEmergencyLinkageById(new Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest { Id = _emergencyLinkageId }).Data;

            //普通联动获取监控主控测点
            if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 1)
            {
                realInfos = sysEmergencyLinkageService.GetAllMasterPointsById(new Sys.Safety.Request.SysEmergencyLinkage.SysEmergencyLinkageGetRequest { Id = _emergencyLinkageId }).Data.Select(o => new DefInfoExtend
                {
                    Point = o.Point,
                    Wz = o.Wz,
                    DevName = o.DevName,
                    Ssz = o.Ssz,
                    DataState = EnumHelper.GetEnumDescription((DeviceDataState)o.DataState),
                    State = EnumHelper.GetEnumDescription((DeviceRunState)o.State),
                }).ToList();
            }
            //大数据分析联动获取分析模型
            else if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 2)
            {
                var analysisConfigs = largedataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(new Sys.Safety.Request.LargeDataAnalysisCacheClientGetAllRequest()).Data;
                if (analysisConfigs != null && analysisConfigs.Count > 0)
                {
                    analysisConfig = analysisConfigs.Where(o => o.Id == _sysEmergencyLinkageInfo.MasterModelId).Select(o => new AnalysisConfigExtend
                    {
                        Name = o.Name,
                        Result = o.AnalysisResult == 2 ? o.TrueDescription : o.FalseDescription
                    }).ToList();
                }
            }
        }

        private void InitGridControl()
        {
            //普通联动获取监控主控测点
            if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 1)
            {
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "测点号", Name = "Point", FieldName = "Point", VisibleIndex = 0 });
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "安装位置", Name = "Wz", FieldName = "Wz", VisibleIndex = 1 });
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "实时值", Name = "Ssz", FieldName = "Ssz", VisibleIndex = 2 });
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "数据状态", Name = "DataState", FieldName = "DataState", VisibleIndex = 3 });
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "设备状态", Name = "State", FieldName = "State", VisibleIndex = 4 });
                this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "设备类型", Name = "DevName", FieldName = "DevName", VisibleIndex = 5 });
            }
            //大数据分析联动获取分析模型
            else if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 2)
            {
                var analysisConfigs = largedataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(new Sys.Safety.Request.LargeDataAnalysisCacheClientGetAllRequest()).Data;
                if (analysisConfigs != null && analysisConfigs.Count > 0)
                {
                    this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "模型名称", Name = "Name", FieldName = "Name", VisibleIndex = 0 });
                    this.gridView1.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn { Caption = "分析结果", Name = "Result", FieldName = "Result", VisibleIndex = 1 });
                }
            }
        }

        private void SysEmergencyLinkageRealtime_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isrun = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 1)
            {
                this.gridControl1.DataSource = realInfos;
            }
            else if (_sysEmergencyLinkageInfo != null && _sysEmergencyLinkageInfo.Type == 2)
            {
                this.gridControl1.DataSource = analysisConfig;
            }
            gridControl1.Refresh();
        }

    }

    public class DefInfoExtend
    {
        public string Point { get; set; }

        public string Wz { get; set; }

        public string DevName { get; set; }

        public string Ssz { get; set; }

        public string DataState { get; set; }

        public string State { get; set; }
    }

    public class AnalysisConfigExtend
    {
        public string Name { get; set; }

        public string Result { get; set; }
    }
}
