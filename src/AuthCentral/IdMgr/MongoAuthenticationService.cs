using System;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class MongoAuthenticationService : AuthenticationService<HierarchicalUserAccount>
    {
        readonly HttpContext _context;

        public MongoAuthenticationService(UserAccountService<HierarchicalUserAccount> userService, HttpContext context) : base(userService)
        {
            _context = context;
        }

        public MongoAuthenticationService(UserAccountService<HierarchicalUserAccount> userService, ClaimsAuthenticationManager claimsAuthenticationManager, HttpContext context) : base(userService, claimsAuthenticationManager)
        {
            _context = context;
        }

        protected override ClaimsPrincipal GetCurentPrincipal()
        {
            return _context.User;
        }

        protected override void RevokeToken()
        {
            _context.Authentication.SignOutAsync(_context.User.Identity.AuthenticationType);
        }

        protected override void IssueToken(ClaimsPrincipal principal, TimeSpan? tokenLifetime = null, bool? persistentCookie = null)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            SignOut();

            var props = new AuthenticationProperties();
            if (tokenLifetime.HasValue) props.ExpiresUtc = DateTime.UtcNow.Add(tokenLifetime.Value);
            if (persistentCookie.HasValue) props.IsPersistent = persistentCookie.Value;

            _context.Authentication.SignInAsync(principal.Identity.AuthenticationType, principal, props);
        }
    }
}