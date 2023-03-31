using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidate;

public sealed class GetCandidateQueryHandler : IRequestHandler<GetCandidateQuery, CandidateEntity?>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetCandidateQuery> _validator;

    public GetCandidateQueryHandler(IValidator<GetCandidateQuery> validator, IApplicationDbContext context)
    {
        _validator = validator;
        _context = context;
    }

    public async Task<CandidateEntity?> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var candidate = await _context.Candidates
            .Include(x => x.Party)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return candidate;
    }
}