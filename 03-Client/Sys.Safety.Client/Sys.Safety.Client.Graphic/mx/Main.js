/**
* @file GIS二次开发平台
*
* @author luoch
*/

/*
* 地图二次开发入口文件。
*/

//console.log("main.js");
//var map = new mxLib.Map();
map = new mxLib.Map("mmap");

//加入导航条
var naviCtrl = new mxLib.NavigationControl();
naviCtrl.setOffset(new mxLib.Size(10, 20));
naviCtrl.setAnchor(MX_ANCHOR_TOP_LEFT);

//加入鹰眼功能
var ovctrl = new mxLib.OverviewMapControl();
map.addControl(ovctrl);

//运行的时候才允许左键事件
map.enableLeftButtonPan();
map.setWheelCapture(true);
try {
    map.mapModel.enableContinuousZoom = true; //(是否开启连续缩放动画)
    map.mapModel.zoomerDuration = 500; //(动画持续多少毫秒)
    map.mapModel.zoomerFps = 18; //(一秒多少帧)
}
catch (e) {

}


/**
* 加载SVG背景图片(SVG图片方式，图形不能放大缩小)
* @class LoadBgSVG 
* @param {svgName} 打开的svg文件名称
* @example 参考示例：
* LoadBgSVG('m.svg');
*/
function LoadBgSVG(svgName) {
    if (mxLib.View.getExtData("openimageurl") == "true") {
        mxLib.View.setExtData("openimageurl", "");

        var bounds = map.getBounds(true);

        var vl = new mxLib.VectorLayer(svgName, bounds, { embedEvents: false, pointerEvents: false });
        map.addOverlay(vl);
        map.setCenter(bounds.getCenter());
    }
    else {
        map.addControl(naviCtrl);
    }
}
//LoadBgSVG("m.svg");
mxLib.View.callOutCommand("LoadBgSVG", "");


//图形编辑状态
var IsGraphicEdit;




//map.setWheelCapture(true);
//map.mapModel.enableContinuousZoom = true; //(是否开启连续缩放动画)
//map.mapModel.zoomerDuration = 500; //(动画持续多少毫秒)
//map.mapModel.zcu oomerFps = 10; //(一秒多少帧)

/**
* 设置图形中心点坐标
* @class UsersetMapCenter 
* @param {x} 中心点X坐标
* @param {y} 中心点Y坐标
* @example 参考示例：
* UsersetMapCenter('104.52940017572882', '24.634454381220216');
*/
function UsersetMapCenter(x, y) {
    var point = new mxLib.Point(parseFloat(x), parseFloat(y));
    map.setCenter(point);
}
/**
* 设置图形缩放级别
* @class UsersetMapZoom 
* @param {zoommin} 地图缩放最小级别
* @param {zoommax} 地图缩放最大级别
* @param {zoomint} 地图缩放初始级别 
* @example 参考示例：
* UsersetMapZoom('4','16','12');
*/
function UsersetMapZoom(zoommin, zoommax, zoominit) {
    //map.setZoom(parseInt(zoomint));
    //设置缩放级别(最小级别，最大级别，起始级别，暂无用)
    map.setZoomRange(parseInt(zoommin), parseInt(zoommax));
    map.setZoom(parseInt(zoominit));
}

