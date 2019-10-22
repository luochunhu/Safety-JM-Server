using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.B_Playlistmusiclink
{
    public partial class B_PlaylistmusiclinkAddRequest : Basic.Framework.Web.BasicRequest
    {
        public B_PlaylistmusiclinkInfo B_PlaylistmusiclinkInfo { get; set; }      
    }

	public partial class B_PlaylistmusiclinkUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public B_PlaylistmusiclinkInfo B_PlaylistmusiclinkInfo { get; set; }      
    }

	public partial class B_PlaylistmusiclinkDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_PlaylistmusiclinkGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class B_PlaylistmusiclinkGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }
}
