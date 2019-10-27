using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.Chart;
using Sys.Safety.Enums;

namespace Sys.Safety.Services.Chart
{
    public class ChartService : IChartService
    {
        private RepositoryBase<ListexModel> _listexRepositoryBase;
        private IPointDefineCacheService _pointDefineCacheService;
        private IPointDefineRepository _jc_DefRepository;
        private IDeviceDefineRepository _deviceDefineRepository;

        public ChartService(IListexRepository listexRepository, IPointDefineCacheService pointDefineCacheService, IPointDefineRepository jc_DefRepository, IDeviceDefineRepository _DeviceDefineRepository)
        {
            _listexRepositoryBase = listexRepository as RepositoryBase<ListexModel>;
            _pointDefineCacheService = pointDefineCacheService;
            _jc_DefRepository = jc_DefRepository;
            _deviceDefineRepository = _DeviceDefineRepository;
        }

        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("TblRefSszService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            //switch (_baseDAO.getServerType())
            //{
            //    case "local": //local
            //        throw ex;
            //        break;
            //    case "wcf": //wcf
            //        throw new FaultException(ex.Message);
            //        break;
            //    default:
            //        throw ex;
            //        break;
            //}
            throw ex;
        }

        #region 曲线公共方法

        /// <summary>
        ///     获取数据库类型
        /// </summary>
        /// <returns>返回数据库类型字符串（SQLServer,MySQL,Oracle）</returns>
        public BasicResponse<string> GetDBType()
        {
            //return DBManager.DBType.ToString();
            var dbType = Basic.Framework.Configuration.Global.DatabaseType;
            var ret = new BasicResponse<string>
            {
                Code = 100,
                Message = "操作成功。",
                Data = dbType.ToString().ToLower()
            };
            return ret;
        }

        /// <summary>
        ///     最后一次更新数据库的时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse<string> GetLastUpdateRealTime()
        {
            var time = "";
            try
            {
                //time = Basic.Framework.Core.Context.ServerContext.Current.GetContextItem("DC", "_LastUpdateRealTime").ToString();
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//直接取当前时间  20170422
                //修改为读数据库 
                //var strsql = "select timer from flag";
                //var dt = GetDataTableBySQL(strsql);
                //if (dt.Rows.Count > 0)
                //    time = dt.Rows[0]["timer"].ToString();
            }
            catch (Exception ex)
            {
                // LogHelper.Error("获取定义改变时间", ex);
                ThrowException("GetLastUpdateRealTime", ex);
            }
            var ret = new BasicResponse<string>()
            {
                Data = time
            };
            return ret;
        }

        /// <summary>
        ///     查找所有实时活动的测点信息
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public BasicResponse<IList<Jc_DefInfo>> QueryAllPointCache()
        {
            //_Cache._CacheMrg.ServerCache.Cache_jc_def._rwLocker.AcquireReaderLock(-1);

            //IList<Jc_DefInfo> result = new List<Jc_DefInfo>();
            //try
            //{
            //    foreach (JCDEFDTO item in _Cache._CacheMrg.ServerCache.Cache_jc_def._JC_DEF.Values)
            //    {
            //        if (item.DTOState == Framework.Core.Service.DTO.DTOStateEnum.Delete)
            //        {
            //            continue;
            //        }
            //        result.Add(item);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ThrowException("QueryAllPointCache", ex);
            //}
            //finally
            //{
            //    if (_Cache._CacheMrg.ServerCache.Cache_jc_def._rwLocker.IsReaderLockHeld)
            //    {
            //        _Cache._CacheMrg.ServerCache.Cache_jc_def._rwLocker.ReleaseReaderLock();
            //    }
            //}

            //result = _CacheMrg.ServerCache.Cache_jc_def.QueryAllCacheList();
            var res = _pointDefineCacheService.GetAllPointDefineCache(new PointDefineCacheGetAllRequest());
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = new BasicResponse<IList<Jc_DefInfo>>
            {
                Data = res.Data.FindAll(a => a.Activity == "1")
            };

            return ret;
        }
        /// <summary>
        /// 根据设备性质获取测点列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_DefInfo>> QueryPointCacheByDevpropertID(GetPointCacheByDevpropertIDRequest request)
        {
            BasicResponse<List<Jc_DefInfo>> result = new BasicResponse<List<Jc_DefInfo>>();
            PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
            pointDefineCacheRequest.Predicate = a => a.DevPropertyID == request.DevpropertID && a.Activity == "1";
            var res = _pointDefineCacheService.GetPointDefineCache(pointDefineCacheRequest);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            result.Data = res.Data;
            return result;
        }

        /// <summary>
        ///     获取数据库名称
        /// </summary>
        /// <returns></returns>
        public BasicResponse<string> GetDBName()
        {
            //return _baseDAO.getDBName();
            var ret = new BasicResponse<string>
            {
                Code = 100,
                Message = "操作成功。",
                Data = _listexRepositoryBase.DataContext.Database.Connection.Database
            };
            return ret;
        }

