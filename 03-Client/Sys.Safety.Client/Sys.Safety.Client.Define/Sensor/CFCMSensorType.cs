using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;
using Basic.Framework.Logging;
using Sys.Safety.Client.Define.Model;
using System.Threading;
using Sys.Safety.Enums;

namespace Sys.Safety.Client.Define.Sensor
{
    public partial class CFCMSensorType : XtraForm
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CFCMSensorType()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CFCMSensorType(string code, string DevPtyID, string DevClassID)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
            if (!string.IsNullOrEmpty(code))
            {
                _DevID = Convert.ToInt32(code);
            }
            if (!string.IsNullOrEmpty(DevClassID))
            {
                _DevClassID = Convert.ToInt32(DevClassID);
            }
            if (!string.IsNullOrEmpty(DevPtyID))
            {
                _DevPtyID = Convert.ToInt32(DevPtyID);
            }
        }
        private void CuContorlType_Load(object sender, EventArgs e)
        {
            isFristLoad = true;
            LoadPretermitInf(); //加载默认信息
            LoadBasicInf();//加载基础信息
            isFristLoad = false;
        }

        #region =========================窗体变量===========================
        /// <summary>
        /// 设备类型名称
        /// </summary>
        private int _DevID;
        /// <summary>
        /// 设备种类
        /// </summary>
        private int _DevClassID;
        /// <summary>
        /// 设备性质
        /// </summary>
        private int _DevPtyID;
        /// <summary>
        /// 是否第一次加载
        /// </summary>
        private bool isFristLoad = true;
        #endregion

        #region =========================操作事件===========================
        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {

                if (Sensorverify())
                {
                    Jc_DevInfo temp = new Jc_DevInfo();
                    Dictionary<int, EnumcodeInfo> tempEnumCode;
                    temp.Devid = CtxbDevID.Text;//DevID
                    temp.Type = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))); //DevPrppertyID
                    tempEnumCode = Model.DEVServiceModel.QueryDevPropertisCache();

                    temp.Bz6 = cmb_zbq.Text;
                    temp.Bz5 = int.Parse(cmb_yxq.Text);
                    //新增库存数量  20170330
                    temp.Bz7 = Stock.Value.ToString();
                    // 20171123
                    temp.Sysid = (int)SystemName.EditValue;
                    //赋值多参数  20170505
                    temp.Bz8 = spinEdit1.Value.ToString();
                    temp.Bz9 = textEdit1.Text;
                    if (checkEdit1.Checked)
                    {
                        temp.Bz10 = "1";
                    }
                    else
                    {
                        temp.Bz10 = "0";
                    }

                    if (null != tempEnumCode)
                    {
                        if (tempEnumCode.ContainsKey(temp.Type))
                        {
                            temp.DevProperty = tempEnumCode[temp.Type].StrEnumDisplay;
                        }
                    }
                    temp.Name = CtxbDevName.Text;
                    if (!string.IsNullOrEmpty(CcmbDevClass.Text))
                    {
                        temp.Bz3 = Convert.ToInt32(CcmbDevClass.Text.Substring(0, CcmbDevClass.Text.IndexOf('.'))); //DevClassID
                        tempEnumCode = Model.DEVServiceModel.QueryDevClasiessCache();
                        if (null != tempEnumCode)
                        {
                            if (tempEnumCode.ContainsKey(temp.Bz3))
                            {
                                temp.DevClass = tempEnumCode[temp.Bz3].StrEnumDisplay;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(CcmbDevID.Text))
                    {
                        temp.Bz4 = Convert.ToInt32(CcmbDevID.Text.Substring(0, CcmbDevID.Text.IndexOf('.'))); //DevTypeID
                        tempEnumCode = Model.DEVServiceModel.QueryDevMoelsCache();
                        if (null != tempEnumCode)
                        {
                            if (tempEnumCode.ContainsKey(temp.Bz4))
                            {
                                temp.DevModel = tempEnumCode[temp.Bz4].StrEnumDisplay;
                            }
                        }
                    }
                    if (this.CpDocument.Controls.Count > 0)
                    {
                        if (!((CuBaseType)(CpDocument.Controls[0])).ConfirmInf(temp))//扩展属性
                        {
                            return;
                        }
                    }

                    if (_DevID > 0)
                    {
                        //更新    
                        Jc_DevInfo ExistDev = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID.ToString());

                        if (ExistDev != temp)
                        {
                            bool isUpdateDefThresholdValueFlag = false;
                            bool isupdateDerailValueFlag = false;
                            bool isupdateControlValueFlag = false;
                            if (ExistDev.Type == 1)
                            {
                                if (ExistDev.Z1 != temp.Z1
                                    || ExistDev.Z2 != temp.Z2
                                     || ExistDev.Z3 != temp.Z3
                                     || ExistDev.Z4 != temp.Z4
                                     || ExistDev.Z5 != temp.Z5
                                     || ExistDev.Z6 != temp.Z6
                                     || ExistDev.Z7 != temp.Z7
                                     || ExistDev.Z8 != temp.Z8)
                                {//判断，如果是模拟量修改了阈值，则提示用户是否将当前类型下已定义设备全部应用  20171220
                                    if (XtraMessageBox.Show("当前设备类型修改了阈值，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isUpdateDefThresholdValueFlag = true;
                                    }
                                }
                            }
                            else if (ExistDev.Type == 2)
                            {
                                //开关量设备类型状态颜色修改
                                if (ExistDev.Color1 != temp.Color1
                                    || ExistDev.Color2 != temp.Color2
                                    || ExistDev.Color3 != temp.Color3)
                                {
                                    if (XtraMessageBox.Show("当前设备类型修改了状态显示颜色，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isupdateDerailValueFlag = true;
                                    }
                                }
                            }
                            else if (ExistDev.Type == 3)
                            {
                                //控制量设备类型状态颜色修改
                                if (ExistDev.Color1 != temp.Color1
                                    || ExistDev.Color2 != temp.Color2)
                                {
                                    if (XtraMessageBox.Show("当前设备类型修改了状态显示颜色，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isupdateControlValueFlag = true;
                                    }
                                }
                            }
                            if (ExistDev != null)
                            {
                                temp.ID = ExistDev.ID;
                            }
                            temp.InfoState = InfoState.Modified;
                            //Model.DEVServiceModel.AddJC_DEVCache(temp);
                            try
                            {
                                try
                                {
                                    if (!Model.DEVServiceModel.UpdateJC_DEVCache(temp))
                                    {
                                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            OperateLogHelper.InsertOperateLog(2, "更新传感器类型【" + temp.Name + "】", "");


                            List<Jc_DefInfo> updatePoints = Model.DEFServiceModel.QueryPointByDevIDCache(temp.Devid.ToString());

                            if (updatePoints != null)
                            {
                                if (updatePoints.Count > 0)
                                {
                                    foreach (var item in updatePoints)
                                    {
                                        item.DevName = temp.Name;//DevID
                                        item.DevModelID = temp.Bz4;//DevTypeID
                                        item.DevModel = temp.DevModel;
                                        item.DevClassID = temp.Bz3;//DevClassID
                                        item.DevClass = temp.DevClass;
                                        item.DevPropertyID = temp.Type;//DevPropertyID
                                        item.DevProperty = temp.DevProperty;

                                        //阈值修改处理  20171220
                                        if (isUpdateDefThresholdValueFlag)
                                        {
                                            item.Z1 = temp.Z1;
                                            item.Z2 = temp.Z2;
                                            item.Z3 = temp.Z3;
                                            item.Z4 = temp.Z4;
                                            item.Z5 = temp.Z5;
                                            item.Z6 = temp.Z6;
                                            item.Z7 = temp.Z7;
                                            item.Z8 = temp.Z8;
                                        }
                                        //开关量控制量状态修改
                                        if (isupdateDerailValueFlag)
                                        {
                                            item.Bz9 = temp.Color1.ToString();
                                            item.Bz10 = temp.Color2.ToString();
                                            item.Bz11 = temp.Color3.ToString();
                                        }
                                        //控制量状态修改
                                        if (isupdateControlValueFlag)
                                        {
                                            item.Bz9 = temp.Color1.ToString();
                                            item.Bz10 = temp.Color2.ToString();
                                        }

                                        item.InfoState = InfoState.Modified;
                                    }
                                    try
                                    {
                                        Model.DEFServiceModel.UpdateDEFsCache(updatePoints);//将相应的测点告知数据采集
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CtxbDevName.Text))//根据设备类型名称，判断是否有重复定义  20170627
                        {
                            List<Jc_DevInfo> tempDevice = Model.DEVServiceModel.QueryDevsCache().FindAll(a => a.Name == CtxbDevName.Text);
                            if (tempDevice != null && tempDevice.Count > 0)
                            {
                                MessageBox.Show("该设备已定义!", "提示");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择设备型号!", "提示");
                            return;
                        }

                        //新增
                        temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        temp.InfoState = InfoState.AddNew;
                        try
                        {
                            if (!Model.DEVServiceModel.AddJC_DEVCache(temp))
                            {
                                XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                            XtraMessageBox.Show("保存失败：" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        OperateLogHelper.InsertOperateLog(2, "新增传感器类型【" + temp.Name + "】", "");
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败：" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Error("确认保存【Cbtn_Confirm_Click】", ex);
            }
        }
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close(); //
        }
        private void Cbtn_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CtxbDevID.Text))
                {
                    return;
                }
                IList<Jc_DefInfo> TempPoints = Model.DEFServiceModel.QueryPointByDevIDCache(CtxbDevID.Text);
                if (null == TempPoints)
                {
                    return;
                }
                if (TempPoints.Count == 0)
                {
                    return;
                }
                if (CpDocument.Controls == null)
                {
                    return;
                }
                if (CpDocument.Controls.Count == 0)
                {
                    return;
                }
                List<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
                CuAnalogType tempType = (CuAnalogType)(CpDocument.Controls[0]);
                Jc_DefInfo TempPoint;
                foreach (var item in TempPoints)
                {
                    item.Z1 = float.Parse(tempType.CtxbHiPreAlarm.Text);
                    item.Z2 = float.Parse(tempType.CtxbHiAlarm.Text);
                    item.Z3 = float.Parse(tempType.CtxbHiPowerOff.Text);
                    item.Z4 = float.Parse(tempType.CtxbHiPowerBack.Text);
                    item.Z5 = float.Parse(tempType.CtxbLowPreAlarm.Text);
                    item.Z6 = float.Parse(tempType.CtxbLowAlarm.Text);
                    item.Z7 = float.Parse(tempType.CtxbLowPowerOff.Text);
                    item.Z8 = float.Parse(tempType.CtxbLowPowerBack.Text);

                    TempPoint = null;
                    TempPoint = Model.DEFServiceModel.QueryPointByCodeCache(item.Point);
                    if (TempPoint == null)
                    {
                        item.Ssz = "";
                        item.State = 46;
                        item.DataState = 46;
                        item.InfoState = InfoState.Modified;
                        UpdatePoints.Add(item);
                    }
                    else if (TempPoint != item)
                    {
                        item.Ssz = TempPoint.Ssz;//xuzp20151030
                        item.State = TempPoint.State; //xuzp20151030
                        item.DataState = TempPoint.DataState; //xuzp20151030
                        item.InfoState = InfoState.Modified;
                        UpdatePoints.Add(item);
                    }
                }
                if (UpdatePoints.Count > 0)
                {
                    try
                    {
                        Model.DEFServiceModel.UpdateDEFsCache(UpdatePoints);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        private void CbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CtxbDevID.Text) || string.IsNullOrEmpty(CtxbDevName.Text))
                {
                    XtraMessageBox.Show("当前未处于编辑状态，不能进行删除操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //判断当前设备类型是否定义测点  20180816
                List<Jc_DefInfo> tempDefList = Model.DEFServiceModel.QueryPointByDevIDCache(CtxbDevID.Text);
                if (tempDefList.Count > 0)
                {
                    XtraMessageBox.Show("当前设备类型已定义设备，请先删除设备定义，再删除设备类型定义！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复,且历史数据关联了此设备的数据无法查询,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(CtxbDevID.Text) && !string.IsNullOrEmpty(CtxbDevName.Text))
                    {
                        DeleteDevType(CtxbDevID.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
            //加延时  20170721
            Thread.Sleep(1000);
            this.Close();
        }
        #endregion

        #region =========================业务函数===========================

        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadBasicInf()
        {
            try
            {
                if (_DevID > 0)
                {

                    Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID.ToString());
                    if (null != temp)
                    {
                        this.CtxbDevName.Text = temp.Name; //设备类型
                        this.CtxbDevID.Text = temp.Devid.ToString();//设备编号
                        if (temp.Bz3 > 0)
                        {
                            var DevClasiess = Model.DEVServiceModel.QueryDevClasiessCache();
                            if (DevClasiess.ContainsKey(temp.Bz3))
                            {
                                this.CcmbDevClass.Text = temp.Bz3.ToString() + "." + DevClasiess[temp.Bz3].StrEnumDisplay;//设备种类
                            }
                        }
                        var DevPropertis = Model.DEVServiceModel.QueryDevPropertisCache();
                        if (DevPropertis.ContainsKey(temp.Type))
                        {
                            this.CcmbDvProPerty.Text = temp.Type.ToString() + "." + DevPropertis[temp.Type].StrEnumDisplay;//设备性质
                        }

                        this.CcmbDvProPerty.Enabled = false; //对于已经存在的设备类型不允许修改设备性质
                        if (temp.Bz4 > 0)
                        {
                            var DevMoels = Model.DEVServiceModel.QueryDevMoelsCache();
                            if (DevMoels.ContainsKey(temp.Bz4))
                            {
                                this.CcmbDevID.Text = temp.Bz4.ToString() + "." + DevMoels[temp.Bz4].StrEnumDisplay;//设备种类
                            }
                            ////修改时，如果已存在设备型号,不允许修改设备型号和设备种类 
                            //CcmbDevID.Enabled = false;
                            //CcmbDevClass.Enabled = false;
                        }

                        cmb_zbq.Text = temp.Bz6;
                        cmb_yxq.Text = temp.Bz5.ToString();
                        //库存数量  20170330
                        if (string.IsNullOrEmpty(temp.Bz7))
                        {
                            Stock.Value = 0;
                        }
                        else
                        {
                            int tempint = 0;
                            int.TryParse(temp.Bz7, out tempint);
                            Stock.Value = tempint;
                        }
                        //赋值多参数  20170505
                        decimal tempBz8int = 0;
                        decimal.TryParse(temp.Bz8, out tempBz8int);
                        if (tempBz8int == 0)
                        {
                            tempBz8int = 1;//默认值1个参数
                        }
                        spinEdit1.Value = tempBz8int;
                        textEdit1.Text = temp.Bz9;
                        if (temp.Bz10 == "1")
                        {
                            checkEdit1.Checked = true;
                        }
                        else
                        {
                            checkEdit1.Checked = false;
                        }

                        // 20171123
                        if (temp.Sysid == 0)
                        {
                            SystemName.EditValue = (int)SystemEnum.Security;
                        }
                        else
                        {
                            SystemName.EditValue = temp.Sysid;
                        }
                    }
                }
                else
                {
                    CtxbDevID.Text = (Model.DEVServiceModel.GetMaxDevID() + 1).ToString();
                    Dictionary<int, EnumcodeInfo> temp;
                    //新增设备类型
                    if (_DevPtyID > 0)
                    {
                        temp = Model.DEVServiceModel.QueryDevPropertisCache();
                        this.CcmbDvProPerty.SelectedItem = _DevPtyID + "." + temp[_DevPtyID].StrEnumDisplay;
                    }
                    if (_DevClassID > 0)
                    {
                        temp = Model.DEVServiceModel.QueryDevClasiessCache();
                        this.CcmbDevClass.SelectedItem = _DevClassID + "." + temp[_DevClassID].StrEnumDisplay;
                    }
                }
                if (_DevPtyID == 3)
                { //控制量可设置自动绑定馈电开停  20170621
                    checkEdit1.Enabled = true;
                }
                else
                {
                    checkEdit1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("确认保存【Cbtn_Confirm_Click】", ex);
            }
        }
        /// <summary>
        /// 加载默认的初始信息
        /// </summary>
        private void LoadPretermitInf()
        {
            try
            {
                //  20171123
                //绑定系统名称
                var systemName = EnumService.GetEnum(21);
                SystemName.Properties.DataSource = systemName;
                SystemName.Properties.NullText = "未选择";

                Dictionary<int, EnumcodeInfo> temp;
                temp = Model.DEVServiceModel.QueryDevPropertisCache();
                foreach (var item in temp.Values)
                {
                    if (item.LngEnumValue == 0)
                    {
                        continue;
                    }
                    this.CcmbDvProPerty.Properties.Items.Add(item.LngEnumValue + "." + item.StrEnumDisplay); //增加设备性质
                }

                temp = Model.DEVServiceModel.QueryDevClasiessCache();
                foreach (var item in temp.Values)
                {
                    this.CcmbDevClass.Properties.Items.Add(item.LngEnumValue + "." + item.StrEnumDisplay); //增加设备种类
                }

                this.CcmbDevID.Properties.Items.Add("");//xuzp20151229

                temp = Model.DEVServiceModel.QueryDevMoelsCache();
                foreach (var item in temp.Values)
                {
                    this.CcmbDevID.Properties.Items.Add(item.LngEnumValue + "." + item.StrEnumDisplay); //增加设备型号
                }
                //质保期，有效期默认值设置  20170716
                cmb_zbq.Text = "1";
                cmb_yxq.Text = "5";
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载默认的初始信息【LoadPretermitInf】", ex);
            }
        }
        ///<summary>
        /// 验证有效性
        /// </summary>
        /// <returns></returns>
        private bool Sensorverify()
        {
            bool ret = false;
            //  20171123
            var systemName = SystemName.EditValue;
            if (systemName == null)
            {
                XtraMessageBox.Show("请输入系统名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbDevName.Text))
            {
                XtraMessageBox.Show("请填写设备型号别名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CtxbDevName.Text))
            {
                XtraMessageBox.Show("设备型号别名中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CtxbDevName.Text.Length > 20)
            {
                XtraMessageBox.Show("设备设备型号别名长度不能超过20个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbDevID.Text))
            {
                XtraMessageBox.Show("请填写设备类型ID", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(cmb_zbq.Text))
            {
                XtraMessageBox.Show("请填写保质期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsInt(cmb_zbq.Text))
            {
                XtraMessageBox.Show("保质期为数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(cmb_yxq.Text))
            {
                XtraMessageBox.Show("请填写有效期", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (!Basic.Framework.Common.ValidationHelper.IsInt(cmb_yxq.Text))
            {
                XtraMessageBox.Show("有效期为数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            //判断多参数设置  20170505
            string[] MultChanel = new string[0];
            if (!string.IsNullOrEmpty(textEdit1.Text))
            {
                MultChanel = textEdit1.Text.Split('|');
            }
            if (MultChanel.Length != spinEdit1.Value - 1)
            {
                XtraMessageBox.Show("多参数副通道个数与多参数个数不匹配，副通道个数等于多参数个数减1", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            //验证控制量如果勾选了自动绑定馈电开停，副通道中必须选择开关量设备 
            bool isBindKglPoint = false;
            List<Jc_DevInfo> allDev = DEVServiceModel.QueryDevsCache();
            if (checkEdit1.Checked)
            {
                foreach (string devid in MultChanel)
                {
                    Jc_DevInfo tempdev = allDev.Find(a => a.Devid == devid);
                    if (tempdev.Type == 2)
                    {
                        isBindKglPoint = true;
                        break;
                    }
                }
                if (!isBindKglPoint)
                {
                    XtraMessageBox.Show("当前控制量设备副通道中没有开停设备，不能自动绑定馈电开停设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return ret;
                }
            }
            //if (string.IsNullOrEmpty(CcmbDvProPerty.Text))
            //{
            //    XtraMessageBox.Show("请选择设备性质", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return ret;
            //}
            ret = true;
            return ret;
        }

        /// <summary>
        /// 删除设备类型
        /// </summary>
        private void DeleteDevType(string DevID)
        {
            Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID);
            if (temp == null)
            {
                return;
            }
            if (GetDevDefined(DevID))
            {
                XtraMessageBox.Show("该设备类型下存在定义测点，不能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            temp.InfoState = InfoState.Delete;
            try
            {
                Model.DEVServiceModel.DelJC_DEVCache(temp);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OperateLogHelper.InsertOperateLog(2, "删除设备类型【" + temp.Name + "】", "");
        }

        private bool GetDevDefined(string DevID)
        {
            bool rvalue = false;

            IList<Jc_DefInfo> PointsExist;
            PointsExist = Model.DEFServiceModel.QueryPointByDevIDCache(DevID);
            if (null != PointsExist)
            {
                if (PointsExist.Count > 0)
                {
                    rvalue = true;
                }
            }

            return rvalue;
        }
        #endregion

        #region =========================触发事件===========================
        /// <summary>
        /// 设备性质改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbDvProPerty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //改变设备类型后引发设备种类的改变
                Dictionary<int, EnumcodeInfo> temp = Model.DEVServiceModel.QueryDevClassByDevpropertID(Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));
                CcmbDevClass.Properties.Items.Clear();
                foreach (var item in temp.Values)
                {
                    CcmbDevClass.Properties.Items.Add(item.LngEnumValue.ToString() + "." + item.StrEnumDisplay);
                }
                //改变属性集内容
                //CpDocument.Controls.Clear();
                //重写释放控件的方法，直接clear会导致句柄资源一直增加  20180422
                while (CpDocument.Controls.Count > 0)
                {
                    if (CpDocument.Controls[0] != null)
                        CpDocument.Controls[0].Dispose();
                }

                CuBaseType SensroTyeptemp = new CuBaseType();
                SensroTyeptemp = Model.DevAdapter.DevTypeAdapter(_DevID, Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))));//测点属性集  
                CpDocument.Controls.Add(SensroTyeptemp);//添加设备属性集
                SensroTyeptemp.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                LogHelper.Error("设备性质改变事件【CcmbDvProPerty_SelectedIndexChanged】", ex);
            }
        }

        /// <summary>
        /// 设备类型改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CcmbDevClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error("设备类型改变事件【CcmbDvProPerty_SelectedIndexChanged】", ex);
            }
        }
        #endregion

        private void CcmbDevID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CcmbDevID.Text) && CcmbDevID.Text.Contains("."))
            {
                CtxbDevName.Text = CcmbDevID.Text.Split('.')[1];
            }
        }
        /// <summary>
        /// 多参数副通道选择  20170505
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //进行是否定义验证  20171110
                if (GetDevDefined(CtxbDevID.Text))
                {
                    XtraMessageBox.Show("该设备类型下存在定义测点，不能修改多参数信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string DvProPertyId = CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'));
                MultiSelect MultiSel = new MultiSelect(this, (int)spinEdit1.Value, textEdit1.Text, DvProPertyId);
                MultiSel.ShowDialog();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //进行是否定义验证  20171110
            if (GetDevDefined(CtxbDevID.Text))
            {
                XtraMessageBox.Show("该设备类型下存在定义测点，不能修改多参数信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            textEdit1.Text = "";
        }

        private void spinEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!isFristLoad)
            {//如果不是首次加载，则进行是否定义验证  20171110
                if (GetDevDefined(CtxbDevID.Text))
                {
                    XtraMessageBox.Show("该设备类型下存在定义测点，不能修改多参数信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (Sensorverify())
                {
                    Jc_DevInfo temp = new Jc_DevInfo();
                    Dictionary<int, EnumcodeInfo> tempEnumCode;
                    temp.Devid = CtxbDevID.Text;//DevID
                    temp.Type = Convert.ToInt32(CcmbDvProPerty.Text.Substring(0, CcmbDvProPerty.Text.IndexOf('.'))); //DevPrppertyID
                    tempEnumCode = Model.DEVServiceModel.QueryDevPropertisCache();

                    temp.Bz6 = cmb_zbq.Text;
                    temp.Bz5 = int.Parse(cmb_yxq.Text);
                    //新增库存数量  20170330
                    temp.Bz7 = Stock.Value.ToString();
                    // 20171123
                    temp.Sysid = (int)SystemName.EditValue;
                    //赋值多参数  20170505
                    temp.Bz8 = spinEdit1.Value.ToString();
                    temp.Bz9 = textEdit1.Text;
                    if (checkEdit1.Checked)
                    {
                        temp.Bz10 = "1";
                    }
                    else
                    {
                        temp.Bz10 = "0";
                    }

                    if (null != tempEnumCode)
                    {
                        if (tempEnumCode.ContainsKey(temp.Type))
                        {
                            temp.DevProperty = tempEnumCode[temp.Type].StrEnumDisplay;
                        }
                    }
                    temp.Name = CtxbDevName.Text;
                    if (!string.IsNullOrEmpty(CcmbDevClass.Text))
                    {
                        temp.Bz3 = Convert.ToInt32(CcmbDevClass.Text.Substring(0, CcmbDevClass.Text.IndexOf('.'))); //DevClassID
                        tempEnumCode = Model.DEVServiceModel.QueryDevClasiessCache();
                        if (null != tempEnumCode)
                        {
                            if (tempEnumCode.ContainsKey(temp.Bz3))
                            {
                                temp.DevClass = tempEnumCode[temp.Bz3].StrEnumDisplay;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(CcmbDevID.Text))
                    {
                        temp.Bz4 = Convert.ToInt32(CcmbDevID.Text.Substring(0, CcmbDevID.Text.IndexOf('.'))); //DevTypeID
                        tempEnumCode = Model.DEVServiceModel.QueryDevMoelsCache();
                        if (null != tempEnumCode)
                        {
                            if (tempEnumCode.ContainsKey(temp.Bz4))
                            {
                                temp.DevModel = tempEnumCode[temp.Bz4].StrEnumDisplay;
                                //增加小数位数赋值
                                int tempInt = 0;
                                int.TryParse(tempEnumCode[temp.Bz4].LngEnumValue3, out tempInt);
                                temp.Bz1 = tempInt;
                            }
                        }
                    }
                    else
                    {
                        //增加小数位数赋值
                        temp.Bz1 = 1;
                    }
                    if (this.CpDocument.Controls.Count > 0)
                    {
                        if (!((CuBaseType)(CpDocument.Controls[0])).ConfirmInf(temp))//扩展属性
                        {
                            return;
                        }
                    }

                    if (_DevID > 0)
                    {
                        //更新    
                        Jc_DevInfo ExistDev = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID.ToString());

                        if (ExistDev != temp)
                        {
                            bool isUpdateDefThresholdValueFlag = false;
                            bool isupdateDerailValueFlag = false;
                            bool isupdateControlValueFlag = false;
                            if (ExistDev.Type == 1)
                            {
                                if (ExistDev.Z1 != temp.Z1
                                    || ExistDev.Z2 != temp.Z2
                                     || ExistDev.Z3 != temp.Z3
                                     || ExistDev.Z4 != temp.Z4
                                     || ExistDev.Z5 != temp.Z5
                                     || ExistDev.Z6 != temp.Z6
                                     || ExistDev.Z7 != temp.Z7
                                     || ExistDev.Z8 != temp.Z8)
                                {//判断，如果是模拟量修改了阈值，则提示用户是否将当前类型下已定义设备全部应用  20171220
                                    if (XtraMessageBox.Show("当前设备类型修改了阈值，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isUpdateDefThresholdValueFlag = true;
                                    }
                                }
                            }
                            else if (ExistDev.Type == 2)
                            {
                                //开关量设备类型状态颜色修改
                                if (ExistDev.Color1 != temp.Color1
                                    || ExistDev.Color2 != temp.Color2
                                    || ExistDev.Color3 != temp.Color3)
                                {
                                    if (XtraMessageBox.Show("当前设备类型修改了状态显示颜色，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isupdateDerailValueFlag = true;
                                    }
                                }
                            }
                            else if (ExistDev.Type == 3)
                            {
                                //控制量设备类型状态颜色修改
                                if (ExistDev.Color1 != temp.Color1
                                    || ExistDev.Color2 != temp.Color2)
                                {
                                    if (XtraMessageBox.Show("当前设备类型修改了状态显示颜色，是否将修改应用到已定义设备上？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                    {
                                        isupdateControlValueFlag = true;
                                    }
                                }
                            }
                            if (ExistDev != null)
                            {
                                temp.ID = ExistDev.ID;
                            }
                            temp.InfoState = InfoState.Modified;
                            //Model.DEVServiceModel.AddJC_DEVCache(temp);
                            try
                            {
                                try
                                {
                                    if (!Model.DEVServiceModel.UpdateJC_DEVCache(temp))
                                    {
                                        XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            OperateLogHelper.InsertOperateLog(2, "更新传感器类型【" + temp.Name + "】", "");


                            List<Jc_DefInfo> updatePoints = Model.DEFServiceModel.QueryPointByDevIDCache(temp.Devid.ToString());

                            if (updatePoints != null)
                            {
                                if (updatePoints.Count > 0)
                                {
                                    foreach (var item in updatePoints)
                                    {
                                        item.DevName = temp.Name;//DevID
                                        item.DevModelID = temp.Bz4;//DevTypeID
                                        item.DevModel = temp.DevModel;
                                        item.DevClassID = temp.Bz3;//DevClassID
                                        item.DevClass = temp.DevClass;
                                        item.DevPropertyID = temp.Type;//DevPropertyID
                                        item.DevProperty = temp.DevProperty;

                                        //阈值修改处理  20171220
                                        if (isUpdateDefThresholdValueFlag)
                                        {
                                            item.Z1 = temp.Z1;
                                            item.Z2 = temp.Z2;
                                            item.Z3 = temp.Z3;
                                            item.Z4 = temp.Z4;
                                            item.Z5 = temp.Z5;
                                            item.Z6 = temp.Z6;
                                            item.Z7 = temp.Z7;
                                            item.Z8 = temp.Z8;
                                        }
                                        //开关量控制量状态修改
                                        if (isupdateDerailValueFlag)
                                        {
                                            item.Bz9 = temp.Color1.ToString();
                                            item.Bz10 = temp.Color2.ToString();
                                            item.Bz11 = temp.Color3.ToString();
                                        }
                                        //控制量状态修改
                                        if (isupdateControlValueFlag)
                                        {
                                            item.Bz9 = temp.Color1.ToString();
                                            item.Bz10 = temp.Color2.ToString();
                                        }

                                        item.InfoState = InfoState.Modified;
                                    }
                                    try
                                    {
                                        Model.DEFServiceModel.UpdateDEFsCache(updatePoints);//将相应的测点告知数据采集
                                    }
                                    catch (Exception ex)
                                    {
                                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CtxbDevName.Text))//根据设备类型名称，判断是否有重复定义  20170627
                        {
                            List<Jc_DevInfo> tempDevice = Model.DEVServiceModel.QueryDevsCache().FindAll(a => a.Name == CtxbDevName.Text);
                            if (tempDevice != null && tempDevice.Count > 0)
                            {
                                MessageBox.Show("该设备已定义!", "提示");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择设备型号!", "提示");
                            return;
                        }

                        //新增
                        temp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        temp.InfoState = InfoState.AddNew;
                        try
                        {
                            if (!Model.DEVServiceModel.AddJC_DEVCache(temp))
                            {
                                XtraMessageBox.Show("保存失败，请确保网关运行正常并与主机的连接正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(ex);
                            XtraMessageBox.Show("保存失败：" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        OperateLogHelper.InsertOperateLog(2, "新增传感器类型【" + temp.Name + "】", "");
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败：" + ex, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Error("确认保存【Cbtn_Confirm_Click】", ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CtxbDevID.Text) || string.IsNullOrEmpty(CtxbDevName.Text))
                {
                    XtraMessageBox.Show("当前未处于编辑状态，不能进行删除操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复,且历史数据关联了此位置的无法查询,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(CtxbDevID.Text) && !string.IsNullOrEmpty(CtxbDevName.Text))
                    {
                        DeleteDevType(CtxbDevID.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
            //加延时  20170721
            Thread.Sleep(1000);
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close(); //
        }
    }
}
