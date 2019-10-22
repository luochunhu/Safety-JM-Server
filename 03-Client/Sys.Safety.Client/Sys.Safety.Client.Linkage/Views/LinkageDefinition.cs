using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.SysEmergencyLinkage;
using Sys.Safety.Client.Linkage.Handlers;
using Sys.Safety.Client.Linkage.Views;
using ObjectConverter = Basic.Framework.Common.ObjectConverter;

namespace Sys.Safety.Client.Linkage.Views
{
    public partial class LinkageDefinition : XtraForm
    {
        /// <summary>查询设备性质
        /// 
        /// </summary>
        private readonly List<int> _propertyIds = new List<int>()
        {
            1,
            2
        };

        /// <summary>数据状态
        /// 
        /// </summary>
        private List<EnumcodeInfo> _dataState;

        /// <summary>大数据模板
        /// 
        /// </summary>
        private DataTable _dtLargedataAnalysisConfig = new DataTable();

        /// <summary>待选择项
        /// 
        /// </summary>
        private readonly DataTable _dtSelectItem = new DataTable();

        /// <summary>已选择主控测点id
        /// 
        /// </summary>
        private List<string> _masterSelectedPoint = new List<string>();

        /// <summary>已选择主控设备类型id
        /// 
        /// </summary>
        private List<string> _masterSelectedEquipmentType = new List<string>();

        /// <summary>已选择主控区域id
        /// 
        /// </summary>
        private List<string> _masterSelectedArea = new List<string>();

        /// <summary>已选被控测点
        /// 
        /// </summary>
        private List<string> _passiveSelectedPoint = new List<string>();

        /// <summary>已选被控区域
        /// 
        /// </summary>
        private List<string> _passiveSelectedArea = new List<string>();

        /// <summary>已选被控人员
        /// 
        /// </summary>
        private List<string> _passiveSelectedperson = new List<string>();

        public LinkageDefinition()
        {
            InitializeComponent();

            MasterPoint.Checked = true;
            PassivePoint.Checked = true;
        }

