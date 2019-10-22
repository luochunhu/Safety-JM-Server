using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Drawing;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.Listtemple;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Reports.Model
{

    public class ListTempleModel
    {
        //IListTempleService listexservice = ServiceFactory.CreateService<IListTempleService>();


        public void SaveListTemple(ListtempleInfo dto)
        {
            //IListTempleService listexservice = ServiceFactory.CreateService<IListTempleService>();
            IListtempleService listexservice = ServiceFactory.Create<IListtempleService>();
            var request = new SaveListTempleInfoRequest()
            {
                Info = dto
            };
            var ret = listexservice.SaveListTempleInfo(request);
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
        }

        public ListtempleInfo GetListTempleDTOByID(int ListDataID)
        {
            //IListTempleService listexservice = ServiceFactory.CreateService<IListTempleService>();
            IListtempleService listexservice = ServiceFactory.Create<IListtempleService>();
            //return listexservice.GetFirstItemByHQL("from ListTempleEntity where ListDataID="+ListDataID);
            var request = new IdRequest()
            {
                Id = ListDataID
            };
            var ret = listexservice.GetListtempleByListDataID(request);
            return ret.Data;
        }

        public DataTable GetNameFromListDataExListEx(int listDataId)
        {
            IListtempleService listexservice = ServiceFactory.Create<IListtempleService>();
            var req = new IdRequest()
            {
                Id = listDataId
            };
            var res = listexservice.GetNameFromListDataExListEx(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
    }
}
