using Basic.Framework.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request
{
    public partial class ReportGetListDataRequest : BasicRequest
    {
        public int ListID { get; set; }
    }

    public partial class ReportGetlistdisplayexRequest : BasicRequest
    {
        public int ListID { get; set; }
    }

    public partial class ReportWebGetlistdisplayexRequest : BasicRequest
    {
        public int ListID { get; set; }

        public int ListDataID { get; set; }
    }

    public partial class ReportGetReportDataRequest : BasicRequest
    {
        public int ListID { get; set; }

        public string _strFreQryCondition { get; set; }

        public List<string> _listdate { get; set; }

        public int pageNum { get; set; }

        public int perPageRecordNum { get; set; }
    }

    public partial class ReportWebGetReportDataRequest : BasicRequest
    {
        public int ListID { get; set; }

        public int ListDataID { get; set; }

        public string _strFreQryCondition { get; set; }

        public List<string> _listdate { get; set; }

        public int pageNum { get; set; }

        public int perPageRecordNum { get; set; }

        public int FilterType { get; set; }

        public int RemoveRepetType { get; set; }

        public string ArrangeTime { get; set; }
    }

    public partial class ReportGetFKDataRequest : BasicRequest
    {
        public string strFKCode { get; set; }
    }

    public partial class ReportGetListTempleRequest : BasicRequest
    {
        public int ListDataID { get; set; }
    }

    public partial class ReportGetFKDataByHashTableRequest : BasicRequest
    {
        public string strFKCode { get; set; }
    }

    public partial class ReportGetEnvirLevelDetailRequest : BasicRequest
    {
        public long PointID { get; set; }
    }

    public partial class ReportGetPointParameterRequest : BasicRequest
    {
        public long fzh { get; set; }
    }

    public partial class ReportGetMorePointDataRequest : BasicRequest
    {
        public string strfzhs { get; set; }

        public string devid { get; set; }

        public string strdate { get; set; }

        public List<string> _listdate { get; set; }
    }

    public partial class ReportGetReportTotalRecordRequest : BasicRequest
    {
        public int ListID { get; set; }

        public int ListDataID { get; set; }

        public string _strFreQryCondition { get; set; }

        public List<string> _listdate { get; set; }

        public int FilterType { get; set; }

        public int RemoveRepetType { get; set; }

        public string ArrangeTime { get; set; }
    }
}
