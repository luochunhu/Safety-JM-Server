using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Control.Model
{
    /// <summary>
    /// 设置网络模块客户端接口  20170703
    /// </summary>
    public class SetNetworkModule
    {
        /// <summary>
        /// 发送网络模块时间同步命令
        /// </summary>
        public void SetNetworkModuleSyncTime()
        {
            try
            {
                INetworkModuleService NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
                NetworkModuleService.SetNetworkModuleSyncTime();
                MessageBox.Show("发送网络模块时间同步成功！");
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                MessageBox.Show("发送网络模块时间同步失败，详细查看日志！");                
            }
        }
    }
}
