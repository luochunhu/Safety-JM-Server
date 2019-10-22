using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ISysEmergencyLinkageRepository : IRepository<SysEmergencyLinkageModel>
    {
        SysEmergencyLinkageModel AddSysEmergencyLinkage(SysEmergencyLinkageModel sysEmergencyLinkageModel);
        void UpdateSysEmergencyLinkage(SysEmergencyLinkageModel sysEmergencyLinkageModel);
        void DeleteSysEmergencyLinkage(string id);
        IList<SysEmergencyLinkageModel> GetSysEmergencyLinkageList(int pageIndex, int pageSize, out int rowCount);

        /// <summary>获取所有
        /// 
        /// </summary>
        /// <returns></returns>
        IList<SysEmergencyLinkageModel> GetAllSysEmergencyLinkageList();

        SysEmergencyLinkageModel GetSysEmergencyLinkageById(string id);

        /// <summary>
        /// 根据lambda获取数据
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        IList<SysEmergencyLinkageModel> GetSysEmergencyLinkageListByLambda(Expression<Func<SysEmergencyLinkageModel, bool>> lambda);

        /// <summary>
        /// 根据lambda判断是否存在
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        bool AnySysEmergencyLinkageByLambda(Expression<Func<SysEmergencyLinkageModel, bool>> lambda);
    }
}