        /// <summary>
        ///     根据时间查找测点列表信息
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="Type">测点类型： 1:模拟量，2：开关量,3:所有测点</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetPointList(GetPointListRequest request)
        {
            var ReturnStr = new List<string>();
            var SzName = "";
            string SzSql = "", strSql = "";
            var SzTiaoJian = "";
            var dt = new DataTable();
            dt.TableName = "GetPointList";
            try
            {
                if (request.Type == 1)
                    SzTiaoJian += " KJ_DeviceType.type='1'";
                else if (request.Type == 2)
                    SzTiaoJian += " KJ_DeviceType.type='2'";
                else if (request.Type == 3)
                    SzTiaoJian += " (KJ_DeviceType.type='1' or KJ_DeviceType.type='2')";

                SzName = "KJ_DeviceDefInfo";
                //SzSql =
                //    "select distinct A.PointID,A.point,A.fzh,A.kh,A.dzh,KJ_DeviceAddress.wz as wz,KJ_DeviceType.name as name,A.wzid as wzid,A.devid as devid from " +
                //    SzName +
                //    @" as A left outer join KJ_DeviceType on KJ_DeviceType.devid=A.devid left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=A.wzid where " +
                //    SzTiaoJian + " and ((A.CreateUpdateTime<='" + request.SzNameE + "' and A.DeleteTime>='" +
                //    request.SzNameS +
                //    "') or (A.CreateUpdateTime<='" + request.SzNameE + "' and A.DeleteTime='1900-01-01 00:00:00'))" +
                //    " order by A.fzh,A.kh";
                dt = _listexRepositoryBase.QueryTable("global_ChartService_GetJcdef_ByStimeEtimeOrder", SzName, SzTiaoJian,
                    request.SzNameE, request.SzNameS);
                //if (SzSql.Length > 0)
                //    dt = GetDataTableBySQL(SzSql);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("GetPointList" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<DataTable>()
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     获取测点的控制口列表信息
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetPointKzList(GetPointKzListRequest request)
        {
            var ReturnStr = new List<string>();
            var SzName = "";
            string SzSql = "", strSql = "";
            var SzTiaoJian = "";
            var dt = new DataTable();
            dt.TableName = "GetPointKzList";
            var KzkList = new List<string>();
            try
            {
                //strSql = "select * from KJ_DeviceDefInfo where PointID='" + PointID + "'";
                //dt = GetDataTableBySQL(strSql);
                var model = _jc_DefRepository.GetPointDefineByPointId(request.PointID);
                dt = ObjectConverter.ToDataTable(model);
                if (dt.Rows.Count > 0)
                {
                    //获取本地控制的所有控制口
                    for (var k = 1; k < 8; k++)
                    {
                        var Kzk = int.Parse(dt.Rows[0]["K" + k].ToString());
                        var fzh = int.Parse(dt.Rows[0]["fzh"].ToString());
                        //for (var i = 0; i < 8; i++)
                        //    if ((Kzk & (1 << i)) == 1 << i)
                        //    {
                        //        var KzPoint = fzh.ToString("000") + "C" + (i + 1).ToString("00") + "0";
                        //        if (!KzkList.Contains(KzPoint))
                        //            KzkList.Add(KzPoint);
                        //    }
                        for (int i = 0; i < 24; i++)//计算智能断电器 
                        {
                            if ((Kzk & (1 << i)) == 1 << i)
                            {
                                string KzPoint = "";
                                if (i < 8)
                                {
                                    KzPoint = fzh.ToString("000") + "C" + (i + 1).ToString("00") + "0";
                                }
                                else
                                {
                                    KzPoint = fzh.ToString("000") + "C" + (i - 7).ToString("00") + "1";
                                }
                                if (!KzkList.Contains(KzPoint))
                                {
                                    KzkList.Add(KzPoint);
                                }
                            }
                        }
                    }
                    //获取交叉控制口
                    var Jckz1 = dt.Rows[0]["Jckz1"].ToString();
                    var Jckz2 = dt.Rows[0]["Jckz2"].ToString();
                    var Jckz3 = dt.Rows[0]["Jckz3"].ToString();
                    var Jckz1List = Jckz1.Split('|');
                    var Jckz2List = Jckz2.Split('|');
                    var Jckz3List = Jckz3.Split('|');
                    for (var j = 0; j < Jckz1List.Length; j++)
                        if (!string.IsNullOrEmpty(Jckz1List[j]))
                            if (!KzkList.Contains(Jckz1List[j]))
                                KzkList.Add(Jckz1List[j]);
                    for (var j = 0; j < Jckz2List.Length; j++)
                        if (!string.IsNullOrEmpty(Jckz2List[j]))
                            if (!KzkList.Contains(Jckz2List[j]))
                                KzkList.Add(Jckz2List[j]);
                    for (var j = 0; j < Jckz3List.Length; j++)
                        if (!string.IsNullOrEmpty(Jckz3List[j]))
                            if (!KzkList.Contains(Jckz3List[j]))
                                KzkList.Add(Jckz3List[j]);
                }
                var KzkTj = "";
                for (var i = 0; i < KzkList.Count; i++)
                    KzkTj += KzkList[i] + ",";
                if (KzkTj.Contains(','))
                    KzkTj = KzkTj.Substring(0, KzkTj.Length - 1);
                SzTiaoJian += " A.Point in ('" + KzkTj.Replace(",", "','") + "')";


                SzName = "KJ_DeviceDefInfo";
                //                SzSql =
                //                    "select distinct A.PointID,A.point,A.fzh,A.kh,A.dzh,KJ_DeviceAddress.wz as wz,KJ_DeviceType.name as name,A.wzid as wzid,A.devid as devid from " +
                //                    SzName + @" as A 
                //left outer join KJ_DeviceType on KJ_DeviceType.devid=A.devid 
                //left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=A.wzid 
                //where " + SzTiaoJian + " and ((A.CreateUpdateTime<='" + request.SzNameE + "' and A.DeleteTime>='" + request.SzNameS +
                //                    "') or (A.CreateUpdateTime<='" + request.SzNameE
                //                    + "' and A.DeleteTime='1900-01-01 00:00:00'))";
                //if (SzSql.Length > 0)
                //    dt = GetDataTableBySQL(SzSql);
                dt = _listexRepositoryBase.QueryTable("global_ChartService_GetJcdef_ByStimeEtime", SzName, SzTiaoJian,
                    request.SzNameE, request.SzNameS);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("GetPointKzList" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<DataTable>()
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     获取设备的单位信息
        /// </summary>
        /// <param name="CurrentDevid">设备ID</param>
        /// <returns></returns>
        public BasicResponse<string> GetPointDw(IdRequest request)
        {
            var dw = "";
            try
            {
                //var SzSql = "select Xs1 from KJ_DeviceType where devid='" + CurrentDevid + "'";
                //var dt = GetDataTableBySQL(SzSql);
                var model = _deviceDefineRepository.GetDeviceDefineByDevId(request.Id.ToString());
                var dt = ObjectConverter.ToDataTable(model);

                if (dt.Rows.Count > 0)
                    if (dt.Rows[0]["Xs1"].ToString() != "")
                        dw = dt.Rows[0]["Xs1"].ToString();
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getPointDw" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<string>
            {
                Data = dw
            };
            return ret;
        }

        /// <summary>
        ///     获取设备的基本信息
        /// </summary>
        /// <param name="CurrentWzid">当前位置ID</param>
        /// <param name="CurrentDevid">设备ID</param>
        public BasicResponse<string[]> ShowPointInf(ShowPointInfRequest request)
        {
            var QueryStr = new string[13];
            for (var i = 0; i <= 12; i++)
                QueryStr[i] = "";
            try
            {
                //                var strsql = @"select  KJ_DeviceAddress.wz,KJ_DeviceType.name,KJ_DeviceType.z2,KJ_DeviceType.z3,KJ_DeviceType.z4,KJ_DeviceType.z6,KJ_DeviceType.z7,KJ_DeviceType.z8,KJ_DeviceType.xs1 from KJ_DeviceType
                //left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + CurrentWzid + " where devid='" + CurrentDevid + "'";
                //                var showpointdt = GetDataTableBySQL(strsql);
                var showpointdt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdev_ByWzIdDevId", request.CurrentWzid,
                    request.CurrentPointId);
                if (showpointdt.Rows.Count > 0)
                {
                    QueryStr[0] = showpointdt.Rows[0]["wz"] + "\\" + showpointdt.Rows[0]["name"];
                    QueryStr[1] = showpointdt.Rows[0]["z2"] + "|" + showpointdt.Rows[0]["z6"];
                    QueryStr[2] = showpointdt.Rows[0]["z3"] + "|" + showpointdt.Rows[0]["z7"];
                    QueryStr[3] = showpointdt.Rows[0]["z4"] + "|" + showpointdt.Rows[0]["z8"];
                    //SzDw = showpointdt.Rows[0]["xs1"].ToString();                    
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_ShowPointInf" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<string[]>()
            {
                Data = QueryStr
            };
            return ret;
        }

        /// <summary>
        ///     获取报警阈值
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public BasicResponse<List<float>> GetZFromTable(PointIdRequest request)
        {
            var Rvalue = new List<float>();
            try
            {
                //var SzSql = "";

                //SzSql = "select z1,z2,z3,z4,z5,z6,z7,z8 from KJ_DeviceType where devid='" + devid + "'";
                //var dt = GetDataTableBySQL(SzSql);
                PointDefineCacheGetByConditonRequest requestPointGet = new PointDefineCacheGetByConditonRequest();
                requestPointGet.Predicate = a => a.PointID == request.PointId;
                var model = _pointDefineCacheService.GetPointDefineCache(requestPointGet);

                if (model.Data != null && model.Data.Count > 0)
                {//获取Z1~Z8  20170628
                    Rvalue.Add(model.Data[0].Z1);
                    Rvalue.Add(model.Data[0].Z2);
                    Rvalue.Add(model.Data[0].Z3);
                    Rvalue.Add(model.Data[0].Z4);
                    Rvalue.Add(model.Data[0].Z5);
                    Rvalue.Add(model.Data[0].Z6);
                    Rvalue.Add(model.Data[0].Z7);
                    Rvalue.Add(model.Data[0].Z8);
                }
                //for (var i = 0; i < dt.Columns.Count; i++)
                //    Rvalue.Add(float.Parse(dt.Rows[0][i].ToString()));
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_GetLcFromTable" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<List<float>>
            {
                Data = Rvalue
            };
            return ret;
        }

        /// <summary>
        ///     返回开关量的状态定义信息
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public BasicResponse<List<string>> GetKglStateDev(PointIdRequest request)
        {
            var rvalue = new List<string>();
            //var sz = "select KJ_DeviceType.xs1,KJ_DeviceType.xs2,KJ_DeviceType.xs3 from KJ_DeviceType where KJ_DeviceType.devid ='" + CurrentDevid + "'";
            //var tempdevdt = GetDataTableBySQL(sz);
            PointDefineCacheGetByConditonRequest requestPointGet = new PointDefineCacheGetByConditonRequest();
            requestPointGet.Predicate = a => a.PointID == request.PointId;
            var model = _pointDefineCacheService.GetPointDefineCache(requestPointGet);

            if (model.Data != null && model.Data.Count > 0)
            {
                rvalue.Add(model.Data[0].Bz6);
                rvalue.Add(model.Data[0].Bz7);
                rvalue.Add(model.Data[0].Bz8);
            }
            else
            {
                rvalue.Add("无");
                rvalue.Add("无");
                rvalue.Add("无");
            }
            var ret = new BasicResponse<List<string>>()
            {
                Data = rvalue
            };
            return ret;
        }


        /// <summary>
        ///     根据devid查询测点的量程
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public List<int> GetLcFromTable(string CurrentDevid)
        {
            var Rvalue = new List<int>();
            int Lc1 = 0, Lc2 = 0;
            try
            {
                //var SzSql = "";
                //SzSql = "select KJ_DeviceType.Lc,KJ_DeviceType.LC2 from KJ_DeviceType where devid='" + CurrentDevid + "'";
                //var dt = GetDataTableBySQL(SzSql);
                var model = _deviceDefineRepository.GetDeviceDefineByDevId(CurrentDevid);
                var dt = ObjectConverter.ToDataTable(model);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Lc"].ToString() != "")
                        Lc1 = Convert.ToInt32(dt.Rows[0]["lc"]);
                    if ((dt.Rows[0]["LC2"].ToString() != "") && (dt.Rows[0]["LC2"].ToString() != "0")) //说明有第二量程
                        Lc2 = Convert.ToInt32(dt.Rows[0]["LC2"]);
                }
                Rvalue.Add(Lc1);
                Rvalue.Add(Lc2);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_GetLcFromTable" + Ex.Message + Ex.StackTrace);
            }
            return Rvalue;
        }

        /// <summary>
        ///     获取中心站时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCenterDateTime()
        {
            var Dttime = new DateTime();
            Dttime = DateTime.Now;
            try
            {
                var CenterTime = GetLastUpdateRealTime().Data;
                if (!string.IsNullOrEmpty(CenterTime))
                    Dttime = Convert.ToDateTime(CenterTime);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_GetCenterDateTime" + Ex.Message + Ex.StackTrace);
            }
            return Dttime;
        }

        /// <summary>
        ///     过滤分站中断、系统退出的重复记录
        /// </summary>
        /// <param name="Dt"></param>
        public void ConvertDt(ref DataTable Dt)
        {
            try
            {
                var IsGd = 0;
                for (var i = 0; i < Dt.Rows.Count; i++)
                    if (Dt.Rows[i]["kh"].ToString() == "0" || Dt.Rows[i]["type"].ToString() == "0" || Dt.Rows[i]["type"].ToString() == "46")//增加未知道状态判断  20170628
                    {
                        Dt.Rows[i]["type"] = "10";
                        if (IsGd == 1)
                        {
                            Dt.Rows.RemoveAt(i);
                            i--;
                        }
                        IsGd = 1;
                    }
                    else
                        IsGd = 0;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_ConvertDt" + Ex.Message + Ex.StackTrace);
            }
        }

        /// <summary>
        ///     处理时间重复的数据  20150909
        /// </summary>
        /// <param name="Dt"></param>
        public void ConvertTimeToDt(ref DataTable Dt)
        {
            //注释，增加毫秒后，不对重复数据进行过滤  20170916 
            //try
            //{
            //    for (var i = 0; i < Dt.Rows.Count; i++)
            //        if (i < Dt.Rows.Count - 1)
            //            if (Dt.Rows[i]["timer"].ToString() == Dt.Rows[i + 1]["timer"].ToString()) //时间相同过滤掉前面的数据
            //            {
            //                Dt.Rows.RemoveAt(i);
            //                i--;
            //            }
            //}
            //catch (Exception Ex)
            //{
            //    LogHelper.Error("QueryPubClass_ConvertTimeToDt" + Ex.Message + Ex.StackTrace);
            //}
        }
        /// <summary>
        /// 控制量曲线处理，增加同时时刻有馈电和控制记录，只保留馈电记录  20171102
        /// </summary>
        /// <param name="Dt"></param>
        public void KzlConvertTimeToDt(ref DataTable Dt)
        {
            try
            {

                DataView dv = Dt.DefaultView;
                dv.Sort = "timer";
                Dt = dv.ToTable();
                for (var i = Dt.Rows.Count - 1; i > 0; i--)
                {
                    if (i > 0)
                    {
                        if (Convert.ToDateTime(Dt.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss") == Convert.ToDateTime(Dt.Rows[i - 1]["timer"]).ToString("yyyy-MM-dd HH:mm:ss")) //时间相同过滤掉前面的数据
                        {
                            if (Dt.Rows[i]["type"].ToString() == "44" || Dt.Rows[i]["type"].ToString() == "43")
                            {
                                Dt.Rows.RemoveAt(i);
                            }
                            if (Dt.Rows[i - 1]["type"].ToString() == "44" || Dt.Rows[i - 1]["type"].ToString() == "43")
                            {
                                Dt.Rows.RemoveAt(i - 1);
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KzlConvertTimeToDt" + Ex.Message + Ex.StackTrace);
            }
        }
        /// <summary>
        ///     过滤状态重复的记录
        /// </summary>
        /// <param name="Dt"></param>
        public void ConvertTypeToDt(ref DataTable Dt)
        {
            try
            {
                for (var i = Dt.Rows.Count - 1; i > 0; i--)
                    if (Dt.Rows[i]["type"].ToString() == Dt.Rows[i - 1]["type"].ToString()) //时间相同过滤掉后面的数据
                        Dt.Rows.RemoveAt(i);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_ConvertTimeToDt" + Ex.Message + Ex.StackTrace);
            }
        }

        /// <summary>
        ///     查询当前测点的控制口信息
        /// </summary>
        /// <param name="Szpoint"></param>
        /// <returns></returns>
        public string GetKzk(string PointID)
        {
            var Szreturn = "";
            try
            {
                var SzFzh = GetPointFzh(PointID);

                //var strsql = "select JCkz1,JCkz2,jckz3,K1,K2,K3,K4,K5,K6,K7,K8 from KJ_DeviceDefInfo where PointID='" + PointID +
                //             "'";
                //var dt = GetDataTableBySQL(strsql);
                var model = _jc_DefRepository.GetPointDefineByPointId(PointID);
                var dt = ObjectConverter.ToDataTable(model);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["jckz1"].ToString() != "")
                        Szreturn = dt.Rows[0]["jckz1"].ToString();

                    if (dt.Rows[0]["jckz2"].ToString() != "")
                        if (Szreturn != "")
                            Szreturn += "|" + dt.Rows[0]["jckz2"];
                        else
                            Szreturn = dt.Rows[0]["jckz2"].ToString();
                    if (dt.Rows[0]["jckz3"].ToString() != "")
                        if (Szreturn != "")
                            Szreturn += "|" + dt.Rows[0]["jckz3"];
                        else
                            Szreturn = dt.Rows[0]["jckz3"].ToString();
                    var SSS = new List<string>();

                    if ((dt.Rows[0]["K1"].ToString() != "0") && (dt.Rows[0]["K1"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K1"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }

                    if ((dt.Rows[0]["K2"].ToString() != "0") && (dt.Rows[0]["K2"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K2"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K3"].ToString() != "0") && (dt.Rows[0]["K3"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K3"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K4"].ToString() != "0") && (dt.Rows[0]["K4"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K4"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K5"].ToString() != "0") && (dt.Rows[0]["K5"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K5"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K6"].ToString() != "0") && (dt.Rows[0]["K6"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K6"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K7"].ToString() != "0") && (dt.Rows[0]["K7"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K7"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    if ((dt.Rows[0]["K8"].ToString() != "0") && (dt.Rows[0]["K8"].ToString() != ""))
                    {
                        var BData = Convert.ToInt32(dt.Rows[0]["K8"]);
                        var bytes = BitConverter.GetBytes(BData);
                        var BDataL = bytes[0]; //取低8位
                        var BDataH = (short)((bytes[2] << 8) + bytes[1]); //取中间16位
                        for (byte i = 0; i < 8; i++)
                            if ((BDataL & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "0"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "0");
                        for (byte i = 0; i < 16; i++)
                            if ((BDataH & (1 << i)) == 1 << i)
                                if (!SSS.Contains(SzFzh + "C0" + (i + 1) + "1"))
                                    SSS.Add(SzFzh + "C0" + (i + 1) + "1");
                    }
                    for (var iv = 0; iv < SSS.Count; iv++)
                        if (Szreturn != "")
                            Szreturn += "|" + SSS[iv];
                        else
                            Szreturn = SSS[iv];
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("QueryPubClass_GetKzk" + Ex.Message + Ex.StackTrace);
            }
            return Szreturn;
        }

        /// <summary>
        ///     获取测点安装位置
        /// </summary>
        /// <param name="Szpoint"></param>
        /// <returns></returns>
        public string GetAddr(string PointID)
        {
            var SzREtu = "";
            try
            {
                //var strsql =
                //    "select KJ_DeviceAddress.wz as wz from KJ_DeviceDefInfo left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=KJ_DeviceDefInfo.wzid where PointID='" +
                //    PointID + "'";
                //var dt = GetDataTableBySQL(strsql);
                var dt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPointId", PointID);
                if (dt.Rows.Count > 0)
                    SzREtu = dt.Rows[0]["wz"].ToString();
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetAddr" + Ex.Message + Ex.StackTrace);
            }
            return SzREtu;
        }

        /// <summary>
        ///     获取当前曲线中的最大值作为量程高值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMaxBv(DataTable dt, string ColumnName)
        {
            float MaxValue = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                        if (float.Parse(dr[ColumnName].ToString()) > MaxValue)
                            MaxValue = float.Parse(dr[ColumnName].ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_getMaxBv" + ex.Message + ex.StackTrace);
            }
            return MaxValue;
        }

        /// <summary>
        ///     获取当前曲线中的最小值作为量程低值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public float getMinBv(DataTable dt, string ColumnName)
        {
            float MinValue = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                    if (!string.IsNullOrEmpty(dr[ColumnName].ToString()))
                        if (float.Parse(dr[ColumnName].ToString()) < MinValue)
                            MinValue = float.Parse(dr[ColumnName].ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_getMinBv" + ex.Message + ex.StackTrace);
            }
            return MinValue;
        }

        /// <summary>
        ///     根据测点的Point获取设备的分站号
        /// </summary>
        /// <param name="SzPoint"></param>
        /// <returns></returns>
        public string GetPointFzh(string PointID)
        {
            //查询测点的分站号 
            var TempFzh = "0";
            try
            {
                //var sz = "select fzh from KJ_DeviceDefInfo where PointID='" + PointID + "'";
                //var tempfzdt = GetDataTableBySQL(sz);
                var model = _jc_DefRepository.GetPointDefineByPointId(PointID);
                var tempfzdt = ObjectConverter.ToDataTable(model);

                if (tempfzdt.Rows.Count > 0)
                    TempFzh = tempfzdt.Rows[0]["fzh"].ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryPubClass_GetPointFzh" + ex.Message + ex.StackTrace);
            }
            return TempFzh;
        }

        /// <summary>
        ///     获取设备类型及位置ID
        /// </summary>
        /// <param name="PointID"></param>
        /// <returns>第一个为设备类型ID，第二个为位置ID</returns>
        public List<string> GetPointDevAndWzID(string PointID)
        {
            //查询测点的分站号 
            var Rvalue = new List<string>();
            try
            {
                //                var sz = @"select KJ_DeviceDefInfo.devid,KJ_DeviceDefInfo.wzid,KJ_DeviceType.name as devname,KJ_DeviceAddress.wz as wz from KJ_DeviceDefInfo 
                //left join KJ_DeviceType on KJ_DeviceType.devid=KJ_DeviceDefInfo.devid left join KJ_DeviceAddress on KJ_DeviceAddress.wzid=KJ_DeviceDefInfo.wzid where PointID='" + PointID +
                //                         "'  ";
                //                var tempfzdt = GetDataTableBySQL(sz);
                var tempfzdt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPointId2", PointID);
                if (tempfzdt.Rows.Count > 0)
                {
                    Rvalue.Add(tempfzdt.Rows[0]["devid"].ToString());
                    Rvalue.Add(tempfzdt.Rows[0]["wzid"].ToString());
                    Rvalue.Add(tempfzdt.Rows[0]["devname"].ToString());
                    Rvalue.Add(tempfzdt.Rows[0]["wz"].ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetPointDevAndWzID" + ex.Message + ex.StackTrace);
            }
            return Rvalue;
        }

        /// <summary>
        ///     将状态转换成文字输出
        /// </summary>
        /// <param name="msg">数字状态</param>
        /// <returns></returns>
        public string StateChange(string ms)
        {
            var msg = "未知";
            //修改从枚举中去加载对应的状态文本  20180131
            int msInt = 0;
            bool isInt = int.TryParse(ms, out msInt);
            if (isInt)
            {
                msg = EnumHelper.GetEnumDescription((DeviceDataState)msInt);
            }
            //switch (ms)
            //{
            //    case "0":
            //        msg = "通讯中断";
            //        break;
            //    case "1":
            //        msg = "通讯误码";
            //        break;
            //    case "2":
            //        msg = "初始化中";
            //        break;
            //    case "3":
            //        msg = "交流正常";
            //        break;
            //    case "4":
            //        msg = "直流正常";
            //        break;
            //    case "5":
            //        msg = "红外遥控";
            //        break;
            //    case "6":
            //        msg = "设备休眠";
            //        break;
            //    case "7":
            //        msg = "设备检修";
            //        break;
            //    case "8":
            //        msg = "上限预警";
            //        break;
            //    case "9":
            //        msg = "上预解除";
            //        break;
            //    case "10":
            //        msg = "上限报警";
            //        break;
            //    case "11":
            //        msg = "上报解除";
            //        break;
            //    case "12":
            //        msg = "上限断电";
            //        break;
            //    case "13":
            //        msg = "上断解除";
            //        break;
            //    case "14":
            //        msg = "下限预警";
            //        break;
            //    case "15":
            //        msg = "下预解除";
            //        break;
            //    case "16":
            //        msg = "下限报警";
            //        break;
            //    case "17":
            //        msg = "下报解除";
            //        break;
            //    case "18":
            //        msg = "下限断电";
            //        break;
            //    case "19":
            //        msg = "下断解除";
            //        break;
            //    case "20":
            //        msg = "断线";
            //        break;
            //    case "21":
            //        msg = "正常";
            //        break;
            //    case "22":
            //        msg = "上溢";
            //        break;
            //    case "23":
            //        msg = "负漂";
            //        break;
            //    case "24":
            //        msg = "设备标校";
            //        break;
            //    case "25":
            //        msg = "0态";
            //        break;
            //    case "26":
            //        msg = "1态";
            //        break;
            //    case "27":
            //        msg = "2态";
            //        break;
            //    case "28":
            //        msg = "开机";
            //        break;
            //    case "29":
            //        msg = "复电成功";
            //        break;
            //    case "30":
            //        msg = "复电失败";
            //        break;
            //    case "31":
            //        msg = "断电成功";
            //        break;
            //    case "32":
            //        msg = "断电失败";
            //        break;
            //    case "33":
            //        msg = "头子断线";
            //        break;
            //    case "34":
            //        msg = "类型有误";
            //        break;
            //    case "35":
            //        msg = "系统退出";
            //        break;
            //    case "36":
            //        msg = "系统启动";
            //        break;
            //    case "37":
            //        msg = "非法退出";
            //        break;
            //    case "38":
            //        msg = "过滤数据";
            //        break;
            //    case "39":
            //        msg = "热备日志";
            //        break;
            //    case "40":
            //        msg = "满足条件";
            //        break;
            //    case "41":
            //        msg = "不满足条件";
            //        break;
            //    case "42":
            //        msg = "线性突变";
            //        break;
            //    case "43":
            //        msg = "0态";
            //        break;
            //    case "44":
            //        msg = "1态";
            //        break;
            //    case "45":
            //        msg = "断线";
            //        break;
            //    case "46":
            //        msg = "未知";
            //        break;
            //    case "119":
            //        msg = "服务器中断";
            //        break;
            //}
            return msg;
        }

        #endregion

        #region 模拟量开关曲线查询私有方法

        /// <summary>
        ///     模拟量5分钟曲线处理算法
        /// </summary>
        /// <param name="timeNow"></param>
        /// <param name="dtqxData"></param>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        private DataTable CreatQXTDataFiveMin(DateTime timeNow, DataTable dtqxData, string CurrentDevid)
        {
            var dtR = new DataTable();
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("Av");
            dtR.Columns.Add("Bv");
            dtR.Columns.Add("Cv");
            dtR.Columns.Add("Dv");
            dtR.Columns.Add("Ev");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");
            dtR.Columns.Add("Timer");
            dtR.Columns.Add("state");
            try
            {
                //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
                float LsCount = 0;
                var icount = 0;
                var j = 0;
                var Fcount = 0;
                var Lc1 = 0;
                var Lc2 = 0;
                var Move = "";
                string intPjz = "0",
                    intzdz = "0",
                    intssz = "0",
                    intzxz = "0",
                    strtype = "",
                    strstate = "",
                    strtypetext = "",
                    timer = "";
                int ReMoveInt;
                var tempList = new List<int>();

                #region 获取当前设备的量程

                tempList = GetLcFromTable(CurrentDevid);
                if (tempList.Count > 0)
                {
                    Lc1 = tempList[0];
                    Lc2 = tempList[1];
                }

                #endregion

                ConvertTimeToDt(ref dtqxData);


                for (var i = 0; i < 288; i++)
                {
                    timer = timeNow.AddMinutes(double.Parse((5 * i).ToString())).ToString("yyyy-MM-dd HH:mm:ss");

                    //查找当前时间是否有5分钟数据
                    var dr = dtqxData.Select("timer='" + timer + "'");
                    if (dr.Length > 0)
                    {
                        if (dr[0]["ssz"].ToString() != "")
                            if (Convert.ToSingle(dr[0]["ssz"]) < 0)
                            {
                                intssz = "0.00001";
                            }
                            else
                            {
                                double Dpjz = Convert.ToSingle(dr[0]["ssz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        intssz = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        intssz = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        intssz = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        intssz = string.Format("{0:F1}", Dpjz);
                                //else
                                //    intssz = string.Format("{0:F0}", Dpjz);
                                //不处理数据  20180109
                                intssz = string.Format("{0:F2}", Dpjz);
                            }
                        else
                            intssz = "0.00001";

                        if (dr[0]["zdz"].ToString() != "")
                            if (Convert.ToSingle(dr[0]["zdz"]) < 0)
                                intzdz = "0.00001";
                            else
                            {
                                double Dpjz = Convert.ToSingle(dr[0]["zdz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        intzdz = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        intzdz = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        intzdz = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        intzdz = string.Format("{0:F1}", Dpjz);
                                //else
                                //    intzdz = string.Format("{0:F0}", Dpjz);
                                //不处理数据  20180109
                                intzdz = string.Format("{0:F2}", Dpjz);
                            }
                        else
                            intzdz = "0.00001";

                        if (dr[0]["pjz"].ToString() != "")
                            if (Convert.ToSingle(dr[0]["pjz"]) < 0)
                                intPjz = "0.00001";
                            else
                            {
                                double Dpjz = Convert.ToSingle(dr[0]["pjz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        intPjz = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        intPjz = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        intPjz = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        intPjz = string.Format("{0:F1}", Dpjz);
                                //else
                                //    intPjz = string.Format("{0:F0}", Dpjz);
                                //不处理数据  20180109
                                intPjz = string.Format("{0:F2}", Dpjz);
                            }
                        else
                            intPjz = "0.00001";


                        if (dr[0]["zxz"].ToString() != "")
                            //屏蔽此处，最小值可为负数  20170731
                            //if (Convert.ToSingle(dr[0]["zxz"]) < 0)
                            //    intzxz = "0.00001";
                            // else 
                            if (Convert.ToInt32(dr[0]["zxz"]) == 9999)
                                intzxz = "9999";
                            else
                            {
                                double Dpjz = Convert.ToSingle(dr[0]["zxz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        intzxz = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        intzxz = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        intzxz = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        intzxz = string.Format("{0:F1}", Dpjz);
                                //else
                                //    intzxz = string.Format("{0:F0}", Dpjz);
                                //不处理数据  20180109
                                intzxz = string.Format("{0:F2}", Dpjz);
                            }
                        else
                            intzxz = "0.00001";


                        //设备状态-曲线需要区分标校数据
                        if (dr[0]["state"].ToString() != "")
                        {
                            strstate = dr[0]["state"].ToString();
                        }

                        j = j + 1;
                        //}

                        if ((intPjz != "") && (intPjz != "0.00001") && (intzxz != "9999"))
                        {
                            Fcount++;
                            LsCount = LsCount + Convert.ToSingle(intPjz);
                            double Dpjz = LsCount / Fcount;
                            //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                            //    if (Dpjz > 9.995)
                            //        Move = string.Format("{0:F1}", Dpjz);
                            //    else
                            //        Move = string.Format("{0:F2}", Dpjz);
                            //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                            //    if (Dpjz > 99.995)
                            //        Move = string.Format("{0:F0}", Dpjz);
                            //    else
                            //        Move = string.Format("{0:F1}", Dpjz);
                            //else
                            //    Move = string.Format("{0:F0}", Dpjz);
                            //不处理数据  20180109
                            Move = string.Format("{0:F2}", Dpjz);
                        }
                        //Av.Append("Av[" + Index + "]=\"" + intssz.ToString() + "\";");
                        //Bv.Append("Bv[" + Index + "]=\"" + intzdz.ToString() + "\";");
                        //Cv.Append("Cv[" + Index + "]=\"" + intzxz.ToString() + "\";");
                        //Dv.Append("Dv[" + Index + "]=\"" + intPjz.ToString() + "\";");
                        //Ev.Append("Ev[" + Index + "]=\"" + Move + "\";");
                        var obj = new object[dtR.Columns.Count];
                        obj[0] = intssz;
                        obj[1] = intzdz;
                        obj[2] = intzxz;
                        obj[3] = intPjz;
                        obj[4] = Move;
                        obj[5] = strtype;
                        obj[6] = strtypetext;
                        obj[7] = timer;
                        obj[8] = strstate;
                        dtR.Rows.Add(obj);
                    }
                    else
                    {
                        var obj = new object[dtR.Columns.Count];
                        obj[0] = "0.00001";
                        obj[1] = "0.00001";
                        obj[2] = "0.00001";
                        obj[3] = "0.00001";
                        obj[4] = "0.00001";
                        obj[5] = "-1";
                        obj[6] = "未记录";
                        obj[7] = timer;
                        obj[8] = "-1";
                        dtR.Rows.Add(obj);
                    }
                }
                return dtR;
            }
            catch (Exception ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_CreatQXTData1" + ex.Message + ex.StackTrace);
                return dtR;
            }
            finally
            {
                dtR.Dispose();
            }
        }

        /// <summary>
        ///     查询断电区域（根据主控的分站号和口号）
        /// </summary>
        /// <param name="pointfzh"></param>
        /// <param name="pointkh"></param>
        /// <returns></returns>
        private string[] GetDdReplace(string CurrentPointID)
        {
            var QueryStr = new string[13];
            for (var i = 0; i <= 12; i++)
                QueryStr[i] = "";
            try
            {
                //var szKzk = "select JCKz1 from KJ_DeviceDefInfo where PointID='" + CurrentPointID + "'";
                //var Dt = GetDataTableBySQL(szKzk);
                var model = _jc_DefRepository.GetPointDefineByPointId(CurrentPointID);
                var Dt = ObjectConverter.ToDataTable(model);

                var szKzk = "";
                if (Dt.Rows.Count > 0)
                    szKzk = Dt.Rows[0]["JCKz1"].ToString();
                else
                {
                    QueryStr[4] = "无断电区域";
                    return QueryStr;
                }

                //var SzSql =
                //    "select KJ_DeviceAddress.wz from KJ_DeviceDefInfo left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=KJ_DeviceDefInfo.wzid where KJ_DeviceDefInfo.point in ('" +
                //    szKzk.Replace("|", "','") + "')";
                //Dt.Clear();
                //Dt = GetDataTableBySQL(SzSql);
                Dt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPoint", szKzk.Replace("|", "','"));

                QueryStr[4] = "";
                for (var i = 0; i < Dt.Rows.Count; i++)
                    QueryStr[4] += Dt.Rows[i]["wz"] + "|";
                if (QueryStr[4] != "")
                    QueryStr[4] = QueryStr[4].Substring(0, QueryStr[4].Length - 1);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetValue" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     查询断电区域（根据控制口列表）
        /// </summary>
        /// <param name="Szz"></param>
        /// <returns></returns>
        private string[] GetDdReplace1(string Szz)
        {
            var QueryStr = new string[13];
            for (var i = 0; i <= 12; i++)
                QueryStr[i] = "";
            try
            {
                var Dt = new DataTable();
                if (Szz == "")
                    return QueryStr;
                //var SzSql =
                //    "select KJ_DeviceAddress.wz from KJ_DeviceDefInfo left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=KJ_DeviceDefInfo.wzid where KJ_DeviceDefInfo.point in ('" +
                //    Szz.Replace("|", "','") + "')";
                //Dt.Clear();
                //Dt = GetDataTableBySQL(SzSql);
                Dt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPoint", Szz.Replace("|", "','"));

                QueryStr[4] = "";
                for (var i = 0; i < Dt.Rows.Count; i++)
                    if (!QueryStr[4].Contains(Dt.Rows[i]["wz"].ToString()))
                        QueryStr[4] += Dt.Rows[i]["wz"] + "|";
                if (QueryStr[4] != "")
                    QueryStr[4] = QueryStr[4].Substring(0, QueryStr[4].Length - 1);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetDdReplace1" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            var newdt = new DataTable();
            try
            {
                newdt = dt.Clone();
                var dr = dt.Select(condition);
                for (var i = 0; i < dr.Length; i++)
                    newdt.ImportRow(dr[i]);
                return newdt; //返回的查询结果
            }
            catch (Exception ex)
            {
                return newdt;
            }
        }

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
        public BasicResponse<DataTable> GetMonthLine(GetMonthLineRequest request)
        {
            string tablename, tablename1, SzTableName, pointdate, strSql = "", tempSQL = "";
            var dtqxData = new DataTable();
            var dtR = new DataTable();
            dtR.TableName = "getMonthLine";
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("HourMax");
            dtR.Columns.Add("HourAvg");
            dtR.Columns.Add("HourMin");
            dtR.Columns.Add("Timer");
            request.SzNameS = Convert.ToDateTime(request.SzNameS.ToShortDateString());
            request.SzNameE = Convert.ToDateTime(request.SzNameE.ToShortDateString() + " 23:59:59");
            try
            {
                for (var SzNameT = request.SzNameS; SzNameT <= request.SzNameE; SzNameT = SzNameT.AddMonths(1))
                {
                    tablename = "KJ_Hour" + SzNameT.Year + SzNameT.Month.ToString("00");
                    SzTableName = tablename;
                    //检查表是否存在
                    var tempdtIsExit = new DataTable();
                    if (GetDBType().Data.ToLower() == "sqlserver")
                        //strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablename +
                        //         "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                        tempdtIsExit = _jc_DefRepository.QueryTable("global_ChartService_GetSqlServerTableName_ByTableName", tablename);
                    else if (GetDBType().Data.ToLower() == "mysql")
                        //strSql = "SHOW TABLES LIKE  '" + tablename + "';";
                        tempdtIsExit = _jc_DefRepository.QueryTable("global_ChartService_GetMySqlTableName_ByTableName", tablename);
                    //var tempdtIsExit = GetDataTableBySQL(strSql);
                    if (tempdtIsExit.Rows.Count < 1)
                        continue;

                    var pointday = SzNameT.Day;
                    if (SzNameT == request.SzNameS)
                        tempSQL = "SELECT zdz as HourMax,pjz as HourAvg,zxz as HourMin,DATE_FORMAT(timer,'%Y-%m-%d %H:00:00') as timer FROM " +
                                  tablename
                                  + " "
                                  + " WHERE PointID = " + request.CurrentPointID + " and timer>='" + request.SzNameS + "' and timer<='" +
                                  request.SzNameE + "'";
                    else
                        tempSQL +=
                            " union all SELECT zdz as HourMax,pjz as HourAvg,zxz as HourMin,DATE_FORMAT(timer,'%Y-%m-%d %H:00:00') as timer FROM " +
                            tablename
                            + " "
                            + " WHERE PointID = " + request.CurrentPointID + " and timer>='" + request.SzNameS + "' and timer<='" +
                            request.SzNameE + "'";
                }
                strSql = "select * from (" + tempSQL + ") as temptab where timer is not null order by timer";

                //var tempdt = GetDataTableBySQL(strSql);
                var tempdt = _listexRepositoryBase.QueryTableBySql(strSql);

                if (tempdt.Rows.Count > 0)
                    foreach (DataRow dr in tempdt.Rows)
                        dtR.Rows.Add(dr.ItemArray);
                //补数据 
                DataRow[] tempdr = null;
                for (var SzNameT = request.SzNameS; SzNameT <= request.SzNameE; SzNameT = SzNameT.AddHours(1))
                {
                    tempdr = dtR.Select("timer='" + SzNameT.ToString("yyyy-MM-dd HH:mm") + ":00'");
                    if (tempdr.Length < 1)
                    {
                        //未找到当天的记录，补充数据
                        var obj = new object[dtR.Columns.Count];
                        obj[0] = "0.00001";
                        obj[1] = "0.00001";
                        obj[2] = "0.00001";
                        obj[3] = SzNameT.ToString("yyyy-MM-dd HH:mm") + ":00";
                        dtR.Rows.Add(obj);
                    }
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getMonthLine" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<DataTable>()
            {
                Data = dtR
            };
            return ret;
        }

        /// <summary>
        ///     模拟量5分钟曲线数据
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">位置ID</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetFiveMiniteLine(GetFiveMiniteLineRequest request)
        {
            string tablename, SzTableName, pointdate, strSql = "", tempSQL;
            var dtqxData = new DataTable();
            var dtR = new DataTable();
            dtR.TableName = "getFiveMiniteLine";
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("Av");
            dtR.Columns.Add("Bv");
            dtR.Columns.Add("Cv");
            dtR.Columns.Add("Dv");
            dtR.Columns.Add("Ev");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");
            dtR.Columns.Add("Timer");
            dtR.Columns.Add("state");
            try
            {
                for (var SzNameT = request.SzNameS; SzNameT <= request.SzNameE; SzNameT = SzNameT.AddDays(1))
                {
                    tablename = "KJ_StaFiveMinute" + SzNameT.Year + SzNameT.Month.ToString("00") + SzNameT.Day.ToString("00");
                    SzTableName = tablename;
                    //检查表是否存在
                    if (GetDBType().Data.ToLower() == "sqlserver")
                        strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablename +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                    else if (GetDBType().Data == "mysql")
                        strSql = "SHOW TABLES LIKE  '" + tablename + "';";
                    //var tempdtIsExit = GetDataTableBySQL(strSql);
                    var tempdtIsExit = _listexRepositoryBase.QueryTableBySql(strSql);

                    if (tempdtIsExit.Rows.Count < 1)
                        continue;

                    var pointday = SzNameT.Day;

                    string PointConditions = "";

                    if (request.IsQueryByPoint)//增加只按测点号查询处理  20180227
                    {
                        string PointNow = "";
                        var dt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPointId", request.CurrentPointID);
                        if (dt.Rows.Count > 0)
                            PointNow = dt.Rows[0]["point"].ToString();
                       

                        PointConditions = " Point = '" + PointNow + "'";
                    }
                    else
                    {
                        PointConditions = " PointID = " + request.CurrentPointID;
                    }

                    tempSQL = "SELECT " + tablename
                                  + ".id,zdz, pjz, ssz, timer,zxz,type,state,BFT_EnumCode.strEnumDisplay as typetext FROM " +
                                  tablename
                                  + " left outer join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + tablename +
                                  ".type and BFT_EnumCode.EnumTypeID='4'"
                                  + " WHERE " + PointConditions;


                    strSql = "select * from (" + tempSQL + ") as temptab order by timer,id";

                    //dtqxData = GetDataTableBySQL(strSql);
                    dtqxData = _listexRepositoryBase.QueryTableBySql(strSql);
                    var tempdt = CreatQXTDataFiveMin(DateTime.Parse(SzNameT.ToShortDateString()), dtqxData, request.CurrentDevid);
                    foreach (DataRow dr in tempdt.Rows)
                        dtR.Rows.Add(dr.ItemArray);
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getFiveMiniteLine" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<DataTable>()
            {
                Data = dtR
            };
            return ret;
        }

        /// <summary>
        ///     5分钟曲线获取 某一时刻的最大值、最小值、平均值
        /// </summary>
        /// <param name="QxDate">时间</param>
        /// <param name="DtStart">未用</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <returns></returns>
        public BasicResponse<string[]> GetDataVale(GetDataValeRequest request)
        {
            var QueryStr = new string[14];
            for (var i = 0; i <= 13; i++)
                QueryStr[i] = "";
            var Lc1 = 0;
            var Lc2 = 0;
            var tempList = new List<int>();


            try
            {
                var SzTableName = "";
                SzTableName = "KJ_StaFiveMinute" + request.QxDate.ToString("yyyyMMdd");

                var Szsql = "";

                Szsql = "select " + SzTableName
                        + ".id,zdz,ssz,pjz,zxz,BFT_EnumCode.strEnumDisplay as statetext,timer from " + SzTableName
                        + " left outer join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + SzTableName +
                        ".state and BFT_EnumCode.EnumTypeID='4'"
                        + " where timer='" + request.QxDate + "'  and PointID=" + request.CurrentPointID + " order by timer,id";

                var Dt = new DataTable();
                //Dt = GetDataTableBySQL(Szsql);
                Dt = _listexRepositoryBase.QueryTableBySql(Szsql);

                #region 获取当前设备的量程

                tempList = GetLcFromTable(request.CurrentDevid);
                if (tempList.Count > 0)
                {
                    Lc1 = tempList[0];
                    Lc2 = tempList[1];
                }

                #endregion

                ConvertTimeToDt(ref Dt);

                float Maxf = 0.0f, AvgF = 0.0f;
                var iCOunt = 0;
                for (var i = 0; i < Dt.Rows.Count; i++)
                {
                    //增加设备状态显示  20150906     
                    if (!QueryStr[13].Contains(Dt.Rows[i]["statetext"].ToString()))
                        if (QueryStr[13] != "")
                            QueryStr[13] += "|" + Dt.Rows[i]["statetext"];
                        else
                            QueryStr[13] = Dt.Rows[i]["statetext"].ToString();

                    if (Dt.Rows[i]["zdz"].ToString() != "")
                        if (Maxf <= Convert.ToSingle(Dt.Rows[i]["zdz"]))
                        {
                            Maxf = Convert.ToSingle(Dt.Rows[i]["zdz"]);

                            //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                            //    if (Convert.ToSingle(Dt.Rows[i]["zdz"]) > 9.995)
                            //        QueryStr[7] = string.Format("{0:F1}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                            //    else
                            //        QueryStr[7] = string.Format("{0:F2}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                            //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                            //    if (Convert.ToSingle(Dt.Rows[i]["zdz"]) > 99.995)
                            //        QueryStr[7] = string.Format("{0:F0}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                            //    else
                            //        QueryStr[7] = string.Format("{0:F1}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                            //else
                            //    QueryStr[7] = string.Format("{0:F0}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                            //不进行数据处理  20180109
                            QueryStr[7] = string.Format("{0:F2}", Convert.ToSingle(Dt.Rows[i]["zdz"]));
                        }

                    if (Dt.Rows[i]["pjz"].ToString() != "")
                        if (Convert.ToSingle(Dt.Rows[i]["pjz"]) >= 0)
                        {
                            AvgF += Convert.ToSingle(Dt.Rows[i]["pjz"]);
                            iCOunt++;
                        }
                }

                #region 平均值

                if ((iCOunt == 0) && (Dt.Rows.Count <= 0))
                    QueryStr[8] = "未记录";
                else if (iCOunt != 0)
                {
                    double FData = AvgF / iCOunt;
                    //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                    //    if (FData > 9.995)
                    //        QueryStr[8] = string.Format("{0:F1}", FData);
                    //    else
                    //        QueryStr[8] = string.Format("{0:F2}", FData);
                    //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                    //    if (FData > 99.995)
                    //        QueryStr[8] = string.Format("{0:F0}", FData);
                    //    else
                    //        QueryStr[8] = string.Format("{0:F1}", FData);
                    //else
                    //    QueryStr[8] = string.Format("{0:F0}", FData);
                    //不进行数据处理  20180109
                    QueryStr[8] = string.Format("{0:F2}", FData);
                }
                else
                    QueryStr[8] = "未记录";

                #endregion

                #region 实时值

                if (Dt.Rows.Count > 0)
                    if (Dt.Rows[Dt.Rows.Count - 1]["ssz"].ToString() != "")
                        if (Convert.ToSingle(Dt.Rows[Dt.Rows.Count - 1]["ssz"]) >= 0)
                        {
                            double FData = Convert.ToSingle(Dt.Rows[Dt.Rows.Count - 1]["ssz"]);
                            //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                            //    if (FData > 9.995)
                            //        QueryStr[6] = string.Format("{0:F1}", FData);
                            //    else
                            //        QueryStr[6] = string.Format("{0:F2}", FData);
                            //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                            //    if (FData > 99.995)
                            //        QueryStr[6] = string.Format("{0:F0}", FData);
                            //    else
                            //        QueryStr[6] = string.Format("{0:F1}", FData);
                            //else
                            //    QueryStr[6] = string.Format("{0:F0}", FData);
                            //不进行数据处理  20180109
                            QueryStr[6] = string.Format("{0:F2}", FData);
                        }
                        else
                        {
                            QueryStr[6] = "未记录";
                        }
                    else
                        QueryStr[6] = "未记录";
                else
                    QueryStr[6] = "未记录";

                #endregion

                #region 最大值，最小值

                if (QueryStr[7] == "")
                    QueryStr[7] = "未记录";
                if (QueryStr[8] == "")
                    QueryStr[8] = "未记录";

                #endregion
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetDataVale" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<string[]>()
            {
                Data = QueryStr
            };
            return ret;
        }

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
        public BasicResponse<string[]> GetValue(GetValueRequest request)
        {
            var NullEtime = DateTime.Parse("1900-01-01 00:00:00");
            var QueryStr = new string[14];
            for (var i = 0; i <= 13; i++)
                QueryStr[i] = "";
            try
            {
                #region

                var TableName = "KJ_DataAlarm" + request.QxDate.ToString("yyyyMM");
                var tableNameJCR = "KJ_DataRunRecord" + request.QxDate.ToString("yyyyMM");
                var SzSql = "";
                var SzSql1 = "";

                SzSql = "select  type from " + TableName + " where ((stime<'" + request.QxDate.AddMinutes(5.0f) +
                        "' and etime>='" + request.QxDate
                        + "') or (stime<='" + request.QxDate.AddMinutes(5.0f) + "' and etime ='" + NullEtime
                        + "')) and PointID='" + request.CurrentPointID +
                        "' and (type=10 or type=12  or type=16 or type=18)  order by stime";

                var Fdd = 0.0f;
                var fFd = 0.0f;

                float.TryParse(QueryStr[2], out Fdd);
                float.TryParse(QueryStr[3], out fFd);

                var Dt = new DataTable();
                var Dt1 = new DataTable();
                //Dt = GetDataTableBySQL(SzSql);
                Dt = _listexRepositoryBase.QueryTableBySql(SzSql);
                QueryStr[9] = "解除";
                QueryStr[10] = "复电";

                for (var i = 0; i < Dt.Rows.Count; i++)
                {
                    if (Dt.Rows[i]["type"].ToString() == "10") //上限报警
                        QueryStr[9] = "报警";
                    if (Dt.Rows[i]["type"].ToString() == "12") //上限断电
                        QueryStr[10] = "断电";
                    if (Dt.Rows[i]["type"].ToString() == "16") //下限报警
                        QueryStr[9] = "下限报警";
                    if (Dt.Rows[i]["type"].ToString() == "18") //下限断电
                        QueryStr[10] = "下限断电";
                }

                #endregion

                QueryStr[4] = "";
                QueryStr[11] = "";
                QueryStr[12] = "";
                var AddFlag = false;
                var LStTest = new List<string>();
                var LstPoint = new List<string>();
                string[] Szz = null;
                //TableName = "KJ_DataAlarm" + QxDate.Year + QxDate.Month.ToString().PadLeft(2, '0') + QxDate.Day.ToString().PadLeft(2, '0');

                SzSql = "select KJ_DeviceAddress.Wz ,BFT_EnumCode.strEnumDisplay as statetext," + TableName + ".type," + TableName +
                        ".ID," + TableName + ".PointID,"
                        + TableName + ".kdid," + TableName + ".kzk," + TableName + ".point," + TableName + ".stime," +
                        TableName + ".etime," + TableName + ".cs From " + TableName
                        + " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + TableName + ".wzid  "
                        + "left outer join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + TableName +
                        ".state and BFT_EnumCode.EnumTypeID='4'"
                        + "where   ( point like '%C%' and (stime<'"
                        + request.QxDate.ToShortDateString() + " 23:59:59" + "' and etime>='" + request.QxDate.ToShortDateString() + "' or stime<='" + request.QxDate.ToShortDateString() + " 23:59:59" +
                        "' and etime ='" + NullEtime
                        + "') or ( (PointID='" + request.CurrentPointID +
                        "' and (stime<'"
                        + request.QxDate.AddMinutes(5.0f) + "' and etime>='" + request.QxDate + "' or stime<='" + request.QxDate +
                        "' and etime ='" + NullEtime
                        + "') and (type=10 or type=12 or type=16 or type=18 or type=20)   ))) order by stime";

                SzSql1 = "select KJ_DeviceAddress.Wz ,BFT_EnumCode.strEnumDisplay as statetext," + tableNameJCR + ".type," + tableNameJCR +
                      ".ID," + tableNameJCR + ".PointID," + tableNameJCR + ".point," + tableNameJCR + ".timer From " + tableNameJCR
                      + " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + tableNameJCR + ".wzid  "
                      + "left outer join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + tableNameJCR +
                      ".state and BFT_EnumCode.EnumTypeID='4'"
                      + "where   point like '%C%' and (timer>='" + request.QxDate + "' and timer<='" + request.QxDate.ToShortDateString() + " 23:59:59" +
                      "')  order by timer";


                //Dt = GetDataTableBySQL(SzSql);
                Dt = _listexRepositoryBase.QueryTableBySql(SzSql);
                Dt1 = _listexRepositoryBase.QueryTableBySql(SzSql1);
                if ((Dt.Rows.Count > 0) && (request.QxDate.AddMinutes(5) <= GetCenterDateTime()))
                {
                    var DrV =
                        Dt.Select(
                            "PointID='" + request.CurrentPointID + "' and (type=10 or type=12 or type=16 or type=18 or type=20)",
                            "stime Asc");
                    for (var i = 0; i < DrV.Length; i++)
                    {
                        //将主控的措施加到列表进行显示   20150831
                        if (!QueryStr[12].Contains(DrV[i]["cs"].ToString()))
                            if (QueryStr[12] != "")
                                QueryStr[12] += "|" + DrV[i]["cs"];
                            else
                                QueryStr[12] = DrV[i]["cs"].ToString();
                        //增加设备状态显示  20150906     
                        if (!QueryStr[13].Contains(DrV[i]["statetext"].ToString()))
                            if (QueryStr[13] != "")
                                QueryStr[13] += "|" + DrV[i]["statetext"];
                            else
                                QueryStr[13] = DrV[i]["statetext"].ToString();
                        if (DrV[i]["kzk"].ToString() != "") //取断电区域
                        {
                            Szz = DrV[i]["kzk"].ToString().Split('|');
                            var kzk = "";
                            for (var ip = 0; ip < Szz.Length; ip++)
                                if (!string.IsNullOrEmpty(Szz[ip]))
                                    if (!LstPoint.Contains(Szz[ip]))
                                        kzk = kzk + "'" + Szz[ip] + "',";
                            if (kzk.Contains(","))
                                kzk = kzk.Substring(0, kzk.Length - 1);
                            var Dr = Dt1.Select("point in(" + kzk + ")", "point,timer Asc");
                            for (var j = 0; j < Dr.Length; j++)
                            {
                                var Sz = Dr[j]["wz"].ToString();
                                if (Sz == "")
                                    Sz = GetAddr(Dr[j]["point"].ToString());
                                if (!QueryStr[4].Contains(Sz))
                                    if ((QueryStr[4] == null) || (QueryStr[4] == ""))
                                        QueryStr[4] = Sz;
                                    else
                                        QueryStr[4] += "|" + Sz;
                            }
                        }
                        //查询馈电失败情况
                        if (DrV[i]["kdid"].ToString() != "")
                        {
                            Szz = DrV[i]["kdid"].ToString().Split(',');
                            var KdId = "";
                            for (var ip = 0; ip < Szz.Length; ip++)
                                if (!string.IsNullOrEmpty(Szz[ip]))
                                    if (!LstPoint.Contains(Szz[ip]))
                                    {
                                        LstPoint.Add(Szz[ip]);
                                        LStTest.Add(Szz[ip]);
                                        KdId = KdId + Szz[ip] + ",";
                                    }
                            if (KdId.Contains(","))
                                KdId = KdId.Substring(0, KdId.Length - 1);
                            var Dr = Dt.Select("ID in(" + KdId + ")", "point,stime Asc");
                            for (var j = 0; j < Dr.Length; j++)
                            {
                                //if (LStTest.Contains(Dr[j]["point"].ToString()))
                                //    LStTest.Remove(Dr[j]["point"].ToString());

                                var Sz = Dr[j]["wz"].ToString();
                                //if (Sz == "")
                                //    Sz = InterfaceClass.QueryPubClass_.GetAddr(Dr[j]["point"].ToString());
                                //if (!QueryStr[4].Contains(Sz))
                                //{
                                //    if (QueryStr[4] == null || QueryStr[4] == "")
                                //        QueryStr[4] = Sz;
                                //    else
                                //        QueryStr[4] += "|" + Sz;
                                //}

                                if ((Dr[j]["type"].ToString() == "32") || (Dr[j]["type"].ToString() == "30") ||
                                    (Dr[j]["type"].ToString() == "20"))
                                {
                                    AddFlag = false;
                                    if ((QueryStr[11] == null) || (QueryStr[11] == ""))
                                    {
                                        if (Dr[j]["type"].ToString() == "32")
                                        {
                                            QueryStr[11] = Sz + "\\断电异常";
                                            AddFlag = true;
                                        }
                                        else if (Dr[j]["type"].ToString() == "30")
                                        {
                                            QueryStr[11] = Sz + "\\复电异常";
                                            AddFlag = true;
                                        }
                                        else if (Dr[j]["type"].ToString() == "20")
                                        {
                                            QueryStr[11] = Sz + "\\断线断电异常";
                                            AddFlag = true;
                                        }
                                        if (AddFlag)
                                            QueryStr[12] = Dr[j]["cs"].ToString();
                                    }
                                    else
                                    {
                                        if (Dr[j]["type"].ToString() == "32")
                                        {
                                            if (!QueryStr[11].Contains(Sz + "\\断电异常"))
                                            {
                                                QueryStr[11] += "|" + Sz + "\\断电异常";
                                                AddFlag = true;
                                            }
                                        }
                                        else if (Dr[j]["type"].ToString() == "30")
                                        {
                                            if (!QueryStr[11].Contains(Sz + "\\复电异常"))
                                            {
                                                QueryStr[11] += "|" + Sz + "\\复电异常";
                                                AddFlag = true;
                                            }
                                        }
                                        else if (Dr[j]["type"].ToString() == "20")
                                        {
                                            if (!QueryStr[11].Contains(Sz + "\\断线断电异常"))
                                            {
                                                QueryStr[11] += Sz + "\\断线断电异常";
                                                AddFlag = true;
                                            }
                                        }
                                        if (AddFlag)
                                            if (!QueryStr[12].Contains(Dr[j]["cs"].ToString()))
                                                if (QueryStr[12] != "")
                                                    QueryStr[12] += "|" + Dr[j]["cs"];
                                                else
                                                    QueryStr[12] = Dr[j]["cs"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                if ((QueryStr[11] == null) || (QueryStr[11] == ""))
                    QueryStr[11] = "正常";
                if ((QueryStr[12] == null) || (QueryStr[12] == ""))
                    QueryStr[12] = "无";
                if ((QueryStr[4] == "") || (QueryStr[4] == null))
                {
                    var tempqueryStr = GetDdReplace(request.CurrentPointID);
                    if (!string.IsNullOrEmpty(tempqueryStr[4]))
                        QueryStr[4] = tempqueryStr[4];
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetValue" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<string[]>
            {
                Data = QueryStr
            };
            return ret;
        }

        /// <summary>
        ///     开关量状态变化 曲线、状态统计列表、柱状图的绑定数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        public BasicResponse<GetStateBarTableResponse> GetStateBarTable(GetStateBarTableRequest request)
        {
            var Rvalue = new GetStateBarTableResponse();
            var TjTxt = "";

            var SzName = "";

            //状态变化柱状图数据
            var dtBarStateChg = new DataTable();
            dtBarStateChg.TableName = "dtBarStateChg";
            dtBarStateChg.Columns.Add("state");
            dtBarStateChg.Columns.Add("stateName");
            dtBarStateChg.Columns.Add("stime");
            dtBarStateChg.Columns.Add("etime");
            //状态统计列表数据
            var dtTotal = new DataTable();
            dtTotal.TableName = "dtTotal";
            dtTotal.Columns.Add("Columns1");
            dtTotal.Columns.Add("Columns2");
            dtTotal.Columns.Add("Columns3");
            dtTotal.Columns.Add("Columns4");
            dtTotal.Columns.Add("Columns5");
            dtTotal.Columns.Add("Columns6");
            dtTotal.Columns.Add("Columns7");
            dtTotal.Columns.Add("Columns8");
            dtTotal.Columns.Add("Columns9");
            //小时开机率统计柱状图数据
            var dtBarHourTj = new DataTable();
            dtBarHourTj.TableName = "dtBarHourTj";
            dtBarHourTj.Columns.Add("percentage1Name");
            dtBarHourTj.Columns.Add("percentage1");
            dtBarHourTj.Columns.Add("percentage2Name");
            dtBarHourTj.Columns.Add("percentage2");
            dtBarHourTj.Columns.Add("timer");

            var sz = string.Empty;
            DataView Dview = null;
            int _1Type = 26, _2Type = 27;
            string Xs1 = "", Xs2 = "";
            var centerTime = new DateTime();
            try
            {
                var Stime = Convert.ToDateTime(request.SzNameT.ToShortDateString());
                var Etime = Convert.ToDateTime(request.SzNameT.ToShortDateString() + " 23:59:59");

                SzName = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");


                sz = "select * from KJ_DeviceType where devid='" + request.CurrentDevid + "'";
                //var tempdt = GetDataTableBySQL(sz);
                var tempdt = _listexRepositoryBase.QueryTableBySql(sz);
                if (tempdt.Rows.Count > 0)
                    if (tempdt.Rows[0]["xs2"].ToString() == tempdt.Rows[0]["xs3"].ToString())
                    {
                        Xs1 = tempdt.Rows[0]["xs1"].ToString();
                        Xs2 = tempdt.Rows[0]["xs2"].ToString();
                        _1Type = 25;
                        _2Type = 27;
                    }
                    else if (tempdt.Rows[0]["xs1"].ToString() == tempdt.Rows[0]["xs2"].ToString())
                    {
                        Xs1 = tempdt.Rows[0]["xs1"].ToString();
                        Xs2 = tempdt.Rows[0]["xs3"].ToString();
                        _1Type = 26;
                        _2Type = 27;
                    }
                    else if (tempdt.Rows[0]["xs3"].ToString() == "")
                    {
                        Xs1 = tempdt.Rows[0]["xs1"].ToString();
                        Xs2 = tempdt.Rows[0]["xs2"].ToString();
                        _1Type = 25;
                        _2Type = 26;
                    }
                    else
                    {
                        Xs1 = tempdt.Rows[0]["xs2"].ToString();
                        Xs2 = tempdt.Rows[0]["xs3"].ToString();
                        _1Type = 26;
                        _2Type = 27;
                    }

                //获取设备的分站号  2016-06-14               
                var TempFzh = GetPointFzh(request.CurrentPointID);

                if (request.kglztjsfs) //增加配置处理   20140428
                    sz = "select fzh,kh,type,val,timer from " + SzName + " as A where ((PointID='" + request.CurrentPointID +
                         "')or (fzh=0 and kh=0) or(type=0 and fzh="
                         + TempFzh + ")) and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                else
                    sz = "select fzh,kh,type,val,timer from " + SzName + " as A where (PointID='" + request.CurrentPointID +
                         "') and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                var Dt = new DataTable();
                //Dt = GetDataTableBySQL(sz);
                Dt = _listexRepositoryBase.QueryTableBySql(sz);

                ConvertDt(ref Dt);
                Dview = Dt.DefaultView;
                Dview.Sort = " timer ";

                if (Dt.Rows.Count > 0)
                {
                    var _StateVal = new int[86400];
                    for (var i = 0; i < 86400; i++)
                        _StateVal[i] = 3;
                    //_1Type  ZB[0]TYPE标记
                    //_2Type  ZB[1]TYPE标记
                    //T=0 代表   开（ZB[0]）
                    //T=1 代表   停或关（ZB[1]）
                    //T=2 代表   变化
                    //T=3 代表   未知 
                    //A[0]=T
                    //B[0]
                    //C[0]=67,33;(ZB[0](0),ZB[1](1))开机率
                    var iFlag = 20;
                    var DtVal = new DateTime();
                    int Sv = 0, Ev = 0;
                    object[] obj = null;
                    ConvertTimeToDt(ref Dt);

                    #region 计算状态变动情况

                    for (var i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"]);
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"]);
                            obj = new object[dtBarStateChg.Columns.Count];
                            if (iFlag != 10)
                                obj[1] = Dt.Rows[i]["val"].ToString();
                            else
                                obj[1] = "未知";


                            if (iFlag == _1Type)
                                for (; Sv <= Ev; Sv++) //6|8
                                    _StateVal[Sv] = 0;
                            else if (iFlag == _2Type) //||(_1Type==6&&(_2Type==7||_2Type==8)))
                                for (; Sv <= Ev; Sv++)
                                    _StateVal[Sv] = 1;
                            else
                                for (; Sv <= Ev; Sv++)
                                    _StateVal[Sv] = 3;
                        }
                        else if (i == Dt.Rows.Count - 1)
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i - 1]["type"]);
                            Sv = DtVal.Hour * 3600 + DtVal.Minute * 60 + DtVal.Second;
                            Ev = Convert.ToDateTime(Dt.Rows[i]["timer"]).Hour * 60 * 60 +
                                 Convert.ToDateTime(Dt.Rows[i]["timer"]).Minute * 60 +
                                 Convert.ToDateTime(Dt.Rows[i]["timer"]).Second;
                            obj[0] = iFlag.ToString();
                            obj[2] = DtVal.ToString("yyyy-MM-dd HH:mm:ss");
                            obj[3] = DateTime.Parse(Dt.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            dtBarStateChg.Rows.Add(obj);

                            if (iFlag == _1Type)
                                for (; Sv < Ev; Sv++) //6|8
                                    _StateVal[Sv] = 0;
                            else if (iFlag == _2Type) //||(_1Type==6&&(_2Type==7||_2Type==8)))
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 1;
                            else
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"]);
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"]);
                            obj = new object[dtBarStateChg.Columns.Count];
                            if (iFlag != 10)
                                obj[1] = Dt.Rows[i]["val"].ToString();
                            else
                                obj[1] = "未知";

                            centerTime = GetCenterDateTime();
                            if (DtVal.ToShortDateString() != centerTime.ToShortDateString())
                                Ev = 86400;
                            else
                                Ev = centerTime.Hour * 60 * 60 +
                                     centerTime.Minute * 60 +
                                     centerTime.Second;

                            Sv = DtVal.Hour * 60 * 60 + DtVal.Minute * 60 + DtVal.Second;

                            obj[0] = iFlag.ToString();
                            obj[2] = DtVal.ToString("yyyy-MM-dd HH:mm:ss");
                            var hourminsec = "";
                            if (Ev / 3600 == 24)
                                hourminsec = "23:59:59";
                            else
                                hourminsec = (Ev / 3600).ToString("00") + ":" + (Ev % 3600 / 60).ToString("00") + ":" +
                                             (Ev % 3600 % 60).ToString("00");
                            obj[3] = request.SzNameT.ToString("yyyy-MM-dd") + " " + hourminsec;
                            dtBarStateChg.Rows.Add(obj);


                            if (iFlag == _1Type)
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 0;
                            else if (iFlag == _2Type) //|| (_1Type == 6 && (_2Type == 7 || _2Type == 8)))
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 1;
                            else
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;

                            if (Ev != 86400)
                            {
                                Sv = Ev;
                                Ev = 86400;
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;
                            }
                        }
                        else
                        {
                            Sv = DtVal.Hour * 60 * 60 + DtVal.Minute * 60 + DtVal.Second;
                            Ev = Convert.ToDateTime(Dt.Rows[i]["timer"]).Hour * 60 * 60 +
                                 Convert.ToDateTime(Dt.Rows[i]["timer"]).Minute * 60 +
                                 Convert.ToDateTime(Dt.Rows[i]["timer"]).Second;

                            obj[0] = iFlag.ToString();
                            obj[2] = DtVal.ToString("yyyy-MM-dd HH:mm:ss");
                            obj[3] = DateTime.Parse(Dt.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            dtBarStateChg.Rows.Add(obj);

                            if (iFlag == _1Type)
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 0;
                            else if (iFlag == _2Type) // || (_1Type == 6 && (_2Type == 7 || _2Type == 8)))
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 1;
                            else
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"]);
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"]);
                            obj = new object[dtBarStateChg.Columns.Count];
                            if (iFlag != 10)
                                obj[1] = Dt.Rows[i]["val"].ToString();
                            else
                                obj[1] = "未知";
                        }

                        if (Dt.Rows.Count == 1)
                        {
                            centerTime = GetCenterDateTime();
                            if (DtVal.ToShortDateString() != centerTime.ToShortDateString())
                                Ev = 86400;
                            else
                                Ev = centerTime.Hour * 60 * 60 +
                                     centerTime.Minute * 60 +
                                     centerTime.Second;

                            Sv = DtVal.Hour * 60 * 60 + DtVal.Minute * 60 + DtVal.Second;

                            obj[0] = iFlag.ToString();
                            obj[2] = DtVal.ToString("yyyy-MM-dd HH:mm:ss");
                            var hourminsec = "";
                            if (Ev / 3600 == 24)
                                hourminsec = "23:59:59";
                            else
                                hourminsec = (Ev / 3600).ToString("00") + ":" + (Ev % 3600 / 60).ToString("00") + ":" +
                                             (Ev % 3600 % 60).ToString("00");
                            obj[3] = request.SzNameT.ToString("yyyy-MM-dd") + " " + hourminsec;
                            dtBarStateChg.Rows.Add(obj);

                            if (iFlag == _1Type)
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 0;
                            else if (iFlag == _2Type) //|| (_1Type == 6 && (_2Type == 7 || _2Type == 8)))
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 1;
                            else
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;

                            if (Ev != 86400)
                            {
                                Sv = Ev;
                                Ev = 86400;
                                for (; Sv < Ev; Sv++)
                                    _StateVal[Sv] = 3;
                            }
                        }
                    }

                    #endregion

                    #region 计算小时统计数据及开关量开机率柱状图

                    ///_A1 0小时时间累计, _A2 0变动次数累计, _A3 1小时时间累计,
                    ///_A4 0变动次数累计, LJA1, LJA2, LJA3, LJA4;
                    int _A1 = 0,
                        _A2 = 0,
                        _A3 = 0,
                        _A4 = 0,
                        LJA1 = 0,
                        LJA2 = 0,
                        LJA3 = 0,
                        LJA4 = 0,
                        LJA11 = 0,
                        LJA22 = 0,
                        LJA33 = 0,
                        LJA44 = 0;
                    int _int1 = 0, _int2 = 0;
                    int Flag = 0, Iqj = 0;
                    //解析_StateVal 60(分)  3600(小时)

                    Flag = _StateVal[0];


                    //B.Append("B[0]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");
                    //B.Append("B[10]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");
                    //B.Append("B[20]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");
                    //B.Append("B[30]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");
                    //B.Append("B[40]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");
                    //B.Append("B[50]=\"" + Xs1 + "(" + Xs2 + ")" + "\";");


                    var obj2 = new object[dtTotal.Columns.Count];
                    obj2[0] = Xs2 + "(" + Xs1 + ")" + "时间";

                    var obj3 = new object[dtTotal.Columns.Count];
                    obj3[0] = Xs2 + "(" + Xs1 + ")" + "次数";
                    var indexHour = 1;

                    object[] obj4 = null;

                    for (var _In = 60; _In <= _StateVal.Length; _In = _In + 60)
                    {
                        _int1 = 0;
                        _int2 = 0;

                        for (var ia = _In - 60; ia < _In; ia++)
                        {
                            if (_StateVal[ia] == 0)
                            {
                                _int1++;
                                _A1++; //小时时间累计
                            }
                            else if (_StateVal[ia] == 1)
                            {
                                _int2++;
                                _A3++; //小时时间累计
                            }
                            if (Flag != _StateVal[ia])
                            {
                                if (_StateVal[ia] == 0)
                                    _A2++; //0态累计次数
                                else if (_StateVal[ia] == 1)
                                    _A4++; //1态累计次数
                                Flag = _StateVal[ia];
                            }
                        }

                        //if (_int1 != 0 && _int2 != 0)
                        //    A.Append("A[" + (_In / 60 - 1) + "]=\"2\";");
                        //else if (_int1 != 0 && _int2 == 0)
                        //    A.Append("A[" + (_In / 60 - 1) + "]=\"0\";");
                        //else if (_int1 == 0 && _int2 != 0)
                        //    A.Append("A[" + (_In / 60 - 1) + "]=\"1\";");
                        //else
                        //    A.Append("A[" + (_In / 60 - 1) + "]=\"3\";");

                        if (_In % 3600 == 0)
                        {
                            //开关量柱状
                            //B.Append("B[" + (_In / 3600 + Iqj) + "]=\"" + _A1 / 60 + ":" + _A1 % 60 + "(" + _A3 / 60 + ":" + _A3 % 60 + ")\";");
                            //B.Append("B[" + (_In / 3600 + 10 + Iqj) + "]=\"" + _A2 + "(" + _A4 + ")\";");
                            obj2[indexHour] = _A3 / 60 + ":" + _A3 % 60 + "(" + _A1 / 60 + ":" + _A1 % 60 + ")";
                            obj3[indexHour] = _A4 + "(" + _A2 + ")";
                            indexHour++;

                            LJA1 += _A1;
                            LJA2 += _A2;
                            LJA3 += _A3;
                            LJA4 += _A4;
                            //28800
                            if (_In % 28800 == 0) //区间统计
                            {
                                //B.Append("B[" + (_In / 3600 + 1 + Iqj) + "]=\"" + LJA1 / 3600 + ":" + LJA1 % 3600 / 60 + ":" + LJA1 % 3600 % 60 + "(" + LJA3 / 3600 + ":" + LJA3 % 3600 / 60 + ":" + LJA3 % 3600 % 60 + ")\";");
                                //B.Append("B[" + (_In / 3600 + 10 + 1 + Iqj) + "]=\"" + LJA2 + "(" + LJA4 + ")\";");

                                #region 加每个区间的列头

                                if (_In / 28800 == 1)
                                {
                                    var obj1 = new object[dtTotal.Columns.Count];
                                    obj1[0] = "";
                                    obj1[1] = "0点";
                                    obj1[2] = "1点";
                                    obj1[3] = "2点";
                                    obj1[4] = "3点";
                                    obj1[5] = "4点";
                                    obj1[6] = "5点";
                                    obj1[7] = "6点";
                                    obj1[8] = "7点";
                                    dtTotal.Rows.Add(obj1);
                                }
                                else if (_In / 28800 == 2)
                                {
                                    var obj1 = new object[dtTotal.Columns.Count];
                                    obj1[0] = "";
                                    obj1[1] = "8点";
                                    obj1[2] = "9点";
                                    obj1[3] = "10点";
                                    obj1[4] = "11点";
                                    obj1[5] = "12点";
                                    obj1[6] = "13点";
                                    obj1[7] = "14点";
                                    obj1[8] = "15点";
                                    dtTotal.Rows.Add(obj1);
                                }
                                else
                                {
                                    var obj1 = new object[dtTotal.Columns.Count];
                                    obj1[0] = "";
                                    obj1[1] = "16点";
                                    obj1[2] = "17点";
                                    obj1[3] = "18点";
                                    obj1[4] = "19点";
                                    obj1[5] = "20点";
                                    obj1[6] = "21点";
                                    obj1[7] = "22点";
                                    obj1[8] = "23点";
                                    dtTotal.Rows.Add(obj1);
                                }

                                #endregion

                                //加区间的数据值
                                dtTotal.Rows.Add(obj2);
                                dtTotal.Rows.Add(obj3);

                                //重新初始化，准备存储下行的数据
                                obj2 = new object[dtTotal.Columns.Count];
                                obj2[0] = Xs2 + "(" + Xs1 + ")" + "时间";
                                obj3 = new object[dtTotal.Columns.Count];
                                obj3[0] = Xs2 + "(" + Xs1 + ")" + "次数";
                                indexHour = 1;

                                if (Iqj == 0)
                                    Iqj = 12;
                                else
                                    Iqj = 24;
                                LJA11 += LJA1;
                                LJA22 += LJA2;
                                LJA33 += LJA3;
                                LJA44 += LJA4;

                                LJA1 = 0;
                                LJA2 = 0;
                                LJA3 = 0;
                                LJA4 = 0;
                            }

                            //小时开机率
                            //C.Append("C[" + (_In / 3600 - 1) + "]=\"" + string.Format("{0:0.00}", _A1 / 3600.00 * 100) +
                            //    "," + string.Format("{0:0.00}", _A3 / 3600.00 * 100) + "\";");
                            obj4 = new object[dtBarHourTj.Columns.Count];
                            obj4[0] = Xs2;
                            obj4[1] = string.Format("{0:0.00}", _A3 / 3600.00 * 100);
                            obj4[2] = Xs1;
                            obj4[3] = string.Format("{0:0.00}", _A1 / 3600.00 * 100);
                            obj4[4] = request.SzNameT.ToShortDateString() + " " + (_In / 3600 - 1).ToString("00") + ":00:00";
                            dtBarHourTj.Rows.Add(obj4);

                            _A1 = 0;
                            _A2 = 0;
                            _A3 = 0;
                            _A4 = 0;
                        }
                    }

                    #endregion

                    //C.Append("C[24]=\"" + string.Format("{0:0.00}", LJA11 / 86400.00 * 100) +
                    //    "," + string.Format("{0:0.00}", LJA33 / 86400.00 * 100) + "\";");
                    //obj4 = new object[dtBarHourTj.Columns.Count];
                    //obj4[0] = Xs1;
                    //obj4[1] = string.Format("{0:0.00}", LJA11 / 86400.00 * 100);
                    //obj4[2] = Xs2;
                    //obj4[3] = string.Format("{0:0.00}", LJA33 / 86400.00 * 100);
                    //obj4[4] = SzNameT.ToShortDateString() + " 24:00:00";
                    //dtBarHourTj.Rows.Add(obj4);

                    TjTxt = "全天[" + Xs1 + "时间:" + LJA11 / 3600 + ":" + LJA11 % 3600 / 60 + ":"
                            + LJA11 % 3600 % 60 + "[" + Xs2 + "时间:" + LJA33 / 3600 + ":" + LJA33 % 3600 / 60 + ":"
                            + LJA33 % 3600 % 60 + "][" + Xs1 + "次数:" + LJA22 +
                            "][" + Xs2 + "次数:" + LJA44 + "][" + Xs1 + "率:" +
                            string.Format("{0:0.00}", LJA11 / 86400.00 * 100) +
                            "%][" + Xs2 + "率:" + string.Format("{0:0.00}", LJA33 / 86400.00 * 100) + "%]";
                }
                else
                {
                    TjTxt = "全天[" + Xs1 + "时间:00:00:00][" + Xs2 + "时间:00:00:00][" + Xs1 + "次数:0][" + Xs2 + "次数:0][" +
                            Xs1 + "率:0%][" + Xs2 + "率:0%]";
                }
                Rvalue.dtBarStateChg = dtBarStateChg;
                Rvalue.dtTotal = dtTotal;
                Rvalue.dtBarHourTj = dtBarHourTj;
                Rvalue.TjTxt = TjTxt;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateChgQueryClass_getStateBarTable" + Ex.Message + Ex.StackTrace);
            }

            var ret = new BasicResponse<GetStateBarTableResponse>
            {
                Data = Rvalue
            };
            return ret;
        }

        /// <summary>
        ///     开关量状态变化 列表显示数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetStateChgdt(GetStateChgdtRequest request)
        {
            var SzName = "";
            var sz = "";
            var Dt = new DataTable();
            Dt.TableName = "getStateChgdt";
            Dt.Columns.Add("fzh");
            Dt.Columns.Add("kh");
            Dt.Columns.Add("type");
            Dt.Columns.Add("val");
            Dt.Columns.Add("timer");
            try
            {
                var Stime = Convert.ToDateTime(request.SzNameT.ToShortDateString());
                var Etime = Convert.ToDateTime(request.SzNameT.ToShortDateString() + " 23:59:59");

                SzName = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");

                //获取设备的分站号  2016-06-14               
                var TempFzh = GetPointFzh(request.CurrentPointID);

                if (request.kglztjsfs) //增加配置处理   20140428
                    sz = "select fzh,kh,type,val,timer from " + SzName + " as A where ((PointID='" + request.CurrentPointID +
                         "')or (fzh=0 and kh=0) or(type=0 and fzh="
                         + TempFzh + ")) and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                else
                    sz = "select fzh,kh,type,val,timer from " + SzName + " as A where (PointID='" +
                         request.CurrentPointID + "') and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                //Dt = GetDataTableBySQL(sz);
                DataTable tempDt = _listexRepositoryBase.QueryTableBySql(sz);
                ConvertTimeToDt(ref Dt);
                for (int i = 0; i < tempDt.Rows.Count; i++)//将日期毫秒显示出来  20170917
                {
                    object[] obj = new object[Dt.Columns.Count];
                    obj[0] = tempDt.Rows[i]["fzh"].ToString();
                    obj[1] = tempDt.Rows[i]["kh"].ToString();
                    obj[2] = tempDt.Rows[i]["type"].ToString();
                    obj[3] = tempDt.Rows[i]["val"].ToString();
                    obj[4] = Convert.ToDateTime(tempDt.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    Dt.Rows.Add(obj);
                }

            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateChgQueryClass_getStateChgdt" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = Dt
            };
            return ret;
        }

        /// <summary>
        ///     开关量曲线数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetStateLineDt(GetStateLineDtRequest request)
        {
            var dtR = new DataTable();
            dtR.TableName = "getStateLineDt";
            //          
            dtR.Columns.Add("C");       //0
            dtR.Columns.Add("D");       //1
            dtR.Columns.Add("E");       //2
            dtR.Columns.Add("sTimer");        //3
            dtR.Columns.Add("eTimer");        //4
            dtR.Columns.Add("stateName");       //5

            var Index = 0;
            var Startime = 0; //本次状态开始时间
            var Endtime = 0; //本次状态结束时间(分)
            var SzCs = ""; //措施
            var SzCs1 = ""; //当没有馈电是用报警的措施暂存变量
            var SzKd = ""; //馈电状态
            var DtS = new DateTime();
            string XS1 = "", XS2 = "", XS3 = "";
            DataView Dview = null;
            DataView Dview1 = null;
            var centerTime = new DateTime();
            var sz = "";
            var Stime = DateTime.Parse(request.SzNameT.ToShortDateString());
            var Etime = DateTime.Parse(request.SzNameT.ToShortDateString() + " 23:59:59");
            try
            {
                sz = "select KJ_DeviceDefInfo.Bz6,KJ_DeviceDefInfo.Bz7,KJ_DeviceDefInfo.Bz8 from KJ_DeviceDefInfo where KJ_DeviceDefInfo.pointid ='" + request.CurrentPointID + "'";
                //var tempdevdt = GetDataTableBySQL(sz);
                var tempdevdt = _listexRepositoryBase.QueryTableBySql(sz);
                if (tempdevdt.Rows.Count > 0)
                {
                    XS1 = tempdevdt.Rows[0]["Bz6"].ToString();
                    XS2 = tempdevdt.Rows[0]["Bz7"].ToString();
                    XS3 = tempdevdt.Rows[0]["Bz8"].ToString();
                }
                else
                {
                    XS1 = "无";
                    XS2 = "无";
                    XS3 = "无";
                }
                //获取设备的分站号  2016-06-14
                var TempFzh = GetPointFzh(request.CurrentPointID);

                var SzTable = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");
                if (request.kglztjsfs)
                    sz = "select fzh,kh,type,timer,val,point from " + SzTable + " where ((PointID='" + request.CurrentPointID +
                         "' and (type=25 or type=26 or type=27) )or (fzh=0 and kh=0) or(type=0 and fzh=" + TempFzh
                         + ")) and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                else
                    sz = "select fzh,kh,type,timer,val,point from " + SzTable + " where ((PointID='" + request.CurrentPointID
                         + "' and (type=25 or type=26 or type=27) )) and timer>='" + Stime + "' and timer<='" + Etime +
                         "' order by timer,id";
                string[] Szz = null;
                var Dtval = new DataTable();
                var Dt = new DataTable();
                var Dtone = new DataTable();
                //Dtval = GetDataTableBySQL(sz);
                Dtval = _listexRepositoryBase.QueryTableBySql(sz);      //KJ_DataRunRecord表数据

                ConvertTimeToDt(ref Dt);//过滤重复时间 

                ConvertDt(ref Dtval);
                Dview = Dtval.DefaultView;
                Dview.Sort = "fzh,kh,timer ";

                var ClType = 0;
                if (Dtval.Rows.Count <= 0)
                {
                    var ret = new BasicResponse<DataTable>
                    {
                        Data = dtR
                    };
                    return ret;
                }

                var SzPbTable = "KJ_DataAlarm" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");
                var dtMidVal = new DataTable();


                sz = "select KJ_DeviceAddress.wz,type,kzk,stime,etime,cs,point from " + SzPbTable +
                     " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + SzPbTable + ".wzid where (PointID='" + request.CurrentPointID +
                     "') or point like '%C%' ";


                //dtMidVal = GetDataTableBySQL(sz);
                dtMidVal = _listexRepositoryBase.QueryTableBySql(sz);       //KJ_DataAlarm表数据
                Dview1 = dtMidVal.DefaultView;
                Dview1.Sort = "point,stime ";
                DataRow[] DrMid = null;
                DataRow[] DrSet = null;
                var i4 = 0;
                var obj = new object[dtR.Columns.Count];
                for (var i = 0; i < Dtval.Rows.Count; i++)
                {
                    if (i == 0) //分析第一条记录
                    {
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        continue;
                    }
                    if (i == Dtval.Rows.Count - 1) //查询的是最后一条记录
                    {
                        #region 处理最后一次记录

                        Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                                  Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                        DrMid = dtMidVal.Select("point='" + Dtval.Rows[i - 1]["point"] + "' and stime<='" + DtS + "' and etime>'" + DtS +
                                "'", "stime");
                        if (dtMidVal.Rows.Count > 0)
                        {
                            if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                            {
                                SzKd = "正常";
                                SzCs = "无";
                            }
                            else
                            {
                                for (var ifor = 0; ifor < DrMid.Length; ifor++)
                                {
                                    SzCs1 = DrMid[0]["cs"].ToString();
                                    if (DrMid[0]["kzk"].ToString() == "")
                                        SzKd = "正常";
                                    else
                                    {
                                        Szz = DrMid[0]["kzk"].ToString().Split('|');
                                        for (var ij = 0; ij < Szz.Length; ij++)
                                        {
                                            DrSet =
                                                dtMidVal.Select(
                                                    " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"] +
                                                    "' and etime>'" + DrMid[0]["stime"] + "'", "stime");
                                            for (i4 = 0; i4 < DrSet.Length; i4++)
                                                if (DrSet[i4]["type"].ToString() == "32")
                                                {
                                                    SzKd = "断电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                                else if (DrSet[i4]["type"].ToString() == "30")
                                                {
                                                    SzKd = "复电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                        }
                                    }
                                }
                                if (SzKd == "")
                                {
                                    SzKd = "正常";
                                    SzCs = SzCs1;
                                }
                            }
                        }
                        else
                        {
                            SzCs = "无";
                            SzKd = "正常";
                        }

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = Convert.ToDateTime(Dtval.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 25)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 26)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }
                            else if (ClType == 27)
                            {
                                obj[0] = "2";
                                obj[5] = XS3;
                            }
                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);
                        Index++;
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        centerTime = GetCenterDateTime();
                        if (DtS.ToShortDateString() == centerTime.ToShortDateString()) //说明查询的是当天的曲线 
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59.000");
                        }
                        DrMid = dtMidVal.Select("point='" + Dtval.Rows[i - 1]["point"] + "' and stime<='" + DtS + "' and etime>'" + DtS + "'", "stime");
                        if (dtMidVal.Rows.Count > 0)
                        {
                            if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                            {
                                SzKd = "正常";
                                SzCs = "无";
                            }
                            else
                            {
                                for (var ifor = 0; ifor < DrMid.Length; ifor++)
                                {
                                    SzCs1 = DrMid[0]["cs"].ToString();
                                    if (DrMid[0]["kzk"].ToString() == "")
                                        SzKd = "正常";
                                    else
                                    {
                                        Szz = DrMid[0]["kzk"].ToString().Split('|');
                                        for (var ij = 0; ij < Szz.Length; ij++)
                                        {
                                            DrSet =
                                                dtMidVal.Select(
                                                    " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"]
                                                    + "' and etime>'" + DrMid[0]["stime"] + "'", "stime");

                                            for (i4 = 0; i4 < DrSet.Length; i4++)
                                                if (DrSet[i4]["type"].ToString() == "32")
                                                {
                                                    SzKd = "断电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                                else if (DrSet[i4]["type"].ToString() == "30")
                                                {
                                                    SzKd = "复电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                        }
                                    }
                                }
                                if (SzKd == "")
                                {
                                    SzKd = "正常";
                                    SzCs = SzCs1;
                                }
                            }
                        }
                        else
                        {
                            SzCs = "无";
                            SzKd = "正常";
                        }
                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 25)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 26)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }
                            else if (ClType == 27)
                            {
                                obj[0] = "2";
                                obj[5] = XS3;
                            }
                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 25)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 26)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }
                            else if (ClType == 27)
                            {
                                obj[0] = "2";
                                obj[5] = XS3;
                            }
                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        Index++;
                        break;

                        #endregion
                    }
                    Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                              Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                    //if (dtMidVal.Rows.Count > 0)
                    //{
                    //    DrMid = dtMidVal.Select("point='" + Dtval.Rows[i - 1]["point"] + "' and stime<='" + DtS + "' and etime>'" + DtS + "'");

                    //    if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                    //    {
                    //        SzKd = "正常";
                    //        SzCs = "无";
                    //    }
                    //    #region

                    //    else
                    //    {
                    //        SzCs1 = DrMid[0]["cs"].ToString();
                    //        if (DrMid[0]["kzk"].ToString() == "")
                    //            SzKd = "正常";
                    //        else
                    //        {
                    //            Szz = DrMid[0]["kzk"].ToString().Split('|');
                    //            for (var ij = 0; ij < Szz.Length; ij++)
                    //            {
                    //                DrSet =
                    //                    dtMidVal.Select(
                    //                        " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"] +
                    //                        "' and etime>'" + DrMid[0]["stime"] + "'", "stime");
                    //                for (i4 = 0; i4 < DrSet.Length; i4++)
                    //                    if (DrSet[i4]["type"].ToString() == "32")
                    //                    {
                    //                        SzKd = "断电异常";
                    //                        SzCs = DrSet[i4]["Cs"].ToString();
                    //                        break;
                    //                    }
                    //                    else if (DrSet[i4]["type"].ToString() == "30")
                    //                    {
                    //                        SzKd = "复电异常";
                    //                        SzCs = DrSet[i4]["Cs"].ToString();
                    //                        break;
                    //                    }
                    //            }
                    //        }
                    //        if (SzKd == "")
                    //        {
                    //            SzKd = "正常";
                    //            SzCs = SzCs1;
                    //        }
                    //    }

                    //    #endregion
                    //}
                    //else
                    //{
                    //    SzKd = "正常";
                    //    SzCs = "无";
                    //}

                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    obj[4] = Convert.ToDateTime(Dtval.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    if (ClType == 10)
                    {
                        obj[0] = "0.00001";
                        obj[1] = "未记录";
                        obj[2] = "未记录";
                        obj[5] = "未记录";
                    }
                    else
                    {
                        if (ClType == 25)
                        {
                            obj[0] = "0";
                            obj[5] = XS1;
                        }
                        else if (ClType == 26)
                        {
                            obj[0] = "1";
                            obj[5] = XS2;
                        }
                        else if (ClType == 27)
                        {
                            obj[0] = "2";
                            obj[5] = XS3;
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;
                    }
                    dtR.Rows.Add(obj);

                    Index++;
                    ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                    DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                    Startime = Convert.ToDateTime(Dtval.Rows[i]["timer"]).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[i]["timer"]).Minute;
                    SzKd = "";
                    SzCs = "";
                    Endtime = 0;
                    SzCs1 = "";
                }

                #region 只有一条记录
                if (Dtval.Rows.Count == 1)
                    if (Dtval.Rows[0]["type"].ToString() == "10")
                    {
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd") + " 00:00:00";
                        obj[4] = DtS.ToString("yyyy-MM-dd") + " 23:59:59";
                        obj[0] = "0.00001";
                        obj[1] = "未记录";
                        obj[2] = "未记录";
                        obj[5] = "未记录";
                        dtR.Rows.Add(obj);
                    }
                    else
                    {
                        DtS = Convert.ToDateTime(Dtval.Rows[0]["timer"].ToString());
                        Startime = Convert.ToDateTime(Dtval.Rows[0]["timer"]).Hour * 60 +
                                   Convert.ToDateTime(Dtval.Rows[0]["timer"]).Minute;

                        centerTime = GetCenterDateTime();
                        if (Convert.ToDateTime(Dtval.Rows[0]["timer"]).ToShortDateString() ==
                            centerTime.ToShortDateString())
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                        }

                        #region

                        if (dtMidVal.Rows.Count > 0)
                        {
                            DrMid = dtMidVal.Select("point='" + Dtval.Rows[0]["point"] + "' and stime<='" + DtS + "' and etime>'" + DtS + "'");

                            if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                            {
                                SzKd = "正常";
                                SzCs = "无";
                            }
                            #region

                            else
                            {
                                SzCs1 = DrMid[0]["cs"].ToString();
                                if (DrMid[0]["kzk"].ToString() == "")
                                    SzKd = "正常";
                                else
                                {
                                    DrSet =
                                        dtMidVal.Select(
                                            " point in('" + DrMid[0]["kzk"].ToString().Replace("|", "','") +
                                            "') and stime<='" + DrMid[0]["etime"] + "' and etime>'" + DrMid[0]["stime"] +
                                            "'", "stime");
                                    for (i4 = 0; i4 < DrSet.Length; i4++)
                                        if (DrSet[i4]["type"].ToString() == "32")
                                        {
                                            SzKd = "断电异常";
                                            SzCs = DrSet[i4]["Cs"].ToString();
                                            break;
                                        }
                                        else if (DrSet[i4]["type"].ToString() == "30")
                                        {
                                            SzKd = "复电异常";
                                            SzCs = DrSet[i4]["Cs"].ToString();
                                            break;
                                        }
                                }
                                if (SzKd == "")
                                {
                                    SzKd = "正常";
                                    SzCs = SzCs1;
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            SzKd = "正常";
                            SzCs = "无";
                        }

                        #endregion

                        #region

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 25)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 26)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }
                            else if (ClType == 27)
                            {
                                obj[0] = "2";
                                obj[5] = XS3;
                            }
                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 25)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 26)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }
                            else if (ClType == 27)
                            {
                                obj[0] = "2";
                                obj[5] = XS3;
                            }
                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        #endregion
                    }
                #endregion

            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            var ret2 = new BasicResponse<DataTable>
            {
                Data = dtR
            };
            return ret2;
        }

        /// <summary>
        ///     查询模拟量报警、断电记录
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="type">1:模拟量报警，2：模拟量断电</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMnlBjLineDt(GetMnlBjLineDtRequest request)
        {
            var dtR = new DataTable();
            dtR.TableName = "getMnlBjLineDt";
            //          
            dtR.Columns.Add("C"); //状态值
            dtR.Columns.Add("D"); //馈电状态
            dtR.Columns.Add("E"); //措施
            dtR.Columns.Add("sTimer", typeof(DateTime)); //开始时间
            dtR.Columns.Add("eTimer", typeof(DateTime)); //结果时间
            dtR.Columns.Add("stateName"); //状态描述

            var Index = 0;
            var Startime = 0; //本次状态开始时间
            var Endtime = 0; //本次状态结束时间(分)
            var SzCs = ""; //措施
            var SzCs1 = ""; //当没有馈电是用报警的措施暂存变量
            var SzKd = ""; //馈电状态
            var DtS = new DateTime();
            DataView Dview = null;
            DataView Dview1 = null;
            var centerTime = new DateTime();
            var sz = "";
            var stime = request.SzNameS.ToShortDateString();
            var etime = request.SzNameE.ToShortDateString() + " 23:59:59";
            try
            {
                //bug修正:20170317
                //string SzTable = "KJ_DataRunRecord" + SzNameS.Year.ToString() + SzNameS.Month.ToString("00");
                var SzTable = "";

                if (request.SzNameS.ToString("yyyy-MM") == request.SzNameE.ToString("yyyy-MM"))
                    SzTable = "KJ_DataRunRecord" + request.SzNameS.ToString("yyyyMM");
                else
                {
                    #region//跨月时检查表是否存在  20170628
                    //处理跨月的情况  20170629
                    string strSql = "";
                    string strSql1 = "";
                    string tablenameS = "KJ_DataRunRecord" + request.SzNameS.ToString("yyyyMM");
                    string tablenameE = "KJ_DataRunRecord" + request.SzNameE.ToString("yyyyMM");
                    if (GetDBType().Data.ToLower() == "sqlserver")
                    {
                        strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameS +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                        strSql1 = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameE +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                    }
                    else if (GetDBType().Data == "mysql")
                    {
                        strSql = "SHOW TABLES LIKE  '" + tablenameS + "';";
                        strSql1 = "SHOW TABLES LIKE  '" + tablenameE + "';";
                    }
                    var tempdtIsExit = _listexRepositoryBase.QueryTableBySql(strSql);
                    var tempdtIsExit1 = _listexRepositoryBase.QueryTableBySql(strSql1);
                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        strSql = "select * from " + tablenameS;
                        SzTable = strSql;
                    }
                    if (tempdtIsExit1.Rows.Count > 0)
                    {
                        strSql1 = "select * from " + tablenameE;
                        if (SzTable != "")
                        {
                            SzTable = SzTable + " union all " + strSql1;
                        }
                        else
                        {
                            SzTable = strSql1;
                        }
                    }
                    SzTable = "(" + SzTable + ") as temp ";
                    #endregion

                }

                if (request.type == "2")
                    sz = "select " + SzTable + ".fzh," + SzTable + ".kh," + SzTable + ".type," + SzTable + ".timer," + SzTable + ".val from "
                        + SzTable + " left join KJ_DeviceDefInfo on KJ_DeviceDefInfo.pointid=" + SzTable + ".pointid where (" + SzTable + ".PointID='" + request.CurrentPointID
                         + "' and (type=21 or (type=12 and (KJ_DeviceDefInfo.k2>0 or KJ_DeviceDefInfo.jckz1<>'')) or type=13 or (type=18 and (KJ_DeviceDefInfo.k4>0 or KJ_DeviceDefInfo.jckz1<>'')) or type=19 or(type=20 and (KJ_DeviceDefInfo.k7>0 or KJ_DeviceDefInfo.jckz2<>''))) and timer>='" + stime + "' and timer<='" + etime +
                         "') order by " + SzTable + ".timer," + SzTable + ".id";
                else //默认模拟量报警
                    sz = "select fzh,kh,type,timer,val from " + SzTable + " where (PointID='" + request.CurrentPointID
                         + "' and (type=21 or type=10 or type=11 or type=16 or type=17) and timer>='" + stime + "' and timer<='" + etime +
                         "') order by timer,id";

                //if (SzNameS.Month != SzNameE.Month)
                //{
                //    string SzTable1 = "KJ_DataRunRecord" + SzNameE.Year.ToString() + SzNameE.Month.ToString("00");
                //    if (type == "2")
                //    {
                //        sz += "union all select fzh,kh,type,timer,val from " + SzTable1 + " where (PointID='" + CurrentPointID
                //            + "' and (type=21 or type=12 or type=13) and timer>='" + stime + "' and timer<='" + etime + "') order by timer,id";
                //    }
                //    else //默认模拟量报警
                //    {
                //        sz += "union all select fzh,kh,type,timer,val from " + SzTable1 + " where (PointID='" + CurrentPointID
                //            + "' and (type=21 or type=10 or type=11) and timer>='" + stime + "' and timer<='" + etime + "') order by timer,id";
                //    }
                //}

                string[] Szz = null;
                var Dtval = new DataTable();
                var Dt = new DataTable();
                var Dtone = new DataTable();
                //Dtval = GetDataTableBySQL(sz);
                Dtval = _listexRepositoryBase.QueryTableBySql(sz);

                ConvertTimeToDt(ref Dt);//过滤重复时间 
                //ConvertTypeToDt(ref Dtval);
                Dview = Dtval.DefaultView;
                Dview.Sort = "fzh,kh,timer ";

                var ClType = 0;
                if (Dtval.Rows.Count <= 0)
                {
                    var ret2 = new BasicResponse<DataTable>()
                    {
                        Data = dtR
                    };
                    return ret2;
                }

                //string SzPbTable = "KJ_DataAlarm" + SzNameS.Year.ToString() + SzNameS.Month.ToString("00");
                var SzPbTable = "";
                if (request.SzNameS.ToString("yyyy-MM") == request.SzNameE.ToString("yyyy-MM"))
                {
                    SzPbTable = "KJ_DataAlarm" + request.SzNameS.ToString("yyyyMM");
                }
                else
                {
                    //SzPbTable = "(select * from KJ_DataAlarm" + request.SzNameS.ToString("yyyyMM") + " union all select * from KJ_DataAlarm" +
                    //            request.SzNameE.ToString("yyyyMM") + ")";
                    //处理跨月的情况  20170629
                    string strSql = "";
                    string strSql1 = "";
                    string tablenameS = "KJ_DataAlarm" + request.SzNameS.ToString("yyyyMM");
                    string tablenameE = "KJ_DataAlarm" + request.SzNameE.ToString("yyyyMM");
                    if (GetDBType().Data.ToLower() == "sqlserver")
                    {
                        strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameS +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                        strSql1 = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameE +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                    }
                    else if (GetDBType().Data == "mysql")
                    {
                        strSql = "SHOW TABLES LIKE  '" + tablenameS + "';";
                        strSql1 = "SHOW TABLES LIKE  '" + tablenameE + "';";
                    }
                    var tempdtIsExit = _listexRepositoryBase.QueryTableBySql(strSql);
                    var tempdtIsExit1 = _listexRepositoryBase.QueryTableBySql(strSql1);
                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        strSql = "select * from " + tablenameS;
                        SzPbTable = strSql;
                    }
                    if (tempdtIsExit1.Rows.Count > 0)
                    {
                        strSql1 = "select * from " + tablenameE;
                        if (SzPbTable != "")
                        {
                            SzPbTable = SzPbTable + " union all " + strSql1;
                        }
                        else
                        {
                            SzPbTable = strSql1;
                        }
                    }
                    SzPbTable = "(" + SzPbTable + ")  ";
                }

                var dtMidVal = new DataTable();

                //sz = "select KJ_DeviceAddress.wz,type,kzk,stime,etime,cs,point from " + SzPbTable +
                //    " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + SzPbTable + ".wzid where (PointID='" + CurrentPointID + "' and stime<='" + etime + "' and etime>='" + stime + "') or point like '%C%' ";
                sz = "select b.wz,type,kzk,stime,etime,cs,point from " + SzPbTable +
                     " as a left outer join KJ_DeviceAddress as b on a.wzid=b.wzid where (PointID='" + request.CurrentPointID +
                     "' and stime<='" + etime + "' and etime>='" + stime + "') or point like '%C%' ";

                //if (SzNameS.Month != SzNameE.Month)
                //{
                //    string SzTable1 = "KJ_DataAlarm" + SzNameE.Year.ToString() + SzNameE.Month.ToString("00");
                //    sz += "union all select KJ_DeviceAddress.wz,type,kzk,stime,etime,cs,point from " + SzPbTable +
                //    " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + SzPbTable + ".wzid where (PointID='" + CurrentPointID + "' and stime<='" + etime + "' and etime>='" + stime + "') or point like '%C%' ";
                //}

                //dtMidVal = GetDataTableBySQL(sz);
                dtMidVal = _listexRepositoryBase.QueryTableBySql(sz);
                Dview1 = dtMidVal.DefaultView;
                Dview1.Sort = "point,stime ";
                DataRow[] DrMid = null;
                DataRow[] DrSet = null;
                var i4 = 0;
                var obj = new object[dtR.Columns.Count];
                for (var i = 0; i < Dtval.Rows.Count; i++)
                {
                    if (i == 0) //分析第一条记录
                    {
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        continue;
                    }
                    if (i == Dtval.Rows.Count - 1) //查询的是最后一条记录
                    {
                        #region 处理最后一次记录

                        Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                                  Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                        DrMid = dtMidVal.Select("stime<='" + DtS + "' and etime>='" + DtS + "'", "stime");
                        if (dtMidVal.Rows.Count > 0)
                        {
                            if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                            {
                                SzKd = "正常";
                                SzCs = "无";
                            }
                            else
                            {
                                for (var ifor = 0; ifor < DrMid.Length; ifor++)
                                {
                                    SzCs1 = DrMid[0]["cs"].ToString();
                                    if (DrMid[0]["kzk"].ToString() == "")
                                        SzKd = "正常";
                                    else
                                    {
                                        Szz = DrMid[0]["kzk"].ToString().Split('|');
                                        for (var ij = 0; ij < Szz.Length; ij++)
                                        {
                                            DrSet =
                                                dtMidVal.Select(
                                                    " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"] +
                                                    "' and etime>='" + DrMid[0]["stime"] + "'", "stime");
                                            for (i4 = 0; i4 < DrSet.Length; i4++)
                                                if (DrSet[i4]["type"].ToString() == "32")
                                                {
                                                    SzKd = "断电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                                else if (DrSet[i4]["type"].ToString() == "30")
                                                {
                                                    SzKd = "复电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                        }
                                    }
                                }
                                if (SzKd == "")
                                {
                                    SzKd = "正常";
                                    SzCs = SzCs1;
                                }
                            }
                        }
                        else
                        {
                            SzCs = "无";
                            SzKd = "正常";
                        }

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 21)
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                        else if (ClType == 10)
                        {
                            obj[0] = "1";
                            obj[5] = "上限报警";
                        }
                        else if (ClType == 11)
                        {
                            obj[0] = "0";
                            obj[5] = "上限报警解除";
                        }
                        else if (ClType == 12)
                        {
                            obj[0] = "1";
                            obj[5] = "上限断电";
                        }
                        else if (ClType == 13)
                        {
                            obj[0] = "0";
                            obj[5] = "上限断电解除";
                        }
                        else if (ClType == 16)
                        {
                            obj[0] = "1";
                            obj[5] = "下限报警";
                        }
                        else if (ClType == 17)
                        {
                            obj[0] = "0";
                            obj[5] = "下限报警解除";
                        }
                        else if (ClType == 18)
                        {
                            obj[0] = "1";
                            obj[5] = "下限断电";
                        }
                        else if (ClType == 19)
                        {
                            obj[0] = "0";
                            obj[5] = "下限断电解除";
                        }
                        else if (ClType == 20)
                        {
                            if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                            {
                                obj[0] = "1";
                                obj[5] = "断线断电";
                            }
                            else
                            {
                                obj[0] = "0";
                                obj[5] = "正常";
                            }
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);


                        Index++;
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        centerTime = GetCenterDateTime();
                        if (DtS.ToShortDateString() == centerTime.ToShortDateString()) //说明查询的是当天的曲线 
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                        }
                        DrMid = dtMidVal.Select("stime<='" + DtS + "' and etime>='" + DtS + "'", "stime");
                        if (dtMidVal.Rows.Count > 0)
                        {
                            if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                            {
                                SzKd = "正常";
                                SzCs = "无";
                            }
                            else
                            {
                                for (var ifor = 0; ifor < DrMid.Length; ifor++)
                                {
                                    SzCs1 = DrMid[0]["cs"].ToString();
                                    if (DrMid[0]["kzk"].ToString() == "")
                                        SzKd = "正常";
                                    else
                                    {
                                        Szz = DrMid[0]["kzk"].ToString().Split('|');
                                        for (var ij = 0; ij < Szz.Length; ij++)
                                        {
                                            DrSet =
                                                dtMidVal.Select(
                                                    " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"]
                                                    + "' and etime>='" + DrMid[0]["stime"] + "'", "stime");

                                            for (i4 = 0; i4 < DrSet.Length; i4++)
                                                if (DrSet[i4]["type"].ToString() == "32")
                                                {
                                                    SzKd = "断电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                                else if (DrSet[i4]["type"].ToString() == "30")
                                                {
                                                    SzKd = "复电异常";
                                                    SzCs = DrSet[i4]["Cs"].ToString();
                                                    break;
                                                }
                                        }
                                    }
                                }
                                if (SzKd == "")
                                {
                                    SzKd = "正常";
                                    SzCs = SzCs1;
                                }
                            }
                        }
                        else
                        {
                            SzCs = "无";
                            SzKd = "正常";
                        }
                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 21)
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                        else if (ClType == 10)
                        {
                            obj[0] = "1";
                            obj[5] = "上限报警";
                        }
                        else if (ClType == 11)
                        {
                            obj[0] = "0";
                            obj[5] = "上限报警解除";
                        }
                        else if (ClType == 12)
                        {
                            obj[0] = "1";
                            obj[5] = "上限断电";
                        }
                        else if (ClType == 13)
                        {
                            obj[0] = "0";
                            obj[5] = "上限断电解除";
                        }
                        else if (ClType == 16)
                        {
                            obj[0] = "1";
                            obj[5] = "下限报警";
                        }
                        else if (ClType == 17)
                        {
                            obj[0] = "0";
                            obj[5] = "下限报警解除";
                        }
                        else if (ClType == 18)
                        {
                            obj[0] = "1";
                            obj[5] = "下限断电";
                        }
                        else if (ClType == 19)
                        {
                            obj[0] = "0";
                            obj[5] = "下限断电解除";
                        }
                        else if (ClType == 20)
                        {
                            if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                            {
                                obj[0] = "1";
                                obj[5] = "断线断电";
                            }
                            else
                            {
                                obj[0] = "0";
                                obj[5] = "正常";
                            }
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 21)
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                        else if (ClType == 10)
                        {
                            obj[0] = "1";
                            obj[5] = "上限报警";
                        }
                        else if (ClType == 11)
                        {
                            obj[0] = "0";
                            obj[5] = "上限报警解除";
                        }
                        else if (ClType == 12)
                        {
                            obj[0] = "1";
                            obj[5] = "上限断电";
                        }
                        else if (ClType == 13)
                        {
                            obj[0] = "0";
                            obj[5] = "上限断电解除";
                        }
                        else if (ClType == 16)
                        {
                            obj[0] = "1";
                            obj[5] = "下限报警";
                        }
                        else if (ClType == 17)
                        {
                            obj[0] = "0";
                            obj[5] = "下限报警解除";
                        }
                        else if (ClType == 18)
                        {
                            obj[0] = "1";
                            obj[5] = "下限断电";
                        }
                        else if (ClType == 19)
                        {
                            obj[0] = "0";
                            obj[5] = "下限断电解除";
                        }
                        else if (ClType == 20)
                        {
                            if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                            {
                                obj[0] = "1";
                                obj[5] = "断线断电";
                            }
                            else
                            {
                                obj[0] = "0";
                                obj[5] = "正常";
                            }
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);

                        Index++;
                        break;

                        #endregion
                    }
                    Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                              Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                    if (dtMidVal.Rows.Count > 0)
                    {
                        DrMid = dtMidVal.Select("stime<='" + DtS + "' and etime>='" + DtS + "'");

                        if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                        {
                            SzKd = "正常";
                            SzCs = "无";
                        }
                        #region

                        else
                        {
                            SzCs1 = DrMid[0]["cs"].ToString();
                            if (DrMid[0]["kzk"].ToString() == "")
                                SzKd = "正常";
                            else
                            {
                                Szz = DrMid[0]["kzk"].ToString().Split('|');
                                for (var ij = 0; ij < Szz.Length; ij++)
                                {
                                    DrSet =
                                        dtMidVal.Select(
                                            " point ='" + Szz[ij] + "' and stime<='" + DrMid[0]["etime"] +
                                            "' and etime>='" + DrMid[0]["stime"] + "'", "stime");
                                    for (i4 = 0; i4 < DrSet.Length; i4++)
                                        if (DrSet[i4]["type"].ToString() == "32")
                                        {
                                            SzKd = "断电异常";
                                            SzCs = DrSet[i4]["Cs"].ToString();
                                            break;
                                        }
                                        else if (DrSet[i4]["type"].ToString() == "30")
                                        {
                                            SzKd = "复电异常";
                                            SzCs = DrSet[i4]["Cs"].ToString();
                                            break;
                                        }
                                }
                            }
                            if (SzKd == "")
                            {
                                SzKd = "正常";
                                SzCs = SzCs1;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        SzKd = "正常";
                        SzCs = "无";
                    }

                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = DateTime.Parse(Dtval.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 21)
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }
                    else if (ClType == 10)
                    {
                        obj[0] = "1";
                        obj[5] = "上限报警";
                    }
                    else if (ClType == 11)
                    {
                        obj[0] = "0";
                        obj[5] = "上限报警解除";
                    }
                    else if (ClType == 12)
                    {
                        obj[0] = "1";
                        obj[5] = "上限断电";
                    }
                    else if (ClType == 13)
                    {
                        obj[0] = "0";
                        obj[5] = "上限断电解除";
                    }
                    else if (ClType == 16)
                    {
                        obj[0] = "1";
                        obj[5] = "下限报警";
                    }
                    else if (ClType == 17)
                    {
                        obj[0] = "0";
                        obj[5] = "下限报警解除";
                    }
                    else if (ClType == 18)
                    {
                        obj[0] = "1";
                        obj[5] = "下限断电";
                    }
                    else if (ClType == 19)
                    {
                        obj[0] = "0";
                        obj[5] = "下限断电解除";
                    }
                    else if (ClType == 20)
                    {
                        if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                        {
                            obj[0] = "1";
                            obj[5] = "断线断电";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                    }
                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    Index++;
                    ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                    DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                    Startime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                    SzKd = "";
                    SzCs = "";
                    Endtime = 0;
                    SzCs1 = "";
                }

                if (Dtval.Rows.Count == 1)
                {
                    DtS = Convert.ToDateTime(Dtval.Rows[0]["timer"].ToString());
                    Startime = Convert.ToDateTime(Dtval.Rows[0]["timer"]).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[0]["timer"]).Minute;

                    centerTime = GetCenterDateTime();
                    if (Convert.ToDateTime(Dtval.Rows[0]["timer"]).ToShortDateString() ==
                        centerTime.ToShortDateString())
                    {
                        Endtime = centerTime.Hour * 60 + centerTime.Minute;
                    }
                    else
                    {
                        Endtime = 1439;
                        centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                    }
                    //

                    #region

                    if (dtMidVal.Rows.Count > 0)
                    {
                        DrMid = dtMidVal.Select("stime<='" + DtS + "' and etime>='" + DtS + "'");

                        if (DrMid.Length <= 0) //在KJ_DataAlarm表中没有相应的记录
                        {
                            SzKd = "正常";
                            SzCs = "无";
                        }
                        #region

                        else
                        {
                            SzCs1 = DrMid[0]["cs"].ToString();
                            if (DrMid[0]["kzk"].ToString() == "")
                                SzKd = "正常";
                            else
                            {
                                DrSet =
                                    dtMidVal.Select(
                                        " point in('" + DrMid[0]["kzk"].ToString().Replace("|", "','") +
                                        "') and stime<='" + DrMid[0]["etime"] + "' and etime>='" + DrMid[0]["stime"] +
                                        "'", "stime");
                                for (i4 = 0; i4 < DrSet.Length; i4++)
                                    if (DrSet[i4]["type"].ToString() == "32")
                                    {
                                        SzKd = "断电异常";
                                        SzCs = DrSet[i4]["Cs"].ToString();
                                        break;
                                    }
                                    else if (DrSet[i4]["type"].ToString() == "30")
                                    {
                                        SzKd = "复电异常";
                                        SzCs = DrSet[i4]["Cs"].ToString();
                                        break;
                                    }
                            }
                            if (SzKd == "")
                            {
                                SzKd = "正常";
                                SzCs = SzCs1;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        SzKd = "正常";
                        SzCs = "无";
                    }

                    #endregion

                    #region

                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 21)
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }
                    else if (ClType == 10)
                    {
                        obj[0] = "1";
                        obj[5] = "上限报警";
                    }
                    else if (ClType == 11)
                    {
                        obj[0] = "0";
                        obj[5] = "上限报警解除";
                    }
                    else if (ClType == 12)
                    {
                        obj[0] = "1";
                        obj[5] = "上限断电";
                    }
                    else if (ClType == 13)
                    {
                        obj[0] = "0";
                        obj[5] = "上限断电解除";
                    }
                    else if (ClType == 16)
                    {
                        obj[0] = "1";
                        obj[5] = "下限报警";
                    }
                    else if (ClType == 17)
                    {
                        obj[0] = "0";
                        obj[5] = "下限报警解除";
                    }
                    else if (ClType == 18)
                    {
                        obj[0] = "1";
                        obj[5] = "下限断电";
                    }
                    else if (ClType == 19)
                    {
                        obj[0] = "0";
                        obj[5] = "下限断电解除";
                    }
                    else if (ClType == 20)
                    {
                        if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                        {
                            obj[0] = "1";
                            obj[5] = "断线断电";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                    }

                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    obj = new object[dtR.Columns.Count];
                    obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 21)
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }
                    else if (ClType == 10)
                    {
                        obj[0] = "1";
                        obj[5] = "上限报警";
                    }
                    else if (ClType == 11)
                    {
                        obj[0] = "0";
                        obj[5] = "上限报警解除";
                    }
                    else if (ClType == 12)
                    {
                        obj[0] = "1";
                        obj[5] = "上限断电";
                    }
                    else if (ClType == 13)
                    {
                        obj[0] = "0";
                        obj[5] = "上限断电解除";
                    }
                    else if (ClType == 16)
                    {
                        obj[0] = "1";
                        obj[5] = "下限报警";
                    }
                    else if (ClType == 17)
                    {
                        obj[0] = "0";
                        obj[5] = "下限报警解除";
                    }
                    else if (ClType == 18)
                    {
                        obj[0] = "1";
                        obj[5] = "下限断电";
                    }
                    else if (ClType == 19)
                    {
                        obj[0] = "0";
                        obj[5] = "下限断电解除";
                    }
                    else if (ClType == 20)
                    {
                        if (!string.IsNullOrEmpty(DrMid[0]["kzk"].ToString()))
                        {
                            obj[0] = "1";
                            obj[5] = "断线断电";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                    }
                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    #endregion
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getMnlBjLineDt" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dtR
            };
            return ret;
        }

        /// <summary>
        ///     获取控制量馈电异常记录
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKzlLineDt(GetKzlLineDtRequest request)
        {
            var dtR = new DataTable();
            dtR.TableName = "getKzlLineDt";
            //          
            dtR.Columns.Add("C"); //状态值
            dtR.Columns.Add("D"); //馈电状态
            dtR.Columns.Add("E"); //措施
            dtR.Columns.Add("sTimer"); //开始时间
            dtR.Columns.Add("eTimer"); //结果时间
            dtR.Columns.Add("stateName"); //状态描述

            var Index = 0;
            var Startime = 0; //本次状态开始时间
            var Endtime = 0; //本次状态结束时间(分)
            var SzCs = ""; //措施
            var SzCs1 = ""; //当没有馈电是用报警的措施暂存变量
            var SzKd = ""; //馈电状态
            var DtS = new DateTime();
            DataView Dview = null;
            DataView Dview1 = null;
            var centerTime = new DateTime();
            var sz = "";
            var stime = request.SzNameS.ToShortDateString();
            var etime = request.SzNameE.ToShortDateString() + " 23:59:59";
            try
            {
                var SzTable = "KJ_DataRunRecord" + request.SzNameS.Year + request.SzNameS.Month.ToString("00");

                if (request.SzNameS.Month == request.SzNameE.Month)
                {
                    sz = "select fzh,kh,type,timer,val from " + SzTable + " where (PointID='" + request.CurrentPointID
                         + "'   and timer>='" + stime + "' and timer<='" + etime + "') ";

                }
                else
                {

                    //var SzTable1 = "KJ_DataRunRecord" + request.SzNameE.Year + request.SzNameE.Month.ToString("00");

                    //sz += "union all select fzh,kh,type,timer,val from " + SzTable1 + " where (PointID='" +
                    //      request.CurrentPointID
                    //      + "' and type in (29,30,31,32) and timer>='" + stime + "' and timer<='" + etime + "') order by timer,id";
                    //处理跨月的情况  20170629
                    string strSql = "";
                    string strSql1 = "";
                    string tablenameS = "KJ_DataRunRecord" + request.SzNameS.ToString("yyyyMM");
                    string tablenameE = "KJ_DataRunRecord" + request.SzNameE.ToString("yyyyMM");
                    if (GetDBType().Data.ToLower() == "sqlserver")
                    {
                        strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameS +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                        strSql1 = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + tablenameE +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                    }
                    else if (GetDBType().Data == "mysql")
                    {
                        strSql = "SHOW TABLES LIKE  '" + tablenameS + "';";
                        strSql1 = "SHOW TABLES LIKE  '" + tablenameE + "';";
                    }
                    var tempdtIsExit = _listexRepositoryBase.QueryTableBySql(strSql);
                    var tempdtIsExit1 = _listexRepositoryBase.QueryTableBySql(strSql1);
                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        strSql = "select fzh,kh,type,timer,val from " + tablenameS + " where (PointID='" + request.CurrentPointID
                         + "'   and timer>='" + stime + "' and timer<='" + etime + "')";
                        sz = strSql;
                    }
                    if (tempdtIsExit1.Rows.Count > 0)
                    {
                        strSql1 = " select fzh,kh,type,timer,val from " + tablenameE + " where (PointID='" + request.CurrentPointID
                         + "'  and timer>='" + stime + "' and timer<='" + etime + "')";
                        if (sz != "")
                        {
                            sz = sz + " union all " + strSql1;
                        }
                        else
                        {
                            sz = strSql1;
                        }
                    }
                }
                string[] Szz = null;
                var Dtval = new DataTable();
                var Dt = new DataTable();
                var Dtone = new DataTable();
                //Dtval = GetDataTableBySQL(sz);
                Dtval = _listexRepositoryBase.QueryTableBySql(sz);

                KzlConvertTimeToDt(ref Dtval);//过滤重复时间 
                //ConvertTypeToDt(ref Dtval);
                Dview = Dtval.DefaultView;
                Dview.Sort = "fzh,kh,timer ";

                var ClType = 0;
                if (Dtval.Rows.Count <= 0)
                {
                    var ret2 = new BasicResponse<DataTable>()
                    {
                        Data = dtR
                    };
                    return ret2;
                }


                DataRow[] DrMid = null;
                DataRow[] DrSet = null;
                var i4 = 0;
                var obj = new object[dtR.Columns.Count];
                for (var i = 0; i < Dtval.Rows.Count; i++)
                {
                    if (i == 0) //分析第一条记录
                    {
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        continue;
                    }
                    if (i == Dtval.Rows.Count - 1) //查询的是最后一条记录
                    {
                        #region 处理最后一次记录

                        Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                                  Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;

                        SzCs = "";
                        SzKd = "";


                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = DateTime.Parse(Dtval.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 30)
                        {
                            obj[0] = "1";
                            obj[5] = "复电失败";
                        }
                        else if (ClType == 32)
                        {
                            obj[0] = "1";
                            obj[5] = "断电失败";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }

                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);
                        Index++;
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        centerTime = GetCenterDateTime();
                        if (DtS.ToShortDateString() == centerTime.ToShortDateString()) //说明查询的是当天的曲线 
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                        }

                        SzCs = "";
                        SzKd = "";

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 30)
                        {
                            obj[0] = "1";
                            obj[5] = "复电失败";
                        }
                        else if (ClType == 32)
                        {
                            obj[0] = "1";
                            obj[5] = "断电失败";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                        if (ClType == 30)
                        {
                            obj[0] = "1";
                            obj[5] = "复电失败";
                        }
                        else if (ClType == 32)
                        {
                            obj[0] = "1";
                            obj[5] = "断电失败";
                        }
                        else
                        {
                            obj[0] = "0";
                            obj[5] = "正常";
                        }
                        obj[1] = SzKd;
                        obj[2] = SzCs;

                        dtR.Rows.Add(obj);

                        Index++;
                        break;

                        #endregion
                    }
                    Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                              Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;


                    SzKd = "";
                    SzCs = "";

                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = DateTime.Parse(Dtval.Rows[i]["timer"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 30)
                    {
                        obj[0] = "1";
                        obj[5] = "复电失败";
                    }
                    else if (ClType == 32)
                    {
                        obj[0] = "1";
                        obj[5] = "断电失败";
                    }
                    else
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }
                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    Index++;
                    ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                    DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString());
                    Startime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;
                    SzKd = "";
                    SzCs = "";
                    Endtime = 0;
                    SzCs1 = "";
                }

                if (Dtval.Rows.Count == 1)
                {
                    DtS = Convert.ToDateTime(Dtval.Rows[0]["timer"].ToString());
                    Startime = Convert.ToDateTime(Dtval.Rows[0]["timer"]).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[0]["timer"]).Minute;

                    centerTime = GetCenterDateTime();
                    if (Convert.ToDateTime(Dtval.Rows[0]["timer"]).ToShortDateString() ==
                        centerTime.ToShortDateString())
                    {
                        Endtime = centerTime.Hour * 60 + centerTime.Minute;
                    }
                    else
                    {
                        Endtime = 1439;
                        centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                    }
                    //

                    #region

                    SzKd = "";
                    SzCs = "";

                    #endregion

                    #region

                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 30)
                    {
                        obj[0] = "1";
                        obj[5] = "复电失败";
                    }
                    else if (ClType == 32)
                    {
                        obj[0] = "1";
                        obj[5] = "断电失败";
                    }
                    else
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }

                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    obj = new object[dtR.Columns.Count];
                    obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");
                    obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss");

                    if (ClType == 30)
                    {
                        obj[0] = "1";
                        obj[5] = "复电失败";
                    }
                    else if (ClType == 32)
                    {
                        obj[0] = "1";
                        obj[5] = "断电失败";
                    }
                    else
                    {
                        obj[0] = "0";
                        obj[5] = "正常";
                    }
                    obj[1] = SzKd;
                    obj[2] = SzCs;

                    dtR.Rows.Add(obj);

                    #endregion
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getKzlLineDt" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dtR
            };
            return ret;
        }

        /// <summary>
        /// 获取控制量状态变化曲线
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKzlStateLineDt(GetStateLineDtRequest request)
        {
            var dtR = new DataTable();
            dtR.TableName = "GetKzlStateLineDt";
            //          
            dtR.Columns.Add("C");       //0
            dtR.Columns.Add("D");       //1
            dtR.Columns.Add("E");       //2
            dtR.Columns.Add("sTimer");        //3
            dtR.Columns.Add("eTimer");        //4
            dtR.Columns.Add("stateName");       //5

            var Index = 0;
            var Startime = 0; //本次状态开始时间
            var Endtime = 0; //本次状态结束时间(分)
            var SzCs = ""; //措施
            var SzCs1 = ""; //当没有馈电是用报警的措施暂存变量
            var SzKd = ""; //馈电状态
            var DtS = new DateTime();
            string XS1 = "", XS2 = "", XS3 = "";
            DataView Dview = null;
            DataView Dview1 = null;
            var centerTime = new DateTime();
            var sz = "";
            var Stime = DateTime.Parse(request.SzNameT.ToShortDateString());
            var Etime = DateTime.Parse(request.SzNameT.ToShortDateString() + " 23:59:59");
            try
            {
                sz = "select KJ_DeviceDefInfo.Bz6,KJ_DeviceDefInfo.Bz7,KJ_DeviceDefInfo.Bz8 from KJ_DeviceDefInfo where KJ_DeviceDefInfo.pointid ='" + request.CurrentPointID + "'";
                //var tempdevdt = GetDataTableBySQL(sz);
                var tempdevdt = _listexRepositoryBase.QueryTableBySql(sz);
                if (tempdevdt.Rows.Count > 0)
                {
                    XS1 = tempdevdt.Rows[0]["Bz6"].ToString();
                    XS2 = tempdevdt.Rows[0]["Bz7"].ToString();
                    XS3 = tempdevdt.Rows[0]["Bz8"].ToString();
                }
                else
                {
                    XS1 = "无";
                    XS2 = "无";
                    XS3 = "无";
                }
                //获取设备的分站号  2016-06-14
                var TempFzh = GetPointFzh(request.CurrentPointID);

                var SzTable = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");
                if (request.kglztjsfs)
                    sz = "select fzh,kh,type,timer,val,point from " + SzTable + " where ((PointID='" + request.CurrentPointID +
                         "' and (type=43 or type=44 ) )or (fzh=0 and kh=0) or(type=0 and fzh=" + TempFzh
                         + ")) and timer>='" + Stime + "' and timer<='" + Etime + "' order by timer,id";
                else
                    sz = "select fzh,kh,type,timer,val,point from " + SzTable + " where ((PointID='" + request.CurrentPointID
                         + "' and (type=43 or type=44 ) )) and timer>='" + Stime + "' and timer<='" + Etime +
                         "' order by timer,id";
                string[] Szz = null;
                var Dtval = new DataTable();
                var Dt = new DataTable();
                var Dtone = new DataTable();
                //Dtval = GetDataTableBySQL(sz);
                Dtval = _listexRepositoryBase.QueryTableBySql(sz);      //KJ_DataRunRecord表数据

                ConvertTimeToDt(ref Dt);//过滤重复时间 

                ConvertDt(ref Dtval);
                Dview = Dtval.DefaultView;
                Dview.Sort = "fzh,kh,timer ";

                var ClType = 0;
                if (Dtval.Rows.Count <= 0)
                {
                    var ret = new BasicResponse<DataTable>
                    {
                        Data = dtR
                    };
                    return ret;
                }






                DataRow[] DrMid = null;
                DataRow[] DrSet = null;
                var i4 = 0;
                var obj = new object[dtR.Columns.Count];
                for (var i = 0; i < Dtval.Rows.Count; i++)
                {
                    if (i == 0) //分析第一条记录
                    {
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        continue;
                    }
                    if (i == Dtval.Rows.Count - 1) //查询的是最后一条记录
                    {
                        #region 处理最后一次记录

                        Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                                  Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;

                        SzKd = "正常";
                        SzCs = "无";


                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = Convert.ToDateTime(Dtval.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 43)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 44)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }

                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);
                        Index++;
                        ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                        DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                        Startime = DtS.Hour * 60 + DtS.Minute;
                        centerTime = GetCenterDateTime();
                        if (DtS.ToShortDateString() == centerTime.ToShortDateString()) //说明查询的是当天的曲线 
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59.000");
                        }

                        SzKd = "正常";
                        SzCs = "无";

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 43)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 44)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }

                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 43)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 44)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }

                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        Index++;
                        break;

                        #endregion
                    }
                    Endtime = Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Hour * 60 +
                              Convert.ToDateTime(Dtval.Rows[i]["timer"].ToString()).Minute;

                    SzKd = "正常";
                    SzCs = "无";


                    if (Startime > Endtime)
                        Endtime = Startime;

                    obj = new object[dtR.Columns.Count];
                    obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    obj[4] = Convert.ToDateTime(Dtval.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    if (ClType == 10)
                    {
                        obj[0] = "0.00001";
                        obj[1] = "未记录";
                        obj[2] = "未记录";
                        obj[5] = "未记录";
                    }
                    else
                    {
                        if (ClType == 43)
                        {
                            obj[0] = "0";
                            obj[5] = XS1;
                        }
                        else if (ClType == 44)
                        {
                            obj[0] = "1";
                            obj[5] = XS2;
                        }

                        obj[1] = SzKd;
                        obj[2] = SzCs;
                    }
                    dtR.Rows.Add(obj);

                    Index++;
                    ClType = Convert.ToInt32(Dtval.Rows[i]["type"].ToString());
                    DtS = Convert.ToDateTime(Dtval.Rows[i]["timer"]);
                    Startime = Convert.ToDateTime(Dtval.Rows[i]["timer"]).Hour * 60 +
                               Convert.ToDateTime(Dtval.Rows[i]["timer"]).Minute;
                    SzKd = "";
                    SzCs = "";
                    Endtime = 0;
                    SzCs1 = "";
                }

                #region 只有一条记录
                if (Dtval.Rows.Count == 1)
                    if (Dtval.Rows[0]["type"].ToString() == "10")
                    {
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd") + " 00:00:00";
                        obj[4] = DtS.ToString("yyyy-MM-dd") + " 23:59:59";
                        obj[0] = "0.00001";
                        obj[1] = "未记录";
                        obj[2] = "未记录";
                        obj[5] = "未记录";
                        dtR.Rows.Add(obj);
                    }
                    else
                    {
                        DtS = Convert.ToDateTime(Dtval.Rows[0]["timer"].ToString());
                        Startime = Convert.ToDateTime(Dtval.Rows[0]["timer"]).Hour * 60 +
                                   Convert.ToDateTime(Dtval.Rows[0]["timer"]).Minute;

                        centerTime = GetCenterDateTime();
                        if (Convert.ToDateTime(Dtval.Rows[0]["timer"]).ToShortDateString() ==
                            centerTime.ToShortDateString())
                        {
                            Endtime = centerTime.Hour * 60 + centerTime.Minute;
                        }
                        else
                        {
                            Endtime = 1439;
                            centerTime = DateTime.Parse(DtS.ToShortDateString() + " 23:59:59");
                        }

                        #region


                        SzKd = "正常";
                        SzCs = "无";


                        #endregion

                        #region

                        if (Startime > Endtime)
                            Endtime = Startime;
                        obj = new object[dtR.Columns.Count];
                        obj[3] = DtS.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 43)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 44)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }

                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        obj = new object[dtR.Columns.Count];
                        obj[3] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        obj[4] = centerTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        if (ClType == 10)
                        {
                            obj[0] = "0.00001";
                            obj[1] = "未记录";
                            obj[2] = "未记录";
                            obj[5] = "未记录";
                        }
                        else
                        {
                            if (ClType == 43)
                            {
                                obj[0] = "0";
                                obj[5] = XS1;
                            }
                            else if (ClType == 44)
                            {
                                obj[0] = "1";
                                obj[5] = XS2;
                            }

                            obj[1] = SzKd;
                            obj[2] = SzCs;
                        }
                        dtR.Rows.Add(obj);

                        #endregion
                    }
                #endregion

            }
            catch (Exception Ex)
            {
                LogHelper.Error("GetKzlStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            var ret2 = new BasicResponse<DataTable>
            {
                Data = dtR
            };
            return ret2;
        }

        /// <summary>
        ///     开关量曲线、柱状图 查询断电范围、报警/解除、断电/复电、馈电状态、措施及时刻
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        public BasicResponse<string[]> GetDgView(GetDgViewRequest request)
        {
            var NullEtime = DateTime.Parse("1900-01-01 00:00:00");
            /* 0:名称及类型
            * 1:报警及断电状态
            * 2:读值时区
            * 3:读值时刻
            * 4:开机率
            * 5:开机时间
            * 6:开停次数
            * 7:报警/解除
            * 8:断电/复电
            * 9:馈电状态
            * 10:措施及时刻
            */
            var QueryStr = new string[11];
            var Dt = request.SzNameT;
            var SzSql = "";
            try
            {
                if (request.SzNameT.Year < 2000)
                {
                    var ret1 = new BasicResponse<string[]>
                    {
                        Data = QueryStr
                    };
                    return ret1;
                }

                var SzTable = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");

                //获取设备的分站号  2016-06-14                
                var TempFzh = GetPointFzh(request.CurrentPointID);

                SzSql = "select  KJ_DeviceAddress.wz,KJ_DeviceType.name," + SzTable + ".type,timer,val from " + SzTable
                        + " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + SzTable +
                        ".wzid left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SzTable
                        + ".devid where timer<='" + Dt + "' and (PointID='" + request.CurrentPointID
                        + "'  or (fzh=0 and kh=0) or(" + SzTable + ".type=0 and fzh=" + TempFzh +
                        "))  order by timer DeSC," + SzTable + ".id DESC";

                var Dtval = new DataTable();
                //Dtval = GetDataTableBySQL(SzSql);
                Dtval = _listexRepositoryBase.QueryTableBySql(SzSql);
                var Val = new DataTable();
                var Dtone = new DataTable();
                var SzName = "";
                QueryStr[9] = "";
                QueryStr[10] = "";
                QueryStr[7] = "";
                var SzCs1 = "";
                var sz = "";

                SzName = "KJ_DataAlarm" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");
                var typeStrtj = true;
                if (Dtval.Rows.Count > 0 && request.kglztjsfs)
                    typeStrtj = Dtval.Rows[0]["type"].ToString() != "0";
                else
                    typeStrtj = true;
                if ((Dtval.Rows.Count > 0) && typeStrtj)
                {
                    //QueryStr[0] = cbbqx.Text.Substring(cbbqx.Text.IndexOf('.') + 1, cbbqx.Text.Length - 1 - cbbqx.Text.IndexOf('.'));

                    //if (QueryStr[0].Length < 5)
                    //    QueryStr[0] = "";
                    //QueryStr[0] = Dtval.Rows[0][0].ToString() + "\\" + Dtval.Rows[0][1].ToString();
                    if ((Dtval.Rows[0]["type"].ToString() != "25") && (Dtval.Rows[0]["type"].ToString() != "26") &&
                        (Dtval.Rows[0]["type"].ToString() != "27"))
                        QueryStr[1] = "未记录";
                    else
                        QueryStr[1] = Dtval.Rows[0]["val"].ToString();


                    sz = "select kzk,kdid,stime,etime,cs,type,isalarm from " + SzName +
                         " where (stime<='" + Dt + "' and etime>='" + Dt + "' or stime<='"
                         + Dt + "' and etime ='" + NullEtime + "') and PointID='" + request.CurrentPointID +
                         "'  order by stime DESC";

                    //Val = GetDataTableBySQL(sz);
                    Val = _listexRepositoryBase.QueryTableBySql(sz);

                    if ((Val.Rows.Count <= 0) || (Dt > GetCenterDateTime())) //在KJ_DataAlarm表中没有相应的记录
                    {
                        QueryStr[7] = "解除";
                        QueryStr[8] = "复电";
                        QueryStr[9] = "正常";
                        QueryStr[10] = "无";
                    }
                    else //只要在报警表里面找得到记录说明其是报警的
                    {
                        if (int.Parse(Val.Rows[0]["isalarm"].ToString()) > 0) //isalarm>0则认为是报警状态   20150831
                            QueryStr[7] = "报警";
                        else
                            QueryStr[7] = "解除";

                        //将主控的措施加到列表进行显示   20150831
                        if (!QueryStr[10].Contains(Val.Rows[0]["cs"].ToString()))
                            if (QueryStr[10] != "")
                                QueryStr[10] += "|" + Val.Rows[0]["cs"];
                            else
                                QueryStr[10] = Val.Rows[0]["cs"].ToString();
                        //查询馈电失败情况
                        if (Val.Rows[0]["kdid"].ToString() != "")
                        {
                            QueryStr[8] = "断电";
                            SzCs1 = Val.Rows[0]["cs"].ToString();

                            var Szz = Val.Rows[0]["kdid"].ToString().Split(',');
                            var KdId = "";
                            for (var ip = 0; ip < Szz.Length; ip++)
                                if (!string.IsNullOrEmpty(Szz[ip]))
                                    KdId = KdId + Szz[ip] + ",";
                            if (KdId.Contains(","))
                                KdId = KdId.Substring(0, KdId.Length - 1);

                            sz = "select KJ_DeviceAddress.wz,cs,type,point from " + SzName +
                                 " left outer join KJ_DeviceAddress on KJ_DeviceAddress.wzid=" + SzName + ".wzid" +
                                 " where " + SzName + ".ID in(" + KdId + ") ";
                            //Dtone = GetDataTableBySQL(sz);
                            Dtone = _listexRepositoryBase.QueryTableBySql(sz);

                            //不读取本地控制口信息，直接从报警表中读取控制信息  20150907
                            //List<string> Plst = new List<string>();
                            //string[] SSz = null;
                            var AddFlag = true;
                            //string Szl = InterfaceClass.QueryPubClass_.GetKzk(SzPoint);//
                            //SSz = Szl.Split('|');
                            //for (int ip = 0; ip < SSz.Length; ip++)
                            //    Plst.Add(SSz[ip]);

                            for (var iv = 0; iv < Dtone.Rows.Count; iv++)
                            {
                                //if (Plst.Contains(Dtone.Rows[iv]["point"].ToString()))
                                //    Plst.Remove(Dtone.Rows[iv]["point"].ToString());
                                AddFlag = false;
                                var Sz = Dtone.Rows[iv]["wz"].ToString();
                                if (Sz == "")
                                    Sz = GetAddr(Dtone.Rows[iv]["point"].ToString());
                                if ((QueryStr[9] == null) || (QueryStr[9] == ""))
                                {
                                    if (Dtone.Rows[iv]["type"].ToString() == "32")
                                    {
                                        QueryStr[9] = Sz + "/断电异常";
                                        AddFlag = true;
                                    }
                                    else if (Dtone.Rows[iv]["type"].ToString() == "30")
                                    {
                                        QueryStr[9] = Sz + "/复电异常";
                                        AddFlag = true;
                                    }
                                    if (AddFlag)
                                        QueryStr[10] = Dtone.Rows[iv]["cs"].ToString();
                                }
                                else
                                {
                                    if (Dtone.Rows[iv]["type"].ToString() == "32")
                                    {
                                        if (!QueryStr[9].Contains(Sz + "/断电异常"))
                                        {
                                            QueryStr[9] += "|" + Sz + "/断电异常";
                                            AddFlag = true;
                                        }
                                    }
                                    else if (Dtone.Rows[iv]["type"].ToString() == "30")
                                    {
                                        if (!QueryStr[9].Contains(Sz + "/复电异常"))
                                        {
                                            QueryStr[9] += "|" + Sz + "/复电异常";
                                            AddFlag = true;
                                        }
                                    }
                                    if (AddFlag)
                                        if (!QueryStr[10].Contains(Dtone.Rows[iv]["cs"].ToString()))
                                            if (QueryStr[10] != "")
                                                QueryStr[10] += "|" + Dtone.Rows[iv]["cs"];
                                            else
                                                QueryStr[10] = Dtone.Rows[iv]["cs"].ToString();
                                }
                            }
                        }
                        else
                            QueryStr[8] = "复电";

                        if (QueryStr[9] == "")
                            QueryStr[9] = "正常";
                        if (QueryStr[10] == "")
                            QueryStr[10] = SzCs1;
                    }
                }
                else
                {
                    QueryStr[7] = "解除";
                    QueryStr[8] = "复电";
                    QueryStr[9] = "无";
                    QueryStr[10] = "无";
                    //if (SzPoint.Length > 0)
                    //{                  
                    //        QueryStr[0] = cbbqx.Text.Substring(cbbqx.Text.IndexOf('.') + 1, cbbqx.Text.Length - 1 - cbbqx.Text.IndexOf('.'));                   
                    //}
                    //else
                    //    QueryStr[0] = "无";
                    QueryStr[1] = "未记录";
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_GetDgView" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<string[]>
            {
                Data = QueryStr
            };
            return ret;
        }

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
        public BasicResponse<string[]> GetKjThings(GetKjThingsRequest request)
        {
            var QueryStr = new string[11];

            QueryStr[4] = "0%";
            QueryStr[5] = "0分0秒";
            QueryStr[6] = "0";
            var Sz = "";
            try
            {
                if (request.DtStart.Year < 2000)
                {
                    var ret1 = new BasicResponse<string[]>
                    {
                        Data = QueryStr
                    };
                    return ret1;
                }

                var SzTable = "KJ_DataRunRecord" + request.DtStart.Year + request.DtStart.Month.ToString("00");

                //获取设备的分站号  2016-06-14                
                var TempFzh = GetPointFzh(request.CurrentPointID);

                if (request.kglztjsfs) //为0时查询 分站中断及系统退出 记录  20140428
                    Sz = "select id,fzh,kh,timer,type,val from " + SzTable + " where timer<='" + request.DtEnd +
                         "' and timer>='" + request.DtStart
                         + "' and ((PointID='" + request.CurrentPointID + "') or(fzh=0 and kh=0)or (fzh=" + TempFzh +
                         " and type=0))  order by timer";
                else //为1时 不查询 分站中断及系统退出 记录  20140428
                    Sz = "select id,fzh,kh,timer,type,val from " + SzTable + " where timer<='" + request.DtEnd +
                         "' and timer>='" + request.DtStart
                         + "' and ((PointID='" + request.CurrentPointID + "'))  order by timer,id";
                var Dt = new DataTable();
                //Dt = GetDataTableBySQL(Sz);
                Dt = _listexRepositoryBase.QueryTableBySql(Sz);
                ConvertDt(ref Dt);
                if (Dt.Rows.Count <= 0)
                {
                    if (request.DtStart >= GetCenterDateTime())
                    {
                        QueryStr[4] = "0%";
                        QueryStr[5] = "0分0秒";
                        QueryStr[6] = "0";
                    }
                    else
                    {
                        if (request.kglztjsfs) //为0时查询 分站中断及系统退出 记录  20140428
                            Sz = "select  type,val from " + SzTable + " where timer <'" + request.DtStart
                                 + "' and ((PointID='" + request.CurrentPointID + "')or(fzh=0 and kh=0)or (fzh=" + TempFzh
                                 + " and type=0))  order by timer DESC,id DESC";
                        else //为1时 不查询 分站中断及系统退出 记录  20140428
                            Sz = "select type,val from " + SzTable + " where timer <'" + request.DtStart + "' and ((PointID='" +
                                 request.CurrentPointID
                                 + "'))  order by timer DESC,id DESC";

                        var Dda = new DataTable();
                        //Dda = GetDataTableBySQL(Sz);
                        Dda = _listexRepositoryBase.QueryTableBySql(Sz);
                        //Dda = GetNewDataTable(Dt, "1=1");

                        //DataView dv = Dda.DefaultView;
                        //dv.Sort = "timer DESC,id DESC";  //排序
                        //Dda = dv.ToTable();

                        if (Dda.Rows.Count > 0)
                        {
                            if (Dda.Rows[0]["type"].ToString() == "27")
                            {
                                var TSS = new TimeSpan();
                                var id = 0;
                                if (request.DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() >= request.DtStart)
                                        TSS = GetCenterDateTime() - request.DtStart;
                                    id = Convert.ToInt32(TSS.Minutes);
                                    QueryStr[5] = TSS.Minutes + "Min";
                                    QueryStr[6] = "0";
                                    if (id > 60)
                                        id = 60;
                                    if (id < 0)
                                        id = 0;
                                    QueryStr[4] = Convert.ToString(id / 60.0 * 100) + "%";
                                    if (QueryStr[4].Length > 4)
                                        QueryStr[4] = QueryStr[4].Substring(0, 4) + "%";
                                }
                                else
                                {
                                    QueryStr[4] = "100%";
                                    QueryStr[5] = "60分00秒";
                                    QueryStr[6] = "0";
                                }
                            }
                        }
                        else
                        {
                            QueryStr[4] = "0%";
                            QueryStr[5] = "0分0秒";
                            QueryStr[6] = "0";
                        }
                    }
                }
                else
                {
                    double KjTime = 0; //以秒为单位计算
                    float TotalQj = 0; //区间秒为单位大小;
                    var Ts = request.DtEnd - request.DtStart;
                    var DtVal = new DateTime();
                    var iFlag = 0;
                    var iValue = "";
                    var bdCs = 0;
                    TotalQj = Convert.ToSingle(Ts.TotalSeconds);
                    ConvertTimeToDt(ref Dt);
                    for (var i = 0; i < Dt.Rows.Count; i++)
                    {
                        if ((i == 0) && (Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) == request.DtStart))
                        //如果第一个数据就和要查询的数据时间相同就不要再查一次数据了
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                        }
                        else if (i == 0)
                        {
                            if (request.kglztjsfs) //为0时查询 分站中断及系统退出 记录  20140428
                                Sz = "select  type,val from " + SzTable + " where timer <'" + request.DtStart +
                                     "' and ((PointID='" + request.CurrentPointID
                                     + "')or(fzh=0 and kh=0)or (fzh="
                                     + TempFzh + " and type=0)) order by timer DESC,id DESC";
                            else
                                Sz = "select  type,val from " + SzTable + " where timer <'" + request.DtStart +
                                     "' and ((PointID='" + request.CurrentPointID
                                     + "')) order by timer DESC,id DESC";
                            var Dda = new DataTable();
                            //Dda = GetDataTableBySQL(Sz);
                            Dda = _listexRepositoryBase.QueryTableBySql(Sz);
                            //Dda = GetNewDataTable(Dt, "1=1");

                            //DataView dv = Dda.DefaultView;
                            //dv.Sort = "timer DESC,id DESC";  //排序
                            //Dda = dv.ToTable();

                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                            if (Dda.Rows.Count > 0)
                                if (Dda.Rows[0]["type"].ToString() == "27")
                                {
                                    Ts = DtVal - request.DtStart;
                                    KjTime += Ts.TotalSeconds;
                                }

                            if (Dda.Rows.Count > 0)
                            {
                                iFlag = Convert.ToInt32(Dda.Rows[0]["type"].ToString());
                                iValue = Dda.Rows[0]["val"].ToString();
                                if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                    (Dt.Rows[i]["val"].ToString() != "断线")
                                    && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                    bdCs++;
                                iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                                iValue = Dt.Rows[i]["val"].ToString();
                            }
                            else //前面没有找到状态，则将当前状态作为初始状态  20140428
                            {
                                iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                                iValue = Dt.Rows[i]["val"].ToString();
                            }
                        }
                        else if (i == Dt.Rows.Count - 1)
                        {
                            if (iFlag == 27)
                            {
                                Ts = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                            if (iFlag == 27)
                            {
                                if (request.DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() > DtVal)
                                        Ts = GetCenterDateTime() - DtVal;
                                }
                                else
                                    Ts = request.DtEnd - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;
                        }
                        else
                        {
                            if (iFlag == 27)
                            {
                                Ts = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                        }

                        if (Dt.Rows.Count == 1)
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            if (iFlag == 27)
                            {
                                if (request.DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() > DtVal)
                                        Ts = GetCenterDateTime() - DtVal;
                                }
                                else
                                    Ts = request.DtEnd - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }
                        }
                    }
                    if (KjTime > 3600)
                        KjTime = 3600;
                    if (KjTime < 0)
                        KjTime = 0;
                    QueryStr[4] = (KjTime / TotalQj * 100).ToString("0.00") + "%";
                    //if (QueryStr[4].Length > 4)
                    //    QueryStr[4] = QueryStr[4].Substring(0, 4) + "%";
                    QueryStr[5] = Math.Floor(KjTime / 60).ToString("00") + "分" + (KjTime % 60).ToString("00") + "秒";
                    QueryStr[6] = bdCs.ToString();
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_GetKjThings" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<string[]>
            {
                Data = QueryStr
            };
            return ret;
        }

        /// <summary>
        ///     开关量柱状图 统计每小时开机率(优化查询效果，不用每次查询)
        /// </summary>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="kglztjsfs"></param>
        /// <param name="Dt_"></param>
        /// <param name="Dda_"></param>
        /// <returns></returns>
        public string[] GetKjThings(DateTime DtStart, DateTime DtEnd, string CurrentPointID, string CurrentDevid,
            string CurrentWzid, bool kglztjsfs, DataTable Dt_, DataTable Dda_)
        {
            var QueryStr = new string[11];

            QueryStr[4] = "0%";
            QueryStr[5] = "0分0秒";
            QueryStr[6] = "0";
            var Sz = "";
            try
            {
                var SzTable = "KJ_DataRunRecord" + DtStart.Year + DtStart.Month.ToString("00");

                //获取设备的分站号  2016-06-14                
                var TempFzh = GetPointFzh(CurrentPointID);
                var Dt = new DataTable();
                Dt = GetNewDataTable(Dt_, "timer<='" + DtEnd + "' and timer>='" + DtStart + "'");

                var dv = Dt.DefaultView;
                dv.Sort = "timer asc,id asc"; //排序
                Dt = dv.ToTable();

                ConvertDt(ref Dt);
                if (Dt.Rows.Count <= 0)
                {
                    if (DtStart >= GetCenterDateTime())
                    {
                        QueryStr[4] = "0%";
                        QueryStr[5] = "0分0秒";
                        QueryStr[6] = "0";
                    }
                    else
                    {
                        var Dda = new DataTable();

                        Dda = GetNewDataTable(Dda_, "timer <'" + DtStart + "'");

                        var dv1 = Dda.DefaultView;
                        dv1.Sort = "timer DESC,id DESC"; //排序
                        Dda = dv1.ToTable();

                        if (Dda.Rows.Count > 0)
                        {
                            if (Dda.Rows[0]["type"].ToString() == "27")
                            {
                                var TSS = new TimeSpan();
                                var id = 0;
                                if (DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() >= DtStart)
                                        TSS = GetCenterDateTime() - DtStart;
                                    id = Convert.ToInt32(TSS.Minutes);
                                    QueryStr[5] = TSS.Minutes + "Min";
                                    QueryStr[6] = "0";
                                    if (id > 60)
                                        id = 60;
                                    if (id < 0)
                                        id = 0;
                                    QueryStr[4] = Convert.ToString(id / 60.0 * 100) + "%";
                                    if (QueryStr[4].Length > 4)
                                        QueryStr[4] = QueryStr[4].Substring(0, 4) + "%";
                                }
                                else
                                {
                                    QueryStr[4] = "100%";
                                    QueryStr[5] = "60分00秒";
                                    QueryStr[6] = "0";
                                }
                            }
                        }
                        else
                        {
                            QueryStr[4] = "0%";
                            QueryStr[5] = "0分0秒";
                            QueryStr[6] = "0";
                        }
                    }
                }
                else
                {
                    double KjTime = 0; //以秒为单位计算
                    float TotalQj = 0; //区间秒为单位大小;
                    var Ts = DtEnd - DtStart;
                    var DtVal = new DateTime();
                    var iFlag = 0;
                    var iValue = "";
                    var bdCs = 0;
                    TotalQj = Convert.ToSingle(Ts.TotalSeconds);
                    ConvertTimeToDt(ref Dt);
                    for (var i = 0; i < Dt.Rows.Count; i++)
                    {
                        if ((i == 0) && (Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) == DtStart))
                        //如果第一个数据就和要查询的数据时间相同就不要再查一次数据了
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                        }
                        else if (i == 0)
                        {
                            var Dda = new DataTable();

                            Dda = GetNewDataTable(Dda_, "timer <'" + DtStart + "'");

                            var dv2 = Dda.DefaultView;
                            dv2.Sort = "timer DESC,id DESC"; //排序
                            Dda = dv2.ToTable();

                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                            if (Dda.Rows.Count > 0)
                                if (Dda.Rows[0]["type"].ToString() == "27")
                                {
                                    Ts = DtVal - DtStart;
                                    KjTime += Ts.TotalSeconds;
                                }

                            if (Dda.Rows.Count > 0)
                            {
                                iFlag = Convert.ToInt32(Dda.Rows[0]["type"].ToString());
                                iValue = Dda.Rows[0]["val"].ToString();
                                if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                    (Dt.Rows[i]["val"].ToString() != "断线")
                                    && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                    bdCs++;
                                iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                                iValue = Dt.Rows[i]["val"].ToString();
                            }
                            else //前面没有找到状态，则将当前状态作为初始状态  20140428
                            {
                                iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                                iValue = Dt.Rows[i]["val"].ToString();
                            }
                        }
                        else if (i == Dt.Rows.Count - 1)
                        {
                            if (iFlag == 27)
                            {
                                Ts = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                            if (iFlag == 27)
                            {
                                if (DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() > DtVal)
                                        Ts = GetCenterDateTime() - DtVal;
                                }
                                else
                                    Ts = DtEnd - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;
                        }
                        else
                        {
                            if (iFlag == 27)
                            {
                                Ts = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString()) - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }

                            if ((Dt.Rows[i]["type"].ToString() != iFlag.ToString()) &&
                                (Dt.Rows[i]["val"].ToString() != "断线")
                                && (Dt.Rows[i]["type"].ToString() != "10")) //断线不算作开停次数  20140428
                                bdCs++;

                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            DtVal = Convert.ToDateTime(Dt.Rows[i]["timer"].ToString());
                        }

                        if (Dt.Rows.Count == 1)
                        {
                            iFlag = Convert.ToInt32(Dt.Rows[i]["type"].ToString());
                            iValue = Dt.Rows[i]["val"].ToString();
                            if (iFlag == 27)
                            {
                                if (DtEnd > GetCenterDateTime())
                                {
                                    if (GetCenterDateTime() > DtVal)
                                        Ts = GetCenterDateTime() - DtVal;
                                }
                                else
                                    Ts = DtEnd - DtVal;
                                KjTime += Convert.ToInt32(Ts.TotalSeconds);
                            }
                        }
                    }
                    if (KjTime > 3600)
                        KjTime = 3600;
                    if (KjTime < 0)
                        KjTime = 0;
                    QueryStr[4] = (KjTime / TotalQj * 100).ToString("0.00") + "%";
                    //if (QueryStr[4].Length > 4)
                    //    QueryStr[4] = QueryStr[4].Substring(0, 4) + "%";
                    QueryStr[5] = Math.Floor(KjTime / 60).ToString("00") + "分" + (KjTime % 60).ToString("00") + "秒";
                    QueryStr[6] = bdCs.ToString();
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_GetKjThings" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     开关量柱状图数据
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>
        /// <returns></returns>
        public BasicResponse<DataTable> InitQxZhuZhuang(InitQxZhuZhuangRequest request)
        {
            var dtR = new DataTable();
            dtR.TableName = "InitQxZhuZhuang";
            dtR.Columns.Add("A");
            dtR.Columns.Add("B");
            dtR.Columns.Add("C");
            dtR.Columns.Add("timer");
            var QueryStr = new string[11];
            var SzSave = "";
            var obj = new object[dtR.Columns.Count];


            var Dt_ = new DataTable();
            var Dda_ = new DataTable();
            var Sz = "";
            var DtStart = request.SzNameT.ToShortDateString();
            var DtEnd = request.SzNameT.ToShortDateString() + " 23:59:59";

            var SzTable = "KJ_DataRunRecord" + request.SzNameT.Year + request.SzNameT.Month.ToString("00");

            //获取设备的分站号  2016-06-14                
            var TempFzh = GetPointFzh(request.CurrentPointID);

            if (request.kglztjsfs) //为0时查询 分站中断及系统退出 记录  20140428
                Sz = "select id,fzh,kh,timer,type,val from " + SzTable + " where timer<='" + DtEnd + "' and timer>='" +
                     DtStart
                     + "' and ((PointID='" + request.CurrentPointID + "') or(fzh=0 and kh=0)or (fzh=" + TempFzh +
                     " and type=0))  order by timer";
            else //为1时 不查询 分站中断及系统退出 记录  20140428
                Sz = "select id,fzh,kh,timer,type,val from " + SzTable + " where timer<='" + DtEnd + "' and timer>='" +
                     DtStart
                     + "' and ((PointID='" + request.CurrentPointID + "'))  order by timer,id";
            //Dt_ = GetDataTableBySQL(Sz);
            Dt_ = _listexRepositoryBase.QueryTableBySql(Sz);

            if (request.kglztjsfs) //为0时查询 分站中断及系统退出 记录  20140428
                Sz = "select  id,fzh,kh,timer,type,val from " + SzTable + " where timer <'" + DtEnd
                     + "' and ((PointID='" + request.CurrentPointID + "')or(fzh=0 and kh=0)or (fzh=" + TempFzh
                     + " and type=0))  order by timer DESC,id DESC";
            else //为1时 不查询 分站中断及系统退出 记录  20140428
                Sz = "select id,fzh,kh,timer,type,val from " + SzTable + " where timer <'" + DtEnd + "' and ((PointID='" +
                     request.CurrentPointID
                     + "'))  order by timer DESC,id DESC";
            //Dda_ = GetDataTableBySQL(Sz);
            Dda_ = _listexRepositoryBase.QueryTableBySql(Sz);

            try
            {
                var Startime = new DateTime(); //本次状态开始时间
                var Endtime = new DateTime(); //本次状态结束时间(分)
                for (var i = 0; i <= 23; i++)
                {
                    Startime = Convert.ToDateTime(request.SzNameT.ToShortDateString());
                    Endtime = Startime;
                    Startime = Startime.AddHours(i);
                    Startime = Startime.AddMinutes(0);
                    Endtime = Endtime.AddHours(i);
                    Endtime = Endtime.AddMinutes(60);
                    QueryStr = GetKjThings(Startime, Endtime, request.CurrentPointID, request.CurrentDevid, request.CurrentWzid, request.kglztjsfs, Dt_,
                        Dda_);
                    obj = new object[dtR.Columns.Count];
                    obj[0] = QueryStr[4].Substring(0, QueryStr[4].IndexOf("%"));
                    obj[1] = QueryStr[5];
                    obj[2] = QueryStr[6];
                    obj[3] = Startime.ToString("yyyy-MM-dd HH:mm:ss");
                    dtR.Rows.Add(obj);
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_InitQxZhuZhuang" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dtR
            };
            return ret;
        }

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
        public BasicResponse<Dictionary<string, DataTable>> GetMcData(GetMcDataRequest request)
        {
            var Rvalue = new Dictionary<string, DataTable>();
            double MaxLC2 = -9999;
            double MinLC2 = 9999;
            string maxValueTime = "";
            var dtR = new DataTable();
            dtR.TableName = "GetMcData";
            var strSql = "";
            string SztB;

            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("A");
            dtR.Columns.Add("B");
            dtR.Columns.Add("Timer");
            dtR.Columns.Add("state");
            dtR.Columns.Add("statetext");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");
            try
            {
                for (var SzNameT = request.SzNameS;
                    DateTime.Parse(SzNameT.ToShortDateString()) <= DateTime.Parse(request.SzNameE.ToShortDateString());
                    SzNameT = SzNameT.AddDays(1))
                {
                    SztB = "KJ_DataDetail" + SzNameT.Year + SzNameT.Month.ToString("00") + SzNameT.Day.ToString("00");
                    DateTime Stime, Etime;
                    if (SzNameT.ToShortDateString() == request.SzNameS.ToShortDateString())
                        Stime = request.SzNameS;
                    else
                        Stime = DateTime.Parse(SzNameT.ToShortDateString() + " 00:00:00");
                    if (SzNameT.ToShortDateString() == request.SzNameE.ToShortDateString())
                        Etime = request.SzNameE;
                    else
                        Etime = DateTime.Parse(SzNameT.ToShortDateString() + " 23:59:59");

                    //检查表是否存在                   
                    if (GetDBType().Data.ToLower() == "sqlserver")
                        strSql = "select name from dbo.sysobjects where id = object_id(N'[dbo].[" + SztB +
                                 "]')and OBJECTPROPERTY(id, N'IsUserTable') = 1";
                    else if (GetDBType().Data.ToLower() == "mysql")
                        strSql = "SHOW TABLES LIKE  '" + SztB + "';";

                    var pointfzh = GetPointFzh(request.CurrentPointID);

                    //var tempdtIsExit = GetDataTableBySQL(strSql);
                    var tempdtIsExit = _listexRepositoryBase.QueryTableBySql(strSql);

                    if (tempdtIsExit.Rows.Count > 0)
                    {
                        string PointConditions = "";

                        if (request.IsQueryByPoint)//增加只按测点号查询处理  20180227
                        {
                            string PointNow = "";
                            var dt = _jc_DefRepository.QueryTable("global_ChartService_GetJcdef_ByPointId", request.CurrentPointID);
                            if (dt.Rows.Count > 0)
                                PointNow = dt.Rows[0]["point"].ToString();

                            PointConditions = " Point = '" + PointNow + "'";
                        }
                        else
                        {
                            PointConditions = " PointID = " + request.CurrentPointID;
                        }

                        if (request.TimeTick == "密采值")
                        {
                            //如果是通过双击日志信息弹出该界面的 2014-1-2 huangxxUP
                            if (request.flag)
                                strSql = "select fzh,kh,ssz,timer," + SztB + ".state," + SztB + ".type from " + SztB +
                                         " left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + ".devid "
                                         + " where ((" + PointConditions + ") ) and timer>='" + Stime + "' and timer<='" + Etime +
                                         "' order by timer";
                                //or (fzh=0 and kh=0) or (fzh=" +
                                         //pointfzh + " and kh=0)
                            else
                                strSql = "select fzh,kh,ssz,timer," + SztB + ".state," + SztB + ".type from " + SztB +
                                         " left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + ".devid "
                                         + " where ((" + PointConditions + ") ) and timer>='" + Stime + "' and timer<='" + Etime +
                                         "' order by " + SztB + ".timer," + SztB + ".id";
                            //or (fzh=0 and kh=0) or (fzh=" +
                                         //pointfzh + " and kh=0)
                        }
                        else if (request.TimeTick == "1分钟")
                        {
                            if (GetDBType().Data.ToLower() == "sqlserver")
                                strSql =
                                    "select fzh,kh, ltrim(str(avg(ssz),len(avg(ssz)),4)) as ssz ,substring(convert(nvarchar,timer,20),1,16) as timer," +
                                    SztB + ".type," + SztB + ".state from " +
                                    SztB + " left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + ".devid  "
                                    + " where ((" + PointConditions + ") ) and timer>='" + Stime + "' and timer<='" + Etime +
                                    "' group by fzh,kh,substring(convert(nvarchar,timer,20),1,16) order by timer";
                                //or (fzh=0 and kh=0)or (fzh=" + pointfzh +
                                    //" and kh=0)
                            else if (GetDBType().Data.ToLower() == "mysql")
                                strSql = @"select fzh,kh, CAST(AVG(ssz) AS CHAR) as ssz ,
SUBSTRING(date_format(timer,'%Y-%m-%d %H:%i:%s'),1,16) as timer," + SztB + ".state," + SztB + ".type from " + SztB + @" 
left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + @".devid"
                                         + " where ((" + PointConditions +  @" ) ) and timer>='" + Stime + "' and timer<='" + Etime + @"'
 group by fzh,kh,SUBSTRING(date_format(timer,'%Y-%m-%d %H:%i:%s'),1,16) order by timer";
                            //or (fzh=0 and kh=0)or (fzh=" +
                                        // pointfzh + @" and kh=0)
                        }
                        else if (request.TimeTick == "1小时")
                        {
                            if (GetDBType().Data.ToLower() == "sqlserver")
                                strSql =
                                    "select fzh,kh, ltrim(str(avg(ssz),len(avg(ssz)),4)) as ssz ,substring(convert(nvarchar,timer,20),1,13) as timer," +
                                    SztB + ".state," + SztB + ".type from "
                                    + SztB + " left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + ".devid  "
                                    + " where ((PointID=" + PointConditions + " ) ) and timer>='" + Stime + "' and timer<='" + Etime +
                                    @"'  group by fzh,kh,substring(convert(nvarchar,timer,20),1,13) order by timer";
                                //or (fzh=0 and kh=0)or (fzh=" +
                                    //pointfzh +
                                    //" and kh=0)
                            else if (GetDBType().Data.ToLower() == "mysql")
                                strSql = @"select fzh,kh, CAST(AVG(ssz) AS CHAR) as ssz ,
SUBSTRING(date_format(timer,'%Y-%m-%d %H:%i:%s'),1,13) as timer," + SztB + ".state," + SztB + ".type from " + SztB + @" 
left outer join KJ_DeviceType on KJ_DeviceType.devid=" + SztB + @".devid"
                                         + " where ((" + PointConditions + " ) ) and timer>='" + Stime + "' and timer<='" + Etime + @"'
 group by fzh,kh,SUBSTRING(date_format(timer,'%Y-%m-%d %H:%i:%s'),1,13) order by timer";
                            //or (fzh=0 and kh=0)or (fzh=" +
                                         //pointfzh + @" and kh=0)
                        }
                    }

                    var DtDay = new DataTable();


                    //DtDay = GetDataTableBySQL(strSql);
                    DtDay = _listexRepositoryBase.QueryTableBySql(strSql);

                    double dCur = 0, Sumfl = 0, Dpjz = 0;
                    int Dindex = 0, Fcount = 0;
                    string SzCur = "0", SzPjz = "0";
                    int Lc1 = 0, Lc2 = 0;
                    var tempList = new List<int>();

                    #region 获取当前设备的量程

                    tempList = GetLcFromTable(request.CurrentDevid);

                    if (tempList.Count > 0)
                    {
                        Lc1 = tempList[0];
                        Lc2 = tempList[1];
                    }

                    #endregion

                    for (var i = 0; i < DtDay.Rows.Count; i++)
                    {
                        if (((DtDay.Rows[i]["fzh"].ToString() == "0") && (DtDay.Rows[i]["kh"].ToString() == "0"))
                            ||
                            ((DtDay.Rows[i]["fzh"].ToString() == pointfzh) &&
                             (DtDay.Rows[i]["kh"].ToString() == "0"))
                            || (DtDay.Rows[i]["type"].ToString() == "20")
                            || (DtDay.Rows[i]["type"].ToString() == "22")
                            || (DtDay.Rows[i]["type"].ToString() == "23")
                            || (DtDay.Rows[i]["type"].ToString() == "33")
                            || (DtDay.Rows[i]["type"].ToString() == "34")
                            || (DtDay.Rows[i]["type"].ToString() == "46"))
                        {
                            if ((DtDay.Rows[i]["fzh"].ToString() == "0") && (DtDay.Rows[i]["kh"].ToString() == "0"))
                                SzCur = "0.00001"; //系统退出
                            else if ((DtDay.Rows[i]["fzh"].ToString() == pointfzh) &&
                                     (DtDay.Rows[i]["kh"].ToString() == "0"))
                                SzCur = "0.00002"; //分站中断
                            else if (DtDay.Rows[i]["type"].ToString() == "20")
                                SzCur = "0.00003"; //断线
                            else if (DtDay.Rows[i]["type"].ToString() == "22")
                                SzCur = "0.00004"; //上溢
                            else if (DtDay.Rows[i]["type"].ToString() == "23")
                                SzCur = "0.00005"; //负漂
                            else if (DtDay.Rows[i]["type"].ToString() == "33")
                                SzCur = "0.00006"; //头子断线
                            else if (DtDay.Rows[i]["type"].ToString() == "34")
                                SzCur = "0.00007"; //类型有误
                            else if (DtDay.Rows[i]["type"].ToString() == "46")
                                SzCur = "0.00008"; //未知
                            Dindex++;
                        }
                        else
                        {
                            if (DtDay.Rows[i]["ssz"].ToString() == "")
                                dCur = 0;
                            else
                                dCur = Convert.ToDouble(DtDay.Rows[i]["ssz"]);
                            if (dCur == 0)
                            {
                                SzCur = "0";
                            }
                            else
                            {
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        SzCur = string.Format("{0:F1}", dCur);
                                //    else
                                //        SzCur = string.Format("{0:F2}", dCur);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        SzCur = string.Format("{0:F0}", dCur);
                                //    else
                                //        SzCur = string.Format("{0:F1}", dCur);
                                //else
                                //    SzCur = string.Format("{0:F0}", dCur);
                                //不处理数据  20180109
                                SzCur = string.Format("{0:F2}", dCur);
                            }
                            //修改，在此处计算最大值最小值  20180122
                            if (dCur > MaxLC2)
                            {
                                MaxLC2 = dCur;
                                maxValueTime = DtDay.Rows[i]["timer"].ToString();
                            }
                            if (dCur < MinLC2)
                                MinLC2 = dCur;
                        }

                        if ((DtDay.Rows[i]["kh"].ToString() != "0") && (Convert.ToDouble(DtDay.Rows[i]["ssz"]) >= 0))
                        {
                            Fcount++;
                            Sumfl += dCur;
                            Dpjz = Sumfl / Fcount;

                            SzPjz = string.Format("{0:F2}", Dpjz);

                            if (Dpjz > MaxLC2)
                            {
                                MaxLC2 = Dpjz;
                                maxValueTime = DtDay.Rows[i]["timer"].ToString();
                            }
                            if (Dpjz < MinLC2)
                                MinLC2 = Dpjz;
                        }
                        //A.Append("A[" + (Count - 1) + "]=\"" + SzCur + "\";");
                        //B.Append("B[" + (Count - 1) + "]=\"" + SzPjz + "\";");
                        //C.Append("C[" + (Count - 1) + "]=\"" + DtDay.Rows[i]["timer"].ToString() + "\";");
                        var obj = new object[dtR.Columns.Count];
                        obj[0] = SzCur;
                        obj[1] = SzPjz;

                        // 20170628
                        if (request.TimeTick == "1分钟")
                            //obj[2] = DateTime.Parse(DtDay.Rows[i]["timer"] + ":00").ToString();
                            obj[2] = DateTime.Parse(DtDay.Rows[i]["timer"] + ":00").ToString("yyyy-MM-dd HH:mm:ss.fff");

                        else if (request.TimeTick == "1小时")
                            //obj[2] = DateTime.Parse(DtDay.Rows[i]["timer"] + ":00:00").ToString();
                            obj[2] = DateTime.Parse(DtDay.Rows[i]["timer"] + ":00:00").ToString("yyyy-MM-dd HH:mm:ss.fff");
                        else
                            //obj[2] = DtDay.Rows[i]["timer"].ToString();
                            obj[2] = Convert.ToDateTime(DtDay.Rows[i]["timer"]).ToString("yyyy-MM-dd HH:mm:ss.fff");

                        obj[3] = DtDay.Rows[i]["state"].ToString(); //DtDay.Rows[i]["statetext"].ToString();
                        obj[4] = StateChange(DtDay.Rows[i]["state"].ToString());
                        obj[5] = DtDay.Rows[i]["type"].ToString();
                        obj[6] = StateChange(DtDay.Rows[i]["type"].ToString());
                        dtR.Rows.Add(obj);
                    }
                    if (DtDay.Rows.Count > 0)
                    {
                        var obj = new object[dtR.Columns.Count];
                        obj[0] = SzCur;
                        obj[1] = SzPjz;
                        // 20170628
                        if (request.flag)
                        {
                            //obj[2] = Etime.ToString();
                            obj[2] = Etime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        }
                        else
                        {
                            if (DateTime.Parse(Stime.ToShortDateString()) ==
                                DateTime.Parse(GetCenterDateTime().ToShortDateString()))
                                //obj[2] = GetCenterDateTime().ToString();
                                obj[2] = GetCenterDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            else
                                //obj[2] = Stime.ToShortDateString() + " 23:59:59";
                                obj[2] = Stime.ToString("yyyy-MM-dd") + " 23:59:59.000";

                        }
                        //obj[3] = ""; //DtDay.Rows[DtDay.Rows.Count-1]["statetext"].ToString();
                        //obj[4] = DtDay.Rows[DtDay.Rows.Count - 1]["type"].ToString();
                        //obj[5] = "";//DtDay.Rows[DtDay.Rows.Count - 1]["typetext"].ToString();
                        obj[3] = DtDay.Rows[DtDay.Rows.Count - 1]["state"].ToString();
                        //DtDay.Rows[i]["statetext"].ToString();
                        obj[4] = StateChange(DtDay.Rows[DtDay.Rows.Count - 1]["state"].ToString());
                        obj[5] = DtDay.Rows[DtDay.Rows.Count - 1]["type"].ToString();
                        obj[6] = StateChange(DtDay.Rows[DtDay.Rows.Count - 1]["type"].ToString());
                        dtR.Rows.Add(obj);
                    }
                    DtDay.Dispose();
                }
                var RString = "";
                RString = MaxLC2 + "," + MinLC2 + "," + maxValueTime;
                Rvalue.Add(RString, dtR);
            }
            catch (Exception Ex)
            {
                LogHelper.Error("McLineQueryClass_GetMcData" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<Dictionary<string, DataTable>>
            {
                Data = Rvalue
            };
            return ret;
        }

        /// <summary>
        ///     模拟量月柱状图数据
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMonthBar(GetMonthBarRequest request)
        {
            var SaveTable = new DataTable();
            SaveTable.TableName = "getMonthBar";
            SaveTable.Columns.Add("zdz", Type.GetType("System.String"));
            SaveTable.Columns.Add("pjz", Type.GetType("System.String"));
            SaveTable.Columns.Add("zxz", Type.GetType("System.String"));
            SaveTable.Columns.Add("dday", typeof(DateTime));
            var strsql = "";
            var Lc1 = 0;
            var Lc2 = 0;
            SaveTable.Rows.Clear();
            var dtzzt = new DataTable();
            float zdz = 0, zxz = 9999;
            var Dt = new DataTable();
            var tempList = new List<int>();
            var tempDate = new DateTime();
            try
            {
                #region 获取当前设备的量程

                tempList = GetLcFromTable(request.CurrentDevid);
                if (tempList.Count > 0)
                {
                    Lc1 = tempList[0];
                    Lc2 = tempList[1];
                }

                #endregion

                //检查表是否存在
                if (GetDBType().Data.ToLower() == "sqlserver")
                    strsql = "select name from sysobjects where name like 'KJ_StaFiveMinute" + request.year + request.month.ToString("00") + "__'";
                else if (GetDBType().Data.ToLower() == "mysql")
                    strsql = "SHOW TABLES LIKE  'KJ_StaFiveMinute" + request.year + request.month.ToString("00") + "__';";


                //var dtzztMinMax = GetDataTableBySQL(strsql);
                var dtzztMinMax = _listexRepositoryBase.QueryTableBySql(strsql);
                for (var i = 0; i < dtzztMinMax.Rows.Count; i++)
                {
                    tempDate =
                        DateTime.Parse(dtzztMinMax.Rows[i][0].ToString().Substring(16, 4) + "-" +
                                       dtzztMinMax.Rows[i][0].ToString().Substring(20, 2)
                                       + "-" + dtzztMinMax.Rows[i][0].ToString().Substring(22, 2));

                    strsql = "select max(zdz)as zdz,avg(pjz)as pjz,min(zxz) as zxz,'" + tempDate + "' as dday from " +
                             dtzztMinMax.Rows[i][0]
                             + " where PointID=" + request.CurrentPointID;

                    dtzzt.Rows.Clear();
                    //dtzzt = GetDataTableBySQL(strsql);
                    dtzzt = _listexRepositoryBase.QueryTableBySql(strsql);
                    if (dtzzt.Rows.Count > 0)
                    {
                        var dr = SaveTable.NewRow();
                        if (dtzzt.Rows[0]["zdz"].ToString() == "")
                            dr["zdz"] = "0";
                        else
                        {
                            if (Convert.ToSingle(dtzzt.Rows[0]["zdz"]) < 0)
                                dr["zdz"] = 0;
                            else
                            {
                                double Dpjz = Convert.ToSingle(dtzzt.Rows[0]["zdz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        dr["zdz"] = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        dr["zdz"] = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        dr["zdz"] = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        dr["zdz"] = string.Format("{0:F1}", Dpjz);
                                //else
                                //    dr["zdz"] = string.Format("{0:F0}", Dpjz);
                                //不进行数据处理  20180109
                                dr["zdz"] = string.Format("{0:F2}", Dpjz);
                            }
                        }

                        if (dtzzt.Rows[0]["zxz"].ToString() == "")
                            dr["zxz"] = "0";
                        else
                        {
                            if (Convert.ToSingle(dtzzt.Rows[0]["zxz"]) < 0)
                                dr["zxz"] = 0;
                            else
                            {
                                double Dpjz = Convert.ToSingle(dtzzt.Rows[0]["zxz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        dr["zxz"] = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        dr["zxz"] = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        dr["zxz"] = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        dr["zxz"] = string.Format("{0:F1}", Dpjz);
                                //else
                                //    dr["zxz"] = string.Format("{0:F0}", Dpjz);
                                //不进行数据处理  20180109
                                dr["zxz"] = string.Format("{0:F2}", Dpjz);
                            }
                        }

                        if (dtzzt.Rows[0]["pjz"].ToString() == "")
                            dr["pjz"] = "0";
                        else
                        {
                            if (Convert.ToSingle(dtzzt.Rows[0]["pjz"]) < 0)
                                dr["pjz"] = 0;
                            else
                            {
                                double Dpjz = Convert.ToSingle(dtzzt.Rows[0]["pjz"]);
                                //if ((Lc1 <= 10) || ((Lc2 != 0) && (Lc2 <= 10)))
                                //    if (Dpjz > 9.995)
                                //        dr["pjz"] = string.Format("{0:F1}", Dpjz);
                                //    else
                                //        dr["pjz"] = string.Format("{0:F2}", Dpjz);
                                //else if ((Lc1 <= 100) || ((Lc2 != 0) && (Lc2 <= 100)))
                                //    if (Dpjz > 99.995)
                                //        dr["pjz"] = string.Format("{0:F0}", Dpjz);
                                //    else
                                //        dr["pjz"] = string.Format("{0:F1}", Dpjz);
                                //else
                                //    dr["pjz"] = string.Format("{0:F0}", Dpjz);
                                //不进行数据处理  20180109
                                dr["pjz"] = string.Format("{0:F2}", Dpjz);
                            }
                        }
                        dr["dday"] = dtzzt.Rows[0]["dday"].ToString();
                        SaveTable.Rows.Add(dr);
                        if (Convert.ToSingle(dr["zdz"]) > zdz)
                            zdz = Convert.ToSingle(dr["zdz"].ToString());
                        if (Convert.ToSingle(dr["zxz"]) < zxz)
                            zxz = Convert.ToSingle(dr["zxz"].ToString());
                    }
                }
                if (zxz == 9999)
                    zxz = 0;
                var isExit = false;
                var stime = DateTime.Parse(request.year + "-" + request.month + "-" + "1");
                var etime = stime.AddMonths(1).AddDays(-1);
                for (var ttime = stime; ttime <= etime; ttime = ttime.AddDays(1))
                {
                    isExit = false;
                    for (var j = 0; j < SaveTable.Rows.Count; j++)
                        if (DateTime.Parse(SaveTable.Rows[j]["dday"].ToString()).Day == ttime.Day)
                            isExit = true;
                    if (!isExit)
                    {
                        var obj = new string[SaveTable.Columns.Count];
                        obj[0] = "0.0";
                        obj[1] = "0.0";
                        obj[2] = "0.0";
                        obj[3] = ttime.ToString("yyyy-MM-dd HH:mm:ss");
                        SaveTable.Rows.Add(obj);
                    }
                }
                //panghbUP 2014-6-17  模拟量柱状图按日排序  影响了放大缩小左右移动功能
                SaveTable.DefaultView.Sort = "dday ASC";
                SaveTable = SaveTable.DefaultView.ToTable();
            }
            catch (Exception Ex)
            {
                LogHelper.Error("MthBarAndLineQueryClass_getMonthBar" + Ex.Message + Ex.StackTrace);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = SaveTable
            };
            return ret;
        }

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
        public BasicResponse<DataTable> GetMLLFiveLine(GetMLLFiveLineRequest request)
        {
            DataTable dtCopy = null;
            try
            {
                #region old

                //string strsql = " ";
                //foreach (string strDate in _listdate)
                //{
                //    strsql += " select ID,PointID,zdz,zxz,pjz,ssz,timer from KJ_StaFiveMinute" + strDate + " where timer between '" + datStart + "' and '" + datEnd + "' and PointID=" + PointID + " union all";
                //}
                //if (strsql.Contains("union all"))
                //    strsql = strsql.Substring(0, strsql.Length - 9);
                //DataTable dt = this.GetDataTableBySQL(strsql);

                //dtCopy = dt.Copy();
                //DataView dv = dt.DefaultView;
                //dv.Sort = "PointID,timer";
                //dtCopy = dv.ToTable();

                #endregion

                dtCopy = new DataTable();
                dtCopy.TableName = "GetMLLFiveLine";
                dtCopy.Columns.Add("ID");
                dtCopy.Columns.Add("PointID");
                dtCopy.Columns.Add("zdz");
                dtCopy.Columns.Add("zxz");
                dtCopy.Columns.Add("pjz");
                dtCopy.Columns.Add("ssz");
                dtCopy.Columns.Add("ydz"); //移动值，新增
                dtCopy.Columns.Add("type"); //状态值
                dtCopy.Columns.Add("typetext"); //状态描述
                dtCopy.Columns.Add("timer");

                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                var req = new GetFiveMiniteLineRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = GetFiveMiniteLine(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var dt = res.Data;

                //转换数据为WEB需要的格式返回 
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var obj = new object[dtCopy.Columns.Count];
                    obj[0] = (i + 1).ToString();
                    obj[1] = CurrentPointID;
                    //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
                    obj[2] = dt.Rows[i]["Bv"].ToString();
                    obj[3] = dt.Rows[i]["Cv"].ToString();
                    obj[4] = dt.Rows[i]["Dv"].ToString();
                    obj[5] = dt.Rows[i]["Av"].ToString();
                    obj[6] = dt.Rows[i]["Ev"].ToString();
                    obj[7] = dt.Rows[i]["type"].ToString();
                    obj[8] = dt.Rows[i]["typetext"].ToString();
                    obj[9] = dt.Rows[i]["Timer"].ToString();
                    dtCopy.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量5分钟曲线数据失败，原因为" + ex);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dtCopy
            };
            return ret;
        }
        /// <summary>
        /// 得到模拟量小时曲线数据
        /// </summary>        
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMHourLine(GetMHourLineRequest request)
        {
            DataTable dt = new DataTable();
            dt.TableName = "GetMHourLine";
            var SzNameS = DateTime.Parse(request.datStart);
            var SzNameE = DateTime.Parse(request.datEnd);
            var CurrentPointID = request.PointID.ToString();
            var DevAWzID = GetPointDevAndWzID(CurrentPointID);
            var CurrentDevid = DevAWzID[0];
            var CurrentWzid = DevAWzID[1];

            //dt = GetMonthLine(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid);
            var req = new GetMonthLineRequest
            {
                SzNameS = SzNameS,
                SzNameE = SzNameE,
                CurrentPointID = CurrentPointID,
                CurrentDevid = CurrentDevid,
                CurrentWzid = CurrentWzid
            };
            var res = GetMonthLine(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            dt = res.Data;

            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     得到5分钟模拟量曲线grid数据
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMLLFiveGirdData(GetMLLFiveGirdDataRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                //                //得到这一时刻5分钟表的记录
                //                string strJCMTable = "KJ_StaFiveMinute" + Convert.ToDateTime(dattime).ToString("yyyyMMdd");
                //                string strsqlKJ_StaFiveMinute = " select " + strJCMTable + ".ID,KJ_DeviceDefInfo.PointID,zdz,zxz,pjz,KJ_DeviceAddress.wz as wzname,KJ_DeviceType.name as devname,"
                //                    + strJCMTable + ".ssz,timer," + strJCMTable + @".state,KJ_DeviceDefInfo.z1,KJ_DeviceDefInfo.z2,KJ_DeviceDefInfo.z3,KJ_DeviceDefInfo.z4,KJ_DeviceDefInfo.z5,KJ_DeviceDefInfo.z6,KJ_DeviceDefInfo.z7,KJ_DeviceDefInfo.z8,BFT_EnumCode.strEnumDisplay  as sbstate
                //,BFT_EnumCode1.strEnumDisplay as datastate,'' as bj,'' as dd,'' as ddwz,'' as kd,'' as js 
                //from KJ_DeviceDefInfo left join " + strJCMTable + "  on KJ_DeviceDefInfo.PointID=" + strJCMTable +
                //                        ".PointID  left  join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + strJCMTable +
                //                        ".state and BFT_EnumCode.EnumTypeID=8 left  join BFT_EnumCode as BFT_EnumCode1 on BFT_EnumCode1.lngEnumValue=" + strJCMTable
                //                        + ".type and BFT_EnumCode1.EnumTypeID=4 left join KJ_DeviceType on KJ_DeviceDefInfo.devid=KJ_DeviceType.devid left join KJ_DeviceAddress on KJ_DeviceDefInfo.wzid=KJ_DeviceAddress.wzid where timer = '"
                //                        + dattime + "' and " + strJCMTable + ".PointID=" + PointID;
                //                dt = this.GetDataTableBySQL(strsqlKJ_StaFiveMinute);

                //                //得到这一时刻到这一时刻加5分钟这段时间内b表里有没有报警和断电的记录，同时按照kzk展开，以得到断电区域
                //                string strJCBTable = "KJ_DataAlarm" + Convert.ToDateTime(dattime).ToString("yyyyMM");
                //                string strddsql = @"select " + strJCBTable + ".id," + strJCBTable + ".pointid,type,kzk,kdid,KJ_DeviceAddress.wz  from " + strJCBTable + " left join KJ_DeviceDefInfo on (locate(concat(',', `KJ_DeviceDefInfo`.`point`, ','),	concat(	',',REPLACE (cast(" + strJCBTable + ".kzk AS CHAR charset utf8),'|',','),',')	) > 0)  	LEFT JOIN `KJ_DeviceAddress` ON ((`KJ_DeviceDefInfo`.`wzid` = `KJ_DeviceAddress`.`WZID`	)) where (type=10 or type=12 or type=16 or type=18 or type=20) and " + strJCBTable + ".PointID=" + PointID + " and (stime between '" + dattime + "' and '" + Convert.ToDateTime(dattime).AddMinutes(5).ToString() + "' or etime between '" + dattime + "' and '" + Convert.ToDateTime(dattime).AddMinutes(5).ToString() + "')";
                //                DataTable dtdd = this.GetDataTableBySQL(strddsql);

                //                string strkdids = "";
                //                DataTable dtDataState = null;
                //                foreach (DataRow row in dtdd.Rows)
                //                {
                //                    if (dtDataState == null) dtDataState = this.GetDataState();
                //                    int TypeID = Convert.ToInt32(row["type"]);

                //                    DataRow[] rows = dtDataState.Select("lngEnumValue=" + TypeID);
                //                    if (rows.Length > 0)
                //                    {
                //                        string strtext = Convert.ToString(rows[0]["strEnumDisplay"]);

                //                        if (TypeID == 10 || TypeID == 16)
                //                        {
                //                            DataRow[] rowdt = dt.Select("bj like '%" + strtext + "%'");
                //                            if (rowdt.Length == 0) dt.Rows[0]["bj"] = strtext;
                //                        }
                //                        if (TypeID == 12 || TypeID == 18)
                //                        {
                //                            DataRow[] rowdt = dt.Select("dd like '%" + strtext + "%'");
                //                            if (rowdt.Length == 0) dt.Rows[0]["dd"] = strtext;
                //                        }
                //                    }

                //                    string strwz = Convert.ToString(row["wz"]);
                //                    dt.Rows[0]["ddwz"] += strwz + "|";

                //                    string strkdid = Convert.ToString(row["kdid"]);
                //                    if (strkdid.Length > 0)
                //                        strkdids += strkdid + ",";
                //                }

                //                //得到馈电相关数据
                //                if (strkdids.Length > 0)
                //                {
                //                    strkdids = strkdids.Substring(0, strkdids.Length - 1);
                //                    string strkdsql = "select * from " + strJCBTable + " where id in(" + strkdids + ")";
                //                    DataTable dtkd = this.GetDataTableBySQL(strkdsql);
                //                    foreach (DataRow row in dtkd.Rows)
                //                    {
                //                        if (dtDataState == null) dtDataState = this.GetDataState();
                //                        int TypeID = Convert.ToInt32(row["type"]);
                //                        DataRow[] rows = dtDataState.Select("lngEnumValue=" + TypeID);
                //                        string strtext = Convert.ToString(rows[0]["strEnumDisplay"]);
                //                        dt.Rows[0]["kd"] += strtext + "|";
                //                        dt.Rows[0]["js"] += Convert.ToString(row["cs"]) + "  ";


                //                    }

                //                }

                #endregion

                dt = new DataTable();
                dt.TableName = "GetMLLFiveGirdData";
                dt.Columns.Add("wzname"); //名称及类型
                dt.Columns.Add("z2"); //报警值
                dt.Columns.Add("z3"); //断电值
                dt.Columns.Add("z4"); //复电值
                dt.Columns.Add("sbstate"); //设备状态
                dt.Columns.Add("ddwz"); //断电范围
                dt.Columns.Add("timer"); //读值时刻
                dt.Columns.Add("ssz"); //监测值
                dt.Columns.Add("zdz"); //最大值
                dt.Columns.Add("pjz"); //平均值
                dt.Columns.Add("bj"); //报警/解除
                dt.Columns.Add("dd"); //断电/复电
                dt.Columns.Add("kd"); //馈电状态
                dt.Columns.Add("js"); //措施及时刻

                var obj = new object[dt.Columns.Count];

                var Time = DateTime.Parse(request.dattime);
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];

                #region//读取测点基础信息

                var tempPointInf = new string[14];
                var req = new ShowPointInfRequest
                {
                    CurrentWzid = CurrentWzid,
                    CurrentPointId = CurrentPointID
                };
                var res = ShowPointInf(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                tempPointInf = res.Data;

                obj[0] = tempPointInf[0]; //
                obj[1] = tempPointInf[1];
                obj[2] = tempPointInf[2];
                obj[3] = tempPointInf[3];

                #endregion

                #region 读取当前时间的最大值，平均值，值，时间，及设备状态

                var szDate = request.dattime;

                var QxDate = Convert.ToDateTime(szDate);
                var DtStart = Convert.ToDateTime(QxDate.ToShortDateString());

                var DtEnd = Convert.ToDateTime(QxDate.ToShortDateString());

                var Ihour = QxDate.Hour;
                var SMin = QxDate.Minute;
                obj[6] = QxDate.Hour + ":" + QxDate.Minute + ":" + QxDate.Second;
                obj[5] = "";
                var Iminite = QxDate.Minute % 10;

                if (Iminite > 4)
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 5;
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 9;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                else
                {
                    DtStart = DtStart.AddHours(Ihour);
                    var Ival = 0;
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtStart = DtStart.AddMinutes(Ival);

                    Ival = 4;
                    DtEnd = DtEnd.AddHours(Ihour);
                    if (SMin / 10 != 0)
                        Ival = Ival + SMin / 10 * 10;
                    DtEnd = DtEnd.AddMinutes(Ival);
                    DtEnd = DtEnd.AddSeconds(59);
                }
                var req2 = new GetDataValeRequest
                {
                    QxDate = QxDate,
                    DtStart = DtStart,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                tempPointInf = GetDataVale(req2).Data;
                obj[7] = tempPointInf[6];
                obj[8] = tempPointInf[7];
                obj[9] = tempPointInf[8];
                obj[4] = tempPointInf[13]; //设备状态

                #endregion

                #region 读取报警、断电、断电范围、馈电异常状态

                var req3 = new GetValueRequest
                {
                    QxDate = QxDate,
                    DtStart = DtStart,
                    DtEnd = DtEnd,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                tempPointInf = GetValue(req3).Data;
                if (obj[7] == "未记录")
                {
                    obj[5] = "";
                    obj[10] = "解除";
                    obj[11] = "复电";
                    obj[12] = "正常";
                    obj[13] = "无";
                }
                else
                {
                    obj[5] = tempPointInf[4];
                    obj[10] = tempPointInf[9];
                    obj[11] = tempPointInf[10];
                    obj[12] = tempPointInf[11];
                    obj[13] = tempPointInf[12];
                }

                #endregion

                dt.Rows.Add(obj);
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量5分钟grid数据失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     得到模拟量月柱状图数据
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMLLMonthBar(GetMLLMonthBarRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                //string strJCMTable = "KJ_Hour" + Convert.ToDateTime(datStart).ToString("yyyyMM");
                //string strsql = "select PointID,date_format(timer,'%Y-%m-%d') as strdate,max(zdz) as zdz,min(zxz) as zxz,ROUND(avg(pjz),2) as pjz from " 
                //    + strJCMTable + " where PointID=" + PointID + " and timer between '" + datStart + "' and '" + datEnd + 
                //    "' group by PointID,date_format(timer,'%Y-%m-%d') order by Pointid,date_format(timer,'%Y-%m-%d') ";
                //dt = this.GetDataTableBySQL(strsql);

                #endregion

                dt = new DataTable();
                dt.TableName = "GetMLLMonthBar";
                dt.Columns.Add("PointID");
                dt.Columns.Add("zdz");
                dt.Columns.Add("zxz");
                dt.Columns.Add("pjz");
                dt.Columns.Add("strdate");

                var stime = DateTime.Parse(request.datStart);
                var etime = DateTime.Parse(request.datEnd);

                if (stime.Month != etime.Month)
                    throw new Exception("不能跨月查询！");
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];

                var req = new GetMonthBarRequest
                {
                    year = stime.Year,
                    month = stime.Month,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = GetMonthBar(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var dtbar = res.Data;

                //转换数据为WEB需要的格式返回 
                for (var i = 0; i < dtbar.Rows.Count; i++)
                {
                    var obj = new object[dt.Columns.Count];
                    obj[0] = CurrentPointID;
                    obj[1] = dtbar.Rows[i]["zdz"].ToString();
                    obj[2] = dtbar.Rows[i]["zxz"].ToString();
                    obj[3] = dtbar.Rows[i]["pjz"].ToString();
                    obj[4] = dtbar.Rows[i]["dday"].ToString();
                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量月柱状图数据失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }

            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     得到模拟量密采曲线
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="PointID">测点ID</param>
        /// <param name="Type">密采值类型：0：密采值，1：1分钟，2：1小时</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMLLMCLine(GetMLLMCLineRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                var strJCMTable = "KJ_DataDetail" + Convert.ToDateTime(request.datStart).ToString("yyyyMMdd");
                var strsql = "select " + strJCMTable + ".ID,PointID,timer,ssz," + strJCMTable +
                             ".type,state,BFT_EnumCode.strEnumDisplay  as sbstate,KJ_DeviceType.name as devname,KJ_DeviceAddress.wz as wzname from " +
                             strJCMTable + " left  join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + strJCMTable +
                             ".state and BFT_EnumCode.EnumTypeID=8 left join KJ_DeviceType on "
                             + strJCMTable + ".devid=KJ_DeviceType.devid left join KJ_DeviceAddress on " + strJCMTable +
                             ".wzid=KJ_DeviceAddress.wzid where timer between '" + request.datStart
                             + "' and '" + request.datEnd + "' and PointID=" + request.PointID;
                //dt = GetDataTableBySQL(strsql);
                dt = _listexRepositoryBase.QueryTableBySql(strsql);

                #endregion

                dt = new DataTable();
                dt.TableName = "GetMLLMCLine";
                dt.Columns.Add("ID");
                dt.Columns.Add("PointID");
                dt.Columns.Add("timer");
                dt.Columns.Add("ssz");
                dt.Columns.Add("ydz");
                dt.Columns.Add("type");
                dt.Columns.Add("typetext");
                dt.Columns.Add("state");
                dt.Columns.Add("sbstate");
                dt.Columns.Add("devname");
                dt.Columns.Add("wzname");

                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                var DevName = DevAWzID[2];
                var Wz = DevAWzID[3];

                string TypeStr = "密采值";
                switch (request.Type)
                {
                    case 0:
                        TypeStr = "密采值";
                        break;
                    case 1:
                        TypeStr = "1分钟";
                        break;
                    case 2:
                        TypeStr = "1小时";
                        break;
                }

                //var Rvalue = GetMcData(SzNameS, SzNameE, false, CurrentPointID, CurrentDevid, CurrentWzid, TypeStr);
                var req = new GetMcDataRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    flag = false,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    TimeTick = TypeStr
                };
                var res = GetMcData(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var Rvalue = res.Data;

                var value = Rvalue.Values;
                var dtR = value.ElementAt(0);

                //转换数据为WEB需要的格式返回 
                for (var i = 0; i < dtR.Rows.Count; i++)
                {
                    var obj = new object[dt.Columns.Count];
                    obj[0] = (i + 1).ToString();
                    obj[1] = CurrentPointID;
                    obj[2] = dtR.Rows[i]["Timer"].ToString();
                    obj[3] = dtR.Rows[i]["A"].ToString();
                    obj[4] = dtR.Rows[i]["B"].ToString();
                    obj[5] = dtR.Rows[i]["type"].ToString();
                    obj[6] = dtR.Rows[i]["typetext"].ToString();
                    obj[7] = dtR.Rows[i]["state"].ToString();
                    obj[8] = dtR.Rows[i]["statetext"].ToString();
                    obj[9] = DevName;
                    obj[10] = Wz;
                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到密采曲线数据失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     得到开关量曲线及甘特图 情况
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKGLLState(GetKGLLStateRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                //string strJCMTable = "KJ_DataAlarm" + Convert.ToDateTime(datStart).ToString("yyyyMM");
                //string strsql = "select id,pointid,type,ssz,stime,etime from " + strJCMTable + " where ((stime between '" +
                //    datStart + "' and '" + datEnd + "') or (etime between '" + datStart + "' and '" + datEnd + "')) and PointID=" + PointID + "   order by stime";
                //dt = this.GetDataTableBySQL(strsql);

                #endregion

                dt = new DataTable();
                dt.TableName = "GetKGLLState";
                dt.Columns.Add("id");
                dt.Columns.Add("pointid");
                dt.Columns.Add("type"); //状态值
                dt.Columns.Add("ssz"); //状态值汉字
                dt.Columns.Add("stime");
                dt.Columns.Add("etime");
                dt.Columns.Add("kd");
                dt.Columns.Add("cs");

                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 3)
                    throw new Exception("查询的最大天数为3天,请重新选择日期！");
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];

                var dt_line = new DataTable();
                for (var NTime = SzNameS; NTime <= SzNameE; NTime = NTime.AddDays(1))
                {
                    //var tempDt = GetStateLineDt(NTime, CurrentPointID, CurrentDevid, CurrentWzid, request.kglztjsfs);
                    var req = new GetStateLineDtRequest
                    {
                        SzNameT = NTime,
                        CurrentPointID = CurrentPointID,
                        CurrentDevid = CurrentDevid,
                        CurrentWzid = CurrentWzid,
                        kglztjsfs = request.kglztjsfs
                    };
                    var res = GetStateLineDt(req);
                    var tempDt = res.Data;

                    if (dt_line.Columns.Count < 1)
                        dt_line = tempDt.Clone();
                    foreach (DataRow dr in tempDt.Rows)
                        dt_line.Rows.Add(dr.ItemArray);
                }

                //转换数据为WEB需要的格式返回 
                for (var i = 0; i < dt_line.Rows.Count; i++)
                {
                    var obj = new object[dt.Columns.Count];
                    obj[0] = (i + 1).ToString();
                    obj[1] = CurrentPointID;
                    obj[2] = dt_line.Rows[i]["C"].ToString();
                    obj[3] = dt_line.Rows[i]["stateName"].ToString();
                    obj[4] = dt_line.Rows[i]["sTimer"].ToString();
                    obj[5] = dt_line.Rows[i]["eTimer"].ToString();
                    obj[6] = dt_line.Rows[i]["D"].ToString();
                    obj[7] = dt_line.Rows[i]["E"].ToString();
                    dt.Rows.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到开关量状动情况失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     得到开关量状态图Grid数据
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKGLStateGirdData(GetKGLStateGirdDataRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                //                //得到时间段，其实就是当时点击时间的一小时
                //                string strsq = Convert.ToDateTime(dattime).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(dattime).Hour + ":00" + "~" + (Convert.ToDateTime(dattime).Hour + 1) + ":00";
                //                string strJCBTable = "KJ_DataAlarm" + Convert.ToDateTime(dattime).ToString("yyyyMM");
                //                string strsqlKJ_StaFiveMinute = " select " + strJCBTable + ".ID,KJ_DeviceDefInfo.PointID,KJ_DeviceAddress.wz as wzname,KJ_DeviceType.name as devname," + strJCBTable
                //                    + ".ssz,stime,etime," + strJCBTable + @".state, BFT_EnumCode.strEnumDisplay  as sbstate,BFT_EnumCode1.strEnumDisplay as datastate,
                //(case isalarm when 1 then  '报警' else '' end) as bj,(case  when LENGTH(kdid) > 0 then '断电' else '' end)  as dd,'' as ddwz,'' as kd,'' as js,'" + 
                //strsq + "' as sj,'' as kjsj,'' as kjxl,'' as kdcs from KJ_DeviceDefInfo left join " + strJCBTable + "  on KJ_DeviceDefInfo.PointID=" + strJCBTable + 
                //".PointID  left  join BFT_EnumCode on BFT_EnumCode.lngEnumValue=" + strJCBTable + ".state and BFT_EnumCode.EnumTypeID=8 left  join BFT_EnumCode as BFT_EnumCode1 on BFT_EnumCode1.lngEnumValue="
                //+ strJCBTable + ".type and BFT_EnumCode1.EnumTypeID=4 left join KJ_DeviceType on KJ_DeviceDefInfo.devid=KJ_DeviceType.devid left join KJ_DeviceAddress on KJ_DeviceDefInfo.wzid=KJ_DeviceAddress.wzid where   '" +
                //dattime + "' between stime and etime and " + strJCBTable + ".PointID=" + PointID;
                //                dt = this.GetDataTableBySQL(strsqlKJ_StaFiveMinute);

                //                //得到开机效率
                //                string[] strkjxl = GetKJXL(dattime, PointID);
                //                dt.Rows[0]["kjsj"] = strkjxl[0];
                //                dt.Rows[0]["kjxl"] = strkjxl[1];
                //                dt.Rows[0]["kdcs"] = strkjxl[2];
                //                //#region
                //                //string strstarttime = Convert.ToDateTime(dattime).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(dattime).Hour + ":00:00";
                //                //string strendtime = Convert.ToDateTime(dattime).ToString("yyyy-MM-dd") + " " + (Convert.ToDateTime(dattime).Hour + 1) + ":00:00";
                //                //string strsql = @"SELECT type,sum((unix_timestamp(case  when etime>'" + strendtime + "' then '" + strendtime + "' else etime end) - unix_timestamp(case when stime < '" + strstarttime + "' then '" + strstarttime + "' else stime end)) )	 AS `sumtime` FROM " + strJCBTable + "  where PointID=" + PointID + "  and  ((stime BETWEEN '" + strstarttime + "' and   '" + strendtime + "') or (etime BETWEEN '" + strstarttime + "' and   '" + strendtime + "'))   group by type  order by stime";
                //                //DataTable dtsum = this.GetDataTableBySQL(strsql);
                //                //int kjsj = 0;//开机时间
                //                //int xysj = 0;//异常时间
                //                //DataRow[] rowsxy = dtsum.Select("Type=27");
                //                //if (rowsxy.Length > 0) kjsj = Convert.ToInt32(rowsxy[0]["sumtime"]);
                //                //rowsxy = dtsum.Select("Type in (25,26)");
                //                //foreach (DataRow row in rowsxy)
                //                //{
                //                //    xysj += Convert.ToInt32(row["sumtime"]);
                //                //}

                //                //if (dt.Rows.Count > 0)
                //                //{
                //                //    TimeSpan ts = new TimeSpan(0, 0, kjsj);
                //                //    dt.Rows[0]["kjsj"] = (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                //                //    dt.Rows[0]["kjxl"] = (Convert.ToDouble(kjsj) / Convert.ToDouble(kjsj + xysj)).ToString("P2");
                //                //}

                //                ////得到开停次数
                //                //strsql = "select  count(1)  from " + strJCBTable + " where pointID = " + PointID + " and stime BETWEEN '" + strstarttime + "'  and '" + strendtime + "'";
                //                //DataTable dtkgcount = this.GetDataTableBySQL(strsql);
                //                //if (dtkgcount.Rows.Count > 0 && dt.Rows.Count > 0)
                //                //{
                //                //    dt.Rows[0]["kdcs"] = Convert.ToInt32(dtkgcount.Rows[0][0]);
                //                //}
                //                //#endregion


                //                string strddsql = @"select " + strJCBTable + ".id," + strJCBTable + ".pointid,type,kzk,kdid,KJ_DeviceAddress.wz  from " + strJCBTable + " left join KJ_DeviceDefInfo on (locate(concat(',', `KJ_DeviceDefInfo`.`point`, ','),	concat(	',',REPLACE (cast(" + strJCBTable + ".kzk AS CHAR charset utf8),'|',','),',')	) > 0)  	LEFT JOIN `KJ_DeviceAddress` ON ((`KJ_DeviceDefInfo`.`wzid` = `KJ_DeviceAddress`.`WZID`	)) where  " + strJCBTable + ".PointID=" + PointID + " and '" + dattime + "' between stime and etime";
                //                DataTable dtdd = this.GetDataTableBySQL(strddsql);

                //                string strkdids = "";
                //                DataTable dtDataState = null;
                //                foreach (DataRow row in dtdd.Rows)
                //                {
                //                    string strkdid = Convert.ToString(row["kdid"]);//开关量通过kdid来判断是不是断电
                //                    if (Convert.ToString(strkdid).Length == 0) continue;

                //                    string strwz = Convert.ToString(row["wz"]);
                //                    dt.Rows[0]["ddwz"] += strwz + "|";


                //                    if (strkdid.Length > 0)
                //                        strkdids += strkdid + ",";
                //                }

                //                //得到馈电相关数据
                //                if (strkdids.Length > 0)
                //                {
                //                    strkdids = strkdids.Substring(0, strkdids.Length - 1);
                //                    string strkdsql = "select * from " + strJCBTable + " where id in(" + strkdids + ")";
                //                    DataTable dtkd = this.GetDataTableBySQL(strkdsql);
                //                    foreach (DataRow row in dtkd.Rows)
                //                    {
                //                        if (dtDataState == null) dtDataState = this.GetDataState();
                //                        int TypeID = Convert.ToInt32(row["type"]);
                //                        DataRow[] rows = dtDataState.Select("lngEnumValue=" + TypeID);
                //                        string strtext = Convert.ToString(rows[0]["strEnumDisplay"]);
                //                        dt.Rows[0]["kd"] += strtext + "|";
                //                        dt.Rows[0]["js"] += Convert.ToString(row["cs"]) + "  ";


                //                    }

                //                }

                #endregion

                dt = new DataTable();
                dt.TableName = "GetKGLStateGirdData";
                dt.Columns.Add("wzname", Type.GetType("System.String")); //名称及类型
                dt.Columns.Add("ssz", Type.GetType("System.String")); //报警及断电状态
                dt.Columns.Add("s_etime", Type.GetType("System.String")); //读值时区                
                dt.Columns.Add("timer", Type.GetType("System.String")); //读值时刻
                dt.Columns.Add("kjxl", Type.GetType("System.String")); //开机效率
                dt.Columns.Add("kjsj", Type.GetType("System.String")); //开机时间
                dt.Columns.Add("kdcs", Type.GetType("System.String")); //开停次数
                dt.Columns.Add("bj", Type.GetType("System.String")); //报警/解除
                dt.Columns.Add("dd", Type.GetType("System.String")); //断电/复电
                dt.Columns.Add("kd", Type.GetType("System.String")); //馈电状态
                dt.Columns.Add("js", Type.GetType("System.String")); //措施及时刻

                var SelTime = Convert.ToDateTime(request.dattime);
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                var DevName = DevAWzID[2];
                var Wz = DevAWzID[3];

                var QueryStr = new object[dt.Columns.Count];
                var tempPointInf = new string[11];


                QueryStr[0] = Wz + "(" + DevName + ")";
                QueryStr[2] = SelTime.ToShortDateString() + " " + SelTime.Hour + ":00" + "~" + (SelTime.Hour + 1) +
                              ":00";
                QueryStr[3] = SelTime.ToLongTimeString();
                var Dt1 = new DateTime();
                var dt2 = new DateTime();
                Dt1 = DateTime.Parse(SelTime.ToLongDateString());
                dt2 = DateTime.Parse(SelTime.ToLongDateString());
                Dt1 = Dt1.AddHours(SelTime.Hour);
                dt2 = dt2.AddHours(SelTime.Hour + 1);

                //tempPointInf = GetDgView(SelTime, CurrentPointID, CurrentDevid, CurrentWzid, request.kglztjsfs);
                var req = new GetDgViewRequest
                {
                    SzNameT = SelTime,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = request.kglztjsfs
                };
                var res = GetDgView(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                tempPointInf = res.Data;

                QueryStr[1] = tempPointInf[1];
                QueryStr[7] = tempPointInf[7];
                QueryStr[8] = tempPointInf[8];
                QueryStr[9] = tempPointInf[9];
                QueryStr[10] = tempPointInf[10];

                //tempPointInf = GetKjThings(Dt1, dt2, CurrentPointID, CurrentDevid, CurrentWzid, request.kglztjsfs);
                var req2 = new GetKjThingsRequest
                {
                    DtStart = Dt1,
                    DtEnd = dt2,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = request.kglztjsfs
                };
                var res2 = GetKjThings(req2);
                if (!res2.IsSuccess)
                {
                    throw new Exception(res2.Message);
                }
                tempPointInf = res2.Data;

                QueryStr[4] = tempPointInf[4];
                QueryStr[5] = tempPointInf[5];
                QueryStr[6] = tempPointInf[6];

                dt.Rows.Add(QueryStr);
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到得到开关量状态图Grid数据失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     开关量柱状图（开关量状态统计）
        /// </summary>
        /// <param name="dattime"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<string[]> GetKJXL(GetKJXLRequest request)
        {
            var strJCBTable = "KJ_DataAlarm" + Convert.ToDateTime(request.dattime).ToString("yyyyMM");
            var str = new string[7];

            try
            {
                #region old

                //string strstarttime = Convert.ToDateTime(dattime).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(dattime).Hour + ":00:00";
                //string strendtime = Convert.ToDateTime(dattime).ToString("yyyy-MM-dd") + " " + (Convert.ToDateTime(dattime).Hour + 1) + ":00:00";
                //string strsql = @"SELECT type,sum((unix_timestamp(case  when etime>'" + strendtime + "' then '" + strendtime +
                //    "' else etime end) - unix_timestamp(case when stime < '" + strstarttime + "' then '" + strstarttime 
                //    + "' else stime end)) )	 AS `sumtime` FROM " + strJCBTable + "  where PointID=" + PointID
                //    + "  and  ((stime BETWEEN '" + strstarttime + "' and   '" + strendtime + "') or (etime BETWEEN '" + 
                //    strstarttime + "' and   '" + strendtime + "'))   group by type  order by stime";
                //DataTable dtsum = this.GetDataTableBySQL(strsql);
                //int kjsj = 0;//有电(开)时间
                //int gjsj = 0;//无电(关)时间
                //int xysj = 0;//异常(断线)时间
                //DataRow[] rowsxy = dtsum.Select("Type=27");//开机时间
                //if (rowsxy.Length > 0) kjsj = Convert.ToInt32(rowsxy[0]["sumtime"]);
                //rowsxy = dtsum.Select("Type=26");//关机时间
                //if (rowsxy.Length > 0) gjsj = Convert.ToInt32(rowsxy[0]["sumtime"]);
                //rowsxy = dtsum.Select("Type=25");
                //if (rowsxy.Length > 0) xysj = Convert.ToInt32(rowsxy[0]["sumtime"]);//异常时间         


                //TimeSpan ts = new TimeSpan(0, 0, kjsj);
                //str[0] = ts.Minutes + "分钟" + ts.Seconds + "秒";//开机时间
                //str[1] = (Convert.ToDouble(kjsj) / 3600).ToString("P2");//开机比例
                ////if (kjsj == 0 && gjsj == 0 && xysj == 0)
                ////{
                ////    str[0] = "0小时60分";
                ////    str[1] = "100%"; 
                ////}


                //strsql = "select  type,count(1)  from " + strJCBTable + " where pointID = " + PointID + " and stime BETWEEN '" + strstarttime + "'  and '" + strendtime + "' and type in(25,26,27) group by type";
                //DataTable dtkgcount = this.GetDataTableBySQL(strsql);
                //int sumcount = 0;

                //for (int i = 0; i < dtkgcount.Rows.Count; i++)
                //{
                //    sumcount += Convert.ToInt32(dtkgcount.Rows[i][1]);
                //}
                //str[2] = sumcount.ToString();//总的次数

                //ts = new TimeSpan(0, 0, gjsj);
                //str[3] = ts.Minutes + "分钟" + ts.Seconds + "秒";//关机时间
                //str[4] = (Convert.ToDouble(gjsj) / 3600).ToString("P2");//关机比例

                //rowsxy = dtkgcount.Select("Type=27");
                //if (rowsxy.Length > 0) str[5] = Convert.ToString(rowsxy[0][1]);//开的次数
                //rowsxy = dtkgcount.Select("Type=26");
                //if (rowsxy.Length > 0) str[6] = Convert.ToString(rowsxy[0][1]);//关的次数

                #endregion

                var strstarttime =
                    DateTime.Parse(Convert.ToDateTime(request.dattime).ToString("yyyy-MM-dd") + " " +
                                   Convert.ToDateTime(request.dattime).Hour + ":00:00");
                var strendtime = strstarttime;
                if (Convert.ToDateTime(request.dattime).Hour < 23)
                {
                    strendtime =
                        DateTime.Parse(Convert.ToDateTime(request.dattime).ToString("yyyy-MM-dd") + " " +
                                       (Convert.ToDateTime(request.dattime).Hour + 1) + ":00:00");
                }
                else
                {
                    strendtime =
                           DateTime.Parse(Convert.ToDateTime(request.dattime).ToString("yyyy-MM-dd") + " " +
                                          (Convert.ToDateTime(request.dattime).Hour) + ":59:59");
                }

                var HourNow = Convert.ToDateTime(request.dattime).Hour;
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                //var Rvalue = GetStateBarTable(strstarttime, CurrentPointID, CurrentDevid, CurrentWzid, request.kglztjsfs);
                var req = new GetStateBarTableRequest
                {
                    SzNameT = strstarttime,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = request.kglztjsfs
                };
                var res = GetStateBarTable(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                var Rvalue = res.Data;

                //取出当前小时的开停机时间及开停次数
                var dt_StateTable = Rvalue.dtTotal;
                var Rowindex = HourNow / 8 * 3 + 1; //开停时间所在的行，开停次数所在行在此基础上加1
                var Columnindex = HourNow % 8 + 1; //开停时间和次数所在的列

                //计算开、关机的时间（格式：0分0秒）
                var KtTime = dt_StateTable.Rows[Rowindex][Columnindex].ToString();
                var KTime = KtTime.Substring(0, KtTime.IndexOf("(")); //开时间
                var TTime = KtTime.Substring(KtTime.IndexOf("(") + 1, KtTime.Length - 2 - KtTime.IndexOf("(")); //停时间
                str[0] = KTime.Split(':')[0] + "分" + KTime.Split(':')[1] + "秒"; //开机时间
                str[3] = TTime.Split(':')[0] + "分" + TTime.Split(':')[1] + "秒"; //关机时间

                //计算开关机及总的次数
                var KtCount = dt_StateTable.Rows[Rowindex + 1][Columnindex].ToString();
                var KCount = KtCount.Substring(0, KtCount.IndexOf("("));
                var TCount = KtCount.Substring(KtCount.IndexOf("(") + 1, KtCount.Length - 2 - KtCount.IndexOf("("));
                str[5] = KCount;//开次数
                str[6] = TCount;//停次数
                str[2] = (int.Parse(TCount) + int.Parse(KCount)).ToString();//变动次数

                //计算开、关机比例
                var KPer = (double.Parse(KTime.Split(':')[0]) * 60 + double.Parse(KTime.Split(':')[1])) / 3600.00;
                var TPer = (double.Parse(TTime.Split(':')[0]) * 60 + double.Parse(TTime.Split(':')[1])) / 3600.00;
                str[1] = Math.Round(KPer * 100, 2) + "%";//开机率
                str[4] = Math.Round(TPer * 100, 2) + "%";//停机率
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到开关量开机效率相关失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<string[]>
            {
                Data = str
            };
            return ret;
        }

        /// <summary>
        ///     得到开关量状态变化图右边Grid数据(KJ_DataRunRecord)
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKGLStateGridDataByRight(GetKGLStateGridDataByRightRequest request)
        {
            DataTable dt = null;
            try
            {
                #region old

                //string strDate = Convert.ToDateTime(dattime).ToString("yyyMMdd");
                //string strJCRTable = "KJ_DataRunRecord" + Convert.ToDateTime(dattime).ToString("yyyyMM");
                //string strsql = "select * from " + strJCRTable + " where date_format(timer,'%Y%m%d')=" + strDate + " and (PointID=" + PointID + " or fzh=0) order by timer";
                //dt = this.GetDataTableBySQL(strsql);

                #endregion

                dt = new DataTable();
                dt.TableName = "GetKGLStateGridDataByRight";
                var SelTime = Convert.ToDateTime(request.dattime);
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                //dt = GetStateChgdt(SelTime, CurrentPointID, CurrentDevid, CurrentWzid, request.kglztjsfs);
                var req = new GetStateChgdtRequest
                {
                    SzNameT = SelTime,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = request.kglztjsfs
                };
                var res = GetStateChgdt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到开关量状态变化图右边Grid数据，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     模拟量报警曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMnlBJLine(GetMnlBJLineRequest request)
        {
            var dt = new DataTable();
            dt.TableName = "GetMnlBJLine";
            try
            {
                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 31)
                    throw new Exception("查询的最大天数为31天,请重新选择日期！");
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                //dt = GetMnlBjLineDt(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid, "1");
                var req = new GetMnlBjLineDtRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    type = "1"
                };
                var res = GetMnlBjLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量报警情况失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     模拟量断电曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMnlDDLine(GetMnlDDLineRequest request)
        {
            var dt = new DataTable();
            dt.TableName = "GetMnlDDLine";
            try
            {
                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 31)
                    throw new Exception("查询的最大天数为31天,请重新选择日期！");
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                //dt = GetMnlBjLineDt(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid, "2");
                var req = new GetMnlBjLineDtRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    type = "2"
                };
                var res = GetMnlBjLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量断电情况失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     获取模拟量测点对应的控制量测点
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetPointKzByPointID(GetPointKzByPointIDRequest request)
        {
            var dt = new DataTable();
            dt.TableName = "GetPointKzByPointID";
            try
            {
                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);

                var CurrentPointID = request.PointID.ToString();

                //dt = GetPointKzList(SzNameS, SzNameE, CurrentPointID);
                var req = new GetPointKzListRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    PointID = CurrentPointID
                };
                var res = GetPointKzList(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取模拟量测点对应的控制量测点，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     获取控制量馈电异常曲线
        /// </summary>
        /// <param name="datStart"></param>
        /// <param name="datEnd"></param>
        /// <param name="PointID"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKzlLine(GetKzlLineRequest request)
        {
            var dt = new DataTable();
            dt.TableName = "GetKzlLine";
            try
            {
                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);
                var ts = SzNameE - SzNameS;
                if (ts.TotalDays > 7)
                    throw new Exception("查询的最大天数为7天,请重新选择日期！");
                var CurrentPointID = request.PointID.ToString();
                var DevAWzID = GetPointDevAndWzID(CurrentPointID);
                var CurrentDevid = DevAWzID[0];
                var CurrentWzid = DevAWzID[1];
                //dt = GetKzlLineDt(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetKzlLineDtRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = GetKzlLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("得到模拟量断电情况失败，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        /// <summary>
        ///     获取模拟量、开关量测点信息
        /// </summary>
        /// <param name="datStart">开始时间</param>
        /// <param name="datEnd">结束时间</param>
        /// <param name="Type">测点类型 0:模拟量，1：开关量</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetPointByType(GetPointByTypeRequest request)
        {
            var dt = new DataTable();
            dt.TableName = "GetPointByPointID";
            try
            {
                var SzNameS = DateTime.Parse(request.datStart);
                var SzNameE = DateTime.Parse(request.datEnd);

                //dt = GetPointList(SzNameS, SzNameE, Type);
                var req = new GetPointListRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    Type = request.Type
                };
                var res = GetPointList(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dt = res.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取模拟量测点对应的控制量测点，原因为" + ex);
                throw new Exception(ex.ToString());
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        #endregion

        #region 曲线配置

        /// <summary>
        ///     保存曲线配置
        /// </summary>
        /// <param name="ChartSet"></param>
        /// <returns></returns>
        public BasicResponse<bool> SaveChartSet(SaveChartSetRequest request)
        {
            var Rvalue = false;
            var strsql = "";
            try
            {
                foreach (var item in request.ChartSet)
                {
                    strsql = "select strKey from BFT_Setting where strKey='" + item.Key + "'";
                    //var dt = GetDataTableBySQL(strsql);
                    var dt = _listexRepositoryBase.QueryTableBySql(strsql);
                    if (dt.Rows.Count > 0)
                        strsql = string.Format("update BFT_Setting set strValue='{0}' where strKey='{1}'", item.Value,
                            item.Key);
                    else
                        strsql =
                            string.Format(
                                @"insert into BFT_Setting(ID,strType,strKey,strKeyCHs,strValue,strDesc,Creator,LastUpdateDate) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')"
                            //, DateTimeUtil.GetDateTimeNowToInt64()
                                , IdHelper.CreateLongId()
                                , "曲线配置"
                                , item.Key
                                , ""
                                , item.Value
                                , ""
                                , ""
                                , DateTime.Now);
                    //ExecuteSQL(strsql);
                    _listexRepositoryBase.ExecuteNonQueryBySql(strsql);
                }
                Rvalue = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error("SaveChartSet" + ex);
            }
            var ret = new BasicResponse<bool>
            {
                Data = Rvalue
            };
            return ret;
        }

        /// <summary>
        ///     获取曲线配置
        /// </summary>
        /// <param name="strKey">曲线配置key,""表示返回所有</param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAllChartSet(GetAllChartSetRequest request)
        {
            var strsql = "";
            if (string.IsNullOrEmpty(request.StrKey))
                strsql = "select strKey,strValue from BFT_Setting ";
            else
                strsql = "select strKey,strValue from BFT_Setting where strKey='" + request.StrKey + "'";
            //var dt = GetDataTableBySQL(strsql);
            var dt = _listexRepositoryBase.QueryTableBySql(strsql);
            var ret = new BasicResponse<DataTable>
            {
                Data = dt
            };
            return ret;
        }

        #endregion

    }
}
