using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Enums;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.R_Personinf;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;
using Sys.Safety.Client.Graphic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetEmergencyLinkageList : XtraForm
    {
        List<SysEmergencyLinkageInfo> SysEmergencyLinkageInfos = new List<SysEmergencyLinkageInfo>();
        private ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();
        private IV_DefService vdefService = ServiceFactory.Create<IV_DefService>();
        private IBroadCastPointDefineService bdefService = ServiceFactory.Create<IBroadCastPointDefineService>();
        private IPersonPointDefineService rdefService = ServiceFactory.Create<IPersonPointDefineService>();
        private IR_PersoninfService personinfoService = ServiceFactory.Create<IR_PersoninfService>();
        private IR_PrealService prealService = ServiceFactory.Create<IR_PrealService>();

        /// <summary>
        /// 多系统融合应急联动配置服务
        /// </summary>
        IEmergencyLinkHistoryService emergencyLinkHistoryService = ServiceFactory.Create<IEmergencyLinkHistoryService>();

        /// <summary>
        /// 广播呼叫缓存服务
        /// </summary>
        IB_CallService bCallService = ServiceFactory.Create<IB_CallService>();

        /// <summary>
        /// 人员呼叫服务
        /// </summary>
        IR_CallService rCallService = ServiceFactory.Create<IR_CallService>();

        /// <summary>
        /// 大数据分析配置缓存服务
        /// </summary>
        ILargeDataAnalysisCacheClientService largedataAnalysisConfigCacheService = ServiceFactory.Create<ILargeDataAnalysisCacheClientService>();

        /// <summary>
        /// 应急联动图形展示界面
        /// </summary>
        GraphDrawingRefresh graphDrawingRefresh = null;

        /// <summary>
        /// 当前选中应急联动配置ID
        /// </summary>
        string selectid = string.Empty;

        /// <summary>
        /// 是否是实时页面
        /// </summary>
        private bool isreal = true;

        public SetEmergencyLinkageList()
        {
            InitializeComponent();
            RefreshSysEmergencyLinkageList(true);
            StaticClass.SystemOut = false;
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetEmergencyLinkageList_Load(object sender, EventArgs e)
        {
            //设置窗体高度和宽度
            Width = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.9);
            Height = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.9);
            Left = Convert.ToInt32(Screen.GetWorkingArea(this).Width * 0.1 / 2);
            Top = Convert.ToInt32(Screen.GetWorkingArea(this).Height * 0.1 / 2);


            Thread refreshThread = new Thread(new ThreadStart(RefreshData));
            refreshThread.IsBackground = true;
            refreshThread.Start();
        }

        private void RefreshData()
        {
            while (!StaticClass.SystemOut)
            {
                try
                {
                    var jsonStr = RefreshRealPoint();
                    if (graphDrawingRefresh != null)
                        graphDrawingRefresh.RefreshEmergencyLinkMap(jsonStr);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("应急联动刷新出错：" + ex.StackTrace + "\r\n" + ex.Message);
                }
                Thread.Sleep(4000);
            }
        }

        private void SetEmergencyLinkageList_FormClosing(object sender, FormClosingEventArgs e)
        {
            StaticClass.SystemOut = true;
            if (groupControlForm.Controls.Count > 0)
            {
                GraphDrawingRefresh itemForm = groupControlForm.Controls[0] as GraphDrawingRefresh;
                itemForm.Close();
                itemForm.Dispose();
            }
        }

        private void listBoxControlLargedataModule_Click(object sender, EventArgs e)
        {
            DevExpress.Utils.WaitDialogForm wdf = new DevExpress.Utils.WaitDialogForm("正在打开应急联动展示窗口...", "请等待...");
            try
            {
                var selectitem = this.listBoxControlLargedataModule.SelectedValue;

                ListBoxControl ctlListBox = sender as ListBoxControl;
                if (selectitem != null)
                {
                    selectid = selectitem.ToString();
                    if (selectid != null)
                    {
                        var jsonStr = RefreshRealPoint();

                        if (graphDrawingRefresh == null)
                        {
                            graphDrawingRefresh = null;
                            graphDrawingRefresh = new GraphDrawingRefresh(jsonStr.ToString(), selectid);
                            graphDrawingRefresh.FormBorderStyle = FormBorderStyle.None;
                            graphDrawingRefresh.MaximizeBox = false;
                            graphDrawingRefresh.MinimizeBox = false;
                            graphDrawingRefresh.TopLevel = false;
                            graphDrawingRefresh.ControlBox = false;
                            graphDrawingRefresh.WindowState = FormWindowState.Normal;
                            graphDrawingRefresh.Visible = true;
                            graphDrawingRefresh.Dock = DockStyle.Fill;
                        }
                        else
                        {
                            graphDrawingRefresh.GraphOpt.refPointssz.SysEmergencyLinkageInfoId = selectid;
                            graphDrawingRefresh.RefreshEmergencyLinkMap(jsonStr);
                            graphDrawingRefresh.RefreshEmergencyLinkMap();
                        }

                        if (groupControlForm.Controls.Count == 0)
                        {
                            groupControlForm.Controls.Add(graphDrawingRefresh);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info(string.Format("加载应急联动展示窗口出错, 错误消息:{0}", ex.Message));
                XtraMessageBox.Show("加载应急联动展示窗口出错, 错误消息:\n" + ex.Message, "加载窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                wdf.Close();
            }
        }

        /// <summary>
        /// 实时显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshSysEmergencyLinkageList(true);
            isreal = true;
        }

        /// <summary>
        /// 显示全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshSysEmergencyLinkageList(false);
            isreal = false;
        }

        /// <summary>
        /// 加载应急联动配置列表
        /// </summary>
        /// <param name="isreal">true-显示实时；false-显示全部</param>
        private void RefreshSysEmergencyLinkageList(bool isreal)
        {
            var response = sysEmergencyLinkageService.GetAllSysEmergencyLinkageList();
            SysEmergencyLinkageInfos.Clear();

            if (isreal)
            {
                SysEmergencyLinkageInfos = response.Data.Where(o => o.EmergencyLinkageState == 1).ToList();
            }
            else
            {
                SysEmergencyLinkageInfos = response.Data;
            }

            this.listBoxControlLargedataModule.DataSource = SysEmergencyLinkageInfos;
            this.listBoxControlLargedataModule.DisplayMember = "Name";
            this.listBoxControlLargedataModule.ValueMember = "Id";
            this.listBoxControlLargedataModule.SelectedIndex = -1;
            this.listBoxControlLargedataModule.Refresh();
        }

        /// <summary>
        /// 详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectid))
            {
                SysEmergencyLinkageRealtime detailform = new SysEmergencyLinkageRealtime(selectid);
                detailform.ShowDialog();
            }
        }

        /// <summary>
        /// 强制结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (listBoxControlLargedataModule.SelectedValue != null)
            {
                var selectid = listBoxControlLargedataModule.SelectedValue.ToString();
                var selectSysEmergencyLinkageInfo = SysEmergencyLinkageInfos.FirstOrDefault(o => o.Id == selectid);

                if (selectSysEmergencyLinkageInfo.EmergencyLinkageState != 0)
                {
                    var delaytimeform = new SetEmergencyLinkageDelayTime(selectSysEmergencyLinkageInfo);
                    delaytimeform.ShowDialog();
                }
                else
                {
                    XtraMessageBox.Show("请选择一条实时记录！", "消息");
                    return;
                }
            }
            else
            {
                XtraMessageBox.Show("请选择一条实时记录！", "消息");
                return;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取应急联动实时主控和被控测点
        /// </summary>
        private string RefreshRealPoint()
        {
            try
            {
                string jsonStr = string.Empty;
                if (!string.IsNullOrEmpty(selectid))
                {
                    var selectSysEmergencyLinkageInfo = SysEmergencyLinkageInfos.FirstOrDefault(o => o.Id == selectid);

                    //获取选中应急联动配置的主控测点
                    SysEmergencyLinkageGetRequest getrequest = new SysEmergencyLinkageGetRequest();
                    getrequest.Id = selectid;
                    var masterpoints = sysEmergencyLinkageService.GetAllMasterPointsById(getrequest).Data;
                    //如果不存在主控测点，则获取关联大数据分析模型配置
                    if (masterpoints.Count > 0)
                    {
                        masterpoints.ForEach(o =>
                        {
                            jsonStr += o.Point + "|";
                        });
                    }
                    else if (!string.IsNullOrEmpty(selectSysEmergencyLinkageInfo.MasterModelId))
                    {
                        var analysisConfigs = largedataAnalysisConfigCacheService.GetAllLargeDataAnalysisConfigCache(new Sys.Safety.Request.LargeDataAnalysisCacheClientGetAllRequest()).Data;
                        if (analysisConfigs != null && analysisConfigs.Count > 0)
                        {
                            var analysisConfig = analysisConfigs.FirstOrDefault(o => o.Id == selectSysEmergencyLinkageInfo.MasterModelId);
                            if (analysisConfig != null)
                                jsonStr += analysisConfig.Name + "|";
                        }
                    }

                    //获取选中应急联动配置的被控测点
                    //如果应急联动配置存在被控区域，则获取此区域的人员设备和广播设备
                    if (selectSysEmergencyLinkageInfo.PassiveAreas.Count > 0)
                    {
                        selectSysEmergencyLinkageInfo.PassiveAreas.ForEach(area =>
                        {
                            //处理区域广播呼叫
                            var bdefinforesponse = bdefService.GetAllPointDefineCache();
                            if (bdefinforesponse.IsSuccess)
                            {
                                var areabdefinfos = bdefinforesponse.Data.Where(b => b.Areaid == area.AreaId).ToList();
                                areabdefinfos.ForEach(def => jsonStr += def.Point + "|");
                            }

                            //处理区域人员呼叫
                            var rdefinforesponse = rdefService.GetAllPointDefineCache();
                            if (rdefinforesponse.IsSuccess)
                            {
                                var areardefinfos = rdefinforesponse.Data.Where(b => b.Areaid == area.AreaId).ToList();
                                areardefinfos.ForEach(def => jsonStr += def.Point + "|");
                            }
                        });
                    }

                    //如果应急联动配置存在被控人员，则获取被控人员实时识别器列表
                    if (selectSysEmergencyLinkageInfo.PassivePersons.Count > 0)
                    {
                        var prealinfos = prealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;

                        if (prealinfos != null)
                            selectSysEmergencyLinkageInfo.PassivePersons.ForEach(p =>
                            {
                                var personreponse = personinfoService.GetAllDefinedPersonInfoCache(new BasicRequest());
                                if (personreponse.IsSuccess)
                                {
                                    var person = personinfoService.GetPersoninfCache(new R_PersoninfGetRequest() { Id = p.PersonId }).Data;
                                    var preal = prealinfos.FirstOrDefault(pr => pr.Bh == person.Bh);

                                    if (preal != null)
                                    {
                                        var persondef = rdefService.GetPointDefineCacheByPointID(new PointDefineGetByPointIDRequest { PointID = preal.Pointid }).Data;
                                        if (persondef != null && !jsonStr.Contains(persondef.Point))
                                            jsonStr += persondef.Point + "|";
                                    }
                                }
                            });
                    }

                    //如果应急联动配置存在被控设备，则根据类型获取被控测点
                    if (selectSysEmergencyLinkageInfo.PassivePoints.Count > 0)
                    {
                        selectSysEmergencyLinkageInfo.PassivePoints.ForEach(p =>
                        {
                            //人员定位
                            if (p.Sysid == (int)SystemEnum.Personnel)
                            {
                                var rdef = rdefService.GetPointDefineCacheByPointID(new PointDefineGetByPointIDRequest { PointID = p.PointId }).Data;
                                if (rdef != null && !jsonStr.Contains(rdef.Point))
                                    jsonStr += rdef.Point + "|";
                            }
                            //广播
                            else if (p.Sysid == (int)SystemEnum.Broadcast)
                            {
                                var bdef = bdefService.GetPointDefineCacheByPointID(new PointDefineGetByPointIDRequest { PointID = p.PointId }).Data;

                                var bdefpoints = jsonStr.Split('|');
                                if (bdef != null && !bdefpoints.Contains(bdef.Point))
                                    jsonStr += bdef.Point + "|";
                            }
                            //视频
                            else if (p.Sysid == (int)SystemEnum.Video)
                            {
                                var vdef = vdefService.GetDefById(new Sys.Safety.Request.Def.DefGetRequest { Id = p.PointId }).Data;
                                if (vdef != null && !jsonStr.Contains(vdef.IPAddress))
                                    jsonStr += vdef.IPAddress + "|";
                            }
                        }); 
                    }

                    if (jsonStr.LastIndexOf('|') > 0)
                    {
                        jsonStr = jsonStr.Substring(0, jsonStr.Length - 1);
                    }
                }

                return jsonStr;
            }
            catch (Exception ex)
            {
                LogHelper.Info("获取应急联动配置主控被控测点出错：" + ex.Message);
                return string.Empty;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isreal)
            {
                RefreshSysEmergencyLinkageList(true);
            }
        }
    }
}
