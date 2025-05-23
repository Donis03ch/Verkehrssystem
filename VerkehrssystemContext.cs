using Microsoft.EntityFrameworkCore;

public class VerkehrssystemContext : DbContext
{
    public DbSet<Kunde> Kunden { get; set; }
    public DbSet<Abonnement> Abonnements { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public VerkehrssystemContext(DbContextOptions<VerkehrssystemContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kunde>()
            .HasIndex(k => k.Email)
            .IsUnique();

        modelBuilder.Entity<Abonnement>()
            .HasOne(a => a.Kunde)
            .WithMany(k => k.Abonnements)
            .HasForeignKey(a => a.KundeId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Kunde)
            .WithMany(k => k.Tickets)
            .HasForeignKey(t => t.KundeId);
    }
}