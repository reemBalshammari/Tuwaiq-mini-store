using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using mini_store;
using mini_store.Data;
using mini_store.Models;

var builder = WebApplication.CreateBuilder(args);


// =======================
// Localization
// =======================

builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");


// =======================
// Database
// =======================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// =======================
// Identity
// =======================

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


// =======================
// Authentication Cookie
// =======================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";

    options.Cookie.HttpOnly = true;
    options.SlidingExpiration = true;

    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});


// =======================
// MVC
// =======================

builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });


// =======================
// Session
// =======================

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// =======================
// Localization Options
// =======================

var supportedCultures = new[] { "ar", "en-US" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0]);

    options.AddSupportedCultures(supportedCultures);

    options.AddSupportedUICultures(supportedCultures);

    var browserProvider = options.RequestCultureProviders
        .OfType<AcceptLanguageHeaderRequestCultureProvider>()
        .FirstOrDefault();

    if (browserProvider != null)
    {
        options.RequestCultureProviders.Remove(browserProvider);
    }
});



var app = builder.Build();


// =======================
// Localization Middleware
// =======================

var localizationOptions = app.Services
    .GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localizationOptions.Value);


// =======================
// Errors
// =======================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


// =======================
// Middleware
// =======================

app.UseHttpsRedirection();

app.MapStaticAssets();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();


// =======================
// Routes
// =======================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();