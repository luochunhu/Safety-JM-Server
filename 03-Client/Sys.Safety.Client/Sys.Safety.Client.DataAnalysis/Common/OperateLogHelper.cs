using EnvDTE;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.Request.Operatelog;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sys.Safety.Client.DataAnalysis.Common
{
    /// <summary>
    /// 系统操作日志帮助类
    /// </summary>
    public class OperateLogHelper
    {
        static IOperatelogService operatelogService = ServiceFactory.Create<IOperatelogService>();
      

        /// <summary>
        /// 构造函数
        /// </summary>
        static OperateLogHelper()
        {
          
        }
       
  
        /// <summary>
        /// 增加操作日志
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <param name="OperationContent">日志内容</param>
        /// <param name="Remark">备注信息</param>
        /// <returns></returns>
        public static void InsertOperateLog(int type, string OperationContent, string Remark)
        {
            try
            {
                OperatelogInfo opdto = new OperatelogInfo();
                opdto.OperateLogID = IdHelper.CreateLongId().ToString();
                //opdto.UserName = ClientContext.Current.ClientItem.UserName;
                ClientItem clientItem = new ClientItem();
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                {
                    clientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                }
                opdto.UserName = clientItem.UserName;
                opdto.LoginIP = Basic.Framework.Common.HardwareHelper.GetIPAddress();
                opdto.Type = type;
                opdto.OperationContent = OperationContent;
                opdto.CreateTime = DateTime.Now;
                opdto.Remark = Remark;
                //InsertOperateLog(opdto);
                operatelogService.AddOperatelog(new OperatelogAddRequest() { OperatelogInfo = opdto });
            }
            catch (Exception ex)
            {
                LogHelper.Error("OperateLogHelper-InsertOperateLog" + ex.Message + ex.StackTrace);
            }
        }
       
    }
}
