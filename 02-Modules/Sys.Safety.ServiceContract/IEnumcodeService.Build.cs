using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IEnumcodeService
    {
        /// <summary>
        /// 保存枚举
        /// </summary>
        /// <param name="enumcoderequest"></param>
        /// <returns></returns>
        BasicResponse<EnumcodeInfo> SaveEnumCode(EnumcodeAddRequest enumcoderequest);        
        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>
        BasicResponse UpdateCache();
        BasicResponse<EnumcodeInfo> AddEnumcode(EnumcodeAddRequest enumcoderequest);
        BasicResponse<EnumcodeInfo> UpdateEnumcode(EnumcodeUpdateRequest enumcoderequest);
        BasicResponse DeleteEnumcode(EnumcodeDeleteRequest enumcoderequest);
        BasicResponse<List<EnumcodeInfo>> GetEnumcodeList(EnumcodeGetListRequest enumcoderequest);
        BasicResponse<List<EnumcodeInfo>> GetEnumcodeList();
        BasicResponse<EnumcodeInfo> GetEnumcodeById(EnumcodeGetRequest enumcoderequest);
        /// <summary>
        /// 根据枚举类型查找类型对应的枚举
        /// </summary>
        /// <param name="enumcoderequest"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetEnumcodeByEnumTypeID(EnumcodeGetByEnumTypeIDRequest enumcoderequest);

        /// <summary>获取所有设备性质
        /// 
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDevicePropertyCache();

        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetAllDeviceClassCache();

        BasicResponse<List<EnumcodeInfo>> GetAllDeviceModelCache();
    }
}

