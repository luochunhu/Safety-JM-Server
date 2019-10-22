using DevExpress.XtraEditors;
using Sys.Safety.DataContract.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.Client.Linkage.Handlers;

namespace Sys.Safety.Client.Linkage.Views
{
    public partial class LinkageDetailAll : XtraForm
    {
        public LinkageDetailAll(GetSysEmergencyLinkageListAndStatisticsResponse info)
        {
            InitializeComponent();

            //生成主控信息
            List<Kvp> masterInfo = new List<Kvp>();     //主控信息
            string masterText = "";
            if (info.Type == 1)
            {
                if (info.MasterPointNum != "0")
                {
                    masterText = "测点信息";
                    var points = LinkageHandler.GetMasterPointInfoByAssId(info.MasterPointAssId);
                    foreach (var item in points)
                    {
                        if (item == null)//加判断，防止报错  20180821
                        {
                            continue;
                        }
                        var newItem = new Kvp()
                        {
                            Id = item.PointID,
                            Text = item.Point + "（" + item.Wz + "）"
                        };
                        masterInfo.Add(newItem);
                    }
                }
                if (info.MasterAreaNum != "0")
                {
                    masterText = "区域信息";
                    var points = LinkageHandler.GetMasterAreaInfoByAssId(info.MasterAreaAssId);
                    foreach (var item in points)
                    {
                        if (item == null)//加判断，防止报错  20180821
                        {
                            continue;
                        }
                        var newItem = new Kvp()
                        {
                            Id = item.Areaid,
                            Text = item.Areaname
                        };
                        masterInfo.Add(newItem);
                    }
                }
                if (info.MasterDevTypeNum != "0")
                {
                    masterText = "设备类型信息";
                    var points = LinkageHandler.GetMasterEquTypeInfoByAssId(info.MasterDevTypeAssId);
                    foreach (var item in points)
                    {
                        if (item == null)//加判断，防止报错  20180821
                        {
                            continue;
                        }
                        var newItem = new Kvp()
                        {
                            Id = item.Devid,
                            Text = item.Name
                        };
                        masterInfo.Add(newItem);
                    }
                }
            }
            else
            {
                masterText = "大数据分析模型";
                var newItem = new Kvp()
                {
                    Id = "0",
                    Text = info.MasterModelName
                };
                masterInfo.Add(newItem);
            }

            //生成主控触发状态信息
            List<Kvp> masterTriDataStateInfo = new List<Kvp>();     //主控触发数据状态信息
            var masterTriDataState = LinkageHandler.GetMasterTriDataStateByAssId(info.MasterTriDataStateAssId);
            foreach (var item in masterTriDataState)
            {
                var newItem = new Kvp()
                {
                    Id = item.LngEnumValue.ToString(),
                    Text = item.StrEnumDisplay
                };
                masterTriDataStateInfo.Add(newItem);
            }

            //生成被控人员信息
            List<Kvp> passivePersonInfo = new List<Kvp>();     //主控触发数据状态信息
            var passivePerson = LinkageHandler.GetPassivePersonByAssId(info.PassivePersonAssId);
            foreach (var item in passivePerson)
            {
                if (item == null)//加判断，防止报错  20180821
                {
                    continue;
                }
                var newItem = new Kvp()
                {
                    Id = item.Id,
                    Text = item.Name
                };
                passivePersonInfo.Add(newItem);
            }

            //生成被控其他信息
            string passiveOtherText = "";
            List<Kvp> passiveOtherInfo = new List<Kvp>();     //主控触发数据状态信息
            if (info.PassivePointNum != "0")
            {
                passiveOtherText = "测点信息";
                var points = LinkageHandler.GetPassivePointInfoByAssId(info.PassivePointAssId);
                foreach (var item in points)
                {
                    var newItem = new Kvp();
                    if (item != null)//判断，如果子系统测点已删除，则不显示(之前要报错)  20180408
                    {
                        newItem = new Kvp()
                        {
                            Id = item.Id,
                            Text = item.Text
                        };
                        passiveOtherInfo.Add(newItem);
                    }
                }
            }
            if (info.PassiveAreaNum != "0")
            {
                passiveOtherText = "区域信息";
                var points = LinkageHandler.GetPassiveAreaInfoByAssId(info.PassiveAreaAssId);
                foreach (var item in points)
                {
                    if (item == null)//加判断，防止报错  20180821
                    {
                        continue;
                    }
                    var newItem = new Kvp()
                    {
                        Id = item.Areaid,
                        Text = item.Areaname
                    };
                    passiveOtherInfo.Add(newItem);
                }
            }

            gridColumnMasterInfo.Caption = masterText == "" ? "信息" : masterText;
            gridColumnPassiveOther.Caption = passiveOtherText == "" ? "信息" : passiveOtherText;
            gridControlMasterInfo.DataSource = masterInfo;
            gridControlMasterTirDataState.DataSource = masterTriDataStateInfo;
            gridControlPassivePerson.DataSource = passivePersonInfo;
            gridControlPassiveOtherInfo.DataSource = passiveOtherInfo;
        }

        private void GoBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}
