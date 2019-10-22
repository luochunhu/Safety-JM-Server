using Basic.Framework.Web;
using Sys.Safety.Request.DataToDb;

namespace Sys.Safety.ServiceContract.DataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-26
    /// 描述:数据处理入库业务RPC接口
    /// 修改记录
    /// 2017-05-26
    /// </summary>
    public interface IInsertToDbService<T> where T : class
    {
        /// <summary>
        /// 添加数据至入库列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        BasicResponse AddItem(DataToDbAddRequest<T> dataToDbRequest);

        /// <summary>
        /// 批量添加数据至入库列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        BasicResponse AddItems(DataToDbBatchAddRequest<T> dataToDbRequest);

        /// <summary>
        /// 启动入库补录线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        BasicResponse Start(DataToDbStartRequest dataToDbRequest);

        /// <summary>
        /// 停止入库补录线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        BasicResponse Stop(DataToDbStopRequest dataToDbRequest);
        /// <summary>
        /// 获取队列积压数量  20170717
        /// </summary>
        /// <returns></returns>
        BasicResponse<int> GetQueueBacklog();

        /// <summary>
        /// 获取总处理量
        /// </summary>
        /// <returns></returns>
        BasicResponse<long> GetTotalCount();
    }
}
