using MassTransit;
using Microsoft.EntityFrameworkCore;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Data.Seed;

namespace SocialMediaService.WebApi.Extensions;

internal static class WebApplicationExtensions
{
    public static void Setup(this WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }
    
    public static void HandleArguments(this WebApplication app, string[] args)
    {
        if (args.Contains("-s") || args.Contains("--seed"))
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var messagePublisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            SeedData.ApplyAsync(context, messagePublisher).GetAwaiter().GetResult();

            Environment.Exit(0);
        }
    }
}