        private void CorrelationDefinition_Load(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Columns.Add("Check", typeof(bool));
                _dtSelectItem.Columns.Add("Id");
                _dtSelectItem.Columns.Add("Text");

                _dataState = EnumHandler.GetTriggerDataState();
                GridControlDataState.DataSource = _dataState;

                var largedataAnalysisConfigInfo = BigDataHandle.GetAllLargedataAnalysisConfig();

                _dtLargedataAnalysisConfig = new DataTable();
                _dtLargedataAnalysisConfig.Columns.Add("Check", typeof(bool));
                _dtLargedataAnalysisConfig.Columns.Add("Id");
                _dtLargedataAnalysisConfig.Columns.Add("Name");
                foreach (var item in largedataAnalysisConfigInfo)
                {
                    var dr = _dtLargedataAnalysisConfig.NewRow();
                    dr["Check"] = item.Check;
                    dr["Id"] = item.Id;
                    dr["Name"] = item.Name;
                    _dtLargedataAnalysisConfig.Rows.Add(dr);
                }
                GridControlLargeData.DataSource = _dtLargedataAnalysisConfig;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MasterType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var button = (RadioButton)sender;
                if (!button.Checked)
                {
                    return;
                }

                if (button.Name == "MasterPoint")
                {
                    MasterPointSet.Enabled = true;
                    MasterEquTypeSet.Enabled = false;
                    MasterAreaSet.Enabled = false;
                }
                if (button.Name == "MasterEquType")
                {
                    MasterPointSet.Enabled = false;
                    MasterEquTypeSet.Enabled = true;
                    MasterAreaSet.Enabled = false;
                }
                if (button.Name == "MasterArea")
                {
                    MasterPointSet.Enabled = false;
                    MasterEquTypeSet.Enabled = false;
                    MasterAreaSet.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassiveType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var button = (RadioButton)sender;
                if (!button.Checked)
                {
                    return;
                }

                if (button.Name == "PassivePoint")
                {
                    PassivePointSet.Enabled = true;
                    PassiveAreaSet.Enabled = false;
                }
                if (button.Name == "PassiveArea")
                {
                    PassivePointSet.Enabled = false;
                    PassiveAreaSet.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void repositoryItemCheckEdit4_Click(object sender, EventArgs e)
        {
            try
            {
                var focRowHandle = GridViewLargeData.FocusedRowHandle;
                string nodeId = GridViewLargeData.GetRowCellValue(focRowHandle, "Id").ToString();
                for (int i = 0; i < _dtLargedataAnalysisConfig.Rows.Count; i++)
                {
                    if (_dtLargedataAnalysisConfig.Rows[i]["Id"].ToString() == nodeId)
                    {
                        _dtLargedataAnalysisConfig.Rows[i]["Check"] = true;
                    }
                    else
                    {
                        _dtLargedataAnalysisConfig.Rows[i]["Check"] = false;
                    }
                }
                GridControlLargeData.RefreshDataSource();
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MasterPointSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allPoint = PointHandler.GetMonitoringSystemPointByPropertyIds(_propertyIds);
                foreach (var item in allPoint)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.PointID;
                    row["Text"] = item.Wz + "（" + item.Point + "）";
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_masterSelectedPoint);
                var selectForm = new LinkageItemSelect("主控测点选择", "测点号", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _masterSelectedPoint = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MasterEquTypeSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allEquType = EquipmentTypeHandler.GetEquipmentTypeByPropertyIds(_propertyIds);
                foreach (var item in allEquType)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.Devid;
                    row["Text"] = item.Name;
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_masterSelectedEquipmentType);
                var selectForm = new LinkageItemSelect("主控设备类型选择", "设备类型名称", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _masterSelectedEquipmentType = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MasterAreaSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allArea = AreaHandler.GetAllArea();
                foreach (var item in allArea)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.Areaid;
                    row["Text"] = item.Areaname;
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_masterSelectedArea);
                var selectForm = new LinkageItemSelect("主控区域选择", "区域名称", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _masterSelectedArea = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassivePointSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allPoint = LinkageHandler.GetLinkagePassivePoint();
                foreach (var item in allPoint)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = item.Check;
                    row["Id"] = item.Id;
                    row["Text"] = item.Text;
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_passiveSelectedPoint);
                var selectForm = new LinkageItemSelect("被控测点选择", "测点名称", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _passiveSelectedPoint = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassiveAreaSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allArea = AreaHandler.GetAllArea();
                foreach (var item in allArea)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.Areaid;
                    row["Text"] = item.Areaname;
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_passiveSelectedArea);
                var selectForm = new LinkageItemSelect("被控区域选择", "区域名称", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _passiveSelectedArea = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PassivePersonSet_Click(object sender, EventArgs e)
        {
            try
            {
                _dtSelectItem.Rows.Clear();
                var allPerson = PersonHandler.GetAllPerson();
                foreach (var item in allPerson)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.Id;
                    row["Text"] = item.Name + "（" + item.Bh + "）";
                    _dtSelectItem.Rows.Add(row);
                }
                //_dtSelectItem.Select("1=1", "Text asc");
                var copyData = ObjectConverter.DeepCopy(_passiveSelectedperson);
                var selectForm = new LinkageItemSelect("人员选择", "人员名称", _dtSelectItem, copyData);
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }

                _passiveSelectedperson = ObjectConverter.DeepCopy(selectForm.SelectedIds);
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Affirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //DataRow[] selLargeDataModel = _dtLargedataAnalysisConfig.Select("Check=true");

                GridViewDataState.CloseEditor();
                GridViewLargeData.CloseEditor();

                var selTabName = MasterTab.SelectedTabPage.Name;        //tab名称

                //数据验证
                if (string.IsNullOrEmpty(LinkageName.Text))
                {
                    XtraMessageBox.Show("请数据联动名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (selTabName == "NormalLinkage")
                {
                    if (string.IsNullOrEmpty(Duration.Text))
                    {
                        XtraMessageBox.Show("请输入持续时间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var duration = Convert.ToInt32(Duration.Text);
                    if (duration < 5 || duration > 300)
                    {
                        XtraMessageBox.Show("持续时间取值范围为5-300！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var ifSelDataState = _dataState.Any(a => a.Check);
                    if (!ifSelDataState)
                    {
                        XtraMessageBox.Show("请选择触发数据状态！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    bool ifPass = false;
                    if (MasterPoint.Checked)
                    {
                        if (_masterSelectedPoint.Count != 0)
                        {
                            ifPass = true;
                        }
                    }
                    else if (MasterArea.Checked)
                    {
                        if (_masterSelectedArea.Count != 0)
                        {
                            ifPass = true;
                        }
                    }
                    else if (MasterEquType.Checked)
                    {
                        if (_masterSelectedEquipmentType.Count != 0)
                        {
                            ifPass = true;
                        }
                    }
                    if (!ifPass)
                    {
                        XtraMessageBox.Show("请设置主控项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    var selLargeDataModel = _dtLargedataAnalysisConfig.Select("Check=true");
                    //var dvLargedataAnalysisConfig = _dtLargedataAnalysisConfig.DefaultView;
                    //dvLargedataAnalysisConfig.RowFilter = "Check=true";
                    //var selLargeDataModel = dvLargedataAnalysisConfig.ToTable();
                    if (selLargeDataModel.Length == 0)
                    {
                        XtraMessageBox.Show("请设置主控项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                bool ifPassivePass = false;
                if (_passiveSelectedperson.Count != 0)
                {
                    ifPassivePass = true;
                }

                if (PassivePoint.Checked)
                {
                    if (_passiveSelectedPoint.Count != 0)
                    {
                        ifPassivePass = true;
                    }
                }
                else if (PassiveArea.Checked)
                {
                    if (_passiveSelectedArea.Count != 0)
                    {
                        ifPassivePass = true;
                    }
                }
                if (!ifPassivePass)
                {
                    XtraMessageBox.Show("请选择被控项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var req = new AddEmergencylinkageconfigMasterInfoPassiveInfoRequest()
                {
                    SysEmergencyLinkageInfo = new SysEmergencyLinkageInfo(),
                    EmergencyLinkageMasterAreaAssInfo = new List<EmergencyLinkageMasterAreaAssInfo>(),
                    EmergencyLinkageMasterDevTypeAssInfo = new List<EmergencyLinkageMasterDevTypeAssInfo>(),
                    EmergencyLinkageMasterPointAssInfo = new List<EmergencyLinkageMasterPointAssInfo>(),
                    EmergencyLinkageMasterTriDataStateAssInfo = new List<EmergencyLinkageMasterTriDataStateAssInfo>(),
                    EmergencyLinkagePassiveAreaAssInfo = new List<EmergencyLinkagePassiveAreaAssInfo>(),
                    EmergencyLinkagePassivePersonAssInfo = new List<EmergencyLinkagePassivePersonAssInfo>(),
                    EmergencyLinkagePassivePointAssInfo = new List<EmergencyLinkagePassivePointAssInfo>()
                };
                req.SysEmergencyLinkageInfo.Name = LinkageName.Text;

                ClientItem clientItem = new ClientItem();
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                {
                    clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                }
                req.SysEmergencyLinkageInfo.EditPerson = clientItem.UserName;

                if (selTabName == "NormalLinkage")      //普通联动
                {
                    req.SysEmergencyLinkageInfo.Duration = Convert.ToInt32(Duration.Text);
                    req.SysEmergencyLinkageInfo.Type = 1;

                    if (MasterPoint.Checked)        //主控测点
                    {
                        foreach (var item in _masterSelectedPoint)
                        {
                            var newItem = new EmergencyLinkageMasterPointAssInfo
                            {
                                PointId = item
                            };
                            req.EmergencyLinkageMasterPointAssInfo.Add(newItem);
                        }
                    }
                    else if (MasterEquType.Checked)     //主控设备类型
                    {
                        foreach (var item in _masterSelectedEquipmentType)
                        {
                            var newItem = new EmergencyLinkageMasterDevTypeAssInfo()
                            {
                                DevId = item
                            };
                            req.EmergencyLinkageMasterDevTypeAssInfo.Add(newItem);
                        }
                    }
                    else if (MasterArea.Checked)        //主控区域
                    {
                        foreach (var item in _masterSelectedArea)
                        {
                            var newItem = new EmergencyLinkageMasterAreaAssInfo()
                            {
                                AreaId = item
                            };
                            req.EmergencyLinkageMasterAreaAssInfo.Add(newItem);
                        }
                    }

                    //触发数据类型
                    foreach (var item in _dataState)
                    {
                        if (!item.Check)
                        {
                            continue;
                        }

                        var newItem = new EmergencyLinkageMasterTriDataStateAssInfo()
                        {
                            DataStateId = item.LngEnumValue.ToString()
                        };
                        req.EmergencyLinkageMasterTriDataStateAssInfo.Add(newItem);
                    }

                    req.SysEmergencyLinkageInfo.MasterModelId = "0";
                }
                else        //大数据分析联动
                {
                    req.SysEmergencyLinkageInfo.Type = 2;
                    req.SysEmergencyLinkageInfo.Duration = 0;

                    var selItem = _dtLargedataAnalysisConfig.Select("Check=true");
                    req.SysEmergencyLinkageInfo.MasterModelId = selItem[0]["Id"].ToString();
                    //req.SysEmergencyLinkageInfo.MasterModelId = selLargeDataModel[0]["Id"].ToString();
                }

                if (PassivePoint.Checked)       //被控测点
                {
                    foreach (var item in _passiveSelectedPoint)
                    {
                        var newItem = new EmergencyLinkagePassivePointAssInfo()
                        {
                            PointId = item
                        };
                        req.EmergencyLinkagePassivePointAssInfo.Add(newItem);
                    }
                }
                else if (PassiveArea.Checked)       //被控区域
                {
                    foreach (var item in _passiveSelectedArea)
                    {
                        var newItem = new EmergencyLinkagePassiveAreaAssInfo()
                        {
                            AreaId = item
                        };
                        req.EmergencyLinkagePassiveAreaAssInfo.Add(newItem);
                    }
                }

                //被控人员
                foreach (var item in _passiveSelectedperson)
                {
                    var newItem = new EmergencyLinkagePassivePersonAssInfo()
                    {
                        PersonId = item
                    };
                    req.EmergencyLinkagePassivePersonAssInfo.Add(newItem);
                }

                LinkageHandler.AddEmergencyLinkage(req);
                XtraMessageBox.Show("操作成功！\r\n如果主控测点未在图形界面定义，请进行相关操作，否则无法在图形界面显示。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                XtraMessageBox.Show(exception.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barHeaderItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

    }
}
