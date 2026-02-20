using FiberVault.Application.DTO;
using FiberVault.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiberVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FibersController : ControllerBase
{
    private readonly FiberVaultDbContext _db;

    public FibersController(FiberVaultDbContext db)
    {
        _db = db;
    }

    [HttpGet("by-cable/{cableId:guid}")]
    [Produces("application/json")]
    public async Task<ActionResult<IEnumerable<FiberResponse>>> GetByCable(Guid cableId)
    {
        var exists = await _db.Cables.AnyAsync(c => c.Id == cableId);
        if (!exists) return NotFound("Cable not found.");

        var fibers = await _db.Fibers
            .Where(f => f.CableId == cableId)
            .OrderBy(f => f.Number)
            .Select(f => new FiberResponse
            {
                Id = f.Id,
                CableId = f.CableId,
                Number = f.Number,
                Color = f.Color
            })
            .ToListAsync();

        return Ok(fibers);
    }
}