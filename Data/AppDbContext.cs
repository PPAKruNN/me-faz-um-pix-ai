using FazUmPix.Models;
using Microsoft.EntityFrameworkCore;

namespace FazUmPix.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<PixKey> PixKey { get; set; }
    public DbSet<PaymentProvider> PaymentProvider { get; set; }
    public DbSet<PaymentProviderAccount> PaymentProviderAccount { get; set; }
    public DbSet<Payment> Payment { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<PaymentProviderAccount>().HasKey(p => p.Id);
        builder.Entity<PixKey>().HasKey(p => p.Id);
        builder.Entity<PaymentProvider>().HasKey(p => p.Id);
        builder.Entity<Payment>().HasKey(p => p.Id);

        builder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.Entity<PaymentProviderAccount>()
            .HasMany(a => a.PixKeys)
            .WithOne(p => p.Account)
            .HasForeignKey(p => p.PaymentProviderAccountId);


        builder.Entity<Payment>()
            .HasOne(p => p.DestinationPaymentProviderAccount)
            .WithMany(a => a.DestinationPayments)
            .HasForeignKey(p => p.DestinationPaymentProviderAccountId);

        builder.Entity<Payment>()
            .HasOne(p => p.OriginPaymentProviderAccount)
            .WithMany(a => a.OriginPayments)
            .HasForeignKey(p => p.OriginPaymentProviderAccountId);

        builder.Entity<Payment>()
            .HasOne(p => p.PixKey)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.PixKeyId);

        builder.Entity<PaymentProvider>()
            .HasMany(p => p.Accounts)
            .WithOne(a => a.PaymentProvider)
            .HasForeignKey(a => a.PaymentProviderId);

        builder.Entity<PixKey>()
                .HasIndex(p => p.Value)
                .HasDatabaseName("IX_PixKey_Value");

        builder.Entity<PaymentProvider>()
                .HasIndex(p => p.Token)
                .HasDatabaseName("IX_PaymentProvider_Token");

        builder.Entity<User>()
            .HasIndex(u => u.CPF)
            .HasDatabaseName("IX_User_CPF");

    }
    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }


    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var now = DateTime.UtcNow;

        foreach (var entity in entities)
        {

            if (entity.State == EntityState.Added)
            {
                ((BaseModel)entity.Entity).CreatedAt = now;
            }

            ((BaseModel)entity.Entity).UpdatedAt = now;
        }
    }
}

