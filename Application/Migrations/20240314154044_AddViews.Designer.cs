﻿// <auto-generated />
using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Application.Migrations
{
    [DbContext(typeof(StarTrekContext))]
    [Migration("20240314154044_AddViews")]
    partial class AddViews
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Model.Character", b =>
                {
                    b.Property<int>("CharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CharacterId"));

                    b.Property<byte>("ClassificationId")
                        .HasColumnType("tinyint");

                    b.Property<DateOnly?>("DateBirth")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DeathDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("SpeciesId")
                        .HasColumnType("smallint");

                    b.HasKey("CharacterId")
                        .HasName("PK_Character");

                    b.HasIndex("ClassificationId");

                    b.HasIndex("SpeciesId");

                    b.ToTable("Character");
                });

            modelBuilder.Entity("Domain.Model.CharacterClassification", b =>
                {
                    b.Property<byte>("CharacterClassificationId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Classification")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CharacterClassificationId")
                        .HasName("PK_CharacterClassification");

                    b.ToTable("CharacterClassification");
                });

            modelBuilder.Entity("Domain.Model.Country", b =>
                {
                    b.Property<short>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("CountryId"));

                    b.Property<string>("ResourceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId")
                        .HasName("PK_Country");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("Domain.Model.Crew", b =>
                {
                    b.Property<int>("CrewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CrewId"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<short?>("CountryId")
                        .HasColumnType("smallint");

                    b.Property<DateOnly?>("DeathDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CrewId")
                        .HasName("PK_Crew");

                    b.HasIndex("CountryId");

                    b.ToTable("Crew");
                });

            modelBuilder.Entity("Domain.Model.CrewRole", b =>
                {
                    b.Property<int>("CrewRoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CrewRoleId"));

                    b.Property<int>("CrewId")
                        .HasColumnType("int");

                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.HasKey("CrewRoleId")
                        .HasName("PK_CrewRole");

                    b.HasIndex("CrewId");

                    b.HasIndex("RoleId");

                    b.ToTable("CrewRole");
                });

            modelBuilder.Entity("Domain.Model.Episode", b =>
                {
                    b.Property<int>("EpisodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EpisodeId"));

                    b.Property<string>("ImdbId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<DateOnly?>("RealeaseDate")
                        .HasColumnType("date");

                    b.Property<short>("SeasonId")
                        .HasColumnType("smallint");

                    b.Property<float?>("StardateFrom")
                        .HasColumnType("real");

                    b.Property<float?>("StardateTo")
                        .HasColumnType("real");

                    b.Property<string>("SynopsisResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte?>("Time")
                        .HasColumnType("tinyint");

                    b.Property<string>("TitleResource")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EpisodeId")
                        .HasName("PK_Episode");

                    b.HasIndex("SeasonId");

                    b.ToTable("Episode");
                });

            modelBuilder.Entity("Domain.Model.Language", b =>
                {
                    b.Property<short>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("LanguageId"));

                    b.Property<string>("CodeISO")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResourceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LanguageId")
                        .HasName("FK_Language");

                    b.ToTable("Language");
                });

            modelBuilder.Entity("Domain.Model.Movie", b =>
                {
                    b.Property<short>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("MovieId"));

                    b.Property<DateTime?>("DateSyncTmdb")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImdbId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("OriginalLanguageId")
                        .HasColumnType("smallint");

                    b.Property<string>("OriginalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("SynopsisResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Time")
                        .HasColumnType("smallint");

                    b.Property<byte>("TimelineId")
                        .HasColumnType("tinyint");

                    b.Property<string>("TitleResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TmdbId")
                        .HasColumnType("int");

                    b.HasKey("MovieId")
                        .HasName("PK_Movie");

                    b.HasIndex("OriginalLanguageId");

                    b.HasIndex("TimelineId");

                    b.ToTable("Movie");
                });

            modelBuilder.Entity("Domain.Model.Place", b =>
                {
                    b.Property<short>("PlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("PlaceId"));

                    b.Property<string>("NameResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("PlaceTypeId")
                        .HasColumnType("tinyint");

                    b.Property<byte>("QuadrantId")
                        .HasColumnType("tinyint");

                    b.HasKey("PlaceId")
                        .HasName("PK_Place");

                    b.HasIndex("PlaceTypeId");

                    b.HasIndex("QuadrantId");

                    b.ToTable("Place");
                });

            modelBuilder.Entity("Domain.Model.PlaceType", b =>
                {
                    b.Property<byte>("PlaceTypeId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlaceTypeId")
                        .HasName("PK_PlaceType");

                    b.ToTable("PlaceType");
                });

            modelBuilder.Entity("Domain.Model.Quadrant", b =>
                {
                    b.Property<byte>("QuadrantId")
                        .HasColumnType("tinyint");

                    b.Property<string>("QuadrantResource")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuadrantId")
                        .HasName("PK_Quadrant");

                    b.ToTable("Quadrant");
                });

            modelBuilder.Entity("Domain.Model.Role", b =>
                {
                    b.Property<byte>("RoleId")
                        .HasColumnType("tinyint");

                    b.Property<string>("RoleResource")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId")
                        .HasName("PK_Roles");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Domain.Model.Season", b =>
                {
                    b.Property<short>("SeasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("SeasonId"));

                    b.Property<byte>("Number")
                        .HasColumnType("tinyint");

                    b.Property<short>("SerieId")
                        .HasColumnType("smallint");

                    b.HasKey("SeasonId")
                        .HasName("PK_Season");

                    b.HasIndex("SerieId");

                    b.ToTable("Season");
                });

            modelBuilder.Entity("Domain.Model.Serie", b =>
                {
                    b.Property<short>("SerieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("SerieId"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateSyncTmdb")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImdbId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("OriginalLanguageId")
                        .HasColumnType("smallint");

                    b.Property<string>("OriginalName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SynopsisResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("TimelineId")
                        .HasColumnType("tinyint");

                    b.Property<string>("TitleResource")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TmdbId")
                        .HasColumnType("int");

                    b.HasKey("SerieId")
                        .HasName("PK_Serie");

                    b.HasIndex("OriginalLanguageId");

                    b.HasIndex("TimelineId");

                    b.ToTable("Serie");
                });

            modelBuilder.Entity("Domain.Model.Species", b =>
                {
                    b.Property<short>("SpeciesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("SpeciesId"));

                    b.Property<short>("PlanetId")
                        .HasColumnType("smallint");

                    b.Property<string>("ResourceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SpeciesId")
                        .HasName("PK_Species");

                    b.HasIndex("PlanetId");

                    b.ToTable("Species");
                });

            modelBuilder.Entity("Domain.Model.Timeline", b =>
                {
                    b.Property<byte>("TimelineId")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TimelineId")
                        .HasName("PK_Timeline");

                    b.ToTable("Timeline");
                });

            modelBuilder.Entity("Domain.Model.vwImdb", b =>
                {
                    b.Property<string>("ImdbId")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("vwImdb", (string)null);
                });

            modelBuilder.Entity("Domain.Model.vwResourcesPlaces", b =>
                {
                    b.Property<string>("Resource")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("vwResourcesPlaces", (string)null);
                });

            modelBuilder.Entity("Domain.Model.vwResourcesTitleSynopsis", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Resource")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("vwResourcesTitleSynopsis", (string)null);
                });

            modelBuilder.Entity("Domain.Model.Character", b =>
                {
                    b.HasOne("Domain.Model.CharacterClassification", "CharacterClassification")
                        .WithMany("Characters")
                        .HasForeignKey("ClassificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Character_Classification");

                    b.HasOne("Domain.Model.Species", "Species")
                        .WithMany("Characters")
                        .HasForeignKey("SpeciesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Character_Species");

                    b.Navigation("CharacterClassification");

                    b.Navigation("Species");
                });

            modelBuilder.Entity("Domain.Model.Crew", b =>
                {
                    b.HasOne("Domain.Model.Country", "Country")
                        .WithMany("Crews")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("FK_Crew_CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Domain.Model.CrewRole", b =>
                {
                    b.HasOne("Domain.Model.Crew", "Crew")
                        .WithMany("CrewRoles")
                        .HasForeignKey("CrewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CrewRole_Crew");

                    b.HasOne("Domain.Model.Role", "Role")
                        .WithMany("CrewRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CrewRole_Role");

                    b.Navigation("Crew");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Model.Episode", b =>
                {
                    b.HasOne("Domain.Model.Season", "Season")
                        .WithMany("Episodes")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Episode_Season");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("Domain.Model.Movie", b =>
                {
                    b.HasOne("Domain.Model.Language", "Languages")
                        .WithMany("Movies")
                        .HasForeignKey("OriginalLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Movie_LanguageId");

                    b.HasOne("Domain.Model.Timeline", "Timeline")
                        .WithMany("Movies")
                        .HasForeignKey("TimelineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Movie_TimelineId");

                    b.Navigation("Languages");

                    b.Navigation("Timeline");
                });

            modelBuilder.Entity("Domain.Model.Place", b =>
                {
                    b.HasOne("Domain.Model.PlaceType", "PlaceType")
                        .WithMany("Places")
                        .HasForeignKey("PlaceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Place_PlaceType");

                    b.HasOne("Domain.Model.Quadrant", "Quadrant")
                        .WithMany("Places")
                        .HasForeignKey("QuadrantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Place_Quadrant");

                    b.Navigation("PlaceType");

                    b.Navigation("Quadrant");
                });

            modelBuilder.Entity("Domain.Model.Season", b =>
                {
                    b.HasOne("Domain.Model.Serie", "Serie")
                        .WithMany("Seasons")
                        .HasForeignKey("SerieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Season_Serie");

                    b.Navigation("Serie");
                });

            modelBuilder.Entity("Domain.Model.Serie", b =>
                {
                    b.HasOne("Domain.Model.Language", "Language")
                        .WithMany("Series")
                        .HasForeignKey("OriginalLanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Serie_LanguageId");

                    b.HasOne("Domain.Model.Timeline", "Timeline")
                        .WithMany("Series")
                        .HasForeignKey("TimelineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Serie_TimelineId");

                    b.Navigation("Language");

                    b.Navigation("Timeline");
                });

            modelBuilder.Entity("Domain.Model.Species", b =>
                {
                    b.HasOne("Domain.Model.Place", "Planet")
                        .WithMany("Species")
                        .HasForeignKey("PlanetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Species_Planet");

                    b.Navigation("Planet");
                });

            modelBuilder.Entity("Domain.Model.CharacterClassification", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("Domain.Model.Country", b =>
                {
                    b.Navigation("Crews");
                });

            modelBuilder.Entity("Domain.Model.Crew", b =>
                {
                    b.Navigation("CrewRoles");
                });

            modelBuilder.Entity("Domain.Model.Language", b =>
                {
                    b.Navigation("Movies");

                    b.Navigation("Series");
                });

            modelBuilder.Entity("Domain.Model.Place", b =>
                {
                    b.Navigation("Species");
                });

            modelBuilder.Entity("Domain.Model.PlaceType", b =>
                {
                    b.Navigation("Places");
                });

            modelBuilder.Entity("Domain.Model.Quadrant", b =>
                {
                    b.Navigation("Places");
                });

            modelBuilder.Entity("Domain.Model.Role", b =>
                {
                    b.Navigation("CrewRoles");
                });

            modelBuilder.Entity("Domain.Model.Season", b =>
                {
                    b.Navigation("Episodes");
                });

            modelBuilder.Entity("Domain.Model.Serie", b =>
                {
                    b.Navigation("Seasons");
                });

            modelBuilder.Entity("Domain.Model.Species", b =>
                {
                    b.Navigation("Characters");
                });

            modelBuilder.Entity("Domain.Model.Timeline", b =>
                {
                    b.Navigation("Movies");

                    b.Navigation("Series");
                });
#pragma warning restore 612, 618
        }
    }
}
