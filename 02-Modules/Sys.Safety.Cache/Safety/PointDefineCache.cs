using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 测点只读缓存模块
    /// added by  20170719
    /// </summary>
    public class PointReadonlyCache : KJ73NCacheManager<Jc_DefInfo>
    {
        private static volatile PointReadonlyCache _instance;

        private bool _isLoaded = false;

        /// <summary>
        /// 创建测点只读缓存
        /// </summary>
        public static PointReadonlyCache GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new PointReadonlyCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private PointReadonlyCache()
        {
            //启动一个同步线程
            Thread syncThread = new Thread(SyncCacheData);
            syncThread.IsBackground = true;
            syncThread.Start();
        }

        /// <summary>
        /// 加载 数据
        /// </summary>
        public override void Load()
        {
            var list = PointWriteCache.GetInstance.Query();
            _cache.Clear();
            _cache = list;

            _isLoaded = true;
        }
        public void Stop()
        {
            _isLoaded = false;
        }
        /// <summary>
        /// 定时从主同步数据
        /// </summary>
        private void SyncCacheData()
        {
            while (_isLoaded)
            {

                try
                {
                    //如果已经加载，则开始同步数据
                    _rwLocker.AcquireWriterLock(-1);
                    var list = PointWriteCache.GetInstance.Query();

                    //_cache.Clear();//取消缓存深复制，存在引用关系  20170814
                    _cache = list;
                }
                catch (Exception ex)
                {
                    LogHelper.Error("测点只读缓存同步最新缓存数据失败，原因：" + ex.ToString());
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }

                Thread.Sleep(1000 * 1);
            }
        }

        protected override void AddEntityToCache(Jc_DefInfo item)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateEntityToCache(Jc_DefInfo item)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteEntityFromCache(Jc_DefInfo item)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:测点定义缓存（写缓存）
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public class PointWriteCache : KJ73NCacheManager<Jc_DefInfo>
    {
        private readonly IPointDefineService pointDefineService;

        /// <summary>
        /// 更新缓存队列计数器
        /// </summary>
        public long pointCacheUpCount { get; set; }

        private static volatile PointWriteCache _instance;
        /// <summary>
        /// 测点定义单例
        /// </summary>
        public static PointWriteCache GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (obj)
                    {
                        if (_instance == null)
                        {
                            _instance = new PointWriteCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private PointWriteCache()
        {
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
        }

        public override void Load()
        {
            //try
            //{
            var respose = pointDefineService.GetPointDefineList();
            if (respose.Data != null && respose.IsSuccess)
            {
                _cache.Clear();
                var data = respose.Data;
                data.ForEach(item => { item.Ssz = ""; });   //ssz全部赋值成“” 2017.6.13 by
                _cache = respose.Data.FindAll(a => a.Activity == "1").OrderBy(a => a.Point).ToList();//只加载活动点的数据  20170602
                //_cache = respose.Data.FindAll(a => a.Fzh == 2 && a.Kh == 0 && a.Activity == "1");
            }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
            //}
        }

        protected override void AddEntityToCache(Jc_DefInfo item)
        {
            if (_cache.Count(pointdefine => pointdefine.PointID == item.PointID) < 1)//修改条件，根据PointID判断  20170606
                _cache.Add(item);
        }

        protected override void DeleteEntityFromCache(Jc_DefInfo item)
        {
            var tempitem = _cache.FirstOrDefault(devdefine => devdefine.PointID == item.PointID);
            if (tempitem != null)//修改条件，根据PointID判断  20170606
                _cache.Remove(tempitem);
        }

        protected override void UpdateEntityToCache(Jc_DefInfo item)
        {
            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);//修改条件，根据PointID判断  20170606
            if (itemIndex >= 0)
            {
                _cache[itemIndex] = item;
                pointCacheUpCount++;
            }
        }

        public void UpdateControlInfo(Jc_DefInfo item)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].BDisCharge = item.BDisCharge;
                    _cache[itemIndex].ClsCommObj.BInit = item.ClsCommObj.BInit;
                    _cache[itemIndex].ClsCommObj.NControlMark = item.ClsCommObj.NControlMark;
                    _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;
                    _cache[itemIndex].ClsCommObj.BSendControlCommand = item.ClsCommObj.BSendControlCommand;
                    _cache[itemIndex].realControlCount = item.realControlCount;
                    _cache[itemIndex].sendDTime = item.sendDTime;
                    _cache[itemIndex].SoleCodingChanels = item.SoleCodingChanels;
                    _cache[itemIndex].DeviceControlItems = item.DeviceControlItems;
                    _cache[itemIndex].GasThreeUnlockContro = item.GasThreeUnlockContro;
                    _cache[itemIndex].GradingAlarmCount = item.GradingAlarmCount;
                    _cache[itemIndex].GradingAlarmItems = item.GradingAlarmItems;
                    _cache[itemIndex].GradingAlarmTime = item.GradingAlarmTime;

                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义控制信息缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateCommTimesInfo(Jc_DefInfo item, int updateType)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    switch (updateType)
                    {
                        case 0:
                            _cache[itemIndex].ClsCommObj.NComTest_TotalCount = item.ClsCommObj.NComTest_TotalCount;
                            _cache[itemIndex].ClsCommObj.NCommCount = item.ClsCommObj.NCommCount;
                            break;
                        case 1:
                            _cache[itemIndex].ClsCommObj.NComTest_TotalCount = item.ClsCommObj.NComTest_TotalCount;
                            break;
                        case 2:
                            _cache[itemIndex].ClsCommObj.NCommCount = item.ClsCommObj.NCommCount;
                            break;
                    }
                    _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义初始化次数缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateInitInfo(Jc_DefInfo item)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].BEdit = item.BEdit;
                    _cache[itemIndex].DttkdStrtime = item.DttkdStrtime;
                    _cache[itemIndex].DttRunStateTime = item.DttRunStateTime;
                    _cache[itemIndex].NCtrlSate = item.NCtrlSate;
                    _cache[itemIndex].DataState = item.DataState;
                    _cache[itemIndex].Ssz = item.Ssz;
                    _cache[itemIndex].BCommDevTypeMatching = item.BCommDevTypeMatching;
                    _cache[itemIndex].Alarm = item.Alarm;
                    _cache[itemIndex].AreaName = item.AreaName;//区域名称 
                    _cache[itemIndex].AreaLoc = item.AreaLoc; //所属区域编码 
                    _cache[itemIndex].XCoordinate = item.XCoordinate;//经度
                    _cache[itemIndex].YCoordinate = item.YCoordinate;//纬度
                    _cache[itemIndex].Wz = item.Wz;//扩展位置 
                    _cache[itemIndex].DevName = item.DevName;//扩展设备名称
                    _cache[itemIndex].DevPropertyID = item.DevPropertyID;//扩展设备性质ID
                    _cache[itemIndex].DevClassID = item.DevClassID;//扩展设备种类ID                   
                    _cache[itemIndex].DevModelID = item.DevModelID;//扩展设备型号ID
                    _cache[itemIndex].Unit = item.Unit;//设备单位
                    _cache[itemIndex].Sysid = item.Sysid;   //系统编号，上位机系统驱动编号 
                    _cache[itemIndex].DevProperty = item.DevProperty;//扩展设备性质
                    _cache[itemIndex].DevClass = item.DevClass;//扩展设备种类
                    _cache[itemIndex].DevModel = item.DevModel;//扩展设备型号
                    _cache[itemIndex].sendIniCount = item.sendIniCount;
                    _cache[itemIndex].PointEditState = item.PointEditState;
                    if (_cache[itemIndex].ClsCommObj == null)
                    {
                        _cache[itemIndex].ClsCommObj = new CommProperty((uint)_cache[itemIndex].Fzh);
                    }
                    _cache[itemIndex].ClsCommObj.BInit = item.ClsCommObj.BInit;
                    _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;

                    if (_cache[itemIndex].ClsAlarmObj == null)
                    {
                        _cache[itemIndex].ClsAlarmObj = new AlarmProperty();
                    }
                    if (_cache[itemIndex].ClsCtrlObj == null)
                    {
                        _cache[itemIndex].ClsCtrlObj = new List<ControlRemote>();
                    }
                }
                else
                {
                    _cache.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义初始化缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateStationFlagInfo(bool isInit)
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                _cache.ForEach(pointdefine => pointdefine.ClsCommObj.BInit = isInit);
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义初始化标记缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
        public void UpdateUniqueCodeInfo(Jc_DefInfo item)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].Bz13 = item.Bz13;
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义唯一编码缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateErrorCount(Jc_DefInfo item)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].NErrCount = item.NErrCount;
                }
                else
                {
                    _cache.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义错误次数缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateRealValueInfo(Jc_DefInfo item)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.Point == item.Point);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].Ssz = item.Ssz;
                    _cache[itemIndex].State = item.State;
                    _cache[itemIndex].DataState = item.DataState;
                    _cache[itemIndex].Alarm = item.Alarm;
                    _cache[itemIndex].Voltage = item.Voltage;
                    _cache[itemIndex].Zts = item.Zts;
                    _cache[itemIndex].BatteryItems = item.BatteryItems;

                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义实时信息缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateModifyItems(Jc_DefInfo item)
        {
            if (item == null)
                return;

            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].ModificationItems = item.ModificationItems;
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义传感器地址缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        public void UpdateStationUniqueCodeConfirm(Jc_DefInfo item)
        {
            if (item == null)
                return;
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
                if (itemIndex >= 0)
                {
                    _cache[itemIndex].SoleCodingChanels = item.SoleCodingChanels;
                    _cache[itemIndex].ClsCommObj.NCommand = item.ClsCommObj.NCommand;
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("更新测点定义唯一编码确认链表缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }

        }

        public void UpdatePointInfo(string PointKey, Dictionary<string, object> updateItems)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            _rwLocker.AcquireWriterLock(-1);
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePointInfo Lock：" + sw.ElapsedMilliseconds);
            }
            sw.Restart();
            try
            {
                Jc_DefInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.PointID == PointKey);
                if (iteminfo != null)
                {
                    iteminfo.CopyProperties(updateItems);
                    pointCacheUpCount++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("部分更新测点定义缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
            sw.Stop();
            if (sw.ElapsedMilliseconds > 0)
            {
                LogHelper.Warn("UpdatePointInfo：" + sw.ElapsedMilliseconds);
            }
        }

        public void BatchUpdatePointInfo(Dictionary<string, Dictionary<string, object>> pointItems)
        {
            _rwLocker.AcquireWriterLock(-1);

            try
            {
                foreach (string pointId in pointItems.Keys)
                {
                    Jc_DefInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.PointID == pointId);
                    if (iteminfo != null)
                    {
                        iteminfo.CopyProperties(pointItems[pointId]);
                        pointCacheUpCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("批量部分更新测点定义缓存信息失败：" + ex.Message);
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }
    }


















    ///// <summary>
    ///// 作者:
    ///// 时间:2017-05-24
    ///// 描述:测点定义缓存
    ///// 修改记录
    ///// 2017-05-24
    ///// </summary>
    //public class PointDefineCache : KJ73NCacheManager<Jc_DefInfo>
    //{
    //    private readonly IPointDefineService pointDefineService;

    //    /// <summary>
    //    /// 更新缓存队列计数器
    //    /// </summary>
    //    public long pointDefineCacheUpCount { get; set; }

    //    private static volatile PointDefineCache pointDefineCahceInstance;
    //    /// <summary>
    //    /// 测点定义单例
    //    /// </summary>
    //    public static PointDefineCache PointDefineCahceInstance
    //    {
    //        get
    //        {
    //            if (pointDefineCahceInstance == null)
    //            {
    //                lock (obj)
    //                {
    //                    if (pointDefineCahceInstance == null)
    //                    {
    //                        pointDefineCahceInstance = new PointDefineCache();
    //                    }
    //                }
    //            }
    //            return pointDefineCahceInstance;
    //        }
    //    }

    //    private PointDefineCache()
    //    {
    //        pointDefineService = ServiceFactory.Create<IPointDefineService>();
    //    }

    //    public override void Load()
    //    {
    //        //try
    //        //{
    //            var respose = pointDefineService.GetPointDefineList();
    //            if (respose.Data != null && respose.IsSuccess)
    //            {
    //                _cache.Clear();
    //                var data = respose.Data;
    //                data.ForEach(item => { item.Ssz = ""; });   //ssz全部赋值成“” 2017.6.13 by
    //                _cache = respose.Data.FindAll(a => a.Activity == "1").OrderBy(a => a.Point).ToList();//只加载活动点的数据  20170602
    //                //_cache = respose.Data.FindAll(a => a.Fzh == 2 && a.Kh == 0 && a.Activity == "1");
    //            }
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    LogHelper.Error(string.Format("{0}:加载缓存失败！" + "\r\n" + ex.Message, CacheName));
    //        //}
    //    }

    //    protected override void AddEntityToCache(Jc_DefInfo item)
    //    {
    //        if (_cache.Count(pointdefine => pointdefine.PointID == item.PointID) < 1)//修改条件，根据PointID判断  20170606
    //            _cache.Add(item);
    //    }

    //    protected override void DeleteEntityFromCache(Jc_DefInfo item)
    //    {
    //        var tempitem = _cache.FirstOrDefault(devdefine => devdefine.PointID == item.PointID);
    //        if (tempitem != null)//修改条件，根据PointID判断  20170606
    //            _cache.Remove(tempitem);
    //    }

    //    protected override void UpdateEntityToCache(Jc_DefInfo item)
    //    {
    //        int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);//修改条件，根据PointID判断  20170606
    //        if (itemIndex >= 0)
    //        {
    //            _cache[itemIndex] = item;
    //            pointDefineCacheUpCount++;
    //        }
    //    }

    //    public void UpdateControlInfo(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].BDisCharge = item.BDisCharge;
    //                _cache[itemIndex].ClsCommObj.BInit = item.ClsCommObj.BInit;
    //                _cache[itemIndex].ClsCommObj.NControlMark = item.ClsCommObj.NControlMark;
    //                _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;
    //                _cache[itemIndex].ClsCommObj.BSendControlCommand = item.ClsCommObj.BSendControlCommand;
    //                _cache[itemIndex].realControlCount = item.realControlCount;
    //                _cache[itemIndex].sendDTime = item.sendDTime;
    //                _cache[itemIndex].SoleCodingChanels = item.SoleCodingChanels;
    //                _cache[itemIndex].DeviceControlItems = item.DeviceControlItems;
    //                _cache[itemIndex].GasThreeUnlockContro = item.GasThreeUnlockContro;
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义控制信息缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateCommTimesInfo(Jc_DefInfo item, int updateType)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                switch (updateType)
    //                {
    //                    case 0:
    //                        _cache[itemIndex].ClsCommObj.NComTest_TotalCount = item.ClsCommObj.NComTest_TotalCount;
    //                        _cache[itemIndex].ClsCommObj.NCommCount = item.ClsCommObj.NCommCount;
    //                        break;
    //                    case 1:
    //                        _cache[itemIndex].ClsCommObj.NComTest_TotalCount = item.ClsCommObj.NComTest_TotalCount;
    //                        break;
    //                    case 2:
    //                        _cache[itemIndex].ClsCommObj.NCommCount = item.ClsCommObj.NCommCount;
    //                        break;
    //                }
    //                _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义初始化次数缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateInitInfo(Jc_DefInfo item)
    //    {
    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].BEdit = item.BEdit;
    //                _cache[itemIndex].DttkdStrtime = item.DttkdStrtime;
    //                _cache[itemIndex].DttRunStateTime = item.DttRunStateTime;
    //                _cache[itemIndex].NCtrlSate = item.NCtrlSate;
    //                _cache[itemIndex].DataState = item.DataState;
    //                _cache[itemIndex].Ssz = item.Ssz;
    //                _cache[itemIndex].BCommDevTypeMatching = item.BCommDevTypeMatching;
    //                _cache[itemIndex].Alarm = item.Alarm;
    //                _cache[itemIndex].AreaName = item.AreaName;//区域名称 
    //                _cache[itemIndex].AreaLoc = item.AreaLoc; //所属区域编码 
    //                _cache[itemIndex].XCoordinate = item.XCoordinate;//经度
    //                _cache[itemIndex].YCoordinate = item.YCoordinate;//纬度
    //                _cache[itemIndex].Wz = item.Wz;//扩展位置 
    //                _cache[itemIndex].DevName = item.DevName;//扩展设备名称
    //                _cache[itemIndex].DevPropertyID = item.DevPropertyID;//扩展设备性质ID
    //                _cache[itemIndex].DevClassID = item.DevClassID;//扩展设备种类ID                   
    //                _cache[itemIndex].DevModelID = item.DevModelID;//扩展设备型号ID
    //                _cache[itemIndex].Unit = item.Unit;//设备单位
    //                _cache[itemIndex].Sysid = item.Sysid;   //系统编号，上位机系统驱动编号 
    //                _cache[itemIndex].DevProperty = item.DevProperty;//扩展设备性质
    //                _cache[itemIndex].DevClass = item.DevClass;//扩展设备种类
    //                _cache[itemIndex].DevModel = item.DevModel;//扩展设备型号
    //                _cache[itemIndex].sendIniCount = item.sendIniCount;
    //                _cache[itemIndex].PointEditState = item.PointEditState;
    //                if (_cache[itemIndex].ClsCommObj == null)
    //                {
    //                    _cache[itemIndex].ClsCommObj = new CommProperty((uint)_cache[itemIndex].Fzh);
    //                }
    //                _cache[itemIndex].ClsCommObj.BInit = item.ClsCommObj.BInit;
    //                _cache[itemIndex].ClsCommObj.NCommandbz = item.ClsCommObj.NCommandbz;

    //                if (_cache[itemIndex].ClsAlarmObj == null)
    //                {
    //                    _cache[itemIndex].ClsAlarmObj = new AlarmProperty();
    //                }
    //                if (_cache[itemIndex].ClsCtrlObj == null)
    //                {
    //                    _cache[itemIndex].ClsCtrlObj = new List<ControlRemote>();
    //                }
    //            }
    //            else
    //            {
    //                _cache.Add(item);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义初始化缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateStationFlagInfo(bool isInit)
    //    {
    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            _cache.ForEach(pointdefine => pointdefine.ClsCommObj.BInit = isInit);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义初始化标记缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }
    //    public void UpdateUniqueCodeInfo(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].Bz13 = item.Bz13;
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义唯一编码缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateErrorCount(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].NErrCount = item.NErrCount;
    //            }
    //            else
    //            {
    //                _cache.Add(item);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义错误次数缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateRealValueInfo(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.Point == item.Point);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].Ssz = item.Ssz;
    //                _cache[itemIndex].State = item.State;
    //                _cache[itemIndex].DataState = item.DataState;
    //                _cache[itemIndex].Alarm = item.Alarm;
    //                _cache[itemIndex].Voltage = item.Voltage;
    //                _cache[itemIndex].Zts = item.Zts;
    //                _cache[itemIndex].BatteryItems = item.BatteryItems;

    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义实时信息缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateModifyItems(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;

    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].ModificationItems = item.ModificationItems;
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex) 
    //        {
    //            LogHelper.Error("更新测点定义传感器地址缓存信息失败：" + ex.Message);
    //        }
    //        finally 
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //    }

    //    public void UpdateStationUniqueCodeConfirm(Jc_DefInfo item)
    //    {
    //        if (item == null)
    //            return;
    //        _rwLocker.AcquireWriterLock(-1);
    //        try
    //        {
    //            int itemIndex = _cache.FindIndex(pointdefine => pointdefine.PointID == item.PointID);
    //            if (itemIndex >= 0)
    //            {
    //                _cache[itemIndex].SoleCodingChanels = item.SoleCodingChanels;
    //                _cache[itemIndex].ClsCommObj.NCommand = item.ClsCommObj.NCommand;
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("更新测点定义唯一编码确认链表缓存信息失败：" + ex.Message);
    //        }
    //        finally
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }

    //    }

    //    public void UpdatePointInfo(string PointKey, Dictionary<string, object> updateItems)
    //    {
    //        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
    //        sw.Start();
    //        _rwLocker.AcquireWriterLock(-1);
    //        sw.Stop();
    //        if (sw.ElapsedMilliseconds > 0)
    //        {
    //            LogHelper.Warn("UpdatePointInfo Lock：" + sw.ElapsedMilliseconds);
    //        }
    //        sw.Restart();
    //        try
    //        {
    //            Jc_DefInfo iteminfo = _cache.FirstOrDefault(pointdefine => pointdefine.PointID == PointKey);
    //            if (iteminfo != null)
    //            {
    //                iteminfo.CopyProperties(updateItems);
    //                pointDefineCacheUpCount++;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            LogHelper.Error("部分更新测点定义缓存信息失败：" + ex.Message);
    //        }
    //        finally 
    //        {
    //            _rwLocker.ReleaseWriterLock();
    //        }
    //        sw.Stop();
    //        if (sw.ElapsedMilliseconds > 0)
    //        {
    //            LogHelper.Warn("UpdatePointInfo：" + sw.ElapsedMilliseconds);
    //        }
    //    }
    //}
}
