using Microsoft.EntityFrameworkCore;
using SIRESU.Models;
using System.Linq;

namespace SIRESU.Data
{
    public class SiresuContext : DbContext
    {
        public SiresuContext(DbContextOptions<SiresuContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evita que EF Core genere nvarchar(max) por defecto en strings
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                property.SetColumnType("varchar(255)");
            }
        }
    }
}
