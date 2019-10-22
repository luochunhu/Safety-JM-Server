using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sys.Safety.Enums;

namespace Sys.Safety.DataContract
{
    public partial class R_UndefinedDefInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PointId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Fzh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Kh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Dzh
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Point
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Devid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get;
            set;
        }

        public Recognizer type
        {
            get;
            set;
        }
    }
}


