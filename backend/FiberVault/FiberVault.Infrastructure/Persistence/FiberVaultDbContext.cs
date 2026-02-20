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
    public DbSet<Cable> Cables => Set<Cable>();
    public DbSet<CableType> CableTypes => Set<CableType>();
    public DbSet<Fiber> Fibers => Set<Fiber>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.Location).HasColumnType("geometry(Point,4326)");
            entity.HasIndex(x => x.Location).HasMethod("GIST");
        });

        modelBuilder.Entity<Cable>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(x => x.Path)
                  .IsRequired()
                  .HasColumnType("geometry(LineString,4326)");

            entity.HasIndex(x => x.Path).HasMethod("GIST");

            entity.HasOne<Node>()
                  .WithMany()
                  .HasForeignKey(x => x.FromNodeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Node>()
                  .WithMany()
                  .HasForeignKey(x => x.ToNodeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(x => x.CableTypeId).IsRequired();

            entity.HasOne<CableType>()
                  .WithMany()
                  .HasForeignKey(x => x.CableTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CableType>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).IsRequired().HasMaxLength(200);
            entity.Property(x => x.FiberCount).IsRequired();

            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Fiber>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Number).IsRequired();
            entity.Property(x => x.Color).IsRequired().HasMaxLength(30);

            entity.HasIndex(x => new { x.CableId, x.Number }).IsUnique();

            entity.HasOne<Cable>()
                  .WithMany()
                  .HasForeignKey(x => x.CableId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}