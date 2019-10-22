using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sys.Safety.Enums.Enums;
using System.ComponentModel;
using Sys.Safety.DataContract.Custom;

namespace Sys.Safety.DataContract
{
    public partial class JC_LargedataAnalysisConfigInfo : CheckInfo, INotifyPropertyChanged

    {
       
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string _id;

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        /// <summary>
        /// 分析模型名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 分析模型名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        /// <summary>
        /// 分析模板ID
        /// </summary>
        public string TempleteId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用(1：启用（默认）；2：停用)
        /// </summary>
        public EnableState IsEnabled
        {
            get;
            set;
        }
      
        /// <summary>
        /// 满足条件时输出值
        /// </summary>
        private string _trueDescription;
        /// <summary>
        /// 满足条件时输出值
        /// </summary>
        public string TrueDescription
        {
            get { return _trueDescription; }
            set
            {
                if (_trueDescription != value)
                {
                    _trueDescription = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("TrueDescription"));
                }
            }
        }
       
        /// <summary>
        /// 表达式实时分析结果列表
        /// </summary>
        private List<ExpressionRealTimeResultInfo> _expressionRealTimeResultList;
        /// <summary>
        /// 表达式实时分析结果列表
        /// </summary>
        public List<ExpressionRealTimeResultInfo> ExpressionRealTimeResultList
        {
            get { return _expressionRealTimeResultList; }
            set
            {
                if (_expressionRealTimeResultList != value)
                {
                    _expressionRealTimeResultList = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("ExpressionRealTimeResultList"));
                }
            }
        }
        /// <summary>
        /// 不满足条件时输出值
        /// </summary>
        private string _falseDescription;
        /// <summary>
        /// 不满足条件时输出值
        /// </summary>
        public string FalseDescription
        {
            get { return _falseDescription; }
            set
            {
                if (_falseDescription != value)
                {
                    _falseDescription = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("FalseDescription"));
                }
            }
        }

        /// <summary>
        /// 分析周期, 1-3600秒
        /// </summary>
        private int _analysisInterval;
        /// <summary>
        /// 分析周期, 1-3600秒
        /// </summary>
        public int AnalysisInterval
        {
            get { return _analysisInterval; }
            set
            {
                if (_analysisInterval != value)
                {
                    _analysisInterval = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AnalysisInterval"));
                }
            }
        }
      
        /// <summary>
        /// 0-未知，1-不成立, 2-成立
        /// </summary>
        public int AnalysisResult
        {
            get;
            set;
        }

        /// <summary>
        /// 分析时间
        /// </summary>
        private DateTime? _analysisTime;
        /// <summary>
        /// 分析时间
        /// </summary>
        public DateTime? AnalysisTime
        {
            get { return _analysisTime; }
            set
            {
                if (_analysisTime != value)
                {
                    _analysisTime = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("AnalysisTime"));
                }
            }
        }
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
        /// 删除标识（1：未删除（默认）；2：已删除）
        /// </summary>
        public DeleteState IsDeleted
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreatorId
        {
            get;
            set;
        }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreatorName
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary
        public DateTime UpdatedTime
        {
            get;
            set;
        }
    }
}