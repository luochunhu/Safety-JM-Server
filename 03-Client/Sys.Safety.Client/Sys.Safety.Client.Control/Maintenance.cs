using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ButtonPanel;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Control.Model;
using Sys.Safety.Client.Control.Properties;
using Sys.Safety.Request.StaionHistoryData;
using Sys.Safety.Request.StaionControlHistoryData;
using Sys.Safety.Request.NetworkModule;

namespace Sys.Safety.Client.Control
{
    public partial class Maintenance : XtraForm
    {
        private bool _ifLoop = true;

        private Thread _refThr = null;

        private readonly object _locker1 = new object();

        private readonly object _locker2 = new object();

        //List<GetSubstationHistoryRealDataByFzhTimeResponse> FiveHistoryRecord = new List<GetSubstationHistoryRealDataByFzhTimeResponse>();

        //List<GetStaionControlHistoryDataByByFzhTimeResponse> ControlHistoryRecord = new List<GetStaionControlHistoryDataByByFzhTimeResponse>();

        IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        List<Jc_DevInfo> devList = new List<Jc_DevInfo>();


        DataTable FiveHistoryRecordData = new DataTable();
        DataTable ControlHistoryRecordData = new DataTable();
        /// <summary>
        /// 5分钟历史数据
        /// </summary>
        private List<SubstationBindDto> _substationFiveHisDataBindDto = null;

        private List<SubstationBindDto> _substationConHisDataBindDto = null;

        public Maintenance()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 刷新分站5分钟历史数据列表
        /// </summary>
        private void RefSubstationFiveHisDataList(object param)
        {
            //var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据
            List<Jc_DefInfo> allSubstation = (List<Jc_DefInfo>)param;

            lock (_locker1)
            {
                if (_substationFiveHisDataBindDto == null) //第一次获取数据
                {
                    _substationFiveHisDataBindDto = new List<SubstationBindDto>();
                    foreach (var item in allSubstation)
                    {
                        var dto = new SubstationBindDto
                        {
                            Fzh = item.Fzh.ToString(),
                            DataLegacyCount = item.HistoryRealDataLegacyCount.ToString(),
                            DataState = item.HistoryRealDataState.ToString()
                        };
                        if (item.HistoryRealDataLegacyCount == -1)
                        {
                            dto.DataLegacyCountText = "获取中";
                        }
                        else
                        {
                            dto.DataLegacyCountText = item.HistoryRealDataLegacyCount.ToString();
                        }

                        if (item.HistoryRealDataState == 0)
                        {
                            dto.DataStateText = "未获取";
                        }
                        else if (item.HistoryRealDataState == 1 || item.HistoryRealDataState == 2)
                        {
                            dto.DataStateText = "获取中";
                        }
                        else if (item.HistoryRealDataState == 3)
                        {
                            dto.DataStateText = "已完成";
                        }
                        _substationFiveHisDataBindDto.Add(dto);
                    }

                    gridControlFiveHisTree.DataSource = _substationFiveHisDataBindDto; //绑定数据
                }
                else //非第一获取数据
                {
                    foreach (var item in _substationFiveHisDataBindDto)
                    {
                        var substation = allSubstation.FirstOrDefault(a => a.Fzh.ToString() == item.Fzh);
                        if (substation == null)
                        {
                            if (item.DataLegacyCount != "未知")
                            {
                                item.DataLegacyCount = "未知";
                                item.DataLegacyCountText = "未知";
                            }

                            if (item.DataState != "未知")
                            {
                                item.DataLegacyCount = "未知";
                                item.DataLegacyCountText = "未知";
                            }
                        }
                        else
                        {
                            if (item.DataLegacyCount != substation.HistoryRealDataLegacyCount.ToString())
                            {
                                item.DataLegacyCount = substation.HistoryRealDataLegacyCount.ToString();
                                if (substation.HistoryRealDataLegacyCount == -1)
                                {
                                    item.DataLegacyCountText = "获取中";
                                }
                                else
                                {
                                    item.DataLegacyCountText =
                                        substation.HistoryRealDataLegacyCount.ToString();
                                }
                            }

                            if (item.DataState != substation.HistoryRealDataState.ToString())
                            {
                                item.DataState = substation.HistoryRealDataState.ToString();
                                if (substation.HistoryRealDataState == 0)
                                {
                                    item.DataStateText = "未获取";
                                }
                                else if (substation.HistoryRealDataState == 1 || substation.HistoryRealDataState == 2)
                                {
                                    item.DataStateText = "获取中";
                                }
                                else if (substation.HistoryRealDataState == 3)
                                {
                                    item.DataStateText = "已完成";
                                }
                            }
                        }
                    }
                }
                gridControlFiveHisTree.RefreshDataSource();
            }
        }

