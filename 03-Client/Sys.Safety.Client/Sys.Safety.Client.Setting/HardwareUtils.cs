using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sys.Safety.Client.Setting
{


    public class HardwareUtils
    {
        ///  <summary> 
        /// 获取指定驱动器的空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            long totalSize = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize / (1024 * 1024 * 1024);
                }
            }
            return totalSize;
        }

        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                }
            }
            return freeSpace;
        }


        public static HardDiskInfo GetDiskInfo(string diskName)
        {
            HardDiskInfo entity = null;

            try
            {
                string diskFullName = diskName + ":\\";
                System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo drive in drives)
                {
                    if (drive.Name.ToLower() == diskFullName.ToLower())
                    {
                        entity = new HardDiskInfo();
                        entity.DiskName = diskName;
                        entity.TotalSize = drive.TotalSize / (1024 * 1024 * 1024);
                        entity.TotalFreeSize = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                        entity.TotalUsageSize = entity.TotalSize - entity.TotalFreeSize;
                        entity.TotalUsageRate = Convert.ToInt32(Math.Round((decimal)entity.TotalUsageSize / (decimal)entity.TotalSize, 2) * 100);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
          
            return entity;
        }

        public static PorcessInfo GetProcessInfo(string processName)
        {
            PorcessInfo result = null;

            var processList = Process.GetProcessesByName(processName);
            if (processList.Length <= 0)
            {
                return result;
            }

            result = new PorcessInfo();
            Process cur = processList[0];

            PerformanceCounter curpcp = new PerformanceCounter("Process", "Working Set - Private", cur.ProcessName);
            PerformanceCounter curtime = new PerformanceCounter("Process", "% Processor Time", cur.ProcessName);

            const int KB_DIV = 1024;
            const int MB_DIV = 1024 * 1024;
            const int GB_DIV = 1024 * 1024 * 1024;

           // Console.WriteLine("{0}:{1}  {2:N}KB CPU使用率：{3}%", cur.ProcessName, "私有工作集    ", curpcp.NextValue() / 1024, curtime.NextValue() / Environment.ProcessorCount);
            result.ProcessName = processName;
            result.MemoryUsageSize = Math.Round((decimal)curpcp.NextValue() / MB_DIV, 2);
            result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);
            //added by  20170318 有时获取CPU为0，这里增加5次循环，尽量让CPU使用率不为0
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                if (result.CpuUsageRate <= 0)
                {
                    result.CpuUsageRate = Math.Round((decimal)curtime.NextValue() / Environment.ProcessorCount, 2);
                }
                else
                {
                    break;
                }
            }
            return result;
        }
    }
}
