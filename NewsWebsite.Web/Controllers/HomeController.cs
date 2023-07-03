using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Web.Models;
using NewsWebsite.Web.Service;
using NewsWebsite.Web.ViewModels;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace NewsWebsite.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IApiService _apiService;

        public HomeController(IApiService api, ILogger<HomeController> logger)
        {
            _apiService = api;
            _logger = logger;
        }

        public IActionResult Index()
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            var Respons = _httpClient.GetAsync("api/News/GetAvailable").Result;

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", new { txt = "News Not Found !" });

            var news = _apiService.ConvertToList<NewsViewModel>(Respons);

            return View(news);
        }
        public IActionResult Card(int id, DateTime date)
        {
            DateTime DateNow = DateTime.Now;
            if (date.Year > DateNow.Year
                ||
                (date.Year == DateNow.Year && date.Month > DateNow.Month)
                ||
                (date.Year == DateNow.Year && date.Month == DateNow.Month && date.Day > date.Day))

                return RedirectToAction("Index");

            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            var Respons = _httpClient.GetAsync($"api/News/GetById/{id}").Result;

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", new { txt = "not found news" });

            var news = _apiService.ConvertToObject<NewsViewModel>(Respons);


            return View(news);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string txt)
        {
            return View(new ErrorViewModel { Message = txt });
        }
    }
}