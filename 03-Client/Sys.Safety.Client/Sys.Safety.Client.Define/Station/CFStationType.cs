using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using Basic.Framework.Web;
using Sys.Safety.ClientFramework.CBFCommon;
using Sys.Safety.Client.Define.Model;
using Sys.Safety.Enums;

namespace Sys.Safety.Client.Define.Station
{
    public partial class CFStationType : XtraForm
    {
        /// <summary>
        /// 传入的设备类型名称
        /// </summary>
        private string _DevID;
        public CFStationType()
        {
            InitializeComponent();
        }
        public CFStationType(string DevID)
        {
            _DevID = DevID;
            InitializeComponent();
        }
        private void StationType_Load(object sender, EventArgs e)
        {
            try
            {
                LoadPretermitInf(); //加载默认属性
                LoadBasicInf(); //加载具体信息
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadBasicInf()
        {
            if (!string.IsNullOrEmpty(_DevID))
            {
                Jc_DevInfo DeviceTypeTemp = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID);

                if (!string.IsNullOrEmpty(DeviceTypeTemp.Name))
                {
                    CtxbDvtypeName.Text = DeviceTypeTemp.Name; //名称
                    CtxbDvtypeID.Text = DeviceTypeTemp.Devid.ToString(); //ID
                    CcmbDriverID.Text = DeviceTypeTemp.LC2.ToString();//DrvID
                    Dictionary<int, EnumcodeInfo> tempDriver = Model.DEVServiceModel.QueryDriverInf();
                    if (null != tempDriver)
                    {
                        if (tempDriver.ContainsKey(DeviceTypeTemp.LC2))
                        {
                            if (null != tempDriver[DeviceTypeTemp.LC2])
                            {
                                CcmbDriverID.Text = DeviceTypeTemp.LC2.ToString() + "." + tempDriver[DeviceTypeTemp.LC2].StrEnumDisplay;//DrvID+DrvName
                            }
                        }
                    }
                    CcmbAnalogNum.Text = DeviceTypeTemp.Pl1.ToString();//模开量
                    CcmbControlNum.Text = DeviceTypeTemp.Pl2.ToString();//控制量
                    CcmbArticalNum.Text = DeviceTypeTemp.Pl3.ToString();//智能量
                    CcmbPersonNum.Text = DeviceTypeTemp.Pl4.ToString();//人员量
                    CtxbDesc.Text = DeviceTypeTemp.Remark;//描述
                    //库存数量  20170330
                    if (string.IsNullOrEmpty(DeviceTypeTemp.Bz7))
                    {
                        Stock.Value = 0;
                    }
                    else
                    {
                        int tempint = 0;
                        int.TryParse(DeviceTypeTemp.Bz7, out tempint);
                        Stock.Value = tempint;
                    }
                    if (DeviceTypeTemp.Bz4 > 0)
                    {
                        if (CcmbDevID.Properties.Items.Count > 0)
                        {
                            for (int i = 0; i < CcmbDevID.Properties.Items.Count; i++)
                            {
                                if (DeviceTypeTemp.Bz4.ToString().Length > 3)//增加判断  20170401
                                {
                                    if (CcmbDevID.Properties.Items[i].ToString().Split('.')[0] == DeviceTypeTemp.Bz4.ToString().Substring(3, DeviceTypeTemp.Bz4.ToString().Length - 3))//txy 20170323
                                    {
                                        CcmbDevID.SelectedIndex = i;
                                    }
                                }
                            }
                        }
                    }

                    // 20171115
                    if (DeviceTypeTemp.Sysid == 0)
                    {
                        SystemName.EditValue = (int)SystemEnum.Security;
                    }
                    else
                    {
                        SystemName.EditValue = DeviceTypeTemp.Sysid;
                    }
                }

            }
            else
            {
                CtxbDvtypeID.Text = (Model.DEVServiceModel.GetMaxDevID() + 1).ToString(); //新增ID
            }
        }
        /// <summary>
        /// 加载默认的初始信息
        /// </summary>
        private void LoadPretermitInf()
        {
            try
            {
                // 20171115
                //绑定系统名称
                var systemName = EnumService.GetEnum(21);
                SystemName.Properties.DataSource = systemName;
                SystemName.Properties.NullText = "未选择";

                Dictionary<int, EnumcodeInfo> temp;
                for (int i = 0; i <= 128; i++)
                {
                    CcmbAnalogNum.Properties.Items.Add(i.ToString());//模开量
                    CcmbControlNum.Properties.Items.Add(i.ToString());//控制量
                    CcmbArticalNum.Properties.Items.Add(i.ToString());//智能量
                    CcmbPersonNum.Properties.Items.Add(i.ToString());//人员量
                }
                Dictionary<int, EnumcodeInfo> tempDriver = Model.DEVServiceModel.QueryDriverInf();
                if (null != tempDriver)
                {
                    foreach (var item in tempDriver.Values)
                    {
                        if (item.LngEnumValue == 0)
                        {
                            continue;
                        }
                        CcmbDriverID.Properties.Items.Add(item.LngEnumValue.ToString() + "." + item.StrEnumDisplay);
                    }
                }
                temp = Model.DEVServiceModel.QueryDevMoelsCache();
                this.CcmbDevID.Properties.Items.Add("");
                foreach (var item in temp.Values)
                {
                    if (item.LngEnumValue > 1000)//txy 20170323
                    {
                        this.CcmbDevID.Properties.Items.Add(item.LngEnumValue.ToString().Substring(3, item.LngEnumValue.ToString().Length - 3) + "." + item.StrEnumDisplay); //增加设备型号
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载默认的初始信息【LoadPretermitInf】分站", ex);
            }
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Sensorverify())
                {
                    return;
                }
                int xh = 0;
                Jc_DevInfo DeviceTypeTemp = new Jc_DevInfo();
                DeviceTypeTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                DeviceTypeTemp.Devid = CtxbDvtypeID.Text.Trim();//编号
                DeviceTypeTemp.Type = 0;//设备性质
                DeviceTypeTemp.Name = CtxbDvtypeName.Text;//名称
                DeviceTypeTemp.LC = 0x01;//0x01监控/0x02抽累/0x04智开/0x08人员 种类
                DeviceTypeTemp.LC2 = Convert.ToInt16(CcmbDriverID.Text.Substring(0, CcmbDriverID.Text.IndexOf('.')));//描述 驱动编号
                DeviceTypeTemp.Pl1 = Convert.ToInt16(CcmbAnalogNum.Text);//模开数量
                DeviceTypeTemp.Pl2 = Convert.ToInt16(CcmbControlNum.Text); //控制数量
                DeviceTypeTemp.Pl3 = Convert.ToInt16(CcmbArticalNum.Text);//智能数量
                DeviceTypeTemp.Pl4 = Convert.ToInt16(CcmbPersonNum.Text);//人员数量
                DeviceTypeTemp.Bz4 = 0;//设备型号(枚举表中枚举数据)
                DeviceTypeTemp.Bz3 = 0;//设备种类(枚举表中枚举数据)
                DeviceTypeTemp.Remark = CtxbDesc.Text;//备注
                //新增库存数量  20170330
                DeviceTypeTemp.Bz7 = Stock.Value.ToString();
                // 20171115
                DeviceTypeTemp.Sysid = (int)SystemName.EditValue;

                Dictionary<int, EnumcodeInfo> tempEnumCode;
                if (!string.IsNullOrEmpty(CcmbDevID.Text))//tanxingyan 20161207 
                {
                    DeviceTypeTemp.Bz4 = Convert.ToInt32("100" + CcmbDevID.Text.Substring(0, CcmbDevID.Text.IndexOf('.'))); //DevTypeID
                    tempEnumCode = Model.DEVServiceModel.QueryDevMoelsCache();
                    if (null != tempEnumCode)
                    {
                        if (tempEnumCode.ContainsKey(DeviceTypeTemp.Bz4))
                        {
                            DeviceTypeTemp.DevModel = tempEnumCode[DeviceTypeTemp.Bz4].StrEnumDisplay;
                        }
                    }
                }
                if (string.IsNullOrEmpty(_DevID)) //表示新增
                {
                    DeviceTypeTemp.InfoState = InfoState.AddNew;
                    //Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp);
                    try
                    {
                        if (!Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp))
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
                    OperateLogHelper.InsertOperateLog(2, "新增分站类型【" + DeviceTypeTemp.Name + "】", "");
                }
                else //表示更新
                {
                    // 20171115
                    Jc_DevInfo DeviceTypeCache = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID);
                    //if (DeviceTypeCache != DeviceTypeTemp)
                    {
                        DeviceTypeTemp.ID = DeviceTypeCache.ID;
                        DeviceTypeTemp.InfoState = InfoState.Modified;
                        //Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp);
                        try
                        {
                            if (!Model.DEVServiceModel.UpdateJC_DEVCache(DeviceTypeTemp))
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
                        OperateLogHelper.InsertOperateLog(2, "修改分站类型【" + DeviceTypeTemp.Name + "】", "");

                        
                        List<Jc_DefInfo> tempStation = Model.DEFServiceModel.QueryPointByDevIDCache(DeviceTypeTemp.Devid);                        

                        if (tempStation != null)
                        {
                            bool bUpdateFlag = false;
                            foreach (var item in tempStation)
                            {
                                if (item.DevName != DeviceTypeTemp.Name)
                                {
                                    bUpdateFlag = true;
                                    item.DevName = DeviceTypeTemp.Name;
                                    item.InfoState = InfoState.Modified;
                                }
                            }

                            if (bUpdateFlag)
                            {
                                try
                                {
                                    Model.DEFServiceModel.UpdateDEFsCache(tempStation);
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
                this.Close(); //执行成功 关闭窗口

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败,请检查输入的合法性！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbnt_Delete_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrEmpty(CtxbDvtypeID.Text) || string.IsNullOrEmpty(CtxbDvtypeName.Text))
                {
                    XtraMessageBox.Show("当前未处于编辑状态，不能进行删除操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(CtxbDvtypeID.Text) && !string.IsNullOrEmpty(CtxbDvtypeName.Text))
                    {
                        DeleteDevType(CtxbDvtypeID.Text);
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
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        ///<summary>
        /// 验证有效性
        /// </summary>
        /// <returns></returns>
        private bool Sensorverify()
        {
            bool ret = false;

            // 20171115
            var systemName = SystemName.EditValue;
            if (systemName == null)
            {
                XtraMessageBox.Show("请输入系统名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }

            if (string.IsNullOrEmpty(CtxbDvtypeName.Text))
            {
                XtraMessageBox.Show("请填写设备名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CtxbDvtypeName.Text))
            {
                XtraMessageBox.Show("设备名称中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CtxbDvtypeName.Text.Length > 20)
            {
                XtraMessageBox.Show("设备名称长度不能超过20个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbDvtypeID.Text))
            {
                XtraMessageBox.Show("请填写设备编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbDriverID.Text))
            {
                XtraMessageBox.Show("请填写驱动编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbAnalogNum.Text))
            {
                XtraMessageBox.Show("请填写基础通道容量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbControlNum.Text))
            {
                XtraMessageBox.Show("请填写控制端口容量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbArticalNum.Text))
            {
                XtraMessageBox.Show("请填写智能端口容量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(CcmbPersonNum.Text))
            {
                XtraMessageBox.Show("请填写人员端口容量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            ret = true;
            return ret;
        }
        /// <summary>
        /// 删除设备类型
        /// </summary>
        private void DeleteDevType(string DevID)
        {
            Jc_DevInfo temp = Model.DEVServiceModel.QueryDevByDevIDCache(DevID);
            IList<Jc_DefInfo> PointsExist;
            if (null == temp)
            {
                return;
            }
            PointsExist = Model.DEFServiceModel.QueryPointByDevIDCache(temp.Devid);
            if (null != PointsExist)
            {
                if (PointsExist.Count > 0)
                {
                    XtraMessageBox.Show("该设备类型下存在定义测点，不能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            temp.InfoState = InfoState.Delete;
            try
            {
                Model.DEVServiceModel.DelJC_DEVCache(temp);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OperateLogHelper.InsertOperateLog(2, "删除分站类型【" + temp.Name + "】", "");
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!Sensorverify())
                {
                    return;
                }
                int xh = 0;
                Jc_DevInfo DeviceTypeTemp = new Jc_DevInfo();
                DeviceTypeTemp.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                DeviceTypeTemp.Devid = CtxbDvtypeID.Text.Trim();//编号
                DeviceTypeTemp.Type = 0;//设备性质
                DeviceTypeTemp.Name = CtxbDvtypeName.Text;//名称
                DeviceTypeTemp.LC = 0x01;//0x01监控/0x02抽累/0x04智开/0x08人员 种类
                DeviceTypeTemp.LC2 = Convert.ToInt16(CcmbDriverID.Text.Substring(0, CcmbDriverID.Text.IndexOf('.')));//描述 驱动编号
                DeviceTypeTemp.Pl1 = Convert.ToInt16(CcmbAnalogNum.Text);//模开数量
                DeviceTypeTemp.Pl2 = Convert.ToInt16(CcmbControlNum.Text); //控制数量
                DeviceTypeTemp.Pl3 = Convert.ToInt16(CcmbArticalNum.Text);//智能数量
                DeviceTypeTemp.Pl4 = Convert.ToInt16(CcmbPersonNum.Text);//人员数量
                DeviceTypeTemp.Bz4 = 0;//设备型号(枚举表中枚举数据)
                DeviceTypeTemp.Bz3 = 0;//设备种类(枚举表中枚举数据)
                DeviceTypeTemp.Remark = CtxbDesc.Text;//备注
                //新增库存数量  20170330
                DeviceTypeTemp.Bz7 = Stock.Value.ToString();
                // 20171115
                DeviceTypeTemp.Sysid = (int)SystemName.EditValue;

                Dictionary<int, EnumcodeInfo> tempEnumCode;
                if (!string.IsNullOrEmpty(CcmbDevID.Text))//tanxingyan 20161207 
                {
                    DeviceTypeTemp.Bz4 = Convert.ToInt32("100" + CcmbDevID.Text.Substring(0, CcmbDevID.Text.IndexOf('.'))); //DevTypeID
                    tempEnumCode = Model.DEVServiceModel.QueryDevMoelsCache();
                    if (null != tempEnumCode)
                    {
                        if (tempEnumCode.ContainsKey(DeviceTypeTemp.Bz4))
                        {
                            DeviceTypeTemp.DevModel = tempEnumCode[DeviceTypeTemp.Bz4].StrEnumDisplay;
                        }
                    }
                }
                if (string.IsNullOrEmpty(_DevID)) //表示新增
                {
                    DeviceTypeTemp.InfoState = InfoState.AddNew;
                    //Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp);
                    try
                    {
                        if (!Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp))
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
                    OperateLogHelper.InsertOperateLog(2, "新增分站类型【" + DeviceTypeTemp.Name + "】", "");
                }
                else //表示更新
                {
                    // 20171115
                    Jc_DevInfo DeviceTypeCache = Model.DEVServiceModel.QueryDevByDevIDCache(_DevID);
                    //if (DeviceTypeCache != DeviceTypeTemp)
                    {
                        DeviceTypeTemp.ID = DeviceTypeCache.ID;
                        DeviceTypeTemp.InfoState = InfoState.Modified;
                        //Model.DEVServiceModel.AddJC_DEVCache(DeviceTypeTemp);
                        try
                        {
                            if (!Model.DEVServiceModel.UpdateJC_DEVCache(DeviceTypeTemp))
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
                        OperateLogHelper.InsertOperateLog(2, "修改分站类型【" + DeviceTypeTemp.Name + "】", "");


                        List<Jc_DefInfo> tempStation = Model.DEFServiceModel.QueryPointByDevIDCache(DeviceTypeTemp.Devid);

                        if (tempStation != null)
                        {
                            bool bUpdateFlag = false;
                            foreach (var item in tempStation)
                            {
                                if (item.DevName != DeviceTypeTemp.Name)
                                {
                                    bUpdateFlag = true;
                                    item.DevName = DeviceTypeTemp.Name;
                                    item.InfoState = InfoState.Modified;
                                }
                            }

                            if (bUpdateFlag)
                            {
                                try
                                {
                                    Model.DEFServiceModel.UpdateDEFsCache(tempStation);
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
                this.Close(); //执行成功 关闭窗口

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败,请检查输入的合法性！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.Error(ex.ToString());
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(CtxbDvtypeID.Text) || string.IsNullOrEmpty(CtxbDvtypeName.Text))
                {
                    XtraMessageBox.Show("当前未处于编辑状态，不能进行删除操作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("删除不可恢复,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!string.IsNullOrEmpty(CtxbDvtypeID.Text) && !string.IsNullOrEmpty(CtxbDvtypeName.Text))
                    {
                        DeleteDevType(CtxbDvtypeID.Text);
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
            this.Close();
        }
    }
}
