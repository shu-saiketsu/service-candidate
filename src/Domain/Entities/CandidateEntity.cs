namespace Saiketsu.Service.Candidate.Domain.Entities;

public sealed class CandidateEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public PartyEntity? Party { get; set; }
    public int? PartyId { get; set; }
}