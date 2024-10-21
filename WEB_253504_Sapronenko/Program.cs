using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WEB_253504_Sapronenko.UI.Authorization;
using WEB_253504_Sapronenko.UI.HelperClasses;
using WEB_253504_Sapronenko.UI.Models;
using WEB_253504_Sapronenko.UI.Services.Authentication;
using WEB_253504_Sapronenko.UI.Services.CategoryService;
using WEB_253504_Sapronenko.UI.Services.FileService;
using WEB_253504_Sapronenko.UI.Services.HeroService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

UriData.ApiUri = builder.Configuration.GetSection("UriData")["ApiUri"];

builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt=>
                                                        opt.BaseAddress=new Uri(UriData.ApiUri));

builder.Services.AddHttpClient<IHeroService, ApiHeroService>(opt=>
                                                        opt.BaseAddress=new Uri(UriData.ApiUri));

builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
                                                        opt.BaseAddress = new Uri(UriData.ApiUri));

builder.Services
    .AddHttpClient<IAuthService, KeycloakAuthService>(opt => opt.BaseAddress = new Uri(UriData.ApiUri));

builder.Services
    .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));




var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services
.AddAuthentication(options =>
{
    options.DefaultScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer()
.AddOpenIdConnect(options =>
{
    options.Authority =
    $"{keycloakData!.Host}/auth/realms/{keycloakData.Realm}";
    options.ClientId = keycloakData.ClientId;
    options.ClientSecret = keycloakData.ClientSecret;
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.Scope.Add("openid");
    options.SaveTokens = true;
    options.RequireHttpsMetadata = false; 
    options.MetadataAddress =
        $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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
