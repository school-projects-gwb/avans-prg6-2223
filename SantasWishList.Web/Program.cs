using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SantasWishList.Data;
using SantasWishList.Data.Models;

var builder = WebApplication.CreateBuilder(args);

//Context and Identity
builder.Services.AddDbContext<DatabaseContext>()
    .AddDefaultIdentity<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<DatabaseContext>();

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

