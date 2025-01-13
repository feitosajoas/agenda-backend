using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using agenda.Common.Interfaces;
using agenda.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace agenda.Common.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly byte[] key;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
        key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value.ToString());
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("sub", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(Int32.Parse(_configuration.GetSection("JwtSettings:Expiration").Value)), // Valor em horas
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string RefreshToken(string token)
    {

        var user = DecodeToken(token);

        return GenerateToken(user);

    }

    public User DecodeToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecutiryToken = securityToken as JwtSecurityToken;

            if (jwtSecutiryToken == null || !jwtSecutiryToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
                throw new SecurityTokenException("Token inválido");

            return new User { 
                Id = Guid.Parse(principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                Name = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value
            };
        }
        catch (Exception)
        {
            return null;
        }
    }
}
