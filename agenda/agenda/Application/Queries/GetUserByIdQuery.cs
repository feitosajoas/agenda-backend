using agenda.Models;
using MediatR;

namespace agenda.Application.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public Guid Id { get; set; }

    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}
