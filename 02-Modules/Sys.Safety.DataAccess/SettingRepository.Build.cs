using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class SettingRepository : RepositoryBase<SettingModel>, ISettingRepository
    {

        public SettingModel AddSetting(SettingModel settingModel)
        {
            return base.Insert(settingModel);
        }
        public void UpdateSetting(SettingModel settingModel)
        {
            base.Update(settingModel);
        }
        public void DeleteSetting(string id)
        {
            base.Delete(id);
        }
        public IList<SettingModel> GetSettingList(int pageIndex, int pageSize, out int rowCount)
        {
            var settingModelLists = base.Datas.ToList();
            rowCount = settingModelLists.Count();
            return settingModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        public List<SettingModel> GetSettingList()
        {
            var settingModelLists = base.Datas.ToList();            
            return settingModelLists;
        }
        public SettingModel GetSettingById(string id)
        {
            SettingModel settingModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return settingModel;
        }
        /// <summary>
        /// 根据strKey获取SettingModel
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public SettingModel GetSettingByKey(string strKey)
        {
            SettingModel settingModel = base.Datas.FirstOrDefault(c => c.StrKey == strKey);
            return settingModel;
        }

        /// <summary>
        ///根据创建者获取GetSettingList
        /// </summary>
        /// <returns></returns>
        public IList<SettingModel> GetSettingListForCreator()
        {
            var settingModelList = base.Datas.Where(c => c.Creator != "-1").ToList();
            return settingModelList;
        }
    }
}
