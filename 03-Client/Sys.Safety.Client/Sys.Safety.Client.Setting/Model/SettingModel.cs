using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Setting;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Setting
{
    public class SettingModel
    {

        private ISettingService settingService = ServiceFactory.Create<ISettingService>();
        public void SaveSetting(SettingInfo dto, int state)
        {
            var request = new SaveSettingForConditionRequest() { SettingInfo = dto, State = state };
            settingService.SaveSettingForCondition(request);
        }
        public void DeleteSetting(string id)
        {
            var request = new SettingDeleteRequest() { Id = id };
            settingService.DeleteSetting(request);
        }
        public List<SettingInfo> GetSettingList()
        {
            var response = settingService.GetSettingListForCreator();
            return response.Data;
        }
    }
}
