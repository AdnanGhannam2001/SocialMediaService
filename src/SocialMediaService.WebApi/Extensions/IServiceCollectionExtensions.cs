using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Extensions;

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
            .AddOpenIdConnect("oidc", config =>
            {
                configurationSection.Bind(config);
                config.Events = new OpenIdConnectEvents()
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status307TemporaryRedirect;
                        context.Properties.RedirectUri = "http://localhost:4200/profiles/profile";

                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();

        return services;
    }
}