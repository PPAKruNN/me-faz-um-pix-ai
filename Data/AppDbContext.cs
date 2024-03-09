using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() {}
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Temporary hardcoded connection string. (I was facing a bug, needed to do this to continue progressing).
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=fazumpix");
    }

    public DbSet<User> User { get; set; }
    public DbSet<PixKey> PixKey { get; set; } 
    public DbSet<PaymentProvider> PaymentProvider { get; set; } 
    public DbSet<PaymentProviderAccount> PaymentProviderAccount { get; set; } 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<PaymentProviderAccount>().HasKey(p => p.Id);
        builder.Entity<PixKey>().HasKey(p => p.Id);
        builder.Entity<PaymentProvider>().HasKey(p => p.Id);

        builder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.Entity<PaymentProviderAccount>()
            .HasMany(a => a.PixKeys)
            .WithOne(p => p.Account)
            .HasForeignKey(p => p.PaymentProviderAccountId);
        
        builder.Entity<PaymentProvider>()
            .HasMany(p => p.Accounts)
            .WithOne(a => a.Bank)
            .HasForeignKey(a => a.PaymentProviderId);
    }
}

