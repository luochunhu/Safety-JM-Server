using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.ListReport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.DataAccess;
using Sys.Safety.Reports;

namespace Sys.Safety.Services
{
    public class ReportService : IReportService
    {
        private IListexRepository _Repository;
        private IListtempleRepository _ListTemplateRepository;
        private IListdatalayountRepository _ListdatalayountRepository;
        private ISqlService _SqlService;
        private IListdataexRepository _ListdataexRepository;
        private IListexRepository _ListexRepository;

        // 20171104
        private RepositoryBase<ListexModel> _repositoryBase;

        public ReportService()
        {
            _Repository = ServiceFactory.Create<IListexRepository>();

            // 20171104
            _repositoryBase = _Repository as RepositoryBase<ListexModel>;

            _ListTemplateRepository = ServiceFactory.Create<IListtempleRepository>();
            _ListdatalayountRepository = ServiceFactory.Create<IListdatalayountRepository>();
            _SqlService = ServiceFactory.Create<ISqlService>();
            _ListdataexRepository = ServiceFactory.Create<IListdataexRepository>();
            _ListexRepository = ServiceFactory.Create<IListexRepository>();
        }

        public BasicResponse<string> SaveDoTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取报表方案集合 web
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetListData(Sys.Safety.Request.ReportGetListDataRequest reportRequest)
        {
            BasicResponse<DataTable> response = new BasicResponse<DataTable>();
            response.Data = _Repository.QueryTable("global_Report_GetListData", new object[] { reportRequest.ListID });
            return response;
        }

        public BasicResponse<DataTable> Getlistdisplayex(Sys.Safety.Request.ReportGetlistdisplayexRequest reportRequest)
        {
            BasicResponse<DataTable> response = new BasicResponse<DataTable>();
            response.Data = _Repository.QueryTable("global_Report_Getlistdisplayex", new object[] { reportRequest.ListID });
            return response;
        }

        /// <summary>
        /// 得到报表需要显示的栏目 web
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> WebGetlistdisplayex(Sys.Safety.Request.ReportWebGetlistdisplayexRequest reportRequest)
        {
            BasicResponse<DataTable> response = new BasicResponse<DataTable>();
            response.Data = _Repository.QueryTable("global_Report_WebGetlistdisplayex", new object[] { reportRequest.ListID, reportRequest.ListDataID });
            return response;
        }

