using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Sys.Safety.ServiceContract
{
    /// <summary>
    /// 报表服务
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// 测试服务接口
        /// </summary>
        BasicResponse<string> SaveDoTest();

        /// <summary>
        /// 获取报表方案集合
        /// </summary>
        /// <param name="reportRequest">报表ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetListData(ReportGetListDataRequest reportRequest);

        /// <summary>
        /// 得到报表需要显示的栏目(第一次打开报表时候需要调用此方法)
        /// </summary>
        /// <param name="reportRequest">报表主ID</param>
        /// <returns>返回具体有哪些需要显示的栏目</returns>
        BasicResponse<DataTable> Getlistdisplayex(ReportGetlistdisplayexRequest reportRequest);


        /// <summary>
        /// 得到报表需要显示的栏目(第一次打开报表时候需要调用此方法)目前是成都旗客在调用
        /// </summary>
        /// <param name="reportRequest">报表主ID,报表方案</param>
        /// <returns>返回具体有哪些需要显示的栏目</returns>
        BasicResponse<DataTable> WebGetlistdisplayex(ReportWebGetlistdisplayexRequest reportRequest);

        /// <summary>
        /// 得到报表的数据源
        /// </summary>
        /// <param reportRequest="ListID">报表主ID,报表条件(传参数的时候参数要转换为测点ID),报表的日期段集合,对像以yyyymmdd、yyyymm、yyyymmddhh格式传递,开始页数,显示行数</param>
        /// <returns>返回具体的数据</returns>
        BasicResponse<DataTable> GetReportData(ReportGetReportDataRequest reportRequest);


        /// <summary>
        /// 得到报表的数据源（目前是成都旗客在调用）
        /// </summary>
        /// <param reportRequest="ListID">报表主ID,报表方案ID,报表条件(传参数的时候参数要转换为测点ID),报表的日期段集合,对像以yyyymmdd、yyyymm、yyyymmddhh格式传递,开始页数,显示行数</param>
        /// <returns>返回具体的数据</returns>
        BasicResponse<DataTable> WebGetReportData(ReportWebGetReportDataRequest reportRequest);

        /// <summary>
        /// 得到报表查询条件参照数据
        /// </summary>
        /// <param name="strFKCode">参照编码</param>
        /// <returns>返回json/returns>
        BasicResponse<string> GetFKData(ReportGetFKDataRequest reportRequest);

        /// <summary>
        /// 获取ListTemplate
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        BasicResponse<ListtempleInfo> GetListTemple(ReportGetListTempleRequest reportRequest);


        /// <summary>
        /// 得到报表查询条件参照数据
        /// </summary>
        /// <param name="strFKCode">参照编码</param>
        /// <returns>返回datatable</returns>
        BasicResponse<Hashtable> GetFKDataByHashTable(ReportGetFKDataByHashTableRequest reportRequest);

        /// <summary>
        /// 得到一个设备类型(大气里面叫就是因子)的质量等级详情
        /// </summary>
        /// <param name="PointID">监测因子PointID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetEnvirLevelDetail(ReportGetEnvirLevelDetailRequest reportRequest);

        /// <summary>
        /// 根据监测站fzh得到此监测站的参数设备(用于加载监测站选择后加载参数标签)监测和因子是fzh相同，kh不同，kh=0表示是监测站、kh<>0表示是监测因子
        /// </summary>
        /// <param name="PointID">fzh</param>
        /// <returns>返回参数设备</returns>
        BasicResponse<DataTable> GetPointParameter(ReportGetPointParameterRequest reportRequest);

        /// <summary>
        /// 查询多站点(单站点)对比分析 （对应原型中统计分析菜单）
        /// </summary>
        /// <param name="reportRequest">选择的监测站Ids(如1,2,3),选择的的设备类型id,选择的日期段(如between '2015-11-12 00:00:00' and '2015-11-13 23:59:59'),选择的时间月集合(如_listdate.Add(201601))</param>
        /// <returns></returns>
        BasicResponse<DataSet> GetMorePointData(ReportGetMorePointDataRequest reportRequest);

        /// <summary>
        /// 得到一共有多少条记录
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        BasicResponse<int> GetReportTotalRecord(ReportGetReportTotalRecordRequest reportRequest);
    }
}
