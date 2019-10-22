/*
* 测点加载及编辑  20150408。
*/
//console.log("pointdef.js");

//当前正在编辑的测点
var PointRichMarkEditNow;

//悬浮图元对象列表
var pointRichMarkerMapArr = [];


//设置当前正在编辑的点
function setPointEditNow(RichMark) {
    PointRichMarkEditNow = RichMark;
}
//获取当前正在编辑的点
function getPointEditNow() {
    return PointRichMarkEditNow;
}
//编辑设备信息
function EditPoint(overlay) {
    if (IsGraphicEdit) { //如果当前处于编辑状态
        setPointEditNow(overlay);
        if (overlay.getBindTypeName() == "1" || overlay.getBindTypeName() == "2") {
            /**
            * 回调_编辑设备信息(用于双击测点时与界面进行交互弹出测点编辑界面)
            * @class callOutCommand_PointEdit 
            * @param 测点属性列表(测点号|测点绑定图元名称|图元类型|缩放等级|绑定的动画状态|图元宽度(可选)|图元高度(可选)) 如:001A01|实时显示|0|0|0
            * @returns 无
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "PointEdit":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "PointEdit") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "PointEdit") {
            *          //这里进行实现
            *      }
            *   }
            */
            mxLib.View.callOutCommand("PointEdit", overlay.getJcdName() + "|" + overlay.getBindUnitName()
        + "|" + overlay.getBindTypeName() + "|" + overlay.getZoomLevel() + "|" + overlay.getanimationState()
        + "|" + overlay.getPointWidth() + "|" + overlay.getPointHeight() + "|" + overlay.getTurnToPage() + "|" + overlay.getPointId() + "|" + overlay.getTransformValue());
        }
        else {
            mxLib.View.callOutCommand("PointEdit", overlay.getJcdName() + "|" + overlay.getBindUnitName()
        + "|" + overlay.getBindTypeName() + "|" + overlay.getZoomLevel() + "|" + overlay.getanimationState()
        + "|" + overlay.getTurnToPage() + "|" + overlay.getPointId() + "|" + overlay.getTransformValue());
        }
    }
    else {
        //如果是运行状态，则返回双击事件到前端
        /**
                * 回调_测点运行时，单击事件
                * @class callOutCommand_PointDblClick
                * @param 测点号 
                * @returns 无
                * @example 参考示例：
                *  c#：
                *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
                *        {
                *            string  Param = e.p_sParam;//参数
                *            switch (e.p_sCmd)//命令
                *            {
                *                case "PointDblClick":
                *                //这里进行实现
                *                break;
                *            }
                *        }
                * IE：
                * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
                *      if (cmd == "PointDblClick") {
                *          //这里进行实现
                *      }
                *   }
                * google:
                *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
                *      if (cmd == "PointDblClick") {
                *          //这里进行实现
                *      }
                *   }
                */
        mxLib.View.callOutCommand("PointDblClick", overlay.getJcdName() + "|" + overlay.getBindUnitName());
    }
}
//删除设备信息
function DelPoint(overlay) {
    if (IsGraphicEdit) { //如果当前处于编辑状态        
        //调用C#先删除数据库中的测点信息
        /**
        * 回调_调用C#先删除数据库中的测点信息(用于删除测点时，调用界面操作数据库并删除数据库中的测点)
        * @class callOutCommand_PointDel 
        * @param 测点号 
        * @returns 无
        * @example 参考示例：
        *  c#：
        *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
        *        {
        *            string  Param = e.p_sParam;//参数
        *            switch (e.p_sCmd)//命令
        *            {
        *                case "PointDel":
        *                //这里进行实现
        *                break;
        *            }
        *        }
        * IE：
        * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
        *      if (cmd == "PointDel") {
        *          //这里进行实现
        *      }
        *   }
        * google:
        *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
        *      if (cmd == "PointDel") {
        *          //这里进行实现
        *      }
        *   }
        */
        mxLib.View.callOutCommand("PointDel", overlay.getJcdName());
    }
}

