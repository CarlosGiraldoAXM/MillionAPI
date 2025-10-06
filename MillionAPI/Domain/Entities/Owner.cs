namespace MillionAPI.Domain.Entities;

public class Owner
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public byte[]? Photo { get; private set; }
    public DateOnly? Birthday { get; private set; }
    
    // Navegación
    public ICollection<Property> Properties { get; private set; } = new List<Property>();

    private Owner() { } // Para EF Core

    public Owner(string name, string address, byte[]? photo = null, DateOnly? birthday = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        Photo = photo;
        Birthday = birthday;
    }

    public void UpdateName(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void UpdateAddress(string address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }

    public void UpdatePhoto(byte[]? photo)
    {
        Photo = photo;
    }

    public void UpdateBirthday(DateOnly? birthday)
    {
        Birthday = birthday;
    }

    public void AddProperty(Property property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        Properties.Add(property);
    }
}
