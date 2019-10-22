using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Model;

namespace Sys.Safety.Data
{
    public partial class KJ73NDataContext : DbContext
    {

        public string ConnectionId { get; set; }

        public KJ73NDataContext()
            : base(Basic.Framework.Configuration.Global.DatabaseConfigName)
        {
            //ConnectionId = Guid.NewGuid().ToString();

            this.Database.Log = (string sql) => {
                System.Diagnostics.Debug.WriteLine(sql);
            };
        }


        public static void RegistDatabaseToIoc()
        {
            //Basic.Framework.Ioc.IocManager.RegistObject<DbContext, KJ73NDataContextForRead>("slaveDatabase" , Framework.Ioc.InstanceType.PerDependency);
            //Basic.Framework.Ioc.IocManager.RegistObject<DbContext, KJ73NDataContextForWrite>("masterDatabase", Framework.Ioc.InstanceType.PerDependency);
        }

        //public KJ73NDataContext(string connectionString):base(connectionString)
        //{

        //}

        public void SetConnectionString(string connectionString)
        {
            this.Database.Connection.ConnectionString = connectionString;
        }

        public DbSet<AreaModel> AreaModel { get; set; }
        public DbSet<ClassModel> ClassModel { get; set; }
        public DbSet<DatarightModel> DatarightModel { get; set; }
        public DbSet<DeletecheckModel> DeletecheckModel { get; set; }
        public DbSet<DeptinfModel> DeptinfModel { get; set; }
        public DbSet<EnumcodeModel> EnumcodeModel { get; set; }
        public DbSet<EnumtypeModel> EnumtypeModel { get; set; }
        public DbSet<FklibModel> FklibModel { get; set; }
        public DbSet<ListcommandexModel> ListcommandexModel { get; set; }
        public DbSet<ListdataexModel> ListdataexModel { get; set; }
        public DbSet<ListdatalayountModel> ListdatalayountModel { get; set; }
        public DbSet<ListdisplayexModel> ListdisplayexModel { get; set; }
        public DbSet<ListexModel> ListexModel { get; set; }
        public DbSet<ListmetadataModel> ListmetadataModel { get; set; }
        public DbSet<ListtempleModel> ListtempleModel { get; set; }
        public DbSet<MenuModel> MenuModel { get; set; }
        public DbSet<MetadataModel> MetadataModel { get; set; }
        public DbSet<MetadatafieldsModel> MetadatafieldsModel { get; set; }
        public DbSet<OperatelogModel> OperatelogModel { get; set; }
        public DbSet<RequestModel> RequestModel { get; set; }
        public DbSet<RightModel> RightModel { get; set; }
        public DbSet<RoleModel> RoleModel { get; set; }
        public DbSet<RoledatarightModel> RoledatarightModel { get; set; }
        public DbSet<RolefieldsModel> RolefieldsModel { get; set; }
        public DbSet<RolerightModel> RolerightModel { get; set; }
        public DbSet<RolewebmenuModel> RolewebmenuModel { get; set; }
        public DbSet<RolewebmenuauthModel> RolewebmenuauthModel { get; set; }
        public DbSet<RunlogModel> RunlogModel { get; set; }
        public DbSet<SettingModel> SettingModel { get; set; }
        public DbSet<SysinfModel> SysinfModel { get; set; }
        public DbSet<UserModel> UserModel { get; set; }
        public DbSet<UserrightModel> UserrightModel { get; set; }
        public DbSet<UserroleModel> UserroleModel { get; set; }
        public DbSet<WebauthorityModel> WebauthorityModel { get; set; }
        public DbSet<WebmenuModel> WebmenuModel { get; set; }
        public DbSet<WebmenuauthModel> WebmenuauthModel { get; set; }
        public DbSet<ConfigModel> ConfigModel { get; set; }
        public DbSet<DataexchangesettingModel> DataexchangesettingModel { get; set; }
        public DbSet<FlagModel> FlagModel { get; set; }
        public DbSet<GraphicsbaseinfModel> GraphicsbaseinfModel { get; set; }
        public DbSet<GraphicspointsinfModel> GraphicspointsinfModel { get; set; }
        public DbSet<GraphicsrouteinfModel> GraphicsrouteinfModel { get; set; }
        public DbSet<Jc_BModel> Jc_BModel { get; set; }
        //public DbSet<Jc_B201703Model> Jc_B201703Model { get; set; } 
        public DbSet<Jc_BxModel> Jc_BxModel { get; set; }
        public DbSet<Jc_BxexModel> Jc_BxexModel { get; set; }
        public DbSet<Jc_BzModel> Jc_BzModel { get; set; }
        public DbSet<Jc_CsModel> Jc_CsModel { get; set; }
        public DbSet<Jc_DayModel> Jc_DayModel { get; set; }
        public DbSet<Jc_DefModel> Jc_DefModel { get; set; }
        public DbSet<Jc_DefwbModel> Jc_DefwbModel { get; set; }
        public DbSet<Jc_DevModel> Jc_DevModel { get; set; }
        public DbSet<Jc_HourModel> Jc_HourModel { get; set; }
        //public DbSet<Jc_Hour201703Model> Jc_Hour201703Model { get; set; } 
        public DbSet<Jc_JcsdkzModel> Jc_JcsdkzModel { get; set; }
        public DbSet<Jc_KdModel> Jc_KdModel { get; set; }
        //public DbSet<Jc_Kd201703Model> Jc_Kd201703Model { get; set; } 
        //public DbSet<Jc_LlModel> Jc_LlModel { get; set; }
        public DbSet<Jc_Ll_DModel> Jc_Ll_DModel { get; set; }
        public DbSet<Jc_Ll_HModel> Jc_Ll_HModel { get; set; }
        //public DbSet<Jc_Ll_H201703Model> Jc_Ll_H201703Model { get; set; } 
        public DbSet<Jc_Ll_MModel> Jc_Ll_MModel { get; set; }
        public DbSet<Jc_Ll_YModel> Jc_Ll_YModel { get; set; }
        public DbSet<Jc_MModel> Jc_MModel { get; set; }
        //public DbSet<Jc_M20170308Model> Jc_M20170308Model { get; set; } 
        public DbSet<Jc_MacModel> Jc_MacModel { get; set; }
        public DbSet<Jc_McModel> Jc_McModel { get; set; }
        //public DbSet<Jc_Mc20170308Model> Jc_Mc20170308Model { get; set; } 
        public DbSet<Jc_MonthModel> Jc_MonthModel { get; set; }
        //public DbSet<Jc_PointhisModel> Jc_PointhisModel { get; set; }
        public DbSet<Jc_RModel> Jc_RModel { get; set; }
        //public DbSet<Jc_R201703Model> Jc_R201703Model { get; set; } 
        public DbSet<Jc_RemarkModel> Jc_RemarkModel { get; set; }
        public DbSet<Jc_SeasonModel> Jc_SeasonModel { get; set; }
        public DbSet<Jc_ShowModel> Jc_ShowModel { get; set; }
        public DbSet<Jc_WzModel> Jc_WzModel { get; set; }
        public DbSet<Jc_YearModel> Jc_YearModel { get; set; }
        //public DbSet<LighthistoryModel> LighthistoryModel { get; set; }
        public DbSet<JC_AnalysistemplateModel> JC_AnalysistemplateModel { get; set; }
        public DbSet<JC_AnalysistemplateconfigModel> JC_AnalysistemplateconfigModel { get; set; }
        public DbSet<JC_AnalyticalexpressionModel> JC_AnalyticalexpressionModel { get; set; }
        public DbSet<JC_EmergencylinkageconfigModel> JC_EmergencylinkageconfigModel { get; set; }
        public DbSet<JC_ExpressionconfigModel> JC_ExpressionconfigModel { get; set; }
        public DbSet<JC_FactorModel> JC_FactorModel { get; set; }
        public DbSet<JC_LargedataanalysisconfigModel> JC_LargedataanalysisconfigModel { get; set; }
        public DbSet<JC_LargedataanalysislogModel> JC_LargedataanalysislogModel { get; set; }
        public DbSet<JC_ParameterModel> JC_ParameterModel { get; set; }
        public DbSet<JC_RegionoutageconfigModel> JC_RegionoutageconfigModel { get; set; }
        public DbSet<JC_SetanalysismodelpointrecordModel> JC_SetanalysismodelpointrecordModel { get; set; }
        public DbSet<JC_AlarmnotificationpersonnelconfigModel> JC_AlarmnotificationpersonnelconfigModel { get; set; }
        public DbSet<JC_AlarmNotificationPersonnelModel> JC_AlarmNotificationPersonnelModel { get; set; }   
        
