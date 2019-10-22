using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Analysistemplatealarmlevel;
using Sys.Safety.ServiceContract;
using System;

namespace Sys.Safety.Client.DataAnalysis
{
    public partial class DeviceAlarmLevelSetting : XtraForm
    {
        /// <summary>
        /// 分析配置模板Id
        /// </summary>
        private string _analysistemplateId;
        /// <summary>
        /// 传感器报警等级服务
        /// </summary>
        private IJc_AnalysistemplatealarmlevelService _AnalysistemplatealarmlevelService;

        public DeviceAlarmLevelSetting(string analysistemplateId, string analysistemplateName)
        {
            InitializeComponent();
            _AnalysistemplatealarmlevelService = ServiceFactory.Create<IJc_AnalysistemplatealarmlevelService>();
            _analysistemplateId = analysistemplateId;
            this.txtname.Text = analysistemplateName;
        }

        private void DeviceAlarmLevelSetting_Load(object sender, EventArgs e)
        {
            try
            {
                AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest request = new AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest();
                request.AnalysistemplateId = _analysistemplateId;
                var alarmlevel = _AnalysistemplatealarmlevelService.GetAnalysistemplatealarmlevelByAnalysistemplateId(request).Data;

                if (alarmlevel == null)
                {
                    this.cbxlevel.SelectedIndex = 0;
                }
                else
                {
                    switch (alarmlevel.Level)
                    {
                        case 4:
                            this.cbxlevel.SelectedIndex = 1;
                            break;
                        case 3:
                            this.cbxlevel.SelectedIndex = 2;
                            break;
                        case 2:
                            this.cbxlevel.SelectedIndex = 3;
                            break;
                        case 1:
                            this.cbxlevel.SelectedIndex = 4;
                            break;
                        default:
                            this.cbxlevel.SelectedIndex = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载传感器报警等级配置失败！" + ex.Message);
            }
        }

        /// <summary>
        /// 保存分析模型传感器报警等级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest request = new AnalysistemplatealarmlevelGetByAnalysistemplateIdRequest();
                request.AnalysistemplateId = _analysistemplateId;
                var alarmlevel = _AnalysistemplatealarmlevelService.GetAnalysistemplatealarmlevelByAnalysistemplateId(request).Data;

                int level = 0;
                if (this.cbxlevel.SelectedIndex == 1)
                    level = 4;
                else if (this.cbxlevel.SelectedIndex == 2)
                    level = 3;
                else if (this.cbxlevel.SelectedIndex == 3)
                    level = 2;
                else if (this.cbxlevel.SelectedIndex == 4)
                    level = 1;
                else
                    level = 0;

                //如果不存在则新增报警等级配置
                if (alarmlevel == null)
                {
                    if (level != 0)
                    {
                        alarmlevel = new Jc_AnalysistemplatealarmlevelInfo();
                        alarmlevel.Id = IdHelper.CreateLongId().ToString();
                        alarmlevel.AnalysisModelId = _analysistemplateId;
                        alarmlevel.Level = level;

                        _AnalysistemplatealarmlevelService.AddAnalysistemplatealarmlevel(new AnalysistemplatealarmlevelAddRequest
                        {
                            AnalysistemplatealarmlevelInfo = alarmlevel
                        });
                    }
                }
                else
                {
                    //如果level==0,则删除报警等级配置；反之修改报警等级配置
                    if (level == 0)
                    {
                        _AnalysistemplatealarmlevelService.DeleteAnalysistemplatealarmlevel(new AnalysistemplatealarmlevelDeleteRequest
                        {
                            Id = alarmlevel.Id
                        });
                    }
                    else
                    {
                        alarmlevel.Level = level;
                        _AnalysistemplatealarmlevelService.UpdateAnalysistemplatealarmlevel(new AnalysistemplatealarmlevelUpdateRequest
                        {
                            AnalysistemplatealarmlevelInfo = alarmlevel
                        });
                    }
                }

                this.StaticMsg.Caption = "保存成功！";

            }
            catch (Exception ex)
            {
                LogHelper.Info("保存传感器报警等级配置失败！" + ex.Message);
                this.StaticMsg.Caption = "保存失败！";
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}
