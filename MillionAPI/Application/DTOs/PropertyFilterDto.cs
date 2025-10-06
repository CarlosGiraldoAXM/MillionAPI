namespace MillionAPI.Application.DTOs;

public class PropertyFilterDto
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? CodeInternal { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Year { get; set; }
    public int? IdOwner { get; set; }
}
