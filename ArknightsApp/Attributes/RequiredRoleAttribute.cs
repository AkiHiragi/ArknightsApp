using System.Security.Claims;
using ArknightsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArknightsApp.Attributes;

public class RequiredRoleAttribute:Attribute, IAuthorizationFilter
{
    private readonly UserRole _requiredRole;

    public RequiredRoleAttribute(UserRole requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var  user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
        if (!Enum.TryParse<UserRole>(userRoleClaim, out var userRole))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (userRole < _requiredRole)
        {
            context.Result = new ForbidResult();
        }
    }
}