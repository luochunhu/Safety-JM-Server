using System.Collections.Generic;

namespace Sys.Safety.Request.DataToDb
{
    /// <summary>
    /// 添加入库列表RPC
    /// </summary>
    public  partial class DataToDbAddRequest<T> where T:class
    {
        /// <summary>
        /// 入库对象
        /// </summary>
        public T Item { get; set; }
    }

    /// <summary>
    /// 批量添加入库列表RPC
    /// </summary>
    public partial class DataToDbBatchAddRequest<T> where T:class
    {
        /// <summary>
        /// 入库对象集合
        /// </summary>
        public List<T> Items { get; set; }
    }

    /// <summary>
    /// 开始入库线程RPC
    /// </summary>
    public partial class DataToDbStartRequest
    {


    }

    /// <summary>
    /// 停止入库线程RPC
    /// </summary>
    public partial class DataToDbStopRequest
    {
    }
}
