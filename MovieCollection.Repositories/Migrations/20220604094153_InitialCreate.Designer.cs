﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieCollection.Repositories;

#nullable disable

namespace MovieCollection.Repositories.Migrations
{
    [DbContext(typeof(MovieContext))]
    [Migration("20220604094153_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("MovieCollection.Domain.Movie", b =>
                {
                    b.Property<string>("MovieDatabaseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MovieDatabaseId");

                    b.ToTable("Movies");
                });
#pragma warning restore 612, 618
        }
    }
}