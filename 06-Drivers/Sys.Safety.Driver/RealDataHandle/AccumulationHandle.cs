using System;
using Sys.Safety.Enums;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract.DataToDb;
using Basic.Framework.Service;
using Sys.Safety.Request.DataToDb;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Basic.Framework.Common;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request;
using System.Collections.Generic;
using System.Linq;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 作者：
    /// 时间：2017-05-31
    /// 描述：累积量实时数据处理
    /// 修改记录
    /// 2017-05-31
    /// </summary>
    public class AccumulationHandle
    {
        private static readonly IInsertToDbService<Jc_Ll_HInfo> accumulationHourDataToDbService;
        private static readonly IInsertToDbService<Jc_Ll_DInfo> accumulationDayDataToDbService;
        private static readonly IInsertToDbService<Jc_Ll_MInfo> accumulationMonthDataToDbService;
        private static readonly IInsertToDbService<Jc_Ll_YInfo> accumulationYearDataToDbService;
        private static readonly IAccumulationDayService accumulationDayService;
        private static readonly IAccumulationHourService accumulationHourService;
        private static readonly IAccumulationMonthService accumulationMonthService;
        private static readonly IAccumulationYearService accumulationYearService;

        /// <summary>
        ///  累计量所属分站PointID
        /// </summary>
        string pointID = "";

        DateTime _creatTime;

        static AccumulationHandle()
        {
            accumulationHourDataToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_HInfo>>();
            accumulationDayDataToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_DInfo>>();
            accumulationMonthDataToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_MInfo>>();
            accumulationYearDataToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_YInfo>>();
            accumulationDayService = ServiceFactory.Create<IAccumulationDayService>();
            accumulationHourService = ServiceFactory.Create<IAccumulationHourService>();
            accumulationMonthService = ServiceFactory.Create<IAccumulationMonthService>();
            accumulationYearService = ServiceFactory.Create<IAccumulationYearService>();
        }

        public void DataHandle(Jc_DefInfo stationInfo, List<RealDataItem> realdataItems, DateTime creatTime, List<Jc_DefInfo> defItems)
        {
            try
            {
                Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();

                pointID = stationInfo.PointID;
                _creatTime = creatTime;
                //List<Jc_DefInfo> defItems = KJ73NHelper.GetPointDefinesByStationID(stationInfo.Fzh);//hdw1
                Dictionary<string, object> updateItems;
                Jc_Ll_HInfo accumulationHInfo = new Jc_Ll_HInfo();
                Jc_Ll_DInfo accumulationDInfo = new Jc_Ll_DInfo();
                Jc_Ll_MInfo accumulationMInfo = new Jc_Ll_MInfo();
                Jc_Ll_YInfo accumulationYInfo = new Jc_Ll_YInfo();
                bool hflag = false;
                bool dflag = false;
                bool mflag = false;
                bool yflag = false;

                realdataItems.ForEach(item =>
                {
                    Jc_DefInfo pointDefineInfo = defItems.FirstOrDefault(a => a.DevPropertyID == (int)item.DeviceProperty && a.Kh.ToString() == item.Channel && a.Dzh.ToString() == item.Address);
                    if (pointDefineInfo != null)
                    {
                        //txy  20180720 每次都赋值  值未改变也赋值 根据标记存储
                        //if (pointDefineInfo.Ssz != item.RealData || pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//值变化存储 ，每小时至少存储一次数据 2017.6.30 by (抽放分站上传的数据时间是精确到小时的)
                        //{
                        updateItems = new Dictionary<string, object>();
                        int channel = Convert.ToInt32(item.Channel);
                        if (channel <= 4)
                        {
                            if (pointDefineInfo.Ssz != item.RealData ||
                    pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//txy 20180720 值变化标记 或每小时更新一次
                            {
                                hflag = true;
                            }
                            CreateAccumulationHInfo(accumulationHInfo, channel, item.RealData);
                        }
                        else if (channel >= 5 && channel <= 8)
                        {
                            if (pointDefineInfo.Ssz != item.RealData ||
                    pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//txy 20180720 值变化标记 或每小时更新一次
                            {
                                dflag = true;
                            }
                            CreateAccumulationDInfo(accumulationDInfo, channel, item.RealData);
                        }
                        else if (channel >= 9 && channel <= 12)
                        {
                            if (pointDefineInfo.Ssz != item.RealData ||
                    pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//txy 20180720 值变化标记 或每小时更新一次
                            {
                                mflag = true;
                            }
                            CreateAccumulationMInfo(accumulationMInfo, channel, item.RealData);
                        }
                        else if (channel >= 13 && channel <= 16)
                        {
                            if (pointDefineInfo.Ssz != item.RealData ||
                    pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//txy 20180720 值变化标记  或每小时更新一次
                            {
                                yflag = true;
                            }
                            CreateAccumulationYInfo(accumulationYInfo, channel, item.RealData);
                        }
                        if (pointDefineInfo.Ssz != item.RealData ||
                    pointDefineInfo.DttRunStateTime.Hour != creatTime.Hour)//txy 20180720 值变化判断 变化才更新实时值 或每小时更新一次
                        {
                            pointDefineInfo.Zts = _creatTime;
                            pointDefineInfo.State = (short)item.State;
                            pointDefineInfo.DataState = (short)DeviceRunState.EquipmentCommOK;
                            pointDefineInfo.Ssz = item.RealData;
                            //if (pointDefineInfo.Ssz.Trim() == "")
                            //{
                            //    LogHelper.Info(pointDefineInfo.Point + "ssz = 空");
                            //}
                            pointDefineInfo.InfoState = InfoState.NoChange;
                            pointDefineInfo.DttRunStateTime = _creatTime;
                            //KJ73NHelper.UpdatePointDefineInfo(pointDefineInfo, 5);
                            updateItems.Add("Zts", pointDefineInfo.Zts);
                            updateItems.Add("State", pointDefineInfo.State);
                            updateItems.Add("DataState", pointDefineInfo.DataState);
                            updateItems.Add("Ssz", pointDefineInfo.Ssz);
                            updateItems.Add("DttStateTime", DateTime.Now);
                            updateItems.Add("DttRunStateTime", pointDefineInfo.DttRunStateTime);
                            UpdateItemsList.Add(pointDefineInfo.PointID, updateItems);
                            //KJ73NHelper.UpdatePointDefineInfoByProperties(pointDefineInfo.PointID, updateItems);
                        }
                        //}
                    }
                });

                if (UpdateItemsList.Count > 0)
                {
                    SafetyHelper.BatchUpdatePointDefineInfoByProperties(UpdateItemsList);
                }

                if (hflag)
                {
                    InsertAccumulationHInfo(accumulationHInfo);
                }
                if (dflag)
                {
                    InsertAccumulationDInfo(accumulationDInfo);
                }
                if (mflag)
                {
                    InsertAccumulationMInfo(accumulationMInfo);
                }
                if (yflag)
                {
                    InsertAccumulationYInfo(accumulationYInfo);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("AccumulationHandle:" + stationInfo.Point + "处理数据出错，原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加累积量小时数据
        /// </summary>
        /// <param name="channel"></param>
        private void CreateAccumulationHInfo(Jc_Ll_HInfo accumulationHInfo, int channel, string realdata)
        {
            switch (channel)
            {
                case 1:
                    accumulationHInfo.BHL = float.Parse(realdata);
                    break;
                case 2:
                    accumulationHInfo.BCL = float.Parse(realdata);
                    break;
                case 3:
                    accumulationHInfo.GHL = float.Parse(realdata);
                    break;
                case 4:
                    accumulationHInfo.GCL = float.Parse(realdata);
                    break;
            }
        }

        private void InsertAccumulationHInfo(Jc_Ll_HInfo accumulationHInfo)
        {
            DateTime timer = Convert.ToDateTime(_creatTime.ToString("yyyy-MM-dd HH:00:00"));
            AccumulationHourExistsRequest request = new AccumulationHourExistsRequest
            {
                Timer = timer,
                PointId = pointID
            };
            var hourresponse = accumulationHourService.ExistsAccumulationHourInfo(request);
            if (hourresponse != null && hourresponse.IsSuccess && hourresponse.Data != null)
            {
                accumulationHInfo.ID = hourresponse.Data.ID;
                accumulationHInfo.InfoState = InfoState.Modified;
            }
            else
            {
                accumulationHInfo.InfoState = InfoState.AddNew;
                accumulationHInfo.ID = IdHelper.CreateLongId().ToString();
                LogHelper.Info(pointID + " H " + timer.ToString("yyy-MM-dd HH:mm:ss") + "未找到！");
            }
            accumulationHInfo.Timer = timer;
            accumulationHInfo.PointID = pointID;
            DataToDbAddRequest<Jc_Ll_HInfo> addrequest = new DataToDbAddRequest<Jc_Ll_HInfo>()
            {
                Item = accumulationHInfo
            };
            accumulationHourDataToDbService.AddItem(addrequest);//hdw1:是否加了锁
        }

        /// <summary>
        /// 添加累积量日数据
        /// </summary>
        /// <param name="channel"></param>
        private void CreateAccumulationDInfo(Jc_Ll_DInfo accumulationDInfo, int channel, string realdata)
        {
            switch (channel)
            {
                case 5:
                    accumulationDInfo.BHL = decimal.Parse(realdata);
                    break;
                case 6:
                    accumulationDInfo.BCL = decimal.Parse(realdata);
                    break;
                case 7:
                    accumulationDInfo.GHL = decimal.Parse(realdata);
                    break;
                case 8:
                    accumulationDInfo.GCL = decimal.Parse(realdata);
                    break;
            }
        }

        private void InsertAccumulationDInfo(Jc_Ll_DInfo accumulationDInfo)
        {
            DateTime timer = Convert.ToDateTime(_creatTime.ToString("yyyy-MM-dd 00:00:00"));
            AccumulationDayExistsRequest request = new AccumulationDayExistsRequest
            {
                Timer = timer,
                PointId = pointID
            };
            var dayresponse = accumulationDayService.ExistsAccumulationDayInfo(request);
            if (dayresponse != null && dayresponse.IsSuccess && dayresponse.Data != null)
            {
                accumulationDInfo.ID = dayresponse.Data.ID;
                accumulationDInfo.InfoState = InfoState.Modified;
            }
            else
            {
                accumulationDInfo.InfoState = InfoState.AddNew;
                accumulationDInfo.ID = IdHelper.CreateLongId().ToString();
                LogHelper.Info(pointID + " D " + timer.ToString("yyy-MM-dd HH:mm:ss") + "未找到！");
            }
            accumulationDInfo.Timer = timer;
            accumulationDInfo.PointID = pointID;
            DataToDbAddRequest<Jc_Ll_DInfo> addrequest = new DataToDbAddRequest<Jc_Ll_DInfo>()
            {
                Item = accumulationDInfo
            };
            accumulationDayDataToDbService.AddItem(addrequest);
        }

        /// <summary>
        /// 添加累积量月数据
        /// </summary>
        /// <param name="channel"></param>
        private void CreateAccumulationMInfo(Jc_Ll_MInfo accumulationMInfo, int channel, string realdata)
        {
            switch (channel)
            {
                case 9:
                    accumulationMInfo.BHL = decimal.Parse(realdata);
                    break;
                case 10:
                    accumulationMInfo.BCL = decimal.Parse(realdata);
                    break;
                case 11:
                    accumulationMInfo.GHL = decimal.Parse(realdata);
                    break;
                case 12:
                    accumulationMInfo.GCL = decimal.Parse(realdata);
                    break;
            }
        }

        private void InsertAccumulationMInfo(Jc_Ll_MInfo accumulationMInfo)
        {
            DateTime timer = Convert.ToDateTime(_creatTime.ToString("yyyy-MM-01 00:00:00"));
            AccumulationMonthExistsRequest request = new AccumulationMonthExistsRequest
            {
                Timer = timer,
                PointId = pointID
            };
            var monthresponse = accumulationMonthService.ExistsAccumulationMonthInfo(request);
            if (monthresponse != null && monthresponse.IsSuccess && monthresponse.Data != null)
            {
                accumulationMInfo.ID = monthresponse.Data.ID;
                accumulationMInfo.InfoState = InfoState.Modified;
            }
            else
            {
                accumulationMInfo.InfoState = InfoState.AddNew;
                accumulationMInfo.ID = IdHelper.CreateLongId().ToString();
                LogHelper.Info(pointID + " M " + timer.ToString("yyy-MM-dd HH:mm:ss") + "未找到！");
            }
            accumulationMInfo.Timer = timer;
            accumulationMInfo.PointID = pointID;
            DataToDbAddRequest<Jc_Ll_MInfo> addrequest = new DataToDbAddRequest<Jc_Ll_MInfo>()
            {
                Item = accumulationMInfo
            };
            accumulationMonthDataToDbService.AddItem(addrequest);
        }

        /// <summary>
        /// 添加累积量年数据
        /// </summary>
        /// <param name="channel"></param>
        private void CreateAccumulationYInfo(Jc_Ll_YInfo accumulationYInfo, int channel, string realdata)
        {
            switch (channel)
            {
                case 13:
                    accumulationYInfo.BHL = decimal.Parse(realdata);
                    break;
                case 14:
                    accumulationYInfo.BCL = decimal.Parse(realdata);
                    break;
                case 15:
                    accumulationYInfo.GHL = decimal.Parse(realdata);
                    break;
                case 16:
                    accumulationYInfo.GCL = decimal.Parse(realdata);
                    break;
            }
        }

        private void InsertAccumulationYInfo(Jc_Ll_YInfo accumulationYInfo)
        {
            DateTime timer = Convert.ToDateTime(_creatTime.ToString("yyyy-01-01 00:00:00"));
            AccumulationYearExistsRequest request = new AccumulationYearExistsRequest
            {
                Timer = timer,
                PointId = pointID
            };
            var yearresponse = accumulationYearService.ExistsAccumulationYearInfo(request);
            if (yearresponse != null && yearresponse.IsSuccess && yearresponse.Data != null)
            {
                accumulationYInfo.ID = yearresponse.Data.ID;
                accumulationYInfo.InfoState = InfoState.Modified;
            }
            else
            {
                accumulationYInfo = new Jc_Ll_YInfo();
                accumulationYInfo.InfoState = InfoState.AddNew;
                accumulationYInfo.ID = IdHelper.CreateLongId().ToString();
                LogHelper.Info(pointID + " Y " + timer.ToString("yyy-MM-dd HH:mm:ss") + "未找到！");
            }
            accumulationYInfo.Timer = timer;
            accumulationYInfo.PointID = pointID;
            DataToDbAddRequest<Jc_Ll_YInfo> addrequest = new DataToDbAddRequest<Jc_Ll_YInfo>()
            {
                Item = accumulationYInfo
            };
            accumulationYearDataToDbService.AddItem(addrequest);
        }
    }
}
