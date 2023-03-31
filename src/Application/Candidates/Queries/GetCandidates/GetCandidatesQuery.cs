using MediatR;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidates;

public sealed class GetCandidatesQuery : IRequest<List<CandidateEntity>>
{
}