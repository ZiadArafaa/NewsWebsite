using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NewsWebsite.Web.Service;
using NewsWebsite.Web.Settings;
using NewsWebsite.Web.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using UoN.ExpressiveAnnotations.NetCore.Analysis;

namespace NewsWebsite.Web.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IDataProtector _dataProtector;
        private const long _imageSize = 4_194_304;
        private readonly IList<string> _extentions = new List<string> { ".jpg", ".png" };
        private readonly CloudinaryKey _key;
        private readonly Cloudinary _cloudinary;
        public NewsController(IApiService api, IDataProtectionProvider dataProtectionProvider,IOptions<CloudinaryKey> key)
        {
            _apiService = api;
            _dataProtector = dataProtectionProvider.CreateProtector("MyProtection");
            _key = key.Value;

            Account account = new Account(_key.Name, _key.ApiKey, _key.ApiSecretKey);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }
        public IActionResult Index()
        {
           
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var Respons = _httpClient.GetAsync("api/News/GetAvailable").Result;

            if (Respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Create");

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", "Home",new {txt="Error Request News"});

            var news = _apiService.ConvertToList<NewsViewModel>(Respons);

            return View(news);
        }
        public IActionResult Create()
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var Respons = _httpClient.GetAsync("api/Authors/GetAvailable").Result;

            if (Respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Create", "Authors");

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);

            return View("Form", new NewsFormViewModel { Authors = authors.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() }) });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsFormViewModel model)
        {
            using var _httpclient = _apiService.CreateHttpClient(new HttpClient());
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", User.FindFirstValue("Token"));

            if (model.ImageForm is null)
            {
                ModelState.AddModelError("ImageForm", "ImageForm is Required");
                var Respons = _httpclient.GetAsync("api/Authors/GetAvailable").Result;
                var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);
                model.Authors = authors.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
                return View("Form", model);
            }

            if (model.ImageForm.Length > _imageSize || !_extentions.Contains(Path.GetExtension(model.ImageForm.FileName).ToLower()))
            {
                ModelState.AddModelError("ImageForm", "Check size and extension");
                var Respons = _httpclient.GetAsync("api/Authors/GetAvailable").Result;
                var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);
                model.Authors = authors.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
                return View("Form", model);
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription("NewsImage", model.ImageForm.OpenReadStream())
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            model.Image = uploadResult.SecureUrl.ToString();
            model.PublicId = uploadResult.PublicId;

            var json = _apiService.ConvertToJson(model);
            var respons = _httpclient.PostAsync("api/News/Create", json).Result;

            if (!respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int NewsId)
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var ResponsAuthor = _httpClient.GetAsync("api/Authors/GetAvailable").Result;

            if (ResponsAuthor.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Create", "Authors");

            if (!ResponsAuthor.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var authors = _apiService.ConvertToList<AuthorViewModel>(ResponsAuthor);

            var ResponsNews = _httpClient.GetAsync($"api/News/GetById/{NewsId}").Result;

            if (!ResponsNews.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var News = _apiService.ConvertToObject<NewsFormViewModel>(ResponsNews);
            News.Authors = authors.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });

            return View("Form", News);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsFormViewModel model)
        {
            using var _httpclient = _apiService.CreateHttpClient(new HttpClient());
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", User.FindFirstValue("Token"));

            if (model.ImageForm is not null)
            {
                if (model.ImageForm.Length > _imageSize || !_extentions.Contains(Path.GetExtension(model.ImageForm.FileName).ToLower()))
                {
                    ModelState.AddModelError("ImageForm", "Check size and extension");
                    var Respons = _httpclient.GetAsync("api/Authors/GetAvailable").Result;
                    var authors = _apiService.ConvertToList<AuthorViewModel>(Respons);
                    model.Authors = authors.Select(p => new SelectListItem { Text = p.Name, Value = p.Id.ToString() });
                    return View("Form", model);
                }

                DeletionParams deletionParams = new DeletionParams(model.PublicId);
                DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                if (deletionResult.Result == "ok")
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription("NewsImage", model.ImageForm.OpenReadStream())
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    model.Image = uploadResult.SecureUrl.ToString();
                    model.PublicId = uploadResult.PublicId;
                }
                else
                {
                    return RedirectToAction("Error", controllerName: "Home");
                }

            }

            var json = _apiService.ConvertToJson(model);
            var respons = _httpclient.PostAsync($"api/News/Update/{model.Id}", json).Result;

            if (!respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Toggle(int NewsId)
        {
           
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var Respons = _httpClient.GetAsync($"api/News/GetById/{NewsId}").Result;
            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var news = _apiService.ConvertToObject<NewsViewModel>(Respons);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var ToggleRespons = _httpClient.GetAsync($"api/News/Toggle/{news.Id}").Result;
            if (!ToggleRespons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Index");
        }
        public IActionResult Archive()
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));

            var Respons = _httpClient.GetAsync("api/News/GetDeleted").Result;

            if (Respons.StatusCode == System.Net.HttpStatusCode.NotFound)
                return RedirectToAction("Index");

            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error","Home");

            var news = _apiService.ConvertToList<NewsViewModel>(Respons);

            return View("Delete", news);
        }
        public async Task<IActionResult> Delete(int NewsId)
        {
            using var _httpClient = _apiService.CreateHttpClient(new HttpClient());
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var Respons = _httpClient.GetAsync($"api/News/GetById/{NewsId}").Result;
            if (!Respons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            var news = _apiService.ConvertToObject<NewsViewModel>(Respons);

            DeletionParams deletionParams = new DeletionParams(news.PublicId);
            DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", User.FindFirstValue("Token"));
            var ToggleRespons = _httpClient.GetAsync($"api/News/Delete/{news.Id}").Result;
            if (!ToggleRespons.IsSuccessStatusCode)
                return RedirectToAction("Error", controllerName: "Home");

            return RedirectToAction("Archive");
        }



        public IActionResult DataValidate(DateTime PublicationDate)
        {
            var acc = PublicationDate.Year.Equals(DateTime.Now.Year) && PublicationDate.Month.Equals(DateTime.Now.Month) &&
                (PublicationDate.Day >= DateTime.Now.Day && PublicationDate.Day < DateTime.Now.Day + 7);
            return Json(acc);
        }
    }
}
