using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Right;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Logging;

namespace Sys.Safety.Services
{
    public partial class RightService : IRightService
    {
        private IRightRepository _Repository;

        public RightService(IRightRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<RightInfo> AddRight(RightAddRequest rightrequest)
        {
            RightInfo rightDTO = rightrequest.RightInfo;
            if (rightDTO.CreateTime != null)
            {
                rightDTO.CreateTime = Convert.ToDateTime(rightDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (string.IsNullOrEmpty(rightDTO.RightCode))
            {
                ThrowException("AddRight", new Exception("权限编码不能为空，请重新输入！"));
            }
            if (string.IsNullOrEmpty(rightDTO.RightName))
            {
                ThrowException("AddRight", new Exception("权限名称不能为空，请重新输入！"));
            }
            if (CheckRightNameExist(rightDTO.RightName))
            {
                //校验权限名是否重复               
                ThrowException("AddRight", new Exception(String.Format("权限名:{0} 已存在，请重新输入！", rightDTO.RightName)));
            }
            if (CheckRightCodeExist(rightDTO.RightCode))
            {
                //校验权限编码是否重复                
                ThrowException("AddRight", new Exception(String.Format("权限编码:{0} 已存在，请重新输入！", rightDTO.RightCode)));
            }
            //判断权限名是否存在，只有不存在才能插入
            if (CheckRightNameExist(rightDTO.RightName))
            {
                //校验权限名是否重复               
                ThrowException("AddRight", new Exception(String.Format("权限名:{0} 已存在，请重新输入！", rightDTO.RightName)));
            }

            var _right = ObjectConverter.Copy<RightInfo, RightModel>(rightDTO);
            var resultright = _Repository.AddRight(_right);
            var rightresponse = new BasicResponse<RightInfo>();
            rightresponse.Data = ObjectConverter.Copy<RightModel, RightInfo>(resultright);
            return rightresponse;
        }
        public BasicResponse<RightInfo> UpdateRight(RightUpdateRequest rightrequest)
        {
            RightInfo rightDTO = rightrequest.RightInfo;
            if (string.IsNullOrEmpty(rightDTO.RightID))
            {
                // throw new BusinessException("权限编号不能为空！");
                ThrowException("UpdateRight", new Exception("权限编号不能为空！"));
            }
            if (string.IsNullOrEmpty(rightDTO.RightName))
            {
                //throw new BusinessException("权限名称不能为空！");
                ThrowException("UpdateRight", new Exception("权限名称不能为空！"));
            }

            var _right = ObjectConverter.Copy<RightInfo, RightModel>(rightDTO);
            _Repository.UpdateRight(_right);
            var rightresponse = new BasicResponse<RightInfo>();
            rightresponse.Data = ObjectConverter.Copy<RightModel, RightInfo>(_right);
            return rightresponse;
        }
        public BasicResponse DeleteRight(RightDeleteRequest rightrequest)
        {
            _Repository.DeleteRight(rightrequest.Id);
            var rightresponse = new BasicResponse();
            return rightresponse;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        public BasicResponse DeleteRights(RightsDeleteRequest rightrequest)
        {
            foreach (string Id in rightrequest.IdList)
            {
                _Repository.DeleteRight(Id);
            }
            var rightresponse = new BasicResponse();
            return rightresponse;
        }
        public BasicResponse<List<RightInfo>> GetRightList(RightGetListRequest rightrequest)
        {
            var rightresponse = new BasicResponse<List<RightInfo>>();
            rightrequest.PagerInfo.PageIndex = rightrequest.PagerInfo.PageIndex - 1;
            if (rightrequest.PagerInfo.PageIndex < 0)
            {
                rightrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var rightModelLists = _Repository.GetRightList(rightrequest.PagerInfo.PageIndex, rightrequest.PagerInfo.PageSize, out rowcount);
            var rightInfoLists = new List<RightInfo>();
            foreach (var item in rightModelLists)
            {
                var RightInfo = ObjectConverter.Copy<RightModel, RightInfo>(item);
                rightInfoLists.Add(RightInfo);
            }
            rightresponse.Data = rightInfoLists;
            return rightresponse;
        }
        public BasicResponse<List<RightInfo>> GetRightList()
        {
            var rightresponse = new BasicResponse<List<RightInfo>>();
            var rightModelLists = _Repository.GetRightList();
            var rightInfoLists = new List<RightInfo>();
            foreach (var item in rightModelLists)
            {
                var RightInfo = ObjectConverter.Copy<RightModel, RightInfo>(item);
                rightInfoLists.Add(RightInfo);
            }
            rightresponse.Data = rightInfoLists;
            return rightresponse;
        }
        public BasicResponse<RightInfo> GetRightById(RightGetRequest rightrequest)
        {
            var result = _Repository.GetRightById(rightrequest.Id);
            var rightInfo = ObjectConverter.Copy<RightModel, RightInfo>(result);
            var rightresponse = new BasicResponse<RightInfo>();
            rightresponse.Data = rightInfo;
            return rightresponse;
        }
        public void ThrowException(string strTiTle, Exception ex)
        {
            LogHelper.Error("RightService-" + strTiTle + "出错:" + "错误原因：\n" + ex.Message + ex.StackTrace);
            throw ex;
        }
        /// <summary>
        /// 添加一个全新信息到权限表并返回成功后的权限对象(支持添加、更新，根据状态来判断)
        /// </summary>
        /// <param name="rightrequest"></param>
        /// <returns></returns>
        public BasicResponse<RightInfo> AddRightEx(RightAddRequest rightrequest)
        {
            BasicResponse<RightInfo> Result = new BasicResponse<RightInfo>();
            RightInfo rightDTO = rightrequest.RightInfo;
            try
            {
                long ID = 0;
                if (rightDTO == null)
                {
                    ThrowException("AddRightEx", new Exception("权限对象为空，请检查是否已赋值！"));
                }
                if (rightDTO.InfoState == InfoState.NoChange)
                {
                    ThrowException("AddRightEx", new Exception("DTO对象未设置状态，请先设置！"));
                }
                if (rightDTO.CreateTime != null)
                {
                    rightDTO.CreateTime = Convert.ToDateTime(rightDTO.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (string.IsNullOrEmpty(rightDTO.RightCode))
                {
                    ThrowException("AddRightEx", new Exception("权限编码不能为空，请重新输入！"));
                }
                if (string.IsNullOrEmpty(rightDTO.RightName))
                {
                    //throw new BusinessException("权限名称不能为空，请重新输入！");
                    ThrowException("AddRightEx", new Exception("权限名称不能为空，请重新输入！"));
                }
                if (rightDTO.InfoState == InfoState.AddNew)
                {
                    //判断权限名是否存在，只有不存在才能插入
                    if (!CheckRightNameExist(rightDTO.RightName) && !CheckRightCodeExist(rightDTO.RightCode))
                    {
                        ID = IdHelper.CreateLongId();
                        rightDTO.RightID = ID.ToString();
                        var _request = ObjectConverter.Copy<RightInfo, RightModel>(rightDTO);
                        var resultmenu = _Repository.AddRight(_request);
                        Result.Data = ObjectConverter.Copy<RightModel, RightInfo>(resultmenu);
                    }
                    else
                    {
                        //校验权限名是否重复                        
                        ThrowException("AddRightEx", new Exception("权限已存在，请重新输入！"));
                    }
                }
                else
                {
                    var _request = ObjectConverter.Copy<RightInfo, RightModel>(rightDTO);
                    _Repository.UpdateRight(_request);
                    var resultmenu = _Repository.GetRightById(rightDTO.RightID);
                    Result.Data = ObjectConverter.Copy<RightModel, RightInfo>(resultmenu);
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
        /// 根据传入的权限名判断当前权限是否已存在
        /// </summary>
        /// <param name="rightName">权限名</param>
        /// <returns>true:存在  false:不存在</returns>
        private bool CheckRightNameExist(string rightName)
        {
            bool isExist = false;
            string strSql = "";
            RightModel rightDTO = null;
            try
            {
                List<RightModel> RequestList = _Repository.GetRightList();
                rightDTO = RequestList.Find(a => a.RightName == rightName);
                if (rightDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(rightDTO.RightID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }
            return isExist;
        }
        /// <summary>
        /// 判断权限编码是否存在
        /// </summary>
        /// <param name="roleCode">权限编码</param>
        /// <returns></returns>
        private bool CheckRightCodeExist(string rightCode)
        {
            bool isExist = false;
            string strSql = "";
            RightModel rightDTO = null;
            try
            {
                List<RightModel> RequestList = _Repository.GetRightList();
                rightDTO = RequestList.Find(a => a.RightCode == rightCode);
                if (rightDTO == null)
                {
                    isExist = false;
                }
                if (long.Parse(rightDTO.RightID) > 0)
                {
                    isExist = true;
                }
                else
                {
                    isExist = false;
                }
            }
            catch
            {
                isExist = false;
            }
            return isExist;
        }
    }
}


