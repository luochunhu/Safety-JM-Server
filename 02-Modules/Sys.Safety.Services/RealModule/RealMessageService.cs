using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sys.Safety.ServiceContract;
using Sys.Safety.Services;
using Sys.Safety.Model;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Web;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Enums.Enums;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.DataContract.UserRoleAuthorize;

namespace Sys.Safety.Services
{
    public class RealMessageService : IRealMessageService
    {

        private IRealMessageRepository _Repository;
        private ICalibrationDefRepository _Jc_BxRepositor;
        private ISettingRepository _SettingRepository;

        private IRunLogCacheService runLogCacheService = ServiceFactory.Create<IRunLogCacheService>();
        private IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        private IAllSystemPointDefineService allSystemPointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();
        private IDeviceDefineCacheService deviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
        private IPositionCacheService positionCacheService = ServiceFactory.Create<IPositionCacheService>();
        private INetworkModuleCacheService networkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();
        private IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();


        public RealMessageService(IRealMessageRepository _Repository, ICalibrationDefRepository _Jc_BxRepositor, ISettingRepository _SettingRepository)
        {
            this._Repository = _Repository;
            this._Jc_BxRepositor = _Jc_BxRepositor;
            this._SettingRepository = _SettingRepository;
        }

        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RealMessageService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }

        public BasicResponse<DataTable> RemoteGetShowTb(RemoteGetShowTbRequest realMessageRequest)
        {
            var response = new BasicResponse<DataTable>();
            //TODO:远程升级功能暂时注释
            //DataTable dt = new DataTable();
            //dt.Columns.Add("c", typeof(int));
            //dt.Columns.Add("c1", typeof(string));
            //dt.Columns.Add("c2", typeof(string));
            //dt.Columns.Add("c3", typeof(string));
            //dt.Columns.Add("c4", typeof(string));
            //dt.Columns.Add("c5", typeof(string));
            //dt.Columns.Add("c6", typeof(string));
            //string[] sjzttxt = new string[] { "未知", "请求中", "等待文件下发", "文件接收中", "文件接收完成", "重启升级中", "升级完成", "取消升级中", "请求成功", "重启升级成功", "取消升级成功", "恢复备份成功", "恢复备份中", "获取版本信息", "获取版本信息成功", "请求失败", "取消升级失败", "巡检接收情况", "补发" };
            //DataRow row;
            //try
            //{
            //    if (!string.IsNullOrEmpty(realMessageRequest.Fzh))
            //    {
            //        int fz = 0;
            //        string[] f = realMessageRequest.Fzh.Split('|');
            //        for (int i = 0; i < f.Length; i++)
            //        {
            //            if (int.TryParse(f[i], out fz))
            //            {
            //                var pointDefineCacheGetByConditonRequest = new PointDefineCacheGetByStationRequest();
            //                var pointDefineCacheGetByConditonResponse = pointDefineCacheService.GetPointDefineCacheByStation(pointDefineCacheGetByConditonRequest);
            //                if (pointDefineCacheGetByConditonResponse.Data != null && pointDefineCacheGetByConditonResponse.Data.Count > 0)
            //                {
            //                    Jc_DefInfo def = pointDefineCacheGetByConditonResponse.Data.FirstOrDefault(a => a.DevPropertyID == (int)DevPropertype.Substation);
            //                    if (def != null)
            //                    {
            //                        row = dt.NewRow();
            //                        row["c"] = fz;
            //                        row["c1"] = def.State;
            //                        row["c2"] = def.Fzyjbb;
            //                        row["c3"] = GlobalConfigInfo.CurrentIndex;
            //                        row["c4"] = sjzttxt[def.Sjstate] == "补发" ? sjzttxt[def.Sjstate] + def.Wjqsxh : sjzttxt[def.Sjstate];
            //                        row["c5"] = GlobalCnfgInfo.CurrentIndex + "/" + GlobalCnfgInfo.UpdateCount;
            //                        dt.Rows.Add(row);
            //                    }
            //                }
            //            }
            //        }
            //        response.Data = dt;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.Code = -100;                
            //    response.Message = ex.Message;
            //    LogHelper.Error("RemoteGetShowTb", ex);
            //}

            return response;
        }

        /// <summary>
        /// 开始升级过程或者结束升级过程
        /// </summary>
        /// <param name="i">0-开始 1-结束过程并结束文件下发</param>
        /// <returns></returns>
        public bool RemoteUpdateStrtOrStop(int i)
        {
            bool flg = false;
            //TODO:远程升级功能暂时注释
            //if (i == 0)
            //{
            //    ReadRemoteConfig();
            //    #region 开始升级
            //    Basic.DTO.ConfigDTO config = new Basic.DTO.ConfigDTO();
            //    config.Name = "UpdateSending";
            //    config.Text = "1";
            //    SaveConfig(new List<Basic.DTO.ConfigDTO> { config });
            //    #endregion
            //}
            //else if (i == 1)
            //{
            //    #region 结束升级
            //    Basic.DTO.ConfigDTO config = new Basic.DTO.ConfigDTO();
            //    config.Name = "UpdateSending";
            //    config.Text = "0";
            //    SaveConfig(new List<Basic.DTO.ConfigDTO> { config });
            //    #endregion
            //}
            return flg;

        }

        /// <summary>
        /// 开始升级过程、或结束升级过程
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> RemoteUpdateStrtOrStop(RemoteUpdateStrtOrStopRequest realMessageRequest)
        {
            var response = new BasicResponse<bool>();
            //TODO:远程升级功能暂时注释
            //if (!realMessageRequest.Type.HasValue)
            //{
            //    response.Code = -100;
            //    response.Data = false;
            //    response.Message = "参数错误！";
            //    return response;
            //}
            //try
            //{
            //    bool b = _Repository.RemoteUpdateStrtOrStop(realMessageRequest.Type.Value);
            //    response.Code = b == true ? 100 : -100;
            //    response.Message = b == true ? "操作成功！" : "操作失败！";
            //}
            //catch (Exception ex)
            //{
            //    response.Code = -100;
            //    response.Data = false;
            //    response.Message = ex.Message;
            //    LogHelper.Error("获取所有绑定电源箱分站", ex);

            //}
            return response;
        }

        /// <summary>
        /// 远程升级命令
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse RemoteUpgradeCommand(RemoteUpgradeCommandRequest realMessageRequest)
        {
            var remoteUpgradeResponse = new BasicResponse();
            //TODO:远程升级功能暂时注释
            //_Repository.RemoteUpgradeCommand(realMessageRequest.Fzh, realMessageRequest.SendD, realMessageRequest.Sjml, realMessageRequest.SjState);
            return remoteUpgradeResponse;
        }

        /// <summary>
        ///获取自定义编排测点号
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetCustomPagePoint(GetCustomPagePointRequest realMessageRequest)
        {
            var GetCustomPagePointResponse = new BasicResponse<DataTable>();
            var dt = _Repository.GetCustomPagePoint(realMessageRequest.Page);
            GetCustomPagePointResponse.Data = dt;
            return GetCustomPagePointResponse;
        }

        /// <summary>
        /// 读取配置信息到config表中
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<string> ReadConfig(ReadConfigRequest realMessageRequest)
        {
            var ReadConfigResponse = new BasicResponse<string>();
            var strValue = _Repository.ReadConfig(realMessageRequest.KeyName);
            ReadConfigResponse.Data = strValue;
            return ReadConfigResponse;
        }

