﻿using Common.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Common.Security.Authorization
{
    public class ClaimBaseAuthorizeFilter : IAsyncAuthorizationFilter, IFilterMetadata
    {
        private string ClaimType = string.Empty;
        private string ClaimValue = string.Empty;

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!this.IsProtectedAction(context))
                return Task.CompletedTask;
            SanAuthorizeAttribute authorizeAttribute = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes<SanAuthorizeAttribute>().FirstOrDefault<SanAuthorizeAttribute>();
            if (authorizeAttribute == null)
            {
                context.Result = new ForbidResult();
                return Task.CompletedTask;
            }
            this.ClaimType = authorizeAttribute.claimtype;
            this.ClaimValue = authorizeAttribute.claimValue;
            ClaimsPrincipal user = context.HttpContext.User;
            if (user != null && (user.IsInRole("SuperAdmin") || user.WithClaim(this.ClaimValue, this.ClaimType)))
                return Task.CompletedTask;
            context.Result = new ForbidResult();
            return Task.CompletedTask;
        }

        private bool IsProtectedAction(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return false;
            ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            TypeInfo controllerTypeInfo = actionDescriptor.ControllerTypeInfo;
            MethodInfo methodInfo = actionDescriptor.MethodInfo;
            AuthorizeAttribute customAttribute = controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
            controllerTypeInfo.GetCustomAttribute<SanAuthorizeAttribute>();
            return customAttribute != null || methodInfo.GetCustomAttribute<AuthorizeAttribute>() != null || methodInfo.GetCustomAttribute<SanAuthorizeAttribute>() != null;
        }
    }
}
