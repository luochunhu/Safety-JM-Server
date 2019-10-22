using System;
using System.Collections.Generic;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Class;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _Repository;

        public ClassService(IClassRepository _Repository)
        {
            //var repository = _Repository as Basic.Framework.Data.RepositoryBase<ClassModel>;
            //repository.QueryTableBySql("");
            this._Repository = _Repository;
        }

        public BasicResponse<ClassInfo> AddClass(ClassAddRequest classrequest)
        {
            var _class = ObjectConverter.Copy<ClassInfo, ClassModel>(classrequest.ClassInfo);
            var resultclass = _Repository.AddClass(_class);
            var classresponse = new BasicResponse<ClassInfo>();
            classresponse.Data = ObjectConverter.Copy<ClassModel, ClassInfo>(resultclass);
            return classresponse;
        }

        public BasicResponse<ClassInfo> UpdateClass(ClassUpdateRequest classrequest)
        {
            var _class = ObjectConverter.Copy<ClassInfo, ClassModel>(classrequest.ClassInfo);
            _Repository.UpdateClass(_class);
            var classresponse = new BasicResponse<ClassInfo>();
            classresponse.Data = ObjectConverter.Copy<ClassModel, ClassInfo>(_class);
            return classresponse;
        }

        public BasicResponse DeleteClass(ClassDeleteRequest classrequest)
        {
            _Repository.DeleteClass(classrequest.Id);
            var classresponse = new BasicResponse();
            return classresponse;
        }

        public BasicResponse<List<ClassInfo>> GetClassList(ClassGetListRequest classrequest)
        {
            var classresponse = new BasicResponse<List<ClassInfo>>();
            classrequest.PagerInfo.PageIndex = classrequest.PagerInfo.PageIndex - 1;
            if (classrequest.PagerInfo.PageIndex < 0)
                classrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var classModelLists = _Repository.GetClassList(classrequest.PagerInfo.PageIndex,
                classrequest.PagerInfo.PageSize, out rowcount);
            var classInfoLists = new List<ClassInfo>();
            foreach (var item in classModelLists)
            {
                var ClassInfo = ObjectConverter.Copy<ClassModel, ClassInfo>(item);
                classInfoLists.Add(ClassInfo);
            }
            classresponse.Data = classInfoLists;
            return classresponse;
        }
        public BasicResponse<List<ClassInfo>> GetAllClassList()
        {
            var classresponse = new BasicResponse<List<ClassInfo>>();
           
            var classModelLists = _Repository.GetAllClassList();
            var classInfoLists = new List<ClassInfo>();
            foreach (var item in classModelLists)
            {
                var ClassInfo = ObjectConverter.Copy<ClassModel, ClassInfo>(item);
                classInfoLists.Add(ClassInfo);
            }
            classresponse.Data = classInfoLists;
            return classresponse;
        }

        public BasicResponse<ClassInfo> GetClassById(ClassGetRequest classrequest)
        {
            var result = _Repository.GetClassById(classrequest.Id);
            var classInfo = ObjectConverter.Copy<ClassModel, ClassInfo>(result);
            var classresponse = new BasicResponse<ClassInfo>();
            classresponse.Data = classInfo;
            return classresponse;
        }

        public BasicResponse SaveClassList(ClassListAddRequest list)
        {
            if (list == null || list.ClassInfoList == null || list.ClassInfoList.Count < 1)
            {
                BasicResponse mr = new BasicResponse(-100, "Class列表对象为空，请检查是否已赋值！");
                return mr;
            }

            //var _class = ObjectConverter.CopyList<ClassInfo, ClassModel>(list.ClassInfoList);
            //DataColumn[] dc = new DataColumn[]
            //{
            //    new DataColumn("ClassID"), 
            //    new DataColumn("StrCode"),
            //    new DataColumn("StrName"), 
            //    new DataColumn("DatStart"),
            //    new DataColumn("DatEnd")
            //};
            //_Repository.BulkInserToDB("cbf_class", _class, dc);

            var _class = ObjectConverter.CopyList<ClassInfo, ClassModel>(list.ClassInfoList);
            _Repository.AddClassList(_class);
            return new BasicResponse(100, "操作成功。");
        }

        /// <summary>
        /// 根据班次编码删除记录
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public BasicResponse DeleteClassByCode(ClassCodeRequest code)
        {
            _Repository.DeleteClassByCode(code.Code);
            return new BasicResponse();
        }

        public BasicResponse<ClassInfo> GetClassDtoByCode(ClassCodeRequest code)
        {
            var model = _Repository.GetClassByCode(code.Code);
            var classInfo = ObjectConverter.Copy<ClassModel, ClassInfo>(model);
            var mr = new BasicResponse<ClassInfo>
            {
                Data = classInfo
            };
            return mr;
        }

        /// <summary>
        /// 根据strName获取
        /// </summary>
        /// <param name="classrequest"></param>
        /// <returns></returns>
        public BasicResponse<ClassInfo> GetClassByStrName(GetClassByStrNameRequest classrequest)
        {
            var respsone = new BasicResponse<ClassInfo>();
            if (string.IsNullOrWhiteSpace(classrequest.StrName))
            {
                respsone.Message = "参数错误！";
                respsone.Code = -100;
                return respsone;
            }
            try
            {
                var classmodel = _Repository.GetClassByStrName(classrequest.StrName);
                var classinfo = ObjectConverter.Copy<ClassModel, ClassInfo>(classmodel);
                respsone.Data = classinfo;
            }
            catch (Exception ex)
            {
                respsone.Message = ex.Message;
                respsone.Code = -100;
                return respsone; ;
            }

            return respsone;
        }

        /// <summary>
        /// 根据条件保存classInfo
        /// </summary>
        /// <param name="classrequest"></param>
        /// <returns></returns>
        public BasicResponse SaveClassByCondition(SaveClassByConditionRequest classrequest)
        {
            var response = new BasicResponse();
            if (!classrequest.State.HasValue || classrequest.ClassInfo == null)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                if (classrequest.State == 1)
                {
                    classrequest.ClassInfo.ClassID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    var classModel = ObjectConverter.Copy<ClassInfo, ClassModel>(classrequest.ClassInfo);
                    _Repository.AddClass(classModel);
                }
                else if (classrequest.State == 2)
                {
                    var classModel = ObjectConverter.Copy<ClassInfo, ClassModel>(classrequest.ClassInfo);
                    _Repository.Update(classModel);
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}