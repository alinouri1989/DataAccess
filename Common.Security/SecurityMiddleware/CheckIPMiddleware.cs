using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace San.CoreCommon.SecurityMiddleware
{
  public class CheckIPMiddleware
  {
    private readonly RequestDelegate _next;
    private List<string> _ips;

    public CheckIPMiddleware(RequestDelegate next, List<string> ips)
    {
      this._next = next;
      this._ips = ips;
    }

    public CheckIPMiddleware(RequestDelegate next, string ip)
    {
      this._next = next;
      this._ips = new List<string>();
      this._ips.Add(ip);
    }

    public async Task Invoke(HttpContext context)
    {
      StringValues forwardedIps;
      context.Request.Headers.TryGetValue("X-Forwarded-For", out forwardedIps);
      string ip = forwardedIps.Any<string>() ? forwardedIps.FirstOrDefault<string>() : context.Connection.RemoteIpAddress.ToString();
      if (ip == "::1")
        ip = "127.0.0.1";
      if (!this._ips.Any<string>((Func<string, bool>) (a => a == ip)))
      {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Invalid IP Address");
        forwardedIps = new StringValues();
      }
      else
      {
        await this._next(context);
        forwardedIps = new StringValues();
      }
    }
  }
}
