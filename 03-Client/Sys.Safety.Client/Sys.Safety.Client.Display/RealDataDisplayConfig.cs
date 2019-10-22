using System.Drawing;

namespace Sys.Safety.Client.Display
{
    /// <summary>
    /// 实时数据显示配置类
    /// </summary>
    public  class RealDataDisplayConfig
    {
        private BaseConfig baseCfg = new BaseConfig();
        /// <summary>
        /// 实时显示基本配置信息
        /// </summary>
        public  BaseConfig BaseCfg
        {
            get { return baseCfg; }
            set { baseCfg = value; }
        }

        private FontConfig fontCfg = new FontConfig();
        /// <summary>
        /// 实时显示字体相关配置
        /// </summary>
        public  FontConfig FontCfg
        {
            get { return fontCfg; }
            set { fontCfg = value; }
        }

        private StateColorConfig stateCorCfg = new StateColorConfig();
        /// <summary>
        /// 实时显示各种状态的颜色配置
        /// </summary>
        public  StateColorConfig StateCorCfg
        {
            get { return stateCorCfg; }
            set { stateCorCfg = value; }
        }

        private DataColumnConfig dataClnCfg = new DataColumnConfig();
        /// <summary>
        /// 实时显示数据列的相关配置
        /// </summary>
        public  DataColumnConfig DataClnCfg
        {
            get { return dataClnCfg; }
            set { dataClnCfg = value; }
        }

        private SpecialPointConfig splPointCfg = new SpecialPointConfig();
        /// <summary>
        /// 特殊测点的显示配置（按类型）
        /// </summary>
        public  SpecialPointConfig SplPointCfg
        {
            get { return splPointCfg; }
            set { splPointCfg = value; }
        }

    }

    /// <summary>
    /// 实时数据显示基本配置
    /// </summary>
    public  class BaseConfig
    {
        private Color backColor;
        /// <summary>
        /// 显示列表背景颜色
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        private Color gridColor;
        /// <summary>
        /// 网格颜色
        /// </summary>
        public Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }

        private bool  showgrid;
        /// <summary>
        /// 是否显示网格
        /// </summary>
        public bool  Showgrid
        {
            get { return showgrid; }
            set { showgrid = value; }
        }

        private string colorchange;
        /// <summary>
        /// 颜色渐变方式
        /// </summary>
        public string Colorchange
        {
            get { return colorchange; }
            set { colorchange = value; }
        }

        private bool showju;
        /// <summary>
        /// 奇偶行是否可用
        /// </summary>
        public bool Showju
        {
            get { return showju; }
            set { showju = value; }
        }

        private Color tableHadeBackColor;
        /// <summary>
        /// 表头背景颜色
        /// </summary>
        public Color TableHadeBackColor
        {
            get { return tableHadeBackColor; }
            set { tableHadeBackColor = value; }
        }

        private Color splitColor;
        /// <summary>
        /// 分隔线颜色
        /// </summary>
        public Color SplitColor
        {
            get { return splitColor; }
            set { splitColor = value; }
        }

        private Color selectColor;
        /// <summary>
        /// 选中后颜色
        /// </summary>
        public Color SelectColor
        {
            get { return selectColor; }
            set { selectColor = value; }
        }
        private Color singleRowColor;
        /// <summary>
        /// 奇数行颜色
        /// </summary>
        public Color SingleRowColor
        {
            get { return singleRowColor; }
            set { singleRowColor = value; }
        }
        private Color doubleRowColor;
        /// <summary>
        /// 偶数行颜色
        /// </summary>
        public Color DoubleRowColor
        {
            get { return doubleRowColor; }
            set { doubleRowColor = value; }
        }

        private Color gvBackColor;
        /// <summary>
        /// 显示控件背景颜色
        /// </summary>
        public Color GvBackColor
        {
            get { return gvBackColor; }
            set { gvBackColor = value; }
        }

        private int tableHadeHigh=20;
        /// <summary>
        /// 表头行高
        /// </summary>
        public int TableHadeHigh
        {
            get { return tableHadeHigh; }
            set { tableHadeHigh = value; }
        }

        private int dataRowHigh=20;
        /// <summary>
        /// 数据行高
        /// </summary>
        public int DataRowHigh
        {
            get { return dataRowHigh; }
            set { dataRowHigh = value; }
        }

        private int splitWidth=2;
        /// <summary>
        /// 分隔线宽
        /// </summary>
        public int  SplitWidth
        {
            get { return splitWidth; }
            set 
            {
                if (value > 0)
                {
                    splitWidth = value;
                }
                else
                {
                    splitWidth = 1;
                }
            }
        }

        private float bjfontsize = 8;
        /// <summary>
        /// 报警栏字体大小
        /// </summary>
        public float Bjfontsize
        {
            get { return bjfontsize; }
            set { bjfontsize = value; }
        }

