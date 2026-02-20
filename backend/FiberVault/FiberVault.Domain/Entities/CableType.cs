namespace FiberVault.Domain.Entities;

public sealed class CableType
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public int FiberCount { get; private set; }

    private CableType() { }

    public CableType(string name, int fiberCount)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (fiberCount <= 0) throw new ArgumentOutOfRangeException(nameof(fiberCount), "FiberCount must be > 0.");

        Id = Guid.NewGuid();
        Name = name.Trim();
        FiberCount = fiberCount;
    }
}