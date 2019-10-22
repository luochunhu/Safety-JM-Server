using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;

using Sys.Safety.ServiceContract;
using Sys.Safety.Model;

namespace Sys.Safety.Reports.Service
{
    public class ServerDataCache
    {
        private static IDictionary<string, DataTable> dicCache = new Dictionary<string, DataTable>();
        private static readonly IMetadataRepository MetadataRepository = ServiceFactory.Create<IMetadataRepository>();
        private static readonly IMetadatafieldsRepository MetadatafieldsRepository = ServiceFactory.Create<IMetadatafieldsRepository>();
        private static readonly IRequestRepository RequestRepository = ServiceFactory.Create<IRequestRepository>();

        /// <summary>
        /// 得到元数据缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMetaData()
        {
            DataTable dt = null;
            if (dicCache.ContainsKey("metadata"))
            {
                dt = dicCache["metadata"];
            }
            else
            {
                //dt = MetadataService.GetDataTableBySQL("select * from BFT_MetaData");
                var model = MetadataRepository.GetMetadataListAll();
                dt = ObjectConverter.ToDataTable(model);
                dicCache.Add("metadata", dt);
            }
            return dt;
        }

        /// <summary>
        /// 根据元数据ID得到元数据缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMetaData(int ID)
        {
            DataTable dt = null;
            if (dicCache.ContainsKey("metadata"))
            {
                dt = dicCache["metadata"];
            }
            else
            {
                //dt = service.GetDataTableBySQL("select * from BFT_MetaData");
                var model = MetadataRepository.GetMetadataListAll();
                dt = ObjectConverter.ToDataTable(model);
                dicCache.Add("metadata", dt);
            }
            DataView view = dt.DefaultView;
            view.RowFilter = "ID=" + ID;
            dt = view.ToTable();
            return dt;

        }


        /// <summary>
        /// 得到元数据字段缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMetaDataFields()
        {
            DataTable dt = null;
            if (dicCache.ContainsKey("metadatafields"))
            {
                dt = dicCache["metadatafields"];
            }
            else
            {
                //dt = service.GetDataTableBySQL("select * from BFT_MetaDataFields");
                var model = MetadatafieldsRepository.GetMetadatafieldsListAll();
                dt = ObjectConverter.ToDataTable(model);
                dicCache.Add("metadatafields", dt);
            }
            return dt;

        }

        /// <summary>
        /// 得到元数据缓存
        /// </summary>
        /// <returns></returns>
        public static void UpdateMetaDataCache()
        {
            //DataTable dt = service.GetDataTableBySQL("select * from BFT_MetaData");
            var model = MetadataRepository.GetMetadataListAll();
            DataTable dt = ObjectConverter.ToDataTable(model);
            if (dicCache.ContainsKey("metadata"))
            {
                dicCache.Remove("metadata");
            }
            dicCache.Add("metadata", dt);

            //dt = service.GetDataTableBySQL("select * from BFT_MetaDataFields");
            var model2 = MetadatafieldsRepository.GetMetadatafieldsListAll();
            dt = ObjectConverter.ToDataTable(model2);
            if (dicCache.ContainsKey("metadatafields"))
            {
                dicCache.Remove("metadatafields");
            }
            dicCache.Add("metadatafields", dt);
        }

        /// <summary>
        /// 加载用户字段权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dict"></param>
        public static DataTable InitFieldRights(int userId)
        {
            DataTable dt = null;
            if (dicCache.ContainsKey("FieldRight"))
            {
                dt = dicCache["metadatafields"];
            }
            else
            {
                IList<string> list = new List<string>();
                //typeid 0表示角色  1表示操作员
                //取操作员独立字段权限
                //string sql = "select distinct metadatafieldid from UserFieldRight where typeid=1  and  operatorid=" + userId;
                //dt = service.GetDataTableBySQL(sql);
                dt = MetadataRepository.QueryTable("global_ServerDataCache_GetMetadatafieldid_ByOperatorid", userId);

                if (dt == null || dt.Rows.Count <= 0)
                {   //如果操作员未设置字段权限，取操作员对应角色字段权限
                    //sql = "select distinct metadatafieldid from UserFieldRight where typeid=0 and operatorid=(select RoleID from operator where operatorid=" + userId + ")";
                    //dt = service.GetDataTableBySQL(sql);
                    dt = MetadataRepository.QueryTable("global_ServerDataCache_GetMetadatafieldid_ByOperatorid2", userId);
                }
            }

            return dt;
        }

        /// <summary>
        /// 得到请求库缓存
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRequest()
        {
            DataTable dt = null;
            //dt = service.GetDataTableBySQL("select * from BFT_Request");
            var model = RequestRepository.GetRequestListAll();
            dt = ObjectConverter.ToDataTable(model);
            return dt;
        }
    }
}
