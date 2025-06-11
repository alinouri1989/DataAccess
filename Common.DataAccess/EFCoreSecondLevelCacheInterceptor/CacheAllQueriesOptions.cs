using System;

namespace Common.DataAccess.EFCoreSecondLevelCacheInterceptor;

/// <summary>
///     CacheAllQueries Options
/// </summary>
public class CacheAllQueriesOptions
{
    /// <summary>
    ///     Defines the expiration mode of the cache item.
    /// </summary>
    public CacheExpirationMode ExpirationMode { set; get; }

    /// <summary>
    ///     The expiration timeout.
    /// </summary>
    public TimeSpan? Timeout { set; get; }

    /// <summary>
    ///     Enables or disables the `CacheAllQueries` feature.
    /// </summary>
    public bool IsActive { set; get; }
}