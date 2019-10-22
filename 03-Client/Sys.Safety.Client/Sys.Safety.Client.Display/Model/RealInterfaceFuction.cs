using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Request;
using Sys.Safety.Request.RealMessage;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Jc_Mc;
using Sys.Safety.Request.NetworkModule;
using Basic.Framework.Logging;
using Sys.Safety.Request.R_Phistory;

namespace Sys.Safety.Client.Display.Model
{
    public class RealInterfaceFuction
    {
        private static IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();
        private static IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        private static INetworkModuleService jc_MacService = ServiceFactory.Create<INetworkModuleService>();
        private static IManualCrossControlService manualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        private static IR_PhistoryService r_PhistoryService = ServiceFactory.Create<IR_PhistoryService>();
        static ISettingService _SettingService = ServiceFactory.Create<ISettingService>();

        /// <summary>
        /// 最后更新实时值时间
        /// </summary>
        public static DateTime lastRefreshRealDataTime = new DateTime(1900, 01, 01);


        /// <summary>
        ///  20170104
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SettingInfo GetConfigFKey(string key)
        {
            SettingInfo res = null;
            List<SettingInfo> sets = _SettingService.GetSettingList().Data;
            if (sets != null)
            {
                for (int i = 0; i < sets.Count; i++)
                {
                    if (sets[i].StrKey == key)
                    {
                        res = sets[i];
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 发送命令到服务端
        /// </summary>
        /// <returns></returns>
        public static void RemoteUpgradeCommand(string fzh, byte cmd, byte cmd1, byte cmd2)
        {
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new RemoteUpgradeCommandRequest();
                    request.Fzh = fzh;
                    request.SendD = cmd;
                    request.Sjml = cmd1;
                    request.SjState = cmd2;
                    var response = realMessageService.RemoteUpgradeCommand(request);
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                OprFuction.SetServerConct();
            }
        }

        /// <summary>
        /// 远程升级，获取升级分站的状态
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static DataTable RemoteGetShowTb(string fzh)
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new RemoteGetShowTbRequest() { Fzh = fzh };
                    var response = realMessageService.RemoteGetShowTb(request);
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 开始升级过程或者结束升级过程
        /// </summary>
        /// <param name="i">0-开始 1-结束过程并结束文件下发</param>
        /// <returns></returns>
        public static bool RemoteUpdateStrtOrStop(int i)
        {
            bool flg = false;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new RemoteUpdateStrtOrStopRequest() { Type = i };
                    var response = realMessageService.RemoteUpdateStrtOrStop(request);
                    flg = response.Data;
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);

                OprFuction.SetServerConct();
            }
            return flg;
        }

        /// <summary>
        /// 获取服务端时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerNowTime()
        {
            DateTime nowtime = DateTime.Now;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetTimeNow();
                    nowtime = response.Data;
                }
            }
            catch
            {
                OprFuction.SetServerConct();
                nowtime = DateTime.Now;
            }
            return nowtime;
        }

