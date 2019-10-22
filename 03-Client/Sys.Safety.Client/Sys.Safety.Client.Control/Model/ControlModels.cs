using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraGrid.Columns;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.Client.Control.Model
{
    public class DEFServiceModel
    {
        private static readonly IPointDefineService PointDefineService = ServiceFactory.Create<IPointDefineService>();

        /// <summary>
        ///     根据测点编号查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByCodeCache(string PointCode)
        {
            //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
            //return DEFService.QueryPointByCodeCache(PointCode);

            var req = new PointDefineGetByPointRequest
            {
                Point = PointCode
            };
            var res = PointDefineService.GetPointDefineCacheByPoint(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        ///     通过设备性质查找测点
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static IList<Jc_DefInfo> QueryPointByDevpropertIDCache(int DevPropertID)
        {
            //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
            //return DEFService.QueryPointByDevpropertIDCache(DevPropertID);

            //var pointDefineCacheRequest = new PointDefineCacheGetByConditonRequest
            //{
            //    Predicate =
            //        a => (a.DevPropertyID == DevPropertID) && (a.Activity == "1") && (a.InfoState != InfoState.Delete)
            //};
            //var res = PointDefineService.GetPointDefineCacheByDynamicCondition(pointDefineCacheRequest);
            var req = new PointDefineGetByDevpropertIDRequest
            {
                DevpropertID = DevPropertID
            };
            var res = PointDefineService.GetPointDefineCacheByDevpropertID(req);

            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        ///     通过分站号、设备性质 查找设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static IList<Jc_DefInfo> QueryPointByInfs(int fzh, int DevPropertID)
        {
            //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
            //return DEFService.QueryPointByInfs(fzh, DevPropertID);
            //var req = new PointDefineCacheGetByConditonRequest
            //{
            //    Predicate =
            //        a =>
            //            (fzh == a.Fzh) && (DevPropertID == a.DevPropertyID) && (a.Activity == "1") &&
            //            (a.InfoState != InfoState.Delete)
            //};
            //var res = PointDefineService.GetPointDefineCacheByDynamicCondition(req);
            var req = new PointDefineGetByStationIDDevPropertIDRequest
            {
                StationID = fzh,
                DevPropertID = DevPropertID
            };
            var res = PointDefineService.GetPointDefineCacheByStationIDDevPropertID(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        /// 根据分站号查询其下属测点
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static IList<Jc_DefInfo> QueryPointByFzh(string fzh)
        {
            var req = new PointDefineGetByStationIDRequest
            {
                StationID = Convert.ToInt32(fzh)
            };
            var res = PointDefineService.GetPointDefineCacheByStationID(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }
    }

    public class JCSDKZServiceModel
    {
        private static readonly IManualCrossControlService ManualCrossControlCacheService =
            ServiceFactory.Create<IManualCrossControlService>();

        /// <summary>
        ///     清除手动控制 2017.3.22 by
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool DoDoJCSDKZ(List<Jc_JcsdkzInfo> items)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.DelJCSKZs(items);
            var req = new ManualCrossControlsRequest
            {
                ManualCrossControlInfos = items
            };
            var res = ManualCrossControlCacheService.DeleteManualCrossControls(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.IsSuccess;
        }

        /// <summary>
        ///     批量添加通讯配置缓存对象 包括更新
        /// </summary>
        /// <param name="items"></param>
        public static bool AddJC_JCSDKZsCache(IList<Jc_JcsdkzInfo> items)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.AddJC_JCSDKZsCache(items);
            var req = new ManualCrossControlsRequest
            {
                ManualCrossControlInfos = items.ToList()
            };
            var res = ManualCrossControlCacheService.AddManualCrossControls(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.IsSuccess;
        }

        /// <summary>
        ///     查询所有手动控制
        /// </summary>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZsCache()
        {
            // 20170626
            //var res = ManualCrossControlCacheService.GetAllManualCrossControl();
            var res = ManualCrossControlCacheService.GetAllManualCrossControlDetail();
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        ///     通过分站号查询手动控制
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZbyFzhOnlyHCtrlCache(int fzh)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.QueryJCSDKZbyFzhOnlyHCtrlCache(fzh);
            //var req = new ManualCrossControlCacheGetByConditionRequest
            //{
            //    Predicate =
            //        a =>
            //            (a.Type == 0) && a.Bkpoint.Contains(fzh.ToString().PadLeft(3, '0')) &&
            //            (a.InfoState != InfoState.Delete)
            //};
            //var res = ManualCrossControlCacheService.GetManualCrossControlByDynamicCondition(req);
            var req = new ManualCrossControlGetByStationIDRequest
            {
                StationID = fzh
            };
            var res = ManualCrossControlCacheService.GetManualCrossControlByStationID(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        ///     通过分站号查询手动控制
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZbyInf(int Type, string ZkPoint, string BkPoint)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.QueryJCSDKZbyInf( Type,  ZkPoint,  BkPoint) ;
            //var req = new ManualCrossControlCacheGetByConditionRequest
            //{
            //    Predicate =
            //        a =>
            //            (a.Bkpoint == BkPoint) && (a.Type == Type) && (a.ZkPoint == ZkPoint) &&
            //            (a.InfoState != InfoState.Delete)
            //};
            //var res = ManualCrossControlCacheService.GetManualCrossControlByDynamicCondition(req);
            var req = new ManualCrossControlGetByTypeZkPointBkPointRequest
            {
                Type = Type,
                ZkPoint = ZkPoint,
                BkPoint = BkPoint
            };
            var res = ManualCrossControlCacheService.GetManualCrossControlByTypeZkPointBkPoint(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }

        /// <summary>
        ///     通过分站号查询手动控制
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static IList<Jc_JcsdkzInfo> QueryJCSDKZbyInf(int Type, string BkPoint)
        {
            //IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
            //return JCSDKZService.QueryJCSDKZbyInf(Type, BkPoint);
            //var req = new ManualCrossControlCacheGetByConditionRequest
            //{
            //    Predicate =
            //        a => (a.Bkpoint == BkPoint) && (a.Type == Type) && (a.InfoState != InfoState.Delete)
            //};
            //var res = ManualCrossControlCacheService.GetManualCrossControlByDynamicCondition(req);
            var req = new ManualCrossControlGetByTypeBkPointRequest
            {
                Type = Type,
                BkPoint = BkPoint
            };
            var res = ManualCrossControlCacheService.GetManualCrossControlByTypeBkPoint(req);
            if (!res.IsSuccess)
                throw new Exception(res.Message);
            return res.Data;
        }
    }

    /// <summary>
    ///     GRID 栏目管理
    /// </summary>
    public class GridClumnsMrg
    {
        /// <summary>
        ///     获取分站类型 GRID 栏目
        /// </summary>
        /// <returns></returns>
        public static List<GridColumn> ControlGridColumn()
        {
            var ret = new List<GridColumn>();
            GridColumn col;
            try
            {
                col = new GridColumn();
                col.FieldName = "Bkpoint";
                col.Caption = "被控测点";
                col.VisibleIndex = 1;
                col.Width = 100;
                ret.Add(col);

                col = new GridColumn();
                col.FieldName = "Zkpoint";
                col.Caption = "主控测点";
                col.VisibleIndex = 2;
                col.Width = 100;
                ret.Add(col);

                col = new GridColumn();
                col.FieldName = "Type";
                col.Caption = "控制类型";
                col.VisibleIndex = 3;
                col.Width = 100;
                ret.Add(col);

                for (var i = 0; i < ret.Count; i++)
                {
                    ret[i].OptionsColumn.ReadOnly = true;
                    ret[i].OptionsColumn.AllowFocus = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }
    }

    /// <summary>
    ///     Grid数据源
    /// </summary>
    public class GridSource
    {
        /// <summary>
        ///     GRID分站数据源
        /// </summary>
        public static List<ControlInfItem> GridControlSource = new List<ControlInfItem>();
    }

    /// <summary>
    ///     分站对象
    /// </summary>
    [Serializable]
    public class ControlInfItem
    {
        private string _Type;

        /// <summary>
        ///     被控测点
        /// </summary>
        public string Bkpoint { get; set; }

        /// <summary>
        ///     主控测点
        /// </summary>
        public string Zkpoint { get; set; }

        /// <summary>
        ///     控制类型
        /// </summary>
        public string Type { 
            get; set;
            //get { return _Type; }
            //set
            //{
            //    if (string.IsNullOrEmpty(value))
            //    {
            //        _Type = value;
            //        return;
            //    }
            //    if (value == "0")
            //        _Type = "手动控制";
            //    else if (value == "10")
            //        _Type = "手动放电";
            //    else
            //        _Type = "交叉控制";
            //}
        }

        /// <summary>
        ///     列状态，1新增；2删除
        /// </summary>
        public string RowStatus { get; set; }
    }
}