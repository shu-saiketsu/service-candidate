using MediatR;
using Microsoft.EntityFrameworkCore;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.IntegrationEvents;

namespace Saiketsu.Service.Candidate.Application.IntegrationEventHandlers;

public sealed class PartyDeletedIntegrationEventHandler : IRequestHandler<PartyDeletedIntegrationEvent>
{
    private readonly IApplicationDbContext _context;

    public PartyDeletedIntegrationEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PartyDeletedIntegrationEvent request, CancellationToken cancellationToken)
    {
        var party = await _context.Parties.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (party == null) return;

        _context.Parties.Remove(party);

        await _context.SaveChangesAsync(cancellationToken);
    }
}