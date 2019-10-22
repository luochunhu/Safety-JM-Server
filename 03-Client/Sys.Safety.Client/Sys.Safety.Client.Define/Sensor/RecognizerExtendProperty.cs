using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sys.Safety.DataContract;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraEditors;

namespace Sys.Safety.Client.Define.Sensor
{
    /// <summary>
    /// 人员定位识别器定义  20171122
    /// </summary>
    public partial class RecognizerExtendProperty : CuBase
    {
        /// <summary>
        /// 禁止、限制进入人员名单
        /// </summary>
        public List<R_RestrictedpersonInfo> restrictedpersonInfoList = new List<R_RestrictedpersonInfo>();
        /// <summary>
        /// 识别器类型
        /// </summary>
        List<EnumcodeInfo> pointTypeEnum = new List<EnumcodeInfo>();
        /// <summary>
        /// 当前编辑的识别器信息
        /// </summary>
        Jc_DefInfo tempEditPoint = null;
        

        public RecognizerExtendProperty(string arrPoint, int devID, uint SourceNum)
            : base(arrPoint, devID, SourceNum)
        {
            InitializeComponent();
        }
        /// <summary>
        /// 限制进入、禁止进入人员选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
            List<R_RestrictedpersonInfo> tempSel = restrictedpersonInfoList.FindAll(a => a.Type == AlarmType.SelectedIndex);
            var formSelPerson = new PersonSetForm(tempSel);
            var res = formSelPerson.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            restrictedpersonInfoList.Clear();
            foreach (R_PersoninfInfo person in formSelPerson.SelectPerson)
            {
                R_RestrictedpersonInfo tempR_RestrictedpersonInfo = new R_RestrictedpersonInfo();
                tempR_RestrictedpersonInfo.Id = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                if (tempEditPoint == null)//如果是识别器新增
                {
                    tempR_RestrictedpersonInfo.PointId = "";//先不赋值PointID，在保存时创建了才能赋值  20171122
                }
                else
                {
                    tempR_RestrictedpersonInfo.PointId = tempEditPoint.PointID;
                }
                tempR_RestrictedpersonInfo.Type = AlarmType.SelectedIndex;
                tempR_RestrictedpersonInfo.Yid = person.Yid;
                restrictedpersonInfoList.Add(tempR_RestrictedpersonInfo);
            }
        }

        /// <summary>
        /// 加载测点的默认信息函数
        /// </summary>
        public override void LoadPretermitInf()
        {
            //加载识别器类型
            if (pointTypeEnum.Count < 1)
            {
                pointTypeEnum = EnumService.GetEnum(22);
            }
            foreach (var item in pointTypeEnum)
            {
                comboPersonPointType.Properties.Items.Add(item.LngEnumValue.ToString() + "." + item.StrEnumDisplay);
            }
            comboPersonPointType.SelectedIndex = 0;
            //加载默认报警时间
            AlarmTime.Text = "08:00";
            //加载默认离开时间
            LeaveTime.Text = "08:00";
            //加载默认报警类型
            AlarmType.SelectedIndex = 0;
            //加载默认超员报警人数
            RatedPersonCount.Value = 0;

            restrictedpersonInfoList = new List<R_RestrictedpersonInfo>();
        }
        /// <summary>
        /// 加载测点信息的函数
        /// </summary>
        public override void LoadInf(string arrPoint, int devID)
        {
            //查找是否是编辑状态，编辑状态，则加载测点的定义信息
            tempEditPoint = Model.DEFServiceModel.QueryPointByCodeCache(arrPoint);
            if (tempEditPoint == null)
            {
                return;
            }
            //加载识别器类型  20171122
            comboPersonPointType.SelectedIndex = tempEditPoint.Bz1 - 1;
            //加载识别器报警时间、离开时间
            AlarmTime.Text = (tempEditPoint.K4 / 60).ToString("00") + ":" + (tempEditPoint.K4 % 60).ToString("00");
            LeaveTime.Text = (tempEditPoint.K5 / 60).ToString("00") + ":" + (tempEditPoint.K5 % 60).ToString("00");
            //加载超员报警人数（额定人数）
            RatedPersonCount.Value = tempEditPoint.K3;
            //加载限制进入、禁止进入人员
            if (tempEditPoint.RestrictedpersonInfoList != null)
            {
                restrictedpersonInfoList = tempEditPoint.RestrictedpersonInfoList;
            }
            if (restrictedpersonInfoList.Count > 0)
            {
                AlarmType.SelectedIndex = restrictedpersonInfoList[0].Type;
            }           
        }
        /// <summary>
        /// 保存测点信息的函数
        /// </summary>
        public override bool ConfirmInf(Jc_DefInfo point)
        {
            if (!SensorInfoverify())
            {
                return false;
            }
            if (point == null)
            {
                return false;
            }
            //识别器类型
            point.Bz1 = comboPersonPointType.SelectedIndex + 1;
            //报警时间、离开时间(存成分钟)
            point.K4 = int.Parse(AlarmTime.Text.Substring(0, 2)) * 60 + int.Parse(AlarmTime.Text.Substring(3, 2));
            point.K5 = int.Parse(LeaveTime.Text.Substring(0, 2)) * 60 + int.Parse(LeaveTime.Text.Substring(3, 2));
            //限制进入、禁止进入人员            
            point.RestrictedpersonInfoList = restrictedpersonInfoList;

            //超员报警人数（额定人数）
            point.K3 = (int)RatedPersonCount.Value;

            return true;
        }
        /// <summary>
        /// 有效性验证
        /// </summary>
        public virtual bool SensorInfoverify()
        {
            bool ret = false;
            if (string.IsNullOrEmpty(this.comboPersonPointType.Text))
            {
                XtraMessageBox.Show("请选择识别器类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            if (string.IsNullOrEmpty(this.LeaveTime.Text))
            {
                XtraMessageBox.Show("请设置识别器离开时间", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ret;
            }
            ret = true;
            return ret;
        }
        /// <summary>
        /// 设备类型变化产生的影响
        /// </summary>
        public virtual void DevTypeChanngeEvent(long DevID, Jc_DefInfo Point)
        {

        }
    }
}
