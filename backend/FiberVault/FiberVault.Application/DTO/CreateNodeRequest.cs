namespace FiberVault.Application.DTO;

public sealed class CreateNodeRequest
{
    public string Name { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}