using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace San.CoreCommon.SecurityMiddleware
{
  public static class CheckIPMiddlewareExtension
  {
    public static IApplicationBuilder UseCheckIP(
      this IApplicationBuilder app,
      List<string> ips)
    {
      return app.UseMiddleware<CheckIPMiddleware>((object) ips);
    }

    public static IApplicationBuilder UseCheckIP(
      this IApplicationBuilder app,
      string ip)
    {
      return app.UseMiddleware<CheckIPMiddleware>((object) ip);
    }
  }
}
