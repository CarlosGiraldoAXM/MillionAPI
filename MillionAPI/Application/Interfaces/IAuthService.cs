using MillionAPI.Application.DTOs;

namespace MillionAPI.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Autentica un usuario y genera un token JWT
    /// </summary>
    /// <param name="loginRequest">Datos de login del usuario</param>
    /// <returns>Token JWT y información del usuario</returns>
    /// <exception cref="UnauthorizedAccessException">Si las credenciales son inválidas</exception>
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
}
