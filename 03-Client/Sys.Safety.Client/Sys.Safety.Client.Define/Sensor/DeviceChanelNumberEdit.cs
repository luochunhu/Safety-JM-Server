using DevExpress.Utils;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.PointDefine;
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

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class DeviceChanelNumberEdit : XtraForm
    {
        string _deviceOnlyCode;
        string _chanelNumber;
        string _branchNumber;
        string _stationNumber;
        int _devType;
        IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        public DeviceChanelNumberEdit(string deviceOnlyCode, string chanelNumber, string branchNumber, string stationNumber, int devType)
        {
            _deviceOnlyCode = deviceOnlyCode;
            _chanelNumber = chanelNumber;
            _branchNumber = branchNumber;
            _stationNumber = stationNumber;
            _devType = devType;
            InitializeComponent();
        }

        private void DeviceChanelNumberEdit_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_deviceOnlyCode))
            {
                XtraMessageBox.Show("设备的唯一编码不存在，不能修改地址号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }

            DeviceOnlyCode.Text = _deviceOnlyCode;
            DeviceChanelNumberNow.Text = _chanelNumber;
            DeviceBranchNumber.Text = _branchNumber;

            DeviceChanelNumberNew.Properties.MinValue = (int.Parse(_branchNumber) - 1) * 4 + 1;
            DeviceChanelNumberNew.Properties.MaxValue = (int.Parse(_branchNumber)) * 4;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //判断当前修改的地址号是否已经存在  20170909
                PointDefineGetByStationIDChannelIDRequest pointDefineGetByStationIDChannelIDRequest = new PointDefineGetByStationIDChannelIDRequest();
                pointDefineGetByStationIDChannelIDRequest.StationID = ushort.Parse(_stationNumber);
                pointDefineGetByStationIDChannelIDRequest.ChannelID = (byte)DeviceChanelNumberNew.Value;
                Jc_DefInfo tempDef = pointDefineService.GetPointDefineCacheByStationIDChannelID(pointDefineGetByStationIDChannelIDRequest).Data.Find(a => a.DevPropertyID == 1 || a.DevPropertyID == 2);
                if (tempDef != null)
                {
                    XtraMessageBox.Show("当前修改的通道已经定义了其它设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DeviceAddressModificationRequest request = new DeviceAddressModificationRequest();

                List<DeviceAddressModificationItem> deviceAddressModificationItems = new List<DeviceAddressModificationItem>();
                DeviceAddressModificationItem deviceAddressModificationItem = new DeviceAddressModificationItem();
                deviceAddressModificationItem.fzh = ushort.Parse(_stationNumber);

                List<DeviceAddressItem> modificationItems = new List<DeviceAddressItem>();

                DeviceAddressItem editDeviceAdressItem = new DeviceAddressItem();
                editDeviceAdressItem.SoleCoding = DeviceOnlyCode.Text;
                editDeviceAdressItem.BeforeModification = byte.Parse(DeviceChanelNumberNow.Text);
                editDeviceAdressItem.AfterModification = (byte)DeviceChanelNumberNew.Value;
                editDeviceAdressItem.DeviceType = (byte)_devType;

                modificationItems.Add(editDeviceAdressItem);

                deviceAddressModificationItem.DeviceAddressItem = modificationItems;
                deviceAddressModificationItems.Add(deviceAddressModificationItem);

                request.DeviceAddressModificationItems = deviceAddressModificationItems;

                var result = pointDefineService.ModificationDeviceAdressRequest(request);

                //删除等待  20180408
                //var wdf = new WaitDialogForm("正在下发命令...", "请等待...");
                //Thread.Sleep(30000);
                //if (wdf != null)
                //    wdf.Close();

                if (result.IsSuccess)
                {
                    XtraMessageBox.Show("下发成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();//下发成功后，关闭页面  20180408
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //判断当前修改的地址号是否已经存在  20170909
                PointDefineGetByStationIDChannelIDRequest pointDefineGetByStationIDChannelIDRequest = new PointDefineGetByStationIDChannelIDRequest();
                pointDefineGetByStationIDChannelIDRequest.StationID = ushort.Parse(_stationNumber);
                pointDefineGetByStationIDChannelIDRequest.ChannelID = (byte)DeviceChanelNumberNew.Value;
                Jc_DefInfo tempDef = pointDefineService.GetPointDefineCacheByStationIDChannelID(pointDefineGetByStationIDChannelIDRequest).Data.Find(a => a.DevPropertyID == 1 || a.DevPropertyID == 2);
                if (tempDef != null)
                {
                    XtraMessageBox.Show("当前修改的通道已经定义了其它设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DeviceAddressModificationRequest request = new DeviceAddressModificationRequest();

                List<DeviceAddressModificationItem> deviceAddressModificationItems = new List<DeviceAddressModificationItem>();
                DeviceAddressModificationItem deviceAddressModificationItem = new DeviceAddressModificationItem();
                deviceAddressModificationItem.fzh = ushort.Parse(_stationNumber);

                List<DeviceAddressItem> modificationItems = new List<DeviceAddressItem>();

                DeviceAddressItem editDeviceAdressItem = new DeviceAddressItem();
                editDeviceAdressItem.SoleCoding = DeviceOnlyCode.Text;
                editDeviceAdressItem.BeforeModification = byte.Parse(DeviceChanelNumberNow.Text);
                editDeviceAdressItem.AfterModification = (byte)DeviceChanelNumberNew.Value;
                editDeviceAdressItem.DeviceType = (byte)_devType;

                modificationItems.Add(editDeviceAdressItem);

                deviceAddressModificationItem.DeviceAddressItem = modificationItems;
                deviceAddressModificationItems.Add(deviceAddressModificationItem);

                request.DeviceAddressModificationItems = deviceAddressModificationItems;

                var result = pointDefineService.ModificationDeviceAdressRequest(request);

                //删除等待  20180408
                //var wdf = new WaitDialogForm("正在下发命令...", "请等待...");
                //Thread.Sleep(30000);
                //if (wdf != null)
                //    wdf.Close();

                if (result.IsSuccess)
                {
                    XtraMessageBox.Show("下发成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();//下发成功后，关闭页面  20180408
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
