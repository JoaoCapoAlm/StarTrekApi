using Application.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Application.Data
{
    public class StarTrekContext : DbContext
    {
        public StarTrekContext(DbContextOptions<StarTrekContext> opts) : base(opts) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crew>()
                .HasKey(c => c.CrewId)
                .HasName("FK_Crew_CountryId");

            modelBuilder.Entity<CrewRole>()
                .HasKey(c => c.CrewRoleId)
                .HasName("PK_CrewRole");

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

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId)
                .HasName("PK_Roles");

            modelBuilder.Entity<Season>()
                .HasKey(s => s.SeasonId)
                .HasName("PK_Season");

            modelBuilder.Entity<Serie>()
                .HasKey(s => s.SerieId)
                .HasName("PK_Serie");

            modelBuilder.Entity<Timeline>()
                .HasKey(t => t.TimelineId)
                .HasName("PK_Timeline");

            modelBuilder.Entity<vwResourcesName>()
                .HasNoKey();

            modelBuilder.Entity<Crew>()
                .HasOne(c => c.Country)
                .WithMany(country => country.Crews)
                .HasForeignKey(c => c.CountryId)
                .HasConstraintName("FK_Crew_CountryId");

            modelBuilder.Entity<CrewRole>()
                .HasOne(c => c.Crew)
                .WithMany(crew => crew.CrewRoles)
                .HasForeignKey(c => c.CrewId)
                .HasConstraintName("FK_CrewRole_Crew");

            modelBuilder.Entity<CrewRole>()
                .HasOne(c => c.Role)
                .WithMany(r => r.CrewRoles)
                .HasForeignKey(c => c.RoleId)
                .HasConstraintName("FK_CrewRole_Role");

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

        public DbSet<Crew> Crew { get; set; }
        public DbSet<CrewRole> CrewRole { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
        public DbSet<vwResourcesName> vwResourcesName { get; set; }
    }
}
