using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaService.Persistent.Data;
using SocialMediaService.Persistent.Interfaces;
using SocialMediaService.Persistent.Repositories;

namespace SocialMediaService.Persistent.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddPersistent(this IServiceCollection services, string? sqlConnection)
    {
        return services
            .AddDbContext<ApplicationDbContext>(config => config.UseNpgsql(sqlConnection))
            .AddScoped(typeof(IReadRepository<,>), typeof(EfRepository<,>))
            .AddScoped(typeof(IWriteRepository<,>), typeof(EfRepository<,>))
            .AddScoped(typeof(IProfileRepository), typeof(ProfileEfRepository))
            .AddScoped(typeof(IPostRepository), typeof(PostEfRepository))
            .AddScoped(typeof(IGroupRepository), typeof(GroupEfRepository));
    }
}