        public BasicResponse<DataTable> GetReportData(Sys.Safety.Request.ReportGetReportDataRequest reportRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<DataTable> WebGetReportData(Sys.Safety.Request.ReportWebGetReportDataRequest reportRequest)
        {
            BasicResponse<DataTable> response = new BasicResponse<DataTable>();

            var ListID = reportRequest.ListID;
            var ListDataID = reportRequest.ListDataID;
            var _strFreQryCondition = reportRequest._strFreQryCondition;
            var _listdate = reportRequest._listdate;
            //var pageNum = reportRequest.pageNum;
            //var perPageRecordNum = reportRequest.perPageRecordNum;
            int filterType = reportRequest.FilterType;
            int removeRepetType = reportRequest.RemoveRepetType;
            string arrangeTime = reportRequest.ArrangeTime;

            SetDayTableSql(_strFreQryCondition, ListID, _listdate, filterType, removeRepetType);

            if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
            {
                string strdate = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                _strFreQryCondition = _strFreQryCondition.Replace(strdate, " <> '1900-1-1'");
            }

            var item = _ListdataexRepository.Datas.FirstOrDefault(o => o.ListID == ListID && o.ListDataID == ListDataID);

            string strsql = string.Empty;
            if (item != null)
            {
                string strSortWhere = "";
                IList<ListdatalayountInfo> listDataLayoutDTO = this.GetListLayout(filterType, arrangeTime, ListDataID);
                strSortWhere = listDataLayoutDTO != null && listDataLayoutDTO.Count > 0 ? "and " + listDataLayoutDTO[0].StrConTextCondition : "";
                strsql = item.StrListSQL; 
                strsql = strsql.Replace("where 1=1", "where 1=1 " + _strFreQryCondition + strSortWhere);
            }
            if (this.GetDBType() == "mysql")
                strsql = strsql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");
            this.GetDistinctSql(ref strsql, ListID);

            SqlRequest sqlrequest = new SqlRequest
            {
                Sql = strsql
            };
            DataTable dt = _SqlService.QueryTableBySql(sqlrequest).Data;

            dt = GetPointSort(dt, this.GetListLayout(filterType, arrangeTime, ListDataID));
            //处理二维表(即行数据变为列数据的情况)
            DataTable dtListDisplay = this.Getlistdisplayex(ListID);
            DataRow[] rows1 = dtListDisplay.Select("lngProivtType=1");
            DataRow[] rows2 = dtListDisplay.Select("lngProivtType=2");
            DataRow[] rows3 = dtListDisplay.Select("lngProivtType=3");
            DataRow[] rows4 = dtListDisplay.Select("lngProivtType=4");


            DataRow[] rowssort = dtListDisplay.Select("strListDisplayFieldNameCHS='参数排序号'");//列排序暂时写死
            if (rows1.Length > 0 && rows2.Length > 0 && rows3.Length > 0)
            {
                string strColummFileName = Convert.ToString(rows1[0]["strListDisplayFieldName"]);
                string strRowFileName = Convert.ToString(rows2[0]["strListDisplayFieldName"]);
                string strValeFileName = Convert.ToString(rows3[0]["strListDisplayFieldName"]);
                string strColummChsName = Convert.ToString(rows2[0]["strListDisplayFieldNameCHS"]);
                string strColumnSortName = rowssort.Length > 0 ? rowssort[0]["strListDisplayFieldName"].ToString() : "";

                dt = RowConvertColumm(dt, strColummFileName, strColumnSortName, rows4, strColummChsName, strRowFileName, strValeFileName);
            }

            //如果是抽放日报表,则把viewjc_lld1_timer列改为字符类型，并把值改为yyyy-MM-dd格式
            if (ListID == 38) 
            {
                dt = ConvertAcculutionDataTable(dt);
            }

            response.Data = dt;
            return response;
        }

        public BasicResponse<string> GetFKData(Sys.Safety.Request.ReportGetFKDataRequest reportRequest)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取ListTemplate web
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        public BasicResponse<ListtempleInfo> GetListTemple(Sys.Safety.Request.ReportGetListTempleRequest reportRequest)
        {
            BasicResponse<ListtempleInfo> response = new BasicResponse<ListtempleInfo>();
            var item = _ListTemplateRepository.Datas.FirstOrDefault(o => o.ListDataID == reportRequest.ListDataID);
            if (item != null)
            {
                response.Data = ObjectConverter.Copy<ListtempleModel, ListtempleInfo>(item);
            }
            return response;
        }

        /// <summary>
        /// 得到报表查询条件参照数据 web
        /// </summary>
        /// <param name="reportRequest"></param>
        /// <returns></returns>
        public BasicResponse<Hashtable> GetFKDataByHashTable(Sys.Safety.Request.ReportGetFKDataByHashTableRequest reportRequest)
        {
            //如果测点参照要根据开始时间和结束时间过滤已删除的数据即  activitiy=1 or (activitiy=0 && 定义的时间在查询时间范围内)
            //则sql语句的条件应该这么写  activitiy=1 or (activitiy=0 && (开始时间  between  Createtime  and  deletetime  or 结束时间  between  Createtime  and  deletetime))

            BasicResponse<Hashtable> response = new BasicResponse<Hashtable>();

            var strFKCode = reportRequest.strFKCode;

            DataTable dtfklib = _Repository.QueryTable("global_Report_GetFKDataByHashTable1", new object[] { reportRequest.strFKCode });
            if (dtfklib == null || dtfklib.Rows.Count == 0)
            {
                throw new Exception("未找到参照编码为" + strFKCode + "的记录，请查看参照表配置！");
            }
            string strsql = Convert.ToString(dtfklib.Rows[0]["strSQL"]);

            SqlRequest sqlRequest = new SqlRequest
            {
                Sql = strsql
            };
            DataTable dataSource = _SqlService.QueryTableBySql(sqlRequest).Data;

            Hashtable lookInfo = new Hashtable();

            lookInfo.Add("StrDsiplayMember", Convert.ToString(dtfklib.Rows[0]["StrDsiplayMember"]).Trim());
            lookInfo.Add("StrValueMember", Convert.ToString(dtfklib.Rows[0]["StrValueMember"]).Trim());
            lookInfo.Add("StrColumns", Convert.ToString(dtfklib.Rows[0]["StrColumns"]).Trim());
            lookInfo.Add("StrParentField", Convert.ToString(dtfklib.Rows[0]["StrParentField"]).Trim());

            lookInfo.Add("dataSource", ConvertDataTable(dataSource));
            //string strReturn = PublicClass.JsonSerializer<Hashtable>(lookInfo);

            //测试反序列化
            //Hashtable bb = PublicClass.JsonDeserialize<Hashtable>(strReturn);
            //DataTable dtaa = PublicClass.JsonDeserialize<DataTable>(bb["dataSource"].ToString());

            response.Data = lookInfo;
            return response;
        }

        public BasicResponse<DataTable> GetEnvirLevelDetail(Sys.Safety.Request.ReportGetEnvirLevelDetailRequest reportRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<DataTable> GetPointParameter(Sys.Safety.Request.ReportGetPointParameterRequest reportRequest)
        {
            BasicResponse<DataTable> response = new BasicResponse<DataTable>();
            response.Data = _Repository.QueryTable("global_Report_GetPointParameter", new object[] { reportRequest.fzh });
            return response;
        }

        public BasicResponse<DataSet> GetMorePointData(Sys.Safety.Request.ReportGetMorePointDataRequest reportRequest)
        {
            throw new NotImplementedException();
        }

        public BasicResponse<int> GetReportTotalRecord(Sys.Safety.Request.ReportGetReportTotalRecordRequest reportRequest)
        {
            // 20171106
            BasicResponse<int> response = new BasicResponse<int>();

            int count = 0;

            var _strFreQryCondition = reportRequest._strFreQryCondition;
            var ListID = reportRequest.ListID;
            var _listdate = reportRequest._listdate;
            var ListDataID = reportRequest.ListDataID;
            int filterType = reportRequest.FilterType;
            int removeRepetType = reportRequest.RemoveRepetType;
            string arrangeTime = reportRequest.ArrangeTime;

            SetDayTableSql(_strFreQryCondition, ListID, _listdate, filterType, removeRepetType);

            if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
            {
                string strdate = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                _strFreQryCondition = _strFreQryCondition.Replace(strdate, " <> '1900-1-1'");
            }

            //string strsql = "select strListSQL from BFT_ListDataEx where ListID=" + ListID + " and ListDataID=" + ListDataID + " LIMIT 0,1 ";
            //DataTable dtSourceSql = _Repository.QueryTable(strsql);
            var item = _ListdataexRepository.Datas.FirstOrDefault(o => o.ListID == ListID && o.ListDataID == ListDataID);
            string strsql = string.Empty;
            if (item != null)
            {
                string strSortWhere = "";
                IList<ListdatalayountInfo> listDataLayoutDTO = this.GetListLayout(filterType, arrangeTime, ListDataID);
                strSortWhere = listDataLayoutDTO != null && listDataLayoutDTO.Count > 0
                    ? "and " + listDataLayoutDTO[0].StrConTextCondition
                    : "";
                strsql = item.StrListSQL;// Convert.ToString(dtSourceSql.Rows[0]["strListSQL"]);
                strsql = strsql.Replace("where 1=1", "where 1=1 " + _strFreQryCondition + strSortWhere);

            }
            if (this.GetDBType() == "mysql")
                strsql = strsql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");
            this.GetDistinctSql(ref strsql, ListID);
            //count = _Repository.GetTotalRecord(strsql);
            SqlRequest countsqlrequest = new SqlRequest
            {
                Sql = strsql
            };
            DataTable temptable = _SqlService.QueryTableBySql(countsqlrequest).Data;
            if (temptable != null && temptable.Rows.Count > 0)
                count = temptable.Rows.Count;

            response.Data = count;
            return response;
        }

        #region 私有方法

        /// <summary>
        /// 组织日表sql
        /// </summary>
        /// <param name="_strFreQryCondition"></param>
        /// <param name="ListID"></param>
        /// <param name="_listdate"></param>
        /// <param name="filterType">过滤类型。1：活动测点；2：存储测点；3：编排测点</param>
        /// <param name="removeRepetType">去重复类型。1：全部；2：去重复（删除前）；3：去重复（删除后）</param>
        private void SetDayTableSql(string _strFreQryCondition, int ListID, List<string> _listdate, int filterType, int removeRepetType)
        {
            #region 老代码

            //if (_listdate == null) return;//如果没有选择日期,那么将不重新组织sql

            //DataTable dtMetadata = _Repository.QueryTable("global_Report_GetCBFMetaData", new object[] { ListID });
            //DataRow[] rows = dtMetadata.Select("blnDay=1");//判断主元数是不是日表，如果是日表，现在默认认为这里面不管有多少个日期字段，反正只存了当天的数据，所以暂时不用判断是哪个元数据字段做为日表日期条件字段
            //if (rows == null || rows.Length == 0) return;

            //string strDayType = Convert.ToString(rows[0]["strDayType"]);
            //if (strDayType.ToLower() == "yyyymm")
            //{//如果是月表,要按照月表的格式来组织sql
            //    List<string> _listdatecopy = new List<string>();
            //    foreach (string strdate in _listdate)
            //    {
            //        string s = strdate.Substring(0, 6);
            //        if (!_listdatecopy.Contains(s))
            //        {
            //            _listdatecopy.Add(s);
            //        }
            //    }
            //    _listdate = _listdatecopy;
            //}


            //if (Convert.ToString(rows[0]["strSrcType"]) == "V")
            //{//如果是日表，并且建立的是视图,则需要动态修改视图的sql//                
            //    DataTable dtTable = this.GetDayTable(dtMetadata);
            //    string strupdatesql = " ";
            //    string strDayTable = "";
            //    foreach (DataRow row in dtTable.Rows)
            //    {
            //        strDayTable += "'" + row["strTableName"].ToString() + "',";
            //    }
            //    strDayTable = strDayTable.Substring(0, strDayTable.Length - 1);
            //    DataTable dt = null;
            //    //得到视图的创建sql 


            //    //todo 执行报错
            //    string dbName = GetDBName();
            //    SqlRequest request = new SqlRequest
            //    {
            //        Sql = string.Format("SELECT TABLE_NAME,VIEW_DEFINITION as Text FROM information_schema.VIEWS where TABLE_NAME in({0}) and TABLE_SCHEMA='{1}'", strDayTable, dbName)
            //    };
            //    dt = _SqlService.QueryTableBySql(request).Data;

            //    foreach (DataRow row in dtTable.Rows)
            //    {
            //        string strViewName = row["strTableName"].ToString();
            //        string strViewSrcTableName = row["strDayTableName"].ToString();
            //        DataRow[] rowscript = dt.Select("TABLE_NAME='" + strViewName + "'");
            //        string strsql = "";
            //        for (int i = 0; i < rowscript.Length; i++)
            //        {//dt里面存的是创建视图脚本，循环得到脚本
            //            string strvalue = Convert.ToString(rowscript[i]["Text"]);
            //            strsql += Convert.ToString(rowscript[i]["Text"]).ToLower();
            //            if (strsql.ToLower().Contains("union "))
            //            {
            //                strsql = strsql.Substring(0, strsql.IndexOf("union"));
            //                break;
            //            }
            //        }

            //        if (this.GetDBType() == "sqlserver")
            //            strupdatesql += "go\r\n alter view " + strViewName + " \r\n as\r\n ";
            //        if (this.GetDBType() == "mysql")
            //            strupdatesql += ";alter view " + strViewName + " \r\n as\r\n ";
            //        int k = 0;
            //        if (strDayType == "") //2016-10-21 ，由于抽放报表日，月，年没有分表，但是jc_ll_dmonthmax视图需要做时间where，所以需要统一修改jc_ll_dmonth的时间，相当于是虚拟日表
            //            strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
            //        else
            //            foreach (string s in _listdate)
            //            {
            //                if (_listdate.Count > 1 && !this.blnExistsTable(strViewSrcTableName + s))//如果日期段大于1天，且选择的日期在数据库中不存在，且直接跳过此表
            //                    continue;
            //                string[] strSrcTableNames = strViewSrcTableName.Split(',');
            //                foreach (string strSrcTableName in strSrcTableNames)
            //                {
            //                    string strname = strsql.Substring(strsql.IndexOf(strSrcTableName) + strSrcTableName.Length, strDayType.Length);
            //                    if (Convert.ToInt32(strname) > 0)
            //                    {
            //                        string strDataBaseTable = strSrcTableName + s;
            //                        if (_listdate.Count == 1 && !this.blnExistsTable(strDataBaseTable))
            //                        {
            //                            throw new Exception("无满足条件数据！");
            //                        }
            //                        strsql = strsql.Replace(strSrcTableName + strname, strSrcTableName + s + "");
            //                    }

            //                    else
            //                    {
            //                        strsql = strsql.Replace(strSrcTableName, strSrcTableName + s + "");
            //                    }
            //                }
            //                strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
            //            }
            //        if (strupdatesql.Contains("union all"))
            //        {
            //            strupdatesql = strupdatesql.Substring(0, strupdatesql.Length - 12);
            //            if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
            //            {
            //                //得到查询日期的日期字符串
            //                string strdate = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
            //                string strsqldate = "";

            //                string strsqlCreateView = strupdatesql.ToLower().Substring(0, strupdatesql.ToLower().IndexOf("as") + 2) + "\r\n";
            //                string strselectsql = strupdatesql.ToLower().Substring(strupdatesql.ToLower().IndexOf("as") + 2);
            //                strselectsql = GetSqlByDeleteNull(strselectsql);


            //                int[] strSqlArray = this.GetSubStrCountInStr(strselectsql, "between", 0);
            //                foreach (int startindex in strSqlArray)
            //                {
            //                    strsqldate = strselectsql.Substring(startindex, 55);
            //                    strupdatesql = strsqlCreateView + strselectsql.Replace(strsqldate, strdate);
            //                }

            //            }
            //            else
            //            {
            //                SqlRequest sqlrequest = new SqlRequest
            //                {
            //                    Sql = strupdatesql
            //                };
            //                _SqlService.ExecuteNonQueryBySql(sqlrequest);
            //            }

            //        }
            //        else
            //        {
            //            throw new Exception("无满足条件数据！");
            //        }
            //    }
            //    if (strupdatesql.Length > 0)
            //    {
            //        SqlRequest sqlrequest = new SqlRequest
            //        {
            //            Sql = strupdatesql
            //        };
            //        _SqlService.ExecuteNonQueryBySql(sqlrequest);
            //    }
            //}

            #endregion

            string sAllUpdateSql = "";

            #region 修改view_mdef视图

            string sDate2 = "";
            if (filterType == 1)
            {
                sDate2 = "between '9999-12-31 23:59:59' and '9999-12-31 23:59:59'";
            }
            else if (filterType == 2 || filterType == 3)
            {
                if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)     //有查询时间
                {
                    sDate2 = _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10, 55);
                    sDate2 = sDate2.Split(new string[] { "and" }, StringSplitOptions.None)[0] + "and" + " '9999-12-31 23:59:59'";
                }
                else        //没有查询时间
                {
                    sDate2 = "between '1900-01-01 00:00:00' and '9999-12-31 23:59:59'";
                }
            }

            string sAllPointView =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM `KJ_DeviceDefInfo` WHERE (( `KJ_DeviceDefInfo`.`DeleteTime` = '1900-01-01 00:00:00' ) OR ( `KJ_DeviceDefInfo`.`DeleteTime` " +
                sDate2 + " ))";
            string sRemoveDuplicationBefore =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM KJ_DeviceDefInfo AS c WHERE c.pointid IN ( SELECT ( SELECT pointid FROM KJ_DeviceDefInfo AS b WHERE b.point = temp.point AND activity = 0 ORDER BY b.DeleteTime DESC LIMIT 1 ) AS point FROM viewjc_mdefsubquerybef AS temp ) UNION ALL SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM KJ_DeviceDefInfo WHERE activity = 1 AND point NOT IN ( SELECT point FROM viewjc_mdefsubquerybef )";
            string sRemoveDuplicationAfter =
                "SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM KJ_DeviceDefInfo AS c WHERE c.pointid IN ( SELECT ( SELECT pointid FROM KJ_DeviceDefInfo AS b WHERE b.point = temp.point AND activity = 0 ORDER BY b.DeleteTime DESC LIMIT 1 ) AS point FROM viewjc_mdefsubqueryaft AS temp ) UNION ALL SELECT `fzh`, `kh`, `devid`, `wzid`, `point`, `pointid`, `pointid` AS `bid`, `bz6`, `bz7`, `bz8` FROM KJ_DeviceDefInfo WHERE activity = 1";
            string sDefSubSqlBef =
                "SELECT DISTINCT point FROM KJ_DeviceDefInfo AS a WHERE a.activity = 0 AND a.`DeleteTime` " + sDate2;
            string sDefSubSqlAft =
                "SELECT DISTINCT point FROM KJ_DeviceDefInfo AS a WHERE a.activity = 0 AND point NOT IN ( SELECT point FROM KJ_DeviceDefInfo WHERE activity = 1 ) AND a.`DeleteTime` " + sDate2;

            string sDefMainUseSql = "";
            string sDefSubUseSqlBef = "";
            string sDefSubUseSqlAft = "";
            if (filterType == 2)
            {
                if (removeRepetType == 2)
                {
                    sDefMainUseSql = sRemoveDuplicationBefore;
                    sDefSubUseSqlBef = sDefSubSqlBef;
                }
                else if (removeRepetType == 3)
                {
                    sDefMainUseSql = sRemoveDuplicationAfter;
                    sDefSubUseSqlAft = sDefSubSqlAft;
                }
                else
                {
                    sDefMainUseSql = sAllPointView;
                }
            }
            else
            {
                sDefMainUseSql = sAllPointView;
            }

            if (this.GetDBType() == "sqlserver")
            {
                sDefMainUseSql = "go\r\n alter view viewjc_mdef \r\n as\r\n " + sDefMainUseSql;
                if (!string.IsNullOrEmpty(sDefSubUseSqlBef))
                {
                    sDefSubUseSqlBef = "go\r\n alter view viewjc_mdefsubquerybef \r\n as\r\n " + sDefSubUseSqlBef;
                }
                if (!string.IsNullOrEmpty(sDefSubUseSqlAft))
                {
                    sDefSubUseSqlAft = "go\r\n alter view viewjc_mdefsubqueryaft \r\n as\r\n " + sDefSubUseSqlAft;
                }
            }
            if (this.GetDBType() == "mysql")
            {
                sDefMainUseSql = "alter view viewjc_mdef \r\n as\r\n " + sDefMainUseSql;
                if (!string.IsNullOrEmpty(sDefSubUseSqlBef))
                {
                    sDefSubUseSqlBef = ";alter view viewjc_mdefsubquerybef \r\n as\r\n " + sDefSubUseSqlBef;
                }
                if (!string.IsNullOrEmpty(sDefSubUseSqlAft))
                {
                    sDefSubUseSqlAft = ";alter view viewjc_mdefsubqueryaft \r\n as\r\n " + sDefSubUseSqlAft;
                }
            }

            sAllUpdateSql += sDefMainUseSql + sDefSubUseSqlBef + sDefSubUseSqlAft;

            #endregion
            
            #region 修改日表视图
            if (_listdate != null) ; //如果没有选择日期,那么将不重新组织sql
            {
                //var dtMetadata = ClientCacheModel.GetServerMetaData(listExvo.MainMetaDataID);
                DataTable dtMetadata = _Repository.QueryTable("global_Report_GetCBFMetaData", new object[] { ListID });
                var rows = dtMetadata.Select("blnDay=1");
                //判断主元数是不是日表，如果是日表，现在默认认为这里面不管有多少个日期字段，反正只存了当天的数据，所以暂时不用判断是哪个元数据字段做为日表日期条件字段
                if (rows != null && rows.Length != 0)
                {
                    var strDayType = TypeUtil.ToString(rows[0]["strDayType"]);
                    if (strDayType.ToLower() == "yyyymm")
                    {
                        //如果是月表,要按照月表的格式来组织sql
                        var _listdatecopy = new List<string>();
                        foreach (var strdate in _listdate)
                        {
                            var s = strdate.Substring(0, 6);
                            if (!_listdatecopy.Contains(s))
                                _listdatecopy.Add(s);
                        }
                        _listdate = _listdatecopy;
                    }

                    var strupdatesql = "";
                    if (TypeUtil.ToString(rows[0]["strSrcType"]) == "V")
                    {
                        //如果是日表，并且建立的是视图,则需要动态修改视图的sql//                
                        var dtTable = GetDayTable(dtMetadata);

                        // 20170916
                        if (dtTable.Rows.Count != 0)
                        {
                            var strDayTable = "";
                            foreach (DataRow row in dtTable.Rows)
                                strDayTable += "'" + row["strTableName"] + "',";
                            strDayTable = strDayTable.Substring(0, strDayTable.Length - 1);

                            DataTable dt = null;
                            //得到视图的创建sql                            
                            dt = _Repository.QueryTable("global_ReportService_GetViewFromViews", strDayTable,
                                this.GetDBName());

                            foreach (DataRow row in dtTable.Rows)
                            {
                                var strViewName = row["strTableName"].ToString();
                                var strViewSrcTableName = row["strDayTableName"].ToString();
                                var rowscript = dt.Select("TABLE_NAME='" + strViewName + "'");
                                var strsql = "";
                                for (var i = 0; i < rowscript.Length; i++)
                                {
                                    //dt里面存的是创建视图脚本，循环得到脚本

                                    var strvalue = TypeUtil.ToString(rowscript[i]["Text"]);
                                    if (this.GetDBType() == "sqlserver")
                                        strvalue = strvalue.Substring(strvalue.ToLower().IndexOf("as") + 2);

                                    strsql += strvalue.ToLower();
                                    if (strsql.ToLower().Contains("union "))
                                    {
                                        strsql = strsql.Substring(0, strsql.IndexOf("union"));
                                        break;
                                    }
                                }

                                if (this.GetDBType() == "sqlserver")
                                    strupdatesql += "go\r\n alter view " + strViewName + " \r\n as\r\n ";
                                if (this.GetDBType() == "mysql")
                                    strupdatesql += ";alter view " + strViewName + " \r\n as\r\n ";
                                var k = 0;

                                if (strDayType == "")
                                    //2016-10-21 ，由于抽放报表日，月，年没有分表，但是jc_ll_dmonthmax视图需要做时间where，所以需要统一修改jc_ll_dmonth的时间，相当于是虚拟日表
                                    strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
                                else
                                    foreach (var s in _listdate)
                                    {
                                        if ((_listdate.Count > 1) && !this.blnExistsTable(strViewSrcTableName + s))
                                            //如果日期段大于1天，且选择的日期在数据库中不存在，且直接跳过此表
                                            continue;
                                        var strSrcTableNames = strViewSrcTableName.Split(',');
                                        foreach (var strSrcTableName in strSrcTableNames)
                                        {
                                            var strname = strsql.Substring(
                                                strsql.IndexOf(strSrcTableName) + strSrcTableName.Length, strDayType.Length);
                                            if (TypeUtil.ToInt(strname) > 0)
                                            {
                                                var strDataBaseTable = strSrcTableName + s;
                                                if ((_listdate.Count == 1) && !this.blnExistsTable(strDataBaseTable))
                                                {
                                                    //gridControl.DataSource = null;
                                                    throw new Exception("无满足条件数据！");
                                                }
                                                strsql = strsql.Replace(strSrcTableName + strname, strSrcTableName + s + "");
                                            }

                                            else
                                            {
                                                strsql = strsql.Replace(strSrcTableName, strSrcTableName + s + "");
                                            }
                                        }
                                        strupdatesql = strupdatesql + strsql + "\r\n union all\r\n ";
                                    }
                                if (strupdatesql.Contains("union all"))
                                {
                                    strupdatesql = strupdatesql.Substring(0, strupdatesql.Length - 12);
                                    if (_strFreQryCondition.ToLower().IndexOf("datsearch") > 0)
                                    {
                                        //得到查询日期的日期字符串
                                        var strdate =
                                            _strFreQryCondition.Substring(_strFreQryCondition.ToLower().IndexOf("datsearch") + 10,
                                                55);
                                        var strsqldate = "";

                                        var strsqlCreateView =
                                            strupdatesql.ToLower().Substring(0, strupdatesql.ToLower().IndexOf("as") + 2) + "\r\n";
                                        var strselectsql = strupdatesql.ToLower()
                                            .Substring(strupdatesql.ToLower().IndexOf("as") + 2);
                                        strselectsql = GetSqlByDeleteNull(strselectsql);


                                        var strSqlArray = TypeUtil.GetSubStrCountInStr(strselectsql, "between", 0);
                                        foreach (var startindex in strSqlArray)
                                        {
                                            strsqldate = strselectsql.Substring(startindex, 55);
                                            strupdatesql = strsqlCreateView + strselectsql.Replace(strsqldate, strdate);
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("无满足条件数据！");
                                }
                            }
                        }

                        sAllUpdateSql += strupdatesql;
                    }
                }
            }

            #endregion

            if (sAllUpdateSql.Length > 0)
                _repositoryBase.ExecuteNonQueryBySql(sAllUpdateSql);
        }

        /// <summary>
        /// 根据元数据表(BFT_MetaData)中的字段strDayTableName来解析具体的表
        /// </summary>
        /// <param name="dtTable"></param>
        /// <returns></returns>
        private DataTable GetDayTable(DataTable dtTable)
        {
            DataTable dtTableCopy = dtTable.Clone();
            foreach (DataRow row in dtTable.Rows)
            {//根据列表用的元数据，然后去看MeataData表中strDayTableName字段，主要是用于strDayTableName也是视图的情况
                string strViewName = Convert.ToString(row["strTableName"]);
                string strViewSrcTableName = Convert.ToString(row["strDayTableName"]).ToLower();
                if (strViewSrcTableName.Contains(";"))
                {

                    string[] strs = strViewSrcTableName.Split(';');
                    foreach (string ss in strs)
                    {
                        if (ss == "") continue;
                        string[] s = ss.Split(':');
                        foreach (string stable in s[1].Split(','))
                        {
                            DataRow r = dtTableCopy.NewRow();
                            r["strName"] = "";
                            r["strDayTableName"] = stable;
                            r["strTableName"] = s[0];
                            dtTableCopy.Rows.Add(r);
                        }
                    }
                }
            }

            return dtTableCopy;
        }

        private string GetDBType()
        {
            return Basic.Framework.Configuration.Global.DatabaseType.ToString().ToLower();
        }

        private string GetDBName()
        {
            return _Repository.DatebaseName;
        }

        private bool blnExistsTable(string strTableName)
        {//暂时取消判断表是否存在，以免影响效率
            string strDBType = this.GetDBType();
            DataTable dt = null;
            //string strsql = "";
            if (strDBType == "sqlserver")
            {
                //strsql = "select object_id as oid,name from sys.objects where name='" + strTableName + "'";
                dt = _Repository.QueryTable("global_Report_ExistsTable", new object[] { strTableName });
            }
            if (strDBType == "mysql")
            {
                //strsql = "select TABLE_CATALOG as oid ,table_name as name from information_schema.tables where table_name='" + strTableName + "' and TABLE_SCHEMA='" + this.GetDBName() + "'";
                dt = _Repository.QueryTable("global_Report_ExistsTable", new object[] { strTableName, GetDBName() });
            }

            if (dt == null || dt.Rows.Count == 0)
                return false;
            else
                return true;
        }

        private string GetSqlByDeleteNull(string strsql)
        {
            string sql = strsql.Replace("\r\n", " ");
            bool blnFor = true;
            while (blnFor)
            {
                sql = sql.Replace("  ", " ");
                if (!sql.Contains("  "))
                    blnFor = false;
            }
            return sql;
        }

        /// <summary>
        /// 得到一个字符串在另一个字符合串出现的所有位置索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="substr"></param>
        /// <param name="StartPos"></param>
        /// <returns></returns>
        private int[] GetSubStrCountInStr(String str, String substr, int StartPos)
        {
            int foundPos = -1;
            int count = 0;
            List<int> foundItems = new List<int>();
            do
            {
                foundPos = str.IndexOf(substr, StartPos);
                if (foundPos > -1)
                {
                    StartPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }
            } while (foundPos > -1 && StartPos < str.Length);

            return ((int[])foundItems.ToArray());
        }

        private IList<ListdatalayountInfo> GetListLayout(int filterType, string arrangeTime, int ListDataID)
        {
            // 20171109
            if (filterType != 3) return null;

            IList<ListdatalayountInfo> listlayount = null;
            if (!string.IsNullOrEmpty(arrangeTime))
            {
                listlayount = this.GetListDataLayoutData(ListDataID, arrangeTime);
            }
            return listlayount;
        }

        private void GetDistinctSql(ref string strListSql, int ListID)
        {
            //DataTable dt = _Repository.QueryTable("select * from BFT_ListEx where ListID=" + ListID);

            var listexitem = _ListexRepository.Datas.FirstOrDefault(o => o.ListID == ListID);
            if (listexitem != null && listexitem.StrListCode == "MLLKDDay")
                strListSql = strListSql.Insert(6, " distinct ");
        }

        /// <summary>
        /// 得到测点编排
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        private IList<ListdatalayountInfo> GetListDataLayountData(int ListDataID)
        {
            //return listdatalayoutservice.GetListExtend("from ListDataLayoutEntity where  ListDataID= " + ListDataID);

            var items = _ListdatalayountRepository.Datas.Where(o => o.ListDataID == ListDataID).ToList();
            return ObjectConverter.CopyList<ListdatalayountModel, ListdatalayountInfo>(items);
        }


        /// <summary>
        /// 得到编排设置
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        private IList<ListdatalayountInfo> GetListDataLayoutData(int ListDataID, string strDate)
        {
            //return listdatalayoutservice.GetList("from ListDataLayoutEntity where  ListDataID= " + ListDataID + "  and  StrDate<='" + strDate + "'order by StrDate desc");

            var items = _ListdatalayountRepository.QueryTable("global_Report_GetListDataLayoutData", new object[] { ListDataID, strDate });
            return _ListdataexRepository.ToEntityFromTable<ListdatalayountInfo>(items);
        }

        private DataTable GetPointSort(DataTable dt, IList<ListdatalayountInfo> listlayount)
        {
            DataTable t = dt;
            if (listlayount != null && listlayount.Count > 0)
            {
                string strSortText = Convert.ToString(listlayount[0].StrCondition);
                strSortText = strSortText.Substring(5);
                string strSortFileName = listlayount[0].StrFileName;
                t = dt.Clone();
                t.Clear();
                string[] str = strSortText.Split(',');
                foreach (string s in str)
                {
                    DataRow[] rowss = dt.Select(strSortFileName + "=" + s + "");
                    foreach (DataRow row in rowss)
                    {
                        t.Rows.Add(row.ItemArray);
                    }

                }
                dt = t;

            }
            return dt;
        }

        public DataTable Getlistdisplayex(int ListID)
        {
            return _Repository.QueryTable("global_Report_Getlistdisplayex", new object[] { ListID });
        }

        /// <summary>
        /// DataTable行数据转化为列数据
        /// </summary>
        /// <param name="SourceTable">要转换的DataTable</param>
        /// <param name="strColummFileName">需要转换为列的列名(主列)</param>
        /// <param name="strRowFileName">转换列后列的排序</param>
        /// <param name="strRowFileName">需要转换为行的列名</param>
        /// <returns></returns>
        private DataTable RowConvertColumm(DataTable SourceTable, string strColummFileName, string strColumnSortName, DataRow[] strColumnFileNameExtened, string strRowChsName, string strRowFileName, string strValeFileName)
        {

            //从DataTable中读取不重复的日期行，用来构造新DataTable的列
            if (Convert.ToString(strColumnSortName).Length > 0)
                SourceTable.DefaultView.Sort = strColumnSortName;
            DataTable distinct_date = SourceTable.DefaultView.ToTable(true, strColummFileName);

            DataTable new_DataTable = new DataTable("TestTable");
            //将客户名称列添加到新表中
            DataColumn new_d_col = new DataColumn();
            new_d_col.ColumnName = strRowChsName;
            new_d_col.Caption = strRowChsName;
            new_DataTable.Columns.Add(new_d_col);

            StringBuilder str_sum = new StringBuilder();

            //开始在新表中构造日期列
            foreach (DataRow dr in distinct_date.Rows)
            {
                if (dr[strColummFileName].ToString() == "") continue;
                new_d_col = new DataColumn();
                new_d_col.DataType = typeof(decimal);
                new_d_col.ColumnName = dr[strColummFileName].ToString();
                new_d_col.Caption = dr[strColummFileName].ToString();
                new_d_col.DefaultValue = 0;
                new_DataTable.Columns.Add(new_d_col);

                //如果设置了行区扩展,则需要循环加行区扩展属性(行区扩展可以理解为附带属性，可能不需要显示出来，可能会根据此列做一些其他处理)
                if (strColumnFileNameExtened == null || strColumnFileNameExtened.Length == 0) continue;
                foreach (DataRow row in strColumnFileNameExtened)
                {
                    DataColumn new_d_cola = new DataColumn();
                    new_d_cola.DataType = typeof(string);
                    string columnName = Convert.ToString(row["strListDisplayFieldName"]) + Convert.ToString(dr[strColummFileName]);
                    new_d_cola.ColumnName = columnName;
                    new_d_cola.Caption = columnName;
                    new_d_cola.DefaultValue = "";
                    new_DataTable.Columns.Add(new_d_cola);

                    //这个的目的是为合计列构造expression
                    str_sum.Append("+[").Append(new_d_col.ColumnName).Append("]");
                }

            }

            //将合计列添加到新表中  2016-03-18  暂时不用合计，没有此需求
            //new_d_col = new DataColumn();
            //new_d_col.DataType = typeof(decimal);
            //new_d_col.ColumnName = "Sum";
            //new_d_col.Caption = "合计";
            //new_d_col.DefaultValue = 0;
            //new_d_col.Expression = str_sum.ToString().Substring(1);
            //new_DataTable.Columns.Add(new_d_col);


            /*好了，到此新表已经构建完毕，下面开始为新表添加数据*/

            //从原DataTable中读出不重复的客户名称，以客户名称为关键字来构造新表的行
            SourceTable.DefaultView.Sort = strRowFileName; //2016-05-03  ，按日期排序
            DataTable distinct_object = SourceTable.DefaultView.ToTable(true, strRowFileName);
            DataRow[] drs;
            DataRow new_dr;
            foreach (DataRow dr in distinct_object.Rows)
            {
                new_dr = new_DataTable.NewRow();
                new_dr[strRowChsName] = dr[strRowFileName].ToString();

                foreach (DataRow _dr in distinct_date.Rows)
                {
                    drs = SourceTable.Select(strRowFileName + "='" + dr[strRowFileName].ToString() + "' and " + strColummFileName + "='" + _dr[strColummFileName].ToString() + "'");
                    if (drs.Length != 0)
                    {
                        new_dr[_dr[strColummFileName].ToString()] = Math.Round(Convert.ToDecimal(drs[0][strValeFileName]), 2);
                        if (Convert.ToString(strColumnFileNameExtened) != "")
                        {//如果设置了行区扩展，则需要给行区扩展列赋值
                            foreach (DataRow row in strColumnFileNameExtened)
                            {
                                new_dr[Convert.ToString(row["strListDisplayFieldName"]) + Convert.ToString(_dr[strColummFileName])] = Convert.ToString(drs[0][row["strListDisplayFieldName"].ToString()]);
                            }
                        }
                    }
                }
                new_DataTable.Rows.Add(new_dr);
            }

            return new_DataTable;


        }

        /// <summary>
        /// 把datatable里面long类型改为字符类型，因为long类型转json会丢失精度。
        /// </summary>
        /// <param name="olddt"></param>
        /// <returns></returns>
        private DataTable ConvertDataTable(DataTable olddt)
        {
            DataTable newdt = new DataTable();

            if (olddt.Rows.Count > 0)
            {
                newdt = olddt.Clone();

                //List<string> listColums = new List<string>();
                foreach (DataColumn col in newdt.Columns)
                {
                    //listColums.Add(col.ColumnName);
                    if (col.DataType.FullName == "System.Int64")
                    {
                        col.DataType = Type.GetType("System.String");
                    }
                }

                foreach (DataRow row in olddt.Rows)
                {
                    DataRow newDtRow = newdt.NewRow();
                    foreach (DataColumn column in olddt.Columns)
                    {
                        if (column.DataType.FullName == "System.Int64")
                        {
                            newDtRow[column.ColumnName] = Convert.ToString(row[column.ColumnName]);
                        }
                        else
                        {
                            newDtRow[column.ColumnName] = row[column.ColumnName];
                        }
                    }
                    newdt.Rows.Add(newDtRow);
                }

            }
            return newdt;
        }

        /// <summary>
        /// 把抽放日报表里时间改为yyyy-MM-dd格式
        /// </summary>
        /// <param name="olddt"></param>
        /// <returns></returns>
        private DataTable ConvertAcculutionDataTable(DataTable olddt) 
        {
            DataTable newdt = new DataTable();
            if (olddt.Rows.Count > 0) 
            {
                newdt = olddt.Clone();
                if (newdt.Columns.Contains("viewjc_lld1_timer")) 
                {
                    newdt.Columns["viewjc_lld1_timer"].DataType = Type.GetType("System.String");
                }

                foreach (DataRow row in olddt.Rows)
                {
                    DataRow newDtRow = newdt.NewRow();

                    foreach (DataColumn column in olddt.Columns)
                    {
                        if (column.ColumnName == "viewjc_lld1_timer")
                        {
                            newDtRow[column.ColumnName] = Convert.ToDateTime(row[column.ColumnName].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            newDtRow[column.ColumnName] = row[column.ColumnName];
                        }
                    }
                    newdt.Rows.Add(newDtRow);
                }

            }
            return newdt;
        }

        #endregion
    }
}