//编辑当前覆盖物信息
/**
* 编辑当前测点信息
* @class EditPointToRichMark 
* @param {pointName} 测点号 如:001A01
* @param {pointWz} 安装位置 
* @param {pointDevName} 设备类型名称 
* @param {zoomLevel} 缩放等级 说明：只针对SVG图元有效
* @param {animationState} 绑定的动画状态 只针对开关量图元有效，状态为：0，1，2态
* @param {width} 图元宽度 说明：只针对SVG图元有效
* @param {height} 图元高度 说明：只针对SVG图元有效  
* @returns 返回值：无
* @example 参考示例：
* EditPointToRichMark('002D010','测试位置','设备开停','0','2','0','0');
*/
function EditPointToRichMark(pointName, pointWz, pointDevName, zoomLevel, animationState, width, height, TurnToPage, PointId, transformDeg) {
    var OldPointName = PointRichMarkEditNow.getJcdName();
    if (PointRichMarkEditNow.getJcdName() == pointName) {
        console.log(pointName + "测点名称未发生改变");
    }
    else {
        //判断是否已添加此点，如果已添加，则进行重命名
        var allOverlay = map.getOverlays();
        for (var i = 0; i < allOverlay.length; i++) {
            if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
                if (allOverlay[i].getJcdName()) {
                    if (allOverlay[i].getJcdName() == pointName) {
                        console.log(pointName + "已存在,进行重命名");
                        //点已存在，进行重新命名
                        var index = 1;
                        for (var j = 0; j < allOverlay.length; j++) {
                            if (allOverlay[j] instanceof mxLib.RichMarker || allOverlay[j] instanceof mxLib.VectorLayer) {
                                if (allOverlay[j].getJcdName()) {
                                    if (allOverlay[j].getJcdName().indexOf(pointName + "￣" + index.toString()) >= 0) {
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
                        pointName = pointName + "￣" + index.toString();
                        console.log("点已存在，重命名为：" + pointName);
                        break;
                    }
                }
                if (allOverlay[i].getPointId() == PointId) {
                    if (allOverlay[i].getBindTypeName() == "2") {
                        allOverlay[i].options.minZoom = parseInt(zoomLevel.split('$')[0]);
                        allOverlay[i].options.maxZoom = parseInt(zoomLevel.split('$')[1]);
                    }
                    else if (allOverlay[i].getBindTypeName() == "3") {
                        allOverlay[i]._opts.minZoom = parseInt(zoomLevel.split('$')[0]);
                        allOverlay[i]._opts.maxZoom = parseInt(zoomLevel.split('$')[1]);
                    }
                }
            }
        }
    }
    //更新当前编辑对象的值
    PointRichMarkEditNow.setJcdName(pointName);
    PointRichMarkEditNow.setJcdWz(pointWz);
    PointRichMarkEditNow.setJcdDevName(pointDevName);
    PointRichMarkEditNow.setZoomLevel(zoomLevel);
    PointRichMarkEditNow.setanimationState(animationState);
    PointRichMarkEditNow.setTurnToPage(TurnToPage);
    //设置旋转角度  20171226
    PointRichMarkEditNow.setTransformValue(transformDeg);
    PointRichMarkEditNow.setTransform(transformDeg);

    if (PointRichMarkEditNow.getBindTypeName() == "1" || PointRichMarkEditNow.getBindTypeName() == "2") {
        PointRichMarkEditNow.setPointWidth(width);
        PointRichMarkEditNow.setPointHeight(height);
    }


    //实时显示图元和拓扑图元设置点号文本
    if (PointRichMarkEditNow.getBindTypeName() == "0" || PointRichMarkEditNow.getBindTypeName() == "1") {
        PointRichMarkEditNow.setText(pointName);
    }
    //调用给所有图元加ToolTips
    addToolTips();

    //更新对应的内存对象
    switch (PointRichMarkEditNow.getBindTypeName()) {
        case "0": //实时显示图元
        case "3": //gif动画图元
            for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
                if (OldPointName == pointRichMarkerMapArr[i].pointName) {
                    pointRichMarkerMapArr[i].pointName = pointName;
                    pointRichMarkerMapArr[i].pointWz = pointWz;
                    pointRichMarkerMapArr[i].pointDevName = pointDevName;
                    pointRichMarkerMapArr[i].overlay = PointRichMarkEditNow;
                }
            }
            //更新连线内存
            for (var j = allPointTranMapArr.length - 1; j >= 0; j--) {
                if (allPointTranMapArr[j].sPointName == OldPointName) {
                    allPointTranMapArr[j].sPointName = pointName;
                }
                if (allPointTranMapArr[j].ePointName == OldPointName) {
                    allPointTranMapArr[j].ePointName = pointName;
                }
            }
            break;
        case "1": //拓扑图图元       
            var nowPoint;
            for (var i = 0; i < TBpointVectorLayerArr.length; i++) {
                if (OldPointName == TBpointVectorLayerArr[i].pointName) {
                    //更新bounds
                    var pointPos = PointRichMarkEditNow.getBounds().getLeftTop();
                    var left = PointRichMarkEditNow.getBounds().getLeftTop().x;
                    var top = PointRichMarkEditNow.getBounds().getLeftTop().y;
                    var vlBounds = new mxLib.Bounds(left, top, left + parseInt(width), top - parseInt(height));
                    PointRichMarkEditNow.setBounds(vlBounds);

                    nowPoint = PointRichMarkEditNow.getBounds().getCenter();

                    TBpointVectorLayerArr[i].pointName = pointName;
                    TBpointVectorLayerArr[i].pointWz = PointRichMarkEditNow.getJcdWz();
                    TBpointVectorLayerArr[i].pointDevName = PointRichMarkEditNow.getJcdDevName();
                    TBpointVectorLayerArr[i].overlay = PointRichMarkEditNow;

                    TBpointVectorLayerArr[i].pointPos = PointRichMarkEditNow.getBounds().getLeftTop();
                    TBpointVectorLayerArr[i].width = width;
                    TBpointVectorLayerArr[i].height = height;
                }
            }
            //更新连线内存
            for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                if (allPointTbObjList[j].sPointName == OldPointName) {
                    allPointTbObjList[j].sPointName = pointName;

                    var sPoint = new mxLib.Point(nowPoint.x, nowPoint.y);
                    var ePoint = new mxLib.Point(allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].x,
                         allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].y);
                    var cPoint1;
                    var cPoint2;
                    if (allPointTbObjList[j].tranType == "0") {
                        cPoint1 = new mxLib.Point(sPoint.x, ((sPoint.y + ePoint.y) / 2));
                        cPoint2 = new mxLib.Point(ePoint.x, ((sPoint.y + ePoint.y) / 2));
                    }
                    else if (allPointTbObjList[j].tranType == "1") {
                        cPoint1 = new mxLib.Point(sPoint.x, ePoint.y);
                        cPoint2 = new mxLib.Point(sPoint.x, ePoint.y);
                    }
                    allPointTbObjList[j].points[0] = sPoint;
                    allPointTbObjList[j].points[1] = cPoint1;
                    allPointTbObjList[j].points[2] = cPoint2;
                    allPointTbObjList[j].points[3] = ePoint;
                }
                if (allPointTbObjList[j].ePointName == OldPointName) {
                    allPointTbObjList[j].ePointName = pointName;

                    var sPoint = new mxLib.Point(allPointTbObjList[j].points[0].x, allPointTbObjList[j].points[0].y);
                    var ePoint = new mxLib.Point(nowPoint.x, nowPoint.y);
                    var cPoint1;
                    var cPoint2;
                    if (allPointTbObjList[j].tranType == "0") {
                        cPoint1 = new mxLib.Point(sPoint.x, ((sPoint.y + ePoint.y) / 2));
                        cPoint2 = new mxLib.Point(ePoint.x, ((sPoint.y + ePoint.y) / 2));
                    }
                    else if (allPointTbObjList[j].tranType == "1") {
                        cPoint1 = new mxLib.Point(sPoint.x, ePoint.y);
                        cPoint2 = new mxLib.Point(sPoint.x, ePoint.y);
                    }
                    allPointTbObjList[j].points[0] = sPoint;
                    allPointTbObjList[j].points[1] = cPoint1;
                    allPointTbObjList[j].points[2] = cPoint2;
                    allPointTbObjList[j].points[3] = ePoint;
                }
                allPointTbObjList[j].polyline.setPath(allPointTbObjList[j].points);
            }
            break;
        case "2": //SVG图元
            for (var i = 0; i < SVGpointVectorLayerArr.length; i++) {
                if (OldPointName == SVGpointVectorLayerArr[i].pointName) {
                    //更新bounds
                    var pointPos = PointRichMarkEditNow.getBounds().getLeftTop();
                    var left = PointRichMarkEditNow.getBounds().getLeftTop().x;
                    var top = PointRichMarkEditNow.getBounds().getLeftTop().y;
                    var vlBounds = new mxLib.Bounds(left, top, left + parseInt(width), top - parseInt(height));
                    PointRichMarkEditNow.setBounds(vlBounds);


                    SVGpointVectorLayerArr[i].pointName = pointName;
                    SVGpointVectorLayerArr[i].pointWz = PointRichMarkEditNow.getJcdWz();
                    SVGpointVectorLayerArr[i].pointDevName = PointRichMarkEditNow.getJcdDevName();
                    SVGpointVectorLayerArr[i].overlay = PointRichMarkEditNow;

                    SVGpointVectorLayerArr[i].pointPos = PointRichMarkEditNow.getBounds().getLeftTop();
                    SVGpointVectorLayerArr[i].width = width;
                    SVGpointVectorLayerArr[i].height = height;
                }
            }
            break;
    }
}
//根据当前监测点名称，删除当前覆盖物信息
/**
* 删除当前测点
* @class DelPointToRichMark 
* @param {pointName} 测点号 如:001A01
* @returns 返回值：无
* @example 参考示例：
* DelPointToRichMark('001A01') ;
*/
function DelPointToRichMark(pointName) {
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getJcdName()) {
                if (pointName == allOverlay[i].getJcdName()) {
                    //移除内存对象
                    switch (allOverlay[i].getBindTypeName()) {
                        case "0": //实时显示图元
                        case "3": //gif动画图元
                            for (var j = 0; j < pointRichMarkerMapArr.length; j++) {
                                if (pointRichMarkerMapArr[j].pointName == pointName) {
                                    pointRichMarkerMapArr.splice(j, 1);
                                }
                            }
                            //删除当前点的连接线及对象
                            var tempPostion = allOverlay[i].getPosition();
                            for (var j = allPointTranMapArr.length - 1; j >= 0; j--) {
                                if (allPointTranMapArr[j].sPointPos.equals(tempPostion) || allPointTranMapArr[j].ePointPos.equals(tempPostion)) {
                                    map.removeOverlay(allPointTranMapArr[j].polyline);
                                    allPointTranMapArr.splice(j, 1);
                                }
                            }
                            break;
                        case "1": //拓扑图图元
                            //测点覆盖物列表
                            for (var j = 0; j < TBpointVectorLayerArr.length; j++) {
                                if (TBpointVectorLayerArr[j].pointName == pointName) {
                                    TBpointVectorLayerArr.splice(j, 1);
                                }
                            }
                            //删除当前点的连接线及对象
                            var tempPostion = allOverlay[i].getBounds().getCenter();
                            var tempPoint = allOverlay[i].getJcdName();
                            for (var j = allPointTbObjList.length - 1; j >= 0; j--) {
                                if (allPointTbObjList[j].points[0].equals(tempPostion) || allPointTbObjList[j].points[allPointTbObjList[j].points.length - 1].equals(tempPostion)) {
                                    map.removeOverlay(allPointTbObjList[j].polyline);
                                    allPointTbObjList.splice(j, 1);
                                }
                            }
                            break;
                        case "2": //SVG图元
                            for (var j = 0; j < SVGpointVectorLayerArr.length; j++) {
                                if (SVGpointVectorLayerArr[j].pointName == pointName) {
                                    SVGpointVectorLayerArr.splice(j, 1);
                                }
                            }
                            break;
                    }
                    map.removeOverlay(allOverlay[i]);
                    break;
                }
            }
        }
    }
}
//删除连接线
function DelTrajectory(overlay) {
    if (IsGraphicEdit) { //如果当前处于编辑状态       
        var tempPostion = overlay.getPosition();
        for (var j = allPointTranMapArr.length - 1; j >= 0; j--) {
            if (allPointTranMapArr[j].sPointPos.equals(tempPostion) || allPointTranMapArr[j].ePointPos.equals(tempPostion)) {
                map.removeOverlay(allPointTranMapArr[j].polyline);
                allPointTranMapArr.splice(j, 1);
            }
        }
    }
}
//myRichMarker.setImage(scriptBaseDir + "mxImg/back2.png")


