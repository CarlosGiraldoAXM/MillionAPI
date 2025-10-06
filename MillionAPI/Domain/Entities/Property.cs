namespace MillionAPI.Domain.Entities;

public class Property
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public decimal Price { get; private set; }
    public string CodeInternal { get; private set; }
    public int? Year { get; private set; }
    public int OwnerId { get; private set; }
    
    // Navegación
    public Owner Owner { get; private set; }
    public ICollection<PropertyImage> Images { get; private set; } = new List<PropertyImage>();
    public ICollection<PropertyTrace> Traces { get; private set; } = new List<PropertyTrace>();

    private Property() { } // Para EF Core

    public Property(string name, string address, decimal price, string codeInternal, int ownerId, int? year = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Price = price;
        CodeInternal = codeInternal ?? throw new ArgumentNullException(nameof(codeInternal));
        OwnerId = ownerId;
        Year = year;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

}
