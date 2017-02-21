
using System;
using System.Runtime.Caching;
using Common.Logging;

namespace Kraken.Core
{
    public abstract class BaseMemoryCache<TKey, TItem> where TItem : class
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger< BaseMemoryCache<TKey, TItem>>();
        private ObjectCache _cache;
        readonly CacheItemPolicy _policy;

        #endregion

        #region Properties

        private static string CacheName
        {
            get { return string.Format("{0}MemoryCache", typeof(TItem).Name); }
        }
        #endregion

        #region Ctor
        protected BaseMemoryCache()
        {
            _cache = GetNewCache();
            _policy = new CacheItemPolicy();
        }
        #endregion

        #region Methods
        public TItem Get(TKey key)
        {
            TItem cacheItem = _cache.Get(KeyAsCacheKey(key)) as TItem;
            return cacheItem;
        }

        public TItem Get(TKey key, Func<TKey, TItem> nonCacheFallback)
        {
            var item = Get(key);
            if (item == null)
            {
                item = nonCacheFallback(key);

                // Cannot add null to the cache
                if (item != null)
                {
                    Add(key, item);
                }
            }
            return item;
        }

        public void Add(TKey key, TItem itemToCache)
        {
            CacheItem cacheItem = new CacheItem(KeyAsCacheKey(key), itemToCache);
            _cache.Add(cacheItem, _policy);
        }

        public void Remove(TKey key)
        {
            _cache.Remove(KeyAsCacheKey(key));
        }

        protected virtual string KeyAsCacheKey(TKey key)
        {
            return key.ToString();
        }

        public void Flush()
        {
            Log.Info(m=>m("{0} flushing", CacheName));
            MemoryCache oldCache = (MemoryCache)_cache;
            _cache = GetNewCache();
            oldCache.Dispose();
        }

        private static MemoryCache GetNewCache()
        {
            return new MemoryCache(CacheName);
        }
        #endregion
    }
}