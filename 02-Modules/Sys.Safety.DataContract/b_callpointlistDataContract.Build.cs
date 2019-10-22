using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class B_CallpointlistInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        public string BCallId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CallId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string AgentPointId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string CalledPointId
        {
            get;
            set;
        }
    }
}


