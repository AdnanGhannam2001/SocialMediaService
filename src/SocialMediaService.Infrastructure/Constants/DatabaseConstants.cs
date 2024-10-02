namespace SocialMediaService.Infrastructure.Constants;

public static class DatabaseConstants
{
#if DOCKER
    public const string ConnectionStringName = "DockerPostgresConnection";
#else
    public const string ConnectionStringName = "PostgresConnection";
#endif // DOCKER 
}