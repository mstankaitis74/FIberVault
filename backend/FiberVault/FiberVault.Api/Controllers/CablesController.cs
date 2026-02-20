using FiberVault.Application.DTO;
using FiberVault.Domain.Entities;
using FiberVault.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace FiberVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CablesController : ControllerBase
{
    private readonly FiberVaultDbContext _db;

    private static readonly string[] FiberColors12 =
    {
        "Blue", "Orange", "Green", "Brown", "Slate", "White",
        "Red", "Black", "Yellow", "Violet", "Rose", "Aqua"
    };

    public CablesController(FiberVaultDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<CableResponse>> Create([FromBody] CreateCableRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        if (request.Path == null || request.Path.Count < 2)
            return BadRequest("Path must contain at least 2 points.");

        // validate nodes exist
        var fromExists = await _db.Nodes.AnyAsync(n => n.Id == request.FromNodeId);
        var toExists = await _db.Nodes.AnyAsync(n => n.Id == request.ToNodeId);

        var cableType = await _db.CableTypes.FirstOrDefaultAsync(t => t.Id == request.CableTypeId);
        if (cableType == null)
            return BadRequest("CableTypeId not found.");

        if (!fromExists) return BadRequest("FromNodeId not found.");
        if (!toExists) return BadRequest("ToNodeId not found.");

        // Build LineString from lng/lat points
        var coords = request.Path
            .Select(p => new Coordinate(p.Longitude, p.Latitude))
            .ToArray();

        // Optional: reject duplicates / invalid coords
        if (coords.Any(c => c.Y < -90 || c.Y > 90 || c.X < -180 || c.X > 180))
            return BadRequest("Invalid coordinates in path.");

        var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var line = gf.CreateLineString(coords);

        var cable = new Cable(request.Name.Trim(), request.FromNodeId, request.ToNodeId, request.CableTypeId, line);

        _db.Cables.Add(cable);
        await _db.SaveChangesAsync();

        for (int i = 1; i <= cableType.FiberCount; i++)
        {
            var color = FiberColors12[(i - 1) % FiberColors12.Length];
            _db.Fibers.Add(new Fiber(cable.Id, i, color));
        }

        await _db.SaveChangesAsync();

        return Ok(ToResponse(cable));
    }

    [HttpGet("bbox")]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<CableResponse>>> GetByBbox(
        [FromQuery] double minLat,
        [FromQuery] double minLng,
        [FromQuery] double maxLat,
        [FromQuery] double maxLng)
    {
        if (minLat < -90 || maxLat > 90 || minLng < -180 || maxLng > 180 || minLat > maxLat || minLng > maxLng)
            return BadRequest("Invalid bbox.");

        var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var env = new Envelope(minLng, maxLng, minLat, maxLat);
        var bbox = gf.ToGeometry(env);

        var cables = await _db.Cables
            .Where(c => bbox.Intersects(c.Path))
            .ToListAsync();

        return Ok(cables.Select(ToResponse));
    }

    private static CableResponse ToResponse(Cable c)
    {
        return new CableResponse
        {
            Id = c.Id,
            Name = c.Name,
            FromNodeId = c.FromNodeId,
            ToNodeId = c.ToNodeId,
            Path = c.Path.Coordinates
                .Select(co => new LngLatDto { Longitude = co.X, Latitude = co.Y })
                .ToList()
        };
    }
}