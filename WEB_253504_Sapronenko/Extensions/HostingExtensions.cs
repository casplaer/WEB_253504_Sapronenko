using WEB_253504_Sapronenko.UI.HelperClasses;

namespace WEB_253504_Sapronenko.UI.Extensions
{
	public static class HostingExtensions
	{
		public static void RegisterCustomServices(
			this WebApplicationBuilder builder)
		{
			builder.Services
				.Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));
		}
	}
}
