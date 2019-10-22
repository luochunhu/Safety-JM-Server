using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_CallRepository : IRepository<B_CallModel>
    {
        B_CallModel AddCall(B_CallModel callModel);
        void UpdateCall(B_CallModel callModel);
        void DeleteCall(string id);
        IList<B_CallModel> GetCallList(int pageIndex, int pageSize, out int rowCount);
        B_CallModel GetCallById(string id);

        /// <summary>获取所有bcall
        /// 
        /// </summary>
        /// <returns></returns>
        IList<B_CallModel> GetAllCall();
    }
}
