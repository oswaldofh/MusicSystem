﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicSystem.Data;

#nullable disable

namespace MusicSystem.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240404003058_AddTableSong")]
    partial class AddTableSong
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MusicSystem.Data.Entities.AlbumSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AlbumSets");
                });

            modelBuilder.Entity("MusicSystem.Data.Entities.SongSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AlbumSetId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AlbumSetId");

                    b.HasIndex("Name", "AlbumSetId")
                        .IsUnique();

                    b.ToTable("SongSets");
                });

            modelBuilder.Entity("MusicSystem.Data.Entities.SongSet", b =>
                {
                    b.HasOne("MusicSystem.Data.Entities.AlbumSet", "AlbumSet")
                        .WithMany("SongSets")
                        .HasForeignKey("AlbumSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AlbumSet");
                });

            modelBuilder.Entity("MusicSystem.Data.Entities.AlbumSet", b =>
                {
                    b.Navigation("SongSets");
                });
#pragma warning restore 612, 618
        }
    }
}