namespace MillionAPI.Domain.Entities;

public class PropertyTrace
{
    public int Id { get; private set; }
    public int PropertyId { get; private set; }
    public DateTime DateSale { get; private set; }
    public string Name { get; private set; }
    public decimal Value { get; private set; }
    public decimal Tax { get; private set; }
    
    // Navegación
    public Property Property { get; private set; }

    private PropertyTrace() { } // Para EF Core

    public PropertyTrace(int propertyId, DateTime dateSale, string name, decimal value, decimal tax)
    {
        PropertyId = propertyId;
        DateSale = dateSale;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value;
        Tax = tax;
    }
}
