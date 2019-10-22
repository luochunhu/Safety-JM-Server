using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRealMessageRepository
    {
        /// <summary>
        /// 远程升级命令
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="send"></param>
        /// <param name="sjml"></param>
        /// <param name="sjstate"></param>
        void RemoteUpgradeCommand(string fzh, byte send, byte sjml, byte sjstate);

        /// <summary>
        /// 开始升级过程、或结束升级过程
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool RemoteUpdateStrtOrStop(int type);

        //TODO:目前没有弄懂这个方法的含义
        DataTable RemoteGetShowTb(string fzh);

        /// <summary>
        /// 读取自定义编排测点号
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        DataTable GetCustomPagePoint(int page);

        /// <summary>
        /// 读取配置信息到config表中
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        string ReadConfig(string keyName);

        /// <summary>
        /// 修改报警措施
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="message"></param>
        void UpdateAlarmStep(string id, string tableName, string message);

        /// <summary>
        ///根据时间获取测点号 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable GetBXPoint(DateTime time);

        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        bool SaveCustomPagePoints(int page, DataTable dt);

        /// <summary>
        /// 根据测点号获取历史维保记录
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        DataTable GetMaintenanceHistoryByPointId(long pointId);

        /// <summary>
        /// 获取手动控制
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        DataTable GetHandControlByPoint(string pointId);

        /// <summary>
        /// 根据Id获取报警数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Jc_BModel GetAlarmInfoById(string tableName, string id);
    }
}
