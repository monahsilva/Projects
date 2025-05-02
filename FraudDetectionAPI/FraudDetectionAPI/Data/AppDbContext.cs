using Microsoft.EntityFrameworkCore;
using FraudDetectionAPI.Models;

namespace FraudDetectionAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .ToTable("Transactions")
                .HasKey(t => t.Id);
        }
    }
}
