namespace FiberVault.Application.DTO;

public sealed class CreateCableRequest
{
    public string Name { get; set; } = null!;
    public Guid CableTypeId { get; set; }

    public Guid FromNodeId { get; set; }
    public Guid ToNodeId { get; set; }

    public List<LngLatDto> Path { get; set; } = new();
}