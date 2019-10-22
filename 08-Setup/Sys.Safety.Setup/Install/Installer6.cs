using Basic.Framework.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Setup.Install
{
    public class Installer6 : Installer
    {
        public override void Load()
        {
            base.Load();
            ConfigGroup serverConfig = ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "server");
            ConfigGroup activeConfig = ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "active");
            ConfigGroup standbyConfig = ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "standby");
            if(activeConfig == null)
            {
                activeConfig = new ConfigGroup();
                activeConfig.Key = "active";
                activeConfig.Port = 3306;
                if (serverConfig != null)
                    activeConfig.ConfigItems.AddRange(Basic.Framework.Common.ObjectConverter.DeepCopy(serverConfig.ConfigItems));
                {
                    activeConfig.ConfigItems.Add(new Config()
                    {
                        Description = "数据库IP",
                        Key = "dbIp",
                        Metadata = "Server",
                        Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("activeDbIp")
                    });
                    activeConfig.ConfigItems.Add(new Config()
                    {
                        Description = "数据库用户名",
                        Key = "dbUserName",
                        Metadata = "User Id",
                        Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("activeDbUserName")
                    });
                    activeConfig.ConfigItems.Add(new Config()
                    {
                        Description = "数据库密码",
                        Key = "dbPassword",
                        Metadata = "Password",
                        Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("activeDbPassword")
                    });
                    activeConfig.ConfigItems.Add(new Config()
                    {
                        Description = "数据库名",
                        Key = "dbName",
                        Metadata = "Database",
                        Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("activeDbName")
                    });
                }
                ConfigModel.ConfigGroup.Add(activeConfig);
            }
            if(standbyConfig == null)
            {
                standbyConfig = new ConfigGroup();
                standbyConfig.Key = "standby";
                standbyConfig.Port = 3306;
                standbyConfig.ConfigItems.Add(new Config()
                {
                    Description = "数据库IP",
                    Key = "dbIp",
                    Metadata = "Server",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("standbyDbIp")
                });
                standbyConfig.ConfigItems.Add(new Config()
                {
                    Description = "数据库用户名",
                    Key = "dbUserName",
                    Metadata = "User Id",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("standbyDbUserName")
                });
                standbyConfig.ConfigItems.Add(new Config()
                {
                    Description = "数据库密码",
                    Key = "dbPassword",
                    Metadata = "Password",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("standbyDbPassword")
                });
                standbyConfig.ConfigItems.Add(new Config()
                {
                    Description = "数据库名",
                    Key = "dbName",
                    Metadata = "Database",
                    Value = Basic.Framework.Common.AppConfigHelper.GetAppSetting("standbyDbName")
                });
                ConfigModel.ConfigGroup.Add(standbyConfig);
            }
        }
        public override void BeforeNext()
        {
            base.BeforeNext();
            //链接字符串
            ConfigGroup activeConfig = ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "active");
            ConfigGroup standbyConfig = ConfigModel.ConfigGroup.FirstOrDefault(q => q.Key == "standby");
            string activeDbIp = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
            string activeDbUserName = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
            string activeDbPassword = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
            string activeDbName = activeConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;
            string standbyDbIp = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbIp").Value;
            string standbyDbUserName = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbUserName").Value;
            string standbyDbPassword = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbPassword").Value;
            string standbyDbName = standbyConfig.ConfigItems.FirstOrDefault(q => q.Key == "dbName").Value;
            string activeConnectionString = string.Format("Server={0};User ID={1};Password={2};", activeDbIp, activeDbUserName, activeDbPassword);
            string standbyConnectionString = string.Format("Server={0};User ID={1};Password={2};", standbyDbIp, standbyDbUserName, standbyDbPassword);

            
            //获取不重复的server_id
            string activeServerId = string.Empty;
            string standbyServerId = string.Empty;
            DataTable dtActive = MySqlHelper.ExecuteDataTable(activeConnectionString, "show variables like 'server_id';");
            DataTable dtStandby = MySqlHelper.ExecuteDataTable(standbyConnectionString, "show variables like 'server_id';");
            activeServerId = dtActive.Rows[0]["Value"].ToString();
            standbyServerId = dtStandby.Rows[0]["Value"].ToString();
            if(standbyServerId == activeServerId)
            {
                activeServerId = (Convert.ToInt32(activeServerId) + 1).ToString();
            }


            //修改MYSQL配置文件
            object datadir = MySqlHelper.ExecuteScalar(activeConnectionString, "select @@datadir");
            if (datadir != null && datadir.ToString() != "" && System.IO.Directory.Exists(datadir.ToString()))
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(datadir.ToString());
                string iniFilePath = directoryInfo.Parent.FullName + "\\my.ini";
                if (System.IO.File.Exists(iniFilePath))
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(iniFilePath))
                    {
                        sw.Write(sw.NewLine);
                        sw.WriteLine("log_bin=mysql-bin");
                        sw.WriteLine(string.Format("server_id={0}", activeServerId));
                        sw.WriteLine(string.Format("replicate_do_db={0}", activeDbName));
                        sw.WriteLine("sync_binlog=1");
                        sw.WriteLine("log_slave_updates=1");
                        sw.WriteLine("relay-log=relay-bin");
                        sw.WriteLine("slave_skip_errors=all");
                        sw.WriteLine("read-only=0");
                        sw.WriteLine("slave_net_timeout=10");
                        sw.Flush();
                        sw.Close();
                    }
                }else
                {
                    throw new Exception("没有找到MySQL配置文件:my.ini");
                }
            }

            //重启MYSQL服务
            System.ServiceProcess.ServiceController[] services = System.ServiceProcess.ServiceController.GetServices();
            System.ServiceProcess.ServiceController service = services.FirstOrDefault(q => q.ServiceName.ToLower() == "mysql56");
            if (service != null && service.CanStop)
            {
                service.Stop();
                service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped);
                service.Start();
                service.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running);
            }

            string syncUserName = Basic.Framework.Common.AppConfigHelper.GetAppSetting("syncUserName");
            string syncPassword = Basic.Framework.Common.AppConfigHelper.GetAppSetting("syncPassword");
            //链接主机和备机分别创建同步账号
            MySqlHelper.ExecuteNonQuery(activeConnectionString, string.Format("GRANT replication SLAVE, replication client on *.* to '{0}'@'%' IDENTIFIED by '{1}';", syncUserName, syncPassword));
            MySqlHelper.ExecuteNonQuery(standbyConnectionString, string.Format("GRANT replication SLAVE, replication client on *.* to '{0}'@'%' IDENTIFIED by '{1}';", syncUserName, syncPassword));

            //链接备机查询show master status（File, Position)
            DataTable dtStandbyStatus = MySqlHelper.ExecuteDataTable(standbyConnectionString, "show master status;");
            string file = dtStandbyStatus.Rows[0]["File"].ToString();
            string position = dtStandbyStatus.Rows[0]["Position"].ToString();


            //链接主机开启复制功能
            StringBuilder replicationString = new StringBuilder();
            replicationString.Append("stop slave;");
            replicationString.Append(string.Format("change master to master_host='{0}',master_user='{1}',master_password='{2}', master_log_file='{3}',master_log_pos={4};", standbyDbIp, syncUserName, syncPassword, file, position));
            replicationString.Append("start slave;");
            MySqlHelper.ExecuteNonQuery(activeConnectionString, replicationString.ToString());

            int waitStepMS = 0;
            bool isSuccessful = false;
            while(waitStepMS <= 10000)
            {
                //检查复制是否已正常运行(show slave status(Slave_IO_Running,Slave_SQL_Running))
                DataTable dtSlaveStatus = MySqlHelper.ExecuteDataTable(activeConnectionString, "show slave status;");
                if (dtSlaveStatus.Rows[0]["Slave_IO_Running"].ToString() != "Yes" || dtSlaveStatus.Rows[0]["Slave_SQL_Running"].ToString() != "Yes")
                {
                    waitStepMS += 2000;
                    System.Threading.Thread.Sleep(2000);
                }else
                {
                    isSuccessful = true;
                    break;
                }
            }
            if(!isSuccessful)
            {
                throw new Exception("开启复制功能失败! 请重试.");
            }
        }
    }
}
