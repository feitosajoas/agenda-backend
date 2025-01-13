using System.Security.Claims;
using agenda.Models;

namespace agenda.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
    string RefreshToken(string token);
    User DecodeToken(string token);
}
