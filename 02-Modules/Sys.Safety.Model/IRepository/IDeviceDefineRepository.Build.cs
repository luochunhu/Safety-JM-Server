using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDeviceDefineRepository : IRepository<Jc_DevModel>
    {
        Jc_DevModel AddDeviceDefine(Jc_DevModel DeviceDefineModel);
        void UpdateDeviceDefine(Jc_DevModel DeviceDefineModel);
        void DeleteDeviceDefine(string id);
        IList<Jc_DevModel> GetDeviceDefineList(int pageIndex, int pageSize, out int rowCount);
        List<Jc_DevModel> GetDeviceDefineList();
        Jc_DevModel GetDeviceDefineById(string id);

        IList<Jc_DevModel> GetDeviceDefineByDevId(string id);
    }
}