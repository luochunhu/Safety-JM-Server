using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Basic.Framework.Entities;

namespace Sys.Safety.DataContract
{
    public partial class R_PrealInfo : CacheEntity
    {
        ///// <summary>
        ///// 唯一编码
        ///// </summary>
        //public string Id
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 标志卡号
        /// </summary>
        public string Bh
        {
            get;
            set;
        }
        /// <summary>
        /// 内部编号
        /// </summary>
        public string Yid
        {
            get;
            set;
        }
        /// <summary>
        /// 当前测点Id
        /// </summary>
        public string Pointid
        {
            get;
            set;
        }
        /// <summary>
        /// 经过测点号
        /// </summary>
        public string Uppointid
        {
            get;
            set;
        }
        /// <summary>
        /// 入井测点号
        /// </summary>
        public string Onpointid
        {
            get;
            set;
        }
        /// <summary>
        /// 进入当前测点时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Intime
        {
            get;
            set;
        }
        /// <summary>
        /// 最近采集时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Rtime
        {
            get;
            set;
        }
        /// <summary>
        /// 经过测点采集时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Uptime
        {
            get;
            set;
        }
        /// <summary>
        /// 入井测点采集时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Ontime
        {
            get;
            set;
        }
        /// <summary>
        /// 入井时长
        /// </summary>
        public string Rjsc
        {
            get;
            set;
        }
        /// <summary>
        /// 报警类型
        /// </summary>
        public int Bjtype
        {
            get;
            set;
        }
        /// <summary>
        /// 报警时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Bjtime
        {
            get;
            set;
        }
        /// <summary>
        /// 报警结束时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Bjjstime
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Ptptime
        {
            get;
            set;
        }
        /// <summary>
        /// 报警时长
        /// </summary>
        public string Bjsc
        {
            get;
            set;
        }
        /// <summary>
        /// 班次
        /// </summary>
        public string Bc
        {
            get;
            set;
        }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Szqy
        {
            get;
            set;
        }
        /// <summary>
        /// 标志 0-正常，1-人员已出井
        /// </summary>
        public string Flag
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类型标志(0人员，1机车)
        /// </summary>
        public string Sysflag
        {
            get;
            set;
        }
        /// <summary>
        /// 地面井下标志（1井下，0 地面）
        /// </summary>
        public string Jxflag
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By5
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Upflag
        {
            get;
            set;
        }
    }
}


