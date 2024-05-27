using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace ExpensesManager.Services;
public static class JwtGenerator
{
    public static string GenerateUserToken(string username, IConfiguration config)
    {
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, username),
        };

        return GenerateUserToken(claims, DateTime.UtcNow.AddHours(12), config);
    }

    private static string GenerateUserToken(Claim[] claims, DateTime expires, IConfiguration config)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string? secret = config.GetSection("Secrets")["JWT_SECRET"];
        string? issuer = config.GetSection("Secrets")["JWT_ISSUER"];
        byte[] key = Encoding.ASCII.GetBytes(secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