        public DbSet<JC_AlarmHandleModel> JC_AlarmHandleModel { get; set; }

        public DbSet<StaionHistoryDataModel> StaionHistoryDataModel { get; set; }
        public DbSet<StaionControlHistoryDataModel> StaionControlHistoryDataModel { get; set; }

        public DbSet<JC_MbModel> JC_MbModel { get; set; }
        public DbSet<JC_MultiplesettingModel> JC_MultiplesettingModel { get; set; }

        public DbSet<MsgLogModel> MsgLogModel { get; set; }
        public DbSet<MsgRuleModel> MsgRuleModel { get; set; }
        public DbSet<MsgUserRuleModel> MsgUserRuleModel { get; set; }

        public DbSet<ListdataremarkModel> ListdataremarkModel { get; set; }

        public DbSet<AreaRuleModel> AreaRuleModel { get; set; }

        public DbSet<ShortCutMenuModel> ShortCutMenuModel { get; set; }

        public DbSet<R_DeptModel> R_DeptModel { get; set; }
        public DbSet<R_PersoninfModel> R_PersoninfModel { get; set; }
        public DbSet<R_PhistoryModel> R_PhistoryModel { get; set; }
        public DbSet<R_PrealModel> R_PrealModel { get; set; }
        public DbSet<R_CallModel> R_CallModel { get; set; }
        public DbSet<R_DefModel> R_DefModel { get; set; }
        public DbSet<R_PhjModel> R_PhjModel { get; set; }
        public DbSet<R_RestrictedpersonModel> R_RestrictedpersonModel { get; set; }

