using Microsoft.AspNetCore.Identity;
using SantasWishList.Data;
using SantasWishList.Data.Models;
using SantasWishList.Web.Authorization;

var builder = WebApplication.CreateBuilder(args);

//Context and Identity
builder.Services.AddDbContext<DatabaseContext>()
    .AddIdentity<User, Role>()
    // .AddRoles<Role>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddMvc();
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

app.UseAuthentication();;
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

