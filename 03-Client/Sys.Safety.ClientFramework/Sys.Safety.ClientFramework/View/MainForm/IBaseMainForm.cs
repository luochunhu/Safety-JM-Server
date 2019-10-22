using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sys.Safety.ClientFramework.View.MainForm
{
    /// <summary>
    /// IBaseMainForm
    /// 主窗口的接口 
    /// 
    /// 版本：V1.1.0.2
    /// 
    /// <作者>
    ///    <名字>基础技术组</名字>
    ///    <创建日期>2015.01.08</创建日期>
    /// </作者>
    /// </summary>
    public interface IBaseMainForm
    {
        /// <summary>
        /// 初始化窗体
        /// </summary>
        void InitForm();

        /// <summary>
        /// 初始化服务
        /// </summary>
        void InitService();

        /// <summary>
        /// 检查菜单
        /// </summary>
        void CheckMenu();
    }
}
