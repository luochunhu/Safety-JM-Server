using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.Client.Display
{
    /// <summary>
    /// 测点编排信息
    /// </summary>
    public  class PointArrangeConfig
    {
        private PageSetConfig typeConfig=new PageSetConfig ();

        /// <summary>
        /// 按类型编排设置
        /// </summary>
        public  PageSetConfig TypeConfig
        {
            get { return typeConfig; }
            set { typeConfig = value; }
        }
        private PageSetConfig netConfig=new PageSetConfig ();
        /// <summary>
        /// 按链路编排设置
        /// </summary>
        public  PageSetConfig NetConfig
        {
            get { return netConfig; }
            set { netConfig = value; }
        }
        private PageSetConfig areaConfig=new PageSetConfig ();
        /// <summary>
        /// 按区域设置
        /// </summary>
        public  PageSetConfig AreaConfig
        {
            get { return areaConfig; }
            set { areaConfig = value; }
        }
        private PageSetConfig stateConfig=new PageSetConfig ();
        /// <summary>
        /// 按状态编排设置
        /// </summary>
        public  PageSetConfig StateConfig
        {
            get { return stateConfig; }
            set { stateConfig = value; }
        }

        private PageSetConfig[] customCofig=new PageSetConfig [15];
        /// <summary>
        /// 自定义编排 设置
        /// </summary>
        public PageSetConfig[] CustomCofig
        {
            get { return customCofig; }
            set { customCofig = value; }
        }
    }

    /// <summary>
    /// 页面设置
    /// </summary>
    public  class PageSetConfig
    {
        private string pageName;
        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }
        private int pageSortType;
        /// <summary>
        /// 排序方式 1-分站号 2-测点号 3-位置 
        /// </summary>
        public int PageSortType
        {
            get { return pageSortType; }
            set { pageSortType = value; }
        }

        private bool isUpIndex;
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsUpIndex
        {
            get { return isUpIndex; }
            set { isUpIndex = value; }
        }
        private int showColumnCount;
        /// <summary>
        /// 显示列组数
        /// </summary>
        public int ShowColumnCount
        {
            get { return showColumnCount; }
            set { showColumnCount = value; }
        }
        private int showRowCount;
        /// <summary>
        /// 显示行数
        /// </summary>
        public int ShowRowCount
        {
            get { return showRowCount; }
            set { showRowCount = value; }
        }
        private bool isAutoSplitPage;
        /// <summary>
        /// 是否自动分页
        /// </summary>
        public bool IsAutoSplitPage
        {
            get { return isAutoSplitPage; }
            set { isAutoSplitPage = value; }
        }
        private bool isDataFillType;
        /// <summary>
        /// 数据填充方式 true 横向  false 纵向
        /// </summary>
        public bool IsDataFillType
        {
            get { return isDataFillType; }
            set { isDataFillType = value; }
        }
        private string isColumnsMsg;
        /// <summary>
        /// 显示列信息  如 1|2|3|7 代表 点号|位置|实时值|上限报警值
        /// </summary>
        public string IsColumnsMsg
        {
            get { return isColumnsMsg; }
            set { isColumnsMsg = value; }
        }
        private colummsg[] column = new colummsg[18];
        /// <summary>
        /// 组信息
        /// </summary>
        public colummsg[] Column
        {
            get { return column; }
            set { column = value; }
        }
        private int page = 0;
        /// <summary>
        /// 页面编号
        /// </summary>
        public int Page
        {
            get { return page; }
            set { page = value; }
        }
    }

    /// <summary>
    /// 显示组信息
    /// </summary>
    public class colummsg
    {
        private bool isuse = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Isuse
        {
            get { return isuse; }
            set { isuse = value; }
        }
        private int index = 0;
        /// <summary>
        /// 排序值
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
    }

}
