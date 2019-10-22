/*
* 所有js加载完毕，调用一些初始化数据的方法
*/

/*------------------------------通知窗体进行测点数据加载----------------------------------------*/
//设置图形的可编辑状态
/**
* 回调_设置图形的可编辑状态(用于在图形加载完成时回调界面设置图形的编辑状态)
* @class callOutCommand_SetMapEditState 
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "SetMapEditState":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "SetMapEditState") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "SetMapEditState") {
*          //这里进行实现
*      }
*   }
*/
mxLib.View.callOutCommand("SetMapEditState", "");
mxLib.View.callOutCommand("MapLoadEndEvent", "");
//加载测点
/**
* 回调_加载测点(用于在图形加载完成时回调界面来加载所有编排的测点信息)
* @class callOutCommand_LoadPoint 
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "LoadPoint":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "LoadPoint") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "LoadPoint") {
*          //这里进行实现
*      }
*   }
*/
mxLib.View.callOutCommand("LoadPoint", "");

//设置拓扑图自动加载井上设备
/**
* 回调_设置拓扑图自动加载井上设备(用于在拓扑图新建时，图形加载完成后回调界面加载所有默认的井上设备)
* @class callOutCommand_SetMapTopologyInit 
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "SetMapTopologyInit":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "SetMapTopologyInit") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "SetMapTopologyInit") {
*          //这里进行实现
*      }
*   }
*/
mxLib.View.callOutCommand("SetMapTopologyInit", "");


/*------------------------------通知窗体进行测点数据加载----------------------------------------*/

//------------------------------调用给所有图元加ToolTips----------------------------------------*/
addToolTips();
//------------------------------调用给所有图元加ToolTips----------------------------------------*/
//设置未编辑状态图元不能拖动
function SetMapdisableDragging() {
    if (!IsGraphicEdit) {
        var allOverlay = map.getOverlays();
        //设置所有图元均不能移动
        for (var i = 0; i < allOverlay.length; i++) {
            if (allOverlay[i] instanceof mxLib.RichMarker || allOverlay[i] instanceof mxLib.VectorLayer) {
                allOverlay[i].disableDragging();
            }
        }
    }
}
SetMapdisableDragging();

//添加右键菜单
/**
* 回调_添加右键菜单(用于在图形加载完成后回调界面加载所有右键菜单及切换页面列表)
* @class callOutCommand_AddMapRightMenu 
* @param 无
* @returns 无
* @example 参考示例：
*  c#：
*  private void mx_OnViewCallOutCommand(object sender, Axmetamap2dLib.IMetaMapX2DEvents_OnViewCallOutCommandEvent e)
*        {
*            string  Param = e.p_sParam;//参数
*            switch (e.p_sCmd)//命令
*            {
*                case "AddMapRightMenu":
*                //这里进行实现
*                break;
*            }
*        }
* IE：
* function MetaMapX::OnViewCallOutCommand (viewId, cmd, param) {
*      if (cmd == "AddMapRightMenu") {
*          //这里进行实现
*      }
*   }
* google:
*  MetaMapX.OnViewCallOutCommand = function(viewId, cmd, param) {
*      if (cmd == "AddMapRightMenu") {
*          //这里进行实现
*      }
*   }
*/
mxLib.View.callOutCommand("AddMapRightMenu", "");

//如果是运行状态则启动图形刷新
function AutoRefPointSsz() {
    if (!IsGraphicEdit) {
        window.setTimeout("DoRefPointSsz()", 3000);
    }
}
AutoRefPointSsz();

