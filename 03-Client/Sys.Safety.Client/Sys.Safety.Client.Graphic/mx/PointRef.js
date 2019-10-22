/*
* 测点实时刷新及隐藏显示 
*/

//console.log("pointref.js");

var RefFlag = false;

//添加热点
function _addOverlayHotSpot() {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker) {
            var point = allOverlay[i].getPosition();
            //map.setCenter(point);
            //            var hotspot = new mxLib.Hotspot(point, { offsets: [1, 80, 26, 1], text: "123123123", minZoom: 0, maxZoom: 10, userData: "我是北京!" });
            //            allOverlay[i].hotspot = hotspot;
            //            map.addHotspot(hotspot);
        }
    }
}
//_addOverlayHotSpot();

//启动实时刷新

function DoRefPointSsz() {
    RefFlag = true;
    /**
    * 回调_启动实时刷新(回调界面读取所有测点的实时数据进行图元刷新【定时回调：每5秒调用一次】)
    * @class callOutCommand_DoRefPointSsz 
    * @param 无
    * @returns 无
    * @example 参考示例：
    *  c#：
    *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
    *        {
    *            string  Param = e.p_sParam;//参数
    *            switch (e.p_sCmd)//命令
    *            {
    *                case "DoRefPointSsz":
    *                //这里进行实现
    *                break;
    *            }
    *        }
    * IE：
    * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
    *      if (cmd == "DoRefPointSsz") {
    *          //这里进行实现
    *      }
    *   }
    * google:
    *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
    *      if (cmd == "DoRefPointSsz") {
    *          //这里进行实现
    *      }
    *   }
    */
    mxLib.View.callOutCommand("DoRefPointSsz", "");
}
//结束实时刷新
function EndRefPointSsz() {
    RefFlag = false;
    StopRefSsz();
}

