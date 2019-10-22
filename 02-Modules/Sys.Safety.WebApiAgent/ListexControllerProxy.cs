using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApiAgent
{
    public class ListexControllerProxy : BaseProxy, IListexService
    {
        private static string _dbType = null;

        private static string _dbName = null;

        public Basic.Framework.Web.BasicResponse<DataContract.ListexInfo> AddListex(Sys.Safety.Request.Listex.ListexAddRequest listexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/AddListex?token=" + Token, JSONHelper.ToJSONString(listexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListexInfo> UpdateListex(Sys.Safety.Request.Listex.ListexUpdateRequest listexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/UpdateListex?token=" + Token, JSONHelper.ToJSONString(listexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteListex(Sys.Safety.Request.Listex.ListexDeleteRequest listexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/DeleteListex?token=" + Token, JSONHelper.ToJSONString(listexrequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<List<DataContract.ListexInfo>> GetListexList(Sys.Safety.Request.Listex.ListexGetListRequest listexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetListexList?token=" + Token, JSONHelper.ToJSONString(listexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DataContract.ListexInfo>>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListexInfo> GetListexById(Sys.Safety.Request.Listex.ListexGetRequest listexrequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetListexById?token=" + Token, JSONHelper.ToJSONString(listexrequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListexInfo> SaveList(Sys.Safety.Request.Listex.SaveListRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveList?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<ListexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<DataContract.ListdataexInfo> SaveList(Sys.Safety.Request.Listex.SaveList2Request request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveList2?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<ListdataexInfo>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteList(Sys.Safety.Request.Listex.IdRequest listId)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/DeleteList?token=" + Token, JSONHelper.ToJSONString(listId));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaData()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetServerMetaData?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaData(Sys.Safety.Request.Listex.IdRequest ID)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetServerMetaData2?token=" + Token, JSONHelper.ToJSONString(ID));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerMetaDataFields()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetServerMetaDataFields?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetServerRequest()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetServerRequest?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse DeleteSchema(Sys.Safety.Request.Listex.DeleteSchemaRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/DeleteSchema?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SetDefaultSchema(Sys.Safety.Request.Listex.SetDefaultSchemaRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SetDefaultSchema?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListDataEx(Sys.Safety.Request.Listex.SaveListDataExRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveListDataEx?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<string> GetSQL(Sys.Safety.Request.Listex.GetSQLRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetSQL?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse UpdateMetaDataCache()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/UpdateMetaDataCache?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<int> GetTotalRecord(Sys.Safety.Request.Listex.SqlRequest strSql)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetTotalRecord?token=" + Token, JSONHelper.ToJSONString(strSql));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetPageData(Sys.Safety.Request.Listex.SqlRequest sql)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetPageData?token=" + Token, JSONHelper.ToJSONString(sql));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListMetaData(Sys.Safety.Request.Listex.SaveListMetaDataRequest lmdList)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveListMetaData?token=" + Token, JSONHelper.ToJSONString(lmdList));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<System.Data.DataTable> GetFieldRights(Sys.Safety.Request.Listex.IdRequest userId)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetFieldRights?token=" + Token, JSONHelper.ToJSONString(userId));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse SaveListDisplayEx(Sys.Safety.Request.Listex.SaveListDisplayExRequest ldList)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveListDisplayEx?token=" + Token, JSONHelper.ToJSONString(ldList));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse ImportReport(Sys.Safety.Request.Listex.ObjectRequest o)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/ImportReport?token=" + Token, JSONHelper.ToJSONString(o));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public Basic.Framework.Web.BasicResponse<string> GetDBType()
        {
            if (_dbType != null)
            {
                var res = new BasicResponse<string>()
                {
                    Data = _dbType
                };
                return res;
            }

            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetDBType?token=" + Token, JSONHelper.ToJSONString(new {Name = "123"}));
            var ret = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            if (ret.IsSuccess)
            {
                _dbType = ret.Data;
            }
            return ret;
        }

        public Basic.Framework.Web.BasicResponse<string> GetDBName()
        {
            if (_dbName != null)
            {
                var res = new BasicResponse<string>()
                {
                    Data = _dbName
                };
                return res;
            }

            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/GetDBName?token=" + Token, "");
            var ret = JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
            if (ret.IsSuccess)
            {
                _dbName = ret.Data;
            }
            return ret;
        }

        public Basic.Framework.Web.BasicResponse SaveListExInfo(Sys.Safety.Request.Listex.SaveListExInfoRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Listex/SaveListExInfo?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
