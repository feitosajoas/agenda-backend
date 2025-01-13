using agenda.Application.Commands;
using agenda.Application.Validators;
using agenda.Common.Utils;
using agenda.Infrastructure.Interfaces;
using agenda.Infrastructure.Persistence;
using agenda.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace agenda.Application.Handlers;

public class ContactCommandHandle : IRequestHandler<ContactCommand, Unit>
{
    private readonly IRepositoryService<Contact> _repository;
    private readonly ContactValidator _validator;

    public ContactCommandHandle(IRepositoryService<Contact> repository)
    {
        _repository = repository;
        _validator = new ContactValidator();
    }

    public async Task<Unit> Handle(ContactCommand request, CancellationToken cancellationToken)
    {
        var contact = new Contact
        {
            Id = request.Id ?? Guid.NewGuid(),
            Name = request.Name!,
            PhoneNumber = request.PhoneNumber!,
            OwnerContactId = request.OwnerContactId,
            Email = request.Email!,
        };

        if(request.CommandType != CommandType.Delete)
        {
            var validationResult = _validator.Validate(contact);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
            
        switch (request.CommandType)
        {
            case CommandType.Add:
                contact.CreatedAt = DateTime.UtcNow;
                contact.UpdatedAt = DateTime.UtcNow;
                await _repository.AddAsync(contact, cancellationToken);
                break;

            case CommandType.Edit:
                var existingContact = await _repository.GetByIdAsync(request.Id!.Value, cancellationToken);
                if (existingContact == null) throw new Exception(Messages.CONTACT_NOT_FOUND);

                existingContact.Name = request.Name!;
                existingContact.PhoneNumber = request.PhoneNumber!;
                existingContact.Email = request.Email!;
                existingContact.UpdatedAt = DateTime.UtcNow;
                await _repository.UpdateAsync(existingContact, cancellationToken);
                break;

            case CommandType.Delete:
                var contactToDelete = await _repository.GetByIdAsync(request.Id!.Value, cancellationToken);
                if (contactToDelete == null) throw new Exception(Messages.CONTACT_NOT_FOUND);

                await _repository.DeleteAsync(contactToDelete, cancellationToken);
                break;

            default:
                throw new InvalidOperationException(Messages.UNSUPPORTED_COMMAND_TYPE);
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}

