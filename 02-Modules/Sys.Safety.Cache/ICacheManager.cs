using System;
using System.Collections.Generic;

namespace Sys.Safety.Cache
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:缓存接口
    /// 修改记录
    /// 2017-02-22
    /// 2017-05-23
    /// </summary>
    public interface ICacheManager<T> where T:class
    {
        /// <summary>
        /// 加载缓存
        /// </summary>
        void Load();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="item"></param>
        void AddItem(T item);

        /// <summary>
        /// 批量添加缓存
        /// </summary>
        /// <param name="items"></param>
        void AddItems(List<T> items);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="item"></param>
        void DeleteItem(T item);

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="items"></param>
        void DeleteItems(List<T> items);

        /// <summary>
        /// 查询缓存列表
        /// </summary>
        /// <param name="predicate">查询条件表达式</param>
        /// <param name="isCopy">是否从缓存复制数据；True：复制（默认）；False：不复制</param>
        /// <returns></returns>
        List<T> Query(Func<T, bool> predicate, bool isCopy = true);

        /// <summary>
        /// 查询缓存列表
        /// </summary>
        /// <param name="isCopy">是否从缓存复制数据；True：复制（默认）；False：不复制</param>
        /// <returns></returns>
        List<T> Query(bool isCopy = true);

        /// <summary>
        /// 修改缓存
        /// </summary>       
        /// <param name="item"></param>
        void UpdateItem(T item);

        /// <summary>
        /// 批量修改缓存
        /// </summary>
        /// <param name="items"></param>
        void UpdateItems(List<T> items);
    }
}
