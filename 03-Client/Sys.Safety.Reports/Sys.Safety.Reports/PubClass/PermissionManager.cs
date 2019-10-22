using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using Sys.Safety.ClientFramework.UserRoleAuthorize;

namespace Sys.Safety.Reports.PubClass
{
    public class PermissionManager
    {
        /// <summary>
        /// 判断权限方法
        /// </summary>
        /// <param name="strRightCode">权限编码</param>
        /// <param name="button">工具栏按钮</param>       
        /// <returns>有权限返回true,否则返回false</returns>
        public static bool HavePermission(string strRightCode,object button)
        {
         
            bool blnHaveRight =  ClientPermission.Authorize(strRightCode);
            if(blnHaveRight) 
                return true;           
            string strPermissType = RequestUtil.GetParameterValue("strPermissType");//如果为1表示是显示，为2则是提示，为2的情况是调用下面的重载方法进行判断
            bool blnContrilRight = TypeUtil.ToBool(System.Configuration.ConfigurationManager.AppSettings["BlnControlRight"]);
            if (!blnContrilRight)
                strPermissType = "1";

            if (strPermissType == "")
                return true;
           
            if (strPermissType == "1")
            {
                if (button.GetType().Name == "BarButtonItem")
                {
                    ((BarButtonItem)button).Visibility = BarItemVisibility.Never;               
                }
                //if (button.GetType().Name == "RepositoryItemButtonEdit")
                //{
                //    ((RepositoryItemButtonEdit)button).Buttons[0].Visible = false;
                //}
                if (button.GetType().Name == "SimpleButton")
                {//这样还是会显示，因为此控件包括了一个LayoutControlItem
                    ((SimpleButton)button).Visible = false;                
                }
                
            }           
            return false;
        }   

        /// <summary>
        /// 判断权限方法
        /// </summary>
        /// <param name="strRightCode">权限编码</param>
        /// <returns>有权限返回true,否则返回false</returns>
        public static bool HavePermission(string strRightCode)
        {
           
            bool blnHaveRight = ClientPermission.Authorize(strRightCode);
            if (blnHaveRight)
                return true;
            string strPermissType = RequestUtil.GetParameterValue("strPermissType");
            bool blnContrilRight = TypeUtil.ToBool(System.Configuration.ConfigurationManager.AppSettings["BlnControlRight"]);
            if (!blnContrilRight)
                strPermissType = "2";
            if (strPermissType == "")
                return true;
            if (strPermissType == "2")
            {
                MessageShowUtil.ShowMsg("无此操作权限，请无越权操作！");
                return false;
            }
            return false;
        }
    }
}
