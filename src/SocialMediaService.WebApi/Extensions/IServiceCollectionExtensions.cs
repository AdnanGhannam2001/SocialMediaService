using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace SocialMediaService.WebApi.Extensions;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "WebApiCookies";
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("WebApiCookies")
            .AddOpenIdConnect("oidc", configurationSection.Bind);

        services.AddAuthorization();

        return services;
    }
}