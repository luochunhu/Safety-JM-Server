using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.EmergencyLinkHistory;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.Request.R_Call;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class SetEmergencyLinkageDelayTime : XtraForm
    {
        private ISysEmergencyLinkageService sysEmergencyLinkageService = ServiceFactory.Create<ISysEmergencyLinkageService>();
        private SysEmergencyLinkageInfo selectSysEmergencyLinkageInfo = null;
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

        public SetEmergencyLinkageDelayTime(SysEmergencyLinkageInfo SysEmergencyLinkageInfo)
        {
            InitializeComponent();
            selectSysEmergencyLinkageInfo = SysEmergencyLinkageInfo;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtreason.Text.Trim()))
                {
                    XtraMessageBox.Show("应急联动强制结束必须填写结束原因！");
                    return;
                }

                //强制结束应急联动1.更新呼叫控制 2.更新应急联动状态 3.更新应急联动运行记录
                if (selectSysEmergencyLinkageInfo.EmergencyLinkageState != 0)
                {
                    var masterid = selectSysEmergencyLinkageInfo.Type == 1 ? selectSysEmergencyLinkageInfo.Id : selectSysEmergencyLinkageInfo.MasterModelId;

                    //解除广播控制
                    BCallInfoGetByMasterIDRequest b_defExistsRequest = new BCallInfoGetByMasterIDRequest();
                    b_defExistsRequest.MasterId = masterid;
                    b_defExistsRequest.CallType = 1;
                    var bcallinfo = bCallService.GetBCallInfoByMasterID(b_defExistsRequest).Data.FirstOrDefault();
                    if (bcallinfo != null)
                    {
                        bcallinfo.CallType = 2;
                        bCallService.UpdateCall(new B_CallUpdateRequest { CallInfo = bcallinfo });
                    }

                    //解除人员定位控制
                    RCallInfoGetByMasterIDRequest rcallgetRequest = new RCallInfoGetByMasterIDRequest();
                    rcallgetRequest.MasterId = masterid;
                    rcallgetRequest.CallType = 1;
                    rcallgetRequest.IsQueryByType = false;
                    rcallgetRequest.Type = 0;
                    var rcallinfo = rCallService.GetRCallInfoByMasterID(rcallgetRequest).Data;
                    if (rcallinfo.Count > 0)
                    {
                        rcallinfo.ForEach(o =>
                        {
                            o.CallType = 2;
                            rCallService.UpdateCall(new R_CallUpdateRequest { CallInfo = o });
                        });
                    }

                    //2.更新配置缓存
                    selectSysEmergencyLinkageInfo.EmergencyLinkageState = 0;
                    selectSysEmergencyLinkageInfo.IsForceEnd = true;
                    selectSysEmergencyLinkageInfo.EndTime = DateTime.Now;
                    var delaytime = Convert.ToInt32(this.spinEdit1.Text);
                    //延迟时间数据库存秒
                    selectSysEmergencyLinkageInfo.DelayTime = delaytime * 60;

                    SysEmergencyLinkageUpdateRequest upddateRequest = new SysEmergencyLinkageUpdateRequest();
                    upddateRequest.SysEmergencyLinkageInfo = selectSysEmergencyLinkageInfo;
                    sysEmergencyLinkageService.UpdateSysEmergencyLinkage(upddateRequest);
                    //3.更新运行记录
                    var emergencyLinkHistory = emergencyLinkHistoryService.GetEmergencyLinkHistoryByEmergency(new EmergencyLinkHistoryGetByEmergencyRequest { EmergencyId = selectSysEmergencyLinkageInfo.Id }).Data;
                    if (emergencyLinkHistory != null && !string.IsNullOrEmpty(emergencyLinkHistory.Id))
                    {
                        emergencyLinkHistory.EndTime = DateTime.Now;
                        emergencyLinkHistory.IsForceEnd = 1;
                        emergencyLinkHistory.By1 = this.txtreason.Text;

                        ClientItem _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                        if (_ClientItem != null)
                        {
                            emergencyLinkHistory.EndPerson = _ClientItem.UserID;
                        }
                        emergencyLinkHistoryService.UpdateEmergencyLinkHistory(new EmergencyLinkHistoryUpdateRequest { EmergencyLinkHistoryInfo = emergencyLinkHistory });
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("应急联动强制结束失败:" + ex.Message);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
