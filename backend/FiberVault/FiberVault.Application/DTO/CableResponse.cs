namespace FiberVault.Application.DTO;

public sealed class CableResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid FromNodeId { get; set; }
    public Guid ToNodeId { get; set; }
    public Guid CableTypeId { get; set; }

    public List<LngLatDto> Path { get; set; } = new();
}