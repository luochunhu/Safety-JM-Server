using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Enumtype;
using Sys.Safety.Request.Enumcode;

namespace Sys.Safety.ClientFramework.Model
{
    public class EnumModel
    {


        IEnumtypeService _EnumtypeService = ServiceFactory.Create<IEnumtypeService>();
        IEnumcodeService _EnumcodeService = ServiceFactory.Create<IEnumcodeService>();

        public EnumtypeInfo SaveEnumType(EnumtypeInfo dto)
        {
            EnumtypeAddRequest enumtyperequest = new EnumtypeAddRequest();
            enumtyperequest.EnumtypeInfo = dto;
            var result = _EnumtypeService.SaveEnumType(enumtyperequest);
            return result.Data;
        }


        public void DeleteEnumType(EnumtypeInfo dto)
        {
            EnumtypeDeleteRequest enumtyperequest = new EnumtypeDeleteRequest();
            enumtyperequest.Id = dto.ID;
            _EnumtypeService.DeleteEnumtype(enumtyperequest);
        }


        public EnumtypeInfo GetEnumTypeByID(int EnumTypeID)
        {
            EnumtypeGetRequest enumtyperequest = new EnumtypeGetRequest();
            enumtyperequest.Id = EnumTypeID.ToString();
            var result = _EnumtypeService.GetEnumtypeById(enumtyperequest);
            return result.Data;
        }

        public EnumcodeInfo SaveEnumCode(EnumcodeInfo dto)
        {
            EnumcodeAddRequest enumtyperequest = new EnumcodeAddRequest();
            enumtyperequest.EnumcodeInfo = dto;
            var result = _EnumcodeService.SaveEnumCode(enumtyperequest);
            return result.Data;
        }


        public void DeleteEnumCode(EnumcodeInfo dto)
        {
            EnumcodeDeleteRequest enumtyperequest = new EnumcodeDeleteRequest();
            enumtyperequest.Id = dto.EnumCodeID;
            _EnumcodeService.DeleteEnumcode(enumtyperequest);
        }

        public void UpdateCache()
        {
            _EnumcodeService.UpdateCache();
        }

        public EnumcodeInfo GetEnumCodeByID(int EnumCodeID)
        {
            EnumcodeGetRequest enumtyperequest = new EnumcodeGetRequest();
            enumtyperequest.Id = EnumCodeID.ToString();
            var result = _EnumcodeService.GetEnumcodeById(enumtyperequest);
            return result.Data;
        }
        /// <summary>
        /// 根据枚举类型编码得到VO
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public bool GetEnumTypeVOByCode(string strCode)
        {
            EnumtypeGetByStrCodeRequest enumtyperequest = new EnumtypeGetByStrCodeRequest();
            enumtyperequest.StrCode = strCode;
            var result = _EnumtypeService.GetEnumtypeByStrCode(enumtyperequest);
            EnumtypeInfo type = result.Data;
            if (type != null && long.Parse(type.EnumTypeID) > 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 加载TreeList
        /// </summary>
        /// <returns></returns>
        public List<EnumtypeInfo> EnumTypeList()
        {
            var result = _EnumtypeService.GetEnumtypeList();
            List<EnumtypeInfo> list = result.Data;
            if (list.Count > 0)
                return list;
            else
                return new List<EnumtypeInfo>();
        }

        /// <summary>
        /// 得到所有枚举
        /// </summary>
        /// <returns></returns>
        public List<EnumcodeInfo> GetAllEnumCodeList()
        {
            var result = _EnumcodeService.GetEnumcodeList();
            List<EnumcodeInfo> lists = result.Data;
            return lists;
        }

        /// <summary>
        /// 根据枚举类型id得到枚举VO
        /// </summary>
        /// <param name="EnumTypeID">枚举类型ID</param>
        /// <returns></returns>
        public DataTable GetEnumCodeByEnumTypeID(int EnumTypeID)
        {
            EnumcodeGetByEnumTypeIDRequest enumtyperequest = new EnumcodeGetByEnumTypeIDRequest();
            enumtyperequest.EnumTypeId = EnumTypeID.ToString();
            var result = _EnumcodeService.GetEnumcodeByEnumTypeID(enumtyperequest);
            List<EnumcodeInfo> lists = result.Data;
            return Basic.Framework.Common.ObjectConverter.ToDataTable<EnumcodeInfo>(lists); 
        }



        public DataTable GetEnumtypeDataTable()
        {
            List<EnumtypeInfo> lists = EnumTypeList();
            return Basic.Framework.Common.ObjectConverter.ToDataTable<EnumtypeInfo>(lists);
        }

        /// <summary>
        /// 弹出MessageBox
        /// 1：错误,2：警告,3：消息,4：是和否对话框,5：是，否和取消对话框
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="type">1：错误,2：警告,3：消息,4：是和否对话框,5：是，否和取消对话框</param>
        /// <param name="IsShowMsg">0：不显示消息框；1：显示消息框</param>
        /// <param name="barStatic1">消息在状态栏中显示</param>
        public DialogResult ShowMessageBox(string msg, int type, int IsShowMsg, DevExpress.XtraBars.BarStaticItem barStatic1)
        {
            #region 弹出消息提示框
            DialogResult dialogresult = new DialogResult();
            switch (type)
            {
                case 1:
                    if (barStatic1 != null)
                        barStatic1.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  " + msg;
                    if (IsShowMsg == 1)
                    {
                        dialogresult = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case 2:
                    if (barStatic1 != null)
                        barStatic1.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  " + msg;
                    if (IsShowMsg == 1)
                    {
                        dialogresult = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case 3:
                    if (barStatic1 != null)
                        barStatic1.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  " + msg;
                    if (IsShowMsg == 1)
                    {
                        dialogresult = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case 4:
                    if (IsShowMsg == 1)
                    {
                        dialogresult = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    }
                    break;
                case 5:
                    if (IsShowMsg == 1)
                    {
                        dialogresult = DevExpress.XtraEditors.XtraMessageBox.Show(msg, "询问？", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    }
                    break;
            }
            return dialogresult;

            #endregion
        }
    }
}