//测点右键信息-----------------------------------
var richpointRightmenu1 = new mxLib.MenuItem("删除", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    if (confirm("确定要删除吗？")) {
        var overlay = e.view.richpointRconMenu.targetObject;
        DelPoint(overlay);
    }
    //DelPointToRichMark(overlay.getJcdName());
});
var richpointRightmenu2 = new mxLib.MenuItem("编辑", "", "", function (e) {
    var overlay = e.view.richpointRconMenu.targetObject;
    //        overlay.enableEditing();
    EditPoint(overlay);
});
var richpointRightmenu3 = new mxLib.MenuItem("删除连线", "", "", function (e) {
    if (confirm("确定要删除连接线吗？")) {
        var overlay = e.view.richpointRconMenu.targetObject;
        //        overlay.enableEditing();
        DelTrajectory(overlay);
    }
});
//var richpointRightmenu4 = new mxLib.MenuItem("波及", "", "", function (e) {
//    var overlay = e.view.contextMenu1.targetObject;
//    setLine(overlay.getJcdName());
//});
//var richpointRightmenu5 = new mxLib.MenuItem("清除波及", "", "", function (e) {
//    var overlay = e.view.contextMenu1.targetObject;
//    clearLine();
//});

//测点右键信息
var richpointRightmenu4 = new mxLib.MenuItem("详细信息查看", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "详细信息查看" + "|" + overlay.getJcdName());
});
var richpointRightmenu5 = new mxLib.MenuItem("运行记录查询", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "运行记录查询" + "|" + overlay.getJcdName());
});
var richpointRightmenu6 = new mxLib.MenuItem("模拟量实时曲线", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模拟量实时曲线" + "|" + overlay.getJcdName());
});
var richpointRightmenu7 = new mxLib.MenuItem("模拟量历史曲线（5分钟）", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模拟量历史曲线（5分钟）" + "|" + overlay.getJcdName());
});
var richpointRightmenu8 = new mxLib.MenuItem("模拟量历史曲线（密采）", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模拟量历史曲线（密采）" + "|" + overlay.getJcdName());
});
var richpointRightmenu9 = new mxLib.MenuItem("模拟量小时曲线", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模拟量小时曲线" + "|" + overlay.getJcdName());
});
var richpointRightmenu10 = new mxLib.MenuItem("开关量状态图显示", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "开关量状态图显示" + "|" + overlay.getJcdName());
});
var richpointRightmenu11 = new mxLib.MenuItem("开关量柱状图显示", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "开关量柱状图显示" + "|" + overlay.getJcdName());
});
var richpointRightmenu12 = new mxLib.MenuItem("模拟量密采记录查询", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模拟量密采记录查询" + "|" + overlay.getJcdName());
});
var richpointRightmenu13 = new mxLib.MenuItem("开关量状态变化查询", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "开关量状态变化查询" + "|" + overlay.getJcdName());
});

var richpointRightmenu15 = new mxLib.MenuItem("模开同屏曲线", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "模开同屏曲线" + "|" + overlay.getJcdName());
});

var richpointRightmenu16 = new mxLib.MenuItem("联动信息查看", ""//图标：scriptBaseDir + "mxImg/star.png"
, "", function (e) {
    var overlay = e.view.richpointRconMenuInRunState.targetObject;
    mxLib.View.callOutCommand("ShowDetailInRightMenu", "联动信息查看" + "|" + overlay.getJcdName());
});

var richpointRconMenu = new mxLib.ContextMenu();
richpointRconMenu.appendItem(richpointRightmenu1);
richpointRconMenu.appendItem(richpointRightmenu2);
richpointRconMenu.appendItem(richpointRightmenu3);
//richpointRconMenu.appendItem(richpointRightmenu4);
//richpointRconMenu.appendItem(richpointRightmenu5);

var richpointRconMenuInRunState = new mxLib.ContextMenu();
richpointRconMenuInRunState.appendItem(richpointRightmenu4);
richpointRconMenuInRunState.appendItem(richpointRightmenu5);
richpointRconMenuInRunState.appendItem(richpointRightmenu6);
richpointRconMenuInRunState.appendItem(richpointRightmenu7);
richpointRconMenuInRunState.appendItem(richpointRightmenu8);
richpointRconMenuInRunState.appendItem(richpointRightmenu9);
richpointRconMenuInRunState.appendItem(richpointRightmenu10);
richpointRconMenuInRunState.appendItem(richpointRightmenu11);
richpointRconMenuInRunState.appendItem(richpointRightmenu12);
richpointRconMenuInRunState.appendItem(richpointRightmenu13);
richpointRconMenuInRunState.appendItem(richpointRightmenu15);
richpointRconMenuInRunState.appendItem(richpointRightmenu16);

