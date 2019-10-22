using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class Installer5 : Installer
    {
        public override void Load()
        {
            base.Load();
            StartServices.AddRange(InstallModel.InstallItems.Where(q => q.Type == "copy" && q.RunType == "service" && q.IsInstalled));
        }
        public override void BeforeNext()
        {
            base.BeforeNext();
            List<InstallItem> selectedService = StartServices.Where(q => q.IsSelected).ToList();
            foreach (var service in selectedService)
            {
                StartService(service.ServiceName);
            }
        }
    }
}
