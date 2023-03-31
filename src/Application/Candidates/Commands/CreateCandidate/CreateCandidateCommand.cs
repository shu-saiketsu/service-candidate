using MediatR;
using Saiketsu.Service.Candidate.Domain.Entities;

namespace Saiketsu.Service.Candidate.Application.Candidates.Commands.CreateCandidate;

public sealed class CreateCandidateCommand : IRequest<CandidateEntity>
{
    public string Name { get; set; } = null!;
    public int? PartyId { get; set; }
}