        public DbSet<V_DefModel> V_DefModel { get; set; }

        public DbSet<EmergencyLinkageMasterAreaAssModel> EmergencyLinkageMasterAreaAssModel { get; set; }
        public DbSet<EmergencyLinkageMasterDevTypeAssModel> EmergencyLinkageMasterDevTypeAssModel { get; set; }
        public DbSet<EmergencyLinkageMasterPointAssModel> EmergencyLinkageMasterPointAssModel { get; set; }
        public DbSet<EmergencyLinkageMasterTriDataStateAssModel> EmergencyLinkageMasterTriDataStateAssModel { get; set; }
        public DbSet<EmergencyLinkagePassiveAreaAssModel> EmergencyLinkagePassiveAreaAssModel { get; set; }
        public DbSet<EmergencyLinkagePassivePersonAssModel> EmergencyLinkagePassivePersonAssModel { get; set; }
        public DbSet<EmergencyLinkagePassivePointAssModel> EmergencyLinkagePassivePointAssModel { get; set; }
        public DbSet<EmergencyLinkHistoryModel> EmergencyLinkHistoryModel { get; set; } 
        public DbSet<EmergencyLinkageHistoryMasterPointAssModel> EmergencyLinkageHistoryMasterPointAssModel { get; set; } 

        public DbSet<R_UndefinedDefModel> UndefinedDefModel { get; set; }
        public DbSet<SysEmergencyLinkageModel> SysEmergencyLinkageModel { get; set; }

        public DbSet<R_AreaAlarmModel> AreaAlarmModel { get; set; }
        public DbSet<R_ArearestrictedpersonModel> ArearestrictedpersonModel { get; set; }
        public DbSet<R_KqbcModel> KqbcModel { get; set; }
        public DbSet<R_PbModel> PbModel { get; set; }

        public DbSet<R_SyncLocalModel> SyncLocalModel { get; set; } 

        public DbSet<B_CallModel> B_CallModel { get; set; }
        public DbSet<B_BroadcastplanModel> BroadcastplanModel { get; set; }
        public DbSet<B_BroadcastplanpointlistModel> BroadcastplanpointlistModel { get; set; }
        public DbSet<B_CallhistoryModel> CallhistoryModel { get; set; }
        public DbSet<B_CallhistorypointlistModel> CallhistorypointlistModel { get; set; }
        public DbSet<B_CallpointlistModel> CallpointlistModel { get; set; }
        public DbSet<B_MusicfilesModel> MusicfilesModel { get; set; }
        public DbSet<B_PlaylistModel> PlaylistModel { get; set; }
        public DbSet<B_PlaylistmusiclinkModel> PlaylistmusiclinkModel { get; set; } 

        public DbSet<B_DefModel> B_DefModel { get; set; }

        public DbSet<PowerboxchargehistoryModel> PowerboxchargehistoryModel { get; set; }

        public DbSet<GascontentanalyzeconfigModel> GascontentanalyzeconfigModel { get; set; }

        public DbSet<Jc_AnalysistemplatealarmlevelModel> Jc_AnalysistemplatealarmlevelModel { get; set; }

        public DbSet<KJ_AddresstypeModel> AddresstypeModel { get; set; }
        public DbSet<KJ_AddresstyperuleModel> AddresstyperuleModel { get; set; } 
    }
}
