using MediatR;

namespace agenda.Application.Queries;

public class LoginQuery : IRequest<string?>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginQuery(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
