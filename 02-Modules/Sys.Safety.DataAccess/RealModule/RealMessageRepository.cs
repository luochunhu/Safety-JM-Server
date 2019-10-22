using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.Model;
using Basic.Framework.Data;
using Basic.Framework.Common;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public class RealMessageRepository : RepositoryBase<RealMessageModel>, IRealMessageRepository
    {
        public DataTable RemoteGetShowTb(string fzh)
        {
            //TODO：缓存模块
            return null;
        }

        /// <summary>
        /// 开始升级过程或结束升级过程
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool RemoteUpdateStrtOrStop(int type)
        {
            return false;
        }

        //读取远程升级相关配置
        public static void ReadRemoteConfig()
        {
            //TODO:缓存模块
        }

        /// <summary>
        /// 远程升级命令
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="send"></param>
        /// <param name="sjml"></param>
        /// <param name="sjstate"></param>
        public void RemoteUpgradeCommand(string fzh, byte send, byte sjml, byte sjstate)
        {
            //TODO：缓存模块
        }

        /// <summary>
        /// 读取自定义升级测点号
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public DataTable GetCustomPagePoint(int page)
        {
            DataTable dt = new DataTable();
            dt = base.QueryTable("global_RealModule_GetCustomPagePoint_byPage", page);
            if (dt == null)
            {
                dt = new DataTable();
            }
            dt.TableName = "point";
            if (dt.Columns.Count == 0)
            {
                dt.Columns.Add("point", typeof(string));
            }

            return dt;
        }

        /// <summary>
        ///  读取配置信息到config表中
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string ReadConfig(string keyName)
        {
            string strvalue = "";
            var dt = base.QueryTable("global_RealModule_readConfig_byKeyName", keyName);
            if (dt != null && dt.Rows.Count > 0)
            {
                strvalue = dt.Rows[0]["strvalue"].ToString();
            }

            return strvalue;
        }

        /// <summary>
        /// 修改报警措施
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tableName"></param>
        /// <param name="message"></param>
        public void UpdateAlarmStep(string id, string tableName, string message)
        {
            base.ExecuteNonQuery("global_RealModule_UpdateAlarmStep", tableName, message, id);
        }

        /// <summary>
        /// 根据时间获取测点
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetBXPoint(DateTime time)
        {
            return base.QueryTable("global_RealModule_GetPoint_Bytime", time);
        }


        /// <summary>
        /// 存储自定义测点
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public bool SaveCustomPagePoints(int page, DataTable dt)
        {
            bool flg = true;
            try
            {
                base.ExecuteNonQuery("global_RealModule_DeleteJc_show_byPage", page);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        base.ExecuteNonQuery("global_RealModule_InsertJc_show", Basic.Framework.Common.IdHelper.CreateLongId(), dt.Rows[i]["col"].ToString(), dt.Rows[i]["row"].ToString(), dt.Rows[i]["points"].ToString(), page);
                    }
                }
            }
            catch(Exception ex)
            {
                flg = false;
            }
            return flg;
        }

        /// <summary>
        /// 根据测点号获取历史维保记录
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public DataTable GetMaintenanceHistoryByPointId(long pointId)
        {
            return base.QueryTable("global_RealModule_MaintenanceHistory_PointId", pointId);
        }

        /// <summary>
        /// 获取手动控制
        /// </summary>
        /// <param name="pointId"></param>
        /// <returns></returns>
        public DataTable GetHandControlByPoint(string pointId)
        {
            return base.QueryTable("global_RealModule_GetHandControlData_ByPoint", pointId);
        }

        /// <summary>
        /// 根据Id获取报警数据
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Jc_BModel GetAlarmInfoById(string tableName, string id)
        {
            var jc_BModel = new Jc_BModel();
            var datatable = base.QueryTable("global_RealModule_GetAlarmData_ById", tableName, id);
            if (datatable != null)
            {
                var row = datatable.Rows[0];
                if (row != null)
                {
                    jc_BModel.ID = row["ID"].ToString();
                    jc_BModel.PointID = row["PointID"].ToString();
                    jc_BModel.Fzh = short.Parse(row["fzh"].ToString());
                    jc_BModel.Kh = short.Parse(row["kh"].ToString());
                    jc_BModel.Dzh = short.Parse(row["dzh"].ToString());
                    jc_BModel.Devid = row["devid"].ToString();
                    jc_BModel.Wzid = row["devid"].ToString();
                    jc_BModel.Point = row["point"].ToString();
                    jc_BModel.Type = short.Parse(row["type"].ToString());
                    jc_BModel.State = short.Parse(row["state"].ToString());
                    jc_BModel.Stime = DateTime.Parse(row["stime"].ToString());
                    jc_BModel.Etime = DateTime.Parse(row["etime"].ToString());
                    jc_BModel.Zdz = double.Parse(row["zdz"].ToString());
                    jc_BModel.Pjz = double.Parse(row["pjz"].ToString());
                    jc_BModel.Zdzs = DateTime.Parse(row["zdzs"].ToString());
                    jc_BModel.Cs = row["cs"].ToString();
                    jc_BModel.Kzk = row["kzk"].ToString();
                    jc_BModel.Kdid = row["kdid"].ToString();
                    jc_BModel.Isalarm = short.Parse(row["isalarm"].ToString());
                    jc_BModel.Remark = row["remark"].ToString();
                    jc_BModel.Pjz = double.Parse(row["Pjz"].ToString());
                    jc_BModel.Bz1 = row["Bz1"].ToString();
                    jc_BModel.Bz2 = row["Bz2"].ToString();
                    jc_BModel.Bz3 = row["Bz3"].ToString();
                    jc_BModel.Bz4 = row["Bz4"].ToString();
                    jc_BModel.Bz5 = row["Bz5"].ToString();
                    jc_BModel.Upflag = row["upflag"].ToString();
                }
            }

            return jc_BModel;
        }
    }
}
