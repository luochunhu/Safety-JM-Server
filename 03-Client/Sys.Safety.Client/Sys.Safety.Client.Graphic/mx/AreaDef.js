var AreaList = [];

//绘制区域图元
function DrawAreaPoint(fillcolor, strokecolor, strokewidth, strokeopacity) {
    var DrawPolygonCommand = new mxLib.DrawPolygonCommand("", null, {
        fillColor: fillcolor, //"#ff0000",
        fillOpacity: 0.1,
        //        strokeStyle: "dashed",
        strokeColor: strokecolor, //"green",
        strokeWidth: strokewidth, //3,
        strokeOpacity: strokeopacity, //0.9,
        enableClicking: false,
        cursor: "crosshair"
    });
    map.startCommand(DrawPolygonCommand);
    DrawPolygonCommand.addEventListener("end", function (e) {
        if (e.command.cancel) return;
        var points = e.command.points;
        var DrawOverlay = e.command.getDrawOverlay();
        var d = 0;
        e.command.getDrawOverlay().setFillColor("green");
        //获取区域中所有测点
        var sonPointList = [];
        for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
            if (mxLib.GeoUtils.isPointInPolygon(pointRichMarkerMapArr[i].pointPos, e.command.getDrawOverlay())) {
                sonPointList.push(pointRichMarkerMapArr[i].pointName);
            }
        }
        AddArea(points, sonPointList, DrawOverlay);
    });
}
function AddArea(points, sonPointList, DrawOverlay) {
    //弹出窗口
    PointName = new Ext.form.TextField({
        fieldLabel: '测点号：',
        labelStyle: 'text-align:right'
    });
    form = new Ext.form.FormPanel({
        lableWidth: 40,
        bodyStyle: 'padding:5px',
        labelPad: 10,
        width: 380,
        title: '',
        defaultType: "textfield",
        region: "center",
        defaults: {
            width: 240,
            msgTarget: 'side'
        },
        layoutConfig: {
            //这里是布局配置项 layout-specific configs go here
            labelSeparator: ''
        },
        items: [PointName]
    });
    //弹出窗口
    win = new Ext.Window({
        height: 120,
        width: 400,
        items: form,
        closeAction: 'close',
        layout: 'fit',
        region: "center",
        resizable: false,
        title: '添加测点',
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'save',
            handler: function () {
                var AreaObj = {}; //区域对象
                AreaObj.Name = PointName.getValue(); //区域名称
                AreaObj.overlay = DrawOverlay;//多边形状
                AreaObj.points = points; //区域多边形绘制的点坐标数组
                AreaObj.sonPoint = sonPointList; //区域包含的测点

                AreaList.push(AreaObj);

                win.close();
            }
        },
            {
                text: '取消',
                iconCls: 'cancel',
                handler: function () {
                    win.close();

                }
            }]

    });
    win.show();
    win.setTitle("添加区域");
}

//定义名称空间
var Basic = Basic || {};
Basic.DrawArea = Basic.DrawArea || {};

//定义图形存储对象
Basic.DrawArea.DrawObject = null;

//定义图形绘制类型
Basic.DrawArea.DrawObjectType = null;

//定义图形存储数据
Basic.DrawArea.DrawObjcetJsonData = '';

//用于编辑时保存绘制图形对象
Basic.DrawArea.DrawObjectForEdit = null;

//清除绘制图形
Basic.DrawArea.ClearDrawingGraphics = function () {
    //if (Basic.DrawArea.DrawObject != null && Basic.DrawArea.DrawObjectForEdit == null) {
    //    map.removeOverlay(Basic.DrawArea.DrawObject.getDrawOverlay());
    //    Basic.DrawArea.DrawObject = null;
    //}
    if (Basic.DrawArea.DrawObjectForEdit) {
        map.removeOverlay(Basic.DrawArea.DrawObjectForEdit);
        Basic.DrawArea.DrawObjectType = null;
        Basic.DrawArea.DrawObject = null;
        Basic.DrawArea.DrawObjectForEdit = null;
        Basic.DrawArea.DrawObjcetJsonData = '';
    }
}

