/*
* @fileoverview 图元测试所有按钮。
*
* @author Metamap Map Api Group
*/

//地图卫星地图切换按钮---------------
var buttons = [
    {
        text: "地图",
        colorIndex: 1,
        callback: function () {
            map.mapModel.setMapTileRequestUrl("web/{zoom}/{row}/{col}.png");
            map.mapModel.loadTiles();
        }
    },
    {
        text: "卫星",
        colorIndex: 2,
        callback: function () {
            map.mapModel.setMapTileRequestUrl("sate/{zoom}/{row}/{col}.png^label/{zoom}/{row}/{col}.png");
            map.mapModel.loadTiles();
        }
    }
];
var ctl = new mxLib.LinkButtonGroupControl(buttons, { style: "border:1px solid #070;padding:3px", anchor: MX_ANCHOR_TOP_RIGHT });
map.addControl(ctl);
//地图卫星地图切换按钮---------------

//增加按钮-----------main
var buttonCtrlTrajectory = new mxLib.LinkButtonControl("与C#交互",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(3, 230),
    opacity: 0.7
},
    function () {
        mxLib.View.callOutCommand("MessagePub", "C#交互成功！")
    });
map.addControl(buttonCtrlTrajectory);

function getDbLayers() {
    var point = new mxLib.Point(900, 620);
    var sContent = "";
    var layers = map.getLayer("");
    layers = $.parseJSON(layers);
    for (var i in layers) {
        sContent += "图层名称:" + layers[i].layerName + "</br>";
        if (layers[i].layerName == "巷道名称") {
            map.setLayerAttribute(layers[i].layerName, "DWG_LAYER_VISBLE", "false");
        }
    }
    console.log(sContent);
}
var buttonCtrlgetLayer = new mxLib.LinkButtonControl("获取并隐藏图层信息", {
    colorIndex: 1,
    offest: new mxLib.Size(0, 0),
    anchor: MX_ANCHOR_BOTTOM_LEFT, opacity: 0.9
}, getDbLayers);
map.addControl(buttonCtrlgetLayer);
//增加按钮-----------main


//增加按钮-----------TrajectoryDef
var buttonCtrlTrajectory = new mxLib.LinkButtonControl("绘制人员轨迹",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(3, 130),
    opacity: 0.7
},
    function () {
        DoTrajectoryDef();
    });
map.addControl(buttonCtrlTrajectory);

var buttonCtrlTrajectory1 = new mxLib.LinkButtonControl("保存轨迹",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(90, 130),
    opacity: 0.7
},
    function () {
        var allPointStr = "";
        allPointTranList.length = 0;
        for (var i = 0; i < allPointTranMapArr.length; i++) {
            var tempTransPoint = "";
            for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
                tempTransPoint += allPointTranMapArr[i].points[j].x + "," + allPointTranMapArr[i].points[j].y + "#";
                var obj = {};
                obj.points = allPointTranMapArr[i].points[j];                
                if (j <= allPointTranMapArr[i].points.length / 2) {                   
                    var dateDis="2015-09-08 00:08:00";
                    obj.DisText = "姓名：张三</br>" + "经过地点：" + allPointTranMapArr[i].sPointName + "</br>时间：" + dateDis;
                }
                else {
                    var dateDis = "2015-09-08 00:08:02";
                    obj.DisText = "姓名：张三</br>" + "经过地点：" + allPointTranMapArr[i].ePointName + "</br>时间：" + dateDis;
                }
                allPointTranList.push(obj);
            }
            allPointStr = allPointTranMapArr[i].sPointName + "|" + allPointTranMapArr[i].ePointName + "|" +
            allPointTranMapArr[i].sPointPos.x + "," + allPointTranMapArr[i].sPointPos.y + "|" +
            allPointTranMapArr[i].ePointPos.x + "," + allPointTranMapArr[i].ePointPos.y + "|" +
            tempTransPoint.substring(0, tempTransPoint.length - 1) + "&";
        }
        allPointStr = allPointStr.substring(0, allPointStr.length - 1);
        //mxLib.Util.toastInfo(allPointStr, { delay: 8000 });
        console.log(allPointStr);
        //结束命令
        rectTrajectoryDefCmd.end();
    });
map.addControl(buttonCtrlTrajectory1);

function clearLine() {
    var count = zhpolylineList.length;
    for (var i = 0; i < count; i++) {
        map.removeOverlay(zhpolylineList[i]);
    }
    zhpolylineList.length = 0;
}

