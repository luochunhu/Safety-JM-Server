using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class ListexInfo
    {
        public List<ListdataexInfo> ListDataExDTOList { get; set; }

        public List<ListcommandexInfo> ListCommandExDTOList { get; set; }
    }
}
