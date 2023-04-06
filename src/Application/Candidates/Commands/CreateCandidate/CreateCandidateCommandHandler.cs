using FluentValidation;
using MediatR;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.Entities;
using Saiketsu.Service.Candidate.Domain.IntegrationEvents;

namespace Saiketsu.Service.Candidate.Application.Candidates.Commands.CreateCandidate;

public sealed class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, CandidateEntity>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;
    private readonly IValidator<CreateCandidateCommand> _validator;

    public CreateCandidateCommandHandler(IValidator<CreateCandidateCommand> validator, IApplicationDbContext context,
        IEventBus eventBus)
    {
        _validator = validator;
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<CandidateEntity> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var candidate = new CandidateEntity
        {
            Name = request.Name,
            PartyId = request.PartyId
        };

        // save data to database
        await _context.Candidates.AddAsync(candidate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // return the party reference
        await _context.Entry(candidate)
            .Reference("Party")
            .LoadAsync(cancellationToken);

        var @event = new CandidateCreatedIntegrationEvent
        {
            Id = candidate.Id,
            Name = candidate.Name
        };

        _eventBus.Publish(@event);

        return candidate;
    }
}