//地图缩放事件完毕
map.addEventListener("zoomend", function () {
    //mxLib.Util.toastSuccess("当前缩放级别是:" + map.getZoom(), { align: "center" });
    // console.log("当前缩放级别是:" + map.getZoom());
    /**
* 回调_地图缩放事件
* @class callOutCommand_MapZoomEnd 
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "MapZoomEnd":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "MapZoomEnd") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "MapZoomEnd") {
*          //这里进行实现
*      }
*   }
*/
    mxLib.View.callOutCommand("MapZoomEnd", "");
});
//地图单击
map.addEventListener("click", function (e) {
    //mxLib.Util.toastSuccess("单击地图", { align: "center" });
    //console.log("单击地图");
    var x = e.clientX;
    var y = e.clientY;
    var point = map.pixelToPoint(x, y);
    var areaId = "";
    //mxLib.Util.toastInfo(point.x + "," + point.y, { delay: 30000 });
    /**
* 回调_地图单击事件
* @class callOutCommand_MapClick
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "MapClick":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "MapClick") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "MapClick") {
*          //这里进行实现
*      }
*   }
*/
    
    mxLib.View.callOutCommand("MapClick", point.x + "," + point.y + "," + areaId);
});
//地图双击
map.addEventListener("dblclick", function (e) {
    //mxLib.Util.toastSuccess("单击地图", { align: "center" });
    //console.log("单击地图");
    var x = e.clientX;
    var y = e.clientY;
    var point = map.pixelToPoint(x, y);
    var areaId = "";
    //mxLib.Util.toastInfo(point.x + "," + point.y, { delay: 30000 });
    /**
* 回调_地图单击事件
* @class callOutCommand_MapClick
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "MapClick":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "MapClick") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "MapClick") {
*          //这里进行实现
*      }
*   }
*/
    if (CoordinatePickUp_EditFlag) {//如果启用了坐标拾取，才进入执行，在地图上加标注
        var x = e.clientX;
        var y = e.clientY;
        var point = map.pixelToPoint(x, y);
        if (CoordinatePickUp_PointNow._opts) {
            CoordinatePickUp_PointNow.setPosition(point);
        }
        else {
            CoordinatePickUp_AddPoint(point.x, point.y);
        }
        //mxLib.Util.toastInfo(point.x + "," + point.y, { delay: 30000 });
        console.log(point.x + "," + point.y);

        for (var i = 0; i < AreaList.length; i++) {
            //判断测点是否在多边形内
            if (mxLib.GeoUtils.isPointInPolygon(point, AreaList[i].overlay)) {
                areaId = AreaList[i].areaId;
            }
        }
    }

    mxLib.View.callOutCommand("MapDblClick", point.x + "," + point.y + "," + areaId);
});

//地图移动事件完毕
map.addEventListener("dragend", function () {
    //mxLib.Util.toastSuccess("拖动完成", { align: "center" });
    //console.log("拖动完成");
    /**
* 回调_地图拖动完成事件
* @class callOutCommand_MapDragEnd
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "MapDragEnd":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "MapDragEnd") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "MapDragEnd") {
*          //这里进行实现
*      }
*   }
*/
    mxLib.View.callOutCommand("MapDragEnd", "");
});
//地图移动事件开始
map.addEventListener("dragstart", function () {
    //mxLib.Util.toastSuccess("拖动完成", { align: "center" });
    //console.log("拖动完成");
    /**
* 回调_地图拖动开始事件
* @class callOutCommand_MapDragStart
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "MapDragStart":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "MapDragStart") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "MapDragStart") {
*          //这里进行实现
*      }
*   }
*/
    mxLib.View.callOutCommand("MapDragStart", "");
});


/**
* 设置默认的导航条是否显示
* @class NavigationControlDis 
* @param {State} 显示状态 true代表显示，false代表不显示
* @example 参考示例：
* NavigationControlDis(true);
*/
function NavigationControlDis(State) {
    if (State) {
        naviCtrl.show();
    }
    else {
        naviCtrl.hide();
    }
}

/**
* 设置图形的编辑状态
* @class SetGraphicEdit 
* @param {State} 编辑状态 true代表为编辑状态，false代表为运行状态
* @example 参考示例：
* SetGraphicEdit(true);
*/
function SetGraphicEdit(State) {
    if (State) {
        IsGraphicEdit = true;
    }
    else {
        IsGraphicEdit = false;
    }
}
//设置图形的编辑状态 ------------------------------------------

//自定义 Circle 属性及方法 ------------------------------------------
mxLib.Circle.prototype.setJcdName = function (name) {
    this.jcname = name;
}
//获取监测点名称
mxLib.Circle.prototype.getJcdName = function (name) {
    return this.jcname;
}
//自定义 Circle 属性及方法 ------------------------------------------



