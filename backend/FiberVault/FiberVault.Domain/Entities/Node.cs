using NetTopologySuite.Geometries;

namespace FiberVault.Domain.Entities;

public sealed class Node
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public Point Location { get; private set; } = null!;

    private Node() { }

    public Node(string name, Point location)
    {
        Id = Guid.NewGuid();
        Name = name;
        Location = location;
    }

    public void Rename(string name)
    {
        Name = name;
    }
}