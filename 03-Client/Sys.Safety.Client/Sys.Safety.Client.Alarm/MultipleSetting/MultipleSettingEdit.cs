using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Multiplesetting;
using Sys.Safety.Request.Setting;
using Sys.Safety.ServiceContract;
using Sys.Safety.ClientFramework.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Alarm.MultipleSetting
{
    public partial class MultipleSettingEdit : XtraForm
    {
        JC_MultiplesettingInfo multiplesettingInfo = null;
        List<Jc_DevInfo> deviceInfos = null;
        MultipleSetting parent = null;
        private IJC_MultiplesettingService multiplesettingService = ServiceFactory.Create<IJC_MultiplesettingService>();
        
        public MultipleSettingEdit(List<Jc_DevInfo> _deviceInfos, MultipleSetting _multipleSetting)
        {
            deviceInfos = _deviceInfos;
            parent = _multipleSetting;
            InitializeComponent();
        }
        public MultipleSettingEdit(JC_MultiplesettingInfo _multiplesettingInfo, List<Jc_DevInfo> _deviceInfos, MultipleSetting _multipleSetting)
        {
            deviceInfos = _deviceInfos;
            parent = _multipleSetting;
            multiplesettingInfo = _multiplesettingInfo;
            InitializeComponent();
        }

        private void MultipleSettingEdit_Load(object sender, EventArgs e)
        {
            try
            {
                //加载设备类型
                if (deviceInfos != null)
                {
                    //DataTable dt = Basic.Framework.Common.ObjectConverter.ToDataTable<Jc_DevInfo>(deviceInfos);
                    //DevSelect.DataSource = dt;
                    //DevSelect.DisplayMember = "Name";
                    //DevSelect.ValueMember = "devid";
                    //DevSelect.Refresh();
                    CheckedListBoxItem[] items = new CheckedListBoxItem[deviceInfos.Count];
                    for (int i = 0; i < deviceInfos.Count; i++)
                    {
                        //if (multiplesettingInfo != null)
                        //{
                        //}
                        items[i] = new CheckedListBoxItem(deviceInfos[i].Devid.ToString(), deviceInfos[i].Name, CheckState.Unchecked);
                    }
                    DevSelect.Items.AddRange(items);
                }
                if (multiplesettingInfo != null)
                {
                    //加载设备类型选择
                    for (int j = 0; j < DevSelect.Items.Count; j++)
                    {
                        if (DevSelect.Items[j].Value.ToString() == multiplesettingInfo.Devid.ToString())
                        {
                            DevSelect.SetItemChecked(j, true);
                        }
                    }
                    DevSelect.Enabled = false;
                    //加载倍数关系
                    string[] multipleTextArray = multiplesettingInfo.MultipleText.Split('|');
                    for (int i = 0; i < multipleTextArray.Length; i++)
                    {
                        string[] tempsonmultipleTextArr = multipleTextArray[i].Split(',');
                        switch (i + 1)
                        {
                            case 1:
                                WeekAverageMin1.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax1.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled1.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 2:
                                WeekAverageMin2.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax2.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled2.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 3:
                                WeekAverageMin3.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax3.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled3.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 4:
                                WeekAverageMin4.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax4.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled4.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 5:
                                WeekAverageMin5.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax5.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled5.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 6:
                                WeekAverageMin6.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax6.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled6.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 7:
                                WeekAverageMin7.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax7.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled7.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 8:
                                WeekAverageMin8.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax8.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled8.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 9:
                                WeekAverageMin9.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax9.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled9.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 10:
                                WeekAverageMin10.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax10.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled10.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                        }
                    }
                }
                else
                {
                    DevSelect.Enabled = true;
                    for (int j = 0; j < DevSelect.Items.Count; j++)
                    {
                        DevSelect.SetItemChecked(j, false);
                    }
                    for (int i = 0; i < 10; i++)//全部置成初始状态 
                    {
                        string[] tempsonmultipleTextArr = ("0,0,0|0,0,0|0,0,0|0,0,0|0,0,0|0,0,0|0,0,0|0,0,0|0,0,0|0,0,0").Split('|');
                        switch (i + 1)
                        {
                            case 1:
                                WeekAverageMin1.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax1.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled1.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 2:
                                WeekAverageMin2.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax2.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled2.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 3:
                                WeekAverageMin3.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax3.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled3.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 4:
                                WeekAverageMin4.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax4.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled4.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 5:
                                WeekAverageMin5.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax5.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled5.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 6:
                                WeekAverageMin6.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax6.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled6.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 7:
                                WeekAverageMin7.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax7.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled7.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 8:
                                WeekAverageMin8.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax8.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled8.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 9:
                                WeekAverageMin9.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax9.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled9.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                            case 10:
                                WeekAverageMin10.Value = decimal.Parse(tempsonmultipleTextArr[0]);
                                WeekAverageMax10.Value = decimal.Parse(tempsonmultipleTextArr[1]);
                                Multipled10.Value = decimal.Parse(tempsonmultipleTextArr[2]);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //获取所有条件
                string saveMultipleTextString = "";
                saveMultipleTextString += WeekAverageMin1.Value + ",";
                saveMultipleTextString += WeekAverageMax1.Value + ",";
                saveMultipleTextString += Multipled1.Value + "|";
                saveMultipleTextString += WeekAverageMin2.Value + ",";
                saveMultipleTextString += WeekAverageMax2.Value + ",";
                saveMultipleTextString += Multipled2.Value + "|";
                saveMultipleTextString += WeekAverageMin3.Value + ",";
                saveMultipleTextString += WeekAverageMax3.Value + ",";
                saveMultipleTextString += Multipled3.Value + "|";
                saveMultipleTextString += WeekAverageMin4.Value + ",";
                saveMultipleTextString += WeekAverageMax4.Value + ",";
                saveMultipleTextString += Multipled4.Value + "|";
                saveMultipleTextString += WeekAverageMin5.Value + ",";
                saveMultipleTextString += WeekAverageMax5.Value + ",";
                saveMultipleTextString += Multipled5.Value + "|";
                saveMultipleTextString += WeekAverageMin6.Value + ",";
                saveMultipleTextString += WeekAverageMax6.Value + ",";
                saveMultipleTextString += Multipled6.Value + "|";
                saveMultipleTextString += WeekAverageMin7.Value + ",";
                saveMultipleTextString += WeekAverageMax7.Value + ",";
                saveMultipleTextString += Multipled7.Value + "|";
                saveMultipleTextString += WeekAverageMin8.Value + ",";
                saveMultipleTextString += WeekAverageMax8.Value + ",";
                saveMultipleTextString += Multipled8.Value + "|";
                saveMultipleTextString += WeekAverageMin9.Value + ",";
                saveMultipleTextString += WeekAverageMax9.Value + ",";
                saveMultipleTextString += Multipled9.Value + "|";
                saveMultipleTextString += WeekAverageMin10.Value + ",";
                saveMultipleTextString += WeekAverageMax10.Value + ",";
                saveMultipleTextString += Multipled10.Value;
                if (multiplesettingInfo == null)//新增
                {
                    for (int j = 0; j < DevSelect.Items.Count; j++)
                    {
                        if (DevSelect.Items[j].CheckState == CheckState.Checked)
                        {
                            JC_MultiplesettingInfo saveMultiplesettingInfo = new JC_MultiplesettingInfo();
                            saveMultiplesettingInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                            saveMultiplesettingInfo.Devid = DevSelect.Items[j].Value.ToString();
                            saveMultiplesettingInfo.MultipleText = saveMultipleTextString;
                            JC_MultiplesettingAddRequest multiplesettingrequest = new JC_MultiplesettingAddRequest();
                            multiplesettingrequest.MultiplesettingInfo = saveMultiplesettingInfo;
                            multiplesettingService.AddMultiplesetting(multiplesettingrequest);
                        }
                    }
                }
                else//修改
                {
                    JC_MultiplesettingInfo saveMultiplesettingInfo = new JC_MultiplesettingInfo();
                    saveMultiplesettingInfo.Id = multiplesettingInfo.Id;
                    saveMultiplesettingInfo.Devid = multiplesettingInfo.Devid;
                    saveMultiplesettingInfo.MultipleText = saveMultipleTextString;
                    JC_MultiplesettingUpdateRequest multiplesettingrequest = new JC_MultiplesettingUpdateRequest();
                    multiplesettingrequest.MultiplesettingInfo = saveMultiplesettingInfo;
                    multiplesettingService.UpdateMultiplesetting(multiplesettingrequest);
                }
                
                parent.LoadData();
                this.Close();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
    }
}
