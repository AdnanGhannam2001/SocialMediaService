using PR2.Shared.Common;
using SocialMediaService.Application.Interfaces;
using SocialMediaService.Domain.Aggregates.Profiles;

namespace SocialMediaService.Application.Features.Queries.GetFavoriteDiscussionsPage;

public sealed record GetFavoriteDiscussionsPageQuery(string ProfileId, PageRequest<FavoriteDiscussion> Request)
    : IQuery<Page<FavoriteDiscussion>>;