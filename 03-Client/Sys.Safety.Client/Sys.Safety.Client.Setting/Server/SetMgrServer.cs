using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.Setting.Server
{
    public class SetMgrServer
    {
        private ISettingService SettingService = ServiceFactory.Create<ISettingService>();
        public void DataCollectorPush(Dictionary<string, SettingInfo> dic)
        {
            //TODO：貌似没有用
            //IConfigService iConfig = ServiceFactory.CreateService<IConfigService>();
            //iConfig.PushSettingToDC(dic);
        }
    }
}
