using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaService.Infrastructure.Services;

namespace SocialMediaService.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationSection rmqSettings)
    {
        return services
            .AddMassTransit(config => config.UsingRabbitMq((context, rmqConfig) =>
                rmqConfig.Host(rmqSettings["Host"], host =>
                {
                    host.Username(rmqSettings["Username"]!);
                    host.Password(rmqSettings["Password"]!);
                })))
            .AddScoped<MessagePublisher>();
    }
}