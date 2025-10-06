using Microsoft.AspNetCore.Http;

namespace MillionAPI.Application.DTOs;

public class OwnerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public byte[]? Photo { get; set; }
    public DateOnly? Birthday { get; set; }
    public int PropertiesCount { get; set; }
}

public class CreateOwnerDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public IFormFile? Photo { get; set; }
    public DateOnly? Birthday { get; set; }
}

