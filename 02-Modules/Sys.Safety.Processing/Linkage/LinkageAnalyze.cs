using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using System.Data;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Enums.Constant;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Processing.Linkage
{
    public class LinkageAnalyze
    {
        private static readonly IPositionService PositionService = ServiceFactory.Create<IPositionService>();

        private static readonly IEmergencyLinkHistoryService EmergencyLinkHistoryService = ServiceFactory.Create<IEmergencyLinkHistoryService>();

        private static readonly IB_CallService BcallService = ServiceFactory.Create<IB_CallService>();

        private static readonly IR_CallService RcallService = ServiceFactory.Create<IR_CallService>();

        private static readonly IPointDefineCacheService PointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();

        private static readonly ISysEmergencyLinkageService SysEmergencyLinkageService =
            ServiceFactory.Create<ISysEmergencyLinkageService>();

        private static readonly IAlarmCacheService AlarmCacheService = ServiceFactory.Create<IAlarmCacheService>();

        /// <summary>无新开始记录时是否已经发送过最后5秒的记录
        /// 
        /// </summary>
        private static bool _isSendStart5 = true;

        /// <summary>上一次是否有新开始纪录
        /// 
        /// </summary>
        private static bool _isLastHaveNewStart = true;

        /// <summary>无新结束记录时是否已经发送过最后5秒的记录
        /// 
        /// </summary>
        private static bool _isSendEnd5 = true;

        /// <summary>上一次是否有新结束纪录
        /// 
        /// </summary>
        private static bool _isLastHaveNewEnd = true;

        /// <summary>最后一个报警开始计算时间
        /// 
        /// </summary>
        private static DateTime aqbjLastStime;

        /// <summary>最后一个报警结束计算时间
        /// 
        /// </summary>
        private static DateTime aqbjLastEtime;

        /// <summary>是否第一次分析
        /// 
        /// </summary>
        //private static bool _ifFirstRun = true;

        /// <summary>最后一次运行时间
        /// 
        /// </summary>
        private static DateTime _lastRunTime = new DateTime();

        /// <summary>运行标记
        /// 
        /// </summary>
        private static bool _isRun;

        /// <summary>处理线程
        /// 
        /// </summary>
        private static Thread _handleThread;

        /// <summary>开始分析
        /// 
        /// </summary>
        public static void Start()
        {
            LogHelper.Info("【LinkageAnalyze】应急联动分析线程开启。");

            _isRun = true;
            if (_handleThread == null || (_handleThread != null && !_handleThread.IsAlive))
            {
                _handleThread = new Thread(HandleThreadFun);
                _handleThread.Start();
            }
        }

        /// <summary>结束分析
        /// 
        /// </summary>
        public static void Stop()
        {
            LogHelper.Info("【LinkageAnalyze】应急联动分析线程结束。");
            _isRun = false;
            while (true)
            {
                if (_isRun) break;
                Thread.Sleep(1000);
            }
        }

        /// <summary>线程函数
        /// 
        /// </summary>
        private static void HandleThreadFun()
        {
            while (_isRun)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    if ((dtNow - _lastRunTime).TotalSeconds >= 5)
                    {
                        Analyze();
                        _lastRunTime = DateTime.Now;
                    }
                }
                catch (Exception e)
                {
                    LogHelper.Error(e.ToString());
                }

                Thread.Sleep(1000);
            }
            _isRun = true;
            LogHelper.Info("【LinkageAnalyze】应急联动分析线程结束成功。");
        }

        /// <summary>分析函数
        /// 
        /// </summary>
        private static void Analyze()
        {
            var nowTime = DateTime.Now;
            //if (_ifFirstRun)
            //{
            //    _ifFirstRun = false;
            //}
            //else
            //{

            //结束已删除的应急联动
            var res4 = EmergencyLinkHistoryService.GetDeleteButNotEndLinkageIds();
            //var allDelLinkageConfig = allLinkageConfig.Where(a => a.Type == 1 && a.Activity != 1 && a.EmergencyLinkageState==1).ToList(); //所有非活动的普通应急联动
            foreach (var item in res4.Data)
            {
                EndLinkageHisAndCall(item, nowTime, false);
            }

            var triggerDataState = LinkageConstant.TriggerDataStateVlaue;
            var req18 = new AlarmCacheGetAllRequest();
            var allJcbCache = AlarmCacheService.GetAllAlarmCache(req18).Data;
            var time1900 = new DateTime(1900, 1, 1);
            var sievingJcb = allJcbCache.Where(a =>
                (a.Point.Contains("A") || a.Point.Contains("D")) && triggerDataState.Contains(a.Type) &&
                a.Etime == time1900).ToList(); //筛选后的未结束的jcb

            //判断jcb是否有重复数据
            var groupData = sievingJcb.GroupBy(a => new { a.Point, a.Type }).Select(a => new { Group = a.Key, Count = a.Count() });
            if (sievingJcb.Count != groupData.Count())
            {
                StringBuilder repeatStr = new StringBuilder();
                foreach (var item in groupData)
                {
                    if (item.Count > 1)
                    {
                        var repeat = sievingJcb.Where(a => a.Point == item.Group.Point && a.Type == item.Group.Type);
                        foreach (var item2 in repeat)
                        {
                            repeatStr.Append("\r\nPoint:" + item2.Point + " PointId:" + item2.PointID + " JcbId:" +
                                                item2.ID + " Stime:" + item2.Stime + " Etime:" + item2.Etime);
                        }
                    }
                }
                LogHelper.Error("Sys.Safety.Processing.Linkage.LinkageAnalyze：jc_b缓存存在重复数据。" + repeatStr);
            }

            var res = SysEmergencyLinkageService.GetAllSysEmergencyLinkageList();
            var allLinkageConfig = res.Data;
            var allActivityLinkageConfig = allLinkageConfig.Where(a => a.Type == 1 && a.Activity == 1).ToList(); //所有活动的普通应急联动

            var req3 = new PointDefineCacheGetAllRequest();
            var res3 = PointDefineCacheService.GetAllPointDefineCache(req3);
            var allPoint = res3.Data; //所有测点

            foreach (var item in allActivityLinkageConfig)
            {
                //判断是否处于强制解除状态 todo  需要加锁
                if (item.IsForceEnd)
                {
                    var endTime = item.EndTime.AddSeconds(item.DelayTime);
                    if (nowTime <= endTime)
                    {
                        continue;
                    }
                    else
                    {
                        item.IsForceEnd = false;
                    }
                }

                //获取主控测点
                List<Jc_DefInfo> masterPoint = null; //主控测点
                if (item.MasterDevTypeAssId != "0") //主控为设备类型
                {
                    var req2 = new LongIdRequest()
                    {
                        Id = Convert.ToInt64(item.MasterDevTypeAssId)
                    };
                    var res2 = SysEmergencyLinkageService.GetMasterEquTypeInfoByAssId(req2);
                    var allDev = res2.Data;
                    var allDevId = new List<string>();
                    foreach (var item2 in allDev)
                    {
                        allDevId.Add(item2.Devid);
                    }

                    masterPoint = allPoint.Where(a => allDevId.Contains(a.Devid)).ToList();
                }
                if (item.MasterAreaAssId != "0")
                {
                    var req2 = new LongIdRequest()
                    {
                        Id = Convert.ToInt64(item.MasterAreaAssId)
                    };
                    var res2 = SysEmergencyLinkageService.GetMasterAreaInfoByAssId(req2);
                    var allArea = res2.Data;
                    var allAreaId = new List<string>();
                    foreach (var item2 in allArea)
                    {
                        allAreaId.Add(item2.Areaid);
                    }

                    masterPoint = allPoint.Where(a => allAreaId.Contains(a.Areaid)).ToList();
                }
                if (item.MasterPointAssId != "0")
                {
                    var req2 = new LongIdRequest()
                    {
                        Id = Convert.ToInt64(item.MasterPointAssId)
                    };
                    var res2 = SysEmergencyLinkageService.GetMasterPointInfoByAssId(req2);
                    masterPoint = res2.Data;
                }

                if (masterPoint == null)
                {
                    continue;
                }

                var masterPointId = new List<string>(); //主控测点id
                foreach (var item2 in masterPoint)
                {
                    if (item2 == null)
                    {
                        continue;
                    }
                    masterPointId.Add(item2.PointID);
                }

                var duration = item.Duration;

                //获取触发主控状态
                var triDataState = new List<short>();
                foreach (var item2 in item.MasterTriDataStates)
                {
                    triDataState.Add(Convert.ToInt16(item2.DataStateId));
                }

                var satisfyJcb = sievingJcb.Where(a =>
                        (a.State == 21 || a.State == 20 || a.State == 5 || a.State == 24) && masterPointId.Contains(a.PointID) && triDataState.Contains(a.Type) &&
                        (nowTime - a.Stime).TotalSeconds >= duration)
                    .OrderBy(a => a.PointID).ThenBy(a => a.Type).ToList(); //满足条件的jcb

                //去掉设备实时状态为标校、红外遥控的jcb数据
                for (int i = satisfyJcb.Count - 1; i >= 0; i--)
                {
                    var ifExist = allPoint.Any(a => a.PointID == satisfyJcb[i].PointID && (a.State == 5 || a.State == 24));
                    if (ifExist)
                    {
                        satisfyJcb.RemoveAt(i);
                    }
                }

                var req11 = new LongIdRequest()
                {
                    Id = Convert.ToInt64(item.Id)
                };
                var res11 = EmergencyLinkHistoryService.GetNotEndLastLinkageHistoryMasterPointByLinkageId(req11);
                var lastNotEndHisLinkagePointInfo =
                    res11.Data.OrderBy(a => a.PointId).ThenBy(a => a.DataState).ToList(); //当前联动上一次未结束的历史联动记录主控测点

                //判断触发的测点及其状态是否一样
                bool same = true;
                if (satisfyJcb.Count == lastNotEndHisLinkagePointInfo.Count)
                {
                    for (int i = 0; i < satisfyJcb.Count; i++)
                    {
                        if (satisfyJcb[i].PointID != lastNotEndHisLinkagePointInfo[i].PointId ||
                            satisfyJcb[i].Type != lastNotEndHisLinkagePointInfo[i].DataState)
                        {
                            same = false;
                            break;
                        }
                    }
                }
                else
                {
                    same = false;
                }

                if (satisfyJcb.Count != 0) //触发联动
                {
                    if (lastNotEndHisLinkagePointInfo.Count > 0)
                    {
                        if (!same) //主控不一样则先结束之前的联动
                        {
                            EndLinkageHisAndCall(item, nowTime, false);

                            AddLinkageHisAndCall(item, satisfyJcb, nowTime, true);
                        }
                    }
                    else
                    {
                        AddLinkageHisAndCall(item, satisfyJcb, nowTime, true);
                    }
                }
                else //解除联动
                {
                    EndLinkageHisAndCall(item, nowTime, true);
                }
                //}

                //删除2天前的已结束的bcall
                BcallService.DeleteFinishedBcall();
                //删除2天前的已结束的rcall
                RcallService.DeleteFinishedBcall();
            }
        }

        /// <summary>新增历史记录和call,更新实时状态
        /// 
        /// </summary>
        private static void AddLinkageHisAndCall(SysEmergencyLinkageInfo item,
            List<Jc_BInfo> satisfyJcb, DateTime nowTime, bool ifUpdateRealTimeValue)
        {
            var allPosition = PositionService.GetAllPositionCache().Data;

            //新增联动历史记录
            var emergencyLinkHistoryInfo = new EmergencyLinkHistoryInfo()
            {
                SysEmergencyLinkageId = item.Id,
                StartTime = nowTime,
                EndTime = new DateTime(1900, 1, 1),
                IsForceEnd = 0,
                EndPerson = "0"
            };
            var emergencyLinkageHistoryMasterPointAssList = new List<EmergencyLinkageHistoryMasterPointAssInfo>();

            foreach (var item2 in satisfyJcb)
            {
                var newItem = new EmergencyLinkageHistoryMasterPointAssInfo
                {
                    PointId = item2.PointID,
                    DataState = item2.Type
                };
                emergencyLinkageHistoryMasterPointAssList.Add(newItem);
            }

            var req = new AddEmergencyLinkHistoryAndAssRequest
            {
                EmergencyLinkHistoryInfo = emergencyLinkHistoryInfo,
                LinkageHistoryMasterPointAssInfoList = emergencyLinkageHistoryMasterPointAssList
            };
            EmergencyLinkHistoryService.AddEmergencyLinkHistoryAndAss(req);

            //新增人员rcall
            if (item.PassivePersonAssId != "0")
            {
                var req2 = new LongIdRequest()
                {
                    Id = Convert.ToInt64(item.PassivePersonAssId)
                };
                var res2 = SysEmergencyLinkageService.GetPassivePersonByAssId(req2);
                var passivePerson = res2.Data;
                StringBuilder sbPersonCard = new StringBuilder();
                foreach (var item2 in passivePerson)
                {
                    sbPersonCard.Append(item2.Bh + ",");
                }
                if (sbPersonCard.Length > 0)
                {
                    sbPersonCard.Remove(sbPersonCard.Length - 1, 1);
                }

                var newRcall = new R_CallInfo()
                {
                    Id = IdHelper.CreateLongId().ToString(),
                    MasterId = item.Id,
                    Type = 0,
                    CallType = 1,
                    CallPersonDefType = 2,
                    BhContent = sbPersonCard.ToString(),
                    CallTime =DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
                var req5 = new R_CallAddRequest()
                {
                    CallInfo = newRcall
                };
                RcallService.AddCall(req5);
            }

            //被控区域测点及所选测点获取
            List<IdTextCheck> lisAllAssDef = new List<IdTextCheck>(); //所有关联被控测点
            if (item.PassiveAreaAssId != "0")
            {
                var req6 = new LongIdRequest
                {
                    Id = Convert.ToInt64(item.PassiveAreaAssId)
                };
                var res6 = SysEmergencyLinkageService.GetPassiveAreaInfoByAssId(req6);
                var allPassiveArea = res6.Data;
                var allPassiveAreaId = new List<string>();
                foreach (var item2 in allPassiveArea)
                {
                    allPassiveAreaId.Add(item2.Areaid);
                }

                var res7 = SysEmergencyLinkageService.GetAllPassivePointInfo();
                var allPassivePoint = res7.Data;

                var passiveAreaPoint =
                    allPassivePoint.Where(a => allPassiveAreaId.Contains(a.AreaId)).ToList();
                lisAllAssDef.AddRange(passiveAreaPoint);
            }
            if (item.PassivePointAssId != "0")
            {
                var req6 = new LongIdRequest()
                {
                    Id = Convert.ToInt64(item.PassivePointAssId)
                };
                var res6 = SysEmergencyLinkageService.GetPassivePointInfoByAssId(req6);
                lisAllAssDef.AddRange(res6.Data);
            }

            //插入测点rcall
            var handlePersonPoint = lisAllAssDef.Where(a => a.SysId == "11").ToList();
            if (handlePersonPoint.Count > 0)
            {
                var handlePersonPointStr = new StringBuilder(); //人员定位系统关联的被控测点字符串
                foreach (var item2 in handlePersonPoint)
                {
                    handlePersonPointStr.Append(item2.Point + ",");
                }
                handlePersonPointStr.Remove(handlePersonPointStr.Length - 1, 1);

                var rcall = new R_CallInfo()
                {
                    Id = IdHelper.CreateLongId().ToString(),
                    MasterId = item.Id,
                    Type = 1,
                    CallType = 1,
                    CallPersonDefType = 4,
                    PointList = handlePersonPointStr.ToString(),
                    CallTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
                var req8 = new R_CallAddRequest()
                {
                    CallInfo = rcall
                };
                RcallService.AddCall(req8);
            }

            //插入bcall
            var handleBroadcastPoint = lisAllAssDef.Where(a => a.SysId == "12").ToList();
            if (handleBroadcastPoint.Count > 0)
            {
                //var handleBroadcastPointStr = new StringBuilder(); //广播系统关联的被控测点字符串
                //foreach (var item2 in handleBroadcastPoint)
                //{
                //    handleBroadcastPointStr.Append(item2.Point + ",");
                //}
                //handleBroadcastPointStr.Remove(handleBroadcastPointStr.Length - 1, 1);
                var bcallId = IdHelper.CreateLongId().ToString();
                var handleBroadcastPointList = new List<B_CallpointlistInfo>();
                foreach (var item2 in handleBroadcastPoint)
                {
                    var newItem = new B_CallpointlistInfo()
                    {
                        Id = IdHelper.CreateLongId().ToString(),
                        BCallId = bcallId,
                        CalledPointId = item2.Id
                    };
                    handleBroadcastPointList.Add(newItem);
                }

                var broadcastDetail = new StringBuilder(); //广播内容
                broadcastDetail.Append(item.Name + ",");
                foreach (var item2 in satisfyJcb)
                {
                    var wzInfo = allPosition.FirstOrDefault(a => a.WzID == item2.Wzid);
                    var wzName = "";
                    if (wzInfo != null)
                    {
                        wzName = wzInfo.Wz;
                    }

                    broadcastDetail.Append(wzName + "," + EnumHelper.GetEnumDescription((DeviceDataState)item2.Type) + ",");
                }
                broadcastDetail.Remove(broadcastDetail.Length - 1, 1);

                var bcall = new B_CallInfo()
                {
                    Id = bcallId,
                    MasterId = item.Id,
                    CallType = 1,
                    //PointList = handleBroadcastPointStr.ToString(),//注释，调整了表结构，此处需要修改  20171227
                    CallPointList = handleBroadcastPointList,
                    Message = broadcastDetail.ToString(),
                    CallTime =DateTime.Parse( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                };
                var req9 = new B_CallAddRequest()
                {
                    CallInfo = bcall
                };
                BcallService.AddCall(req9);
            }

            //更新实时状态
            if (ifUpdateRealTimeValue)
            {
                var req17 = new UpdateRealTimeStateRequest
                {
                    LinkageId = item.Id,
                    State = "1"
                };
                SysEmergencyLinkageService.UpdateRealTimeState(req17);
            }
        }

        /// <summary>结束之前的历史记录和call
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nowTime"></param>
        private static void EndLinkageHisAndCall(SysEmergencyLinkageInfo item, DateTime nowTime,
            bool ifUpdateRealTimeValue)
        {
            //结束之前的历史记录
            var req12 = new EndByLinkageIdRequest
            {
                EndTime = nowTime,
                Id = item.Id
            };
            EmergencyLinkHistoryService.EndByLinkageId(req12);

            //结束之前的rcall
            RCallInfoGetByMasterIDRequest req13 = new RCallInfoGetByMasterIDRequest();
            req13.CallType = 1;
            if (item.MasterModelId != "0")
                req13.MasterId = item.MasterModelId;
            else
                req13.MasterId = item.Id;

            var res13 = RcallService.GetRCallInfoByMasterID(req13);
            var req14 = new EndRcallByRcallInfoListEequest
            {
                RcallInfo = res13.Data
            };
            RcallService.EndRcallByRcallInfoList(req14);

            //结束之前的bcall
            BCallInfoGetByMasterIDRequest req15 = new BCallInfoGetByMasterIDRequest();
            req15.CallType = 1;
            if (item.MasterModelId != "0")
                req15.MasterId = item.MasterModelId;
            else
                req15.MasterId = item.Id;

            var res15 = BcallService.GetBCallInfoByMasterID(req15);
            var req16 = new EndBcallByBcallInfoListRequest
            {
                Info = res15.Data
            };
            BcallService.EndBcallByBcallInfoList(req16);

            if (ifUpdateRealTimeValue)
            {
                //更新实时状态
                var req10 = new UpdateRealTimeStateRequest
                {
                    LinkageId = item.Id,
                    State = "0"
                };
                SysEmergencyLinkageService.UpdateRealTimeState(req10);
            }
        }

        public static void EndAllLinkageAndCall()
        {
            //结束所有联动历史记录
            var req = new EndAllRequest
            {
                EndTime = DateTime.Now
            };
            EmergencyLinkHistoryService.EndAll(req);

            //结束之前的应急联动rcall
            var res3 = SysEmergencyLinkageService.GetAllSysEmergencyLinkageListDb();
            //var allLinkageConfigInDel = res3.Data.Where(a => a.Type == 1).ToList();
            var allLinkageConfigInDel = res3.Data;
            var allLinkageConfigInDelId = new List<string>(); //所有存储的应急联动id
            foreach (var item in allLinkageConfigInDel)
            {
                if (item.Type == 1)
                {
                    allLinkageConfigInDelId.Add(item.Id);
                }
                else
                {
                    allLinkageConfigInDelId.Add(item.MasterModelId);
                }
            }

            var res2 = RcallService.GetAllCall();
            var allRcall = res2.Data;
            var allNormalRcall = allRcall
                .Where(a => allLinkageConfigInDelId.Contains(a.MasterId) && a.CallType == 1)
                .ToList(); //所有呼叫中的rcall
            var req15 = new EndRcallDbByRcallInfoListEequest
            {
                RcallInfo = allNormalRcall
            };
            RcallService.EndRcallDbByRcallInfoList(req15);

            //结束之前的应急联动bcall
            var req13 = new BasicRequest();
            var res13 = BcallService.GetAll(req13);
            var allBcall = res13.Data;
            var allNormalBcall = allBcall
                .Where(a => allLinkageConfigInDelId.Contains(a.MasterId) && a.CallType == 1)
                .ToList(); //所有呼叫中的bcall
            var req16 = new EndBcallDbByBcallInfoListRequest
            {
                Info = allNormalBcall
            };
            BcallService.EndBcallDbByBcallInfoList(req16);
        }
    }
}
