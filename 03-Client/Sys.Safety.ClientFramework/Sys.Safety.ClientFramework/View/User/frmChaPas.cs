using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.User;

namespace Sys.Safety.ClientFramework.View.User
{
    public partial class frmChaPas : DevExpress.XtraEditors.XtraForm
    {
      
        private string UserName = "";

        IUserService _UserService = ServiceFactory.Create<IUserService>();
        public frmChaPas()
        {
            InitializeComponent();
            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
            {
                UserName = (Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem).UserName;
            }
        }

        private void Submit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (string.IsNullOrEmpty(OldPas.Text)||
                string.IsNullOrEmpty(NewPas.Text)||
                string.IsNullOrEmpty(RepeatPas.Text))
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请输入标有星号的项。");
                return;
            }

            UserGetByCodeRequest userrequest = new UserGetByCodeRequest();
            userrequest.Code = UserName;
            var result = _UserService.GetUserByCode(userrequest);
            UserInfo user = result.Data;
            string sEncOldPas = Basic.Framework.Common.MD5Helper.MD5Encrypt(OldPas.Text);

            if (user == null || user.Password != sEncOldPas)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "密码错误。");
                return;
            }

            if (NewPas.Text != RepeatPas.Text)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "重复新密码与新密码不一致。");
                return;
            }

            //if (NewPas.Text.Length < 6)
            //{
            //    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "密码不能小于6位。");
            //    return;
            //}

            user.Password = Basic.Framework.Common.MD5Helper.MD5Encrypt(NewPas.Text);
            UserUpdateRequest updateuserrequest = new UserUpdateRequest();
            updateuserrequest.UserInfo = user;
            var updateresult = _UserService.UpdateUser(updateuserrequest);
            bool isSec = updateresult.IsSuccess;
            if (isSec)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "操作成功。");
                Close();
            }
            else
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "操作失败,请稍后再试。");
            }
        }

        private void Close_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }
    }
}