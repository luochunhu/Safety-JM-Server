using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ISettingRepository : IRepository<SettingModel>
    {
        SettingModel AddSetting(SettingModel settingModel);
        void UpdateSetting(SettingModel settingModel);
        void DeleteSetting(string id);
        IList<SettingModel> GetSettingList(int pageIndex, int pageSize, out int rowCount);
        SettingModel GetSettingById(string id);
        List<SettingModel> GetSettingList();
        /// <summary>
        /// 根据strKey获取SettingModel
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        SettingModel GetSettingByKey(string strKey);

        /// <summary>
        /// 根据创建者获取GetSettingList
        /// </summary>
        /// <returns></returns>
        IList<SettingModel> GetSettingListForCreator();
    }
}
