namespace MillionAPI.Application.DTOs;

public class PropertyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public int ImagesCount { get; set; }
}

public class CreatePropertyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int? Year { get; set; }
    public int OwnerId { get; set; }
    public List<IFormFile>? Images { get; set; } = new List<IFormFile>();
}

public class UpdatePropertyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int? Year { get; set; }
}

public class UpdatePropertyPriceDto
{
    public decimal Price { get; set; }
}
