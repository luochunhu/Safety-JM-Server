using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sys.Safety.Client.Define.Model
{
    /// <summary>
    /// TreeList数据源
    /// </summary>
    public class TreeListSource
    {
        /// <summary>
        /// 设备数据源
        /// </summary>
        public static ArrayList TreeListDev = new ArrayList();

        /// <summary>
        /// 设备类型数据源
        /// </summary>
        public static ArrayList TreeListDevType = new ArrayList();

        /// <summary>
        /// 通道信息数据源
        /// </summary>
        public static ArrayList TreeListChannle = new ArrayList();

        /// <summary>
        /// 分站数据源
        /// </summary>
        public static ArrayList TreeListStation = new ArrayList();

        /// <summary>
        /// 安装位置数据源
        /// </summary>
        public static ArrayList TreeListWZ = new ArrayList();
    }
    /// <summary>
    /// TreeList数据源对象
    /// </summary>
    public class TreeListItem
    {
        public TreeListItem()
        {

        }
        public TreeListItem(string name,string code ,string type,int id,int ParentID)
        {
            m_sName = name;
            m_sCode = code;
            m_Type = type;
            m_iID = id;
            m_iParentID = ParentID;
        }


        public TreeListItem(string name, string code, string type, string pragram1, string pragram2, int id, int ParentID)
        {
            m_sName = name;
            m_sCode = code;
            m_Type = type;
            m_iID = id;
            m_iParentID = ParentID;
            m_pragram1 = pragram1;
            m_pragram2 = pragram2;
        }

        //名称字段变量
        private string m_sName = string.Empty;
        //编码字段变量
        private string m_sCode = string.Empty;
        //选择字段类型
        private string m_Type = string.Empty;
        //子Node节点ID变量
        private int m_iID = -1;
        //父Node节点ID变量
        private int m_iParentID = -1;
        //参数1
        private string m_pragram1 = string.Empty;
        //参数2
        private string m_pragram2 = string.Empty;

        public int ID
        {
            get
            {
                return m_iID;
            }
            set
            {
                m_iID = value;
            }
        }
        public int ParentID
        {
            get
            {
                return m_iParentID;
            }
            set
            {
                m_iParentID = value;
            }
        }
        public string Name
        {
            get
            {
                return m_sName;
            }
            set
            {
                m_sName = value;
            }
        }
        public string Tag
        {
            get
            {
                return m_Type;
            }

            set
            {
                m_Type = value;
            }
        }
        public string Code
        {
            get
            {
                return m_sCode;
            }
            set
            {
                m_sCode = value;
            }
        }

        public string Pragram1
        {
            get
            {
                return m_pragram1;
            }
            set
            {
                m_pragram1 = value;
            }
        }
        public string Pragram2
        {
            get
            {
                return m_pragram2;
            }
            set
            {
                m_pragram2 = value;
            }
        }
    }
}
