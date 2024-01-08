using Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public class StarTrekContext : DbContext
    {
        public StarTrekContext(DbContextOptions<StarTrekContext> opts) : base(opts) {
        }

        public DbSet<Cast> Cast { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
    }
}
