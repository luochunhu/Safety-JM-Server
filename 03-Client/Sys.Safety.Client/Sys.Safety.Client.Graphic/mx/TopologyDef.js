/*
*拓扑图测点加载及编辑 
*/

//测点覆盖物列表
var TBpointVectorLayerArr = [];

//所有测点路径数组
var allPointTbObjList = [];


//删除连接线
function DelTopologyTrans(overlay) {
    if (IsGraphicEdit) { //如果当前处于编辑状态       
        var tempPostion = overlay.getBounds().getCenter();
        for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
            if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                map.removeOverlay(allPointTbObjList[j].polyline);
                allPointTbObjList.splice(j, 1);
            }
        }
    }
}

//拓扑图测点右键信息-----------------------------------
var TbPnt_menu1 = new mxLib.MenuItem("删除", scriptBaseDir + "mxImg/star.png", "", function (e) {
    if (confirm("确定要删除吗？")) {
        var overlay = e.view.TbPnt_contextMenu1.targetObject;
        DelPoint(overlay);
    }
});
var TbPnt_menu2 = new mxLib.MenuItem("编辑", "", "", function (e) {
    var overlay = e.view.TbPnt_contextMenu1.targetObject;
    //        overlay.enableEditing();
    EditPoint(overlay);
});
var TbPnt_menu3 = new mxLib.MenuItem("删除连线", "", "", function (e) {
    if (confirm("确定要删除连接线吗？")) {
        var overlay = e.view.TbPnt_contextMenu1.targetObject;
        //        overlay.enableEditing();
        DelTopologyTrans(overlay);
    }
});
//风电闭锁右键菜单项-添加开关
var TbPnt_menu4 = new mxLib.MenuItem("添加开停", "", "", function (e) {
    var overlay = e.view.TbPnt_contextMenu2.targetObject;
    AddWindPowerAtresiaPoint(overlay.getJcdName(), "000000", "", "");
});
var TbPnt_menu5 = new mxLib.MenuItem("删除开停", "", "", function (e) {
    if (confirm("确定要删除吗？")) {
        var overlay = e.view.TbPnt_contextMenu3.targetObject;
        if (IsGraphicEdit) { //如果当前处于编辑状态  
            for (var k = TBpointVectorLayerArr.length - 1; k >= 0; k--) {
                if (TBpointVectorLayerArr[k].ChildPoints.indexOf(overlay.getJcdName()) >= 0) {
                    var tempValue = TBpointVectorLayerArr[k].ChildPoints.substring(TBpointVectorLayerArr[k].ChildPoints.lastIndexOf(",") + 1);
                    if (tempValue != overlay.getJcdName()) {//存在下级设备
                        //alert("存在下级设备，不能直接删除，请修改绑定的设备或者先删除下级开关量！！");
                        mxLib.Util.toastInfo("存在下级设备，不能直接删除，请修改绑定的设备或者先删除下级开关量！！", { delay: 5000 });
                        return;
                    }
                }
            }
            var tempPostion = overlay.getBounds().getCenter();
            for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                    map.removeOverlay(allPointTbObjList[j].polyline);
                    allPointTbObjList.splice(j, 1);
                }
            }
            for (var i = TBpointVectorLayerArr.length - 1; i >= 0; i--) {
                if (TBpointVectorLayerArr[i].ChildPoints) {//判断是否是配电柜，如果是配电柜，且是当前设备的上级，则删除ChildPoints中对应的当前设备编号
                    if (TBpointVectorLayerArr[i].ChildPoints.indexOf(overlay.getJcdName()) >= 0) {
                        var tempArr = TBpointVectorLayerArr[i].ChildPoints.split(',');
                        for (var k = tempArr.length - 1; k >= 0; k--) {
                            if (tempArr[k] == overlay.getJcdName()) {
                                tempArr.splice(k, 1);
                            }
                        }
                        var NewSonPointList = "";
                        for (var h = 0; h < tempArr.length; h++) {
                            if (h == tempArr.length - 1) {
                                NewSonPointList = NewSonPointList + tempArr[h];
                            }
                            else {
                                NewSonPointList = NewSonPointList + tempArr[h] + ",";
                            }
                        }
                        TBpointVectorLayerArr[i].ChildPoints = NewSonPointList;
                    }
                }
                if (TBpointVectorLayerArr[i].pointName == overlay.getJcdName()) {//如果是当前设备，则删除数组对应的对象
                    TBpointVectorLayerArr.splice(i, 1);
                }
            }
            map.removeOverlay(overlay);
        }
    }
});
var TbPnt_contextMenu1 = new mxLib.ContextMenu();
TbPnt_contextMenu1.appendItem(TbPnt_menu1);
TbPnt_contextMenu1.appendItem(TbPnt_menu2);
TbPnt_contextMenu1.appendItem(TbPnt_menu3);

