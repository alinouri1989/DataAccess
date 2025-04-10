﻿using Microsoft.AspNetCore.Http;

namespace Common.Security.Authorization
{
    public static class AuthenticationHelpers
    {
        public static void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite != SameSiteMode.None)
                return;
            string userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            if (!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
                options.SameSite = SameSiteMode.None;
        }

        public static bool DisallowsSameSiteNone(string userAgent) => userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12") || userAgent.Contains("Macintosh; Intel Mac OS X 10_14") && userAgent.Contains("Version/") && userAgent.Contains("Safari") || userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6");
    }
}
