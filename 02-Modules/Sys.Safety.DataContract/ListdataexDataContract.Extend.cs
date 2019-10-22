using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class ListdataexInfo
    {
        public List<ListmetadataInfo> ListMetaDataDTOList { get; set; }

        public List<ListdisplayexInfo> ListDisplayExDTOList { get; set; }

        public List<ListtempleInfo> ListTempleDTOList { get; set; }

        public List<ListdatalayountInfo> ListDataLayoutDTOList { get; set; }
    }
}
