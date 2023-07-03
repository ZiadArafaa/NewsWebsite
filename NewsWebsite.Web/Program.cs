using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using NewsWebsite.Web.Service;
using NewsWebsite.Web.Settings;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CloudinaryKey>(builder.Configuration.GetSection("CloudinaryKey"));

builder.Services.AddDataProtection().SetApplicationName(nameof(NewsWebsite));
builder.Services.AddExpressiveAnnotations();

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "YourCookieName";
        options.LoginPath = "/Auth/Login"; // Redirect unauthorized users to login page
        options.LogoutPath = "/Home/Index";
    });


builder.Services.AddTransient<IApiService,ApiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
