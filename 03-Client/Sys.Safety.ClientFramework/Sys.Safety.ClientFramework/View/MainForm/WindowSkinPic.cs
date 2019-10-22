using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Sys.Safety.ClientFramework.Properties;

namespace Sys.Safety.ClientFramework.View.MainForm
{
    /// <summary>
    /// WindowSkinPic
    /// 主窗口的皮肤图片
    /// 
    /// 版本：V1.1.0.2
    /// 
    /// <作者>
    ///    <名字>基础技术组</名字>
    ///    <创建日期>2015.01.08</创建日期>
    /// </作者>
    /// </summary>
    internal class WindowSkinPic
    {
        #region ====================窗体属性====================
        /// <summary>快捷按钮图片路径</summary>
        private readonly string _superDir = Application.StartupPath + @"\Image\WorkImage\";

        /// <summary>窗体顶部背景图片</summary>
        private Image _mainHeadPanle;

        /// <summary>窗体logo图片</summary>
        private Image _logoPicBox;

        /// <summary>窗体动态菜单加载区的背景图片</summary>
        private Image _mainMenuPanel;

        /// <summary>窗体快捷菜单背景色</summary>
        private Image _menuStripBk;

        /// <summary>窗体关闭按钮背景图片</summary>
        private Image _closePicbox;

        /// <summary系统logo背景图片</summary>
        private Icon _systemLogo;
        
        /// <summary>窗体关闭按钮背景图片-鼠标移动</summary>
        private Image _closePicboxMouse;

        /// <summary>窗体最大化按钮背景图片</summary>
        private Image _maxFormPicbox;

        /// <summary>窗体最大化按钮背景图片-鼠标移动</summary>
        private Image _maxFormPicboxMouse;

        /// <summary>窗体最小化按钮背景图片</summary>
        private Image _minFormPicbox;

        /// <summary>窗体最小化按钮背景图片-鼠标移动</summary>
        private Image _minFormPicboxMouse;

        /// <summary>窗体皮肤按钮背景图片</summary>
        private Image _skinPicbox;

        /// <summary>窗体皮肤按钮背景图片-鼠标移动</summary>
        private Image _skinPicboxMouse;

        /// <summary>快捷按钮的图片</summary>
        private Dictionary<string, Image> _suBtnImage;

        /// <summary>快捷按钮鼠标移动上去的Image</summary>
        private Image _sBtnMouseEnter;

        /// <summary>快捷按钮点击时的Image</summary>
        private Image _sBtnMouseClick;
        #endregion

        #region ====================公共属性====================
        /// <summary>快捷按钮图标的默认路径</summary>
        public string SuperDir
        {
            get { return this._superDir; }
        }

        /// <summary>窗体顶部背景图片</summary>
        public Image mainHeadPanle
        {
            get { return this._mainHeadPanle; }
            set { this._mainHeadPanle = value; }
        }

        /// <summary>窗体logo图片</summary>
        public Image logoPicBox
        {
            get { return this._logoPicBox; }
            set { this._logoPicBox = value; }
        }

        /// <summary>窗体动态菜单加载区的背景图片</summary>
        public Image mainMenuPanel
        {
            get { return this._mainMenuPanel; }
            set { this._mainMenuPanel = value; }
        }

        /// <summary>窗体快捷菜单背景色</summary>
        public Image menuStripBk
        {
            get { return this._menuStripBk; }
            set { this._menuStripBk = value; }
        }

        /// <summary>窗体关闭按钮背景图片</summary>
        public Image closePicbox
        {
            get { return this._closePicbox; }
            set { this._closePicbox = value; }
        } 

        /// <summary>系统logo背景图片</summary>
        public Icon SystemLogo
        {
            get { return this._systemLogo; }
            set { this._systemLogo = value; }
        }
        /// <summary>窗体关闭按钮背景图片-鼠标移动</summary>
        public Image closePicboxMouse
        {
            get { return this._closePicboxMouse; }
            set { this._closePicboxMouse = value; }
        }

        /// <summary>窗体最大化按钮背景图片</summary>
        public Image maxFormPicbox
        {
            get { return this._maxFormPicbox; }
            set { this._maxFormPicbox = value; }
        }

        /// <summary>窗体最大化按钮背景图片-鼠标移动</summary>
        public Image maxFormPicboxMouse
        {
            get { return this._maxFormPicboxMouse; }
            set { this._maxFormPicboxMouse = value; }
        }

