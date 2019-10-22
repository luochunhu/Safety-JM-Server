using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Reflection;
using Basic.Framework.Common;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Listcommandex;
using Sys.Safety.Request.Listdataex;
using Sys.Safety.Request.Listdatalayount;
using Sys.Safety.Request.Listdataremark;
using Sys.Safety.Request.Listdisplayex;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.Listmetadata;
using Sys.Safety.Request.Metadata;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.ListReport;

namespace Sys.Safety.Reports.Model
{

    public class ListExModel
    {
        readonly ISqlService _sqlService = ServiceFactory.Create<ISqlService>();
        readonly IListexService _listexservice = ServiceFactory.Create<IListexService>();
        readonly IListdataexService _listdataexservice = ServiceFactory.Create<IListdataexService>();
        readonly IListcommandexService _listcommandexService = ServiceFactory.Create<IListcommandexService>();
        readonly IListdisplayexService _listdisplayexService = ServiceFactory.Create<IListdisplayexService>();
        readonly IListmetadataService _listmetadataService = ServiceFactory.Create<IListmetadataService>();
        readonly IListdatalayountService _listdatalayountService = ServiceFactory.Create<IListdatalayountService>();
        readonly IListtempleService _listtempleService = ServiceFactory.Create<IListtempleService>();
        readonly IMetadataService _metadataService = ServiceFactory.Create<IMetadataService>();
        readonly IListdataremarkService _listdataremarkService = ServiceFactory.Create<IListdataremarkService>();

        public string GetDbType()
        {
            var ret = _listexservice.GetDBType();
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
            return ret.Data;
        }

        public string GetDBName()
        {
            var ret = _listexservice.GetDBName();
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
            return ret.Data;
        }

        public DataTable GetDataTable(string strsql)
        {
            var ret = _listexservice.GetDBType();
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
            if (ret.Data == "mysql")
                strsql = strsql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");

            var req = new SqlRequest()
            {
                Sql = strsql
            };
            var ret2 = _sqlService.QueryTableBySql(req);
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
            return ret2.Data;
        }

        public int ExecuteSQL(string strsql)
        {
            var ret2 = _listexservice.GetDBType();
            if (!ret2.IsSuccess)
            {
                throw new Exception(ret2.Message);
            }
            if (ret2.Data == "mysql")
                strsql = strsql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");
            var req = new SqlRequest()
            {
                Sql = strsql
            };
            var ret = _sqlService.ExecuteNonQueryBySql(req);
            if (!ret.IsSuccess)
            {
                throw new Exception(ret.Message);
            }
            return ret.Data;

        }


        public void SaveVO(object T)
        {
            // 20170619
            string strname = T.GetType().Name;
            if (strname.ToLower().Trim() == "listexinfo")
            {
                var request = new SaveListExInfoRequest()
                {
                    Info = (ListexInfo)T
                };
                var res = _listexservice.SaveListExInfo(request);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
            }
            if (strname.ToLower().Trim() == "listdataexinfo")
            {
                var req = new SaveListDataExInfoRequest
                {
                    Info = (ListdataexInfo) T
                };
                var ret = _listdataexservice.SaveListDataExInfo(req);
                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
            }
            if (strname.ToLower().Trim() == "listcommandexinfo")
            {
                var req = new SaveListCommandInfoRequest
                {
                    Info = (ListcommandexInfo)T
                };
                var ret = _listcommandexService.SaveListCommandInfo(req);
                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
            }
            if (strname.ToLower().Trim() == "listdisplayexinfo")
            {
                var req = new SaveListDisplayExInfoRequest
                {
                    Info = (ListdisplayexInfo)T
                };
                var ret = _listdisplayexService.SaveListDisplayExInfo(req);
                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
            }
            if (strname.ToLower().Trim() == "listmetadatainfo")
            {
                var req = new SaveListMetaDataExInfoRequest
                {
                    Info = (ListmetadataInfo)T
                };
                var ret = _listmetadataService.SaveListMetaDataExInfo(req);
                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
            }
            if (strname.ToLower().Trim() == "listdatalayountinfo")
            {
                var req = new SaveListDataLayountInfoRequest()
                {
                    Info = (ListdatalayountInfo)T
                };
                var ret = _listdatalayountService.SaveListDataLayountInfo(req);
                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
            }

        }

