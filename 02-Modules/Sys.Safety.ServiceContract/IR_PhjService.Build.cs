using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Phj;

namespace Sys.Safety.ServiceContract
{
    public interface IR_PhjService
    {
        BasicResponse<R_PhjInfo> AddPhj(PhjAddRequest phjRequest);
        BasicResponse<R_PhjInfo> UpdatePhj(PhjUpdateRequest phjRequest);
        BasicResponse DeletePhj(PhjDeleteRequest phjRequest);
        BasicResponse<List<R_PhjInfo>> GetPhjList(PhjGetListRequest phjRequest);
        BasicResponse<R_PhjInfo> GetPhjById(PhjGetRequest phjRequest);
        /// <summary>
        /// 添加呼叫记录到数据库
        /// </summary>
        /// <param name="phjRequest"></param>
        /// <returns></returns>
        BasicResponse AddPhjToDB(PhjAddRequest phjRequest);
        /// <summary>
        /// 获取当前呼叫的人员信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<string>> GetPhjAlarmedList();
    }
}

