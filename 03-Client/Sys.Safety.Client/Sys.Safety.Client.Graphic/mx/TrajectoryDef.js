/*
* 人员轨迹绘制 
*/

//所有矩形覆盖物数组对象
//console.log("trajectorydef.js");
var OverlayRectArr = new Array();

////所有测点路径数组对象
//var allPointTrance = [];
var allPointTranList = [];
////所有轨迹线路数组对象
//var allpolylineList = [];

//保存所有的轨迹线路及测点对象
var allPointTranMapArr = [];

//所有巷道报警轨迹线路数组对象
var zhpolylineList = [];
//判断当前是点的确定结束，还是用户结束命令按钮
var isByButton = false;
     


//定义图形绘制命令--------------------------------------------
function LoadTrajectoryDef() {
    /* 
    * @exports TrajectoryDef as mxLib.Command
    */
    var TrajectoryDef =
    /*
    *TrajectoryDef类的构造函数
    * 矩形缩放命令类
    * @param  {String} name 命令名称 
    * @param  {Command} parentCmd 调用命令对象 
    * @param {Json Object} opts 命令选项
    * @constructor
    * @see mxLib.Map#selEntity
    */
    mxLib.TrajectoryDef = function TrajectoryDef(name, parentCmd, opts) {
        mxLib.Command.prototype.constructor.call(this, name, parentCmd, opts);
    }

    //继承自Command类
    mxLib.Lang.inherits(TrajectoryDef, mxLib.Command);

    /*
    * 抽象方法，开始命令
    * @param {Map} map 地图对象
    * @returns {None} 
    */
    //    TrajectoryDef.prototype.start = function (map) {
    //        mxLib.Command.prototype.start.call(this, map);
    //        this.points = [];
    //        map.setCursor("crosshair");
    //    }


    /*
    * 抽象方法，左键按下事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    //    TrajectoryDef.prototype.leftMousedown = function (e, data) {
    //            
    //    }



    /*
    * 抽象方法，左键抬起事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TrajectoryDef.prototype.leftMouseup = function (e, data) {
        var point = mxLib.Util.getEventPoint(this.map, e);
        for (var i = 0; i < OverlayRectArr.length; i++) {
            if (OverlayRectArr[i].isPointInRect(point)) {
                if (!this.startPoint) {
                    this.startPoint = OverlayRectArr[i].getOverlay().getPosition();
                    //轨迹路线对象开始点信息
                    this.PointTranObj.sPointName = OverlayRectArr[i].getOverlay().getJcdName();
                    this.PointTranObj.sPointPos = OverlayRectArr[i].getOverlay().getPosition();
                    this.points.push(new mxLib.Point(this.startPoint.x, this.startPoint.y));
                    this.points.push(new mxLib.Point(point.x, point.y));
                    var polyline = new mxLib.Polyline(this.points, { strokeColor: "green", strokeWidth: 3, strokeOpacity: 0.8, enableClicking: false });
                    this.map.addOverlay(polyline);
                    this.polyline = polyline;
                    //mxLib.Util.toastInfo(this.startPoint.x, { delay: 8000 });
                    return;
                }
                else//绘制最后一个点并结束
                {
                    this.endPoint = OverlayRectArr[i].getOverlay().getPosition();
                    //轨迹路线对象结束点信息
                    this.PointTranObj.ePointName = OverlayRectArr[i].getOverlay().getJcdName();
                    this.PointTranObj.ePointPos = OverlayRectArr[i].getOverlay().getPosition();
                    this.points[this.points.length - 1] = this.endPoint;
                    this.polyline.setPath(this.points);

                    //判断当前路线之前是否有重复路径，如有重复路径，将之前路径删除
                    for (var i = 0; i < allPointTranMapArr.length; i++) {
                        if (allPointTranMapArr[i].points[0].equals(this.startPoint) && allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1].equals(this.endPoint) ||
                        allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1].equals(this.startPoint) && allPointTranMapArr[i].points[0].equals(this.endPoint)) {
                            map.removeOverlay(allPointTranMapArr[i].polyline);
                            allPointTranMapArr.splice(i, 1);
                        }
                    }


                    //添加到线路数组
                    this.PointTranObj.polyline = this.polyline;
                    //添加到路线点数组
                    this.PointTranObj.points = this.points;

                    allPointTranMapArr.push(this.PointTranObj);



                    //清空对象并结束
                    this.startPoint = null;
                    this.endPoint = null;
                    this.polyline = null;
                    this.points = [];

                    this.end();
                    return;
                }
            }
        }
        if (!this.startPoint) return;

        this.points[this.points.length - 1] = point;
        this.points.push(new mxLib.Point(point.x, point.y));
        this.polyline.setPath(this.points);
    }

    /*
    * 抽象方法，右键抬起事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TrajectoryDef.prototype.rightMouseup = function (e, data) {

        this.map.showContextMenu(e);
    }

    /*
    * 抽象方法，鼠标移动事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TrajectoryDef.prototype.mousemove = function (e, data) {
        var point = mxLib.Util.getEventPoint(this.map, e);
        if (this.points.length > 0) {
            this.polyline.setPositionAt(this.polyline.getPath().length - 1, point);
        }
    }

    /*
    * 抽象方法，开始命令
    * @param {Map} map 地图对象
    * @returns {None} 
    */
    TrajectoryDef.prototype.start = function (map) {
        mxLib.Command.prototype.start.call(this, map);
        this.points = [];
        this.PointTranObj = {}; //定义轨迹路线对象
        map.setCursor("crosshair");
        //根据参数添加右键结束功能

        var self = this;
        var contextMenu = new mxLib.ContextMenu();
        var menu1 = new mxLib.MenuItem("确定", "", "", function (e) {
            isByButton = false;
            self.end();
        });
        contextMenu.appendItem(menu1);

        var menu2 = new mxLib.MenuItem("取消", "", "", function (e) {
            self.points.length = 0;
            self.end();
        });
        contextMenu.appendItem(menu2);
        map.addContextMenu(contextMenu);
        //        var self = this;
        //        var contextMenu = new mxLib.ContextMenu();
        //        var menu1 = new mxLib.MenuItem("保存路径", "", "", function (e) {
        //            //            self.end();
        //            var allPointStr = "";
        //            allPointTranList.length=0;
        //            for (var i = 0; i < allPointTrance.length; i++) {
        //                for (var j = 0; j < allPointTrance[i].length; j++) {
        //                    allPointStr += allPointTrance[i][j].x + "," + allPointTrance[i][j].y + "|";
        //                    allPointTranList.push(allPointTrance[i][j]);
        //                }
        //                allPointStr = allPointStr.substring(0, allPointStr.length - 1) + "&";
        //            }
        //            allPointStr = allPointStr.substring(0, allPointStr.length - 1);
        //            mxLib.Util.toastInfo(allPointStr, { delay: 8000 });

        //            self.end();
        //        });
        //        contextMenu.appendItem(menu1);

        //        var menu2 = new mxLib.MenuItem("清除路径", "", "", function (e) {
        //            //            self.points.length = 1;
        //            //删除所有轨迹线路
        //            for (var i = 0; i < allpolylineList.length; i++) {
        //                map.removeOverlay(allpolylineList[i]);
        //            }
        //            //轨迹路线点数组重置
        //            allPointTrance.length = 0;
        //            allPointTranList.length = 0;
        //            //删除所有线路
        //            allpolylineList.length = 0;
        //            
        //            self.end();
        //        });
        //        contextMenu.appendItem(menu2);
        //        map.addContextMenu(contextMenu);
    }

    /*
    * 抽象方法，结束命令
    * @param {Boolean} bCancel 是否取消
    * @returns {None} 
    */
    TrajectoryDef.prototype.end = function (bCancel) {
        //查看是否为点确定结束，如果是，则保存单点的路线轨迹  
        if (this.points.length > 0) {
            if (isByButton == true) {
                this.points.splice(this.points.length - 1, 2);
            }
            else{
                this.points.splice(this.points.length - 2, 2);
            }
            this.endPoint = this.points[this.points.length-1];
            //轨迹路线对象结束点信息
            this.PointTranObj.ePointName = "无";
            this.PointTranObj.ePointPos = this.endPoint;
            //this.points[this.points.length - 1] = this.endPoint;
            this.polyline.setPath(this.points);

            //判断当前路线之前是否有重复路径，如有重复路径，将之前路径删除
            for (var i = 0; i < allPointTranMapArr.length; i++) {
                if (allPointTranMapArr[i].points[0].equals(this.startPoint) && allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1].equals(this.endPoint) ||
                allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1].equals(this.startPoint) && allPointTranMapArr[i].points[0].equals(this.endPoint)) {
                    map.removeOverlay(allPointTranMapArr[i].polyline);
                    allPointTranMapArr.splice(i, 1);
                }
            }


            //添加到线路数组
            this.PointTranObj.polyline = this.polyline;
            //添加到路线点数组
            this.PointTranObj.points = this.points;

            allPointTranMapArr.push(this.PointTranObj);



            //清空对象并结束
            this.startPoint = null;
            this.endPoint = null;
            this.polyline = null;
            this.points = [];
        }
        //删除所有矩形
        for (var i = 0; i < OverlayRectArr.length; i++) {
            OverlayRectArr[i].removeRectOverlay();
        }
        if (this.polyline) {
            map.removeOverlay(this.polyline);
        }
        mxLib.Command.prototype.end.call(this, bCancel);
    }
}
LoadTrajectoryDef();
//定义图形绘制命令--------------------------------------------

