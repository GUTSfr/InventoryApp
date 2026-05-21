using InventoryApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryField> InventoryFields => Set<InventoryField>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<CustomIdFormat> CustomIdFormats => Set<CustomIdFormat>();
    public DbSet<CustomIdElement> CustomIdElements => Set<CustomIdElement>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tag> Tags => Set<Tag>();   
    public DbSet<InventoryTag> InventoryTags => Set<InventoryTag>();
    public DbSet<InventoryAccess> InventoryAccesses => Set<InventoryAccess>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("public");

        builder.Entity<InventoryTag>()
            .HasKey(it => new { it.InventoryId, it.TagId });

        builder.Entity<InventoryAccess>()
            .HasKey(ia => new { ia.InventoryId, ia.UserId });

        builder.Entity<Like>()
            .HasKey(l => new { l.ItemId, l.UserId });

        builder.Entity<Item>()
            .HasIndex(i => new { i.InventoryId, i.CustomId })
            .IsUnique();

        builder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        builder.Entity<InventoryField>()
            .HasIndex(f => new { f.InventoryId, f.Type, f.SlotIndex })
            .IsUnique();

        builder.Entity<Item>()
            .HasOne(i => i.Inventory)
            .WithMany(inv => inv.Items)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(u => u.Comments)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Item>()
            .HasOne(i => i.CreatedBy)
            .WithMany(u => u.CreatedItems)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Like>()
            .HasOne(l => l.Item)
            .WithMany(i => i.Likes)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<InventoryAccess>()
            .HasOne(ia => ia.User)
            .WithMany(u => u.GrantedAccesses)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Item>()
            .Property(i => i.Number1)
            .HasPrecision(18, 4);

        builder.Entity<Item>()
            .Property(i => i.Number2)
            .HasPrecision(18, 4);

        builder.Entity<Item>()
            .Property(i => i.Number3)
            .HasPrecision(18, 4);
    }
}