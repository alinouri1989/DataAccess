using Microsoft.Extensions.Configuration;
using System;

namespace Common.DataAccess.Repository.Cache
{
  public class CachProviderConfig
  {
    private ProviderEnum _Provider = ProviderEnum.memoryCach;

    public ProviderEnum Provider
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
            ProviderEnum result = ProviderEnum.memoryCach;
      Enum.TryParse(provider, out result);
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
