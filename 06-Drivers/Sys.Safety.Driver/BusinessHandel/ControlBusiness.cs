using Basic.Framework.Common;
using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.Request.StaionControlHistoryData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver
{
    class ControlBus
    {
        #region 控制相关操作

        /// <summary>
        /// 获取开关量控制口
        /// </summary>
        /// <param name="pointInfo"></param>
        /// <param name="datastate"></param>
        /// <param name="zkid"></param>
        /// <param name="writeType">0不写、 1断电、2复电</param>
        /// <returns></returns>
        public static string GetDerailControlPort(Jc_DefInfo pointInfo, DeviceDataState datastate)
        {
            int controport = 0;
            string controlportstr = string.Empty;
            decimal fzh = 0, tdh = 0, dzh = 0;
            AbnormalFeedItem kditem;
            string jckz = string.Empty;

            int controlType = 0;
            #region 取本地控制点
            switch (datastate)
            {
                case DeviceDataState.DataDerailState0://0态                                              
                    if (!string.IsNullOrEmpty(pointInfo.K1.ToString()))
                    {
                        controport = pointInfo.K1;
                        jckz = pointInfo.Jckz1;
                        controlType = (int)ControlType.ControlState0;
                    }
                    break;
                case DeviceDataState.DataDerailState1://1态
                    if (!string.IsNullOrEmpty(pointInfo.K2.ToString()))
                    {
                        controport = pointInfo.K2;
                        jckz = pointInfo.Jckz2;
                        controlType = (int)ControlType.ControlState1;
                    }
                    break;
                case DeviceDataState.DataDerailState2://2态
                    if (!string.IsNullOrEmpty(pointInfo.K3.ToString()))
                    {
                        controport = pointInfo.K3;
                        jckz = pointInfo.Jckz3;
                        controlType = (int)ControlType.ControlState2;
                    }
                    break;
            }

            string strKzkPoint = GetKzkPoint(pointInfo, controport);
            if (!string.IsNullOrEmpty(strKzkPoint))
            {
                controlportstr = strKzkPoint + "|";
            }
            //SetControlRemote(jckz, (ControlType)controlType);
            controlportstr += SetControlRemote(jckz, (ControlType)controlType);//2018.4.17 by
            #endregion
            if (controlportstr != "")
            {
                controlportstr = controlportstr.Remove(controlportstr.Length - 1);
            }
            return controlportstr;
        }
        /// <summary>
        /// 获取模拟量控制口
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="datastate"></param>
        /// <param name="createTime"></param>
        /// <param name="zkid"></param>
        /// <param name="writeType">0不写、 1断电、2复电</param>
        /// <returns></returns>
        public static string GetAnalogControlPort(Jc_DefInfo defInfo, DeviceDataState datastate )
        {
            int controlport = 0;
            string controlportstr = "";

            #region 取本地控制点
            switch (datastate)
            {
                case DeviceDataState.EquipmentDown://断线
                    if (!string.IsNullOrEmpty(defInfo.K7.ToString()))
                    {
                        controlport = defInfo.K7;
                    }
                    break;
                case DeviceDataState.EquipmentOverrange://上溢
                    if (!string.IsNullOrEmpty(defInfo.K5.ToString()))
                    {
                        controlport = defInfo.K5;
                    }
                    break;
                case DeviceDataState.EquipmentUnderrange://负漂
                    if (!string.IsNullOrEmpty(defInfo.K6.ToString()))
                    {
                        controlport = defInfo.K6;
                    }
                    break;
                case DeviceDataState.DataHighAlarm://上限报警
                    if (!string.IsNullOrEmpty(defInfo.K1.ToString()))
                    {
                        controlport = defInfo.K1;
                    }
                    break;
                case DeviceDataState.DataHighAlarmPowerOFF://上限断电
                    if (!string.IsNullOrEmpty(defInfo.K2.ToString()))
                    {
                        controlport = defInfo.K2;
                    }
                    break;
                case DeviceDataState.DataLowAlarm://下限报警
                    if (!string.IsNullOrEmpty(defInfo.K3.ToString()))
                    {
                        controlport = defInfo.K3;
                    }
                    break;
                case DeviceDataState.DataLowPower://下限断电
                    if (!string.IsNullOrEmpty(defInfo.K4.ToString()))
                    {
                        controlport = defInfo.K4;
                    }

                    break;
            }

            string strKzkPoint = GetKzkPoint(defInfo, controlport);
            if (!string.IsNullOrEmpty(strKzkPoint))
            {
                controlportstr = strKzkPoint + "|";
            }
            #endregion

            switch (datastate)
            {
                case DeviceDataState.EquipmentDown://断线
                    controlportstr += SetControlRemote(defInfo.Jckz2, ControlType.ControlLineDown);
                    break;
                case DeviceDataState.EquipmentOverrange:
                case DeviceDataState.EquipmentUnderrange:
                    if (datastate == DeviceDataState.EquipmentOverrange)
                    {
                        controlportstr += SetControlRemote(defInfo.Jckz3, ControlType.ControlErro);
                    }
                    else
                    {
                        controlportstr += SetControlRemote(defInfo.Jckz3, ControlType.ControlErro);
                    }
                    break;
                case DeviceDataState.DataHighAlarmPowerOFF:
                    controlportstr += SetControlRemote(defInfo.Jckz1, ControlType.ControlPowerOff);
                    break;
                case DeviceDataState.DataLowPower:
                    controlportstr += SetControlRemote(defInfo.Jckz1, ControlType.ControlPowerOff);
                    break;
            }
            if (controlportstr != "")
            {
                controlportstr = controlportstr.Remove(controlportstr.Length - 1);
            }
            return controlportstr;
        }

        /// <summary>
        /// 获取交叉控制口
        /// </summary>
        /// <param name="jckz">交叉控制字段</param>
        /// <param name="controlType">ControlType 枚举</param>
        /// <returns></returns>
        public static string SetControlRemote(string jckz, ControlType controlType)
        {
            string str = "";
            List<ControlRemote> ls_control = new List<ControlRemote>();
            ControlRemote control;
            string[] strKzk = jckz.Split('|');
            List<Jc_DefInfo> items;
            Jc_DefInfo item;
            try
            {
                if (strKzk.Length > 0)
                {
                    #region 交叉控制
                    for (int ii = 0; ii < strKzk.Length; ii++)
                    {
                        if (!string.IsNullOrEmpty(strKzk[ii]))
                        {
                            try
                            {
                                control = new ControlRemote();
                                control.ControlType = (int)controlType;// ControlType.ControlState0;
                                control.m_nCtrlAdrID = decimal.Parse(strKzk[ii].Substring(6, 1));       //dzh
                                control.m_nCtrlChannelID = decimal.Parse(strKzk[ii].Substring(4, 2));   //kh
                                control.m_nCtrlStationID = decimal.Parse(strKzk[ii].Substring(0, 3));  //fzh
                                control.m_strLabel = strKzk[ii];
                                ls_control.Add(control);
                            }
                            catch
                            {

                            }
                        }
                    }
                    for (int i = 0; i < ls_control.Count; i++)
                    {
                        control = ls_control[i];
                        items =SafetyHelper. GetPointDefinesByStationID((short)control.m_nCtrlStationID);
                        if (items.Count > 1)
                        {
                            for (int ii = 0; ii < items.Count; ii++)
                            {
                                item = items[ii];
                                if ((item.Kh == control.m_nCtrlChannelID)
                                    && (item.Dzh == control.m_nCtrlAdrID)
                                    && (item.DevPropertyID == (int)ItemDevProperty.Control))
                                {
                                    str += item.Point + "|";
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("SetControlRemote：" + ex.Message);
            }
            return str;
        }

        /// <summary>
        /// 获取分站的触点控制口和智能控制口(本地控制)
        /// </summary>
        /// <param name="defInfo">分站结构体</param>
        /// <param name="kz"></param>
        /// <returns>001C01|001C02......</returns>
        public static string GetKzkPoint(Jc_DefInfo defInfo, int _kzk)
        {
            string strKzkPoint = "";
            //不从缓存中取，直接按位计算  20170718
            //Jc_DefInfo tempKzItem;
            //List<Jc_DefInfo> pointItems = KJ73NHelper.GetPointDefinesByStationID(defInfo.Fzh);
            for (int k = 0; k < 24; k++)
            {
                if ((_kzk & (1 << k)) == (1 << k))
                {
                    if (k < 8)
                    {
                        //获取触点控制口
                        //tempKzItem = pointItems.FirstOrDefault(a => (a.DevPropertyID == (int)ItemDevProperty.Control) && (a.Kh == (k + 1)));
                        //strKzkPoint += tempKzItem == null ? string.Format("{0:000}C{1:00}0|", defInfo.Fzh, (k + 1)) : tempKzItem.Point + "|";
                        strKzkPoint += string.Format("{0:000}C{1:00}0|", defInfo.Fzh, (k + 1));
                    }
                    else
                    {
                        //获取智能控制口
                        //tempKzItem = pointItems.FirstOrDefault(a => (a.DevPropertyID == (int)ItemDevProperty.Control) && (a.Kh == (k - 7) && (a.Dzh == 1)));
                        //strKzkPoint += tempKzItem == null ? string.Format("{0:000}C{1:00}1|", defInfo.Fzh, (k - 7)) : tempKzItem.Point + "|";
                        strKzkPoint += string.Format("{0:000}C{1:00}1|", defInfo.Fzh, (k - 7));
                    }
                }
            }
            if (!string.IsNullOrEmpty(strKzkPoint))
            {
                strKzkPoint = strKzkPoint.Remove(strKzkPoint.Length - 1);
            }

            return strKzkPoint;
        }
        #endregion

        #region ----交叉控制操作----
        /// <summary>
        /// 根据主控点（和控制类型）删除手动交叉控制
        /// </summary>
        /// <param name="zkpoint">主控点</param>
        /// <param name="controltype">控制类型</param>
        public static bool DeleteManualCrossControlByZkpoint(string zkpoint, object controltype)
        {
            ManualCrossControlCacheGetByConditionRequest getByConditionRequest = new ManualCrossControlCacheGetByConditionRequest();
            getByConditionRequest.Predicate = manual => manual.ZkPoint == zkpoint;
            if (controltype != null && controltype is ControlType)
            {
                var type = (ControlType)controltype;
                getByConditionRequest.Predicate = manual => manual.ZkPoint == zkpoint && manual.Type == (short)type;
            }

            var response =SafetyHelper.manualControlCacheService.GetManualCrossControlCache(getByConditionRequest);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                ManualCrossControlsRequest manualRequest = new ManualCrossControlsRequest
                {
                    ManualCrossControlInfos = response.Data
                };
                var deleteresponse = SafetyHelper.manualControlService.DeleteManualCrossControls(manualRequest);
                if (deleteresponse != null && deleteresponse.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 根据被控点（和控制类型）删除手动交叉控制
        /// </summary>
        /// <param name="bkpoint"></param>
        /// <param name="controltype"></param>
        /// <returns></returns>
        public static bool DeleteManualCrossControlByBkpoint(string bkpoint, object controltype)
        {
            ManualCrossControlCacheGetByConditionRequest getByConditionRequest = new ManualCrossControlCacheGetByConditionRequest();
            getByConditionRequest.Predicate = manual => manual.Bkpoint == bkpoint;
            if (controltype != null && controltype is ControlType)
            {
                var type = (ControlType)controltype;
                getByConditionRequest.Predicate = manual => manual.Bkpoint == bkpoint && manual.Type == (short)type;
            }

            var response = SafetyHelper.manualControlCacheService.GetManualCrossControlCache(getByConditionRequest);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                ManualCrossControlsRequest manualRequest = new ManualCrossControlsRequest
                {
                    ManualCrossControlInfos = response.Data
                };
                var deleteresponse = SafetyHelper.manualControlService.DeleteManualCrossControls(manualRequest);
                if (deleteresponse != null && deleteresponse.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteManualCrossControlByZkpointByFzhTypeUpflag(string fzh, int type, string upflag)
        {
            ManualCrossControlCacheGetByConditionRequest getByConditionRequest = new ManualCrossControlCacheGetByConditionRequest();
            getByConditionRequest.Predicate = manual => 
                manual.Bkpoint.Substring(0, 3) == fzh &&
                manual.Type == type &&
                manual.Upflag == upflag;

            var response = SafetyHelper.manualControlCacheService.GetManualCrossControlCache(getByConditionRequest);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                ManualCrossControlsRequest manualRequest = new ManualCrossControlsRequest
                {
                    ManualCrossControlInfos = response.Data
                };
                var deleteresponse = SafetyHelper.manualControlService.DeleteManualCrossControls(manualRequest);
                if (deleteresponse != null && deleteresponse.IsSuccess)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取要增加的交叉控制列表
        /// </summary>
        /// <param name="point"></param>
        /// <param name="controlTye"></param>
        public static List<Jc_JcsdkzInfo> GetAddManualCrossControlFromDefine(Jc_DefInfo point, ControlType controlType)
        {
            List<Jc_JcsdkzInfo> jckzItems = new List<Jc_JcsdkzInfo>();
            try
            {
                string[] strKzk = { };
                Jc_JcsdkzInfo jckz;
                //开关量0态或模拟量断电
                if ((controlType == ControlType.ControlState0 || controlType == ControlType.ControlPowerOff) && !string.IsNullOrEmpty(point.Jckz1))
                {
                    strKzk = point.Jckz1.Split('|');
                }
                //开关量1态或模拟量断线控制
                else if ((controlType == ControlType.ControlState1 || controlType == ControlType.ControlLineDown) && !string.IsNullOrEmpty(point.Jckz2))
                {
                    strKzk = point.Jckz2.Split('|');
                }
                //开关量2态或模拟量故障控制
                else if ((controlType == ControlType.ControlState2 || controlType == ControlType.ControlErro) && !string.IsNullOrEmpty(point.Jckz3))
                {
                    strKzk = point.Jckz3.Split('|');
                }

                Array.ForEach(strKzk, bkPoint =>
                {
                    jckz = new Jc_JcsdkzInfo();
                    jckz.ID = IdHelper.CreateLongId().ToString();
                    jckz.Type = (short)controlType; //手动控制
                    jckz.ZkPoint = point.Point;//主控测点
                    jckz.Bkpoint = bkPoint;//被控测点
                    jckzItems.Add(jckz);
                });

            }
            catch (Exception ex)
            {
                LogHelper.Error("处理交叉控制失败：" + "\r\n" + ex.Message);
            }

            return jckzItems;
        }

        /// <summary>
        /// 根据控制类型获取交叉控制列表
        /// </summary>
        /// <param name="zkpoint"></param>
        /// <param name="controltype"></param>
        /// <returns></returns>
        public static List<Jc_JcsdkzInfo> GetManualCrossControlFromCache(string zkpoint, object controltype)
        {
            List<Jc_JcsdkzInfo> jckzItems = new List<Jc_JcsdkzInfo>();
            ManualCrossControlCacheGetByConditionRequest getByConditionRequest = new ManualCrossControlCacheGetByConditionRequest();
            getByConditionRequest.Predicate = manual => manual.ZkPoint == zkpoint;
            if (controltype != null && controltype is ControlType)
            {
                var type = (ControlType)controltype;
                getByConditionRequest.Predicate = manual => manual.ZkPoint == zkpoint && manual.Type == (short)type;
            }

            var response = SafetyHelper.manualControlCacheService.GetManualCrossControlCache(getByConditionRequest);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                jckzItems = response.Data;
            }

            return jckzItems;
        }

        /// <summary>
        /// 批量添加、更新、删除交叉控制入缓存和数据库（根据infostate判断）
        /// </summary>
        /// <param name="items"></param>
        public static void OperationManualCrossControl(List<Jc_JcsdkzInfo> items)
        {
            ManualCrossControlsRequest mnualCrossControlsRequest = new ManualCrossControlsRequest();
            mnualCrossControlsRequest.ManualCrossControlInfos = items;
            SafetyHelper.manualControlService.BatchOperationManualCrossControls(mnualCrossControlsRequest);
        }
        /// <summary>
        /// 根据控制类型重新加载交叉控制信息（以前有，现在有=不变；以前没有，现在有=新增；以前有，现在没有=删除）
        /// </summary>
        /// <param name="controlType"></param>
        public static void DoControlChange(Jc_DefInfo def, ControlType controlType)
        {
            try
            {
                List<Jc_JcsdkzInfo> jckzItems;
                List<Jc_JcsdkzInfo> myJckzItems = new List<Jc_JcsdkzInfo>();
                List<Jc_JcsdkzInfo> updateJckzItems = new List<Jc_JcsdkzInfo>();
                int index = 0;
                jckzItems = GetManualCrossControlFromCache(def.Point, null); //获取当前测点的所有交叉控制（默认置删除标记）
                jckzItems.ForEach(a =>
                {
                    a.InfoState = Basic.Framework.Web.InfoState.Delete;
                });
                if (controlType != ControlType.NoControl)
                {
                    myJckzItems = GetAddManualCrossControlFromDefine(def, controlType);  //获取当前应该生成的交叉控制（置新增标记）
                }
                foreach (Jc_JcsdkzInfo jckzItem in myJckzItems)
                {
                    index = jckzItems.FindIndex(a => a.ZkPoint == jckzItem.ZkPoint && a.Bkpoint == jckzItem.Bkpoint && a.Type == jckzItem.Type);
                    if (index < 0)
                    {
                        //若此交叉控制在原交叉控制链表中不存在，则新增加，否则不操作
                        jckzItem.InfoState = Basic.Framework.Web.InfoState.AddNew;
                        updateJckzItems.Add(jckzItem);
                    }
                    else
                    {
                        //若此交叉控制在原交叉控制链表中存在，则不操作
                        jckzItems.RemoveAt(index);
                    }
                }
                if (jckzItems.Count > 0)
                {
                    //当前链表中不存在，以前链表中有，则删除
                    updateJckzItems.AddRange(jckzItems);
                }
                if (updateJckzItems.Count > 0)
                {
                    OperationManualCrossControl(updateJckzItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DoControlChange Error【" + def.Point + "】" + controlType + "  --  " + ex.Message);
            }
        }

        #endregion

        #region ----历史控制数据----

        /// <summary>
        /// 历史控制记录入库
        /// </summary>
        /// <param name="items"></param>
        public static void StaionControlHistoryDataToDB(List<StaionControlHistoryDataInfo> items)
        {
            //删除数据库现有记录
            DeleteByPointAndTimeStaionControlHistoryDataRequest deleteByPointAndTimeStaionControlHistoryDataRequest = new DeleteByPointAndTimeStaionControlHistoryDataRequest();
            foreach (StaionControlHistoryDataInfo item in items)
            {
                deleteByPointAndTimeStaionControlHistoryDataRequest.Point = item.Point;
                deleteByPointAndTimeStaionControlHistoryDataRequest.Time = item.SaveTime;
                SafetyHelper.staionControlHistoryDataService.DeleteStaionControlHistoryDataByPointAndTime(deleteByPointAndTimeStaionControlHistoryDataRequest);
            }
            //数据入库
            StationControlHistoryDataToDBRequest stationControlHistoryDataToDBRequest = new StationControlHistoryDataToDBRequest();
            stationControlHistoryDataToDBRequest.StaionControlHistoryDataItems = items;
            SafetyHelper.staionControlHistoryDataService.InsertStationControlHistoryDataToDB(stationControlHistoryDataToDBRequest);
        }


        public static StaionControlHistoryDataInfo GetStaionControlHistoryData(string fzh, string point, DeviceHistoryControlItem item, DateTime time)
        {
            StaionControlHistoryDataInfo staionControlHistoryDataInfo = new StaionControlHistoryDataInfo();

            staionControlHistoryDataInfo.ControlDevice = item.ControlDevice;
            staionControlHistoryDataInfo.DataTime = time;
            staionControlHistoryDataInfo.Dzh = item.Address;
            staionControlHistoryDataInfo.Fzh = fzh;
            staionControlHistoryDataInfo.Id = IdHelper.CreateLongId().ToString() ;
            staionControlHistoryDataInfo.Kh = item.Channel;
            staionControlHistoryDataInfo.Point = point;
            staionControlHistoryDataInfo.SaveTime = item.SaveTime;
            staionControlHistoryDataInfo.State = (int)item.State;
            staionControlHistoryDataInfo.Value = item.RealData;

            return staionControlHistoryDataInfo;
        }

        #endregion

    }
}
