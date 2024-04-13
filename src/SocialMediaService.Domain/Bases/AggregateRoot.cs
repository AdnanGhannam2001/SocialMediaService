using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class AggregateRoot() : AggregateRoot<string>(Nanoid.Generate(size: 15));