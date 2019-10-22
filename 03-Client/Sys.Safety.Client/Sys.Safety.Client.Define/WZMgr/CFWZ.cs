using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Collections;
using Sys.Safety.Client.Define.Model;
using System.Threading;
using DevExpress.XtraBars.Ribbon;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Basic.Framework.Web;

namespace Sys.Safety.Client.Define.WZMgr
{
    public partial class CFWZ : XtraForm
    {
        public CFWZ()
        {
            InitializeComponent();
        }
        public CFWZ(string WZID)
        {
            if (!string.IsNullOrEmpty(WZID))
            {
                int _WZID = 0;
                try
                {
                    _WZID = Convert.ToInt32(WZID);
                }
                catch (Exception)
                {
                    _WZID = 0;
                }

                if (_WZID > 0)
                {
                    _WZDTO = Model.WZServiceModel.QueryWZbyWZIDCache(_WZID);
                }
            }
            InitializeComponent();
        }

        /// <summary>
        /// 位置结构体
        /// </summary>
        private Jc_WzInfo _WZDTO = null;

        private void CFWZ_Load(object sender, EventArgs e)
        {
            try
            {
                LoadBasicInf();
            }
            catch (Exception ex)
            {

                LogHelper.Error(ex.ToString());
            }
        }

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("删除不可恢复,且历史数据关联了此位置的无法查询,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (null == _WZDTO)
                    {
                        return;
                    }
                    bool AllowDelete = true;
                    Jc_DefInfo tempPoints = Model.DEFServiceModel.QueryPointByWzCache(_WZDTO.Wz);

                    if (null != tempPoints)
                    {
                        //foreach (var item in tempPoints)
                        //{
                        //    if (item.Wzid == _WZDTO.WzID)
                        //    {
                        AllowDelete = false;
                        //break;
                        //    }
                        //}
                    }
                    if (AllowDelete)
                    {
                        IList<Jc_MacInfo> tempMACS = Model.MACServiceModel.QueryMACByWzCache(_WZDTO.Wz);
                        if (null != tempMACS && tempMACS.Count > 0)
                        {
                            //foreach (var item in tempMACS)
                            //{
                            //    if (item.Wzid == _WZDTO.WzID)
                            //    {
                            AllowDelete = false;
                            //        break;
                            //    }
                            //}
                        }
                    }

                    if (AllowDelete)
                    {
                        //_WZDTO.InfoState = InfoState.Delete;
                        Model.WZServiceModel.DeleteJC_WZCache(_WZDTO);// 20170411

                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("该安装位置当前正在被使用,禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WZverify())
                {
                    return;
                }

                Jc_WzInfo tempWZ = new Jc_WzInfo();
                tempWZ.WzID = CtxbWZID.Text;
                tempWZ.Wz = CtxbWZNAME.Text;

                if (null == _WZDTO)
                {
                    //增加安装位置重复检测  20170908
                    Jc_WzInfo tempWz = null;
                    tempWz = Model.WZServiceModel.QueryWZbyWZCache(CtxbWZNAME.Text);
                    if (null != tempWz)
                    {
                        XtraMessageBox.Show("当前添加的安装位置已经存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //新增
                    tempWZ.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    tempWZ.CreateTime = DateTime.Now;// 20170331
                    tempWZ.InfoState = InfoState.AddNew;
                    try
                    {
                        Model.WZServiceModel.AddJC_WZCache(tempWZ);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    //更新
                    if (_WZDTO != tempWZ)
                    {
                        tempWZ.ID = _WZDTO.ID;
                        tempWZ.CreateTime = _WZDTO.CreateTime;
                        tempWZ.InfoState = InfoState.Modified;
                        try
                        {
                            Model.WZServiceModel.UpdateJC_WZCache(tempWZ);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        #region 更新Point和MAC
                        List<Jc_DefInfo> tempPoints = Model.DEFServiceModel.QueryAllCache();
                        List<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
                        if (null != tempPoints)
                        {
                            for (int i = 0; i < tempPoints.Count; i++)
                            {
                                if (tempPoints[i].Wzid == tempWZ.WzID)
                                {
                                    tempPoints[i].Wz = tempWZ.Wz;
                                    tempPoints[i].InfoState = InfoState.Modified;
                                    UpdatePoints.Add(tempPoints[i]);
                                }
                            }
                            if (UpdatePoints.Count > 0)
                            {
                                try
                                {
                                    Model.DEFServiceModel.UpdateDEFsCache(UpdatePoints);//将相应的测点告知服务端
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        List<Jc_MacInfo> tempMACs = Model.MACServiceModel.QueryAllCache();
                        List<Jc_MacInfo> UpdateMACs = new List<Jc_MacInfo>();
                        if (null != tempMACs)
                        {
                            for (int i = 0; i < tempMACs.Count; i++)
                            {
                                if (tempMACs[i].Wzid == tempWZ.WzID)
                                {
                                    tempMACs[i].Wz = tempWZ.Wz;
                                    tempMACs[i].InfoState = InfoState.Modified;
                                    UpdateMACs.Add(tempMACs[i]);
                                }
                            }

                            if (UpdateMACs.Count > 0)
                            {
                                try
                                {
                                    Model.MACServiceModel.UpdateMACsCache(UpdateMACs);//将相应的MAC告知服务端
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        #endregion
                    }
                }
                //加延时  20170721
                Thread.Sleep(1000);

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbtn_Cancle_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }

        }


        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadBasicInf()
        {

            if (null == _WZDTO)
            {
                //新增安装位置
                CtxbWZID.Text = (Model.WZServiceModel.GetMaxWzID() + 1).ToString();

            }
            else
            {
                //编辑安装位置
                CtxbWZID.Text = _WZDTO.WzID.ToString();
                CtxbWZNAME.Text = _WZDTO.Wz;
            }
        }

        private bool WZverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(CtxbWZID.Text))
            {
                XtraMessageBox.Show("请输入位置编号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (string.IsNullOrEmpty(CtxbWZNAME.Text))
            {
                XtraMessageBox.Show("请输入位置名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ret;
            }
            if (DefinePublicClass.ValidationSpecialSymbols(CtxbWZNAME.Text))
            {
                XtraMessageBox.Show("位置名称中不能包含特殊字符,请切换成全角录入", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (CtxbWZNAME.Text.Length > 30) //xuzp20151126
            {
                XtraMessageBox.Show("位置名称长度不能超过30个字符", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
           
            ret = true;
            return ret;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!WZverify())
                {
                    return;
                }

                Jc_WzInfo tempWZ = new Jc_WzInfo();
                tempWZ.WzID = CtxbWZID.Text;
                tempWZ.Wz = CtxbWZNAME.Text;

                if (null == _WZDTO)
                {
                    //增加安装位置重复检测  20170908
                    Jc_WzInfo tempWz = null;
                    tempWz = Model.WZServiceModel.QueryWZbyWZCache(CtxbWZNAME.Text);
                    if (null != tempWz)
                    {
                        XtraMessageBox.Show("当前添加的安装位置已经存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    //新增
                    tempWZ.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    tempWZ.CreateTime = DateTime.Now;// 20170331
                    tempWZ.InfoState = InfoState.AddNew;
                    try
                    {
                        Model.WZServiceModel.AddJC_WZCache(tempWZ);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    //更新
                    if (_WZDTO != tempWZ)
                    {
                        tempWZ.ID = _WZDTO.ID;
                        tempWZ.CreateTime = _WZDTO.CreateTime;
                        tempWZ.InfoState = InfoState.Modified;
                        try
                        {
                            Model.WZServiceModel.UpdateJC_WZCache(tempWZ);
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        #region 更新Point和MAC
                        List<Jc_DefInfo> tempPoints = Model.DEFServiceModel.QueryAllCache();
                        List<Jc_DefInfo> UpdatePoints = new List<Jc_DefInfo>();
                        if (null != tempPoints)
                        {
                            for (int i = 0; i < tempPoints.Count; i++)
                            {
                                if (tempPoints[i].Wzid == tempWZ.WzID)
                                {
                                    tempPoints[i].Wz = tempWZ.Wz;
                                    tempPoints[i].InfoState = InfoState.Modified;
                                    UpdatePoints.Add(tempPoints[i]);
                                }
                            }
                            if (UpdatePoints.Count > 0)
                            {
                                try
                                {
                                    Model.DEFServiceModel.UpdateDEFsCache(UpdatePoints);//将相应的测点告知服务端
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }

                        List<Jc_MacInfo> tempMACs = Model.MACServiceModel.QueryAllCache();
                        List<Jc_MacInfo> UpdateMACs = new List<Jc_MacInfo>();
                        if (null != tempMACs)
                        {
                            for (int i = 0; i < tempMACs.Count; i++)
                            {
                                if (tempMACs[i].Wzid == tempWZ.WzID)
                                {
                                    tempMACs[i].Wz = tempWZ.Wz;
                                    tempMACs[i].InfoState = InfoState.Modified;
                                    UpdateMACs.Add(tempMACs[i]);
                                }
                            }

                            if (UpdateMACs.Count > 0)
                            {
                                try
                                {
                                    Model.MACServiceModel.UpdateMACsCache(UpdateMACs);//将相应的MAC告知服务端
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        #endregion
                    }
                }
                //加延时  20170721
                Thread.Sleep(1000);

                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("删除不可恢复,且历史数据关联了此位置的无法查询,是否确定删除？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (null == _WZDTO)
                    {
                        return;
                    }
                    bool AllowDelete = true;
                    Jc_DefInfo tempPoints = Model.DEFServiceModel.QueryPointByWzCache(_WZDTO.Wz);

                    if (null != tempPoints)
                    {
                        //foreach (var item in tempPoints)
                        //{
                        //    if (item.Wzid == _WZDTO.WzID)
                        //    {
                        AllowDelete = false;
                        //break;
                        //    }
                        //}
                    }
                    if (AllowDelete)
                    {
                        IList<Jc_MacInfo> tempMACS = Model.MACServiceModel.QueryMACByWzCache(_WZDTO.Wz);
                        if (null != tempMACS && tempMACS.Count > 0)
                        {
                            //foreach (var item in tempMACS)
                            //{
                            //    if (item.Wzid == _WZDTO.WzID)
                            //    {
                            AllowDelete = false;
                            //        break;
                            //    }
                            //}
                        }
                    }

                    if (AllowDelete)
                    {
                        //_WZDTO.InfoState = InfoState.Delete;
                        Model.WZServiceModel.DeleteJC_WZCache(_WZDTO);// 20170411

                        this.Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("该安装位置当前正在被使用,禁止删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.ToString());
            }
        }
    }
}
