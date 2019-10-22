using Basic.Framework.Web;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Jc_B
{
    public partial class AlarmRecordAddRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BInfo Jc_BInfo { get; set; }      
    }

    public partial class AlarmRecordUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public Jc_BInfo Jc_BInfo { get; set; }      
    }

    public partial class AlarmRecordDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class AlarmRecordGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

    public partial class AlarmRecordGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class AlarmRecordBatchUpateRequesst: Basic.Framework.Web.BasicRequest
    {
        public List<Jc_BInfo> AlarmInfos { get; set; }
    }

    public partial class AlarmRecordGetByStimeRequest : BasicRequest
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string Stime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string ETime { get; set; }
    }

    public partial class AlarmRecordGetDateIdRequest 
    {
        public string Id { get; set; }

        public string AlarmDate { get; set; }
    }

    public partial class AlarmRecordUpdateDateRequest 
    {
        public Jc_BInfo AlarmInfo { get; set; }
    }

    public partial class AlarmRecordUpdateProperitesRequest 
    {

        public Jc_BInfo AlarmInfo { get; set; }

        public Dictionary<string, object> UpdateItems { get; set; }
    }
}
