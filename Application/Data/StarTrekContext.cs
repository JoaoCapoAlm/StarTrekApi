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
                .HasKey(c => c.CastId)
                .HasName("FK_Cast_CountryId");

            modelBuilder.Entity<Country>()
                .HasKey(c => c.CountryId)
                .HasName("PK_Country");

            modelBuilder.Entity<Episode>()
                .HasKey(e => e.EpisodeId)
                .HasName("PK_Episode");

            modelBuilder.Entity<Language>()
                .HasKey(l => l.LanguageId)
                .HasName("FK_Language");

            modelBuilder.Entity<Movie>()
                .HasKey(m => m.MovieId)
                .HasName("PK_Movie");

            modelBuilder.Entity<Season>()
                .HasKey(s => s.SeasonId)
                .HasName("PK_Season");

            modelBuilder.Entity<Serie>()
                .HasKey(s => s.SerieId)
                .HasName("PK_Serie");

            modelBuilder.Entity<Timeline>()
                .HasKey(t => t.TimelineId)
                .HasName("PK_Timeline");

            modelBuilder.Entity<Cast>()
                .HasOne(c => c.Country)
                .WithMany(country => country.Casts)
                .HasForeignKey(c => c.CountryId)
                .HasConstraintName("FK_Cast_CountryId");

            modelBuilder.Entity<Episode>()
                .HasOne(e => e.Season)
                .WithMany(s => s.Episodes)
                .HasForeignKey(e => e.SeasonId)
                .HasConstraintName("FK_Episode_Season");

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Languages)
                .WithMany(l => l.Movies)
                .HasForeignKey(m => m.OriginalLanguageId)
                .HasConstraintName("FK_Movie_LanguageId");

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Timeline)
                .WithMany(t => t.Movies)
                .HasForeignKey(m => m.TimelineId)
                .HasConstraintName("FK_Movie_TimelineId");

            modelBuilder.Entity<Season>()
                .HasOne(s => s.Serie)
                .WithMany(s => s.Seasons)
                .HasForeignKey(s => s.SerieId)
                .HasConstraintName("FK_Season_Serie");

            modelBuilder.Entity<Serie>()
                .HasOne(s => s.Timeline)
                .WithMany(t => t.Series)
                .HasForeignKey(s => s.TimelineId)
                .HasConstraintName("FK_Serie_TimelineId");

            modelBuilder.Entity<Serie>()
                .HasOne(s => s.Language)
                .WithMany(l => l.Series)
                .HasForeignKey(s => s.OriginalLanguageId)
                .HasConstraintName("FK_Serie_LanguageId");
        }

        public DbSet<Cast> Cast { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
    }
}
