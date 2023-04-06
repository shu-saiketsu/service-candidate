using System.Text.Json.Serialization;

namespace Saiketsu.Service.Candidate.Domain.Entities;

public sealed class PartyEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    [JsonIgnore] public ICollection<CandidateEntity> Candidates { get; set; } = null!;
}