using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Management;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.Cache;
using Basic.Framework.Web;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.DeviceDefine;
using Sys.Safety.Request.Position;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Request.Enumcode;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Enums;

namespace Sys.Safety.Client.Define.Model
{
    public class CONFIGServiceModel
    {
        static IConfigService _ConfigService = ServiceFactory.Create<IConfigService>();
        static ISettingService _SettingService = ServiceFactory.Create<ISettingService>();
        static List<EnumcodeInfo> personPointTypeEnum = new List<EnumcodeInfo>();

        /// <summary>
        ///  保存巡检
        /// </summary>
        public static void SaveRouting()
        {
            _ConfigService.SaveInspection();
        }

        /// <summary>
        ///  20170104
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SettingInfo GetConfigFKey(string key)
        {
            SettingInfo res = null;
            List<SettingInfo> sets = _SettingService.GetSettingList().Data;
            if (sets != null)
            {
                for (int i = 0; i < sets.Count; i++)
                {
                    if (sets[i].StrKey == key)
                    {
                        res = sets[i];
                    }
                }
            }
            return res;
        }

        #region  20160111

        /// <summary>
        /// 新增or删除测点写日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelPointLog(Jc_DefInfo jcdef)
        {
            string log = "";
            try
            {
                if (jcdef.DevPropertyID == 0)
                {
                    log = AddOrDelFZLogs(jcdef);
                }
                else if (jcdef.DevPropertyID == 1)
                {
                    log = AddOrDelMnlLogs(jcdef);
                }
                else if (jcdef.DevPropertyID == 2)
                {
                    log = AddOrDelKglLogs(jcdef);
                }
                else if (jcdef.DevPropertyID == 3)
                {
                    log = AddOrDelKzlLogs(jcdef);
                }
                else if (jcdef.DevPropertyID == 7)
                {
                    log = AddOrDelSBQLogs(jcdef);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("写操作日志", ex);
            }
            return log;
        }

        /// <summary>
        /// 修改测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <param name="newdef"></param>
        /// <returns></returns>
        public static string UpdatePointLog(Jc_DefInfo jcdef, Jc_DefInfo newdef)
        {
            string log = "";
            try
            {
                if (jcdef.DevPropertyID == 0)
                {
                    log = UpdateFZLogs(jcdef, newdef);
                }
                else if (jcdef.DevPropertyID == 1)
                {
                    log = UpdateMnlLogs(jcdef, newdef);
                }
                else if (jcdef.DevPropertyID == 2)
                {
                    log = UpdateKglLogs(jcdef, newdef);
                }
                else if (jcdef.DevPropertyID == 3)
                {
                    log = UpdateKzlLogs(jcdef, newdef);
                }
                else if (jcdef.DevPropertyID == 7)
                {
                    log = UpdateSBQLogs(jcdef, newdef);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("UpdatePointLog-" + ex);
            }
            return log;
        }

        /// <summary>
        /// 新增或删除分站测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelFZLogs(Jc_DefInfo jcdef)
        {

            string log = "";
            if (jcdef.Activity == "0")
            {
                log = string.Format(@"删除分站【{16}】,删除时间【{15}】,测点号【{0}】,测点ID【{1}】,设备类型【{15}】,型号【{2}:{3}】,位置【{4}】,MAC【{5}】,IP【{6}】,抽放绑定开停【{7}】,
            大气压【{8}】,正负压【{9}】,串口号【{10}】,风电闭锁【{11}】,逻辑控制【{12}】,故障闭锁【{13}】,运行状态【{14}】",
              jcdef.Point, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz), ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3),
              jcdef.K1, jcdef.K2, jcdef.K3, (jcdef.Bz3 & 0x01) == 0x01 ? "是" : "否", (jcdef.Bz3 & 0x02) == 0x02 ? "是" : "否"
              , (jcdef.Bz3 & 0x04) == 0x04 ? "是" : "否", getzt(jcdef.Bz4), jcdef.DeleteTime, jcdef.Fzh, jcdef.DevName);
            }
            else
            {
                log = string.Format(@"新增分站【{16}】,新增时间【{15}】,测点号【{0}】,测点ID【{1}】,设备类型【{15}】,型号【{2}:{3}】,位置【{4}】,MAC【{5}】,IP【{6}】,抽放绑定开停【{7}】,
            大气压【{8}】,正负压【{9}】,串口号【{10}】,风电闭锁【{11}】,逻辑控制【{12}】,故障闭锁【{13}】,运行状态【{14}】",
                jcdef.Point, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz), ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3),
                jcdef.K1, jcdef.K2, jcdef.K3, (jcdef.Bz3 & 0x01) == 0x01 ? "是" : "否", (jcdef.Bz3 & 0x02) == 0x02 ? "是" : "否"
                , (jcdef.Bz3 & 0x04) == 0x04 ? "是" : "否", getzt(jcdef.Bz4), jcdef.CreateUpdateTime, jcdef.Fzh, jcdef.DevName);
            }
            return log;
        }

        private static string getzt(int zt)
        {
            string msg = "";
            if ((byte)(zt & 0x02) == 0x02)
            {
                msg = "休眠";
            }
            else if ((byte)(zt & 0x04) == 0x04)
            {
                msg = "检修";
            }
            else if ((byte)(zt & 0x08) == 0x08)
            {
                msg = "标校";
            }
            else
            {
                msg = "运行";
            }
            return msg;
        }


        private static string GetControlStr(int _SourceNum, int controlInt)
        {
            //return Convert.ToString(controlInt, 2).PadLeft(24, '0');
            return SetLocalControlText(_SourceNum, controlInt);
        }
        private static string SetLocalControlText(int _SourceNum, int K)
        {
            string temp = "";
            if (K > 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    if (((K >> i) & 0x1) == 0x1)
                    {
                        if (i < 8)
                        {
                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0') + "0" + ",";
                        }
                        else
                        {
                            temp += _SourceNum.ToString().PadLeft(3, '0') + "C" + (i - 7).ToString().PadLeft(2, '0') + "1" + ",";
                        }
                    }
                }
                if (temp.Contains(","))
                {
                    temp = temp.Substring(0, temp.Length - 1);
                }
            }
            return temp;
        }
        /// <summary>
        /// 修改分站测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string UpdateFZLogs(Jc_DefInfo olddef, Jc_DefInfo newdef)
        {
            string log = "";
            //            log = string.Format(@"修改分站【{31}】,修改时间【{30}】,point【{0}】改为【{1}】,pointid【{2}】改为【{3}】,型号【{4}:{5}】改为【{6}:{7}】,位置【{8}】改为【{9}】,
            //MAC【{10}】改为【{11}】,IP【{12}】改为【{13}】,抽放绑定开停【{14}】改为【{15}】,
            //            大气压【{16}】改为【{17}】,正负压【{18}】改为【{19}】,串口号【{20}】改为【{21}】,风电闭锁【{22}】改为【{23}】,
            //逻辑控制【{24}】改为【{25}】,故障闭锁【{26}】改为【{27}】,运行状态【{28}】改为【{29}】",
            //                olddef.Point, newdef.Point, olddef.PointID, newdef.PointID, olddef.DevModelID, olddef.DevModel, newdef.DevModelID, newdef.DevModel,
            //                ContString(olddef.Wz), ContString(newdef.Wz), ContString(olddef.Jckz1), ContString(newdef.Jckz1), ContString(olddef.Jckz2), ContString(newdef.Jckz2),
            //                ContString(olddef.Jckz3), ContString(newdef.Jckz3),
            //                olddef.K1, newdef.K1, olddef.K2, newdef.K2, olddef.K3, newdef.K3, (olddef.Bz3 & 0x01) == 0x01 ? "是" : "否", (newdef.Bz3 & 0x01) == 0x01 ? "是" : "否",
            //                (olddef.Bz3 & 0x02) == 0x02 ? "是" : "否", (newdef.Bz3 & 0x02) == 0x02 ? "是" : "否"
            //                , (olddef.Bz3 & 0x04) == 0x04 ? "是" : "否", (newdef.Bz3 & 0x04) == 0x04 ? "是" : "否", getzt(olddef.Bz4), getzt(newdef.Bz4), newdef.CreateUpdateTime.ToString(), newdef.Fzh);

            log = string.Format(@"修改分站【{0}】,修改时间【{1}】,", newdef.Fzh, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (olddef.Point != newdef.Point)
            {
                log += string.Format("测点点【{0}】改为【{1}】,", olddef.Point, newdef.Point);
            }
            if (olddef.DevName != newdef.DevName)
            {
                log += string.Format("设备类型【{0}】改为【{1}】,", olddef.DevName, newdef.DevName);
            }
            if (olddef.DevModelID != newdef.DevModelID)
            {
                log += string.Format("型号【{0}:{1}】改为【{2}:{3}】,", olddef.DevModelID, olddef.DevModel, newdef.DevModelID, newdef.DevModel);
            }
            if (olddef.Wz != newdef.Wz)
            {
                log += string.Format("位置【{0}】改为【{1}】,", olddef.Wz, newdef.Wz);
            }
            if (olddef.Jckz1 != newdef.Jckz1)
            {
                log += string.Format("MAC【{0}】改为【{1}】,", ContString(olddef.Jckz1), ContString(newdef.Jckz1));
            }
            if (olddef.Jckz2 != newdef.Jckz2)
            {
                log += string.Format("IP【{0}】改为【{1}】,", ContString(olddef.Jckz2), ContString(newdef.Jckz2));
            }
            if (olddef.Jckz3 != newdef.Jckz3)
            {
                log += string.Format("抽放绑定开停【{0}】改为【{1}】,", ContString(olddef.Jckz3), ContString(newdef.Jckz3));
            }
            if (olddef.K1 != newdef.K1)
            {
                log += string.Format("大气压【{0}】改为【{1}】,", olddef.K1, newdef.K1);
            }
            if (olddef.K2 != newdef.K2)
            {
                log += string.Format("正负压【{0}】改为【{1}】,", olddef.K2, newdef.K2);
            }
            if (olddef.K3 != newdef.K3)
            {
                log += string.Format("串口号【{0}】改为【{1}】,", olddef.K3, newdef.K3);
            }
            if ((olddef.Bz3 & 0x01) != (newdef.Bz3 & 0x01))
            {
                log += string.Format("风电闭锁【{0}】改为【{1}】,", (olddef.Bz3 & 0x01) == 0x01 ? "是" : "否", (newdef.Bz3 & 0x01) == 0x01 ? "是" : "否");
            }
            if ((olddef.Bz3 & 0x02) != (newdef.Bz3 & 0x02))
            {
                log += string.Format("逻辑控制【{0}】改为【{1}】,", (olddef.Bz3 & 0x02) == 0x02 ? "是" : "否", (newdef.Bz3 & 0x02) == 0x02 ? "是" : "否");
            }
            if ((olddef.Bz3 & 0x04) != (newdef.Bz3 & 0x04))
            {
                log += string.Format("故障闭锁【{0}】改为【{1}】,", (olddef.Bz3 & 0x04) == 0x04 ? "是" : "否", (newdef.Bz3 & 0x04) == 0x04 ? "是" : "否");
            }
            if (olddef.Bz4 != newdef.Bz4)
            {
                log += string.Format("运行状态【{0}】改为【{1}】,", getzt(olddef.Bz4), getzt(newdef.Bz4));
            }
            return log;
        }

        /// <summary>
        /// 新增或删除模拟量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelMnlLogs(Jc_DefInfo jcdef)
        {
            string log = "";
            if (jcdef.Activity == "0")
            {
                //                log = string.Format(@"删除模拟量【{0}】,时间【{1}】,pointid【{2}】,型号【{3}:{4}】,位置【{5}】,断电交叉控制【{6}】,
                //                断线交叉控制【{7}】,故障交叉控制【{8}】,上预【{9}】,上报【{10}】,上断【{11}】,上复【{12}】,下预【{13}】,下报【{14}】,
                //下断【{15}】,下复【{16}】,上报控制口【{17}】,上断控制口【{18}】,下报控制口【{19}】,下断控制口【{20}】,上溢控制口【{21}】,断线控制口【{22}】,
                //运行状态【{23}】,标校周期【{24}】", jcdef.Point, jcdef.DeleteTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                //                                      ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3), jcdef.Z1, jcdef.Z2, jcdef.Z3, jcdef.Z4, jcdef.Z5, jcdef.Z6, jcdef.Z7,
                //                                      jcdef.Z8, jcdef.K1, jcdef.K2, jcdef.K3, jcdef.K4, jcdef.K5, jcdef.K6, getzt(jcdef.Bz4), ContString(jcdef.Bz14));
                log = string.Format(@"删除模拟量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{25}】,型号【{3}:{4}】,位置【{5}】,断电交叉控制【{6}】,
                断线交叉控制【{7}】,故障交叉控制【{8}】,上预【{9}】,上报【{10}】,上断【{11}】,上复【{12}】,下预【{13}】,下报【{14}】,
下断【{15}】,下复【{16}】,上报控制口【{17}】,上断控制口【{18}】,下报控制口【{19}】,下断控制口【{20}】,上溢控制口【{21}】,负漂控制口【{22}】,断线控制口【{23}】,
运行状态【{24}】", jcdef.Point, jcdef.DeleteTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                                     ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3), jcdef.Z1, jcdef.Z2, jcdef.Z3, jcdef.Z4, jcdef.Z5, jcdef.Z6, jcdef.Z7,
                                     jcdef.Z8,
                                     GetControlStr(jcdef.Fzh, jcdef.K1),
                                     GetControlStr(jcdef.Fzh, jcdef.K2),
                                     GetControlStr(jcdef.Fzh, jcdef.K3),
                                     GetControlStr(jcdef.Fzh, jcdef.K4),
                                     GetControlStr(jcdef.Fzh, jcdef.K5),
                                     GetControlStr(jcdef.Fzh, jcdef.K6),
                                      GetControlStr(jcdef.Fzh, jcdef.K7),
                                     getzt(jcdef.Bz4), jcdef.DevName);
            }
            else
            {
                //                log = string.Format(@"新增模拟量【{0}】,时间【{1}】,pointid【{2}】, 型号【{3}:{4}】,位置【{5}】,断电交叉控制【{6}】, 断线交叉控制【{7}】,故障交叉控制【{8}】,上预【{9}】,上报【{10}】,上断【{11}】,上复【{12}】,下预【{13}】,下报【{14}】,
                //                    下断【{15}】,下复【{16}】,上报控制口【{17}】,上断控制口【{18}】,下报控制口【{19}】,下断控制口【{20}】,上溢控制口【{21}】,断线控制口【{22}】,
                //                    运行状态【{23}】,标校周期【{24}】",
                //                    jcdef.Point, jcdef.CreateUpdateTime.ToString(), jcdef.PointID.ToString(), jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                //                      ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3), jcdef.Z1, jcdef.Z2, jcdef.Z3, jcdef.Z4, jcdef.Z5, jcdef.Z6, jcdef.Z7,
                //                      jcdef.Z8, jcdef.K1, jcdef.K2, jcdef.K3, jcdef.K4, jcdef.K5, jcdef.K6, getzt(jcdef.Bz4), ContString(jcdef.Bz14));
                log = string.Format(@"新增模拟量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{25}】, 型号【{3}:{4}】,位置【{5}】,断电交叉控制【{6}】, 断线交叉控制【{7}】,故障交叉控制【{8}】,上预【{9}】,上报【{10}】,上断【{11}】,上复【{12}】,下预【{13}】,下报【{14}】,
                    下断【{15}】,下复【{16}】,上报控制口【{17}】,上断控制口【{18}】,下报控制口【{19}】,下断控制口【{20}】,上溢控制口【{21}】,负漂控制口【{22}】,断线控制口【{23}】,
                    运行状态【{24}】",
                    jcdef.Point, jcdef.CreateUpdateTime.ToString(), jcdef.PointID.ToString(), jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                      ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3), jcdef.Z1, jcdef.Z2, jcdef.Z3, jcdef.Z4, jcdef.Z5, jcdef.Z6, jcdef.Z7,
                      jcdef.Z8,
                      GetControlStr(jcdef.Fzh, jcdef.K1),
                      GetControlStr(jcdef.Fzh, jcdef.K2),
                      GetControlStr(jcdef.Fzh, jcdef.K3),
                      GetControlStr(jcdef.Fzh, jcdef.K4),
                      GetControlStr(jcdef.Fzh, jcdef.K5),
                      GetControlStr(jcdef.Fzh, jcdef.K6),
                       GetControlStr(jcdef.Fzh, jcdef.K7),
                      getzt(jcdef.Bz4), jcdef.DevName);
            }
            return log;
        }

        /// <summary>
        /// 新增或删除识别器测点日志  20171124
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelSBQLogs(Jc_DefInfo jcdef)
        {
            string log = "";
            if (jcdef.Activity == "0")
            {
                log = string.Format(@"删除识别器【{0}】,时间【{1}】,pointid【{2}】,设备类型【{12}】,型号【{3}:{4}】,位置【{5}】,识别器类型【{6}】,
                报警时间【{7}】,离开时间【{8}】,限制进入人员【{9}】,禁止进入人员【{10}】,超员报警人数【{11}】,运行状态【{12}】", jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                                   GetPerPointType(jcdef.Bz1), (jcdef.K4 / 60) + ":" + (jcdef.K4 % 60), (jcdef.K5 / 60) + ":" + (jcdef.K5 % 60),
                                   GetRestrictedpersonInfo(0, jcdef.RestrictedpersonInfoList), GetRestrictedpersonInfo(1, jcdef.RestrictedpersonInfoList),
                                   jcdef.K3,
                                   getzt(jcdef.Bz4), jcdef.DevName);
            }
            else
            {
                log = string.Format(@"新增识别器【{0}】,时间【{1}】,pointid【{2}】,设备类型【{12}】,型号【{3}:{4}】,位置【{5}】,识别器类型【{6}】,
                报警时间【{7}】,离开时间【{8}】,限制进入人员【{9}】,禁止进入人员【{10}】,超员报警人数【{11}】,运行状态【{12}】", jcdef.Point, jcdef.DeleteTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, jcdef.Wz,
                                    GetPerPointType(jcdef.Bz1), (jcdef.K4 / 60) + ":" + (jcdef.K4 % 60), (jcdef.K5 / 60) + ":" + (jcdef.K5 % 60),
                                    GetRestrictedpersonInfo(0, jcdef.RestrictedpersonInfoList), GetRestrictedpersonInfo(1, jcdef.RestrictedpersonInfoList),
                                    jcdef.K3,
                                    getzt(jcdef.Bz4), jcdef.DevName);
            }
            return log;
        }
        /// <summary>
        /// 获取识别器类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetPerPointType(int type)
        {
            string res = "";
            if (personPointTypeEnum.Count < 1)
            {
                personPointTypeEnum = EnumService.GetEnum(22);
            }
            var result = personPointTypeEnum.Find(a => a.LngEnumValue == type);
            if (result != null)
            {
                res = result.StrEnumDisplay;
            }
            return res;
        }
        /// <summary>
        /// 根据类型获取限制进入，禁止进入人员
        /// </summary>
        /// <param name="type"></param>
        /// <param name="RestrictedpersonInfoList"></param>
        /// <returns></returns>
        private static string GetRestrictedpersonInfo(int type, List<R_RestrictedpersonInfo> RestrictedpersonInfoList)
        {
            string res = "";
            if (RestrictedpersonInfoList.Count > 0)
            {
                List<R_PersoninfInfo> _allPerson = PersonInfoHandle.GetAllRPersoninfCache();
                foreach (R_RestrictedpersonInfo temp in RestrictedpersonInfoList)
                {
                    if (temp.Type == type)
                    {
                        R_PersoninfInfo tempPerson = _allPerson.Find(a => a.Yid == temp.Yid);
                        if (tempPerson != null)
                        {
                            res += tempPerson.Name + ",";
                        }
                    }
                }
            }
            return res;
        }
        private static string ContString(string msg)
        {
            string res = "无";
            if (!string.IsNullOrEmpty(msg))
            {
                res = msg.ToString();
            }
            return res;
        }


        /// <summary>
        /// 修改模拟量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string UpdateMnlLogs(Jc_DefInfo jcdef, Jc_DefInfo newdef)
        {
            string log = "";
            //            log = string.Format(@"修改模拟量【{0}】,时间【{1}】,pointid【{2}】改为【{3}】,型号【{4}:{5}】改为【{6}:{7}】,位置【{8}】改为【{9}】,
            //断电交叉控制【{10}】改为【{11}】,断线交叉控制【{12}】改为【{13}】,故障交叉控制【{14}】改为【{15}】,上预【{16}】改为【{17}】,上报【{18}】改为【{19}】,
            //上断【{20}】改为【{21}】,上复【{22}】改为【{23}】,下预【{24}】改为【{25}】,下报【{26}】改为【{27}】,
            //下断【{28}】改为【{29}】,下复【{30}】改为【{31}】,上报控制口【{32}】改为【{33}】,上断控制口【{34}】改为【{35}】,下报控制口【{36}】改为【{37}】,
            //下断控制口【{38}】改为【{39}】,上溢控制口【{40}】改为【{41}】,负漂控制口【{42}】改为【{43}】,断线控制口【{44}】改为【{45}】,
            //运行状态【{46}】改为【{47}】", jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, newdef.PointID, jcdef.DevModelID, jcdef.DevModel,
            //                                      newdef.DevModelID, newdef.DevModel, ContString(jcdef.Wz), ContString(newdef.Wz), ContString(jcdef.Jckz1), ContString(newdef.Jckz1),
            //                                      ContString(jcdef.Jckz2), ContString(newdef.Jckz2), ContString(jcdef.Jckz3),
            //                                      ContString(newdef.Jckz3), jcdef.Z1, newdef.Z1, jcdef.Z2, newdef.Z2, jcdef.Z3, newdef.Z3, jcdef.Z4, newdef.Z4, jcdef.Z5, newdef.Z5, jcdef.Z6,
            //                                      newdef.Z6, jcdef.Z7, newdef.Z7, jcdef.Z8, newdef.Z8,
            //                                       GetControlStr(jcdef.Fzh, jcdef.K1), GetControlStr(newdef.Fzh, newdef.K1),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K2), GetControlStr(newdef.Fzh, newdef.K2),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K3), GetControlStr(newdef.Fzh, newdef.K3),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K4), GetControlStr(newdef.Fzh, newdef.K4),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K5), GetControlStr(newdef.Fzh, newdef.K5),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K6), GetControlStr(newdef.Fzh, newdef.K6),
            //                                       GetControlStr(jcdef.Fzh, jcdef.K7), GetControlStr(newdef.Fzh, newdef.K7),
            //                                      getzt(jcdef.Bz4), getzt(newdef.Bz4));
            log = string.Format(@"修改模拟量【{0}】,修改时间【{1}】,", jcdef.Point, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (jcdef.Point != newdef.Point)
            {
                log += string.Format("测点号【{0}】改为【{1}】,", jcdef.Point, newdef.Point);
            }
            if (jcdef.PointID != newdef.PointID)
            {
                log += string.Format("测点ID【{0}】改为【{1}】,", jcdef.PointID, newdef.PointID);
            }
            if (jcdef.DevName != newdef.DevName)
            {
                log += string.Format("设备类型【{0}】改为【{1}】,", jcdef.DevName, newdef.DevName);
            }
            if (jcdef.DevModelID != newdef.DevModelID)
            {
                log += string.Format("型号【{0}:{1}】改为【{2}:{3}】,", jcdef.DevModelID, jcdef.DevModel, newdef.DevModelID, newdef.DevModel);
            }
            if (jcdef.Wz != newdef.Wz)
            {
                log += string.Format("位置【{0}】改为【{1}】,", ContString(jcdef.Wz), ContString(newdef.Wz));
            }
            if (jcdef.Jckz1 != newdef.Jckz1)
            {
                log += string.Format("断电交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz1), ContString(newdef.Jckz1));
            }
            if (jcdef.Jckz2 != newdef.Jckz2)
            {
                log += string.Format("断线交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz2), ContString(newdef.Jckz2));
            }
            if (jcdef.Jckz3 != newdef.Jckz3)
            {
                log += string.Format("故障交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz3), ContString(newdef.Jckz3));
            }
            if (jcdef.Z1 != newdef.Z1)
            {
                log += string.Format("上预【{0}】改为【{1}】,", jcdef.Z1, newdef.Z1);
            }
            if (jcdef.Z2 != newdef.Z2)
            {
                log += string.Format("上报【{0}】改为【{1}】,", jcdef.Z2, newdef.Z2);
            }
            if (jcdef.Z3 != newdef.Z3)
            {
                log += string.Format("上断【{0}】改为【{1}】,", jcdef.Z3, newdef.Z3);
            }
            if (jcdef.Z4 != newdef.Z4)
            {
                log += string.Format("上复【{0}】改为【{1}】,", jcdef.Z4, newdef.Z4);
            }
            if (jcdef.Z5 != newdef.Z5)
            {
                log += string.Format("下预【{0}】改为【{1}】,", jcdef.Z5, newdef.Z5);
            }
            if (jcdef.Z6 != newdef.Z6)
            {
                log += string.Format("下报【{0}】改为【{1}】,", jcdef.Z6, newdef.Z6);
            }
            if (jcdef.Z7 != newdef.Z7)
            {
                log += string.Format("下断【{0}】改为【{1}】,", jcdef.Z7, newdef.Z7);
            }
            if (jcdef.Z8 != newdef.Z8)
            {
                log += string.Format("下复【{0}】改为【{1}】,", jcdef.Z8, newdef.Z8);
            }
            if (jcdef.K1 != newdef.K1)
            {
                log += string.Format("上报控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K1), GetControlStr(newdef.Fzh, newdef.K1));
            }
            if (jcdef.K2 != newdef.K2)
            {
                log += string.Format("上断控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K2), GetControlStr(newdef.Fzh, newdef.K2));
            }
            if (jcdef.K3 != newdef.K3)
            {
                log += string.Format("下报控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K3), GetControlStr(newdef.Fzh, newdef.K3));
            }
            if (jcdef.K4 != newdef.K4)
            {
                log += string.Format("下断控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K4), GetControlStr(newdef.Fzh, newdef.K4));
            }
            if (jcdef.K5 != newdef.K5)
            {
                log += string.Format("上溢控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K5), GetControlStr(newdef.Fzh, newdef.K5));
            }
            if (jcdef.K6 != newdef.K6)
            {
                log += string.Format("负漂控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K6), GetControlStr(newdef.Fzh, newdef.K6));
            }
            if (jcdef.K7 != newdef.K7)
            {
                log += string.Format("断线控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K7), GetControlStr(newdef.Fzh, newdef.K7));
            }
            if (jcdef.Bz4 != newdef.Bz4)
            {
                log += string.Format("运行状态【{0}】改为【{1}】,", getzt(jcdef.Bz4), getzt(newdef.Bz4));
            }
            return log;
        }
        /// <summary>
        /// 更新识别器  20171124
        /// </summary>
        /// <param name="jcdef"></param>
        /// <param name="newdef"></param>
        /// <returns></returns>
        public static string UpdateSBQLogs(Jc_DefInfo jcdef, Jc_DefInfo newdef)
        {
            string log = "";
            log = string.Format(@"修改识别器【{0}】,修改时间【{1}】,", jcdef.Point, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (jcdef.Point != newdef.Point)
            {
                log += string.Format("测点号【{0}】改为【{1}】,", jcdef.Point, newdef.Point);
            }
            if (jcdef.PointID != newdef.PointID)
            {
                log += string.Format("测点ID【{0}】改为【{1}】,", jcdef.PointID, newdef.PointID);
            }
            if (jcdef.DevName != newdef.DevName)
            {
                log += string.Format("设备类型【{0}】改为【{1}】,", jcdef.DevName, newdef.DevName);
            }
            if (jcdef.DevModelID != newdef.DevModelID)
            {
                log += string.Format("型号【{0}:{1}】改为【{2}:{3}】,", jcdef.DevModelID, jcdef.DevModel, newdef.DevModelID, newdef.DevModel);
            }
            if (jcdef.Wz != newdef.Wz)
            {
                log += string.Format("位置【{0}】改为【{1}】,", ContString(jcdef.Wz), ContString(newdef.Wz));
            }
            if (jcdef.Bz1 != newdef.Bz1)
            {
                log += string.Format("识别器类型【{0}】改为【{1}】,", GetPerPointType(jcdef.Bz1), GetPerPointType(newdef.Bz1));
            }
            if (jcdef.K4 != newdef.K4)
            {
                log += string.Format("报警时间【{0}】改为【{1}】,", (jcdef.K4 / 60) + ":" + (jcdef.K4 % 60), (newdef.K4 / 60) + ":" + (newdef.K4 % 60));
            }
            if (jcdef.K5 != newdef.K5)
            {
                log += string.Format("离开时间【{0}】改为【{1}】,", (jcdef.K5 / 60) + ":" + (jcdef.K5 % 60), (newdef.K5 / 60) + ":" + (newdef.K5 % 60));
            }
            if (jcdef.RestrictedpersonInfoList != newdef.RestrictedpersonInfoList)
            {
                log += string.Format("限制进入人员【{0}】改为【{1}】,", GetRestrictedpersonInfo(0, jcdef.RestrictedpersonInfoList), GetRestrictedpersonInfo(0, newdef.RestrictedpersonInfoList));

                log += string.Format("禁止进入人员【{0}】改为【{1}】,", GetRestrictedpersonInfo(1, jcdef.RestrictedpersonInfoList), GetRestrictedpersonInfo(1, newdef.RestrictedpersonInfoList));
            }
            if (jcdef.K3 != newdef.K3)
            {
                log += string.Format("超员报警人数【{0}】改为【{1}】,", jcdef.K3, newdef.K3);
            }
            if (jcdef.Bz4 != newdef.Bz4)
            {
                log += string.Format("运行状态【{0}】改为【{1}】,", getzt(jcdef.Bz4), getzt(newdef.Bz4));
            }
            return log;
        }

        /// <summary>
        /// 新增或删除开关量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelKglLogs(Jc_DefInfo jcdef)
        {
            string log = "";
            if (jcdef.Activity == "0")
            {
                log = string.Format(@"删除开关量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{21}】,型号【{3}:{4}】,位置【{5}】,0态交叉控制【{6}】,
                1态交叉控制【{7}】,2态交叉控制【{8}】,0态控制口【{9}】,1态控制口【{10}】,2态控制口【{11}】,逻辑报警类型【{12}】,逻辑报警关联控制口【{13}】,运行状态【{14}】,
0态显示【{15}】,1态显示【{16}】,2态显示【{17}】,0态颜色【{18}】,1态颜色【{19}】,2态颜色【{20}】", jcdef.Point, jcdef.DeleteTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz),
                   ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3),
                   GetControlStr(jcdef.Fzh, jcdef.K1),
                   GetControlStr(jcdef.Fzh, jcdef.K2),
                   GetControlStr(jcdef.Fzh, jcdef.K3),
                   jcdef.K4,
                   jcdef.K5,
                   getzt(jcdef.Bz4), jcdef.Bz6, jcdef.Bz7
                    , jcdef.Bz8, jcdef.Bz9, jcdef.Bz10, jcdef.Bz11, jcdef.DevName);
            }
            else
            {
                log = string.Format(@"新增开关量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{21}】,型号【{3}:{4}】,位置【{5}】,0态交叉控制【{6}】,
                1态交叉控制【{7}】,2态交叉控制【{8}】,0态控制口【{9}】,1态控制口【{10}】,2态控制口【{11}】,逻辑报警类型【{12}】,逻辑报警关联控制口【{13}】,运行状态【{14}】,
0态显示【{15}】,1态显示【{16}】,2态显示【{17}】,0态颜色【{18}】,1态颜色【{19}】,2态颜色【{20}】", jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz),
                     ContString(jcdef.Jckz1), ContString(jcdef.Jckz2), ContString(jcdef.Jckz3),
                     GetControlStr(jcdef.Fzh, jcdef.K1),
                     GetControlStr(jcdef.Fzh, jcdef.K2),
                     GetControlStr(jcdef.Fzh, jcdef.K3),
                     jcdef.K4,
                     jcdef.K5,
                     getzt(jcdef.Bz4), ContString(jcdef.Bz6), ContString(jcdef.Bz7)
                      , ContString(jcdef.Bz8), ContString(jcdef.Bz9), ContString(jcdef.Bz10), ContString(jcdef.Bz11), jcdef.DevName);
            }
            return log;
        }

        /// <summary>
        /// 修改开关量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string UpdateKglLogs(Jc_DefInfo jcdef, Jc_DefInfo newdef)
        {
            string log = "";
            //            log = string.Format(@"修改开关量【{0}】,时间【{1}】,pointid【{2}】改为【{3}】,型号【{4}:{5}】改为【{6}:{7}】,位置【{8}】改为【{9}】,0态交叉控制【{10}】改为【{11}】,
            //                1态交叉控制【{12}】改为【{13}】,2态交叉控制【{14}】改为【{15}】,0态控制口【{16}】改为【{17}】,1态控制口【{18}】改为【{19}】,2态控制口【{20}】改为【{21}】,
            //逻辑报警类型【{22}】改为【{23}】,逻辑报警关联控制口【{24}】改为【{25}】,运行状态【{26}】改为【{27}】,
            //0态显示【{28}】改为【{29}】,1态显示【{30}】改为【{31}】,2态显示【{32}】改为【{33}】,0态颜色【{34}】改为【{35}】,1态颜色【{36}】改为【{37}】,2态颜色【{38}】改为【{39}】",
            //                    jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, newdef.PointID, jcdef.DevModelID, jcdef.DevModel, newdef.DevModelID, newdef.DevModel, ContString(jcdef.Wz),
            //                      ContString(newdef.Wz), ContString(jcdef.Jckz1), ContString(newdef.Jckz1), ContString(jcdef.Jckz2), ContString(newdef.Jckz2), ContString(jcdef.Jckz3),
            //                      ContString(newdef.Jckz3),
            //                      GetControlStr(jcdef.Fzh, jcdef.K1), GetControlStr(newdef.Fzh, newdef.K1),
            //                      GetControlStr(jcdef.Fzh, jcdef.K2), GetControlStr(newdef.Fzh, newdef.K2),
            //                      GetControlStr(jcdef.Fzh, jcdef.K3), GetControlStr(newdef.Fzh, newdef.K3),
            //                      jcdef.K4, newdef.K4,
            //                      jcdef.K5, newdef.K5,
            //                      getzt(jcdef.Bz4), getzt(newdef.Bz4), ContString(jcdef.Bz6), ContString(newdef.Bz6),
            //                      ContString(jcdef.Bz7), ContString(newdef.Bz7)
            //                      , ContString(jcdef.Bz8), ContString(newdef.Bz8), ContString(jcdef.Bz9), ContString(newdef.Bz9), ContString(jcdef.Bz10),
            //                      ContString(newdef.Bz10), ContString(jcdef.Bz11), ContString(newdef.Bz11));
            log = string.Format(@"修改开关量【{0}】,修改时间【{1}】,", jcdef.Point, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (jcdef.Point != newdef.Point)
            {
                log += string.Format("测点号【{0}】改为【{1}】,", jcdef.Point, newdef.Point);
            }
            if (jcdef.PointID != newdef.PointID)
            {
                log += string.Format("测点ID【{0}】改为【{1}】,", jcdef.PointID, newdef.PointID);
            }
            if (jcdef.DevName != newdef.DevName)
            {
                log += string.Format("设备类型【{0}】改为【{1}】,", jcdef.DevName, newdef.DevName);
            }
            if (jcdef.DevModelID != newdef.DevModelID)
            {
                log += string.Format("型号【{0}:{1}】改为【{2}:{3}】,", jcdef.DevModelID, jcdef.DevModel, newdef.DevModelID, newdef.DevModel);
            }
            if (jcdef.Wz != newdef.Wz)
            {
                log += string.Format("位置【{0}】改为【{1}】,", ContString(jcdef.Wz), ContString(newdef.Wz));
            }
            if (jcdef.Jckz1 != newdef.Jckz1)
            {
                log += string.Format("0态交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz1), ContString(newdef.Jckz1));
            }
            if (jcdef.Jckz2 != newdef.Jckz2)
            {
                log += string.Format("1态交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz2), ContString(newdef.Jckz2));
            }
            if (jcdef.Jckz3 != newdef.Jckz3)
            {
                log += string.Format("2态交叉控制【{0}】改为【{1}】,", ContString(jcdef.Jckz3), ContString(newdef.Jckz3));
            }
            if (jcdef.K1 != newdef.K1)
            {
                log += string.Format("0态控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K1), GetControlStr(newdef.Fzh, newdef.K1));
            }
            if (jcdef.K2 != newdef.K2)
            {
                log += string.Format("1态控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K2), GetControlStr(newdef.Fzh, newdef.K2));
            }
            if (jcdef.K3 != newdef.K3)
            {
                log += string.Format("2态控制口【{0}】改为【{1}】,", GetControlStr(jcdef.Fzh, jcdef.K3), GetControlStr(newdef.Fzh, newdef.K3));
            }
            if (jcdef.K4 != newdef.K4)
            {
                log += string.Format("逻辑报警类型【{0}】改为【{1}】,", jcdef.K4, newdef.K4);
            }
            if (jcdef.K5 != newdef.K5)
            {
                log += string.Format("逻辑报警关联控制口【{0}】改为【{1}】,", jcdef.K5, newdef.K5);
            }
            if (jcdef.Bz4 != newdef.Bz4)
            {
                log += string.Format("运行状态【{0}】改为【{1}】,", getzt(jcdef.Bz4), getzt(newdef.Bz4));
            }
            if (jcdef.Bz6 != newdef.Bz6)
            {
                log += string.Format("0态显示【{0}】改为【{1}】,", ContString(jcdef.Bz6), ContString(newdef.Bz6));
            }
            if (jcdef.Bz7 != newdef.Bz7)
            {
                log += string.Format("1态显示【{0}】改为【{1}】,", ContString(jcdef.Bz7), ContString(newdef.Bz7));
            }
            if (jcdef.Bz8 != newdef.Bz8)
            {
                log += string.Format("2态显示【{0}】改为【{1}】,", ContString(jcdef.Bz8), ContString(newdef.Bz8));
            }
            if (jcdef.Bz9 != newdef.Bz9)
            {
                log += string.Format("0态颜色【{0}】改为【{1}】,", ContString(jcdef.Bz9), ContString(newdef.Bz9));
            }
            if (jcdef.Bz10 != newdef.Bz10)
            {
                log += string.Format("1态颜色【{0}】改为【{1}】,", ContString(jcdef.Bz10), ContString(newdef.Bz10));
            }
            if (jcdef.Bz11 != newdef.Bz11)
            {
                log += string.Format("2态颜色【{0}】改为【{1}】,", ContString(jcdef.Bz11), ContString(newdef.Bz11));
            }
            return log;
        }
        /// <summary>
        /// 新增或删除控制量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string AddOrDelKzlLogs(Jc_DefInfo jcdef)
        {
            string log = "";
            if (jcdef.Activity == "0")
            {
                log = string.Format(@"删除控制量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{14}】,型号【{3}:{4}】,位置【{5}】,关联馈电【{6}:{7}】,是否智能开停【{8}】
运行状态【{9}】,0态显示【{10}】,1态显示【{11}】,0态颜色【{12}】,1态颜色【{13}】",
                            jcdef.Point, jcdef.DeleteTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz),
                       jcdef.K1,
                       jcdef.K2,
                       jcdef.K3,
                       getzt(jcdef.Bz4), ContString(jcdef.Bz6), ContString(jcdef.Bz7)
                      , ContString(jcdef.Bz9), ContString(jcdef.Bz10), jcdef.DevName);
            }
            else
            {
                log = string.Format(@"新增控制量【{0}】,时间【{1}】,pointid【{2}】,设备类型【{14}】,型号【{3}:{4}】,位置【{5}】,关联馈电【{6}:{7}】,是否智能开停【{8}】
运行状态【{9}】,0态显示【{10}】,1态显示【{11}】,0态颜色【{12}】,1态颜色【{13}】",
                            jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, jcdef.DevModelID, jcdef.DevModel, ContString(jcdef.Wz),
                       jcdef.K1, jcdef.K2, jcdef.K3, getzt(jcdef.Bz4), ContString(jcdef.Bz6), ContString(jcdef.Bz7)
                      , ContString(jcdef.Bz9), ContString(jcdef.Bz10), jcdef.DevName);
            }
            return log;
        }

        /// <summary>
        /// 修改控制量测点日志
        /// </summary>
        /// <param name="jcdef"></param>
        /// <returns></returns>
        public static string UpdateKzlLogs(Jc_DefInfo jcdef, Jc_DefInfo newdef)
        {
            string log = "";
            //            log = string.Format(@"修改控制量【{0}】,时间【{1}】,pointid【{2}】改为【{3}】,型号【{4}:{5}】改为【{6}:{7}】,
            //位置【{8}】改为【{9}】,关联馈电【{10}:{11}】改为【{12}:{13}】,是否智能开停【{14}】改为【{15}】
            //运行状态【{16}】改为【{17}】,0态显示【{18}】改为【{19}】,1态显示【{20}】改为【{21}】,0态颜色【{22}】改为【{23}】,1态颜色【{24}】改为【{25}】",
            //                          jcdef.Point, jcdef.CreateUpdateTime, jcdef.PointID, newdef.PointID, jcdef.DevModelID, jcdef.DevModel,
            //                          newdef.DevModelID, newdef.DevModel, ContString(jcdef.Wz), ContString(newdef.Wz),
            //                     jcdef.K1, jcdef.K2, newdef.K1, newdef.K2, jcdef.K3, newdef.K3, getzt(jcdef.Bz4), getzt(newdef.Bz4), ContString(jcdef.Bz6), ContString(newdef.Bz6),
            //                     ContString(jcdef.Bz7), ContString(newdef.Bz7), ContString(jcdef.Bz9), ContString(newdef.Bz9), ContString(jcdef.Bz10), ContString(newdef.Bz10));
            log = string.Format(@"修改控制量【{0}】,修改时间【{1}】,", jcdef.Point, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (jcdef.Point != newdef.Point)
            {
                log += string.Format("测点号【{0}】改为【{1}】,", jcdef.Point, newdef.Point);
            }
            if (jcdef.PointID != newdef.PointID)
            {
                log += string.Format("测点ID【{0}】改为【{1}】,", jcdef.PointID, newdef.PointID);
            }
            if (jcdef.DevName != newdef.DevName)
            {
                log += string.Format("设备类型【{0}】改为【{1}】,", jcdef.DevName, newdef.DevName);
            }
            if (jcdef.DevModelID != newdef.DevModelID)
            {
                log += string.Format("型号【{0}:{1}】改为【{2}:{3}】,", jcdef.DevModelID, jcdef.DevModel, newdef.DevModelID, newdef.DevModel);
            }
            if (jcdef.Wz != newdef.Wz)
            {
                log += string.Format("位置【{0}】改为【{1}】,", ContString(jcdef.Wz), ContString(newdef.Wz));
            }
            if (jcdef.K1 != newdef.K1 || jcdef.K2 != newdef.K2)
            {
                log += string.Format("关联馈电【{0}:{1}】改为【{2}:{3}】,", jcdef.K1, jcdef.K2, newdef.K1, newdef.K2);
            }
            if (jcdef.K3 != newdef.K3)
            {
                log += string.Format("是否智能开停【{0}】改为【{1}】,", jcdef.K3, newdef.K3);
            }
            if (jcdef.Bz4 != newdef.Bz4)
            {
                log += string.Format("运行状态【{0}】改为【{1}】,", getzt(jcdef.Bz4), getzt(newdef.Bz4));
            }
            if (jcdef.Bz6 != newdef.Bz6)
            {
                log += string.Format("0态显示【{0}】改为【{1}】,", ContString(jcdef.Bz6), ContString(newdef.Bz6));
            }
            if (jcdef.Bz7 != newdef.Bz7)
            {
                log += string.Format("1态显示【{0}】改为【{1}】,", ContString(jcdef.Bz7), ContString(newdef.Bz7));
            }
            if (jcdef.Bz9 != newdef.Bz9)
            {
                log += string.Format("0态颜色【{0}】改为【{1}】,", ContString(jcdef.Bz9), ContString(newdef.Bz9));
            }
            if (jcdef.Bz10 != newdef.Bz10)
            {
                log += string.Format("1态颜色【{0}】改为【{1}】,", ContString(jcdef.Bz10), ContString(newdef.Bz10));
            }
            return log;
        }

        /// <summary>
        /// 新增模块，串口
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static string AddMacLogs(Jc_MacInfo mac)
        {
            string log = "";
            if (mac.Type == 1)
            {
                log = string.Format("新增串口【{0}】,波特率【{1}】,通讯制式【{2}】,数据位【{3}】,校验位【{4}】,停止位【{5}】",
                    mac.MAC, mac.Bz1, mac.Bz2, mac.Bz3, mac.Bz4, mac.Bz5);
            }
            else if (mac.Type == 0)
            {
                log = string.Format("新增模块【{0}】,ip【{1}】,位置【{2}】,是否透传【{3}】,挂接分站【{4}】",
                    mac.MAC, mac.IP, mac.Wz, mac.Istmcs == 0 ? "否" : "是", mac.Bz1);
            }
            return log;
        }

        public static string UpdateMacLogs(Jc_MacInfo mac, Jc_MacInfo newmac)
        {
            string log = "";
            if (mac.Type == 1)
            {
                log = string.Format(@"修改串口【{0}】,波特率【{1}】改为【{2}】,通讯制式【{3}】改为【{4}】,
                数据位【{5}】改为【{6}】,校验位【{7}】改为【{8}】,停止位【{9}】改为【{10}】",
                    mac.MAC, mac.Bz1, newmac.Bz1, mac.Bz2, newmac.Bz2, mac.Bz3, newmac.Bz3, mac.Bz4, newmac.Bz4, mac.Bz5, newmac.Bz5);
            }
            else if (mac.Type == 0)
            {
                log = string.Format(@"修改模块【{0}】,ip【{1}】改为【{2}】,位置【{3}】改为【{4}】,
                是否透传【{5}】改为【{6}】,挂接分站【{7}】改为【{8}】",
                    mac.MAC, mac.IP, newmac.IP, mac.Wz, newmac.Wz, mac.Istmcs == 0 ? "否" : "是", newmac.Istmcs == 0 ? "否" : "是", mac.Bz1, newmac.Bz1);
            }
            return log;
        }

        #endregion


        /// <summary>
        /// 获取MAC地址在本机是否存在
        /// </summary>
        /// <returns></returns>
        public static bool getMAC(string MAC)
        {
            bool macIn = false;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    if (mo["MacAddress"].ToString() == MAC)
                    {
                        macIn = true;
                    }
                }
            }
            return macIn;
        }
        /// <summary>
        /// 获取客户端是否有定义权限
        /// </summary>
        /// <returns></returns>
        public static bool GetClinetDefineState()
        {
            bool IsDefine = false;
            List<ConfigInfo> ConfigList = _ConfigService.GetConfigList().Data;
            ConfigInfo MasterMAC = ConfigList.Find(a => a.Name == "MasterMAC");
            if (MasterMAC != null)
            {
                IsDefine = CONFIGServiceModel.getMAC(MasterMAC.Text);
            }
            return IsDefine;
        }
    }

    public class MACServiceModel
    {
        static INetworkModuleService _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
        /// <summary> 添加MAC 缓存
        /// </summary>
        /// <returns></returns>
        public static bool AddMACCache(Jc_MacInfo item)
        {

            NetworkModuleAddRequest NetworkModuleRequest = new NetworkModuleAddRequest();
            NetworkModuleRequest.NetworkModuleInfo = item;
            var result = _NetworkModuleService.AddNetworkModule(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 更新MAC
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateMACCache(Jc_MacInfo item)
        {

            NetworkModuleUpdateRequest NetworkModuleRequest = new NetworkModuleUpdateRequest();
            NetworkModuleRequest.NetworkModuleInfo = item;
            var result = _NetworkModuleService.UpdateNetworkModule(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>批量添加MAC 缓存
        /// </summary>
        /// <returns></returns>
        public static bool AddMACsCache(List<Jc_MacInfo> items)
        {
            NetworkModulesRequest NetworkModuleRequest = new NetworkModulesRequest();
            NetworkModuleRequest.NetworkModulesInfo = items;
            var result = _NetworkModuleService.AddNetworkModules(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool UpdateMACsCache(List<Jc_MacInfo> items)
        {
            NetworkModulesRequest NetworkModuleRequest = new NetworkModulesRequest();
            NetworkModuleRequest.NetworkModulesInfo = items;
            var result = _NetworkModuleService.UpdateNetworkModules(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 查找所有MAC缓存信息（IP&COM）
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryAllCache()
        {
            var result = _NetworkModuleService.GetAllNetworkModuleCache();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>得到缓存中的所有IP模块
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryAllIPCache()
        {
            var result = _NetworkModuleService.GetAllNetworkModuleCache();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>得到当前缓存中已经定义的网络模块
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryAllDefineIPCache()
        {
            var result = _NetworkModuleService.GetAllNetworkModuleCache();
            if (result.Code == 100)
            {
                return result.Data.FindAll(a => a.Type == 0 && a.Wzid != "-1");
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>得到缓存中的所有COM口
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryAllCOMCache()
        {
            var result = _NetworkModuleService.GetAllNetworkModuleCache();
            if (result.Code == 100)
            {
                return result.Data.FindAll(a => a.Type == 1);
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>得到缓存中的所有IP模块+搜索到的IP模块
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> SearchALLIPCache(int StationFind)
        {
            SearchNetworkModuleRequest request = new SearchNetworkModuleRequest();
            request.StationFind = StationFind;
            var result = _NetworkModuleService.SearchALLNetworkModuleAndAddCache(request);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>得到缓存中的所有IP模块  20170112
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> SearchALLIPCache8962(int StationFind)
        {
            SearchNetworkModuleRequest request = new SearchNetworkModuleRequest();
            request.StationFind = StationFind;
            var result = _NetworkModuleService.SearchALLNetworkModuleAndAddCache(request);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                Basic.Framework.Logging.LogHelper.Error(result.Message);
                return new List<Jc_MacInfo>();//搜索异常
            }
        }

        /// <summary> 得到缓存中的所有COM口+搜索到的COM口
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> SearchALLCOMCache()
        {
            //IJC_MACService MACService = ServiceFactory.CreateService<IJC_MACService>();
            //return MACService.SearchALLCOMCache();
            return new List<Jc_MacInfo>();//暂不实现  20170531
        }
        /// <summary> 通过安装位置查询缓存中IP
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryMACByWzCache(string wz)
        {
            NetworkModuleGetByWzRequest NetworkModuleRequest = new NetworkModuleGetByWzRequest();
            NetworkModuleRequest.Wz = wz;
            var result = _NetworkModuleService.GetNetworkModuleCacheByWz(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 通过交换机mac查询缓存中IP
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> QueryMACBybz2Cache(string Bz2)
        {
            NetworkModuleGetBySwitchesMacRequest NetworkModuleRequest = new NetworkModuleGetBySwitchesMacRequest();
            NetworkModuleRequest.SwitchesMac = Bz2;
            var result = _NetworkModuleService.GetNetworkModuleCacheBySwitchesMac(NetworkModuleRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 查询所有交换机
        /// </summary>
        /// <returns></returns>
        public static List<string> QuerySwitchsCache()
        {
            var result = _NetworkModuleService.GetSwitchsPosition();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>根据编码查找MAC/COM
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static Jc_MacInfo QueryMACByCode(string Code)
        {
            NetworkModuleGetByMacRequest NetworkModuleRequest = new NetworkModuleGetByMacRequest();
            NetworkModuleRequest.Mac = Code;
            var result = _NetworkModuleService.GetNetworkModuleCacheByMac(NetworkModuleRequest);
            if (result.Code == 100)
            {
                if (result.Data.Count > 0)
                {
                    return result.Data[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> C2000 参数设置---基础参数设备
        /// </summary>
        /// <returns></returns>
        public static bool SetConvSetting(string MAC, NetDeviceSettingInfo pConvSetting, uint waitTime, string stationFind)
        {
            NetworkModuletParametersSetRequest networkModuleCacheRequest = new NetworkModuletParametersSetRequest();
            networkModuleCacheRequest.Parameters = pConvSetting;
            networkModuleCacheRequest.MAC = MAC;
            networkModuleCacheRequest.StationFind=stationFind;
            var result = _NetworkModuleService.SetNetworkModuletParameters(networkModuleCacheRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                Basic.Framework.Logging.LogHelper.Error(result.Message);
                return false;//设置参数异常
            }
        }
        /// <summary> C2000 参数设置----串口参数设置
        /// </summary>
        /// <returns></returns>
        public static bool SetConvCommSetting(string MAC, NetDeviceSettingInfo pConvSetting, uint waitTime,int CommPort)
        {
            NetworkModuletCommParametersSetRequest networkModuleCacheRequest = new NetworkModuletCommParametersSetRequest();
            networkModuleCacheRequest.Parameters = pConvSetting;
            networkModuleCacheRequest.MAC = MAC;
            networkModuleCacheRequest.CommPort = CommPort;
            var result = _NetworkModuleService.SetNetworkModuletParametersComm(networkModuleCacheRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                Basic.Framework.Logging.LogHelper.Error(result.Message);
                return false;//设置参数异常
            }
        }
        /// <summary>C2000 参数读取
        /// </summary>
        /// <returns></returns>
        public static NetDeviceSettingInfo GetConvSetting(string MAC, uint waitTime)
        {
            NetworkModuletParametersGetRequest networkModuleCacheRequest = new NetworkModuletParametersGetRequest();
            networkModuleCacheRequest.Mac = MAC;
            networkModuleCacheRequest.WaitTime = waitTime;
            var result = _NetworkModuleService.GetNetworkModuletParameters(networkModuleCacheRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                Basic.Framework.Logging.LogHelper.Error(result.Message);
                return new NetDeviceSettingInfo();//获取参数异常
            }
        }
    }

    public class DEFServiceModel
    {
        static IPointDefineService _PointDefineService = ServiceFactory.Create<IPointDefineService>();
        static IAllSystemPointDefineService _AllSystemPointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();//所有系统测点定义服务
        static IPersonPointDefineService _PersonPointDefineService = ServiceFactory.Create<IPersonPointDefineService>();//人员测点定义服务
        static IR_UndefinedDefService _R_UndefinedDefService = ServiceFactory.Create<IR_UndefinedDefService>();//人员未定义测点查找服务

        /// <summary> 添加DEF 缓存
        /// </summary>
        /// <returns></returns>
        public static bool AddDEFCache(Jc_DefInfo item)
        {
            PointDefineAddRequest PointDefineRequest = new PointDefineAddRequest();
            PointDefineRequest.PointDefineInfo = item;
            var result = new BasicResponse();
            //根据系统类型判断，调用服务端不同系统服务进行缓存及入库操作  20171122
            var resultDev = DEVServiceModel.QueryDevByDevIDCache(item.Devid);
            if (resultDev != null)
            {
                PointDefineRequest.PointDefineInfo.Sysid = resultDev.Sysid;//赋值系统ID  20171205
                if ((SystemEnum)resultDev.Sysid == SystemEnum.Security)//监控系统
                {
                    result = _PointDefineService.AddPointDefine(PointDefineRequest);
                }
                else if ((SystemEnum)resultDev.Sysid == SystemEnum.Personnel)//人员定位系统
                {
                    result = _PersonPointDefineService.AddPointDefine(PointDefineRequest);
                }
            }

            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 更新def
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateDEFCache(Jc_DefInfo item)
        {
            PointDefineUpdateRequest PointDefineRequest = new PointDefineUpdateRequest();
            PointDefineRequest.PointDefineInfo = item;
            //var result = _PointDefineService.UpdatePointDefine(PointDefineRequest);
            var result = new BasicResponse();
            //根据系统类型判断，调用服务端不同系统服务进行缓存及入库操作  20171122
            var resultDev = DEVServiceModel.QueryDevByDevIDCache(item.Devid);
            if (resultDev != null)
            {
                PointDefineRequest.PointDefineInfo.Sysid = resultDev.Sysid;//赋值系统ID  20171205
                if ((SystemEnum)resultDev.Sysid == SystemEnum.Security)//监控系统
                {
                    result = _PointDefineService.UpdatePointDefine(PointDefineRequest);
                }
                else if ((SystemEnum)resultDev.Sysid == SystemEnum.Personnel)//人员定位系统
                {
                    result = _PersonPointDefineService.UpdatePointDefine(PointDefineRequest);
                }
            }
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 更新定义及Mac信息
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="def"></param>
        /// <param name="sondef"></param>
        /// <returns></returns>
        public static bool AddUpdatePointDefineAndNetworkModuleCache(Jc_MacInfo mac, Jc_MacInfo macold, Jc_MacInfo switches, Jc_MacInfo switchesold, Jc_DefInfo def, List<Jc_DefInfo> sondef)
        {
            PointDefineAddNetworkModuleAddUpdateRequest PointDefineAddNetworkModuleRequest = new PointDefineAddNetworkModuleAddUpdateRequest();
            PointDefineAddNetworkModuleRequest.NetworkModuleInfo = mac;
            PointDefineAddNetworkModuleRequest.NetworkModuleInfoOld = macold;
            PointDefineAddNetworkModuleRequest.SwitchesInfo = switches;
            PointDefineAddNetworkModuleRequest.SwitchesInfoOld = switchesold;
            PointDefineAddNetworkModuleRequest.PointDefineInfo = def;
            PointDefineAddNetworkModuleRequest.UpdateSonPointList = sondef;
            //var result = _PointDefineService.AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleRequest);
            var result = new BasicResponse();
            //根据系统类型判断，调用服务端不同系统服务进行缓存及入库操作  20171122
            var resultDev = DEVServiceModel.QueryDevByDevIDCache(def.Devid);
            if (resultDev != null)
            {
                PointDefineAddNetworkModuleRequest.PointDefineInfo.Sysid = resultDev.Sysid;//赋值系统ID  20171205
                if ((SystemEnum)resultDev.Sysid == SystemEnum.Security)//监控系统
                {
                    result = _PointDefineService.AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleRequest);
                }
                else if ((SystemEnum)resultDev.Sysid == SystemEnum.Personnel)//人员定位系统
                {
                    result = _PersonPointDefineService.AddUpdatePointDefineAndNetworkModuleCache(PointDefineAddNetworkModuleRequest);
                }
            }
            if (result.Code == 100) //表示新增,添加定义及MAC信息
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>批量添加DEF 缓存(不支持同时入监控和人员定位)
        /// </summary>
        /// <returns></returns>
        public static bool AddDEFsCache(List<Jc_DefInfo> items)
        {
            PointDefinesAddRequest PointDefineRequest = new PointDefinesAddRequest();
            PointDefineRequest.PointDefinesInfo = items;
            //var result = _PointDefineService.AddPointDefines(PointDefineRequest);
            var result = new BasicResponse();
            //根据系统类型判断，调用服务端不同系统服务进行缓存及入库操作  20171122
            var resultDev = DEVServiceModel.QueryDevByDevIDCache(items[0].Devid);
            if (resultDev != null)
            {
                foreach (Jc_DefInfo item in PointDefineRequest.PointDefinesInfo)
                {
                    item.Sysid = resultDev.Sysid;//赋值系统ID  20171205
                }
                if ((SystemEnum)resultDev.Sysid == SystemEnum.Security)//监控系统
                {
                    result = _PointDefineService.AddPointDefines(PointDefineRequest);
                }
                else if ((SystemEnum)resultDev.Sysid == SystemEnum.Personnel)//人员定位系统
                {
                    result = _PersonPointDefineService.AddPointDefines(PointDefineRequest);
                }
            }
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 批量更新(不支持同时入监控和人员定位)
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool UpdateDEFsCache(List<Jc_DefInfo> items)
        {
            PointDefinesUpdateRequest PointDefineRequest = new PointDefinesUpdateRequest();
            PointDefineRequest.PointDefinesInfo = items;
            //var result = _PointDefineService.UpdatePointDefines(PointDefineRequest);
            var result = new BasicResponse();
            //根据系统类型判断，调用服务端不同系统服务进行缓存及入库操作  20171122
            var resultDev = DEVServiceModel.QueryDevByDevIDCache(items[0].Devid);
            if (resultDev != null)
            {
                foreach (Jc_DefInfo item in PointDefineRequest.PointDefinesInfo)
                {
                    item.Sysid = resultDev.Sysid;//赋值系统ID  20171205
                }
                if ((SystemEnum)resultDev.Sysid == SystemEnum.Security)//监控系统
                {
                    result = _PointDefineService.UpdatePointDefines(PointDefineRequest);
                }
                else if ((SystemEnum)resultDev.Sysid == SystemEnum.Personnel)//人员定位系统
                {
                    result = _PersonPointDefineService.UpdatePointDefines(PointDefineRequest);
                }
            }
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>
        /// 查找所有测点信息
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryAllCache()
        {
            //var result = _PointDefineService.GetAllPointDefineCache();
            var result = _AllSystemPointDefineService.GetAllPointDefineCache();//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>根据测点编号查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByCodeCache(string PointCode)
        {
            PointDefineGetByPointRequest PointDefineRequest = new PointDefineGetByPointRequest();
            PointDefineRequest.Point = PointCode;
            //var result = _PointDefineService.GetPointDefineCacheByPoint(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByPoint(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 查询所有人员未定义识别器缓存
        /// </summary>
        /// <returns></returns>
        public static List<R_UndefinedDefInfo> QueryAllR_UndefinedDefCache()
        {
            var request = new RUndefinedDefCacheGetAllRequest();
            return _R_UndefinedDefService.GetAllRUndefinedDefCache(request).Data;
        }

        /// <summary>根据测点名称/位置查询测点
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByWzCache(string wz)
        {
            PointDefineGetByWzRequest PointDefineRequest = new PointDefineGetByWzRequest();
            PointDefineRequest.Wz = wz;
            //var result = _PointDefineService.GetPointDefineCacheByWz(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByWz(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                return null;
            }
        }
        /// <summary>查找MAC地址下的所有分站
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByMACCache(string mac)
        {
            PointDefineGetByMacRequest PointDefineRequest = new PointDefineGetByMacRequest();
            PointDefineRequest.Mac = mac;
            //var result = _PointDefineService.GetPointDefineCacheByMac(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByMac(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 查找交换机MAC地址查找交换机下面的分站
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointBySwitchCache(string mac)
        {
            PointDefineGetByMacRequest PointDefineRequest = new PointDefineGetByMacRequest();
            PointDefineRequest.Mac = mac;
            //var result = _PointDefineService.GetPointDefineCacheByMac(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheBySwitch(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>查找COM下的所有分站
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByCOMCache(string com)
        {
            PointDefineGetByCOMRequest PointDefineRequest = new PointDefineGetByCOMRequest();
            PointDefineRequest.COM = com;
            //var result = _PointDefineService.GetPointDefineCacheByCOM(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByCOM(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备性质查找测点
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevpropertIDCache(int DevPropertID)
        {
            PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            PointDefineRequest.DevpropertID = DevPropertID;
            //var result = _PointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data.OrderBy(a => a.Point).ToList();
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备种类查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevClassIDCache(int DevClassID)
        {
            PointDefineGetByDevClassIDRequest PointDefineRequest = new PointDefineGetByDevClassIDRequest();
            PointDefineRequest.DevClassID = DevClassID;
            //var result = _PointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByDevClassID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过设备型号查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevModelIDCache(int DevModelID)
        {
            PointDefineGetByDevModelIDRequest PointDefineRequest = new PointDefineGetByDevModelIDRequest();
            PointDefineRequest.DevModelID = DevModelID;
            //var result = _PointDefineService.GetPointDefineCacheByDevModelID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByDevModelID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过设备类型查找测点
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByDevIDCache(string DevID)
        {
            PointDefineGetByDevIDRequest PointDefineRequest = new PointDefineGetByDevIDRequest();
            PointDefineRequest.DevID = DevID;
            //var result = _PointDefineService.GetPointDefineCacheByDevID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByDevID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查找分站下的所有测点(包括分站)
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByFzhCache(int fzh)
        {
            PointDefineGetByStationIDRequest PointDefineRequest = new PointDefineGetByStationIDRequest();
            PointDefineRequest.StationID = fzh;
            //var result = _PointDefineService.GetPointDefineCacheByStationID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查询所有网络通讯的分站
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static IList<Jc_DefInfo> QueryStationOfNet()
        {
            //var result = _PointDefineService.GetNetworkCommunicationStation();
            var result = _AllSystemPointDefineService.GetNetworkCommunicationStation();//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 查询所有串口通讯的分站
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryStationOfCOM()
        {
            //IJC_DEFService DEFService = ServiceFactory.CreateService<IJC_DEFService>();
            //return DEFService.QueryStationOfCOMCache();
            return new List<Jc_DefInfo>();//未实现  20170531
        }
        /// <summary> 按分站测点号查找分站下的所有测点
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByStationCache(string station)
        {
            PointDefineGetByStationPointRequest PointDefineRequest = new PointDefineGetByStationPointRequest();
            PointDefineRequest.StationPoint = station;
            //var result = _PointDefineService.GetPointDefineCacheByStationPoint(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationPoint(PointDefineRequest);//多系统融合  20171122
            List<Jc_DefInfo> StationList = result.Data;

            if (result.Code == 100)
            {
                return StationList;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary> 通过分站号、口号、设备性质
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByInfs(int fzh, int kh, int DevPropertID)
        {
            PointDefineGetByStationIDChannelIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            PointDefineRequest.DevPropertID = DevPropertID;
            //var result = _PointDefineService.GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationIDChannelIDDevPropertID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 通过分站号、设备性质 查找设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryPointByInfs(int fzh, int DevPropertID)
        {
            PointDefineGetByStationIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.DevPropertID = DevPropertID;
            //var result = _PointDefineService.GetPointDefineCacheByStationIDDevPropertID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationIDDevPropertID(PointDefineRequest);//多系统融合  20171122
            if (result.Data != null)
            {
                return result.Data.OrderBy(a => a.Point).ToList();
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary>
        /// 判断控制口是否被用作甲烷风电闭锁控制口(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ControlPointLegal(string pointId)
        {
            BasicResponse<bool> response = _PointDefineService.ControlPointLegal(new PointDefineGetByPointIDRequest() { PointID = pointId });
            if (response.IsSuccess)
                return response.Data;
            else
                throw new Exception(response.Message);
        }
        /// <summary>
        /// 判断控制口是否被用作风电闭锁控制口或者甲烷风电闭锁控制口(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool ControlPointLegalAll(string pointId)
        {
            BasicResponse<bool> response = _PointDefineService.ControlPointLegalAll(new PointDefineGetByPointIDRequest() { PointID = pointId });
            if (response.IsSuccess)
                return response.Data;
            else
                throw new Exception(response.Message);
        }
        /// <summary> 通过分站号、通道号 地址号查找设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static Jc_DefInfo QueryPointByChannelInfs(int fzh, int kh, int dzh, int DevPropertID)
        {
            PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDAddressIDDevPropertIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            PointDefineRequest.AddressID = dzh;
            PointDefineRequest.DevPropertID = DevPropertID;
            //var result = _PointDefineService.GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationIDChannelIDAddressIDDevPropertID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                if (result.Data.Count > 0)
                {
                    return result.Data[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过分站号、口号查找设备多参数设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryMulitPramPointByChannel(int fzh, int kh)
        {
            PointDefineGetByStationIDChannelIDRequest PointDefineRequest = new PointDefineGetByStationIDChannelIDRequest();
            PointDefineRequest.StationID = fzh;
            PointDefineRequest.ChannelID = kh;
            //var result = _PointDefineService.GetPointDefineCacheByStationIDChannelID(PointDefineRequest);
            var result = _AllSystemPointDefineService.GetPointDefineCacheByStationIDChannelID(PointDefineRequest);//多系统融合  20171122
            if (result.Code == 100)
            {
                List<Jc_DefInfo> resultData = new List<Jc_DefInfo>();
                if (result.Data != null && result.Data.Count > 0)
                {
                    resultData = result.Data.FindAll(a => a.Dzh > 0);
                }
                return resultData;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        /// <summary> 查找未通讯设备
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="DevPropertID"></param>
        /// <returns></returns>
        public static List<Jc_DefInfo> QueryStationNoComm()
        {
            //var result = _PointDefineService.GetNonCommunicationStation();
            var result = _AllSystemPointDefineService.GetNonCommunicationStation();//多系统融合  20171122
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 获取所有自动挂接设备列表
        /// </summary>
        /// <returns></returns>
        public static List<AutomaticArticulatedDeviceInfo> GetAllAutomaticArticulatedDeviceCache()
        {
            AutomaticArticulatedDeviceCacheGetAllRequest automaticArticulatedDeviceCacheRequest = new AutomaticArticulatedDeviceCacheGetAllRequest();
            return _PointDefineService.GetAllAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheRequest).Data;
        }
    }

    public class DEVServiceModel
    {
        static IDeviceDefineService _DeviceDefineService = ServiceFactory.Create<IDeviceDefineService>();

        /// <summary>添加设备类型缓存对象 包括更新
        /// </summary>
        /// <param name="item"></param>
        public static bool AddJC_DEVCache(Jc_DevInfo item)
        {
            DeviceDefineAddRequest DeviceDefineRequest = new DeviceDefineAddRequest();
            DeviceDefineRequest.Jc_DevInfo = item;
            var result = _DeviceDefineService.AddDeviceDefine(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        public static bool UpdateJC_DEVCache(Jc_DevInfo item)
        {
            DeviceDefineUpdateRequest DeviceDefineRequest = new DeviceDefineUpdateRequest();
            DeviceDefineRequest.Jc_DevInfo = item;
            var result = _DeviceDefineService.UpdateDeviceDefine(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        public static bool DelJC_DEVCache(Jc_DevInfo item)
        {
            DeviceDefineDeleteRequest DeviceDefineRequest = new DeviceDefineDeleteRequest();
            DeviceDefineRequest.Id = item.Devid;
            var result = _DeviceDefineService.DeleteDeviceDefine(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过所有设备
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static List<Jc_DevInfo> QueryDevsCache()
        {
            var result = _DeviceDefineService.GetAllDeviceDefineCache();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备编号查询设备
        /// </summary>
        /// <param name="pointCode"></param>
        /// <returns></returns>
        public static Jc_DevInfo QueryDevByDevIDCache(string DevID)
        {
            DeviceDefineGetByDevIdRequest DeviceDefineRequest = new DeviceDefineGetByDevIdRequest();
            DeviceDefineRequest.DevId = DevID;
            var result = _DeviceDefineService.GetDeviceDefineCacheByDevId(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public static bool QueryDevByDevMoelIDCache(int DevPropertID, int DevDevMoelID)
        {
            DeviceDefineGetByDevpropertIDDevModelIDRequest DeviceDefineRequest = new DeviceDefineGetByDevpropertIDDevModelIDRequest();
            DeviceDefineRequest.DevModelID = DevDevMoelID;
            DeviceDefineRequest.DevpropertID = DevPropertID;
            var result = _DeviceDefineService.GetDeviceDefineCacheByDevpropertIDDevModelID(DeviceDefineRequest);
            if (result.Data.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>通过设备性质查找设备
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static List<Jc_DevInfo> QueryDevByDevpropertIDCache(int DevPropertID)
        {
            DeviceDefineGetByDevpropertIDRequest DeviceDefineRequest = new DeviceDefineGetByDevpropertIDRequest();
            DeviceDefineRequest.DevpropertID = DevPropertID;
            var result = _DeviceDefineService.GetDeviceDefineCacheByDevpropertID(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备种类查找设备
        /// </summary>
        /// <param name="DevClassID"></param>
        /// <returns></returns>
        public static List<Jc_DevInfo> QueryDevByDevClassIDCache(int DevClassID)
        {
            DeviceDefineGetByDevClassIDRequest DeviceDefineRequest = new DeviceDefineGetByDevClassIDRequest();
            DeviceDefineRequest.DevClassID = DevClassID;
            var result = _DeviceDefineService.GetDeviceDefineCacheByDevClassID(DeviceDefineRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>获取设备性质枚举
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Dictionary<int, EnumcodeInfo> QueryDevPropertisCache()
        {
            var result = _DeviceDefineService.GetAllDevicePropertyCache();
            Dictionary<int, EnumcodeInfo> Result = result.Data.OrderBy(a => a.LngEnumValue).ToDictionary(key => key.LngEnumValue);
            if (result.Code == 100)
            {
                return Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>获取设备种类枚举
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Dictionary<int, EnumcodeInfo> QueryDevClasiessCache()
        {
            var result = _DeviceDefineService.GetAllDeviceClassCache();
            Dictionary<int, EnumcodeInfo> Result = result.Data.OrderBy(a => a.LngEnumValue).ToDictionary(key => key.LngEnumValue);
            if (result.Code == 100)
            {
                return Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>获取设备型号枚举
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Dictionary<int, EnumcodeInfo> QueryDevMoelsCache()
        {
            var result = _DeviceDefineService.GetAllDeviceTypeCache();
            Dictionary<int, EnumcodeInfo> Result = result.Data.OrderBy(a => a.LngEnumValue).ToDictionary(key => key.LngEnumValue);
            if (result.Code == 100)
            {
                return Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>通过设备性质查找设备种类
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public static Dictionary<int, EnumcodeInfo> QueryDevClassByDevpropertID(int DevPropertID)
        {
            var result = _DeviceDefineService.GetAllDeviceClassCache();
            List<EnumcodeInfo> DeviceClassList = result.Data;
            DeviceClassList = DeviceClassList.FindAll(a => a.LngEnumValue3 == DevPropertID.ToString()).OrderBy(a => a.LngEnumValue).ToList();
            Dictionary<int, EnumcodeInfo> Result = DeviceClassList.ToDictionary(key => key.LngEnumValue);
            if (result.Code == 100)
            {
                return Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 获取驱动信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, EnumcodeInfo> QueryDriverInf()
        {
            var result = _DeviceDefineService.GetAllDriverInf();
            Dictionary<int, EnumcodeInfo> Result = result.Data.ToDictionary(key => key.LngEnumValue);
            if (result.Code == 100)
            {
                return Result;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 得到最大的DevID
        /// </summary>
        /// <returns></returns>
        public static long GetMaxDevID()
        {
            var result = _DeviceDefineService.GetMaxDeviceDefineId();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
    }

    public class WZServiceModel
    {
        static IPositionService _PositionService = ServiceFactory.Create<IPositionService>();
        /// <summary>
        /// 添加位置缓存对象 包括更新
        /// </summary>
        /// <param name="item"></param>
        public static bool AddJC_WZCache(Jc_WzInfo item)
        {
            PositionAddRequest PositionRequest = new PositionAddRequest();
            PositionRequest.PositionInfo = item;
            var result = _PositionService.AddPosition(PositionRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 更新安装位置
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool UpdateJC_WZCache(Jc_WzInfo item)
        {
            PositionUpdateRequest PositionRequest = new PositionUpdateRequest();
            PositionRequest.PositionInfo = item;
            var result = _PositionService.UpdatePosition(PositionRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        public static bool DeleteJC_WZCache(Jc_WzInfo item)
        {
            PositionDeleteRequest PositionRequest = new PositionDeleteRequest();
            PositionRequest.Id = item.WzID;
            var result = _PositionService.DeletePosition(PositionRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 查询所有的位置
        /// </summary>
        public static List<Jc_WzInfo> QueryWZsCache()
        {
            var result = _PositionService.GetAllPositionCache();
            if (result.Code == 100)
            {
                return result.Data.OrderBy(a => long.Parse(a.WzID)).ToList();
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过位置名称查询位置
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static Jc_WzInfo QueryWZbyWZCache(string wz)
        {
            PositionGetByWzRequest PositionRequest = new PositionGetByWzRequest();
            PositionRequest.Wz = wz;
            var result = _PositionService.GetPositionCacheByWz(PositionRequest);
            if (result.Code == 100)
            {
                if (result.Data.Count > 0)
                {
                    return result.Data[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过位置ID查询位置
        /// </summary>
        /// <param name="wzid"></param>
        /// <returns></returns>
        public static Jc_WzInfo QueryWZbyWZIDCache(long wzid)
        {
            PositionGetByWzIDRequest PositionRequest = new PositionGetByWzIDRequest();
            PositionRequest.WzID = wzid.ToString();
            var result = _PositionService.GetPositionCacheByWzID(PositionRequest);
            if (result.Code == 100)
            {
                if (result.Data.Count > 0)
                {
                    return result.Data[0];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 获取安装位置的最大值
        /// </summary>
        /// <returns></returns>
        public static long GetMaxWzID()
        {
            var result = _PositionService.GetMaxPositionId();
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 在当前缓存中获取安装位置的最大值
        /// </summary>
        /// <returns></returns>
        public static long GetMaxWzidInCache(DataTable dt)
        {
            long ret = 0;
            if (null == dt)
            {
                return ret;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (null == dt.Rows[i]["wzid"])
                {
                    continue;
                }
                if (Convert.ToInt64(dt.Rows[i]["wzid"].ToString()) > ret)
                {
                    ret = Convert.ToInt64(dt.Rows[i]["wzid"].ToString());
                }
            }
            return ret;
        }
    }

    public class JCSDKZServiceModel
    {
        static IManualCrossControlService _ManualCrossControlService = ServiceFactory.Create<IManualCrossControlService>();
        /// <summary>
        /// 删除手动、交叉控制
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool DoJCSDKZ(List<Jc_JcsdkzInfo> items)
        {
            ManualCrossControlsRequest ManualCrossControlRequest = new ManualCrossControlsRequest();
            ManualCrossControlRequest.ManualCrossControlInfos = items;
            var result = _ManualCrossControlService.DeleteManualCrossControls(ManualCrossControlRequest);
            if (result.Code == 100)
            {
                return true;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过分站号查询手动控制
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static List<Jc_JcsdkzInfo> QueryJCSDKZbyInf(int Type, string ZkPoint, string BkPoint)
        {
            ManualCrossControlGetByTypeZkPointBkPointRequest ManualCrossControlRequest = new ManualCrossControlGetByTypeZkPointBkPointRequest();
            ManualCrossControlRequest.Type = Type;
            ManualCrossControlRequest.ZkPoint = ZkPoint;
            ManualCrossControlRequest.BkPoint = BkPoint;
            var result = _ManualCrossControlService.GetManualCrossControlByTypeZkPointBkPoint(ManualCrossControlRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过分站号查询手动控制
        /// </summary>
        /// <param name="wz"></param>
        /// <returns></returns>
        public static List<Jc_JcsdkzInfo> QueryJCSDKZbyInf(int Type, string BkPoint)
        {
            ManualCrossControlGetByTypeBkPointRequest ManualCrossControlRequest = new ManualCrossControlGetByTypeBkPointRequest();
            ManualCrossControlRequest.Type = Type;
            ManualCrossControlRequest.BkPoint = BkPoint;
            var result = _ManualCrossControlService.GetManualCrossControlByTypeBkPoint(ManualCrossControlRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
        /// <summary>
        /// 通过信息获取手动/交叉控制
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        public static List<Jc_JcsdkzInfo> QueryJCSDKZbyInf(string BkPoint)
        {
            ManualCrossControlGetByBkPointRequest ManualCrossControlRequest = new ManualCrossControlGetByBkPointRequest();
            ManualCrossControlRequest.BkPoint = BkPoint;
            var result = _ManualCrossControlService.GetManualCrossControlByBkPoint(ManualCrossControlRequest);
            if (result.Code == 100)
            {
                return result.Data;
            }
            else
            {
                throw new Exception(result.Message);
            }
        }
    }

    public class StatuMrg
    {
        private static string tips;
        /// <summary>
        /// 用户显示定义主框架的用户提示内容
        /// </summary>
        public static string Tips
        {
            get { return tips; }
            set
            {
                if (tips != value)
                {
                    tips = value;
                    OnTipsChanged(new EventArgs());
                }
            }
        }
        /// <summary>
        ///OnTipsChanged改变所触发的事件
        /// </summary>
        public static event EventHandler TipsChanged;
        /// <summary>
        /// 事件体
        /// </summary>
        /// <param name="eventArgs"></param>
        private static void OnTipsChanged(EventArgs eventArgs)
        {
            if (TipsChanged != null)
            {
                TipsChanged(null, eventArgs);
            }
        }
    }

    public class CopyInf
    {
        /// <summary>
        /// 带粘贴分站信息
        /// </summary>
        public static Jc_DefInfo CopyStation = null;
        /// <summary>
        /// 带粘贴传感器
        /// </summary>
        public static Jc_DefInfo CopySensor = null;
        /// <summary>
        /// 带粘贴传感器(分站下的信息)
        /// </summary>
        public static List<Jc_DefInfo> CopySensorUStation = null;
    }


    public class RelateUpdate
    {
        /// <summary>
        /// 控制量删除关联更新
        /// </summary>
        /// <param name="point"></param>
        public static void ControlReUpdate(Jc_DefInfo TempPoint)
        {
            #region 处理模开本地控制和交叉控制更新

            if (TempPoint == null)
            {
                return;
            }
            List<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
            int ControlNum = TempPoint.Kh;
            if (TempPoint.Dzh > 0)
            {
                ControlNum = TempPoint.Kh + 8;
            }
            bool UpdateFlag = false;

            List<Jc_DefInfo> AnalogTempPoint;
            List<Jc_DefInfo> DerailTempPoint;

            #region 本地控制&交叉控制处理 模拟量部分
            AnalogTempPoint = Model.DEFServiceModel.QueryPointByInfs(TempPoint.Fzh, 1);
            if (AnalogTempPoint != null)
            {
                if (AnalogTempPoint.Count > 0)
                {
                    foreach (var item in AnalogTempPoint)
                    {
                        UpdateFlag = false;
                        #region 上限报警控制口
                        if (item.K1 > 0)
                        {
                            if (((item.K1 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K1 = item.K1 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 上限断电控制口
                        if (item.K2 > 0)
                        {
                            if (((item.K2 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K2 = item.K2 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 下限报警控制口
                        if (item.K3 > 0)
                        {
                            if (((item.K3 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K3 = item.K3 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 下限断电控制口
                        if (item.K4 > 0)
                        {
                            if (((item.K4 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K4 = item.K4 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 上溢控制口
                        if (item.K5 > 0)
                        {
                            if (((item.K5 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K5 = item.K5 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 负漂控制口
                        if (item.K6 > 0)
                        {
                            if (((item.K6 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K6 = item.K6 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 断线控制口
                        if (item.K7 > 0)
                        {
                            if (((item.K7 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K7 = item.K7 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion


                        if (UpdateFlag)
                        {
                            item.InfoState = InfoState.Modified;
                            UpdatePoints.Add(item);
                        }
                    }
                }
            }


            AnalogTempPoint = Model.DEFServiceModel.QueryPointByDevpropertIDCache(1);
            if (AnalogTempPoint != null)
            {
                if (AnalogTempPoint.Count > 0)
                {
                    foreach (var item in AnalogTempPoint)
                    {
                        if (item.Fzh == TempPoint.Fzh)
                        {
                            continue;
                        }
                        UpdateFlag = false;

                        #region JCKZ1
                        if (!string.IsNullOrEmpty(item.Jckz1))
                        {
                            if (item.Jckz1.Contains(TempPoint.Point))
                            {
                                if (item.Jckz1.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz1 = item.Jckz1.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz1.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz1 = item.Jckz1.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz1.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz1 = item.Jckz1.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz1 == TempPoint.Point)
                                {
                                    item.Jckz1 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region JCKZ2
                        if (!string.IsNullOrEmpty(item.Jckz2))
                        {
                            if (item.Jckz2.Contains(TempPoint.Point))
                            {
                                if (item.Jckz2.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz2 = item.Jckz2.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz2.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz2 = item.Jckz2.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz2.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz2 = item.Jckz2.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz2 == TempPoint.Point)
                                {
                                    item.Jckz2 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region JCKZ3
                        if (!string.IsNullOrEmpty(item.Jckz3))
                        {
                            if (item.Jckz3.Contains(TempPoint.Point))
                            {
                                if (item.Jckz3.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz3 = item.Jckz3.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz3.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz3 = item.Jckz3.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz3.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz3 = item.Jckz3.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz3 == TempPoint.Point)
                                {
                                    item.Jckz3 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        if (UpdateFlag)
                        {
                            item.InfoState = InfoState.Modified;
                            UpdatePoints.Add(item);
                        }
                    }
                }
            }

            #endregion

            #region 本地控制处理&交叉控制 开关量部分
            DerailTempPoint = Model.DEFServiceModel.QueryPointByInfs(TempPoint.Fzh, 2);
            if (DerailTempPoint != null)
            {
                if (DerailTempPoint.Count > 0)
                {
                    foreach (var item in DerailTempPoint)
                    {
                        UpdateFlag = false;
                        #region 0态控制口
                        if (item.K1 > 0)
                        {
                            if (((item.K1 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K1 = item.K1 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 1态控制口
                        if (item.K2 > 0)
                        {
                            if (((item.K2 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K2 = item.K2 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region 2态控制口
                        if (item.K3 > 0)
                        {
                            if (((item.K3 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                item.K3 = item.K3 & (~(1 << (ControlNum - 1)));
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        if (UpdateFlag)
                        {
                            item.InfoState = InfoState.Modified;
                            UpdatePoints.Add(item);
                        }
                    }
                }
            }

            DerailTempPoint = Model.DEFServiceModel.QueryPointByDevpropertIDCache(2);
            if (DerailTempPoint != null)
            {
                if (DerailTempPoint.Count > 0)
                {
                    foreach (var item in DerailTempPoint)
                    {
                        if (item.Fzh == TempPoint.Fzh)
                        {
                            continue;
                        }
                        UpdateFlag = false;

                        #region JCKZ1
                        if (!string.IsNullOrEmpty(item.Jckz1))
                        {
                            if (item.Jckz1.Contains(TempPoint.Point))
                            {
                                if (item.Jckz1.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz1 = item.Jckz1.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz1.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz1 = item.Jckz1.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz1.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz1 = item.Jckz1.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz1 == TempPoint.Point)
                                {
                                    item.Jckz1 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region JCKZ2
                        if (!string.IsNullOrEmpty(item.Jckz2))
                        {
                            if (item.Jckz2.Contains(TempPoint.Point))
                            {
                                if (item.Jckz2.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz2 = item.Jckz2.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz2.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz2 = item.Jckz2.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz2.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz2 = item.Jckz2.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz2 == TempPoint.Point)
                                {
                                    item.Jckz2 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        #region JCKZ3
                        if (!string.IsNullOrEmpty(item.Jckz3))
                        {
                            if (item.Jckz3.Contains(TempPoint.Point))
                            {
                                if (item.Jckz3.Contains("|" + TempPoint.Point + "|"))
                                {
                                    item.Jckz3 = item.Jckz3.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz3.Contains(TempPoint.Point + "|"))
                                {
                                    item.Jckz3 = item.Jckz3.Replace(TempPoint.Point + "|", "");
                                }
                                else if (item.Jckz3.Contains("|" + TempPoint.Point))
                                {
                                    item.Jckz3 = item.Jckz3.Replace("|" + TempPoint.Point, "");
                                }
                                else if (item.Jckz3 == TempPoint.Point)
                                {
                                    item.Jckz3 = "";
                                }
                                UpdateFlag = true;
                            }
                        }
                        #endregion

                        if (UpdateFlag)
                        {
                            item.InfoState = InfoState.Modified;
                            UpdatePoints.Add(item);
                        }
                    }
                }
            }
            #endregion

            if (UpdatePoints != null)
            {
                if (UpdatePoints.Count > 0)
                {
                    Model.DEFServiceModel.UpdateDEFsCache(UpdatePoints);
                }
            }
            #endregion

            #region 处理手动控制
            List<Jc_JcsdkzInfo> tempSDKZ = Model.JCSDKZServiceModel.QueryJCSDKZbyInf(0, TempPoint.Point);
            if (null != tempSDKZ)
            {


                foreach (var item in tempSDKZ)
                {
                    item.InfoState = InfoState.Delete;
                    //Model.JCSDKZServiceModel.DoJCSDKZ(item);//取消，调用批量接口   20170320
                    OperateLogHelper.InsertOperateLog(4, "取消控制(被控点删除)：主控【" + item.ZkPoint + "】-【" + item.Bkpoint + "】", "");
                }

                //调用批量删除接口  20170322
                try
                {
                    if (tempSDKZ.Count > 0)
                    {
                        Model.JCSDKZServiceModel.DoJCSDKZ(tempSDKZ.ToList());
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            #endregion
        }
        /// <summary>
        /// 开关量删除关联更新
        /// </summary>
        /// <param name="point"></param>
        public static void DerailReUpdate(Jc_DefInfo TempPoint)
        {
            List<Jc_DefInfo> ControlTempPoint;
            List<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
            ControlTempPoint = Model.DEFServiceModel.QueryPointByInfs(TempPoint.Fzh, 3);
            if (ControlTempPoint != null)
            {
                if (ControlTempPoint.Count > 0)
                {
                    foreach (var item in ControlTempPoint)
                    {
                        if (item.K1 == TempPoint.Fzh && item.K2 == TempPoint.Kh && item.K4 == TempPoint.Dzh)
                        {
                            item.K1 = 0;
                            item.K2 = 0;
                            item.K4 = 0;
                            item.InfoState = InfoState.Modified;
                            UpdatePoints.Add(item);
                        }
                    }

                    if (UpdatePoints != null)
                    {
                        if (UpdatePoints.Count > 0)
                        {
                            Model.DEFServiceModel.UpdateDEFsCache(UpdatePoints);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 判断控制口是否被用作本地控制和交叉控制
        /// </summary>
        /// <param name="point"></param>
        public static bool CheckControlEnable(Jc_DefInfo TempPoint)
        {
            if (TempPoint == null)
            {
                return false;
            }
            IList<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
            int ControlNum = TempPoint.Kh;
            if (TempPoint.Dzh > 0)
            {
                ControlNum = TempPoint.Kh + 8;
            }
            bool UpdateFlag = true;

            IList<Jc_DefInfo> AnalogTempPoint;
            IList<Jc_DefInfo> DerailTempPoint;

            #region 本地控制&交叉控制处理 模拟量部分
            AnalogTempPoint = Model.DEFServiceModel.QueryPointByInfs(TempPoint.Fzh, 1);
            if (AnalogTempPoint != null)
            {
                if (AnalogTempPoint.Count > 0)
                {
                    foreach (var item in AnalogTempPoint)
                    {
                        #region 上限报警控制口
                        if (item.K1 > 0)
                        {
                            if (((item.K1 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 上限断电控制口
                        if (item.K2 > 0)
                        {
                            if (((item.K2 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 下限报警控制口
                        if (item.K3 > 0)
                        {
                            if (((item.K3 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 下限断电控制口
                        if (item.K4 > 0)
                        {
                            if (((item.K4 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 上溢控制口
                        if (item.K5 > 0)
                        {
                            if (((item.K5 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 负漂控制口
                        if (item.K6 > 0)
                        {
                            if (((item.K6 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 断线控制口
                        if (item.K7 > 0)
                        {
                            if (((item.K7 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion
                    }
                }
            }


            AnalogTempPoint = Model.DEFServiceModel.QueryPointByDevpropertIDCache(1);
            if (AnalogTempPoint != null)
            {
                if (AnalogTempPoint.Count > 0)
                {
                    foreach (var item in AnalogTempPoint)
                    {
                        if (item.Fzh == TempPoint.Fzh)
                        {
                            continue;
                        }
                        #region JCKZ1
                        if (!string.IsNullOrEmpty(item.Jckz1))
                        {
                            if (item.Jckz1.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region JCKZ2
                        if (!string.IsNullOrEmpty(item.Jckz2))
                        {
                            if (item.Jckz2.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region JCKZ3
                        if (!string.IsNullOrEmpty(item.Jckz3))
                        {
                            if (item.Jckz3.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion
                    }
                }
            }

            #endregion

            #region 本地控制处理&交叉控制 开关量部分
            DerailTempPoint = Model.DEFServiceModel.QueryPointByInfs(TempPoint.Fzh, 2);
            if (DerailTempPoint != null)
            {
                if (DerailTempPoint.Count > 0)
                {
                    foreach (var item in DerailTempPoint)
                    {
                        #region 0态控制口
                        if (item.K1 > 0)
                        {
                            if (((item.K1 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 1态控制口
                        if (item.K2 > 0)
                        {
                            if (((item.K2 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region 2态控制口
                        if (item.K3 > 0)
                        {
                            if (((item.K3 >> (ControlNum - 1)) & 0x1) == 0x1)
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion
                    }
                }
            }

            DerailTempPoint = Model.DEFServiceModel.QueryPointByDevpropertIDCache(2);
            if (DerailTempPoint != null)
            {
                if (DerailTempPoint.Count > 0)
                {
                    foreach (var item in DerailTempPoint)
                    {
                        if (item.Fzh == TempPoint.Fzh)
                        {
                            continue;
                        }
                        #region JCKZ1
                        if (!string.IsNullOrEmpty(item.Jckz1))
                        {
                            if (item.Jckz1.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region JCKZ2
                        if (!string.IsNullOrEmpty(item.Jckz2))
                        {
                            if (item.Jckz2.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion

                        #region JCKZ3
                        if (!string.IsNullOrEmpty(item.Jckz3))
                        {
                            if (item.Jckz3.Contains(TempPoint.Point))
                            {
                                UpdateFlag = false;
                            }
                        }
                        #endregion
                    }
                }
            }
            #endregion

            return UpdateFlag;
        }

        /// <summary>
        /// 判断控制口是否被用作甲烷风电闭锁控制口(true：表示未使用，false：表示已使用) //xuzp20151126
        /// </summary>
        /// <returns></returns>
        public static bool ControlPointLegal(Jc_DefInfo ControlPoint)
        {
            bool ret = true;
            if (null == ControlPoint)
            {
                return ret;
            }
            try
            {
                return Model.DEFServiceModel.ControlPointLegal(ControlPoint.PointID);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }
        /// <summary>
        /// 判断测点是否在风电闭锁中使用  20170919
        /// </summary>
        /// <param name="ControlPoint"></param>
        /// <returns></returns>
        public static bool ControlPointLegalAll(Jc_DefInfo ControlPoint)
        {
            bool ret = true;
            if (null == ControlPoint)
            {
                return ret;
            }
            try
            {
                return Model.DEFServiceModel.ControlPointLegalAll(ControlPoint.PointID);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return ret;
        }
        /// <summary>
        /// 获取所有甲烷风电闭锁控制口信息  20170923 
        /// </summary>
        /// <param name="_SourceNum"></param>
        /// <returns></returns>
        public static List<string> GetStationWindBreakControlPoint()
        {
            //获取分站对应的风电闭锁控制口  20170721
            List<string> StationWindBreakControlList = new List<string>();
            try
            {
                List<Jc_DefInfo> StationList = Model.DEFServiceModel.QueryPointByDevpropertIDCache(0);

                List<Jc_DefInfo> StatinControlPointList = Model.DEFServiceModel.QueryPointByDevpropertIDCache(3);

                foreach (Jc_DefInfo StationInfo in StationList)
                {
                    int _SourceNum = StationInfo.Fzh;
                    if (!string.IsNullOrEmpty(StationInfo.Bz11))
                    {
                        string[] WindBreakConditionArr = StationInfo.Bz9.Split('|');
                        string[] CH4WindPowerLockControlPoint = WindBreakConditionArr[2].Split('&')[0].Split(',');//瓦斯风电闭锁控制口
                        string[] WindPowerLockControlPoint = WindBreakConditionArr[2].Split('&')[1].Split(',');//风电闭锁控制口               
                        //加载瓦斯风电闭锁控制口已选择
                        foreach (string point in CH4WindPowerLockControlPoint)
                        {
                            StationWindBreakControlList.Add(point);
                        }
                        //风电闭锁控制口可以进行交叉控制  20170923
                        ////加载风电闭锁控制口已选择
                        //foreach (string point in WindPowerLockControlPoint)
                        //{
                        //    StationWindBreakControlList.Add(point);
                        //}
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(StationInfo.Bz10))
                        {
                            continue;
                        }
                        string[] fdbs = StationInfo.Bz10.Split(',');
                        if (fdbs.Length < 33)
                        {
                            continue;
                        }
                        //风电闭锁控制口可以进行交叉控制  20170923
                        //UInt16 ckwind = Convert.ToByte(fdbs[30]);
                        //ckwind |= (UInt16)(Convert.ToByte(fdbs[31]) << 8);              
                        //if (ckwind > 0)
                        //{
                        //    for (int i = 0; i < 16; i++)
                        //    {
                        //        if (((ckwind >> i) & 0x1) == 0x1)
                        //        {
                        //            foreach (Jc_DefInfo point in StatinControlPointList)
                        //            {
                        //                if (point.Point.IndexOf(((int)_SourceNum).ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0')) == 0)
                        //                {
                        //                    StationWindBreakControlList.Add(point.Point);
                        //                }
                        //            }
                        //        }
                        //    }                    
                        //}

                        UInt16 ckws = Convert.ToByte(fdbs[1]);
                        ckws |= (UInt16)(Convert.ToByte(fdbs[fdbs.Length - 1]) << 8);
                        if (ckws > 0)
                        {
                            for (int i = 0; i < 16; i++)
                            {
                                if (((ckws >> i) & 0x1) == 0x1)
                                {
                                    foreach (Jc_DefInfo point in StatinControlPointList)
                                    {
                                        if (point.Point.IndexOf(((int)_SourceNum).ToString().PadLeft(3, '0') + "C" + (i + 1).ToString().PadLeft(2, '0')) == 0)
                                        {
                                            StationWindBreakControlList.Add(point.Point);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return StationWindBreakControlList;
        }
        /// <summary>
        /// 判断当前设备是否在风电闭锁中定义(true：表示未使用，false：表示已使用)
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public static bool PointLegal(Jc_DefInfo Point)
        {
            bool ret = true;
            if (null == Point)
            {
                return ret;
            }
            List<Jc_DefInfo> tempStation = Model.DEFServiceModel.QueryPointByInfs(Point.Fzh, 0);
            if (null != tempStation)
            {
                if (tempStation.Count == 1)
                {
                    if ((tempStation[0].Bz3 & 0x01) == 0x1 || ((tempStation[0].Bz3 >> 1) & 0x1) == 0x1)
                    {
                        if (tempStation[0].Bz9.Length > 0)
                        {
                            if (tempStation[0].Bz9.Contains(Point.Point))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }
    public class EnumService
    {
        private static readonly IEnumcodeService EnumcodeService = ServiceFactory.Create<IEnumcodeService>();

        /// <summary>
        /// 获枚举
        /// </summary>
        /// <returns></returns>
        public static List<EnumcodeInfo> GetEnum(long enumTypeId)
        {
            var req = new EnumcodeGetByEnumTypeIDRequest
            {
                EnumTypeId = enumTypeId.ToString()
            };
            var res = EnumcodeService.GetEnumcodeByEnumTypeID(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }

            var data = res.Data.OrderBy(a => a.LngRowIndex);
            return data.ToList();
        }
    }
}
