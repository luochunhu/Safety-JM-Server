using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.ServiceContract;
using Sys.Safety.DataContract;
using Sys.Safety.Model;
using Sys.Safety.Request;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Basic.Framework.Common;
using Basic.Framework.Web;
using System.Data;
using Basic.Framework.Data;

namespace Sys.Safety.Services
{
    public partial class AlarmNotificationPersonnelService : IAlarmNotificationPersonnelService
    {
        private IAlarmNotificationPersonnelRepository _Repository;
        private IUserRepository _userRepository;

        public AlarmNotificationPersonnelService(IAlarmNotificationPersonnelRepository _Repository, IUserRepository _userRepository)
        {
            this._Repository = _Repository;
            this._userRepository = _userRepository;
        }
        public BasicResponse<JC_AlarmNotificationPersonnelInfo> AddJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        {
            var _jC_AlarmNotificationPersonnel = ObjectConverter.Copy<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>(jC_AlarmNotificationPersonnelrequest.JC_AlarmNotificationPersonnelInfo);
            var resultjC_AlarmNotificationPersonnel = _Repository.AddJC_AlarmNotificationPersonnel(_jC_AlarmNotificationPersonnel);
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<JC_AlarmNotificationPersonnelInfo>();
            jC_AlarmNotificationPersonnelresponse.Data = ObjectConverter.Copy<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(resultjC_AlarmNotificationPersonnel);
            return jC_AlarmNotificationPersonnelresponse;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelrequest"></param>
        /// <returns></returns>
        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> AddJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelAddRequest jC_AlarmNotificationPersonnelrequest)
        { 
            DataTable dataTable = _Repository.QueryTable("global_AlarmNotificationPersonnelService_GetAlarmNotificationPersonnelListByAnalysisModelId", jC_AlarmNotificationPersonnelrequest.AnalysisModelId);

            List<JC_AlarmNotificationPersonnelModel> listResult = ObjectConverter.Copy<JC_AlarmNotificationPersonnelModel>(dataTable);
            var _jC_AlarmNotificationPersonnel = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>
                (jC_AlarmNotificationPersonnelrequest.JC_AlarmNotificationPersonnelInfoList);
            try
            {
                TransactionsManager.BeginTransaction(() =>
                {
                    _Repository.DeleteJC_AlarmNotificationPersonnelList(listResult);

                    _Repository.AddJC_AlarmNotificationPersonnelList(_jC_AlarmNotificationPersonnel.ToList());
                });
            }
            catch
            {

            }

            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<List<JC_AlarmNotificationPersonnelInfo>>();
            jC_AlarmNotificationPersonnelresponse.Data = jC_AlarmNotificationPersonnelrequest.JC_AlarmNotificationPersonnelInfoList;
            return jC_AlarmNotificationPersonnelresponse;
        }
        

        public BasicResponse<JC_AlarmNotificationPersonnelInfo> UpdateJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelUpdateRequest jC_AlarmNotificationPersonnelrequest)
        {
            var _jC_AlarmNotificationPersonnel = ObjectConverter.Copy<JC_AlarmNotificationPersonnelInfo, JC_AlarmNotificationPersonnelModel>(jC_AlarmNotificationPersonnelrequest.JC_AlarmNotificationPersonnelInfo);
            _Repository.UpdateJC_AlarmNotificationPersonnel(_jC_AlarmNotificationPersonnel);
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<JC_AlarmNotificationPersonnelInfo>();
            jC_AlarmNotificationPersonnelresponse.Data = ObjectConverter.Copy<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(_jC_AlarmNotificationPersonnel);
            return jC_AlarmNotificationPersonnelresponse;
        }
        public BasicResponse DeleteJC_AlarmNotificationPersonnel(AlarmNotificationPersonnelDeleteRequest jC_AlarmNotificationPersonnelrequest)
        {
            _Repository.DeleteJC_AlarmNotificationPersonnel(jC_AlarmNotificationPersonnelrequest.Id);
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse();
            return jC_AlarmNotificationPersonnelresponse;
        }
        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelList(AlarmNotificationPersonnelGetListRequest jC_AlarmNotificationPersonnelrequest)
        {
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<List<JC_AlarmNotificationPersonnelInfo>>();
            jC_AlarmNotificationPersonnelrequest.PagerInfo.PageIndex = jC_AlarmNotificationPersonnelrequest.PagerInfo.PageIndex - 1;
            if (jC_AlarmNotificationPersonnelrequest.PagerInfo.PageIndex < 0)
            {
                jC_AlarmNotificationPersonnelrequest.PagerInfo.PageIndex = 0;
            }
            int rowcount = 0;
            var jC_AlarmNotificationPersonnelModelLists = _Repository.GetJC_AlarmNotificationPersonnelList(jC_AlarmNotificationPersonnelrequest.PagerInfo.PageIndex, jC_AlarmNotificationPersonnelrequest.PagerInfo.PageSize, out rowcount);
            var jC_AlarmNotificationPersonnelInfoLists = new List<JC_AlarmNotificationPersonnelInfo>();
            foreach (var item in jC_AlarmNotificationPersonnelModelLists)
            {
                var JC_AlarmNotificationPersonnelInfo = ObjectConverter.Copy<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(item);
                jC_AlarmNotificationPersonnelInfoLists.Add(JC_AlarmNotificationPersonnelInfo);
            }
            jC_AlarmNotificationPersonnelresponse.Data = jC_AlarmNotificationPersonnelInfoLists;
            return jC_AlarmNotificationPersonnelresponse;
        }
        public BasicResponse<JC_AlarmNotificationPersonnelInfo> GetJC_AlarmNotificationPersonnelById(AlarmNotificationPersonnelGetRequest jC_AlarmNotificationPersonnelrequest)
        {
            var result = _Repository.GetJC_AlarmNotificationPersonnelById(jC_AlarmNotificationPersonnelrequest.Id);
            var jC_AlarmNotificationPersonnelInfo = ObjectConverter.Copy<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(result);
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<JC_AlarmNotificationPersonnelInfo>();
            jC_AlarmNotificationPersonnelresponse.Data = jC_AlarmNotificationPersonnelInfo;
            return jC_AlarmNotificationPersonnelresponse;
        }

