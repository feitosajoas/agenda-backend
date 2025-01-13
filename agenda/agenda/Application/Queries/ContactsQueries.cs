using agenda.Models;
using MediatR;

namespace agenda.Application.Queries;

public class GetContactByIdQuery : IRequest<Contact?>
{
    public Guid Id { get; set; }
    public Guid OwnerContactId { get; set; }
}

public class GetAllContactsQuery : IRequest<List<Contact>>
{
    public Guid OwnerContactId { get; set; }
}
