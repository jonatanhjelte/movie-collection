using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MovieCollection.Repositories;
using MovieCollection.Services;
using MovieCollection.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);


var rootPath = builder.Environment.WebRootPath ?? builder.Environment.ContentRootPath;
var dbDir = Path.Combine(rootPath, "app_data");
if (!Directory.Exists(dbDir))
{
    Directory.CreateDirectory(dbDir);
}

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie();



builder.Services.AddDbContext<MovieContext>(
    options => options.UseSqlite($"Data Source={Path.Combine(dbDir, "movies.db")}"));

builder.Services.AddScoped<IUserService, UserService>();

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
    var dataContext = scope.ServiceProvider.GetRequiredService<MovieContext>();
    dataContext.Database.Migrate();
}

app.Run();
