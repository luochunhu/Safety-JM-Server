using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Processing.Linkage
{
    [ServiceContract]
    public interface IRLService
    {
        [OperationContract]
        bool UpdateRealDevices(int systemID, RealDevice[] items);
    }

}
