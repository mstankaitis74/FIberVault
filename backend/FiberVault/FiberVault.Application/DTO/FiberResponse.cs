namespace FiberVault.Application.DTO;

public sealed class FiberResponse
{
    public Guid Id { get; set; }
    public Guid CableId { get; set; }
    public int Number { get; set; }
    public string Color { get; set; } = null!;
}