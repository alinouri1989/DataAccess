using Asp.Versioning;
using Common.Constants;
using Common.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Common.ExtensionMethods
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal identity) => Guid.Parse(identity?.FindFirst(CustomClaimTypes.userId).Value);

        public static bool WithClaim(
          this ClaimsPrincipal identity,
          string ClaimValue,
          string ClaimType)
        {
            return identity.Claims.Any<Claim>((Func<Claim, bool>)(a => a.Value == ClaimValue));
        }

        public static string GetAppName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return (identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "AppName")?.Value;
        }


        public static string GetAppName(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.FindFirst(c => c.Type == "AppName")?.Value;
        }
        public static string GetUserName(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return (identity as ClaimsIdentity).Claims.FirstOrDefault(c => c.Type == "name")?.Value;
        }


        public static string GetUserName(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.FindFirst(c => c.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
        }
        public static string GetMobilePhone(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.FindFirst(c => c.Type == ClaimTypes.MobilePhone)?.Value;
        }

        public static string GetUserNationalCode(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user.FindFirst(c => c.Type == "Identifier")?.Value;
        }

        public static string GetUserNationalCode(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return (identity as ClaimsIdentity)?.FindFirst(c => c.Type == "Identifier")?.Value;
        }

        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                string text = claimsIdentity.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (text != null)
                {
                    return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return default;
        }

        public static T GetUserId<T>(this ClaimsPrincipal user) where T : IConvertible
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (user != null)
            {
                string text = user.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (text != null)
                {
                    return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
                }
            }

            return default;
        }
        public static string GetUserId(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return (identity as ClaimsIdentity)?.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        }

        public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            return identity.FindFirst(claimType)?.Value;
        }
        public static IServiceCollection RegisterIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            //services.RegisterApiVersion();
            services.RegisterApiAuthentication(configuration);
            return services;
        }

        public static IServiceCollection RegisterApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver")
                );
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection RegisterApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.Authority = jwtConfig["validIssuer"];
                //options.TokenValidationParameters = new TokenValidationParameters
                //{
                //    ValidateAudience = false
                //};
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
            return services;
        }

        #region WebApp Identity

        /// <summary>
        /// برای احراز هویت به صورت oidc
        /// معمولا در وب اپ استفاده می شود
        /// </summary>
        /// <param name="services"></param>
        /// <param name="adminConfiguration"></param>
        /// <returns></returns>
        public static IServiceCollection RegsiterIdentityServer(this IServiceCollection services, AdminConfiguration adminConfiguration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = AuthenticationConsts.OidcAuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                 options =>
                 {
                     options.Cookie.Name = adminConfiguration.IdentityAdminCookieName;
                 })
             .AddOpenIdConnect(AuthenticationConsts.OidcAuthenticationScheme, options =>
             {
                 options.Authority = adminConfiguration.IdentityServerBaseUrl;
                 options.RequireHttpsMetadata = adminConfiguration.RequireHttpsMetadata;
                 options.ClientId = adminConfiguration.ClientId;
                 options.ClientSecret = adminConfiguration.ClientSecret;
                 options.ResponseType = adminConfiguration.OidcResponseType;

                 options.Scope.Clear();
                 foreach (var scope in adminConfiguration.Scopes)
                 {
                     options.Scope.Add(scope);
                 }

                 options.ClaimActions.MapJsonKey(adminConfiguration.TokenValidationClaimRole,
                     adminConfiguration.TokenValidationClaimRole,
                     adminConfiguration.TokenValidationClaimRole);

                 options.SaveTokens = true;
                 //options.SaveTokens = false;

                 options.GetClaimsFromUserInfoEndpoint = true;

                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     NameClaimType = adminConfiguration.TokenValidationClaimName,
                     RoleClaimType = adminConfiguration.TokenValidationClaimRole
                 };

                 options.Events = new OpenIdConnectEvents
                 {
                     OnMessageReceived = context => OnMessageReceived(context, adminConfiguration),
                     OnRedirectToIdentityProvider = context => OnRedirectToIdentityProvider(context, adminConfiguration)
                 };
             });
            return services;
        }

        private static Task OnMessageReceived(MessageReceivedContext context, AdminConfiguration adminConfiguration)
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(adminConfiguration.IdentityAdminCookieExpiresUtcHours));

            return Task.FromResult(0);
        }

        private static Task OnRedirectToIdentityProvider(RedirectContext n, AdminConfiguration adminConfiguration)
        {
            n.ProtocolMessage.RedirectUri = adminConfiguration.IdentityAdminRedirectUri;

            return Task.FromResult(0);
        }
        #endregion
    }
}
