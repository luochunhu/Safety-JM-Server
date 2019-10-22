using System;
using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class MetadatafieldsRepository : RepositoryBase<MetadatafieldsModel>, IMetadatafieldsRepository
    {
        public MetadatafieldsModel AddMetadatafields(MetadatafieldsModel metadatafieldsModel)
        {
            return Insert(metadatafieldsModel);
        }

        public void UpdateMetadatafields(MetadatafieldsModel metadatafieldsModel)
        {
            Update(metadatafieldsModel);
        }

        public void DeleteMetadatafields(string id)
        {
            Delete(Convert.ToInt32(id));
        }

        public IList<MetadatafieldsModel> GetMetadatafieldsList(int pageIndex, int pageSize, out int rowCount)
        {
            var metadatafieldsModelLists = Datas;
            rowCount = metadatafieldsModelLists.Count();
            return metadatafieldsModelLists.OrderBy(a => a.ID).Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }

        public MetadatafieldsModel GetMetadatafieldsById(string id)
        {
            var iId = Convert.ToInt32(id);
            var metadatafieldsModel = Datas.FirstOrDefault(c => c.ID == iId);
            return metadatafieldsModel;
        }

        public IList<MetadatafieldsModel> GetMetadatafieldsListAll()
        {
            var model = Datas.ToList();
            return model;
        }
    }
}