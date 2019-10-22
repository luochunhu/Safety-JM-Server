using IWshRuntimeLibrary;
using System;

namespace Sys.Safety.Setup.Install
{
    public static class InstallHelper
    {
        public static void CreateShortcut(string filePath, string applicationName, string systemName)
        {
            //从COM中引用 Windows Script Host Object Model
            //using IWshRuntimeLibrary;
            WshShell shell = new WshShell();

            //创建桌面快捷方式
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + applicationName + ".lnk");
            shortcut.TargetPath = filePath;
            shortcut.WorkingDirectory = Environment.CurrentDirectory;
            shortcut.WindowStyle = 1;
            shortcut.Description = applicationName;
            shortcut.Save();

            //创建开始菜单快捷方式
            string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + systemName;
            if (!System.IO.Directory.Exists(startMenuPath))
            {
                System.IO.Directory.CreateDirectory(startMenuPath);
            }
            IWshShortcut shortcut1 = (IWshShortcut)shell.CreateShortcut(startMenuPath + "\\" + applicationName + ".lnk");
            shortcut1.TargetPath = filePath;
            shortcut1.WorkingDirectory = Environment.CurrentDirectory;
            shortcut1.WindowStyle = 1;
            shortcut1.Description = applicationName;
            shortcut1.Save();       
        }
    }
}
