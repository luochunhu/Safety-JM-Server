using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using Basic.Framework.Common;
using Basic.Framework.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.DataAccess;
using Sys.Safety.Request.Listcommandex;
using Sys.Safety.Request.Listdataex;
using Sys.Safety.Request.Listdatalayount;
using Sys.Safety.Request.Listdisplayex;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.Listmetadata;
using Sys.Safety.Request.Listtemple;
using Sys.Safety.ServiceContract;
using Sys.Safety.Reports;
using Sys.Safety.Reports.Service;

namespace Sys.Safety.Services
{
    public class ListexService : IListexService
    {
        private readonly IListcommandexRepository _listcommandexRepository;
        private readonly IListdataexRepository _listdataexRepository;
        //private readonly IListdatalayountRepository _listdatalayountRepository;
        private readonly IListdisplayexRepository _listdisplayexRepository;
        private readonly IListexRepository _listexRepository;
        private readonly RepositoryBase<ListexModel> _listexRepositoryBase;
        private readonly IListmetadataRepository _listmetadataRepository;
        //private readonly IListtempleRepository _listtempleRepository;
        private readonly IRequestRepository _requestRepository;


        private readonly BulidSQLUtil bulidSqlUtil = new BulidSQLUtil();
        private DataTable dt = null;
        private DataTable listMetaDataDt = null;
        private string strMasterCondition = "";
        private string strMasterTableName = "";

        //public ListexService(IListexRepository listexRepository, IListdataexRepository listdataexRepository,
        //    IListmetadataRepository listmetadataRepository, IListdisplayexRepository listdisplayexRepository,
        //    IListcommandexRepository listcommandexRepository, IListtempleRepository listtempleRepository,
        //    IListdatalayountRepository listdatalayountRepository, IRequestRepository requestRepository)
        public ListexService(IListexRepository listexRepository, IListdataexRepository listdataexRepository,
            IListmetadataRepository listmetadataRepository, IListdisplayexRepository listdisplayexRepository,
            IListcommandexRepository listcommandexRepository, IRequestRepository requestRepository)
        {
            _listexRepository = listexRepository;
            _listdataexRepository = listdataexRepository;
            _listmetadataRepository = listmetadataRepository;
            _listdisplayexRepository = listdisplayexRepository;
            _listcommandexRepository = listcommandexRepository;
            //_listtempleRepository = listtempleRepository;
            //_listdatalayountRepository = listdatalayountRepository;
            _requestRepository = requestRepository;
            _listexRepositoryBase = listexRepository as RepositoryBase<ListexModel>;
        }

        //public ListexService(IListexRepository listexRepository)
        //{
        //    _listexRepository = listexRepository;
        //    _listdataexRepository = ServiceFactory.Create<IListdataexRepository>();
        //    _listmetadataRepository = ServiceFactory.Create<ListmetadataRepository>();
        //    _listdisplayexRepository = ServiceFactory.Create<IListdisplayexRepository>();
        //    _listcommandexRepository = ServiceFactory.Create<IListcommandexRepository>();
        //    _listtempleRepository = ServiceFactory.Create<IListtempleRepository>();
        //    _listdatalayountRepository = ServiceFactory.Create<IListdatalayountRepository>();
        //    _requestRepository = ServiceFactory.Create<IRequestRepository>();
        //    _listexRepositoryBase = listexRepository as RepositoryBase<ListexModel>;
        //}

        public BasicResponse<ListexInfo> AddListex(ListexAddRequest listexrequest)
        {
            var _listex = ObjectConverter.Copy<ListexInfo, ListexModel>(listexrequest.ListexInfo);
            var resultlistex = _listexRepository.AddListex(_listex);
            var listexresponse = new BasicResponse<ListexInfo>();
            listexresponse.Data = ObjectConverter.Copy<ListexModel, ListexInfo>(resultlistex);
            return listexresponse;
        }

        public BasicResponse<ListexInfo> UpdateListex(ListexUpdateRequest listexrequest)
        {
            var _listex = ObjectConverter.Copy<ListexInfo, ListexModel>(listexrequest.ListexInfo);
            _listexRepository.UpdateListex(_listex);
            var listexresponse = new BasicResponse<ListexInfo>();
            listexresponse.Data = ObjectConverter.Copy<ListexModel, ListexInfo>(_listex);
            return listexresponse;
        }

        public BasicResponse DeleteListex(ListexDeleteRequest listexrequest)
        {
            _listexRepository.DeleteListex(listexrequest.Id);
            var listexresponse = new BasicResponse();
            return listexresponse;
        }

        public BasicResponse<List<ListexInfo>> GetListexList(ListexGetListRequest listexrequest)
        {
            var listexresponse = new BasicResponse<List<ListexInfo>>();
            listexrequest.PagerInfo.PageIndex = listexrequest.PagerInfo.PageIndex - 1;
            if (listexrequest.PagerInfo.PageIndex < 0)
                listexrequest.PagerInfo.PageIndex = 0;
            var rowcount = 0;
            var listexModelLists = _listexRepository.GetListexList(listexrequest.PagerInfo.PageIndex,
                listexrequest.PagerInfo.PageSize, out rowcount);
            var listexInfoLists = new List<ListexInfo>();
            foreach (var item in listexModelLists)
            {
                var ListexInfo = ObjectConverter.Copy<ListexModel, ListexInfo>(item);
                listexInfoLists.Add(ListexInfo);
            }
            listexresponse.Data = listexInfoLists;
            return listexresponse;
        }

        public BasicResponse<ListexInfo> GetListexById(ListexGetRequest listexrequest)
        {
            var result = _listexRepository.GetListexById(listexrequest.Id);
            var listexInfo = ObjectConverter.Copy<ListexModel, ListexInfo>(result);
            var listexresponse = new BasicResponse<ListexInfo>();
            listexresponse.Data = listexInfo;
            return listexresponse;
        }

        public BasicResponse<DataTable> GetServerMetaData()
        {
            var ret = new BasicResponse<DataTable>
            {
                Data = ServerDataCache.GetMetaData()
            };
            return ret;
        }

        public BasicResponse<DataTable> GetServerMetaDataFields()
        {
            var ret = new BasicResponse<DataTable>
            {
                Data = ServerDataCache.GetMetaDataFields()
            };
            return ret;
        }

        public BasicResponse<DataTable> GetServerMetaData(IdRequest ID)
        {
            var ret = new BasicResponse<DataTable>
            {
                Data = ServerDataCache.GetMetaData(ID.Id)
            };
            return ret;
        }

        public BasicResponse<DataTable> GetFieldRights(IdRequest UserID)
        {
            var ret = new BasicResponse<DataTable>
            {
                Data = ServerDataCache.InitFieldRights(UserID.Id)
            };
            return ret;
        }

        public BasicResponse UpdateMetaDataCache()
        {
            ServerDataCache.UpdateMetaDataCache();
            return new BasicResponse();
        }

        public BasicResponse<DataTable> GetServerRequest()
        {
            var ret = new BasicResponse<DataTable>
            {
                Data = ServerDataCache.GetRequest()
            };
            return ret;
        }

