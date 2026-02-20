using FiberVault.Application.DTO;
using FiberVault.Domain.Entities;
using FiberVault.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace FiberVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NodesController : ControllerBase
{
    private readonly FiberVaultDbContext _db;

    public NodesController(FiberVaultDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [Produces("application/json")]
    public async Task<ActionResult<NodeResponse>> Create(CreateNodeRequest request)
    {
        var point = new Point(request.Longitude, request.Latitude)
        {
            SRID = 4326
        };

        var node = new Node(request.Name, point);

        _db.Nodes.Add(node);
        await _db.SaveChangesAsync();

        var response = new NodeResponse
        {
            Id = node.Id,
            Name = node.Name,
            Latitude = node.Location.Y,
            Longitude = node.Location.X
        };

        return CreatedAtAction(nameof(GetAll), response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NodeResponse>>> GetAll()
    {
        var nodes = await _db.Nodes.ToListAsync();

        var result = nodes.Select(n => new NodeResponse
        {
            Id = n.Id,
            Name = n.Name,
            Latitude = n.Location.Y,
            Longitude = n.Location.X
        });

        return Ok(result);
    }

    [HttpGet("bbox")]
    public async Task<ActionResult<IEnumerable<NodeResponse>>> GetByBbox(
    [FromQuery] double minLat,
    [FromQuery] double minLng,
    [FromQuery] double maxLat,
    [FromQuery] double maxLng)
    {
        if (minLat < -90 || maxLat > 90 || minLng < -180 || maxLng > 180 || minLat > maxLat || minLng > maxLng)
            return BadRequest("Invalid bbox.");

        var gf = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var env = new Envelope(minLng, maxLng, minLat, maxLat); // X=lng, Y=lat
        var bbox = gf.ToGeometry(env);

        var nodes = await _db.Nodes
            .Where(n => bbox.Intersects(n.Location))
            .Select(n => new NodeResponse
            {
                Id = n.Id,
                Name = n.Name,
                Latitude = n.Location.Y,
                Longitude = n.Location.X
            })
            .ToListAsync();

        return Ok(nodes);
    }
}