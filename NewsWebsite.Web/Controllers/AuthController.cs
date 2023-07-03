using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Web.Service;
using NewsWebsite.Web.ViewModels;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;

namespace NewsWebsite.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IDataProtector _dataProtector;

        public AuthController(IApiService api, IDataProtectionProvider dataProtectionProvider)
        {
            _apiService = api;
            _dataProtector = dataProtectionProvider.CreateProtector("MyProtection");
        }
        public IActionResult Login()
        {
            return View("Form");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Login)
        {
            if (!ModelState.IsValid)
                return View("Form",Login);

            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            var json = _apiService.ConvertToJson(Login);
            var Response = _httpClient.PostAsync("api/Auths/Login", json).Result;

            if(!Response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(nameof(Login.UserName), "UserName incorrect !");
                ModelState.AddModelError(nameof(Login.Password), "Password incorrect !");
                return View("Form", Login); 
            }

            var AuthModel = _apiService.ConvertToObject<AuthViewModel>(Response);

            var claimsIdentity = new ClaimsIdentity(new List<Claim> {new Claim(ClaimTypes.Name,Login.UserName),new Claim("Token",AuthModel.Token!)}
            ,CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Admin");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", controllerName: "Home");
        }
        [Authorize]
        public IActionResult Admin()
        {
            return View();
        }
    }
}