        /// <summary>
        /// 刷新分站控制历史数据列表
        /// </summary>
        private void RefSubstationConHisDataList(object param)
        {
            //var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据
            List<Jc_DefInfo> allSubstation = (List<Jc_DefInfo>)param;

            lock (_locker2)
            {
                if (_substationConHisDataBindDto == null) //第一次获取数据
                {
                    _substationConHisDataBindDto = new List<SubstationBindDto>();
                    foreach (var item in allSubstation)
                    {
                        var dto = new SubstationBindDto
                        {
                            Fzh = item.Fzh.ToString(),
                            DataLegacyCount = item.HistoryControlLegacyCount.ToString(),
                            DataState = item.HistoryControlState.ToString()
                        };
                        if (item.HistoryControlLegacyCount == -1)
                        {
                            dto.DataLegacyCountText = "获取中";
                        }
                        else
                        {
                            dto.DataLegacyCountText = item.HistoryControlLegacyCount.ToString();
                        }

                        if (item.HistoryControlState == 0)
                        {
                            dto.DataStateText = "未获取";
                        }
                        else if (item.HistoryControlState == 1 || item.HistoryControlState == 2)
                        {
                            dto.DataStateText = "获取中";
                        }
                        else if (item.HistoryControlState == 3)
                        {
                            dto.DataStateText = "已完成";
                        }
                        _substationConHisDataBindDto.Add(dto);
                    }

                    gridControlConHisTree.DataSource = _substationConHisDataBindDto; //绑定数据
                }
                else //非第一获取数据
                {
                    foreach (var item in _substationConHisDataBindDto)
                    {
                        var substation = allSubstation.FirstOrDefault(a => a.Fzh.ToString() == item.Fzh);
                        if (substation == null)
                        {
                            if (item.DataLegacyCount != "未知")
                            {
                                item.DataLegacyCount = "未知";
                                item.DataLegacyCountText = "未知";
                            }

                            if (item.DataState != "未知")
                            {
                                item.DataLegacyCount = "未知";
                                item.DataLegacyCountText = "未知";
                            }
                        }
                        else
                        {
                            if (item.DataLegacyCount != substation.HistoryControlLegacyCount.ToString())
                            {
                                item.DataLegacyCount = substation.HistoryControlLegacyCount.ToString();
                                if (substation.HistoryControlLegacyCount == -1)
                                {
                                    item.DataLegacyCountText = "获取中";
                                }
                                else
                                {
                                    item.DataLegacyCountText =
                                        substation.HistoryControlLegacyCount.ToString();
                                }
                            }

                            if (item.DataState != substation.HistoryControlState.ToString())
                            {
                                item.DataState = substation.HistoryControlState.ToString();
                                if (substation.HistoryControlState == 0)
                                {
                                    item.DataStateText = "未获取";
                                }
                                else if (substation.HistoryControlState == 1 || substation.HistoryControlState == 2)
                                {
                                    item.DataStateText = "获取中";
                                }
                                else if (substation.HistoryControlState == 3)
                                {
                                    item.DataStateText = "已完成";
                                }
                            }
                        }
                    }
                }
                gridControlConHisTree.RefreshDataSource();
            }
        }

