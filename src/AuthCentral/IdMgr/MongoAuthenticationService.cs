using System;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authentication.OpenIdConnect;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class MongoAuthenticationService : AuthenticationService<HierarchicalUserAccount>
    {
        readonly HttpContext _context;

        public MongoAuthenticationService(DefaultUserAccountServiceContainer container, IHttpContextAccessor ctxAccessor) : base(container.Service)
        {
            _context = ctxAccessor.HttpContext;
        }

        public MongoAuthenticationService(DefaultUserAccountServiceContainer container, ClaimsAuthenticationManager claimsAuthenticationManager, IHttpContextAccessor ctxAccessor) : base(container.Service, claimsAuthenticationManager)
        {
            _context = ctxAccessor.HttpContext;
        }

        protected override ClaimsPrincipal GetCurentPrincipal()
        {
            return _context.User;
        }

        protected override void RevokeToken()
        {
            _context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _context.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        protected override void IssueToken(ClaimsPrincipal principal, TimeSpan? tokenLifetime = null, bool? persistentCookie = null)
        {
            if (principal == null) throw new ArgumentNullException(nameof(principal));

            SignOut();

            var props = new AuthenticationProperties();
            if (tokenLifetime.HasValue) props.ExpiresUtc = DateTime.UtcNow.Add(tokenLifetime.Value);
            if (persistentCookie.HasValue) props.IsPersistent = persistentCookie.Value;

            _context.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
        }
    }
}