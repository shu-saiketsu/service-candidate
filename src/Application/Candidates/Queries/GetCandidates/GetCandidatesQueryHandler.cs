using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Saiketsu.Service.Candidate.Application.Common;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidates;

public sealed class GetCandidatesQueryHandler : IRequestHandler<GetCandidatesQuery, List<CandidateEntity>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetCandidatesQuery> _validator;

    public GetCandidatesQueryHandler(IApplicationDbContext context, IValidator<GetCandidatesQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<List<CandidateEntity>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var candidates = await _context.Candidates
            .Include(x => x.Party)
            .ToListAsync(cancellationToken);

        return candidates;
    }
}