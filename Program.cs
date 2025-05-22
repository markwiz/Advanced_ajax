global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.ComponentModel;
global using Microsoft.EntityFrameworkCore;
global using AdvancedAjax.Models;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using AdvancedAjax.Data;

var builder = WebApplication.CreateBuilder(args);

// Set up HTTP and HTTPS listeners
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5110); // HTTP
    options.ListenLocalhost(7298, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

// Fix HTTPS redirection issue
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7298;
});

// Register the DbContext with connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Set default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();