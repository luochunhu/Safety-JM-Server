using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IConfigRepository : IRepository<ConfigModel>
    {
        ConfigModel AddConfig(ConfigModel configModel);
        void UpdateConfig(ConfigModel configModel);
        void DeleteConfig(string id);
        IList<ConfigModel> GetConfigList(int pageIndex, int pageSize, out int rowCount);

        List<ConfigModel> GetConfigList();
        ConfigModel GetConfigById(string id);
        /// <summary>
        /// 根据名称获取配置  20170522
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        ConfigModel GetConfigByName(string name);

        /// <summary>
        /// 批量保存配置
        /// </summary>
        /// <param name="list"></param>
        void SaveConfig(List<ConfigModel> list);
    }
}
