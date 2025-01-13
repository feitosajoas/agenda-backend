using agenda.Application.Queries;
using agenda.Common.Services;
using agenda.Common.Utils;
using agenda.Models;
using agenda.Models.ModelResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace agenda.Controllers;

[ApiController]
[Route("api/login")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = new LoginQuery(request.Username, request.Password);
        var token = await _mediator.Send(query);

        if (token == null)
        {
            return Unauthorized(new ApiResponse(401, Messages.INVALID_CREDENTIALS));
        }

        return Ok(new { Token = token });
    }

}


