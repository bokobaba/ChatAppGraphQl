
using Microsoft.AspNetCore.Authorization;

namespace ChatAppGraphQl.Authentication {
    public class AdminHandler : AuthorizationHandler<IsAdmin> {
        private const string ADMIN_EMAIL = "kalledemos13@gmail.com";
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            IsAdmin requirement) {
            var claim = context.User.Claims.First(c => c.Type == FirebaseUserClaimType.EMAIL);

            if (claim is null)
                return Task.CompletedTask;

            string email = claim.Value;

            if (email == ADMIN_EMAIL)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
