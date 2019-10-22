using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class MetadataRepository : RepositoryBase<MetadataModel>, IMetadataRepository
    {
        public MetadataModel AddMetadata(MetadataModel metadataModel)
        {
            return Insert(metadataModel);
        }

        public void UpdateMetadata(MetadataModel metadataModel)
        {
            Update(metadataModel);
        }

        public void DeleteMetadata(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<MetadataModel> GetMetadataList(int pageIndex, int pageSize, out int rowCount)
        {
            var metadataModelLists = Datas;
            rowCount = metadataModelLists.Count();
            return metadataModelLists.OrderBy(a => a.ID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public MetadataModel GetMetadataById(string id)
        {
            var iId = Convert.ToInt32(id);
            var metadataModel = Datas.FirstOrDefault(c => c.ID == iId);
            return metadataModel;
        }

        public IList<MetadataModel> GetMetadataListAll()
        {
            var model = Datas.ToList();
            return model;
        }
    }
}