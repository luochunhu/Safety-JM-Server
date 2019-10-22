using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_B;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;
using Basic.Framework.Logging;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.Services
{
    public partial class AlarmRecordService : IAlarmRecordService
    {
        private IAlarmRecordRepository _Repository;
        private IAlarmCacheService _alarmCacheService;

        string[] columns = new string[] {
                    "ID",
                    "pointID",
                    "fzh",
                    "kh",
                    "dzh",
                    "devid",
                    "wzid",
                    "point",
                    "type",
                    "state",
                    "stime",
                    "etime",
                    "ssz",
                    "zdz",
                    "pjz",
                    "zdzs",
                    "cs",
                    "kzk",
                    "kdid",
                    "isalarm",
                    "upflag",
                    "remark",
                    "Bz1",
                    "Bz2",
                    "Bz3",
                    "Bz4",
                    "Bz5"
                };

        public AlarmRecordService(IAlarmRecordRepository _Repository, IAlarmCacheService alarmCacheService)
        {
            this._Repository = _Repository;
            this._alarmCacheService = alarmCacheService;
        }
        public BasicResponse<Jc_BInfo> AddAlarmRecord(AlarmRecordAddRequest AlarmRecordRequest)
        {
            var _jc_B = ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(AlarmRecordRequest.Jc_BInfo);
            var resultjc_B = _Repository.AddAlarmRecord(_jc_B);
            var jc_Bresponse = new BasicResponse<Jc_BInfo>();
            jc_Bresponse.Data = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(resultjc_B);
            return jc_Bresponse;
        }
        public BasicResponse<Jc_BInfo> UpdateAlarmRecord(AlarmRecordUpdateRequest AlarmRecordRequest)
        {
            var _jc_B = ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(AlarmRecordRequest.Jc_BInfo);
            _Repository.UpdateAlarmRecord(_jc_B);
            var jc_Bresponse = new BasicResponse<Jc_BInfo>();
            jc_Bresponse.Data = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(_jc_B);
            return jc_Bresponse;
        }
        public BasicResponse DeleteAlarmRecord(AlarmRecordDeleteRequest AlarmRecordRequest)
        {
            _Repository.DeleteAlarmRecord(AlarmRecordRequest.Id);
            var jc_Bresponse = new BasicResponse();
            return jc_Bresponse;
        }
        public BasicResponse<List<Jc_BInfo>> GetAlarmRecordList(AlarmRecordGetListRequest AlarmRecordRequest)
        {
            var jc_Bresponse = new BasicResponse<List<Jc_BInfo>>();
            AlarmRecordRequest.PagerInfo.PageIndex = AlarmRecordRequest.PagerInfo.PageIndex - 1;
            if (AlarmRecordRequest.PagerInfo.PageIndex < 0)
            {
                AlarmRecordRequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_BModelLists = _Repository.GetAlarmRecordList(AlarmRecordRequest.PagerInfo.PageIndex, AlarmRecordRequest.PagerInfo.PageSize, out rowcount);
            var jc_BInfoLists = new List<Jc_BInfo>();
            foreach (var item in jc_BModelLists)
            {
                var Jc_BInfo = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(item);
                jc_BInfoLists.Add(Jc_BInfo);
            }
            jc_Bresponse.Data = jc_BInfoLists;
            return jc_Bresponse;
        }
        /// <summary>
        /// 获取当前正在报警的数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_BInfo>> GetAlarmedDataList()
        {
            var jc_Bresponse = new BasicResponse<List<Jc_BInfo>>();
            var jc_BModelLists = _Repository.GetAlarmedDataList();
            if (jc_BModelLists == null)
            {
                return jc_Bresponse;
            }
            var jc_BInfoLists = new List<Jc_BInfo>();
            foreach (var item in jc_BModelLists)
            {
                var Jc_BInfo = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(item);
                jc_BInfoLists.Add(Jc_BInfo);
            }
            jc_Bresponse.Data = jc_BInfoLists;
            return jc_Bresponse;
        }
        /// <summary>
        /// 获取所有人员未结束设备报警信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_BInfo>> GetR_AlarmedDataList()
        {
            var jc_Bresponse = new BasicResponse<List<Jc_BInfo>>();
            DataTable AlarmedDataList = _Repository.QueryTable("global_AlarmModelService_GetR_AlarmDataList", DateTime.Now.ToString("yyyyMM"));
            var jc_BModelLists = Basic.Framework.Common.ObjectConverter.Copy<Jc_BModel>(AlarmedDataList);
            if (jc_BModelLists == null)
            {
                return jc_Bresponse;
            }
            var jc_BInfoLists = new List<Jc_BInfo>();
            foreach (var item in jc_BModelLists)
            {
                var Jc_BInfo = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(item);
                jc_BInfoLists.Add(Jc_BInfo);
            }
            jc_Bresponse.Data = jc_BInfoLists;
            return jc_Bresponse;
        }
        public BasicResponse<Jc_BInfo> GetAlarmRecordById(AlarmRecordGetRequest AlarmRecordRequest)
        {
            var result = _Repository.GetAlarmRecordById(AlarmRecordRequest.Id);
            var jc_BInfo = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(result);
            var jc_Bresponse = new BasicResponse<Jc_BInfo>();
            jc_Bresponse.Data = jc_BInfo;
            return jc_Bresponse;
        }

        public BasicResponse BacthUpdateAlarmRecord(AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            try
            {
                var alarmInfos = AlarmRecordRequest.AlarmInfos;
                if (alarmInfos != null && alarmInfos.Any())
                {
                    IEnumerable<IGrouping<string, Jc_BInfo>> groupYYYYMM = alarmInfos.GroupBy(p => p.Stime.ToString("yyyyMM"));
                    foreach (IGrouping<string, Jc_BInfo> info in groupYYYYMM)
                    {
                        var alarminfolist = info.ToList();
                        var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarminfolist);
                        _Repository.BulkUpdate("KJ_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns), "ID");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量更新报警记录失败：" + "\r\n" + ex.Message);
            }
            return new BasicResponse();
        }
        /// <summary>
        /// 更新人员报警信息
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        public BasicResponse BacthUpdateR_AlarmRecord(AlarmRecordBatchUpateRequesst AlarmRecordRequest)
        {
            try
            {
                var alarmInfos = AlarmRecordRequest.AlarmInfos;
                if (alarmInfos != null && alarmInfos.Any())
                {
                    IEnumerable<IGrouping<string, Jc_BInfo>> groupYYYYMM = alarmInfos.GroupBy(p => p.Stime.ToString("yyyyMM"));
                    foreach (IGrouping<string, Jc_BInfo> info in groupYYYYMM)
                    {
                        var alarminfolist = info.ToList();
                        var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarminfolist);
                        _Repository.BulkUpdate("PE_DataAlarm" + info.Key, alarmModels, BuildDataColumn(columns), "ID");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量更新报警记录失败：" + "\r\n" + ex.Message);
            }
            return new BasicResponse();
        }

        public BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            BasicResponse<List<AlarmProcessInfo>> resultResponse = new BasicResponse<List<AlarmProcessInfo>>();

            int pageindex = AlarmRecordRequest.PagerInfo.PageIndex;
            int pagesize = AlarmRecordRequest.PagerInfo.PageSize;

            try
            {
                DateTime stime = new DateTime();
                DateTime.TryParse(AlarmRecordRequest.Stime, out stime);
                DateTime etime = new DateTime();
                DateTime.TryParse(AlarmRecordRequest.ETime, out etime); 
                List<AlarmProcessInfo> alarmRecordList = new List<AlarmProcessInfo>();
                DataTable alarmTable = new DataTable();
                //如果跨月,则需要查询当前报警月表与上一月报警月表;反之则查询当前月表
                if (stime.Month != etime.Month)
                {
                    //DateTime temptime = new DateTime();
                    ////跨一天
                    //if (stime.Month == etime.AddDays(-1).Month)
                    //{
                    //    temptime = etime.AddDays(-1);
                    //}
                    ////跨2天
                    //else if (stime.Month == etime.AddDays(-2).Month)
                    //{
                    //    temptime = etime.AddDays(-2);
                    //}

                    //if (temptime.Year != 1)
                    //{
                        try
                        {
                            //查询上一月数据
                            alarmTable = _Repository.QueryTable("global_GetAlarmRecordByStime", new object[] { "KJ_DataAlarm" + stime.ToString("yyyyMM"), etime, stime, stime, etime });
                            //alarmRecordList = _Repository.ToEntityFromTable<AlarmProcessInfo>(alarmTable).ToList();
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("根据日期获取报警数据失败:" + "\r\n" + ex.Message);
                        }
                        try
                        {
                            //查询本月数据
                            DataTable tempTable = _Repository.QueryTable("global_GetAlarmRecordByStime", new object[] { "KJ_DataAlarm" + etime.ToString("yyyyMM"), etime, stime, stime, etime });

                            //List<AlarmProcessInfo> tempalarmRecordList = _Repository.ToEntityFromTable<AlarmProcessInfo>(tempTable).ToList();
                            //alarmRecordList.AddRange(tempalarmRecordList);

                            foreach (DataRow row in tempTable.Rows)
                            {
                                alarmTable.ImportRow(row);
                            }

                            //alarmTable.Merge(tempTable,)
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("根据日期获取报警数据失败:" + "\r\n" + ex.Message);
                        }
                    //}
                }
                else
                {
                    alarmTable = _Repository.QueryTable("global_GetAlarmRecordByStime", new object[] { "KJ_DataAlarm" + stime.ToString("yyyyMM"), etime, stime, stime, etime });
                    //alarmRecordList = _Repository.ToEntityFromTable<AlarmProcessInfo>(alarmTable).ToList();
                }


                DataTable alarmdt = GetPagedTable(alarmTable, AlarmRecordRequest.PagerInfo.PageIndex, AlarmRecordRequest.PagerInfo.PageSize);
                alarmRecordList = _Repository.ToEntityFromTable<AlarmProcessInfo>(alarmdt).ToList();

                alarmRecordList.ForEach(o =>
                {
                    if (o.Etime == null || o.Etime.ToString("yyyy-MM-dd HH:mm:ss") == "1900-01-01 00:00:00")
                    {
                        o.EtimeDisplay = "-";
                    }
                    else
                    {
                        o.EtimeDisplay = o.Etime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                });

                resultResponse.Data = alarmRecordList.OrderBy(o => o.Stime).ToList();
                resultResponse.PagerInfo.RowCount = alarmTable.Rows.Count;
                resultResponse.PagerInfo.PageSize = pagesize;
                resultResponse.PagerInfo.PageIndex = pageindex;
                return resultResponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取设备报警记录失败：" + "\r\n" + ex.Message);
                resultResponse.Data = new List<AlarmProcessInfo>();
                return resultResponse;
            }
        }
        /// <summary>
        /// 查询分站中断记录
        /// </summary>
        /// <param name="AlarmRecordRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<AlarmProcessInfo>> GetStaionInterruptRecordListByStime(AlarmRecordGetByStimeRequest AlarmRecordRequest)
        {
            BasicResponse<List<AlarmProcessInfo>> resultResponse = new BasicResponse<List<AlarmProcessInfo>>();

            int pageindex = AlarmRecordRequest.PagerInfo.PageIndex;
            int pagesize = AlarmRecordRequest.PagerInfo.PageSize;

            try
            {
                DateTime stime = new DateTime();
                DateTime.TryParse(AlarmRecordRequest.Stime, out stime);
                DateTime etime = new DateTime();
                DateTime.TryParse(AlarmRecordRequest.ETime, out etime); 
                List<AlarmProcessInfo> alarmRecordList = new List<AlarmProcessInfo>();
                DataTable alarmTable = new DataTable();
                alarmTable = _Repository.QueryTable("global_GetStationInterruptAlarmRecordByStime", new object[] { "KJ_DataAlarm" + stime.ToString("yyyyMM"), etime, stime });
                alarmRecordList = _Repository.ToEntityFromTable<AlarmProcessInfo>(alarmTable).ToList();
                resultResponse.Data = alarmRecordList;
                return resultResponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取分站中断记录失败：" + "\r\n" + ex.Message);
                resultResponse.Data = new List<AlarmProcessInfo>();
                return resultResponse;
            }
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

        public BasicResponse<Jc_BInfo> GetDateAlarmRecordById(AlarmRecordGetDateIdRequest AlarmRecordRequest)
        {
            var alarmTable = _Repository.QueryTable("global_GetDateAlarmRecordById", new object[] { "KJ_DataAlarm" + AlarmRecordRequest.AlarmDate, AlarmRecordRequest.Id });

            BasicResponse<Jc_BInfo> response = new BasicResponse<Jc_BInfo>();
            if (alarmTable != null && alarmTable.Rows.Count > 0)
            {
                var alarmList = _Repository.ToEntityFromTable<Jc_BInfo>(alarmTable);
                response.Data = alarmList.FirstOrDefault();
            }

            return response;
        }

        public BasicResponse<bool> UpdateDateAlarmRecord(AlarmRecordUpdateDateRequest AlarmRecordRequest)
        {
            List<Jc_BInfo> alarmInfos = new List<Jc_BInfo>();
            string alarmDate = AlarmRecordRequest.AlarmInfo.Stime.ToString("yyyyMM");
            alarmInfos.Add(AlarmRecordRequest.AlarmInfo);

            var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarmInfos);
            BasicResponse<bool> resultResponse = new BasicResponse<bool>();
            if (_Repository.BulkUpdate("KJ_DataAlarm" + alarmDate, alarmModels, BuildDataColumn(columns), "ID"))
            {

                resultResponse.Data = true;
                //更新缓存
                var updateRequest = new AlarmCacheUpdateRequest
                {
                    AlarmInfo = AlarmRecordRequest.AlarmInfo
                };
                _alarmCacheService.UpdateAlarmCahce(updateRequest);
            }
            else
            {
                resultResponse.Data = false;
            }

            return resultResponse;
        }

        public BasicResponse<bool> UpdateAlarmInfoProperties(AlarmRecordUpdateProperitesRequest AlarmRecordRequest)
        {
            BasicResponse<bool> resultResponse = new BasicResponse<bool>();
            try
            {
                List<Jc_BInfo> alarmInfos = new List<Jc_BInfo>();
                string alarmDate = AlarmRecordRequest.AlarmInfo.Stime.ToString("yyyyMM");
                alarmInfos.Add(AlarmRecordRequest.AlarmInfo);

                var alarmModels = ObjectConverter.CopyList<Jc_BInfo, Jc_BModel>(alarmInfos);
                List<string> updateItems = GetUpdateColumns(AlarmRecordRequest.UpdateItems);
                updateItems.Add("ID");
                string[] updateColumns = updateItems.ToArray();
                if (updateColumns.Length > 0)
                {
                    if (_Repository.BulkUpdate("KJ_DataAlarm" + alarmDate, alarmModels, BuildDataColumn(updateColumns), "ID"))
                    {
                        resultResponse.Data = true;
                        //更新缓存
                        var updateRequest = new AlarmCacheUpdatePropertiesRequest
                        {
                            AlarmKey = AlarmRecordRequest.AlarmInfo.ID,
                            UpdateItems = AlarmRecordRequest.UpdateItems
                        };
                        _alarmCacheService.UpdateAlarmInfoProperties(updateRequest);

                        return resultResponse;
                    }
                }

                resultResponse.Data = false;
                return resultResponse;
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新报警信息失败：" + "\r\n" + ex.Message);
                resultResponse.Data = false;
                return resultResponse;
            }
        }

        private List<string> GetUpdateColumns(Dictionary<string, object> updateItems)
        {
            List<string> updatecolumns = new List<string>();
            foreach (var item in updateItems)
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    updatecolumns.Add(item.Key);
                }
            }

            return updatecolumns;
            //return updatecolumns.ToArray();
        }

        /// <summary>
        /// 分页获取DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        private DataTable GetPagedTable(DataTable dt, int PageIndex, int PageSize)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
    }
}


