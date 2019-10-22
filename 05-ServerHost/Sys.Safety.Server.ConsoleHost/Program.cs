using Basic.Framework.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sys.Safety.Server.ConsoleHost
{
    class Program
    {
        static string _serviceName = "Sys.Safety.Server.ConsoleHost";
        static bool isHAClose = false;
        public static byte[] ConvertIpToAscllByte(string IP, byte[] data, int offSet)
        {
            byte[] ipBytes = new byte[IP.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                ipBytes[i] = Convert.ToByte(IP[i]);
            }
            Buffer.BlockCopy(ipBytes, 0, data, offSet, ipBytes.Length);

            return data;
        }
        static void Main(string[] args)
        {
            //判断互斥
            Process[] process = Process.GetProcessesByName(_serviceName);
            if (process.Length > 1)
            {
                Environment.Exit(0);
            }
            byte[] data1 = new byte[30];
            ConvertIpToAscllByte("192.168.2.17", data1, 0);
            Task.Run(() =>
            {
                Sys.Safety.CoreService.KJ73NService.Start();

                //启动双机热备状态检测
                StartHAService();
            });

            Console.WriteLine("输入 exit 退出程序");

            while (Console.ReadLine().ToLower() != "exit" )
            {
                Console.WriteLine("输入 exit 退出程序");
            }

            Sys.Safety.CoreService.KJ73NService.Stop();

        }

        /// <summary>
        /// 开始启动HA检测
        /// </summary>
        private static void StartHAService()
        {
            Thread haThread = new Thread(new ThreadStart(MonitorHAService));
            haThread.IsBackground = true;
            haThread.Start();

            //LogHelper.SystemInfo("热切检测服务启动");
        }
        /// <summary>
        /// 监测双机热备退出
        /// </summary>
        private static void MonitorHAService()
        {
            LogHelper.Debug("热备热切状态检测服务DoWork()开始执行");
            string haConfigPath = "";
            try
            {
                haConfigPath = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetString("HAPath", @"HA\BackConfig.ini");
                if (!haConfigPath.StartsWith(@"\"))
                {
                    haConfigPath = @"\" + haConfigPath;
                }

                var dr = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                if (dr != null && dr.Parent != null)
                {
                    //获取双机热备的完整目录
                    haConfigPath = dr.Parent.FullName + haConfigPath;
                }
                // LogHelper.SystemInfo("haConfigPath：" + haConfigPath);

                if (!File.Exists(haConfigPath))
                {
                    LogHelper.Debug("HA热备配置文件路径不存，haConfigPath：" + haConfigPath);
                    return;
                }

                LogHelper.Debug("热备热切状态检测服务开始执行循环检测状态");
            }
            catch (Exception ex)
            {
                LogHelper.SystemInfo("Exception:" + ex.ToString());
                return;
            }

            while (true)
            {
                try
                {
                    #region 监测双机热备是否已置退出标记，如果已置，则退出程序
                    string flagIndex = "";

                    string[] tempArrStr = Basic.Framework.Common.IniConfigHelper.INIGetStringValue(haConfigPath, "Backupdb", "PFilePath", "").Split('|');

                    for (int i = 0; i < tempArrStr.Length; i++)
                    {
                        if (tempArrStr[i].ToString().ToLower().Contains(_serviceName.ToLower()))
                        {
                            flagIndex = (i + 1).ToString();
                            break;
                        }
                    }

                    if (Basic.Framework.Common.IniConfigHelper.INIGetStringValue(haConfigPath, "Backupdb", "ProgCloseFlag" + flagIndex, "") == "1")
                    {
                        LogHelper.Debug("检测到HA ProgCloseFlag=1，开始停止" + _serviceName + " ");

                        //停止核心服务
                        Sys.Safety.CoreService.KJ73NService.Stop();
                       

                        //重新回写标识
                        Basic.Framework.Common.IniConfigHelper.INIWriteValue(haConfigPath, "Backupdb", "ProgCloseFlag" + flagIndex, "2");

                        LogHelper.Debug("重写 HA ProgCloseFlag=2");


                        System.Environment.Exit(0);
                    }
                    #endregion
                }
                catch (Exception err)
                {
                    LogHelper.Error("监测双机热备退出异常:", err);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }

            LogHelper.SystemInfo("热备热切状态检测服务停止");
        }
    }
}
