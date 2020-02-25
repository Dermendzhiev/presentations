namespace Authorization.Requirements
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    public class DomainRestrictedRequirement :
        AuthorizationHandler<DomainRestrictedRequirement, HubInvocationContext>,
        IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DomainRestrictedRequirement requirement,
            HubInvocationContext resource)
        {
            if (this.IsUserAllowedToDoThis(resource.HubMethodName, context.User.Identity.Name) &&
                context.User != null &&
                context.User.Identity != null)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool IsUserAllowedToDoThis(string hubMethodName,
            string currentUsername) => false;
    }
}
