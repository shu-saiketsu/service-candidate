using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saiketsu.Service.Candidate.Application.Candidates.Commands.CreateCandidate;
using Saiketsu.Service.Candidate.Application.Candidates.Commands.DeleteCandidate;
using Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidate;
using Saiketsu.Service.Candidate.Application.Candidates.Queries.GetCandidates;
using Saiketsu.Service.Candidate.Domain.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace Saiketsu.Service.Candidate.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CandidatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandidatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieve a candidates")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retrieved candidates successfully", typeof(List<CandidateEntity>))]
    public async Task<IActionResult> GetAll()
    {
        var request = new GetCandidatesQuery();
        var response = await _mediator.Send(request);

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Retrieve a candidate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Retrieved candidate successfully", typeof(CandidateEntity))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Candidate does not exist")]
    public async Task<IActionResult> GetCandidate(int id)
    {
        var request = new GetCandidateQuery { Id = id };
        var response = await _mediator.Send(request);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Create a new candidate")]
    [SwaggerResponse(StatusCodes.Status201Created, "Created candidate successfully", typeof(CandidateEntity))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unable to create candidate")]
    public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateCommand command)
    {
        var response = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetCandidate), new { id = response.Id }, response);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Delete a candidate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Deleted candidate successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Candidate does not exist")]
    public async Task<IActionResult> DeleteCandidate(int id)
    {
        var request = new DeleteCandidateCommand { Id = id };
        var response = await _mediator.Send(request);

        if (response)
            return Ok();

        return NotFound();
    }
}