using Basic.Framework.Rpc;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.R_Personinf
{
    public partial class R_PersoninfAddRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PersoninfInfo PersoninfInfo { get; set; }      
    }

    public partial class R_PersoninfUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public R_PersoninfInfo PersoninfInfo { get; set; }      
    }

    public partial class R_PersoninfDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class R_PersoninfGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }
    public partial class R_PersoninfGetByBhRequest : Basic.Framework.Web.BasicRequest
    {
        public string Bh { get; set; }
    }

    public partial class R_PersoninfGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class R_PersonGetByConditionRequest : Basic.Framework.Web.BasicRequest 
    {
        public Expression<Func<R_PersoninfInfo, bool>> Predicate { get; set; }
    }
}
