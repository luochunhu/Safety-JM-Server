using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    public partial class ChartSetting : XtraForm
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        public ChartSetting()
        {
            InitializeComponent();
        }
        public void Reload()
        {            
            object sender1 = null;
            var e1 = new EventArgs();
            ChartSetting_Load(sender1, e1);
        }
        private void ChartSetting_Load(object sender, EventArgs e)
        {
            try
            {
                var req = new GetAllChartSetRequest
                {
                    StrKey = ""
                };
                var res = _chartService.GetAllChartSet(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);

                var ChartSets = res.Data;
                for (var i = 0; i < ChartSets.Rows.Count; i++)
                    switch (ChartSets.Rows[i]["strKey"].ToString())
                    {
                        case "Chart_ZdzColor":
                            zdzColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_ZxzColor":
                            zxzColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_PjzColor":
                            pjzColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_JczColor":
                            jczColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_YdzColor":
                            ydzColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_McColor":
                            mcColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_KglColor":
                            kglColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_MonthColor":
                            monthColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_BgColor":
                            bgColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_BxColor":
                            bxcolor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_PseudoColor":
                            colorPseudo.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_BjColor":
                            bjcolor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_WarnColor"://预警阈值颜色
                            warnColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_AlarmColor"://报警阈值颜色
                            alarmColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_PowerOffColor"://预警阈值颜色
                            poweroffColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                        case "Chart_PowerOnColor"://预警阈值颜色
                            poweronColor.Color = Color.FromArgb(int.Parse(ChartSets.Rows[i]["strValue"].ToString()));
                            break;
                    }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ChartSetting_Load" + ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var Chart_ZdzColor = zdzColor.Color.ToArgb().ToString();
                var Chart_ZxzColor = zxzColor.Color.ToArgb().ToString();
                var Chart_PjzColor = pjzColor.Color.ToArgb().ToString();
                var Chart_JczColor = jczColor.Color.ToArgb().ToString();
                var Chart_YdzColor = ydzColor.Color.ToArgb().ToString();
                var Chart_McColor = mcColor.Color.ToArgb().ToString();
                var Chart_KglColor = kglColor.Color.ToArgb().ToString();
                var Chart_BgColor = bgColor.Color.ToArgb().ToString();
                var Chart_MonthColor = monthColor.Color.ToArgb().ToString();
                var Chart_BxColor = bxcolor.Color.ToArgb().ToString();
                var Chart_PseudoColor = colorPseudo.Color.ToArgb().ToString();
                var Chart_BjColor = bjcolor.Color.ToArgb().ToString();
                //增加阈值颜色设置  20171218
                var Chart_WarnColor = warnColor.Color.ToArgb().ToString();
                var Chart_AlarmColor = alarmColor.Color.ToArgb().ToString();
                var Chart_PowerOffColor = poweroffColor.Color.ToArgb().ToString();
                var Chart_PowerOnColor = poweronColor.Color.ToArgb().ToString();
                var ChartSets = new Dictionary<string, string>();
                ChartSets.Add("Chart_ZdzColor", Chart_ZdzColor);
                ChartSets.Add("Chart_ZxzColor", Chart_ZxzColor);
                ChartSets.Add("Chart_PjzColor", Chart_PjzColor);
                ChartSets.Add("Chart_JczColor", Chart_JczColor);
                ChartSets.Add("Chart_YdzColor", Chart_YdzColor);
                ChartSets.Add("Chart_McColor", Chart_McColor);
                ChartSets.Add("Chart_KglColor", Chart_KglColor);
                ChartSets.Add("Chart_BgColor", Chart_BgColor);
                ChartSets.Add("Chart_MonthColor", Chart_MonthColor);
                ChartSets.Add("Chart_BxColor", Chart_BxColor);
                ChartSets.Add("Chart_PseudoColor", Chart_PseudoColor);
                ChartSets.Add("Chart_BjColor", Chart_BjColor);

                ChartSets.Add("Chart_WarnColor", Chart_WarnColor);
                ChartSets.Add("Chart_AlarmColor", Chart_AlarmColor);
                ChartSets.Add("Chart_PowerOffColor", Chart_PowerOffColor);
                ChartSets.Add("Chart_PowerOnColor", Chart_PowerOnColor);
                    
                //ServiceFactory.CreateService<IChartService>().SaveChartSet(ChartSets);
                var req = new SaveChartSetRequest
                {
                    ChartSet = ChartSets
                };
                var res = _chartService.SaveChartSet(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                LogHelper.Error("simpleButton1_Click" + ex);
            }
        }
    }
}