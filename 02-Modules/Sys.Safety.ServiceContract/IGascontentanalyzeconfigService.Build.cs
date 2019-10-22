using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Gascontentanalyzeconfig;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.ServiceContract
{
    public interface IGascontentanalyzeconfigService
    {
        BasicResponse<GascontentanalyzeconfigInfo> AddGascontentanalyzeconfig(
            GascontentanalyzeconfigAddRequest gascontentanalyzeconfigRequest);

        BasicResponse<GascontentanalyzeconfigInfo> UpdateGascontentanalyzeconfig(
            GascontentanalyzeconfigUpdateRequest gascontentanalyzeconfigRequest);

        BasicResponse DeleteGascontentanalyzeconfig(GascontentanalyzeconfigDeleteRequest gascontentanalyzeconfigRequest);

        BasicResponse<List<GascontentanalyzeconfigInfo>> GetGascontentanalyzeconfigList(
            GascontentanalyzeconfigGetListRequest gascontentanalyzeconfigRequest);

        BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigList();

        BasicResponse<List<GascontentanalyzeconfigInfo>> GetAllGascontentanalyzeconfigListCache();

        BasicResponse<GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigById(
            GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest);

        BasicResponse<GascontentanalyzeconfigInfo> GetGascontentanalyzeconfigCacheById(
            GascontentanalyzeconfigGetRequest gascontentanalyzeconfigRequest);
    }
}