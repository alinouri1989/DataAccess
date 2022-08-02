// Decompiled with JetBrains decompiler
// Type: Repository.Cache.StatiCachInst
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using System.Collections.Generic;

namespace Repository.Cache
{
  public static class StatiCachInst
  {
    private static List<object> StaticCacheSetting = new List<object>();

    public static void add(object staticCache) => StatiCachInst.StaticCacheSetting.Add(staticCache);

    public static List<object> Get() => StatiCachInst.StaticCacheSetting;
  }
}