//自定义 RichMarker 属性及方法 ------------------------------------------
//设置测点Id
mxLib.RichMarker.prototype.setPointId = function (pointId) {
    this.pointId = pointId;
}
//设置测点Id
mxLib.RichMarker.prototype.getPointId = function () {
    return this.pointId;
}
//设置监测点名称
mxLib.RichMarker.prototype.setJcdName = function (name) {
    this.jcname = name;
}
//获取监测点名称
mxLib.RichMarker.prototype.getJcdName = function (name) {
    return this.jcname;
}
//设置监测点安装位置
mxLib.RichMarker.prototype.setJcdWz = function (name) {
    this.jcwz = name;
}
//获取监测点安装位置
mxLib.RichMarker.prototype.getJcdWz = function (name) {
    return this.jcwz;
}
//设置监测点设备类型
mxLib.RichMarker.prototype.setJcdDevName = function (name) {
    this.jcdevname = name;
}
//获取监测点设备类型
mxLib.RichMarker.prototype.getJcdDevName = function (name) {
    return this.jcdevname;
}

mxLib.RichMarker.prototype.setText = function (txt) {
    if ($(this.getDomElement()).children("div").find(".RichMarker_Text")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Text").html("<nobr>" + txt + "</nobr>");
    }
}
mxLib.RichMarker.prototype.setTextColor = function (color) {
    if ($(this.getDomElement()).children("div").find(".RichMarker_Text")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Text").css("color", color);
    }
}

//myRichMarker.setText("12.36m/s")
mxLib.RichMarker.prototype.setImage = function (src) {
    if ($(this.getDomElement()).children("div").find(".Marker")) {
        $(this.getDomElement()).children("div").find(".Marker").css("background-image", "url('" + src + "')");
    }
}
mxLib.RichMarker.prototype.setImageUrl = function (src) {
    if ($(this.getDomElement()).children("div").find(".RichMarker_Image")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Image").attr("src", src);
    }
}
mxLib.RichMarker.prototype.setImage0 = function (srcL, srcM, srcR) {
    if ($(this.getDomElement()).children("div").find(".RichMarker_Left")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Left").attr("src", srcL);
    }
    if ($(this.getDomElement()).children("div").find(".RichMarker_Middle")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Middle").css("background-image", "url('" + srcM + "')");
    }
    if ($(this.getDomElement()).children("div").find(".RichMarker_Rright")) {
        $(this.getDomElement()).children("div").find(".RichMarker_Rright").attr("src", srcR);
    }
}
mxLib.RichMarker.prototype.setImageRun = function () {
    if ($(this.getDomElement()).children("div").find(".AmniStop")) {
        $(this.getDomElement()).children("div").find(".AmniStop").attr("class", "AmniRun");
    }
}
mxLib.RichMarker.prototype.setImageStop = function () {
    if ($(this.getDomElement()).children("div").find(".AmniRun")) {
        $(this.getDomElement()).children("div").find(".AmniRun").attr("class", "AmniStop");
    }
}
//设置旋转角度  20171226
//设置旋转角度值
mxLib.RichMarker.prototype.setTransformValue = function (name) {
    this.transformValue = name;
}
//获取旋转角度值
mxLib.RichMarker.prototype.getTransformValue = function (name) {
    return this.transformValue;
}
mxLib.RichMarker.prototype.setTransform = function (degree) {
    $(this.getDomElement()).css("transform", "rotate(" + degree + "deg)");
}
//设置填充图元高度
mxLib.RichMarker.prototype.setHeight = function (height) {
    if ($(this.getDomElement()).children("div").find(".FillPix")) {
        $(this.getDomElement()).children("div").find(".FillPix").height(height);
    }
}
//设置监测点图元名称
mxLib.RichMarker.prototype.setBindUnitName = function (name) {
    this.typename = name;
}
//获取监测点图元名称
mxLib.RichMarker.prototype.getBindUnitName = function () {
    return this.typename;
}
//设置监测点类型名称
mxLib.RichMarker.prototype.setBindTypeName = function (name) {
    this.bindtypename = name;
}
//获取监测点类型名称
mxLib.RichMarker.prototype.getBindTypeName = function () {
    return this.bindtypename;
}
//设置监测点的显示缩放等级
mxLib.RichMarker.prototype.setZoomLevel = function (name) {
    this.zoomlevel = name;
}
//获取监测点显示缩放等级
mxLib.RichMarker.prototype.getZoomLevel = function () {
    return this.zoomlevel;
}
//设置动画状态
mxLib.RichMarker.prototype.setanimationState = function (name) {
    this.animationstate = name;
}
//获取动画状态
mxLib.RichMarker.prototype.getanimationState = function () {
    return this.animationstate;
}
//设置跳转的页面
mxLib.RichMarker.prototype.setTurnToPage = function (name) {
    this.TurnToPage = name;
}
//获取跳转的页面
mxLib.RichMarker.prototype.getTurnToPage = function () {
    return this.TurnToPage;
}
//设置监测点的当前状态
mxLib.RichMarker.prototype.setPointState = function (name) {
    this.pointstate = name;
}
//获取监测点的当前状态
mxLib.RichMarker.prototype.getPointState = function () {
    return this.pointstate;
}
//设置监测点的设备状态
mxLib.RichMarker.prototype.setPointStateText = function (name) {
    this.pointstatetext = name;
}
//获取监测点的设备状态
mxLib.RichMarker.prototype.getPointStateText = function () {
    return this.pointstatetext;
}
//设置监测点的数据状态
mxLib.RichMarker.prototype.setPointDataText = function (name) {
    this.pointdatatext = name;
}
//获取监测点的数据状态
mxLib.RichMarker.prototype.getPointDataText = function () {
    return this.pointdatatext;
}
//设置监测点的ToolTip内容
mxLib.RichMarker.prototype.setPointToolTip = function (name) {
    this.pointToolTip = name;
}
//获取监测点的ToolTip内容
mxLib.RichMarker.prototype.getPointToolTip = function () {
    return this.pointToolTip;
}

