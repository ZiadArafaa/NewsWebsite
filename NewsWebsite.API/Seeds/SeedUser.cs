using Microsoft.AspNetCore.Identity;
using NewsWebsite.API.Models;

namespace NewsWebsite.API.Seeds
{
    public class SeedUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SeedUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SeedAsync()
        {
            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName= "User",
                    Email= "User@dev.com",
                };
                await _userManager.CreateAsync(user,"P@ssword123");
                await _userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
