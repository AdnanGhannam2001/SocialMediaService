namespace SocialMediaService.Domain.Aggregates.Profiles.ValueObjects;

public sealed record JobInformations
{
    public JobInformations(string? jobTitle = null, string? company = null, DateTime? startDate = null)
    {
        JobTitle = jobTitle;
        Company = company;
        StartDate = startDate;
    }

    public string? JobTitle { get; }
    public string? Company { get; }
    public DateTime? StartDate { get; }
}