using Basic.Framework.Logging;
using Sys.Safety.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sys.Safety.Cache.Safety
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-22
    /// 描述:KJ73N缓存基类
    /// 修改记录
    /// 2017-05-22
    /// 2017-05-23
    /// 2017-05-24
    /// </summary>
    /// <typeparam name="TEntity">DataContract</typeparam>
    public abstract class KJ73NCacheManager<TEntity> : ICacheManager<TEntity> where TEntity : class
    {
        /// <summary>
        /// 缓存列表
        /// </summary>
        protected List<TEntity> _cache = new List<TEntity>();

        /// <summary>
        /// 读写标记锁
        /// </summary>
        protected ReaderWriterLock _rwLocker = new ReaderWriterLock();

        /// <summary>
        /// 缓存多线程创建单例锁
        /// </summary>
        protected static readonly object obj = new object();

        /// <summary>
        /// 缓存名称
        /// </summary>
        protected string CacheName
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        public abstract void Load();

        /// <summary>
        /// 添加缓存至列表
        /// </summary>
        /// <param name="item"></param>
        protected abstract void AddEntityToCache(TEntity item);

        /// <summary>
        /// 从列表删除缓存
        /// </summary>
        /// <param name="item"></param>
        protected abstract void DeleteEntityFromCache(TEntity item);

        /// <summary>
        /// 修改缓存至列表
        /// </summary>
        /// <param name="item"></param>
        protected abstract void UpdateEntityToCache(TEntity item);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(TEntity item)
        {
            if (item != null)
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    AddEntityToCache(item);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:添加缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// 批量添加缓存
        /// </summary>
        /// <param name="items"></param>
        public void AddItems(List<TEntity> items)
        {
            if (items != null && items.Any())
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    items.ForEach(item => AddEntityToCache(item));
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:批量添加缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="item"></param>
        public void DeleteItem(TEntity item)
        {
            if (item != null)
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    DeleteEntityFromCache(item);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:删除缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <param name="items"></param>
        public void DeleteItems(List<TEntity> items)
        {
            if (items != null && items.Any())
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    items.ForEach(item => DeleteEntityFromCache(item));
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:批量删除缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// 根据条件获取缓存
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="isCopy">是否从缓存复制数据；True：复制（默认）；False：不复制</param>
        /// <returns></returns>
        public List<TEntity> Query(Func<TEntity, bool> predicate,bool isCopy = true)
        {
            _rwLocker.AcquireReaderLock(-1);
            List<TEntity> result;
            try
            {
                //modified by  20170719
                //根据参数是否需要复制（默认复制）

                result = _cache.Where(predicate).ToList();
                if (result == null)
                {
                    result = new List<TEntity>();
                }
                else if (isCopy && typeof(TEntity) != typeof(Jc_DefInfo) && typeof(TEntity) != typeof(Jc_DevInfo) && typeof(TEntity) != typeof(R_PersoninfInfo) && typeof(TEntity) != typeof(R_PrealInfo))
                //else if (isCopy)//测点定义和设备类型定义取消缓存深复制  20170814 人员档案和实时表取消深复制  20171124
                {              
                    result = Basic.Framework.Common.ObjectConverter.DeepCopy<List<TEntity>>(result);
                    //Framework.Common.JSONHelper.ParseJSONString<List<TEntity>>(Framework.Common.JSONHelper.ToJSONString(result));
                }                
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("{0}:获取缓存失败！" + "\r\n" + ex, CacheName));
                result = new List<TEntity>();
            }
            finally
            {
                _rwLocker.ReleaseReaderLock();
            }
            return result;
        }

        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        public List<TEntity> Query(bool isCopy = true)
        {
            _rwLocker.AcquireReaderLock(-1);
            List<TEntity> result;
            try
            {
                //modified by  20170719
                //根据参数是否需要复制（默认复制）
                result = _cache;
                if (isCopy && typeof(TEntity) != typeof(Jc_DefInfo) && typeof(TEntity) != typeof(Jc_DevInfo) && typeof(TEntity) != typeof(R_PersoninfInfo) && typeof(TEntity) != typeof(R_PrealInfo))
                //if (isCopy)//测点定义和设备类型定义取消缓存深复制  20170814 人员档案和实时表取消深复制  20171124
                {
                    result = Basic.Framework.Common.ObjectConverter.DeepCopy<List<TEntity>>(result);
                }
               // result = Framework.Common.JSONHelper.ParseJSONString<List<TEntity>>(Framework.Common.JSONHelper.ToJSONString(_cache));
            }
            catch (Exception ex)
            {
                LogHelper.Error(string.Format("{0}:获取所有缓存失败！" + "\r\n" + ex, CacheName));
                result = new List<TEntity>();
            }
            finally
            {
                _rwLocker.ReleaseReaderLock();
            }
            return result;
        }

        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="item"></param>
        public void UpdateItem(TEntity item)
        {
            if (item != null)
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    UpdateEntityToCache(item);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:更新缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }

        /// <summary>
        /// 批量更新缓存
        /// </summary>
        /// <param name="items"></param>
        public void UpdateItems(List<TEntity> items)
        {
            if (items != null && items.Any())
            {
                _rwLocker.AcquireWriterLock(-1);
                try
                {
                    items.ForEach(item => UpdateEntityToCache(item));
                }
                catch (Exception ex)
                {
                    LogHelper.Error(string.Format("{0}:批量更新缓存失败！" + "\r\n" + ex.Message, CacheName));
                }
                finally
                {
                    _rwLocker.ReleaseWriterLock();
                }
            }
        }
    }
}
