/*
* SVG图元管理API
*/

//测点覆盖物对象
var SVGpointVectorLayerArr = [];

//拓扑图测点右键信息-----------------------------------
var SVGPnt_menu1 = new mxLib.MenuItem("删除", scriptBaseDir + "mxImg/star.png", "", function (e) {

    if (confirm("确定要删除吗？")) {
        var overlay = e.view.SVGPnt_contextMenu1.targetObject;
        DelPoint(overlay);
    }
});
var SVGPnt_menu2 = new mxLib.MenuItem("编辑", "", "", function (e) {
    var overlay = e.view.SVGPnt_contextMenu1.targetObject;
    //    //        overlay.enableEditing();
    EditPoint(overlay);
});
var SVGPnt_menu3 = new mxLib.MenuItem("开始动画", "", "", function (e) {
    var overlay = e.view.SVGPnt_contextMenu1.targetObject;
    //    //        overlay.enableEditing();
    //    EditPoint(overlay);
    overlay.beginAnmi();
});
var SVGPnt_menu4 = new mxLib.MenuItem("结束动画", "", "", function (e) {
    var overlay = e.view.SVGPnt_contextMenu1.targetObject;
    //    //        overlay.enableEditing();
    //    EditPoint(overlay);
    overlay.endAnmi();
});

var SVGPnt_contextMenu1 = new mxLib.ContextMenu();
SVGPnt_contextMenu1.appendItem(SVGPnt_menu1);
SVGPnt_contextMenu1.appendItem(SVGPnt_menu2);
SVGPnt_contextMenu1.appendItem(SVGPnt_menu3);
SVGPnt_contextMenu1.appendItem(SVGPnt_menu4);

//添加点到地图
function AddSVGPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, x, y, width, height, graphType, TurnToPage, transformDeg) {

    //判断是否已添加此点，如果已添加，则不进行添加
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.Marker || allOverlay[i] instanceof mxLib.VectorLayer) {
            try {
                if (allOverlay[i].getJcdName()) {
                    if (allOverlay[i].getJcdName() == name) {
                        console.log(name + "已存在,进行重命名");
                        //点已存在，进行重新命名
                        var index = 1;
                        for (var j = 0; j < allOverlay.length; j++) {
                            if (allOverlay[j] instanceof mxLib.RichMarker || allOverlay[j] instanceof mxLib.VectorLayer) {
                                if (allOverlay[j].getJcdName()) {
                                    if (allOverlay[j].getJcdName().indexOf(name + "￣" + index.toString()) >= 0) {
                                        //找到之后，将下标加1,重新寻找
                                        index++;
                                        j = -1;
                                    }
                                    else {
                                        continue;
                                    }
                                }
                            }
                        }
                        name = name + "￣" + index.toString();
                        console.log("点已存在，重命名为：" + name);
                        break;
                    }
                }
            } catch (e) {
            }
        }
    }
    var point = new mxLib.Point(parseFloat(x), parseFloat(y));
    var bounds = map.getBounds(true);
    var w = parseInt(map.pixelDistToPointDist(80).toString());
    var h = parseInt(map.pixelDistToPointDist(80).toString());
    if (width > 0 && height > 0) {
        w = width;
        h = height;
    }
    var left = point.x;
    var top = point.y;
    var vlBounds = new mxLib.Bounds(left, top, left + w, top - h);
    var svgUrl = "";
    //获取图元
    svgUrl = "mx/Svg/" + GrapUnitName + ".svg";
    //var disZoomLevel = zoomLevel;
    //if (IsGraphicEdit) {
    //    disZoomLevel = 0; //如果是编辑状态，不启用SVG显示级别
    //}

    var zoomLevelArr = new Array();
    zoomLevelArr = zoomLevel.split("$");
    var disMinZoomLevel = parseInt(zoomLevelArr[0]);
    var disMaxZoomLevel = parseInt(zoomLevelArr[1]);
    //if (IsGraphicEdit) {
    //    disMinZoomLevel = 0;
    //    disMaxZoomLevel = 0;
    //}
    var enableDrag = IsGraphicEdit;
    
    var vl = new mxLib.VectorLayer(svgUrl, vlBounds, { enableDragging: enableDrag, embedEvents: false, minZoom: parseInt(disMinZoomLevel), maxZoom: parseInt(disMaxZoomLevel) }); //,autoHeight:true

    //console.log("11");
    //设置设备Id
    var guid = new GUID();
    vl.setPointId(guid.newGUID());
    //设置设备属性
    vl.setJcdName(name);
    //设置点安装位置
    vl.setJcdWz(wz);
    //设置点设备类型
    vl.setJcdDevName(devname);
    vl.setBindUnitName(GrapUnitName);
    vl.setBindTypeName(type);
    //设置点的显示级别
    vl.setZoomLevel(zoomLevel);
    //设置点的动画状态
    vl.setanimationState(animationState);
    //设置点的高度和宽度
    vl.setPointWidth(w);
    vl.setPointHeight(h);
    //设置页面跳转
    vl.setTurnToPage(TurnToPage);
    

    map.addOverlay(vl);

    //设置旋转角度  20171226
    vl.setTransformValue(transformDeg);
    vl.setTransform(transformDeg);

    vl.addEventListener("load", function () {
        //vl.snap().select("#Text_0").attr("text", name);
    });
    //覆盖物移动开始
    vl.addEventListener("ondragstart", function (e) {
        mxLib.View.callOutCommand("setMapEditSave", "false");
    });
    //双击事件
    vl.addEventListener("ondblclick", function (e) {
        EditPoint(this);
    });
    //加载事件
    vl.addEventListener("load", function () {
        //svg加载成功后就开启动画
        if (this.AnmiState) {
            this.beginAnmi();
        }
        else {
            this.endAnmi();
        }
    });

    if (IsGraphicEdit) { //如果当前处于编辑状态       
        vl.addContextMenu(SVGPnt_contextMenu1); //添加右键
    }
    else {//如果当前处于运行状态
        vl.addContextMenu(richpointRconMenuInRunState);
    }
    //加到对象数组中
    var pointObj = {};
    pointObj.pointName = name;
    pointObj.pointWz = vl.getJcdWz();
    pointObj.pointDevName = vl.getJcdDevName();
    pointObj.bindTypeName = vl.getBindTypeName();
    pointObj.bindUnitName = GrapUnitName;
    pointObj.pointPos = vl.getBounds().getLeftTop();
    pointObj.width = w;
    pointObj.height = h;
    pointObj.vl = vl;
    SVGpointVectorLayerArr.push(pointObj);
}
//保存所有拓扑关系
function saveSVGPoint() {
    var arrPoint = new Array();
    var strPoint = "";
    var allOverlay = map.getOverlays();

    //所有SVG设备
    strPoint1 = "";
    for (var i = 0; i < SVGpointVectorLayerArr.length; i++) {
        strPoint1 = strPoint1 + SVGpointVectorLayerArr[i].pointName + "&" + SVGpointVectorLayerArr[i].type + "&"
             + SVGpointVectorLayerArr[i].pointPos.x + "," + SVGpointVectorLayerArr[i].pointPos.y + "|";
    }
    strPoint1 = strPoint1.substring(0, strPoint1.length - 1);
    console.log(strPoint1);
}



