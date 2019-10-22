using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Graphicsbaseinf;
using Sys.Safety.Request.Graphicspointsinf;
using Sys.Safety.Request.Graphicsrouteinf;
using Sys.Safety.Enums;

namespace Sys.Safety.Client.GraphDefine
{
    /// <summary>
    /// 图形操作类
    /// </summary>
    public class GraphicOperations
    {

        private IGraphicsbaseinfService graphicsbaseinfService = ServiceFactory.Create<IGraphicsbaseinfService>();
        private IGraphicsrouteinfService graphicsrouteinfService = ServiceFactory.Create<IGraphicsrouteinfService>();
        private IGraphicspointsinfService graphicspointsinfService = ServiceFactory.Create<IGraphicspointsinfService>();
        private ILargedataAnalysisConfigService analysisconfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();

        private IV_DefService vdefService = ServiceFactory.Create<IV_DefService>();

        /// <summary>
        /// 是否处于编辑状态
        /// </summary>
        public bool IsGraphicEdit = true;
        /// <summary>
        /// 当前图形是否处于已保存状态
        /// </summary>
        public bool IsGraphicEditSave = true;
        /// <summary>
        /// 是否进行拓扑图井上设备初始化
        /// </summary>
        public bool IsTopologyInit = false;
        /// <summary>
        /// 当前打开的图形
        /// </summary>
        public string GraphNameNow = "";

        

        /// <summary>
        /// 图形库加载
        /// </summary>
        /// <param name="type">图形库类型（0：实时显示值图形，1：拓扑图形，2：SVG图形 3：gif动画图形）</param>
        public IList<string> GraphicsLibLoad(int type)
        {
            IList<string> GraphImgList = new List<string>();
            string[] files = null;
            string Path = Application.StartupPath + "\\mx";
            string PathStr = "";
            try
            {
                switch (type)
                {
                    case 0:
                        PathStr = Path + "\\Text";
                        break;
                    case 1:
                        PathStr = Path + "\\Topology";
                        break;
                    case 2:
                        PathStr = Path + "\\Svg";
                        break;
                    case 3:
                        PathStr = Path + "\\Gif";
                        break;
                }
                files = Directory.GetFiles(PathStr);//得到文件
                foreach (string file in files)
                {
                    if (file.Contains(".png"))
                    {
                        GraphImgList.Add(file.Substring(file.LastIndexOf("\\") + 1, file.LastIndexOf(".") - file.LastIndexOf("\\") - 1));
                    }
                }
            }
            catch (Exception ex)
            {
                //日志
            }
            return GraphImgList;
        }
        /// <summary>
        /// 添加图元到地图
        /// </summary>
        /// <param name="view"></param>
        /// <param name="PointName"></param>
        /// <param name="GraphBindType"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void AddPointToMap(Axmetamap2dLib.AxMetaMapX2D mx, string PointName, string GrapUnitName,
            int GraphBindType, string zoomLevel, string animationState, float left, float top, string Width, string Height, string graphType)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页  
            //view.EvaluateJavaScript          
            mx.CurViewEvaluateJavaScript("AddPixelPointToMap('" + PointName + "','','','" + GrapUnitName + "','"
                + GraphBindType + "','" + zoomLevel + "','" + animationState + "', '" + left + "',' " + top + "','" + Width + "','" + Height + "','" + graphType + "')");

