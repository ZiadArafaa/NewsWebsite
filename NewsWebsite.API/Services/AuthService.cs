using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsWebsite.API.Data;
using NewsWebsite.API.Dtos;
using NewsWebsite.API.Models;
using NewsWebsite.API.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsWebsite.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
        public async Task<AuthDto> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username) ??
                await _userManager.FindByNameAsync(username);

            if (user is null || !await _userManager.CheckPasswordAsync(user, password))
            {
                return new AuthDto { ErrorMessage = "Password or Username not found!" };
            }

            var jwtSecurity = await CreateTokenAsync(user);

            return new AuthDto
            {
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurity),
                ExpireOn = jwtSecurity.ValidTo
            };
        }
        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user)
        {
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaims = new List<Claim>();

            foreach (var role in Roles)
            {
                RoleClaims.Add(new Claim("roles", role));
            }

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }.Union(RoleClaims).Union(UserClaims);

            var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var SigningCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer
                , claims: Claims
                , audience: _jwt.Audience
                , expires: DateTime.Now.AddDays(_jwt.DurationInDays)
                , signingCredentials: SigningCredentials
                );
        }

    }
}
