using MediatR;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.Entities;
using Saiketsu.Service.Candidate.Domain.IntegrationEvents;

namespace Saiketsu.Service.Candidate.Application.IntegrationEventHandlers;

public sealed class PartyCreatedIntegrationEventHandler : IRequestHandler<PartyCreatedIntegrationEvent>
{
    private readonly IApplicationDbContext _context;

    public PartyCreatedIntegrationEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PartyCreatedIntegrationEvent request, CancellationToken cancellationToken)
    {
        var party = new PartyEntity
        {
            Id = request.Id,
            Name = request.Name
        };

        await _context.Parties.AddAsync(party, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}