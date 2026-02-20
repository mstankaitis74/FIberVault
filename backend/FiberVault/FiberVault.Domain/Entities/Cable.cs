using NetTopologySuite.Geometries;

namespace FiberVault.Domain.Entities;

public sealed class Cable
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;

    public Guid FromNodeId { get; private set; }
    public Guid ToNodeId { get; private set; }
    public Guid CableTypeId { get; private set; }

    public LineString Path { get; private set; } = null!;

    private Cable() { }

    public Cable(string name, Guid fromNodeId, Guid toNodeId, Guid cableTypeId, LineString path)
    {
        Id = Guid.NewGuid();
        Name = name;
        FromNodeId = fromNodeId;
        ToNodeId = toNodeId;
        CableTypeId = cableTypeId;
        Path = path;
    }

    public void Rename(string name) => Name = name;

    public void UpdatePath(LineString path) => Path = path;
}