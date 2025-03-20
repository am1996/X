using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace X.Services;
public interface IJwtGenerator
{
    string GenerateJwtToken(User user,string role);
}

public class JWTGenerator(IConfiguration configuration) : IJwtGenerator
{
    private readonly IConfiguration _configuration= configuration;
    public string GenerateJwtToken(User user, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] keyBytes = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"] 
            ?? throw new Exception("Secret Key not found"));
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Role, role),
            ]),
            Expires = DateTime.UtcNow.AddDays(90),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}