/**
* 右键菜单移除
* @class RemoveRightMenu 
* @param {menuName} 菜单名称 如:联动信息查看
* @returns 返回值：无
* @example 参考示例：
* RemoveRightMenu('联动信息查看');
*/
function RemoveRichPointRconMenu(menuName) {
    richpointRconMenuInRunState.hideItem(menuName);
}

//测点右键信息-----------------------------------
/**
* 添加测点到地图
* @class AddPixelPointToMap 
* @param {name} 测点号 如:001A01
* @param {wz} 安装位置 
* @param {devname} 设备类型名称 
* @param {GrapUnitName} 测点绑定图元名称 
* @param {type} 图元类型 说明：0：实时显示图元，1:拓扑图图元，2：SVG图元，3：gif动画图元,4:标注图元
* @param {zoomLevel} 缩放等级 说明：只针对SVG图元有效
* @param {animationState} 绑定的动画状态 只针对开关量图元有效，状态为：0，1，2态
* @param {x} 图元左边距 
* @param {y} 图元右边距  
* @param {width} 图元宽度 说明：只针对SVG图元有效
* @param {height} 图元高度 说明：只针对SVG图元有效  
* @returns 返回值：无
* @example 参考示例：
* AddPixelPointToMap('001A01','测试安装位置','低浓瓦斯（迎头）','实时显示','0','0','-1', '772',' 206','60','60','0');
*/
function AddPixelPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, x, y, width, height) {
    var pos = new mxLib.Pixel(x, y);
    var pointnew = map.pixelToPoint(pos); //将像素点转成地理坐标    
    switch (type) {
        case "0": //实时显示图元
        case "3": //gif动画图元
        case "4": //标注图元
            AddPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, pointnew.x, pointnew.y, width, height, "0", "", "0");
            break;
        case "1": //拓扑图图元
            AddTBPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, pointnew.x, pointnew.y, 0, 0, "1", "", "0");
            break;
        case "2": //SVG图元
            AddSVGPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, pointnew.x, pointnew.y, 0, 0, "0", "", "0");
            break;
    }
    //调用给所有图元加ToolTips
    addToolTips();
}
//添加点到地图
function AddPointToMap(name, wz, devname, GrapUnitName, type, zoomLevel, animationState, x, y, width, height, graphType, TurnToPage, transformDeg) {

    //mxLib.Util.toastInfo($('#PointName').val(), { delay: 8000 });
    var enableDragging = IsGraphicEdit;
    //判断是否已添加此点，如果已添加，则进行点重新命名
    var allOverlay = map.getOverlays();
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
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
        }
    }
    var point = new mxLib.Point(x, y);
    var html = "";
    var mxLibSize;
    if (type == "0") {
        if (GrapUnitName == "无背景") {
            html = "<div style=\"position: absolute; margin: 0pt; padding: 0pt; left: 0px; top: 0px; overflow: hidden;\">"
      + "<table cellpadding=\"0\" cellspacing=\"0\">"
               + "<tr>"
                   + "<td style=\"height: 26px;width:auto; "
                       + "background-repeat: repeat-x; font-size: 11pt; vertical-align: top;\">"
                       + "<div class=\"RichMarker_Text\" style=\"margin-top: 2px;float:left; display:inline;\">"
                            + "<nobr>" + name + "</nobr></div>"
                   + "</td>"
               + "</tr>"
           + "</table>"
  + "</div>";
        }
        else if (GrapUnitName == "实时显示2") {
            html = "<div style=\"position: absolute; margin: 0pt; padding: 0pt; left: 0px; top: 0px; overflow: hidden;\">"
+ "<table cellpadding=\"0\" cellspacing=\"0\">"
     + "<tr>"
         + "<td style=\"width: 4px; height: 26px;\">"
             + "<img src=\" mx/Text/" + GrapUnitName + "_Left.gif\" class=\"RichMarker_Left\" />"
         + "</td>"
         + "<td style=\"height: 26px;width:auto; background-image: url('mx/Text/" + GrapUnitName + "_Middle.gif');"
             + "background-repeat: repeat-x; font-size: 11pt; vertical-align: top;\" class=\"RichMarker_Middle\">"
             + "<div class=\"RichMarker_Text\" style=\"margin-top: 2px;float:left; display:inline;\">"
                  + "<nobr>" + name + "</nobr></div>"
         + "</td>"
         + "<td style=\"width: 4px; height: 26px;\">"
            + "<img src=\" mx/Text/" + GrapUnitName + "_Rright.gif\" class=\"RichMarker_Rright\" />"
         + "</td>"
     + "</tr>"
 + "</table>"
+ "</div>";
        }
        else {
            html = "<div style=\"position: absolute; margin: 0pt; padding: 0pt; left: 0px; top: 0px; overflow: hidden;\">"
        + "<table cellpadding=\"0\" cellspacing=\"0\">"
                 + "<tr>"
                     + "<td style=\"width: 24px; height: 26px;\">"
                         + "<img src=\" mx/Text/" + GrapUnitName + "_Left.gif\" class=\"RichMarker_Left\" />"
                     + "</td>"
                     + "<td style=\"height: 26px;width:auto; background-image: url('mx/Text/" + GrapUnitName + "_Middle.gif');"
                         + "background-repeat: repeat-x; font-size: 11pt; vertical-align: top;\" class=\"RichMarker_Middle\">"
                         + "<div class=\"RichMarker_Text\" style=\"margin-top: 2px;float:left; display:inline;\">"
                              + "<nobr>" + name + "</nobr></div>"
                     + "</td>"
                     + "<td style=\"width: 4px; height: 26px;\">"
                        + "<img src=\" mx/Text/" + GrapUnitName + "_Rright.gif\" class=\"RichMarker_Rright\" />"
                     + "</td>"
                 + "</tr>"
             + "</table>"
    + "</div>";
        }
        mxLibSize = new mxLib.Size(-18, -27);
    }
    else if (type == "3") {
        if (GrapUnitName.indexOf("gif") == 0) {
            html = "<div style=\"position: absolute; margin: 0pt; padding: 0pt; left: 0px; top: 0px; overflow: hidden;\">"
                   + "<img src=\" mx/Gif/" + GrapUnitName + "_.gif\" class=\"RichMarker_Image\" />"
                   + "</div>";
        }
        else {
            switch (GrapUnitName) {
                case "采煤机":
                    html = "<div style=\"background: transparent url(mx/Gif/采煤机底座.gif) no-repeat;display: block; position: relative; margin: 0px auto; width: 140px;"
                        + "height: 60px;\">"
                        + "<div class=\"AmniStop\" style=\"background: transparent url(mx/Gif/采煤机扇叶.gif) no-repeat;"
                            + "width: 30px; height: 30px; margin: 0; top: -5px; left: 10px; position: absolute;"
                    + "z-index: 70;\">"
                        + "</div>"
                        + "<div class=\"AmniStop\" style=\"background: transparent url(mx/Gif/采煤机扇叶.gif) no-repeat;"
                           + "width: 30px; height: 30px; margin: 0; top: 8px; right: 10px; position: absolute; z-index: 70;\">"
                        + "</div>"
                   + "</div>";
                    break;
                case "风扇":
                    html = "<div style=\"background: transparent url(mx/Gif/小风扇外框.gif) no-repeat; display: block;"
                        + "position: relative; margin: 0px auto; width: 60px; height: 60px;\">"
                        + "<div class=\"AmniStop\" style=\"background: transparent url(mx/Gif/小风扇扇叶.gif) no-repeat;"
                            + "width: 42px; height: 42px; margin: 0; top: 6px; left: 9px; position: absolute; z-index: 70;\">"
                        + "</div>"
                    + "</div>";
                    break;
                case "风扇2":
                    html = "<div style=\"background: transparent url(mx/Gif/小风扇外框1.gif) no-repeat; display: block;"
                        + "position: relative; margin: 0px auto; width: 32px; height: 32px;\">"
                        + "<div class=\"AmniStop\" style=\"background: transparent url(mx/Gif/小风扇扇叶1.gif) no-repeat;"
                            + "width: 24px; height: 24px; margin: 0; top: 4px; left: 4px; position: absolute; z-index: 70;\">"
                        + "</div>"
                    + "</div>";
                    break;
                case "风扇1":
                    html = "<div style=\"background: transparent url(mx/Gif/大风扇主机.gif) no-repeat; display: block;"
                        + "position: relative; margin: 0px auto; width: 60px; height: 60px;\">"
                        + "<div class=\"AmniStop\" style=\"background: transparent url(mx/Gif/大风扇扇叶.gif) no-repeat;"
                            + "width: 30px; height: 30px; margin: 0; top: 13px; left: 15px; position: absolute; z-index: 70;\">"
                        + "</div>"
                    + "</div>";
                    break;

                case "填充煤仓":
                    html = "<div style=\"height: 111px;width: 7px;padding: 0 13px;text-align: center; position: absolute; top:50%; left: 50%;transform: translate(-50%,-50%); background: url(mx/Gif/填充煤仓_.gif) no-repeat;\">"
                        + "<div style=\"height:100px;width:7px;position:absolute;bottom:0;left:13px;margin:0;background:#A0A0A0\">"
                        + "<div class=\"FillPix\" style=\"background: #F4FFC0;height:0; width: 7px;position: absolute;bottom: 0;margin:0\"></div>"
                        + "</div>";
                    + "</div>";
                    break;

            }
        }
        mxLibSize = new mxLib.Size(-(parseInt(width) / 2), -(parseInt(height) / 2));
    }
    else if (type == "4") {
        html = "<div style=\"position: absolute; margin: 0pt; padding: 0pt; left: 0px; top: 0px; overflow: hidden;\">"
        + "<div class=\"Marker\" style=\"background: transparent url(mx/Marker/" + GrapUnitName + ".png) no-repeat;"
        + "position: relative; margin: 0px auto; width: 32px; height: 44px;\">"
          + "</div>"
                + "</div>";
        mxLibSize = new mxLib.Size(-16, -44);
    }
    else {
        return;
    }

    var zoomLevelArr = new Array();
    zoomLevelArr = zoomLevel.split("$");
    var disMinZoomLevel = parseInt(zoomLevelArr[0]);
    var disMaxZoomLevel = parseInt(zoomLevelArr[1]);
    var myRichMarker = new mxLib.RichMarker(html, point, {
        "anchor": mxLibSize,
        "id": name,
        "enableDragging": enableDragging,
        zoomScale: 0,//设置图元在指定级别下随地图缩小
        minScale: true,
        maxScale: false,
        minZoom: disMinZoomLevel,
        maxZoom: disMaxZoomLevel
    });

    //设置点Id
    var guid = new GUID();
    myRichMarker.setPointId(guid.newGUID());
    //设置点名称
    myRichMarker.setJcdName(name);
    //设置点安装位置
    myRichMarker.setJcdWz(wz);
    //设置点设备类型
    myRichMarker.setJcdDevName(devname);
    //覆盖物移动开始
    myRichMarker.addEventListener("ondragstart", function (e) {
        this.oldPoint = this.getPosition();
        /**
        * 回调_设置图形是否保存(用于回调界面设置当前图形是否保存，表示图形已经修改)
        * @class callOutCommand_setMapEditSave 
        * @param bool类型 "true"/"false"
        * @returns 无
        * @example 参考示例：
        *  c#：
        *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
        *        {
        *            string  Param = e.p_sParam;//参数
        *            switch (e.p_sCmd)//命令
        *            {
        *                case "setMapEditSave":
        *                //这里进行实现
        *                break;
        *            }
        *        }
        * IE：
        * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
        *      if (cmd == "setMapEditSave") {
        *          //这里进行实现
        *      }
        *   }
        * google:
        *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
        *      if (cmd == "setMapEditSave") {
        *          //这里进行实现
        *      }
        *   }
        */
        mxLib.View.callOutCommand("setMapEditSave", "false");
    });
    //双击事件
    myRichMarker.addEventListener("ondblclick", function (e) {
        EditPoint(this);
    });
    //单击事件（只在运行时生效）
    myRichMarker.addEventListener("onclick", function (e) {
        if (!IsGraphicEdit) {
            //回调.net C/S B/S页面
            /**
            * 回调_图形运行时，测点单击事件
            * @class callOutCommand_PointClick 
            * @param 无
            * @returns 测点名称|当前左边距X|当前右边距Y 
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "PointClick":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "PointClick") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "PointClick") {
            *          //这里进行实现
            *      }
            *   }
            */
            var temppoint = this.getPosition();
            var tempPixel = map.pointToPixel(temppoint.x, temppoint.y);
            mxLib.View.callOutCommand("PointClick", this.getJcdName() + "|" + tempPixel.x + "|" + tempPixel.y);
        }
    });
    //覆盖物移动时
    myRichMarker.addEventListener("ondragging", function (e) {
        this.nowPoint = this.getPosition();
        for (var i = 0; i < allPointTranMapArr.length; i++) {
            if (allPointTranMapArr[i].points[0].equals(this.oldPoint)) {
                allPointTranMapArr[i].points[0] = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
                allPointTranMapArr[i].sPointPos = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
            }
            if (allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1].equals(this.oldPoint)) {
                allPointTranMapArr[i].points[allPointTranMapArr[i].points.length - 1] = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
                allPointTranMapArr[i].ePointPos = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
            }
            allPointTranMapArr[i].polyline.setPath(allPointTranMapArr[i].points);
        }
        for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
            if (pointRichMarkerMapArr[i].pointPos.equals(this.oldPoint)) {
                pointRichMarkerMapArr[i].pointPos = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
            }
        }
        this.oldPoint = new mxLib.Point(this.nowPoint.x, this.nowPoint.y);
    });

    //设置点的图元名称
    myRichMarker.setBindUnitName(GrapUnitName);
    //设置点的图元类型
    myRichMarker.setBindTypeName(type);
    //设置点的显示级别
    myRichMarker.setZoomLevel(zoomLevel);
    //设置点的动画状态
    myRichMarker.setanimationState(animationState);
    //设置页面跳转
    myRichMarker.setTurnToPage(TurnToPage);


    //将点添加到地图上
    map.addOverlay(myRichMarker);

    if (IsGraphicEdit) { //如果当前处于编辑状态       
        //添加右键菜单   
        myRichMarker.addContextMenu(richpointRconMenu);
    }
    else {//如果当前处于运行状态
        myRichMarker.addContextMenu(richpointRconMenuInRunState);
    }

    myRichMarker.setJcdName(name);

    //设置旋转角度  20171226   
    myRichMarker.setTransformValue(transformDeg);
    myRichMarker.setTransform(transformDeg);

    //加到对象中
    //    pointRichMarkerMap[name] = myRichMarker;

    var pointObj = {};
    pointObj.pointName = myRichMarker.getJcdName();
    pointObj.pointWz = myRichMarker.getJcdWz();
    pointObj.pointDevName = myRichMarker.getJcdDevName();
    pointObj.width = width;
    pointObj.height = height;
    pointObj.bindTypeName = myRichMarker.getBindTypeName();
    pointObj.bindUnitName = myRichMarker.getBindUnitName();
    pointObj.pointPos = myRichMarker.getPosition();
    pointObj.overlay = myRichMarker;
    pointRichMarkerMapArr.push(pointObj);
}
//保存所有添加的测点(坐标全部保留两位小数)
/**
* 保存所有测点
* @class savePoint 
* @param 无
* @returns 无
* @example 参考示例：
* savePoint();
*/
function savePoint() {
    var arrPoint = new Array();
    var strPoint = "";
    var allOverlay = map.getOverlays();
    //保存所有测点信息
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getJcdName()) {
                if (allOverlay[i].getBindTypeName()) {
                    var x;
                    var y;
                    var Pwidth = 0, Pheight = 0;

                    if (allOverlay[i].getBindTypeName() == "0" || allOverlay[i].getBindTypeName() == "3") {
                        x = allOverlay[i].getPosition().x.toFixed(2);
                        y = allOverlay[i].getPosition().y.toFixed(2);
                    }
                    else {
                        x = allOverlay[i].getBounds().getLeftTop().x.toFixed(2);
                        y = allOverlay[i].getBounds().getLeftTop().y.toFixed(2);
                        //取出SVG图元的高度和宽度进行保存
                        Pwidth = allOverlay[i].getPointWidth();
                        Pheight = allOverlay[i].getPointHeight();
                    }

                    strPoint = strPoint + allOverlay[i].getJcdName() + "|" + allOverlay[i].getBindUnitName() + "|"
             + allOverlay[i].getBindTypeName() + "|" + allOverlay[i].getZoomLevel() + "|" + allOverlay[i].getanimationState()
              + "|" + x + "|" + y + "|" + Pwidth + "|" + Pheight + "|" + allOverlay[i].getTurnToPage() + "|" + allOverlay[i].getTransformValue() + ",";
                }
            }
        }
    }
    if (strPoint.indexOf(",") > 0) {
        strPoint = strPoint.substring(0, strPoint.length - 1);
    }
    console.log(strPoint);
    /**
    * 回调_保存所有测点信息(用于回调界面将所有编排的测点信息保存到数据库中)
    * @class callOutCommand_PointsSave
    * @param 测点属性列表(测点号|绑定图元名称|图元类型|缩放等级|绑定的动画状态|图元左边距|图元右边距|图元宽度|图元高度),多个之间用","分隔
    * @returns 无
    * @example 参考示例：
    *  c#：
    *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
    *        {
    *            string  Param = e.p_sParam;//参数
    *            switch (e.p_sCmd)//命令
    *            {
    *                case "PointsSave":
    *                //这里进行实现
    *                break;
    *            }
    *        }
    * IE：
    * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
    *      if (cmd == "PointsSave") {
    *          //这里进行实现
    *      }
    *   }
    * google:
    *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
    *      if (cmd == "PointsSave") {
    *          //这里进行实现
    *      }
    *   }
    */
    mxLib.View.callOutCommand("PointsSave", strPoint);
    //保存所有拓扑连线
    var strTopologyTrans = "";
    for (var i = 0; i < allPointTbObjList.length; i++) {
        var tranType = allPointTbObjList[i].tranType;
        var P_PointCode = allPointTbObjList[i].sPointName;
        var S_PointCOde = allPointTbObjList[i].ePointName;
        var P_PointPos = allPointTbObjList[i].sPointPos.x.toFixed(2) + "," + allPointTbObjList[i].sPointPos.y.toFixed(2);
        var S_PointPos = allPointTbObjList[i].ePointPos.x.toFixed(2) + "," + allPointTbObjList[i].ePointPos.y.toFixed(2);


        var PointTrans = "";
        for (var j = 0; j < allPointTbObjList[i].points.length; j++) {
            PointTrans = PointTrans + allPointTbObjList[i].points[j].x.toFixed(2) + "," + allPointTbObjList[i].points[j].y.toFixed(2) + "#";
        }
        PointTrans = PointTrans.substring(0, PointTrans.length - 1);

        strTopologyTrans = strTopologyTrans + tranType + "|" + P_PointCode + "|" + S_PointCOde + "|" + P_PointPos + "|" + S_PointPos + "|" + PointTrans + "&";
    }
    if (strTopologyTrans.indexOf("&") > 0) {
        strTopologyTrans = strTopologyTrans.substring(0, strTopologyTrans.length - 1);
    }
    console.log(strTopologyTrans);
    /**
    * 回调_保存所有测点信息连线信息(用于回调界面将所有编排的测点连线信息保存到数据库中)
    * @class callOutCommand_RoutesSave
    * @param 路线列表（路线类型[0:S线，1:L线]|开始点号|结束点号|开始点X坐标,开始点Y坐标|结束点X坐标,结束点Y坐标|两点间的所有连线坐标”#“分隔）,多个用“&”分隔
    * @returns 无
    * @example 参考示例：
    *  c#：
    *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
    *        {
    *            string  Param = e.p_sParam;//参数
    *            switch (e.p_sCmd)//命令
    *            {
    *                case "RoutesSave":
    *                //这里进行实现
    *                break;
    *            }
    *        }
    * IE：
    * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
    *      if (cmd == "RoutesSave") {
    *          //这里进行实现
    *      }
    *   }
    * google:
    *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
    *      if (cmd == "RoutesSave") {
    *          //这里进行实现
    *      }
    *   }
    */
    mxLib.View.callOutCommand("RoutesSave", strTopologyTrans);
    //保存所有测点波及巷道连线
    var strTrajectorys = "";
    for (var i = 0; i < allPointTranMapArr.length; i++) {
        var tranType = "";
        var P_PointCode = allPointTranMapArr[i].sPointName;
        var S_PointCOde = allPointTranMapArr[i].ePointName;
        var P_PointPos = allPointTranMapArr[i].sPointPos.x.toFixed(2) + "," + allPointTranMapArr[i].sPointPos.y.toFixed(2);
        var S_PointPos = allPointTranMapArr[i].ePointPos.x.toFixed(2) + "," + allPointTranMapArr[i].ePointPos.y.toFixed(2);

        var PointTrans = "";
        for (var j = 0; j < allPointTranMapArr[i].points.length; j++) {
            PointTrans = PointTrans + allPointTranMapArr[i].points[j].x.toFixed(2) + "," + allPointTranMapArr[i].points[j].y.toFixed(2) + "#";
        }
        PointTrans = PointTrans.substring(0, PointTrans.length - 1);

        strTrajectorys = strTrajectorys + tranType + "|" + P_PointCode + "|" + S_PointCOde + "|" + P_PointPos + "|" + S_PointPos + "|" + PointTrans + "&";
    }
    if (strTrajectorys.indexOf("&") > 0) {
        strTrajectorys = strTrajectorys.substring(0, strTrajectorys.length - 1);
    }
    console.log(strTrajectorys);
    mxLib.View.callOutCommand("RoutesSave", strTrajectorys);
}
/**
* 获取所有测点的显示隐藏状态
* @class GetAllPointDis 
* @param 无
* @returns 返回值：Point列表（多个用“|”分隔），如:Point1,显示隐藏(true：表示显示，false：表示隐藏)|Point2,显示隐藏
* @example 参考示例：
* GetAllPointDis();
*/
function GetAllPointDis() {
    var strPoint = "";
    var allOverlay = map.getOverlays();
    //获取所有测点的显示隐藏状态
    for (var i = 0; i < allOverlay.length; i++) {
        if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
            if (allOverlay[i].getJcdName()) {
                if (allOverlay[i].getBindTypeName()) {
                    strPoint = strPoint + allOverlay[i].getJcdName() + "," + allOverlay[i].isVisible() + "|";
                }
            }
        }
    }
    return strPoint;
}

