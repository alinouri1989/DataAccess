﻿using System.Collections.Generic;

namespace DataAccess.Repository.Cache
{
  public static class StatiCachInst
  {
    private static List<object> StaticCacheSetting = new List<object>();

    public static void add(object staticCache) => StatiCachInst.StaticCacheSetting.Add(staticCache);

    public static List<object> Get() => StatiCachInst.StaticCacheSetting;
  }
}