        /// <summary>
        /// 获取所有测点信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetAllPointinformation()
        {
            var response = new BasicResponse<DataTable>();
            string devid, wzid;
            DataTable dt = CreateDataTable();
            DataRow row = null;
            StateToClient state = new StateToClient();
            try
            {
                //var getAllPointRequest = new PointDefineCacheGetAllRequest();
                //var getAllPointResponse = pointDefineCacheService.GetAllPointDefineCache(getAllPointRequest);
                //多系统融合  20171123
                var getAllPointResponse = allSystemPointDefineService.GetAllPointDefineCache();
                if (getAllPointResponse.Data != null)
                {
                    foreach (var item in getAllPointResponse.Data)
                    {
                        #region 加载设备信息
                        row = dt.NewRow();
                        devid = item.Devid;
                        wzid = item.Wzid;
                        row["point"] = item.Point;
                        row["pointid"] = item.PointID;
                        row["fzh"] = item.Fzh;
                        row["tdh"] = item.Kh;
                        row["dzh"] = item.Dzh;
                        row["ssz"] = item.Ssz;
                        row["zt"] = item.DataState;
                        if ((item.Bz4 & 0x0002) == 2)//休眠
                        {
                            row["sbzt"] = state.EqpState33;
                            row["zt"] = state.EqpState33;
                            row["ssz"] = "休眠";
                        }
                        else if ((item.Bz4 & 0x0004) == 4)//检修
                        {
                            row["sbzt"] = state.EqpState34;
                        }
                        else if ((item.Bz4 & 0x0008) == 8)//标校
                        {
                            row["sbzt"] = state.EqpState23;
                        }
                        else
                        {
                            row["sbzt"] = item.State;
                        }
                        row["voltage"] = item.Voltage;
                        row["bj"] = item.Alarm;
                        row["sxyj"] = item.Z1;
                        row["sxbj"] = item.Z2;
                        row["sxdd"] = item.Z3;
                        row["sxfd"] = item.Z4;
                        row["xxyj"] = item.Z5;
                        row["xxbj"] = item.Z6;
                        row["xxdd"] = item.Z7;
                        row["xxfd"] = item.Z8;
                        row["time"] = item.Zts;
                        row["lx"] = item.DevProperty;
                        row["lxtype"] = item.DevPropertyID;
                        row["xh"] = item.DevModel;
                        row["xhtype"] = item.DevModelID;
                        row["lb"] = item.DevName;
                        row["zl"] = item.DevClass;
                        if (item.DevProperty == "分站")
                        {
                            if (item.Voltage > 0)
                            {
                                row["dldj"] = item.Voltage.ToString() + " %";
                            }
                            else
                            {
                                row["dldj"] = "0";
                            }
                        }
                        else
                        {
                            if (item.Voltage == 1)
                            {
                                row["dldj"] = "<9.0 V";
                            }
                            else if (item.Voltage == 2)
                            {
                                row["dldj"] = "9.0～16.5 V";
                            }
                            else if (item.Voltage == 3)
                            {
                                row["dldj"] = "16.5～24.0 V";
                            }
                            else if (item.Voltage == 4)
                            {
                                row["dldj"] = ">24.0 V";
                            }
                            else
                            {
                                row["dldj"] = item.Voltage;
                            }
                        }
                        #region 加载设备类型
                        var deviceDefineRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                        var deviceDefineResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineRequest);
                        if (deviceDefineResponse.Data != null)
                        {
                            row["devname"] = deviceDefineResponse.Data.Name;
                            row["dw"] = deviceDefineResponse.Data.Xs1;
                        }
                        #endregion
                        row["0t"] = item.Bz6;
                        row["1t"] = item.Bz7;
                        row["2t"] = item.Bz8;
                        row["0tcolor"] = item.Bz9;
                        row["1tcolor"] = item.Bz10;
                        row["2tcolor"] = item.Bz11;
                        row["wz"] = item.Wz;
                        row["k1"] = item.K1;
                        row["k2"] = item.K2;
                        row["k3"] = item.K3;
                        row["k8"] = item.K8;
                        row["NCtrlSate"] = item.NCtrlSate;//获取控制量馈电状态  20170725
                        row["GradingAlarmLevel"] = item.GradingAlarmLevel;
                        row["StationDyType"] = item.StationDyType;
                        dt.Rows.Add(row);
                        #endregion
                    }
                    response.Data = dt;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取所有定义信息", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取所有绑定电源箱的分站
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetBindDianYuanFenzhan()
        {
            var response = new BasicResponse<DataTable>();
            DataTable msg = new DataTable();
            msg.Columns.Add("fzh", typeof(int));
            msg.Columns.Add("fd", typeof(int));
            msg.TableName = "fz";
            try
            {
                //var getAllPointRequest = new PointDefineCacheGetAllRequest();
                //var getAllPointResponse = pointDefineCacheService.GetAllPointDefineCache(getAllPointRequest);

                //多系统融合  20171123
                var getAllPointResponse = allSystemPointDefineService.GetAllPointDefineCache();
                if (getAllPointResponse.Data != null && getAllPointResponse.Data.Count > 0)
                {
                    foreach (var item in getAllPointResponse.Data)
                    {
                        if (item.Activity == "1" && item.DevPropertyID == 0)
                        {
                            if ((item.Bz3 & 0x8) == 0x8)
                            {
                                msg.Rows.Add(item.Fzh, 0);
                            }
                        }
                    }
                }
                if (msg.Rows.Count > 0)
                {
                    DataView ds = new DataView(msg);
                    ds.Sort = "fzh";
                    msg = ds.ToTable("ff");
                }
                response.Data = msg;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取所有绑定电源箱的分站", ex); ;
            }

            return response;
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <returns></returns>
        //public BasicResponse<DataTable> GetRealData()
        //{
        //    var response = new BasicResponse<DataTable>();
        //    DataTable dt = new DataTable();
        //    dt.TableName = "point";
        //    DataRow row;
        //    StateToClient state = new StateToClient();
        //    dt.Columns.Add("point", typeof(string));
        //    dt.Columns.Add("bj", typeof(string));
        //    dt.Columns.Add("ssz", typeof(string));
        //    dt.Columns.Add("zt", typeof(string));
        //    dt.Columns.Add("sbzt", typeof(string));
        //    dt.Columns.Add("dldj", typeof(string));
        //    dt.Columns.Add("time", typeof(string));
        //    try
        //    {
        //        var getAllPointRequest = new PointDefineCacheGetAllRequest();
        //        var getAllPointResponse = pointDefineCacheService.GetAllPointDefineCache(getAllPointRequest);
        //        if (getAllPointResponse.Data != null && getAllPointResponse.Data.Count > 0)
        //        {
        //            foreach (var item in getAllPointResponse.Data)
        //            {
        //                #region 加载设备实时信息
        //                row = dt.NewRow();
        //                row["point"] = item.Point;
        //                row["ssz"] = item.Ssz;
        //                row["zt"] = item.DataState;
        //                if ((item.Bz4 & 0x0002) == 2)//休眠
        //                {
        //                    row["sbzt"] = state.EqpState33;
        //                    row["ssz"] = "休眠";
        //                }
        //                else if ((item.Bz4 & 0x0004) == 4)//检修
        //                {
        //                    row["sbzt"] = state.EqpState34;
        //                }
        //                else if ((item.Bz4 & 0x0008) == 8)//标校
        //                {
        //                    row["sbzt"] = state.EqpState23;
        //                }
        //                else
        //                {
        //                    row["sbzt"] = item.State;
        //                }
        //                row["bj"] = item.Alarm;
        //                row["time"] = item.Zts;
        //                if (item.DevProperty == "分站")
        //                {
        //                    if (item.Voltage > 0)
        //                    {
        //                        row["dldj"] = item.Voltage.ToString() + " V";
        //                    }
        //                    else
        //                    {
        //                        row["dldj"] = "0";
        //                    }
        //                }
        //                else
        //                {
        //                    if (item.Voltage == 1)
        //                    {
        //                        row["dldj"] = "<9.0 V";
        //                    }
        //                    else if (item.Voltage == 2)
        //                    {
        //                        row["dldj"] = "9.0～16.5 V";
        //                    }
        //                    else if (item.Voltage == 3)
        //                    {
        //                        row["dldj"] = "16.5～24.0 V";
        //                    }
        //                    else if (item.Voltage == 4)
        //                    {
        //                        row["dldj"] = ">24.0 V";
        //                    }
        //                    else
        //                    {
        //                        row["dldj"] = item.Voltage;
        //                    }
        //                }
        //                dt.Rows.Add(row);
        //                #endregion
        //            }
        //            response.Data = dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("获取所有实时数据", ex);
        //    }
        //    return response;
        //}
        /// <summary>
        /// 改为增量获取  20170719
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BasicResponse<List<RealDataDataInfo>> GetRealData(GetRealDataRequest request)
        {
            var response = new BasicResponse<List<RealDataDataInfo>>();
            List<RealDataDataInfo> realList = new List<RealDataDataInfo>();
            try
            {
                //var getAllPointRequest = new PointDefineCacheGetByConditonRequest();
                //getAllPointRequest.Predicate = a => a.DttStateTime >= request.LastRefreshRealDataTime;
                //var getAllPointResponse = pointDefineCacheService.GetPointDefineCache(getAllPointRequest);
                //多系统融合  20171123
                var getAllPointResponse = allSystemPointDefineService.GetAllPointDefineCache();

                //realList = ObjectConverter.CopyList<Jc_DefInfo, RealDataDataInfo>(getAllPointResponse.Data).ToList();
                realList = new List<RealDataDataInfo>();
                foreach (Jc_DefInfo temp in getAllPointResponse.Data)
                {
                    RealDataDataInfo item = new RealDataDataInfo();
                    item.PointID = temp.PointID;
                    item.Areaid = temp.Areaid;
                    item.Sysid = temp.Sysid;
                    item.Point = temp.Point;
                    item.Wz = temp.Wz;
                    item.Ssz = temp.Ssz;
                    item.DataState = temp.DataState;
                    item.State = temp.State;
                    item.Alarm = temp.Alarm;
                    item.Voltage = temp.Voltage;
                    item.Zts = temp.Zts;
                    item.Bz4 = temp.Bz4;
                    item.NCtrlSate = temp.NCtrlSate;
                    item.DttStateTime = temp.DttStateTime;
                    //item.DevName = temp.DevName;
                    item.DevClass = temp.DevClass;
                    item.DevModel = temp.DevModel;
                    item.Unit = temp.Unit;
                    item.DevProperty = temp.DevProperty;
                    item.GradingAlarmLevel = temp.GradingAlarmLevel;
                    item.StationDyType = temp.StationDyType;
                    realList.Add(item);
                }

                //realList = realList.Where(a => a.Bz19 != "1").ToList();//2018.6.11 by

                response.Data = realList;
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取所有实时数据", ex);
            }
            return response;
        }



        /// <summary>
        /// 获取运行记录
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetRunLogs(GetRunLogsRequest realMessageRequest)
        {
            var response = new BasicResponse<DataTable>();
            var dt = new DataTable();
            dt.TableName = "point";
            dt.Columns.Add("time", typeof(DateTime));
            dt.Columns.Add("point", typeof(string));
            dt.Columns.Add("wz", typeof(string));
            dt.Columns.Add("state", typeof(string));
            dt.Columns.Add("sbstate", typeof(string));
            dt.Columns.Add("ssz", typeof(string));
            dt.Columns.Add("uid", typeof(long));
            DataRow row = null;
            List<Jc_RInfo> listR;
            DateTime nowtime = DateTime.Now.AddHours(-2);
            try
            {
                var runLogCacheGetAllRequest = new RunLogCacheGetAllRequest();
                var runLogCacheGetAllResponse = runLogCacheService.GetAllRunLogCache(runLogCacheGetAllRequest);
                if (runLogCacheGetAllResponse.Data != null && runLogCacheGetAllResponse.Data.Count > 0)
                {
                    listR = runLogCacheGetAllResponse.Data.FindAll(x => long.Parse(x.ID) > realMessageRequest.UserId);
                    if (listR.Count > 0)
                    {
                        for (int i = 0; i < listR.Count; i++)
                        {
                            if (listR[i].Timer > nowtime)
                            {
                                row = dt.NewRow();
                                if (listR[i].Point == "module")
                                {
                                    row["point"] = listR[i].Remark;
                                }
                                else
                                {
                                    row["point"] = listR[i].Point;
                                }
                                row["time"] = listR[i].Timer;

                                var positionCacheGetByKeyRequest = new PositionCacheGetByKeyRequest();
                                var PositionCacheGetByKeyResponse = positionCacheService.GetPositionCacheByKey(positionCacheGetByKeyRequest);
                                if (PositionCacheGetByKeyResponse.Data != null)
                                {
                                    row["wz"] = PositionCacheGetByKeyResponse.Data.Wz;
                                }
                                else
                                {
                                    row["wz"] = "";
                                }
                                row["state"] = listR[i].Type;
                                row["sbstate"] = listR[i].State;
                                row["ssz"] = listR[i].Val;
                                row["uid"] = listR[i].ID;
                                dt.Rows.Add(row);
                            }
                        }
                        response.Data = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("  获取运行记录", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取网络模块数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetRealMac()
        {
            var response = new BasicResponse<DataTable>();
            DataTable dt = CreatMacTable();
            DataRow row;
            try
            {
                var networkModuleCacheGetAllRequest = new NetworkModuleCacheGetAllRequest();
                var networkModuleCacheGetAllResponse = networkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheGetAllRequest);
                List<Jc_MacInfo> allSwitches = networkModuleCacheGetAllResponse.Data.FindAll(a => a.Upflag == "1");//只加载交换机
                if (allSwitches != null && allSwitches.Count > 0)
                {
                    foreach (var item in allSwitches)
                    {
                        if (item.Type == 0 && item.MAC.Length >= 17)
                        {
                            row = dt.NewRow();
                            row["ip"] = item.IP;
                            row["mac"] = item.MAC;
                            row["wz"] = item.Wz;
                            row["zt"] = item.State;
                            row["ljh"] = item.NetID;
                            row["dl"] = item.Bz1.Replace(";", ";\r\n");
                            if (item.State == 0)
                            {
                                row["BatteryControlState"] = "-";
                                row["BatteryState"] = "-";
                                row["BatteryCapacity"] = "-";
                                row["SerialPortBatteryState"] = "-";
                                row["SerialPortRunState"] = "-";
                                row["SwitchBatteryState"] = "-";
                                row["SwitchRunState"] = "-";
                                row["Switch1000State"] = "-";
                                row["Switch100State"] = "-";
                            }
                            else
                            {
                                row["BatteryControlState"] = item.BatteryControlState == 0 ? "放电" : "不放电";
                                row["BatteryState"] = item.BatteryState == 0 ? "交流供电" : "直流供电";
                                row["BatteryCapacity"] = item.BatteryCapacity + "%";
                                row["SerialPortBatteryState"] = item.SerialPortBatteryState == 0 ? "供电故障" : "供电正常";
                                row["SerialPortRunState"] = item.SerialPortRunState == 0 ? "运行故障" : "运行正常";
                                row["SwitchBatteryState"] = item.SwitchBatteryState == 0 ? "供电故障" : "供电正常";
                                row["SwitchRunState"] = item.SwitchRunState == 0 ? "运行故障" : "运行正常";
                                //string Switch1000StateString = "";
                                //if (item.Switch1000State != null)
                                //{
                                //    for (int i = 0; i < item.Switch1000State.Length; i++)
                                //    {
                                //        Switch1000StateString += "千兆光口" + (i + 1).ToString() + ":" + (item.Switch1000State[i] == 0 ? "通信故障" : "通信正常") + "\r\n";
                                //    }
                                //}
                                row["Switch1000State"] = item.Switch1000State;
                                //string Switch100StateString = "";
                                //if (item.Switch100State != null)
                                //{
                                //    for (int i = 0; i < item.Switch100State.Length; i++)
                                //    {
                                //        Switch100StateString += "百兆光口" + (i + 1).ToString() + ":" + (item.Switch100State[i] == 0 ? "通信故障" : "通信正常") + "\r\n";
                                //    }
                                //}
                                row["Switch100State"] = item.Switch100State;

                                //string Switch100RJ45StateString = "";
                                //if (item.Switch100RJ45State != null)
                                //{
                                //    for (int i = 0; i < item.Switch100RJ45State.Length; i++)
                                //    {
                                //        Switch100RJ45StateString += "百兆电口" + (i + 1).ToString() + ":" + (item.Switch100RJ45State[i] == 0 ? "通信故障" : "通信正常") + "\r\n";
                                //    }
                                //}
                                row["Switch100RJ45State"] = item.Switch100RJ45State;
                            }
                            dt.Rows.Add(row);
                        }
                    }
                    response.Data = dt;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取运行记录", ex);
            }

            return response;
        }
        private DataTable CreatMacTable()
        {
            DataColumn col;
            string[] tcolname = new string[] { "ip", "mac", "wz", "ljh", "zt", "dl" ,"BatteryControlState","BatteryState","BatteryCapacity","SerialPortBatteryState","SerialPortRunState"
        ,"SwitchBatteryState","SwitchRunState","Switch1000State","Switch100State","Switch100RJ45State"};
            DataTable dt = new DataTable();
            for (int i = 0; i < tcolname.Length; i++)
            {
                col = new DataColumn(tcolname[i]);
                dt.Columns.Add(col);
            }
            dt.TableName = "point";
            return dt;
        }

        /// <summary>
        /// 获取报警数据
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<Jc_BInfo>> GetAlarmData()
        {
            var response = new BasicResponse<List<Jc_BInfo>>();
            var alarmCacheGetAllRequest = new AlarmCacheGetAllRequest();
            var alarmCacheGetAllResponse = alarmCacheService.GetAllAlarmCache(alarmCacheGetAllRequest);
            if (alarmCacheGetAllResponse.Data != null)
            {
                response.Data = alarmCacheGetAllResponse.Data;
            }
            return response;
        }

        /// <summary>
        /// 修改报警表 录入措施
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse UpdateAlarmStep(UpdateAlarmStepRequest realMessageRequest)
        {
            var updateAlarmStepResponse = new BasicResponse();
            if (string.IsNullOrWhiteSpace(realMessageRequest.Id) || string.IsNullOrWhiteSpace(realMessageRequest.Message) || string.IsNullOrWhiteSpace(realMessageRequest.TableName))
            {
                updateAlarmStepResponse.Code = -100;
                updateAlarmStepResponse.Message = "操作失败！";
                return updateAlarmStepResponse;
            }
            try
            {
                //修改报警措施
                _Repository.UpdateAlarmStep(realMessageRequest.Id, realMessageRequest.TableName, realMessageRequest.Message);
                #region 更新缓存
                var jc_BModel = _Repository.GetAlarmInfoById(realMessageRequest.TableName, realMessageRequest.Id);
                var jc_BInfo = ObjectConverter.Copy<Jc_BModel, Jc_BInfo>(jc_BModel);
                var alarmCacheUpdateRequest = new AlarmCacheUpdateRequest() { AlarmInfo = jc_BInfo };
                alarmCacheService.UpdateAlarmCahce(alarmCacheUpdateRequest);
                #endregion
            }
            catch (Exception ex)
            {
                updateAlarmStepResponse.Code = -100;
                updateAlarmStepResponse.Message = ex.Message;
                this.ThrowException("修改报警表", ex); ;
            }

            return updateAlarmStepResponse;
        }

        /// <summary>
        /// 根据时间获取测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetBXPoint(GetbxpointRequest realMessageRequest)
        {
            var getbxpointResponse = new BasicResponse<DataTable>();
            var dt = _Repository.GetBXPoint(realMessageRequest.Time);
            getbxpointResponse.Data = dt;
            return getbxpointResponse;
        }

        /// <summary>
        /// 保存测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> SavePoint(SavePointRequest realMessageRequest)
        {
            var savePointResponse = new BasicResponse<bool>();
            if (!realMessageRequest.Time.HasValue && string.IsNullOrWhiteSpace(realMessageRequest.PointStr))
            {
                savePointResponse.Data = false;
                savePointResponse.Code = -100;
                savePointResponse.Message = "操作失败！";
                return savePointResponse;
            }

            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    //根据时间删除标校
                    _Jc_BxRepositor.DeleteCalibrationDefByTime(realMessageRequest.Time.Value);

                    string[] points = realMessageRequest.PointStr.Split('|');
                    for (int i = 0; i < points.Length; i++)
                    {
                        var jc_bxModel = new Jc_BxModel();
                        jc_bxModel.ID = IdHelper.CreateLongId().ToString();
                        jc_bxModel.Point = points[i];
                        jc_bxModel.Timer = realMessageRequest.Time.Value;
                        _Jc_BxRepositor.AddCalibrationDef(jc_bxModel);
                    }
                    savePointResponse.Data = true;
                    savePointResponse.Code = 100;
                    savePointResponse.Message = "操作成功！";
                });
            }
            catch (Exception ex)
            {
                savePointResponse.Data = false;
                savePointResponse.Code = -100;
                savePointResponse.Message = ex.Message;
                this.ThrowException("保存测点", ex);
            }

            return savePointResponse;
        }

        /// <summary>
        /// 获取控制量测点号
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetKZPoint()
        {
            var response = new BasicResponse<DataTable>();
            DataTable dt = new DataTable();
            dt.TableName = "point";
            dt.Columns.Add("fzh", typeof(int));
            dt.Columns.Add("kh", typeof(int));
            dt.Columns.Add("point", typeof(string));
            try
            {
                var pointDefineCacheGetAllRequest = new PointDefineCacheGetByConditonRequest();
                pointDefineCacheGetAllRequest.Predicate = a => a.DevPropertyID == 3;//修改，直接查询控制量  20170720
                var pointDefineCacheGetAllResponse = pointDefineCacheService.GetPointDefineCache(pointDefineCacheGetAllRequest);
                if (pointDefineCacheGetAllResponse.Data != null && pointDefineCacheGetAllResponse.Data.Count > 0)
                {
                    foreach (var item in pointDefineCacheGetAllResponse.Data)
                    {
                        //if (item.DevProperty == "控制量")
                        //{
                        dt.Rows.Add(item.Fzh, item.Kh, item.Point);
                        //}
                    }
                    response.Data = dt;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取控制量测点号", ex);
            }
            if (dt.Rows.Count > 0)
            {
                DataView view = new DataView();
                view.Table = dt;
                view.Sort = "fzh,kh";
                dt = view.ToTable("point", true, "fzh", "kh", "point");
            }
            response.Data = dt;

            return response;
        }

        /// <summary>
        /// 获取分站测点号
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetFZPoint()
        {
            var response = new BasicResponse<DataTable>();
            DataTable dt = new DataTable();
            dt.TableName = "point";
            dt.Columns.Add("fzh", typeof(int));
            dt.Columns.Add("point", typeof(string));
            try
            {
                //var pointDefineCacheGetAllRequest = new PointDefineCacheGetByConditonRequest();
                //pointDefineCacheGetAllRequest.Predicate = a => a.DevPropertyID == 0;//修改，直接查询分站  20170720
                //var pointDefineCacheGetAllResponse = pointDefineCacheService.GetPointDefineCache(pointDefineCacheGetAllRequest);
                //多系统融合  20171123
                PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
                pointDefineCacheRequest.Predicate = a => a.DevPropertyID == 0;
                var pointDefineCacheGetAllResponse = allSystemPointDefineService.GetPointDefineCache(pointDefineCacheRequest);

                if (pointDefineCacheGetAllResponse.Data != null && pointDefineCacheGetAllResponse.Data.Count > 0)
                {
                    foreach (var item in pointDefineCacheGetAllResponse.Data)
                    {
                        //if (item.DevProperty == "分站")
                        //{
                        dt.Rows.Add(item.Fzh, item.Point);
                        //}
                    }
                    response.Data = dt;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取分站测点号", ex);
            }
            if (dt.Rows.Count > 0)
            {
                DataView view = new DataView();
                view.Table = dt;
                view.Sort = "fzh";
                dt = view.ToTable("point", true, "fzh", "point");

            }
            response.Data = dt;

            return response;
        }

        /// <summary>
        /// 根据测点获取结构体
        /// </summary>
        /// <returns></returns>
        public BasicResponse<Jc_DefInfo> GetPoint(GetPointRequest realMessageRequest)
        {
            var response = new BasicResponse<Jc_DefInfo>();
            try
            {
                //var pointDefineCacheGetByKeyRequest = new PointDefineCacheGetByKeyRequest() { Point = realMessageRequest.Point };
                //var pointDefineCacheGetByKeyResponse = pointDefineCacheService.GetPointDefineCacheByKey(pointDefineCacheGetByKeyRequest);
                //多系统融合  20171123
                PointDefineCacheGetByConditonRequest pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest();
                pointDefineCacheRequest.Predicate = a => a.Point == realMessageRequest.Point;
                var pointDefineCacheGetByKeyResponse = allSystemPointDefineService.GetPointDefineCache(pointDefineCacheRequest);
                if (pointDefineCacheGetByKeyResponse.Data != null && pointDefineCacheGetByKeyResponse.Data.Count > 0)
                {
                    response.Data = pointDefineCacheGetByKeyResponse.Data[0];
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据测点获取结构体", ex);
            }

            return response;
        }

        /// <summary>
        ///  根据测点号获取主控点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetZKPoint(GetZKPointRequest realMessageRequest)
        {
            var response = new BasicResponse<DataTable>();
            string point = realMessageRequest.Point;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow row;
            StateToClient statedata = new StateToClient();
            int fzh = int.Parse(point.Substring(0, 3));
            int nkh = int.Parse(point.Substring(4, 2));
            nkh--;
            int adr = 0;
            if (point.Contains("-"))
            {
                string[] temp = point.Split('-');
                adr = int.Parse(temp[temp.Length - 1]);
            }
            else
            {
                adr = int.Parse(point.Substring(6, 1));
            }
            if (adr == 1)
            {
                nkh += 8;
            }
            dt.TableName = "point";
            dt.Columns.Add("point", typeof(string));
            dt.Columns.Add("wz", typeof(string));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("kzlx", typeof(string));
            dt.Columns.Add("ssz", typeof(string));
            try
            {

                #region 是否有手动控制
                dt1 = _Repository.GetHandControlByPoint(point);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    row = dt.NewRow();
                    row["point"] = "--";
                    row["wz"] = "--";
                    row["type"] = "--";
                    row["ssz"] = "--";
                    row["kzlx"] = "--手动控制";
                    dt.Rows.Add(row);
                }
                #endregion

                var pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
                var pointDefineCacheGetAllResponse = pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheGetAllRequest);
                if (pointDefineCacheGetAllResponse.Data != null && pointDefineCacheGetAllResponse.Data.Count > 0)
                {
                    foreach (var item in pointDefineCacheGetAllResponse.Data)
                    {
                        #region 获取交叉控制
                        if ((item.Jckz1 != null && item.Jckz1.Contains(point)) || (item.Jckz2 != null && item.Jckz2.Contains(point)) || (item.Jckz3 != null && item.Jckz3.Contains(point)))
                        {
                            row = dt.NewRow();
                            row["point"] = item.Point;
                            row["wz"] = item.Wz;
                            row["type"] = item.DevName;
                            var deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                            var deviceDefineCacheGetByKeyResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
                            if (deviceDefineCacheGetByKeyResponse.Data != null && !row.IsNull("ssz"))
                            {
                                row["ssz"] = item.Ssz + " " + deviceDefineCacheGetByKeyResponse.Data.Xs1;
                            }
                            else
                            {
                                row["ssz"] = item.Ssz;
                            }
                            row["kzlx"] = "无";
                            if (item.DataState == statedata.EqpState11 && item.Jckz1 != null && item.Jckz1.Contains(point))
                            {
                                row["kzlx"] = "断电控制";
                            }
                            else if (item.DataState == statedata.EqpState21 && item.Jckz1 != null && item.Jckz1.Contains(point))
                            {
                                row["kzlx"] = "断电控制";
                            }
                            else if (item.DataState == statedata.EqpState15 && item.Jckz3 != null && item.Jckz3.Contains(point))
                            {
                                row["ssz"] = "上溢";
                                row["kzlx"] = "故障控制";
                            }
                            else if (item.DataState == statedata.EqpState16 && item.Jckz3 != null && item.Jckz3.Contains(point))
                            {
                                row["ssz"] = "负漂";
                                row["kzlx"] = "故障控制";
                            }
                            else if (item.DataState == statedata.EqpState13 && item.Jckz2 != null && item.Jckz2.Contains(point))
                            {
                                row["ssz"] = "断线";
                                row["kzlx"] = "断线控制";
                            }
                            else if (item.DataState == statedata.EqpState24 && item.Jckz1 != null && item.Jckz1.Contains(point))
                            {
                                row["kzlx"] = "0态控制";
                            }
                            else if (item.DataState == statedata.EqpState25 && item.Jckz2 != null && item.Jckz2.Contains(point))
                            {
                                row["kzlx"] = "1态控制";
                            }
                            else if (item.DataState == statedata.EqpState26 && item.Jckz3 != null && item.Jckz3.Contains(point))
                            {
                                row["kzlx"] = "2态控制";
                            }
                            dt.Rows.Add(row);
                        }
                        #endregion

                        #region 判断是否为开关量逻辑控制
                        #endregion

                        #region 获取本地控制
                        if (item.Fzh == fzh && item.DevProperty == "模拟量")
                        {
                            if ((item.K1 & (1 << nkh)) == (1 << nkh) || (item.K2 & (1 << nkh)) == (1 << nkh) || (item.K3 & (1 << nkh)) == (1 << nkh) || (item.K4 & (1 << nkh)) == (1 << nkh) || (item.K5 & (1 << nkh)) == (1 << nkh) || (item.K6 & (1 << nkh)) == (1 << nkh) || (item.K7 & (1 << nkh)) == (1 << nkh))
                            {
                                row = dt.NewRow();
                                row["point"] = item.Point;
                                row["wz"] = item.Wz;
                                row["type"] = item.DevName;

                                var deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                                var deviceDefineCacheGetByKeyResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
                                if (deviceDefineCacheGetByKeyResponse.Data != null && !row.IsNull("ssz"))
                                {
                                    row["ssz"] = item.Ssz + " " + deviceDefineCacheGetByKeyResponse.Data.Xs1;
                                }
                                else
                                {
                                    row["ssz"] = item.Ssz;
                                }
                                row["kzlx"] = "无";
                                if ((item.DataState == statedata.EqpState9 || item.DataState == statedata.EqpState11) && ((item.K1 & (1 << nkh)) == (1 << nkh)))
                                {
                                    row["kzlx"] = "上限报警控制";
                                }
                                else if (item.DataState == statedata.EqpState11 && (item.K2 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "上限断电控制";
                                }
                                else if ((item.DataState == statedata.EqpState19 || item.DataState == statedata.EqpState21) && ((item.K3 & (1 << nkh)) == (1 << nkh)))
                                {
                                    row["kzlx"] = "下限报警控制";
                                }
                                else if (item.DataState == statedata.EqpState21 && (item.K4 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "下限断电控制";
                                }
                                else if (item.DataState == statedata.EqpState15 && (item.K5 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "上溢控制";
                                }
                                else if (item.DataState == statedata.EqpState16 && (item.K6 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "负漂控制";
                                }
                                else if (item.DataState == statedata.EqpState13 && (item.K7 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "断线控制";
                                }
                                dt.Rows.Add(row);
                            }
                        }

                        if (item.Fzh == fzh && item.DevProperty == "开关量")
                        {
                            if ((item.K1 & (1 << nkh)) == (1 << nkh) || (item.K2 & (1 << nkh)) == (1 << nkh) || (item.K3 & (1 << nkh)) == (1 << nkh))
                            {
                                row = dt.NewRow();
                                row["point"] = item.Point;
                                row["wz"] = item.Wz;
                                row["type"] = item.DevName;
                                row["ssz"] = item.Ssz;
                                row["kzlx"] = "无";
                                if (item.DataState == statedata.EqpState24 && (item.K1 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "0态控制";
                                }
                                else if (item.DataState == statedata.EqpState25 && (item.K2 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "1态控制";
                                }
                                else if (item.DataState == statedata.EqpState26 && (item.K3 & (1 << nkh)) == (1 << nkh))
                                {
                                    row["kzlx"] = "2态控制";
                                }
                                dt.Rows.Add(row);
                            }
                        }
                        #endregion
                    }
                }
                response.Data = dt;
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据测点号获取主控点", ex);
            }

            return response;
        }

        /// <summary>
        /// 获取主控点
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetZKPointex()
        {
            var BasicResponse = new BasicResponse<DataTable>();
            Jc_DefInfo obj = null;
            Jc_DefInfo kdObj = null;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataRow row;
            StateToClient statedata = new StateToClient();
            int fzh, nkh, adr;
            string point;
            string kdpoint;
            string wz = "";
            try
            {
                dt.TableName = "point";
                dt.Columns.Add("point", typeof(string));
                dt.Columns.Add("type", typeof(string));
                dt.Columns.Add("wz", typeof(string));
                dt.Columns.Add("kzd", typeof(string));
                dt.Columns.Add("kztype", typeof(string));//增加定义控制类型
                dt.Columns.Add("qy", typeof(string));
                dt.Columns.Add("sd", typeof(string));
                dt.Columns.Add("sf", typeof(string));
                dt.Columns.Add("kzlx", typeof(string));
                dt.Columns.Add("ssz", typeof(string));
                dt.Columns.Add("kdpoint", typeof(string));  //2018.3.7 by
                dt.Columns.Add("kdwz", typeof(string));  //2018.3.7 by
                var request = new PointDefineCacheGetAllRequest();
                var response = pointDefineCacheService.GetAllPointDefineCache(request);
                if (response.Data == null && response.Data.Count <= 0)
                {
                    BasicResponse.Data = dt;
                    return BasicResponse;
                }
                Dictionary<string, Jc_DefInfo> _JC_DEF = response.Data.FindAll(a => a.Activity == "1").ToList().ToDictionary(k => k.Point, v => v);
                foreach (string key1 in _JC_DEF.Keys)
                {
                    if (key1.Contains('C'))
                    {
                        point = key1;
                        fzh = int.Parse(point.Substring(0, 3));
                        nkh = int.Parse(point.Substring(4, 2));
                        nkh--;
                        adr = int.Parse(point.Substring(6, 1));
                        if (adr == 1)
                        {
                            nkh += 8;
                        }
                        try
                        {
                            #region
                            if (_JC_DEF.ContainsKey(point))
                            {
                                wz = _JC_DEF[point].Wz;
                            }
                            foreach (string key in _JC_DEF.Keys)
                            {
                                if (key.Contains('D') || key.Contains('A'))
                                {
                                    if (_JC_DEF.ContainsKey(key))
                                    {
                                        obj = _JC_DEF[key];
                                        #region 获取交叉控制
                                        if ((obj.Jckz1 != null && obj.Jckz1.Contains(point))
                                            || (obj.Jckz2 != null && obj.Jckz2.Contains(point))
                                            || (obj.Jckz3 != null && obj.Jckz3.Contains(point)))
                                        {
                                            row = dt.NewRow();
                                            row["point"] = obj.Point;
                                            row["wz"] = obj.Wz;
                                            row["type"] = obj.DevName;
                                            row["sd"] = obj.Z3;
                                            row["sf"] = obj.Z4;
                                            row["qy"] = wz;
                                            row["kzd"] = point;

                                            Jc_DevInfo tempdev = null;
                                            var devRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = obj.Devid };
                                            var devResponse = deviceDefineCacheService.GetPointDefineCacheByKey(devRequest);
                                            if (devResponse != null)
                                            {
                                                tempdev = devResponse.Data;
                                            }
                                            if (key.Contains('A') && tempdev != null && !row.IsNull("ssz"))
                                            {
                                                row["ssz"] = obj.Ssz + " " + tempdev.Xs1;
                                            }
                                            else
                                            {
                                                row["ssz"] = obj.Ssz;
                                            }
                                            row["kzlx"] = "无";
                                            row["kztype"] = "";
                                            if (obj.Jckz1 != null)
                                            {
                                                row["kztype"] += "交叉-断电控制,";
                                            }
                                            //if (obj.Jckz3 != null)//无故障控制，不显示   20170914
                                            //{
                                            //    row["kztype"] += "交叉-故障控制,";
                                            //}
                                            if (obj.Jckz2 != null)
                                            {
                                                row["kztype"] += "交叉-断线控制,";
                                            }
                                            if (obj.DataState == statedata.EqpState11 && obj.Jckz1 != null && obj.Jckz1.Contains(point))
                                            {
                                                row["kzlx"] = "断电控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState21 && obj.Jckz1 != null && obj.Jckz1.Contains(point))
                                            {
                                                row["kzlx"] = "断电控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState15 && obj.Jckz3 != null && obj.Jckz3.Contains(point))
                                            {
                                                row["ssz"] = "上溢";
                                                row["kzlx"] = "故障控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState16 && obj.Jckz3 != null && obj.Jckz3.Contains(point))
                                            {
                                                row["ssz"] = "负漂";
                                                row["kzlx"] = "故障控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState13 && obj.Jckz2 != null && obj.Jckz2.Contains(point))
                                            {
                                                row["ssz"] = "断线";
                                                row["kzlx"] = "断线控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState24 && obj.Jckz1 != null && obj.Jckz1.Contains(point))
                                            {
                                                row["kzlx"] = "0态控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState25 && obj.Jckz2 != null && obj.Jckz2.Contains(point))
                                            {
                                                row["kzlx"] = "1态控制";
                                            }
                                            else if (obj.DataState == statedata.EqpState26 && obj.Jckz3 != null && obj.Jckz3.Contains(point))
                                            {
                                                row["kzlx"] = "2态控制";
                                            }

                                            //2018.3.7 by
                                            kdObj = GetKdPoint(_JC_DEF[key1], _JC_DEF);
                                            if (kdObj != null)
                                            {
                                                row["kdpoint"] = kdObj.Point;
                                                row["kdwz"] = kdObj.Wz;
                                            }
                                            else
                                            {
                                                row["kdpoint"] = "";
                                                row["kdwz"] = "";
                                            }
                                            dt.Rows.Add(row);
                                        }
                                        #endregion

                                        #region 获取本地控制

                                        if (obj.Fzh == fzh && obj.DevProperty == "模拟量")
                                        {
                                            //nkh = obj.Kh;

                                            if ((obj.K1 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K2 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K3 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K4 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K5 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K6 & (1 << nkh)) == (1 << nkh) ||
                                              (obj.K7 & (1 << nkh)) == (1 << nkh))
                                            {
                                                row = dt.NewRow();
                                                row["point"] = obj.Point;
                                                row["wz"] = obj.Wz;
                                                row["type"] = obj.DevName;
                                                row["sd"] = obj.Z3;
                                                row["sf"] = obj.Z4;
                                                row["qy"] = wz;
                                                row["kzd"] = point;
                                                Jc_DevInfo tempdev = null;
                                                var devRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = obj.Devid };
                                                var devResponse = deviceDefineCacheService.GetPointDefineCacheByKey(devRequest);
                                                if (devResponse != null)
                                                {
                                                    tempdev = devResponse.Data;
                                                }
                                                if (tempdev != null && !row.IsNull("ssz"))
                                                {
                                                    row["ssz"] = obj.Ssz + " " + tempdev.Xs1;
                                                }
                                                else
                                                {
                                                    row["ssz"] = obj.Ssz;
                                                }
                                                row["kztype"] = "";
                                                if (((obj.K1 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kztype"] += "上限报警控制,";
                                                }
                                                if ((obj.K2 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "上限断电控制,";
                                                }
                                                if (((obj.K3 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kztype"] += "下限报警控制,";
                                                }
                                                if ((obj.K4 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "下限断电控制,";
                                                }
                                                if ((obj.K5 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "上溢控制,";
                                                }
                                                if ((obj.K6 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "负漂控制,";
                                                }
                                                if ((obj.K7 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "断线控制,";
                                                }
                                                row["kzlx"] = "无";
                                                if ((obj.DataState == statedata.EqpState9 || obj.DataState == statedata.EqpState11) && ((obj.K1 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kzlx"] = "上限报警控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState11 && (obj.K2 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "上限断电控制";
                                                }
                                                else if ((obj.DataState == statedata.EqpState19 || obj.DataState == statedata.EqpState21) && ((obj.K3 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kzlx"] = "下限报警控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState21 && (obj.K4 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "下限断电控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState15 && (obj.K5 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "上溢控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState16 && (obj.K6 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "负漂控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState13 && (obj.K7 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "断线控制";
                                                }

                                                //2018.3.7 by
                                                kdObj = GetKdPoint(_JC_DEF[key1], _JC_DEF);
                                                if (kdObj != null)
                                                {
                                                    row["kdpoint"] = kdObj.Point;
                                                    row["kdwz"] = kdObj.Wz;
                                                }
                                                else
                                                {
                                                    row["kdpoint"] = "";
                                                    row["kdwz"] = "";
                                                }

                                                dt.Rows.Add(row);
                                            }
                                        }

                                        if (obj.Fzh == fzh && obj.DevProperty == "开关量")
                                        {
                                            //nkh = obj.Kh;

                                            if ((obj.K1 & (1 << nkh)) == (1 << nkh) ||
                                                (obj.K2 & (1 << nkh)) == (1 << nkh) ||
                                                (obj.K3 & (1 << nkh)) == (1 << nkh))
                                            {
                                                row = dt.NewRow();
                                                row["point"] = obj.Point;
                                                row["wz"] = obj.Wz;
                                                row["type"] = obj.DevName;
                                                row["sd"] = obj.Z3;
                                                row["sf"] = obj.Z4;
                                                row["ssz"] = obj.Ssz;
                                                row["kzlx"] = "无";
                                                row["qy"] = wz;
                                                row["kzd"] = point;

                                                row["kztype"] = "";
                                                if (((obj.K1 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kztype"] += "0态控制,";
                                                }
                                                if ((obj.K2 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kztype"] += "1态控制,";
                                                }
                                                if (((obj.K3 & (1 << nkh)) == (1 << nkh)))
                                                {
                                                    row["kztype"] += "2态控制,";
                                                }
                                                if (obj.DataState == statedata.EqpState24 && (obj.K1 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "0态控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState25 && (obj.K2 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "1态控制";
                                                }
                                                else if (obj.DataState == statedata.EqpState26 && (obj.K3 & (1 << nkh)) == (1 << nkh))
                                                {
                                                    row["kzlx"] = "2态控制";
                                                }

                                                //2018.3.7 by
                                                kdObj = GetKdPoint(_JC_DEF[key1], _JC_DEF);
                                                if (kdObj != null)
                                                {
                                                    row["kdpoint"] = kdObj.Point;
                                                    row["kdwz"] = kdObj.Wz;
                                                }
                                                else
                                                {
                                                    row["kdpoint"] = "";
                                                    row["kdwz"] = "";
                                                }

                                                dt.Rows.Add(row);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(" 获取主控点", ex);
                        }
                    }
                }

                //2018.3.8 by
                //foreach (string key1 in _JC_DEF.Keys)
                //{
                //    if (key1.Contains('A') || key1.Contains('D'))
                //    {
                //        if (dt.Select("point = '" + key1 + "'").Length == 0)
                //        {
                //            obj = _JC_DEF[key1];
                //            if (obj.DevClassID == 13) { continue; }//流量传感器不算
                //            row = dt.NewRow();
                //            row["point"] = obj.Point;
                //            row["wz"] = obj.Wz;
                //            row["type"] = obj.DevName;
                //            row["sd"] = obj.Z3;
                //            row["sf"] = obj.Z4;
                //            row["qy"] = wz;
                //            row["kzd"] = "";

                //            Jc_DevInfo tempdev = null;
                //            var devRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = obj.Devid };
                //            var devResponse = deviceDefineCacheService.GetPointDefineCacheByKey(devRequest);
                //            if (devResponse != null)
                //            {
                //                tempdev = devResponse.Data;
                //            }
                //            if (key1.Contains('A') && tempdev != null && !row.IsNull("ssz"))
                //            {
                //                row["ssz"] = obj.Ssz + " " + tempdev.Xs1;
                //            }
                //            else
                //            {
                //                row["ssz"] = obj.Ssz;
                //            }
                //            row["kzlx"] = "无";
                //            row["kztype"] = "";
                //            row["kdpoint"] = "";
                //            row["kdwz"] = "";
                //            dt.Rows.Add(row);
                //        }
                //    }
                //}

                dt.DefaultView.Sort = "point";
                BasicResponse.Data = dt.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                LogHelper.Error(" 获取主控点1", ex);
            }
            return BasicResponse;

        }

        private Jc_DefInfo GetKdPoint(Jc_DefInfo kzpoint, Dictionary<string, Jc_DefInfo> _JC_DEF)
        {
            Jc_DefInfo def = null;
            #region ----获取馈电点----
            string key1 = kzpoint.Point;
            if (_JC_DEF[key1].K1 > 0 && _JC_DEF[key1].K2 > 0)
            {
                string kdpoint = _JC_DEF[key1].K1.ToString().PadLeft(3, '0') + "D" + _JC_DEF[key1].K2.ToString().PadLeft(2, '0') + _JC_DEF[key1].K4.ToString();
                if (_JC_DEF.ContainsKey(kdpoint))
                {
                    def = _JC_DEF[kdpoint];
                }
            }
            #endregion
            return def;
        }

        /// <summary>
        /// 获取分站交叉控制
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetFZJXControl(GetFZJXControlRequest realMessageRequest)
        {
            var response = new BasicResponse<DataTable>();
            DataTable dt = new DataTable();
            string[] strkz;
            DataRow row;
            StateToClient statedata = new StateToClient();
            dt.Columns.Add("zk", typeof(string));
            dt.Columns.Add("zktype", typeof(string));
            dt.Columns.Add("wz", typeof(string));
            dt.Columns.Add("kzlx", typeof(string));
            dt.Columns.Add("state", typeof(string));
            dt.Columns.Add("bk", typeof(string));
            dt.Columns.Add("bkwz", typeof(string));
            dt.TableName = "point";
            try
            {
                var pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
                var pointDefineCacheGetAllResponse = pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheGetAllRequest);
                if (pointDefineCacheGetAllResponse.Data != null && pointDefineCacheGetAllResponse.Data.Count > 0)
                {
                    foreach (var item in pointDefineCacheGetAllResponse.Data)
                    {
                        if ((item.DevPropertyID == 1 || item.DevPropertyID == 2) && item.Fzh == realMessageRequest.Fzh)
                        {
                            #region 获取断电交叉控制/0态控制  20170329
                            if (!string.IsNullOrEmpty(item.Jckz1))
                            {
                                strkz = item.Jckz1.Split('|');
                                if (strkz.Length > 0)
                                {
                                    for (int i = 0; i < strkz.Length; i++)
                                    {
                                        row = dt.NewRow();
                                        row["zk"] = item.Point;
                                        row["zktype"] = item.DevName;
                                        row["wz"] = item.Wz;
                                        if (item.DevPropertyID == 2)
                                        {
                                            row["kzlx"] = "0态控制";
                                        }
                                        else
                                        {
                                            row["kzlx"] = "断电控制";
                                        }
                                        //var deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                                        //var deviceDefineCacheGetByKeyResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
                                        //if (deviceDefineCacheGetByKeyResponse.Data != null)
                                        //{
                                        //if (item.State == statedata.EqpState24)
                                        //{
                                        //    row["state"] = item.Bz6;
                                        //}
                                        //else if (item.State == statedata.EqpState25)
                                        //{
                                        //    row["state"] = item.Bz7;
                                        //}
                                        //else
                                        //{
                                        //    row["state"] = item.Bz8;
                                        //}
                                        //}
                                        row["state"] = "未控制";
                                        if (item.DataState == statedata.EqpState21 || item.DataState == statedata.EqpState11)
                                        {
                                            row["state"] = "已控制";
                                        }
                                        if (item.DataState == statedata.EqpState24)
                                        {
                                            row["state"] = "已控制";
                                        }
                                        row["bk"] = strkz[i];
                                        row["bkwz"] = item.Wz;
                                        dt.Rows.Add(row);
                                    }

                                }
                            }
                            #endregion

                            #region 获取断线交叉控制/1态控制  20170329
                            if (!string.IsNullOrEmpty(item.Jckz2))
                            {
                                strkz = item.Jckz2.Split('|');
                                if (strkz.Length > 0)
                                {
                                    for (int i = 0; i < strkz.Length; i++)
                                    {
                                        row = dt.NewRow();
                                        row["zk"] = item.Point;
                                        row["zktype"] = item.DevName;
                                        row["wz"] = item.Wz;
                                        if (item.DevPropertyID == 2)
                                        {
                                            row["kzlx"] = "1态控制";
                                        }
                                        else
                                        {
                                            row["kzlx"] = "断线控制";
                                        }
                                        //var deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                                        //var deviceDefineCacheGetByKeyResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
                                        //if (deviceDefineCacheGetByKeyResponse.Data != null)
                                        //{
                                        //    if (item.State == statedata.EqpState24)
                                        //    {
                                        //        row["state"] = deviceDefineCacheGetByKeyResponse.Data.Xs1;
                                        //    }
                                        //    else
                                        //    {
                                        //        row["state"] = deviceDefineCacheGetByKeyResponse.Data.Xs2;
                                        //    }
                                        //}
                                        //if (item.State == statedata.EqpState24)
                                        //{
                                        //    row["state"] = item.Bz6;
                                        //}
                                        //else if (item.State == statedata.EqpState25)
                                        //{
                                        //    row["state"] = item.Bz7;
                                        //}
                                        //else
                                        //{
                                        //    row["state"] = item.Bz8;
                                        //}
                                        row["state"] = "未控制";
                                        if (item.DataState == statedata.EqpState13)
                                        {
                                            row["state"] = "已控制";
                                        }
                                        if (item.DataState == statedata.EqpState25)
                                        {
                                            row["state"] = "已控制";
                                        }
                                        row["bk"] = strkz[i];
                                        row["bkwz"] = item.Wz;
                                        dt.Rows.Add(row);
                                    }
                                }
                            }
                            #endregion

                            #region 获取故障交叉控制/2态控制  20170329
                            if (!string.IsNullOrEmpty(item.Jckz3))
                            {
                                strkz = item.Jckz3.Split('|');
                                if (strkz.Length > 0)
                                {
                                    for (int i = 0; i < strkz.Length; i++)
                                    {
                                        row = dt.NewRow();
                                        row["zk"] = item.Point;
                                        row["zktype"] = item.DevName;
                                        row["wz"] = item.Wz;
                                        if (item.DevPropertyID == 2)
                                        {
                                            row["kzlx"] = "2态控制";
                                        }
                                        else
                                        {
                                            //row["kzlx"] = "故障控制"; //不支持故障控制  20170715
                                        }
                                        //var deviceDefineCacheGetByKeyRequest = new DeviceDefineCacheGetByKeyRequest() { Devid = item.Devid };
                                        //var deviceDefineCacheGetByKeyResponse = deviceDefineCacheService.GetPointDefineCacheByKey(deviceDefineCacheGetByKeyRequest);
                                        //if (deviceDefineCacheGetByKeyResponse.Data != null)
                                        //{
                                        //    if (item.State == statedata.EqpState24)
                                        //    {
                                        //        row["state"] = deviceDefineCacheGetByKeyResponse.Data.Xs1;
                                        //    }
                                        //    else
                                        //    {
                                        //        row["state"] = deviceDefineCacheGetByKeyResponse.Data.Xs2;
                                        //    }
                                        //}
                                        //if (item.State == statedata.EqpState24)
                                        //{
                                        //    row["state"] = item.Bz6;
                                        //}
                                        //else if (item.State == statedata.EqpState25)
                                        //{
                                        //    row["state"] = item.Bz7;
                                        //}
                                        //else
                                        //{
                                        //    row["state"] = item.Bz8;
                                        //}
                                        row["state"] = "未控制";
                                        if (item.DataState == statedata.EqpState26)
                                        {
                                            row["state"] = "已控制";
                                        }
                                        row["bk"] = strkz[i];
                                        row["bkwz"] = item.Wz;
                                        dt.Rows.Add(row);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    response.Data = dt;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取分站交叉控制", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取服务端时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DateTime> GetTimeNow()
        {
            var response = new BasicResponse<DateTime>();
            response.Data = DateTime.Now;
            return response;
        }

        /// <summary>
        /// 存储配置信息到config表
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> SaveConfig(SaveConfigRequest realMessageRequest)
        {
            var response = new BasicResponse<bool>();
            if (realMessageRequest.ConfigInfoList == null || realMessageRequest.ConfigInfoList.Count <= 0)
            {
                response.Code = -100;
                response.Data = false;
                response.Message = "参数错误！";
                return response;
            }

            if (realMessageRequest.ConfigInfoList != null && realMessageRequest.ConfigInfoList.Count > 0)
            {
                try
                {
                    foreach (var item in realMessageRequest.ConfigInfoList)
                    {
                        var settingModel = _SettingRepository.GetSettingByKey(item.Name);
                        if (settingModel != null)
                        {
                            settingModel.StrKey = item.Name;
                            settingModel.StrValue = item.Text;
                            settingModel.LastUpdateDate = DateTime.Now.ToString();
                            _SettingRepository.Update(settingModel);
                        }
                        else
                        {
                            var addSettingModel = new SettingModel();
                            addSettingModel.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                            addSettingModel.StrType = "实时显示配置";
                            addSettingModel.StrKey = item.Name;
                            addSettingModel.StrKeyCHs = "实时显示配置";
                            addSettingModel.StrValue = item.Text;
                            addSettingModel.LastUpdateDate = DateTime.Now.ToString();
                            _SettingRepository.AddSetting(addSettingModel);
                        }
                    }

                    response.Code = 100;
                    response.Data = true;
                    response.Message = "操作成功！";
                }
                catch (Exception ex)
                {
                    response.Code = -100;
                    response.Data = false;
                    response.Message = ex.Message;
                    this.ThrowException("存储配置信息到config表", ex);
                }
            }

            return response;
        }

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest realMessageRequest)
        {
            var response = new BasicResponse<bool>();
            if (!realMessageRequest.Page.HasValue || realMessageRequest.dt == null)
            {
                response.Code = -100;
                response.Data = false;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                _Repository.SaveCustomPagePoints(realMessageRequest.Page.Value, realMessageRequest.dt);
                response.Data = true;
                response.Message = "操作成功！";

            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Data = false;
                response.Message = ex.Message;
                this.ThrowException("存储自定义测点", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取所有绑定电源箱的交换机
        /// </summary>
        /// <returns></returns>
        public BasicResponse<DataTable> GetBindDianYuanMac()
        {
            var response = new BasicResponse<DataTable>();
            DataTable msg = new DataTable();
            msg.Columns.Add("mac", typeof(string));
            msg.Columns.Add("wz", typeof(string));
            msg.Columns.Add("fd", typeof(int));
            msg.TableName = "fz";
            try
            {
                var networkModuleCacheGetAllRequest = new NetworkModuleCacheGetAllRequest();
                var networkModuleCacheGetAllRespone = networkModuleCacheService.GetAllNetworkModuleCache(networkModuleCacheGetAllRequest);
                if (networkModuleCacheGetAllRespone.Data != null && networkModuleCacheGetAllRespone.Data.Count > 0)
                {
                    foreach (var item in networkModuleCacheGetAllRespone.Data)
                    {
                        if (item.Type == 0 && !string.IsNullOrEmpty(item.Wz) && item.Bz4 == "1")
                        {
                            msg.Rows.Add(item.MAC, item.Wz, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("获取所有绑定电源箱分站", ex);
            }
            return response;
        }

        /// <summary>
        ///  根据测点号获取历史维保记录
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetMaintenanceHistoryByPointId(GetMaintenanceHistoryByPointIdRequst realMessageRequest)
        {
            var response = new BasicResponse<DataTable>();
            if (!realMessageRequest.PointId.HasValue)
            {
                response.Code = -100;
                response.Message = "参数错误！";
                return response;
            }
            try
            {
                response.Data = _Repository.GetMaintenanceHistoryByPointId(realMessageRequest.PointId.Value);
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("根据测点号获取历史维保记录", ex);
            }

            return response;
        }


        /// <summary>
        /// 测点总表初始化
        /// </summary>
        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.TableName = "point";
            #region 初始化
            DataColumn clm = new DataColumn();
            clm.ColumnName = "point";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "pointid";
            clm.DataType = typeof(long);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "fzh";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "tdh";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "dzh";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "lb";//类别  详细类别
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "wz";//测点名称
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "dw";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "ssz";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "zt";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            //电压等级
            clm = new DataColumn();
            clm.ColumnName = "voltage";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sbzt";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "bj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "lx";//类型 模拟量 开关量
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "lxtype";//类型 模拟量 开关量
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xh";//类型 模拟量 开关量
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xhtype";//类型 模拟量 开关量
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "zl";//种类
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "devname";//设备名称
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sxbj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sxdd";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xxbj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xxdd";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sxyj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sxfd";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xxyj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "xxfd";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "time";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "0t";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "1t";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "2t";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "0tcolor";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "1tcolor";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "2tcolor";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "statecolor";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "sszcolor";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "dldj";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "k1";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "k2";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "k3";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            //新增用来处理开关量的数据状态
            clm = new DataColumn();
            clm.ColumnName = "k8";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "NCtrlSate";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);          
            clm = new DataColumn();
            clm.ColumnName = "GradingAlarmLevel";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);
            clm = new DataColumn();
            clm.ColumnName = "StationDyType";
            clm.DataType = typeof(string);
            dt.Columns.Add(clm);   
            #endregion
            return dt;
        }

        /// <summary>
        ///  获取定义改变时间 判断定义是否改变
        /// </summary>
        /// <returns></returns>
        public BasicResponse<string> GetDefineChangeFlg()
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.DefUpdateTimeKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.DefUpdateTimeKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取定义改变时间", ex);
            }
            return response;
        }

        /// <summary>
        /// 获取显示配置改变时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse<string> GetRealCfgChangeFlg()
        {
            var response = new BasicResponse<string>();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.RealListCfgUpdateTimeKey))
                {
                    response.Data = Basic.Framework.Data.PlatRuntime.Items[KeyConst.RealListCfgUpdateTimeKey].ToString();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取显示配置改变时间", ex);
            }
            return response;
        }

        /// <summary>
        /// 设置显示配置改变时间
        /// </summary>
        /// <returns></returns>
        public BasicResponse SetRealCfgChange()
        {
            var response = new BasicResponse();
            try
            {
                if (Basic.Framework.Data.PlatRuntime.Items[KeyConst.RealListCfgUpdateTimeKey] != null)
                {
                    Basic.Framework.Data.PlatRuntime.Items[KeyConst.RealListCfgUpdateTimeKey] = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                response.Code = -100;
                response.Message = ex.Message;
                this.ThrowException("设置显示配置改变时间", ex);
            }
            return response;
        }

        /// <summary>
        /// 根据Counter获取运行记录信息
        /// </summary>
        /// <param name="realMessageRequest"></param>
        /// <returns></returns>
        public BasicResponse<List<Jc_RInfo>> GetRunRecordListByCounter(GetRunRecordListByCounterRequest realMessageRequest)
        {
            BasicResponse<List<Jc_RInfo>> result = new BasicResponse<List<Jc_RInfo>>();

            RunLogCacheGetByConditionRequest runLogCacheRequest = new RunLogCacheGetByConditionRequest();
            runLogCacheRequest.Pridicate = a => a.Counter > realMessageRequest.Counter;//去掉等于
            result.Data = runLogCacheService.GetRunLogCache(runLogCacheRequest).Data;

            return result;
        }
    }
}
