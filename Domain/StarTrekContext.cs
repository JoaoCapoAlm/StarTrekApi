﻿using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class StarTrekContext : DbContext
    {
        public StarTrekContext(DbContextOptions<StarTrekContext> opts) : base(opts)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasKey(x => x.CharacterId)
                .HasName("PK_Character");

            modelBuilder.Entity<Crew>()
                .HasKey(c => c.CrewId)
                .HasName("PK_Crew");

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

            modelBuilder.Entity<Place>()
                .HasKey(x => x.PlaceId)
                .HasName("PK_Place");

            modelBuilder.Entity<PlaceType>()
                .HasKey(x => x.PlaceTypeId)
                .HasName("PK_PlaceType");

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId)
                .HasName("PK_Roles");

            modelBuilder.Entity<Season>()
                .HasKey(s => s.SeasonId)
                .HasName("PK_Season");

            modelBuilder.Entity<Serie>()
                .HasKey(s => s.SerieId)
                .HasName("PK_Serie");

            modelBuilder.Entity<Species>()
                .HasKey(x => x.SpeciesId)
                .HasName("PK_Species");

            modelBuilder.Entity<Timeline>()
                .HasKey(t => t.TimelineId)
                .HasName("PK_Timeline");

            modelBuilder.Entity<Quadrant>()
                .HasKey(x => x.QuadrantId)
                .HasName("PK_Quadrant");

            modelBuilder.Entity<vwImdb>()
                .ToView("vwImdb")
                .HasNoKey();

            modelBuilder.Entity<vwResourcesPlaces>()
                .ToView("vwResourcesPlaces")
                .HasNoKey();

            modelBuilder.Entity<vwResourcesTitleSynopsis>()
                .ToView("vwResourcesTitleSynopsis")
                .HasNoKey();

            modelBuilder.Entity<Character>()
                .HasOne(x => x.Species)
                .WithMany(x => x.Characters)
                .HasForeignKey(x => x.SpeciesId)
                .HasConstraintName("FK_Character_Species");

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

            modelBuilder.Entity<Place>()
                .HasOne(x => x.PlaceType)
                .WithMany(x => x.Places)
                .HasForeignKey(x => x.PlaceTypeId)
                .HasConstraintName("FK_Place_PlaceType");

            modelBuilder.Entity<Place>()
                .HasOne(x => x.Quadrant)
                .WithMany(x => x.Places)
                .HasForeignKey(x => x.QuadrantId)
                .HasConstraintName("FK_Place_Quadrant");

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

            modelBuilder.Entity<Species>()
                .HasOne(x => x.Planet)
                .WithMany(x => x.Species)
                .HasForeignKey(x => x.PlanetId)
                .HasConstraintName("FK_Species_Planet");
        }

        public DbSet<Character> Character { get; set; }
        public DbSet<Crew> Crew { get; set; }
        public DbSet<CrewRole> CrewRole { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<PlaceType> PlaceType { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Serie> Serie { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Timeline> Timeline { get; set; }
        public DbSet<Quadrant> Quadrant { get; set; }
        public DbSet<vwImdb> vwImdb { get; set; }
        public DbSet<vwResourcesPlaces> vwResourcesPlaces { get; set; }
        public DbSet<vwResourcesTitleSynopsis> vwResourcesTitleSynopsis { get; set; }
    }
}
