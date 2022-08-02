using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Common.Security.Security.Authorization
{
  public static class AddAuthorizationPolicies
  {
    public static IServiceCollection AddAuthorizationPolicy(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      IdentityConfiguration _identityConfiguration = configuration.GetSection("IdentityConfiguration").Get<IdentityConfiguration>();
      services.Configure<CookiePolicyOptions>((Action<CookiePolicyOptions>) (options =>
      {
        options.MinimumSameSitePolicy = SameSiteMode.None;
        options.Secure = CookieSecurePolicy.SameAsRequest;
        options.OnAppendCookie = (Action<AppendCookieContext>) (cookieContext => AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions));
        options.OnDeleteCookie = (Action<DeleteCookieContext>) (cookieContext => AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions));
      }));
      services.AddAuthentication((Action<AuthenticationOptions>) (options =>
      {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultAuthenticateScheme = "Cookies";
        options.DefaultForbidScheme = "Cookies";
        options.DefaultSignInScheme = "Cookies";
        options.DefaultSignOutScheme = "Cookies";
      })).AddCookie("Cookies", (Action<CookieAuthenticationOptions>) (options => options.Cookie.Name = _identityConfiguration.IdentityCookieName)).AddOpenIdConnect("oidc", (Action<OpenIdConnectOptions>) (options =>
      {
        options.Authority = _identityConfiguration.IdentityServerBaseUrl;
        options.RequireHttpsMetadata = _identityConfiguration.RequireHttpsMetadata;
        options.ClientId = _identityConfiguration.ClientId;
        options.ClientSecret = _identityConfiguration.ClientSecret;
        options.ResponseType = _identityConfiguration.OidcResponseType;
        options.Scope.Clear();
        foreach (string scope in _identityConfiguration.Scopes)
          options.Scope.Add(scope);
        options.ClaimActions.MapJsonKey(_identityConfiguration.TokenValidationClaimRole, _identityConfiguration.TokenValidationClaimRole, _identityConfiguration.TokenValidationClaimRole);
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          NameClaimType = _identityConfiguration.TokenValidationClaimName,
          RoleClaimType = _identityConfiguration.TokenValidationClaimRole
        };
      }));
      return services;
    }

    public static IServiceCollection AddBearerAuthorizationPolicy(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      IdentityConfiguration _identityConfiguration = configuration.GetSection("IdentityConfiguration").Get<IdentityConfiguration>();
      services.AddAuthentication("Bearer").AddJwtBearer("Bearer", (Action<JwtBearerOptions>) (options =>
      {
        options.Authority = _identityConfiguration.IdentityServerBaseUrl;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateAudience = false
        };
      }));
      return services;
    }
  }
}
