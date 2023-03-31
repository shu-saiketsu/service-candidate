using MediatR;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidate;

public sealed class GetCandidateQuery : IRequest<CandidateEntity?>
{
    public int Id { get; set; }
}