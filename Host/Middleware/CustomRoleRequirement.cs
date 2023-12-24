using Microsoft.AspNetCore.Authorization;

namespace OggettoCase.Middleware;

public class CustomRoleRequirement : AuthorizationHandler<CustomRoleRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRoleRequirement requirement)
    {
        var roles = new[] { "notApproved"}; 
        var userIsInRole = roles.Any(role => context.User.IsInRole(role));
        if (userIsInRole)
        {
            context.Fail();
            return Task.FromResult(false);
        }

        context.Succeed(requirement);
        return Task.FromResult(true);
    }
}