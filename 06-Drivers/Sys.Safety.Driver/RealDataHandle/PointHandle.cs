using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.ServiceContract.Cache;
using System;
using System.Collections.Generic;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：实时数据处理基类
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public abstract class PointHandle
    {
        /// <summary>
        /// 设备类型集合，模拟量，用于变值变态判断
        /// </summary>
        protected List<Jc_DevInfo> DevItems;
        /// <summary>
        /// 实时数据
        /// </summary>
        protected RealDataItem RealDataItem;
        /// <summary>
        /// 测点信息
        /// </summary>
        protected Jc_DefInfo PointDefineInfo;
        /// <summary>
        /// 数据采集时间
        /// </summary>
        protected DateTime CreatedTime;
        /// <summary>
        /// 设备定义缓存
        /// </summary>
        protected static readonly IDeviceDefineCacheService DeviceDefineCacheService;
        /// <summary>
        /// 测点定义缓存
        /// </summary>
        protected static readonly IPointDefineCacheService PointDefineCacheServicde;
        /// <summary>
        /// 表示实时数据上行的分站类型,0x02,0x16表示老的智能分站(匹配小类)，0x26表示新智能分站（匹配大类），0x00表示大分站（不匹配）。
        /// </summary>
        byte deviceCommperType = 0;
        static PointHandle()
        {
            DeviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
            PointDefineCacheServicde = ServiceFactory.Create<IPointDefineCacheService>();
        }

        /// <summary>
        /// 实时数据处理流程
        /// </summary>
        public Dictionary<string, object> DataHandleFlow(RealDataItem realDataItem, Jc_DefInfo pointDefineInfo, DateTime createdTime, List<Jc_DefInfo> _defItems, List<Jc_DevInfo> _devitems, byte _deviceCommperType)
        {
            Dictionary<string, object> item = null;

            IniData(pointDefineInfo);
            RealDataItem = realDataItem;
            CreatedTime = createdTime;
            DevItems = _devitems;
            deviceCommperType = _deviceCommperType;
            //if (DeviceTypeIsNotMatched()) 2017.11.27 by  把这个判断移到下面去，否则设备定义了不保存巡检，会写一条设备类型不匹配，点保存巡检时，默认置类型为匹配，又会进设备不匹配判断
            {
                //if (PointDefineInfo.PointEditState == 0)
                //{
                    if (DeviceTypeIsNotMatched())
                    {
                        item = DataHandle();
                    }
                //}
                //else
                //{
                //    SafetyHelper.WriteLogInfo("【" + PointDefineInfo.Point + "】状态：" + PointDefineInfo.PointEditState + "不解析");
                //}
            }

            return item;
        }

        public void IniData(Jc_DefInfo pointDefineInfo)
        {
            PointDefineInfo = pointDefineInfo;
        }

        /// <summary>
        /// 数据处理
        /// </summary>
        protected abstract Dictionary<string, object> DataHandle();

        /// <summary>
        /// 预处理
        /// </summary>
        public abstract bool PretreatmentHandle(Jc_DefInfo pointDefineInfo);

        /// <summary>
        /// 测点运行记录
        /// </summary>
        protected abstract void PointRunRecord(DeviceDataState dataState, DeviceRunState runState);

        /// <summary>
        /// 判断设备型号是否匹配true 匹配，false不匹配
        /// </summary>
        /// <returns>true:设备型号不匹配,不执行后续操作;false:设备型号匹配,执行后续操作</returns>
        protected bool DeviceTypeIsNotMatched()
        {
            try
            {
                int datatype = 0;

                if (RealDataItem.State == ItemState.EquipmentTypeError)
                {
                    if (PointDefineInfo.BCommDevTypeMatching)
                    {
                        PointDefineInfo.BCommDevTypeMatching = false;
                        //PointDefineInfo.RealTypeInfo = GetRealTypeInfoStr(RealDataItem.DeviceTypeCode, RealDataItem.RealData);

                        LogHelper.Error("【" + PointDefineInfo.Point + "】类型定义错误（0）,当前定义类型： " + PointDefineInfo.DevName + "应更新为：" + PointDefineInfo.RealTypeInfo);

                        if (PointDefineInfo.DevPropertyID == (int)DeviceProperty.Derail)
                        {
                            PointRunRecord(DeviceDataState.DataDerailState0, DeviceRunState.EquipmentTypeError);    //开关量断线为0态
                        }
                        else
                        {
                            PointRunRecord(DeviceDataState.EquipmentDown, DeviceRunState.EquipmentTypeError);
                        }
                        // PointRunRecord(DeviceDataState.EquipmentDown, DeviceRunState.EquipmentTypeError);
                        PointDefineInfo.Ssz = "断线";
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("BCommDevTypeMatching", PointDefineInfo.BCommDevTypeMatching);
                        //updateItems.Add("RealTypeInfo", PointDefineInfo.RealTypeInfo);
                        updateItems.Add("Zts", PointDefineInfo.Zts);
                        updateItems.Add("Ssz", PointDefineInfo.Ssz);
                        updateItems.Add("Alarm", PointDefineInfo.Alarm);
                        updateItems.Add("DataState", PointDefineInfo.DataState);
                        updateItems.Add("State", PointDefineInfo.State);
                        updateItems.Add("DttStateTime", DateTime.Now);
                        updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                        SafetyHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);

                       // Jc_DefInfo aaaa = KJ73NHelper.GetPointDefinesByPoint(PointDefineInfo.Point);

                        //写JC_R记录
                        //KJ73NHelper.CreateRunLogInfo(PointDefineInfo, CreatedTime, (short)DeviceDataState.EquipmentTypeError, (short)DeviceRunState.EquipmentTypeError, EnumHelper.GetEnumDescription(DeviceDataState.EquipmentTypeError));

                        return false;
                    }
                }
                else
                {
                    if (PointDefineInfo.DevPropertyID == (int)DeviceProperty.Substation)
                    {
                        datatype = 0;//分站不做设备类型不匹配判断    
                    }
                    else
                    {
                        datatype = PointDefineInfo.DevModelID;
                    }

                    //找不到设备类型，直接返回ture，按照正常逻辑处理  2017.7.22 by
                    if ((RealDataItem.DeviceTypeCode != 0 && datatype != 0))
                    {
                        if (datatype != RealDataItem.DeviceTypeCode)
                        {

                            Jc_DevInfo devInfo = SafetyHelper.GetDeviceInfoByDevModelID(RealDataItem.DeviceTypeCode);
                            if (devInfo == null)
                            {
                                LogHelper.Error(PointDefineInfo.Point + "设备回发类型编码" + RealDataItem.DeviceTypeCode + "，但数据库中未找到对应设备(数据按正常处理)！");
                                datatype = 0;
                            }
                            else
                            {
                                //if (deviceCommperType == 0x26)
                                //{
                                //    //匹配大类
                                //    LogHelper.Info("设备类型不匹配，分站类型" + deviceCommperType.ToString("x2") + "进行大类匹配判断！");
                                //    //2017.9.22 by  AI  设备类型不匹配只判断大类
                                //    if (devInfo.Bz3 == PointDefineInfo.DevClassID)
                                //    {
                                //        LogHelper.Error(PointDefineInfo.Point + "设备回发类型编码" + RealDataItem.DeviceTypeCode + "，小类不匹配但大类匹配，按类型匹配处理！");
                                //        datatype = 0;
                                //    }
                                //}
                                //else
                                //{
                                    LogHelper.Info(PointDefineInfo.Point + "设备类型不匹配，分站类型" + deviceCommperType.ToString("x2") + "进行默认（小类）匹配判断！");
                                //}
                                //默认匹配小类
                                //不需要匹配时，下面分站传的类型会是0
                            }
                        }
                    }
                    else
                    {
                        //LogHelper.Info(PointDefineInfo.Point + "回发设备类型" + RealDataItem.DeviceTypeCode + "，定义设备类型" + datatype + "，分站类型" + deviceCommperType.ToString("x2") + "，不进行匹配判断！");
                    }

                    //设备型号等于0且设备匹配标记为false,设备匹配标记置为匹配
                    if ((RealDataItem.DeviceTypeCode == 0 || datatype == 0) && !PointDefineInfo.BCommDevTypeMatching)
                    {
                        PointDefineInfo.BCommDevTypeMatching = true;
                        //PointDefineInfo.RealTypeInfo = GetRealTypeInfoStr(RealDataItem.DeviceTypeCode, RealDataItem.RealData);

                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("BCommDevTypeMatching", PointDefineInfo.BCommDevTypeMatching);
                        //updateItems.Add("RealTypeInfo", PointDefineInfo.RealTypeInfo);
                        updateItems.Add("DttStateTime", DateTime.Now);
                        SafetyHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);
                    }
                    //设备型号不等于0,需判断匹配标记
                    else if (RealDataItem.DeviceTypeCode != 0 && datatype != 0)
                    {
                        //如果设备型号相等且测点定义标记为false,此时设备匹配标记置为匹配
                        if (RealDataItem.DeviceTypeCode == datatype && !PointDefineInfo.BCommDevTypeMatching)
                        {
                            PointDefineInfo.BCommDevTypeMatching = true;
                            //PointDefineInfo.RealTypeInfo = GetRealTypeInfoStr(RealDataItem.DeviceTypeCode, RealDataItem.RealData);
                            Dictionary<string, object> updateItems = new Dictionary<string, object>();
                            updateItems.Add("BCommDevTypeMatching", PointDefineInfo.BCommDevTypeMatching);
                            //updateItems.Add("RealTypeInfo", PointDefineInfo.RealTypeInfo);
                            updateItems.Add("DttStateTime", DateTime.Now);
                            SafetyHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);
                        }
                        //如果设备型号不相等且原设备匹配标记为true,此时判断设备为不匹配
                        else if (RealDataItem.DeviceTypeCode != datatype && PointDefineInfo.BCommDevTypeMatching)
                        {
                            PointDefineInfo.BCommDevTypeMatching = false;
                            //PointDefineInfo.RealTypeInfo = GetRealTypeInfoStr(RealDataItem.DeviceTypeCode, RealDataItem.RealData);
                            PointDefineInfo.RealTypeInfo = SafetyHelper.GetRealTypeInfoStr(RealDataItem.DeviceTypeCode, RealDataItem.RealData);//2017.11.27 by
                            LogHelper.Error("【" + PointDefineInfo.Point + "】类型定义错误（1）,当前定义类型： " + PointDefineInfo.DevName + "应更新为：" + PointDefineInfo.RealTypeInfo);

                            if (PointDefineInfo.DevPropertyID == (int)DeviceProperty.Derail)
                            {
                                PointRunRecord(DeviceDataState.DataDerailState0, DeviceRunState.EquipmentTypeError);    //开关量断线为0态
                            }
                            else
                            {
                                PointRunRecord(DeviceDataState.EquipmentDown, DeviceRunState.EquipmentTypeError);
                            }
                            PointDefineInfo.Ssz = "断线";
                            Dictionary<string, object> updateItems = new Dictionary<string, object>();

                            updateItems.Add("BCommDevTypeMatching", PointDefineInfo.BCommDevTypeMatching);
                            //updateItems.Add("RealTypeInfo", PointDefineInfo.RealTypeInfo);
                            updateItems.Add("Zts", PointDefineInfo.Zts);
                            updateItems.Add("Ssz", PointDefineInfo.Ssz);
                            updateItems.Add("Alarm", PointDefineInfo.Alarm);
                            updateItems.Add("DataState", PointDefineInfo.DataState);
                            updateItems.Add("State", PointDefineInfo.State);
                            updateItems.Add("DttStateTime", DateTime.Now);
                            updateItems.Add("DttRunStateTime", PointDefineInfo.DttRunStateTime);
                            SafetyHelper.UpdatePointDefineInfoByProperties(PointDefineInfo.PointID, updateItems);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeviceTypeIsNotMatched Error【" + PointDefineInfo.Point + "】" + "DeviceTypeCode = " + RealDataItem.DeviceTypeCode + "   ---   " + ex.Message);
            }
            return PointDefineInfo.BCommDevTypeMatching;
        }

        ///// <summary>
        ///// 或取网关回发的设备信息及实时值
        ///// </summary>
        ///// <param name="devModelID"></param>
        ///// <param name="ssz"></param>
        ///// <returns></returns>
        //private string GetRealTypeInfoStr(int devModelID, string ssz)
        //{
        //    string realTypeInfo = "";

        //    Jc_DevInfo devInfo = KJ73NHelper.GetDeviceInfoByDevModelID(devModelID, PointDefineInfo.Point);
        //    if (devInfo != null)
        //    {
        //        realTypeInfo = devInfo.Name + "【" + ssz + "】";
        //    }
        //    else
        //    {
        //        LogHelper.Error("设备回发类型编码" + PointDefineInfo.Point + "----" + devModelID + "，但数据库中未找到对应设备！");
        //    }

        //    return realTypeInfo;
        //}
    }
}