        /// <summary>窗体最小化按钮背景图片</summary>
        public Image minFormPicbox
        {
            get { return this._minFormPicbox; }
            set { this._minFormPicbox = value; }
        }

        /// <summary>窗体最小化按钮背景图片-鼠标移动</summary>
        public Image minFormPicboxMouse
        {
            get { return this._minFormPicboxMouse; }
            set { this._minFormPicboxMouse = value; }
        }

        /// <summary>窗体皮肤按钮背景图片</summary>
        public Image skinPicbox
        {
            get { return this._skinPicbox; }
            set { this._skinPicbox = value; }
        }

        /// <summary>窗体皮肤按钮背景图片-鼠标移动</summary>
        public Image skinPicboxMouse
        {
            get { return this._skinPicboxMouse; }
            set { this._skinPicboxMouse = value; }
        }

        /// <summary>快捷按钮的背景图片集合</summary>
        public Dictionary<string, Image> SuBtnImage
        {
            get { return this._suBtnImage; }
        }

        /// <summary>快捷按钮鼠标移动上去的图片</summary>
        public Image SBtnMouseEnter
        {
            get { return this._sBtnMouseEnter; }
            private set { this._sBtnMouseEnter = value; }
        }

        /// <summary>快捷按钮点击时的Image</summary>
        public Image SBtnMouseClick
        {
            get { return this._sBtnMouseClick; }
            private set { this._sBtnMouseClick = value; }
        }

        /// <summary>快捷按钮默认的Image</summary>
        public List<Image> DefaultBtnImage;
        #endregion

        #region ====================公共方法====================
        public WindowSkinPic(string formTitle = "")
        {
            mainHeadPanle = Resources.mainHeadBk;//头部背景
            logoPicBox = Resources.logoBk;//logo图片
            mainMenuPanel = Resources.menuHeadBk;
            _systemLogo = Resources.Form;
            menuStripBk = Resources.menuStripBk;//菜单背景
            closePicbox = Resources.closeBtnNorBk;//关闭按钮背景色
            _closePicboxMouse = Resources.closeBtnMouseBk;//关闭按钮鼠标
            maxFormPicbox = Resources.maxBtnNorBk;//最大化按钮
            maxFormPicboxMouse = Resources.maxBtnMouseBk;
            minFormPicbox = Resources.minBtnNorBk;//最小化按钮
            minFormPicboxMouse = Resources.minBtnMouseBk;
            skinPicbox = Resources.skinBtnNorBk;//换肤按钮
            skinPicboxMouse = Resources.skinBtnMouseBk;
            SBtnMouseEnter = Resources.superMenuOnMouseBk;
            SBtnMouseClick = Resources.superMenuOnMouseBk;

            ReadSuBtnImage();
            
            DefaultBtnImage = new List<Image>();
            DefaultBtnImage.Add(Resources.superMenuBtnBk_0);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_1);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_2);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_3);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_4);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_5);
            DefaultBtnImage.Add(Resources.superMenuBtnBk_6);
        }

        /// <summary>
        /// 根据图片名称获取快捷按钮的图片
        /// </summary>
        /// <param name="imageName">图片名称</param>
        /// <returns>图片，如果不存在，返回null</returns>
        public Image GetSuBtnImageByName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName) || null == SuBtnImage || !SuBtnImage.ContainsKey(imageName)) { return null; }

            return SuBtnImage[imageName];
        }

        /// <summary>
        /// 读取所有的快捷按钮的图片文件
        /// </summary>
        public void ReadSuBtnImage()
        {
            if (!Directory.Exists(_superDir)) { return; }

            DirectoryInfo dir = new DirectoryInfo(_superDir);

            FileInfo[] images = dir.GetFiles();

            if (null == images || images.Length < 1) { return; }

            if (null == _suBtnImage) { _suBtnImage = new Dictionary<string, Image>(); }
            else { _suBtnImage.Clear(); }

            foreach (FileInfo fi in images)
            {
                try
                {
                    if (null == fi||!fi.Extension.ToLower().Contains("png")) { continue; }
                    
                    _suBtnImage.Add(fi.Name, Image.FromFile(fi.FullName));
                }
                finally { }
            }
        }
        #endregion
    }
}
