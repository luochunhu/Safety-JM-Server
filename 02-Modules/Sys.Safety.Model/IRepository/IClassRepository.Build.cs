using System.Collections.Generic;
using Basic.Framework.Data;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IClassRepository : IRepository<ClassModel>
    {
        ClassModel AddClass(ClassModel classModel);
        void UpdateClass(ClassModel classModel);
        void DeleteClass(string id);
        IList<ClassModel> GetClassList(int pageIndex, int pageSize, out int rowCount);
        IList<ClassModel> GetAllClassList();
        ClassModel GetClassById(string id);

        void DeleteClassByCode(string code);

        void AddClassList(IEnumerable<ClassModel> modelList);

        ClassModel GetClassByCode(string code);

        /// <summary>
        /// 根据strName获取ClassModel
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        ClassModel GetClassByStrName(string strName);

    }
}