//自定义 RichMarker 属性及方法 ------------------------------------------

//自定义 VectorLayer 属性及方法 ------------------------------------------

//设置监测点Id
mxLib.VectorLayer.prototype.setPointId = function (pointId) {
    this.pointId = pointId;
}

//获取监测点Id
mxLib.VectorLayer.prototype.getPointId = function () {
    return this.pointId;
}

mxLib.VectorLayer.prototype.setJcdName = function (name) {
    this.jcname = name;
}
//获取监测点名称
mxLib.VectorLayer.prototype.getJcdName = function () {
    return this.jcname;
}

//设置监测点图元名称
mxLib.VectorLayer.prototype.setBindUnitName = function (name) {
    this.typename = name;
}
//获取监测点图元名称
mxLib.VectorLayer.prototype.getBindUnitName = function () {
    return this.typename;
}
//设置监测点类型名称
mxLib.VectorLayer.prototype.setBindTypeName = function (name) {
    this.bindtypename = name;
}
//获取监测点类型名称
mxLib.VectorLayer.prototype.getBindTypeName = function () {
    return this.bindtypename;
}
//设置监测点的显示缩放等级
mxLib.VectorLayer.prototype.setZoomLevel = function (name) {
    this.zoomlevel = name;
}
//获取监测点显示缩放等级
mxLib.VectorLayer.prototype.getZoomLevel = function () {
    return this.zoomlevel;
}
//设置动画状态
mxLib.VectorLayer.prototype.setanimationState = function (name) {
    this.animationstate = name;
}
//获取动画状态
mxLib.VectorLayer.prototype.getanimationState = function () {
    return this.animationstate;
}
//设置跳转的页面
mxLib.VectorLayer.prototype.setTurnToPage = function (name) {
    this.TurnToPage = name;
}
//获取跳转的页面
mxLib.VectorLayer.prototype.getTurnToPage = function () {
    return this.TurnToPage;
}
//设置监测点安装位置
mxLib.VectorLayer.prototype.setJcdWz = function (name) {
    this.jcwz = name;
}
//获取监测点安装位置
mxLib.VectorLayer.prototype.getJcdWz = function (name) {
    return this.jcwz;
}
//设置监测点设备类型
mxLib.VectorLayer.prototype.setJcdDevName = function (name) {
    this.jcdevname = name;
}
//获取监测点设备类型
mxLib.VectorLayer.prototype.getJcdDevName = function (name) {
    return this.jcdevname;
}
//设置监测点的当前状态
mxLib.VectorLayer.prototype.setPointState = function (name) {
    this.pointstate = name;
}
//获取监测点的当前状态
mxLib.VectorLayer.prototype.getPointState = function () {
    return this.pointstate;
}
//设置监测点的当前设备状态
mxLib.VectorLayer.prototype.setPointStateText = function (name) {
    this.pointstatetext = name;
}
//获取监测点的当前设备状态
mxLib.VectorLayer.prototype.getPointStateText = function () {
    return this.pointstatetext;
}
//设置监测点的当前数据状态
mxLib.VectorLayer.prototype.setPointDataText = function (name) {
    this.pointdatatext = name;
}
//获取监测点的当前数据状态
mxLib.VectorLayer.prototype.getPointDataText = function () {
    return this.pointdatatext;
}
//设置监测点的宽度
mxLib.VectorLayer.prototype.setPointWidth = function (name) {
    this.pointwidth = name;
}
//获取监测点的宽度
mxLib.VectorLayer.prototype.getPointWidth = function () {
    return this.pointwidth;
}
//设置监测点的高度
mxLib.VectorLayer.prototype.setPointHeight = function (name) {
    this.pointheight = name;
}
//获取监测点的高度
mxLib.VectorLayer.prototype.getPointHeight = function () {
    return this.pointheight;
}
//设置监测点的ToolTip内容
mxLib.VectorLayer.prototype.setPointToolTip = function (name) {
    this.pointToolTip = name;
}
//获取监测点的ToolTip内容
mxLib.VectorLayer.prototype.getPointToolTip = function () {
    return this.pointToolTip;
}

