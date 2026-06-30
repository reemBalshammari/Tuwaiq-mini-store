using Microsoft.EntityFrameworkCore;
using mini_store.Data;
using mini_store;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResource));
    });
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
    var supportedCultures = new[] { "ar", "en-US" };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);

    var browserProvider = options.RequestCultureProviders
        .OfType<Microsoft.AspNetCore.Localization.AcceptLanguageHeaderRequestCultureProvider>()
        .FirstOrDefault();

    if (browserProvider != null)
    {
        options.RequestCultureProviders.Remove(browserProvider);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var localizationOptions = app.Services
    .GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(localizationOptions.Value);

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
