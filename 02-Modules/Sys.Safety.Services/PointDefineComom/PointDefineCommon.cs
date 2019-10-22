using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Model;
using Sys.Safety.Enums;
using Sys.Safety.Enums.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.DataToDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Services.PointDefineComom
{
    /// <summary>
    /// 测点定义服务公共方法
    /// </summary>
    public class PointDefineCommon
    {
        /// <summary>
        /// 置是否下发初始化标记
        /// </summary>
        /// <param name="item">当前对象</param>
        /// <param name="olditem">之前的对象</param>
        public static void PointInitializes(Jc_DefInfo item, Jc_DefInfo olditem)
        {
            //置是否下发初始化标记
            try
            {
                //置是否需要下发初始化标记
                if (item.InfoState == InfoState.AddNew || item.Activity == "0")
                {
                    if (item.InfoState == InfoState.AddNew)
                    {
                        item.PointEditState = 1;//置测点定义状态标记，用来判断初始化是否下发到分站上
                    }
                    item.DefIsInit = true;//新增加测点，都发初始化
                }
                else //修改测点，判断是否需要发送初始化
                {
                    bool kzchangeflag = false;//控制口变化标记                    
                    bool dormancyflag = false;

                    switch (item.DevPropertyID)
                    {
                        case 0://分站修改
                            item.DefIsInit = CompareMonitorStation(item, olditem, out dormancyflag);
                            item.Dormancyflag = dormancyflag;
                            break;
                        case 1://模拟量修改                            
                            item.DefIsInit = CompareMoniterMNL(item, olditem, out kzchangeflag, out dormancyflag);
                            item.kzchangeflag = kzchangeflag;
                            item.ReDoDeal = 1;
                            item.Dormancyflag = dormancyflag;
                            break;
                        case 2://开关量修改
                            item.DefIsInit = CompareMoniterKGL(item, olditem, out kzchangeflag, out dormancyflag);
                            item.kzchangeflag = kzchangeflag;
                            item.ReDoDeal = 1;
                            item.Dormancyflag = dormancyflag;
                            break;
                        case 3://控制量修改
                            item.DefIsInit = CompareMoniterKZL(item, olditem, out dormancyflag);
                            item.ReDoDeal = 1;
                            item.Dormancyflag = dormancyflag;
                            break;
                        case 4://累计量修改(不需要发初始化)
                            //item.DefIsInit = CompareMoniterLJL(item, olditem);
                            break;
                    }
                    if (item.Dormancyflag)//如果是休眠状态，则暂时置为上一次的状态，待保存巡检的时候再置成休眠(数据处理是直接根据Bz4进行处理的，不然保存了定义就会开始处理了)  20170705
                    {
                        if (olditem != null)
                        {
                            item.Bz4 = olditem.Bz4;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 对比分站是否需要下发初始化
        /// </summary>
        /// <param name="newitem"></param>
        /// <param name="olditem"></param>
        /// <returns></returns>
        private static bool CompareMonitorStation(Jc_DefInfo newitem, Jc_DefInfo olditem, out bool dormancyflag)
        {
            bool flg = olditem.DefIsInit;
            dormancyflag = olditem.Dormancyflag;
            if ((newitem.Bz4 & 0x02) == 0x02)
            {
                dormancyflag = true;
            }
            if (newitem.ClsCommObj.Cfzfy != olditem.ClsCommObj.Cfzfy
                || newitem.ClsCommObj.Bddqy != olditem.ClsCommObj.Bddqy)//抽放（暂时没地方配置）
            {
                flg = true;
            }
            if (newitem.Bz3 != olditem.Bz3)//风电闭锁
            {
                flg = true;
            }
            if (newitem.Bz10 != olditem.Bz10)//风电闭锁
            {
                flg = true;
            }


            //增加以下属性的判断  20170609
            if (newitem.Jckz1 != olditem.Jckz1)//Mac
            {
                flg = true;
            }
            if (newitem.Jckz2 != olditem.Jckz2)//IP
            {
                flg = true;
            }
            
            return flg;
        }
        /// <summary>
        /// 对比2个模拟量的信息是否相同 是否需要重新下发初始化
        /// </summary>
        /// <param name="newobj">新对象</param>
        /// <param name="oldobj">旧对象</param>
        /// <param name="kzchangeflg">控制口变化标记</param>
        /// <param name="EndAlarmflag">报警结束标记</param>
        /// <returns></returns>
        private static bool CompareMoniterMNL(Jc_DefInfo newitem, Jc_DefInfo olditem, out bool kzchangeflg, out bool dormancyflag)
        {
            bool flg = olditem.DefIsInit;
            dormancyflag = olditem.Dormancyflag;
            kzchangeflg = olditem.kzchangeflag;
            if ((newitem.Bz4 & 0x02) == 0x02)
            {
                dormancyflag = true;
            }
            if (newitem.Z1 != olditem.Z1
                || newitem.Z2 != olditem.Z2
                || newitem.Z3 != olditem.Z3
                || newitem.Z4 != olditem.Z4
                || newitem.Z5 != olditem.Z5
                || newitem.Z6 != olditem.Z6
                || newitem.Z7 != olditem.Z7
                || newitem.Z8 != olditem.Z8)//预警、报警、断电值
            {
                flg = true;
            }
            if ((newitem.Jckz1 != olditem.Jckz1) || (newitem.Jckz2 != olditem.Jckz2) || (newitem.Jckz3 != olditem.Jckz3))//交叉控制口
            {
                kzchangeflg = true;
            }
            if ((newitem.K1 != olditem.K1) ||
                (newitem.K2 != olditem.K2) ||
                (newitem.K3 != olditem.K3) ||
                (newitem.K4 != olditem.K4) ||
                (newitem.K5 != olditem.K5) ||
                (newitem.K6 != olditem.K6) ||
                (newitem.K7 != olditem.K7) ||
                (newitem.K8 != olditem.K8))//本地控制口
            {
                kzchangeflg = true;
                flg = true;
            }

            //217.9.8 by 传感器通讯类型变化，要下发初始化
            if (newitem.Bz18 != olditem.Bz18)
            {
                flg = true;
            }
            if (newitem.Bz8 != olditem.Bz8)
            {
                flg = true;
            }
            if (newitem.Bz9 != olditem.Bz9)
            {
                flg = true;
            }
            if (newitem.DevModelID != olditem.DevModelID)//设备型号改变，要发初始化  20171031
            {
                flg = true;
            }
            if (newitem.DevClassID != olditem.DevClassID)//设备种类改变，要发初始化  20171031
            {
                flg = true;
            }
            return flg;
        }
        /// <summary>
        /// 对比2个开关量的信息是否相同 是否需要重新下发初始化
        /// </summary>
        /// <param name="newobj">新对象</param>
        /// <param name="oldobj">旧对象</param>
        /// <param name="kzchangeflg">控制口变化标记</param>
        /// <param name="EndAlarmflag">报警结束标记</param>
        /// <returns></returns>
        private static bool CompareMoniterKGL(Jc_DefInfo newitem, Jc_DefInfo olditem, out bool kzchangeflag, out bool dormancyflag)
        {
            bool flg = olditem.DefIsInit;
            kzchangeflag = olditem.kzchangeflag;
            dormancyflag = olditem.Dormancyflag;
            if ((newitem.Bz4 & 0x02) == 0x02)
            {
                dormancyflag = true;
            }
            if ((newitem.Jckz1 != olditem.Jckz1) ||
                (newitem.Jckz2 != olditem.Jckz2) ||
                (newitem.Jckz3 != olditem.Jckz3))
            {
                kzchangeflag = true;
            }

            if (newitem.K1 != olditem.K1 ||
                newitem.K2 != olditem.K2 ||
                newitem.K3 != olditem.K3 
                //||                newitem.K4 != olditem.K4  //逻辑报警修改，不发初始化  20170716
                //||                newitem.K5 != olditem.K5
                )
            {
                flg = true;
                kzchangeflag = true;
            }
            //217.9.8 by 传感器通讯类型变化，要下发初始化
            if (newitem.Bz18 != olditem.Bz18)
            {
                flg = true;
            }
            if (newitem.DevModelID != olditem.DevModelID)//设备型号改变，要发初始化  20171031
            {
                flg = true;
            }
            if (newitem.DevClassID != olditem.DevClassID)//设备种类改变，要发初始化  20171031
            {
                flg = true;
            }
            #region//开关量0、1、2态是否报警发生变化，将对应JC_B记录中的isAlram标记置为相应的是否报警状态，K8由低到高位分别为0、1、2态是否报警标记。

            if (olditem.DataState != (short)DeviceDataState.EquipmentStateUnknow)
            {
                //此处理移植到驱动中进行处理，开关量每次数据都要进行报警判断 2017.7.12 by


                //IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
                //AlarmCacheGetByConditonRequest alarmCacheGetByConditonRequest = new AlarmCacheGetByConditonRequest();
                //List<Jc_BInfo> alarmItems;
                //bool alarmFlag = false;
                //if (olditem.DataState == (short)DeviceDataState.DataDerailState0 && (olditem.K8 & 0x01) != (newitem.K8 & 0x01))
                //{
                //    if ((newitem.K8 & 0x01) == 0x01)
                //    {
                //        alarmFlag = true;
                //    }
                //    alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState0;
                //}
                //else if (olditem.DataState == (short)DeviceDataState.DataDerailState1 && (olditem.K8 & 0x02) != (newitem.K8 & 0x02))
                //{
                //    if ((newitem.K8 & 0x02) == 0x02)
                //    {
                //        alarmFlag = true;
                //    }
                //    alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState1;
                //}
                //else if (olditem.DataState == (short)DeviceDataState.DataDerailState2 && (olditem.K8 & 0x04) != (newitem.K8 & 0x04))
                //{
                //    if ((newitem.K8 & 0x04) == 0x04)
                //    {
                //        alarmFlag = true;
                //    }
                //    alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState2;
                //}
                //else
                //{
                //    LogHelper.Error("CompareMoniterKGL Error;DataState = " + olditem.DataState);
                //}
                //2017.7.12 by 
                //    if (olditem.DataState == (short)DeviceDataState.DataDerailState0 )
                //    {
                //        if ((newitem.K8 & 0x01) == 0x01)
                //        {
                //            alarmFlag = true;
                //        }
                //        alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState0;
                //    }
                //    else if (olditem.DataState == (short)DeviceDataState.DataDerailState1 )
                //    {
                //        if ((newitem.K8 & 0x02) == 0x02)
                //        {
                //            alarmFlag = true;
                //        }
                //        alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState1;
                //    }
                //    else if (olditem.DataState == (short)DeviceDataState.DataDerailState2 )
                //    {
                //        if ((newitem.K8 & 0x04) == 0x04)
                //        {
                //            alarmFlag = true;
                //        }
                //        alarmCacheGetByConditonRequest.Predicate = p => p.Stime == olditem.DttRunStateTime && p.Point == olditem.Point && p.Type == (short)DeviceDataState.DataDerailState2;
                //    }
                //    else
                //    {
                //        LogHelper.Error("CompareMoniterKGL Error;DataState = " + olditem.DataState);
                //    }
                //    if (alarmCacheGetByConditonRequest.Predicate != null)   //2017.7.11 by
                //    {
                //        var alarmResponse = alarmCacheService.GetAlarmCache(alarmCacheGetByConditonRequest);
                //        if (alarmResponse.Data != null && alarmResponse.IsSuccess)
                //        {

                //            #region ----计算逻辑报警----2017.7.12 by
                //            if (CheckDerailLogicRelationState((alarmFlag ? 1 : 0), newitem) == 0)
                //            {
                //                alarmFlag = false;
                //            }
                //            else
                //            {
                //                alarmFlag = true;
                //            }
                //            #endregion
                //            if (olditem.Alarm != (alarmFlag ? 1 : 0))
                //            {
                //                alarmItems = alarmResponse.Data;
                //                alarmItems.ForEach(a =>
                //                {
                //                    if (alarmFlag)
                //                    {
                //                        a.Isalarm = 1;
                //                    }
                //                    else
                //                    {
                //                        a.Isalarm = 0;
                //                    }
                //                    a.InfoState = InfoState.Modified;
                //                });
                //                BatchUpdateAlarmInfo(alarmItems);
                //                //更新缓存中的alarm字段 2017.7.12 by
                //                UpdateAlarmParameter(newitem, (alarmFlag ? 1 : 0));
                //            }
                //        }
                //    }
            }
            #endregion

            return flg;
        }
        ///// <summary>
        ///// 更新JC_DEF的alarm  2017.7.12 by
        ///// </summary>
        //private static void UpdateAlarmParameter(Jc_DefInfo def,int alarm)
        //{
        //    try
        //    {
        //        Dictionary<string, object> updateItems = new Dictionary<string, object>();
        //        updateItems.Add("Alarm", alarm);
        //        IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        //        DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
        //        defineCacheUpdatePropertiesRequest.PointID = def.PointID;
        //        defineCacheUpdatePropertiesRequest.UpdateItems = updateItems;
        //        pointDefineCacheService.UpdatePointDefineIfo(defineCacheUpdatePropertiesRequest);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("UpdateAlarmParameter Error:" + ex.Message);
        //    }
        //}

        ///// <summary>
        ///// 判断开关量逻辑关联测点状态 2017.7.12 by  从取驱动中copy过来
        ///// </summary>
        ///// <param name="pointAlarmState">当前开关量报警状态</param>
        //private static int CheckDerailLogicRelationState(int pointAlarmState, Jc_DefInfo def)
        //{
        //    //逻辑关联处理 与或（1,2）认为此开关量有关联测点
        //    if (def.K4 == 1 || def.K4 == 2)
        //    {
        //        Jc_DefInfo pointItem;
        //        if (def.Fzh > 0 && def.K5 > 0)
        //        {
        //            IPointDefineCacheService pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
        //            PointDefineCacheGetByConditonRequest stationquest = new PointDefineCacheGetByConditonRequest
        //            {
        //                Predicate = point => point.Fzh == def.Fzh && point.Kh == def.K5 && point.DevPropertyID == (int)DeviceProperty.Derail
        //            };
                    
        //            var relationresponse = pointDefineCacheService.GetPointDefineCache(stationquest);
        //            //如果此开关量有关联测点,则判断关联测点的报警状态
        //            if (relationresponse != null && relationresponse.IsSuccess && relationresponse.Data != null && relationresponse.Data.Any())
        //            {
        //                pointItem = relationresponse.Data.FirstOrDefault();
        //                int relationalarm = 0;
        //                if ((pointItem.DataState == (short)DeviceDataState.DataDerailState0) && ((pointItem.K8 & 0x01) == 0x01) ||
        //                    (pointItem.DataState == (short)DeviceDataState.DataDerailState1) && ((pointItem.K8 & 0x02) == 0x02) ||
        //                    (pointItem.DataState == (short)DeviceDataState.DataDerailState1) && ((pointItem.K8 & 0x04) == 0x04))
        //                    relationalarm = 1;

        //                //当前开关量与关联开关量与或运算,判断当前开关量是否报警
        //                if (def.K4 == 2)
        //                {
        //                    pointAlarmState |= relationalarm;
        //                }
        //                else if (def.K4 == 1)
        //                {
        //                    pointAlarmState &= relationalarm;
        //                }
        //            }
        //        }
        //    }
        //    return pointAlarmState;
        //}

        /// <summary>
        /// 对比2个控制量的信息是否相同 是否需要重新下发初始化
        /// </summary>
        /// <param name="newobj">新对象</param>
        /// <param name="oldobj">旧对象</param>
        /// <returns></returns>
        private static bool CompareMoniterKZL(Jc_DefInfo newitem, Jc_DefInfo olditem, out bool dormancyflag)
        {
            bool flg = olditem.DefIsInit;
            dormancyflag = olditem.Dormancyflag;
            if ((newitem.Bz4 & 0x02) == 0x02)
            {
                dormancyflag = true;
            }
            if (newitem.K2 != olditem.K2 || newitem.K1 != olditem.K1 || newitem.K4 != olditem.K4)//绑定的馈电开关
            {
                flg = true;
            }
            if (newitem.DevModelID != olditem.DevModelID)//设备型号改变，要发初始化  20171031
            {
                flg = true;
            }
            if (newitem.DevClassID != olditem.DevClassID)//设备种类改变，要发初始化  20171031
            {
                flg = true;
            }
            return flg;
        }

        //private static void BatchUpdateAlarmInfo(List<Jc_BInfo> alarmItems)
        //{
        //    IAlarmCacheService alarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
        //    AlarmCacheUpdatePropertiesRequest alarmCacheUpdatePropertiesRequest = new AlarmCacheUpdatePropertiesRequest();
        //    Dictionary<string, object> updateItems = new Dictionary<string, object>();
        //    foreach (Jc_BInfo alarmInfo in alarmItems)
        //    {
        //        updateItems = new Dictionary<string, object>();
        //        updateItems.Add("Isalarm", alarmInfo.Isalarm);
        //        //更新到缓存
        //        alarmCacheUpdatePropertiesRequest.AlarmKey = alarmInfo.ID;
        //        alarmCacheUpdatePropertiesRequest.UpdateItems = updateItems;
        //        alarmCacheService.UpdateAlarmInfoProperties(alarmCacheUpdatePropertiesRequest);
        //    }
        //    //更新到数据库
        //    IAlarmRecordRepository alarmRecordRepository = ServiceFactory.Create<IAlarmRecordRepository>();
        //    System.Data.DataColumn[] cols = new System.Data.DataColumn[updateItems.Count];
        //    for (int i = 0; i < updateItems.Count; i++)
        //    {
        //        cols[i] = new System.Data.DataColumn(updateItems.Keys.ToList()[i]);
        //    }
        //    List<Jc_BModel> alarmModel;
        //    alarmItems.ForEach(item =>
        //    {
        //        alarmModel = new List<Jc_BModel>();
        //        alarmModel.Add(ObjectConverter.Copy<Jc_BInfo, Jc_BModel>(item));
        //        alarmRecordRepository.BulkUpdate("KJ_DataAlarm" + item.Stime.ToString("yyyyMM"), alarmModel, cols, "ID");
        //    });
        //}
    }
}
