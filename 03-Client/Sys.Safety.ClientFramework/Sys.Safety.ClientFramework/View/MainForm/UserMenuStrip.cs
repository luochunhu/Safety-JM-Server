using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Sys.Safety.ClientFramework.View.MainForm
{
    /// <summary>建立圆角路径的样式</summary>
    public enum RoundStyle
    {
        /// <summary>四个角都不是圆角</summary>
        None = 0,

        /// <summary>四个角都为圆角</summary>
        All = 1,

        /// <summary>左边两个角为圆角</summary>
        Left = 2,

        /// <summary>右边两个角为圆角</summary>
        Right = 3,

        /// <summary>上边两个角为圆角</summary>
        Top = 4,

        /// <summary>下边两个角为圆角</summary>
        Bottom = 5,

        /// <summary>左下角为圆角</summary>
        BottomLeft = 6,

        /// <summary>右下角为圆角</summary>
        BottomRight = 7,
    }

    /// <summary>创建圆角路径样式</summary>
    public static class GraphicsPathHelper
    {
        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePath(Rectangle rect, int radius, RoundStyle style, bool correction = true)
        {
            GraphicsPath path = new GraphicsPath();

            int radiusCorrection = correction ? 1 : 0;

            #region
            switch (style)
            {
                case RoundStyle.None://普通矩形
                    path.AddRectangle(rect);
                    break;
                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);

                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);

                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);

                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);
                    break;
                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);

                    path.AddLine(rect.Right - radiusCorrection, rect.Y, rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);

                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);
                    break;
                case RoundStyle.Right:
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);

                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);

                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    break;
                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);

                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Y, radius, radius, 270, 90);

                    path.AddLine(rect.Right - radiusCorrection, rect.Bottom - radiusCorrection, rect.X, rect.Bottom - radiusCorrection);
                    break;
                case RoundStyle.Bottom:
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);

                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);

                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
                case RoundStyle.BottomLeft:
                    path.AddArc(rect.X, rect.Bottom - radius - radiusCorrection, radius, radius, 90, 90);

                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);

                    path.AddLine(rect.Right - radiusCorrection, rect.Y, rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
                    break;
                case RoundStyle.BottomRight:
                    path.AddArc(rect.Right - radius - radiusCorrection, rect.Bottom - radius - radiusCorrection, radius, radius, 0, 90);

                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);

                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
            }
            #endregion

            path.CloseFigure();

            return path;
        }
    }

    /// <summary>控件区域路径绘制</summary>
    public static class RegionHelper
    {
        /// <summary>
        /// 绘制控件的区域路径
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="bounds">矩形区域</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="roundStyle">圆角样式</param>
        public static void CreateRegion(Control control, Rectangle bounds, int radius, RoundStyle roundStyle)
        {
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(bounds, radius, roundStyle, true))
            {
                Region region = new Region(path);

                path.Widen(Pens.White);

                region.Union(path);

                if (null != control.Region) { control.Region.Dispose(); }

                control.Region = region;
            }
        }

        /// <summary>
        /// 绘制控件的区域路径
        /// 默认四个圆角，均为8
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="bounds">矩形区域</param>
        public static void CreateRegion(Control control, Rectangle bounds)
        {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }
    }

    /// <summary>图像插值算法适配器</summary>
    public class InterpolationModeGraphics : IDisposable
    {
        /// <summary>图像转换插值算法枚举</summary>
        private InterpolationMode _oldMode;

        /// <summary>绘图句柄</summary>
        private Graphics _graphics;

        /// <summary>默认构造函数，默认采用最高的插值算法</summary>
        public InterpolationModeGraphics(Graphics graphics)
            : this(graphics, InterpolationMode.HighQualityBicubic)
        {
        }

        /// <summary>构造函数，设置新的插值模式</summary>
        public InterpolationModeGraphics(Graphics graphics, InterpolationMode newMode)
        {
            _graphics = graphics;

            _oldMode = graphics.InterpolationMode;

            graphics.InterpolationMode = newMode;
        }

        #region IDisposable 成员
        /// <summary>恢复Graph旧的插值算法</summary>
        public void Dispose()
        {
            _graphics.InterpolationMode = _oldMode;
        }

        #endregion
    }

    /// <summary>图像平滑适配器</summary>
    public class SmoothingModeGraphics : IDisposable
    {
        /// <summary>平滑处理模式</summary>
        private SmoothingMode _oldMode;

        /// <summary>GDI</summary>
        private Graphics _graphics;

        /// <summary>默认构造函数，抗锯齿</summary>
        public SmoothingModeGraphics(Graphics graphics)
            : this(graphics, SmoothingMode.AntiAlias)
        {
        }

        /// <summary>构造函数</summary>
        public SmoothingModeGraphics(Graphics graphics, SmoothingMode newMode)
        {
            _graphics = graphics;
            _oldMode = graphics.SmoothingMode;
            graphics.SmoothingMode = newMode;
        }

        #region IDisposable 成员
        /// <summary>恢复初始的平滑模式</summary>
        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }

        #endregion
    }

    public sealed class ControlPaintEx
    {
        public static void DrawCheckedFlag(Graphics graphics, Rectangle rect, Color color)
        {
            PointF[] points = new PointF[3];

            points[0] = new PointF(rect.X + rect.Width / 4.5f, rect.Y + rect.Height / 2.5f);

            points[1] = new PointF(rect.X + rect.Width / 2.5f, rect.Bottom - rect.Height / 3f);

            points[2] = new PointF(rect.Right - rect.Width / 4.0f, rect.Y + rect.Height / 4.5f);

            using (Pen pen = new Pen(color, 2F)) { graphics.DrawLines(pen, points); }
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, Color glassColor, int alphaCenter, int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);

                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);

                    brush.SurroundColors = new Color[] { Color.FromArgb(alphaSurround, glassColor) };

                    brush.CenterPoint = new PointF(glassRect.X + glassRect.Width / 2, glassRect.Y + glassRect.Height / 2);

                    g.FillPath(brush, path);
                }
            }
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset, RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor, ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset, RightToLeft rightToLeft)
        {
            if (g == null) { throw new ArgumentNullException("g"); }

            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (TextureBrush brush = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    if (scrollOffset != Point.Empty)
                    {
                        Matrix transform = brush.Transform;

                        transform.Translate((float)scrollOffset.X, (float)scrollOffset.Y);

                        brush.Transform = transform;
                    }

                    g.FillRectangle(brush, clipRect);
                    return;
                }
            }

            Rectangle rect = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);

            if ((rightToLeft == RightToLeft.Yes) && (backgroundImageLayout == ImageLayout.None)) { rect.X += clipRect.Width - rect.Width; }

            using (SolidBrush brush2 = new SolidBrush(backColor)) { g.FillRectangle(brush2, clipRect); }

            if (!clipRect.Contains(rect))
            {
                if ((backgroundImageLayout == ImageLayout.Stretch) || (backgroundImageLayout == ImageLayout.Zoom))
                {
                    rect.Intersect(clipRect);
                    g.DrawImage(backgroundImage, rect);
                }
                else if (backgroundImageLayout == ImageLayout.None)
                {
                    rect.Offset(clipRect.Location);
                    Rectangle destRect = rect;
                    destRect.Intersect(clipRect);
                    Rectangle rectangle3 = new Rectangle(Point.Empty, destRect.Size);
                    g.DrawImage(backgroundImage, destRect, rectangle3.X, rectangle3.Y, rectangle3.Width, rectangle3.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    Rectangle rectangle4 = rect;
                    rectangle4.Intersect(clipRect);
                    Rectangle rectangle5 = new Rectangle(new Point(rectangle4.X - rect.X, rectangle4.Y - rect.Y), rectangle4.Size);
                    g.DrawImage(backgroundImage, rectangle4, rectangle5.X, rectangle5.Y, rectangle5.Width, rectangle5.Height, GraphicsUnit.Pixel);
                }
            }
            else
            {
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(backgroundImage, rect, 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, imageAttr);
                imageAttr.Dispose();
            }
        }

        internal static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage, ImageLayout imageLayout)
        {
            Rectangle rectangle = bounds;

            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        rectangle.Size = backgroundImage.Size;
                        return rectangle;
                    case ImageLayout.Tile:
                        return rectangle;
                    case ImageLayout.Center:
                        {
                            rectangle.Size = backgroundImage.Size;
                            Size size = bounds.Size;
                            if (size.Width > rectangle.Width) { rectangle.X = (size.Width - rectangle.Width) / 2; }
                            if (size.Height > rectangle.Height) { rectangle.Y = (size.Height - rectangle.Height) / 2; }
                            return rectangle;
                        }
                    case ImageLayout.Stretch:
                        rectangle.Size = bounds.Size;
                        return rectangle;
                    case ImageLayout.Zoom:
                        {
                            Size size2 = backgroundImage.Size;
                            float num = ((float)bounds.Width) / ((float)size2.Width);
                            float num2 = ((float)bounds.Height) / ((float)size2.Height);
                            if (num >= num2)
                            {
                                rectangle.Height = bounds.Height;
                                rectangle.Width = (int)((size2.Width * num2) + 0.5);
                                if (bounds.X >= 0) { rectangle.X = (bounds.Width - rectangle.Width) / 2; }
                                return rectangle;
                            }
                            rectangle.Width = bounds.Width;
                            rectangle.Height = (int)((size2.Height * num) + 0.5);
                            if (bounds.Y >= 0) { rectangle.Y = (bounds.Height - rectangle.Height) / 2; }
                            return rectangle;
                        }
                    default:
                        break;
                }
            }
            return rectangle;
        }
    }

    internal class RenderHelper
    {
        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, 8, drawBorder, drawGlass, mode);
        }

        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, 8, 0.45f, drawBorder, drawGlass, mode);
        }

        internal static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor, Color innerBorderColor, RoundStyle style, int roundWidth, float basePosition, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            if (drawBorder) { --rect.Width; --rect.Height; }

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 35, 24, 9);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;

                #region
                if (style != RoundStyle.None)
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false)) { g.FillPath(brush, path); }

                    #region
                    if (baseColor.A > 80)
                    {
                        Rectangle rectTop = rect;

                        if (mode == LinearGradientMode.Vertical) { rectTop.Height = (int)(rectTop.Height * basePosition); }
                        else { rectTop.Width = (int)(rect.Width * basePosition); }

                        using (GraphicsPath pathTop = GraphicsPathHelper.CreatePath(rectTop, roundWidth, RoundStyle.Top, false))
                        {
                            using (SolidBrush brushAlpha = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) { g.FillPath(brushAlpha, pathTop); }
                        }
                    }
                    #endregion

                    #region
                    if (drawGlass)
                    {
                        RectangleF glassRect = rect;

                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        }
                        else
                        {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }

                        ControlPaintEx.DrawGlass(g, glassRect, 170, 0);
                    }
                    #endregion

                    #region
                    if (drawBorder)
                    {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                        {
                            using (Pen pen = new Pen(borderColor)) { g.DrawPath(pen, path); }
                        }

                        rect.Inflate(-1, -1);
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                        {
                            using (Pen pen = new Pen(innerBorderColor)) { g.DrawPath(pen, path); }
                        }
                    }
                    #endregion
                }
                else
                {
                    g.FillRectangle(brush, rect);

                    #region
                    if (baseColor.A > 80)
                    {
                        Rectangle rectTop = rect;

                        if (mode == LinearGradientMode.Vertical) { rectTop.Height = (int)(rectTop.Height * basePosition); }
                        else { rectTop.Width = (int)(rect.Width * basePosition); }

                        using (SolidBrush brushAlpha = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) { g.FillRectangle(brushAlpha, rectTop); }
                    }
                    #endregion

                    #region
                    if (drawGlass)
                    {
                        RectangleF glassRect = rect;

                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        }
                        else
                        {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }

                        ControlPaintEx.DrawGlass(g, glassRect, 200, 0);
                    }
                    #endregion

                    #region
                    if (drawBorder)
                    {
                        using (Pen pen = new Pen(borderColor)) { g.DrawRectangle(pen, rect); }

                        rect.Inflate(-1, -1);

                        using (Pen pen = new Pen(innerBorderColor)) { g.DrawRectangle(pen, rect); }
                    }
                    #endregion
                }
                #endregion
            }
        }

        internal static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(0, a + a0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(0, r + r0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(0, g + g0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(0, b + b0); }

            return Color.FromArgb(a, r, g, b);
        }
    }

    public class ToolStripColorTable
    {
        #region
        private bool _headUseLiBrush;//头部是否渐变填充

        private bool _bfillBack;//头部背景色填充

        private Color _base = Color.Blue;//基本颜色

        private Color _border = Color.FromArgb(194, 169, 120);//边框颜色

        private Color _backNormal = Color.FromArgb(250, 250, 250);//背景颜色

        private Color _backHover = Color.FromArgb(255, 201, 15);//背景提示颜色

        private Color _backPressed = Color.FromArgb(226, 176, 0);//鼠标按下去的颜色

        private Color _fore = Color.FromArgb(21, 66, 139);//前景颜色


        private Color _dropDownImageBack = Color.FromArgb(233, 238, 238);//下拉背景

        private Color _dropDownImageSeparator = Color.FromArgb(197, 197, 197);//分割符
        #endregion

        public ToolStripColorTable(bool fill = false, bool brush = false)
        {
            _bfillBack = fill;

            _headUseLiBrush = false;
        }

        public bool HeadUseLiBrush
        {
            get { return this._headUseLiBrush; }
            set { this._headUseLiBrush = value; }
        }

        public bool BfillBack
        {
            get { return this._bfillBack; }
            set { this._bfillBack = value; }
        }

        public virtual Color Base
        {
            get { return _base; }
            set { _base = value; }
        }

        public virtual Color Border
        {
            get { return _border; }
            set { _border = value; }
        }

        public virtual Color BackNormal
        {
            get { return _backNormal; }
            set { _backNormal = value; }
        }

        public virtual Color BackHover
        {
            get { return _backHover; }
            set { _backHover = value; }
        }

        public virtual Color BackPressed
        {
            get { return _backPressed; }
            set { _backPressed = value; }
        }

        public virtual Color Fore
        {
            get { return _fore; }
            set { _fore = value; }
        }

        public virtual Color DropDownImageBack
        {
            get { return _dropDownImageBack; }
            set { _dropDownImageBack = value; }
        }

        public virtual Color DropDownImageSeparator
        {
            get { return _dropDownImageSeparator; }
            set { _dropDownImageSeparator = value; }
        }
    }

    internal class TextRenderingHintGraphics : IDisposable
    {
        private Graphics _graphics;

        private TextRenderingHint _oldTextRenderingHint;

        public TextRenderingHintGraphics(Graphics graphics)
            : this(graphics, TextRenderingHint.AntiAlias)
        {
        }

        public TextRenderingHintGraphics(Graphics graphics, TextRenderingHint newTextRenderingHint)
        {
            _graphics = graphics;
            _oldTextRenderingHint = graphics.TextRenderingHint;
            _graphics.TextRenderingHint = newTextRenderingHint;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _graphics.TextRenderingHint = _oldTextRenderingHint;
        }

        #endregion
    }

    internal static class GetPixByPoint
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern uint GetPixel(IntPtr hDC, int XPos, int YPos);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr ReleaseDC(IntPtr handle, IntPtr hDC);
        public static Color GetControlPixByPoint(IntPtr controlPtr, Point point)
        {
            if (IntPtr.Zero == controlPtr) { throw new ArgumentNullException("控件为空"); }

            IntPtr hdc = GetDC(controlPtr);

            uint color = GetPixel(hdc, point.X, point.Y);

            ReleaseDC(controlPtr, hdc);

            Color result = ColorTranslator.FromWin32((int)color);

            return result;
        }

        public static Color GetBitmapPixByPoint(Bitmap map, Point point)
        {
            if (null == map) { return Color.White; }

            return map.GetPixel(point.X, point.Y);
        }

        public static Color GetImagePixByPoint(Image image, Point point)
        {
            if (null == image) { return SystemColors.AppWorkspace; }

            return new Bitmap(image).GetPixel(point.X, point.Y);
        }
    }

    public class ProfessionalToolStripRendererEx : ToolStripRenderer
    {
        private static readonly int OffsetMargin = 0;

        private ToolStripColorTable _colorTable;

        public ProfessionalToolStripRendererEx()
            : base()
        {
            _colorTable = new ToolStripColorTable();
        }

        public ProfessionalToolStripRendererEx(ToolStripColorTable colorTable)
            : base()
        {
            _colorTable = colorTable;
        }

        protected virtual ToolStripColorTable ColorTable
        {
            get
            {
                if (_colorTable == null) { _colorTable = new ToolStripColorTable(); }

                return _colorTable;
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                RegionHelper.CreateRegion(toolStrip, bounds);
                using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, toolStrip.Height), Color.FromArgb(255, Color.White), Color.FromArgb(255, ColorTable.Base)))
                {
                    g.FillRectangle(brush, bounds);
                }
            }
            else if (toolStrip is MenuStrip)
            {
                if (!ColorTable.BfillBack) { return; }

                if (ColorTable.HeadUseLiBrush)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(toolStrip.Width, toolStrip.Height), Color.White, ColorTable.Base))
                    {
                        g.FillRectangle(brush, bounds);
                    }
                }
                else
                {
                    LinearGradientMode mode = toolStrip.Orientation == Orientation.Horizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
                    RenderHelper.RenderBackgroundInternal(g, bounds, ColorTable.Base, ColorTable.Border, ColorTable.BackNormal, RoundStyle.None, 0, .35f, false, false, mode);
                }
            }
            else
            {
                base.OnRenderToolStripBackground(e);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //ToolStrip toolStrip = e.ToolStrip;
            //Graphics g = e.Graphics;
            //Rectangle bounds = e.AffectedBounds;

            //if (toolStrip is ToolStripDropDown)
            //{
            //    bool bDrawLogo = NeedDrawLogo(toolStrip);
            //    bool bRightToLeft = toolStrip.RightToLeft == RightToLeft.Yes;

            //    Rectangle imageBackRect = bounds;
            //    imageBackRect.Width = OffsetMargin;

            //    #region
            //    if (bDrawLogo)
            //    {
            //        Rectangle logoRect = bounds;
            //        logoRect.Width = OffsetMargin;
            //        if (bRightToLeft) { logoRect.X -= 2; imageBackRect.X = logoRect.X - OffsetMargin; }
            //        else { logoRect.X += 2; imageBackRect.X = logoRect.Right; }
            //        logoRect.Y += 1;
            //        logoRect.Height -= 2;

            //        using (LinearGradientBrush brush = new LinearGradientBrush(logoRect, ColorTable.BackHover, ColorTable.BackNormal, 90f))
            //        {
            //            Blend blend = new Blend();
            //            blend.Positions = new float[] { 0f, .2f, 1f };
            //            blend.Factors = new float[] { 0f, 0.1f, .9f };
            //            brush.Blend = blend;
            //            logoRect.Y += 1;
            //            logoRect.Height -= 2;
            //            using (GraphicsPath path = GraphicsPathHelper.CreatePath(logoRect, 4, RoundStyle.All, false))
            //            {
            //                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) { g.FillPath(brush, path); }
            //            }
            //        }

            //        StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
            //        Font font = new Font(toolStrip.Font.FontFamily, 11, FontStyle.Bold);
            //        sf.Alignment = StringAlignment.Near;
            //        sf.LineAlignment = StringAlignment.Center;
            //        sf.Trimming = StringTrimming.EllipsisCharacter;

            //        g.TranslateTransform(logoRect.X, logoRect.Bottom);
            //        g.RotateTransform(270f);

            //        if (!string.IsNullOrEmpty(MenuLogoString))
            //        {
            //            Rectangle newRect = new Rectangle(0, 0, logoRect.Height, logoRect.Width);

            //            using (Brush brush = new SolidBrush(ColorTable.Fore))
            //            {
            //                using (TextRenderingHintGraphics tg = new TextRenderingHintGraphics(g)) { g.DrawString(MenuLogoString, font, brush, newRect, sf); }
            //            }
            //        }

            //        g.ResetTransform();
            //    }
            //    else
            //    {
            //        if (bRightToLeft) { imageBackRect.X -= 3; }
            //        else { imageBackRect.X += 3; }
            //    }
            //    #endregion

            //    #region
            //    imageBackRect.Y += 2;
            //    imageBackRect.Height -= 4;
            //    using (SolidBrush brush = new SolidBrush(ColorTable.DropDownImageBack)) { g.FillRectangle(brush, imageBackRect); }

            //    Point ponitStart;
            //    Point pointEnd;
            //    if (bRightToLeft)
            //    {
            //        ponitStart = new Point(imageBackRect.X, imageBackRect.Y);
            //        pointEnd = new Point(imageBackRect.X, imageBackRect.Bottom);
            //    }
            //    else
            //    {
            //        ponitStart = new Point(imageBackRect.Right - 1, imageBackRect.Y);
            //        pointEnd = new Point(imageBackRect.Right - 1, imageBackRect.Bottom);
            //    }

            //    using (Pen pen = new Pen(ColorTable.DropDownImageSeparator)) { g.DrawLine(pen, ponitStart, pointEnd); }
            //    #endregion
            //}
            //else
            //{
            //    base.OnRenderImageMargin(e);
            //}

            base.OnRenderImageMargin(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Graphics g = e.Graphics;
            Rectangle bounds = e.AffectedBounds;

            if (toolStrip is ToolStripDropDown)
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(bounds, 4, RoundStyle.All, true))
                    {
                        using (Pen pen = new Pen(ColorTable.DropDownImageSeparator)) { path.Widen(pen); g.DrawPath(pen, path); }
                    }
                }

                bounds.Inflate(-1, -1);
                using (GraphicsPath innerPath = GraphicsPathHelper.CreatePath(bounds, 4, RoundStyle.All, true))
                {
                    using (Pen pen = new Pen(ColorTable.BackNormal)) { g.DrawPath(pen, innerPath); }
                }
            }
            else if (toolStrip is StatusStrip)
            {
                using (Pen pen = new Pen(ColorTable.Border)) { e.Graphics.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1); }
            }
            else if (toolStrip is MenuStrip)
            {
                base.OnRenderToolStripBorder(e);
            }
            else
            {
                using (Pen pen = new Pen(ColorTable.Border)) { g.DrawRectangle(pen, 0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1); }
            }
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            ToolStripItem item = e.Item;

            if (!item.Enabled) { return; }

            Graphics g = e.Graphics;
            Rectangle rect = new Rectangle(Point.Empty, e.Item.Size);

            if (toolStrip is MenuStrip)
            {
                #region
                LinearGradientMode mode = toolStrip.Orientation == Orientation.Horizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

                if (item.Selected)
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, ColorTable.BackHover, ColorTable.Border, ColorTable.BackNormal, RoundStyle.All, true, true, mode);
                }
                else if (item.Pressed)
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, ColorTable.BackPressed, ColorTable.Border, ColorTable.BackNormal, RoundStyle.All, true, true, mode);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
                #endregion
            }
            else if (toolStrip is ToolStripDropDown)
            {
                #region
                g.SmoothingMode = SmoothingMode.HighQuality;
                LinearGradientBrush lgbrush = new LinearGradientBrush(new Point(0, 0), new Point(item.Width, 0), Color.FromArgb(100, ColorTable.Border), Color.FromArgb(250, ColorTable.BackHover));
                if (item.Selected)
                {
                    GraphicsPath gp = GraphicsPathHelper.CreatePath(new Rectangle(0, 0, item.Width, item.Height), 4, RoundStyle.All);
                    g.FillPath(lgbrush, gp);
                }
                else
                {
                    base.OnRenderMenuItemBackground(e);
                }
                #endregion


                #region
                //bool bDrawLogo = NeedDrawLogo(toolStrip);

                //int offsetMargin = bDrawLogo ? OffsetMargin : 0;

                //if (item.RightToLeft == RightToLeft.Yes) { rect.X += 4; }
                //else { rect.X += offsetMargin + 4; }

                //rect.Width -= offsetMargin + 8;
                //--rect.Height;

                //if (item.Selected)
                //{
                //    RenderHelper.RenderBackgroundInternal(g,rect,ColorTable.BackHover,ColorTable.Border,ColorTable.BackNormal,RoundStyle.All,true,true,LinearGradientMode.Vertical);
                //}
                //else
                //{
                //    base.OnRenderMenuItemBackground(e);
                //}
                #endregion
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;

            Graphics g = e.Graphics;

            if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);
                int offsetMargin = bDrawLogo ? OffsetMargin : 0;

                ToolStripMenuItem item = (ToolStripMenuItem)e.Item;
                if (item.Checked) { return; }
                Rectangle rect = e.ImageRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes) { rect.X -= offsetMargin + 2; }
                else { rect.X += offsetMargin + 2; }

                using (InterpolationModeGraphics ig = new InterpolationModeGraphics(g))
                {
                    ToolStripItemImageRenderEventArgs ne = new ToolStripItemImageRenderEventArgs(g, e.Item, e.Image, rect);
                    base.OnRenderItemImage(ne);
                }
            }
            else
            {
                base.OnRenderItemImage(e);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;

            //e.TextColor = ColorTable.Fore;

            if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
            {
                //bool bDrawLogo = NeedDrawLogo(toolStrip);

                //int offsetMargin = bDrawLogo ? 18 : 0;

                Rectangle rect = e.TextRectangle;
                //if (e.Item.RightToLeft != RightToLeft.Yes) { rect.X -= offsetMargin; }
                //else { rect.X += offsetMargin; }

                e.TextRectangle = rect;

                e.TextColor = ColorTable.Fore;
            }
            else
            {
                if (null != (toolStrip as MenuStrip)) { e.TextColor = Color.White; }
            }



            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;

            Graphics g = e.Graphics;

            if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);
                int offsetMargin = bDrawLogo ? OffsetMargin : 0;
                Rectangle rect = e.ImageRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes) { rect.X -= offsetMargin + 2; }
                else { rect.X += offsetMargin + 2; }

                rect.Width = 13;
                rect.Y += 1;
                rect.Height -= 3;

                #region
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        path.AddRectangle(rect);

                        using (PathGradientBrush brush = new PathGradientBrush(path))
                        {
                            brush.CenterColor = Color.White;
                            brush.SurroundColors = new Color[] { ControlPaint.Light(ColorTable.BackNormal) };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.3f, 1f };
                            blend.Factors = new float[] { 0f, 0.5f, 1f };
                            brush.Blend = blend;
                            g.FillRectangle(brush, rect);
                        }
                    }

                    using (Pen pen = new Pen(ControlPaint.Light(ColorTable.BackNormal)))
                    {
                        g.DrawRectangle(pen, rect);
                    }

                    ControlPaintEx.DrawCheckedFlag(g, rect, ColorTable.Fore);
                }
                #endregion
            }
            else
            {
                base.OnRenderItemCheck(e);
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            if (e.Item.Enabled) { e.ArrowColor = ColorTable.Fore; }

            ToolStrip toolStrip = e.Item.Owner;

            if (toolStrip is ToolStripDropDown && e.Item is ToolStripMenuItem)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? 3 : 0;

                Rectangle rect = e.ArrowRectangle;

                if (e.Item.RightToLeft == RightToLeft.Yes) { rect.X -= offsetMargin; }
                else { rect.X += offsetMargin; }

                e.ArrowRectangle = rect;
            }

            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            ToolStrip toolStrip = e.ToolStrip;
            Rectangle rect = e.Item.ContentRectangle;
            Graphics g = e.Graphics;

            if (toolStrip is ToolStripDropDown)
            {
                bool bDrawLogo = NeedDrawLogo(toolStrip);

                int offsetMargin = bDrawLogo ? OffsetMargin * 2 : OffsetMargin;

                if (e.Item.RightToLeft != RightToLeft.Yes) { rect.X += offsetMargin + 2; }

                rect.Width -= offsetMargin + 4;
            }

            RenderSeparatorLine(g, rect, ColorTable.DropDownImageSeparator, ColorTable.BackNormal, SystemColors.ControlLightLight, e.Vertical);
        }

        internal void RenderSeparatorLine(Graphics g, Rectangle rect, Color baseColor, Color backColor, Color shadowColor, bool vertical)
        {
            float angle;

            if (vertical) { angle = 90F; }
            else { angle = 180F; }

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, baseColor, backColor, angle))
            {
                Blend blend = new Blend();
                blend.Positions = new float[] { 0f, .2f, .5f, .8f, 1f };
                blend.Factors = new float[] { 1f, .3f, 0f, .3f, 1f };
                brush.Blend = blend;

                using (Pen pen = new Pen(brush))
                {
                    if (vertical) { g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom); }
                    else { g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y); }

                    brush.LinearColors = new Color[] { shadowColor, ColorTable.BackNormal };
                    pen.Brush = brush;

                    if (vertical) { g.DrawLine(pen, rect.X + 1, rect.Y, rect.X + 1, rect.Bottom); }
                    else { g.DrawLine(pen, rect.X, rect.Y + 1, rect.Right, rect.Y + 1); }
                }
            }
        }

        internal bool NeedDrawLogo(ToolStrip toolStrip)
        {
            ToolStripDropDown dropDown = toolStrip as ToolStripDropDown;

            bool bDrawLogo = (dropDown.OwnerItem != null && dropDown.OwnerItem.Owner is MenuStrip) || (toolStrip is ContextMenuStrip);

            return bDrawLogo;
        }
    }
}
