using System.Security.Claims;
using agenda.Application.Commands;
using agenda.Application.Queries;
using agenda.Common.Interfaces;
using agenda.Common.Utils;
using agenda.Models.ModelResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace agenda.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJwtService _jwtService;

    public ContactController(IMediator mediator, IJwtService jwtService)
    {
        _mediator = mediator;
        _jwtService = jwtService;
    }

    [HttpPost]
    public async Task<IActionResult> AddContact([FromBody] ContactCommand command)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            command.CommandType = CommandType.Add;
            command.OwnerContactId = Guid.Parse(userId);
            await _mediator.Send(command);
            return Ok(new ApiResponse(200, Messages.CONTACT_SAVED_SUCESSFULLY));
        }
        return Unauthorized(new ApiResponse(401, Messages.INVALID_TOKEN));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditContact(Guid id, [FromBody] ContactCommand command)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            command.CommandType = CommandType.Edit;
            command.Id = id;
            command.OwnerContactId = Guid.Parse(userId);
            await _mediator.Send(command);
            return Ok(new ApiResponse(201, Messages.CONTACT_UPDATED_SUCESSFULLY));
        }
        return Unauthorized(new ApiResponse(401, Messages.INVALID_TOKEN));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var command = new ContactCommand
            {
                CommandType = CommandType.Delete,
                Id = id,
                OwnerContactId = Guid.Parse(userId)
            };
            await _mediator.Send(command);
            return Ok(new ApiResponse(200, Messages.CONTACT_DELETED_SUCESSFULLY));
        }
        return Unauthorized(new ApiResponse(401, Messages.INVALID_TOKEN));
    }

    // Query Endpoints
    [HttpGet("{id}")]
    public async Task<IActionResult> GetContactById(Guid id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var query = new GetContactByIdQuery
            {
                Id = id,
                OwnerContactId = Guid.Parse(userId),
            };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null)
                return NotFound(new ApiResponse(404, Messages.CONTACT_NOT_FOUND));
            return Ok(new ApiResponse(200, Messages.CONTACT_RETRIVED_SUCESSFULLY));
        }
        return Unauthorized(new ApiResponse(401, Messages.INVALID_TOKEN));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllContacts(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            var query = new GetAllContactsQuery
            {
                OwnerContactId = Guid.Parse(userId)
            };
            var result = await _mediator.Send(query, cancellationToken);

            if (result.Count() == 0) return Ok(new ApiResponse(200, Messages.USER_NOT_CONTACT_REGISTERED));

            return Ok(result);
        }
        return Unauthorized(new ApiResponse(401, Messages.INVALID_TOKEN));
    }
}


