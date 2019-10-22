using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
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
using Sys.Safety.Request.KJ_Addresstype;
using Sys.Safety.Request.KJ_Addresstyperule;

namespace Sys.Safety.Client.Define
{
    public partial class Frm_AddressTypeDefine : XtraForm
    {
        IDeviceDefineService deviceDefineService;

        IKJ_AddresstypeService addressTypeService;
        IKJ_AddresstyperuleService addresstyperuleService;
        List<Jc_DevInfo> devItems;
        DataTable AddressTypeRuleList = new DataTable();
        KJ_AddresstypeInfo AddressType = null;

        List<KJ_AddresstyperuleInfo> addresstyperuleSInfoList = new List<KJ_AddresstyperuleInfo>();

        /// <summary>
        /// 0新增，1修改
        /// </summary>
        int type = 0;

        string position = "";
        string AddressTypeid = "";


        /// <summary>
        /// 编辑、修改区域规则
        /// </summary>
        /// <param name="_obj"> obj = 区域编码</param>
        /// <param name="_type">0新增，1修改</param>
        public Frm_AddressTypeDefine(string _AddressTypeId, int _type)
        {
            InitializeComponent();
            AddressTypeid = _AddressTypeId;
            type = _type;
        }
        private void Frm_AddressTypeDefine_Load(object sender, EventArgs e)
        {
            deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();

            addressTypeService = ServiceFactory.Create<IKJ_AddresstypeService>();

            addresstyperuleService = ServiceFactory.Create<IKJ_AddresstyperuleService>();

            AddressTypeRuleList = new DataTable();
            AddressTypeRuleList.Columns.Add("ID");
            AddressTypeRuleList.Columns.Add("Addresstypeid");
            AddressTypeRuleList.Columns.Add("Devid");
            AddressTypeRuleList.Columns.Add("Devname");
            AddressTypeRuleList.Columns.Add("UpAlarmValue");
            AddressTypeRuleList.Columns.Add("UpPoweroffValue");
            AddressTypeRuleList.Columns.Add("LowAlarmValue");
            AddressTypeRuleList.Columns.Add("LowPoweroffValue");
            gridControl1.DataSource = AddressTypeRuleList;

            //加载人员报警设置  20171128



            if (type == 1)
            {
                AddressType = GetAddressTypeByAddressTypeID(AddressTypeid);
                if (AddressType != null)
                {

                    txt_AddressTypeName.Text = AddressType.Addresstypename;

                    List<KJ_AddresstyperuleInfo> AddressTypeRuleItems = addresstyperuleService.GetKJ_AddresstyperuleListByAddressTypeId(new Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest()
                    {
                        Id = AddressTypeid
                    }).Data;
                    Jc_DevInfo devInfo;
                    //加载已有规则信息
                    devItems = deviceDefineService.GetAllDeviceDefineCache().Data.OrderBy(a => a.Devid).ToList();
                    foreach (KJ_AddresstyperuleInfo item in AddressTypeRuleItems)
                    {
                        devInfo = devItems.FirstOrDefault(a => a.Devid.ToString() == item.Devid);
                        AddGridData(item.ID, item.Addresstypeid, item.Devid, devInfo == null ? "未知" : devInfo.Name,
                           Math.Round(item.UpAlarmLowValue, 2) + "~" + Math.Round(item.UpAlarmHighValue, 2),
                            Math.Round(item.UpPoweroffLowValue, 2) + "~" + Math.Round(item.UpPoweroffHighValue, 2),
                            Math.Round(item.LowAlarmLowValue, 2) + "~" + Math.Round(item.LowAlarmHighValue, 2),
                            Math.Round(item.LowPoweroffLowValue, 2) + "~" + Math.Round(item.LowPoweroffHighValue, 2));
                    }


                }
            }
            else
            {
                AddressType = null;
            }


            LoadDev();




        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            AddAddressTypeAndRulesToDB();
            this.DialogResult = DialogResult.OK;
        }
        private void btn_add_Click(object sender, EventArgs e)
        {
            //int tempInt = 0;
            //int.TryParse(txt_count.Text.Trim(), out tempInt);
            //if (tempInt <= 0)
            //{
            //    XtraMessageBox.Show("传感器最少安装数量需大于0！");
            //    return;
            //}
            double upAlarmLowVal = 0;
            double upAlarmHighVal = 0;
            double upPoweroffLowVal = 0;
            double upPoweroffHighVal = 0;
            double lowAlarmLowVal = 0;
            double lowAlarmHighVal = 0;
            double lowPoweroffLowVal = 0;
            double lowPoweroffHighVal = 0;
            double.TryParse(txt_UpAlarmLowVal.Text.Trim(), out upAlarmLowVal);
            double.TryParse(txt_UpAlarmHighVal.Text.Trim(), out upAlarmHighVal);
            double.TryParse(txt_UpPoweroffLowVal.Text.Trim(), out upPoweroffLowVal);
            double.TryParse(txt_UpPoweroffHighVal.Text.Trim(), out upPoweroffHighVal);
            double.TryParse(txt_LowAlarmLowVal.Text.Trim(), out lowAlarmLowVal);
            double.TryParse(txt_LowAlarmHighVal.Text.Trim(), out lowAlarmHighVal);
            double.TryParse(txt_LowPoweroffLowVal.Text.Trim(), out lowPoweroffLowVal);
            double.TryParse(txt_LowPoweroffHighVal.Text.Trim(), out lowPoweroffHighVal);


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
                AddGridData(Basic.Framework.Common.IdHelper.CreateLongId().ToString(), AddressTypeid, devid,
                    devname,
                    upAlarmLowVal + "~" + upAlarmHighVal,
                     upPoweroffLowVal + "~" + upPoweroffHighVal,
                     lowAlarmLowVal + "~" + lowAlarmHighVal,
                      lowPoweroffLowVal + "~" + lowPoweroffHighVal);
            }
        }

