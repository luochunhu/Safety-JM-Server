using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsbaseinf;
using Basic.Framework.Service;
using Basic.Framework.Logging;
using Basic.Framework.Common;
using Sys.Safety.Enums;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request;
using Sys.Safety.Request.Listex;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Request.PersonCache;

namespace Sys.Safety.Client.Graphic
{
    /// <summary>
    /// 实时刷新
    /// </summary>
    public class RefPointSsz
    {
        private IGraphicsbaseinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();

        private ILargeDataAnalysisCacheClientService largeDataAnalysisCacheClientService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        private ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();

        private IR_PrealService prealSerive = ServiceFactory.Create<IR_PrealService>();

        private IPersonPointDefineService rdefService = ServiceFactory.Create<IPersonPointDefineService>();

        private IV_DefService vdefService = ServiceFactory.Create<IV_DefService>();

        private IR_CallService rcallService = ServiceFactory.Create<IR_CallService>();

        private IR_PhjService r_PhjService = ServiceFactory.Create<IR_PhjService>();

        private IDeviceDefineService devService = ServiceFactory.Create<IDeviceDefineService>();

        private IBroadCastPointDefineService _bdefService = ServiceFactory.Create<IBroadCastPointDefineService>();

        private IB_CallService _bcallService = ServiceFactory.Create<IB_CallService>();

        private List<Jc_DevInfo> DevInfos = new List<Jc_DevInfo>();

        /// <summary>
        /// 实时数据刷新线程
        /// </summary>
        private System.Threading.Thread m_PointSszThread;
        /// <summary>
        /// 读写标记锁
        /// </summary>
        private ReaderWriterLock _rwLocker;
        /// <summary>
        /// 测点实时值字符串集
        /// </summary>
        private List<string> PointSsz;
        /// <summary>
        /// 图形测点列表
        /// </summary>
        private DataTable PointInMap;
        /// <summary>
        /// 运行标记
        /// </summary>
        public bool _isRun = false;
        /// <summary>
        /// 中心站flag表时间临时变量
        /// </summary>
        string sxflag;
        /// <summary>
        /// 中心站中断计数器
        /// </summary>
        int sxerr;
        /// <summary>
        /// 中心站中断次数
        /// </summary>
        int freshOutTime = 20;
        /// <summary>
        /// 当前图形id
        /// </summary>
        string GraphNameNow = "";
        /// <summary>
        /// 存储枚举的内存表
        /// </summary>
        DataTable dtStateVal = new DataTable();
        /// <summary>
        /// 是否连接异常标记
        /// </summary>
        bool isConnLose = false;

        /// <summary>
        /// 应急联动配置ID
        /// </summary>
        public string SysEmergencyLinkageInfoId = string.Empty;

        /// <summary>
        /// 启动线程
        /// </summary>
        public void Start()
        {
            if (!_isRun)
            {
                _isRun = true;
                PointSsz = new List<string>();
                _rwLocker = new ReaderWriterLock();

                //读取所有设备、数据状态枚举类型
                var response = graphicsbaseinfService.GetAllDeviceEnumcode();
                dtStateVal = response.Data;

                DevInfos = devService.GetAllDeviceDefineCache().Data;


                m_PointSszThread = new Thread(setAllPointSszThread);
                //object o = mx;
                m_PointSszThread.Start();

                //注册关闭事件
                RequestUtil.OnMainFormCloseEvent += new RequestUtil.OnMainFormClose(MainFormCloseEvent);
            }
        }
        /// <summary>
        /// 主控关闭委托事件
        /// </summary>
        private void MainFormCloseEvent()
        {
            //停止实时刷新
            Stop();
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _isRun = false;
        }

