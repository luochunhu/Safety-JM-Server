using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.App
{
    public class BaseDataAppDataContract
    {
        /// <summary>
        /// 设备性质集合
        /// </summary>
        public List<DevProperty> DevPropertyDTOs { get; set; }
        /// <summary>
        /// 设备种类集合
        /// </summary>
        public List<DevType> DevTypeDTOs { get; set; }
        /// <summary>
        /// 设备类型集合
        /// </summary>
        public List<DevModel> DevModelDTOs { get; set; }
        /// <summary>
        /// 设备状态集合
        /// </summary>
        public List<DevState> DevStateDTOs { get; set; }
        /// <summary>
        ///测点集合
        /// </summary>
        public List<Point> PointDTOs { get; set; }
    }

    /// <summary>
    /// 设备性质
    /// </summary>
    public class DevProperty
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备性质编号
        /// </summary>
        public int DevPropertyID { get; set; }
    }

    /// <summary>
    /// 设备种类
    /// </summary>
    public class DevType
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备性质ID
        /// </summary>
        public int DevPropertyID { get; set; }
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public class DevModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string StrDevSpecification { get; set; }
        /// <summary>
        /// 设备性质ID
        /// </summary>
        public int DevPropertyID { get; set; }
        /// <summary>
        /// 设备种类ID
        /// </summary>
        public int DevTypeID { get; set; }

    }

    /// <summary>
    /// 设备状态
    /// </summary>
    public class DevState
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 设备性质ID
        /// </summary>
        public int DevPropertyID { get; set; }
        /// <summary>
        /// 关联设备性质
        /// </summary>
        public string RelDevProperty { get; set; }
    }

    /// <summary>
    /// 测点
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        ///分站ID
        /// </summary>
        public string StationID { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Place { get; set; }
        /// <summary>
        /// 设备类型编号
        /// </summary>
        public string DevModelID { get; set; }
        /// <summary>
        /// 设备种类编号
        /// </summary>
        public string DevTypeID { get; set; }
        /// <summary>
        /// 设备性质编号
        /// </summary>
        public string DevPropertyID { get; set; }
    }
}
