using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using MovieCollection.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Repositories
{
    public class MovieContext : DbContext
    {
        public DbSet<MovieModel> Movies { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;


        public MovieContext(DbContextOptions options)
            :base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(u => u.UserName)
                    .UseCollation("NOCASE");
        }
    }
}
