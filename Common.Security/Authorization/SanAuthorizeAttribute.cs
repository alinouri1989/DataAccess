using System;

namespace Common.Security.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SanAuthorizeAttribute : FlagsAttribute
    {
        public string claimtype { get; set; }

        public string claimValue { get; set; }

        public SanAuthorizeAttribute(string Claimtype, string ClaimValue)
        {
            this.claimtype = Claimtype;
            this.claimValue = ClaimValue;
        }
    }
}