//刷新实时值
/**
* 刷新实时值
* @class RefPointSsz 
* @param {PointSszStr} 测点实时值列表(测点号,实时值,数据状态值,是否报警（1：报警，0：不报警）,数据状态文本,设备状态文本),多个用“|”分隔,不传值表示清除所有实时显示 如：001A01,0.85%,10,0,上限预警,正常|001D01,开,27,0,0态,正常
* @returns 无
* @example 参考示例：
* RefPointSsz('001A01,0.85%,10,0,上限预警,正常|001D01,开,27,0,0态,正常');
*/
function RefPointSsz(PointSszStr) {
    if (PointSszStr.length < 1) {
        //停止所有动画，清除实时数据
        var allOverlay = map.getOverlays();
        for (var i = 0; i < allOverlay.length; i++) {
            if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
                if (allOverlay[i].getBindTypeName()) {
                    if (allOverlay[i].getBindTypeName() == "2") {//SVG图元               
                        allOverlay[i].endAnmi();
                    }
                    else if (allOverlay[i].getBindTypeName() == "3") {//Gif图元
                        var pic = 'mx/Gif/' + allOverlay[i].getBindUnitName() + '_0.gif'; //不动作gif
                        allOverlay[i].setImageStop(pic);
                    }
                    else if (allOverlay[i].getBindTypeName() == "0") {
                        allOverlay[i].setText("");
                        //更新背景图片
                        picL = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_20_Left.gif';
                        picM = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_20_Middle.gif';
                        picR = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_20_Rright.gif';
                        allOverlay[i].setImage0(picL, picM, picR);
                    }
                    else if (allOverlay[i].getBindTypeName() == "1") {
                        if (allOverlay[i].getBindUnitName().indexOf('&') < 0) {
                            allOverlay[i].setText("");
                        }
                    }
                    allOverlay[i].setPointState("-1");
                }
            }
        }
        if (RefFlag) {//定时刷新
            window.setTimeout("DoRefPointSsz()", 5000);
        }
        return;
    }
    var PointSszObjStr = [];
    var PointSszObjList = [];
    PointSszObjStr = PointSszStr.split('|');
    for (var i = 0; i < PointSszObjStr.length; i++) {
        var pointssz = PointSszObjStr[i].split(',');
        var PointSszObj = {};
        PointSszObj.point = pointssz[0];
        PointSszObj.ssz = pointssz[1];
        PointSszObj.state = pointssz[2];//状态
        PointSszObj.statenumber = pointssz[3];//数据状态值
        PointSszObj.alarm = pointssz[4];
        PointSszObj.datatext = pointssz[5]; //数据状态
        PointSszObj.statetext = pointssz[6]; //设备状态

        PointSszObj.callstate = pointssz[7]; //人员设备是否呼叫

        PointSszObjList[PointSszObj.point] = PointSszObj;
    }
    var allOverlay = map.getOverlays();
    //var value = Math.random() * 10;
    //value = Math.round(value * 100) / 100;
    var picL = null, picM = null, picR = null;
    var pic = null;

    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            //allOverlay[i].setContent(getLableDiv(value, value + 'm/s'));
            if (allOverlay[i].getBindTypeName()) {
                if (PointSszObjList[allOverlay[i].getJcdName()]) {//如果实时值列表中存在当前图元的实时值
                    if (allOverlay[i].getBindTypeName() == "0") {//悬浮实时显示图元

                        allOverlay[i].setText(PointSszObjList[allOverlay[i].getJcdName()].ssz);



                        DeletePointGif(allOverlay[i].getJcdName());
                        //人员呼叫状态设置动画                 
                        if (PointSszObjList[allOverlay[i].getJcdName()].callstate == "1") {
                            DrawPointGif(allOverlay[i].getJcdName(), 'red', '0');
                        }
                        else if (PointSszObjList[allOverlay[i].getJcdName()].callstate == "2") {
                            DrawPointGif(allOverlay[i].getJcdName(), 'blue', '0');
                        }
                        else if (PointSszObjList[allOverlay[i].getJcdName()].callstate == "3") {
                            DrawPointGif(allOverlay[i].getJcdName(), 'green', '0');
                        }

                        if (allOverlay[i].getPointState() != PointSszObjList[allOverlay[i].getJcdName()].state) {

                            //更新背景图片
                            if (allOverlay[i].getBindUnitName() == "无背景") {
                                if (parseInt(PointSszObjList[allOverlay[i].getJcdName()].alarm) > 0 || PointSszObjList[allOverlay[i].getJcdName()].ssz == "断线") {//报警显示处理
                                    allOverlay[i].setTextColor("red");
                                }
                                else {
                                    allOverlay[i].setTextColor("green");
                                }
                            }
                            else if (allOverlay[i].getBindUnitName() == "实时显示2") {
                                if (parseInt(PointSszObjList[allOverlay[i].getJcdName()].alarm) > 0 || PointSszObjList[allOverlay[i].getJcdName()].ssz == "断线") {//报警显示处理
                                    allOverlay[i].setTextColor("red");
                                }
                                else {
                                    allOverlay[i].setTextColor("white");
                                }
                            }
                            else {
                                picL = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_' + PointSszObjList[allOverlay[i].getJcdName()].statenumber + '_Left.gif';
                                picM = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_' + PointSszObjList[allOverlay[i].getJcdName()].statenumber + '_Middle.gif';
                                picR = 'mx/Text/' + allOverlay[i].getBindUnitName() + '_' + PointSszObjList[allOverlay[i].getJcdName()].statenumber + '_Rright.gif';
                                allOverlay[i].setImage0(picL, picM, picR);

                                //巷道图元实时值显示
                                if (PointSszObjList[allOverlay[i].getJcdName()].alarm == "1") {//报警显示处理
                                    DrawAlarmRoad(allOverlay[i].getJcdName());
                                    //DrawPointAnmi(allOverlay[i].getJcdName(), 'red', '0');
                                    //DrawPointGif(allOverlay[i].getJcdName(), 'red', '0');
                                }
                                else {
                                    ClearAlarmRoad(allOverlay[i].getJcdName());
                                    //StopPointAnmi(allOverlay[i].getJcdName());
                                    //DeletePointGif(allOverlay[i].getJcdName());
                                }
                            }
                        }
                    }
                    else if (allOverlay[i].getBindTypeName() == "1") {//拓扑图图元
                        if (allOverlay[i].getBindUnitName().indexOf('&') < 0) {
                            allOverlay[i].setText(PointSszObjList[allOverlay[i].getJcdName()].ssz);
                        }
                        if (parseInt(PointSszObjList[allOverlay[i].getJcdName()].alarm) > 0 || PointSszObjList[allOverlay[i].getJcdName()].ssz=="断线") {//报警显示处理
                            allOverlay[i].setTextColor("rgb(255,0,0)");
                        }
                        else {
                            allOverlay[i].setTextColor("rgb(0,0,0)");
                        }
                    }
                    else if (allOverlay[i].getBindTypeName() == "2") {//SVG图元
                        if (allOverlay[i].getPointState() != PointSszObjList[allOverlay[i].getJcdName()].state) {
                            if (PointSszObjList[allOverlay[i].getJcdName()].state == allOverlay[i].getanimationState()) {
                                allOverlay[i].beginAnmi();
                            }
                            else {
                                allOverlay[i].endAnmi();
                                allOverlay[i].setBgColor("rgb(255,0,0)");
                            }
                        }
                    }
                    else if (allOverlay[i].getBindTypeName() == "3") {//Gif图元
                        if (allOverlay[i].getBindUnitName().indexOf("gif") == 0) {
                            pic = 'mx/Gif/' + allOverlay[i].getBindUnitName() + '_' + PointSszObjList[allOverlay[i].getJcdName()].statenumber + '.gif';
                            allOverlay[i].setImageUrl(pic);
                        }
                        else {

                            //更新填充图元实时值
                            allOverlay[i].setHeight(PointSszObjList[allOverlay[i].getJcdName()].ssz);

                            if (allOverlay[i].getPointState() != PointSszObjList[allOverlay[i].getJcdName()].state) {
                                if (PointSszObjList[allOverlay[i].getJcdName()].state == allOverlay[i].getanimationState()) {//动作
                                    allOverlay[i].setImageRun();
                                }
                                else {//不动作
                                    allOverlay[i].setImageStop();
                                }

                            }
                        }
                    }
                    else if (allOverlay[i].getBindTypeName() == "4") {//标注图元
                        //更新背景图片
                        picL = 'mx/Marker/' + allOverlay[i].getBindUnitName() + '_' + PointSszObjList[allOverlay[i].getJcdName()].statenumber + '.png';
                        allOverlay[i].setImage(pic);
                    }
                    allOverlay[i].setPointState(PointSszObjList[allOverlay[i].getJcdName()].state);
                    allOverlay[i].setPointDataText(PointSszObjList[allOverlay[i].getJcdName()].datatext);
                    allOverlay[i].setPointStateText(PointSszObjList[allOverlay[i].getJcdName()].statetext);
                }
            }
        }
    }

    if (RefFlag) {//定时刷新
        window.setTimeout("DoRefPointSsz()", 5000);
    }
}

