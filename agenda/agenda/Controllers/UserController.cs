using agenda.Application.Commands;
using agenda.Application.Queries;
using agenda.Common.Utils;
using agenda.Models.ModelResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace agenda.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            return Ok(user);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse(404, Messages.USER_NOT_FOUND));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserCommand command)
    {
        try
        {
            var user = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            if (ex.Message == Messages.EMAIL_ALREADY_USE)
            {
                return Conflict(new ApiResponse(409, ex.Message));
            }
            return BadRequest(new ApiResponse(400, ex.Message));
        }
    }

}
