using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaService.Persistent.Data;

namespace SocialMediaService.Persistent.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistent(this IServiceCollection services, string? sqlConnection)
    {
        return services.AddDbContext<ApplicationDbContext>(config => config.UseNpgsql(sqlConnection));
    }
}