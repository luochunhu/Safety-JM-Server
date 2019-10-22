using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Powerboxchargehistory;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class PowerboxchargehistoryService : IPowerboxchargehistoryService
    {
        private IPowerboxchargehistoryRepository _Repository;

        public PowerboxchargehistoryService(IPowerboxchargehistoryRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<PowerboxchargehistoryInfo> AddPowerboxchargehistory(PowerboxchargehistoryAddRequest powerboxchargehistoryRequest)
        {
            var _powerboxchargehistory = ObjectConverter.Copy<PowerboxchargehistoryInfo, PowerboxchargehistoryModel>(powerboxchargehistoryRequest.PowerboxchargehistoryInfo);
            var resultpowerboxchargehistory = _Repository.AddPowerboxchargehistory(_powerboxchargehistory);
            var powerboxchargehistoryresponse = new BasicResponse<PowerboxchargehistoryInfo>();
            powerboxchargehistoryresponse.Data = ObjectConverter.Copy<PowerboxchargehistoryModel, PowerboxchargehistoryInfo>(resultpowerboxchargehistory);
            return powerboxchargehistoryresponse;
        }
        public BasicResponse<PowerboxchargehistoryInfo> UpdatePowerboxchargehistory(PowerboxchargehistoryUpdateRequest powerboxchargehistoryRequest)
        {
            var _powerboxchargehistory = ObjectConverter.Copy<PowerboxchargehistoryInfo, PowerboxchargehistoryModel>(powerboxchargehistoryRequest.PowerboxchargehistoryInfo);
            _Repository.UpdatePowerboxchargehistory(_powerboxchargehistory);
            var powerboxchargehistoryresponse = new BasicResponse<PowerboxchargehistoryInfo>();
            powerboxchargehistoryresponse.Data = ObjectConverter.Copy<PowerboxchargehistoryModel, PowerboxchargehistoryInfo>(_powerboxchargehistory);
            return powerboxchargehistoryresponse;
        }
        public BasicResponse DeletePowerboxchargehistory(PowerboxchargehistoryDeleteRequest powerboxchargehistoryRequest)
        {
            _Repository.DeletePowerboxchargehistory(powerboxchargehistoryRequest.Id);
            var powerboxchargehistoryresponse = new BasicResponse();
            return powerboxchargehistoryresponse;
        }
        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryList(PowerboxchargehistoryGetListRequest powerboxchargehistoryRequest)
        {
            var powerboxchargehistoryresponse = new BasicResponse<List<PowerboxchargehistoryInfo>>();
            powerboxchargehistoryRequest.PagerInfo.PageIndex = powerboxchargehistoryRequest.PagerInfo.PageIndex - 1;
            if (powerboxchargehistoryRequest.PagerInfo.PageIndex < 0)
            {
                powerboxchargehistoryRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var powerboxchargehistoryModelLists = _Repository.GetPowerboxchargehistoryList(powerboxchargehistoryRequest.PagerInfo.PageIndex, powerboxchargehistoryRequest.PagerInfo.PageSize, out rowcount);
            var powerboxchargehistoryInfoLists = new List<PowerboxchargehistoryInfo>();
            foreach (var item in powerboxchargehistoryModelLists)
            {
                var PowerboxchargehistoryInfo = ObjectConverter.Copy<PowerboxchargehistoryModel, PowerboxchargehistoryInfo>(item);
                powerboxchargehistoryInfoLists.Add(PowerboxchargehistoryInfo);
            }
            powerboxchargehistoryresponse.Data = powerboxchargehistoryInfoLists;
            return powerboxchargehistoryresponse;
        }
        public BasicResponse<PowerboxchargehistoryInfo> GetPowerboxchargehistoryById(PowerboxchargehistoryGetRequest powerboxchargehistoryRequest)
        {
            var result = _Repository.GetPowerboxchargehistoryById(powerboxchargehistoryRequest.Id);
            var powerboxchargehistoryInfo = ObjectConverter.Copy<PowerboxchargehistoryModel, PowerboxchargehistoryInfo>(result);
            var powerboxchargehistoryresponse = new BasicResponse<PowerboxchargehistoryInfo>();
            powerboxchargehistoryresponse.Data = powerboxchargehistoryInfo;
            return powerboxchargehistoryresponse;
        }

        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByFzhOrMac(PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest)
        {
            var result = _Repository.QueryTable("global_GetPowerBoxChargeByFzhOrMac", powerboxchargehistoryRequest.Fzh, powerboxchargehistoryRequest.Mac);
            var powerboxchargehistoryInfo = _Repository.ToEntityFromTable<PowerboxchargehistoryInfo>(result);
            var powerboxchargehistoryresponse = new BasicResponse<List<PowerboxchargehistoryInfo>>();
            powerboxchargehistoryresponse.Data = powerboxchargehistoryInfo.ToList();
            return powerboxchargehistoryresponse;
        }

        public BasicResponse<List<PowerboxchargehistoryInfo>> GetPowerboxchargehistoryByStime(PowerboxchargehistoryGetByStimeRequest powerboxchargehistoryRequest)
        {
            var result = _Repository.QueryTable("global_GetPowerBoxChargeByStime", powerboxchargehistoryRequest.Stime.ToString("yyyy-MM-dd HH:mm:ss"));
            var powerboxchargehistoryInfo = _Repository.ToEntityFromTable<PowerboxchargehistoryInfo>(result);
            var powerboxchargehistoryresponse = new BasicResponse<List<PowerboxchargehistoryInfo>>();
            powerboxchargehistoryresponse.Data = powerboxchargehistoryInfo.ToList();
            return powerboxchargehistoryresponse;
        }
    }
}