//添加监测点绑定的设备
mxLib.VectorLayer.prototype.setSonDev = function (name) {
    if (!this.SonDevArr) {
        this.SonDevArr = [];
    }
    this.SonDevArr.push(name);
}
//删除监测点绑定的设备
mxLib.VectorLayer.prototype.delSonDev = function (name) {
    var index = this.indexOf(name);
    if (index > -1) {
        this.SonDevArr.splice(index, 1);
    }
}
mxLib.VectorLayer.prototype.beginAnmi = function () {
    this.AnmiState = true;
    if (this.getSVGDocument().getElementById('Rotate_1')) {
        this.getSVGDocument().getElementById('Rotate_1').beginElement();      
    }
    if (this.getSVGDocument().getElementById('Rotate_2')) {
        this.getSVGDocument().getElementById('Rotate_2').beginElement();
    }
    if (this.getSVGDocument().getElementById('Rotate_3')) {
        this.getSVGDocument().getElementById('Rotate_3').beginElement();
    }
    
}
mxLib.VectorLayer.prototype.endAnmi = function () {
    this.AnmiState = false;
    if (this.getSVGDocument().getElementById('Rotate_1')) {
        this.getSVGDocument().getElementById('Rotate_1').endElement();
    }
    if (this.getSVGDocument().getElementById('Rotate_2')) {
        this.getSVGDocument().getElementById('Rotate_2').endElement();
    }
    if (this.getSVGDocument().getElementById('Rotate_3')) {
        this.getSVGDocument().getElementById('Rotate_3').endElement();
    }
    
}
mxLib.VectorLayer.prototype.setText = function (txt) {
    if (this.snap().select("#Text_0")) {
        this.snap().select("#Text_0").attr("text", txt);
    }
}
mxLib.VectorLayer.prototype.setTextColor = function (color) {
    if (this.snap().select("#Text_0")) {
        this.snap().select("#Text_0").attr("fill", color);
    }
}
mxLib.VectorLayer.prototype.setBgColor = function (color) {
    if (this.snap().select('#Fill_0')) {
        this.snap().select("#Fill_0").attr("fill", color);
    }
    if (this.snap().select('#Fill_1')) {
        this.snap().select("#Fill_1").attr("fill", color);
    }
    if (this.snap().select('#Fill_2')) {
        this.snap().select("#Fill_2").attr("fill", color);
    }
    if (this.snap().select('#Fill_3')) {
        this.snap().select("#Fill_3").attr("fill", color);
    }
    if (this.snap().select('#Fill_4')) {
        this.snap().select("#Fill_4").attr("fill", color);
    }
}
//设置旋转角度  20171226
//设置旋转角度值
mxLib.VectorLayer.prototype.setTransformValue = function (name) {
    this.transformValue = name;
}
//获取旋转角度值
mxLib.VectorLayer.prototype.getTransformValue = function (name) {
    return this.transformValue;
}
mxLib.VectorLayer.prototype.setTransform = function (degree) {
    $(this.getDomElement()).css("transform", "rotate(" + degree + "deg)");
}
//自定义 VectorLayer 属性及方法 ------------------------------------------






