using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Cache.BigDataAnalysis;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.Cache
{
    public class SysEmergencyLinkageCacheService : ISysEmergencyLinkageCacheService
    {
        private IEmergencyLinkageMasterAreaAssService EmergencyLinkageMasterAreaAssService = ServiceFactory.Create<IEmergencyLinkageMasterAreaAssService>();
        private IEmergencyLinkageMasterDevTypeAssService EmergencyLinkageMasterDevTypeAssService = ServiceFactory.Create<IEmergencyLinkageMasterDevTypeAssService>();
        private IEmergencyLinkageMasterPointAssService EmergencyLinkageMasterPointAssService = ServiceFactory.Create<IEmergencyLinkageMasterPointAssService>();
        private IEmergencyLinkageMasterTriDataStateAssService EmergencyLinkageMasterTriDataStateAssService = ServiceFactory.Create<IEmergencyLinkageMasterTriDataStateAssService>();
        private IEmergencyLinkagePassiveAreaAssService EmergencyLinkagePassiveAreaAssService = ServiceFactory.Create<IEmergencyLinkagePassiveAreaAssService>();
        private IEmergencyLinkagePassivePersonAssService EmergencyLinkagePassivePersonAssService = ServiceFactory.Create<IEmergencyLinkagePassivePersonAssService>();
        private IEmergencyLinkagePassivePointAssService EmergencyLinkagePassivePointAssService = ServiceFactory.Create<IEmergencyLinkagePassivePointAssService>();

        public Basic.Framework.Web.BasicResponse LoadSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheLoadRequest EmergencyLinkageConfigCacheRequest)
        {
             SysEmergencyLinkageCache.Instance.Load();

             //加载应急联动基本信息之后，加载测应急联动拓展属性
             var sysEmergencyLinkageList = SysEmergencyLinkageCache.Instance.Query();
             if (sysEmergencyLinkageList.Any())
             {
                 sysEmergencyLinkageList.ForEach(e =>
                 {
                     if(!string.IsNullOrEmpty(e.MasterAreaAssId))
                     {
                         var masterareas = EmergencyLinkageMasterAreaAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.MasterAreaAssId) }).Data.ToList();
                         e.MasterAreas = masterareas;
                     }
                     if(!string.IsNullOrEmpty(e.MasterDevTypeAssId))
                     {
                         var masterDevTypes = EmergencyLinkageMasterDevTypeAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.MasterDevTypeAssId) }).Data.ToList();
                         e.MasterDevTypes = masterDevTypes;

                     }
                     if (!string.IsNullOrEmpty(e.MasterPointAssId)) 
                     {
                         var masterpoints = EmergencyLinkageMasterPointAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.MasterPointAssId) }).Data.ToList();
                         e.MasterPoint = masterpoints;
                     }
                     if (!string.IsNullOrEmpty(e.MasterTriDataStateAssId)) 
                     {
                         var masterTriDataStates = EmergencyLinkageMasterTriDataStateAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.MasterTriDataStateAssId) }).Data.ToList();
                         e.MasterTriDataStates = masterTriDataStates;
                     }
                     if (!string.IsNullOrEmpty(e.PassiveAreaAssId)) 
                     {
                         var passiveArea = EmergencyLinkagePassiveAreaAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.PassiveAreaAssId) }).Data.ToList();
                         e.PassiveAreas = passiveArea;
                     }
                     if (!string.IsNullOrEmpty(e.PassivePersonAssId)) 
                     {
                         var passivePerson = EmergencyLinkagePassivePersonAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.PassivePersonAssId) }).Data.ToList();
                         e.PassivePersons = passivePerson;
                     }
                     if (!string.IsNullOrEmpty(e.PassivePointAssId)) 
                     {
                         var PassivePoint = EmergencyLinkagePassivePointAssService.GetEmergencyLinkageMasterAreaAssListByAssId(new Sys.Safety.Request.Listex.LongIdRequest { Id = Convert.ToInt64(e.PassivePointAssId) }).Data.ToList();
                         e.PassivePoints = PassivePoint;
                     }
                 });

                 SysEmergencyLinkageCache.Instance.UpdateItems(sysEmergencyLinkageList);
             }

            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse AddSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheAddRequest EmergencyLinkageConfigCacheRequest)
        {
            SysEmergencyLinkageCache.Instance.AddItem(EmergencyLinkageConfigCacheRequest.SysEmergencyLinkageInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse UpdateSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheUpdateRequest EmergencyLinkageConfigCacheRequest)
        {
            SysEmergencyLinkageCache.Instance.UpdateItem(EmergencyLinkageConfigCacheRequest.SysEmergencyLinkageInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse DeleteSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheDeleteRequest EmergencyLinkageConfigCacheRequest)
        {
            SysEmergencyLinkageCache.Instance.DeleteItem(EmergencyLinkageConfigCacheRequest.SysEmergencyLinkageInfo);
            return new BasicResponse();
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheGetAllRequest EmergencyLinkageConfigCacheRequest)
        {
            var response = new BasicResponse<List<DataContract.SysEmergencyLinkageInfo>>();
            response.Data= SysEmergencyLinkageCache.Instance.Query();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<DataContract.SysEmergencyLinkageInfo> GetSysEmergencyLinkageCacheByKey(Sys.Safety.Request.EmergencyLinkageConfigCacheGetByKeyRequest EmergencyLinkageConfigCacheRequest)
        {
            var response = new BasicResponse<DataContract.SysEmergencyLinkageInfo>();
            response.Data = SysEmergencyLinkageCache.Instance.Query(o => o.Id == EmergencyLinkageConfigCacheRequest.Id).FirstOrDefault();
            return response;
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.SysEmergencyLinkageInfo>> GetSysEmergencyLinkageCache(Sys.Safety.Request.EmergencyLinkageConfigCacheGetByConditonRequest EmergencyLinkageConfigCacheRequest)
        {
            var response = new BasicResponse<List<DataContract.SysEmergencyLinkageInfo>>();
            response.Data = SysEmergencyLinkageCache.Instance.Query(EmergencyLinkageConfigCacheRequest.Predicate);
            return response;
        }
    }
}