        /// <summary>
        /// 存储配置信息到config表
        /// </summary>
        /// <param name="dtos">配置结构体链表</param>
        /// <returns></returns>
        public static bool SaveConfig(List<ConfigInfo> dtos)
        {
            bool flg = false;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new SaveConfigRequest() { ConfigInfoList = dtos };
                    var response = realMessageService.SaveConfig(request);
                    flg = response.Data;
                }
            }
            catch (Exception ex)
            {
                OprFuction.SetServerConct();
                flg = false;
            }
            return flg;
        }
        /// <summary>
        /// 设置显示配置改变时间
        /// </summary>
        /// <returns></returns>
        public static void SetRealCfgChange()
        {
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.SetRealCfgChange();
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                OprFuction.SetServerConct();
            }
        }
        /// <summary>
        /// 读取配置信息到config表
        /// </summary>
        /// <param name="name">键名</param>
        /// <returns></returns>
        public static string ReadConfig(string name)
        {
            string msg = "";
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new ReadConfigRequest() { KeyName = name };
                    var response = realMessageService.ReadConfig(request);
                    if (response.Data != null)
                    {
                        msg = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);

                OprFuction.SetServerConct();
            }
            return msg;
        }

        /// <summary>
        /// 获取自定义编排的测点号
        /// </summary>
        /// <param name="page">页面号</param>
        /// <returns></returns>
        public static DataTable GetCustomPagePoint(int page)
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetCustomPagePointRequest() { Page = page };
                    var response = realMessageService.GetCustomPagePoint(request);
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);

                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="page">页面号</param>
        /// <param name="dt">表</param>
        /// <returns></returns>
        public static bool SaveCustomPagePoints(int page, DataTable dt)
        {
            bool flg = false;
            try
            {
                if (StaticClass.ServerConet)
                {
                    dt.TableName = "point";
                    var request = new SaveCustomPagePointsRequest();
                    request.Page = page;
                    request.dt = dt;
                    var response = realMessageService.SaveCustomPagePoints(request);
                    flg = response.Data;
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                OprFuction.SetServerConct();
            }
            return flg;
        }

        /// <summary>
        /// 获取定义信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllPoint()
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetAllPointinformation();
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
                dt = CreateDT();
            }
            return dt;
        }

        /// <summary>
        /// 获取所有实时数据
        /// </summary>
        /// <returns></returns>
        //public static DataTable GetRealData()
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        if (StaticClass.ServerConet)
        //        {
        //            var response = realMessageService.GetRealData();
        //            if (response.Data != null)
        //            {
        //                dt = response.Data;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        OprFuction.SetServerConct();
        //    }
        //    return dt;
        //}
        /// <summary>
        /// 修改为增量获取  20170719
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRealData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "point";
            DataRow row;
            StateToClient state = new StateToClient();
            dt.Columns.Add("point", typeof(string));
            dt.Columns.Add("bj", typeof(string));
            dt.Columns.Add("ssz", typeof(string));
            dt.Columns.Add("zt", typeof(string));
            dt.Columns.Add("sbzt", typeof(string));
            dt.Columns.Add("voltage", typeof(string));
            dt.Columns.Add("dldj", typeof(string));
            dt.Columns.Add("time", typeof(string));
            dt.Columns.Add("NCtrlSate", typeof(string));
            dt.Columns.Add("GradingAlarmLevel", typeof(string));
            dt.Columns.Add("StationDyType", typeof(string));
            try
            {
                if (StaticClass.ServerConet)
                {
                    GetRealDataRequest request = new GetRealDataRequest();
                    //request.LastRefreshRealDataTime = lastRefreshRealDataTime;
                    var getAllPointResponse = realMessageService.GetRealData(request);
                    if (getAllPointResponse.Data != null && getAllPointResponse.Data.Count > 0)
                    {
                        foreach (var item in getAllPointResponse.Data)
                        {
                            //if (item.DttStateTime > lastRefreshRealDataTime)
                            //{
                            //    lastRefreshRealDataTime = item.DttStateTime;//更新最后实时值最后获取时间
                            //}
                            #region 加载设备实时信息
                            row = dt.NewRow();
                            row["point"] = item.Point;
                            row["ssz"] = item.Ssz;
                            if (item.DevClass != null)
                            {
                                if (item.DevClass.Contains("甲烷"))
                                {
                                    if (item.Ssz == "0")
                                    {
                                        item.Ssz = "0.00";
                                        row["ssz"] = item.Ssz;
                                    }
                                }
                            }
                            row["zt"] = item.DataState;
                            row["NCtrlSate"] = item.NCtrlSate;//获取控制量馈电状态  20170725
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
                            row["bj"] = item.Alarm;
                            row["time"] = item.Zts;
                            row["voltage"] = Math.Round(item.Voltage, 1);//电压/电量  20180622
                            row["GradingAlarmLevel"] = item.GradingAlarmLevel;
                            row["StationDyType"] = item.StationDyType;
                            if (item.DevProperty == "分站")
                            {
                                if (item.Voltage > 0)
                                {
                                    row["dldj"] = item.Voltage.ToString("f1") + " %";
                                }
                                else
                                {
                                    row["dldj"] = "0";
                                }
                            }
                            else
                            {
                                //if (item.Voltage == 1)
                                //{
                                //    row["dldj"] = "<9.0 V";
                                //}
                                //else if (item.Voltage == 2)
                                //{
                                //    row["dldj"] = "9.0～16.5 V";
                                //}
                                //else if (item.Voltage == 3)
                                //{
                                //    row["dldj"] = "16.5～24.0 V";
                                //}
                                //else if (item.Voltage == 4)
                                //{
                                //    row["dldj"] = ">24.0 V";
                                //}
                                //else
                                //{
                                row["dldj"] = item.Voltage.ToString("f1");
                                //}
                            }
                            dt.Rows.Add(row);
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SetServerConct();
                LogHelper.Error("获取所有实时数据", ex);
            }
            return dt;
        }

        /// <summary>
        /// 测点总表初始化
        /// </summary>
        private static DataTable CreateDT()
        {
            DataTable dt = new DataTable();
            dt.TableName = "point";
            #region 初始化
            DataColumn clm = new DataColumn();
            clm.ColumnName = "point";
            clm.DataType = typeof(string);
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
            clm.ColumnName = "dldj";
            clm.DataType = typeof(int);
            dt.Columns.Add(clm);
            #endregion
            return dt;
        }

        /// <summary>
        /// 获取定义改变时间 判断定义是否改变
        /// </summary>
        /// <returns></returns>
        public static bool GetDefineChangeFlg()
        {
            bool flg = false;
            string time = "";
            try
            {
                var respone = realMessageService.GetDefineChangeFlg();
                time = respone.Data;
                if (time != "")
                {
                    if (StaticClass.DefineTime == "")
                    {
                        StaticClass.DefineTime = time;
                    }
                    else
                    {
                        if (StaticClass.DefineTime != time)
                        {
                            StaticClass.DefineTime = time;
                            StaticClass.Definechange = true;
                            flg = true;
                        }
                    }
                }
                if (!StaticClass.ServerConet)
                {
                    StaticClass.ServerConet = true;
                    Alarm.ClientAlarmConfig.setserverconnectstate(true);
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return flg;
        }

        /// <summary>
        /// 获取显示配置改变标记
        /// </summary>
        /// <returns></returns>
        public static bool GetRealCfgChangeFlg()
        {
            bool flg = false;
            string time = "";
            try
            {
                if (StaticClass.ServerConet)
                {
                    var respone = realMessageService.GetRealCfgChangeFlg();
                    time = respone.Data;
                    if (time != "")
                    {
                        if (StaticClass.RealCfgTime == "")
                        {
                            StaticClass.RealCfgTime = time;
                        }
                        else
                        {
                            if (StaticClass.RealCfgTime != time)
                            {
                                StaticClass.RealCfgTime = time;
                                flg = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return flg;
        }

        /// <summary>
        /// 获取新的运行记录
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DataTable GetRunLogs(long uid)
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetRunLogsRequest() { UserId = uid };
                    var response = realMessageService.GetRunLogs(request);
                    if (response.Data != null)
                    {
                        dt = response.Data;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["state"] = OprFuction.StateChange(dt.Rows[i]["state"].ToString());
                                dt.Rows[i]["sbstate"] = OprFuction.StateChange(dt.Rows[i]["sbstate"].ToString());
                            }
                        }
                    }

                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 返回所有报警信息
        /// </summary>
        /// <returns></returns>
        public static List<Jc_BInfo> GetBjData()
        {
            var result = new List<Jc_BInfo>();
            try
            {
                if (StaticClass.ServerConet)
                {
                    var getAlarmDataResponse = realMessageService.GetAlarmData();
                    if (getAlarmDataResponse.Data != null)
                    {
                        result = getAlarmDataResponse.Data;
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                OprFuction.SetServerConct();
                result = null;
            }
            return result;
        }

        public static bool deleteMac(string mac)
        {
            bool flg = true;
            try
            {
                if (StaticClass.ServerConet)
                {
                    try
                    {
                        var request = new NetworkModuleDeleteByMacRequest() { Mac = mac };
                        var response = jc_MacService.DeleteNetworkModule(request);
                        flg = true;
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        flg = false;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return flg;
        }
        /// <summary>
        /// 获取网络模块数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRealMac()
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetRealMac();
                    if (response.Data != null)
                    {
                        dt = response.Data;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["zt"] = OprFuction.StateChange(dt.Rows[i]["zt"].ToString());
                            }
                        }
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 修改报警断电措施
        /// </summary>
        /// <param name="id">报警断电主键id</param>
        /// <param name="time">开始时间</param>
        /// <param name="cs">措施</param>
        public static void UpdateCs(string id, DateTime time, string cs)
        {
            string tablename = "KJ_DataAlarm" + time.ToString("yyyyMMdd");
            try
            {
                if (StaticClass.ServerConet)
                {
                    var resquest = new UpdateAlarmStepRequest();
                    resquest.Id = id;
                    resquest.TableName = tablename;
                    resquest.Message = cs;
                    var response = realMessageService.UpdateAlarmStep(resquest);
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
        }

        /// <summary>
        /// 获取控制量测点号
        /// </summary>
        /// <returns></returns>
        public static DataTable Getkzpoint()
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetKZPoint();
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 获取所有绑定电源箱的分站  20170117
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDyxFz()
        {
            DataTable msg = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetBindDianYuanFenzhan();
                    if (response.Data != null)
                    {
                        msg = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return msg;
        }

        /// <summary>
        /// 获取所有绑定电源箱的交换机  20170118
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDyxMac()
        {
            DataTable msg = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetBindDianYuanMac();
                    if (response.Data != null)
                    {
                        msg = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return msg;
        }

        /// <summary>
        /// 获取标校测点  20170122
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DataTable Getbxpoint(DateTime time)
        {
            DataTable msg = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetbxpointRequest() { Time = time };
                    var response = realMessageService.GetBXPoint(request);
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return msg;
        }


        /// <summary>
        /// 保存标校测点  20170122
        /// </summary>
        /// <param name="time"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SavePoint(DateTime time, string str)
        {
            bool msg = false;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new SavePointRequest();
                    request.Time = time;
                    request.PointStr = str;
                    var response = realMessageService.SavePoint(request);
                    msg = response.Data;
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return msg;
        }


        /// <summary>
        /// 获取分站测点号
        /// </summary>
        /// <returns></returns>
        public static DataTable Getfzpoint()
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetFZPoint();
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 根据测点获取结构体
        /// </summary>
        /// <param name="point">测点号</param>
        /// <returns></returns>
        public static Jc_DefInfo Getpoint(string point)
        {
            Jc_DefInfo obj = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetPointRequest() { Point = point };
                    var response = realMessageService.GetPoint(request);
                    if (response.Data != null)
                    {
                        obj = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return obj;
        }


        /// <summary>
        /// 获取主控点
        /// </summary>
        /// <param name="point">控制量测点号</param>
        /// <returns></returns>
        public static DataTable Getzkpoint(string point)
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetZKPointRequest() { Point = point };
                    var response = realMessageService.GetZKPoint(request);
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 获取主控点
        /// </summary>
        /// <param name="point">控制量测点号</param>
        /// <returns></returns>
        public static DataTable Getzkpointex()
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var response = realMessageService.GetZKPointex();
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }

        /// <summary>
        /// 获取该分站下的交叉控制
        /// </summary>
        /// <param name="fzh">分站地址</param>
        /// <returns></returns>
        public static DataTable Getfzjckz(int fzh)
        {
            DataTable dt = null;
            try
            {
                if (StaticClass.ServerConet)
                {
                    var request = new GetFZJXControlRequest() { Fzh = fzh };
                    var response = realMessageService.GetFZJXControl(request);
                    if (response.Data != null)
                    {
                        dt = response.Data;
                    }
                }
            }
            catch
            {
                OprFuction.SetServerConct();
            }
            return dt;
        }


        /// <summary>
        ///     查询所有手动控制
        /// </summary>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZsCache()
        {
            var res = manualCrossControlService.GetAllManualCrossControlDetail();
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }
        /// <summary>
        /// 根据counter获取运行记录
        /// </summary>
        /// <param name="counter"></param>
        /// <returns></returns>
        public static List<Jc_RInfo> GetRunRecordByCounter(long counter)
        {
            GetRunRecordListByCounterRequest realMessageRequest = new GetRunRecordListByCounterRequest();
            realMessageRequest.Counter = counter;
            return realMessageService.GetRunRecordListByCounter(realMessageRequest).Data;
        }
        /// <summary>
        /// 根据时间获取最近的人员采集记录
        /// </summary>
        /// <returns></returns>
        public static List<R_PhistoryInfo> GetRealR_PhistoryInfoList(DateTime timer)
        {
            List<R_PhistoryInfo> rvalue = new List<R_PhistoryInfo>();
            R_PhistoryGetLastByTimerRequest request = new R_PhistoryGetLastByTimerRequest();
            request.Timer = timer;
            rvalue = r_PhistoryService.GetPersonR_PhistoryByTimer(request).Data;
            return rvalue;
        }

        /// <summary>
        /// 获取所有设备类型
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DevInfo> GetAllDeviceDefine()
        {
            return deviceDefineService.GetAllDeviceDefineCache().Data;
        }
    }
}