//图元动画效果对象列表
var Point_AnmiList = [];
//图元动画效果对象
var Point_Anmi = function (pointName, circle, timer, AnmiFlag) {

    this.Point_Name = pointName;
    this.Point_AnmiIndex = 0;
    this.Point_circle = circle;
    this.Point_AnmiFlag = AnmiFlag;
    this.Point_timer = timer;
}
//设置图元动画闪烁效果
function PointSetAnmi() {
    for (var i = Point_AnmiList.length - 1; i >= 0; i--) {
        if (Point_AnmiList[i].Point_AnmiFlag) {
            //闪烁
            mxLib.Util.setAnimation(Point_AnmiList[i].Point_circle.getDomElement(), MX_ANIMATION_FLASH, 1000);
            //扩大 
            Point_AnmiList[i].Point_circle.setRadius(Point_AnmiList[i].Point_circle.getRadius());

            Point_AnmiList[i].Point_circle.snap().animate({
                r: Point_AnmiList[i].Point_circle.getRadius()
            }, 1000);
            if (Point_AnmiList[i].Point_timer > 0) {
                Point_AnmiList[i].Point_AnmiIndex++;
                if (Point_AnmiList[i].Point_AnmiIndex * 1000 > Point_AnmiList[i].Point_timer) {
                    Point_AnmiList[i].Point_AnmiFlag = false;
                }
            }
        }
        else {
            //动画效果完成后删除覆盖物       
            map.removeOverlay(Point_AnmiList[i].Point_circle);
            Point_AnmiList.splice(i, 1);
        }
    }
    window.setTimeout("PointSetAnmi()", 1000);
}
window.setTimeout("PointSetAnmi()", 2000);
/**
* 停止图元动画效果
* @class StopPointAnmi 
* @param {PointName} 测点号 
* @returns 返回值：无
* @example 参考示例：
* StopPointAnmi('001A010');
*/
function StopPointAnmi(PointName) {
    for (var i = 0; i < Point_AnmiList.length; i++) {
        if (Point_AnmiList[i].Point_Name == PointName) {
            Point_AnmiList[i].Point_AnmiFlag = false;
        }
    }
}
//绘制图元闪烁动画效果
/**
* 绘制图元闪烁动画效果
* @class DrawPointAnmi 
* @param {PointName} 测点号
* @param {color} 绘制闪烁动画颜色 如：'red','green'
* @param {timer} 持续时间 单位：毫秒
* @returns 返回值：无
* @example 参考示例：
* DrawPointAnmi('001A010','red','5000');
*/
function DrawPointAnmi(PointName, color, timer) {
    var tempPoint;
    //查找RichMarker图元缓存
    for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
        if (pointRichMarkerMapArr[i].pointName == PointName) {
            tempPoint = pointRichMarkerMapArr[i].pointPos;
            break;
        }
    }
    //查找RichMarker图元缓存
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        if (TBpointVectorLayerArr[i].pointName == PointName) {
            tempPoint = TBpointVectorLayerArr[i].overlay.getBounds().getCenter();
            break;
        }
    }
    //查找RichMarker图元缓存
    for (var i = 0; i < SVGpointVectorLayerArr.length; i++) {
        if (SVGpointVectorLayerArr[i].pointName == PointName) {
            tempPoint = SVGpointVectorLayerArr[i].vl.getBounds().getCenter();
            break;
        }
    }
    //map.setCenter(tempPoint);
    //添加一个跳动动画
    //mxLib.Util.setAnimation(pointRichMarkerMap[PointName.getValue()].getDomElement(), MX_ANIMATION_BOUNCE,3000);


    var isin = false;
    for (var i = 0; i < Point_AnmiList.length; i++) {
        if (Point_AnmiList[i].Point_Name == PointName) {
            isin = true;
        }
    }
    if (!isin) {
        //闪烁背景效果 
        var circle = new mxLib.Circle(tempPoint, 20, {
            strokeColor: color,
            strokeWidth: 2,
            strokeOpacity: 0.5,
            fillOpacity: 0.5,
            enableClicking: true,
            fillColor: color
        });
        circle.setJcdName(PointName + "Anmi");
        map.addOverlay(circle);
        var tempAnmi = new Point_Anmi(PointName, circle, parseInt(timer), true);
        Point_AnmiList.push(tempAnmi);
    }
}
/**
* 绘制地图点闪烁动画效果
* @class DrawXYAnmi 
* @param {x} 点的坐标（经度）
* @param {y} 点的坐标（纬度）
* @param {color} 绘制闪烁动画颜色 如：'red','green'
* @param {timer} 持续时间 单位：毫秒
* @returns 返回值：无
* @example 参考示例：
* DrawXYAnmi('106.56463893487008','24.552746147915386','red','5000');
*/
function DrawXYAnmi(x, y, color, timer) {

    var tempPoint = new mxLib.Point();
    tempPoint.x = x;
    tempPoint.y = y;
    var PointName = x + "-" + y;

    var isin = false;
    for (var i = 0; i < Point_AnmiList.length; i++) {
        if (Point_AnmiList[i].Point_Name == PointName) {
            isin = true;
        }
    }
    if (!isin) {
        //闪烁背景效果 
        var circle = new mxLib.Circle(tempPoint, 0.03, {
            strokeColor: color,
            strokeWidth: 2,
            strokeOpacity: 0.5,
            fillOpacity: 0.5,
            enableClicking: true,
            fillColor: color
        });
        circle.setJcdName(PointName + "Anmi");
        map.addOverlay(circle);
        var tempAnmi = new Point_Anmi(PointName, circle, parseInt(timer), true);
        Point_AnmiList.push(tempAnmi);
    }
}

