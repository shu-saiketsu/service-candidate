using MediatR;

namespace Saiketsu.Service.Candidate.Domain.IntegrationEvents;

public sealed class CandidateCreatedIntegrationEvent : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}