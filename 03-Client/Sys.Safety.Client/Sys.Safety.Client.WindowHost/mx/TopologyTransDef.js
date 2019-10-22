/*
* 拓扑图绘制
*/

//console.log("TbTranDef.js");

//抽象方法，定义拓扑图绘制命令
function LoadTbTranDef() {
    /* 
    * @exports TbTranDef as mxLib.Command
    */
    var TbTranDef =
    /*   
    * 矩形缩放命令类
    * @param  {String} name 命令名称 
    * @param  {Command} parentCmd 调用命令对象 
    * @param {Json Object} opts 命令选项
    * @constructor
    * @see mxLib.Map#selEntity
    */
    mxLib.TbTranDef = function TbTranDef(name, parentCmd, opts) {
        mxLib.Command.prototype.constructor.call(this, name, parentCmd, opts);
        this.TranType = opts.TranType;
    }

    //继承自Command类
    mxLib.Lang.inherits(TbTranDef, mxLib.Command);

    /*
    * 抽象方法，开始命令
    * @param {Map} map 地图对象
    * @returns {None} 
    */
    //    TbTranDef.prototype.start = function (map) {
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
    //    TbTranDef.prototype.leftMousedown = function (e, data) {
    //            
    //    }



    /*
    * 抽象方法，左键抬起事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TbTranDef.prototype.leftMouseup = function (e, data) {

    }

    /*
    * 抽象方法，右键抬起事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TbTranDef.prototype.rightMouseup = function (e, data) {


    }

    /*
    * 抽象方法，鼠标移动事件
    * @param {Object} e 事件
    * @param {Object} data 事件参数
    * @returns {None} 
    */
    TbTranDef.prototype.mousemove = function (e, data) {

    }

    /*
    * 抽象方法，开始命令
    * @param {Map} map 地图对象
    * @returns {None} 
    */
    TbTranDef.prototype.start = function (map) {
        var _this = this;
        mxLib.Command.prototype.start.call(this, map);
        this.points = [];
        map.setCursor("crosshair");
        var selOverlays = [];
        for (var i = 0; i < this.overlays.length; i++) {
            this.overlays[i].disableDragging();
            //            this.overlays[i].addEventListener("mouseover", function (e) {
            //                var ol = e.currentTarget;
            //                ol.snap().select("g").attr("fill", "rgb(0, 255, 0)");
            //                _this.currentOverlay = e.currentTarget;
            //            });

            //            this.overlays[i].addEventListener("mouseout", function (e) {
            //                var ol = e.currentTarget;
            //                ol.snap().select("g").attr("fill", "rgb(255, 255, 255)");
            //                _this.currentOverlay = null;
            //            });

            //单击命令
            this.overlays[i].addEventListener("click", function (e) {
                for (var i = 0; i < selOverlays.length; i++) {
                    if (selOverlays[i] == e.currentTarget) {
                        mxLib.Util.toastInfo("已经选择了此节点！", { delay: 3000 });
                        return;
                    }
                }
                selOverlays.push(e.currentTarget);
                if (selOverlays.length == 2) {
                    _this.startPoint = selOverlays[0].getBounds().getCenter();
                    _this.startPtype = selOverlays[0].getBindUnitName();
                    _this.endPoint = selOverlays[1].getBounds().getCenter();
                    _this.endPtype = selOverlays[1].getBindUnitName();
                    //                    if (!(_this.startPtype == "交换机" && _this.endPtype == "分站"
                    //                 || _this.startPtype == "分站" && _this.endPtype == "传感器")) {
                    //                        mxLib.Util.toastInfo("建立节点关系错误，请先选择父节点，再选择子节点！", { delay: 3000 });
                    //                        selOverlays.splice(1, 1);
                    //                        return;
                    //                    }
                    if (_this.TranType == "0") {
                        _this.centerPoint1 = new mxLib.Point(_this.startPoint.x, ((_this.startPoint.y + _this.endPoint.y) / 2));
                        _this.centerPoint2 = new mxLib.Point(_this.endPoint.x, ((_this.startPoint.y + _this.endPoint.y) / 2));
                    }
                    else if (_this.TranType == "1") {
                        _this.centerPoint1 = new mxLib.Point(_this.startPoint.x, _this.endPoint.y);
                        _this.centerPoint2 = new mxLib.Point(_this.startPoint.x, _this.endPoint.y);
                    }
                    _this.points = new Array();
                    _this.points.push(_this.startPoint);
                    _this.points.push(_this.centerPoint1);
                    _this.points.push(_this.centerPoint2);
                    _this.points.push(_this.endPoint);
                    _this.polyline = new mxLib.Polyline(_this.points, { strokeColor: "DarkGray", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: false });
                    _this.map.addOverlay(_this.polyline);

                    //判断当前路线之前是否有重复路径，如有重复路径，将之前路径删除
                    for (var i = 0; i < allPointTbObjList.length; i++) {
                        if (allPointTbObjList[i].points[0].equals(_this.startPoint) && allPointTbObjList[i].points[allPointTbObjList[i].points.length - 1].equals(_this.endPoint)) {
                            map.removeOverlay(allPointTbObjList[i].polyline);
                            allPointTbObjList.splice(i, 1);
                        }
                    }

                    var TbTranObj = {}; //定义拓扑图对象
                    //添加当前线路类型
                    TbTranObj.tranType = _this.TranType;
                    //添加到线路数组
                    TbTranObj.polyline = _this.polyline;
                    //添加到路线点数组
                    TbTranObj.points = _this.points;
                    //将路线的起始点编号、结束点编号添加到路线编号数组                    
                    TbTranObj.sPointName = selOverlays[0].getJcdName();
                    TbTranObj.ePointName = selOverlays[1].getJcdName();
                    //将路线起始点、结束点类型存放到对象中
                    //TbTranObj.sPointType = selOverlays[0].getBindUnitName();
                    //TbTranObj.ePointType = selOverlays[1].getBindUnitName();
                    //将Svg的起始点坐标存到对象数组                   
                    TbTranObj.sPointPos = selOverlays[0].getBounds().getLeftTop();
                    TbTranObj.ePointPos = selOverlays[1].getBounds().getLeftTop();

                    allPointTbObjList.push(TbTranObj);

                    //清空变量  
                    selOverlays.length = 0;
                }
            });
        }
    }

    /*
    * 抽象方法，结束命令
    * @param {Boolean} bCancel 是否取消
    * @returns {None} 
    */
    TbTranDef.prototype.end = function (bCancel) {
        //        if (this.currentOverlay) {
        //            this.currentOverlay.snap().select("g").attr("fill", "rgb(255, 255, 255)");
        //        }
        for (var i = 0; i < this.overlays.length; i++) {
            this.overlays[i].enableDragging();
        }
        mxLib.Command.prototype.end.call(this, bCancel);
    }
}
LoadTbTranDef();