function HidePointGif(PointName) {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker) {
            if (PointName + "Gif" == allOverlay[i].getJcdName()) {
                //map.removeOverlay(allOverlay[i]);
                //console.log("删除报警效果：" + PointName);
                allOverlay[i].hide();
            }
        }
    }
}

function DeletePointGif(PointName) {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker) {
            if (PointName + "Gif" == allOverlay[i].getJcdName()) {
                map.removeOverlay(allOverlay[i]);
                //console.log("删除报警效果：" + PointName);
                //allOverlay[i].hide();
            }
        }
    }
}

function DrawPointGif(PointName, color, timer) {
    var tempPoint;
    var tempOverly;
    //查找RichMarker图元缓存
    for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
        if (pointRichMarkerMapArr[i].pointName == PointName) {
            tempPoint = pointRichMarkerMapArr[i].pointPos;
            tempOverly = pointRichMarkerMapArr[i].overlay;
            break;
        }
    }
    //查找拓扑图元缓存
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        if (TBpointVectorLayerArr[i].pointName == PointName) {
            tempPoint = TBpointVectorLayerArr[i].overlay.getBounds().getCenter();
            tempOverly = TBpointVectorLayerArr[i].overlay;
            break;
        }
    }
    //查找SVG动画图元缓存
    for (var i = 0; i < SVGpointVectorLayerArr.length; i++) {
        if (SVGpointVectorLayerArr[i].pointName == PointName) {
            tempPoint = SVGpointVectorLayerArr[i].vl.getBounds().getCenter();
            tempOverly = SVGpointVectorLayerArr[i].vl;
            break;
        }
    }
    //map.setCenter(tempPoint);
    //添加一个跳动动画
    //mxLib.Util.setAnimation(pointRichMarkerMap[PointName.getValue()].getDomElement(), MX_ANIMATION_BOUNCE,3000);

    //判断当前效果是否存在
    var isinMap = false;
    var index = 0;
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker) {
            if (PointName + "Gif" == allOverlay[i].getJcdName()) {
                isinMap = true;
                index = i;
                break;
            }
        }
    }
    if (!isinMap) {//如果地图上没有当前点的报警动画，则添加
        //console.log("添加报警效果：" + PointName);
        //闪烁背景效果
        var html = "<div style='position: absolute; margin-top: 0px; margin-right: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 0px; padding-right: 0px; padding-bottom: 0px; padding-left: 0px; width: 128px; height: 128px; overflow-x: hidden; overflow-y: hidden; '><img src='mx/mxImg/gif/" + color + ".gif' style='display: block; border:none;margin-left:0px; margin-top:0px; '></div>";
        var mxLibSize = new mxLib.Size(-(128 / 2), -(128 / 2));
        var circle = new mxLib.RichMarker(html, tempPoint, {
            "anchor": mxLibSize
        });
        circle.setJcdName(PointName + "Gif");
        //console.log("报警效果总数：" + map.getOverlays().length);
        map.addOverlay(circle);
        if (tempOverly.getDomElement().style.zIndex) {
            circle.getDomElement().style.zIndex = parseInt(tempOverly.getDomElement().style.zIndex) - 1;
        }
    }
    else {
        allOverlay[index].show();
    }

    if (parseInt(timer) > 0) {
        //动画效果完成后删除覆盖物
        window.setTimeout(function () {
            DeletePointGif(PointName);
        }, parseInt(timer));
    }
}
//绘制报警路径
function DrawAlarmRoad(PointName) {
    var count = zhpolylineList.length;
    var isinMap = false;
    var zhpolyindex = 0;
    for (var i = count - 1; i >= 0; i--) {
        if (zhpolylineList[i].point == PointName) {
            zhpolyindex = i;
            isinMap = true;
        }
    }
    for (var i = 0; i < allPointTranMapArr.length; i++) {
        if (allPointTranMapArr[i].sPointName == PointName) {
            var points = [];
            var index = 0;
            for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
                points[index] = new mxLib.Point(allPointTranMapArr[i].points[j].x, allPointTranMapArr[i].points[j].y);
                index++;
            }
            //添加当前点的报警路径
            if (points.length > 0) {
                if (!isinMap) {
                    var zhpolyline = {};
                    zhpolyline.point = PointName;
                    zhpolyline.line = new mxLib.Polyline(points
                                , { strokeColor: "red", strokeWidth: 10, strokeOpacity: 0.8, enableClicking: true });
                    map.addOverlay(zhpolyline.line);
                    //mxLib.Util.setAnimation(zhpolyline.line.getDomElement(), MX_ANIMATION_FLASH, 10000);
                    zhpolylineList.push(zhpolyline);
                }
                else {
                    zhpolylineList[zhpolyindex].line.setStrokeColor("red");
                }
            }
        }
    }

}
//清除报警路径
function ClearAlarmRoad(PointName) {
    var count = zhpolylineList.length;
    var isinMap = false;
    var zhpolyindex = 0;
    for (var i = count - 1; i >= 0; i--) {
        if (zhpolylineList[i].point == PointName) {
            zhpolyindex = i;
            isinMap = true;
        }
    }
    for (var i = 0; i < allPointTranMapArr.length; i++) {
        if (allPointTranMapArr[i].sPointName == PointName) {
            var points = [];
            var index = 0;
            for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
                points[index] = new mxLib.Point(allPointTranMapArr[i].points[j].x, allPointTranMapArr[i].points[j].y);
                index++;
            }
            //添加当前点的报警路径
            if (points.length > 0) {
                if (!isinMap) {
                    var zhpolyline = {};
                    zhpolyline.point = PointName;
                    zhpolyline.line = new mxLib.Polyline(points
                                , { strokeColor: "green", strokeWidth: 10, strokeOpacity: 0.8, enableClicking: true });
                    map.addOverlay(zhpolyline.line);
                    //mxLib.Util.setAnimation(zhpolyline.line.getDomElement(), MX_ANIMATION_FLASH, 10000);
                    zhpolylineList.push(zhpolyline);
                }
                else {
                    zhpolylineList[zhpolyindex].line.setStrokeColor("green");
                }
            }
        }
    }
}
//设置显示或隐藏覆盖物和热点
/**
* 测点显示/隐藏
* @class setPointShow 
* @param {PointStr} 测点号,多个用“|”分隔 如：001A01|001A02
* @param {flag} 显示隐藏标记 如：0:表示隐藏，1：表示显示
* @returns 无
* @example 参考示例：
* setPointShow('001A01|001A02','0');
*/
function setPointShow(PointStr, flag) {
    var PointArr = PointStr.split('|');
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getJcdName()) {
                for (var j = 0; j < PointArr.length; j++) {
                    if (allOverlay[i].getJcdName() == PointArr[j]) {
                        pointshowORhide(allOverlay[i], flag);
                    }
                }
            }
        }
    }
}
//设置图元显示或者隐藏0-隐藏，1-显示
function pointshowORhide(overlay, flag) {
    if (flag == "0") {
        overlay.hide();
        DeletePointGif(overlay.getJcdName());
        //map.removeHotspot(allOverlay.hotspot);
        //allOverlay.hotspot = null;
        if (overlay.getBindTypeName() == "0" || overlay.getBindTypeName() == "3") {
            //隐藏删除当前点的连接线及对象
            var tempPostion = overlay.getPosition();
            for (var j = allPointTranMapArr.length - 1; j >= 0; j--) {
                if (allPointTranMapArr[j].sPointPos.equals(tempPostion) || allPointTranMapArr[j].ePointPos.equals(tempPostion)) {
                    allPointTranMapArr[j].polyline.hide();
                }
            }
            //隐藏实时显示巷道对象
            var count = zhpolylineList.length;
            for (var i = count - 1; i >= 0; i--) {
                if (zhpolylineList[i].point == overlay.getJcdName()) {
                    zhpolylineList[i].line.hide();
                }
            }
        }
        else if (overlay.getBindTypeName() == "1") {
            //隐藏当前点的连接线及对象
            var tempPostion = overlay.getBounds().getCenter();
            var tempPoint = overlay.getJcdName();
            for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                    allPointTbObjList[j].polyline.hide();
                }
            }
        }
    } else {
        overlay.show();
        if (overlay.getBindTypeName() == "0" || overlay.getBindTypeName() == "3") {
            //显示当前点的连接线及对象
            var tempPostion = overlay.getPosition();
            for (var j = allPointTranMapArr.length - 1; j >= 0; j--) {
                if (allPointTranMapArr[j].sPointPos.equals(tempPostion) || allPointTranMapArr[j].ePointPos.equals(tempPostion)) {
                    allPointTranMapArr[j].polyline.show();
                }
            }
            //显示实时显示巷道对象
            var count = zhpolylineList.length;
            for (var i = count - 1; i >= 0; i--) {
                if (zhpolylineList[i].point == overlay.getJcdName()) {
                    zhpolylineList[i].line.show();
                }
            }
        }
        else if (overlay.getBindTypeName() == "1") {
            //显示当前点的连接线及对象
            var tempPostion = overlay.getBounds().getCenter();
            var tempPoint = overlay.getJcdName();
            for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                    allPointTbObjList[j].polyline.show();
                }
            }
        }
        //var point = allOverlay.getPosition();
        //map.setCenter(point);
        //        var hotspot = new mxLib.Hotspot(point, { offsets: [1, 80, 26, 1], text: "123123123", minZoom: 0, maxZoom: 10, userData: "我是北京!" });
        //        allOverlay.hotspot = hotspot;
        //        map.addHotspot(hotspot);
    }
}

