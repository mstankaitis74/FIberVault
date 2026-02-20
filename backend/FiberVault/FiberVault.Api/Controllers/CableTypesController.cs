using FiberVault.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiberVault.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CableTypesController : ControllerBase
{
    private readonly FiberVaultDbContext _db;

    public CableTypesController(FiberVaultDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.CableTypes
            .OrderBy(t => t.FiberCount)
            .ThenBy(t => t.Name)
            .Select(t => new
            {
                t.Id,
                t.Name,
                t.FiberCount
            })
            .ToListAsync();

        return Ok(items);
    }
}