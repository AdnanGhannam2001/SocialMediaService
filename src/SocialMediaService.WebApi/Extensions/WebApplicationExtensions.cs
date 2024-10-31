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
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (args.Contains("-c") || args.Contains("--clear"))
        {
            SeedData.ClearAsync(context).GetAwaiter().GetResult();
            Environment.Exit(0);
        }

        if (args.Contains("-s") || args.Contains("--seed"))
        {
            var messagePublisher = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
            SeedData.ApplyAsync(context, messagePublisher).GetAwaiter().GetResult();
            Environment.Exit(0);
        }
    }
}