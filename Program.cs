using Microsoft.EntityFrameworkCore;
using QuickNotes.Contexts;
using QuickNotes.Models;

var builder = WebApplication.CreateBuilder(args);

// what i added :::
builder.Services.AddDbContext<QuickNotesDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<QuickNotesDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "HelloCookie";
    options.Cookie = cookieBuilder;

    // This tells the system where to redirect unauthenticated users
    // when they try to access a restricted page
    options.LoginPath = new PathString("/Home/LogIn");

    // The cookie will expire 60 days after being issued
    // unless refreshed via sliding expiration
    options.ExpireTimeSpan = TimeSpan.FromDays(60);

    // Expiration time will reset with each user activity (e.g., page load)
    options.SlidingExpiration = true;
});
// ::: what i added

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
