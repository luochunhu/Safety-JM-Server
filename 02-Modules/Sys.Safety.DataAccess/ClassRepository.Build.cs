using System.Collections.Generic;
using System.Linq;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public class ClassRepository : RepositoryBase<ClassModel>, IClassRepository
    {
        public ClassModel AddClass(ClassModel classModel)
        {
            return Insert(classModel);
        }

        public void UpdateClass(ClassModel classModel)
        {
            Update(classModel);
        }

        public void DeleteClass(string id)
        {
            Delete(id);
        }

        public IList<ClassModel> GetClassList(int pageIndex, int pageSize, out int rowCount)
        {
            var classModelLists = Datas;
            rowCount = classModelLists.Count();
            return classModelLists.Skip(pageIndex*pageSize).Take(pageSize).ToList();
        }
        public IList<ClassModel> GetAllClassList()
        {
            var classModelLists = Datas.ToList();            
            return classModelLists;
        }

        public ClassModel GetClassById(string id)
        {
            var classModel = Datas.FirstOrDefault(c => c.ClassID == id);
            return classModel;
        }

        public void DeleteClassByCode(string code)
        {
            var model = Datas.FirstOrDefault(m => m.StrCode == code);
            if (model != null)
            {
                DeleteClass(model.ClassID);
            }
        }

        public void AddClassList(IEnumerable<ClassModel> modelList)
        {
            this.Insert(modelList);
        }

        public ClassModel GetClassByCode(string code)
        {
            return this.Datas.FirstOrDefault(m => m.StrCode == code);
        }

        /// <summary>
        /// 根据strName获取ClassModel
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public ClassModel GetClassByStrName(string strName)
        {
            return this.Datas.FirstOrDefault(m => m.StrName == strName);
        }
    }
}