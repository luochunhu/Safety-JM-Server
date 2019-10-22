using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IEnumcodeRepository : IRepository<EnumcodeModel>
    {
        EnumcodeModel AddEnumcode(EnumcodeModel enumcodeModel);
        void UpdateEnumcode(EnumcodeModel enumcodeModel);
        void DeleteEnumcode(string id);
        IList<EnumcodeModel> GetEnumcodeList(int pageIndex, int pageSize, out int rowCount);
        List<EnumcodeModel> GetEnumcodeList();
        EnumcodeModel GetEnumcodeById(string id);
        /// <summary>
        /// 根据枚举类型ID查找对应类型的枚举信息
        /// </summary>
        /// <param name="EnumTypeID"></param>
        /// <returns></returns>

        List<EnumcodeModel> GetEnumcodeByEnumTypeID(string EnumTypeID);
        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>
        DataTable GetAllEnumcode();
    }
}
