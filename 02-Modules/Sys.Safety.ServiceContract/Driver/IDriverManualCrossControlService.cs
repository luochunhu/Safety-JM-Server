using Sys.Safety.Request.Driver;

namespace Sys.Safety.ServiceContract.Driver
{
    /// 作者：
    /// 时间：2017-06-03
    /// 描述：驱动手动交叉控制接口
    /// 修改记录
    /// 2017-06-03
    /// </summary>
    public interface IDriverManualCrossControlService
    {
        /// <summary>
        /// 加载手动交叉控制到分站控制链表
        /// </summary>
        void ReLoad(DriverManualCrossControlReLoadRequest reLoadRequest);
    }
}
