using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Chart;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    /// <summary>
    ///     Chart查询条件及相关方法
    /// </summary>
    public class QueryConditions
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        ///     返回历史测点列表信息
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="Wzid">位置ID</param>
        /// <param name="DevId">设备类型ID</param>
        /// <param name="Type">1:模拟量，2：开关量，3：所有测点</param>
        /// <returns></returns>
        public List<string> GetPointList(DateTime SzNameS, DateTime SzNameE, int Type, ref List<string> PointIDList,
            ref List<string> DevList, ref List<string> WzList)
        {
            var ReturnStr = new List<string>();
            var dt = new DataTable();
            SzNameS = Convert.ToDateTime(SzNameS.ToShortDateString());
            SzNameE = Convert.ToDateTime(SzNameE.ToShortDateString() + " 23:59:59");
            try
            {
                PointIDList.Clear();
                DevList.Clear();
                WzList.Clear();
                //dt = ServiceFactory.CreateService<IChartService>().GetPointList(SzNameS, SzNameE, Type);
                var req = new GetPointListRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    Type = Type
                };
                var res = _chartService.GetPointList(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
                DataView dv = dt.DefaultView;
                dv.Sort = "point asc";
                dt = dv.ToTable();
                foreach (DataRow Dr in dt.Rows)
                {
                    if (DateTime.Parse(Dr["DeleteTime"].ToString()) != DateTime.Parse("1900-01-01"))
                    {
                        ReturnStr.Add(Dr["point"] + "." + Dr["wz"] + "[" + Dr["name"] + "]" + "-" + Dr["DeleteTime"]);
                    }
                    else
                    {
                        ReturnStr.Add(Dr["point"] + "." + Dr["wz"] + "[" + Dr["name"] + "]");
                    }
                    PointIDList.Add(Dr["PointID"].ToString());
                    DevList.Add(Dr["devid"].ToString());
                    WzList.Add(Dr["wzid"].ToString());
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryConditions_GetPointList" + Ex.Message + Ex.StackTrace);
            }
            return ReturnStr;
        }
        /// <summary>
        /// 获取所有活动点
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="Type"></param>
        /// <param name="PointIDList"></param>
        /// <param name="DevList"></param>
        /// <param name="WzList"></param>
        /// <returns></returns>
        public List<string> GetActivePointList(int Type, ref List<string> PointIDList,
            ref List<string> DevList, ref List<string> WzList)
        {
            var ReturnStr = new List<string>();
            var dt = new DataTable();

            try
            {
                PointIDList.Clear();
                DevList.Clear();
                WzList.Clear();
                GetPointCacheByDevpropertIDRequest request = new GetPointCacheByDevpropertIDRequest();
                request.DevpropertID = Type;
                var res = _chartService.QueryPointCacheByDevpropertID(request);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                res.Data = res.Data.OrderBy(a => a.Point).ToList();
                if (res.Data != null)
                {
                    foreach (Jc_DefInfo point in res.Data)
                    {
                        ReturnStr.Add(point.Point + "." + point.Wz + "[" + point.DevName + "]");
                        PointIDList.Add(point.PointID);
                        DevList.Add(point.Devid);
                        WzList.Add(point.Wzid);
                    }
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryConditions_GetPointList" + Ex.Message + Ex.StackTrace);
            }
            return ReturnStr;
        }

        /// <summary>
        ///     获取所有被控测点（包括本地、交叉控制的点）
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="PointID"></param>
        /// <param name="PointIDList"></param>
        /// <param name="DevList"></param>
        /// <param name="WzList"></param>
        /// <returns></returns>
        public List<string> GetPointKzList(DateTime SzNameS, DateTime SzNameE, string PointID,
            ref List<string> PointIDList, ref List<string> DevList, ref List<string> WzList)
        {
            var ReturnStr = new List<string>();
            var dt = new DataTable();
            SzNameS = Convert.ToDateTime(SzNameS.ToShortDateString());
            SzNameE = Convert.ToDateTime(SzNameE.ToShortDateString() + " 23:59:59");
            try
            {
                PointIDList.Clear();
                DevList.Clear();
                WzList.Clear();
                //dt = ServiceFactory.CreateService<IChartService>().GetPointKzList(SzNameS, SzNameE, PointID);
                var req = new GetPointKzListRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    PointID = PointID
                };
                var res = _chartService.GetPointKzList(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;

                DataView dataView = dt.DefaultView;
                dataView.Sort = "point asc";
                dt = dataView.ToTable();

                foreach (DataRow Dr in dt.Rows)
                {
                    ReturnStr.Add(Dr["point"] + "." + Dr["wz"] + "[" + Dr["name"] + "]");
                    PointIDList.Add(Dr["PointID"].ToString());
                    DevList.Add(Dr["devid"].ToString());
                    WzList.Add(Dr["wzid"].ToString());
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryConditions_GetPointList" + Ex.Message + Ex.StackTrace);
            }
            return ReturnStr;
        }

        /// <summary>
        ///     返回测点定义信息
        /// </summary>
        /// <returns></returns>
        public List<string> GetPointList()
        {
            var ReturnStr = new List<string>();
            var strSql = "";
            var dt = new DataTable();
            try
            {
                //dt =
                //    Basic.Framework.Utils.Generic.TypeConvert.ToDataTable<JCDEFDTO>(
                //        ServiceFactory.CreateService<IChartService>().QueryAllPointCache());
                var res = _chartService.QueryAllPointCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = ObjectConverter.ToDataTable(res.Data);

                dt.DefaultView.Sort = "Point ASC";
                dt = dt.DefaultView.ToTable();

                foreach (DataRow Dr in dt.Rows)
                    if (Dr["DevPropertyID"].ToString() == "1")
                        ReturnStr.Add(Dr["point"] + "." + Dr["wz"] + "[" + Dr["devname"] + "]");

            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryConditions_GetPointList" + Ex.Message + Ex.StackTrace);
            }
            return ReturnStr;
        }
    }
}