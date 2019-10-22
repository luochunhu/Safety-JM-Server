using System.Collections.Generic;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.WebApiAgent.Chart
{
    public class ChartControllerProxy : BaseProxy, IChartService
    {
        private static string _dbType = null;

        public BasicResponse<string> GetDBType()
        {
            if (_dbType != null)
            {
                var res = new BasicResponse<string>()
                {
                    Data = _dbType
                };
                return res;
            }

            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetDBType?token=" + Token, "");
            var ret = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            if (ret.IsSuccess)
            {
                _dbType = ret.Data;
            }
            return ret;
        }

        public BasicResponse<string> GetLastUpdateRealTime()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetLastUpdateRealTime?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }

        public BasicResponse<IList<Jc_DefInfo>> QueryAllPointCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/QueryAllPointCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<IList<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_DefInfo>> QueryPointCacheByDevpropertID(GetPointCacheByDevpropertIDRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/QueryPointCacheByDevpropertID?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_DefInfo>>>(responseStr);
        }

        public BasicResponse<string> GetDBName()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetDBName?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }

        public BasicResponse<DataTable> GetPointList(GetPointListRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetPointList?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetPointKzList(GetPointKzListRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetPointKzList?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<string> GetPointDw(IdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetPointDw?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }

        public BasicResponse<string[]> ShowPointInf(ShowPointInfRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/ShowPointInf?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<List<float>> GetZFromTable(PointIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetZFromTable?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<float>>>(responseStr);
        }

        public BasicResponse<List<string>> GetKglStateDev(PointIdRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKglStateDev?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<string>>>(responseStr);
        }

        public BasicResponse<DataTable> GetMonthLine(GetMonthLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMonthLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetFiveMiniteLine(GetFiveMiniteLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetFiveMiniteLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<string[]> GetDataVale(GetDataValeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetDataVale?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<string[]> GetValue(GetValueRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetValue?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<GetStateBarTableResponse> GetStateBarTable(GetStateBarTableRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetStateBarTable?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<GetStateBarTableResponse>(res.Data);
            return new BasicResponse<GetStateBarTableResponse>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetStateChgdt(GetStateChgdtRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetStateChgdt?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetStateLineDt(GetStateLineDtRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetStateLineDt?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMnlBjLineDt(GetMnlBjLineDtRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMnlBjLineDt?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetKzlLineDt(GetKzlLineDtRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKzlLineDt?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }
        public BasicResponse<DataTable> GetKzlStateLineDt(GetStateLineDtRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKzlStateLineDt?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<string[]> GetDgView(GetDgViewRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetDgView?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<string[]> GetKjThings(GetKjThingsRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKjThings?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<DataTable> InitQxZhuZhuang(InitQxZhuZhuangRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/InitQxZhuZhuang?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<Dictionary<string, DataTable>> GetMcData(GetMcDataRequest request)
        {
            Dictionary<string, DataTable> McData = new Dictionary<string, DataTable>();
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMcData?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<Dictionary<string, string>>>(responseStr);

            foreach (KeyValuePair<string, string> kvp in res.Data)
            {
                McData.Add(kvp.Key, ObjectConverter.FromBase64String<DataTable>(kvp.Value));
            }
            return new BasicResponse<Dictionary<string, DataTable>>
            {
                Data = McData
            };
        }

        public BasicResponse<DataTable> GetMonthBar(GetMonthBarRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMonthBar?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMLLFiveLine(GetMLLFiveLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMLLFiveLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMHourLine(GetMHourLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMHourLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMLLFiveGirdData(GetMLLFiveGirdDataRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMLLFiveGirdData?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMLLMonthBar(GetMLLMonthBarRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMLLMonthBar?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMLLMCLine(GetMLLMCLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMLLMCLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetKGLLState(GetKGLLStateRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKGLLState?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetKGLStateGirdData(GetKGLStateGirdDataRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKGLStateGirdData?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<string[]> GetKJXL(GetKJXLRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKJXL?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string[]>>(responseStr);
        }

        public BasicResponse<DataTable> GetKGLStateGridDataByRight(GetKGLStateGridDataByRightRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKGLStateGridDataByRight?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMnlBJLine(GetMnlBJLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMnlBJLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetMnlDDLine(GetMnlDDLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetMnlDDLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetPointKzByPointID(GetPointKzByPointIDRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetPointKzByPointID?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetKzlLine(GetKzlLineRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetKzlLine?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<DataTable> GetPointByType(GetPointByTypeRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetPointByType?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }

        public BasicResponse<bool> SaveChartSet(SaveChartSetRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/SaveChartSet?token=" + Token,
                JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<bool>>(responseStr);
        }

        public BasicResponse<DataTable> GetAllChartSet(GetAllChartSetRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Chart/GetAllChartSet?token=" + Token,
                JSONHelper.ToJSONString(request));
            var res = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            var resZh = ObjectConverter.FromBase64String<DataTable>(res.Data);
            return new BasicResponse<DataTable>
            {
                Data = resZh
            };
        }
    }
}