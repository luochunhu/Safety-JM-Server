using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public enum StationUpdateState
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        error = 0,
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        unKnowm = 1,
        /// <summary>
        /// 请求升级中
        /// </summary>
        [Description("请求升级")]
        requesting = 2,
        /// <summary>
        /// 请求升级成功
        /// </summary>
        [Description("请求升级成功")]
        requestSuccess = 3,
        /// <summary>
        /// 请求升级失败
        /// </summary>
        [Description("请求升级失败")]
        requestFailure = 4,
        /// <summary>
        /// 等待文件下发
        /// </summary>
        [Description("等待文件下发")]
        waitingFile = 5,
        /// <summary>
        /// 文件接收中
        /// </summary>
        [Description("文件接收中")]
        recivingFile = 6,
        /// <summary>
        /// 文件接收完成
        /// </summary>
        [Description("文件接收完成")]
        reciveComplete = 7,
        /// <summary>
        /// 重启升级中
        /// </summary>
        [Description("重启升级中")]
        restart = 8,
        /// <summary>
        /// 重启升级成功
        /// </summary>
        [Description("重启升级成功")]
        restartSuccess = 9,
        /// <summary>
        /// 升级完成
        /// </summary>
        [Description("升级完成")]
        updateComplete = 10,
        /// <summary>
        /// 取消升级中
        /// </summary>
        [Description("取消升级中")]
        updateCancle = 11,
        /// <summary>
        /// 取消升级成功
        /// </summary>
        [Description("取消升级成功")]
        updateCancleSuccess = 12,
        /// <summary>
        /// 取消升级失败
        /// </summary>
        [Description("取消升级失败")]
        updateCancleFailure = 13,
        /// <summary>
        /// 恢复备份中
        /// </summary>
        [Description("恢复备份中")]
        restore = 14,
        /// <summary>
        /// 恢复备份成功
        /// </summary>
        [Description("恢复备份成功")]
        restoreSuccess = 15,
        /// <summary>
        /// 获取版本信息
        /// </summary>
        [Description("获取版本信息")]
        getVersion = 16,
        /// <summary>
        /// 获取版本信息成功
        /// </summary>
        [Description("获取版本信息成功")]
        getVersionSuccess = 17,
        /// <summary>
        /// 巡检接收情况
        /// </summary>
        [Description("巡检接收情况")]
        reiciveCheck = 18,
        /// <summary>
        /// 补发
        /// </summary>
        [Description("补发")]
        replacement = 19,
        /// <summary>
        /// 升级文件缺失
        /// </summary>
        [Description("升级文件缺失")]
        fileMissing = 20,
        /// <summary>
        /// 分站状态正常
        /// </summary>
        [Description("分站状态正常")]
        normal = 21,
        /// <summary>
        /// 补发完成
        /// </summary>
        [Description("补发完成")]
        replacComplete = 22
    }
}