        /// <summary>
        ///     列表保存
        /// </summary>
        /// <param name="listEx">列表对象</param>
        /// <param name="cmdList">列表按钮对象</param>
        /// <param name="blnSaveAs">是否为另存</param>
        /// <param name="blnSaveAsSchema">是否另存方案</param>
        public BasicResponse<ListexInfo> SaveList(SaveListRequest request)
        {
            BasicResponse<ListexInfo> mr = new BasicResponse<ListexInfo>()
            {
                Code = -100,
                Message = "操作失败。"
            };

            TransactionsManager.BeginTransaction(() =>
            {
                if (!TypeUtil.IsValidCode(request.ListEx.StrListCode))
                    throw new Exception("编码只能为字母，数字和下划线！");

                //编码重复性检查
                //var strSql = "select ListID from BFT_ListEx where strListCode='" + listEx.StrListCode + "'";
                //if (listEx.DTOState != Framework.Core.Service.DTO.DTOStateEnum.AddNew)
                //    strSql = "select ListID from BFT_ListEx where strListCode='" + listEx.StrListCode +
                //             "' and ListID <>" + listEx.ListID;
                //DataTable dt = this.GetDataTableBySQL(strSql);
                DataTable dt;
                if (request.ListEx.InfoState != InfoState.AddNew)
                    dt = _listexRepository.QueryTable("global_ListExService_GetListExListID_ByStrListCodeListId",
                        request.ListEx.StrListCode, request.ListEx.ListID);
                else
                    dt = _listexRepository.QueryTable("global_ListExService_GetListExListID_ByStrListCode",
                        request.ListEx.StrListCode);

                if ((dt != null) && (dt.Rows.Count > 0))
                    throw new Exception("列表编码[" + request.ListEx.StrListCode + "]已经存在！");

                var ListID = request.ListEx.ListID;
                if (request.ListEx.InfoState == InfoState.AddNew)
                {
                    request.ListEx.StrGuidCode = Guid.NewGuid().ToString();
                    //ListID = TypeUtil.ToInt(Insert(listEx));
                    var _class = ObjectConverter.Copy<ListexInfo, ListexModel>(request.ListEx);
                    ListID = Convert.ToInt32(_listexRepository.AddListex(_class).ListID);

                    //long RequestID = Framework.Utils.Date.DateTimeUtil.GetDateTimeNowToInt64();
                    var RequestID = IdHelper.CreateLongId();
                    var strRequestCode = "Request" + request.ListEx.StrListCode;
                    var strRequedstName = request.ListEx.StrListName;
                    var MenuURL = "frmList";
                    var MenuFile = "Sys.Safety.Reports.dll";
                    var MenuNamespace = "Sys.Safety.Reports";
                    var MenuParams = "ListID=" + ListID;
                    var showType = 0;
                    try
                    {
                        //var strsql =
                        //    string.Format(
                        //        "insert BFT_Request(RequestID,RequestCode,RequestName,MenuURL,MenuFile,MenuNamespace,MenuParams,ShowType) values(" +
                        //        RequestID + ",'" + strRequestCode + "','" + strRequedstName + "','" + MenuURL + "','" +
                        //        MenuFile + "','" + MenuNamespace + "','" + MenuParams + "'," + showType + ")");
                        //this.DAOManager.ExecuteSQL(strsql);
                        var rm = new RequestModel
                        {
                            RequestID = RequestID.ToString(),
                            RequestCode = strRequestCode,
                            RequestName = strRequedstName,
                            MenuURL = MenuURL,
                            MenuFile = MenuFile,
                            MenuNamespace = MenuNamespace,
                            MenuParams = MenuParams,
                            ShowType = showType
                        };
                        _requestRepository.AddRequest(rm);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("加入请求库表失败，原因为" + ex.Message + "列表ID" + ListID);
                    }
                }
                else
                {
                    //this.SaveDTO(listEx);
                    var request2 = new SaveListExInfoRequest()
                    {
                        Info = request.ListEx
                    };
                    SaveListExInfo(request2);
                }

                //IListCommandExService listcommandService = ServiceFactory.CreateService<IListCommandExService>();

                //保存列表按钮对象 
                foreach (ListcommandexInfo cmd in request.CmdList)
                {
                    cmd.ListID = ListID;

                    if (request.BlnSaveAs)
                        cmd.InfoState = InfoState.AddNew;

                    var service = ServiceFactory.Create<IListcommandexService>();
                    var request2 = new SaveListCommandInfoRequest()
                    {
                        Info = cmd
                    };
                    service.SaveListCommandInfo(request2);
                }

                //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
                //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
                //IListDisplayExService listdisplayservice = ServiceFactory.CreateService<IListDisplayExService>();

                if (request.BlnSaveAs && request.BlnSaveAsSchema)
                {
                    //列表为另存              

                    //IList<ListdataexModel> listDataExList =
                    //    listdataService.GetList("from ListDataExEntity where UserID=0 and ListID=" + listEx.ListID +
                    //                            " order by BlnDefault desc");
                    IList<ListdataexModel> listDataExList =
                        _listdataexRepository.GetListdataexListByListId(request.ListEx.ListID);
                    ListdataexModel listDataEx = null;
                    for (var i = 0; i < listDataExList.Count; i++)
                    {
                        if (i > 0)
                            break;
                        listDataEx = listDataExList[i];
                        //IList<ListmetadataModel> lmdList =
                        //    listmetadataservice.GetList("from ListMetaDataEntity where ListDataID=" +
                        //                                listDataEx.ListDataID);
                        IList<ListmetadataModel> lmdList =
                            _listmetadataRepository.GetListmetadataListByListDataId(
                                Convert.ToInt32(listDataEx.ListDataID));

                        //IList<ListdisplayexModel> ldList =
                        //    listdisplayservice.GetList("from ListDisplayExEntity where ListDataID=" +
                        //                               listDataEx.ListDataID);
                        IList<ListdisplayexModel> ldList =
                            _listdisplayexRepository.GetListdisplayexListByListDataId(
                                Convert.ToInt32(listDataEx.ListDataID));

                        listDataEx.ListID = ListID;
                        //listDataEx.DTOState = Framework.Core.Service.DTO.DTOStateEnum.AddNew;
                        //var newlistdataID = TypeUtil.ToInt(listdataService.Insert(listDataEx));
                        var newlistdataID = _listdataexRepository.AddListdataex(listDataEx).ListDataID;

                        //保存列表元数据对象
                        if (lmdList != null)
                            foreach (ListmetadataModel lmd in lmdList)
                            {
                                lmd.ListDataID = Convert.ToInt32(newlistdataID);
                                //lmd.DTOState = Framework.Core.Service.DTO.DTOStateEnum.AddNew;
                                //listmetadataservice.SaveDTO(lmd);
                                _listmetadataRepository.AddListmetadata(lmd);
                            }

                        //保存列表显示对象
                        if (ldList != null)
                            foreach (ListdisplayexModel ld in ldList)
                            {
                                ld.ListDataID = Convert.ToInt32(newlistdataID);
                                //ld.DTOState = Framework.Core.Service.DTO.DTOStateEnum.AddNew;
                                //listdisplayservice.SaveDTO(ld);
                                _listdisplayexRepository.AddListdisplayex(ld);
                            }
                    }
                }

                //listexDTO = this.GetById(ListID);
                var model = _listexRepository.GetListexById(ListID.ToString());
                mr.Code = 100;
                mr.Message = "操作成功。";
                mr.Data = ObjectConverter.Copy<ListexModel, ListexInfo>(model);
            });

            return mr;
        }

        /// <summary>
        ///     删除列表
        /// </summary>
        /// <param name="listId">列表ID</param>
        public BasicResponse DeleteList(IdRequest request)
        {
            //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
            //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
            //IListDisplayExService listdisplayservice = ServiceFactory.CreateService<IListDisplayExService>();
            //IListCommandExService listcommandService = ServiceFactory.CreateService<IListCommandExService>();
            var ret = new BasicResponse()
            {
                Code = -100,
                Message = "操作失败。"
            };

            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    //删除列表、按钮对象
                    //this.Delete("from ListExEntity where ListID=" + listId);
                    _listexRepository.DeleteListex(request.Id.ToString());
                    //listcommandService.Delete("from ListCommandExEntity where ListID=" + listId);
                    _listcommandexRepository.DeleteListcommandex(request.Id.ToString());
                    //this.DAOManager.ExecuteSQL("delete from  BFT_Request where MenuURL='frmList' and MenuParams= 'ListID=" +
                    //                           listId + "'");
                    _requestRepository.DeleteRequestByMenuUrlMenuParams("frmList", "ListID=" + request.Id);

                    //DataTable listDataIds =
                    //    this.GetDataTableBySQL("select listdataId from BFT_ListDataEx where listId=" + listId);
                    var model = _listdataexRepository.GetListdataexListByListId(request.Id);
                    DataTable listDataIds = ObjectConverter.ToDataTable(model);
                    if ((listDataIds != null) && (listDataIds.Rows.Count > 0))
                    {
                        //删除列表方案对象
                        //listdataService.Delete("from ListDataExEntity where ListID=" + listId);
                        _listdataexRepository.DeleteListdataexByListId(request.Id);

                        //删除列表元数据对象、列表显示对象、列表用户显示对象
                        var listDataId = 0;
                        foreach (DataRow dr in listDataIds.Rows)
                        {
                            listDataId = TypeUtil.ToInt(dr["listdataId"]);
                            //listmetadataservice.Delete("from ListMetaDataEntity where ListDataID=" + listDataId);
                            _listmetadataRepository.DeleteListmetadataByListDataID(listDataId.ToString());
                            //listdisplayservice.Delete("from ListDisplayExEntity where ListDataID=" + listDataId);
                            _listdisplayexRepository.DeleteListdisplayexByListDataID(listDataId.ToString());
                        }
                    }
                });

