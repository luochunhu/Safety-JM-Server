using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sys.Safety.Model
{
    public interface IKJ_AddresstypeRepository : IRepository<KJ_AddresstypeModel>
    {
                KJ_AddresstypeModel AddKJ_Addresstype(KJ_AddresstypeModel kJ_AddresstypeModel);
		        void UpdateKJ_Addresstype(KJ_AddresstypeModel kJ_AddresstypeModel);
	            void DeleteKJ_Addresstype(string id);
		        IList<KJ_AddresstypeModel> GetKJ_AddresstypeList(int pageIndex, int pageSize, out int rowCount);
				KJ_AddresstypeModel GetKJ_AddresstypeById(string id);
    }
}
