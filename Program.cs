using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Igrocom.Data;
using Igrocom.Models;
using Humanizer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<IgrocomContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IgrocomContext") ?? throw new InvalidOperationException("Connection string 'IgrocomContext' not found.")));


// Add services to the container.
builder.Services.AddControllersWithViews();

// builder.Services.AddSession( options =>
//     {
//         options.IdleTimeout = TimeSpan.FromMinutes(30);
//         options.Cookie.HttpOnly = true;
//         options.Cookie.IsEssential = true;
//     }
// );

builder.Services.AddAuthentication("MyCookie").AddCookie("MyCookie", options =>
{
    options.LoginPath = "/User/Login";
}
);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    SeedData.Initialize(service);
}



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

//app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
