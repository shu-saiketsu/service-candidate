using FluentValidation;

namespace Saiketsu.Service.Candidate.Application.Candidates.Commands.DeleteCandidate;

public sealed class DeleteCandidateCommandValidator : AbstractValidator<DeleteCandidateCommand>
{
    public DeleteCandidateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}