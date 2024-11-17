using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253504_Sapronenko.BlazorWasm;
using WEB_253504_Sapronenko.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

UriData.ApiUri = builder.Configuration.GetSection("UriData")["ApiUri"];

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

/*builder.Services.AddScoped<IDataService, ApiDataService>();
*/builder.Services
    .AddHttpClient<IDataService, ApiDataService>(opt =>
                                                opt.BaseAddress = new Uri(UriData.ApiUri));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
});

await builder.Build().RunAsync();