//定义矩形覆盖物----------------------------------------------
OverlayRect = function (map, overlay) {
    this.map = map;
    this.overlay = overlay;
    //得到覆盖物的矩形
    var width = $(overlay.getDomElement()).children("div").width();
    var height = $(overlay.getDomElement()).children("div").height();
    var left = $(overlay.getDomElement()).offset().left;
    var top = $(overlay.getDomElement()).offset().top;
    var p1 = new mxLib.Pixel(left, top); //像素坐标
    var p2 = new mxLib.Pixel(left + width, top + height);
    var pt1 = map.pixelToPoint(p1);
    var pt2 = map.pixelToPoint(p2);
    this.rect = new mxLib.Bounds(pt1, pt2); //地理坐标
}

/*
* 点的地理坐标是否位于此矩形内。
* @param {Point} point 地理坐标 
* @returns {boolean} 类型判断结果 true相等;false不相等
*/
OverlayRect.prototype.isPointInRect = function (point) {
    return this.rect.containsPoint(point);
}

OverlayRect.prototype.getOverlay = function () {
    return this.overlay;
}

/*
* 创建矩形覆盖物。
* @returns {Overlay} 
*/
OverlayRect.prototype.createRectOverlay = function () {
    var points = [];
    points.push(this.rect.getLeftTop());
    points.push(this.rect.getRightTop());
    points.push(this.rect.getRightBottom());
    points.push(this.rect.getLeftBottom());
    var polygon = new mxLib.Polygon(points, { strokeColor: "#3399FF", fillColor: "#A8CAEC", strokeWidth: 1, strokeOpacity: 0.9, enableClicking: false });
    this.map.addOverlay(polygon);
    this.polygon = polygon;
    return polygon;
}