var buttonCtrlTrajectory2 = new mxLib.LinkButtonControl("清空轨迹",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(150, 130),
    opacity: 0.7
},
    function () {
        //删除所有波及线路
        clearLine();
        //删除所有轨迹线路
        for (var i = 0; i < allPointTranMapArr.length; i++) {
            map.removeOverlay(allPointTranMapArr[i].polyline);
        }
        //轨迹路线点数组重置
        allPointTranMapArr.length = 0;
        //结束命令
        rectTrajectoryDefCmd.end();
    });
map.addControl(buttonCtrlTrajectory2);
var buttonCtrlTrajectory3 = new mxLib.LinkButtonControl("灾害路径",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(210, 130),
    opacity: 0.7
},
    function () {
        //绘制灾害路径
        var points = [];
        var index = 0;
        for (var i = 0; i < allPointTranMapArr.length; i++) {
            for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
                points[index] = new mxLib.Point(allPointTranMapArr[i].points[j].x, allPointTranMapArr[i].points[j].y);
                index++;
            }
        }
        var zhpolyline = new mxLib.Polyline(points
                , { strokeColor: "red", strokeWidth: 15, strokeOpacity: 0.9, enableClicking: true });
        map.addOverlay(zhpolyline);
        mxLib.Util.setAnimation(zhpolyline.getDomElement(), MX_ANIMATION_FLASH, 10000);
        zhpolylineList.push(zhpolyline);
    });
map.addControl(buttonCtrlTrajectory3);

var buttonCtrlTrajectory4 = new mxLib.LinkButtonControl("浏览路径",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(270, 130),
    opacity: 0.7
},
    function () {
        //开始线路
        var startTfpolyline = [];
        var svgPointsStr = [];
        var TfpolyIndex = 0;
        var flag = true;

        var vl = new mxLib.VectorLayer({ minZoom: 2 });
        map.addOverlay(vl);

        var svg = vl.snap();
        var g1 = svg.paper.gradient("l(0, 0, 0, 0.7, 0, 1)#f00-#ff0-#0f0"); //渐变色

        for (var i = 0; i < allPointTranMapArr.length; i++) {
            svgPoints = "M";
            g1 = svg.paper.gradient("l(0, 0, 0, 0.7, 0, 1)#" + Math.floor(Math.random() * 16777215).toString(16) + "-#" + Math.floor(Math.random() * 16777215).toString(16) + "-#" + Math.floor(Math.random() * 16777215).toString(16) + "");
            for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
                svgPoints += vl.svgMapPoint(allPointTranMapArr[i].points[j]) + " ";
            }
            svgPointsStr[i] = svgPoints;
        }
        var pth = svg.paper.path(svgPointsStr).attr({
            stroke: g1,
            strokeWidth: 3,
            fill: "none",
            markerStart: m1,
            // 结束标记
            "marker-end": m2
        });
        //开始头圆
        var p1 = svg.paper.circle().attr({
            r: "1",
            fill: "#f0f"
        });
        var m1 = p1.marker(-1, -1, 2, 2);
        m1.attr({ markerWidth: 2.5, markerHeight: 2.5 });

        //结束点箭头
        var p2 = svg.paper.polygon("0,1 1,0 0,-1").attr({
            fill: "#f00"
        });
        var m2 = p2.marker(-1, -1, 2, 2);
        m2.attr({ markerWidth: 5, markerHeight: 5 });

        //显示动画
        var route = svg.paper.path(svgPointsStr).attr({ "display": "none" }); //路线路径
        var len = Snap.path.getTotalLength(route.attr("d"));

        var animateCallback = null;
        Snap.animate(0, len, function (l) { var dot = route.getPointAtLength(l); pth.attr({ d: route.getSubpath(0, l) }); },
        2000, mina.linear, animateCallback = function () {
            //动画结束后，再次调用动画函数，循环显示
            //Snap.animate(this.start, this.end, this.set, this.dur, this.easing, animateCallback);
            //结束后删除动画
            map.removeOverlay(vl);
        });
    });
map.addControl(buttonCtrlTrajectory4);
//增加按钮-----------TrajectoryDef


//增加按钮-----------TopologyTransDef
var buttonCtrlTbTran = new mxLib.LinkButtonControl("拓扑图连线", {
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(3, 260),
    opacity: 0.7
},
    function () {
        DoTopologyTransDef();
    });
map.addControl(buttonCtrlTbTran);

var buttonCtrlTbTran1 = new mxLib.LinkButtonControl("保存拓扑图", {
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(80, 260),
    opacity: 0.7
},
    function () {
        saveTBPoint();
    });
map.addControl(buttonCtrlTbTran1);

