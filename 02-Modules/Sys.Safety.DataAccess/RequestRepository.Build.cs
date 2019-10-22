using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public partial class RequestRepository : RepositoryBase<RequestModel>, IRequestRepository
    {

        public RequestModel AddRequest(RequestModel requestModel)
        {
            return base.Insert(requestModel);
        }
        public void UpdateRequest(RequestModel requestModel)
        {
            base.Update(requestModel);
        }
        public void DeleteRequest(string id)
        {
            base.Delete(id);
        }
        public IList<RequestModel> GetRequestList(int pageIndex, int pageSize, out int rowCount)
        {
            var requestModelLists = base.Datas;
            rowCount = requestModelLists.Count();
            return requestModelLists.OrderBy(p => p.RequestID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<RequestModel> GetRequestList()
        {
            var requestModelLists = base.Datas.ToList();
            return requestModelLists;
        }
        public RequestModel GetRequestById(string id)
        {
            RequestModel requestModel = base.Datas.FirstOrDefault(c => c.RequestID == id);
            return requestModel;
        }
        /// <summary>
        /// 根据Code获取
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public RequestModel GetRequestByCode(string Code)
        {
            RequestModel requestModel = base.Datas.FirstOrDefault(c => c.RequestCode == Code);
            return requestModel;
        }
        /// <summary>
        /// 获取请求库对应的菜单信息
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public DataTable GetRequestMenuByCode(string Code)
        {
            DataTable RequestMenu = base.QueryTable("global_RequestUtil_GetMenuFromRequest_ByRequestCode", Code);
            return RequestMenu;
        }
        public void DeleteRequestByMenuUrlMenuParams(string menuUrl, string menuParams)
        {
            var model = from m in Datas
                where m.MenuParams == menuParams && m.MenuURL == menuUrl
                select m;
            var firstOrDefault = model.FirstOrDefault();
            if (firstOrDefault != null) Delete(firstOrDefault.RequestID);
        }

        public IList<RequestModel> GetRequestListAll()
        {
            return Datas.ToList();
        }
    }
}
