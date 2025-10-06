namespace MillionAPI.Domain.Entities;

public class PropertyImage
{
    public int Id { get; private set; }
    public int PropertyId { get; private set; }
    public byte[] File { get; private set; }
    public bool Enabled { get; private set; }
    
    // Navegación
    public Property Property { get; private set; }

    private PropertyImage() { } // Para EF Core

    public PropertyImage(int propertyId, byte[] file, bool enabled = true)
    {
        PropertyId = propertyId;
        File = file ?? throw new ArgumentNullException(nameof(file));
        Enabled = enabled;
    }

}
