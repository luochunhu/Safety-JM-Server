using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.Client.DataAnalysis.BusinessModel;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Business
{
    public class LargedataAnalysisConfigBusiness
    {        //模型
        ILargedataAnalysisConfigService largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();

        //大数据分析客户端服务
        ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        /// <summary>
        /// 初始化大数据分析模型名称列表
        /// </summary>
        /// <returns></returns>
        public List<JC_LargedataAnalysisConfigInfo> LoadAnalysisTemplate()
        {
            //初始化大数据分析模型名称
            //调用接口
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> analysisTemplateResult =
                largedataAnalysisConfigService.GetAllLargeDataAnalysisConfigList(new LargedataAnalysisConfigGetListRequest());

            return analysisTemplateResult.Data;
        }

        /// <summary>
        /// 获取所有设备种类
        /// </summary>
        /// <returns>获取所有设备种类</returns>
        public List<EnumcodeInfo> GetAllDeviceClassCache()
        {
            BasicResponse< List < EnumcodeInfo >> response = largeDataAnalysisCacheClientService.GetAllDeviceClassCache(new DeviceClassCacheGetAllRequest());
            if (response.IsSuccess && response.Data != null)
                return response.Data;
            return new List<EnumcodeInfo>();
        }

        /// <summary>
        /// 大数据曲线查询可查看的模型
        /// </summary>
        /// <param name="queryDate">查询日期</param>
        /// <returns>大数据曲线查询可查看的模型</returns>
        public List<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigListForCurve(string queryDate)
        {
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> analysisTemplateResult =
                largedataAnalysisConfigService.GetLargeDataAnalysisConfigListForCurve(new LargeDataAnalysisConfigListForCurveRequest() { QueryDate = queryDate });

            return analysisTemplateResult.Data;
        }

        /// <summary>
        /// 获取没有关联报警通知的分析模型
        /// </summary>
        /// <returns>获取没有关联报警通知的分析模型</returns>
        public List<JC_LargedataAnalysisConfigInfo> GetAnalysisModelWithoutAlarmConfig()
        {
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response =
               largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithoutAlarmConfigList(new LargedataAnalysisConfigGetListRequest());

            return response.Data;
        }

        /// <summary>
        /// 获取关联有报警配置的分析模型 2017-06-26 
        /// </summary>
        /// <param name="analysisModelName">分析模型名称</param>
        /// <returns>获取关联有报警配置的分析模型</returns>
        public List<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigWithRegionOutage(string analysisModelName)
        {
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response =
               largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithRegionOutage(new LargedataAnalysisConfigGetListWithRegionOutageRequest() { AnalysisModelName = analysisModelName });

            return response.Data;
        }
        /// <summary>
        /// 获取关联有报警配置的分析模型 2017-06-26   
        /// 2017-07-13 追加分页 
        /// </summary>
        /// <param name="analysisModelName">分析模型名称</param>
        /// <returns>获取关联有报警配置的分析模型</returns>
        public LargedataAnalysisConfigBusinessModel GetLargeDataAnalysisConfigWithRegionOutagePage(string analysisModelName, int pageIndex = 1, int pageSize = 0)
        {
            LargedataAnalysisConfigBusinessModel model = new LargedataAnalysisConfigBusinessModel();
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response =
               largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithRegionOutagePage(new LargedataAnalysisConfigGetListWithRegionOutageRequest() {
                   AnalysisModelName = analysisModelName,
                   PagerInfo = new PagerInfo()
                   {
                       PageSize = pageSize,
                       PageIndex = pageIndex
                   }
               });

            model.pagerInfo = response.PagerInfo;
            if (response.Data != null && response.Data.Count > 0)
            {
                model.LargedataAnalysisConfigInfoList = response.Data;
            }
            else
            {
                model.LargedataAnalysisConfigInfoList = new List<JC_LargedataAnalysisConfigInfo>();
            }

            return model;
        }

        /// <summary>
        /// 获取没有关联报警配置的分析模型 2017-06-26 
        /// </summary>
        /// <returns>没有关联报警配置的分析模型</returns>
        public List<JC_LargedataAnalysisConfigInfo> GetLargeDataAnalysisConfigWithoutRegionOutage()
        {
            Basic.Framework.Web.BasicResponse<List<JC_LargedataAnalysisConfigInfo>> response =
               largedataAnalysisConfigService.GetLargeDataAnalysisConfigWithoutRegionOutage(new LargedataAnalysisConfigGetListRequest());

            return response.Data;
        }

        /// <summary>
        /// 添加分析模型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string AddLargedataAnalysisConfig(LargedataAnalysisConfigBusinessModel data)
        {
            BasicResponse<JC_LargedataAnalysisConfigInfo> largedataAnalysisConfigResult = new BasicResponse<JC_LargedataAnalysisConfigInfo>();
            if (data.LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList.GroupBy(g => g.ParameterId).ToList().Count == 1 && data.LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList[0].PointId != "")
            {
                int transferCount = 20;
                //单参数.
                string[] points = data.LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList[0].PointId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (points.Length <= transferCount)
                {
                    largedataAnalysisConfigResult =
                       largedataAnalysisConfigService.AddLargeDataAnalysisConfig(
                       new LargedataAnalysisConfigAddRequest()
                       {
                           JC_LargedataAnalysisConfigInfo = data.LargedataAnalysisConfigInfo
                       });
                }
                else
                {
                    DevExpress.Utils.WaitDialogForm wdf;
                    StringBuilder pointString = new StringBuilder();
                    int k = points.Length / transferCount;
                    int m = points.Length % transferCount;
                    for (int i = 0; i < k; i++)
                    {
                        wdf = new DevExpress.Utils.WaitDialogForm(string.Format("正在保存第{0}到第{1}个分析模型,共{2}个分析模型...", i * transferCount + 1, (i + 1) * transferCount, points.Length), "请等待...");
                        pointString.Clear();
                        for (int j = 0; j < transferCount; j++)
                        {
                            pointString.Append(points[i * transferCount + j]).Append(",");
                        }

                        foreach (var item in data.LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList)
                        {
                            item.PointId = pointString.ToString().TrimEnd(',');
                        }
                        try
                        {
                            largedataAnalysisConfigResult = largedataAnalysisConfigService.AddLargeDataAnalysisConfig(
                           new LargedataAnalysisConfigAddRequest()
                           {
                               JC_LargedataAnalysisConfigInfo = data.LargedataAnalysisConfigInfo
                           });
                        }
                        catch (Exception ex)
                        {
                            //log
                        }
                        finally
                        {
                            wdf.Close();
                        }
                    }
                    if (m > 0)
                    {
                        wdf = new DevExpress.Utils.WaitDialogForm(string.Format("正在保存第{0}到第{1}个分析模型,共{2}个分析模型...", k * transferCount + 1, points.Length, points.Length), "请等待...");
                        pointString.Clear();
                        for (int n = k * transferCount; n < points.Length; n++)
                        {
                            pointString.Append(points[n]).Append(",");
                        }
                        foreach (var item in data.LargedataAnalysisConfigInfo.AnalysisModelPointRecordInfoList)
                        {
                            item.PointId = pointString.ToString().TrimEnd(',');
                        }
                        try
                        {
                            largedataAnalysisConfigResult = largedataAnalysisConfigService.AddLargeDataAnalysisConfig(
                           new LargedataAnalysisConfigAddRequest()
                           {
                               JC_LargedataAnalysisConfigInfo = data.LargedataAnalysisConfigInfo
                           });
                        }
                        catch (Exception ex)
                        {
                            //log
                        }
                        finally
                        {
                            wdf.Close();
                        }
                    }
                }
            }
            else
            {
                largedataAnalysisConfigResult =
                       largedataAnalysisConfigService.AddLargeDataAnalysisConfig(
                       new LargedataAnalysisConfigAddRequest()
                       {
                           JC_LargedataAnalysisConfigInfo = data.LargedataAnalysisConfigInfo
                       });
            }

            if (largedataAnalysisConfigResult.IsSuccess)
            {
                data.LargedataAnalysisConfigInfo = largedataAnalysisConfigResult.Data;
                return string.Empty;
            }
            else
            {
                return largedataAnalysisConfigResult.Message;
            }
        }

        /// <summary>
        /// 编辑分析模型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EditLargedataAnalysisConfig(LargedataAnalysisConfigBusinessModel data)
        {
            BasicResponse<JC_LargedataAnalysisConfigInfo> largedataAnalysisConfigResult =
                       largedataAnalysisConfigService.UpdateLargeDataAnalysisConfig(
                       new LargedataAnalysisConfigUpdateRequest()
                       {
                           JC_LargedataAnalysisConfigInfo = data.LargedataAnalysisConfigInfo
                       });

            if (largedataAnalysisConfigResult.IsSuccess)
            {
                data.LargedataAnalysisConfigInfo = largedataAnalysisConfigResult.Data;
                return string.Empty;
            }
            else
            {
                return largedataAnalysisConfigResult.Message;
            }


        }

        public JC_LargedataAnalysisConfigInfo GetLargeDataAnalysisConfigById(string analysisModelId)
        {
            BasicResponse<JC_LargedataAnalysisConfigInfo> analysisModelResponse = largedataAnalysisConfigService.GetLargeDataAnalysisConfigById(new LargedataAnalysisConfigGetRequest() { Id = analysisModelId });
            return analysisModelResponse.Data;
        }
        /// <summary>
        /// 根据模型ID查询模型详细信息
        /// </summary>
        /// <param name="analysisModelId"></param>
        /// <returns></returns>
        public List<JC_LargedataAnalysisConfigInfo> GetLargedataAnalysisConfigDetailById(string analysisModelId)
        {
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> analysisModelResponse = largedataAnalysisConfigService.GetLargedataAnalysisConfigDetailById(new LargedataAnalysisConfigGetRequest() { Id = analysisModelId });
            List<JC_LargedataAnalysisConfigInfo> largedataAnalysisConfigInfo = new List<JC_LargedataAnalysisConfigInfo>();
            largedataAnalysisConfigInfo = analysisModelResponse.Data;


            return largedataAnalysisConfigInfo;
        }
        /// <summary>
        /// 根据模型名称模糊查询模型列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LargedataAnalysisConfigBusinessModel GetLargeDataAnalysisConfigListByName(string name, int pageIndex = 1, int pageSize = 0)
        {
            LargedataAnalysisConfigBusinessModel model = new LargedataAnalysisConfigBusinessModel();
            BasicResponse<List<JC_LargedataAnalysisConfigInfo>> analysisModelResponse = largedataAnalysisConfigService.GetLargeDataAnalysisConfigListByName(
                new LargedataAnalysisConfigGetListByNameRequest()
                {
                    Name = name,
                    PagerInfo = new PagerInfo()
                   {
                       PageSize = pageSize,
                       PageIndex = pageIndex
                   }
                });
            model.pagerInfo = analysisModelResponse.PagerInfo;
            if (analysisModelResponse.Data != null && analysisModelResponse.Data.Count > 0)
            {
                model.LargedataAnalysisConfigInfoList = analysisModelResponse.Data;
            }
            else
            {
                model.LargedataAnalysisConfigInfoList = new List<JC_LargedataAnalysisConfigInfo>();
            }

            return model;
        }
        /// <summary>
        /// 模型ID删除模型
        /// </summary>
        /// <param name="analysisModelId"></param>
        /// <returns></returns>
        public string DeleteLargedataAnalysisConfigById(string analysisModelId)
        {

            BasicResponse analysisModelResponse =
                largedataAnalysisConfigService.DeleteLargeDataAnalysisConfig(new LargedataAnalysisConfigDeleteRequest() { Id = analysisModelId });

            if (analysisModelResponse.IsSuccess == true)
            {
                return "100";
            }
            return analysisModelResponse.Message;
        }
    }
}
