using Application.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Data
{
    public class StarTrekContext : DbContext
    {
        public StarTrekContext(DbContextOptions<StarTrekContext> opts) : base(opts) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cast>()
                .HasKey(c => new { c.CastId, c.CountryId });

            modelBuilder.Entity<Cast>()
                .HasOne(c => c.Country)
                .WithMany(country => country.Casts)
                .HasForeignKey(c => c.CountryId);

            modelBuilder.Entity<Movie>()
                .HasKey(m => new { m.MovieId, m.OriginalLanguageId });

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Languages)
                .WithMany(l => l.Movies)
                .HasForeignKey(m => m.OriginalLanguageId);

        }

        public DbSet<Cast> Cast { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
    }
}
