/*
* 人员轨迹行走、测点定位demo 
*/

//console.log("lushu.js");

function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}
/**
* 显示避灾路线
* @class ViewRoute 
* @param {type} 路线类型 如：ryRoute（人员路线）【暂不支持】，qyRoute（区域路线）【暂不支持】，escapeRoute（避灾路线）
* @param {points} 路线上的所有点  如：388.26|489.3,2943.26|224.3
* @param {linecolor} 路线颜色设置
* @param {arrowcolor} 箭头颜色设置
* @param {timer} 显示时间 单位：毫秒
* @returns 返回值：无
* @example 参考示例：
* ViewRoute('escapeRoute','388.26|489.3,2943.26|224.3','green','blue','60000');
*/
function ViewRoute(type, pointlist, linecolor, arrowcolor, timer) {
    if (pointlist.length < 1) {
        return;
    }
    this.escapeRouteArr = [];
    var points = [];//测点对象列表
    var temppoints = pointlist.split(',');
    for (var i = 0; i < temppoints.length; i++) {
        var temppoint = temppoints[i].split('|');
        var point = new mxLib.Point(parseFloat(temppoint[0]), parseFloat(temppoint[1]));
        points.push(point);
    }
    switch (type) {
        case "ryRoute"://人员路线

            break;
        case "qyRoute"://区域路线

            break;
        case "escapeRoute"://避灾路线

            if (this.escapeRouteArr.length > 0)
                return;

            //画路线
            var polyline = new mxLib.Polyline(points, { strokeColor: linecolor, strokeWidth: 3, strokeOpacity: 0.7, enableClicking: false });
            window.map.addOverlay(polyline);

            //添加到数组
            this.escapeRouteArr.push(polyline);

            //转换为svgpath
            var vl = map.curZoomVectorLayer();

            var route = "M";
            for (var i = 0; i < points.length; i++) {
                var co = vl.fromMapPoint(points[i].x, points[i].y);
                route = route + co.x + "," + co.y + " ";
                if (i == 0) {
                    route = route + "L";
                }
                else {
                    route = route + "";
                }
            }

            var svg = vl.snap();
            //求取长度
            var c = svg.paper.path(route).attr({
                strokeWidth: 2,
                fill: "none"
            });
            var length = c.getTotalLength();

            var svgArr = [];
            for (var i = 0; i < 10; i++) {
                if (i == 11)
                    break;

                var begin = i * (length / 10);
                var end = (i + 1) * length / 10;

                //新建path
                var subPath = c.getSubpath(begin, end);

                //调用svg函数
                var arrowAnimate1 = new mxLib.AnimatePath(map, subPath, { dur: "3s", scale: 0.7, color: arrowcolor });

                //开始播放
                if (arrowAnimate1) {
                    arrowAnimate1.startAnimate();
                }

                svgArr.push(arrowAnimate1);
            }

            //60s后删除
            var self = this;
            window.setTimeout(function () {
                for (var i in svgArr) {
                    var arrowAnimate = svgArr[i];
                    if ($.isEmptyObject(arrowAnimate))
                        continue;
                    if (arrowAnimate instanceof mxLib.AnimatePath) {
                        arrowAnimate.remove();
                        arrowAnimate = null;
                    }
                    if (!$.isEmptyObject(polyline)) {
                        //移除覆盖物
                        map.removeOverlay(polyline);
                        //清空数组
                        self.escapeRouteArr.length = 0;
                    }
                }
            }, parseInt(timer));
            break;
        default:
            break;
    }
}

