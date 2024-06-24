using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;

namespace SocialMediaService.WebApi.Extensions;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddDataProtection()
            .SetApplicationName("SocialMedia");

        services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "SocialMediaCookies";
                opt.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("SocialMediaCookies")
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