using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class AlarmNotificationPersonnelBusiness
    {
        IAlarmNotificationPersonnelService alarmNotificationPersonnelService = ServiceFactory.Create<IAlarmNotificationPersonnelService>();
        IAlarmNotificationPersonnelConfigService alarmNotificationPersonnelConfigService = ServiceFactory.Create<IAlarmNotificationPersonnelConfigService>();

        /// <summary>
        /// 新增报警推送人员信息 dataType : add 新增  edit 修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddAlarmNotificationPersonnelConfig(AlarmNotificationPersonnelConfigBusinessModel data, string dataType)
        {
            string error = "";
            try
            {
                if (dataType == "add")
                {
                    //新增报警配置信息
                    BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> alarmNotificationPersonnelConfigInfoResult =
                      alarmNotificationPersonnelConfigService.AddAlarmNotificationPersonnelConfig(
                      new AlarmNotificationPersonnelConfigListAddRequest()
                      {
                          JC_AlarmNotificationPersonnelConfigListInfo = data.AlarmNotificationPersonnelConfigInfoList,
                          JC_AlarmNotificationPersonnelInfoList = data.AlarmNotificationPersonnelInfoList
                      });

                    if (!alarmNotificationPersonnelConfigInfoResult.IsSuccess)
                    {
                        error = alarmNotificationPersonnelConfigInfoResult.Message;
                    }
                    else
                    {
                        error = "100";
                    }
                }
                else
                {
                    //修改报警配置信息
                    BasicResponse<JC_AlarmNotificationPersonnelConfigInfo> alarmNotificationPersonnelConfigInfoResult =
                  alarmNotificationPersonnelConfigService.UpdateJC_AlarmNotificationPersonnelConfig(
                  new AlarmNotificationPersonnelConfigUpdateRequest()
                  {
                      JC_AlarmNotificationPersonnelConfigInfo = data.AlarmNotificationPersonnelConfigInfo,
                      JC_AlarmNotificationPersonnelInfoList = data.AlarmNotificationPersonnelInfoList
                  });

                    if (!alarmNotificationPersonnelConfigInfoResult.IsSuccess || alarmNotificationPersonnelConfigInfoResult.Code == -100)
                    {
                        error = alarmNotificationPersonnelConfigInfoResult.Message;
                    }
                    else
                    {
                        error = "100";
                    }
                }
                return error;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return ex.Message;
            }
        }


        /// <summary>
        /// 查询报警推送人员信息
        /// </summary>
        /// <param name="id">模型ID</param>
        /// <returns></returns>
        public AlarmNotificationPersonnelConfigBusinessModel GetAlarmNotificationPersonnelByanalysisModelId(string analysisModelId)
        {
            try
            {
                AlarmNotificationPersonnelConfigBusinessModel businessModel = new AlarmNotificationPersonnelConfigBusinessModel();
                BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> alarmNotificationPersonnelInfoResult =
                    alarmNotificationPersonnelService.GetAlarmNotificationPersonnelByAnalysisModelId(
                    new AlarmNotificationPersonnelGetListByAlarmConfigIdRequest() { AnalysisModelId = analysisModelId }
                    );

                businessModel.AlarmNotificationPersonnelInfoList = alarmNotificationPersonnelInfoResult.Data;

                BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> alarmNotificationPersonnelConfigInfoResult =
                     alarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigByAnalysisModelId(
                     new AlarmNotificationPersonnelConfigGetListRequest() { AnalysisModelId = analysisModelId });


                if (alarmNotificationPersonnelConfigInfoResult.Data != null && alarmNotificationPersonnelConfigInfoResult.Data.Count > 0)
                {
                    businessModel.AlarmNotificationPersonnelConfigInfo = alarmNotificationPersonnelConfigInfoResult.Data[0];
                }
                return businessModel;


            }
            catch (Exception ex)
            {
                return new AlarmNotificationPersonnelConfigBusinessModel();
            }

        }

        /// <summary>
        /// 验证数据是新增还是修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns>dataType  新增模板  add   修改模板  edit</returns>
        public string CheckAlarmNotificationPersonnelConfig(string id)
        {
            try
            {
                BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> alarmNotificationPersonnelConfigInfoResult =
                      alarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelConfigByAnalysisModelId(
                      new AlarmNotificationPersonnelConfigGetListRequest() { AnalysisModelId = id });

                if (alarmNotificationPersonnelConfigInfoResult.IsSuccess == false)
                {
                    return alarmNotificationPersonnelConfigInfoResult.Message;
                }
                else
                {
                    if (alarmNotificationPersonnelConfigInfoResult.Data != null && alarmNotificationPersonnelConfigInfoResult.Data.Count > 0)
                    {
                        return "edit";
                    }
                    else
                    {
                        return "add";
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                return ex.Message;
            }

        }
        /// <summary>
        /// 删除报警推送信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public string DeleteJC_AlarmNotificationPersonnelConfig(List<string> ids)
        {
            try
            {
                //1.模板
                //调用接口
                Basic.Framework.Web.BasicResponse analysisTemplateResult =
                    alarmNotificationPersonnelConfigService.DeleteJC_AlarmNotificationPersonnelConfig(
                    new AlarmNotificationPersonnelConfigDeleteRequest() { ids = ids });

                if (analysisTemplateResult.IsSuccess == false)
                {
                    return analysisTemplateResult.Message;
                }
                else
                {
                    if (analysisTemplateResult.Code != 100)
                        return analysisTemplateResult.Message;
                    else
                        return "100";
                }

            }
            catch (Exception ex)
            {
                return "-100";
            }

        }
        /// <summary>
        /// 根据模型名称模糊查询报警推送配置信息
        /// </summary>
        /// <param name="AnalysisModeName">模型名称</param>
        /// <returns></returns>
        public AlarmNotificationPersonnelConfigBusinessModel GetAlarmNotificationPersonnelListByAnalysisModeName(string AnalysisModeName, int pageIndex = 1, int pageSize = 0)
        {
            try
            {
                AlarmNotificationPersonnelConfigBusinessModel businessModel = new AlarmNotificationPersonnelConfigBusinessModel();
                BasicResponse<List<JC_AlarmNotificationPersonnelConfigInfo>> alarmNotificationPersonnelConfigInfoResult =
                    alarmNotificationPersonnelConfigService.GetAlarmNotificationPersonnelListByAnalysisModeName(
                    new AlarmNotificationPersonnelConfigGetListRequest() { AnalysisModeName = AnalysisModeName 
                    ,
                                                                           PagerInfo = new PagerInfo()
                                                                           {
                                                                               PageSize = pageSize,
                                                                               PageIndex = pageIndex
                                                                           }
                    }
                    );
                businessModel.pagerInfo = alarmNotificationPersonnelConfigInfoResult.PagerInfo;
                if (alarmNotificationPersonnelConfigInfoResult.Data != null && alarmNotificationPersonnelConfigInfoResult.Data.Count > 0)
                {
                    businessModel.AlarmNotificationPersonnelConfigInfoList = alarmNotificationPersonnelConfigInfoResult.Data;
                }
                else
                {
                    businessModel.AlarmNotificationPersonnelConfigInfoList = new List<JC_AlarmNotificationPersonnelConfigInfo>();
                }
                return businessModel;


            }
            catch (Exception ex)
            {
                return new AlarmNotificationPersonnelConfigBusinessModel();
            }

        }
    }
}