var TbPnt_contextMenu2 = new mxLib.ContextMenu();
TbPnt_contextMenu2.appendItem(TbPnt_menu4);

var TbPnt_contextMenu3 = new mxLib.ContextMenu();
TbPnt_contextMenu3.appendItem(TbPnt_menu5);


//添加点到地图
function AddTBPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, x, y, width, height, graphType, TurnToPage, transformDeg) {
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
    var left = point.x;
    var top = point.y;
    var w, h;
    if (graphType == "1") {//当前类型为拓扑图，设置初始宽度
        w = 70;
        h = 70;
        if (GrapUnitName.indexOf('&字') >= 0) {
            w = 100;
            h = 20;
        }
        if (GrapUnitName.indexOf('&网') >= 0) {
            w = 1000;
            h = 10;
        }
        if (GrapUnitName.indexOf('&地') >= 0) {
            w = 450;
            h = 30;
        }
        if (GrapUnitName.indexOf('&屏') >= 0) {
            w = 80;
            h = 50;
        }
        if (GrapUnitName.indexOf('&电') >= 0) {
            w = 80;
            h = 65;
        }
        if (GrapUnitName.indexOf('&静') >= 0) {
            w = 45;
            h = 45;
        }
        if (GrapUnitName.indexOf('&接口') >= 0) {
            w = 70;
            h = 70;
        }
    }
    else {//当前类型为动态图，设置初始宽度
        w = parseInt(map.pixelDistToPointDist(60).toString());
        h = parseInt(map.pixelDistToPointDist(60).toString());
        if (GrapUnitName.indexOf('&字') >= 0) {
            w = parseInt(map.pixelDistToPointDist(100).toString());
            h = parseInt(map.pixelDistToPointDist(20).toString());
        }
        if (GrapUnitName.indexOf('&网') >= 0) {
            w = parseInt(map.pixelDistToPointDist(1000).toString());
            h = parseInt(map.pixelDistToPointDist(10).toString());
        }
        if (GrapUnitName.indexOf('&地') >= 0) {
            w = parseInt(map.pixelDistToPointDist(450).toString());
            h = parseInt(map.pixelDistToPointDist(30).toString());
        }
        if (GrapUnitName.indexOf('&屏') >= 0) {
            w = parseInt(map.pixelDistToPointDist(80).toString());
            h = parseInt(map.pixelDistToPointDist(50).toString());
        }
        if (GrapUnitName.indexOf('&电') >= 0) {
            w = parseInt(map.pixelDistToPointDist(80).toString());
            h = parseInt(map.pixelDistToPointDist(65).toString());
        }
        if (GrapUnitName.indexOf('&静') >= 0) {
            w = parseInt(map.pixelDistToPointDist(45).toString());
            h = parseInt(map.pixelDistToPointDist(45).toString());
        }
        if (GrapUnitName.indexOf('&接口') >= 0) {
            w = parseInt(map.pixelDistToPointDist(70).toString());
            h = parseInt(map.pixelDistToPointDist(70).toString());
        }
    }
    if (width > 0 && height > 0) {
        w = width;
        h = height;
    }
    var vlBounds = new mxLib.Bounds(left, top, left + w, top - h);
    var svgUrl = "";
    //获取图元
    var EnableDragging = IsGraphicEdit;
    if (GrapUnitName.indexOf("拓扑定义-") == 0) {
        EnableDragging = false;
    }
    svgUrl = "mx/Topology/" + GrapUnitName + ".svg";
    var vl = new mxLib.VectorLayer(svgUrl, vlBounds, { enableDragging: EnableDragging, embedEvents: false }); //,autoHeight:true
    //console.log("11");

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
        try {
            vl.snap().select("#Text_0").attr("text", name);
        }
        catch (e)
        { }
    });
    //双击事件
    vl.addEventListener("ondblclick", function (e) {
        EditPoint(this);
    });

    //覆盖物移动开始
    vl.addEventListener("ondragstart", function (e) {
        this.oldPoint = this.getBounds().getCenter();
        mxLib.View.callOutCommand("setMapEditSave", "false");
    });
    //覆盖物移动时
    vl.addEventListener("ondragging", function (e) {
        this.nowPoint = this.getBounds().getCenter();
        var tempPoint = this.getBounds().getLeftTop();
        var pointName = this.getJcdName();
        for (var i = 0; i < allPointTbObjList.length; i++) {
            if (allPointTbObjList[i].points[0].equals(this.oldPoint)) {
                var sPoint = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
                var ePoint = new mxLib.Point(allPointTbObjList[i].points[allPointTbObjList[i].points.length - 1].x,
                         allPointTbObjList[i].points[allPointTbObjList[i].points.length - 1].y);
                var cPoint1;
                var cPoint2;
                if (allPointTbObjList[i].tranType == "0") {
                    cPoint1 = new mxLib.Point(sPoint.x, ((sPoint.y + ePoint.y) / 2));
                    cPoint2 = new mxLib.Point(ePoint.x, ((sPoint.y + ePoint.y) / 2));
                }
                else if (allPointTbObjList[i].tranType == "1") {
                    cPoint1 = new mxLib.Point(sPoint.x, ePoint.y);
                    cPoint2 = new mxLib.Point(sPoint.x, ePoint.y);
                }
                allPointTbObjList[i].points[0] = sPoint;
                allPointTbObjList[i].points[1] = cPoint1;
                allPointTbObjList[i].points[2] = cPoint2;
                allPointTbObjList[i].points[3] = ePoint;
            }
            if (allPointTbObjList[i].points[allPointTbObjList[i].points.length - 1].equals(this.oldPoint)) {
                var sPoint = new mxLib.Point(allPointTbObjList[i].points[0].x, allPointTbObjList[i].points[0].y);
                var ePoint = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
                var cPoint1;
                var cPoint2;
                if (allPointTbObjList[i].tranType == "0") {
                    cPoint1 = new mxLib.Point(sPoint.x, ((sPoint.y + ePoint.y) / 2));
                    cPoint2 = new mxLib.Point(ePoint.x, ((sPoint.y + ePoint.y) / 2));
                }
                else if (allPointTbObjList[i].tranType == "1") {
                    cPoint1 = new mxLib.Point(sPoint.x, ePoint.y);
                    cPoint2 = new mxLib.Point(sPoint.x, ePoint.y);
                }
                allPointTbObjList[i].points[0] = sPoint;
                allPointTbObjList[i].points[1] = cPoint1;
                allPointTbObjList[i].points[2] = cPoint2;
                allPointTbObjList[i].points[3] = ePoint;
            }
            allPointTbObjList[i].polyline.setPath(allPointTbObjList[i].points);
        }
        for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
            if (TBpointVectorLayerArr[i].pointName == pointName) {
                TBpointVectorLayerArr[i].pointPos = tempPoint;
            }
        }
        this.oldPoint = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
    });
    if (IsGraphicEdit) { //如果当前处于编辑状态
        if (GrapUnitName.indexOf("拓扑定义-") < 0) {
            if (GrapUnitName.indexOf("风电闭锁配电柜") >= 0) {
                vl.addContextMenu(TbPnt_contextMenu2); //添加右键
            }
            else if (GrapUnitName.indexOf("风电闭锁开关") >= 0) {
                vl.addContextMenu(TbPnt_contextMenu3); //添加右键
            }
            else {
                vl.addContextMenu(TbPnt_contextMenu1); //添加右键
            }
        }
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
    pointObj.ChildPoints = "";
    pointObj.width = w;
    pointObj.height = h;
    pointObj.overlay = vl;
    TBpointVectorLayerArr.push(pointObj);
}
//保存所有拓扑关系
function saveTBPoint() {
    var arrPoint = new Array();
    var strPoint = "";
    var allOverlay = map.getOverlays();
    //已经建立拓扑关系的设备
    for (var i = 0; i < allPointTbObjList.length; i++) {

        var P_PointCode = allPointTbObjList[i].sPointName;
        var S_PointCOde = allPointTbObjList[i].ePointName;
        var P_PointPos = allPointTbObjList[i].sPointPos.x + "," + allPointTbObjList[i].sPointPos.y;
        var S_PointPos = allPointTbObjList[i].ePointPos.x + "," + allPointTbObjList[i].ePointPos.y;
        var P_PointType = allPointTbObjList[i].sPointType;
        var S_PointType = allPointTbObjList[i].ePointType;

        var PointTrans = "";
        for (var j = 0; j < allPointTbObjList[i].points.length; j++) {
            PointTrans = PointTrans + allPointTbObjList[i].points[j].x + "," + allPointTbObjList[i].points[j].y + "#";
        }
        PointTrans = PointTrans.substring(0, PointTrans.length - 1);

        strPoint = strPoint + P_PointCode + "&" + S_PointCOde + "&" + P_PointType + "&" + S_PointType + "&" + P_PointPos + "&" + S_PointPos + "&" + PointTrans + "|";
    }
    strPoint = strPoint.substring(0, strPoint.length - 1);
    console.log(strPoint);
    //未建立拓扑关系的设备
    strPoint1 = "";
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        var isInTb = false;
        for (var j = 0; j < allPointTbObjList.length; j++) {
            if (allPointTbObjList[j].sPointName == TBpointVectorLayerArr[i].pointName
           || allPointTbObjList[j].ePointName == TBpointVectorLayerArr[i].pointName) {
                isInTb = true;
                break;
            }
        }
        if (!isInTb) {
            strPoint1 = strPoint1 + TBpointVectorLayerArr[i].pointName + "&" + TBpointVectorLayerArr[i].type + "&"
             + TBpointVectorLayerArr[i].pointPos.x + "," + TBpointVectorLayerArr[i].pointPos.y + "|";
        }
    }
    strPoint1 = strPoint1.substring(0, strPoint1.length - 1);
    console.log(strPoint1);
}

