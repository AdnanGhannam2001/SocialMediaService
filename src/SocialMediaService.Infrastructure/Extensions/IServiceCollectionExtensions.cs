using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaService.Infrastructure.Services;

namespace SocialMediaService.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddMassTransit(config => config.UsingRabbitMq())
            .AddScoped<MessagePublisher>();
    }
}