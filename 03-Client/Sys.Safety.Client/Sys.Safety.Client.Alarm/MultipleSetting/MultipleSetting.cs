using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.Request.JC_Multiplesetting;
using Sys.Safety.Request.Setting;
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

namespace Sys.Safety.Client.Alarm.MultipleSetting
{
    public partial class MultipleSetting : XtraForm
    {
        private IJC_MultiplesettingService multiplesettingService = ServiceFactory.Create<IJC_MultiplesettingService>();
        private IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        List<Jc_DevInfo> deviceInfos = new List<Jc_DevInfo>();
        List<JC_MultiplesettingInfo> multiplesettingList = null;
        private ISettingService settingService = ServiceFactory.Create<ISettingService>();
        public MultipleSetting()
        {
            InitializeComponent();
        }

        private void MultipleSetting_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        public void LoadData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("devid");
            dt.Columns.Add("devName");
            dt.Columns.Add("MultipleText");
            //加载已定义倍数关系
            DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest = new DeviceDefineGetByDevpropertIDRequest();
            DeviceDefineRequest.DevpropertID = 1;
            deviceInfos = deviceDefineService.GetDeviceDefineCacheByDevpropertID(DeviceDefineRequest).Data;
            multiplesettingList = multiplesettingService.GetAllMultiplesettingList().Data;
            if (multiplesettingList != null)
            {
                foreach (JC_MultiplesettingInfo multiplesetting in multiplesettingList)
                {
                    object[] obj = new object[dt.Columns.Count];
                    obj[0] = multiplesetting.Devid;
                    Jc_DevInfo tempDev = deviceInfos.Find(a => a.Devid == multiplesetting.Devid);
                    obj[1] = "";
                    if (tempDev != null)
                    {
                        obj[1] = tempDev.Name;
                    }
                    obj[2] = "";
                    string multipleTextString = "";
                    string[] multipleTextArray = multiplesetting.MultipleText.Split('|');
                    for (int i = 0; i < multipleTextArray.Length; i++)
                    {
                        string[] tempsonmultipleTextArr = multipleTextArray[i].Split(',');
                        if (float.Parse(tempsonmultipleTextArr[2]) != 0)
                        {
                            multipleTextString += string.Format("条件{3}：范围({0}至{1})，倍数：{2}倍)\r\n",
                                tempsonmultipleTextArr[0], tempsonmultipleTextArr[1], tempsonmultipleTextArr[2], (i + 1).ToString());
                        }
                        else
                        {
                            multipleTextString += string.Format("条件{0}：未设置\r\n", (i + 1).ToString());
                        }
                    }
                    obj[2] = multipleTextString;
                    dt.Rows.Add(obj);
                }
            }
            gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 添加倍数关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MultipleSettingEdit multipleSettingEdit = new MultipleSettingEdit(deviceInfos, this);
            multipleSettingEdit.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string devid = this.gridView1.GetRowCellValue(selectedHandle, "devid").ToString();
                if (multiplesettingList != null)
                {
                    JC_MultiplesettingInfo editMultiplesetting = multiplesettingList.Find(a => a.Devid == devid);
                    if (editMultiplesetting != null)
                    {
                        MultipleSettingEdit multipleSettingEdit = new MultipleSettingEdit(editMultiplesetting, deviceInfos, this);
                        multipleSettingEdit.ShowDialog();
                    }
                }
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("删除不可恢复，是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int selectedHandle;
                selectedHandle = this.gridView1.GetSelectedRows()[0];
                if (selectedHandle >= 0)
                {
                    string devid = this.gridView1.GetRowCellValue(selectedHandle, "devid").ToString();
                    if (multiplesettingList != null)
                    {
                        JC_MultiplesettingInfo editMultiplesetting = multiplesettingList.Find(a => a.Devid == devid);
                        if (editMultiplesetting != null)
                        {
                            JC_MultiplesettingDeleteRequest multiplesettingrequest = new JC_MultiplesettingDeleteRequest();
                            multiplesettingrequest.Id = editMultiplesetting.Id;
                            multiplesettingService.DeleteMultiplesetting(multiplesettingrequest);
                            LoadData();
                        }
                    }
                }
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            int selectedHandle;
            selectedHandle = this.gridView1.GetSelectedRows()[0];
            if (selectedHandle >= 0)
            {
                string devid = this.gridView1.GetRowCellValue(selectedHandle, "devid").ToString();
                if (multiplesettingList != null)
                {
                    JC_MultiplesettingInfo editMultiplesetting = multiplesettingList.Find(a => a.Devid == devid);
                    if (editMultiplesetting != null)
                    {
                        MultipleSettingEdit multipleSettingEdit = new MultipleSettingEdit(editMultiplesetting, deviceInfos, this);
                        multipleSettingEdit.ShowDialog();
                    }
                }
            }
        }

        private void MultipleSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GetSettingByKeyRequest settingrequest = new GetSettingByKeyRequest();
                settingrequest.StrKey = "MultipleSettingUpdateTime";
                SettingInfo settingInfo = settingService.GetSettingByKey(settingrequest).Data;
                if (settingInfo != null)
                {
                    settingInfo.StrValue = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    settingInfo.InfoState = InfoState.Modified;
                }
                //更新setting标记
                SettingAddRequest settingrequest1 = new SettingAddRequest();
                settingrequest1.SettingInfo = settingInfo;
                settingService.SaveSetting(settingrequest1);
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
    }
}
