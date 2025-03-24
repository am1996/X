using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace X.Services;

public interface IJwtGenerator
{
    string GenerateJwtToken(User user, string role);
}

public class JWTGenerator(IConfiguration configuration) : IJwtGenerator
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateJwtToken(User user, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // ✅ Store the encoded key once and reuse it
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["JwtSettings:Secret"] ?? throw new Exception("Secret Key not found")
        ));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ✅ Corrected User ID claim
                new Claim(ClaimTypes.Name, user.UserName!), // ✅ Added username
                new Claim(ClaimTypes.Role, role),
            }),
            Expires = DateTime.UtcNow.AddDays(90), // ✅ Expiration set to 90 days
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}
