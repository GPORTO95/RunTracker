namespace Infrastructure.Data.Models;

internal sealed class UserReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool HasPublicProfile { get; set; }
}