//字符串转换成json数组
function StringToJson(jsonstr) {
    //var jsonstr = "[{'name':'a','value':1},{'name':'b','value':2}]";//json字符串格式
    var jsonarray = eval('(' + jsonstr + ')');

    return jsonarray;
}

/*------------------------------图层控制----------------------------------------*/
//获取所有图层
/**
* 获取所有图层
* @class getMapLayers 
* @returns 返回值：巷道名称列表（多个用“|”分隔），如:layer1,显示隐藏（true：表示隐藏，false：表示显示）|layer2,显示隐藏
* @example 参考示例：
* getMapLayers();
*/
function getMapLayers() {
    var sContent = "";
    var layers = map.getLayer("");
    layers = $.parseJSON(layers);
    for (var i in layers) {
        sContent += layers[i].layerName + "," + layers[i].switch + "|";
        //        if (layers[i].layerName == "巷道名称") {
        //            map.setLayerAttribute(layers[i].layerName, "DWG_LAYER_VISBLE", "false");
        //        }
    }
    if (sContent.length > 0) {
        sContent = sContent.substring(0, sContent.length - 1);
    }
    return sContent;
}
//图层隐藏
/**
* 图层隐藏
* @class LayerHidden 
* @param {layerName} 图层名称 图层必须是CAD图形内的图层
* @example 参考示例：
* LayerHidden('mylayerName');
*/
function LayerHidden(layerName) {
    map.setLayerAttribute(layerName, "DWG_LAYER_VISBLE", "false");
}
//图层显示
/**
* 图层显示
* @class LayerDisplay 
* @param {layerName} 图层名称 图层必须是CAD图形内的图层
* @example 参考示例：
* LayerHidden('mylayerName');
*/
function LayerDisplay(layerName) {
    map.setLayerAttribute(layerName, "DWG_LAYER_VISBLE", "true");
}
/*------------------------------图层控制----------------------------------------*/


/*------------------------------获取当前鼠标坐标----------------------------------------*/
//当前鼠标坐标
var MousePoint;

//获取当前鼠标坐标
function getMousePos(e) {
    if (e.button != 2) return;
    var x = e.clientX;
    var y = e.clientY;
    var point = map.pixelToPoint(x, y);
    MousePoint = point;
    //mxLib.Util.toastInfo(point.x + "," + point.y, { delay: 8000 });
    console.log(point.x + "," + point.y); //按F9打开调试器，点击控制台console，可查看输出
}
//地图鼠标按下事件
map.addEventListener("mousedown", getMousePos);
/*------------------------------获取当前鼠标坐标----------------------------------------*/

