// Decompiled with JetBrains decompiler
// Type: Repository.Cache.CachProviderConfig
// Assembly: DataAccess, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6804894C-F989-4432-B8EA-6F3F70ACE424
// Assembly location: C:\Users\sa.nori\AppData\Local\Temp\Riqygac\68676b643f\lib\net5.0\DataAccess.dll

using Microsoft.Extensions.Configuration;
using System;

namespace Repository.Cache
{
  public class CachProviderConfig
  {
    private CachProviderConfig.ProviderEnum _Provider = CachProviderConfig.ProviderEnum.memoryCach;

    public CachProviderConfig.ProviderEnum Provider
    {
      get => this._Provider;
      set => this._Provider = value;
    }

    public CachProviderConfig(IConfiguration config)
    {
      string provider = "memoryCache";
      if (config.GetSection("CacheProvider").Exists() && config.GetSection("CacheProvider").GetSection("provider").Exists())
        provider = config.GetSection("CacheProvider").GetSection("provider").Value;
      this.SetProvider(provider);
    }

    private void SetProvider(string provider)
    {
      CachProviderConfig.ProviderEnum result = CachProviderConfig.ProviderEnum.memoryCach;
      Enum.TryParse<CachProviderConfig.ProviderEnum>(provider, out result);
      this.Provider = result;
    }

    public enum ProviderEnum
    {
      memoryCach,
      easyCaching,
      disttibutedCaching,
    }
  }
}
