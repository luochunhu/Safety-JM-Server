using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IEnumtypeRepository : IRepository<EnumtypeModel>
    {
        EnumtypeModel AddEnumtype(EnumtypeModel enumtypeModel);
        void UpdateEnumtype(EnumtypeModel enumtypeModel);
        void DeleteEnumtype(string id);
        IList<EnumtypeModel> GetEnumtypeList(int pageIndex, int pageSize, out int rowCount);
        List<EnumtypeModel> GetEnumtypeList();
        EnumtypeModel GetEnumtypeById(string id);
        EnumtypeModel GetEnumtypeByStrCode(string StrCode);
    }
}
