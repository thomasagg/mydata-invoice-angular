using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username).IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalNet).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalVat).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalGross).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceLine>()
            .Property(l => l.UnitPrice).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceLine>()
            .Property(l => l.NetAmount).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceLine>()
            .Property(l => l.VatAmount).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceLine>()
            .Property(l => l.VatRate).HasColumnType("decimal(5,2)");
    }
}
