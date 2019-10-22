using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Data
{
    public  class KJ73NDataContextForWrite : KJ73NDataContext
    {
        public KJ73NDataContextForWrite()
        {
            this.Database.Connection.ConnectionString = Basic.Framework.Configuration.Global.MasterDatabase;
        }
    }

        
}
