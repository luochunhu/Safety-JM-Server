using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class JC_LargedataAnalysisConfigInfo
    {
        ///// <summary>
        ///// 分析模型关联测点信息.
        ///// </summary>
        public List<JC_SetAnalysisModelPointRecordInfo> AnalysisModelPointRecordInfoList { get; set; }
     

        /// <summary>
        /// 控制测点信息
        /// </summary>
        public List<Jc_DefInfo> LevelTrueDescription { get; set; }
        /// <summary>
        /// 解控测点信息
        /// </summary>
        public List<Jc_DefInfo> LevelFalseDescription { get; set; }

        /// <summary>
        /// 上一次 0-未知，1-不成立, 2-成立
        /// </summary>
        public int PrevAnalysisResult
        {
            get;
            set;
        }


        private DateTime _PrevAnalysisTime;
        /// <summary>
        /// 上一次分析时间
        /// </summary>
        public DateTime PrevAnalysisTime {
            get {
                if (_PrevAnalysisTime == null)
                    _PrevAnalysisTime = DateTime.Now;
                return _PrevAnalysisTime;
            }
            set
            {
                _PrevAnalysisTime = value;
            }
        }

        /// <summary>
        /// 是否应急联动
        /// </summary>
        public bool IsEmergencyLinkage { get; set; }

        /// <summary>
        /// 应急联动配置
        /// </summary>
        public string EmergencyLinkageConfig { get; set; }

        /// <summary>
        /// 表达式Lua用
        /// </summary>
        public string Expresstion
        {
            get;
            set;
        }
        /// <summary>
        /// 持续时间
        /// </summary>
        public int ContinueTime
        {
            get;
            set;
        }

        /// <summary>
        /// 最大持续时间
        /// </summary>
        public int MaxContinueTime
        {
            get;
            set;
        }

        private List<AnalysisSuccessfulPointInfo> _AnalysisSuccessfulPointList;
        /// <summary>
        /// 分析成立的测点列表
        /// </summary>
        public List<AnalysisSuccessfulPointInfo> AnalysisSuccessfulPointList {
            get {
                if (_AnalysisSuccessfulPointList == null)
                    _AnalysisSuccessfulPointList = new List<AnalysisSuccessfulPointInfo>();
                return _AnalysisSuccessfulPointList;
            }
        }
    }



    /// <summary>
    /// 表达式实时分析结果
    /// </summary>
    public partial class ExpressionRealTimeResultInfo: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        /// <summary>
        /// 分析模型Id
        /// </summary>
        public string AnalysisModelId { get; set; }
        /// <summary>
        /// 分析模型名称
        /// </summary>
        private string _analysisModelName;

        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string AnalysisModelName
        {
            get { return _analysisModelName; }
            set
            {
                if (_analysisModelName != value)
                {
                    _analysisModelName = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AnalysisModelName"));
                }
            }
        }
        /// <summary>
        /// 表达式Id
        /// </summary>
        public string _expressionId = string.Empty;
        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string ExpressionId
        {
            get { return _expressionId; }
            set
            {
                if (_expressionId != value)
                {
                    _expressionId = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ExpressionId"));
                }
            }
        }


        /// <summary>
        /// 表达式
        /// </summary>
        public string Expression { get; set; }
       
        /// <summary>
        /// 表达式显示文本
        /// </summary>
        private string _expressionText;

        /// <summary>
        /// 表达式显示文本
        /// </summary>
        public string ExpressionText
        {
            get { return _expressionText; }
            set
            {
                if (_expressionText != value)
                {
                    _expressionText = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ExpressionText"));
                }
            }
        }

      

        /// <summary>
        /// 表达式开始成立的时间
        /// </summary>
        private DateTime _firstSuccessfulTime = DateTime.MinValue;

        /// <summary>
        /// 表达式开始成立的时间
        /// </summary>
        public DateTime FirstSuccessfulTime
        {
            get { return _firstSuccessfulTime; }
            set
            {
                if (_firstSuccessfulTime != value)
                {
                    _firstSuccessfulTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("FirstSuccessfulTime"));
                }
            }
        }
        /// <summary>
        /// 表达式开始成立的时间
        /// </summary>
        private string _showFirstSuccessfulTime = "";

        /// <summary>
        /// 表达式开始成立的时间
        /// </summary>
        public string ShowFirstSuccessfulTime
        {
            get { return _showFirstSuccessfulTime; }
            set
            {
                if (_showFirstSuccessfulTime != value)
                {
                    _showFirstSuccessfulTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ShowFirstSuccessfulTime"));
                }
            }
        }
    
        /// <summary>
        /// 表达式最后一次分析时间
        /// </summary>
        private DateTime _lastAnalysisTime = DateTime.MinValue;

        /// <summary>
        /// 表达式最后一次分析时间
        /// </summary>
        public DateTime LastAnalysisTime
        {
            get { return _lastAnalysisTime; }
            set
            {
                if (_lastAnalysisTime != value)
                {
                    _lastAnalysisTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("LastAnalysisTime"));
                }
            }
        }


        /// <summary>
        /// 表达式最后一次分析时间
        /// </summary>
        private string _showLastAnalysisTime = "";

        /// <summary>
        /// 表达式最后一次分析时间
        /// </summary>
        public string ShowLastAnalysisTime
        {
            get { return _showLastAnalysisTime; }
            set
            {
                if (_showLastAnalysisTime != value)
                {
                    _showLastAnalysisTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ShowLastAnalysisTime"));
                }
            }
        }

        /// <summary>
        /// 表达式分析结果: 0-未知，1-不成立, 2-成立
        /// </summary>
        public int AnalysisResult { get; set; }

       

        /// <summary>
        /// 分析结果描述
        /// </summary>
        private string _analysisResultText;

        /// <summary>
        /// 分析结果描述
        /// </summary>
        public string AnalysisResultText
        {
            get { return _analysisResultText; }
            set
            {
                if (_analysisResultText != value)
                {
                    _analysisResultText = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AnalysisResultText"));
                }
            }
        }
        /// <summary>
        /// 表达式配置的持续时间
        /// </summary>
        public int ContinueTime { get; set; }
        /// <summary>
        /// 表达式配置最大持续时间
        /// </summary>
        public int MaxContinueTime { get; set; }
        
        /// <summary>
        /// 实际持续时间
        /// </summary>
        private long _actualContinueTime;

        /// <summary>
        /// 实际持续时间
        /// </summary>
        public long ActualContinueTime
        {
            get { return _actualContinueTime; }
            set
            {
                if (_actualContinueTime != value)
                {
                    _actualContinueTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ActualContinueTime"));
                }
            }
        }

        /// <summary>
        /// 实际持续时间
        /// </summary>
        private string _showActualContinueTime;

        /// <summary>
        /// 实际持续时间
        /// </summary>
        public string ShowActualContinueTime
        {
            get { return _showActualContinueTime; }
            set
            {
                if (_showActualContinueTime != value)
                {
                    _showActualContinueTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ShowActualContinueTime"));
                }
            }
        }
    }


    public partial class AnalysisSuccessfulPointInfo
    {
        /// <summary>
        /// 测点Id
        /// </summary>
        public string PointId { get; set; }
        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Wz { get; set; }
    }
}
