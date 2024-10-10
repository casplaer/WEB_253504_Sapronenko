using Microsoft.EntityFrameworkCore;
using WEB_253504_Sapronenko.UI.Extensions;
using WEB_253504_Sapronenko.UI.Models;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.HeroService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

UriData.ApiUri = builder.Configuration.GetSection("UriData")["ApiUri"];

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt=>
                                                        opt.BaseAddress=new Uri(UriData.ApiUri));

builder.Services.AddHttpClient<IHeroService, ApiHeroService>(opt=>
                                                        opt.BaseAddress=new Uri(UriData.ApiUri));


//builder.RegisterCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