//添加游标
var point = map.getCenter();
var opts = {
    position: point,    // 指定文本标注所在的地理位置
    offset: new mxLib.Size(2, 2)    //设置文本偏移量
}
var label = new mxLib.Label("", opts);  // 创建文本标注对象
label.setStyle({
    color: "black",
    fontSize: "12px",
    borderColor: "green",
    lineHeight: "20px",
    fontFamily: "微软雅黑"
});
map.addOverlay(label);
label.hide(); //先隐藏


//给所有图元加ToolTips
function addToolTips() {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getJcdName()) {
                allOverlay[i].addEventListener("onmouseover", function (e) {

                    //设置label的zindex
                    var newZindex = 999999999;
                    label.setStyle({ 'z-index': newZindex });

                    //label.setTop(true);

                    var pointname = e.target.getJcdName();
                    var pointwz = "";
                    var pointdevname = "";
                    var pointstatetext = "";
                    var pointdatatext = "";
                    var pointTipContent = "";
                    var GrapUnitName = e.target.getBindUnitName();
                    if (e.target.getJcdWz()) {
                        pointwz = e.target.getJcdWz();
                    }
                    if (e.target.getJcdDevName()) {
                        pointdevname = e.target.getJcdDevName();
                    }
                    if (e.target.getPointStateText()) {
                        pointstatetext = e.target.getPointStateText();
                    }
                    if (e.target.getPointDataText()) {
                        pointdatatext = e.target.getPointDataText();
                    }
                    if (e.target.getPointToolTip()) {
                        pointTipContent = e.target.getPointToolTip();
                    }
                    if (GrapUnitName.indexOf('&') >= 0) {
                        label.setContent("编号：" + pointname);
                        //label.setStyle({ height: "20px" });
                    }
                    else {
                        if (IsGraphicEdit) {
                            if (pointTipContent.length > 0) {
                                label.setContent(pointTipContent);
                            }
                            else {
                                label.setContent("编号：" + pointname + "<br/>位置：" + pointwz
                               + "<br/>类型：" + pointdevname);
                                //label.setStyle({ height: "60px" });
                            }
                        }
                        else {
                            if (pointTipContent.length > 0) {
                                label.setContent(pointTipContent);
                            }
                            else {
                                label.setContent("编号：" + pointname + "<br/>位置：" + pointwz
                                + "<br/>类型：" + pointdevname
                                + "<br/>数据状态：" + pointdatatext
                                + "<br/>运行状态：" + pointstatetext);
                                //label.setStyle({ height: "100px" });
                            }
                        }
                    }
                    var point = new mxLib.Point(e.point.x, e.point.y);
                    label.setPosition(point);
                    label.show();

                });

                allOverlay[i].addEventListener("onmouseout", function (e) {
                    label.hide();
                });
            }
        }
    }
}

