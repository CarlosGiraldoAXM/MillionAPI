namespace MillionAPI.Application.DTOs;

public class PropertyImageDto
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public byte[] File { get; set; } = Array.Empty<byte>();
    public bool Enabled { get; set; }
}

public class CreatePropertyImageDto
{
    public int PropertyId { get; set; }
    public byte[] File { get; set; } = Array.Empty<byte>();
    public bool Enabled { get; set; } = true;
}

public class UpdatePropertyImageDto
{
    public byte[] File { get; set; } = Array.Empty<byte>();
    public bool Enabled { get; set; }
}
