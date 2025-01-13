using agenda.Application.Queries;
using agenda.Common.Utils;
using agenda.Infrastructure.Interfaces;
using agenda.Models;
using MediatR;

namespace agenda.Application.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IRepositoryService<User> _repository;

    public GetUserByIdQueryHandler(IRepositoryService<User> repository)
    {
        _repository = repository;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException(Messages.USER_NOT_FOUND);
        }
        return user;
    }
}
