using EFCoreSecondLevelCacheInterceptor;
using Quartz;
using System;
using System.Linq.Expressions;

namespace Repository.Cache
{
    public class StaticCacheSetting<T>
    {
        public CacheExpirationMode expirationMode { get; set; }

        public TimeSpan timeout { get; set; }

        public Expression<Func<T, T>> cacheSelector { get; set; }

        public Expression<Func<T, bool>> cacheFilter { get; set; }

        public Action<SimpleScheduleBuilder> Schedule { get; set; }

        public string TypeName { get; set; }
    }

    public class StaticCacheSetting<T, TResult>
    {
        public CacheExpirationMode expirationMode { get; set; }

        public TimeSpan timeout { get; set; }

        public Expression<Func<T, TResult>> cacheSelector { get; set; }

        public Expression<Func<T, bool>> cacheFilter { get; set; }

        public Action<SimpleScheduleBuilder> Schedule { get; set; }

        public string TypeName { get; set; }
    }
}