//加载测点到地图上
/**
* 加载测点到地图上
* @class LoadPoint 
* @param {pointinitStr} 测点列表(测点号|安装位置|设备类型|绑定图元名称|图元类型【说明：0：实时显示图元，1:拓扑图图元，2：SVG图元，3：gif动画图元,4:标注图元】|缩放等级|绑定的动画状态|图元左边距|图元右边距|图元宽度|图元高度|图形类型【0:动态图，1：拓扑图】|跳转页面名称|旋转角度),多个用“,”分隔 如：001A060|2205综采工作面上隅角一氧化碳|低浓瓦斯(迎头)|实时显示|0|0|0|8144.84|112.62||,001A010|2205综采工作面一氧化碳|低浓瓦斯(迎头)|实时显示|0|0|0|8265.15|112.45||
* @returns 返回值：无
* @example 参考示例：
* LoadPoint('001A060|2205综采工作面上隅角一氧化碳|低浓瓦斯(迎头)|实时显示|0|0|0|8144.84|112.62|60|60|0|XX页面
,001A010|2205综采工作面一氧化碳|低浓瓦斯(迎头)|实时显示|0|0|0|8265.15|112.45|60|60|0|XX页面|0');
*/
function LoadPoint(pointinitStr) {
    if (pointinitStr.length < 0) {
        return;
    }
    var tempArr = pointinitStr.split(",");
    for (var i = 0; i < tempArr.length; i++) {
        var tempArr_ = tempArr[i].split("|");
        if (tempArr_[0].length > 0) {
            //将当前测点添加到地图上
            var type = tempArr_[4];
            switch (type) {
                case "0": //实时显示图元
                case "4": //实时显示图元
                case "3": //gif动画图元
                    AddPointToMap(tempArr_[0], tempArr_[1], tempArr_[2], tempArr_[3], tempArr_[4], tempArr_[5],
                    tempArr_[6], tempArr_[7], tempArr_[8], tempArr_[9], tempArr_[10], tempArr_[11], tempArr_[12], tempArr_[13]);
                    break;
                case "1": //拓扑图图元                   
                    AddTBPointToMap(tempArr_[0], tempArr_[1], tempArr_[2], tempArr_[3], tempArr_[4], tempArr_[5],
                    tempArr_[6], tempArr_[7], tempArr_[8], parseFloat(tempArr_[9]), parseFloat(tempArr_[10]), tempArr_[11], tempArr_[12], tempArr_[13]);
                    break;
                case "2": //SVG图元
                    AddSVGPointToMap(tempArr_[0], tempArr_[1], tempArr_[2], tempArr_[3], tempArr_[4], tempArr_[5],
                     tempArr_[6], tempArr_[7], tempArr_[8], parseFloat(tempArr_[9]), parseFloat(tempArr_[10]), tempArr_[11], tempArr_[12], tempArr_[13]);
                    break;
            }

        }
    }
    ////加载完成，全部置成显示状态
    //var allOverlay = map.getOverlays();
    ////获取所有测点的显示隐藏状态
    //for (var i = 0; i < allOverlay.length; i++) {
    //    if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
    //        if (allOverlay[i].getJcdName()) {
    //            if (allOverlay[i].getBindTypeName()) {
    //                 allOverlay[i].show();
    //            }
    //        }
    //    }
    //}
    //加载完成，调用加载tooltips
    addToolTips();
    //测点加载完成，根据编辑状态来判断是否启动实时刷新
    if (!IsGraphicEdit) {
        window.setTimeout("DoRefPointSsz()", 1000);
    }
}
//加载拓扑路线到地图上
/**
* 加载拓扑图路线到地图上
* @class LoadRoutes 
* @param {topologyTransStr} 路线列表（路线类型[0:S线，1:L线]|开始点号|结束点号|开始点X坐标,开始点Y坐标|结束点X坐标,结束点Y坐标|两点间的所有连线坐标”#“分隔）,多个用“&”分隔 如：0|000000-13|COM2|388.26,489.3|2943.26,224.3
|388.26,489.30#388.26,356.80#2943.26,356.80#2943.26,224.30
&0|000000-13|COM3|388.26,489.3|3063.26,224.3
|388.26,489.30#388.26,356.80#3063.26,356.80#3063.26,224.30
* @returns 返回值：无
* @example 参考示例：
* LoadRoutes('0|000000-13|COM2|388.26,489.3|2943.26,224.3
|388.26,489.30#388.26,356.80#2943.26,356.80#2943.26,224.30
&0|000000-13|COM3|388.26,489.3|3063.26,224.3
|388.26,489.30#388.26,356.80#3063.26,356.80#3063.26,224.30');
*/
function LoadRoutes(topologyTransStr) {
    if (topologyTransStr.length < 0) {
        return;
    }
    var tempPointTbObjList = topologyTransStr.split('&');
    for (var i = 0; i < tempPointTbObjList.length; i++) {
        var tempPointTbObj = tempPointTbObjList[i].split('|');
        var tranType = tempPointTbObj[0];
        var P_PointCode = tempPointTbObj[1];
        var S_PointCode = tempPointTbObj[2];
        var P_PointPos = tempPointTbObj[3];
        var S_PointPos = tempPointTbObj[4];

        var points = tempPointTbObj[5].split('#');
        var pointsList = [];
        for (var j = 0; j < points.length; j++) {
            var pointobj = points[j].split(',');
            pointsList.push(new mxLib.Point(parseFloat(pointobj[0]), parseFloat(pointobj[1])));
        }
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
        TbTranObj.sPointPos = new mxLib.Point(parseFloat(P_PointPos.split(',')[0]), parseFloat(P_PointPos.split(',')[1]));
        TbTranObj.ePointPos = new mxLib.Point(parseFloat(S_PointPos.split(',')[0]), parseFloat(S_PointPos.split(',')[1]));

        allPointTbObjList.push(TbTranObj);

    }
}
//加载巷道路线
/**
* 加载动态图路线到地图上
* @class LoadTrajectorys 
* @param {trajectorysStr} 路线列表（路线类型[此处未使用]|开始点号|结束点号|开始点X坐标,开始点Y坐标|结束点X坐标,结束点Y坐标|两点间的所有连线坐标”#“分隔）,多个用“&”分隔 如：|001A010|002A070|8144.84,112.62|8265.15,112.45|8144.84,112.62#8265.15,112.45
&|002A070|001A040|8265.15,112.45|8514.1,111.94|8265.15,112.45#8514.10,111.94
* @param {strokewidth} 显示线宽度
* @returns 返回值：无
* @example 参考示例：
* LoadTrajectorys('|001A010|002A070|8144.84,112.62|8265.15,112.45|8144.84,112.62#8265.15,112.45
&|002A070|001A040|8265.15,112.45|8514.1,111.94|8265.15,112.45#8514.10,111.94','3');
*/

