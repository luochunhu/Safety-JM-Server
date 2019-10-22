using Sys.Safety.ServiceContract.DataToDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.DataToDb;
using System.Threading;
using System.IO;
using Basic.Framework.Common;
using System.Data;
using Basic.Framework.Logging;
using System.Globalization;
using Basic.Framework.Service;
using Basic.Framework.Data;
using Sys.Safety.Processing.DataToDb;

namespace Sys.Safety.Services.DataToDb
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-26
    /// 描述:数据处理入库业务基类
    /// 修改记录
    /// 2017-05-26
    /// </summary>
    public abstract class InsertToDbService<T> : IInsertToDbService<T> where T : BasicInfo
    {
        #region 外部接口

        /// <summary>
        /// 添加数据至列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public abstract BasicResponse AddItem(DataToDbAddRequest<T> dataToDbRequest);

        /// <summary>
        /// 批量添加数据至列表
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public abstract BasicResponse AddItems(DataToDbBatchAddRequest<T> dataToDbRequest);

        /// <summary>
        /// 启动线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public abstract BasicResponse Start(DataToDbStartRequest dataToDbRequest);

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <param name="dataToDbRequest"></param>
        /// <returns></returns>
        public abstract BasicResponse Stop(DataToDbStopRequest dataToDbRequest);
        /// <summary>
        /// 获取队列积压数量  20170717
        /// </summary>
        /// <returns></returns>
        public abstract BasicResponse<int> GetQueueBacklog();

        /// <summary>
        /// 获取总的数据量
        /// </summary>
        /// <returns></returns>
        public abstract BasicResponse<long> GetTotalCount();
        #endregion
    }
}