        /// <summary>
        /// 刷新历史记录列表
        /// </summary>
        private void RefFiveHistoryRecord(object result)
        {
            //根据分站号获取历史记录信息
            //var rows = gridControlSubstationView.GetSelectedRows();
            //var fzh = gridControlSubstationView.GetRowCellDisplayText(rows[0], "Fzh");
            List<GetSubstationHistoryRealDataByFzhTimeResponse> dataList = (List<GetSubstationHistoryRealDataByFzhTimeResponse>)result;
            dataList = dataList.OrderBy(a => a.SaveTime).ToList();
            foreach (GetSubstationHistoryRealDataByFzhTimeResponse data in dataList)
            {
                //DataRow[] dr = FiveHistoryRecordData.Select("Point='" + data.Point + "' and SaveTime='" + data.SaveTime.ToString("yyyy/MM/dd HH:mm:ss") + "'");
                //if (dr.Length == 0)//分站测点号和时间一样的记录无法正常显示   2017-11-03
                //{
                DataRow obj = FiveHistoryRecordData.NewRow();
                obj["Point"] = data.Point;
                obj["SaveTime"] = data.SaveTime.ToString("yyyy/MM/dd HH:mm:ss");
                obj["TypeName"] = data.TypeName;
                obj["StateName"] = data.StateName;
                obj["Location"] = data.Location;
                obj["DeviceTypeName"] = data.DeviceTypeName;
                obj["RealData"] = data.RealData;
                //如果是开关量显示0，1，2态对应的显示值
                Jc_DevInfo dev = devList.Find(a => a.DevModel == data.DeviceTypeName);
                if (dev != null && (dev.Type == 2 || dev.Type == 3))
                {
                    if (data.RealData == "1")
                    {
                        obj["RealData"] = dev.Xs2;
                    }
                    else if (data.RealData == "2")
                    {
                        obj["RealData"] = dev.Xs3;
                    }
                    else
                    {
                        obj["RealData"] = dev.Xs1;
                    }
                }
                obj["GradingAlarmLevel"] = data.GradingAlarmLevel;
                obj["Voltage"] = data.Voltage + "V";
                string feedBackState = "";
                switch (data.FeedBackState)
                {
                    case "0":
                        feedBackState = "有电";
                        break;
                    case "1":
                        feedBackState = "无电";
                        break;
                    default:
                        feedBackState = "";
                        break;
                }
                obj["FeedBackState"] = feedBackState;
                string feedState = "";
                switch (data.FeedState)
                {
                    case "1":
                        feedState = "馈电成功";
                        break;
                    case "2":
                        feedState = "馈电失败";
                        break;
                    case "3":
                        feedState = "复电成功";
                        break;
                    case "4":
                        feedState = "复电失败";
                        break;
                    default:
                        feedState = "";
                        break;
                }
                obj["FeedState"] = feedState;
                FiveHistoryRecordData.Rows.InsertAt(obj, 0);
                //}
            }
            //gridViewFiveHisData.FocusedRowHandle = 0;
            //gridControlFiveHisData.DataSource = FiveHistoryRecord;
        }

