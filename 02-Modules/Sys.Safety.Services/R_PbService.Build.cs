using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Pb;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class R_PbService : IR_PbService
    {
        private IR_PbRepository _Repository;

        string[] columns = new string[] {
                    "Id",
                    "Areaid",
                    "Yid",
                    "Pointid",
                    "Zdzs",
                    "Starttime",
                    "Endtime",
                    "Type",
                    "Z1",
                    "Z2",
                    "Z3",
                    "Z4",
                    "Z5",
                    "Z6",
                    "Upflag"
                };


        public R_PbService(IR_PbRepository _Repository)
        {
            this._Repository = _Repository;
        }



        public BasicResponse<R_PbInfo> AddPb(R_PbAddRequest pbRequest)
        {
            var _pb = ObjectConverter.Copy<R_PbInfo, R_PbModel>(pbRequest.PbInfo);
            var resultpb = _Repository.AddPb(_pb);
            var pbresponse = new BasicResponse<R_PbInfo>();
            pbresponse.Data = ObjectConverter.Copy<R_PbModel, R_PbInfo>(resultpb);
            return pbresponse;
        }
        public BasicResponse<R_PbInfo> UpdatePb(R_PbUpdateRequest pbRequest)
        {
            var _pb = ObjectConverter.Copy<R_PbInfo, R_PbModel>(pbRequest.PbInfo);
            _Repository.UpdatePb(_pb);
            var pbresponse = new BasicResponse<R_PbInfo>();
            pbresponse.Data = ObjectConverter.Copy<R_PbModel, R_PbInfo>(_pb);
            return pbresponse;
        }
        public BasicResponse DeletePb(R_PbDeleteRequest pbRequest)
        {
            _Repository.DeletePb(pbRequest.Id);
            var pbresponse = new BasicResponse();
            return pbresponse;
        }
        public BasicResponse<List<R_PbInfo>> GetPbList(R_PbGetListRequest pbRequest)
        {
            var pbresponse = new BasicResponse<List<R_PbInfo>>();
            pbRequest.PagerInfo.PageIndex = pbRequest.PagerInfo.PageIndex - 1;
            if (pbRequest.PagerInfo.PageIndex < 0)
            {
                pbRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var pbModelLists = _Repository.GetPbList(pbRequest.PagerInfo.PageIndex, pbRequest.PagerInfo.PageSize, out rowcount);
            var pbInfoLists = new List<R_PbInfo>();
            foreach (var item in pbModelLists)
            {
                var PbInfo = ObjectConverter.Copy<R_PbModel, R_PbInfo>(item);
                pbInfoLists.Add(PbInfo);
            }
            pbresponse.Data = pbInfoLists;
            return pbresponse;
        }
        public BasicResponse<R_PbInfo> GetPbById(R_PbGetRequest pbRequest)
        {
            var result = _Repository.GetPbById(pbRequest.Id);
            var pbInfo = ObjectConverter.Copy<R_PbModel, R_PbInfo>(result);
            var pbresponse = new BasicResponse<R_PbInfo>();
            pbresponse.Data = pbInfo;
            return pbresponse;
        }


        public BasicResponse<List<R_PbInfo>> GetPbByPar(R_PbGetByParRequest request)
        {
            //yid,datastate,Sysflag
            var result = _Repository.Datas.Where(a => a.Yid == request.yid && a.Type == (int)request.datastate && a.Z4 == request.Sysflag).ToList();
            var response = new BasicResponse<List<R_PbInfo>>();
            response.Data = ObjectConverter.Copy<List<R_PbModel>, List<R_PbInfo>>(result);
            return response;
        }

        public BasicResponse<List<R_PbInfo>> GetAlarmedDataList()
        {
            var response = new BasicResponse<List<R_PbInfo>>();

            var alarmTable = _Repository.QueryTable("global_R_PBModelService_GetR_PBDataList", DateTime.Now.ToString("yyyyMM"));
            if (alarmTable != null && alarmTable.Rows.Count > 0)
            {
                var alarmList = _Repository.ToEntityFromTable<R_PbInfo>(alarmTable);
                response.Data = alarmList.ToList();
            }
            return response;
        }
        public BasicResponse BacthUpdateAlarmRecord(R_PBBatchUpateRequest r_PbList)
        {
            try
            {
                var alarmInfos = r_PbList.PbInfoList;
                if (alarmInfos != null && alarmInfos.Any())
                {
                    IEnumerable<IGrouping<string, R_PbInfo>> groupYYYYMM = alarmInfos.GroupBy(p => p.Starttime.ToString("yyyyMM"));
                    foreach (IGrouping<string, R_PbInfo> info in groupYYYYMM)
                    {
                        var alarminfolist = info.ToList();
                        var alarmModels = ObjectConverter.CopyList<R_PbInfo, R_PbModel>(alarminfolist);
                        _Repository.BulkUpdate("PE_PersonAlarm" + info.Key, alarmModels, BuildDataColumn(columns), "ID");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量更新人员报警记录失败：" + "\r\n" + ex.Message);
            }
            return new BasicResponse();
        }
        private DataColumn[] BuildDataColumn(string[] columnsName)
        {
            DataColumn[] dataColumns = new DataColumn[columnsName.Count()];
            for (int i = 0; i < columnsName.Count(); i++)
            {
                dataColumns[i] = new DataColumn();
                dataColumns[i].ColumnName = columnsName[i];
            }
            return dataColumns;
        }

    }
}


