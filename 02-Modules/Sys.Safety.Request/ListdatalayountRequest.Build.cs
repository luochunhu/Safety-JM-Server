using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listdatalayount
{
    public partial class ListdatalayountAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdatalayountInfo ListdatalayountInfo { get; set; }      
    }

	public partial class ListdatalayountUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdatalayountInfo ListdatalayountInfo { get; set; }      
    }

	public partial class ListdatalayountDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdatalayountGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListdatalayountGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListDataLayountInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdatalayountInfo Info { get; set; }
    }

    public partial class DeleteListdatalayountByTimeListDataIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string Time { get; set; }

        public string ListDataId { get; set; }
    }

    public partial class GetListdatalayountByListDataIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class GetListdatalayountByListDataIdArrangeTimeRequest : Basic.Framework.Web.BasicRequest
    {
        public string ListDataId { get; set; }

        public string ArrangeName { get; set; }
    }
}
