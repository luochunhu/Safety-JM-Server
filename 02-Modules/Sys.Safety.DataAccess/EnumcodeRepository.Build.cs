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
    public partial class EnumcodeRepository : RepositoryBase<EnumcodeModel>, IEnumcodeRepository
    {

        public EnumcodeModel AddEnumcode(EnumcodeModel enumcodeModel)
        {
            return base.Insert(enumcodeModel);
        }
        public void UpdateEnumcode(EnumcodeModel enumcodeModel)
        {
            base.Update(enumcodeModel);
        }
        public void DeleteEnumcode(string id)
        {
            base.Delete(id);
        }
        public IList<EnumcodeModel> GetEnumcodeList(int pageIndex, int pageSize, out int rowCount)
        {
            var enumcodeModelLists = base.Datas;
            rowCount = enumcodeModelLists.Count();
            return enumcodeModelLists.OrderBy(p => p.EnumCodeID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<EnumcodeModel> GetEnumcodeList()
        {
            var enumcodeModelLists = base.Datas.ToList();            
            return enumcodeModelLists;
        }
        public EnumcodeModel GetEnumcodeById(string id)
        {
            EnumcodeModel enumcodeModel = base.Datas.FirstOrDefault(c => c.EnumCodeID == id);
            return enumcodeModel;
        }        
        /// <summary>
        /// 根据枚举类型ID查找对应类型的枚举信息
        /// </summary>
        /// <param name="EnumTypeID"></param>
        /// <returns></returns>

        public List<EnumcodeModel> GetEnumcodeByEnumTypeID(string EnumTypeID)
        {
            List<EnumcodeModel> enumcodeModel = base.Datas.Where(c => c.EnumTypeID == EnumTypeID).ToList();
            return enumcodeModel;
        }
        /// <summary>
        /// 获取数据库数据并更新到服务端枚举缓存
        /// </summary>
        public DataTable GetAllEnumcode()
        {            
            DataTable dt = base.QueryTable("global_EnumcodeService_UpdateCache_GetAllEnumcode");           
            return dt;
        }
    }
}
