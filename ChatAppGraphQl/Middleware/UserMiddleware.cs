using ChatAppGraphQl.Authentication;
using HotChocolate.Resolvers;
using System.Security.Claims;

namespace ChatAppGraphQl.Middleware {
    public class UserMiddleware {
        public const string USER_CONTEXT_DATA_KEY = "User";

        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next) {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context) {
            if (context.ContextData.TryGetValue("ClaimsPrincipal", out object? rawClaimsPrincipal)
                && rawClaimsPrincipal != null && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal) {
                string? firebaseId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);
                if (firebaseId != null) {
                    context.ContextData.Add(USER_CONTEXT_DATA_KEY, firebaseId);
                }
            }

            await _next(context);
        }
    }
}
