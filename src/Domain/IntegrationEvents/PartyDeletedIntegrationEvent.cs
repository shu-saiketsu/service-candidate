using MediatR;

namespace Saiketsu.Service.Candidate.Domain.IntegrationEvents;

public sealed class PartyDeletedIntegrationEvent : IRequest
{
    public int Id { get; set; }
}