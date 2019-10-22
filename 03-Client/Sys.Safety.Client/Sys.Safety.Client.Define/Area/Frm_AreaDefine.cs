using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.Area;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Client.GraphDefine;
using Sys.Safety.Client.Define.Sensor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Define.Area
{
    public partial class Frm_AreaDefine : XtraForm
    {
        IDeviceDefineService deviceDefineService;
        
        IAreaService areaService;
        List<Jc_DevInfo> devItems;
        DataTable areaRuleList = new DataTable();
        AreaInfo area = null;

        List<R_ArearestrictedpersonInfo> restrictedpersonInfoList = new List<R_ArearestrictedpersonInfo>();

        /// <summary>
        /// 0新增，1修改
        /// </summary>
        int type = 0;

        string position = "";
        string areaid = "";


        /// <summary>
        /// 编辑、修改区域规则
        /// </summary>
        /// <param name="_obj"> obj = 区域编码</param>
        /// <param name="_type">0新增，1修改</param>
        public Frm_AreaDefine(string _areaId, int _type)
        {
            InitializeComponent();
            areaid = _areaId;
            type = _type;
        }
        private void Frm_AreaDefine_Load(object sender, EventArgs e)
        {
            deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
            
            areaService = ServiceFactory.Create<IAreaService>();

            areaRuleList = new DataTable();
            areaRuleList.Columns.Add("id");
            areaRuleList.Columns.Add("devid");
            areaRuleList.Columns.Add("devname");
            areaRuleList.Columns.Add("deviceCount");
            areaRuleList.Columns.Add("maxVal");
            areaRuleList.Columns.Add("minVal");
            gridControl1.DataSource = areaRuleList;

            //加载人员报警设置  20171128

            //加载默认报警时间
            AlarmTime.Text = "08:00";
            //加载默认报警类型
            AlarmType.SelectedIndex = 0;
            //加载默认超员报警人数
            RatedPersonCount.Value = 0;


            if (type == 1)
            {
                area = GetAreaByAreaID(areaid);
                if (area != null)
                {
                    position = area.AreaBound;
                    txt_areaPosition.Text = position;
                    txt_areaPosition.ReadOnly = true;
                    txt_areaName.Text = area.Areaname;

                    List<AreaRuleInfo> areaRuleItems = area.AreaRuleInfoList;
                    Jc_DevInfo devInfo;
                    //加载已有规则信息
                    devItems = deviceDefineService.GetAllDeviceDefineCache().Data.OrderBy(a => a.Devid).ToList();
                    foreach (AreaRuleInfo item in areaRuleItems)
                    {
                        devInfo = devItems.FirstOrDefault(a => a.Devid.ToString() == item.Devid);
                        AddGridData(item.Devid, devInfo == null ? "未知" : devInfo.Name,
                            item.DeviceCount.ToString(),
                            item.MaxValue.ToString(),
                            item.MinValue.ToString());
                    }

                    //加载已有区域人员报警信息  20171128
                    if (area.Bz1 != null)
                    {
                        AlarmTime.Text = area.Bz1.ToString();
                    }
                    if (area.Bz2 != null)
                    {
                        RatedPersonCount.Value = int.Parse(area.Bz2);
                    }
                    if (area.RestrictedpersonInfoList != null)
                    {
                        if (area.RestrictedpersonInfoList.Count > 0)
                        {
                            restrictedpersonInfoList = area.RestrictedpersonInfoList;
                            AlarmType.SelectedIndex = area.RestrictedpersonInfoList[0].Type;
                        }
                    }
                }
            }
            else
            {
                area = null;
            }


            LoadDev();




        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            AddAreaAndRulesToDB();
            this.DialogResult = DialogResult.OK;
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            int tempInt = 0;
            int.TryParse(txt_count.Text.Trim(), out tempInt);
            if (tempInt <= 0)
            {
                XtraMessageBox.Show("传感器最少安装数量需大于0！");
                return;
            }
            string devid = cmb_dev.Text.Split('【')[0];
            string devname = cmb_dev.Text.Split('【')[1].Substring(0, cmb_dev.Text.Split('【')[1].Length - 1);

            bool isExsit = false;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "devid").ToString() == devid)
                {
                    isExsit = true;    
                    break;
                }
            }
            if (isExsit)
            {
                MessageBox.Show("该传感器已设置参数，请先删除原设置后再进行添加。");
            }
            else
            {
                AddGridData(devid,
                    devname,
                    txt_count.Text.Trim(),
                    txt_maxVal.Text.Trim(),
                    txt_minVal.Text.Trim());
            }
        }

        private void AddGridData(string devid, string devname, string deviceCount, string maxVal, string minVal)
        {
            //gridView1.AddNewRow();
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "id", gridView1.RowCount);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "devid", devid);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "devname", devname);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "deviceCount", deviceCount);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "maxVal", maxVal);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "minVal", minVal);
            object[] obj = new object[areaRuleList.Columns.Count];
            obj[0] = gridView1.RowCount + 1;
            obj[1] = devid;
            obj[2] = devname;
            obj[3] = deviceCount;
            obj[4] = string.IsNullOrEmpty(maxVal) ? "0" : maxVal;
            obj[5] = string.IsNullOrEmpty(minVal) ? "0" : minVal;
            areaRuleList.Rows.Add(obj);
        }

        private void LoadDev()
        {
            var result = deviceDefineService.GetAllDeviceDefineCache();
            if (result.Data != null && result.IsSuccess)
            {
                devItems = result.Data.FindAll(a => a.Type == 1 || a.Type == 2);
                devItems = devItems.OrderBy(a => a.Type).ThenBy(a => long.Parse(a.Devid)).ToList();
                for (int i = 0; i < devItems.Count; i++)
                {
                    cmb_dev.Items.Add(devItems[i].Devid + "【" + devItems[i].Name + "】");
                }
            }
        }

        /// <summary>
        /// 添加或修改区域及其规则入数据库
        /// </summary>
        private void AddAreaAndRulesToDB()
        {
            AreaInfo temparea = null;
            if (type == 0)
            {
                //新增
                temparea = GetAreaInfoItem(txt_areaName.Text.Trim(), txt_areaPosition.Text);                
            }
            else
            {
                //修改
                temparea = GetAreaByAreaID(areaid);                
            }
            List<AreaRuleInfo> areaRuleInfoList = new List<AreaRuleInfo>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {

                AreaRuleInfo areaRuleItem = GetAreaRuleItem(Convert.ToInt32(gridView1.GetRowCellValue(i, "devid").ToString())
                    , Convert.ToInt32(gridView1.GetRowCellValue(i, "deviceCount").ToString())
                    , float.Parse(gridView1.GetRowCellValue(i, "maxVal").ToString())
                    , float.Parse(gridView1.GetRowCellValue(i, "minVal").ToString())
                    , temparea.Areaid);

                areaRuleInfoList.Add(areaRuleItem);
            }
            if (type == 0)
            {                
                //增加区域设备定义限制信息
                temparea.AreaRuleInfoList = areaRuleInfoList;

                AreaAddRequest areaAddRequest = new AreaAddRequest();
                areaAddRequest.AreaInfo = temparea;
                areaService.AddArea(areaAddRequest);
            }
            else
            {
                //修改                
                temparea.AreaBound = txt_areaPosition.Text;
                temparea.Areaname = txt_areaName.Text.Trim();
                temparea.CreateUpdateTime = DateTime.Now;
               
                //增加人员报警信息  20171129
                temparea.Bz1 = AlarmTime.Text;
                temparea.Bz2 = RatedPersonCount.Value.ToString();
                temparea.RestrictedpersonInfoList = restrictedpersonInfoList;

                //增加区域设备定义限制信息
                temparea.AreaRuleInfoList = areaRuleInfoList;

                AreaUpdateRequest areaUpdateRequest = new AreaUpdateRequest();
                areaUpdateRequest.AreaInfo = temparea;
                areaService.UpdateArea(areaUpdateRequest);
            }

            //DeleteAreaRules(temparea.Areaid);
            //AreaRuleInfo areaRuleItem;
            //AreaRuleAddRequest areaRuleAddRequest;
            //for (int i = 0; i < gridView1.RowCount; i++)
            //{

            //    areaRuleItem = GetAreaRuleItem(Convert.ToInt32(gridView1.GetRowCellValue(i, "devid").ToString())
            //        , Convert.ToInt32(gridView1.GetRowCellValue(i, "deviceCount").ToString())
            //        , float.Parse(gridView1.GetRowCellValue(i, "maxVal").ToString())
            //        , float.Parse(gridView1.GetRowCellValue(i, "minVal").ToString())
            //        , temparea.Areaid);

            //    areaRuleAddRequest = new AreaRuleAddRequest();
            //    areaRuleAddRequest.AreaRuleInfo = areaRuleItem;
            //    areaRuleService.AddAreaRule(areaRuleAddRequest);
            //}

        }
        ///// <summary>
        ///// 删除该区域下已定义的规则
        ///// </summary>
        ///// <param name="areaID"></param>
        //private void DeleteAreaRules(string areaID)
        //{
        //    //删除现有规则
        //    AreaRuleDeleteRequest areaRuleDeleteRequest = new AreaRuleDeleteRequest();
        //    areaRuleDeleteRequest.Id = areaID;
        //    areaRuleService.DeleteAreaRuleByAreaID(areaRuleDeleteRequest);
        //}

        private AreaInfo GetAreaInfoItem(string areaName, string areaBound)
        {
            AreaInfo areaInfo = new AreaInfo();

            areaInfo.Areaid = IdHelper.CreateLongId().ToString();
            areaInfo.Areaname = areaName;
            areaInfo.Activity = "1";
            areaInfo.AreaBound = areaBound;
            areaInfo.Loc = "";
            areaInfo.Parentloc = "";
            areaInfo.CreateUpdateTime = DateTime.Now;

            //增加人员报警信息  20171129
            areaInfo.Bz1 = AlarmTime.Text;
            areaInfo.Bz2 = RatedPersonCount.Value.ToString();
            areaInfo.RestrictedpersonInfoList = restrictedpersonInfoList;

            return areaInfo;
        }
        private AreaRuleInfo GetAreaRuleItem(int devid, int deviceCount, float maxVal, float minVal, string areaID)
        {
            AreaRuleInfo areaRule = new AreaRuleInfo();

            areaRule.RuleID = IdHelper.CreateLongId().ToString();
            areaRule.Areaid = areaID;
            areaRule.Devid = devid.ToString();
            areaRule.DeviceCount = deviceCount;
            areaRule.MaxValue = maxVal;
            areaRule.MinValue = minVal;

            return areaRule;
        }

        ///// <summary>
        ///// 获取该区域已定义的规则
        ///// </summary>
        ///// <param name="areaID"></param>
        ///// <returns></returns>
        //private List<AreaRuleInfo> GetAreaRulesFromDB(string areaID)
        //{
        //    List<AreaRuleInfo> items = new List<AreaRuleInfo>();

        //    GetAreaRuleListByAreaIDRequest getAreaRuleListByAreaIDRequest = new GetAreaRuleListByAreaIDRequest();
        //    getAreaRuleListByAreaIDRequest.areaID = areaID;
        //    var result = areaRuleService.GetAreaRuleListByAreaID(getAreaRuleListByAreaIDRequest);
        //    if (result.Data != null && result.IsSuccess)
        //    {
        //        items = result.Data;
        //    }

        //    return items;
        //}

        /// <summary>
        /// 根据区域ID获取区域信息
        /// </summary>
        /// <param name="areaID"></param>
        /// <returns></returns>
        private AreaInfo GetAreaByAreaID(string areaID)
        {
            AreaInfo area = null;

            var result = areaService.GetAllAreaCache(new AreaCacheGetAllRequest());
            if (result.Data != null && result.IsSuccess)
            {
                area = result.Data.FirstOrDefault(a => a.Areaid == areaID);
            }

            return area;
        }
        /// <summary>
        /// 区域坐标拾取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AreaGraphDrawing areaGraphDrawing = new AreaGraphDrawing(txt_areaPosition.Text);
            areaGraphDrawing.ShowDialog();
            if (areaGraphDrawing.DialogResult == DialogResult.OK)
            {
                this.txt_areaPosition.Text = areaGraphDrawing.Jsonstr;
            }
        }
        /// <summary>
        /// 删除设备定义关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    int selectedHandle;
                    selectedHandle = this.gridView1.GetSelectedRows()[0];
                    //this.gridView1.GetRowCellValue(selectedHandle, "id").ToString();
                    for (int i = areaRuleList.Rows.Count - 1; i >= 0; i--)
                    {
                        if (this.gridView1.GetRowCellValue(selectedHandle, "id") != null)
                        {
                            if (areaRuleList.Rows[i]["id"].ToString() == this.gridView1.GetRowCellValue(selectedHandle, "id").ToString())
                            {
                                areaRuleList.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 区域限制进入、禁止进入人员选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            List<R_ArearestrictedpersonInfo> tempSel = restrictedpersonInfoList.FindAll(a => a.Type == AlarmType.SelectedIndex);
            var formSelPerson = new PersonSetForm(tempSel);
            var res = formSelPerson.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            restrictedpersonInfoList.Clear();
            foreach (R_PersoninfInfo person in formSelPerson.SelectPerson)
            {
                R_ArearestrictedpersonInfo tempR_RestrictedpersonInfo = new R_ArearestrictedpersonInfo();
                tempR_RestrictedpersonInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                if (area == null)//如果是识别器新增
                {
                    tempR_RestrictedpersonInfo.AreaId = "";//先不赋值PointID，在保存时创建了才能赋值  20171122
                }
                else
                {
                    tempR_RestrictedpersonInfo.AreaId = area.Areaid;
                }
                tempR_RestrictedpersonInfo.Type = AlarmType.SelectedIndex;
                tempR_RestrictedpersonInfo.Yid = person.Yid;
                restrictedpersonInfoList.Add(tempR_RestrictedpersonInfo);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddAreaAndRulesToDB();
            this.DialogResult = DialogResult.OK;
        }


    }
}
