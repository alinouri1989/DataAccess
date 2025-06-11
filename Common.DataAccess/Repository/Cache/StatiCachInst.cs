using System.Collections.Generic;

namespace Common.DataAccess.Repository.Cache
{
    public static class StatiCachInst
    {
        private static List<object> StaticCacheSetting = new List<object>();

        public static void add(object staticCache) => StaticCacheSetting.Add(staticCache);

        public static List<object> Get() => StaticCacheSetting;
    }
}