var buttonCtrlTbTran2 = new mxLib.LinkButtonControl("结束连线命令", {
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(160, 260),
    opacity: 0.7
},
    function () {
        //结束命令
        if (rectTbTranDefCmd) {
            rectTbTranDefCmd.end();
        }
    });
map.addControl(buttonCtrlTbTran2);
//增加按钮-----------TopologyTransDef


//设置旋转、皮带动画-----------SvgAnimation
var bounds = new mxLib.Bounds(new mxLib.Point(7800, 140), new mxLib.Point(8000, 51));
var vl1 = new mxLib.VectorLayer("mx/Svg/风扇.svg", bounds);
map.addOverlay(vl1);

bounds = new mxLib.Bounds(new mxLib.Point(7800, 240), new mxLib.Point(8000, 150));
var vl11 = new mxLib.VectorLayer("mx/Svg/给煤机.svg", bounds);
map.addOverlay(vl11);

var buttonSetVal1 = new mxLib.LinkButtonControl("开始旋转",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(220, 200),
    opacity: 0.7
},
    function () {
        vl1.getSVGDocument().getElementById('Rotate_1').beginElement();
        vl11.getSVGDocument().getElementById('Move_Path_19').beginElement();
    });
map.addControl(buttonSetVal1);
var buttonSetVal2 = new mxLib.LinkButtonControl("停止旋转",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(160, 200),
    opacity: 0.7
},
    function () {
        vl11.getSVGDocument().getElementById('Move_Path_19').endElement();
        vl1.getSVGDocument().getElementById('Rotate_1').endElement();

    });
map.addControl(buttonSetVal2);
//设置旋转、皮带动画-----------SvgAnimation

//设置填充动画-----------SvgAnimation
bounds = new mxLib.Bounds(new mxLib.Point(8300, 140), new mxLib.Point(8500, 51));
var vl2 = new mxLib.VectorLayer("mx/Svg/填充.svg", bounds);
map.addOverlay(vl2);
var buttonSetVal3 = new mxLib.LinkButtonControl("随机填充",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(80, 200),
    opacity: 0.7
},
    function () {
        vl2.getSVGDocument().getElementById('Fill_0').setAttribute("to", Math.random() * 100);
        vl2.getSVGDocument().getElementById('Fill_0').beginElement();
    });
map.addControl(buttonSetVal3);
//设置填充动画-----------SvgAnimation

//随机设置文本显示-----------SvgAnimation
bounds = new mxLib.Bounds(new mxLib.Point(8800, 140), new mxLib.Point(9000, 51));
var vl3 = new mxLib.VectorLayer("mx/Svg/文本.svg", bounds);
map.addOverlay(vl3);

var buttonSetVal4 = new mxLib.LinkButtonControl("设置文本",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(0, 200),
    opacity: 0.7
},
    function () {
        vl3.snap().select("#Text_0").attr("text", Math.random());
    });
map.addControl(buttonSetVal4);
//随机设置文本显示-----------SvgAnimation


//增加按钮-----------PointRef
var buttonCtrl = new mxLib.LinkButtonControl("开始/停止刷新数据",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(3, 30),
    opacity: 0.7
},
    function () {
        if (RefFlag) {

            RefFlag = false;
        }
        else {
            RefFlag = true;
        }
    });

map.addControl(buttonCtrl);

var buttons = [
    {
        text: "显示",
        group: "visible",
        callback: function () {
            setShow(1);
        }
    },
    {
        text: "隐藏",
        group: "drag",
        selected: true,
        callback: function () {
            setShow(0);
        }
    }
];
var ctl = new mxLib.LinkButtonGroupControl(buttons, { anchor: MX_ANCHOR_BOTTOM_LEFT, offest: new mxLib.Size(3, 60), style: "border:1px solid #070;padding:3px" });
map.addControl(ctl);

buttons = [
    {
        text: "显示1隐藏2",
        group: "visible",
        callback: function () {
            setShow(3);
        }
    },
    {
        text: "隐藏1显示2",
        group: "drag",
        selected: true,
        callback: function () {
            setShow(4);
        }
    }
];
var ctl1 = new mxLib.LinkButtonGroupControl(buttons, { anchor: MX_ANCHOR_BOTTOM_LEFT, offest: new mxLib.Size(3, 90), style: "border:1px solid #070;padding:3px" });
map.addControl(ctl1);
//增加按钮-----------PointRef

