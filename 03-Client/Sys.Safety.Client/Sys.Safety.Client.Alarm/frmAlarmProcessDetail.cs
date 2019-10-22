using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Jc_B;
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

namespace Sys.Safety.Client.Alarm
{
    public partial class frmAlarmProcessDetail : XtraForm
    {
        private int _alarmType;
        private string _alarmId;
        private string _stime;

        private readonly IAlarmRecordService alarmRecordService;
        private readonly IAlarmHandleService alarmHandleService;

        public frmAlarmProcessDetail(int alarmType, string alarmId, string stime)
        {
            InitializeComponent();
            _alarmType = alarmType;
            _alarmId = alarmId;
            _stime = stime;
            alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();
            alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();

            if (_alarmType == 1)
            {
                AlarmRecordGetDateIdRequest alarmgetByIdrequest = new AlarmRecordGetDateIdRequest
                {
                    Id = _alarmId,
                    AlarmDate = _stime
                };
                var response = alarmRecordService.GetDateAlarmRecordById(alarmgetByIdrequest);
                if (response.IsSuccess && response.Data != null)
                {
                    reasonEdit.Text = response.Data.Remark;
                    measureEdit.Text = response.Data.Cs;
                }
            }
            else if (_alarmType == 2)
            {
                AlarmHandleGetRequest getRequest = new AlarmHandleGetRequest
                {
                    Id = _alarmId
                };
                var getresponse = alarmHandleService.GetJC_AlarmHandleById(getRequest);
                if (getresponse.IsSuccess && getresponse.Data != null)
                {
                    reasonEdit.Text = getresponse.Data.ExceptionReason;
                    measureEdit.Text = getresponse.Data.Handling;
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (reasonEdit.Text.Length <= 0)
            {
                XtraMessageBox.Show("报警原因必填！");
                return;
            }
            if (reasonEdit.Text.Length > 500)
            {
                XtraMessageBox.Show("报警原因长度不能大于500");
                return;
            }
            if (measureEdit.Text.Length <= 0)
            {
                XtraMessageBox.Show("处理措施必填！");
                return;
            }
            if (measureEdit.Text.Length > 500)
            {
                XtraMessageBox.Show("处理措施长度不能大于500");
                return;
            }


            bool issuccess = false;

            if (_alarmType == 1)
            {
                issuccess = EndDeviceAlarm();
            }
            else if (_alarmType == 2)
            {
                issuccess = EndLogicAlarm();
            }
            if (issuccess)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 结束设备报警
        /// </summary>
        /// <returns></returns>
        private bool EndDeviceAlarm()
        {
            AlarmRecordGetDateIdRequest alarmgetByIdrequest = new AlarmRecordGetDateIdRequest
            {
                Id = _alarmId,
                AlarmDate = _stime
            };
            var response = alarmRecordService.GetDateAlarmRecordById(alarmgetByIdrequest);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                Jc_BInfo alarmInfo = response.Data;
                ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                if (_ClientItem != null)
                {
                    alarmInfo.Bz1 = _ClientItem.UserName;
                }
                //alarmInfo.Etime = DateTime.Now;
                alarmInfo.Bz2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                alarmInfo.Remark = reasonEdit.Text;
                alarmInfo.Cs = measureEdit.Text;

                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                updateItems.Add("Bz1", alarmInfo.Bz1);
                updateItems.Add("Bz2", alarmInfo.Bz2);
                updateItems.Add("Remark", alarmInfo.Remark);
                updateItems.Add("Cs", alarmInfo.Cs);

                AlarmRecordUpdateProperitesRequest updateRequest = new AlarmRecordUpdateProperitesRequest()
                {
                    AlarmInfo = alarmInfo,
                    UpdateItems = updateItems
                };
                var updateResponse = alarmRecordService.UpdateAlarmInfoProperties(updateRequest);
                if (updateResponse != null && updateResponse.IsSuccess)
                {
                    return updateResponse.Data;
                }
            }
            return false;
        }

        private bool EndLogicAlarm()
        {
            AlarmHandleGetRequest getRequest = new AlarmHandleGetRequest
            {
                Id = _alarmId
            };
            var getresponse = alarmHandleService.GetJC_AlarmHandleById(getRequest);
            if (getresponse != null && getresponse.IsSuccess && getresponse.Data != null)
            {
                JC_AlarmHandleInfo alarmHandelInfo = getresponse.Data;
                alarmHandelInfo.ExceptionReason = reasonEdit.Text;
                alarmHandelInfo.Handling = measureEdit.Text;
                //alarmHandelInfo.EndTime = DateTime.Now;
                alarmHandelInfo.HandlingTime = DateTime.Now;
                ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                if (_ClientItem != null)
                {
                    alarmHandelInfo.HandlingPerson = _ClientItem.UserName;
                }

                AlarmHandleUpdateRequest updateRequest = new AlarmHandleUpdateRequest
                {
                    JC_AlarmHandleInfo = alarmHandelInfo
                };

                var updateResonse = alarmHandleService.UpdateJC_AlarmHandle(updateRequest);
                if (updateResonse != null && updateResonse.IsSuccess && updateResonse.Data != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
