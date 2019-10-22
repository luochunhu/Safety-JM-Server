using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class Installer1 : Installer
    {
        const string InstallFolderMessage = "请选择安装位置";
        const string LicenceFileMessage = "请选择Licence文件";
        const string InstallItemMessage = "请选择安装项";
        public override void BeforeNext()
        {
            base.BeforeNext();
            if(string.IsNullOrEmpty(BaseFolder))
                throw new Exception(InstallFolderMessage);
            if(!InstallModel.InstallItems.Any(q=>q.IsSelected == true))
                throw new Exception(InstallItemMessage);
            if (InstallModel.InstallItems.Exists(q => q.IsSelected && q.InstallLicence) && string.IsNullOrEmpty(LicenceFilePath))
                throw new Exception(LicenceFileMessage);
            SelectedItems.AddRange(InstallModel.InstallItems.Where(q => q.IsSelected));
        }
    }
}
