using Basic.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public class R_SyncLocalInfo : CacheEntity
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string Bh { get; set; }
        /// <summary>
        /// 员工YID
        /// </summary>
        public string Yid { get; set; }
        /// <summary>
        /// 采集分站号
        /// </summary>
        public int Fzh { get; set; }
        /// <summary>
        /// 采集口号
        /// </summary>
        public int Kh { get; set; }
        /// <summary>
        /// 是否是呼叫上
        /// </summary>
        public int Zt { get; set; }
        /// <summary>
        /// 采集时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Rtime { get; set; }
        /// <summary>
        /// 系统类型标志:0—人员,1—机车
        /// </summary>
        public int Sysflag { get; set; }
        /// <summary>
        /// 标识卡欠压标识  20171214
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// 是否为补传
        /// </summary>
        public int Passup { get; set; }
        /// <summary>
        /// 写记录时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Timer { get; set; }
        /// <summary>
        /// 是否为特殊工种
        /// </summary>
        public int Tsgzflag { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        public string Upflag { get; set; }
    }
}