            IsGraphicEditSave = false;//设置图形为未保存状态
        }
        /// <summary>
        /// 加载图形文件
        /// </summary>
        /// <param name="view"></param>
        /// <param name="GraphName"></param>
        public void LoadMap(Axmetamap2dLib.AxMetaMapX2D mx, string GraphName)
        {
            try
            {
                if (!IsGraphicEditSave)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("刚才修改的图形未保存，建议先保存再打开新图形，确定要打开新的图形吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                string tempGraphName = "";
                GraphicsbaseinfInfo graphDto = getGraphicDto(GraphName);
                if (graphDto != null && !string.IsNullOrEmpty(graphDto.Bz1))
                {
                    tempGraphName = graphDto.Bz1;
                }
                else
                {
                    tempGraphName = GraphName;
                }


                string path = Application.StartupPath;
                mx.SetWebRootPath(path + "\\");//设置根目录
                mx.SetMapScriptPath("mx/");//设置元图脚本API路径，相对于根目录地址
                string dwgFileName = "";
                if (tempGraphName.Contains(".dwg") || tempGraphName.Contains(".dxf"))
                {
                    dwgFileName = path + "\\mx\\dwg\\" + tempGraphName;//相对于网站根目录 的地址
                }
                else if (tempGraphName.Contains(".html"))
                {
                    dwgFileName = path + "\\" + tempGraphName;//相对于网站根目录 的地址
                }
                else
                {
                    dwgFileName = "width:1000;height:800";
                }
                //打开dwg图形 
                //filePath 文件路径
                //bNewView 是否新建视图  当为false时，如果没有一个视图，自动新建,否则为最后一个view打开
                //sViewName 视图名称，后可根据名称检索，如需检索请调入都确保名字唯一
                //int OpenDwg(string filePath, bool bNewView, string sViewName);
                //mx.CurViewSetVisibleOrClose(true);



                if (tempGraphName.Contains(".html"))
                {
                    mx.OpenUrl(dwgFileName, false, "");
                }
                else
                {
                    mx.OpenDwg(dwgFileName, false, "");
                }


                GraphNameNow = GraphName;//设置当前图形   
                IsGraphicEditSave = true;//设置初始状态为已保存状态  
                
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicOperations_LoadMap" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 加载图形对应测点绑定信息
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="GraphName"></param>
        public void LoadMapPointsInfo(Axmetamap2dLib.AxMetaMapX2D mx, string GraphName)
        {
            try
            {
                string LoadPointStr = "";
                Image img;
                string Width = "";
                string Height = "";
                string PathStr = "";
                string Path = Application.StartupPath + "\\mx";
                int strokeWidth = 3;
                GraphicsbaseinfInfo GraphicsbaseinfDTO_ = getGraphicDto(GraphName);
                string strsql = "";

                //                if (ServiceFactory.CreateService<IGraphicsbaseinfService>().getDBType() == "MySQL")
                //                {
                //                    strsql = @"select graphicspointsinf.*,jc_wz.wz,wz1.wz as wz1,jc_dev.name from graphicspointsinf 
                //                    left join jc_def on substring_index(graphicspointsinf.point,'￣',1)=jc_def.point and jc_def.activity=1
                //                    left join jc_wz on jc_wz.wzid=jc_def.wzid 
                //                    left join jc_dev on jc_dev.devid=jc_def.devid
                //                    left join jc_mac on substring_index(graphicspointsinf.point,'￣',1)=jc_mac.IP
                //                    left join jc_wz as wz1 on wz1.wzid=jc_mac.wzid 
                //                    
                //                    where GraphId='" + GraphicsbaseinfDTO_.GraphId + "'";
                //                }
                //                else
                //                {
                //                    strsql = @"select graphicspointsinf.*,jc_wz.wz,wz1.wz as wz1,jc_dev.name from graphicspointsinf 
                //left join jc_def on substring(graphicspointsinf.point,0,
                //case when charindex('￣',graphicspointsinf.point)>0 
                //then charindex('￣',graphicspointsinf.point) 
                //else len(graphicspointsinf.point)+1 end 
                // )=jc_def.point and jc_def.activity=1
                //left join jc_wz on jc_wz.wzid=jc_def.wzid 
                //left join jc_dev on jc_dev.devid=jc_def.devid
                //left join jc_mac on substring(graphicspointsinf.point,0,
                //case when charindex('￣',graphicspointsinf.point)>0 
                //then charindex('￣',graphicspointsinf.point) 
                //else len(graphicspointsinf.point)+1 end 
                // )=jc_mac.IP
                //left join jc_wz as wz1 on wz1.wzid=jc_mac.wzid 
                //
                //where GraphId='" + GraphicsbaseinfDTO_.GraphId + "'";
                //                }

                //DataTable dt = ServiceFactory.CreateService<IGraphicspointsinfService>().GetDataTableBySQL(strsql);
                if (GraphicsbaseinfDTO_ == null)
                {
                    return;
                }
                var getMapPointsInfoRequest = new GetMapPointsInfoRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
                var getMapPointsInfoResponse = graphicsbaseinfService.GetMapPointsInfo(getMapPointsInfoRequest);
                DataTable dt = getMapPointsInfoResponse.Data;

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["GraphBindType"].ToString() == "3")
                    {
                        PathStr = Path + "\\Gif";
                        img = Image.FromFile(PathStr + "\\" + dt.Rows[i]["GraphBindName"].ToString() + "_.gif");
                        Width = img.PhysicalDimension.Width.ToString();
                        Height = img.PhysicalDimension.Height.ToString();
                    }
                    if (dt.Rows[i]["GraphBindType"].ToString() == "1" || dt.Rows[i]["GraphBindType"].ToString() == "2")//SVG图元从数据库中读取高度和宽度
                    {

                        if (dt.Rows[i]["GraphBindName"].ToString().Contains("交换机"))
                        {
                            if (dt.Rows[i]["Point"].ToString().Contains("."))
                            {
                                LoadPointStr += dt.Rows[i]["Point"].ToString() + "|" + dt.Rows[i]["wz1"].ToString() + "|" + "交换机"
                                  + "|" + dt.Rows[i]["GraphBindName"].ToString() + "|" + dt.Rows[i]["GraphBindType"].ToString() + "|"
                                  + dt.Rows[i]["DisZoomlevel"].ToString() + "|" + dt.Rows[i]["Bz1"].ToString() + "|"
                                  + dt.Rows[i]["XCoordinate"].ToString() + "|" + dt.Rows[i]["YCoordinate"].ToString()
                                   + "|" + dt.Rows[i]["Bz2"].ToString() + "|" + dt.Rows[i]["Bz3"].ToString() + "|0|" + dt.Rows[i]["Bz4"].ToString() + "|" + dt.Rows[i]["Bz5"].ToString() + ",";
                            }
                            else
                            {
                                LoadPointStr += dt.Rows[i]["Point"].ToString() + "|" + dt.Rows[i]["wz1"].ToString() + "|" + "串口//其它设备"
                                 + "|" + dt.Rows[i]["GraphBindName"].ToString() + "|" + dt.Rows[i]["GraphBindType"].ToString() + "|"
                                 + dt.Rows[i]["DisZoomlevel"].ToString() + "|" + dt.Rows[i]["Bz1"].ToString() + "|"
                                 + dt.Rows[i]["XCoordinate"].ToString() + "|" + dt.Rows[i]["YCoordinate"].ToString()
                                  + "|" + dt.Rows[i]["Bz2"].ToString() + "|" + dt.Rows[i]["Bz3"].ToString() + "|0|" + dt.Rows[i]["Bz4"].ToString() + "|" + dt.Rows[i]["Bz5"].ToString() + ",";
                            }
                        }
                        else
                        {
                            LoadPointStr += dt.Rows[i]["Point"].ToString() + "|" + dt.Rows[i]["wz"].ToString() + "|" + dt.Rows[i]["name"].ToString()
                              + "|" + dt.Rows[i]["GraphBindName"].ToString() + "|" + dt.Rows[i]["GraphBindType"].ToString() + "|"
                              + dt.Rows[i]["DisZoomlevel"].ToString() + "|" + dt.Rows[i]["Bz1"].ToString() + "|"
                              + dt.Rows[i]["XCoordinate"].ToString() + "|" + dt.Rows[i]["YCoordinate"].ToString()
                               + "|" + dt.Rows[i]["Bz2"].ToString() + "|" + dt.Rows[i]["Bz3"].ToString() + "|0|" + dt.Rows[i]["Bz4"].ToString() + "|" + dt.Rows[i]["Bz5"].ToString() + ",";
                        }
                    }
                    else
                    {
                        if (Basic.Framework.Common.ValidationHelper.IsRightIP(dt.Rows[i]["Point"].ToString()) && dt.Rows[i]["GraphBindName"].ToString().Contains("交换机"))
                        {
                            LoadPointStr += dt.Rows[i]["Point"].ToString() + "|" + dt.Rows[i]["wz1"].ToString() + "|" + "交换机"
                                 + "|" + dt.Rows[i]["GraphBindName"].ToString() + "|" + dt.Rows[i]["GraphBindType"].ToString() + "|"
                                 + dt.Rows[i]["DisZoomlevel"].ToString() + "|" + dt.Rows[i]["Bz1"].ToString() + "|"
                                 + dt.Rows[i]["XCoordinate"].ToString() + "|" + dt.Rows[i]["YCoordinate"].ToString()
                                  + "|" + dt.Rows[i]["Bz2"].ToString() + "|" + dt.Rows[i]["Bz3"].ToString() + "|0|" + dt.Rows[i]["Bz4"].ToString() + "|" + dt.Rows[i]["Bz5"].ToString() + ",";
                        }
                        else
                        {
                            LoadPointStr += dt.Rows[i]["Point"].ToString() + "|" + dt.Rows[i]["wz"].ToString() + "|" + dt.Rows[i]["name"].ToString()
                                + "|" + dt.Rows[i]["GraphBindName"].ToString() + "|" + dt.Rows[i]["GraphBindType"].ToString() + "|"
                                + dt.Rows[i]["DisZoomlevel"].ToString() + "|" + dt.Rows[i]["Bz1"].ToString() + "|"
                                + dt.Rows[i]["XCoordinate"].ToString() + "|" + dt.Rows[i]["YCoordinate"].ToString()
                                 + "|" + Width + "|" + Height + "|0|" + dt.Rows[i]["Bz4"].ToString() + "|" + dt.Rows[i]["Bz5"].ToString() + ",";
                        }
                    }
                }
                if (LoadPointStr.Contains(","))
                {
                    LoadPointStr = LoadPointStr.Substring(0, LoadPointStr.Length - 1);
                }

                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页   
                if (LoadPointStr.Length > 0)
                {
                    mx.CurViewEvaluateJavaScript("LoadPoint('" + LoadPointStr + "')");
                }

                string LoadRoutesStr = "";

                //strsql = "select * from graphicsrouteinf  where GraphId='" + GraphicsbaseinfDTO_.GraphId + "'";

                //dt = ServiceFactory.CreateService<IGraphicspointsinfService>().GetDataTableBySQL(strsql);

                var getMapRoutesInfoRequest = new GetMapRoutesInfoRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
                var getMapRoutesInfoResponse = graphicsbaseinfService.GetMapRoutesInfo(getMapRoutesInfoRequest);
                dt = getMapRoutesInfoResponse.Data;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LoadRoutesStr += dt.Rows[i]["Bz1"].ToString() + "|" + dt.Rows[i]["SPoint"].ToString() + "|" + dt.Rows[i]["EPoint"].ToString() + "|"
                         + dt.Rows[i]["SPointX"].ToString() + "," + dt.Rows[i]["SPointY"].ToString() + "|"
                         + dt.Rows[i]["EPointX"].ToString() + "," + dt.Rows[i]["EPointY"].ToString() + "|"
                         + dt.Rows[i]["GraphLines"].ToString() + "&";
                }
                if (LoadRoutesStr.Contains("&"))
                {
                    LoadRoutesStr = LoadRoutesStr.Substring(0, LoadRoutesStr.Length - 1);
                }
                if (LoadRoutesStr.Length > 0)
                {
                    if (getGraphicNowType() == 1)//拓扑图
                    {
                        mx.CurViewEvaluateJavaScript("LoadRoutes('" + LoadRoutesStr + "')");
                    }
                    else//动态图
                    {
                        if (IsGraphicEdit)
                        {
                            strokeWidth = 3;
                        }
                        else//显示状态将巷道连接置宽
                        {
                            strokeWidth = 10;
                        }
                        mx.CurViewEvaluateJavaScript("LoadTrajectorys('" + LoadRoutesStr + "','" + strokeWidth + "')");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GraphicOperations_LoadMapPointsInfo" + ex.Message + ex.StackTrace);
            }

        }

        /// <summary>
        /// 获取所有图元测点
        /// </summary>
        /// <param name="GraphName"></param>
        /// <returns></returns>
        public DataTable LoadAllMapPointInfo(string GraphName)
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = getGraphicDto(GraphName);
            var getMapPointsInfoRequest = new GetMapPointsInfoRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
            var getMapPointsInfoResponse = graphicsbaseinfService.GetMapPointsInfo(getMapPointsInfoRequest);
            DataTable dt = getMapPointsInfoResponse.Data;
            return dt;
        }

        /// <summary>
        /// 加载地图图层
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public List<string> LoadLayers(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            List<string> MapLayersList = new List<string>();
            string scriptCmd = "getMapLayers()";
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return MapLayersList;//如果还没有打开图形或网页
            string allMapLayers = mx.CurViewEvaluateJavaScript(scriptCmd);
            MapLayersList = allMapLayers.Split('|').ToList<string>();
            return MapLayersList;
        }
        /// <summary>
        /// 图层隐藏
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="LayerName"></param>
        public void LayerHidden(Axmetamap2dLib.AxMetaMapX2D mx, string LayerName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("LayerHidden('" + LayerName + "')");
        }
        /// <summary>
        /// 测点隐藏
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointName"></param>
        public void PointHidden(Axmetamap2dLib.AxMetaMapX2D mx, string PointName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("setPointShow('" + PointName + "','0')");
        }
        /// <summary>
        /// 图层显示
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="LayerName"></param>
        public void LayerDisplay(Axmetamap2dLib.AxMetaMapX2D mx, string LayerName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("LayerDisplay('" + LayerName + "')");
        }
        /// <summary>
        /// 测点显示
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointName"></param>
        public void PointDisplay(Axmetamap2dLib.AxMetaMapX2D mx, string PointName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("setPointShow('" + PointName + "','1')");
        }
        /// <summary>
        /// 测点定位
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointName"></param>
        public void PointSercah(Axmetamap2dLib.AxMetaMapX2D mx, string PointName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("DrawPointGif('" + PointName + "','blue','5000')");
        }
        /// <summary>
        /// 放大图形
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="Multiple">倍数</param>
        public void ZoomOut(Axmetamap2dLib.AxMetaMapX2D mx, string Multiple)
        {
            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = "map.setZoom(map.getZoom() + " + Multiple + ");";
            //得到当前视图
            //方法一
            //metamap2dLib.IMetaMapViewList =  MetaMapX.ViewList();//得到控件的视图集合
            //if(viewList.Count() == 0) return;//如果还没有打开图形或网页
            //metamap2dLib.IMetaMapView = viewList.ItemIndex(0);//得到当前的视图对象。
            //方法二
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
        }
        /// <summary>
        /// 缩小图形
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="Multiple">倍数</param>
        public void ZoomIn(Axmetamap2dLib.AxMetaMapX2D mx, string Multiple)
        {
            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = "map.setZoom(map.getZoom() - " + Multiple + ");";
            //得到当前视图
            //方法一
            //metamap2dLib.IMetaMapViewList =  MetaMapX.ViewList();//得到控件的视图集合
            //if(viewList.Count() == 0) return;//如果还没有打开图形或网页
            //metamap2dLib.IMetaMapView = viewList.ItemIndex(0);//得到当前的视图对象。
            //方法二
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
        }
        /// <summary>
        /// 图形缩放
        /// </summary>
        /// <param name="mx"></param>        
        public void zoomExtent(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = " map.zoomExtent();";
            //得到当前视图
            //方法一
            //metamap2dLib.IMetaMapViewList =  MetaMapX.ViewList();//得到控件的视图集合
            //if(viewList.Count() == 0) return;//如果还没有打开图形或网页
            //metamap2dLib.IMetaMapView = viewList.ItemIndex(0);//得到当前的视图对象。
            //方法二
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
        }
        /// <summary>
        /// 设置图形的编辑状态
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="State"></param>
        public void setGraphEditState(Axmetamap2dLib.AxMetaMapX2D mx, bool State)
        {
            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = "SetGraphicEdit(" + State.ToString().ToLower() + ")";
            //得到当前视图
            //方法一
            //metamap2dLib.IMetaMapViewList =  MetaMapX.ViewList();//得到控件的视图集合
            //if(viewList.Count() == 0) return;//如果还没有打开图形或网页
            //metamap2dLib.IMetaMapView = viewList.ItemIndex(0);//得到当前的视图对象。
            //方法二
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
        }
        /// <summary>
        /// 设备拓扑图初始化所有井上设备
        /// </summary>
        /// <param name="mx"></param>
        public void SetMapDefineTopologyInit(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //if (getGraphicNowType() == 1)//拓扑图
            //{
            #region 加载地面测点
            string PointListStr = "";
            PointListStr = PointListStr + @"000000|||打印机&静|1|0|0|20.7133|693.846|64|64|1|,";
            PointListStr = PointListStr + @"000000-1|||防火墙&静|1|0|0|114.8671|696.699|64|64|1|,";
            PointListStr = PointListStr + @"000000-2|||数据库&静|1|0|0|220.434|696.993|64|64|1|,";
            PointListStr = PointListStr + @"000000-3|||数据库&静|1|0|0|250.392|696.993|64|64|1|,";
            PointListStr = PointListStr + @"000000-4|||客户端&静|1|0|0|354.49|696.14|64|64|1|,";
            //PointListStr = PointListStr + @"000000-5|||客户端&静|1|0|0|338.825|696.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-6|||调度台&电|1|0|0|505.79|696.832|114|92|1|,";
            PointListStr = PointListStr + @"000000-7|||监控主机&静|1|0|0|601.315|696.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-8|||监控主机&静|1|0|0|639.832|696.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-9|||显示屏&屏|1|0|0|756.867|696.713|114|71|1|,";
            PointListStr = PointListStr + @"打印机|||文字&字|1|0|0|20.58042|750.51|142|28|1|,";
            PointListStr = PointListStr + @"防火墙|||文字&字|1|0|0|114.1469|750.937|142|28|1|,";
            PointListStr = PointListStr + @"数据库服务器|||文字&字|1|0|0|240.021|750.364|142|28|1|,";
            PointListStr = PointListStr + @"客户端|||文字&字|1|0|0|351.65|750.937|142|28|1|,";
            PointListStr = PointListStr + @"调度电话|||文字&字|1|0|0|505.762|750.51|142|28|1|,";
            PointListStr = PointListStr + @"监控主机|||文字&字|1|0|0|618.434|750.084|142|28|1|,";
            PointListStr = PointListStr + @"调度大屏|||文字&字|1|0|0|752.545|750.657|142|28|1|,";
            PointListStr = PointListStr + @"000000-10|||局域网&网|1|0|0|390.986|605.427|1426|14|1|,";
            PointListStr = PointListStr + @"000000-11|||地板左&地|1|0|0|20.524|445.65|641|42|1|,";
            PointListStr = PointListStr + @"000000-12|||地板右&地|1|0|0|735.888|442.797|641|42|1|,";
            PointListStr = PointListStr + @"000000-13|||数据接口&接口|1|0|0|384.517|504.084|99|99|1|,";
            PointListStr = PointListStr + @"数据接口|||文字&字|1|0|0|435.846|544.098|142|28|1|,";
            PointListStr = PointListStr + @"地面|||文字&字|1|0|0|762.49|497.007|142|28|1|,";
            PointListStr = PointListStr + @"井下|||文字&字|1|0|0|763.916|405.706|142|28|1|";





            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = "LoadPointTB('" + PointListStr + "')";

            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
            #endregion
            #region 加载地面测点连线
            string RoutesListStr = "";
            RoutesListStr = RoutesListStr + @"1|000000|000000-10|-10.7133,713.846|-325.986,605.427|21.286699999999996,681.846#21.286699999999996,598.427#21.286699999999996,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-1|000000-10|84.8671,716.699|-325.986,605.427|116.8671,684.699#116.8671,598.427#116.8671,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-2|000000-10|190.434,710.993|-325.986,605.427|222.434,678.993#222.434,598.427#222.434,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-5|000000-10|318.825,706.713|-325.986,605.427|350.825,674.713#350.825,598.427#350.825,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-6|000000-10|445.79,723.832|-325.986,605.427|502.78999999999996,677.832#502.78999999999996,598.427#502.78999999999996,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-7|000000-10|581.315,706.713|-325.986,605.427|613.315,674.713#613.315,598.427#613.315,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-9|000000-10|696.867,706.713|-325.986,605.427|753.867,671.213#753.867,598.427#753.867,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-10|000000-13|-325.986,605.427|334.517,544.084|387.01400000000007,598.427#387.01400000000007,494.58399999999995#387.01400000000007,494.58399999999995#384.017,494.58399999999995";

            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            scriptCmd = "LoadRoutes('" + RoutesListStr + "')";

            //view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
            #endregion
            IsGraphicEditSave = false;//置图形未保存
            //}
        }

        /// <summary>
        /// 设备拓扑图初始化所有井上设备
        /// </summary>
        /// <param name="mx"></param>
        public void SetMapTopologyInit(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //if (getGraphicNowType() == 1)//拓扑图
            //{
            #region 加载地面测点
            string PointListStr = "";
            PointListStr = PointListStr + @"000000|||打印机&静|1|0|0|-10.7133|713.846|64|64|1|,";
            PointListStr = PointListStr + @"000000-1|||防火墙&静|1|0|0|84.8671|716.699|64|64|1|,";
            PointListStr = PointListStr + @"000000-2|||数据库&静|1|0|0|190.434|710.993|64|64|1|,";
            PointListStr = PointListStr + @"000000-3|||数据库&静|1|0|0|220.392|710.993|64|64|1|,";
            PointListStr = PointListStr + @"000000-4|||客户端&静|1|0|0|354.49|708.14|64|64|1|,";
            PointListStr = PointListStr + @"000000-5|||客户端&静|1|0|0|318.825|706.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-6|||调度台&电|1|0|0|445.79|723.832|114|92|1|,";
            PointListStr = PointListStr + @"000000-7|||监控主机&静|1|0|0|581.315|706.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-8|||监控主机&静|1|0|0|619.832|706.713|64|64|1|,";
            PointListStr = PointListStr + @"000000-9|||显示屏&屏|1|0|0|696.867|706.713|114|71|1|,";
            PointListStr = PointListStr + @"打印机|||文字&字|1|0|0|-3.58042|749.51|142|28|1|,";
            PointListStr = PointListStr + @"防火墙|||文字&字|1|0|0|89.1469|750.937|142|28|1|,";
            PointListStr = PointListStr + @"数据库服务器|||文字&字|1|0|0|179.021|752.364|142|28|1|,";
            PointListStr = PointListStr + @"客户端|||文字&字|1|0|0|341.65|750.937|142|28|1|,";
            PointListStr = PointListStr + @"调度电话|||文字&字|1|0|0|465.762|749.51|142|28|1|,";
            PointListStr = PointListStr + @"监控主机|||文字&字|1|0|0|598.434|748.084|142|28|1|,";
            PointListStr = PointListStr + @"调度大屏|||文字&字|1|0|0|722.545|746.657|142|28|1|,";
            PointListStr = PointListStr + @"000000-10|||局域网&网|1|0|0|-325.986|605.427|1426|14|1|,";
            PointListStr = PointListStr + @"000000-11|||地板左&地|1|0|0|-247.524|445.65|641|42|1|,";
            PointListStr = PointListStr + @"000000-12|||地板右&地|1|0|0|375.888|442.797|641|42|1|,";
            PointListStr = PointListStr + @"000000-13|||数据接口&接口|1|0|0|334.517|544.084|99|99|1|,";
            PointListStr = PointListStr + @"数据接口|||文字&字|1|0|0|405.846|534.098|142|28|1|,";
            PointListStr = PointListStr + @"地面|||文字&字|1|0|0|762.49|497.007|142|28|1|,";
            PointListStr = PointListStr + @"井下|||文字&字|1|0|0|763.916|405.706|142|28|1|";





            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            string scriptCmd = "LoadPoint('" + PointListStr + "')";

            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
            #endregion
            #region 加载地面测点连线
            string RoutesListStr = "";
            RoutesListStr = RoutesListStr + @"1|000000|000000-10|-10.7133,713.846|-325.986,605.427|21.286699999999996,681.846#21.286699999999996,598.427#21.286699999999996,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-1|000000-10|84.8671,716.699|-325.986,605.427|116.8671,684.699#116.8671,598.427#116.8671,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-2|000000-10|190.434,710.993|-325.986,605.427|222.434,678.993#222.434,598.427#222.434,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-5|000000-10|318.825,706.713|-325.986,605.427|350.825,674.713#350.825,598.427#350.825,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-6|000000-10|445.79,723.832|-325.986,605.427|502.78999999999996,677.832#502.78999999999996,598.427#502.78999999999996,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-7|000000-10|581.315,706.713|-325.986,605.427|613.315,674.713#613.315,598.427#613.315,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-9|000000-10|696.867,706.713|-325.986,605.427|753.867,671.213#753.867,598.427#753.867,598.427#387.01400000000007,598.427&";
            RoutesListStr = RoutesListStr + @"1|000000-10|000000-13|-325.986,605.427|334.517,544.084|387.01400000000007,598.427#387.01400000000007,494.58399999999995#387.01400000000007,494.58399999999995#384.017,494.58399999999995";

            //因为这个页面是IE来加载了。而mx.aspx是由元图控件去加载的。两个的环境空间是一样的，是无法直接访问的。只能调用控件的方法去访问
            scriptCmd = "LoadRoutes('" + RoutesListStr + "')";

            //view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页
            mx.CurViewEvaluateJavaScript(scriptCmd);
            #endregion
            IsGraphicEditSave = false;//置图形未保存
            //}
        }
        /// <summary>
        /// 设置图形的保存状态
        /// </summary>
        /// <param name="State"></param>
        public void setGraphEditSave(bool State)
        {
            IsGraphicEditSave = State;
        }
        /// <summary>
        /// 编辑图元绑定测点
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointName"></param>
        /// <param name="PointWz"></param>
        /// <param name="PointDevName"></param>
        public void EditPoint(Axmetamap2dLib.AxMetaMapX2D mx, string PointName, string PointWz, string PointDevName, string DisZoomlevel,
           string animationState, string Width, string Height, string TurnToPage, string PointId, string transformDeg)
        {
            #region DisZoomlevel为大于0的数字时生效,否则置0

            int minZoomLevel = 0, maxZoomLevel = 0;
            string[] zoomLevelArr = DisZoomlevel.Split('$');

            if (int.TryParse(zoomLevelArr[0], out minZoomLevel))
            {
                if (minZoomLevel <= 0)
                {
                    minZoomLevel = 0;
                }
            }
            else
            {
                minZoomLevel = 0;
            }

            if (int.TryParse(zoomLevelArr[1], out maxZoomLevel))
            {
                if (minZoomLevel <= 0)
                {
                    maxZoomLevel = 0;
                }
            }
            else
            {
                maxZoomLevel = 0;
            }
            DisZoomlevel = minZoomLevel + "$" + maxZoomLevel;
            int width = 0;
            if (int.TryParse(Width, out width))
            {
                if (width <= 0)
                {
                    Width = "0";
                }
            }
            else
            {
                Width = "0";
            }
            int height = 0;
            if (int.TryParse(Height, out height))
            {
                if (height <= 0)
                {
                    Height = "0";
                }
            }
            else
            {
                Height = "0";
            }
            #endregion

            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("EditPointToRichMark('" + PointName + "','" + PointWz + "','" + PointDevName + "','" + DisZoomlevel
                + "','" + animationState + "','" + Width + "','" + Height + "','" + TurnToPage + "','" + PointId + "','" + transformDeg + "')");

            IsGraphicEditSave = false;//设置图形为未保存状态
        }
        /// <summary>
        /// 删除图元及绑定测点
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointName"></param>
        public void DelPoint(Axmetamap2dLib.AxMetaMapX2D mx, string PointName)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("DelPointToRichMark('" + PointName + "')");

            IsGraphicEditSave = false;//设置图形为未保存状态
        }
        /// <summary>
        /// 调用客户端脚本保存测点编辑
        /// </summary>
        public void DoSaveAllPoints(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("savePoint()");
        }
        /// <summary>
        /// 调用拓扑图连线命令
        /// </summary>
        /// <param name="mx"></param>
        public void DoMapTopologyTransDef(Axmetamap2dLib.AxMetaMapX2D mx, int type)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("DoTopologyTransDef('" + type + "')");
        }
        /// <summary>
        /// 结束拓扑图连线命令
        /// </summary>
        /// <param name="mx"></param>
        public void EndMapTopologyTransDef(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("EndTopologyTransDef()");
        }
        /// <summary>
        /// 巷道连接命令
        /// </summary>
        /// <param name="mx"></param>
        public void DoMapTrajectoryDef(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("DoTrajectoryDef()");
        }
        /// <summary>
        /// 结束巷道连线命令
        /// </summary>
        /// <param name="mx"></param>
        public void EndMapTrajectoryDef(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("EndTrajectoryDef()");
        }
        /// <summary>
        /// 调用刷新实时值
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="PointSszStr"></param>
        public void DoRefPointSsz(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            string PointSszStr = "";
            if (!IsGraphicEdit)//判断是否为运行状态，运行状态才进行刷新  20171027
            {
                //PointSszStr = refPointssz.getAllPointSsz();
                try
                {
                    //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                    //if (view == null) return;//如果还没有打开图形或网页            
                    mx.CurViewEvaluateJavaScript("RefPointSsz('" + PointSszStr + "')");
                }
                catch { }
                PointSszStr = string.Empty;
            }
        }
        public void RemoveAllOverlay(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            try
            {
                //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
                //if (view == null) return;//如果还没有打开图形或网页            
                mx.CurViewEvaluateJavaScript("RemoveAllOverlay()");
            }
            catch { }
        }
        /// <summary>
        /// 根据定义信息自动生成拓扑图
        /// </summary>
        /// <param name="mx"></param>
        public void AutoDragTopologyTrans(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            string TopologyTransStr = "";
            string fzDefStr = "", sensorDefStr = "";
            string strsql = "";
            DataTable dt_ip;
            DataTable dt_def;
            string fzChildList = "", sensorChildList = "";
            DataRow[] dr_fz;
            DataRow[] dr_sensor;
            //根据定义信息获取生成拓扑图的基础数据            

            var gettDefPointResponse = graphicsbaseinfService.GetDefPointInformation();
            dt_def = gettDefPointResponse.Data;

            var getSwitchResponse = graphicsbaseinfService.GetSwitchInformation();
            dt_ip = getSwitchResponse.Data;

            for (int i = 0; i < dt_ip.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dt_ip.Rows[i]["ip"].ToString()))
                {
                    continue;//如果IP为空，则不进行加载
                }
                dr_fz = dt_def.Select("jckz2='" + dt_ip.Rows[i]["ip"].ToString() + "'");//查找当前IP模块下的所有分站
                fzChildList = "";
                fzDefStr = "";

                for (int j = 0; j < dr_fz.Length; j++)
                {
                    fzChildList = fzChildList + dr_fz[j]["point"].ToString() + "#";
                    fzDefStr = dr_fz[j]["point"].ToString() + "|" + dr_fz[j]["wz"].ToString() + "|" + dr_fz[j]["name"].ToString() + "|分站|";

                    sensorChildList = "";
                    sensorDefStr = "";
                    //自动生成不加载传感器 
                    //dr_sensor = dt_def.Select("fzh='" + dr_fz[j]["fzh"].ToString() + "' and type<>'0'");//查找当前分站下面的所有传感器
                    //for (int k = 0; k < dr_sensor.Length; k++)
                    //{
                    //    sensorChildList = sensorChildList + dr_sensor[k]["point"].ToString() + "#";
                    //    sensorDefStr = dr_sensor[k]["point"].ToString() + "|" + dr_sensor[k]["wz"].ToString() + "|" + dr_sensor[k]["name"].ToString() + "|传感器|,";
                    //    TopologyTransStr = TopologyTransStr + sensorDefStr;
                    //}
                    if (sensorChildList.Contains("#"))
                    {
                        sensorChildList = sensorChildList.Substring(0, sensorChildList.Length - 1);
                    }
                    fzDefStr += sensorChildList + ",";
                    TopologyTransStr = TopologyTransStr + fzDefStr;
                }
                if (fzChildList.Contains("#"))
                {
                    fzChildList = fzChildList.Substring(0, fzChildList.Length - 1);
                }
                TopologyTransStr = TopologyTransStr + @"" + dt_ip.Rows[i]["ip"].ToString() + "|" + dt_ip.Rows[i]["wz"].ToString() + "|交换机|交换机|" + fzChildList + ",";
            }
            if (TopologyTransStr.Contains(","))
            {
                TopologyTransStr = TopologyTransStr.Substring(0, TopologyTransStr.Length - 1);
            }
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("AutoDragTopologyTrans('" + TopologyTransStr + "')");
            IsGraphicEditSave = false;//置图形未保存
        }

        /// <summary>
        /// 根据定义信息自动生成系统定义拓扑图
        /// </summary>
        /// <param name="mx"></param>
        public void AutoDragDefineTopologyTrans(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            string TopologyTransStr = "";
            string fzDefStr = "", sensorDefStr = "";
            string strsql = "";
            DataTable dt_ip;
            DataTable dt_def;
            string moduleChildList = "", fzChildList = "", sensorChildList = "";
            DataRow[] dr_fz=new DataRow[0];
            DataRow[] dr_sensor;
            //根据定义信息获取生成拓扑图的基础数据            
            //获取所有已经定义设备信息
            var gettDefPointResponse = graphicsbaseinfService.GetDefPointInformation();
            dt_def = gettDefPointResponse.Data;
            //获取所有网络模块信息
            var getSwitchResponse = graphicsbaseinfService.GetSwitchInformation();
            dt_ip = getSwitchResponse.Data;

            for (int i = 0; i < dt_ip.Rows.Count; i++)
            {

                if (string.IsNullOrEmpty(dt_ip.Rows[i]["ip"].ToString()))
                {
                    continue;//如果IP为空，则不进行加载
                }                

                if (i > 0 && dt_ip.Rows[i]["Bz2"].ToString() != dt_ip.Rows[i - 1]["Bz2"].ToString())
                {
                    if (moduleChildList.Contains("#"))
                    {
                        moduleChildList = moduleChildList.Substring(0, moduleChildList.Length - 1);
                    }
                    TopologyTransStr = TopologyTransStr + @"" + dt_ip.Rows[i - 1]["Bz2"].ToString() + "|" + dt_ip.Rows[i - 1]["wz"].ToString() + "|交换机|拓扑定义-交换机|" + moduleChildList + ",";
                    moduleChildList = dt_ip.Rows[i]["ip"].ToString() + "#";                   
                }                 
                else
                {
                    moduleChildList += dt_ip.Rows[i]["ip"].ToString() + "#";                    
                }
                if (i == dt_ip.Rows.Count - 1)
                {
                    if (moduleChildList.Contains("#"))
                    {
                        moduleChildList = moduleChildList.Substring(0, moduleChildList.Length - 1);
                    }
                    TopologyTransStr = TopologyTransStr + @"" + dt_ip.Rows[i]["Bz2"].ToString() + "|" + dt_ip.Rows[i]["wz"].ToString() + "|交换机|拓扑定义-交换机|" + moduleChildList + ",";
                }

                fzChildList = "";
                fzDefStr = "";
                if (dt_def.Columns.Contains("jckz2"))
                {
                    dr_fz = dt_def.Select("jckz2='" + dt_ip.Rows[i]["ip"].ToString() + "'");//查找当前IP模块下的所有分站
                   

                    for (int j = 0; j < dr_fz.Length; j++)
                    {
                        fzChildList = fzChildList + dr_fz[j]["point"].ToString() + "#";
                        fzDefStr = dr_fz[j]["point"].ToString() + "|" + dr_fz[j]["wz"].ToString() + "|" + dr_fz[j]["name"].ToString() + "|拓扑定义-分站|";

                        sensorChildList = "";
                        sensorDefStr = "";
                        //自动生成不加载传感器 
                        //dr_sensor = dt_def.Select("fzh='" + dr_fz[j]["fzh"].ToString() + "' and type<>'0'");//查找当前分站下面的所有传感器
                        //for (int k = 0; k < dr_sensor.Length; k++)
                        //{
                        //    sensorChildList = sensorChildList + dr_sensor[k]["point"].ToString() + "#";
                        //    sensorDefStr = dr_sensor[k]["point"].ToString() + "|" + dr_sensor[k]["wz"].ToString() + "|" + dr_sensor[k]["name"].ToString() + "|传感器|,";
                        //    TopologyTransStr = TopologyTransStr + sensorDefStr;
                        //}
                        if (sensorChildList.Contains("#"))
                        {
                            sensorChildList = sensorChildList.Substring(0, sensorChildList.Length - 1);
                        }
                        fzDefStr += sensorChildList + ",";
                        TopologyTransStr = TopologyTransStr + fzDefStr;
                    }
                }
                //如果未挂满分站，则增加添加分站的图元  20180525
                if (dr_fz.Length < 8)
                {
                    fzChildList = fzChildList + dt_ip.Rows[i]["ip"].ToString() + "+" + "#";
                    TopologyTransStr = TopologyTransStr + dt_ip.Rows[i]["ip"].ToString() + "+" + "|" + "添加分站" + "|" + "分站" + "|拓扑定义-添加分站|,";
                }
                if (fzChildList.Contains("#"))
                {
                    fzChildList = fzChildList.Substring(0, fzChildList.Length - 1);
                }
                TopologyTransStr = TopologyTransStr + @"" + dt_ip.Rows[i]["ip"].ToString() + "|" + dt_ip.Rows[i]["mac"].ToString() + "|网络模块|拓扑定义-网络模块|" + fzChildList + ",";
            }
            //增加添加交换机图元  20180525
            TopologyTransStr = TopologyTransStr + @"" + "00.00.00.00.00.00" + "|" + "添加交换机" + "|交换机|拓扑定义-添加交换机|" + "" + ",";
            if (TopologyTransStr.Contains(","))
            {
                TopologyTransStr = TopologyTransStr.Substring(0, TopologyTransStr.Length - 1);
            }
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("AutoDragDefineTopologyTrans('" + TopologyTransStr + "')");
            IsGraphicEditSave = false;//置图形未保存
        }

        /// <summary>
        /// 打印图形
        /// </summary>
        /// <param name="mx"></param>
        public void mapPrint(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("mappaintPicture(true)");
        }
        /// <summary>
        /// 测量命令
        /// </summary>
        /// <param name="mx"></param>
        public void MeasureCommand(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("var disCmd = new mxLib.DistanceMeasureCmd(); map.startCommand(disCmd);");
        }

        /// <summary>
        /// 导出图片
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="fileName"></param>
        public void mapToImage(Axmetamap2dLib.AxMetaMapX2D mx, string fileName)
        {
            fileName = fileName.Replace("\\", "\\\\");
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("mapsavePicture('" + fileName + "','PNG')");
        }
        /// <summary>
        /// 加载地图右键页面
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="pageListstr"></param>
        public void AddMapRightMenu(Axmetamap2dLib.AxMetaMapX2D mx, string pageListstr)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("AddMapRightMenu('" + pageListstr + "')");
        }
        public void RemoveRichPointRconMenu(Axmetamap2dLib.AxMetaMapX2D mx, string menuName)
        {
            mx.CurViewEvaluateJavaScript("RemoveRichPointRconMenu('" + menuName + "')");
        }
        /// <summary>
        /// 获取所有图形页面
        /// </summary>
        /// <returns></returns>
        public List<string> c()
        {
            List<string> Rvalue = new List<string>();
            try
            {
                var request = new GraphicsbaseinfGetListRequest();
                request.PagerInfo.PageIndex = 1;
                request.PagerInfo.PageSize = int.MaxValue;
                var response = graphicsbaseinfService.GetGraphicsbaseinfList(request);
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var item in response.Data)
                    {
                        Rvalue.Add(item.GraphName);
                    }
                }
            }
            catch (Exception ex)
            { }
            return Rvalue;
        }
        /// <summary>
        /// 线程中调用
        /// </summary>
        /// <returns></returns>
        public List<string> GraphPagesLoad()
        {
            List<string> Rvalue = new List<string>();
            try
            {
                //IList<MAS.mas.DTO.GraphicsbaseinfDTO> GraphicsbaseinfDTOs = ServiceFactory.CreateService<IGraphSaveFlagService>().GetAll();
                //foreach (MAS.mas.DTO.GraphicsbaseinfDTO GraphicsbaseinfDTO_ in GraphicsbaseinfDTOs)
                //{
                //    Rvalue.Add(GraphicsbaseinfDTO_.GraphName);
                //}
                var request = new GraphicsbaseinfGetListRequest();
                request.PagerInfo.PageIndex = 1;
                request.PagerInfo.PageSize = int.MaxValue;
                var response = graphicsbaseinfService.GetGraphicsbaseinfList(request);
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var item in response.Data)
                    {
                        Rvalue.Add(item.GraphName);
                    }
                }

            }
            catch (Exception ex)
            { }
            return Rvalue;
        }
        /// <summary>
        /// 新建图形-GraphicsBaseInf表
        /// </summary>
        public void GraphInsert(string GraphName, short Type, string bz3, string bz4, byte[] GraphData)
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_.GraphName = GraphName;
            GraphicsbaseinfDTO_.Type = Type;
            GraphicsbaseinfDTO_.GraphData = GraphData == null ? string.Empty : Convert.ToBase64String(GraphData);
            GraphicsbaseinfDTO_.Timer = DateTime.Now;
            GraphicsbaseinfDTO_.Upflag = "0";
            GraphicsbaseinfDTO_.Bz3 = bz3;
            GraphicsbaseinfDTO_.Bz4 = bz4;
            var requst = new GraphicsbaseinfAddRequest() { GraphicsbaseinfInfo = GraphicsbaseinfDTO_ };
            var response = graphicsbaseinfService.AddGraphicsbaseinf(requst);
        }
        /// <summary>
        /// 新建图形-GraphicsBaseInf表
        /// </summary>
        public void GraphInsert(string GraphName, short Type, string bz4, byte[] GraphData, string HtmlFileName, string SVGFileName)
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_.GraphName = GraphName;
            GraphicsbaseinfDTO_.Type = Type;
            GraphicsbaseinfDTO_.GraphData = GraphData == null ? string.Empty : Convert.ToBase64String(GraphData);
            GraphicsbaseinfDTO_.Timer = DateTime.Now;
            GraphicsbaseinfDTO_.Upflag = "0";
            GraphicsbaseinfDTO_.Bz1 = HtmlFileName;
            GraphicsbaseinfDTO_.Bz2 = SVGFileName;
            GraphicsbaseinfDTO_.Bz4 = bz4;
            var requst = new GraphicsbaseinfAddRequest() { GraphicsbaseinfInfo = GraphicsbaseinfDTO_ };
            var response = graphicsbaseinfService.AddGraphicsbaseinf(requst);
        }
        /// <summary>
        /// 图形更新-GraphicsBaseInf表
        /// </summary>
        /// <param name="GraphName"></param>
        /// <param name="Type"></param>
        /// <param name="GraphData"></param>
        public void GraphUpdate(string SGraphName, string NGraphName, short Type, string bz3, string bz4, byte[] GraphData)
        {
            var OldGraphicsbaseinfDTO = new GraphicsbaseinfInfo();
            var getGraphicsbaseinfByNameRequest = new GetGraphicsbaseinfByNameRequest() { GraphName = SGraphName };
            var getGraphicsbaseinfByNameResponse = graphicsbaseinfService.GetGraphicsbaseinfByName(getGraphicsbaseinfByNameRequest);
            if (getGraphicsbaseinfByNameResponse.Data != null)
            {
                OldGraphicsbaseinfDTO = getGraphicsbaseinfByNameResponse.Data;
                OldGraphicsbaseinfDTO.GraphName = NGraphName;
                OldGraphicsbaseinfDTO.Type = Type;
                OldGraphicsbaseinfDTO.GraphData = GraphData == null ? string.Empty : Convert.ToBase64String(GraphData);
                OldGraphicsbaseinfDTO.Timer = DateTime.Now;
                OldGraphicsbaseinfDTO.Upflag = "0";
                OldGraphicsbaseinfDTO.Bz3 = bz3;
                OldGraphicsbaseinfDTO.Bz4 = bz4;
                var updateGraphicsbaseinfRequst = new GraphicsbaseinfUpdateRequest() { GraphicsbaseinfInfo = OldGraphicsbaseinfDTO };
                var updateGraphicsbaseinfResponse = graphicsbaseinfService.UpdateGraphicsbaseinf(updateGraphicsbaseinfRequst);
            }
        }
        /// <summary>
        /// 图形更新-GraphicsBaseInf表
        /// </summary>
        /// <param name="GraphName"></param>
        /// <param name="Type"></param>
        /// <param name="GraphData"></param>
        public void GraphUpdate(string SGraphName, string NGraphName, short Type, byte[] GraphData, string HtmlFileName, string SVGFileName)
        {

            var OldGraphicsbaseinfDTO = new GraphicsbaseinfInfo();
            var getGraphicsbaseinfByNameRequest = new GetGraphicsbaseinfByNameRequest() { GraphName = SGraphName };
            var getGraphicsbaseinfByNameResponse = graphicsbaseinfService.GetGraphicsbaseinfByName(getGraphicsbaseinfByNameRequest);
            if (getGraphicsbaseinfByNameResponse.Data != null)
            {
                OldGraphicsbaseinfDTO = getGraphicsbaseinfByNameResponse.Data;
                OldGraphicsbaseinfDTO.GraphName = NGraphName;
                OldGraphicsbaseinfDTO.Type = Type;
                OldGraphicsbaseinfDTO.GraphData = Convert.ToBase64String(GraphData);
                OldGraphicsbaseinfDTO.GraphData = GraphData == null ? string.Empty : Convert.ToBase64String(GraphData);
                OldGraphicsbaseinfDTO.Timer = DateTime.Now;
                OldGraphicsbaseinfDTO.Upflag = "0";
                OldGraphicsbaseinfDTO.Bz1 = HtmlFileName;
                OldGraphicsbaseinfDTO.Bz2 = SVGFileName;
                var updateGraphicsbaseinfRequst = new GraphicsbaseinfUpdateRequest() { GraphicsbaseinfInfo = OldGraphicsbaseinfDTO };
                var updateGraphicsbaseinfResponse = graphicsbaseinfService.UpdateGraphicsbaseinf(updateGraphicsbaseinfRequst);
            }
        }
        /// <summary>
        /// 图形删除-GraphicsBaseInf表
        /// </summary>
        /// <param name="GraphName"></param>
        public void GraphDelete(string GraphName)
        {
            var getGraphicsbaseinfByNameRequest = new GetGraphicsbaseinfByNameRequest() { GraphName = GraphName };
            var getGraphicsbaseinfByNameResponse = graphicsbaseinfService.GetGraphicsbaseinfByName(getGraphicsbaseinfByNameRequest);
            if (getGraphicsbaseinfByNameResponse.Data != null)
            {
                var deleteGraphicsbaseinfRequest = new GraphicsbaseinfDeleteRequest() { Id = getGraphicsbaseinfByNameResponse.Data.GraphId };
                var deleteGraphicsbaseinfResponse = graphicsbaseinfService.DeleteGraphicsbaseinf(deleteGraphicsbaseinfRequest);
            }
        }
        /// <summary>
        /// 保存测点绑定信息
        /// </summary>
        /// <param name="PointsStr">测点绑定信息数据</param>
        public void PointsSave(string PointsStr)
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_ = getGraphicDto(GraphNameNow);
            if (GraphicsbaseinfDTO_ != null)
            {
                //先删除图形原有的绑定测点信息
                var deleteGraphicsPointsInfForGraphIdRequest = new DeleteGraphicsPointsInfForGraphIdRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
                var deleteGraphicsPointsInfForGraphIdResponse = graphicspointsinfService.DeleteGraphicsPointsInfForGraphId(deleteGraphicsPointsInfForGraphIdRequest);
            }
            if (PointsStr.Length < 1)
            {
                return;
            }
            string[] PointsObj = PointsStr.Split(',');
            //生成测点绑定信息List
            //IList<GraphicspointsinfInfo> ListGraphicspointsinfDTO = new List<GraphicspointsinfInfo>();
            foreach (string Point in PointsObj)
            {
                string[] PointObj = Point.Split('|');
                GraphicspointsinfInfo GraphicspointsinfDTO_ = new GraphicspointsinfInfo();
                GraphicspointsinfDTO_.GraphId = GraphicsbaseinfDTO_.GraphId;
                GraphicspointsinfDTO_.Point = PointObj[0];
                GraphicspointsinfDTO_.GraphBindName = PointObj[1];
                GraphicspointsinfDTO_.GraphBindType = short.Parse(PointObj[2]);
                GraphicspointsinfDTO_.DisZoomlevel = PointObj[3];
                GraphicspointsinfDTO_.Bz1 = PointObj[4];//图元的动画状态(-1,0,1,2)，-1：表示未设置
                GraphicspointsinfDTO_.XCoordinate = PointObj[5];
                GraphicspointsinfDTO_.YCoordinate = PointObj[6];
                GraphicspointsinfDTO_.Bz2 = PointObj[7];//图元的宽度
                GraphicspointsinfDTO_.Bz3 = PointObj[8];//图元的高度
                GraphicspointsinfDTO_.Bz4 = PointObj[9];//双击要跳转的页面（存页面的名称，空表示不跳转）
                GraphicspointsinfDTO_.Bz5 = PointObj[10];//增加旋转角度保存  20171226
                GraphicspointsinfDTO_.Upflag = "0";

                //图元测点系统编号
                if (PointObj[1] == "实时显示")
                    GraphicspointsinfDTO_.SysId = (int)SystemEnum.Security;
                else if (PointObj[1] == "识别器实时显示")
                    GraphicspointsinfDTO_.SysId = (int)SystemEnum.Personnel;
                else if (PointObj[1] == "gif_摄像机")
                    GraphicspointsinfDTO_.SysId = (int)SystemEnum.Video;
                else if (PointObj[1] == "gif_广播")
                    GraphicspointsinfDTO_.SysId = (int)SystemEnum.Broadcast;
                else if (PointObj[1] == "gif_大数据分析")
                    GraphicspointsinfDTO_.SysId = -1;
                else
                    GraphicspointsinfDTO_.SysId = 0;

                var graphicspointsinfAddRequest = new GraphicspointsinfAddRequest() { GraphicspointsinfInfo = GraphicspointsinfDTO_ };
                var graphicspointsinfAddResponse = graphicspointsinfService.AddGraphicspointsinf(graphicspointsinfAddRequest);

            }
            //批量添加所有测点绑定信息
            //ServiceFactory.CreateService<IGraphicspointsinfService>().SaveDTOs(ListGraphicspointsinfDTO);
            IsGraphicEditSave = true;

        }
        /// <summary>
        /// 保存测点连线
        /// </summary>
        /// <param name="RoutesStr"></param>
        public void RoutesSave(string RoutesStr)
        {

            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_ = getGraphicDto(GraphNameNow);
            //先删除图形原有的绑定测点信息          
            if (GraphicsbaseinfDTO_ != null)
            {
                var deleteGraphicsrouteinfRequest = new DeleteGraphicsrouteinfRequest() { GraphId = GraphicsbaseinfDTO_.GraphId };
                var deleteGraphicsrouteinfResponse = graphicsrouteinfService.DeleteGraphicsrouteinfForGraphId(deleteGraphicsrouteinfRequest);
            }
            if (RoutesStr.Length < 1)
            {
                return;
            }
            string[] RoutesObj = RoutesStr.Split('&');
            //生成测点绑定信息List
            //IList<GraphicsrouteinfInfo> ListGraphicspointsinfDTO = new List<GraphicsrouteinfInfo>();
            foreach (string Route in RoutesObj)
            {
                string[] RouteObj = Route.Split('|');
                GraphicsrouteinfInfo GraphicsrouteinfDTO_ = new GraphicsrouteinfInfo();
                GraphicsrouteinfDTO_.GraphId = GraphicsbaseinfDTO_.GraphId;
                GraphicsrouteinfDTO_.Bz1 = RouteObj[0];
                GraphicsrouteinfDTO_.SPoint = RouteObj[1];
                GraphicsrouteinfDTO_.EPoint = RouteObj[2];
                GraphicsrouteinfDTO_.SPointX = float.Parse(RouteObj[3].Split(',')[0]);
                GraphicsrouteinfDTO_.SPointY = float.Parse(RouteObj[3].Split(',')[1]);
                GraphicsrouteinfDTO_.EPointX = float.Parse(RouteObj[4].Split(',')[0]);
                GraphicsrouteinfDTO_.EPointY = float.Parse(RouteObj[4].Split(',')[1]);
                GraphicsrouteinfDTO_.GraphLines = RouteObj[5];
                GraphicsrouteinfDTO_.Upflag = "0";

                var graphicsrouteinfAddRequest = new GraphicsrouteinfAddRequest() { GraphicsrouteinfInfo = GraphicsrouteinfDTO_ };
                var graphicsrouteinfAddResponse = graphicsrouteinfService.AddGraphicsrouteinf(graphicsrouteinfAddRequest);


            }
            //批量添加所有测点绑定信息

            //ServiceFactory.CreateService<IGraphicsrouteinfService>().SaveDTOs(ListGraphicspointsinfDTO);
            IsGraphicEditSave = true;

            //DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！");

        }
        /// <summary>
        /// 加载所有测点定义信息（含交换机）
        /// </summary>
        /// <param name="type">加载类型（0：所有测点，1：分站，2：所有传感器，3：交换机，4：开关量，5：模拟量）</param>
        /// <returns></returns>
        public DataTable LoadAllpointDef(string type)
        {
            DataTable dtRvalue = new DataTable();
            var request = new LoadAllpointDefByTypeRequest() { Type = type };
            var resposne = graphicsbaseinfService.LoadAllpointDefByType(request);
            dtRvalue = resposne.Data;

            return dtRvalue;
        }

        /// <summary>
        /// 加载所有视频测点
        /// </summary>
        /// <returns></returns>
        public List<V_DefInfo> LoadAllVideoPointDef()
        {
            var response = vdefService.GetAllVideoDefCache();
            var vdefinfos = response.Data;
            return vdefinfos;
        }

        public List<JC_LargedataAnalysisConfigInfo> LoadAllAnalysisConfigInfo()
        {
            var response = analysisconfigService.GetAllLargeDataAnalysisConfigList(new Sys.Safety.Request.JC_Largedataanalysisconfig.LargedataAnalysisConfigGetListRequest());
            var analysisconfiginfos = response.Data;
            return analysisconfiginfos;
        }

        /// <summary>
        /// 根据图形名称获取图形DTO对象
        /// </summary>
        /// <param name="GraphName"></param>
        /// <returns></returns>
        public GraphicsbaseinfInfo getGraphicDto(string GraphName)
        {
            var request = new GetGraphicsbaseinfByNameRequest() { GraphName = GraphName };
            var response = graphicsbaseinfService.GetGraphicsbaseinfByName(request);
            return response.Data;
        }
        /// <summary>
        /// 获取所有图形文件
        /// </summary>
        /// <returns></returns>
        public IList<GraphicsbaseinfInfo> getAllGraphicDto()
        {
            IList<GraphicsbaseinfInfo> GraphicsbaseinfDTOs;
            try
            {
                var request = new GraphicsbaseinfGetListRequest();
                request.PagerInfo.PageIndex = 1;
                request.PagerInfo.PageSize = int.MaxValue;
                var response = graphicsbaseinfService.GetGraphicsbaseinfList(request);
                GraphicsbaseinfDTOs = response.Data;
            }
            catch
            {
                //失败后再重新获取一次
                var request = new GraphicsbaseinfGetListRequest();
                request.PagerInfo.PageIndex = 1;
                request.PagerInfo.PageSize = int.MaxValue;
                var response = graphicsbaseinfService.GetGraphicsbaseinfList(request);
                GraphicsbaseinfDTOs = response.Data;
            }
            return GraphicsbaseinfDTOs;
        }
        /// <summary>
        /// 从数据库读取地图文件到本地
        /// </summary>
        public void LoadGraphicsInfo()
        {

            string PathStr = Application.StartupPath + "\\mx\\dwg";
            string[] files = Directory.GetFiles(PathStr);
            bool isexit = false;
            DataTable dt = new DataTable();
            var resposne = graphicsbaseinfService.LoadGraphicsInfo();
            dt = resposne.Data;

            string distFileName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Type"].ToString() == "0" || dt.Rows[i]["Type"].ToString() == "3")//动态图、组态图进行读取，拓扑图无底图
                    {
                        isexit = false;
                        for (int j = 0; j < files.Length; j++)
                        {
                            if (files[j].ToString().Substring(files[j].ToString().LastIndexOf("\\") + 1) == dt.Rows[i]["GraphName"].ToString())
                            {
                                isexit = true;
                                FileInfo fileinfo = new FileInfo(files[j]);
                                if (fileinfo.CreationTime < DateTime.Parse(dt.Rows[i]["Timer"].ToString()))
                                {
                                    File.Delete(files[j]);
                                    distFileName = Application.StartupPath + "\\mx\\dwg\\" + dt.Rows[i]["GraphName"].ToString();
                                    File.WriteAllBytes(distFileName, Convert.FromBase64String(dt.Rows[i]["GraphData"].ToString()));
                                }
                            }
                        }
                        if (!isexit)
                        {
                            distFileName = Application.StartupPath + "\\mx\\dwg\\" + dt.Rows[i]["GraphName"].ToString();
                            File.WriteAllBytes(distFileName, Convert.FromBase64String(dt.Rows[i]["GraphData"].ToString()));
                        }
                    }
                }
            }
            isexit = false;
            for (int j = 0; j < files.Length; j++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (files[j].ToString().Substring(files[j].ToString().LastIndexOf("\\") + 1) == dt.Rows[i]["GraphName"].ToString())
                    {
                        isexit = true;
                    }
                }
                if (!isexit)//删除本地不存在的图形文件
                {
                    File.Delete(files[j]);
                }
            }

        }
        /// <summary>
        /// 加载图形信息（线程调用）
        /// </summary>
        public void LoadGraphicsInfo1()
        {
            string PathStr = Application.StartupPath + "\\mx\\dwg";
            string[] files = Directory.GetFiles(PathStr);
            bool isexit = false;

            DataTable dt = new DataTable();
            var respsone = graphicsbaseinfService.LoadGraphicsInfo();
            dt = respsone.Data;
            string distFileName = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Type"].ToString() == "0")//动态图进行读取，拓扑图无底图
                    {
                        isexit = false;
                        for (int j = 0; j < files.Length; j++)
                        {
                            if (files[j].ToString().Substring(files[j].ToString().LastIndexOf("\\") + 1) == dt.Rows[i]["GraphName"].ToString())
                            {
                                isexit = true;
                                FileInfo fileinfo = new FileInfo(files[j]);
                                if (fileinfo.CreationTime < DateTime.Parse(dt.Rows[i]["Timer"].ToString()))
                                {
                                    File.Delete(files[j]);
                                    distFileName = Application.StartupPath + "\\mx\\dwg\\" + dt.Rows[i]["GraphName"].ToString();
                                    File.WriteAllBytes(distFileName, Convert.FromBase64String(dt.Rows[i]["GraphData"].ToString()));
                                }
                            }
                        }
                        if (!isexit)
                        {
                            distFileName = Application.StartupPath + "\\mx\\dwg\\" + dt.Rows[i]["GraphName"].ToString();
                            File.WriteAllBytes(distFileName, Convert.FromBase64String(dt.Rows[i]["GraphData"].ToString()));
                        }
                    }
                }
            }
            isexit = false;
            for (int j = 0; j < files.Length; j++)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (files[j].ToString().Substring(files[j].ToString().LastIndexOf("\\") + 1) == dt.Rows[i]["GraphName"].ToString())
                    {
                        isexit = true;
                    }
                }
                if (!isexit)//删除本地不存在的图形文件
                {
                    File.Delete(files[j]);
                }
            }

        }
        /// <summary>
        /// 获取当前图形的类型
        /// </summary>
        /// <returns></returns>
        public short getGraphicNowType()
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_ = getGraphicDto(GraphNameNow);
            if (GraphicsbaseinfDTO_ == null)
            {
                return -1;
            }
            if (!string.IsNullOrWhiteSpace(GraphicsbaseinfDTO_.GraphId))
            {
                return GraphicsbaseinfDTO_.Type;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 根据测点名称，获取测点的绑定类型
        /// </summary>
        /// <param name="PointName"></param>
        /// <returns></returns>
        public short getPointType(string PointName)
        {
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_ = getGraphicDto(GraphNameNow);

            var request = new Get1GraphBindTypeRequest();
            request.GraphId = GraphicsbaseinfDTO_.GraphId;
            request.PointId = PointName;
            var resposne = graphicspointsinfService.Get1GraphBindType(request);

            return resposne.Data;
        }
        /// <summary>
        /// 获取测点名称对应的dto对象
        /// </summary>
        /// <param name="PointName"></param>
        /// <returns></returns>
        public GraphicspointsinfInfo getPointDto(string PointName)
        {
            GraphicspointsinfInfo GraphicspointsinfDTO_ = new GraphicspointsinfInfo();
            var getGraphicspointsinfByPointRequest = new GetGraphicspointsinfByPointRequest() { PointId = PointName };
            var getGraphicspointsinfByPointResponse = graphicspointsinfService.GetGraphicspointsinfByPoint(getGraphicspointsinfByPointRequest);
            return getGraphicspointsinfByPointResponse.Data;
        }
        /// <summary>
        /// 返回当前打开图形下所有测点信息
        /// </summary>
        /// <returns></returns>
        public DataTable getAllGraphPoint(string type)
        {
            DataTable dtRvalue = new DataTable();
            GraphicsbaseinfInfo GraphicsbaseinfDTO_ = new GraphicsbaseinfInfo();
            GraphicsbaseinfDTO_ = getGraphicDto(GraphNameNow);
            if (GraphicsbaseinfDTO_ != null)
            {
                var getAllGraphPointRequest = new GetAllGraphPointRequest() { Type = type, GraphId = GraphicsbaseinfDTO_.GraphId };
                var getAllGraphPointResponse = graphicsbaseinfService.GetAllGraphPoint(getAllGraphPointRequest);
                dtRvalue = getAllGraphPointResponse.Data;
            }
            return dtRvalue;
        }
        /// <summary>
        /// 获取所有测点的显示隐藏状态
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public List<string> getAllGraphPointDis(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            List<string> MapLayersList = new List<string>();
            string scriptCmd = "GetAllPointDis()";
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return MapLayersList;//如果还没有打开图形或网页
            string allMapLayers = mx.CurViewEvaluateJavaScript(scriptCmd);
            MapLayersList = allMapLayers.Split('|').ToList<string>();
            return MapLayersList;
        }

        /// <summary>
        /// 获取应急联动图形
        /// </summary>
        /// <returns></returns>
        public GraphicsbaseinfInfo GetEmergencyLinkageGraphics()
        {
            var response = graphicsbaseinfService.GetEmergencyLinkageGraphics();
            return response.Data;
        }


        /// <summary>
        /// 获取系统默认图形
        /// </summary>
        /// <returns></returns>
        public GraphicsbaseinfInfo GetSystemtDefaultGraphics(short type)
        {
            var request = new GetSystemtDefaultGraphicsRequest() { Type = type };
            var response = graphicsbaseinfService.GetSystemtDefaultGraphics(request);
            return response.Data;
        }


        /// <summary>
        /// 获取用户自定义图形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GraphicsbaseinfInfo GetUserDefinedGraphicsByType(short type)
        {
            var request = new GetUserDefinedGraphicsByTypeRequest() { Type = type };
            var response = graphicsbaseinfService.GetUserDefinedGraphicsByType(request);
            return response.Data;
        }

        /// <summary>
        /// 修改用户自定义图形
        /// </summary>
        /// <param name="bz3"></param>
        /// <param name="graphId"></param>
        public void UpdateSystemDefaultGraphics(string bz3, string graphId)
        {
            var request = new UpdateSystemDefaultGraphicsRequest();
            request.Bz3 = bz3;
            request.GraphId = graphId;
            var response = graphicsbaseinfService.UpdateSystemDefaultGraphics(request);
        }

        /// <summary>
        /// 修改应急联动图形
        /// </summary>
        /// <param name="graphId"></param>
        public void UpdateEmergencyLinkageGraphicsRequest(string graphId)
        {
            var request = new UpdateEmergencyLinkageGraphicsRequest();
            request.GraphId = graphId;
            var responsoe = graphicsbaseinfService.UpdateEmergencyLinkageGraphics(request);
        }


        /// <summary>
        /// 绘制圆形
        /// </summary>
        /// <param name="mx"></param>
        public void DoDrawingCircle(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.DrawingCircle()");
        }

        /// <summary>
        /// 绘制多边形
        /// </summary>
        /// <param name="mx"></param>
        public void DoDrawingPolygon(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.DrawingPolygon()");
        }

        /// <summary>
        /// 清除图形
        /// </summary>
        /// <param name="mx"></param>
        public void DoClearDrawing(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.ClearDrawingGraphics()");
        }

        /// <summary>
        /// 是否有绘制其它图形
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public string DoOtherDrawingGraphics(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return "";
            return mx.CurViewEvaluateJavaScript("mas.DrawArea.DoOtherDrawingGraphics()");
        }

        /// <summary>
        /// 保存图形的数据
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public string DoSaveDrawing(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return "";
            return mx.CurViewEvaluateJavaScript("mas.DrawArea.SaveDrawingData()");
        }

        /// <summary>
        /// 绘制图形在界面刷新状态
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public void DoDrawingGraphicsInRefresh(Axmetamap2dLib.AxMetaMapX2D mx, string json, string type)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.DrawingGraphicsInRefresh('" + json + "','" + type + "')");
        }
        /// <summary>
        /// 绘制图形并传入名称
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        public void DoDrawinggGraphicsAndName(Axmetamap2dLib.AxMetaMapX2D mx, string json, string type, string name, string areaId)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.DrawinggGraphicsAndName('" + json + "','" + type + "','" + name + "','" + areaId + "')");
        }
        /// <summary>
        /// 绘制图形在界面编辑状态
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="json"></param>
        /// <param name="type"></param>
        public void DoDrawingGraphicsInEdit(Axmetamap2dLib.AxMetaMapX2D mx, string json, string type)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.DrawinggGraphicsInEdit('" + json + "','" + type + "')");
        }

        /// <summary>
        /// 获取地图上所有测点的设备名称
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public string DoGetMapPointNameAndDevName(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return "";
            return mx.CurViewEvaluateJavaScript("mas.DrawArea.GetMapPointNameAndDevName()");
        }

        /// <summary>
        /// 移除右键菜单
        /// </summary>
        /// <param name="mx"></param>
        public void DoRemoveMapContextMenu(Axmetamap2dLib.AxMetaMapX2D mx)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;
            mx.CurViewEvaluateJavaScript("mas.DrawArea.RemoveAreaContextMenu()");
        }
        /// <summary>
        /// 设置是否开启坐标拾取功能
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="state"></param>
        public void SetCoordinatePickUpEditFlag(Axmetamap2dLib.AxMetaMapX2D mx, string state)
        {
            //metamap2dLib.IMetaMapView view = mx.CurVisibleView();
            //if (view == null) return;//如果还没有打开图形或网页            
            mx.CurViewEvaluateJavaScript("SetCoordinatePickUpEditFlag('" + state + "')");
        }
        /// <summary>
        /// 根据坐标自动移动图上的点的位置
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void CoordinatePickUpAddPoint(Axmetamap2dLib.AxMetaMapX2D mx, string x, string y)
        {
            mx.CurViewEvaluateJavaScript("CoordinatePickUp_AddPoint('" + x + "','" + y + "')");
        }
        /// <summary>
        /// 打开SVG图形
        /// </summary>
        /// <param name="mx"></param>
        /// <param name="svgName"></param>
        public void OpenSVG(Axmetamap2dLib.AxMetaMapX2D mx, string svgName)
        {
            mx.CurViewEvaluateJavaScript("LoadBgSVG('" + "mx/dwg/" + svgName + "')");
        }
    }
}
