using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MovieCollection.Repositories;
using MovieCollection.Services;
using MovieCollection.Services.Implementations;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("appsettings.json", optional: false) 
          .AddJsonFile("appsettings.local.json", optional: true)
          .AddEnvironmentVariables();

    if (args != null)
    {
        config.AddCommandLine(args);
    }
});

builder.Logging.AddAzureWebAppDiagnostics();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie();
builder.Services.AddHttpClient();

var connString = builder.Configuration.GetConnectionString("Database");

if (Debugger.IsAttached)
{
    builder.Services.AddDbContext<MovieContext, FileMovieContext>();
}
else
{
    builder.Services.AddDbContext<MovieContext>();
}

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMovieService, MovieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetService<MovieContext>()
        ?? scope.ServiceProvider.GetService<FileMovieContext>();

    if (dataContext != null)
    {
        dataContext.Database.Migrate();
    }
}

app.Run();