function LoadTrajectorys(trajectorysStr, strokewidth) {
    if (trajectorysStr.length < 0) {
        return;
    }
    var tempPointTbObjList = trajectorysStr.split('&');
    for (var i = 0; i < tempPointTbObjList.length; i++) {
        var tempPointTbObj = tempPointTbObjList[i].split('|');
        var tranType = tempPointTbObj[0];
        var P_PointCode = tempPointTbObj[1];
        var S_PointCode = tempPointTbObj[2];
        var P_PointPos = tempPointTbObj[3];
        var S_PointPos = tempPointTbObj[4];

        var points = tempPointTbObj[5].split('#');
        var pointsList = [];
        for (var j = 0; j < points.length; j++) {
            var pointobj = points[j].split(',');
            pointsList.push(new mxLib.Point(parseFloat(pointobj[0]), parseFloat(pointobj[1])));
        }
        if (!allPointTbObjList) {
            allPointTbObjList = [];
        }

        var TbTranObj = {}; //定义拓扑图对象

        //连线类型
        TbTranObj.tranType = tranType;
        //添加到路线点数组
        TbTranObj.points = pointsList;

        //添加到线路数组
        TbTranObj.polyline = new mxLib.Polyline(pointsList, { strokeColor: "green", strokeWidth: parseInt(strokewidth), strokeOpacity: 0.8, enableClicking: false });
        map.addOverlay(TbTranObj.polyline);

        //将路线的起始点编号、结束点编号添加到路线编号数组                    
        TbTranObj.sPointName = P_PointCode;
        TbTranObj.ePointName = S_PointCode;

        //将Svg的起始点坐标存到对象数组                   
        TbTranObj.sPointPos = new mxLib.Point(parseFloat(P_PointPos.split(',')[0]), parseFloat(P_PointPos.split(',')[1]));
        TbTranObj.ePointPos = new mxLib.Point(parseFloat(S_PointPos.split(',')[0]), parseFloat(S_PointPos.split(',')[1]));

        allPointTranMapArr.push(TbTranObj);

    }
}


