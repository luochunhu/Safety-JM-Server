using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class ListexController : Basic.Framework.Web.WebApi.BasicApiController, IListexService
    {
        IListexService _service = ServiceFactory.Create<IListexService>();

        ListexController()
        {
            
        }

        [HttpPost]
        [Route("v1/Listex/AddListex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListexInfo> AddListex(Sys.Safety.Request.Listex.ListexAddRequest listexrequest)
        {
            return _service.AddListex(listexrequest);
        }

        [HttpPost]
        [Route("v1/Listex/UpdateListex")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListexInfo> UpdateListex(Sys.Safety.Request.Listex.ListexUpdateRequest listexrequest)
        {
            return _service.UpdateListex(listexrequest);
        }

        [HttpPost]
        [Route("v1/Listex/DeleteListex")]
        public Basic.Framework.Web.BasicResponse DeleteListex(Sys.Safety.Request.Listex.ListexDeleteRequest listexrequest)
        {
            return _service.DeleteListex(listexrequest);
        }

        [HttpPost]
        [Route("v1/Listex/GetListexList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.ListexInfo>> GetListexList(Sys.Safety.Request.Listex.ListexGetListRequest listexrequest)
        {
            return _service.GetListexList(listexrequest);
        }

        [HttpPost]
        [Route("v1/Listex/GetListexById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListexInfo> GetListexById(Sys.Safety.Request.Listex.ListexGetRequest listexrequest)
        {
            return _service.GetListexById(listexrequest);
        }

        [HttpPost]
        [Route("v1/Listex/SaveList")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListexInfo> SaveList(Sys.Safety.Request.Listex.SaveListRequest request)
        {
            return _service.SaveList(request);
        }

        [HttpPost]
        [Route("v1/Listex/SaveList2")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.ListdataexInfo> SaveList(Sys.Safety.Request.Listex.SaveList2Request request)
        {
            return _service.SaveList(request);
        }

        [HttpPost]
        [Route("v1/Listex/DeleteList")]
        public Basic.Framework.Web.BasicResponse DeleteList(Sys.Safety.Request.Listex.IdRequest listId)
        {
            return _service.DeleteList(listId);
        }

        [HttpPost]
        [Route("v1/Listex/GetServerMetaData")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaData()
        {
            return _service.GetServerMetaData();
        }

        [HttpPost]
        [Route("v1/Listex/GetServerMetaData2")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaData(Sys.Safety.Request.Listex.IdRequest ID)
        {
            return _service.GetServerMetaData(ID);
        }

        [HttpPost]
        [Route("v1/Listex/GetServerMetaDataFields")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaDataFields()
        {
            return _service.GetServerMetaDataFields();
        }

        [HttpPost]
        [Route("v1/Listex/GetServerRequest")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerRequest()
        {
            return _service.GetServerRequest();
        }

        [HttpPost]
        [Route("v1/Listex/DeleteSchema")]
        public Basic.Framework.Web.BasicResponse DeleteSchema(Sys.Safety.Request.Listex.DeleteSchemaRequest request)
        {
            return _service.DeleteSchema(request);
        }

        [HttpPost]
        [Route("v1/Listex/SetDefaultSchema")]
        public Basic.Framework.Web.BasicResponse SetDefaultSchema(Sys.Safety.Request.Listex.SetDefaultSchemaRequest request)
        {
            return _service.SetDefaultSchema(request);
        }

        [HttpPost]
        [Route("v1/Listex/SaveListDataEx")]
        public Basic.Framework.Web.BasicResponse SaveListDataEx(Sys.Safety.Request.Listex.SaveListDataExRequest request)
        {
            return _service.SaveListDataEx(request);
        }

        [HttpPost]
        [Route("v1/Listex/GetSQL")]
        public Basic.Framework.Web.BasicResponse<string> GetSQL(Sys.Safety.Request.Listex.GetSQLRequest request)
        {
            return _service.GetSQL(request);
        }

        [HttpPost]
        [Route("v1/Listex/UpdateMetaDataCache")]
        public Basic.Framework.Web.BasicResponse UpdateMetaDataCache()
        {
            return _service.UpdateMetaDataCache();
        }

        [HttpPost]
        [Route("v1/Listex/GetTotalRecord")]
        public Basic.Framework.Web.BasicResponse<int> GetTotalRecord(Sys.Safety.Request.Listex.SqlRequest strSql)
        {
            return _service.GetTotalRecord(strSql);
        }

        [HttpPost]
        [Route("v1/Listex/GetPageData")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetPageData(Sys.Safety.Request.Listex.SqlRequest sql)
        {
            return _service.GetPageData(sql);
        }

        [HttpPost]
        [Route("v1/Listex/SaveListMetaData")]
        public Basic.Framework.Web.BasicResponse SaveListMetaData(Sys.Safety.Request.Listex.SaveListMetaDataRequest lmdList)
        {
            return _service.SaveListMetaData(lmdList);
        }

        [HttpPost]
        [Route("v1/Listex/GetFieldRights")]
        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetFieldRights(Sys.Safety.Request.Listex.IdRequest userId)
        {
            return _service.GetFieldRights(userId);
        }

        [HttpPost]
        [Route("v1/Listex/SaveListDisplayEx")]
        public Basic.Framework.Web.BasicResponse SaveListDisplayEx(Sys.Safety.Request.Listex.SaveListDisplayExRequest ldList)
        {
            return _service.SaveListDisplayEx(ldList);
        }

        [HttpPost]
        [Route("v1/Listex/ImportReport")]
        public Basic.Framework.Web.BasicResponse ImportReport(Sys.Safety.Request.Listex.ObjectRequest o)
        {
            return _service.ImportReport(o);
        }

        [HttpPost]
        [Route("v1/Listex/GetDBType")]
        public Basic.Framework.Web.BasicResponse<string> GetDBType()
        {
            return _service.GetDBType();
        }

        [HttpPost]
        [Route("v1/Listex/GetDBName")]
        public Basic.Framework.Web.BasicResponse<string> GetDBName()
        {
            return _service.GetDBName();
        }

        [HttpPost]
        [Route("v1/Listex/SaveListExInfo")]
        public Basic.Framework.Web.BasicResponse SaveListExInfo(Sys.Safety.Request.Listex.SaveListExInfoRequest request)
        {
            return _service.SaveListExInfo(request);
        }
    }
}
