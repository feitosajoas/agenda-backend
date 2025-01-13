using agenda.Application.Queries;
using agenda.Infrastructure.Interfaces;
using agenda.Models;
using MediatR;

namespace agenda.Application.Handlers
{
    public class ContactQueryHandler :
    IRequestHandler<GetContactByIdQuery, Contact?>,
    IRequestHandler<GetAllContactsQuery, List<Contact>>
    {
        private readonly IRepositoryService<Contact> _repository;

        public ContactQueryHandler(IRepositoryService<Contact> repository)
        {
            _repository = repository;
        }

        public async Task<List<Contact>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _repository.GetByExpressionAsync(
                c => c.OwnerContactId == request.OwnerContactId,
                cancellationToken
            );

            return contacts;
        }

        public async Task<Contact?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetByExpressionAsync(
                c => c.Id == request.Id && c.OwnerContactId == request.OwnerContactId,
                cancellationToken
            );

            return contact?.FirstOrDefault();
        }
    }

}
