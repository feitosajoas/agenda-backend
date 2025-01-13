using agenda.Application.Queries;
using agenda.Common.Interfaces;
using agenda.Infrastructure.Interfaces;
using agenda.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace agenda.Application.Handlers;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string?>
{
    private readonly IRepositoryService<User> _repository;
    private readonly IPasswordHasherService _passwordHasher;
    private readonly IJwtService _jwtService;

    public LoginQueryHandler(
        IRepositoryService<User> repository,
        IPasswordHasherService passwordHasher,
        IJwtService jwtService)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetByExpressionAsync(u => u.Email == request.Username, cancellationToken);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return null;
        }

        if (!_passwordHasher.VerifyPassword(user.Password, request.Password))
        {
            return null;
        }

        return _jwtService.GenerateToken(user);
    }
}

