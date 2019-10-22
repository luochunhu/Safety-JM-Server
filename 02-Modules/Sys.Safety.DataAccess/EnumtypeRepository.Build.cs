using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class EnumtypeRepository : RepositoryBase<EnumtypeModel>, IEnumtypeRepository
    {

        public EnumtypeModel AddEnumtype(EnumtypeModel enumtypeModel)
        {
            return base.Insert(enumtypeModel);
        }
        public void UpdateEnumtype(EnumtypeModel enumtypeModel)
        {
            base.Update(enumtypeModel);
        }
        public void DeleteEnumtype(string id)
        {
            base.Delete(id);
        }
        public IList<EnumtypeModel> GetEnumtypeList(int pageIndex, int pageSize, out int rowCount)
        {
            var enumtypeModelLists = base.Datas;
            rowCount = enumtypeModelLists.Count();
            return enumtypeModelLists.OrderBy(p => p.EnumTypeID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<EnumtypeModel> GetEnumtypeList()
        {
            var enumtypeModelLists = base.Datas.ToList();            
            return enumtypeModelLists;
        }
        public EnumtypeModel GetEnumtypeById(string id)
        {
            EnumtypeModel enumtypeModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return enumtypeModel;
        }
        public EnumtypeModel GetEnumtypeByStrCode(string StrCode)
        {
            EnumtypeModel enumtypeModel = base.Datas.FirstOrDefault(c => c.StrCode == StrCode);
            return enumtypeModel;
        }
    }
}
