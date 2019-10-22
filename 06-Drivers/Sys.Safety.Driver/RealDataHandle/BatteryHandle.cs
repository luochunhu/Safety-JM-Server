using Basic.Framework.Logging;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver.RealDataHandle
{
    /// <summary>
    /// 电源箱数据处理
    /// </summary>
    public class BatteryHandle 
    {
        public void DataHandle(Jc_DefInfo station, List<RealDataItem> dataItems)
        {
            try
            {
                int index = 0;
                bool changeFlag = false;
                BatteryItem batteryItem;
                List<BatteryItem> batteryItems = station.BatteryItems;
                foreach (RealDataItem item in dataItems)
                {
                    index = batteryItems.FindIndex(a => a.Channel == item.Channel && a.BatteryAddress == item.Address);
                    if (index >= 0)
                    {
                        if (batteryItems[index].DeviceOnlyCode != item.SoleCoding)
                        {
                            changeFlag = true;
                            batteryItems[index].DeviceOnlyCode = item.SoleCoding;
                        }
                    }
                    else
                    {
                        changeFlag = true;
                        batteryItem = new BatteryItem();
                        batteryItem.Channel = item.Channel;
                        batteryItem.BatteryAddress = item.Address;
                        batteryItem.DeviceOnlyCode = item.SoleCoding;
                        batteryItems.Add(batteryItem);
                    }
                }

                if (changeFlag)
                {
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("BatteryItems", batteryItems);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("BatteryHandle DataHandle Error【" + station.Point + "】" + ex.Message);
            }
        }
    }
}
