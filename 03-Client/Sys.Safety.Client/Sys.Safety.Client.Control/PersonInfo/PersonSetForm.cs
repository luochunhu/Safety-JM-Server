using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Client.Define.Model;
using System.Collections;
using Basic.Framework.Common;

namespace Sys.Safety.Client.Control.PersonInfo
{
    public partial class PersonSetForm : XtraForm
    {
        /// <summary>
        /// 所有人员
        /// </summary>
        private List<R_PersoninfInfo> _allPerson = null;

        /// <summary>
        /// 查询人员
        /// </summary>
        private List<R_PersoninfInfo> _queryPerson = null;

        /// <summary>
        /// 已选人员
        /// </summary>
        public List<R_PersoninfInfo> SelectPerson = new List<R_PersoninfInfo>();

        public PersonSetForm()
        {
            InitializeComponent();
        }       
        

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonSetForm_Load(object sender, EventArgs e)
        {
            try
            {
                _allPerson = PersonInfoHandle.GetAllRPersoninfCache();
                _queryPerson = ObjectConverter.DeepCopy(_allPerson);
                GridControlAllPeople.DataSource = _queryPerson;

                GridControlSelectPeople.DataSource = SelectPerson;
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_Click(object sender, EventArgs e)
        {
            try
            {
                var name = TextEditName.Text;
                var rows = _allPerson.Where(a => a.Name.Contains(name)).ToList();
                _queryPerson = ObjectConverter.DeepCopy(rows);
                GridControlAllPeople.DataSource = _queryPerson;
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var selRows = GridViewAllPeople.GetSelectedRows();
                for (var i = 0; i < selRows.Length; i++)
                {
                    string id = GridViewAllPeople.GetRowCellValue(selRows[i], "Id").ToString();
                    bool exist = SelectPerson.Any(a => a.Id == id);
                    if (!exist)
                    {
                        var rows = _allPerson.Where(a => a.Id == id);
                        var copyRows = ObjectConverter.DeepCopy(rows);
                        SelectPerson.AddRange(copyRows);
                    }
                }
                GridControlSelectPeople.RefreshDataSource();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 添加所有按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectPerson.Clear();
                var queRows = GridControlAllPeople.DataSource as List<R_PersoninfInfo>;
                var copyRows = ObjectConverter.DeepCopy(queRows);
                SelectPerson = copyRows;
                GridControlSelectPeople.DataSource = SelectPerson;
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        private void ButDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var selRows = GridViewSelectPeople.GetSelectedRows();
                for (var i = selRows.Length - 1; i >= 0; i--)
                {
                    string id = GridViewSelectPeople.GetRowCellValue(selRows[i], "Id").ToString();
                    var rowData = SelectPerson.FirstOrDefault(a => a.Id == id);
                    SelectPerson.Remove(rowData);
                }
                GridControlSelectPeople.RefreshDataSource();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        private void ButDeleteAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectPerson.Clear();
                GridControlSelectPeople.RefreshDataSource();
            }
            catch (Exception exception)
            {
                LogHelper.Error(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
