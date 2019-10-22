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
    public partial class RolerightRepository : RepositoryBase<RolerightModel>, IRolerightRepository
    {

        public RolerightModel AddRoleright(RolerightModel rolerightModel)
        {
            return base.Insert(rolerightModel);
        }
        public void UpdateRoleright(RolerightModel rolerightModel)
        {
            base.Update(rolerightModel);
        }
        public void DeleteRoleright(string id)
        {
            base.Delete(id);
        }
        /// <summary>
        /// 根据角色ID删除角色对应的权限绑定
        /// </summary>
        /// <param name="RoleId"></param>
        public void DeleteRolerightByRoleId(string RoleId)
        {
           List<RolerightModel> RolerightList= base.Datas.Where(a => a.RoleID == RoleId).ToList();
            foreach (RolerightModel roleright in RolerightList) {
                base.Delete(roleright.RoleRightID);
            }           
        }
        /// <summary>
        /// 根据权限ID删除角色对应权限
        /// </summary>
        /// <param name="RightId"></param>
        public void DeleteRolerightByRightId(string RightId)
        {
            List<RolerightModel> RolerightList = base.Datas.Where(a => a.RightID == RightId).ToList();
            foreach (RolerightModel roleright in RolerightList)
            {
                base.Delete(roleright.RoleRightID);
            }
        }
        public IList<RolerightModel> GetRolerightList(int pageIndex, int pageSize, out int rowCount)
        {
            var rolerightModelLists = base.Datas;
            rowCount = rolerightModelLists.Count();
            return rolerightModelLists.OrderBy(p => p.RoleRightID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<RolerightModel> GetRolerightList()
        {
            var rolerightModelLists = base.Datas.ToList();            
            return rolerightModelLists;
        }
        public RolerightModel GetRolerightById(string id)
        {
            RolerightModel rolerightModel = base.Datas.FirstOrDefault(c => c.RoleRightID == id);
            return rolerightModel;
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限信息
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public List<RightModel> GetRightsByRoleId(string RoleId) {            
            DataTable Rights = base.QueryTable("global_RolerightService_GetRightsByRoleId", RoleId);
            return Basic.Framework.Common.ObjectConverter.Copy<RightModel>(Rights);        
        }
        /// <summary>
        /// 根据角色ID获取角色对应的权限关联信息
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public List<RolerightModel> GetRolerightByRoleId(string RoleId)
        {
            List<RolerightModel> rolerightModel = base.Datas.Where(c => c.RoleID == RoleId).ToList();
            return rolerightModel;
        }
    }
}
