using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Metadatafields;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class MetadatafieldsService : IMetadatafieldsService
    {
        private readonly IMetadatafieldsRepository _Repository;

        public MetadatafieldsService(IMetadatafieldsRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<MetadatafieldsInfo> AddMetadatafields(MetadatafieldsAddRequest metadatafieldsrequest)
        {
            var _metadatafields =
                ObjectConverter.Copy<MetadatafieldsInfo, MetadatafieldsModel>(metadatafieldsrequest.MetadatafieldsInfo);
            var resultmetadatafields = _Repository.AddMetadatafields(_metadatafields);
            var metadatafieldsresponse = new BasicResponse<MetadatafieldsInfo>();
            metadatafieldsresponse.Data =
                ObjectConverter.Copy<MetadatafieldsModel, MetadatafieldsInfo>(resultmetadatafields);
            return metadatafieldsresponse;
        }

        public BasicResponse<MetadatafieldsInfo> UpdateMetadatafields(MetadatafieldsUpdateRequest metadatafieldsrequest)
        {
            var _metadatafields =
                ObjectConverter.Copy<MetadatafieldsInfo, MetadatafieldsModel>(metadatafieldsrequest.MetadatafieldsInfo);
            _Repository.UpdateMetadatafields(_metadatafields);
            var metadatafieldsresponse = new BasicResponse<MetadatafieldsInfo>();
            metadatafieldsresponse.Data = ObjectConverter.Copy<MetadatafieldsModel, MetadatafieldsInfo>(_metadatafields);
            return metadatafieldsresponse;
        }

        public BasicResponse DeleteMetadatafields(MetadatafieldsDeleteRequest metadatafieldsrequest)
        {
            _Repository.DeleteMetadatafields(metadatafieldsrequest.Id);
            var metadatafieldsresponse = new BasicResponse();
            return metadatafieldsresponse;
        }

        public BasicResponse<List<MetadatafieldsInfo>> GetMetadatafieldsList(
            MetadatafieldsGetListRequest metadatafieldsrequest)
        {
            var metadatafieldsresponse = new BasicResponse<List<MetadatafieldsInfo>>();
            metadatafieldsrequest.PagerInfo.PageIndex = metadatafieldsrequest.PagerInfo.PageIndex - 1;
            if (metadatafieldsrequest.PagerInfo.PageIndex < 0)
                metadatafieldsrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var metadatafieldsModelLists = _Repository.GetMetadatafieldsList(metadatafieldsrequest.PagerInfo.PageIndex,
                metadatafieldsrequest.PagerInfo.PageSize, out rowcount);
            var metadatafieldsInfoLists = new List<MetadatafieldsInfo>();
            foreach (var item in metadatafieldsModelLists)
            {
                var MetadatafieldsInfo = ObjectConverter.Copy<MetadatafieldsModel, MetadatafieldsInfo>(item);
                metadatafieldsInfoLists.Add(MetadatafieldsInfo);
            }
            metadatafieldsresponse.Data = metadatafieldsInfoLists;
            return metadatafieldsresponse;
        }

        public BasicResponse<MetadatafieldsInfo> GetMetadatafieldsById(MetadatafieldsGetRequest metadatafieldsrequest)
        {
            var result = _Repository.GetMetadatafieldsById(metadatafieldsrequest.Id);
            var metadatafieldsInfo = ObjectConverter.Copy<MetadatafieldsModel, MetadatafieldsInfo>(result);
            var metadatafieldsresponse = new BasicResponse<MetadatafieldsInfo>();
            metadatafieldsresponse.Data = metadatafieldsInfo;
            return metadatafieldsresponse;
        }
    }
}