        public void DeleteListDataLay(string time, long listDataId)
        {
            var req = new DeleteListdatalayountByTimeListDataIdRequest()
            {
                Time = time,
                ListDataId = listDataId.ToString()
            };
            var res = _listdatalayountService.DeleteListdatalayountByTimeListDataId(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        public object GetVO(string strdtoname, int ID)
        {
            string strname = strdtoname;
            object o = null;
            if (strname.ToLower().Trim() == "listexinfo")
            {
                var req = new ListexGetRequest()
                {
                    Id = ID.ToString()
                };
                var res = _listexservice.GetListexById(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                o = res.Data;
            }
            if (strname.ToLower().Trim() == "listdatainfo")
            {
                var ret = _listdataexservice.GetListdataexById(new ListdataexGetRequest
                {
                    Id = ID.ToString()
                });

                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
                else
                {
                    o = ret.Data;
                }
            }
            if (strname.ToLower().Trim() == "listcommandexinfo")
            {
                var ret = _listcommandexService.GetListcommandexById(new ListcommandexGetRequest
                {
                    Id = ID.ToString()
                });

                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
                else
                {
                    o = ret.Data;
                }
            }
            if (strname.ToLower().Trim() == "listdisplayexinfo")
            {
                var ret = _listdisplayexService.GetListdisplayexById(new ListdisplayexGetRequest
                {
                    Id = ID.ToString()
                });

                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
                else
                {
                    o = ret.Data;
                }
            }
            if (strname.ToLower().Trim() == "listmetadatainfo")
            {
                var ret = _listmetadataService.GetListmetadataById(new ListmetadataGetRequest
                {
                    Id = ID.ToString()
                });

                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
                else
                {
                    o = ret.Data;
                }
            }
            if (strname.ToLower().Trim() == "listdatalayountinfo")
            {
                var ret = _listdatalayountService.GetListdatalayountById(new ListdatalayountGetRequest
                {
                    Id = ID.ToString()
                });

                if (!ret.IsSuccess)
                {
                    throw new Exception(ret.Message);
                }
                else
                {
                    o = ret.Data;
                }
            }
            return o;
        }


        /// <summary>
        /// 获取列表目录
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetListDir()
        {
            string strCus = "";
            DataTable dt = this.GetDataTable(string.Format("select * from BFT_ListEx where blnEnable=1 and blnList=0  {0} order by ListID", strCus));
            return dt;
        }

        /// <summary>
        /// 获取子列表目录
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetChildListDir(int listId)
        {
            string strCus = "";
            DataTable dt = this.GetDataTable(string.Format("select * from BFT_ListEx where blnEnable=1 and DirID={1}  {0} order by ListID", strCus, listId));
            return dt;
        }

        /// <summary>
        /// 是否存在孩子目录列表
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public bool IsExistChildListDir(int listId)
        {
            string strCus = "";
            DataTable dt = this.GetDataTable(string.Format("select  ListID from BFT_ListEx where DirID={1}  {0} ", strCus, listId));
            return dt.Rows.Count >= 1;
        }


        /// <summary>
        /// 获取列表DTO
        /// </summary>
        /// <param name="listId">列表ID</param>
        /// <returns>ListEx</returns>
        public ListexInfo GetListEx(int listId)
        {
            var req = new ListexGetRequest()
            {
                Id = listId.ToString()
            };
            var res = _listexservice.GetListexById(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 获取列表命令
        /// </summary>
        /// <param name="listId">列表ID</param>
        /// <returns>ListCommandEx</returns>
        public DataTable GetListCommandExData(int listId)
        {
            return GetDataTable("select * from BFT_ListCommandEx where  ListID=" + listId);
        }

        /// <summary>
        /// 得到列表命令对象
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public IList<ListcommandexInfo> GetListCommandExDTOs(int ListID)
        {
            string sql = "select * from BFT_ListCommandEx where ListID=" + ListID + " and BlnVisible=1 " +
                         "  Order by LngRowIndex";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListcommandexInfo>(res.Data);
            return ret;
        }


        /// <summary>
        /// 得到列表方案
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public ListdataexInfo GetListDataExData(int ListDataID)
        {
            var res = _listdataexservice.GetListdataexById(new ListdataexGetRequest()
            {
                Id= ListDataID.ToString()
            });

            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            else
            {
                return res.Data;
            }
        }


        /// <summary>
        /// 得到报表模板文件
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public ListtempleInfo GetListTemple(int ListDataID)
        {
            var req = new IdRequest
            {
                Id = ListDataID
            };
            var res = _listtempleService.GetListtempleByListDataID(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 得到列表显示对象
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public IList<ListdisplayexInfo> GetListDisplayExData(int ListDataID)
        {
            string sql = "select * from BFT_ListDisplayEx where  ListDataID= " + ListDataID +
                         " order by LngRowIndex Asc";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdisplayexInfo>(res.Data);
            return ret;
        }

        /// <summary>
        /// 得到列表元数据对象
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public IList<ListmetadataInfo> GetListMetaDataData(int ListDataID)
        {
            string sql = "select * from BFT_ListMetaData where  ListDataID= " + ListDataID;
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListmetadataInfo>(res.Data);
            return ret;
        }


        /// <summary>
        /// 得到报表模板
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListtempleInfo> GetListTempleData(int ListDataID)
        {
            string sql = "select * from BFT_ListTemple where  ListDataID= " + ListDataID;
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListtempleInfo>(res.Data);
            return ret;
        }

        /// <summary>
        /// 得到测点编排
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListdatalayountInfo> GetListDataLayountData(int ListDataID)
        {
            string sql = "select * from BFT_ListDataLayount where  ListDataID= " + ListDataID;
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdatalayountInfo>(res.Data);
            return ret;
        }


        /// <summary>
        /// 得到编排设置
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListdatalayountInfo> GetListDataLayoutData(int ListDataID, string strDate)
        {
            string sql = "select * from BFT_ListDataLayount where  ListDataID= " + ListDataID + "  and  StrDate<='" + strDate + "'order by StrDate desc";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdatalayountInfo>(res.Data);
            return ret;
        }

        /// <summary>
        /// 得到编排设置
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListdatalayountInfo> GetListDataLayoutDataA(int ListDataID, string strDate)
        {
            string sql = "select * from BFT_ListDataLayount where  ListDataID= " + ListDataID + "  and  StrDate='" + strDate + "'order by StrDate desc";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdatalayountInfo>(res.Data);
            return ret;
        }

        /// <summary>
        /// 得到小于等于日期的第一个编排编排设置
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListdatalayountInfo> GetListDataLayoutByNearData(int ListDataID, string strDate)
        {
            string sql = "select * from BFT_ListDataLayount where  ListDataID= " + ListDataID + "  and  Convert(StrDate,date)=(SELECT strdate FROM `BFT_ListDataLayount` where ListDataID=" + ListDataID + " and CONVERT(strdate,date)<='" + strDate + "' order by StrDate desc limit 1) order by StrDate desc";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdatalayountInfo>(res.Data);
            return ret;
        }

        /// <summary>
        /// 得到编排设置
        /// </summary>
        /// <param name="ListDataID"></param>
        /// <returns></returns>
        public IList<ListdatalayountInfo> GetListDataLayoutData(int ListDataID)
        {
            string sql = "select * from BFT_ListDataLayount where  ListDataID= " + ListDataID + "  order by strDate Desc";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdatalayountInfo>(res.Data);
            return ret;
        }


        /// <summary>
        /// 列表保存
        /// </summary>
        /// <param name="listEx">列表对象</param>
        /// <param name="cmdList">列表按钮对象</param>
        /// <param name="blnSaveAs">是否为另存</param>
        /// <param name="blnSaveAsSchema">是否另存方案</param>
        public ListexInfo SaveList(ListexInfo listEx, IList<ListcommandexInfo> cmdList, bool blnSaveAs, bool blnSaveAsSchema)
        {
            var req = new SaveListRequest()
            {
                ListEx = listEx,
                CmdList = cmdList,
                BlnSaveAs = blnSaveAs,
                BlnSaveAsSchema = blnSaveAsSchema
            };
            var res = _listexservice.SaveList(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>   
        /// 列表保存
        /// </summary>
        /// <param name="listEx">列表对象</param>
        /// <param name="cmdList">列表按钮对象，新增列表才需传此参数，方案另存传空或者空列表</param>
        /// <param name="listDataEx">列表数据对象</param>
        /// <param name="lmdList">列表元数据对象</param>
        /// <param name="ldList">列表显示对象，当UserID等于零时才需传此参数，否则传空</param>
        /// <param name="lduList">列表显示对象，当UserID不等于零时才需传此参数，否则传空</param>
        /// <param name="lmdDt">列表方案数据</param>
        /// <param name="lngState">0 新增列表，1 修改方案 2另存方案（新增方案）</param>
        public ListdataexInfo SaveList(ListexInfo listEx, IList<ListcommandexInfo> cmdList, ListdataexInfo listDataEx,
            IList<ListmetadataInfo> lmdList, IList<ListdisplayexInfo> ldList, DataTable lmdDt, int lngState)
        {
            var req = new SaveList2Request()
            {
                ListEx = listEx,
                CmdList = cmdList,
                ListDataEx = listDataEx,
                ListmdList = lmdList,
                LdList = ldList,
                LmdDt = lmdDt,
                LngState = lngState
            };
            var res = _listexservice.SaveList(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 删除列表
        /// </summary>
        /// <param name="listId">列表ID</param>
        public void DeleteList(int listId)
        {
            var req = new IdRequest
            {
                Id = listId
            };
            var res = _listexservice.DeleteList(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 删除方案
        /// </summary>
        /// <param name="listId">列表ID</param>
        public void DeleteSchema(int listId, int listdataId)
        {
            var req = new DeleteSchemaRequest
            {
                ListID = listId,
                ListDataID = listdataId
            };
            var res = _listexservice.DeleteSchema(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 设置当前方案为默认方案
        /// </summary>
        /// <param name="listDataEx">列表数据</param>
        public void SetDefaultSchema(ListdataexInfo listDataEx)
        {
            var req = new SetDefaultSchemaRequest
            {
                ListDataEx = listDataEx
            };
            var res = _listexservice.SetDefaultSchema(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 列表方案保存
        /// </summary>
        /// <param name="listDataEx">列表方案对象</param>
        public void SaveListDataEx(ListdataexInfo listDataEx)
        {
            var req = new SaveListDataExRequest
            {
                ListDataEx = listDataEx
            };
            var res = _listexservice.SaveListDataEx(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 获取生成SQL
        /// </summary>
        /// <param name="lmdDt">列表方案数据</param>
        /// <returns>DataTable</returns>
        public string GetSQL(DataTable lmdDt)
        {
            var req = new GetSQLRequest
            {
                LmdDt = lmdDt
            };
            var res = _listexservice.GetSQL(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 测试SQL
        /// </summary>
        /// <param name="strSql">测试</param>
        public DataTable TestSql(string strSql)
        {
            strSql = RequestUtil.ProcessDynamicParameters(strSql);
            return this.GetDataTable(strSql);
        }


        /// <summary>
        /// 获取别名计数列表
        /// </summary>
        /// <param name="listDataId">列表数据ID</param>
        /// <returns>IDictionary</returns>
        public IDictionary<string, int> GetAliasNumDic(int listDataId)
        {
            IDictionary<string, int> aliasNumDic = new Dictionary<string, int>();
            string sql = @"select  distinct strFullPath as strKey,lngAliasCount 
                          from BFT_ListMetaData where ListDataID=" + listDataId;
            DataTable dt = this.GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    aliasNumDic.Add(TypeUtil.ToString(dt.Rows[i]["strKey"]), TypeUtil.ToInt(dt.Rows[i]["lngAliasCount"]));
                }
            }

            return aliasNumDic;
        }

        /// <summary>
        /// 获取列表元数据（通过列表数据ID）
        /// </summary>
        /// <param name="listDataId">列表数据ID</param>
        /// <returns>DataTable</returns>
        public DataTable GetListMetaData(int listDataId, int userId)
        {
            string sql = @"select BFT_ListMetaData.*,replace(replace(strConditionCHS,'&&$$',';'),'&&$',',') as strConditionShow,'' as strSrcFieldNameCHS,
                strListDisplayFieldNameCHS,lngSummaryType,blnSummary,lngDisplayWidth,lngHyperLinkType,strParaColName,strHyperlink,
                strSummaryDisplayFormat,strConditionFormat,replace(replace(strConditionFormat,'&&$$',';'),'&&$',',') as strConditionFormatShow,strBluerCondition,blnMerge,lngApplyType,lngFKType,blnMainMerge,blnKeyWord,lngKeyGroup,blnConstant,lngProivtType 
                from BFT_ListMetaData 
                left join BFT_ListDisplayEx on BFT_ListMetaData.MetaDataFieldName = BFT_ListDisplayEx.strListDisplayFieldName
                and BFT_ListMetaData.ListDataID=BFT_ListDisplayEx.ListDataID 
                where  BFT_ListMetaData.ListDataID = " + listDataId + "  order by BFT_ListMetaData.ID";//isnull(ListMetaData.blnSysProcess,0)=0 and

            DataTable dt = this.GetDataTable(sql);
            if (dt != null)
            {
                dt.Columns["lngDisplayWidth"].DefaultValue = 100;
                dt.Columns["lngOrder"].DefaultValue = 0;
                dt.Columns["lngFreConIndex"].DefaultValue = 0;
                dt.Columns["blnSummary"].DefaultValue = false;
                dt.Columns["blnFreCondition"].DefaultValue = false;
                dt.Columns["isCalcField"].DefaultValue = false;
                dt.Columns["blnReceivePara"].DefaultValue = false;
            }
            return dt;
        }

        /// <summary>
        /// 获取列表元数据关联列数据
        /// </summary>
        /// <param name="listDataId">列表数据ID</param>
        /// <returns>DataTable</returns>
        public DataTable GetLMDRelativeCol(int listDataId)
        {
            string sql = string.Format(@"select * from BFT_ListMetaData where ListDataID={0} and (lngRelativeFieldID >0 or MetaDataFieldID in (
                    select lngRelativeFieldID from BFT_ListMetaData where ListDataID={0} and lngRelativeFieldID >0))", listDataId);

            if (0 == listDataId)
            {   //列表数据ID为零，表示为新增列表或者方案

                sql = "select * from BFT_ListMetaData where 1=0";
            }

            DataTable dt = this.GetDataTable(sql);

            return dt;
        }


        /// <summary>
        /// 获取列表方案列表
        /// </summary>
        /// <param name="listId">列表ID</param>
        /// <returns>DataTable</returns>
        public DataTable GetSchemaList(int listId)
        {
            IList<string> list = new List<string>();
            string sql = "select ListDataID,strListDataName,(case blnDefault when 1 then '√' else '' end) as blnDefault  from BFT_ListDataEx where UserID in (0," + TypeUtil.ToInt(RequestUtil.GetParameterValue("userId")) + ") and ListID = " + listId + "  order by BlnDefault desc,UserID desc,ListDataID desc";
            DataTable dt = this.GetDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 根据列表ID
        /// </summary>
        /// <returns></returns>
        public IList<ListdataexInfo> GetListDataByListID(int ListID)
        {
            string sql = "select * from BFT_ListDataEx where  ListID=" + ListID;
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            var ret = ObjectConverter.Copy<ListdataexInfo>(res.Data);
            return ret;
        }


        /// <summary>
        /// 根据列表方案ID得到元数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetMetaDataFieldsByListDataID(int ListDataID)
        {
            string sql =
                "select ID,blnFieldRight from BFT_MetaDataFields where ID in (select MetaDataFieldID from BFT_ListMetaData where ListDataID=" +
                ListDataID + ")";
            var req = new SqlRequest()
            {
                Sql = sql
            };
            var res = _sqlService.QueryTableBySql(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }


        /// <summary>
        /// 获取记录总行数
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <returns>int</returns>
        public int GetTotalRecord(string strSql)
        {
            bool blnDistinct = strSql.Contains("distinct") ? true : false;

            strSql = RequestUtil.ProcessDynamicParameters(strSql);

            int index = strSql.IndexOf("FROM ", 0);
            if (index == -1) {
                index = strSql.IndexOf("from ", 0);
            }
            strSql = strSql.Substring(index);
            if (blnDistinct) strSql = strSql.Insert(15, " distinct ");
            if (strSql.Contains("order by"))
            {
                index = strSql.IndexOf("order by");
                strSql = strSql.Remove(index);
            }
            strSql = "Select COUNT(*) AS TotalRow " + strSql;
            if (strSql.Contains("group by"))
            {
                strSql = "Select COUNT(*)  As TotalRow from (" + strSql + ") as NewTable";
            }

            var req = new SqlRequest
            {
                Sql = strSql
            };
            var res = _listexservice.GetTotalRecord(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }


        /// <summary>
        /// 根据sql语句,直接获取数据表
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="pageNum">页数</param>
        /// <param name="perPageRecordNum">每页记录数</param>
        /// <returns>DataTable</returns>
        public DataTable GetPageData(string strSql, int pageNum, int perPageRecordNum)
        {
            strSql = RequestUtil.ProcessDynamicParameters(strSql);
            var req = new SqlRequest
            {
                Sql = strSql,
                PagerInfo = new PagerInfo()
                {
                    PageIndex = pageNum,
                    PageSize = perPageRecordNum
                }
            };
            var res = _listexservice.GetPageData(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 删除列表的记录
        /// </summary>
        /// <param name="RowIDs">所选择列的ID号，可能是一个多选的ID集合</param>
        /// <param name="entityName">实体名BrandID</param>

        /// <returns></returns>
        public bool DeteteRows(string strRowIDs, Type dtoType)
        {
            string[] strRows = strRowIDs.Split(',');
            foreach (string id in strRows)
            {
                object[] arg = new object[2] { "I" + dtoType.Name.Substring(0, dtoType.Name.Length - 3) + "Service", id };
                Type type = typeof(DeleteHelper<>);
                type = type.MakeGenericType(dtoType);
                object o = Activator.CreateInstance(type);
                type.InvokeMember("Delete", BindingFlags.Default | BindingFlags.InvokeMethod, null, o, arg);
            }

            return true;


        }


        /// <summary>
        /// 列表常用条件保存
        /// </summary>
        /// <param name="lmdList">列表元数据对象</param>
        public void SaveListMetaData(IList<ListmetadataInfo> lmdList)
        {
            //IListExService listexservice = ServiceFactory.CreateService<IListExService>();
            //listexservice.SaveListMetaData(lmdList);

        }

        /// <summary>
        /// 列表宽度保存
        /// </summary>
        /// <param name="lmdList">列表显示表</param>
        public void SaveListDisplayEx(IList<ListdisplayexInfo> ldlList)
        {
            var req = new SaveListDisplayExRequest
            {
                LdList = ldlList
            };
            var res = _listexservice.SaveListDisplayEx(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }


        /// <summary>
        /// 根据条件格式里设置的颜色存的字符串自动解析为颜色
        /// </summary>
        /// <param name="ss">传入的字符串参数,包括6个字段</param>
        /// <returns></returns>
        public void GetColorByString(string[] ss, ref Color color)
        {
            if (ss.Length >= 6)
            {
                if (ss[0] == "1")
                {
                    color = Color.FromArgb(TypeUtil.ToInt(ss[1]), TypeUtil.ToInt(ss[4]), TypeUtil.ToInt(ss[3]), TypeUtil.ToInt(ss[2]));
                }
                if (ss[0] == "2")
                {
                    color = Color.FromName(ss[5]);
                }
                if (ss[0] == "3")
                {
                    KnownColor knowColor = (KnownColor)Enum.Parse(typeof(KnownColor), ss[5], false);
                    color = Color.FromKnownColor(knowColor);
                }
            }
            else
            {
                //throw new Exception("颜色定义数组小于6位!不能生成Color对象！");
            }

        }

        /// <summary>
        /// 组织OrderBySQL(仅用于日表的时候用，因为日表要查询日期的时候动态组织sql,所以要在UI层组织，其他所有情况就是在服务层组织)
        /// </summary>
        public string AssemblyOrderBySql(DataTable _listMetaDataDt)
        {
            DataTable listMetaDataDt = _listMetaDataDt.Copy();
            string strSqlOrder = "";
            listMetaDataDt.DefaultView.RowFilter = "lngOrderMethod>0";
            listMetaDataDt.DefaultView.Sort = "lngOrder asc";
            DataTable dt = listMetaDataDt.DefaultView.ToTable();
            DataRow dr;
            int lngOrderMethod = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];

                //if (GroupFieldList.Count > 0 && !GroupFieldList.Contains(TypeUtil.ToString(dr["MetaDataFieldName"])))
                //{    //如果存在分组，且没有此分组字段，不能排序
                //    continue;
                //}

                lngOrderMethod = TypeUtil.ToInt(dr["lngOrderMethod"]);
                string strOrderByFileName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                int index = strOrderByFileName.LastIndexOf("_");
                if (index > -1)
                {
                    strOrderByFileName = strOrderByFileName.Remove(index, 1).Insert(index, ".");
                }


                if (lngOrderMethod == 1 || lngOrderMethod != 2)
                {    //升序

                    strSqlOrder += strOrderByFileName + " Asc ,";
                }
                else
                {    //降序
                    strSqlOrder += strOrderByFileName + " Desc ,";
                }
            }

            if (strSqlOrder != string.Empty)
            {
                strSqlOrder = "  order by " + strSqlOrder.Remove(strSqlOrder.Length - 1).Replace('.', '_');
            }

            return strSqlOrder;
        }


        /// <summary>
        /// 升级元数据
        /// </summary>
        /// <param name="o"></param>
        public void ImportMetadata(object o)
        {
            //IMetaDataService metadataservice = ServiceFactory.CreateService<IMetaDataService>();
            //metadataservice.ImportMetadata(o);
            var req = new ImportMetadataRequest
            {
                Obj = o
            };
            var res = _metadataService.ImportMetadata(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 升级报表
        /// </summary>
        /// <param name="o"></param>
        public void ImportReport(object o)
        {
            var req = new ObjectRequest
            {
                Obj = o
            };
            var res = _listexservice.ImportReport(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
        }

        /// <summary>
        /// 根据List组织List对象，用于导出 
        /// </summary>
        /// <param name="ListID"></param>
        /// <returns></returns>
        public ListexInfo OrgListExByExport(int ListID)
        {
            ListexInfo listex = this.GetListEx(ListID);
            List<ListdataexInfo> listsDataExDTO = this.GetListDataByListID(ListID) as List<ListdataexInfo>;
            foreach (ListdataexInfo listdatadto in listsDataExDTO)
            {
                List<ListmetadataInfo> listMetaDataDTO = this.GetListMetaDataData(listdatadto.ListDataID) as List<ListmetadataInfo>;
                listdatadto.ListMetaDataDTOList = listMetaDataDTO;
                List<ListdisplayexInfo> listsDisplayExDTO = this.GetListDisplayExData(listdatadto.ListDataID) as List<ListdisplayexInfo>;
                listdatadto.ListDisplayExDTOList = listsDisplayExDTO;
                List<ListtempleInfo> listsTempleDTO = this.GetListTempleData(listdatadto.ListDataID) as List<ListtempleInfo>;
                listdatadto.ListTempleDTOList = listsTempleDTO;
                List<ListdatalayountInfo> listDataLayoutDTO = this.GetListDataLayountData(listdatadto.ListDataID) as List<ListdatalayountInfo>;
                listdatadto.ListDataLayoutDTOList = listDataLayoutDTO;

            }
            listex.ListDataExDTOList = listsDataExDTO;
            List<ListcommandexInfo> listsCommandExDTO = this.GetListCommandExDTOs(ListID) as List<ListcommandexInfo>;
            listex.ListCommandExDTOList = listsCommandExDTO;
            return listex;
        }

        public DataTable GetFKLibDataTable(string strFKCode)
        {
            return this.GetDataTable("select * from BFT_FKLib where strFKCode='" + strFKCode + "'");
        }
        
        public bool blnExistsTable(string strTableName)
        {//暂时取消判断表是否存在，以免影响效率
            // return true;
            string strDBType = GetDbType();
            DataTable dt = null;
            string strsql = "";
            if (strDBType == "sqlserver")
            {
                strsql = "select object_id as oid,name from sys.objects where name='" + strTableName + "'";
            }
            if (strDBType == "mysql")
            {
                strsql = "select TABLE_CATALOG as oid ,table_name as name from information_schema.tables where table_name='" + strTableName + "'  and  table_schema='" + this.GetDBName() + "'";
            }
            dt = this.GetDataTable(strsql);
            if (dt == null || dt.Rows.Count == 0)
                return false;
            else
                return true;
        }

        public bool IfExistTables(List<string> tables)
        {            
            string sTables = "";
            foreach(var item in tables)
            {
                sTables += "'" + item + "',";
            }
            if (!string.IsNullOrEmpty(sTables))
            {
                sTables = sTables.Substring(0, sTables.Length - 1);
            }
            string sql = "select TABLE_CATALOG as oid ,table_name as name from information_schema.tables where table_name in(" + sTables + ") and table_schema='" + this.GetDBName() + "'";
            var dt = this.GetDataTable(sql);
            if (dt == null || dt.Rows.Count != tables.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 根据时间和listdataid获取备注信息
        /// </summary>
        /// <param name="time"></param>
        /// <param name="listDataId"></param>
        /// <returns></returns>
        public ListdataremarkInfo GetListDataRemarkByTimeListDataId(DateTime time, long listDataId)
        {
            var req = new GetListdataremarkByTimeListDataIdRequest
            {
                Time = time.ToString(),
                ListDataId = listDataId.ToString()
            };
            var res = _listdataremarkService.GetListdataremarkByTimeListDataId(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 新增备注
        /// </summary>
        /// <param name="listDataId"></param>
        /// <param name="time"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ListdataremarkInfo AddListdataremark(long listDataId, DateTime time, string remark)
        {
            var req = new ListdataremarkAddRequest
            {
                ListdataremarkInfo = new ListdataremarkInfo()
                {
                    Listdataid = listDataId.ToString(),
                    Time = time,
                    Remark = remark
                }
            };
            var res = _listdataremarkService.AddListdataremark(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="listDataId"></param>
        /// <param name="time"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ListdataremarkInfo UpdateListdataremarkByTimeListDataId(long listDataId, DateTime time, string remark)
        {
            var req = new ListdataremarkUpdateRequest
            {
                ListdataremarkInfo = new ListdataremarkInfo()
                {
                    Listdataid = listDataId.ToString(),
                    Time = time,
                    Remark = remark
                }
            };
            var res = _listdataremarkService.UpdateListdataremarkByTimeListDataId(req);
            if (!res.IsSuccess)
            {
                throw new Exception(res.Message);
            }
            return res.Data;
        }
    }

    public class DeleteHelper<T>
    {
        public static void Delete(string serviceName, object id)
        {
        }
    }


}