        private void RefControlHistoryRecord(object result)
        {
            List<GetStaionControlHistoryDataByByFzhTimeResponse> dataList = (List<GetStaionControlHistoryDataByByFzhTimeResponse>)result;
            dataList = dataList.OrderBy(a => a.SaveTime).ToList();
            foreach (GetStaionControlHistoryDataByByFzhTimeResponse data in dataList)
            {
                //DataRow[] dr = ControlHistoryRecordData.Select("Point='" + data.Point + "' and SaveTime='" + data.SaveTime.ToString("yyyy/MM/dd HH:mm:ss") + "'");
                //if (dr.Length == 0)//分站测点号和时间一样的记录无法正常显示   2017-11-03
                //{
                DataRow obj = ControlHistoryRecordData.NewRow();
                obj["Point"] = data.Point;
                obj["SaveTime"] = data.SaveTime.ToString("yyyy/MM/dd HH:mm:ss");
                obj["TypeName"] = data.TypeName;
                obj["StateName"] = data.StateName;
                obj["Location"] = data.Location;
                obj["Value"] = data.Value;
                obj[6] = data.ControlDeviceConvert;

                ControlHistoryRecordData.Rows.InsertAt(obj, 0);
                //}
            }
            //gridViewConHisData.FocusedRowHandle = 0;
            //gridContrlConHisData.DataSource = ControlHistoryRecord;
        }

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maintenance_Load(object sender, EventArgs e)
        {
            try
            {
                FiveHistoryRecordData = new DataTable();
                FiveHistoryRecordData.Columns.Add("Point");
                FiveHistoryRecordData.Columns.Add("SaveTime");
                FiveHistoryRecordData.Columns.Add("TypeName");
                FiveHistoryRecordData.Columns.Add("StateName");
                FiveHistoryRecordData.Columns.Add("Location");
                FiveHistoryRecordData.Columns.Add("DeviceTypeName");
                FiveHistoryRecordData.Columns.Add("RealData");
                FiveHistoryRecordData.Columns.Add("GradingAlarmLevel");
                FiveHistoryRecordData.Columns.Add("Voltage");
                FiveHistoryRecordData.Columns.Add("FeedBackState");
                FiveHistoryRecordData.Columns.Add("FeedState");
                gridControlFiveHisData.DataSource = FiveHistoryRecordData;

                ControlHistoryRecordData = new DataTable();
                ControlHistoryRecordData.Columns.Add("Point");
                ControlHistoryRecordData.Columns.Add("SaveTime");
                ControlHistoryRecordData.Columns.Add("TypeName");
                ControlHistoryRecordData.Columns.Add("StateName");
                ControlHistoryRecordData.Columns.Add("Location");
                ControlHistoryRecordData.Columns.Add("Value");
                ControlHistoryRecordData.Columns.Add("ControlDeviceConvert");
                gridContrlConHisData.DataSource = ControlHistoryRecordData;


                var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据
                RefSubstationFiveHisDataList(allSubstation);
                RefSubstationConHisDataList(allSubstation);
                if (_refThr == null)
                {
                    _ifLoop = true;
                    _refThr = new Thread(RefThrFun)
                    {
                        IsBackground = true
                    };
                    //_refThr = new Thread(RefThrFun);
                    _refThr.Start();
                }

                LoadKhAndDzh();

                StationUpdate su = new StationUpdate();
                su.Dock = DockStyle.Fill;
                su.TopLevel = false;
                su.TopMost = true;
                xtraTabPage2.Controls.Add(su);
                su.Show();


                devList = deviceDefineService.GetAllDeviceDefineCache().Data;

            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //窗体关闭时
        private void Maintenance_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (
                //    XtraMessageBox.Show("窗体关闭后将取消所有分站历史数据的获取，确认要执行该操作吗？", "提示", MessageBoxButtons.YesNo,
                //        MessageBoxIcon.Question) == DialogResult.Yes)
                //{
                //取消所有数据获取
                var lisSc = new List<StationControlItem>();     //5分钟历史数据和控制历史数据共用
                lock (_locker1)
                {
                    foreach (var item in _substationFiveHisDataBindDto)
                    {
                        lisSc.Add(new StationControlItem()
                        {
                            fzh = Convert.ToUInt16(item.Fzh),
                            controlType = 2
                        });
                    }
                }
                ControlInterfaceFuction.SendQueryHistoryRealDataRequest(lisSc);
                ControlInterfaceFuction.SendQueryHistoryControlRequest(lisSc);

                //if (_refThr != null && _refThr.IsAlive == true)
                //{
                //    _refThr.Abort();
                //}
                _ifLoop = false;
                //注释，造成界面卡，就不用调用此段代码  20170811
                //if (_refThr != null && _refThr.IsAlive == true)
                //{
                //    _refThr.Join();
                //}                    
                //Dispose();
                //}
                //else
                //{
                //    e.Cancel = true;
                //}
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //5分钟历史数据操作按钮事件
        private void buttonFiveHisData_Click(object sender, EventArgs e)
        {
            try
            {
                var rowHandle = gridViewFiveHisTree.FocusedRowHandle;
                var fzh = gridViewFiveHisTree.GetRowCellValue(rowHandle, "Fzh");
                SubstationBindDto sbd;
                lock (_locker1)
                {
                    sbd = _substationFiveHisDataBindDto.FirstOrDefault(a => a.Fzh == (string)fzh);
                }

                if (sbd == null)
                {
                    XtraMessageBox.Show("未能从列表数据中找到该分站。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //发送获取数据命令
                byte conType;
                if (sbd.DataState == "0" || sbd.DataState == "3")
                {
                    conType = 1;
                }
                else
                {
                    conType = 0;
                }

                var req = new List<StationControlItem>()
                {
                    new StationControlItem()
                    {
                        controlType = conType,
                        fzh = Convert.ToUInt16(fzh)
                    }
                };
                ControlInterfaceFuction.SendQueryHistoryRealDataRequest(req);

                var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据
                RefSubstationFiveHisDataList(allSubstation);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //控制历史数据操作按钮事件
        private void buttonControlHisData_Click(object sender, EventArgs e)
        {
            try
            {
                var rowHandle = gridViewConHisTree.FocusedRowHandle;
                var fzh = gridViewConHisTree.GetRowCellValue(rowHandle, "Fzh");
                SubstationBindDto sbd;
                lock (_locker2)
                {
                    sbd = _substationConHisDataBindDto.FirstOrDefault(a => a.Fzh == (string)fzh);
                }

                if (sbd == null)
                {
                    XtraMessageBox.Show("未能从列表数据中找到该分站。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //发送获取数据命令
                byte conType;
                if (sbd.DataState == "0" || sbd.DataState == "3")
                {
                    conType = 1;
                }
                else
                {
                    conType = 0;
                }

                var req = new List<StationControlItem>()
                {
                    new StationControlItem()
                    {
                        controlType = conType,
                        fzh = Convert.ToUInt16(fzh)
                    }
                };
                ControlInterfaceFuction.SendQueryHistoryControlRequest(req);

                var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据
                RefSubstationConHisDataList(allSubstation);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dateTimePickerFiveHisData_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                FiveHistoryRecordData.Rows.Clear();


                var result = GetSubstationHistoryRealDataByFzhTime();
                RefFiveHistoryRecord(result);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void dateTimePickerConHisData_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ControlHistoryRecordData.Rows.Clear();//刷新前先清除原来的数据  20171114
                //根据分站号获取历史记录信息
                var result1 = GetStaionControlHistoryDataByByFzhTime();
                RefControlHistoryRecord(result1);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridViewFiveHisTree_FocusedRowChanged(object sender,
            DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                FiveHistoryRecordData.Rows.Clear();

                var result = GetSubstationHistoryRealDataByFzhTime();
                RefFiveHistoryRecord(result);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridViewConHisTree_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var result1 = GetStaionControlHistoryDataByByFzhTime();
                RefControlHistoryRecord(result1);
            }
            catch (Exception exc)
            {
                XtraMessageBox.Show(exc.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public List<GetSubstationHistoryRealDataByFzhTimeResponse> GetSubstationHistoryRealDataByFzhTime()
        {
            List<GetSubstationHistoryRealDataByFzhTimeResponse> result = new List<GetSubstationHistoryRealDataByFzhTimeResponse>();
            try
            {
                var rowHandle = gridViewFiveHisTree.FocusedRowHandle;
                var fzh = gridViewFiveHisTree.GetRowCellValue(rowHandle, "Fzh");
                var date = dateTimePickerFiveHisData.Value;
                result = ControlInterfaceFuction.GetSubstationHistoryRealDataByFzhTime(fzh.ToString(), date);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return result;
        }

        public List<GetStaionControlHistoryDataByByFzhTimeResponse> GetStaionControlHistoryDataByByFzhTime()
        {
            List<GetStaionControlHistoryDataByByFzhTimeResponse> result1 = new List<GetStaionControlHistoryDataByByFzhTimeResponse>();
            try
            {
                //根据分站号获取历史记录信息
                var rowHandle1 = gridViewConHisTree.FocusedRowHandle;
                var fzh1 = gridViewConHisTree.GetRowCellValue(rowHandle1, "Fzh");
                var date1 = dateTimePickerConHisData.Value;
                result1 = ControlInterfaceFuction.GetStaionControlHistoryDataByByFzhTime(fzh1.ToString(), date1);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return result1;
        }
        private void RefThrFun()
        {
            while (_ifLoop)
            {
                try
                {
                    Thread.Sleep(5000);
                    var allSubstation = ControlInterfaceFuction.GetAllSubstation(); //获取所有分站缓存数据

                    //刷新5分钟历史数据
                    if (this.InvokeRequired)
                    {
                        Action<object> act = RefSubstationFiveHisDataList;
                        BeginInvoke(act, allSubstation);
                    }

                    //var result = GetSubstationHistoryRealDataByFzhTime();
                    //if (this.InvokeRequired)
                    //{
                    //    Action<object> act2 = RefFiveHistoryRecord;
                    //    BeginInvoke(act2, result);
                    //}

                    //刷新控制历史数据
                    if (this.InvokeRequired)
                    {
                        Action<object> act3 = RefSubstationConHisDataList;
                        BeginInvoke(act3, allSubstation);
                    }

                    //var result1 = GetStaionControlHistoryDataByByFzhTime();
                    //if (this.InvokeRequired)
                    //{
                    //    Action<object> act4 = RefControlHistoryRecord;
                    //    BeginInvoke(act4, result1);
                    //}
                }
                catch (Exception exc)
                {
                    LogHelper.Error(exc);
                }
            }
        }

        private void LoadKhAndDzh()
        {
            cmb_kh.Items.Add("全部");
            cmb_kh1.Items.Add("全部");
            for (int i = 0; i < 24; i++)
            {
                cmb_kh.Items.Add(i + 1);
                cmb_kh1.Items.Add(i + 1);
            }

            cmb_dzh.Items.Add("全部");
            cmb_dzh1.Items.Add("全部");
            for (int i = 0; i < 4; i++)
            {
                cmb_dzh.Items.Add(i + 1);
                cmb_dzh1.Items.Add(i + 1);
            }
        }

        private void btn_fileter_Click(object sender, EventArgs e)
        {
            FiveHistoryRecordData.Rows.Clear();
            var result = GetSubstationHistoryRealDataByFzhTime();
            if (cmb_kh.SelectedIndex <= 0 && cmb_dzh.SelectedIndex <= 0)
            {

            }
            else if (cmb_kh.SelectedIndex <= 0 && cmb_dzh.SelectedIndex > 0)
            {
                result = result.Where(a => a.Dzh == cmb_dzh.SelectedIndex).ToList();
            }
            else if (cmb_kh.SelectedIndex > 0 && cmb_dzh.SelectedIndex <= 0)
            {
                result = result.Where(a => a.Kh == cmb_kh.SelectedIndex).ToList();
            }
            else if (cmb_kh.SelectedIndex > 0 && cmb_dzh.SelectedIndex > 0)
            {
                result = result.Where(a => a.Kh == cmb_kh.SelectedIndex && a.Dzh == cmb_dzh.SelectedIndex).ToList();
            }

            RefFiveHistoryRecord(result);
        }

        private void btn_fileter1_Click(object sender, EventArgs e)
        {
            ControlHistoryRecordData.Rows.Clear();

            var result = GetStaionControlHistoryDataByByFzhTime();

            if (cmb_kh1.SelectedIndex <= 0 && cmb_dzh1.SelectedIndex <= 0)
            {

            }
            else if (cmb_kh1.SelectedIndex <= 0 && cmb_dzh1.SelectedIndex > 0)
            {
                result = result.Where(a => a.Kh == cmb_dzh1.SelectedIndex.ToString()).ToList();
            }
            else if (cmb_kh1.SelectedIndex > 0 && cmb_dzh1.SelectedIndex <= 0)
            {
                result = result.Where(a => a.Kh == cmb_kh1.SelectedIndex.ToString()).ToList();
            }
            else if (cmb_kh1.SelectedIndex > 0 && cmb_dzh1.SelectedIndex > 0)
            {
                result = result.Where(a => a.Kh == cmb_kh1.SelectedIndex.ToString() && a.Dzh == cmb_dzh1.SelectedIndex.ToString()).ToList();
            }
            RefControlHistoryRecord(result);
        }

        private void btn_sendAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                INetworkModuleService networkModuleService = ServiceFactory.Create<INetworkModuleService>();
                TestAlarmRequest testAlarmRequest = new TestAlarmRequest();
                testAlarmRequest.macItems = new List<Jc_MacInfo>();
                testAlarmRequest.testAlarmFlag = 1;
                networkModuleService.TestAlarm(testAlarmRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，原因：" + ex.Message);
            }
        }

        private void btn_cancleAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                INetworkModuleService networkModuleService = ServiceFactory.Create<INetworkModuleService>();
                TestAlarmRequest testAlarmRequest = new TestAlarmRequest();
                testAlarmRequest.macItems = new List<Jc_MacInfo>();
                testAlarmRequest.testAlarmFlag = 0;
                networkModuleService.TestAlarm(testAlarmRequest);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，原因：" + ex.Message);
            }
        }
    }

    /// <summary>
    /// grid绑定类
    /// </summary>
    public class SubstationBindDto
    {
        /// <summary>
        /// 分站号
        /// </summary>
        public string Fzh { get; set; }

        /// <summary>
        /// 数据获取状态
        /// </summary>
        public string DataState { get; set; }

        /// <summary>
        /// 数据获取状态文本
        /// </summary>
        public string DataStateText { get; set; }

        /// <summary>
        /// 剩余数据条数
        /// </summary>
        public string DataLegacyCount { get; set; }

        /// <summary>
        /// 剩余数据条数文本
        /// </summary>
        public string DataLegacyCountText { get; set; }

    }
}