//设置某个拓扑图形的测点名称
function SetTBPointName(OldPointName, SetPointName, SetPointWz, SetPointDevName) {
    //判断新设置的测点是否已经绑定
    for (var i = TBpointVectorLayerArr.length - 1; i >= 0; i--) {
        if (TBpointVectorLayerArr[i].pointName == SetPointName && SetPointName != OldPointName) {
            //alert("当前测点已绑定，请重新选择测点进行绑定！");
            mxLib.Util.toastInfo("当前测点已绑定，请重新选择测点进行绑定！", { delay: 5000 });
            return;
        }
    }
    //更新测点信息
    for (var i = TBpointVectorLayerArr.length - 1; i >= 0; i--) {
        if (TBpointVectorLayerArr[i].pointName == OldPointName) {

            TBpointVectorLayerArr[i].overlay.setJcdName(SetPointName);
            TBpointVectorLayerArr[i].overlay.setJcdWz(SetPointWz);
            TBpointVectorLayerArr[i].overlay.setJcdDevName(SetPointDevName);

            TBpointVectorLayerArr[i].overlay.setText(SetPointName);

            TBpointVectorLayerArr[i].pointName = SetPointName;
            TBpointVectorLayerArr[i].pointWz = SetPointWz;
            TBpointVectorLayerArr[i].pointDevName = SetPointDevName;
        }
        //更新配电柜的子集
        var ChildPoints = TBpointVectorLayerArr[i].ChildPoints.split(',');
        var NewChildPoints = "";
        for (var j = 0; j < ChildPoints.length; j++) {
            if (ChildPoints[j] == OldPointName) {
                NewChildPoints = NewChildPoints + SetPointName + ",";
            }
            else {
                NewChildPoints = NewChildPoints + ChildPoints[j] + ",";
            }
        }
        if (NewChildPoints.indexOf(",") >= 0) {
            NewChildPoints = NewChildPoints.substring(0, NewChildPoints.lastIndexOf(","));
        }
        if (NewChildPoints.length > 0) {
            TBpointVectorLayerArr[i].ChildPoints = NewChildPoints;
        }
    }
    //更新连线信息    
    for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
        if (allPointTbObjList[j].sPointName == OldPointName) {
            allPointTbObjList[j].sPointName = SetPointName;
        }
        if (allPointTbObjList[j].ePointName == OldPointName) {
            allPointTbObjList[j].ePointName = SetPointName;
        }
    }
}
//保存风电闭锁信息
function SaveWindAtresia() {
    var strPointWindAtresia = "";
    for (var i = 0; i < TBpointVectorLayerArr.length ; i++) {
        if (TBpointVectorLayerArr[i].pointName == "1#") {
            if (TBpointVectorLayerArr[i].ChildPoints.length < 1) {
                //alert("1#配电柜请至少，绑定一个开关量设备！");
                mxLib.Util.toastInfo("1#配电柜请至少，绑定一个开关量设备！", { delay: 5000 });
                return;
            }
            strPointWindAtresia = strPointWindAtresia + TBpointVectorLayerArr[i].ChildPoints;
        }
        if (TBpointVectorLayerArr[i].pointName == "2#") {
            if (TBpointVectorLayerArr[i].ChildPoints.length < 1) {
                //alert("2#配电柜请至少，绑定一个开关量设备！");
                mxLib.Util.toastInfo("2#配电柜请至少，绑定一个开关量设备！", { delay: 5000 });
                return;
            }
            strPointWindAtresia = strPointWindAtresia + "|" + TBpointVectorLayerArr[i].ChildPoints;
        }
    }

    mxLib.View.callOutCommand("SaveWindAtresia", strPointWindAtresia);
}
//添加风电闭锁开关量测点
function AddWindPowerAtresiaPoint(ParentPointName, NewPointName, NewPointWz, NewPointDevName) {
    var overlay;
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        if (TBpointVectorLayerArr[i].pointName == ParentPointName) {
            overlay = TBpointVectorLayerArr[i].overlay;
        }
    }
    var TopologyDefObjNow;
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        if (TBpointVectorLayerArr[i].pointName == overlay.getJcdName()) {
            TopologyDefObjNow = TBpointVectorLayerArr[i];
        }
    }

    var SonPointList = [];
    var LastPointName = [];
    if (TopologyDefObjNow.ChildPoints.length > 0) {
        SonPointList = TopologyDefObjNow.ChildPoints.split(',');
        LastPointName = SonPointList[SonPointList.length - 1];
    }

    var YOffset = 80;

    if (LastPointName.length > 0) {
        for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
            if (TBpointVectorLayerArr[i].pointName == LastPointName) {
                overlay = TBpointVectorLayerArr[i].overlay;
                YOffset = 60;
            }
        }
    }

    var NewPointX = overlay.getBounds().getLeftTop().x;
    var NewPointY = overlay.getBounds().getLeftTop().y - YOffset;

    AddTBPointToMap(NewPointName, NewPointWz, NewPointDevName, "风电闭锁开关", '1', '0', '0', NewPointX, NewPointY, 165, 40, "1", "","0");

    //添加连线
    var startCentrPoint = overlay.getBounds().getCenter();
    var tranType = "0";
    var P_PointCode = overlay.getJcdName();
    var S_PointCode = TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].pointName;
    var P_PointPos = new mxLib.Point(startCentrPoint.x, startCentrPoint.y);
    var S_PointPos = new mxLib.Point(TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().x,
     TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().y);
    var pointsList = [];
    pointsList.push(P_PointPos);
    var centerPoint1 = new mxLib.Point(P_PointPos.x, ((P_PointPos.y + S_PointPos.y) / 2));
    var centerPoint2 = new mxLib.Point(S_PointPos.x, ((P_PointPos.y + S_PointPos.y) / 2));
    pointsList.push(centerPoint1);
    pointsList.push(centerPoint2);
    pointsList.push(S_PointPos);
    if (!allPointTbObjList) {
        allPointTbObjList = [];
    }
    var TbTranObj = {}; //定义拓扑图对象
    //连线类型
    TbTranObj.tranType = tranType;
    //添加到路线点数组
    TbTranObj.points = pointsList;
    //添加到线路数组
    TbTranObj.polyline = new mxLib.Polyline(pointsList, { strokeColor: "DarkGray", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false });
    map.addOverlay(TbTranObj.polyline);
    //将路线的起始点编号、结束点编号添加到路线编号数组                    
    TbTranObj.sPointName = P_PointCode;
    TbTranObj.ePointName = S_PointCode;
    //将Svg的起始点坐标存到对象数组                   
    TbTranObj.sPointPos = P_PointPos;
    TbTranObj.ePointPos = S_PointPos;
    allPointTbObjList.push(TbTranObj);

    //将新增加的设备加到配电柜的子设备下面
    if (TopologyDefObjNow.ChildPoints.length < 1) {
        TopologyDefObjNow.ChildPoints = S_PointCode;
    }
    else {
        TopologyDefObjNow.ChildPoints = TopologyDefObjNow.ChildPoints + "," + S_PointCode;
    }
}
//添加分站拓扑图定义连线
function AddStationTopologyDefineLine(PointName, BranchNumBer) {
    var Startpoint;
    switch (BranchNumBer) {
        case "1"://分支1
            Startpoint = new mxLib.Point(15, 625);
            break;
        case "2"://分支2
            Startpoint = new mxLib.Point(15, 355);
            break;
        case "3"://分支3
            Startpoint = new mxLib.Point(375, 625);
            break;
        case "4"://分支4
            Startpoint = new mxLib.Point(375, 355);
            break;
        case "5"://控制通道
            Startpoint = new mxLib.Point(185, 275);
            break;
        case "6"://智能通道
            Startpoint = new mxLib.Point(265, 275);
            break;
        case "7"://控制通道
            Startpoint = new mxLib.Point(75, 275);
            break;
    }
    var overlay;
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        if (TBpointVectorLayerArr[i].pointName == PointName) {
            overlay = TBpointVectorLayerArr[i].overlay;
        }
    }
    //添加连线
    var startCentrPoint = overlay.getBounds().getCenter();
    var tranType = "0";
    var P_PointCode = "分支" + BranchNumBer;
    var S_PointCode = overlay.getJcdName();
    var P_PointPos = Startpoint;
    var S_PointPos = new mxLib.Point(startCentrPoint.x, startCentrPoint.y);
    var pointsList = [];
    pointsList.push(P_PointPos);
    var centerPoint1;
    var centerPoint2;
    if (BranchNumBer == "5" || BranchNumBer == "6" || BranchNumBer == "7") {
        centerPoint1 = new mxLib.Point(P_PointPos.x, ((P_PointPos.y + S_PointPos.y) / 2));
        centerPoint2 = new mxLib.Point(S_PointPos.x, ((P_PointPos.y + S_PointPos.y) / 2));
    }
    else {
        centerPoint1 = new mxLib.Point(((P_PointPos.x + S_PointPos.x) / 2), P_PointPos.y);
        centerPoint2 = new mxLib.Point(((P_PointPos.x + S_PointPos.x) / 2), S_PointPos.y);
    }
    pointsList.push(centerPoint1);
    pointsList.push(centerPoint2);
    pointsList.push(S_PointPos);

    ////绘制线路
    //var polyline = new mxLib.Polyline(pointsList, { strokeColor: "DarkGray", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false });
    //map.addOverlay(polyline);

    var TbTranObj = {}; //定义拓扑图对象
    //连线类型
    TbTranObj.tranType = tranType;
    //添加到路线点数组
    TbTranObj.points = pointsList;
    //添加到线路数组
    TbTranObj.polyline = new mxLib.Polyline(pointsList, { strokeColor: "DarkGray", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false });
    map.addOverlay(TbTranObj.polyline);
    //将路线的起始点编号、结束点编号添加到路线编号数组                    
    TbTranObj.sPointName = P_PointCode;
    TbTranObj.ePointName = S_PointCode;
    //将Svg的起始点坐标存到对象数组                   
    TbTranObj.sPointPos = P_PointPos;
    TbTranObj.ePointPos = S_PointPos;
    allPointTbObjList.push(TbTranObj);
}
//删除所有拓扑图定义图元
function RemoveAllVectorLayerOverlay() {
    for (var i = 0; i < allPointTbObjList.length; i++) {
        map.removeOverlay(allPointTbObjList[i].polyline);
    }
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        map.removeOverlay(TBpointVectorLayerArr[i].overlay);
    }
        
    //清除所有对象缓存  
    allPointTbObjList = [];
    TBpointVectorLayerArr = [];
}
//加载分站图形化定义界面
function SubstationGraphicalLoad(json) {
    var json = JSON.parse(json);
    //是否需要删除之前的设备
    if (json.IsRemoveAllVectorLayerOverlay) {
        RemoveAllVectorLayerOverlay();
    }
    //是否需要重新加载
    if (json.LoadPointString.length > 0) {
        LoadPoint(json.LoadPointString);
    }
    //绘线路
    for (var i = 0; i < json.AddStationTopologyDefineLineList.length; i++) {
        AddStationTopologyDefineLine(json.AddStationTopologyDefineLineList[i].Point, json.AddStationTopologyDefineLineList[i].BranchNumBer);
    }
    //是否加载ToolTips
    if (json.IsRemoveAllVectorLayerOverlay) {
        addToolTips();
    }
}