//添加设备信息
function SearchPoint() {

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
        height: 150,
        width: 400,
        items: form,
        closeAction: 'close',
        layout: 'fit',
        region: "center",
        resizable: false,
        title: '添加测点',
        modal: true,
        buttons: [{
            text: '定位',
            iconCls: 'save',
            handler: function () {
                //测点定位
                var tempPoint;
                for (var i = 0; i < pointRichMarkerMapArr.length; i++) {
                    if (pointRichMarkerMapArr[i].pointName == PointName.getValue()) {
                        tempPoint = pointRichMarkerMapArr[i].pointPos;
                        break;
                    }
                }
                map.setCenter(tempPoint);
                //添加一个跳动动画
                //mxLib.Util.setAnimation(pointRichMarkerMap[PointName.getValue()].getDomElement(), MX_ANIMATION_BOUNCE,3000);
                //闪烁背景效果 
                var circle = new mxLib.Circle(tempPoint, 20, {
                    strokeColor: "green",
                    strokeWidth: 2,
                    strokeOpacity: 0.5,
                    enableClicking: true,
                    fillColor: "blue"
                });
                map.addOverlay(circle);
                //闪烁
                mxLib.Util.setAnimation(circle.getDomElement(), MX_ANIMATION_FLASH, 5000);
                //扩大
                circle.snap().animate({
                    r: 50
                }, 2000);
                //动画效果完成后删除覆盖物
                window.setTimeout(function () {
                    map.removeOverlay(circle);
                }, 5000);
                //关闭窗口
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
    win.setTitle("测点定位");
}

(function () {

    //声明baidu包
    var T, baidu = T = baidu || { version: '1.5.0' };
    var pauseFlag = false; //暂停
    baidu.guid = '$BAIDU$';
    //以下方法为百度Tangram框架中的方法，请到http://tangram.baidu.com 查看文档
    (function () {
        window[baidu.guid] = window[baidu.guid] || {};
        baidu.dom = baidu.dom || {};
        baidu.dom.g = function (id) {
            if ('string' == typeof id || id instanceof String) {
                return document.getElementById(id);
            } else if (id && id.nodeName && (id.nodeType == 1 || id.nodeType == 9)) {
                return id;
            }
            return null;
        };
        baidu.g = baidu.G = baidu.dom.g;
        baidu.lang = baidu.lang || {};
        baidu.lang.isString = function (source) {
            return '[object String]' == Object.prototype.toString.call(source);
        };
        baidu.isString = baidu.lang.isString;
        baidu.dom._g = function (id) {
            if (baidu.lang.isString(id)) {
                return document.getElementById(id);
            }
            return id;
        };
        baidu._g = baidu.dom._g;
        baidu.dom.getDocument = function (element) {
            element = baidu.dom.g(element);
            return element.nodeType == 9 ? element : element.ownerDocument || element.document;
        };
        baidu.browser = baidu.browser || {};
        baidu.browser.ie = baidu.ie = /msie (\d+\.\d+)/i.test(navigator.userAgent) ? (document.documentMode || +RegExp['\x241']) : undefined;
        baidu.dom.getComputedStyle = function (element, key) {
            element = baidu.dom._g(element);
            var doc = baidu.dom.getDocument(element),
                styles;
            if (doc.defaultView && doc.defaultView.getComputedStyle) {
                styles = doc.defaultView.getComputedStyle(element, null);
                if (styles) {
                    return styles[key] || styles.getPropertyValue(key);
                }
            }
            return '';
        };
        baidu.dom._styleFixer = baidu.dom._styleFixer || {};
        baidu.dom._styleFilter = baidu.dom._styleFilter || [];
        baidu.dom._styleFilter.filter = function (key, value, method) {
            for (var i = 0, filters = baidu.dom._styleFilter, filter; filter = filters[i]; i++) {
                if (filter = filter[method]) {
                    value = filter(key, value);
                }
            }
            return value;
        };
        baidu.string = baidu.string || {};


        baidu.string.toCamelCase = function (source) {

            if (source.indexOf('-') < 0 && source.indexOf('_') < 0) {
                return source;
            }
            return source.replace(/[-_][^-_]/g, function (match) {
                return match.charAt(1).toUpperCase();
            });
        };
        baidu.dom.getStyle = function (element, key) {
            var dom = baidu.dom;
            element = dom.g(element);
            key = baidu.string.toCamelCase(key);

            var value = element.style[key] ||
                        (element.currentStyle ? element.currentStyle[key] : '') ||
                        dom.getComputedStyle(element, key);

            if (!value) {
                var fixer = dom._styleFixer[key];
                if (fixer) {
                    value = fixer.get ? fixer.get(element) : baidu.dom.getStyle(element, fixer);
                }
            }

            if (fixer = dom._styleFilter) {
                value = fixer.filter(key, value, 'get');
            }
            return value;
        };
        baidu.getStyle = baidu.dom.getStyle;
        baidu.dom._NAME_ATTRS = (function () {
            var result = {
                'cellpadding': 'cellPadding',
                'cellspacing': 'cellSpacing',
                'colspan': 'colSpan',
                'rowspan': 'rowSpan',
                'valign': 'vAlign',
                'usemap': 'useMap',
                'frameborder': 'frameBorder'
            };

            if (baidu.browser.ie < 8) {
                result['for'] = 'htmlFor';
                result['class'] = 'className';
            } else {
                result['htmlFor'] = 'for';
                result['className'] = 'class';
            }

            return result;
        })();
        baidu.dom.setAttr = function (element, key, value) {
            element = baidu.dom.g(element);
            if ('style' == key) {
                element.style.cssText = value;
            } else {
                key = baidu.dom._NAME_ATTRS[key] || key;
                element.setAttribute(key, value);
            }
            return element;
        };
        baidu.setAttr = baidu.dom.setAttr;
        baidu.dom.setAttrs = function (element, attributes) {
            element = baidu.dom.g(element);
            for (var key in attributes) {
                baidu.dom.setAttr(element, key, attributes[key]);
            }
            return element;
        };
        baidu.setAttrs = baidu.dom.setAttrs;
        baidu.dom.create = function (tagName, opt_attributes) {
            var el = document.createElement(tagName),
                attributes = opt_attributes || {};
            return baidu.dom.setAttrs(el, attributes);
        };
        baidu.object = baidu.object || {};
        baidu.extend =
        baidu.object.extend = function (target, source) {
            for (var p in source) {
                if (source.hasOwnProperty(p)) {
                    target[p] = source[p];
                }
            }
            return target;
        };
    })();

    /*
    * exports LuShu as mxLibLib.LuShu
    */
    var LuShu =
    /*
    * LuShu类的构造函数
    * LuShu <b>入口</b>。
    * 实例化该类后，可调用,start,end,pause等方法控制覆盖物的运动。

    * constructor
    * param {Map} map Baidu map的实例对象.
    * param {Array} path 构成路线的point的数组.
    * param {Json Object} opts 可选的输入参数，非必填项。可输入选项包括：<br />
    * {<br />"<b>landmarkPois</b>" : {Array} 要在覆盖物移动过程中，显示的特殊点。格式如下:landmarkPois:[<br />
    *      {lng:116.314782,lat:39.913508,html:'加油站',pauseTime:2},<br />
    *      {lng:116.315391,lat:39.964429,html:'高速公路收费站,pauseTime:3}]<br />
    * <br />"<b>icon</b>" : {Icon} 覆盖物的icon,
    * <br />"<b>speed</b>" : {Number} 覆盖物移动速度，单位米/秒    <br />
    * <br />"<b>defaultContent</b>" : {String} 覆盖物中的内容    <br />
    * }<br />.
    * example <b>参考示例：</b><br />
    * var lushu = new mxLibLib.LuShu(map,arrPois,{defaultContent:"从北京到天津",landmarkPois:[]});
    */
     mxLib.LuShu = function (map, path, opts) {
         //定义当前轨迹行走的所有路线对象
         this.trancelineList;
         //定义当前轨迹行走的guid
         this.guid = newGuid();
         //定义人物方向缓存变量
         this.PersonDirection = "";
         //定义当前人物
         this.myIconDefault = new mxLib.Icon("mx/PersonImg/前.gif",
            { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerRight = new mxLib.Icon("mx/PersonImg/右.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerLeft = new mxLib.Icon("mx/PersonImg/左.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerBefore = new mxLib.Icon("mx/PersonImg/前.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerAfter = new mxLib.Icon("mx/PersonImg/后.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerLeftTop = new mxLib.Icon("mx/PersonImg/左上.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerLeftButtom = new mxLib.Icon("mx/PersonImg/左下.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerRightTop = new mxLib.Icon("mx/PersonImg/右上.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });
         this.IconPerRightButtom = new mxLib.Icon("mx/PersonImg/右下.gif",
                     { width: 64, height: 94 }, { anchor: new mxLib.Size(32, 94) });

         if (!path || path.length < 1) {
             return;
         }
         this._map = map;
         //存储一条路线
         this._path = path;
         //移动到当前点的索引
         this.i = 0;
         //控制暂停后开始移动的队列的数组
         this._setTimeoutQuene = [];
         //进行坐标转换的类

         this._opts = {
             icon: null,
             //默认速度 米/秒
             speed: 4000,
             defaultContent: ''
         };
         this._setOptions(opts);
         this._rotation = 0; //小车转动的角度

         //如果不是默认实例，则使用默认的icon
         if (!this._opts.icon instanceof mxLib.Icon) {
             this._opts.icon = this.myIconDefault;
         }
     }
    /*
    * 根据用户输入的opts，修改默认参数_opts
    * param {Json Object} opts 用户输入的修改参数.
    * return 无返回值.
    */
    LuShu.prototype._setOptions = function (opts) {
        if (!opts) {
            return;
        }
        for (var p in opts) {
            if (opts.hasOwnProperty(p)) {
                this._opts[p] = opts[p];
            }
        }
    }

    /*
    * description 开始运动
    * param none
    * return 无返回值.
    *
    * example <b>参考示例：</b><br />
    * lushu.start();
    */
    LuShu.prototype.start = function () {
        var me = this,
            len = me._path.length;

        //绘制轨迹路线，并保存到内存对象中
        var points = [];
        var index = 0;
        for (var i = 0; i < me._path.length; i++) {
            points[index] = new mxLib.Point(me._path[i].points.x, me._path[i].points.y);
            index++;
        }
        me.trancelineList = new mxLib.Polyline(points, { strokeColor: "red", strokeWidth: 3, strokeOpacity: 0.9, enableClicking: true });
        map.addOverlay(me.trancelineList);


        //不是第一次点击开始,并且小车还没到达终点
        if (me.i && me.i < len - 1) {
            me.stop();
        } else {
            //第一次点击开始，或者点了stop之后点开始
            me._addMarker();
            //等待marker动画完毕再加载infowindow
            me._timeoutFlag = setTimeout(function () {
                me._addInfoWin();
                if (me._opts.defaultContent == "") {
                    me.hideInfoWindow();
                }
                me._moveNext(me.i);
            }, 100);
        }
    },
    /*
    * 结束运动
    * return 无返回值.
    *
    * example <b>参考示例：</b><br />
    * lushu.stop();
    */
    LuShu.prototype.stop = function () {
        this.i = 0;
        this._fromStop = true;

        clearInterval(this._intervalFlag);
        this._clearTimeout();
        map.removeOverlay(this._marker);
        map.removeOverlay(this._overlay);
        //重置landmark里边的poi为未显示状态
        //        for (var i = 0, t = this._opts.landmarkPois, len = t.length; i < len; i++) {
        //            t[i].bShow = false;
        //        }
        //重置状态       
        this._fromPause = false;
        this._fromStop = false;
    };
    /*
    * 暂停运动
    * return 无返回值.
    */
    LuShu.prototype.pause = function () {
        //clearInterval(this._intervalFlag);

        //标识是否是按过pause按钮
        var me = this;
        me._fromPause = true;
        //this._clearTimeout();
    };
    /*
    * 继续运动
    * return 无返回值.
    */
    LuShu.prototype.repause = function () {
        //clearInterval(this._intervalFlag);

        //标识是否是按过pause按钮
        var me = this;
        me._fromPause = false;
        //this._clearTimeout();
    };
    /*
    * 隐藏上方overlay
    * return 无返回值.
    *
    * example <b>参考示例：</b><br />
    * lushu.hideInfoWindow();
    */
    LuShu.prototype.hideInfoWindow = function () {
        this._overlay._div.style.visibility = 'hidden';
    };
    /*
    * 显示上方overlay
    * return 无返回值.
    *
    * example <b>参考示例：</b><br />
    * lushu.showInfoWindow();
    */
    LuShu.prototype.showInfoWindow = function () {
        this._overlay._div.style.visibility = 'visible';
    };
    //Lushu私有方法
    baidu.object.extend(LuShu.prototype, {
        /*
        * 添加marker到地图上
        * param {Function} 回调函数.
        * return 无返回值.
        */
        _addMarker: function (callback) {
            if (this._marker) {
                this.stop();
                this._map.removeOverlay(this._marker);
                clearTimeout(this._timeoutFlag);
            }
            //移除之前的overlay
            this._overlay && this._map.removeOverlay(this._overlay);
            var marker = new mxLib.Marker(this._path[0].points);
            this._opts.icon && marker.setIcon(this._opts.icon);
            this._map.addOverlay(marker);
            //marker.setAnimation(mxLib_ANIMATION_DROP);
            this._marker = marker;
        },
        /*
        * 添加上方overlay
        * return 无返回值.
        */
        _addInfoWin: function () {
            var me = this;
            //if(me._opts.defaultContent!== ""){
            var overlay = new CustomOverlay(me._marker.getPosition(), me._opts.defaultContent);

            //将当前类的引用传给overlay。
            overlay.setRelatedClass(this);
            this._overlay = overlay;
            this._map.addOverlay(overlay);

            //}

        },
        /*
        * 获取墨卡托坐标
        * param {Point} poi 经纬度坐标.
        * return 无返回值.
        */
        _getMercator: function (poi) {
            return this._map.getMapType().getProjection().lngLatToPoint(poi);
        },
        /*
        * 计算两点间的距离
        * param {Point} poi 经纬度坐标A点.
        * param {Point} poi 经纬度坐标B点.
        * return 无返回值.
        */
        _getDistance: function (pxA, pxB) {
            return Math.sqrt(Math.pow(pxA.x - pxB.x, 2) + Math.pow(pxA.y - pxB.y, 2));
        },
        //目标点的  当前的步长,position,总的步长,动画效果,回调
        /*
        * 移动小车
        * param {Number} poi 当前的步长.
        * param {Point} initPos 经纬度坐标初始点.
        * param {Point} targetPos 经纬度坐标目标点.
        * param {Function} effect 缓动效果.
        * return 无返回值.
        */
        _move: function (initPos, targetPos, distext, effect) {
            var me = this,
            //当前的帧数
                currentCount = 0,
            //步长，米/秒
                timer = 10,
                step = this._opts.speed / (1000 / timer),
            //初始坐标
                initPosPoint = initPos,
            //init_pos = map.pointToPixel(initPos),
            //获取结束点的(x,y)坐标
                targetPosPoint = targetPos,
            //target_pos = map.pointToPixel(targetPos),
            //总的步长
                count = Math.round(me._getDistance(initPosPoint, targetPosPoint) / step);

            //如果小于1直接移动到下一点
            if (count < 1) {
                me._moveNext(++me.i);
                return;
            }
            //两点之间匀速移动
            me._intervalFlag = setInterval(function () {
                if (!me._fromPause) {
                    //mxLib.Util.toastInfo("未暂停！", { delay: 5 });
                    //两点之间当前帧数大于总帧数的时候，则说明已经完成移动
                    if (currentCount >= count) {
                        clearInterval(me._intervalFlag);
                        //me._opts.defaultContent=me.i;
                        //me._opts.icon=myIcon1;
                        //移动的点已经超过总的长度
                        if (me.i >= me._path.length - 2) {
                            //走完之后，清除缓存，删除覆盖物   
                            //删除缓存对象中的lushu
                            for (var i = lushuArr.length - 1; i >= 0; i--) {
                                if (me.guid == lushuArr[i].guid) {
                                    lushuArr.splice(i, 1);
                                }
                            }
                            map.removeOverlay(me._marker);
                            map.removeOverlay(me._overlay);
                            //清除轨迹线路
                            map.removeOverlay(me.trancelineList);
                            //重置状态       
                            me._fromPause = false;
                            me._fromStop = false;
                            return;
                        }
                        //运行下一个点
                        me._moveNext(++me.i);
                    } else {
                        currentCount++;
                        init_pos = map.pointToPixel(initPosPoint); //开始点
                        target_pos = map.pointToPixel(targetPosPoint); //结束点
                        var x = effect(init_pos.x, target_pos.x, currentCount, count),
                        y = effect(init_pos.y, target_pos.y, currentCount, count),
                        pos = new mxLib.Pixel(x, y);
                        pos = map.pixelToPoint(pos); //将像素点转成地理坐标
                        //根据方向设置人物的图像
                        if (me._opts.enableRotation == true) {
                            me.setRotation(init_pos, target_pos);
                        }
                        //保持小人始终在中间显示
                        if (lushuArr.length < 2) {//只有一个小人的时候才启用
                            if (me._opts.autoView) {
                                if (!me._map.getBounds().containsPoint(pos)) {
                                    me._map.setCenter(pos);
                                }
                            }
                        }
                        //正在移动
                        me._marker.setPosition(pos);
                        //设置自定义overlay的位置
                        me._setInfoWin(pos);
                        //设置InfoWin显示内容
                        me._setInfoWinText(distext);
                    }
                }
            }, timer);
        },
        /*
        *在每个点的真实步骤中设置小车转动的角度
        */
        setRotation: function (curPos, targetPos) {
            var me = this;
            var tan = (targetPos.y - curPos.y) / (targetPos.x - curPos.x),
                    atan = Math.atan(tan);
            deg = atan * 360 / (2 * Math.PI);
            var fx = Math.abs(deg);

            if (fx >= 0 && fx <= 15)//平行(小于15度)
            {
                if (targetPos.x - curPos.x > 0)//从左向右走
                {
                    //console.log("平行，从左向右走！");
                    //this._marker.setIcon(myIcon1); 
                    if (me.PersonDirection != "IconPerRight") {
                        me.PersonDirection = "IconPerRight";
                        this._marker.setIcon(me.IconPerRight);
                    }
                }
                else//从右向左走
                {
                    //console.log("平行，从右向左走！");
                    if (me.PersonDirection != "IconPerLeft") {
                        me.PersonDirection = "IconPerLeft";
                        this._marker.setIcon(me.IconPerLeft);
                    }
                }
            }
            else if (fx > 15 && fx <= 75)//斜行（大于15度小于75度）
            {
                if (targetPos.x - curPos.x > 0 && targetPos.y - curPos.y > 0)//右下
                {
                    //console.log("斜行，从左上往下走！");
                    if (me.PersonDirection != "IconPerRightButtom") {
                        me.PersonDirection = "IconPerRightButtom";
                        this._marker.setIcon(me.IconPerRightButtom);
                    }
                }
                else if (targetPos.x - curPos.x > 0 && targetPos.y - curPos.y < 0)//右上
                {
                    //console.log("斜行，从左下往上走！");
                    if (me.PersonDirection != "IconPerRightTop") {
                        me.PersonDirection = "IconPerRightTop";
                        this._marker.setIcon(me.IconPerRightTop);
                    }
                }
                else if (targetPos.x - curPos.x < 0 && targetPos.y - curPos.y > 0)//左下
                {
                    //console.log("斜行，从右上往下走！");
                    if (me.PersonDirection != "IconPerLeftButtom") {
                        me.PersonDirection = "IconPerLeftButtom";
                        this._marker.setIcon(me.IconPerLeftButtom);
                    }
                }
                else if (targetPos.x - curPos.x < 0 && targetPos.y - curPos.y < 0)//左上
                {
                    //console.log("斜行，从右下往上走！");
                    if (me.PersonDirection != "IconPerLeftTop") {
                        me.PersonDirection = "IconPerLeftTop";
                        this._marker.setIcon(me.IconPerLeftTop);
                    }
                }
            }
            else if (fx > 75 && fx <= 90)//垂直行走（大于75度小于等于90度）
            {
                if (targetPos.y - curPos.y > 0)//从上向下走
                {
                    //console.log("垂直行走，从上向下走！");
                    if (me.PersonDirection != "IconPerBefore") {
                        me.PersonDirection = "IconPerBefore";
                        this._marker.setIcon(me.IconPerBefore);
                    }
                }
                else//从下向上走
                {
                    //console.log("垂直行走，从下向上走！");
                    if (me.PersonDirection != "IconPerAfter") {
                        me.PersonDirection = "IconPerAfter";
                        this._marker.setIcon(me.IconPerAfter);
                    }
                }
            }
            return;

        },

        linePixellength: function (from, to) {
            return Math.sqrt(Math.abs(from.x - to.x) * Math.abs(from.x - to.x) + Math.abs(from.y - to.y) * Math.abs(from.y - to.y));

        },
        pointToPoint: function (from, to) {
            return Math.abs(from.x - to.x) * Math.abs(from.x - to.x) + Math.abs(from.y - to.y) * Math.abs(from.y - to.y)

        },
        /*
        * 移动到下一个点
        * param {Number} index 当前点的索引.
        * return 无返回值.
        */
        _moveNext: function (index) {
            var me = this;
            if (index < this._path.length - 1) {
                me._move(me._path[index].points, me._path[index + 1].points, me._path[index + 1].DisText, me._tween.linear);
            }
        },
        /*
        * 设置小车上方infowindow的内容，位置等
        * param {Point} pos 经纬度坐标点.
        * return 无返回值.
        */
        _setInfoWin: function (pos) {
            //设置上方overlay的position
            var me = this;
            if (!me._overlay) {
                return;
            }
            me._overlay.setPosition(pos, me._marker.getIcon().size);
            var index = me._troughPointIndex(pos);
            if (index != -1) {
                clearInterval(me._intervalFlag);
                me._overlay.setHtml(me._opts.landmarkPois[index].html);
                me._overlay.setPosition(pos, me._marker.getIcon().size);
                me._pauseForView(index);
            } else {
                me._overlay.setHtml(me._opts.defaultContent);
            }
        },
        _setInfoWinText: function (text) {
            //设置上方overlay的position
            var me = this;
            if (!me._overlay) {
                return;
            }
            me._overlay.setHtml(text);
        },
        /*
        * 在某个点暂停的时间
        * param {Number} index 点的索引.
        * return 无返回值.
        */
        _pauseForView: function (index) {
            var me = this;
            var t = setTimeout(function () {
                //运行下一个点
                me._moveNext(++me.i);
            }, me._opts.landmarkPois[index].pauseTime * 1000);
            me._setTimeoutQuene.push(t);
        },
        //清除暂停后再开始运行的timeout
        _clearTimeout: function () {
            for (var i in this._setTimeoutQuene) {
                clearTimeout(this._setTimeoutQuene[i]);
            }
            this._setTimeoutQuene.length = 0;
        },
        //缓动效果
        _tween: {
            //初始坐标，目标坐标，当前的步长，总的步长
            linear: function (initPos, targetPos, currentCount, count) {
                var b = initPos, c = targetPos - initPos, t = currentCount,
                d = count;
                return c * t / d + b;
            }
        },

        /*
        * 否经过某个点的index
        * param {Point} markerPoi 当前小车的坐标点.
        * return 无返回值.
        */
        _troughPointIndex: function (markerPoi) {
            //            var t = this._opts.landmarkPois, distance;
            //            for (var i = 0, len = t.length; i < len; i++) {
            //                //landmarkPois中的点没有出现过的话
            //                if (!t[i].bShow) {
            //                    distance = this._map.getDistance(new mxLib.Point(t[i].lng, t[i].lat), markerPoi);
            //                    //两点距离小于10米，认为是同一个点
            //                    if (distance < 10) {
            //                        t[i].bShow = true;
            //                        return i;
            //                    }
            //                }
            //            }
            return -1;
        }
    });
    /*
    * 自定义的overlay，显示在小车的上方
    * param {Point} Point 要定位的点.
    * param {String} html overlay中要显示的东西.
    * return 无返回值.
    */
    function CustomOverlay(point, html) {
        this._point = point;
        this._html = html;
    }
    CustomOverlay.prototype = new mxLib.Overlay();
    CustomOverlay.prototype.initialize = function (map) {
        var div = this._div = baidu.dom.create('div', { style: 'border:solid 1px #ccc;width:auto;min-width:50px;text-align:center;position:absolute;background:#fff;color:#000;font-size:12px;border-radius: 10px;padding:5px;white-space: nowrap;' });
        div.innerHTML = this._html;
        map.getPanes().floatPane.appendChild(div);
        this._map = map;
        return div;
    }
    CustomOverlay.prototype.draw = function () {
        this.setPosition(this.lushuMain._marker.getPosition(), this.lushuMain._marker.getIcon().size);
    }
    baidu.object.extend(CustomOverlay.prototype, {
        //设置overlay的position
        setPosition: function (poi, markerSize) {
            // 此处的bug已修复，感谢 苗冬(diligentcat@gmail.com) 的细心查看和认真指出
            var px = this._map.pointToPixel(poi.x, poi.y),
                styleW = baidu.dom.getStyle(this._div, 'width'),
                styleH = baidu.dom.getStyle(this._div, 'height');
            overlayW = parseInt(this._div.clientWidth || styleW, 10),
                overlayH = parseInt(this._div.clientHeight || styleH, 10);
            this._div.style.left = px.x - overlayW / 2 + 'px';
            this._div.style.bottom = -(px.y - markerSize.height) + 'px';
        },
        //设置overlay的内容
        setHtml: function (html) {
            this._div.innerHTML = html;
        },
        //跟customoverlay相关的实例的引用
        setRelatedClass: function (lushuMain) {
            this.lushuMain = lushuMain;
        }
    });
})();

var lushuArr = new Array();
var lushu;
//轨迹行走
/**
* 轨迹行走
* @class lushuStart 
* @param {personName} 默认显示信息（可不设置）  如：张三
* @param {personSpeed} 速度  单位：米/秒，值越大速度越快建议值:1-100间
* @param {tranlist} 测点信息(X坐标|Y坐标|人物头顶显示信息)多个用","分隔 如：388.26|489.3|姓名：张三</br>经过地点：地点1</br>时间：2015-09-09 00:08:00
,2943.26|224.3|姓名：张三</br>经过地点：地点2</br>时间：2015-09-09 00:15:00
* @returns 返回值：无
* @example 参考示例：
* lushuStart('张三','50','388.26|489.3|姓名：张三</br>经过地点：地点1</br>时间：2015-09-09 00:08:00
,2943.26|224.3|姓名：张三</br>经过地点：地点2</br>时间：2015-09-09 00:15:00');
*/
function lushuStart(personName, personSpeed, tranlist) {
    if (tranlist.length < 1) {
        mxLib.Util.toastInfo("未找到路径！", { delay: 8000 });
        return;
    }
    //对象转换
    var tranObj = [];
    var trans = tranlist.split(',');
    for (var i = 0; i < trans.length; i++) {
        var tranarr = trans[i].split('|');
        var obj = {};
        obj.points = new mxLib.Point(parseFloat(tranarr[0]), parseFloat(tranarr[1]));
        obj.DisText = tranarr[2];
        tranObj.push(obj);
    }
    if (tranObj.length > 0) {
        // 实例化一个驾车导航用来生成路线
        lushu = new mxLib.LuShu(map, tranObj, {
            defaultContent: personName,
            speed: parseInt(personSpeed),
            autoView: true,
            enableRotation: true
        });
        lushu.start();
        lushuArr.push(lushu);
    }
}

//鼠标中键按下开始，停止所有轨迹
map.addEventListener("dragstart", function () {
    //lushu.pause();
    for (var i = 0; i < lushuArr.length; i++) {
        lushuArr[i].pause();
    }
});
//鼠标中键按下结束，继续所有轨迹
map.addEventListener("dragend", function () {
    //lushu.repause();
    for (var i = 0; i < lushuArr.length; i++) {
        lushuArr[i].repause();
    }
});

////绑定事件
//$("run").onclick = function () {
//    lushu.start();
//}
//$("stop").onclick = function () {
//    lushu.stop();
//}
//$("pause").onclick = function () {
//    lushu.pause();
//}
//$("hide").onclick = function () {
//    lushu.hideInfoWindow();
//}
//$("show").onclick = function () {
//    lushu.showInfoWindow();
//}
//function $(element) {
//    return document.getElementById(element);
//}


