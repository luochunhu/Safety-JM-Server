using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Menu;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class MenuService : IMenuService
    {
        private IMenuRepository _Repository;

        /// <summary>
        /// 根据传入的菜单名称判断当前菜单是否已存在
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <returns>true:存在  false:不存在</returns>
        private bool CheckMenuNameExist(string menuName)
        {
            bool isExist = false;
            string strSql = "";
            MenuModel menuDTO = null;
            try
            {
                List<MenuModel> MenuList = _Repository.GetMenuList();
                menuDTO = MenuList.Find(a => a.MenuName == menuName);
                if (menuDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(menuDTO.MenuID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch (System.Exception ex)
            {
                isExist = false;
                //ThrowException("CheckMenuNameExist", ex);
            }
            return isExist;
        }
        /// <summary>
        /// 根据传入的菜单名称判断当前菜单是否已存在
        /// </summary>
        /// <param name="menuName">菜单名称</param>
        /// <returns>true:存在  false:不存在</returns>
        private string GetMenuCodeByMenuName(string menuName)
        {
            string strMenuCode = "";
            string strSql = "";
            MenuModel menuDTO = null;
            try
            {
                if (menuName != "一级模块")
                {
                    List<MenuModel> MenuList = _Repository.GetMenuList();
                    menuDTO = MenuList.Find(a => a.MenuName == menuName);
                    strMenuCode = menuDTO.MenuCode;
                }
            }
            catch
            {
                strMenuCode = "";
            }
            return strMenuCode;
        }
        public MenuService(IMenuRepository _Repository)
        {
            this._Repository = _Repository;
        }

        public BasicResponse<MenuInfo> AddMenu(MenuAddRequest menurequest)
        {
            MenuInfo menuDTO = menurequest.MenuInfo;
            string menuCode = "001";//菜单编码
            string parentMenuCode = "";//父菜单编码
            short menuMemo = 0;//菜单标志
            string menuSort = "0";

            if (string.IsNullOrEmpty(menuDTO.MenuName))
            {
                ThrowException("AddMenu", new Exception("模块名称必须输入！"));
            }
            if (string.IsNullOrEmpty(menuDTO.MenuParent))
            {
                ThrowException("AddMenu", new Exception("请选择模块上级模块！"));
            }
            if (!string.IsNullOrEmpty(menuDTO.MenuFile))
            {
                if (!menuDTO.MenuFile.Contains(".dll") && !menuDTO.MenuFile.Contains(".exe"))
                {
                    ThrowException("AddMenu", new Exception("菜单引用文件必须是dll文件或者是exe文件！"));
                }
            }
            parentMenuCode = GetMenuCodeByMenuName(menuDTO.MenuParent.Trim());
            List<MenuModel> MenuList = _Repository.GetMenuList();
            List<MenuModel> SonMenuList = new List<MenuModel>();
            if (string.IsNullOrEmpty(parentMenuCode))//一级目录
            {
                SonMenuList = MenuList.FindAll(a => a.MenuMemo == -1).OrderByDescending(a => a.MenuCode).ToList();
                if (SonMenuList.Count > 0)
                {
                    //将此一级目录加在当前最后一个一级目录的后面
                    menuCode = (int.Parse(SonMenuList[0].MenuCode) + 1).ToString("000");
                    menuSort = (int.Parse(SonMenuList[0].MenuCode) + 1).ToString();
                }
                else
                {
                    menuSort = "1";
                }
                menuMemo = -1;
            }
            else//子模块
            {
                SonMenuList = MenuList.FindAll(a => a.MenuParent == parentMenuCode).OrderByDescending(a => a.MenuCode).ToList();
                if (SonMenuList.Count > 0)
                {
                    //将此一级目录加在当前最后一个一级目录的后面
                    menuCode = parentMenuCode.ToString() +
                        (int.Parse(SonMenuList[0].MenuCode.Substring(parentMenuCode.Length + 1)) + 1).ToString("000");
                    menuSort = (int.Parse(SonMenuList[0].MenuCode.Substring(parentMenuCode.Length + 1)) + 1).ToString();
                }
                else
                {
                    //当前模块是父模块的第一个模块
                    menuCode = parentMenuCode.ToString() + "001";
                    menuSort = "1";
                }
            }
            menuDTO.MenuCode = menuCode;
            menuDTO.MenuMemo = menuMemo;
            menuDTO.MenuSort = menuSort;
            menuDTO.MenuParent = parentMenuCode;

            //判断菜单名是否存在，只有不存在才能插入
            if (CheckMenuNameExist(menuDTO.MenuName))
            {
                ThrowException("AddMenu", new Exception(String.Format("菜单名:{0} 已存在，请重新输入！", menuDTO.MenuName)));
            }

            var _menu = ObjectConverter.Copy<MenuInfo, MenuModel>(menuDTO);
            var resultmenu = _Repository.AddMenu(_menu);
            var menuresponse = new BasicResponse<MenuInfo>();
            menuresponse.Data = ObjectConverter.Copy<MenuModel, MenuInfo>(resultmenu);
            return menuresponse;
        }
        public BasicResponse<MenuInfo> UpdateMenu(MenuUpdateRequest menurequest)
        {
            var _menu = ObjectConverter.Copy<MenuInfo, MenuModel>(menurequest.MenuInfo);
            if (string.IsNullOrEmpty(_menu.MenuName))
            {
                //throw new BusinessException("菜单名称不能为空！");
                ThrowException("UpdateMenu", new Exception(String.Format("菜单名称不能为空！")));
            }
            _Repository.UpdateMenu(_menu);
            var menuresponse = new BasicResponse<MenuInfo>();
            menuresponse.Data = ObjectConverter.Copy<MenuModel, MenuInfo>(_menu);
            return menuresponse;
        }
        public BasicResponse DeleteMenu(MenuDeleteRequest menurequest)
        {
            _Repository.DeleteMenu(menurequest.Id);
            var menuresponse = new BasicResponse();
            return menuresponse;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menurequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteMenus(MenusDeleteRequest menurequest)
        {
            foreach (string Id in menurequest.IdList)
            {
                _Repository.DeleteMenu(Id);
            }
            var menuresponse = new BasicResponse();
            return menuresponse;
        }
        public BasicResponse<List<MenuInfo>> GetMenuList(MenuGetListRequest menurequest)
        {
            var menuresponse = new BasicResponse<List<MenuInfo>>();
            menurequest.PagerInfo.PageIndex = menurequest.PagerInfo.PageIndex - 1;
            if (menurequest.PagerInfo.PageIndex < 0)
            {
                menurequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var menuModelLists = _Repository.GetMenuList(menurequest.PagerInfo.PageIndex, menurequest.PagerInfo.PageSize, out rowcount);
            var menuInfoLists = new List<MenuInfo>();
            foreach (var item in menuModelLists)
            {
                var MenuInfo = ObjectConverter.Copy<MenuModel, MenuInfo>(item);
                menuInfoLists.Add(MenuInfo);
            }
            menuresponse.Data = menuInfoLists;
            return menuresponse;
        }
        /// <summary>
        /// 获取所有菜单信息
        /// </summary>
        /// <returns></returns>
        public BasicResponse<List<MenuInfo>> GetMenuList()
        {
            var menuresponse = new BasicResponse<List<MenuInfo>>();
            var menuModelLists = _Repository.GetMenuList();
            var menuInfoLists = new List<MenuInfo>();
            foreach (var item in menuModelLists)
            {
                var MenuInfo = ObjectConverter.Copy<MenuModel, MenuInfo>(item);
                menuInfoLists.Add(MenuInfo);
            }
            menuresponse.Data = menuInfoLists;
            return menuresponse;
        }
        public BasicResponse<MenuInfo> GetMenuById(MenuGetRequest menurequest)
        {
            var result = _Repository.GetMenuById(menurequest.Id);
            var menuInfo = ObjectConverter.Copy<MenuModel, MenuInfo>(result);
            var menuresponse = new BasicResponse<MenuInfo>();
            menuresponse.Data = menuInfo;
            return menuresponse;
        }
        private void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("MenuService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 根据菜单编码得到菜单名称
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public BasicResponse<string> GetMenuNameByMenuCode(MenuGetByCOdeRequest menurequest)
        {
            BasicResponse<string> Result = new BasicResponse<string>();

            string strMenuName = "";

            string strSql = "";
            MenuModel menuDTO = null;
            try
            {
                if (string.IsNullOrEmpty(menurequest.MenuCode))
                {
                    strMenuName = "一级模块";
                    Result.Data = strMenuName;
                    return Result;
                }
                menuDTO = _Repository.GetMenuByCode(menurequest.MenuCode);
                strMenuName = menuDTO.MenuName;
            }
            catch (System.Exception ex)
            {
                strMenuName = "一级模块";
                //ThrowException("GetMenuNameByMenuCode", ex);
            }
            Result.Data = strMenuName;
            return Result;
        }
        /// <summary>
        /// 添加一个全新信息到菜单表并返回成功后的菜单对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="menuDTO"></param>
        /// <returns></returns>
        public BasicResponse<MenuInfo> AddMenuEx(MenuAddRequest menurequest)
        {
            BasicResponse<MenuInfo> Result = new BasicResponse<MenuInfo>();
            MenuInfo menuDTO = menurequest.MenuInfo;
            try
            {
                long ID = 0;
                string menuCode = "001";//菜单编码
                string parentMenuCode = "";//父菜单编码
                short menuMemo = 0;//菜单标志
                string menuSort = "0";

                if (menuDTO == null)
                {
                    ThrowException("AddMenuEx", new Exception("菜单对象为空，请检查是否已赋值！"));
                }
                if (menuDTO.InfoState == InfoState.NoChange)
                {
                    ThrowException("AddMenuEx", new Exception("DTO对象未设置状态，请先设置！"));
                }
                if (string.IsNullOrEmpty(menuDTO.MenuName))
                {
                    ThrowException("AddMenuEx", new Exception("菜单名称必须输入！"));
                }
                if (string.IsNullOrEmpty(menuDTO.Remark1))
                {
                    ThrowException("AddMenuEx", new Exception("菜单简称必须输入！"));
                }
                //if (string.IsNullOrEmpty(menuDTO.MenuParent))
                //{
                //    ThrowException("AddMenuEx", new Exception("请选择菜单上级模块！"));
                //}
                if (!string.IsNullOrEmpty(menuDTO.MenuFile))
                {
                    if (!menuDTO.MenuFile.Contains(".dll") && !menuDTO.MenuFile.Contains(".exe"))
                    {
                        ThrowException("AddMenuEx", new Exception("菜单引用文件必须是dll文件或者是exe文件！"));
                    }
                }
                if (string.IsNullOrEmpty(menuDTO.MenuCode))
                {
                    #region 计算菜单编码
                    parentMenuCode = menuDTO.MenuParent.Trim();
                    List<MenuModel> MenuList = _Repository.GetMenuList();
                    List<MenuModel> SonMenuList = new List<MenuModel>();
                    if (string.IsNullOrEmpty(parentMenuCode))//一级目录
                    {
                        SonMenuList = MenuList.FindAll(a => a.MenuMemo == -1).OrderByDescending(a => a.MenuCode).ToList();
                        if (SonMenuList.Count > 0)
                        {
                            //将此一级目录加在当前最后一个一级目录的后面
                            menuCode = (int.Parse(SonMenuList[0].MenuCode) + 1).ToString("000");
                            menuSort = (int.Parse(SonMenuList[0].MenuCode) + 1).ToString();
                        }
                        else
                        {
                            menuSort = "1";
                        }
                        menuMemo = -1;
                    }
                    else//子模块
                    {
                        SonMenuList = MenuList.FindAll(a => a.MenuParent == parentMenuCode).OrderByDescending(a => a.MenuCode).ToList();
                        if (SonMenuList.Count > 0)
                        {
                            //将此一级目录加在当前最后一个一级目录的后面
                            menuCode = parentMenuCode.ToString() +
                                (int.Parse(SonMenuList[0].MenuCode.Substring(parentMenuCode.Length + 1)) + 1).ToString("000");

                            menuSort = (int.Parse(SonMenuList[0].MenuCode.Substring(parentMenuCode.Length + 1)) + 1).ToString();
                        }
                        else
                        {
                            //当前模块是父模块的第一个模块
                            menuCode = parentMenuCode.ToString() + "001";

                            menuSort = "1";
                        }
                    }
                    menuDTO.MenuCode = menuCode;
                    menuDTO.MenuMemo = menuMemo;
                    menuDTO.MenuSort = menuSort;
                    menuDTO.MenuParent = parentMenuCode;
                    #endregion
                }
                else
                {
                    parentMenuCode = menuDTO.MenuParent.Trim();
                    if (string.IsNullOrEmpty(parentMenuCode))//一级目录
                    {
                        menuDTO.MenuParent = "";
                    }
                    else
                    {
                        menuDTO.MenuParent = parentMenuCode;
                    }
                }
                if (menuDTO.MenuCode == menuDTO.MenuParent)
                {
                    ThrowException("AddMenuEx", new Exception("父菜单不能和菜单名称相同！"));
                }
                if (menuDTO.MenuParent.Length > 0)
                {
                    menuDTO.MenuMemo = 0;
                }

                if (menuDTO.InfoState == InfoState.AddNew)
                {
                    ID = IdHelper.CreateLongId();
                    menuDTO.MenuID = ID.ToString();
                    var _menu = ObjectConverter.Copy<MenuInfo, MenuModel>(menuDTO);
                    var resultmenu = _Repository.AddMenu(_menu);
                    Result.Data = ObjectConverter.Copy<MenuModel, MenuInfo>(resultmenu);
                }
                else
                {
                    var _menu = ObjectConverter.Copy<MenuInfo, MenuModel>(menuDTO);
                    _Repository.UpdateMenu(_menu);
                    var resultmenu = _Repository.GetMenuById(menuDTO.MenuID);
                    Result.Data = ObjectConverter.Copy<MenuModel, MenuInfo>(resultmenu);
                }

            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        public BasicResponse EnablMenu(MenusUpdateRequest lstMenuDTO)
        {
            BasicResponse Result = new BasicResponse();
            try
            {
                if (lstMenuDTO.MenuInfo.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数为空";
                }
                foreach (MenuInfo tempMenuDTO in lstMenuDTO.MenuInfo)
                {
                    tempMenuDTO.MenuFlag = 1;
                    _Repository.UpdateMenu(ObjectConverter.Copy<MenuInfo, MenuModel>(tempMenuDTO));
                }
                Result.Code = 100;

            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = "服务端执行异常" + ex.Message;
            }
            return Result;
        }
        /// <summary>
        /// 禁用菜单
        /// </summary>
        /// <param name="lstMenuDTO"></param>
        /// <returns></returns>
        public BasicResponse DisableMenu(MenusUpdateRequest lstMenuDTO)
        {
            BasicResponse Result = new BasicResponse();
            try
            {
                if (lstMenuDTO.MenuInfo.Count <= 0)
                {
                    Result.Code = 1;
                    Result.Message = "传入参数为空";
                }
                foreach (MenuInfo tempMenuDTO in lstMenuDTO.MenuInfo)
                {
                    tempMenuDTO.MenuFlag = 1;
                    _Repository.UpdateMenu(ObjectConverter.Copy<MenuInfo, MenuModel>(tempMenuDTO));
                }
                Result.Code = 100;

            }
            catch (System.Exception ex)
            {
                Result.Code = 1;
                Result.Message = "服务端执行异常" + ex.Message;
            }
            return Result;
        }
    }
}


