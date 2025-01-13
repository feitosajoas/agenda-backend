using agenda.Models.Generic;

namespace agenda.Models;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
