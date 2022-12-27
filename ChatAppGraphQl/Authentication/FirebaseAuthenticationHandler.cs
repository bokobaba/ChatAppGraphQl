using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ChatAppGraphQl.Authentication {
    public struct FirebaseUserClaimType {
        public const string ID = "user_id";
        public const string EMAIL = "email";
    }

    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
        private FirebaseApp _firebaseApp;

        public FirebaseAuthenticationHandler(
            FirebaseApp firebaseApp,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock) {
            _firebaseApp = firebaseApp;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (!Context.Request.Headers.TryGetValue("Authorization", out StringValues authHeader)) {
                return AuthenticateResult.NoResult();
            }

            string bearerToken = authHeader.ToString();
            if (bearerToken == null || !bearerToken.StartsWith("Bearer ") || bearerToken.Length < 1) {
                return AuthenticateResult.Fail("Invalid scheme");
            }

            string token = bearerToken["Bearer ".Length..];

            try {
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);

                return AuthenticateResult.Success(new AuthenticationTicket(
                    new ClaimsPrincipal(new List<ClaimsIdentity>() {
                    new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler))
                    }),
                    JwtBearerDefaults.AuthenticationScheme
                ));
            } catch(Exception ex) {
                return AuthenticateResult.Fail(ex);
            }
        }

        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims) {
            return new List<Claim> {
                new Claim(FirebaseUserClaimType.ID, claims[FirebaseUserClaimType.ID].ToString() ?? ""),
                new Claim(FirebaseUserClaimType.EMAIL, claims[FirebaseUserClaimType.EMAIL].ToString() ?? ""),
            };
        }
    }
}
