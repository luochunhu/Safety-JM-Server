using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Metadata;
using Sys.Safety.ServiceContract;
using Sys.Safety.Reports;
using Basic.Framework.Configuration;

namespace Sys.Safety.Services
{
    public class MetadataService : IMetadataService
    {
        private readonly IMetadataRepository _Repository;
        private readonly IMetadatafieldsRepository _metadatafieldsRepository;
        private readonly RepositoryBase<MetadataModel> _metadataRepositoryBase;

        public MetadataService(IMetadataRepository _Repository, IMetadatafieldsRepository metadatafieldsRepository)
        {
            this._Repository = _Repository;
            this._metadatafieldsRepository = metadatafieldsRepository;
            _metadataRepositoryBase = _Repository as RepositoryBase<MetadataModel>;
        }

        public BasicResponse<MetadataInfo> AddMetadata(MetadataAddRequest metadatarequest)
        {
            var _metadata = ObjectConverter.Copy<MetadataInfo, MetadataModel>(metadatarequest.MetadataInfo);
            var resultmetadata = _Repository.AddMetadata(_metadata);
            var metadataresponse = new BasicResponse<MetadataInfo>();
            metadataresponse.Data = ObjectConverter.Copy<MetadataModel, MetadataInfo>(resultmetadata);
            return metadataresponse;
        }

        public BasicResponse<MetadataInfo> UpdateMetadata(MetadataUpdateRequest metadatarequest)
        {
            var _metadata = ObjectConverter.Copy<MetadataInfo, MetadataModel>(metadatarequest.MetadataInfo);
            _Repository.UpdateMetadata(_metadata);
            var metadataresponse = new BasicResponse<MetadataInfo>();
            metadataresponse.Data = ObjectConverter.Copy<MetadataModel, MetadataInfo>(_metadata);
            return metadataresponse;
        }

        public BasicResponse DeleteMetadata(MetadataDeleteRequest metadatarequest)
        {
            _Repository.DeleteMetadata(metadatarequest.Id);
            var metadataresponse = new BasicResponse();
            return metadataresponse;
        }

        public BasicResponse<List<MetadataInfo>> GetMetadataList(MetadataGetListRequest metadatarequest)
        {
            var metadataresponse = new BasicResponse<List<MetadataInfo>>();
            metadatarequest.PagerInfo.PageIndex = metadatarequest.PagerInfo.PageIndex - 1;
            if (metadatarequest.PagerInfo.PageIndex < 0)
                metadatarequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var metadataModelLists = _Repository.GetMetadataList(metadatarequest.PagerInfo.PageIndex,
                metadatarequest.PagerInfo.PageSize, out rowcount);
            var metadataInfoLists = new List<MetadataInfo>();
            foreach (var item in metadataModelLists)
            {
                var MetadataInfo = ObjectConverter.Copy<MetadataModel, MetadataInfo>(item);
                metadataInfoLists.Add(MetadataInfo);
            }
            metadataresponse.Data = metadataInfoLists;
            return metadataresponse;
        }

        public BasicResponse<MetadataInfo> GetMetadataById(MetadataGetRequest metadatarequest)
        {
            var result = _Repository.GetMetadataById(metadatarequest.Id);
            var metadataInfo = ObjectConverter.Copy<MetadataModel, MetadataInfo>(result);
            var metadataresponse = new BasicResponse<MetadataInfo>();
            metadataresponse.Data = metadataInfo;
            return metadataresponse;
        }

        public BasicResponse ImportMetadata(ImportMetadataRequest request)
        {
            try
            {
                var dataset = request.Obj as DataSet;
                //this.GetDataTableBySQL("delete from BFT_MetaData    ");
                //this.GetDataTableBySQL("delete from BFT_MetaDataFields     ");
                _Repository.ExecuteNonQuery("global_MetadataService_DeleteMetaDataAll");
                _Repository.ExecuteNonQuery("global_MetadataService_DeleteMetaDataFieldsAll");

                var dtmetadata = new DataTable("BFT_MetaData");
                dtmetadata = dataset.Tables["BFT_MetaData"];
                var dtmetadatafiles = new DataTable("BFT_MetaDataFields");
                dtmetadatafiles = dataset.Tables["BFT_MetaDataFields"];

                var strsql = GetInsertSql("BFT_MetaDataFields", dtmetadatafiles);
                //this.GetDataTableBySQL(strsql + " select  * from BFT_FKLib");
                _metadataRepositoryBase.ExecuteNonQueryBySql(strsql);
                strsql = GetInsertSql("BFT_MetaData", dtmetadata);
                //this.GetDataTableBySQL(strsql + " select  * from BFT_FKLib");
                _metadataRepositoryBase.ExecuteNonQueryBySql(strsql);

                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("导入元数据", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        /// <summary>
        ///     插入数据库的通用方法
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetInsertSql(string strTableName, DataTable dt)
        {
            var dbType = Global.DatabaseType.ToString().ToLower();
            var strinsertmetadata = "";
            var strvaluesmeetadata = "";
            var strsql = " set IDENTITY_Insert " + strTableName + " on "; //设置后标识列才能插入值
            if (dbType == "mysql")
                strsql = "";
            for (var i = 0; i < dt.Rows.Count; ++i)
            {
                strinsertmetadata = "insert into " + strTableName + "(";
                strvaluesmeetadata = " values(";
                foreach (DataColumn column in dt.Columns)
                {
                    strinsertmetadata += column.ColumnName + ",";
                    if ((column.DataType.Name == "Boolean") || column.DataType.Name.Contains("Int"))
                        strvaluesmeetadata += TypeUtil.ToInt(dt.Rows[i][column]) + ",";
                    else
                        strvaluesmeetadata += "'" + dt.Rows[i][column] + "',";
                }

                strinsertmetadata = strinsertmetadata.Substring(0, strinsertmetadata.Length - 1) + " )";
                strvaluesmeetadata = strvaluesmeetadata.Substring(0, strvaluesmeetadata.Length - 1) + ")  ";
                strsql += strinsertmetadata + strvaluesmeetadata;
                if (dbType == "mysql")
                    strsql += ";";
            }
            if (dbType != "mysql")
                strsql += " set IDENTITY_Insert BFT_MetaDataFields off ";
            return strsql;
        }

        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error(strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            //switch (this._baseDAO.getServerType())
            //{
            //    case "local": //local
            //        throw ex;
            //        break;
            //    case "wcf": //wcf
            //        throw new FaultException(strTiTle + "出错\n" + ex.Message);
            //        break;
            //    default:
            //        throw ex;
            //        break;
            //}
            throw ex;
        }
    }
}