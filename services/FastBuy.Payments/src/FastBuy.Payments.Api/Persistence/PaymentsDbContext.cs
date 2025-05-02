using FastBuy.Payments.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastBuy.Payments.Api.Persistence
{
    public class PaymentsDbContext : DbContext
    {
        public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
        {

        }

        public DbSet<Order> Order { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.OrderId);
        }
    }
}
