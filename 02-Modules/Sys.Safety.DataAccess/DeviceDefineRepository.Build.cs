using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class DeviceDefineRepository : RepositoryBase<Jc_DevModel>, IDeviceDefineRepository
    {
        public Jc_DevModel AddDeviceDefine(Jc_DevModel DeviceDefineModel)
        {
            return Insert(DeviceDefineModel);
        }

        public void UpdateDeviceDefine(Jc_DevModel DeviceDefineModel)
        {
            Update(DeviceDefineModel);
        }

        public void DeleteDeviceDefine(string id)
        {
            Delete(id);
        }

        public IList<Jc_DevModel> GetDeviceDefineList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_DevModelLists = Datas;
            rowCount = jc_DevModelLists.Count();
            return jc_DevModelLists.OrderBy(p => p.Devid).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<Jc_DevModel> GetDeviceDefineList()
        {
            var jc_DevModelLists = Datas.ToList();            
            return jc_DevModelLists;
        }

        public Jc_DevModel GetDeviceDefineById(string id)
        {
            var jc_DevModel = Datas.FirstOrDefault(c => c.ID == id);
            return jc_DevModel;
        }

        public IList<Jc_DevModel> GetDeviceDefineByDevId(string id)
        {
            var model = from m in Datas
                where m.Devid == id
                select m;
            return model.ToList();
        }
    }
}