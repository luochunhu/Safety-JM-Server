/*
* 地图右键菜单管理
*/
var MapcontextMenu = new mxLib.ContextMenu();

//添加地图右键菜单
/**
* 添加地图右键菜单
* @class AddMapRightMenu 
* @param {pageList} 切换的图形列表,多个之间用“|”分隔 如：图形1|图形2
* @returns 返回值：无
* @example 参考示例：
* AddMapRightMenu('图形1|图形2');
*/
function AddMapRightMenu(pageList) {
    try {
        map.removeContextMenu(MapcontextMenu);

        var menu_ZoomOut = new mxLib.MenuItem("放大图形", scriptBaseDir + "icon/zoomin_16x16.png", "", function () { map.zoomIn(); });
        var menu_ZoomIn = new mxLib.MenuItem("缩小图形", scriptBaseDir + "icon/zoomout_16x16.png", "", function () { map.setZoom(map.getZoom() - 1); });
        //    var menu_ZoomIn1 = new mxLib.MenuItem("缩小一级", "", "缩小图形", function () { map.setZoom(map.getZoom() - 1); });
        //    var menu_ZoomIn2 = new mxLib.MenuItem("缩小二级", "", "缩小图形", function () { map.setZoom(map.getZoom() - 2); });
        var menu_zoomExtent = new mxLib.MenuItem("缩放图形", scriptBaseDir + "icon/zoom100_16x16.png", "", function () { map.zoomExtent(); });
        var menu_pagesChg = new mxLib.MenuItem("页面切换", scriptBaseDir + "icon/open_16x16.png", "", function () {

        });
        var pages = pageList.split('|');
        for (var i = 0; i < pages.length; i++) {
            var PageName = pages[i];
            var menu_temp = new mxLib.MenuItem(PageName, scriptBaseDir + "icon/next_16x16.png", "页面切换", function (e) {
                /**
                * 回调_页面切换功能(用于回调界面对选择的页面进行重新加载)
                * @class callOutCommand_PageChange 
                * @param 无
                * @returns 无
                * @example 参考示例：
                *  c#：
                *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
                *        {
                *            string  Param = e.p_sParam;//参数
                *            switch (e.p_sCmd)//命令
                *            {
                *                case "PageChange":
                *                //这里进行实现
                *                break;
                *            }
                *        }
                * IE：
                * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
                *      if (cmd == "PageChange") {
                *          //这里进行实现
                *      }
                *   }
                * google:
                *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
                *      if (cmd == "PageChange") {
                *          //这里进行实现
                *      }
                *   }
                */
                mxLib.View.callOutCommand("PageChange", e.currentTarget.innerText);
            });
            MapcontextMenu.appendItem(menu_temp);
        }

        var menu_pagePrint = new mxLib.MenuItem("打印图形", scriptBaseDir + "icon/print_16x16.png", "", function () {
            mappaintPicture(true);
        });
        var menu_pageToImg = new mxLib.MenuItem("导出图片", scriptBaseDir + "icon/importimage_16x16.png", "", function () {
            /**
            * 回调_导出图片功能(用于回调界面弹出图片的保存路径选择并保存图片)
            * @class callOutCommand_pageToImg
            * @param 无
            * @returns 无
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "pageToImg":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "pageToImg") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "pageToImg") {
            *          //这里进行实现
            *      }
            *   }
            */
            mxLib.View.callOutCommand("pageToImg", "");
        });
        var menu_layerDis = new mxLib.MenuItem("图层显示/隐藏", scriptBaseDir + "icon/find_16x16.png", "", function () {
            /**
            * 回调_图层显示/隐藏(用于回调界面弹出图层的显示/隐藏选择界面进行设备显示/隐藏操作)
            * @class callOutCommand_layerDis
            * @param 无
            * @returns 无
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "layerDis":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "layerDis") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "layerDis") {
            *          //这里进行实现
            *      }
            *   }
            */
            mxLib.View.callOutCommand("layerDis", "");
        });
        var menu_pointDis = new mxLib.MenuItem("设备显示/隐藏", scriptBaseDir + "icon/findbyid_16x16.png", "", function () {
            /**
            * 回调_设备显示/隐藏(用于回调界面弹出设备的显示/隐藏选择界面进行图层显示/隐藏操作)
            * @class callOutCommand_pointDis
            * @param 无
            * @returns 无
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "pointDis":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "pointDis") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "pointDis") {
            *          //这里进行实现
            *      }
            *   }
            */
            mxLib.View.callOutCommand("pointDis", "");
        });
        var menu_pointSercah = new mxLib.MenuItem("设备查找", scriptBaseDir + "icon/show_16x16.png", "", function () {
            /**
            * 回调_设备查找(用于回调界面弹出设备的查找选择界面进行设备查找操作)
            * @class callOutCommand_pointSercah
            * @param 无
            * @returns 无
            * @example 参考示例：
            *  c#：
            *  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
            *        {
            *            string  Param = e.p_sParam;//参数
            *            switch (e.p_sCmd)//命令
            *            {
            *                case "pointSercah":
            *                //这里进行实现
            *                break;
            *            }
            *        }
            * IE：
            * function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
            *      if (cmd == "pointSercah") {
            *          //这里进行实现
            *      }
            *   }
            * google:
            *  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
            *      if (cmd == "pointSercah") {
            *          //这里进行实现
            *      }
            *   }
            */
            mxLib.View.callOutCommand("pointSercah", "");
        });

        MapcontextMenu.appendItem(menu_ZoomOut);
        MapcontextMenu.appendItem(menu_ZoomIn);
        //    MapcontextMenu.appendItem(menu_ZoomIn1);
        //    MapcontextMenu.appendItem(menu_ZoomIn2);
        MapcontextMenu.appendItem(menu_zoomExtent);
        MapcontextMenu.appendItem(menu_pagesChg);
        MapcontextMenu.appendItem(menu_pagePrint);
        MapcontextMenu.appendItem(menu_pageToImg);
        MapcontextMenu.appendItem(menu_layerDis);
        MapcontextMenu.appendItem(menu_pointDis);
        MapcontextMenu.appendItem(menu_pointSercah);

        map.addContextMenu(MapcontextMenu);
    }
    catch (e) {
        console.log(e);
    }
}

