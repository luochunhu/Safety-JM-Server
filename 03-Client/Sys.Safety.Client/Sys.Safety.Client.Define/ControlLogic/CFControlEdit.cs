using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Sys.Safety.Client.Define.Model;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Basic.Framework.Logging;

namespace Sys.Safety.Client.Define.ControlLogic
{
    public partial class CFControlEdit : XtraForm
    {
        ///// <summary>
        ///// 默认构造函数
        ///// </summary>
        public CFControlEdit()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            InitializeComponent();
        }


        private void CFControlLogic_Load(object sender, EventArgs e)
        {

        }

        #region =========================事件函数===========================
        
        #endregion

        #region =========================业务函数===========================
        /// <summary> 加载信息
        /// </summary>
        private void LoadBasicInf()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.Error("加载基础信息【LoadBasicInf】", ex);
            }
        }
        /// <summary> 加载默认的初始信息
        /// </summary>
        private void LoadPretermitInf()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                LogHelper.Error("加载默认的初始信息【LoadPretermitInf】", ex);
            }
        }
        #endregion
    }
}
