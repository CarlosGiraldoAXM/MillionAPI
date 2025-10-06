using Microsoft.AspNetCore.Http;

namespace MillionAPI.Application.Services;

public interface IFileService
{
    Task<byte[]?> ProcessImageAsync(IFormFile? file);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<byte[]?> ProcessImageAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;

        // Validar tamaño
        if (file.Length > MaxFileSize)
            throw new ArgumentException($"El archivo excede el tamaño máximo de {MaxFileSize / (1024 * 1024)}MB");

        // Validar extensión
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new ArgumentException($"Tipo de archivo no permitido. Extensiones permitidas: {string.Join(", ", _allowedExtensions)}");

        // Leer el archivo como byte array
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }


}
