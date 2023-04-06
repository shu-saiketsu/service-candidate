using MediatR;

namespace Saiketsu.Service.Candidate.Domain.IntegrationEvents;

public sealed class CandidateDeletedIntegrationEvent : IRequest
{
    public int Id { get; set; }
}