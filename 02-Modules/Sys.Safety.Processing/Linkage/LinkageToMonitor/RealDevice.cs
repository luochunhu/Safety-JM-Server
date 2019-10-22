using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sys.Safety.Processing.Linkage
{
    [DataContract(Name = "RealDevice", Namespace = "http://schemas.datacontract.org/2004/07/WcfService")]
    public class RealDevice
    {
        // Properties
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public DateTime CTime { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RealValue { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string Unit { get; set; }
    }
}
