using agenda.Models;
using MediatR;

namespace agenda.Application.Commands;

public class UserCommand : IRequest<User>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public CommandType CommandType { get; set; }
}
