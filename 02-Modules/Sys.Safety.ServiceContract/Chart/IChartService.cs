using System.Collections.Generic;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.Listex;

namespace Sys.Safety.ServiceContract.Chart
{
    public interface IChartService
    {
        #region 曲线公共方法

        BasicResponse<string> GetDBType();

        /// <summary>
        ///     最后一次更新数据库的时间
        /// </summary>
        /// <returns></returns>
        BasicResponse<string> GetLastUpdateRealTime();

        /// <summary>
        ///     查找所有测点信息
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        BasicResponse<IList<Jc_DefInfo>> QueryAllPointCache();
        /// <summary>
        /// 根据设备性质获取测点列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> QueryPointCacheByDevpropertID(GetPointCacheByDevpropertIDRequest request);


        BasicResponse<string> GetDBName();

        /// <summary>
        ///     根据时间查找测点列表信息
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="Type">测点类型： 1:模拟量，2：开关量,3:所有测点</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetPointList(GetPointListRequest request);

        /// <summary>
        ///     获取测点的控制口列表信息
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetPointKzList(GetPointKzListRequest request);

        /// <summary>
        ///     获取测点的单位信息
        /// </summary>
        /// <param name="CurrentDevid">设备ID</param>
        /// <returns></returns>
        BasicResponse<string> GetPointDw(IdRequest request);

        /// <summary>
        ///     获取设备的基本信息
        /// </summary>
        /// <param name="CurrentWzid">当前位置ID</param>
        /// <param name="CurrentDevid">设备ID</param>
        BasicResponse<string[]> ShowPointInf(ShowPointInfRequest request);

        /// <summary>
        ///     获取报警阈值
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        BasicResponse<List<float>> GetZFromTable(PointIdRequest request);

        /// <summary>
        ///     返回开关量的状态定义信息
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        BasicResponse<List<string>> GetKglStateDev(PointIdRequest request);

        #endregion

        #region 模拟量，开关量曲线查询算法

        /// <summary>
        ///     获取模拟量月曲线数据
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">位置ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMonthLine(GetMonthLineRequest request);

        /// <summary>
        ///     模拟量5分钟曲线数据
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">位置ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetFiveMiniteLine(GetFiveMiniteLineRequest request);

        /// <summary>
        ///     5分钟曲线获取 某一时刻的最大值、最小值、平均值
        /// </summary>
        /// <param name="QxDate">时间</param>
        /// <param name="DtStart">未用</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <returns></returns>
        BasicResponse<string[]> GetDataVale(GetDataValeRequest request);

        /// <summary>
        ///     5分钟曲线 查询当前时刻断电范围、报警/解除、断电/复电、馈电状态、措施及时刻
        /// </summary>
        /// <param name="QxDate">当前时间</param>
        /// <param name="DtStart">未用</param>
        /// <param name="DtEnd">未用</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <returns></returns>
        BasicResponse<string[]> GetValue(GetValueRequest request);

        /// <summary>
        ///     开关量状态变化 曲线、状态统计列表、柱状图的绑定数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<GetStateBarTableResponse> GetStateBarTable(GetStateBarTableRequest request);

        /// <summary>
        ///     开关量状态变化 列表显示数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetStateChgdt(GetStateChgdtRequest request);

        /// <summary>
        ///     开关量曲线数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetStateLineDt(GetStateLineDtRequest request);

        /// <summary>
        ///     查询模拟量报警、断电、馈电异常记录
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="type">1:模拟量报警，2：模拟量断电</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMnlBjLineDt(GetMnlBjLineDtRequest request);

        /// <summary>
        ///     获取控制量馈电异常记录
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKzlLineDt(GetKzlLineDtRequest request);

        /// <summary>
        /// 获取控制量状态变化曲线
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKzlStateLineDt(GetStateLineDtRequest request);

        /// <summary>
        ///     开关量曲线、柱状图 查询断电范围、报警/解除、断电/复电、馈电状态、措施及时刻
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<string[]> GetDgView(GetDgViewRequest request);

        /// <summary>
        ///     开关量柱状图 统计每小时开机率
        /// </summary>
        /// <param name="DtStart">开始时间</param>
        /// <param name="DtEnd">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<string[]> GetKjThings(GetKjThingsRequest request);

        /// <summary>
        ///     开关量柱状图数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        BasicResponse<DataTable> InitQxZhuZhuang(InitQxZhuZhuangRequest request);

        /// <summary>
        ///     模拟量密采曲线数据
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="flag">是否根据开始时间结束时间过滤数据</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="TimeTick">时间间隔（密采值，1分钟，1小时）</param>
        /// <returns></returns>
        BasicResponse<Dictionary<string, DataTable>> GetMcData(GetMcDataRequest request);

        /// <summary>
        ///     模拟量月柱状图数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMonthBar(GetMonthBarRequest request);

        #endregion

        #region WEB 接口

        /// <summary>
        ///     得到模拟量5分钟曲线数据
        /// </summary>
        /// <param name="_listdate">选择的日期集合，格式为20160401</param>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMLLFiveLine(GetMLLFiveLineRequest request);

        /// <summary>
        ///     得到模拟量小时曲线数据
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMHourLine(GetMHourLineRequest request);

        /// <summary>
        ///     得到模拟量5分钟曲线界面grid数据
        /// </summary>
        /// <param name="dattime">点击曲线上的时间</param>
        /// <param name="PointID">选择的测点</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMLLFiveGirdData(GetMLLFiveGirdDataRequest request);

        /// <summary>
        ///     得到模拟量月柱状图数据
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMLLMonthBar(GetMLLMonthBarRequest request);

        /// <summary>
        ///     得到模拟量密采曲线
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <param name="Type">密采值类型：0：密采值，1：1分钟，2：1小时</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMLLMCLine(GetMLLMCLineRequest request);

        /// <summary>
        ///     得到开关量曲线情况
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKGLLState(GetKGLLStateRequest request);

        /// <summary>
        ///     得到开关量状态图Grid数据
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKGLStateGirdData(GetKGLStateGirdDataRequest request);

        /// <summary>
        ///     开关量柱状图（开关量状态统计）
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<string[]> GetKJXL(GetKJXLRequest request);

        /// <summary>
        ///     得到开关量状态变化图右边Grid数据(JC_R)
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKGLStateGridDataByRight(GetKGLStateGridDataByRightRequest request);

        /// <summary>
        ///     模拟量报警曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMnlBJLine(GetMnlBJLineRequest request);

        /// <summary>
        ///     模拟量断电曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetMnlDDLine(GetMnlDDLineRequest request);

        /// <summary>
        ///     获取模拟量测点对应的控制量测点
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetPointKzByPointID(GetPointKzByPointIDRequest request);

        /// <summary>
        ///     获取控制量馈电异常曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetKzlLine(GetKzlLineRequest request);

        /// <summary>
        ///     获取模拟量、开关量测点信息
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="Type">测点类型 0:模拟量，1：开关量</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetPointByType(GetPointByTypeRequest request);

        #endregion

        #region 曲线配置

        /// <summary>
        ///     保存曲线配置
        /// </summary>
        /// <param name="ChartSet"></param>
        /// <returns></returns>
        BasicResponse<bool> SaveChartSet(SaveChartSetRequest request);

        /// <summary>
        ///     获取曲线配置
        /// </summary>
        /// <param name="strKey">曲线配置key,""表示返回所有</param>
        /// <returns></returns>
        BasicResponse<DataTable> GetAllChartSet(GetAllChartSetRequest request);

        #endregion
    }
}