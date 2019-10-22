using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic.Framework.Logging;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Alarm
{
    public partial class SensorCalibration : DevExpress.XtraEditors.XtraForm
    {
        private IEnumcodeService enumcodeService = ServiceFactory.Create<IEnumcodeService>();

        private IConfigService _configService = ServiceFactory.Create<IConfigService>();
        private IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();
        private IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        private IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();

        private DateTime dtmLast = DateTime.Now;
        /// <summary>
        /// 当前设备类型定义缓存
        /// </summary>
        List<Jc_DevInfo> deviceDefineList = new List<Jc_DevInfo>();
        /// <summary>
        /// 测点定义缓存
        /// </summary>
        List<Jc_DefInfo> pointDefineList = new List<Jc_DefInfo>();
        /// <summary>
        /// 实时数据列表
        /// </summary>
        List<RealDataDataInfo> realDataList = new List<RealDataDataInfo>();
        public SensorCalibration()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //方法1
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "传感器未标校记录";
            fileDialog.FileName = "传感器未标校记录";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl1.ExportToXls(fileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 服务端上下文中设备定义是否发生变化（通过对比客户端设备定义时间和服务端上下文设备定义时间来判断）
        /// </summary>
        /// <returns></returns>
        public bool bIsDevChange()
        {
            bool b = false;
            try
            {
                string TempTime = ClientAlarmServer.GetDevDefineChangeDatetime();
                if (!string.IsNullOrEmpty(TempTime))
                {
                    DateTime dtmTemp = dtmLast;
                    DateTime.TryParse(TempTime, out dtmTemp);

                    if (dtmLast != dtmTemp)//20151028 txy
                    {
                        b = true;
                        dtmLast = dtmTemp;
                    }
                }
                else
                {
                    b = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("bIsDevChange-发生异常 " + ex.Message);
                b = false;
            }
            return b;
        }
        private void GetCalibrationDue()
        {
            try
            {
                List<EnumcodeInfo> enumcodeInfos = new List<EnumcodeInfo>();
                EnumcodeInfo enumcodeInfo;
                EnumcodeGetByEnumTypeIDRequest enumcoderequest = new EnumcodeGetByEnumTypeIDRequest();
                enumcoderequest.EnumTypeId = "3";
                var result = enumcodeService.GetEnumcodeByEnumTypeID(enumcoderequest);
                if (result.IsSuccess & result.Data != null)
                {
                    enumcodeInfos = result.Data;
                }


                //获取设备类型及测点定义基础数据
                if (deviceDefineList.Count == 0 || bIsDevChange())
                {
                    deviceDefineList = deviceDefineService.GetAllDeviceDefineCache().Data;
                    pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
                }
                GetRealDataRequest request = new GetRealDataRequest();
                realDataList = realMessageService.GetRealData(request).Data;

                //标效及传感器到期提醒

                //查询近90天内的标效记录(统计前一天，及之前90天的记录)
                DateTime startTime = DateTime.Parse(DateTime.Now.AddDays(-91).ToShortDateString());
                DateTime endTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");
                DataTable calibrationRecord = ClientAlarmServer.GetCalibrationRecord(startTime, endTime);
                //按设备类型循环获取未标校、到期的传感器数量
                int NoCalibrationCount = 0;
                int DueCount = 0;
                foreach (Jc_DevInfo dev in deviceDefineList)
                {
                    //2018.5.9 by AI
                    enumcodeInfo = enumcodeInfos.FirstOrDefault(a => a.LngEnumValue == dev.Bz4);
                    if (enumcodeInfo == null)
                    {
                        //LogHelper.Error("【GetCalibrationDue】枚举中未找到设备：" + dev.Bz4);
                        continue;
                    }                  
                    int tempCalibrationTime = 0;
                    int.TryParse(dev.Pl4.ToString(), out tempCalibrationTime);//标校周期
                    List<Jc_DefInfo> pointList = pointDefineList.FindAll(a => a.Devid == dev.Devid);
                    if (tempCalibrationTime > 0)
                    {
                        #region 标效周期报警
                        //计算当前设备类型下面的传感器是否到标效期                    
                        foreach (Jc_DefInfo def in pointList)
                        {
                            DataRow[] dr = calibrationRecord.Select("point='" + def.Point + "' and stime<='" + endTime + "'", "stime desc");
                            if (dr.Length > 0)
                            {
                                TimeSpan ts = endTime - DateTime.Parse(dr[0]["stime"].ToString());
                                if ((int)ts.TotalDays >= tempCalibrationTime)//如果上一次标校记录时间超过了设置的标校时间周期，则记入未标校数量
                                {
                                    NoCalibrationCount++;

                                    SensorCalibrationInfo tempSensorCalibrationInfo = new SensorCalibrationInfo();
                                    tempSensorCalibrationInfo.Point = def.Point;
                                    tempSensorCalibrationInfo.Position = def.Wz;
                                    tempSensorCalibrationInfo.DevName = def.DevName;
                                    tempSensorCalibrationInfo.SetCalibrationTime = tempCalibrationTime.ToString();
                                    tempSensorCalibrationInfo.LastCalibrationTime = dr[0]["stime"].ToString();
                                    tempSensorCalibrationInfo.CalibrationDays = ((int)(ts.TotalDays)).ToString();
                                    tempSensorCalibrationInfo.id = dr[0]["id"].ToString();
                                    tempSensorCalibrationInfo.pointid = def.PointID;
                                    if (ClientAlarmServer.sensorCalibrationInfoList.Find(a => a.Point == tempSensorCalibrationInfo.Point) == null)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.Add(tempSensorCalibrationInfo);
                                    }
                                }
                                else
                                {
                                    int index = ClientAlarmServer.sensorCalibrationInfoList.FindIndex(a => a.Point == def.Point);
                                    if (index >= 0)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.RemoveAt(index);
                                    }
                                }
                            }
                            else//未找到标校记录，则直接记入未标校
                            {

                                NoCalibrationCount++;

                                SensorCalibrationInfo tempSensorCalibrationInfo = new SensorCalibrationInfo();
                                tempSensorCalibrationInfo.Point = def.Point;
                                tempSensorCalibrationInfo.Position = def.Wz;
                                tempSensorCalibrationInfo.DevName = def.DevName;
                                tempSensorCalibrationInfo.SetCalibrationTime = tempCalibrationTime.ToString();
                                tempSensorCalibrationInfo.LastCalibrationTime = "未记录";
                                tempSensorCalibrationInfo.CalibrationDays = "-";
                                tempSensorCalibrationInfo.pointid = def.PointID;
                                //tempSensorCalibrationInfo.id = dr[0]["id"].ToString();
                                if (ClientAlarmServer.sensorCalibrationInfoList.Find(a => a.Point == tempSensorCalibrationInfo.Point) == null)
                                {
                                    ClientAlarmServer.sensorCalibrationInfoList.Add(tempSensorCalibrationInfo);
                                }
                            }
                        }
                        #endregion
                    }
                }
                //清除已删除的测点
                foreach (SensorCalibrationInfo tempSensorCalibrationInfo in ClientAlarmServer.sensorCalibrationInfoList)
                {
                    if (pointDefineList.FindAll(a => a.Point == tempSensorCalibrationInfo.Point).Count == 0)//如果测点定义缓存中不存在测点，则清除未标校集合中的记录
                    {
                        ClientAlarmServer.sensorCalibrationInfoList.Remove(tempSensorCalibrationInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void SensorCalibration_Load(object sender, EventArgs e)
        {
            if (ClientAlarmServer.sensorCalibrationInfoList == null || ClientAlarmServer.sensorCalibrationInfoList.Count==0)
            {
                GetCalibrationDue();
            }
            gridControl1.DataSource = ClientAlarmServer.sensorCalibrationInfoList;          
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    SensorCalibrationDetail dig = new SensorCalibrationDetail();
                    dig.ShowDialog();
                    if (dig.isSave)
                    {
                        string csStr = dig.csStr;

                        //var alarmId = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id");
                        //if (alarmId != null)
                        //{

                        //    ClientAlarmServer.UpdateCalibrationRecord(alarmId.ToString(), csStr);
                        //}
                        //else
                        //{
                            Jc_BxexInfo bxInfo = new Jc_BxexInfo();

                            bxInfo.ID = IdHelper.CreateLongId().ToString();
                            bxInfo.PointID = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "pointid").ToString();
                            bxInfo.Point = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Point").ToString();
                            ClientItem clientItem = new ClientItem();
                            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                            {
                                clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                            }
                            bxInfo.Name = clientItem.UserName;
                            bxInfo.Stime = dig.stime;
                            bxInfo.Etime = dig.etime;
                            bxInfo.Cx = (int)(dig.etime - dig.stime).TotalSeconds;
                            bxInfo.Zdz = 0;
                            bxInfo.Zxz = 0;
                            bxInfo.Pjz = 0;

                            bxInfo.Zdztime = dig.etime;
                            bxInfo.Zxztime = dig.etime;
                            bxInfo.Bxzt = 3;
                            bxInfo.Cs = dig.csStr;

                            ClientAlarmServer.InsertCalibrationRecord(bxInfo);
                       // }
                    }
                }
            }
        }
    }
}
