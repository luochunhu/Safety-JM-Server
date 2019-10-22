using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Listex
{
    public partial class ListexAddRequest : Basic.Framework.Web.BasicRequest
    {
        public ListexInfo ListexInfo { get; set; }      
    }

	public partial class ListexUpdateRequest : Basic.Framework.Web.BasicRequest
    {
        public ListexInfo ListexInfo { get; set; }      
    }

	public partial class ListexDeleteRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListexGetRequest : Basic.Framework.Web.BasicRequest
    {
        public string Id { get; set; }
    }

	public partial class ListexGetListRequest : Basic.Framework.Web.BasicRequest
    {
        public string SearchName { get; set; }
		public string Id	{ get; set; }
    }

    public partial class SaveListRequest : Basic.Framework.Web.BasicRequest
    {
        public ListexInfo ListEx { get; set; }
        public IList<ListcommandexInfo> CmdList { get; set; }
        public bool BlnSaveAs { get; set; }
        public bool BlnSaveAsSchema { get; set; }
    }

    public partial class SaveList2Request : Basic.Framework.Web.BasicRequest
    {
        public ListexInfo ListEx { get; set; }
        public IList<ListcommandexInfo> CmdList { get; set; }
        public ListdataexInfo ListDataEx { get; set; }
        public IList<ListmetadataInfo> ListmdList { get; set; }
        public IList<ListdisplayexInfo> LdList { get; set; }
        public DataTable LmdDt { get; set; }
        public int LngState { get; set; }
    }

    public partial class IdRequest : Basic.Framework.Web.BasicRequest
    {
        public int Id { get; set; }
    }

    public partial class LongIdRequest : Basic.Framework.Web.BasicRequest
    {
        public long Id { get; set; }
    }

    public partial class StringRequest : Basic.Framework.Web.BasicRequest
    {
        public String Str { get; set; }
    }

    public partial class PointIdRequest : Basic.Framework.Web.BasicRequest
    {
        public string PointId { get; set; }
    }

    public partial class DeleteSchemaRequest : Basic.Framework.Web.BasicRequest
    {
        public int ListID { get; set; }
        public int ListDataID { get; set; }
    }

    public partial class GetSQLRequest : Basic.Framework.Web.BasicRequest
    {
        public DataTable LmdDt { get; set; }
    }

    public partial class SqlRequest : Basic.Framework.Web.BasicRequest
    {
        public string Sql { get; set; }
    }

    public partial class SaveListMetaDataRequest : Basic.Framework.Web.BasicRequest
    {
        public IList<ListmetadataInfo> LmdList { get; set; }
    }

    public partial class SaveListDisplayExRequest : Basic.Framework.Web.BasicRequest
    {
        public IList<ListdisplayexInfo> LdList { get; set; }
    }

    public partial class ObjectRequest : Basic.Framework.Web.BasicRequest
    {
        public object Obj { get; set; }
    }

    public partial class SetDefaultSchemaRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataexInfo ListDataEx { get; set; }
    }

    public partial class SaveListDataExRequest : Basic.Framework.Web.BasicRequest
    {
        public ListdataexInfo ListDataEx { get; set; }
    }

    public partial class SaveListExInfoRequest : Basic.Framework.Web.BasicRequest
    {
        public ListexInfo Info { get; set; }
    }
}