/*
* @var rectTbTranDefCmd：拓扑图绘图命令对象
*/
var rectTbTranDefCmd;

/*
* 抽象方法，启动绘图命令
* @param type 连线类型（0:S型线，1：L型线）
* @returns {None} 
*/
/**
* 启动绘图命令
* @class DoTopologyTransDef 
* @param {type} 连线类型 0代表为S型连线，1代表为L型连线
* @returns 返回值：无
* @example 参考示例：
* DoTopologyTransDef(0);
*/
function DoTopologyTransDef(type) {
    var saveEventOverlays = [];
    for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
        //只对静态用动态图元进行拓扑连线，文字不进行连线
        if (TBpointVectorLayerArr[i].bindUnitName.indexOf('&字') < 0) {
            saveEventOverlays.push(TBpointVectorLayerArr[i].overlay);
        }
    }
    //启动图形绘制命令
    rectTbTranDefCmd = new mxLib.TbTranDef("", null, { saveEventOverlays: saveEventOverlays, TranType: type });
    rectTbTranDefCmd.overlays = saveEventOverlays;
    map.startCommand(rectTbTranDefCmd);
}

/*
* 抽象方法，结束绘图命令
* @param 无
* @returns {None} 
*/
/**
* 结束绘图命令
* @class EndTopologyTransDef 
* @param 无
* @returns 无
* @example 参考示例：
* EndTopologyTransDef();
*/
function EndTopologyTransDef() {
    if (rectTbTranDefCmd) {
        rectTbTranDefCmd.end();
    }
}
/*
* 抽象方法，根据测点定义规则，自动生成拓扑图
* @param TopologyDefStr 测点定义信息串 格式:192.168.1.1|交换机|001000,001000|分站|
* @returns {None} 
*/
/**
* 根据测点定义规则，自动生成拓扑图
* @class AutoDragTopologyTrans 
* @param {TopologyDefStr} 测点定义信息串(设备编号/IP|安装位置|设备类型|绑定类型|子设备编号) 格式:192.168.1.1|安装位置1|交换机|交换机|003000,0030000|智能分站3|KJ306通用分站|分站|003A100#003D080
* @returns 无
* @example 参考示例：
* AutoDragTopologyTrans(192.168.1.1|安装位置1|交换机|交换机|003000,0030000|智能分站3|KJ306通用分站|分站|003A100#003D080);
*/
function AutoDragTopologyTrans(TopologyDefStr) {
    var TopologyDefObjArr = [];
    var startCentrPoint; //接口中心点坐标
    var startCentrName = ""; //接口名称
    var TopologyLength = 0; //所有设备的总长度
    var TopologyCount = 0; //顶层设备总个数
    var AutoY = 120; //自动高度设置
    var allOverlay = map.getOverlays();
    //将定义参数转换成对象
    var tempstr = TopologyDefStr.split(',');
    for (var i = 0; i < tempstr.length; i++) {
        var tempobjstr = tempstr[i].split('|');
        var TopologyDefObj = {};
        TopologyDefObj.point = tempobjstr[0];
        TopologyDefObj.wz = tempobjstr[1];
        TopologyDefObj.devname = tempobjstr[2];
        TopologyDefObj.bindUnitName = tempobjstr[3];
        TopologyDefObj.ChildPoints = tempobjstr[4];
        TopologyDefObj.ChindLen = 0;
        TopologyDefObj.ChindCount = 0;
        TopologyDefObjArr.push(TopologyDefObj);
    }
    //得到自动绘图的中心点,并先删除现在图形下的所有设备信息
    for (var i = allOverlay.length - 1; i >= 0; i--) {
        if (allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getBindUnitName() == "数据接口&接口") {
                startCentrPoint = new mxLib.Point(allOverlay[i].getBounds().getCenter().x, allOverlay[i].getBounds().getCenter().y);
                startCentrName = allOverlay[i].getJcdName();
            }
            else if (allOverlay[i].getBindUnitName() == "交换机" ||
            allOverlay[i].getBindUnitName() == "分站" ||
            allOverlay[i].getBindUnitName() == "传感器") {//删除设备及连线
                TBpointVectorLayerArr.splice(i, 1);
                //删除当前点的连接线及对象
                var tempPostion = allOverlay[i].getBounds().getCenter();
                var tempPoint = allOverlay[i].getJcdName();
                for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                    if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                        map.removeOverlay(allPointTbObjList[j].polyline);
                        allPointTbObjList.splice(j, 1);
                    }
                }
                map.removeOverlay(allOverlay[i]);
            }
        }
    }
    //判断接口是否存在
    if (startCentrName == "") {
        mxLib.Util.toastInfo("提示：未找到数据接口，不能进行拓扑生成！", { align: "center" });
        //mxLib.Util.toastSuccess("提示：未找到数据接口，不能进行拓扑生成！", { align: "center" });
        return;
    }
    //得到所有交换机的个数及总像素长度,包括子设备个数及长度
    for (var i = 0; i < TopologyDefObjArr.length; i++) {
        if (TopologyDefObjArr[i].bindUnitName == "交换机") {

            if (TopologyDefObjArr[i].ChildPoints.length > 0) {
                TopologyDefObjArr[i].ChindLen = TopologyDefObjArr[i].ChindLen + getChindLen(TopologyDefObjArr[i], TopologyDefObjArr); //递归增加当前设备的所有子设备
                TopologyDefObjArr[i].ChindCount = TopologyDefObjArr[i].ChildPoints.split('#').length;
            }
            else {
                TopologyDefObjArr[i].ChindLen = TopologyDefObjArr[i].ChindLen + 120;
                TopologyDefObjArr[i].ChindCount++;
            }

            TopologyLength = TopologyLength + TopologyDefObjArr[i].ChindLen;
            TopologyCount++;
        }
    }
    //动态添加设备，并连线 
    var startPointxNow;
    startPointxNow = (startCentrPoint.x - (TopologyLength / 2));
    for (var i = 0; i < TopologyDefObjArr.length; i++) {
        if (TopologyDefObjArr[i].bindUnitName == "交换机") {
            var thisstart_x, thisstart_y;
            thisstart_x = (startPointxNow + (TopologyDefObjArr[i].ChindLen) / 2); //交换机中心点位置x
            thisstart_y = (startCentrPoint.y - 230); //交换机中心点位置y
            //添加设备
            AddTBPointToMap(TopologyDefObjArr[i].point, TopologyDefObjArr[i].wz, TopologyDefObjArr[i].devname, TopologyDefObjArr[i].bindUnitName,
             "1", "0", "0", thisstart_x,
             thisstart_y, 0, 0, "1", "","0");
            //添加连线
            var tranType = "0";
            var P_PointCode = startCentrName;
            var S_PointCode = TopologyDefObjArr[i].point;
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

            //递归添加所有子设备
            if (TopologyDefObjArr[i].ChildPoints.length > 0) {
                var point = new mxLib.Point(TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().x,
                TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().y);
                AutoAddChind(TopologyDefObjArr[i], TopologyDefObjArr, point, startPointxNow);
            }
            startPointxNow = startPointxNow + TopologyDefObjArr[i].ChindLen;
        }
    }
    //生成完成,调用给所有图元加ToolTips   
    addToolTips();
}
/*
* 抽象方法，得到每个顶层设备的子设备所占的长度（递归）
* @param TopologyDefObj 当前设备对象
* @param TopologyDefObjArr 所有设备对象数组
* @returns {int} TopologyDefObj.ChindLen  当前设备所占的长度
*/
function getChindLen(TopologyDefObj, TopologyDefObjArr) {
    var tempstr = TopologyDefObj.ChildPoints.split('#');
    for (var i = 0; i < tempstr.length; i++) {
        for (var j = 0; j < TopologyDefObjArr.length; j++) {
            if (TopologyDefObjArr[j].point == tempstr[i]) {
                if (TopologyDefObjArr[j].ChildPoints.length > 0) {
                    TopologyDefObj.ChindLen = TopologyDefObj.ChindLen + getChindLen(TopologyDefObjArr[j], TopologyDefObjArr);
                    TopologyDefObj.ChindCount = TopologyDefObjArr[j].ChildPoints.split('#').length;
                }
                else {
                    TopologyDefObj.ChindLen = TopologyDefObj.ChindLen + 120;
                    TopologyDefObj.ChindCount++;
                    TopologyDefObjArr[j].ChindLen = TopologyDefObjArr[j].ChindLen + 120;
                    TopologyDefObjArr[j].ChindCount++;
                }
            }
        }
    }
    return TopologyDefObj.ChindLen;
}
/*
* 抽象方法，递归添加子设备
* @param TopologyDefObj 当前设备对象
* @param TopologyDefObjArr 所有设备对象数组
* @param parentPoint 父节点的坐标
* @param startPointxNow 当前位置开始绘制的X坐标
* @returns {None} 
*/
function AutoAddChind(TopologyDefObj, TopologyDefObjArr, parentPoint, startPointxNow) {
    var tempstr = TopologyDefObj.ChildPoints.split('#');
    for (var i = 0; i < tempstr.length; i++) {
        for (var j = 0; j < TopologyDefObjArr.length; j++) {
            if (TopologyDefObjArr[j].point == tempstr[i]) {
                var thisstart_x = (startPointxNow + (TopologyDefObjArr[j].ChindLen) / 2); //子设备中心点位置x
                var thisstart_y = (parentPoint.y - 150); //子设备中心点位置y
                //添加设备
                AddTBPointToMap(TopologyDefObjArr[j].point, TopologyDefObjArr[j].wz, TopologyDefObjArr[j].devname, TopologyDefObjArr[j].bindUnitName,
                 "1", "0", "0", thisstart_x,
                  thisstart_y, 0, 0, "1","","0");
                //添加连线
                var tranType = "0";
                var P_PointCode = TopologyDefObj.point;
                var S_PointCode = TopologyDefObjArr[j].point;
                var P_PointPos = new mxLib.Point(parentPoint.x, parentPoint.y);
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

                if (TopologyDefObjArr[j].ChildPoints.length > 0) {
                    var point = new mxLib.Point(TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().x,
                TBpointVectorLayerArr[TBpointVectorLayerArr.length - 1].overlay.getBounds().getCenter().y);
                    AutoAddChind(TopologyDefObjArr[j], TopologyDefObjArr, point, startPointxNow);
                }
                startPointxNow = startPointxNow + TopologyDefObjArr[j].ChindLen;
            }
        }
    }
}



