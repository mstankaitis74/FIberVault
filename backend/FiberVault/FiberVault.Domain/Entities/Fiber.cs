namespace FiberVault.Domain.Entities;

public sealed class Fiber
{
    public Guid Id { get; private set; }
    public Guid CableId { get; private set; }
    public int Number { get; private set; }        // 1..N
    public string Color { get; private set; } = null!;

    private Fiber() { }

    public Fiber(Guid cableId, int number, string color)
    {
        if (cableId == Guid.Empty) throw new ArgumentException("CableId is required.", nameof(cableId));
        if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number), "Number must be > 0.");
        if (string.IsNullOrWhiteSpace(color)) throw new ArgumentException("Color is required.", nameof(color));

        Id = Guid.NewGuid();
        CableId = cableId;
        Number = number;
        Color = color.Trim();
    }
}