using agenda.Application.Commands;
using agenda.Application.Validators;
using agenda.Common.Interfaces;
using agenda.Common.Utils;
using agenda.Infrastructure.Interfaces;
using agenda.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace agenda.Application.Handlers;

public class UserCommandHandle : IRequestHandler<UserCommand, User>
{
    private readonly IRepositoryService<User> _repository;
    private readonly UserValidator _validator;
    private readonly IPasswordHasherService _passwordHasher;

    public UserCommandHandle(IRepositoryService<User> repository, IPasswordHasherService passwordHasher)
    {
        _repository = repository;
        _validator = new UserValidator();
        _passwordHasher = passwordHasher;
    }

    public async Task<User> Handle(UserCommand request, CancellationToken cancellationToken)
    {
        User user;

        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        switch (request.CommandType)
        {
            case CommandType.Add:
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name!,
                    Password = _passwordHasher.HashPassword(request.Password!),
                    Email = request.Email!,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repository.AddAsync(user, cancellationToken);
                break;

            case CommandType.Edit:
                if (!request.Id.HasValue) throw new Exception(Messages.USER_ID_REQUIRED_EDIT);

                user = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);
                if (user == null) throw new Exception(Messages.USER_NOT_FOUND);

                user.Name = request.Name!;
                user.Email = request.Email!;
                user.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(user, cancellationToken);
                break;

            case CommandType.Delete:
                if (!request.Id.HasValue) throw new Exception(Messages.USER_ID_REQUIRED_DELETE);

                user = await _repository.GetByIdAsync(request.Id.Value, cancellationToken);
                if (user == null) throw new Exception(Messages.USER_NOT_FOUND);

                await _repository.DeleteAsync(user, cancellationToken);
                break;

            default:
                throw new InvalidOperationException(Messages.UNSUPPORTED_COMMAND_TYPE);
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return user;
    }
}

