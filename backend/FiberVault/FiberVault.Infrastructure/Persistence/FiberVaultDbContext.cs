using FiberVault.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiberVault.Infrastructure.Persistence;

public sealed class FiberVaultDbContext : DbContext
{
    public FiberVaultDbContext(DbContextOptions<FiberVaultDbContext> options)
        : base(options)
    {
    }

    public DbSet<Node> Nodes => Set<Node>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Location)
                  .HasColumnType("geometry(Point,4326)");
        });

        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Location).HasColumnType("geometry(Point,4326)");

            entity.HasIndex(x => x.Location).HasMethod("GIST");
        });
    }
}