using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_PersoninfRepository : RepositoryBase<R_PersoninfModel>, IR_PersoninfRepository
    {

        public R_PersoninfModel AddPersoninf(R_PersoninfModel personinfModel)
        {
            return base.Insert(personinfModel);
        }
        public void UpdatePersoninf(R_PersoninfModel personinfModel)
        {
            base.Update(personinfModel);
        }
        public void DeletePersoninf(string id)
        {
            base.Delete(id);
        }
        public IList<R_PersoninfModel> GetPersoninfList(int pageIndex, int pageSize, out int rowCount)
        {
            var personinfModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return personinfModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_PersoninfModel GetPersoninfById(string id)
        {
            R_PersoninfModel personinfModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return personinfModel;
        }
    }
}
