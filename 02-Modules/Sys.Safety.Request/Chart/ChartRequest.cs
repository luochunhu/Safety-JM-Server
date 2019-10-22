using System;
using System.Collections.Generic;

namespace Sys.Safety.Request.Chart
{
    public class GetPointListRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public int Type { get; set; }
    }

    public class GetPointCacheByDevpropertIDRequest
    {
        public int DevpropertID { get; set; }
    }

    public class GetPointKzListRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public string PointID { get; set; }
    }

    public class ShowPointInfRequest
    {
        public string CurrentWzid { get; set; }
        public string CurrentPointId { get; set; }
    }

    public class GetMonthLineRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
    }

    public class GetFiveMiniteLineRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        /// <summary>
        /// 是否只按测点号查询（将同一天内删除前和删除后的记录都查询出来）
        /// </summary>
        public bool IsQueryByPoint { get; set; }
    }

    public class GetDataValeRequest
    {
        public DateTime QxDate { get; set; }
        public DateTime DtStart { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
    }

    public class GetValueRequest
    {
        public DateTime QxDate { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
    }

    public class GetStateBarTableRequest
    {
        public DateTime SzNameT { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetStateChgdtRequest
    {
        public DateTime SzNameT { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetStateLineDtRequest
    {
        public DateTime SzNameT { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetMnlBjLineDtRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public string type { get; set; }
    }

    public class GetKzlLineDtRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
    }

    public class GetDgViewRequest
    {
        public DateTime SzNameT { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetKjThingsRequest
    {
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class InitQxZhuZhuangRequest
    {
        public DateTime SzNameT { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetMcDataRequest
    {
        public DateTime SzNameS { get; set; }
        public DateTime SzNameE { get; set; }
        public bool flag { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
        public string TimeTick { get; set; }
        /// <summary>
        /// 是否只按测点号查询（将同一天内删除前和删除后的记录都查询出来）
        /// </summary>
        public bool IsQueryByPoint { get; set; }
    }

    public class GetMonthBarRequest
    {
        public int year { get; set; }
        public int month { get; set; }
        public string CurrentPointID { get; set; }
        public string CurrentDevid { get; set; }
        public string CurrentWzid { get; set; }
    }

    public class GetMLLFiveLineRequest
    {
        public List<string> _listdate { get; set; }
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetMHourLineRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetMLLFiveGirdDataRequest
    {
        public string dattime { get; set; }
        public long PointID { get; set; }
    }

    public class GetMLLMonthBarRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetMLLMCLineRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
        public int Type { get; set; }
    }

    public class GetKGLLStateRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetKGLStateGirdDataRequest
    {
        public string dattime { get; set; }
        public long PointID { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetKJXLRequest
    {
        public string dattime { get; set; }
        public long PointID { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetKGLStateGridDataByRightRequest
    {
        public string dattime { get; set; }
        public long PointID { get; set; }
        public bool kglztjsfs { get; set; }
    }

    public class GetMnlBJLineRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetMnlDDLineRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetPointKzByPointIDRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetKzlLineRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public long PointID { get; set; }
    }

    public class GetPointByTypeRequest
    {
        public string datStart { get; set; }
        public string datEnd { get; set; }
        public int Type { get; set; }
    }

    public class SaveChartSetRequest
    {
        public Dictionary<string, string> ChartSet { get; set; }
    }

    public class GetAllChartSetRequest
    {
        public string StrKey { get; set; }
    }
}