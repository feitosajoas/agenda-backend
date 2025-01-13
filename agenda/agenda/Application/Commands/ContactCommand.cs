using System.Data;
using MediatR;

namespace agenda.Application.Commands;

public class ContactCommand : IRequest<Unit>
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid OwnerContactId { get; set; }
    public CommandType CommandType { get; set; }
}

public enum CommandType
{
    Add,
    Edit,
    Delete
}