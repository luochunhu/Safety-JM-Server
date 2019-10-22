using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Logging;
using Basic.Framework.Web;
using Sys.Safety.Cache.Safety;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Control;

namespace Sys.Safety.Services.Control
{
    public class ControlService: IControlService
    {
        private readonly IPointDefineService _pointDefineService;
        private readonly INetworkModuleService _networkModuleService;

        public ControlService(IPointDefineService pointDefineService, INetworkModuleService networkModuleService)
        {
            _pointDefineService = pointDefineService;
            _networkModuleService = networkModuleService;
        }

        public BasicResponse<DataTable> GetDyxFz()
        {
            DataTable msg = new DataTable();
            List<string> keys = null;
            Jc_DefInfo def = null;
            msg.Columns.Add("fzh", typeof(int));
            msg.Columns.Add("fd", typeof(int));
            msg.TableName = "fz";
            try
            {
                //List<Jc_DefInfo> _JC_DEF = _CacheMrg.ServerCache.Cache_jc_def.QueryAllCacheList();
                var res = _pointDefineService.GetAllPointDefineCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                List<Jc_DefInfo> _JC_DEF = res.Data;

                if (_JC_DEF.Count > 0)
                {
                    //keys = new List<string>(_CacheMrg.ServerCache.Cache_jc_def._JC_DEF.Keys);
                    for (int i = 0; i < _JC_DEF.Count; i++)
                    {
                        def = _JC_DEF[i];
                        if (def.Activity == "1" && def.DevPropertyID == 0)
                        {
                            if ((def.Bz3 & 0x8) == 0x8)
                            {
                                msg.Rows.Add(def.Fzh, 0);
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
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取所有绑定电源箱分站", ex);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = msg
            };
            return ret;
        }

        public BasicResponse<DataTable> GetDyxMac()
        {
            DataTable msg = new DataTable();
            List<string> keys = null;
            Jc_MacInfo def = null;
            msg.Columns.Add("mac", typeof(string));
            msg.Columns.Add("wz", typeof(string));
            msg.Columns.Add("fd", typeof(int));
            msg.TableName = "fz";
            try
            {
                //List<Jc_MacInfo> JC_MAC = _CacheMrg.ServerCache.Cache_jc_mac.QueryAllCache();
                var res = _networkModuleService.GetAllNetworkModuleCache();
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                List<Jc_MacInfo> JC_MAC = res.Data;

                if (JC_MAC.Count > 0)
                {
                    for (int i = 0; i < JC_MAC.Count; i++)
                    {
                        def = JC_MAC[i];
                        if (def.Type == 0 && !string.IsNullOrEmpty(def.Wz) && def.Bz4 == "1")
                        {
                            msg.Rows.Add(def.MAC, def.Wz, 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取所有绑定电源箱分站", ex);
            }
            var ret = new BasicResponse<DataTable>
            {
                Data = msg
            };
            return ret;
        }
    }
}
