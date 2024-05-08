using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace SocialMediaService.WebApi.Extensions;

internal static class IServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = "WebApiCookies";
            opt.DefaultChallengeScheme = "oidc";
        })
            .AddCookie("WebApiCookies")
            .AddOpenIdConnect("oidc", opt =>
            {
                opt.Authority = "https://localhost:5001";
                opt.RequireHttpsMetadata = false;

                opt.ClientId = "WEB_API";
                opt.ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A";
                opt.ResponseType = "code";

                opt.Scope.Clear();
                opt.Scope.Add("openid");
                opt.Scope.Add("profile");
                opt.GetClaimsFromUserInfoEndpoint = true;

                opt.MapInboundClaims = false;

                opt.SaveTokens = true;
            });

        services.AddAuthorization();

        return services;
    }
}