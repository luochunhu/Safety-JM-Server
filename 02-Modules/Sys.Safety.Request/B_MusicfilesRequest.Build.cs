using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Musicfiles
{
    public partial class B_MusicfilesAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_MusicfilesInfo B_MusicfilesInfo { get; set; }      
    }

	public partial class B_MusicfilesUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_MusicfilesInfo B_MusicfilesInfo { get; set; }      
    }

	public partial class B_MusicfilesDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_MusicfilesGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_MusicfilesGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
