using System;
using System.Linq;
using System.Security.Claims;

namespace Common.Security.Authorization
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
    }
}