/*
* 删除矩形覆盖物。
* @return
*/
OverlayRect.prototype.removeRectOverlay = function () {
    if (this.polygon) {
        map.removeOverlay(this.polygon);
    }
}

/*
* 设置矩形覆盖物的颜色。
* @param {String} color
* @return
*/
OverlayRect.prototype.setColor = function (color) {
    if (this.polygon) {
        this.polygon.setFillColor(color);
    }
}

/*
* 删除矩形覆盖物。
* @return
*/
OverlayRect.prototype.draw = function () {
    if (!this.polygon) return;
    var width = $(this.overlay.getDomElement()).children("div").width();
    var height = $(this.overlay.getDomElement()).children("div").height();
    var left = $(this.overlay.getDomElement()).offset().left;
    var top = $(this.overlay.getDomElement()).offset().top;
    var p1 = new mxLib.Pixel(left, top); //像素坐标
    var p2 = new mxLib.Pixel(left + width, top + height);
    var pt1 = map.pixelToPoint(p1);
    var pt2 = map.pixelToPoint(p2);
    var rect = new mxLib.Bounds(pt1, pt2); //地理坐标 
    var points = [];
    points.push(rect.getLeftTop());
    points.push(rect.getRightTop());
    points.push(rect.getRightBottom());
    points.push(rect.getLeftBottom());
    this.polygon.setPath(points);
    this.rect = rect;
}
//定义矩形覆盖物----------------------------------------------

var rectTrajectoryDefCmd;
//查找所有需要进行轨迹绘制的覆盖物，并用矩形标注
function DoTrajectoryDef() {
    OverlayRectArr.length = 0;
    //将满足条件的对象添加到轨迹绘制列表中
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker) {
            if (allOverlay[i].getBindTypeName() == "0") {
                var tempOverlayRect = new OverlayRect(map, allOverlay[i]);
                tempOverlayRect.removeRectOverlay();
                tempOverlayRect.createRectOverlay();
                OverlayRectArr.push(tempOverlayRect);
            }
        }
    }

    map.addEventListener("zoomend", function () {
        for (var i = 0; i < OverlayRectArr.length; i++) {
            OverlayRectArr[i].draw();
        }
    });
    //启动图形绘制命令
    rectTrajectoryDefCmd = new mxLib.TrajectoryDef();
    map.startCommand(rectTrajectoryDefCmd);
}
//结束连线命令
function EndTrajectoryDef() {
    isByButton = true;
    //结束命令
    rectTrajectoryDefCmd.end();
}
//根据位置获取传感器
function getPointNameByPosition(x, y) {
    for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
        if ((x == pointRichMarkerMapArr[i].pointPos.x) && (y == pointsaveArr[i].pointPos.y)) {
            return pointsaveArr[i].pointName;
        }
    }
    return null;
}