        private void AddGridData(string id, string addresstypeId, string devid, string devname, string upAlarmValue, string upPoweroffVlaue, string lowAlarmValue, string lowPoweroffVlaue)
        {
            //gridView1.AddNewRow();
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "id", gridView1.RowCount);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "devid", devid);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "devname", devname);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "deviceCount", deviceCount);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "maxVal", maxVal);
            //gridView1.SetRowCellValue(gridView1.RowCount - 1, "minVal", minVal);
            object[] obj = new object[AddressTypeRuleList.Columns.Count];
            obj[0] = id;
            obj[1] = addresstypeId;
            obj[2] = devid;
            obj[3] = devname;
            obj[4] = upAlarmValue;
            obj[5] = upPoweroffVlaue;
            obj[6] = lowAlarmValue;
            obj[7] = lowPoweroffVlaue;
            AddressTypeRuleList.Rows.Add(obj);
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
        private void AddAddressTypeAndRulesToDB()
        {
            KJ_AddresstypeInfo tempAddressType = null;
            if (type == 0)
            {
                //新增
                tempAddressType = GetAddressTypeInfoItem(txt_AddressTypeName.Text.Trim());
            }
            else
            {
                //修改
                tempAddressType = GetAddressTypeByAddressTypeID(AddressTypeid);
            }
            List<KJ_AddresstyperuleInfo> AddressTypeRuleInfoList = new List<KJ_AddresstyperuleInfo>();
            for (int i = 0; i < gridView1.RowCount; i++)
            {

                KJ_AddresstyperuleInfo AddressTypeRuleItem = GetAddressTypeRuleItem(
                    gridView1.GetRowCellValue(i, "ID").ToString()
                    , gridView1.GetRowCellValue(i, "Addresstypeid").ToString()
                    , gridView1.GetRowCellValue(i, "Devid").ToString()
                    , gridView1.GetRowCellValue(i, "UpAlarmValue").ToString()
                    , gridView1.GetRowCellValue(i, "UpPoweroffValue").ToString()
                     , gridView1.GetRowCellValue(i, "LowAlarmValue").ToString()
                     , gridView1.GetRowCellValue(i, "LowPoweroffValue").ToString());

                AddressTypeRuleInfoList.Add(AddressTypeRuleItem);
            }
            if (type == 0)
            {
                KJ_AddresstypeAddRequest AddressTypeAddRequest = new KJ_AddresstypeAddRequest();
                AddressTypeAddRequest.KJ_AddresstypeInfo = tempAddressType;
                addressTypeService.AddKJ_Addresstype(AddressTypeAddRequest);

                foreach (KJ_AddresstyperuleInfo temp in AddressTypeRuleInfoList)
                {
                    //增加区域设备定义限制信息              
                    KJ_AddresstyperuleAddRequest AddressTypeUpdateRequest = new KJ_AddresstyperuleAddRequest();
                    AddressTypeUpdateRequest.KJ_AddresstyperuleInfo = temp;
                    addresstyperuleService.AddKJ_Addresstyperule(AddressTypeUpdateRequest);
                }
            }
            else
            {
                //修改
                tempAddressType.Addresstypename = txt_AddressTypeName.Text.Trim();
                tempAddressType.Createupdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                KJ_AddresstypeUpdateRequest AddressTypeUpdateRequest = new KJ_AddresstypeUpdateRequest();
                AddressTypeUpdateRequest.KJ_AddresstypeInfo = tempAddressType;
                addressTypeService.UpdateKJ_Addresstype(AddressTypeUpdateRequest);

                //先删除，再添加 todo
                addresstyperuleService.DeleteKJ_AddresstyperuleByAddressTypeId(new KJ_AddresstyperuleDeleteByAddressTypeIdRequest()
                {
                    AddressTypeId = AddressTypeid
                });
                foreach (KJ_AddresstyperuleInfo temp in AddressTypeRuleInfoList)
                {
                    //增加区域设备定义限制信息              
                    KJ_AddresstyperuleAddRequest AddressTypeUpdateRequest1 = new KJ_AddresstyperuleAddRequest();
                    AddressTypeUpdateRequest1.KJ_AddresstyperuleInfo = temp;
                    addresstyperuleService.AddKJ_Addresstyperule(AddressTypeUpdateRequest1);
                }
            }

            //DeleteAddressTypeRules(tempAddressType.AddressTypeid);
            //AddressTypeRuleInfo AddressTypeRuleItem;
            //AddressTypeRuleAddRequest AddressTypeRuleAddRequest;
            //for (int i = 0; i < gridView1.RowCount; i++)
            //{

            //    AddressTypeRuleItem = GetAddressTypeRuleItem(Convert.ToInt32(gridView1.GetRowCellValue(i, "devid").ToString())
            //        , Convert.ToInt32(gridView1.GetRowCellValue(i, "deviceCount").ToString())
            //        , float.Parse(gridView1.GetRowCellValue(i, "maxVal").ToString())
            //        , float.Parse(gridView1.GetRowCellValue(i, "minVal").ToString())
            //        , tempAddressType.AddressTypeid);

            //    AddressTypeRuleAddRequest = new AddressTypeRuleAddRequest();
            //    AddressTypeRuleAddRequest.AddressTypeRuleInfo = AddressTypeRuleItem;
            //    AddressTypeRuleService.AddAddressTypeRule(AddressTypeRuleAddRequest);
            //}

        }
        ///// <summary>
        ///// 删除该区域下已定义的规则
        ///// </summary>
        ///// <param name="AddressTypeID"></param>
        //private void DeleteAddressTypeRules(string AddressTypeID)
        //{
        //    //删除现有规则
        //    AddressTypeRuleDeleteRequest AddressTypeRuleDeleteRequest = new AddressTypeRuleDeleteRequest();
        //    AddressTypeRuleDeleteRequest.Id = AddressTypeID;
        //    AddressTypeRuleService.DeleteAddressTypeRuleByAddressTypeID(AddressTypeRuleDeleteRequest);
        //}

        private KJ_AddresstypeInfo GetAddressTypeInfoItem(string AddressTypeName)
        {
            KJ_AddresstypeInfo AddressTypeInfo = new KJ_AddresstypeInfo();

            AddressTypeInfo.ID = IdHelper.CreateLongId().ToString();
            AddressTypeid = AddressTypeInfo.ID;
            AddressTypeInfo.Addresstypename = AddressTypeName;
            AddressTypeInfo.Addresstypedesc = "";
            AddressTypeInfo.Createupdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            AddressTypeInfo.Bz1 = "";
            AddressTypeInfo.Bz2 = "";
            AddressTypeInfo.Bz3 = "";
            return AddressTypeInfo;
        }
        private KJ_AddresstyperuleInfo GetAddressTypeRuleItem(string id, string addresstypeid, string devid, string upAlarmValue, string upPoweroffValue, string lowAlarmValue, string lowPoweroffValue)
        {
            KJ_AddresstyperuleInfo AddressTypeRule = new KJ_AddresstyperuleInfo();

            AddressTypeRule.ID = IdHelper.CreateLongId().ToString();
            AddressTypeRule.Addresstypeid = AddressTypeid;
            AddressTypeRule.Devid = devid;
            AddressTypeRule.UpAlarmLowValue = double.Parse(upAlarmValue.Split('~')[0]);
            AddressTypeRule.UpAlarmHighValue = double.Parse(upAlarmValue.Split('~')[1]);
            AddressTypeRule.UpPoweroffLowValue = double.Parse(upPoweroffValue.Split('~')[0]);
            AddressTypeRule.UpPoweroffHighValue = double.Parse(upPoweroffValue.Split('~')[1]);
            AddressTypeRule.LowAlarmLowValue = double.Parse(lowAlarmValue.Split('~')[0]);
            AddressTypeRule.LowAlarmHighValue = double.Parse(lowAlarmValue.Split('~')[1]);
            AddressTypeRule.LowPoweroffLowValue = double.Parse(lowPoweroffValue.Split('~')[0]);
            AddressTypeRule.LowPoweroffHighValue = double.Parse(lowPoweroffValue.Split('~')[1]);
            return AddressTypeRule;
        }

        ///// <summary>
        ///// 获取该区域已定义的规则
        ///// </summary>
        ///// <param name="AddressTypeID"></param>
        ///// <returns></returns>
        //private List<AddressTypeRuleInfo> GetAddressTypeRulesFromDB(string AddressTypeID)
        //{
        //    List<AddressTypeRuleInfo> items = new List<AddressTypeRuleInfo>();

        //    GetAddressTypeRuleListByAddressTypeIDRequest getAddressTypeRuleListByAddressTypeIDRequest = new GetAddressTypeRuleListByAddressTypeIDRequest();
        //    getAddressTypeRuleListByAddressTypeIDRequest.AddressTypeID = AddressTypeID;
        //    var result = AddressTypeRuleService.GetAddressTypeRuleListByAddressTypeID(getAddressTypeRuleListByAddressTypeIDRequest);
        //    if (result.Data != null && result.IsSuccess)
        //    {
        //        items = result.Data;
        //    }

        //    return items;
        //}

        /// <summary>
        /// 根据区域ID获取区域信息
        /// </summary>
        /// <param name="AddressTypeID"></param>
        /// <returns></returns>
        private KJ_AddresstypeInfo GetAddressTypeByAddressTypeID(string AddressTypeID)
        {
            KJ_AddresstypeInfo AddressType = null;

            var result = addressTypeService.GetKJ_AddresstypeById(new KJ_AddresstypeGetRequest()
            {
                Id = AddressTypeID
            });
            if (result.Data != null && result.IsSuccess)
            {
                AddressType = result.Data;
            }

            return AddressType;
        }
        /// <summary>
        /// 区域坐标拾取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //AddressTypeGraphDrawing AddressTypeGraphDrawing = new AddressTypeGraphDrawing(txt_AddressTypePosition.Text);
            //AddressTypeGraphDrawing.ShowDialog();
            //if (AddressTypeGraphDrawing.DialogResult == DialogResult.OK)
            //{
            //    this.txt_AddressTypePosition.Text = AddressTypeGraphDrawing.Jsonstr;
            //}
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
                    for (int i = AddressTypeRuleList.Rows.Count - 1; i >= 0; i--)
                    {
                        if (this.gridView1.GetRowCellValue(selectedHandle, "ID") != null)
                        {
                            if (AddressTypeRuleList.Rows[i]["ID"].ToString() == this.gridView1.GetRowCellValue(selectedHandle, "ID").ToString())
                            {
                                AddressTypeRuleList.Rows.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }


        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddAddressTypeAndRulesToDB();
            this.DialogResult = DialogResult.OK;
        }


    }
}
