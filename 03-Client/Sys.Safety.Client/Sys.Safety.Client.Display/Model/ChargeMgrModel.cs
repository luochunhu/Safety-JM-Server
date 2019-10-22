//TODO:与其它业务模块有相应的交互，其它模块目前没有实现


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Basic.Framework.Core.Aop;
//using System.Data;
//using Basic.DTO;
//using Sys.Safety.IServer.PointMrg;

//namespace Sys.Safety.Client.Display.Model
//{
//    /// <summary>
//    /// 电源管理【测点定义及控制相关接口】
//    /// </summary>
//    public class ChargeMrg
//    {
//        /// <summary>获取电源箱状态 txy 20170330
//        /// </summary>
//        /// <returns></returns>
//        public static void  sendD(int m,string fzhormac)
//        {
//            IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
//            DEFService.sendD( m, fzhormac);
//        }
//        /// <summary> 添加DEF 缓存
//        /// </summary>
//        /// <returns></returns>
//        public static bool AddDEFCache(JCDEFDTO item)
//        {
//            IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
//            return DEFService.AddDEFCache(item);
//        }

//        /// <summary> 添加mac 缓存
//        /// </summary>
//        /// <returns></returns>
//        public static bool AddMacCache(JCMACDTO  item)
//        {
//            IJC_MACService DEFService = ServiceFactory.CreateService<IJC_MACService>();
//            return DEFService.AddMACCache(item);
//        }

//        /// <summary>根据测点编号查询测点
//        /// </summary>
//        /// <param name="pointCode"></param>
//        /// <returns></returns>
//        public static JCDEFDTO QueryPointByCodeCache(string PointCode)
//        {
//            IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
//            return DEFService.QueryPointByCodeCache(PointCode);
//        }
        

//        public static JCMACDTO  QueryJcMac(string PointCode)
//        {
//            IJC_MACService DEFService = ServiceFactory.CreateService<IJC_MACService>();
//            return DEFService.QueryMACByCode(PointCode);
//        }

//        /// <summary>
//        ///  保存定义数据数据
//        /// </summary>
//        public static bool SaveDataDef()
//        {
//            IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
//            return DEFService.SaveData();
//        }

//        /// <summary>
//        ///  保存定义数据数据
//        /// </summary>
//        public static bool SaveDataMac()
//        {
//            IJC_MACService DEFService = ServiceFactory.CreateService<IJC_MACService>();
//            return DEFService.SaveData();
//        }

//        /// <summary>通过设备编号查询设备
//        /// </summary>
//        /// <param name="pointCode"></param>
//        /// <returns></returns>
//        public static JCDEVDTO QueryDevByDevIDCache(long DevID)
//        {
//            IJC_DEVService DEVService = ServiceFactory.CreateService<IJC_DEVService>();
//            return DEVService.QueryDevByDevIDCache(DevID);
//        }

//        /// <summary>
//        /// 通过被控点和控制类型查询放电信息   //此方法在实时模块也在调用，目前框架不支持多线程同时调用同一服务，修改为不同的接口  20170425
//        /// </summary>
//        /// <param name="wz"></param>
//        /// <returns></returns>
//        public static IList<JCJCSDKZDTO> QueryJCSDKZbyInf(int Type, string BkPoint)
//        {
//            IJC_JCSDKZServiceInThread JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZServiceInThread>();
//            return JCSDKZService.QueryJCSDKZbyInf(Type, BkPoint);
//        }

//        /// <summary>
//        /// 添加控制配置缓存对象 包括更新
//        /// </summary>
//        /// <param name="item"></param>
//        /// 
//        public static bool AddJC_JCSDKZCache(JCJCSDKZDTO item)
//        {
//            IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
//            return JCSDKZService.AddJC_JCSDKZCache(item);
//        }
//        /// <summary>
//        /// 删除控制配置缓存对象
//        /// </summary>
//        /// <param name="item"></param>
//        /// 
//        public static bool DelJC_JCSDKZCache(long ID)
//        {
//            IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
//            return JCSDKZService.DoJCSDKZ(ID);
//        }
//        /// <summary>
//        /// 批量删除控制配置缓存
//        /// </summary>
//        /// <param name="ID"></param>
//        /// <returns></returns>
//        public static bool DelJC_JCSDKZCache(List<JCJCSDKZDTO> items)
//        {
//            IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
//            return JCSDKZService.DelJCSKZs(items);
//        }

//        /// <summary>
//        /// 数据入库并执行控制
//        /// </summary>
//        /// <returns></returns>
//        public static bool SaveDataJCSDKZ()
//        {
//            IJC_JCSDKZService JCSDKZService = ServiceFactory.CreateService<IJC_JCSDKZService>();
//            return JCSDKZService.SaveData();
//        }
//    }
//}
