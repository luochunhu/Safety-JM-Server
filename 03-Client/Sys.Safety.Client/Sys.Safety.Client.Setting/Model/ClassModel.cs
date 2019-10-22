using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Setting;
using Sys.Safety.Request.Class;
using Basic.Framework.Service;

namespace Sys.Safety.Client.Setting.Model
{
    public class ClassModel
    {
        private IClassService classService = ServiceFactory.Create<IClassService>();

        public void SaveClassList(List<ClassInfo> list)
        {
            var request = new ClassListAddRequest() { ClassInfoList = list };
            classService.SaveClassList(request);
        }

        public void SaveClass(ClassInfo dto, int state)
        {
            var request = new SaveClassByConditionRequest() { ClassInfo = dto,  State= state };
            classService.SaveClassByCondition(request);
        }

        public void DeleteClass(ClassDeleteRequest dto)
        {
            var request = new ClassDeleteRequest() { Id = dto.Id };
            classService.DeleteClass(request);
        }

        public void DeleteClassBySql(string sKey)
        {
            //TODO
        }

        public ClassInfo GetClassDtoByKey(string sKey)
        {
            var request = new GetClassByStrNameRequest() { StrName = sKey };
            var response = classService.GetClassByStrName(request);
            return response.Data;
        }
    }
}