//增加按钮-----------AreaDef
var buttonCtrlAreaDef = new mxLib.LinkButtonControl("区域多边形绘制", {
    colorIndex: 1,
    offest: new mxLib.Size(130, 0),
    anchor: MX_ANCHOR_BOTTOM_LEFT, opacity: 0.9
}, function () {
    //    this.options.strokeColor = this.options.strokeColor || "blue";//边框
    //    this.options.fillColor = this.options.fillColor || "green";//填充
    //    this.options.strokeWidth = this.options.strokeWidth || 2;//边框线宽
    //    this.options.strokeOpacity = this.options.strokeOpacity || 0.2;//透明度
    DrawAreaPoint("#ff0000", "green", 3, 0.9);
});
map.addControl(buttonCtrlAreaDef);

var buttonCtrlAreaLoad = new mxLib.LinkButtonControl("加载区域", {
    colorIndex: 1,
    offest: new mxLib.Size(330, 0),
    anchor: MX_ANCHOR_BOTTOM_LEFT, opacity: 0.9
}, function () {
    for (var i = 0; i < AreaList.length; i++) {
        var polygon = new mxLib.Polygon(AreaList[i].points, { fillColor: "#ff0000", strokeOpacity: 0.5 });
        map.addOverlay(polygon);
    }
});
var buttonCtrlAreaLoad = new mxLib.LinkButtonControl("获取所有区域", {
    colorIndex: 1,
    offest: new mxLib.Size(230, 0),
    anchor: MX_ANCHOR_BOTTOM_LEFT, opacity: 0.9
}, function () {
    var SaveStr = "";
    for (var i = 0; i < AreaList.length; i++) {
        var AreaName = AreaList[i].Name; //区域名称
        var AreaPoints = AreaList[i].points; //区域多边形绘制的点坐标数组
        var AreaPointsStr = "";
        for (var j = 0; j < AreaPoints.length; j++) {
            AreaPointsStr += AreaPoints[j].x + "," + AreaPoints[j].y + "#";
        }
        AreaPointsStr = AreaPointsStr.substring(0, AreaPointsStr.length - 1);
        var AreaSonPoints = AreaList[i].sonPoint; //区域包含的测点
        var AreaSonPointsStr = "";
        for (var j = 0; j < AreaSonPoints.length; j++) {
            AreaSonPointsStr += AreaSonPoints[j] + "#";
        }
        AreaSonPointsStr = AreaSonPointsStr.substring(0, AreaSonPointsStr.length - 1);
        SaveStr = AreaName + "&" + AreaPointsStr + "&" + AreaSonPointsStr + "|";
    }
    SaveStr = SaveStr.substring(0, SaveStr.length - 1);
    console.log(SaveStr);
});
map.addControl(buttonCtrlAreaLoad);
//增加按钮-----------AreaDef

//增加按钮-------------LuShu
var buttonCtrlTrance = new mxLib.LinkButtonControl("轨迹行走",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(3, 160),
    opacity: 0.7
},
    function () {
        //对象转换
        var transList = "";
        for (var i = 0; i < allPointTranList.length; i++) {
            var tranpoints = allPointTranList[i].points;
            var trandistext = allPointTranList[i].DisText;
            transList = transList + tranpoints.x + "|" + tranpoints.y + "|" + trandistext+","
        }
        if (transList.indexOf(",") > 0) {
            transList = transList.substring(0, transList.length - 1);
        }
        lushuStart("测试人员", 50,transList);
    });
map.addControl(buttonCtrlTrance);

var buttonCtrlViewRoute = new mxLib.LinkButtonControl("避灾路线",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(190, 160),
    opacity: 0.7
},
    function () {
        //对象转换
        var transList = "";
        for (var i = 0; i < allPointTranList.length; i++) {
            var tranpoints = allPointTranList[i].points;            
            transList = transList + tranpoints.x + "|" + tranpoints.y  + ","
        }
        if (transList.indexOf(",") > 0) {
            transList = transList.substring(0, transList.length - 1);
        }
        ViewRoute('escapeRoute', transList,'blue','red','10000');
    });
map.addControl(buttonCtrlViewRoute);


var buttonCtrlTrance1 = new mxLib.LinkButtonControl("停止",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(80, 160),
    opacity: 0.7
},
    function () {
        for (var i = 0; i < lushuArr.length; i++) {
            lushuArr[i].stop();
        }
    });
map.addControl(buttonCtrlTrance1);

var buttonCtrlTranc2 = new mxLib.LinkButtonControl("测点定位",
{
    colorIndex: 1,
    anchor: MX_ANCHOR_BOTTOM_LEFT,
    offest: new mxLib.Size(120, 160),
    opacity: 0.7
},
    function () {
        SearchPoint();
    });
map.addControl(buttonCtrlTranc2);
//增加按钮-------------LuShu