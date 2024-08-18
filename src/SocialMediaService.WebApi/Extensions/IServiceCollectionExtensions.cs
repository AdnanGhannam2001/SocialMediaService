using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using SocialMediaService.WebApi.Services;

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
                        var uri = new Uri(context.Request.GetDisplayUrl());

                        if (uri.Segments.LastOrDefault() != "login")
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.HandleResponse();

                            return Task.CompletedTask;
                        }

                        var redirectUri = context.Request.Query.FirstOrDefault(x => x.Key.Equals("redirect_uri"));
                        if (!redirectUri.Equals(new KeyValuePair<string, StringValues>()))
                        {
                            context.Properties.RedirectUri = redirectUri.Value.FirstOrDefault();
                        }

                        return Task.CompletedTask;
                    },
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services.AddScoped<FilesService>();
    }
}