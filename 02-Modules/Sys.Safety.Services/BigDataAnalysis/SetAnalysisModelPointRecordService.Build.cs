using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;

namespace Sys.Safety.Services
{
    public partial class SetAnalysisModelPointRecordService : ISetAnalysisModelPointRecordService
    {
        private ISetAnalysisModelPointRecordRepository _Repository;

        public SetAnalysisModelPointRecordService(ISetAnalysisModelPointRecordRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> AddJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordAddRequest jC_Setanalysismodelpointrecordrequest)
        {
            var _jC_Setanalysismodelpointrecord = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo, JC_SetanalysismodelpointrecordModel>(jC_Setanalysismodelpointrecordrequest.JC_SetAnalysisModelPointRecordInfo);
            var resultjC_Setanalysismodelpointrecord = _Repository.AddJC_Setanalysismodelpointrecord(_jC_Setanalysismodelpointrecord);
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse<JC_SetAnalysisModelPointRecordInfo>();
            jC_Setanalysismodelpointrecordresponse.Data = ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel, JC_SetAnalysisModelPointRecordInfo>(resultjC_Setanalysismodelpointrecord);
            return jC_Setanalysismodelpointrecordresponse;
        }
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> UpdateJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordUpdateRequest jC_Setanalysismodelpointrecordrequest)
        {
            var _jC_Setanalysismodelpointrecord = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo, JC_SetanalysismodelpointrecordModel>(jC_Setanalysismodelpointrecordrequest.JC_SetAnalysisModelPointRecordInfo);
            _Repository.UpdateJC_Setanalysismodelpointrecord(_jC_Setanalysismodelpointrecord);
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse<JC_SetAnalysisModelPointRecordInfo>();
            jC_Setanalysismodelpointrecordresponse.Data = ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel, JC_SetAnalysisModelPointRecordInfo>(_jC_Setanalysismodelpointrecord);
            return jC_Setanalysismodelpointrecordresponse;
        }
        public BasicResponse DeleteJC_Setanalysismodelpointrecord(SetanalysismodelpointrecordDeleteRequest jC_Setanalysismodelpointrecordrequest)
        {
            _Repository.DeleteJC_Setanalysismodelpointrecord(jC_Setanalysismodelpointrecordrequest.Id);
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse();
            return jC_Setanalysismodelpointrecordresponse;
        }
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetJC_SetanalysismodelpointrecordList(SetAnalysisModelPointRecordGetListRequest jC_Setanalysismodelpointrecordrequest)
        {
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>();
            jC_Setanalysismodelpointrecordrequest.PagerInfo.PageIndex = jC_Setanalysismodelpointrecordrequest.PagerInfo.PageIndex - 1;
            if (jC_Setanalysismodelpointrecordrequest.PagerInfo.PageIndex < 0)
            {
                jC_Setanalysismodelpointrecordrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_SetanalysismodelpointrecordModelLists = _Repository.GetJC_SetanalysismodelpointrecordList(jC_Setanalysismodelpointrecordrequest.PagerInfo.PageIndex, jC_Setanalysismodelpointrecordrequest.PagerInfo.PageSize, out rowcount);
            var jC_SetanalysismodelpointrecordInfoLists = new List<JC_SetAnalysisModelPointRecordInfo>();
            foreach (var item in jC_SetanalysismodelpointrecordModelLists)
            {
                var JC_SetAnalysisModelPointRecordInfo = ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel, JC_SetAnalysisModelPointRecordInfo>(item);
                jC_SetanalysismodelpointrecordInfoLists.Add(JC_SetAnalysisModelPointRecordInfo);
            }
            jC_Setanalysismodelpointrecordresponse.Data = jC_SetanalysismodelpointrecordInfoLists;
            return jC_Setanalysismodelpointrecordresponse;
        }

        /// <summary>
        /// 获取分析模型对应的测点关联列表。
        /// </summary>
        /// <param name="id">分析模型Id</param>
        /// <returns>分析模型对应的测点关联列表</returns>
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAnalysisModelPointRecordsByAnalysisModelId(SetAnalysisModelPointRecordByModelIdGetRequest setanalysismodelpointrecordrequest)
        {
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>();
            if (string.IsNullOrEmpty(setanalysismodelpointrecordrequest.AnalysisModelId))
            {
                return new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "分析模型Id不能为空" };
            }
            var jC_SetanalysismodelpointrecordModelLists = _Repository.GetAnalysisModelPointRecordsByAnalysisModelId(setanalysismodelpointrecordrequest.AnalysisModelId);
            var jC_SetanalysismodelpointrecordInfoLists = new List<JC_SetAnalysisModelPointRecordInfo>();
            foreach (var item in jC_SetanalysismodelpointrecordModelLists)
            {
                var JC_SetAnalysisModelPointRecordInfo = ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel, JC_SetAnalysisModelPointRecordInfo>(item);
                jC_SetanalysismodelpointrecordInfoLists.Add(JC_SetAnalysisModelPointRecordInfo);
            }
            jC_Setanalysismodelpointrecordresponse.Data = jC_SetanalysismodelpointrecordInfoLists;
            return jC_Setanalysismodelpointrecordresponse;
        }
        public BasicResponse<JC_SetAnalysisModelPointRecordInfo> GetJC_SetanalysismodelpointrecordById(SetAnalysisModelPointRecordGetRequest jC_Setanalysismodelpointrecordrequest)
        {
            var result = _Repository.GetJC_SetanalysismodelpointrecordById(jC_Setanalysismodelpointrecordrequest.Id);
            var jC_SetanalysismodelpointrecordInfo = ObjectConverter.Copy<JC_SetanalysismodelpointrecordModel, JC_SetAnalysisModelPointRecordInfo>(result);
            var jC_Setanalysismodelpointrecordresponse = new BasicResponse<JC_SetAnalysisModelPointRecordInfo>();
            jC_Setanalysismodelpointrecordresponse.Data = jC_SetanalysismodelpointrecordInfo;
            return jC_Setanalysismodelpointrecordresponse;
        }

