using Microsoft.EntityFrameworkCore;
using MinhaApi.Models.Entities;

namespace MinhaApi.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>()
                .Property(p => p.Id)
                .UseIdentityByDefaultColumn();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}
