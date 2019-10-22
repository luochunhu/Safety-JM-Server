using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Uninstall
{
    public class UninstallModel
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }
        /// <summary>
        /// 安装目录
        /// </summary>
        public string InstallFolder { get; set; }

        private List<UninstallItem> _uninstallItems;
        /// <summary>
        /// 卸载列表
        /// </summary>
        public List<UninstallItem> UninstallItems
        {
            get
            {
                if (_uninstallItems == null)
                    _uninstallItems = new List<UninstallItem>();
                return _uninstallItems;
            }
        }
    }

    public class UninstallItem
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplyName { get; set; }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// 文件路径(FullName)
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// 运行类型(exe, service)
        /// </summary>
        public string RunType { get; set; }

        public string RegistryKey { get; set; }

        /// <summary>
        /// 是否已卸载
        /// </summary>
        public bool IsUninstalled { get; set; }

        /// <summary>
        /// 是否正在卸载
        /// </summary>
        public bool Uninstalling { get; set; }
        /// <summary>
        /// 是否选择卸载
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
