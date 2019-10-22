using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Reflection;
using System.Collections;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.User;
using Basic.Framework.Common;
using Sys.Safety.Request.Userrole;
using Sys.Safety.Request.Role;
using Basic.Framework.Web;

namespace Sys.Safety.ClientFramework.View.User
{
    public partial class frmUserAdmin : DevExpress.XtraEditors.XtraForm
    {
        #region ======================成员变量==========================
        /// <summary>
        /// 菜单列表
        /// </summary>
        private IList<UserInfo> lstUser = new List<UserInfo>();
        /// <summary>
        /// 当前选择的用户编号
        /// </summary>
        private int userID = 0;
        /// <summary>
        /// 用户角色对象列表
        /// </summary>
        private List<UserroleInfo> lstUserRole = new List<UserroleInfo>();

        /// <summary>
        /// dt
        /// </summary>
        private DataTable dtUser = new DataTable();

        IUserService _UserService = ServiceFactory.Create<IUserService>();
        IRoleService _RoleService = ServiceFactory.Create<IRoleService>();
        IUserroleService _UserroleService = ServiceFactory.Create<IUserroleService>();
        #endregion

        #region ======================窗体函数==========================
        public frmUserAdmin()
        {
            InitializeComponent();
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void frmUserAdmin_Load(object sender, EventArgs e)
        {
            UserGridView.IndicatorWidth = 35;
            LoadData();
            UserGridView.FocusedRowHandle = -1;
        }

        private void simplePager1_myPagerEvents(int curPage, int pageSize)
        {
            simplePager1.RefreshPager(pageSize, dtUser.Rows.Count, curPage);
            PageCount = pageSize;
            DataBind(curPage - 1);
        }
        private int PageCount = 5;

        private void DataBind(int PageInt)
        {
            if (dtUser != null)
            {
                DataTable Dt = dtUser.Clone();
                if (dtUser.Rows.Count == 0)
                {
                    gridCtrlView.DataSource = Dt;
                    return;
                }
                //显示当前页的记录，并加载到Dt表里面
                for (int i = PageInt * PageCount; i < dtUser.Rows.Count && (i < PageInt * PageCount + PageCount); i++)
                {
                    Dt.ImportRow(dtUser.Rows[i]);
                }
                gridCtrlView.DataSource = Dt;
            }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                UserInfo dto = null;
                List<MenuInfo> rightTypeDTO = new List<MenuInfo>();
                if (userID > 0)
                {
                    DialogResult dr = Sys.Safety.ClientFramework.View.UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Confirm, "确定要删除此用户?");
                    if (dr == DialogResult.Yes)
                    {
                        try
                        {
                            dto = GetData();
                            UserDeleteRequest userrequest = new UserDeleteRequest();
                            userrequest.Id = dto.UserID;
                            _UserService.DeleteUser(userrequest);
                            
                            LoadData();
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  删除数据成功";
                        }
                        catch (Exception ex)
                        {
                            StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " " + ex.Message.ToString();
                            UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, StaticMsg.Caption.ToString());
                        }
                    }
                }
                else
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  请选择需要删除的数据!";
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void btnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = UserGridView.FocusedRowHandle;
                userID = TypeConvert.ToInt(UserGridView.GetRowCellValue(rowHandle, "UserID"));
                if (userID > 0)
                {
                    frmAddUser frm = new frmAddUser(userID, 1);
                    frm.ShowDialog();
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  修改数据成功";
                    LoadData();
                }
                else
                {
                    UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, "请先选择需要修改的用户！");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                frmAddUser frm = new frmAddUser();
                frm.ShowDialog();
                LoadData();
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  添加数据成功";
            }
            catch (Exception ex)
            {
                UserControl.Message.MessageBox.Show(UserControl.Message.MessageBox.MessageType.Information, ex.Message.ToString());
            }
            finally { }
        }

        private void UserGridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowHandle = UserGridView.FocusedRowHandle;
            if (rowHandle < 0)
            {
                return;
            }
            userID = TypeConvert.ToInt(UserGridView.GetRowCellValue(rowHandle, "UserID"));
        }
        private void UserGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.Name == "GridRole")
            {
                for (int i = 0; i < lstUser.Count; i++)
                {
                    UserroleGetByUserIdRequest userrolerequest = new UserroleGetByUserIdRequest();
                    userrolerequest.UserId = lstUser[i].UserID;
                    var result = _UserroleService.GetUserRoleByUserId(userrolerequest);
                    lstUserRole = result.Data;
                    if (lstUserRole.Count > 0)
                    {
                        RoleGetRequest rolerequest = new RoleGetRequest();
                        rolerequest.Id = lstUserRole[0].RoleID;
                        var resultGetRoleById = _RoleService.GetRoleById(rolerequest);
                        string str = resultGetRoleById.Data.RoleName;
                        e.DisplayText = str;
                    }
                }
                //e.DisplayText = "xxx";
            }
        }
        #endregion

        #region ======================自定义函数========================
        /// <summary>
        /// 加载数据
        /// </summary>
        private void LoadData()
        {
            var result = _UserService.GetUserList();
            lstUser = result.Data;
            gridCtrlView.DataSource = lstUser;

            dtUser = DataTableExtensions.ToDataTable<UserInfo>(lstUser);
            simplePager1.RefreshPager(5, dtUser.Rows.Count, 1);
            //for (int i = 0; i < lstUser.Count; i++)
            //{
            //    lstUserRole = (List<UserroleInfo>)ServiceFactory.CreateService<IUserRoleService>().GetUserRoles(lstUser[i].UserID);
            //    if (lstUserRole.Count > 0)
            //    {
            //        string str = ServiceFactory.CreateService<IRoleService>().GetById(lstUserRole[0].RoleID).RoleName;

            //        UserGridView.SetRowCellValue(i, "RoleName", str);                     
            //    }
            //} 
        }

        /// <summary>
        /// 收据数据，用于删除菜单对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private UserInfo GetData()
        {
            UserInfo user = new UserInfo();
            object obj = UserGridView.GetFocusedRow();
            user = (UserInfo)obj;
            user.InfoState = InfoState.Delete;
            return user;
        }
        #endregion


    }

    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> list)
        {

            //创建属性的集合    
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口    

            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列    
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例    
                DataRow row = dt.NewRow();
                //给row 赋值    
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable    
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}