        /// <summary>
        /// 获取报警配置相关的人员信息
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelrequest">获取报警配置相关的人员信息请求对象</param>
        /// <returns>报警配置相关的人员信息</returns>
        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetJC_AlarmNotificationPersonnelListByAlarmConfigId(AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            var result = _Repository.GetJC_AlarmNotificationPersonnelListByAlarmConfigId(jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest.AlarmConfigId);
            var jC_AlarmNotificationPersonnelresponse = new BasicResponse<List<JC_AlarmNotificationPersonnelInfo>>();
            IList<JC_AlarmNotificationPersonnelInfo> responseData = ObjectConverter.CopyList<JC_AlarmNotificationPersonnelModel, JC_AlarmNotificationPersonnelInfo>(result);
            jC_AlarmNotificationPersonnelresponse.Data = responseData.ToList();
            return jC_AlarmNotificationPersonnelresponse;
        }
        /// <summary>
        /// 根据模型ID查询报警推送人员配置信息 
        /// </summary>
        /// <param name="jC_AlarmNotificationPersonnelrequest"></param>
        /// <returns>报警配置相关的人员信息</returns>
        public BasicResponse<List<JC_AlarmNotificationPersonnelInfo>> GetAlarmNotificationPersonnelByAnalysisModelId(AlarmNotificationPersonnelGetListByAlarmConfigIdRequest jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest)
        {
            DataTable dataTable = _Repository.QueryTable("global_AlarmNotificationPersonnelService_GetAlarmNotificationPersonnelByAnalysisModelId", jC_AlarmNotificationPersonnelListByAlarmConfigIdRequest.AnalysisModelId);

            List<JC_AlarmNotificationPersonnelInfo> listResult = new List<JC_AlarmNotificationPersonnelInfo>();

            List<UserInfo> UserInfolist = ObjectConverter.CopyList<UserModel, UserInfo>(_userRepository.GetUserList()).ToList();

            if (UserInfolist != null && UserInfolist.Count > 0)
            {
                foreach (var item in UserInfolist)
                {
                    JC_AlarmNotificationPersonnelInfo JC_AlarmNotificationPersonnelInfo = new JC_AlarmNotificationPersonnelInfo();
                    bool stateCheck = false;
                    if (dataTable == null || dataTable.Rows.Count == 0)
                    {
                        stateCheck = false;
                    }
                    else
                    {
                        for (int i = 0; i <     dataTable.Rows.Count; i++)
                        {
                            if (dataTable.Rows[i]["UserID"].ToString()==item.UserID)
                            {
                                stateCheck = true;
                                break;
                            }
                        }

                    }
                    JC_AlarmNotificationPersonnelInfo.UserID = item.UserID;
                    JC_AlarmNotificationPersonnelInfo.UserCode = item.UserCode;
                    JC_AlarmNotificationPersonnelInfo.UserName = item.UserName;
                    JC_AlarmNotificationPersonnelInfo.IsCheck = stateCheck;

                    listResult.Add(JC_AlarmNotificationPersonnelInfo);
                   
                }
            }

            var jC_AnalyticalExpressionresponse = new BasicResponse<List<JC_AlarmNotificationPersonnelInfo>>();
            jC_AnalyticalExpressionresponse.Data = listResult;
            return jC_AnalyticalExpressionresponse;

        }
    }
}


