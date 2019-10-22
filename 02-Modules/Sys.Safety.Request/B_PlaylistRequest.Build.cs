using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Playlist
{
    public partial class B_PlaylistAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_PlaylistInfo B_PlaylistInfo { get; set; }      
    }

	public partial class B_PlaylistUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_PlaylistInfo B_PlaylistInfo { get; set; }      
    }

	public partial class B_PlaylistDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_PlaylistGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_PlaylistGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