        /// <summary>
        /// 通过模板Id, 获取新增表达式测点的模板
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordGetTempleteRequest">请求对象包含模板Id信息</param>
        /// <returns>新增表达式测点的模板</returns>
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(SetAnalysisModelPointRecordGetTempleteRequest jc_SetAnalysisModelPointRecordGetTempleteRequest)
        {
            if (string.IsNullOrEmpty(jc_SetAnalysisModelPointRecordGetTempleteRequest.TemplateId))
            {
                return new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "模板Id不能为空" };
            }

            DataTable dataTable = _Repository.QueryTable("dataAnalysis_GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId", jc_SetAnalysisModelPointRecordGetTempleteRequest.TemplateId);

            List<JC_SetAnalysisModelPointRecordInfo> listResult = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo>(dataTable);

            var jc_SetAnalysisModelPointRecordResponse = new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>();
            jc_SetAnalysisModelPointRecordResponse.Data = listResult;
            return jc_SetAnalysisModelPointRecordResponse;
        }

        /// <summary>
        /// 获取和模型相关的友好的编辑表达式测点信息
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordGetTempleteRequest">请求对象包含模型Id</param>
        /// <returns>和模型相关的友好的编辑表达式测点信息</returns>
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationEditAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            if (string.IsNullOrEmpty(jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId))
            {
                return new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "分析模型Id不能为空" };
            }
            DataTable dataTable = _Repository.QueryTable("dataAnalysis_GetCustomizationEditAnalysisModelPointRecordInfoByModelId", jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId);

            List<JC_SetAnalysisModelPointRecordInfo> listResult = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo>(dataTable);

            var jc_SetAnalysisModelPointRecordResponse = new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>();
            jc_SetAnalysisModelPointRecordResponse.Data = listResult;
            return jc_SetAnalysisModelPointRecordResponse;
        }

        /// <summary>
        /// 获取和模型相关的友好的表达式测点信息.
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordByModelIdGetRequest">请求对象包含模型Id</param>
        /// <returns>和模型相关的友好的表达式测点信息</returns>
        public BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            if (string.IsNullOrEmpty(jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId))
            {
                return new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "分析模型Id不能为空" };
            }
            DataTable dataTable = _Repository.QueryTable("dataAnalysis_GetCustomizationAnalysisModelPointRecordInfoByModelId", jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId);

            List<JC_SetAnalysisModelPointRecordInfo> listResult = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo>(dataTable);

            var jc_SetAnalysisModelPointRecordResponse = new BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>>();
            jc_SetAnalysisModelPointRecordResponse.Data = listResult;
            return jc_SetAnalysisModelPointRecordResponse;
        }

        /// <summary>
        /// 根据模型ID查询模型配置的测点信息(包括表达式)
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordByModelIdGetRequest">请求对象包含模型Id</param>
        /// <returns>测点信息</returns>
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest)
        {
            if (string.IsNullOrEmpty(jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId))
            {
                return new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "分析模型Id不能为空" };
            }
            DataTable dataTable = _Repository.QueryTable("global_SetAnalysisModelPointRecordService_GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId", jc_SetAnalysisModelPointRecordByModelIdGetRequest.AnalysisModelId);

            List<JC_SetAnalysisModelPointRecordInfo> listResult = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo>(dataTable);

            var jc_SetAnalysisModelPointRecordResponse = new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>();
            jc_SetAnalysisModelPointRecordResponse.Data = listResult;
            return jc_SetAnalysisModelPointRecordResponse;
        }

        /// <summary>
        /// 查询所有模型测点信息
        /// </summary>
        public BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAllAnalysisModelPointList()
        {
            try
            {
                DataTable dataTable = _Repository.QueryTable("global_SetAnalysisModelPointRecordService_GetAllAnalysisModelPointList");

                List<JC_SetAnalysisModelPointRecordInfo> listResult = ObjectConverter.Copy<JC_SetAnalysisModelPointRecordInfo>(dataTable);

                var jc_SetAnalysisModelPointRecordResponse = new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>();
                jc_SetAnalysisModelPointRecordResponse.Data = listResult;
                return jc_SetAnalysisModelPointRecordResponse;
            }
            catch 
            {
                
                 return new BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>>() { Data = new List<JC_SetAnalysisModelPointRecordInfo>(), Code = 1, Message = "数据查询失败" };
            }
        }
    }
}


