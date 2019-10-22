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
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Controls;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Client.Alarm
{
    public partial class SensorDefineError : DevExpress.XtraEditors.XtraForm
    {
        private IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        private IConfigService _ConfigService = ServiceFactory.Create<IConfigService>();
        public SensorDefineError()
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
            fileDialog.Title = "传感器与中心站定义不匹配记录";
            fileDialog.FileName = "传感器与中心站定义不匹配记录";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                gridControl1.ExportToXls(fileDialog.FileName);
                DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SensorCalibration_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PointId");
                dt.Columns.Add("Point");
                dt.Columns.Add("Position");
                dt.Columns.Add("DevName");
                dt.Columns.Add("UpAarmValue");
                dt.Columns.Add("DownAarmValue");
                dt.Columns.Add("UpDdValue");
                dt.Columns.Add("DownDdValue");
                dt.Columns.Add("UpHfValue");
                dt.Columns.Add("DownHfValue");
                dt.Columns.Add("LC2");
                dt.Columns.Add("SeniorGradeAlarmValue");
                dt.Columns.Add("SeniorGradeTimeValue");
                //object[] obj = new object[13];
                //obj[0] = "1";
                //obj[1] = "001A010";
                //obj[2] = "10101工作面甲烷";
                //obj[3] = "甲烷传感器";
                //obj[4] = "1.0/0.9";
                //obj[5] = "0/0";
                //obj[6] = "1.5/1.4";
                //obj[7] = "0/0";
                //obj[8] = "0.9/0.9";
                //obj[9] = "0/0";
                //obj[10] = "40/40";
                //obj[11] = "0.5/0.8/1.0/1.5";
                //obj[12] = "1/2/3/4";
                //dt.Rows.Add(obj);

                PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
                PointDefineRequest.DevpropertID = 1;
                List<Jc_DefInfo> defList = pointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest).Data;
                //defList = defList.FindAll(a => a.Z2 != a.DeviceInfoItem.UpAarmValue || a.Z3 != a.DeviceInfoItem.UpDdValue || a.Z4 != a.DeviceInfoItem.UpHfValue
                //    || a.Z6 != a.DeviceInfoItem.DownAarmValue || a.Z7 != a.DeviceInfoItem.DownDdValue || a.Z8 != a.DeviceInfoItem.DownHfValue);
                List<Jc_DefInfo> allSensorDefineNotMatchList = new List<Jc_DefInfo>();
                foreach (Jc_DefInfo def in defList)
                {
                    if (def.Z2 != def.DeviceInfoItem.UpAarmValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (def.Z3 != def.DeviceInfoItem.UpDdValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (def.Z4 != def.DeviceInfoItem.UpHfValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (def.Z6 != def.DeviceInfoItem.DownAarmValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (def.Z7 != def.DeviceInfoItem.DownDdValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (def.Z8 != def.DeviceInfoItem.DownHfValue)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    string[] GradingAlarmLevel = def.Bz8.Split(',');
                    if (GradingAlarmLevel.Length < 4)
                    {
                        GradingAlarmLevel = new string[4] { "0", "0", "0", "0" };
                    }
                    if (float.Parse(GradingAlarmLevel[0]) != def.DeviceInfoItem.SeniorGradeAlarmValue1)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmLevel[1]) != def.DeviceInfoItem.SeniorGradeAlarmValue2)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmLevel[2]) != def.DeviceInfoItem.SeniorGradeAlarmValue3)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmLevel[3]) != def.DeviceInfoItem.SeniorGradeAlarmValue4)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    string[] GradingAlarmTime = def.Bz9.Split(',');
                    if (GradingAlarmTime.Length < 4)
                    {
                        GradingAlarmTime = new string[4] { "0", "0", "0", "0" };
                    }
                    if (float.Parse(GradingAlarmTime[0]) != def.DeviceInfoItem.SeniorGradeTimeValue1)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmTime[1]) != def.DeviceInfoItem.SeniorGradeTimeValue2)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmTime[2]) != def.DeviceInfoItem.SeniorGradeTimeValue3)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                    if (float.Parse(GradingAlarmTime[3]) != def.DeviceInfoItem.SeniorGradeTimeValue4)
                    {
                        allSensorDefineNotMatchList.Add(def);
                    }
                }
                foreach (Jc_DefInfo def in allSensorDefineNotMatchList)
                {
                    object[] obj = new object[13];
                    obj[0] = def.PointID;
                    obj[1] = def.Point;
                    obj[2] = def.Wz;
                    obj[3] = def.DevName;
                    obj[4] = def.Z2 + "/" + def.DeviceInfoItem.UpAarmValue;
                    obj[5] = def.Z6 + "/" + def.DeviceInfoItem.DownAarmValue;
                    obj[6] = def.Z3 + "/" + def.DeviceInfoItem.UpDdValue;
                    obj[7] = def.Z7 + "/" + def.DeviceInfoItem.DownDdValue;
                    obj[8] = def.Z4 + "/" + def.DeviceInfoItem.UpHfValue;
                    obj[9] = def.Z8 + "/" + def.DeviceInfoItem.DownHfValue;
                    obj[10] = "";
                    obj[11] = def.Bz8 + "/" + def.DeviceInfoItem.SeniorGradeAlarmValue1 + "," + def.DeviceInfoItem.SeniorGradeAlarmValue2
                        + "," + def.DeviceInfoItem.SeniorGradeAlarmValue3 + "," + def.DeviceInfoItem.SeniorGradeAlarmValue4;
                    obj[12] = def.Bz9 + "/" + def.DeviceInfoItem.SeniorGradeTimeValue1 + "," + def.DeviceInfoItem.SeniorGradeTimeValue2
                        + "," + def.DeviceInfoItem.SeniorGradeTimeValue3 + "," + def.DeviceInfoItem.SeniorGradeTimeValue4;
                    dt.Rows.Add(obj);
                }

                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }

        }



        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int rowhandle = gridView1.FocusedRowHandle;
                DataRow dr = gridView1.GetDataRow(rowhandle);
                PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
                PointDefineRequest.Point = dr["Point"].ToString().Substring(0, 3) + "0000";
                Jc_DefInfo def = pointDefineService.GetPointDefineCacheByPoint(PointDefineRequest).Data;
                Dictionary<string, Dictionary<string, object>> PointItemsList = new Dictionary<string, Dictionary<string, object>>();             
                Dictionary<string, object> PointItems = new Dictionary<string, object>();
                PointItems.Add("DefIsInit", true);
                PointItemsList.Add(def.PointID, PointItems);

                //调用服务端批量更新缓存业务来更新，修改为更新定义保存之后的是否发初始化标记，并保存巡检调用驱动预处理发初始化  20170720            
                DefineCacheBatchUpdatePropertiesRequest PointDefineUpdateRequest = new DefineCacheBatchUpdatePropertiesRequest();
                PointDefineUpdateRequest.PointItems = PointItemsList;
                pointDefineService.BatchUpdatePointDefineInfo(PointDefineUpdateRequest);

                _ConfigService.SaveInspection();//保存巡检
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int rowhandle = gridView1.FocusedRowHandle;
                DataRow dr = gridView1.GetDataRow(rowhandle);
                PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
                PointDefineRequest.Point = dr["Point"].ToString();
                Jc_DefInfo def = pointDefineService.GetPointDefineCacheByPoint(PointDefineRequest).Data;
                def.Z2 = def.DeviceInfoItem.UpAarmValue;
                def.Z3 = def.DeviceInfoItem.UpDdValue;
                def.Z4 = def.DeviceInfoItem.UpHfValue;
                def.Z6 = def.DeviceInfoItem.DownAarmValue;
                def.Z7 = def.DeviceInfoItem.DownDdValue;
                def.Z8 = def.DeviceInfoItem.DownHfValue;
                def.Bz8 = def.DeviceInfoItem.SeniorGradeAlarmValue1 + "," + def.DeviceInfoItem.SeniorGradeAlarmValue2
                    + "," + def.DeviceInfoItem.SeniorGradeAlarmValue3 + "," + def.DeviceInfoItem.SeniorGradeAlarmValue4;
                def.Bz9 = def.DeviceInfoItem.SeniorGradeTimeValue1 + "," + def.DeviceInfoItem.SeniorGradeTimeValue2
                  + "," + def.DeviceInfoItem.SeniorGradeTimeValue3 + "," + def.DeviceInfoItem.SeniorGradeTimeValue4;
                PointDefineUpdateRequest PointDefineUpdateRequest = new PointDefineUpdateRequest();
                PointDefineUpdateRequest.PointDefineInfo = def;
                pointDefineService.UpdatePointDefine(PointDefineUpdateRequest);//修改测点的基础属性

                _ConfigService.SaveInspection();//保存巡检
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }


    }
}
