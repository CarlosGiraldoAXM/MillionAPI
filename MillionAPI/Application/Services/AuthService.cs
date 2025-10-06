using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MillionAPI.Application.DTOs;
using MillionAPI.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MillionAPI.Application.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly TestUser _testUser;

    public AuthService(IOptions<JwtSettings> jwtSettings, IOptions<TestUser> testUser)
    {
        _jwtSettings = jwtSettings.Value;
        _testUser = testUser.Value;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        // Simular validación de credenciales (en un caso real, esto vendría de una base de datos)
        if (loginRequest.Username != _testUser.Username || loginRequest.Password != _testUser.Password)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Generar token JWT
        var token = GenerateJwtToken(loginRequest.Username, _testUser.Email);

        return new LoginResponseDto
        {
            Token = token,
            Username = loginRequest.Username,
            Email = _testUser.Email,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        };
    }


    private string GenerateJwtToken(string username, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim("username", username),
            new Claim("email", email)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
