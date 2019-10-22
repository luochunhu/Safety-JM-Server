using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Enumtype;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IEnumtypeService
    {
        /// <summary>
        /// 保存枚举类型
        /// </summary>
        /// <param name="enumtyperequest"></param>
        /// <returns></returns>
        BasicResponse<EnumtypeInfo> SaveEnumType(EnumtypeAddRequest enumtyperequest);

        BasicResponse<EnumtypeInfo> AddEnumtype(EnumtypeAddRequest enumtyperequest);
        BasicResponse<EnumtypeInfo> UpdateEnumtype(EnumtypeUpdateRequest enumtyperequest);
        BasicResponse DeleteEnumtype(EnumtypeDeleteRequest enumtyperequest);
        BasicResponse<List<EnumtypeInfo>> GetEnumtypeList(EnumtypeGetListRequest enumtyperequest);
        BasicResponse<List<EnumtypeInfo>> GetEnumtypeList();
        BasicResponse<EnumtypeInfo> GetEnumtypeById(EnumtypeGetRequest enumtyperequest);
        BasicResponse<EnumtypeInfo> GetEnumtypeByStrCode(EnumtypeGetByStrCodeRequest enumtyperequest);
    }
}

