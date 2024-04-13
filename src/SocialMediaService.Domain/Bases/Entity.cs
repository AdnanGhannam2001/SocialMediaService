using NanoidDotNet;

namespace SocialMediaService.Domain.Bases;

public abstract class Entity() : Entity<string>(Nanoid.Generate(size: 15));