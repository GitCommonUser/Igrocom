﻿// <auto-generated />
using System;
using Igrocom.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Igrocom.Migrations
{
    [DbContext(typeof(IgrocomContext))]
    [Migration("20241008133944_UserUpdate5")]
    partial class UserUpdate5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Igrocom.Models.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("ImageMime")
                        .HasColumnType("text");

                    b.Property<string>("Preface")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Content");
                });

            modelBuilder.Entity("Igrocom.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("ImageMime")
                        .HasColumnType("text");

                    b.Property<string>("Peculiarities")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("Rating")
                        .HasColumnType("smallint");

                    b.Property<DateOnly>("ReleaseDate")
                        .HasColumnType("date");

                    b.Property<string>("Review")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("Igrocom.Models.GameFiles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("FileMime")
                        .HasColumnType("text");

                    b.Property<string>("FileName")
                        .HasColumnType("text");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GameFiles");
                });

            modelBuilder.Entity("Igrocom.Models.MediaFiles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

                    b.Property<byte[]>("File")
                        .HasColumnType("bytea");

                    b.Property<string>("FileMime")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.ToTable("MediaFiles");
                });

            modelBuilder.Entity("Igrocom.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Igrocom.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Igrocom.Models.UserContent", b =>
                {
                    b.Property<int>("ContentId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("ContentId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserContent");
                });

            modelBuilder.Entity("Igrocom.Models.UserGame", b =>
                {
                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("GameId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGame");
                });

            modelBuilder.Entity("Igrocom.Models.GameFiles", b =>
                {
                    b.HasOne("Igrocom.Models.Game", null)
                        .WithMany("GameFiles")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Igrocom.Models.MediaFiles", b =>
                {
                    b.HasOne("Igrocom.Models.Content", null)
                        .WithMany("MediaFiles")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Igrocom.Models.Rating", b =>
                {
                    b.HasOne("Igrocom.Models.Game", null)
                        .WithMany("Ratings")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Igrocom.Models.UserContent", b =>
                {
                    b.HasOne("Igrocom.Models.Content", "Content")
                        .WithMany("UserContent")
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Igrocom.Models.User", "User")
                        .WithMany("UserContent")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Content");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Igrocom.Models.UserGame", b =>
                {
                    b.HasOne("Igrocom.Models.Game", "Game")
                        .WithMany("UserGame")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Igrocom.Models.User", "User")
                        .WithMany("UserGame")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Igrocom.Models.Content", b =>
                {
                    b.Navigation("MediaFiles");

                    b.Navigation("UserContent");
                });

            modelBuilder.Entity("Igrocom.Models.Game", b =>
                {
                    b.Navigation("GameFiles");

                    b.Navigation("Ratings");

                    b.Navigation("UserGame");
                });

            modelBuilder.Entity("Igrocom.Models.User", b =>
                {
                    b.Navigation("UserContent");

                    b.Navigation("UserGame");
                });
#pragma warning restore 612, 618
        }
    }
}
