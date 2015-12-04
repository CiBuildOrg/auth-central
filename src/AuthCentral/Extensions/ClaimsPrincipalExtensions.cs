using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Authentication;

namespace Fsw.Enterprise.AuthCentral.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetId(this ClaimsPrincipal user)
        {
            Guid userId;
            if(!Guid.TryParse(user.Claims.FirstOrDefault(claim => claim.Type == "sub").Value, out userId))
            {
                throw new AuthenticationException();
            }

            return userId;
        }
    }
}
