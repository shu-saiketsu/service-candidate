using MediatR;

namespace Saiketsu.Service.Candidate.Application.Candidates.Commands.DeleteCandidate;

public sealed class DeleteCandidateCommand : IRequest<bool>
{
    public int Id { get; set; }
}