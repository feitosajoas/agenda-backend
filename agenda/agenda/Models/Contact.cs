using agenda.Models.Generic;

namespace agenda.Models;

public class Contact : BaseEntity
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Guid OwnerContactId { get; set; }
}
