using Microsoft.AspNetCore.Authorization;

namespace ChatAppGraphQl.Authentication {
    public class IsAdmin : IAuthorizationRequirement {
        public IsAdmin() { }
    }
}
