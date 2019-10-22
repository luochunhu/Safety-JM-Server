using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Client.Alarm
{
    public partial class frmViewAnalysisAlarm : DevExpress.XtraEditors.XtraForm
    {
        private volatile static frmViewAnalysisAlarm _instance = null;
        private static readonly object lockHelper = new object();
        [DllImport("user32")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private const int AW_ACTIVE = 0x20000;//激活窗口,在应用了AW_HIDE标记后不要应用这个标记
        private const int AW_SLIDE = 0x40000;//应用滑动类型动画结果,默认为迁移转变动画类型,当应用AW_CENTER标记时,这个标记就被忽视
        private const int AW_VER_NEGATIVE = 0x0008;//自下向上显示窗口,该标记可以在迁移转变动画和滑动动画中应用。应用AW_CENTER标记时忽视该标记该标记
        private const int AW_BLEND = 0x80000;//应用淡入淡出结果
        private const int AW_HIDE = 0x10000;//隐蔽窗口
        private frmViewAnalysisAlarm()
        {
            InitializeComponent();
        }
        public static frmViewAnalysisAlarm Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockHelper)
                    {
                        if (_instance == null)
                        {
                            _instance = new frmViewAnalysisAlarm();
                        }
                    }
                }
                return _instance;
            }
        }

        public new void Show()
        {
            int w = this.Width;
            int h = this.Height;
            this.Width = 0;
            this.Height = 0;
            base.Show();
            this.Visible = false;
            this.Width = w;
            this.Height = h;
            int x = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;           
            this.Location = new Point(x, y);//设置窗体在屏幕右下角显示
            listBoxControl1.DataSource = GetDataSource();
            AnimateWindow(this.Handle, 1000, AW_SLIDE | AW_ACTIVE | AW_VER_NEGATIVE);
        }

        private void FrmViewAnalysisAlarm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            AnimateWindow(this.Handle, 1000, AW_BLEND | AW_HIDE);
            e.Cancel = true;
            this.Visible = false;
        }

        private List<string> GetDataSource()
        {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(DisplayText))
            {
                string[] displayItems = DisplayText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                result = displayItems.ToList();
            }
            return result;
        }

        public string DisplayText
        {
            get; set;
        }
    }
}