//绘制圆形（新增状态）
Basic.DrawArea.DrawingCircle = function () {
    //绘制圆形命令
    var drawCircleCommand = new mxLib.DrawCircleCommand("圆形", null, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
    //执行命令
    map.startCommand(drawCircleCommand);
    //添加命令执行后的事件
    drawCircleCommand.addEventListener("end", function (e) {
        if (e.command.cancel) return;
        if (e.command.getDrawOverlay() != null) {
            //获取圆的半径
            var radius = e.command.getDrawOverlay().getRadius();
            //获取圆中心点的x坐标
            var pointX = e.command.getDrawOverlay().getCenter().x;
            //获取圆中心点的y坐标
            var pointY = e.command.getDrawOverlay().getCenter().y;

            if (Basic.DrawArea.DrawObject == null) {
                Basic.DrawArea.DrawObjectType = "circle";
                Basic.DrawArea.DrawObject = drawCircleCommand;

                //圆中心点的x、y坐标和半径保存到DrawObjcetJsonData中
                var circleArr = [];
                var circleObj = { "pointX": pointX, "pointY": pointY, "radius": radius };
                circleArr.push(circleObj);
                Basic.DrawArea.DrawObjcetJsonData = JSON.stringify(circleArr);
            }
        }
    });
}

//绘制多边形（新增状态）
Basic.DrawArea.DrawingPolygon = function () {
    //绘制多边形命令
    var drawPolygonCommand = new mxLib.DrawPolygonCommand("多边形", null, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
    //执行命令
    map.startCommand(drawPolygonCommand);
    //添加命令执行后的事件
    drawPolygonCommand.addEventListener("end", function (e) {
        if (e.command.bCancel) return;
        if (e.command.getDrawOverlay() != null) {
            //多边形的每个点x、y保存到DrawObjcetJsonData中
            var pointArry = [];
            point = e.command.getDrawOverlay().getPath();
            for (var i = 0; i < point.length; i++) {
                var polygonObj = { "pointX": point[i].x, "pointY": point[i].y };
                pointArry.push(polygonObj);
            }
            Basic.DrawArea.DrawObjcetJsonData = JSON.stringify(pointArry);
            Basic.DrawArea.DrawObjectType = "polygon";
            Basic.DrawArea.DrawObject = drawPolygonCommand;
        }
    });
}

//保存绘制图形数据
Basic.DrawArea.SaveDrawingData = function () {
    if (Basic.DrawArea.DrawObjcetJsonData != '' && Basic.DrawArea.DrawObject) {
        return Basic.DrawArea.DrawObjcetJsonData + "|" + Basic.DrawArea.DrawObjectType;
    }
    else {
        return "";
    }
}

//判断是否有绘制其它图形
Basic.DrawArea.DoOtherDrawingGraphics = function () {
    if (Basic.DrawArea.DrawObjectType == null) {
        return "";
    }
    else {
        if (Basic.DrawArea.DrawObjectType == "circle") {
            return "已经绘制了一个圆形联动区域，如果要重新绘制请点击清除按钮！";
        }
        else if (Basic.DrawArea.DrawObjectType == "polygon") {
            return "已经绘制了一个多边形联动区域，如果要重新绘制请点击清除按钮！";
        }
    }
}

//绘制圆形或多边形（编辑状态）
Basic.DrawArea.DrawinggGraphicsInEdit = function (json, type) {
    if (type == "circle") {
        var jsonObj = eval("(" + json + ")");
        var pointX = parseFloat(jsonObj[0].pointX);
        var pointY = parseFloat(jsonObj[0].pointY);
        var radius = parseFloat(jsonObj[0].radius);
        //创建中心点坐标
        var point = new mxLib.Point(pointX, pointY);
        //创建圆
        var circle = new mxLib.Circle(point, radius, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
        //添加到地图上
        map.addOverlay(circle);
        //保存类型（编辑状态）
        Basic.DrawArea.DrawObjectType = type;
        //保存绘制对象（编辑状态）
        Basic.DrawArea.DrawObjectForEdit = circle;
        //保存图形数据 (编辑状态)
        Basic.DrawArea.DrawObjcetJsonData = json;
        //保存图形对象 (编辑状体)
        Basic.DrawArea.DrawObject = circle;
    }
    else if (type == "polygon") {
        var jsonObj = eval("(" + json + ")");
        if (jsonObj != null) {
            pointArr = [];
            for (var i = 0; i < jsonObj.length; i++) {
                var pointX = parseFloat(jsonObj[i].pointX);
                var pointY = parseFloat(jsonObj[i].pointY);
                var point = new mxLib.Point(pointX, pointY);
                pointArr.push(point);
            }
            //创建多边形
            var polygon = new mxLib.Polygon(pointArr, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
            //添加到地图上
            map.addOverlay(polygon);
            //保存绘制对象（编辑状态）
            Basic.DrawArea.DrawObjectForEdit = polygon;
            //保存绘制对象 （编辑状态）
            Basic.DrawArea.DrawObject = polygon;
        }
        //保存类型（编辑状态）
        Basic.DrawArea.DrawObjectType = type;
        //保存图形数据
        Basic.DrawArea.DrawObjcetJsonData = json;
    }
}

//绘制多边形（带名称显示）
Basic.DrawArea.DrawinggGraphicsAndName = function (json, type, name, areaId) {
    var jsonObj = eval("(" + json + ")");
    if (jsonObj != null) {
        pointArr = [];
        for (var i = 0; i < jsonObj.length; i++) {
            var pointX = parseFloat(jsonObj[i].pointX);
            var pointY = parseFloat(jsonObj[i].pointY);
            var point = new mxLib.Point(pointX, pointY);
            pointArr.push(point);
        }
        //创建多边形
        var polygon;
        if (type == "polyline") {

            polygon = new mxLib.Polygon(pointArr, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });

            if (jsonObj.length > 0) {
                var pointX = parseFloat(jsonObj[0].pointX);
                var pointY = parseFloat(jsonObj[0].pointY);
                var point = new mxLib.Point(pointX, pointY);
                pointArr.push(point);
            }
            var tempPolyline = new mxLib.Polyline(pointArr, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 6, strokeOpacity: 0.9, enableClicking: false, });
            map.addOverlay(tempPolyline);

        }
        else {
            polygon = new mxLib.Polygon(pointArr, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
            //添加到地图上
            map.addOverlay(polygon);
        }
       

        var labelTitlePosition = polygon.getBounds().getCenter();
        var labelTitleOffset = new mxLib.Size(-85, 0);
        var labelTitleName = "<div style='text-align:center; width:180px;'><span style='padding-left:4px;font-family:SimHei;font-weight:600';text-shadow:5px 5px 5px #FF0000>"
            + name + "</span></div>";

        var opts = {
            "position": labelTitlePosition,
            "offset": labelTitleOffset
            //"minZoom": labelMinZoom,
            //"maxZoom": labelMaxZoom
            //,"labelTitleName1": labelTitleName
        }

        var labelTitle = new mxLib.Label(labelTitleName, opts);
        labelTitle.setStyle({
            background: '#56ABE4',
            color: "white",
            fontweight: 'bold',
            border: '0px solid #ccc',
            fontSize: "12px",
            height: "20px",
            lineHeight: "20px",
            fontFamily: "微软雅黑"
        });

        map.addOverlay(labelTitle);

        var AreaObj = {}; //区域对象
        AreaObj.Name = name; //区域名称
        AreaObj.overlay = polygon;//多边形状
        AreaObj.points = pointArr; //区域多边形绘制的点坐标数组
        AreaObj.sonPoint = ""; //区域包含的测点
        AreaObj.labelTitle = labelTitle;//区域标注
        AreaObj.areaId = areaId;//区域ID

        AreaList.push(AreaObj);//添加到区域对象数组中
    }
}

//绘制圆形或多边形（刷新状态）
Basic.DrawArea.DrawingGraphicsInRefresh = function (json, type) {
    if (type == "circle") {
        var jsonObj = eval("(" + json + ")");
        var pointX = parseFloat(jsonObj[0].pointX);
        var pointY = parseFloat(jsonObj[0].pointY);
        var radius = parseFloat(jsonObj[0].radius);
        //创建中心点坐标
        var point = new mxLib.Point(pointX, pointY);
        //创建圆
        var circle = new mxLib.Circle(point, radius, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
        //添加到地图上
        map.addOverlay(circle);

        for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
            //判断测点是否在圆形内
            if (!mxLib.GeoUtils.isPointInCircle(pointRichMarkerMapArr[i].pointPos, circle)) {
                map.removeOverlay(pointRichMarkerMapArr[i].overlay);
            }
        }
        for (var j = 0; j < SVGpointVectorLayerArr.length; j++) {
            //判断测点是否在圆形内
            if (!mxLib.GeoUtils.isPointInCircle(SVGpointVectorLayerArr[j].pointPos, circle)) {
                map.removeOverlay(SVGpointVectorLayerArr[j].vl);
            }
        }
        for (var k = 0; k < TBpointVectorLayerArr.length; k++) {
            //判断测点是否在圆形内
            if (!mxLib.GeoUtils.isPointInCircle(TBpointVectorLayerArr[k].pointPos, circle)) {
                map.removeOverlay(TBpointVectorLayerArr[k].overlay);
            }
        }
    }
    else if (type == "polygon") {
        var jsonObj = eval("(" + json + ")");
        if (jsonObj != null) {
            pointArr = [];
            for (var i = 0; i < jsonObj.length; i++) {
                var pointX = parseFloat(jsonObj[i].pointX);
                var pointY = parseFloat(jsonObj[i].pointY);
                var point = new mxLib.Point(pointX, pointY);
                pointArr.push(point);
            }
            //创建多边形
            var polygon = new mxLib.Polygon(pointArr, { fillColor: "#ff0000", fillOpacity: 0.1, strokeStyle: "dashed", strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false, });
            //添加到地图上
            map.addOverlay(polygon);
            //获取区域中所有测点
            for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
                //判断测点是否在多边形内
                if (!mxLib.GeoUtils.isPointInPolygon(pointRichMarkerMapArr[i].pointPos, polygon)) {
                    map.removeOverlay(pointRichMarkerMapArr[i].overlay);
                }
            }
            for (var j = 0; j < SVGpointVectorLayerArr.length; j++) {
                //判断测点是否在圆形内
                if (!mxLib.GeoUtils.isPointInCircle(SVGpointVectorLayerArr[j].pointPos, polygon)) {
                    map.removeOverlay(SVGpointVectorLayerArr[j].vl);
                }
            }
            for (var k = 0; k < TBpointVectorLayerArr.length; k++) {
                //判断测点是否在圆形内
                if (!mxLib.GeoUtils.isPointInCircle(TBpointVectorLayerArr[k].pointPos, polygon)) {
                    map.removeOverlay(TBpointVectorLayerArr[k].overlay);
                }
            }

        }
    }
}

//获取地图上所有测点所绑定的设备名称
Basic.DrawArea.GetMapPointNameAndDevName = function () {
    var objNames = [];
    for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
        var objName = { "PointName": pointRichMarkerMapArr[i].pointName, "DevName": pointRichMarkerMapArr[i].pointDevName };
        objNames.push(objName);
    }
    for (var i = 0; i < SVGpointVectorLayerArr.length; i++) {
        var objName = { "PointName": SVGpointVectorLayerArr[i].pointName, "DevName": SVGpointVectorLayerArr[i].pointDevName };
        objNames.push(objName);
    }
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        var objName = { "PointName": TBpointVectorLayerArr[i].pointName, "DevName": TBpointVectorLayerArr[i].pointDevName };
        objNames.push(objName);
    }
    return JSON.stringify(objNames);
}

//移除右键菜单
Basic.DrawArea.RemoveAreaContextMenu = function () {
    if (richpointRconMenu != null) {
        map.removeContextMenu(richpointRconMenu);
    }
    if (SVGPnt_contextMenu1 != null) {
        map.removeContextMenu(SVGPnt_contextMenu1);
    }
    if (TbPnt_contextMenu1 != null) {
        map.removeContextMenu(TbPnt_contextMenu1);
    }
}