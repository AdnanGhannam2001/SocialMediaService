namespace SocialMediaService.Infrastructure.Constants;

// TODO Rename
public static class DatabaseConstants
{
#if DOCKER
    public const string ConnectionStringName = "DockerPostgresConnection";
    public const string RabbitMqSettings = "DockerRabbitMqSettings";
#else
    public const string ConnectionStringName = "PostgresConnection";
    public const string RabbitMqSettings = "RabbitMqSettings";
#endif // DOCKER 
}