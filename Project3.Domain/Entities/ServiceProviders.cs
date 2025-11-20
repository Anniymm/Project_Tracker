namespace Project3.Domain.Entities;

public class ServiceProviders
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Specialty { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ServiceProviders() { }

    public ServiceProviders(
        Guid id,
        string name,
        string email,
        string specialty,
        bool isActive,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        Email = email;
        Specialty = specialty;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public void Update(string? name, string? email, string? specialty, bool? isActive)
    {
        Name = name ?? Name;
        Email = email ?? Email;
        Specialty = specialty ?? Specialty;
        IsActive = isActive ?? IsActive;
    }
}