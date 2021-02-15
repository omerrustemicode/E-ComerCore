using E_ComerCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace E_ComerCore.Security
{
    public class SecurityManager
    {
        public async void SignIn(HttpContext HttpContext,Account account,string schema)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(getUserClaims(account), schema);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(schema,claimsPrincipal);
        }
        public async void SignOut(HttpContext HttpContext,string schema)
        {
            await HttpContext.SignOutAsync(schema);
        }

            private IEnumerable<Claim> getUserClaims(Account account)
            {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, account.Username));
            foreach(var roleAccount in account.RoleAccount)
            {
                claims.Add(new Claim(ClaimTypes.Role,roleAccount.Role.Name));
            }
            return claims;
            }
    }
}
