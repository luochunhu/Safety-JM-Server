using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Basic.Framework.Data;
using Basic.Framework.Service;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class SysEmergencyLinkageRepository : RepositoryBase<SysEmergencyLinkageModel>,
        ISysEmergencyLinkageRepository
    {
        //private readonly IEmergencyLinkageMasterAreaAssRepository _emergencyLinkageMasterAreaAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkageMasterAreaAssRepository>();

        //private readonly IEmergencyLinkageMasterDevTypeAssRepository _emergencyLinkageMasterDevTypeAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkageMasterDevTypeAssRepository>();

        //private readonly IEmergencyLinkageMasterPointAssRepository _emergencyLinkageMasterPointAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkageMasterPointAssRepository>();

        //private readonly IEmergencyLinkageMasterTriDataStateAssRepository _emergencyLinkageMasterTriDataStateAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkageMasterTriDataStateAssRepository>();

        //private readonly IEmergencyLinkagePassiveAreaAssRepository _emergencyLinkagePassiveAreaAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkagePassiveAreaAssRepository>();

        //private readonly IEmergencyLinkagePassivePersonAssRepository _emergencyLinkagePassivePersonAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkagePassivePersonAssRepository>();

        //private readonly IEmergencyLinkagePassivePointAssRepository _emergencyLinkagePassivePointAssRepository =
        //    ServiceFactory.Create<IEmergencyLinkagePassivePointAssRepository>();
        
        public SysEmergencyLinkageModel AddSysEmergencyLinkage(SysEmergencyLinkageModel sysEmergencyLinkageModel)
        {
            return Insert(sysEmergencyLinkageModel);
        }

        public void UpdateSysEmergencyLinkage(SysEmergencyLinkageModel sysEmergencyLinkageModel)
        {
            Update(sysEmergencyLinkageModel);
        }

        public void DeleteSysEmergencyLinkage(string id)
        {
            Delete(id);
        }

        public IList<SysEmergencyLinkageModel> GetSysEmergencyLinkageList(int pageIndex, int pageSize, out int rowCount)
        {
            var sysEmergencyLinkageModelLists = Datas.ToList();
            rowCount = Datas.Count();
            return sysEmergencyLinkageModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public IList<SysEmergencyLinkageModel> GetAllSysEmergencyLinkageList()
        {
            var sysEmergencyLinkageModelLists = Datas.ToList();
            return sysEmergencyLinkageModelLists;
        }

        public SysEmergencyLinkageModel GetSysEmergencyLinkageById(string id)
        {
            var sysEmergencyLinkageModel = Datas.FirstOrDefault(c => c.Id == id);
            return sysEmergencyLinkageModel;
        }

        public IList<SysEmergencyLinkageModel> GetSysEmergencyLinkageListByLambda(Expression<Func<SysEmergencyLinkageModel, bool>> lambda)
        {
            var res = Datas.Where(lambda);
            return res.ToList();
        }

        public bool AnySysEmergencyLinkageByLambda(Expression<Func<SysEmergencyLinkageModel, bool>> lambda)
        {
            var res = Datas.Any(lambda);
            return res;
        }
    }
}