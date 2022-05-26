using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using social_media.Data;
using social_media.Entity;
using soicalMedia.Data;
using soicalMedia.Extensions;
using soicalMedia.Interfaces;
using soicalMedia.Middleware;
using soicalMedia.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

//custome services
builder.Services.AddAplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);





//------------------------cors
builder.Services.AddCors(); //
//------------------------cors



builder.Services.AddTransient<Seed>();

var app = builder.Build();


    

//Seed Data-----------------------------------------------------------------
await SeedDataAsync(app);
async Task SeedDataAsync(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    
    using (var scope = scopedFactory?.CreateScope())
    {

        var service = scope.ServiceProvider.GetService<Seed>();
        await service.SeedUsers();
    }
}
//Seed Data-----------------------------------------------------------------


app.UseMiddleware<ExceptionMiddleware>();
/*// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
*/


//cors-------------------------------
app.UseCors(builder => builder     //
     .AllowAnyOrigin()             //
     .AllowAnyMethod()             // 
     .AllowAnyHeader());           //
//cors-------------------------------

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
