using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Uninstall
{
    public class Uninstaller2 : Uninstaller
    {
        public override void AfterLoad()
        {
            foreach (var item in SelectedItems)
            {
                try
                {
                    item.Uninstalling = true;
                    UninstallSoftware(item);
                    Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(string.Format("{0}\\{1}", Configuration.ROOT, item.RegistryKey));
                    if (item.ServiceName == "BasicHAService")//删除热切自动启动项
                    {
                        RunWhenStart(false, item.ServiceName);
                    }
                    item.IsUninstalled = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    item.Uninstalling = false;
                }
            }

            if (!UninstallModel.UninstallItems.Exists(q => q.IsUninstalled == false))
            {
                string menuRootFolder = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + UninstallModel.SystemName;
                if (Directory.Exists(menuRootFolder))
                    Directory.Delete(menuRootFolder);
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(Configuration.ROOT);

                DirectoryInfo installFolderInfo = new DirectoryInfo(UninstallModel.InstallFolder);
                if (installFolderInfo.Exists)
                {
                    bool isDeleted = false;
                    int tryCount = 0;
                    while (!isDeleted)
                    {
                        try
                        {
                            if (tryCount > 5)
                                break;
                            installFolderInfo.Delete(true);
                            isDeleted = true;
                        }
                        catch
                        {
                            tryCount++;
                            System.Threading.Thread.Sleep(3000);
                        }
                    }
                    try
                    {
                        installFolderInfo.Delete(true);//删除安装目录
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                    }
                    if (installFolderInfo.Parent.Name.ToLower() == "zhzsoft")
                    {
                        try
                        {
                            installFolderInfo.Parent.Delete(false);//若zhzsoft目录为空则删除zhzsoft目录
                        }
                        catch (Exception ex){
                            var message = ex.Message;
                        }
                    }
                }
            }
        }
        private void UninstallSoftware(UninstallItem uninstallItem)
        {
            //删除快捷方式
            string desktopIcon = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + uninstallItem.ApplicationName + ".lnk";
            string startMenuIcon = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\" + UninstallModel.SystemName + "\\" + uninstallItem.ApplicationName + ".lnk";
            if (File.Exists(desktopIcon))
            {
                File.Delete(desktopIcon);
            }
            if (File.Exists(startMenuIcon))
            {
                File.Delete(startMenuIcon);
            }
            if (uninstallItem.RunType == "service")
            {
                //卸载服务
                UninstallService(uninstallItem.File, uninstallItem.ServiceName);
            }
            //删除文件夹及文件
            FileInfo fi = new FileInfo(uninstallItem.File);
            if (fi.Exists)
            {
                //服务卸载后可能还有一些资源在占用， 这里尝试几次之后如果还是不能删除就不删除了。
                bool isDeleted = false;
                int tryCount = 0;
                while (!isDeleted)
                {
                    try
                    {
                        if (tryCount > 5)
                            break;
                        fi.Directory.Delete(true);
                        isDeleted = true;
                    }
                    catch
                    {
                        tryCount++;
                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }
        }
    }
}
