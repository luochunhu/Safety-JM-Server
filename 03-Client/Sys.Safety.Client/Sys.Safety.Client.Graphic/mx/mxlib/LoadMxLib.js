
//样式
//linkcss(scriptBaseDir + "css/easyui.css");
linkcss(scriptBaseDir + "css/map.css");

//linkcss(scriptBaseDir + "mxlib/css/ext-all.css");
linkcss(scriptBaseDir + "mxlib/css/button.css");
linkcss(scriptBaseDir + "css/GraphStyle.css");



//脚本
//第三方库
//include(scriptBaseDir + "mxlib/javascript/ext-base.js");
//include(scriptBaseDir + "mxlib/javascript/ext-all.js");

include(scriptBaseDir + "mxlib/jquery-1.11.2.min.js");
include(scriptBaseDir + "mxlib/snap.svg-min.js");
//include(scriptBaseDir + "mxlib/jquery.easyui.min.js");


//元图地图库
include(scriptBaseDir + "mxlib/api.min.js");

////二次开发入口文件 加载完成了dom后
//domLoadedInclude(scriptBaseDir + "Basic.GraphGIS.min.js");

//二次开发入口文件 加载完成了dom后
domLoadedInclude(scriptBaseDir + "main.js");

//地图右键菜单加载
domLoadedInclude(scriptBaseDir + "RightMenu.js");

//加载测点编排js
domLoadedInclude(scriptBaseDir + "RichMarkerDef.js");

//区域定义 js
domLoadedInclude(scriptBaseDir + "AreaDef.js");

//加载测点显示、隐藏，实时刷新js
domLoadedInclude(scriptBaseDir + "PointRef.js");

//轨迹绘制、巷道拓扑图 js
domLoadedInclude(scriptBaseDir + "TrajectoryDef.js");

//人员轨迹行走 js
domLoadedInclude(scriptBaseDir + "LuShu.js");

//拓扑图测点编排js
domLoadedInclude(scriptBaseDir + "TopologyDef.js");

//拓扑图绘制 js
domLoadedInclude(scriptBaseDir + "TopologyTransDef.js");

//svg动画 js
domLoadedInclude(scriptBaseDir + "SvgAnimation.js");

//测量命令 js
domLoadedInclude(scriptBaseDir + "measure.js");

//图元测试所有按钮
//domLoadedInclude(scriptBaseDir + "TestButton.js");

////所有js加载完毕，进行数据初始化
domLoadedInclude(scriptBaseDir + "LoadEnd.js");



