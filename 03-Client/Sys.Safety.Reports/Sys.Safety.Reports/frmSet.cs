using System;
using System.Collections.Generic;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Sys.Safety.Reports.Model;

namespace Sys.Safety.Reports
{
    public partial class frmSet : XtraForm
    {
        private readonly ListExModel Model = new ListExModel();

        public frmSet()
        {
            InitializeComponent();
        }


        private void frmSet_Load(object sender, EventArgs e)
        {
            LoadData();
        }


        private void LoadData()
        {
            var blnFirstOpenLoadData = TypeUtil.ToBool(RequestUtil.GetParameterValue("blnFirstOpenLoadData"));
            var blnListFreCondition = TypeUtil.ToBool(RequestUtil.GetParameterValue("blnListFreCondition"));
            chkblnFirstOpenLoadData.Checked = blnFirstOpenLoadData;
            chkblnListFreCondition.Checked = blnListFreCondition;
        }

        private void tlbSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var strsql = "";
                if (RequestUtil.GetParameterValue("blnFirstOpenLoadData") == "")
                    strsql =
                        "insert into BFT_Setting(strType,strKey,strKeyCHs,strValue,Creator,LastUpdateDate) values('列表报表','blnFirstOpenLoadData','打开列表加载数据','" +
                        chkblnFirstOpenLoadData.Checked + "',0,'" + DateTime.Now + "')";
                else
                    strsql = "update BFT_Setting set strValue='" + chkblnFirstOpenLoadData.Checked +
                             "' where strKey='blnFirstOpenLoadData'";
                Model.ExecuteSQL(strsql);
                if (RequestUtil.GetParameterValue("blnListFreCondition") == "")
                    strsql =
                        "insert into BFT_Setting(strType,strKey,strKeyCHs,strValue,Creator,LastUpdateDate) values('列表报表','blnListFreCondition','打开列表显示常用条件','" +
                        chkblnListFreCondition.Checked + "',0,'" + DateTime.Now + "')";
                else
                    strsql = "update BFT_Setting set strValue='" + chkblnListFreCondition.Checked +
                             "' where strKey='blnListFreCondition'";
                Model.ExecuteSQL(strsql);
                //IDictionary<string, string> dic =
                //    ClientContext.Current.GetContext("CustomerSetting") as Dictionary<string, string>;
                IDictionary<string, string> dic =
                    Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, string>>(Basic.Framework.Data.PlatRuntime.Items["CustomerSetting"].ToString());

                if (dic != null)
                {
                    if (dic.ContainsKey("blnFirstOpenLoadData"))
                        dic["blnFirstOpenLoadData"] = chkblnFirstOpenLoadData.Checked.ToString();
                    else
                        dic.Add("blnFirstOpenLoadData", chkblnFirstOpenLoadData.Checked.ToString());
                    if (dic.ContainsKey("blnListFreCondition"))
                        dic["blnListFreCondition"] = chkblnListFreCondition.Checked.ToString();
                    else
                        dic.Add("blnListFreCondition", chkblnListFreCondition.Checked.ToString());
                    //ClientContext.Current.SetContext("CustomerSetting", dic);

                    if (!Basic.Framework.Data.PlatRuntime.Items.ContainsKey("CustomerSetting"))
                    {
                        Basic.Framework.Data.PlatRuntime.Items.Add("CustomerSetting", null);
                    }

                    // 20170623
                    //Basic.Framework.Data.PlatRuntime.Items["CustomerSetting"] = dic;
                    Basic.Framework.Data.PlatRuntime.Items["CustomerSetting"] = JSONHelper.ToJSONString(dic);

                }

                MessageShowUtil.ShowInfo("保存成功");
            }
            catch (Exception ex)
            {
                MessageShowUtil.ShowErrow(ex);
            }
        }

        private void tlbClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }
    }
}