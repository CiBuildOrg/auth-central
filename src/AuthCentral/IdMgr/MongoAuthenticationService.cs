﻿using System;
using System.Security.Claims;
using BrockAllen.MembershipReboot;
using BrockAllen.MembershipReboot.Hierarchical;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;

namespace Fsw.Enterprise.AuthCentral.IdMgr
{
    public class MongoAuthenticationService : AuthenticationService<HierarchicalUserAccount>
    {
        readonly HttpContext _context;

        public MongoAuthenticationService(UserAccountService<HierarchicalUserAccount> userService, IHttpContextAccessor ctxAccessor) : base(userService)
        {
            _context = ctxAccessor.HttpContext;
        }

        public MongoAuthenticationService(UserAccountService<HierarchicalUserAccount> userService, ClaimsAuthenticationManager claimsAuthenticationManager, IHttpContextAccessor ctxAccessor) : base(userService, claimsAuthenticationManager)
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