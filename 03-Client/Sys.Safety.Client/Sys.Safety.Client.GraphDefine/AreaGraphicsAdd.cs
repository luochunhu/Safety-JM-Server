using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using Basic.Framework.Logging;
using Sys.Safety.DataContract;

namespace Sys.Safety.Client.GraphDefine
{
    public partial class AreaGraphicsAdd : DevExpress.XtraEditors.XtraForm
    {

        GraphicOperations GraphOpt = new GraphicOperations();
        /// <summary>
        /// 系统默认图形id
        /// </summary>
        private string SysetemDefaultGraphicsId { get; set; }

        /// <summary>
        /// 应急联动图形id
        /// </summary>
        private string EmergencyLinkageGraphicsId { get; set; }
        public AreaGraphicsAdd(GraphicOperations _graphOpt)
        {
            GraphOpt = _graphOpt;
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 图形选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (opdlg.ShowDialog() == DialogResult.OK)
                {
                    textFileName.Text = opdlg.FileName;
                    if (textGraphName.Text.Length < 1)
                    {
                        textGraphName.Text = opdlg.FileName.Substring(opdlg.FileName.LastIndexOf("\\") + 1,
                            opdlg.FileName.Length - opdlg.FileName.LastIndexOf("\\") - 1);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsAdd_simpleButton1_Click" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GraphOpt.IsTopologyInit = false;

            string suffixName = "",//新图形后缀名
                SGraphFileName = "",//原图形路径及图形名称
                GraphFileName = "",//新图形名称
                distFileName = "";//新图形路径及图形名称
            try
            {
                if (textGraphName.Text.Length < 1)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请输入图形文件名称!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (comboBoxType.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择图形类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (comboBoxType.SelectedItem.ToString() == "动态图")
                {
                    if (textFileName.Text.Length < 1)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("请选择图形文件底图!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                //if (comboBoxType.SelectedItem.ToString() == "SVG组态图")
                //{
                //    if (textSCGFileName.Text.Length < 1)
                //    {
                //        DevExpress.XtraEditors.XtraMessageBox.Show("请输入本地目录SVG底图文件名称!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //}
                short Type = 0;
                if (comboBoxType.SelectedItem.ToString() == "拓扑图")
                {
                    Type = 1;
                }
                if (comboBoxType.SelectedItem.ToString() == "SVG组态图")
                {
                    Type = 3;
                }

                suffixName = textFileName.Text.Substring(textFileName.Text.LastIndexOf(".") + 1, textFileName.Text.Length - textFileName.Text.LastIndexOf(".") - 1);
                SGraphFileName = Application.StartupPath + "\\mx\\dwg\\" + textGraphName.Text;
                if (comboBoxType.SelectedItem.ToString() == "动态图")
                {
                    if (textGraphName.Text.Contains("."))
                    {
                        GraphFileName = textGraphName.Text.Substring(0, textGraphName.Text.LastIndexOf('.')) + "." + suffixName;
                    }
                    else//未输入后缀名时，直接用输入的名称+后缀名
                    {
                        GraphFileName = textGraphName.Text + "." + suffixName;
                    }
                }
                else
                {
                    GraphFileName = textGraphName.Text;
                }
                distFileName = Application.StartupPath + "\\mx\\dwg\\" + GraphFileName;

                //图形保存到数据库中 
                if (this.Text == "新建图形")
                {
                    //修改应急联动图形
                    if (!string.IsNullOrWhiteSpace(this.EmergencyLinkageGraphicsId))
                    {
                        GraphOpt.UpdateEmergencyLinkageGraphicsRequest(this.EmergencyLinkageGraphicsId);
                    }
                    if (Type == 0)
                    {
                        //查找目录下是否具有同名文件
                        if (File.Exists(distFileName))
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("当前图形已存在，请进行编辑操作!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        string bz3 = "0", bz4 = "0";
                        if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "通风系统默认图形")
                        {
                            bz3 = "1";
                            bz4 = "1";//通风系统图与应急联动图同时选择  20171221
                            if (!string.IsNullOrWhiteSpace(this.SysetemDefaultGraphicsId))
                            {
                                GraphOpt.UpdateSystemDefaultGraphics("0", this.SysetemDefaultGraphicsId);
                            }

                        }
                        else if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "拓扑定义默认图形")
                        {
                            bz3 = "2";
                            if (!string.IsNullOrWhiteSpace(this.SysetemDefaultGraphicsId))
                            {
                                GraphOpt.UpdateSystemDefaultGraphics("0", this.SysetemDefaultGraphicsId);
                            }
                        }

                        GraphOpt.GraphInsert(GraphFileName, Type, bz3, bz4, File.ReadAllBytes(textFileName.Text));
                    }
                    else
                    {
                        //查找数据库中是否有相同图形
                        GraphicsbaseinfInfo GraphicsbaseinfDTO_ = GraphOpt.getGraphicDto(GraphFileName);
                        if (GraphicsbaseinfDTO_ != null)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("当前图形已存在，请进行编辑操作!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            if (comboBoxType.SelectedItem.ToString() == "SVG组态图")
                            {
                                string bz4 = "0";
                                //if (this.checkEdit3.Checked == true)
                                //{
                                //    bz4 = "1";
                                //}
                                //else if (this.checkEdit4.Checked == true)
                                //{
                                //    bz4 = "0";
                                //}
                                //GraphOpt.GraphInsert(GraphFileName, Type, bz4, null, "SVGLoad.html", textSCGFileName.Text);
                            }
                            else
                            {
                                string bz3 = "0", bz4 = "0";
                                if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "通风系统默认图形")
                                {
                                    if (!string.IsNullOrWhiteSpace(this.SysetemDefaultGraphicsId))
                                    {
                                        GraphOpt.UpdateSystemDefaultGraphics("0", this.SysetemDefaultGraphicsId);
                                    }
                                    bz3 = "1";
                                    bz4 = "1";//通风系统图与应急联动图同时选择  20171221
                                }
                                else if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "拓扑定义默认图形")
                                {
                                    if (!string.IsNullOrWhiteSpace(this.SysetemDefaultGraphicsId))
                                    {
                                        GraphOpt.UpdateSystemDefaultGraphics("0", this.SysetemDefaultGraphicsId);
                                    }
                                    bz3 = "2";
                                }
                                //if (this.checkEdit3.Checked == true)
                                //{
                                //    bz4 = "1";
                                //}
                                //else if (this.checkEdit4.Checked == true)
                                //{
                                //    bz4 = "0";
                                //}
                                GraphOpt.IsTopologyInit = true;
                                GraphOpt.GraphInsert(GraphFileName, Type, bz3, bz4, null);
                            }
                        }
                    }
                }
                else if (this.Text == "图形编辑")
                {
                    if (SGraphFileName != textFileName.Text)
                    {
                        //删除原来的图形
                        if (File.Exists(SGraphFileName))
                        {
                            File.Delete(SGraphFileName);
                        }
                    }
                    if (comboBoxType.SelectedItem.ToString() == "SVG组态图")
                    {
                        GraphOpt.GraphUpdate(textGraphName.Text, GraphFileName, Type, File.ReadAllBytes(textFileName.Text), "SVGLoad.html", textGraphName.Text);
                    }
                    else
                    {
                        string bz3 = "0", bz4 = "0";
                        if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "通风系统默认图形")
                        {
                            bz3 = "1";
                            bz4 = "1";//通风系统图与应急联动图同时选择  20171221
                        }
                        else if (this.checkEdit2.Checked == true && this.checkEdit2.Text == "拓扑定义默认图形")
                        {
                            bz3 = "2";
                        }
                        //if (this.checkEdit3.Checked == true)
                        //{
                        //    bz4 = "1";
                        //}
                        //else if (this.checkEdit4.Checked == true)
                        //{
                        //    bz4 = "0";
                        //}

                        if (Type == 0)
                        {
                            //更新动态图形
                            GraphOpt.GraphUpdate(textGraphName.Text, GraphFileName, Type, bz3, bz4, File.ReadAllBytes(textFileName.Text));
                        }
                        else if (Type == 1)
                        {
                            //修改拓扑图形
                            GraphOpt.GraphUpdate(textGraphName.Text, GraphFileName, Type, bz3, bz4, null);
                        }
                    }
                }
                if (Type == 0)//只有动态图才进行底图保存
                {
                    if (SGraphFileName != textFileName.Text)
                    {
                        //保存本地
                        File.Copy(textFileName.Text, distFileName);
                    }
                }

                GraphOpt.GraphNameNow = GraphFileName;

                DevExpress.XtraEditors.XtraMessageBox.Show("系统默认图形上传成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsAdd_simpleButton2_Click" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "动态图")
            {
                //textFileName.Enabled = true;               
                //simpleButton1.Enabled = true;

                //textSCGFileName.Enabled = false;
                //ButtonSelSvgFile.Enabled = false;
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                this.checkEdit2.Properties.Caption = "通风系统默认图形";
                this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (comboBoxType.SelectedItem.ToString() == "拓扑图")
            {
                //textFileName.Enabled = false;
                //simpleButton1.Enabled = false;

                //textSCGFileName.Enabled = false;
                //ButtonSelSvgFile.Enabled = false;
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                this.checkEdit2.Properties.Caption = "拓扑定义默认图形";
                this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (comboBoxType.SelectedItem.ToString() == "SVG组态图")
            {
                //textFileName.Enabled = false;
                //simpleButton1.Enabled = false;

                //textSCGFileName.Enabled = true;
                //ButtonSelSvgFile.Enabled = true;
                layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                //layoutControlItem7.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                //textSCGFileName.Text = "m.svg";

                this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItem9.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }
        private void GraphicsAdd_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Text == "图形编辑")
                {
                    this.comboBoxType.Enabled = false;
                    this.textGraphName.Enabled = false;

                    if (GraphOpt.getGraphicNowType() == 0)
                    {
                        comboBoxType.SelectedItem = "动态图";
                    }
                    else if (GraphOpt.getGraphicNowType() == 1)
                    {
                        comboBoxType.SelectedItem = "拓扑图";
                    }
                    else if (GraphOpt.getGraphicNowType() == 3)
                    {
                        comboBoxType.SelectedItem = "SVG组态图";
                    }

                    var graphicOperations = new GraphicOperations();
                    var graphicInfo = graphicOperations.getGraphicDto(GraphOpt.GraphNameNow);
                    if (graphicInfo != null)
                    {
                        if (graphicInfo.Bz3 == "0")
                        {
                            checkEdit1.Checked = true;
                        }
                        else if (graphicInfo.Bz3 == "1" || graphicInfo.Bz3 == "2")
                        {
                            checkEdit2.Checked = true;
                        }
                        if (graphicInfo.Bz4 == "1")
                        {
                            checkEdit3.Checked = true;
                        }
                        else if (graphicInfo.Bz4 == "0")
                        {
                            checkEdit4.Checked = true;
                        }
                    }

                    string graphFileName = "", graphNameNow = GraphOpt.GraphNameNow;
                    this.textGraphName.Text = graphNameNow;
                    string suffixName = graphNameNow.Substring(graphNameNow.LastIndexOf(".") + 1, graphNameNow.Length - graphNameNow.LastIndexOf(".") - 1);
                    if (comboBoxType.SelectedItem.ToString() == "动态图")
                    {
                        if (textGraphName.Text.Contains("."))
                        {
                            graphFileName = graphNameNow.Substring(0, graphNameNow.LastIndexOf('.')) + "." + suffixName;
                        }
                        else//未输入后缀名时，直接用输入的名称+后缀名
                        {
                            graphFileName = graphNameNow + "." + suffixName;
                        }
                    }
                    else
                    {
                        graphFileName = graphNameNow;
                    }
                    this.textFileName.Text = Application.StartupPath + "\\mx\\dwg\\" + graphFileName;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicsAdd_GraphicsAdd_Load" + ex.Message + ex.StackTrace);
            }
        }
        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Text == "新建图形")
            {
                //判断是否选择了图形类型
                if (comboBoxType.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择对应的图形类型", "提示");
                    return;
                }
                var graphicsInfo = GraphOpt.GetEmergencyLinkageGraphics();
                if (graphicsInfo != null && !string.IsNullOrWhiteSpace(graphicsInfo.GraphId))
                {
                    if (this.checkEdit4.Checked == true)
                    {
                        return;
                    }
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("已存在应急联动图形，确定要把当前图形设置应急联动图形吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.EmergencyLinkageGraphicsId = graphicsInfo.GraphId;
                        this.checkEdit3.Checked = true;
                    }
                    else
                    {
                        this.checkEdit4.Checked = true;
                    }
                }
            }
        }

        private void checkEdit2_Click(object sender, EventArgs e)
        {
            if (this.Text == "新建图形")
            {
                string message = "";
                GraphicsbaseinfInfo graphicsInfo = null;
                //判断是否选择了图形类型
                if (comboBoxType.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("请选择对应的图形类型", "提示");
                    return;
                }
                if (comboBoxType.SelectedItem.ToString() == "动态图")
                {
                    message = "已存在通风系统默认图形，确定要把当前图形设置为通风系统默认图形吗?";
                    graphicsInfo = GraphOpt.GetSystemtDefaultGraphics(0);
                }
                else if (comboBoxType.SelectedItem.ToString() == "拓扑图")
                {
                    message = "已存在拓扑定义默认图形，确定要把当前图形设置为拓扑定义默认图形吗?";
                    graphicsInfo = GraphOpt.GetSystemtDefaultGraphics(1);
                }
                if (graphicsInfo != null && !string.IsNullOrWhiteSpace(graphicsInfo.GraphId))
                {
                    if (this.checkEdit2.Checked == true)
                    {
                        return;
                    }
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        this.SysetemDefaultGraphicsId = graphicsInfo.GraphId;
                        this.EmergencyLinkageGraphicsId = graphicsInfo.GraphId;
                        this.checkEdit2.Checked = true;
                    }
                    else
                    {
                        this.checkEdit1.Checked = true;
                    }
                }
            }
        }
    }
}
