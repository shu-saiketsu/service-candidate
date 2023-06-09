﻿using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.IntegrationEvents;

namespace Saiketsu.Service.Candidate.Application.Candidates.Commands.DeleteCandidate;

public sealed class DeleteCandidateCommandHandler : IRequestHandler<DeleteCandidateCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;
    private readonly IValidator<DeleteCandidateCommand> _validator;

    public DeleteCandidateCommandHandler(IValidator<DeleteCandidateCommand> validator, IApplicationDbContext context,
        IEventBus eventBus)
    {
        _validator = validator;
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(DeleteCandidateCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var candidate = await _context.Candidates.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (candidate == null) return false;

        _context.Candidates.Remove(candidate);
        await _context.SaveChangesAsync(cancellationToken);

        var @event = new CandidateDeletedIntegrationEvent
        {
            Id = request.Id
        };

        _eventBus.Publish(@event);

        return true;
    }
}