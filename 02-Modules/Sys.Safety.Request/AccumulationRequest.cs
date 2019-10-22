using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request
{
    public partial class AccumulationDayExistsRequest
    {
        public string PointId { get; set; }

        public DateTime Timer { get; set; }
    }

    public partial class AccumulationHourExistsRequest
    {
        public string PointId { get; set; }

        public DateTime Timer { get; set; }
    }

    public partial class AccumulationMonthExistsRequest
    {
        public string PointId { get; set; }

        public DateTime Timer { get; set; }
    }

    public partial class AccumulationYearExistsRequest
    {
        public string PointId { get; set; }

        public DateTime Timer { get; set; }
    }
}