        private void setAllPointSszThread()
        {
            while (_isRun)
            {
                try
                {
                    if (GraphNameNow != Program.main.GraphOpt.GraphNameNow)
                    {
                        //加载图形测点
                        PointInMap = getAllPointInMap();
                    }

                    setAllPointSsz();//读取所有实时值数据到内存
                }
                catch (Exception ex)
                {
                    LogHelper.Error("RefPointSsz-setAllPointSszThread" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(3000);
            }
        }
        /// <summary>
        /// 返回图形测点列表
        /// </summary>
        /// <returns></returns>
        private DataTable getAllPointInMap()
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            DataTable pointsdt = new DataTable();
            try
            {
                GraphicsbaseinfDTO_ = getGraphicDto(Program.main.GraphOpt.GraphNameNow);
                //先删除图形原有的绑定测点信息

                var getAllPointInMapRequest = new GetAllPointInMapRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
                var getAllPointInMapResponse = graphicsbaseinfService.GetAllPointInMap(getAllPointInMapRequest);
                pointsdt = getAllPointInMapResponse.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("RefPointSsz-getAllPointInMap" + ex.Message + ex.StackTrace);
            }
            return pointsdt;
        }
        /// <summary>
        /// 获取所有测点的实时值
        /// </summary>
        /// <returns>返回格式：测点号,实时值,设备状态,数据状态值,是否报警（1：报警，0：不报警）,数据状态文本,设备状态文本,人员定位设备是否呼叫（1：井上呼叫，2，井下呼叫，3，井上和惊吓呼叫，0：不呼叫）</returns>
        private void setAllPointSsz()
        {
            _rwLocker.AcquireWriterLock(-1);
            try
            {
                PointSsz.Clear();
                string pointssz = string.Empty;

                //修改为从实时缓存中读取数据
                DataTable pointsdt = PointInMap;

                var pointResponse = graphicsbaseinfService.GetDefPointInformationInCache();
                DataTable pointsszdt = ObjectConverter.ToDataTable<Jc_DefInfo>(pointResponse.Data);

                var masResponse = graphicsbaseinfService.GetSwitchInformationInCache();
                DataTable dt_ip = ObjectConverter.ToDataTable<Jc_MacInfo>(masResponse.Data);

                //应急联动配置
                var SysEmergencyLinkageInfos = sysEmergencyLinkageService.GetAllSysEmergencyLinkageList().Data;

                //大数据分析模型
                var largeDataAnalysisInfos = largeDataAnalysisCacheClientService.GetAllLargeDataAnalysisConfigCache(new LargeDataAnalysisCacheClientGetAllRequest()).Data;

                //人员定位
                var prealinfos = prealSerive.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
                var rdefinfos = rdefService.GetAllPointDefineCache().Data;
                if (prealinfos == null)
                {
                    prealinfos = new List<R_PrealInfo>();
                }
                var rcallinfos = rcallService.GetAllRCallCache(new RCallCacheGetAllRequest()).Data;

                //视频
                var videoinfos = vdefService.GetAllVideoDefCache().Data;
                //广播
                var bdefinfos = _bdefService.GetAllPointDefineCache().Data;

                //当前应急联动配置
                var currSysEmergencyLinkageInfo = SysEmergencyLinkageInfos.FirstOrDefault(o => o.Id == SysEmergencyLinkageInfoId);
                //应急联动呼叫关联MasterId
                var masterid = "";
                if (currSysEmergencyLinkageInfo != null)
                {
                    masterid = currSysEmergencyLinkageInfo.Type == 2 ? currSysEmergencyLinkageInfo.MasterModelId : currSysEmergencyLinkageInfo.Id;
                }

                dt_ip.Columns.Add("ssz");
                dt_ip.Columns.Add("type");
                DataView dv = pointsszdt.DefaultView;
                dv.Sort = "alarm Asc";
                pointsszdt = dv.ToTable();
                foreach (DataRow tmpdr in dt_ip.Rows)
                {
                    DataRow drtemp = pointsszdt.NewRow();
                    drtemp["state"] = tmpdr["state"];
                    drtemp["datastate"] = tmpdr["state"];
                    drtemp["Point"] = tmpdr["IP"];
                    drtemp["ID"] = tmpdr["ID"];
                    DataRow[] drstateIP = dtStateVal.Select("lngEnumValue='" + tmpdr["state"].ToString() + "'");
                    if (drstateIP.Length > 0)
                    {
                        drtemp["ssz"] = drstateIP[0]["strEnumDisplay"].ToString();
                        drtemp["alarm"] = "0";
                    }
                    else
                    {
                        drtemp["ssz"] = "未知";
                        drtemp["alarm"] = "0";
                    }
                    pointsszdt.Rows.InsertAt(drtemp, 0);
                }

                for (int i = 0; i < pointsdt.Rows.Count; i++)
                {
                    pointssz = "";
                    string callstate = "0";
                    pointssz += pointsdt.Rows[i]["Point"].ToString() + ",";

                    int sysid = Convert.ToInt32(pointsdt.Rows[i]["SysId"].ToString());
                    switch (sysid)
                    {
                        case 0:
                        case (int)SystemEnum.Security:
                            pointssz += SecurityHandle(pointsdt, i, pointsszdt);
                            break;
                        case (int)SystemEnum.Broadcast:
                            pointssz += BroadCastHandle(pointsdt, i, bdefinfos);
                            break;
                        case (int)SystemEnum.Personnel:
                            pointssz += PersonnelHandle(pointsdt, i, pointsszdt, rdefinfos, rcallinfos, prealinfos, masterid, ref callstate);
                            break;
                        case (int)SystemEnum.Video:
                            pointssz += VideoHandle(pointsdt, i, videoinfos, currSysEmergencyLinkageInfo, SysEmergencyLinkageInfos);
                            break;
                        case -1:
                            pointssz += AnalysisConfigHandle(pointsdt, i, largeDataAnalysisInfos);
                            break;
                        default:
                            pointssz += "无数据,0,46,0,未知,未知";
                            break;
                    }

                    pointssz += "," + callstate;
                    PointSsz.Add(pointssz);
                }

                isConnLose = false;
            }
            catch (Exception ex)
            {
                LogHelper.Error("RefPointSsz-setAllPointSsz" + ex.Message + ex.StackTrace);
                isConnLose = true;
            }
            finally
            {
                _rwLocker.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// 返回所有实时值字符集
        /// </summary>
        /// <returns></returns>
        public string getAllPointSsz()
        {
            string R_PointSsz = "";
            if (!isConnLose)
            {
                _rwLocker.AcquireReaderLock(-1);


                try
                {
                    foreach (string tempssz in PointSsz)
                    {
                        R_PointSsz += tempssz + "|";
                    }
                    if (R_PointSsz.Contains("|"))
                    {
                        R_PointSsz = R_PointSsz.Substring(0, R_PointSsz.Length - 1);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("RefPointSsz-getAllPointSsz" + ex.Message + ex.StackTrace);
                }
                finally
                {
                    _rwLocker.ReleaseReaderLock();
                }
            }
            return R_PointSsz.ToString();
        }
        /// <summary>
        /// 根据图形名称获取图形DTO对象
        /// </summary>
        /// <param name="GraphName"></param>
        /// <returns></returns>
        private GraphicsbaseinfInfo getGraphicDto(string GraphName)
        {
            GraphicsbaseinfInfo Rvalue = new GraphicsbaseinfInfo();
            try
            {
                var getGraphicsbaseinfByNameRequest = new GetGraphicsbaseinfByNameRequest() { GraphName = GraphName };
                var getGraphicsbaseinfByNameResponse = graphicsbaseinfService.GetGraphicsbaseinfByName(getGraphicsbaseinfByNameRequest);
                Rvalue = getGraphicsbaseinfByNameResponse.Data;
            }
            catch (Exception ex)
            {
                LogHelper.Error("RefPointSsz-getGraphicDto" + ex.Message + ex.StackTrace);
            }
            return Rvalue;
        }

        public DataTable GetAllPointInMap()
        {
            return PointInMap;
        }

        /// <summary>
        /// 监控实时值刷新
        /// </summary>
        /// <param name="pointsdt"></param>
        /// <param name="rowindex"></param>
        /// <param name="pointsszdt"></param>
        /// <returns></returns>
        private string SecurityHandle(DataTable pointsdt, int rowindex, DataTable pointsszdt)
        {
            string pointssz = string.Empty;
            try
            {
                string tempPoint = "", tempstate = "", tempssz = "", tempdatatext = "", tempstatetext = "";
                DataRow[] dr, drdata, drstate;

                //测点号
                if (pointsdt.Rows[rowindex]["Point"].ToString().Contains("￣"))
                    tempPoint = pointsdt.Rows[rowindex]["Point"].ToString().Substring(0, pointsdt.Rows[rowindex]["Point"].ToString().IndexOf('￣'));
                else
                    tempPoint = pointsdt.Rows[rowindex]["Point"].ToString();

                dr = pointsszdt.Select("point='" + tempPoint + "'");

                //获取图元名称，如果是填充图元需要特殊处理
                string graphBindName = pointsdt.Rows[rowindex]["GraphBindName"].ToString();
                if (graphBindName.Contains("填充"))
                {
                    if (dr.Length > 0)
                    {
                        //如果是正常设备状态
                        if (dr[0]["State"].ToString() == "21")
                        {
                            var devinfo = DevInfos.FirstOrDefault(dev => dev.Devid == dr[0]["DevId"].ToString());
                            int lc = devinfo == null ? 0 : devinfo.LC;
                            double ssz = Convert.ToDouble(dr[0]["ssz"].ToString());
                            tempssz = ((int)((ssz / lc) * 100)).ToString();
                        }
                        else
                        {
                            tempssz = "0";
                        }
                    }

                    pointssz += tempssz + "," + "0," + "0," + "0," + "正常," + "正常";
                }
                else
                {
                    if (dr.Length > 0)
                    {
                        if (dr[0]["DevPropertyID"].ToString() == "1" || dr[0]["DevPropertyID"].ToString() == "4"
                            || dr[0]["DevPropertyID"].ToString() == "5")//模拟量,累计量，导出量
                        {
                            if (dr[0]["datastate"].ToString() == "20" || dr[0]["datastate"].ToString() == "22" || dr[0]["datastate"].ToString() == "23"
                                || string.IsNullOrEmpty(dr[0]["ssz"].ToString()))
                                tempssz = dr[0]["ssz"].ToString();
                            else
                                tempssz = dr[0]["ssz"].ToString() + dr[0]["Unit"].ToString();
                        }
                        else
                        {
                            tempssz = dr[0]["ssz"].ToString();
                        }
                        //实时值
                        pointssz += tempssz + ",";

                        if (dr[0]["DevPropertyID"].ToString() == "2")//开关量
                        {
                            tempstate = Convert.ToString(int.Parse(dr[0]["datastate"].ToString()) - 25);
                        }
                        else if (dr[0]["DevPropertyID"].ToString() == "3")//控制量
                        {
                            if (dr[0]["datastate"].ToString() == "43")
                                tempstate = "0";
                            else if (dr[0]["datastate"].ToString() == "44")
                                tempstate = "1";
                            else
                                tempstate = "2";
                        }
                        else
                        {
                            tempstate = dr[0]["datastate"].ToString();
                        }
                        //状态
                        pointssz += tempstate + ",";

                        //数据状态值
                        pointssz += dr[0]["datastate"].ToString() + ",";

                        //上、下限报警断电显示报警相关动画  
                        if (dr[0]["datastate"].ToString() == "10" || dr[0]["datastate"].ToString() == "12"
                            || dr[0]["datastate"].ToString() == "16" || dr[0]["datastate"].ToString() == "18")
                            pointssz += "1,";//设置报警             
                        else
                            pointssz += "0,";//设置不报警

                        //数据状态
                        drdata = dtStateVal.Select("lngEnumValue='" + dr[0]["datastate"].ToString() + "'");
                        if (drdata.Length > 0)
                            tempdatatext = drdata[0]["strEnumDisplay"].ToString();
                        else
                            tempdatatext = "未知";

                        pointssz += tempdatatext + ",";
                        //设备状态
                        drstate = dtStateVal.Select("lngEnumValue='" + dr[0]["state"].ToString() + "'");
                        if (drstate.Length > 0)
                        {
                            tempstatetext = drstate[0]["strEnumDisplay"].ToString();
                        }
                        else
                        {
                            tempstatetext = "未知";
                        }
                        pointssz += tempstatetext + ",";
                    }
                    else
                    {
                        pointssz += "无数据,0,46,0,未知,未知";
                    }
                }

                return pointssz;
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载监控设备出错！" + ex.Message);
                pointssz += "无数据,0,46,0,未知,未知";
                return pointssz;
            }
        }

        /// <summary>
        /// 广播实时值刷新
        /// </summary>
        /// <param name="pointsdt"></param>
        /// <param name="rowindex"></param>
        /// <param name="pointsszdt"></param>
        /// <returns></returns>
        private string BroadCastHandle(DataTable pointsdt, int rowindex, List<Jc_DefInfo> bdefinfos)
        {
            string pointssz = string.Empty;

            try
            {
                string tempstate = "", tempdatastate = "", tempssz = "", tempdatatext = "", tempstatetext = "";
                DataRow[] drdata, drstate;

                //if (pointsdt.Rows[rowindex]["Point"].ToString().Contains("￣"))
                //{
                //    tempPoint = pointsdt.Rows[rowindex]["Point"].ToString().Substring(0, pointsdt.Rows[rowindex]["Point"].ToString().IndexOf('￣'));
                //    //tempPoint = pointsdt.Rows[i]["Point"].ToString();
                //}
                //else
                //{
                //    tempPoint = pointsdt.Rows[rowindex]["Point"].ToString();
                //}
                //dr = pointsszdt.Select("point='" + tempPoint + "'");

                var bdefinfo = bdefinfos.FirstOrDefault(o => o.Point == pointsdt.Rows[rowindex]["Point"].ToString());

                if (bdefinfo != null)
                {

                    var bcallinfos = _bcallService.GetAllCache().Data;

                    var defcallinfos = bcallinfos.Where(o => o.CallType != 2 && o.CallPointList != null && o.CallPointList.Select(c => c.CalledPointId).Contains(bdefinfo.PointID)).ToList();

                    //实时值
                    tempssz = bdefinfo.Ssz;
                    pointssz += tempssz + ",";
                    //设备状态值
                    tempstate = bdefinfo.State.ToString();
                    pointssz += tempstate + ",";
                    //数据状态值
                    if (defcallinfos.Count > 0)
                    {
                        tempdatastate = "49";
                    }
                    else
                    {
                        tempdatastate = bdefinfo.DataState.ToString();
                    }

                    pointssz += tempdatastate + ",";
                    //报警状态
                    pointssz += "0,";//设置不报警
                    //数据状态
                    drdata = dtStateVal.Select("lngEnumValue='" + tempdatastate + "'");
                    if (drdata.Length > 0)
                    {
                        tempdatatext = drdata[0]["strEnumDisplay"].ToString();
                    }
                    else
                    {
                        tempdatatext = "未知";
                    }

                    pointssz += tempdatatext + ",";
                    //设备状态
                    drstate = dtStateVal.Select("lngEnumValue='" + tempstate + "'");
                    if (drstate.Length > 0)
                    {
                        tempstatetext = drstate[0]["strEnumDisplay"].ToString();
                    }
                    else
                    {
                        tempstatetext = "未知";
                    }
                    pointssz += tempstatetext;
                }
                else
                {
                    pointssz += "无数据,0,0,0,未知,未知";
                }

                return pointssz;
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载广播设备出错！" + ex.Message);
                pointssz += "无数据,0,46,0,未知,未知";
                return pointssz;
            }
        }

        /// <summary>
        /// 人员定位实时值刷新
        /// </summary>
        /// <param name="pointsdt"></param>
        /// <param name="rowindex"></param>
        /// <param name="pointsszdt"></param>
        /// <param name="rdefinfos"></param>
        /// <param name="rcallinfos"></param>
        /// <param name="prealinfos"></param>
        /// <param name="masterid"></param>
        /// <param name="callstate"></param>
        /// <returns></returns>
        private string PersonnelHandle(DataTable pointsdt, int rowindex, DataTable pointsszdt, List<Jc_DefInfo> rdefinfos, List<R_CallInfo> rcallinfos, List<R_PrealInfo> prealinfos, string masterid, ref string callstate)
        {

            string pointssz = string.Empty;

            try
            {
                string tempdatatext = "", tempstatetext = "";
                DataRow[] drdata, drstate;

                var rdef = rdefinfos.FirstOrDefault(o => o.Point == pointsdt.Rows[rowindex]["Point"].ToString());
                if (rdef != null)
                {
                    //实时值
                    pointssz += rdef.Ssz + ",";
                    //设备状态
                    pointssz += rdef.State.ToString() + ",";
                    var defprealinfos = prealinfos.Where(o => o.Pointid == rdef.PointID).ToList();

                    bool isalarm = false;

                    bool isupcall = false;
                    bool isdowncall = false;
                    var phjinfos = r_PhjService.GetPhjAlarmedList().Data;

                    //如果此识别器没有人员实时信息
                    if (defprealinfos.Count == 0)
                    {
                        //数据状态
                        //pointssz += rdef.DataState.ToString() + ",";

                        List<R_CallInfo> defcalls = new List<R_CallInfo>();

                        //如果是应急联动刷新，则取应急联动生成的RCall记录；否则取全部的RCall记录
                        if (string.IsNullOrEmpty(SysEmergencyLinkageInfoId))
                            defcalls = rcallinfos.Where(o => !string.IsNullOrEmpty(o.PointList) && o.PointList.Contains(rdef.Point)).ToList();
                        else
                            defcalls = rcallinfos.Where(o => !string.IsNullOrEmpty(o.PointList) && o.PointList.Contains(rdef.Point) && o.MasterId == masterid).ToList();

                        if (defcalls != null && defcalls.Count > 0)
                        {
                            isupcall = true;
                        }
                    }
                    //如果此识别器有人员实时信息,则根据人员信息判断识别器状态
                    else
                    {
                        //bool isalarm = false;

                        //bool isupcall = false;
                        //bool isdowncall = false;
                        //var phjinfos = r_PhjService.GetPhjAlarmedList().Data;

                        //判断是否存在呼叫
                        foreach (var preal in defprealinfos)
                        {
                            List<R_CallInfo> personcalls = new List<R_CallInfo>();
                            //如果是应急联动刷新，则取应急联动生成的RCall记录；否则取全部的RCall记录
                            if (string.IsNullOrEmpty(SysEmergencyLinkageInfoId))
                                personcalls = rcallinfos.Where(o => !string.IsNullOrEmpty(o.BhContent) && o.BhContent.Contains(preal.Bh)).ToList();
                            else
                                personcalls = rcallinfos.Where(o => !string.IsNullOrEmpty(o.BhContent) && o.BhContent.Contains(preal.Bh) && o.MasterId == masterid).ToList();
                            if (personcalls.Count > 0)
                            {
                                isupcall = true;
                                callstate = "1";
                            }

                            if (phjinfos.Contains(preal.Bh))
                            {
                                isdowncall = true;
                            }

                            if (preal.Bjtype > 0)
                            {
                                //报警
                                isalarm = true;
                            }
                        }
                    }

                    //井上呼叫
                    if (isupcall && !isdowncall)
                    {
                        callstate = "1";
                    }
                    //井下呼叫
                    else if (!isupcall && isdowncall)
                    {
                        callstate = "2";
                    }
                    //井上和井下呼叫
                    else if (isupcall && isdowncall)
                    {
                        callstate = "3";
                    }
                    else
                        callstate = "0";


                    //识别器报警状态根据人员标识卡状态判断
                    pointssz += isalarm ? "47," : rdef.DataState.ToString() + ",";
                    //}

                    //报警状态
                    pointssz += "0,";
                    //数据状态描述
                    drdata = dtStateVal.Select("lngEnumValue='" + rdef.DataState.ToString() + "'");
                    if (drdata.Length > 0)
                    {
                        tempdatatext = drdata[0]["strEnumDisplay"].ToString();
                    }
                    else
                    {
                        tempdatatext = "未知";
                    }
                    pointssz += tempdatatext + ",";
                    //设备状态描述
                    drstate = dtStateVal.Select("lngEnumValue='" + rdef.DataState.ToString() + "'");
                    if (drstate.Length > 0)
                    {
                        tempstatetext = drstate[0]["strEnumDisplay"].ToString();
                    }
                    else
                    {
                        tempstatetext = "未知";
                    }
                }
                else
                {
                    pointssz += "无数据,0,46,0,未知,未知";
                }

                return pointssz;
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载人员设备出错！" + ex.Message);
                pointssz += "无数据,0,46,0,未知,未知";
                return pointssz;
            }
        }

        /// <summary>
        /// 视频实时值刷新
        /// </summary>
        /// <param name="pointsdt"></param>
        /// <param name="rowindex"></param>
        /// <param name="videoinfos"></param>
        /// <param name="sysEmergencyLinkageInfo"></param>
        /// <param name="SysEmergencyLinkageInfos"></param>
        /// <returns></returns>
        private string VideoHandle(DataTable pointsdt, int rowindex, List<V_DefInfo> videoinfos, SysEmergencyLinkageInfo sysEmergencyLinkageInfo, List<SysEmergencyLinkageInfo> SysEmergencyLinkageInfos)
        {
            string pointssz = string.Empty;
            try
            {
                bool isreal = false;

                //如果存在包含此视频测点的应急联动配置成立，则把视频测点状态置1

                if (string.IsNullOrEmpty(SysEmergencyLinkageInfoId))
                {
                    foreach (var item in SysEmergencyLinkageInfos)
                    {
                        if (item.EmergencyLinkageState == 1)
                        {
                            var passivepoints = sysEmergencyLinkageService.GetPassivePointInfoByAssId(new LongIdRequest { Id = Convert.ToInt64(item.PassivePointAssId) }).Data.Select(o => o.Point).ToList();

                            var vdefinfo = videoinfos.FirstOrDefault(o => o.IPAddress == pointsdt.Rows[rowindex]["Point"].ToString());

                            if (vdefinfo != null && passivepoints.Contains(vdefinfo.Devname))
                            {
                                pointssz += vdefinfo.Devname + "," + "1," + "1," + "0," + "正常," + "正常";
                                isreal = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (sysEmergencyLinkageInfo != null && sysEmergencyLinkageInfo.EmergencyLinkageState == 1)
                    {
                        var passivepoints = sysEmergencyLinkageService.GetPassivePointInfoByAssId(new LongIdRequest { Id = Convert.ToInt64(sysEmergencyLinkageInfo.PassivePointAssId) }).Data.Select(o => o.Point).ToList();

                        var vdefinfo = videoinfos.FirstOrDefault(o => o.IPAddress == pointsdt.Rows[rowindex]["Point"].ToString());

                        if (vdefinfo != null && passivepoints.Contains(vdefinfo.IPAddress))
                        {
                            pointssz += vdefinfo.Devname + "," + "1," + "1," + "0," + "正常," + "正常";
                            isreal = true;
                        }
                    }
                }

                if (!isreal)
                {
                    var vdefinfo = videoinfos.FirstOrDefault(o => o.IPAddress == pointsdt.Rows[rowindex]["Point"].ToString());

                    if (vdefinfo == null)
                    {
                        pointssz += "无数据," + "0," + "0," + "0," + "未知," + "未知";

                    }
                    else
                    {
                        pointssz += vdefinfo.Devname + "," + "0," + "0," + "0," + "正常," + "正常";
                    }

                }

                return pointssz;
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载视频设备出错！" + ex.Message);
                pointssz += "无数据,0,46,0,未知,未知";
                return pointssz;
            }
        }

        /// <summary>
        /// 分析模型实时值刷新
        /// </summary>
        /// <param name="pointsdt"></param>
        /// <param name="rowindex"></param>
        /// <param name="largeDataAnalysisInfos"></param>
        /// <returns></returns>
        private string AnalysisConfigHandle(DataTable pointsdt, int rowindex, List<JC_LargedataAnalysisConfigInfo> largeDataAnalysisInfos)
        {
            string pointssz = string.Empty;
            try
            {
                var largeDataAnalysisInfo = largeDataAnalysisInfos.FirstOrDefault(o => o.Name == pointsdt.Rows[rowindex]["Point"].ToString());
                if (largeDataAnalysisInfo != null)
                {
                    if (largeDataAnalysisInfo.AnalysisResult == 2)
                    {
                        pointssz += "成立," + "1," + "1," + "0," + "成立," + "成立";
                    }
                    else
                    {
                        pointssz += "不成立," + "0," + "0," + "0," + "不成立," + "不成立";
                    }
                }
                else
                {
                    pointssz += "无数据," + "0," + "0," + "0," + "未知," + "未知";
                }

                return pointssz;
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载分析模型出错！" + ex.Message);
                pointssz += "无数据,0,46,0,未知,未知";
                return pointssz;
            }
        }
    }
}