        private int pageChangeInterval=5;
        /// <summary>
        /// 自动翻页间隔时间 默认5秒
        /// </summary>
        public int PageChangeInterval
        {
            get { return pageChangeInterval; }
            set { pageChangeInterval = value; }
        }
    }

    /// <summary>
    /// 实时数据显示文字相关配置
    /// </summary>
    public  class FontConfig
    {
        private string  tableHadeFontName;
        /// <summary>
        /// 表头字体名称
        /// </summary>
        public string  TableHadeFontName
        {
            get { return tableHadeFontName; }
            set { tableHadeFontName = value; }
        }
        private int tableHadeFontSize;
        /// <summary>
        /// 表头字体大小
        /// </summary>
        public int TableHadeFontSize
        {
            get { return tableHadeFontSize; }
            set { tableHadeFontSize = value; }
        }
        private bool isBold;
        /// <summary>
        /// 是否为粗体
        /// </summary>
        public bool IsBold
        {
            get { return isBold; }
            set { isBold = value; }
        }
        private bool isItalic;
        /// <summary>
        /// 是否为斜体
        /// </summary>
        public bool IsItalic
        {
            get { return isItalic; }
            set { isItalic = value; }
        }
        private bool isHaveUnderLine;
        /// <summary>
        /// 是否有下划线
        /// </summary>
        public bool IsHaveUnderLine
        {
            get { return isHaveUnderLine; }
            set { isHaveUnderLine = value; }
        }
        private Color tableHadeFontColor;
        /// <summary>
        /// 表头字体颜色
        /// </summary>
        public Color TableHadeFontColor
        {
            get { return tableHadeFontColor; }
            set { tableHadeFontColor = value; }
        }

        private string  dataFontName;
        /// <summary>
        /// 数据行字体名称
        /// </summary>
        public string  DataFontName
        {
            get { return dataFontName; }
            set { dataFontName = value; }
        }
        private int dataFontSize;
        /// <summary>
        /// 数据行字体大小
        /// </summary>
        public int DataFontSize
        {
            get { return dataFontSize; }
            set { dataFontSize = value; }
        }
        private bool dataisBold;
        /// <summary>
        /// 数据行字体是否为粗体
        /// </summary>
        public bool DataIsBold
        {
            get { return dataisBold; }
            set { dataisBold = value; }
        }
        private bool dataisItalic;
        /// <summary>
        /// 数据行是否为斜体
        /// </summary>
        public bool DataIsItalic
        {
            get { return dataisItalic; }
            set { dataisItalic = value; }
        }
        private bool dataisHaveUnderLine;
        /// <summary>
        /// 数据行是否有下划线
        /// </summary>
        public bool DataIsHaveUnderLine
        {
            get { return dataisHaveUnderLine; }
            set { dataisHaveUnderLine = value; }
        }
        private Color dataFontColor;
        /// <summary>
        /// 数据行字体颜色
        /// </summary>
        public Color DataFontColor
        {
            get { return dataFontColor; }
            set { dataFontColor = value; }
        }
    }

    /// <summary>
    /// 实时显示状态颜色配置
    /// </summary>
    public  class StateColorConfig
    {

        private Color upPreAlarmColor;
        /// <summary>
        /// 上限预警颜色
        /// </summary>
        public Color UpPreAlarmColor
        {
            get { return upPreAlarmColor; }
            set { upPreAlarmColor = value; }
        }

        private Color upAlarmColor;
        /// <summary>
        /// 上限报警颜色
        /// </summary>
        public Color UpAlarmColor
        {
            get { return upAlarmColor; }
            set { upAlarmColor = value; }
        }

        private Color upBlackOutColor;
        /// <summary>
        /// 上限断电显示颜色
        /// </summary>
        public Color UpBlackOutColor
        {
            get { return upBlackOutColor; }
            set { upBlackOutColor = value; }
        }

        private Color lowPreAlarmColor;
        /// <summary>
        /// 下限预警显示颜色
        /// </summary>
        public Color LowPreAlarmColor
        {
            get { return lowPreAlarmColor; }
            set { lowPreAlarmColor = value; }
        }

        private Color lowAlarmColor;
        /// <summary>
        /// 下限报警显示颜色
        /// </summary>
        public Color LowAlarmColor
        {
            get { return lowAlarmColor; }
            set { lowAlarmColor = value; }
        }

        private Color lowBlackOutColor;
        /// <summary>
        /// 下限断电颜色
        /// </summary>
        public Color LowBlackOutColor
        {
            get { return lowBlackOutColor; }
            set { lowBlackOutColor = value; }
        }

        private Color kAlarmColor;
        /// <summary>
        /// 开关量报警颜色
        /// </summary>
        public Color KAlarmColor
        {
            get { return kAlarmColor; }
            set { kAlarmColor = value; }
        }

        private Color kBlackOutColor;
        /// <summary>
        /// 开关量断电颜色
        /// </summary>
        public Color KBlackOutColor
        {
            get { return kBlackOutColor; }
            set { kBlackOutColor = value; }
        }

        private Color interruptionColor;
        /// <summary>
        /// 通讯中断颜色
        /// </summary>
        public Color InterruptionColor
        {
            get { return interruptionColor; }
            set { interruptionColor = value; }
        }

        private Color defaultColor;
        /// <summary>
        /// 正常颜色
        /// </summary>
        public Color DefaultColor
        {
            get { return defaultColor; }
            set { defaultColor = value; }
        }

        private Color overRangeColor;

        /// <summary>
        /// 超量程颜色
        /// </summary>
        public Color OverRangeColor
        {
            get { return overRangeColor; }
            set { overRangeColor = value; }
        }

        private Color dcColor;

        /// <summary>
        /// 直流颜色
        /// </summary>
        public Color DcColor
        {
            get { return dcColor; }
            set { dcColor = value; }
        }

        /// <summary>
        /// 标效颜色
        /// </summary>
        public Color EffectColor
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 数据列设置
    /// </summary>
    public  class DataColumnConfig
    {
        private bool showUnit = false;
        /// <summary>
        /// 是否在实时值后面显示单位
        /// </summary>
        public bool ShowUnit
        {
            get { return showUnit; }
            set { showUnit = value; }
        }

        private DataColumnMsg[] columnsMsg = new DataColumnMsg[18];
        /// <summary>
        /// 各列设置信息 
        /// columnsMsg[0]为标签列 columnsMsg[1]为位置列 columnsMsg[2]为实时值列 columnsMsg[3]为状态列
        /// columnsMsg[4]为类型列 columnsMsg[5]为采集时间列 columnsMsg[6]为上限报警列 
        /// columnsMsg[7]为上限断电列  columnsMsg[8]为下限报警列 columnsMsg[9]为下限断电列
        /// columnsMsg[10]为最近报警列 columnsMsg[11]为最近断电列 columnsMsg[12]为报警最大值列
        /// columnsMsg[13] 电量等级列  columnsMsg[14]为报警最小值列 columnsMsg[15]为预警开始时间列 
        /// columnsMsg[16]为变化率列
        /// </summary>
        public  DataColumnMsg[] ColumnsMsg
        {
            get { return columnsMsg; }
            set { columnsMsg = value; }
        }
    }

    /// <summary>
    /// 数据列设置信息
    /// </summary>
    public  class DataColumnMsg
    {
        private string columnName="标签名";
        /// <summary>
        /// 显示列自定义名称
        /// </summary>
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        private int columnWidth;
        /// <summary>
        /// 显示列宽度
        /// </summary>
        public int ColumnWidth
        {
            get { return columnWidth; }
            set { columnWidth = value; }
        }

        private bool isLocked;
        /// <summary>
        /// 是否锁定列宽
        /// </summary>
        public bool IsLocked
        {
            get { return isLocked; }
            set { isLocked = value; }
        }

        private string  columnType;
        /// <summary>
        /// 对齐方式 1-左对齐 2-居中 3-右对齐
        /// </summary>
        public string  ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }
    }

    /// <summary>
    /// 特殊点显示设置
    /// </summary>
    public  class SpecialPointConfig
    {
        private bool selectNet = false;
        /// <summary>
        /// 启用设置网络模块颜色
        /// </summary>
        public bool Selectnet
        {
            get { return selectNet; }
            set { selectNet = value; }
        }

        private Color[] netColor = new Color[2];
        /// <summary>
        /// 链路点显示颜色 color[0]为字体颜色 color[1]为背景颜色
        /// </summary>
        public Color[] NetColor
        {
            get { return netColor; }
            set { netColor = value; }
        }

        private bool selectM = false;
        /// <summary>
        /// 启用设置模拟量测点颜色
        /// </summary>
        public bool SelectM
        {
            get { return selectM; }
            set { selectM = value; }
        }

        private Color[] mColor = new Color[2];
        /// <summary>
        /// 模拟量显示颜色 color[0]为字体颜色 color[1]为背景颜色
        /// </summary>
        public Color[] MColor
        {
            get { return mColor; }
            set { mColor = value; }
        }

        private bool selectK = false;
        /// <summary>
        /// 启用设置开关量测点颜色
        /// </summary>
        public bool SelectK
        {
            get { return selectK; }
            set { selectK = value; }
        }

        private Color[] kColor = new Color[2];
        /// <summary>
        /// 开关量显示颜色 color[0]为字体颜色 color[1]为背景颜色
        /// </summary>
        public Color[] KColor
        {
            get { return kColor; }
            set { kColor = value; }
        }

        private bool selectC = false;
        /// <summary>
        /// 启用设置控制量测点颜色
        /// </summary>
        public bool SelectC
        {
            get { return selectC; }
            set { selectC = value; }
        }

        private Color[] cColor = new Color[2];
        /// <summary>
        /// 控制量显示颜色 color[0]为字体颜色 color[1]为背景颜色
        /// </summary>
        public Color[] CColor
        {
            get { return cColor; }
            set { cColor = value; }
        }

        private bool selectS = false;
        /// <summary>
        /// 启用分站测点颜色配置
        /// </summary>
        public bool SelectS
        {
            get { return selectS; }
            set { selectS = value; }
        }

        private Color[] scolor = new Color[2];
        /// <summary>
        /// 分站显示颜色 color[0]为字体颜色 color[1]为背景颜色
        /// </summary>
        public Color[] Scolor
        {
            get { return scolor; }
            set { scolor = value; }
        }

    }

}