                ret.Code = 100;
                ret.Message = "操作成功。";
            }
            catch (Exception ex)
            {
                ThrowException("删除列表", ex);
            }

            return ret;
        }


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
        public BasicResponse<ListdataexInfo> SaveList(SaveList2Request request)
        {
            var ret = new BasicResponse<ListdataexInfo>
            {
                Code = -100,
                Message = "操作失败。"
            };

            TransactionsManager.BeginTransaction(() =>
            {
                //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
                //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
                //IListDisplayExService listdisplayservice = ServiceFactory.CreateService<IListDisplayExService>();
                //IListCommandExService listcommandService = ServiceFactory.CreateService<IListCommandExService>();
                //ListdataexInfo listdatadto = null;
                var ListID = 0;

                //保存列表、按钮对象
                if (0 == request.LngState)
                {
                    //新增列表                    

                    ListID = request.ListEx.ListID;
                    if (request.ListEx.InfoState == InfoState.AddNew)
                    {
                        //ListID = Convert.ToInt32(this.Insert(listEx));
                        var model = ObjectConverter.Copy<ListexInfo, ListexModel>(request.ListEx);
                        ListID = Convert.ToInt32(_listexRepository.AddListex(model).ListID);
                    }
                    else
                    {
                        var request3 = new SaveListExInfoRequest()
                        {
                            Info = request.ListEx
                        };
                        SaveListExInfo(request3);

                    }

                    foreach (ListcommandexInfo cmd in request.CmdList)
                    {
                        cmd.ListID = ListID;
                        var service = ServiceFactory.Create<IListcommandexService>();
                        var request2 = new SaveListCommandInfoRequest()
                        {
                            Info = cmd
                        };
                        service.SaveListCommandInfo(request2);
                    }
                }

                //保存列表、数据对象
                // 20170605
                //if (request.ListEx != null) request.ListDataEx.ListID = ListID;
                var strListSQL = bulidSqlUtil.GetSQL(request.LmdDt);
                if ((request.ListDataEx.StrConTextCondition != null) && (request.ListDataEx.StrConTextCondition != string.Empty))
                {
                    string[] strs = request.ListDataEx.StrConTextCondition.Split(new[] { "&&&$$$" },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 2)
                        strListSQL = strListSQL.Replace("where 1=1",
                            "where 1=1 " + " and " + strs[1].Trim().Replace('_', '.'));
                }
                request.ListDataEx.StrListSQL = strListSQL;

                int ListDataID = request.ListDataEx.ListDataID;
                if (request.ListDataEx.InfoState == InfoState.AddNew)
                {
                    //ListDataID = TypeUtil.ToInt(listdataService.Insert(listDataEx));
                    var model = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(request.ListDataEx);
                    ListDataID = Convert.ToInt32(_listdataexRepository.AddListdataex(model).ListDataID);
                }
                else
                {
                    var service = ServiceFactory.Create<IListdataexService>();
                    var request2 = new SaveListDataExInfoRequest()
                    {
                        Info = request.ListDataEx
                    };
                    service.SaveListDataExInfo(request2);
                }


                var sw = new Stopwatch();

                //保存列表元数据对象
                if (request.ListmdList != null)
                {
                    if (1 == request.LngState)
                        //listmetadataservice.Delete("from ListMetaDataEntity where ListDataID=" + ListDataID);
                        _listmetadataRepository.DeleteListmetadataByListDataID(ListDataID.ToString());

                    sw.Start();
                    foreach (ListmetadataInfo lmd in request.ListmdList)
                    {
                        lmd.ListDataID = ListDataID;
                        //listmetadataservice.SaveDTO(lmd);
                        var service = ServiceFactory.Create<IListmetadataService>();
                        var request2 = new SaveListMetaDataExInfoRequest()
                        {
                            Info = lmd
                        };
                        service.SaveListMetaDataExInfo(request2);
                    }

                    sw.Stop();

                    Console.WriteLine(sw.ElapsedMilliseconds);
                }

                //保存列表显示对象
                if (request.LdList != null)
                {
                    if (1 == request.LngState)
                        //listdisplayservice.Delete("from ListDisplayExEntity where ListDataID=" + ListDataID);
                        _listdisplayexRepository.DeleteListdisplayexByListDataID(ListDataID.ToString());

                    sw.Restart();

                    foreach (ListdisplayexInfo ld in request.LdList)
                    {
                        ld.ListDataID = ListDataID;
                        //listdisplayservice.SaveDTO(ld);
                        var service = ServiceFactory.Create<IListdisplayexService>();
                        var request2 = new SaveListDisplayExInfoRequest()
                        {
                            Info = ld
                        };
                        service.SaveListDisplayExInfo(request2);
                    }

                    sw.Stop();

                    Console.WriteLine(sw.ElapsedMilliseconds);
                }

                //listdatadto = listdataService.GetById(ListDataID);
                var model2 = _listdataexRepository.GetListdataexById(ListDataID.ToString());
                ret.Code = 100;
                ret.Message = "操作成功。";
                ret.Data = ObjectConverter.Copy<ListdataexModel, ListdataexInfo>(model2);


            });

            return ret;
        }

        /// <summary>
        ///     列表常用条件保存
        /// </summary>
        /// <param name="lmdList">列表元数据对象</param>
        public BasicResponse SaveListMetaData(SaveListMetaDataRequest request)
        {
            //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    if (request.LmdList != null)
                    {
                        var service = ServiceFactory.Create<IListmetadataService>();
                        foreach (ListmetadataInfo lmd in request.LmdList)
                        {
                            //listmetadataservice.SaveDTO(lmd);
                            var request2 = new SaveListMetaDataExInfoRequest()
                            {
                                Info = lmd
                            };
                            service.SaveListMetaDataExInfo(request2);
                        }
                            
                    }
                });
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("保存常用查询条件", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        /// <summary>
        ///     列表宽度保存
        /// </summary>
        /// <param name="lmdList">列表显示表</param>
        public BasicResponse SaveListDisplayEx(SaveListDisplayExRequest request)
        {
            //IListDisplayExService listmetadataservice = ServiceFactory.CreateService<IListDisplayExService>();
            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    if (request.LdList != null)
                    {
                        var service = ServiceFactory.Create<IListdisplayexService>();
                        foreach (ListdisplayexInfo ldl in request.LdList)
                        {
                            //listmetadataservice.SaveDTO(ldl);
                            var request2 = new SaveListDisplayExInfoRequest()
                            {
                                Info = ldl
                            };
                            service.SaveListDisplayExInfo(request2);
                        }
                            
                    }
                });
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("保存列表列宽度", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        /// <summary>
        ///     删除方案
        /// </summary>
        /// <param name="listID">列表ID</param>
        /// <param name="listDataID">列表数据ID（方案ID）</param>
        public BasicResponse DeleteSchema(DeleteSchemaRequest request)
        {
            //IListExService listexService = ServiceFactory.CreateService<IListExService>();
            //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
            //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
            //IListDisplayExService listdisplayservice = ServiceFactory.CreateService<IListDisplayExService>();
            //IListCommandExService listcommandService = ServiceFactory.CreateService<IListCommandExService>();

            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    //删除列表方案对象
                    //listdataService.Delete("from ListDataExEntity where ListID=" + listID + " and ListDataID=" + listDataID);
                    _listdataexRepository.DeleteListdataexByListIdListDataId(request.ListID.ToString(), request.ListDataID.ToString());
                    //listmetadataservice.Delete("from ListMetaDataEntity where ListDataID=" + listDataID);
                    _listmetadataRepository.DeleteListmetadataByListDataID(request.ListDataID.ToString());
                    //listdisplayservice.Delete("from ListDisplayExEntity  where ListDataID=" + listDataID);
                    _listdisplayexRepository.DeleteListdisplayexByListDataID(request.ListDataID.ToString());
                });
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("删除方案", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        /// <summary>
        ///     设置当前方案为默认方案
        /// </summary>
        /// <param name="listDataEx">列表数据</param>
        public BasicResponse SetDefaultSchema(SetDefaultSchemaRequest request)
        {
            //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    //IList<ListdataexModel> list =
                    //    listdataService.GetListDataExEntity("from ListDataExEntity where ListID=" + listDataEx.ListID +
                    //                                        " and UserID=" + listDataEx.UserID + " and ListDataID<>" +
                    //                                        listDataEx.ListDataID);
                    IList<ListdataexModel> list = _listdataexRepository.GetListdataexListByThreeId(request.ListDataEx.ListID.ToString(), request.ListDataEx.UserID.ToString(), request.ListDataEx.ListDataID.ToString());

                    foreach (ListdataexModel item in list)
                    {
                        item.BlnDefault = false;
                        //listdataService.SaveListDataExentity(item);
                        _listdataexRepository.UpdateListdataex(item);
                    }

                    //设置当前方案为默认方案
                    request.ListDataEx.BlnDefault = true;
                    request.ListDataEx.InfoState = InfoState.Modified;
                    //listdataService.SaveDTO(listDataEx);
                    var service = ServiceFactory.Create<IListdataexService>();
                    var request2 = new SaveListDataExInfoRequest()
                    {
                        Info = request.ListDataEx
                    };
                    service.SaveListDataExInfo(request2);
                });
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("设为默认方案", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        /// <summary>
        ///     列表方案保存
        /// </summary>
        /// <param name="listDataEx">列表方案对象</param>
        public BasicResponse SaveListDataEx(SaveListDataExRequest request)
        {
            //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();

            try
            {
                request.ListDataEx.InfoState = InfoState.Modified;
                //listdataService.SaveDTO(listDataEx);
                var service = ServiceFactory.Create<IListdataexService>();
                var request2 = new SaveListDataExInfoRequest()
                {
                    Info = request.ListDataEx
                };
                service.SaveListDataExInfo(request2);
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("列表方案另存", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作成功。"
                };
            }
        }

        /// <summary>
        ///     拼装sql语句
        /// </summary>
        /// <param name="lmdDt"></param>
        /// <returns></returns>
        public BasicResponse<string> GetSQL(GetSQLRequest request)
        {
            try
            {
                string strSql = bulidSqlUtil.GetSQL(request.LmdDt);
                return new BasicResponse<string>()
                {
                    Data = strSql
                };
            }
            catch (Exception ex)
            {
                ThrowException("拼装sql语句", ex);
                return new BasicResponse<string>()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }

        }

        /// <summary>
        ///     获取记录总行数
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <returns>int</returns>
        public BasicResponse<int> GetTotalRecord(SqlRequest request)
        {
            var type = Basic.Framework.Configuration.Global.DatabaseType.ToString().ToLower();
            if (type == "mysql")
                request.Sql = request.Sql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");
            //int rowCount = base.GetTotalRecord(strSql); //暂时不调用底层方法

            var rowCount = 0;
            DataTable dt = _listexRepositoryBase.QueryTableBySql(request.Sql);
            if ((dt != null) && (dt.Rows.Count > 0))
                rowCount = TypeUtil.ToInt(dt.Rows[0][0].ToString());
            var ret = new BasicResponse<int>()
            {
                Data = rowCount
            };
            return ret;
        }

        /// <summary>
        ///     列表分页查询方法
        /// </summary>
        /// <param name="strSql">SQL</param>
        /// <param name="pageNum">页数</param>
        /// <param name="perPageRecordNum">每页记录数</param>
        /// <returns>DataTable</returns>
        public BasicResponse<DataTable> GetPageData(SqlRequest request)
        {
            if (GetDBType().Data == "mysql")
                request.Sql = request.Sql.Replace("ISNULL", "IFNULL").Replace("isnull", "ifnull");
            //return this.GetPageDataTableBySQL(strSql, pageNum, perPageRecordNum);
            var dt =_listexRepositoryBase.QueryTableBySql(request.Sql, request.PagerInfo.PageIndex, request.PagerInfo.PageSize);
            var ret = new BasicResponse<DataTable>()
            {
                Data = dt
            };
            return ret;        //todo

            //int startIndex = perPageRecordNum * (pageNum - 1);
            //string strsql = GetLimitString(strSql, startIndex, startIndex + perPageRecordNum);
            //return this.GetDataTableBySQL(strsql);
        }

        /// <summary>
        ///     得到查询的sql语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="offset"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public string GetLimitString(string strSql, int offset, int last)
        {
            var strReturnSql = "";
            if (GetDBType().Data == "sqlserver")
            {
                var pagingBuilder = new StringBuilder();
                var orderByStringBuilder = new StringBuilder();
                var distinctStr = string.Empty;

                var loweredString = strSql.ToLower();
                var orderByIndex = loweredString.IndexOf("order by");
                if (orderByIndex != -1)
                {
                    orderByStringBuilder = new StringBuilder();
                    orderByStringBuilder.Append(strSql.Substring(orderByIndex));
                }
                if (loweredString.TrimStart().StartsWith("select"))
                {
                    var index = 6;
                    if (loweredString.StartsWith("select distinct"))
                    {
                        distinctStr = "DISTINCT ";
                        index = 15;
                    }
                    strSql = strSql.Substring(index);
                }

                pagingBuilder.Append(strSql);


                var orderby = orderByStringBuilder.ToString();
                // if no ORDER BY is specified use fake ORDER BY field to avoid errors 
                if ((orderby == null) || (orderby.Length == 0))
                    orderby = "ORDER BY CURRENT_TIMESTAMP";

                var beginning =
                    string.Format("WITH query AS (SELECT {0}TOP {1} ROW_NUMBER() OVER ({2}) as __hibernate_row_nr__, ",
                        distinctStr, last, orderby);
                var ending =
                    string.Format(
                        ") SELECT * FROM query WHERE __hibernate_row_nr__ > {0} ORDER BY __hibernate_row_nr__",
                        offset);

                pagingBuilder.Insert(0, beginning);
                pagingBuilder.Append(ending);
                strReturnSql = pagingBuilder.ToString();
            }
            if (GetDBType().Data == "mysql")
                strReturnSql = string.Format(" {0}  LIMIT {1}, {2} ", strSql, offset, last);
            return strReturnSql;
        }

        /// <summary>
        ///     根据选择的xml文件序列化后的listdto导入到客户电脑(方案确定只考虑新增情况)
        /// </summary>
        /// <param name="o"></param>
        public BasicResponse ImportReport(ObjectRequest o)
        {
            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    //IListDataExService listdataService = ServiceFactory.CreateService<IListDataExService>();
                    //IListMetaDataService listmetadataservice = ServiceFactory.CreateService<IListMetaDataService>();
                    //IListDisplayExService listdisplayservice = ServiceFactory.CreateService<IListDisplayExService>();
                    //IListCommandExService listcommandService = ServiceFactory.CreateService<IListCommandExService>();
                    //IListTempleService listtempService = ServiceFactory.CreateService<IListTempleService>();
                    //IListDataLayoutService listdatalayoutService = ServiceFactory.CreateService<IListDataLayoutService>();
                    var listcommandexService = ServiceFactory.Create<IListcommandexService>();
                    var listmetadataService = ServiceFactory.Create<IListmetadataService>();
                    var listdisplayexService = ServiceFactory.Create<IListdisplayexService>();
                    var listtempleService = ServiceFactory.Create<IListtempleService>();
                    var listdatalayountService = ServiceFactory.Create<IListdatalayountService>();

                    var ListID = 0;
                    ListexInfo listex = o.Obj as ListexInfo;
                    listex.StrGuidCode = Guid.NewGuid().ToString();
                    //IList<ListexInfo> listsEx =
                    //    this.GetList("from  ListExEntity where StrGuidCode='" + listex.StrGuidCode + "'");
                    var model = _listexRepository.GetListexByStrGuidCode(listex.StrGuidCode);
                    IList<ListexInfo> listsEx = ObjectConverter.CopyList<ListexModel, ListexInfo>(model);

                    if (listsEx.Count > 0)
                        throw new Exception("当前系统已存在此报表,请确认是不是已经导入过！");

                    listex.InfoState = InfoState.AddNew;
                    //ListID = TypeUtil.ToInt(this.Insert(listex));
                    var model2 = ObjectConverter.Copy<ListexInfo, ListexModel>(listex);
                    ListID = Convert.ToInt32(_listexRepository.AddListex(model2).ListID);

                    foreach (ListcommandexInfo command in listex.ListCommandExDTOList)
                    {
                        command.InfoState = InfoState.AddNew;
                        command.ListID = ListID;
                        var request2 = new SaveListCommandInfoRequest()
                        {
                            Info = command
                        };
                        listcommandexService.SaveListCommandInfo(request2);
                    }
                    foreach (ListdataexInfo listdata in listex.ListDataExDTOList)
                    {
                        listdata.InfoState = InfoState.AddNew;
                        listdata.ListID = ListID;
                        //var ListDataID = TypeUtil.ToInt(listdataService.Insert(listdata));
                        var model3 = ObjectConverter.Copy<ListdataexInfo, ListdataexModel>(listdata);
                        var ListDataID = Convert.ToInt32(_listdataexRepository.AddListdataex(model3).ListDataID);

                        foreach (ListmetadataInfo listmetadata in listdata.ListMetaDataDTOList)
                        {
                            listmetadata.InfoState = InfoState.AddNew;
                            listmetadata.ListDataID = ListDataID;
                            //listmetadataservice.SaveDTO(listmetadata);
                            var request2 = new SaveListMetaDataExInfoRequest()
                            {
                                Info = listmetadata
                            };
                            listmetadataService.SaveListMetaDataExInfo(request2);
                        }
                        foreach (ListdisplayexInfo listdisplay in listdata.ListDisplayExDTOList)
                        {
                            listdisplay.InfoState = InfoState.AddNew;
                            listdisplay.ListDataID = ListDataID;
                            //listdisplayservice.SaveDTO(listdisplay);
                            var request2 = new SaveListDisplayExInfoRequest()
                            {
                                Info = listdisplay
                            };
                            listdisplayexService.SaveListDisplayExInfo(request2);
                        }
                        foreach (ListtempleInfo listtemple in listdata.ListTempleDTOList)
                        {
                            listtemple.InfoState = InfoState.AddNew;
                            listtemple.ListDataID = ListDataID;
                            //listtempService.SaveDTO(listtemple);
                            var request2 = new SaveListTempleInfoRequest()
                            {
                                Info = listtemple
                            };
                            listtempleService.SaveListTempleInfo(request2);
                        }
                        foreach (ListdatalayountInfo listdatalayout in listdata.ListDataLayoutDTOList)
                        {
                            listdatalayout.InfoState = InfoState.AddNew;
                            listdatalayout.ListDataID = ListDataID;
                            //listdatalayoutService.SaveDTO(listdatalayout);
                            var request2 = new SaveListDataLayountInfoRequest()
                            {
                                Info = listdatalayout
                            };
                            listdatalayountService.SaveListDataLayountInfo(request2);
                        }
                    }
                });
                return new BasicResponse();
            }
            catch (Exception ex)
            {
                ThrowException("导入报表", ex);
                return new BasicResponse()
                {
                    Code = -100,
                    Message = "操作失败。"
                };
            }
        }

        public BasicResponse<string> GetDBType()
        {
            var dbType = Basic.Framework.Configuration.Global.DatabaseType;
            var ret = new BasicResponse<string>
            {
                Code = 100,
                Message = "操作成功。",
                Data = dbType.ToString().ToLower()
            };
            return ret;
        }

        public BasicResponse<string> GetDBName()
        {
            var ret = new BasicResponse<string>
            {
                Code = 100,
                Message = "操作成功。",
                Data = _listexRepositoryBase.DataContext.Database.Connection.Database
            };
            //return this._baseDAO.getDBName();
            return ret;
        }

        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error(strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            //switch (this._baseDAO.getServerType())
            //{
            //    case "local": //local
            //        throw ex;
            //        break;
            //    case "wcf": //wcf
            //        throw new FaultException(strTiTle + "出错\n" + ex.Message);
            //        break;
            //    default:
            //        throw ex;
            //        break;
            //}
            throw ex;
        }


        /// <summary>
        ///     SQL拼装工具
        /// </summary>
        public sealed class BulidSQLUtil
        {
            private readonly IList<string> GroupFieldList = new List<string>();
            private DataTable listMetaDataDt;
            private DataTable metadataDt;
            private DataTable metadataFieldDt;

            private string strDbCode = string.Empty;
            private string strSqlFrom = string.Empty;
            private string strSqlGroup = string.Empty;
            private string strSqlOrder = string.Empty;
            private string strSqlSelect = string.Empty;
            private string strSqlWhere = string.Empty;

            private IDictionary<string, int> tableNameAilasCounter
                = new Dictionary<string, int>(); //用于生成SQL主表与关联表的别名


            /// <summary>
            ///     获取SQL
            /// </summary>
            /// <param name="lmdItem"></param>
            /// <returns>string</returns>
            public string GetSQL(DataTable lmdDt)
            {
                listMetaDataDt = lmdDt.Copy();

                var strSql = string.Empty;

                try
                {
                    metadataDt = ServerDataCache.GetMetaData();
                    metadataFieldDt = ServerDataCache.GetMetaDataFields();

                    if (listMetaDataDt == null)
                        return string.Empty;

                    Clear(); //清空原始数据

                    AssemblySQL(string.Empty, 0, string.Empty, string.Empty, 0); //组织SQL

                    if (strSqlSelect != string.Empty)
                        strSqlSelect = strSqlSelect.Remove(strSqlSelect.Length - 1);

                    //组织计算列
                    AssemblyCalcBySql();
                    //组织GroupBySql
                    AssemblyGroupBySql();
                    strSql = strSqlSelect + strSqlFrom + strSqlWhere + strSqlGroup;

                    //组织strSqlOrder
                    AssemblyOrderBySql();
                    strSql = strSql + strSqlOrder;
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                return strSql;
            }

            /// <summary>
            ///     组织SQL
            /// </summary>
            /// <param name="strParentFullPath">字段全路径</param>
            /// <param name="lngParentFieldId">父字段ID</param>
            /// <param name="parentFieldName">父字段名</param>
            /// <param name="relationFieldName">关联字段名</param>
            /// <param name="lngSourceType">连接类型</param>
            public void AssemblySQL(string strParentFullPath, int lngParentFieldId, string parentFieldName,
                string relationFieldName, int lngSourceType)
            {
                try
                {
                    var strFilter = "isnull(isCalcField,0)=0 and isnull(strParentFullPath,'')='" + strParentFullPath +
                                    "'";
                    if (lngParentFieldId != 0) strFilter += " and isnull(lngParentFieldID,0)=" + lngParentFieldId;
                    var curDrs = listMetaDataDt.Select(strFilter);

                    MetadataInfo md = null;
                    MetadatafieldsInfo mdf = null;
                    ListmetadataInfo lmd = new ListmetadataInfo();

                    if ((curDrs == null) || (curDrs.Length <= 0))
                        return;

                    var metaDataId = TypeUtil.ToInt(curDrs[0]["MetaDataID"]);
                    md = GetMetaDataById(metaDataId); //获取元数据

                    var strSelect = "";
                    var strWhere = "";

                    //取表别名
                    var tableAlias = "";
                    if ((curDrs != null) && (curDrs.Length > 0))
                    {
                        lmd.MetaDataFieldName = TypeUtil.ToString(curDrs[0]["MetaDataFieldName"]); //表名_字段名
                        string strfilename = lmd.MetaDataFieldName;

                        #region 2014-11-26  由于表名有可能有下划线，所以不能直接根据下画线来定表名，要去掉字符串0索引到最后一个下画线之间的文本

                        var index = strfilename.LastIndexOf("_");
                        if (index > -1)
                            strfilename = strfilename.Substring(0, index);
                        tableAlias = strfilename;

                        #endregion

                        //string[] strF = lmd.MetaDataFieldName.Split('_');
                        //tableAlias = strF[0];
                    }

                    //字段、条件处理
                    foreach (var dr in curDrs)
                    {
                        lmd.MetaDataID = metaDataId; //元数据ID  
                        lmd.LngSourceType = TypeUtil.ToInt(dr["lngSourceType"]);
                        lmd.MetaDataFieldID = TypeUtil.ToInt(dr["MetaDataFieldID"]); //元数据字段ID
                        lmd.MetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]); //表名_字段名
                        mdf = GetMetaDataFieldsById(lmd.MetaDataFieldID); //获取元数据字段信息                        

                        //构造字段                   
                        ////if (true || false == mdf.BlnFieldRight || ServerDataCache.IsHaveFieldRigth(operContext.UserId, lmd.MetaDataFieldID))
                        ////{    //判断字段权限                        
                        ////}

                        strSelect += mdf.StrFieldName + ",";
                        strSqlSelect += tableAlias + "." + mdf.StrFieldName + "  " + lmd.MetaDataFieldName + ",";

                        //条件处理 
                        //判断是否为机构或者商品列 数据权限
                        var strReturnWhere = string.Empty;
                        if (mdf.LngDataRightType != 0)
                        {
                            strReturnWhere = GetDataRightString(mdf.LngDataRightType, mdf.StrFieldName, tableAlias);
                            if (strReturnWhere != string.Empty)
                                if (strWhere != string.Empty)
                                    strWhere = strWhere + " and " +
                                               strReturnWhere.Replace(tableAlias + ".strmanageOrgsIds",
                                                   "strmanageOrgsIds");
                                else
                                    strWhere = " where " +
                                               strReturnWhere.Replace(tableAlias + ".strmanageOrgsIds",
                                                   "strmanageOrgsIds");
                        }
                        lmd.StrCondition = TypeUtil.ToString(dr["strCondition"]); //条件串
                        lmd.StrFkCode = TypeUtil.ToString(dr["strFkCode"]);
                        if (lmd.StrCondition != string.Empty)
                        {
                            lmd.StrFieldType = TypeUtil.ToString(dr["strFieldType"]); //字段类型                       
                            //if (lmd.LngSourceType >0)
                            if ((lmd.LngSourceType > 0) || (lmd.StrFkCode != string.Empty))
                                strReturnWhere = BulidConditionUtil.GetRefCondition(mdf.StrFieldName, lmd.StrCondition,
                                    lmd.StrFieldType);
                            else
                                strReturnWhere = BulidConditionUtil.GetConditionString(mdf.StrFieldName,
                                    lmd.StrFieldType, lmd.StrCondition);

                            if (strReturnWhere != string.Empty)
                                if (strSqlWhere != string.Empty)
                                    strSqlWhere = strSqlWhere + " and " +
                                                  strReturnWhere.Replace(mdf.StrFieldName,
                                                      tableAlias + "." + mdf.StrFieldName);
                                else
                                    strSqlWhere = " where " +
                                                  strReturnWhere.Replace(mdf.StrFieldName,
                                                      tableAlias + "." + mdf.StrFieldName);
                        }
                    }

                    if (md.StrFilter != string.Empty) //元数据自带的条件
                        if (strWhere != string.Empty)
                            strWhere = strWhere + " and " + md.StrFilter;
                        else
                            strWhere = " where " + md.StrFilter;

                    if (strSelect != string.Empty)
                        strSelect = strSelect.Remove(strSelect.Length - 1);

                    var strSubSql = "   (select " + strSelect + " from " + md.StrTableName + strWhere + ") " +
                                    tableAlias;
                    if (string.Empty == strParentFullPath)
                    {
                        //strFullPath为0，表示是主实体

                        strSqlSelect = "select " + strSqlSelect;
                        strSqlFrom = "  from " + strSubSql;
                    }
                    else
                    {
                        // string[] strF = parentFieldName.Split('_');

                        //2014-12-15  由于表名有可能有下划线，所以不能直接根据下画线来定表名，要去掉字符串0索引到最后一个下画线之间的文本
                        var strTableName = "";
                        var index = parentFieldName.LastIndexOf("_");
                        if (index > -1)
                            strTableName = parentFieldName.Substring(0, index);

                        var strColumnName = parentFieldName.Substring(parentFieldName.LastIndexOf("_") + 1);

                        var strJoinType = " left join ";
                        if (lngSourceType == 2)
                            strJoinType = " right join ";
                        else if (lngSourceType == 3)
                            strJoinType = " inner join ";
                        //strSqlFrom += strJoinType + strSubSql + " on " + strF[0] + "." + strF[1] + "=" + tableAlias + "." + relationFieldName;
                        strSqlFrom += strJoinType + strSubSql + " on " + strTableName + "." + strColumnName + "=" +
                                      tableAlias + "." + relationFieldName;
                    }

                    //处理子节点
                    foreach (var dr in curDrs)
                    {
                        lmd.LngSourceType = TypeUtil.ToInt(dr["lngSourceType"]);
                        lmd.MetaDataFieldID = TypeUtil.ToInt(dr["MetaDataFieldID"]);
                        lmd.StrFullPath = TypeUtil.ToString(dr["strFullPath"]); //字段全路径描述
                        lmd.MetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]); //表名_字段名

                        mdf = GetMetaDataFieldsById(lmd.MetaDataFieldID); //获取元数据字段信息
                        if (lmd.LngSourceType > 0)
                        {
                            //lngSourceType为1，表示此字段有关联

                            MetadatafieldsInfo mdfRelative = GetMetaDataFieldsById(mdf.LngRelativeFieldID);
                            if (mdfRelative != null)
                            {
                                //throw new Exception("元数据配置有误或者列表方案存在胀数据，[" + lmd.MetaDataFieldName + "]关联无效");

                                AssemblySQL(lmd.StrFullPath, lmd.MetaDataFieldID, lmd.MetaDataFieldName,
                                    mdfRelative.StrFieldName, lmd.LngSourceType);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


            /// <summary>
            ///     组织计算列
            /// </summary>
            public void AssemblyCalcBySql()
            {
                listMetaDataDt.DefaultView.RowFilter = "isnull(isCalcField,0)>0";
                var dt = listMetaDataDt.DefaultView.ToTable();

                DataRow dr;
                var strSelect = string.Empty;
                var strFormula = string.Empty;
                var strMetaDataFieldName = string.Empty;
                var strRefColList = string.Empty;
                var strWhere = string.Empty;
                var strCondition = string.Empty;
                var strFieldType = string.Empty;
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    strRefColList = TypeUtil.ToString(dr["strRefColList"]);
                    strFormula = TypeUtil.ToString(dr["strFormula"]);
                    strMetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                    if (strRefColList == string.Empty)
                    {
                        strSelect += ",(" + strFormula + ") " + strMetaDataFieldName;
                        continue;
                    }

                    var strs = strRefColList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var str in strs)
                    {
                        //2014-11-28 sb ，由于表名有可能自身有下画线，所以不能直接把_替换为.，只能替换最后一个_
                        var strTableName = "";
                        var index = str.LastIndexOf("_");
                        if (index > -1)
                        {
                            strTableName = str.Substring(0, index);
                            strFormula = strFormula.Replace(strTableName + "_", strTableName + ".");
                        }

                        // strFormula = strFormula.Replace(str, str.Replace('_', '.'));
                    }


                    strSelect += ",(" + strFormula + ") " + strMetaDataFieldName;

                    strCondition = TypeUtil.ToString(dr["strCondition"]); //条件串                
                    strFieldType = TypeUtil.ToString(dr["strFieldType"]); //字段类型  
                    if (strWhere == string.Empty)
                        strWhere += BulidConditionUtil.GetConditionString(" (" + strFormula + ") ", strFieldType,
                            strCondition);
                    else
                        strWhere += " and " +
                                    BulidConditionUtil.GetConditionString("(" + strFormula + ") ", strFieldType,
                                        strCondition);
                }

                if (strSelect != string.Empty)
                    strSqlSelect += strSelect;
                if (strWhere != string.Empty)
                    if (strSqlWhere != string.Empty)
                        strSqlWhere += " and " + strWhere;
                    else
                        strSqlWhere = strWhere;
            }

            /// <summary>
            ///     组织GroupBySql
            /// </summary>
            public void AssemblyGroupBySql()
            {
                listMetaDataDt.DefaultView.RowFilter = "lngSummaryType>0";
                //listMetaDataDt.DefaultView.Sort = "lngRowIndex asc";
                var dt = listMetaDataDt.DefaultView.ToTable();
                DataRow dr;
                var strSelect = "";
                var lngSummaryType = 0;
                var strMetaDataFieldName = string.Empty;
                var strSummaryOper = "";
                var strRefColList = "";
                var strFormula = "";
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    lngSummaryType = TypeUtil.ToInt(dr["lngSummaryType"]);
                    if ((lngSummaryType < 1) || (lngSummaryType > 5))
                        continue;

                    if (lngSummaryType == 1)
                        strSummaryOper = "Sum";
                    else if (lngSummaryType == 2)
                        strSummaryOper = "Min";
                    else if (lngSummaryType == 3)
                        strSummaryOper = "Max";
                    else if (lngSummaryType == 4)
                        strSummaryOper = "Count";
                    else if (lngSummaryType == 5)
                        strSummaryOper = "Avg";

                    strMetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);

                    //处理计算列
                    strRefColList = TypeUtil.ToString(dr["strRefColList"]);
                    strFormula = TypeUtil.ToString(dr["strFormula"]);
                    if (strFormula != string.Empty)
                    {
                        //计算列

                        if (strRefColList != string.Empty)
                        {
                            var strs = strRefColList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var str in strs)
                            {
                                var strTableName = "";
                                var index = str.LastIndexOf("_");
                                if (index > -1)
                                {
                                    strTableName = str.Substring(0, index);
                                    strFormula = strFormula.Replace(strTableName + "_", strTableName + ".");
                                }
                                //strFormula = strFormula.Replace(str, str.Replace('_', '.'));
                            }
                        }

                        strSelect += strSummaryOper + "(" + strFormula + ") " + strMetaDataFieldName + ",";
                    }
                    else
                    {
                        var strfilename = "";
                        var index = strMetaDataFieldName.LastIndexOf("_");
                        if (index > -1)
                        {
                            strfilename = "" + strMetaDataFieldName.Remove(index, 1).Insert(index, ".");
                            strSelect += strSummaryOper + "(" + strfilename + ") " + strMetaDataFieldName + ",";
                        }
                        //strSelect += strSummaryOper + "(" + strMetaDataFieldName.Replace('_', '.') + ") " + strMetaDataFieldName + ",";
                    }

                    GroupFieldList.Add(strMetaDataFieldName);
                }

                if (strSelect != string.Empty)
                {
                    listMetaDataDt.DefaultView.RowFilter = "isnull(lngSummaryType,0)<=0 and blnShow=1";
                    //listMetaDataDt.DefaultView.Sort = "lngRowIndex asc";
                    dt = listMetaDataDt.DefaultView.ToTable();
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = dt.Rows[i];
                        strMetaDataFieldName = TypeUtil.ToString(dr["MetaDataFieldName"]);

                        //处理计算列
                        strRefColList = TypeUtil.ToString(dr["strRefColList"]);
                        strFormula = TypeUtil.ToString(dr["strFormula"]);
                        if (strFormula != string.Empty)
                        {
                            //计算列

                            if (strRefColList != string.Empty)
                            {
                                var strs = strRefColList.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var str in strs)
                                {
                                    var strTableName = "";
                                    var index = str.LastIndexOf("_");
                                    if (index > -1)
                                    {
                                        strTableName = str.Substring(0, index);
                                        strFormula = strFormula.Replace(strTableName + "_", strTableName + ".");
                                    }
                                    //strFormula = strFormula.Replace(str, str.Replace('_', '.'));
                                }
                            }

                            strSelect += "(" + strFormula + ") " + strMetaDataFieldName + ",";
                            strSqlGroup += "(" + strFormula + ") " + " ,";
                        }
                        else
                        {
                            var strfilename = "";
                            var index = strMetaDataFieldName.LastIndexOf("_");
                            if (index > -1)
                                strfilename = "" + strMetaDataFieldName.Remove(index, 1).Insert(index, ".");
                            strSelect += strfilename + "  " + strMetaDataFieldName + ",";
                            strSqlGroup += strfilename + " ,";

                            //strSelect += strMetaDataFieldName.Replace('_', '.') + "  " + strMetaDataFieldName + ",";
                            //strSqlGroup += strMetaDataFieldName.Replace('_', '.') + " ,";
                        }

                        GroupFieldList.Add(strMetaDataFieldName);
                    }

                    if (strSqlGroup != string.Empty)
                        strSqlGroup = "  group by " + strSqlGroup.Remove(strSqlGroup.Length - 1);

                    strSelect = strSelect.Remove(strSelect.Length - 1);

                    strSqlSelect = "select " + strSelect;
                }
            }


            /// <summary>
            ///     组织OrderBySQL
            /// </summary>
            public void AssemblyOrderBySql()
            {
                listMetaDataDt.DefaultView.RowFilter = "lngOrderMethod>0";
                listMetaDataDt.DefaultView.Sort = "lngOrder asc";
                var dt = listMetaDataDt.DefaultView.ToTable();
                DataRow dr;
                var lngOrderMethod = 0;
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];

                    if ((GroupFieldList.Count > 0) &&
                        !GroupFieldList.Contains(TypeUtil.ToString(dr["MetaDataFieldName"])))
                        continue;

                    lngOrderMethod = TypeUtil.ToInt(dr["lngOrderMethod"]);
                    var strOrderByFileName = TypeUtil.ToString(dr["MetaDataFieldName"]);
                    var index = strOrderByFileName.LastIndexOf("_");
                    if (index > -1)
                        strOrderByFileName = strOrderByFileName.Remove(index, 1).Insert(index, ".");


                    if ((lngOrderMethod == 1) || (lngOrderMethod != 2))
                        strSqlOrder += strOrderByFileName + " Asc ,";
                    else
                        strSqlOrder += strOrderByFileName + " Desc ,";
                }

                if (strSqlOrder != string.Empty)
                    strSqlOrder = "  order by " + strSqlOrder.Remove(strSqlOrder.Length - 1);
            }


            /// <summary>
            ///     获取数据权限字符串（机构和商品）
            /// </summary>
            /// <param name="lngDataRightType">数据权限类型</param>
            /// <returns>string</returns>
            /// <remarks>
            ///     数据权限类型
            ///     0 不控制数据权限
            ///     1 制单机构 2 管辖机构 3 查看机构
            ///     4 制单机构或者管辖机构
            ///     5 查看机构或者管辖机构
            ///     6 商品类型
            /// </remarks>
            public string GetDataRightString(int lngDataRightType, string fieldName)
            {
                var strCondition = "";

                // 0 不控制数据权限 1 机构 2 商品类型
                if (lngDataRightType == 1)
                {
                    //机构

                    var strOrg = "${orgId}";
                    var strManageOrg = "${manageOrgsId}";
                    //strCondition = string.Format(@" ({0}={1} or {0} in ({2}) or ','+'{1}'+',' like '%,'+{3}+',%')",
                    //   fieldName, strOrg, strManageOrg);
                    strCondition = string.Format(@" ({0}={1} or {0} in ({2}))", fieldName, strOrg, strManageOrg);
                }
                else if (lngDataRightType == 2)
                {
                    //商品类型，暂不控制

                    var strBlnPTDR = "${blnPTDR}";
                    var strProTypeIds = "${ProTypeIds}";
                    strCondition = string.Format("(0={1}  or isnull({0},0)=0 or {0} in ({2}))", fieldName, strBlnPTDR,
                        strProTypeIds);
                }

                #region oldCode

                //switch (lngDataRightType)
                //{
                //    case 1://制单机构
                //        strCondition = fieldName + "=${orgId}";
                //        break;
                //    case 2://管辖机构
                //        strCondition = fieldName + " in (${manageOrgsId})";
                //        break;
                //    case 3://查看机构   
                //        strCondition = ",${orgId}, like '%," + fieldName + ",%'";
                //        break;
                //    case 4://制单机构或者管辖机构
                //        strCondition = " (" + fieldName + "=${orgId}  or  " +
                //            fieldName + " in (${manageOrgsId}))";
                //        break;
                //    case 5://查看机构或者管辖机构
                //        strCondition = ",${orgId}, like '%," + fieldName + ",%'" + "  or  " +
                //           fieldName + " in (${manageOrgsId}))";
                //        break;
                //    case 6://商品类型
                //        strCondition = fieldName + " in (${ProTypeIds})";
                //        break;
                //    default:
                //        break;
                //}

                #endregion

                return strCondition;
            }

            /// <summary>
            ///     获取数据权限字符串（机构和商品）
            /// </summary>
            /// <param name="lngDataRightType">数据权限类型</param>
            /// <returns>string</returns>
            /// <remarks>
            ///     数据权限类型
            ///     0 不控制数据权限
            ///     1 机构
            ///     2 商品类型
            /// </remarks>
            public string GetDataRightString(int lngDataRightType, string fieldName, string tableAlias)
            {
                var strCondition = "";

                // 0 不控制数据权限 1 机构 2 商品类型
                if (lngDataRightType == 1)
                {
                    //机构

                    var strOrg = "${orgId}";
                    var strManageOrg = "${manageOrgsId}";
                    var strBlnManagerAllOrg = "${blnManagerAllOrg}";

                    var strFilter = "isnull(Metadatafieldname,'')='" + tableAlias + "_strmanageOrgsIds'";
                    var curDrs = listMetaDataDt.Select(strFilter);
                    if ((curDrs != null) && (curDrs.Length > 0))
                        strCondition =
                            string.Format(
                                @" ({0}={1} or (1={4} or {0} in ({2})) or   ','+{3}.strmanageOrgsIds+',' like '%,'+'{1}'+',%')",
                                fieldName, strOrg, strManageOrg, tableAlias, strBlnManagerAllOrg);
                    else
                        strCondition = string.Format(@" ({0}={1} or (1={3} or {0} in ({2})))", fieldName, strOrg,
                            strManageOrg, strBlnManagerAllOrg);
                }
                else if (lngDataRightType == 2)
                {
                    //商品类型

                    var strBlnPTDR = "${blnPTDR}";
                    var strProTypeIds = "${ProTypeIds}";
                    strCondition = string.Format("(0={1}  or  isnull({0},0)=0 or {0} in ({2}))", fieldName, strBlnPTDR,
                        strProTypeIds);
                }

                return strCondition;
            }


            /// <summary>
            ///     获取条件串
            /// </summary>
            /// <param name="fieldName">字段名</param>
            /// <param name="strOper">运算符</param>
            /// <param name="strDataRange1">数据范围1</param>
            /// <param name="strDataRange2">数据范围2</param>
            /// <returns>string</returns>
            public string GetConditionString(string fieldName, string strOper, string strDataRange1,
                string strDataRange2)
            {
                var strCondition = "";

                switch (strOper)
                {
                    case "等于":
                        strCondition = fieldName + "='" + strDataRange1 + "'";
                        break;
                    case "不等于":
                        strCondition = fieldName + "<>'" + strDataRange1 + "'";
                        break;
                    case "大于":
                        strCondition = fieldName + ">'" + strDataRange1 + "'";
                        break;
                    case "小于":
                        strCondition = fieldName + "<'" + strDataRange1 + "'";
                        break;
                    case "大于等于":
                        strCondition = fieldName + ">='" + strDataRange1 + "'";
                        break;
                    case "小于等于":
                        strCondition = fieldName + "<='" + strDataRange1 + "'";
                        break;
                    case "两者之间":
                        strCondition = fieldName + "between '" + strDataRange1 + "' and '" + strDataRange2 + "'";
                        break;
                    case "等于空": //用于非数字类型
                        strCondition = "isnull(" + fieldName + ",'')=''";
                        break;
                    case "不等于空":
                        strCondition = "isnull(" + fieldName + ",'')<>''";
                        break;
                    default:
                        break;
                }

                return strCondition;
            }

            /// <summary>
            ///     清空原始数据
            /// </summary>
            private void Clear()
            {
                strSqlSelect = string.Empty;
                strSqlFrom = string.Empty;
                strSqlWhere = " where 1=1 "; //添加常用查询条件修改 2011-06-15
                strSqlOrder = string.Empty;
                strSqlGroup = string.Empty;
                GroupFieldList.Clear();
            }


            /// <summary>
            ///     获取元数据
            /// </summary>
            /// <param name="id">元数据ID</param>
            /// <returns>MetaData</returns>
            public MetadataInfo GetMetaDataById(int id)
            {
                MetadataInfo md = null;
                try
                {
                    var drs = metadataDt.Select("ID=" + id);
                    if (drs.Length > 0)
                    {
                        md = new MetadataInfo();
                        md.ID = id;
                        md.StrTableName = TypeUtil.ToString(drs[0]["strTableName"]);
                        md.StrName = TypeUtil.ToString(drs[0]["strName"]);
                        md.StrBusinessModule = TypeUtil.ToString(drs[0]["strBusinessModule"]);
                        md.StrSrcType = TypeUtil.ToString(drs[0]["strSrcType"]);
                        md.TypeID = TypeUtil.ToInt(drs[0]["TypeID"]);
                        md.StrFilter = TypeUtil.ToString(drs[0]["strFilter"]);
                        md.SourceMetaDataID = TypeUtil.ToInt(drs[0]["SourceMetaDataID"]);
                        md.StrKeyFieldPropName = TypeUtil.ToString(drs[0]["strKeyFieldPropName"]);
                        md.StrDesc = TypeUtil.ToString(drs[0]["strDesc"]);
                        md.StrLastUpdateTime = TypeUtil.ToString(drs[0]["strLastUpdateTime"]);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return md;
            }

            /// <summary>
            ///     获取元数据字段数据
            /// </summary>
            /// <returns>MetaDataFields</returns>
            public MetadatafieldsInfo GetMetaDataFieldsById(int id)
            {
                MetadatafieldsInfo mdf = null;
                try
                {
                    var drs = metadataFieldDt.Select("ID=" + id);
                    if (drs.Length > 0)
                    {
                        mdf = new MetadatafieldsInfo();
                        mdf.ID = id;
                        mdf.MetaDataID = TypeUtil.ToInt(drs[0]["MetaDataID"]);
                        mdf.StrMetaDataTable = TypeUtil.ToString(drs[0]["strMetaDataTable"]);
                        mdf.StrFieldName = TypeUtil.ToString(drs[0]["strFieldName"]);
                        mdf.StrFieldChName = TypeUtil.ToString(drs[0]["strFieldChName"]);
                        mdf.StrFieldDesc = TypeUtil.ToString(drs[0]["strFieldDesc"]);
                        mdf.StrFieldType = TypeUtil.ToString(drs[0]["strFieldType"]);
                        mdf.LngFieldLen = TypeUtil.ToInt(drs[0]["lngFieldLen"]);
                        mdf.DecimalNum = TypeUtil.ToInt(drs[0]["decimalNum"]);
                        mdf.BlnMust = TypeUtil.ToBool(drs[0]["blnMust"]);
                        mdf.StrDefaultValue = TypeUtil.ToString(drs[0]["strDefaultValue"]);
                        mdf.BlnHidden = TypeUtil.ToBool(drs[0]["blnHidden"]);
                        mdf.BlnFieldRight = TypeUtil.ToBool(drs[0]["blnFieldRight"]);
                        mdf.LngDataRightType = TypeUtil.ToInt(drs[0]["lngDataRightType"]);
                        mdf.LngRowIndex = TypeUtil.ToInt(drs[0]["lngRowIndex"]);
                        mdf.LngSourceType = TypeUtil.ToInt(drs[0]["lngSourceType"]);
                        mdf.LngRelativeID = TypeUtil.ToInt(drs[0]["lngRelativeID"]);
                        mdf.LngRelativeFieldID = TypeUtil.ToInt(drs[0]["lngRelativeFieldID"]);
                        mdf.LngFieldShowStyle = TypeUtil.ToInt(drs[0]["lngFieldShowStyle"]);
                        mdf.StrDisplayFormat = TypeUtil.ToString(drs[0]["strDisplayFormat"]);
                        mdf.StrLastUpdateTime = TypeUtil.ToString(drs[0]["strLastUpdateTime"]);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return mdf;
            }

            /// <summary>
            ///     是否控制字段权限
            /// </summary>
            public bool IsFieldRightById(int id)
            {
                var blnFieldRight = false;
                try
                {
                    var drs = metadataFieldDt.Select("ID=" + id);
                    if (drs.Length > 0)
                        blnFieldRight = TypeUtil.ToBool(drs[0]["blnFieldRight"]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return blnFieldRight;
            }
        }

        public BasicResponse SaveListExInfo(SaveListExInfoRequest request)
        {
            switch (request.Info.InfoState)
            {
                case InfoState.AddNew:
                    var model2 = ObjectConverter.Copy<ListexInfo, ListexModel>(request.Info);
                    _listexRepository.AddListex(model2);
                    break;
                case InfoState.Delete:
                    _listexRepository.DeleteListex(request.Info.ListID.ToString());
                    break;
                case InfoState.Modified:
                    var model = ObjectConverter.Copy<ListexInfo, ListexModel>(request.Info);
                    _listexRepository.UpdateListex(model);
                    break;
            }
            return new BasicResponse();
        }
    }
}