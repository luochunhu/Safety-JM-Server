using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Data
{
    public  class KJ73NDataContextForRead : KJ73NDataContext
    {
        private static Random rdm = new Random();
        public KJ73NDataContextForRead()
        {
            int len = Basic.Framework.Configuration.Global.SlaveDatabase.Count;
            var index = rdm.Next(0, len - 1);
            var slave = Basic.Framework.Configuration.Global.SlaveDatabase[index];


            this.Database.Connection.ConnectionString = slave;
        }
    }

        
}
