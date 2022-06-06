using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

        protected readonly IConfiguration Configuration;

        public MovieContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Database"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(u => u.UserName)
                    .UseCollation("NOCASE");
        }
    }
}
