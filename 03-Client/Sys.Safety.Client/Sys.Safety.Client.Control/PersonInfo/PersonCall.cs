using DevExpress.XtraEditors;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.R_Call;
using Sys.Safety.ServiceContract;

using Sys.Safety.Client.Define.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Control.PersonInfo
{
    public partial class PersonCall : XtraForm
    {
        /// <summary>
        /// 人员选择信息
        /// </summary>
        Dictionary<string, string> personSelectList = new Dictionary<string, string>();
        /// <summary>
        /// 设备选择信息
        /// </summary>
        Dictionary<string, string> pointSelectList = new Dictionary<string, string>();



        public PersonCall()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 人员选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                var formSelPerson = new PersonSetForm();
                var res = formSelPerson.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }
                //重新加载选择内容
                personSelectList.Clear();
                listBoxControl1.Items.Clear();
                foreach (R_PersoninfInfo person in formSelPerson.SelectPerson)
                {
                    personSelectList.Add(person.Bh, person.Name);
                    listBoxControl1.Items.Add(person.Bh + "(" + person.Name + ")");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //验证是否已下发呼叫
                if (!Validation())
                {
                    return;
                }
                R_CallInfo tempCallInfo = null;

                if (xtraTabControl1.SelectedTabPageIndex == 0)//人员呼叫
                {
                    if (radioGroup1.SelectedIndex == 0 || radioGroup1.SelectedIndex == 1)//呼叫人员
                    {
                        tempCallInfo = new R_CallInfo();
                        tempCallInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        tempCallInfo.MasterId = "0";
                        tempCallInfo.Type = 0;
                        tempCallInfo.CallType = radioGroup1.SelectedIndex;
                        tempCallInfo.CallPersonDefType = radioGroup2.SelectedIndex;
                        tempCallInfo.SendCount = 3;
                        tempCallInfo.CallTime = DateTime.Now;
                        if (radioGroup2.SelectedIndex == 1)
                        {
                            tempCallInfo.BhContent = textEdit1.Text + "-" + textEdit2.Text;
                        }
                        else if (radioGroup2.SelectedIndex == 2)
                        {
                            string selectPersons = "";
                            foreach (string key in personSelectList.Keys)
                            {
                                selectPersons += key + ",";
                            }
                            if (selectPersons.Contains(","))
                            {
                                selectPersons = selectPersons.Substring(0, selectPersons.Length - 1);
                            }
                            tempCallInfo.BhContent = selectPersons;
                        }
                    }
                }
                else
                {
                    if (radioGroup1.SelectedIndex == 0 || radioGroup1.SelectedIndex == 1)//呼叫设备
                    {
                        tempCallInfo = new R_CallInfo();
                        tempCallInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                        tempCallInfo.MasterId = "0";
                        tempCallInfo.Type = 1;
                        tempCallInfo.CallType = radioGroup1.SelectedIndex;
                        tempCallInfo.CallPersonDefType = 4;
                        tempCallInfo.SendCount = 3;
                        tempCallInfo.CallTime = DateTime.Now;

                        string selectPoints = "";
                        foreach (string key in pointSelectList.Keys)
                        {
                            selectPoints += key + ",";
                        }
                        if (selectPoints.Contains(","))
                        {
                            selectPoints = selectPoints.Substring(0, selectPoints.Length - 1);
                        }
                        tempCallInfo.PointList = selectPoints;
                    }
                }
                //添加到服务器
                if (tempCallInfo != null)
                {
                    Sys.Safety.Client.Control.Model.R_CallModel.R_CallModelInstance.AddR_CallInfo(tempCallInfo);
                }
                XtraMessageBox.Show("添加呼叫成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        private bool Validation()
        {
            bool rvalue = true;
            try
            {
                List<R_CallInfo> R_CallList = Sys.Safety.Client.Control.Model.R_CallModel.R_CallModelInstance.GetAllRCallCache();

                if (xtraTabControl1.SelectedTabPageIndex == 0)//人员呼叫
                {
                    if (radioGroup2.SelectedIndex == 0)//呼叫所有人员
                    {
                        if (R_CallList.FindAll(a => a.Type == 0 && a.CallPersonDefType == 0).Count > 0)
                        {//已经存在
                            XtraMessageBox.Show("已经进行了所有人员/设备呼叫，不能重复发送！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }
                    if (radioGroup2.SelectedIndex == 1)//呼叫指定卡号段
                    {
                        if (!Basic.Framework.Common.ValidationHelper.IsInt(textEdit1.Text))
                        {
                            XtraMessageBox.Show("起始卡号输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        if (!Basic.Framework.Common.ValidationHelper.IsInt(textEdit2.Text))
                        {
                            XtraMessageBox.Show("结束卡号输入不合法！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        foreach (R_CallInfo tempCall in R_CallList)
                        {
                            if (!string.IsNullOrEmpty(tempCall.BhContent))
                            {
                                if (tempCall.BhContent.Contains("-"))
                                {
                                    int startBh = int.Parse(tempCall.BhContent.Split('-')[0]);
                                    int endBh = int.Parse(tempCall.BhContent.Split('-')[1]);
                                    if (startBh <= int.Parse(textEdit2.Text) && endBh >= int.Parse(textEdit2.Text))
                                    {//已经存在
                                        XtraMessageBox.Show("当前选择的卡号段存在重复呼叫！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    if (radioGroup2.SelectedIndex == 2)//呼叫指定卡号
                    {
                        foreach (R_CallInfo tempCall in R_CallList)
                        {
                            foreach (string tempBh in personSelectList.Keys)
                            {
                                if (tempCall.BhContent.Split(',').Contains(tempBh))
                                {
                                    XtraMessageBox.Show("当前选择的人员存在重复呼叫！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (xtraTabControl1.SelectedTabPageIndex == 1)//设备呼叫
                {
                    if (radioGroup3.SelectedIndex == 1)//呼叫指定设备
                    {
                        foreach (R_CallInfo tempCall in R_CallList)
                        {
                            foreach (string tempPoint in pointSelectList.Keys)
                            {
                                if (tempCall.PointList.Split(',').Contains(tempPoint))
                                {
                                    XtraMessageBox.Show("当前选择的设备已发送过呼叫，不能重复发送！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return rvalue;
        }

        /// <summary>
        /// 设备选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                //先获取所有人员设备信息
                List<Jc_DefInfo> AllPersonPointInfo = DEFServiceModel.QueryPointByDevpropertIDCache(7);
                DataTable _dtSelectItem = new DataTable();
                _dtSelectItem.Columns.Add("Check");
                _dtSelectItem.Columns.Add("Id");
                _dtSelectItem.Columns.Add("Text");
                foreach (var item in AllPersonPointInfo)
                {
                    var row = _dtSelectItem.NewRow();
                    row["Check"] = false;
                    row["Id"] = item.Point;
                    row["Text"] = item.Point + item.Wz;
                    _dtSelectItem.Rows.Add(row);
                }

                var selectForm = new ItemSelect("设备选择", "设备名称", _dtSelectItem, pointSelectList.Keys.ToList());
                var res = selectForm.ShowDialog();
                if (res != DialogResult.OK)
                {
                    return;
                }
                pointSelectList.Clear();
                listBoxControl1.Items.Clear();
                foreach (string point in selectForm.SelectedIds)
                {
                    string Wz = AllPersonPointInfo.Find(a => a.Point == point).Wz;
                    pointSelectList.Add(point, Wz);
                    listBoxControl1.Items.Add(point + "(" + Wz + ")");
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonCall_Load(object sender, EventArgs e)
        {

        }


    }
}
