namespace Common.Security.Security.Authorization
{
  public class IdentityConfiguration
  {
    public string PageTitle { get; set; }

    public string FaviconUri { get; set; }

    public string IdentityRedirectUri { get; set; }

    public string[] Scopes { get; set; }

    public string istrationRole { get; set; }

    public bool RequireHttpsMetadata { get; set; }

    public string IdentityCookieName { get; set; }

    public double IdentityCookieExpiresUtcHours { get; set; }

    public string TokenValidationClaimName { get; set; }

    public string TokenValidationClaimRole { get; set; }

    public string IdentityServerBaseUrl { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string OidcResponseType { get; set; }

    public bool HideUIForMSSqlErrorLogging { get; set; }

    public string Theme { get; set; }

    public string CustomThemeCss { get; set; }
  }
}
