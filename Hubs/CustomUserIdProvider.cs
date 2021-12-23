using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Web;

namespace HMTStationery.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var claimsIdentity = HttpContext.Current.User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            string email = claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
            return email;
        }
    }
}