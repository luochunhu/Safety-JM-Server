using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_ListEx")]
    public class ListexModel
    {
        /// <summary>
        ///     列表ID
        /// </summary>
        [Key]
        public int ListID { get; set; }

        /// <summary>
        /// </summary>
        public int DirID { get; set; }

        /// <summary>
        ///     列表编码
        /// </summary>
        public string StrListCode { get; set; }

        /// <summary>
        ///     列表名称
        /// </summary>
        public string StrListName { get; set; }

        /// <summary>
        ///     是否为列表
        /// </summary>
        public bool BlnList { get; set; }

        /// <summary>
        ///     主元数据ID
        /// </summary>
        public int MainMetaDataID { get; set; }

        /// <summary>
        ///     列表描述信息
        /// </summary>
        public string StrListDescription { get; set; }

        /// <summary>
        ///     支持报表
        /// </summary>
        public bool BlnPivot { get; set; }

        /// <summary>
        ///     支持图表
        /// </summary>
        public bool BlnChart { get; set; }

        /// <summary>
        ///     是否有效
        /// </summary>
        public bool BlnEnable { get; set; }

        /// <summary>
        ///     所需权限编码
        /// </summary>
        public string StrRightCode { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public string StrDescription { get; set; }

        /// <summary>
        ///     配置程序员
        /// </summary>
        public string Programer { get; set; }

        /// <summary>
        ///     最后修改日期
        /// </summary>
        public string LastUpdateDate { get; set; }

        /// <summary>
        ///     开发说明
        /// </summary>
        public string ProgramerNotes { get; set; }

        /// <summary>
        ///     是否预制
        /// </summary>
        public bool BlnPredefine { get; set; }

        /// <summary>
        ///     列表分组编码
        /// </summary>
        public string StrListGroupCode { get; set; }

        /// <summary>
        ///     列表唯一编码
        /// </summary>
        public string StrGuidCode { get; set; }
    }
}