/*------------------------------打印及导出----------------------------------------*/
//导出图片
/**
* 导出图片
* @class mapsavePicture 
* @param {filePath} 导出图片名称 
* @param {format} 导出图片后缀名 如：PNG,JPG,BMP,GIF
* @returns 返回值：无
* @example 参考示例：
* mapsavePicture('C:\\test.png','PNG');
*/
function mapsavePicture(filePath, format) {
    map.savePicture(true, filePath, format);
}
//打印
/**
* 打印图形
* @class mappaintPicture 
* @param {bool} 是否打印图元 true:表示包含所有图元，false:只打印底图
* @returns 返回值：无
* @example 参考示例：
* mappaintPicture(true);
*/
function mappaintPicture(bool) {
    //打印图形前先移除地图上的右键菜单、否则打印出来的图形上面会出现“右键菜单”
    map.removeContextMenu(MapcontextMenu);
    map.paintPicture(bool);
}
/*------------------------------打印及导出----------------------------------------*/

/*
*生成GUID字符串
*/
function GUID() {
    this.date = new Date();   /* 判断是否初始化过，如果初始化过以下代码，则以下代码将不再执行，实际中只执行一次 */
    if (typeof this.newGUID != 'function') {   /* 生成GUID码 */
        GUID.prototype.newGUID = function () {
            this.date = new Date(); var guidStr = '';
            sexadecimalDate = this.hexadecimal(this.getGUIDDate(), 16);
            sexadecimalTime = this.hexadecimal(this.getGUIDTime(), 16);
            for (var i = 0; i < 9; i++) {
                guidStr += Math.floor(Math.random() * 16).toString(16);
            }
            guidStr += sexadecimalDate;
            guidStr += sexadecimalTime;
            while (guidStr.length < 32) {
                guidStr += Math.floor(Math.random() * 16).toString(16);
            }
            return this.formatGUID(guidStr);
        }
        /* * 功能：获取当前日期的GUID格式，即8位数的日期：19700101 * 返回值：返回GUID日期格式的字条串 */
        GUID.prototype.getGUIDDate = function () {
            return this.date.getFullYear() + this.addZero(this.date.getMonth() + 1) + this.addZero(this.date.getDay());
        }
        /* * 功能：获取当前时间的GUID格式，即8位数的时间，包括毫秒，毫秒为2位数：12300933 * 返回值：返回GUID日期格式的字条串 */
        GUID.prototype.getGUIDTime = function () {
            return this.addZero(this.date.getHours()) + this.addZero(this.date.getMinutes()) + this.addZero(this.date.getSeconds()) + this.addZero(parseInt(this.date.getMilliseconds() / 10));
        }
        /* * 功能: 为一位数的正整数前面添加0，如果是可以转成非NaN数字的字符串也可以实现 * 参数: 参数表示准备再前面添加0的数字或可以转换成数字的字符串 * 返回值: 如果符合条件，返回添加0后的字条串类型，否则返回自身的字符串 */
        GUID.prototype.addZero = function (num) {
            if (Number(num).toString() != 'NaN' && num >= 0 && num < 10) {
                return '0' + Math.floor(num);
            } else {
                return num.toString();
            }
        }
        /*  * 功能：将y进制的数值，转换为x进制的数值 * 参数：第1个参数表示欲转换的数值；第2个参数表示欲转换的进制；第3个参数可选，表示当前的进制数，如不写则为10 * 返回值：返回转换后的字符串 */GUID.prototype.hexadecimal = function (num, x, y) {
            if (y != undefined) { return parseInt(num.toString(), y).toString(x); }
            else { return parseInt(num.toString()).toString(x); }
        }
        /* * 功能：格式化32位的字符串为GUID模式的字符串 * 参数：第1个参数表示32位的字符串 * 返回值：标准GUID格式的字符串 */
        GUID.prototype.formatGUID = function (guidStr) {
            var str1 = guidStr.slice(0, 8) + '-', str2 = guidStr.slice(8, 12) + '-', str3 = guidStr.slice(12, 16) + '-', str4 = guidStr.slice(16, 20) + '-', str5 = guidStr.slice(20);
            return str1 + str2 + str3 + str4 + str5;
        }
    }
}