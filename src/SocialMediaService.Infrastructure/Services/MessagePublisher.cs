using MassTransit;

namespace SocialMediaService.Infrastructure.Services;

public sealed class MessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessagePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task Publish<T>(T message) where T : class
    {
        return _publishEndpoint.Publish(message);
    }
}