/**
* 给每个测点添加用户自定义tooltips
* @class UseraddTollTips 
* @param {PointTipListStr} 测点号|Tip内容（文本内容，换行用'<br/>'）,多个用“,”分隔 如：001A01|测点号：001A01<br/>安装位置：XX位置1,001A02|测点号：001A02<br/>安装位置：XX位置2
* @returns 无
* @example 参考示例：
* UseraddTollTips('001A01|测点号：001A01<br/>安装位置：XX位置1,001A02|测点号：001A02<br/>安装位置：XX位置2');
*/
function UseraddTollTips(PointTipListStr) {
    var pointTipArr = [];
    var strtemp = PointTipListStr.split(',');
    for (var j = 0; j < strtemp.length; j++) {
        var strtempson = strtemp[j].split('|');
        var pointTip = {};
        pointTip.Point = strtempson[0];
        pointTip.TipContent = strtempson[1];
        pointTipArr.push(pointTip);
    }

    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            for (var j = 0; j < pointTipArr.length; j++) {
                if (allOverlay[i].getJcdName() == pointTipArr[j].Point) {
                    allOverlay[i].setPointToolTip(pointTipArr[j].TipContent);
                }
            }
        }
    }
    //调用接口给所有图元添加tooltips
    addToolTips();
}
/**
* 根据点的x,y坐标，给测点添加用户自定义tooltips
* @class UserAddToolTipsByXY 
* @param {x} x坐标
* @param {y} y坐标
* @param {content} 显示内容
* @returns 无
* @example 参考示例：
* UserAddToolTipsByXY('106.56463893487008','24.552746147915386','测点号：001A02<br/>安装位置：XX位置2');
*/
function UserAddToolTipsByXY(x, y, content) {
    //设置label的zindex
    var newZindex = 99999999;
    label.setStyle({ 'z-index': newZindex });
    label.setContent(content);
    var point = new mxLib.Point(x, y);
    label.setPosition(point);
    label.show();
}
/**
* 隐藏ToolTips
* @class UserDelToolTips 
* @param 无
* @returns 无
* @example 参考示例：
* UserDelToolTips();
*/
function UserDelToolTips() {
    label.hide();
}
