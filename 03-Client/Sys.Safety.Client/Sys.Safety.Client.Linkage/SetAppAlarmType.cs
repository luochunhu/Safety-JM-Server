using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Setting;
using Sys.Safety.ServiceContract;
using Sys.Safety.Client.Linkage.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Linkage
{
    public partial class SetAppAlarmType : XtraForm
    {
        ISettingService SettingService = ServiceFactory.Create<ISettingService>();
        /// <summary>
        /// 待选择项
        /// </summary>
        private readonly DataTable _dtSelectItem = new DataTable();
        /// <summary>
        /// 数据状态
        /// </summary>
        private List<EnumcodeInfo> _dataState;
        public SetAppAlarmType()
        {
            InitializeComponent();
        }

        private void SetAppAlarmType_Load(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Columns.Add("Check", typeof(bool));
                _dtSelectItem.Columns.Add("Id");
                _dtSelectItem.Columns.Add("Text");

                _dataState = EnumHandler.GetTriggerDataState();

                GetSettingByKeyRequest getRequest = new GetSettingByKeyRequest();
                getRequest.StrKey = "AppAlarmType";
                var getData = SettingService.GetSettingByKey(getRequest);
                if (getData.IsSuccess && getData.Data != null)
                {
                    SettingInfo setting = getData.Data;
                    string[] alarmType = setting.StrValue.Split(',');
                    _dataState.ForEach(a =>
                    {
                        if (alarmType.FirstOrDefault(b => b == a.LngEnumValue.ToString()) != null)
                        {
                            a.Check = true;
                        }
                    });
                }

                GridControlDataState.DataSource = _dataState;
            }
            catch { }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                string appAlarmType = "";
                //触发数据类型
                foreach (var item in _dataState)
                {
                    if (!item.Check)
                    {
                        continue;
                    }
                    appAlarmType += item.LngEnumValue.ToString() + ",";
                }
                if (appAlarmType.Length > 0)
                {
                    appAlarmType = appAlarmType.Substring(0, appAlarmType.Length - 1);
                }
                SettingInfo setting = null;

                GetSettingByKeyRequest getRequest = new GetSettingByKeyRequest();
                getRequest.StrKey = "AppAlarmType";
                var getData = SettingService.GetSettingByKey(getRequest);
                if (getData.IsSuccess && getData.Data != null)
                {
                    setting = getData.Data;
                }
                if (setting == null)
                {
                    //新增
                    setting = new SettingInfo();
                    setting.ID = IdHelper.CreateLongId().ToString();
                    setting.StrType = "系统设置";
                    setting.StrKey = "AppAlarmType";
                    setting.StrKeyCHs = "App报警推送设置";
                    setting.StrValue = appAlarmType;
                    setting.StrDesc = "App报警推送设置，多个类型用逗号分隔";
                    setting.LastUpdateDate = DateTime.Now.ToString();

                    SettingAddRequest request = new SettingAddRequest();
                    request.SettingInfo = setting;

                    SettingService.AddSetting(request);
                }
                else
                {
                    //修改
                    setting.StrValue = appAlarmType;

                    SettingUpdateRequest request = new SettingUpdateRequest();
                    request.SettingInfo = setting;

                    SettingService.UpdateSetting(request);
                }
                MessageBox.Show("操作成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show("操作失败，原因："+ex.Message);
            }
            
            
        }
    }
}