//------------------20170825(新增测点坐标拾取功能)--------------------------------
var CoordinatePickUp_PointNow = {};
var CoordinatePickUp_EditFlag = false;
//在地图上面添加一个坐标拾取传感器的点
function CoordinatePickUp_AddSensor(opt) {
    var name = opt.name;
    var PointId = opt.PointId;
    var wz = opt.wz;
    var position = opt.position;
    var minZoom = opt.minZoom;
    var maxZoom = opt.maxZoom;
    var path = opt.path;
    var isAlarm = opt.isAlarm;

    //       var html = '<div style="position: absolute; margin: 0pt; padding: 0pt; width: 80px; height: 36px; left: 0px; top: 0px; overflow: hidden;">'
    //+ '<img id="rm3_image" style="border:none;left:0px; top:0px; position:absolute;" src="' + path + '">'
    //+ '</div>'
    //+ '<label class=" BMapLabel" unselectable="on" style="position: absolute; -moz-user-select: none; display: inline; cursor: inherit; border: 0px none; padding: 2px 1px 1px; white-space: nowrap; font: 12px arial,simsun bold; z-index: 80; color: rgb(30, 144, 255); left: 25px; top: 3px;">38℃</label>';

    var html = '<div style="position: absolute; margin: 0pt; padding: 0pt; width: 34px; height: 50px; left: 0px; top: 0px; overflow: hidden;">'
    + '<img  class="RichMarker_Image" style="border:none;left:0px; top:0px; position:absolute;" src="' + path + '">'
    + '</div>';

    var myMarker = new mxLib.RichMarker(html, position, {
        "anchor": new mxLib.Size(-17, -50),
        "enableDragging": false
        //"minZoom1": minZoom,
        //"maxZoom1": maxZoom,
        //"wz1": wz,
        //"position1": position,
        //"ssz1": opt.ssz,
        //"isalarm1": opt.isAlarm,
        //"devname1": opt.devname,
        //"name1": opt.name,
        //"PointId1": opt.PointId,
        //"datastate1": opt.datastate
    });


    map.addOverlay(myMarker);
    CoordinatePickUp_PointNow = myMarker;
    //if (isAlarm) {
    //    //DrawXYAnmi(position.x, position.y, '#FF3030', '5000000', 0.0003);
    //}
}
//添加一个坐标拾取传感器的点
function CoordinatePickUp_AddPoint(x, y) {
    x = parseFloat(x);
    y = parseFloat(y);
    if (CoordinatePickUp_PointNow._opts) {
        var point = new mxLib.Point(x, y);
        CoordinatePickUp_PointNow.setPosition(point);
        return;
    }
    //设备
    var sbOpt = {
        name: '',
        wz: '',
        devname: '',
        ssz: '',
        position: new mxLib.Point(x, y),
        minZoom: 1,
        maxZoom: 22,
        path: 'mx/Marker/设备.png',
        isAlarm: 0,
        PointId: "15",
        datastate: "0"
    }
    CoordinatePickUp_AddSensor(sbOpt);
}
//设置是否启动坐标拾取
function SetCoordinatePickUpEditFlag(State) {
    if (State == "true") {
        CoordinatePickUp_EditFlag = true;
    }
    else {
        CoordinatePickUp_EditFlag = false;
    }
}

