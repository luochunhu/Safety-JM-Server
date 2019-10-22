using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract.Chart;
using Basic.Framework.Web;
using Basic.Framework.Common;
using System.Data;
using Sys.Safety.Request.Chart;

namespace Sys.Safety.WebApi.Chart
{
    public class ChartController : Basic.Framework.Web.WebApi.BasicApiController, IChartService
    {
        IChartService _chartService = ServiceFactory.Create<IChartService>();

        [HttpPost]
        [Route("v1/Chart/GetDBType")]
        public Basic.Framework.Web.BasicResponse<string> GetDBType()
        {
            return _chartService.GetDBType();
        }

        [HttpPost]
        [Route("v1/Chart/GetLastUpdateRealTime")]
        public Basic.Framework.Web.BasicResponse<string> GetLastUpdateRealTime()
        {
            return _chartService.GetLastUpdateRealTime();
        }

        [HttpPost]
        [Route("v1/Chart/QueryAllPointCache")]
        public Basic.Framework.Web.BasicResponse<IList<Sys.Safety.DataContract.Jc_DefInfo>> QueryAllPointCache()
        {
            return _chartService.QueryAllPointCache();
        }
        [HttpPost]
        [Route("v1/Chart/QueryPointCacheByDevpropertID")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_DefInfo>> QueryPointCacheByDevpropertID(GetPointCacheByDevpropertIDRequest request)
        {
            return _chartService.QueryPointCacheByDevpropertID(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetDBName")]
        public Basic.Framework.Web.BasicResponse<string> GetDBName()
        {
            return _chartService.GetDBName();
        }

        [HttpPost]
        [Route("v1/Chart/GetPointList")]
        public Basic.Framework.Web.BasicResponse<string> GetPointList(Sys.Safety.Request.Chart.GetPointListRequest request)
        {
            var res = _chartService.GetPointList(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetPointKzList")]
        public Basic.Framework.Web.BasicResponse<string> GetPointKzList(Sys.Safety.Request.Chart.GetPointKzListRequest request)
        {
            var res = _chartService.GetPointKzList(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetPointDw")]
        public Basic.Framework.Web.BasicResponse<string> GetPointDw(Sys.Safety.Request.Listex.IdRequest request)
        {
            return _chartService.GetPointDw(request);
        }

        [HttpPost]
        [Route("v1/Chart/ShowPointInf")]
        public Basic.Framework.Web.BasicResponse<string[]> ShowPointInf(Sys.Safety.Request.Chart.ShowPointInfRequest request)
        {
            return _chartService.ShowPointInf(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetZFromTable")]
        public Basic.Framework.Web.BasicResponse<List<float>> GetZFromTable(Sys.Safety.Request.Listex.PointIdRequest request)
        {
            return _chartService.GetZFromTable(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetKglStateDev")]
        public Basic.Framework.Web.BasicResponse<List<string>> GetKglStateDev(Sys.Safety.Request.Listex.PointIdRequest request)
        {
            return _chartService.GetKglStateDev(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetMonthLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMonthLine(Sys.Safety.Request.Chart.GetMonthLineRequest request)
        {
            var res = _chartService.GetMonthLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetFiveMiniteLine")]
        public Basic.Framework.Web.BasicResponse<string> GetFiveMiniteLine(Sys.Safety.Request.Chart.GetFiveMiniteLineRequest request)
        {
            var res = _chartService.GetFiveMiniteLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetDataVale")]
        public Basic.Framework.Web.BasicResponse<string[]> GetDataVale(Sys.Safety.Request.Chart.GetDataValeRequest request)
        {
            return _chartService.GetDataVale(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetValue")]
        public Basic.Framework.Web.BasicResponse<string[]> GetValue(Sys.Safety.Request.Chart.GetValueRequest request)
        {
            return _chartService.GetValue(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetStateBarTable")]
        public Basic.Framework.Web.BasicResponse<string> GetStateBarTable(Sys.Safety.Request.Chart.GetStateBarTableRequest request)
        {
            var res = _chartService.GetStateBarTable(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetStateChgdt")]
        public Basic.Framework.Web.BasicResponse<string> GetStateChgdt(Sys.Safety.Request.Chart.GetStateChgdtRequest request)
        {
            var res = _chartService.GetStateChgdt(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetStateLineDt")]
        public Basic.Framework.Web.BasicResponse<string> GetStateLineDt(Sys.Safety.Request.Chart.GetStateLineDtRequest request)
        {
            var res = _chartService.GetStateLineDt(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetMnlBjLineDt")]
        public Basic.Framework.Web.BasicResponse<string> GetMnlBjLineDt(Sys.Safety.Request.Chart.GetMnlBjLineDtRequest request)
        {
            var res = _chartService.GetMnlBjLineDt(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetKzlLineDt")]
        public Basic.Framework.Web.BasicResponse<string> GetKzlLineDt(Sys.Safety.Request.Chart.GetKzlLineDtRequest request)
        {
            var res = _chartService.GetKzlLineDt(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetKzlStateLineDt")]
        public Basic.Framework.Web.BasicResponse<string> GetKzlStateLineDt(GetStateLineDtRequest request)
        {
            var res = _chartService.GetKzlStateLineDt(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetDgView")]
        public Basic.Framework.Web.BasicResponse<string[]> GetDgView(Sys.Safety.Request.Chart.GetDgViewRequest request)
        {
            return _chartService.GetDgView(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetKjThings")]
        public Basic.Framework.Web.BasicResponse<string[]> GetKjThings(Sys.Safety.Request.Chart.GetKjThingsRequest request)
        {
            return _chartService.GetKjThings(request);
        }

        [HttpPost]
        [Route("v1/Chart/InitQxZhuZhuang")]
        public Basic.Framework.Web.BasicResponse<string> InitQxZhuZhuang(Sys.Safety.Request.Chart.InitQxZhuZhuangRequest request)
        {
            var res = _chartService.InitQxZhuZhuang(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetMcData")]
        public Basic.Framework.Web.BasicResponse<Dictionary<string, string>> GetMcData(Sys.Safety.Request.Chart.GetMcDataRequest request)
        {
            Dictionary<string, string> McData = new Dictionary<string, string>();
            var res = _chartService.GetMcData(request);
            foreach (KeyValuePair<string, DataTable> kvp in res.Data)
            {
                McData.Add(kvp.Key, ObjectConverter.ToBase64String(kvp.Value));
            }
            return new BasicResponse<Dictionary<string, string>>()
            {
                Data = McData
            };
        }

        [HttpPost]
        [Route("v1/Chart/GetMonthBar")]
        public Basic.Framework.Web.BasicResponse<string> GetMonthBar(Sys.Safety.Request.Chart.GetMonthBarRequest request)
        {
            var res = _chartService.GetMonthBar(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };            
        }

        [HttpPost]
        [Route("v1/Chart/GetMLLFiveLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMLLFiveLine(Sys.Safety.Request.Chart.GetMLLFiveLineRequest request)
        {
            var res = _chartService.GetMLLFiveLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };   
        }

        [HttpPost]
        [Route("v1/Chart/GetMHourLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMHourLine(Sys.Safety.Request.Chart.GetMHourLineRequest request)
        {
            var res = _chartService.GetMHourLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };   
        }

        [HttpPost]
        [Route("v1/Chart/GetMLLFiveGirdData")]
        public Basic.Framework.Web.BasicResponse<string> GetMLLFiveGirdData(Sys.Safety.Request.Chart.GetMLLFiveGirdDataRequest request)
        {
            var res = _chartService.GetMLLFiveGirdData(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetMLLMonthBar")]
        public Basic.Framework.Web.BasicResponse<string> GetMLLMonthBar(Sys.Safety.Request.Chart.GetMLLMonthBarRequest request)
        {
            var res = _chartService.GetMLLMonthBar(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetMLLMCLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMLLMCLine(Sys.Safety.Request.Chart.GetMLLMCLineRequest request)
        {
            var res = _chartService.GetMLLMCLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetKGLLState")]
        public Basic.Framework.Web.BasicResponse<string> GetKGLLState(Sys.Safety.Request.Chart.GetKGLLStateRequest request)
        {
            var res = _chartService.GetKGLLState(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetKGLStateGirdData")]
        public Basic.Framework.Web.BasicResponse<string> GetKGLStateGirdData(Sys.Safety.Request.Chart.GetKGLStateGirdDataRequest request)
        {
            var res = _chartService.GetKGLStateGirdData(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetKJXL")]
        public Basic.Framework.Web.BasicResponse<string[]> GetKJXL(Sys.Safety.Request.Chart.GetKJXLRequest request)
        {
            return _chartService.GetKJXL(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetKGLStateGridDataByRight")]
        public Basic.Framework.Web.BasicResponse<string> GetKGLStateGridDataByRight(Sys.Safety.Request.Chart.GetKGLStateGridDataByRightRequest request)
        {
            var res = _chartService.GetKGLStateGridDataByRight(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetMnlBJLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMnlBJLine(Sys.Safety.Request.Chart.GetMnlBJLineRequest request)
        {
            var res = _chartService.GetMnlBJLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetMnlDDLine")]
        public Basic.Framework.Web.BasicResponse<string> GetMnlDDLine(Sys.Safety.Request.Chart.GetMnlDDLineRequest request)
        {
            var res = _chartService.GetMnlDDLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetPointKzByPointID")]
        public Basic.Framework.Web.BasicResponse<string> GetPointKzByPointID(Sys.Safety.Request.Chart.GetPointKzByPointIDRequest request)
        {
            var res = _chartService.GetPointKzByPointID(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            };  
        }

        [HttpPost]
        [Route("v1/Chart/GetKzlLine")]
        public Basic.Framework.Web.BasicResponse<string> GetKzlLine(Sys.Safety.Request.Chart.GetKzlLineRequest request)
        {
            var res = _chartService.GetKzlLine(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            }; 
        }

        [HttpPost]
        [Route("v1/Chart/GetPointByType")]
        public Basic.Framework.Web.BasicResponse<string> GetPointByType(Sys.Safety.Request.Chart.GetPointByTypeRequest request)
        {
            var res = _chartService.GetPointByType(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            }; 
        }

        [HttpPost]
        [Route("v1/Chart/SaveChartSet")]
        public Basic.Framework.Web.BasicResponse<bool> SaveChartSet(Sys.Safety.Request.Chart.SaveChartSetRequest request)
        {
            return _chartService.SaveChartSet(request);
        }

        [HttpPost]
        [Route("v1/Chart/GetAllChartSet")]
        public Basic.Framework.Web.BasicResponse<string> GetAllChartSet(Sys.Safety.Request.Chart.GetAllChartSetRequest request)
        {
            var res = _chartService.GetAllChartSet(request);
            return new BasicResponse<string>()
            {
                Data = ObjectConverter.ToBase64String(res.Data)
            }; 
        }


        BasicResponse<System.Data.DataTable> IChartService.GetPointList(Sys.Safety.Request.Chart.GetPointListRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetPointKzList(Sys.Safety.Request.Chart.GetPointKzListRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMonthLine(Sys.Safety.Request.Chart.GetMonthLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetFiveMiniteLine(Sys.Safety.Request.Chart.GetFiveMiniteLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<Sys.Safety.Request.Chart.GetStateBarTableResponse> IChartService.GetStateBarTable(Sys.Safety.Request.Chart.GetStateBarTableRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetStateChgdt(Sys.Safety.Request.Chart.GetStateChgdtRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetStateLineDt(Sys.Safety.Request.Chart.GetStateLineDtRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMnlBjLineDt(Sys.Safety.Request.Chart.GetMnlBjLineDtRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetKzlLineDt(Sys.Safety.Request.Chart.GetKzlLineDtRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.InitQxZhuZhuang(Sys.Safety.Request.Chart.InitQxZhuZhuangRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<Dictionary<string, System.Data.DataTable>> IChartService.GetMcData(Sys.Safety.Request.Chart.GetMcDataRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMonthBar(Sys.Safety.Request.Chart.GetMonthBarRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMLLFiveLine(Sys.Safety.Request.Chart.GetMLLFiveLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMHourLine(Sys.Safety.Request.Chart.GetMHourLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMLLFiveGirdData(Sys.Safety.Request.Chart.GetMLLFiveGirdDataRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMLLMonthBar(Sys.Safety.Request.Chart.GetMLLMonthBarRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMLLMCLine(Sys.Safety.Request.Chart.GetMLLMCLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetKGLLState(Sys.Safety.Request.Chart.GetKGLLStateRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetKGLStateGirdData(Sys.Safety.Request.Chart.GetKGLStateGirdDataRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetKGLStateGridDataByRight(Sys.Safety.Request.Chart.GetKGLStateGridDataByRightRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMnlBJLine(Sys.Safety.Request.Chart.GetMnlBJLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetMnlDDLine(Sys.Safety.Request.Chart.GetMnlDDLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetPointKzByPointID(Sys.Safety.Request.Chart.GetPointKzByPointIDRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetKzlLine(Sys.Safety.Request.Chart.GetKzlLineRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetPointByType(Sys.Safety.Request.Chart.GetPointByTypeRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<System.Data.DataTable> IChartService.GetAllChartSet(Sys.Safety.Request.Chart.GetAllChartSetRequest request)
        {
            throw new NotImplementedException();
        }

        BasicResponse<DataTable> IChartService.GetKzlStateLineDt(GetStateLineDtRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
