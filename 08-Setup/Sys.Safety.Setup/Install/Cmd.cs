using System;
using System.Diagnostics;

namespace Sys.Safety.Setup.Install
{
    public class Cmd
    {
        public static bool bOK = true;
        public static string Err;
        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        /// <param name="workingDirectory">要启动的进程的目录</param>
        /// <param name="command">要执行的命令</param>
        public static void StartCmd(String workingDirectory, String command)
        {
            bOK = true; 
            Err = "";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            bool b = p.Start();        
            p.StandardInput.WriteLine(command);
            p.BeginErrorReadLine();          
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();           
            p.Close();
        }
        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {

        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null  && e.Data.ToLower().Contains("error") && !e.Data.Contains("password "))
            {
                bOK = false;
                Err = e.Data;
                Trace.WriteLine(e.Data);
                (sender as Process).CancelErrorRead();
            }
        }
    }
}
