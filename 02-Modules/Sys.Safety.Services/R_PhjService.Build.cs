using System.Collections.Generic;
using System.Linq;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Phj;
using System;
using Sys.Safety.Request.Setting;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class R_PhjService : IR_PhjService
    {
        private IR_PhjRepository _Repository;
        private ISettingService _SettingService;

        public R_PhjService(IR_PhjRepository _Repository, ISettingService _SettingService)
        {
            this._Repository = _Repository;
            this._SettingService = _SettingService;
        }
        public BasicResponse<R_PhjInfo> AddPhj(PhjAddRequest phjRequest)
        {
            var _phj = ObjectConverter.Copy<R_PhjInfo, R_PhjModel>(phjRequest.PhjInfo);
            var resultphj = _Repository.AddPhj(_phj);
            var phjresponse = new BasicResponse<R_PhjInfo>();
            phjresponse.Data = ObjectConverter.Copy<R_PhjModel, R_PhjInfo>(resultphj);
            return phjresponse;
        }
        public BasicResponse<R_PhjInfo> UpdatePhj(PhjUpdateRequest phjRequest)
        {
            var _phj = ObjectConverter.Copy<R_PhjInfo, R_PhjModel>(phjRequest.PhjInfo);
            _Repository.UpdatePhj(_phj);
            var phjresponse = new BasicResponse<R_PhjInfo>();
            phjresponse.Data = ObjectConverter.Copy<R_PhjModel, R_PhjInfo>(_phj);
            return phjresponse;
        }
        public BasicResponse DeletePhj(PhjDeleteRequest phjRequest)
        {
            _Repository.DeletePhj(phjRequest.Id);
            var phjresponse = new BasicResponse();
            return phjresponse;
        }
        public BasicResponse<List<R_PhjInfo>> GetPhjList(PhjGetListRequest phjRequest)
        {
            var phjresponse = new BasicResponse<List<R_PhjInfo>>();
            phjRequest.PagerInfo.PageIndex = phjRequest.PagerInfo.PageIndex - 1;
            if (phjRequest.PagerInfo.PageIndex < 0)
            {
                phjRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var phjModelLists = _Repository.GetPhjList(phjRequest.PagerInfo.PageIndex, phjRequest.PagerInfo.PageSize, out rowcount);
            var phjInfoLists = new List<R_PhjInfo>();
            foreach (var item in phjModelLists)
            {
                var PhjInfo = ObjectConverter.Copy<R_PhjModel, R_PhjInfo>(item);
                phjInfoLists.Add(PhjInfo);
            }
            phjresponse.Data = phjInfoLists;
            return phjresponse;
        }
        public BasicResponse<R_PhjInfo> GetPhjById(PhjGetRequest phjRequest)
        {
            var result = _Repository.GetPhjById(phjRequest.Id);
            var phjInfo = ObjectConverter.Copy<R_PhjModel, R_PhjInfo>(result);
            var phjresponse = new BasicResponse<R_PhjInfo>();
            phjresponse.Data = phjInfo;
            return phjresponse;
        }
        /// <summary>
        /// 添加呼叫记录到数据库
        /// </summary>
        /// <param name="phjRequest"></param>
        /// <returns></returns>
        public BasicResponse AddPhjToDB(PhjAddRequest phjRequest)
        {
            var _phj = ObjectConverter.Copy<R_PhjInfo, R_PhjModel>(phjRequest.PhjInfo);
            var resultphj = _Repository.ExecuteNonQuery("global_R_PhjService_AddPhjToDB"
                , _phj.Timer.ToString("yyyyMM")
                , _phj.Id
                , _phj.Hjlx
                , _phj.Bh
                , _phj.Yid
                , _phj.PointId
                , _phj.CallTime
                , _phj.Tsycs
                , _phj.State
                , _phj.Type
                , _phj.Card
                , _phj.Username
                , _phj.IP
                , _phj.Timer
                , _phj.Flag
                , _phj.SysFlag
                , _phj.upflag
                , _phj.By2);

            var phjresponse = new BasicResponse();
            return phjresponse;
        }
        /// <summary>
        /// 获取当前呼叫的人员信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<string>> GetPhjAlarmedList()
        {
            var result = new BasicResponse<List<string>>();
            result.Data = new List<string>();
            GetSettingByKeyRequest settingrequest = new GetSettingByKeyRequest();
            settingrequest.StrKey = "PersonCallAlarmTime";
            var resultSetting = _SettingService.GetSettingByKey(settingrequest);
            int PersonCallAlarmTime = 120;//默认取1小时内的呼叫记录
            if (resultSetting.Data != null)
            {
                int.TryParse(resultSetting.Data.StrValue, out PersonCallAlarmTime);
            }
            DataTable resultphj = _Repository.QueryTable("global_R_PHJModelService_GetRealR_PHJDataList", DateTime.Now.ToString("yyyyMM"), DateTime.Now.AddMinutes(-PersonCallAlarmTime));
            if (resultphj != null)
            {
                for (int i = 0; i < resultphj.Rows.Count; i++)
                {
                    result.Data.Add(resultphj.Rows[i]["bh"].ToString());
                }
            }
            return result;
        }
    }
}


