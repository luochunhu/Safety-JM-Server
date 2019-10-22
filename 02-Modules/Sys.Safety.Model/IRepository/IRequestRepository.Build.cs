using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IRequestRepository : IRepository<RequestModel>
    {
        RequestModel AddRequest(RequestModel requestModel);
        void UpdateRequest(RequestModel requestModel);
        void DeleteRequest(string id);
        IList<RequestModel> GetRequestList(int pageIndex, int pageSize, out int rowCount);
        List<RequestModel> GetRequestList();
        RequestModel GetRequestById(string id);
        /// <summary>
        /// 根据Code获取
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        RequestModel GetRequestByCode(string Code);
        /// <summary>
        /// 获取请求库对应的菜单信息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        DataTable GetRequestMenuByCode(string Code);
		void DeleteRequestByMenuUrlMenuParams(string menuUrl, string menuParams);

        IList<RequestModel> GetRequestListAll();    }
}
