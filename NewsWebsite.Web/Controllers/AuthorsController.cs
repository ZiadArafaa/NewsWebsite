using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Web.Service;
using NewsWebsite.Web.ViewModels;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;

namespace NewsWebsite.Web.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly IApiService _apiService;
        public AuthorsController(IApiService api, IDataProtectionProvider dataProtectionProvider)
        {
            _apiService = api;
        }
        public IActionResult Index()
        {

            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var Respons = _httpClient.GetAsync("api/Authors/GetAvailable").Result;

            if (Respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Create");

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Create");

            var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);

            return View(authors);
        }
        public IActionResult Create()
        {
            return View("Form", new AuthorViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorViewModel model)  
        {   
            using var _httpclient = _apiService.CreateHttpClient(new HttpClient());
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", User.FindFirstValue("Token"));
            var json = _apiService.ConvertToJson(model);
            var respons = _httpclient.PostAsync("api/Authors/Create", json).Result;

            if (!respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int AuthorId)
        {
         
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var Respons = _httpClient.GetAsync($"api/Authors/GetById/{AuthorId}").Result;

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var author = _apiService.ConvertToObject<AuthorViewModel>(Respons);

            return View("Form", author);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorViewModel model)
        {       
            using var _httpclient = _apiService.CreateHttpClient(new HttpClient());
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", User.FindFirstValue("Token"));
            var json = _apiService.ConvertToJson(model);
            var respons = _httpclient.PostAsync($"api/Authors/Update/{model.Id}", json).Result;

            if (!respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Toggle(int AuthorId)
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var Respons = _httpClient.GetAsync($"api/Authors/GetById/{AuthorId}").Result;
            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var author = _apiService.ConvertToObject<AuthorViewModel>(Respons);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var ToggleRespons = _httpClient.GetAsync($"api/Authors/Toggle/{author.Id}").Result;
            if (!ToggleRespons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Archive()
        {
           
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var Respons = _httpClient.GetAsync("api/Authors/GetDeleted").Result;

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);

            return View("Delete",authors);
        }
        public IActionResult Delete(int AuthorId)
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var Respons = _httpClient.GetAsync($"api/Authors/GetById/{AuthorId}").Result;
            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var author = _apiService.ConvertToObject<AuthorViewModel>(Respons);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var ToggleRespons = _httpClient.GetAsync($"api/Authors/Delete/{author.Id}").Result;
            if (!ToggleRespons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Archive");
        }
    }
}
