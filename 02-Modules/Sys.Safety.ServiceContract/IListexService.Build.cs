using System.Collections.Generic;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract
{
    public interface IListexService
    {
        BasicResponse<ListexInfo> AddListex(ListexAddRequest listexrequest);
        BasicResponse<ListexInfo> UpdateListex(ListexUpdateRequest listexrequest);
        BasicResponse DeleteListex(ListexDeleteRequest listexrequest);
        BasicResponse<List<ListexInfo>> GetListexList(ListexGetListRequest listexrequest);
        BasicResponse<ListexInfo> GetListexById(ListexGetRequest listexrequest);

        BasicResponse<ListexInfo> SaveList(SaveListRequest request);

        BasicResponse<ListdataexInfo> SaveList(SaveList2Request request);

        BasicResponse DeleteList(IdRequest listId);

        BasicResponse<DataTable> GetServerMetaData();

        BasicResponse<DataTable> GetServerMetaData(IdRequest ID);

        BasicResponse<DataTable> GetServerMetaDataFields();

        BasicResponse<DataTable> GetServerRequest();

        BasicResponse DeleteSchema(DeleteSchemaRequest request);

        BasicResponse SetDefaultSchema(SetDefaultSchemaRequest request);

        BasicResponse SaveListDataEx(SaveListDataExRequest request);

        BasicResponse<string> GetSQL(GetSQLRequest request);

        BasicResponse UpdateMetaDataCache();

        BasicResponse<int> GetTotalRecord(SqlRequest strSql);

        BasicResponse<DataTable> GetPageData(SqlRequest sql);

        BasicResponse SaveListMetaData(SaveListMetaDataRequest lmdList);

        BasicResponse<DataTable> GetFieldRights(IdRequest userId);

        BasicResponse SaveListDisplayEx(SaveListDisplayExRequest ldList);

        BasicResponse ImportReport(ObjectRequest o);

        BasicResponse<string> GetDBType();

        BasicResponse<string> GetDBName();

        BasicResponse SaveListExInfo(SaveListExInfoRequest request);
    }
}