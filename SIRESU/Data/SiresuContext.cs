using Microsoft.EntityFrameworkCore;
using SIRESU.Models;

namespace SIRESU.Data
{
    public class SiresuContext : DbContext
    {
        public SiresuContext(DbContextOptions<SiresuContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }


    }

}