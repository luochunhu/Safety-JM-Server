using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Setanalysismodelpointrecord;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface ISetAnalysisModelPointRecordService
    {
        BasicResponse<JC_SetAnalysisModelPointRecordInfo> AddJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordAddRequest jC_Setanalysismodelpointrecordrequest);
        BasicResponse<JC_SetAnalysisModelPointRecordInfo> UpdateJC_Setanalysismodelpointrecord(SetAnalysisModelPointRecordUpdateRequest jC_Setanalysismodelpointrecordrequest);
        BasicResponse DeleteJC_Setanalysismodelpointrecord(SetanalysismodelpointrecordDeleteRequest jC_Setanalysismodelpointrecordrequest);
        BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetJC_SetanalysismodelpointrecordList(SetAnalysisModelPointRecordGetListRequest jC_Setanalysismodelpointrecordrequest);
        BasicResponse<JC_SetAnalysisModelPointRecordInfo> GetJC_SetanalysismodelpointrecordById(SetAnalysisModelPointRecordGetRequest jC_Setanalysismodelpointrecordrequest);
        /// <summary>
        /// 通过模板Id, 获取新增表达式测点的模板
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordGetTempleteRequest">请求对象包含模板Id信息</param>
        /// <returns>新增表达式测点的模板</returns>
        BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAddAnalysisModelPointRecordInfoByTempleteId(SetAnalysisModelPointRecordGetTempleteRequest jc_SetAnalysisModelPointRecordGetTempleteRequest);

        /// <summary>
        /// 获取和模型相关的友好的编辑表达式测点信息
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordGetTempleteRequest">请求对象包含模型Id</param>
        /// <returns>和模型相关的友好的编辑表达式测点信息</returns>
        BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationEditAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest);


        /// <summary>
        /// 获取和模型相关的友好的表达式测点信息.
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordByModelIdGetRequest">请求对象包含模型Id</param>
        /// <returns>和模型相关的友好的表达式测点信息</returns>
        BasicResponse<IList<JC_SetAnalysisModelPointRecordInfo>> GetCustomizationAnalysisModelPointRecordInfoByModelId(SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest);
        /// <summary>
        /// 根据模型ID查询模型配置的测点信息(包括表达式)
        /// </summary>
        /// <param name="jc_SetAnalysisModelPointRecordByModelIdGetRequest">请求对象包含模型Id</param>
        /// <returns>测点信息</returns>
        BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetSetAnalysisModelPointRecordByLargedataAnalysisConfigId(
            SetAnalysisModelPointRecordByModelIdGetRequest jc_SetAnalysisModelPointRecordByModelIdGetRequest);

        /// <summary>
        /// 获取分析模型对应的测点关联列表。
        /// </summary>
        /// <param name="id">分析模型Id</param>
        /// <returns>分析模型对应的测点关联列表</returns>
        BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAnalysisModelPointRecordsByAnalysisModelId(SetAnalysisModelPointRecordByModelIdGetRequest setanalysismodelpointrecordrequest);
        /// <summary>
        /// 查询所有模型测点信息
        /// </summary>
        BasicResponse<List<JC_SetAnalysisModelPointRecordInfo>> GetAllAnalysisModelPointList();

    }
}

