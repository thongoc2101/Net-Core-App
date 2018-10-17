using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetCoreApp.Data.Entities;

namespace NetCoreApp.Helpers
{
    public class CustomClaimPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private readonly UserManager<AppUser> _userManager;

        public CustomClaimPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            _userManager = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser appUser)
        {
            var principal = await base.CreateAsync(appUser);
            var role = await _userManager.GetRolesAsync(appUser);
            ((ClaimsIdentity)principal.Identity).AddClaims(new []
            {
                new Claim("Email", appUser.Email), 
                new Claim("FullName", appUser.FullName??String.Empty),
                new Claim("Avatar", appUser.Avatar??string.Empty), 
                new Claim("Roles", string.Join(";", role)), 
            });
            return principal;
        }
    }
}
