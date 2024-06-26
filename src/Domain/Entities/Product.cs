#nullable disable

namespace Domain.Entities;

public record Product
    : BaseEntity
{
    public string Name { get; set; }
}
