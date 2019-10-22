using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.Setting;

namespace Sys.Safety.ClientFramework.Model
{
    public class SettingModel
    {

        ISettingService _SettingService = ServiceFactory.Create<ISettingService>();

        public SettingInfo SaveSetting(SettingInfo dto)
        {
            SettingAddRequest settingrequest = new SettingAddRequest();
            settingrequest.SettingInfo = dto;
            var result = _SettingService.SaveSetting(settingrequest);
            return result.Data;
        }

        public void DeleteSetting(SettingInfo dto)
        {
            SettingDeleteRequest settingrequest = new SettingDeleteRequest();
            settingrequest.Id = dto.ID;
            _SettingService.DeleteSetting(settingrequest);
        }


        public List<SettingInfo> GetSettingList()
        {
            var result = _SettingService.GetSettingList();
            return result.Data;
        }
       
    }
}
