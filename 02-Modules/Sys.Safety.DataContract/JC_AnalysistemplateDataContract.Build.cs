using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sys.Safety.Enums.Enums;
using System.ComponentModel;

namespace Sys.Safety.DataContract
{
    public partial class JC_AnalysisTemplateInfo : Basic.Framework.Web.BasicInfo,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string _id;
        /// <summary>
        /// 模板ID
        /// </summary>
        public string Id
        {
            get { return _id; }
            set {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            }
        }
        private string _name;
        /// <summary>
        /// 模板名称
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
        /// </summary>
        public DateTime UpdatedTime
        {
            get;
            set;
        }
    }
}


