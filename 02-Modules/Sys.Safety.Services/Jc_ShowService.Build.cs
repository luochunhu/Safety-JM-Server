using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.Jc_Show;
using Basic.Framework.Common;
using Basic.Framework.Web;

namespace Sys.Safety.Services
{
    public partial class Jc_ShowService : IJc_ShowService
    {
        private IJc_ShowRepository _Repository;

        public Jc_ShowService(IJc_ShowRepository _Repository)
        {
            this._Repository = _Repository;
        }
        public BasicResponse<Jc_ShowInfo> AddJc_Show(Jc_ShowAddRequest jc_Showrequest)
        {
            var _jc_Show = ObjectConverter.Copy<Jc_ShowInfo, Jc_ShowModel>(jc_Showrequest.Jc_ShowInfo);
            var resultjc_Show = _Repository.AddJc_Show(_jc_Show);
            var jc_Showresponse = new BasicResponse<Jc_ShowInfo>();
            jc_Showresponse.Data = ObjectConverter.Copy<Jc_ShowModel, Jc_ShowInfo>(resultjc_Show);
            return jc_Showresponse;
        }
        public BasicResponse<Jc_ShowInfo> UpdateJc_Show(Jc_ShowUpdateRequest jc_Showrequest)
        {
            var _jc_Show = ObjectConverter.Copy<Jc_ShowInfo, Jc_ShowModel>(jc_Showrequest.Jc_ShowInfo);
            _Repository.UpdateJc_Show(_jc_Show);
            var jc_Showresponse = new BasicResponse<Jc_ShowInfo>();
            jc_Showresponse.Data = ObjectConverter.Copy<Jc_ShowModel, Jc_ShowInfo>(_jc_Show);
            return jc_Showresponse;
        }
        public BasicResponse DeleteJc_Show(Jc_ShowDeleteRequest jc_Showrequest)
        {
            _Repository.DeleteJc_Show(jc_Showrequest.Id);
            var jc_Showresponse = new BasicResponse();
            return jc_Showresponse;
        }
        public BasicResponse<List<Jc_ShowInfo>> GetJc_ShowList(Jc_ShowGetListRequest jc_Showrequest)
        {
            var jc_Showresponse = new BasicResponse<List<Jc_ShowInfo>>();
            jc_Showrequest.PagerInfo.PageIndex = jc_Showrequest.PagerInfo.PageIndex - 1;
            if (jc_Showrequest.PagerInfo.PageIndex < 0)
            {
                jc_Showrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jc_ShowModelLists = _Repository.GetJc_ShowList(jc_Showrequest.PagerInfo.PageIndex, jc_Showrequest.PagerInfo.PageSize, out rowcount);
            var jc_ShowInfoLists = new List<Jc_ShowInfo>();
            foreach (var item in jc_ShowModelLists)
            {
                var Jc_ShowInfo = ObjectConverter.Copy<Jc_ShowModel, Jc_ShowInfo>(item);
                jc_ShowInfoLists.Add(Jc_ShowInfo);
            }
            jc_Showresponse.Data = jc_ShowInfoLists;
            return jc_Showresponse;
        }
        public BasicResponse<Jc_ShowInfo> GetJc_ShowById(Jc_ShowGetRequest jc_Showrequest)
        {
            var result = _Repository.GetJc_ShowById(jc_Showrequest.Id);
            var jc_ShowInfo = ObjectConverter.Copy<Jc_ShowModel, Jc_ShowInfo>(result);
            var jc_Showresponse = new BasicResponse<Jc_ShowInfo>();
            jc_Showresponse.Data = jc_ShowInfo;
            return jc_Showresponse;
        }

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="jc_Showrequest"></param>
        /// <returns></returns>
        public BasicResponse<bool> SaveCustomPagePoints(SaveCustomPagePointsRequest jc_Showrequest)
        {
            var jc_Showresponse = new BasicResponse<bool>();
            if (!jc_Showrequest.Page.HasValue && jc_Showrequest.Jc_ShowInfoList == null && jc_Showrequest.Jc_ShowInfoList.Count < 0)
            {
                jc_Showresponse.Data = false;
                jc_Showresponse.Code = -100;
                jc_Showresponse.Message = "参数错误！";
                return jc_Showresponse;
            }
            try
            {
                //根据page删除jc_Show
                _Repository.DeleteJc_ShowModelByPage(jc_Showrequest.Page.Value);
                //批量插入js_showModel到数据库
                _Repository.Insert(ObjectConverter.CopyList<Jc_ShowInfo, Jc_ShowModel>(jc_Showrequest.Jc_ShowInfoList));

                jc_Showresponse.Data = false;
                jc_Showresponse.Code = 100;
                jc_Showresponse.Message = "操作成功！";
            }
            catch (Exception ex)
            {
                jc_Showresponse.Data = false;
                jc_Showresponse.Code = -100;
                jc_Showresponse.Message = ex.ToString();
            }            

            return jc_Showresponse;
        }
    }
}


