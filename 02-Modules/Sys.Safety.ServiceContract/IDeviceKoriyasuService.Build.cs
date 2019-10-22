using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Defwb;

namespace Sys.Safety.ServiceContract
{
    public interface IDeviceKoriyasuService
    {
        BasicResponse<Jc_DefwbInfo> AddDeviceKoriyasu(DeviceKoriyasuAddRequest jc_Defwbrequest);
        BasicResponse<Jc_DefwbInfo> UpdateDeviceKoriyasu(Jc_DefwbUpdateRequest jc_Defwbrequest);
        BasicResponse DeleteDeviceKoriyasu(Jc_DefwbDeleteRequest jc_Defwbrequest);
        BasicResponse<List<Jc_DefwbInfo>> GetDeviceKoriyasuList(Jc_DefwbGetListRequest jc_Defwbrequest);
        BasicResponse<Jc_DefwbInfo> GetDeviceKoriyasuById(Jc_DefwbGetRequest jc_Defwbrequest);
    }
}