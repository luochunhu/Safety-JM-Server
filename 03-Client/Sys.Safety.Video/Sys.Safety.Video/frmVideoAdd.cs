using DevExpress.XtraEditors;
using Basic.Framework.Common;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.Enums.Enums;
using Sys.Safety.Request.Def;
using Sys.Safety.Request.Graphicspointsinf;
using Sys.Safety.ServiceContract;
using Sys.Safety.Client.Graphic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sys.Safety.Video
{
    public partial class frmVideoAdd : XtraForm
    {
        private readonly IV_DefService _vdefService = ServiceFactory.Create<IV_DefService>();
        private readonly IGraphicspointsinfService _graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();

        /// <summary>
        /// 操作标识 0-添加；1-编辑
        /// </summary>
        private int operateflag = 0;

        /// <summary>
        /// 图形Id
        /// </summary>
        private string PointInGraphId;

        /// <summary>
        /// 视频测点信息
        /// </summary>
        private V_DefInfo vdefinfo;

        public frmVideoAdd()
        {
            InitializeComponent();
            operateflag = 0;
            vdefinfo = new V_DefInfo();

            GetVendorDataSource(0);
        }

        public frmVideoAdd(string defid)
        {
            InitializeComponent();
            operateflag = 1;
            this.Text = "编辑视频测点";

            if (!string.IsNullOrEmpty(defid))
            {
                var defgetRequest = new DefGetRequest();
                defgetRequest.Id = defid;
                var defgetResponse = _vdefService.GetDefById(defgetRequest);
                if (defgetResponse.IsSuccess)
                {
                    vdefinfo = defgetResponse.Data;

                    this.videoname.Text = vdefinfo.Devname;
                    this.videoip.Text = vdefinfo.IPAddress;
                    this.videoport.Text = vdefinfo.Port;
                    this.videochannel.Text = vdefinfo.Channel.ToString();
                    this.videousername.Text = vdefinfo.Username;
                    this.videopw.Text = vdefinfo.Password;

                    var graphicspoint = _graphicspointsinfService.GetGraphicspointsinfByPoint(new Sys.Safety.Request.Graphicspointsinf.GetGraphicspointsinfByPointRequest { PointId = vdefinfo.IPAddress }).Data;
                    this.txt_Coordinate.Text = graphicspoint == null ? string.Empty : graphicspoint.XCoordinate + "," + graphicspoint.YCoordinate;

                    GetVendorDataSource(vdefinfo.Vendor);
                }
            }
        }

        /// <summary>
        /// 拾取坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getzb_Click(object sender, EventArgs e)
        {
            PointCoordinatePickUp coordinateGraphDrawing = new PointCoordinatePickUp(txt_Coordinate.Text);
            coordinateGraphDrawing.ShowDialog();
            if (coordinateGraphDrawing.DialogResult == DialogResult.OK)
            {
                txt_Coordinate.Text = coordinateGraphDrawing.Jsonstr;
                vdefinfo.By3 = coordinateGraphDrawing.Jsonstr;
                if (!string.IsNullOrEmpty(coordinateGraphDrawing.AreaIdNow))
                    vdefinfo.AreaId = coordinateGraphDrawing.AreaIdNow;
                //PointAreaId = coordinateGraphDrawing.AreaIdNow;
                PointInGraphId = coordinateGraphDrawing.GraphIdNow;
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CheckIpPort())
                {
                    if (operateflag == 0)
                    {
                        if (IsExistsIPDef(this.videoip.Text.Trim()))
                        {
                            XtraMessageBox.Show("已存在相同的IP地址");
                            return;
                        }
                        vdefinfo.Id = IdHelper.CreateLongId().ToString();
                    }
                    else
                    {
                        //如果IP地址有改变，判断是否存在相同的IP
                        if (vdefinfo.IPAddress != this.videoip.Text && IsExistsIPDef(this.videoip.Text.Trim()))
                        {
                            XtraMessageBox.Show("已存在相同的IP地址");
                            return;
                        }
                    }

                    vdefinfo.Devname = this.videoname.Text;

                    vdefinfo.Vendor = (int)this.comboBoxvendor.SelectedValue;
                    vdefinfo.IPAddress = this.videoip.Text;
                    vdefinfo.Port = this.videoport.Text;
                    vdefinfo.Channel = Convert.ToInt32(this.videochannel.Text);
                    vdefinfo.Username = this.videousername.Text;
                    vdefinfo.Password = this.videopw.Text;

                    if (operateflag == 0)
                    {
                        AddDefInfo(vdefinfo);
                    }
                    else
                    {
                        UpdateDeptInfo(vdefinfo);
                    }

                    InsertGraphicspointinf(vdefinfo);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("操作失败： " + ex.Message);
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 数据保存失败");
            }
        }

        private bool CheckIpPort()
        {
            bool flag = true;
            try
            {
                int tempInt = 0;
                string[] ips = this.videoip.Text.Trim().Split('.');
                if (ips.Length != 4)
                {
                    MessageBox.Show("IP地址格式错误");
                    flag = false;
                }
                else
                {
                    for (int i = 0; i < ips.Length; i++)
                    {
                        if (!int.TryParse(ips[i], out tempInt))
                        {
                            flag = false;
                            break;
                        }
                        else
                        {
                            if (tempInt > 255 || tempInt < 0)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }
                    if (flag == false)
                    {
                        MessageBox.Show("IP地址格式错误");
                        flag = false;
                    }
                    else
                    {
                        if (!int.TryParse(this.videoport.Text, out tempInt))
                        {
                            MessageBox.Show("端口号格式错误");
                            flag = false;
                        }
                        else
                        {
                            if (tempInt < 0)
                            {
                                MessageBox.Show("端口号格式错误");
                                flag = false;
                            }
                        }
                    }
                }
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        private void AddDefInfo(V_DefInfo definfo)
        {
            try
            {
                DefAddRequest defaddRequest = new DefAddRequest();
                defaddRequest.DefInfo = definfo;

                var defaddResponse = _vdefService.AddDef(defaddRequest);

                if (defaddResponse.IsSuccess)
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据成功";
                }
                else
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss " + "添加添加失败");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("添加添加失败： " + ex.Message);
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 添加添加失败");
            }
        }

        private void UpdateDeptInfo(V_DefInfo definfo)
        {
            try
            {
                DefUpdateRequest defupdateRequest = new DefUpdateRequest();
                defupdateRequest.DefInfo = definfo;

                var defupdateResponse = _vdefService.UpdateDef(defupdateRequest);

                if (defupdateResponse.IsSuccess)
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "  保存数据成功";
                }
                else
                {
                    StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss " + "人员编辑失败");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Info("人员编辑失败： " + ex.Message);
                StaticMsg.Caption = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + " 人员编辑失败");
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void GetVendorDataSource(int selectvalue)
        {
            var datasource = Sys.Safety.Enums.EnumHelper.GetEnumKeyPairValue<VideoVendorType>();
            this.comboBoxvendor.DisplayMember = "Value";
            this.comboBoxvendor.ValueMember = "Key";
            this.comboBoxvendor.DataSource = datasource;
            this.comboBoxvendor.SelectedValue = selectvalue;
        }

        private bool IsExistsIPDef(string ipaddress)
        {
            var defino = _vdefService.GetDefByIP(new DefIPRequest { IPAddress = ipaddress }).Data;
            if (defino != null)
                return true;
            return false;
        }

        /// <summary>
        /// 添加图形测点
        /// </summary>
        /// <param name="vdefinfo"></param>
        private void InsertGraphicspointinf(V_DefInfo vdefinfo)
        {
            if (!string.IsNullOrEmpty(PointInGraphId))
            {
                //将图形信息添加到图形测点表中
                GraphicspointsinfInfo graphpointInfo = new GraphicspointsinfInfo();
                graphpointInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                graphpointInfo.GraphId = PointInGraphId;
                graphpointInfo.PointID = vdefinfo.Id;
                graphpointInfo.Point = vdefinfo.IPAddress;

                graphpointInfo.GraphBindName = "gif_摄像机";
                graphpointInfo.GraphBindType = 3;
                graphpointInfo.DisZoomlevel = "1$22";
                var arrcoordinate = vdefinfo.By3.Split(',');
                graphpointInfo.XCoordinate = arrcoordinate[0];
                graphpointInfo.YCoordinate = arrcoordinate[1];
                graphpointInfo.Bz1 = "-1";
                graphpointInfo.Bz2 = "0";
                graphpointInfo.Bz3 = "0";
                graphpointInfo.Upflag = "0";

                GetGraphicspointsinfByGraphIdAndPointRequest graphicspointsinfrequest = new GetGraphicspointsinfByGraphIdAndPointRequest();
                graphicspointsinfrequest.PointId = vdefinfo.IPAddress;
                graphicspointsinfrequest.GraphId = PointInGraphId;
                GraphicspointsinfInfo graphicspointsinfInfo = _graphicspointsinfService.GetGraphicspointsinfByGraphIdAndPoint(graphicspointsinfrequest).Data;
                if (graphicspointsinfInfo != null)
                {//先删除之前定义的坐标信息
                    GraphicspointsinfDeleteRequest deletegraphicspointsinfrequest = new GraphicspointsinfDeleteRequest();
                    deletegraphicspointsinfrequest.Id = graphicspointsinfInfo.ID;
                    _graphicspointsinfService.DeleteGraphicspointsinf(deletegraphicspointsinfrequest);
                }
                //将测点信息保存到图形测点信息表中
                GraphicspointsinfAddRequest addgraphicspointsinfrequest = new GraphicspointsinfAddRequest();
                addgraphicspointsinfrequest.GraphicspointsinfInfo = graphpointInfo;
                _graphicspointsinfService.AddGraphicspointsinf(addgraphicspointsinfrequest);
            }
        }
    }
}
