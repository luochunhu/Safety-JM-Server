using DevExpress.XtraEditors;
using Basic.Framework.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.ClientFramework.View.Video
{
    /// <summary>
    /// 帮助视频页面
    /// </summary>
    public partial class frmHelpVideo : XtraForm
    {
        public frmHelpVideo()
        {
            InitializeComponent();
            InitHelpTree();
        }

        public frmHelpVideo(Dictionary<string, string> param)
        {
            InitializeComponent();
            InitHelpTree();

            try
            {
                string helpName = param["helpName"];

                if (!string.IsNullOrEmpty(helpName))
                {
                    for (int i = 0; i < helptree.Nodes.Count; i++)
                    {
                        if (helptree.Nodes[i].Name == helpName)
                        {
                            var imagepath = helptree.Nodes[i].Tag.ToString();
                            this.helepic.Image = Image.FromFile(imagepath);
                            this.helepic.Tag = imagepath;

                            helptree.SelectedNode = helptree.Nodes[i];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("加载引导图片出错：" + ex.Message);
            }
        }

        private void frmHelpVideo_Load(object sender, EventArgs e)
        {
            this.Width = 1156;
            this.Height = 690;
            this.StartPosition = FormStartPosition.CenterScreen;
        }


        public void InitHelpTree()
        {
            try
            {

                //string helpfilepath = System.Environment.CurrentDirectory + "\\Help";

                string helpfilepath = Application.StartupPath + "\\Help";

                LogHelper.Debug("帮助文件路径：" + helpfilepath);

                DirectoryInfo folder = new DirectoryInfo(helpfilepath);
                var directories = folder.GetDirectories();

                foreach (var item in directories)
                {
                    TreeNode parentnode = new TreeNode();
                    parentnode.Name = item.Name;
                    parentnode.Text = item.Name;

                    var childfolder = new DirectoryInfo(item.FullName);
                    var childdfiles = childfolder.GetFiles();

                    LogHelper.Debug(item.Name + " 文件个数 ：" + childdfiles.Count());

                    List<string> fielnames = new List<string>();
                    foreach (var childitem in childdfiles)
                    {

                        var filename = childitem.Name;
                        var filetype = filename.Substring(filename.IndexOf('-') + 1, filename.LastIndexOf('-') - filename.IndexOf('-') - 1);

                        if (!fielnames.Contains(filetype))
                        {
                            TreeNode childnode = new TreeNode();
                            childnode.Name = filetype;
                            childnode.Text = filetype;
                            childnode.Tag = childitem.FullName;

                            parentnode.Nodes.Add(childnode);

                            fielnames.Add(filetype);
                        }
                    }
                    parentnode.Tag = childdfiles[0].FullName;

                    this.helptree.Nodes.Add(parentnode);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("初始化系统引导节点错误：" + ex.Message);
            }
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            try
            {

                var imagepath = helepic.Tag == null ? string.Empty : helepic.Tag.ToString();
                if (!string.IsNullOrEmpty(imagepath))
                {
                    var imagenum = imagepath.Substring(imagepath.LastIndexOf('-') + 1, imagepath.LastIndexOf('.') - imagepath.LastIndexOf('-') - 1);
                    int imageint = Convert.ToInt32(imagenum);

                    if (imageint <= 1)
                    {
                        XtraMessageBox.Show("已到第一页！");
                    }
                    else
                    {
                        imageint -= 1;

                        DirectoryInfo directoryinfo = new DirectoryInfo(imagepath).Parent;
                        var files = directoryinfo.GetFiles();

                        foreach (var item in files)
                        {
                            var itempath = item.FullName;
                            var preimagenum = itempath.Substring(itempath.LastIndexOf('-') + 1, itempath.LastIndexOf('.') - itempath.LastIndexOf('-') - 1);
                            int preimageint = Convert.ToInt32(preimagenum);

                            if (preimageint == imageint)
                            {
                                this.helepic.Image = Image.FromFile(item.FullName);
                                this.helepic.Tag = item.FullName;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("系统引导上一页出错：" + ex.Message);
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            try
            {

                var imagepath = helepic.Tag == null ? string.Empty : helepic.Tag.ToString();
                if (!string.IsNullOrEmpty(imagepath))
                {

                    var imagenum = imagepath.Substring(imagepath.LastIndexOf('-') + 1, imagepath.LastIndexOf('.') - imagepath.LastIndexOf('-') - 1);
                    int imageint = Convert.ToInt32(imagenum);

                    imageint += 1;

                    DirectoryInfo directoryinfo = new DirectoryInfo(imagepath).Parent;
                    var files = directoryinfo.GetFiles();

                    string nextimagepath = string.Empty;
                    foreach (var item in files)
                    {
                        var itempath = item.FullName;
                        var nextimagenum = itempath.Substring(itempath.LastIndexOf('-') + 1, itempath.LastIndexOf('.') - itempath.LastIndexOf('-') - 1);
                        int nextimageint = Convert.ToInt32(nextimagenum);
                        if (nextimageint == imageint)
                        {
                            nextimagepath = item.FullName;
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(nextimagepath))
                    {
                        this.helepic.Image = Image.FromFile(nextimagepath);
                        this.helepic.Tag = nextimagepath;
                    }
                    else
                    {
                        XtraMessageBox.Show("已到最后一页！");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("系统引导下一页出错：" + ex.Message);
            }
        }

        private void helptree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                var imagepath = e.Node.Tag.ToString();
                this.helepic.Image = Image.FromFile(imagepath);
                this.helepic.Tag = imagepath;
            }
            catch (Exception ex)
            {
                LogHelper.Info("系统引导节点选择出错：" + ex.Message);
            }
        }
    }


}
