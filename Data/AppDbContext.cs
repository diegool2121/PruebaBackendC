using Microsoft.EntityFrameworkCore;
using PruebaBackend.Models; 

namespace PruebaBackend.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MarcaAuto> MarcasAutos { get; set; }
    }
}
