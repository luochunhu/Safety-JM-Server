using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using Sys.Safety.DataContract;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request;
using Basic.Framework.Logging;
using Sys.Safety.ClientFramework.UserRoleAuthorize;
using DevExpress.Utils;
using System.Threading;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.ClientFramework.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sys.Safety.ClientFramework.CBFCommon
{

    /// <summary>
    /// 获取系统启动参数类
    /// </summary>
    public class RequestUtil
    {
        static IRequestService requestService = ServiceFactory.Create<IRequestService>();
        /// <summary>
        /// 得到系统配置表参照值
        /// </summary>
        /// <param name="strKey">传入Key</param>
        /// <returns>返回Value</returns>
        public static string GetParameterValue(string strKey)
        {
            // IDictionary<string, string> dic = ClientContext.Current.GetContext("CustomerSetting") as Dictionary<string, string>;
            IDictionary<string, string> dic = Basic.Framework.Common.JSONHelper.ParseJSONString<Dictionary<string, string>>(Basic.Framework.Data.PlatRuntime.Items["CustomerSetting"].ToString());
            if (dic == null)
                return "";
            if (dic.ContainsKey(strKey))
                return dic[strKey];
            return "";
        }

        /// <summary>历史打开的窗体对象</summary>
        public static List<object> formObjects = new List<object>();

        #region ====================外部接口====================
        /// <summary>
        /// 定义委托
        /// </summary>
        /// <param name="objFrm"></param>
        public delegate void ShowToMainForm(Form objFrm, RequestInfo dto, bool isSystemDesktop);
        /// <summary>
        /// 添加窗体触发
        /// </summary>
        public static ShowToMainForm OnShowToMainForm;
        [DllImport("kernel32.dll")]
        //uCmdShow 参数可选值:
        //SW_HIDE            = 0; {隐藏, 并且任务栏也没有最小化图标}
        //SW_SHOWNORMAL      = 1; {用最近的大小和位置显示, 激活}
        //SW_NORMAL          = 1; {同 SW_SHOWNORMAL}
        //SW_SHOWMINIMIZED   = 2; {最小化, 激活}
        //SW_SHOWMAXIMIZED   = 3; {最大化, 激活}
        //SW_MAXIMIZE        = 3; {同 SW_SHOWMAXIMIZED}
        //SW_SHOWNOACTIVATE  = 4; {用最近的大小和位置显示, 不激活}
        //SW_SHOW            = 5; {同 SW_SHOWNORMAL}
        //SW_MINIMIZE        = 6; {最小化, 不激活}
        //SW_SHOWMINNOACTIVE = 7; {同 SW_MINIMIZE}
        //SW_SHOWNA          = 8; {同 SW_SHOWNOACTIVATE}
        //SW_RESTORE         = 9; {同 SW_SHOWNORMAL}
        //SW_SHOWDEFAULT     = 10; {同 SW_SHOWNORMAL}
        //SW_MAX             = 10; {同 SW_SHOWNORMAL}
        public static extern int WinExec(string exeName, int operType);
        /// <summary>
        /// 定义委托通知刷新报表数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void OnRefreshReport(object sender, object args);
        /// <summary>
        /// 定义事件通知刷新报表数据
        /// </summary>
        public static OnRefreshReport OnRefreshReportEvent;

        /// <summary>
        /// 主控退出委托定义
        /// </summary>
        public delegate void OnMainFormClose();
        /// <summary>
        /// 主控退出委托方法
        /// </summary>
        public static OnMainFormClose OnMainFormCloseEvent;
        /// <summary>
        /// 切换主控窗口委托
        /// </summary>
        public delegate void OnMainTabChange(string MenuName);
        /// <summary>
        /// 切换主控窗口委托方法
        /// </summary>
        public static OnMainTabChange OnMainTabChangeEvent;
        /// <summary>
        /// 主控子窗口退出委托
        /// </summary>
        public delegate void OnSunFormClose();
        /// <summary>
        /// 主控子窗口退出委托方法
        /// </summary>
        public static OnSunFormClose OnSunFormCloseEvent;


        #endregion
        /// <summary>
        /// 执行请求库
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="formParams"></param>
        public static void ExcuteCommand(string requestCode, Dictionary<string, string> formParams, bool isSystemDesktop)
        {
            try
            {
                RequestInfo dto = null;
                if (string.IsNullOrEmpty(requestCode))
                {
                    return;
                }
                try
                {
                    if (!string.IsNullOrEmpty(requestCode))
                    {
                        RequestGetByCodeRequest requestrequest = new RequestGetByCodeRequest();
                        requestrequest.Code = requestCode;
                        var result = requestService.GetRequestByCode(requestrequest);
                        if (result != null)
                        {
                            dto = result.Data;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message + ex.StackTrace);
                    DevExpress.XtraEditors.XtraMessageBox.Show("通道异常，请检查网络及配置是否正确！");
                    return;
                }
                //判断当前请求是否为主控请求，如果是主控请求，则判断当前主机是否有主控权限 
                if (dto.BZ2 == "1")//主控请求
                {
                    int resultIsMaster = MasterManagement.IsMaster();//等于0，表示正常
                    if (resultIsMaster == 1)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("当前非主控电脑,请确认本机是否为主控并检查本机网络是否正常！");
                        return;
                    }
                    if (resultIsMaster == 2)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("连接服务器失败,请检查网络是否正常！");
                        return;
                    }
                    if (resultIsMaster == 3)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("获取当前主机是否为主控主机失败，详细见日志！");
                        return;
                    }
                    Sys.Safety.ClientFramework.View.LogOn.frmLogOn loginForm = new View.LogOn.frmLogOn(false);
                    //loginForm.ShowInTaskbar = false;
                    loginForm.ShowDialog();
                    if (!LoginManager.isLoginSuccess)//登录不成功
                    {
                        return;
                    }
                }
                if (null != dto && dto.RequestID.Length > 0)
                {


                    if (!Sys.Safety.ClientFramework.UserRoleAuthorize.LoginManager.IsLogin)//判断是否登录,如果未登录则将当前窗体置成登录窗口
                    {
                        if (!string.IsNullOrEmpty("RequestUserLogOn"))
                        {
                            //dto = reqSvr.GetRequest("RequestUserLogOn");
                            RequestGetByCodeRequest requestrequest = new RequestGetByCodeRequest();
                            requestrequest.Code = "RequestUserLogOn";
                            var result = requestService.GetRequestByCode(requestrequest);
                            if (result != null)
                            {
                                dto = result.Data;
                            }
                            formParams = null;
                        }
                    }

                    string strDllname = Convert.ToString(dto.MenuFile);

                    if (string.IsNullOrEmpty(strDllname))//如果未设置加载的文件，就直接退出函数
                    {
                        return;
                    }

                    object[] obj = null;//参数
                    if (formParams != null && formParams.Count > 0)
                    {
                        obj = new object[1];
                        obj[0] = formParams;
                    }
                    if (dto.MenuForSys.ToString() == "1")//打开exe程序
                    {
                        #region  打开exe程序,并实现窗体参数传递

                        try
                        {
                            string StrModuleParams = "";
                            if (obj != null)
                            {
                                if (obj.Length > 0)
                                {
                                    Dictionary<string, string> tempParam = obj[0] as Dictionary<string, string>;
                                    foreach (KeyValuePair<string, string> kvp in tempParam)
                                    {
                                        StrModuleParams += kvp.Key + "=" + kvp.Value + " ";
                                    }
                                }
                            }
                            //增加当前登录用户参数传递  20170714
                            ClientItem clientItem = new ClientItem();
                            if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                            {
                                clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                            }
                            StrModuleParams += "userName=" + clientItem.UserName + " ";
                            StrModuleParams += "userID=" + clientItem.UserID + " ";
                            if (!string.IsNullOrEmpty(BaseInfo.WindowStyleNow))
                            {
                                StrModuleParams += "WindowStyle=" + BaseInfo.WindowStyleNow + " ";
                            }
                            if (!string.IsNullOrEmpty(dto.MenuURL))//传需要打开的窗体参数  20170924
                            {
                                StrModuleParams += "menuURL=" + dto.MenuURL + " ";
                            }

                            if (!strDllname.Contains("Sys.Safety.Client.Graphic") && !strDllname.Contains("Sys.Safety.Client.Chart"))//除GIS图形，chart曲线外，其它模块不允许多开  20170715
                            {
                                string processName = strDllname.Substring(0, strDllname.LastIndexOf("."));
                                Process[] process = Process.GetProcessesByName(processName);
                                if (process.Length > 0)
                                {
                                    for (int k = 0; k < process.Length; k++)//关闭所有的应用程序
                                    {
                                        process[k].Kill();
                                    }
                                }
                            }

                            strDllname = Application.StartupPath + "\\" + strDllname;

                            //WinExec(strDllname + StrModuleParams, 1);
                            WaitDialogForm wdf = new WaitDialogForm("正在加载数据...", "请等待...");


                            //Process myprocess = new Process();
                            //ProcessStartInfo startInfo = new ProcessStartInfo(strDllname, StrModuleParams);
                            ////设置启动动作,确保以管理员身份运行
                            //startInfo.Verb = "runas";
                            //myprocess.StartInfo = startInfo;
                            //myprocess.Start();
                            LogHelper.Debug(DateTime.Now);
                            WinExec(@"""" + strDllname + @""" " + StrModuleParams + @"", 1);

                            Thread.Sleep(3000);

                            if (wdf != null)
                            {
                                wdf.Close();
                            }
                            return;
                        }
                        catch (Exception)
                        {
                            return;
                        }

                        #endregion
                    }
                    else if (dto.MenuForSys.ToString() == "0")//反射加载窗体
                    {
                        #region 加载窗体
                        string MenuName = "";
                        #region 获取当前菜单名称作为窗口的名称
                        RequestGetMenuByCodeRequest operatelogrequest = new RequestGetMenuByCodeRequest();
                        operatelogrequest.Code = requestCode;
                        var result = requestService.GetRequestMenuByCode(operatelogrequest);
                        DataTable menudt = result.Data;
                        if (menudt.Rows.Count > 0)
                        {
                            MenuName = menudt.Rows[0]["menuname"].ToString();
                        }
                        #endregion

                        Form TempForm = null;
                        //判断窗体缓存对象中是否存在Form对象,如果存在，直接用缓存对象
                        foreach (Form form in formObjects)
                        {
                            if (form.Text == MenuName && dto.LoadByIframe == 0)//仅打开一次 
                            {
                                TempForm = form;
                                break;
                            }
                        }
                        if (TempForm == null)
                        {
                            #region 反射方式动态加载窗体文件，并实现窗体参数传递

                            string fileName = Application.StartupPath + "\\" + dto.MenuFile;//dll文件
                            string UrlName = Convert.ToString(dto.MenuNamespace) + "." + Convert.ToString(dto.MenuURL); //类名称              
                            object Temp_Obj = null;
                            bool isReload = false;
                            Temp_Obj = AssemblyManager.CreateInstance(fileName,
                                 UrlName, obj, ref isReload);
                            if (Temp_Obj != null && !isReload)
                            {
                                //调用窗体
                                TempForm = Temp_Obj as Form;
                                //设置默认图标
                                string MyFileName = Application.StartupPath + "\\Image\\Icon\\客户端.ico";
                                if (File.Exists(MyFileName))
                                {
                                    TempForm.Icon = new Icon(MyFileName);
                                }
                                TempForm.Text = MenuName;
                                TempForm.FormClosed += new FormClosedEventHandler(TempForm_FormClosed);
                                //TempForm.FormClosing += new FormClosingEventHandler(TempForm_FormClosing);
                            }
                            else
                            {
                                //调用窗体
                                TempForm = Temp_Obj as Form;
                            }
                            #endregion
                        }
                        //委托更新主控
                        if (null != TempForm)
                        {
                            if (null != OnShowToMainForm)
                            {
                                OnShowToMainForm(TempForm, dto, isSystemDesktop);
                            }
                        }
                        #endregion
                    }
                    else if (dto.MenuForSys.ToString() == "2")//反射执行方法
                    {
                        #region 调用方法
                        string fileName = Application.StartupPath + "\\" + dto.MenuFile;//dll文件
                        string UrlName = Convert.ToString(dto.MenuNamespace) + "." + Convert.ToString(dto.MenuURL); //类名称            

                        AssemblyManager.CreateInstance(fileName,
                             UrlName, obj, dto.BZ1);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RequestUtil_TempForm_FormClosed" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 当业务窗体关闭时，像指定的列表窗体发送事件通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void TempForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //响应子窗口关闭事件，将主窗体置到最前
                if (OnSunFormCloseEvent != null)
                {
                    OnSunFormCloseEvent();
                }
                Form temp = (Form)sender;
                //注销内存对象
                for (int j = RequestUtil.formObjects.Count - 1; j >= 0; j--)
                {
                    if (((Form)(RequestUtil.formObjects[j])).Text == temp.Text || string.IsNullOrEmpty(((Form)(RequestUtil.formObjects[j])).Text))
                    {
                        //移除对象、释放资源
                        ((Form)(RequestUtil.formObjects[j])).Dispose();
                        RequestUtil.formObjects[j] = null;
                        RequestUtil.formObjects.RemoveAt(j);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("RequestUtil_ExcuteCommand" + ex.Message + ex.StackTrace);
            }
            if (OnRefreshReportEvent != null)
            {
                OnRefreshReportEvent(null, null);
            }
        }
        private static void TempForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form)sender).Visible = false;
            e.Cancel = true;
        }
        /// <summary>
        /// 主控关闭时，向子窗口发送关闭事件通知
        /// </summary>
        public static void OnMainFormCloseing()
        {
            //退出时，查找所有请求库，把调用的执行文件结束
            try
            {
                List<RequestInfo> AllRequest = requestService.GetRequestList().Data;
                foreach (RequestInfo request in AllRequest)
                {
                    if (request.MenuForSys == 1)//如果是打开执行文件，则关闭执行文件  20170720
                    {
                        string processName = request.MenuFile.Substring(0, request.MenuFile.LastIndexOf('.'));
                        if (processName.Contains("\\"))
                        {
                            processName = processName.Substring(processName.LastIndexOf('\\') + 1);
                        }

                        Process[] process = Process.GetProcessesByName(processName);
                        for (int k = 0; k < process.Length; k++)//关闭所有的应用程序
                        {
                            process[k].Kill();
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            if (OnMainFormCloseEvent != null)
            {
                OnMainFormCloseEvent();
            }
        }
        /// <summary>
        ///  调用主控，执行菜单切换
        /// </summary>
        /// <param name="menuname">窗体对应的菜单名称</param>
        public static void DoMainTabChange(string menuname)
        {
            if (OnMainTabChangeEvent != null)
            {
                OnMainTabChangeEvent(menuname);
            }
        }


    }
}
