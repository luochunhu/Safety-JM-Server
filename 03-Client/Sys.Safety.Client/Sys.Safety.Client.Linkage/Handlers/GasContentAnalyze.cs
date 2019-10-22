using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Linkage.Handlers
{
    public static class GasContentAnalyzeRequest
    {
        private static readonly IGascontentanalyzeconfigService GascontentanalyzeconfigService = ServiceFactory.Create<IGascontentanalyzeconfigService>();

        public static bool AddGasContentAnalyzeConfig(GascontentanalyzeconfigInfo info)
        {
            var req = new GascontentanalyzeconfigAddRequest
            {
                GascontentanalyzeconfigInfo = info
            };
            var res = GascontentanalyzeconfigService.AddGascontentanalyzeconfig(req);
            return res.Code == 100;
        }

        public static List<GascontentanalyzeconfigInfo> GetAllGasContentAnalyzeConfig()
        {
            var res = GascontentanalyzeconfigService.GetAllGascontentanalyzeconfigListCache();
            return res.Data;
        }

        public static void DeleteGasContentAnalyzeConfig(string id)
        {
            var req = new GascontentanalyzeconfigDeleteRequest
            {
                Id = id
            };
            GascontentanalyzeconfigService.DeleteGascontentanalyzeconfig(req);
        }
    }
}
