using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listcommandex
{
    public partial class ListcommandexAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListcommandexInfo ListcommandexInfo { get; set; }      
    }

	public partial class ListcommandexUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListcommandexInfo ListcommandexInfo { get; set; }      
    }

	public partial class ListcommandexDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListcommandexGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListcommandexGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListCommandInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListcommandexInfo Info { get; set; }
    }
}
