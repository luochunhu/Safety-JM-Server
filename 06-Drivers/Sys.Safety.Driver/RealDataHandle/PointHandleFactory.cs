using Sys.Safety.Enums;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：实时数据对象工厂
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class PointHandleFactory
    {
        /// <summary>
        /// 创建测点实例
        /// </summary>
        /// <param name="realDataItem">当前接收到的该测点数据</param>
        /// <param name="pointDefineInfo">当前内存中的该测点数据</param>
        /// <param name="createdTime">数据接收时间</param>
        /// <returns></returns>
        public static PointHandle CreateRealDataHandle(int pointType)
        {
            PointHandle dataHandle = null;
            switch (pointType)
            {
                case (int)DeviceProperty.Derail:
                    dataHandle = new DerailHandle();
                    break;
                case (int)DeviceProperty.Control:
                    dataHandle = new ControlHandle();
                    break;
                //case (int)DeviceProperty.Accumulation:
                //    dataHandle = new AccumulationHandle();
                //    break;
                case (int)DeviceProperty.Analog:
                    dataHandle = new AnalogHandle();
                    break;
                case (int)DeviceProperty.Substation:
                    dataHandle = new StationHandle();
                    break;
            }

            return dataHandle;
        }
    }
}
