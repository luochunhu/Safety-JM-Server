using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class ConfigRepository : RepositoryBase<ConfigModel>, IConfigRepository
    {

        public ConfigModel AddConfig(ConfigModel configModel)
        {
            return base.Insert(configModel);
        }
        public void UpdateConfig(ConfigModel configModel)
        {
            base.Update(configModel);
        }
        public void DeleteConfig(string id)
        {
            base.Delete(id);
        }
        public IList<ConfigModel> GetConfigList(int pageIndex, int pageSize, out int rowCount)
        {
            var configModelLists = base.Datas;
            rowCount = configModelLists.Count();
            return configModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<ConfigModel> GetConfigList()
        {
            var configModelLists = base.Datas.ToList();            
            return configModelLists;
        }
        public ConfigModel GetConfigById(string id)
        {
            ConfigModel configModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return configModel;
        }
        /// <summary>
        /// 根据名称获取配置  20170522
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns></returns>
        public ConfigModel GetConfigByName(string name)
        {
            ConfigModel configModel = base.Datas.FirstOrDefault(c => c.Name == name);
            return configModel;
        }

        public void SaveConfig(List<ConfigModel> list)
        {
            base.Insert(list);
        }
    }
}
